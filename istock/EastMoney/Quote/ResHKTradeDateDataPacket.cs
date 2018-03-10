namespace OwLib
{
    using System;
    using System.IO;

    public class ResHKTradeDateDataPacket : OrgDataPacket
    {
        protected override bool DecodingBody(BinaryReader br)
        {
            byte num = br.ReadByte();
            for (int i = 0; i < num; i++)
            {
                short num3 = br.ReadInt16();
                OneMarketTradeDateDataRec rec = new OneMarketTradeDateDataRec();
                rec.MarketTypeCode = (TypeCode) num3;
                rec.DstStatus = br.ReadByte();
                TimeUtilities.SetSummerFlag((TypeCode) num3, rec.DstStatus);
                byte num4 = br.ReadByte();
                for (byte j = 0; j < num4; j = (byte) (j + 1))
                {
                    OneMarketTradeDateDataRec.TradeDateStruct item = new OneMarketTradeDateDataRec.TradeDateStruct();
                    item.Date = br.ReadInt32();
                    item.Type = br.ReadByte();
                    rec.TradeDateDict.Add(item);
                }
                TimeUtilities.TypeCodeTradeDate[(TypeCode) num3] = rec;
            }
            return true;
        }
    }
}

