using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 资金流
    /// </summary>
    public class CapitalFlowTable : DataTableBase
    {
        private Dictionary<int, CapitalFlowDataRec> _capitalFlowData;
        /// <summary>
        /// 资金流数据
        /// </summary>
        public Dictionary<int, CapitalFlowDataRec> CapitalFlowData{get { return _capitalFlowData; }}
        /// <summary>
        /// 构造函数
        /// </summary>
        public CapitalFlowTable()
        {
            _capitalFlowData = new Dictionary<int, CapitalFlowDataRec>(1);
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResCapitalFlowDataPacket) {
                ResCapitalFlowDataPacket temp = (ResCapitalFlowDataPacket)dataPacket;
                if(null==temp.CapitalFlowData)
                    return;
                if (_capitalFlowData.ContainsKey(temp.CapitalFlowData.Code))
                {
                    _capitalFlowData[temp.CapitalFlowData.Code] = temp.CapitalFlowData;
                }
                else
                {
                    _capitalFlowData.Add(temp.CapitalFlowData.Code,temp.CapitalFlowData);
                }

            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            switch (status)
            {
                case InitOrgStatus.SHSZ:
                    _capitalFlowData.Clear();
                    break;
            }
        }
    }
}
