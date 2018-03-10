using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class FinanceSeriesDataPacket : SeriesDataPacket
    {
        private List<DateTime> _huanDateList = new List<DateTime>();
        private List<DateTime> _sameDateList = new List<DateTime>();

        public FinanceSeriesDataPacket(CommonEnumerators.IndicatorRequestType requestDatType, List<StockEntity> stockList, List<int> customerIndicatorIdList)
        {
            base.RequestId = RequestType.FinanceSeriesData;
            base._requestDataType = requestDatType;
            base._stockList = stockList;
            base._customerIndicatorIdList = customerIndicatorIdList;
            if ((base._stockList.Count > 1) && (base._customerIndicatorIdList.Count > 1))
            {
                throw new Exception("行情序列不支持多个股票多个指标");
            }
            base._indicatorList = new List<IndicatorEntity>();
            foreach (int num in base._customerIndicatorIdList)
            {
                IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(num, false);
                base._indicatorList.Add(indicatorEntityByCustomerId);
                base._indicatorUnitDict[num] = DataCore.CreateInstance().GetIndicatorUnit(indicatorEntityByCustomerId);
            }
        }

        public override String Coding()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("$-{0}{1}", CommonEnumerators.GetIndicatorRequestTypeCmd(base._requestDataType), Environment.NewLine);
            List<String> list = new List<String>();
            List<String> list2 = new List<String>();
            for (int i = 0; i < base._indicatorList.Count; i++)
            {
                String[] strArray;
                IndicatorEntity entity = base._indicatorList[i];
                list.Add(entity.ProductFunc);
                List<String> list3 = new List<String>();
                list3.Add(String.Format("$TableName={0}", base._customerIndicatorIdList[i]));
                Dictionary<String, object> dictionary = base.BuildParamDictionary(entity.Parameters);
                Dictionary<String, object> dictionary2 = base.BuildParamDictionary(entity.ParaInfo);
                foreach (KeyValuePair<String, object> pair in dictionary)
                {
                    dictionary2[pair.Key] = dictionary[pair.Key];
                }
                foreach (KeyValuePair<String, object> pair2 in base.ParamDictionary)
                {
                    if (dictionary2.ContainsKey(pair2.Key.ToLower()))
                    {
                        dictionary2[pair2.Key.ToLower()] = pair2.Value;
                    }
                }
                dictionary2["indicatorcode"] = entity.IndicatorCode;
                String str = dictionary2["reportdate"].ToString();
                DateTime secuLatestReportDate = DataServiceHelper.GetSecuLatestReportDate(base._stockList[0].StockCode);
                if ((base.ParamDictionary.Count == 0) || str.ToUpper().Trim().Equals("N"))
                {
                    List<String> list4 = new List<String>();
                    list4.Add(new DateTime(secuLatestReportDate.Year - 5, 12, 0x1f).ToString("yyyy-MM-dd"));
                    list4.Add(new DateTime(secuLatestReportDate.Year - 4, 12, 0x1f).ToString("yyyy-MM-dd"));
                    list4.Add(new DateTime(secuLatestReportDate.Year - 3, 12, 0x1f).ToString("yyyy-MM-dd"));
                    list4.Add(new DateTime(secuLatestReportDate.Year - 2, 12, 0x1f).ToString("yyyy-MM-dd"));
                    list4.Add(new DateTime(secuLatestReportDate.Year - 1, 12, 0x1f).ToString("yyyy-MM-dd"));
                    String item = secuLatestReportDate.ToString("yyyy-MM-dd");
                    if (!list4.Contains(item))
                    {
                        list4.Add(item);
                    }
                    strArray = list4.ToArray();
                }
                else
                {
                    strArray = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
                List<String> list5 = new List<String>();
                foreach (String str3 in strArray)
                {
                    DateTime time2 = DateTime.Parse(str3);
                    if (time2.Date <= secuLatestReportDate.Date)
                    {
                        list5.Add(str3);
                        String s = String.Empty;
                        if (!base.DateList.Contains(time2))
                        {
                            base.DateList.Add(time2);
                        }
                        s = DateHelper.GetSameCompareReportDate(str3);
                        this._sameDateList.Add(DateTime.Parse(s));
                        list5.Add(s);
                        s = DateHelper.GetHuanCompareReportDate(str3);
                        this._huanDateList.Add(DateTime.Parse(s));
                        list5.Add(s);
                    }
                }
                base.DateList.Sort();
                base.DateList.Reverse();
                dictionary2["reportdate"] = String.Join("|", list5.ToArray());
                foreach (KeyValuePair<String, object> pair3 in dictionary2)
                {
                    list3.Add(String.Format("{0}={1}", pair3.Key, pair3.Value.ToString().Trim()));
                }
                list2.Add(String.Join(",", list3.ToArray()));
            }
            builder.AppendFormat("$name={0}{1}", String.Join(",", list.ToArray()), Environment.NewLine);
            foreach (StockEntity entity2 in base._stockList)
            {
                if (!base._categoryStockCodeDict.ContainsKey(entity2.CategoryCode))
                {
                    base._categoryStockCodeDict[entity2.CategoryCode] = new List<String>();
                }
                base._categoryStockCodeDict[entity2.CategoryCode].Add(entity2.StockCode);
            }
            List<String> list6 = new List<String>();
            foreach (KeyValuePair<int, List<String>> pair4 in base._categoryStockCodeDict)
            {
                list6.Add(String.Format("{0}:{1}", pair4.Key, String.Join(",", pair4.Value.ToArray())));
            }
            builder.AppendFormat("$secucode={0}{1}", String.Join("#", list6.ToArray()), Environment.NewLine);
            builder.Append(String.Join(Environment.NewLine, list2.ToArray()));
            return builder.ToString();
        }

        public List<double> GetHuanCompareList(String stockCode, int customerId)
        {
            this._huanDateList.Clear();
            foreach (DateTime time in base.DateList)
            {
                this._huanDateList.Add(DateTime.Parse(DateHelper.GetHuanCompareReportDate(time.ToString("yyyy-MM-dd"))));
            }
            List<double> list = new List<double>();
            List<double> list2 = base.GetValueList(stockCode, customerId, this._huanDateList);
            List<double> list3 = base.GetValueList(stockCode, customerId, base.DateList);
            for (int i = 0; i < base.DateList.Count; i++)
            {
                if (((list2[i] == 0.0) || (list2[i] == double.MaxValue)) || ((double.IsInfinity(list2[i]) || (list3[i] == double.MaxValue)) || double.IsInfinity(list3[i])))
                {
                    list.Add(double.MaxValue);
                }
                else
                {
                    list.Add(((list3[i] - list2[i]) * 100.0) / list2[i]);
                }
            }
            return list;
        }

        public List<double> GetSameCompareList(String stockCode, int customerId)
        {
            this._sameDateList.Clear();
            foreach (DateTime time in base.DateList)
            {
                this._sameDateList.Add(DateTime.Parse(DateHelper.GetSameCompareReportDate(time.ToString("yyyy-MM-dd"))));
            }
            List<double> list = new List<double>();
            List<double> list2 = base.GetValueList(stockCode, customerId, this._sameDateList);
            List<double> list3 = base.GetValueList(stockCode, customerId, base.DateList);
            for (int i = 0; i < base.DateList.Count; i++)
            {
                if (((list2[i] == 0.0) || (list2[i] == double.MaxValue)) || ((double.IsInfinity(list2[i]) || (list3[i] == double.MaxValue)) || double.IsInfinity(list3[i])))
                {
                    list.Add(double.MaxValue);
                }
                else
                {
                    list.Add(((list3[i] - list2[i]) * 100.0) / list2[i]);
                }
            }
            return list;
        }

        public List<DateTime> HuanDateList
        {
            get
            {
                return this._huanDateList;
            }
        }

        public List<DateTime> SameDateList
        {
            get
            {
                return this._sameDateList;
            }
        }
    }
}
