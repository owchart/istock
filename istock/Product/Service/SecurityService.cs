using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using OwLibSV;
using node.gs;

namespace OwLib
{
    /// <summary>
    /// ��Ʊ����
    /// </summary>
    public class SecurityService
    {
        #region Lord 2015/11/14

        /// <summary>
        /// ������ֵ�
        /// </summary>
        public static Dictionary<String, Security> m_codedMap = new Dictionary<String, Security>();

        /// <summary>
        /// ��������
        /// </summary>
        private static Dictionary<String, List<SecurityData>> m_historyDatas = new Dictionary<String, List<SecurityData>>();

        /// <summary>
        /// ��������
        /// </summary>
        private static Dictionary<String, SecurityLatestData> m_latestDatas = new Dictionary<String, SecurityLatestData>();

        /// <summary>
        /// ���������ַ���
        /// </summary>
        private static Dictionary<String, String> m_latestDatasStr = new Dictionary<String, String>();

        /// <summary>
        /// ��ʱ����
        /// </summary>
        private static Dictionary<String, List<SecurityData>> m_minuteDatas = new Dictionary<String, List<SecurityData>>();

        /// <summary>
        /// ÿ�������ݴ��Ŀ¼
        /// </summary>
        private static String m_newFileDir = "";

        /// <summary>
        /// ��Ʊ�б�
        /// </summary>
        private static List<Security> m_securities = new List<Security>();

        /// <summary>
        /// ����ʱ��
        /// </summary>
        private static long m_today = 0;

        /// <summary>
        /// ����ʱ����������xls�����ļ�
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
        /// �������к�Լ
        /// </summary>
        /// <param name="codes">��Լ���뼯��</param>
        public static void GetCodes(List<String> codes)
        { 
            foreach(String key in m_codedMap.Keys)
            {
                codes.Add(key);
            }
        }

        /// <summary>
        /// ͨ�����뷵�غ�Լ��Ϣ
        /// </summary>
        /// <param name="code">��Լ����</param>
        /// <param name="sd">out ��Ʊ������Ϣ</param>
        public static int GetSecurityByCode(String code, ref Security security)
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
        /// ��ȡ�����ֵ�
        /// </summary>
        public static void GetCodeMap()
        {
            m_codedMap.Clear();
            String codeFullStr = "";
            CFileA.Read(DataCenter.GetAppPath() + "\\codes.txt", ref codeFullStr);
            String[] lines = codeFullStr.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String line in lines)
            {
                if (line.Trim() == "")
                {
                    continue;
                }
                String pCode = line.Split(new String[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)[0];
                String code = CStrA.ConvertFileCodeToMemoryCode(pCode);
                String cName = line.Split(new String[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)[1];
                Security security =  new Security();
                security.m_code = code;
                security.m_name = cName;
                m_codedMap[code] = security;
            }
        }

        /// <summary>
        /// ��ȡǰһ���µ��Ĺ�Ʊ
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
        /// ��ȡ��������
        /// </summary>
        /// <param name="code">����</param>
        /// <param name="latestData">��������</param>
        /// <returns>״̬</returns>
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
        /// ��ȡ��������
        /// </summary>
        /// <param name="code">����</param>
        /// <param name="latestData">��������</param>
        /// <returns>״̬</returns>
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
        /// ������ʷ����
        /// </summary>
        /// <param name="history"></param>
        public static void LoadHistory()
        {
            if (m_historyDatas.Count > 0)
            {
                return;
            }
            foreach(String code in m_codedMap.Keys)
            {
                String fileName = DataCenter.GetAppPath() + "\\data\\day\\fdata\\" + code + ".txt";
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
        /// ��ȡ��ʱ����
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
                    fileName = m_newFileDir + code.Substring(2) + "." + code.Substring(0, 2) + ".txt";
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
        /// ��ȡ��ͣ��Ʊ��Լ����
        /// </summary>
        /// <param name="limitUpCodes">out ��ͣ��Ʊ��Լ����</param>
        public static void GetLimitUp(ref List<String> limitUpCodes)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    if (lastData.m_sellVolume1 == 0)
                    {
                        if (lastData.m_buyVolume1 == 0)
                        {
                            //�޽��׹�Ʊ
                            continue;
                        }
                        limitUpCodes.Add(code);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ��ͣ��Լ����
        /// </summary>
        /// <param name="limitDownCodes">out ��ͣ��Լ����</param>
        public static void GetLimitDown(ref List<String> limitDownCodes)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    if (lastData.m_buyVolume1 == 0)
                    {
                        if (lastData.m_sellVolume1 == 0)
                        {
                            //�޽��׹�Ʊ
                            continue;
                        }
                        limitDownCodes.Add(code);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ�޽��׺�Լ����
        /// </summary>
        /// <param name="notTradedCodes">out  �޽��׺�Լ����</param>
        public static void GetNotTradedCodes(ref List<String> notTradedCodes)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    if (lastData.m_buyVolume1 == 0 && lastData.m_sellVolume1 == 0)
                    {
                        notTradedCodes.Add(code);
                    }
                }
            }
        }

        /// <summary>
        /// ���׺�Լ����
        /// </summary>
        /// <param name="notTradedCodes">out  ���׺�Լ����</param>
        public static void GetTradedCodes(ref List<String> tradedCodes)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    if (lastData.m_buyVolume1 != 0 || lastData.m_sellVolume1 != 0)
                    {
                        tradedCodes.Add(code);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡST��Ʊ
        /// </summary>
        /// <param name="stCodes">out ST��Ʊ</param>
        public static void GetSTCodes(ref List<String> stCodes)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            foreach (String code in codes)
            {
                Security security = new Security();
                if (SecurityService.GetSecurityByCode(code, ref security) == 1)
                {
                    if (security.m_name.IndexOf("ST") == 0)
                    {
                        stCodes.Add(code);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ*ST��Ʊ
        /// </summary>
        /// <param name="stCodes">out ��ȡ*ST��Ʊ</param>
        public static void GetStarSTCodes(ref List<String> starSTCodes)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            foreach (String code in codes)
            {
                Security security = new Security();
                if (SecurityService.GetSecurityByCode(code, ref security) == 1)
                {
                    if (security.m_name.IndexOf("*ST") == 0)
                    {
                        starSTCodes.Add(code);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ�Ƿ�����
        /// </summary>
        /// <param name="riseRanking">out �Ƿ�����</param>
        public static void GetRiseRanking(ref List<String> riseRanking)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    double rank = (lastData.m_close - lastData.m_lastClose) / lastData.m_lastClose;
                    dic.Add(code, rank);
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
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="riseRanking">out ��������</param>
        public static void GetFallRanking(ref List<String> fallRanking)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    double rank = (lastData.m_close - lastData.m_lastClose) / lastData.m_lastClose;
                    dic.Add(code, rank);
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
        }

        /// <summary>
        /// ���ݳɽ�������
        /// </summary>
        /// <param name="codesByVol">�ɽ�������</param>
        public static void GetCodesByVolume(ref List<String> codesByVol)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    dic.Add(code, lastData.m_volume);
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
                codesByVol.Add(lst[i].Key);
            }
        }

        /// <summary>
        /// ���ݳɽ�������
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static void GetCodesByAmount(ref List<String> codesByAmount)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    dic.Add(code, lastData.m_amount);
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
                codesByAmount.Add(lst[i].Key);
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static void GetCodesBySwing(ref List<String> codesBySwing)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    dic.Add(code, (int)(lastData.m_high - lastData.m_low) / lastData.m_lastClose);
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
                codesBySwing.Add(lst[i].Key);
            }
        }

        /// <summary>
        /// ���ݹɼ�����
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static void GetCodesByPrice(ref List<String> codesByPrice)
        {
            List<String> codes = new List<String>();
            SecurityService.GetCodes(codes);
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (String code in codes)
            {
                SecurityLatestData lastData = new SecurityLatestData();
                if (SecurityService.GetLatestData(code, ref lastData) == 1)
                {
                    dic.Add(code, lastData.m_close);
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
                codesByPrice.Add(lst[i].Key);
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public static void Start()
        {
            Thread thread = new Thread(new ThreadStart(StartWork));
            thread.Start();
        }

        /// <summary>
        /// �������߳�
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
        /// ������������߳�
        /// </summary>
        public static void Start3()
        {
            Thread thread = new Thread(new ThreadStart(StartWork3));
            thread.Start();
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private static void StartWork()
        {
            //���ش����//step 1
            String codes = "";
            if (m_securities.Count == 0)
            {
                String codesStr = "";
                CFileA.Read(DataCenter.GetAppPath() + "\\codes.txt", ref codesStr);
                String[] strs = codesStr.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < strs.Length; i++)
                {
                    String[] subStrs = strs[i].Split('\t');
                    Security security = new Security();
                    security.m_code = CStrA.ConvertFileCodeToMemoryCode(subStrs[0]);
                    security.m_name = subStrs[1];
                    lock (m_securities)
                    {
                        m_securities.Add(security);
                    }
                    m_codedMap[security.m_code] = security;
                    codes += security.m_code;
                    codes += ",";
                    if (!security.m_code.StartsWith("A"))
                    {
                        sb.Append(security.m_code + "," + security.m_name + "\r\n");
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
                                        m_latestDatasStr[latestData.m_securityCode] = subStrs[j];
                                        bool append = true;
                                        if (m_latestDatas.ContainsKey(latestData.m_securityCode))
                                        {
                                            if (!m_latestDatas[CStrA.ConvertFileCodeToMemoryCode(latestData.m_securityCode)].Equals(latestData))
                                            {
                                                append = false;
                                            }
                                        }
                                        if(append)
                                        {
                                            long today = (long)DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds / 86400000;
                                            if (m_today < today)
                                            {
                                                m_today = today;
                                                String nPath = DataCenter.GetAppPath() + "\\tick\\" + DateTime.Now.ToString("yyyy-MM-dd");
                                                if (!Directory.Exists(nPath))
                                                {
                                                    Directory.CreateDirectory(nPath);
                                                }
                                                m_newFileDir = nPath + "\\";
                                            }
                                            String line = String.Format("{0},{1},{2},{3}\r\n", latestData.m_date,//
                                                latestData.m_close, latestData.m_volume, latestData.m_amount);
                                            CFileA.Append(m_newFileDir + latestData.m_securityCode + ".txt", line);
                                        }
                                        SecurityLatestData cp = new SecurityLatestData();
                                        cp.Copy(latestData);
                                        cp.m_securityCode = CStrA.ConvertFileCodeToMemoryCode(latestData.m_securityCode);
                                        m_latestDatas[CStrA.ConvertFileCodeToMemoryCode(latestData.m_securityCode)] = cp;
                                        cp = null;
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
        /// ��ʼ����2
        /// </summary>
        /// <param name="param"></param>
        public static void StartWork2(object param)
        {
            object[] arr = (object[])param;
            int type =  (int)arr[0];
            double min = (double)arr[1];
            double max = (double)arr[2];
            LoadHistory();
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
        /// ��������̹߳���
        /// </summary>
        public static void StartWork3()
        {
            //��������
            LoadHistory();
            GetMinuteDatas();
            Dictionary<String, List<SecurityData>> cpHistoryDatas = new Dictionary<String, List<SecurityData>>(m_historyDatas.Count);
            foreach (String oCode in m_historyDatas.Keys)
            {
                cpHistoryDatas[oCode] = m_historyDatas[oCode];
            }
            //�¾����ݺϲ�
            foreach(String oCode in m_historyDatas.Keys)
            {
                if (!m_latestDatas.ContainsKey(oCode) || !m_historyDatas.ContainsKey(oCode) || !m_minuteDatas.ContainsKey(oCode)) continue;
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
                    List<SecurityData> temp = m_minuteDatas[oCode];
                    SecurityData temp2 = temp[temp.Count - 1];
                    nSecurityData.m_avgPrice = temp2.m_avgPrice;
                    nSecurityData.m_close = securityLatestData.m_close;
                    nSecurityData.m_date = securityLatestData.m_date;
                    nSecurityData.m_high = securityLatestData.m_high;
                    nSecurityData.m_low = securityLatestData.m_low;
                    nSecurityData.m_open = securityLatestData.m_open;
                    nSecurityData.m_volume = securityLatestData.m_volume;
                    List<SecurityData> temp3 = cpHistoryDatas[oCode];
                    if (day == mday)
                    {
                        temp3.RemoveAt(temp3.Count - 1);
                    }
                    temp3.Add(nSecurityData);
                }
            }
            String outputFileTemplate = DataCenter.GetAppPath() + "\\data\\day\\fdata\\{0}.txt";
            String fileInfo = "{0} {1} ���� ǰ��Ȩ\r\n";
            String title = "      ����	    ����	    ���	    ���	    ����	    �ɽ���	    �ɽ���\r\n";
            String lineTemp = "{0},{1},{2},{3},{4},{5},{6}\r\n";
            String timeFormatStr = "yyyy-MM-dd";
            //д���ļ�
            foreach (String code in cpHistoryDatas.Keys)
            {
                List<SecurityData> temp3 = cpHistoryDatas[code];
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
                strbuff.Append("������Դ:ͨ����\r\n");
                CFileA.Write(String.Format(outputFileTemplate, code), strbuff.ToString());
            }
            //�����ڴ��е�������
            m_historyDatas.Clear();
            m_historyDatas = null;
            m_historyDatas = cpHistoryDatas;
        }

        /// <summary>
        /// ��ȡ����
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
        /// �������ػ�ȡ��Ʊ
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

        /// <summary>
        /// ��ʱ����
        /// </summary>
        public static void TimeStrategy()
        {
            //long today = (long)DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds / 86400000;
            long dayOfNineHalfAM = 0;//����9���
            long dayOfElvHalfAM = 0;//����11��
            long dayOfOnePM = 0;//����1��
            long dayOfThreePM = 0;//����3��
            long minOfDay = (long)DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds / 8640000;
            long eveyWeekend = 0;
            if ((minOfDay > dayOfNineHalfAM && minOfDay < dayOfElvHalfAM) || (minOfDay > dayOfOnePM && minOfDay < dayOfThreePM))
            {
                //�����ʱ����ȡ��Ʊ//but how does weekend
            }
        }
        #endregion
    }
}
