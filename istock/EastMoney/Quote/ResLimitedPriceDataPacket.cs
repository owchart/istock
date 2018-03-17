using System;
using System.IO;

namespace OwLib
{
    public class ResLimitedPriceDataPacket : RealTimeDataPacket
    {
        public int Code;

        protected override bool DecodingBody(BinaryReader br)
        {
            byte market = br.ReadByte();
            byte[] buffer = br.ReadBytes(0x10);
            byte[] srcCode = new byte[7];
            for (int i = 0; i < srcCode.Length; i++)
            {
                srcCode[i] = buffer[i];
            }
            String emcode = ConvertCode.ConvertIntToCode((uint) ConvertCode.ConvertCodeToInt(srcCode, market));
            switch (market)
            {
                case 8:
                {
                    emcode = ConvertCode.ConvertFuturesCftEmCodeToOrgEmCode(emcode);
                    break;
                }
            }
            if (DetailData.EmCodeToUnicode.ContainsKey(emcode))
            {
                this.Code = DetailData.EmCodeToUnicode[emcode];
            }
            else
            {
                return false;
            }
            br.ReadBytes(4);
            float fieldValue = br.ReadSingle();
            float num5 = br.ReadSingle();
            switch (DataCenterCore.CreateInstance().GetMarketType(this.Code))
            {
                case MarketType.TB_NEW:
                case MarketType.TB_OLD:
                    if (fieldValue >= 900000f)
                    {
                        fieldValue = 0f;
                    }
                    if (num5 < 0f)
                    {
                        num5 = 0f;
                    }
                    break;
            }
            DetailData.FieldIndexDataSingle[this.Code][FieldIndex.UpLimit] = fieldValue;
            DetailData.FieldIndexDataSingle[this.Code][FieldIndex.DownLimit] = num5;
            CFTService.CallBack(FuncTypeRealTime.LimitedPrice, fieldValue.ToString() + num5.ToString());
            return true;
        }
    }
}

