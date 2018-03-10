using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using System.Data;


namespace EmQComm
{
    #region 枚举值
    public class ReqLogonCftDataPacket : RealTimeDataPacket
    {
        public ReqLogonCftDataPacket()
        {
            base.RequestType = FuncTypeRealTime.InitLogon;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)1);
            bw.Write((byte)0);
            bw.Write((byte)0);
            bw.Write((byte)0);
            bw.Write((byte)0);
            bw.Write(0);
            bw.Write(-3);
            byte[] buffer = new byte[0x20];
            string s = "guest\0";
            byte[] bytes = Encoding.Default.GetBytes(s);
            for (int i = 0; (i < bytes.Length) && (i < buffer.Length); i++)
            {
                buffer[i] = bytes[i];
            }
            bw.Write(buffer);
            bw.Write(0);
            byte[] buffer3 = new byte[0x20];
            string str2 = "guest\0";
            byte[] buffer4 = Encoding.Default.GetBytes(str2);
            for (int j = 0; (j < buffer4.Length) && (j < buffer3.Length); j++)
            {
                buffer3[j] = buffer4[j];
            }
            bw.Write(buffer3);
            bw.Write(0);
            bw.Write((byte)0);
            byte[] buffer5 = new byte[0x40];
            bw.Write(buffer5);
        }
    }

    public enum FuncTypeRealTime
    {
        CustomReport = 0x3ec4,
        AllOrderStockDetailLevel2 = 0x331,
        BlockIndexReport = 0x17b5,
        BlockOverViewList = 0x177e,
        BlockQuoteReport = 0x17b2,
        BlockSimpleQuote = 0x177f,
        CapitalFlow = 0x328,
        CapitalFlowDay = 0x3f0,
        ContributionBlock = 0x177d,
        ContributionStock = 0x177c,
        DealRequest = 0x1773,
        DealSubscribe = 0x206,
        FundIopv = 0x2e3,
        FundTradeInfo = 0x270d,
        Heart = 0x201,
        HisHeart = 0x17a7,
        HisKLine = 0x3e9,
        HisTrend = 0x8f,
        HisTrendlinecfs = 0x3f8,
        IndexDetail = 0x195,
        IndexFutruesPriceStatus = 0x32d,
        IndexFuturesDetail = 0x32c,
        IndexFuturesTrend = 0x17af,
        Init = 0x270f,
        InitLogon = 0x141,
        LimitedPrice = 0x2de,
        MinKLine = 0x403,
        NOrderStockDetailLevel2 = 0x333,
        OceanHeart = 0x32b,
        OceanRecord = 11,
        OceanTrend = 0x85,
        OrderDetail = 0x322,
        OrderQueue = 0x323,
        PriceStatus = 0x330,
        Rank = 0x1774,
        Rank5Mint = 0x1775,
        RedGreen = 0x1778,
        ReqF10 = 0x3ea,
        SectorQuoteReport = 0x17b1,
        ShortLineStrategy = 0x26d,
        StatisticsAnalysis = 0x270e,
        StockDetail = 0x1784,
        StockDetailLev2 = 0x32f,
        StockDetailOrderQueue = 0x332,
        StockDict = 0x1783,
        StockIndexReport = 0x17b3,
        StockSimpleQuote = 0x1ff,
        StockTrend = 0xbcc,
        StockTrendAskBid = 0x17ac,
        StockTrendInOutDiff = 0x17ad,
        StockTrendPush = 0x1785,
        TickTrade = 0x324,
        TrendCapitalFlow = 0xbcd
    }

    ///// <summary>
    ///// 实时，历史行情服务器请求ID
    ///// </summary>
    //public enum FuncTypeRealTime
    //{
    //    /// <summary>
    //    /// 外盘行情记录
    //    /// </summary>
    //    OceanRecord = 11,
    //    /// <summary>
    //    /// 外盘走势
    //    /// </summary>
    //    OceanTrend = 133,
    //    /// <summary>
    //    /// 历史走势
    //    /// </summary>
    //    HisTrend = 143,
    //    /// <summary>
    //    /// 登陆包
    //    /// </summary>
    //    InitLogon = 321,
    //    /// <summary>
    //    /// 外盘心跳
    //    /// </summary>
    //    OceanHeart = 513,
    //    /// <summary>
    //    /// 历史K线
    //    /// </summary>
    //    HisKLine = 1001,
    //    /// <summary>
    //    /// F10
    //    /// </summary>
    //    ReqF10 = 1002,

    //    /// <summary>
    //    /// 资金流日K线
    //    /// </summary>
    //    CapitalFlowDay = 1008,
    //    /// <summary>
    //    /// 历史分时资金流
    //    /// </summary>
    //    HisTrendlinecfs = 1016,

    //    /// <summary>
    //    /// 请求成交明细
    //    /// </summary>
    //    DealRequest = 6003,
    //    /// <summary>
    //    /// 综合排名
    //    /// </summary>
    //    Rank = 6004,
    //    /// <summary>
    //    /// 5分钟综合排名
    //    /// </summary>
    //    Rank5Mint = 6005,
    //    /// <summary>
    //    /// 红绿柱
    //    /// </summary>
    //    RedGreen = 6008,
    //    /// <summary>
    //    /// 个股贡献指数点数
    //    /// </summary>
    //    ContributionStock = 6012,
    //    /// <summary>
    //    /// 板块贡献指数点数
    //    /// </summary>
    //    ContributionBlock = 6013,
    //    /// <summary>
    //    /// 板块基本信息
    //    /// </summary>
    //    BlockOverViewList = 6014,
    //    /// <summary>
    //    /// 板块简易行情
    //    /// </summary>
    //    BlockSimpleQuote = 6015,
    //    /// <summary>
    //    /// 指数行情
    //    /// </summary>
    //    IndexDetail = 6018,
    //    /// <summary>
    //    /// 码表
    //    /// </summary>
    //    StockDict = 6019,
    //    /// <summary>
    //    /// 个股行情
    //    /// </summary>
    //    StockDetail = 6020,
    //    /// <summary>
    //    /// 分时走势推送
    //    /// </summary>
    //    StockTrendPush = 6021,
    //    /// <summary>
    //    /// 订阅成交明细
    //    /// </summary>
    //    DealSubscribe = 6022,
    //    /// <summary>
    //    /// 短线精灵
    //    /// </summary>
    //    ShortLineStrategy = 6035,
    //    /// <summary>
    //    /// 个股行情Level2
    //    /// </summary>
    //    StockDetailLev2 = 6046,
    //    /// <summary>
    //    /// 委托明细（深证）
    //    /// </summary>
    //    OrderDetail = 6047,
    //    /// <summary>
    //    /// 委托队列
    //    /// </summary>
    //    OrderQueue = 6048,
    //    /// <summary>
    //    /// 逐笔成交
    //    /// </summary>
    //    TickTrade = 6049,
    //    /// <summary>
    //    /// 个股资金流
    //    /// </summary>
    //    CapitalFlow = 6051,
    //    /// <summary>
    //    /// 分价表
    //    /// </summary>
    //    PriceStatus = 6052,
    //    /// <summary>
    //    /// 心跳
    //    /// </summary>
    //    Heart = 6055,
    //    /// <summary>
    //    /// 股指期货行情
    //    /// </summary>
    //    IndexFuturesDetail = 6056,
    //    /// <summary>
    //    /// 分价
    //    /// </summary>
    //    IndexFutruesPriceStatus = 6057,
    //    /// <summary>
    //    /// 当日K线
    //    /// </summary>
    //    MinKLine = 6059,
    //    /// <summary>
    //    /// 走势委买委卖
    //    /// </summary>
    //    StockTrendAskBid = 6060,
    //    /// <summary>
    //    /// 走势内外盘差
    //    /// </summary>
    //    StockTrendInOutDiff = 6061,
    //    /// <summary>
    //    /// 个股走势
    //    /// </summary>
    //    StockTrend = 6062,
    //    /// <summary>
    //    /// 股指期货走势
    //    /// </summary>
    //    IndexFuturesTrend = 6063,
    //    /// <summary>
    //    /// 市场报价(指定个股栏位)
    //    /// </summary>
    //    SectorQuoteReport = 6065,
    //    /// <summary>
    //    /// 板块个股行情(制定个股栏位)
    //    /// </summary>
    //    BlockQuoteReport = 6066,
    //    /// <summary>
    //    /// 自选股类型个股行情(制定个股栏位)
    //    /// </summary>
    //    StockIndexReport = 6067,
    //    /// <summary>
    //    /// 自选股类型板块行情(指定板块栏位)
    //    /// </summary>
    //    BlockIndexReport = 6069,
    //    /// <summary>
    //    /// 百档行情明细
    //    /// </summary>
    //    AllOrderStockDetailLevel2 = 6071,
    //    /// <summary>
    //    /// 百档挂单行情
    //    /// </summary>
    //    StockDetailOrderQueue = 6072,
    //    /// <summary>
    //    /// 分时资金流
    //    /// </summary>
    //    TrendCapitalFlow = 6073,
    //    /// <summary>
    //    /// 多档行情明细
    //    /// </summary>
    //    NOrderStockDetailLevel2 = 6074,
    //    /// <summary>
    //    /// 统计分析
    //    /// </summary>
    //    StatisticsAnalysis = 9998,

    //    /// <summary>
    //    /// 初始化消息
    //    /// </summary>
    //    Init = 9999,
    //}

    /// <summary>
    /// 资讯服务器请求ID
    /// </summary>
    public enum FuncTypeInfo
    {
        /// <summary>
        /// 24小时滚动新闻
        /// </summary>
        News24 = 1,

        /// <summary>
        /// 财务
        /// </summary>
        Finance = 2,

        /// <summary>
        /// 板块结构
        /// </summary>
        Block = 4,

        /// <summary>
        /// 个股机构评级
        /// </summary>
        OrgRate = 7,

        /// <summary>
        /// 盈利预测
        /// </summary>
        ProfitForecast = 8,

        /// <summary>
        /// 除权除息
        /// </summary>
        DivideRight = 10,

        /// <summary>
        /// 提示
        /// </summary>
        Notice = 11,

        /// <summary>
        /// 信息地雷
        /// </summary>
        NewsReport = 14,

        /// <summary>
        /// 自选股信息地雷
        /// </summary>
        CustomStockNewsReport = 17,

        /// <summary>
        /// 心跳
        /// </summary>
        InfoHeart = 4005
    }

    /// <summary>
    /// 机构服务器请求ID
    /// </summary>
    public enum FuncTypeOrg
    {
        ABHRelation = 0x42ca,
        BankBondReport = 0x4a8d,
        BlockIndexReport = 0x3ec3,
        BlockReport = 0x3ec1,
        BlockStockReport = 0x3ec2,
        BondPublicOpeartion = 0x3ee7,
        BondStockReport = 0x3ec7,
        BrokerQueue = 0x470d,
        CapitalFlowReport = 0x2af9,
        CASVCMFLAG = 0x470f,
        CFERelation = 0x42c9,
        ChangeName = 0x272a,
        CirculatingSharesHistory = 0x4a75,
        CNIndexDetail = 0x4a50,
        ConvertBondDetail = 0x42b5,
        ConvertBondNewDetail = 0x42c8,
        CSIIndexDetail = 0x4a52,
        CSIndexDetail = 0x4a51,
        CustomCapitalFlowReport = 0x3edb,
        CustomDDEReport = 0x3ede,
        CustomFinanceStockReport = 0x3edc,
        CustomNetInFlowReport = 0x3edf,
        CustomProfitForecastReport = 0x3edd,
        CustomRank = 0x3ee0,
        CustomReport = 0x3ec4,
        DDEReport = 0x2afb,
        DepthAnalyse = 0x42af,
        DivideRightOrg = 0x2728,
        DomesticFutures = 0x46a7,
        EMIndexDetail = 0x42b0,
        EmIndexReport = 0x3ecd,
        FinanceHeaveBondReport = 0x4a47,
        FinanceHeaveFundReport = 0x4a48,
        FinanceHeaveHYReport = 0x4a46,
        FinanceHeaveManagerReport = 0x4a49,
        FinanceHeaveStockReport = 0x4a45,
        FinanceOrg = 0x2727,
        FinanceReport = 0x3ecb,
        FinanceStockReport = 0x2afc,
        FJJJHistoryData = 0x4333,
        FJJJRelation = 0x42cd,
        ForexDetail = 0x469a,
        ForexDetailNew = 0x469b,
        ForexReport = 0x3ee1,
        FundBFPDetail = 0x4a4f,
        FundCIPMonetaryDetail = 0x4a43,
        FundCIPNonMonetaryDetail = 0x4a44,
        FundHeaveStockReport = 0x4a3b,
        FundHYReport = 0x4a3c,
        FundIndicator = 0x433a,
        FundKeyBondReport = 0x4a3d,
        FundKlineAfterDivide = 0x4a40,
        FundManager = 0x4a3e,
        FundPriceDetail = 0x4a41,
        FundStockReport = 0x3ec6,
        FundTrpAndSunDetail = 0x4a4b,
        FuturesStockReport = 0x3ec8,
        GlobalData = 0x2724,
        GlobalIndexDetail = 0x42bf,
        GlobalIndexNewDetail = 0x46af,
        GlobalIndexReport = 0x3ec9,
        HeartOrg = 0x3eb7,
        HisKLineOrg = 0x4308,
        HKDetail = 0x42c0,
        HKDetailAll = 0x470e,
        HKFinance = 0x3e8b,
        HKStockReport = 0x3ec5,
        HKTradeDate = 0x2718,
        IncrementFactor = 0x2791,
        IncrementFactorHK = 0x2794,
        IncrementFactorTB = 0x2796,
        IncrementFactorUS = 0x2795,
        IncrementFinance = 0x272e,
        IncrementPrice = 0x272f,
        IndexFuturesReport = 0x3ecc,
        IndexStatic = 0x42b1,
        InitOrg = 0x3e80,
        InitReportData = 0x2726,
        InterBankDetail = 0x4a56,
        InterBankRepurchaseDetail = 0x4a58,
        LowFrequencyTBY = 0x4a8b,
        MarketHistoryTradeDate = 0x2733,
        MarketTradeDate = 0x2729,
        MinKLineOrg = 0x4307,
        MinKLineOrgDP = 0x42a5,
        MonetaryFundDetail = 0x4a39,
        NetInFlowRank = 0x3ee4,
        NetInFlowReport = 0x2afa,
        NewProfitForcast = 0x42bd,
        NewSanbanStockparty = 0x4326,
        NonConvertBondDetail = 0x42b6,
        NonMonetaryFundDetail = 0x4a3a,
        OptionBlock = 0x272b,
        OptionDetail = 0x4332,
        OptionDetailDomestic = 0x4717,
        OptionDetailHK = 0x42cb,
        OptionDetailLev2 = 0x432f,
        OptionLinkedSecurity = 0x42c5,
        OptionReport = 0x3e8d,
        OSFuturesDetail = 0x469c,
        OSFuturesLMEDeal = 0x431f,
        OSFuturesLMEDetail = 0x469f,
        OsFuturesLMEReport = 0x3ee6,
        OSFuturesReport = 0x3ee3,
        OSFuturesReportNew = 0x3ed7,
        PeKlineAfter = 0x4aa4,
        PortfoliosDetail = 0x4658,
        PortfoliosHoldStock = 0x7537,
        PortfoliosHoldStockGather = 0x7538,
        PortfoliosList = 0x7536,
        ProfitForecastReport = 0x2afd,
        Rank = 0x2afe,
        RateReport = 0x3eca,
        RateSwapDetail = 0x4a5b,
        RePullStaticData = 0x3e81,
        ReqLogin = 0x3eb8,
        RZRQ = 0x42cc,
        SanBanFinance = 0x4325,
        ShiborDetail = 0x4a59,
        ShiborReport = 0x4a8e,
        SimuBao = 0x4651,
        StockPriceRatioReport = 0x3ec0,
        TBFinance = 0x4328,
        TBStockContractQuote = 0x4330,
        TBStockHistoryTradeDetail_XieYi = 0x432a,
        TBStockHistoryTradeDetail_ZuoShi = 0x4329,
        TBStockTransIntention = 0x4334,
        TBStockZuoShiTrader = 0x432b,
        TBTradeFlag = 0x432c,
        TradeDate = 0x2725,
        TrendOrg = 0x42b8,
        TrendOrgDP = 0x42b9,
        TrendOrgHistory = 0x4a74,
        USSFinance = 0x3e8c,
        USStockDetail = 0x4317,
        USStockNewDetail = 0x4700,
        USStockReport = 0x3ee2
        ///// <summary>
        ///// 历史交易日,含当天
        ///// </summary>
        //TradeDate = 10021,
        ///// <summary>
        ///// 报价初始化静态数据
        ///// </summary>
        //InitReportData = 10022,
        ///// <summary>
        ///// 财务数据
        ///// </summary>
        //FinanceOrg = 10023,
        ///// <summary>
        ///// 除复权
        ///// </summary>
        //DivideRightOrg = 10024,
        ///// <summary>
        ///// 股票中文名称更改
        ///// </summary>
        //ChangeName = 10026,
        ///// <summary>
        ///// 报价资金流向
        ///// </summary>
        //CapitalFlowReport = 11001,
        ///// <summary>
        ///// 报价增仓排名
        ///// </summary>
        //NetInFlowReport = 11002,
        ///// <summary>
        ///// 报价dde系列
        ///// </summary>
        //DDEReport = 11003,
        ///// <summary>
        ///// 报价财务数据
        ///// </summary>
        //FinanceStockReport = 11004,
        ///// <summary>
        ///// 报价盈利预测
        ///// </summary>
        //ProfitForecastReport = 11005,
        ///// <summary>
        ///// 综合排名
        ///// </summary>
        //Rank = 11006,
        ///// <summary>
        ///// 心跳
        ///// </summary>
        //HeartOrg = 16055,
        ///// <summary>
        ///// 初始化包
        ///// </summary>
        //InitOrg = 16000,
        ///// <summary>
        ///// 板块指数自身行情
        ///// </summary>
        //BlockReport = 16065,
        ///// <summary>
        ///// A股报价
        ///// </summary>
        //BlockStockReport = 16066,
        ///// <summary>
        ///// 指数报价
        ///// </summary>
        //BlockIndexReport = 16067,
        ///// <summary>
        ///// 自选股
        ///// </summary>
        //CustomReport = 16068,
        ///// <summary>
        ///// 港股报价
        ///// </summary>
        //HKStockReport = 16069,
        ///// <summary>
        ///// 基金报价
        ///// </summary>
        //FundStockReport = 16070,
        ///// <summary>
        ///// 债券报价
        ///// </summary>
        //BondStockReport = 16071,
        ///// <summary>
        ///// 期货报价
        ///// </summary>
        //FuturesStockReport = 16072,
        ///// <summary>
        ///// 全球指数
        ///// </summary>
        //GlobalIndexReport = 16073,
        ///// <summary>
        ///// 利率
        ///// </summary>
        //RateReport = 16074,
        ///// <summary>
        ///// 理财
        ///// </summary>
        //FinanceReport = 16075,
        ///// <summary>
        ///// 股指期货
        ///// </summary>
        //IndexFuturesReport = 16076,
        ///// <summary>
        ///// 东财指数
        ///// </summary>
        //EmIndexReport = 16077,
        ///// <summary>
        ///// 新期货报价
        ///// </summary>
        //OSFuturesReportNew = 16087,
        ///// <summary>
        ///// 自选股资金流向
        ///// </summary>
        //CustomCapitalFlowReport = 16091,
        ///// <summary>
        ///// 自选股财务数据
        ///// </summary>
        //CustomFinanceStockReport = 16092,
        ///// <summary>
        ///// 自选股盈利预测
        ///// </summary>
        //CustomProfitForecastReport = 16093,
        ///// <summary>
        ///// 自选股DDE
        ///// </summary>
        //CustomDDEReport = 16094,
        ///// <summary>
        ///// 自选股增仓排名
        ///// </summary>
        //CustomNetInFlowReport = 16095,
        ///// <summary>
        ///// 自选股综合排名
        ///// </summary>
        //CustomRank = 16096,
        ///// <summary>
        ///// 外汇报价
        ///// </summary>
        //ForexReport = 16097,
        ///// <summary>
        ///// 美股报价
        ///// </summary>
        //USStockReport = 16098,
        ///// <summary>
        ///// 国外期货报价
        ///// </summary>
        //OSFuturesReport = 16099,
        ///// <summary>
        ///// 资金流向排名
        ///// </summary>
        //NetInFlowRank = 16100,
        ///// <summary>
        ///// LME报价
        ///// </summary>
        //OsFuturesLMEReport = 16102,
        ///// <summary>
        ///// 国债综合屏公开市场操作列表
        ///// </summary>  
        //BondPublicOpeartion = 16103,
        ///// <summary>
        ///// 当日K线(高频)
        ///// </summary>
        //MinKLineOrg = 17059,
        ///// <summary>
        ///// 历史K线
        ///// </summary>
        //HisKLineOrg = 17060,
        ///// <summary>
        ///// 当日K线(低频)
        ///// </summary>
        //MinKLineOrgDP = 17061,
        ///// <summary>
        ///// 深度资料
        ///// </summary>
        //DepthAnalyse = 17071,
        ///// <summary>
        ///// 东财指数F5盘口
        ///// </summary>
        //EMIndexDetail = 17072,
        ///// <summary>
        ///// 指数静态数据
        ///// </summary>
        //IndexStatic = 17073,
        ///// <summary>
        ///// 外汇Detail
        ///// </summary>
        //ForexDetail = 17074,
        ///// <summary>
        ///// 美股Detail
        ///// </summary>
        //USStockDetail = 17075,
        ///// <summary>
        ///// 海外期货
        ///// </summary>
        //OSFuturesDetail = 17076,
        ///// <summary>
        ///// 可转债券
        ///// </summary>
        //ConvertBondDetail = 17077,
        ///// <summary>
        ///// 非可转债券
        ///// </summary>
        //NonConvertBondDetail = 17078,
        ///// <summary>
        ///// LME盘口
        ///// </summary>
        //OSFuturesLMEDetail = 17079,
        ///// <summary>
        ///// 分时走势（高频）
        ///// </summary>
        //TrendOrg = 17080,
        ///// <summary>
        ///// 分时走势(低频)
        ///// </summary>
        //TrendOrgDP = 17081,
        ///// <summary>
        ///// 个股盈利预测接口(高频)
        ///// </summary>
        //NewProfitForcast = 17085,
        ///// <summary>
        ///// LME成交明细
        ///// </summary>
        //OSFuturesLMEDeal = 17183,
        ///// <summary>
        ///// 货币式基金盘口
        ///// </summary>
        //MonetaryFundDetail = 19001,
        ///// <summary>
        ///// 非货币式基金盘口
        ///// </summary>
        //NonMonetaryFundDetail,
        ///// <summary>
        ///// 重仓持股
        ///// </summary>
        //FundHeaveStockReport,
        ///// <summary>
        ///// 重仓行业
        ///// </summary>
        //FundHYReport,
        ///// <summary>
        ///// 重仓债券
        ///// </summary>
        //FundKeyBondReport,
        ///// <summary>
        ///// 基金经理
        ///// </summary>
        //FundManager,
        ///// <summary>
        ///// 基金净值后复权
        ///// </summary>
        //FundKlineAfterDivide = 19008,
        ///// <summary>
        ///// 券商集合理财(货币式)
        ///// </summary>
        //FundCIPMonetaryDetail = 19011,
        ///// <summary>
        ///// 券商集合理财(非货币式)
        ///// </summary>
        //FundCIPNonMonetaryDetail,
        ///// <summary>
        ///// 理财重仓持股
        ///// </summary>
        //FinanceHeaveStockReport,
        ///// <summary>
        ///// 理财重仓行业
        ///// </summary>
        //FinanceHeaveHYReport,
        ///// <summary>
        ///// 理财重仓债券
        ///// </summary>
        //FinanceHeaveBondReport,
        ///// <summary>
        ///// 理财重仓基金
        ///// </summary>
        //FinanceHeaveFundReport,
        ///// <summary>
        ///// 理财投资经理
        ///// </summary>
        //FinanceHeaveManagerReport,
        ///// <summary>
        ///// 信托理财和阳光私募
        ///// </summary>
        //FundTrpAndSunDetail = 19019,
        ///// <summary>
        ///// 银行理财
        ///// </summary>
        //FundBFPDetail = 19023,
        ///// <summary>
        ///// 巨潮指数
        ///// </summary>
        //CNIndexDetail = 19024,
        ///// <summary>
        ///// 中债指数
        ///// </summary>
        //CSIndexDetail = 19025,
        ///// <summary>
        ///// 中证指数
        ///// </summary>
        //CSIIndexDetail = 19026,
        ///// <summary>
        ///// 全球指数
        ///// </summary>
        //GLOBALIndexDetail = 19027,
        ///// <summary>
        ///// 银行间债券detail
        ///// </summary>
        //InterBankDetail = 19030,
        ///// <summary>
        ///// 利率互换
        ///// </summary>
        //RateSwapDetail = 19035,
        ///// <summary>
        ///// 银行间拆借
        ///// </summary>
        //InterBankRepurchaseDetail = 19032,
        ///// <summary>
        ///// shibor
        ///// </summary>
        //ShiborDetail = 19033,
        ///// <summary>
        ///// 低频分笔交易(Low Frequency Trade BY Trade)
        ///// </summary>
        //LowFrequencyTBY = 19083,
        ///// <summary>
        ///// 银行间债券报价明细
        ///// </summary>
        //BankBondReport = 19085,
        ///// <summary>
        ///// SHIBOR报价行明细
        ///// </summary>
        //ShiborReport = 19086
    }

    /// <summary>
    /// 资讯机构服务器请求ID
    /// </summary>
    public enum FuncTypeInfoOrg
    {
        /// <summary>
        /// 资讯列表
        /// </summary>
        InfoMineOrg = 1,
        /// <summary>
        /// 盈利预测
        /// </summary>
        ProfitForecast = 2,
        /// <summary>
        /// 机构评级
        /// </summary>
        OrgRate = 3,
        /// <summary>
        /// 24小时新闻
        /// </summary>
        News24H = 4,
        /// <summary>
        /// 要闻精华
        /// </summary>
        ImportantNews = 5,
        /// <summary>
        /// 公司快讯
        /// </summary>
        NewsFlash = 6,
        ///// <summary>
        ///// 提示
        ///// </summary>
        //Notice = 7,
        /// <summary>
        /// 研究报告
        /// </summary>
        ResearchReport = 7,
        /// <summary>
        /// 新资讯列表请求
        /// </summary>
        NewInfoMineOrg = 8,
        /// <summary>
        /// 9.	根据列表id取资讯请求
        /// </summary>
        InfoMineOrgByIds = 9
    }

    /// <summary>
    /// 请求包中的市场类型
    /// </summary>
    public enum ReqMarketType
    {
        /// <summary>
        /// 深证,三板
        /// </summary>
        MT_SZ = 0,
        /// <summary>
        /// 上证
        /// </summary>
        MT_SH = 1,
        /// <summary>
        /// 创业板
        /// </summary>
        MT_CY = 2,
        /// <summary>
        /// A+H
        /// </summary>
        MT_AH = 3,
        /// <summary>
        /// 三板
        /// </summary>
        MT_OC = 4,
        /// <summary>
        /// 外汇
        /// </summary>
        MT_Forex = 5,
        /// <summary>
        /// 港股
        /// </summary>
        MT_HK = 6,
        /// <summary>
        /// 期货，全球指数
        /// </summary>
        MT_Futures_GlobalIndex = 7,
        /// <summary>
        /// 股指期货
        /// </summary>
        MT_IndexFutures = 8,
        /// <summary>
        /// 国外期货
        /// </summary>
        MT_FUND = 9,
        /// <summary>
        /// 上期所
        /// </summary>
        MT_SHF = 21,
        /// <summary>
        /// 大商所
        /// </summary>
        MT_DCE = 22,
        /// <summary>
        /// 郑商所
        /// </summary>
        MT_CZC = 23,
        /// <summary>
        /// 理财
        /// </summary>
        MT_LC = 51,
        /// <summary>
        /// 银行间
        /// </summary>
        MT_BOND_IB = 60,
        /// <summary>
        /// 柜台交易债
        /// </summary>
        MT_BOND_BC = 61,
        /// <summary>
        /// 
        /// </summary>
        MT_BOND_00 = 62,
        /// <summary>
        /// 板块
        /// </summary>
        MT_Plate = 90,
        /// <summary>
        /// 东财指数
        /// </summary>
        MT_EMINDEX = 91,
        /// <summary>
        /// 
        /// </summary>
        MT_NA = 99

    }

    /// <summary>
    /// 请求K线的数据范围
    /// </summary>
    public enum ReqKLineDataRange
    {
        /// <summary>
        /// 所有K线数据
        /// </summary>
        All = 0x00,
        /// <summary>
        /// 从StartDate到现在的数据
        /// </summary>
        StartDateToNow,
        /// <summary>
        /// 从今天向前N个数据
        /// </summary>
        SizeToNow,
        /// <summary>
        /// 从StartDate到EndDate的数据
        /// </summary>
        StartDateToEndDate,
        /// <summary>
        /// 从StartDate向后N个数据
        /// </summary>
        StartDateToSize,
        /// <summary>
        /// 从EndDate向前N个数据
        /// </summary>
        SizeToEndDate,
        //  RecentNDay//最近N个交易日数据
    }

    /// <summary>
    /// 逐笔成交请求方式
    /// </summary>
    public enum ReqTickFlag
    {
        /// <summary>
        /// 最新的num条记录
        /// </summary>
        TypeTradeNoLast,

        /// <summary>
        /// 此记录后的num条记录
        /// </summary>
        TypeTradeNoBack,

        /// <summary>
        /// 此记录前的num条记录
        /// </summary>
        TypeTradeNoForward,

        /// <summary>
        /// 最新的num条记录
        /// </summary>
        TypeIndexLast,

        /// <summary>
        /// index后的num条记录
        /// </summary>
        TypeIndexBack,

        /// <summary>
        /// index前的num条记录
        /// </summary>
        TypeIndexForward,

    }

    /// <summary>
    /// 信息地雷请求类型
    /// </summary>
    [Flags]
    public enum InfoMine
    {
        /// <summary>
        /// 新闻
        /// </summary>
        News = 1,

        /// <summary>
        /// 公告
        /// </summary>
        Notice = 2,

        /// <summary>
        /// 提示
        /// </summary>
        Prompt = 4,

        /// <summary>
        /// 法律
        /// </summary>
        Law = 8,

        /// <summary>
        /// 研报
        /// </summary>
        Report = 16,

        /// <summary>
        /// 重要提示
        /// </summary>
        ImportPrompt = 32,

        /// <summary>
        /// 指数新闻
        /// </summary>
        IndexNews = 64,

        /// <summary>
        /// 异动点评
        /// </summary>
        SmartShort = 128


    }

    /// <summary>
    /// 机构版资讯列表
    /// </summary>
    public enum InfoMineOrg
    {
        /// <summary>
        /// 新闻
        /// </summary>
        News = 1,
        /// <summary>
        /// 公告
        /// </summary>
        Notice = 2,
        /// <summary>
        /// 研报
        /// </summary>
        Report = 3,
        /// <summary>
        /// 公告和研报
        /// </summary>
        NoticeAndReport = 5,
        /// <summary>
        /// 新闻和公告
        /// </summary>
        NewsAndNotic = 6,
        /// <summary>
        /// 提示和新闻
        /// </summary>
        NewsAndTip = 7,
        /// <summary>
        /// 提示
        /// </summary>
        Tip = 11,
    }

    /// <summary>
    /// 请求的咨询类型：OldInfo 旧 ；NewInfo 新
    /// </summary>
    public enum ClassType : byte
    {
        /// <summary>
        /// 原始咨询类型
        /// </summary>
        OldInfo = 1,
        /// <summary>
        /// 新的咨询类型：左边个股，右边行情
        /// </summary>
        NewInfo = 2
    }

    /// <summary>
    /// 评级
    /// </summary>
    public enum EmratingValue
    {
        /// <summary>
        /// 卖出
        /// </summary>
        Sell = -3,
        /// <summary>
        /// 减持
        /// </summary>
        Reduce = -2,
        /// <summary>
        /// 回避
        /// </summary>
        AVoid = -1,
        /// <summary>
        /// 中性
        /// </summary>
        Neutur = 0,
        /// <summary>
        /// 持有
        /// </summary>
        Hold = 1,
        /// <summary>
        /// 增持
        /// </summary>
        Add = 2,
        /// <summary>
        /// 买入
        /// </summary>
        Buy = 3,

    }

    /// <summary>
    /// 机构版服务器初始化状态
    /// </summary>
    public enum InitOrgStatus
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = 0,
        /// <summary>
        /// 沪深
        /// </summary>
        SHSZ = 10,
        /// <summary>
        /// 沪深静态数据
        /// </summary>
        StaticData = 11,
        /// <summary>
        /// 港股
        /// </summary>
        HK = 20,
        /// <summary>
        /// 银行间
        /// </summary>
        IB = 30,
        /// <summary>
        /// 美股
        /// </summary>
        US = 40,
        /// <summary>
        /// 外盘期货(废除)
        /// </summary>
        OSF = 50,
        /// <summary>
        /// 外汇
        /// </summary>
        WH = 60,
        /// <summary>
        /// 纽约商品交易所
        /// </summary>
        OSF_CMX_NMX = 70,
        /// <summary>
        /// 芝加哥商品交易所
        /// </summary>
        OSF_CBT = 80,
        /// <summary>
        /// 新加坡商品交易所
        /// </summary>
        OSF_SGX = 90,
        /// <summary>
        /// LME电子综合盘
        /// </summary>
        LME_ELEC = 100,
        /// <summary>
        /// LME 场内、现货
        /// </summary>
        LME_FLOOR = 110,
        /// <summary>
        /// 上期所黄金白银
        /// </summary>
        CF_AUAG = 120,

    }
    #endregion

    #region 包结构
    /// <summary>
    /// 数据包的基类
    /// </summary>
    public class DataPacket
    {
        [DllImport("Dozlib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int doGeneralCompress([MarshalAs(UnmanagedType.LPArray)] byte[] dest, ref int destLen, [MarshalAs(UnmanagedType.LPArray)] byte[] source, int sourceLen);
        [DllImport("Dozlib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int doGeneralUncompress([MarshalAs(UnmanagedType.LPArray)] byte[] dest, ref int destLen, [MarshalAs(UnmanagedType.LPArray)] byte[] source, int sourceLen);
        [DllImport("Dozlib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int doStaticUncompressNoHeader([MarshalAs(UnmanagedType.LPArray)] byte[] dest, ref long destLen, [MarshalAs(UnmanagedType.LPArray)] byte[] source, long sourceLen);
        [DllImport("dlcm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeInstance(IntPtr pIf);
        [DllImport("dlcm.dll")]
        public static extern IntPtr CreateInstance();
        [DllImport("dlcm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UncompressOrderDetail(IntPtr pIf, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, [MarshalAs(UnmanagedType.LPArray)] byte[] pNew, [MarshalAs(UnmanagedType.LPArray)] int[] pSize);
        [DllImport("dlcm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UncompressPriceOrderQueue(IntPtr pIf, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, [MarshalAs(UnmanagedType.LPArray)] byte[] pNew, [MarshalAs(UnmanagedType.LPArray)] int[] pSize);
        public static int TrendCtrlId = 0x3e9;
        private int _msgid;
        /// <summary>
        /// 消息序号
        /// </summary>
        public int MsgId
        {
            get { return _msgid; }
            set { this._msgid = value; }
        }

        private int _len;
        /// <summary>
        /// 消息长度
        /// </summary>
        public int Len
        {
            get { return _len; }
            set { this._len = value; }
        }

        /// <summary>
        /// 消息序号个数
        /// </summary>
        protected static int MsgIdCount = 1;

        private bool _isZip;
        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsZip
        {
            get { return _isZip; }
            protected set { _isZip = value; }
        }

        private bool _isResult;
        /// <summary>
        /// 返回包是否正常
        /// </summary>
        public bool IsResult
        {
            get { return _isResult; }
            protected set { this._isResult = value; }
        }

        private bool _isEncrypt;
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypt
        {
            get { return _isEncrypt; }
            protected set { this._isEncrypt = value; }
        }

        private bool _isPush;
        /// <summary>
        /// 是否推送，放在基类里，用于取消订阅
        /// </summary>
        public virtual bool IsPush
        {
            get { return _isPush; }
            set { this._isPush = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DataPacket()
        {
            if (MsgIdCount >= int.MaxValue)
                MsgIdCount = 0;
            MsgId = MsgIdCount++;
        }

        /// <summary>
        /// 组包
        /// </summary>
        /// <param name="bw"></param>
        public virtual void Coding(BinaryWriter bw) { }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public virtual bool Decoding(BinaryReader br) { return false; }

        /// <summary>
        /// 回填包头中包长的参数
        /// </summary>
        /// <param name="bw"></param>
        /// <returns></returns>
        public virtual int CodePacket(BinaryWriter bw)
        {
            Coding(bw);
            return Len;
        }

        /// <summary>
        /// 把Code解析成请求包用的市场和代码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="market"></param>
        /// <param name="shortCode"></param>
        public static void ParseCode(string code, out ReqMarketType market, out string shortCode)
        {
            ReqMarketType type = ReqMarketType.MT_NA;
            shortCode = string.Empty;
            if (shortCode.StartsWith("BK"))
            {
                market = ReqMarketType.MT_Plate;
                shortCode = code;
            }
            else
            {
                string[] strArray = code.Split(new char[] { '.' });
                shortCode = strArray[0];
                if (strArray.Length >= 2)
                {
                    switch (strArray[1])
                    {
                        case "SH":
                            type = ReqMarketType.MT_SH;
                            break;

                        case "SZ":
                        case "OC":
                            type = ReqMarketType.MT_SZ;
                            break;

                        case "IF":
                        case "CFE":
                            type = ReqMarketType.MT_IndexFutures;
                            break;

                        case "HK":
                            type = ReqMarketType.MT_HK;
                            break;

                        case "EI":
                            type = ReqMarketType.MT_EMINDEX;
                            break;

                        case "SHF":
                        case "DCE":
                            type = ReqMarketType.MT_Futures_GlobalIndex;
                            shortCode = shortCode.ToLower();
                            break;

                        case "CZC":
                            type = ReqMarketType.MT_Futures_GlobalIndex;
                            shortCode = shortCode.ToUpper();
                            break;

                        case "GI":
                        case "HI":
                        case "TW":
                            if (SecurityAttribute.DicGlobalIndexOrgToCft.ContainsKey(shortCode))
                            {
                                shortCode = SecurityAttribute.DicGlobalIndexOrgToCft[shortCode];
                            }
                            type = ReqMarketType.MT_Futures_GlobalIndex;
                            break;
                    }
                }
                switch (shortCode.Length)
                {
                    case 3:
                        shortCode = string.Concat(new object[] { shortCode, '\0', '\0', '\0', '\0' });
                        break;

                    case 4:
                        shortCode = string.Concat(new object[] { shortCode, '\0', '\0', '\0' });
                        break;

                    case 5:
                        shortCode = shortCode + '\0' + '\0';
                        break;

                    default:
                        shortCode = shortCode + '\0';
                        break;
                }
                market = type;
            }
        }

        /// <summary>
        /// 根据市场代码，得到客户端用的代码格式
        /// </summary>
        /// <param name="market"></param>
        /// <param name="shortCode"></param>
        /// <returns></returns>
        public static string GetEmCode(ReqMarketType market, string shortCode)
        {
            string strMarket;

            switch (market)
            {
                case ReqMarketType.MT_SH: strMarket = "SH"; break;
                case ReqMarketType.MT_SZ:
                    if (shortCode.StartsWith("4"))
                        strMarket = "OC";
                    else strMarket = "SZ";
                    break;
                case ReqMarketType.MT_HK: strMarket = "HK"; break;
                case ReqMarketType.MT_IndexFutures: strMarket = "CFE"; break;
                case ReqMarketType.MT_Plate: strMarket = "BK"; break;
                case ReqMarketType.MT_EMINDEX: strMarket = "EI"; break;
                case ReqMarketType.MT_DCE: strMarket = "DCE"; break;
                case ReqMarketType.MT_SHF: strMarket = "SHF"; break;
                case ReqMarketType.MT_CZC: strMarket = "CZC"; break;
                case ReqMarketType.MT_Futures_GlobalIndex:
                    if (SecurityAttribute.FuturesCode.ContainsKey(shortCode.ToUpper()))
                        return SecurityAttribute.FuturesCode[shortCode.ToUpper()];
                    if (SecurityAttribute.DicGlobalIndexCftToOrg.ContainsKey(shortCode))
                        return SecurityAttribute.DicGlobalIndexCftToOrg[shortCode];
                    return null;
                case ReqMarketType.MT_FUND: strMarket = "OF";
                    break;
                case ReqMarketType.MT_BOND_IB: strMarket = "IB";
                    break;
                case ReqMarketType.MT_BOND_BC: strMarket = "BC";
                    break;
                case ReqMarketType.MT_LC:
                    return shortCode.ToUpper();
                case ReqMarketType.MT_OC:
                    strMarket = "OC";
                    break;
                default:
                    return null;
            }
            if (strMarket == "BK")
                return shortCode;
            return string.Format("{0}.{1}", shortCode.ToUpper(), strMarket);
        }

        /// <summary>
        /// C++的time转成C#的DateTime
        /// </summary>
        /// <param name="cTime"></param>
        /// <returns></returns>
        public static int CTimeToDateTimeInt(int cTime)
        {
            double dTime = Convert.ToDouble(cTime + 28800);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(dTime);
            string strTime = dt.ToString("HHmmss");
            return Convert.ToInt32(strTime);
        }

        /// <summary>
        /// 1312051105格式转成 日期20131205 时间11:05:00
        /// </summary>
        /// <param name="tempTime"></param>
        /// <param name="resultTime"></param>
        /// <param name="resultDate"></param>
        public static void TrendCaptialFlowTimeConverter(int tempTime, out int resultTime, out int resultDate)
        {
            resultDate = 20000000 + 10000 * (tempTime / 100000000) + 100 * ((tempTime % 100000000) / 1000000) +
                         ((tempTime % 100000000) % 1000000) / 10000;
            resultTime = 10000 * ((tempTime % 10000) / 100) + 100 * ((tempTime % 10000) % 100);
        }


        /// <summary>
        /// 股指期货开平性质
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="openInterest"></param>
        /// <param name="flagBS"></param>
        /// <returns></returns>
        public static string GetIFOpenCloseStatus(long volume, long openInterest, int flagBS)
        {
            string result = string.Empty;
            long OpenVolume = (volume + openInterest) / 2;
            long CloseVolume = (volume - openInterest) / 2;
            if (flagBS == 1)//卖
            {
                if (OpenVolume == CloseVolume)
                    result = "空换";
                else if (OpenVolume == 0)
                    result = "双平";
                else if (CloseVolume == 0)
                    result = "双开";
                else if (OpenVolume > CloseVolume)
                    result = "空开";
                else if (OpenVolume < CloseVolume)
                    result = "空平";
            }
            else
            {
                if (OpenVolume == CloseVolume)
                    result = "多换";
                else if (OpenVolume == 0)
                    result = "双平";
                else if (CloseVolume == 0)
                    result = "双开";
                else if (OpenVolume > CloseVolume)
                    result = "多开";
                else if (OpenVolume < CloseVolume)
                    result = "多平";
            }
            return result;
        }

    }

    /// <summary>
    /// 实时行情，历史行情服务器对应的包的基类
    /// </summary>
    public class RealTimeDataPacket : DataPacket
    {
        public bool IsGetOnceNoPush;

        /// <summary>
        /// 请求ID
        /// </summary>
        public FuncTypeRealTime RequestType;

        /// <summary>
        /// 处理返回消息的窗口指针
        /// </summary>
        protected static int OwnerId = 0;


        /// <summary>
        /// 包的首部长度
        /// </summary>
        public static short PacketHeaderLength = 14;


        /// <summary>
        /// 包转流
        /// </summary>
        /// <param name="bw"></param>
        public override void Coding(BinaryWriter bw)
        {
            //head
            bw.Write(Len);
            bw.Write((short)RequestType);
            bw.Write(OwnerId);
            bw.Write(MsgId);
        }


        public override bool Decoding(BinaryReader br)
        {
            base.Len = br.ReadInt32();
            this.RequestType = (FuncTypeRealTime)br.ReadInt16();
            base.MsgId = br.ReadInt32();
            byte num = br.ReadByte();
            base.IsResult = num == 1;
            base.IsEncrypt = br.ReadBoolean();
            base.IsZip = br.ReadBoolean();
            OwnerId = br.ReadInt32();
            if (!base.IsResult)
            {
                short num5 = br.ReadInt16();
                int count = base.Len - 2;
                Encoding.Default.GetString(br.ReadBytes(count));
                if (num5 == 0x24)
                {
                    //LogUtilities.LogMessagePublishException("财富通服务器初始化！");
                    this.RequestType = FuncTypeRealTime.Init;
                    return true;
                }
                return false;
            }
            if (base.IsZip)
            {
                int num2 = br.ReadInt32();
                int num3 = br.ReadInt32();
                byte[] source = br.ReadBytes(num3);
                byte[] dest = new byte[num2];
                if (this.RequestType == FuncTypeRealTime.ReqF10)
                {
                    long destLen = Convert.ToInt64(num2);
                    if (DataPacket.doStaticUncompressNoHeader(dest, ref destLen, source, (long)num3) != 0)
                    {
                        goto Label_0183;
                    }
                    MemoryStream input = null;
                    try
                    {
                        input = new MemoryStream(dest);
                        using (BinaryReader reader = new BinaryReader(input))
                        {
                            return this.DecodingBody(reader);
                        }
                    }
                    catch (Exception exception)
                    {
                        LogUtilities.LogMessage(exception.StackTrace);
                        goto Label_0183;
                    }
                    finally
                    {
                        if (input != null)
                        {
                            input.Dispose();
                        }
                    }
                }
                if (DataPacket.doGeneralUncompress(dest, ref num2, source, num3) == 0)
                {
                    MemoryStream stream2 = null;
                    try
                    {
                        stream2 = new MemoryStream(dest);
                        using (BinaryReader reader2 = new BinaryReader(stream2))
                        {
                            return this.DecodingBody(reader2);
                        }
                    }
                    catch (Exception exception2)
                    {
                        LogUtilities.LogMessage(exception2.Message);
                        goto Label_0183;
                    }
                    finally
                    {
                        if (stream2 != null)
                        {
                            stream2.Dispose();
                        }
                    }
                }
                LogUtilities.LogMessage(this.RequestType.ToString() + "解压出错");
                return false;
            }
        Label_0183:
            return this.DecodingBody(br);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool DecodingBody(BinaryReader br)
        {
            return false;
        }

        /// <summary>
        /// 组包，cm层调用
        /// </summary>
        /// <param name="bw"></param>
        /// <returns></returns>
        public override int CodePacket(BinaryWriter bw)
        {
            base.CodePacket(bw);
            //修改包头中，length的值
            int len = Convert.ToInt32(bw.BaseStream.Length);
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(len - PacketHeaderLength);
            Len = len - PacketHeaderLength;
            return Convert.ToInt32(bw.BaseStream.Length);
        }


        /// <summary>
        /// 解包，cm层调用
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static RealTimeDataPacket DecodePacket(byte[] bytes, int len)
        {
            RealTimeDataPacket dataPacket = null;
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                ms.Write(bytes, 0, len);
                using (BinaryReader br = new BinaryReader(ms))
                {
                    //br.BaseStream.Position = 0;
                    //int lenBody = br.ReadInt32();
                    //FuncTypeRealTime funcType = (FuncTypeRealTime)br.ReadInt16();
                    //br.BaseStream.Position = 0;
                    //if (funcType == FuncTypeRealTime.AllOrderStockDetailLevel2)
                    //{
                    //   // if (funcType == FuncTypeRealTime.AllOrderStockDetailLevel2)
                    //   // {
                    //        using (FileStream fs = new FileStream("d://abc.txt", FileMode.Create))
                    //        {
                    //            using (BinaryWriter bw = new BinaryWriter(fs))
                    //            {
                    //                bw.Write(bytes, 0, len);
                    //            }
                    //        }
                    //   // }
                    //}
                    dataPacket = DecodePacket(br);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
                throw;
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    ms.Write(bytes, 0, len);
            //    using (BinaryReader br = new BinaryReader(ms))
            //    {
            //        dataPacket = DecodePacket(br);
            //    }
            //}
            return dataPacket;
        }

        /// <summary>
        /// 确定是哪个包，然后调用对应的Decoding
        /// </summary> 
        /// <param name="br"></param>
        /// <returns></returns>
        private static RealTimeDataPacket DecodePacket(BinaryReader br)
        {
            RealTimeDataPacket dataPacket = null;
            br.BaseStream.Position = 0;

            int len = br.ReadInt32();
            FuncTypeRealTime funcType = (FuncTypeRealTime)br.ReadInt16();
            //LogUtilities.LogMessage("收到数据包--" + FuncTypeRealTime.ToString());

            switch (funcType)
            {
                case FuncTypeRealTime.Heart:
                    dataPacket = new ResHeartDataPacket();
                    break;
                case FuncTypeRealTime.StockDict:
                    //dataPacket = new ResStockDictDataPacket();
                    break;
                case FuncTypeRealTime.StockDetail:
                    dataPacket = new ResStockDetailDataPacket();
                    break;
                case FuncTypeRealTime.StockDetailLev2:
                    dataPacket = new ResStockDetailLev2DataPacket();
                    break;
                case FuncTypeRealTime.IndexDetail:
                    dataPacket = new ResIndexDetailDataPacket();
                    break;
                case FuncTypeRealTime.StockTrend:
                    dataPacket = new ResStockTrendDataPacket();
                    break;
                case FuncTypeRealTime.HisKLine:
                    dataPacket = new ResHisKLineDataPacket();
                    break;
                case FuncTypeRealTime.MinKLine:
                    dataPacket = new ResMinKLineDataPacket();
                    break;
                case FuncTypeRealTime.BlockOverViewList:
                    dataPacket = new ResBlockOverViewDataPacket();
                    break;
                case FuncTypeRealTime.BlockSimpleQuote:
                    dataPacket = new ResBlockSimpleQuoteDataPacket();
                    break;
                case FuncTypeRealTime.SectorQuoteReport:
                    dataPacket = new ResSectorQuoteReportDataPacket();
                    ((ResSectorQuoteReportDataPacket)dataPacket).IsBlock = true;
                    break;
                case FuncTypeRealTime.BlockQuoteReport:
                    dataPacket = new ResSectorQuoteReportDataPacket();
                    ((ResSectorQuoteReportDataPacket)dataPacket).IsBlock = true;
                    break;
                case FuncTypeRealTime.BlockIndexReport:
                    dataPacket = new ResBlockIndexReportDataPacket();
                    break;
                case FuncTypeRealTime.DealSubscribe:
                    dataPacket = new ResDealSubscribeDataPacket();
                    break;
                case FuncTypeRealTime.DealRequest:
                    dataPacket = new ResDealRequestDataPacket();
                    break;
                case FuncTypeRealTime.CapitalFlow:
                    dataPacket = new ResCapitalFlowDataPacket();
                    break;
                case FuncTypeRealTime.PriceStatus:
                    dataPacket = new ResPriceStatusDataPacket();
                    break;
                case FuncTypeRealTime.Rank:
                    dataPacket = new ResRankDataPacket();
                    break;
                case FuncTypeRealTime.ReqF10:
                    dataPacket = new ResF10DataPacket();
                    break;
                case FuncTypeRealTime.TickTrade:
                    dataPacket = new ResTickDataPacket();
                    break;
                case FuncTypeRealTime.OrderDetail:
                    dataPacket = new ResOrderDetailDataPacket();
                    break;
                case FuncTypeRealTime.StockTrendPush:
                    dataPacket = new ResStockTrendPushDataPacket();
                    break;
                case FuncTypeRealTime.IndexFuturesTrend:
                    dataPacket = new ResIndexFuturesTrendDataPacket();
                    break;
                case FuncTypeRealTime.StockTrendAskBid:
                    dataPacket = new ResTrendAskBidDataPacket();
                    break;
                case FuncTypeRealTime.StockTrendInOutDiff:
                    dataPacket = new ResTrendInOutDiffDataPacket();
                    break;
                case FuncTypeRealTime.HisTrend:
                    dataPacket = new ResHisTrendDataPacket();
                    break;
                case FuncTypeRealTime.RedGreen:
                    dataPacket = new ResRedGreenDataPacket();
                    break;
                case FuncTypeRealTime.OrderQueue:
                    dataPacket = new ResOrderQueueDataPacket();
                    break;
                case FuncTypeRealTime.ShortLineStrategy:
                    dataPacket = new ResShortLineStragedytDataPacket();
                    break;
                case FuncTypeRealTime.LimitedPrice:
                    dataPacket = new ReqLimitedPriceDataPacket();
                    break;
                case FuncTypeRealTime.ContributionStock:
                    dataPacket = new ResContributionStockDataPacket();
                    break;
                case FuncTypeRealTime.ContributionBlock:
                    dataPacket = new ResContributionBlockDataPacket();
                    break;
                case FuncTypeRealTime.IndexFuturesDetail:
                    dataPacket = new ResIndexFuturesDetailDataPacket();
                    break;
                case FuncTypeRealTime.InitLogon:
                    dataPacket = new ResLogonCftDataPacket();
                    break;
                case FuncTypeRealTime.OceanHeart:
                    dataPacket = new ResOceanHeart();
                    break;
                case FuncTypeRealTime.OceanRecord:
                    dataPacket = new ResOceanRecordDataPacket();
                    break;
                case FuncTypeRealTime.OceanTrend:
                    dataPacket = new ResOceanTrendDataPacket();
                    break;
                case FuncTypeRealTime.NOrderStockDetailLevel2:
                    dataPacket = new ResNOrderStockDetailLev2DataPacket();
                    break;
                case FuncTypeRealTime.AllOrderStockDetailLevel2:
                    dataPacket = new ResAllOrderStockDetailLev2DataPacket();
                    break;
                case FuncTypeRealTime.StockDetailOrderQueue:
                    dataPacket = new ResStockDetailLev2OrderQueueDataPacket();
                    break;
                case FuncTypeRealTime.TrendCapitalFlow:
                    dataPacket = new ResTrendCaptialFlowDataPacket();
                    break;

                case FuncTypeRealTime.CapitalFlowDay:
                    dataPacket = new ResCapitalFlowDayDataPacket();
                    break;
                case FuncTypeRealTime.HisTrendlinecfs:
                    dataPacket = new ResHisTrendlinecfsDataPacket();
                    break;

                default:
                    LogUtilities.LogMessage("收到未知数据包,FuncType等于" + funcType.ToString());
                    break;
            }
            br.BaseStream.Position = 0;


            if (dataPacket != null)
            {
                try
                {
                    dataPacket.Decoding(br);
                }
                catch (Exception ex)
                {
                    LogUtilities.LogMessage(funcType.ToString() + ex.Message);
                }

            }

            return dataPacket;
        }


    }

    /// <summary>
    /// 资讯服务器的包
    /// </summary>
    public class InfoDataPacket : DataPacket
    {
        byte[] _workName = new byte[32];

        private static int CommonHeaderLen = 14;
        private static int BodyXmlLen = 40;
        private static int MQHeadLen = 16;
        private short _infoMsg = 4170;

        private bool _isInfoHeartPacket;

        /// <summary>
        /// 是不是InfoHeart包
        /// </summary>
        public bool IsInfoHeartPacket
        {
            get { return _isInfoHeartPacket; }
            set
            {
                if (value)
                    _infoMsg = 4005;//心跳
                else
                    _infoMsg = 4170;
            }
        }

        private FuncTypeInfo _requestType;
        /// <summary>
        /// 请求ID
        /// </summary>
        public FuncTypeInfo RequestType
        {
            get { return _requestType; }
            set { this._requestType = value; }
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            bw.Write(Len);//包的大小(消息体大小)
            bw.Write(_infoMsg);//消息编码
            bw.Write(0);
            bw.Write(MsgId);

            if (_isInfoHeartPacket)
                return;

            bw.Write(1);//请求类型
            bw.Write(_workName);//应用名称
            bw.Write(100);//数据长度

            bw.Write((short)1);//消息标记
            bw.Write((short)0);//消息id，废弃，填0
            bw.Write(100);//消息体长度
            bw.Write((byte)RequestType);//消息分类,请求类型
            bw.Write((byte)0);
            bw.Write((short)0);
            bw.Write(0);
        }

        /// <summary>
        /// 组包
        /// </summary>
        public override int CodePacket(BinaryWriter bw)
        {
            base.CodePacket(bw);
            if (_isInfoHeartPacket)
                return Convert.ToInt32(bw.BaseStream.Length);
            //修改包头中，length的值
            int len = Convert.ToInt32(bw.BaseStream.Length);
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(len - CommonHeaderLen);
            bw.Seek(CommonHeaderLen + BodyXmlLen - 4, SeekOrigin.Begin);
            bw.Write(len - CommonHeaderLen - BodyXmlLen);
            bw.Seek(CommonHeaderLen + BodyXmlLen + 4, SeekOrigin.Begin);
            bw.Write(len - CommonHeaderLen - BodyXmlLen - MQHeadLen);
            Len = len - CommonHeaderLen - BodyXmlLen - MQHeadLen;
            return Convert.ToInt32(bw.BaseStream.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool DecodingBody(BinaryReader br)
        {
            return false;
        }

        /// <summary>
        /// 解包，cm层调用
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static InfoDataPacket DecodePacket(byte[] bytes, int len)
        {
            InfoDataPacket dataPacket = null;
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                ms.Write(bytes, 0, len);
                using (BinaryReader br = new BinaryReader(ms))
                {
                    dataPacket = DecodePacket(br);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
                throw;
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    ms.Write(bytes, 0, len);
            //    using (BinaryReader br = new BinaryReader(ms))
            //    {
            //        dataPacket = DecodePacket(br);
            //    }
            //}
            return dataPacket;
        }

        private static InfoDataPacket DecodePacketPartHeader(BinaryReader br)
        {
            //BodyXML
            int type = br.ReadInt32();
            int len2 = br.ReadInt32();

            //MQ_COMMON_HEADER
            ushort flag = br.ReadUInt16();
            ushort noway = br.ReadUInt16();
            int len3 = br.ReadInt32();
            byte id = br.ReadByte();//请求类型
            byte reserved1 = br.ReadByte();
            ushort reserved2 = br.ReadUInt16();
            uint reserved3 = br.ReadUInt32();

            InfoDataPacket packet = null;
            switch ((FuncTypeInfo)id)
            {
                case FuncTypeInfo.Block:
                    //packet = new ResBlockInfoDataPacket();
                    break;
                case FuncTypeInfo.NewsReport:
                    packet = new ResNewsReportDataPacket();
                    break;
                case FuncTypeInfo.Finance:
                    packet = new ResFinanceDataPacket();
                    break;
                case FuncTypeInfo.OrgRate:
                    packet = new ResOrgRateDataPacket();
                    break;
                case FuncTypeInfo.ProfitForecast:
                    packet = new ResProfitForecastDataPacket();
                    break;
                case FuncTypeInfo.DivideRight:
                    packet = new ResDivideRightDataPacket();
                    break;
                case FuncTypeInfo.News24:
                    packet = new ResNews24DataPacket();
                    break;
                case FuncTypeInfo.CustomStockNewsReport:
                    packet = new ResCustomStockNewsDataPacket();
                    break;
            }
            if (packet != null)
                packet.RequestType = (FuncTypeInfo)id;
            return packet;
        }

        /// <summary>
        /// 确定是哪个包，然后调用对应的Decoding
        /// </summary> 
        /// <param name="br"></param>
        /// <returns></returns>
        private static InfoDataPacket DecodePacket(BinaryReader br)
        {
            InfoDataPacket dataPacket = null;
            br.BaseStream.Position = 0;

            int len1 = br.ReadInt32();
            short msgId = br.ReadInt16();
            int owner = br.ReadInt32();
            bool isResult = br.ReadBoolean();
            bool isEncrypt = br.ReadBoolean();
            bool isZip = br.ReadBoolean();
            int msgId1 = br.ReadInt32();
            if (len1 == 0)//心跳
                return new ResInfoHeart();
            if (isResult)
            {
                if (isZip)
                {

                    int originalSize = br.ReadInt32();
                    int compressedSize = br.ReadInt32();
                    byte[] bytesCompressed = br.ReadBytes(compressedSize);
                    byte[] bytesDecompress = new byte[originalSize];


                    {
                        // long originalSizeLong = Convert.ToInt64(originalSize);
                        // int tmp = doStaticUncompressNoHeader(bytesDecompress, ref originalSizeLong, bytesCompressed,
                        //                                      compressedSize);
                        int tmp = doGeneralUncompress(bytesDecompress, ref originalSize, bytesCompressed, compressedSize);
                        if (tmp == 0)
                        {
                            MemoryStream ms = null;
                            try
                            {
                                ms = new MemoryStream(bytesDecompress);
                                using (BinaryReader brr = new BinaryReader(ms))
                                {
                                    dataPacket = DecodePacketPartHeader(brr);
                                    if (dataPacket != null)
                                    {
                                        dataPacket.MsgId = msgId1;
                                        dataPacket.IsResult = true;
                                        dataPacket.IsZip = true;

                                        //                                         FileStream fs = new FileStream("d:\\block.txt",FileMode.Create);
                                        //                                         fs.Write(ms.ToArray(),24,originalSize-24);
                                        //                                         fs.Close();
                                        dataPacket.DecodingBody(brr);
                                    }

                                    return dataPacket;
                                }
                            }
                            catch (Exception e)
                            {
                                LogUtilities.LogMessage(e.Message);
                                throw;
                            }
                            finally
                            {
                                if (ms != null)
                                    ms.Dispose();
                            }
                            //                            using (MemoryStream ms = new MemoryStream(bytesDecompress))
                            //                            {
                            //                                using (BinaryReader brr = new BinaryReader(ms))
                            //                                {
                            //                                    dataPacket = DecodePacketPartHeader(brr);
                            //                                    if (dataPacket != null)
                            //                                    {
                            //                                        dataPacket.MsgId = msgId1;
                            //                                        dataPacket.IsResult = true;
                            //                                        dataPacket.IsZip = true;

                            ////                                         FileStream fs = new FileStream("d:\\block.txt",FileMode.Create);
                            ////                                         fs.Write(ms.ToArray(),24,originalSize-24);
                            ////                                         fs.Close();
                            //                                        dataPacket.DecodingBody(brr);
                            //                                    }

                            //                                    return dataPacket;
                            //                                }
                            //                            }
                        }
                    }

                    /*
                    byte[] deCompressBytes = Decompress(br.ReadBytes(len1));
                    using (MemoryStream ms = new MemoryStream(deCompressBytes))
                    {
                        using (BinaryReader brr = new BinaryReader(ms))
                        {
                            dataPacket = DecodePacketPartHeader(brr);
                            if (dataPacket != null)
                            {
                                dataPacket.MsgId = msgId1;
                                dataPacket.IsResult = true;
                                dataPacket.IsZip = true;
                                dataPacket.DecodingBody(brr);    
                            }
                            
                            return dataPacket;
                        }
                    }
                    */
                }

                dataPacket = DecodePacketPartHeader(br);
                if (dataPacket != null)
                {
                    dataPacket.MsgId = msgId1;
                    dataPacket.IsResult = true;
                    dataPacket.DecodingBody(br);
                }
            }
            else
            {
                short errorId = br.ReadInt16();
                byte[] errorBytes = br.ReadBytes(len1 - 2);
                string errorStr = Encoding.Default.GetString(errorBytes);
                LogUtilities.LogMessage("返回包失败,ErrorId=" + errorId + ",ErrorStr=" + errorStr);
            }
            return dataPacket;
        }

    }


    /// <summary>
    /// 机构行情服务器基类
    /// </summary>
    public class OrgDataPacket : DataPacket
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public FuncTypeOrg RequestType;

        /// <summary>
        /// 处理返回消息的窗口指针
        /// </summary>
        protected static int OwnerId = 0;

        /// <summary>
        /// 包的首部长度
        /// </summary>
        public static short PacketHeaderLength = 14;


        /// <summary>
        /// 包转流
        /// </summary>
        /// <param name="bw"></param>
        public override void Coding(BinaryWriter bw)
        {
            //head
            bw.Write(Len);
            bw.Write((short)RequestType);
            bw.Write(OwnerId);
            bw.Write(MsgId);
        }

        /// <summary>
        /// 流转包
        /// </summary>
        public override bool Decoding(BinaryReader br)
        {
            bool result = false;

            Len = br.ReadInt32();
            RequestType = (FuncTypeOrg)br.ReadInt16();
            OwnerId = br.ReadInt32();
            IsResult = br.ReadBoolean();
            IsEncrypt = br.ReadBoolean();
            IsZip = br.ReadBoolean();
            MsgId = br.ReadInt32();
            if (IsResult)
            {
                if (IsZip)
                {
                    byte[] uc = br.ReadBytes(Len);
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(uc))
                        using (GZipInputStream unzip = new GZipInputStream(ms))
                        using (BinaryReader brDecompress = new BinaryReader(unzip))
                        {
                            try
                            {
                                result = DecodingBody(brDecompress);
                            }
                            catch (Exception e)
                            {
                                LogUtilities.LogMessage("org解压缩出错" + e.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("org解压缩出错" + e.Message);
                    }
                }
                else
                {
                    result = DecodingBody(br);
                }
            }
            else
            {
                Debug.Print("错误包" + RequestType);
                LogUtilities.LogMessage(RequestType.ToString() + "解包出错");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool DecodingBody(BinaryReader br)
        {
            return false;
        }

        /// <summary>
        /// 组包，cm层调用
        /// </summary>
        /// <param name="bw"></param>
        /// <returns></returns>
        public override int CodePacket(BinaryWriter bw)
        {
            base.CodePacket(bw);
            //修改包头中，length的值
            int len = Convert.ToInt32(bw.BaseStream.Length);
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(len - PacketHeaderLength);
            Len = len - PacketHeaderLength;
            return Convert.ToInt32(bw.BaseStream.Length);
        }


        /// <summary>
        /// 解包，cm层调用
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static OrgDataPacket DecodePacket(byte[] bytes, int len)
        {

            OrgDataPacket dataPacket = null;
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                ms.Write(bytes, 0, len);
                using (BinaryReader br = new BinaryReader(ms))
                {
                    dataPacket = DecodePacket(br);
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
                throw;
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    ms.Write(bytes, 0, len);
            //    using (BinaryReader br = new BinaryReader(ms))
            //    {
            //        dataPacket = DecodePacket(br);
            //    }
            //}
            return dataPacket;
        }

        /// <summary>
        /// 确定是哪个包，然后调用对应的Decoding
        /// </summary> 
        /// <param name="br"></param>
        /// <returns></returns>
        private static OrgDataPacket DecodePacket(BinaryReader br)
        {
            OrgDataPacket dataPacket = null;
            br.BaseStream.Position = 0;

            int len = br.ReadInt32();
            FuncTypeOrg funcType = (FuncTypeOrg)br.ReadInt16();
            //LogUtilities.LogMessage("收到数据包--" + FuncTypeRealTime.ToString());

            switch (funcType)
            {
                case FuncTypeOrg.HKTradeDate: dataPacket = new ResHKTradeDateDataPacket();
                    break;
                case FuncTypeOrg.MarketTradeDate: dataPacket = new ReqMarketTradeDateDataPacket();
                    break;
                case FuncTypeOrg.HeartOrg:
                    dataPacket = new ResHeartOrgDataPacket();
                    break;
                case FuncTypeOrg.InitOrg:
                    LogUtilities.LogMessage("***********收到机构版服务端 初始化数据包***********");
                    dataPacket = new ResInitOrgDataPacket();
                    break;
                case FuncTypeOrg.TradeDate:
                    dataPacket = new ResTradeDateDataPacket();
                    break;
                case FuncTypeOrg.BlockReport:
                case FuncTypeOrg.BlockIndexReport:
                case FuncTypeOrg.GlobalIndexReport:
                case FuncTypeOrg.EmIndexReport:
                    dataPacket = new ResBlockReportDataPacket();
                    break;
                case FuncTypeOrg.BlockStockReport:
                    dataPacket = new ResBlockStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.HKStockReport:
                    dataPacket = new ResHKStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.FundStockReport:
                    dataPacket = new ResFundStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.BondStockReport:
                    dataPacket = new ResBondStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.FuturesStockReport:
                    dataPacket = new ResFuturesStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.IndexFuturesReport:
                    dataPacket = new ResIndexFutruesReportOrgDataPacket();
                    break;
                case FuncTypeOrg.OsFuturesLMEReport:
                    dataPacket = new ResOSFuturesLMEReportDataPacket();
                    break;
                case FuncTypeOrg.CustomReport:
                    dataPacket = new ResCustomReportOrgDataPacket();
                    break;
                case FuncTypeOrg.HisKLineOrg:
                    dataPacket = new ResHisKLineOrgDataPacket();
                    break;
                case FuncTypeOrg.MinKLineOrg:
                case FuncTypeOrg.MinKLineOrgDP:
                    dataPacket = new ResMintKLineOrgDataPacket();
                    break;
                case FuncTypeOrg.TrendOrg:
                case FuncTypeOrg.TrendOrgDP:
                    dataPacket = new ResTrendOrgDataPacket();
                    break;
                case FuncTypeOrg.InitReportData:
                    dataPacket = new ResReportInitDataPacket();
                    break;
                case FuncTypeOrg.RateReport:
                    dataPacket = new ResRateReportOrgDataPacket();
                    break;
                case FuncTypeOrg.FinanceReport:
                    dataPacket = new ResFinanceReportOrgDataPacket();
                    break;
                case FuncTypeOrg.FinanceOrg:
                    dataPacket = new ResFinanceOrgDataPacket();
                    break;
                case FuncTypeOrg.DivideRightOrg:
                    dataPacket = new ResDivideRightOrgDataPacket();
                    break;
                case FuncTypeOrg.CapitalFlowReport:
                    dataPacket = new ResCapitalFlowReportOrgDataPacket();
                    break;
                case FuncTypeOrg.DDEReport:
                    dataPacket = new ResDDEReportOrgDataPacket();
                    break;
                case FuncTypeOrg.NetInFlowReport:
                    dataPacket = new ResNetInFlowReportOrgDataPacket();
                    break;
                case FuncTypeOrg.ProfitForecastReport:
                    dataPacket = new ResProfitForecastReportOrgDataPacket();
                    break;
                case FuncTypeOrg.FinanceStockReport:
                    dataPacket = new ResFinanceStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.CustomCapitalFlowReport: dataPacket = new ResCustomCapitalFlowReportOrgDataPacket();
                    break;
                case FuncTypeOrg.CustomNetInFlowReport: dataPacket = new ResCustomNetInFlowReportOrgDataPacket();
                    break;
                case FuncTypeOrg.CustomDDEReport: dataPacket = new ResCustomDDEReportOrgDataPacket();
                    break;
                case FuncTypeOrg.CustomProfitForecastReport: dataPacket = new ResCustomProfitForecastReportOrgDataPacket();
                    break;
                case FuncTypeOrg.CustomFinanceStockReport: dataPacket = new ResCustomFinanceStockReportOrgDataPacket();
                    break;
                case FuncTypeOrg.MonetaryFundDetail:
                    dataPacket = new ResMonetaryFundDetailDataPacket();
                    break;
                case FuncTypeOrg.NonMonetaryFundDetail:
                    dataPacket = new ResNonMonetaryFundDetailDataPacket();
                    break;
                //case FuncTypeOrg.FundKlineNonDivide: 
                //    dataPacket = new ResFundKLineNonDivideDataPacket();
                //    break;
                case FuncTypeOrg.FundKlineAfterDivide:
                    dataPacket = new ResFundKLineAfterDivideDataPacket();
                    break;
                case FuncTypeOrg.FundTrpAndSunDetail: dataPacket = new ResTrpAndSunDetailDataPacket();
                    break;
                case FuncTypeOrg.FundCIPMonetaryDetail: dataPacket = new ResCIPMonetaryDetailDataPacket();
                    break;
                case FuncTypeOrg.FundCIPNonMonetaryDetail: dataPacket = new ResCIPNonMonetaryDetailDataPacket();
                    break;
                case FuncTypeOrg.FundBFPDetail: dataPacket = new ResBFPDetailDataPacket();
                    break;
                case FuncTypeOrg.FundHeaveStockReport: dataPacket = new ResFundHeaveStockReport();
                    break;
                case FuncTypeOrg.FundHYReport: dataPacket = new ResFundHYReport();
                    break;
                case FuncTypeOrg.FundKeyBondReport: dataPacket = new ResKeyBondReport();
                    break;
                case FuncTypeOrg.FundManager: dataPacket = new ResFundManager();
                    break;
                case FuncTypeOrg.FinanceHeaveStockReport: dataPacket = new ResFinanceHeaveStockReport();
                    break;
                case FuncTypeOrg.FinanceHeaveHYReport: dataPacket = new ResFinanceHYReport();
                    break;
                case FuncTypeOrg.FinanceHeaveBondReport: dataPacket = new ResFinanceKeyBondReport();
                    break;
                case FuncTypeOrg.FinanceHeaveManagerReport: dataPacket = new ResFinanceManager();
                    break;
                case FuncTypeOrg.FinanceHeaveFundReport: dataPacket = new ResFinanceHeaveFundReprotDataPacket();
                    break;
                case FuncTypeOrg.DepthAnalyse: dataPacket = new ResDepthAnalyseDataPacket();
                    break;
                case FuncTypeOrg.EMIndexDetail: dataPacket = new ResEMIndexDetailDataPacket();
                    break;
                case FuncTypeOrg.Rank: dataPacket = new ResRankOrgDataPacket();
                    break;
                case FuncTypeOrg.IndexStatic: dataPacket = new ResIndexStaticOrgDataPacket();
                    break;
                case FuncTypeOrg.CNIndexDetail: dataPacket = new ResCNIndexDetailDataPacket();
                    break;
                case FuncTypeOrg.CSIIndexDetail: dataPacket = new ResCSIIndexDetailDataPacket();
                    break;
                case FuncTypeOrg.CSIndexDetail: dataPacket = new ResCSIndexDetailDataPacket();
                    break;
                case FuncTypeOrg.GlobalIndexDetail: dataPacket = new ResGlobalIndexDetailDataPacket();
                    break;
                case FuncTypeOrg.InterBankDetail: dataPacket = new ResInterBankDetailDataPacket();
                    break;
                case FuncTypeOrg.ShiborDetail: dataPacket = new ResShiborDetailDataPacket();
                    break;
                case FuncTypeOrg.RateSwapDetail: dataPacket = new ResRateSwapDetailDataPacket();
                    break;
                case FuncTypeOrg.InterBankRepurchaseDetail: dataPacket = new ResInterBankRepurchaseDetailDataPacket();
                    break;
                case FuncTypeOrg.USStockReport: dataPacket = new ResUSStockReportDataPacket();
                    break;
                case FuncTypeOrg.ForexReport: dataPacket = new ResForexReportDataPacket();
                    break;

                case FuncTypeOrg.OSFuturesReport:
                case FuncTypeOrg.OSFuturesReportNew:
                    dataPacket = new ResOSFuturesReportDataPacket();
                    break;

                case FuncTypeOrg.USStockDetail: dataPacket = new ResUSStockDetailDataPacket();
                    break;
                case FuncTypeOrg.ForexDetail: dataPacket = new ResForexDetailDataPacket();
                    break;
                case FuncTypeOrg.OSFuturesDetail: dataPacket = new ResOSFuturesDetailDataPacket();
                    break;
                case FuncTypeOrg.OSFuturesLMEDetail: dataPacket = new ResOSFuturesLMEDetailDataPacket();
                    break;
                case FuncTypeOrg.NetInFlowRank: dataPacket = new ResNetInflowRankDataPacket();
                    break;
                case FuncTypeOrg.ConvertBondDetail: dataPacket = new ResConvertBondDetailDataPacket();
                    break;
                case FuncTypeOrg.NonConvertBondDetail: dataPacket = new ResNonConvertBondDetailDataPacket();
                    break;
                case FuncTypeOrg.OSFuturesLMEDeal: dataPacket = new ResOSFuturesLMEDealDataPacket();
                    break;
                case FuncTypeOrg.LowFrequencyTBY: dataPacket = new ResLowFrequencyTBYDataPacket();
                    break;
                case FuncTypeOrg.BankBondReport: dataPacket = new ResBankBondReportDataPacket();
                    break;
                case FuncTypeOrg.ShiborReport: dataPacket = new ResShiborReportDataPacket();
                    break;
                case FuncTypeOrg.NewProfitForcast: dataPacket = new ResNewProfitForecastDataPacket();
                    break;
                case FuncTypeOrg.BondPublicOpeartion: dataPacket = new ResBondDashboardPublicMarketOpeartion();
                    break;
                case FuncTypeOrg.ChangeName: dataPacket = new ResChangeNameDataPacket();
                    break;
                default:
                    LogUtilities.LogMessage("收到未知数据包,FuncType等于" + funcType.ToString());
                    break;
            }
            br.BaseStream.Position = 0;

            if (dataPacket != null)
            {
                try
                {
                    dataPacket.Decoding(br);
                }
                catch (Exception ex)
                {
                    LogUtilities.LogMessage(funcType.ToString() + ex.Message);
                }

            }
            return dataPacket;
        }
    }

    /// <summary>
    /// 机构资讯
    /// </summary>
    public class InfoOrgBaseDataPacket : DataPacket
    {
        public static string NewsUrlHead = string.Format(@"http://app.jg.eastmoney.com/html_News/DetailOnly.html?infoCode=");
        public static string NoticeUrlHead = string.Format(@"http://app.jg.eastmoney.com/html_Notice/Detail.html?isprice=1&infocode=");
        public static string ReportUrlHead = string.Format(@"http://app.jg.eastmoney.com/html_Report/Detail.html?isprice=1&infocode=");

        /// <summary>
        /// 请求类型
        /// </summary>
        public FuncTypeInfoOrg RequestId;

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="br"></param>
        public override bool Decoding(BinaryReader br)
        {
            return false;
        }

        /// <summary>
        /// 组包
        /// </summary>
        /// <returns></returns>
        public virtual byte[] CodeInfoPacket()
        { return new byte[0]; }
    }



    #endregion

    #region 个股宏观指标

    /// <summary>
    /// 宏观指标数据的请求包的类别
    /// </summary>
    public enum IndicateRequestType : byte
    {
        /// <summary>
        /// 请求当前选择的宏观指标的指标值
        /// </summary>
        IndicatorValuesReport = 0,
        /// <summary>
        /// 请求股票对应的宏观指标的左侧报表
        /// </summary>
        LeftIndicatorsReport = 1,

        /// <summary>
        /// 请求股票对应的宏观指标的右侧报表
        /// </summary>
        RightIndicatorsReport = 2
    }


    /// <summary>
    /// 个股宏观指标
    /// </summary>
    public class IndicatorDataPacket : DataPacket
    {

        /// <summary>
        /// 请求（股票的默认）宏观指标报表命令："$-edbrpt\r\n$stockrise(name={0}|reporttype={1})\r\n"
        /// eg. "$-edbrpt\r\n$stockrise(name=000960.SZ|reporttype=2)\r\n"
        /// 位置0: 是股票代码(eg. 000960.SZ
        /// 位置1: 请求左侧(1)还是右侧报表(2)
        /// </summary>
        public static readonly string DefaultIndicatorsReportCmd
            = "$-edbrpt\r\n$stockrise(name={0}|reporttype={1})\r\n";

        /// <summary>
        /// 请求（股票的默认+自定义）宏观指标报表命令："$-edbrpt\r\n$stockrise(name={0}|MACROIDS={1}|reporttype={2})\r\n" 
        /// eg. "$-edbrpt\r\n$stockrise(name=000960.SZ|MACROIDS=EMI00224884,EMI00064805|reporttype=2)\r\n"
        /// 位置0: 是股票代码(eg. 000960.SZ
        /// 位置1：宏观指标Id类表（多个时用","隔开）
        /// 位置2: 请求左侧(1)还是右侧报表(2)
        /// </summary>
        public static readonly string CustomIndicatorsReportCmd
            = "$-edbrpt\r\n$stockrise(name={0}|MACROIDS={1}|reporttype={2})\r\n";


        /// <summary>
        /// 请求宏观指标值的命令: "$-edb\r\n$indicate(name={0}|startDate={1}|enddate={2})\r\n"
        /// eg. "$-edb\r\n$indicate(name=EMI00224884|startDate=2012-12-12|enddate=2013-12-12)\r\n"
        /// 位置0：name:指标code
        /// 位置1: startDate:开始时间
        /// 位置2: endDate:结束时间
        /// </summary>
        public static readonly string IndicatorValuesReportCmd
            = "$-edb\r\n$indicate(name={0}|startDate={1}|enddate={2})\r\n";

        /// <summary>
        /// 命令请求的股票代码或者指标代码（"000960.SZ" 或者 "EMI00064805"）
        /// </summary>
        public string TableKeyCode;


        /// <summary>
        /// 宏观指标查询命令字符串
        /// </summary>
        public string Cmd;

        ///// <summary>
        ///// 返回数据
        ///// </summary>
        //public DataTable Table;
        /// <summary>
        /// 请求类型
        /// </summary>
        public IndicateRequestType RequestId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        public override bool Decoding(BinaryReader br)
        {
            return false;
        }


        /// <summary>
        /// 讲DataTable转成我们需要的实体对象
        /// </summary>
        /// <param name="br"></param>
        public virtual bool Decoding(DataTable dt)
        {
            return false;
        }
        /// <summary>
        /// 组包
        /// </summary>
        /// <returns></returns>
        public virtual string CreateCommand()
        {
            return string.Empty;
        }


    }
    #endregion

}