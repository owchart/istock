using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// OrderQueueDataTable
    /// </summary>
    public class OrderQueueDataTable : DataTableBase
    {
        private Dictionary<int, OrderQueueDataRec> _orderQueueDataBuy;
        private Dictionary<int, OrderQueueDataRec> _orderQueueDataSell;

        /// <summary>
        /// 委托队列买一
        /// </summary>
        public Dictionary<int, OrderQueueDataRec> OrderQueueDataBuy
        {
            get { return _orderQueueDataBuy; }
            private set { _orderQueueDataBuy = value; }
        }

        /// <summary>
        /// 委托队列卖一
        /// </summary>
        public Dictionary<int, OrderQueueDataRec> OrderQueueDataSell
        {
            get { return _orderQueueDataSell; }
            private set { _orderQueueDataSell = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderQueueDataTable()
        {
            OrderQueueDataBuy = new Dictionary<int, OrderQueueDataRec>();
            OrderQueueDataSell = new Dictionary<int, OrderQueueDataRec>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResOrderQueueDataPacket)
            {
                SetBuyOrderQueue((ResOrderQueueDataPacket) dataPacket);
                SetSellOrderQueue((ResOrderQueueDataPacket)dataPacket);
            }
        }

        private void SetBuyOrderQueue(ResOrderQueueDataPacket dataPacket)
        {
            if(dataPacket.OrderQueueDataBuy != null)
            {
                OrderQueueDataRec memData;
                OrderQueuePartDealItem itemPart = null;
                if(OrderQueueDataBuy.TryGetValue(dataPacket.Code, out memData))
                {
                    bool hasPart = false;

                    foreach (OrderQueueItem item in dataPacket.OrderQueueDataBuy.ItemDatas)
                    {
                        if(item.Status == OrderQueueItemStatus.Part)
                        {
                            hasPart = true;
                            itemPart = item as OrderQueuePartDealItem;
                            break;
                        }
                    }

                    if(hasPart)
                    {
                        int oldData = 0;
                        int newData = 0;
                        foreach (OrderQueueItem memOneData in memData.ItemDatas)
                        {
                            if (memOneData.Status == OrderQueueItemStatus.Normal ||
                                memOneData.Status == OrderQueueItemStatus.New ||
                                memOneData.Status == OrderQueueItemStatus.Part)
                                oldData += memOneData.Volume;
                        }
                        foreach (OrderQueueItem packetOneData in dataPacket.OrderQueueDataBuy.ItemDatas)
                        {
                            if (packetOneData.Status != OrderQueueItemStatus.New)
                                newData += packetOneData.Volume;
                        }
                        if (itemPart != null)
                        {
                            itemPart.DealVolume = Math.Abs((short) (newData - oldData));
                        }

                    }
                    OrderQueueDataBuy[dataPacket.Code] = dataPacket.OrderQueueDataBuy;
                }
                else
                {
                    OrderQueueDataBuy.Add(dataPacket.Code,dataPacket.OrderQueueDataBuy);
                }
            }
        }
        private void SetSellOrderQueue(ResOrderQueueDataPacket dataPacket)
        {
            if(dataPacket.OrderQueueDataSell != null)
            {
                OrderQueueDataRec memData;
                OrderQueuePartDealItem itemPart = null;
                if(OrderQueueDataSell.TryGetValue(dataPacket.Code, out memData))
                {
                    bool hasPart = false;

                    foreach (OrderQueueItem item in dataPacket.OrderQueueDataSell.ItemDatas)
                    {
                        if(item.Status == OrderQueueItemStatus.Part)
                        {
                            hasPart = true;
                            itemPart = item as OrderQueuePartDealItem;
                            break;
                        }
                    }

                    if(hasPart)
                    {
                        int oldData = 0;
                        int newData = 0;
                        foreach (OrderQueueItem memOneData in memData.ItemDatas)
                        {
                            if (memOneData.Status == OrderQueueItemStatus.Normal ||
                                memOneData.Status == OrderQueueItemStatus.New ||
                                memOneData.Status == OrderQueueItemStatus.Part)
                                oldData += memOneData.Volume;
                        }
                        foreach (OrderQueueItem packetOneData in dataPacket.OrderQueueDataSell.ItemDatas)
                        {
                            if (packetOneData.Status != OrderQueueItemStatus.New)
                                newData += packetOneData.Volume;
                        }
                        if (itemPart != null)
                        {
                            itemPart.DealVolume = Math.Abs((short) (newData - oldData));
                        }
                    }
                    OrderQueueDataSell[dataPacket.Code] = dataPacket.OrderQueueDataSell;
                }
                else
                {
                    OrderQueueDataSell.Add(dataPacket.Code,dataPacket.OrderQueueDataSell);
                }
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            if (status == InitOrgStatus.SHSZ)
            {
                if (_orderQueueDataBuy != null)
                    _orderQueueDataBuy.Clear();
                if (_orderQueueDataSell != null)
                    _orderQueueDataSell.Clear();
            }
                
        }
    }
}
