using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class DepthAnalyseDataTable : DataTableBase
    {
        private Dictionary<int, ShareHolderDataRec> _allShareHolderDataRec;
        private Dictionary<int, ProfitTrendDataRec> _allProfitTrendDataRec;

        /// <summary>
        /// 十大股东
        /// </summary>
        public Dictionary<int, ShareHolderDataRec> AllShareHolderDataRec
        {
            get { return _allShareHolderDataRec; }
            private set { _allShareHolderDataRec = value; }
        }

        /// <summary>
        /// 利润趋势
        /// </summary>
        public Dictionary<int, ProfitTrendDataRec> AllProfitTrendDataRec
        {
            get { return _allProfitTrendDataRec; }
            private set { _allProfitTrendDataRec = value; }
        }

        public DepthAnalyseDataTable()
        {
            AllShareHolderDataRec = new Dictionary<int, ShareHolderDataRec>();
            AllProfitTrendDataRec = new Dictionary<int, ProfitTrendDataRec>();
        }

         public override void SetData(DataPacket dataPacket)
         {
             if(dataPacket is ResDepthAnalyseDataPacket)
             {
                ResDepthAnalyseDataPacket packet = dataPacket as ResDepthAnalyseDataPacket;
                AllShareHolderDataRec[packet.Code] = packet.HolderDataRec;
                AllProfitTrendDataRec[packet.Code] = packet.ProfitTrend;
             }
         }
    }
}
