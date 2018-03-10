using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 交易日
    /// </summary>
    class TradeDateDataTable : DataTableBase
    {
        private Dictionary<MarketType, List<int>> _marketTradeDate;
        public Dictionary<MarketType, List<int>> MarketTradeDate
        {
            get { return _marketTradeDate; }
            private set { _marketTradeDate = value; }
        }

        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResTradeDateDataPacket)
            {
                if(MarketTradeDate != null)
                    MarketTradeDate.Clear();
                MarketTradeDate = (dataPacket as ResTradeDateDataPacket).MarketTradeDate;
                if (MarketTradeDate != null && MarketTradeDate.ContainsKey(MarketType.SHALev1))
                    TimeUtilities.TradeDate = MarketTradeDate[MarketType.SHALev1][0];
            }
        }

        public void SetSHTradeDate(int date)
        {
             if (MarketTradeDate != null && MarketTradeDate.ContainsKey(MarketType.SHALev1))
             {
                 List<int> sh = MarketTradeDate[MarketType.SHALev1];
                 if (sh != null && sh.Count > 0)
                     sh[0] = date;
                 else
                 {
                     sh = new List<int>(1);
                     sh.Add(date);
                 }
             }
                 
        }
       
    }
}
