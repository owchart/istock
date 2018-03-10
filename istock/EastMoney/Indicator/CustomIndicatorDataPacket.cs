using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EmCore.Utils;
using EmCore;

namespace OwLib
{
    public class CustomIndicatorDataPacket : DataPacketBase
    {
        private Dictionary<int, List<String>> _categoryStockCodeDict;
        private Dictionary<int, IndicatorEntity> _customIndicatorDictionary;
        private List<IndicatorEntity> _indicatorList;
        private CommonEnumerators.IndicatorRequestType _requestDataType;
        private List<StockEntity> _stockList;

        public CustomIndicatorDataPacket(CommonEnumerators.IndicatorRequestType requestDataType, List<StockEntity> stockList, List<IndicatorEntity> indicatorList)
        {
            this._categoryStockCodeDict = new Dictionary<int, List<String>>();
            base.RequestId = RequestType.CustomIndicatorData;
            this._requestDataType = requestDataType;
            this._indicatorList = new List<IndicatorEntity>();
            this._indicatorList.AddRange(indicatorList);
            this._stockList = stockList;
        }

        public CustomIndicatorDataPacket(CommonEnumerators.IndicatorRequestType requestDatType, List<StockEntity> stockList, List<int> customerIndicatorIdList)
        {
            this._categoryStockCodeDict = new Dictionary<int, List<String>>();
            base.RequestId = RequestType.IndicatorData;
            this._requestDataType = requestDatType;
            this._stockList = stockList;
            this._indicatorList = new List<IndicatorEntity>();
            foreach (int num in customerIndicatorIdList)
            {
                IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(num);
                indicatorEntityByCustomerId.CustomerId = num;
                this._indicatorList.Add(indicatorEntityByCustomerId);
            }
        }

        public override String Coding()
        {
            this._customIndicatorDictionary = new Dictionary<int, IndicatorEntity>();
            foreach (IndicatorEntity entity in this.IndicatorList)
            {
                if (((entity.CustomIndicator == null) || (entity.CustomIndicator.IndicatorList == null)) || (entity.CustomIndicator.IndicatorList.Count == 0))
                {
                    this._customIndicatorDictionary[entity.CustomerId] = entity;
                }
                else
                {
                    foreach (IndicatorEntity entity2 in entity.CustomIndicator.IndicatorList)
                    {
                        this._customIndicatorDictionary[entity2.CustomerId] = entity2;
                    }
                }
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("$-{0}{1}", CommonEnumerators.GetIndicatorRequestTypeCmd(this._requestDataType), Environment.NewLine);
            List<String> list = new List<String>();
            List<String> list2 = new List<String>();
            foreach (KeyValuePair<int, IndicatorEntity> pair in this._customIndicatorDictionary)
            {
                IndicatorEntity entity3 = pair.Value;
                list.Add(entity3.IndicatorCode);
                List<String> list3 = new List<String>();
                list3.Add(String.Format("$TableName={0}", entity3.CustomerId));
                if (String.IsNullOrEmpty(entity3.Parameters) || entity3.Parameters.Equals("null"))
                {
                    list2.Add(String.Join(",", list3.ToArray()));
                    continue;
                }
                List<ParamterObject> indicatorParameterList = JSONHelper.DeserializeObject<List<ParamterObject>>(entity3.Parameters);
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
                list2.Add(String.Join(",", list3.ToArray()));
            }
            builder.AppendFormat("$name={0}{1}", String.Join(",", list.ToArray()), Environment.NewLine);
            foreach (StockEntity entity4 in this._stockList)
            {
                if (!this._categoryStockCodeDict.ContainsKey(entity4.CategoryCode))
                {
                    this._categoryStockCodeDict[entity4.CategoryCode] = new List<String>();
                }
                this._categoryStockCodeDict[entity4.CategoryCode].Add(entity4.StockCode);
            }
            List<String> list5 = new List<String>();
            foreach (KeyValuePair<int, List<String>> pair2 in this._categoryStockCodeDict)
            {
                list5.Add(String.Format("{0}:{1}", pair2.Key, String.Join(",", pair2.Value.ToArray())));
            }
            builder.AppendFormat("$secucode={0}{1}", String.Join("#", list5.ToArray()), Environment.NewLine);
            builder.Append(String.Join(Environment.NewLine, list2.ToArray()));
            return builder.ToString();
        }

        public override bool Decoding(byte[] bytes)
        {
            List<String> list = new List<String>();
            foreach (int num in this._customIndicatorDictionary.Keys)
            {
                list.Add(num.ToString());
            }
            if ((bytes != null) && (bytes.Length == 8))
            {
                //LogUtility.LogTableMessage("数据中心关键点|指标表格数据返回，无数据, 指标ID：" + String.Join("|", list.ToArray()));
                return true;
            }
            //LogUtility.LogTableMessage("数据中心关键点|指标表格数据开始解析, 指标ID：" + String.Join("|", list.ToArray()));
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
            this._categoryStockCodeDict.Clear();
            //LogUtility.LogTableMessage("数据中心关键点|指标表格数据解析完毕, 指标ID：" + String.Join("|", list.ToArray()));
            return true;
        }

        private List<KeyValuePair<String, DataType>> ExtractColumnDataType(BinaryReader br)
        {
            long num = br.ReadInt16();
            long num2 = 0L;
            List<KeyValuePair<String, DataType>> list = new List<KeyValuePair<String, DataType>>();
            long position = br.BaseStream.Position;
            while (num2 < num)
            {
                String key = br.ReadString();
                int num4 = br.ReadByte();
                DataType type = (DataType)Enum.Parse(typeof(DataType), num4.ToString(), true);
                list.Add(new KeyValuePair<String, DataType>(key, type));
                num2 = br.BaseStream.Position - position;
            }
            return list;
        }

        private void ExtractIndicatorTableData(BinaryReader br)
        {
            long num = br.ReadInt64();
            if (num != 0L)
            {
                long num2 = num + br.BaseStream.Position;
                String tableName = br.ReadString();
                List<KeyValuePair<String, DataType>> fColumnsType = this.ExtractColumnDataType(br);
                foreach (KeyValuePair<int, List<String>> pair in this._categoryStockCodeDict)
                {
                    foreach (String str2 in pair.Value)
                    {
                        if (br.BaseStream.Position < num2)
                        {
                            this.ExtractSecuIndicator(br, str2, tableName, fColumnsType);
                        }
                    }
                }
            }
        }

        private void ExtractSecuIndicator(BinaryReader br, String stockCode, String tableName, List<KeyValuePair<String, DataType>> fColumnsType)
        {
            String str = String.Empty;
            for (int i = 0; i < fColumnsType.Count; i++)
            {
                if (i == 0)
                {
                    stockCode = br.ReadString();
                    str = String.Format("{0}{1}", stockCode, tableName);
                }
                else
                {
                    KeyValuePair<String, DataType> pair = fColumnsType[i];
                    switch (pair.Value)
                    {
                        case DataType.Bool:
                            IndicatorTableDataCore.Int32IndicatorDict[str] = int.Parse(br.ReadBoolean().ToString());
                            break;

                        case DataType.Byte:
                            IndicatorTableDataCore.Int32IndicatorDict[str] = br.ReadByte();
                            break;

                        case DataType.ByteArray:
                            {
                                int count = br.ReadInt32();
                                br.ReadBytes(count);
                                break;
                            }
                        case DataType.Char:
                            IndicatorTableDataCore.StrIndicatorDict[str] = br.ReadChar().ToString();
                            break;

                        case DataType.DateTime:
                            IndicatorTableDataCore.StrIndicatorDict[str] = br.ReadString();
                            break;

                        case DataType.Decimal:
                            {
                                decimal num4 = br.ReadDecimal();
                                if ((num4 != -79228162514264337593543950335M) && (num4 != 79228162514264337593543950335M))
                                {
                                    double num5 = (double)num4;
                                    if ((num5 != double.MinValue) && (num5 != double.MaxValue))
                                    {
                                        IndicatorTableDataCore.DoubleIndicatorDict[str] = num5;
                                    }
                                }
                                break;
                            }
                        case DataType.Double:
                            {
                                double num3 = br.ReadDouble();
                                if ((num3 != double.MinValue) && (num3 != double.MaxValue))
                                {
                                    IndicatorTableDataCore.DoubleIndicatorDict[str] = num3;
                                }
                                break;
                            }
                        case DataType.Float:
                            {
                                float num2 = br.ReadSingle();
                                if ((num2 != float.MinValue) && (num2 != float.MaxValue))
                                {
                                    IndicatorTableDataCore.DoubleIndicatorDict[str] = num2;
                                }
                                break;
                            }
                        case DataType.Int:
                            {
                                int num8 = br.ReadInt32();
                                if ((num8 != -2147483648) && (num8 != 0x7fffffff))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num8;
                                }
                                break;
                            }
                        case DataType.Long:
                            {
                                long num9 = br.ReadInt64();
                                if ((num9 != -9223372036854775808L) && (num9 != 0x7fffffffffffffffL))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num9;
                                }
                                break;
                            }
                        case DataType.Short:
                            {
                                short num7 = br.ReadInt16();
                                if ((num7 != -32768) && (num7 != 0x7fff))
                                {
                                    IndicatorTableDataCore.Int32IndicatorDict[str] = num7;
                                }
                                break;
                            }
                        case DataType.String:
                            IndicatorTableDataCore.StrIndicatorDict[str] = br.ReadString();
                            break;

                        case DataType.UInt:
                            {
                                int num11 = br.ReadInt32();
                                if ((num11 != -2147483648) && (num11 != 0x7fffffff))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num11;
                                }
                                break;
                            }
                        case DataType.ULong:
                            {
                                long num12 = br.ReadInt64();
                                if ((num12 != -9223372036854775808L) && (num12 != 0x7fffffffffffffffL))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num12;
                                }
                                break;
                            }
                        case DataType.UShort:
                            {
                                short num10 = br.ReadInt16();
                                if ((num10 != -32768) && (num10 != 0x7fff))
                                {
                                    IndicatorTableDataCore.Int32IndicatorDict[str] = num10;
                                }
                                break;
                            }
                    }
                }
            }
        }

        public List<IndicatorEntity> IndicatorList
        {
            get
            {
                return this._indicatorList;
            }
            set
            {
                this._indicatorList = value;
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
