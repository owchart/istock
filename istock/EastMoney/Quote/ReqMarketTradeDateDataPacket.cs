namespace EmQComm
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ReqMarketTradeDateDataPacket : OrgDataPacket
    {
        public List<EmQComm.TypeCode> TypeCodeList;

        public ReqMarketTradeDateDataPacket()
        {
            base.RequestType = FuncTypeOrg.MarketTradeDate;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            if ((this.TypeCodeList == null) || (this.TypeCodeList.Count < 1))
            {
                this.TypeCodeList = new List<EmQComm.TypeCode>(1);
                foreach (EmQComm.TypeCode code in Enum.GetValues(typeof(EmQComm.TypeCode)))
                {
                    if (code != EmQComm.TypeCode.Na)
                    {
                        this.TypeCodeList.Add(code);
                    }
                }
            }
            if (this.TypeCodeList != null)
            {
                bw.Write((byte) this.TypeCodeList.Count);
                for (byte i = 0; i < this.TypeCodeList.Count; i = (byte) (i + 1))
                {
                    bw.Write((short) this.TypeCodeList[i]);
                }
            }
        }
    }
}

