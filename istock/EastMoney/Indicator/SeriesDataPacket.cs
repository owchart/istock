using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EmCore.Utils;
using EmCore;

namespace dataquery.indicator
{
    public class SeriesDataPacket : DataPacketBase
    {
        protected Dictionary<String, Dictionary<int, Dictionary<DateTime, double>>> _allValueDictionary = new Dictionary<String, Dictionary<int, Dictionary<DateTime, double>>>();
        protected Dictionary<int, List<String>> _categoryStockCodeDict = new Dictionary<int, List<String>>();
        protected List<int> _customerIndicatorIdList;
        private Dictionary<DateTime, byte> _dateDict = new Dictionary<DateTime, byte>();
        private List<DateTime> _dateList = new List<DateTime>();
        protected List<IndicatorEntity> _indicatorList;
        protected Dictionary<int, double> _indicatorUnitDict = new Dictionary<int, double>();
        protected Dictionary<String, object> _paramsDictionary = new Dictionary<String, object>();
        protected CommonEnumerators.IndicatorRequestType _requestDataType;
        protected List<StockEntity> _stockList;

        protected Dictionary<String, object> BuildParamDictionary(String parameters)
        {
            Dictionary<String, object> dictionary = new Dictionary<String, object>();
            if (!String.IsNullOrEmpty(parameters) && !parameters.Trim().Equals("null"))
            {
                List<ParamterObject> indicatorParameterList = JSONHelper.DeserializeObject<List<ParamterObject>>(parameters);
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
                            dictionary["fieldname"] = obj2.DefaultValue;
                        }
                        else if (obj2.Name.Trim().Equals("type"))
                        {
                            String str = obj2.DefaultValue.ToString();
                            dictionary["type"] = str.Substring(str.Length - 1, 1);
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
                        dictionary["type"] = s;
                        continue;
                    }
                    dictionary[obj2.Name.Trim().ToLower()] = obj2.DefaultValue;
                }
            }
            return dictionary;
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
                //LogUtility.LogTableMessage("数据中心关键点|时序数据返回，无数据, 指标ID：" + String.Join("|", list.ToArray()));
                return true;
            }
            //LogUtility.LogTableMessage(String.Format("数据中心关键点|时序数据开始解析, 指标ID：{0}, 包大小：{1}", String.Join("|", list.ToArray()), bytes.Length));
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    reader.ReadInt64();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        this.ExtractIndicatorTableData(reader);
                    }
                }
            }
            if (this.IsQuoteSeries())
            {
                this.DateList.Clear();
                this.DateList.AddRange(this._dateDict.Keys);
            }
            //LogUtility.LogTableMessage("数据中心关键点|时序数据解析完毕, 指标ID：" + String.Join("|", list.ToArray()));
            return true;
        }

        private void ExtractIndicatorTableData(BinaryReader br)
        {
            long num = br.ReadInt64();
            if (num != 0L)
            {
                long num2 = num + br.BaseStream.Position;
                String s = br.ReadString();
                List<KeyValuePair<String, DataType>> columnsList = base.ExtractColumnDataType(br);
                int result = 1;
                int.TryParse(s, out result);
                while (br.BaseStream.Position < num2)
                {
                    this.ExtractSecuIndicator(br, result, columnsList);
                }
            }
        }

        private void ExtractSecuIndicator(BinaryReader br, int customerId, List<KeyValuePair<String, DataType>> columnsList)
        {
            String key = String.Empty;
            DateTime time = new DateTime();
            double maxValue = double.MaxValue;
            for (int i = 0; i < columnsList.Count; i++)
            {
                String str2;
                KeyValuePair<String, DataType> pair = columnsList[i];
                switch (pair.Value)
                {
                    case DataType.Bool:
                        {
                            br.ReadBoolean();
                            continue;
                        }
                    case DataType.Byte:
                        {
                            br.ReadByte();
                            continue;
                        }
                    case DataType.ByteArray:
                        {
                            int count = br.ReadInt32();
                            br.ReadBytes(count);
                            continue;
                        }
                    case DataType.Char:
                        {
                            br.ReadChar();
                            continue;
                        }
                    case DataType.UshortDate:
                        {
                            continue;
                        }
                    case DataType.DateTime:
                        {
                            time = Convert.ToDateTime(br.ReadString());
                            this._dateDict[time] = 1;
                            continue;
                        }
                    case DataType.Decimal:
                        {
                            decimal num4 = br.ReadDecimal();
                            if (!(num4 != -79228162514264337593543950335M) || !(num4 != 79228162514264337593543950335M))
                            {
                                goto Label_0163;
                            }
                            double num5 = (double)num4;
                            if (num5 == double.MinValue || num5 == double.MaxValue)
                            {
                                goto Label_0154;
                            }
                            maxValue = num5;
                            continue;
                        }
                    case DataType.Double:
                        {
                            double num3 = br.ReadDouble();
                            if (num3 == double.MinValue || num3 == double.MaxValue)
                            {
                                goto Label_00E7;
                            }
                            maxValue = num3;
                            continue;
                        }
                    case DataType.Float:
                        {
                            br.ReadSingle();
                            continue;
                        }
                    case DataType.Int:
                        {
                            int num7 = br.ReadInt32();
                            if ((num7 != -2147483648) && (num7 != 0x7fffffff))
                            {
                                maxValue = num7;
                            }
                            continue;
                        }
                    case DataType.Long:
                        {
                            long num8 = br.ReadInt64();
                            if ((num8 != 0x7fffffffffffffffL) && (num8 != -9223372036854775808L))
                            {
                                maxValue = num8;
                            }
                            continue;
                        }
                    case DataType.Short:
                        {
                            br.ReadInt16();
                            continue;
                        }
                    case DataType.String:
                        {
                            str2 = br.ReadString();
                            if (i != 0)
                            {
                                break;
                            }
                            key = str2;
                            continue;
                        }
                    case DataType.UInt:
                        {
                            br.ReadInt32();
                            continue;
                        }
                    case DataType.ULong:
                        {
                            br.ReadInt64();
                            continue;
                        }
                    case DataType.UShort:
                        {
                            br.ReadInt16();
                            continue;
                        }
                    default:
                        {
                            continue;
                        }
                }
                if (i == 1)
                {
                    time = DateTime.Parse(str2);
                    this._dateDict[time] = 1;
                }
                continue;
            Label_00E7:
                maxValue = double.MaxValue;
                continue;
            Label_0154:
                maxValue = double.MaxValue;
                continue;
            Label_0163:
                maxValue = double.MaxValue;
            }
            if (!this._allValueDictionary.ContainsKey(key))
            {
                this._allValueDictionary[key] = new Dictionary<int, Dictionary<DateTime, double>>();
            }
            if (!this._allValueDictionary[key].ContainsKey(customerId))
            {
                this._allValueDictionary[key][customerId] = new Dictionary<DateTime, double>();
            }
            if (maxValue == double.MaxValue)
            {
                this._allValueDictionary[key][customerId][time] = maxValue;
            }
            else
            {
                this._allValueDictionary[key][customerId][time] = maxValue / this._indicatorUnitDict[customerId];
            }
        }

        public List<double> GetValueList(String stockCode, int customerId, List<DateTime> dateList)
        {
            List<double> list = new List<double>();
            if (this._allValueDictionary.ContainsKey(stockCode))
            {
                if (!this._allValueDictionary[stockCode].ContainsKey(customerId))
                {
                    return list;
                }
                foreach (DateTime time in dateList)
                {
                    if (this._allValueDictionary[stockCode][customerId].ContainsKey(time))
                    {
                        double d = this._allValueDictionary[stockCode][customerId][time];
                        if (((d == double.MaxValue) || double.IsInfinity(d)) || double.IsNaN(d))
                        {
                            list.Add(double.MaxValue);
                        }
                        else
                        {
                            list.Add(d);
                        }
                        continue;
                    }
                    list.Add(double.MaxValue);
                }
            }
            return list;
        }

        protected virtual bool IsQuoteSeries()
        {
            return false;
        }

        public List<int> CustomerIndicatorIdList
        {
            get
            {
                return this._customerIndicatorIdList;
            }
        }

        public List<DateTime> DateList
        {
            get
            {
                return this._dateList;
            }
        }

        public List<IndicatorEntity> IndicatorList
        {
            get
            {
                return this._indicatorList;
            }
        }

        public Dictionary<String, object> ParamDictionary
        {
            get
            {
                return this._paramsDictionary;
            }
            set
            {
                this._paramsDictionary = value;
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
