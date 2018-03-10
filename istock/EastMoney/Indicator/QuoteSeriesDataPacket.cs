using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class QuoteSeriesDataPacket : SeriesDataPacket
    {
        public QuoteSeriesDataPacket(CommonEnumerators.IndicatorRequestType requestDatType, List<StockEntity> stockList, List<int> customerIndicatorIdList)
        {
            base.RequestId = RequestType.QuoteSeriesData;
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
                base._indicatorList.Add(IndicatorDataCore.GetIndicatorEntityByCustomerId(num, false));
                base._indicatorUnitDict[num] = DataCore.CreateInstance().GetIndicatorUnit(num);
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
                if (!dictionary2.ContainsKey("period"))
                {
                    dictionary2["period"] = 1;
                }
                foreach (KeyValuePair<String, object> pair3 in dictionary2)
                {
                    list3.Add(String.Format("{0}={1}", pair3.Key, pair3.Value));
                }
                if (dictionary2.ContainsKey("startdate"))
                {
                    base.ParamDictionary["startdate"] = DateHelper.ResolveDate(dictionary2["startdate"].ToString());
                }
                if (dictionary2.ContainsKey("enddate"))
                {
                    base.ParamDictionary["enddate"] = DateHelper.ResolveDate(dictionary2["enddate"].ToString());
                }
                if (dictionary2.ContainsKey("period"))
                {
                    base.ParamDictionary["period"] = dictionary2["period"];
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
            List<String> list4 = new List<String>();
            foreach (KeyValuePair<int, List<String>> pair4 in base._categoryStockCodeDict)
            {
                list4.Add(String.Format("{0}:{1}", pair4.Key, String.Join(",", pair4.Value.ToArray())));
            }
            builder.AppendFormat("$secucode={0}{1}", String.Join("#", list4.ToArray()), Environment.NewLine);
            builder.Append(String.Join(Environment.NewLine, list2.ToArray()));
            return builder.ToString();
        }

        protected override bool IsQuoteSeries()
        {
            return true;
        }
    }
}
