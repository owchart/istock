using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace OwLib
{
    /// <summary>
    /// TrendDataTable
    /// </summary>
    public class TrendDataTable : DataTableBase
    {
        private Dictionary<int, Dictionary<int, OneDayTrendDataRec>> _allTrendData;
        private Dictionary<int, OneDayRedGreenDataRec> _allRedGreenData;
        private Dictionary<int, Dictionary<int, StockTrendCaptialFlowDataRec>> _allTrendCaptialFlowData;

        /// <summary>
        /// 内存中所有证券的走势数据
        /// </summary>
        public Dictionary<int, Dictionary<int, OneDayTrendDataRec>> AllTrendData { get { return _allTrendData; } }

        /// <summary>
        /// 分时资金流数据
        /// </summary>
        public Dictionary<int, Dictionary<int, StockTrendCaptialFlowDataRec>> AllTrendCaptialFlowData { get { return _allTrendCaptialFlowData; } } 

        /// <summary>
        /// 红绿均
        /// </summary>
        public Dictionary<int, OneDayRedGreenDataRec> AllRedGreenData
        {
            get { return _allRedGreenData; }
            private set { _allRedGreenData = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TrendDataTable()
        {
            _allTrendData = new Dictionary<int, Dictionary<int, OneDayTrendDataRec>>();
            AllRedGreenData = new Dictionary<int, OneDayRedGreenDataRec>();
            _allTrendCaptialFlowData = new Dictionary<int, Dictionary<int, StockTrendCaptialFlowDataRec>>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            int shIndex = 0;
            int szIndex = 0;
            DetailData.EmCodeToUnicode.TryGetValue("000001.SH", out shIndex);
            DetailData.EmCodeToUnicode.TryGetValue("399001.SZ", out szIndex);
            if (dataPacket is ResStockTrendDataPacket)
                SetTrendDataPacket((ResStockTrendDataPacket)dataPacket);
            else if (dataPacket is ResTrendAskBidDataPacket)
                SetAskBidDataPacket((ResTrendAskBidDataPacket)dataPacket);
            else if (dataPacket is ResTrendInOutDiffDataPacket)
                SetInOutDiffDataPacket((ResTrendInOutDiffDataPacket)dataPacket);
            else if (dataPacket is ResHisTrendDataPacket)
                SetHisTrendDataPacket((ResHisTrendDataPacket)dataPacket);
            else if (dataPacket is ResOceanTrendDataPacket)//外盘
                SetOceanTrendDataPacket((ResOceanTrendDataPacket)dataPacket);
            else if (dataPacket is ResIndexFuturesTrendDataPacket)//股指期货
                SetTrendIFDataPacket((ResIndexFuturesTrendDataPacket)dataPacket);
            else if (dataPacket is ResTrendOrgDataPacket)//机构
                SetTrendDataPacket((ResTrendOrgDataPacket)dataPacket);
            else if (dataPacket is ResRedGreenDataPacket)
                SetRedGreenDataPacket((ResRedGreenDataPacket)dataPacket);
            else if (dataPacket is ResTrendCaptialFlowDataPacket)
                SetTrendCaptialFlowDataPacket((ResTrendCaptialFlowDataPacket)dataPacket);

            else if (dataPacket is ResStockDetailDataPacket && shIndex != 0 && szIndex != 0
                && ((ResStockDetailDataPacket)dataPacket).Code != shIndex && ((ResStockDetailDataPacket)dataPacket).Code != szIndex)//level1推送
                SetDetailDataPacket((ResStockDetailDataPacket)dataPacket);
            else if (dataPacket is ResStockDetailLev2DataPacket && shIndex != 0 && szIndex != 0
                && ((ResStockDetailLev2DataPacket)dataPacket).Code != shIndex && ((ResStockDetailLev2DataPacket)dataPacket).Code != szIndex)//level2推送
                SetDetailDataPacket((ResStockDetailLev2DataPacket)dataPacket);
            else if (dataPacket is ResNOrderStockDetailLev2DataPacket)//level2推送)
                SetDetailDataPacket((ResNOrderStockDetailLev2DataPacket)dataPacket);
            else if (dataPacket is ResOceanRecordDataPacket)//外盘推送
                SetOceanRecordDataPacket((ResOceanRecordDataPacket)dataPacket);
            else if (dataPacket is ResIndexDetailDataPacket && shIndex != 0 && szIndex != 0
                && !((ResIndexDetailDataPacket)dataPacket).Codes.Contains(shIndex) && !((ResIndexDetailDataPacket)dataPacket).Codes.Contains(szIndex))//指数推送数据
                SetIndexDetailDataPacket((ResIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResIndexFuturesDetailDataPacket)//股指期货
                SetIndexFuturesDetailDataPacket((ResIndexFuturesDetailDataPacket)dataPacket);
            else if (dataPacket is ResEMIndexDetailDataPacket)//东财指数推送包
                SetDetailOrgDataPacket((ResEMIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResShiborDetailDataPacket)//利率
                SetDetailOrgDataPacket((ResShiborDetailDataPacket)dataPacket);
            else if (dataPacket is ResInterBankRepurchaseDetailDataPacket)//银行间回购
                SetDetailOrgDataPacket((ResInterBankRepurchaseDetailDataPacket)dataPacket);
            else if (dataPacket is ResRateSwapDetailDataPacket)//利率互换
                SetDetailOrgDataPacket((ResRateSwapDetailDataPacket)dataPacket);
            else if (dataPacket is ResInterBankDetailDataPacket)//银行间债券
                SetDetailOrgDataPacket((ResInterBankDetailDataPacket)dataPacket);
            //else if (dataPacket is ResCSIndexDetailDataPacket)//中债指数
            //    SetDetailOrgDataPacket((ResCSIndexDetailDataPacket)dataPacket);
            //else if (dataPacket is ResCSIIndexDetailDataPacket)//中证指数
            //    SetDetailOrgDataPacket((ResCSIIndexDetailDataPacket)dataPacket);
            //else if (dataPacket is ResCNIndexDetailDataPacket)//巨潮指数
            //    SetDetailOrgDataPacket((ResCNIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResForexDetailDataPacket)//外汇
                SetDetailOrgDataPacket((ResForexDetailDataPacket)dataPacket);
            else if (dataPacket is ResUSStockDetailDataPacket)//美股
                SetDetailOrgDataPacket((ResUSStockDetailDataPacket)dataPacket);
            else if (dataPacket is ResOSFuturesDetailDataPacket)//海外期货
                SetDetailOrgDataPacket((ResOSFuturesDetailDataPacket)dataPacket);
            else if (dataPacket is ResOSFuturesLMEDetailDataPacket)//LME
                SetDetailOrgDataPacket((ResOSFuturesLMEDetailDataPacket)dataPacket);

            //else if (dataPacket is ResHisTrendlinecfsDataPacket)
            //    SetDetailDataPacket((ResHisTrendlinecfsDataPacket)dataPacket);
        }
        ///// <summary>
        ///// 请求历史分时资金流数据 响应包的处理
        ///// </summary>
        ///// <param name="dataPacket"></param>
        //private void SetTrendDataPacket(ResHisTrendlinecfsDataPacket dataPacket)
        //{ 
        //    int code=dataPacket.StockId;
        //    int date=dataPacket.Date;

        //    Dictionary<int, StockTrendCaptialFlowDataRec> dic;
        //    StockTrendCaptialFlowDataRec rec;

        //    if (!_allTrendCaptialFlowData.TryGetValue(code, out dic))           
        //        dic = new Dictionary<int, StockTrendCaptialFlowDataRec>();

        //    if (!dic.TryGetValue(date, out rec))
        //        rec = new StockTrendCaptialFlowDataRec();

        //    rec.Code = code;
        //    rec.MintData = new OneMintTrendCaptialFlowDataRec[1];           
        //}
        /// <summary>
        /// 个股走势包处理
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetTrendDataPacket(ResStockTrendDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
            if (dataPacket.TrendData == null || dataPacket.TrendData.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.TrendData.Code, out menAllDayTrendData))
            {
                OneDayTrendDataRec memOneDayTrendData;
                if (menAllDayTrendData.TryGetValue(dataPacket.TrendData.Date, out memOneDayTrendData))
                {
                    memOneDayTrendData.High = dataPacket.TrendData.High;
                    memOneDayTrendData.Low = dataPacket.TrendData.Low;
                    memOneDayTrendData.PreClose = dataPacket.TrendData.PreClose;

                    int i = 0;
                    while (i < dataPacket.MinNum)
                    {
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].Price = dataPacket.TrendData.MintDatas[i].Price;
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                        i++;
                    }
                }
                else
                    menAllDayTrendData.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
            }
            else
            {
                if (!_allTrendData.ContainsKey(dataPacket.TrendData.Code))
                {
                    Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                    tmpDic.Add(dataPacket.TrendData.Date, dataPacket.TrendData);

                    _allTrendData.Add(dataPacket.TrendData.Code, tmpDic);
                }

            }

            _allTrendData[dataPacket.TrendData.Code][dataPacket.TrendData.Date].RequestLastPoint =
                Convert.ToInt16(dataPacket.Offset + dataPacket.MinNum - 1);
        }

        private void SetTrendDataPacket(ResTrendOrgDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
            if (dataPacket.TrendData.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.TrendData.Code, out menAllDayTrendData))
            {
                OneDayTrendDataRec memOneDayTrendData;
                if (menAllDayTrendData.TryGetValue(dataPacket.TrendData.Date, out memOneDayTrendData))
                {
                    memOneDayTrendData.High = dataPacket.TrendData.High;
                    memOneDayTrendData.Low = dataPacket.TrendData.Low;
                    memOneDayTrendData.PreClose = dataPacket.TrendData.PreClose;
                    for (int i = dataPacket.FirstIndex; i < dataPacket.TrendData.MintDatas.Length; i++)
                    {
                        memOneDayTrendData.MintDatas[i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
                        memOneDayTrendData.MintDatas[i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                        memOneDayTrendData.MintDatas[i].Price = dataPacket.TrendData.MintDatas[i].Price;
                        memOneDayTrendData.MintDatas[i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                    }
                }
                else
                    menAllDayTrendData.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
            }
            else
            {
                Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                OneDayTrendDataRec tempData = new OneDayTrendDataRec(dataPacket.TrendData.Code);
                tempData.High = dataPacket.TrendData.High;
                tempData.Low = dataPacket.TrendData.Low;
                tempData.PreClose = dataPacket.TrendData.PreClose;
                // int tmpMinNum = Math.Min(dataPacket.MinNum, dataPacket.TrendData.MintDatas.Length);
                for (int i = dataPacket.FirstIndex; i < dataPacket.TrendData.MintDatas.Length; i++)
                {
                    tempData.MintDatas[i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
                    tempData.MintDatas[i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                    tempData.MintDatas[i].Price = dataPacket.TrendData.MintDatas[i].Price;
                    tempData.MintDatas[i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                }
                tmpDic.Add(dataPacket.TrendData.Date, tempData);
                _allTrendData.Add(dataPacket.TrendData.Code, tmpDic);
            }

            _allTrendData[dataPacket.TrendData.Code][dataPacket.TrendData.Date].RequestLastPoint =
                Convert.ToInt16(dataPacket.Offset + dataPacket.MinNum - 1);
        }

        //private void SetTrendDataPacket(ResTrendOrgDataPacket dataPacket)
        //{
        //    Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
        //    if (dataPacket.TrendData.Code == 0)
        //        return;
        //    if (_allTrendData.TryGetValue(dataPacket.TrendData.Code, out menAllDayTrendData))
        //    {
        //        OneDayTrendDataRec memOneDayTrendData;
        //        if (menAllDayTrendData.TryGetValue(dataPacket.TrendData.Date, out memOneDayTrendData))
        //        {
        //            memOneDayTrendData.High = dataPacket.TrendData.High;
        //            memOneDayTrendData.Low = dataPacket.TrendData.Low;
        //            memOneDayTrendData.PreClose = dataPacket.TrendData.PreClose;

        //            int i = 0;
        //            while (i < dataPacket.TrendData.MintDatas.Length && (dataPacket.Offset + i)<memOneDayTrendData.MintDatas.Length)
        //            {
        //                memOneDayTrendData.MintDatas[dataPacket.Offset + i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
        //                memOneDayTrendData.MintDatas[dataPacket.Offset + i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
        //                memOneDayTrendData.MintDatas[dataPacket.Offset + i].Price = dataPacket.TrendData.MintDatas[i].Price;
        //                memOneDayTrendData.MintDatas[dataPacket.Offset + i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
        //                i++;
        //            }
        //        }
        //        else
        //            menAllDayTrendData.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
        //    }
        //    else
        //    {
        //        Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
        //        OneDayTrendDataRec tempData = new OneDayTrendDataRec(dataPacket.TrendData.Code);
        //        tempData.High = dataPacket.TrendData.High;
        //        tempData.Low = dataPacket.TrendData.Low;
        //        tempData.PreClose = dataPacket.TrendData.PreClose;
        //       // int tmpMinNum = Math.Min(dataPacket.MinNum, dataPacket.TrendData.MintDatas.Length);
        //        for (int i = 0; i < dataPacket.TrendData.MintDatas.Length && (dataPacket.Offset + i)<tempData.MintDatas.Length; i++)
        //        {
        //            tempData.MintDatas[dataPacket.Offset + i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
        //            tempData.MintDatas[dataPacket.Offset + i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
        //            tempData.MintDatas[dataPacket.Offset + i].Price = dataPacket.TrendData.MintDatas[i].Price;
        //            tempData.MintDatas[dataPacket.Offset + i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
        //        }
        //        tmpDic.Add(dataPacket.TrendData.Date, tempData);
        //        _allTrendData.Add(dataPacket.TrendData.Code, tmpDic);
        //    }

        //    _allTrendData[dataPacket.TrendData.Code][dataPacket.TrendData.Date].RequestLastPoint =
        //        Convert.ToInt16(dataPacket.Offset + dataPacket.MinNum - 1);
        //}

        /// <summary>
        /// 外盘走势
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetOceanTrendDataPacket(ResOceanTrendDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
            if (dataPacket.TrendData.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.TrendData.Code, out menAllDayTrendData))
            {
                OneDayTrendDataRec memOneDayTrendData;
                if (menAllDayTrendData.TryGetValue(dataPacket.TrendData.Date, out memOneDayTrendData))
                {
                    memOneDayTrendData.High = dataPacket.TrendData.High;
                    memOneDayTrendData.Low = dataPacket.TrendData.Low;
                    memOneDayTrendData.PreClose = dataPacket.TrendData.PreClose;

                    int i = 0;
                    while (i < dataPacket.MinNum)
                    {
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].Price = dataPacket.TrendData.MintDatas[i].Price;
                        memOneDayTrendData.MintDatas[dataPacket.Offset + i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                        i++;
                    }
                }
                else
                    menAllDayTrendData.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
            }
            else
            {
                Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                tmpDic.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
                _allTrendData.Add(dataPacket.TrendData.Code, tmpDic);
            }

            _allTrendData[dataPacket.TrendData.Code][dataPacket.TrendData.Date].RequestLastPoint =
                Convert.ToInt16(dataPacket.Offset + dataPacket.MinNum - 1);
        }

        /// <summary>
        /// 期指走势包
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetTrendIFDataPacket(ResIndexFuturesTrendDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
            if (dataPacket.TrendData == null || dataPacket.TrendData.Code == 0)
                return;
            //股指期货重新请求（把内存中数据删除重新添加）
            if (_allTrendData.TryGetValue(dataPacket.TrendData.Code, out menAllDayTrendData))
            {
                OneDayTrendDataRec memOneDayTrendData;
                if (menAllDayTrendData.TryGetValue(dataPacket.TrendData.Date, out memOneDayTrendData))
                {
                    memOneDayTrendData.High = dataPacket.TrendData.High;
                    memOneDayTrendData.Low = dataPacket.TrendData.Low;
                    memOneDayTrendData.PreClose = dataPacket.TrendData.PreClose;

                    //int i = 0;
                    //while (i < dataPacket.MinNum - 5)
                    //{
                    //    memOneDayTrendData.MintDatas[dataPacket.Offset + i].Amount = dataPacket.TrendData.MintDatas[i].Amount;
                    //    memOneDayTrendData.MintDatas[dataPacket.Offset + i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                    //    memOneDayTrendData.MintDatas[dataPacket.Offset + i].Price = dataPacket.TrendData.MintDatas[i].Price;
                    //    memOneDayTrendData.MintDatas[dataPacket.Offset + i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                    //    ((OneMinuteIFDataRec) memOneDayTrendData.MintDatas[dataPacket.Offset + i]).OpenInterest =
                    //        ((OneMinuteIFDataRec) dataPacket.TrendData.MintDatas[i]).OpenInterest;
                    //    i++;
                    //}

                    if (dataPacket.Offset < 5)
                    {
                        for (int i = 0; i < 5 - dataPacket.Offset; i++)
                        {
                            memOneDayTrendData.PreMintDatas[i + dataPacket.Offset].Price = dataPacket.TrendData.PreMintDatas[i].Price;
                            memOneDayTrendData.PreMintDatas[i + dataPacket.Offset].Volume = dataPacket.TrendData.PreMintDatas[i].Volume;
                            memOneDayTrendData.PreMintDatas[i + dataPacket.Offset].AverPrice = dataPacket.TrendData.PreMintDatas[i].AverPrice;
                            if (dataPacket.TrendData.PreMintDatas[i] is OneMinuteIFDataRec && memOneDayTrendData.PreMintDatas[i + dataPacket.Offset] is OneMinuteIFDataRec)
                            {
                                (memOneDayTrendData.PreMintDatas[i + dataPacket.Offset] as OneMinuteIFDataRec).OpenInterest = (dataPacket.TrendData.PreMintDatas[i] as OneMinuteIFDataRec).OpenInterest;
                            }
                        }

                        for (int i = 0; i < dataPacket.MinNum - 5 + dataPacket.Offset; i++)
                        {
                            memOneDayTrendData.MintDatas[i].Price = dataPacket.TrendData.MintDatas[i].Price;
                            memOneDayTrendData.MintDatas[i].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                            memOneDayTrendData.MintDatas[i].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                            if (dataPacket.TrendData.MintDatas[i] is OneMinuteIFDataRec && memOneDayTrendData.MintDatas[i] is OneMinuteIFDataRec)
                            {
                                (memOneDayTrendData.MintDatas[i] as OneMinuteIFDataRec).OpenInterest = (dataPacket.TrendData.MintDatas[i] as OneMinuteIFDataRec).OpenInterest;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dataPacket.MinNum; i++)
                        {
                            memOneDayTrendData.MintDatas[i + dataPacket.Offset-5].Price = dataPacket.TrendData.MintDatas[i].Price;
                            memOneDayTrendData.MintDatas[i + dataPacket.Offset-5].Volume = dataPacket.TrendData.MintDatas[i].Volume;
                            memOneDayTrendData.MintDatas[i + dataPacket.Offset-5].AverPrice = dataPacket.TrendData.MintDatas[i].AverPrice;
                            if (dataPacket.TrendData.MintDatas[i] is OneMinuteIFDataRec && memOneDayTrendData.MintDatas[i + dataPacket.Offset-5] is OneMinuteIFDataRec)
                            {
                                (memOneDayTrendData.MintDatas[i + dataPacket.Offset-5] as OneMinuteIFDataRec).OpenInterest = (dataPacket.TrendData.MintDatas[i] as OneMinuteIFDataRec).OpenInterest;
                            }
                        }
                    }
                }
                else
                    menAllDayTrendData.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
            }
            else
            {
                Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                tmpDic.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
                _allTrendData.Add(dataPacket.TrendData.Code, tmpDic);
            }

            _allTrendData[dataPacket.TrendData.Code][dataPacket.TrendData.Date].RequestLastPoint =
                Convert.ToInt16(dataPacket.Offset + dataPacket.MinNum - 1);
        }

        /// <summary>
        /// Detail包处理
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetDetailDataPacket(ResStockDetailDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;
            if (dataPacket.Code == 0)
                return;
            //时间转走势下标
            int indexTrend =
                TimeUtilities.GetPointFromTime(
                    Dc.GetFieldDataInt32(dataPacket.Code,FieldIndex.Time),
                    dataPacket.Code);
            //拿到当天的交易日Date
            int date = Dc.GetTradeDate(dataPacket.Code);
            if (_allTrendData.TryGetValue(dataPacket.Code, out memAllDayTrendData))
            {
                try
                {
                    OneDayTrendDataRec memOneDayTrendData;
                    if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                    {
                        long sumVolume = 0;
                        double sumAmount = 0;
                        long sumNeiWaiCha = 0;
                        for (int i = 0; i < indexTrend; i++)
                        {
                            sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                            sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                            sumNeiWaiCha += memAllDayTrendData[date].MintDatas[i].NeiWaiCha;
                        }

                        foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                        {
                            sumVolume += temp.Volume;
                        }

                        memAllDayTrendData[date].MintDatas[indexTrend].Price =
                            Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);

                    
                            switch (Dc.GetMarketType(dataPacket.Code))
                            {
                                case MarketType.SHINDEX:
                                case MarketType.SZINDEX:
                                    memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) / 100 - sumVolume;
                                    break;
                                default:
                                    memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) - sumVolume;
                                    break;
                            }
                        
                       
                        memAllDayTrendData[date].MintDatas[indexTrend].Amount =
                            Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount) - sumAmount;

                        byte mtByteValue = 0;
                       
                            if (!SecurityAttribute.SameMarketValue.TryGetValue(Dc.GetMarketType(dataPacket.Code), out mtByteValue))
                                mtByteValue = 0;


                        if (mtByteValue != 4)
                            memAllDayTrendData[date].MintDatas[indexTrend].AverPrice =
                                Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.AveragePrice);

                        memAllDayTrendData[date].MintDatas[indexTrend].NeiWaiCha =
                            Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.RedVolume) -
                            Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.GreenVolume) - sumNeiWaiCha;
                        //TODO:需要确定买卖力道
                        //memAllDayTrendData[date].MintDatas[indexTrend].BuyVolume += Convert.ToInt32(dataPacket.TotalBuyVolume);
                        //memAllDayTrendData[date].MintDatas[indexTrend].SellVolume += Convert.ToInt32(dataPacket.TotalSellVolume);
                        memAllDayTrendData[date].High =
                            Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                        memAllDayTrendData[date].Low =
                            Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);

                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage(e.StackTrace);
                    throw;
                }

            }
            else
            {
                if (indexTrend == 0)
                {
                    Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                    OneDayTrendDataRec trendData = new OneDayTrendDataRec(dataPacket.Code);
                    trendData.Date = date;
                    trendData.Time = Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time);
                    trendData.High = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                    trendData.Low = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);
                    trendData.Open = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Open);
                    trendData.PreClose = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.PreClose);
                    trendData.PreRequestLastPoint = 0;
                    trendData.RequestLastPoint = 0;
                    trendData.Code = dataPacket.Code;
                    trendData.MintDatas[0].Price = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);
                    trendData.MintDatas[0].Amount = Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount);
                    trendData.MintDatas[0].AverPrice = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.AveragePrice);
                   
                        switch (Dc.GetMarketType(dataPacket.Code))
                        {
                            case MarketType.SHINDEX:
                            case MarketType.SZINDEX:
                                trendData.MintDatas[0].Volume =
                                    Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume)/100;
                                break;
                            default:
                                trendData.MintDatas[0].Volume =
                                    Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume);
                                break;
                        }
                    

                    tmpDic.Add(date, trendData);
                    _allTrendData.Add(dataPacket.Code, tmpDic);
                }
            }
        }

        /// <summary>
        /// Detail Level2
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetDetailDataPacket(ResStockDetailLev2DataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;
            //上证指数、深证指数
            if (dataPacket.Code == 0)
                return;

            int time = Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time);

            bool isCallQuction = TimeUtilities.IsCallAuction(dataPacket.Code, Dc.GetTradeDate(dataPacket.Code), Dc.ServerDate, Dc.ServerTime);
            //时间转走势下标
            int indexTrend =
                TimeUtilities.GetPointFromTime(
                    Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time),
                    dataPacket.Code);

            //集合竞价下标
            int indexCallAuction =
                TimeUtilities.GetCallAuctionPointFromTime(time,
                                                          dataPacket.Code);
            //拿到当天的交易日Date
            int date = Dc.GetTradeDate(dataPacket.Code);

            if (_allTrendData.TryGetValue(dataPacket.Code, out memAllDayTrendData))
            {
                try
                {
                    OneDayTrendDataRec memOneDayTrendData;
                    if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                    {
                        long sumVolume = 0;
                        double sumAmount = 0;
                        long sumNeiWaiCha = 0;


                        if (isCallQuction)//当前加到集合竞价
                        {
                            for (int i = 0; i < indexCallAuction; i++)
                            {
                                sumVolume += memAllDayTrendData[date].PreMintDatas[i].Volume;
                                sumAmount += memAllDayTrendData[date].PreMintDatas[i].Amount;
                                sumNeiWaiCha += memAllDayTrendData[date].PreMintDatas[i].NeiWaiCha;
                            }
                            memAllDayTrendData[date].PreMintDatas[indexCallAuction].Price =
                                Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);

                           
                                switch (Dc.GetMarketType(dataPacket.Code))
                                {
                                    case MarketType.SHINDEX:
                                    case MarketType.SZINDEX:
                                        memAllDayTrendData[date].PreMintDatas[indexCallAuction].Volume =
                                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) / 100 - sumVolume;
                                        break;
                                    default:
                                        memAllDayTrendData[date].PreMintDatas[indexCallAuction].Volume =
                                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) - sumVolume;
                                        break;
                                }
                            



                            memAllDayTrendData[date].PreMintDatas[indexCallAuction].Amount =
                                Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount) - sumAmount;

                            memAllDayTrendData[date].High =
                               Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                            memAllDayTrendData[date].Low =
                                 Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);
                        }
                        else//交易时间
                        {
                            for (int i = 0; i < indexTrend; i++)
                            {
                                sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                                sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                                sumNeiWaiCha += memAllDayTrendData[date].MintDatas[i].NeiWaiCha;
                            }

                            foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                            {
                                sumVolume += temp.Volume;
                            }

                            memAllDayTrendData[date].MintDatas[indexTrend].Price =
                                Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);

                           
                                switch (Dc.GetMarketType(dataPacket.Code))
                                {
                                    case MarketType.SHINDEX:
                                    case MarketType.SZINDEX:
                                        memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) / 100 - sumVolume;
                                        break;
                                    default:
                                        memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) - sumVolume;
                                        break;
                                }
                            



                            memAllDayTrendData[date].MintDatas[indexTrend].Amount =
                                Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount) - sumAmount;

                           
                                memAllDayTrendData[date].MintDatas[indexTrend].AverPrice =
                                   Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.AveragePrice);

                            //byte mtByteValue = 0;
                            //if (Dc.GetDetailData(dataPacket.Code, FieldIndex.Market) != null)
                            //{
                            //    MarketType mt = (MarketType)Dc.GetDetailData(dataPacket.Code, FieldIndex.Market);
                            //    if (!SecurityAttribute.SameMarketValue.TryGetValue(mt, out mtByteValue))
                            //        mtByteValue = 0;
                            //}

                            //if (mtByteValue != 4)
                            //    memAllDayTrendData[date].MintDatas[indexTrend].AverPrice =
                            //        Convert.ToSingle(dataPacket.PacketDetailData[FieldIndex.AveragePrice]);

                            memAllDayTrendData[date].MintDatas[indexTrend].NeiWaiCha =
                                Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.RedVolume) - Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.GreenVolume) -sumNeiWaiCha;
                            //TODO:需要确定买卖力道
                            //memAllDayTrendData[date].MintDatas[indexTrend].BuyVolume += Convert.ToInt32(dataPacket.TotalBuyVolume);
                            //memAllDayTrendData[date].MintDatas[indexTrend].SellVolume += Convert.ToInt32(dataPacket.TotalSellVolume);
                            memAllDayTrendData[date].High =
                                 Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                            memAllDayTrendData[date].Low =
                                 Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);
                        }


                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage(e.StackTrace);
                    throw;
                }

            }
            else
            {
                if (isCallQuction)
                {
                    if (indexCallAuction == 0)
                    {
                        Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                        OneDayTrendDataRec trendData = new OneDayTrendDataRec(dataPacket.Code);
                        trendData.Date = date;
                        trendData.Time = Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time);
                        trendData.High = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                        trendData.Low = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);
                        trendData.Open = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Open);
                        trendData.PreClose = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.PreClose);
                        trendData.PreRequestLastPoint = 0;
                        trendData.RequestLastPoint = 0;
                        trendData.Code = dataPacket.Code;
                        trendData.PreMintDatas[0].Price = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);
                        trendData.PreMintDatas[0].Amount = Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount);
                        trendData.PreMintDatas[0].AverPrice = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.AveragePrice);
                        
                            switch (Dc.GetMarketType(dataPacket.Code))
                            {
                                case MarketType.SHINDEX:
                                case MarketType.SZINDEX:
                                    trendData.PreMintDatas[0].Volume = Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) / 100;
                                    break;
                                default:
                                    trendData.PreMintDatas[0].Volume = Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume);
                                    break;
                            }
                        


                        tmpDic.Add(date, trendData);
                        _allTrendData.Add(dataPacket.Code, tmpDic);
                    }
                }
                else
                {
                    if (indexTrend == 0)
                    {
                        Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                        OneDayTrendDataRec trendData = new OneDayTrendDataRec(dataPacket.Code);
                        trendData.Date = date;
                        trendData.Time = Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time);
                        trendData.High = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                        trendData.Low = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);
                        trendData.Open = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Open);
                        trendData.PreClose = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.PreClose);
                        trendData.PreRequestLastPoint = 0;
                        trendData.RequestLastPoint = 0;
                        trendData.Code = dataPacket.Code;
                        trendData.MintDatas[0].Price = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);
                        trendData.MintDatas[0].Amount = Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount);
                        trendData.MintDatas[0].AverPrice = Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.AveragePrice);

                      
                            switch (Dc.GetMarketType(dataPacket.Code))
                            {
                                case MarketType.SHINDEX:
                                case MarketType.SZINDEX:
                                    trendData.MintDatas[0].Volume = Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) / 100;
                                    break;
                                default:
                                    trendData.MintDatas[0].Volume = Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume);
                                    break;
                            }
                        


                        tmpDic.Add(date, trendData);
                        _allTrendData.Add(dataPacket.Code, tmpDic);
                    }
                }

            }
        }

        /// <summary>
        /// 多档detail行情lev2
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetDetailDataPacket(ResNOrderStockDetailLev2DataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;
            Dictionary<FieldIndex, object> memDetailData;
            if (dataPacket.OrderDetailData == null)
                return;
            foreach (StockOrderDetailDataRec oneStock in dataPacket.OrderDetailData)
            {
                //上证指数、深证指数
                if (oneStock.Code == 0)
                    return;


                int time = Dc.GetFieldDataInt32(oneStock.Code, FieldIndex.Time);

                bool isCallQuction = TimeUtilities.IsCallAuction(oneStock.Code, Dc.GetTradeDate(oneStock.Code),
                    Dc.ServerDate, Dc.ServerTime);
                //时间转走势下标
                int indexTrend =
                    TimeUtilities.GetPointFromTime(
                        Dc.GetFieldDataInt32(oneStock.Code, FieldIndex.Time),
                        oneStock.Code);

                //集合竞价下标
                int indexCallAuction =
                    TimeUtilities.GetCallAuctionPointFromTime(time,
                        oneStock.Code);
                //拿到当天的交易日Date
                int date = Dc.GetTradeDate(oneStock.Code);

                if (_allTrendData.TryGetValue(oneStock.Code, out memAllDayTrendData))
                {
                    try
                    {
                        OneDayTrendDataRec memOneDayTrendData;
                        if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                        {
                            long sumVolume = 0;
                            double sumAmount = 0;
                            long sumNeiWaiCha = 0;


                            if (isCallQuction) //当前加到集合竞价
                            {
                                for (int i = 0; i < indexCallAuction; i++)
                                {
                                    sumVolume += memAllDayTrendData[date].PreMintDatas[i].Volume;
                                    sumAmount += memAllDayTrendData[date].PreMintDatas[i].Amount;
                                    sumNeiWaiCha += memAllDayTrendData[date].PreMintDatas[i].NeiWaiCha;
                                }
                                memAllDayTrendData[date].PreMintDatas[indexCallAuction].Price =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Now);


                                switch (Dc.GetMarketType(oneStock.Code))
                                {
                                    case MarketType.SHINDEX:
                                    case MarketType.SZINDEX:
                                        memAllDayTrendData[date].PreMintDatas[indexCallAuction].Volume =
                                            Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume)/100 -
                                            sumVolume;
                                        break;
                                    default:
                                        memAllDayTrendData[date].PreMintDatas[indexCallAuction].Volume =
                                            Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume) -
                                            sumVolume;
                                        break;
                                }




                                memAllDayTrendData[date].PreMintDatas[indexCallAuction].Amount =
                                    Dc.GetFieldDataDouble(oneStock.Code, FieldIndex.Amount) - sumAmount;

                                memAllDayTrendData[date].High =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.High);
                                memAllDayTrendData[date].Low =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Low);
                            }
                            else //交易时间
                            {
                                for (int i = 0; i < indexTrend; i++)
                                {
                                    sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                                    sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                                    sumNeiWaiCha += memAllDayTrendData[date].MintDatas[i].NeiWaiCha;
                                }

                                foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                                {
                                    sumVolume += temp.Volume;
                                }

                                memAllDayTrendData[date].MintDatas[indexTrend].Price =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Now);


                                switch (Dc.GetMarketType(oneStock.Code))
                                {
                                    case MarketType.SHINDEX:
                                    case MarketType.SZINDEX:
                                        memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                            Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume)/100 -
                                            sumVolume;
                                        break;
                                    default:
                                        memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                            Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume) -
                                            sumVolume;
                                        break;
                                }




                                memAllDayTrendData[date].MintDatas[indexTrend].Amount =
                                    Dc.GetFieldDataDouble(oneStock.Code, FieldIndex.Amount) - sumAmount;


                                memAllDayTrendData[date].MintDatas[indexTrend].AverPrice =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.AveragePrice);

                                //byte mtByteValue = 0;
                                //if (Dc.GetDetailData(dataPacket.Code, FieldIndex.Market) != null)
                                //{
                                //    MarketType mt = (MarketType)Dc.GetDetailData(dataPacket.Code, FieldIndex.Market);
                                //    if (!SecurityAttribute.SameMarketValue.TryGetValue(mt, out mtByteValue))
                                //        mtByteValue = 0;
                                //}

                                //if (mtByteValue != 4)
                                //    memAllDayTrendData[date].MintDatas[indexTrend].AverPrice =
                                //        Convert.ToSingle(dataPacket.PacketDetailData[FieldIndex.AveragePrice]);

                                memAllDayTrendData[date].MintDatas[indexTrend].NeiWaiCha =
                                    Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.RedVolume) -
                                    Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.GreenVolume) - sumNeiWaiCha;
                                //TODO:需要确定买卖力道
                                //memAllDayTrendData[date].MintDatas[indexTrend].BuyVolume += Convert.ToInt32(dataPacket.TotalBuyVolume);
                                //memAllDayTrendData[date].MintDatas[indexTrend].SellVolume += Convert.ToInt32(dataPacket.TotalSellVoDc.GetFieldDataInt32(oneStocklume);
                                memAllDayTrendData[date].High =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.High);
                                memAllDayTrendData[date].Low =
                                    Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Low);
                            }


                        }
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage(e.StackTrace);
                        throw;
                    }

                }
                else
                {
                    if (isCallQuction)
                    {
                        if (indexCallAuction == 0)
                        {
                            Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                            OneDayTrendDataRec trendData = new OneDayTrendDataRec(oneStock.Code);
                            trendData.Date = date;
                            trendData.Time = Dc.GetFieldDataInt32(oneStock.Code, FieldIndex.Time);
                            trendData.High = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.High);
                            trendData.Low = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Low);
                            trendData.Open = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Open);
                            trendData.PreClose = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.PreClose);
                            trendData.PreRequestLastPoint = 0;
                            trendData.RequestLastPoint = 0;
                            trendData.Code = oneStock.Code;
                            trendData.PreMintDatas[0].Price =
                                Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Now);
                            trendData.PreMintDatas[0].Amount =
                                Dc.GetFieldDataDouble(oneStock.Code, FieldIndex.Amount);
                            trendData.PreMintDatas[0].AverPrice =
                                Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.AveragePrice);

                            switch (Dc.GetMarketType(oneStock.Code))
                            {
                                case MarketType.SHINDEX:
                                case MarketType.SZINDEX:
                                    trendData.PreMintDatas[0].Volume =
                                        Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume)/100;
                                    break;
                                default:
                                    trendData.PreMintDatas[0].Volume =
                                        Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume);
                                    break;
                            }



                            tmpDic.Add(date, trendData);
                            _allTrendData.Add(oneStock.Code, tmpDic);
                        }
                    }
                    else
                    {
                        if (indexTrend == 0)
                        {
                            Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                            OneDayTrendDataRec trendData = new OneDayTrendDataRec(oneStock.Code);
                            trendData.Date = date;
                            trendData.Time = Dc.GetFieldDataInt32(oneStock.Code, FieldIndex.Time);
                            trendData.High = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.High);
                            trendData.Low = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Low);
                            trendData.Open = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Open);
                            trendData.PreClose = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.PreClose);
                            trendData.PreRequestLastPoint = 0;
                            trendData.RequestLastPoint = 0;
                            trendData.Code = oneStock.Code;
                            trendData.MintDatas[0].Price = Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.Now);
                            trendData.MintDatas[0].Amount =
                                Dc.GetFieldDataDouble(oneStock.Code, FieldIndex.Amount);
                            trendData.MintDatas[0].AverPrice =
                                Dc.GetFieldDataSingle(oneStock.Code, FieldIndex.AveragePrice);


                            switch (Dc.GetMarketType(oneStock.Code))
                            {
                                case MarketType.SHINDEX:
                                case MarketType.SZINDEX:
                                    trendData.MintDatas[0].Volume =
                                        Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume)/100;
                                    break;
                                default:
                                    trendData.MintDatas[0].Volume =
                                        Dc.GetFieldDataInt64(oneStock.Code, FieldIndex.Volume);
                                    break;
                            }



                            tmpDic.Add(date, trendData);
                            _allTrendData.Add(oneStock.Code, tmpDic);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Detail Level2
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetDetailOrgDataPacket(OrgDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;
            Dictionary<FieldIndex, object> tempObj;
            int tempCode = 0;
            if (dataPacket is ResEMIndexDetailDataPacket)//东财指数推送包
                tempCode = (dataPacket as ResEMIndexDetailDataPacket).Code;
            else if (dataPacket is ResShiborDetailDataPacket)//利率
                tempCode = (dataPacket as ResShiborDetailDataPacket).Code;
            else if (dataPacket is ResInterBankRepurchaseDetailDataPacket)//银行间回购
                tempCode = (dataPacket as ResInterBankRepurchaseDetailDataPacket).Code;
            else if (dataPacket is ResRateSwapDetailDataPacket)//利率互换
                tempCode = (dataPacket as ResRateSwapDetailDataPacket).Code;
            else if (dataPacket is ResInterBankDetailDataPacket)//银行间债券
                tempCode = (dataPacket as ResInterBankDetailDataPacket).Code;
            else if (dataPacket is ResCSIndexDetailDataPacket)//中债指数
                tempCode = (dataPacket as ResCSIndexDetailDataPacket).Code;
            else if (dataPacket is ResCSIIndexDetailDataPacket)//中证指数
                tempCode = (dataPacket as ResCSIIndexDetailDataPacket).Code;
            else if (dataPacket is ResCNIndexDetailDataPacket)//巨潮指数
                tempCode = (dataPacket as ResCNIndexDetailDataPacket).Code;
            else if (dataPacket is ResForexDetailDataPacket)//外汇
                tempCode = (dataPacket as ResForexDetailDataPacket).Code;
            else if (dataPacket is ResUSStockDetailDataPacket)//美股
                tempCode = (dataPacket as ResUSStockDetailDataPacket).Code;
            else if (dataPacket is ResOSFuturesDetailDataPacket)//海外期货
                tempCode = (dataPacket as ResOSFuturesDetailDataPacket).Code;
            else if (dataPacket is ResOSFuturesLMEDetailDataPacket)//LME
                tempCode = (dataPacket as ResOSFuturesLMEDetailDataPacket).Code;


            int time = 0;

            time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time);
            if (time == 0)
                return;
            bool isCallQuction = TimeUtilities.IsCallAuction(tempCode, Dc.GetTradeDate(tempCode), Dc.ServerDate, Dc.ServerTime);
            //时间转走势下标
            int indexTrend = TimeUtilities.GetPointFromTime(Dc.GetFieldDataInt32(tempCode, FieldIndex.Time), tempCode);
            //集合竞价下标
            int indexCallAuction =TimeUtilities.GetCallAuctionPointFromTime(time,tempCode);
            //拿到当天的交易日Date
            int date = Dc.GetTradeDate(tempCode);
            if (_allTrendData.TryGetValue(tempCode, out memAllDayTrendData))
            {
                try
                {
                    OneDayTrendDataRec memOneDayTrendData;
                    if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                    {
                        long sumVolume = 0;
                        double sumAmount = 0;
                        long sumNeiWaiCha = 0;
                        if (isCallQuction)//当前加到集合竞价
                        {
                            for (int i = 0; i < indexCallAuction; i++)
                            {
                                sumVolume += memAllDayTrendData[date].PreMintDatas[i].Volume;
                                sumAmount += memAllDayTrendData[date].PreMintDatas[i].Amount;
                                sumNeiWaiCha += memAllDayTrendData[date].PreMintDatas[i].NeiWaiCha;
                            }
                            memAllDayTrendData[date].PreMintDatas[indexCallAuction].Price =Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);

                                memAllDayTrendData[date].PreMintDatas[indexCallAuction].Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume) - sumVolume;

                                memAllDayTrendData[date].PreMintDatas[indexCallAuction].Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount) - sumAmount;
                                memAllDayTrendData[date].High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                                memAllDayTrendData[date].Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        }
                        else//交易时间
                        {
                            for (int i = 0; i < indexTrend; i++)
                            {
                                sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                                sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                                sumNeiWaiCha += memAllDayTrendData[date].MintDatas[i].NeiWaiCha;
                            }

                            foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                            {
                                sumVolume += temp.Volume;
                            }

                            memAllDayTrendData[date].MintDatas[indexTrend].Price =Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);

                            memAllDayTrendData[date].MintDatas[indexTrend].Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume) -sumVolume;

                                memAllDayTrendData[date].MintDatas[indexTrend].Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount) -sumAmount;


                            memAllDayTrendData[date].MintDatas[indexTrend].AverPrice = Dc.GetFieldDataSingle(tempCode, FieldIndex.AveragePrice);

                                memAllDayTrendData[date].MintDatas[indexTrend].NeiWaiCha =
                                    Dc.GetFieldDataInt64(tempCode, FieldIndex.RedVolume) -Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume) -
                                   Dc.GetFieldDataInt64(tempCode, FieldIndex.GreenVolume) - sumNeiWaiCha;

                                memAllDayTrendData[date].High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);

                                memAllDayTrendData[date].Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage(e.StackTrace);
                }

            }
            else
            {
                if (isCallQuction)
                {
                    if (indexCallAuction == 0)
                    {
                        Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                        OneDayTrendDataRec trendData = new OneDayTrendDataRec(tempCode);
                        trendData.Date = date;
                        trendData.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time);
                        trendData.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                        trendData.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        trendData.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                        trendData.PreClose = Dc.GetFieldDataSingle(tempCode,FieldIndex.PreClose);
                        trendData.PreRequestLastPoint = 0;
                        trendData.RequestLastPoint = 0;
                        trendData.Code = tempCode;
                        trendData.PreMintDatas[0].Price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);

                        trendData.PreMintDatas[0].Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount);

                        trendData.PreMintDatas[0].AverPrice = Dc.GetFieldDataSingle(tempCode, FieldIndex.AveragePrice);

                        trendData.PreMintDatas[0].Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume);
                        tmpDic.Add(date, trendData);
                        _allTrendData.Add(tempCode, tmpDic);
                    }
                }
                else
                {
                    if (indexTrend == 0)
                    {
                        Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                        OneDayTrendDataRec trendData = new OneDayTrendDataRec(tempCode);
                        trendData.Date = date;
                        trendData.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time);
                        trendData.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                        trendData.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        trendData.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                        trendData.PreClose = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                        trendData.PreRequestLastPoint = 0;
                        trendData.RequestLastPoint = 0;
                        trendData.Code = tempCode;
                        trendData.MintDatas[0].Price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                            trendData.MintDatas[0].Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount);

                        trendData.MintDatas[0].AverPrice = Dc.GetFieldDataSingle(tempCode, FieldIndex.AveragePrice);

                        trendData.MintDatas[0].Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume);
                        tmpDic.Add(date, trendData);
                        _allTrendData.Add(tempCode, tmpDic);
                    }
                }

            }
        }

        /// <summary>
        /// 外盘Record
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetOceanRecordDataPacket(ResOceanRecordDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;
            if (dataPacket.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.Code, out memAllDayTrendData))
            {
                //时间转走势下标
                int indexTrend =
                    TimeUtilities.GetPointFromTime(
                        Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time),
                        dataPacket.Code);

                //拿到当天的交易日Date
                int date = Dc.GetTradeDate(dataPacket.Code);

                OneDayTrendDataRec memOneDayTrendData;
                if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                {
                    long sumVolume = 0;
                    double sumAmount = 0;
                    long sumNeiWaiCha = 0;
                    for (int i = 0; i < indexTrend; i++)
                    {
                        sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                        sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                        sumNeiWaiCha += memAllDayTrendData[date].MintDatas[i].NeiWaiCha;
                    }

                    foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                    {
                        sumVolume += temp.Volume;
                    }

                    memAllDayTrendData[date].MintDatas[indexTrend].Price =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);
                    
                        switch (Dc.GetMarketType(dataPacket.Code))
                        {
                            case MarketType.SHINDEX:
                            case MarketType.SZINDEX:
                                memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                         Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) /100 - sumVolume;
                                break;
                            default:
                                memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) - sumVolume;
                                break;
                        }
                    
                    
                    memAllDayTrendData[date].MintDatas[indexTrend].Amount =
                        Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount) - sumAmount;
                    memAllDayTrendData[date].MintDatas[indexTrend].AverPrice =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.AveragePrice);
                    memAllDayTrendData[date].MintDatas[indexTrend].NeiWaiCha =
                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.RedVolume) - Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.GreenVolume) - sumNeiWaiCha;
                    memAllDayTrendData[date].High =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                    memAllDayTrendData[date].Low =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);
                    //TODO:需要确定买卖力道
                }
            }
        }

        /// <summary>
        /// 股指期货推送
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetIndexFuturesDetailDataPacket(ResIndexFuturesDetailDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;
            if (dataPacket.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.Code, out memAllDayTrendData))
            {
                //时间转走势下标
                int indexTrend =
                    TimeUtilities.GetPointFromTime(
                        Dc.GetFieldDataInt32(dataPacket.Code, FieldIndex.Time),
                        dataPacket.Code);

                //拿到当天的交易日Date
                int date = Dc.GetTradeDate(dataPacket.Code);

                OneDayTrendDataRec memOneDayTrendData;
                if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                {
                    long sumVolume = 0;
                    double sumAmount = 0;
                    long sumNeiWaiCha = 0;
                    for (int i = 0; i < indexTrend; i++)
                    {
                        sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                        sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                        sumNeiWaiCha += memAllDayTrendData[date].MintDatas[i].NeiWaiCha;
                    }

                    foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                    {
                        sumVolume += temp.Volume;
                    }

                    memAllDayTrendData[date].MintDatas[indexTrend].Price =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Now);


                    switch (Dc.GetMarketType(dataPacket.Code))
                    {
                        case MarketType.SHINDEX:
                        case MarketType.SZINDEX:
                            memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume)/100 - sumVolume;
                            break;
                        default:
                            memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.Volume) - sumVolume;
                            break;
                    }

                    memAllDayTrendData[date].MintDatas[indexTrend].Amount =
                        Dc.GetFieldDataDouble(dataPacket.Code, FieldIndex.Amount) - sumAmount;
                    memAllDayTrendData[date].MintDatas[indexTrend].NeiWaiCha =
                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.RedVolume) -
                        Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.GreenVolume) - sumNeiWaiCha;
                    memAllDayTrendData[date].High =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.High);
                    memAllDayTrendData[date].Low =
                        Dc.GetFieldDataSingle(dataPacket.Code, FieldIndex.Low);
                    ((OneMinuteIFDataRec) memAllDayTrendData[date].MintDatas[indexTrend]).OpenInterest =
                        (int) Dc.GetFieldDataInt64(dataPacket.Code, FieldIndex.OpenInterest);
                    //TODO:需要确定买卖力道
                }
            }
        }

        /// <summary>
        /// 指数推送
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetIndexDetailDataPacket(ResIndexDetailDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> memAllDayTrendData;

            foreach (int code in dataPacket.Codes)
            {
                if (_allTrendData.TryGetValue(code, out memAllDayTrendData))
                {
                    //时间转走势下标
                    int indexTrend =
                        TimeUtilities.GetPointFromTime(
                            Dc.GetFieldDataInt32(code, FieldIndex.Time),
                            code);

                    //拿到当天的交易日Date
                    int date = Dc.GetTradeDate(code);

                    OneDayTrendDataRec memOneDayTrendData;
                    if (memAllDayTrendData.TryGetValue(date, out memOneDayTrendData))
                    {
                        long sumVolume = 0;
                        double sumAmount = 0;
                        for (int i = 0; i < indexTrend; i++)
                        {
                            sumVolume += memAllDayTrendData[date].MintDatas[i].Volume;
                            sumAmount += memAllDayTrendData[date].MintDatas[i].Amount;
                        }

                        foreach (OneMinuteDataRec temp in memAllDayTrendData[date].PreMintDatas)
                        {
                            sumVolume += temp.Volume;
                        }

                        memAllDayTrendData[date].MintDatas[indexTrend].Price =
                            Dc.GetFieldDataSingle(code, FieldIndex.Now);


                        switch (Dc.GetMarketType(code))
                        {
                            case MarketType.SHINDEX:
                            case MarketType.SZINDEX:
                                memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                    Dc.GetFieldDataInt64(code, FieldIndex.IndexVolume)/100 - sumVolume;
                                break;
                            default:
                                memAllDayTrendData[date].MintDatas[indexTrend].Volume =
                                    Dc.GetFieldDataInt64(code, FieldIndex.IndexVolume) - sumVolume;
                                break;
                        }


                        memAllDayTrendData[date].MintDatas[indexTrend].Amount =
                            Dc.GetFieldDataDouble(code, FieldIndex.IndexAmount) - sumAmount;

                        if (Dc.GetFieldDataSingle(code, FieldIndex.Now) > memAllDayTrendData[date].High)
                            memAllDayTrendData[date].High = Dc.GetFieldDataSingle(code, FieldIndex.Now);
                        if (Dc.GetFieldDataSingle(code, FieldIndex.Now) < memAllDayTrendData[date].Low)
                            memAllDayTrendData[date].Low = Dc.GetFieldDataSingle(code, FieldIndex.Now);
                    }
                }
            }

        }

        /// <summary>
        /// 委买委卖包
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetAskBidDataPacket(ResTrendAskBidDataPacket dataPacket)
        {
            try
            {
                Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
                if (dataPacket.Code == 0)
                    return;
                if (_allTrendData.TryGetValue(dataPacket.Code, out menAllDayTrendData))
                {
                    OneDayTrendDataRec memOneDayTrendData;
                    if (menAllDayTrendData.TryGetValue(Dc.GetTradeDate(dataPacket.Code), out memOneDayTrendData))
                    {
                        int i = 0;
                        while (i < dataPacket.Num)
                        {
                            memOneDayTrendData.MintDatas[dataPacket.IndexOffset + i].SellVolume = dataPacket.SellVolumes[i];
                            memOneDayTrendData.MintDatas[dataPacket.IndexOffset + i].BuyVolume = dataPacket.BuyVolumes[i];
                            i++;
                        }
                    }
                    else
                    {
                        OneDayTrendDataRec oneDayTrend = new OneDayTrendDataRec(dataPacket.Code);
                        int i = 0;
                        while (i < dataPacket.Num)
                        {
                            oneDayTrend.MintDatas[dataPacket.IndexOffset + i].SellVolume = dataPacket.SellVolumes[i];
                            oneDayTrend.MintDatas[dataPacket.IndexOffset + i].BuyVolume = dataPacket.BuyVolumes[i];
                            i++;
                        }
                        menAllDayTrendData.Add(Dc.GetTradeDate(dataPacket.Code), oneDayTrend);
                    }
                }
                else
                {
                    OneDayTrendDataRec oneDayTrend = new OneDayTrendDataRec(dataPacket.Code);
                    int i = 0;
                    while (i < dataPacket.Num)
                    {
                        oneDayTrend.MintDatas[dataPacket.IndexOffset + i].SellVolume = dataPacket.SellVolumes[i];
                        oneDayTrend.MintDatas[dataPacket.IndexOffset + i].BuyVolume = dataPacket.BuyVolumes[i];
                        i++;
                    }

                    Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                    tmpDic.Add(Dc.GetTradeDate(dataPacket.Code), oneDayTrend);
                    if (!_allTrendData.ContainsKey(dataPacket.Code))
                        _allTrendData.Add(dataPacket.Code, tmpDic);
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// 内外盘差
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetInOutDiffDataPacket(ResTrendInOutDiffDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
            if (dataPacket.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.Code, out menAllDayTrendData))
            {
                OneDayTrendDataRec memOneDayTrendData;
                if (menAllDayTrendData.TryGetValue(Dc.GetTradeDate(dataPacket.Code), out memOneDayTrendData))
                {
                    int i = 0;
                    while (i < dataPacket.Num)
                    {
                        memOneDayTrendData.MintDatas[dataPacket.IndexOffset + i].NeiWaiCha = dataPacket.InOutDiff[i];
                        i++;
                    }
                }
                else
                {
                    OneDayTrendDataRec oneDayTrend = new OneDayTrendDataRec(dataPacket.Code);
                    int i = 0;
                    while (i < dataPacket.Num)
                    {
                        oneDayTrend.MintDatas[dataPacket.IndexOffset + i].NeiWaiCha = dataPacket.InOutDiff[i];
                        i++;
                    }
                    menAllDayTrendData.Add(Dc.GetTradeDate(dataPacket.Code), oneDayTrend);
                }
            }
            else
            {
                OneDayTrendDataRec oneDayTrend = new OneDayTrendDataRec(dataPacket.Code);
                int i = 0;
                while (i < dataPacket.Num)
                {
                    oneDayTrend.MintDatas[dataPacket.IndexOffset + i].NeiWaiCha = dataPacket.InOutDiff[i];
                    i++;
                }

                Dictionary<int, OneDayTrendDataRec> tmpDic = new Dictionary<int, OneDayTrendDataRec>(1);
                tmpDic.Add(Dc.GetTradeDate(dataPacket.Code), oneDayTrend);
                _allTrendData.Add(dataPacket.Code, tmpDic);
            }
        }

        /// <summary>
        /// 历史走势
        /// </summary>
        /// <param name="dataPacket"></param>
        private void SetHisTrendDataPacket(ResHisTrendDataPacket dataPacket)
        {
            Dictionary<int, OneDayTrendDataRec> menAllDayTrendData;
            if (dataPacket.TrendData.Code == 0)
                return;
            if (_allTrendData.TryGetValue(dataPacket.TrendData.Code, out menAllDayTrendData))
            {
                OneDayTrendDataRec memOneDayTrendData;
                if (menAllDayTrendData.TryGetValue(dataPacket.TrendData.Date, out memOneDayTrendData))
                    menAllDayTrendData[dataPacket.TrendData.Date] = dataPacket.TrendData;
                else
                    menAllDayTrendData.Add(dataPacket.TrendData.Date, dataPacket.TrendData);
            }
            else
            {
                Dictionary<int, OneDayTrendDataRec> tmp = new Dictionary<int, OneDayTrendDataRec>(1);
                tmp.Add(dataPacket.TrendData.Date,dataPacket.TrendData);
                _allTrendData.Add(dataPacket.TrendData.Code, tmp);
            }
        }

        private void SetRedGreenDataPacket(ResRedGreenDataPacket dataPacket)
        {
            OneDayRedGreenDataRec menRedGreenData;
            if (dataPacket.Code == 0)
                return;
            if (AllRedGreenData.TryGetValue(dataPacket.Code, out menRedGreenData))
            {
                menRedGreenData.MintDatas = dataPacket.RedGreenDatas;
                menRedGreenData.Code = dataPacket.Code;
                menRedGreenData.Date = dataPacket.Date;
                menRedGreenData.Time = dataPacket.Time;
            }
            else
            {
                menRedGreenData = new OneDayRedGreenDataRec(dataPacket.Code);
                menRedGreenData.MintDatas = dataPacket.RedGreenDatas;
                menRedGreenData.Code = dataPacket.Code;
                menRedGreenData.Date = dataPacket.Date;
                menRedGreenData.Time = dataPacket.Time;
                AllRedGreenData.Add(dataPacket.Code, menRedGreenData);
            }
        }

        private void SetTrendCaptialFlowDataPacket(ResTrendCaptialFlowDataPacket dataPacket)
        {
            if (dataPacket == null || dataPacket.Data == null)
                return;
            Dictionary<int, StockTrendCaptialFlowDataRec> memData;
            if(_allTrendCaptialFlowData.TryGetValue(dataPacket.Code,out memData))
            {
                StockTrendCaptialFlowDataRec todayMemData;
                if (memData.TryGetValue(Dc.GetTradeDate(dataPacket.Code), out todayMemData))
                {
                    for (int i = 0; i < dataPacket.Num; i++)
                    {
                        if(i < dataPacket.Data.MintData.Length && ((i + dataPacket.Offset) < todayMemData.MintData.Length))
                            todayMemData.MintData[i + dataPacket.Offset] = dataPacket.Data.MintData[i];
                    }
                }
                else
                {
                    memData[Dc.GetTradeDate(dataPacket.Code)] = dataPacket.Data;
                }
                
            }
            else
            {
                memData = new Dictionary<int, StockTrendCaptialFlowDataRec>(1);
                memData[Dc.GetTradeDate(dataPacket.Code)] = dataPacket.Data;
                _allTrendCaptialFlowData[dataPacket.Code] = memData;
            }
        }

    }
}

