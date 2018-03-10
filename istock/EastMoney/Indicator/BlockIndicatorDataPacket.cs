using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EmCore.Utils;
using EmCore;

namespace OwLib
{
    public class BlockIndicatorDataPacket : DataPacketBase
    {
        private List<String> _blockList;
        private BlockIndicatorParams _blockParam;
        private List<int> _customerIndicatorIdList;
        private List<IndicatorEntity> _indicatorList;
        private CommonEnumerators.IndicatorRequestType _requestDataType;

        public BlockIndicatorDataPacket(CommonEnumerators.IndicatorRequestType requestDataType, List<String> blockCodeList, List<int> customerIndicatorIdList, BlockIndicatorParams blockParam)
        {
            base.RequestId = RequestType.BlockQuoteSeriesData;
            this._blockList = blockCodeList;
            this._customerIndicatorIdList = new List<int>();
            this._blockParam = blockParam;
            this._requestDataType = requestDataType;
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
            List<String> list = new List<String>();
            foreach (IndicatorEntity entity in this._indicatorList)
            {
                list.Add(entity.IndicatorCode);
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("$-fun");
            builder.AppendLine(String.Format("$name={0}", String.Join(",", list.ToArray())));
            builder.AppendLine("$secucode=");
            foreach (IndicatorEntity entity2 in this._indicatorList)
            {
                String str = String.Format("$TableName={0},StartDate={1},EndDate={2},Period={3},IsHistory=0,publishcode={4}", new object[] { entity2.CustomerId, this._blockParam.StartDate.ToString("yyyy-MM-dd"), this._blockParam.EndDate.ToString("yyyy-MM-dd"), ((int)this._blockParam.Cycle) + 1, String.Join(",", this._blockList.ToArray()) });
                List<ParamterObject> indicatorParameterList = JSONHelper.DeserializeObject<List<ParamterObject>>(entity2.Parameters);
                base.ResolveObjectParameters(indicatorParameterList);
                List<String> list3 = new List<String>();
                foreach (ParamterObject obj2 in indicatorParameterList)
                {
                    if (!obj2.IsHide && !obj2.Type.Equals("261"))
                    {
                        if (String.IsNullOrEmpty(obj2.Name))
                        {
                            if (obj2.Name.Trim().Equals("type"))
                            {
                                String str2 = obj2.DefaultValue.ToString();
                                list3.Add(String.Format("type={0}", str2.Substring(str2.Length - 1, 1)));
                            }
                        }
                        else
                        {
                            list3.Add(String.Format("{0}={1}", obj2.Name.Trim(), obj2.DefaultValue));
                        }
                    }
                }
                builder.AppendLine(str + "," + String.Join(",", list3.ToArray()));
            }
            return builder.ToString().TrimEnd(new char[0]);
        }

        public override bool Decoding(byte[] bytes)
        {
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
            return true;
        }

        private void ExtractIndicatorTableData(BinaryReader br)
        {
            long num = br.ReadInt64();
            if (num != 0L)
            {
                long num2 = num + br.BaseStream.Position;
                int customerId = int.Parse(br.ReadString());
                List<KeyValuePair<String, DataType>> columnsType = base.ExtractColumnDataType(br);
                while (br.BaseStream.Position < num2)
                {
                    this.ExtractSecuIndicator(br, customerId, columnsType);
                }
            }
        }

        private void ExtractSecuIndicator(BinaryReader br, int customerId, List<KeyValuePair<String, DataType>> columnsType)
        {
            String blockCode = String.Empty;
            DateTime result = new DateTime();
            double v = 0.0;
            for (int i = 0; i < columnsType.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        blockCode = br.ReadString();
                        break;

                    case 1:
                        DateTime.TryParse(br.ReadString(), out result);
                        break;

                    default:
                        {
                            KeyValuePair<String, DataType> pair = columnsType[i];
                            switch (pair.Value)
                            {
                                case DataType.Byte:
                                    v = br.ReadByte();
                                    break;

                                case DataType.Decimal:
                                    {
                                        decimal num5 = br.ReadDecimal();
                                        if ((num5 != -79228162514264337593543950335M) && (num5 != 79228162514264337593543950335M))
                                        {
                                            double num6 = (double)num5;
                                            if ((num6 != double.MinValue) && (num6 != double.MaxValue))
                                            {
                                                v = num6;
                                            }
                                        }
                                        break;
                                    }
                                case DataType.Double:
                                    {
                                        double num4 = br.ReadDouble();
                                        if ((num4 != double.MinValue) && (num4 != double.MaxValue))
                                        {
                                            v = num4;
                                        }
                                        break;
                                    }
                                case DataType.Float:
                                    {
                                        float num3 = br.ReadSingle();
                                        if ((num3 != float.MinValue) && (num3 != float.MaxValue))
                                        {
                                            v = num3;
                                        }
                                        break;
                                    }
                                case DataType.Int:
                                    {
                                        int num8 = br.ReadInt32();
                                        if ((num8 != -2147483648) && (num8 != 0x7fffffff))
                                        {
                                            v = num8;
                                        }
                                        break;
                                    }
                                case DataType.Long:
                                    {
                                        long num9 = br.ReadInt64();
                                        if ((num9 != -9223372036854775808L) && (num9 != 0x7fffffffffffffffL))
                                        {
                                            v = num9;
                                        }
                                        break;
                                    }
                                case DataType.Short:
                                    {
                                        short num7 = br.ReadInt16();
                                        if ((num7 != -32768) && (num7 != 0x7fff))
                                        {
                                            v = num7;
                                        }
                                        break;
                                    }
                                case DataType.UInt:
                                    {
                                        int num11 = br.ReadInt32();
                                        if ((num11 != -2147483648) && (num11 != 0x7fffffff))
                                        {
                                            v = num11;
                                        }
                                        break;
                                    }
                                case DataType.ULong:
                                    {
                                        long num12 = br.ReadInt64();
                                        if ((num12 != -9223372036854775808L) && (num12 != 0x7fffffffffffffffL))
                                        {
                                            v = num12;
                                        }
                                        break;
                                    }
                                case DataType.UShort:
                                    {
                                        short num10 = br.ReadInt16();
                                        if ((num10 != -32768) && (num10 != 0x7fff))
                                        {
                                            v = num10;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            BlockSeriesDataCore.SetValue(blockCode, customerId, result, this.BlockParam.Cycle, v);
        }

        public List<String> BlockList
        {
            get
            {
                return this._blockList;
            }
        }

        public BlockIndicatorParams BlockParam
        {
            get
            {
                return this._blockParam;
            }
            set
            {
                this._blockParam = value;
            }
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
    }
}
