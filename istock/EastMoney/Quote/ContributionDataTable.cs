using System;
using System.Collections.Generic;

using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// 贡献点数
    /// </summary>
    public class ContributionDataTable : DataTableBase
    {
        private Dictionary<ReqMarketType, List<ContributionDataRec>> _contributionBlock;
        private Dictionary<ReqMarketType, List<ContributionDataRec>> _contributionStock;

        /// <summary>
        /// ContributionStock
        /// </summary>
        public Dictionary<ReqMarketType, List<ContributionDataRec>> ContributionStock
        {
            get { return _contributionStock; }
            private set { _contributionStock = value; }
        }

        /// <summary>
        /// ContributionBlock
        /// </summary>
        public Dictionary<ReqMarketType, List<ContributionDataRec>> ContributionBlock
        {
            get { return _contributionBlock; }
            private set { _contributionBlock = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContributionDataTable()
        {
            ContributionStock = new Dictionary<ReqMarketType, List<ContributionDataRec>>();
            ContributionBlock = new Dictionary<ReqMarketType, List<ContributionDataRec>>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResContributionStockDataPacket)
                SetContributionStockData((ResContributionStockDataPacket)dataPacket);
            else if (dataPacket is ResContributionBlockDataPacket)
                SetContributionBlockData((ResContributionBlockDataPacket) dataPacket);
        }

        void SetContributionStockData(ResContributionStockDataPacket dataPacket)
        {
            if(ContributionStock.ContainsKey(dataPacket.Mt))
            {
                ContributionStock[dataPacket.Mt].Clear();
                ContributionStock[dataPacket.Mt] = dataPacket.ContributionData;
            }
            else
            {
                ContributionStock.Add(dataPacket.Mt,dataPacket.ContributionData);
            }
        }

        void SetContributionBlockData(ResContributionBlockDataPacket dataPacket)
        {
            dataPacket.ContributionData.Sort(Compare);
            if(ContributionBlock.ContainsKey(dataPacket.Mt))
            {
                ContributionBlock[dataPacket.Mt].Clear();
                ContributionBlock[dataPacket.Mt] = dataPacket.ContributionData;
            }
            else
            {
                ContributionBlock.Add(dataPacket.Mt, dataPacket.ContributionData);
            }
        }

        private static int Compare(ContributionDataRec a, ContributionDataRec b)
        {
            if (a.Price < b.Price)
                return 1;
            if (a.Price > b.Price)
                return -1;
            return 0;
        }
    }
}
