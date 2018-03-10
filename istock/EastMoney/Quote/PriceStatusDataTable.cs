using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// PriceStatusDataTable
    /// </summary>
    public class PriceStatusDataTable : DataTableBase
    {
        private Dictionary<int, PriceStatusDataRec> _priceStatusData;

        /// <summary>
        /// 分价表数据
        /// </summary>
        public Dictionary<int, PriceStatusDataRec> PriceStatusData { get { return _priceStatusData; } }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PriceStatusDataTable()
        {
            _priceStatusData = new Dictionary<int, PriceStatusDataRec>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResPriceStatusDataPacket)
            {
                ResPriceStatusDataPacket packet = dataPacket as ResPriceStatusDataPacket;
                PriceStatusDataRec memData;

                if(_priceStatusData.TryGetValue(packet.PriceStatusData.Code, out memData))
                {
                    for(int i = 0;i < packet.PriceStatusData.PriceStatusList.Count; i++)
                    {
                        for (int j = memData.PriceStatusList.Count - 1; j >= 0; j--)
                        {
                            if(memData.PriceStatusList[j].Price > packet.PriceStatusData.PriceStatusList[i].Price)
                            {
                                memData.PriceStatusList.Insert(j+1,packet.PriceStatusData.PriceStatusList[i]);
                                break;
                            }
                            if(memData.PriceStatusList[j].Price == packet.PriceStatusData.PriceStatusList[i].Price)
                            {
                                memData.PriceStatusList[j].BuyVolume =
                                    packet.PriceStatusData.PriceStatusList[i].BuyVolume;
                                memData.PriceStatusList[j].SellVolume =
                                    packet.PriceStatusData.PriceStatusList[i].SellVolume;
                                break;
                            }
                            if (j == 0)
                            {
                                memData.PriceStatusList.Insert(0,packet.PriceStatusData.PriceStatusList[i]);
                                break;
                            }
                        }
                    }
                }
                else
                    _priceStatusData.Add(packet.PriceStatusData.Code,packet.PriceStatusData);
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            if (status == InitOrgStatus.SHSZ)
            {
                if (_priceStatusData != null)
                    _priceStatusData.Clear();
            }

        }
    }


}
