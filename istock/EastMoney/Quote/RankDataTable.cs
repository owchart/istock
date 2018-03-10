using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// RankDataTable
    /// </summary>
    public class RankDataTable : DataTableBase
    {
        private Dictionary<string, RankOrgDataRec> _rankOrgData;
        private Dictionary<string, NetInflowRankType> _netInflowRankData;
        private Dictionary<ReqSecurityType, RankDataRec> _rankData;

        /// <summary>
        /// RankData
        /// </summary>
        public Dictionary<ReqSecurityType, RankDataRec> RankData
        {
            get { return _rankData; }
            private set { _rankData = value; }
        }

        /// <summary>
        /// 机构版综合排名
        /// </summary>
        public Dictionary<string, RankOrgDataRec> RankOrgData
        {
            get { return _rankOrgData; }
            private set { _rankOrgData = value; }
        }

        /// <summary>
        /// 净流入排名
        /// </summary>
        public Dictionary<string, NetInflowRankType> NetInflowRankData
        {
            get { return _netInflowRankData; }
            private set { _netInflowRankData = value; }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public RankDataTable()
        {
            RankData = new Dictionary<ReqSecurityType, RankDataRec>(0); 
            RankOrgData = new Dictionary<string, RankOrgDataRec>(0);
            NetInflowRankData = new Dictionary<string, NetInflowRankType>(0);
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResRankDataPacket)
                SetRankData(dataPacket as ResRankDataPacket);
            else if(dataPacket is ResRankOrgDataPacket)
                SetRankData(dataPacket as ResRankOrgDataPacket);
            else if(dataPacket is ResNetInflowRankDataPacket)
                SetRankData(dataPacket as ResNetInflowRankDataPacket);
            
        }

        void SetRankData(ResRankDataPacket packet)
        {
            if (packet == null)
                return;

            if (RankData.ContainsKey(packet.RankData.SType))
                RankData[packet.RankData.SType] = packet.RankData;
            else
                RankData.Add(packet.RankData.SType, packet.RankData);
        }

        void SetRankData(ResRankOrgDataPacket dataPacket)
        {
            if (dataPacket == null)
                return;
            RankOrgDataRec memData;
            if(RankOrgData.TryGetValue(dataPacket.BlockId, out memData))
            {
                foreach (KeyValuePair<RankType,List<int>> oneType in dataPacket.Data.RankData)
                    memData.RankData[oneType.Key] = oneType.Value;
            }
            else
                RankOrgData[dataPacket.BlockId] = dataPacket.Data;
        }

        void SetRankData(ResNetInflowRankDataPacket dataPacket)
        {
            if (dataPacket == null)
                return;
            
            NetInflowRankType memData;
            if (NetInflowRankData.TryGetValue(dataPacket.BlockId, out memData))
            {
                memData.BottomStocks = dataPacket.BottomStocks;
                memData.TopStocks = dataPacket.TopStocks;
            }
            else
            {
                NetInflowRankType tmp = new NetInflowRankType();
                tmp.BottomStocks = dataPacket.BottomStocks;
                tmp.TopStocks = dataPacket.TopStocks;
                NetInflowRankData[dataPacket.BlockId] = tmp;
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            if (status == InitOrgStatus.SHSZ)
            {
                if (_rankOrgData != null)
                    _rankOrgData.Clear();
                if (_netInflowRankData != null)
                    _netInflowRankData.Clear();
            }

        }
    }

    public class NetInflowRankType
    {
        public List<int> TopStocks;
        public List<int> BottomStocks;
    }
}

