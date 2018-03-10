using System;
using System.Collections.Generic;
 
using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// F10DataTable
    /// </summary>
    class F10DataTable : DataTableBase
    {
        private Dictionary<int, F10DataRec> _allF10Data;

        /// <summary>
        /// AllF10Data
        /// </summary>
        public Dictionary<int, F10DataRec> AllF10Data
        {
            get { return _allF10Data; }
            private set { _allF10Data = value; }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public F10DataTable()
        {
            AllF10Data = new Dictionary<int, F10DataRec>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResF10DataPacket)
            {
                ResF10DataPacket packet = (ResF10DataPacket) dataPacket;
                F10DataRec memData;
                if(AllF10Data.TryGetValue(packet.F10Data.Code, out memData))
                {
                    if (packet.F10Data.F10FieldData.ContainsKey(Convert.ToInt32(packet.FieldType)))
                    {
                        if (memData.F10FieldData.ContainsKey(Convert.ToInt32(packet.FieldType)))
                            memData.F10FieldData[Convert.ToInt32(packet.FieldType)] =
                                packet.F10Data.F10FieldData[Convert.ToInt32(packet.FieldType)];
                        else
                            memData.F10FieldData.Add(Convert.ToInt32(packet.FieldType),
                                                     packet.F10Data.F10FieldData[Convert.ToInt32(packet.FieldType)]);
                    }
                }
                else
                {
                    AllF10Data.Add(packet.F10Data.Code,packet.F10Data);
                }
            }
        }
    }
}
