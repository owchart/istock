using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OwLib
{

    /// <summary>
    /// 机构行情板块字段id
    /// </summary>
    public enum ReqFieldIndexOrg
    {
        #region 基金静态
        /// <summary>
        /// 基金最新净值对应日期
        /// </summary>
        FundLatestDate = 6004,
        /// <summary>
        /// 基金单位净值
        /// </summary>
        FundPernav = 6005,
        /// <summary>
        /// 上个交易日单位净值
        /// </summary>
        FundNvper1 = 6006,
        ///// <summary>
        ///// 日回报
        ///// </summary>
        //FundNvgrwtd = 6007,
        /// <summary>
        /// 年初至今复权单位净值增长率
        /// </summary>
        FundNvgrty = 6008,
        /// <summary>
        /// 近一周复权单位净值增长率
        /// </summary>
        FundNvgrw1w = 6009,
        /// <summary>
        /// 近一月复权单位净值增长率
        /// </summary>
        FundNvgr4w = 6021,
        /// <summary>
        /// 近一季复权单位净值增长率
        /// </summary>
        FundNvgr13w = 6022,
        /// <summary>
        /// 近半年复权单位净值增长率
        /// </summary>
        FundNvgr26w = 6023,
        /// <summary>
        /// 近一年复权单位净值增长率
        /// </summary>
        FundNvgr52w = 6024,
        /// <summary>
        /// 近两年复权单位净值增长率
        /// </summary>
        FundNvgr104w = 6025,
        /// <summary>
        /// 近三年复权单位净值增长率
        /// </summary>
        FundNvgr156w = 6026,
        /// <summary>
        /// 近五年复权单位净值增长率
        /// </summary>
        FundNvgr208w = 6027,
        /// <summary>
        /// 成立至今复权单位净值增长率
        /// </summary>
        FundNvgrf = 6028,
        /// <summary>
        /// 年化回报
        /// </summary>
        FundDecYearhb = 6029,
        /// <summary>
        /// 晨星评级
        /// </summary>
        FundStrCxrank3y = 6041,
        /// <summary>
        /// 银河评级
        /// </summary>
        FundNvgrthl3year = 6042,
        /// <summary>
        /// 最新规模（亿元）
        /// </summary>
        FundNav = 6043,
        /// <summary>
        /// 投资类型
        /// </summary>
        FundParaname = 6044,
        /// <summary>
        /// 申赎状态
        /// </summary>
        FundStrSgshzt = 6045,
        /// <summary>
        /// 基金公司代码
        /// </summary>
        FundManagercode = 6046,
        /// <summary>
        /// 基金公司
        /// </summary>
        FundManagername = 6047,
        /// <summary>
        /// 托管银行
        /// </summary>
        FundStrTgrcom = 6048,
        /// <summary>
        /// 托管银行代码
        /// </summary>
        FundTgrcode = 6049,
        /// <summary>
        /// 最新涨幅
        /// </summary>
        FundDecZdf = 6051,
        /// <summary>
        /// 累计净值
        /// </summary>
        FundAccunav = 6052,
        /// <summary>
        /// 成立日期
        /// </summary>
        FundFounddate = 6053,
        /// <summary>
        /// 到期日期
        /// </summary>
        FundEnddate = 6054,
        /// <summary>
        /// 基金经理
        /// </summary>
        FundFmanager = 6055,
        /// <summary>
        /// 交易所
        /// </summary>
        FundStrTexch = 6056,
        /// <summary>
        /// 场内流通份额
        /// </summary>
        FundCshare = 6057,
        /// <summary>
        /// 贴水率
        /// </summary>
        FundAgior = 6058,
        /// <summary>
        /// 最新份额
        /// </summary>
        FundLastestShare = 6100,
        #endregion

        #region 债券静态
        /// <summary>
        /// 债券交易日期
        /// </summary>
        BondDate = 7001,
        /// <summary>
        /// 最新票面利率
        /// </summary>
        BondNewrate = 7005,
        /// <summary>
        /// 债券期限(年)
        /// </summary>
        BondBondperiod = 7006,
        /// <summary>
        /// 债券评级(最新)
        /// </summary>
        BondStrZqpj = 7007,
        /// <summary>
        /// 主体评级(最新)
        /// </summary>
        BondStrZtpj = 7008,
        /// <summary>
        /// 特殊条款
        /// </summary>
        BondStrTstk = 7009,
        /// <summary>
        /// 发行人
        /// </summary>
        BondInstname = 7021,
        /// <summary>
        /// 麦式久期
        /// </summary>
        BondDuration = 7022,
        /// <summary>
        /// 修正久期
        /// </summary>
        BondMduration = 7023,
        /// <summary>
        /// 凸性
        /// </summary>
        BondConvexity = 7024,
        /// <summary>
        /// 基点价值
        /// </summary>
        BondBasisvalue = 7025,
        /// <summary>
        /// 应计利息
        /// </summary>
        BondAI = 7026,
        /// <summary>
        /// 前收盘全价
        /// </summary>
        BondLfclose = 7027,
        /// <summary>
        /// 前收盘净价
        /// </summary>
        BondLcclose = 7028,
        /// <summary>
        /// 前收（YTM）
        /// </summary>
        BondDecLcytm = 7029,
        /// <summary>
        /// 上日均价
        /// </summary>
        BondDecLcavg = 7041,
        /// <summary>
        /// 到期日期
        /// </summary>
        BondMrtydate = 7042,
        /// <summary>
        /// 剩余期限(年)
        /// </summary>
        BondTomrtyyear = 7043,
        /// <summary>
        /// 上日加权YTM
        /// </summary>
        BondDecLavgytm = 7044,
        /// <summary>
        /// 债券类型
        /// </summary>
        BondType = 7045,
        /// <summary>
        /// 债券余额(亿元)
        /// </summary>
        BondDecZqye = 7046,
        /// <summary>
        /// 正股价格
        /// </summary>
        BondStockPrice = 7050,
        /// <summary>
        /// 正股涨跌幅
        /// </summary>
        BondStockDiffRange = 7051,
        /// <summary>
        /// 正股换手率
        /// </summary>
        BondStockTurnOver = 7052,
        /// <summary>
        /// 转股价值
        /// </summary>
        BondSwapValue=7053,
        /// <summary>
        /// 转股溢价率
        /// </summary>
        BondConversionPremium = 7054,
        /// <summary>
        /// 套利空间
        /// </summary>
        BondArbitrageSpace = 7055,
        /// <summary>
        /// 纯债溢价率
        /// </summary>
        BondPremiumRate = 7056,
        /// <summary>
        /// 平价底价溢价率
        /// </summary>
        BondPbpPremiumRate = 7057,
        /// <summary>
        /// 开盘价ytm
        /// </summary>
        BondOpenYtm = 7058,
        /// <summary>
        /// 最高价ytm
        /// </summary>
        BondHighYtm = 7059,

        /// <summary>
        /// 转股价格
        /// </summary>
        BondSwapPrice = 7069,
        /// <summary>
        /// 转股比例
        /// </summary>
        BondSwapRate = 7070,
        /// <summary>
        /// 纯债价值
        /// </summary>
        BondCzjz = 7071,
        /// <summary>
        /// 利率均价涨跌
        /// </summary>
        BondAveDifference = 7072,
        /// <summary>
        /// 最新YTM
        /// </summary>
        BondNowYTM = 7073,
        /// <summary>
        /// 最新加权YTM
        /// </summary>
        BondDecNowYTM = 7075,
        /// <summary>
        /// 最低价ytm
        /// </summary>
        BondLowYtm = 7080,

        /// <summary>
        /// 利率(%)
        /// </summary>
        CouponRate = 17002,
        /// <summary>
        /// 发行总量(亿元)
        /// </summary>
        IssueVol = 17003,
        /// <summary>
        /// 期限
        /// </summary>
        Period = 17004,
        /// <summary>
        /// 操作类型
        /// </summary>
        OType = 17005,
        #endregion

        #region 股票静态
        /// <summary>
        /// 总股本long
        /// </summary>
        TotalShares = 8069,
        /// <summary>
        /// 流通股本
        /// </summary>
        FlowShares = 8080,
        /// <summary>
        /// 财务数据更新日期
        /// </summary>
        BGQDate = 9401,
        /// <summary>
        /// 流通A股
        /// </summary>
        NetAShare,
        /// <summary>
        /// 每股收益
        /// </summary>
        EpsTtm,
        /// <summary>
        /// 每股净资产
        /// </summary>
        MGJZC,
        /// <summary>
        /// 加权净资产收益率
        /// </summary>
        JZC,
        /// <summary>
        /// 营业收入
        /// </summary>
        ZYYWSR,
        /// <summary>
        /// 净利润
        /// </summary>
        ZYYWLR,
        /// <summary>
        /// 总资产
        /// </summary>
        ZZC,
        /// <summary>
        /// 总负债
        /// </summary>
        TotalLiab,
        /// <summary>
        /// 股东权益
        /// </summary>
        OWnerEqu,
        /// <summary>
        /// 公积金
        /// </summary>
        CapRes,
        /// <summary>
        /// 流通B股
        /// </summary>
        NetBShare,
        /// <summary>
        /// 营业利润
        /// </summary>
        ProfitFO,
        /// <summary>
        /// 投资收益
        /// </summary>
        InvIncome,
        /// <summary>
        /// 利润总额
        /// </summary>
        PBTax,
        /// <summary>
        /// 未分配利润
        /// </summary>
        RProfotAA,
        /// <summary>
        /// 流动资产
        /// </summary>
        CurAsset,
        /// <summary>
        /// 固定资产
        /// </summary>
        FixAsset,
        /// <summary>
        /// 无形资产
        /// </summary>
        IntAsset,
        /// <summary>
        /// 流动负债
        /// </summary>
        TCurLiab,
        /// <summary>
        /// 长期负债
        /// </summary>
        TLongLiab,
        /// <summary>
        /// 总股本
        /// </summary>
        HShare,
        /// <summary>
        /// 上市日期
        /// </summary>
        ListDate,
        /// <summary>
        /// 营业收入同比
        /// </summary>
        IncomeRatio,
        /// <summary>
        /// 净利润同比
        /// </summary>
        NPRatio,
        #endregion

        #region 股票动态
        /// <summary>
        /// 股票代码String
        /// </summary>
        Code = 8000,//String
        /// <summary>
        /// 股票名称String
        /// </summary>
        Name = 8001,
        /// <summary>
        /// 现价float
        /// </summary>
        Now = 8002,//float
        /// <summary>
        /// 涨跌幅float
        /// </summary>
        DifferRange = 8004,
        /// <summary>
        /// 涨跌float
        /// </summary>
        Differ,
        /// <summary>
        /// 成交量int
        /// </summary>
        Volume,
        /// <summary>
        /// 现手int
        /// </summary>
        CurVol,//现手
        /// <summary>
        /// 买入价float
        /// </summary>
        BuyPrice1,
        /// <summary>
        /// 买二价
        /// </summary>
        BuyPrice2 = 8011,
        /// <summary>
        /// 买3价
        /// </summary>
        BuyPrice3 = 8012,
        /// <summary>
        /// 买4价
        /// </summary>
        BuyPrice4 = 8013,
        /// <summary>
        /// 买5价
        /// </summary>
        BuyPrice5 = 8014,
        /// <summary>
        /// 买6价
        /// </summary>
        BuyPrice6 = 8015,
        /// <summary>
        /// 买7价
        /// </summary>
        BuyPrice7 = 8016,
        /// <summary>
        /// 买8价
        /// </summary>
        BuyPrice8 = 8017,
        /// <summary>
        /// 买9价
        /// </summary>
        BuyPrice9 = 8018,
        /// <summary>
        /// 买10价
        /// </summary>
        BuyPrice10 = 8019,
        /// <summary>
        /// 卖出价float
        /// </summary>
        SellPrice1 = 8020,
        /// <summary>
        /// 涨速float
        /// </summary>
        Accer = 8022,
        /// <summary>
        /// 换手率float
        /// </summary>
        Turnover,
        /// <summary>
        /// 成交金额double
        /// </summary>
        Amount,
        /// <summary>
        /// 市盈率float
        /// </summary>
        PE = 8026,
        /// <summary>
        /// 最高价float
        /// </summary>
        High = 8028,
        /// <summary>
        /// 卖2价
        /// </summary>
        SellPrice2 = 8031,
        /// <summary>
        /// 卖3价
        /// </summary>
        SellPrice3 = 8032,
        /// <summary>
        /// 卖4价
        /// </summary>
        SellPrice4 = 8033,
        /// <summary>
        /// 卖5价
        /// </summary>
        SellPrice5 = 8034,
        /// <summary>
        /// 卖6价
        /// </summary>
        SellPrice6 = 8035,
        /// <summary>
        /// 卖7价
        /// </summary>
        SellPrice7 = 8036,
        /// <summary>
        /// 卖8价
        /// </summary>
        SellPrice8 = 8037,
        /// <summary>
        /// 卖9价
        /// </summary>
        SellPrice9 = 8038,
        /// <summary>
        /// 卖10价
        /// </summary>
        SellPrice10 = 8039,
        /// <summary>
        /// 最低价float
        /// </summary>
        Low = 8040,
        /// <summary>
        /// 开盘价float
        /// </summary>
        Open = 8042,
        /// <summary>
        /// 昨收价float
        /// </summary>
        PreClose = 8044,
        /// <summary>
        /// 振幅float
        /// </summary>
        Delta = 8046,//振幅
        /// <summary>
        /// 量比float
        /// </summary>
        LiangBi= 8047,
        /// <summary>
        /// 委比float
        /// </summary>
        WeiBi,
        /// <summary>
        /// 委差float
        /// </summary>
        WeiCha,
        /// <summary>
        /// 板块均价float
        /// </summary>
        BlockAvePrice = 8060,
        /// <summary>
        /// 内盘int
        /// </summary>
        NeiPan,
        /// <summary>
        /// 外盘int
        /// </summary>
        WaiPan,
        /// <summary>``
        /// 买一量int
        /// </summary>
        BuyVolume1,
        /// <summary>
        /// 卖一量int
        /// </summary>
        SellVolume1,
        /// <summary>
        /// 市净率float
        /// </summary>
        PB = 8067,

        /// <summary>
        /// 总市值float
        /// </summary>
        TotalValue = 8070,
        /// <summary>
        /// 买二量
        /// </summary>
        BuyVolume2 = 8071,
        /// <summary>
        /// 买3量
        /// </summary>
        BuyVolume3 = 8072,
        /// <summary>
        /// 买4量
        /// </summary>
        BuyVolume4 = 8073,
        /// <summary>
        /// 买5量
        /// </summary>
        BuyVolume5 = 8074,
        /// <summary>
        /// 买6量
        /// </summary>
        BuyVolume6 = 8075,
        /// <summary>
        /// 买7量
        /// </summary>
        BuyVolume7 = 8076,
        /// <summary>
        /// 买8量
        /// </summary>
        BuyVolume8 = 8077,
        /// <summary>
        /// 买9量
        /// </summary>
        BuyVolume9 = 8078,
        /// <summary>
        /// 买10量
        /// </summary>
        BuyVolume10 = 8079,

        /// <summary>
        /// 流通市值float
        /// </summary>
        LTSZ = 8081,
        /// <summary>
        /// 前2日收盘价float
        /// </summary>
        PreCloseDay3,
        /// <summary>
        /// 前5日收盘价float
        /// </summary>
        PreCloseDay5,
        /// <summary>
        /// 前2日总量int
        /// </summary>
        SumVolumeDay2 = 8084,
        /// <summary>
        /// 前5日总量int
        /// </summary>
        SumVolumeDay5 = 8085,
        /// <summary>
        /// 领涨股String
        /// </summary>
        Leader = 8086,
        /// <summary>
        /// 平均收益float
        /// </summary>
        AverRevenue = 8087,
        /// <summary>
        /// 平均股本float
        /// </summary>
        AverGB = 8088,
        /// <summary>
        /// 股票数int
        /// </summary>
        StockNum = 8089,
        /// <summary>
        /// 卖2量
        /// </summary>
        SellVolume2 = 8091,
        /// <summary>
        /// 卖3量
        /// </summary>
        SellVolume3 = 8092,
        /// <summary>
        /// 卖4量
        /// </summary>
        SellVolume4 = 8093,
        /// <summary>
        /// 卖5量
        /// </summary>
        SellVolume5 = 8094,
        /// <summary>
        /// 卖6量
        /// </summary>
        SellVolume6 = 8095,
        /// <summary>
        /// 卖7量
        /// </summary>
        SellVolume7 = 8096,
        /// <summary>
        /// 卖8量
        /// </summary>
        SellVolume8 = 8097,
        /// <summary>
        /// 卖9量
        /// </summary>
        SellVolume9 = 8098,
        /// <summary>
        /// 卖10量
        /// </summary>
        SellVolume10 = 8099,
        /// <summary>
        /// 上涨家数int
        /// </summary>
        UpNum = 8100,
        /// <summary>
        /// 下跌家数int
        /// </summary>
        DownNum = 8101,
        /// <summary>
        /// 时间int
        /// </summary>
        Time = 8103,
        /// <summary>
        /// 5日涨跌幅float
        /// </summary>
        DifferRangeDay5 = 8104,
        /// <summary>
        /// 20日涨跌幅float
        /// </summary>
        DifferRangeDay20 = 8105,
        /// <summary>
        /// 60日涨跌幅float
        /// </summary>
        DifferRangeDay60 = 8107,
        /// <summary>
        /// 前60日收盘价float
        /// </summary>
        //PreCloseDay60 = 8108,
        /// <summary>
        /// 年初至今涨跌幅float
        /// </summary>
        DifferRangeYTD = 8109,
        /// <summary>
        /// 10日涨跌幅float
        /// </summary>
        DifferRangeDay10,
        /// <summary>
        /// 1分钟涨跌幅float
        /// </summary>
        DifferRange1Mint,
        /// <summary>
        /// 3分钟涨跌幅float
        /// </summary>
        DifferRange3Mint,
        /// <summary>
        /// 5分钟涨跌幅float
        /// </summary>
        DifferRange5Mint,
        /// <summary>
        /// 120日涨跌幅float
        /// </summary>
        DifferRangeDay120,
        /// <summary>
        /// 250日涨跌幅float
        /// </summary>
        DifferRangeDay250,

        /// <summary>
        /// 年初至今收盘价float
        /// </summary>
        PreCloseYTD = 8120,
        /// <summary>
        /// 52周最高价float
        /// </summary>
        HighW52,
        /// <summary>
        /// 52周最低价float
        /// </summary>
        LowW52,
        /// <summary>
        /// 平盘家数int
        /// </summary>
        SameNum,
        /// <summary>
        /// 上涨比例float
        /// </summary>
        UpRange,
        /// <summary>
        /// 净流入额double
        /// </summary>
        NetInFlow = 8126,
        /// <summary>
        /// 净流入率
        /// </summary>
        NetFlowRange = 8127,
        /// <summary>
        /// 5日内净流入>0天数
        /// </summary>
        NetFlowRedDay5 = 8128,
        /// <summary>
        /// 净流入5日
        /// </summary>
        NetFlowDay5 = 8129,
        /// <summary>
        /// 连红天数
        /// </summary>
        EvenRedDays = 8135,
        /// <summary>
        /// 连涨天数
        /// </summary>
        RedDays = 8136,
        /// <summary>
        /// 净流入率5天
        /// </summary>
        NetFlowRangeDay5 = 8140,
        /// <summary>
        /// 20日内净流入>0天数
        /// </summary>
        NetFlowRedDay20 = 8144,
        /// <summary>
        /// 净流入20日
        /// </summary>
        NetFlowDay20 = 8145,
        /// <summary>
        /// 净流入率20天
        /// </summary>
        NetFlowRangeDay20 = 8146,
        /// <summary>
        /// 60日内净流入>0天数
        /// </summary>
        NetFlowRedDay60,
        /// <summary>
        /// 净流入60日
        /// </summary>
        NetFlowDay60,
        /// <summary>
        /// 净流入率60天
        /// </summary>
        NetFlowRangeDay60,
        /// <summary>
        /// 龙头股1
        /// </summary>
        MarketValueTop1Code = 8160,
        /// <summary>
        /// 龙头股1的涨跌幅
        /// </summary>
        DifferRangeLead1 = 8162,
        /// <summary>
        /// 龙头股2
        /// </summary>
        MarketValueTop2Code,
        /// <summary>
        /// 龙头股2的涨跌幅
        /// </summary>
        DifferRangeLead2,
        /// <summary>
        /// 龙头股3
        /// </summary>
        MarketValueTop3Code,
        /// <summary>
        /// 龙头股3的涨跌幅
        /// </summary>
        DifferRangeLead3,
        /// <summary>
        /// 评级家数
        /// </summary>
        InvestGradeNum = 8261,
        /// <summary>
        /// 投资评级
        /// </summary>
        InvestGrade = 8262,
        /// <summary>
        /// 持仓量
        /// </summary>
        OpenInterest = 8263,
        /// <summary>
        /// 结算价
        /// </summary>
        SettlementPrice = 8264,
        /// <summary>
        /// 昨结算
        /// </summary>
        PreSettlementPrice = 8265,
        /// <summary>
        /// 增仓
        /// </summary>
        Increase = 8266,
        /// <summary>
        /// 日增仓
        /// </summary>
        IncreaseDaily = 8267,
        /// <summary>
        /// 昨持仓
        /// </summary>
        PreOpenInterest = 8269,
        /// <summary>
        /// 前10日收盘价
        /// </summary>
        PreCloseDay10 = 8301,
        /// <summary>
        /// 前20日收盘价
        /// </summary>
        PreCloseDay20 = 8302,
        /// <summary>
        /// 前60日收盘价
        /// </summary>
        PreCloseDay60 = 8303,
        /// <summary>
        /// 前120日收盘价
        /// </summary>
        PreCloseDay120 = 8304,
        /// <summary>
        /// 前250日收盘价
        /// </summary>
        PreCloseDay250 = 8305,
        /// <summary>
        /// 52周收盘价
        /// </summary>
        PreCloseWeek52 = 8306,
        /// <summary>
        /// 5日均量
        /// </summary>
        VolumeAvgDay5 = 8307,
        /// <summary>
        /// 4日净流入额
        /// </summary>
        NetInFlowDay4 = 8308,
        /// <summary>
        /// 4日净流入天数
        /// </summary>
        NetInFlowRedDay4 = 8309,
        /// <summary>
        /// 4日总成交金额
        /// </summary>
        AmountDay4 = 8310, 
        /// <summary>
        /// 19日净流入额
        /// </summary>
        NetInFlowDay19 = 8311,
        /// <summary>
        /// 19日净流入天数
        /// </summary>
        NetInFlowRedDay19 = 8312,
        /// <summary>
        /// 19日总成交金额
        /// </summary>
        AmountDay19 = 8313,
        /// <summary>
        /// 59日净流入额
        /// </summary>
        NetInFlowDay59 = 8314,
        /// <summary>
        /// 59日净流入天数
        /// </summary>
        NetInFlowRedDay59 = 8315,
        /// <summary>
        /// 59日总成交金额
        /// </summary>
        AmountDay59 = 8316,
        /// <summary>
        /// 资金流向集合竞价
        /// </summary>
        ValueCallAuction = 9001,
        /// <summary>
        /// 小单买入
        /// </summary>
        BuySmall,
        /// <summary>
        /// 小单卖出
        /// </summary>
        SellSmall,
        /// <summary>
        /// 中单买入
        /// </summary>
        BuyMiddle,
        /// <summary>
        /// 中单卖出
        /// </summary>
        SellMiddle,
        /// <summary>
        /// 大单买入
        /// </summary>
        BuyBig,
        /// <summary>
        /// 大单卖出
        /// </summary>
        SellBig,
        /// <summary>
        /// 特大单买入
        /// </summary>
        BuySuper,
        /// <summary>
        /// 特大单卖出
        /// </summary>
        SellSuper,
        /// <summary>
        /// 对中间价涨跌BP
        /// </summary>
        MedDiffer = 9051,
        /// <summary>
        /// 对中间价涨跌幅
        /// </summary>
        MedDifferRange = 9052,
        /// <summary>
        /// 当日DDX
        /// </summary>
        DDX = 9101,
        /// <summary>
        /// 当日DDY
        /// </summary>
        DDY,
        /// <summary>
        /// 当日DDZ
        /// </summary>
        DDZ,
        /// <summary>
        /// 5日DDX
        /// </summary>
        DDX5,
        /// <summary>
        /// 5日DDY
        /// </summary>
        DDY5,
        /// <summary>
        /// 10日DDX
        /// </summary>
        DDX10,
        /// <summary>
        /// 10日DDY
        /// </summary>
        DDY10,
        /// <summary>
        /// DDX连续飘红天数
        /// </summary>
        DDXRedFollowDay,
        /// <summary>
        /// 5日内DDX飘红天数
        /// </summary>
        DDXRedFollowDay5,
        /// <summary>
        /// 10日内DDX飘红天数
        /// </summary>
        DDXRedFollowDay10,
        /// <summary>
        /// 特大单买入占比
        /// </summary>
        BuyFlowRangeSuper,
        /// <summary>
        /// 特大单卖出占比
        /// </summary>
        SellFlowRangeSuper,
        /// <summary>
        /// 大单买入占比
        /// </summary>
        BuyFlowRangeBig,
        /// <summary>
        /// 大单卖出占比
        /// </summary>
        SellFlowRangeBig,
        /// <summary>
        /// 当日增仓占比
        /// </summary>
        ZengCangRange= 9201,
        /// <summary>
        /// 当日增仓占比排名
        /// </summary>
        ZengCangRank,
        /// <summary>
        /// 上一个增仓占比排名（当日）
        /// </summary>
        ZengCangRankHis,
        /// <summary>
        /// 3日增仓占比
        /// </summary>
        ZengCangRangeDay3,
        /// <summary>
        /// 3日增仓占比排名
        /// </summary>
        ZengCangRankDay3,
        /// <summary>
        /// 上一个增仓占比排名（3日）
        /// </summary>
        ZengCangRankHisDay3,
        /// <summary>
        /// 5日增仓占比
        /// </summary>
        ZengCangRangeDay5,
        /// <summary>
        /// 5日增仓占比排名
        /// </summary>
        ZengCangRankDay5,
        /// <summary>
        /// 上一个增仓占比排名（5日）
        /// </summary>
        ZengCangRankHisDay5,
        /// <summary>
        /// 10日增仓占比
        /// </summary>
        ZengCangRangeDay10 = 9211,
        /// <summary>
        /// 10日增仓占比排名
        /// </summary>
        ZengCangRankDay10,
        /// <summary>
        /// 上一个增仓占比排名（10日）
        /// </summary>
        ZengCangRankHisDay10,

        /// <summary>
        /// 是否有研报
        /// </summary>
        HasReport = 9301,
        /// <summary>
        /// 研报数
        /// </summary>
        ReportNum,
        /// <summary>
        /// 买入评级家次
        /// </summary>
        BuyReportNum,
        /// <summary>
        /// 增持评级家次
        /// </summary>
        AddReportNum,
        /// <summary>
        /// 中性评级家次
        /// </summary>
        NeuturReportNum,
        /// <summary>
        /// 减持评级家次
        /// </summary>
        ReduceReportNum,
        /// <summary>
        /// 卖出评级家次
        /// </summary>
        SellReportNum,
        /// <summary>
        /// 前一年收益
        /// </summary>
        EpsQmtby,
        /// <summary>
        /// 当年收益
        /// </summary>
        EpsCurrentYear,
        /// <summary>
        /// 后一年收益
        /// </summary>
        EpsNextYear1,
        /// <summary>
        /// 后两年收益
        /// </summary>
        EpsNextYear2,
        /// <summary>
        /// 后三年收益
        /// </summary>
        EpsNextYear3,
        #endregion
    }


    /// <summary>
    /// 客户端字段id
    /// </summary>
    public enum FieldIndex : short
    {
          A_FundSpecialPrevNetValue = 0x375,
        AB_PremiumRate = 0x241,
        AB_StockPriceRatio = 0x242,
        AccountTradeLinkId = 0x50d,
        AccountTradeLinkType = 0x8e,
        AccountTradeTypeEnum = 0x8d,
        ActualFactor = 0x227,
        AddReportNum = 100,
        AgvTurnoverDay5 = 0x165,
        AH_PremiumRate = 0x239,
        AH_StockPriceRatio = 570,
        AllBlockCode = 0x2330,
        Amount = 800,
        AmountDay19 = 0x323,
        AmountDay4 = 0x322,
        AmountDay59 = 0x324,
        ASecurityId = 0x74,
        AveragePrice = 0x142,
        AvgNetS = 0x329,
        AvgProfit = 0x34b,
        AvgShare = 0x34c,
        B_Amount = 0x35f,
        B_Code = 0x4f5,
        B_Difference = 0x246,
        B_DifferRange = 0x247,
        B_FundSpecialPrevNetValue = 0x376,
        B_Now = 0x243,
        B_NowRMB = 0x245,
        B_PreClose = 580,
        BA_StockPriceRatio = 0x248,
        BenchMark = 0x504,
        BGQDate = 0x39,
        BindTarget = 0x2332,
        BondAI = 0x1bf,
        BondAvgDiffer = 0x1d1,
        BondBasisvalue = 0x1be,
        BondBestBuyInitiator = 0x4c6,
        BondBestBuyNet = 0x1ec,
        BondBestBuyTotalFaceValue = 0x35a,
        BondBestBuyYtm = 0x1ed,
        BondBestSellInitiator = 0x4c5,
        BondBestSellNet = 490,
        BondBestSellTotalFaceValue = 0x35b,
        BondBestSellYtm = 0x1eb,
        BondBondperiod = 0x1cf,
        BondCBDate = 0x58,
        BondCBNet = 0x1ee,
        BondCBYtm = 0x1ef,
        BondConversionRate = 0x1e3,
        BondConvexity = 0x1bd,
        BondCSDate = 0x59,
        BondCSNet = 0x1f0,
        BondCSYtm = 0x1f1,
        BondDate = 0x55,
        BondDecAvgNetDecAvgYTM = 0x4e0,
        BondDecDiffRangeYTM = 0x1d0,
        BondDecLavgytm = 0x1c5,
        BondDecLcavg = 0x1c3,
        BondDecLcytm = 450,
        BondDecNowYTM = 0x1c7,
        BondDecYTMAvg = 0x1de,
        BondDiffRangeYTM = 0x1c8,
        BondDiffRangeYTMDay10 = 0x1ca,
        BondDiffRangeYTMDay120 = 0x1cd,
        BondDiffRangeYTMDay20 = 0x1cb,
        BondDiffRangeYTMDay250 = 0x1ce,
        BondDiffRangeYTMDay5 = 0x1c9,
        BondDiffRangeYTMDay60 = 460,
        BondDuration = 0x1bb,
        BondExerciseDay = 0x57,
        BondFullHigh = 0x1d7,
        BondFullLow = 0x1da,
        BondFullNow = 0x1d2,
        BondFullOpen = 0x1d4,
        BondFuturePublicOperate_BondDate = 0x4e7,
        BondFuturePublicOperate_CouponRate = 0x4e8,
        BondFuturePublicOperate_IssueVol = 0x4e9,
        BondFuturePublicOperate_OType = 0x4eb,
        BondFuturePublicOperate_Period = 0x4ea,
        BondInstDetail = 0x4c2,
        BondInstIndustry = 0x4c4,
        BondInstname = 0x4c1,
        BondInstType = 0x4c3,
        BondIsVouch = 0x4e2,
        BondLcclose = 0x1c1,
        BondLfclose = 0x1c0,
        BondMarket = 0x4bc,
        BondMduration = 0x1bc,
        BondMrtydate = 0x56,
        BondNetHigh = 0x1d8,
        BondNetLow = 0x1db,
        BondNetNow = 0x1d3,
        BondNetOpen = 0x1d5,
        BondNetYTMHigh = 0x4df,
        BondNetYTMLow = 0x4e1,
        BondNetYTMOpen = 0x4de,
        BondNewrate = 0x1ba,
        BondNowYTM = 0x1c6,
        BondSNDate = 90,
        BondSNNet = 0x1f2,
        BondSNYtm = 0x1f3,
        BondStockPrice = 0x1df,
        BondStockTurnover = 0x1e1,
        BondStrTstk = 0x4c0,
        BondStrZqpj = 0x4be,
        BondStrZtpj = 0x4bf,
        BondSwapPrice = 0x1e7,
        BondTomrtyyear = 0x1c4,
        BondTradeMarket = 0x4e3,
        BondType = 0x4bd,
        BondYTMAvg = 0x1dd,
        BondYTMHigh = 0x1d9,
        BondYTMLow = 0x1dc,
        BondYTMOpen = 470,
        BreakEvenPoint = 0x22b,
        BSecurityId = 0x75,
        BSFlag = 0x53,
        Buy_InstitutionName = 0x4f9,
        BuyBig = 0x3f5,
        BuyFlowRangeBig = 0x1ad,
        BuyFlowRangeSuper = 0x1ac,
        BuyMiddle = 0x3f6,
        BuyPrice1 = 0x17b,
        BuyPrice10 = 0x184,
        BuyPrice2 = 380,
        BuyPrice3 = 0x17d,
        BuyPrice4 = 0x17e,
        BuyPrice5 = 0x17f,
        BuyPrice6 = 0x180,
        BuyPrice7 = 0x181,
        BuyPrice8 = 0x182,
        BuyPrice9 = 0x183,
        BuyPriceYtm1 = 0x18f,
        BuyPriceYtm10 = 0x198,
        BuyPriceYtm2 = 400,
        BuyPriceYtm3 = 0x191,
        BuyPriceYtm4 = 0x192,
        BuyPriceYtm5 = 0x193,
        BuyPriceYtm6 = 0x194,
        BuyPriceYtm7 = 0x195,
        BuyPriceYtm8 = 0x196,
        BuyPriceYtm9 = 0x197,
        BuyReportNum = 0x63,
        BuySmall = 0x3f7,
        BuySuper = 0x3f4,
        BuyVolume1 = 15,
        BuyVolume10 = 0x18,
        BuyVolume10Delta = 0x2c,
        BuyVolume1Delta = 0x23,
        BuyVolume2 = 0x10,
        BuyVolume2Delta = 0x24,
        BuyVolume3 = 0x11,
        BuyVolume3Delta = 0x25,
        BuyVolume4 = 0x12,
        BuyVolume4Delta = 0x26,
        BuyVolume5 = 0x13,
        BuyVolume5Delta = 0x27,
        BuyVolume6 = 20,
        BuyVolume6Delta = 40,
        BuyVolume7 = 0x15,
        BuyVolume7Delta = 0x29,
        BuyVolume8 = 0x16,
        BuyVolume8Delta = 0x2a,
        BuyVolume9 = 0x17,
        BuyVolume9Delta = 0x2b,
        CapRes = 0x344,
        CASCVMFlag = 0x97,
        CASFlag = 0x50f,
        CASLowerPrice = 0x288,
        CASReferencePrice = 0x287,
        CASUpperPrice = 0x289,
        ChiSpelling = 0x4b4,
        Code = 0x4b0,
        ContractNumber = 0x403,
        ContractUnit = 0x84,
        CurAsset = 0x33b,
        CurOI = 0x3f0,
        CurrencyDescription = 0x50c,
        CVT_Date = 0x87,
        CZJZ = 0x1e9,
        CZYJL = 0x1e5,
        Date = 0,
        DayOI = 0x54,
        DDX = 0x174,
        DDX10 = 0x179,
        DDX5 = 0x177,
        DDXRedFollowDay = 12,
        DDXRedFollowDay10 = 14,
        DDXRedFollowDay5 = 13,
        DDY = 0x175,
        DDY10 = 0x17a,
        DDY5 = 0x178,
        DDZ = 0x176,
        DealPrice = 0x24d,
        DebtRatio = 0x1b9,
        Delta = 0x162,
        DeltaPrice = 0x21f,
        Difference = 0x12d,
        DifferRange = 0x12e,
        DifferRange10D = 0x132,
        DifferRange120D = 0x135,
        DifferRange1Mint = 0x13b,
        DifferRange20D = 0x133,
        DifferRange250D = 310,
        DifferRange2Y = 0x137,
        DifferRange3D = 0x12f,
        DifferRange3Mint = 0x13c,
        DifferRange3Y = 0x138,
        DifferRange5D = 0x130,
        DifferRange5Mint = 0x13d,
        DifferRange60D = 0x134,
        DifferRange6D = 0x131,
        DifferRangeFromCreate = 0x13a,
        DifferRangeLead1 = 0x13e,
        DifferRangeLead2 = 0x13f,
        DifferRangeLead3 = 320,
        DifferRangeYTD = 0x139,
        DifferSpeed = 0x141,
        DividendPerShare = 0x21e,
        DownLimit = 0x173,
        DownNum = 7,
        DQSYL = 0x349,
        DRPCapRes = 0x345,
        DRPRPAA = 0x338,
        EMCode = 0x4b1,
        EndDate = 0x6c,
        EpsCurrentYear = 0x356,
        EpsNextYear1 = 0x357,
        EpsNextYear2 = 0x358,
        EpsNextYear3 = 0x359,
        EpsQMTB = 810,
        EpsQMTBY = 0x32b,
        EpsTTM = 0x32c,
        EqualNum = 6,
        Evg5Volume = 0x3ed,
        ExchangeCNY = 0x249,
        ExercisePrice = 0x229,
        Factor = 550,
        FinanceHeaveFund = 0x232e,
        FinancingCost = 0x252,
        FixAsset = 0x33c,
        FJJJ_BAmount = 0x36b,
        FJJJ_BBuyPrice1 = 0x270,
        FJJJ_BDelta = 0x272,
        FJJJ_BDifference = 0x26f,
        FJJJ_BDifferRange = 0x25f,
        FJJJ_BFundAddShare = 0x260,
        FJJJ_BHigh = 0x274,
        FJJJ_BLastVolume = 0x404,
        FJJJ_BLow = 0x275,
        FJJJ_BNow = 0x25d,
        FJJJ_BOpen = 0x273,
        FJJJ_BPreClose = 0x25e,
        FJJJ_BSellPrice1 = 0x271,
        FJJJ_BStockCode = 0x4fd,
        FJJJ_BStockName = 0x4fe,
        FJJJ_BTurnover = 0x261,
        FJJJ_MGrossValue = 0x264,
        FJJJ_MNetValue = 600,
        FJJJ_MStockCode = 0x4ff,
        FJJJ_MStockName = 0x500,
        FJJJEndDate = 0x83,
        ForexClose = 0x285,
        FPLRatio = 0x27b,
        Frequency = 0x2331,
        FundAccunav = 0x207,
        FundAddShare = 0x268,
        FundAgior = 0x209,
        FundAvgIncomeYear = 0x20b,
        FundAVsFundB = 0x4fc,
        FundBondRange = 0x219,
        FundCshare = 520,
        FundCyjzhb = 540,
        FundDecYearhb = 0x204,
        FundDecZdf = 0x206,
        FundEnddate = 0x5d,
        FundFmanager = 0x4cf,
        FundFounddate = 0x5c,
        FundHeaveHY = 0x232c,
        FundHeaveManager = 0x232f,
        FundHeaveStock = 0x232b,
        FundHoldShare = 0x215,
        FundHoldValue = 0x216,
        FundIncome10K = 0x20e,
        FundIncomeRankYear1 = 0x4d5,
        FundIncomeRankYear2 = 0x4d6,
        FundIncomeRankYear3 = 0x4d7,
        FundIncomeRankYear4 = 0x4d8,
        FundIncomeRankYear5 = 0x4d9,
        FundIncomeRankYear6 = 0x4da,
        FundIncomeYear = 0x20d,
        FundIncomeYear1 = 0x20f,
        FundIncomeYear2 = 0x210,
        FundIncomeYear3 = 0x211,
        FundIncomeYear4 = 530,
        FundIncomeYear5 = 0x213,
        FundIncomeYear6 = 0x214,
        FundIncomeYear7D = 0x20c,
        FundIndustryIndexShortCode = 0x4d3,
        FundInvestmentType = 0x505,
        FundKeyBond = 0x232d,
        FundLastestShare = 0x355,
        FundLatestDate = 0x5b,
        FundManagerAyieldSinces = 0x21d,
        FundManagercode = 0x4cb,
        FundManagername = 0x4cc,
        FundManagerName = 0x4dc,
        FundMgrLeaveDate = 0x60,
        FundMgrPostDate = 0x5f,
        FundMgrYieldSince = 0x21b,
        FundName = 0x4d1,
        FundNameEn = 0x4d2,
        FundNav = 0x205,
        FundNetValueRange = 0x218,
        FundNetZZL = 0x20a,
        FundNvgr104w = 0x200,
        FundNvgr13w = 0x1fd,
        FundNvgr156w = 0x201,
        FundNvgr208w = 0x202,
        FundNvgr26w = 510,
        FundNvgr4w = 0x1fc,
        FundNvgr52w = 0x1ff,
        FundNvgrf = 0x203,
        FundNvgrthl3year = 0x4c8,
        FundNvgrty = 0x1fa,
        FundNvgrw1w = 0x1fb,
        FundNvgrwtd = 0x1f9,
        FundNvper1 = 0x1f8,
        FundParaname = 0x4c9,
        FundPernav = 0x1f7,
        FundRank = 0x4d4,
        FundScpz = 0x21a,
        FundSpecialPrevNetValue = 0x374,
        FundStockValueRange = 0x217,
        FundStrCxrank3y = 0x4c7,
        FundStrSgshzt = 0x4ca,
        FundStrTexch = 0x4d0,
        FundStrTgrcom = 0x4cd,
        FundTgrcode = 0x4ce,
        FundTrusteName = 0x4db,
        Gamma = 0x22e,
        Gprofit = 0x339,
        GreenVolume = 0x3eb,
        GrossProfitMargin = 0x1b7,
        GrossValue = 590,
        GroupCash = 890,
        GroupCashAccountFound = 0x37f,
        GroupCostPrice = 0x27e,
        GroupCriteria = 0x50a,
        GroupCriteriaEnum = 0x8b,
        GroupDefaultAccountUnicode = 140,
        GroupDilutionCost = 0x385,
        GroupDilutionCostPrice = 0x27f,
        GroupFPL = 0x381,
        GroupFrozenFound = 0x37d,
        GroupHaveBreak = 0x382,
        GroupHoldWarehouse = 900,
        GroupManager = 0x508,
        GroupManagerAccount = 0x509,
        GroupPLAll = 0x380,
        GroupPLNow = 0x383,
        GroupRateCommission = 0x388,
        GroupRateMinCommission = 0x389,
        GroupRateStampTax = 0x38a,
        GroupRateTransferSH = 0x38b,
        GroupRateTransferSZ = 0x38c,
        GroupRestFound = 0x37c,
        GroupStartCast = 0x386,
        GroupTotalAssets = 0x379,
        GroupTotalValue = 0x387,
        GroupUseableFound = 0x37e,
        GroupWithdrawCash = 0x37b,
        H_Amount = 0x35e,
        H_Code = 0x4f4,
        H_Difference = 0x23e,
        H_DifferRange = 0x23f,
        H_Now = 0x23b,
        H_NowRMB = 0x23d,
        H_PreClose = 0x23c,
        HA_StockPriceRatio = 0x240,
        High = 0x15d,
        HighPurchaseCost = 0x265,
        HighRansomCost = 0x266,
        HighW52 = 0x15f,
        HisRedDay = 0x6a,
        HisUpDay = 0x44,
        HKMarketPlanFlag = 120,
        HKV = 860,
        HoldStockCount = 0x38d,
        HoldWeight = 0x27a,
        HSecurityId = 0x76,
        Hshare = 840,
        IEPrice = 0x286,
        IEVolume = 0x90,
        ImpliedVolatility = 0x228,
        IncomeRatio = 0x331,
        Increase = 11,
        Index5DownLimit = 0x282,
        Index5UpLimit = 0x281,
        Index7DownLimit = 0x284,
        Index7UpLimit = 0x283,
        IndexAmount = 0x321,
        IndexGSMGSGDQY = 0x371,
        IndexVolume = 0x3e9,
        IndustryBlockCode = 0x68,
        IndustryBlockName = 0x4b5,
        InfoCode = 0x4e6,
        InfoMine = 0x2329,
        InnerValue = 0x223,
        InsSName = 0x4e5,
        Institution_DiamondType = 0x2333,
        InstitutionActiveList_BuyAmount = 0x361,
        InstitutionActiveList_BuyCount = 0x7d,
        InstitutionActiveList_Count = 0x7c,
        InstitutionActiveList_InstitutionCode = 0x4f8,
        InstitutionActiveList_InstitutionName = 0x4f7,
        InstitutionActiveList_SellAmount = 0x362,
        InstitutionActiveList_SellCount = 0x7e,
        IntAsset = 0x33d,
        InvestGrade = 0x1f6,
        InvestGradeNum = 0x5e,
        InvestManager = 0x50e,
        InvestmentType = 0x507,
        InvestmentTypeEnum = 0x8a,
        InvIncome = 0x333,
        IOPV = 0x277,
        IOPVPremium = 0x278,
        IOPVPremiumRatio = 0x279,
        IsDefaultCustomStock = 0x232a,
        JZC = 0x32f,
        LastTotalShare = 0x267,
        LastVolume = 0x3ea,
        LeftDay = 0x6d,
        ListDate = 0x38,
        Lmpyiled = 0x378,
        LotSize = 0x6b,
        Low = 350,
        LowW52 = 0x160,
        Ltg = 0x34d,
        LTSZ = 0x328,
        M_FundSpecialPrevNetValue = 0x377,
        MacroID = 0x4ec,
        MacroName = 0x4ed,
        MainNetFlow = 0x400,
        MarginFlag = 0x79,
        Market = 0x37,
        MarketValueTop1Code = 0x4b9,
        MarketValueTop2Code = 0x4ba,
        MarketValueTop3Code = 0x4bb,
        MaxSellCount = 910,
        MedDiffer = 0x1b4,
        MedDifferRange = 0x1b5,
        MergerPremium = 0x263,
        MergerPrice = 610,
        MGJZC = 0x32d,
        MGJZC2 = 0x32e,
        Na = 0x2336,
        Name = 0x4b2,
        NeiWaiBi = 0x1a3,
        NetAShare = 0x346,
        NetBShare = 0x347,
        NetFlow = 0x34e,
        NetFlowBig = 0x3fd,
        NetFlowDay19 = 0x351,
        NetFlowDay20 = 850,
        NetFlowDay4 = 0x34f,
        NetFlowDay5 = 0x350,
        NetFlowDay59 = 0x353,
        NetFlowDay60 = 0x354,
        NetFlowMiddle = 0x3fe,
        NetFlowRange = 0x1b0,
        NetFlowRangeBig = 0x1a9,
        NetFlowRangeDay20 = 0x1b2,
        NetFlowRangeDay5 = 0x1b1,
        NetFlowRangeDay60 = 0x1b3,
        NetFlowRangeMiddle = 0x1aa,
        NetFlowRangeSmall = 0x1ab,
        NetFlowRangeSuper = 0x1a8,
        NetFlowRedDay20 = 0x3f,
        NetFlowRedDay5 = 0x3d,
        NetFlowRedDay60 = 0x41,
        NetFlowSmall = 0x3ff,
        NetFlowSuper = 0x3fc,
        NetInFlowRedDay19 = 0x3e,
        NetInFlowRedDay4 = 60,
        NetInFlowRedDay59 = 0x40,
        NetProfitMargin = 440,
        NetProfitYOY = 0x1b6,
        NetValueFactor = 0x251,
        NeuturReportNum = 0x65,
        NextEngageYield = 0x25a,
        NoticeBell = 0x98,
        Now = 300,
        NPRatio = 0x336,
        Ntdisct_Date = 0x88,
        OEquRatio = 0x343,
        Open = 0x15c,
        OpenCloseStatus = 0x4b6,
        OpenInterest = 0x3ef,
        OpenInterestDaily = 10,
        OptionBlockType = 0x6f,
        OptionDelta = 0x22d,
        OptionType = 110,
        OptionTypeStr = 0x4f2,
        OptionYJL = 0x238,
        OrderImbalanceDirection = 0x511,
        OrderImbalanceQuantity = 0x405,
        OverflowPrice = 0x225,
        OWnerEqu = 0x342,
        PB = 0x16f,
        PBTax = 820,
        PE = 360,
        PECurrentYear = 0x16b,
        PELYR = 0x16a,
        PENextYear1 = 0x16c,
        PENextYear2 = 0x16d,
        PENextYear3 = 0x16e,
        PerformPoint = 0x22c,
        PETTM = 0x169,
        PJYJL = 0x1e6,
        PLAllRatio = 0x27c,
        PreClose = 0x14f,
        PreCloseDay10 = 0x152,
        PreCloseDay120 = 0x155,
        PreCloseDay20 = 0x153,
        PreCloseDay250 = 0x156,
        PreCloseDay3 = 0x150,
        PreCloseDay5 = 0x151,
        PreCloseDay60 = 340,
        PreCloseDayYTD = 0x159,
        PreCloseFromCreate = 0x15a,
        PreCloseWeek52 = 0x15b,
        PreCloseYear2 = 0x157,
        PreCloseYear3 = 0x158,
        PremiumRate = 0x256,
        PreOpenInterest = 0x3ee,
        PreSettlementPrice = 0x170,
        PrevIOPV = 630,
        PrevT1PremiumRate = 0x372,
        PrevT2PremiumRate = 0x373,
        PriceFactor = 0x250,
        ProfitFO = 0x332,
        PromiseIncomeRate = 0x259,
        Purchasefee = 0x26d,
        QuoteUnit = 0x402,
        RateAvgPrice10D = 0x144,
        RateAvgPrice120D = 0x147,
        RateAvgPrice20D = 0x145,
        RateAvgPrice250D = 0x148,
        RateAvgPrice5D = 0x143,
        RateAvgPrice60D = 0x146,
        RateAvgPricePre119D = 0x14d,
        RateAvgPricePre19D = 0x14b,
        RateAvgPricePre249D = 0x14e,
        RateAvgPricePre4D = 0x149,
        RateAvgPricePre59D = 0x14c,
        RateAvgPricePre9D = 330,
        RateDecAvgPrice = 500,
        Ratedes = 0x4e4,
        RateOfReturn = 640,
        RatePreDecPrice = 0x1f5,
        RateType = 0x4f3,
        RedDay = 0x42,
        Redepmptionfee = 0x26e,
        ReduceReportNum = 0x66,
        RedVolume = 0x3ec,
        ReferIndexDifferRange = 0x253,
        ReferIndexName = 0x4fb,
        ReferIndexUniCode = 130,
        RemainingYear = 0x24f,
        Remark = 0x50b,
        ReportNum = 0x62,
        Rho = 0x231,
        RProfotAA = 0x337,
        SecuCode = 0x4b3,
        SecurityNum = 0x4f0,
        Sell_InstitutionName = 0x4fa,
        SellBig = 0x3f9,
        SellFlowRangeBig = 0x1af,
        SellFlowRangeSuper = 430,
        SellMiddle = 0x3fa,
        SellPrice1 = 0x185,
        SellPrice10 = 0x18e,
        SellPrice2 = 390,
        SellPrice3 = 0x187,
        SellPrice4 = 0x188,
        SellPrice5 = 0x189,
        SellPrice6 = 0x18a,
        SellPrice7 = 0x18b,
        SellPrice8 = 0x18c,
        SellPrice9 = 0x18d,
        SellPriceYtm1 = 0x199,
        SellPriceYtm10 = 0x1a2,
        SellPriceYtm2 = 410,
        SellPriceYtm3 = 0x19b,
        SellPriceYtm4 = 0x19c,
        SellPriceYtm5 = 0x19d,
        SellPriceYtm6 = 0x19e,
        SellPriceYtm7 = 0x19f,
        SellPriceYtm8 = 0x1a0,
        SellPriceYtm9 = 0x1a1,
        SellReportNum = 0x67,
        SellSmall = 0x3fb,
        SellSuper = 0x3f8,
        SellVolume1 = 0x19,
        SellVolume10 = 0x22,
        SellVolume10Delta = 0x36,
        SellVolume1Delta = 0x2d,
        SellVolume2 = 0x1a,
        SellVolume2Delta = 0x2e,
        SellVolume3 = 0x1b,
        SellVolume3Delta = 0x2f,
        SellVolume4 = 0x1c,
        SellVolume4Delta = 0x30,
        SellVolume5 = 0x1d,
        SellVolume5Delta = 0x31,
        SellVolume6 = 30,
        SellVolume6Delta = 50,
        SellVolume7 = 0x1f,
        SellVolume7Delta = 0x33,
        SellVolume8 = 0x20,
        SellVolume8Delta = 0x34,
        SellVolume9 = 0x21,
        SellVolume9Delta = 0x35,
        SerialNumber = 0x2328,
        SettlementPrice = 0x171,
        SHMarketPlanFlag = 0x77,
        StockActiveList_BuyAmount = 0x363,
        StockActiveList_Count = 0x7f,
        StockActiveList_HBuyAmount = 0x365,
        StockActiveList_HCount = 0x80,
        StockActiveList_HNetAmount = 0x367,
        StockActiveList_HSellAmount = 870,
        StockActiveList_IBuyAmount = 0x368,
        StockActiveList_ICount = 0x81,
        StockActiveList_INetAmount = 0x36a,
        StockActiveList_ISellAmount = 0x369,
        StockActiveList_SellAmount = 0x364,
        StockHotList_Amount = 0x360,
        StockHotList_BuyMoney = 0x36e,
        StockHotList_BuySeat = 0x72,
        StockHotList_Close = 0x24a,
        StockHotList_DifferRange = 0x24b,
        StockHotList_NetMoney = 880,
        StockHotList_Reason = 0x4f6,
        StockHotList_ReasonID = 0x71,
        StockHotList_SellMoney = 0x36f,
        StockHotList_SellSeat = 0x73,
        StockNum = 0x3b,
        StockTagEnum = 0x69,
        StockTagText = 0x4ee,
        StockType = 0x2334,
        SubSecurityAmount = 0x35d,
        SubSecurityCode = 0x4f1,
        SubSecurityFlowCap = 0x3f3,
        SubSecurityGrowth = 0x222,
        SubSecurityName = 0x4ef,
        SubSecurityPreClose = 0x237,
        SubSecurityPrice = 0x221,
        SubSecurityTurnover = 0x22a,
        SubSecurityUniCode = 0x70,
        SubSecurityVolume = 0x3f2,
        SumBuyVolume = 8,
        SumSellVolume = 9,
        SumVol2 = 3,
        SumVol5 = 4,
        SZMuJiXZ = 0x254,
        SZThreshold = 0x25b,
        TBStockLayer = 0x7b,
        TBStockLayerStr = 0x512,
        TBStockTradeType = 0x7a,
        TCurLiab = 0x33f,
        TheoreticalPrice = 0x232,
        Theta = 560,
        Time = 1,
        TimedValue = 0x224,
        TLKJ = 0x1e4,
        TLongLiab = 0x340,
        TopPercent = 0x45,
        TopStock = 0x3a,
        TotalLiab = 830,
        TotalYieldRate = 0x27d,
        TradeBtn = 0x2335,
        TradeCode = 0x506,
        TradeType = 0x89,
        TradingStatus = 0x8f,
        Turnover = 0x163,
        Turnover3D = 0x164,
        Turnover6D = 0x166,
        UniCode = 0x61,
        UpDay = 0x43,
        UpLimit = 370,
        UpNum = 5,
        UpRange = 0x167,
        ValueCallAuction = 70,
        VCMDate = 0x91,
        VCMDate1 = 0x94,
        VCMEndTime = 0x93,
        VCMEndTime1 = 150,
        VCMFlag = 0x510,
        VCMLowerPrice = 0x28b,
        VCMLowerPrice1 = 0x28e,
        VCMReferencePrice = 650,
        VCMReferencePrice1 = 0x28d,
        VCMStartTime = 0x92,
        VCMStartTime1 = 0x95,
        VCMUpperPrice = 0x28c,
        VCMUpperPrice1 = 0x28f,
        Vega = 0x22f,
        Volatility120Day = 0x235,
        Volatility20Day = 0x233,
        Volatility250Day = 0x236,
        Volatility60Day = 0x234,
        Volume = 0x3e8,
        VolumeAvgDay5 = 0x3f1,
        VolumeRatio = 0x161,
        Weibi = 0x325,
        Weicha = 2,
        XZMuJiXZ = 0x255,
        XZThreshold = 0x25c,
        YieldfFixed = 0x503,
        YTC_YTP = 0x24c,
        YTMAndBP = 0x4dd,
        ZCFZL = 0x341,
        ZengCangRange = 420,
        ZengCangRangeDay10 = 0x1a7,
        ZengCangRangeDay3 = 0x1a5,
        ZengCangRangeDay5 = 0x1a6,
        ZengCangRank = 0x47,
        ZengCangRankChange = 0x48,
        ZengCangRankChangeDay10 = 0x51,
        ZengCangRankChangeDay3 = 0x4b,
        ZengCangRankChangeDay5 = 0x4e,
        ZengCangRankDay10 = 80,
        ZengCangRankDay3 = 0x4a,
        ZengCangRankDay5 = 0x4d,
        ZengCangRankHis = 0x49,
        ZengCangRankHisDay10 = 0x52,
        ZengCangRankHisDay3 = 0x4c,
        ZengCangRankHisDay5 = 0x4f,
        ZGB = 0x326,
        ZGDiffRange = 480,
        ZGJZ = 0x1e2,
        ZGYJL = 0x1e8,
        ZQPJ = 0x4b8,
        ZQYE = 0x34a,
        ZSZ = 0x327,
        ZTPJ = 0x4b7,
        ZTPremiumRate = 0x257,
        ZuoShi_Aoumt = 0x36c,
        ZuoShi_AveragePirce = 0x26b,
        ZuoShi_BuyCode = 0x401,
        ZuoShi_IsFirstTrader = 0x502,
        ZuoShi_MarketCap = 0x36d,
        ZuoShi_PriceAgio = 620,
        ZuoShi_ProfitRatio = 0x26a,
        ZuoShi_StartDate = 0x85,
        ZuoShi_TotalStock = 0x86,
        ZuoShi_TraderName = 0x501,
        ZuoShi_ZGBRatio = 0x269,
        ZXL = 0x220,
        ZYYWLR = 0x335,
        ZYYWSR = 0x330,
        ZZC = 0x33a,
        //#region Int32(0到299)
        ///// <summary>
        ///// 日期
        ///// </summary>
        //Date = 0,
        ///// <summary>
        ///// 时间
        ///// </summary>
        //Time,
        ///// <summary>
        ///// 委差
        ///// </summary>
        //Weicha,

        ///// <summary>
        ///// 前两日成交量
        ///// </summary>
        //SumVol2,
        ///// <summary>
        ///// 前5日成交量
        ///// </summary>
        //SumVol5,

        ///// <summary>
        ///// 指数上涨家数
        ///// </summary>
        //UpNum,
        ///// <summary>
        ///// 指数平盘家数
        ///// </summary>
        //EqualNum,
        ///// <summary>
        ///// 指数下跌家数
        ///// </summary>
        //DownNum,
        ///// <summary>
        ///// 委买总量
        ///// </summary>
        //SumBuyVolume,
        ///// <summary>
        ///// 委卖总量
        ///// </summary>
        //SumSellVolume,

        ///// <summary>
        ///// 日增仓
        ///// </summary>
        //OpenInterestDaily,
        ///// <summary>
        ///// 增仓
        ///// </summary>
        //Increase,
        ///// <summary>
        ///// ddx连续飘红天数
        ///// </summary>
        //DDXRedFollowDay,
        ///// <summary>
        ///// ddx连续5日飘红天数
        ///// </summary>
        //DDXRedFollowDay5,
        ///// <summary>
        ///// ddx连续10日飘红天数
        ///// </summary>
        //DDXRedFollowDay10,
        ///// <summary>
        ///// 委买一量
        ///// </summary>
        //BuyVolume1,

        ///// <summary>
        ///// 委买二量
        ///// </summary>
        //BuyVolume2,
        ///// <summary>
        ///// 委买三量
        ///// </summary>
        //BuyVolume3,
        ///// <summary>
        ///// 委买四量
        ///// </summary>
        //BuyVolume4,
        ///// <summary>
        ///// 委买五量
        ///// </summary>
        //BuyVolume5,
        ///// <summary>
        ///// 委买六量
        ///// </summary>
        //BuyVolume6,
        ///// <summary>
        ///// 委买七量
        ///// </summary>
        //BuyVolume7,
        ///// <summary>
        ///// 委买八量
        ///// </summary>
        //BuyVolume8,
        ///// <summary>
        ///// 委买九量
        ///// </summary>
        //BuyVolume9,
        ///// <summary>
        ///// 委买十量
        ///// </summary>
        //BuyVolume10,
        ///// <summary>
        ///// 委卖一量
        ///// </summary>
        //SellVolume1,
        ///// <summary>
        ///// 委卖二量
        ///// </summary>
        //SellVolume2,
        ///// <summary>
        ///// 委卖三量
        ///// </summary>
        //SellVolume3,
        ///// <summary>
        ///// 委卖四量
        ///// </summary>
        //SellVolume4,
        ///// <summary>
        ///// 委卖五量
        ///// </summary>
        //SellVolume5,
        ///// <summary>
        ///// 委卖六量
        ///// </summary>
        //SellVolume6,
        ///// <summary>
        ///// 委卖七量
        ///// </summary>
        //SellVolume7,
        ///// <summary>
        ///// 委卖八量
        ///// </summary>
        //SellVolume8,
        ///// <summary>
        ///// 委卖九量
        ///// </summary>
        //SellVolume9,
        ///// <summary>
        ///// 委卖十量
        ///// </summary>
        //SellVolume10,
        ///// <summary>
        ///// 委买一量差值
        ///// </summary>
        //BuyVolume1Delta,
        ///// <summary>
        ///// 委买二量差值
        ///// </summary>
        //BuyVolume2Delta,
        ///// <summary>
        ///// 委买三量差值
        ///// </summary>
        //BuyVolume3Delta,
        ///// <summary>
        ///// 委买四量差值
        ///// </summary>
        //BuyVolume4Delta,
        ///// <summary>
        ///// 委买五量差值
        ///// </summary>
        //BuyVolume5Delta,
        ///// <summary>
        ///// 委买六量差值
        ///// </summary>
        //BuyVolume6Delta,
        ///// <summary>
        ///// 委买七量差值
        ///// </summary>
        //BuyVolume7Delta,
        ///// <summary>
        ///// 委买八量差值
        ///// </summary>
        //BuyVolume8Delta,
        ///// <summary>
        ///// 委买九量差值
        ///// </summary>
        //BuyVolume9Delta,
        ///// <summary>
        ///// 委买十量差值
        ///// </summary>
        //BuyVolume10Delta,
        ///// <summary>
        ///// 委卖一量差值
        ///// </summary>
        //SellVolume1Delta,
        ///// <summary>
        ///// 委卖二量差值
        ///// </summary>
        //SellVolume2Delta,
        ///// <summary>
        ///// 委卖三量差值
        ///// </summary>
        //SellVolume3Delta,
        ///// <summary>
        ///// 委卖四量差值
        ///// </summary>
        //SellVolume4Delta,
        ///// <summary>
        ///// 委卖五量差值
        ///// </summary>
        //SellVolume5Delta,
        ///// <summary>
        ///// 委卖六量差值
        ///// </summary>
        //SellVolume6Delta,
        ///// <summary>
        ///// 委卖七量差值
        ///// </summary>
        //SellVolume7Delta,
        ///// <summary>
        ///// 委卖八量差值
        ///// </summary>
        //SellVolume8Delta,
        ///// <summary>
        ///// 委卖九量差值
        ///// </summary>
        //SellVolume9Delta,
        ///// <summary>
        ///// 委卖十量差值
        ///// </summary>
        //SellVolume10Delta,
        ///// <summary>
        ///// 市场类型
        ///// </summary>
        //Market,
        ///// <summary>
        ///// 上市日期
        ///// </summary>
        //ListDate,
        ///// <summary>
        ///// 报告期日期
        ///// </summary>
        //BGQDate,

													
        ///// <summary>
        ///// 领涨股
        ///// </summary>
        //TopStock,									
        							
        /////// <summary>
        /////// 市盈率
        /////// </summary>
        ////PeRatio,	
								
        ///// <summary>
        ///// 股票个数
        ///// </summary>
        //StockNum,

        ///// <summary>
        ///// 4日净流入天数
        ///// </summary>
        //NetInFlowRedDay4,
        ///// <summary>
        ///// 5日内净流入>0天数
        ///// </summary>
        //NetFlowRedDay5,
        ///// <summary>
        ///// 19日净流入天数
        ///// </summary>
        //NetInFlowRedDay19,
        ///// <summary>
        ///// 20日内净流入>0天数
        ///// </summary>
        //NetFlowRedDay20,
        ///// <summary>
        ///// 59日净流入天数
        ///// </summary>
        //NetInFlowRedDay59,
        ///// <summary>
        ///// 60日内净流入>0天数
        ///// </summary>
        //NetFlowRedDay60,


        ///// <summary>
        ///// 连红天数
        ///// </summary>
        //RedDay,                                                  
        ///// <summary>
        ///// 连涨天数
        ///// </summary>
        //UpDay,	
        ///// <summary>
        ///// 历史连涨天数
        ///// </summary>
        //HisUpDay,						    
        ///// <summary>
        ///// 领涨股涨跌幅
        ///// </summary>
        //TopPercent,      
        ///// <summary>
        ///// 集合竞价成交额
        ///// </summary>
        //ValueCallAuction,               
        ///// <summary>
        ///// 当日增仓排名
        ///// </summary>
        //ZengCangRank,
        ///// <summary>
        ///// 当日增仓排名变化
        ///// </summary>
        //ZengCangRankChange,
        ///// <summary>
        ///// 当日增仓历史排名
        ///// </summary>
        //ZengCangRankHis,
        ///// <summary>
        ///// 3日增仓排名
        ///// </summary>
        //ZengCangRankDay3,
        ///// <summary>
        ///// 3日增仓排名变化
        ///// </summary>
        //ZengCangRankChangeDay3,
        ///// <summary>
        ///// 3日增仓历史排名
        ///// </summary>
        //ZengCangRankHisDay3,
        ///// <summary>
        ///// 5日增仓排名
        ///// </summary>
        //ZengCangRankDay5,
        ///// <summary>
        ///// 5日增仓排名变化
        ///// </summary>
        //ZengCangRankChangeDay5,
        ///// <summary>
        ///// 5日增仓历史排名
        ///// </summary>
        //ZengCangRankHisDay5,
        ///// <summary>
        ///// 10日增仓排名
        ///// </summary>
        //ZengCangRankDay10,
        ///// <summary>
        ///// 10日增仓排名变化
        ///// </summary>
        //ZengCangRankChangeDay10,
        ///// <summary>
        ///// 10日增仓历史排名
        ///// </summary>
        //ZengCangRankHisDay10,
        ///// <summary>
        ///// 特大单买入（金额）
        ///// </summary>
        //BuySuper,
        ///// <summary>
        ///// 大单买入（金额）
        ///// </summary>
        //BuyBig,
        ///// <summary>
        ///// 中单买入（金额）
        ///// </summary>
        //BuyMiddle,
        ///// <summary>
        ///// 小单买入（金额）
        ///// </summary>
        //BuySmall,
        ///// <summary>
        ///// 特大单卖出（金额）
        ///// </summary>
        //SellSuper,
        ///// <summary>
        ///// 大单卖出（金额）
        ///// </summary>
        //SellBig,
        ///// <summary>
        ///// 中单卖出（金额）
        ///// </summary>
        //SellMiddle,
        ///// <summary>
        ///// 小单卖出 （金额）
        ///// </summary>
        //SellSmall,
        ///// <summary>
        ///// 特大单净额
        ///// </summary>
        //NetFlowSuper,
        ///// <summary>
        ///// 大单净额
        ///// </summary>
        //NetFlowBig,
        ///// <summary>
        ///// 中单净额
        ///// </summary>
        //NetFlowMiddle,
        ///// <summary>
        ///// 小单净额
        ///// </summary>
        //NetFlowSmall,
        ///// <summary>
        ///// 主力净流入(金额)
        ///// </summary>
        //MainNetFlow,

        ///// <summary>
        ///// 买卖方向(股指期货)
        ///// </summary>
        //BSFlag,
        ///// <summary>
        ///// 日增仓(股指期货)
        ///// </summary>
        //DayOI,
        ///// <summary>
        ///// 债券交易日期
        ///// </summary>
        //BondDate,

        ///// <summary>
        ///// 到期日期
        ///// </summary>
        //BondMrtydate,
        ///// <summary>
        ///// 最新行权日
        ///// </summary>
        //BondExerciseDay,
        ///// <summary>
        ///// 中债估值日期
        ///// </summary>
        //BondCBDate,
        ///// <summary>
        ///// 中证估值日期
        ///// </summary>
        //BondCSDate,
        ///// <summary>
        ///// 上清所估值日期
        ///// </summary>
        //BondSNDate,
        ///// <summary>
        ///// 基金最新净值对应日期
        ///// </summary>
        //FundLatestDate,
        ///// <summary>
        ///// 成立日期
        ///// </summary>
        //FundFounddate,
        ///// <summary>
        ///// 到期日期
        ///// </summary>
        //FundEnddate,
        ///// <summary>
        ///// 评级家数
        ///// </summary>
        //InvestGradeNum, 
        ///// <summary>
        ///// 基金经理任职日期
        ///// </summary>
        //FundMgrPostDate,
        ///// <summary>
        ///// 基金经理离职日期
        ///// </summary>
        //FundMgrLeaveDate,
        ///// <summary>
        ///// 内码
        ///// </summary>
        //UniCode,

        ///// <summary>
        ///// 研报数
        ///// </summary>
        //ReportNum,
        ///// <summary>
        ///// 买入评级家数
        ///// </summary>
        //BuyReportNum,
        ///// <summary>
        ///// 增持评级家数
        ///// </summary>
        //AddReportNum,
        ///// <summary>
        ///// 中性评级家数
        ///// </summary>
        //NeuturReportNum,
        ///// <summary>
        ///// 减持评级家数
        ///// </summary>
        //ReduceReportNum,
        ///// <summary>
        ///// 卖出评级家数
        ///// </summary>
        //SellReportNum,
        ///// <summary>
        ///// 所属行业Code
        ///// </summary>
        //IndustryBlockCode,
        ///// <summary>
        ///// 股票标记，类型是StockTag
        ///// </summary>
        //StockTagEnum,
        //#endregion

        //#region float(300-799)
        ////float 300
        ///// <summary>
        ///// 现价
        ///// </summary>
        //Now = 300,
        ///// <summary>
        ///// 涨跌金额
        ///// </summary>
        //Difference,
        ///// <summary>
        ///// 涨跌幅
        ///// </summary>
        //DifferRange,
        ///// <summary>
        ///// 3日涨跌幅
        ///// </summary>
        //DifferRange3D,
        ///// <summary>
        ///// 5日涨跌幅
        ///// </summary>
        //DifferRange5D,
        ///// <summary>
        ///// 6日涨跌幅
        ///// </summary>
        //DifferRange6D,
        ///// <summary>
        ///// 10日涨跌幅
        ///// </summary>
        //DifferRange10D,
        ///// <summary>
        ///// 20日涨跌幅
        ///// </summary>
        //DifferRange20D,
        ///// <summary>
        ///// 60日涨跌幅
        ///// </summary>
        //DifferRange60D,
        ///// <summary>
        ///// 120日涨跌幅
        ///// </summary>
        //DifferRange120D,
        ///// <summary>
        ///// 250日涨跌幅
        ///// </summary>
        //DifferRange250D,
        ///// <summary>
        ///// 年初至今涨跌幅
        ///// </summary>
        //DifferRangeYTD,
        ///// <summary>
        ///// 1分钟涨跌幅
        ///// </summary>
        //DifferRange1Mint,
        ///// <summary>
        ///// 3分钟涨跌幅
        ///// </summary>
        //DifferRange3Mint,
        ///// <summary>
        ///// 5分钟涨跌幅
        ///// </summary>
        //DifferRange5Mint,
        ///// <summary>
        ///// 龙头股1的涨跌幅
        ///// </summary>
        //DifferRangeLead1,
        ///// <summary>
        ///// 龙头股2的涨跌幅
        ///// </summary>
        //DifferRangeLead2,
        ///// <summary>
        ///// 龙头股3的涨跌幅
        ///// </summary>
        //DifferRangeLead3,
        ///// <summary>
        ///// 涨速s
        ///// </summary>
        //DifferSpeed,
        ///// <summary>
        ///// 均价
        ///// </summary>
        //AveragePrice,
        ///// <summary>
        ///// 5日均价
        ///// </summary>
        //RateAvgPrice5D,
        ///// <summary>
        ///// 10日均价
        ///// </summary>
        //RateAvgPrice10D,
        ///// <summary>
        ///// 20日均价
        ///// </summary>
        //RateAvgPrice20D,
        ///// <summary>
        ///// 60日均价
        ///// </summary>
        //RateAvgPrice60D,
        ///// <summary>
        ///// 120日均价
        ///// </summary>
        //RateAvgPrice120D,
        ///// <summary>
        ///// 250日均价
        ///// </summary>
        //RateAvgPrice250D,

        ///// <summary>
        ///// 5日均价
        ///// </summary>
        //RateAvgPricePre4D,
        ///// <summary>
        ///// 10日均价
        ///// </summary>
        //RateAvgPricePre9D,
        ///// <summary>
        ///// 20日均价
        ///// </summary>
        //RateAvgPricePre19D,
        ///// <summary>
        ///// 60日均价
        ///// </summary>
        //RateAvgPricePre59D,
        ///// <summary>
        ///// 120日均价
        ///// </summary>
        //RateAvgPricePre119D,
        ///// <summary>
        ///// 250日均价
        ///// </summary>
        //RateAvgPricePre249D,

        ///// <summary>
        ///// 昨收
        ///// </summary>
        //PreClose,
        ///// <summary>
        ///// 3日收盘价
        ///// </summary>
        //PreCloseDay3,
        ///// <summary>
        ///// 5日收盘价
        ///// </summary>
        //PreCloseDay5,
        ///// <summary>
        ///// 10日收盘价
        ///// </summary>
        //PreCloseDay10,
        ///// <summary>
        ///// 20日收盘价
        ///// </summary>
        //PreCloseDay20,
        ///// <summary>
        ///// 60日收盘价
        ///// </summary>
        //PreCloseDay60,
        ///// <summary>
        ///// 250日收盘价
        ///// </summary>
        //PreCloseDay120,
        ///// <summary>
        ///// 250日收盘价
        ///// </summary>
        //PreCloseDay250,
        ///// <summary>
        ///// 年初至今日收盘价
        ///// </summary>
        //PreCloseDayYTD,
        ///// <summary>
        ///// 52周收盘价
        ///// </summary>
        //PreCloseWeek52,
        ///// <summary>
        ///// 开盘价
        ///// </summary>
        //Open,
        ///// <summary>
        ///// 最高价
        ///// </summary>
        //High,
        ///// <summary>
        ///// 最低价
        ///// </summary>
        //Low,
        ///// <summary>
        ///// 52周最高价
        ///// </summary>
        //HighW52,
        ///// <summary>
        ///// 52周最低价
        ///// </summary>
        //LowW52,
        ///// <summary>
        ///// 量比
        ///// </summary>
        //VolumeRatio,
        ///// <summary>
        ///// 振幅
        ///// </summary>
        //Delta,
        ///// <summary>
        ///// 换手率
        ///// </summary>
        //Turnover,
        ///// <summary>
        ///// 3天换手率
        ///// </summary>
        //Turnover3D,
        ///// <summary>
        ///// 5日换手率
        ///// </summary>
        //AgvTurnoverDay5,
        ///// <summary>
        ///// 6天换手率
        ///// </summary>
        //Turnover6D,
        ///// <summary>
        ///// 上涨比率
        ///// </summary>
        //UpRange,
        ///// <summary>
        ///// 市盈率(最新报告期),财富通个股使用
        ///// </summary>
        //PE,
        ///// <summary>
        ///// 收益率(TTM)
        ///// </summary>
        //PETTM,
        ///// <summary>
        ///// 市盈率(最近年报)
        ///// </summary>
        //PELYR,
        ///// <summary>
        ///// 当年市盈率预测
        ///// </summary>
        //PECurrentYear,
        ///// <summary>
        ///// 后一年市盈率预测
        ///// </summary>
        //PENextYear1,
        ///// <summary>
        ///// 后两年市盈率预测
        ///// </summary>
        //PENextYear2,
        ///// <summary>
        ///// 后三年市盈率预测
        ///// </summary>
        //PENextYear3,
        ///// <summary>
        ///// 市净率
        ///// </summary>
        //PB,
			
        ///// <summary>
        ///// 昨结算价
        ///// </summary>
        //PreSettlementPrice,
        ///// <summary>
        ///// 结算价
        ///// </summary>
        //SettlementPrice,
        ///// <summary>
        ///// 涨停
        ///// </summary>
        //UpLimit,
        ///// <summary>
        ///// 跌停
        ///// </summary>
        //DownLimit,

        ///// <summary>
        ///// 当日DDX
        ///// </summary>
        //DDX,
        ///// <summary>
        ///// 当日DDY
        ///// </summary>
        //DDY,
        ///// <summary>
        ///// 当日DDZ
        ///// </summary>
        //DDZ,
        ///// <summary>
        ///// 5日DDX
        ///// </summary>
        //DDX5,
        ///// <summary>
        ///// 5日DDY
        ///// </summary>
        //DDY5,
        ///// <summary>
        ///// 10日DDX
        ///// </summary>
        //DDX10,
        ///// <summary>
        ///// 10日DDY
        ///// </summary>
        //DDY10,
        ///// <summary>
        ///// 买一价
        ///// </summary>
        //BuyPrice1,
        ///// <summary>
        ///// 买二价
        ///// </summary>
        //BuyPrice2,
        ///// <summary>
        ///// 买三价
        ///// </summary>
        //BuyPrice3,
        ///// <summary>
        ///// 买四价
        ///// </summary>
        //BuyPrice4,
        ///// <summary>
        ///// 买五价
        ///// </summary>
        //BuyPrice5,
        ///// <summary>
        ///// 买六价
        ///// </summary>
        //BuyPrice6,
        ///// <summary>
        ///// 买七价
        ///// </summary>
        //BuyPrice7,
        ///// <summary>
        ///// 买八价
        ///// </summary>
        //BuyPrice8,
        ///// <summary>
        ///// 买九价
        ///// </summary>
        //BuyPrice9,
        ///// <summary>
        ///// 买十价
        ///// </summary>
        //BuyPrice10,
        ///// <summary>
        ///// 卖一价
        ///// </summary>
        //SellPrice1,
        ///// <summary>
        ///// 卖二价
        ///// </summary>
        //SellPrice2,
        ///// <summary>
        ///// 卖三价
        ///// </summary>
        //SellPrice3,
        ///// <summary>
        ///// 卖四价
        ///// </summary>
        //SellPrice4,
        ///// <summary>
        ///// 卖五价
        ///// </summary>
        //SellPrice5,
        ///// <summary>
        ///// 卖六价
        ///// </summary>
        //SellPrice6,
        ///// <summary>
        ///// 卖七价
        ///// </summary>
        //SellPrice7,
        ///// <summary>
        ///// 卖八价
        ///// </summary>
        //SellPrice8,
        ///// <summary>
        ///// 卖九价
        ///// </summary>
        //SellPrice9,
        ///// <summary>
        ///// 卖十价
        ///// </summary>
        //SellPrice10,
        ///// <summary>
        ///// 买一价
        ///// </summary>
        //BuyPriceYtm1,
        ///// <summary>
        ///// 买二价
        ///// </summary>
        //BuyPriceYtm2,
        ///// <summary>
        ///// 买三价
        ///// </summary>
        //BuyPriceYtm3,
        ///// <summary>
        ///// 买四价
        ///// </summary>
        //BuyPriceYtm4,
        ///// <summary>
        ///// 买五价
        ///// </summary>
        //BuyPriceYtm5,
        ///// <summary>
        ///// 买六价
        ///// </summary>
        //BuyPriceYtm6,
        ///// <summary>
        ///// 买七价
        ///// </summary>
        //BuyPriceYtm7,
        ///// <summary>
        ///// 买八价
        ///// </summary>
        //BuyPriceYtm8,
        ///// <summary>
        ///// 买九价
        ///// </summary>
        //BuyPriceYtm9,
        ///// <summary>
        ///// 买十价
        ///// </summary>
        //BuyPriceYtm10,
        ///// <summary>
        ///// 卖一价
        ///// </summary>
        //SellPriceYtm1,
        ///// <summary>
        ///// 卖二价
        ///// </summary>
        //SellPriceYtm2,
        ///// <summary>
        ///// 卖三价
        ///// </summary>
        //SellPriceYtm3,
        ///// <summary>
        ///// 卖四价
        ///// </summary>
        //SellPriceYtm4,
        ///// <summary>
        ///// 卖五价
        ///// </summary>
        //SellPriceYtm5,
        ///// <summary>
        ///// 卖六价
        ///// </summary>
        //SellPriceYtm6,
        ///// <summary>
        ///// 卖七价
        ///// </summary>
        //SellPriceYtm7,
        ///// <summary>
        ///// 卖八价
        ///// </summary>
        //SellPriceYtm8,
        ///// <summary>
        ///// 卖九价
        ///// </summary>
        //SellPriceYtm9,
        ///// <summary>
        ///// 卖十价
        ///// </summary>
        //SellPriceYtm10,
        ///// <summary>
        ///// 内外比
        ///// </summary>
        //NeiWaiBi,
        ///// <summary>
        ///// 当日增仓占比
        ///// </summary>
        //ZengCangRange,
        ///// <summary>
        ///// 3日增仓占比
        ///// </summary>
        //ZengCangRangeDay3,
        ///// <summary>
        ///// 5日增仓占比
        ///// </summary>
        //ZengCangRangeDay5,
        ///// <summary>
        ///// 10日增仓占比
        ///// </summary>
        //ZengCangRangeDay10,
        ///// <summary>
        ///// 特大单净占比
        ///// </summary>
        //NetFlowRangeSuper,
        ///// <summary>
        ///// 大单净占比
        ///// </summary>
        //NetFlowRangeBig,
        ///// <summary>
        ///// 中单净占比
        ///// </summary>
        //NetFlowRangeMiddle,
        ///// <summary>
        ///// 小单净占比
        ///// </summary>
        //NetFlowRangeSmall,
        ///// <summary>
        ///// 特大单买入占比
        ///// </summary>
        //BuyFlowRangeSuper,
        ///// <summary>
        ///// 大单买入占比
        ///// </summary>
        //BuyFlowRangeBig,
        ///// <summary>
        ///// 特大单卖出占比
        ///// </summary>
        //SellFlowRangeSuper,
        ///// <summary>
        ///// 大单卖出占比
        ///// </summary>
        //SellFlowRangeBig,
        ///// <summary>
        ///// 净流入率
        ///// </summary>
        //NetFlowRange,
        ///// <summary>
        ///// 净流入率5天
        ///// </summary>
        //NetFlowRangeDay5,
        ///// <summary>
        ///// 净流入率20天
        ///// </summary>
        //NetFlowRangeDay20,
        ///// <summary>
        ///// 净流入率60天
        ///// </summary>
        //NetFlowRangeDay60,
        ///// <summary>
        ///// 对中间价涨跌BP
        ///// </summary>
        //MedDiffer,
        ///// <summary>
        ///// 对中间价涨跌幅
        ///// </summary>
        //MedDifferRange,
        ///// <summary>
        ///// 5日均量
        ///// </summary>
        //VolumeAvgDay5,
        ///// <summary>
        ///// 净利润同比
        ///// </summary>
        //NetProfitYOY,
        ///// <summary>
        ///// 毛利率
        ///// </summary>
        //GrossProfitMargin,
        ///// <summary>
        ///// 净利率
        ///// </summary>
        //NetProfitMargin,
        ///// <summary>
        ///// 负债率
        ///// </summary>
        //DebtRatio,
        ///// <summary>
        ///// 最新票面利率
        ///// </summary>
        //BondNewrate,
        ///// <summary>
        ///// 麦式久期
        ///// </summary>
        //BondDuration,
        ///// <summary>
        ///// 修正久期
        ///// </summary>
        //BondMduration,
        ///// <summary>
        ///// 凸性
        ///// </summary>
        //BondConvexity,
        ///// <summary>
        ///// 基点价值
        ///// </summary>
        //BondBasisvalue,
        ///// <summary>
        ///// 应计利息
        ///// </summary>
        //BondAI,
        ///// <summary>
        ///// 前收盘全价
        ///// </summary>
        //BondLfclose,
        ///// <summary>
        ///// 前收盘净价
        ///// </summary>
        //BondLcclose,
        ///// <summary>
        ///// 前收（YTM）
        ///// </summary>
        //BondDecLcytm,
        ///// <summary>
        ///// 上日均价
        ///// </summary>
        //BondDecLcavg,
        ///// <summary>
        ///// 剩余期限(年)
        ///// </summary>
        //BondTomrtyyear,
        ///// <summary>
        ///// 上日加权YTM
        ///// </summary>
        //BondDecLavgytm,
        ///// <summary>
        ///// 最新YTM
        ///// </summary>
        //BondNowYTM,
        ///// <summary>
        ///// 最新加权YTM
        ///// </summary>
        //BondDecNowYTM,
        ///// <summary>
        ///// YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTM,
        ///// <summary>
        ///// 5日YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTMDay5,
        ///// <summary>
        ///// 10日YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTMDay10,
        ///// <summary>
        ///// 20日YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTMDay20,
        ///// <summary>
        ///// 60日YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTMDay60,
        ///// <summary>
        ///// 120日YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTMDay120,
        ///// <summary>
        ///// 250日YTM涨跌BP
        ///// </summary>
        //BondDiffRangeYTMDay250,
        ///// <summary>
        ///// 债券期限(年)
        ///// </summary>
        //BondBondperiod,
        ///// <summary>
        ///// 加权YTM涨跌BP
        ///// </summary>
        //BondDecDiffRangeYTM,
        ///// <summary>
        ///// 均价涨跌
        ///// </summary>
        //BondAvgDiffer,
        ///// <summary>
        ///// 最新价（全价）
        ///// </summary>
        //BondFullNow,
        ///// <summary>
        ///// 最新价（净价）
        ///// </summary>
        //BondNetNow,
        ///// <summary>
        ///// 开盘价（全价）
        ///// </summary>
        //BondFullOpen,
        ///// <summary>
        ///// 开盘价（净价）
        ///// </summary>
        //BondNetOpen,
        ///// <summary>
        ///// 开盘价（收益率）
        ///// </summary>
        //BondYTMOpen,
        ///// <summary>
        ///// 最高价（全价）
        ///// </summary>
        //BondFullHigh,
        ///// <summary>
        ///// 最高价（净价）
        ///// </summary>
        //BondNetHigh,
        ///// <summary>
        ///// 最高价（收益率）
        ///// </summary>
        //BondYTMHigh,
        ///// <summary>
        ///// 最低价（全价）
        ///// </summary>
        //BondFullLow,
        ///// <summary>
        ///// 最低价（净价）
        ///// </summary>
        //BondNetLow,
        ///// <summary>
        ///// 最低价（收益率）
        ///// </summary>
        //BondYTMLow,
        ///// <summary>
        ///// 均价（收益率）
        ///// </summary>
        //BondYTMAvg,
        ///// <summary>
        ///// 加权平均收益率
        ///// </summary>
        //BondDecYTMAvg,
        ///// <summary>
        ///// 正股价格
        ///// </summary>
        //BondStockPrice,
        ///// <summary>
        ///// 正股涨跌幅
        ///// </summary>
        //ZGDiffRange,
        ///// <summary>
        ///// 正股换手率
        ///// </summary>
        //BondStockTurnover,
        ///// <summary>
        ///// 转股价值
        ///// </summary>
        //ZGJZ,
        ///// <summary>
        ///// 转股比例
        ///// </summary>
        //BondConversionRate,
        ///// <summary>
        ///// 套利空间
        ///// </summary>
        //TLKJ,
        ///// <summary>
        ///// 纯债溢价率
        ///// </summary>
        //CZYJL,
        ///// <summary>
        ///// 平价低价溢价率
        ///// </summary>
        //PJYJL,
        ///// <summary>
        ///// 转股价格
        ///// </summary>
        //BondSwapPrice,
        ///// <summary>
        ///// 转股溢价率
        ///// </summary>
        //ZGYJL,
        ///// <summary>
        ///// 纯债价值
        ///// </summary>
        //CZJZ,
        ///// <summary>
        ///// 最优卖出净价
        ///// </summary>
        //BondBestSellNet,
        ///// <summary>
        ///// 最优卖出净价收益率
        ///// </summary>
        //BondBestSellYtm,
        
        ///// <summary>
        ///// 最优买入净价
        ///// </summary>
        //BondBestBuyNet,
        ///// <summary>
        ///// 最优买入净价收益率
        ///// </summary>
        //BondBestBuyYtm,

        ///// <summary>
        ///// 中债估价净价
        ///// </summary>
        //BondCBNet,
        ///// <summary>
        ///// 中债估价净价收益率
        ///// </summary>
        //BondCBYtm,
        ///// <summary>
        ///// 中证估价净价
        ///// </summary>
        //BondCSNet,
        ///// <summary>
        ///// 中证估价净价收益率
        ///// </summary>
        //BondCSYtm,
        ///// <summary>
        ///// 上清所估价净价
        ///// </summary>
        //BondSNNet,
        ///// <summary>
        ///// 上清所估价净价收益率
        ///// </summary>
        //BondSNYtm,
        ///// <summary>
        ///// 加权平均利率
        ///// </summary>
        //RateDecAvgPrice,
        ///// <summary>
        ///// 前加权
        ///// </summary>
        //RatePreDecPrice,
        ///// <summary>
        ///// 投资评级
        ///// </summary>
        //InvestGrade,
        ///// <summary>
        ///// 基金单位净值
        ///// </summary>
        //FundPernav,
        ///// <summary>
        ///// 上个交易日单位净值
        ///// </summary>
        //FundNvper1,
        ///// <summary>
        ///// 日回报
        ///// </summary>
        //FundNvgrwtd,
        ///// <summary>
        ///// 年初至今复权单位净值增长率
        ///// </summary>
        //FundNvgrty,
        ///// <summary>
        ///// 近一周复权单位净值增长率
        ///// </summary>
        //FundNvgrw1w,
        ///// <summary>
        ///// 近一月复权单位净值增长率
        ///// </summary>
        //FundNvgr4w,
        ///// <summary>
        ///// 近一季复权单位净值增长率
        ///// </summary>
        //FundNvgr13w,
        ///// <summary>
        ///// 近半年复权单位净值增长率
        ///// </summary>
        //FundNvgr26w,
        ///// <summary>
        ///// 近一年复权单位净值增长率
        ///// </summary>
        //FundNvgr52w,
        ///// <summary>
        ///// 近两年复权单位净值增长率
        ///// </summary>
        //FundNvgr104w,
        ///// <summary>
        ///// 近三年复权单位净值增长率
        ///// </summary>
        //FundNvgr156w,
        ///// <summary>
        ///// 近五年复权单位净值增长率
        ///// </summary>
        //FundNvgr208w,
        ///// <summary>
        ///// 成立至今复权单位净值增长率
        ///// </summary>
        //FundNvgrf,
        ///// <summary>
        ///// 年化回报
        ///// </summary>
        //FundDecYearhb,
        ///// <summary>
        ///// 最新规模（亿元）
        ///// </summary>
        //FundNav,
        ///// <summary>
        ///// 最新涨幅
        ///// </summary>
        //FundDecZdf,
        ///// <summary>
        ///// 累计净值
        ///// </summary>
        //FundAccunav,
        ///// <summary>
        ///// 场内流通份额
        ///// </summary>
        //FundCshare,
        ///// <summary>
        ///// 贴水率
        ///// </summary>
        //FundAgior,
        ///// <summary>
        ///// 最新净值增长率
        ///// </summary>
        //FundNetZZL,
        ///// <summary>
        ///// 平均年化收益率
        ///// </summary>
        //FundAvgIncomeYear,
        ///// <summary>
        ///// 7日年化收益率
        ///// </summary>
        //FundIncomeYear7D,
        ///// <summary>
        ///// 年化收益
        ///// </summary>
        //FundIncomeYear,
        ///// <summary>
        ///// 万份基金收益
        ///// </summary>
        //FundIncome10K,
        ///// <summary>
        ///// 当年收益率
        ///// </summary>
        //FundIncomeYear1,
        ///// <summary>
        ///// 前一年收益率
        ///// </summary>
        //FundIncomeYear2,
        ///// <summary>
        ///// 前二年收益率
        ///// </summary>
        //FundIncomeYear3,
        ///// <summary>
        ///// 前三年收益率
        ///// </summary>
        //FundIncomeYear4,
        ///// <summary>
        ///// 前四年收益率
        ///// </summary>
        //FundIncomeYear5,
        ///// <summary>
        ///// 前五年收益率
        ///// </summary>
        //FundIncomeYear6,

        ///// <summary>
        ///// 基金重仓持股的持仓量
        ///// </summary>
        //FundHoldShare,
        ///// <summary>
        ///// 基金重仓持股的持仓市值
        ///// </summary>
        //FundHoldValue,
        ///// <summary>
        ///// 基金重仓持股的占股票市值比
        ///// </summary>
        //FundStockValueRange,
        ///// <summary>
        ///// 基金重仓债券的占净值比
        ///// </summary>
        //FundNetValueRange,
        ///// <summary>
        ///// 基金重仓持仓占债券比
        ///// </summary>
        //FundBondRange,
        ///// <summary>
        ///// 基金相对市场配置
        ///// </summary>
        //FundScpz,
        ///// <summary>
        ///// 基金经理任职期间总回报
        ///// </summary>
        //FundMgrYieldSince,
        ///// <summary>
        ///// 超基准总回报
        ///// </summary>
        //FundCyjzhb,
        ///// <summary>
        ///// 基金经理年化回报
        ///// </summary>
        //FundManagerAyieldSinces,
        //#endregion

        //#region double(800-999)
        ////Double 800
        ///// <summary>
        ///// 成交金额
        ///// </summary>
        //Amount = 800,
        ///// <summary>
        ///// 指数成交金额
        ///// </summary>
        //IndexAmount,
        ///// <summary>
        ///// 4日总成交金额
        ///// </summary>
        //AmountDay4,
        ///// <summary>
        ///// 19日总成交金额
        ///// </summary>
        //AmountDay19,
        ///// <summary>
        ///// 59日总成交金额
        ///// </summary>
        //AmountDay59,
        ///// <summary>
        ///// 委比
        ///// </summary>
        //Weibi,
        ///// <summary>
        ///// 总股本
        ///// </summary>
        //ZGB,
        ///// <summary>
        ///// 总市值
        ///// </summary>
        //ZSZ,
        ///// <summary>
        ///// 流通市值
        ///// </summary>
        //LTSZ,
        ///// <summary>
        ///// 人均持股数
        ///// </summary>
        //AvgNetS,
        /// <summary>
        /// 每股收益（最新报告期）
        /// </summary>
        MGSY,
        ///// <summary>
        ///// 每股收益（最新摊薄/除汇率后）
        ///// </summary>
        //MGSY2, 
        ///// <summary>
        ///// 每股收益（最新年报）
        ///// </summary>
        EpsQmtby,
        /// <summary>
        /// 每股收益（最新报告期）
        /// </summary>
        EpsTtm
        /// <summary>
        /// 每股净资产（最新摊薄/除汇率后）
        /// </summary>
        //MGJZC2,
        ///// <summary>
        ///// 净资产收益率（加权）
        ///// </summary>
        //JZC,
        ///// <summary>
        ///// 营业收入
        ///// </summary>
        //ZYYWSR,
        ///// <summary>
        ///// 营业收入同比
        ///// </summary>
        //IncomeRatio,
        ///// <summary>
        ///// 营业利润
        ///// </summary>
        //ProfitFO,
        ///// <summary>
        ///// 投资收益
        ///// </summary>
        //InvIncome,
        ///// <summary>
        ///// 利润总额
        ///// </summary>
        //PBTax,
        ///// <summary>
        ///// 净利润
        ///// </summary>
        //ZYYWLR,
        ///// <summary>
        ///// 净利润同比
        ///// </summary>
        //NPRatio,
        ///// <summary>
        ///// 未分配利润
        ///// </summary>
        //RProfotAA,
        ///// <summary>
        ///// 每股未分配利润（摊薄）
        ///// </summary>
        //DRPRPAA,
        ///// <summary>
        ///// 毛利率
        ///// </summary>
        //Gprofit,
        ///// <summary>
        ///// 总资产
        ///// </summary>
        //ZZC,
        ///// <summary>
        ///// 流动资产
        ///// </summary>
        //CurAsset,
        ///// <summary>
        ///// 固定资产
        ///// </summary>
        //FixAsset,
        ///// <summary>
        ///// 无形资产
        ///// </summary>
        //IntAsset,
        ///// <summary>
        ///// 总负债
        ///// </summary>
        //TotalLiab,
        ///// <summary>
        ///// 流动负债
        ///// </summary>
        //TCurLiab,
        ///// <summary>
        ///// 非流动负债
        ///// </summary>
        //TLongLiab,
        ///// <summary>
        ///// 资产负债比率
        ///// </summary>
        //ZCFZL,
        ///// <summary>
        ///// 股东权益
        ///// </summary>
        //OWnerEqu,
        ///// <summary>
        ///// 股东权益比
        ///// </summary>
        //OEquRatio,
        ///// <summary>
        ///// 资本公积金
        ///// </summary>
        //CapRes,
        ///// <summary>
        ///// 每股资本公积金(摊薄）
        ///// </summary>
        //DRPCapRes,
        ///// <summary>
        ///// 流通A股
        ///// </summary>
        //NetAShare,
        ///// <summary>
        ///// 流通B股
        ///// </summary>
        //NetBShare,
        ///// <summary>
        ///// H股
        ///// </summary>
        //Hshare,
        /////// <summary>
        /////// 正股涨跌幅
        /////// </summary>
        ////ZGDiffRange,
        /////// <summary>
        /////// 纯债价值
        /////// </summary>
        ////CZJZ,
        /////// <summary>
        /////// 套利空间
        /////// </summary>
        ////TLKJ,
        ///// <summary>
        ///// 到期收益率
        ///// </summary>
        //DQSYL,
        /////// <summary>
        /////// 转股溢价率
        /////// </summary>
        ////ZGYJL,
        /////// <summary>
        /////// 纯债溢价率
        /////// </summary>
        ////CZYJL,
        /////// <summary>
        /////// 平价底价溢价率
        /////// </summary>
        ////PJYJL,
        /////// <summary>
        /////// 剩余期限
        /////// </summary>
        ////SYQX,
        ///// <summary>
        ///// 债券余额(万元)
        ///// </summary>
        //ZQYE,
        /////// <summary>
        /////// 转股价值
        /////// </summary>
        ////ZGJZ,
        ///// <summary>
        ///// 平均收益double
        ///// </summary>
        //AvgProfit,
        ///// <summary>
        ///// 平均股本double
        ///// </summary>
        //AvgShare,
        ///// <summary>
        ///// 流通股
        ///// </summary>
        //Ltg,
        ///// <summary>
        ///// 净流入
        ///// </summary>
        //NetFlow,
        ///// <summary>
        ///// 净流入4日
        ///// </summary>
        //NetFlowDay4,
        ///// <summary>
        ///// 净流入5日
        ///// </summary>
        //NetFlowDay5,
        ///// <summary>
        ///// 净流入19日
        ///// </summary>
        //NetFlowDay19,
        ///// <summary>
        ///// 净流入20日
        ///// </summary>
        //NetFlowDay20,
        ///// <summary>
        ///// 净流入59日
        ///// </summary>
        //NetFlowDay59,
        ///// <summary>
        ///// 净流入60日
        ///// </summary>
        //NetFlowDay60,
        ///// <summary>
        ///// 最新份额（亿）
        ///// </summary>
        //FundLastestShare,

        ///// <summary>
        ///// 当年每股收益预测
        ///// </summary>
        //EpsCurrentYear,
        ///// <summary>
        ///// 后一年每股收益预测
        ///// </summary>
        //EpsNextYear1,
        ///// <summary>
        ///// 后两年每股收益预测
        ///// </summary>
        //EpsNextYear2,
        ///// <summary>
        ///// 后三年每股收益预测
        ///// </summary>
        //EpsNextYear3,
        ///// <summary>
        ///// 最优买入券面总额
        ///// </summary>
        //BondBestBuyTotalFaceValue,
        ///// <summary>
        ///// 最优卖出券面总额
        ///// </summary>
        //BondBestSellTotalFaceValue,
        //#endregion

        //#region long(1000-1199)
        ////long (1000-1199)
        ///// <summary>
        /////成交量（股）
        ///// </summary>
        //Volume = 1000,
        ///// <summary>
        ///// 指数成交手数
        ///// </summary>
        //IndexVolume,
        ///// <summary>
        /////现手
        ///// </summary>
        //LastVolume,
        ///// <summary>
        ///// 内盘
        ///// </summary>
        //GreenVolume,
        ///// <summary>
        ///// 外盘
        ///// </summary>
        //RedVolume,
        ///// <summary>
        /////5日均量
        ///// </summary>
        //Evg5Volume,
        ///// <summary>
        ///// 昨持仓
        ///// </summary>
        //PreOpenInterest,
        ///// <summary>
        ///// 持仓
        ///// </summary>
        //OpenInterest,
        ///// <summary>
        ///// 增仓(股指期货)
        ///// </summary>
        //CurOI,
        //#endregion

        //#region String(1200--8999)
        ////String 1200
        ///// <summary>
        ///// 股票代码
        ///// </summary>
        //Code = 1200,
        ///// <summary>
        ///// 东财指数
        ///// </summary>
        //EMCode,
        ///// <summary>
        ///// 股票名称
        ///// </summary>
        //Name,
        ///// <summary>
        ///// 暂时未用
        ///// </summary>
        //SecuCode,
        ///// <summary>
        ///// 拼音简称
        ///// </summary>
        //ChiSpelling,

        ///// <summary>
        ///// 所属行业名称
        ///// </summary>
        //IndustryBlockName,

        ///// <summary>
        ///// 开平性质
        ///// </summary>
        //OpenCloseStatus,
        ///// <summary>
        ///// 主体评级
        ///// </summary>
        //ZTPJ,
        ///// <summary>
        ///// 债券评级
        ///// </summary>
        //ZQPJ,
        ///// <summary>
        ///// 龙头股1
        ///// </summary>
        //MarketValueTop1Code,
        ///// <summary>
        ///// 龙头股2
        ///// </summary>
        //MarketValueTop2Code,
        ///// <summary>
        ///// 龙头股3
        ///// </summary>
        //MarketValueTop3Code,

        ///// <summary>
        ///// 债券市场
        ///// </summary>
        //BondMarket,
        ///// <summary>
        ///// 债券类型
        ///// </summary>
        //BondType,
        ///// <summary>
        ///// 债券评级(最新)
        ///// </summary>
        //BondStrZqpj,
        ///// <summary>
        ///// 主体评级(最新)
        ///// </summary>
        //BondStrZtpj,
        ///// <summary>
        ///// 特殊条款
        ///// </summary>
        //BondStrTstk,
        ///// <summary>
        ///// 发行人
        ///// </summary>
        //BondInstname,
        ///// <summary>
        ///// 发行人详细
        ///// </summary>
        //BondInstDetail,
        ///// <summary>
        ///// 发行人企业性质
        ///// </summary>
        //BondInstType,
        ///// <summary>
        ///// 发行人行业
        ///// </summary>
        //BondInstIndustry,
        ///// <summary>
        ///// 最优卖出报价机构
        ///// </summary>
        //BondBestSellInitiator,
        ///// <summary>
        ///// 最优买入报价机构
        ///// </summary>
        //BondBestBuyInitiator,
        ///// <summary>
        ///// 晨星评级
        ///// </summary>
        //FundStrCxrank3y,
        ///// <summary>
        ///// 银河评级
        ///// </summary>
        //FundNvgrthl3year,


        ///// <summary>
        ///// 投资类型
        ///// </summary>
        //FundParaname,
        ///// <summary>
        ///// 申赎状态
        ///// </summary>
        //FundStrSgshzt,
        ///// <summary>
        ///// 基金公司代码
        ///// </summary>
        //FundManagercode,
        ///// <summary>
        ///// 基金公司
        ///// </summary>
        //FundManagername,
        ///// <summary>
        ///// 托管银行
        ///// </summary>
        //FundStrTgrcom,
        ///// <summary>
        ///// 托管银行代码
        ///// </summary>
        //FundTgrcode,
        ///// <summary>
        ///// 基金经理
        ///// </summary>
        //FundFmanager,
        ///// <summary>
        ///// 交易所
        ///// </summary>
        //FundStrTexch,
        ///// <summary>
        ///// 证券名称
        ///// </summary>
        //FundName,
        ///// <summary>
        ///// 英文简称
        ///// </summary>
        //FundNameEn,
        ///// <summary>
        ///// 基金所属行业指数代码
        ///// </summary>
        //FundIndustryIndexShortCode,
        ///// <summary>
        ///// 同类排名
        ///// </summary>
        //FundRank,
        ///// <summary>
        ///// 当年收益率排名
        ///// </summary>
        //FundIncomeRankYear1,
        ///// <summary>
        ///// 前一年收益率排名
        ///// </summary>
        //FundIncomeRankYear2,
        ///// <summary>
        ///// 前二年收益率排名
        ///// </summary>
        //FundIncomeRankYear3,
        ///// <summary>
        ///// 前三年收益率排名
        ///// </summary>
        //FundIncomeRankYear4,
        ///// <summary>
        ///// 前四年收益率排名
        ///// </summary>
        //FundIncomeRankYear5,
        ///// <summary>
        ///// 前五年收益率排名
        ///// </summary>
        //FundIncomeRankYear6,
        ///// <summary>
        ///// 受托人
        ///// </summary>
        //FundTrusteName,
        ///// <summary>
        ///// 投资顾问
        ///// </summary>
        //FundManagerName,
        ///// <summary>
        ///// 收益率/BP
        ///// </summary>
        //YTMAndBP,
        ///// <summary>
        ///// 开盘 净价/ytm
        ///// </summary>
        //BondNetYTMOpen,
        ///// <summary>
        ///// 最高价 净价/ytm
        ///// </summary>
        //BondNetYTMHigh,
        ///// <summary>
        ///// 均价 净价/ytm
        ///// </summary>
        //BondDecAvgNetDecAvgYTM,
        ///// <summary>
        ///// 最低价 净价/ytm
        ///// </summary>
        //BondNetYTMLow,
        ///// <summary>
        ///// 是否担保
        ///// </summary>
        //BondIsVouch,
        ///// <summary>
        ///// 交易市场
        ///// </summary>
        //BondTradeMarket,
        ///// <summary>
        ///// 利率说明
        ///// </summary>
        //Ratedes,
        ///// <summary>
        ///// 机构名称
        ///// </summary>
        //InsSName,
        ///// <summary>
        ///// 信息编码
        ///// </summary>
        //InfoCode,

        ///// <summary>
        ///// 国债期货公开市场操作--日期
        ///// </summary>
        //BondFuturePublicOperate_BondDate,
        ///// <summary>
        ///// 国债期货公开市场操作--利率(%)
        ///// </summary>
        //BondFuturePublicOperate_CouponRate,
        ///// <summary>
        ///// 国债期货公开市场操作--实际发行总量(亿元)
        ///// </summary>
        //BondFuturePublicOperate_IssueVol,
        ///// <summary>
        ///// 国债期货公开市场操作--期限
        ///// </summary>
        //BondFuturePublicOperate_Period,
        ///// <summary>
        ///// 国债期货公开市场操作--操作类型
        ///// </summary>
        //BondFuturePublicOperate_OType,
        //#endregion

        //#region other
        ////system 9000
        ///// <summary>
        ///// 序号
        ///// </summary>
        //SerialNumber = 9000,
       
        ///// <summary>
        ///// 自定义类型,信息地雷
        ///// </summary>
        //InfoMine,

        ///// <summary>
        ///// bool,是否在我的自选股中
        ///// </summary>
        //IsDefaultCustomStock,

        ///// <summary>
        ///// 股票标记String内容
        ///// </summary>
        //StockTagText,
        ///// <summary>
        ///// 重仓持股内码，List<int>
        ///// </summary>
        //FundHeaveStock,
        ///// <summary>
        ///// 重仓行业内码，List<int>
        ///// </summary>
        //FundHeaveHY,
        ///// <summary>
        ///// 重仓债券内码，List<int>
        ///// </summary>
        //FundKeyBond,
        ///// <summary>
        ///// 重仓基金内码，List<int>
        ///// </summary>
        //FinanceHeaveFund,
        ///// <summary>
        ///// 基金经理内码，List<FundManagerDataRec>
        ///// </summary>
        //FundHeaveManager,
        ///// <summary>
        ///// 所属板块的内码 
        ///// </summary>
        //AllBlockCode,
        ///// <summary>
        ///// 无
        ///// </summary>
        //Na
        //#endregion
    }

    /// <summary>
    /// 服务端个股字段id
    /// </summary>
    public enum ReqFieldIndex
    {
        /// <summary>
        /// 股票代码,通常不需要订阅
        /// </summary>
        Code,

        //byte
        /// <summary>
        /// 市场信息,通常不需要订阅
        /// </summary>
        Market,
		
		/// <summary>
        /// 证券类别
		/// </summary>						
        Type,

        /// <summary>
        /// 
        /// </summary>						 
        CurVolByte,

        //uint32
        /// <summary>
        /// 股票唯一标识，必须订阅！
        /// </summary>
        StockID,

        /// <summary>
        /// 开盘价
        /// </summary>
        Open,   
        /// <summary>
        /// 最高价
        /// </summary>
        High,
        /// <summary>
        /// 最低价
        /// </summary>
        Low,
        /// <summary>
        /// 昨收价
        /// </summary>
        PreClose,
        /// <summary>
        /// 最新价
        /// </summary>
        Now,
        /// <summary>
        /// 委买一价
        /// </summary>
        BuyPrice1,
        /// <summary>
        /// 委卖一价
        /// </summary>
        SellPrice1,
        /// <summary>
        /// 成交量
        /// </summary>
        Volume,
        /// <summary>
        /// 成交额
        /// </summary>
        Amount,
        /// <summary>
        /// 现手?
        /// </summary>
        CurVol,
        /// <summary>
        /// 量比
        /// </summary>
        LiangBi,
        /// <summary>
        /// 内盘
        /// </summary>
        NeiPan,
        /// <summary>
        /// 流通股
        /// </summary>
        LTG,
        /// <summary>
        /// 前2日成交量的和(算3日换手)
        /// </summary>
        SumVol2,
        /// <summary>
        /// 前5日成交量的和(算6日换手)
        /// </summary>
        SumVol5,
        /// <summary>
        /// 2日前收盘价(算3日涨跌幅)
        /// </summary>
        PreCloseDay2,
        /// <summary>
        /// 5日前收盘价(算6日涨跌幅)
        /// </summary>
        PreCloseDay5,
        /// <summary>
        /// 委买总量
        /// </summary>
        SumBuyVol,
        /// <summary>
        /// 委卖总量
        /// </summary>
        SumSellVol,
        /// <summary>
        /// 委买一量
        /// </summary>
        BuyVol1,
        /// <summary>
        /// 委卖一量
        /// </summary>
        SellVol1,
        /// <summary>
        /// 昨持仓量
        /// </summary>
        PreOpenInterest,
        /// <summary>
        /// 昨结算
        /// </summary>
        PreSettlementPrice,
        /// <summary>
        /// 持仓量
        /// </summary>
        OpenInterest,
        /// <summary>
        /// 今结算
        /// </summary>
        SettlementPrice,
        /// <summary>
        /// 集合竞价成交额
        /// </summary>
        ValueCallAuction,
        /// <summary>
        /// 当日DDX
        /// </summary>
        DDX,
        /// <summary>
        /// 当日DDY
        /// </summary>
        DDY,
        /// <summary>
        /// 当日DDZ
        /// </summary>
        DDZ,
        /// <summary>
        /// 5日DDX
        /// </summary>
        DDX5,
        /// <summary>
        /// 5日DDY
        /// </summary>
        DDY5,
        /// <summary>
        /// 10日DDX
        /// </summary>
        DDX10,
        /// <summary>
        /// 10日DDY
        /// </summary>
        DDY10,
        /// <summary>
        /// DDX连续飘红天数
        /// </summary>
        DDXRedFollowDay,
        /// <summary>
        /// 5日内DDX飘红天数
        /// </summary>
        DDXRedDay5,
        /// <summary>
        /// 10日内DDX飘红天数
        /// </summary>
        DDXRedDay10,

        /// <summary>
        /// 
        /// </summary>
        PrevPrice1,

        /// <summary>
        /// 今日增仓排名
        /// </summary>
        SListFlowItemDay,
        /// <summary>
        /// 3日增仓排名
        /// </summary>
        SListFlowItemDay3,
        /// <summary>
        /// 5日增仓排名
        /// </summary>
        SListFlowItemDay5,
        /// <summary>
        /// 10日增仓排名
        /// </summary>
        SListFlowItemDay10,

        /// <summary>
        /// 小单买入卖出%
        /// </summary>
        SListFlowDetailItemSmall,
        /// <summary>
        /// 中单买入卖出%
        /// </summary>
        SListFlowDetailItemMiddle,
        /// <summary>
        /// 大单买入卖出%
        /// </summary>
        SListFlowDetailItemBig,
        /// <summary>
        /// 特大单买入卖出%
        /// </summary>
        SListFlowDetailItemSuper,

        

    }

    /// <summary>
    /// 服务端板块字段id
    /// </summary>
    public enum ReqBlockFieldIndex
    {
        /// <summary>
        /// 市场信息
        /// </summary>
        Market,
		/// <summary>
		/// 证券类别
		/// </summary>
	    Type,										
        /// <summary>
        /// 唯一编号
        /// </summary>
	    StockID,
        /// <summary>
        /// 开盘价
        /// </summary>
	    Open,                               
        /// <summary>
        /// 最高价
        /// </summary>
	    High,              
        /// <summary>
        /// 最低价
        /// </summary>
	    Low,                                
        /// <summary>
        /// 昨收价
        /// </summary>
	    PreClose,				                
        /// <summary>
        /// 最新价
        /// </summary>
	    Now,
        /// <summary>
        /// 成交量
        /// </summary>
	    Volume,
        /// <summary>
        /// 成交额
        /// </summary>
	    Amount,
        /// <summary>
        /// 平均收益
        /// </summary>
	    AvgProfit,							
        /// <summary>
        /// 平局股本
        /// </summary>
	    AvgShare,								
        /// <summary>
        /// 换手率
        /// </summary>
	    Turnover,							
        /// <summary>
        /// 5日换手率
        /// </summary>
	    AgvTurnoverDay5,						
        /// <summary>
        /// 5日涨跌幅
        /// </summary>
	    DiffRangeDay5,							
	    /// <summary>
	    /// 领涨股
	    /// </summary>
	    TopStock,								
        /// <summary>
        /// 总市值
        /// </summary>
	    ZSZ,								
        /// <summary>
        /// 流通市值
        /// </summary>
	    LTSZ,						
        /// <summary>
        /// 市盈率
        /// </summary>
	    PeRatio,							
        /// <summary>
        /// 股票个数
        /// </summary>
	    StockNum,							
        /// <summary>
        /// 上涨个数
        /// </summary>
	    UpNum,                               
        /// <summary>
        /// 下跌个数
        /// </summary>
	    DownNum,						
        /// <summary>
        /// 净流入
        /// </summary>
	    NetFlow,	                       
        /// <summary>
        /// 涨跌幅
        /// </summary>
	    DiffRange,                   
        /// <summary>
        /// 连帐天数
        /// </summary>
	    UpDay,								
        /// <summary>
        /// 领涨股涨跌幅
        /// </summary>
	    TopPercent,                      

        /// <summary>
        /// 集合竞价成交额
        /// </summary>
	    ValueCallAuction, // 集合竞价成交额
        /// <summary>
        /// ddx
        /// </summary>
	    DDX,
        /// <summary>
        /// ddy
        /// </summary>
	    DDY,
        /// <summary>
        /// ddz
        /// </summary>
	    DDZ,
        /// <summary>
        /// 5日ddx
        /// </summary>
	    DDX5,
        /// <summary>
        /// 5日ddy
        /// </summary>
	    DDY5,
        /// <summary>
        /// 10日ddx
        /// </summary>
	    DDX10,
        /// <summary>
        /// 10日ddy
        /// </summary>
	    DDY10,  
        /// <summary>
        /// ddx连续飘红
        /// </summary>
	    DDXRedFollowDay,
        /// <summary>
        /// 5日ddx连续飘红
        /// </summary>
	    DDXRedDay5,
        /// <summary>
        /// 10日ddx连续飘红
        /// </summary>
	    DDXRedDay10,
        /// <summary>
        /// 5日价格？
        /// </summary>
	    PrevPrice1,
        /// <summary>
        /// 当日增仓排名
        /// </summary>
	    SListFlowItemDay,
		/// <summary>
        /// 3日增仓排名
		/// </summary>	
	    SListFlowItemDay3,
		/// <summary>
        /// 5日增仓排名
		/// </summary>
	    SListFlowItemDay5,
		/// <summary>
        /// 10日增仓排名
		/// </summary>
	    SListFlowItemDay10,
        /// <summary>
        /// 小单资金流向
        /// </summary>
	    SListFlowDetailItemSmall,
        /// <summary>
        /// 中单资金流向
        /// </summary>
	    SListFlowDetailItemMiddle,
        /// <summary>
        /// 大单资金流向
        /// </summary>
	    SListFlowDetailItemBig,
        /// <summary>
        /// 特单资金流向
        /// </summary>
	    SListFlowDetailItemSuper,
    }

    /// <summary>
    /// 服务端报价增仓排名
    /// </summary>
    public struct ListFlowItem
    {
        /// <summary>
        /// 大单净占比
        /// </summary>
        public int PercentDec;

        /// <summary>
        /// 历史大单净占比排名
        /// </summary>
        public int HisPercentDecRange;

        /// <summary>
        /// 涨跌幅
        /// </summary>
        public int DiffRanger;
    }

    /// <summary>
    /// 报价资金流向
    /// </summary>
    public struct ListFlowDetailItem
    {
        /// <summary>
        /// 买入
        /// </summary>
        public int Buy;

        /// <summary>
        /// 卖出
        /// </summary>
        public int Sell;
    }


    /// <summary>
    /// 市场报价行情推送字段
    /// </summary>
    public enum PushQuoteFieldIndex
    {
        /// <summary>
        /// 股票id
        /// </summary>
        StockID,
        /// <summary>
        /// 
        /// </summary>
        Code,
        /// <summary>
        /// 市场信息
        /// </summary>
	    Market,
        /// <summary>
        /// 证券类别
        /// </summary>
        Type,
        /// <summary>
        /// 开盘价
        /// </summary>
        Open, 
        /// <summary>
        /// 最高价
        /// </summary>
        High, 
        /// <summary>
        /// 最低价
        /// </summary>
        Low, 
        /// <summary>
        /// 前收价
        /// </summary>
        Close,	
        /// <summary>
        /// 最新价
        /// </summary>
        Now, 
        /// <summary>
        /// 委买一价
        /// </summary>
        BuyPrice1, 
        /// <summary>
        /// 委卖一价
        /// </summary>
        SellPrice1,                           
        /// <summary>
        /// 
        /// </summary>
        Volume,
        /// <summary>
        /// 
        /// </summary>
        Amount,
        /// <summary>
        /// 
        /// </summary>
        CurVol,
        /// <summary>
        /// 
        /// </summary>
        cCurVol,
        /// <summary>
        /// 
        /// </summary>
        LiangBi,

        /// <summary>
        /// 内盘
        /// </summary>
        NeiPan,	

        /// <summary>
        /// 流通股数
        /// </summary>
        LTG, 

        /// <summary>
        /// 前2日成交量的和(算3日换手)
        /// </summary>
        SumVol2,
        /// <summary>
        /// 前5日成交量的和(算6日换手)
        /// </summary>
        SumVol5,
        /// <summary>
        /// 2日前收盘价(算3日涨跌幅)
        /// </summary>
        PreCloseDay2,
        /// <summary>
        /// 5日前收盘价(算6日涨跌幅)
        /// </summary>
        PreCloseDay5,
        /// <summary>
        /// 
        /// </summary>
        PrevPrice,
        /// <summary>
        /// 委买总量
        /// </summary>
        SumBuyVol,
        /// <summary>
        /// 委卖总量
        /// </summary>
        SumSellVol, 
        /// <summary>
        /// 委买一量
        /// </summary>
        BuyVol1, 
        /// <summary>
        /// 委卖一量
        /// </summary>
        SellVol1,
        /// <summary>
        /// 昨持仓量
        /// </summary>
 	    PreOpenInterest,
        /// <summary>
        /// 昨结算 
        /// </summary>
        PreSettlementPrice,
        /// <summary>
        /// 持仓量
        /// </summary>
        OpenInterest,
        /// <summary>
        /// 今结算
        /// </summary>
        SettlementPrice,	
    }

    /// <summary>
    /// 板块报价行情推送字段
    /// </summary>
    public enum PushBlockQuoteFieldIndex
    {
        /// <summary>
        /// 
        /// </summary>
        StockID,
        /// <summary>
        /// 市场信息
        /// </summary>
	    Market,						
        /// <summary>
        /// 证券类别
        /// </summary>
        Type,				
        /// <summary>
        /// 开盘价
        /// </summary>
        Open,            
        /// <summary>
        /// 最高价
        /// </summary>
        High,              
        /// <summary>
        /// 最低价
        /// </summary>
        Low,              
        /// <summary>
        /// 前收价	
        /// </summary>
        PreClose,		
        /// <summary>
        /// 
        /// </summary>
        Now,
        /// <summary>
        /// 
        /// </summary>
        Volume,
        /// <summary>
        /// 
        /// </summary>
        Amount,
        /// <summary>
        /// 平均收益
        /// </summary>
	    AvgProfit,								   
        /// <summary>
        /// 平均股本
        /// </summary>
        AvgShare,			
        /// <summary>
        /// 换手率
        /// </summary>
        Turnover,				
        /// <summary>
        /// 5日换手
        /// </summary>
        AgvTurnoverDay5,					
        /// <summary>
        /// 5日涨跌	
        /// </summary>
	    DifferRangeDay5,						
        /// <summary>
        /// 领涨股
        /// </summary>
        TopStock,								
        /// <summary>
        /// 总市值
        /// </summary>
        ZSZ,							
        /// <summary>
        /// 流通市值
        /// </summary>
        LTSZ,								
        /// <summary>
        /// 市盈率
        /// </summary>
        PeRatio,						
        /// <summary>
        /// 股票个数
        /// </summary>
        StockNum,							
        /// <summary>
        /// 上涨个数
        /// </summary>
        UpNum,                                  
        /// <summary>
        /// 下跌个数
        /// </summary>
        DownNum,						
        /// <summary>
        /// 连涨天数
        /// </summary>
        UpDay,								
        /// <summary>
        /// 
        /// </summary>
        PrevPrice,
    }

    /// <summary>
    /// 报价DDE推送字段
    /// </summary>
    public enum PushDDEFieldIndex
    {
        /// <summary>
        /// 
        /// </summary>
        StockID,
        /// <summary>
        /// 
        /// </summary>
        Now,
        /// <summary>
        /// 
        /// </summary>
        PreClose,
        /// <summary>
        /// 
        /// </summary>
        Amount,
        /// <summary>
        /// 
        /// </summary>
        DDX,
        /// <summary>
        /// 
        /// </summary>
        DDY,
        /// <summary>
        /// 
        /// </summary>
        DDZ,
        /// <summary>
        /// 
        /// </summary>
        DDX5,
        /// <summary>
        /// 
        /// </summary>
        DDY5,
        /// <summary>
        /// 
        /// </summary>
        DDX10,
        /// <summary>
        /// 
        /// </summary>
        DDY10,
        /// <summary>
        /// 
        /// </summary>
        DDXRedFollowDay,
        /// <summary>
        /// 
        /// </summary>
        DDXRedDay5,
        /// <summary>
        /// 
        /// </summary>
        DDXRedDay10,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemSmall,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemMiddle,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemBig,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemSuper,    
    }

    /// <summary>
    /// 报价增仓排名推送
    /// </summary>
    public enum PushListFlowFieldIndex
    {
        /// <summary>
        /// 
        /// </summary>
        StockID,
        /// <summary>
        /// 
        /// </summary>
        Now,
        /// <summary>
        /// 
        /// </summary>
        PreClose,
        /// <summary>
        /// 
        /// </summary>
        Volume,
        /// <summary>
        /// 
        /// </summary>
        SListFlowItemDay,
        /// <summary>
        /// 
        /// </summary>
        SListFlowItemDay3,
        /// <summary>
        /// 
        /// </summary>
        SListFlowItemDay5,
        /// <summary>
        /// 
        /// </summary>
        SListFlowItemDay10,
    }

    /// <summary>
    /// 报价资金流向推送
    /// </summary>
    public enum PushListFlowDetailFieldIndex
    {
        /// <summary>
        /// 
        /// </summary>
        StockID,
        /// <summary>
        /// 
        /// </summary>
	    Now,
        /// <summary>
        /// 
        /// </summary>
        PreClose,
        /// <summary>
        /// 
        /// </summary>
        Amount,
        /// <summary>
        /// 
        /// </summary>
	    ValueCallAuction,
        /// <summary>
        /// 
        /// </summary>
	    SListFlowDetailItemSmall,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemMiddle,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemBig,
        /// <summary>
        /// 
        /// </summary>
        SListFlowDetailItemSuper,
    }

    /// <summary>
    /// 股票标记
    /// </summary>
    public enum StockTag
    {
        Na = 0,
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Text
    }


    #region 财富通服务端
    /// <summary>
    /// 服务端字段的类型
    /// </summary>
    public enum ReqFieldIndexType
    {
        FT_NA,
        FT_STOCKID,
        FT_CODE,
        FT_MARKET,
        FT_INT16,
        FT_INT32,
        FT_UINT32,
        FT_BYTE,
        FT_LISTFLOWITEM,
        FT_LISTFLOWDETAILITEM,
        FT_FLOAT,
        FT_FLOATFINANCE,
        FT_LONGFINANCE,
        FT_LONG,
        FT_DOUBLE,
        FT_DOUBLETOFLOAT,
        FT_TOPCODE,
        FT_STRINGORG,
        FT_INTTOLONG,
        FT_FLOATTOINT32,
        FT_FLOATRoundPrice
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_NA,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_STOCKID,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_CODE,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_MARKET,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_INT32,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_UINT32,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_BYTE,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_LISTFLOWITEM,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_LISTFLOWDETAILITEM,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_FLOAT,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_FLOATFINANCE,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_LONGFINANCE,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_LONG,
        ///// <summary>
        ///// 16字节的String
        ///// </summary>
        ////FT_CODEORG,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_DOUBLE,
        ///// <summary>
        ///// 
        ///// </summary>
        //FT_TOPCODE,//龙头股
        ///// <summary>
        ///// 18个字节的String
        ///// </summary>
        ////FT_STRING18,
        //FT_STRINGORG,//先byte，后String
    }


    /// <summary>
    /// 服务端数据转客户端数据时，除以的系数
    /// </summary>
    public enum FieldFactor
    {
        /// <summary>
        /// 不处理
        /// </summary>
        NoFactor,
        /// <summary>
        /// 价格系数，除1000
        /// </summary>
        PriceFactor,
        /// <summary>
        /// 成交量系数，除100转成手
        /// </summary>
        VolumeFactor,
        /// <summary>
        /// 转成百分数，除100
        /// </summary>
        RangeFactor,
        /// <summary>
        /// 最新价，用于计算涨速，除1000
        /// </summary>
        NowPriceFactor,
        /// <summary>
        /// 除10000
        /// </summary>
        BlockDifferRange,
    }
    /// <summary>
    /// 
    /// </summary>
    public class DicFieldIndexItem
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqFieldIndexType FieldTypeServer;
        /// <summary>
        /// 
        /// </summary>
        public FieldIndex FieldClient;
        /// <summary>
        /// 
        /// </summary>
        public FieldFactor FieldFactor;

        /// <summary>
        /// 
        /// </summary>
        public DicFieldIndexItem(ReqFieldIndexType fieldType, FieldIndex fieldClient, FieldFactor factor)
        {
            FieldTypeServer = fieldType;
            FieldClient = fieldClient;
            FieldFactor = factor;
        }
    }

    /// <summary>
    /// 市场报价字段转换
    /// </summary>
    public class ConvertFieldIndex
    {
        static ConvertFieldIndex(){
            DicFieldIndex.Add(ReqFieldIndex.Code,new DicFieldIndexItem(ReqFieldIndexType.FT_CODE,FieldIndex.Code,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.Market,new DicFieldIndexItem(ReqFieldIndexType.FT_BYTE,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.Type,new DicFieldIndexItem(ReqFieldIndexType.FT_BYTE,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.CurVolByte,new DicFieldIndexItem(ReqFieldIndexType.FT_BYTE,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.StockID,new DicFieldIndexItem(ReqFieldIndexType.FT_STOCKID,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.Open,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Open,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.High,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.High,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.Low,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Low,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.PreClose,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.PreClose,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.Now,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Now,FieldFactor.NowPriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.BuyPrice1,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.BuyPrice1,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.SellPrice1,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.SellPrice1,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.Volume,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.Volume,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SumVol2,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.SumVol2,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.Amount,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.Amount,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.CurVol,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.LastVolume,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.LiangBi,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.VolumeRatio,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.NeiPan,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.GreenVolume,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.LTG,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.Ltg,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.PreCloseDay2,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Na,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.PreCloseDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Na,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.SumBuyVol,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.SumBuyVolume,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SumSellVol,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.SumSellVolume,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.BuyVol1,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.BuyVolume1,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SellVol1,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.SellVolume1,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.ValueCallAuction,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.ValueCallAuction,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.PreOpenInterest,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.PreOpenInterest,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.OpenInterest,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.OpenInterest,FieldFactor.NoFactor));
            
            DicFieldIndex.Add(ReqFieldIndex.PreSettlementPrice,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.PreSettlementPrice,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.SettlementPrice,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.SettlementPrice,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDX,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDX,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDY,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDY,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDZ,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDZ,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDX5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDX5,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDY5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDY5,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDX10,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDX10,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDY10,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDY10,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDXRedFollowDay,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDXRedFollowDay,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDXRedDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDXRedFollowDay5,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.DDXRedDay10,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDXRedFollowDay10,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.PrevPrice1,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowItemDay,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowItemDay3,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowItemDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowItemDay10,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowDetailItemBig,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowDetailItemMiddle,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowDetailItemSmall,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndex.SListFlowDetailItemSuper,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));

            PushTypeArray.Add(PushType.Quote, _quoteArray);
            PushTypeArray.Add(PushType.DDE, _ddeArray);
            PushTypeArray.Add(PushType.ListFlow,_listFlowArray);
            PushTypeArray.Add(PushType.FlowDetail,_flowDetailArray);
        }
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<ReqFieldIndex, DicFieldIndexItem> DicFieldIndex = new Dictionary<ReqFieldIndex, DicFieldIndexItem>();
                                                                                
        /// <summary>
        /// 
        /// </summary>
        public enum PushType
        {
            /// <summary>
            /// 
            /// </summary>
            Quote,
            /// <summary>
            /// 
            /// </summary>
            DDE,
            /// <summary>
            /// 
            /// </summary>
            ListFlow,
            /// <summary>
            /// 
            /// </summary>
            FlowDetail
        }
        static readonly short[] _ddeArray = new short[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 };
        static readonly short[] _flowDetailArray = new short[] { 30, 46, 47, 48, 49 };
        static readonly short[] _listFlowArray = new short[] { 42, 43, 44, 45 };
        static readonly short[] _quoteArray = new short[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
		21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 41};

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<PushType, short[]> PushTypeArray = new Dictionary<PushType, short[]>();
        /// <summary>
        /// 
        /// </summary>
        public static bool ConvertPushToReq(PushType pushType, int enumValue, out ReqFieldIndex field)
        {
            try
            {
                switch (pushType)
                {
                    case PushType.Quote:
                        if (Enum.IsDefined(typeof(ReqFieldIndex), ((PushQuoteFieldIndex)enumValue).ToString()))
                        {
                            field =
                                (ReqFieldIndex)
                                Enum.Parse(typeof (ReqFieldIndex), ((PushQuoteFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                    case PushType.DDE:
                        if (Enum.IsDefined(typeof(ReqFieldIndex), ((PushDDEFieldIndex)enumValue).ToString()))
                        {
                            field =
                                (ReqFieldIndex)
                                Enum.Parse(typeof (ReqFieldIndex), ((PushDDEFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                    case PushType.ListFlow:
                        if(Enum.IsDefined(typeof(ReqFieldIndex), ((PushListFlowFieldIndex)enumValue).ToString()))
                        {
                            field =
                                (ReqFieldIndex)
                                Enum.Parse(typeof (ReqFieldIndex), ((PushListFlowFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                    case PushType.FlowDetail:
                        if (Enum.IsDefined(typeof(ReqFieldIndex), ((PushListFlowDetailFieldIndex)enumValue).ToString()))
                        {
                            field =
                                (ReqFieldIndex)
                                Enum.Parse(typeof (ReqFieldIndex), ((PushListFlowDetailFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                field = 0;
                return false;
            }
            field = 0;
            return false;
        }

    }

    /// <summary>
    /// 板块报价字段转换
    /// </summary>
    public class ConvertBlockFieldIndex
    {

        static ConvertBlockFieldIndex(){
            DicFieldIndex.Add(ReqBlockFieldIndex.Market,new DicFieldIndexItem(ReqFieldIndexType.FT_BYTE,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Type,new DicFieldIndexItem(ReqFieldIndexType.FT_BYTE,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.StockID,new DicFieldIndexItem(ReqFieldIndexType.FT_STOCKID,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Open,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Open,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.High,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.High,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Low,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Low,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.PreClose,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.PreClose,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Now,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Now,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Volume,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.Volume,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Amount,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.Amount,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.AvgProfit,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.AvgProfit,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.AvgShare,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.AvgShare,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.Turnover,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.Turnover,FieldFactor.RangeFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.AgvTurnoverDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.AgvTurnoverDay5,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DiffRangeDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DifferRange5D,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.TopStock,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.TopStock,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.ZSZ,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.ZSZ,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.LTSZ,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.LTSZ,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.PeRatio,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.PE,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.StockNum,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.StockNum,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.UpNum,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.UpNum,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DownNum,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DownNum,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.NetFlow,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.NetFlow,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DiffRange,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DifferRange,FieldFactor.BlockDifferRange));
            DicFieldIndex.Add(ReqBlockFieldIndex.UpDay,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.UpDay,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.TopPercent,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.TopPercent,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.ValueCallAuction,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.ValueCallAuction,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDX,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDX,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDY,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDY,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDZ,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDZ,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDX5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDX5,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDY5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDY5,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDX10,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDX10,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDY10,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDY10,FieldFactor.PriceFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDXRedFollowDay,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDXRedFollowDay,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDXRedDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDXRedFollowDay5,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.DDXRedDay10,new DicFieldIndexItem(ReqFieldIndexType.FT_INT32,FieldIndex.DDXRedFollowDay10,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.PrevPrice1,new DicFieldIndexItem(ReqFieldIndexType.FT_UINT32,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowItemDay,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowItemDay3,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowItemDay5,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowItemDay10,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowDetailItemBig,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowDetailItemMiddle,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowDetailItemSmall,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqBlockFieldIndex.SListFlowDetailItemSuper,new DicFieldIndexItem(ReqFieldIndexType.FT_LISTFLOWDETAILITEM,FieldIndex.Na,FieldFactor.NoFactor));

            PushTypeArray.Add(BlockPushType.Quote, _quoteArray);
            PushTypeArray.Add(BlockPushType.DDE, _ddeArray);
            PushTypeArray.Add(BlockPushType.ListFlow,_listFlowArray);
            PushTypeArray.Add(BlockPushType.FlowDetail,_flowDetailArray);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<ReqBlockFieldIndex, DicFieldIndexItem> DicFieldIndex = new Dictionary<ReqBlockFieldIndex, DicFieldIndexItem>();

        /// <summary>
        /// 
        /// </summary>
        public enum BlockPushType
        {
            /// <summary>
            /// 
            /// </summary>
            Quote,
            /// <summary>
            /// DDE
            /// </summary>
            DDE,
            /// <summary>
            /// 
            /// </summary>
            ListFlow,
            /// <summary>
            /// 
            /// </summary>
            FlowDetail
        }
        static readonly short[] _ddeArray = new short[] { 27,28,29,30,31,32,33,34,35,36,37};
        static readonly short[] _flowDetailArray = new short[] { 26, 42,43,44,45 };
        static readonly short[] _listFlowArray = new short[] { 38,39,40,41 };
        static readonly short[] _quoteArray = new short[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
		21, 22, 23, 24, 25};

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<BlockPushType, short[]> PushTypeArray = new Dictionary<BlockPushType, short[]>();
        /// <summary>
        /// 
        /// </summary>
        public static bool ConvertPushToReq(BlockPushType pushType, int enumValue, out ReqBlockFieldIndex field)
        {
            try
            {
                switch (pushType)
                {
                    case BlockPushType.Quote:
                        if (Enum.IsDefined(typeof(ReqBlockFieldIndex), ((PushBlockQuoteFieldIndex)enumValue).ToString()))
                        {

                            field =
                                (ReqBlockFieldIndex)
                                Enum.Parse(typeof (ReqBlockFieldIndex),
                                           ((PushBlockQuoteFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                    case BlockPushType.DDE:
                        if (Enum.IsDefined(typeof(ReqBlockFieldIndex), ((PushDDEFieldIndex)enumValue).ToString()))
                        {
                            field =
                                (ReqBlockFieldIndex)
                                Enum.Parse(typeof (ReqBlockFieldIndex), ((PushDDEFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                    case BlockPushType.ListFlow:
                        if (Enum.IsDefined(typeof(ReqBlockFieldIndex), ((PushListFlowFieldIndex)enumValue).ToString()))
                        {
                            field =
                                (ReqBlockFieldIndex)
                                Enum.Parse(typeof (ReqBlockFieldIndex), ((PushListFlowFieldIndex) enumValue).ToString());
                            return true;
                        }
                        break;
                    case BlockPushType.FlowDetail:
                        if (Enum.IsDefined(typeof(ReqBlockFieldIndex), ((PushListFlowFieldIndex)enumValue).ToString()))
                        {
                            field =
                              (ReqBlockFieldIndex)
                              Enum.Parse(typeof(ReqBlockFieldIndex), ((PushListFlowDetailFieldIndex)enumValue).ToString());
                            return true;
                        }
                        break;
                }
            }
            catch (Exception)
            {

                field = 0;
                return false;
            }
           

            field = 0;
            return false;

        }

    }
    #endregion 

    #region 机构版服务端
    /// <summary>
    /// 
    /// </summary>
    public class ConvertOrgFieldIndex
    {
        public static float ConvertPriceRound(float price, int code)
        {
            MarketType marketType = DllImportHelper.GetMarketType(code);
            return ConvertPriceRound(price, marketType);
        }
        public static float ConvertPriceRound(float price, MarketType mt)
        {
            float num = price;
            switch (mt)
            {
                case MarketType.SHINDEX:
                case MarketType.SZINDEX:
                case MarketType.CircuitBreakerIndex:
                    return Convert.ToSingle(Math.Round((decimal)price, 2, MidpointRounding.AwayFromZero));

                case MarketType.EMINDEX:
                    return num;
            }
            return num;
        }

        static ConvertOrgFieldIndex()
        {
            DicFieldIndex.Add( ReqFieldIndexOrg.Code, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.Code, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Accer, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferSpeed, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Amount, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.Amount, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.AverGB, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.AvgShare, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.AverRevenue, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.AvgProfit, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BlockAvePrice, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.AveragePrice, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice1, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice1, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice2, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice2, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice3, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice4, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice4, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice6, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice6, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice7, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice7, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice8, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice8, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice9, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice9, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyPrice10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyPrice10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume1, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume1, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume2, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume2, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume3, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume4, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume4, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume6, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume6, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume7, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume7, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume8, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume8, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume9, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume9, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyVolume10, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyVolume10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.CurVol, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.LastVolume, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Delta, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Delta, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Differ, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Difference, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRange, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeDay20, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange20D, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange5D, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeDay60, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange60D, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeYTD, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRangeYTD, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DownNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.DownNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FlowShares, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.Ltg, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.High, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.High, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.HighW52, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.HighW52, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.LTSZ, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.LTSZ, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Leader, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.TopStock, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.LiangBi, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.VolumeRatio, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Low, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Low, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.LowW52, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.LowW52, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NeiPan, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.RedVolume, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetInFlow, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlow, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Now, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Now, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Open, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Open, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PB, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PB, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PE, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PETTM, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreClose, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreClose, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay3, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay20, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay20, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay60, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay60, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay120, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay120, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseDay250, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDay250, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseWeek52, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseWeek52, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.PreCloseYTD, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreCloseDayYTD, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeDay10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange10D, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.VolumeAvgDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.VolumeAvgDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.RedDays, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.UpDay, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SameNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.EqualNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice1, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice1, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice2, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice2, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice3, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice4, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice4, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice6, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice6, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice7, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice7, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice8, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice8, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice9, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice9, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellPrice10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellPrice10, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume1, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume1, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume2, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume2, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume3, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume3, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume4, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume4, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume5, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume6, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume6, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume7, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume7, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume8, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume8, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume9, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume9, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellVolume10, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellVolume10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.StockNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.StockNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SumVolumeDay2, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SumVol2, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SumVolumeDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SumVol5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Time, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.Time, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.TotalShares, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.ZGB, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.TotalValue, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.ZSZ, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Turnover, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Turnover, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.UpNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.UpNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.UpRange, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.UpRange, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.Volume, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.Volume, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.WaiPan, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.RedVolume, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.WeiBi, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Weibi, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.WeiCha, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.Weicha, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EvenRedDays, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.RedDay, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowRange, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.NetFlowRange, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetInFlowRedDay4, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NetInFlowRedDay4, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetInFlowDay4, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlowDay4, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.AmountDay4, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.AmountDay4, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowRedDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NetFlowRedDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlowDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowRangeDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.NetFlowRangeDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetInFlowRedDay19, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NetInFlowRedDay19, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.NetInFlowDay19, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlowDay19, FieldFactor.NoFactor) );
            DicFieldIndex.Add(ReqFieldIndexOrg.AmountDay19, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.AmountDay19, FieldFactor.NoFactor)  );
            DicFieldIndex.Add(ReqFieldIndexOrg.NetFlowRedDay20, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NetFlowRedDay20, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowDay20, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlowDay20, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.NetFlowRangeDay20, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.NetFlowRangeDay20, FieldFactor.NoFactor) );
            DicFieldIndex.Add(ReqFieldIndexOrg.NetInFlowRedDay59, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NetInFlowRedDay59, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.NetInFlowDay59, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlowDay59, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.AmountDay59, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.AmountDay59, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowRedDay60, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NetFlowRedDay60, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowDay60, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NetFlowDay60, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetFlowRangeDay60, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.NetFlowRangeDay60, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.MarketValueTop1Code, new DicFieldIndexItem(ReqFieldIndexType.FT_TOPCODE, FieldIndex.MarketValueTop1Code, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeLead1, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRangeLead1, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.MarketValueTop2Code, new DicFieldIndexItem(ReqFieldIndexType.FT_TOPCODE, FieldIndex.MarketValueTop2Code, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeLead2, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRangeLead2, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.MarketValueTop3Code, new DicFieldIndexItem(ReqFieldIndexType.FT_TOPCODE, FieldIndex.MarketValueTop3Code, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeLead3, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRangeLead3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeDay120, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange120D, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRangeDay250, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange250D, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRange1Mint, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange1Mint, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRange3Mint, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange3Mint, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DifferRange5Mint, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DifferRange5Mint, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.InvestGrade, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.InvestGrade, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.InvestGradeNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.InvestGradeNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ValueCallAuction, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ValueCallAuction, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuySuper, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.BuySuper, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.SellSuper, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.SellSuper, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyBig, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.BuyBig, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellBig, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.SellBig, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyMiddle, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.BuyMiddle, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellMiddle, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.SellMiddle, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuySmall, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.BuySmall, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellSmall, new DicFieldIndexItem(ReqFieldIndexType.FT_LONG, FieldIndex.SellSmall, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDX, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDX, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDY, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDY, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDZ, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDZ, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDX5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDX5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDY5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDY5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDX10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDX10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDY10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.DDY10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDXRedFollowDay, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.DDXRedFollowDay, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDXRedFollowDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.DDXRedFollowDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.DDXRedFollowDay10, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.DDXRedFollowDay10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyFlowRangeSuper, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyFlowRangeSuper, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellFlowRangeSuper, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellFlowRangeSuper, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyFlowRangeBig, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BuyFlowRangeBig, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellFlowRangeBig, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SellFlowRangeBig, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRange, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZengCangRange, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRank, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRank, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankHis, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankHis, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRangeDay3, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZengCangRangeDay3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankDay3, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankDay3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankHisDay3, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankHisDay3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRangeDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZengCangRangeDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankHisDay5, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankHisDay5, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRangeDay10, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZengCangRangeDay10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankDay10, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankDay10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZengCangRankHisDay10, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ZengCangRankHisDay10, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ReportNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ReportNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BuyReportNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BuyReportNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.AddReportNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.AddReportNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NeuturReportNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.NeuturReportNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ReduceReportNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ReduceReportNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SellReportNum, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.SellReportNum, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EpsQmtby, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.EpsQmtby, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EpsCurrentYear, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.EpsCurrentYear, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EpsNextYear1, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.EpsNextYear1, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EpsNextYear2, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.EpsNextYear2, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EpsNextYear3, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.EpsNextYear3, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.EpsTtm, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.EpsTtm, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.MGJZC, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.MGJZC, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.JZC, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.JZC, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZYYWSR, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.ZYYWSR, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZYYWLR, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.ZYYWLR, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ZZC, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.ZZC, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.TotalLiab, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.TotalLiab, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.OWnerEqu, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.OWnerEqu, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.CapRes, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOATFINANCE, FieldIndex.CapRes, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.NetBShare, new DicFieldIndexItem(ReqFieldIndexType.FT_LONGFINANCE, FieldIndex.NetBShare, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ProfitFO, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.ProfitFO, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.InvIncome, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.InvIncome, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PBTax, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.PBTax, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.RProfotAA, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.RProfotAA, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.CurAsset, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.CurAsset, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FixAsset, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.FixAsset, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.IntAsset, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.IntAsset, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.TCurLiab, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.TCurLiab, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.TLongLiab, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.TLongLiab, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.HShare, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.Hshare, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BGQDate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BGQDate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.ListDate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.ListDate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.NetAShare, new DicFieldIndexItem(ReqFieldIndexType.FT_LONGFINANCE, FieldIndex.NetAShare, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.IncomeRatio, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.IncomeRatio, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.NPRatio, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.NPRatio, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.OpenInterest, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.OpenInterest, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.PreOpenInterest, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.PreOpenInterest, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.SettlementPrice, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.SettlementPrice, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.PreSettlementPrice, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PreSettlementPrice, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.IncreaseDaily, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.OpenInterestDaily, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundLatestDate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.FundLatestDate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundPernav, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundPernav, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvper1, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvper1, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgrty, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgrty, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundNvgrw1w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgrw1w, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr4w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr4w, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr13w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr13w, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr26w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr26w, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr52w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr52w, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr104w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr104w, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr156w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr156w, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgr208w, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgr208w, FieldFactor.NoFactor) );
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNvgrf, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNvgrf, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundDecYearhb, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundDecYearhb, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundStrCxrank3y, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundStrCxrank3y, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundNvgrthl3year, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundNvgrthl3year, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundNav, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundNav, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundParaname, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundParaname, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundStrSgshzt, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundStrSgshzt, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundManagercode, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundManagercode, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundManagername, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundManagername, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundStrTgrcom, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundStrTgrcom, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundTgrcode, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundTgrcode, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundDecZdf, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundDecZdf, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundAccunav, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundAccunav, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundFounddate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.FundFounddate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundEnddate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.FundEnddate, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundFmanager, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundFmanager, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.FundStrTexch, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.FundStrTexch, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundCshare, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundCshare, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundAgior, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundAgior, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.FundLastestShare, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.FundLastestShare, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDecLavgytm, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondDecLavgytm, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondTomrtyyear, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondTomrtyyear, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondMrtydate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BondMrtydate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDecLcavg, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondDecLcavg, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDecLcytm, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondDecLcytm, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondLcclose, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondLcclose, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondLfclose, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondLfclose, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondAI, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondAI, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondBasisvalue, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondBasisvalue, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondConvexity, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondConvexity, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondMduration, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondMduration, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDuration, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondDuration, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.BondInstname, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.BondInstname, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.BondStrTstk, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.BondStrTstk, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.BondStrZtpj, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.BondStrZtpj, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.BondStrZqpj, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.BondStrZqpj, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondBondperiod, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondBondperiod, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondNewrate, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondNewrate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDate, new DicFieldIndexItem(ReqFieldIndexType.FT_INT32, FieldIndex.BondDate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondNowYTM, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondNowYTM, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDecNowYTM, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondDecNowYTM, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondAveDifference, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondAvgDiffer, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondDecZqye, new DicFieldIndexItem(ReqFieldIndexType.FT_DOUBLE, FieldIndex.ZQYE, FieldFactor.NoFactor));
            DicFieldIndex.Add(ReqFieldIndexOrg.BondType, new DicFieldIndexItem(ReqFieldIndexType.FT_STRINGORG, FieldIndex.BondType, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondStockPrice, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondStockPrice, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondStockDiffRange, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZGDiffRange, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondStockTurnOver, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondStockTurnover, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondSwapValue, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZGJZ, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondConversionPremium, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.ZGYJL, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondArbitrageSpace, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.TLKJ, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondPremiumRate, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.CZYJL, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondPbpPremiumRate, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.PJYJL, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondOpenYtm, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondYTMOpen, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondHighYtm, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondYTMHigh, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondSwapPrice, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondSwapPrice, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondSwapRate, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondConversionRate, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondCzjz, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.CZJZ, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.BondLowYtm, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.BondYTMLow, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.MedDiffer, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.MedDiffer, FieldFactor.NoFactor));
            DicFieldIndex.Add( ReqFieldIndexOrg.MedDifferRange, new DicFieldIndexItem(ReqFieldIndexType.FT_FLOAT, FieldIndex.MedDifferRange, FieldFactor.NoFactor));
        }

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<ReqFieldIndexOrg, DicFieldIndexItem> DicFieldIndex = new Dictionary<ReqFieldIndexOrg, DicFieldIndexItem>();
                                                                                
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class CustomKeyValue
    {
        /// <summary>
        /// 添加键值
        /// </summary>
        public static void SafeAddKeyValue(Dictionary<FieldIndex,object> dic, FieldIndex key, object val)
        {
            if (dic.ContainsKey(key))
                dic[key] = val;
            else
                dic.Add(key,val);
        }
    }

    public enum Fieldtype
    {
        TypeInt32,
        TypeSingle,
        TypeInt64,
        TypeDouble,
        TypeString,
        TypeObject,
    }

    /// <summary>
    /// 实时数据集合
    /// </summary>
    public class DetailData
    {
        //public static Dictionary<String, Dictionary<FieldIndex, object>> AllDetailData =
        //    new Dictionary<String, Dictionary<FieldIndex, object>>();

        //public static Dictionary<int, String> UnicodeToCode = new Dictionary<int, String>();
        public static Dictionary<uint, IntPtr> CodeIntPtrsAllOrder = new Dictionary<uint, IntPtr>(1);
        public static Dictionary<uint, IntPtr> CodeIntPtrsNOrder = new Dictionary<uint, IntPtr>(1);
        public static Dictionary<uint, IntPtr> CodeIntPtrsTrendFF = new Dictionary<uint, IntPtr>(1);

        /// <summary>
        /// emcode和内码的对应关系
        /// </summary>
        public static Dictionary<String, int> EmCodeToUnicode = new Dictionary<String, int>(14000);

        /// <summary>
        /// 内存中所有的报价数据
        /// </summary>
       // public static Dictionary<int, Dictionary<FieldIndex, object>> AllStockDetailData = new Dictionary<int, Dictionary<FieldIndex, object>>();

        #region 新版FieldIndex


        /// <summary>
        /// int型flield集合
        /// </summary>
        public static Dictionary<int, Dictionary<FieldIndex, int>> FieldIndexDataInt32 = new Dictionary<int, Dictionary<FieldIndex, int>>(1);
        
        /// <summary>
        /// float型flield集合
        /// </summary>
        public static Dictionary<int, Dictionary<FieldIndex, float>> FieldIndexDataSingle = new Dictionary<int, Dictionary<FieldIndex, float>>(1);
        
        /// <summary>
        /// long型flield集合
        /// </summary>
        public static Dictionary<int, Dictionary<FieldIndex, long>> FieldIndexDataInt64 = new Dictionary<int, Dictionary<FieldIndex, long>>(1);
        
        /// <summary>
        /// double型flield集合
        /// </summary>
        public static Dictionary<int, Dictionary<FieldIndex, double>> FieldIndexDataDouble = new Dictionary<int, Dictionary<FieldIndex, double>>(1);
        
        /// <summary>
        /// String型flield集合
        /// </summary>
        public static Dictionary<int, Dictionary<FieldIndex, String>> FieldIndexDataString = new Dictionary<int, Dictionary<FieldIndex, String>>(1);
        
        /// <summary>
        /// object型flield集合
        /// </summary>
        public static Dictionary<int, Dictionary<FieldIndex, object>> FieldIndexDataObject= new Dictionary<int, Dictionary<FieldIndex, object>>(1);

        /// <summary>
        /// 设置Field的值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="field"></param>
        /// <param name="index">取哪一个值的下标，从0开始</param>
        /// <param name="paramInt"></param>
        /// <param name="paramSingle"></param>
        /// <param name="paramInt64"></param>
        /// <param name="paramDouble"></param>
        /// <param name="paramString"></param>
        /// <param name="paramObject"></param>
        public static void SetFieldData(int code, FieldIndex field, Fieldtype index, int paramInt, float paramSingle, long paramInt64,
            double paramDouble, String paramString, object paramObject)
        {
            switch ((byte)index)
            {
                case 0:
                    if (field >= 0 && (int) field <= 299)
                    {
                        Dictionary<FieldIndex, int> fieldInt32;
                        if (!FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                        {
                            fieldInt32 = new Dictionary<FieldIndex, int>(1);
                            FieldIndexDataInt32[code] = fieldInt32;
                        }
                        fieldInt32[field] = paramInt;
                    }
                    else if ((int)field >= 300 && (int)field <= 799)
                    {
                        Dictionary<FieldIndex, float> fieldSingle;
                        if (!FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                        {
                            fieldSingle = new Dictionary<FieldIndex, float>(1);
                            FieldIndexDataSingle[code] = fieldSingle;
                        }
                        fieldSingle[field] = paramInt;
                    }
                    else if ((int) field >= 800 && (int) field <= 999)
                    {
                        Dictionary<FieldIndex, double> fieldDouble;
                        if (!FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                        {
                            fieldDouble = new Dictionary<FieldIndex, double>(1);
                            FieldIndexDataDouble[code] = fieldDouble;
                        }
                        fieldDouble[field] = paramInt;
                    }
                    else if ((int) field >= 1000 && (int) field <= 1199)
                    {
                        Dictionary<FieldIndex, long> fieldInt64;
                        if (!FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                        {
                            fieldInt64 = new Dictionary<FieldIndex, long>(1);
                            FieldIndexDataInt64[code] = fieldInt64;
                        }
                        fieldInt64[field] = paramInt;
                    }
                    break;
                case 1:
                    if (field >= 0 && (int)field <= 299)
                    {
                        Dictionary<FieldIndex, int> fieldInt32;
                        if (!FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                        {
                            fieldInt32 = new Dictionary<FieldIndex, int>(1);
                            FieldIndexDataInt32[code] = fieldInt32;
                        }
                        fieldInt32[field] = (int)paramSingle;
                    }
                    else if ((int)field >= 300 && (int)field <= 799)
                    {
                        Dictionary<FieldIndex, float> fieldSingle;
                        if (!FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                        {
                            fieldSingle = new Dictionary<FieldIndex, float>(1);
                            FieldIndexDataSingle[code] = fieldSingle;
                        }
                        fieldSingle[field] = paramSingle;
                    }
                    else if ((int)field >= 800 && (int)field <= 999)
                    {
                        Dictionary<FieldIndex, double> fieldDouble;
                        if (!FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                        {
                            fieldDouble = new Dictionary<FieldIndex, double>(1);
                            FieldIndexDataDouble[code] = fieldDouble;
                        }
                        fieldDouble[field] = paramSingle;
                    }
                    else if ((int)field >= 1000 && (int)field <= 1199)
                    {
                        Dictionary<FieldIndex, long> fieldInt64;
                        if (!FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                        {
                            fieldInt64 = new Dictionary<FieldIndex, long>(1);
                            FieldIndexDataInt64[code] = fieldInt64;
                        }
                        fieldInt64[field] = (long)paramSingle;
                    }
                    break;
                case 2:
                    if (field >= 0 && (int)field <= 299)
                    {
                        Dictionary<FieldIndex, int> fieldInt32;
                        if (!FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                        {
                            fieldInt32 = new Dictionary<FieldIndex, int>(1);
                            FieldIndexDataInt32[code] = fieldInt32;
                        }
                        fieldInt32[field] = (int)paramInt64;
                    }
                    else if ((int)field >= 300 && (int)field <= 799)
                    {
                        Dictionary<FieldIndex, float> fieldSingle;
                        if (!FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                        {
                            fieldSingle = new Dictionary<FieldIndex, float>(1);
                            FieldIndexDataSingle[code] = fieldSingle;
                        }
                        fieldSingle[field] = paramInt64;
                    }
                    else if ((int)field >= 800 && (int)field <= 999)
                    {
                        Dictionary<FieldIndex, double> fieldDouble;
                        if (!FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                        {
                            fieldDouble = new Dictionary<FieldIndex, double>(1);
                            FieldIndexDataDouble[code] = fieldDouble;
                        }
                        fieldDouble[field] = paramInt64;
                    }
                    else if ((int)field >= 1000 && (int)field <= 1199)
                    {
                        Dictionary<FieldIndex, long> fieldInt64;
                        if (!FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                        {
                            fieldInt64 = new Dictionary<FieldIndex, long>(1);
                            FieldIndexDataInt64[code] = fieldInt64;
                        }
                        fieldInt64[field] = paramInt64;
                    }
                    break;
                case 3:
                    if (field >= 0 && (int)field <= 299)
                    {
                        Dictionary<FieldIndex, int> fieldInt32;
                        if (!FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                        {
                            fieldInt32 = new Dictionary<FieldIndex, int>(1);
                            FieldIndexDataInt32[code] = fieldInt32;
                        }
                        fieldInt32[field] = (int)paramDouble;
                    }
                    else if ((int)field >= 300 && (int)field <= 799)
                    {
                        Dictionary<FieldIndex, float> fieldSingle;
                        if (!FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                        {
                            fieldSingle = new Dictionary<FieldIndex, float>(1);
                            FieldIndexDataSingle[code] = fieldSingle;
                        }
                        fieldSingle[field] = (float)paramDouble;
                    }
                    else if ((int)field >= 800 && (int)field <= 999)
                    {
                        Dictionary<FieldIndex, double> fieldDouble;
                        if (!FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                        {
                            fieldDouble = new Dictionary<FieldIndex, double>(1);
                            FieldIndexDataDouble[code] = fieldDouble;
                        }
                        fieldDouble[field] = paramDouble;
                    }
                    else if ((int)field >= 1000 && (int)field <= 1199)
                    {
                        Dictionary<FieldIndex, long> fieldInt64;
                        if (!FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                        {
                            fieldInt64 = new Dictionary<FieldIndex, long>(1);
                            FieldIndexDataInt64[code] = fieldInt64;
                        }
                        fieldInt64[field] = (long)paramDouble;
                    }
                    break;
                case 4:
                    Dictionary<FieldIndex, String> fieldString;
                    if (!FieldIndexDataString.TryGetValue(code, out fieldString))
                    {
                        fieldString = new Dictionary<FieldIndex, String>(1);
                        FieldIndexDataString[code] = fieldString;
                    }
                    fieldString[field] = paramString;
                    break;
                case 5:
                    Dictionary<FieldIndex, object> fieldObject;
                    if (!FieldIndexDataObject.TryGetValue(code, out fieldObject))
                    {
                        fieldObject = new Dictionary<FieldIndex, object>(1);
                        FieldIndexDataObject[code] = fieldObject;
                    }
                    fieldObject[field] = paramObject;
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 初始化的静态数据
        /// </summary>
       // public static Dictionary<int, OneDetailStaticDataRec> AllStockStaticData = new Dictionary<int, OneDetailStaticDataRec>(2700);
            
        /// <summary>
        /// 初始化的财务数据
        /// </summary>
      //  public static Dictionary<int, OneFinanceDataRec> AllFinanceData = new Dictionary<int, OneFinanceDataRec>(2700);


        /// <summary>
        /// MsgId和解压IntPtr的映射
        /// </summary>
        //public static Dictionary<int,IntPtr> MsgIdIntPtrs = new Dictionary<int, IntPtr>(1);

        /// <summary>
        /// MsgId和解压IntPtr的映射
        /// </summary>
        public static Dictionary<uint, IntPtr> MsgIdIntPtrsAllOrder = new Dictionary<uint, IntPtr>(1);

        /// <summary>
        /// MsgId和解压IntPtr的映射
        /// </summary>
        public static Dictionary<uint, IntPtr> MsgIdIntPtrsNOrder = new Dictionary<uint, IntPtr>(1);

        /// <summary>
        /// MsgId和解压IntPtr的映射
        /// </summary>
        public static Dictionary<uint, IntPtr> MsgIdIntPtrsQueue = new Dictionary<uint, IntPtr>(1); 


        /// <summary>
        /// 债券价格和ytm对应关系，内码，
        /// </summary>
        public static Dictionary<int, Dictionary<float, float>> AllBondYtmDataRec = new Dictionary<int, Dictionary<float, float>>(1);

        /// <summary>
        /// 从键盘精灵获结构取一只股票的基础字段，写到内存
        /// </summary>
        /// <param name="code"></param>
        /// <param name="emCode"></param>
        /// <param name="typeCode"></param>
        /// <param name="name"></param>
        public static void SetStockBasicField(int code, String emCode, String typeCode, String name)
        {
            SetStockBasicField(code, emCode, Convert.ToInt32(typeCode), name);
        }

        /// <summary>
        /// 从键盘精灵获结构取一只股票的基础字段，写到内存
        /// </summary>
        /// <param name="code"></param>
        /// <param name="emCode"></param>
        /// <param name="typeCode"></param>
        /// <param name="name"></param>
        public static void SetStockBasicField(int code, String emCode, int typeCode, String name)
        {
            Dictionary<FieldIndex, String> fieldString;
            Dictionary<FieldIndex, int> fieldInt32;
            if (!FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                FieldIndexDataInt32[code] = fieldInt32;
            }
            if (!FieldIndexDataString.TryGetValue(code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, String>(1);
                FieldIndexDataString[code] = fieldString;
            }
            if (!fieldString.ContainsKey(FieldIndex.EMCode))
                fieldString[FieldIndex.EMCode] = emCode;
            if (!fieldString.ContainsKey(FieldIndex.Name))
                fieldString[FieldIndex.Name] = name;
            if (!fieldString.ContainsKey(FieldIndex.Code))
                fieldString[FieldIndex.Code] = emCode.Split('.')[0];
            if (!fieldInt32.ContainsKey(FieldIndex.Market))
                fieldInt32[FieldIndex.Market] = (int)SecurityAttribute.TypeCodeToMarketType(typeCode, emCode);
        }

        /// <summary>
        /// 从键盘精灵获结构取一只股票的基础字段，写到内存
        /// </summary>
        /// <param name="code"></param>
        /// <param name="emCode"></param>
        /// <param name="market"></param>
        /// <param name="name"></param>
        public static void SetStockBasicField(int code, String emCode, MarketType market, String name)
        {
            Dictionary<FieldIndex, String> fieldString;
            Dictionary<FieldIndex, int> fieldInt32;
            if (!FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                FieldIndexDataInt32[code] = fieldInt32;
            }
            if (!FieldIndexDataString.TryGetValue(code, out fieldString))
            {
                fieldString = new Dictionary<FieldIndex, String>(1);
                FieldIndexDataString[code] = fieldString;
            }
            if (!fieldString.ContainsKey(FieldIndex.EMCode))
                fieldString[FieldIndex.EMCode] = emCode;
            if (!fieldString.ContainsKey(FieldIndex.Name))
                fieldString[FieldIndex.Name] = name;
            if (!fieldString.ContainsKey(FieldIndex.Code))
                fieldString[FieldIndex.Code] = emCode.Split('.')[0];
            if (!fieldInt32.ContainsKey(FieldIndex.Market))
                fieldInt32[FieldIndex.Market] = (int)market;
        }

       
    }

    /// <summary>
    /// 股票标记转字符
    /// </summary>
    public class ConvertTag
    {
        /// <summary>
        /// 股票标记转字符
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static void ConvertTagToString(StockTag tag,out String tagShowString, out SolidBrush brush)
        {
            switch (tag)
            {
                case StockTag.One:
                    tagShowString = "①";
                    break;
                case StockTag.Two:
                    tagShowString = "②";
                    break;
                case StockTag.Three:
                    tagShowString = "③";
                    break;
                case StockTag.Four:
                    tagShowString = "④";
                    break;
                case StockTag.Five:
                    tagShowString = "⑤";
                    break;
                case StockTag.Six:
                    tagShowString = "⑥";
                    break;
                case StockTag.Seven:
                    tagShowString = "⑦";
                    break;
                case StockTag.Eight:
                    tagShowString = "⑧";
                    break;
                case StockTag.Nine:
                    tagShowString = "⑨";
                    break;
                case StockTag.Text:
                    tagShowString = "T";
                    break;
                default:
                    tagShowString = String.Empty;
                    break;
            }
            brush = QuoteDrawService.BrushColorDown;
        }
    }
}
    