using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Data.SQLite;

namespace OwLib
{
    /// <summary>
    /// 股票服务
    /// </summary>
    public class SecurityService
    {
        #region Lord 2015/11/14
        /// <summary>
        /// 代码表字典
        /// </summary>
        public static Dictionary<String, GSecurity> m_codedMap = new Dictionary<String, GSecurity>();

        /// <summary>
        /// 昨日数据
        /// </summary>
        private static Dictionary<String, List<SecurityData>> m_historyDatas = new Dictionary<String, List<SecurityData>>();

        /// <summary>
        /// 最新数据
        /// </summary>
        public static Dictionary<String, SecurityLatestData> m_latestDatas = new Dictionary<String, SecurityLatestData>();

        /// <summary>
        /// 最新数据字符串
        /// </summary>
        private static Dictionary<String, String> m_latestDatasStr = new Dictionary<String, String>();

        /// <summary>
        /// 分时数据
        /// </summary>
        private static Dictionary<String, List<SecurityData>> m_minuteDatas = new Dictionary<String, List<SecurityData>>();

        /// <summary>
        /// 每日新数据存放目录
        /// </summary>
        private static String m_newFileDir = "";

        /// <summary>
        /// 今日时间
        /// </summary>
        private static long m_today = 0;

        /// <summary>
        /// 根据时间下载新浪xls数据文件
        /// </summary>
        public static void DownLoadSinaXlsDataByDate(String dateStr)
        {
            String contentPath = DataCenter.GetAppPath() + "\\download\\xls\\";
            String urlTemplate = "http://market.finance.sina.com.cn/downxls.php?date={0}&symbol={1}";
            foreach(String code in m_codedMap.Keys)
            {
                String filePath = contentPath + code + "-" + dateStr + ".xls";
                String url = String.Format(urlTemplate, dateStr, code.ToLower());
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                Stream fs = new FileStream(filePath, FileMode.Create);
                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
            }
        }

        /// <summary>
        /// 返回所有合约
        /// </summary>
        /// <param name="codes">合约代码集合</param>
        public static void GetCodes(List<String> codes)
        { 
            foreach(String key in m_codedMap.Keys)
            {
                codes.Add(key);
            }
        }

        /// <summary>
        /// 通过代码返回合约信息
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="sd">out 股票基本信息</param>
        public static int GetSecurityByCode(String code, ref GSecurity security)
        {
            int ret = 0;
            foreach (String key in m_codedMap.Keys)
            {
                if (key == code)
                {
                    security = m_codedMap[key];
                    ret = 1;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取前一天下跌的股票
        /// </summary>
        public static List<String> GetLastDayCodes(int type)
        {
            List<String> ret = new List<String>();
            foreach (String code in m_historyDatas.Keys)
            {
                List<SecurityData> sds = m_historyDatas[code];
                SecurityData sd = sds[sds.Count -1];
                if(type == 0)
                {
                    if (sd.m_close < sd.m_open)
                    {
                        ret.Add(code);
                    }
                }
                else if (type == 1)
                {
                    if (sd.m_close > sd.m_open)
                    {
                        ret.Add(code);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取最新数据
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="latestData">最新数据</param>
        /// <returns>状态</returns>
        public static int GetLatestData(String code, ref SecurityLatestData latestData)
        {
            int state = 0;
            lock (m_latestDatas)
            {
                if (m_latestDatas.ContainsKey(code))
                {
                    latestData.Copy(m_latestDatas[code]);
                    state = 1;
                }
            }
            return state;
        }

        /// <summary>
        /// 获取最新数据
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="latestData">最新数据</param>
        /// <returns>状态</returns>
        public static String GetLatestData(String code)
        {
            lock (m_latestDatas)
            {
                if (m_latestDatasStr.ContainsKey(code))
                {
                    return m_latestDatasStr[code];
                }
            }
            return "";
        }

        /// <summary>
        /// 加载历史数据
        /// </summary>
        /// <param name="history"></param>
        public static void LoadHistoryDatas()
        {
            if (m_historyDatas.Count > 0)
            {
                return;
            }
            foreach(String code in m_codedMap.Keys)
            {
                String fileName = DataCenter.GetAppPath() + "\\day\\" + CStrA.ConvertDBCodeToSinaCode(code).ToUpper() + ".txt";
                if (File.Exists(fileName))
                {
                    StreamReader sra = new StreamReader(fileName, Encoding.Default);
                    String text = sra.ReadToEnd();
                    List<SecurityData> datas = new List<SecurityData>();
                    StockService.GetHistoryDatasByTdxStr(text, datas);
                    if (datas.Count > 0)
                    {
                        m_historyDatas[code] = datas;
                    }
                }
            }  
        }

        /// <summary>
        /// 获取最近活跃的代码
        /// </summary>
        /// <returns>活跃代码集合</returns>
        public static int GetActiveCodes(List<String> activeCodes)
        {
            LoadHistoryDatas();
            Dictionary<String, double> dic = new Dictionary<string, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                //非停牌
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    if (m_historyDatas.ContainsKey(latestData.m_code))
                    {
                        List<SecurityData> datas = m_historyDatas[latestData.m_code];
                        int datasSize = datas.Count;
                        if (datasSize > 250)
                        {
                            double totalVol = 0;
                            double latestVol = 0;
                            for (int i = 0; i < datasSize; i++)
                            {
                                totalVol += datas[i].m_volume;
                                if (i > datasSize - 20)
                                {
                                    latestVol += datas[i].m_volume;
                                }
                            }
                            if (totalVol > 0)
                            {
                                dic[latestData.m_code] = latestVol / totalVol;
                            }
                        }
                    }
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < 50; i++)
            {
                activeCodes.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 获取分时数据
        /// </summary>
        public static void GetMinuteDatas()
        {
            if(m_minuteDatas.Count > 0)
            {
                return;
            }
            String appPath = DataCenter.GetAppPath();
            foreach(String code in m_codedMap.Keys)
            {
                String fileName = m_newFileDir + CStrA.ConvertDBCodeToFileName(code);
                if (!CFileA.IsFileExist(fileName))
                {
                    fileName = m_newFileDir + CStrA.ConvertDBCodeToSinaCode(code).ToUpper() + ".txt";
                }
                if (CFileA.IsFileExist(fileName))
                {
                    String text = "";
                    CFileA.Read(fileName, ref text);
                    List<SecurityData> datas = new List<SecurityData>();
                    StockService.GetHistoryDatasByMinuteStr(text, datas);
                    if (datas.Count > 0)
                    {
                        int rindex = 0;
                        int dataSize = datas.Count;
                        while (rindex < dataSize)
                        {
                            SecurityData d = datas[rindex];
                            if (rindex == 0)
                            {
                                d.m_avgPrice = d.m_close;
                            }
                            else
                            {
                                SecurityData ld = datas[rindex -1];
                                d.m_avgPrice = (ld.m_avgPrice * rindex + d.m_close) / (rindex + 1);
                            }
                            rindex++;
                        }
                        m_minuteDatas[code] = datas;
                    }
                }
            }
        }

        /// <summary>
        /// 获取涨停股票合约集合
        /// </summary>
        /// <param name="limitUpCodes">out 涨停股票合约集合</param>
        public static int GetLimitUp(List<String> limitUpCodes)
        {
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                //非停牌
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    if (latestData.m_sellVolume1 == 0)
                    {
                        if (latestData.m_buyVolume1 == 0)
                        {
                            //无交易股票
                            continue;
                        }
                        limitUpCodes.Add(latestData.m_code);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取跌停合约集合
        /// </summary>
        /// <param name="limitDownCodes">out 跌停合约集合</param>
        public static int GetLimitDown(List<String> limitDownCodes)
        {
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                //非停牌
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    if (latestData.m_buyVolume1 == 0)
                    {
                        if (latestData.m_sellVolume1 == 0)
                        {
                            //无交易股票
                            continue;
                        }
                        limitDownCodes.Add(latestData.m_code);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取无交易合约集合
        /// </summary>
        /// <param name="notTradedCodes">out  无交易合约集合</param>
        public static int GetNotTradedCodes(List<String> notTradedCodes)
        {
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && latestData.m_buyVolume1 == 0 && latestData.m_sellVolume1 == 0)
                {
                    notTradedCodes.Add(latestData.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// 交易合约集合
        /// </summary>
        /// <param name="notTradedCodes">out  交易合约集合</param>
        public static int GetTradedCodes(List<String> tradedCodes)
        {
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    tradedCodes.Add(latestData.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// 次新股
        /// </summary>
        /// <param name="secondNewCodes"></param>
        /// <returns></returns>
        public static int GetSecondNewCodes(List<String> secondNewCodes)
        {
            LoadHistoryDatas();
            Dictionary<String, double> dic = new Dictionary<string, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                //非停牌
                if (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)
                {
                    if (m_historyDatas.ContainsKey(latestData.m_code))
                    {
                        List<SecurityData> datas = m_historyDatas[latestData.m_code];
                        int datasSize = datas.Count;
                        if (datasSize > 20 && datasSize < 60)
                        {
                            dic[latestData.m_code] = datasSize;
                        }
                    }
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < 50; i++)
            {
                secondNewCodes.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 获取ST股票
        /// </summary>
        /// <param name="stCodes">out ST股票</param>
        public static int GetSTCodes(List<String> stCodes)
        {
            foreach (GSecurity security in m_codedMap.Values)
            {
                if (security.m_name.IndexOf("ST") == 0)
                {
                    stCodes.Add(security.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取*ST股票
        /// </summary>
        /// <param name="stCodes">out 获取*ST股票</param>
        public static int GetStarSTCodes(List<String> starSTCodes)
        {
            foreach (GSecurity security in m_codedMap.Values)
            {
                if (security.m_name.IndexOf("*ST") == 0)
                {
                    starSTCodes.Add(security.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取涨幅排名
        /// </summary>
        /// <param name="riseRanking">out 涨幅排名</param>
        public static int GetRiseRanking(List<String> riseRanking)
        {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    double rank = (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    dic.Add(latestData.m_code, rank);
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count; i++)
            {
                riseRanking.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 获取跌幅排名
        /// </summary>
        /// <param name="riseRanking">out 跌幅排名</param>
        public static int GetFallRanking(ref List<String> fallRanking)
        {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    double rank = (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    dic.Add(latestData.m_code, rank);
                }
            }

            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count; i++)
            {
                fallRanking.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 根据成交量排名
        /// </summary>
        /// <param name="codesByVol">成交量排名</param>
        public static int GetCodesByVolume(List<String> codesByVol)
        {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    dic.Add(latestData.m_code, latestData.m_volume);
                }
            }

            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < 50; i++)
            {
                codesByVol.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 根据成交额排名
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static int GetCodesByAmount(List<String> codesByAmount)
        {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    dic.Add(latestData.m_code, latestData.m_amount);
                }
            }

            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < 50; i++)
            {
                codesByAmount.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 根据振幅排名
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static int GetCodesBySwing(List<String> codesBySwing)
        {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0))
                {
                    if (latestData.m_lastClose > 0)
                    {
                        dic.Add(latestData.m_code, (int)(latestData.m_high - latestData.m_low) / latestData.m_lastClose);
                    }
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < 50; i++)
            {
                codesBySwing.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 根据股价排名
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static int GetCodesByPrice(List<String> codesByPrice)
        {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values)
            {
                if (latestData.m_close > 0)
                {
                    dic.Add(latestData.m_code, latestData.m_close);
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < 50; i++)
            {
                codesByPrice.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// 导入股票
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="type">类型</param>
        public static void ImportSecurities(Dictionary<String, KwItem> kwItems)
        {
            String dataBasePath = DataCenter.GetAppPath() + "\\securities.db";
            if (CFileA.IsFileExist(dataBasePath))
            {
                CFileA.RemoveFile(dataBasePath);
            }
            String connectStr = "Data Source = " + dataBasePath;
            if (!CFileA.IsFileExist(dataBasePath))
            {
                //创建数据库文件
                SQLiteConnection.CreateFile("securities.db");
                //创建表
                SQLiteConnection conn2 = new SQLiteConnection(connectStr);
                conn2.Open();
                SQLiteCommand cmd2 = conn2.CreateCommand();
                cmd2.CommandText = "CREATE TABLE SECURITY(ID INTEGER PRIMARY KEY, CODE, NAME, PINGYIN, TYPE INTEGER, STATUS INTEGER, CREATETIME DATE, MODIFYTIME DATE)";
                cmd2.ExecuteNonQuery();
                conn2.Close();
            }
            SQLiteConnection conn = new SQLiteConnection(connectStr);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            int strSize = kwItems.Count;
            foreach(KwItem kwItem  in kwItems.Values)
            {
                String sql = String.Format("INSERT INTO SECURITY(CODE, NAME, PINGYIN, TYPE, STATUS, CREATETIME, MODIFYTIME) VALUES ('{0}', '{1}', '{2}', {3}, {4}, '1970-1-1', '1970-1-1')",
                    kwItem.Code, kwItem.Name, kwItem.Pingyin, kwItem.Type, kwItem.Innercode);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        /// <summary>
        /// 开始策略
        /// </summary>
        public static void Start()
        {
            Thread thread = new Thread(new ThreadStart(StartWork));
            thread.Start();
        }

        /// <summary>
        /// 启动任线程
        /// </summary>
        public static void Start2(int type, double min, double max)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(StartWork2));
            object[] param = new object[3];
            param[0] = type;
            param[1] = min;
            param[2] = max;
            thread.Start(param);
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        private static void StartWork()
        {
            //加载代码表//step 1
            String codes = "";
            if (m_codedMap.Count == 0)
            {
                Dictionary<String, KwItem> items = EMSecurityService.KwItems;
                foreach (KwItem item in items.Values)
                {
                    GSecurity security = new GSecurity();
                    security.m_code = item.Code;
                    security.m_name = item.Name;
                    security.m_pingyin = item.Pingyin;
                    security.m_type = item.Type;
                    if (!security.m_code.StartsWith("A"))
                    {
                        m_codedMap[security.m_code] = security;
                        codes += security.m_code;
                        codes += ",";
                    }
                }
            }
            while (true)
            {
                if (codes != null && codes.Length > 0)
                {
                    if (codes.EndsWith(","))
                    {
                        codes.Remove(codes.Length - 1);
                    }
                    String[] strCodes = codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    int codesSize = strCodes.Length;
                    String latestCodes = "";
                    for (int i = 0; i < codesSize; i++)
                    {
                        latestCodes += strCodes[i];
                        if (i == codesSize - 1 || (i > 0 && i % 50 == 0))
                        {
                            String latestDatasResult = StockService.GetSinaLatestDatasStrByCodes(latestCodes);
                            if (latestDatasResult != null && latestDatasResult.Length > 0)
                            {
                                List<SecurityLatestData> latestDatas = new List<SecurityLatestData>();
                                StockService.GetLatestDatasBySinaStr(latestDatasResult, 0, latestDatas);
                                String[] subStrs = latestDatasResult.Split(new String[] { ";\n" }, StringSplitOptions.RemoveEmptyEntries);
                                int latestDatasSize = latestDatas.Count;
                                for (int j = 0; j < latestDatasSize; j++)
                                {
                                    SecurityLatestData latestData = latestDatas[j];
                                    if (latestData.m_close == 0)
                                    {
                                        latestData.m_close = latestData.m_buyPrice1;
                                    }
                                    if (latestData.m_close == 0)
                                    {
                                        latestData.m_close = latestData.m_sellPrice1;
                                    }
                                    lock (m_latestDatas)
                                    {
                                        m_latestDatasStr[latestData.m_code] = subStrs[j];
                                        //bool append = true;
                                        //if (m_latestDatas.ContainsKey(latestData.m_code))
                                        //{
                                        //    if (!m_latestDatas[CStrA.ConvertFileCodeToMemoryCode(latestData.m_code)].Equals(latestData))
                                        //    {
                                        //        append = false;
                                        //    }
                                        //}
                                        //if(append)
                                        //{
                                        //    long today = (long)DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds / 86400000;
                                        //    if (m_today < today)
                                        //    {
                                        //        m_today = today;
                                        //        String nPath = DataCenter.GetAppPath() + "\\tick\\" + DateTime.Now.ToString("yyyy-MM-dd");
                                        //        if (!Directory.Exists(nPath))
                                        //        {
                                        //            Directory.CreateDirectory(nPath);
                                        //        }
                                        //        m_newFileDir = nPath + "\\";
                                        //    }
                                        //    String line = String.Format("{0},{1},{2},{3}\r\n", latestData.m_date,//
                                        //        latestData.m_close, latestData.m_volume, latestData.m_amount);
                                        //    CFileA.Append(m_newFileDir + latestData.m_code + ".txt", line);
                                        //}
                                        if (!m_latestDatas.ContainsKey(latestData.m_code))
                                        {
                                            m_latestDatas[latestData.m_code] = latestData;
                                        }
                                        else
                                        {
                                            m_latestDatas[latestData.m_code].Copy(latestData);
                                        }
                                    }
                                }
                                latestDatas.Clear();
                            }
                            latestCodes = "";
                        }
                        else
                        {
                            latestCodes += ",";
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 开始工作2
        /// </summary>
        /// <param name="param"></param>
        public static void StartWork2(object param)
        {
            object[] arr = (object[])param;
            int type =  (int)arr[0];
            double min = (double)arr[1];
            double max = (double)arr[2];
            LoadHistoryDatas();
            List<String> fallCodes = GetLastDayCodes(type);
            GetMinuteDatas();
            Dictionary<String, double> pMap = Step5(fallCodes);
            List<String> inCode = Step6(pMap, min, max);
            String dateStr = DateTime.Now.ToString("yyyyMMdd");
            StringBuilder result = new StringBuilder();
            foreach (String code in inCode)
            {
                result.Append(code + "," + pMap[code] + "\r\n");
            }
            String pathTemplate = DataCenter.GetAppPath() + "\\result\\result_{0}.txt";
            String outputPath = String.Format(pathTemplate, dateStr);
            CFileA.Append(outputPath, result.ToString());
        }

        /// <summary>
        /// 数据落地线程工作
        /// </summary>
        public static void StartWork3()
        {
            //复制数据
            LoadHistoryDatas();
            GetMinuteDatas();
            //新旧数据合并
            foreach(String oCode in m_historyDatas.Keys)
            {
                if (!m_latestDatas.ContainsKey(oCode) || !m_historyDatas.ContainsKey(oCode)) continue;
                SecurityLatestData securityLatestData = m_latestDatas[oCode];
                List<SecurityData> oldSecurityDatas = m_historyDatas[oCode];
                SecurityData oldSecurityData = oldSecurityDatas[oldSecurityDatas.Count -1];
                int myear = 0, mmonth = 0, mday = 0, mhour = 0, mmin = 0, msec = 0, mmsec = 0;
                CStrA.M130(oldSecurityData.m_date, ref myear, ref mmonth, ref mday, ref mhour, ref mmin, ref msec, ref mmsec);
                int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0, msec2 = 0;
                CStrA.M130(securityLatestData.m_date, ref year, ref month, ref day, ref hour, ref min, ref sec, ref msec2);
                if (year >= myear && month >= mmonth && day >= mday)
                {
                    SecurityData nSecurityData = new SecurityData();
                    nSecurityData.m_amount = securityLatestData.m_amount;
                    nSecurityData.m_close = securityLatestData.m_close;
                    nSecurityData.m_date = securityLatestData.m_date;
                    nSecurityData.m_high = securityLatestData.m_high;
                    nSecurityData.m_low = securityLatestData.m_low;
                    nSecurityData.m_open = securityLatestData.m_open;
                    nSecurityData.m_volume = securityLatestData.m_volume;
                    if (day == mday)
                    {
                        m_historyDatas[oCode].RemoveAt(m_historyDatas[oCode].Count - 1);
                    }
                    m_historyDatas[oCode].Add(nSecurityData);
                }
            }
            String outputFileTemplate = DataCenter.GetAppPath() + "\\day\\{0}.txt";
            String fileInfo = "{0} {1} 日线 前复权\r\n";
            String title = "      日期	    开盘	    最高	    最低	    收盘	    成交量	    成交额\r\n";
            String lineTemp = "{0},{1},{2},{3},{4},{5},{6}\r\n";
            String timeFormatStr = "yyyy-MM-dd";
            //写入文件
            foreach (String code in m_historyDatas.Keys)
            {
                List<SecurityData> temp3 = m_historyDatas[code];
                StringBuilder strbuff = new StringBuilder();
                strbuff.Append(String.Format(fileInfo, m_codedMap[code].m_code, m_codedMap[code].m_name));
                strbuff.Append(title);
                foreach (SecurityData sdt in temp3)
                {
                    strbuff.Append(String.Format(lineTemp,//
                        CStr.ConvertNumToDate(sdt.m_date).ToString(timeFormatStr),//
                        sdt.m_open,//
                        sdt.m_high,//
                        sdt.m_low,//
                        sdt.m_close,//
                        sdt.m_volume,//
                        sdt.m_amount));
                }
                strbuff.Append("数据来源:通达信\r\n");
                CFileA.Write(String.Format(outputFileTemplate, code), strbuff.ToString());
            }
        }

        /// <summary>
        /// 获取比例
        /// </summary>
        /// <param name="fallCodes"></param>
        /// <returns></returns>
        public static Dictionary<String, double> Step5(List<String> fallCodes)
        {
            int index = 0;
            int size = fallCodes.Count;
            Dictionary<String, double> ret = new Dictionary<String, double>();
            while (index < size)
            {
               if (m_minuteDatas.ContainsKey(fallCodes[index]))
               {
                   List<SecurityData> datas = m_minuteDatas[fallCodes[index]];
                   if(datas != null)
                   {
                       SecurityData m =  datas[datas.Count - 1];
                       double av = (m.m_close - m.m_avgPrice) / m.m_close;
                       ret[fallCodes[index]] = av;
                   }
               }
               index++;
            }
            return ret;
        }

        /// <summary>
        /// 比例返回获取股票
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<String> Step6(Dictionary<String, double> datas, double min, double max)
        {
            List<String> ret = new List<String>();
            foreach (String code in datas.Keys)
            {
                double value = datas[code];
                if (value >= min && value <= max)
                {
                    ret.Add(code);
                }
            }
            return ret;
        }
        #endregion
    }
}
