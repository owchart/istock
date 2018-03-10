using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EmQComm;

namespace EmQDataCore
{
    public class DetailLev2DataTable : DataTableBase
    {
        private Dictionary<int, StockOrderDetailDataRec> _allStockOrderDetailDataRec;
        private Dictionary<int, StockOrderDetailDataRec> _nStockOrderDetailDataRec;      
        private Dictionary<int, Dictionary<float, StockPriceOrderQueueDataRec>> _allStockBuyOrderQueueDataRec;
        private Dictionary<int, Dictionary<float, StockPriceOrderQueueDataRec>> _allStockSellOrderQueueDataRec;
        private Dictionary<int, StatisticsAnalysisDataRec> _allStatisticsAnalysisDataRec;


        /// <summary>
        /// 所有股票的百档行情
        /// </summary>
        public Dictionary<int, StockOrderDetailDataRec> AllStockOrderDetailDataRec
        {
            get
            {
                return _allStockOrderDetailDataRec;
            }
        }
        /// <summary>
        /// 所有股票的N档行情(默认20档)
        /// </summary>
        public Dictionary<int, StockOrderDetailDataRec> NStockOrderDetailDataRec
        {
            get { return _nStockOrderDetailDataRec; }
        }
        /// <summary>
        /// 委买详单
        /// </summary>
        public Dictionary<int, Dictionary<float, StockPriceOrderQueueDataRec>> AllStockSellOrderQueueDataRec
        {
            get { return _allStockSellOrderQueueDataRec; }
        }

        /// <summary>
        /// 委卖详单
        /// </summary>
        public Dictionary<int, Dictionary<float, StockPriceOrderQueueDataRec>> AllStockBuyOrderQueueDataRec
        {
            get { return _allStockBuyOrderQueueDataRec; }
        }

        /// <summary>
        /// 统计明细
        /// </summary>
        public Dictionary<int, StatisticsAnalysisDataRec> AllStatisticsAnalysisDataRec
        {
            get { return _allStatisticsAnalysisDataRec; }
        }

        public DetailLev2DataTable()
        {
            _allStockOrderDetailDataRec = new Dictionary<int, StockOrderDetailDataRec>(1);
            _nStockOrderDetailDataRec = new Dictionary<int, StockOrderDetailDataRec>(1);
            _allStockBuyOrderQueueDataRec = new Dictionary<int, Dictionary<float, StockPriceOrderQueueDataRec>>(1);
            _allStockSellOrderQueueDataRec = new Dictionary<int, Dictionary<float, StockPriceOrderQueueDataRec>>(1);
            _allStatisticsAnalysisDataRec = new Dictionary<int, StatisticsAnalysisDataRec>(1);
        }

        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResAllOrderStockDetailLev2DataPacket)
                SetAllOrderStockDetailLev2Data(dataPacket as ResAllOrderStockDetailLev2DataPacket);
            else if (dataPacket is ResNOrderStockDetailLev2DataPacket)
                SetNOrderStockDetailLev2Data(dataPacket as ResNOrderStockDetailLev2DataPacket);
            else if (dataPacket is ResStockDetailLev2OrderQueueDataPacket)
                SetStockDetailLev2OrderQueueData(dataPacket as ResStockDetailLev2OrderQueueDataPacket);
            else if (dataPacket is ResStatisticsAnalysisDataPacket)
                SetStatisticsAnalysisData(dataPacket as ResStatisticsAnalysisDataPacket);
        }

        /// <summary>
        /// 百档行情
        /// </summary>
        /// <param name="packet"></param>
        private void SetAllOrderStockDetailLev2Data(ResAllOrderStockDetailLev2DataPacket packet)
        {
            if (packet.OrderDetailData != null)
            {
                for (int i = 0; i < packet.OrderDetailData.Count; i++)
                {
                    //StockOrderDetailDataRec memData;
                    //if (_allStockOrderDetailDataRec.TryGetValue(packet.OrderDetailData[i].Code, out memData))
                    //{
                        //MarketType mt = MarketType.NA;
                        
                        //if (DetailData.AllStockDetailData.ContainsKey(packet.OrderDetailData[i].Code))
                        //    mt = (MarketType)DetailData.AllStockDetailData[packet.OrderDetailData[i].Code][FieldIndex.Market];
                        //switch (mt)
                        //{
                        //    case MarketType.SHALev1:
                        //    case MarketType.SHALev2:
                        //    case MarketType.SHBLev1:
                        //    case MarketType.SHBLev2:
                        //    case MarketType.SHConvertBondLev1:
                        //    case MarketType.SHConvertBondLev2:
                        //    case MarketType.SHNonConvertBondLev1:
                        //    case MarketType.SHNonConvertBondLev2:
                        //    case MarketType.SHFundLev1:
                        //    case MarketType.SHFundLev2:
                        //        SetDeltaVolume(memData, packet.OrderDetailData[i]);
                        //        break;
                        //}
                    //    _allStockOrderDetailDataRec[packet.OrderDetailData[i].Code] = packet.OrderDetailData[i];
                    //}
                    //else
                    //{
                    //    _allStockOrderDetailDataRec[packet.OrderDetailData[i].Code] = packet.OrderDetailData[i];    
                    //}
                    _allStockOrderDetailDataRec[packet.OrderDetailData[i].Code] = packet.OrderDetailData[i];
                    //Debug.Print("code=" + packet.OrderDetailData[i].Code + "  ,sellPrice1=" + packet.OrderDetailData[i].SellDetail[0].Price);
                }
                    
            }
        }

        /// <summary>
        /// N(默认20)档行情
        /// </summary>
        /// <param name="packet"></param>
        private void SetNOrderStockDetailLev2Data(ResNOrderStockDetailLev2DataPacket packet)
        {
            if (packet.OrderDetailData != null)
            {
                for (int i = 0; i < packet.OrderDetailData.Count; i++)
                {
                    StockOrderDetailDataRec memData;
                    if (_nStockOrderDetailDataRec.TryGetValue(packet.OrderDetailData[i].Code, out memData))
                    {
                        MarketType mt = MarketType.NA;
                        int mtInt = 0;
                        if (DetailData.FieldIndexDataInt32.ContainsKey(packet.OrderDetailData[i].Code))
                            DetailData.FieldIndexDataInt32[packet.OrderDetailData[i].Code].TryGetValue(FieldIndex.Market, out mtInt);
                        mt = (MarketType)mtInt;
                        switch (mt)
                        {
                            case MarketType.SHALev1:
                            case MarketType.SHALev2:
                            case MarketType.SHBLev1:
                            case MarketType.SHBLev2:
                            case MarketType.SHConvertBondLev1:
                            case MarketType.SHConvertBondLev2:
                            case MarketType.SHNonConvertBondLev1:
                            case MarketType.SHNonConvertBondLev2:
                            case MarketType.SHFundLev1:
                            case MarketType.SHFundLev2:
                                SetDeltaVolume(memData, packet.OrderDetailData[i]);
                                break;
                        }
                        _nStockOrderDetailDataRec[packet.OrderDetailData[i].Code] = packet.OrderDetailData[i];
                    }
                    else
                    {
                        _nStockOrderDetailDataRec[packet.OrderDetailData[i].Code] = packet.OrderDetailData[i];
                    }
                }

            }
        }

        private void SetDeltaVolume(StockOrderDetailDataRec memData, StockOrderDetailDataRec packetData)
        {
            List<OneOrderDetail> memBuyList = memData.BuyDetail;
            List<OneOrderDetail> packetBuyList = packetData.BuyDetail;

            for (int i = 0; i < packetBuyList.Count; i++)
            {
                if (packetBuyList[i].Price == 0)
                    continue;
                for (int j = 0; j < memBuyList.Count; j++)
                {
                    if (packetBuyList[i].Price == memBuyList[j].Price)
                    {
                        packetBuyList[i].DeltaVolume = (packetBuyList[i].Volume - memBuyList[j].Volume);
                        break;
                    }
                }
            }

            List<OneOrderDetail> memSellList = memData.SellDetail;
            List<OneOrderDetail> packetSellList = packetData.SellDetail;

            for (int i = 0; i < packetSellList.Count; i++)
            {
                if (packetSellList[i].Price == 0)
                    continue;
                for (int j = 0; j < memSellList.Count; j++)
                {
                    if (packetSellList[i].Price == memSellList[j].Price)
                    {
                        packetSellList[i].DeltaVolume = (packetSellList[i].Volume - memSellList[j].Volume);
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// 百档挂单队列
        /// </summary>
        /// <param name="packet"></param>
        private void SetStockDetailLev2OrderQueueData(ResStockDetailLev2OrderQueueDataPacket packet)
        {
            if (packet.StockPriceOrderData != null)
            {
                for (int i = 0; i < packet.StockPriceOrderData.Count; i++)
                {
                    if (packet.StockPriceOrderData[i].BSFlag == 0)
                    {
                        Dictionary<float, StockPriceOrderQueueDataRec> memData;
                        if (_allStockBuyOrderQueueDataRec.TryGetValue(packet.StockPriceOrderData[i].Code,
                            out memData))
                        {
                            memData[packet.StockPriceOrderData[i].Price] = packet.StockPriceOrderData[i];
                        }
                        else
                        {
                            memData = new Dictionary<float, StockPriceOrderQueueDataRec>(1);
                            memData[packet.StockPriceOrderData[i].Price] = packet.StockPriceOrderData[i];
                            _allStockBuyOrderQueueDataRec[packet.StockPriceOrderData[i].Code] = memData;
                        }
                    }
                    else if (packet.StockPriceOrderData[i].BSFlag == 1)
                    {
                        Dictionary<float, StockPriceOrderQueueDataRec> memData;
                        if (_allStockSellOrderQueueDataRec.TryGetValue(packet.StockPriceOrderData[i].Code,
                            out memData))
                        {
                            memData[packet.StockPriceOrderData[i].Price] = packet.StockPriceOrderData[i];
                        }
                        else
                        {
                            memData = new Dictionary<float, StockPriceOrderQueueDataRec>(1);
                            memData[packet.StockPriceOrderData[i].Price] = packet.StockPriceOrderData[i];
                            _allStockSellOrderQueueDataRec[packet.StockPriceOrderData[i].Code] = memData;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 统计明细
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetStatisticsAnalysisData(ResStatisticsAnalysisDataPacket dataPacket)
        {
            _allStatisticsAnalysisDataRec[dataPacket.Code] = dataPacket.StatisticsData;
        }

        public override void ClearData(InitOrgStatus status)
        {
            MarketType mt = MarketType.NA;
            switch (status)
            {
                case InitOrgStatus.SHSZ:
                    if (_allStockOrderDetailDataRec != null)
                        _allStockOrderDetailDataRec.Clear();
                    if (_nStockOrderDetailDataRec != null)
                        _nStockOrderDetailDataRec.Clear();
                    if (_allStockBuyOrderQueueDataRec != null)
                        _allStockBuyOrderQueueDataRec.Clear();
                    if (_allStockSellOrderQueueDataRec != null)
                        _allStockSellOrderQueueDataRec.Clear();
                    if (_allStatisticsAnalysisDataRec != null)
                        _allStatisticsAnalysisDataRec.Clear();
                    break;
            }
        }
    }
}
