using System;
using System.Collections.Generic;
 
using System.Text;
using EmQComm;
using System.Collections;

namespace EmQDataCore
{
    /// <summary>
    /// KLineDataTable
    /// </summary>
    public class KLineDataTable : DataTableBase
    {
        public  delegate void MinutesDataRequestHandler();
        public static event MinutesDataRequestHandler MinutesDataRequestEvent;
        private Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>> _allKLineData;
        private Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>> _todayKLineData;
        private Dictionary<int, List<List<OneDivideRightBase>>> _divideRightData;
        private Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>> _fundAfterDivideKlineData;
        private Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>> _capitalFlowDayKLineData;
        private Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>> _capitalFlowTodayKLineData;

        /// <summary>
        /// K线是否要向后请求
        /// </summary>
        public Dictionary<int, Dictionary<KLineCycle, bool>> KLineReqBack;

        /// <summary>
        /// 资金流日K线数据
        /// </summary>
        public Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>> CapitalFlowDayKLineData
        {
            get { return _capitalFlowDayKLineData; }      
        }
        /// <summary>
        /// 当日资金流日K线数据
        /// </summary>
        public Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>> CapitalFlowTodayKLineData
        {
            get { return _capitalFlowTodayKLineData; }
        }

        /// <summary>
        /// 内存中所有历史k线的数据
        /// </summary>
        public Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>> AllKLineData { get { return _allKLineData; } }

        /// <summary>
        /// 当日K线
        /// </summary>
        public Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>> TodayKLineData { get { return _todayKLineData; } }

        /// <summary>
        /// 除复权
        /// </summary>
        public Dictionary<int, List<List<OneDivideRightBase>>> DivideRightData { get { return _divideRightData; } }

        /// <summary>
        /// 净值后复权
        /// </summary>
        public Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>> FundAfterDivideKlineData { get { return _fundAfterDivideKlineData; } }
        /// <summary>
        /// 构造函数
        /// </summary>
        public KLineDataTable()
        {
            _allKLineData = new Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>>();
            _todayKLineData = new Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>>();
            _divideRightData = new Dictionary<int, List<List<OneDivideRightBase>>>(0);
            _fundAfterDivideKlineData = new Dictionary<int, Dictionary<KLineCycle, OneStockKLineDataRec>>();
            _capitalFlowDayKLineData = new Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>>();
            _capitalFlowTodayKLineData=new Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>>();
            KLineReqBack = new Dictionary<int, Dictionary<KLineCycle, bool>>(1);
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResMinKLineDataPacket)
                SetMinKLineData((ResMinKLineDataPacket)dataPacket);
            else if (dataPacket is ResHisKLineDataPacket)
                SetHisKLineData((ResHisKLineDataPacket)dataPacket);
            else if (dataPacket is ResHisKLineOrgDataPacket)
                SetHisKLineData((ResHisKLineOrgDataPacket)dataPacket);
            else if (dataPacket is ResMintKLineOrgDataPacket)
                SetHisKLineData((ResMintKLineOrgDataPacket)dataPacket);
            else if (dataPacket is ResDivideRightOrgDataPacket)
                SetDivideRightData((ResDivideRightOrgDataPacket)dataPacket);
            else if (dataPacket is ResFundKLineAfterDivideDataPacket)
                SetFundKLineData((ResFundKLineAfterDivideDataPacket)dataPacket);

            else if(dataPacket is ResNOrderStockDetailLev2DataPacket)
                SetTodayKLineData((ResNOrderStockDetailLev2DataPacket)dataPacket);
            else if (dataPacket is ResStockDetailLev2DataPacket)//level2推送包
                SetTodayKLineData((ResStockDetailLev2DataPacket)dataPacket);
            else if (dataPacket is ResStockDetailDataPacket)//level1推送包
                SetTodayKLineData((ResStockDetailDataPacket)dataPacket);
            else if (dataPacket is ResIndexDetailDataPacket)//指数推送包
                SetTodayKLineData((ResIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResIndexFuturesDetailDataPacket)//股指期货推送包
                SetTodayKLineData((ResIndexFuturesDetailDataPacket)dataPacket);
            else if (dataPacket is ResOceanRecordDataPacket)//外盘推送包
                SetTodayKLineData((ResOceanRecordDataPacket)dataPacket);
            else if (dataPacket is ResEMIndexDetailDataPacket)//东财指数推送包
                SetTodayKLineOrgData((ResEMIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResShiborDetailDataPacket)//利率
                SetTodayKLineOrgData((ResShiborDetailDataPacket)dataPacket);
            else if (dataPacket is ResInterBankRepurchaseDetailDataPacket)//银行间回购
                SetTodayKLineOrgData((ResInterBankRepurchaseDetailDataPacket)dataPacket);
            else if (dataPacket is ResRateSwapDetailDataPacket)//利率互换
                SetTodayKLineOrgData((ResRateSwapDetailDataPacket)dataPacket);
            else if (dataPacket is ResInterBankDetailDataPacket)//银行间债券
                SetTodayKLineOrgData((ResInterBankDetailDataPacket)dataPacket);
            //else if (dataPacket is ResCSIndexDetailDataPacket)//中债指数
            //    SetTodayKLineOrgData((ResCSIndexDetailDataPacket)dataPacket);
            //else if (dataPacket is ResCSIIndexDetailDataPacket)//中证指数
            //    SetTodayKLineOrgData((ResCSIIndexDetailDataPacket)dataPacket);
            //else if (dataPacket is ResCNIndexDetailDataPacket)//巨潮指数
            //    SetTodayKLineOrgData((ResCNIndexDetailDataPacket)dataPacket);
             else if (dataPacket is ResForexDetailDataPacket)//外汇
                SetTodayKLineOrgData((ResForexDetailDataPacket)dataPacket);
            else if (dataPacket is ResUSStockDetailDataPacket)//美股
                SetTodayKLineOrgData((ResUSStockDetailDataPacket)dataPacket);
            else if (dataPacket is ResOSFuturesDetailDataPacket)//海外期货
                SetTodayKLineOrgData((ResOSFuturesDetailDataPacket)dataPacket);
            else if (dataPacket is ResOSFuturesLMEDetailDataPacket)//LME
                SetTodayKLineOrgData((ResOSFuturesLMEDetailDataPacket)dataPacket);
            else if (dataPacket is ResCapitalFlowDayDataPacket)
                SetCapitalFlowDayKLineOrgData((ResCapitalFlowDayDataPacket)dataPacket);
            else if (dataPacket is ResCapitalFlowDataPacket)
                SetCapitalFlowTodayData((ResCapitalFlowDataPacket)dataPacket);
        }
        private void SetCapitalFlowDayKLineOrgData(ResCapitalFlowDayDataPacket packetData)
        {
            int code =0;
            Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> dic = null;
            if (packetData != null && packetData.DicCapitalFlowDayData != null)
            {
                code = packetData.Code;
                if (!_capitalFlowDayKLineData.TryGetValue(code, out dic))
                {                   
                    dic = new Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>();
                    _capitalFlowDayKLineData[code] = dic;
                }
                foreach (KeyValuePair<KLineCycle, CapitalFlowDayKLineDataRecs> item in packetData.DicCapitalFlowDayData)
                {
                    dic[item.Key] = item.Value;
                }
                //修改历史数据（填充当日）
                Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> todayData = null;
                if (!_capitalFlowTodayKLineData.TryGetValue(code, out todayData))
                    return;
                int tradeDate = Dc.GetTradeDate(Dc.GetMarketType(code));
                dic[KLineCycle.CycleDay].CaptialFlowList.Add(todayData[KLineCycle.CycleDay].SortDicCaptialFlowList[tradeDate]);
                dic[KLineCycle.CycleDay].SortDicCaptialFlowList.Add(tradeDate, todayData[KLineCycle.CycleDay].SortDicCaptialFlowList[tradeDate]);
            }
        }

        private void SetCapitalFlowTodayData(ResCapitalFlowDataPacket dataPacket)
        {
            int code = 0;
            Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> dic = null;
            if (dataPacket != null && dataPacket.CapitalFlowData != null)
            {
                code = dataPacket.CapitalFlowData.Code;
                int tradeDate = Dc.GetTradeDate(Dc.GetMarketType(code));
                if (!_capitalFlowTodayKLineData.TryGetValue(code, out dic))
                {
                    dic = new Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>();
                    CapitalFlowDayKLineDataRecs tempCycleDay = new CapitalFlowDayKLineDataRecs();
                    TrendCaptialFlowDataRec tempRec = new TrendCaptialFlowDataRec();

                    for (int i = 0; i < dataPacket.CapitalFlowData.FlowItems.Length; i++)
                    {
                        tempRec.BuyAmount[i] = dataPacket.CapitalFlowData.FlowItems[i].AmountBuy;
                        tempRec.SellAmount[i] = dataPacket.CapitalFlowData.FlowItems[i].AmountSell;
                        tempRec.BuyNum[i] = dataPacket.CapitalFlowData.FlowItems[i].BishuBuy;
                        tempRec.SellNum[i] = dataPacket.CapitalFlowData.FlowItems[i].BishuSell;
                        tempRec.BuyVolume[i] = (ulong) dataPacket.CapitalFlowData.FlowItems[i].VolumeBuy;
                        tempRec.SellVolume[i] = (ulong) dataPacket.CapitalFlowData.FlowItems[i].VolumeSell;
                    }
                    tempCycleDay.CaptialFlowList.Add(tempRec);
                    tempCycleDay.SortDicCaptialFlowList.Add(tradeDate, tempRec);
                    dic.Add(KLineCycle.CycleDay, tempCycleDay);
                }
                else
                {
                    for (int i = 0; i < dataPacket.CapitalFlowData.FlowItems.Length; i++)
                    {
                        dic[KLineCycle.CycleDay].CaptialFlowList[0].BuyAmount[i] = dataPacket.CapitalFlowData.FlowItems[i].AmountBuy;
                        dic[KLineCycle.CycleDay].CaptialFlowList[0].SellAmount[i] = dataPacket.CapitalFlowData.FlowItems[i].AmountSell;
                        dic[KLineCycle.CycleDay].CaptialFlowList[0].BuyNum[i] = dataPacket.CapitalFlowData.FlowItems[i].BishuBuy;
                        dic[KLineCycle.CycleDay].CaptialFlowList[0].SellNum[i] = dataPacket.CapitalFlowData.FlowItems[i].BishuSell;
                        dic[KLineCycle.CycleDay].CaptialFlowList[0].BuyVolume[i] = (ulong)dataPacket.CapitalFlowData.FlowItems[i].VolumeBuy;
                        dic[KLineCycle.CycleDay].CaptialFlowList[0].SellVolume[i] = (ulong)dataPacket.CapitalFlowData.FlowItems[i].VolumeSell;
                    }
                    dic[KLineCycle.CycleDay].SortDicCaptialFlowList[tradeDate] =
                        dic[KLineCycle.CycleDay].CaptialFlowList[0];
                }
                //修改内存中数据
                Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> totalData = null;
                if (!_capitalFlowDayKLineData.TryGetValue(code, out totalData))
                    return;
                //修改当日的值
                if (totalData[KLineCycle.CycleDay].SortDicCaptialFlowList.ContainsKey(tradeDate))
                {
                    totalData[KLineCycle.CycleDay].SortDicCaptialFlowList[tradeDate] =
                        dic[KLineCycle.CycleDay].SortDicCaptialFlowList[tradeDate];
                }
                else//不包括当日时，将当日数据插入到最后
                {
                    totalData[KLineCycle.CycleDay].SortDicCaptialFlowList.Add(tradeDate, dic[KLineCycle.CycleDay].SortDicCaptialFlowList[tradeDate]);
                }
            }
            //GC.Collect();
        }


        private ResHisKLineDataPacket GetYearKLineDataPacket(OneStockKLineDataRec packetData)
        {
            ResHisKLineDataPacket yearPacket = new ResHisKLineDataPacket();
            OneStockKLineDataRec oneStockKLineYear = new OneStockKLineDataRec();
            oneStockKLineYear.Code = packetData.Code;
            oneStockKLineYear.Cycle = KLineCycle.CycleYear;
            foreach (OneDayDataRec oneStockKLineDataRec in packetData.OneDayDataList)
            {
                int yearPacketTime = oneStockKLineDataRec.Date / 10000;
                int indexFind = -1;
                if (oneStockKLineYear.OneDayDataList.Count == 0)
                    oneStockKLineYear.OneDayDataList.Add(oneStockKLineDataRec.DeepCopy());
                else
                {
                    indexFind = -1;
                    for (int i = 0; i < oneStockKLineYear.OneDayDataList.Count; i++)
                    {
                        int yearMem = oneStockKLineYear.OneDayDataList[i].Date / 10000;
                        if (yearMem == yearPacketTime)
                        {
                            indexFind = i;
                            break;
                        }
                        if (yearMem > yearPacketTime)
                        {
                            indexFind = i;
                            break;
                        }

                    }
                    if (indexFind >= 0 && indexFind < oneStockKLineYear.OneDayDataList.Count)
                    {
                        int yearMem = oneStockKLineYear.OneDayDataList[indexFind].Date / 10000;
                        if (oneStockKLineYear.OneDayDataList[indexFind].Date == oneStockKLineDataRec.Date)
                            break;
                        if ((yearMem == yearPacketTime) && (oneStockKLineDataRec.Date > oneStockKLineYear.OneDayDataList[indexFind].Date))
                        {
                            oneStockKLineYear.OneDayDataList[indexFind].Date = oneStockKLineDataRec.Date;
                            oneStockKLineYear.OneDayDataList[indexFind].High = Math.Max(oneStockKLineYear.OneDayDataList[indexFind].High, oneStockKLineDataRec.High);
                            oneStockKLineYear.OneDayDataList[indexFind].Low = Math.Min(oneStockKLineYear.OneDayDataList[indexFind].Low, oneStockKLineDataRec.Low);
                            oneStockKLineYear.OneDayDataList[indexFind].Close = oneStockKLineDataRec.Close;
                            oneStockKLineYear.OneDayDataList[indexFind].Volume += oneStockKLineDataRec.Volume;
                            oneStockKLineYear.OneDayDataList[indexFind].Amount += oneStockKLineDataRec.Amount;
                        }
                        else if ((yearMem == yearPacketTime) && (oneStockKLineDataRec.Date < oneStockKLineYear.OneDayDataList[indexFind].Date))
                        {
                            oneStockKLineYear.OneDayDataList[indexFind].High = Math.Max(oneStockKLineYear.OneDayDataList[indexFind].High, oneStockKLineDataRec.High);
                            oneStockKLineYear.OneDayDataList[indexFind].Low = Math.Min(oneStockKLineYear.OneDayDataList[indexFind].Low, oneStockKLineDataRec.Low);
                            oneStockKLineYear.OneDayDataList[indexFind].Open = oneStockKLineDataRec.Open;
                            oneStockKLineYear.OneDayDataList[indexFind].Volume += oneStockKLineDataRec.Volume;
                            oneStockKLineYear.OneDayDataList[indexFind].Amount += oneStockKLineDataRec.Amount;
                        }
                    }
                    else if (indexFind < 0)
                    {
                        oneStockKLineYear.OneDayDataList.Add(oneStockKLineDataRec.DeepCopy());
                    }
                }
            }
            yearPacket.KLineDataRec = oneStockKLineYear;
            return yearPacket;
        }

        private void SetHisKLineData(ResHisKLineDataPacket dataPacket)
        {
            try
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
                if (dataPacket.KLineDataRec == null || dataPacket.KLineDataRec.Code == 0)
                    return;
                _allKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData);
                //如果周期是年线并且内存中没有内线数据,拼接年线
                if (dataPacket.KLineDataRec.Cycle == KLineCycle.CycleSeason
                    && (memCycleKLineData == null || !memCycleKLineData.ContainsKey(KLineCycle.CycleYear)))
                {
                    SetHisKLineData(GetYearKLineDataPacket(dataPacket.KLineDataRec));
                }

                if (_allKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData))
                {
                    OneStockKLineDataRec memKLineData;
                    if (memCycleKLineData.TryGetValue(dataPacket.KLineDataRec.Cycle, out memKLineData))
                    {
                        List<OneDayDataRec> packetDataList = dataPacket.KLineDataRec.OneDayDataList;
                        List<OneDayDataRec> memDataList = memKLineData.OneDayDataList;
                        if (packetDataList == null || packetDataList.Count <= 0)
                            return;

                        long packet0 = packetDataList[0].Date*(long)1000000 + packetDataList[0].Time;
                        long packetEnd = packetDataList[packetDataList.Count - 1].Date * (long)1000000 + packetDataList[packetDataList.Count - 1].Time;
                        long mem0 = memDataList[0].Date * (long)1000000 + memDataList[0].Time;
                        long memEnd = memDataList[memDataList.Count - 1].Date * (long)1000000 + memDataList[memDataList.Count - 1].Time;

                        if (packet0 < mem0 && packetEnd < memEnd)
                        {
                            for (int i = packetDataList.Count - 1; i >= 0; i--)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp < mem0)
                                    memDataList.Insert(0, packetDataList[i]);
                            }
                        }
                        else if (packet0 < mem0 && packetEnd >= memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 == mem0 && packetEnd > memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 > mem0 && packetEnd > memEnd)
                        {
                            for (int i = 0; i < packetDataList.Count; i++)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp > memEnd)
                                    memDataList.Add(packetDataList[i]);
                            }
                        }
                    }
                    else
                        memCycleKLineData.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                }
                else
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>(1);
                    tmp.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                    _allKLineData.Add(dataPacket.KLineDataRec.Code, tmp);
                }
                #region 拼接当日数据
                Dictionary<KLineCycle, OneStockKLineDataRec> hismemCycleKlineData;
                if (_allKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out hismemCycleKlineData))
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> allCycleData = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                    foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in hismemCycleKlineData)
                    {
                        switch (perCycle.Key)
                        {
                            case KLineCycle.CycleWeek:
                            case KLineCycle.CycleMonth:
                            case KLineCycle.CycleSeason:
                            case KLineCycle.CycleYear:
                                Dictionary<KLineCycle, OneStockKLineDataRec> allCycleTodayData = null;
                                OneStockKLineDataRec todayData = null;
                                if (_todayKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out allCycleTodayData))
                                {
                                    try
                                    {
                                        if (allCycleTodayData.TryGetValue(KLineCycle.CycleDay, out todayData)
                                        && (perCycle.Value.OneDayDataList.Count==0
                                        ||todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Date > perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1].Date ))
                                        {
                                            SetTodayKlineCycleWeek(todayData, perCycle.Value, 0, 0, 10000);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                    }

                                }
                                break;
                        }
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("KLineDataTable " + e.Message);
                throw;
            }
           
        }

        private void SetHisKLineData(ResHisKLineOrgDataPacket dataPacket)
        {
            try
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
                if (dataPacket.KLineDataRec == null || dataPacket.KLineDataRec.Code == 0)
                    return;
                _allKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData);
                //如果周期是年线并且内存中没有内线数据,拼接年线
                if (dataPacket.KLineDataRec.Cycle == KLineCycle.CycleSeason
                    && (memCycleKLineData == null || !memCycleKLineData.ContainsKey(KLineCycle.CycleYear)))
                {
                    SetHisKLineData(GetYearKLineDataPacket(dataPacket.KLineDataRec));
                }
                if (_allKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData))
                {
                    OneStockKLineDataRec memKLineData;
                    if (memCycleKLineData.TryGetValue(dataPacket.KLineDataRec.Cycle, out memKLineData))
                    {
                        List<OneDayDataRec> packetDataList = dataPacket.KLineDataRec.OneDayDataList;
                        List<OneDayDataRec> memDataList = memKLineData.OneDayDataList;
                        if (packetDataList.Count == 0 || memDataList.Count == 0)
                            return;
                        long packet0 = packetDataList[0].Date * (long)1000000 + packetDataList[0].Time;
                        long packetEnd = packetDataList[packetDataList.Count - 1].Date * (long)1000000 + packetDataList[packetDataList.Count - 1].Time;
                        long mem0 = memDataList[0].Date * (long)1000000 + memDataList[0].Time;
                        long memEnd = memDataList[memDataList.Count - 1].Date * (long)1000000 + memDataList[memDataList.Count - 1].Time;

                        if (packet0 < mem0 && packetEnd < memEnd)
                        {
                            for (int i = packetDataList.Count - 1; i >= 0; i--)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp < mem0)
                                    memDataList.Insert(0, packetDataList[i]);
                            }
                        }
                        else if (packet0 < mem0 && packetEnd >= memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 == mem0 && packetEnd > memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 > mem0 && packetEnd > memEnd)
                        {
                            for (int i = 0; i < packetDataList.Count; i++)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp > memEnd)
                                    memDataList.Add(packetDataList[i]);
                            }
                        }
                    }
                    else
                        memCycleKLineData.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                }
                else
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>(1);
                    tmp.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                    _allKLineData.Add(dataPacket.KLineDataRec.Code, tmp);
                }
                #region 拼接当日数据
                Dictionary<KLineCycle, OneStockKLineDataRec> hismemCycleKlineData;
                if (_allKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out hismemCycleKlineData))
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> allCycleData = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                    foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in hismemCycleKlineData)
                    {
                        switch (perCycle.Key)
                        {
                            case KLineCycle.CycleWeek:
                            case KLineCycle.CycleMonth:
                            case KLineCycle.CycleSeason:
                            case KLineCycle.CycleYear:
                                Dictionary<KLineCycle, OneStockKLineDataRec> allCycleTodayData = null;
                                OneStockKLineDataRec todayData = null;
                                if (_todayKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out allCycleTodayData))
                                {
                                    if (allCycleTodayData.TryGetValue(KLineCycle.CycleDay, out todayData)
                                        && ( perCycle.Value.OneDayDataList.Count==0||
                                        todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Date > perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1].Date))
                                        SetTodayKlineCycleWeek(todayData, perCycle.Value, 0, 0,1);
                                }
                                break;
                        }
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("KLineDataTable " + e.Message);
                throw;
            }

        }

        private void SetMinKLineData(ResMinKLineDataPacket dataPacket)
        {
            try
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
                if (dataPacket.KLineDataRec.Code == 0 || dataPacket.KLineDataRec.OneDayDataList.Count == 0)
                    return;
                if (_todayKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData))
                {
                    OneStockKLineDataRec memKLineData;
                    if (memCycleKLineData.TryGetValue(dataPacket.KLineDataRec.Cycle, out memKLineData))
                    {
                        List<OneDayDataRec> packetDataList = dataPacket.KLineDataRec.OneDayDataList;
                        List<OneDayDataRec> memDataList = memKLineData.OneDayDataList;

                        long packet0 = packetDataList[0].Date * (long)1000000 + packetDataList[0].Time;
                        long packetEnd = packetDataList[packetDataList.Count - 1].Date * (long)1000000 + packetDataList[packetDataList.Count - 1].Time;
                        long mem0 = memDataList[0].Date * (long)1000000 + memDataList[0].Time;
                        long memEnd = memDataList[memDataList.Count - 1].Date * (long)1000000 + memDataList[memDataList.Count - 1].Time;

                        if (packet0 < mem0 && packetEnd >= mem0 && packetEnd < memEnd)
                        {
                            for (int i = packetDataList.Count - 1; i >= 0; i--)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp < mem0)
                                    memDataList.Insert(0, packetDataList[i]);
                            }
                        }
                        else if (packet0 < mem0 && packetEnd >= memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 == mem0 && packetEnd > memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 > mem0 && packet0 <= memEnd && packetEnd > memEnd)
                        {
                            for (int i = 0; i < packetDataList.Count; i++)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp > memEnd)
                                    memDataList.Add(packetDataList[i]);
                            }
                        }
                    }
                    else
                        memCycleKLineData.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                }
                else
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>(1);
                    tmp.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                    _todayKLineData.Add(dataPacket.KLineDataRec.Code, tmp);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("KLineDataTable " + e.Message);
                throw;
            }
        }

        private void SetHisKLineData(ResMintKLineOrgDataPacket dataPacket)
        {
            try
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
                if (dataPacket.KLineDataRec.Code == 0 || dataPacket.KLineDataRec.OneDayDataList.Count==0)
                    return;
                if (_todayKLineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData))
                {
                    OneStockKLineDataRec memKLineData;
                    if (memCycleKLineData.TryGetValue(dataPacket.KLineDataRec.Cycle, out memKLineData))
                    {
                        List<OneDayDataRec> packetDataList = dataPacket.KLineDataRec.OneDayDataList;
                        List<OneDayDataRec> memDataList = memKLineData.OneDayDataList;

                        long packet0 = packetDataList[0].Date * (long)1000000 + packetDataList[0].Time;
                        long packetEnd = packetDataList[packetDataList.Count - 1].Date * (long)1000000 + packetDataList[packetDataList.Count - 1].Time;
                        long mem0 = memDataList[0].Date * (long)1000000 + memDataList[0].Time;
                        long memEnd = memDataList[memDataList.Count - 1].Date * (long)1000000 + memDataList[memDataList.Count - 1].Time;

                        if (packet0 < mem0 && packetEnd >= mem0 && packetEnd < memEnd)
                        {
                            for (int i = packetDataList.Count - 1; i >= 0; i--)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp < mem0)
                                    memDataList.Insert(0, packetDataList[i]);
                            }
                        }
                        else if (packet0 < mem0 && packetEnd >= memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 == mem0 && packetEnd > memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 > mem0 && packet0 <= memEnd && packetEnd > memEnd)
                        {
                            for (int i = 0; i < packetDataList.Count; i++)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp > memEnd)
                                    memDataList.Add(packetDataList[i]);
                            }
                        }
                    }
                    else
                        memCycleKLineData.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                }
                else
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>(1);
                    tmp.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                    _todayKLineData.Add(dataPacket.KLineDataRec.Code, tmp);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("KLineDataTable " + e.Message);
                throw;
            }
        }

        private void SetDivideRightData(ResDivideRightOrgDataPacket dataPacket)
        {
            _divideRightData.Clear();
            foreach (KeyValuePair<int, List<List<OneDivideRightBase>>> oneStock in dataPacket.DivideData)
            {
                _divideRightData.Add(oneStock.Key,oneStock.Value);
            }
            //foreach (KeyValuePair<int, List<List<OneDivideRightBase>>> oneStock in dataPacket.DivideData)
            //{
            //    if(_divideRightData.ContainsKey(oneStock.Key))
            //    {
            //        List<List<OneDivideRightBase>> memData = _divideRightData[oneStock.Key];
            //        foreach (List<OneDivideRightBase> oneDay in oneStock.Value)
            //        {
            //            if(memData)
            //        }
            //    }
            //    else
            //    {
            //        _divideRightData.Add(oneStock.Key,oneStock.Value);
            //    }
            //}
            
        }

        private void SetFundKLineData(ResFundKLineAfterDivideDataPacket dataPacket)
        {
            try
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
                if (dataPacket.KLineDataRec.Code == 0 || dataPacket.KLineDataRec.OneDayDataList.Count == 0)
                    return;
                _fundAfterDivideKlineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData);
                //如果周期是年线并且内存中没有内线数据,拼接年线
                if (dataPacket.KLineDataRec.Cycle == KLineCycle.CycleSeason
                    && (memCycleKLineData == null || !memCycleKLineData.ContainsKey(KLineCycle.CycleYear)))
                {
                    SetHisKLineData(GetYearKLineDataPacket(dataPacket.KLineDataRec));
                }
                if (_fundAfterDivideKlineData.TryGetValue(dataPacket.KLineDataRec.Code, out memCycleKLineData))
                {
                    OneStockKLineDataRec memKLineData;
                    if (memCycleKLineData.TryGetValue(dataPacket.KLineDataRec.Cycle, out memKLineData))
                    {
                        List<OneDayDataRec> packetDataList = dataPacket.KLineDataRec.OneDayDataList;
                        List<OneDayDataRec> memDataList = memKLineData.OneDayDataList;

                        long packet0 =  packetDataList[0].Date * (long)1000000 + packetDataList[0].Time;
                        long packetEnd = packetDataList[packetDataList.Count - 1].Date * (long)1000000 + packetDataList[packetDataList.Count - 1].Time;
                        long mem0 = memDataList[0].Date * (long)1000000 + memDataList[0].Time;
                        long memEnd = memDataList[memDataList.Count - 1].Date * (long)1000000 + memDataList[memDataList.Count - 1].Time;

                        if (packet0 < mem0 && packetEnd >= mem0 && packetEnd < memEnd)
                        {
                            for (int i = packetDataList.Count - 1; i >= 0; i--)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp < mem0)
                                    memDataList.Insert(0, packetDataList[i]);
                            }
                        }
                        else if (packet0 < mem0 && packetEnd >= memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 == mem0 && packetEnd > memEnd)
                        {
                            memDataList.Clear();
                            memDataList.AddRange(packetDataList);
                        }
                        else if (packet0 > mem0 && packet0 <= memEnd && packetEnd > memEnd)
                        {
                            for (int i = 0; i < packetDataList.Count; i++)
                            {
                                long tmp = packetDataList[i].Date * (long)1000000 + packetDataList[i].Time;
                                if (tmp > memEnd)
                                    memDataList.Add(packetDataList[i]);
                            }
                        }
                    }
                    else
                        memCycleKLineData.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                }
                else
                {
                    Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>(1);
                    tmp.Add(dataPacket.KLineDataRec.Cycle, dataPacket.KLineDataRec);
                    _fundAfterDivideKlineData.Add(dataPacket.KLineDataRec.Code, tmp);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("KLineDataTable " + e.Message);
                throw;
            }
        }

        #region 新改k线推送请求
        private void SetTodayKLineData(RealTimeDataPacket dataPacket)
        {
            #region 参数类型处理
            int tempCode = 0;
            if (dataPacket is ResStockDetailLev2DataPacket)
            {
                tempCode = (dataPacket as ResStockDetailLev2DataPacket).Code;
            }
            else if (dataPacket is ResNOrderStockDetailLev2DataPacket)
            {
                ResNOrderStockDetailLev2DataPacket tmpPacket = dataPacket as ResNOrderStockDetailLev2DataPacket;
                if (tmpPacket.OrderDetailData != null && tmpPacket.OrderDetailData.Count > 0)
                {
                    tempCode = (dataPacket as ResNOrderStockDetailLev2DataPacket).OrderDetailData[0].Code;
                }
            }
            else if (dataPacket is ResStockDetailDataPacket)
            {
                tempCode = (dataPacket as ResStockDetailDataPacket).Code;
            }
            else if (dataPacket is ResIndexFuturesDetailDataPacket)
            {
                tempCode = (dataPacket as ResIndexFuturesDetailDataPacket).Code;
            }
            else if (dataPacket is ResOceanRecordDataPacket)
            {
                tempCode = (dataPacket as ResOceanRecordDataPacket).Code;
            }
            else if (dataPacket is ResIndexDetailDataPacket)
            {
                //TODO:指数处理
            }

            #endregion
            float nowPrice = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
            if (nowPrice == 0 || tempCode == 0)
                return;
            //净值线当日数据不添加
            MarketType tempMarket = Dc.GetMarketType(tempCode);
            if (SecurityAttribute.FundFinancingTypeList.Contains(tempMarket))
                return;

            #region 当日数据修改
            Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
            int memData = 0;
            double tempAmount = 0;
            long tempVolume = 0;
            if (_todayKLineData.TryGetValue(tempCode, out memCycleKLineData))
            {
                foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> oneCycle in memCycleKLineData)
                {
                    if (oneCycle.Value != null && oneCycle.Value.OneDayDataList != null && oneCycle.Value.OneDayDataList.Count > 0)
                    {
                        memData = oneCycle.Value.OneDayDataList[0].Date;
                        if (oneCycle.Key == KLineCycle.CycleDay)
                        {
                            tempAmount = oneCycle.Value.OneDayDataList[oneCycle.Value.OneDayDataList.Count - 1].Amount;
                            tempVolume = oneCycle.Value.OneDayDataList[oneCycle.Value.OneDayDataList.Count - 1].Volume;
                            break;
                        }
                    }
                }
            }
            #region 集合竞价时间日线拼接
            MarketTime mt = TimeUtilities.GetMarketTime(tempCode);
            int packetTime = Dc.GetFieldDataInt32(tempCode,FieldIndex.Time);
            if (mt.PushTime != 0 && packetTime < mt.ClearTime && packetTime >= mt.PushTime)
            {
                return;
            }
            if (packetTime < mt.KLineTime && packetTime >= mt.ClearTime)//初始化到k线绘制时间
            {
                return;
            }
            else if (packetTime < mt.FirstOpenTime && packetTime >= mt.KLineTime)
            {
                if (memData == Dc.GetTradeDate(tempCode) && memCycleKLineData != null && memCycleKLineData.ContainsKey(KLineCycle.CycleDay))
                {
                    #region 修改内存中的当日数据
                    foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in memCycleKLineData)
                    {
                        switch (perCycle.Key)
                        {
                            case KLineCycle.CycleDay:
                                OneDayDataRec oneDayDataRec1 =
                                    perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1];
                                oneDayDataRec1.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;
                                oneDayDataRec1.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);

                                float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                                if (price.Equals(0.00F))
                                    price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                                if (!price.Equals(0.00F))
                                {
                                    oneDayDataRec1.Close = price;
                                    oneDayDataRec1.Low = price;
                                    oneDayDataRec1.High = price;
                                    oneDayDataRec1.Open = price;
                                }
                                switch (Dc.GetMarketType(tempCode))
                                {
                                    case MarketType.SHINDEX:
                                    case MarketType.SZINDEX:
                                        oneDayDataRec1.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume)/100;
                                        break;
                                    default:
                                        oneDayDataRec1.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume);
                                        break;
                                }
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 创建新的k线数据
                    if (TimeUtilities.GetKLineLastDate(Dc.GetTradeDate(tempCode), KLineCycle.CycleDay) ==
                        Dc.GetTradeDate(tempCode))
                    {
                        OneStockKLineDataRec oneStockDay = new OneStockKLineDataRec();
                        OneDayDataRec oneDayDataRec = new OneDayDataRec();
                        oneStockDay.Code = tempCode;
                        oneStockDay.Cycle = KLineCycle.CycleDay;
                        oneDayDataRec.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;
                        oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);
                        float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                        if (price.Equals(0.00F))
                            price = Dc.GetFieldDataInt32(tempCode, FieldIndex.PreClose);
                        if (!price.Equals(0.00F))
                        {
                            oneDayDataRec.Close = price;
                            oneDayDataRec.High = price;
                            oneDayDataRec.Low = price;
                            oneDayDataRec.Open = price;
                        }
                        switch (Dc.GetMarketType(tempCode))
                        {
                            case MarketType.SHINDEX:
                            case MarketType.SZINDEX:
                                oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume)/100;
                                break;
                            default:
                                oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume);
                                break;
                        }
                        oneDayDataRec.Date = Dc.GetTradeDate(tempCode);
                        oneStockDay.OneDayDataList.Add(oneDayDataRec);
                        Dictionary<KLineCycle, OneStockKLineDataRec> tmp =
                            new Dictionary<KLineCycle, OneStockKLineDataRec>();
                        tmp.Add(KLineCycle.CycleDay, oneStockDay);
                        _todayKLineData[tempCode] = tmp;
                    }
                    #endregion
                }
                return;
            }
            #endregion
            #region 盘中交易k线数据拼接
            if (memData == Dc.GetTradeDate(tempCode) && memCycleKLineData != null)
            {
                #region 修改内存中的当日数据（分钟线、日线）
                foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in memCycleKLineData)
                {
                    switch (perCycle.Key)
                    {
                        case KLineCycle.CycleMint1:
                        case KLineCycle.CycleMint5:
                        case KLineCycle.CycleMint15:
                        case KLineCycle.CycleMint30:
                        case KLineCycle.CycleMint60:
                        case KLineCycle.CycleMint120:
                            //计算当前分钟线所有金额之和
                            double SumAmount = 0;
                            long SumVolume = 0;
                            foreach (OneDayDataRec tempMinData in perCycle.Value.OneDayDataList)
                            {
                                SumAmount += tempMinData.Amount;
                                SumVolume += tempMinData.Volume;
                            }
                            int endTime = TimeUtilities.GetMintKLineTimeFromPoint(tempCode,
                                perCycle.Value.OneDayDataList.Count -
                                1,
                                perCycle.Key);
                            int nextEndTime = TimeUtilities.GetMintKLineTimeFromPoint(tempCode,
                                perCycle.Value.OneDayDataList.
                                    Count,
                                perCycle.Key);
                            if (endTime > Dc.GetFieldDataInt32(tempCode, FieldIndex.Time)) //修改最后一根的值
                            {
                                OneDayDataRec oneDayDataRec =
                                    perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1];
                                oneDayDataRec.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;

                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Now).Equals(0.00F))
                                {
                                    oneDayDataRec.Close = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                                    oneDayDataRec.High = Math.Max(Dc.GetFieldDataSingle(tempCode, FieldIndex.Now), oneDayDataRec.High);
                                    oneDayDataRec.Low = Math.Min(Dc.GetFieldDataSingle(tempCode, FieldIndex.Now), oneDayDataRec.Low);
                                }

                                oneDayDataRec.Amount += Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount) - SumAmount;
                                switch (Dc.GetMarketType(tempCode))
                                {
                                    case MarketType.SHINDEX:
                                    case MarketType.SZINDEX:
                                        oneDayDataRec.Volume += Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume)/100 - SumVolume;
                                        break;
                                    default:
                                        oneDayDataRec.Volume += Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume) - SumVolume;
                                        break;
                                }
                            }
                            else if (endTime <= Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) &&
                                     nextEndTime > Dc.GetFieldDataInt32(tempCode, FieldIndex.Time)) //新建一根
                            {
                                if (MinutesDataRequestEvent != null)
                                    MinutesDataRequestEvent(); //重新发送请求(分钟线)
                            }
                            break;
                        case KLineCycle.CycleDay:
                            OneDayDataRec oneDayDataRec1 =
                                perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1];
                            oneDayDataRec1.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;
                            oneDayDataRec1.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);
                            float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                            if (price.Equals(0.00F))
                                price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                            if (!price.Equals(0.00f))
                            {
                                oneDayDataRec1.Close = price;

                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.High).Equals(0.0F))
                                    oneDayDataRec1.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                                else
                                    oneDayDataRec1.High = price;
                             
                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Low).Equals(0.0F))
                                    oneDayDataRec1.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                                else
                                    oneDayDataRec1.Low = price;

                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Open).Equals(0.0F))
                                    oneDayDataRec1.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                                else
                                    oneDayDataRec1.Open = price;
                            }
                            
                            switch (Dc.GetMarketType(tempCode))
                            {
                                case MarketType.SHINDEX:
                                case MarketType.SZINDEX:
                                    oneDayDataRec1.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume)/100;
                                    break;
                                default:
                                    oneDayDataRec1.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume);
                                    break;
                            }
                            break;
                    }
                }

                #endregion

                #region 如果当日数据中没有当日日线数据 添加新的日线数据
                if (!memCycleKLineData.ContainsKey(KLineCycle.CycleDay))
                {
                    OneStockKLineDataRec oneStockDay = new OneStockKLineDataRec();
                    OneDayDataRec oneDayDataRec = new OneDayDataRec();
                    oneStockDay.Code = tempCode;
                    oneStockDay.Cycle = KLineCycle.CycleDay;
                    oneDayDataRec.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;
                    oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);

                    float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                    if (price.Equals(0.00F))
                        price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                    if (!price.Equals(0.00F))
                    {
                        oneDayDataRec.Close = price;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.High).Equals(0.0F))
                            oneDayDataRec.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                        else
                            oneDayDataRec.High = price;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Low).Equals(0.0F))
                            oneDayDataRec.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        else
                            oneDayDataRec.Low = price;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Open).Equals(0.0F))
                            oneDayDataRec.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                        else
                            oneDayDataRec.Open = price;
                    }
                    switch (Dc.GetMarketType(tempCode))
                    {
                        case MarketType.SHINDEX:
                        case MarketType.SZINDEX:
                            oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume)/100;
                            break;
                        default:
                            oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume);
                            break;
                    }
                    oneDayDataRec.Date = Dc.GetTradeDate(tempCode);
                    oneStockDay.OneDayDataList.Add(oneDayDataRec);
                    memCycleKLineData.Add(KLineCycle.CycleDay, oneStockDay);
                }
                #endregion
            }
            else
            {
                int indexTrend =
                   TimeUtilities.GetPointFromTime(Dc.GetFieldDataInt32(tempCode, FieldIndex.Time), tempCode);
                if (indexTrend == 0)
                {
                    if (MinutesDataRequestEvent != null)
                        MinutesDataRequestEvent(); //发送k线请求(分钟线)
                }

                #region 日线
                if (TimeUtilities.GetKLineLastDate(Dc.GetTradeDate(tempCode), KLineCycle.CycleDay) == Dc.GetTradeDate(tempCode))
                {
                    OneStockKLineDataRec oneStockDay = new OneStockKLineDataRec();
                    OneDayDataRec oneDayDataRec = new OneDayDataRec();
                    oneStockDay.Code = tempCode;
                    oneStockDay.Cycle = KLineCycle.CycleDay;
                    oneDayDataRec.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;
                    oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);
                    float price8 = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                    if (price8.Equals(0.00F))
                        price8 = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                    if (!price8.Equals(0.00F))
                    {
                        oneDayDataRec.Close = price8;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.High).Equals(0.0F))
                            oneDayDataRec.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                        else
                            oneDayDataRec.High = price8;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Low).Equals(0.0F))
                            oneDayDataRec.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        else
                            oneDayDataRec.Low = price8;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Open).Equals(0.0F))
                            oneDayDataRec.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                        else
                            oneDayDataRec.Open = price8;
                    }  
                    switch (Dc.GetMarketType(tempCode))
                    {
                        case MarketType.SHINDEX:
                        case MarketType.SZINDEX:
                            oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume) / 100;
                            break;
                        default:
                            oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume);
                            break;
                    }
                        
                    oneDayDataRec.Date = Dc.GetTradeDate(tempCode);
                    oneStockDay.OneDayDataList.Add(oneDayDataRec);
                    Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                    tmp.Add(KLineCycle.CycleDay, oneStockDay);
                    _todayKLineData[tempCode] = tmp;
                }
                #endregion
            }
            #endregion
            #endregion

            #region 历史数据修改
            Dictionary<KLineCycle, OneStockKLineDataRec> hismemCycleKlineData;
            if (_allKLineData.TryGetValue(tempCode, out hismemCycleKlineData))
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> allCycleData = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in hismemCycleKlineData)
                {
                    switch (perCycle.Key)
                    {
                        case KLineCycle.CycleWeek:
                        case KLineCycle.CycleMonth:
                        case KLineCycle.CycleSeason:
                        case KLineCycle.CycleYear:
                            OneStockKLineDataRec todayData = null;
                            if (_todayKLineData[tempCode].TryGetValue(KLineCycle.CycleDay, out todayData))
                                SetTodayKlineCycleWeek(todayData, perCycle.Value, tempAmount, tempVolume, GetPushVolumeDivideValue((Dc.GetMarketType(tempCode)), perCycle.Key));
                            break;
                    }
                }
            }
            #endregion

            //GC.Collect();
        }

        private void SetTodayKLineOrgData(OrgDataPacket dataPacket)
        {
            #region 参数类型处理
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
            #endregion
            float nowPrice = 0;
            if (Dc.GetFieldDataSingle(tempCode, FieldIndex.Now)!=0)
                nowPrice = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
            float preClose = 0;
            if (Dc.GetFieldDataSingle(tempCode,FieldIndex.PreClose) != null)
                preClose = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
            if (nowPrice.Equals(0) || tempCode == 0)
                return;
            //净值线当日数据不添加
            
            if (SecurityAttribute.FundFinancingTypeList.Contains(Dc.GetMarketType(tempCode)))
                return;

            #region 当日数据修改
            Dictionary<KLineCycle, OneStockKLineDataRec> memCycleKLineData;
            int memData = 0;
            double tempAmount = 0;
            long tempVolume = 0;
            if (_todayKLineData.TryGetValue(tempCode, out memCycleKLineData))
            {
                foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> oneCycle in memCycleKLineData)
                {
                    if (oneCycle.Value != null && oneCycle.Value.OneDayDataList != null && oneCycle.Value.OneDayDataList.Count > 0)
                    {
                        memData = oneCycle.Value.OneDayDataList[0].Date;
                        if (oneCycle.Key == KLineCycle.CycleDay)
                        {
                            tempAmount = oneCycle.Value.OneDayDataList[oneCycle.Value.OneDayDataList.Count - 1].Amount;
                            tempVolume = oneCycle.Value.OneDayDataList[oneCycle.Value.OneDayDataList.Count - 1].Volume;
                            break;
                        }
                    }
                }
            }
            #region 集合竞价时间日线拼接
            MarketTime mt = TimeUtilities.GetMarketTime(tempCode);
            int packetTime = -1;
            packetTime = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time);

            if (mt.PushTime != 0 && packetTime < mt.ClearTime && packetTime >= mt.PushTime)
            {
                return;
            }
            if (packetTime == -1 || (packetTime < mt.KLineTime && packetTime >= mt.ClearTime))
            {
                return;
            }
            else if (packetTime < mt.FirstOpenTime && packetTime >= mt.KLineTime)
            {
                if (memData == Dc.GetTradeDate(tempCode) && memCycleKLineData != null &&
                    memCycleKLineData.ContainsKey(KLineCycle.CycleDay))
                {
                    #region 修改内存中的当日数据
                    foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in memCycleKLineData)
                    {
                        switch (perCycle.Key)
                        {
                            case KLineCycle.CycleDay:
                                OneDayDataRec oneDayDataRec1 =
                                    perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1];
                                oneDayDataRec1.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;
                                oneDayDataRec1.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);
                                float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                                if (price.Equals(0.00F))
                                    price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                                if (!price.Equals(0.00F))
                                {
                                    oneDayDataRec1.Close = price;
                                    oneDayDataRec1.High = price;
                                    oneDayDataRec1.Low = price;
                                    oneDayDataRec1.Open = price;
                                }
                                oneDayDataRec1.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.Volume);
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 创建新的当日数据
                    if (TimeUtilities.GetKLineLastDate(Dc.GetTradeDate(tempCode), KLineCycle.CycleDay) ==
                        Dc.GetTradeDate(tempCode))
                    {
                        OneStockKLineDataRec oneStockDay = new OneStockKLineDataRec();
                        OneDayDataRec oneDayDataRec = new OneDayDataRec();
                        oneStockDay.Code = tempCode;
                        oneStockDay.Cycle = KLineCycle.CycleDay;
                        oneDayDataRec.Time =Dc.GetFieldDataInt32(tempCode,FieldIndex.Time) + 100;
                        oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount);
                        float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                        if (price.Equals(0.00F))
                            price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                        if (!price.Equals(0.00F))
                        {
                            oneDayDataRec.Close = price;
                            oneDayDataRec.High = price;
                            oneDayDataRec.Low = price;
                            oneDayDataRec.Open = price;
                        }
                        oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume);
                        oneDayDataRec.Date = Dc.GetTradeDate(tempCode);
                        oneStockDay.OneDayDataList.Add(oneDayDataRec);
                        Dictionary<KLineCycle, OneStockKLineDataRec> tmp =
                            new Dictionary<KLineCycle, OneStockKLineDataRec>();
                        tmp.Add(KLineCycle.CycleDay, oneStockDay);
                        if (!_todayKLineData.ContainsKey(tempCode))
                            _todayKLineData.Add(tempCode, tmp);
                    }
                    #endregion
                }
                return;
            }
            #endregion

            #region 盘中交易时间数据拼接
            if (memData == Dc.GetTradeDate(tempCode) && memCycleKLineData != null)
            {
                #region 修改内存中的当日数据（分钟线、日线）
                foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in memCycleKLineData)
                {
                    switch (perCycle.Key)
                    {
                        case KLineCycle.CycleMint1:
                        case KLineCycle.CycleMint5:
                        case KLineCycle.CycleMint15:
                        case KLineCycle.CycleMint30:
                        case KLineCycle.CycleMint60:
                        case KLineCycle.CycleMint120:
                            int endTime = TimeUtilities.GetMintKLineTimeFromPoint(tempCode,
                                                                                    perCycle.Value.OneDayDataList.Count -
                                                                                    1,
                                                                                    perCycle.Key);
                            int nextEndTime = TimeUtilities.GetMintKLineTimeFromPoint(tempCode,
                                                                                        perCycle.Value.OneDayDataList.
                                                                                            Count,
                                                                                        perCycle.Key);
                            if (endTime > Dc.GetFieldDataInt32(tempCode, FieldIndex.Time)) //修改最后一根的值
                            {
                                OneDayDataRec oneDayDataRec =
                                    perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1];
                                oneDayDataRec.Time = Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) + 100;

                                oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);
                                if (!Dc.GetFieldDataSingle(tempCode,FieldIndex.Now).Equals(0.00F))
                                {
                                    oneDayDataRec.Close = Dc.GetFieldDataSingle(tempCode,FieldIndex.Now);
                                    oneDayDataRec.High = Math.Max(Dc.GetFieldDataSingle(tempCode,FieldIndex.Now), oneDayDataRec.High);
                                    oneDayDataRec.Low = Math.Min(Dc.GetFieldDataSingle(tempCode,FieldIndex.Now), oneDayDataRec.Low);
                                }

                                oneDayDataRec.Volume += Dc.GetFieldDataInt64(tempCode, FieldIndex.LastVolume);
                            }
                            else if (endTime <= Dc.GetFieldDataInt32(tempCode, FieldIndex.Time) &&
                                     nextEndTime > Dc.GetFieldDataInt32(tempCode, FieldIndex.Time)) //新建一根
                            {
                                if (MinutesDataRequestEvent != null)
                                    MinutesDataRequestEvent(); //重新发送k线请求(分钟线)
                            }
                            break;
                        case KLineCycle.CycleDay:
                            OneDayDataRec oneDayDataRec1 =
                                perCycle.Value.OneDayDataList[perCycle.Value.OneDayDataList.Count - 1];
                            oneDayDataRec1.Time =Dc.GetFieldDataInt32(tempCode,FieldIndex.Time) + 100;
                            oneDayDataRec1.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount);
                           float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                            if (price.Equals(0.00F))
                                price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                            if (!price.Equals(0.00F))
                            {
                                oneDayDataRec1.Close = price;

                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.High).Equals(0.0F))
                                    oneDayDataRec1.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                                else
                                    oneDayDataRec1.High = price;

                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Low).Equals(0.0F))
                                    oneDayDataRec1.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                                else
                                    oneDayDataRec1.Low = price;

                                if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Open).Equals(0.0F))
                                    oneDayDataRec1.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                                else
                                    oneDayDataRec1.Open = price;
                            }
                            oneDayDataRec1.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume);
                            break;
                    }
                }
                #endregion
                #region 如果内存中没有日线当日数据 添加新的日线数据
                if (!memCycleKLineData.ContainsKey(KLineCycle.CycleDay))
                {
                    OneStockKLineDataRec oneStockDay = new OneStockKLineDataRec();
                    OneDayDataRec oneDayDataRec = new OneDayDataRec();
                    oneStockDay.Code = tempCode;
                    oneStockDay.Cycle = KLineCycle.CycleDay;
                    oneDayDataRec.Time =Dc.GetFieldDataInt32(tempCode,FieldIndex.Time) + 100;

                    oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.Amount);
                    float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                    if (price.Equals(0.00F))
                        price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                    if (!price.Equals(0.00F))
                    {
                        oneDayDataRec.Close = price;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.High).Equals(0.0F))
                            oneDayDataRec.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                        else
                            oneDayDataRec.High = price;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Low).Equals(0.0F))
                            oneDayDataRec.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                        else
                            oneDayDataRec.Low = price;

                        if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Open).Equals(0.0F))
                            oneDayDataRec.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                        else
                            oneDayDataRec.Open = price;
                    }
                    oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume);
                    oneDayDataRec.Date = Dc.GetTradeDate(tempCode);
                    oneStockDay.OneDayDataList.Add(oneDayDataRec);
                    memCycleKLineData.Add(KLineCycle.CycleDay, oneStockDay);
                }
                #endregion
            }
            else
            {
                int indexTrend =
                    TimeUtilities.GetPointFromTime(Dc.GetFieldDataInt32(tempCode, FieldIndex.Time), tempCode);
                if (indexTrend == 0)
                {
                    if (MinutesDataRequestEvent != null)
                        MinutesDataRequestEvent(); //重新发送k线请求(分钟线)
                }

                #region 日线
                    if (TimeUtilities.GetKLineLastDate(Dc.GetTradeDate(tempCode), KLineCycle.CycleDay) == Dc.GetTradeDate(tempCode))
                    {
                        OneStockKLineDataRec oneStockDay = new OneStockKLineDataRec();
                        OneDayDataRec oneDayDataRec = new OneDayDataRec();
                        oneStockDay.Code = tempCode;
                        oneStockDay.Cycle = KLineCycle.CycleDay;
                        oneDayDataRec.Time =Dc.GetFieldDataInt32(tempCode,FieldIndex.Time) + 100;
                        oneDayDataRec.Amount = Dc.GetFieldDataDouble(tempCode, FieldIndex.IndexAmount);
                        float price = Dc.GetFieldDataSingle(tempCode, FieldIndex.Now);
                        if (price.Equals(0.00F))
                            price = Dc.GetFieldDataSingle(tempCode, FieldIndex.PreClose);
                        if (!price.Equals(0.00F))
                        {
                            oneDayDataRec.Close = price;

                            if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.High).Equals(0.0F))
                                oneDayDataRec.High = Dc.GetFieldDataSingle(tempCode, FieldIndex.High);
                            else
                                oneDayDataRec.High = price;

                            if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Low).Equals(0.0F))
                                oneDayDataRec.Low = Dc.GetFieldDataSingle(tempCode, FieldIndex.Low);
                            else
                                oneDayDataRec.Low = price;

                            if (!Dc.GetFieldDataSingle(tempCode, FieldIndex.Open).Equals(0.0F))
                                oneDayDataRec.Open = Dc.GetFieldDataSingle(tempCode, FieldIndex.Open);
                            else
                                oneDayDataRec.Open = price;
                        }
                        oneDayDataRec.Volume = Dc.GetFieldDataInt64(tempCode, FieldIndex.IndexVolume);
                        oneDayDataRec.Date = Dc.GetTradeDate(tempCode);
                        oneStockDay.OneDayDataList.Add(oneDayDataRec);
                        Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                        tmp.Add(KLineCycle.CycleDay, oneStockDay);
                        if (!_todayKLineData.ContainsKey(tempCode))
                            _todayKLineData.Add(tempCode, tmp);
                    }
                    #endregion
            }
            #endregion
            #endregion

            #region 历史数据修改
            Dictionary<KLineCycle, OneStockKLineDataRec> hismemCycleKlineData;
            if (_allKLineData.TryGetValue(tempCode, out hismemCycleKlineData))
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> allCycleData = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                foreach (KeyValuePair<KLineCycle, OneStockKLineDataRec> perCycle in hismemCycleKlineData)
                {
                    switch (perCycle.Key)
                    {
                        case KLineCycle.CycleWeek:
                        case KLineCycle.CycleMonth:
                        case KLineCycle.CycleSeason:
                        case KLineCycle.CycleYear:
                            OneStockKLineDataRec todayData = null;
                            if (_todayKLineData[tempCode].TryGetValue(KLineCycle.CycleDay, out todayData))
                                SetTodayKlineCycleWeek(todayData, perCycle.Value, tempAmount, tempVolume, 1);
                            break;
                    }
                }
            }
            #endregion

            //GC.Collect();
        }
        #endregion

        /// <summary>
        /// 拼接周线及以上周期的当日k线
        /// </summary>
        private void SetTodayKlineCycleWeek(OneStockKLineDataRec todayData, OneStockKLineDataRec hisData,double tempAmount,long tempVolume,int divideValue) 
        {
            if (todayData == null || todayData.OneDayDataList.Count == 0)
                return;
            int lastDate = TimeUtilities.GetKLineLastDate(Dc.GetTradeDate(todayData.Code), hisData.Cycle);
            //创建新的k线
            if (lastDate <= Dc.GetTradeDate(todayData.Code) && (hisData.OneDayDataList.Count==0|| hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Date < lastDate))
            {
                OneDayDataRec TodayKlineDataRec = new OneDayDataRec();
                TodayKlineDataRec.Time = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Time;
                TodayKlineDataRec.Amount = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Amount;
                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Close.Equals(0.00F))
                    TodayKlineDataRec.Close = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Close;

                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].High.Equals(0.00F))
                    TodayKlineDataRec.High = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].High;

                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Low.Equals(0.00F))
                    TodayKlineDataRec.Low = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Low;

                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Open.Equals(0.00F))
                    TodayKlineDataRec.Open = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Open;

                TodayKlineDataRec.Volume = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Volume / divideValue;
                TodayKlineDataRec.Date = Dc.GetTradeDate(todayData.Code);
                hisData.OneDayDataList.Add(TodayKlineDataRec);
            }
            else
            {
                hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Date = Dc.GetTradeDate(todayData.Code);
                hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Time = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Time;
                hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Amount += (todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Amount - tempAmount);

                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Close.Equals(0.00F))
                    hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Close = todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Close;

                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].High.Equals(0.00F))
                    hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].High = Math.Max(todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].High, hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].High);

                if (!todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Low.Equals(0.00F))
                    hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Low = Math.Min(todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Low, hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Low);

                hisData.OneDayDataList[hisData.OneDayDataList.Count - 1].Volume += (todayData.OneDayDataList[todayData.OneDayDataList.Count - 1].Volume - tempVolume) / divideValue;
            }
        }

        /// <summary>
        /// 自定义指数，添加数据，不走行情服务器
        /// </summary>
        /// <param name="data"></param>
        public void AddKLineData(OneStockKLineDataRec data)
        {
            if (data.Code == 0)
                return;

            if(AllKLineData.ContainsKey(data.Code))
            {
                if (AllKLineData[data.Code].ContainsKey(data.Cycle))
                    AllKLineData[data.Code][data.Cycle] = data;
                else
                    AllKLineData[data.Code].Add(data.Cycle,data);
            }
            else
            {
                Dictionary<KLineCycle, OneStockKLineDataRec> tmp = new Dictionary<KLineCycle, OneStockKLineDataRec>();
                tmp.Add(data.Cycle,data);
                AllKLineData.Add(data.Code, tmp);
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            switch (status)
            {
                case InitOrgStatus.SHSZ:
                case InitOrgStatus.All:
                    if(_todayKLineData!=null)
                        _todayKLineData.Clear();
                    if (_capitalFlowTodayKLineData!=null)
                        _capitalFlowTodayKLineData.Clear();
                    if(KLineReqBack != null)
                        KLineReqBack.Clear();
                    break;
            }
        }

        private int GetPushVolumeDivideValue(MarketType tempMarket,KLineCycle tempCycle)
        {
            int result = 1;
            //股票：日线正常，周月季年K线小1万倍
            if ((SecurityAttribute.SHL1MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SHRepurchaseLevel1)
            || (SecurityAttribute.SHL2MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SHRepurchaseLevel2)
            || (SecurityAttribute.SZL1MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SZRepurchaseLevel1)
            || (SecurityAttribute.SZL2MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SZRepurchaseLevel2))
            {
                if ((int)tempCycle > (int)KLineCycle.CycleDay)
                    result =10000;
            }
            //沪深指数：日K线小100倍，周月季年k线小100万倍
            if (tempMarket == MarketType.SHINDEX || tempMarket == MarketType.SZINDEX)
            {
                if ((int)tempCycle > (int)KLineCycle.CycleDay)
                    result =1000000;
                else if ((int)tempCycle == (int)KLineCycle.CycleDay)
                    result =100;
            }
            return result;
        }

        
    }
}
