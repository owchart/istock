using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace OwLib
{
    public class QuoteSequencService
    {
        /// <summary>
        /// 复权类型 1-不复权，2-后复权， 3-前复权
        /// </summary> 
        private static List<String> m_complexRightType = new List<String>();
        /// <summary>
        /// 指标ID列表
        /// </summary>
        private static List<String> m_lstAIndicators = new List<String>();
        /// <summary>
        /// 指标字段列表
        /// </summary>
        private static List<String> m_lstAIndicatorFields = new List<String>();

        static QuoteSequencService()
        {
            //复权类型 1-不复权，2-后复权， 3-前复权
            m_complexRightType.Add("1");
            m_complexRightType.Add("2");
            m_complexRightType.Add("3");

            //开盘价--100000000001096
            //收盘价--100000000001098
            //最高价--1100000000001094
            //最低价--1100000000001095
            //涨跌--1100000000001103
            //涨跌幅--1100000000001097
            //前收盘价--1100000000001093
            //成交量--1100000000001100
            //成交金额--1100000000004754
            m_lstAIndicators.Add("100000000001096");
            m_lstAIndicators.Add("100000000001094");
            m_lstAIndicators.Add("100000000001095");
            m_lstAIndicators.Add("100000000001098");
            m_lstAIndicators.Add("100000000001100");
            m_lstAIndicators.Add("100000000001104");

            //字段列表
            m_lstAIndicatorFields.Add("OPEN");
            m_lstAIndicatorFields.Add("HIGH");
            m_lstAIndicatorFields.Add("LOW");
            m_lstAIndicatorFields.Add("NEW");
            m_lstAIndicatorFields.Add("TVOL");
            m_lstAIndicatorFields.Add("TVAL");
        }

        /// <summary>
        /// 下载所有的A股市场股票历史数据
        /// </summary>
        public static void DownAllStockHistory(int index)
        {
            List<KwItem> availableItems = new List<KwItem>();
            foreach (KwItem item in EMSecurityService.KwItems.Values)
            {
                availableItems.Add(item);
            }
            int itemsSize = availableItems.Count;
            int complexRightIndex = 0;
            String saveFilePath = "";
            for (int i = index * 50; i < itemsSize && i < (index + 1) * 50; i++)
            {
                KwItem item = availableItems[i];
                complexRightIndex = 0;
                for (; complexRightIndex < m_complexRightType.Count; complexRightIndex++)
                {
                    try
                    {
                        String cmd = String.Format(GetSearchCmd(m_complexRightType[complexRightIndex], m_lstAIndicators, item.Code), Environment.NewLine);
                        if (String.IsNullOrEmpty(cmd))
                        {
                            continue;
                        }
                        DataSet dsResult = DataCenter.DataQuery.QueryIndicate(cmd) as DataSet;
                        if (dsResult == null || dsResult.Tables.Count == 0)
                        {
                            continue;
                        }
                        IDictionary<String, String[]> dicResult = GetDictionaryFromDataSet(dsResult, m_lstAIndicators);
                        if (dicResult.Count > 0)
                        {
                            String code = item.Code;
                            if (code.IndexOf(".") != -1)
                            {
                                code = code.Substring(code.IndexOf(".") + 1) + code.Substring(0, code.IndexOf("."));
                            }
                            saveFilePath = String.Format("{0}{1}{2}{3}", "D:\\owchart\\quotenew\\data\\day", "\\fdata\\", code, ".txt");
                            StringBuilder sbResult = new StringBuilder();
                            sbResult.AppendLine(item.Code + " " + item.Name + " 日线 前复权");
                            sbResult.AppendLine("      日期	    开盘	    最高	    最低	    收盘	    成交量	    成交额");
                            foreach (KeyValuePair<String, String[]> pair in dicResult)
                            {
                                sbResult.AppendLine(FormatStockInfo(pair.Value, ","));
                            }
                            sbResult.AppendLine("OWCHART荣誉出品");
                            File.WriteAllText(saveFilePath, sbResult.ToString());
                            sbResult = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                    }
                }
            }
        }

        private static String FormatDateValue(object obj, String format)
        {
            DateTime time;
            if (DateTime.TryParse(obj.ToString(), out time))
            {
                return time.ToString(format);
            }
            return "";
        }

        private static String FormatDoubleValue4Fraction(double value)
        {
            return Math.Round(value, 4).ToString("#,##0.0000");
        }

        private static String FormatString(List<String> lstFields)
        {
            StringBuilder builder = new StringBuilder();
            foreach (String str in lstFields)
            {
                builder.Append(",");
                builder.Append(str);
            }
            if (builder.Length > 1)
            {
                builder.Remove(0, 1);
            }
            return builder.ToString();
        }

        private static String FormatStockInfo(String[] fields, String separater)
        {
            StringBuilder sb = new StringBuilder();
            int fieldsSize = fields.Length;
            for (int i = 1; i < fieldsSize; i++)
            {
                if (i == 6 || i == 7)
                {
                    sb.Append(((long)Convert.ToDouble(fields[i])).ToString());
                }
                else
                {
                    sb.Append(fields[i]);
                }
                if (i != fieldsSize - 1)
                {
                    sb.Append(separater);
                }
            }
            return sb.ToString();
        }

        private static IDictionary<String, String[]> GetDictionaryFromDataSet(DataSet dsStockInfo, List<String> lstAIndicators)
        {
            IDictionary<String, String[]> dicResult = new Dictionary<String, String[]>();
            DataTable table = null;
            String sort = " SecuCode ASC ,TradeDate ASC";
            int arrayLength = lstAIndicators.Count + 2;
            int index = 2;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Reset();
                stopwatch.Start();
                foreach (String name in lstAIndicators)
                {
                    if (!dsStockInfo.Tables.Contains(name))
                    {
                        index++;
                    }
                    else
                    {
                        table = dsStockInfo.Tables[name];
                        try
                        {
                            DataRow[] rowArray = table.Select("", sort);
                            foreach (DataRow row in rowArray)
                            {
                                if (row.ItemArray.Length >= 3)
                                {
                                    String key = String.Format("{0}~{1}", row[0], row[1]);
                                    if (!dicResult.ContainsKey(key))
                                    {
                                        dicResult[key] = new String[arrayLength];
                                    }
                                    String[] objArray = dicResult[key];
                                    objArray[0] = row[0].ToString();
                                    objArray[1] = FormatDateValue(row[1], "yyyy-MM-dd");
                                    objArray[index] = FormatDoubleValue4Fraction(Convert.ToDouble(row[2]));
                                    dicResult[key] = objArray;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                        }
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            return dicResult;
        }

        private static String GetSearchCmd(String complexRightType, List<String> lstFields, String stockCode)
        {
            if ((lstFields == null) || (lstFields.Count == 0))
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("$-quote{0}$name=");
            builder.Append(FormatString(lstFields));
            builder.Append("{0}$secucode=");
            builder.Append(stockCode);
            foreach (String str in lstFields)
            {
                builder.Append("{0}$TableName=");
                builder.Append(str);
                builder.Append(",TradeDate=");
                builder.Append(DateTime.MinValue.ToString("yyyy-MM-dd"));
                builder.Append("~");
                builder.Append(DateTime.MaxValue.ToString("yyyy-MM-dd"));
                builder.Append(",AdjustFlag=");
                builder.Append(complexRightType);
            }
            return builder.ToString();
        }
    }
}
