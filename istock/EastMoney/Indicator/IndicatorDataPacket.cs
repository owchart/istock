using System;
using System.Collections.Generic;
using System.Text;
using EmCore;

namespace OwLib
{
    public class IndicatorDataPacket2 : DataPacketBase
    {
        private Dictionary<int, List<String>> _categoryStockCodeDict = new Dictionary<int, List<String>>();
        private List<int> _customerIndicatorIdList;
        private List<IndicatorEntity> _indicatorList;
        private CommonEnumerators.IndicatorRequestType _requestDataType;
        private List<StockEntity> _stockList;

        public IndicatorDataPacket2(CommonEnumerators.IndicatorRequestType requestDatType, List<StockEntity> stockList, List<int> customerIndicatorIdList)
        {
            base.RequestId = RequestType.IndicatorData;
            this._requestDataType = requestDatType;
            this._stockList = stockList;
            this._customerIndicatorIdList = new List<int>();
            this._indicatorList = new List<IndicatorEntity>();
            foreach (int num in customerIndicatorIdList)
            {
                IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(num);
                indicatorEntityByCustomerId.CustomerId = num;
                this._indicatorList.Add(indicatorEntityByCustomerId);
                this._customerIndicatorIdList.Add(num);
            }
        }

        public override String Coding()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("$-{0}{1}", CommonEnumerators.GetIndicatorRequestTypeCmd(this._requestDataType), Environment.NewLine);
            List<String> list = new List<String>();
            List<String> list2 = new List<String>();
            for (int i = 0; i < this._indicatorList.Count; i++)
            {
                IndicatorEntity entity = this._indicatorList[i];
                list.Add(entity.IndicatorCode);
                List<String> list3 = new List<String>();
                list3.Add(String.Format("$TableName={0}", this._customerIndicatorIdList[i]));
                if (String.IsNullOrEmpty(entity.Parameters) || entity.Parameters.Equals("null"))
                {
                    if (String.IsNullOrEmpty(entity.IndicatorType) || entity.IndicatorType.Trim().Equals("0"))
                    {
                        list3.Add("KeyField=SECURITYCODE");
                    }
                    list2.Add(String.Join(",", list3.ToArray()));
                    continue;
                }
                List<ParamterObject> indicatorParameterList = JSONHelper.DeserializeObject<List<ParamterObject>>(entity.Parameters);
                base.ResolveObjectParameters(indicatorParameterList);
                foreach (ParamterObject obj2 in indicatorParameterList)
                {
                    if (obj2.Type.Equals("261"))
                    {
                        continue;
                    }
                    if (String.IsNullOrEmpty(obj2.Name))
                    {
                        if (obj2.Type != "279")
                        {
                            list3.Add(String.Format("FieldName={0}", obj2.DefaultValue));
                        }
                        else if (obj2.Name.Trim().Equals("type"))
                        {
                            String str = obj2.DefaultValue.ToString();
                            list3.Add(String.Format("type={0}", str.Substring(str.Length - 1, 1)));
                        }
                        continue;
                    }
                    if ((obj2.Name.Trim().Equals("type") && (obj2.Type != "115")) && (obj2.Type != "219"))
                    {
                        int result = 0;
                        String s = obj2.DefaultValue.ToString();
                        if (!int.TryParse(s, out result))
                        {
                            s = s.Substring(s.Length - 1, 1);
                        }
                        list3.Add(String.Format("type={0}", s));
                        continue;
                    }
                    list3.Add(String.Format("{0}={1}", obj2.Name.Trim(), obj2.DefaultValue));
                }
                if (String.IsNullOrEmpty(entity.IndicatorType) || entity.IndicatorType.Trim().Equals("0") || entity.IndicatorType.Trim().Equals("1"))
                {
                    list3.Add("KeyField=SECURITYCODE");
                }
                list2.Add(String.Join(",", list3.ToArray()));
            }
            builder.AppendFormat("$name={0}{1}", String.Join(",", list.ToArray()), Environment.NewLine);
            foreach (StockEntity entity2 in this._stockList)
            {
                if (!this._categoryStockCodeDict.ContainsKey(entity2.CategoryCode))
                {
                    this._categoryStockCodeDict[entity2.CategoryCode] = new List<String>();
                }
                this._categoryStockCodeDict[entity2.CategoryCode].Add(entity2.StockCode);
            }
            if (_requestDataType != CommonEnumerators.IndicatorRequestType.Blk)
            {
                List<String> list5 = new List<String>();
                foreach (KeyValuePair<int, List<String>> pair in this._categoryStockCodeDict)
                {
                    list5.Add(String.Format("{0}:{1}", pair.Key, String.Join(",", pair.Value.ToArray())));
                }
                builder.AppendFormat("$secucode={0}{1}", String.Join("#", list5.ToArray()), Environment.NewLine);
            }
            else
            {
                List<String> list5 = new List<String>();
                foreach (KeyValuePair<int, List<String>> pair in this._categoryStockCodeDict)
                {
                    list5.Add(String.Format("{0}", String.Join(",", pair.Value.ToArray())));
                }
                builder.AppendFormat("$publishcode={0}{1}", String.Join("#", list5.ToArray()), Environment.NewLine);
            }
            builder.Append(String.Join(Environment.NewLine, list2.ToArray()));
            return builder.ToString().Trim();
        }

        public override bool Decoding(byte[] bytes)
        {
            List<String> list = new List<String>();
            foreach (int num in this.CustomerIndicatorIdList)
            {
                list.Add(num.ToString());
            }
            if ((bytes != null) && (bytes.Length == 8))
            {
                //LogUtility.LogTableMessage("数据中心关键点|指标表格数据返回，无数据, 指标ID：" + String.Join("|", list.ToArray()));
                return true;
            }
            //LogUtility.LogTableMessage(String.Format("数据中心关键点|指标表格数据开始解析, 指标ID：{0}, 包大小：{1}", String.Join("|", list.ToArray()), bytes.Length));
            base.BaseDecoding(bytes, this._categoryStockCodeDict);
            //LogUtility.LogTableMessage("数据中心关键点|指标表格数据解析完毕, 指标ID：" + String.Join("|", list.ToArray()));
            return true;
        }

        public List<int> CustomerIndicatorIdList
        {
            get
            {
                return this._customerIndicatorIdList;
            }
        }

        public List<IndicatorEntity> IndicatorList
        {
            get
            {
                return this._indicatorList;
            }
        }

        public CommonEnumerators.IndicatorRequestType RequestDataType
        {
            get
            {
                return this._requestDataType;
            }
        }

        public List<StockEntity> StockList
        {
            get
            {
                return this._stockList;
            }
        }
    }
}
