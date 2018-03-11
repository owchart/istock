
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using EmSerDSComm;
using EmSerDataService;
using Newtonsoft.Json;

namespace OwLib
{

    #region 实时

    public class ResLogonCftDataPacket : RealTimeDataPacket
    {
        protected override bool DecodingBody(BinaryReader br)
        {
            br.ReadByte();
            br.ReadByte();
            br.ReadByte();
            DataQuery.IsQuoteLogined = true;
            return true;
        }
    }

    /// <summary>
    /// 心跳包响应
    /// </summary>
    public class ResHeartDataPacket : RealTimeDataPacket
    {
        private int _date;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date
        {
            get { return _date; }
            private set { this._date = value; }
        }

        private int _time;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time
        {
            get { return _time; }
            private set { this._time = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(System.IO.BinaryReader br)
        {
            Date = br.ReadInt32();
            Time = br.ReadInt32();
            return true;
        }
    }

    /// <summary>
    /// 个股Detail行情
    /// </summary>
    public class ResStockDetailDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(System.IO.BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');

            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];
            market = br.ReadByte();

            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }

            int time = CTimeToDateTimeInt(br.ReadInt32());
            fieldInt32[FieldIndex.Time] = time;

            byte[] codeBytes2 = br.ReadBytes(7);
            byte[] nameByte = br.ReadBytes(9);

            byte type = br.ReadByte();
            fieldSingle[FieldIndex.PreClose] = br.ReadSingle();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = br.ReadSingle();
            fieldDouble[FieldIndex.Amount] = br.ReadDouble();
            fieldInt64[FieldIndex.Volume] = (long)br.ReadUInt32();

            if (fieldInt64[FieldIndex.Volume] != 0 && fieldDouble[FieldIndex.Amount] != 0)
                fieldSingle[FieldIndex.AveragePrice] = Convert.ToSingle(fieldDouble[FieldIndex.Amount] / fieldInt64[FieldIndex.Volume]);

            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            int buyVolume1 = br.ReadInt32();
            int oldBuyVolume1 = FindOldVolume(Code, fieldSingle[FieldIndex.BuyPrice1], true);
            if (oldBuyVolume1 != 0)
                fieldInt32[FieldIndex.BuyVolume1Delta] = buyVolume1 - oldBuyVolume1;
            fieldInt32[FieldIndex.BuyVolume1] = buyVolume1;

            fieldSingle[FieldIndex.BuyPrice2] = br.ReadSingle();
            int buyVolume2 = br.ReadInt32();
            int oldBuyVolume2 = FindOldVolume(Code, fieldSingle[FieldIndex.BuyPrice2], true);
            if (oldBuyVolume2 != 0)
                fieldInt32[FieldIndex.BuyVolume2Delta] = buyVolume2 - oldBuyVolume2;
            fieldInt32[FieldIndex.BuyVolume2] = buyVolume2;

            fieldSingle[FieldIndex.BuyPrice3] = br.ReadSingle();
            int buyVolume3 = br.ReadInt32();
            int oldBuyVolume3 = FindOldVolume(Code, fieldSingle[FieldIndex.BuyPrice3], true);
            if (oldBuyVolume3 != 0)
                fieldInt32[FieldIndex.BuyVolume3Delta] = buyVolume3 - oldBuyVolume3;
            fieldInt32[FieldIndex.BuyVolume3] = buyVolume3;

            fieldSingle[FieldIndex.BuyPrice4] = br.ReadSingle();
            int buyVolume4 = br.ReadInt32();
            int oldBuyVolume4 = FindOldVolume(Code, fieldSingle[FieldIndex.BuyPrice4], true);
            if (oldBuyVolume4 != 0)
                fieldInt32[FieldIndex.BuyVolume4Delta] = buyVolume4 - oldBuyVolume4;
            fieldInt32[FieldIndex.BuyVolume4] = buyVolume4;

            fieldSingle[FieldIndex.BuyPrice5] = br.ReadSingle();
            int buyVolume5 = br.ReadInt32();
            int oldBuyVolume5 = FindOldVolume(Code, fieldSingle[FieldIndex.BuyPrice5], true);
            if (oldBuyVolume5 != 0)
                fieldInt32[FieldIndex.BuyVolume5Delta] = buyVolume5 - oldBuyVolume5;
            fieldInt32[FieldIndex.BuyVolume5] = buyVolume5;

            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            int sellVolume1 = br.ReadInt32();
            int oldSellVolume1 = FindOldVolume(Code, fieldSingle[FieldIndex.SellPrice1], false);
            if (oldSellVolume1 != 0)
                fieldInt32[FieldIndex.SellVolume1Delta] = sellVolume1 - oldSellVolume1;
            fieldInt32[FieldIndex.SellVolume1] = sellVolume1;

            fieldSingle[FieldIndex.SellPrice2] = br.ReadSingle();
            int sellVolume2 = br.ReadInt32();
            int oldSellVolume2 = FindOldVolume(Code, fieldSingle[FieldIndex.SellPrice2], false);
            if (oldSellVolume2 != 0)
                fieldInt32[FieldIndex.SellVolume2Delta] = sellVolume2 - oldSellVolume2;
            fieldInt32[FieldIndex.SellVolume2] = sellVolume2;

            fieldSingle[FieldIndex.SellPrice3] = br.ReadSingle();
            int sellVolume3 = br.ReadInt32();
            int oldSellVolume3 = FindOldVolume(Code, fieldSingle[FieldIndex.SellPrice3], false);
            if (oldSellVolume3 != 0)
                fieldInt32[FieldIndex.SellVolume3Delta] = sellVolume3 - oldSellVolume3;
            fieldInt32[FieldIndex.SellVolume3] = sellVolume3;

            fieldSingle[FieldIndex.SellPrice4] = br.ReadSingle();
            int sellVolume4 = br.ReadInt32();
            int oldSellVolume4 = FindOldVolume(Code, fieldSingle[FieldIndex.SellPrice4], false);
            if (oldSellVolume4 != 0)
                fieldInt32[FieldIndex.SellVolume4Delta] = sellVolume4 - oldSellVolume4;
            fieldInt32[FieldIndex.SellVolume4] = sellVolume4;

            fieldSingle[FieldIndex.SellPrice5] = br.ReadSingle();
            int sellVolume5 = br.ReadInt32();
            int oldSellVolume5 = FindOldVolume(Code, fieldSingle[FieldIndex.SellPrice5], false);
            if (oldSellVolume5 != 0)
                fieldInt32[FieldIndex.SellVolume5Delta] = sellVolume5 - oldSellVolume5;
            fieldInt32[FieldIndex.SellVolume5] = sellVolume5;

            fieldSingle[FieldIndex.Difference] = br.ReadSingle();
            fieldSingle[FieldIndex.DifferRange] = br.ReadSingle();
            fieldSingle[FieldIndex.Delta] = br.ReadSingle() / 100.0f;
            fieldSingle[FieldIndex.Turnover] = br.ReadSingle() / 100.0f;
            fieldSingle[FieldIndex.PE] = br.ReadSingle();
            fieldSingle[FieldIndex.VolumeRatio] = br.ReadSingle();
            fieldInt32[FieldIndex.Evg5Volume] = br.ReadInt32();
            fieldInt32[FieldIndex.LastVolume] = br.ReadInt32();
            fieldInt32[FieldIndex.GreenVolume] = br.ReadInt32();
            fieldInt32[FieldIndex.RedVolume] = br.ReadInt32();
            fieldDouble[FieldIndex.Ltg] = br.ReadSingle();
            fieldSingle[FieldIndex.DifferRange3D] = br.ReadSingle();
            fieldSingle[FieldIndex.DifferRange6D] = br.ReadSingle();
            fieldSingle[FieldIndex.Turnover3D] = br.ReadSingle();
            fieldSingle[FieldIndex.Turnover6D] = br.ReadSingle();

            return true;
        }

        public static int FindOldVolume(int code, float price, bool isBuy)
        {
            if (price == 0)
                return 0;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            if (!DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                return 0;
            if (!DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                return 0;

            if (isBuy)
            {
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice1) && fieldSingle[FieldIndex.BuyPrice1] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume1))
                    return fieldInt32[FieldIndex.BuyVolume1];
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice2) && fieldSingle[FieldIndex.BuyPrice2] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume2))
                    return fieldInt32[FieldIndex.BuyVolume2];
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice3) && fieldSingle[FieldIndex.BuyPrice3] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume3))
                    return (fieldInt32[FieldIndex.BuyVolume3]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice4) && fieldSingle[FieldIndex.BuyPrice4] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume4))
                    return (fieldInt32[FieldIndex.BuyVolume4]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice5) && fieldSingle[FieldIndex.BuyPrice5] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume5))
                    return (fieldInt32[FieldIndex.BuyVolume5]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice6) && fieldSingle[FieldIndex.BuyPrice6] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume6))
                    return (fieldInt32[FieldIndex.BuyVolume6]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice7) && fieldSingle[FieldIndex.BuyPrice7] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume7))
                    return (fieldInt32[FieldIndex.BuyVolume7]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice8) && fieldSingle[FieldIndex.BuyPrice8] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume8))
                    return (fieldInt32[FieldIndex.BuyVolume8]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice9) && fieldSingle[FieldIndex.BuyPrice9] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume9))
                    return (fieldInt32[FieldIndex.BuyVolume9]);
                if (fieldSingle.ContainsKey(FieldIndex.BuyPrice10) && fieldSingle[FieldIndex.BuyPrice10] == price && fieldInt32.ContainsKey(FieldIndex.BuyVolume10))
                    return (fieldInt32[FieldIndex.BuyVolume10]);
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice1) && (fieldSingle[FieldIndex.SellPrice1]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume1))
                    return (fieldInt32[FieldIndex.SellVolume1]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice2) && (fieldSingle[FieldIndex.SellPrice2]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume2))
                    return (fieldInt32[FieldIndex.SellVolume2]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice3) && (fieldSingle[FieldIndex.SellPrice3]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume3))
                    return (fieldInt32[FieldIndex.SellVolume3]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice4) && (fieldSingle[FieldIndex.SellPrice4]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume4))
                    return (fieldInt32[FieldIndex.SellVolume4]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice5) && (fieldSingle[FieldIndex.SellPrice5]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume5))
                    return (fieldInt32[FieldIndex.SellVolume5]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice6) && (fieldSingle[FieldIndex.SellPrice6]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume6))
                    return (fieldInt32[FieldIndex.SellVolume6]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice7) && (fieldSingle[FieldIndex.SellPrice7]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume7))
                    return (fieldInt32[FieldIndex.SellVolume7]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice8) && (fieldSingle[FieldIndex.SellPrice8]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume8))
                    return (fieldInt32[FieldIndex.SellVolume8]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice9) && (fieldSingle[FieldIndex.SellPrice9]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume9))
                    return (fieldInt32[FieldIndex.SellVolume9]);
                if (fieldSingle.ContainsKey(FieldIndex.SellPrice10) && (fieldSingle[FieldIndex.SellPrice10]) == price && fieldInt32.ContainsKey(FieldIndex.SellVolume10))
                    return (fieldInt32[FieldIndex.SellVolume10]);
            }
            return 0;
        }
    }

    /// <summary>
    /// 个股Detail Level2行情
    /// </summary>
    public class ResStockDetailLev2DataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte num = br.ReadByte();
            String text = "";
            for (int i = 0; i < num; i++)
            {
                byte num3 = br.ReadByte();
                int fieldValue = DataPacket.CTimeToDateTimeInt(br.ReadInt32());
                byte[] bytes = br.ReadBytes(7);
                string str = Encoding.ASCII.GetString(bytes).TrimEnd(new char[1]);
                if (string.IsNullOrEmpty(str))
                {
                    this.Code = 0;
                    return false;
                }
                string emCode = DataPacket.GetEmCode((ReqMarketType)num3, str);
                if ((emCode == null) || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                {
                    return false;
                }
                this.Code = DetailData.EmCodeToUnicode[emCode];
                Dictionary<FieldIndex, int> fieldInt32;
                Dictionary<FieldIndex, float> fieldSingle;
                Dictionary<FieldIndex, long> fieldInt64;
                Dictionary<FieldIndex, double> fieldDouble;

                if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[Code] = fieldInt32;
                }
                if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
                {
                    fieldInt64 = new Dictionary<FieldIndex, long>(1);
                    DetailData.FieldIndexDataInt64[Code] = fieldInt64;
                }
                if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[Code] = fieldSingle;
                }
                if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
                {
                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                    DetailData.FieldIndexDataDouble[Code] = fieldDouble;
                }
                MarketType marketType = DllImportHelper.GetMarketType(this.Code);
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.Time, fieldValue);
                br.ReadBytes(9);
                br.ReadByte();
                float num6 = br.ReadSingle();
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.PreClose, num6);
                float num7 = br.ReadSingle();
                float num8 = br.ReadSingle();
                float num9 = br.ReadSingle();
                float num10 = br.ReadSingle();
                double amount = br.ReadDouble();
                long volume = br.ReadUInt32();
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Open, num7);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.High, num8);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Low, num9);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Now, num10);
                float num13 = br.ReadSingle();
                int num14 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume1, num14);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice1, num13);
                int num15 = ResStockDetailDataPacket.FindOldVolume(this.Code, num13, true);
                if (num15 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume1Delta, num14 - num15);
                }
                float num16 = br.ReadSingle();
                int num17 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume2, num17);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice2, num16);
                int num18 = ResStockDetailDataPacket.FindOldVolume(this.Code, num16, true);
                if (num18 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume1Delta, num17 - num18);
                }
                float num19 = br.ReadSingle();
                int num20 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume3, num20);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice3, num19);
                int num21 = ResStockDetailDataPacket.FindOldVolume(this.Code, num19, true);
                if (num21 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume3Delta, num20 - num21);
                }
                float num22 = br.ReadSingle();
                int num23 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume4, num23);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice4, num22);
                int num24 = ResStockDetailDataPacket.FindOldVolume(this.Code, num22, true);
                if (num24 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume4Delta, num23 - num24);
                }
                float num25 = br.ReadSingle();
                int num26 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume5, num26);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice5, num25);
                int num27 = ResStockDetailDataPacket.FindOldVolume(this.Code, num25, true);
                if (num27 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume5Delta, num26 - num27);
                }
                float num28 = br.ReadSingle();
                int num29 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume1, num29);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice1, num28);
                int num30 = ResStockDetailDataPacket.FindOldVolume(this.Code, num28, false);
                if (num30 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume1Delta, num29 - num30);
                }
                float num31 = br.ReadSingle();
                int num32 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume2, num32);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice2, num31);
                int num33 = ResStockDetailDataPacket.FindOldVolume(this.Code, num31, false);
                if (num33 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume2Delta, num32 - num33);
                }
                float num34 = br.ReadSingle();
                int num35 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume3, num35);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice3, num34);
                int num36 = ResStockDetailDataPacket.FindOldVolume(this.Code, num34, false);
                if (num36 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume3Delta, num35 - num36);
                }
                float num37 = br.ReadSingle();
                int num38 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume4, num38);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice4, num37);
                int num39 = ResStockDetailDataPacket.FindOldVolume(this.Code, num37, false);
                if (num39 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume4Delta, num38 - num39);
                }
                float num40 = br.ReadSingle();
                int num41 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume5, num41);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice5, num40);
                int num42 = ResStockDetailDataPacket.FindOldVolume(this.Code, num40, false);
                if (num42 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume5Delta, num41 - num42);
                }
                float num43 = br.ReadSingle();
                float num44 = br.ReadSingle();
                if (DllImportHelper.EqualZero(num6))
                {
                    num43 = num10 - num7;
                    if (!DllImportHelper.EqualZero(num7))
                    {
                        num44 = (num10 - num7) / num7;
                    }
                }
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Difference, num43);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.DifferRange, num44);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Delta, br.ReadSingle() / 100f);
                float num45 = br.ReadSingle();
                switch (marketType)
                {
                    case MarketType.TB_OLD:
                    case MarketType.TB_NEW:
                        DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Turnover, num45 / 100f);
                        break;
                }
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.PE, br.ReadSingle());
                float num46 = br.ReadSingle();
                if (DllImportHelper.EqualZero(num46))
                {
                    LogUtilities.LogMessage("xwf StockDetailLev2 收到的量比 " + num46);
                }
                long num47 = Convert.ToInt64(br.ReadInt32());
                SecurityAttribute.SetVolumeRatio(this.Code, num47, num46, volume, fieldValue);
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.LastVolume, br.ReadInt32());
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.RedVolume, br.ReadInt32());
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.GreenVolume, br.ReadInt32());
                br.ReadSingle();
                br.ReadSingle();
                br.ReadSingle();
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Turnover3D, br.ReadSingle());
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.Turnover6D, br.ReadSingle());
                float num48 = br.ReadSingle();
                int num49 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume6, num49);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice6, num48);
                int num50 = ResStockDetailDataPacket.FindOldVolume(this.Code, num48, true);
                if (num50 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume6Delta, num49 - num50);
                }
                float num51 = br.ReadSingle();
                int num52 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume7, num52);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice7, num51);
                int num53 = ResStockDetailDataPacket.FindOldVolume(this.Code, num51, true);
                if (num53 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume7Delta, num52 - num53);
                }
                float num54 = br.ReadSingle();
                int num55 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume8, num55);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice8, num54);
                int num56 = ResStockDetailDataPacket.FindOldVolume(this.Code, num54, true);
                if (num56 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume8Delta, num55 - num56);
                }
                float num57 = br.ReadSingle();
                int num58 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume9, num58);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice9, num57);
                int num59 = ResStockDetailDataPacket.FindOldVolume(this.Code, num57, true);
                if (num59 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume9Delta, num58 - num59);
                }
                float num60 = br.ReadSingle();
                int num61 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume10, num61);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.BuyPrice10, num60);
                int num62 = ResStockDetailDataPacket.FindOldVolume(this.Code, num60, true);
                if (num62 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.BuyVolume10Delta, num61 - num62);
                }
                float num63 = br.ReadSingle();
                int num64 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume6, num64);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice6, num63);
                int num65 = ResStockDetailDataPacket.FindOldVolume(this.Code, num63, false);
                if (num65 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume6Delta, num64 - num65);
                }
                float num66 = br.ReadSingle();
                int num67 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume7, num67);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice7, num66);
                int num68 = ResStockDetailDataPacket.FindOldVolume(this.Code, num66, false);
                if (num68 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume7Delta, num67 - num68);
                }
                float num69 = br.ReadSingle();
                int num70 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume8, num70);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice8, num69);
                int num71 = ResStockDetailDataPacket.FindOldVolume(this.Code, num69, false);
                if (num71 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume8Delta, num70 - num71);
                }
                float num72 = br.ReadSingle();
                int num73 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume9, num73);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice9, num72);
                int num74 = ResStockDetailDataPacket.FindOldVolume(this.Code, num72, false);
                if (num74 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume9Delta, num73 - num74);
                }
                float num75 = br.ReadSingle();
                int num76 = br.ReadInt32();
                DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume10, num76);
                DllImportHelper.SetFieldData<float>(this.Code, FieldIndex.SellPrice10, num75);
                int num77 = ResStockDetailDataPacket.FindOldVolume(this.Code, num75, false);
                if (num77 != 0)
                {
                    DllImportHelper.SetFieldData<int>(this.Code, FieldIndex.SellVolume10Delta, num76 - num77);
                }
                DataCenter.MainUI.LV2DatasCallBack(this.Code, fieldInt32, fieldSingle, fieldInt64, fieldDouble);
                //DataPacket.CalcBondAi(this.Code, true, amount, volume);
            }
            return true;
        }
    }

    /// <summary>
    /// 百档行情
    /// </summary>
    public class ResAllOrderStockDetailLev2DataPacket : RealTimeDataPacket
    {
        public List<StockOrderDetailDataRec> OrderDetailData;

        protected override bool DecodingBody(BinaryReader br)
        {
            br.ReadUInt32();
            byte num = br.ReadByte();
            byte capacity = br.ReadByte();
            IntPtr zero = IntPtr.Zero;
            this.OrderDetailData = new List<StockOrderDetailDataRec>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                StockOrderDetailDataRec item = new StockOrderDetailDataRec();
                uint stockId = br.ReadUInt32();
                ConvertCode.ConvertIntToCode(stockId);
                int count = br.ReadInt32();
                byte[] pBuf = br.ReadBytes(count);
                byte[] pNew = new byte[0x19000];
                int[] pSize = new int[] { 0x19000 };
                switch (num)
                {
                    case 1:
                        DetailData.CodeIntPtrsAllOrder[stockId] = DataPacket.CreateInstance();
                        zero = DetailData.CodeIntPtrsAllOrder[stockId];
                        break;

                    case 2:
                        if (DetailData.CodeIntPtrsAllOrder.ContainsKey(stockId))
                        {
                            DataPacket.FreeInstance(DetailData.CodeIntPtrsAllOrder[stockId]);
                            DetailData.CodeIntPtrsAllOrder.Remove(stockId);
                        }
                        return false;

                    default:
                        if ((num == 0) && DetailData.CodeIntPtrsAllOrder.ContainsKey(stockId))
                        {
                            zero = DetailData.CodeIntPtrsAllOrder[stockId];
                        }
                        break;
                }
                if ((capacity <= 0) || (zero == IntPtr.Zero))
                {
                    return false;
                }
                if (DataPacket.UncompressOrderDetail(zero, pBuf, count, pNew, pSize) == 0)
                {
                    using (MemoryStream stream = new MemoryStream(pNew, 0, pNew.Length))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            reader.ReadInt32();
                            string key = ConvertCode.ConvertIntToCode(stockId);
                            if (!DetailData.EmCodeToUnicode.ContainsKey(key))
                            {
                                return false;
                            }
                            item.Code = DetailData.EmCodeToUnicode[key];
                            item.BuyAvgPrice = reader.ReadSingle();
                            item.BuyVolume = reader.ReadInt64();
                            item.BuyOrderCount = reader.ReadInt16();
                            item.BuyBiShu = reader.ReadInt32();
                            item.SellAvgPrice = reader.ReadSingle();
                            item.SellVolume = reader.ReadInt64();
                            item.SellOrderCount = reader.ReadInt16();
                            item.SellBiShu = reader.ReadInt32();
                            int num7 = reader.ReadInt32();
                            for (int j = 0; j < num7; j++)
                            {
                                OneOrderDetail detail = new OneOrderDetail();
                                detail.Price = reader.ReadSingle();
                                detail.Volume = reader.ReadInt64();
                                detail.OrderNum = reader.ReadInt32();
                                detail.Flag = reader.ReadByte();
                                item.BuyDetail.Add(detail);
                            }
                            int num9 = reader.ReadInt32();
                            for (int k = 0; k < num9; k++)
                            {
                                OneOrderDetail detail2 = new OneOrderDetail();
                                detail2.Price = reader.ReadSingle();
                                detail2.Volume = reader.ReadInt64();
                                detail2.OrderNum = reader.ReadInt32();
                                detail2.Flag = reader.ReadByte();
                                item.SellDetail.Add(detail2);
                            }
                        }
                    }
                }
                this.OrderDetailData.Add(item);
            }
            CFTService.CallBack(FuncTypeRealTime.AllOrderStockDetailLevel2, JsonConvert.SerializeObject(OrderDetailData));
            return true;
        }
    }

    /// <summary>
    /// N档行情
    /// </summary>
    public class ResNOrderStockDetailLev2DataPacket : RealTimeDataPacket
    {
        public List<StockOrderDetailDataRec> OrderDetailData;
        public List<int> Codes;

        public ResNOrderStockDetailLev2DataPacket()
        {
            OrderDetailData = new List<StockOrderDetailDataRec>(0);
            Codes = new List<int>(0);
        }
        public static bool EqualZero(double value)
        {
            return ((value > -4.94065645841247E-324) && (value < double.Epsilon));
        }

        public static bool EqualZero(float value)
        {
            return ((value > -1.401298E-45f) && (value < float.Epsilon));
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            try
            {
                br.ReadUInt32();
                byte num = br.ReadByte();
                byte num2 = br.ReadByte();
                for (int i = 0; i < num2; i++)
                {
                    StockOrderDetailDataRec item = new StockOrderDetailDataRec();
                    uint key = br.ReadUInt32();
                    int count = br.ReadInt32();
                    byte[] pBuf = br.ReadBytes(count);
                    byte[] pNew = new byte[0x2800];
                    IntPtr zero = IntPtr.Zero;
                    switch (num)
                    {
                        case 1:
                            DetailData.CodeIntPtrsNOrder[key] = CompressHelper.CreateInstance();
                            zero = DetailData.CodeIntPtrsNOrder[key];
                            break;

                        case 2:
                            if (DetailData.CodeIntPtrsNOrder.ContainsKey(key))
                            {
                                zero = DetailData.CodeIntPtrsNOrder[key];
                            }
                            else
                            {
                                zero = CompressHelper.CreateInstance();
                            }
                            break;

                        default:
                            if ((num == 0) && DetailData.CodeIntPtrsNOrder.ContainsKey(key))
                            {
                                zero = DetailData.CodeIntPtrsNOrder[key];
                            }
                            break;
                    }
                    if ((num2 <= 0) || (zero == IntPtr.Zero))
                    {
                        return false;
                    }
                    if (CompressHelper.UncompressQuoteRecNew(zero, pBuf, count, pNew))
                    {
                        using (MemoryStream stream = new MemoryStream(pNew, 0, pNew.Length))
                        {
                            BinaryReader reader = new BinaryReader(stream);
                            try
                            {
                                reader.ReadBytes(4);
                                byte num6 = reader.ReadByte();
                                int fieldValue = DataPacket.CTimeToDateTimeInt(reader.ReadInt32());
                                byte[] bytes = reader.ReadBytes(7);
                                string str = Encoding.ASCII.GetString(bytes).TrimEnd(new char[1]);
                                if (string.IsNullOrEmpty(str))
                                {
                                    return false;
                                }
                                string emCode = DataPacket.GetEmCode((ReqMarketType)num6, str);
                                if ((emCode == null) || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                                {
                                    return false;
                                }
                                item.Code = DetailData.EmCodeToUnicode[emCode];
                                this.Codes.Add(item.Code);
                                Dictionary<FieldIndex, int> fieldInt32;
                                Dictionary<FieldIndex, float> fieldSingle;
                                Dictionary<FieldIndex, long> fieldInt64;
                                Dictionary<FieldIndex, double> fieldDouble;

                                if (!DetailData.FieldIndexDataInt32.TryGetValue(item.Code, out fieldInt32))
                                {
                                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                                    DetailData.FieldIndexDataInt32[item.Code] = fieldInt32;
                                }
                                if (!DetailData.FieldIndexDataInt64.TryGetValue(item.Code, out fieldInt64))
                                {
                                    fieldInt64 = new Dictionary<FieldIndex, long>(1);
                                    DetailData.FieldIndexDataInt64[item.Code] = fieldInt64;
                                }
                                if (!DetailData.FieldIndexDataSingle.TryGetValue(item.Code, out fieldSingle))
                                {
                                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                                    DetailData.FieldIndexDataSingle[item.Code] = fieldSingle;
                                }
                                if (!DetailData.FieldIndexDataDouble.TryGetValue(item.Code, out fieldDouble))
                                {
                                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                                    DetailData.FieldIndexDataDouble[item.Code] = fieldDouble;
                                }
                                DetailData.FieldIndexDataInt32[item.Code][FieldIndex.Time] = fieldValue;
                                reader.ReadByte();
                                float num9 = reader.ReadSingle();
                                DetailData.FieldIndexDataSingle[item.Code][FieldIndex.PreClose] = num9;
                                float num10 = reader.ReadSingle();
                                float num11 = reader.ReadSingle();
                                float num12 = reader.ReadSingle();
                                float num13 = reader.ReadSingle();
                                DetailData.FieldIndexDataSingle[item.Code][FieldIndex.Now] = num13;
                                if (!EqualZero(num13))
                                {
                                    if (EqualZero(num10))
                                    {
                                        num10 = num13;
                                    }
                                    if (EqualZero(num11))
                                    {
                                        num11 = num13;
                                    }
                                    if (EqualZero(num12))
                                    {
                                        num12 = num13;
                                    }
                                }
                                DetailData.FieldIndexDataSingle[item.Code][FieldIndex.Open] =num10;
                                DetailData.FieldIndexDataSingle[item.Code][FieldIndex.High] =num11;
                                DetailData.FieldIndexDataSingle[item.Code][FieldIndex.Low] =num12;
                                double num14 = reader.ReadDouble();
                                DetailData.FieldIndexDataDouble[item.Code][FieldIndex.Amount] =num14;
                                DetailData.FieldIndexDataDouble[item.Code][FieldIndex.IndexAmount]= num14;
                                long num15 = reader.ReadInt64();
                                DetailData.FieldIndexDataInt64[item.Code][FieldIndex.Volume] =num15;
                                DetailData.FieldIndexDataInt64[item.Code][FieldIndex.IndexVolume]= num15;
                                MarketType marketType = DataCenterCore.CreateInstance().GetMarketType(item.Code);
                                bool flag2 = (((marketType == MarketType.SHConvertBondLev2) || (marketType == MarketType.SHNonConvertBondLev2)) || (marketType == MarketType.SZConvertBondLev2)) || (marketType == MarketType.SZNonConvertBondLev2);
                                for (int j = 0; j < 20; j++)
                                {
                                    if (flag2)
                                    {
                                        OneBondOrderDetail detail = new OneBondOrderDetail();
                                        item.BuyDetail.Add(detail);
                                        OneBondOrderDetail detail2 = new OneBondOrderDetail();
                                        item.SellDetail.Add(detail2);
                                    }
                                    else
                                    {
                                        OneOrderDetail detail3 = new OneOrderDetail();
                                        item.BuyDetail.Add(detail3);
                                        OneOrderDetail detail4 = new OneOrderDetail();
                                        item.SellDetail.Add(detail4);
                                    }
                                }
                                bool isConvertBond = marketType == MarketType.SHConvertBondLev2;
                                for (int k = 0; k < 20; k++)
                                {
                                    item.BuyDetail[k].Price = reader.ReadSingle();
                                    if (flag2 && (item.BuyDetail[k] is OneBondOrderDetail))
                                    {
                                        ((OneBondOrderDetail)item.BuyDetail[k]).Ytm = SecurityAttribute.CalcBondYtm(item.BuyDetail[k].Price, item.Code, isConvertBond);
                                    }
                                }
                                for (int m = 0; m < 20; m++)
                                {
                                    item.BuyDetail[m].Volume = reader.ReadInt64();
                                }
                                for (int n = 0; n < 20; n++)
                                {
                                    item.BuyDetail[n].OrderNum = reader.ReadInt32();
                                }
                                for (int num20 = 0; num20 < 20; num20++)
                                {
                                    item.BuyDetail[num20].Flag = reader.ReadByte();
                                }
                                for (int num21 = 0; num21 < 20; num21++)
                                {
                                    item.SellDetail[num21].Price = reader.ReadSingle();
                                    if (flag2 && (item.SellDetail[num21] is OneBondOrderDetail))
                                    {
                                        ((OneBondOrderDetail)item.SellDetail[num21]).Ytm = SecurityAttribute.CalcBondYtm(item.SellDetail[num21].Price, item.Code, isConvertBond);
                                    }
                                }
                                for (int num22 = 0; num22 < 20; num22++)
                                {
                                    item.SellDetail[num22].Volume = reader.ReadInt64();
                                }
                                for (int num23 = 0; num23 < 20; num23++)
                                {
                                    item.SellDetail[num23].OrderNum = reader.ReadInt32();
                                }
                                for (int num24 = 0; num24 < 20; num24++)
                                {
                                    item.SellDetail[num24].Flag = reader.ReadByte();
                                }
                                fieldInt32[FieldIndex.BSFlag] = reader.ReadByte();
                                fieldInt64[FieldIndex.LastVolume] = reader.ReadInt64();
                                fieldInt64[FieldIndex.RedVolume] = reader.ReadInt64();
                                fieldInt64[FieldIndex.GreenVolume] = reader.ReadInt64();

                                fieldInt64[FieldIndex.PreOpenInterest] = reader.ReadInt64();
                                fieldSingle[FieldIndex.PreSettlementPrice] = reader.ReadSingle();
                                fieldInt64[FieldIndex.CurOI] = reader.ReadInt64();
                                fieldInt64[FieldIndex.OpenInterest] = reader.ReadInt64();
                                fieldSingle[FieldIndex.SettlementPrice] = reader.ReadSingle();
                                float num25 = reader.ReadSingle();
                                float volumeRatio = 0f;
                                if (EqualZero(num25))
                                {
                                    LogUtilities.LogMessage("xwf NOrderDetail 收到的量比 " + num25);
                                }
                                long num27 = reader.ReadInt64();
                                SecurityAttribute.SetVolumeRatio(item.Code, num27, volumeRatio, num15, fieldValue);
                                item.BuyAvgPrice = reader.ReadSingle();
                                item.BuyVolume = reader.ReadInt64();
                                item.BuyOrderCount = reader.ReadInt16();
                                item.BuyBiShu = reader.ReadInt32();
                                item.SellAvgPrice = reader.ReadSingle();
                                item.SellVolume = reader.ReadInt64();
                                item.SellOrderCount = reader.ReadInt16();
                                item.SellBiShu = reader.ReadInt32();
                                float num28 = 0f;
                                if ((num15 != 0L) && (num14 != 0.0))
                                {
                                    num28 = Convert.ToSingle((double)((num14 * 1.0) / ((double)num15)));
                                }
                                fieldSingle[FieldIndex.AveragePrice] = num28;
                            }
                            catch (Exception exception)
                            {
                                LogUtilities.LogMessage(exception.Message);
                            }
                            finally
                            {
                                if (reader != null)
                                {
                                    reader.Close();
                                }
                            }
                        }
                    }
                    this.OrderDetailData.Add(item);
                    if (num == 2)
                    {
                        if (!DetailData.CodeIntPtrsNOrder.ContainsKey(key) && (zero != IntPtr.Zero))
                        {
                            CompressHelper.FreeInstance(zero);
                        }
                        else
                        {
                            CompressHelper.FreeInstance(DetailData.CodeIntPtrsNOrder[key]);
                            DetailData.CodeIntPtrsNOrder.Remove(key);
                        }
                    }
                }
                CFTService.CallBack(FuncTypeRealTime.NOrderStockDetailLevel2, JsonConvert.SerializeObject(OrderDetailData));
                this.SetStockDetailPacket(this);
            }
            catch (Exception exception2)
            {
                LogUtilities.LogMessage(exception2.Message);
                return false;
            }
            return true;
        }

        private void SetStockDetailPacket(ResNOrderStockDetailLev2DataPacket dataPacket)
        {

        }
    }


    /// <summary>
    /// 百档挂单队列
    /// </summary>
    public class ResStockDetailLev2OrderQueueDataPacket : RealTimeDataPacket
    {

        public List<StockPriceOrderQueueDataRec> StockPriceOrderData;
        public List<int> Codes;
        protected override bool DecodingBody(BinaryReader br)
        {
            uint msgId = br.ReadUInt32();
            byte status = br.ReadByte();//enum {StatusMiddle = 0, StatusBegin, StatusEnd, StatusUnknown};
            byte count = br.ReadByte();

            IntPtr pIf = IntPtr.Zero;
            if (status == 1)
            {
                DetailData.MsgIdIntPtrsQueue[msgId] = CreateInstance();
                pIf = DetailData.MsgIdIntPtrsQueue[msgId];
            }
            else if (status == 2)
            {
                if (DetailData.MsgIdIntPtrsQueue.ContainsKey(msgId))
                {
                    FreeInstance(DetailData.MsgIdIntPtrsQueue[msgId]);
                    DetailData.MsgIdIntPtrsQueue.Remove(msgId);
                }
                return false;
            }
            else if (status == 0)
            {
                if (DetailData.MsgIdIntPtrsQueue.ContainsKey(msgId))
                {
                    pIf = DetailData.MsgIdIntPtrsQueue[msgId];
                }
            }
            if (count <= 0 || pIf == IntPtr.Zero)
                return false;

            StockPriceOrderData = new List<StockPriceOrderQueueDataRec>(count);
            Codes = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                StockPriceOrderQueueDataRec oneStock = new StockPriceOrderQueueDataRec();
                uint stockId = br.ReadUInt32();
                float price = br.ReadSingle();
                byte bsFlag = br.ReadByte();

                int len = br.ReadInt32();
                byte[] buf = br.ReadBytes(len);

                byte[] pNew = new byte[10240];
                int[] pSize = new int[1];
                pSize[0] = 10240;

                int intResult = UncompressPriceOrderQueue(pIf, buf, len, pNew, pSize);
                if (intResult == 0)
                {
                    using (MemoryStream ms = new MemoryStream(pNew, 0, pSize[0]))
                    {
                        using (BinaryReader brUncompress = new BinaryReader(ms))
                        {
                            int stockid = brUncompress.ReadInt32();
                            string emCode = ConvertCode.ConvertIntToCode(stockId);
                            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                                return false;
                            oneStock.Code = DetailData.EmCodeToUnicode[emCode];
                            Codes.Add(oneStock.Code);
                            oneStock.BSFlag = brUncompress.ReadByte();//0buy,1sell
                            oneStock.Price = brUncompress.ReadSingle();
                            int itemCount = brUncompress.ReadInt32();
                            int orderId = -1;
                            for (int j = 0; j < itemCount; j++)
                            {
                                OnePriceOrder oneItem = new OnePriceOrder();
                                oneItem.Status = brUncompress.ReadByte();
                                oneItem.OrderId = brUncompress.ReadInt32();
                                if (oneItem.Status == 1)
                                {
                                    if (j == itemCount - 1)
                                        return true;
                                    else
                                        orderId = oneItem.OrderId;
                                }
                                else
                                {
                                    if (orderId > 0)
                                    {
                                        if (oneItem.OrderId != orderId)
                                        {
                                            oneStock.OrderData.RemoveAt(oneStock.OrderData.Count - 1);
                                            //Debug.Print("移除一个");
                                        }
                                        orderId = -1;
                                    }
                                }
                                oneItem.Volume = brUncompress.ReadInt32();
                                //Debug.Print("status = " + oneItem.Status + "   ,volume=" + oneItem.Volume + "  ,orderId=" + oneItem.OrderId);
                                oneStock.OrderData.Add(oneItem);
                            }
                            //Debug.Print("========================================================");
                        }
                    }
                }
                StockPriceOrderData.Add(oneStock);
            }
            return true;
        }
    }

    /// <summary>
    /// 股指期货Detail行情
    /// </summary>
    public class ResIndexFuturesDetailDataPacket : RealTimeDataPacket
    {
        private int _code;
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            private set { this._code = value; }
        }


        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            string emCode = ConvertCode.ConvertIntToCode(br.ReadUInt32());
            if (emCode.StartsWith("04"))
            {
                if (emCode == "040120.CFE")
                    emCode = "IF00C1.CFE";
                else if (emCode == "040121.CFE")
                    emCode = "IF00C2.CFE";
                else if (emCode == "040122.CFE")
                    emCode = "IF00C3.CFE";
                else if (emCode == "040123.CFE")
                    emCode = "IF00C4.CFE";
                else
                {
                    string month = emCode.Substring(emCode.Length - 6, 2);
                    string headStr = string.Empty;
                    if (DateTime.Now.Month <= Convert.ToInt32(month))
                        headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100);
                    else
                    {
                        headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                    }
                    emCode = (headStr + month) + ".CFE";
                }
            }
            else if (emCode.StartsWith("05"))
            {
                if (emCode == "050120.CFE")
                    emCode = "TF00C1.CFE";
                else if (emCode == "050121.CFE")
                    emCode = "TF00C2.CFE";
                else if (emCode == "050122.CFE")
                    emCode = "TF00C3.CFE";
                else if (emCode == "050123.CFE")
                    emCode = "TF00C4.CFE";
                else
                {
                    string month = emCode.Substring(emCode.Length - 6, 2);
                    string headStr = string.Empty;
                    if (DateTime.Now.Month <= Convert.ToInt32(month))
                        headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100);
                    else
                    {
                        headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                    }
                    emCode = (headStr + month) + ".CFE";
                }
            }

            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];

            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;
            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }
            byte type = br.ReadByte();
            int cTime = br.ReadInt32();
            fieldInt32[FieldIndex.Time] = CTimeToDateTimeInt(cTime);
            fieldSingle[FieldIndex.Now] = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = br.ReadSingle();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            fieldSingle[FieldIndex.VolumeRatio] = br.ReadSingle();
            fieldInt32[FieldIndex.BSFlag] = br.ReadByte();
            fieldInt64[FieldIndex.LastVolume] = br.ReadInt32();
            fieldSingle[FieldIndex.PreSettlementPrice] = br.ReadSingle();
            fieldSingle[FieldIndex.SettlementPrice] = br.ReadSingle();
            fieldInt64[FieldIndex.CurOI] = br.ReadInt32();
            fieldInt64[FieldIndex.Volume] = br.ReadInt64();
            fieldDouble[FieldIndex.Amount] = br.ReadDouble();
            fieldInt64[FieldIndex.OpenInterest] = br.ReadInt64();
            fieldInt64[FieldIndex.PreOpenInterest] = br.ReadInt64();
            fieldInt64[FieldIndex.RedVolume] = br.ReadInt64();
            fieldInt64[FieldIndex.GreenVolume] = br.ReadInt64();

            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume1] = br.ReadInt32() * 100;
            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume1] = br.ReadInt32() * 100;

            fieldSingle[FieldIndex.SellPrice2] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume2] = br.ReadInt32() * 100;
            fieldSingle[FieldIndex.BuyPrice2] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume2] = br.ReadInt32() * 100;

            fieldSingle[FieldIndex.SellPrice3] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume3] = br.ReadInt32() * 100;
            fieldSingle[FieldIndex.BuyPrice3] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume3] = br.ReadInt32() * 100;

            fieldSingle[FieldIndex.SellPrice4] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume4] = br.ReadInt32() * 100;
            fieldSingle[FieldIndex.BuyPrice4] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume4] = br.ReadInt32() * 100;

            fieldSingle[FieldIndex.SellPrice5] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume5] = br.ReadInt32() * 100;
            fieldSingle[FieldIndex.BuyPrice5] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume5] = br.ReadInt32() * 100;
            return true;
        }
    }

    /// <summary>
    /// 指数Detail行情
    /// </summary>
    public class ResIndexDetailDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 股票代码
        /// </summary>
        public List<int> Codes;

        public ResIndexDetailDataPacket()
        {
            Codes = new List<int>(1);
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int count = br.ReadByte();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, double> fieldDouble;
            for (int i = 0; i < count; i++)
            {
                int date = br.ReadInt32();
                int time = br.ReadInt32();
                string emCode = ConvertCode.ConvertIntToCode(br.ReadUInt32());

                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                int code = DetailData.EmCodeToUnicode[emCode];
                Codes.Add(code);

                if (!DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[code] = fieldInt32;
                }
                if (!DetailData.FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                {
                    fieldInt64 = new Dictionary<FieldIndex, long>(1);
                    DetailData.FieldIndexDataInt64[code] = fieldInt64;
                }
                if (!DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[code] = fieldSingle;
                }
                if (!DetailData.FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                {
                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                    DetailData.FieldIndexDataDouble[code] = fieldDouble;
                }



                fieldSingle[FieldIndex.Now] = br.ReadSingle();
                fieldSingle[FieldIndex.PreClose] = br.ReadSingle();
                fieldInt64[FieldIndex.IndexVolume] = ((long)br.ReadUInt32() * 100);
                fieldInt64[FieldIndex.Volume] = fieldInt64[FieldIndex.IndexVolume];
                fieldDouble[FieldIndex.IndexAmount] = (double)br.ReadUInt32() * 10000;
                fieldDouble[FieldIndex.Amount] = fieldDouble[FieldIndex.IndexAmount];

                fieldInt32[FieldIndex.UpNum] = br.ReadInt16();
                fieldInt32[FieldIndex.EqualNum] = br.ReadInt16();
                fieldInt32[FieldIndex.DownNum] = br.ReadInt16();

                if (fieldSingle[FieldIndex.Now] != 0 && fieldSingle[FieldIndex.PreClose] != 0)
                {
                    fieldSingle[FieldIndex.Difference] = fieldSingle[FieldIndex.Now] - fieldSingle[FieldIndex.PreClose];
                    fieldSingle[FieldIndex.DifferRange] = fieldSingle[FieldIndex.Difference] /
                                                          fieldSingle[FieldIndex.PreClose];
                }
                CFTService.CallBack(FuncTypeRealTime.IndexDetail, JsonConvert.SerializeObject(fieldSingle));
            }
            return true;
        }
    }

    /// <summary>
    /// 个股分时走势
    /// </summary>
    public class ResStockTrendDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 走势数据
        /// </summary>
        public OneDayTrendDataRec TrendData;

        private short _preOffset;
        /// <summary>
        /// 
        /// </summary>
        public short PreOffset
        {
            get { return _preOffset; }
            private set { this._preOffset = value; }
        }

        private short _offset;
        /// <summary>
        /// 
        /// </summary>
        public short Offset
        {
            get { return _offset; }
            private set { this._offset = value; }
        }

        private short _preMinNum;
        /// <summary>
        /// 
        /// </summary>
        public short PreMinNum
        {
            get { return _preMinNum; }
            private set { this._preMinNum = value; }
        }

        private short _minNum;
        /// <summary>
        /// 
        /// </summary>
        public short MinNum
        {
            get { return _minNum; }
            private set { this._minNum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResStockTrendDataPacket()
        {
            RequestType = FuncTypeRealTime.StockTrend;
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(System.IO.BinaryReader br)
        {
            br.ReadUInt32();
            if (br.ReadByte() == 2)
            {
                return false;
            }
            short num2 = br.ReadInt16();
            if (num2 <= 0)
            {
                return false;
            }
            IntPtr ptr = CompressHelper.CreateInstance();
            if ((num2 <= 0) || (ptr == IntPtr.Zero))
            {
                return false;
            }
            try
            {
                int count = br.ReadInt32();
                byte[] zipData = br.ReadBytes(count);
                int stockId = 0;
                List<CompressHelper.SRawNewRtMin> datas = null;
                int date = 0;
                CompressHelper.GetTrendData(ptr, zipData, count, out stockId, out datas, out date);
                string emcode = ConvertCode.ConvertIntToCode((uint)stockId);
                if (emcode.EndsWith("CFE"))
                {
                    emcode = ConvertCode.ConvertFuturesCftEmCodeToOrgEmCode(emcode);
                }
                if ((emcode == null) || !DetailData.EmCodeToUnicode.ContainsKey(emcode))
                {
                    return false;
                }
                int code = DetailData.EmCodeToUnicode[emcode];
                this.TrendData = new OneDayTrendDataRec(code);
                this.TrendData.Date = date;
                MarketTime marketTime = TimeUtilities.GetMarketTime(DllImportHelper.GetMarketType(code));
                this.PreOffset = -1;
                this.Offset = -1;
                this.PreMinNum = 0;
                this.MinNum = 0;
                if (datas.Count > 0)
                {
                    this.TrendData.Time = (int)datas[datas.Count - 1].m_dwTime;
                }
                short index = 0;
                short num8 = 0;
                short callAuctionPointFromTime = -1;
                for (int i = 0; i < datas.Count; i++)
                {
                    int dwTime = (int)datas[i].m_dwTime;
                    if ((dwTime >= marketTime.CallAuctionTime) && (dwTime < marketTime.TrendTimeList[0].Open))
                    {
                        callAuctionPointFromTime = TimeUtilities.GetCallAuctionPointFromTime(dwTime, this.TrendData.Code);
                        if (callAuctionPointFromTime < 0)
                        {
                            callAuctionPointFromTime = 0;
                        }
                        this.TrendData.PreMintDatas[index].Price = (float)datas[i].m_dblClose;
                        this.TrendData.PreMintDatas[index].Volume = datas[i].m_xVolume;
                        this.TrendData.PreMintDatas[index].AverPrice = (float)datas[i].m_dblAve;
                        this.TrendData.PreMintDatas[index].Amount = datas[i].m_dblAmount;
                        this.TrendData.PreMintDatas[index].BuyVolume = datas[i].m_xExt1;
                        this.TrendData.PreMintDatas[index].SellVolume = datas[i].m_xExt2;
                        this.TrendData.PreMintDatas[index].Fast = datas[i].m_dblExt1;
                        this.TrendData.PreMintDatas[index].Low = datas[i].m_dblExt2;
                        this.TrendData.PreMintDatas[index].NeiWaiCha = datas[i].m_xVolume - (2L * datas[i].m_xWaiPan);
                        this.TrendData.PreMintDatas[index].Wai = datas[i].m_xWaiPan;
                        this.TrendData.PreMintDatas[index].Nei = datas[i].m_xVolume - datas[i].m_xWaiPan;
                        this.PreMinNum = (short)(this.PreMinNum + 1);
                        index = (short)(index + 1);
                        if (this.PreOffset < 0)
                        {
                            this.PreOffset = callAuctionPointFromTime;
                        }
                    }
                    else if ((dwTime >= marketTime.TrendTimeList[0].Open) && (dwTime <= marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close))
                    {
                        callAuctionPointFromTime = TimeUtilities.GetPointFromTimeForTrend(dwTime, this.TrendData.Code);
                        if (callAuctionPointFromTime < 0)
                        {
                            callAuctionPointFromTime = 0;
                        }
                        this.TrendData.MintDatas[num8].Price = (float)datas[i].m_dblClose;
                        this.TrendData.MintDatas[num8].Volume = datas[i].m_xVolume;
                        this.TrendData.MintDatas[num8].AverPrice = (float)datas[i].m_dblAve;
                        this.TrendData.MintDatas[num8].Amount = datas[i].m_dblAmount;
                        this.TrendData.MintDatas[num8].BuyVolume = datas[i].m_xExt1;
                        this.TrendData.MintDatas[num8].SellVolume = datas[i].m_xExt2;
                        this.TrendData.MintDatas[num8].Fast = datas[i].m_dblExt1;
                        this.TrendData.MintDatas[num8].Low = datas[i].m_dblExt2;
                        this.TrendData.MintDatas[num8].NeiWaiCha = datas[i].m_xVolume - (2L * datas[i].m_xWaiPan);
                        this.TrendData.MintDatas[num8].Wai = datas[i].m_xWaiPan;
                        this.TrendData.MintDatas[num8].Nei = datas[i].m_xVolume - datas[i].m_xWaiPan;
                        if (this.TrendData.High < datas[i].m_dblHigh)
                        {
                            this.TrendData.High = (float)datas[i].m_dblHigh;
                        }
                        if (this.TrendData.Low <= 0f)
                        {
                            this.TrendData.Low = (float)datas[i].m_dblLow;
                        }
                        else if (this.TrendData.Low > datas[i].m_dblLow)
                        {
                            this.TrendData.Low = (float)datas[i].m_dblLow;
                        }
                        this.MinNum = (short)(this.MinNum + 1);
                        num8 = (short)(num8 + 1);
                        if (this.Offset < 0)
                        {
                            this.Offset = callAuctionPointFromTime;
                        }
                    }
                }
                CFTService.CallBack(FuncTypeRealTime.StockTrend, JsonConvert.SerializeObject(TrendData.MintDatas));
            }
            catch (Exception exception)
            {
                //LogUtilities.LogMessagePublishException(exception.Message + "\n" + exception.StackTrace);
            }
            finally
            {
                CompressHelper.FreeInstance(ptr);
            }
            return true;
        }
    }

    /// <summary>
    /// 股指期货走势
    /// </summary>
    public class ResIndexFuturesTrendDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 走势数据
        /// </summary>
        public OneDayTrendDataRec TrendData;

        private short _offset;
        /// <summary>
        /// 
        /// </summary>
        public short Offset
        {
            get { return _offset; }
            private set { this._offset = value; }
        }

        private short _minNum;
        /// <summary>
        /// 
        /// </summary>
        public short MinNum
        {
            get { return _minNum; }
            private set { this._minNum = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            string emCode = ConvertCode.ConvertIntToCode(br.ReadUInt32());
            if (emCode.StartsWith("04"))
            {
                if (emCode == "040120.CFE")
                    emCode = "IF00C1.CFE";
                else if (emCode == "040121.CFE")
                    emCode = "IF00C2.CFE";
                else if (emCode == "040122.CFE")
                    emCode = "IF00C3.CFE";
                else if (emCode == "040123.CFE")
                    emCode = "IF00C4.CFE";
                else
                {
                    string month = emCode.Substring(emCode.Length - 6, 2);
                    string headStr = string.Empty;
                    if (DateTime.Now.Month <= Convert.ToInt32(month))
                        headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100);
                    else
                    {
                        headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                    }
                    emCode = (headStr + month) + ".CFE";
                }
            }
            else if (emCode.StartsWith("05"))
            {
                if (emCode == "050120.CFE")
                    emCode = "TF00C1.CFE";
                else if (emCode == "050121.CFE")
                    emCode = "TF00C2.CFE";
                else if (emCode == "050122.CFE")
                    emCode = "TF00C3.CFE";
                else if (emCode == "050123.CFE")
                    emCode = "TF00C4.CFE";
                else
                {
                    string month = emCode.Substring(emCode.Length - 6, 2);
                    string headStr = string.Empty;
                    if (DateTime.Now.Month <= Convert.ToInt32(month))
                        headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100);
                    else
                    {
                        headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                    }
                    emCode = (headStr + month) + ".CFE";
                }
            }
            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            int uniCode = DetailData.EmCodeToUnicode[emCode];
            TrendData = new OneDayTrendDataRec(uniCode); //该包的走势数据（不是全天的走势数据）
            TrendData.Date = br.ReadInt32();

            Offset = br.ReadInt16();
            TrendData.Time = br.ReadInt32();
            TrendData.Open = br.ReadSingle();
            TrendData.High = br.ReadSingle();
            TrendData.Low = br.ReadSingle();
            TrendData.PreClose = br.ReadSingle();
            int volume = br.ReadInt32();
            double amount = br.ReadDouble();
            MinNum = br.ReadInt16();


            //             try
            //             {
            //                 int j = 0;
            //                 for (int i = 0; i < MinNum; i++)
            //                 {
            //                     float price = br.ReadSingle();
            //                     int volume1 = br.ReadInt32();
            //                     float averPrice = br.ReadSingle();
            //                     int openinterest = br.ReadInt32();
            //                     j++;
            //                 }
            //             }
            //             catch (Exception)
            //             {
            //                 
            //                 
            //             }


            if (Offset < 5)
            {
                //集合竞价阶段数据
                int preNum = 0;
                if (MinNum > 5 - Offset)
                    preNum = 5 - Offset;
                else
                    preNum = MinNum;
                for (int i = 0; i < preNum; i++)
                {
                    TrendData.PreMintDatas[i].Price = br.ReadSingle();
                    TrendData.PreMintDatas[i].Volume = br.ReadInt32();
                    TrendData.PreMintDatas[i].AverPrice = br.ReadSingle();
                    if (TrendData.PreMintDatas[i] is OneMinuteIFDataRec)
                    {
                        (TrendData.PreMintDatas[i] as OneMinuteIFDataRec).OpenInterest = br.ReadInt32();
                    }
                }

                for (int i = 0; i < MinNum - 5 + Offset; i++)
                {
                    TrendData.MintDatas[i].Price = br.ReadSingle();
                    TrendData.MintDatas[i].Volume = br.ReadInt32();
                    TrendData.MintDatas[i].AverPrice = br.ReadSingle();
                    if (TrendData.MintDatas[i] is OneMinuteIFDataRec)
                    {
                        (TrendData.MintDatas[i] as OneMinuteIFDataRec).OpenInterest = br.ReadInt32();
                    }
                    else
                    {
                        LogUtilities.LogMessage("ResIndexFuturesTrendDataPacket 解包错误");
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < MinNum; i++)
                {
                    TrendData.MintDatas[i].Price = br.ReadSingle();
                    TrendData.MintDatas[i].Volume = br.ReadInt32();
                    TrendData.MintDatas[i].AverPrice = br.ReadSingle();
                    if (TrendData.MintDatas[i] is OneMinuteIFDataRec)
                    {
                        (TrendData.MintDatas[i] as OneMinuteIFDataRec).OpenInterest = br.ReadInt32();
                    }
                    else
                    {
                        LogUtilities.LogMessage("ResIndexFuturesTrendDataPacket 解包错误");
                        return false;
                    }
                }
            }

            return true;
        }
    }

    /// <summary>
    /// 个股走势推送
    /// </summary>
    public class ResStockTrendPushDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 走势数据
        /// </summary>
        public OneDayTrendDataRec TrendData;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            int uniCode = DetailData.EmCodeToUnicode[emCode];
            TrendData = new OneDayTrendDataRec(uniCode);
            short num = br.ReadInt16();
            for (int i = 0; i < num; i++)
            {
                int index = br.ReadInt32();
                TrendData.MintDatas[i].Price = br.ReadSingle();
                TrendData.MintDatas[i].Volume = br.ReadInt32();
                TrendData.MintDatas[i].Amount = br.ReadDouble();
                TrendData.MintDatas[i].BuyVolume = br.ReadInt32();
                TrendData.MintDatas[i].SellVolume = br.ReadInt32();

            }

            return base.DecodingBody(br);
        }
    }

    /// <summary>
    /// 走势委买委卖量
    /// </summary>
    public class ResTrendAskBidDataPacket : RealTimeDataPacket
    {
        private int _code;
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            private set { this._code = value; }
        }

        private short _indexOffset;
        /// <summary>
        /// 
        /// </summary>
        public short IndexOffset
        {
            get { return _indexOffset; }
            private set { this._indexOffset = value; }
        }

        private short _num;
        /// <summary>
        /// 数量
        /// </summary>
        public short Num
        {
            get { return _num; }
            private set { this._num = value; }
        }

        private int[] _buyVols;
        /// <summary>
        /// 
        /// </summary>
        public int[] BuyVolumes
        {
            get { return _buyVols; }
            private set { this._buyVols = value; }
        }

        private int[] _sellVols;
        /// <summary>
        /// 
        /// </summary>
        public int[] SellVolumes
        {
            get { return _sellVols; }
            private set { this._sellVols = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];

            int date = br.ReadInt32();
            int time = br.ReadInt32();
            IndexOffset = br.ReadInt16();
            Num = br.ReadInt16();
            BuyVolumes = new int[Num];
            SellVolumes = new int[Num];
            for (int i = 0; i < Num; i++)
            {
                BuyVolumes[i] = br.ReadInt32();
                SellVolumes[i] = br.ReadInt32();
            }
            return base.DecodingBody(br);
        }
    }

    /// <summary>
    /// 走势内外盘差
    /// </summary>
    public class ResTrendInOutDiffDataPacket : RealTimeDataPacket
    {
        private int _code;
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            private set { this._code = value; }
        }

        private short _indexOffset;
        /// <summary>
        /// 
        /// </summary>
        public short IndexOffset
        {
            get { return _indexOffset; }
            private set { this._indexOffset = value; }
        }

        private int[] _inOutDiff;
        /// <summary>
        /// 
        /// </summary>
        public int[] InOutDiff
        {
            get { return _inOutDiff; }
            private set { this._inOutDiff = value; }
        }

        private short _num;
        /// <summary>
        /// 数量
        /// </summary>
        public short Num
        {
            get { return _num; }
            private set { this._num = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];
            int date = br.ReadInt32();
            int time = br.ReadInt32();
            IndexOffset = br.ReadInt16();
            Num = br.ReadInt16();
            InOutDiff = new int[Num];

            for (int i = 0; i < Num; i++)
                InOutDiff[i] = br.ReadInt32() * (-1);

            return true;
        }
    }

    /// <summary>
    /// 红绿柱
    /// </summary>
    public class ResRedGreenDataPacket : RealTimeDataPacket
    {
        private int _code;
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            private set { this._code = value; }
        }

        private short _indexOffset;
        /// <summary>
        /// 
        /// </summary>
        public short IndexOffset
        {
            get { return _indexOffset; }
            private set { this._indexOffset = value; }
        }

        private OneMintRedGreen[] _redGreenDatas;
        /// <summary>
        /// 
        /// </summary>
        public OneMintRedGreen[] RedGreenDatas
        {
            get { return _redGreenDatas; }
            private set { this._redGreenDatas = value; }
        }

        private short _num;
        /// <summary>
        /// 数量
        /// </summary>
        public short Num
        {
            get { return _num; }
            private set { this._num = value; }
        }

        private int _date;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date
        {
            get { return _date; }
            private set { this._date = value; }
        }

        private int _time;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time
        {
            get { return _time; }
            private set { this._time = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            switch ((ReqMarketType)market)
            {
                case ReqMarketType.MT_SH:
                    Code = DetailData.EmCodeToUnicode["000001.SH"];
                    break;
                case ReqMarketType.MT_SZ:
                    Code = DetailData.EmCodeToUnicode["399001.SZ"];
                    break;
                default:
                    break;
            }
            Date = br.ReadInt32();
            Time = br.ReadInt32();
            IndexOffset = br.ReadInt16();
            Num = br.ReadInt16();
            RedGreenDatas = new OneMintRedGreen[Num];
            for (int i = 0; i < Num; i++)
            {
                OneMintRedGreen tmp = new OneMintRedGreen();
                tmp.Fast = br.ReadSingle();
                tmp.Slow = br.ReadSingle();
                RedGreenDatas[i] = tmp;
            }

            return true;
        }
    }


    /// <summary>
    /// 当日K线
    /// </summary>
    public class ResMinKLineDataPacket : ResHisKLineDataPacket
    {

    }

    /// <summary>
    /// 市场报价
    /// </summary>
    public class ResSectorQuoteReportDataPacket : RealTimeDataPacket
    {
        private bool _isBlock;
        /// <summary>
        /// 是否是板块
        /// </summary>
        public bool IsBlock
        {
            get { return _isBlock; }
            set { _isBlock = value; }
        }

        private byte _packetStatus;
        /// <summary>
        /// 响应包的标识，1表示开始包，0表示中间包，2表示结尾包，其他表示错误包
        /// </summary>
        public byte PacketStatus
        {
            get { return _packetStatus; }
            private set { this._packetStatus = value; }
        }

        Dictionary<int, Dictionary<byte, object>> _dicFieldValue;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, Dictionary<byte, object>> DicFieldValue
        {
            get { return _dicFieldValue; }
            private set { this._dicFieldValue = value; }
        }

        Dictionary<int, List<FieldIndex>> _changedFields;
        /// <summary>
        /// 有数据改变的字段
        /// </summary>
        public Dictionary<int, List<FieldIndex>> ChangedFields
        {
            get { return _changedFields; }
            private set { this._changedFields = value; }
        }

        private void ReadFieldIndex(List<ReqFieldIndex> reqFieldIndex, BinaryReader br)
        {
            try
            {
                Dictionary<byte, object> result = new Dictionary<byte, object>(reqFieldIndex.Count);
                List<FieldIndex> fieldClient = new List<FieldIndex>();

                uint stockId = 0;

                foreach (ReqFieldIndex field in reqFieldIndex)
                {
                    DicFieldIndexItem fieldIndexItem = ConvertFieldIndex.DicFieldIndex[field];
                    switch (fieldIndexItem.FieldTypeServer)
                    {
                        case ReqFieldIndexType.FT_STOCKID:
                            stockId = br.ReadUInt32();
                            break;
                        case ReqFieldIndexType.FT_CODE:
                            byte[] codeBytes = br.ReadBytes(7);
                            //                         string code = Encoding.ASCII.GetString(codeBytes);
                            //                         code = code.TrimEnd('\0');
                            //                         result.Add(field,codeBytes);
                            break;
                        case ReqFieldIndexType.FT_BYTE:
                            byte byteResult = br.ReadByte();
                            result.Add(Convert.ToByte(field), byteResult);
                            break;
                        case ReqFieldIndexType.FT_INT32:
                            int intResult = br.ReadInt32();
                            result.Add(Convert.ToByte(field), intResult);
                            break;
                        case ReqFieldIndexType.FT_UINT32:
                            uint uintResult = br.ReadUInt32();
                            result.Add(Convert.ToByte(field), uintResult);
                            break;
                        case ReqFieldIndexType.FT_LISTFLOWITEM:
                            ListFlowItem item = new ListFlowItem();
                            item.PercentDec = br.ReadInt32();
                            item.HisPercentDecRange = br.ReadInt32();
                            item.DiffRanger = br.ReadInt32();
                            result.Add(Convert.ToByte(field), item);
                            break;
                        case ReqFieldIndexType.FT_LISTFLOWDETAILITEM:
                            ListFlowDetailItem detailItem = new ListFlowDetailItem();
                            detailItem.Buy = br.ReadInt32();
                            detailItem.Sell = br.ReadInt32();
                            result.Add(Convert.ToByte(field), detailItem);
                            break;
                    }

                    switch (field)
                    {
                        case ReqFieldIndex.Now:
                            fieldClient.Add(FieldIndex.Difference);
                            fieldClient.Add(FieldIndex.DifferRange);
                            fieldClient.Add(FieldIndex.DifferSpeed);
                            fieldClient.Add(FieldIndex.PE);
                            break;
                        case ReqFieldIndex.PreClose:
                            fieldClient.Add(FieldIndex.Difference);
                            fieldClient.Add(FieldIndex.DifferRange);
                            break;
                        case ReqFieldIndex.High:
                        case ReqFieldIndex.Low:
                            fieldClient.Add(FieldIndex.Delta);
                            break;
                        case ReqFieldIndex.LTG:
                        case ReqFieldIndex.Volume:
                            fieldClient.Add(FieldIndex.Turnover);
                            break;
                        case ReqFieldIndex.SumBuyVol:
                        case ReqFieldIndex.SumSellVol:
                            fieldClient.Add(FieldIndex.Weibi);
                            fieldClient.Add(FieldIndex.Weicha);
                            break;
                        case ReqFieldIndex.NeiPan:
                            fieldClient.Add(FieldIndex.RedVolume);
                            fieldClient.Add(FieldIndex.NeiWaiBi);
                            break;
                        case ReqFieldIndex.SListFlowDetailItemBig:
                            fieldClient.Add(FieldIndex.BuyBig);
                            fieldClient.Add(FieldIndex.SellBig);
                            fieldClient.Add(FieldIndex.NetFlowBig);
                            fieldClient.Add(FieldIndex.NetFlowRangeBig);
                            fieldClient.Add(FieldIndex.MainNetFlow);
                            fieldClient.Add(FieldIndex.BuyFlowRangeBig);
                            fieldClient.Add(FieldIndex.SellFlowRangeBig);
                            break;
                        case ReqFieldIndex.SListFlowDetailItemMiddle:
                            fieldClient.Add(FieldIndex.BuyMiddle);
                            fieldClient.Add(FieldIndex.SellMiddle);
                            fieldClient.Add(FieldIndex.NetFlowMiddle);
                            fieldClient.Add(FieldIndex.NetFlowRangeMiddle);
                            break;
                        case ReqFieldIndex.SListFlowDetailItemSmall:
                            fieldClient.Add(FieldIndex.BuySmall);
                            fieldClient.Add(FieldIndex.SellSmall);
                            fieldClient.Add(FieldIndex.NetFlowSmall);
                            fieldClient.Add(FieldIndex.NetFlowRangeSmall);
                            break;
                        case ReqFieldIndex.SListFlowDetailItemSuper:
                            fieldClient.Add(FieldIndex.BuySuper);
                            fieldClient.Add(FieldIndex.SellSuper);
                            fieldClient.Add(FieldIndex.NetFlowSuper);
                            fieldClient.Add(FieldIndex.NetFlowRangeSuper);
                            fieldClient.Add(FieldIndex.MainNetFlow);
                            fieldClient.Add(FieldIndex.BuyFlowRangeSuper);
                            fieldClient.Add(FieldIndex.SellFlowRangeSuper);
                            break;
                        case ReqFieldIndex.SListFlowItemDay:
                            fieldClient.Add(FieldIndex.ZengCangRange);
                            fieldClient.Add(FieldIndex.ZengCangRank);
                            fieldClient.Add(FieldIndex.ZengCangRankChange);
                            break;
                        case ReqFieldIndex.SListFlowItemDay3:
                            fieldClient.Add(FieldIndex.ZengCangRangeDay3);
                            fieldClient.Add(FieldIndex.ZengCangRankDay3);
                            fieldClient.Add(FieldIndex.ZengCangRankChangeDay3);
                            break;
                        case ReqFieldIndex.SListFlowItemDay5:
                            fieldClient.Add(FieldIndex.ZengCangRangeDay5);
                            fieldClient.Add(FieldIndex.ZengCangRankDay5);
                            fieldClient.Add(FieldIndex.ZengCangRankChangeDay5);
                            break;
                        case ReqFieldIndex.SListFlowItemDay10:
                            fieldClient.Add(FieldIndex.ZengCangRangeDay10);
                            fieldClient.Add(FieldIndex.ZengCangRankDay10);
                            fieldClient.Add(FieldIndex.ZengCangRankChangeDay10);
                            break;

                    }
                    if (fieldIndexItem.FieldClient != FieldIndex.Na)
                        fieldClient.Add(fieldIndexItem.FieldClient);
                }
                string emCode = ConvertCode.ConvertIntToCode(stockId);
                if (emCode != null && DetailData.EmCodeToUnicode.ContainsKey(emCode))
                {
                    int unicode = DetailData.EmCodeToUnicode[emCode];
                    if (!DicFieldValue.ContainsKey(unicode))
                        DicFieldValue.Add(unicode, result);
                    if (!ChangedFields.ContainsKey(unicode))
                        ChangedFields.Add(unicode, fieldClient);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
            }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte bResponse = br.ReadByte();
            int id = br.ReadInt32();
            PacketStatus = br.ReadByte(); //0中间，1开始，2末尾，3未知
            int nStock = br.ReadInt32(); //股票数
            DicFieldValue = new Dictionary<int, Dictionary<byte, object>>(nStock);
            ChangedFields = new Dictionary<int, List<FieldIndex>>(nStock);

            byte nNumBits = br.ReadByte(); //栏位字节，0-1字节，其他-2字节
            short nField = br.ReadInt16(); //栏位个数
            List<ReqFieldIndex> fieldIndexList = new List<ReqFieldIndex>(nField);
            for (int i = 0; i < nField; i++)
            {
                if (nNumBits == 0)
                    fieldIndexList.Add((ReqFieldIndex)br.ReadByte());
                else
                    fieldIndexList.Add((ReqFieldIndex)br.ReadInt16());
            }

            ConvertFieldIndex.PushType pushType = ConvertFieldIndex.PushType.Quote;
            if (PacketStatus == 0)
            {
                foreach (KeyValuePair<ConvertFieldIndex.PushType, short[]> entry in ConvertFieldIndex.PushTypeArray)
                {
                    if (Array.IndexOf(entry.Value, (short)fieldIndexList[0]) >= 0)
                        pushType = entry.Key;
                }
            }

            byte biaoshiLen = br.ReadByte(); //股票数据标识所占的字节数
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                if (PacketStatus == 1)
                {
                    ReadFieldIndex(fieldIndexList, br);
                }
                else if (PacketStatus == 0)
                {
                    int biaoshi = 0;
                    if (biaoshiLen == 1)
                        biaoshi = br.ReadByte();
                    else
                    {
                        byte[] biaoshiBytes = br.ReadBytes(biaoshiLen);
                        if (biaoshiLen <= 2)
                            biaoshi = BitConverter.ToInt16(biaoshiBytes, 0);
                        else
                            biaoshi = BitConverter.ToInt32(biaoshiBytes, 0);
                    }
                    List<ReqFieldIndex> fields = new List<ReqFieldIndex>();
                    int len = biaoshiLen * 8;
                    for (int j = 0; j < len; j++)
                    {
                        int tmp = 1 << j;
                        if ((tmp & biaoshi) != 0)
                        {
                            ReqFieldIndex tmpField;
                            if (ConvertFieldIndex.ConvertPushToReq(pushType, j, out tmpField))
                                fields.Add(tmpField);
                        }
                    }

                    ReadFieldIndex(fields, br);
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 自选股类型板块栏位的报价
    /// </summary>
    public class ResBlockIndexReportDataPacket : RealTimeDataPacket
    {
        private byte _packetStatus;
        /// <summary>
        /// 响应包的标识，1表示开始包，0表示中间包，2表示结尾包，其他表示错误包
        /// </summary>
        public byte PacketStatus
        {
            get { return _packetStatus; }
            private set { this._packetStatus = value; }
        }

        Dictionary<int, Dictionary<byte, object>> _dicFieldValue;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, Dictionary<byte, object>> DicFieldValue
        {
            get { return _dicFieldValue; }
            private set { this._dicFieldValue = value; }
        }

        Dictionary<int, List<FieldIndex>> _changedFields;
        /// <summary>
        /// 有数据改变的字段
        /// </summary>
        public Dictionary<int, List<FieldIndex>> ChangedFields
        {
            get { return _changedFields; }
            private set { this._changedFields = value; }
        }

        private void ReadFieldIndex(List<ReqBlockFieldIndex> reqFieldIndex, BinaryReader br)
        {
            try
            {
                Dictionary<byte, object> result = new Dictionary<byte, object>(reqFieldIndex.Count);
                List<FieldIndex> fieldClient = new List<FieldIndex>();

                uint stockId = 0;

                foreach (ReqBlockFieldIndex field in reqFieldIndex)
                {
                    DicFieldIndexItem fieldIndexItem = ConvertBlockFieldIndex.DicFieldIndex[field];
                    switch (fieldIndexItem.FieldTypeServer)
                    {
                        case ReqFieldIndexType.FT_STOCKID:
                            stockId = br.ReadUInt32();
                            break;
                        case ReqFieldIndexType.FT_CODE:
                            byte[] codeBytes = br.ReadBytes(7);
                            //                         string code = Encoding.ASCII.GetString(codeBytes);
                            //                         code = code.TrimEnd('\0');
                            //                         result.Add(field,codeBytes);
                            break;
                        case ReqFieldIndexType.FT_BYTE:
                            byte byteResult = br.ReadByte();
                            result.Add(Convert.ToByte(field), byteResult);
                            break;
                        case ReqFieldIndexType.FT_INT32:
                            int intResult = br.ReadInt32();
                            result.Add(Convert.ToByte(field), intResult);
                            break;
                        case ReqFieldIndexType.FT_UINT32:
                            uint uintResult = br.ReadUInt32();
                            result.Add(Convert.ToByte(field), uintResult);
                            break;
                        case ReqFieldIndexType.FT_LISTFLOWITEM:
                            ListFlowItem item = new ListFlowItem();
                            item.PercentDec = br.ReadInt32();
                            item.HisPercentDecRange = br.ReadInt32();
                            item.DiffRanger = br.ReadInt32();
                            result.Add(Convert.ToByte(field), item);
                            break;
                        case ReqFieldIndexType.FT_LISTFLOWDETAILITEM:
                            ListFlowDetailItem detailItem = new ListFlowDetailItem();
                            detailItem.Buy = br.ReadInt32();
                            detailItem.Sell = br.ReadInt32();
                            result.Add(Convert.ToByte(field), detailItem);
                            break;
                    }

                    switch (field)
                    {
                        case ReqBlockFieldIndex.Now:
                        case ReqBlockFieldIndex.PreClose:
                            fieldClient.Add(FieldIndex.Difference);
                            fieldClient.Add(FieldIndex.DifferRange);
                            break;
                        case ReqBlockFieldIndex.High:
                        case ReqBlockFieldIndex.Low:
                            fieldClient.Add(FieldIndex.Delta);
                            break;
                        case ReqBlockFieldIndex.Volume:
                            fieldClient.Add(FieldIndex.Turnover);
                            break;
                        case ReqBlockFieldIndex.SListFlowDetailItemBig:
                            fieldClient.Add(FieldIndex.BuyBig);
                            fieldClient.Add(FieldIndex.SellBig);
                            fieldClient.Add(FieldIndex.NetFlowBig);
                            fieldClient.Add(FieldIndex.NetFlowRangeBig);
                            fieldClient.Add(FieldIndex.MainNetFlow);
                            fieldClient.Add(FieldIndex.BuyFlowRangeBig);
                            fieldClient.Add(FieldIndex.SellFlowRangeBig);
                            break;
                        case ReqBlockFieldIndex.SListFlowDetailItemMiddle:
                            fieldClient.Add(FieldIndex.BuyMiddle);
                            fieldClient.Add(FieldIndex.SellMiddle);
                            fieldClient.Add(FieldIndex.NetFlowMiddle);
                            fieldClient.Add(FieldIndex.NetFlowRangeMiddle);
                            break;
                        case ReqBlockFieldIndex.SListFlowDetailItemSmall:
                            fieldClient.Add(FieldIndex.BuySmall);
                            fieldClient.Add(FieldIndex.SellSmall);
                            fieldClient.Add(FieldIndex.NetFlowSmall);
                            fieldClient.Add(FieldIndex.NetFlowRangeSmall);
                            break;
                        case ReqBlockFieldIndex.SListFlowDetailItemSuper:
                            fieldClient.Add(FieldIndex.BuySuper);
                            fieldClient.Add(FieldIndex.SellSuper);
                            fieldClient.Add(FieldIndex.NetFlowSuper);
                            fieldClient.Add(FieldIndex.NetFlowRangeSuper);
                            fieldClient.Add(FieldIndex.MainNetFlow);
                            fieldClient.Add(FieldIndex.BuyFlowRangeSuper);
                            fieldClient.Add(FieldIndex.SellFlowRangeSuper);
                            break;
                        case ReqBlockFieldIndex.SListFlowItemDay:
                            fieldClient.Add(FieldIndex.ZengCangRange);
                            fieldClient.Add(FieldIndex.ZengCangRank);
                            fieldClient.Add(FieldIndex.ZengCangRankChange);
                            break;
                        case ReqBlockFieldIndex.SListFlowItemDay3:
                            fieldClient.Add(FieldIndex.ZengCangRangeDay3);
                            fieldClient.Add(FieldIndex.ZengCangRankDay3);
                            fieldClient.Add(FieldIndex.ZengCangRankChangeDay3);
                            break;
                        case ReqBlockFieldIndex.SListFlowItemDay5:
                            fieldClient.Add(FieldIndex.ZengCangRangeDay5);
                            fieldClient.Add(FieldIndex.ZengCangRankDay5);
                            fieldClient.Add(FieldIndex.ZengCangRankChangeDay5);
                            break;
                        case ReqBlockFieldIndex.SListFlowItemDay10:
                            fieldClient.Add(FieldIndex.ZengCangRangeDay10);
                            fieldClient.Add(FieldIndex.ZengCangRankDay10);
                            fieldClient.Add(FieldIndex.ZengCangRankChangeDay10);
                            break;

                    }
                    if (fieldIndexItem.FieldClient != FieldIndex.Na)
                        fieldClient.Add(fieldIndexItem.FieldClient);
                }
                string emCode = ConvertCode.ConvertIntToCode(stockId);
                if (emCode != null && DetailData.EmCodeToUnicode.ContainsKey(emCode))
                {
                    int unicode =
                        DetailData.EmCodeToUnicode[emCode];
                    if (!DicFieldValue.ContainsKey(unicode))
                        DicFieldValue.Add(unicode, result);
                    if (!ChangedFields.ContainsKey(unicode))
                        ChangedFields.Add(unicode, fieldClient);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
            }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte bResponse = br.ReadByte();
            int id = br.ReadInt32();
            PacketStatus = br.ReadByte(); //0中间，1开始，2末尾，3未知
            int nStock = br.ReadInt32(); //股票数
            DicFieldValue = new Dictionary<int, Dictionary<byte, object>>(nStock);
            ChangedFields = new Dictionary<int, List<FieldIndex>>(nStock);

            byte nNumBits = br.ReadByte(); //栏位字节，0-1字节，其他-2字节
            short nField = br.ReadInt16(); //栏位个数
            List<ReqBlockFieldIndex> fieldIndexList = new List<ReqBlockFieldIndex>(nField);
            for (int i = 0; i < nField; i++)
            {
                if (nNumBits == 0)
                    fieldIndexList.Add((ReqBlockFieldIndex)br.ReadByte());
                else
                    fieldIndexList.Add((ReqBlockFieldIndex)br.ReadInt16());
            }

            ConvertBlockFieldIndex.BlockPushType pushType = ConvertBlockFieldIndex.BlockPushType.Quote;
            if (PacketStatus == 0)
            {
                foreach (
                    KeyValuePair<ConvertBlockFieldIndex.BlockPushType, short[]> entry in
                        ConvertBlockFieldIndex.PushTypeArray)
                {
                    if (Array.IndexOf(entry.Value, (short)fieldIndexList[0]) >= 0)
                        pushType = entry.Key;
                }
            }

            byte biaoshiLen = br.ReadByte(); //股票数据标识所占的字节数
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                if (PacketStatus == 1)
                {
                    ReadFieldIndex(fieldIndexList, br);
                }
                else if (PacketStatus == 0)
                {
                    int biaoshi = 0;
                    if (biaoshiLen == 1)
                        biaoshi = br.ReadByte();
                    else
                    {
                        byte[] biaoshiBytes = br.ReadBytes(biaoshiLen);
                        if (biaoshiLen <= 2)
                            biaoshi = BitConverter.ToInt16(biaoshiBytes, 0);
                        else
                            biaoshi = BitConverter.ToInt32(biaoshiBytes, 0);
                    }
                    List<ReqBlockFieldIndex> fields = new List<ReqBlockFieldIndex>();
                    int len = biaoshiLen * 8;
                    for (int j = 0; j < len; j++)
                    {
                        int tmp = 1 << j;
                        if ((tmp & biaoshi) != 0)
                        {
                            ReqBlockFieldIndex tmpField;
                            if (ConvertBlockFieldIndex.ConvertPushToReq(pushType, j, out tmpField))
                                fields.Add(tmpField);
                        }
                    }

                    ReadFieldIndex(fields, br);
                }
            }
            return true;
        }
    }


    /// <summary>
    /// 成交明细推送
    /// </summary>
    public class ResDealSubscribeDataPacket : RealTimeDataPacket
    {
        OneStockDealDataRec _oneStockDealDatas;
        /// <summary>
        /// 
        /// </summary>
        public OneStockDealDataRec OneStockDealDatas
        {
            get { return _oneStockDealDatas; }
            private set { this._oneStockDealDatas = value; }
        }

        bool _isIndexFutures;
        /// <summary>
        /// 
        /// </summary>
        public bool IsIndexFutures
        {
            get { return _isIndexFutures; }
            private set { this._isIndexFutures = value; }
        }

        /// <summary>
        /// 可转债
        /// </summary>
        public bool IsConvertBond;

        /// <summary>
        /// 非可转债
        /// </summary>
        public bool IsNonConvertBond;

        /// <summary>
        /// 
        /// </summary>
        public ResDealSubscribeDataPacket()
        {
            OneStockDealDatas = new OneStockDealDataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');

            string emCode = GetEmCode((ReqMarketType)market, code);

            if (emCode.EndsWith("CFE"))
            {
                if (emCode.StartsWith("04"))
                {
                    if (emCode == "040120.CFE")
                        emCode = "IF00C1.CFE";
                    else if (emCode == "040121.CFE")
                        emCode = "IF00C2.CFE";
                    else if (emCode == "040122.CFE")
                        emCode = "IF00C3.CFE";
                    else if (emCode == "040123.CFE")
                        emCode = "IF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }
                else if (emCode.StartsWith("05"))
                {
                    if (emCode == "050120.CFE")
                        emCode = "TF00C1.CFE";
                    else if (emCode == "050121.CFE")
                        emCode = "TF00C2.CFE";
                    else if (emCode == "050122.CFE")
                        emCode = "TF00C3.CFE";
                    else if (emCode == "050123.CFE")
                        emCode = "TF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }

            }

            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;

            OneStockDealDatas.Code = DetailData.EmCodeToUnicode[emCode];

            int num = br.ReadInt32();
            OneStockDealDatas.DealDatas = new List<OneDealDataRec>(num);

            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(OneStockDealDatas.Code))
                DetailData.FieldIndexDataInt32[OneStockDealDatas.Code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;



            switch (mt)
            {
                case MarketType.IF:
                case MarketType.GoverFutures:
                    IsIndexFutures = true;
                    break;
                case MarketType.SHConvertBondLev1:
                case MarketType.SHConvertBondLev2:
                case MarketType.SZConvertBondLev1:
                case MarketType.SZConvertBondLev2:
                    IsConvertBond = true;
                    break;
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHNonConvertBondLev2:
                case MarketType.SZNonConvertBondLev1:
                case MarketType.SZNonConvertBondLev2:
                    IsNonConvertBond = true;
                    break;
            }


            for (int i = 0; i < num; i++)
            {
                OneDealDataRec oneDeal;
                if (IsIndexFutures)
                    oneDeal = new OneIndexFuturesDealDataRec();
                else if (IsNonConvertBond || IsConvertBond)
                    oneDeal = new OneBondDealDataRec();
                else
                    oneDeal = new OneDealDataRec();

                oneDeal.Hour = br.ReadByte();
                oneDeal.Mint = br.ReadByte();
                oneDeal.Second = br.ReadByte();
                oneDeal.Flag = br.ReadByte();
                oneDeal.Price = br.ReadInt32() / 1000.0f;
                oneDeal.Volume = br.ReadInt32();
                oneDeal.TradeNum = br.ReadInt16();

                if (IsIndexFutures)
                {
                    OneIndexFuturesDealDataRec tmp = oneDeal as OneIndexFuturesDealDataRec;
                    tmp.OpenVolume = (short)((oneDeal.Volume + oneDeal.TradeNum) / 2);
                    tmp.CloseVolume = (short)((oneDeal.Volume - oneDeal.TradeNum) / 2);
                    tmp.OpenCloseStatus = GetIFOpenCloseStatus(oneDeal.Volume, oneDeal.TradeNum, oneDeal.Flag);

                    //Dictionary<FieldIndex, object> fieldsObjects;
                    //if (DetailData.AllStockDetailData.TryGetValue(OneStockDealDatas.Code, out fieldsObjects))
                    //{
                    //    //if (fieldsObjects.ContainsKey(FieldIndex.OpenCloseStatus))
                    //        fieldsObjects[FieldIndex.OpenCloseStatus] = tmp.OpenCloseStatus;
                    //    //if (fieldsObjects.ContainsKey(FieldIndex.BSFlag))
                    //        fieldsObjects[FieldIndex.BSFlag] = tmp.Flag;
                    //}
                }
                else if (IsNonConvertBond || IsConvertBond)
                {
                    OneBondDealDataRec tmpBond = oneDeal as OneBondDealDataRec;
                    Dictionary<float, float> memYtmData;
                    if (DetailData.AllBondYtmDataRec.TryGetValue(OneStockDealDatas.Code, out memYtmData))
                    {
                        float memYtm;
                        if (memYtmData.TryGetValue(oneDeal.Price, out memYtm))
                        {
                            if (tmpBond != null) tmpBond.BondYtm = memYtm;
                        }
                        else
                        {
                            string cmd = string.Empty;
                            if (IsNonConvertBond)
                                cmd = string.Format(
                               @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=1 columns=netPrice,ytm",
                               emCode, oneDeal.Price);
                            else
                            {
                                cmd = string.Format(
                               @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=0 columns=netPrice,ytm",
                               emCode, oneDeal.Price);
                            }
                            DataTable dt = Requestor.GetDataTable(cmd, null, null, null);
                            float ytm = Convert.ToSingle(dt.Rows[0]["ytm"]);
                            if (!float.IsInfinity(ytm))
                            {
                                if (tmpBond != null) tmpBond.BondYtm = ytm;
                                memYtmData[oneDeal.Price] = ytm;
                            }
                            dt.Dispose();
                        }
                    }
                    else
                    {
                        string cmd = string.Empty;
                        if (IsNonConvertBond)
                            cmd = string.Format(
                           @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=1 columns=netPrice,ytm",
                           emCode, oneDeal.Price);
                        else
                        {
                            cmd = string.Format(
                           @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=0 columns=netPrice,ytm",
                           emCode, oneDeal.Price);
                        }
                        DataTable dt = Requestor.GetDataTable(cmd, null, null, null);
                        float ytm = Convert.ToSingle(dt.Rows[0]["ytm"]);
                        if (!float.IsInfinity(ytm))
                        {
                            if (tmpBond != null) tmpBond.BondYtm = ytm;
                            Dictionary<float, float> tmpData = new Dictionary<float, float>(1);
                            tmpData[oneDeal.Price] = ytm;
                            DetailData.AllBondYtmDataRec[OneStockDealDatas.Code] = tmpData;
                        }
                        dt.Dispose();
                    }
                }

                OneStockDealDatas.DealDatas.Add(oneDeal);
            }
            return true;
        }
    }

    /// <summary>
    /// 成交明细请求
    /// </summary>
    public class ResDealRequestDataPacket : RealTimeDataPacket
    {
        OneStockDealDataRec _oneStockDealDatas;
        /// <summary>
        /// 
        /// </summary>
        public OneStockDealDataRec OneStockDealDatas
        {
            get { return _oneStockDealDatas; }
            private set { this._oneStockDealDatas = value; }
        }

        bool _isIndexFutures;
        /// <summary>
        /// 
        /// </summary>
        public bool IsIndexFutures
        {
            get { return _isIndexFutures; }
            private set { this._isIndexFutures = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResDealRequestDataPacket()
        {
            OneStockDealDatas = new OneStockDealDataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int date = br.ReadInt32();
            int time = br.ReadInt32();
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');

            string emCode = GetEmCode((ReqMarketType)market, code);

            if (emCode.EndsWith("CFE"))
            {
                if (emCode.StartsWith("04"))
                {
                    if (emCode == "040120.CFE")
                        emCode = "IF00C1.CFE";
                    else if (emCode == "040121.CFE")
                        emCode = "IF00C2.CFE";
                    else if (emCode == "040122.CFE")
                        emCode = "IF00C3.CFE";
                    else if (emCode == "040123.CFE")
                        emCode = "IF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }
                else if (emCode.StartsWith("05"))
                {
                    if (emCode == "050120.CFE")
                        emCode = "TF00C1.CFE";
                    else if (emCode == "050121.CFE")
                        emCode = "TF00C2.CFE";
                    else if (emCode == "050122.CFE")
                        emCode = "TF00C3.CFE";
                    else if (emCode == "050123.CFE")
                        emCode = "TF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }

            }

            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            OneStockDealDatas.Code = DetailData.EmCodeToUnicode[emCode];

            int total = br.ReadInt32(); //total?
            int num = br.ReadInt32(); //条数

            OneStockDealDatas.DealDatas = new List<OneDealDataRec>(num);

            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(OneStockDealDatas.Code))
                DetailData.FieldIndexDataInt32[OneStockDealDatas.Code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;



            if (mt == MarketType.IF || mt == MarketType.GoverFutures)
                IsIndexFutures = true;


            for (int i = 0; i < num; i++)
            {
                OneDealDataRec oneDeal;
                if (IsIndexFutures)
                    oneDeal = new OneIndexFuturesDealDataRec();
                else
                    oneDeal = new OneDealDataRec();

                oneDeal.Hour = br.ReadByte();
                oneDeal.Mint = br.ReadByte();
                oneDeal.Second = br.ReadByte();
                oneDeal.Flag = br.ReadByte();
                oneDeal.Price = br.ReadInt32() / 1000.0f;
                oneDeal.Volume = br.ReadInt32();
                oneDeal.TradeNum = br.ReadInt16();

                if (IsIndexFutures)
                {
                    OneIndexFuturesDealDataRec tmp = oneDeal as OneIndexFuturesDealDataRec;
                    tmp.OpenVolume = (short)((oneDeal.Volume + oneDeal.TradeNum) / 2);
                    tmp.CloseVolume = (short)((oneDeal.Volume - oneDeal.TradeNum) / 2);
                    tmp.OpenCloseStatus = GetIFOpenCloseStatus(oneDeal.Volume, oneDeal.TradeNum, oneDeal.Flag);
                    //                     tmp.OpenVolume = (short)((oneDeal.Volume + oneDeal.TradeNum) / 2);
                    //                     tmp.CloseVolume = (short)((oneDeal.Volume - oneDeal.TradeNum) / 2);
                    //                     if (oneDeal.Flag == 1)//卖
                    //                     {
                    //                         if (tmp.OpenVolume == tmp.CloseVolume)
                    //                             tmp.OpenCloseStatus = "空换";
                    //                         else if (tmp.OpenVolume == 0)
                    //                             tmp.OpenCloseStatus = "双平";
                    //                         else if (tmp.CloseVolume == 0)
                    //                             tmp.OpenCloseStatus = "双开";
                    //                         else if (tmp.OpenVolume > tmp.CloseVolume)
                    //                             tmp.OpenCloseStatus = "空开";
                    //                         else if (tmp.OpenVolume < tmp.CloseVolume)
                    //                             tmp.OpenCloseStatus = "多平";
                    //                     }
                    //                     else
                    //                     {
                    //                         if (tmp.OpenVolume == tmp.CloseVolume)
                    //                             tmp.OpenCloseStatus = "多换";
                    //                         else if (tmp.OpenVolume == 0)
                    //                             tmp.OpenCloseStatus = "双平";
                    //                         else if (tmp.CloseVolume == 0)
                    //                             tmp.OpenCloseStatus = "双开";
                    //                         else if (tmp.OpenVolume > tmp.CloseVolume)
                    //                             tmp.OpenCloseStatus = "多开";
                    //                         else if (tmp.OpenVolume < tmp.CloseVolume)
                    //                             tmp.OpenCloseStatus = "空平";
                    //                     }
                }

                OneStockDealDatas.DealDatas.Add(oneDeal);
            }
            return true;
        }
    }

    /// <summary>
    /// 板块信息
    /// </summary>
    public class ResBlockOverViewDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(System.IO.BinaryReader br)
        {
            uint id = br.ReadUInt32();
            byte status = br.ReadByte();
            byte cType = br.ReadByte();
            uint size = br.ReadUInt32();

            int lenOrigin = br.ReadInt32();
            int lenCompressed = br.ReadInt32();
            byte[] bytesCompressed = br.ReadBytes(lenCompressed);
            byte[] bytesOrigin = new byte[lenOrigin];

            int nUncompress = doGeneralUncompress(bytesOrigin, ref lenOrigin, bytesCompressed, lenCompressed);
            return true;

        }
    }

    /// <summary>
    /// 板块简易行情
    /// </summary>
    public class ResBlockSimpleQuoteDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            uint id = br.ReadUInt32();
            byte status = br.ReadByte();
            int compressedSize1 = br.ReadInt32();
            //byte[] bytesCompressed = br.ReadBytes(compressedSize1);
            int originalSize = br.ReadInt32();
            //             int compressedSize = br.ReadInt32();
            byte[] bytesCompressed = br.ReadBytes(compressedSize1);
            byte[] bytesDecompress = new byte[originalSize];
            int tmp = doGeneralUncompress(bytesDecompress, ref originalSize, bytesCompressed, compressedSize1);
            return true;
        }
    }

    /// <summary>
    /// 个股资金流
    /// </summary>
    public class ResCapitalFlowDataPacket : RealTimeDataPacket
    {
        CapitalFlowDataRec _capitalFlowData;
        /// <summary>
        /// 个股资金流向数据
        /// </summary>
        public CapitalFlowDataRec CapitalFlowData
        {
            get { return _capitalFlowData; }
            private set { this._capitalFlowData = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            uint id = br.ReadUInt32();
            byte status = br.ReadByte();
            uint uicode = br.ReadUInt32();

            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            byte market = br.ReadByte();
            CapitalFlowData = new CapitalFlowDataRec();

            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;

            CapitalFlowData.Code = DetailData.EmCodeToUnicode[emCode];
            CapitalFlowData.Now = br.ReadSingle();
            CapitalFlowData.DiffRange = br.ReadSingle();
            CapitalFlowData.AmountCallAution = br.ReadDouble();

            for (int i = 0; i < CapitalFlowData.FlowItems.Length; i++)
            {
                CapitalFlowDetailItem item = new CapitalFlowDetailItem();
                item.AmountBuy = br.ReadDouble();
                item.AmountSell = br.ReadDouble();
                item.AmountAdd = br.ReadDouble();
                item.AmountAddRange = br.ReadSingle();
                item.AmountNet = br.ReadDouble();
                item.AmountNetRanger = br.ReadSingle();
                CapitalFlowData.FlowItems[i] = item;
            }

            for (int i = 0; i < CapitalFlowData.FlowDays.Length; i++)
            {
                OneDayCapitalFlow item = new OneDayCapitalFlow();
                item.SmallBuy = br.ReadDouble();
                item.SmallSell = br.ReadDouble();
                item.MiddleBuy = br.ReadDouble();
                item.MiddleSell = br.ReadDouble();
                item.LargeBuy = br.ReadDouble();
                item.LargeSell = br.ReadDouble();
                item.SuperBuy = br.ReadDouble();
                item.SuperSell = br.ReadDouble();
                item.Amount = br.ReadDouble();
                CapitalFlowData.FlowDays[i] = item;
            }

            for (int i = 0; i < CapitalFlowData.FlowDays.Length; i++)
            {
                CapitalFlowData.FlowDays[i].LargeNetRange = br.ReadSingle();
                CapitalFlowData.FlowDays[i].RankRec = br.ReadInt16();
                CapitalFlowData.FlowDays[i].RankChange = br.ReadInt16();
                CapitalFlowData.FlowDays[i].HisNetRank = br.ReadInt16();
                CapitalFlowData.FlowDays[i].HisNetRange = br.ReadSingle();
                CapitalFlowData.FlowDays[i].Percent = br.ReadSingle();
            }

            for (int i = 0; i < CapitalFlowData.FlowItems.Length; i++)
                CapitalFlowData.FlowItems[i].VolumeBuy = br.ReadUInt64();

            for (int i = 0; i < CapitalFlowData.FlowItems.Length; i++)
                CapitalFlowData.FlowItems[i].VolumeSell = br.ReadUInt64();

            for (int i = 0; i < CapitalFlowData.FlowItems.Length; i++)
                CapitalFlowData.FlowItems[i].BishuBuy = br.ReadUInt32();

            for (int i = 0; i < CapitalFlowData.FlowItems.Length; i++)
                CapitalFlowData.FlowItems[i].BishuSell = br.ReadUInt32();

            return true;
        }
    }

    /// <summary>
    /// 分价表
    /// </summary>
    public class ResPriceStatusDataPacket : RealTimeDataPacket
    {
        PriceStatusDataRec _priceStatusData;
        /// <summary>
        /// 
        /// </summary>
        public PriceStatusDataRec PriceStatusData
        {
            get { return _priceStatusData; }
            private set { this._priceStatusData = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            uint stockId = br.ReadUInt32();
            string emCode = ConvertCode.ConvertIntToCode(stockId);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;

            PriceStatusData = new PriceStatusDataRec();
            PriceStatusData.Code = DetailData.EmCodeToUnicode[emCode];
            int num = br.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                OnePriceStatus priceStatus = new OnePriceStatus();
                priceStatus.Price = br.ReadSingle();
                priceStatus.BuyVolume = br.ReadDouble();
                priceStatus.SellVolume = br.ReadDouble();
                PriceStatusData.PriceStatusList.Insert(0, priceStatus);
            }
            return true;
        }
    }

    /// <summary>
    /// 股指期货分价表
    /// </summary>
    public class ResIndexFuturesPriceStatusDataPacket : RealTimeDataPacket
    {

    }

    /// <summary>
    /// 综合排名
    /// </summary>
    public class ResRankDataPacket : RealTimeDataPacket
    {
        RankDataRec _rankData;
        /// <summary>
        /// 排序数据
        /// </summary>
        public RankDataRec RankData
        {
            get { return _rankData; }
            private set { this._rankData = value; }
        }

        private const int RankSize = 20;

        /// <summary>
        /// 
        /// </summary>
        public ResRankDataPacket()
        {
            RankData = new RankDataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            RankData.SType = (ReqSecurityType)br.ReadByte();
            for (int i = 0; i < RankData.RankElementArray.Length; i++)
            {
                List<RankElement> elements = new List<RankElement>(RankSize);

                for (int j = 0; j < RankSize; j++)
                {
                    RankElement element = new RankElement();
                    byte[] codeBytes = br.ReadBytes(7);
                    string emCode = Encoding.ASCII.GetString(codeBytes);
                    emCode = emCode.TrimEnd('\0');
                    if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                        return false;
                    element.Code = DetailData.EmCodeToUnicode[emCode];
                    byte[] nameBytes = br.ReadBytes(9);
                    string name = Encoding.Default.GetString(nameBytes);
                    name = name.TrimEnd('\0');
                    element.Name = name;
                    element.PreClose = br.ReadSingle();
                    element.Now = br.ReadSingle();
                    element.Volume = br.ReadInt32();
                    element.IndexValue = br.ReadSingle();
                    elements.Add(element);
                }

                RankData.RankElementArray[i] = elements;
            }
            return true;
        }
    }

    /// <summary>
    /// F10
    /// </summary>
    public class ResF10DataPacket : RealTimeDataPacket
    {
        F10DataRec _f10Data;
        /// <summary>
        /// F10数据
        /// </summary>
        public F10DataRec F10Data
        {
            get { return _f10Data; }
            private set { this._f10Data = value; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public short FieldType;

        /// <summary>
        /// 
        /// </summary>
        public ResF10DataPacket()
        {
            F10Data = new F10DataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            F10Data.Code = DetailData.EmCodeToUnicode[emCode];

            F10Field field = new F10Field();
            FieldType = br.ReadByte();
            field.Date = br.ReadInt32();
            field.Time = br.ReadInt32();
            int len = br.ReadInt32();
            if (len == 0)
                return false;

            byte[] contextBytes = br.ReadBytes(len);
            field.Context = Encoding.Default.GetString(contextBytes);

            F10Data.F10FieldData.Add(Convert.ToInt32(FieldType), field);
            return true;
        }
    }
    /// <summary>
    /// 历史分时资金流 响应包(暂停|| 等待解压接口)
    /// </summary>
    public class ResHisTrendlinecfsDataPacket : RealTimeDataPacket
    {
        private UInt32 _stockId;
        private UInt32 _date;
        private UInt32 _compDataLen;
        private byte _compData;

        /// <summary>
        /// stock id
        /// </summary>
        public UInt32 StockId
        {
            get { return _stockId; }
            set { _stockId = value; }
        }
        /// <summary>
        /// 趋势线日期
        /// </summary>
        public UInt32 Date
        {
            get { return _date; }
            set { _date = value; }
        }
        /// <summary>
        /// 趋势线压缩数据长度
        /// </summary>
        public UInt32 CompDataLen
        {
            get { return _compDataLen; }
            set { _compDataLen = value; }
        }
        /// <summary>
        /// 趋势线压缩数据
        /// </summary>
        public byte CompData
        {
            get { return _compData; }
            set { _compData = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public ResHisTrendlinecfsDataPacket()
        {

        }
        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            _stockId = br.ReadUInt32();
            _date = br.ReadUInt32();
            _compDataLen = br.ReadUInt32();
            _compData = br.ReadByte();

            return true;

        }
    }
    /// <summary>
    /// 资金流日K线 响应包
    /// </summary>
    public class ResCapitalFlowDayDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private KLineCycle _dataType;
        private int _dayDataNum;
        private int _code;

        /// <summary>
        /// 股票代码(内码)
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> _dicCapitalFlowDayData;
        /// <summary>
        /// 资金流日K线 数据集合
        /// </summary>
        public Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> DicCapitalFlowDayData
        {
            get { return _dicCapitalFlowDayData; }
            set { _dicCapitalFlowDayData = value; }
        }
        private CapitalFlowDayKLineDataRecs _kLineData;
        private TrendCaptialFlowDataRec _item;

        /// <summary>
        /// 股票简码转化为内码
        /// </summary>
        /// <param name="shortCode">股票简码</param>
        /// <param name="market">市场代码</param>
        /// <param name="code">股票内码</param>
        /// <returns>true for convert successfully, otherwise false</returns>
        public bool TryConvertShortCode2Code(string shortCode, byte market, out  int code)
        {
            code = 0;
            shortCode = shortCode.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, shortCode);
            if (emCode == null)
                return false;
            if (emCode.EndsWith("CFE"))
            {
                if (emCode.StartsWith("04"))
                {
                    if (emCode == "040120.CFE")
                        emCode = "IF00C1.CFE";
                    else if (emCode == "040121.CFE")
                        emCode = "IF00C2.CFE";
                    else if (emCode == "040122.CFE")
                        emCode = "IF00C3.CFE";
                    else if (emCode == "040123.CFE")
                        emCode = "IF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr = string.Empty;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }
                else if (emCode.StartsWith("05"))
                {
                    if (emCode == "050120.CFE")
                        emCode = "TF00C1.CFE";
                    else if (emCode == "050121.CFE")
                        emCode = "TF00C2.CFE";
                    else if (emCode == "050122.CFE")
                        emCode = "TF00C3.CFE";
                    else if (emCode == "050123.CFE")
                        emCode = "TF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr = string.Empty;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }
            }

            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;

            code = DetailData.EmCodeToUnicode[emCode];
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public ResCapitalFlowDayDataPacket()
        {
            _dicCapitalFlowDayData = new Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            _market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            _shortCode = Encoding.ASCII.GetString(codeBytes);

            if (!TryConvertShortCode2Code(_shortCode, _market, out _code))
                return false;

            _dataType = (KLineCycle)br.ReadByte();
            _dayDataNum = br.ReadInt32();

            if (!_dicCapitalFlowDayData.TryGetValue(_dataType, out _kLineData))
            {
                _kLineData = new CapitalFlowDayKLineDataRecs(_dayDataNum);
            }
            _kLineData.Code = _code;
            _kLineData.Cycle = _dataType;

            _dicCapitalFlowDayData[_dataType] = _kLineData;

            byte k = 0;
            for (int i = 0; i < _dayDataNum; i++)
            {
                _item = new TrendCaptialFlowDataRec();
                _item.Time = br.ReadUInt32();
                for (k = 0; k < 4; k++)
                    _item.BuyAmount[k] = br.ReadDouble();
                for (k = 0; k < 4; k++)
                    _item.SellAmount[k] = br.ReadDouble();

                for (k = 0; k < 4; k++)
                    _item.BuyVolume[k] = br.ReadUInt64();
                for (k = 0; k < 4; k++)
                    _item.SellVolume[k] = br.ReadUInt64();

                for (k = 0; k < 4; k++)
                    _item.BuyNum[k] = br.ReadUInt32();
                for (k = 0; k < 4; k++)
                    _item.SellNum[k] = br.ReadUInt32();

                _kLineData.SortDicCaptialFlowList[(int)_item.Time] = _item;
            }
            return true;
        }
    }


    /// <summary>
    /// 逐笔成交
    /// </summary>
    public class ResTickDataPacket : RealTimeDataPacket
    {
        OneStockTickDataRec _tickData;
        /// <summary>
        /// 逐笔成交数据
        /// </summary>
        public OneStockTickDataRec TickData
        {
            get { return _tickData; }
            private set { this._tickData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResTickDataPacket()
        {
            TickData = new OneStockTickDataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int id = br.ReadInt32();
            byte status = br.ReadByte(); //0中间，1开始，2末尾，3未知
            uint stockId = br.ReadUInt32();
            string emCode = ConvertCode.ConvertIntToCode(stockId);

            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            TickData.Code = DetailData.EmCodeToUnicode[emCode];

            int beginIndex = br.ReadInt32();
            int num = br.ReadInt32();
            TickData.TickDatasList = new List<OneTickDataRec>(num);
            for (int i = 0; i < num; i++)
            {
                OneTickDataRec oneTickData = new OneTickDataRec();
                oneTickData.Time = br.ReadInt32();
                int tradeNo = br.ReadInt32();
                oneTickData.Flag = br.ReadByte();
                oneTickData.Price = br.ReadInt32();
                oneTickData.Volume = br.ReadInt32();
                oneTickData.Index = beginIndex + i;
                TickData.TickDatasList.Add(oneTickData);
            }
            return true;
        }
    }

    /// <summary>
    /// 委托明细
    /// </summary>
    public class ResOrderDetailDataPacket : RealTimeDataPacket
    {
        OneStockOrderDetailDataRec _orderDetailData;
        /// <summary>
        /// 委托明细数据
        /// </summary>
        public OneStockOrderDetailDataRec OrderDetailData
        {
            get { return _orderDetailData; }
            private set { this._orderDetailData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResOrderDetailDataPacket()
        {
            OrderDetailData = new OneStockOrderDetailDataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int id = br.ReadInt32();
            byte status = br.ReadByte(); //0中间，1开始，2末尾，3未知
            uint stockId = br.ReadUInt32();
            string emCode = ConvertCode.ConvertIntToCode(stockId);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            OrderDetailData.Code = DetailData.EmCodeToUnicode[emCode];

            int beginIndex = br.ReadInt32();
            int num = br.ReadInt32();
            OrderDetailData.OrderDetailList = new List<OneOrderDetailDataRec>(num);
            for (int i = 0; i < num; i++)
            {
                OneOrderDetailDataRec oneOrderDetail = new OneOrderDetailDataRec();
                oneOrderDetail.Time = br.ReadInt32();
                int tradeNo = br.ReadInt32();
                oneOrderDetail.Price = br.ReadInt32();
                oneOrderDetail.Volume = br.ReadInt32();
                byte orderKind = br.ReadByte();
                oneOrderDetail.Flag = br.ReadByte();

                oneOrderDetail.Index = beginIndex + i;
                OrderDetailData.OrderDetailList.Add(oneOrderDetail);
            }
            return true;
        }
    }

    /// <summary>
    /// 委托队列
    /// </summary>
    public class ResOrderQueueDataPacket : RealTimeDataPacket
    {
        int _code;
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            private set { this._code = value; }
        }

        OrderQueueDataRec _orderQueueDataBuy;
        /// <summary>
        /// 
        /// </summary>
        public OrderQueueDataRec OrderQueueDataBuy
        {
            get { return _orderQueueDataBuy; }
            private set { this._orderQueueDataBuy = value; }
        }

        OrderQueueDataRec _orderQueueDataSell;
        /// <summary>
        /// 
        /// </summary>
        public OrderQueueDataRec OrderQueueDataSell
        {
            get { return _orderQueueDataSell; }
            private set { this._orderQueueDataSell = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            uint stockId = br.ReadUInt32();
            string emCode = ConvertCode.ConvertIntToCode(stockId);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];
            byte num = br.ReadByte();
            for (int i = 0; i < num; i++)
            {
                OrderQueueDataRec oneOrderQueue = new OrderQueueDataRec();
                int time = br.ReadInt32();
                float price = br.ReadSingle();
                oneOrderQueue.BuySell = br.ReadByte(); //1买，2卖
                if (oneOrderQueue.BuySell == 1)
                    OrderQueueDataBuy = oneOrderQueue;
                else
                    OrderQueueDataSell = oneOrderQueue;

                oneOrderQueue.TotalOrderNum = br.ReadInt32(); //总共多少笔，显示到界面
                oneOrderQueue.ShowOrderNum = br.ReadByte(); //显示的数据有多少笔

                for (int j = 0; j < oneOrderQueue.ShowOrderNum; j++)
                {
                    int volume = br.ReadInt32(); //委托单量
                    OrderQueueItemStatus status = (OrderQueueItemStatus)br.ReadByte();

                    OrderQueueItem item = null;
                    if (status == OrderQueueItemStatus.Part)
                        item = new OrderQueuePartDealItem();
                    else
                        item = new OrderQueueItem();

                    item.Volume = volume;
                    item.Status = status;
                    oneOrderQueue.ItemDatas.Add(item);
                }

            }
            return true;
        }

    }

    /// <summary>
    /// 短线精灵
    /// </summary>
    public class ResShortLineStragedytDataPacket : RealTimeDataPacket
    {
        //  public Dictionary<ShortLineType, Dictionary<int, OneShortLineDataRec>> DicShortLine { get; private set; }
        List<OneShortLineDataRec> _shortLineData;
        /// <summary>
        /// 短线精灵数据
        /// </summary>
        public List<OneShortLineDataRec> ShortLineData
        {
            get { return _shortLineData; }
            private set { this._shortLineData = value; }
        }

        byte _isValidate;
        /// <summary>
        /// 
        /// </summary>
        public byte IsValidate
        {
            get { return _isValidate; }
            private set { this._isValidate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResShortLineStragedytDataPacket()
        {
            // DicShortLine = new Dictionary<ShortLineType, Dictionary<int, OneShortLineDataRec>>();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            this.IsValidate = br.ReadByte();
            int capacity = br.ReadInt32();
            this.ShortLineData = new List<OneShortLineDataRec>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                OneShortLineDataRec item = new OneShortLineDataRec();
                br.ReadByte();
                int num3 = br.ReadInt32();
                ReqShortLineTye tye = (ReqShortLineTye)num3;
                switch (tye)
                {
                    case ReqShortLineTye.TractorsAskOrders:
                        item.SlType = ShortLineType.MultiSameAskOrders;
                        break;

                    case ReqShortLineTye.TractorsBidOrders:
                        item.SlType = ShortLineType.MultiSameBidOrders;
                        break;

                    default:
                        if (Enum.IsDefined(typeof(ShortLineType), tye.ToString()))
                        {
                            item.SlType = (ShortLineType)Enum.Parse(typeof(ShortLineType), ((ReqShortLineTye)num3).ToString());
                        }
                        break;
                }
                item.Time = br.ReadInt32();
                item.SeriesId = br.ReadInt32();
                string key = ConvertCode.ConvertIntToCode(br.ReadUInt32());
                if ((key == null) || !DetailData.EmCodeToUnicode.ContainsKey(key))
                {
                    return false;
                }
                item.Code = DetailData.EmCodeToUnicode[key];
                short count = br.ReadInt16();
                byte[] bytes = br.ReadBytes(count);
                item.Content = Encoding.Default.GetString(bytes);
                br.ReadByte();
                this.ShortLineData.Add(item);
            }
            CFTService.CallBack(FuncTypeRealTime.ShortLineStrategy, JsonConvert.SerializeObject(ShortLineData));
            return true;
        }
    }

    /// <summary>
    /// 个股指数贡献点
    /// </summary>
    public class ResContributionStockDataPacket : RealTimeDataPacket
    {
        List<ContributionDataRec> _contributionData;
        /// <summary>
        /// 个股指数贡献点数据
        /// </summary>
        public List<ContributionDataRec> ContributionData
        {
            get { return _contributionData; }
            private set { this._contributionData = value; }
        }

        ReqMarketType _mt;
        /// <summary>
        /// 
        /// </summary>
        public ReqMarketType Mt
        {
            get { return _mt; }
            private set { this._mt = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            Mt = ReqMarketType.MT_NA;
            if (market == 0)
                Mt = ReqMarketType.MT_SZ;
            else if (market == 1)
                Mt = ReqMarketType.MT_SH;
            if (Mt == ReqMarketType.MT_NA)
                return false;

            int num = br.ReadInt32();
            ContributionData = new List<ContributionDataRec>(num);

            for (int i = 0; i < num / 2; i++)
            {
                ContributionDataRec data = new ContributionDataRec();
                byte[] codeBytes = br.ReadBytes(7);
                string code = Encoding.ASCII.GetString(codeBytes);
                code = code.TrimEnd('\0');
                string emCode = GetEmCode(Mt, code);
                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                data.Code = DetailData.EmCodeToUnicode[emCode];
                data.Price = br.ReadSingle();
                ContributionData.Add(data);
            }

            for (int i = 0; i < num / 2; i++)
            {
                ContributionDataRec data = new ContributionDataRec();
                byte[] codeBytes = br.ReadBytes(7);
                string code = Encoding.ASCII.GetString(codeBytes);
                code = code.TrimEnd('\0');
                string emCode = GetEmCode(Mt, code);
                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                data.Code = DetailData.EmCodeToUnicode[emCode];

                data.Price = br.ReadSingle();
                ContributionData.Insert(num / 2, data);
            }

            return true;
        }
    }

    /// <summary>
    /// 板块指数贡献点
    /// </summary>
    public class ResContributionBlockDataPacket : RealTimeDataPacket
    {
        List<ContributionDataRec> _contributionData;
        /// <summary>
        /// 板块指数贡献点数据
        /// </summary>
        public List<ContributionDataRec> ContributionData
        {
            get { return _contributionData; }
            private set { this._contributionData = value; }
        }

        ReqMarketType _mt;
        /// <summary>
        /// 
        /// </summary>
        public ReqMarketType Mt
        {
            get { return _mt; }
            private set { this._mt = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            Mt = ReqMarketType.MT_NA;
            if (market == 0)
                Mt = ReqMarketType.MT_SZ;
            else if (market == 1)
                Mt = ReqMarketType.MT_SH;
            if (Mt == ReqMarketType.MT_NA)
                return false;

            int num = br.ReadInt32();
            ContributionData = new List<ContributionDataRec>(num);

            for (int i = 0; i < num; i++)
            {
                ContributionDataRec data = new ContributionDataRec();
                byte[] codeBytes = br.ReadBytes(7);
                string code = Encoding.ASCII.GetString(codeBytes);
                code = code.TrimEnd('\0');
                string emCode = GetEmCode(ReqMarketType.MT_Plate, code);
                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                data.Code = DetailData.EmCodeToUnicode[emCode];
                data.Price = br.ReadSingle();
                ContributionData.Add(data);
            }
            return true;
        }
    }

    /// <summary>
    /// 统计明细
    /// </summary>
    public class ResStatisticsAnalysisDataPacket : RealTimeDataPacket
    {
        public StatisticsAnalysisDataRec StatisticsData;
        public int Code;

        public override bool Decoding(BinaryReader br)
        {

            short version = br.ReadInt16();
            short type = br.ReadInt16();
            byte[] reserveBytes = br.ReadBytes(8);
            int updateTime = br.ReadInt32();

            switch (type)
            {
                case 1://normal
                    StatisticsData = new StatisticsAnalysisDataRec();
                    StatisticsData.WithdrawBuyNumber = br.ReadInt32();	// 买入撤单笔数
                    StatisticsData.WithdrawSellNumber = br.ReadInt32();	// 卖出撤单笔数
                    StatisticsData.WithdrawBuyAmount = br.ReadInt64();	// 买入撤单量
                    StatisticsData.WithdrawSellAmount = br.ReadInt64();	// 卖出撤单量
                    StatisticsData.WithdrawBuyMoney = br.ReadDouble();	// 买入撤单额
                    StatisticsData.WithdrawSellMoney = br.ReadDouble();	// 卖出撤单额
                    StatisticsData.WithdrawBuyAvg = br.ReadSingle();		// 买入撤单均价
                    StatisticsData.WithdrawSellAvg = br.ReadSingle();	// 卖出撤单均价
                    StatisticsData.WithdrawBuyAvgVal = br.ReadDouble();	// 买入撤单每笔均额
                    StatisticsData.WithdrawSellAvgVal = br.ReadDouble();	// 卖出撤单每笔均额
                    StatisticsData.TotalBidNumber = br.ReadInt32();		// 委托买入总笔数
                    StatisticsData.TotalOfferNumber = br.ReadInt32();	// 委托卖出总笔数
                    StatisticsData.NumBidOrders = br.ReadInt32();		// 委托买入总档数
                    StatisticsData.NumOfferOrders = br.ReadInt32();		// 委托卖出总档数
                    StatisticsData.BidTradeMaxDuration = br.ReadInt32();// 买入成交最大等待时间
                    StatisticsData.OfferTradeMaxDuration = br.ReadInt32();	// 卖出成交最大等待时间	
                    break;
                case 2://fund
                    break;
            }
            return true;
        }
    }

    /// <summary>
    /// 走势资金流
    /// </summary>
    public class ResTrendCaptialFlowDataPacket : RealTimeDataPacket
    {
        public int Code;
        public StockTrendCaptialFlowDataRec Data;
        public short Num;
        public int Offset;

        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');

            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];
            Data = new StockTrendCaptialFlowDataRec(Code);

            Num = br.ReadInt16();
            for (int i = 0; i < Num; i++)
            {
                int tempTime = br.ReadInt32();
                int outdate = 0;
                int outTime = 0;
                TrendCaptialFlowTimeConverter(tempTime, out outTime, out outdate);
                Data.MintData[i].Time = outTime;
                if (i == 0)
                    Offset = TimeUtilities.GetPointFromTime(Data.MintData[i].Time, Code);
                for (int j = 0; j < 4; j++)
                    Data.MintData[i].BuyNum[j] = br.ReadInt32();
                for (int j = 0; j < 4; j++)
                    Data.MintData[i].SellNum[j] = br.ReadInt32();
                for (int j = 0; j < 4; j++)
                    Data.MintData[i].BuyVolume[j] = br.ReadInt32();
                for (int j = 0; j < 4; j++)
                    Data.MintData[i].SellVolume[j] = br.ReadInt32();
                for (int j = 0; j < 4; j++)
                    Data.MintData[i].BuyAmount[j] = br.ReadInt32();
                for (int j = 0; j < 4; j++)
                    Data.MintData[i].SellAmount[j] = br.ReadInt32();
            }
            return true;
        }
    }

    #endregion

    #region 历史

    /// <summary>
    /// 历史分时走势
    /// </summary>
    public class ResHisTrendDataPacket : RealTimeDataPacket
    {
        OneDayTrendDataRec _trendData;
        /// <summary>
        /// 走势数据
        /// </summary>
        public OneDayTrendDataRec TrendData
        {
            get { return _trendData; }
            private set { this._trendData = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            TrendData = new OneDayTrendDataRec(DetailData.EmCodeToUnicode[emCode]);
            TrendData.Code = DetailData.EmCodeToUnicode[emCode];
            TrendData.Date = br.ReadInt32();


            short num = br.ReadInt16();
            TrendData.RequestLastPoint = num;

            long sumVolume = 0;
            double sumAmount = 0;

            //  double totalAmount = 0;
            for (int i = 0; i < num; i++)
            {
                TrendData.MintDatas[i].Price = br.ReadSingle();
                TrendData.MintDatas[i].Volume = br.ReadInt32();
                //   double amount = br.ReadDouble();
                TrendData.MintDatas[i].Amount = br.ReadDouble();
                TrendData.MintDatas[i].BuyVolume = br.ReadInt32();
                TrendData.MintDatas[i].SellVolume = br.ReadInt32();
                sumVolume += TrendData.MintDatas[i].Volume;
                sumAmount += TrendData.MintDatas[i].Amount;

                if (i == 0)
                    TrendData.MintDatas[i].AverPrice = TrendData.MintDatas[i].Price;
                else if (TrendData.MintDatas[i].Volume != 0)
                    TrendData.MintDatas[i].AverPrice =
                        Convert.ToSingle(sumAmount * 1.0f / sumVolume);
                //totalAmount = amount;
            }
            return true;
        }
    }

    /// <summary>
    /// 历史K线
    /// </summary>
    public class ResHisKLineDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 响应包对应的K线结构
        /// </summary>
        public OneStockKLineDataRec KLineDataRec;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null)
                return false;
            if (emCode.EndsWith("CFE"))
            {
                if (emCode.StartsWith("04"))
                {
                    if (emCode == "040120.CFE")
                        emCode = "IF00C1.CFE";
                    else if (emCode == "040121.CFE")
                        emCode = "IF00C2.CFE";
                    else if (emCode == "040122.CFE")
                        emCode = "IF00C3.CFE";
                    else if (emCode == "040123.CFE")
                        emCode = "IF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr = string.Empty;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "IF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }
                else if (emCode.StartsWith("05"))
                {
                    if (emCode == "050120.CFE")
                        emCode = "TF00C1.CFE";
                    else if (emCode == "050121.CFE")
                        emCode = "TF00C2.CFE";
                    else if (emCode == "050122.CFE")
                        emCode = "TF00C3.CFE";
                    else if (emCode == "050123.CFE")
                        emCode = "TF00C4.CFE";
                    else
                    {
                        string month = emCode.Substring(emCode.Length - 6, 2);
                        string headStr = string.Empty;
                        if (DateTime.Now.Month <= Convert.ToInt32(month))
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100);
                        else
                        {
                            headStr = "TF" + Convert.ToString(DateTime.Now.Year % 100 + 1);
                        }
                        emCode = (headStr + month) + ".CFE";
                    }
                }

            }

            if (!DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;


            KLineCycle cycle = (KLineCycle)br.ReadByte();
            int lastSum = 0;
            double lastSumAmount = 0;

            int num = br.ReadInt32();
            KLineDataRec = new OneStockKLineDataRec(num);
            KLineDataRec.Code = DetailData.EmCodeToUnicode[emCode];
            KLineDataRec.Cycle = cycle;
            int oneDayNum = TimeUtilities.GetMintKLinePointOneDay(DetailData.EmCodeToUnicode[emCode], cycle);

            for (int i = 0; i < num; i++)
            {
                OneDayDataRec oneDayData = new OneDayDataRec();
                oneDayData.Date = br.ReadInt32();
                oneDayData.Open = br.ReadSingle();
                oneDayData.Close = br.ReadSingle();
                oneDayData.High = br.ReadSingle();
                oneDayData.Low = br.ReadSingle();
                int tmpVolume = br.ReadInt32();
                double tmpAmount = br.ReadDouble();
                if (i % oneDayNum == 0)
                {
                    lastSum = 0;
                    lastSumAmount = 0;
                }
                switch (cycle)
                {
                    case KLineCycle.CycleMint1:
                    case KLineCycle.CycleMint5:
                    case KLineCycle.CycleMint15:
                    case KLineCycle.CycleMint30:
                    case KLineCycle.CycleMint60:
                    case KLineCycle.CycleMint120:
                        oneDayData.Volume = tmpVolume - lastSum;
                        lastSum = tmpVolume;
                        oneDayData.Amount = tmpAmount - lastSumAmount;
                        lastSumAmount = tmpAmount;
                        break;
                    default:
                        oneDayData.Volume = tmpVolume;
                        oneDayData.Amount = tmpAmount;
                        break;
                }
                oneDayData.Time = TimeUtilities.GetMintKLineTimeFromPoint(DetailData.EmCodeToUnicode[emCode],
                                                                            i % oneDayNum, cycle);
                KLineDataRec.OneDayDataList.Add(oneDayData);
            }
            DataCenter.MainUI.HistoryDatasCallBack(KLineDataRec);
            return true;
        }
    }

    #endregion

    #region 资讯

    /// <summary>
    /// 心跳包
    /// </summary>
    public class ResInfoHeart : InfoDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ResInfoHeart()
        {
            RequestType = FuncTypeInfo.InfoHeart;
        }
    }



    /// <summary>
    /// 24小时新闻
    /// </summary>
    public class ResNews24DataPacket : InfoDataPacket
    {
        /// <summary>
        /// 24小时新闻数据
        /// </summary>
        public List<OneNews24HDataRec> News24HData;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int id = br.ReadInt32();
            int num = br.ReadInt32();
            News24HData = new List<OneNews24HDataRec>(num);
            for (int i = 0; i < num; i++)
            {
                OneNews24HDataRec oneNews24HData = new OneNews24HDataRec();
                oneNews24HData.NewsID = br.ReadInt32();
                byte[] bytesTitle = br.ReadBytes(60);
                string strTitle = Encoding.Default.GetString(bytesTitle);
                int tmpIndex = strTitle.IndexOf('\0');
                strTitle = strTitle.Substring(0, tmpIndex);
                oneNews24HData.Title = strTitle;
                oneNews24HData.UpdateDate = br.ReadInt32();
                oneNews24HData.UpdateTime = br.ReadInt32();
                oneNews24HData.PublishDate = br.ReadInt32();
                oneNews24HData.PublishTime = br.ReadInt32();
                oneNews24HData.IsValid = br.ReadBoolean();
                oneNews24HData.HasShown = false;
                oneNews24HData.Url = string.Format(
                    "http://mineapi.eastmoney.com/WebFiles/0/MainPageNews/{0}/{1}.shtml", oneNews24HData.NewsID / 5000,
                    oneNews24HData.NewsID);
                if (oneNews24HData.IsValid)
                    News24HData.Add(oneNews24HData);
            }
            //QuoteSortService.SortNews(ref News24HData);
            return true;
        }
    }

    /// <summary>
    /// 新闻研报
    /// </summary>
    public class ResNewsReportDataPacket : InfoDataPacket
    {
        /// <summary>
        /// 新闻研报数据
        /// </summary>
        public InfoMineDataRec InfoMineData;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');
            byte market = br.ReadByte();

            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;


            InfoMineData = new InfoMineDataRec();
            InfoMineData.Code = DetailData.EmCodeToUnicode[emCode];

            ushort showType = br.ReadUInt16(); //显示方式
            int dateStart = br.ReadInt32();
            int timeStart = br.ReadInt32();
            int dateEnd = br.ReadInt32();
            int timeEnd = br.ReadInt32();
            int count = br.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                OneInfoMineDataRec oneInfo = new OneInfoMineDataRec();

                oneInfo.TextId = br.ReadInt64();
                long tmp = oneInfo.TextId / 5000;

                byte[] title = br.ReadBytes(61);
                string titleStr = Encoding.Default.GetString(title);
                int index = titleStr.IndexOf('\0');
                if (index >= 0)
                    titleStr = titleStr.Substring(0, index);
                oneInfo.Title = titleStr;

                oneInfo.PublishDate = br.ReadInt32();
                oneInfo.PublishTime = br.ReadInt32();
                oneInfo.InfoType = (InfoMine)br.ReadUInt16(); //类型

                string url = string.Empty;
                switch (oneInfo.InfoType)
                {
                    case InfoMine.News:
                    case InfoMine.IndexNews:
                        url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/News/{0}/{1}.shtml", tmp,
                                            oneInfo.TextId);
                        break;
                    case InfoMine.Report:
                        url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/Report/{0}/{1}.shtml", tmp,
                                            oneInfo.TextId);
                        break;
                    case InfoMine.Notice:
                        url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/Bulletin/{0}/{1}.shtml", tmp,
                                            oneInfo.TextId);
                        break;
                }

                oneInfo.ContentUrl = url;
                bool isValid = br.ReadBoolean(); //是否有效，0表示无效，不显示到前台
                if (isValid)
                {
                    if (InfoMineData.InfoMineData.ContainsKey(oneInfo.InfoType))
                    {
                        InfoMineData.InfoMineData[oneInfo.InfoType].Add(oneInfo);
                    }
                    else
                    {
                        List<OneInfoMineDataRec> list = new List<OneInfoMineDataRec>();
                        list.Add(oneInfo);
                        InfoMineData.InfoMineData.Add(oneInfo.InfoType, list);
                    }
                }
            }
            return true;

        }
    }

    /// <summary>
    /// 自选股资讯
    /// </summary>
    public class ResCustomStockNewsDataPacket : InfoDataPacket
    {
        public List<InfoMineDataRec> InfoMineData;

        protected override bool DecodingBody(BinaryReader br)
        {
            int msgId = br.ReadInt32();
            ushort type = br.ReadUInt16();
            int count = br.ReadInt32();
            InfoMineData = new List<InfoMineDataRec>(count);
            for (int i = 0; i < count; i++)
            {
                InfoMineDataRec memData = new InfoMineDataRec();

                OneInfoMineDataRec oneInfoMine = new OneInfoMineDataRec();
                oneInfoMine.TextId = br.ReadInt64();
                long tmp = oneInfoMine.TextId / 5000;
                byte[] title = br.ReadBytes(61);
                string titleStr = Encoding.Default.GetString(title);

                int index = titleStr.IndexOf('\0');
                if (index >= 0)
                    titleStr = titleStr.Substring(0, index);
                oneInfoMine.Title = titleStr;
                oneInfoMine.PublishDate = br.ReadInt32();
                oneInfoMine.PublishTime = br.ReadInt32();

                string emCode = ConvertCode.ConvertIntToCode(br.ReadUInt32());

                oneInfoMine.InfoType = (InfoMine)br.ReadUInt16(); //类型
                int dateBegin = br.ReadInt32();
                int timeBegin = br.ReadInt32();
                int dateEnd = br.ReadInt32();
                int timeEnd = br.ReadInt32();
                bool isvalide = br.ReadBoolean();
                int dateUpdate = br.ReadInt32();
                int timeUpdate = br.ReadInt32();

                string url = string.Empty;
                switch (oneInfoMine.InfoType)
                {
                    case InfoMine.News:
                    case InfoMine.IndexNews:
                        url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/News/{0}/{1}.shtml", tmp,
                                            oneInfoMine.TextId);
                        break;
                    case InfoMine.Report:
                        url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/Report/{0}/{1}.shtml", tmp,
                                            oneInfoMine.TextId);
                        break;
                    case InfoMine.Notice:
                        url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/Bulletin/{0}/{1}.shtml", tmp,
                                            oneInfoMine.TextId);
                        break;
                }
                oneInfoMine.ContentUrl = url;
                if (!isvalide && emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                int code = DetailData.EmCodeToUnicode[emCode];

                memData.Code = code;
                List<OneInfoMineDataRec> temp = new List<OneInfoMineDataRec>();
                temp.Add(oneInfoMine);
                memData.InfoMineData.Add(oneInfoMine.InfoType, temp);

                InfoMineData.Add(memData);
            }
            return true;
        }
    }

    /// <summary>
    /// 财务数据
    /// </summary>
    public class ResFinanceDataPacket : InfoDataPacket
    {
        ///// <summary>
        ///// 财务数据数据
        ///// </summary>
        //public Dictionary<int,Dictionary<FieldIndex, object>> FinanceData { get; private set; }

        /// <summary>
        /// 财务数据数据
        /// </summary>
        // public Dictionary<int, OneFinanceDataRec> FinanceData { get; private set; } 
        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            //    int count = br.ReadInt32();
            //    DetailData.AllFinanceData = new Dictionary<int, OneFinanceDataRec>(count);

            //    for(int i=0;i<count;i++)
            //    {
            //        OneFinanceDataRec oneStockFinance = new OneFinanceDataRec();
            //        int date = br.ReadInt32();
            //        int time = br.ReadInt32();
            //        uint stockid = br.ReadUInt32();
            //        string emCode = ConvertCode.ConvertIntToCode(stockid);
            //        oneStockFinance.ZGB = br.ReadDouble();
            //        oneStockFinance.AvgNetS= br.ReadDouble();
            //        oneStockFinance.MGSY= br.ReadDouble();
            //        oneStockFinance.MGSY2= br.ReadDouble();
            //        oneStockFinance.MGJZC= br.ReadDouble();
            //        oneStockFinance.MGJZC2= br.ReadDouble();
            //        oneStockFinance.JZC= br.ReadDouble();
            //        oneStockFinance.ZYYWSR= br.ReadDouble();
            //        oneStockFinance.IncomeRatio= br.ReadDouble();
            //        oneStockFinance.ProfitFO= br.ReadDouble();
            //        oneStockFinance.InvIncome= br.ReadDouble();
            //        oneStockFinance.PBTax= br.ReadDouble();
            //        oneStockFinance.ZYYWLR= br.ReadDouble();
            //        oneStockFinance.NPRatio= br.ReadDouble();
            //        oneStockFinance.RProfotAA= br.ReadDouble();
            //        oneStockFinance.DRPRPAA= br.ReadDouble();
            //        oneStockFinance.Gprofit= br.ReadDouble();
            //        oneStockFinance.ZZC= br.ReadDouble();
            //        oneStockFinance.CurAsset= br.ReadDouble();
            //        oneStockFinance.FixAsset= br.ReadDouble();
            //        oneStockFinance.IntAsset= br.ReadDouble();
            //        oneStockFinance.TotalLiab= br.ReadDouble();
            //        oneStockFinance.TCurLiab= br.ReadDouble();
            //        oneStockFinance.TLongLiab= br.ReadDouble();
            //        oneStockFinance.ZCFZL= br.ReadDouble();
            //        oneStockFinance.OWnerEqu=br.ReadDouble();
            //        oneStockFinance.OEquRatio= br.ReadDouble();
            //        oneStockFinance.CapRes= br.ReadDouble();
            //        oneStockFinance.DRPCapRes= br.ReadDouble();
            //        oneStockFinance.NetAShare= br.ReadDouble();
            //        oneStockFinance.NetBShare= br.ReadDouble();
            //        oneStockFinance.Hshare= br.ReadDouble();
            //        oneStockFinance.ListDate=br.ReadInt32();
            //        oneStockFinance.BGQDate=br.ReadInt32();

            //        if (emCode != null && DetailData.EmCodeToUnicode.ContainsKey(emCode))
            //            DetailData.AllFinanceData[DetailData.EmCodeToUnicode[emCode]]=oneStockFinance;
            //    }
            return true;
        }
    }

    /// <summary>
    /// 机构评级
    /// </summary>
    public class ResOrgRateDataPacket : InfoDataPacket
    {
        OrgRateDataRec _rateData;
        /// <summary>
        /// 机构评级数据
        /// </summary>
        public OrgRateDataRec RateData
        {
            get { return _rateData; }
            private set { this._rateData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResOrgRateDataPacket()
        {
            RateData = new OrgRateDataRec();
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                OneOrgRateItem item = new OneOrgRateItem();
                item.Id = br.ReadInt64();
                item.Url = string.Format(@"http://mineapi.eastmoney.com/WebFiles/0/Report/{0}/{1}.shtml", item.Id / 5000,
                                         item.Id);

                item.WrittenDate = br.ReadInt32();

                uint stockid = br.ReadUInt32();
                string emCode = ConvertCode.ConvertIntToCode(stockid);
                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                byte[] rankBytes = br.ReadBytes(9);
                string rankStr = Encoding.Default.GetString(rankBytes);
                int rankIndex = rankStr.IndexOf('\0');
                if (rankIndex > 0)
                    rankStr = rankStr.Substring(0, rankIndex);
                item.Rate = rankStr;

                byte[] orgNameBytes = br.ReadBytes(13);
                string orgNameStr = Encoding.Default.GetString(orgNameBytes);
                int orgNameIndex = orgNameStr.IndexOf('\0');
                if (orgNameIndex > 0)
                    orgNameStr = orgNameStr.Substring(0, orgNameIndex);
                item.OrgName = orgNameStr;

                byte[] titleBytes = br.ReadBytes(61);
                string titleStr = Encoding.Default.GetString(titleBytes);
                int titleIndex = titleStr.IndexOf('\0');
                if (titleIndex > 0)
                    titleStr = titleStr.Substring(0, titleIndex);
                item.Title = titleStr;

                item.Influence = br.ReadByte();
                item.ProfitPerShare1 = br.ReadDouble();
                item.Forecast1 = br.ReadInt16();
                item.RealValue1 = br.ReadByte();
                item.ProfitPerShare2 = br.ReadDouble();
                item.Forecast2 = br.ReadInt16();
                item.RealValue2 = br.ReadByte();
                item.ProfitPerShare3 = br.ReadDouble();
                item.Forecast3 = br.ReadInt16();
                item.RealValue3 = br.ReadByte();
                item.ProfitPerShare4 = br.ReadDouble();
                item.Forecast4 = br.ReadInt16();
                item.RealValue4 = br.ReadByte();
                item.ProfitPerShare5 = br.ReadDouble();
                item.Forecast5 = br.ReadInt16();
                item.RealValue5 = br.ReadByte();

                int date = br.ReadInt32();
                int time = br.ReadInt32();
                bool isvalid = br.ReadBoolean();
                if (isvalid)
                {
                    RateData.Code = DetailData.EmCodeToUnicode[emCode];
                    RateData.OrgRateDataList.Add(item);
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 盈利预测
    /// </summary>
    public class ResProfitForecastDataPacket : InfoDataPacket
    {
        private const int OneProfitForecastLength = 55;

        /// <summary>
        /// 盈利预测数据
        /// </summary>
        public Dictionary<int, OneProfitForecastDataRec> AllProfitForecast;

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int size = br.ReadInt32();
            int count = size / OneProfitForecastLength;
            AllProfitForecast = new Dictionary<int, OneProfitForecastDataRec>(count);

            for (int i = 0; i < count; i++)
            {
                OneProfitForecastDataRec item = new OneProfitForecastDataRec();
                br.ReadBytes(12);
                item.Date = br.ReadInt32();
                item.ProfitPerShare1 = br.ReadSingle();
                item.Forecast1 = br.ReadInt16();
                item.RealValue1 = br.ReadByte();
                item.ProfitPerShare2 = br.ReadSingle();
                item.Forecast2 = br.ReadInt16();
                item.RealValue2 = br.ReadByte();
                item.ProfitPerShare3 = br.ReadSingle();
                item.Forecast3 = br.ReadInt16();
                item.RealValue3 = br.ReadByte();
                item.ProfitPerShare4 = br.ReadSingle();
                item.Forecast4 = br.ReadInt16();
                item.RealValue4 = br.ReadByte();
                item.ProfitPerShare5 = br.ReadSingle();
                item.Forecast5 = br.ReadInt16();
                item.RealValue5 = br.ReadByte();

                string emCode = ConvertCode.ConvertIntToCode(br.ReadUInt32());
                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                item.Code = DetailData.EmCodeToUnicode[emCode];
                AllProfitForecast.Add(item.Code, item);
            }
            return true;
        }
    }

    /// <summary>
    /// 除权除息
    /// </summary>
    public class ResDivideRightDataPacket : InfoDataPacket
    {
        Dictionary<int, List<DivideRightDataRec>> _packetData;
        /// <summary>
        /// 除权除息数据
        /// </summary>
        public Dictionary<int, List<DivideRightDataRec>> PacketData
        {
            get { return _packetData; }
            private set { this._packetData = value; }
        }

        private int _updateDate;
        /// <summary>
        /// 更新日期
        /// </summary>
        public int UpdateDate
        {
            get { return _updateDate; }
            private set { this._updateDate = value; }
        }

        private int _updateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public int UpdateTime
        {
            get { return _updateTime; }
            private set { this._updateTime = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int num = br.ReadInt32();
            PacketData = new Dictionary<int, List<DivideRightDataRec>>(num);
            for (int i = 0; i < num; i++)
            {
                long recordId = br.ReadInt64();
                bool isValid = br.ReadBoolean();
                DivideRightDataRec item = new DivideRightDataRec();
                item.PunishDate = br.ReadInt32();
                string emCode = ConvertCode.ConvertIntToCode(br.ReadUInt32());
                if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                    return false;
                int type = br.ReadInt32();
                item.PGBL = br.ReadSingle();
                item.PGJG = br.ReadSingle();
                item.ZFBL = br.ReadSingle();
                float tmp = br.ReadSingle();
                item.ZFJG = br.ReadSingle();
                item.PXBL = br.ReadSingle();
                item.SGBL = br.ReadSingle();
                UpdateDate = br.ReadInt32();
                UpdateTime = br.ReadInt32();
                if (DetailData.EmCodeToUnicode[emCode] != 0 && isValid)
                {
                    if (PacketData.ContainsKey(DetailData.EmCodeToUnicode[emCode]))
                    {
                        PacketData[DetailData.EmCodeToUnicode[emCode]].Add(item);
                    }
                    else
                    {
                        List<DivideRightDataRec> list = new List<DivideRightDataRec>();
                        list.Add(item);
                        PacketData.Add(DetailData.EmCodeToUnicode[emCode], list);
                    }

                }

            }
            return true;
        }
    }
    #endregion

    #region 机构版

    /// <summary>
    /// 报价静态初始化包
    /// </summary>
    public class ResReportInitDataPacket : OrgDataPacket
    {
        protected override bool DecodingBody(BinaryReader br)
        {
            int num = br.ReadInt32();

            for (int i = 0; i < num; i++)
            {

                long codeId = br.ReadInt64();
                int unicode = 0;
                string code = string.Empty;
                if (codeId > 10000000000)
                    code = ConvertCodeOrg.ConvertLongToCode(codeId); //等会用ConvertCodeOrd替换
                else
                    unicode = Convert.ToInt32(codeId);

                if ((!string.IsNullOrEmpty(code)) && DetailData.EmCodeToUnicode.ContainsKey(code))
                    unicode = DetailData.EmCodeToUnicode[code];

                Dictionary<FieldIndex, int> fieldInt32;
                Dictionary<FieldIndex, float> fieldSingle;
                Dictionary<FieldIndex, long> fieldInt64;
                Dictionary<FieldIndex, double> fieldDouble;

                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }
                if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
                {
                    fieldInt64 = new Dictionary<FieldIndex, long>(1);
                    DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
                }
                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }
                if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
                {
                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                    DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
                }

                fieldSingle[FieldIndex.HighW52] = br.ReadSingle();
                fieldSingle[FieldIndex.LowW52] = br.ReadSingle();
                fieldSingle[FieldIndex.PreClose] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay3] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay5] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay10] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay20] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay60] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay120] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDay250] = br.ReadSingle();
                fieldSingle[FieldIndex.PreCloseDayYTD] = br.ReadSingle();
                fieldInt64[FieldIndex.VolumeAvgDay5] = Convert.ToInt64(br.ReadSingle());
                fieldDouble[FieldIndex.AmountDay4] = br.ReadDouble();
                fieldDouble[FieldIndex.AmountDay19] = br.ReadDouble();
                fieldDouble[FieldIndex.AmountDay59] = br.ReadDouble();
                fieldDouble[FieldIndex.NetFlowDay4] = br.ReadDouble();
                fieldDouble[FieldIndex.NetFlowDay19] = br.ReadDouble();
                fieldDouble[FieldIndex.NetFlowDay59] = br.ReadDouble();
                fieldInt32[FieldIndex.NetInFlowRedDay4] = br.ReadInt32();
                fieldInt32[FieldIndex.NetInFlowRedDay19] = br.ReadInt32();
                fieldInt32[FieldIndex.NetInFlowRedDay59] = br.ReadInt32();
                fieldSingle[FieldIndex.InvestGrade] = br.ReadSingle();
                fieldInt32[FieldIndex.InvestGradeNum] = br.ReadInt32();
            }
            LogUtilities.LogMessage("收到静态数据初始化包");
            return true;
        }
    }

    /// <summary>
    /// 机构版板块指数报价列表
    /// </summary>
    public class ResBlockReportDataPacket : ResReportDataPacketBase
    {


    }

    /// <summary>
    /// 机构版报价返回基类
    /// </summary>
    public class ResReportDataPacketBase : OrgDataPacket
    {
        private Dictionary<int, List<FieldIndex>> _changedFields;
        private byte _packetStatus;
        public List<int> CodeList;
        public int ResponseStockCount;

        protected virtual void BuildChangedFieldIndex(int unicode, List<FieldIndex> fieldIndex)
        {
            if (!this.ChangedFields.ContainsKey(unicode))
            {
                this.ChangedFields.Add(unicode, fieldIndex);
            }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            DateTime now = DateTime.Now;
            br.ReadByte();
            br.ReadInt32();
            this.PacketStatus = br.ReadByte();
            int capacity = br.ReadInt32();
            this.ResponseStockCount = capacity;
            this.ChangedFields = new Dictionary<int, List<FieldIndex>>(capacity);
            this.CodeList = new List<int>(capacity);
            byte num2 = br.ReadByte();
            List<ReqFieldIndexOrg> list = new List<ReqFieldIndexOrg>(num2);
            for (int i = 0; i < num2; i++)
            {
                list.Add((ReqFieldIndexOrg)br.ReadInt16());
            }
            for (int j = 0; j < capacity; j++)
            {
                try
                {
                    List<ReqFieldIndexOrg> stockFieldIndex = new List<ReqFieldIndexOrg>();
                    long num5 = br.ReadInt64();
                    for (int k = 0; k < list.Count; k++)
                    {
                        long num7 = ((long)1L) << k;
                        if ((num7 & num5) != 0L)
                        {
                            stockFieldIndex.Insert(0, list[(list.Count - 1) - k]);
                        }
                    }
                    this.ReadFieldIndex(stockFieldIndex, br, (int)base.RequestType);
                }
                catch (Exception exception)
                {
                    LogUtilities.LogMessage("report push error : " + exception.Message);
                }
            }
            list.Clear();
            list = null;
            CFTService.CallBack(FuncTypeRealTime.CustomReport, JsonConvert.SerializeObject(DetailData.FieldIndexDataSingle));
            return true;
        }

        private void ReadFieldIndex(List<ReqFieldIndexOrg> stockFieldIndex, BinaryReader br, int requestType)
        {
            try
            {
                List<FieldIndex> fieldIndex = new List<FieldIndex>();
                int code = ConvertCodeOrg.CommonConvertLongToUnicode(br.ReadInt64());
                foreach (ReqFieldIndexOrg org in stockFieldIndex)
                {
                    try
                    {
                        DicFieldIndexItem item = ConvertOrgFieldIndex.DicFieldIndex[org];
                        switch (item.FieldTypeServer)
                        {
                            case ReqFieldIndexType.FT_INT16:
                                {
                                    short fieldValue = br.ReadInt16();
                                    DllImportHelper.SetFieldData<short>(code, item.FieldClient, fieldValue);
                                    break;
                                }
                            case ReqFieldIndexType.FT_INT32:
                                {
                                    int num7 = br.ReadInt32();
                                    DllImportHelper.SetFieldData<int>(code, item.FieldClient, num7);
                                    break;
                                }
                            case ReqFieldIndexType.FT_FLOAT:
                                {
                                    float num4 = br.ReadSingle();
                                    DllImportHelper.SetFieldData<float>(code, item.FieldClient, num4);
                                    break;
                                }
                            case ReqFieldIndexType.FT_FLOATFINANCE:
                                {
                                    double num10 = Convert.ToDouble(br.ReadSingle());
                                    DllImportHelper.SetFieldData<double>(code, item.FieldClient, num10);
                                    break;
                                }
                            case ReqFieldIndexType.FT_LONGFINANCE:
                                {
                                    double num11 = Convert.ToDouble(br.ReadInt64());
                                    DllImportHelper.SetFieldData<double>(code, item.FieldClient, num11);
                                    break;
                                }
                            case ReqFieldIndexType.FT_LONG:
                                {
                                    long num9 = br.ReadInt64();
                                    DllImportHelper.SetFieldData<long>(code, item.FieldClient, num9);
                                    break;
                                }
                            case ReqFieldIndexType.FT_DOUBLE:
                                {
                                    double num5 = br.ReadDouble();
                                    DllImportHelper.SetFieldData<double>(code, item.FieldClient, num5);
                                    break;
                                }
                            case ReqFieldIndexType.FT_DOUBLETOFLOAT:
                                {
                                    float num17 = Convert.ToSingle(br.ReadDouble());
                                    DllImportHelper.SetFieldData<float>(code, item.FieldClient, num17);
                                    break;
                                }
                            case ReqFieldIndexType.FT_TOPCODE:
                                {
                                    string str2 = ConvertCodeOrg.ConvertLongToCode(br.ReadInt64());
                                    DllImportHelper.SetFieldData<string>(code, item.FieldClient, str2);
                                    break;
                                }
                            case ReqFieldIndexType.FT_STRINGORG:
                                {
                                    byte count = br.ReadByte();
                                    string str = Encoding.Default.GetString(br.ReadBytes(count)).Trim();
                                    DllImportHelper.SetFieldData<string>(code, item.FieldClient, str);
                                    break;
                                }
                            case ReqFieldIndexType.FT_INTTOLONG:
                                {
                                    long num12 = Convert.ToInt64(br.ReadInt32());
                                    DllImportHelper.SetFieldData<long>(code, item.FieldClient, num12);
                                    break;
                                }
                            case ReqFieldIndexType.FT_FLOATTOINT32:
                                {
                                    int num13 = Convert.ToInt32(br.ReadSingle());
                                    DllImportHelper.SetFieldData<int>(code, item.FieldClient, num13);
                                    break;
                                }
                            case ReqFieldIndexType.FT_FLOATRoundPrice:
                                {
                                    float num15 = ConvertOrgFieldIndex.ConvertPriceRound(br.ReadSingle(), code);
                                    DllImportHelper.SetFieldData<float>(code, item.FieldClient, num15);
                                    break;
                                }
                        }
                        if (item.FieldClient != FieldIndex.Na)
                        {
                            fieldIndex.Add(item.FieldClient);
                        }
                        continue;
                    }
                    catch (Exception exception)
                    {
                        LogUtilities.LogMessage("ReadFieldIndex Error: " + exception.Message);
                        continue;
                    }
                }
                this.CodeList.Add(code);
                this.BuildChangedFieldIndex(code, fieldIndex);
                //DllImportHelper.CalculateFieldData(code, requestType);
            }
            catch (Exception exception2)
            {
                LogUtilities.LogMessage("ReadFieldIndex出错 ：" + exception2.Message);
            }
        }

        public Dictionary<int, List<FieldIndex>> ChangedFields
        {
            get
            {
                return this._changedFields;
            }
            protected set
            {
                this._changedFields = value;
            }
        }

        public byte PacketStatus
        {
            get
            {
                return this._packetStatus;
            }
            protected set
            {
                this._packetStatus = value;
            }
        }
    }

    /// <summary>
    /// 国债综合屏-"公开市场操作"模块回应包体
    /// </summary>
    public class ResBondDashboardPublicMarketOpeartion : OrgDataPacket
    {
        /// <summary>
        /// 债券综合屏-公开市场操作模块的静态数据列表
        /// </summary>
        public List<BondPublicOpeartionItem> BondPublicOpeartionList;

        byte _packetStatus;
        /// <summary>
        /// 响应包的标识，1表示开始包，0表示中间包，2表示结尾包，其他表示错误包
        /// </summary>
        public byte PacketStatus
        {
            get { return _packetStatus; }
            protected set { this._packetStatus = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            DateTime dt = DateTime.Now;
            Debug.Print("Res :" + DateTime.Now.ToLongTimeString() + "mi" + DateTime.Now.Millisecond.ToString());

            byte bResponse = br.ReadByte();
            int id = br.ReadInt32();
            PacketStatus = br.ReadByte(); //0中间，1开始，2末尾，3未知

            int nStock = br.ReadInt32(); //股票数      

            BondPublicOpeartionList = new List<BondPublicOpeartionItem>(nStock);

            byte nField = br.ReadByte(); //栏位个数
            List<ReqFieldIndexOrg> fieldIndexList = new List<ReqFieldIndexOrg>(nField);
            for (int i = 0; i < nField; i++)
            {
                fieldIndexList.Add((ReqFieldIndexOrg)br.ReadInt16());
            }

            BondPublicOpeartionItem item;
            for (int i = 0; i < nStock; i++)
            {
                long biaoshi = br.ReadInt64();
                try
                {
                    item = GetPerItem(br);
                    BondPublicOpeartionList.Add(item);
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("ReadFieldIndex error :" + e.Message);
                }
            }

            Debug.Print("DecodingBody :" + (DateTime.Now - dt).TotalMilliseconds.ToString());
            fieldIndexList.Clear();
            fieldIndexList = null;
            return true;
        }

        private BondPublicOpeartionItem GetPerItem(BinaryReader br)
        {
            BondPublicOpeartionItem result = new BondPublicOpeartionItem();

            long codeId = br.ReadInt64();// nouse 
            int bondDate = br.ReadInt32();
            result.BondDate = GetFormatDate(bondDate);

            result.CouponRate = br.ReadSingle().ToString();
            result.IssueVol = br.ReadSingle().ToString();

            byte count = br.ReadByte();
            result.Period = Encoding.Default.GetString(br.ReadBytes(count));

            count = br.ReadByte();
            result.OType = Encoding.Default.GetString(br.ReadBytes(count));

            return result;
        }
        private string GetFormatDate(int date)
        {
            string result = string.Empty;

            int day = (int)(date % 100);
            int month = (int)((date - day) % 10000) / 100;
            int year = (date - day - month * 100) / 10000;
            result = string.Format("{0}-{1}-{2}", year.ToString("D4"), month.ToString("D2"), day.ToString("D2"));

            return result;
        }

    }

    /// <summary>
    /// 沪深
    /// </summary>
    public class ResBlockStockReportOrgDataPacket : ResBlockReportDataPacket
    {

    }
    /// <summary>
    /// 机构版港股报价
    /// </summary>
    public class ResHKStockReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 机构版基金报价
    /// </summary>
    public class ResFundStockReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 机构版债券报价
    /// </summary>
    public class ResBondStockReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 机构版期货报价
    /// </summary>
    public class ResFuturesStockReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 机构版股指期货报价
    /// </summary>
    public class ResIndexFutruesReportOrgDataPacket : ResBlockReportDataPacket
    {

    }

    /// <summary>
    /// LME报价
    /// </summary>
    public class ResOSFuturesLMEReportDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 利率
    /// </summary>
    public class ResRateReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 理财
    /// </summary>
    public class ResFinanceReportOrgDataPacket : ResBlockReportDataPacket
    {

    }

    /// <summary>
    /// 自选股类型报价
    /// </summary>
    public class ResCustomReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 资金流向报价
    /// </summary>
    public class ResCapitalFlowReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// DDE报价
    /// </summary>
    public class ResDDEReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 增仓排名
    /// </summary>
    public class ResNetInFlowReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 报价财务数据
    /// </summary>
    public class ResFinanceStockReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 报价盈利预测
    /// </summary>
    public class ResProfitForecastReportOrgDataPacket : ResBlockReportDataPacket
    {
    }


    /// <summary>
    /// 海外期货报价
    /// </summary>
    public class ResOSFuturesReportDataPacket : ResBlockReportDataPacket
    {

    }

    /// <summary>
    /// 美股报价
    /// </summary>
    public class ResUSStockReportDataPacket : ResBlockReportDataPacket
    {

    }

    /// <summary>
    /// 外汇报价
    /// </summary>
    public class ResForexReportDataPacket : ResBlockReportDataPacket
    {

    }


    /// <summary>
    /// 自选股盈利预测
    /// </summary>
    public class ResCustomProfitForecastReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 自选股资金流向
    /// </summary>
    public class ResCustomCapitalFlowReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 自选股DDE系列
    /// </summary>
    public class ResCustomDDEReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 自选增仓排名
    /// </summary>
    public class ResCustomNetInFlowReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 自选股财务数据
    /// </summary>
    public class ResCustomFinanceStockReportOrgDataPacket : ResBlockReportDataPacket
    {
    }

    /// <summary>
    /// 机构服务器心跳
    /// </summary>
    public class ResHeartOrgDataPacket : OrgDataPacket
    {
        private int _date;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date
        {
            get { return _date; }
            private set { this._date = value; }
        }

        private int _time;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time
        {
            get { return _time; }
            private set { this._time = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            Date = br.ReadInt32();
            Time = br.ReadInt32();
            return true;
        }
    }

    /// <summary>
    /// 初始化包
    /// </summary>
    public class ResInitOrgDataPacket : OrgDataPacket
    {
        InitOrgStatus _initStatus;
        /// <summary>
        /// 初始化状态
        /// </summary>
        public InitOrgStatus InitStatus
        {
            get { return _initStatus; }
            private set { this._initStatus = value; }
        }
        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            InitStatus = (InitOrgStatus)br.ReadInt16();
            LogUtilities.LogMessage("【******行情初始化******】收到初始化包，Status= " + InitStatus);
            Debug.Print("收到初始化包，Status= " + InitStatus);
            return true;
        }
    }

    /// <summary>
    /// 机构版历史K线响应
    /// </summary>
    public class ResHisKLineOrgDataPacket : OrgDataPacket
    {
        OneStockKLineDataRec _klineDataRec;
        /// <summary>
        /// 响应包对应的K线结构
        /// </summary>
        public OneStockKLineDataRec KLineDataRec
        {
            get { return _klineDataRec; }
            private set { this._klineDataRec = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            long sid = br.ReadInt64();
            string emCode = string.Empty;
            int unicode = 0;

            if (sid > 10000000000)
                emCode = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
            else
                unicode = Convert.ToInt32(sid);
            if ((!string.IsNullOrEmpty(emCode)) && DetailData.EmCodeToUnicode.ContainsKey(emCode))
                unicode = DetailData.EmCodeToUnicode[emCode];

            if (emCode == null || unicode == 0)
                return false;
            KLineCycleOrg cycleOrg = (KLineCycleOrg)br.ReadInt16();
            KLineCycle cycle = (KLineCycle)Enum.Parse(typeof(KLineCycle), cycleOrg.ToString());
            int num = br.ReadInt32();
            KLineDataRec = new OneStockKLineDataRec(num);
            KLineDataRec.Code = unicode;
            KLineDataRec.Cycle = cycle;

            for (int i = 0; i < num; i++)
            {
                OneDayDataRec oneDayData = new OneDayDataRec();
                oneDayData.Date = br.ReadInt32();
                oneDayData.Time = br.ReadInt32();
                oneDayData.High = br.ReadSingle();
                oneDayData.Open = br.ReadSingle();
                oneDayData.Low = br.ReadSingle();
                oneDayData.Close = br.ReadSingle();
                oneDayData.Volume = br.ReadInt64();
                oneDayData.Amount = br.ReadDouble();
                KLineDataRec.OneDayDataList.Add(oneDayData);
            }
            return true;
        }
    }

    /// <summary>
    /// 机构版当日K线
    /// </summary>
    public class ResMintKLineOrgDataPacket : OrgDataPacket
    {
        OneStockKLineDataRec _kLineDataRec;
        /// <summary>
        /// 响应包对应的K线结构
        /// </summary>
        public OneStockKLineDataRec KLineDataRec
        {
            get { return _kLineDataRec; }
            private set { this._kLineDataRec = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            long sid = br.ReadInt64();
            string emCode = string.Empty;
            int unicode = 0;

            if (sid > 10000000000)
                emCode = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
            else
                unicode = Convert.ToInt32(sid);
            if ((!string.IsNullOrEmpty(emCode)) && DetailData.EmCodeToUnicode.ContainsKey(emCode))
                unicode = DetailData.EmCodeToUnicode[emCode];

            if (emCode == null || unicode == 0)
                return false;
            KLineCycleOrg cycleOrg = (KLineCycleOrg)br.ReadInt16();
            KLineCycle cycle = (KLineCycle)Enum.Parse(typeof(KLineCycle), cycleOrg.ToString());
            int num = br.ReadInt32();
            KLineDataRec = new OneStockKLineDataRec(num);
            KLineDataRec.Code = unicode;
            KLineDataRec.Cycle = cycle;

            for (int i = 0; i < num; i++)
            {
                OneDayDataRec oneDayData = new OneDayDataRec();
                oneDayData.Date = br.ReadInt32();
                oneDayData.Time = br.ReadInt32();
                oneDayData.High = br.ReadSingle();
                oneDayData.Open = br.ReadSingle();
                oneDayData.Low = br.ReadSingle();
                oneDayData.Close = br.ReadSingle();
                oneDayData.Volume = br.ReadInt64();
                oneDayData.Amount = br.ReadDouble();
                KLineDataRec.OneDayDataList.Add(oneDayData);
            }
            return true;
        }
    }

    /// <summary>
    /// 机构版走势
    /// </summary>
    public class ResTrendOrgDataPacket : OrgDataPacket
    {
        OneDayTrendDataRec _trendData;
        /// <summary>
        /// 走势数据 
        /// </summary>
        public OneDayTrendDataRec TrendData
        {
            get { return _trendData; }
            private set { this._trendData = value; }
        }

        short _preOffset;
        /// <summary>
        /// 
        /// </summary>
        public short PreOffset
        {
            get { return _preOffset; }
            private set { this._preOffset = value; }
        }

        short _offset;
        /// <summary> 
        /// 
        /// </summary>
        public short Offset
        {
            get { return _offset; }
            private set { this._offset = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private short _preMinNum;

        public short PreMinNum
        {
            get { return _preMinNum; }
            set { _preMinNum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private short _minNum;

        public short MinNum
        {
            get { return _minNum; }
            set { _minNum = value; }
        }

        /// <summary>
        /// 第一个有效数据对应的下标
        /// </summary>
        public int FirstIndex = int.MaxValue;
        /// <summary>
        /// 
        /// </summary>
        public ResTrendOrgDataPacket()
        {
            RequestType = FuncTypeOrg.TrendOrg;
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(System.IO.BinaryReader br)
        {
            long sid = br.ReadInt64();
            string emCode = string.Empty;
            int unicode = 0;

            if (sid > 10000000000)
                emCode = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
            else
                unicode = Convert.ToInt32(sid);
            if ((!string.IsNullOrEmpty(emCode)) && DetailData.EmCodeToUnicode.ContainsKey(emCode))
                unicode = DetailData.EmCodeToUnicode[emCode];


            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(unicode))
                DetailData.FieldIndexDataInt32[unicode].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;
            MarketTime marketTime = TimeUtilities.GetMarketTime(mt);

            if (emCode == null || unicode == 0)
                return false;
            TrendData = new OneDayTrendDataRec(unicode); //该包的走势数据（不是全天的走势数据）
            TrendData.High = br.ReadSingle();
            TrendData.Open = br.ReadSingle();
            TrendData.Low = br.ReadSingle();
            TrendData.PreClose = br.ReadSingle();
            TrendData.Date = br.ReadInt32();

            //PreOffset = br.ReadInt16();
            //Offset = br.ReadInt16();//该包的第一个点，对应到该天总的走势的下标
            PreMinNum = br.ReadInt16();
            int tmpPreMinNum = Math.Min(PreMinNum, TrendData.PreMintDatas.Length);
            for (int i = 0; i < PreMinNum; i++)
            {
                int time = br.ReadInt32();
                TrendData.PreMintDatas[i].Price = br.ReadSingle();
                TrendData.PreMintDatas[i].Volume = br.ReadInt32();
                TrendData.PreMintDatas[i].Amount = br.ReadDouble();
                TrendData.PreMintDatas[i].AverPrice = br.ReadSingle();
                TrendData.PreMintDatas[i].SellVolume = br.ReadInt32();
                TrendData.PreMintDatas[i].BuyVolume = br.ReadInt32();
                int nei = br.ReadInt32();
                long wai = TrendData.PreMintDatas[i].Volume - nei;
                TrendData.PreMintDatas[i].NeiWaiCha = Convert.ToInt32(nei - wai); //内盘
            }


            MinNum = br.ReadInt16(); //改包走势点的数目
            int tmpMinNum = Math.Min(MinNum, TrendData.MintDatas.Length);
            int currentIndex = 0;
            for (int i = 0; i < tmpMinNum; i++)
            {
                int time = br.ReadInt32();

                switch (mt)
                {
                    case MarketType.US:
                    case MarketType.OSFutures:
                    case MarketType.OSFuturesCBOT:
                    case MarketType.OSFuturesSGX:
                    case MarketType.OSFuturesLMEElec:
                    case MarketType.OSFuturesLMEVenue:
                        if (time < marketTime.FirstOpenTime)
                            time += 240000;
                        break;
                }
                int currentOffset = TimeUtilities.GetPointFromTime(time, unicode);

                if (currentOffset >= 0)
                {
                    TrendData.MintDatas[currentOffset].Price = br.ReadSingle();
                    TrendData.MintDatas[currentOffset].Volume = br.ReadInt32();
                    TrendData.MintDatas[currentOffset].Amount = br.ReadDouble();
                    TrendData.MintDatas[currentOffset].AverPrice = br.ReadSingle();
                    TrendData.MintDatas[currentOffset].SellVolume = br.ReadInt32();
                    TrendData.MintDatas[currentOffset].BuyVolume = br.ReadInt32();
                    int nei = br.ReadInt32();
                    long wai = TrendData.MintDatas[currentOffset].Volume - nei;
                    TrendData.MintDatas[currentOffset].NeiWaiCha = Convert.ToInt32(nei - wai); //内盘

                    if (currentOffset < FirstIndex)
                        FirstIndex = currentOffset;
                }

                //if (i == 0)
                //    Offset = (short)currentOffset;
                //currentIndex = currentOffset - Offset;
                //if (currentIndex >= 0)
                //{
                //    TrendData.MintDatas[currentIndex].Price = br.ReadSingle();
                //    TrendData.MintDatas[currentIndex].Volume = br.ReadInt32();
                //    TrendData.MintDatas[currentIndex].Amount = br.ReadDouble();
                //    TrendData.MintDatas[currentIndex].AverPrice = br.ReadSingle();
                //    TrendData.MintDatas[currentIndex].SellVolume = br.ReadInt32();
                //    TrendData.MintDatas[currentIndex].BuyVolume = br.ReadInt32();
                //    int nei = br.ReadInt32();
                //    long wai = TrendData.MintDatas[currentIndex].Volume - nei;
                //    TrendData.MintDatas[currentIndex].NeiWaiCha = Convert.ToInt32(nei - wai); //内盘
                //}
                //else
                //{
                //    continue;
                //}

            }

            return true;
        }
    }

    /// <summary>
    /// 历史交易日
    /// </summary>
    public class ResTradeDateDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 各市场的历史交易日
        /// </summary>
        private Dictionary<MarketType, List<int>> _marketTradeDate;

        public Dictionary<MarketType, List<int>> MarketTradeDate
        {
            get { return _marketTradeDate; }
            private set { _marketTradeDate = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            MarketTradeDate = new Dictionary<MarketType, List<int>>(6);
            //沪深
            int numSH = br.ReadInt32();
            List<int> SH = new List<int>(numSH);
            for (int i = 0; i < numSH; i++)
                SH.Add(br.ReadInt32());
            //港股
            int numHK = br.ReadInt32();
            List<int> HK = new List<int>(numHK);
            for (int i = 0; i < numHK; i++)
                HK.Add(br.ReadInt32());
            //银行间
            int numIB = br.ReadInt32();
            List<int> IB = new List<int>(numIB);
            for (int i = 0; i < numIB; i++)
                IB.Add(br.ReadInt32());
            //美股
            int numUS = br.ReadInt32();
            List<int> US = new List<int>(numUS);
            for (int i = 0; i < numUS; i++)
                US.Add(br.ReadInt32());

            Debug.Print("美股交易日：" + US[0].ToString() + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //上期所黄金白银
            int numAUAG = br.ReadInt32();
            List<int> AUAG = new List<int>(numAUAG);
            for (int i = 0; i < numAUAG; i++)
                AUAG.Add(br.ReadInt32());
            //外汇
            int numForex = br.ReadInt32();
            List<int> Forex = new List<int>(numForex);
            for (int i = 0; i < numForex; i++)
                Forex.Add(br.ReadInt32());

            //纽交所期货NMX
            int numNMX = br.ReadInt32();
            List<int> NMX = new List<int>(numNMX);
            for (int i = 0; i < numNMX; i++)
                NMX.Add(br.ReadInt32());

            //芝加哥期货
            int numCBT = br.ReadInt32();
            List<int> CBT = new List<int>(numCBT);
            for (int i = 0; i < numCBT; i++)
                CBT.Add(br.ReadInt32());

            //新加坡期货
            int numSGX = br.ReadInt32();
            List<int> SGX = new List<int>(numSGX);
            for (int i = 0; i < numSGX; i++)
                SGX.Add(br.ReadInt32());

            //LMEElec
            int numLMEElec = br.ReadInt32();
            List<int> LMEElec = new List<int>(numLMEElec);
            for (int i = 0; i < numLMEElec; i++)
                LMEElec.Add(br.ReadInt32());

            //LMEVenue
            int numLMEVenue = br.ReadInt32();
            List<int> LMEVenue = new List<int>(numLMEVenue);
            for (int i = 0; i < numLMEVenue; i++)
                LMEVenue.Add(br.ReadInt32());

            MarketTradeDate[MarketType.SHALev1] = SH;
            MarketTradeDate[MarketType.HK] = HK;
            MarketTradeDate[MarketType.IB] = IB;
            MarketTradeDate[MarketType.US] = US;
            MarketTradeDate[MarketType.ForexSpot] = Forex;
            MarketTradeDate[MarketType.OSFutures] = NMX;
            MarketTradeDate[MarketType.OSFuturesCBOT] = CBT;
            MarketTradeDate[MarketType.OSFuturesSGX] = SGX;
            MarketTradeDate[MarketType.OSFuturesLMEElec] = LMEElec;
            MarketTradeDate[MarketType.OSFuturesLMEVenue] = LMEVenue;

            LogUtilities.LogMessage("收到交易日包，sh=" + SH[0]);
            return true;
        }
    }

    /// <summary>
    /// 财务数据（机构版）
    /// </summary>
    public class ResFinanceOrgDataPacket : OrgDataPacket
    {
        protected override bool DecodingBody(BinaryReader br)
        {
            int count = br.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                long sid = br.ReadInt64();
                int unicode = 0;
                string code = string.Empty;
                if (sid > 10000000000)
                    code = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
                else
                    unicode = Convert.ToInt32(sid);

                if ((!string.IsNullOrEmpty(code)) && DetailData.EmCodeToUnicode.ContainsKey(code))
                    unicode = DetailData.EmCodeToUnicode[code];

                Dictionary<FieldIndex, int> fieldInt32;
                Dictionary<FieldIndex, double> fieldDouble;

                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }

                if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
                {
                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                    DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
                }
                fieldDouble[FieldIndex.ZGB] = br.ReadInt64();
                long ltgb = br.ReadInt64();
                fieldDouble[FieldIndex.NetAShare] = br.ReadInt64();
                fieldDouble[FieldIndex.NetBShare] = br.ReadInt64();
                fieldDouble[FieldIndex.MGSY] = br.ReadSingle();
                fieldDouble[FieldIndex.MGJZC] = br.ReadSingle();
                fieldDouble[FieldIndex.JZC] = br.ReadSingle();
                fieldDouble[FieldIndex.ZYYWSR] = br.ReadSingle();
                fieldDouble[FieldIndex.ZYYWLR] = br.ReadSingle();
                fieldDouble[FieldIndex.ZZC] = br.ReadSingle();
                fieldDouble[FieldIndex.TotalLiab] = br.ReadSingle();
                fieldDouble[FieldIndex.OWnerEqu] = br.ReadSingle();
                fieldDouble[FieldIndex.CapRes] = br.ReadSingle();
                float PARENTNETPROFITYLFR = br.ReadSingle(); //净利润（最新年报）
                fieldDouble[FieldIndex.EpsQmtby] = br.ReadSingle();
                fieldDouble[FieldIndex.EpsTtm] = br.ReadSingle();
                fieldInt32[FieldIndex.BGQDate] = br.ReadInt32();
            }
            return true;
        }
    }

    /// <summary>
    /// 除复权
    /// </summary>
    public class ResDivideRightOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 除复权数据
        /// </summary>
        private Dictionary<int, List<List<OneDivideRightBase>>> _divideData;

        public Dictionary<int, List<List<OneDivideRightBase>>> DivideData
        {
            get { return _divideData; }
            private set { _divideData = value; }
        }

        public ResDivideRightOrgDataPacket()
        {
            DivideData = new Dictionary<int, List<List<OneDivideRightBase>>>();
        }

        //TextWriter textWriter = new StreamWriter("d:\\1234.txt", true);
        protected override bool DecodingBody(BinaryReader br)
        {
            int stockNum = br.ReadInt32();
            for (int i = 0; i < stockNum; i++)
            {
                long sid = br.ReadInt64();
                int unicode = 0;
                string code = string.Empty;
                if (sid > 10000000000)
                    code = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
                else
                    unicode = Convert.ToInt32(sid);

                if ((!string.IsNullOrEmpty(code)) && DetailData.EmCodeToUnicode.ContainsKey(code))
                    unicode = DetailData.EmCodeToUnicode[code];

                int divideNum = br.ReadInt32();
                List<List<OneDivideRightBase>> oneStockData = new List<List<OneDivideRightBase>>();
                for (int j = 0; j < divideNum; j++)
                {
                    int date = br.ReadInt32();
                    float factor = br.ReadSingle();
                    byte typeNum = br.ReadByte();
                    List<OneDivideRightBase> oneStockOneDayData = new List<OneDivideRightBase>();
                    for (int k = 0; k < typeNum; k++)
                    {
                        DivideRightType oneType = (DivideRightType)br.ReadByte();
                        OneDivideRightBase oneData;
                        switch (oneType)
                        {
                            case DivideRightType.GengMing:
                                oneData = new OneDivideGengMing();
                                byte oldNameCount = br.ReadByte();
                                (oneData as OneDivideGengMing).OldName = Encoding.Default.GetString(br.ReadBytes(oldNameCount));
                                (oneData as OneDivideGengMing).OldName =
                                    (oneData as OneDivideGengMing).OldName.TrimEnd('\0');

                                byte newNameCount = br.ReadByte();
                                (oneData as OneDivideGengMing).NewName = Encoding.Default.GetString(br.ReadBytes(newNameCount));
                                (oneData as OneDivideGengMing).NewName =
                                    (oneData as OneDivideGengMing).NewName.TrimEnd('\0');
                                break;
                            case DivideRightType.SongGu:
                            case DivideRightType.PaiXi:
                            case DivideRightType.ZhuanZeng:
                                oneData = new OneDivideRightBase();
                                oneData.DataValue = br.ReadSingle();
                                break;
                            case DivideRightType.ZengFa:
                                oneData = new OneDivideRightZengfa();
                                oneData.DataValue = br.ReadSingle();
                                (oneData as OneDivideRightZengfa).ZengfaPrice = br.ReadSingle();
                                break;
                            case DivideRightType.PeiGu:
                                oneData = new OneDivideRightPeigu();
                                oneData.DataValue = br.ReadSingle();
                                (oneData as OneDivideRightPeigu).PeiguPrice = br.ReadSingle();
                                break;
                            default:
                                oneData = null;
                                break;
                        }
                        if (oneData != null)
                        {
                            oneData.Date = date;
                            oneData.DivideType = oneType;
                            oneData.Factor = factor;
                            oneStockOneDayData.Add(oneData);
                        }
                    }
                    if (oneStockOneDayData.Count == 0)
                        continue;
                    if (oneStockData.Count == 0)
                        oneStockData.Add(oneStockOneDayData);
                    else
                    {
                        bool isAddEnd = false;
                        for (int s = 0; s < oneStockData.Count; s++)
                        {
                            if (oneStockData[s][0].Date >= oneStockOneDayData[0].Date)
                            {
                                oneStockData.Insert(s, oneStockOneDayData);
                                break;
                            }
                            if (s == oneStockData.Count - 1)
                                isAddEnd = true;
                        }
                        if (isAddEnd)
                            oneStockData.Add(oneStockOneDayData);
                    }

                }
                if (unicode != 0)
                    DivideData.Add(unicode, oneStockData);
            }
            //textWriter.Close();
            return true;
        }
    }

    /// <summary>
    /// 货币基金detail
    /// </summary>
    public class ResMonetaryFundDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }

            fieldInt32[FieldIndex.FundLatestDate] = br.ReadInt32();
            fieldSingle[FieldIndex.FundIncomeYear7D] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncome10K] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear1] = br.ReadSingle();
            byte count1 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear1] = Encoding.Default.GetString(br.ReadBytes(count1));
            fieldSingle[FieldIndex.FundIncomeYear2] = br.ReadSingle();
            byte count2 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear2] = Encoding.Default.GetString(br.ReadBytes(count2));
            fieldSingle[FieldIndex.FundIncomeYear3] = br.ReadSingle();
            byte count3 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear3] = Encoding.Default.GetString(br.ReadBytes(count3));
            fieldSingle[FieldIndex.FundIncomeYear4] = br.ReadSingle();
            byte count4 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear4] = Encoding.Default.GetString(br.ReadBytes(count4));
            fieldSingle[FieldIndex.FundIncomeYear5] = br.ReadSingle();
            byte count5 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear5] = Encoding.Default.GetString(br.ReadBytes(count5));
            fieldSingle[FieldIndex.FundIncomeYear6] = br.ReadSingle();
            byte count6 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear6] = Encoding.Default.GetString(br.ReadBytes(count6));
            fieldSingle[FieldIndex.FundNvgr4w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr13w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr26w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr52w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr156w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr208w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgrf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear] = br.ReadSingle();
            fieldInt32[FieldIndex.FundFounddate] = br.ReadInt32();
            fieldInt32[FieldIndex.FundEnddate] = br.ReadInt32();
            byte countFundInvestType = br.ReadByte();
            fieldString[FieldIndex.FundParaname] = Encoding.Default.GetString(br.ReadBytes(countFundInvestType));
            fieldSingle[FieldIndex.FundNav] = br.ReadSingle();
            byte countFundStrSgshzt = br.ReadByte();
            fieldString[FieldIndex.FundStrSgshzt] = Encoding.Default.GetString(br.ReadBytes(countFundStrSgshzt));
            byte countFundManager = br.ReadByte();
            fieldString[FieldIndex.FundFmanager] = Encoding.Default.GetString(br.ReadBytes(countFundManager));
            byte countFundManagername = br.ReadByte();
            fieldString[FieldIndex.FundManagername] = Encoding.Default.GetString(br.ReadBytes(countFundManagername));
            byte countFundStrTgrcom = br.ReadByte();
            fieldString[FieldIndex.FundStrTgrcom] = Encoding.Default.GetString(br.ReadBytes(countFundStrTgrcom));
            return true;
        }
    }

    /// <summary>
    /// 非货币基金detail
    /// </summary>
    public class ResNonMonetaryFundDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }

            fieldInt32[FieldIndex.FundLatestDate] = br.ReadInt32();
            fieldSingle[FieldIndex.FundPernav] = br.ReadSingle();
            fieldSingle[FieldIndex.FundDecZdf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundAccunav] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear1] = br.ReadSingle();
            byte count1 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear1] = Encoding.Default.GetString(br.ReadBytes(count1));
            fieldSingle[FieldIndex.FundIncomeYear2] = br.ReadSingle();
            byte count2 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear2] = Encoding.Default.GetString(br.ReadBytes(count2));
            fieldSingle[FieldIndex.FundIncomeYear3] = br.ReadSingle();
            byte count3 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear3] = Encoding.Default.GetString(br.ReadBytes(count3));
            fieldSingle[FieldIndex.FundIncomeYear4] = br.ReadSingle();
            byte count4 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear4] = Encoding.Default.GetString(br.ReadBytes(count4));
            fieldSingle[FieldIndex.FundIncomeYear5] = br.ReadSingle();
            byte count5 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear5] = Encoding.Default.GetString(br.ReadBytes(count5));
            fieldSingle[FieldIndex.FundIncomeYear6] = br.ReadSingle();
            byte count6 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear6] = Encoding.Default.GetString(br.ReadBytes(count6));
            fieldSingle[FieldIndex.FundNvgr4w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr13w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr26w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr52w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr156w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr208w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgrf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear] = br.ReadSingle();
            fieldInt32[FieldIndex.FundFounddate] = br.ReadInt32();
            fieldInt32[FieldIndex.FundEnddate] = br.ReadInt32();
            byte countFundInvestType = br.ReadByte();
            fieldString[FieldIndex.FundParaname] = Encoding.Default.GetString(br.ReadBytes(countFundInvestType));
            fieldSingle[FieldIndex.FundNav] = br.ReadSingle();
            byte countFundStrSgshzt = br.ReadByte();
            fieldString[FieldIndex.FundStrSgshzt] = Encoding.Default.GetString(br.ReadBytes(countFundStrSgshzt));
            byte countFundManager = br.ReadByte();
            fieldString[FieldIndex.FundFmanager] = Encoding.Default.GetString(br.ReadBytes(countFundManager));
            byte countFundManagername = br.ReadByte();
            fieldString[FieldIndex.FundManagername] = Encoding.Default.GetString(br.ReadBytes(countFundManagername));
            byte countFundStrTgrcom = br.ReadByte();
            fieldString[FieldIndex.FundStrTgrcom] = Encoding.Default.GetString(br.ReadBytes(countFundStrTgrcom));
            return true;
        }
    }

    /// <summary>
    /// 基金净值不复权
    /// </summary>
    public class ResFundKLineAfterDivideDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 响应包对应的K线结构
        /// </summary>
        private OneStockKLineDataRec _kLineDataRec;

        public OneStockKLineDataRec KLineDataRec
        {
            get { return _kLineDataRec; }
            private set { _kLineDataRec = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int code = (int)br.ReadInt64();
            KLineCycleOrg cycleOrg = (KLineCycleOrg)br.ReadInt16();
            KLineCycle cycle = (KLineCycle)Enum.Parse(typeof(KLineCycle), cycleOrg.ToString());
            int num = br.ReadInt32();
            KLineDataRec = new OneStockKLineDataRec(num);
            KLineDataRec.Code = code;
            KLineDataRec.Cycle = cycle;

            for (int i = 0; i < num; i++)
            {
                OneDayDataRec oneDayData = new OneDayDataRec();
                oneDayData.Date = br.ReadInt32();
                oneDayData.Time = br.ReadInt32();
                oneDayData.High = br.ReadSingle();
                oneDayData.Open = br.ReadSingle();
                oneDayData.Low = br.ReadSingle();
                oneDayData.Close = br.ReadSingle();
                oneDayData.Volume = br.ReadInt64();
                oneDayData.Amount = br.ReadDouble();
                KLineDataRec.OneDayDataList.Add(oneDayData);
            }
            return true;
        }
    }

    /// <summary>
    /// 信托理财和阳光私募
    /// </summary>
    public class ResTrpAndSunDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }
            fieldInt32[FieldIndex.FundLatestDate] = br.ReadInt32();
            fieldSingle[FieldIndex.FundPernav] = br.ReadSingle();
            fieldSingle[FieldIndex.FundDecZdf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundAccunav] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear1] = br.ReadSingle();
            byte count1 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear1] = Encoding.Default.GetString(br.ReadBytes(count1));
            fieldSingle[FieldIndex.FundIncomeYear2] = br.ReadSingle();
            byte count2 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear2] = Encoding.Default.GetString(br.ReadBytes(count2));
            fieldSingle[FieldIndex.FundIncomeYear3] = br.ReadSingle();
            byte count3 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear3] = Encoding.Default.GetString(br.ReadBytes(count3));
            fieldSingle[FieldIndex.FundIncomeYear4] = br.ReadSingle();
            byte count4 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear4] = Encoding.Default.GetString(br.ReadBytes(count4));
            fieldSingle[FieldIndex.FundIncomeYear5] = br.ReadSingle();
            byte count5 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear5] = Encoding.Default.GetString(br.ReadBytes(count5));
            fieldSingle[FieldIndex.FundIncomeYear6] = br.ReadSingle();
            byte count6 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear6] = Encoding.Default.GetString(br.ReadBytes(count6));
            fieldSingle[FieldIndex.FundNvgr4w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr13w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr26w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr52w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr156w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr208w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgrf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear] = br.ReadSingle();
            fieldInt32[FieldIndex.FundFounddate] = br.ReadInt32();
            fieldInt32[FieldIndex.FundEnddate] = br.ReadInt32();
            byte countFundInvestType = br.ReadByte();
            fieldString[FieldIndex.FundParaname] = Encoding.Default.GetString(br.ReadBytes(countFundInvestType));
            fieldSingle[FieldIndex.FundNav] = br.ReadSingle();
            byte countFundStrSgshzt = br.ReadByte();
            fieldString[FieldIndex.FundStrSgshzt] = Encoding.Default.GetString(br.ReadBytes(countFundStrSgshzt));
            byte countFundManager = br.ReadByte();
            fieldString[FieldIndex.FundFmanager] = Encoding.Default.GetString(br.ReadBytes(countFundManager));
            byte countFundTrusteName = br.ReadByte();
            fieldString[FieldIndex.FundTrusteName] = Encoding.Default.GetString(br.ReadBytes(countFundTrusteName));
            byte countFundManagerName = br.ReadByte();
            fieldString[FieldIndex.FundManagerName] = Encoding.Default.GetString(br.ReadBytes(countFundManagerName));
            return true;
        }
    }

    /// <summary>
    /// 券商集合（货币式）Detail
    /// </summary>
    public class ResCIPMonetaryDetailDataPacket : ResMonetaryFundDetailDataPacket
    {
    }

    /// <summary>
    /// 券商集合（非货币式）Detail
    /// </summary>
    public class ResCIPNonMonetaryDetailDataPacket : ResNonMonetaryFundDetailDataPacket
    {
    }

    /// <summary>
    /// 银行理财
    /// </summary>
    public class ResBFPDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }
            fieldInt32[FieldIndex.FundLatestDate] = br.ReadInt32();
            fieldSingle[FieldIndex.FundPernav] = br.ReadSingle();
            fieldSingle[FieldIndex.FundDecZdf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundAccunav] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear1] = br.ReadSingle();
            byte count1 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear1] = Encoding.Default.GetString(br.ReadBytes(count1));
            fieldSingle[FieldIndex.FundIncomeYear2] = br.ReadSingle();
            byte count2 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear2] = Encoding.Default.GetString(br.ReadBytes(count2));
            fieldSingle[FieldIndex.FundIncomeYear3] = br.ReadSingle();
            byte count3 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear3] = Encoding.Default.GetString(br.ReadBytes(count3));
            fieldSingle[FieldIndex.FundIncomeYear4] = br.ReadSingle();
            byte count4 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear4] = Encoding.Default.GetString(br.ReadBytes(count4));
            fieldSingle[FieldIndex.FundIncomeYear5] = br.ReadSingle();
            byte count5 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear5] = Encoding.Default.GetString(br.ReadBytes(count5));
            fieldSingle[FieldIndex.FundIncomeYear6] = br.ReadSingle();
            byte count6 = br.ReadByte();
            fieldString[FieldIndex.FundIncomeRankYear6] = Encoding.Default.GetString(br.ReadBytes(count6));
            fieldSingle[FieldIndex.FundNvgr4w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr13w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr26w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr52w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr156w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgr208w] = br.ReadSingle();
            fieldSingle[FieldIndex.FundNvgrf] = br.ReadSingle();
            fieldSingle[FieldIndex.FundIncomeYear] = br.ReadSingle();
            fieldInt32[FieldIndex.FundFounddate] = br.ReadInt32();
            fieldInt32[FieldIndex.FundEnddate] = br.ReadInt32();
            byte countFundInvestType = br.ReadByte();
            fieldString[FieldIndex.FundParaname] = Encoding.Default.GetString(br.ReadBytes(countFundInvestType));
            fieldSingle[FieldIndex.FundNav] = br.ReadSingle();
            byte countFundStrSgshzt = br.ReadByte();
            fieldString[FieldIndex.FundStrSgshzt] = Encoding.Default.GetString(br.ReadBytes(countFundStrSgshzt));
            byte countFundManager = br.ReadByte();
            fieldString[FieldIndex.FundFmanager] = Encoding.Default.GetString(br.ReadBytes(countFundManager));
            byte countFundTrusteName = br.ReadByte();
            fieldString[FieldIndex.FundManagername] = Encoding.Default.GetString(br.ReadBytes(countFundTrusteName));
            byte countFundManagerName = br.ReadByte();
            fieldString[FieldIndex.FundManagerName] = Encoding.Default.GetString(br.ReadBytes(countFundManagerName));
            return true;
        }
    }

    /// <summary>
    /// 美股Detail
    /// </summary>
    public class ResUSStockDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();

            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }

            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }


            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            bool isPush = br.ReadBoolean();
            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume1] = br.ReadInt32();
            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume1] = br.ReadInt32();

            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.AveragePrice] = br.ReadSingle();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            fieldSingle[FieldIndex.VolumeRatio] = br.ReadSingle();

            long volume = br.ReadInt64();
            fieldInt64[FieldIndex.Volume] = volume;
            fieldInt64[FieldIndex.IndexVolume] = volume;
            double amount = br.ReadDouble();
            fieldDouble[FieldIndex.Amount] = amount;
            fieldDouble[FieldIndex.IndexAmount] = amount;

            fieldInt64[FieldIndex.RedVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.GreenVolume] = br.ReadInt32();
            if (!isPush)
                fieldSingle[FieldIndex.PreClose] = br.ReadSingle();

            float preClose = 0;
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = fieldSingle[FieldIndex.PreClose];
            if (now != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
            }
            return true;

        }
    }

    /// <summary>
    /// 外汇Detail
    /// </summary>
    public class ResForexDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();

            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            bool isPush = br.ReadBoolean();
            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            if (!isPush)
                fieldSingle[FieldIndex.PreClose] = br.ReadSingle();

            float preClose = 0;
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = fieldSingle[FieldIndex.PreClose];
            if (now != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
            }
            return true;

        }
    }


    /// <summary>
    /// 海外期货
    /// </summary>
    public class ResOSFuturesDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }

            bool isPush = br.ReadBoolean();
            fieldInt32[FieldIndex.Time] = (int)br.ReadInt64();
            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            float preClose = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = preClose;
            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            fieldSingle[FieldIndex.BuyPrice2] = br.ReadSingle();
            fieldSingle[FieldIndex.BuyPrice3] = br.ReadSingle();
            fieldSingle[FieldIndex.BuyPrice4] = br.ReadSingle();
            fieldSingle[FieldIndex.BuyPrice5] = br.ReadSingle();
            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            fieldSingle[FieldIndex.SellPrice2] = br.ReadSingle();
            fieldSingle[FieldIndex.SellPrice3] = br.ReadSingle();
            fieldSingle[FieldIndex.SellPrice4] = br.ReadSingle();
            fieldSingle[FieldIndex.SellPrice5] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume1] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.BuyVolume2] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.BuyVolume3] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.BuyVolume4] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.BuyVolume5] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.SellVolume1] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.SellVolume2] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.SellVolume3] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.SellVolume4] = (int)br.ReadInt64();
            fieldInt32[FieldIndex.SellVolume5] = (int)br.ReadInt64();
            long volume = br.ReadInt64();
            fieldInt64[FieldIndex.Volume] = volume;
            fieldInt64[FieldIndex.IndexVolume] = volume;
            fieldInt64[FieldIndex.LastVolume] = br.ReadInt32();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.Delta] = br.ReadSingle();
            fieldInt64[FieldIndex.RedVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.GreenVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.OpenInterest] = br.ReadInt32();
            fieldInt64[FieldIndex.CurOI] = br.ReadInt32();
            fieldSingle[FieldIndex.SettlementPrice] = br.ReadSingle();
            if (!isPush)
            {
                fieldInt32[FieldIndex.Date] = br.ReadInt32();
                fieldSingle[FieldIndex.PreSettlementPrice] = br.ReadSingle();
            }

            float preSettlementPrice = 0;
            if (fieldSingle.ContainsKey(FieldIndex.PreSettlementPrice))
                preSettlementPrice = fieldSingle[FieldIndex.PreSettlementPrice];
            if (now != 0 && preSettlementPrice != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preSettlementPrice;
                fieldSingle[FieldIndex.DifferRange] = (now - preSettlementPrice) / preSettlementPrice;
            }
            return true;
        }
    }

    /// <summary>
    /// LME盘口
    /// </summary>
    public class ResOSFuturesLMEDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();

            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }

            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            bool isPush = br.ReadBoolean();
            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume1] = (int)br.ReadInt64();
            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume1] = (int)br.ReadInt64();
            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.SettlementPrice] = br.ReadSingle();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            float high = br.ReadSingle();
            fieldSingle[FieldIndex.High] = high;
            float low = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = low;
            long volume = br.ReadInt64();
            fieldInt64[FieldIndex.Volume] = volume;
            fieldInt64[FieldIndex.IndexVolume] = volume;
            fieldInt64[FieldIndex.RedVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.GreenVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.LastVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.OpenInterest] = br.ReadInt32();
            if (!isPush)
            {
                fieldSingle[FieldIndex.PreClose] = br.ReadSingle();
                fieldSingle[FieldIndex.PreSettlementPrice] = br.ReadSingle();
                fieldInt64[FieldIndex.PreOpenInterest] = (long)br.ReadSingle();
            }

            float preSettlementPrice = 0;
            if (fieldSingle.ContainsKey(FieldIndex.PreSettlementPrice))
                preSettlementPrice = (fieldSingle[FieldIndex.PreSettlementPrice]);
            if (now != 0 && preSettlementPrice != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preSettlementPrice;
                fieldSingle[FieldIndex.DifferRange] = (now - preSettlementPrice) / preSettlementPrice;
            }

            if (preSettlementPrice != 0)
            {
                fieldSingle[FieldIndex.Delta] = (high - low) / preSettlementPrice;
            }

            return true;

        }
    }

    /// <summary>
    /// 重仓持股
    /// </summary>
    public class ResFundHeaveStockReport : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, object> fieldObject;
            Dictionary<FieldIndex, string> fieldString;
            if (!DetailData.FieldIndexDataObject.TryGetValue(Code, out fieldObject))
            {
                fieldObject = new Dictionary<FieldIndex, object>(1);
                DetailData.FieldIndexDataObject[Code] = fieldObject;
            }

            int num = br.ReadInt32();
            List<int> stocks = new List<int>(num);
            for (int i = 0; i < num; i++)
            {
                int bgqDate = br.ReadInt32();
                byte countName = br.ReadByte();
                string strName = Encoding.Default.GetString(br.ReadBytes(countName));
                byte countCode = br.ReadByte();
                string strCode = Encoding.Default.GetString(br.ReadBytes(countCode));
                float fundHoldShare = br.ReadSingle();
                float fundHoldValue = br.ReadSingle();
                float fundStockValueRange = br.ReadSingle();
                int unicode = (int)br.ReadInt64();
                short typeShort = br.ReadInt16();
                if (unicode == 0)
                    continue;
                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }

                if (!DetailData.FieldIndexDataString.TryGetValue(unicode, out fieldString))
                {
                    fieldString = new Dictionary<FieldIndex, string>(1);
                    DetailData.FieldIndexDataString[unicode] = fieldString;
                }
                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.EMCode] = strCode;
                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.Code] = strCode.Split('.')[0];
                if (!fieldString.ContainsKey(FieldIndex.Name))
                    fieldString[FieldIndex.Name] = strName;
                if (!fieldInt32.ContainsKey(FieldIndex.Market))
                    fieldInt32[FieldIndex.Market] = (int)SecurityAttribute.TypeCodeToMarketType(typeShort, strCode);


                fieldSingle[FieldIndex.FundHoldShare] = fundHoldShare;
                fieldSingle[FieldIndex.FundHoldValue] = fundHoldValue;
                fieldSingle[FieldIndex.FundStockValueRange] = fundStockValueRange;
                fieldInt32[FieldIndex.BGQDate] = bgqDate;

                stocks.Add(unicode);
            }
            fieldObject[FieldIndex.FundHeaveStock] = stocks;
            return true;
        }
    }

    /// <summary>
    /// 重仓行业
    /// </summary>
    public class ResFundHYReport : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, object> fieldObject;
            Dictionary<FieldIndex, string> fieldString;
            if (!DetailData.FieldIndexDataObject.TryGetValue(Code, out fieldObject))
            {
                fieldObject = new Dictionary<FieldIndex, object>(1);
                DetailData.FieldIndexDataObject[Code] = fieldObject;
            }

            int num = br.ReadInt32();
            List<int> stocks = new List<int>(num);
            for (int i = 0; i < num; i++)
            {
                byte countName = br.ReadByte();
                string strName = Encoding.Default.GetString(br.ReadBytes(countName));
                int bgqDate = br.ReadInt32();
                float fundHoldValue = br.ReadSingle();
                float fundStockValueRange = br.ReadSingle();
                float fundScpz = br.ReadSingle();
                byte countCode = br.ReadByte();
                string strCode = Encoding.Default.GetString(br.ReadBytes(countCode));
                int unicode = (int)br.ReadInt64();
                short typeShort = br.ReadInt16();

                if (unicode == 0)
                    continue;
                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }

                if (!DetailData.FieldIndexDataString.TryGetValue(unicode, out fieldString))
                {
                    fieldString = new Dictionary<FieldIndex, string>(1);
                    DetailData.FieldIndexDataString[unicode] = fieldString;
                }

                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.EMCode] = strCode;
                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.Code] = strCode.Split('.')[0];
                if (!fieldString.ContainsKey(FieldIndex.Name))
                    fieldString[FieldIndex.Name] = strName;
                if (!fieldInt32.ContainsKey(FieldIndex.Market))
                    fieldInt32[FieldIndex.Market] = (int)SecurityAttribute.TypeCodeToMarketType(typeShort, strCode);


                fieldSingle[FieldIndex.FundHoldValue] = fundHoldValue;
                fieldSingle[FieldIndex.FundStockValueRange] = fundStockValueRange;
                fieldSingle[FieldIndex.FundScpz] = fundScpz;
                fieldInt32[FieldIndex.BGQDate] = bgqDate;
                stocks.Add(unicode);
            }
            fieldObject[FieldIndex.FundHeaveHY] = stocks;
            return true;
        }
    }

    /// <summary>
    /// 重仓债券
    /// </summary>
    public class ResKeyBondReport : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }


        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, object> fieldObject;
            Dictionary<FieldIndex, string> fieldString;
            if (!DetailData.FieldIndexDataObject.TryGetValue(Code, out fieldObject))
            {
                fieldObject = new Dictionary<FieldIndex, object>(1);
                DetailData.FieldIndexDataObject[Code] = fieldObject;
            }
            int num = br.ReadInt32();
            List<int> stocks = new List<int>(num);
            for (int i = 0; i < num; i++)
            {
                byte countName = br.ReadByte();
                string strName = Encoding.Default.GetString(br.ReadBytes(countName));
                byte countCode = br.ReadByte();
                string strCode = Encoding.Default.GetString(br.ReadBytes(countCode));
                int bgqDate = br.ReadInt32();
                float FundHoldValue = br.ReadSingle();
                float FundNetValueRange = br.ReadSingle();
                float BondTomrtyyear = br.ReadSingle();
                float FundBondRange = br.ReadSingle();
                byte countBondStrZtpj = br.ReadByte();
                string strBondStrZtpj = Encoding.Default.GetString(br.ReadBytes(countBondStrZtpj));
                byte countBondType = br.ReadByte();
                string strBondType = Encoding.Default.GetString(br.ReadBytes(countBondType));
                byte countBondMarket = br.ReadByte();
                string strBondMarket = Encoding.Default.GetString(br.ReadBytes(countBondMarket));
                int unicode = (int)br.ReadInt64();
                short typeShort = br.ReadInt16();
                if (unicode == 0)
                    continue;
                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }

                if (!DetailData.FieldIndexDataString.TryGetValue(unicode, out fieldString))
                {
                    fieldString = new Dictionary<FieldIndex, string>(1);
                    DetailData.FieldIndexDataString[unicode] = fieldString;
                }

                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.EMCode] = strCode;
                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.Code] = strCode.Split('.')[0];
                if (!fieldString.ContainsKey(FieldIndex.Name))
                    fieldString[FieldIndex.Name] = strName;
                if (!fieldInt32.ContainsKey(FieldIndex.Market))
                    fieldInt32[FieldIndex.Market] = (int)SecurityAttribute.TypeCodeToMarketType(typeShort, strCode);

                fieldSingle[FieldIndex.FundHoldValue] = FundHoldValue;
                fieldSingle[FieldIndex.FundNetValueRange] = FundNetValueRange;
                fieldSingle[FieldIndex.BondTomrtyyear] = BondTomrtyyear;
                fieldSingle[FieldIndex.FundBondRange] = FundBondRange;
                fieldString[FieldIndex.BondStrZtpj] = strBondStrZtpj;
                fieldString[FieldIndex.BondType] = strBondType;
                fieldString[FieldIndex.BondMarket] = strBondMarket;
                fieldInt32[FieldIndex.BGQDate] = bgqDate;
                stocks.Add(unicode);
            }
            fieldObject[FieldIndex.FundKeyBond] = stocks;
            return true;
        }
    }

    /// <summary>
    /// 基金经理
    /// </summary>
    public class ResFundManager : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }


        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();

            Dictionary<FieldIndex, object> fieldObject;

            if (!DetailData.FieldIndexDataObject.TryGetValue(Code, out fieldObject))
            {
                fieldObject = new Dictionary<FieldIndex, object>(1);
                DetailData.FieldIndexDataObject[Code] = fieldObject;
            }
            int num = br.ReadInt32();
            List<FundManagerDataRec> data = new List<FundManagerDataRec>(num);
            for (int i = 0; i < num; i++)
            {
                FundManagerDataRec oneRec = new FundManagerDataRec();
                byte managerByte = br.ReadByte();
                oneRec.ManagerName = Encoding.Default.GetString(br.ReadBytes(managerByte));
                oneRec.StartDate = br.ReadInt32();
                oneRec.EndDate = br.ReadInt32();
                oneRec.YieldSinces = br.ReadSingle();
                oneRec.AyieldSinces = br.ReadSingle();
                byte rankByte = br.ReadByte();
                oneRec.Rank = Encoding.Default.GetString(br.ReadBytes(rankByte));
                oneRec.Cyjzhb = br.ReadSingle();
                data.Add(oneRec);
            }
            fieldObject[FieldIndex.FundHeaveManager] = data;
            return true;
        }
    }

    /// <summary>
    /// 理财重仓持股
    /// </summary>
    public class ResFinanceHeaveStockReport : ResFundHeaveStockReport
    {
    }

    /// <summary>
    /// 理财重仓行业
    /// </summary>
    public class ResFinanceHYReport : ResFundHYReport
    {
    }

    /// <summary>
    /// 理财重仓债券
    /// </summary>
    public class ResFinanceKeyBondReport : ResKeyBondReport
    {

    }

    /// <summary>
    /// 理财基金经理
    /// </summary>
    public class ResFinanceManager : ResFundManager
    {

    }

    /// <summary>
    /// 重仓基金
    /// </summary>
    public class ResFinanceHeaveFundReprotDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, object> fieldObject;
            Dictionary<FieldIndex, string> fieldString;
            if (!DetailData.FieldIndexDataObject.TryGetValue(Code, out fieldObject))
            {
                fieldObject = new Dictionary<FieldIndex, object>(1);
                DetailData.FieldIndexDataObject[Code] = fieldObject;
            }
            int num = br.ReadInt32();
            List<int> stocks = new List<int>(num);
            for (int i = 0; i < num; i++)
            {
                byte countCode = br.ReadByte();
                string strCode = Encoding.Default.GetString(br.ReadBytes(countCode));
                byte countName = br.ReadByte();
                string strName = Encoding.Default.GetString(br.ReadBytes(countName));
                byte tmp = br.ReadByte();
                string strtmp = Encoding.Default.GetString(br.ReadBytes(tmp));
                int bgqDate = br.ReadInt32();
                float fundHoldShare = br.ReadSingle();
                float fundHoldValue = br.ReadSingle();
                float pctnv = br.ReadSingle();
                float aayield = br.ReadSingle();
                byte countParaname = br.ReadByte();
                string strParaname = Encoding.Default.GetString(br.ReadBytes(countParaname));
                byte countManager = br.ReadByte();
                string manager = Encoding.Default.GetString(br.ReadBytes(countManager));
                int endDate = br.ReadInt32();
                int unicode = (int)br.ReadInt64();
                short typeShort = br.ReadInt16();
                if (unicode == 0)
                    continue;
                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }

                if (!DetailData.FieldIndexDataString.TryGetValue(unicode, out fieldString))
                {
                    fieldString = new Dictionary<FieldIndex, string>(1);
                    DetailData.FieldIndexDataString[unicode] = fieldString;
                }

                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.EMCode] = strCode;
                if (!fieldString.ContainsKey(FieldIndex.EMCode))
                    fieldString[FieldIndex.Code] = strCode.Split('.')[0];
                if (!fieldString.ContainsKey(FieldIndex.Name))
                    fieldString[FieldIndex.Name] = strName;
                if (!fieldInt32.ContainsKey(FieldIndex.Market))
                    fieldInt32[FieldIndex.Market] = (int)SecurityAttribute.TypeCodeToMarketType(typeShort, strCode);

                fieldSingle[FieldIndex.FundHoldShare] = fundHoldShare;
                fieldSingle[FieldIndex.FundHoldValue] = fundHoldValue;
                fieldSingle[FieldIndex.FundNetValueRange] = pctnv;
                fieldSingle[FieldIndex.FundDecYearhb] = aayield;
                fieldString[FieldIndex.FundParaname] = strParaname;
                fieldString[FieldIndex.FundFmanager] = manager;
                fieldInt32[FieldIndex.FundLatestDate] = endDate;
                fieldInt32[FieldIndex.BGQDate] = bgqDate;
                stocks.Add(unicode);
            }
            fieldObject[FieldIndex.FinanceHeaveFund] = stocks;
            return true;
        }
    }

    /// <summary>
    /// 东财指数Detail
    /// </summary>
    public class ResEMIndexDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }



            bool isPush = br.ReadBoolean();
            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            float high = br.ReadSingle();
            fieldSingle[FieldIndex.High] = high;
            float low = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = low;
            long volume = br.ReadInt64();
            double amount = br.ReadDouble();
            fieldInt64[FieldIndex.IndexVolume] = volume;
            fieldDouble[FieldIndex.IndexAmount] = amount;
            fieldInt64[FieldIndex.Volume] = volume;
            fieldDouble[FieldIndex.Amount] = amount;
            fieldInt32[FieldIndex.UpNum] = br.ReadInt32();
            fieldInt32[FieldIndex.EqualNum] = br.ReadInt32();
            fieldInt32[FieldIndex.DownNum] = br.ReadInt32();

            float preClose = 0;
            float preCloseDay5 = 0;
            float preCloseDay20 = 0;
            float preCloseDay60 = 0;
            float preCloseDayYTD = 0;
            float highw52 = 0;
            float loww52 = 0;

            if (!isPush)
            {
                preClose = br.ReadSingle();
                preCloseDay5 = br.ReadSingle();
                preCloseDay20 = br.ReadSingle();
                preCloseDay60 = br.ReadSingle();
                preCloseDayYTD = br.ReadSingle();
                highw52 = br.ReadSingle();
                loww52 = br.ReadSingle();

                fieldSingle[FieldIndex.PreClose] = preClose;
                fieldSingle[FieldIndex.PreCloseDay5] = preCloseDay5;
                fieldSingle[FieldIndex.PreCloseDay20] = preCloseDay20;
                fieldSingle[FieldIndex.PreCloseDay60] = preCloseDay60;
                fieldSingle[FieldIndex.PreCloseDayYTD] = preCloseDayYTD;
                fieldSingle[FieldIndex.HighW52] = highw52;
                fieldSingle[FieldIndex.LowW52] = loww52;


            }
            else
            {
                preClose = (fieldSingle[FieldIndex.PreClose]);
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
                preCloseDayYTD = (fieldSingle[FieldIndex.PreCloseDayYTD]);
                highw52 = (fieldSingle[FieldIndex.HighW52]);
                loww52 = (fieldSingle[FieldIndex.LowW52]);


            }

            if (now != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                fieldSingle[FieldIndex.Difference] = (now - preClose);
            }
            if (now != 0 && preCloseDay5 != 0)
                fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
            if (now != 0 && preCloseDay20 != 0)
                fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
            if (now != 0 && preCloseDay60 != 0)
                fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
            if (now != 0 && preCloseDayYTD != 0)
                fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseDayYTD) / preCloseDayYTD;

            if (now != 0)
            {
                fieldSingle[FieldIndex.HighW52] = Math.Max(high, highw52);
                fieldSingle[FieldIndex.LowW52] = Math.Min(low, loww52);
            }

            return true;
        }
    }

    /// <summary>
    /// 巨潮指数Detail
    /// </summary>
    public class ResCNIndexDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }

            float now = 0, preClose = 0, preCloseDay5 = 0, preCloseDay20 = 0, preCloseDay60 = 0, preCloseYtd = 0;
            now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            preClose = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = preClose;

            long volume = br.ReadInt64();
            fieldInt64[FieldIndex.Volume] = volume;
            fieldInt64[FieldIndex.IndexVolume] = volume;
            double amount = br.ReadDouble();
            fieldDouble[FieldIndex.Amount] = amount;
            fieldDouble[FieldIndex.IndexAmount] = amount;

            preCloseDay5 = br.ReadSingle();
            preCloseDay20 = br.ReadSingle();
            preCloseDay60 = br.ReadSingle();
            preCloseYtd = br.ReadSingle();
            fieldSingle[FieldIndex.HighW52] = br.ReadSingle();
            fieldSingle[FieldIndex.LowW52] = br.ReadSingle();

            if (now != 0)
            {
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }
            }
            else
            {
                now = preClose;
            }

            if (preCloseDay5 != 0)
                fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
            if (preCloseDay20 != 0)
                fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
            if (preCloseDay60 != 0)
                fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
            if (preCloseYtd != 0)
                fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;

            return true;
        }
    }

    /// <summary>
    /// 中债指数Detail
    /// </summary>
    public class ResCSIndexDetailDataPacket : ResCNIndexDetailDataPacket
    {

        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, float> fieldSingle;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }


            float now = 0, preClose = 0, preCloseDay5 = 0, preCloseDay20 = 0, preCloseDay60 = 0, preCloseYtd = 0;
            now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            preClose = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = preClose;

            preCloseDay5 = br.ReadSingle();
            preCloseDay20 = br.ReadSingle();
            preCloseDay60 = br.ReadSingle();
            preCloseYtd = br.ReadSingle();
            fieldSingle[FieldIndex.HighW52] = br.ReadSingle();
            fieldSingle[FieldIndex.LowW52] = br.ReadSingle();

            if (now != 0)
            {
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }
            }
            else
            {
                now = preClose;
            }

            if (preCloseDay5 != 0)
                fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
            if (preCloseDay20 != 0)
                fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
            if (preCloseDay60 != 0)
                fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
            if (preCloseYtd != 0)
                fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;

            return true;
        }
    }

    /// <summary>
    /// 中证指数Detail
    /// </summary>
    public class ResCSIIndexDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            float now = 0, preClose = 0, preCloseDay5 = 0, preCloseDay20 = 0, preCloseDay60 = 0, preCloseYtd = 0;
            now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            preClose = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = preClose;

            preCloseDay5 = br.ReadSingle();
            preCloseDay20 = br.ReadSingle();
            preCloseDay60 = br.ReadSingle();
            preCloseYtd = br.ReadSingle();
            fieldSingle[FieldIndex.HighW52] = br.ReadSingle();
            fieldSingle[FieldIndex.LowW52] = br.ReadSingle();
            fieldInt32[FieldIndex.UpNum] = br.ReadInt32();
            fieldInt32[FieldIndex.EqualNum] = br.ReadInt32();
            fieldInt32[FieldIndex.DownNum] = br.ReadInt32();

            if (now != 0)
            {
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }
            }
            else
            {
                now = preClose;
            }

            if (preCloseDay5 != 0)
                fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
            if (preCloseDay20 != 0)
                fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
            if (preCloseDay60 != 0)
                fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
            if (preCloseYtd != 0)
                fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;


            return true;
        }
    }

    /// <summary>
    /// 全球指数
    /// </summary>
    public class ResGlobalIndexDetailDataPacket : ResCNIndexDetailDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }

            bool isPush = br.ReadBoolean();
            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            float high = br.ReadSingle();
            fieldSingle[FieldIndex.High] = high;
            float low = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = low;
            fieldInt64[FieldIndex.Volume] = br.ReadInt64();
            fieldDouble[FieldIndex.Amount] = br.ReadDouble();

            float preClose = 0;
            float preCloseDay5 = 0;
            float preCloseDay20 = 0;
            float preCloseDay60 = 0;
            float preCloseDayYTD = 0;
            float highw52 = 0;
            float loww52 = 0;
            if (!isPush)
            {
                preClose = br.ReadSingle();
                preCloseDay5 = br.ReadSingle();
                preCloseDay20 = br.ReadSingle();
                preCloseDay60 = br.ReadSingle();
                preCloseDayYTD = br.ReadSingle();
                highw52 = br.ReadSingle();
                loww52 = br.ReadSingle();

                fieldSingle[FieldIndex.PreClose] = preClose;
                fieldSingle[FieldIndex.PreCloseDay5] = preCloseDay5;
                fieldSingle[FieldIndex.PreCloseDay20] = preCloseDay20;
                fieldSingle[FieldIndex.PreCloseDay60] = preCloseDay60;
                fieldSingle[FieldIndex.PreCloseDayYTD] = preCloseDayYTD;
                fieldSingle[FieldIndex.HighW52] = highw52;
                fieldSingle[FieldIndex.LowW52] = loww52;


            }
            else
            {
                preClose = (fieldSingle[FieldIndex.PreClose]);
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
                preCloseDayYTD = (fieldSingle[FieldIndex.PreCloseDayYTD]);
                highw52 = (fieldSingle[FieldIndex.HighW52]);
                loww52 = (fieldSingle[FieldIndex.LowW52]);


            }

            if (now != 0)
            {
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                    fieldSingle[FieldIndex.Difference] = (now - preClose);
                }
            }
            else
            {
                now = preClose;
            }
            if (now != 0 && preCloseDay5 != 0)
                fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
            if (now != 0 && preCloseDay20 != 0)
                fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
            if (now != 0 && preCloseDay60 != 0)
                fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
            if (now != 0 && preCloseDayYTD != 0)
                fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseDayYTD) / preCloseDayYTD;

            if (now != 0)
            {
                fieldSingle[FieldIndex.HighW52] = Math.Max(high, highw52);
                fieldSingle[FieldIndex.LowW52] = Math.Min(low, loww52);
            }
            return true;
        }
    }

    /// <summary>
    /// 银行间债券Detail
    /// </summary>
    public class ResInterBankDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }
            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }


            bool isPush = br.ReadBoolean();
            int time = br.ReadInt32();
            fieldInt32[FieldIndex.Time] = time;
            float netNow = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = netNow;
            float netPreClose = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = netPreClose;
            if (netNow != 0 && netPreClose != 0)
            {
                fieldSingle[FieldIndex.DifferRange] = (netNow - netPreClose) / netPreClose;
                fieldSingle[FieldIndex.Difference] = (netNow - netPreClose);
            }

            float ai = br.ReadSingle();
            fieldSingle[FieldIndex.BondAI] = ai;
            fieldSingle[FieldIndex.BondNetNow] = netNow;
            fieldSingle[FieldIndex.BondFullNow] = netNow + ai;
            float ytm = br.ReadSingle();
            float bp = br.ReadSingle();
            fieldSingle[FieldIndex.BondNowYTM] = ytm;
            fieldSingle[FieldIndex.BondDiffRangeYTM] = bp;
            fieldString[FieldIndex.YTMAndBP] = string.Format(
                "{0}({1}BP)", ytm == 0 ? "─" : ytm.ToString("F2"), ytm == 0 ? "─" : (bp).ToString("F2"));
            fieldSingle[FieldIndex.BondDuration] = br.ReadSingle();
            fieldSingle[FieldIndex.BondConvexity] = br.ReadSingle();

            long volume = br.ReadInt64();
            fieldInt64[FieldIndex.Volume] = volume;
            fieldInt64[FieldIndex.IndexVolume] = volume;
            double amount = br.ReadDouble();
            fieldDouble[FieldIndex.Amount] = amount;
            fieldDouble[FieldIndex.IndexAmount] = amount;

            float netOpen = br.ReadSingle();
            fieldSingle[FieldIndex.BondNetOpen] = netOpen;
            float ytmOpen = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMOpen] = ytmOpen;
            float netHigh = br.ReadSingle();
            fieldSingle[FieldIndex.BondNetHigh] = netHigh;
            float ytmHigh = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMHigh] = ytmHigh;
            float netAvg = br.ReadSingle();
            fieldSingle[FieldIndex.AveragePrice] = netAvg;

            fieldDouble[FieldIndex.Amount] = Convert.ToDouble(volume * netAvg);

            float ytmAvg = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMAvg] = ytmAvg;
            float netLow = br.ReadSingle();
            fieldSingle[FieldIndex.BondNetLow] = netLow;
            float ytmLow = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMLow] = ytmLow;
            fieldString[FieldIndex.BondNetYTMOpen] = netOpen.ToString() + '/' + ytmOpen.ToString();
            fieldString[FieldIndex.BondNetYTMHigh] = netHigh.ToString() + '/' + ytmHigh.ToString();
            fieldString[FieldIndex.BondDecAvgNetDecAvgYTM] = netAvg.ToString() + '/' + ytmAvg.ToString();
            fieldString[FieldIndex.BondNetYTMLow] = netLow.ToString() + '/' + ytmLow.ToString();

            fieldSingle[FieldIndex.BondBestSellNet] = br.ReadSingle();
            fieldSingle[FieldIndex.BondBestSellYtm] = br.ReadSingle();
            fieldDouble[FieldIndex.BondBestSellTotalFaceValue] = br.ReadInt64();
            byte initiator4Size = br.ReadByte();
            fieldString[FieldIndex.BondBestSellInitiator] = Encoding.Default.GetString(br.ReadBytes(initiator4Size));

            fieldSingle[FieldIndex.BondBestBuyNet] = br.ReadSingle();
            fieldSingle[FieldIndex.BondBestBuyYtm] = br.ReadSingle();
            fieldDouble[FieldIndex.BondBestBuyTotalFaceValue] = br.ReadInt64();
            byte initiator1Size = br.ReadByte();
            fieldString[FieldIndex.BondBestBuyInitiator] = Encoding.Default.GetString(br.ReadBytes(initiator1Size));

            if (!isPush)
            {

                byte zqpjSize = br.ReadByte();
                fieldString[FieldIndex.BondStrZqpj] = Encoding.Default.GetString(br.ReadBytes(zqpjSize));
                fieldSingle[FieldIndex.BondBondperiod] = br.ReadSingle();
                fieldSingle[FieldIndex.BondNewrate] = br.ReadSingle();
                fieldDouble[FieldIndex.ZQYE] = br.ReadSingle();
                byte tstkSize = br.ReadByte();
                fieldString[FieldIndex.BondStrTstk] = Encoding.Default.GetString(br.ReadBytes(tstkSize));
                byte sfdbSize = br.ReadByte();
                fieldString[FieldIndex.BondIsVouch] = Encoding.Default.GetString(br.ReadBytes(sfdbSize));
                byte instsNameSize = br.ReadByte();
                string instName = Encoding.Default.GetString(br.ReadBytes(instsNameSize));
                fieldString[FieldIndex.BondInstname] = instName;
                byte ztpjSize = br.ReadByte();
                fieldString[FieldIndex.BondStrZtpj] = Encoding.Default.GetString(br.ReadBytes(ztpjSize));
                fieldSingle[FieldIndex.BondTomrtyyear] = br.ReadSingle();
                byte parTypeSize = br.ReadByte();
                fieldString[FieldIndex.BondType] = Encoding.Default.GetString(br.ReadBytes(parTypeSize));
                fieldInt32[FieldIndex.BondExerciseDay] = br.ReadInt32();
                byte marketNameSize = br.ReadByte();
                fieldString[FieldIndex.BondTradeMarket] = Encoding.Default.GetString(br.ReadBytes(marketNameSize));

                //    中债估值:估价净价 、估价收益率  、估值日期
                fieldSingle[FieldIndex.BondCBNet] = br.ReadSingle();
                fieldSingle[FieldIndex.BondCBYtm] = br.ReadSingle();
                fieldInt32[FieldIndex.BondCBDate] = br.ReadInt32();

                // 中证估值:估价净价 、估价收益率  、估值日期
                fieldSingle[FieldIndex.BondCSNet] = br.ReadSingle();
                fieldSingle[FieldIndex.BondCSYtm] = br.ReadSingle();
                fieldInt32[FieldIndex.BondCSDate] = br.ReadInt32();

                // 上清所估值:估价净价 、估价收益率  、估值日期
                fieldSingle[FieldIndex.BondSNNet] = br.ReadSingle();
                fieldSingle[FieldIndex.BondSNYtm] = br.ReadSingle();
                fieldInt32[FieldIndex.BondSNDate] = br.ReadInt32();

                byte instTypeSize = br.ReadByte();
                string instType = Encoding.Default.GetString(br.ReadBytes(instTypeSize));
                if (string.IsNullOrEmpty(instType))
                {
                    instType = "─";
                }
                fieldString[FieldIndex.BondInstType] = instType;

                byte instIndustrySize = br.ReadByte();
                string instIndustry = Encoding.Default.GetString(br.ReadBytes(instIndustrySize));
                if (string.IsNullOrEmpty(instIndustry))
                {
                    instIndustry = "─";
                }
                fieldString[FieldIndex.BondInstIndustry] = instIndustry;

                fieldString[FieldIndex.BondInstDetail] = string.Format("{0}({1},{2})", instName, instType, instIndustry);
            }
            return true;
        }
    }

    /// <summary>
    /// 可转债
    /// </summary>
    public class ResConvertBondDetailDataPacket : OrgDataPacket
    {
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            long sid = br.ReadInt64();
            string emcode = ConvertCodeOrg.ConvertLongToCode(sid);
            if (!DetailData.EmCodeToUnicode.TryGetValue(emcode, out Code))
                return false;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }
            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }

            bool isPush = br.ReadBoolean();
            fieldSingle[FieldIndex.BondNowYTM] = br.ReadSingle();
            fieldSingle[FieldIndex.BondDuration] = br.ReadSingle();
            fieldSingle[FieldIndex.BondConvexity] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMOpen] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMAvg] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMHigh] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMLow] = br.ReadSingle();
            if (!isPush)
            {
                byte zqpjSize = br.ReadByte();
                fieldString[FieldIndex.BondStrZqpj] = Encoding.Default.GetString(br.ReadBytes(zqpjSize));
                fieldSingle[FieldIndex.BondBondperiod] = br.ReadSingle();
                byte ztpjSize = br.ReadByte();
                fieldString[FieldIndex.BondStrZtpj] = Encoding.Default.GetString(br.ReadBytes(ztpjSize));
                fieldSingle[FieldIndex.BondTomrtyyear] = br.ReadSingle();
                fieldSingle[FieldIndex.BondNewrate] = br.ReadSingle();
                fieldDouble[FieldIndex.ZQYE] = Convert.ToDouble(br.ReadSingle());
                byte parTypeSize = br.ReadByte();
                fieldString[FieldIndex.BondType] = Encoding.Default.GetString(br.ReadBytes(parTypeSize));
                fieldSingle[FieldIndex.BondAI] = br.ReadSingle();
                byte tstkSize = br.ReadByte();
                fieldString[FieldIndex.BondStrTstk] = Encoding.Default.GetString(br.ReadBytes(tstkSize));
                byte sfdbSize = br.ReadByte();
                fieldString[FieldIndex.BondIsVouch] = Encoding.Default.GetString(br.ReadBytes(sfdbSize));
                fieldInt32[FieldIndex.BondExerciseDay] = br.ReadInt32();
                byte marketNameSize = br.ReadByte();
                fieldString[FieldIndex.BondTradeMarket] = Encoding.Default.GetString(br.ReadBytes(marketNameSize));
                fieldSingle[FieldIndex.BondDecLcytm] = br.ReadSingle();
                fieldSingle[FieldIndex.BondConversionRate] = br.ReadSingle();
                fieldSingle[FieldIndex.BondSwapPrice] = br.ReadSingle();
                fieldSingle[FieldIndex.CZJZ] = br.ReadSingle();
            }

            float preYtm = 0, nowYtm = 0;
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                preYtm = (fieldSingle[FieldIndex.BondDecLcytm]);

            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                nowYtm = (fieldSingle[FieldIndex.BondNowYTM]);

            if (preYtm != 0 && nowYtm != 0)
            {
                float bp = (nowYtm - preYtm) * 100;
                fieldSingle[FieldIndex.BondDiffRangeYTM] = bp;
                fieldString[FieldIndex.YTMAndBP] = string.Format("{0}({1}BP)", nowYtm == 0 ? "─" : nowYtm.ToString("F2"),
                    nowYtm == 0 ? "─" : (bp).ToString("F2"));
            }

            return true;
        }
    }

    /// <summary>
    /// 非可转债
    /// </summary>
    public class ResNonConvertBondDetailDataPacket : OrgDataPacket
    {
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            long sid = br.ReadInt64();
            string emcode = ConvertCodeOrg.ConvertLongToCode(sid);
            if (!DetailData.EmCodeToUnicode.TryGetValue(emcode, out Code))
                return false;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, double> fieldDouble;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }
            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }

            bool isPush = br.ReadBoolean();
            fieldSingle[FieldIndex.BondNowYTM] = br.ReadSingle();
            fieldSingle[FieldIndex.BondDuration] = br.ReadSingle();
            fieldSingle[FieldIndex.BondConvexity] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMOpen] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMAvg] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMHigh] = br.ReadSingle();
            fieldSingle[FieldIndex.BondYTMLow] = br.ReadSingle();
            if (!isPush)
            {
                byte zqpjSize = br.ReadByte();
                fieldString[FieldIndex.BondStrZqpj] = Encoding.Default.GetString(br.ReadBytes(zqpjSize));
                fieldSingle[FieldIndex.BondBondperiod] = br.ReadSingle();
                byte ztpjSize = br.ReadByte();
                fieldString[FieldIndex.BondStrZtpj] = Encoding.Default.GetString(br.ReadBytes(ztpjSize));
                fieldSingle[FieldIndex.BondTomrtyyear] = br.ReadSingle();
                fieldSingle[FieldIndex.BondNewrate] = br.ReadSingle();
                fieldDouble[FieldIndex.ZQYE] = Convert.ToDouble(br.ReadSingle());
                byte parTypeSize = br.ReadByte();
                fieldString[FieldIndex.BondType] = Encoding.Default.GetString(br.ReadBytes(parTypeSize));
                fieldSingle[FieldIndex.BondAI] = br.ReadSingle();
                byte tstkSize = br.ReadByte();
                fieldString[FieldIndex.BondStrTstk] = Encoding.Default.GetString(br.ReadBytes(tstkSize));
                byte sfdbSize = br.ReadByte();
                fieldString[FieldIndex.BondIsVouch] = Encoding.Default.GetString(br.ReadBytes(sfdbSize));
                fieldInt32[FieldIndex.BondExerciseDay] = br.ReadInt32();
                byte marketNameSize = br.ReadByte();
                fieldString[FieldIndex.BondTradeMarket] = Encoding.Default.GetString(br.ReadBytes(marketNameSize));
                fieldSingle[FieldIndex.BondDecLcytm] = br.ReadSingle();
                //    中债估值:估价净价 、估价收益率  、估值日期
                fieldSingle[FieldIndex.BondCBNet] = br.ReadSingle();
                fieldSingle[FieldIndex.BondCBYtm] = br.ReadSingle();
                fieldInt32[FieldIndex.BondCBDate] = br.ReadInt32();

                // 中证估值:估价净价 、估价收益率  、估值日期
                fieldSingle[FieldIndex.BondCSNet] = br.ReadSingle();
                fieldSingle[FieldIndex.BondCSYtm] = br.ReadSingle();
                fieldInt32[FieldIndex.BondCSDate] = br.ReadInt32();

                byte instsNameSize = br.ReadByte();
                string Instname = Encoding.Default.GetString(br.ReadBytes(instsNameSize));
                if (string.IsNullOrEmpty(Instname))
                    Instname = "─";
                fieldString[FieldIndex.BondInstname] = Instname;

                byte typeSize = br.ReadByte();
                string instType = Encoding.Default.GetString(br.ReadBytes(typeSize));
                if (string.IsNullOrEmpty(instType))
                    instType = "─";

                byte industrySize = br.ReadByte();
                string instIndustry = Encoding.Default.GetString(br.ReadBytes(industrySize));
                if (string.IsNullOrEmpty(instIndustry))
                    instIndustry = "─";

                fieldString[FieldIndex.BondInstDetail] = Instname + '(' + instType + ',' + instIndustry + ')';
            }
            float preYtm = 0, nowYtm = 0;
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                preYtm = (fieldSingle[FieldIndex.BondDecLcytm]);

            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                nowYtm = (fieldSingle[FieldIndex.BondNowYTM]);

            if (preYtm != 0 && nowYtm != 0)
            {
                float bp = (nowYtm - preYtm) * 100;
                fieldSingle[FieldIndex.BondDiffRangeYTM] = bp;
                fieldString[FieldIndex.YTMAndBP] = string.Format("{0}({1}BP)", nowYtm == 0 ? "─" : nowYtm.ToString("F2"),
                    nowYtm == 0 ? "─" : (bp).ToString("F2"));
            }

            return true;
        }
    }

    /// <summary>
    /// 利率互换
    /// </summary>
    public class ResRateSwapDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }
            bool isPush = br.ReadBoolean();
            float now = br.ReadSingle();
            float rateDecAvgPrice = br.ReadSingle(); // 加权平均利率 
            float preClose = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.RateDecAvgPrice] = rateDecAvgPrice;
            fieldSingle[FieldIndex.PreClose] = preClose;
            if (now != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
            }
            fieldInt32[FieldIndex.Date] = br.ReadInt32();
            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();

            float pavg = 0; //前加权 

            float avg4D = 0, avg9D = 0, avg19D = 0, avg59D = 0, avg119D = 0, avg249D = 0;
            if (!isPush)
            {
                pavg = br.ReadSingle();
                fieldSingle[FieldIndex.RatePreDecPrice] = pavg;
                avg4D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre4D] = avg4D;
                avg9D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre9D] = avg9D;
                avg19D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre19D] = avg19D;
                avg59D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre59D] = avg59D;
                avg119D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre119D] = avg119D;
                avg249D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre249D] = avg249D;
                short size = br.ReadInt16();
                fieldString[FieldIndex.Ratedes] = Encoding.Default.GetString(br.ReadBytes(size));
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre4D))
                    avg4D = (fieldSingle[FieldIndex.RateAvgPricePre4D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre9D))
                    avg9D = (fieldSingle[FieldIndex.RateAvgPricePre9D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre19D))
                    avg19D = (fieldSingle[FieldIndex.RateAvgPricePre19D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre59D))
                    avg59D = (fieldSingle[FieldIndex.RateAvgPricePre59D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre119D))
                    avg119D = (fieldSingle[FieldIndex.RateAvgPricePre119D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre249D))
                    avg249D = (fieldSingle[FieldIndex.RateAvgPricePre249D]);
            }
            fieldSingle[FieldIndex.RateAvgPrice5D] = (now + avg4D) / 5;
            fieldSingle[FieldIndex.RateAvgPrice10D] = (now + avg9D) / 10;
            fieldSingle[FieldIndex.RateAvgPrice20D] = (now + avg19D) / 20;
            fieldSingle[FieldIndex.RateAvgPrice60D] = (now + avg59D) / 60;
            fieldSingle[FieldIndex.RateAvgPrice120D] = (now + avg119D) / 120;
            fieldSingle[FieldIndex.RateAvgPrice250D] = (now + avg249D) / 250;

            return true;
        }
    }

    /// <summary>
    /// 银行间拆借
    /// </summary>
    public class ResInterBankRepurchaseDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }

            bool isPush = br.ReadBoolean();
            float now = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            float avg = br.ReadSingle();
            fieldSingle[FieldIndex.RateDecAvgPrice] = avg;
            fieldSingle[FieldIndex.AveragePrice] = avg;
            float preClose = br.ReadSingle();
            long volume = br.ReadInt64();
            fieldInt64[FieldIndex.Volume] = volume;
            fieldInt64[FieldIndex.IndexVolume] = volume;
            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            if (now != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                fieldSingle[FieldIndex.Difference] = (now - preClose);
            }

            float avgPre = 0, avg4D = 0, avg9D = 0, avg19D = 0, avg59D = 0, avg119D = 0, avg249D = 0;
            if (!isPush)
            {
                avgPre = br.ReadSingle();
                fieldSingle[FieldIndex.RatePreDecPrice] = avgPre;
                avg4D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre4D] = avg4D;
                avg9D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre9D] = avg9D;
                avg19D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre19D] = avg19D;
                avg59D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre59D] = avg59D;
                avg119D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre119D] = avg119D;
                avg249D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre249D] = avg249D;
                short size = br.ReadInt16();
                fieldString[FieldIndex.Ratedes] = Encoding.Default.GetString(br.ReadBytes(size));
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre4D))
                    avg4D = (fieldSingle[FieldIndex.RateAvgPricePre4D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre9D))
                    avg9D = (fieldSingle[FieldIndex.RateAvgPricePre9D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre19D))
                    avg19D = (fieldSingle[FieldIndex.RateAvgPricePre19D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre59D))
                    avg59D = (fieldSingle[FieldIndex.RateAvgPricePre59D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre119D))
                    avg119D = (fieldSingle[FieldIndex.RateAvgPricePre119D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre249D))
                    avg249D = (fieldSingle[FieldIndex.RateAvgPricePre249D]);
            }
            fieldSingle[FieldIndex.RateAvgPrice5D] = (now + avg4D) / 5;
            fieldSingle[FieldIndex.RateAvgPrice10D] = (now + avg9D) / 10;
            fieldSingle[FieldIndex.RateAvgPrice20D] = (now + avg19D) / 20;
            fieldSingle[FieldIndex.RateAvgPrice60D] = (now + avg59D) / 60;
            fieldSingle[FieldIndex.RateAvgPrice120D] = (now + avg119D) / 120;
            fieldSingle[FieldIndex.RateAvgPrice250D] = (now + avg249D) / 250;


            return true;
        }
    }

    /// <summary>
    /// shibor
    /// </summary>
    public class ResShiborDetailDataPacket : OrgDataPacket
    {

        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)br.ReadInt64();
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }

            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, string>(1);
                DetailData.FieldIndexDataString[Code] = fieldString;
            }
            bool ispush = br.ReadBoolean();
            float now = br.ReadSingle();
            float preClose = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = now;
            fieldSingle[FieldIndex.PreClose] = preClose;
            if (now != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                fieldSingle[FieldIndex.Difference] = now - preClose;
            }
            fieldInt32[FieldIndex.BondDate] = br.ReadInt32();
            fieldInt32[FieldIndex.Time] = br.ReadInt32();
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();

            float avg4D = 0, avg9D = 0, avg19D = 0, avg59D = 0, avg119D = 0, avg249D = 0;
            if (!ispush)
            {
                avg4D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre4D] = avg4D;
                avg9D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre9D] = avg9D;
                avg19D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre19D] = avg19D;
                avg59D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre59D] = avg59D;
                avg119D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre119D] = avg119D;
                avg249D = br.ReadSingle();
                fieldSingle[FieldIndex.RateAvgPricePre249D] = avg249D;
                short size = br.ReadInt16();
                fieldString[FieldIndex.Ratedes] = Encoding.Default.GetString(br.ReadBytes(size));
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre4D))
                    avg4D = (fieldSingle[FieldIndex.RateAvgPricePre4D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre9D))
                    avg9D = (fieldSingle[FieldIndex.RateAvgPricePre9D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre19D))
                    avg19D = (fieldSingle[FieldIndex.RateAvgPricePre19D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre59D))
                    avg59D = (fieldSingle[FieldIndex.RateAvgPricePre59D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre119D))
                    avg119D = (fieldSingle[FieldIndex.RateAvgPricePre119D]);
                if (fieldSingle.ContainsKey(FieldIndex.RateAvgPricePre249D))
                    avg249D = (fieldSingle[FieldIndex.RateAvgPricePre249D]);
            }
            fieldSingle[FieldIndex.RateAvgPrice5D] = (now + avg4D) / 5;
            fieldSingle[FieldIndex.RateAvgPrice10D] = (now + avg9D) / 10;
            fieldSingle[FieldIndex.RateAvgPrice20D] = (now + avg19D) / 20;
            fieldSingle[FieldIndex.RateAvgPrice60D] = (now + avg59D) / 60;
            fieldSingle[FieldIndex.RateAvgPrice120D] = (now + avg119D) / 120;
            fieldSingle[FieldIndex.RateAvgPrice250D] = (now + avg249D) / 250;

            return true;
        }
    }

    /// <summary>
    /// 深度资料
    /// </summary>
    public class ResDepthAnalyseDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        /// <summary>
        /// 十大流通股东
        /// </summary>
        private ShareHolderDataRec _holderDataRec;

        public ShareHolderDataRec HolderDataRec
        {
            get { return _holderDataRec; }
            private set { _holderDataRec = value; }
        }

        /// <summary>
        /// 利润趋势
        /// </summary>
        private ProfitTrendDataRec _profitTrend;

        public ProfitTrendDataRec ProfitTrend
        {
            get { return _profitTrend; }
            private set { _profitTrend = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            long sid = br.ReadInt64();

            Code = 0;
            string strCode = string.Empty;
            if (sid > 10000000000)
                strCode = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
            else
                Code = Convert.ToInt32(sid);

            if ((!string.IsNullOrEmpty(strCode)) && DetailData.EmCodeToUnicode.ContainsKey(strCode))
            {
                Code = DetailData.EmCodeToUnicode[strCode];
            }


            int reportDate = br.ReadInt32();
            int lastReportDate = br.ReadInt32();
            float epsttm = br.ReadSingle();
            double totalOperatereve = br.ReadDouble();
            float netProfit = br.ReadSingle();
            float roeAvg = br.ReadSingle();
            float sumLiab = br.ReadSingle();
            float sumAsset = br.ReadSingle();
            float revenueYoy = (float)br.ReadDouble();
            float netProfitYoy = (float)br.ReadDouble();
            float grossProfitMargin = (float)br.ReadDouble();

            Dictionary<FieldIndex, float> fieldSingle;
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            fieldSingle[FieldIndex.IncomeRatio] = revenueYoy;
            fieldSingle[FieldIndex.NetProfitYOY] = netProfitYoy;
            if (!(float.IsInfinity(grossProfitMargin)))
                fieldSingle[FieldIndex.Gprofit] = grossProfitMargin;
            if (totalOperatereve != 0)
                fieldSingle[FieldIndex.NetProfitMargin] = Convert.ToSingle(netProfit / totalOperatereve);
            if (sumAsset != 0)
                fieldSingle[FieldIndex.DebtRatio] = sumLiab / sumAsset;

            HolderDataRec = new ShareHolderDataRec();
            HolderDataRec.GmCount = br.ReadInt32();
            HolderDataRec.SmCount = br.ReadInt32();
            HolderDataRec.SbCount = br.ReadInt32();
            HolderDataRec.QfiiCount = br.ReadInt32();
            HolderDataRec.ShareHolderNum = br.ReadInt32();
            HolderDataRec.ShareHolderNumYOY = br.ReadSingle();
            HolderDataRec.ShareHolderNumHis = br.ReadInt32();
            HolderDataRec.ShareHolderNumHisYOY = br.ReadSingle();
            HolderDataRec.GmCountHis = br.ReadInt32();
            HolderDataRec.SmCountHis = br.ReadInt32();
            HolderDataRec.SbCountHis = br.ReadInt32();
            HolderDataRec.QfiiCountHis = br.ReadInt32();
            HolderDataRec.ShareAvg = br.ReadSingle();
            HolderDataRec.ShareAvgHis = br.ReadSingle();
            HolderDataRec.ReportDate = reportDate;
            HolderDataRec.LastDate = lastReportDate;

            int numProfitTrend = br.ReadInt32();
            ProfitTrend = new ProfitTrendDataRec();
            for (int i = 0; i < numProfitTrend; i++)
            {
                int date = br.ReadInt32();
                float profit = br.ReadSingle();
                bool hasYear = false;
                foreach (OneYearProfitTrendDataRec oneYear in ProfitTrend.ProfitTrend)
                {
                    if (oneYear.Date / 10000 == date / 10000)
                    {
                        switch ((date % 10000) / 100)
                        {
                            case 12:
                                oneYear.Profit4 = profit;
                                break;
                            case 3:
                                oneYear.Profit1 = profit;
                                break;
                            case 9:
                                oneYear.Profit3 = profit;
                                break;
                            case 6:
                                oneYear.Profit2 = profit;
                                break;
                        }
                        hasYear = true;
                    }
                }
                if (!hasYear)
                {
                    OneYearProfitTrendDataRec oneYear = new OneYearProfitTrendDataRec();
                    oneYear.Date = date;
                    switch ((date % 10000) / 100)
                    {
                        case 12:
                            oneYear.Profit4 = profit;
                            break;
                        case 3:
                            oneYear.Profit1 = profit;
                            break;
                        case 9:
                            oneYear.Profit3 = profit;
                            break;
                        case 6:
                            oneYear.Profit2 = profit;
                            break;
                    }
                    ProfitTrend.ProfitTrend.Insert(0, oneYear);
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 综合排名
    /// </summary>
    public class ResRankOrgDataPacket : OrgDataPacket
    {
        private RankOrgDataRec _data;

        public RankOrgDataRec Data
        {
            get { return _data; }
            private set { _data = value; }
        }
        private string _blockId;

        public string BlockId
        {
            get { return _blockId; }
            private set { _blockId = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            Data = new RankOrgDataRec();

            byte response = br.ReadByte();
            int id = br.ReadInt32();
            byte status = br.ReadByte();
            long blockIdLong = br.ReadInt64();
            string blockid = Convert.ToString(blockIdLong);
            if (blockid.StartsWith("91"))
                BlockId = blockid.Substring(2, blockid.Length - 2);
            else
                BlockId = blockid;
            int num = br.ReadInt32();

            RankType type;
            int unicode = 0;
            string code = string.Empty;
            Dictionary<FieldIndex, object> memData;

            for (int i = 0; i < num; i++)
            {
                type = (RankType)br.ReadInt32();
                long tmpUnicode = br.ReadInt64();

                if (tmpUnicode > 10000000000)
                    code = ConvertCodeOrg.ConvertLongToCode(tmpUnicode); //等会用ConvertCodeOrd替换
                else
                    unicode = Convert.ToInt32(tmpUnicode);

                if ((!string.IsNullOrEmpty(code)) && DetailData.EmCodeToUnicode.ContainsKey(code))
                    unicode = DetailData.EmCodeToUnicode[code];

                Dictionary<FieldIndex, float> fieldSingle;
                Dictionary<FieldIndex, double> fieldDouble;

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }
                if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
                {
                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                    DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
                }


                fieldSingle[FieldIndex.Now] = br.ReadSingle();
                switch (type)
                {
                    case RankType.Amount:
                        fieldDouble[FieldIndex.Amount] = br.ReadDouble();
                        break;
                    case RankType.WeiBiTop:
                        fieldSingle[FieldIndex.Weibi] = (float)br.ReadDouble();
                        break;
                    case RankType.VolumeRatio:
                        fieldSingle[FieldIndex.VolumeRatio] = (float)br.ReadDouble();
                        break;
                    case RankType.DiffRangeBottom5Mint:
                    case RankType.DiffRangeTop5Mint:
                        fieldSingle[FieldIndex.DifferRange5Mint] = (float)br.ReadDouble();
                        break;
                    case RankType.Delta:
                        fieldSingle[FieldIndex.Delta] = (float)br.ReadDouble();
                        break;
                    case RankType.DiffRangeBottom:
                    case RankType.DiffRangeTop:
                        fieldSingle[FieldIndex.DifferRange] = (float)br.ReadDouble();
                        break;
                    case RankType.WeiBiBottom:
                        fieldSingle[FieldIndex.Weibi] = (float)br.ReadDouble();
                        break;
                }


                if (unicode != 0)
                {
                    if (Data.RankData.ContainsKey(type))
                        Data.RankData[type].Add(unicode);
                    else
                    {
                        List<int> tmp = new List<int>(10);
                        tmp.Add(unicode);
                        Data.RankData[type] = tmp;
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 资金流向排名
    /// </summary>
    public class ResNetInflowRankDataPacket : OrgDataPacket
    {
        private string _blockId;

        public string BlockId
        {
            get { return _blockId; }
            private set { _blockId = value; }
        }
        private List<int> _topStocks;

        public List<int> TopStocks
        {
            get { return _topStocks; }
            private set { _topStocks = value; }
        }
        private List<int> _bottomStocks;

        public List<int> BottomStocks
        {
            get { return _bottomStocks; }
            private set { _bottomStocks = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            byte bResponse = br.ReadByte();
            int id = br.ReadInt32();
            byte PacketStatus = br.ReadByte(); //0中间，1开始，2末尾，3未知
            BlockId = Convert.ToString(br.ReadInt64());
            int nStock = br.ReadInt32(); //股票数

            TopStocks = new List<int>(nStock / 2);
            BottomStocks = new List<int>(nStock / 2);

            byte nField = br.ReadByte(); //栏位个数
            br.ReadBytes(nField * 2);

            long buyBig = 0, sellBig = 0, buySuper = 0, sellSuper = 0;
            for (int i = 0; i < nStock; i++)
            {
                long biaoshi = br.ReadInt64();
                long codeId = br.ReadInt64();
                int unicode = 0;
                string code = string.Empty;
                if (codeId > 10000000000)
                    code = ConvertCodeOrg.ConvertLongToCode(codeId); //等会用ConvertCodeOrd替换
                else
                    unicode = Convert.ToInt32(codeId);
                if ((!string.IsNullOrEmpty(code)) && DetailData.EmCodeToUnicode.ContainsKey(code))
                    unicode = DetailData.EmCodeToUnicode[code];

                Dictionary<FieldIndex, int> fieldInt32;
                Dictionary<FieldIndex, float> fieldSingle;

                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }


                buyBig = br.ReadInt64();
                sellBig = br.ReadInt64();
                buySuper = br.ReadInt64();
                sellSuper = br.ReadInt64();
                fieldInt32[FieldIndex.BuyBig] = (int)buyBig;
                fieldInt32[FieldIndex.SellBig] = (int)sellBig;
                fieldInt32[FieldIndex.BuySuper] = (int)buySuper;
                fieldInt32[FieldIndex.SellSuper] = (int)sellSuper;
                fieldInt32[FieldIndex.MainNetFlow] = Convert.ToInt32((buyBig - sellBig) + (buySuper - sellSuper));
                fieldSingle[FieldIndex.ZengCangRange] = br.ReadSingle();
                fieldSingle[FieldIndex.ZengCangRangeDay3] = br.ReadSingle();
                fieldSingle[FieldIndex.ZengCangRangeDay5] = br.ReadSingle();
                if (i < nStock / 2)
                    TopStocks.Add(unicode);
                else
                    BottomStocks.Add(unicode);
            }
            return true;
        }
    }

    /// <summary>
    /// 指数静态数据
    /// </summary>
    public class ResIndexStaticOrgDataPacket : OrgDataPacket
    {
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }

        protected override bool DecodingBody(BinaryReader br)
        {

            long tmpUnicode = br.ReadInt64();
            string codeStr = string.Empty;
            if (tmpUnicode > 10000000000)
                codeStr = ConvertCodeOrg.ConvertLongToCode(tmpUnicode); //等会用ConvertCodeOrd替换
            else
                Code = Convert.ToInt32(tmpUnicode);

            if ((!string.IsNullOrEmpty(codeStr)) && DetailData.EmCodeToUnicode.ContainsKey(codeStr))
                Code = DetailData.EmCodeToUnicode[codeStr];

            Dictionary<FieldIndex, float> fieldSingle;
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            fieldSingle[FieldIndex.PreCloseDay5] = br.ReadSingle();
            fieldSingle[FieldIndex.PreCloseDay20] = br.ReadSingle();
            fieldSingle[FieldIndex.PreCloseDay60] = br.ReadSingle();
            fieldSingle[FieldIndex.PreCloseDayYTD] = br.ReadSingle();
            fieldSingle[FieldIndex.HighW52] = br.ReadSingle();
            fieldSingle[FieldIndex.LowW52] = br.ReadSingle();
            return true;
        }
    }

    /// <summary>
    /// LME成交明细
    /// </summary>
    public class ResOSFuturesLMEDealDataPacket : OrgDataPacket
    {
        public OneStockDealDataRec OneStockDealDatas;

        public ResOSFuturesLMEDealDataPacket()
        {
            OneStockDealDatas = new OneStockDealDataRec();
        }

        protected override bool DecodingBody(BinaryReader br)
        {
            OneStockDealDatas.Code = (int)(br.ReadInt64());
            int count = br.ReadInt32();
            OneStockDealDatas.DealDatas = new List<OneDealDataRec>(count);
            for (int i = 0; i < count; i++)
            {
                OneDealDataRec oneData = new OneDealDataRec();
                int time = br.ReadInt32();
                oneData.Hour = (byte)(time / 10000);
                oneData.Mint = (byte)((time % 10000) / 100);
                oneData.Second = (byte)(time % 100);
                int date = br.ReadInt32();
                oneData.Price = br.ReadSingle();//四位小数
                oneData.Volume = br.ReadInt64();
                oneData.Flag = br.ReadByte();
                oneData.UId = br.ReadInt32();
                int reserve = br.ReadInt32();
                float reserveFloatField = br.ReadSingle();
                OneStockDealDatas.DealDatas.Add(oneData);
            }
            return true;
        }
    }

    /// <summary>
    /// 低频分笔交易响应包
    /// </summary>
    public class ResLowFrequencyTBYDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public List<OneDealDataRec> Details;
        /// <summary>
        /// 股票代码（内码）
        /// </summary>
        public int Code;
        /// <summary>
        /// 数据个数
        /// </summary>
        public int DataNum;

        /// <summary>
        /// 低频分笔交易请求包
        /// </summary>
        public ResLowFrequencyTBYDataPacket()
        {
            Details = new List<OneDealDataRec>();
        }
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)(br.ReadInt64());
            DataNum = br.ReadInt32();
            if (Details != null)
                Details.Capacity = DataNum;

            OneBondDealDataRec item;
            for (int i = 0; i < DataNum; i++)
            {
                item = new OneBondDealDataRec();
                int time = br.ReadInt32();
                item.Second = (byte)(time % 100);
                item.Mint = (byte)((time % 10000 - item.Second) / 100);
                item.Hour = (byte)((time - item.Hour * 100 - item.Second) / 10000);

                int date = br.ReadInt32();
                item.Price = br.ReadSingle();//四位小数
                item.Volume = br.ReadInt64();
                item.Flag = br.ReadByte();
                item.UId = br.ReadInt32();
                int reserveField = br.ReadInt32();
                item.BondYtm = br.ReadSingle();

                this.Details.Add(item);
            }
            return true;
        }
    }

    /// <summary>
    /// 银行间债券报价明细-响应包
    /// </summary>
    public class ResBankBondReportDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public List<BidDetail> Details;
        /// <summary>
        /// 股票代码（内码）
        /// </summary>
        public int Code;
        /// <summary>
        /// 数据个数
        /// </summary>
        public int DataNum;

        /// <summary>
        /// 低频分笔交易请求包
        /// </summary>
        public ResBankBondReportDataPacket()
        {
            Details = new List<BidDetail>();
        }
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)(br.ReadInt64());
            DataNum = br.ReadInt32();
            if (Details != null)
                Details.Capacity = DataNum;

            BidDetail item;
            short strLength;
            for (int i = 0; i < DataNum; i++)
            {
                item = new BidDetail();
                item.Id = br.ReadInt64();
                item.Time = br.ReadInt32();
                item.Date = br.ReadInt32();

                strLength = br.ReadInt16();
                item.BidInitiator = Encoding.GetEncoding("GBK").GetString(br.ReadBytes(strLength));
                item.BidCleanPrice = br.ReadSingle();
                item.BidYield = br.ReadSingle();
                item.BidSettlementSpeed = br.ReadInt32();
                item.BidTotalFaceValue = br.ReadDouble();

                item.DeltaCleanPrice = br.ReadSingle();
                item.DeltaYield = br.ReadSingle();

                strLength = br.ReadInt16();
                item.OfferInitiator = Encoding.GetEncoding("GBK").GetString(br.ReadBytes(strLength));
                item.OfferCleanPrice = br.ReadSingle();
                item.OfferYield = br.ReadSingle();
                item.OfferSettlementSpeed = br.ReadInt32();
                item.OfferTotalFaceValue = br.ReadDouble();

                this.Details.Add(item);
            }
            return true;
        }
    }

    /// <summary>
    /// SHIBOR报价行明细-响应包
    /// </summary>
    public class ResShiborReportDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public List<OfferBankDetail> Details;
        /// <summary>
        /// 股票代码（内码）
        /// </summary>
        public int Code;
        /// <summary>
        /// 数据个数
        /// </summary>
        public int DataNum;

        /// <summary>
        /// 低频分笔交易请求包
        /// </summary>
        public ResShiborReportDataPacket()
        {
            Details = new List<OfferBankDetail>();
        }
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected override bool DecodingBody(BinaryReader br)
        {
            Code = (int)(br.ReadInt64());
            DataNum = br.ReadInt32();
            if (Details != null)
                Details.Capacity = DataNum;

            OfferBankDetail item;
            short offerBanlLen;
            for (int i = 0; i < DataNum; i++)
            {
                item = new OfferBankDetail();

                item.Time = br.ReadInt32();
                item.Date = br.ReadInt32();
                item.Price = br.ReadSingle();
                item.BP = br.ReadSingle();
                offerBanlLen = br.ReadInt16();
                item.OfferBank = Encoding.GetEncoding("GBK").GetString(br.ReadBytes(offerBanlLen));

                this.Details.Add(item);
            }
            return true;
        }
    }
    /// <summary>
    /// 个股盈利预测(高频)-发送包
    /// </summary>
    public class ResNewProfitForecastDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 预测数据条目数
        /// </summary>
        private const int RecCount = 3;
        /// <summary>
        /// 盈利预测数据（一个股票的）
        /// </summary>
        public OneProfitForecastOrgDataRec ProfitForecastDataByOneCode;
        /// <summary>
        /// 股票代码（内码）
        /// </summary>
        public int Code;


        /// <summary>
        /// 个股盈利预测(高频)-发送包
        /// </summary>
        public ResNewProfitForecastDataPacket()
        {
            ProfitForecastDataByOneCode = new OneProfitForecastOrgDataRec();
        }
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected override bool DecodingBody(BinaryReader br)
        {
            long sid = br.ReadInt64();
            string emcode = ConvertCodeOrg.ConvertLongToCode(sid);
            if (!DetailData.EmCodeToUnicode.TryGetValue(emcode, out Code))
                return false;
            ProfitForecastDataByOneCode.Code = Code;
            ProfitForecastDataByOneCode.ProfitForecastData = new List<ProfitForecast>(RecCount);
            ProfitForecast item;
            List<ProfitForecast> tempResult = new List<ProfitForecast>(RecCount);
            for (int i = 0; i < RecCount; i++)
            {
                item = new ProfitForecast();
                item.PredictYear = br.ReadInt32();
                item.BasicEPS = br.ReadSingle();

                tempResult.Add(item);
            }
            tempResult.Reverse();
            ProfitForecastDataByOneCode.ProfitForecastData.AddRange(tempResult);
            return true;
        }
    }

    /// <summary>
    /// 股票中文名称变更
    /// </summary>
    public class ResChangeNameDataPacket : OrgDataPacket
    {
        /// <summary>
        /// Decoding.
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected override bool DecodingBody(BinaryReader br)
        {
            int num = br.ReadInt32();
            long sid;
            byte newnameLen = 0;
            byte newPYLen = 0;
            string emCode = string.Empty;//新旧emcode一样

            string newName = string.Empty;
            string newPY = string.Empty;

            string oldName = string.Empty;
            string oldPY = string.Empty;

            int unicode = 0;
            string code = string.Empty;
            Dictionary<FieldIndex, string> fieldString;


            if (num > 0)
            {

                StringBuilder sendSecuAsstInfo = new StringBuilder(100);
                StringBuilder sendBlockInfo = new StringBuilder(100);
                for (int i = 0; i < num; i++)
                {
                    // Get stock unicode.
                    sid = br.ReadInt64();


                    newnameLen = br.ReadByte();
                    newName = Encoding.GetEncoding("GBK").GetString(br.ReadBytes(newnameLen));

                    newPYLen = br.ReadByte();
                    newPY = Encoding.GetEncoding("GBK").GetString(br.ReadBytes(newPYLen));


                    if (sid > 10000000000)
                        code = ConvertCodeOrg.ConvertLongToCode(sid); //等会用ConvertCodeOrd替换
                    else
                        unicode = Convert.ToInt32(sid);
                    if ((!string.IsNullOrEmpty(code)) && DetailData.EmCodeToUnicode.ContainsKey(code))
                        unicode = DetailData.EmCodeToUnicode[code];

                    if (DetailData.FieldIndexDataString.TryGetValue(unicode, out fieldString)
                        && fieldString.TryGetValue(FieldIndex.EMCode, out emCode))
                    {


                        fieldString[FieldIndex.Name] = newName;
                        sendSecuAsstInfo.AppendFormat("{0}~{1}${2}", emCode, newName, newPY);
                        sendSecuAsstInfo.Append("}");

                        sendBlockInfo.AppendFormat("{0}~{1}", emCode, newName);
                        sendBlockInfo.Append("}");
                    }
                }


                UpdateBlockInfo(sendBlockInfo.ToString(), sendSecuAsstInfo.ToString());
                sendBlockInfo.Length = 0;
                sendSecuAsstInfo.Length = 0;

            }
            else
                UpdateBlockInfo(string.Empty, string.Empty);

            return true;
        }



        /// <summary>
        /// 更新板块和键盘精灵
        /// </summary>
        /// <param name="BlockServiceInfo"></param>
        /// <param name="SecuAsstInfo"></param>
        private void UpdateBlockInfo(string blockServiceInfo, string secuAsstInfo)
        {
        }

    }
    #endregion

    #region 外盘

    /// <summary>
    /// 外盘登陆
    /// </summary>
    public class ResLogonDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            //byte nVersion = br.ReadByte();														// 主版本号
            //byte nMinorVersion = br.ReadByte();														// 次版本号
            //byte nRevision = br.ReadByte();															// 修订版本号
            //long nBeginDate = br.ReadInt32();														// 用户有效开始日期
            //long nEndDate = br.ReadInt32();														// 用户有效结束日期
            //long nUserType = br.ReadInt32();														// 用户类型
            //int nUserId = br.ReadInt32();														// 用户内部id号
            //byte byteRight = br.ReadByte();														// 用户使用权利
            //short wDummy = br.ReadInt16();
            //byte[] szDHPubKey = br.ReadBytes(8);
            //// DHPubKey
            //int dwServerIpOuter = br.ReadInt32();
            //int iQuoteDate = br.ReadInt32();													// 最新行情日期 yyyyMMdd
            //int iQuoteTime = br.ReadInt32();	// 最新行情时间 HHmmss
            //byte[] sCodeListMD5 = br.ReadBytes(33);// 码表的MD5摘要
            return true;
        }


    }


    /// <summary>
    /// 外盘心跳
    /// </summary>
    public class ResOceanHeart : RealTimeDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        private int _date;

        /// <summary>
        /// 时间
        /// </summary>
        public int Date
        {
            get { return _date; }
            private set { _date = value; }
        }

        private int _time;

        public int Time
        {
            get { return _time; }
            private set { _time = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(System.IO.BinaryReader br)
        {
            Date = br.ReadInt32();
            Time = br.ReadInt32();
            return true;
        }
    }

    /// <summary>
    /// 外盘行情记录
    /// </summary>
    public class ResOceanRecordDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        private int _code;

        public int Code
        {
            get { return _code; }
            private set { _code = value; }
        }


        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            int cTime = br.ReadInt32();
            int time = CTimeToDateTimeInt(cTime);

            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');

            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;
            Code = DetailData.EmCodeToUnicode[emCode];

            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(Code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[Code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(Code, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[Code] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(Code, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[Code] = fieldDouble;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(Code, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[Code] = fieldSingle;
            }

            fieldInt32[FieldIndex.Time] = time;


            byte[] nameByte = br.ReadBytes(9);
            byte type = br.ReadByte();
            float price = 0;
            float diff = 0;
            float diffRange = 0;
            float preClose = 0;

            preClose = br.ReadSingle();
            fieldSingle[FieldIndex.PreClose] = preClose;
            fieldSingle[FieldIndex.PreSettlementPrice] = preClose;
            fieldSingle[FieldIndex.Open] = br.ReadSingle();
            fieldSingle[FieldIndex.High] = br.ReadSingle();
            fieldSingle[FieldIndex.Low] = br.ReadSingle();
            price = br.ReadSingle();
            fieldSingle[FieldIndex.Now] = price;
            fieldDouble[FieldIndex.Amount] = br.ReadDouble();
            fieldInt64[FieldIndex.Volume] = br.ReadUInt32();
            if ((fieldInt64[FieldIndex.Volume]) != 0)
                fieldSingle[FieldIndex.AveragePrice] = Convert.ToSingle((fieldDouble[FieldIndex.Amount]) /
                                    (fieldInt64[FieldIndex.Volume]));
            else
            {
                fieldSingle[FieldIndex.AveragePrice] = 0.0f;
            }
            fieldSingle[FieldIndex.BuyPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume1] = br.ReadInt32();
            fieldSingle[FieldIndex.BuyPrice2] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume2] = br.ReadInt32();
            fieldSingle[FieldIndex.BuyPrice3] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume3] = br.ReadInt32();
            fieldSingle[FieldIndex.BuyPrice4] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume4] = br.ReadInt32();
            fieldSingle[FieldIndex.BuyPrice5] = br.ReadSingle();
            fieldInt32[FieldIndex.BuyVolume5] = br.ReadInt32();
            fieldSingle[FieldIndex.SellPrice1] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume1] = br.ReadInt32();
            fieldSingle[FieldIndex.SellPrice2] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume2] = br.ReadInt32();
            fieldSingle[FieldIndex.SellPrice3] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume3] = br.ReadInt32();
            fieldSingle[FieldIndex.SellPrice4] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume4] = br.ReadInt32();
            fieldSingle[FieldIndex.SellPrice5] = br.ReadSingle();
            fieldInt32[FieldIndex.SellVolume5] = br.ReadInt32();
            diff = br.ReadSingle();
            diffRange = br.ReadSingle();
            if (price != 0 && preClose != 0)
            {
                fieldSingle[FieldIndex.Difference] = price - preClose;
                fieldSingle[FieldIndex.DifferRange] = (price - preClose) / preClose;
            }
            fieldSingle[FieldIndex.Delta] = br.ReadSingle() / 100.0f;
            fieldSingle[FieldIndex.Turnover] = br.ReadSingle() / 100.0f;
            fieldSingle[FieldIndex.PE] = br.ReadSingle();
            fieldSingle[FieldIndex.VolumeRatio] = br.ReadSingle();
            fieldInt64[FieldIndex.Evg5Volume] = br.ReadInt32();
            fieldInt64[FieldIndex.LastVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.GreenVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.RedVolume] = br.ReadInt32();
            fieldInt64[FieldIndex.Ltg] = (long)br.ReadSingle();
            fieldSingle[FieldIndex.DifferRange3D] = br.ReadSingle();
            fieldSingle[FieldIndex.DifferRange6D] = br.ReadSingle();
            fieldSingle[FieldIndex.Turnover3D] = br.ReadSingle();
            fieldSingle[FieldIndex.Turnover6D] = br.ReadSingle();
            return true;
        }
    }

    /// <summary>
    /// 外盘走势
    /// </summary>
    public class ResOceanTrendDataPacket : RealTimeDataPacket
    {
        //public string Code { get; private set; }
        //public Dictionary<FieldIndex, object> PacketDetailData { get; private set; }
        /// <summary>
        /// 外盘走势数据
        /// </summary>
        private OneDayTrendDataRec _trendData;

        public OneDayTrendDataRec TrendData
        {
            get { return _trendData; }
            private set { _trendData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private short _offset;

        public short Offset
        {
            get { return _offset; }
            private set { _offset = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private short _minNum;

        public short MinNum
        {
            get { return _minNum; }
            private set { _minNum = value; }
        }

        /// <summary>
        /// 从流中读取数据
        /// </summary>
        protected override bool DecodingBody(BinaryReader br)
        {
            int date1 = br.ReadInt32();
            int time1 = br.ReadInt32();
            Offset = br.ReadInt16();
            MinNum = br.ReadInt16();

            /////行情记录开始
            byte market = br.ReadByte();
            int cTime = br.ReadInt32();
            //int time = CTimeToDateTimeInt(cTime);

            byte[] codeBytes = br.ReadBytes(7);
            string code = Encoding.ASCII.GetString(codeBytes);
            code = code.TrimEnd('\0');

            string emCode = GetEmCode((ReqMarketType)market, code);
            if (emCode == null || !DetailData.EmCodeToUnicode.ContainsKey(emCode))
                return false;

            TrendData = new OneDayTrendDataRec(DetailData.EmCodeToUnicode[emCode]);
            TrendData.Code = DetailData.EmCodeToUnicode[emCode];
            TrendData.Date = date1;
            TrendData.Time = time1;

            byte[] nameByte = br.ReadBytes(9);
            byte type = br.ReadByte();

            TrendData.PreClose = br.ReadSingle();
            TrendData.Open = br.ReadSingle();
            TrendData.High = br.ReadSingle();
            TrendData.Low = br.ReadSingle();
            br.ReadBytes(156);
            ////行情记录结束

            bool bMinData = br.ReadBoolean();
            if (bMinData) //longMinData
            {
                for (int i = 0; i < MinNum; i++)
                {
                    TrendData.MintDatas[i].Price = br.ReadSingle();
                    TrendData.MintDatas[i].Volume = br.ReadInt32();
                    TrendData.MintDatas[i].Amount = br.ReadDouble();
                    if (TrendData.MintDatas[i].Volume != 0)
                        TrendData.MintDatas[i].AverPrice =
                            Convert.ToSingle((TrendData.MintDatas[i].Amount / TrendData.MintDatas[i].Volume));
                    TrendData.MintDatas[i].BuyVolume = br.ReadInt32();
                    TrendData.MintDatas[i].SellVolume = br.ReadInt32();
                    //TrendData.MintDatas[i].NeiWaiCha = br.ReadInt32() - br.ReadInt32();
                }
            }
            else //shortMinData
            {
                for (int i = 0; i < MinNum; i++)
                {
                    //TrendData.MintDatas[i].Price = br.ReadSingle();
                    //TrendData.MintDatas[i].Volume = br.ReadInt32();
                    //TrendData.MintDatas[i].Amount = br.ReadDouble();
                    //if (TrendData.MintDatas[i].Volume != 0)
                    //    TrendData.MintDatas[i].AverPrice =
                    //        Convert.ToSingle((TrendData.MintDatas[i].Amount/TrendData.MintDatas[i].Volume));
                    TrendData.MintDatas[i].BuyVolume = br.ReadInt32();
                    TrendData.MintDatas[i].SellVolume = br.ReadInt32();

                }
            }

            return true;
        }
    }

    #endregion

    #region 机构资讯


    /// <summary>
    /// 新闻列表
    /// </summary>
    public class ResInfoOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private List<int> _codes;

        public List<int> Codes
        {
            get { return _codes; }
            private set { _codes = value; }
        }
        /// <summary>
        /// 资讯内容
        /// </summary>
        private Dictionary<int, InfoMineOrgDataRec> _infoMineDatas;

        public Dictionary<int, InfoMineOrgDataRec> InfoMineDatas
        {
            get { return _infoMineDatas; }
            set { _infoMineDatas = value; }
        }

        public ResInfoOrgDataPacket()
        {
            Codes = new List<int>();
            InfoMineDatas = new Dictionary<int, InfoMineOrgDataRec>();
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        public override bool Decoding(BinaryReader br)
        {
            try
            {
                ushort num = br.ReadUInt16();

                int currentCode = 0;
                InfoMineOrgDataRec currentData = null;

                for (int j = 0; j < num; j++)
                {
                    currentCode = br.ReadInt32();

                    OneInfoMineOrgDataRec oneInfoMineData = new OneInfoMineOrgDataRec();
                    oneInfoMineData.Code = currentCode;
                    short lenInfoCode = br.ReadInt16();
                    if (lenInfoCode == 0)
                        oneInfoMineData.InfoCode = string.Empty;
                    else
                        oneInfoMineData.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));

                    short lenTitle = br.ReadInt16();
                    if (lenTitle == 0)
                        oneInfoMineData.Title = string.Empty;
                    else
                        oneInfoMineData.Title = Encoding.UTF8.GetString(br.ReadBytes(lenTitle));

                    oneInfoMineData.PublishDate = br.ReadInt32();
                    oneInfoMineData.PublishTime = br.ReadInt32();
                    oneInfoMineData.InfoType = (InfoMineOrg)br.ReadByte();
                    if (oneInfoMineData.InfoType == InfoMineOrg.News || oneInfoMineData.InfoType == InfoMineOrg.Report)
                    {
                        short lenMediaName = br.ReadInt16();
                        string mediaNameStr = Encoding.UTF8.GetString(br.ReadBytes(lenMediaName));
                        if (mediaNameStr == null)
                            oneInfoMineData.MediaName = string.Empty;
                        else
                            oneInfoMineData.MediaName = mediaNameStr;
                    }
                    if (oneInfoMineData.InfoType == InfoMineOrg.NewsAndTip
                        || oneInfoMineData.InfoType == InfoMineOrg.Tip)
                    {
                        oneInfoMineData.IsTop = br.ReadBoolean();
                    }
                    if (oneInfoMineData.InfoType == InfoMineOrg.Report)
                    {
                        oneInfoMineData.Star = br.ReadByte();
                        oneInfoMineData.EmratingValue = (EmratingValue)br.ReadByte();
                    }
                    switch (oneInfoMineData.InfoType)
                    {
                        case InfoMineOrg.Tip:
                        case InfoMineOrg.News:
                            oneInfoMineData.Url = NewsUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Notice:
                            oneInfoMineData.Url = NoticeUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Report:
                            oneInfoMineData.Url = ReportUrlHead + oneInfoMineData.InfoCode;
                            break;
                    }

                    if (InfoMineDatas.ContainsKey(currentCode))
                    {
                        currentData = InfoMineDatas[currentCode];
                    }
                    else
                    {
                        Codes.Add(currentCode);
                        currentData = new InfoMineOrgDataRec();
                        currentData.Code = currentCode;
                        InfoMineDatas.Add(currentCode, currentData);
                    }

                    if (currentData.InfoMineData.ContainsKey(oneInfoMineData.InfoType))
                        currentData.InfoMineData[oneInfoMineData.InfoType].Add(oneInfoMineData);
                    else
                    {
                        List<OneInfoMineOrgDataRec> list = new List<OneInfoMineOrgDataRec>();
                        list.Add(oneInfoMineData);
                        currentData.InfoMineData.Add(oneInfoMineData.InfoType, list);
                    }

                }
                LogUtilities.LogMessage("msgId=" + MsgId + "解包，解到数量=" + num);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 24小时新闻
    /// </summary>
    public class ResNews24HOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 24小时新闻数据
        /// </summary>
        private List<OneNews24HOrgDataRec> _news24HData;

        public List<OneNews24HOrgDataRec> News24HData
        {
            get { return _news24HData; }
            set { _news24HData = value; }
        }

        public override bool Decoding(BinaryReader br)
        {
            try
            {
                ushort num = br.ReadUInt16();
                News24HData = new List<OneNews24HOrgDataRec>(num);

                for (int i = 0; i < num; i++)
                {
                    OneNews24HOrgDataRec oneNews24h = new OneNews24HOrgDataRec();
                    short lenInfoCode = br.ReadInt16();
                    oneNews24h.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));
                    short lenTitle = br.ReadInt16();
                    oneNews24h.Title = Encoding.UTF8.GetString(br.ReadBytes(lenTitle));
                    oneNews24h.PublishDate = br.ReadInt32();
                    oneNews24h.PublishTime = br.ReadInt32();
                    short lenMediaName = br.ReadInt16();
                    oneNews24h.MediaName = Encoding.UTF8.GetString(br.ReadBytes(lenMediaName));
                    oneNews24h.Url = NewsUrlHead + oneNews24h.InfoCode;
                    News24HData.Add(oneNews24h);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("24小时新闻解包" + e.Message);
            }
            return true;
        }
    }

    /// <summary>
    /// 要闻精华
    /// </summary>
    public class ResImportantNewsDataPacket : ResNews24HOrgDataPacket
    {

    }

    /// <summary>
    /// 公司要闻
    /// </summary>
    public class ResNewsFlashDataPacket : ResNews24HOrgDataPacket
    {

    }

    /// <summary>
    /// 盈利预测
    /// </summary>
    public class ResProfitForecastOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 盈利预测数据
        /// </summary>
        public OneProfitForecastOrgDataRec ProfitForecastData;

        public override bool Decoding(BinaryReader br)
        {
            ProfitForecastData = new OneProfitForecastOrgDataRec();
            ProfitForecastData.Code = br.ReadInt32();
            short num = br.ReadInt16();
            ProfitForecastData.ProfitForecastData = new List<ProfitForecast>(num);
            for (int i = 0; i < num; i++)
            {
                ProfitForecast oneData = new ProfitForecast();
                oneData.PredictYear = br.ReadInt32();
                oneData.BasicEPS = br.ReadInt32() / 100.0f;
                ProfitForecastData.ProfitForecastData.Add(oneData);
            }
            return true;
        }
    }

    /// <summary>
    /// 机构评级
    /// </summary>
    public class ResInfoRateOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 盈利预测数据
        /// </summary>
        public List<OneInfoRateOrgItem> InfoRateOrgList;

        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        public override bool Decoding(BinaryReader br)
        {
            ushort count = br.ReadUInt16();
            InfoRateOrgList = new List<OneInfoRateOrgItem>(count);

            for (int i = 0; i < count; i++)
            {
                Code = br.ReadInt32();
                OneInfoRateOrgItem item = new OneInfoRateOrgItem();
                short lenInfoCode = br.ReadInt16();
                item.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));
                short lenTitle = br.ReadInt16();
                item.Title = Encoding.UTF8.GetString(br.ReadBytes(lenTitle));
                item.Date = br.ReadInt32();
                item.Time = br.ReadInt32();
                item.TypeCode = br.ReadByte();
                short lenMediaName = br.ReadInt16();
                item.MediaName = Encoding.UTF8.GetString(br.ReadBytes(lenMediaName));
                item.Star = br.ReadByte();
                item.EmratingValue = (EmratingValue)br.ReadByte();
                item.Url = ReportUrlHead + item.InfoCode;
                InfoRateOrgList.Add(item);
            }

            return true;
        }
    }
    /// <summary>
    ///  研究报告(机构评级)
    /// </summary>
    public class ResResearchReportOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 盈利预测数据
        /// </summary>
        public List<ResearchReportItem> InfoResearchReportList;

        public override bool Decoding(BinaryReader br)
        {
            //RequestId = (FuncTypeInfoOrg)br.ReadByte();
            ushort count = br.ReadUInt16();

            InfoResearchReportList = new List<ResearchReportItem>(count);

            for (int i = 0; i < count; i++)
            {
                ResearchReportItem item = new ResearchReportItem();

                short lenInfoCode = br.ReadInt16();
                item.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));

                short lenSecuCode = br.ReadInt16();
                item.SecuCode = Encoding.UTF8.GetString(br.ReadBytes(lenSecuCode));


                short lenSecuSName = br.ReadInt16();
                item.SecuSName = Encoding.UTF8.GetString(br.ReadBytes(lenSecuSName));

                item.SecuVarietyCode = br.ReadInt32();

                item.EmratingValue = (EmratingValue)br.ReadByte();

                short lenInsSName = br.ReadInt16();
                item.InsSName = Encoding.UTF8.GetString(br.ReadBytes(lenInsSName));

                item.Date = br.ReadInt32();
                item.Time = br.ReadInt32();

                InfoResearchReportList.Add(item);
            }

            return true;
        }
    }

    /// <summary>
    /// 8. 新闻列表
    /// </summary>
    public class ResNewInfoOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        private List<int> _codes;

        public List<int> Codes
        {
            get { return _codes; }
            private set { _codes = value; }
        }
        /// <summary>
        /// 资讯内容
        /// </summary>
        private Dictionary<int, InfoMineOrgDataRec> _infoMineDatas;

        public Dictionary<int, InfoMineOrgDataRec> InfoMineDatas
        {
            get { return _infoMineDatas; }
            set { _infoMineDatas = value; }
        }
        /// <summary>
        /// 新资讯列表请求
        /// </summary>
        public ResNewInfoOrgDataPacket()
        {
            Codes = new List<int>();
            InfoMineDatas = new Dictionary<int, InfoMineOrgDataRec>();
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        public override bool Decoding(BinaryReader br)
        {
            try
            {
                ushort num = br.ReadUInt16();

                int currentCode = 0;
                InfoMineOrgDataRec currentData = null;

                for (int j = 0; j < num; j++)
                {
                    currentCode = br.ReadInt32();

                    OneInfoMineOrgDataRec oneInfoMineData = new OneInfoMineOrgDataRec();
                    oneInfoMineData.Code = currentCode;
                    short lenInfoCode = br.ReadInt16();
                    if (lenInfoCode == 0)
                        oneInfoMineData.InfoCode = string.Empty;
                    else
                        oneInfoMineData.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));

                    short lenTitle = br.ReadInt16();
                    if (lenTitle == 0)
                        oneInfoMineData.Title = string.Empty;
                    else
                        oneInfoMineData.Title = Encoding.UTF8.GetString(br.ReadBytes(lenTitle));

                    oneInfoMineData.PublishDate = br.ReadInt32();
                    oneInfoMineData.PublishTime = br.ReadInt32();
                    oneInfoMineData.InfoType = (InfoMineOrg)br.ReadByte();
                    if (oneInfoMineData.InfoType == InfoMineOrg.News || oneInfoMineData.InfoType == InfoMineOrg.Report)
                    {
                        short lenMediaName = br.ReadInt16();
                        string mediaNameStr = Encoding.UTF8.GetString(br.ReadBytes(lenMediaName));
                        if (mediaNameStr == null)
                            oneInfoMineData.MediaName = string.Empty;
                        else
                            oneInfoMineData.MediaName = mediaNameStr;
                    }
                    if (oneInfoMineData.InfoType == InfoMineOrg.NewsAndTip
                        || oneInfoMineData.InfoType == InfoMineOrg.Tip)
                    {
                        oneInfoMineData.IsTop = br.ReadBoolean();
                    }
                    if (oneInfoMineData.InfoType == InfoMineOrg.Report)
                    {
                        oneInfoMineData.Star = br.ReadByte();
                        oneInfoMineData.EmratingValue = (EmratingValue)br.ReadByte();
                    }
                    switch (oneInfoMineData.InfoType)
                    {
                        case InfoMineOrg.Tip:
                        case InfoMineOrg.News:
                            oneInfoMineData.Url = NewsUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Notice:
                            oneInfoMineData.Url = NoticeUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Report:
                            oneInfoMineData.Url = ReportUrlHead + oneInfoMineData.InfoCode;
                            break;
                    }

                    if (InfoMineDatas.ContainsKey(currentCode))
                    {
                        currentData = InfoMineDatas[currentCode];
                    }
                    else
                    {
                        Codes.Add(currentCode);
                        currentData = new InfoMineOrgDataRec();
                        currentData.Code = currentCode;
                        InfoMineDatas.Add(currentCode, currentData);
                    }

                    if (currentData.InfoMineData.ContainsKey(oneInfoMineData.InfoType))
                        currentData.InfoMineData[oneInfoMineData.InfoType].Add(oneInfoMineData);
                    else
                    {
                        List<OneInfoMineOrgDataRec> list = new List<OneInfoMineOrgDataRec>();
                        list.Add(oneInfoMineData);
                        currentData.InfoMineData.Add(oneInfoMineData.InfoType, list);
                    }

                }
                LogUtilities.LogMessage("msgId=" + MsgId + "解包，解到数量=" + num);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 9.	根据列表id取资讯请求
    /// </summary>
    public class ResInfoOrgByIdsDataPacket : InfoOrgBaseDataPacket
    {

        private List<int> _codes;
        /// <summary>
        /// 大类
        ///新闻：1
        ///研报：3
        /// </summary>
        public InfoMineOrg TypeLevel1;
        /// <summary>
        /// 小类，比如F009，S001001
        /// </summary>
        public string TypeLevel2;
        /// <summary>
        ///返回格式 （暂时只对研报有效）
        ///1：个股的返回类型
        ///2：全景图的返回类型
        /// </summary>
        public ReportType ReturnType;
        private Dictionary<string, List<OneInfoMineOrgDataRec>> _dicNewsInfoByBlock;//按照小类存储新闻结构
        private Dictionary<string, List<OneInfoMineOrgDataRec>> _dicNewsReportInfoByBlock;// 按照小类存储新闻类研报结构（returnType=1）
        private Dictionary<string, List<ResearchReportItem>> _dicEmratingReportInfoByBlock;// 按照小类存储评估类研报结构（returnType=2）

        /// <summary>
        /// 内码
        /// </summary>
        public List<int> Codes
        {
            get { return _codes; }
            private set { _codes = value; }
        }
        /// <summary>
        /// 按照小类存储新闻结构(key: 小类代码(板块代码)； value：对应的新闻结构列表)
        /// </summary>
        public Dictionary<string, List<OneInfoMineOrgDataRec>> DicNewsInfoByBlock
        {
            get { return _dicNewsInfoByBlock; }
            set { _dicNewsInfoByBlock = value; }
        }
        /// <summary>
        /// 按照小类存储新闻结构(key: 小类代码(板块代码)； value：对应的新闻类研报列表)
        /// </summary>
        public Dictionary<string, List<OneInfoMineOrgDataRec>> DicNewsReportInfoByBlock
        {
            get { return _dicNewsReportInfoByBlock; }
            set { _dicNewsReportInfoByBlock = value; }
        }
        /// <summary>
        /// 按照小类存储新闻结构(key: 小类代码(板块代码)； value：对应的评估类研报结构列表)
        /// </summary>
        public Dictionary<string, List<ResearchReportItem>> DicEmratingReportInfoByBlock
        {
            get { return _dicEmratingReportInfoByBlock; }
            set { _dicEmratingReportInfoByBlock = value; }
        }

        /// <summary>
        /// 新资讯列表请求
        /// </summary>
        public ResInfoOrgByIdsDataPacket()
        {
            Codes = new List<int>();
            DicNewsInfoByBlock = new Dictionary<string, List<OneInfoMineOrgDataRec>>();
            DicNewsReportInfoByBlock = new Dictionary<string, List<OneInfoMineOrgDataRec>>();
            DicEmratingReportInfoByBlock = new Dictionary<string, List<ResearchReportItem>>();
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        public override bool Decoding(BinaryReader br)
        {
            bool success = false;
            try
            {
                ushort num = br.ReadUInt16();
                TypeLevel1 = (InfoMineOrg)br.ReadByte();
                short level2Length = br.ReadInt16();
                TypeLevel2 = Encoding.UTF8.GetString(br.ReadBytes(level2Length));
                ReturnType = (ReportType)br.ReadByte();

                switch (TypeLevel1)
                {
                    case InfoMineOrg.News:
                        success = TryFillNews(TypeLevel2, num, br, out _dicNewsInfoByBlock);
                        break;

                    case InfoMineOrg.Report:
                        switch (ReturnType)
                        {
                            case ReportType.NewsReport:
                                success = TryFillNewsReport(TypeLevel2, num, br, out _dicNewsReportInfoByBlock);
                                break;

                            case ReportType.EmratingReport:
                                success = TryFillEmratingReport(TypeLevel2, num, br, out _dicEmratingReportInfoByBlock);
                                break;
                        }
                        break;

                    default:
                        success = false;
                        break;

                }

                LogUtilities.LogMessage("msgId=" + MsgId + "解包，解到数量=" + num);
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        private bool TryFillNewsReport(string _typeLevel2, ushort num,
            BinaryReader br, out Dictionary<string, List<OneInfoMineOrgDataRec>> dic)
        {
            dic = new Dictionary<string, List<OneInfoMineOrgDataRec>>();
            try
            {
                List<OneInfoMineOrgDataRec> recs = new List<OneInfoMineOrgDataRec>(num);

                OneInfoMineOrgDataRec oneInfoMineData;
                for (int j = 0; j < num; j++)
                {
                    oneInfoMineData = new OneInfoMineOrgDataRec();
                    oneInfoMineData.InfoType = InfoMineOrg.Report;

                    oneInfoMineData.Code = br.ReadInt32();

                    short lenInfoCode = br.ReadInt16();
                    if (lenInfoCode == 0)
                        oneInfoMineData.InfoCode = string.Empty;
                    else
                        oneInfoMineData.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));

                    short lenTitle = br.ReadInt16();
                    if (lenTitle == 0)
                        oneInfoMineData.Title = string.Empty;
                    else
                        oneInfoMineData.Title = Encoding.UTF8.GetString(br.ReadBytes(lenTitle));

                    oneInfoMineData.PublishDate = br.ReadInt32();
                    oneInfoMineData.PublishTime = br.ReadInt32();

                    oneInfoMineData.InfoType = (InfoMineOrg)br.ReadByte();//InfoMineOrg.Report;

                    short lenInsSName = br.ReadInt16();
                    string insSName = Encoding.UTF8.GetString(br.ReadBytes(lenInsSName));
                    //if (insSName == null)
                    //    oneInfoMineData.InsSName = string.Empty;
                    //else
                    //    oneInfoMineData.InsSName = insSName;

                    oneInfoMineData.Star = br.ReadByte();
                    oneInfoMineData.EmratingValue = (EmratingValue)br.ReadByte();
                    switch (oneInfoMineData.InfoType)
                    {
                        case InfoMineOrg.Tip:
                        case InfoMineOrg.News:
                            oneInfoMineData.Url = NewsUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Notice:
                            oneInfoMineData.Url = NoticeUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Report:
                            oneInfoMineData.Url = ReportUrlHead + oneInfoMineData.InfoCode;
                            break;
                    }

                    recs.Add(oneInfoMineData);
                }

                dic[_typeLevel2] = recs;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool TryFillEmratingReport(string _typeLevel2, ushort count,
            BinaryReader br, out Dictionary<string, List<ResearchReportItem>> dic)
        {
            dic = new Dictionary<string, List<ResearchReportItem>>();
            try
            {
                List<ResearchReportItem> recs = new List<ResearchReportItem>(count);

                ResearchReportItem item;


                for (int i = 0; i < count; i++)
                {
                    item = new ResearchReportItem();

                    short lenInfoCode = br.ReadInt16();
                    item.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));

                    short lenSecuCode = br.ReadInt16();
                    item.SecuCode = Encoding.UTF8.GetString(br.ReadBytes(lenSecuCode));

                    short lenSecuSName = br.ReadInt16();
                    item.SecuSName = Encoding.UTF8.GetString(br.ReadBytes(lenSecuSName));

                    item.SecuVarietyCode = br.ReadInt32();
                    item.EmratingValue = (EmratingValue)br.ReadByte();

                    short lenInsSName = br.ReadInt16();
                    item.InsSName = Encoding.UTF8.GetString(br.ReadBytes(lenInsSName));

                    item.Date = br.ReadInt32();
                    item.Time = br.ReadInt32();

                    recs.Add(item);
                }
                dic[_typeLevel2] = recs;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool TryFillNews(string _typeLevel2, int num,
            BinaryReader br, out  Dictionary<string, List<OneInfoMineOrgDataRec>> dic)
        {
            dic = new Dictionary<string, List<OneInfoMineOrgDataRec>>();
            try
            {
                List<OneInfoMineOrgDataRec> recs = new List<OneInfoMineOrgDataRec>(num);

                OneInfoMineOrgDataRec oneInfoMineData;
                for (int j = 0; j < num; j++)
                {
                    oneInfoMineData = new OneInfoMineOrgDataRec();
                    oneInfoMineData.InfoType = InfoMineOrg.News;

                    short lenInfoCode = br.ReadInt16();
                    if (lenInfoCode == 0)
                        oneInfoMineData.InfoCode = string.Empty;
                    else
                        oneInfoMineData.InfoCode = Encoding.UTF8.GetString(br.ReadBytes(lenInfoCode));

                    short lenTitle = br.ReadInt16();
                    if (lenTitle == 0)
                        oneInfoMineData.Title = string.Empty;
                    else
                        oneInfoMineData.Title = Encoding.UTF8.GetString(br.ReadBytes(lenTitle));

                    oneInfoMineData.PublishDate = br.ReadInt32();
                    oneInfoMineData.PublishTime = br.ReadInt32();

                    short lenMediaName = br.ReadInt16();
                    string mediaNameStr = Encoding.UTF8.GetString(br.ReadBytes(lenMediaName));
                    if (mediaNameStr == null)
                        oneInfoMineData.MediaName = string.Empty;
                    else
                        oneInfoMineData.MediaName = mediaNameStr;

                    switch (oneInfoMineData.InfoType)
                    {
                        case InfoMineOrg.Tip:
                        case InfoMineOrg.News:
                            oneInfoMineData.Url = NewsUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Notice:
                            oneInfoMineData.Url = NoticeUrlHead + oneInfoMineData.InfoCode;
                            break;
                        case InfoMineOrg.Report:
                            oneInfoMineData.Url = ReportUrlHead + oneInfoMineData.InfoCode;
                            break;
                    }

                    recs.Add(oneInfoMineData);
                }

                dic[_typeLevel2] = recs;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
    #endregion

    #region 宏观指标
    /// <summary>
    /// 宏观指标的单侧侧报表——返回包
    /// </summary>
    public class ResIndicatorsReportDataPacket : IndicatorDataPacket
    {
        public List<StockIndicatorLeftItem> LeftItems;
        public List<StockIndicatorRightItem> RightItems;

        /// <summary>
        /// 
        /// </summary>
        public ResIndicatorsReportDataPacket(string tableKeyCode)
        {
            this.TableKeyCode = tableKeyCode;
        }

        public override bool Decoding(DataTable dt)
        {
            switch (RequestId)
            {
                case IndicateRequestType.LeftIndicatorsReport:
                    LeftItems = GetLeftItems(dt);
                    break;
                case IndicateRequestType.RightIndicatorsReport:
                    RightItems = GetRightItems(dt);
                    break;

                case IndicateRequestType.IndicatorValuesReport:
                    break;
            }


            return true;
        }

        private List<StockIndicatorRightItem> GetRightItems(DataTable dt)
        {
            List<StockIndicatorRightItem> result = new List<StockIndicatorRightItem>();

            if (dt == null || dt.Rows.Count == 0)
                return new List<StockIndicatorRightItem>(0);

            result.Capacity = dt.Rows.Count;
            StockIndicatorRightItem item;

            foreach (DataRow row in dt.Rows)
            {
                item = new StockIndicatorRightItem();
                item.MacroId = row[0].ToString();
                item.MacroName = row[1].ToString();
                DateTime.TryParse(row[2].ToString(), out item.ReportDate);
                float.TryParse(row[3].ToString(), out item.Current);
                float.TryParse(row[4].ToString(), out item.Previous);
                float.TryParse(row[5].ToString(), out item.Central);
                float.TryParse(row[6].ToString(), out item.Compare);

                byte fre;
                if (byte.TryParse(row[7].ToString(), out fre))
                {
                    item.Frequency = (DateFrequency)fre;
                }
                bool.TryParse(row[8].ToString(), out item.IsCustomize);

                result.Add(item);
            }

            return result;
        }

        private List<StockIndicatorLeftItem> GetLeftItems(DataTable dt)
        {
            List<StockIndicatorLeftItem> result = new List<StockIndicatorLeftItem>();

            if (dt == null || dt.Rows.Count == 0)
                return new List<StockIndicatorLeftItem>(0);

            result.Capacity = dt.Rows.Count;
            StockIndicatorLeftItem item;
              
            foreach (DataRow row in dt.Rows)
            {
                item = new StockIndicatorLeftItem();
                item.MacroId = row[0].ToString();
                item.MacroName = row[1].ToString();
                DateTime.TryParse(row[2].ToString(), out item.ReportDate);
                float.TryParse(row[3].ToString(), out item.Current);
                float.TryParse(row[4].ToString(), out item.DifferRangeDay5);
                float.TryParse(row[5].ToString(), out item.DifferRangeDay20);
                float.TryParse(row[6].ToString(), out item.DifferRangeDay60);
                float.TryParse(row[7].ToString(), out item.DifferRangeYTD);
                byte fre;
                if (byte.TryParse(row[8].ToString(), out fre))
                {
                    item.Frequency = (DateFrequency)fre;
                }
                bool.TryParse(row[9].ToString(), out item.IsCustomize);


                result.Add(item);
            }

            return result;
        }

    }

    /// <summary>
    /// 宏观指标的指标值——返回包
    /// </summary>
    public class ResIndicatorValuesDataPacket : IndicatorDataPacket
    {
        /// <summary>
        /// 指标值(key: 指标Id; value: 指标的date-value值序列) 
        /// </summary>
        public Dictionary<string, SortedList<int, double>> DicIndicatorValues;

        /// <summary>
        /// 
        /// </summary>
        public ResIndicatorValuesDataPacket()
        {
            DicIndicatorValues = new Dictionary<string, SortedList<int, double>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public ResIndicatorValuesDataPacket(string macroId)
            : this()
        {
            this.TableKeyCode = macroId;
        }

        public override bool Decoding(DataTable dt)
        {
            switch (RequestId)
            {
                case IndicateRequestType.LeftIndicatorsReport:
                case IndicateRequestType.RightIndicatorsReport:
                    break;

                case IndicateRequestType.IndicatorValuesReport:
                    DicIndicatorValues = GetIndicatorValues(dt);
                    break;
            }


            return true;
        }

        private Dictionary<string, SortedList<int, double>> GetIndicatorValues(DataTable dt)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    #endregion
}
