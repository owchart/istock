using System;
using System.Collections.Generic;


namespace EmQComm
{
    #region 枚举
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum SortMode
    {
        /// <summary>
        /// 默认排序
        /// </summary>
        Mode_Code = 0,
        /// <summary>
        /// 降序
        /// </summary>
        Mode_DESC,
        /// <summary>
        /// 升序
        /// </summary>
        Mode_ASC,

    }

    /// <summary>
    /// K线周期
    /// </summary>
    public enum KLineCycle
    {
        /// <summary>
        /// 1分钟
        /// </summary>
        CycleMint1 = 1,
        /// <summary>
        /// 5分钟
        /// </summary>
        CycleMint5 = 2,
        /// <summary>
        /// 15分钟
        /// </summary>
        CycleMint15 = 3,
        /// <summary>
        /// 30分钟
        /// </summary>
        CycleMint30 = 4,
        /// <summary>
        /// 60分钟
        /// </summary>
        CycleMint60 = 5,
        /// <summary>
        /// 120分钟
        /// </summary>
        CycleMint120 = 6,
        /// <summary>
        /// 日
        /// </summary>
        CycleDay = 7,
        /// <summary>
        /// 周
        /// </summary>
        CycleWeek = 8,
        /// <summary>
        /// 月
        /// </summary>
        CycleMonth = 9,
        /// <summary>
        /// 季度
        /// </summary>
        CycleSeason = 10,
        /// <summary>
        /// 年
        /// </summary>
        CycleYear = 11
    }

    /// <summary>
    /// K线周期
    /// </summary>
    public enum KLineCycleOrg
    {
        /// <summary>
        /// 1分钟
        /// </summary>
        CycleMint1 = 1,
        /// <summary>
        /// 5分钟
        /// </summary>
        CycleMint5 = 5,
        /// <summary>
        /// 15分钟
        /// </summary>
        CycleMint15 = 15,
        /// <summary>
        /// 30分钟
        /// </summary>
        CycleMint30 = 30,
        /// <summary>
        /// 60分钟
        /// </summary>
        CycleMint60 = 60,
        /// <summary>
        /// 120分钟
        /// </summary>
        CycleMint120 = 120,
        /// <summary>
        /// 日
        /// </summary>
        CycleDay = 1440,
        /// <summary>
        /// 周
        /// </summary>
        CycleWeek = 2000,
        /// <summary>
        /// 月
        /// </summary>
        CycleMonth = 2001,
        /// <summary>
        /// 季度
        /// </summary>
        CycleSeason = 2002,
        /// <summary>
        /// 年
        /// </summary>
        CycleYear = 2003,
    }

    /// <summary>
    /// 复权状态
    /// </summary>
    public enum IsDivideRightType
    {
        /// <summary>
        /// 不复权
        /// </summary>
        Non = 1,
        /// <summary>
        /// 前复权
        /// </summary>
        Forward = 2,
        /// <summary>
        /// 后复权
        /// </summary>
        Backward = 3,
    }

    /// <summary>
    /// 委托队列没笔的状态
    /// </summary>
    public enum OrderQueueItemStatus
    {
        /// <summary>
        /// 不变
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 全部成交
        /// </summary>
        Full,
        /// <summary>
        ///  部分成交
        /// </summary>
        Part,
        /// <summary>
        /// 撤单
        /// </summary>
        Revoke,
        /// <summary>
        /// 新增
        /// </summary>
        New,

    }

    /// <summary>
    /// 服务端短线精灵类型
    /// </summary>
    public enum ReqShortLineTye
    {
        ///// <summary>
        ///// 封涨停板
        ///// </summary>
        //SurgedLimit = 4,
        ///// <summary>
        ///// 封跌停板
        ///// </summary>
        //DeclineLimit = 8,
        ///// <summary>
        ///// 打开涨停
        ///// </summary>
        //OpenSurgedLimit = 16,
        ///// <summary>
        ///// 打开跌停
        ///// </summary>
        //OpenDeclineLimit = 32,
        ///// <summary>
        ///// 有大买盘
        ///// </summary>
        //BiggerAskOrder = 64,
        ///// <summary>
        ///// 有大卖盘
        ///// </summary>
        //BiggerBidOrder = 128,
        ///// <summary>
        ///// 机构买单
        ///// </summary>
        //InstitutionAskOrder = 256,
        ///// <summary>
        ///// 机构卖单
        ///// </summary>
        //InstitutionBidOrder = 512,
        ///// <summary>
        ///// 火箭发射
        ///// </summary>
        //RocketLaunch = 8201,
        ///// <summary>
        ///// 快速反弹
        ///// </summary>
        //StrongRebound = 8202,
        ///// <summary>
        ///// 高台跳水
        ///// </summary>
        //HighDiving = 8203,
        ///// <summary>
        ///// 加速下跌
        ///// </summary>
        //SpeedupDown = 8204,
        ///// <summary>
        ///// 买入撤单
        ///// </summary>
        //CancelBigAskOrder = 8205,
        ///// <summary>
        ///// 卖出撤单
        ///// </summary>
        //CancelBigBidOrder = 8206,
        ///// <summary>
        ///// 大笔卖出
        ///// </summary>
        //InstitutionBidTrans = 8193,
        ///// <summary>
        ///// 大笔买入
        ///// </summary>
        //InstitutionAskTrans = 8194,
        ///// <summary>
        ///// 买单分单
        ///// </summary>
        //MultiSameAskOrders = 8195,
        ///// <summary>
        ///// 卖单分单
        ///// </summary>
        //MultiSameBidOrders = 8196,
        BidDown = 0x2010,
        BidUp = 0x200f,
        BiggerAskOrder = 0x40,
        BiggerBidOrder = 0x80,
        CancelBigAskOrder = 0x200d,
        CancelBigBidOrder = 0x200e,
        DeclineLimit = 8,
        Down5DaysAverage = 0x2012,
        DownGap = 0x2014,
        DropSharply = 0x2018,
        HighDiving = 0x200b,
        HighestIn60Days = 0x2015,
        IncreaseSharply = 0x2017,
        InstitutionAskOrder = 0x100,
        InstitutionAskTrans = 0x2001,
        InstitutionBidOrder = 0x200,
        InstitutionBidTrans = 0x2002,
        LowestIn60Days = 0x2016,
        MultiSameAskOrders = 0x2019,
        MultiSameBidOrders = 0x201a,
        OpenDeclineLimit = 0x20,
        OpenSurgedLimit = 0x10,
        RocketLaunch = 0x2009,
        SpeedupDown = 0x200c,
        StrongRebound = 0x200a,
        SurgedLimit = 4,
        TopAskOrders = 1,
        TopBidOrders = 2,
        TractorsAskOrders = 0x2003,
        TractorsBidOrders = 0x2004,
        Up5DaysAverage = 0x2011,
        UpGap = 0x2013
    }

    /// <summary>
    /// 客户端短线精灵类型
    /// </summary>
    [Flags]
    public enum ShortLineType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0x00,
        /// <summary>
        /// 封涨停板
        /// </summary>
        SurgedLimit = 0x01,
        /// <summary>
        /// 封跌停板
        /// </summary>
        DeclineLimit = 0x02,
        /// <summary>
        /// 打开涨停
        /// </summary>
        OpenSurgedLimit = 0x04,
        /// <summary>
        /// 打开跌停
        /// </summary>
        OpenDeclineLimit = 0x08,
        /// <summary>
        /// 有大买盘
        /// </summary>
        BiggerAskOrder = 0x10,
        /// <summary>
        /// 有大卖盘
        /// </summary>
        BiggerBidOrder = 0x20,
        /// <summary>
        /// 机构买单
        /// </summary>
        InstitutionAskOrder = 0x40,
        /// <summary>
        /// 机构卖单
        /// </summary>
        InstitutionBidOrder = 0x80,
        /// <summary>
        /// 火箭发射
        /// </summary>
        RocketLaunch = 0x100,
        /// <summary>
        /// 快速反弹
        /// </summary>
        StrongRebound = 0x200,
        /// <summary>
        /// 高台跳水
        /// </summary>
        HighDiving = 0x400,
        /// <summary>
        /// 加速下跌
        /// </summary>
        SpeedupDown = 0x800,
        /// <summary>
        /// 买入撤单
        /// </summary>
        CancelBigAskOrder = 0x1000,
        /// <summary>
        /// 卖出撤单
        /// </summary>
        CancelBigBidOrder = 0x2000,
        /// <summary>
        /// 大笔卖出
        /// </summary>
        InstitutionBidTrans = 0x4000,
        /// <summary>
        /// 大笔买入
        /// </summary>
        InstitutionAskTrans = 0x8000,
        /// <summary>
        /// 买单分单
        /// </summary>
        MultiSameAskOrders = 0x10000,
        /// <summary>
        /// 卖单分单
        /// </summary>
        MultiSameBidOrders = 0x20000,
    }
    #endregion

    #region 0.用户ID号的数据结构，UserID
    /// <summary>
    /// 
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInfo()
        {
            IsVerifyFromSrv = true;
            IsVIPTerminal = false;
            IsSingle = true;
            WebF10Address = "";
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 每个sim卡的卡号
        /// </summary>
        public long IMSI;
        /// <summary>
        /// 和sim卡相对应的用户号 
        /// </summary>
        public int UserId;
        /// <summary>
        /// 用户手机号
        /// </summary>
        public long PhoneNum;
        /// <summary>
        /// 用户级别
        /// </summary>
        public byte UserGrade;
        /// <summary>
        /// webF10的地址 ，应vip用户要求，进行设置
        /// </summary>
        public string WebF10Address;

        /// <summary>
        /// 收到单点登入的消息后，
        /// 单点登入：true
        /// 有新用户登入：false
        /// </summary>
        /// <returns></returns>
        public bool IsSingle;

        /// <summary>
        /// 用户是否有深圳Level2实时行情权限
        /// </summary>
        public bool HaveSHLevel2Right;

        /// <summary>
        /// 用户是否有深圳Level2实时行情权限
        /// </summary>
        public bool HaveSZLevel2Right;

        ///<summary>
        /// 用户是否有港股实时行情权限
        ///</summary>
        public bool HaveHKRealTimeRight;

        ///<summary>
        /// 用户是否有港股延时实时行情权限
        ///</summary>
        public bool HaveHKDelayRight;

        ///<summary>
        /// 用户是否有银行间债券实时行情权限
        ///</summary>
        public bool HaveInterbankBondRight;

        /// <summary>
        /// 是否有三板行情
        /// </summary>
        public bool HaveThirdBoardMarketRight;

        /// <summary>
        /// 是否有中债指数行情
        /// </summary>
        public bool HaveIndexChinaBondRight;

        /// <summary>
        /// 是否有股指期货行情
        /// </summary>
        public bool HaveIndexFutureRight;

        /// <summary>
        /// 是否有中债估值权限
        /// </summary>
        public bool HaveDebtMemberRight;

        /// <summary>
        /// 是否是vip终端
        /// </summary>
        public bool IsVIPTerminal;

        /// <summary>
        /// 是否通过服务器验证
        /// </summary>
        public bool IsVerifyFromSrv;


    }
    #endregion

    #region 1.报价数据结构
    /// <summary>
    /// 第一次报价的数据结构
    /// </summary>
    public class OneDetailReportDataRec
    {
        public double Amount;
        public double NetInFlow;
        public float PreClose;
        public float BuyPrice1;
        public float High;
        public float Low;
        public float Now;
        public float Open;
        //public float PreClose;
        public float SellPrice1;
        public float Accer;
        public float WeiBi;
        public float WeiCha;
        public float DifferRange1Mint;
        public float DifferRange3Mint;
        public float DifferRange5Mint;

        public int NeiPan;
        public int BuyVolume1;
        public int SellVolume1;
        public int CurVol;
        public long Volume;
        public int EvenRedDays;


    }

    /// <summary>
    /// 沪深AB的静态数据
    /// </summary>
    public class OneDetailStaticDataRec
    {
        public float HighW52;
        public float LowW52;
        public float PreClose;
        public float PreCloseDay3;
        public float PreCloseDay5;
        public float PreCloseDay10;
        public float PreCloseDay20;
        public float PreCloseDay60;
        public float PreCloseDay120;
        public float PreCloseDay250;
        public float PreCloseYTD;
        public float VolumeAvgDay5;
        public double AmountDay4;
        public double AmountDay19;
        public double AmountDay59;
        public double NetInFlowDay4;
        public double NetInFlowDay19;
        public double NetInFlowDay59;
        public int NetInFlowRedDay4;
        public int NetInFlowRedDay19;
        public int NetInFlowRedDay59;
        public float InvestGrade;
        public int InvestGradeNum;

    }

    /*
    /// <summary>
    /// 
    /// </summary>
    public class OneDetailDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code ;
        /// <summary>
        /// 初始值
        /// </summary>
        protected const byte InitValue = 0;
        /// <summary>
        /// 股票数据
        /// </summary>
        public Dictionary<FieldIndex, object> StockDetailDataRec = new Dictionary<FieldIndex, object>()
                                                                {
                                                                    {FieldIndex.Date, InitValue},
                                                                    {FieldIndex.Time, InitValue},
                                                                    {FieldIndex.Weicha, InitValue},
                                                                    {FieldIndex.Now, InitValue},
                                                                    {FieldIndex.Difference, InitValue},
                                                                    {FieldIndex.AveragePrice, InitValue},
                                                                    {FieldIndex.PreClose, InitValue},
                                                                    {FieldIndex.Open, InitValue},
                                                                    {FieldIndex.High, InitValue},
                                                                    {FieldIndex.Low, InitValue},
                                                                    {FieldIndex.Volume, InitValue},
                                                                    {FieldIndex.SumVol2, InitValue},
                                                                    {FieldIndex.SumVol5, InitValue},
                                                                    {FieldIndex.LastVolume, InitValue},
                                                                    {FieldIndex.Evg5Volume, InitValue},
                                                                    {FieldIndex.Amount, InitValue},
                                                                    {FieldIndex.GreenVolume, InitValue},
                                                                    {FieldIndex.RedVolume, InitValue},
                                                                    {FieldIndex.Weibi, InitValue},
                                                                    {FieldIndex.SumBuyVolume, InitValue},
                                                                    {FieldIndex.SumSellVolume, InitValue},
                                                                    {FieldIndex.DifferRange, InitValue},
                                                                    {FieldIndex.DifferRange3D, InitValue},
                                                                    {FieldIndex.DifferRange6D, InitValue},
                                                                    {FieldIndex.DifferSpeed,InitValue},
                                                                    {FieldIndex.VolumeRatio, InitValue},
                                                                    {FieldIndex.PE, InitValue},
                                                                    {FieldIndex.PB, InitValue},
                                                                    {FieldIndex.Ltg, InitValue},
                                                                    {FieldIndex.Delta, InitValue},
                                                                    {FieldIndex.Turnover, InitValue},
                                                                    {FieldIndex.Turnover3D, InitValue},
                                                                    {FieldIndex.Turnover6D, InitValue},
                                                                    {FieldIndex.Code, InitValue},
                                                                    {FieldIndex.Name, InitValue},
                                                                    {FieldIndex.SecuCode, InitValue},
                                                                    {FieldIndex.ChiSpelling, InitValue},
                                                                    {FieldIndex.ValueCallAuction, InitValue},
                                                                    {FieldIndex.BuyVolume1, InitValue},
                                                                    {FieldIndex.BuyVolume2, InitValue},
                                                                    {FieldIndex.BuyVolume3, InitValue},
                                                                    {FieldIndex.BuyVolume4, InitValue},
                                                                    {FieldIndex.BuyVolume5, InitValue},
                                                                    {FieldIndex.BuyVolume6, InitValue},
                                                                    {FieldIndex.BuyVolume7, InitValue},
                                                                    {FieldIndex.BuyVolume8, InitValue},
                                                                    {FieldIndex.BuyVolume9, InitValue},
                                                                    {FieldIndex.BuyVolume10, InitValue},
                                                                    {FieldIndex.SellVolume1, InitValue},
                                                                    {FieldIndex.SellVolume2, InitValue},
                                                                    {FieldIndex.SellVolume3, InitValue},
                                                                    {FieldIndex.SellVolume4, InitValue},
                                                                    {FieldIndex.SellVolume5, InitValue},
                                                                    {FieldIndex.SellVolume6, InitValue},
                                                                    {FieldIndex.SellVolume7, InitValue},
                                                                    {FieldIndex.SellVolume8, InitValue},
                                                                    {FieldIndex.SellVolume9, InitValue},
                                                                    {FieldIndex.SellVolume10, InitValue},
                                                                    {FieldIndex.BuyVolume1Delta, InitValue},
                                                                    {FieldIndex.BuyVolume2Delta, InitValue},
                                                                    {FieldIndex.BuyVolume3Delta, InitValue},
                                                                    {FieldIndex.BuyVolume4Delta, InitValue},
                                                                    {FieldIndex.BuyVolume5Delta, InitValue},
                                                                    {FieldIndex.BuyVolume6Delta, InitValue},
                                                                    {FieldIndex.BuyVolume7Delta, InitValue},
                                                                    {FieldIndex.BuyVolume8Delta, InitValue},
                                                                    {FieldIndex.BuyVolume9Delta, InitValue},
                                                                    {FieldIndex.BuyVolume10Delta, InitValue},
                                                                    {FieldIndex.SellVolume1Delta, InitValue},
                                                                    {FieldIndex.SellVolume2Delta, InitValue},
                                                                    {FieldIndex.SellVolume3Delta, InitValue},
                                                                    {FieldIndex.SellVolume4Delta, InitValue},
                                                                    {FieldIndex.SellVolume5Delta, InitValue},
                                                                    {FieldIndex.SellVolume6Delta, InitValue},
                                                                    {FieldIndex.SellVolume7Delta, InitValue},
                                                                    {FieldIndex.SellVolume8Delta, InitValue},
                                                                    {FieldIndex.SellVolume9Delta, InitValue},
                                                                    {FieldIndex.SellVolume10Delta, InitValue},
                                                                    {FieldIndex.BuyPrice1, InitValue},
                                                                    {FieldIndex.BuyPrice2, InitValue},
                                                                    {FieldIndex.BuyPrice3, InitValue},
                                                                    {FieldIndex.BuyPrice4, InitValue},
                                                                    {FieldIndex.BuyPrice5, InitValue},
                                                                    {FieldIndex.BuyPrice6, InitValue},
                                                                    {FieldIndex.BuyPrice7, InitValue},
                                                                    {FieldIndex.BuyPrice8, InitValue},
                                                                    {FieldIndex.BuyPrice9, InitValue},
                                                                    {FieldIndex.BuyPrice10, InitValue},
                                                                    {FieldIndex.SellPrice1, InitValue},
                                                                    {FieldIndex.SellPrice2, InitValue},
                                                                    {FieldIndex.SellPrice3, InitValue},
                                                                    {FieldIndex.SellPrice4, InitValue},
                                                                    {FieldIndex.SellPrice5, InitValue},
                                                                    {FieldIndex.SellPrice6, InitValue},
                                                                    {FieldIndex.SellPrice7, InitValue},
                                                                    {FieldIndex.SellPrice8, InitValue},
                                                                    {FieldIndex.SellPrice9, InitValue},
                                                                    {FieldIndex.SellPrice10, InitValue},
                                                                    {FieldIndex.Market, InitValue},
                                                                    {FieldIndex.ListDate, InitValue},
                                                                    {FieldIndex.BGQDate,InitValue},
                                                                    {FieldIndex.InfoMine,InitValue},
                                                                    {FieldIndex.ZGB,InitValue},
                                                                    {FieldIndex.AvgNetS,InitValue},
                                                                    {FieldIndex.MGSY,InitValue},
                                                                    {FieldIndex.MGSY2,InitValue},
                                                                    {FieldIndex.MGJZC,InitValue},
                                                                    {FieldIndex.MGJZC2,InitValue},
                                                                    {FieldIndex.JZC,InitValue},
                                                                    {FieldIndex.ZYYWSR,InitValue},
                                                                    {FieldIndex.IncomeRatio,InitValue},
                                                                    {FieldIndex.ProfitFO,InitValue},
                                                                    {FieldIndex.InvIncome,InitValue},
                                                                    {FieldIndex.PBTax,InitValue},
                                                                    {FieldIndex.ZYYWLR,InitValue},
                                                                    {FieldIndex.NPRatio,InitValue},
                                                                    {FieldIndex.RProfotAA,InitValue},
                                                                    {FieldIndex.DRPRPAA,InitValue},
                                                                    {FieldIndex.Gprofit,InitValue},
                                                                    {FieldIndex.ZZC,InitValue},
                                                                    {FieldIndex.CurAsset,InitValue},
                                                                    {FieldIndex.FixAsset,InitValue},
                                                                    {FieldIndex.IntAsset,InitValue},
                                                                    {FieldIndex.TotalLiab,InitValue},
                                                                    {FieldIndex.TCurLiab,InitValue},
                                                                    {FieldIndex.TLongLiab,InitValue},
                                                                    {FieldIndex.ZCFZL,InitValue},
                                                                    {FieldIndex.OWnerEqu,InitValue},
                                                                    {FieldIndex.OEquRatio,InitValue},
                                                                    {FieldIndex.CapRes,InitValue},
                                                                    {FieldIndex.DRPCapRes,InitValue},
                                                                    {FieldIndex.NetAShare,InitValue},
                                                                    {FieldIndex.NetBShare,InitValue},
                                                                    {FieldIndex.Hshare,InitValue},
                                                                    {FieldIndex.ZengCangRank,InitValue},
                                                                    {FieldIndex.ZengCangRankChange,InitValue},
                                                                    {FieldIndex.ZengCangRankHis,InitValue},
                                                                    {FieldIndex.ZengCangRankDay3,InitValue},
                                                                    {FieldIndex.ZengCangRankChangeDay3,InitValue},
                                                                    {FieldIndex.ZengCangRankHisDay3,InitValue},
                                                                    {FieldIndex.ZengCangRankDay5,InitValue},
                                                                    {FieldIndex.ZengCangRankChangeDay5,InitValue},
                                                                    {FieldIndex.ZengCangRankHisDay5,InitValue},
                                                                    {FieldIndex.ZengCangRankDay10,InitValue},
                                                                    {FieldIndex.ZengCangRankChangeDay10,InitValue},
                                                                    {FieldIndex.ZengCangRankHisDay10,InitValue},
                                                                    {FieldIndex.BuySuper,InitValue},
                                                                    {FieldIndex.BuyBig,InitValue},
                                                                    {FieldIndex.BuyMiddle,InitValue},
                                                                    {FieldIndex.BuySmall,InitValue},
                                                                    {FieldIndex.SellSuper,InitValue},
                                                                    {FieldIndex.SellBig,InitValue},
                                                                    {FieldIndex.SellMiddle,InitValue},
                                                                    {FieldIndex.SellSmall,InitValue},
                                                                    {FieldIndex.NetFlowSuper,InitValue},
                                                                    {FieldIndex.NetFlowBig,InitValue},
                                                                    {FieldIndex.NetFlowMiddle,InitValue},
                                                                    {FieldIndex.NetFlowSmall,InitValue},
                                                                    {FieldIndex.MainNetFlow,InitValue},
                                                                    {FieldIndex.DifferRange5D,InitValue},
                                                                    {FieldIndex.DifferRange10D,InitValue},
                                                                    {FieldIndex.ZengCangRange,InitValue},
                                                                    {FieldIndex.ZengCangRangeDay3,InitValue},
                                                                    {FieldIndex.ZengCangRangeDay5,InitValue},
                                                                    {FieldIndex.ZengCangRangeDay10,InitValue},
                                                                    {FieldIndex.NetFlowRangeSuper,InitValue},
                                                                    {FieldIndex.NetFlowRangeBig,InitValue},
                                                                    {FieldIndex.NetFlowRangeMiddle,InitValue},
                                                                    {FieldIndex.NetFlowRangeSmall,InitValue},
                                                                    {FieldIndex.BuyFlowRangeSuper,InitValue},
                                                                    {FieldIndex.BuyFlowRangeBig,InitValue},
                                                                    {FieldIndex.SellFlowRangeSuper,InitValue},
                                                                    {FieldIndex.SellFlowRangeBig,InitValue},
                                                                    {FieldIndex.IndustryBlockName,InitValue},
                                                                    {FieldIndex.IndustryBlockCode,InitValue},
                                                                    {FieldIndex.ZSZ,InitValue},
                                                                    {FieldIndex.LTSZ,InitValue},
                                                                    {FieldIndex.AllBlockCode,new List<string>()},
                                                                    {FieldIndex.StockMarektType,MarketType.NA},
                                                                    {FieldIndex.UpDay,InitValue},
                                                                    {FieldIndex.DifferRange20D, InitValue},
                                                                    {FieldIndex.DifferRange60D, InitValue},
                                                                    {FieldIndex.DifferRangeYTD, InitValue},
                                                                    {FieldIndex.HighW52, InitValue},
                                                                    {FieldIndex.LowW52, InitValue},
                                                                    {FieldIndex.NetFlowDay5, InitValue},
                                                                    {FieldIndex.NetFlowDay20, InitValue},
                                                                    {FieldIndex.NetFlowDay60, InitValue},
                                                                    {FieldIndex.NetFlowRedDay5, InitValue},
                                                                    {FieldIndex.NetFlowRedDay20, InitValue},
                                                                    {FieldIndex.NetFlowRedDay60, InitValue},
                                                                    {FieldIndex.DifferRange120D, InitValue},
                                                                    {FieldIndex.DifferRange250D, InitValue},
                                                                    {FieldIndex.IsDefaultCustomStock, false},
                                                                };
    }

    /// <summary>
    /// 指数
    /// </summary>
    public class IndexDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public IndexDetailDataRec()
        {
            StockDetailDataRec.Add(FieldIndex.IndexVolume, InitValue);
            StockDetailDataRec.Add(FieldIndex.IndexAmount, InitValue);
            StockDetailDataRec.Add(FieldIndex.UpNum, InitValue);
            StockDetailDataRec.Add(FieldIndex.EqualNum, InitValue);
            StockDetailDataRec.Add(FieldIndex.DownNum, InitValue);
            StockDetailDataRec.Add(FieldIndex.DifferRangeLead1, InitValue);
            StockDetailDataRec.Add(FieldIndex.DifferRangeLead2, InitValue);
            StockDetailDataRec.Add(FieldIndex.DifferRangeLead3, InitValue);
            StockDetailDataRec.Add(FieldIndex.MarketValueTop1Code, InitValue);
            StockDetailDataRec.Add(FieldIndex.MarketValueTop2Code, InitValue);
            StockDetailDataRec.Add(FieldIndex.MarketValueTop3Code, InitValue);
            StockDetailDataRec.Add(FieldIndex.StockNum, InitValue);

        }
    }

    /// <summary>
    /// 港股
    /// </summary>
    public class HKDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public HKDetailDataRec()
        {
            //可在构造函数中添加不同的FieldIndex项
            //比如：StockDetailDataRec.Add(10000,0);
        }
    }

    /// <summary>
    /// 三板市场
    /// </summary>
    public class OTCDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public OTCDetailDataRec()
        {

        }
    }

    /// <summary>
    /// 期货
    /// </summary>
    public class FuturesDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public FuturesDetailDataRec()
        {
            StockDetailDataRec.Add(FieldIndex.FundCshare, InitValue);
            StockDetailDataRec.Add(FieldIndex.CurOI, InitValue);
            StockDetailDataRec.Add(FieldIndex.DayOI, InitValue);
            StockDetailDataRec.Add(FieldIndex.SettlementPrice, InitValue);
            StockDetailDataRec.Add(FieldIndex.PreSettlementPrice, InitValue);
            StockDetailDataRec.Add(FieldIndex.OpenInterest, InitValue);
            StockDetailDataRec.Add(FieldIndex.PreOpenInterest, InitValue);
            StockDetailDataRec.Add(FieldIndex.OpenCloseStatus, InitValue);
        }
    }
    

    /// <summary>
    /// 沪深基金
    /// </summary>
    public class SHSZFundDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public SHSZFundDetailDataRec()
        {
            StockDetailDataRec.Add(FieldIndex.FundLatestDate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundPernav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvper1, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrwtd, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrty, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrw1w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr4w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr13w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr26w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr52w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr104w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr156w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr208w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrf, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundDecYearhb, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrCxrank3y, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrthl3year, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundParaname, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrSgshzt, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundManagercode, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundManagername, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrTgrcom, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundTgrcode, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundDecZdf, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundAccunav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundFounddate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundEnddate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundFmanager, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrTexch, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundCshare, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundAgior, InitValue);
        }
    }


    /// <summary>
    /// 货币型基金
    /// </summary>
    public class MonetaryFundDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// construct
        /// </summary>
        public MonetaryFundDetailDataRec()
        {
            //可在构造函数中添加不同的FieldIndex项
            //比如：StockDetailDataRec.Add(10000,0);
            StockDetailDataRec.Add(FieldIndex.FundLatestDate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundPernav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvper1, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrwtd, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrty, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrw1w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr4w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr13w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr26w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr52w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr104w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr156w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr208w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrf, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundDecYearhb, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrCxrank3y, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrthl3year, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundParaname, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrSgshzt, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundManagercode, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundManagername, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrTgrcom, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundTgrcode, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundDecZdf, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundAccunav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundFounddate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundEnddate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundFmanager, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrTexch, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundCshare, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundAgior, InitValue);
        }
    }

    /// <summary>
    /// 非货币型基金
    /// </summary>
    public class NonMonetaryFundDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public NonMonetaryFundDetailDataRec()
        {
            //可在构造函数中添加不同的FieldIndex项
            //比如：StockDetailDataRec.Add(10000,0);
            StockDetailDataRec.Add(FieldIndex.FundLatestDate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundPernav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvper1, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrwtd, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrty, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrw1w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr4w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr13w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr26w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr52w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr104w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr156w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgr208w, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrf, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundDecYearhb, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrCxrank3y, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNvgrthl3year, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundNav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundParaname, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrSgshzt, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundManagercode, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundManagername, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrTgrcom, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundTgrcode, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundDecZdf, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundAccunav, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundFounddate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundEnddate, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundFmanager, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundStrTexch, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundCshare, InitValue);
            StockDetailDataRec.Add(FieldIndex.FundAgior, InitValue);
        }
    }


    /// <summary>
    /// 银行间债券
    /// </summary>
    public class InterBankBondDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public InterBankBondDetailDataRec()
        {
            //可在构造函数中添加不同的FieldIndex项
            //比如：StockDetailDataRec.Add(10000,0);
            StockDetailDataRec.Add(FieldIndex.BondYTMLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondYTMHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondYTMOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetNow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullNow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondAvgDiffer, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecDiffRangeYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDiffRangeYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecNowYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNowYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLavgytm, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondTomrtyyear, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLcavg, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLcytm, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondLcclose, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondLfclose, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondAI, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondBasisvalue, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondConvexity, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondMduration, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDuration, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNewrate, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrZqpj, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrZtpj, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrTstk, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondInstname, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDate, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondBondperiod, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondMrtydate, InitValue);
        }
    }

    /// <summary>
    /// 沪深交易所可转换债券
    /// </summary>
    public class SHSZConvertibleBondDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public SHSZConvertibleBondDetailDataRec()
        {
            StockDetailDataRec.Add(FieldIndex.BondYTMLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondYTMHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondYTMOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetNow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullNow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondAvgDiffer, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecDiffRangeYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDiffRangeYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecNowYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNowYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLavgytm, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondTomrtyyear, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLcavg, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLcytm, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondLcclose, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondLfclose, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondAI, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondBasisvalue, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondConvexity, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondMduration, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDuration, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNewrate, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrZqpj, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrZtpj, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrTstk, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondInstname, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDate, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondBondperiod, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondMrtydate, InitValue);

            StockDetailDataRec.Add(FieldIndex.ZGDiffRange, InitValue);
            StockDetailDataRec.Add(FieldIndex.CZJZ, InitValue);
            StockDetailDataRec.Add(FieldIndex.TLKJ, InitValue);
            StockDetailDataRec.Add(FieldIndex.DQSYL, InitValue);
            StockDetailDataRec.Add(FieldIndex.ZGYJL, InitValue);
            StockDetailDataRec.Add(FieldIndex.CZYJL, InitValue);
            StockDetailDataRec.Add(FieldIndex.PJYJL, InitValue);
            StockDetailDataRec.Add(FieldIndex.SYQX, InitValue);
            StockDetailDataRec.Add(FieldIndex.ZQYE, InitValue);
            StockDetailDataRec.Add(FieldIndex.ZGJZ, InitValue);
            StockDetailDataRec.Add(FieldIndex.ZTPJ, InitValue);
            StockDetailDataRec.Add(FieldIndex.ZQPJ, InitValue);
        }
    }

    /// <summary>
    /// 沪深交易所非可转债
    /// </summary>
    public class SHSZNonConvertibleBondDetailDataRec : OneDetailDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public SHSZNonConvertibleBondDetailDataRec()
        {
            StockDetailDataRec.Add(FieldIndex.BondYTMLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullLow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondYTMHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullHigh, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondYTMOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullOpen, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNetNow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondFullNow, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondAvgDiffer, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecDiffRangeYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDiffRangeYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecNowYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNowYTM, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLavgytm, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondTomrtyyear, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLcavg, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDecLcytm, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondLcclose, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondLfclose, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondAI, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondBasisvalue, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondConvexity, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondMduration, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDuration, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondNewrate, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrZqpj, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrZtpj, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondStrTstk, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondInstname, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondDate, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondBondperiod, InitValue);
            StockDetailDataRec.Add(FieldIndex.BondMrtydate, InitValue);
        }
    }
   
    */
    #endregion

    #region 2.分时走势数据结构
    /// <summary>
    /// 每分钟数据结构
    /// </summary>
    public class OneMinuteDataRec
    {
        public double Amount;
        public float AverPrice;
        public long BuyVolume;
        public double Fast;
        public double Low;
        public long Nei;
        public long NeiWaiCha;
        public float Price;
        public long SellVolume;
        public long Volume;
        public long Wai;
        ///// <summary>
        ///// 价格
        ///// </summary>
        //public float Price;

        ///// <summary>
        ///// 分钟成交量
        ///// </summary>
        //public long Volume;

        ///// <summary>
        ///// 成交额
        ///// </summary>
        //public double Amount;

        ///// <summary>
        ///// 均价 (分钟金额/分钟量)
        ///// </summary>
        //public float AverPrice;

        ///// <summary>
        ///// 分钟委买数
        ///// </summary>
        //public int BuyVolume;

        ///// <summary>
        ///// 分钟委卖数
        ///// </summary>
        //public int SellVolume;

        ///// <summary>
        ///// 内外盘差
        ///// </summary>
        //public long NeiWaiCha;
    }

    /// <summary>
    /// 股指期货每分钟数据结构
    /// </summary>
    public class OneMinuteIFDataRec : OneMinuteDataRec
    {
        /// <summary>
        /// 持仓量
        /// </summary>
        public int OpenInterest;
    }

    /// <summary>
    /// 一只股票每天的走势数据
    /// </summary>
    public class OneDayTrendDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 上次该股票该天走势数据，请求到第几个点(本地保存到第几个点)
        /// </summary>
        public short RequestLastPoint;

        /// <summary>
        /// 上次该股票该天走势数据，请求到第几个点(本地保存到第几个点),(盘前)
        /// </summary>
        public short PreRequestLastPoint;

        /// <summary>
        /// 
        /// </summary>
        public float High;
        /// <summary>
        /// 
        /// </summary>
        public float Low;
        /// <summary>
        /// 
        /// </summary>
        public float PreClose;
        /// <summary>
        /// 
        /// </summary>
        public float Open;

        /// <summary>
        /// 开盘阶段每分钟走势数据
        /// </summary>
        private OneMinuteDataRec[] _mintDatas;

        public OneMinuteDataRec[] MintDatas
        {
            get { return _mintDatas; }
            private set { _mintDatas = value; }
        }
        /// <summary>
        /// 盘前每分钟走势数据
        /// </summary>
        private OneMinuteDataRec[] _preMintDatas;

        public OneMinuteDataRec[] PreMintDatas
        {
            get { return _preMintDatas; }
            private set { _preMintDatas = value; }
        }

        private static int _preMintNum = 15;
        private static int _preMintIFNum = 5;
        /// <summary>
        /// 
        /// </summary>
        public OneDayTrendDataRec(int code)
        {
            RequestLastPoint = -1;

            Code = code;
            short totalPoint = TimeUtilities.GetTrendTotalPoint(code);
            MintDatas = new OneMinuteDataRec[totalPoint];

            MarketType marketType = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
            marketType = (MarketType)mtInt;

            if (marketType == MarketType.IF || marketType == MarketType.GoverFutures)
            {
                for (int i = 0; i < MintDatas.Length; i++)
                    MintDatas[i] = new OneMinuteIFDataRec();

                PreMintDatas = new OneMinuteDataRec[_preMintIFNum];

                for (int i = 0; i < _preMintIFNum; i++)
                    PreMintDatas[i] = new OneMinuteIFDataRec();
            }
            else
            {
                for (int i = 0; i < MintDatas.Length; i++)
                    MintDatas[i] = new OneMinuteDataRec();

                PreMintDatas = new OneMinuteDataRec[_preMintNum];
                for (int i = 0; i < _preMintNum; i++)
                    PreMintDatas[i] = new OneMinuteDataRec();
            }

        }
    }


    #endregion

    #region 3.日线数据结构

    /// <summary>
    /// 一只股票一的根蜡烛线数据结构
    /// </summary>
    public class OneDayDataRec
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;
        /// <summary>
        /// 最高价
        /// </summary>
        public float High;
        /// <summary>
        /// 
        /// </summary>
        public float Open;
        /// <summary>
        /// 最低价
        /// </summary>
        public float Low;
        /// <summary>
        /// 昨收
        /// </summary>
        public float Close;
        /// <summary>
        /// 成交量
        /// </summary>
        public long Volume;
        /// <summary>
        /// 成交金额 
        /// </summary>
        public double Amount;

        /// <summary>
        /// 深拷贝
        /// </summary>
        public OneDayDataRec DeepCopy()
        {
            OneDayDataRec temp = new OneDayDataRec();
            temp.Date = this.Date;
            temp.Time = this.Time;
            temp.High = this.High;
            temp.Open = this.Open;
            temp.Low = this.Low;
            temp.Close = this.Close;
            temp.Volume = this.Volume;
            temp.Amount = this.Amount;
            return temp;
        }

        public void Clear()
        {
            this.Date = 0;
            this.Time = 0;
            this.High = 0;
            this.Open = 0;
            this.Low = 0;
            this.Close = 0;
            this.Volume = 0;
            this.Amount = 0;
        }

    }

    /// <summary>
    /// 资金流日K线  数据对象
    /// </summary>
    public class CapitalFlowDayKLineDataRecs
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 周期
        /// </summary>
        public KLineCycle Cycle;

        /// <summary>
        /// K线数据集合(Key: 时间; value: 单笔数据)
        /// </summary>
        public SortedDictionary<int, TrendCaptialFlowDataRec> SortDicCaptialFlowList;

        /// <summary>
        /// K线数据集合(单笔数据集合)
        /// </summary>
        public List<TrendCaptialFlowDataRec> CaptialFlowList
        {
            get
            {
                List<TrendCaptialFlowDataRec> result = new List<TrendCaptialFlowDataRec>();
                int count = SortDicCaptialFlowList.Count;
                if (count > 0)
                {
                    TrendCaptialFlowDataRec[] tempResult = new TrendCaptialFlowDataRec[count];

                    SortDicCaptialFlowList.Values.CopyTo(tempResult, 0);
                    result.AddRange(tempResult);
                }

                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public CapitalFlowDayKLineDataRecs()
        {
            SortDicCaptialFlowList = new SortedDictionary<int, TrendCaptialFlowDataRec>();
        }
        /// <summary>
        /// 
        /// </summary>
        public CapitalFlowDayKLineDataRecs(int applySize)
        {
            SortDicCaptialFlowList = new SortedDictionary<int, TrendCaptialFlowDataRec>();
        }
        /// <summary>
        /// 
        /// </summary>
        public SortedDictionary<int, TrendCaptialFlowDataRec> TrendCaptialFlowRecsCopy()
        {
            SortedDictionary<int, TrendCaptialFlowDataRec> result = new SortedDictionary<int, TrendCaptialFlowDataRec>();

            foreach (KeyValuePair<int, TrendCaptialFlowDataRec> item in SortDicCaptialFlowList)
            {
                result[item.Key] = item.Value.DeepCopy();
            }

            return result;
        }
    }

    /// <summary>
    /// 一只股票的K线数据集合
    /// </summary>
    public class OneStockKLineDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 周期
        /// </summary>
        public KLineCycle Cycle;

        /// <summary>
        /// K线数据集合
        /// </summary>
        public List<OneDayDataRec> OneDayDataList;

        /// <summary>
        /// 
        /// </summary>
        public OneStockKLineDataRec()
        {
            OneDayDataList = new List<OneDayDataRec>(0);
        }
        /// <summary>
        /// 
        /// </summary>
        public OneStockKLineDataRec(int applySize)
        {
            OneDayDataList = new List<OneDayDataRec>(applySize);
        }
        /// <summary>
        /// 
        /// </summary>
        public List<OneDayDataRec> OneDayDataListCopy()
        {
            try
            {
                List<OneDayDataRec> resultCopy = new List<OneDayDataRec>(0);
                for (int i = 0; i < OneDayDataList.Count; i++)
                {
                    resultCopy.Add(OneDayDataList[i].DeepCopy());
                }
                return resultCopy;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 封装k线结构、新闻研报数据、除复权数据的类
    /// </summary>
    public class DrawKlineDataStru
    {
        /// <summary>
        /// 一根k线数据
        /// </summary>
        public OneDayDataRec KlineData;
        /// <summary>
        /// 新闻研报index
        /// </summary>
        public List<OneInfoMineOrgDataRec> NewsData;
        /// <summary>
        /// 除复权index
        /// </summary>
        public List<OneDivideRightBase> ChuFuQuanData;
        /// <summary>
        /// 前复权复权因子
        /// </summary>
        public float ForwardFactor;
        /// <summary>
        /// 后复权
        /// </summary>
        public float BackFactor;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DrawKlineDataStru()
        {
            KlineData = null;
            NewsData = new List<OneInfoMineOrgDataRec>(0);
            ChuFuQuanData = new List<OneDivideRightBase>(0);
            ForwardFactor = 1.0f;
            BackFactor = 1.0f;
        }
    }

    #endregion

    #region 4.成交明细
    /// <summary>
    /// 一条成交记录的结构
    /// </summary>
    public class OneDealDataRec
    {
        /// <summary>
        /// 时
        /// </summary>
        public byte Hour;
        /// <summary>
        /// 分
        /// </summary>
        public byte Mint;
        /// <summary>
        /// 秒
        /// </summary>
        public byte Second;

        /// <summary>
        /// 买卖标记；0卖，1买，2中间，3集合竞价，其他未知
        /// </summary>
        public byte Flag;

        /// <summary>
        /// 价格
        /// </summary>
        public float Price;

        /// <summary>
        /// 成交量
        /// </summary>
        public long Volume;

        /// <summary>
        /// 成交笔数
        /// </summary>
        public short TradeNum;


        /// <summary>
        /// 唯一标识
        /// </summary>
        public int UId;
    }

    /// <summary>
    /// 股指期货一条成交记录的结构
    /// </summary>
    public class OneIndexFuturesDealDataRec : OneDealDataRec
    {
        /// <summary>
        /// 开仓手数
        /// </summary>
        public short OpenVolume;

        /// <summary>
        /// 平仓手数
        /// </summary>
        public short CloseVolume;

        /// <summary>
        /// 开平性质
        /// </summary>
        public string OpenCloseStatus;
    }

    /// <summary>
    /// 债券一条成交记录的结构，多了个ytm
    /// </summary>
    public class OneBondDealDataRec : OneDealDataRec
    {
        /// <summary>
        /// 债券收益率
        /// </summary>
        public float BondYtm;
    }

    /// <summary>
    /// 一只股票的成交明细
    /// </summary>
    public class OneStockDealDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 
        /// </summary>
        public List<OneDealDataRec> DealDatas;
    }
    #endregion

    #region 5.个股资金流向

    /// <summary>
    /// 一只股票最新的资金流向
    /// </summary>
    public class CapitalFlowDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 现价
        /// </summary>
        public float Now
        {
            get { return _now; }
            set { _now = value; }
        }

        /// <summary>
        /// 涨跌幅
        /// </summary>
        public float DiffRange
        {
            get { return _diffRange; }
            set { _diffRange = value; }
        }

        /// <summary>
        /// 集合竞价成交额
        /// </summary>
        public double AmountCallAution
        {
            get { return _amountCallAution; }
            set { _amountCallAution = value; }
        }

        /// <summary>
        ///  0小，1中，2大，3特大
        /// </summary>
        public CapitalFlowDetailItem[] FlowItems;

        /// <summary>
        /// 0当日，1三日，2五日，3十日
        /// </summary>
        public OneDayCapitalFlow[] FlowDays;

        private int _code;
        private float _now;
        private float _diffRange;
        private double _amountCallAution;

        /// <summary>
        /// 
        /// </summary>
        public CapitalFlowDataRec()
        {
            FlowItems = new CapitalFlowDetailItem[4];
            FlowDays = new OneDayCapitalFlow[4];
        }

    }

    /// <summary>
    /// 特大/大/中/小/单的资金流明细
    /// </summary>
    public class CapitalFlowDetailItem
    {
        private double _amountBuy;
        private double _amountSell;
        private double _amountAdd;
        private float _amountAddRange;
        private double _amountNet;
        private float _amountNetRanger;
        private double _volumeBuy;
        private double _volumeSell;
        private uint _bishuBuy;
        private uint _bishuSell;

        /// <summary>
        /// 流入
        /// </summary>
        public double AmountBuy
        {
            get { return _amountBuy; }
            set { _amountBuy = value; }
        }

        /// <summary>
        /// 流出
        /// </summary>
        public double AmountSell
        {
            get { return _amountSell; }
            set { _amountSell = value; }
        }

        /// <summary>
        /// 总量
        /// </summary>
        public double AmountAdd
        {
            get { return _amountAdd; }
            set { _amountAdd = value; }
        }

        /// <summary>
        /// 总量占比
        /// </summary>
        public float AmountAddRange
        {
            get { return _amountAddRange; }
            set { _amountAddRange = value; }
        }

        /// <summary>
        /// 净流量
        /// </summary>
        public double AmountNet
        {
            get { return _amountNet; }
            set { _amountNet = value; }
        }

        /// <summary>
        /// 净占比
        /// </summary>
        public float AmountNetRanger
        {
            get { return _amountNetRanger; }
            set { _amountNetRanger = value; }
        }

        /// <summary>
        /// 买盘成交量
        /// </summary>
        public double VolumeBuy
        {
            get { return _volumeBuy; }
            set { _volumeBuy = value; }
        }

        /// <summary>
        /// 卖盘成交量
        /// </summary>
        public double VolumeSell
        {
            get { return _volumeSell; }
            set { _volumeSell = value; }
        }

        /// <summary>
        /// 买盘成交笔数
        /// </summary>
        public uint BishuBuy
        {
            get { return _bishuBuy; }
            set { _bishuBuy = value; }
        }

        /// <summary>
        /// 卖盘成交笔数
        /// </summary>
        public uint BishuSell
        {
            get { return _bishuSell; }
            set { _bishuSell = value; }
        }
    }

    /// <summary>
    /// 一天的资金流向
    /// </summary>
    public class OneDayCapitalFlow
    {
        private double _superBuy;
        private double _largeBuy;
        private double _superSell;
        private double _largeSell;
        private double _middleBuy;
        private double _middleSell;
        private double _smallBuy;
        private double _smallSell;
        private double _amount;
        private float _largeNetRange;
        private short _rankRec;
        private short _rankChange;
        private short _hisNetRank;
        private float _hisNetRange;
        private float _percent;

        /// <summary>
        /// 特大单流入
        /// </summary>
        public double SuperBuy
        {
            get { return _superBuy; }
            set { _superBuy = value; }
        }

        /// <summary>
        /// 特大单流出
        /// </summary>
        public double SuperSell
        {
            get { return _superSell; }
            set { _superSell = value; }
        }

        /// <summary>
        /// 大单流入
        /// </summary>
        public double LargeBuy
        {
            get { return _largeBuy; }
            set { _largeBuy = value; }
        }

        /// <summary>
        /// 大单流出
        /// </summary>
        public double LargeSell
        {
            get { return _largeSell; }
            set { _largeSell = value; }
        }

        /// <summary>
        /// 中单流入
        /// </summary>
        public double MiddleBuy
        {
            get { return _middleBuy; }
            set { _middleBuy = value; }
        }

        /// <summary>
        /// 中单流出
        /// </summary>
        public double MiddleSell
        {
            get { return _middleSell; }
            set { _middleSell = value; }
        }

        /// <summary>
        /// 小单流入
        /// </summary>
        public double SmallBuy
        {
            get { return _smallBuy; }
            set { _smallBuy = value; }
        }

        /// <summary>
        /// 小单流出
        /// </summary>
        public double SmallSell
        {
            get { return _smallSell; }
            set { _smallSell = value; }
        }

        /// <summary>
        /// 金额
        /// </summary>
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// 大单净占比
        /// </summary>
        public float LargeNetRange
        {
            get { return _largeNetRange; }
            set { _largeNetRange = value; }
        }

        /// <summary>
        /// 排名
        /// </summary>
        public short RankRec
        {
            get { return _rankRec; }
            set { _rankRec = value; }
        }

        /// <summary>
        /// 排名变化
        /// </summary>
        public short RankChange
        {
            get { return _rankChange; }
            set { _rankChange = value; }
        }

        /// <summary>
        /// 昨日大单净占比排名
        /// </summary>
        public short HisNetRank
        {
            get { return _hisNetRank; }
            set { _hisNetRank = value; }
        }

        /// <summary>
        /// 昨日大单净占比
        /// </summary>
        public float HisNetRange
        {
            get { return _hisNetRange; }
            set { _hisNetRange = value; }
        }

        /// <summary>
        /// 百分比？
        /// </summary>
        public float Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }
    }
    #endregion

    #region 6.分价表

    /// <summary>
    /// 一只股票一天的分价表
    /// </summary>
    public class PriceStatusDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 
        /// </summary>
        public List<OnePriceStatus> PriceStatusList;

        /// <summary>
        /// 
        /// </summary>
        public PriceStatusDataRec()
        {
            PriceStatusList = new List<OnePriceStatus>();
        }
    }

    /// <summary>
    /// 一只股票一个价格的结构
    /// </summary>
    public class OnePriceStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public float Price;

        /// <summary>
        /// 买入量
        /// </summary>
        public double BuyVolume;

        /// <summary>
        /// 卖出量
        /// </summary>
        public double SellVolume;
    }
    #endregion

    #region 7.综合排名
    /// <summary>
    /// 
    /// </summary>
    public class RankDataRec
    {
        /// <summary>
        /// 证券类别
        /// </summary>
        public ReqSecurityType SType;

        /// <summary>
        /// 0涨幅，1跌幅，2五分钟涨幅，3五分钟跌幅，4振幅，5总成交量，6量比，7委比前几，8委比后几，9总金额
        /// </summary>
        private List<RankElement>[] _rankElementArray;

        public List<RankElement>[] RankElementArray
        {
            get { return _rankElementArray; }
            private set { _rankElementArray = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public RankDataRec()
        {
            RankElementArray = new List<RankElement>[10];
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RankElement
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public float PreClose;
        /// <summary>
        /// 现价
        /// </summary>
        public float Now;
        /// <summary>
        /// 成交量
        /// </summary>
        public int Volume;

        /// <summary>
        /// 指标数值
        /// </summary>
        public float IndexValue;
    }

    #endregion

    #region 8.F10基本资料
    /// <summary>
    /// 一只股票F10的数据
    /// </summary>
    public class F10DataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;
        /// <summary>
        /// F10字段
        /// </summary>
        public Dictionary<int, F10Field> F10FieldData;
        /// <summary>
        /// 
        /// </summary>
        public F10DataRec()
        {
            F10FieldData = new Dictionary<int, F10Field>(16);
        }
    }

    /// <summary>
    /// 一个F10栏位的数据
    /// </summary>
    public class F10Field
    {
        // public F10Type Field ;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;
        /// <summary>
        /// 内容
        /// </summary>
        public string Context;
    }

    #endregion

    #region 9.服务端板块

    public enum EnumBlockType
    {
        /// <summary>
        /// 全部
        /// </summary>
        All,
        /// <summary>
        /// 行业
        /// </summary>
        Industry,
        /// <summary>
        /// 地区
        /// </summary>
        Area,
        /// <summary>
        /// 概念
        /// </summary>
        Conception,
    }

    /// <summary>
    /// 板块信息
    /// </summary>
    public class BlockDataRec
    {
        private Dictionary<string, string> _areaBlock;
        private Dictionary<EnumBlockType, List<int>> _blockItems;
        private Dictionary<string, string> _conceptionBlock;
        private Dictionary<string, string> _industryBlock;

        /// <summary>
        /// 行业板块，<中文名称,blockid>
        /// </summary>
        public Dictionary<string, string> IndustryBlock
        {
            get { return _industryBlock; }
            private set { _industryBlock = value; }
        }

        /// <summary>
        /// 地区板块<中文名称,blockid>
        /// </summary>
        public Dictionary<string, string> AreaBlock
        {
            get { return _areaBlock; }
            private set { _areaBlock = value; }
        }

        /// <summary>
        /// 概念板块<中文名称,blockid>
        /// </summary>
        public Dictionary<string, string> ConceptionBlock
        {
            get { return _conceptionBlock; }
            private set { _conceptionBlock = value; }
        }

        /// <summary>
        /// 板块成分
        /// </summary>
        public Dictionary<EnumBlockType, List<int>> BlockItems
        {
            get { return _blockItems; }
            private set { _blockItems = value; }
        }

        /// <summary>
        /// 获取板块成分
        /// </summary>
        public Dictionary<string, string> GetBlockItems(string blockType)
        {
            switch (blockType)
            {
                case "industryblock":
                    return IndustryBlock;
                case "conceptionblock":
                    return ConceptionBlock;
                case "areablock":
                    return AreaBlock;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取板块成分
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<int> GetBlockItems(EnumBlockType type)
        {
            List<int> result;
            if (!BlockItems.TryGetValue(type, out result))
                result = new List<int>();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public BlockDataRec(Dictionary<string, string> industry, Dictionary<string, string> area, Dictionary<string, string> conception)
        {
            IndustryBlock = industry;
            AreaBlock = area;
            ConceptionBlock = conception;
            BlockItems = new Dictionary<EnumBlockType, List<int>>(4);
        }

        public void SetData(List<int> industry, List<int> area, List<int> concept)
        {
            if (BlockItems == null)
                BlockItems = new Dictionary<EnumBlockType, List<int>>(4);
            int sumCount = 0;
            if (industry != null)
                sumCount += industry.Count;
            if (area != null)
                sumCount += area.Count;
            if (concept != null)
                sumCount += concept.Count;
            List<int> all = new List<int>(sumCount);
            if (industry != null) all.AddRange(industry);
            if (area != null) all.AddRange(area);
            if (concept != null) all.AddRange(concept);
            BlockItems.Add(EnumBlockType.Area, area);
            BlockItems.Add(EnumBlockType.Industry, industry);
            BlockItems.Add(EnumBlockType.Conception, concept);
            BlockItems.Add(EnumBlockType.All, all);
        }

    }
    #endregion

    #region 10.服务端码表
    ///// <summary>
    ///// 码表结构（行情服务端）
    ///// </summary>
    //public class StockDictDataRec
    //{

    //    public int Date ;

    //    /// <summary>
    //    /// 按市场分的结构，string指ReqMarketType
    //    /// </summary>
    //    public Dictionary<string, OneBlockItem> MarketBlock ;

    //    public StockDictDataRec()
    //    {
    //        MarketBlock = new Dictionary<string, OneBlockItem>();
    //    }
    //}
    #endregion

    #region 11.信息地雷
    /// <summary>
    /// 研报类型
    /// </summary>
    public enum ReportType : byte
    {
        /// <summary>
        /// 1:新闻式研报（returnType=1， 带有“新闻标题”）
        /// </summary>
        NewsReport = 1,
        /// <summary>
        /// 2:机构评级式研报（returnType=2 ）
        /// </summary>
        EmratingReport = 2
      
    }

    /// <summary>
    /// 地雷信息数据结构
    /// </summary>
    public class InfoMineDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<InfoMine, List<OneInfoMineDataRec>> InfoMineData;

        /// <summary>
        /// 
        /// </summary>
        public InfoMineDataRec()
        {
            InfoMineData = new Dictionary<InfoMine, List<OneInfoMineDataRec>>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OneInfoMineDataRec
    {
        /// <summary>
        /// 
        /// </summary>
        public InfoMine InfoType;
        /// <summary>
        /// 
        /// </summary>
        public long TextId;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 日期
        /// </summary>
        public int PublishDate;
        /// <summary>
        /// 时间
        /// </summary>
        public int PublishTime;
        /// <summary>
        /// url
        /// </summary>
        public string ContentUrl;
        /// <summary>
        /// 是否点击查看过
        /// </summary>
        public bool HasShown;
    }

    /// <summary>
    /// 信息地雷数据结构（机构版）
    /// </summary>
    public class InfoMineOrgDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> InfoMineData;

        /// <summary>
        /// 
        /// </summary>
        public InfoMineOrgDataRec()
        {
            InfoMineData = new Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>>();
        }
    }

    /// <summary>
    /// 一条资讯(机构版)
    /// </summary>
    public class OneInfoMineOrgDataRec : IComparable
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;
        /// <summary>
        /// 类型
        /// </summary>
        public InfoMineOrg InfoType;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 日期
        /// </summary>
        public int PublishDate;
        /// <summary>
        /// 时间
        /// </summary>
        public int PublishTime;
        /// <summary>
        /// 是否点击查看过
        /// </summary>
        public bool HasShown;
        /// <summary>
        /// 资讯编码
        /// </summary>
        public string InfoCode;
        /// <summary>
        /// 新闻公告中表示来源，研报表示机构名称
        /// </summary>
        public string MediaName;

        /// <summary>
        /// 星级
        /// </summary>
        public byte Star;

        /// <summary>
        /// 评级内容
        /// </summary>
        public EmratingValue EmratingValue;

        /// <summary>
        /// url
        /// </summary>
        public string Url;

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop;

        public int CompareTo(object obj)
        {
            int result = 0;
            if (null != obj && obj is OneInfoMineOrgDataRec)
            {
                OneInfoMineOrgDataRec other = obj as OneInfoMineOrgDataRec;

                long otherTime = ((long)other.PublishDate) * 1000000 + other.PublishTime;
                long thisTime = ((long)this.PublishDate) * 1000000 + this.PublishTime;
                if (thisTime > otherTime)
                {
                    result = -1;
                }
                if (thisTime < otherTime)
                {
                    result = 1;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 新的机构研报结构
    /// </summary>
    public class OneInfoMineOrgDataRecWithInsSName : OneInfoMineOrgDataRec
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        public string InsSName;
    }

    /// <summary>
    /// 自选股新闻的请求参数
    /// </summary>
    public class CustomStockNewsParam
    {
        private byte _market;
        private string _shortCode;
        public int DateStart, TimeStart, DateEnd, TimeEnd;
        /// <summary>
        /// 是否
        /// </summary>
        public bool IsValide
        {
            get { return _isValide; }
            private set { _isValide = value; }
        }

        /// <summary>
        /// 内码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);
                DataPacket.ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
                if (tmp == ReqMarketType.MT_NA)
                    IsValide = false;
                else
                    IsValide = true;
            }
        }
        private int _code;
        private bool _isValide;

        /// <summary>
        /// 获取财富通代码
        /// </summary>
        /// <param name="shortCode"></param>
        /// <param name="market"></param>
        public void GetCode(out string shortCode, out byte market)
        {
            shortCode = _shortCode;
            market = _market;
        }

    }
    #endregion

    #region 12.机构评级
    /// <summary>
    /// 机构评级（个股研报）
    /// </summary>
    public class OrgRateDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 更新日期
        /// </summary>
        public int UpdateDate;
        /// <summary>
        /// 更新时间
        /// </summary>
        public int UpdateTime;
        /// <summary>
        /// 
        /// </summary>
        public List<OneOrgRateItem> OrgRateDataList;
        /// <summary>
        /// 
        /// </summary>
        public OrgRateDataRec()
        {
            OrgRateDataList = new List<OneOrgRateItem>();
        }
    }
    /// <summary>
    /// 一条评级内容
    /// </summary>
    public class OneOrgRateItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id;

        /// <summary>
        /// 链接
        /// </summary>
        public string Url;

        /// <summary>
        /// 撰写时间(yyyymmdd)
        /// </summary>
        public int WrittenDate;

        /// <summary>
        /// 评级
        /// </summary>
        public string Rate;

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName;

        /// <summary>
        /// 研报标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 机构影响力
        /// </summary>
        public byte Influence;

        /// <summary>
        /// 年度实际每股收益
        /// </summary>
        public double ProfitPerShare1;

        /// <summary>
        /// 年份(yyyy)
        /// </summary>
        public short Forecast1;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue1;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare2;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast2;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue2;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare3;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast3;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue3;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare4;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast4;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue4;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare5;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast5;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue5;
    }


    /// <summary>
    /// 一条评级内容(new)
    /// </summary>
    public class OneInfoRateOrgItem
    {
        /// <summary>
        /// 资讯编码
        /// </summary>
        public string InfoCode;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 资讯类型，3表示机构评级
        /// </summary>
        public byte TypeCode;

        /// <summary>
        /// 机构名称
        /// </summary>
        public string MediaName;

        /// <summary>
        /// 星级
        /// </summary>
        public byte Star;

        /// <summary>
        /// 评级
        /// </summary>
        public EmratingValue EmratingValue;

        /// <summary>
        /// url
        /// </summary>
        public string Url;
    }
    /// <summary>
    /// (全景图上的)研究报告中个股评级条目
    /// </summary>
    public class ResearchReportItem
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string InfoCode;
        /// <summary>
        /// 股票代码（不带后缀）
        /// </summary>
        public string SecuCode;
        /// <summary>
        /// 股票名称
        /// </summary>
        public string SecuSName;
        /// <summary>
        /// 股票内码
        /// </summary>
        public int SecuVarietyCode;
        /// <summary>
        /// 评级
        /// </summary>
        public EmratingValue EmratingValue;
        /// <summary>
        /// 机构名称
        /// </summary>
        public string InsSName;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;
    }

    /// <summary>
    /// 债券综合屏-公开市场操作模块的每行数据
    /// </summary>
    public class BondPublicOpeartionItem
    {
        /// <summary>
        /// 债券交易日期, 格式 "yyyy-MM-dd"
        /// </summary>
        public string BondDate;
        /// <summary>
        /// 利率(%)
        /// </summary>
        public string CouponRate;
        /// <summary>
        /// 发行总量(亿元)
        /// </summary>
        public string IssueVol;
        /// <summary>
        /// 期限
        /// </summary>
        public string Period;
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OType;
    }
    #endregion

    #region 13.盈利预测
    /// <summary>
    /// 盈利预测
    /// </summary>
    public class OneProfitForecastDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 年度实际每股收益
        /// </summary>
        public double ProfitPerShare1;

        /// <summary>
        /// 年份(yyyy)
        /// </summary>
        public short Forecast1;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue1;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare2;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast2;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue2;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare3;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast3;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue3;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare4;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast4;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue4;

        /// <summary>
        /// 年度预测每股收益
        /// </summary>
        public double ProfitPerShare5;

        /// <summary>
        /// 预测年份 (yyyy)
        /// </summary>
        public short Forecast5;

        /// <summary>
        /// 1 真实数据，0 预测数据
        /// </summary>
        public byte RealValue5;
    }

    /// <summary>
    /// 盈利预测（机构版）
    /// </summary>
    public class OneProfitForecastOrgDataRec
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 预测数据
        /// </summary>
        public List<ProfitForecast> ProfitForecastData;

    }

    /// <summary>
    /// 一只股票一个盈利预测的结构
    /// </summary>
    public class ProfitForecast
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int PredictYear;
        /// <summary>
        /// 预测每股收益
        /// </summary>
        public float BasicEPS;
        /// <summary>
        /// 预测pe
        /// </summary>
        public float PredictPe;

    }
    #endregion

    #region 14.逐笔成交




    /// <summary>
    /// 一条成交记录的结构
    /// </summary>
    public class OneTickDataRec
    {
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 买卖标记；0卖，1买，2中间，3集合竞价，其他未知
        /// </summary>
        public byte Flag;

        /// <summary>
        /// 在所有逐笔数据中的下标
        /// </summary>
        public int Index;

        /// <summary>
        /// 价格
        /// </summary>
        public int Price;

        /// <summary>
        /// 成交量
        /// </summary>
        public int Volume;
    }

    /// <summary>
    /// 一只股票的成交明细
    /// </summary>
    public class OneStockTickDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 逐笔List，时间小的在前
        /// </summary>
        public List<OneTickDataRec> TickDatasList;
    }
    #endregion

    #region 15.委托明细(深证)
    /// <summary>
    /// 一条成交记录的结构
    /// </summary>
    public class OneOrderDetailDataRec
    {
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 买卖标记；0卖，1买，2中间，3集合竞价，其他未知
        /// </summary>
        public byte Flag;

        /// <summary>
        /// 在所有委托数据中的下标
        /// </summary>
        public int Index;

        /// <summary>
        /// 价格
        /// </summary>
        public int Price;

        /// <summary>
        /// 成交量
        /// </summary>
        public int Volume;
    }

    /// <summary>
    /// 一只股票的委托明细
    /// </summary>
    public class OneStockOrderDetailDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 委托明细List，时间小的在前
        /// </summary>
        public List<OneOrderDetailDataRec> OrderDetailList;
    }
    #endregion

    #region 16.指数红绿柱
    /// <summary>
    /// 每一根红绿柱 
    /// </summary>
    public class OneMintRedGreen
    {
        /// <summary>
        /// 
        /// </summary>
        public float Fast;
        /// <summary>
        /// 
        /// </summary>
        public float Slow;
    }

    /// <summary>
    /// 一只股票一天的红绿柱
    /// </summary>
    public class OneDayRedGreenDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 上次该股票该天走势数据，请求到第几个点(本地保存到第几个点)
        /// </summary>
        public short RequestLastPoint;
        /// <summary>
        /// 
        /// </summary>
        public OneMintRedGreen[] MintDatas;
        /// <summary>
        /// 
        /// </summary>
        public OneDayRedGreenDataRec(int code)
        {
            RequestLastPoint = -1;

            Code = code;
            short totalPoint = TimeUtilities.GetTrendTotalPoint(code);
            MintDatas = new OneMintRedGreen[totalPoint];

            for (int i = 0; i < MintDatas.Length; i++)
                MintDatas[i] = new OneMintRedGreen();
        }
    }
    #endregion

    #region 17.委托队列
    /// <summary>
    /// 委托队列一笔挂单的结构
    /// </summary>
    public class OrderQueueItem
    {
        /// <summary>
        /// 当前挂单的成交量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 状态
        /// </summary>
        public OrderQueueItemStatus Status;
    }

    /// <summary>
    /// 委托队列部分成交的结构
    /// </summary>
    public class OrderQueuePartDealItem : OrderQueueItem
    {
        /// <summary>
        /// 成交掉的成交量
        /// </summary>
        public int DealVolume;
    }

    /// <summary>
    /// 委托队列数据结构
    /// </summary>
    public class OrderQueueDataRec
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 买卖类型，1买，2卖
        /// </summary>
        public byte BuySell;

        /// <summary>
        /// 总共有多少笔
        /// </summary>
        public int TotalOrderNum;

        /// <summary>
        /// 显示多少笔
        /// </summary>
        public byte ShowOrderNum;

        /// <summary>
        /// 每笔的数据集合
        /// </summary>
        public List<OrderQueueItem> ItemDatas;
        /// <summary>
        /// 
        /// </summary>
        public OrderQueueDataRec()
        {
            ItemDatas = new List<OrderQueueItem>();
        }
    }

    #endregion

    #region 18.短线精灵
    /// <summary>
    /// 短线精灵数据))))
    /// </summary>
    public class OneShortLineDataRec
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ShortLineType SlType;

        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code;

        /// <summary>
        /// id号
        /// </summary>
        public int SeriesId;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content;
    }
    #endregion

    #region 19.贡献点数
    /// <summary>
    /// 
    /// </summary>
    public class ContributionDataRec
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;
        /// <summary>
        /// 价格
        /// </summary>
        public float Price;
    }
    #endregion

    #region 20.除权除息
    /// <summary>
    /// 除权除息
    /// </summary>
    public enum DivideRightType
    {
        /// <summary>
        /// 增发
        /// </summary>
        ZengFa = 1,
        /// <summary>
        /// 配股
        /// </summary>
        PeiGu,
        /// <summary>
        /// 派息
        /// </summary>
        PaiXi,
        /// <summary>
        /// 更名
        /// </summary>
        GengMing,
        /// <summary>
        /// 送股
        /// </summary>
        SongGu,
        /// <summary>
        /// 转增
        /// </summary>
        ZhuanZeng,

    }

    /// <summary>
    /// 一个除复权信息的基类
    /// </summary>
    public class OneDivideRightBase
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 类型
        /// </summary>
        public DivideRightType DivideType;

        /// <summary>
        /// 值
        /// </summary>
        public float DataValue;

        /// <summary>
        /// 复权因子
        /// </summary>
        public float Factor;

    }

    /// <summary>
    /// 增发
    /// </summary>
    public class OneDivideRightZengfa : OneDivideRightBase
    {
        /// <summary>
        /// 增发价格
        /// </summary>
        public float ZengfaPrice;
    }

    /// <summary>
    /// 配股
    /// </summary>
    public class OneDivideRightPeigu : OneDivideRightBase
    {
        /// <summary>
        /// 配股价格
        /// </summary>
        public float PeiguPrice;
    }

    /// <summary>
    /// 更名
    /// </summary>
    public class OneDivideGengMing : OneDivideRightBase
    {
        /// <summary>
        /// 旧名
        /// </summary>
        public string OldName;

        /// <summary>
        /// 新名
        /// </summary>
        public string NewName;
    }

    /// <summary>
    /// 除权除息
    /// </summary>
    public class DivideRightDataRec
    {

        /// <summary>
        /// 发布日期
        /// </summary>
        public int PunishDate;

        /// <summary>
        /// 配股比例
        /// </summary>
        public float PGBL;

        /// <summary>
        /// 配股价格
        /// </summary>
        public float PGJG;

        /// <summary>
        /// 增发比例
        /// </summary>
        public float ZFBL;

        /// <summary>
        /// 增发价格
        /// </summary>
        public float ZFJG;

        /// <summary>
        /// 派现比例(税前派现比例,以10股为单位，如10配几)
        /// </summary>
        public float PXBL;

        /// <summary>
        /// 送股比例
        /// </summary>
        public float SGBL;


    }
    #endregion

    #region 21.滚动新闻24小时
    /// <summary>
    /// 24小时滚动新闻
    /// </summary>
    public class OneNews24HDataRec
    {
        /// <summary>
        /// id
        /// </summary>
        public int NewsID;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public int UpdateDate;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public int UpdateTime;

        /// <summary>
        /// 发布日期
        /// </summary>
        public int PublishDate;

        /// <summary>
        /// 发布时间
        /// </summary>
        public int PublishTime;

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid;

        /// <summary>
        /// url
        /// </summary>
        public string Url;

        /// <summary>
        /// 是否点击查看过
        /// </summary>
        public bool HasShown;
    }

    /// <summary>
    /// 24小时滚动新闻（机构版）
    /// </summary>
    public class OneNews24HOrgDataRec
    {
        /// <summary>
        /// 资讯编码
        /// </summary>
        public string InfoCode;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 日期
        /// </summary>
        public int PublishDate;

        /// <summary>
        /// 时间
        /// </summary>
        public int PublishTime;

        /// <summary>
        /// 来源
        /// </summary>
        public string MediaName;

        /// <summary>
        /// 是否点击查看过
        /// </summary>
        public bool HasShown;
        /// <summary>
        /// url
        /// </summary>
        public string Url;

        public OneNews24HOrgDataRec DeepCopy()
        {
            OneNews24HOrgDataRec temp = new OneNews24HOrgDataRec();
            temp.InfoCode = this.InfoCode;
            temp.Title = this.Title;
            temp.PublishDate = this.PublishDate;
            temp.PublishTime = this.PublishTime;
            temp.MediaName = this.MediaName;
            temp.HasShown = this.HasShown;
            temp.Url = this.Url;

            return temp;
        }
    }
    #endregion

    #region 22.财务数据
    /// <summary>
    /// 一只股票财务数据
    /// </summary>
    public class OneFinanceDataRec
    {
        /// <summary>
        /// 总股本
        /// </summary>
        public double ZGB;
        // public double AvgNetS;
        // public double MGSY;
        // public double MGSY2;
        /// <summary>
        /// 每股净资产
        /// </summary>
        public double MGJZC;
        // public double MGJZC2;
        /// <summary>
        /// 净资产收益率
        /// </summary>
        public double JZC;
        /// <summary>
        /// 营业收入
        /// </summary>
        public double ZYYWSR;
        // public double IncomeRatio;
        // public double ProfitFO;
        // public double InvIncome;
        // public double PBTax;
        /// <summary>
        /// 净利润
        /// </summary>
        public double ZYYWLR;
        // public double NPRatio;
        // public double RProfotAA;
        // public double DRPRPAA;
        // public double Gprofit;
        /// <summary>
        /// 总资产
        /// </summary>
        public double ZZC;
        // public double CurAsset;
        // public double FixAsset;
        // public double IntAsset;
        /// <summary>
        /// 总负债
        /// </summary>
        public double TotalLiab;
        // public double TCurLiab;
        // public double TLongLiab;
        // public double ZCFZL;
        /// <summary>
        /// 股东权益
        /// </summary>
        public double OWnerEqu;
        // public double OEquRatio;
        /// <summary>
        /// 资本公积金
        /// </summary>
        public double CapRes;
        // public double DRPCapRes;
        /// <summary>
        /// 流通A股
        /// </summary>
        public double NetAShare;
        /// <summary>
        /// 流通B股
        /// </summary>
        public double NetBShare;
        // public double Hshare;
        //public double ListDate;
        /// <summary>
        /// 报告期日期
        /// </summary>
        public int BGQDate;
        /// <summary>
        /// 每股收益（最新报告期）
        /// </summary>
        public double MGSY;
        /// <summary>
        /// 每股收益（最新年报）
        /// </summary>
        public double EpsQmtby;
        /// <summary>
        /// 每股收益(最新报告期)
        /// </summary>
        public double EpsTtm;

    }
    #endregion

    #region 23.十档行情Level2
    /// <summary>
    /// 五档行情
    /// </summary>
    public class OneStockDetailLevel1DataRec
    {
        /// <summary>
        /// 股票内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 昨收价
        /// </summary>
        public float PreClose;
    }


    /// <summary>
    /// 十档行情
    /// </summary>
    public class OneStockDetailLevel2DataRec : OneStockDetailLevel1DataRec
    {

    }
    #endregion

    #region 24.基金经理
    /// <summary>
    /// 基金经理
    /// </summary>
    public class FundManagerDataRec
    {
        /// <summary>
        /// 基金经理
        /// </summary>
        public string ManagerName;

        /// <summary>
        /// 任职日期
        /// </summary>
        public int StartDate;

        /// <summary>
        /// 离职日期
        /// </summary>
        public int EndDate;

        /// <summary>
        /// 任职总回报
        /// </summary>
        public float YieldSinces;

        /// <summary>
        /// 年化回报
        /// </summary>
        public float AyieldSinces;

        /// <summary>
        /// 同类排名
        /// </summary>
        public string Rank;

        /// <summary>
        /// 超基准总回报
        /// </summary>
        public float Cyjzhb;
    }
    #endregion

    #region 25.十大流通股东
    /// <summary>
    /// 十大流通股东
    /// </summary>
    public class ShareHolderDataRec
    {
        /// <summary>
        /// 公募家数
        /// </summary>
        public int GmCount;
        /// <summary>
        /// 私募家数
        /// </summary>
        public int SmCount;
        /// <summary>
        /// 社保家数
        /// </summary>
        public int SbCount;
        /// <summary>
        /// Qfii家数
        /// </summary>
        public int QfiiCount;
        /// <summary>
        /// 上期公募家数
        /// </summary>
        public int GmCountHis;
        /// <summary>
        /// 上期私募家数
        /// </summary>
        public int SmCountHis;
        /// <summary>
        /// 上期社保家数
        /// </summary>
        public int SbCountHis;
        /// <summary>
        /// 上期Qfii家数
        /// </summary>
        public int QfiiCountHis;
        /// <summary>
        /// 报告期日期
        /// </summary>
        public int ReportDate;
        /// <summary>
        /// 上期报告期日期
        /// </summary>
        public int LastDate;
        /// <summary>
        /// 股东户数
        /// </summary>
        public int ShareHolderNum;
        /// <summary>
        /// 上期股东户数
        /// </summary>
        public int ShareHolderNumHis;
        /// <summary>
        /// 股东户数环比
        /// </summary>
        public float ShareHolderNumYOY;
        /// <summary>
        /// 上期股东户数环比
        /// </summary>
        public float ShareHolderNumHisYOY;
        /// <summary>
        /// 人均持股
        /// </summary>
        public float ShareAvg;
        /// <summary>
        /// 上期人均持股
        /// </summary>
        public float ShareAvgHis;
    }
    #endregion

    #region 26.利润趋势
    /// <summary>
    /// 利润趋势
    /// </summary>
    public class ProfitTrendDataRec
    {
        /// <summary>
        /// 一只股票的利润趋势
        /// </summary>
        public List<OneYearProfitTrendDataRec> ProfitTrend;

        public ProfitTrendDataRec()
        {
            ProfitTrend = new List<OneYearProfitTrendDataRec>();
        }
    }

    /// <summary>
    /// 每年的利润趋势
    /// </summary>
    public class OneYearProfitTrendDataRec
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 一季度利润
        /// </summary>
        public float Profit1;
        /// <summary>
        /// 二季度利润
        /// </summary>
        public float Profit2;
        /// <summary>
        /// 三季度利润
        /// </summary>
        public float Profit3;
        /// <summary>
        /// 四季度利润
        /// </summary>
        public float Profit4;
    }
    #endregion

    #region 27.综合排名（机构版）
    /// <summary>
    /// 综合排名种类
    /// </summary>
    [Flags]
    public enum RankType
    {
        /// <summary>
        /// 涨幅
        /// </summary>
        DiffRangeTop = 1,
        /// <summary>
        /// 跌幅
        /// </summary>
        DiffRangeBottom = 2,
        /// <summary>
        /// 振幅 
        /// </summary>
        Delta = 4,
        /// <summary>
        /// 5分钟涨幅
        /// </summary>
        DiffRangeTop5Mint = 8,
        /// <summary>
        /// 5分钟跌幅
        /// </summary>
        DiffRangeBottom5Mint = 16,
        /// <summary>
        /// 量比
        /// </summary>
        VolumeRatio = 32,
        /// <summary>
        /// 委比前十
        /// </summary>
        WeiBiTop = 64,
        /// <summary>
        /// 委比后十
        /// </summary>
        WeiBiBottom = 128,
        /// <summary>
        /// 成交金额
        /// </summary>
        Amount = 256
    }
    /// <summary>
    /// 综合排名（机构）
    /// </summary>
    public class RankOrgDataRec
    {
        public Dictionary<RankType, List<int>> RankData;
        /// <summary>
        /// 
        /// </summary>
        public RankOrgDataRec()
        {
            RankData = new Dictionary<RankType, List<int>>(10);
        }
    }

    #endregion

    #region 28.百档行情

    /// <summary>
    /// 每档挂单
    /// </summary>
    public class OneOrderDetail
    {
        /// <summary>
        /// 价格
        /// </summary>
        public float Price;

        /// <summary>
        /// 量
        /// </summary>
        public long Volume;

        /// <summary>
        /// 单数
        /// </summary>
        public int OrderNum;

        /// <summary>
        /// 大单标记, Flag=1表示有大单
        /// </summary>
        public byte Flag;

        /// <summary>
        /// 委托量的差值
        /// </summary>
        public long DeltaVolume;
    }

    /// <summary>
    /// 百档行情
    /// </summary>
    public class StockOrderDetailDataRec
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 委买均价
        /// </summary>
        public float BuyAvgPrice;

        /// <summary>
        /// 委买总量(股)
        /// </summary>
        public long BuyVolume;

        /// <summary>
        /// 委买挂单数量
        /// </summary>
        public int BuyOrderCount;

        /// <summary>
        /// 委买总笔数
        /// </summary>
        public int BuyBiShu;

        /// <summary>
        /// 委买均价
        /// </summary>
        public float SellAvgPrice;

        /// <summary>
        /// 委买总量(股)
        /// </summary>
        public long SellVolume;

        /// <summary>
        /// 委买挂单数量
        /// </summary>
        public int SellOrderCount;

        /// <summary>
        /// 委买总笔数
        /// </summary>
        public int SellBiShu;

        /// <summary>
        /// 委买挂单, 买一在最前面
        /// </summary>
        public List<OneOrderDetail> BuyDetail;

        /// <summary>
        /// 委卖挂单, 卖一在最前面
        /// </summary>
        public List<OneOrderDetail> SellDetail;

        public StockOrderDetailDataRec()
        {
            BuyDetail = new List<OneOrderDetail>(1);
            SellDetail = new List<OneOrderDetail>(1);
        }

    }

    #endregion

    #region 29.百档挂单详细

    /// <summary>
    /// 一个价格的挂单
    /// </summary>
    public class OnePriceOrder
    {
        /// <summary>
        /// 0-交易，1-部分成交, 2-撤单，4-大单, 6-大单撤单
        /// </summary>
        public byte Status;

        /// <summary>
        /// 委托编号
        /// </summary>
        public int OrderId;

        /// <summary>
        /// 委托量
        /// </summary>
        public int Volume;
    }

    /// <summary>
    /// 挂单详细
    /// </summary>
    public class StockPriceOrderQueueDataRec
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 委托价格
        /// </summary>
        public float Price;

        /// <summary>
        /// 0-buy, 1-sell
        /// </summary>
        public byte BSFlag;

        /// <summary>
        /// 挂单队列
        /// </summary>
        public List<OnePriceOrder> OrderData;

        public StockPriceOrderQueueDataRec()
        {
            OrderData = new List<OnePriceOrder>(1);
        }
    }

    #endregion

    #region 30.统计分析Lev2
    /// <summary>
    /// 统计分析
    /// </summary>
    public class StatisticsAnalysisDataRec
    {
        /// <summary>
        /// // 买入撤单笔数
        /// </summary>
        public int WithdrawBuyNumber;

        /// <summary>
        /// // 卖出撤单笔数
        /// </summary>
        public int WithdrawSellNumber;

        /// <summary>
        /// // 买入撤单量
        /// </summary>
        public long WithdrawBuyAmount;

        /// <summary>
        /// // 卖出撤单量
        /// </summary>
        public long WithdrawSellAmount;

        /// <summary>
        /// // 买入撤单额
        /// </summary>
        public double WithdrawBuyMoney;

        /// <summary>
        /// // 卖出撤单额
        /// </summary>
        public double WithdrawSellMoney;

        /// <summary>
        /// // 买入撤单均价
        /// </summary>
        public float WithdrawBuyAvg;

        /// <summary>
        /// // 卖出撤单均价
        /// </summary>
        public float WithdrawSellAvg;	// 

        /// <summary>
        /// // 买入撤单每笔均额
        /// </summary>
        public double WithdrawBuyAvgVal;

        /// <summary>
        /// // 卖出撤单每笔均额
        /// </summary>
        public double WithdrawSellAvgVal;

        /// <summary>
        /// // 委托买入总笔数
        /// </summary>
        public int TotalBidNumber;

        /// <summary>
        /// // 委托卖出总笔数
        /// </summary>
        public int TotalOfferNumber;

        /// <summary>
        /// // 委托买入总档数
        /// </summary>
        public int NumBidOrders;

        /// <summary>
        /// // 委托卖出总档数
        /// </summary>
        public int NumOfferOrders;

        /// <summary>
        /// 买入成交最大等待时间
        /// </summary>
        public int BidTradeMaxDuration;

        /// <summary>
        /// // 卖出成交最大等待时间
        /// </summary>
        public int OfferTradeMaxDuration;
    }
    #endregion

    #region 31.分时资金流
    /// <summary>
    /// 一分钟的分时资金流
    /// </summary>
    public class OneMintTrendCaptialFlowDataRec
    {
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 买盘成交笔数, 0小单，1中单，2大单，3特大单
        /// </summary>
        public int[] BuyNum;

        /// <summary>
        /// 卖盘成交笔数, 0小单，1中单，2大单，3特大单
        /// </summary>
        public int[] SellNum;

        /// <summary>
        /// 买盘成交量,0小单，1中单，2大单，3特大单
        /// </summary>
        public int[] BuyVolume;

        /// <summary>
        /// 卖盘成交量, 0小单，1中单，2大单，3特大单
        /// </summary>
        public int[] SellVolume;

        /// <summary>
        /// 买盘成交额，0小单，1中单，2大单，3特大单
        /// </summary>
        public int[] BuyAmount;

        /// <summary>
        /// 卖盘成交额， 0小单，1中单，2大单，3特大单
        /// </summary>
        public int[] SellAmount;

        public OneMintTrendCaptialFlowDataRec()
        {
            SellAmount = new int[4];
            BuyAmount = new int[4];
            SellVolume = new int[4];
            BuyVolume = new int[4];
            SellNum = new int[4];
            BuyNum = new int[4];
        }

    }

    /// <summary>
    /// 分时资金流
    /// </summary>
    public class TrendCaptialFlowDataRec
    {
        /// <summary>
        /// 时间
        /// </summary>
        public UInt32 Time;

        /// <summary>
        /// 买盘成交笔数, 0小单，1中单，2大单，3特大单
        /// </summary>
        public UInt32[] BuyNum;
        /// <summary>
        /// 卖盘成交笔数, 0小单，1中单，2大单，3特大单
        /// </summary>
        public UInt32[] SellNum;

        /// <summary>
        /// 买盘成交量,0小单，1中单，2大单，3特大单
        /// </summary>
        public UInt64[] BuyVolume;
        /// <summary>
        /// 卖盘成交量, 0小单，1中单，2大单，3特大单
        /// </summary>
        public UInt64[] SellVolume;

        /// <summary>
        /// 买盘成交额，0小单，1中单，2大单，3特大单
        /// </summary>
        public double[] BuyAmount;
        /// <summary>
        /// 卖盘成交额， 0小单，1中单，2大单，3特大单
        /// </summary>
        public double[] SellAmount;
        /// <summary>
        /// 分时资金流单笔数据
        /// </summary>
        public TrendCaptialFlowDataRec()
        {
            SellAmount = new double[4];
            BuyAmount = new double[4];
            SellVolume = new UInt64[4];
            BuyVolume = new UInt64[4];
            SellNum = new UInt32[4];
            BuyNum = new UInt32[4];
        }
        /// <summary>
        /// 深Copy k线数据
        /// </summary>
        /// <returns></returns>
        public TrendCaptialFlowDataRec DeepCopy()
        {
            TrendCaptialFlowDataRec original = this;
            TrendCaptialFlowDataRec copy = new TrendCaptialFlowDataRec();   
            copy.Time = original.Time;
            original.SellAmount.CopyTo(copy.SellAmount, 0);
            original.BuyAmount.CopyTo(copy.BuyAmount, 0);

            original.SellVolume.CopyTo(copy.SellVolume, 0);
            original.BuyVolume.CopyTo(copy.BuyVolume, 0);

            original.SellNum.CopyTo(copy.SellNum, 0);
            original.BuyNum.CopyTo(copy.BuyNum, 0);
            return copy;
        }
    }

    /// <summary>
    /// 一只股票一天的分时资金流
    /// </summary>
    public class StockTrendCaptialFlowDataRec
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 数据
        /// </summary>
        public OneMintTrendCaptialFlowDataRec[] MintData;

        public StockTrendCaptialFlowDataRec(int code)
        {
            Code = code;
            short num = TimeUtilities.GetTrendTotalPoint(code);
            MintData = new OneMintTrendCaptialFlowDataRec[num];
            for (int i = 0; i < num; i++)
            {
                MintData[i] = new OneMintTrendCaptialFlowDataRec();
            }
        }
    }
    #endregion 

    #region 债券
    /// <summary>
    /// 银行间债券报价明细
    /// </summary>
    public class BidDetail
    {
        /// <summary>
        /// Id:唯一标识
        /// </summary>
        public long Id;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;		//时间	
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;		//日期
        ///// <summary>
        ///// 字符长度
        ///// </summary>
        //public short BidInitiatorLen;//字符长度
        /// <summary>
        /// 报价方
        /// </summary>
        public string BidInitiator;//报价方
        /// <summary>
        /// clean报价
        /// </summary>
        public float BidCleanPrice;//clean报价
        /// <summary>
        /// 收益率
        /// </summary>
        public float BidYield; //收益率
        /// <summary>
        /// 速度
        /// </summary>
        public int BidSettlementSpeed;//速度
        /// <summary>
        /// 面额
        /// </summary>
        public double BidTotalFaceValue;//面额
        /// <summary>
        /// 净价差额
        /// </summary>
        public float DeltaCleanPrice; //净价差额
        /// <summary>
        /// 收益率差
        /// </summary>
        public float DeltaYield; //收益率差
        ///// <summary>
        ///// 字符长度
        ///// </summary>
        //public short OfferInitiatorLen;//字符长度
        /// <summary>
        /// 报价方
        /// </summary>
        public string OfferInitiator;//报价方
        /// <summary>
        /// clean报价
        /// </summary>
        public float OfferCleanPrice;//clean报价
        /// <summary>
        /// 收益率
        /// </summary>
        public float OfferYield; //收益率
        /// <summary>
        /// 速度
        /// </summary>
        public int OfferSettlementSpeed;//速度
        /// <summary>
        /// 面额
        /// </summary>
        public double OfferTotalFaceValue;//面额
    }
    /// <summary>
    /// SHIBOR报价行明细
    /// </summary>
    public class OfferBankDetail
    {
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;		//时间	
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;		//日期
        /// <summary>
        /// 报价
        /// </summary>
        public float Price;		//报价
        ///// <summary>
        ///// 报价行字符长度
        ///// </summary>
        //public short Len;   //报价行字符长度
        /// <summary>
        /// 报价行
        /// </summary>
        public string OfferBank;//报价行

        /// <summary>
        /// 涨跌bp
        /// </summary>
        public float BP;
    }
    #endregion    

    #region 宏观指标

    /// <summary>
    /// 宏观指标数频率
    /// </summary>
    public enum DateFrequency:byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 日
        /// </summary>
        Day = 1,
        /// <summary>
        /// 周
        /// </summary>
        Week = 2,
        /// <summary>
        /// 旬
        /// </summary>
        TenDays = 3,
        /// <summary>
        /// 半月
        /// </summary>
        HalfMonth = 4,
        /// <summary>
        /// 月
        /// </summary>
        Month = 5,
        /// <summary>
        /// 季
        /// </summary>
        Season = 6,
        /// <summary>
        /// 半年
        /// </summary>
        HalfYear = 7,
        /// <summary>
        /// 年
        /// </summary>
        Year = 8,
        /// <summary>
        /// 不定期
        /// </summary>
        Irregularly = 9
    }
    public class StockIndicatorItem
    {
        ///// <summary>
        ///// 股票代码
        ///// </summary>
        //public string EMCode;
        /// <summary>
        /// 宏观指标Id
        /// </summary>
        public string MacroId;
        /// <summary>
        /// 宏观指标名称
        /// </summary>
        public string MacroName;
        /// <summary>
        /// 是否自定义指标
        /// </summary>
        public bool IsCustomize;
        /// <summary>
        /// 报告期
        /// </summary>
        public DateTime ReportDate;
        /// <summary>
        /// 本期
        /// </summary>
        public float Current;
        /// <summary>
        /// 频率
        /// </summary>
        public DateFrequency Frequency;
    }

    /// <summary>
    /// 个股面板-宏观报表左侧条目
    /// </summary>
    public class StockIndicatorLeftItem : StockIndicatorItem
    {
        /// <summary>
        /// 5日
        /// </summary>
        public float DifferRangeDay5;
        /// <summary>
        /// 20日
        /// </summary>
        public float DifferRangeDay20;
        /// <summary>
        /// 60日
        /// </summary>
        public float DifferRangeDay60;
        /// <summary>
        /// 年初至今
        /// </summary>
        public float DifferRangeYTD;

    }
    /// <summary>
    /// 个股面板-宏观报表右侧条目
    /// </summary>
    public class StockIndicatorRightItem : StockIndicatorItem
    {
        /// <summary>
        /// 上期
        /// </summary>
        public float Previous;

        /// <summary>
        /// 环比
        /// </summary>
        public float Central;
        /// <summary>
        /// 同比
        /// </summary>
        public float Compare;
    }
    #endregion
}
