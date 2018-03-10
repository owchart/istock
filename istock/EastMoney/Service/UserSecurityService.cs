using System;
using System.Collections.Generic;
using System.Text;
using EmSocketClient;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace dataquery
{
    /// <summary>
    /// 码表状态排序
    /// </summary>
    public class KwItemCompareChange : IComparer<KwItem>
    {
        public int Compare(KwItem x, KwItem y)
        {
            if (x.State > y.State)
            {
                return -1;
            }
            else if (x.State < y.State)
            {
                return 1;
            }
            return 0;
        }
    }

    /// <summary>
    /// 码表类型排序
    /// </summary>
    public class KwItemCompareNum : IComparer<KwItem>
    {
        public int Compare(KwItem x, KwItem y)
        {
            if (x.Type > y.Type)
            {
                return -1;
            }
            else if (x.Type < y.Type)
            {
                return 1;
            }
            return 0;
        }
    }

    public class CommandBaseInfo
    {
        private int _requestId = 0;
        private String _userId = "";
        private String _serviceName = "";
        private String _syncType = "";
        private String _command = "";
        private String _uniqueId_N = "";
        private String _pi = "";
        private String _loginType = "";

        /// <summary>
        /// 操作编号，如果请求有回调，需要用该编号进行匹配
        /// </summary>
        public int RequestID
        {
            get { return _requestId; }
            set { _requestId = value; }
        }

        /// <summary>
        /// 用户编号 uniqueID
        /// </summary>
        public String UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 服务类型，例如StockWatchModule为数据浏览器模板。
        /// 路由服务使用
        /// </summary>
        public String ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        /// 同步类型，标识同步以
        /// 自己定义自己解析
        /// </summary>
        public String SyncType
        {
            get { return _syncType; }
            set { _syncType = value; }
        }

        /// <summary>
        /// 操作命令，每个服务自定义。字符串形式传递
        /// </summary>
        public String Command
        {
            get { return _command; }
            set { _command = value; }
        }

        /// <summary>
        /// 通行证 UniqueId_N
        /// </summary>
        public String UniqueId_N
        {
            get { return _uniqueId_N; }
            set { _uniqueId_N = value; }
        }

        /// <summary>
        /// 通行证 PI 登陆成功后有
        /// </summary>
        public String PI
        {
            get { return _pi; }
            set { _pi = value; }
        }

        /// <summary>
        /// 通行证 登陆类型
        /// </summary>
        public String LoginType
        {
            get { return _loginType; }
            set { _loginType = value; }
        }


        /// <summary>
        /// 字符串转换成实体类
        /// </summary>
        /// <param name="content"></param>
        public virtual void StringToClass(String content)
        {
            int index = 0;
            int length = 0;
            int startIndex = 0;

            index = content.IndexOf("\r\n");
            if (index < 0)
                return;

            length = index - startIndex;
            int.TryParse(content.Substring(0, length), out _requestId);

            startIndex = index + "\r\n".Length;
            index = content.IndexOf("\r\n", startIndex);
            length = index - startIndex;
            _userId = content.Substring(startIndex, length);

            startIndex = index + "\r\n".Length;
            index = content.IndexOf("\r\n", startIndex);
            length = index - startIndex;
            _serviceName = content.Substring(startIndex, length);

            startIndex = index + "\r\n".Length;
            index = content.IndexOf("\r\n", startIndex);
            length = index - startIndex;
            _syncType = content.Substring(startIndex, length);

            startIndex = index + "\r\n".Length;
            index = content.IndexOf("\r\n", startIndex);
            length = index - startIndex;
            _uniqueId_N = content.Substring(startIndex, length);

            startIndex = index + "\r\n".Length;
            index = content.IndexOf("\r\n", startIndex);
            length = index - startIndex;
            _pi = content.Substring(startIndex, length);

            startIndex = index + "\r\n".Length;
            index = content.IndexOf("\r\n", startIndex);
            length = index - startIndex;
            _loginType = content.Substring(startIndex, length);

            startIndex = index + "\r\n".Length;
            _command = content.Substring(startIndex);
        }

        /// <summary>
        /// 实体类转换成字符串
        /// </summary>
        /// <returns></returns>
        public virtual String ClassToString()
        {
            return String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}"
                , this._requestId
                , this._userId
                , this._serviceName
                , this._syncType
                , this._uniqueId_N
                , this._pi
                , this._loginType
                , this._command);
        }
    }

    public class StockInfo
    {
        public Dictionary<String, int> m_items = new Dictionary<String, int>();
    }

    /// <summary>
    /// 自选股服务
    /// </summary>
    public class UserSecurityService
    {
        private static EmSocketClient.EmSocketClient client;

        /// <summary>
        /// 计算
        /// </summary>
        public static void Calculate()
        {
            String reportDir = @"D:\owchart\quotenew\data\reports";
            if (!Directory.Exists(reportDir))
            {
                Directory.CreateDirectory(reportDir);
            }
            
            DirectoryInfo dirInfo = new DirectoryInfo(reportDir);
            FileInfo []oldReportFiles = dirInfo.GetFiles();
            int oldReportFilesSize = oldReportFiles.Length;
            for (int i = 0; i < oldReportFilesSize; i++)
            {
                File.Delete(oldReportFiles[i].FullName);
            }
            String dir = DataCenter.GetAppPath() + "\\securities\\";
            DirectoryInfo[] dirs = new DirectoryInfo(dir).GetDirectories();
            int dirsSize = dirs.Length;
            List<StockInfo> stockInfos = new List<StockInfo>();
            for (int i = 0; i < dirsSize; i++)
            {
                StockInfo newStockInfo = new StockInfo();
                stockInfos.Add(newStockInfo);
                FileInfo[] files = new DirectoryInfo(dirs[i].FullName).GetFiles();
                int filesSize = files.Length;
                Dictionary<String, int> items = new Dictionary<String, int>();
                for (int j = 0; j < filesSize; j++)
                {
                    String content = File.ReadAllText(files[j].FullName);
                    String[] strs = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    int strsSize = strs.Length;
                    int maxSize = 0;
                    for (int x = 0; x < strsSize && x < 20; x++)
                    {
                        String code = strs[x];
                        if (code.Length > 10)
                        {
                            String[] subStrs = code.Split(new String[] { ".SZ", ".SH" }, StringSplitOptions.RemoveEmptyEntries);
                            int subStrsSize = subStrs.Length;
                            for (int m = 0; m < subStrsSize; m++)
                            {
                                String subCode = subStrs[m];
                                if(subCode.StartsWith("6"))
                                {
                                    subCode += ".SH";
                                }
                                else
                                {
                                    subCode += ".SZ";
                                }
                                if (!items.ContainsKey(subCode))
                                {
                                    items[subCode] = 1;
                                }
                                else
                                {
                                    items[subCode] = items[subCode] + 1;
                                }
                            }
                        }
                        else
                        {
                            if (!items.ContainsKey(code))
                            {
                                items[code] = (int)(20 - x) / 2;
                            }
                            else
                            {
                                items[code] = items[code] + (int)(20 - x) / 2;
                            }
                        }
                    }
                }
                newStockInfo.m_items = items;
            }
            //生成报告
            for (int i = 0; i < dirsSize - 1; i++)
            {
                List<KwItem> kwItems = new List<KwItem>();
                StockInfo lastStockInfo = stockInfos[dirsSize - 1];
                StockInfo thisStockInfo = stockInfos[i];
                foreach (String key in thisStockInfo.m_items.Keys)
                {
                    int count = thisStockInfo.m_items[key];
                    KwItem item = new KwItem();
                    item.Code = key;
                    item.Type = count;
                    if (lastStockInfo.m_items.ContainsKey(key))
                    {
                        item.State = count - lastStockInfo.m_items[key];
                    }
                    else
                    {
                        item.State = count;
                    }
                    kwItems.Add(item);
                }
                kwItems.Sort(new KwItemCompareChange());
                String strResult = "";
                if (kwItems.Count > 0)
                {
                    for (int k = 0; k < kwItems.Count; k++)
                    {
                        KwItem kwItem = kwItems[k];
                        strResult += kwItem.Code + "," + kwItem.State.ToString();
                        if (k != kwItems.Count - 1)
                        {
                            strResult += "\r\n";
                        }
                    }
                    File.WriteAllText(reportDir + "\\" + (dirsSize - 1 - i).ToString() + "日变动.txt", strResult);
                }
            }
        }

        public static String DownLoad(String uniqueID)
        {
            try
            {
                try
                {
                    if (client == null)
                    {
                        client = new EmSocketClient.EmSocketClient("183.136.162.250", 1818);
                    }
                    else if (!client.IsConnected)
                    {
                        Thread.Sleep(1000000);
                        client = new EmSocketClient.EmSocketClient("183.136.162.250", 1818);
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
                CommandBaseInfo info = new CommandBaseInfo();
                info.RequestID = 1;
                info.UserID = uniqueID;
                info.ServiceName = "MyStockService";
                info.UniqueId_N = uniqueID;
                info.PI = "0";
                info.LoginType = "0";
                info.SyncType = "2";
                info.Command = "0.U$我的自选股$-100.U$0$$AStock$0$1}";
                String cmd = "10002_" + info.ClassToString();
                EmSocketClient.MsgHeader msgHeader = new EmSocketClient.MsgHeader();
                msgHeader.CommandCode = EmCommandType.CMD1;
                SocketMessageStatus message = client.Send(msgHeader, Encoding.UTF8.GetBytes(cmd));
                String result = Encoding.UTF8.GetString(message.ResponseBytes);
                CommandBaseInfo command = new CommandBaseInfo();
                command.StringToClass(result);
                return command.Command;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 开始服务
        /// </summary>
        public static void StartService()
        {
            DateTime date = DateTime.Now;
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return;
            }
            String idFile = DataCenter.GetAppPath() + "\\config\\userid.txt";
            String identifier = "Mongo Insert: ";
            if (!File.Exists(idFile))
            {
                Dictionary<String, String> dict = new Dictionary<String, String>();
                FileInfo[] files = new DirectoryInfo(DataCenter.GetAppPath() + "\\config\\log").GetFiles();
                if (files.Length > 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        String content = File.ReadAllText(files[i].FullName);
                        if (content.Length > 0)
                        {
                            String[] strs = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < strs.Length; j++)
                            {
                                String str = strs[j];
                                if (str.IndexOf(identifier) != -1)
                                {
                                    try
                                    {
                                        String userid = str.Substring(str.IndexOf(identifier) + identifier.Length);
                                        dict[userid] = "";
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                                    }
                                }
                            }
                        }
                    }
                    foreach (String key in dict.Keys)
                    {
                        File.AppendAllText(idFile, key + "\r\n");
                    }
                }
            }
            String usDir = DataCenter.GetAppPath() + "\\securities\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(usDir))
            {
                Directory.CreateDirectory(usDir);
            }
            if (File.Exists(idFile))
            {
                String content = File.ReadAllText(idFile);
                if (content.Length > 0)
                {
                    String[] strs = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    List<String> strList = new List<String>();
                    strList.AddRange(strs);
                    Random rd = new Random();
                    int maxCount = 10000;
                    int index = 0;
                    Dictionary<String, KwItem> allStocks = new Dictionary<String, KwItem>();
                    while (strList.Count > 0 && index < maxCount)
                    {
                        try
                        {
                            int num = rd.Next(10, 30);
                            for (int i = 0; i < num; i++)
                            {
                                if (strList.Count > 0)
                                {
                                    int idx = rd.Next(0, strList.Count);
                                    if (maxCount < strList.Count)
                                    {
                                        idx = 0;
                                    }
                                    String userid = strList[idx];
                                    String result = UserSecurityService.DownLoad(userid);
                                    if (result.Length > 0)
                                    {
                                        String[] subStrs = result.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                        if (subStrs.Length > 0)
                                        {
                                            List<String> stocks = new List<String>();
                                            for (int j = 0; j < subStrs.Length; j++)
                                            {
                                                String[] sunStrs = subStrs[j].Split('$');
                                                if (sunStrs.Length == 5)
                                                {
                                                    if (sunStrs[1].Length > 0)
                                                    {
                                                        String code = sunStrs[0];
                                                        if (code == "300059.SZ")
                                                        {
                                                            continue;
                                                        }
                                                        if ((code.EndsWith(".SH") && code.StartsWith("6"))
                                                            || (code.EndsWith(".SZ") && code.StartsWith("0")))
                                                        {
                                                            String name = sunStrs[1];
                                                            stocks.Add(code);
                                                            if (allStocks.ContainsKey(code))
                                                            {
                                                                allStocks[code].Type = allStocks[code].Type + 1;
                                                            }
                                                            else
                                                            {
                                                                KwItem item = new KwItem();
                                                                item.Code = code;
                                                                item.Name = name;
                                                                item.Type = 1;
                                                                allStocks[code] = item;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            String stockStr = "";
                                            if (stocks.Count > 0)
                                            {
                                                for (int j = 0; j < stocks.Count; j++)
                                                {
                                                    stockStr += stocks[j];
                                                    if (i != stocks.Count - 1)
                                                    {
                                                        stockStr += "\r\n";
                                                    }
                                                }
                                                File.WriteAllText(usDir + "\\" + userid + ".txt", stockStr);
                                            }
                                        }
                                    }
                                    strList.RemoveAt(idx);
                                    index++;
                                }
                            }
                            Thread.Sleep(1000 * num);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("1");
                        }
                    }
                    //读取上次结果
                    String lastResultFile = DataCenter.GetAppPath() + "\\securities\\codes_sortnum.txt";
                    Dictionary<String, int> lastStockNums = new Dictionary<String, int>();
                    if (File.Exists(lastResultFile))
                    {
                        String lContent = File.ReadAllText(lastResultFile);
                        if (lContent.Length > 0)
                        {
                            String[] lStrs = lContent.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            if (lStrs != null && lStrs.Length > 0)
                            {
                                for (int i = 0; i < lStrs.Length; i++)
                                {
                                    String[] sublStrs = lStrs[i].Split(',');
                                    lastStockNums[sublStrs[0]] = Convert.ToInt32(sublStrs[2]);
                                }
                            }
                        }
                    }
                    //计算变动
                    foreach (String code in allStocks.Keys)
                    {
                        KwItem item = allStocks[code];
                        int num = item.Type;
                        int lastNum = 0;
                        if (lastStockNums.ContainsKey(code))
                        {
                            lastNum = lastStockNums[code];
                        }
                        item.State = num - lastNum;
                    }
                    //保存到集合
                    List<KwItem> items = new List<KwItem>();
                    foreach (KwItem item in allStocks.Values)
                    {
                        items.Add(item);
                    }
                    items.Sort(new KwItemCompareNum());
                    //按数量排序生成结果
                    String strResult = "";
                    if (items.Count > 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            KwItem item = items[i];
                            strResult += item.Code + "," + item.Name + "," + item.Type.ToString() + "," + item.State;
                            if (i != items.Count - 1)
                            {
                                strResult += "\r\n";
                            }
                        }
                        File.WriteAllText(lastResultFile, strResult);
                    }
                    //按变动排序生成结果
                    String changeFile = DataCenter.GetAppPath() + "\\securities\\codes_sortchange.txt";
                    items.Sort(new KwItemCompareChange());
                    strResult = "";
                    if (items.Count > 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            KwItem item = items[i];
                            strResult += item.Code + "," + item.Name + "," + item.Type.ToString() + "," + item.State;
                            if (i != items.Count - 1)
                            {
                                strResult += "\r\n";
                            }
                        }
                        File.WriteAllText(changeFile, strResult);
                        File.WriteAllText("D:\\owchart\\quotenew\\data\\securities\\report.txt", strResult);
                    }
                }
            }
            Calculate();
        }
    }
}
