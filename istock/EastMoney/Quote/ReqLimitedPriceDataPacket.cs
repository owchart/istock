namespace EmQComm
{
    using System;
    using System.IO;
    using System.Text;
    using EmQDataCore;

    public class ReqLimitedPriceDataPacket : RealTimeDataPacket
    {
        private int _code;
        private byte _market;
        private string _shortCode;

        public ReqLimitedPriceDataPacket()
        {
            base.RequestType = FuncTypeRealTime.LimitedPrice;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(this._market);
            byte[] bytes = Encoding.ASCII.GetBytes(this._shortCode);
            byte[] buffer = new byte[0x10];
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[i] = bytes[i];
            }
            bw.Write(buffer);
        }

        public int Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                ReqMarketType market = ReqMarketType.MT_NA;
                DataPacket.ParseCode(DetailData.FieldIndexDataString[this._code][FieldIndex.EMCode], out market, out this._shortCode);
                this._market = (byte) market;
                switch (DataCenterCore.CreateInstance().GetMarketType(this._code))
                {
                    case MarketType.IF:
                    case MarketType.GoverFutures:
                        this._shortCode = ConvertCode.ConvertFuturesOrgCodeToCftShortCode(this._code);
                        break;
                }
            }
        }
    }
}

