using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EmCore;
using EmSerDSComm;

namespace OwLib
{
    /// <summary>
    /// DetailDataTable
    /// </summary>
    public class DetailDataTable : DataTableBase
    {

        #region 静态数据格式

        /// <summary>
        /// 静态财务数据
        /// </summary>
        private DataTable _dtMainFinace;

        /// <summary>
        /// 具有和静态财务数据相同的表结构的临时数据表
        /// </summary>
        private DataTable _tbTmp;
        private Dictionary<string, List<OneInfoMineOrgDataRec>> _dicNewsInfoByBlock;//按照小类存储新闻结构
        private Dictionary<string, List<OneInfoMineOrgDataRec>> _dicNewsReportInfoByBlock;// 按照小类存储新闻类研报结构（returnType=1）
        private Dictionary<string, List<ResearchReportItem>> _dicEmratingReportInfoByBlock;// 按照小类存储评估类研报结构（returnType=2）

        #endregion

        /// <summary>
        /// 信息地雷
        /// </summary>
        public Dictionary<int, Dictionary<InfoMine, List<OneInfoMineDataRec>>> InfoMineData
        {
            get { return _infoMineData; }
            private set { _infoMineData = value; }
        }

        /// <summary>
        /// 置顶
        /// </summary>
        public Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>> InfoMineOrgTopData
        {
            get { return _infoMineOrgTopData; }
            private set { _infoMineOrgTopData = value; }
        }


        public Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>> InfoMineOrgData
        {
            get { return _infoMineOrgData; }
            private set { _infoMineOrgData = value; }
        }
        /// <summary>
        /// 按照小类存储新闻结构(key: 小类代码(板块代码)； value：对应的新闻结构列表)
        /// </summary>
        public Dictionary<string, List<OneInfoMineOrgDataRec>> DicNewsInfoByBlock
        {
            get { return _dicNewsInfoByBlock; }
            set { _dicNewsInfoByBlock = value; }
        }
        /// <summary>
        /// 按照小类存储新闻结构(key: 小类代码(板块代码)； value：对应的新闻类研报列表)
        /// </summary>
        public Dictionary<string, List<OneInfoMineOrgDataRec>> DicNewsReportInfoByBlock
        {
            get { return _dicNewsReportInfoByBlock; }
            set { _dicNewsReportInfoByBlock = value; }
        }
        /// <summary>
        /// 按照小类存储新闻结构(key: 小类代码(板块代码)； value：对应的评估类研报结构列表)
        /// </summary>
        public Dictionary<string, List<ResearchReportItem>> DicEmratingReportInfoByBlock
        {
            get { return _dicEmratingReportInfoByBlock; }
            set { _dicEmratingReportInfoByBlock = value; }
        }




        private bool _isSort = false;
        private Dictionary<int, Dictionary<InfoMine, List<OneInfoMineDataRec>>> _infoMineData;
        private Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>> _infoMineOrgData;
        private Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>> _infoMineOrgTopData;

        /// <summary>
        /// 不读键盘精灵，直接码表
        /// </summary>
        /// <param name="isDependence"></param>
        public DetailDataTable()
            : this(false)
        {
        }

        /// <summary>
        /// 不读键盘精灵，直接码表
        /// </summary>
        /// <param name="isDependence"></param>
        public DetailDataTable(bool isDependence)
        {


            DicNewsInfoByBlock = new Dictionary<string, List<OneInfoMineOrgDataRec>>();
            DicNewsReportInfoByBlock = new Dictionary<string, List<OneInfoMineOrgDataRec>>();
            DicEmratingReportInfoByBlock = new Dictionary<string, List<ResearchReportItem>>();


            if (isDependence)
            {
                InfoMineData = new Dictionary<int, Dictionary<InfoMine, List<OneInfoMineDataRec>>>();
                InfoMineOrgData = new Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>>();
            }
            else
            {
                try
                {
                    InfoMineData = new Dictionary<int, Dictionary<InfoMine, List<OneInfoMineDataRec>>>();
                    InfoMineOrgData = new Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>>(1);
                    InfoMineOrgTopData = new Dictionary<int, Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>>(1);
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage(e.Message);
                }
            }

        }


        #region 数据包的处理

        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResStockDetailLev2DataPacket)
                SetStockDetailPacket((ResStockDetailLev2DataPacket)dataPacket);
            else if (dataPacket is ResIndexFuturesDetailDataPacket)
                SetStockDetailPacket((ResIndexFuturesDetailDataPacket)dataPacket);
            else if (dataPacket is ResIndexDetailDataPacket)
                SetIndexDetailPacket((ResIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResNOrderStockDetailLev2DataPacket)
                SetStockDetailPacket((ResNOrderStockDetailLev2DataPacket)dataPacket);

            else if (dataPacket is ResNewsReportDataPacket)
                SetNewsDataPacket((ResNewsReportDataPacket)dataPacket);
            else if (dataPacket is ResCustomStockNewsDataPacket)
                SetNewsDataPacket((ResCustomStockNewsDataPacket)dataPacket);
            else if (dataPacket is ResProfitForecastReportOrgDataPacket)
                SetOrgReportDataPacket((ResProfitForecastReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCapitalFlowReportOrgDataPacket)
                SetOrgReportDataPacket((ResCapitalFlowReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResDDEReportOrgDataPacket)
                SetOrgReportDataPacket((ResDDEReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResNetInFlowReportOrgDataPacket)
                SetOrgReportDataPacket((ResNetInFlowReportOrgDataPacket)dataPacket);


            else if (dataPacket is ResForexReportDataPacket)
                SetOrgReportDataPacket((ResForexReportDataPacket)dataPacket);
            else if (dataPacket is ResUSStockReportDataPacket)
                SetOrgReportDataPacket((ResUSStockReportDataPacket)dataPacket);
            else if (dataPacket is ResOSFuturesReportDataPacket)
                SetOrgReportDataPacket((ResOSFuturesReportDataPacket)dataPacket);
            else if (dataPacket is ResFinanceStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResFinanceStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCustomProfitForecastReportOrgDataPacket)
                SetOrgReportDataPacket((ResCustomProfitForecastReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCustomCapitalFlowReportOrgDataPacket)
                SetOrgReportDataPacket((ResCustomCapitalFlowReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCustomDDEReportOrgDataPacket)
                SetOrgReportDataPacket((ResCustomDDEReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCustomNetInFlowReportOrgDataPacket)
                SetOrgReportDataPacket((ResCustomNetInFlowReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCustomReportOrgDataPacket)
                SetOrgReportDataPacket((ResCustomReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResCustomFinanceStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResCustomFinanceStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResBlockStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResBlockStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResFundStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResFundStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResHKStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResHKStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResBondStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResBondStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResFuturesStockReportOrgDataPacket)
                SetOrgReportDataPacket((ResFuturesStockReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResIndexFutruesReportOrgDataPacket)
                SetOrgReportDataPacket((ResIndexFutruesReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResOSFuturesLMEReportDataPacket)
                SetOrgReportDataPacket((ResOSFuturesLMEReportDataPacket)dataPacket);
            else if (dataPacket is ResRateReportOrgDataPacket)
                SetOrgReportDataPacket((ResRateReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResFinanceReportOrgDataPacket)
                SetOrgReportDataPacket((ResFinanceReportOrgDataPacket)dataPacket);
            else if (dataPacket is ResBlockReportDataPacket)
                SetOrgReportDataPacket((ResBlockReportDataPacket)dataPacket);
            else if (dataPacket is ResInfoOrgDataPacket)
                SetOrgInfoDataPacket((ResInfoOrgDataPacket)dataPacket);
            else if (dataPacket is ResEMIndexDetailDataPacket)
                SetEmIndexDetailDataPacket((ResEMIndexDetailDataPacket)dataPacket);
            else if (dataPacket is ResIndexStaticOrgDataPacket)
                SetIndexStaticDataPacket((ResIndexStaticOrgDataPacket)dataPacket);
            else if (dataPacket is ResNewInfoOrgDataPacket)
                SetOrgInfoDataPacket((ResNewInfoOrgDataPacket)dataPacket);
            else if (dataPacket is ResInfoOrgByIdsDataPacket)
                SetOrgInfoDataPacket((ResInfoOrgByIdsDataPacket)dataPacket);
        }

        #region 设置各种包的数据

        private void SetStockDetailPacket(ResStockDetailLev2DataPacket dataPacket)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(dataPacket.Code, out fieldSingle))
                return;
            if (!DetailData.FieldIndexDataDouble.TryGetValue(dataPacket.Code, out fieldDouble))
                return;

            double zsz = 0;
            double ltsz = 0;
            double mgsyTTM = 0, mgjzc = 0;

            float nowPrice = fieldSingle[FieldIndex.Now];
            if (nowPrice == 0)
                nowPrice = fieldSingle[FieldIndex.PreClose];

            if (fieldDouble.ContainsKey(FieldIndex.ZGB))
                zsz = (fieldDouble[FieldIndex.ZGB]) * nowPrice;

            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(dataPacket.Code))
                DetailData.FieldIndexDataInt32[dataPacket.Code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;
            switch (mt)
            {
                case MarketType.SHALev1:
                case MarketType.SZALev1:
                case MarketType.SHConvertBondLev1:
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHFundLev1:
                case MarketType.SZFundLev1:
                    if (fieldDouble.ContainsKey(FieldIndex.NetAShare))
                        ltsz = (fieldDouble[FieldIndex.NetAShare]) * nowPrice;
                    break;
                case MarketType.SHBLev1:
                case MarketType.SZBLev1:
                case MarketType.SHBLev2:
                case MarketType.SZBLev2:
                    if (fieldDouble.ContainsKey(FieldIndex.NetBShare))
                        ltsz = (fieldDouble[FieldIndex.NetBShare]) * nowPrice;
                    break;
                default:
                    if (fieldDouble.ContainsKey(FieldIndex.NetAShare))
                        ltsz = (fieldDouble[FieldIndex.NetAShare]) * nowPrice;
                    break;
            }

            if (fieldDouble.ContainsKey(FieldIndex.EpsTtm))
                mgsyTTM = (fieldDouble[FieldIndex.EpsTtm]);
            if (mgsyTTM != 0)
                fieldSingle[FieldIndex.PETTM] = Convert.ToSingle(nowPrice / mgsyTTM);

            if (fieldDouble.ContainsKey(FieldIndex.MGJZC))
                mgjzc = (fieldDouble[FieldIndex.MGJZC]);
            if (mgjzc != 0)
                fieldSingle[FieldIndex.PB] = Convert.ToSingle(nowPrice / mgjzc);

            fieldDouble[FieldIndex.ZSZ] = zsz;
            fieldDouble[FieldIndex.LTSZ] = ltsz;

            float highw52 = 0, loww52 = 0, high = 0, low = 0, preClose = 0;
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                low = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.HighW52))
                highw52 = (fieldSingle[FieldIndex.HighW52]);
            if (fieldSingle.ContainsKey(FieldIndex.LowW52))
                loww52 = (fieldSingle[FieldIndex.LowW52]);
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (low.Equals(0))
                low = preClose;
            if (low.Equals(0) && !loww52.Equals(0))
                fieldSingle[FieldIndex.LowW52] = loww52;
            else if (!low.Equals(0) && loww52.Equals(0))
                fieldSingle[FieldIndex.LowW52] = low;
            else
                fieldSingle[FieldIndex.LowW52] = Math.Min(low, loww52);
            fieldSingle[FieldIndex.HighW52] = Math.Max(high, highw52);
            SetLimitedPrice(dataPacket.Code);

            //如果是交易所债券，计算挂单的ytm
            float buyPrice1 = 0,
                buyPrice2 = 0,
                buyPrice3 = 0,
                buyPrice4 = 0,
                buyPrice5 = 0,
                buyPrice6 = 0,
                buyPrice7 = 0,
                buyPrice8 = 0,
                buyPrice9 = 0,
                buyPrice10 = 0,
                sellPrice1 = 0,
                sellPrice2 = 0,
                sellPrice3 = 0,
                sellPrice4 = 0,
                sellPrice5 = 0,
                sellPrice6 = 0,
                sellPrice7 = 0,
                sellPrice8 = 0,
                sellPrice9 = 0,
                sellPrice10 = 0;

            switch (mt)
            {
                case MarketType.SHConvertBondLev1:
                case MarketType.SHConvertBondLev2:
                case MarketType.SZConvertBondLev1:
                case MarketType.SZConvertBondLev2:
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice1)) buyPrice1 = (fieldSingle[FieldIndex.BuyPrice1]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice2)) buyPrice2 = (fieldSingle[FieldIndex.BuyPrice2]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice3)) buyPrice3 = (fieldSingle[FieldIndex.BuyPrice3]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice4)) buyPrice4 = (fieldSingle[FieldIndex.BuyPrice4]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice5)) buyPrice5 = (fieldSingle[FieldIndex.BuyPrice5]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice6)) buyPrice6 = (fieldSingle[FieldIndex.BuyPrice6]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice7)) buyPrice7 = (fieldSingle[FieldIndex.BuyPrice7]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice8)) buyPrice8 = (fieldSingle[FieldIndex.BuyPrice8]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice9)) buyPrice9 = (fieldSingle[FieldIndex.BuyPrice9]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice10))
                        buyPrice10 = (fieldSingle[FieldIndex.BuyPrice10]);

                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice1))
                        sellPrice1 = (fieldSingle[FieldIndex.SellPrice1]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice2))
                        sellPrice2 = (fieldSingle[FieldIndex.SellPrice2]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice3))
                        sellPrice3 = (fieldSingle[FieldIndex.SellPrice3]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice4))
                        sellPrice4 = (fieldSingle[FieldIndex.SellPrice4]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice5))
                        sellPrice5 = (fieldSingle[FieldIndex.SellPrice5]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice6))
                        sellPrice6 = (fieldSingle[FieldIndex.SellPrice6]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice7))
                        sellPrice7 = (fieldSingle[FieldIndex.SellPrice7]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice8))
                        sellPrice8 = (fieldSingle[FieldIndex.SellPrice8]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice9))
                        sellPrice9 = (fieldSingle[FieldIndex.SellPrice9]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice10))
                        sellPrice10 = (fieldSingle[FieldIndex.SellPrice10]);

                    SetBondYtm(buyPrice1, dataPacket.Code, FieldIndex.BuyPriceYtm1, true);
                    SetBondYtm(buyPrice2, dataPacket.Code, FieldIndex.BuyPriceYtm2, true);
                    SetBondYtm(buyPrice3, dataPacket.Code, FieldIndex.BuyPriceYtm3, true);
                    SetBondYtm(buyPrice4, dataPacket.Code, FieldIndex.BuyPriceYtm4, true);
                    SetBondYtm(buyPrice5, dataPacket.Code, FieldIndex.BuyPriceYtm5, true);
                    SetBondYtm(buyPrice6, dataPacket.Code, FieldIndex.BuyPriceYtm6, true);
                    SetBondYtm(buyPrice7, dataPacket.Code, FieldIndex.BuyPriceYtm7, true);
                    SetBondYtm(buyPrice8, dataPacket.Code, FieldIndex.BuyPriceYtm8, true);
                    SetBondYtm(buyPrice9, dataPacket.Code, FieldIndex.BuyPriceYtm9, true);
                    SetBondYtm(buyPrice10, dataPacket.Code, FieldIndex.BuyPriceYtm10, true);

                    SetBondYtm(sellPrice1, dataPacket.Code, FieldIndex.SellPriceYtm1, true);
                    SetBondYtm(sellPrice2, dataPacket.Code, FieldIndex.SellPriceYtm2, true);
                    SetBondYtm(sellPrice3, dataPacket.Code, FieldIndex.SellPriceYtm3, true);
                    SetBondYtm(sellPrice4, dataPacket.Code, FieldIndex.SellPriceYtm4, true);
                    SetBondYtm(sellPrice5, dataPacket.Code, FieldIndex.SellPriceYtm5, true);
                    SetBondYtm(sellPrice6, dataPacket.Code, FieldIndex.SellPriceYtm6, true);
                    SetBondYtm(sellPrice7, dataPacket.Code, FieldIndex.SellPriceYtm7, true);
                    SetBondYtm(sellPrice8, dataPacket.Code, FieldIndex.SellPriceYtm8, true);
                    SetBondYtm(sellPrice9, dataPacket.Code, FieldIndex.SellPriceYtm9, true);
                    SetBondYtm(sellPrice10, dataPacket.Code, FieldIndex.SellPriceYtm10, true);
                    break;
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHNonConvertBondLev2:
                case MarketType.SZNonConvertBondLev1:
                case MarketType.SZNonConvertBondLev2:
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice1)) buyPrice1 = (fieldSingle[FieldIndex.BuyPrice1]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice2)) buyPrice2 = (fieldSingle[FieldIndex.BuyPrice2]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice3)) buyPrice3 = (fieldSingle[FieldIndex.BuyPrice3]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice4)) buyPrice4 = (fieldSingle[FieldIndex.BuyPrice4]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice5)) buyPrice5 = (fieldSingle[FieldIndex.BuyPrice5]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice6)) buyPrice6 = (fieldSingle[FieldIndex.BuyPrice6]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice7)) buyPrice7 = (fieldSingle[FieldIndex.BuyPrice7]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice8)) buyPrice8 = (fieldSingle[FieldIndex.BuyPrice8]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice9)) buyPrice9 = (fieldSingle[FieldIndex.BuyPrice9]);
                    if (fieldSingle.ContainsKey(FieldIndex.BuyPrice10))
                        buyPrice10 = (fieldSingle[FieldIndex.BuyPrice10]);

                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice1))
                        sellPrice1 = (fieldSingle[FieldIndex.SellPrice1]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice2))
                        sellPrice2 = (fieldSingle[FieldIndex.SellPrice2]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice3))
                        sellPrice3 = (fieldSingle[FieldIndex.SellPrice3]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice4))
                        sellPrice4 = (fieldSingle[FieldIndex.SellPrice4]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice5))
                        sellPrice5 = (fieldSingle[FieldIndex.SellPrice5]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice6))
                        sellPrice6 = (fieldSingle[FieldIndex.SellPrice6]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice7))
                        sellPrice7 = (fieldSingle[FieldIndex.SellPrice7]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice8))
                        sellPrice8 = (fieldSingle[FieldIndex.SellPrice8]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice9))
                        sellPrice9 = (fieldSingle[FieldIndex.SellPrice9]);
                    if (fieldSingle.ContainsKey(FieldIndex.SellPrice10))
                        sellPrice10 = (fieldSingle[FieldIndex.SellPrice10]);

                    SetBondYtm(buyPrice1, dataPacket.Code, FieldIndex.BuyPriceYtm1, false);
                    SetBondYtm(buyPrice2, dataPacket.Code, FieldIndex.BuyPriceYtm2, false);
                    SetBondYtm(buyPrice3, dataPacket.Code, FieldIndex.BuyPriceYtm3, false);
                    SetBondYtm(buyPrice4, dataPacket.Code, FieldIndex.BuyPriceYtm4, false);
                    SetBondYtm(buyPrice5, dataPacket.Code, FieldIndex.BuyPriceYtm5, false);
                    SetBondYtm(buyPrice6, dataPacket.Code, FieldIndex.BuyPriceYtm6, false);
                    SetBondYtm(buyPrice7, dataPacket.Code, FieldIndex.BuyPriceYtm7, false);
                    SetBondYtm(buyPrice8, dataPacket.Code, FieldIndex.BuyPriceYtm8, false);
                    SetBondYtm(buyPrice9, dataPacket.Code, FieldIndex.BuyPriceYtm9, false);
                    SetBondYtm(buyPrice10, dataPacket.Code, FieldIndex.BuyPriceYtm10, false);

                    SetBondYtm(sellPrice1, dataPacket.Code, FieldIndex.SellPriceYtm1, false);
                    SetBondYtm(sellPrice2, dataPacket.Code, FieldIndex.SellPriceYtm2, false);
                    SetBondYtm(sellPrice3, dataPacket.Code, FieldIndex.SellPriceYtm3, false);
                    SetBondYtm(sellPrice4, dataPacket.Code, FieldIndex.SellPriceYtm4, false);
                    SetBondYtm(sellPrice5, dataPacket.Code, FieldIndex.SellPriceYtm5, false);
                    SetBondYtm(sellPrice6, dataPacket.Code, FieldIndex.SellPriceYtm6, false);
                    SetBondYtm(sellPrice7, dataPacket.Code, FieldIndex.SellPriceYtm7, false);
                    SetBondYtm(sellPrice8, dataPacket.Code, FieldIndex.SellPriceYtm8, false);
                    SetBondYtm(sellPrice9, dataPacket.Code, FieldIndex.SellPriceYtm9, false);
                    SetBondYtm(sellPrice10, dataPacket.Code, FieldIndex.SellPriceYtm10, false);
                    break;
                case MarketType.SHINDEX:
                case MarketType.SZINDEX:
                    SetIndexStaticDataPacketAfterNewPrice(dataPacket);
                    break;
                default:
                    break;
            }
        }

        private void SetStockDetailPacket(ResNOrderStockDetailLev2DataPacket dataPacket)
        {

            if (dataPacket.OrderDetailData == null)
                return;
            foreach (StockOrderDetailDataRec onePacket in dataPacket.OrderDetailData)
            {
                Dictionary<FieldIndex, float> fieldSingle;
                Dictionary<FieldIndex, double> fieldDouble;
                Dictionary<FieldIndex, long> fieldInt64;

                if (!DetailData.FieldIndexDataSingle.TryGetValue(onePacket.Code, out fieldSingle))
                    continue;
                if (!DetailData.FieldIndexDataDouble.TryGetValue(onePacket.Code, out fieldDouble))
                    continue;
                if (!DetailData.FieldIndexDataInt64.TryGetValue(onePacket.Code, out fieldInt64))
                    continue;

                double zsz = 0;
                double ltsz = 0;
                double mgsyTTM = 0, mgjzc = 0;
                double netShare = 0;


                float nowPrice = Convert.ToSingle(fieldSingle[FieldIndex.Now]);
                if (nowPrice == 0)
                    nowPrice = Convert.ToSingle(fieldSingle[FieldIndex.PreClose]);

                if (fieldDouble.ContainsKey(FieldIndex.ZGB))
                    zsz = (fieldDouble[FieldIndex.ZGB]) * nowPrice;

                MarketType mt = MarketType.NA;
                int mtInt = 0;
                if (DetailData.FieldIndexDataInt32.ContainsKey(onePacket.Code))
                    DetailData.FieldIndexDataInt32[onePacket.Code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;

                switch (mt)
                {
                    case MarketType.SHBLev1:
                    case MarketType.SZBLev1:
                    case MarketType.SHBLev2:
                    case MarketType.SZBLev2:
                        if (fieldDouble.ContainsKey(FieldIndex.NetBShare))
                            netShare = Convert.ToDouble(fieldDouble[FieldIndex.NetBShare]);
                        break;
                    default:
                        if (fieldDouble.ContainsKey(FieldIndex.NetAShare))
                            netShare = Convert.ToDouble(fieldDouble[FieldIndex.NetAShare]);
                        break;
                }


                ltsz = netShare * nowPrice;

                if (fieldDouble.ContainsKey(FieldIndex.EpsTtm))
                    mgsyTTM = (fieldDouble[FieldIndex.EpsTtm]);
                if (mgsyTTM != 0)
                    fieldSingle[FieldIndex.PETTM] = Convert.ToSingle(nowPrice / mgsyTTM);

                if (fieldDouble.ContainsKey(FieldIndex.MGJZC))
                    mgjzc = (fieldDouble[FieldIndex.MGJZC]);
                if (mgjzc != 0)
                    fieldSingle[FieldIndex.PB] = Convert.ToSingle(nowPrice / mgjzc);

                fieldDouble[FieldIndex.ZSZ] = zsz;
                fieldDouble[FieldIndex.LTSZ] = ltsz;

                float highw52 = 0, loww52 = 0, high = 0, low = 0, preClose = 0, now = 0;
                if (fieldSingle.ContainsKey(FieldIndex.Now))
                    now = Convert.ToSingle(fieldSingle[FieldIndex.Now]);
                if (fieldSingle.ContainsKey(FieldIndex.High))
                    high = Convert.ToSingle(fieldSingle[FieldIndex.High]);
                if (fieldSingle.ContainsKey(FieldIndex.High))
                    low = Convert.ToSingle(fieldSingle[FieldIndex.High]);
                if (fieldSingle.ContainsKey(FieldIndex.HighW52))
                    highw52 = Convert.ToSingle(fieldSingle[FieldIndex.HighW52]);
                if (fieldSingle.ContainsKey(FieldIndex.LowW52))
                    loww52 = Convert.ToSingle(fieldSingle[FieldIndex.LowW52]);
                if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                    preClose = Convert.ToSingle(fieldSingle[FieldIndex.PreClose]);
                if (low.Equals(0))
                    low = preClose;
                if (low.Equals(0) && !loww52.Equals(0))
                    fieldSingle[FieldIndex.LowW52] = loww52;
                else if (!low.Equals(0) && loww52.Equals(0))
                    fieldSingle[FieldIndex.LowW52] = low;
                else
                    fieldSingle[FieldIndex.LowW52] = Math.Min(low, loww52);
                fieldSingle[FieldIndex.HighW52] = Math.Max(high, highw52);
                if (preClose != 0 && now != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }

                long volume = 0;
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (netShare != 0)
                    fieldSingle[FieldIndex.Turnover] = Convert.ToSingle(volume * 1.0f / netShare);


                SetLimitedPrice(onePacket.Code);

                //如果是交易所债券，计算挂单的ytm

                float buyPrice1 = 0,
                    buyPrice2 = 0,
                    buyPrice3 = 0,
                    buyPrice4 = 0,
                    buyPrice5 = 0,
                    buyPrice6 = 0,
                    buyPrice7 = 0,
                    buyPrice8 = 0,
                    buyPrice9 = 0,
                    buyPrice10 = 0,
                    sellPrice1 = 0,
                    sellPrice2 = 0,
                    sellPrice3 = 0,
                    sellPrice4 = 0,
                    sellPrice5 = 0,
                    sellPrice6 = 0,
                    sellPrice7 = 0,
                    sellPrice8 = 0,
                    sellPrice9 = 0,
                    sellPrice10 = 0;

                switch (mt)
                {
                    case MarketType.SHConvertBondLev1:
                    case MarketType.SHConvertBondLev2:
                    case MarketType.SZConvertBondLev1:
                    case MarketType.SZConvertBondLev2:
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice1))
                            buyPrice1 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice1]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice2))
                            buyPrice2 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice2]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice3))
                            buyPrice3 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice3]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice4))
                            buyPrice4 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice4]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice5))
                            buyPrice5 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice5]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice6))
                            buyPrice6 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice6]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice7))
                            buyPrice7 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice7]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice8))
                            buyPrice8 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice8]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice9))
                            buyPrice9 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice9]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice10))
                            buyPrice10 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice10]);

                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice1))
                            sellPrice1 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice1]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice2))
                            sellPrice2 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice2]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice3))
                            sellPrice3 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice3]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice4))
                            sellPrice4 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice4]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice5))
                            sellPrice5 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice5]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice6))
                            sellPrice6 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice6]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice7))
                            sellPrice7 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice7]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice8))
                            sellPrice8 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice8]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice9))
                            sellPrice9 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice9]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice10))
                            sellPrice10 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice10]);

                        SetBondYtm(buyPrice1, onePacket.Code, FieldIndex.BuyPriceYtm1, true);
                        SetBondYtm(buyPrice2, onePacket.Code, FieldIndex.BuyPriceYtm2, true);
                        SetBondYtm(buyPrice3, onePacket.Code, FieldIndex.BuyPriceYtm3, true);
                        SetBondYtm(buyPrice4, onePacket.Code, FieldIndex.BuyPriceYtm4, true);
                        SetBondYtm(buyPrice5, onePacket.Code, FieldIndex.BuyPriceYtm5, true);
                        SetBondYtm(buyPrice6, onePacket.Code, FieldIndex.BuyPriceYtm6, true);
                        SetBondYtm(buyPrice7, onePacket.Code, FieldIndex.BuyPriceYtm7, true);
                        SetBondYtm(buyPrice8, onePacket.Code, FieldIndex.BuyPriceYtm8, true);
                        SetBondYtm(buyPrice9, onePacket.Code, FieldIndex.BuyPriceYtm9, true);
                        SetBondYtm(buyPrice10, onePacket.Code, FieldIndex.BuyPriceYtm10, true);

                        SetBondYtm(sellPrice1, onePacket.Code, FieldIndex.SellPriceYtm1, true);
                        SetBondYtm(sellPrice2, onePacket.Code, FieldIndex.SellPriceYtm2, true);
                        SetBondYtm(sellPrice3, onePacket.Code, FieldIndex.SellPriceYtm3, true);
                        SetBondYtm(sellPrice4, onePacket.Code, FieldIndex.SellPriceYtm4, true);
                        SetBondYtm(sellPrice5, onePacket.Code, FieldIndex.SellPriceYtm5, true);
                        SetBondYtm(sellPrice6, onePacket.Code, FieldIndex.SellPriceYtm6, true);
                        SetBondYtm(sellPrice7, onePacket.Code, FieldIndex.SellPriceYtm7, true);
                        SetBondYtm(sellPrice8, onePacket.Code, FieldIndex.SellPriceYtm8, true);
                        SetBondYtm(sellPrice9, onePacket.Code, FieldIndex.SellPriceYtm9, true);
                        SetBondYtm(sellPrice10, onePacket.Code, FieldIndex.SellPriceYtm10, true);
                        break;
                    case MarketType.SHNonConvertBondLev1:
                    case MarketType.SHNonConvertBondLev2:
                    case MarketType.SZNonConvertBondLev1:
                    case MarketType.SZNonConvertBondLev2:
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice1))
                            buyPrice1 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice1]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice2))
                            buyPrice2 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice2]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice3))
                            buyPrice3 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice3]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice4))
                            buyPrice4 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice4]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice5))
                            buyPrice5 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice5]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice6))
                            buyPrice6 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice6]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice7))
                            buyPrice7 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice7]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice8))
                            buyPrice8 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice8]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice9))
                            buyPrice9 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice9]);
                        if (fieldSingle.ContainsKey(FieldIndex.BuyPrice10))
                            buyPrice10 = Convert.ToSingle(fieldSingle[FieldIndex.BuyPrice10]);

                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice1))
                            sellPrice1 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice1]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice2))
                            sellPrice2 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice2]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice3))
                            sellPrice3 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice3]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice4))
                            sellPrice4 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice4]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice5))
                            sellPrice5 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice5]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice6))
                            sellPrice6 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice6]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice7))
                            sellPrice7 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice7]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice8))
                            sellPrice8 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice8]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice9))
                            sellPrice9 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice9]);
                        if (fieldSingle.ContainsKey(FieldIndex.SellPrice10))
                            sellPrice10 = Convert.ToSingle(fieldSingle[FieldIndex.SellPrice10]);

                        SetBondYtm(buyPrice1, onePacket.Code, FieldIndex.BuyPriceYtm1, false);
                        SetBondYtm(buyPrice2, onePacket.Code, FieldIndex.BuyPriceYtm2, false);
                        SetBondYtm(buyPrice3, onePacket.Code, FieldIndex.BuyPriceYtm3, false);
                        SetBondYtm(buyPrice4, onePacket.Code, FieldIndex.BuyPriceYtm4, false);
                        SetBondYtm(buyPrice5, onePacket.Code, FieldIndex.BuyPriceYtm5, false);
                        SetBondYtm(buyPrice6, onePacket.Code, FieldIndex.BuyPriceYtm6, false);
                        SetBondYtm(buyPrice7, onePacket.Code, FieldIndex.BuyPriceYtm7, false);
                        SetBondYtm(buyPrice8, onePacket.Code, FieldIndex.BuyPriceYtm8, false);
                        SetBondYtm(buyPrice9, onePacket.Code, FieldIndex.BuyPriceYtm9, false);
                        SetBondYtm(buyPrice10, onePacket.Code, FieldIndex.BuyPriceYtm10, false);

                        SetBondYtm(sellPrice1, onePacket.Code, FieldIndex.SellPriceYtm1, false);
                        SetBondYtm(sellPrice2, onePacket.Code, FieldIndex.SellPriceYtm2, false);
                        SetBondYtm(sellPrice3, onePacket.Code, FieldIndex.SellPriceYtm3, false);
                        SetBondYtm(sellPrice4, onePacket.Code, FieldIndex.SellPriceYtm4, false);
                        SetBondYtm(sellPrice5, onePacket.Code, FieldIndex.SellPriceYtm5, false);
                        SetBondYtm(sellPrice6, onePacket.Code, FieldIndex.SellPriceYtm6, false);
                        SetBondYtm(sellPrice7, onePacket.Code, FieldIndex.SellPriceYtm7, false);
                        SetBondYtm(sellPrice8, onePacket.Code, FieldIndex.SellPriceYtm8, false);
                        SetBondYtm(sellPrice9, onePacket.Code, FieldIndex.SellPriceYtm9, false);
                        SetBondYtm(sellPrice10, onePacket.Code, FieldIndex.SellPriceYtm10, false);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetBondYtm(float price, int code, FieldIndex ytmField, bool isConvertBond)
        {
            if (!price.Equals(0))
            {
                Dictionary<float, float> memYtmData;

                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(code))
                    DetailData.FieldIndexDataString[code].TryGetValue(FieldIndex.EMCode, out emcode);

                Dictionary<FieldIndex, float> fieldSingle;
                if (!DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[code] = fieldSingle;
                }

                if (DetailData.AllBondYtmDataRec.TryGetValue(code, out memYtmData))
                {
                    if (memYtmData.ContainsKey(price))
                        fieldSingle[ytmField] = memYtmData[price];
                    else
                    {
                        string cmd = string.Empty;
                        if (isConvertBond)
                            cmd =
                                string.Format(
                                    @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=0 columns=netPrice,ytm",
                                    emcode, price);
                        else
                            cmd =
                                string.Format(
                                    @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=1 columns=netPrice,ytm",
                                    emcode, price);

                        try
                        {
                            DataTable dt = Requestor.GetDataTable(cmd, null, null, null);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                float ytm = Convert.ToSingle(dt.Rows[0]["ytm"]);
                                if (!float.IsInfinity(ytm))
                                {
                                    fieldSingle[ytmField] = ytm;
                                    memYtmData[price] = ytm;
                                    break;
                                }
                            }
                            dt.Dispose();
                        }
                        catch (Exception e)
                        {
                            LogUtilities.LogMessage("行情调用债券计算服务，计算ytm时异常" + e.Message);
                        }

                    }
                }
                else
                {
                    string cmd = string.Empty;
                    if (isConvertBond)
                        cmd =
                            string.Format(
                                @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=0 columns=netPrice,ytm", emcode,
                                price);
                    else
                        cmd =
                            string.Format(
                                @"rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=1 columns=netPrice,ytm", emcode,
                                price);

                    try
                    {
                        DataTable dt = Requestor.GetDataTable(cmd, null, null, null);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            float ytm = Convert.ToSingle(dt.Rows[0]["ytm"]);
                            if (!float.IsInfinity(ytm))
                            {
                                fieldSingle[ytmField] = ytm;
                                Dictionary<float, float> tmpData = new Dictionary<float, float>(1);
                                tmpData[price] = ytm;
                                DetailData.AllBondYtmDataRec[code] = tmpData;
                                break;
                            }
                        }
                        dt.Dispose();
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("行情调用债券计算服务，计算ytm时异常" + e.Message);
                    }
                }
            }
        }

        private void SetStockDetailPacket(ResIndexFuturesDetailDataPacket dataPacket)
        {

            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, double> fieldDouble;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, string> fieldString;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(dataPacket.Code, out fieldSingle))
                return;
            if (!DetailData.FieldIndexDataDouble.TryGetValue(dataPacket.Code, out fieldDouble))
                return;
            if (!DetailData.FieldIndexDataInt64.TryGetValue(dataPacket.Code, out fieldInt64))
                return;
            if (!DetailData.FieldIndexDataInt32.TryGetValue(dataPacket.Code, out fieldInt32))
                return;
            if (!DetailData.FieldIndexDataString.TryGetValue(dataPacket.Code, out fieldString))
                return;
            fieldSingle[FieldIndex.Difference] = (fieldSingle[FieldIndex.Now]) -
                                                 (fieldSingle[FieldIndex.PreSettlementPrice]);

            float high = 0, low = 0;
            float tmpDiffRange = 0;
            float preSettlementPrice = 0;
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);

            preSettlementPrice = (fieldSingle[FieldIndex.PreSettlementPrice]);
            if (preSettlementPrice != 0)
            {
                tmpDiffRange = (fieldSingle[FieldIndex.Difference]) /
                               preSettlementPrice;
                fieldSingle[FieldIndex.Delta] = (high - low) / preSettlementPrice;

            }
            fieldSingle[FieldIndex.DifferRange] = tmpDiffRange;

            int tmpDayOI = Convert.ToInt32(fieldInt64[FieldIndex.OpenInterest] -
                                           fieldInt64[FieldIndex.PreOpenInterest]);
            fieldInt32[FieldIndex.OpenInterestDaily] = tmpDayOI;


            double amount = 0;
            long volume = 0;
            if (fieldDouble.ContainsKey(FieldIndex.Amount))
                amount = (fieldDouble[FieldIndex.Amount]);
            if (fieldInt64.ContainsKey(FieldIndex.Volume))
                volume = (fieldInt64[FieldIndex.Volume]);

            int factor = 1;
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(dataPacket.Code))
                DetailData.FieldIndexDataInt32[dataPacket.Code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;

            if (mt == MarketType.IF)
                factor = 300;
            else if (mt == MarketType.GoverFutures)
                factor = 10000;
            if (volume != 0)
                fieldSingle[FieldIndex.AveragePrice] = Convert.ToSingle(amount / (volume * factor));

            string tmpStatus = DataPacket.GetIFOpenCloseStatus(fieldInt64[FieldIndex.LastVolume],
                fieldInt64[FieldIndex.CurOI],
                fieldInt32[FieldIndex.BSFlag]);
            fieldString[FieldIndex.OpenCloseStatus] = tmpStatus;

            SetLimitedPrice(dataPacket.Code);

        }

        private void SetIndexDetailPacket(ResIndexDetailDataPacket dataPacket)
        {
            foreach (int code in dataPacket.Codes)
            {
                Dictionary<FieldIndex, float> fieldSingle;
                if (!DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                    return;
                float preClose5 = 0, preClose20 = 0, preClose60 = 0, preCloseYtd = 0, now = 0, preClose = 0;
                if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                    preClose5 = Convert.ToSingle(fieldSingle[FieldIndex.PreCloseDay5]);
                if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                    preClose20 = Convert.ToSingle(fieldSingle[FieldIndex.PreCloseDay20]);
                if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                    preClose60 = Convert.ToSingle(fieldSingle[FieldIndex.PreCloseDay60]);
                if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                    preCloseYtd = Convert.ToSingle(fieldSingle[FieldIndex.PreCloseDayYTD]);
                if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                    preClose = Convert.ToSingle(fieldSingle[FieldIndex.PreClose]);
                if (fieldSingle.ContainsKey(FieldIndex.Now))
                    now = Convert.ToSingle(fieldSingle[FieldIndex.Now]);
                if (now.Equals(0))
                    now = preClose;
                if (!now.Equals(0))
                {
                    if (!preClose5.Equals(0))
                        fieldSingle[FieldIndex.DifferRange5D] = (now - preClose5) / preClose5;
                    if (!preClose20.Equals(0))
                        fieldSingle[FieldIndex.DifferRange20D] = (now - preClose20) / preClose20;
                    if (!preClose60.Equals(0))
                        fieldSingle[FieldIndex.DifferRange60D] = (now - preClose60) / preClose60;
                    if (!preCloseYtd.Equals(0))
                        fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
                }
            }
        }

        private void SetLimitedPrice(int code)
        {
            float upLimited = 0;
            float downLimited = 0;
            string name = string.Empty;
            float preClose = 0;

            Dictionary<FieldIndex, string> fieldString;
            Dictionary<FieldIndex, float> fieldSingle;
            if (!DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                return;
            if (!DetailData.FieldIndexDataString.TryGetValue(code, out fieldString))
                return;

            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;

            if (!fieldString.TryGetValue(FieldIndex.Name, out name))
                return;


            preClose = fieldSingle[FieldIndex.PreClose];
            if (mt == MarketType.IF || mt == MarketType.SHF || mt == MarketType.CZC || mt == MarketType.CZC ||
                mt == MarketType.GoverFutures || mt == MarketType.CHFAG || mt == MarketType.CHFCU)
                preClose = fieldSingle[FieldIndex.PreSettlementPrice];


            if (name.ToUpper().Contains("ST"))
            {
                upLimited = preClose * 1.05f;
                downLimited = preClose * 0.95f;
            }
            else if (name.ToUpper().Contains("PT"))
            {
                upLimited = preClose * 1.05f;
            }
            else if (name.ToUpper().StartsWith("N"))
            {

            }
            else
            {
                switch (mt)
                {
                    case MarketType.SHALev1:
                    case MarketType.SHALev2:
                    case MarketType.SZALev1:
                    case MarketType.SZALev2:
                    case MarketType.SHBLev1:
                    case MarketType.SHBLev2:
                    case MarketType.SZBLev1:
                    case MarketType.SZBLev2:
                    case MarketType.SHFundLev1:
                    case MarketType.SHFundLev2:
                    case MarketType.SZFundLev1:
                    case MarketType.SZFundLev2:
                    case MarketType.SZNonConvertBondLev1:
                    case MarketType.SZNonConvertBondLev2:
                    case MarketType.SZConvertBondLev1:
                    case MarketType.SZConvertBondLev2:
                    case MarketType.SHConvertBondLev1:
                    case MarketType.SHConvertBondLev2:
                    case MarketType.SHF:
                    case MarketType.DCE:
                    case MarketType.CZC:
                    case MarketType.IF:
                    case MarketType.CHFAG:
                    case MarketType.CHFCU:
                        upLimited = preClose * 1.1f;
                        downLimited = preClose * 0.9f;
                        break;
                    case MarketType.GoverFutures:
                        upLimited = preClose * 1.02f;
                        downLimited = preClose * 0.98f;
                        break;
                }
            }

            if (upLimited != 0)
                fieldSingle[FieldIndex.UpLimit] = upLimited;

            if (downLimited != 0)
                fieldSingle[FieldIndex.DownLimit] = downLimited;

        }

        private void SetOrgReportDataPacket(ResBlockStockReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateStockData(code);
        }

        private void SetOrgReportDataPacket(ResFundStockReportOrgDataPacket dataPacket)
        {
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            foreach (int code in dataPacket.CodeList)
            {
                if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                    DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;
                switch (mt)
                {
                    case MarketType.SHFundLev1:
                    case MarketType.SHFundLev2:
                    case MarketType.SZFundLev1:
                    case MarketType.SZFundLev2:
                        CaculateCloseFundData(code);
                        break;
                    case MarketType.MonetaryFund:
                    case MarketType.NonMonetaryFund:
                        CaculateOpenFundData(code);
                        break;
                }
            }

        }

        private void SetOrgReportDataPacket(ResHKStockReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateHKData(code);
        }

        private void SetOrgReportDataPacket(ResBondStockReportOrgDataPacket dataPacket)
        {
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            foreach (int code in dataPacket.CodeList)
            {
                if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                    DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;
                switch (mt)
                {
                    case MarketType.SHNonConvertBondLev1:
                    case MarketType.SHNonConvertBondLev2:
                        CaculateSHNonConvertBondData(code);
                        break;
                    case MarketType.SZNonConvertBondLev1:
                    case MarketType.SZNonConvertBondLev2:
                        CaculateSZNonConvertBondData(code);
                        break;
                    case MarketType.SHConvertBondLev1:
                    case MarketType.SHConvertBondLev2:
                        CaculateSHConvertBondData(code);
                        break;
                    case MarketType.SZConvertBondLev1:
                    case MarketType.SZConvertBondLev2:
                        CaculateSZConvertBondData(code);
                        break;
                    default:
                        CaculateIBBondData(code);
                        break;

                }
            }
        }

        private void SetOrgReportDataPacket(ResFinanceStockReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFinanceData(code);
        }

        private void SetOrgReportDataPacket(ResRateReportOrgDataPacket dataPacket)
        {
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            foreach (int code in dataPacket.CodeList)
            {
                if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                    DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;
                switch (mt)
                {
                    case MarketType.Chibor:
                    case MarketType.SZRepurchaseLevel1:
                    case MarketType.SZRepurchaseLevel2:
                    case MarketType.SHRepurchaseLevel1:
                    case MarketType.SHRepurchaseLevel2:
                    case MarketType.InterBankRepurchase:
                    case MarketType.RateSwap:
                        CacultateRateData(code);
                        break;
                    case MarketType.InterestRate:
                        CaculateCSIndexData(code);
                        break;
                }
            }
        }

        private void SetOrgReportDataPacket(ResFinanceReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFinanceData(code);
        }

        private void SetOrgReportDataPacket(ResIndexFutruesReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFuturesData(code);
        }

        private void SetOrgReportDataPacket(ResFuturesStockReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFuturesData(code);
        }

        private void SetOrgReportDataPacket(ResCustomReportOrgDataPacket dataPacket)
        {
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            foreach (int code in dataPacket.CodeList)
            {
                if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                    DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;
                switch (mt)
                {
                    case MarketType.SHALev1:
                    case MarketType.SHALev2:
                    case MarketType.SZALev1:
                    case MarketType.SZALev2:
                    case MarketType.SHBLev1:
                    case MarketType.SHBLev2:
                    case MarketType.SZBLev1:
                    case MarketType.SZBLev2:

                        CaculateStockData(code);
                        break;
                    case MarketType.SHConvertBondLev1:
                    case MarketType.SHConvertBondLev2:
                        CaculateSHConvertBondData(code);
                        break;
                    case MarketType.SZConvertBondLev1:
                    case MarketType.SZConvertBondLev2:
                        CaculateSZConvertBondData(code);
                        break;
                    case MarketType.SHNonConvertBondLev1:
                    case MarketType.SHNonConvertBondLev2:
                        CaculateSHNonConvertBondData(code);
                        break;
                    case MarketType.SZNonConvertBondLev1:
                    case MarketType.SZNonConvertBondLev2:
                        CaculateSZNonConvertBondData(code);
                        break;
                    case MarketType.HK:
                    case MarketType.US:
                        CaculateHKData(code);
                        break;
                    case MarketType.IB:
                    case MarketType.BC:
                        CaculateIBBondData(code);
                        break;
                    case MarketType.SHFundLev1:
                    case MarketType.SHFundLev2:
                    case MarketType.SZFundLev1:
                    case MarketType.SZFundLev2:
                        CaculateCloseFundData(code);
                        break;
                    case MarketType.MonetaryFund:
                    case MarketType.NonMonetaryFund:
                        CaculateOpenFundData(code);
                        break;
                    case MarketType.SHF:
                    case MarketType.CZC:
                    case MarketType.DCE:
                    case MarketType.IF:
                    case MarketType.OSFuturesLMEElec:
                    case MarketType.OSFuturesLMEVenue:
                    case MarketType.CHFAG:
                    case MarketType.CHFCU:
                        CaculateFuturesData(code);
                        break;
                    case MarketType.Chibor:
                    case MarketType.SZRepurchaseLevel1:
                    case MarketType.SZRepurchaseLevel2:
                    case MarketType.SHRepurchaseLevel1:
                    case MarketType.SHRepurchaseLevel2:
                    case MarketType.InterBankRepurchase:
                    case MarketType.RateSwap:
                        CacultateRateData(code);
                        break;
                    case MarketType.CSIINDEX:
                    case MarketType.GLOBAL:
                    case MarketType.SHINDEX:
                    case MarketType.SZINDEX:
                    case MarketType.EMINDEX:
                    case MarketType.HSINDEX:
                    case MarketType.JKINDEX:
                    case MarketType.MalaysiaIndex:
                    case MarketType.KoreaIndex:
                    case MarketType.NikkeiIndex:
                    case MarketType.PhilippinesIndex:
                    case MarketType.SensexIndex:
                    case MarketType.SingaporeIndex:
                    case MarketType.TaiwanIndex:
                    case MarketType.NewZealandIndex:
                    case MarketType.NasdaqIndex:
                    case MarketType.DutchAEXIndex:
                    case MarketType.AustriaIndex:
                    case MarketType.NorwayIndex:
                    case MarketType.RussiaIndex:
                    case MarketType.CNINDEX:
                        CaculateIndexData(code);
                        break;
                    case MarketType.CSINDEX:
                    case MarketType.InterestRate:
                        CaculateCSIndexData(code);
                        break;
                }
            }
        }

        private void SetOrgReportDataPacket(ResBlockReportDataPacket dataPacket)
        {
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            foreach (int code in dataPacket.CodeList)
            {
                if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                    DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;
                switch (mt)
                {
                    case MarketType.CNINDEX:
                    case MarketType.CSIINDEX:
                    case MarketType.GLOBAL:
                    case MarketType.SHINDEX:
                    case MarketType.SZINDEX:
                    case MarketType.EMINDEX:
                    case MarketType.HSINDEX:
                    case MarketType.JKINDEX:
                    case MarketType.MalaysiaIndex:
                    case MarketType.KoreaIndex:
                    case MarketType.NikkeiIndex:
                    case MarketType.PhilippinesIndex:
                    case MarketType.SensexIndex:
                    case MarketType.SingaporeIndex:
                    case MarketType.TaiwanIndex:
                    case MarketType.NewZealandIndex:
                    case MarketType.NasdaqIndex:
                    case MarketType.DutchAEXIndex:
                    case MarketType.AustriaIndex:
                    case MarketType.NorwayIndex:
                    case MarketType.RussiaIndex:
                        CaculateIndexData(code);
                        break;
                    case MarketType.CSINDEX:
                        CaculateCSIndexData(code);
                        break;
                }
            }
        }

        private void SetOrgReportDataPacket(ResDDEReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateDDEData(code);
        }

        private void SetOrgReportDataPacket(ResCapitalFlowReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateCapitalFlowReportData(code);
        }

        private void SetOrgReportDataPacket(ResNetInFlowReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateNetInflowReportData(code);
        }

        private void SetOrgReportDataPacket(ResProfitForecastReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateProfitForestReportData(code);
        }

        private void SetOrgReportDataPacket(ResCustomDDEReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateDDEData(code);
        }

        private void SetOrgReportDataPacket(ResForexReportDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateHKData(code);
        }

        private void SetOrgReportDataPacket(ResUSStockReportDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateHKData(code);
        }

        private void SetOrgReportDataPacket(ResOSFuturesReportDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFuturesData(code);
        }

        private void SetOrgReportDataPacket(ResOSFuturesLMEReportDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFuturesData(code);
        }

        private void SetOrgReportDataPacket(ResCustomCapitalFlowReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateCapitalFlowReportData(code);
        }

        private void SetOrgReportDataPacket(ResCustomNetInFlowReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateNetInflowReportData(code);
        }

        private void SetOrgReportDataPacket(ResCustomProfitForecastReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateProfitForestReportData(code);
        }

        private void SetOrgReportDataPacket(ResCustomFinanceStockReportOrgDataPacket dataPacket)
        {
            foreach (int code in dataPacket.CodeList)
                CaculateFinanceData(code);
        }

        private void SetEmIndexDetailDataPacket(ResEMIndexDetailDataPacket dataPacket)
        {
            CaculateIndexStaticData(dataPacket.Code);
        }

        private void SetIndexStaticDataPacket(ResIndexStaticOrgDataPacket dataPacket)
        {
            CaculateIndexStaticData(dataPacket.Code);
        }

        private void SetIndexStaticDataPacketAfterNewPrice(ResStockDetailLev2DataPacket dataPacket)
        {
            CaculateIndexStaticData(dataPacket.Code);
        }

        #region 财富通的接口,废弃

        /**************
        private void SetReportDataPacket(ResSectorQuoteReportDataPacket dataPacket)
        {
            //             TextWriter textWriter = new StreamWriter("d:\\aaa.txt");
            //             int tmpIndex = 1;
            //             foreach (KeyValuePair<string,Dictionary<byte,object>> entry in dataPacket.DicFieldValue)
            //             {
            //                 //临时，写到txt
            //                 string tmpstr = tmpIndex.ToString() + "    " + entry.Key + "    " +
            //                                 _allDetailDataRec[entry.Key][FieldIndex.Name];
            //                 textWriter.WriteLine(tmpstr);
            //                 tmpIndex++;
            //             }
            // 
            //             textWriter.Close();
            foreach (KeyValuePair<int, Dictionary<byte, object>> entry in dataPacket.DicFieldValue)
            {

                Dictionary<FieldIndex, object> fieldKeyValue;
                if (AllDetailDataRec.TryGetValue(entry.Key, out fieldKeyValue))
                {

                    //写到客户端的detail
                    foreach (KeyValuePair<byte, object> reqFieldValue in entry.Value)
                    {

                        DicFieldIndexItem fieldIndexItem = ConvertFieldIndex.DicFieldIndex[(ReqFieldIndex)reqFieldValue.Key];
                        if (fieldIndexItem.FieldClient != FieldIndex.Na)
                        {
                            switch (fieldIndexItem.FieldFactor)
                            {
                                case FieldFactor.PriceFactor:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = Convert.ToInt32(reqFieldValue.Value) / 1000.0f;
                                    break;
                                case FieldFactor.RangeFactor:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = (Convert.ToInt32(reqFieldValue.Value)) / 100.0f;
                                    break;
                                case FieldFactor.NowPriceFactor:
                                    float nowPrice = Convert.ToInt32(reqFieldValue.Value) / 1000.0f;
                                    float oldPrice = Convert.ToSingle(fieldKeyValue[FieldIndex.Now]);
                                    if (oldPrice * 1000 != 0)
                                        fieldKeyValue[FieldIndex.DifferSpeed] =
                                            Convert.ToSingle((nowPrice - oldPrice) / oldPrice);
                                    fieldKeyValue[fieldIndexItem.FieldClient] = nowPrice;
                                    break;
                                default:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = reqFieldValue.Value;
                                    break;
                            }
                        }

                        //引起其他字段变化的字段
                        switch ((ReqFieldIndex)reqFieldValue.Key)
                        {
                            case ReqFieldIndex.Now:
                                float diff = 0;
                                float diffRange = 0;

                                if (Convert.ToInt32(fieldKeyValue[FieldIndex.Now]) > 0)
                                {
                                    float close = 0;
                                    if ((MarketType)Dc.GetDetailData(entry.Key, FieldIndex.Market) == MarketType.IF)
                                        close = Convert.ToSingle(fieldKeyValue[FieldIndex.PreSettlementPrice]);
                                    else
                                        close = Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);

                                    diff = Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) - close;

                                    if (close != 0)
                                        diffRange = diff / close;
                                }
                                fieldKeyValue[FieldIndex.Difference] = diff;
                                fieldKeyValue[FieldIndex.DifferRange] = diffRange;

                                //计算Pe
                                float pe = 0;
                                int quater =
                                    SecurityAttribute.GetQuarterBGRQ(Convert.ToInt32(fieldKeyValue[FieldIndex.BGQDate]));
                                float ser = 1;
                                switch (quater)
                                {
                                    case 1: ser = 4; break;
                                    case 2: ser = 2; break;
                                    case 3: ser = 0.75f; break;
                                    case 4: ser = 1; break;
                                    default: ser = 1; break;
                                }

                                if (Convert.ToDouble(fieldKeyValue[FieldIndex.MGSY]) != 0)
                                    pe = Convert.ToSingle((Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) /
                                                  Convert.ToDouble(fieldKeyValue[FieldIndex.MGSY])) / ser);

                                fieldKeyValue[FieldIndex.PE] = pe;

                                //计算PB
                                if (Convert.ToDouble(fieldKeyValue[FieldIndex.MGJZC]) != 0)
                                    fieldKeyValue[FieldIndex.PB] =
                                        Convert.ToSingle((Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) /
                                                 Convert.ToDouble(fieldKeyValue[FieldIndex.MGJZC])));

                                //计算流通市值
                                fieldKeyValue[FieldIndex.LTSZ] = Convert.ToDouble(Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]) *
                                                                Convert.ToSingle(fieldKeyValue[FieldIndex.Now]));
                                break;
                            case ReqFieldIndex.PreCloseDay2:
                                float priceDay2 = Convert.ToInt32(entry.Value[(byte)ReqFieldIndex.PreCloseDay2]) / 1000.0f;
                                fieldKeyValue[FieldIndex.DifferRange3D] =
                                    (Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) - priceDay2) / priceDay2;
                                break;
                            case ReqFieldIndex.PreCloseDay5:
                                float priceDay5 = Convert.ToInt32(entry.Value[(byte)ReqFieldIndex.PreCloseDay5]) / 1000.0f;
                                fieldKeyValue[FieldIndex.DifferRange6D] =
                                    (Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) - priceDay5) / priceDay5;
                                break;
                            case ReqFieldIndex.SumVol2:
                                fieldKeyValue[FieldIndex.Turnover3D] =
                                    (Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]) +
                                     Convert.ToInt32(fieldKeyValue[FieldIndex.SumVol2])) * 100.0f /
                                    Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]);
                                break;
                            case ReqFieldIndex.SumVol5:
                                fieldKeyValue[FieldIndex.Turnover6D] =
                                    (Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.SumVol5])) * 100.0f /
                                    Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]);
                                break;
                            case ReqFieldIndex.PreClose:
                                float diff1 = 0;
                                float diffRange1 = 0;

                                if (Convert.ToInt32(fieldKeyValue[FieldIndex.Now]) > 0)
                                {
                                    diff1 = Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) -
                                            Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                    if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]).CompareTo(0.0f) != 0)
                                        diffRange1 = diff1 / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                }
                                fieldKeyValue[FieldIndex.Difference] = diff1;
                                fieldKeyValue[FieldIndex.DifferRange] = diffRange1;
                                float delta1 = 0;
                                if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]) != 0)
                                    delta1 = (Convert.ToSingle(fieldKeyValue[FieldIndex.High]) -
                                              Convert.ToSingle(fieldKeyValue[FieldIndex.Low])) / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                fieldKeyValue[FieldIndex.Delta] = delta1;

                                break;
                            case ReqFieldIndex.High:
                            case ReqFieldIndex.Low:
                                float delta = 0;
                                if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]) != 0)
                                    delta = (Convert.ToSingle(fieldKeyValue[FieldIndex.High]) -
                                              Convert.ToSingle(fieldKeyValue[FieldIndex.Low])) / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                fieldKeyValue[FieldIndex.Delta] = delta;
                                break;
                            case ReqFieldIndex.Amount:
                                double amount = Convert.ToDouble(fieldKeyValue[FieldIndex.Amount]);
                                long volume = Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]);
                                if (volume != 0)
                                    fieldKeyValue[FieldIndex.AveragePrice] = Convert.ToSingle(amount / volume);
                                break;
                            case ReqFieldIndex.Volume:
                                if (Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]) != 0)
                                {
                                    fieldKeyValue[FieldIndex.Turnover] = Convert.ToInt32(fieldKeyValue[FieldIndex.Volume]) * 1.0f /
                                                   Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]);

                                    fieldKeyValue[FieldIndex.Turnover3D] =
                                        (Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]) +
                                        Convert.ToInt32(fieldKeyValue[FieldIndex.SumVol2])) * 1.0f /
                                        Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]);
                                    fieldKeyValue[FieldIndex.Turnover6D] =
                                        (Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]) +
                                         Convert.ToInt32(fieldKeyValue[FieldIndex.SumVol5])) * 1.0f /
                                        Convert.ToDouble(fieldKeyValue[FieldIndex.NetAShare]);
                                }

                                double amount1 = Convert.ToDouble(fieldKeyValue[FieldIndex.Amount]);
                                long volume1 = Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]);
                                if (volume1 != 0)
                                    fieldKeyValue[FieldIndex.AveragePrice] = Convert.ToSingle(amount1 / volume1);
                                break;
                            case ReqFieldIndex.SumBuyVol:
                            case ReqFieldIndex.SumSellVol:
                                fieldKeyValue[FieldIndex.Weicha] =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.SumBuyVolume]) -
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.SumSellVolume]);
                                int sum = Convert.ToInt32(fieldKeyValue[FieldIndex.SumBuyVolume]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.SumSellVolume]);
                                if (sum != 0)
                                    fieldKeyValue[FieldIndex.Weibi] = Convert.ToInt32(fieldKeyValue[FieldIndex.Weicha]) /
                                                                      sum;
                                break;
                            case ReqFieldIndex.NeiPan:
                                fieldKeyValue[FieldIndex.RedVolume] =
                                    Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]) -
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.GreenVolume]);
                                if (Convert.ToInt32(fieldKeyValue[FieldIndex.RedVolume]) != 0)
                                    fieldKeyValue[FieldIndex.NeiWaiBi] =
                                        Convert.ToInt32(fieldKeyValue[FieldIndex.GreenVolume]) * 1.0f / Convert.ToInt32(fieldKeyValue[FieldIndex.RedVolume]);
                                break;
                            case ReqFieldIndex.SListFlowDetailItemBig:
                                ListFlowDetailItem detailItemBig = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuyBig] = detailItemBig.Buy;
                                fieldKeyValue[FieldIndex.SellBig] = detailItemBig.Sell;
                                fieldKeyValue[FieldIndex.NetFlowBig] = detailItemBig.Buy - detailItemBig.Sell;
                                int totalAmount =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount);
                                fieldKeyValue[FieldIndex.MainNetFlow] = Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) +
                                                                        Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount;

                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle( Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount;
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount);
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount);
                                break;
                            case ReqFieldIndex.SListFlowDetailItemMiddle:
                                ListFlowDetailItem detailItemMiddle = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuyMiddle] = detailItemMiddle.Buy;
                                fieldKeyValue[FieldIndex.SellMiddle] = detailItemMiddle.Sell;
                                fieldKeyValue[FieldIndex.NetFlowBig] = detailItemMiddle.Buy - detailItemMiddle.Sell;
                                int totalAmount2 =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount2);

                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount2);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount2);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount2);
                                break;
                            case ReqFieldIndex.SListFlowDetailItemSmall:
                                ListFlowDetailItem detailItemSmall = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuySmall] = detailItemSmall.Buy;
                                fieldKeyValue[FieldIndex.SellSmall] = detailItemSmall.Sell;
                                fieldKeyValue[FieldIndex.NetFlowSmall] = detailItemSmall.Buy - detailItemSmall.Sell;
                                int totalAmount3 =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount3);
                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount3);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount3);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount3);
                                break;
                            case ReqFieldIndex.SListFlowDetailItemSuper:
                                ListFlowDetailItem detailItemSuper = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuySuper] = detailItemSuper.Buy;
                                fieldKeyValue[FieldIndex.SellSuper] = detailItemSuper.Sell;
                                fieldKeyValue[FieldIndex.NetFlowSuper] = detailItemSuper.Buy - detailItemSuper.Sell;
                                int totalAmount1 =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount1);
                                fieldKeyValue[FieldIndex.MainNetFlow] = Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) +
                                                                        Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount1;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount1;

                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount1);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount1;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount1;
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount1);
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount1);
                                break;
                            case ReqFieldIndex.SListFlowItemDay:
                                ListFlowItem item = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRange] = item.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHis] = item.HisPercentDecRange;
                                _isSort = true;
                                break;
                            case ReqFieldIndex.SListFlowItemDay3:
                                ListFlowItem itemD3 = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRangeDay3] = itemD3.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHisDay3] = itemD3.HisPercentDecRange;
                                fieldKeyValue[FieldIndex.DifferRange3D] = itemD3.DiffRanger * 0.0001f;
                                _isSort = true;
                                break;
                            case ReqFieldIndex.SListFlowItemDay5:
                                ListFlowItem itemD5 = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRangeDay5] = itemD5.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHisDay5] = itemD5.HisPercentDecRange;
                                fieldKeyValue[FieldIndex.DifferRange5D] = itemD5.DiffRanger * 0.0001f;
                                _isSort = true;
                                break;
                            case ReqFieldIndex.SListFlowItemDay10:
                                ListFlowItem itemD10 = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRangeDay10] = itemD10.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHisDay10] = itemD10.HisPercentDecRange;
                                fieldKeyValue[FieldIndex.DifferRange10D] = itemD10.DiffRanger * 0.0001f;
                                _isSort = true;
                                break;
                        }

                    }

                }
            }
        }

        private void SetReportDataPacket(ResBlockIndexReportDataPacket dataPacket)
        {
            foreach (KeyValuePair<int, Dictionary<byte, object>> entry in dataPacket.DicFieldValue)
            {
                Dictionary<FieldIndex, object> fieldKeyValue;
                if (AllDetailDataRec.TryGetValue(entry.Key, out fieldKeyValue))
                {
                    //写到客户端的detail
                    foreach (KeyValuePair<byte, object> reqFieldValue in entry.Value)
                    {

                        DicFieldIndexItem fieldIndexItem = ConvertBlockFieldIndex.DicFieldIndex[(ReqBlockFieldIndex)reqFieldValue.Key];
                        if (fieldIndexItem.FieldClient != FieldIndex.Na)
                        {
                            switch (fieldIndexItem.FieldFactor)
                            {
                                case FieldFactor.PriceFactor:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = Convert.ToInt32(reqFieldValue.Value) / 1000.0f;
                                    break;
                                case FieldFactor.RangeFactor:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = (Convert.ToInt32(reqFieldValue.Value)) / 100.0f;
                                    break;
                                case FieldFactor.BlockDifferRange:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = (Convert.ToInt32(reqFieldValue.Value)) / 10000.0f;
                                    break;
                                default:
                                    fieldKeyValue[fieldIndexItem.FieldClient] = reqFieldValue.Value;
                                    break;
                            }
                        }

                        //引起其他字段变化的字段
                        switch ((ReqBlockFieldIndex)reqFieldValue.Key)
                        {
                            case ReqBlockFieldIndex.Now:
                                float diff = 0;
                                float diffRange = 0;

                                if (Convert.ToInt32(fieldKeyValue[FieldIndex.Now]) > 0)
                                {
                                    diff = Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) -
                                            Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                    if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]).CompareTo(0.0f) != 0)
                                        diffRange = diff / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                }
                                fieldKeyValue[FieldIndex.Difference] = diff;
                                fieldKeyValue[FieldIndex.DifferRange] = diffRange;

                                break;
                            case ReqBlockFieldIndex.PreClose:
                                float diff1 = 0;
                                float diffRange1 = 0;

                                if (Convert.ToInt32(fieldKeyValue[FieldIndex.Now]) > 0)
                                {
                                    diff1 = Convert.ToSingle(fieldKeyValue[FieldIndex.Now]) -
                                            Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                    if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]).CompareTo(0.0f) != 0)
                                        diffRange1 = diff1 / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                }
                                fieldKeyValue[FieldIndex.Difference] = diff1;
                                fieldKeyValue[FieldIndex.DifferRange] = diffRange1;

                                float delta1 = 0;
                                if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]) != 0)
                                    delta1 = (Convert.ToSingle(fieldKeyValue[FieldIndex.High]) -
                                              Convert.ToSingle(fieldKeyValue[FieldIndex.Low])) / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                fieldKeyValue[FieldIndex.Delta] = delta1;
                                break;
                            case ReqBlockFieldIndex.High:
                            case ReqBlockFieldIndex.Low:
                                float delta = 0;
                                if (Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]) != 0)
                                    delta = (Convert.ToSingle(fieldKeyValue[FieldIndex.High]) -
                                              Convert.ToSingle(fieldKeyValue[FieldIndex.Low])) / Convert.ToSingle(fieldKeyValue[FieldIndex.PreClose]);
                                fieldKeyValue[FieldIndex.Delta] = delta;
                                break;
                            case ReqBlockFieldIndex.Volume:
                                float turnover = 0;
                                if (Convert.ToInt32(fieldKeyValue[FieldIndex.Ltg]) != 0)
                                {
                                    turnover = Convert.ToInt64(fieldKeyValue[FieldIndex.Volume]) /
                                                   Convert.ToInt32(fieldKeyValue[FieldIndex.Ltg]);
                                }
                                fieldKeyValue[FieldIndex.Turnover] = turnover;
                                break;
                            case ReqBlockFieldIndex.SListFlowDetailItemBig:
                                ListFlowDetailItem detailItemBig = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuyBig] = detailItemBig.Buy;
                                fieldKeyValue[FieldIndex.SellBig] = detailItemBig.Sell;
                                fieldKeyValue[FieldIndex.NetFlowBig] = detailItemBig.Buy - detailItemBig.Sell;
                                int totalAmount =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount);
                                fieldKeyValue[FieldIndex.MainNetFlow] = Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) +
                                                                        Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount;

                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount;
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount);
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount);
                                break;
                            case ReqBlockFieldIndex.SListFlowDetailItemMiddle:
                                ListFlowDetailItem detailItemMiddle = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuyMiddle] = detailItemMiddle.Buy;
                                fieldKeyValue[FieldIndex.SellMiddle] = detailItemMiddle.Sell;
                                fieldKeyValue[FieldIndex.NetFlowBig] = detailItemMiddle.Buy - detailItemMiddle.Sell;
                                int totalAmount2 =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount2);

                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount2);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount2);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount2;
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount2);
                                break;
                            case ReqBlockFieldIndex.SListFlowDetailItemSmall:
                                ListFlowDetailItem detailItemSmall = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuySmall] = detailItemSmall.Buy;
                                fieldKeyValue[FieldIndex.SellSmall] = detailItemSmall.Sell;
                                fieldKeyValue[FieldIndex.NetFlowSmall] = detailItemSmall.Buy - detailItemSmall.Sell;
                                int totalAmount3 =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount3);
                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount3);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount3);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount3;
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount3);
                                break;
                            case ReqBlockFieldIndex.SListFlowDetailItemSuper:
                                ListFlowDetailItem detailItemSuper = (ListFlowDetailItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.BuySuper] = detailItemSuper.Buy;
                                fieldKeyValue[FieldIndex.SellSuper] = detailItemSuper.Sell;
                                fieldKeyValue[FieldIndex.NetFlowSuper] = detailItemSuper.Buy - detailItemSuper.Sell;
                                int totalAmount1 =
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuyMiddle]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySmall]) +
                                    Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]);
                                fieldKeyValue[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]) *
                                                                            1.0f / totalAmount1);
                                fieldKeyValue[FieldIndex.MainNetFlow] = Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) +
                                                                        Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSuper]);
                                fieldKeyValue[FieldIndex.BuyFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuySuper]) * 1.0f / totalAmount1;
                                fieldKeyValue[FieldIndex.SellFlowRangeSuper] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellSuper]) * 1.0f / totalAmount1;

                                fieldKeyValue[FieldIndex.NetFlowRangeBig] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowBig]) *
                                                                            1.0f / totalAmount1);
                                fieldKeyValue[FieldIndex.BuyFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.BuyBig]) * 1.0f / totalAmount1;
                                fieldKeyValue[FieldIndex.SellFlowRangeBig] = Convert.ToInt32(fieldKeyValue[FieldIndex.SellBig]) * 1.0f / totalAmount1;
                                fieldKeyValue[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowMiddle]) *
                                                                            1.0f / totalAmount1);
                                fieldKeyValue[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle(Convert.ToInt32(fieldKeyValue[FieldIndex.NetFlowSmall]) *
                                                                            1.0f / totalAmount1);
                                break;
                            case ReqBlockFieldIndex.SListFlowItemDay:
                                ListFlowItem item = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRange] = item.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHis] = item.HisPercentDecRange;
                                _isSort = true;
                                break;
                            case ReqBlockFieldIndex.SListFlowItemDay3:
                                ListFlowItem itemD3 = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRangeDay3] = itemD3.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHisDay3] = itemD3.HisPercentDecRange;
                                fieldKeyValue[FieldIndex.DifferRange3D] = itemD3.DiffRanger * 0.01f;
                                _isSort = true;
                                break;
                            case ReqBlockFieldIndex.SListFlowItemDay5:
                                ListFlowItem itemD5 = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRangeDay5] = itemD5.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHisDay5] = itemD5.HisPercentDecRange;
                                fieldKeyValue[FieldIndex.DifferRange5D] = itemD5.DiffRanger * 0.01f;
                                _isSort = true;
                                break;
                            case ReqBlockFieldIndex.SListFlowItemDay10:
                                ListFlowItem itemD10 = (ListFlowItem)reqFieldValue.Value;
                                fieldKeyValue[FieldIndex.ZengCangRangeDay10] = itemD10.PercentDec * 0.01f;
                                fieldKeyValue[FieldIndex.ZengCangRankHisDay10] = itemD10.HisPercentDecRange;
                                fieldKeyValue[FieldIndex.DifferRange10D] = itemD10.DiffRanger * 0.01f;
                                _isSort = true;
                                break;
                        }

                    }

                }
            }

        }
         *         ***/

        #endregion

        private void SetNewsDataPacket(ResNewsReportDataPacket dataPacket)
        {
            if (dataPacket.InfoMineData == null)
                return;
            Dictionary<InfoMine, List<OneInfoMineDataRec>> memData;
            if (InfoMineData.TryGetValue(dataPacket.InfoMineData.Code, out memData))
            {
                foreach (KeyValuePair<InfoMine, List<OneInfoMineDataRec>> entry
                    in dataPacket.InfoMineData.InfoMineData)
                {
                    if (memData.ContainsKey(entry.Key))
                    {
                        List<OneInfoMineDataRec> memDataList = memData[entry.Key];
                        foreach (OneInfoMineDataRec packetData in entry.Value)
                        {
                            for (int i = 0; i < memDataList.Count; i++)
                            {
                                if (packetData.TextId > memDataList[i].TextId)
                                {
                                    memDataList.Insert(i, packetData);
                                    break;
                                }
                                if (packetData.TextId == memDataList[i].TextId)
                                    break;
                                if (i == memDataList.Count - 1)
                                {
                                    memDataList.Add(packetData);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        memData.Add(entry.Key, entry.Value);
                    }
                }
            }
            else
            {
                try
                {
                    InfoMineData.Add(dataPacket.InfoMineData.Code, dataPacket.InfoMineData.InfoMineData);
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage(e.Message);
                }

            }

            //找出当天的信息地雷
            if (!InfoMineData.TryGetValue(dataPacket.InfoMineData.Code, out memData))
                return;
            Dictionary<InfoMine, List<OneInfoMineDataRec>> currentInfoMine =
                new Dictionary<InfoMine, List<OneInfoMineDataRec>>();
            foreach (KeyValuePair<InfoMine, List<OneInfoMineDataRec>> entry in memData)
            {
                List<OneInfoMineDataRec> currentInfoMineList = new List<OneInfoMineDataRec>();

                for (int i = 0; i < entry.Value.Count; i++)
                {
                    if (entry.Value[i].PublishDate >= Dc.GetTradeDate(dataPacket.InfoMineData.Code))
                    {
                        currentInfoMineList.Add(entry.Value[i]);
                    }
                }

                if (currentInfoMineList.Count > 0)
                {
                    currentInfoMine.Add(entry.Key, currentInfoMineList);
                }
            }
            if (currentInfoMine.Count > 0)
            {
                try
                {
                    Dictionary<FieldIndex, object> fieldObject;
                    if (!DetailData.FieldIndexDataObject.TryGetValue(dataPacket.InfoMineData.Code, out fieldObject))
                    {
                        fieldObject = new Dictionary<FieldIndex, object>(1);
                        DetailData.FieldIndexDataObject[dataPacket.InfoMineData.Code] = fieldObject;
                    }
                    fieldObject[FieldIndex.InfoMine] = currentInfoMine;
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("dc infomine error" + e.Message);
                }
            }

        }

        private void SetNewsDataPacket(ResCustomStockNewsDataPacket dataPacket)
        {
            if (dataPacket.InfoMineData == null)
                return;
            Dictionary<InfoMine, List<OneInfoMineDataRec>> memData;
            foreach (InfoMineDataRec oneStockData in dataPacket.InfoMineData)
            {
                if (InfoMineData.TryGetValue(oneStockData.Code, out memData))
                {
                    foreach (KeyValuePair<InfoMine, List<OneInfoMineDataRec>> entry
                        in oneStockData.InfoMineData)
                    {
                        if (memData.ContainsKey(entry.Key))
                        {
                            List<OneInfoMineDataRec> memDataList = memData[entry.Key];
                            foreach (OneInfoMineDataRec packetData in entry.Value)
                            {
                                for (int i = 0; i < memDataList.Count; i++)
                                {
                                    if (packetData.TextId > memDataList[i].TextId)
                                    {
                                        memDataList.Insert(i, packetData);
                                        break;
                                    }
                                    if (packetData.TextId == memDataList[i].TextId)
                                        break;
                                    if (i == memDataList.Count - 1)
                                    {
                                        memDataList.Add(packetData);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            memData.Add(entry.Key, entry.Value);
                        }
                    }
                }
                else
                {
                    try
                    {
                        InfoMineData.Add(oneStockData.Code, oneStockData.InfoMineData);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage(e.Message);
                    }
                }
            }

        }

        private void SetOrgInfoDataPacket(ResInfoOrgDataPacket dataPacket)
        {
            if (dataPacket.InfoMineDatas == null)
                return;
            Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> memData;
            Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> memTopData;
            foreach (KeyValuePair<int, InfoMineOrgDataRec> OneInfoMin in dataPacket.InfoMineDatas)
            {
                if (InfoMineOrgData.TryGetValue(OneInfoMin.Key, out memData))
                {
                    List<OneInfoMineOrgDataRec> tipData = new List<OneInfoMineOrgDataRec>(1);
                    foreach (KeyValuePair<InfoMineOrg, List<OneInfoMineOrgDataRec>> entry
                        in OneInfoMin.Value.InfoMineData)
                    {
                        if (memData.ContainsKey(entry.Key))
                        {
                            List<OneInfoMineOrgDataRec> memDataList = memData[entry.Key];
                            int insertIndex = -1;
                            long memDate = 0;
                            long packetDate = 0;
                            bool isAddEnd = false;
                            bool isSame = false;
                            foreach (OneInfoMineOrgDataRec packetData in entry.Value)
                            {
                                isSame = false;
                                isAddEnd = false;
                                insertIndex = -1;

                                for (int i = 0; i < memDataList.Count; i++)
                                {
                                    if (memDataList[i].InfoCode == packetData.InfoCode)
                                    {
                                        isSame = true;
                                        break;
                                    }
                                }
                                if (isSame)
                                    continue;
                                packetDate = ((long)packetData.PublishDate) * 100000 + packetData.PublishTime;
                                for (int i = 0; i < memDataList.Count; i++)
                                {
                                    memDate = ((long)memDataList[i].PublishDate) * 100000 + memDataList[i].PublishTime;

                                    if (packetDate >= memDate)
                                    {
                                        insertIndex = i;
                                        break;
                                    }
                                    if (i == memDataList.Count - 1)
                                        isAddEnd = true;
                                }

                                if (insertIndex >= 0)
                                    memDataList.Insert(insertIndex, packetData);
                                else if (isAddEnd)
                                    memDataList.Add(packetData);
                            }

                        }
                        else
                        {
                            memData.Add(entry.Key, entry.Value);
                        }
                    }
                }
                else
                {
                    try
                    {
                        InfoMineOrgData.Add(OneInfoMin.Key, OneInfoMin.Value.InfoMineData);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage(e.Message);
                    }

                }

                //找出最近一个交易日到今天的信息地雷
                if (!InfoMineOrgData.TryGetValue(OneInfoMin.Key, out memData))
                    return;
                Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> currentInfoMine =
                    new Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>();
                foreach (KeyValuePair<InfoMineOrg, List<OneInfoMineOrgDataRec>> entry in memData)
                {
                    List<OneInfoMineOrgDataRec> currentInfoMineList = new List<OneInfoMineOrgDataRec>();

                    for (int i = 0; i < entry.Value.Count; i++)
                    {
                        if (entry.Value[i].PublishDate >= Dc.GetTradeDate(entry.Value[i].Code))
                        {
                            currentInfoMineList.Add(entry.Value[i]);
                        }
                    }

                    if (currentInfoMineList.Count > 0)
                    {
                        currentInfoMine.Add(entry.Key, currentInfoMineList);
                    }
                }
                if (currentInfoMine.Count > 0)
                {
                    try
                    {
                        Dictionary<FieldIndex, object> fieldObject;
                        if (!DetailData.FieldIndexDataObject.TryGetValue(OneInfoMin.Key, out fieldObject))
                        {
                            fieldObject = new Dictionary<FieldIndex, object>(1);
                            DetailData.FieldIndexDataObject[OneInfoMin.Key] = fieldObject;
                        }
                        fieldObject[FieldIndex.InfoMine] = currentInfoMine;
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("dc infomine error" + e.Message);

                    }
                }
            }

        }
        private void SetOrgInfoDataPacket(ResNewInfoOrgDataPacket dataPacket)
        {
            if (dataPacket.InfoMineDatas == null)
                return;
            Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> memData;
            Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> memTopData;
            foreach (KeyValuePair<int, InfoMineOrgDataRec> OneInfoMin in dataPacket.InfoMineDatas)
            {
                if (InfoMineOrgData.TryGetValue(OneInfoMin.Key, out memData))
                {
                    List<OneInfoMineOrgDataRec> tipData = new List<OneInfoMineOrgDataRec>(1);
                    foreach (KeyValuePair<InfoMineOrg, List<OneInfoMineOrgDataRec>> entry
                        in OneInfoMin.Value.InfoMineData)
                    {
                        if (memData.ContainsKey(entry.Key))
                        {
                            List<OneInfoMineOrgDataRec> memDataList = memData[entry.Key];
                            int insertIndex = -1;
                            long memDate = 0;
                            long packetDate = 0;
                            bool isAddEnd = false;
                            bool isSame = false;
                            foreach (OneInfoMineOrgDataRec packetData in entry.Value)
                            {
                                isSame = false;
                                isAddEnd = false;
                                insertIndex = -1;

                                for (int i = 0; i < memDataList.Count; i++)
                                {
                                    if (memDataList[i].InfoCode == packetData.InfoCode)
                                    {
                                        isSame = true;
                                        break;
                                    }
                                }
                                if (isSame)
                                    continue;
                                packetDate = ((long)packetData.PublishDate) * 100000 + packetData.PublishTime;
                                for (int i = 0; i < memDataList.Count; i++)
                                {
                                    memDate = ((long)memDataList[i].PublishDate) * 100000 + memDataList[i].PublishTime;

                                    if (packetDate >= memDate)
                                    {
                                        insertIndex = i;
                                        break;
                                    }
                                    if (i == memDataList.Count - 1)
                                        isAddEnd = true;
                                }

                                if (insertIndex >= 0)
                                    memDataList.Insert(insertIndex, packetData);
                                else if (isAddEnd)
                                    memDataList.Add(packetData);
                            }

                        }
                        else
                        {
                            memData.Add(entry.Key, entry.Value);
                        }
                    }
                }
                else
                {
                    try
                    {
                        InfoMineOrgData.Add(OneInfoMin.Key, OneInfoMin.Value.InfoMineData);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage(e.Message);
                    }

                }

                //找出最近一个交易日到今天的信息地雷
                if (!InfoMineOrgData.TryGetValue(OneInfoMin.Key, out memData))
                    return;
                Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> currentInfoMine =
                    new Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>();
                foreach (KeyValuePair<InfoMineOrg, List<OneInfoMineOrgDataRec>> entry in memData)
                {
                    List<OneInfoMineOrgDataRec> currentInfoMineList = new List<OneInfoMineOrgDataRec>();

                    for (int i = 0; i < entry.Value.Count; i++)
                    {
                        if (entry.Value[i].PublishDate >= Dc.GetTradeDate(entry.Value[i].Code))
                        {
                            currentInfoMineList.Add(entry.Value[i]);
                        }
                    }

                    if (currentInfoMineList.Count > 0)
                    {
                        currentInfoMine.Add(entry.Key, currentInfoMineList);
                    }
                }
                if (currentInfoMine.Count > 0)
                {
                    try
                    {
                        Dictionary<FieldIndex, object> fieldObject;
                        if (!DetailData.FieldIndexDataObject.TryGetValue(OneInfoMin.Key, out fieldObject))
                        {
                            fieldObject = new Dictionary<FieldIndex, object>(1);
                            DetailData.FieldIndexDataObject[OneInfoMin.Key] = fieldObject;
                        }
                        fieldObject[FieldIndex.InfoMine] = currentInfoMine;
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("dc infomine error" + e.Message);

                    }
                }
            }

        }
        private void SetOrgInfoDataPacket(ResInfoOrgByIdsDataPacket dataPacket)
        {
            if (dataPacket.TypeLevel1 == InfoMineOrg.News && dataPacket.DicNewsInfoByBlock.Count == 0)
                return;

            if (dataPacket.TypeLevel1 == InfoMineOrg.Report)
            {
                if (dataPacket.ReturnType == ReportType.NewsReport && dataPacket.DicNewsReportInfoByBlock.Count == 0)
                    return;
                else if (dataPacket.ReturnType == ReportType.EmratingReport && dataPacket.DicEmratingReportInfoByBlock.Count == 0)
                    return;
            }

            switch (dataPacket.TypeLevel1)
            {
                case InfoMineOrg.News:
                    FillNewsCacheData(dataPacket.DicNewsInfoByBlock);
                    break;

                case InfoMineOrg.Report:
                    switch (dataPacket.ReturnType)
                    {
                        case ReportType.NewsReport:
                            FillNewsReportCacheData(dataPacket.DicNewsReportInfoByBlock);
                            break;

                        case ReportType.EmratingReport:
                            FillEmratingReportCacheData(dataPacket.DicEmratingReportInfoByBlock);
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        private void FillEmratingReportCacheData(Dictionary<string, List<ResearchReportItem>> dic)
        {
            List<ResearchReportItem> recs;
            foreach (KeyValuePair<string, List<ResearchReportItem>> pair in dic)
            {
                if (!this.DicEmratingReportInfoByBlock.TryGetValue(pair.Key, out recs))
                {
                    recs = new List<ResearchReportItem>();
                    DicEmratingReportInfoByBlock[pair.Key] = recs;
                }
                recs = pair.Value;
            }
        }

        /// <summary>
        /// 返回包数据插入到缓存里面
        /// </summary>
        /// <param name="packetDic">返回包数据</param>
        private void FillNewsReportCacheData(Dictionary<string, List<OneInfoMineOrgDataRec>> packetDic)
        {
            List<OneInfoMineOrgDataRec> cacheRecs;
            foreach (KeyValuePair<string, List<OneInfoMineOrgDataRec>> packetItem in packetDic)
            {
                #region 没有该key对应的缓存
                if (!DicNewsReportInfoByBlock.TryGetValue(packetItem.Key, out cacheRecs))
                {
                    cacheRecs = packetItem.Value;
                    DicNewsReportInfoByBlock.Add(packetItem.Key, cacheRecs);
                    continue;
                }
                #endregion

                #region 有缓存，但是缓存为空或者缓存内无数据
                if (cacheRecs == null || cacheRecs.Count == 0)
                {
                    cacheRecs = packetItem.Value;
                    DicNewsReportInfoByBlock[packetItem.Key] = cacheRecs;
                    continue;
                }
                #endregion

                #region 缓存有数据需要插入排序（缓存是时间倒序排列：插入逻辑，按照时间排序，如果时间一样，后来的数据前置）



                int insertIndex = -1;
                long cacheRecTimeKey = 0;
                long packetRecTimeKey = 0;
                bool isAddEnd = false;
                bool isSame = false;
                foreach (OneInfoMineOrgDataRec packetRec in packetItem.Value)
                {
                    isSame = false;
                    isAddEnd = false;
                    insertIndex = -1;

                    for (int i = 0; i < cacheRecs.Count; i++)
                    {
                        if (cacheRecs[i].InfoCode == packetRec.InfoCode)
                        {
                            isSame = true;
                            break;
                        }
                    }

                    if (isSame)
                        continue;

                    packetRecTimeKey = ((long)packetRec.PublishDate) * 100000 + packetRec.PublishTime;
                    for (int i = 0; i < cacheRecs.Count; i++)
                    {
                        cacheRecTimeKey = ((long)cacheRecs[i].PublishDate) * 100000 + cacheRecs[i].PublishTime;

                        if (packetRecTimeKey >= cacheRecTimeKey)
                        {
                            insertIndex = i;
                            break;
                        }
                        if (i == cacheRecs.Count - 1)
                            isAddEnd = true;
                    }

                    if (insertIndex >= 0)
                        cacheRecs.Insert(insertIndex, packetRec);
                    else if (isAddEnd)
                        cacheRecs.Add(packetRec);
                }

                #endregion
            }
        }



        /// <summary>
        /// 返回包数据填充缓存
        /// </summary>
        /// <param name="packetDic">返回包数据</param>
        private void FillNewsCacheData(Dictionary<string, List<OneInfoMineOrgDataRec>> packetDic)
        {         
            List<OneInfoMineOrgDataRec> cacheRecs;
            foreach (KeyValuePair<string, List<OneInfoMineOrgDataRec>> packetItem in packetDic)
            {
                #region 没有该key对应的缓存
                if (!DicNewsInfoByBlock.TryGetValue(packetItem.Key, out cacheRecs))
                {
                    cacheRecs = packetItem.Value;
                    DicNewsInfoByBlock.Add(packetItem.Key, cacheRecs);
                    continue;
                } 
                #endregion

                #region 有缓存，但是缓存为空或者缓存内无数据
                if (cacheRecs == null || cacheRecs.Count == 0)
                {
                    cacheRecs = packetItem.Value;
                    DicNewsInfoByBlock[packetItem.Key] = cacheRecs;
                    continue;
                }
                #endregion

                #region 缓存有数据需要插入排序（缓存是时间倒序排列：插入逻辑，按照时间排序，如果时间一样，后来的数据前置）
               

                        
                int insertIndex = -1;
                long cacheRecTimeKey = 0;
                long packetRecTimeKey = 0;
                bool isAddEnd = false;
                bool isSame = false;
                foreach (OneInfoMineOrgDataRec packetRec in packetItem.Value)
                {
                    isSame = false;
                    isAddEnd = false;
                    insertIndex = -1;

                    for (int i = 0; i < cacheRecs.Count; i++)
                    {
                        if (cacheRecs[i].InfoCode == packetRec.InfoCode)
                        {
                            isSame = true;
                            break;
                        }
                    }

                    if (isSame)
                        continue;

                    packetRecTimeKey = ((long)packetRec.PublishDate) * 100000 + packetRec.PublishTime;
                    for (int i = 0; i < cacheRecs.Count; i++)
                    {
                        cacheRecTimeKey = ((long)cacheRecs[i].PublishDate) * 100000 + cacheRecs[i].PublishTime;

                        if (packetRecTimeKey >= cacheRecTimeKey)
                        {
                            insertIndex = i;
                            break;
                        }
                        if (i == cacheRecs.Count - 1)
                            isAddEnd = true;
                    }

                    if (insertIndex >= 0)
                        cacheRecs.Insert(insertIndex, packetRec);
                    else if (isAddEnd)
                        cacheRecs.Add(packetRec);
                }

                #endregion  
            }

        }

        #endregion

        #region Caculate

        private void CaculateIBBondData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            long volume = 0;
            float ai = 0,
                now = 0,
                preClose = 0,
                avgPrice = 0,
                preAvgPrice = 0,
                open = 0,
                high = 0,
                low = 0,
                nowYtm = 0,
                preCloseYtm = 0,
                nowYtmDce = 0,
                preCloseYtmDce = 0,
                preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            //full，net
            if (fieldSingle.ContainsKey(FieldIndex.Now))
            {
                fieldSingle[FieldIndex.BondNetNow] = fieldSingle[FieldIndex.Now];
                now = (fieldSingle[FieldIndex.Now]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Open))
            {
                fieldSingle[FieldIndex.BondNetOpen] = fieldSingle[FieldIndex.Open];
                open = (fieldSingle[FieldIndex.Open]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.High))
            {
                fieldSingle[FieldIndex.BondNetHigh] = fieldSingle[FieldIndex.High];
                high = (fieldSingle[FieldIndex.High]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Low))
            {
                fieldSingle[FieldIndex.BondNetLow] = fieldSingle[FieldIndex.Low];
                low = (fieldSingle[FieldIndex.Low]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
            {
                fieldSingle[FieldIndex.BondLcclose] = fieldSingle[FieldIndex.PreClose];
                preClose = (fieldSingle[FieldIndex.PreClose]);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
                ai = (fieldSingle[FieldIndex.BondAI]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.BondFullNow] = now + ai;
                fieldSingle[FieldIndex.BondFullOpen] = open + ai;
                fieldSingle[FieldIndex.BondFullLow] = low + ai;
                fieldSingle[FieldIndex.BondFullHigh] = high + ai;
                fieldSingle[FieldIndex.BondLfclose] = preClose + ai;

                //diff,differRange,delta
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                }

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;

                //avgPrice
                if (fieldSingle.ContainsKey(FieldIndex.AveragePrice))
                    avgPrice = (fieldSingle[FieldIndex.AveragePrice]);
                if (fieldSingle.ContainsKey(FieldIndex.BondDecLcavg))
                    preAvgPrice = (fieldSingle[FieldIndex.BondDecLcavg]);
                fieldSingle[FieldIndex.BondAvgDiffer] = avgPrice - preAvgPrice;
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                fieldDouble[FieldIndex.Amount] = Convert.ToDouble(volume * avgPrice);
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);

                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }
            //ytm
            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                nowYtm = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecNowYTM))
                nowYtmDce = (fieldSingle[FieldIndex.BondDecNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                preCloseYtm = (fieldSingle[FieldIndex.BondDecLcytm]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLavgytm))
                preCloseYtmDce = (fieldSingle[FieldIndex.BondDecLavgytm]);

            fieldSingle[FieldIndex.BondDiffRangeYTM] = (nowYtm - preCloseYtm) * 100;
            fieldSingle[FieldIndex.BondDecDiffRangeYTM] = (nowYtmDce - preCloseYtmDce) * 100;

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
            {
                if (float.IsInfinity(ai))
                    fieldSingle.Remove(FieldIndex.BondAI);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondNewrate))
            {
                float newrate = (fieldSingle[FieldIndex.BondNewrate]);
                if (float.IsInfinity(newrate))
                    fieldSingle.Remove(FieldIndex.BondNewrate);
            }
        }

        private void CaculateSHNonConvertBondData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float ai = 0,
                now = 0,
                preClose = 0,
                avgPrice = 0,
                preAvgPrice = 0,
                open = 0,
                high = 0,
                low = 0;

            long volume = 0;
            double amount = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);
            //full，net
            if (fieldSingle.ContainsKey(FieldIndex.Now))
            {
                fieldSingle[FieldIndex.BondNetNow] = fieldSingle[FieldIndex.Now];
                now = (fieldSingle[FieldIndex.Now]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Open))
            {
                fieldSingle[FieldIndex.BondNetOpen] = fieldSingle[FieldIndex.Open];
                open = (fieldSingle[FieldIndex.Open]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.High))
            {
                fieldSingle[FieldIndex.BondNetHigh] = fieldSingle[FieldIndex.High];
                high = (fieldSingle[FieldIndex.High]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Low))
            {
                fieldSingle[FieldIndex.BondNetLow] = fieldSingle[FieldIndex.Low];
                low = (fieldSingle[FieldIndex.Low]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
            {
                fieldSingle[FieldIndex.BondLcclose] = fieldSingle[FieldIndex.PreClose];
                preClose = (fieldSingle[FieldIndex.PreClose]);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
                ai = (fieldSingle[FieldIndex.BondAI]);

            if (now != 0)
            {
                fieldSingle[FieldIndex.BondFullNow] = now + ai;
                fieldSingle[FieldIndex.BondFullOpen] = open + ai;
                fieldSingle[FieldIndex.BondFullLow] = low + ai;
                fieldSingle[FieldIndex.BondFullHigh] = high + ai;
                fieldSingle[FieldIndex.BondLfclose] = preClose + ai;

                //diff,differRange,delta
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                }

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;

                //avgPrice
                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (volume != 0)
                    avgPrice = Convert.ToSingle(amount / volume);
                if (fieldSingle.ContainsKey(FieldIndex.BondDecLcavg))
                    preAvgPrice = (fieldSingle[FieldIndex.BondDecLcavg]);
                fieldSingle[FieldIndex.AveragePrice] = avgPrice;
                fieldSingle[FieldIndex.BondAvgDiffer] = avgPrice - preAvgPrice;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);

                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }

            float nowYtm = 0, nowYtmDce = 0, preCloseYtm = 0, preCloseYtmDce = 0;
            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                nowYtm = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecNowYTM))
                nowYtmDce = (fieldSingle[FieldIndex.BondDecNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                preCloseYtm = (fieldSingle[FieldIndex.BondDecLcytm]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLavgytm))
                preCloseYtmDce = (fieldSingle[FieldIndex.BondDecLavgytm]);

            fieldSingle[FieldIndex.BondDiffRangeYTM] = (nowYtm - preCloseYtm) * 100;
            fieldSingle[FieldIndex.BondDecDiffRangeYTM] = (nowYtmDce - preCloseYtmDce) * 100;

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
            {
                if (float.IsInfinity(ai))
                    fieldSingle.Remove(FieldIndex.BondAI);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondNewrate))
            {
                float newrate = (fieldSingle[FieldIndex.BondNewrate]);
                if (float.IsInfinity(newrate))
                    fieldSingle.Remove(FieldIndex.BondNewrate);
            }
        }

        private void CaculateSZNonConvertBondData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }
            float ai = 0,
                now = 0,
                preClose = 0,
                avgPrice = 0,
                preAvgPrice = 0,
                open = 0,
                high = 0,
                low = 0;

            long volume = 0;
            double amount = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            //full，net
            if (fieldSingle.ContainsKey(FieldIndex.Now))
            {
                fieldSingle[FieldIndex.BondNetNow] = fieldSingle[FieldIndex.Now];
                now = (fieldSingle[FieldIndex.Now]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Open))
            {
                fieldSingle[FieldIndex.BondNetOpen] = fieldSingle[FieldIndex.Open];
                open = (fieldSingle[FieldIndex.Open]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.High))
            {
                fieldSingle[FieldIndex.BondNetHigh] = fieldSingle[FieldIndex.High];
                high = (fieldSingle[FieldIndex.High]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Low))
            {
                fieldSingle[FieldIndex.BondNetLow] = fieldSingle[FieldIndex.Low];
                low = (fieldSingle[FieldIndex.Low]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
            {
                fieldSingle[FieldIndex.BondLcclose] = fieldSingle[FieldIndex.PreClose];
                preClose = (fieldSingle[FieldIndex.PreClose]);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
                ai = (fieldSingle[FieldIndex.BondAI]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.BondFullNow] = now + ai;
                fieldSingle[FieldIndex.BondFullOpen] = open + ai;
                fieldSingle[FieldIndex.BondFullLow] = low + ai;
                fieldSingle[FieldIndex.BondFullHigh] = high + ai;
                fieldSingle[FieldIndex.BondLfclose] = preClose + ai;

                //diff,differRange,delta
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                }

                //avgPrice
                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (volume != 0)
                    avgPrice = Convert.ToSingle(amount / volume);
                if (fieldSingle.ContainsKey(FieldIndex.BondDecLcavg))
                    preAvgPrice = (fieldSingle[FieldIndex.BondDecLcavg]);
                fieldSingle[FieldIndex.AveragePrice] = avgPrice;
                fieldSingle[FieldIndex.BondAvgDiffer] = avgPrice - preAvgPrice;

            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                now = preClose;
            }
            if (now != 0)
            {
                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
            }

            float nowYtm = 0, nowYtmDce = 0, preCloseYtm = 0, preCloseYtmDce = 0;
            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                nowYtm = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecNowYTM))
                nowYtmDce = (fieldSingle[FieldIndex.BondDecNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                preCloseYtm = (fieldSingle[FieldIndex.BondDecLcytm]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLavgytm))
                preCloseYtmDce = (fieldSingle[FieldIndex.BondDecLavgytm]);

            fieldSingle[FieldIndex.BondDiffRangeYTM] = (nowYtm - preCloseYtm) * 100;
            fieldSingle[FieldIndex.BondDecDiffRangeYTM] = (nowYtmDce - preCloseYtmDce) * 100;
            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
            {
                if (float.IsInfinity(ai))
                    fieldSingle.Remove(FieldIndex.BondAI);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondNewrate))
            {
                float newrate = (fieldSingle[FieldIndex.BondNewrate]);
                if (float.IsInfinity(newrate))
                    fieldSingle.Remove(FieldIndex.BondNewrate);
            }
        }

        private void CaculateSHConvertBondData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }
            float ai = 0,
                now = 0,
                preClose = 0,
                avgPrice = 0,
                preAvgPrice = 0,
                open = 0,
                high = 0,
                low = 0;

            long volume = 0;
            double amount = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            //full，net
            if (fieldSingle.ContainsKey(FieldIndex.Now))
            {
                fieldSingle[FieldIndex.BondFullNow] = fieldSingle[FieldIndex.Now];
                now = (fieldSingle[FieldIndex.Now]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Open))
            {
                fieldSingle[FieldIndex.BondFullOpen] = fieldSingle[FieldIndex.Open];
                open = (fieldSingle[FieldIndex.Open]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.High))
            {
                fieldSingle[FieldIndex.BondFullHigh] = fieldSingle[FieldIndex.High];
                high = (fieldSingle[FieldIndex.High]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Low))
            {
                fieldSingle[FieldIndex.BondFullLow] = fieldSingle[FieldIndex.Low];
                low = (fieldSingle[FieldIndex.Low]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
            {
                fieldSingle[FieldIndex.BondLfclose] = fieldSingle[FieldIndex.PreClose];
                preClose = (fieldSingle[FieldIndex.PreClose]);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
                ai = (fieldSingle[FieldIndex.BondAI]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.BondNetNow] = now - ai;
                fieldSingle[FieldIndex.BondNetOpen] = open - ai;
                fieldSingle[FieldIndex.BondNetLow] = low - ai;
                fieldSingle[FieldIndex.BondNetHigh] = high - ai;
                fieldSingle[FieldIndex.BondLcclose] = preClose - ai;

                //diff,differRange,delta
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                }

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;


                //avgPrice
                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (volume != 0)
                    avgPrice = Convert.ToSingle(amount / volume);
                if (fieldSingle.ContainsKey(FieldIndex.BondDecLcavg))
                    preAvgPrice = (fieldSingle[FieldIndex.BondDecLcavg]);
                fieldSingle[FieldIndex.AveragePrice] = avgPrice;
                fieldSingle[FieldIndex.BondAvgDiffer] = avgPrice - preAvgPrice;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);

                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }

            float ytmNow = 0, ytmPre = 0, ytmNowDec = 0, ytmPreDec = 0;
            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                ytmNow = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                ytmPre = (fieldSingle[FieldIndex.BondDecLcytm]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecNowYTM))
                ytmNowDec = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLavgytm))
                ytmPreDec = (fieldSingle[FieldIndex.BondDecLavgytm]);
            fieldSingle[FieldIndex.BondDiffRangeYTM] = (ytmNow - ytmPre) * 100;
            fieldSingle[FieldIndex.BondDecDiffRangeYTM] = (ytmNowDec - ytmPreDec) * 100;

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
            {
                if (float.IsInfinity(ai))
                    fieldSingle.Remove(FieldIndex.BondAI);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondNewrate))
            {
                float newrate = (fieldSingle[FieldIndex.BondNewrate]);
                if (float.IsInfinity(newrate))
                    fieldSingle.Remove(FieldIndex.BondNewrate);
            }
        }

        private void CaculateSZConvertBondData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }
            float ai = 0,
                now = 0,
                preClose = 0,
                avgPrice = 0,
                preAvgPrice = 0,
                open = 0,
                high = 0,
                low = 0;

            long volume = 0;
            double amount = 0;
            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;


            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);
            //full，net
            if (fieldSingle.ContainsKey(FieldIndex.Now))
            {
                fieldSingle[FieldIndex.BondFullNow] = fieldSingle[FieldIndex.Now];
                now = (fieldSingle[FieldIndex.Now]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Open))
            {
                fieldSingle[FieldIndex.BondFullOpen] = fieldSingle[FieldIndex.Open];
                open = (fieldSingle[FieldIndex.Open]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.High))
            {
                fieldSingle[FieldIndex.BondFullHigh] = fieldSingle[FieldIndex.High];
                high = (fieldSingle[FieldIndex.High]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.Low))
            {
                fieldSingle[FieldIndex.BondFullLow] = fieldSingle[FieldIndex.Low];
                low = (fieldSingle[FieldIndex.Low]);
            }
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
            {
                fieldSingle[FieldIndex.BondLfclose] = fieldSingle[FieldIndex.PreClose];
                preClose = (fieldSingle[FieldIndex.PreClose]);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
                ai = (fieldSingle[FieldIndex.BondAI]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.BondNetNow] = now - ai;
                fieldSingle[FieldIndex.BondNetOpen] = open - ai;
                fieldSingle[FieldIndex.BondNetLow] = low - ai;
                fieldSingle[FieldIndex.BondNetHigh] = high - ai;
                fieldSingle[FieldIndex.BondLcclose] = preClose - ai;

                //diff,differRange,delta
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                }

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;


                //avgPrice
                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (volume != 0)
                    avgPrice = Convert.ToSingle(amount / volume);
                if (fieldSingle.ContainsKey(FieldIndex.BondDecLcavg))
                    preAvgPrice = (fieldSingle[FieldIndex.BondDecLcavg]);
                fieldSingle[FieldIndex.AveragePrice] = avgPrice;
                fieldSingle[FieldIndex.BondAvgDiffer] = avgPrice - preAvgPrice;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }

            float ytmNow = 0, ytmPre = 0, ytmNowDec = 0, ytmPreDec = 0;
            if (fieldSingle.ContainsKey(FieldIndex.BondNowYTM))
                ytmNow = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLcytm))
                ytmPre = (fieldSingle[FieldIndex.BondDecLcytm]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecNowYTM))
                ytmNowDec = (fieldSingle[FieldIndex.BondNowYTM]);
            if (fieldSingle.ContainsKey(FieldIndex.BondDecLavgytm))
                ytmPreDec = (fieldSingle[FieldIndex.BondDecLavgytm]);
            fieldSingle[FieldIndex.BondDiffRangeYTM] = (ytmNow - ytmPre) * 100;
            fieldSingle[FieldIndex.BondDecDiffRangeYTM] = (ytmNowDec - ytmPreDec) * 100;

            if (fieldSingle.ContainsKey(FieldIndex.BondAI))
            {
                if (float.IsInfinity(ai))
                    fieldSingle.Remove(FieldIndex.BondAI);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BondNewrate))
            {
                float newrate = (fieldSingle[FieldIndex.BondNewrate]);
                if (float.IsInfinity(newrate))
                    fieldSingle.Remove(FieldIndex.BondNewrate);
            }
        }

        private void CaculateStockData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float diff = 0,
                diffRange = 0,
                diffRangeDay5 = 0,
                diffRangeDay10 = 0,
                diffRangeDay20 = 0,
                diffRangeDay60 = 0,
                diffRangeDay120 = 0,
                diffRangeDay250 = 0,
                diffRangeYtd = 0,
                now = 0,
                pb = 0,
                pe = 0,
                pettm = 0,
                pelyr = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0,
                preClose = 0,
                liangbi = 0,
                volumeAvgDay5 = 0,
                netInflowRange = 0,
                netInflowRangeDay5 = 0,
                netInflowRangeDay20 = 0,
                netInflowRangeDay60 = 0,
                turnover = 0,
                delta = 0,
                high = 0,
                low = 0,
                high52w = 0,
                low52w = 0,
                avgPrice = 0;
            double mgsy = 0,
                mgsyTTM = 0,
                mgsyLYR = 0,
                mgjzc = 0,
                netInFlow = 0,
                netInFlowDay4 = 0,
                netInFlowDay19 = 0,
                netInFlowDay59 = 0,
                amount = 0,
                amountDay4 = 0,
                amountDay19 = 0,
                amountDay59 = 0,
                zsz = 0,
                ltsz = 0,
                netAShare = 0;
            long volume = 0,
                redVolume = 0,
                greenVolume = 0;
            int netInFlowRedDay4 = 0,
                netInFlowRedDay19 = 0,
                netInFlowRedDay59 = 0;


            //价格相关
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);

            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = fieldSingle[FieldIndex.PreClose];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = fieldSingle[FieldIndex.PreCloseDay5];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = fieldSingle[FieldIndex.PreCloseDay10];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = fieldSingle[FieldIndex.PreCloseDay20];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = fieldSingle[FieldIndex.PreCloseDay60];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = fieldSingle[FieldIndex.PreCloseDay120];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = fieldSingle[FieldIndex.PreCloseDay250];
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = fieldSingle[FieldIndex.PreCloseDayYTD];


            if (now != 0)
            {
                diff = now - preClose;
                if (preClose != 0)
                    diffRange = diff / preClose;
                if (preCloseDay5 != 0)
                    diffRangeDay5 = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    diffRangeDay10 = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    diffRangeDay20 = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    diffRangeDay60 = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    diffRangeDay120 = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    diffRangeDay250 = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    diffRangeYtd = (now - preCloseYtd) / preCloseYtd;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                if (preClose != 0)
                {
                    if (preCloseDay5 != 0)
                        diffRangeDay5 = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        diffRangeDay10 = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        diffRangeDay20 = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        diffRangeDay60 = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        diffRangeDay120 = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        diffRangeDay250 = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        diffRangeYtd = (preClose - preCloseYtd) / preCloseYtd;
                }
            }

            if (fieldDouble.ContainsKey(FieldIndex.EpsQmtby))
                mgsyLYR = (fieldDouble[FieldIndex.EpsQmtby]);
            if (mgsyLYR != 0)
                pelyr = Convert.ToSingle(now / mgsyLYR);

            if (fieldDouble.ContainsKey(FieldIndex.EpsTtm))
                mgsyTTM = (fieldDouble[FieldIndex.EpsTtm]);
            if (mgsyTTM != 0)
                pettm = Convert.ToSingle(now / mgsyTTM);

            if (fieldDouble.ContainsKey(FieldIndex.MGJZC))
                mgjzc = (fieldDouble[FieldIndex.MGJZC]);
            if (mgjzc != 0)
                pb = Convert.ToSingle(now / mgjzc);


            //成交量相关
            if (fieldInt64.ContainsKey(FieldIndex.Volume))
                volume = (fieldInt64[FieldIndex.Volume]);

            if (fieldInt64.ContainsKey(FieldIndex.VolumeAvgDay5))
                volumeAvgDay5 = fieldInt64[FieldIndex.VolumeAvgDay5];
            float liangbiParam = volumeAvgDay5 * (TimeUtilities.GetPointFromTime(unicode)) / 241;
            if (liangbiParam != 0)
                liangbi = volume / liangbiParam;

            fieldSingle[FieldIndex.VolumeRatio] = liangbi;

            if (fieldDouble.ContainsKey(FieldIndex.ZGB))
            {
                if (now != 0)
                    zsz = fieldDouble[FieldIndex.ZGB] * now;
                else
                    zsz = fieldDouble[FieldIndex.ZGB] * preClose;
            }

            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(unicode))
                DetailData.FieldIndexDataInt32[unicode].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;

            switch (mt)
            {
                case MarketType.SHBLev1:
                case MarketType.SZBLev1:
                case MarketType.SHBLev2:
                case MarketType.SZBLev2:
                    if (fieldDouble.ContainsKey(FieldIndex.NetBShare))
                        netAShare = (fieldDouble[FieldIndex.NetBShare]);
                    break;
                default:
                    if (fieldDouble.ContainsKey(FieldIndex.NetAShare))
                        netAShare = (fieldDouble[FieldIndex.NetAShare]);
                    break;
            }
            if (now != 0)
                ltsz = netAShare * now;
            else
                ltsz = netAShare * preClose;

            if(netAShare != 0)
                turnover = Convert.ToSingle(volume*1.0f/netAShare);


            fieldDouble[FieldIndex.ZSZ] = zsz;
            fieldDouble[FieldIndex.LTSZ] = ltsz;
            fieldSingle[FieldIndex.Turnover] = turnover;
            fieldSingle[FieldIndex.Difference] = diff;
            fieldSingle[FieldIndex.DifferRange] = diffRange;
            fieldSingle[FieldIndex.DifferRange5D] = diffRangeDay5;
            fieldSingle[FieldIndex.DifferRange10D] = diffRangeDay10;
            fieldSingle[FieldIndex.DifferRange20D] = diffRangeDay20;
            fieldSingle[FieldIndex.DifferRange60D] = diffRangeDay60;
            fieldSingle[FieldIndex.DifferRange120D] = diffRangeDay120;
            fieldSingle[FieldIndex.DifferRange250D] = diffRangeDay250;

            fieldSingle[FieldIndex.DifferRangeYTD] = diffRangeYtd;
            //fieldSingle[FieldIndex.PE] = pe;
            fieldSingle[FieldIndex.PELYR] = pelyr;
            fieldSingle[FieldIndex.PETTM] = pettm;
            fieldSingle[FieldIndex.PB] = pb;

            //成交金额相关
            if (fieldDouble.ContainsKey(FieldIndex.Amount))
                amount = (fieldDouble[FieldIndex.Amount]);

            if (fieldDouble.ContainsKey(FieldIndex.AmountDay4))
                amountDay4 = fieldDouble[FieldIndex.AmountDay4];
            if (fieldDouble.ContainsKey(FieldIndex.AmountDay19))
                amountDay19 = fieldDouble[FieldIndex.AmountDay19];
            if (fieldDouble.ContainsKey(FieldIndex.AmountDay59))
                amountDay59 = fieldDouble[FieldIndex.AmountDay59];
            if (fieldDouble.ContainsKey(FieldIndex.NetFlow))
                netInFlow = (fieldDouble[FieldIndex.NetFlow]);

            if (fieldDouble.ContainsKey(FieldIndex.NetFlowDay4))
                netInFlowDay4 = fieldDouble[FieldIndex.NetFlowDay4];
            if (fieldDouble.ContainsKey(FieldIndex.NetFlowDay19))
                netInFlowDay19 = fieldDouble[FieldIndex.NetFlowDay19];
            if (fieldDouble.ContainsKey(FieldIndex.NetFlowDay59))
                netInFlowDay59 = fieldDouble[FieldIndex.NetFlowDay59];
            if (fieldDouble.ContainsKey(FieldIndex.NetInFlowRedDay4))
                netInFlowRedDay4 = fieldInt32[FieldIndex.NetInFlowRedDay4];
            if (fieldDouble.ContainsKey(FieldIndex.NetInFlowRedDay19))
                netInFlowRedDay19 = fieldInt32[FieldIndex.NetInFlowRedDay19];
            if (fieldDouble.ContainsKey(FieldIndex.NetInFlowRedDay59))
                netInFlowRedDay59 = fieldInt32[FieldIndex.NetInFlowRedDay59];

            if (amount != 0)
                netInflowRange = Convert.ToSingle(netInFlow / amount);
            if ((amountDay4 + amount) != 0)
                netInflowRangeDay5 = Convert.ToSingle((netInFlowDay4 + netInFlow) / (amountDay4 + amount));
            if ((amountDay19 + amount) != 0)
                netInflowRangeDay20 = Convert.ToSingle((netInFlowDay19 + netInFlow) / (amountDay19 + amount));
            if ((amountDay59 + amount) != 0)
                netInflowRangeDay60 = Convert.ToSingle((netInFlowDay59 + netInFlow) / (amountDay59 + amount));

            if (volume != 0)
                avgPrice = Convert.ToSingle(amount / volume);

            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (preClose > 0)
                delta = (high - low) / preClose;
            if (fieldSingle.ContainsKey(FieldIndex.HighW52))
                high52w = (fieldSingle[FieldIndex.HighW52]);
            if (fieldSingle.ContainsKey(FieldIndex.LowW52))
                low52w = (fieldSingle[FieldIndex.LowW52]);
            high52w = Math.Max(high52w, high);
            if (low != 0)
                low52w = Math.Min(low52w, low);

            //if (memData.ContainsKey(FieldIndex.GreenVolume))
            //    greenVolume = Convert.ToInt32(memData[FieldIndex.GreenVolume]);
            //redVolume = volume - greenVolume;
            if (fieldInt64.ContainsKey(FieldIndex.RedVolume))
                redVolume = (fieldInt64[FieldIndex.RedVolume]);
            greenVolume = (volume - redVolume);

            fieldSingle[FieldIndex.HighW52] = high52w;
            fieldSingle[FieldIndex.LowW52] = low52w;
            fieldInt64[FieldIndex.RedVolume] = redVolume;
            fieldInt64[FieldIndex.GreenVolume] = greenVolume;
            fieldSingle[FieldIndex.NetFlowRange] = netInflowRange;
            fieldSingle[FieldIndex.NetFlowRangeDay5] = netInflowRangeDay5;
            fieldSingle[FieldIndex.NetFlowRangeDay20] = netInflowRangeDay20;
            fieldSingle[FieldIndex.NetFlowRangeDay60] = netInflowRangeDay60;
            fieldDouble[FieldIndex.NetFlowDay5] = netInFlowDay4 + netInFlow;
            fieldDouble[FieldIndex.NetFlowDay20] = netInFlowDay19 + netInFlow;

            if (netInFlow > 0)
            {
                fieldInt32[FieldIndex.NetFlowRedDay5] = netInFlowRedDay4 + 1;
                fieldInt32[FieldIndex.NetFlowRedDay60] = netInFlowRedDay59 + 1;
                fieldInt32[FieldIndex.NetFlowRedDay20] = netInFlowRedDay19 + 1;
            }
            else
            {
                fieldInt32[FieldIndex.NetFlowRedDay5] = netInFlowRedDay4;
                fieldInt32[FieldIndex.NetFlowRedDay60] = netInFlowRedDay59;
                fieldInt32[FieldIndex.NetFlowRedDay20] = netInFlowRedDay19;

            }
            fieldSingle[FieldIndex.Delta] = delta;
            fieldSingle[FieldIndex.AveragePrice] = avgPrice;

        }

        private void CaculateIndexStaticData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }

            float preClose = 0,
                now = 0,
                preClose5 = 0,
                preClose20 = 0,
                preClose60 = 0,
                preCloseYTD = 0,
                high52w = 0,
                low52w = 0,
                high = 0,
                low = 0;
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preClose5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preClose20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preClose60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYTD = (fieldSingle[FieldIndex.PreCloseDayYTD]);
            if (fieldSingle.ContainsKey(FieldIndex.HighW52))
                high52w = (fieldSingle[FieldIndex.HighW52]);
            if (fieldSingle.ContainsKey(FieldIndex.LowW52))
                low52w = (fieldSingle[FieldIndex.LowW52]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                now = preClose;
            }

            if (now != 0)
            {
                if (preClose5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preClose5) / preClose5;
                if (preClose20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preClose20) / preClose20;
                if (preClose60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preClose60) / preClose60;
                if (preCloseYTD != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYTD) / preCloseYTD;
            }

            fieldSingle[FieldIndex.HighW52] = Math.Max(high52w, high);

            if (low.Equals(0))
                low = preClose;
            if (low52w.Equals(0) && !low.Equals(0))
                fieldSingle[FieldIndex.LowW52] = low;
            else if (!low52w.Equals(0) && low.Equals(0))
                fieldSingle[FieldIndex.LowW52] = low52w;
            else
                fieldSingle[FieldIndex.LowW52] = Math.Min(low52w, low);
        }

        private void CaculateFuturesData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            long preOpenInterest = 0, openInterest = 0;
            float preSettlementPrice = 0, now = 0, high = 0, low = 0, high52w = 0, low52w = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            if (fieldSingle.ContainsKey(FieldIndex.PreSettlementPrice))
                preSettlementPrice = (fieldSingle[FieldIndex.PreSettlementPrice]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preSettlementPrice;
                if (preSettlementPrice != 0)
                {
                    fieldSingle[FieldIndex.Delta] = (high - low) / preSettlementPrice;
                    fieldSingle[FieldIndex.DifferRange] = (now - preSettlementPrice) / preSettlementPrice;
                }
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                now = preSettlementPrice;
            }

            if (now != 0)
            {
                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
            }
            if (fieldSingle.ContainsKey(FieldIndex.HighW52))
                high52w = (fieldSingle[FieldIndex.HighW52]);
            if (fieldSingle.ContainsKey(FieldIndex.LowW52))
                low52w = (fieldSingle[FieldIndex.LowW52]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            high52w = Math.Max(high52w, high);
            if (low != 0 && low52w != 0)
                low52w = Math.Min(low52w, low);
            else low52w = Math.Max(low52w, low);
            fieldSingle[FieldIndex.HighW52] = high52w;
            fieldSingle[FieldIndex.LowW52] = low52w;

            if (fieldInt64.ContainsKey(FieldIndex.PreOpenInterest))
                preOpenInterest = (fieldInt64[FieldIndex.PreOpenInterest]);
            if (fieldInt64.ContainsKey(FieldIndex.OpenInterest))
                openInterest = (fieldInt64[FieldIndex.OpenInterest]);
            fieldInt32[FieldIndex.OpenInterestDaily] = Convert.ToInt32(openInterest - preOpenInterest);

            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(unicode))
                DetailData.FieldIndexDataInt32[unicode].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;
            long volume = 0;
            float settlementPrice = 0;
            if (fieldInt64.ContainsKey(FieldIndex.Volume))
                volume = (fieldInt64[FieldIndex.Volume]);
            if (fieldSingle.ContainsKey(FieldIndex.SettlementPrice))
                settlementPrice = (fieldSingle[FieldIndex.SettlementPrice]);
            switch (mt)
            {
                case MarketType.SHF:
                case MarketType.DCE:
                case MarketType.CZC:
                case MarketType.CHFAG:
                case MarketType.CHFCU:
                    fieldDouble[FieldIndex.Amount] = Convert.ToDouble(settlementPrice * volume);
                    break;
            }
        }


        private void CaculateHKData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float preClose = 0, now = 0, high = 0, low = 0;
            long volume = 0;
            double amount = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            if (now != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }

                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (volume != 0)
                    fieldSingle[FieldIndex.AveragePrice] = Convert.ToSingle(amount / volume);
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                now = preClose;
            }

            if (now != 0)
            {
                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
            }
        }

        private void CaculateCloseFundData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }
            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float preClose = 0, now = 0, high = 0, low = 0;
            long volume = 0;
            double amount = 0;

            float preCloseDay3 = 0,
            preCloseDay5 = 0,
            preCloseDay10 = 0,
            preCloseDay20 = 0,
            preCloseDay60 = 0,
            preCloseDay120 = 0,
            preCloseDay250 = 0,
            preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);
            if (now != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;

                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (fieldInt64.ContainsKey(FieldIndex.Volume))
                    volume = (fieldInt64[FieldIndex.Volume]);
                if (volume != 0)
                    fieldSingle[FieldIndex.AveragePrice] = Convert.ToSingle(amount / volume);
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);

                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }
            if (fieldSingle.ContainsKey(FieldIndex.FundDecYearhb))
                fieldSingle[FieldIndex.FundAvgIncomeYear] = fieldSingle[FieldIndex.FundDecYearhb];
            if (fieldSingle.ContainsKey(FieldIndex.FundDecZdf))
            {
                fieldSingle[FieldIndex.FundNvgrwtd] = fieldSingle[FieldIndex.FundDecZdf];
                fieldSingle[FieldIndex.FundNetZZL] = fieldSingle[FieldIndex.FundDecZdf];
            }

        }

        private void CaculateOpenFundData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;


            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }

            if (fieldSingle.ContainsKey(FieldIndex.FundDecYearhb))
                fieldSingle[FieldIndex.FundAvgIncomeYear] = fieldSingle[FieldIndex.FundDecYearhb];
            if (fieldSingle.ContainsKey(FieldIndex.FundDecZdf))
            {
                fieldSingle[FieldIndex.FundNvgrwtd] = fieldSingle[FieldIndex.FundDecZdf];
                fieldSingle[FieldIndex.FundNetZZL] = fieldSingle[FieldIndex.FundDecZdf];
            }

        }

        private void CacultateRateData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }

            float preClose = 0, now = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);


            if (now != 0)
            {
                fieldSingle[FieldIndex.Difference] = now - preClose;
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                now = preClose;
            }

            if (now != 0)
            {
                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
            }

        }

        private void CaculateIndexData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }

            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float preClose = 0, now = 0, high = 0, low = 0;
            int upNum = 0, stockNum = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);


            if (now != 0)
            {
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);

                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }

            if (fieldInt32.ContainsKey(FieldIndex.UpNum))
                upNum = (fieldInt32[FieldIndex.UpNum]);
            if (fieldInt32.ContainsKey(FieldIndex.StockNum))
                stockNum = (fieldInt32[FieldIndex.StockNum]);
            if (stockNum != 0)
                fieldSingle[FieldIndex.UpRange] = upNum * 1.0f / stockNum;

            if (fieldSingle.ContainsKey(FieldIndex.DifferRange3Mint))
                fieldSingle[FieldIndex.DifferSpeed] = fieldSingle[FieldIndex.DifferRange3Mint];

            if (fieldInt64.ContainsKey(FieldIndex.Volume))
                fieldInt64[FieldIndex.Volume] = (fieldInt64[FieldIndex.Volume]);

        }

        private void CaculateCSIndexData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }

            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float preClose = 0, now = 0, high = 0, low = 0;
            int upNum = 0, stockNum = 0;

            float preCloseDay3 = 0,
                preCloseDay5 = 0,
                preCloseDay10 = 0,
                preCloseDay20 = 0,
                preCloseDay60 = 0,
                preCloseDay120 = 0,
                preCloseDay250 = 0,
                preCloseYtd = 0;

            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay20))
                preCloseDay20 = (fieldSingle[FieldIndex.PreCloseDay20]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay60))
                preCloseDay60 = (fieldSingle[FieldIndex.PreCloseDay60]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay120))
                preCloseDay120 = (fieldSingle[FieldIndex.PreCloseDay120]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay250))
                preCloseDay250 = (fieldSingle[FieldIndex.PreCloseDay250]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDayYTD))
                preCloseYtd = (fieldSingle[FieldIndex.PreCloseDayYTD]);

            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.Low))
                low = (fieldSingle[FieldIndex.Low]);
            if (fieldSingle.ContainsKey(FieldIndex.High))
                high = (fieldSingle[FieldIndex.High]);
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);


            if (now != 0)
            {
                if (preClose != 0)
                {
                    fieldSingle[FieldIndex.Difference] = now - preClose;
                    fieldSingle[FieldIndex.Delta] = (high - low) / preClose;
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }

                fieldSingle[FieldIndex.Open] = now;
                fieldSingle[FieldIndex.High] = now;
                fieldSingle[FieldIndex.Low] = now;
                fieldSingle[FieldIndex.AveragePrice] = now;

                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
                if (preCloseDay20 != 0)
                    fieldSingle[FieldIndex.DifferRange20D] = (now - preCloseDay20) / preCloseDay20;
                if (preCloseDay60 != 0)
                    fieldSingle[FieldIndex.DifferRange60D] = (now - preCloseDay60) / preCloseDay60;
                if (preCloseDay120 != 0)
                    fieldSingle[FieldIndex.DifferRange120D] = (now - preCloseDay120) / preCloseDay120;
                if (preCloseDay250 != 0)
                    fieldSingle[FieldIndex.DifferRange250D] = (now - preCloseDay250) / preCloseDay250;
                if (preCloseYtd != 0)
                    fieldSingle[FieldIndex.DifferRangeYTD] = (now - preCloseYtd) / preCloseYtd;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);

                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                    if (preCloseDay20 != 0)
                        fieldSingle[FieldIndex.DifferRange20D] = (preClose - preCloseDay20) / preCloseDay20;
                    if (preCloseDay60 != 0)
                        fieldSingle[FieldIndex.DifferRange60D] = (preClose - preCloseDay60) / preCloseDay60;
                    if (preCloseDay120 != 0)
                        fieldSingle[FieldIndex.DifferRange120D] = (preClose - preCloseDay120) / preCloseDay120;
                    if (preCloseDay250 != 0)
                        fieldSingle[FieldIndex.DifferRange250D] = (preClose - preCloseDay250) / preCloseDay250;
                    if (preCloseYtd != 0)
                        fieldSingle[FieldIndex.DifferRangeYTD] = (preClose - preCloseYtd) / preCloseYtd;
                }
            }

            if (fieldInt32.ContainsKey(FieldIndex.UpNum))
                upNum = (fieldInt32[FieldIndex.UpNum]);
            if (fieldInt32.ContainsKey(FieldIndex.StockNum))
                stockNum = (fieldInt32[FieldIndex.StockNum]);
            if (stockNum != 0)
                fieldSingle[FieldIndex.UpRange] = upNum * 1.0f / stockNum;

            if (fieldSingle.ContainsKey(FieldIndex.DifferRange3Mint))
                fieldSingle[FieldIndex.DifferSpeed] = fieldSingle[FieldIndex.DifferRange3Mint];

        }

        private void CaculateFinanceData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            if (fieldSingle.ContainsKey(FieldIndex.FundDecYearhb))
                fieldSingle[FieldIndex.FundAvgIncomeYear] = fieldSingle[FieldIndex.FundDecYearhb];
            if (fieldSingle.ContainsKey(FieldIndex.FundDecZdf))
            {
                fieldSingle[FieldIndex.FundNvgrwtd] = fieldSingle[FieldIndex.FundDecZdf];
                fieldSingle[FieldIndex.FundNetZZL] = fieldSingle[FieldIndex.FundDecZdf];
            }

            double ZZC = 0, TotalLiab = 0, OWnerEqu = 0, CapRes = 0, RProfotAA = 0, ZGB = 0;
            if (fieldDouble.ContainsKey(FieldIndex.ZZC))
                ZZC = (fieldDouble[FieldIndex.ZZC]);
            if (fieldDouble.ContainsKey(FieldIndex.TotalLiab))
                TotalLiab = (fieldDouble[FieldIndex.TotalLiab]);
            if (fieldDouble.ContainsKey(FieldIndex.OWnerEqu))
                OWnerEqu = (fieldDouble[FieldIndex.OWnerEqu]);
            if (fieldDouble.ContainsKey(FieldIndex.CapRes))
                CapRes = (fieldDouble[FieldIndex.CapRes]);
            if (fieldDouble.ContainsKey(FieldIndex.RProfotAA))
                RProfotAA = (fieldDouble[FieldIndex.RProfotAA]);
            if (fieldDouble.ContainsKey(FieldIndex.ZGB))
                ZGB = (fieldDouble[FieldIndex.ZGB]);

            if (ZZC != 0)
            {
                fieldDouble[FieldIndex.ZCFZL] = TotalLiab / ZZC;
                fieldDouble[FieldIndex.OEquRatio] = OWnerEqu / ZZC;
            }
            if (ZGB != 0)
            {
                fieldDouble[FieldIndex.DRPCapRes] = CapRes / ZGB;
                fieldDouble[FieldIndex.DRPRPAA] = RProfotAA / ZGB;
            }

        }

        private void CaculateDDEData(int unicode)
        {

            Dictionary<FieldIndex, float> fieldSingle;


            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }

            float now = 0, buySuperRange = 0, sellSuperRange = 0, buyBigRange = 0, sellBigRange = 0, preClose = 0;

            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (!now.Equals(0))
            {

                if (!preClose.Equals(0))
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
            }

            if (fieldSingle.ContainsKey(FieldIndex.BuyFlowRangeSuper))
                buySuperRange = (fieldSingle[FieldIndex.BuyFlowRangeSuper]);
            if (fieldSingle.ContainsKey(FieldIndex.SellFlowRangeSuper))
                sellSuperRange = (fieldSingle[FieldIndex.SellFlowRangeSuper]);
            if (fieldSingle.ContainsKey(FieldIndex.BuyFlowRangeBig))
                buyBigRange = (fieldSingle[FieldIndex.BuyFlowRangeBig]);
            if (fieldSingle.ContainsKey(FieldIndex.SellFlowRangeBig))
                sellBigRange = (fieldSingle[FieldIndex.SellFlowRangeBig]);
            fieldSingle[FieldIndex.NetFlowRangeSuper] = buySuperRange - sellSuperRange;
            fieldSingle[FieldIndex.NetFlowRangeBig] = buyBigRange - sellBigRange;
        }

        private void CaculateCapitalFlowReportData(int unicode)
        {
            try
            {
                Dictionary<FieldIndex, float> fieldSingle;
                Dictionary<FieldIndex, int> fieldInt32;
                Dictionary<FieldIndex, long> fieldInt64;
                Dictionary<FieldIndex, double> fieldDouble;

                if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
                {
                    fieldSingle = new Dictionary<FieldIndex, float>(1);
                    DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
                }
                if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
                {
                    fieldInt32 = new Dictionary<FieldIndex, int>(1);
                    DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
                }
                if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
                {
                    fieldInt64 = new Dictionary<FieldIndex, long>(1);
                    DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
                }

                if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
                {
                    fieldDouble = new Dictionary<FieldIndex, double>(1);
                    DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
                }
                float now = 0, preClose = 0;
                int buySuper = 0,
                    sellSuper = 0,
                    buyBig = 0,
                    sellBig = 0,
                    buyMiddle = 0,
                    sellMiddle = 0,
                    buySmall = 0,
                    sellSmall = 0;
                double amount = 0;

                if (fieldSingle.ContainsKey(FieldIndex.Now))
                    now = (fieldSingle[FieldIndex.Now]);
                if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                    preClose = (fieldSingle[FieldIndex.PreClose]);
                if (!now.Equals(0))
                {
                    if (preClose != 0)
                        fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                }
                else
                {
                    if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                        fieldSingle.Remove(FieldIndex.DifferRange);
                    if (fieldSingle.ContainsKey(FieldIndex.Delta))
                        fieldSingle.Remove(FieldIndex.Delta);
                    if (fieldSingle.ContainsKey(FieldIndex.Difference))
                        fieldSingle.Remove(FieldIndex.Difference);
                }

                if (fieldInt32.ContainsKey(FieldIndex.BuySuper))
                    buySuper = (fieldInt32[FieldIndex.BuySuper]);
                if (fieldInt32.ContainsKey(FieldIndex.SellSuper))
                    sellSuper = (fieldInt32[FieldIndex.SellSuper]);
                if (fieldInt32.ContainsKey(FieldIndex.BuyBig))
                    buyBig = (fieldInt32[FieldIndex.BuyBig]);
                if (fieldInt32.ContainsKey(FieldIndex.SellBig))
                    sellBig = (fieldInt32[FieldIndex.SellBig]);
                if (fieldInt32.ContainsKey(FieldIndex.BuyMiddle))
                    buyMiddle = (fieldInt32[FieldIndex.BuyMiddle]);
                if (fieldInt32.ContainsKey(FieldIndex.SellMiddle))
                    sellMiddle = (fieldInt32[FieldIndex.SellMiddle]);
                if (fieldInt32.ContainsKey(FieldIndex.BuySmall))
                    buySmall = (fieldInt32[FieldIndex.BuySmall]);
                if (fieldInt32.ContainsKey(FieldIndex.SellSmall))
                    sellSmall = (fieldInt32[FieldIndex.SellSmall]);

                fieldInt32[FieldIndex.NetFlowSuper] = (buySuper - sellSuper);
                fieldInt32[FieldIndex.NetFlowBig] = (buyBig - sellBig);
                fieldInt32[FieldIndex.NetFlowMiddle] = (buyMiddle - sellMiddle);
                fieldInt32[FieldIndex.NetFlowSmall] = (buySmall - sellSmall);
                fieldInt32[FieldIndex.MainNetFlow] = ((buySuper - sellSuper) + (buyBig - sellBig));


                if (fieldDouble.ContainsKey(FieldIndex.Amount))
                    amount = (fieldDouble[FieldIndex.Amount]);
                if (amount != 0)
                {
                    fieldSingle[FieldIndex.NetFlowRangeSuper] = Convert.ToSingle((buySuper - sellSuper) * 1.0f / amount);
                    fieldSingle[FieldIndex.NetFlowRangeBig] = Convert.ToSingle((buyBig - sellBig) * 1.0f / amount);
                    fieldSingle[FieldIndex.NetFlowRangeMiddle] = Convert.ToSingle((buyMiddle - sellMiddle) * 1.0f / amount);
                    fieldSingle[FieldIndex.NetFlowRangeSmall] = Convert.ToSingle((buySmall - sellSmall) * 1.0f / amount);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("资金流向 Error" + e.Message);
            }

        }

        private void CaculateNetInflowReportData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }

            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }

            float now = 0, preClose = 0, preCloseDay3 = 0, preCloseDay5 = 0, preCloseDay10 = 0;
            int ZengCangRank = 0,
                ZengCangRankHis = 0,
                ZengCangRankDay3 = 0,
                ZengCangRankHisDay3 = 0,
                ZengCangRankDay5 = 0,
                ZengCangRankHisDay5 = 0,
                ZengCangRankDay10 = 0,
                ZengCangRankHisDay10 = 0;

            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay3))
                preCloseDay3 = (fieldSingle[FieldIndex.PreCloseDay3]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay5))
                preCloseDay5 = (fieldSingle[FieldIndex.PreCloseDay5]);
            if (fieldSingle.ContainsKey(FieldIndex.PreCloseDay10))
                preCloseDay10 = (fieldSingle[FieldIndex.PreCloseDay10]);
            if (now != 0)
            {
                if (preClose != 0)
                    fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;
                if (preCloseDay3 != 0)
                    fieldSingle[FieldIndex.DifferRange3D] = (now - preCloseDay3) / preCloseDay3;
                if (preCloseDay5 != 0)
                    fieldSingle[FieldIndex.DifferRange5D] = (now - preCloseDay5) / preCloseDay5;
                if (preCloseDay10 != 0)
                    fieldSingle[FieldIndex.DifferRange10D] = (now - preCloseDay10) / preCloseDay10;
            }
            else
            {
                if (fieldSingle.ContainsKey(FieldIndex.DifferRange))
                    fieldSingle.Remove(FieldIndex.DifferRange);
                if (fieldSingle.ContainsKey(FieldIndex.Delta))
                    fieldSingle.Remove(FieldIndex.Delta);
                if (fieldSingle.ContainsKey(FieldIndex.Difference))
                    fieldSingle.Remove(FieldIndex.Difference);
                if (preClose != 0)
                {
                    if (preCloseDay3 != 0)
                        fieldSingle[FieldIndex.DifferRange3D] = (preClose - preCloseDay3) / preCloseDay3;
                    if (preCloseDay5 != 0)
                        fieldSingle[FieldIndex.DifferRange5D] = (preClose - preCloseDay5) / preCloseDay5;
                    if (preCloseDay10 != 0)
                        fieldSingle[FieldIndex.DifferRange10D] = (preClose - preCloseDay10) / preCloseDay10;
                }
            }

            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRank))
                ZengCangRank = (fieldInt32[FieldIndex.ZengCangRank]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankHis))
                ZengCangRankHis = (fieldInt32[FieldIndex.ZengCangRankHis]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankDay3))
                ZengCangRankDay3 = (fieldInt32[FieldIndex.ZengCangRankDay3]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankHisDay3))
                ZengCangRankHisDay3 = (fieldInt32[FieldIndex.ZengCangRankHisDay3]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankDay5))
                ZengCangRankDay5 = (fieldInt32[FieldIndex.ZengCangRankDay5]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankHisDay5))
                ZengCangRankHisDay5 = (fieldInt32[FieldIndex.ZengCangRankHisDay5]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankDay10))
                ZengCangRankDay10 = (fieldInt32[FieldIndex.ZengCangRankDay10]);
            if (fieldInt32.ContainsKey(FieldIndex.ZengCangRankHisDay10))
                ZengCangRankHisDay10 = (fieldInt32[FieldIndex.ZengCangRankHisDay10]);

            fieldInt32[FieldIndex.ZengCangRankChange] = ZengCangRankHis - ZengCangRank;
            fieldInt32[FieldIndex.ZengCangRankChangeDay3] = ZengCangRankHisDay3 - ZengCangRankDay3;
            fieldInt32[FieldIndex.ZengCangRankChangeDay5] = ZengCangRankHisDay5 - ZengCangRankDay5;
            fieldInt32[FieldIndex.ZengCangRankChangeDay10] = ZengCangRankHisDay10 - ZengCangRankDay10;
        }

        private void CaculateProfitForestReportData(int unicode)
        {
            Dictionary<FieldIndex, float> fieldSingle;
            Dictionary<FieldIndex, int> fieldInt32;
            Dictionary<FieldIndex, long> fieldInt64;
            Dictionary<FieldIndex, double> fieldDouble;

            if (!DetailData.FieldIndexDataSingle.TryGetValue(unicode, out fieldSingle))
            {
                fieldSingle = new Dictionary<FieldIndex, float>(1);
                DetailData.FieldIndexDataSingle[unicode] = fieldSingle;
            }
            if (!DetailData.FieldIndexDataInt32.TryGetValue(unicode, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[unicode] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataInt64.TryGetValue(unicode, out fieldInt64))
            {
                fieldInt64 = new Dictionary<FieldIndex, long>(1);
                DetailData.FieldIndexDataInt64[unicode] = fieldInt64;
            }

            if (!DetailData.FieldIndexDataDouble.TryGetValue(unicode, out fieldDouble))
            {
                fieldDouble = new Dictionary<FieldIndex, double>(1);
                DetailData.FieldIndexDataDouble[unicode] = fieldDouble;
            }
            float now = 0, preClose = 0;
            double espCurrentYear = 0, epsNextYear1 = 0, epsNextYear2 = 0, epsNextYear3 = 0;
            if (fieldSingle.ContainsKey(FieldIndex.Now))
                now = (fieldSingle[FieldIndex.Now]);
            if (fieldSingle.ContainsKey(FieldIndex.PreClose))
                preClose = (fieldSingle[FieldIndex.PreClose]);
            if (now != 0 && preClose != 0)
                fieldSingle[FieldIndex.DifferRange] = (now - preClose) / preClose;

            if (fieldDouble.ContainsKey(FieldIndex.EpsCurrentYear))
                espCurrentYear = (fieldDouble[FieldIndex.EpsCurrentYear]);
            if (fieldDouble.ContainsKey(FieldIndex.EpsNextYear1))
                epsNextYear1 = (fieldDouble[FieldIndex.EpsNextYear1]);
            if (fieldDouble.ContainsKey(FieldIndex.EpsNextYear2))
                epsNextYear2 = (fieldDouble[FieldIndex.EpsNextYear2]);
            if (fieldDouble.ContainsKey(FieldIndex.EpsNextYear3))
                epsNextYear3 = (fieldDouble[FieldIndex.EpsNextYear3]);
            if (now != 0)
            {
                if (espCurrentYear != 0)
                    fieldSingle[FieldIndex.PECurrentYear] = Convert.ToSingle(now / espCurrentYear);
                if (epsNextYear1 != 0)
                    fieldSingle[FieldIndex.PENextYear1] = Convert.ToSingle(now / epsNextYear1);
                if (epsNextYear2 != 0)
                    fieldSingle[FieldIndex.PENextYear2] = Convert.ToSingle(now / epsNextYear2);
                if (epsNextYear3 != 0)
                    fieldSingle[FieldIndex.PENextYear3] = Convert.ToSingle(now / epsNextYear1);
            }
        }

        #endregion

        #endregion

        #region 自选股
        // <summary>
        // 自选股成分发生变化
        // </summary>
        // <param name="blockId">发生成分变化的板块id</param>
        private void OnCustomBlockElementUpdated(string blockId, List<string> lstUpdCodes, List<string> lstDelCodes)
        {
            if (blockId == "0.U")
            {
                if (lstUpdCodes != null && lstUpdCodes.Count > 0)
                {
                    Dictionary<FieldIndex, object> fieldObject;
                    foreach (string code in lstUpdCodes)
                    {
                        if (!DetailData.FieldIndexDataObject.TryGetValue(Convert.ToInt32(code), out fieldObject))
                        {
                            fieldObject = new Dictionary<FieldIndex, object>(1);
                            DetailData.FieldIndexDataObject[Convert.ToInt32(code)] = fieldObject;
                        }

                        fieldObject[FieldIndex.IsDefaultCustomStock] = true;
                        Dc.CustomCodesList.Add(Convert.ToInt32(code));
                    }
                }

                if (lstDelCodes != null && lstDelCodes.Count > 0)
                {
                    Dictionary<FieldIndex, object> fieldObject;
                    foreach (string code in lstDelCodes)
                    {
                        if (!DetailData.FieldIndexDataObject.TryGetValue(Convert.ToInt32(code), out fieldObject))
                        {
                            fieldObject = new Dictionary<FieldIndex, object>(1);
                            DetailData.FieldIndexDataObject[Convert.ToInt32(code)] = fieldObject;
                        }
                        fieldObject[FieldIndex.IsDefaultCustomStock] = false;
                        if (Dc.CustomCodesList.Contains(Convert.ToInt32(code)))
                            Dc.CustomCodesList.Remove(Convert.ToInt32(code));
                    }
                }
            }
        }

        #endregion

        public override void ClearData(InitOrgStatus status)
        {
            MarketType mt = MarketType.NA;
            List<MarketType> mtList;
            List<int> removeCode = new List<int>(1);
            foreach (KeyValuePair<int, Dictionary<FieldIndex, string>> oneStock in DetailData.FieldIndexDataString)
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
                if (DetailData.FieldIndexDataString.ContainsKey(code))
                    DetailData.FieldIndexDataString.Remove(code);
                if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                    DetailData.FieldIndexDataInt32.Remove(code);
                if (DetailData.FieldIndexDataInt64.ContainsKey(code))
                    DetailData.FieldIndexDataInt64.Remove(code);
                if (DetailData.FieldIndexDataSingle.ContainsKey(code))
                    DetailData.FieldIndexDataSingle.Remove(code);
                if (DetailData.FieldIndexDataDouble.ContainsKey(code))
                    DetailData.FieldIndexDataDouble.Remove(code);
                if (DetailData.FieldIndexDataObject.ContainsKey(code))
                    DetailData.FieldIndexDataObject.Remove(code);
            }
        }

        //初始化部分，待确定..........
            //List<int> keys = new List<int>(AllDetailDataRec.Keys);
            //for (int i = 0; i < keys.Count; i++)
            //{

            //    if (AllDetailDataRec[keys[i]].ContainsKey(FieldIndex.Market))
            //    {
            //        objMt = AllDetailDataRec[keys[i]][FieldIndex.Market];
            //        if (objMt != null)
            //            mt = (MarketType)objMt;
            //        if (clearMarketType.Contains(mt))
            //        {
            //            Dictionary<FieldIndex, object> newData = new Dictionary<FieldIndex, object>(5);
            //            newData[FieldIndex.Code] = AllDetailDataRec[keys[i]][FieldIndex.Code];
            //            newData[FieldIndex.EMCode] = AllDetailDataRec[keys[i]][FieldIndex.EMCode];
            //            newData[FieldIndex.Name] = AllDetailDataRec[keys[i]][FieldIndex.Name];
            //            newData[FieldIndex.Market] = AllDetailDataRec[keys[i]][FieldIndex.Market];
            //            AllDetailDataRec[keys[i]].Clear();
            //            AllDetailDataRec[keys[i]] = newData;
            //        }
            //    }
            //}
        
    }
}
