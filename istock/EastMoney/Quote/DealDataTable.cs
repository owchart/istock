using System;
using System.Collections.Generic;

using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// DealDataTable
    /// </summary>
    public class DealDataTable : DataTableBase
    {
        private Dictionary<int, List<OneDealDataRec>> _allDealData;

        private Dictionary<int, List<BidDetail>> _allBidDetailData;


        private Dictionary<int, List<OfferBankDetail>> _allOfferBankDetailData;

        /// <summary>
        /// “银行间债券报价明细”全部缓存
        /// </summary>
        public Dictionary<int, List<BidDetail>> AllBidDetailData
        {
            get { return _allBidDetailData; }
            set { _allBidDetailData = value; }
        }
        /// <summary>
        /// “SHIBOR报价行明细”全部缓存
        /// </summary>
        public Dictionary<int, List<OfferBankDetail>> AllOfferBankDetailData
        {
            get { return _allOfferBankDetailData; }
            set { _allOfferBankDetailData = value; }
        }


        /// <summary>
        /// AllDealData
        /// </summary>
        public Dictionary<int, List<OneDealDataRec>> AllDealData { get { return _allDealData; } }
        /// <summary>
        /// 构造方法
        /// </summary>
        public DealDataTable()
        {
            _allDealData = new Dictionary<int, List<OneDealDataRec>>(1);
            _allBidDetailData = new Dictionary<int, List<BidDetail>>(1);
            _allOfferBankDetailData = new Dictionary<int, List<OfferBankDetail>>(1);
        }

        /// <summary>
        /// 找到timePacket的插入位子
        /// </summary>
        /// <param name="memDatas"></param>
        /// <param name="timePacket"></param>
        /// <param name="beginIndex"></param>
        /// <returns>若memDatas里有timePacket相同的项，则返回-1</returns>
        private int FindIndex(List<OneDealDataRec> memDatas, int timePacket, int beginIndex)
        {
            int lowerBound = beginIndex;
            int upperBound = memDatas.Count - 1;
            int curIndex = 0;
            if (memDatas.Count == 0)
                return 0;
            while (true)
            {
                curIndex = (upperBound + lowerBound) / 2;
                int timeMem = memDatas[curIndex].Hour * 10000 + memDatas[curIndex].Mint * 100 + memDatas[curIndex].Second;
                if (timeMem > timePacket)
                {
                    upperBound = curIndex - 1;
                    if (lowerBound > upperBound)
                        return curIndex;
                }
                else if (timeMem < timePacket)
                {
                    lowerBound = curIndex + 1;
                    if (lowerBound > upperBound)
                        return lowerBound;
                }
                else
                    return -1;
            }
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            //string testname = typeof(dataPacket).FullName;

            if (dataPacket is ResDealSubscribeDataPacket)
                SetStockDealData(((ResDealSubscribeDataPacket)dataPacket).OneStockDealDatas);
            else if (dataPacket is ResDealRequestDataPacket)
                SetStockDealData(((ResDealRequestDataPacket)dataPacket).OneStockDealDatas);
            else if (dataPacket is ResOSFuturesLMEDealDataPacket)
                SetLmeDealData(dataPacket as ResOSFuturesLMEDealDataPacket);
            else if (dataPacket is ResLowFrequencyTBYDataPacket)
                SetLowFreDealData(dataPacket as ResLowFrequencyTBYDataPacket);
            else if (dataPacket is ResBankBondReportDataPacket)
                SetBankBondReportData(dataPacket as ResBankBondReportDataPacket);
            else if (dataPacket is ResShiborReportDataPacket)
                SetShiborReportData(dataPacket as ResShiborReportDataPacket);
        }

        private void SetShiborReportData(ResShiborReportDataPacket dataPacket)
        {
            int code = dataPacket.Code;
            List<OfferBankDetail> details;

            if (!_allOfferBankDetailData.TryGetValue(code, out details))
                _allOfferBankDetailData.Add(dataPacket.Code, dataPacket.Details);
            else
                _allOfferBankDetailData[code] = dataPacket.Details;
        }

        private void SetBankBondReportData(ResBankBondReportDataPacket packet)
        {
            List<BidDetail> details;

            #region 没有该key对应的缓存
            if (!_allBidDetailData.TryGetValue(packet.Code, out details))
            {
                details = packet.Details;
                _allBidDetailData.Add(packet.Code, details);
                return;
            }
            #endregion

            #region 有缓存，但是缓存为空或者缓存内无数据
            if (details == null || details.Count == 0)
            {
                details = packet.Details;
                _allBidDetailData[packet.Code] = details;
                return;
            }
            #endregion

            #region 缓存有数据需要插入排序（缓存是时间倒序排列：插入逻辑，按照时间排序，如果时间一样，后来的数据前置）

            int insertIndex = -1;
            long cacheRecTimeKey = 0;
            long packetRecTimeKey = 0;
            bool isAddEnd = false;
            bool isSame = false;
            foreach (BidDetail rec in packet.Details)
            {
                isSame = false;
                isAddEnd = false;
                insertIndex = -1;

                for (int i = 0; i < details.Count; i++)
                {
                    if (details[i].Id == rec.Id)
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame)
                    continue;

                packetRecTimeKey = ((long)rec.Date) * 100000 + rec.Time;
                for (int i = 0; i < details.Count; i++)
                {
                    cacheRecTimeKey = ((long)details[i].Date) * 100000 + details[i].Time;

                    if (packetRecTimeKey >= cacheRecTimeKey)
                    {
                        insertIndex = i;
                        break;
                    }
                    if (i == details.Count - 1)
                        isAddEnd = true;
                }

                if (insertIndex >= 0)
                    details.Insert(insertIndex, rec);
                else if (isAddEnd)
                    details.Add(rec);
            }

            #endregion

        }
        private void SetLowFreDealData(ResLowFrequencyTBYDataPacket dataPacket)
        {
            int code = dataPacket.Code;
            List<OneDealDataRec> details;

            if (_allDealData.TryGetValue(code, out details))
            {
                int tmpIndex = -1;
                for (int i = 0; i < details.Count; i++)
                {
                    if (details[i].UId >= dataPacket.Details[0].UId)
                    {
                        tmpIndex = i;
                        break;
                    }
                }
                if (tmpIndex >= 0)
                    details.RemoveRange(tmpIndex, details.Count - tmpIndex);

                details.AddRange(dataPacket.Details);
            }
            else
            {
                _allDealData.Add(dataPacket.Code, dataPacket.Details);
            }
        }
        private void SetStockDealData(OneStockDealDataRec packeDatas)
        {
            List<OneDealDataRec> memDatas;
            if (_allDealData.TryGetValue(packeDatas.Code, out memDatas))
            {
                int lastIndex = 0;
                MarketType mt = Dc.GetMarketType(packeDatas.Code);

                if (mt == MarketType.IF || mt == MarketType.GoverFutures)
                {
                    int packetTime = packeDatas.DealDatas[0].Hour * 10000 + packeDatas.DealDatas[0].Mint * 100 +
                                                packeDatas.DealDatas[0].Second;
                    lastIndex = FindIndex(memDatas, packetTime, lastIndex);
                    if (lastIndex >= 0)
                        memDatas.RemoveRange(lastIndex, memDatas.Count - lastIndex);
                    memDatas.AddRange(packeDatas.DealDatas);
                }
                else
                {
                    //找到第一个需要插入的值的插入位子
                    for (int i = 0; i < packeDatas.DealDatas.Count; i++)
                    {
                        int packetTime = packeDatas.DealDatas[i].Hour * 10000 + packeDatas.DealDatas[i].Mint * 100 +
                                                packeDatas.DealDatas[i].Second;
                        lastIndex = FindIndex(memDatas, packetTime, lastIndex);
                        if (lastIndex >= 0)
                        {
                            if (lastIndex == memDatas.Count - 1)
                            {
                                memDatas.AddRange(packeDatas.DealDatas);
                                return;
                            }
                            memDatas.Insert(lastIndex, packeDatas.DealDatas[i]);
                        }
                    }
                }


            }
            else
            {
                _allDealData.Add(packeDatas.Code, packeDatas.DealDatas);
            }
        }

        private void SetLmeDealData(ResOSFuturesLMEDealDataPacket dataPacket)
        {
            List<OneDealDataRec> memDatas;
            if (_allDealData.TryGetValue(dataPacket.OneStockDealDatas.Code, out memDatas))
            {
                int tmpIndex = -1;
                for (int i = 0; i < memDatas.Count; i++)
                {
                    if (memDatas[i].UId >= dataPacket.OneStockDealDatas.DealDatas[0].UId)
                    {
                        tmpIndex = i;
                        break;
                    }
                }
                if (tmpIndex >= 0)
                    memDatas.RemoveRange(tmpIndex, memDatas.Count - tmpIndex);
                memDatas.AddRange(dataPacket.OneStockDealDatas.DealDatas);
            }
            else
            {
                _allDealData.Add(dataPacket.OneStockDealDatas.Code, dataPacket.OneStockDealDatas.DealDatas);
            }
        }


        public override void ClearData(InitOrgStatus status)
        {
            MarketType mt = MarketType.NA;
            List<MarketType> mtList;
            List<int> removeCode = new List<int>(1);
            foreach (KeyValuePair<int, List<OneDealDataRec>> oneStock in _allDealData)
            {
                mt = Dc.GetMarketType(oneStock.Key);
                if (SecurityAttribute.InitMarketType.TryGetValue(status, out mtList))
                {
                    if (mtList.Contains(mt))
                        removeCode.Add(oneStock.Key);
                }
            }
            foreach (int code in removeCode)
            {
                if (_allDealData.ContainsKey(code))
                    _allDealData.Remove(code);
            }
        }
    }
}
