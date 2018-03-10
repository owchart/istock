using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EmCore.Utils;
using EmCore;

namespace dataquery.indicator
{
    public enum RequestType
    {
        BlockQuoteSeriesData = 9,
        CustomIndicatorData = 5,
        FinanceSeriesData = 8,
        IndicatorCategory = 1,
        IndicatorData = 2,
        IndicatorEntity = 3,
        IndicatorEntityList = 6,
        IndicatorLeaf = 4,
        QuoteSeriesData = 7
    }

    public class DataPacketBase
    {
        private int _msgId;
        private static int _msgIdCounter;
        private byte _reserveFlag;
        public RequestType RequestId;

        public DataPacketBase()
        {
            if (_msgIdCounter == 0x7fffffff)
            {
                _msgIdCounter = 0;
            }
            this._msgId = _msgIdCounter;
            _msgIdCounter++;
        }

        protected void BaseDecoding(byte[] bytes, Dictionary<int, List<String>> categoryStockCodeDict)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    reader.ReadInt64();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        this.ExtractIndicatorTableData(reader, categoryStockCodeDict);
                    }
                }
            }
            categoryStockCodeDict.Clear();
        }

        public virtual String Coding()
        {
            return String.Empty;
        }

        public virtual bool Decoding(byte[] bytes)
        {
            return false;
        }

        protected List<KeyValuePair<String, DataType>> ExtractColumnDataType(BinaryReader br)
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

        private void ExtractIndicatorTableData(BinaryReader br, Dictionary<int, List<String>> categoryStockCodeDict)
        {
            long num = br.ReadInt64();
            if (num != 0L)
            {
                long num2 = num + br.BaseStream.Position;
                String tableName = br.ReadString();
                List<KeyValuePair<String, DataType>> fColumnsType = this.ExtractColumnDataType(br);
                while (br.BaseStream.Position < num2)
                {
                    this.ExtractSecuIndicator(br, tableName, fColumnsType);
                }
            }
        }

        private void ExtractSecuIndicator(BinaryReader br, String tableName, List<KeyValuePair<String, DataType>> FColumnsType)
        {
            String str = String.Empty;
            String str2 = String.Empty;
            for (int i = 0; i < FColumnsType.Count; i++)
            {
                String str3;
                if (i == 0)
                {
                    str2 = br.ReadString();
                    str = String.Format("{0}{1}", str2, tableName);
                }
                else
                {
                    KeyValuePair<String, DataType> pair = FColumnsType[i];
                    switch (pair.Value)
                    {
                        case DataType.Bool:
                            {
                                IndicatorTableDataCore.Int32IndicatorDict[str] = int.Parse(br.ReadBoolean().ToString());
                                continue;
                            }
                        case DataType.Byte:
                            {
                                IndicatorTableDataCore.Int32IndicatorDict[str] = br.ReadByte();
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
                                IndicatorTableDataCore.StrIndicatorDict[str] = br.ReadChar().ToString();
                                continue;
                            }
                        case DataType.UshortDate:
                            {
                                continue;
                            }
                        case DataType.DateTime:
                            {
                                DateTime time;
                                str3 = br.ReadString();
                                if (!DateTime.TryParse(str3, out time))
                                {
                                    goto Label_034A;
                                }
                                IndicatorTableDataCore.StrIndicatorDict[str] = time.ToString("yyyy-MM-dd");
                                continue;
                            }
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
                                continue;
                            }
                        case DataType.Double:
                            {
                                double num3 = br.ReadDouble();
                                if ((num3 != double.MinValue) && (num3 != double.MaxValue))
                                {
                                    IndicatorTableDataCore.DoubleIndicatorDict[str] = num3;
                                }
                                continue;
                            }
                        case DataType.Float:
                            {
                                float num2 = br.ReadSingle();
                                if ((num2 != float.MinValue) && (num2 != float.MaxValue))
                                {
                                    IndicatorTableDataCore.DoubleIndicatorDict[str] = num2;
                                }
                                continue;
                            }
                        case DataType.Int:
                            {
                                int num8 = br.ReadInt32();
                                if ((num8 != -2147483648) && (num8 != 0x7fffffff))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num8;
                                }
                                continue;
                            }
                        case DataType.Long:
                            {
                                long num9 = br.ReadInt64();
                                if ((num9 != -9223372036854775808L) && (num9 != 0x7fffffffffffffffL))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num9;
                                }
                                continue;
                            }
                        case DataType.Short:
                            {
                                short num7 = br.ReadInt16();
                                if ((num7 != -32768) && (num7 != 0x7fff))
                                {
                                    IndicatorTableDataCore.Int32IndicatorDict[str] = num7;
                                }
                                continue;
                            }
                        case DataType.String:
                            {
                                IndicatorTableDataCore.StrIndicatorDict[str] = br.ReadString();
                                continue;
                            }
                        case DataType.UInt:
                            {
                                int num11 = br.ReadInt32();
                                if ((num11 != -2147483648) && (num11 != 0x7fffffff))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num11;
                                }
                                continue;
                            }
                        case DataType.ULong:
                            {
                                long num12 = br.ReadInt64();
                                if ((num12 != -9223372036854775808L) && (num12 != 0x7fffffffffffffffL))
                                {
                                    IndicatorTableDataCore.LongIndicatorDict[str] = num12;
                                }
                                continue;
                            }
                        case DataType.UShort:
                            {
                                short num10 = br.ReadInt16();
                                if ((num10 != -32768) && (num10 != 0x7fff))
                                {
                                    IndicatorTableDataCore.Int32IndicatorDict[str] = num10;
                                }
                                continue;
                            }
                    }
                }
                continue;
            Label_034A:
                IndicatorTableDataCore.StrIndicatorDict[str] = str3;
            }
        }

        protected void ResolveObjectParameters(List<ParamterObject> indicatorParameterList)
        {
            String str = String.Empty;
            String str2 = String.Empty;
            foreach (ParamterObject obj2 in indicatorParameterList)
            {
                String str4;
                String str5;
                if ((!obj2.Type.Equals("106") && !obj2.Type.Equals("274")) && (!obj2.Type.Equals("275") && !obj2.Type.Equals("219")))
                {
                    goto Label_018E;
                }
                String name = String.Empty;
                try
                {
                    List<NameValue> list = JSONHelper.DeserializeObject<List<NameValue>>(obj2.DefaultValue.ToString());
                    if ((list != null) && (list.Count > 0))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].Selected)
                            {
                                name = list[i].Name;
                                break;
                            }
                        }
                        if (String.IsNullOrEmpty(name))
                        {
                            name = list[0].Name;
                        }
                    }
                }
                catch (Exception)
                {
                    name = obj2.DefaultValue.ToString();
                }
                String str7 = name;
                if (str7 == null)
                {
                    goto Label_0166;
                }
                if (!(str7 == "不复权"))
                {
                    if (str7 == "前复权")
                    {
                        goto Label_0146;
                    }
                    if (str7 == "后复权")
                    {
                        goto Label_014E;
                    }
                    if (str7 == "净价")
                    {
                        goto Label_0156;
                    }
                    if (str7 == "全价")
                    {
                        goto Label_015E;
                    }
                    goto Label_0166;
                }
                str = "1";
                goto Label_0168;
            Label_0146:
                str = "3";
                goto Label_0168;
            Label_014E:
                str = "2";
                goto Label_0168;
            Label_0156:
                str = "1";
                goto Label_0168;
            Label_015E:
                str = "2";
                goto Label_0168;
            Label_0166:
                str = name;
            Label_0168:
                if (!obj2.Type.Equals("219"))
                {
                    obj2.Name = "AdjustFlag";
                }
                obj2.DefaultValue = str;
                goto Label_01F7;
            Label_018E:
                if (CreateParameter.IsDropDownSet(obj2.Type))
                {
                    List<NameValue> list2 = JSONHelper.DeserializeObject<List<NameValue>>(obj2.DefaultValue.ToString());
                    if ((list2 != null) && (list2.Count > 0))
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            if (list2[j].Selected)
                            {
                                obj2.DefaultValue = list2[j].Value;
                                break;
                            }
                        }
                    }
                }
            Label_01F7:
                switch (obj2.Type.ToString())
                {
                    case "133":
                        {
                            str4 = obj2.DefaultValue.ToString();
                            if (!str4.Contains("一年定存"))
                            {
                                break;
                            }
                            obj2.DefaultValue = "1";
                            continue;
                        }
                    case "115":
                        {
                            if (!obj2.DefaultValue.ToString().Equals("合并"))
                            {
                                goto Label_0444;
                            }
                            obj2.DefaultValue = "1";
                            continue;
                        }
                    case "314":
                        {
                            if (!obj2.DefaultValue.Equals("调整前"))
                            {
                                goto Label_04DF;
                            }
                            obj2.DefaultValue = 1;
                            continue;
                        }
                    case "310":
                    case "121":
                    case "101":
                    case "100":
                    case "119":
                    case "267":
                    case "264":
                    case "102":
                    case "520":
                        {
                            str5 = obj2.DefaultValue.ToString();
                            if ((!str5.Contains("GetLatestDay") && !str5.Contains("GetLastestClosingDate")) && !str5.Contains("GetLatestPhase"))
                            {
                                goto Label_054C;
                            }
                            obj2.DefaultValue = "N";
                            continue;
                        }
                    case "217":
                        {
                            if (obj2.DefaultValue != null)
                            {
                                String[] strArray = obj2.DefaultValue.ToString().Split(new char[] { ':' });
                                if (strArray.Length == 2)
                                {
                                    obj2.DefaultValue = strArray[1];
                                    str2 = strArray[0];
                                }
                            }
                            continue;
                        }
                    case "444":
                        {
                            if (!String.IsNullOrEmpty(str2))
                            {
                                obj2.DefaultValue = str2;
                            }
                            continue;
                        }
                    case "297":
                        {
                            if (!obj2.DefaultValue.Equals("1"))
                            {
                                goto Label_0821;
                            }
                            obj2.DefaultValue = "30";
                            continue;
                        }
                    default:
                        {
                            continue;
                        }
                }
                if (str4.Contains("五年定存"))
                {
                    obj2.DefaultValue = "2";
                }
                else if (str4.Contains("银行间市场七日回购"))
                {
                    obj2.DefaultValue = "3";
                }
                else if (str4.Contains("最新发行的一年期"))
                {
                    obj2.DefaultValue = "4";
                }
                else if (str4.Contains("十年期国债收益"))
                {
                    obj2.DefaultValue = "5";
                }
                else if (str4.Contains("五年期国债收益"))
                {
                    obj2.DefaultValue = "6";
                }
                else if (str4.Contains("一年期国债收益"))
                {
                    obj2.DefaultValue = "7";
                }
                continue;
            Label_0444:
                if (obj2.DefaultValue.ToString().Equals("母公司"))
                {
                    obj2.DefaultValue = "2";
                }
                else if (obj2.DefaultValue.ToString().Equals("合并调整"))
                {
                    obj2.DefaultValue = "3";
                }
                else if (obj2.DefaultValue.ToString().Equals("母公司调整"))
                {
                    obj2.DefaultValue = "4";
                }
                continue;
            Label_04DF:
                if (obj2.DefaultValue.Equals("调整后"))
                {
                    obj2.DefaultValue = 2;
                }
                continue;
            Label_054C:
                if (str5.Contains("GetLastTradingDate"))
                {
                    obj2.DefaultValue = "P";
                }
                else if (str5.Contains("GetListingFirstDate"))
                {
                    obj2.DefaultValue = "S";
                }
                else if (str5.Contains("GetThisYearFirstDay"))
                {
                    obj2.DefaultValue = "YS";
                }
                else if (str5.Contains("GetThisMonthFirstDay"))
                {
                    obj2.DefaultValue = "MS";
                }
                else if (str5.Contains("GetTradingDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "EP";
                }
                else if (str5.Contains("GetOneWeekDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E1W";
                }
                else if (str5.Contains("GetTwoWeekDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E2W";
                }
                else if (str5.Contains("Get52WeekDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E52W";
                }
                else if (str5.Contains("GetOneMonthDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E1M";
                }
                else if (str5.Contains("GetThreeMonthDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E3M";
                }
                else if (str5.Contains("GetSixMonthDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E6M";
                }
                else if (str5.Contains("GetOneYearDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E1Y";
                }
                else if (str5.Contains("GetThreeYearDateBeforeClosingDate"))
                {
                    obj2.DefaultValue = "E3Y";
                }
                else if ((obj2.DefaultValue == null) || String.IsNullOrEmpty(obj2.DefaultValue.ToString()))
                {
                    obj2.DefaultValue = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime time;
                    if (DateTime.TryParse(obj2.DefaultValue.ToString(), out time))
                    {
                        obj2.DefaultValue = time.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        obj2.DefaultValue = str5.Substring(str5.LastIndexOf(".") + 1);
                        String toolsMethodInfo = String.Format("{0}.{1}", typeof(TimeParameter).FullName, obj2.DefaultValue);
                        obj2.DefaultValue = GetMethodInfo.GetToolsMethodValue(toolsMethodInfo);
                        if (DateTime.TryParse(obj2.DefaultValue.ToString(), out time))
                        {
                            obj2.DefaultValue = time.ToString("yyyy-MM-dd");
                        }
                    }
                }
                continue;
            Label_0821:
                if (obj2.DefaultValue.Equals("2"))
                {
                    obj2.DefaultValue = "90";
                }
                else if (obj2.DefaultValue.Equals("3"))
                {
                    obj2.DefaultValue = "180";
                }
            }
        }

        public int MsgId
        {
            get
            {
                return this._msgId;
            }
        }

        public byte ReserveFlag
        {
            get
            {
                return this._reserveFlag;
            }
            set
            {
                this._reserveFlag = value;
            }
        }
    }
}
