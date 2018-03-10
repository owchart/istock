using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// OrgRateDataTable
    /// </summary>
    class OrgRateDataTable : DataTableBase
    {
        private Dictionary<int, OrgRateDataRec> _orgRateDatas;

        /// <summary>
        /// OrgRateDatas
        /// </summary>
        public Dictionary<int, OrgRateDataRec> OrgRateDatas
        {
            get { return _orgRateDatas; }
            private set { _orgRateDatas = value; }
        }

        /// <summary>
        /// 机构版机构评级
        /// </summary>
        public Dictionary<int, List<OneInfoRateOrgItem>> InfoRateOrgData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrgRateDataTable()
        {
            OrgRateDatas = new Dictionary<int, OrgRateDataRec>();
            InfoRateOrgData = new Dictionary<int, List<OneInfoRateOrgItem>>(1);
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResOrgRateDataPacket)
            {
                ResOrgRateDataPacket packet = (ResOrgRateDataPacket) dataPacket;
                if (packet.RateData.Code != 0)
                {
                    if (OrgRateDatas.ContainsKey(packet.RateData.Code))
                        OrgRateDatas[packet.RateData.Code] = packet.RateData;
                    else
                        OrgRateDatas.Add(packet.RateData.Code, packet.RateData);
                }
            }
            else if (dataPacket is ResInfoRateOrgDataPacket)
            {
                ResInfoRateOrgDataPacket packet = (ResInfoRateOrgDataPacket) dataPacket;
                InfoRateOrgData[packet.Code] = packet.InfoRateOrgList;
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            if (status == InitOrgStatus.SHSZ)
            {
                if (_orgRateDatas != null)
                    _orgRateDatas.Clear();
            }

        }
    }
}
