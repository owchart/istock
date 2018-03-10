using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmSerDataService;
using System.Diagnostics;
using dataquery;

namespace dataquery.indicator
{
    public class DataServiceHelper
    {
        private static Dictionary<String, DateTime> _secuLatestReportDateDict = new Dictionary<String, DateTime>();
        private static Dictionary<String, DateTime> dict = new Dictionary<String, DateTime>();
        private static DataQuery dq = DataCenter.DataQuery;
        private const String LatestReportDate = "$-fun\r\n$name=100000000015309\r\n$secucode=\r\n$";
        private const String LatestTradeDateCommandStr = "$-fun\r\n$name=100000000016822\r\n$secucode=000001.SZ\r\n$TradeDate=N,N=0";
        private const String SecuLatestReportDateCommandStr = "$-fun\r\n$name=100000000016823\r\n$secucode={0}\r\n$KeyField=SECURITYCODE,TableName=139fd340-9c1c-44ae-b0da-d92a2ed82456,Type=0";

        public static DateTime GetLastestReportDateOnlyOne(String typeCode)
        {
            if (dict.ContainsKey(typeCode))
            {
                return dict[typeCode];
            }
            DateTime latestDate = GetLatestDate("$-fun\r\n$name=100000000015309\r\n$secucode=\r\n$");
            dict.Add(typeCode, latestDate);
            return latestDate;
        }

        public static DateTime GetLastestTradeDate()
        {
            return GetLatestDate("$-fun\r\n$name=100000000016822\r\n$secucode=000001.SZ\r\n$TradeDate=N,N=0");
        }

        private static DateTime GetLatestDate(String commandStr)
        {
            DateTime now = DateTime.Now;
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                DataSet set = dq.QueryIndicate(commandStr) as DataSet;
                stopwatch.Stop();
                //LogUtility.LogTableMessage("数据中心：请求最新日期或报告期QueryIndicate：" + stopwatch.ElapsedMilliseconds + "毫秒 ");
                if ((set == null) || (set.Tables.Count <= 0))
                {
                    return now;
                }
                DataTable table = set.Tables[0];
                if (table.Rows.Count <= 0)
                {
                    return now;
                }
                if (table.Rows[0].ItemArray.Length >= 2)
                {
                    if (table.Rows[0].ItemArray[1] != DBNull.Value)
                    {
                        DateTime.TryParse(table.Rows[0].ItemArray[1].ToString(), out now);
                    }
                    return now;
                }
                if ((table.Rows[0].ItemArray.Length > 0) && (table.Rows[0].ItemArray[0] != DBNull.Value))
                {
                    DateTime.TryParse(table.Rows[0].ItemArray[0].ToString(), out now);
                }
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("请求【系统报告期】异常：" + commandStr + " " + exception.Message + exception.StackTrace);
            }
            return now;
        }

        public static DateTime GetSecuLatestReportDate(String stockCode)
        {
            if (!_secuLatestReportDateDict.ContainsKey(stockCode))
            {
                DataSet set = dq.QueryIndicate(String.Format("$-fun\r\n$name=100000000016823\r\n$secucode={0}\r\n$KeyField=SECURITYCODE,TableName=139fd340-9c1c-44ae-b0da-d92a2ed82456,Type=0", stockCode)) as DataSet;
                if ((set != null) && (set.Tables.Count > 0))
                {
                    DataTable table = set.Tables[0];
                    if ((table.Rows.Count > 0) && (table.Rows[0].ItemArray.Length >= 2))
                    {
                        DateTime now = DateTime.Now;
                        if (table.Rows[0].ItemArray[1] != DBNull.Value)
                        {
                            DateTime.TryParse(table.Rows[0].ItemArray[1].ToString(), out now);
                            _secuLatestReportDateDict[stockCode] = now;
                            return _secuLatestReportDateDict[stockCode];
                        }
                    }
                }
                _secuLatestReportDateDict[stockCode] = GetLastestReportDateOnlyOne(stockCode);
            }
            return _secuLatestReportDateDict[stockCode];
        }
    }
}
