using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using EmSerDSComm;

namespace EmQComm
{
    #region 类型 
    /// <summary>
    /// 客户端用的市场类型(按交易所分)
    /// </summary>
    public enum MarketType
    {
                NA,
        SHALev1,
        SHALev2,
        SZALev1,
        SZALev2,
        SHBLev1,
        SHBLev2,
        SZBLev1,
        SZBLev2,
        SHINDEX,
        SZINDEX,
        EMINDEX,
        CircuitBreakerIndex,
        CSINDEX,
        CSIINDEX,
        GLOBAL,
        CNINDEX,
        HSINDEX,
        JKINDEX,
        MalaysiaIndex,
        KoreaIndex,
        NikkeiIndex,
        PhilippinesIndex,
        SensexIndex,
        SingaporeIndex,
        TaiwanIndex,
        NewZealandIndex,
        NasdaqIndex,
        DutchAEXIndex,
        AustriaIndex,
        NorwayIndex,
        RussiaIndex,
        AXAT,
        CRB,
        HSIA,
        FTSEA50,
        SHHKCreditIndex,
        HKCreditIndex,
        IF,
        SHF,
        CHFAG,
        CHFCU,
        SHFRU,
        CZC,
        CZCSR,
        CZCFe,
        DZDCE,
        DCE,
        OSFutures,
        OSFuturesCBOT,
        OSFuturesSGX,
        GoverFutures,
        OSFuturesLMEElec,
        OSFuturesLMEVenue,
        BCE,
        FME,
        IB,
        SHConvertBondLev1,
        SHConvertBondLev2,
        SZConvertBondLev1,
        SZConvertBondLev2,
        SHNonConvertBondLev1,
        SHNonConvertBondLev2,
        SZNonConvertBondLev1,
        SZNonConvertBondLev2,
        BC,
        MonetaryFund,
        NonMonetaryFund,
        SHFundLev1,
        SHFundLev2,
        SZFundLev1,
        SZFundLev2,
        InterBankRepurchase,
        SHRepurchaseLevel1,
        SHRepurchaseLevel2,
        SZRepurchaseLevel1,
        SZRepurchaseLevel2,
        Chibor,
        InterestRate,
        RateSwap,
        CIP,
        CIPMonetary,
        ISP,
        BFP,
        TRPm,
        TRPSun,
        ForexSpot,
        ForexNonSpot,
        ForexInter,
        ForexInterWinterSummer,
        StockOption,
        StockOptionSell,
        FuturesOption,
        FuturesOptionSell,
        HKwarrentBuy,
        HKwarrentSell,
        CallableContractsBuy,
        CallableContractsSell,
        CommodityOptionBuy,
        CommodityOptionSell,
        Portfolios,
        PortfoliosAccount,
        TB_OLD,
        TB_NEW,
        HK,
        SHHK,
        GT,
        OS,
        US,
        SHSZINDEXLev2,
        HKOptionLev2,
        HKLev2,
        HSIforMTime
        ///// <summary>
        ///// 未知
        ///// </summary>
        //NA, 

        //#region 股票
        ///// <summary>
        ///// 上海A股Level1
        ///// </summary>
        //SHALev1,
        ///// <summary>
        ///// 上海A股Level2
        ///// </summary>
        //SHALev2,
        ///// <summary>
        ///// 深证A股Level1
        ///// </summary>
        //SZALev1,
        ///// <summary>
        ///// 深证A股Level2
        ///// </summary>
        //SZALev2,
        ///// <summary>
        ///// 上海B股Level1
        ///// </summary>
        //SHBLev1,
        ///// <summary>
        ///// 上海B股Level2
        ///// </summary>
        //SHBLev2,
        ///// <summary>
        ///// 深证B股Level1
        ///// </summary>
        //SZBLev1,
        ///// <summary>
        ///// 深证B股Level2
        ///// </summary>
        //SZBLev2,
        //#endregion

        //#region 指数
        ///// <summary>
        ///// 沪市指数
        ///// </summary>
        //SHINDEX,
        ///// <summary>
        ///// 深市指数
        ///// </summary>
        //SZINDEX,
        ///// <summary>
        ///// 东财指数
        ///// </summary>
        //EMINDEX,
        ///// <summary>
        ///// 中债指数
        ///// </summary>
        //CSINDEX,
        ///// <summary>
        ///// 中证指数
        ///// </summary>
        //CSIINDEX,
        ///// <summary>
        ///// 全球指数
        ///// </summary>
        //GLOBAL,
        ///// <summary>
        ///// 巨潮指数
        ///// </summary>
        //CNINDEX,
        ///// <summary>
        ///// 恒生指数
        ///// </summary>
        //HSINDEX,
        ///// <summary>
        ///// 印尼综合指数
        ///// </summary>
        //JKINDEX,
        ///// <summary>
        ///// 马来西亚指数
        ///// </summary>
        //MalaysiaIndex,
        ///// <summary>
        ///// 韩国指数
        ///// </summary>
        //KoreaIndex,
        ///// <summary>
        ///// 日经225指数
        ///// </summary>
        //NikkeiIndex,
        ///// <summary>
        ///// 菲律宾指数
        ///// </summary>
        //PhilippinesIndex,
        ///// <summary>
        ///// 印度孟买指数
        ///// </summary>
        //SensexIndex,
        ///// <summary>
        ///// 新加坡指数
        ///// </summary>
        //SingaporeIndex,
        ///// <summary>
        ///// 台湾加权指数
        ///// </summary>
        //TaiwanIndex,
        ///// <summary>
        ///// 新西兰指数
        ///// </summary>
        //NewZealandIndex,
        ///// <summary>
        ///// 纳斯达克/标普/道琼斯/墨西哥指数
        ///// </summary>
        //NasdaqIndex,
        ///// <summary>
        ///// 荷兰AEX指数
        ///// </summary>
        //DutchAEXIndex,
        ///// <summary>
        ///// 奥地利指数
        ///// </summary>
        //AustriaIndex,
        ///// <summary>
        ///// 挪威指数
        ///// </summary>
        //NorwayIndex,
        ///// <summary>
        ///// 俄罗斯指数
        ///// </summary>
        //RussiaIndex,
        //#endregion

        //#region 期货
        ///// <summary>
        ///// 中金所(股指期货)
        ///// </summary>
        //IF, 
        ///// <summary>
        ///// 上期所
        ///// </summary>
        //SHF,
        ///// <summary>
        ///// 上期所金银
        ///// </summary>
        //CHFAG,
        ///// <summary>
        ///// 上期所铜铝铅锌
        ///// </summary>
        //CHFCU,
        ///// <summary>
        ///// 郑商所
        ///// </summary>
        //CZC,
        ///// <summary>
        ///// 大商所
        ///// </summary>
        //DCE,
        ///// <summary>
        ///// 外盘期货
        ///// </summary>
        //OSFutures,
        ///// <summary>
        ///// 外盘期货CBOT
        ///// </summary>
        //OSFuturesCBOT,
        ///// <summary>
        ///// 外盘期货SGX
        ///// </summary>
        //OSFuturesSGX,
        ///// <summary>
        ///// 国债期货
        ///// </summary>
        //GoverFutures,
        ///// <summary>
        ///// LME电子盘，综合
        ///// </summary> 
        //OSFuturesLMEElec,
        ///// <summary>
        ///// LME场内
        ///// </summary>
        //OSFuturesLMEVenue,

        //#endregion

        //#region 现货
        ///// <summary>
        ///// 渤商所
        ///// </summary>
        //BCE,
        ///// <summary>
        ///// 泛亚有色金属
        ///// </summary>
        //FME,
        //#endregion

        //#region 债券
        ///// <summary>
        ///// 外汇交易中心（银行间债券）
        ///// </summary>
        //IB, 
        ///// <summary>
        ///// 上交所可转债Lev1
        ///// </summary>
        //SHConvertBondLev1,
        ///// <summary>
        ///// 上交所可转债Lev2
        ///// </summary>
        //SHConvertBondLev2,
        ///// <summary>
        ///// 深交所可转债Lev1
        ///// </summary>
        //SZConvertBondLev1,
        ///// <summary>
        ///// 深交所可转债Lev2
        ///// </summary>
        //SZConvertBondLev2,
        ///// <summary>
        ///// 上交所非可转债Lev1
        ///// </summary>
        //SHNonConvertBondLev1,
        ///// <summary>
        ///// 上交所非可转债Lev2
        ///// </summary>
        //SHNonConvertBondLev2,
        ///// <summary>
        ///// 深交所非可转债Lev1
        ///// </summary>
        //SZNonConvertBondLev1,
        ///// <summary>
        ///// 深交所非可转债Lev2
        ///// </summary>
        //SZNonConvertBondLev2,
        ///// <summary>
        ///// 柜台交易债
        ///// </summary>
        //BC,
        //#endregion

        //#region 基金
        ///// <summary>
        ///// 货币式式基金
        ///// </summary>
        //MonetaryFund,
        ///// <summary>
        ///// 非货币式式基金
        ///// </summary>
        //NonMonetaryFund, 
        ///// <summary>
        ///// 上交所基金
        ///// </summary>
        //SHFundLev1,
        ///// <summary>
        ///// 上交所基金
        ///// </summary>
        //SHFundLev2,
        ///// <summary>
        ///// 深交所基金
        ///// </summary>
        //SZFundLev1,
        ///// <summary>
        ///// 深交所基金
        ///// </summary>
        //SZFundLev2,
        //#endregion

        //#region 利率
        ///// <summary>
        ///// 银行间回购
        ///// </summary>
        //InterBankRepurchase,

        ///// <summary>
        ///// 上交所回购Level1
        ///// </summary>
        //SHRepurchaseLevel1,

        ///// <summary>
        ///// 上交所回购Levle2
        ///// </summary>
        //SHRepurchaseLevel2,

        ///// <summary>
        ///// 深交所回购Level1
        ///// </summary>
        //SZRepurchaseLevel1,

        ///// <summary>
        ///// 深交所回购Level2
        ///// </summary>
        //SZRepurchaseLevel2,

        ///// <summary>
        ///// chibor利率
        ///// </summary>
        //Chibor,

        ///// <summary>
        ///// SHIBOR、HIBOR、LIBOR、基准利率
        ///// </summary>
        //InterestRate,

        ///// <summary>
        ///// 利率互换
        ///// </summary>
        //RateSwap,
        //#endregion

        //#region 理财
        ///// <summary>
        ///// 券商集合理财(非货币式)
        ///// </summary>
        //CIP,

        ///// <summary>
        ///// 券商集合理财(货币式)
        ///// </summary>
        //CIPMonetary,

        ///// <summary>
        ///// 保险理财
        ///// </summary>
        //ISP,

        ///// <summary>
        ///// 银行理财
        ///// </summary>
        //BFP,

        ///// <summary>
        ///// 信托理财
        ///// </summary>
        //TRPm,
        ///// <summary>
        ///// 阳光私募
        ///// </summary>
        //TRPSun,
        //#endregion

        //#region 外汇
        ///// <summary>
        ///// 外汇(即期)
        ///// </summary>
        //ForexSpot,
        ///// <summary>
        ///// 外汇(非即期)
        ///// </summary>
        //ForexNonSpot,
        ///// <summary>
        ///// 国际外汇
        ///// </summary>
        //ForexInter,
        //#endregion

        //#region 期权
        ///// <summary>
        ///// 个股期权
        ///// </summary>
        //StockOption,

        ///// <summary>
        ///// 股指期权
        ///// </summary>
        //FuturesOption,
        //#endregion 

        //#region 其他
        ///// <summary>
        ///// 老三板
        ///// </summary>
        //TB_OLD,
        ///// <summary>
        ///// 新三板
        ///// </summary>
        //TB_NEW,
        ///// <summary>
        ///// 香港
        ///// </summary>
        //HK, 
        ///// <summary>
        ///// 上海黄金交易所（黄金）
        ///// </summary>
        //GT, 
        ///// <summary>
        ///// 海外
        ///// </summary>
        //OS,
        ///// <summary>
        ///// 美股
        ///// </summary>
        //US,

        
        //#endregion
    };

    /// <summary>
    /// 客户端用的证券类型（A股，B股……）
    /// </summary>
    public enum SecurityType
    {
        /// <summary>
        /// 无
        /// </summary>
        NA,
        /// <summary>
        /// 指数
        /// </summary>
        Index, 
        /// <summary>
        /// A股
        /// </summary>
        EQTY_A, 
        /// <summary>
        /// B股
        /// </summary>
        EQTY_B, 
        /// <summary>
        /// 
        /// </summary>
        BOND, 
        /// <summary>
        /// 银行间债券
        /// </summary>
        InterBankBond, 
        /// <summary>
        /// 权证
        /// </summary>
        WRNT, 
        /// <summary>
        /// 封闭式基金
        /// </summary>
        FUND_CLOSE, 
        /// <summary>
        /// 开放式基金
        /// </summary>
        FUND_OPEN, 
        /// <summary>
        /// EFT
        /// </summary>
        FUND_ETF, 
        /// <summary>
        /// LOF
        /// </summary>
        FUND_LOF, 
        /// <summary>
        /// 回购
        /// </summary>
        Repurchase, 
        /// <summary>
        /// 股指期货
        /// </summary>
        INDEX_FUTURE, 
        /// <summary>
        /// 港股
        /// </summary>
        EQTY_HK, 
        /// <summary>
        /// 黄金
        /// </summary>
        Gold, 
        /// <summary>
        /// 台股
        /// </summary>
        EQTY_TB, 

    };

    /// <summary>
    /// 服务端证券类别码
    /// </summary>
    public enum ReqSecurityType
    {
        /// <summary>
        /// 上海指数
        /// </summary>
        ReqShZs = 1,	
        /// <summary>
        /// 上海Ａ股
        /// </summary>
        ReqShAg = 2,	
        /// <summary>
        /// 上海Ｂ股
        /// </summary>
        ReqShBg = 3,	
        /// <summary>
        /// 上海债券
        /// </summary>
        ReqShZq = 4,	 
        /// <summary>
        /// 深圳指数
        /// </summary>
        ReqSzZs = 5,	
        /// <summary>
        /// 深圳Ａ股
        /// </summary>
        ReqSzAg = 6,	 
        /// <summary>
        /// 深圳Ｂ股
        /// </summary>
        ReqSzBg = 7,	
        /// <summary>
        /// 深圳债券
        /// </summary>
        ReqSzZq = 8,	
        /// <summary>
        /// 上海基金
        /// </summary>
        ReqShJj = 9,
        /// <summary>
        /// 深圳基金
        /// </summary>
        ReqSzJj = 10,
        /// <summary>
        /// 上海权证
        /// </summary>
        ReqShQz = 11,
        /// <summary>
        ///  深圳权证
        /// </summary>
        ReqSzQz = 12,
        /// <summary>
        /// 深圳中小企业板
        /// </summary>
        ReqSzSm = 13,	
        /// <summary>
        /// 开放式基金
        /// </summary>
        ReqOFnd = 14,	
        /// <summary>
        /// 上证系列指数(IMPORTANT INDEX)(999)
        /// </summary>
        ReqSHZSIM9 = 15,	
        /// <summary>
        /// 中证系列指数(IMPORTANT INDEX)(666)
        /// </summary>
        ReqSHZSIM6 = 16,	
        /// <summary>
        /// 深证所有A股， 包括中小板和创业板
        /// </summary>
        ReqSZALLAG = 17,	
        /// <summary>
        /// 创业板
        /// </summary>
        ReqSZCY = 80,	
        /// <summary>
        /// 三板
        /// </summary>
        ReqSBSC = 81,
        /// <summary>
        /// 上证和深证所有A股
        /// </summary>
        ReqALLAG = 40,
        /// <summary>
        /// 上证和深证所有权证
        /// </summary>
        ReqALLQZ = 41,	
        /// <summary>
        ///  Forex
        /// </summary>
        ReqFOREX = 32,	
        /// <summary>
        ///  国外期货
        /// </summary>
        ReqGWQH = 33,	
        /// <summary>
        /// 上证和深证所有债券
        /// </summary>
        ReqALLZQ = 42,	
        /// <summary>
        /// A+H
        /// </summary>
        ReqAH = 20,	    
        /// <summary>
        /// HK
        /// </summary>
        ReqHK = 101,	
        /// <summary>
        /// QH
        /// </summary>
        ReqQH = 31, 	
        /// <summary>
        /// 股指期货
        /// </summary>
        ReqGZQH = 168,	
    }

    /// <summary>
    /// 板块树各节点种类值
    /// 注意：添加或修改品种，请呼叫【赵亮】
    /// </summary>
    public enum BlockTreeCategory
    {
        /// <summary>
        /// 沪深股票
        /// </summary>
        SHSZ = 11,
        /// <summary>
        /// B股
        /// </summary>
        BStock = 12,
        /// <summary>
        /// 美股
        /// </summary>
        USA = 21,
        /// <summary>
        /// 投资理财
        /// </summary>
        FinancialManager = 31,
        /// <summary>
        /// 港股
        /// </summary>
        HK = 41,
        /// <summary>
        /// 开放式基金
        /// </summary>
        OpenFund = 51,
        /// <summary>
        /// 封闭式基金
        /// </summary>
        CloseFund = 52,
        /// <summary>
        /// 债券
        /// </summary>
        Bond = 61,
        /// <summary>
        /// 商品期货
        /// </summary>
        Futures = 71,
        /// <summary>
        /// 股指期货
        /// </summary>
        IndexFutures = 72,
        /// <summary>
        /// 海外期货
        /// </summary>
        OverSeaFutures = 73,
        /// <summary>
        /// LME期货
        /// </summary>
        OSFuturesLME = 74,
        /// <summary>
        /// 混合品种商品期货
        /// </summary>
        ChaosFutures = 75,
        /// <summary>
        /// 利率
        /// </summary>
        InterestRate = 81,
        /// <summary>
        /// 指数
        /// </summary>
        Index = 91,
        /// <summary>
        /// 全球指数
        /// </summary>
        Global = 92,
        /// <summary>
        /// 东财指数
        /// </summary>
        EmIndex = 93,
        /// <summary>
        /// 外汇交易中心债券指数
        /// </summary>
        ForeignIndex = 94,
        /// <summary>
        /// 行业应用
        /// </summary>
        Industry = 101,
        /// <summary>
        /// 外汇
        /// </summary>
        Exchange = 111,
    }

    /// <summary>
    /// 买卖档宽度类型
    /// </summary>
    public enum InfoPanelWidthType
    {
        /// <summary>
        /// 双栏
        /// </summary>
        Double,
        /// <summary>
        /// 单栏
        /// </summary>
        CommonSingle,
        /// <summary>
        /// 股指期货单栏
        /// </summary>
        IFSingle,
        /// <summary>
        /// 开放式基金
        /// </summary>
        OpenFund,
        /// <summary>
        /// 债券
        /// </summary>
        IB
    }



    #endregion
    /// <summary>
    /// 
    /// </summary>
    public class SecurityAttribute
    {
        public static bool EqualZero(double value)
        {
            return ((value > -4.94065645841247E-324) && (value < double.Epsilon));
        }

        public static bool EqualZero(float value)
        {
            return ((value > -1.401298E-45f) && (value < float.Epsilon));
        }

        public static void SetVolumeRatio(int code, long volumeAveDay5, float volumeRatio, long volume, int time)
        {
            float num;
            if (EqualZero((float)volumeAveDay5))
            {
                volumeAveDay5 = DetailData.FieldIndexDataInt64[code][FieldIndex.VolumeAvgDay5];
            }
            if ((time >= 0x16954) && (time < 0x16bac))
            {
                num = ((float)volumeAveDay5) / 241f;
            }
            else
            {
                num = ((float)(volumeAveDay5 * TimeUtilities.GetPointFromTime(code))) / 241f;
            }
            if (!EqualZero(num))
            {
                volumeRatio = ((float)volume) / num;
            }
            DetailData.FieldIndexDataSingle[code][FieldIndex.VolumeRatio] = volumeRatio;
        }

        public static float CalcBondYtm(float price, int code, bool isConvertBond)
        {
            if (!EqualZero(price))
            {
                Dictionary<float, float> dictionary;
                string fieldDataString = DetailData.FieldIndexDataString[code][FieldIndex.EMCode];
                if (DetailData.AllBondYtmDataRec.TryGetValue(code, out dictionary))
                {
                    if (dictionary.ContainsKey(price))
                    {
                        return dictionary[price];
                    }
                    string cmd = string.Empty;
                    if (isConvertBond)
                    {
                        cmd = string.Format("rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=0 columns=netPrice,ytm", fieldDataString, price);
                    }
                    else
                    {
                        cmd = string.Format("rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=1 columns=netPrice,ytm", fieldDataString, price);
                    }
                    try
                    {
                        DataTable table = Requestor.GetDataTable(cmd, null, false);
                        if (table == null)
                        {
                            return float.NaN;
                        }
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            float f = Convert.ToSingle(table.Rows[0]["ytm"]);
                            if (!float.IsInfinity(f))
                            {
                                dictionary[price] = f;
                                return f;
                            }
                        }
                        table.Dispose();
                    }
                    catch (Exception exception)
                    {
                        LogUtilities.LogMessage("行情调用债券计算服务，计算ytm时异常" + exception.Message);
                    }
                }
                else
                {
                    string str3 = string.Empty;
                    if (isConvertBond)
                    {
                        str3 = string.Format("rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=0 columns=netPrice,ytm", fieldDataString, price);
                    }
                    else
                    {
                        str3 = string.Format("rpt name=InstantCalc emCodes={0} prices={1} isNetPrice=1 columns=netPrice,ytm", fieldDataString, price);
                    }
                    try
                    {
                        DataTable table2 = Requestor.GetDataTable(str3, null, false);
                        if (table2 == null)
                        {
                            return float.NaN;
                        }
                        for (int j = 0; j < table2.Rows.Count; j++)
                        {
                            float num4 = Convert.ToSingle(table2.Rows[0]["ytm"]);
                            if (!float.IsInfinity(num4))
                            {
                                Dictionary<float, float> dictionary2 = new Dictionary<float, float>(1);
                                dictionary2[price] = num4;
                                DetailData.AllBondYtmDataRec[code] = dictionary2;
                                return num4;
                            }
                        }
                        table2.Dispose();
                    }
                    catch (Exception exception2)
                    {
                        LogUtilities.LogMessage("行情调用债券计算服务，计算ytm时异常" + exception2.Message);
                    }
                }
            }
            return float.NaN;
        }

        static SecurityAttribute(){
            DicGlobalIndexCftToOrg.Add("DJIA", "DJIA.GI");
            DicGlobalIndexCftToOrg.Add("NDX", "IXIC.GI");
            DicGlobalIndexCftToOrg.Add("SP5I", "SPX.GI");
            DicGlobalIndexCftToOrg.Add("HSI", "HSI.HI");
            DicGlobalIndexCftToOrg.Add("TWII", "TWII.TW");
            DicGlobalIndexCftToOrg.Add("N225", "N225.GI");
            DicGlobalIndexCftToOrg.Add("NHI", "KS11.GI");
            DicGlobalIndexCftToOrg.Add("STI", "STI.GI");
            DicGlobalIndexCftToOrg.Add("YNI", "JKSE.GI");
            DicGlobalIndexCftToOrg.Add("MGI", "KLSE.GI");
            DicGlobalIndexCftToOrg.Add("FCHI", "FCHI.GI");
            DicGlobalIndexCftToOrg.Add("FTSE", "FTSE.GI");
            DicGlobalIndexCftToOrg.Add("GDAXI", "GDAXI.GI");
            DicGlobalIndexCftToOrg.Add("AEX", "AEX.GI");
            DicGlobalIndexCftToOrg.Add("SSMI", "SSMI.GI");
            DicGlobalIndexCftToOrg.Add("BELI", "BFX.GI");

            DicGlobalIndexOrgToCft.Add("DJIA","DJIA");
            DicGlobalIndexOrgToCft.Add("IXIC","NDX");
            DicGlobalIndexOrgToCft.Add("SPX","SP5I");
            DicGlobalIndexOrgToCft.Add("HSI", "HSI");
            DicGlobalIndexOrgToCft.Add("TWII", "TWII");
            DicGlobalIndexOrgToCft.Add("N225", "N225");
            DicGlobalIndexOrgToCft.Add("KS11","NHI");
            DicGlobalIndexOrgToCft.Add("STI", "STI");
            DicGlobalIndexOrgToCft.Add("JKSE","YNI");
            DicGlobalIndexOrgToCft.Add("KLSE","MGI");
            DicGlobalIndexOrgToCft.Add("FCHI", "FCHI");
            DicGlobalIndexOrgToCft.Add("FTSE", "FTSE");
            DicGlobalIndexOrgToCft.Add("GDAXI", "GDAXI");
            DicGlobalIndexOrgToCft.Add("AEX", "AEX");
            DicGlobalIndexOrgToCft.Add("SSMI", "SSMI");
            DicGlobalIndexOrgToCft.Add("BFX","BELI");

            TrendStyleByMarket.Add(MarketType.NA, 1);
            TrendStyleByMarket.Add(MarketType.SHALev1, 1);
            TrendStyleByMarket.Add(MarketType.SHALev2, 1);
            TrendStyleByMarket.Add(MarketType.SZALev1, 1);
            TrendStyleByMarket.Add(MarketType.SZALev2, 1);
            TrendStyleByMarket.Add(MarketType.SHBLev1, 1);
            TrendStyleByMarket.Add(MarketType.SHBLev2, 1);
            TrendStyleByMarket.Add(MarketType.SZBLev1, 1);
            TrendStyleByMarket.Add(MarketType.SZBLev2, 1);
            TrendStyleByMarket.Add(MarketType.IF, 1);
            TrendStyleByMarket.Add(MarketType.GoverFutures, 1);
            TrendStyleByMarket.Add(MarketType.MonetaryFund, 1);
            TrendStyleByMarket.Add(MarketType.NonMonetaryFund, 1);
            TrendStyleByMarket.Add(MarketType.SHFundLev1, 1);
            TrendStyleByMarket.Add(MarketType.SHFundLev2, 1);
            TrendStyleByMarket.Add(MarketType.SZFundLev1, 1);
            TrendStyleByMarket.Add(MarketType.SZFundLev2, 1);
            TrendStyleByMarket.Add(MarketType.TB_OLD, 1);
            TrendStyleByMarket.Add(MarketType.TB_NEW, 1);
            TrendStyleByMarket.Add(MarketType.CNINDEX, 2);
            TrendStyleByMarket.Add(MarketType.SHINDEX, 2);
            TrendStyleByMarket.Add(MarketType.SZINDEX, 2);
            TrendStyleByMarket.Add(MarketType.EMINDEX, 2);
            TrendStyleByMarket.Add(MarketType.CSINDEX, 2);
            TrendStyleByMarket.Add(MarketType.CSIINDEX, 2);
            TrendStyleByMarket.Add(MarketType.SHF, 2);
            TrendStyleByMarket.Add(MarketType.CZC, 2);
            TrendStyleByMarket.Add(MarketType.DCE, 2);
            TrendStyleByMarket.Add(MarketType.GT, 2);
            TrendStyleByMarket.Add(MarketType.OS, 2);
            TrendStyleByMarket.Add(MarketType.IB, 2);
            TrendStyleByMarket.Add(MarketType.SHConvertBondLev1, 2);
            TrendStyleByMarket.Add(MarketType.SHConvertBondLev2, 2);
            TrendStyleByMarket.Add(MarketType.SZConvertBondLev1, 2);
            TrendStyleByMarket.Add(MarketType.SZConvertBondLev2, 2);
            TrendStyleByMarket.Add(MarketType.SHNonConvertBondLev1, 2);
            TrendStyleByMarket.Add(MarketType.SHNonConvertBondLev2, 2);
            TrendStyleByMarket.Add(MarketType.SZNonConvertBondLev1, 2);
            TrendStyleByMarket.Add(MarketType.SZNonConvertBondLev2, 2);
            TrendStyleByMarket.Add(MarketType.BC, 2);
            TrendStyleByMarket.Add(MarketType.InterBankRepurchase, 2);
            TrendStyleByMarket.Add(MarketType.SHRepurchaseLevel1, 2);
            TrendStyleByMarket.Add(MarketType.SHRepurchaseLevel2, 2);
            TrendStyleByMarket.Add(MarketType.SZRepurchaseLevel1, 2);
            TrendStyleByMarket.Add(MarketType.SZRepurchaseLevel2, 2);
            TrendStyleByMarket.Add(MarketType.Chibor, 2);
            TrendStyleByMarket.Add(MarketType.InterestRate, 2);
            TrendStyleByMarket.Add(MarketType.RateSwap, 2);
            TrendStyleByMarket.Add(MarketType.ISP, 2);
            TrendStyleByMarket.Add(MarketType.CIP, 2);
            TrendStyleByMarket.Add(MarketType.CIPMonetary, 2);
            TrendStyleByMarket.Add(MarketType.BFP, 2);
            TrendStyleByMarket.Add(MarketType.TRPm, 2);
            TrendStyleByMarket.Add(MarketType.HSINDEX, 2);
            TrendStyleByMarket.Add(MarketType.JKINDEX, 2);
            TrendStyleByMarket.Add(MarketType.MalaysiaIndex, 2);
            TrendStyleByMarket.Add(MarketType.KoreaIndex, 2);
            TrendStyleByMarket.Add(MarketType.NikkeiIndex, 2);
            TrendStyleByMarket.Add(MarketType.PhilippinesIndex, 2);
            TrendStyleByMarket.Add(MarketType.SensexIndex, 2);
            TrendStyleByMarket.Add(MarketType.SingaporeIndex, 2);
            TrendStyleByMarket.Add(MarketType.TaiwanIndex, 2);
            TrendStyleByMarket.Add(MarketType.NewZealandIndex, 2);
            TrendStyleByMarket.Add(MarketType.NasdaqIndex, 2);
            TrendStyleByMarket.Add(MarketType.DutchAEXIndex, 2);
            TrendStyleByMarket.Add(MarketType.AustriaIndex, 2);
            TrendStyleByMarket.Add(MarketType.NorwayIndex, 2);
            TrendStyleByMarket.Add(MarketType.RussiaIndex, 2);
            TrendStyleByMarket.Add(MarketType.HK, 3);
            TrendStyleByMarket.Add(MarketType.US, 3);
            TrendStyleByMarket.Add(MarketType.ForexSpot, 3);
            TrendStyleByMarket.Add(MarketType.ForexNonSpot, 3);
            TrendStyleByMarket.Add(MarketType.OSFutures, 3);
            TrendStyleByMarket.Add(MarketType.OSFuturesCBOT, 3);
            TrendStyleByMarket.Add(MarketType.OSFuturesSGX, 3);
            TrendStyleByMarket.Add(MarketType.OSFuturesLMEElec,3);
            TrendStyleByMarket.Add(MarketType.OSFuturesLMEVenue, 3);

            SameMarketValue.Add(MarketType.NA, 0);
            SameMarketValue.Add(MarketType.SHALev1, 1);
            SameMarketValue.Add(MarketType.SHALev2, 2);
            SameMarketValue.Add(MarketType.SZALev1, 1);
            SameMarketValue.Add(MarketType.SZALev2, 3);
            SameMarketValue.Add(MarketType.SHBLev1, 1);
            SameMarketValue.Add(MarketType.SHBLev2, 2);
            SameMarketValue.Add(MarketType.SZBLev1, 1);
            SameMarketValue.Add(MarketType.SZBLev2, 3);
            SameMarketValue.Add(MarketType.SHINDEX, 4);
            SameMarketValue.Add(MarketType.SZINDEX, 4);
            SameMarketValue.Add(MarketType.EMINDEX, 4);
            SameMarketValue.Add(MarketType.CSINDEX, 4);
            SameMarketValue.Add(MarketType.CSIINDEX, 4);
            SameMarketValue.Add(MarketType.GLOBAL, 4);
            SameMarketValue.Add(MarketType.CNINDEX, 4);
            SameMarketValue.Add(MarketType.HSINDEX, 4);
            SameMarketValue.Add(MarketType.JKINDEX, 4);
            SameMarketValue.Add(MarketType.MalaysiaIndex, 4);
            SameMarketValue.Add(MarketType.NikkeiIndex, 4);
            SameMarketValue.Add(MarketType.PhilippinesIndex, 4);
            SameMarketValue.Add(MarketType.SensexIndex, 4);
            SameMarketValue.Add(MarketType.SingaporeIndex, 4);
            SameMarketValue.Add(MarketType.TaiwanIndex, 4);
            SameMarketValue.Add(MarketType.NewZealandIndex, 4);
            SameMarketValue.Add(MarketType.NasdaqIndex, 4);
            SameMarketValue.Add(MarketType.DutchAEXIndex, 4);
            SameMarketValue.Add(MarketType.AustriaIndex, 4);
            SameMarketValue.Add(MarketType.NorwayIndex, 4);
            SameMarketValue.Add(MarketType.RussiaIndex, 4);
            SameMarketValue.Add(MarketType.IF, 5);
            SameMarketValue.Add(MarketType.GoverFutures, 5); 
            SameMarketValue.Add(MarketType.SHF, 6);
            SameMarketValue.Add(MarketType.CZC, 6);
            SameMarketValue.Add(MarketType.DCE, 6);
            SameMarketValue.Add(MarketType.OSFutures, 6);
            SameMarketValue.Add(MarketType.OSFuturesCBOT, 6);
            SameMarketValue.Add(MarketType.OSFuturesSGX, 6);
            SameMarketValue.Add(MarketType.OSFuturesLMEElec, 6);
            SameMarketValue.Add(MarketType.OSFuturesLMEVenue, 6);
            SameMarketValue.Add(MarketType.IB, 7);
            SameMarketValue.Add(MarketType.SHConvertBondLev1, 14);
            SameMarketValue.Add(MarketType.SHConvertBondLev2, 15);
            SameMarketValue.Add(MarketType.SZConvertBondLev1, 14);
            SameMarketValue.Add(MarketType.SZConvertBondLev2, 16);
            SameMarketValue.Add(MarketType.SHNonConvertBondLev1, 17);
            SameMarketValue.Add(MarketType.SHNonConvertBondLev2, 18);
            SameMarketValue.Add(MarketType.SZNonConvertBondLev1, 17);
            SameMarketValue.Add(MarketType.SZNonConvertBondLev2, 19);
            SameMarketValue.Add(MarketType.BC, 7);
            SameMarketValue.Add(MarketType.InterBankRepurchase, 13);
            SameMarketValue.Add(MarketType.SHRepurchaseLevel1, 20);
            SameMarketValue.Add(MarketType.SHRepurchaseLevel2, 21);
            SameMarketValue.Add(MarketType.SZRepurchaseLevel1, 20);
            SameMarketValue.Add(MarketType.SZRepurchaseLevel2, 22);
            SameMarketValue.Add(MarketType.Chibor, 23);
            SameMarketValue.Add(MarketType.InterestRate, 24);
            SameMarketValue.Add(MarketType.RateSwap, 24);
            SameMarketValue.Add(MarketType.MonetaryFund, 8);
            SameMarketValue.Add(MarketType.NonMonetaryFund, 9);
            SameMarketValue.Add(MarketType.SHFundLev1, 10);
            SameMarketValue.Add(MarketType.SHFundLev2, 11);
            SameMarketValue.Add(MarketType.SZFundLev1, 10);
            SameMarketValue.Add(MarketType.SZFundLev2, 12);
            SameMarketValue.Add(MarketType.ISP, 8);
            SameMarketValue.Add(MarketType.CIP, 8);
            SameMarketValue.Add(MarketType.CIPMonetary, 8);
            SameMarketValue.Add(MarketType.BFP, 8);
            SameMarketValue.Add(MarketType.TRPm, 8);
            SameMarketValue.Add(MarketType.TRPSun, 8);
            SameMarketValue.Add(MarketType.TB_OLD, 25);
            SameMarketValue.Add(MarketType.TB_NEW, 26);
            SameMarketValue.Add(MarketType.HK, 27);
            SameMarketValue.Add(MarketType.GT, 28);
            SameMarketValue.Add(MarketType.OS, 29);
            SameMarketValue.Add(MarketType.US, 30);
            SameMarketValue.Add(MarketType.ForexSpot, 31);
            SameMarketValue.Add(MarketType.ForexNonSpot, 32);
            SameMarketValue.Add(MarketType.CHFAG, 33);
            SameMarketValue.Add(MarketType.CHFCU, 34);
            SameMarketValue.Add(MarketType.BCE, 35);
            SameMarketValue.Add(MarketType.FME, 36);
            SameMarketValue.Add(MarketType.ForexInter, 37);
            SameMarketValue.Add(MarketType.StockOption, 38);
            SameMarketValue.Add(MarketType.FuturesOption, 39);

            List<MarketType> shszMarketTypes = new List<MarketType>();
            shszMarketTypes.Add(MarketType.SHALev1);
            shszMarketTypes.Add(MarketType.SHALev2);
            shszMarketTypes.Add(MarketType.SZALev1);
            shszMarketTypes.Add(MarketType.SZALev2);
            shszMarketTypes.Add(MarketType.SHBLev1);
            shszMarketTypes.Add(MarketType.SHBLev2);
            shszMarketTypes.Add(MarketType.SZBLev1);
            shszMarketTypes.Add(MarketType.SZBLev2);
            shszMarketTypes.Add(MarketType.SHINDEX);
            shszMarketTypes.Add(MarketType.SZINDEX);
            shszMarketTypes.Add(MarketType.EMINDEX);
            shszMarketTypes.Add(MarketType.IF);
            shszMarketTypes.Add(MarketType.SHF);
            shszMarketTypes.Add(MarketType.CZC);
            shszMarketTypes.Add(MarketType.DCE);
            shszMarketTypes.Add(MarketType.TB_OLD);
            shszMarketTypes.Add(MarketType.TB_NEW);
            shszMarketTypes.Add(MarketType.SHConvertBondLev1);
            shszMarketTypes.Add(MarketType.SHConvertBondLev2);
            shszMarketTypes.Add(MarketType.SZConvertBondLev1);
            shszMarketTypes.Add(MarketType.SZConvertBondLev2);
            shszMarketTypes.Add(MarketType.SHNonConvertBondLev1);
            shszMarketTypes.Add(MarketType.SHNonConvertBondLev2);
            shszMarketTypes.Add(MarketType.SZNonConvertBondLev1);
            shszMarketTypes.Add(MarketType.SZNonConvertBondLev2);
            shszMarketTypes.Add(MarketType.SHFundLev1);
            shszMarketTypes.Add(MarketType.SHFundLev2);
            shszMarketTypes.Add(MarketType.SZFundLev1);
            shszMarketTypes.Add(MarketType.SZFundLev2);
            shszMarketTypes.Add(MarketType.SHRepurchaseLevel1);
            shszMarketTypes.Add(MarketType.SHRepurchaseLevel2);
            shszMarketTypes.Add(MarketType.SZRepurchaseLevel1);
            shszMarketTypes.Add(MarketType.SZRepurchaseLevel2);
            InitMarketType[InitOrgStatus.SHSZ] = shszMarketTypes;

            List<MarketType> hkMarketTypes = new List<MarketType>();
            hkMarketTypes.Add(MarketType.HK);
            hkMarketTypes.Add(MarketType.HSINDEX);
            InitMarketType[InitOrgStatus.HK] = hkMarketTypes;

            List<MarketType> ibMarketTypes = new List<MarketType>();
            ibMarketTypes.Add(MarketType.InterBankRepurchase);
            ibMarketTypes.Add(MarketType.IB);
            InitMarketType[InitOrgStatus.IB] = ibMarketTypes;

            List<MarketType> whMarketTypes = new List<MarketType>();
            whMarketTypes.Add(MarketType.ForexSpot);
            whMarketTypes.Add(MarketType.ForexNonSpot);
            InitMarketType[InitOrgStatus.WH] = whMarketTypes;

            List<MarketType> usMarketTypes = new List<MarketType>();
            usMarketTypes.Add(MarketType.US);
            usMarketTypes.Add(MarketType.NasdaqIndex);
            InitMarketType[InitOrgStatus.US] = usMarketTypes;

            List<MarketType> cbtMarketTypes = new List<MarketType>();
            cbtMarketTypes.Add(MarketType.OSFuturesCBOT);
            InitMarketType[InitOrgStatus.OSF_CBT] = cbtMarketTypes;

            List<MarketType> cmxMarketTypes = new List<MarketType>();
            cmxMarketTypes.Add(MarketType.OSFutures);
            InitMarketType[InitOrgStatus.OSF_CMX_NMX] = cmxMarketTypes;

            List<MarketType> sgxMarketTypes = new List<MarketType>();
            sgxMarketTypes.Add(MarketType.OSFuturesSGX);
            InitMarketType[InitOrgStatus.OSF_SGX] = sgxMarketTypes;

            List<MarketType> lemelecMarketTypes = new List<MarketType>();
            lemelecMarketTypes.Add(MarketType.OSFuturesLMEElec);
            InitMarketType[InitOrgStatus.LME_ELEC] = lemelecMarketTypes;

            List<MarketType> lemfloorMarketTypes = new List<MarketType>();
            lemfloorMarketTypes.Add(MarketType.OSFuturesLMEVenue);
            InitMarketType[InitOrgStatus.LME_FLOOR] = lemfloorMarketTypes;

            HistoryTrendMarket.Add(MarketType.SHALev1);
            HistoryTrendMarket.Add(MarketType.SHALev2);
            HistoryTrendMarket.Add(MarketType.SZALev1);
            HistoryTrendMarket.Add(MarketType.SZALev2);
            HistoryTrendMarket.Add(MarketType.SHBLev1);
            HistoryTrendMarket.Add(MarketType.SHBLev2);
            HistoryTrendMarket.Add(MarketType.SZBLev1);
            HistoryTrendMarket.Add(MarketType.SHINDEX);
            HistoryTrendMarket.Add(MarketType.SZINDEX);
            HistoryTrendMarket.Add(MarketType.EMINDEX);
            HistoryTrendMarket.Add(MarketType.CSINDEX);
            HistoryTrendMarket.Add(MarketType.CSIINDEX);
            HistoryTrendMarket.Add(MarketType.SHConvertBondLev1);
            HistoryTrendMarket.Add(MarketType.SHConvertBondLev2);
            HistoryTrendMarket.Add(MarketType.SZConvertBondLev1);
            HistoryTrendMarket.Add(MarketType.SZConvertBondLev2);
            HistoryTrendMarket.Add(MarketType.SHNonConvertBondLev1);
            HistoryTrendMarket.Add(MarketType.SHNonConvertBondLev2);
            HistoryTrendMarket.Add(MarketType.SZNonConvertBondLev1);
            HistoryTrendMarket.Add(MarketType.SZNonConvertBondLev2);
            HistoryTrendMarket.Add(MarketType.SHFundLev1);
            HistoryTrendMarket.Add(MarketType.SHFundLev2);
            HistoryTrendMarket.Add(MarketType.SZFundLev1);
            HistoryTrendMarket.Add(MarketType.SZFundLev2);

            FundFinancingTypeList.Add(MarketType.MonetaryFund);
            FundFinancingTypeList.Add(MarketType.NonMonetaryFund);
            FundFinancingTypeList.Add(MarketType.CIP);
            FundFinancingTypeList.Add(MarketType.CIPMonetary);
            FundFinancingTypeList.Add(MarketType.BFP);
            FundFinancingTypeList.Add(MarketType.TRPm);
            FundFinancingTypeList.Add(MarketType.TRPSun);
            FundFinancingTypeList.Add(MarketType.InterestRate);

            HasDivideRightTypeList.Add(MarketType.SHALev1);//个股
            HasDivideRightTypeList.Add(MarketType.SHALev2);
            HasDivideRightTypeList.Add(MarketType.SZALev1);
            HasDivideRightTypeList.Add(MarketType.SZALev2);
            HasDivideRightTypeList.Add(MarketType.SHBLev1);
            HasDivideRightTypeList.Add(MarketType.SHBLev2);
            HasDivideRightTypeList.Add(MarketType.SZBLev1);
            HasDivideRightTypeList.Add(MarketType.SZBLev2);
            HasDivideRightTypeList.Add(MarketType.MonetaryFund);//基金
            HasDivideRightTypeList.Add(MarketType.NonMonetaryFund);
            HasDivideRightTypeList.Add(MarketType.SHFundLev1);
            HasDivideRightTypeList.Add(MarketType.SHFundLev2);
            HasDivideRightTypeList.Add(MarketType.SZFundLev1);
            HasDivideRightTypeList.Add(MarketType.SZFundLev2);
            HasDivideRightTypeList.Add(MarketType.CIP);//理财
            HasDivideRightTypeList.Add(MarketType.CIPMonetary);

            HasCallAuctionTypeList.Add(MarketType.SHALev1);
            HasCallAuctionTypeList.Add(MarketType.SHALev2);
            HasCallAuctionTypeList.Add(MarketType.SZALev1);
            HasCallAuctionTypeList.Add(MarketType.SZALev2);
            HasCallAuctionTypeList.Add(MarketType.SHBLev1);
            HasCallAuctionTypeList.Add(MarketType.SHBLev2);
            HasCallAuctionTypeList.Add(MarketType.SZBLev1);
            HasCallAuctionTypeList.Add(MarketType.SZBLev2);
            HasCallAuctionTypeList.Add(MarketType.IF);
            HasCallAuctionTypeList.Add(MarketType.GoverFutures);

            GlobalIndexTypeList.Add(MarketType.GLOBAL);
            GlobalIndexTypeList.Add(MarketType.HSINDEX);
            GlobalIndexTypeList.Add(MarketType.JKINDEX);
            GlobalIndexTypeList.Add(MarketType.MalaysiaIndex);
            GlobalIndexTypeList.Add(MarketType.KoreaIndex);
            GlobalIndexTypeList.Add(MarketType.NikkeiIndex);
            GlobalIndexTypeList.Add(MarketType.PhilippinesIndex);
            GlobalIndexTypeList.Add(MarketType.SensexIndex);
            GlobalIndexTypeList.Add(MarketType.SingaporeIndex);
            GlobalIndexTypeList.Add(MarketType.TaiwanIndex);
            GlobalIndexTypeList.Add(MarketType.NewZealandIndex);
            GlobalIndexTypeList.Add(MarketType.NasdaqIndex);
            GlobalIndexTypeList.Add(MarketType.DutchAEXIndex);
            GlobalIndexTypeList.Add(MarketType.AustriaIndex);
            GlobalIndexTypeList.Add(MarketType.NorwayIndex);
            GlobalIndexTypeList.Add(MarketType.RussiaIndex);

            SHL1MarketType.Add(MarketType.SHALev1,1);
            SHL1MarketType.Add(MarketType.SHBLev1,2);
            SHL1MarketType.Add(MarketType.SHConvertBondLev1,3);
            SHL1MarketType.Add(MarketType.SHNonConvertBondLev1,4);
            SHL1MarketType.Add(MarketType.SHFundLev1,5);
            SHL1MarketType.Add(MarketType.SHRepurchaseLevel1,6);

            SHL2MarketType.Add(MarketType.SHALev2,1);
            SHL2MarketType.Add(MarketType.SHBLev2,2);
            SHL2MarketType.Add(MarketType.SHConvertBondLev2,3);
            SHL2MarketType.Add(MarketType.SHNonConvertBondLev2,4);
            SHL2MarketType.Add(MarketType.SHFundLev2,5);
            SHL2MarketType.Add(MarketType.SHRepurchaseLevel2,6);

            SZL1MarketType.Add(MarketType.SZALev1,1);
            SZL1MarketType.Add(MarketType.SZBLev1,2);
            SZL1MarketType.Add(MarketType.SZConvertBondLev1,3);
            SZL1MarketType.Add(MarketType.SZNonConvertBondLev1,4);
            SZL1MarketType.Add(MarketType.SZFundLev1,5);
            SZL1MarketType.Add(MarketType.SZRepurchaseLevel1,6);

            SZL2MarketType.Add(MarketType.SZALev2,1);
            SZL2MarketType.Add(MarketType.SZBLev2,2);
            SZL2MarketType.Add(MarketType.SZConvertBondLev2,3);
            SZL2MarketType.Add(MarketType.SZNonConvertBondLev2,4);
            SZL2MarketType.Add(MarketType.SZFundLev2,5);
            SZL2MarketType.Add(MarketType.SZRepurchaseLevel2,6);

            SHLevel2Stock.Add(MarketType.SHALev2);
            SHLevel2Stock.Add(MarketType.SHBLev2);
            SZLevel2Stock.Add(MarketType.SZALev2);
            SZLevel2Stock.Add(MarketType.SZBLev2);

            Level2MarketType.Add(MarketType.SHALev2);
            Level2MarketType.Add(MarketType.SZALev2);
            Level2MarketType.Add(MarketType.SHBLev2);
            Level2MarketType.Add(MarketType.SZBLev2);
            Level2MarketType.Add(MarketType.SHConvertBondLev2);
            Level2MarketType.Add(MarketType.SZConvertBondLev2);
            Level2MarketType.Add(MarketType.SHNonConvertBondLev2);
            Level2MarketType.Add(MarketType.SZNonConvertBondLev2);
            Level2MarketType.Add(MarketType.SHFundLev2);
            Level2MarketType.Add(MarketType.SZFundLev2);
            Level2MarketType.Add(MarketType.SHRepurchaseLevel2);
            Level2MarketType.Add(MarketType.SZRepurchaseLevel2);

            BMarketType.Add(MarketType.SHBLev1);
            BMarketType.Add(MarketType.SHBLev2);
            BMarketType.Add(MarketType.SZBLev1);
            BMarketType.Add(MarketType.SZBLev2);

            IndexTypes.Add(MarketType.SHINDEX);
            IndexTypes.Add(MarketType.SZINDEX);
            IndexTypes.Add(MarketType.EMINDEX);
            IndexTypes.Add(MarketType.CSINDEX);
            IndexTypes.Add(MarketType.CSIINDEX);
            IndexTypes.Add(MarketType.GLOBAL);
            IndexTypes.Add(MarketType.CNINDEX);
            IndexTypes.Add(MarketType.HSINDEX);
            IndexTypes.Add(MarketType.JKINDEX);
            IndexTypes.Add(MarketType.MalaysiaIndex);
            IndexTypes.Add(MarketType.KoreaIndex);
            IndexTypes.Add(MarketType.NikkeiIndex);
            IndexTypes.Add(MarketType.PhilippinesIndex);
            IndexTypes.Add(MarketType.SensexIndex);
            IndexTypes.Add(MarketType.SingaporeIndex);
            IndexTypes.Add(MarketType.TaiwanIndex);
            IndexTypes.Add(MarketType.NewZealandIndex);
            IndexTypes.Add(MarketType.NasdaqIndex);
            IndexTypes.Add(MarketType.DutchAEXIndex);
            IndexTypes.Add(MarketType.AustriaIndex);
            IndexTypes.Add(MarketType.NorwayIndex);
            IndexTypes.Add(MarketType.RussiaIndex);

            FDDX.name = "DDX";
            FDDX.drawtype = 1;
            FDDX.type = 1;
            FDDX.src = Marshal.StringToHGlobalAnsi(
                    @"DDX:(BIGORDER1-BIGORDER2)*volflow/capitalflow*100,color3d;
                        N4:=MIN(BARSCOUNT(C),N1);
                        DDX1:EMA(DDX,N4)*N4;
                        DDX2:MA(DDX1,N2);
                        DDX3:MA(DDX1,N3);");

            FDDY.name = "DDY";
            FDDY.drawtype = 1;
            FDDY.type = 1;
            FDDY.src = Marshal.StringToHGlobalAnsi(
                    @"DDY:(ORDER2-ORDER1)/capitalflow*((2-BIGORDER1-BIGORDER2)*volflow/(ORDER1+ORDER2))*100,color3d;
                        M4:=MIN(BARSCOUNT(C),M1);
                        DDY1:EMA(DDY,M4)*M4;
                        DDY2:MA(DDY1,M2);
                        DDY3:MA(DDY1,M3);");

            FDDZ.name = "DDZ";
            FDDZ.drawtype = 1;
            FDDZ.type = 1;
            FDDZ.src = Marshal.StringToHGlobalAnsi(
                    @"ddz:(BIGORDER1*volflow/ORDER1-BIGORDER2*volflow/ORDER2)/(2*volflow/(ORDER1+ORDER2))*100;
                        FLOATRGN(DDZ, (BIGORDER1 * volflow / ORDER1 -BIGORDER2 * volflow /ORDER2)/100, BIGORDER1 * volflow / ORDER1 > BIGORDER2 * volflow / ORDER2, RGB(255, 0, 0), BIGORDER1 * volflow / ORDER1 < BIGORDER2 * volflow / ORDER2, RGB(0, 255, 0));");

            FFinancialGame.name = "资金博弈";
            FFinancialGame.drawtype = 1;
            FFinancialGame.type = 1;
            FFinancialGame.src = Marshal.StringToHGlobalAnsi(
                    @"super:=(BIGORDER13-BIGORDER23)*volflow;							                       
                        big:=(BIGORDER12-BIGORDER22-BIGORDER13+BIGORDER23)*volflow;		
                        middle:=(BIGORDER11-BIGORDER21-BIGORDER12+BIGORDER22)*volflow;		
                        small:=(BIGORDER21-BIGORDER11)*volflow;			
                        a:=COUNT(BIGORDER10,0);							
                        b:=min(a,60);							
                        超级资金:ema(super,b)*b/capitalflow*100;	
                        大户资金:ema(big,b)*b/capitalflow*100;		
                        中户资金:ema(middle,b)*b/capitalflow*100;			
                        散户资金:ema(small,b)*b/capitalflow*100;");

            FFinancialTrend.name = "资金趋势";
            FFinancialTrend.drawtype = 1;
            FFinancialTrend.type = 1;
            FFinancialTrend.src = Marshal.StringToHGlobalAnsi(
                    @"当日资金流:BIGAMOUNT12,color3d;
                    五日平均:ma((BIGAMOUNT12),5);
                    十日平均:ma((BIGAMOUNT12),10);
                    二十日平均:ma((BIGAMOUNT12),20);");

            FTrendDDX.name = "分时DDX";
            FTrendDDX.drawtype = 1;
            FTrendDDX.type = 1;
            FTrendDDX.src = Marshal.StringToHGlobalAnsi(
                    @"DDX:(SumSuperOrder+SumBigOrder)/capitalflow*100,color3d;
                        DDX累计:sum(ddx,0);");
            
            FTrendFinancialGame.name = "分时博弈";
            FTrendFinancialGame.drawtype = 1;
            FTrendFinancialGame.type = 1;
            FTrendFinancialGame.src = Marshal.StringToHGlobalAnsi(
                    @" 超大:sum(SumSuperOrder/capitalflow*100,0);							
                        大户:sum(SumBigOrder/capitalflow*100,0);							
                        中户:sum(SumMiddleOrder/capitalflow*100,0);					
                        散户:sum(SumSmallOrder/capitalflow*100,0);");

        }
        /// <summary>
        /// 市场代码：上海，深圳，香港
        /// </summary>
        public MarketType Market_Code ;

        /// <summary>
        /// 股票类型：指数，A股，b股...
        /// </summary>
        private readonly SecurityType Security_Type;

        /// <summary>
        /// Code和MarketType对应关系
        /// </summary>
        // public static Dictionary<string,MarketType>  CodeMarket = new Dictionary<string, MarketType>();

        /// <summary>
        /// 期货ShortCode和EMCode对应关系
        /// </summary>
        public static Dictionary<string, string> FuturesCode = new Dictionary<string, string>(400);

        public static Dictionary<string, string> DicGlobalIndexCftToOrg = new Dictionary<string, string>(22);

        public static Dictionary<string, string> DicGlobalIndexOrgToCft = new Dictionary<string, string>(22);

        public static List<MarketType> CFTMarketTypes = new List<MarketType>();

        /// <summary>
        /// 市场类型对应走势画法：1、均线，价格线都画 2、只画价格线 3、当前交易日只画价格线，历史走势画价格及均线
        /// </summary>
        public static Dictionary<MarketType,byte> TrendStyleByMarket=new Dictionary<MarketType, byte>();

        /// <summary>
        /// 每个MarketType假设的值，用于判断两个MarketType是否需要触发OnMarketChanged
        /// </summary>
        public static Dictionary<MarketType, byte> SameMarketValue = new Dictionary<MarketType, byte>();

        /// <summary>
        /// 获取一个markettype买卖档宽度类型
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        public static InfoPanelWidthType GetInfoPanelWidthType(MarketType mt) {
            switch (mt) {
                case MarketType.OS:
                case MarketType.DCE:
                case MarketType.CZC:
                case MarketType.SHF:
                case MarketType.GLOBAL:
                case MarketType.CSIINDEX:
                case MarketType.CSINDEX:
                case MarketType.EMINDEX:
                case MarketType.SZINDEX:
                case MarketType.SHINDEX:
                case MarketType.CNINDEX:
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

                case MarketType.InterestRate:
                case MarketType.Chibor:
                case MarketType.InterBankRepurchase:
                case MarketType.RateSwap:
                case MarketType.HK:
                case MarketType.BC:
                
                case MarketType.GT:
                case MarketType.SHFundLev1:
                case MarketType.SHFundLev2:
                case MarketType.SZFundLev1:
                case MarketType.SZFundLev2:

                case MarketType.OSFuturesCBOT:
                case MarketType.OSFuturesSGX:
                case MarketType.OSFutures:
                case MarketType.US:
                case MarketType.ForexNonSpot:
                case MarketType.ForexSpot:
                case MarketType.OSFuturesLMEElec:
                case MarketType.OSFuturesLMEVenue:

                case MarketType.TB_NEW:
                case MarketType.TB_OLD:

                case MarketType.CHFAG:
                case MarketType.CHFCU:
                case MarketType.BCE:
                case MarketType.FME:
                case MarketType.ForexInter:
                case MarketType.StockOption:
                case MarketType.FuturesOption:
                    return InfoPanelWidthType.CommonSingle;
                
                case MarketType.CIP:
                case MarketType.CIPMonetary:
                case MarketType.ISP:
                case MarketType.BFP:
                case MarketType.TRPm:
                case MarketType.TRPSun:
                case MarketType.NonMonetaryFund:
                case MarketType.MonetaryFund:
                    return InfoPanelWidthType.OpenFund;
                case MarketType.IF:
                case MarketType.GoverFutures:
                    return InfoPanelWidthType.IFSingle;
                case MarketType.IB:
                    return InfoPanelWidthType.IB;
                default:
                    return InfoPanelWidthType.Double;
            }
        }

        /// <summary>
        /// 绘制历史走势的市场类型
        /// </summary>
        public static List<MarketType> HistoryTrendMarket = new List<MarketType>();

        /// <summary>
        /// 判断两个marketType的假设值是否一致
        /// </summary>
        /// <param name="mt1"></param>
        /// <param name="mt2"></param>
        /// <returns></returns>
        public static bool IsMarketValueChanged(MarketType mt1, MarketType mt2)
        {
            byte value1 = 0;
            byte value2 = 0;
            SameMarketValue.TryGetValue(mt1, out value1);
            SameMarketValue.TryGetValue(mt2, out value2);

            if (value1 == value2)
                return false;
            return true;
        }


        public static MarketType TypeCodeToMarketType(int typeCode, string emCode)
        {
            MarketType result = MarketType.NA;

            switch (typeCode)
            {
                //沪A
                case 1:
                    if (SystemConfig.UserInfo.HaveSHLevel2Right)
                        result = MarketType.SHALev2;
                    else
                        result = MarketType.SHALev1;
                    break;
                //深A
                case 2:
                    if (SystemConfig.UserInfo.HaveSZLevel2Right)
                        result = MarketType.SZALev2;
                    else
                        result = MarketType.SZALev1;
                    break;
                //沪B
                case 3:
                    if (SystemConfig.UserInfo.HaveSHLevel2Right)
                        result = MarketType.SHBLev2;
                    else
                        result = MarketType.SHBLev1;
                    break;
                //深B
                case 4:
                    if (SystemConfig.UserInfo.HaveSZLevel2Right)
                        result = MarketType.SZBLev2;
                    else
                        result = MarketType.SZBLev1;
                    break;
                //新三板
                case 5:
                    result = MarketType.TB_NEW;
                    break;
                //老三板
                case 6:
                    result = MarketType.TB_OLD;
                    break;
                //港股主板
                case 71:
                //港股创业板
                case 72:
                    result = MarketType.HK;
                    break;
                //债券-非交易所
                case 8:
                    if (emCode.EndsWith(".IB"))
                        result = MarketType.IB;
                    else
                        result = MarketType.BC;
                    break;
                //债券-交易所非可转债
                case 9:
                    if (emCode.EndsWith(".SH"))
                    {
                        if (SystemConfig.UserInfo.HaveSHLevel2Right)
                            result = MarketType.SHNonConvertBondLev2;
                        else
                            result = MarketType.SHNonConvertBondLev1;
                    }
                    else if (emCode.EndsWith(".SZ"))
                    {
                        if (SystemConfig.UserInfo.HaveSZLevel2Right)
                            result = MarketType.SZNonConvertBondLev2;
                        else
                            result = MarketType.SZNonConvertBondLev1;
                    }
                    break;
                //债券-交易所可转债
                case 10:
                    if (emCode.EndsWith(".SH"))
                    {
                        if (SystemConfig.UserInfo.HaveSHLevel2Right)
                            result = MarketType.SHConvertBondLev2;
                        else
                            result = MarketType.SHConvertBondLev1;
                    }
                    else if (emCode.EndsWith(".SZ"))
                    {
                        if (SystemConfig.UserInfo.HaveSZLevel2Right)
                            result = MarketType.SZConvertBondLev2;
                        else
                            result = MarketType.SZConvertBondLev1;
                    }
                    break;
                //商品期货
                case 11:
                    if (emCode.EndsWith(".DCE"))
                        result = MarketType.DCE;
                    else if (emCode.EndsWith(".CZC"))
                        result = MarketType.CZC;
                    else if (emCode.EndsWith(".SHF"))
                        result = MarketType.SHF;
                    break;
                //金融期货
                case 12:
                    if (emCode.EndsWith(".CFE"))
                        result = MarketType.IF;
                    break;
                //基金沪
                case 13:
                    if (SystemConfig.UserInfo.HaveSHLevel2Right)
                        result = MarketType.SHFundLev2;
                    else
                        result = MarketType.SHFundLev1;
                    break;
                //基金深
                case 14:
                    if (SystemConfig.UserInfo.HaveSZLevel2Right)
                        result = MarketType.SZFundLev2;
                    else
                        result = MarketType.SZFundLev1;
                    break;
                //基金货币型
                case 15:
                    result = MarketType.MonetaryFund;
                    break;
                //基金非货币型
                case 16:
                    result = MarketType.NonMonetaryFund;
                    break;
                //指数
                case 28:
                    result = MarketType.CNINDEX;
                    break;
                case 29:
                    result = MarketType.CSINDEX;
                    break;
                case 30:
                case 31:
                    result = MarketType.CSIINDEX;
                    break;
                case 17:
                    if (emCode.EndsWith(".SH"))
                        result = MarketType.SHINDEX;
                    else if (emCode.EndsWith(".SZ"))
                        result = MarketType.SZINDEX;
                    else if (emCode.EndsWith(".EI") || emCode.EndsWith(".IB"))
                        result = MarketType.EMINDEX;
                    else if (emCode.EndsWith(".GI"))
                        result = MarketType.GLOBAL;
                    else if (emCode.EndsWith(".CSI"))
                        result = MarketType.CSIINDEX;
                    break;
                //银行间回购
                case 18:
                    result = MarketType.InterBankRepurchase;
                    break;
                //交易所回购
                case 19:
                    if (emCode.EndsWith(".SH"))
                    {
                        if (SystemConfig.UserInfo.HaveSHLevel2Right)
                            result = MarketType.SHRepurchaseLevel2;
                        else
                            result = MarketType.SHRepurchaseLevel1;
                    }
                    else if (emCode.EndsWith(".SZ"))
                    {
                        if (SystemConfig.UserInfo.HaveSZLevel2Right)
                            result = MarketType.SZRepurchaseLevel2;
                        else
                            result = MarketType.SZRepurchaseLevel1;
                    }
                    break;
                //SHIBOR、HIBOR、LIBOR、基准利率、利率互换
                case 20:
                case 27:
                    result = MarketType.InterestRate;
                    break;
                case 34:
                    result = MarketType.RateSwap;
                    break;
                //CHIBOR
                case 21:
                    result = MarketType.Chibor;
                    break;
                //券商集合理财 CIP
                case 22:
                    result = MarketType.CIP;
                    break;
                //券商集合理财（货币式）
                case 33:
                    result = MarketType.CIPMonetary;
                    break;
                //保险理财 ISP
                case 23:
                    result = MarketType.ISP;
                    break;
                //银行理财 BFP
                case 24:
                    result = MarketType.BFP;
                    break;
                //信托理财 TRP
                case 25:
                    result = MarketType.TRPm;
                    break;
                case 26:
                    result = MarketType.TRPSun;
                    break;
                case 51:
                    result = MarketType.GoverFutures;
                    break;
                case 101:
                    result = MarketType.US;
                    break;
                case 111:
                    result = MarketType.OSFutures;
                    break;
                case 112:
                    result = MarketType.OSFuturesCBOT;
                    break;
                case 113:
                    result = MarketType.OSFuturesSGX;
                    break;
                case 114:
                    result = MarketType.OSFuturesLMEElec;
                    break;
                case 115:
                    result = MarketType.OSFuturesLMEVenue;
                    break;
                case 121:
                    result = MarketType.ForexNonSpot;
                    break;
                case 122:
                    result = MarketType.ForexSpot;
                    break;
                case 35:
                    result = MarketType.HSINDEX;
                    break;
                case 36:
                    result = MarketType.JKINDEX;
                    break;
                case 37:result = MarketType.MalaysiaIndex;
                    break;
                case 38:result = MarketType.KoreaIndex;
                    break;
                case 39:result = MarketType.NikkeiIndex;
                    break;
                case 40:result = MarketType.PhilippinesIndex;
                    break;
                case 41:result = MarketType.SensexIndex;
                    break;
                case 42:result = MarketType.SingaporeIndex;
                    break;
                case 43:result = MarketType.TaiwanIndex;
                    break;
                case 44:result = MarketType.NewZealandIndex;
                    break;
                case 45:result = MarketType.NasdaqIndex;
                    break;
                case 46:result = MarketType.DutchAEXIndex;
                    break;
                case 47:result = MarketType.AustriaIndex;
                    break;
                case 48:result = MarketType.NorwayIndex;
                    break;
                case 49:result = MarketType.RussiaIndex;
                    break;
                case 50:result = MarketType.CHFAG;
                    break;
                case 52: result = MarketType.CHFCU;
                    break;
                case 53: result = MarketType.BCE;
                    break;
                case 54: result = MarketType.FME;
                    break;
                case 124: result = MarketType.ForexInter;
                    break;
                case 55: result = MarketType.StockOption;
                    break;
                case 56: result = MarketType.FuturesOption;
                    break;

            }
            return result;
        }

        /// <summary>
        /// 根据typecode和emcode，获得markettype
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="emCode"></param>
        /// <returns></returns>
        public static MarketType TypeCodeToMarketType(string typeCode, string emCode)
        {
            int typeCodeInt = 0;
            if (!string.IsNullOrEmpty(typeCode))
                typeCodeInt = Convert.ToInt32(typeCode);
            return TypeCodeToMarketType(typeCodeInt,emCode);
        }

        /// <summary>
        /// 获取一只股票所在的板块id，用于上一只/下一只跳转
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetBlockId(int code)
        {
            string result = string.Empty;
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType) mtInt;


            switch (mt)
            {
                case MarketType.SHALev1:
                case MarketType.SHALev2:
                    result = "001005,001015,001016,001019，001020，001022";
                    break;
                case MarketType.SZALev1:
                case MarketType.SZALev2:
                    result = "001006,001015,001016,001019，001020，001022";
                    break;
                case MarketType.SHBLev1:
                case MarketType.SHBLev2:
                    result = "001012,001015,001016,001019，001020，001022";
                    break;
                case MarketType.SZBLev1:
                case MarketType.SZBLev2:
                    result = "001013,001015,001016,001019，001020，001022";
                    break;
                case MarketType.SHINDEX:
                case MarketType.SZINDEX:
                    result = "905008,902008,903001";
                    break;
                case MarketType.TB_NEW:
                case MarketType.TB_OLD:
                    result = "001014";
                    break;
                case MarketType.EMINDEX:
                    result = "905017";
                    break;
                case MarketType.HSINDEX:
                case MarketType.GLOBAL:
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
                    result = "905005,905007,905010";
                    break;
                case MarketType.CSINDEX:
                    result = "902002,902004,902008";
                    break;
                case MarketType.CSIINDEX:
                    result = "903009,904003,903001";
                    break;
                case MarketType.CNINDEX:
                    result =
                        "905008023,905008026,905008032,905008020,905008024,902008,903011,905008034,905008038,905008027,905008025,905008021,905008022";
                    break;
                case MarketType.SHConvertBondLev1:
                case MarketType.SHConvertBondLev2:
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHNonConvertBondLev2:
                case MarketType.IB:
                case MarketType.BC:
                    result = "611002,611003,611004,611001,611005,621001,621002";
                    break;
                case MarketType.SHFundLev1:
                case MarketType.SHFundLev2:
                case MarketType.SZFundLev1:
                case MarketType.SZFundLev2:
                case MarketType.MonetaryFund:
                case MarketType.NonMonetaryFund:
                    result = "507015,507004,507001,507022,50701";
                    break;
                case MarketType.InterBankRepurchase:
                case MarketType.SHRepurchaseLevel1:
                case MarketType.SHRepurchaseLevel2:
                case MarketType.SZRepurchaseLevel1:
                case MarketType.SZRepurchaseLevel2:
                case MarketType.Chibor:
                case MarketType.InterestRate:
                case MarketType.RateSwap:
                    result = "801001,801002,801003,806001,806002,807001,807002,807003,807004";
                    break;
                case MarketType.IF:
                    result = "701002";
                    break;
                case MarketType.SHF:
                case MarketType.CHFAG:
                case MarketType.CHFCU:
                    result = "702011";
                    break;
                case MarketType.DCE:
                    result = "703010";
                    break;
                case MarketType.CZC:
                    result = "704001";
                    break;
                case MarketType.OSFuturesLMEElec:
                case MarketType.OSFuturesLMEVenue:
                    result = "708001";
                    break;
                case MarketType.OSFutures:
                    result = "711001,712001";
                    break;
                case MarketType.OSFuturesCBOT:
                    result = "710001";
                    break;
                case MarketType.OSFuturesSGX:
                    result = "716001";
                    break;
                case MarketType.HK:
                    result = "401001,401014";
                    break;
                case MarketType.CIP:
                case MarketType.CIPMonetary:
                    result = "301001";
                    break;
                case MarketType.ISP:
                    result = "302001";
                    break;
                case MarketType.BFP:
                case MarketType.TRPm:
                    result = "304001,305001,305003,305002";
                    break;
                case MarketType.TRPSun:
                    result = "302001";
                    break;
                case MarketType.US:
                    result = "202001";
                    break;
                case MarketType.ForexNonSpot:
                case MarketType.ForexSpot:
                    result = "111001,111002,111003,111004,111018,111011";
                    break;
                case MarketType.FME:
                    result = "719001";
                    break;
                case MarketType.BCE:
                    result = "705001";
                    break;
                case MarketType.ForexInter:
                    result = "112002";
                    break;
                case MarketType.StockOption:
                case MarketType.FuturesOption:
                    break;
            }
            return result;
        }

        #region 放大倍数 暂时未启用
        /*
        /// <summary>
        /// 小数位数属性,设置证券价格小数位数和证券价格放大倍数
        /// </summary>
        public byte DecimalNum
        {
            get
            {
                if (IsBond)
                    return 4;
                if (IsWarrant || IsBond || IsFund || (IsSHMarket && IsEQTY_B) || IsHKMarket || IsRepurchase)
                    return 3;
                return 2;
            }
        }

        /// <summary>
        /// 价格放大倍数
        /// </summary>
        public double PricePowerNum { get { return 1000.0; } }

        /// <summary>
        /// 成交量放大倍数
        /// </summary>
        public byte VolumePowerNum
        {
            get
            {
                if (IsIndex)
                    return 100; //百股（手）
                if (IsIndexFuture)
                    return 1; //百股（手）
                if (IsBond || IsRepurchase)
                {
                    return (byte)(Market_Code == MarketCode.SH ? 1 : 10);
                }

                return 100; //百股（手）
            }
        }

        /// <summary>
        /// 成交金额放大倍数
        /// </summary>
        public int AmountPowerNum
        {
            get { return IsIndex ? 100000000 : 10000; }
        }
                 * */
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public SecurityAttribute(string code)
        {
            //Market_Code = GetMarketCodeByCode(code);
            //Security_Type = GetInstrumentTypeByCode(code);
        }

        ///// <summary>
        ///// 通过code获取交易市场
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public static MarketType GetMarketCodeByCode(string code)
        //{
        //    MarketType marketCode = MarketType.NA;
        //    if (code == null)
        //        return marketCode;
        //    if (code.EndsWith(".SH"))
        //    {
        //        string strShortCode = code.Substring(0, 6);
        //        if (char.IsNumber(strShortCode, 0))
        //        {
        //            int nShortCode = Convert.ToInt32(strShortCode);
        //            if ((nShortCode > 0 && nShortCode < 000080) ||
        //                (nShortCode >= 000090 && nShortCode <= 000999) ||
        //                (nShortCode >= 801000 && nShortCode <= 803000))
        //                marketCode = MarketType.SHINDEX;
        //            else if (SystemConfig.UserInfo.HaveSHLevel2Right)
        //                marketCode = MarketType.SHLev2;
        //            else
        //                marketCode = MarketType.SHLev1;
        //        }
        //        else
        //        {
        //            marketCode = MarketType.SHINDEX;
        //        }
        //    }
        //    else if (code.EndsWith(".SZ"))
        //    {
        //        if (code.StartsWith("399"))
        //            marketCode = MarketType.SZINDEX;
        //        else if (SystemConfig.UserInfo.HaveSZLevel2Right)
        //            marketCode = MarketType.SZLev2;
        //        else
        //            marketCode = MarketType.SZLev1;
        //    }
        //    else if(code.EndsWith(".OC"))
        //        marketCode = MarketType.TB;
        //    else if (code.EndsWith(".HK"))
        //        marketCode = MarketType.HK;
        //    else if (code.EndsWith(".OF"))
        //        marketCode = MarketType.OF;
        //    else if (code.EndsWith(".EI"))
        //        marketCode = MarketType.EMINDEX;
        //    else if (code.EndsWith(".CS"))
        //        marketCode = MarketType.CSINDEX;
        //    else if (code.EndsWith(".CSI"))
        //        marketCode = MarketType.CSINDEX;
        //    else if (code.EndsWith(".IB"))
        //        marketCode = MarketType.IB;
        //    else if (code.EndsWith(".BC"))
        //        marketCode = MarketType.BC;
        //    else if (code.EndsWith(".DCE"))
        //        marketCode = MarketType.DCE;
        //    else if (code.EndsWith(".SHF"))
        //        marketCode = MarketType.SHF;
        //    else if (code.EndsWith(".CZC"))
        //        marketCode = MarketType.CZC;
        //    else if (code.EndsWith(".CFE"))
        //        marketCode = MarketType.IF;
        //    else if (code.EndsWith(".TB"))
        //        marketCode = MarketType.TB;
        //    else if (code.StartsWith("BK"))
        //        marketCode = MarketType.EMINDEX;

        //    return marketCode;
        //}

        ///// <summary>
        ///// 通过Code获得品种类型
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public static SecurityType GetInstrumentTypeByCode(string code)
        //{
        //    SecurityType instrumentType;
        //    if (code.EndsWith(".SZ"))
        //    {
        //        int codenum = Convert.ToInt32(code.Substring(0, 6));
        //        switch (codenum)
        //        {
        //            case 160105:
        //            case 160106:
        //            case 160311:
        //            case 160314:
        //            case 160505:
        //            case 160607:
        //            case 160610:
        //            case 160611:
        //            case 160613:
        //            case 160706:
        //            case 160805:
        //            case 160910:
        //            case 161005:
        //            case 161010:
        //            case 161607:
        //            case 161610:
        //            case 161706:
        //            case 161903:
        //            case 162006:
        //            case 162207:
        //            case 162605:
        //            case 162607:
        //            case 162703:
        //            case 163302:
        //            case 163402:
        //            case 163503:
        //            case 163801:
        //            case 166001:
        //                instrumentType = SecurityType.FUND_LOF;
        //                break;
        //            default:
        //                if (codenum > 100000 && codenum < 130000)
        //                    instrumentType = SecurityType.BOND;
        //                else if (code.StartsWith("000") || code.StartsWith("001") || code.StartsWith("002") || code.StartsWith("30"))
        //                    instrumentType = SecurityType.EQTY_A;
        //                else if (code.StartsWith("031"))
        //                    instrumentType = SecurityType.WRNT;
        //                else if (codenum > 100000 && codenum < 130000)
        //                    instrumentType = SecurityType.BOND;
        //                else if (code.StartsWith("131"))
        //                    instrumentType = SecurityType.Repurchase;
        //                else if (code.StartsWith("150"))
        //                    instrumentType = SecurityType.FUND_CLOSE;
        //                else if (code.StartsWith("1599"))
        //                    instrumentType = SecurityType.FUND_ETF;
        //                else if (code.StartsWith("16"))
        //                    instrumentType = SecurityType.FUND_OPEN;
        //                else if (code.StartsWith("184"))
        //                    instrumentType = SecurityType.FUND_CLOSE;
        //                else if (code.StartsWith("200"))
        //                    instrumentType = SecurityType.EQTY_B;
        //                else if (code.StartsWith("399"))
        //                    instrumentType = SecurityType.Index;
        //                else
        //                    instrumentType = SecurityType.NA;
        //                break;
        //        }
        //    }
        //    else if (code.EndsWith(".SH"))
        //    {
        //        int codenum = Convert.ToInt32(code.Substring(0, 6));
        //        instrumentType = SecurityType.NA;
        //        if (codenum > 0)
        //        {
        //            if (codenum <= 000999)
        //            {
        //                if (codenum >= 80 && codenum <= 89)
        //                    instrumentType = SecurityType.BOND;
        //                else
        //                    instrumentType = SecurityType.Index;
        //            }
        //            else if (codenum < 139999)
        //            {
        //                if (codenum >= 110000 || (codenum > 1000 && codenum < 090000))
        //                    instrumentType = SecurityType.BOND;
        //            }
        //            else if (codenum < 205000)
        //            {
        //                if (codenum >= 201000)
        //                    instrumentType = SecurityType.Repurchase;
        //            }
        //            else if (codenum >= 500000)
        //            {
        //                if (codenum < 510000)
        //                    instrumentType = SecurityType.FUND_CLOSE;
        //                else if (codenum < 519000)
        //                    instrumentType = SecurityType.FUND_ETF;
        //                else if (codenum < 520000)
        //                    instrumentType = SecurityType.FUND_OPEN;
        //                else if (code.StartsWith("580"))
        //                    instrumentType = SecurityType.WRNT;
        //                else if (code.StartsWith("600") || code.StartsWith("601"))
        //                    instrumentType = SecurityType.EQTY_A;
        //                else if (code.StartsWith("900"))
        //                    instrumentType = SecurityType.EQTY_B;
        //                else if (codenum >= 801000 && codenum <= 803000)
        //                    instrumentType = SecurityType.Index;
        //            }
        //        }
        //    }
        //    else if (code.EndsWith(".HK"))
        //        instrumentType = SecurityType.EQTY_HK;
        //    else if (code.EndsWith(".IF"))
        //        instrumentType = SecurityType.INDEX_FUTURE;
        //    else if (code.EndsWith(".IB"))
        //        instrumentType = SecurityType.BOND;
        //    else if (code.EndsWith(".FND"))
        //        instrumentType = SecurityType.FUND_OPEN;
        //    else if (code.EndsWith(".TB"))
        //        instrumentType = SecurityType.EQTY_TB;
        //    else if (code.EndsWith(".CSI"))
        //        instrumentType = SecurityType.Index;
        //    else
        //        instrumentType = SecurityType.EQTY_A;
        //    return instrumentType;
        //}

        /// <summary>
        /// 获取价格的精度
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetPriceAccuracy(int code)
        {
            if (code == 0)
                return "F2";
            string result = "F2";
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;
            
            switch (mt)
            {
                case MarketType.HK:
                case MarketType.SHFundLev1:
                case MarketType.SHFundLev2:
                case MarketType.SZFundLev1:
                case MarketType.SZFundLev2:
                case MarketType.SHBLev1:
                case MarketType.SHBLev2:
                case MarketType.SZBLev1:
                case MarketType.SZBLev2:
                case MarketType.US:
                case MarketType.GoverFutures:
                    result = "F3";
                    break;
                case MarketType.CZC:
                case MarketType.SHF:
                case MarketType.DCE:
                case MarketType.CHFAG:
                case MarketType.OSFutures:
                case MarketType.OSFuturesCBOT:
                case MarketType.OSFuturesSGX:
                case MarketType.OSFuturesLMEElec:
                case MarketType.OSFuturesLMEVenue:
                    result = "F2";
                    break;
                case MarketType.IB:
                case MarketType.SHConvertBondLev1:
                case MarketType.SHConvertBondLev2:
                case MarketType.SZConvertBondLev1:
                case MarketType.SZConvertBondLev2:
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHNonConvertBondLev2:
                case MarketType.SZNonConvertBondLev1:
                case MarketType.SZNonConvertBondLev2:
                case MarketType.BC:
                case MarketType.ForexSpot:
                case MarketType.ForexNonSpot:
                case MarketType.CIP:
                case MarketType.CIPMonetary:
                case MarketType.ISP:
                case MarketType.BFP:
                case MarketType.TRPm:
                case MarketType.TRPSun:
                case MarketType.InterBankRepurchase:
                case MarketType.SHRepurchaseLevel1:
                case MarketType.SHRepurchaseLevel2:
                case MarketType.SZRepurchaseLevel1:
                case MarketType.SZRepurchaseLevel2:
                case MarketType.Chibor:
                case MarketType.InterestRate:
                case MarketType.RateSwap:
                case MarketType.MonetaryFund:
                case MarketType.NonMonetaryFund:
                case MarketType.ForexInter:
                    result = "F4";
                    break;
                case MarketType.IF:
                    result = "F1";
                    break;
                case MarketType.CHFCU:
                    result = "F0";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 检查指定板块ID是否为服务端证券类别市场
        /// </summary>
        /// <param name="sectorID">需要验证的板块ID</param>
        /// <returns>验证结果, true:板块, false:市场</returns>
        public static bool CheckMarketOrSector(string sectorID)
        {
            return sectorID.StartsWith("BK");

            //int sID = int.Parse(sectorID);

            //return Enum.IsDefined(typeof(ReqSecurityType), sID);
        }

        /// <summary>
        /// 获取报告日期的季度
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetQuarterBGRQ(int date)
        {
            int mounthday = date%10000;
            if (mounthday >= 1200)
                return 4;
            if(mounthday >=900 && mounthday < 1200)
                return 3;
            if (mounthday >= 600 && mounthday < 900)
                return 2;
            if (mounthday >= 300 && mounthday < 600)
                return 1;

            return 0;
        }

        /// <summary>
        /// 根据市场类型判断收发订阅包:请求包、请求响应包、订阅包、订阅响应包,历史k线包,历史k线响应包，当日k线请求包，当日k线响应包,历史走势请求包，历史走势响应包
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public static Type[] GetResAndReqDataPacketByMarket(MarketType market)
        {
            Type[] result=new Type[10];//请求包、请求响应包、订阅包、订阅响应包
            switch (market)
            {
                #region 交易所
                case MarketType.SHALev1:
                case MarketType.SHALev2:
                case MarketType.SZALev1:
                case MarketType.SZALev2:
                case MarketType.SHBLev1:
                case MarketType.SHBLev2:
                case MarketType.SZBLev1:
                case MarketType.SZBLev2:
                case MarketType.SHConvertBondLev1:
                case MarketType.SHConvertBondLev2:
                case MarketType.SZConvertBondLev1:
                case MarketType.SZConvertBondLev2:
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHNonConvertBondLev2:
                case MarketType.SZNonConvertBondLev1:
                case MarketType.SZNonConvertBondLev2:
                case MarketType.SHFundLev1:
                case MarketType.SHFundLev2:
                case MarketType.SZFundLev1:
                case MarketType.SZFundLev2:
                case MarketType.SHRepurchaseLevel1:
                case MarketType.SHRepurchaseLevel2:
                case MarketType.SZRepurchaseLevel1:
                case MarketType.SZRepurchaseLevel2:
                case MarketType.TB_NEW:
                case MarketType.TB_OLD:
                    result[0] = typeof(ReqStockTrendDataPacket);
                    result[1] = typeof(ResStockTrendDataPacket);
                    result[2] = typeof(ReqStockDetailLev2DataPacket);
                    result[3] = typeof(ResStockDetailLev2DataPacket);
                    result[4] = typeof(ReqHisKLineDataPacket);
                    result[5] = typeof(ResHisKLineDataPacket);
                    result[6] = typeof(ReqMinKLineDataPacket);
                    result[7] = typeof(ResMinKLineDataPacket);
                    result[8] = typeof(ReqHisTrendDataPacket);
                    result[9] = typeof(ResHisTrendDataPacket);
                    break;
                #endregion    

                #region 沪深指数
                case MarketType.SHINDEX:
                case MarketType.SZINDEX:
                    result[0] = typeof(ReqStockTrendDataPacket);
                    result[1] = typeof(ResStockTrendDataPacket);
                    result[2] = typeof(ReqStockDetailLev2DataPacket);
                    result[3] = typeof(ResStockDetailLev2DataPacket);
                    result[4] = typeof(ReqHisKLineDataPacket);
                    result[5] = typeof(ResHisKLineDataPacket);
                    result[6] = typeof(ReqMinKLineDataPacket);
                    result[7] = typeof(ResMinKLineDataPacket);
                    result[8] = typeof(ReqHisTrendDataPacket);
                    result[9] = typeof(ResHisTrendDataPacket);
                    break;
                #endregion

                #region 股指期货
                case MarketType.IF:
                case MarketType.GoverFutures:
                    result[0] = typeof(ReqIndexFuturesTrendDataPacket);
                    result[1] = typeof(ResIndexFuturesTrendDataPacket);
                    result[2] = typeof(ReqIndexFuturesDetailDataPacket);
                    result[3] = typeof(ResIndexFuturesDetailDataPacket);
                    result[4] = typeof(ReqHisKLineDataPacket);
                    result[5] = typeof(ResHisKLineDataPacket);
                    result[6] = typeof(ReqMinKLineDataPacket);
                    result[7] = typeof(ResMinKLineDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                #endregion

                #region 外盘
                case MarketType.SHF:
                case MarketType.CZC:
                case MarketType.DCE:
                case MarketType.HK:
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
                    result[0] = typeof(ReqOceanTrendDataPacket);
                    result[1] = typeof(ResOceanTrendDataPacket);
                    result[2] = typeof(ReqOceanRecordDataPacket);
                    result[3] = typeof(ResOceanRecordDataPacket);
                    result[4] = typeof(ReqHisKLineDataPacket);
                    result[5] = typeof(ResHisKLineDataPacket);
                    result[6] = typeof(ReqMinKLineDataPacket);
                    result[7] = typeof(ResMinKLineDataPacket);
                    result[8] = typeof(ReqHisTrendDataPacket);
                    result[9] = typeof(ResHisTrendDataPacket);
                    break;
                #endregion

                #region 高频
                case MarketType.EMINDEX://东财指数
                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqEMIndexDetailDataPacket);
                    result[3] = typeof(ResEMIndexDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                case MarketType.CNINDEX://巨潮指数
                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqCNIndexDetailDataPacket);
                    result[3] = typeof(ResCNIndexDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                case MarketType.ForexSpot://外汇
                case MarketType.ForexNonSpot:
                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqForexDetailDataPacket);
                    result[3] = typeof(ResForexDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                case MarketType.US://美股
                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqUSStockDetailDataPacket);
                    result[3] = typeof(ResUSStockDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                case MarketType.OSFutures://海外期货
                case MarketType.OSFuturesCBOT:
                case MarketType.OSFuturesSGX:
                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqOSFuturesDetailDataPacket);
                    result[3] = typeof(ResOSFuturesDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                case MarketType.OSFuturesLMEElec:
                case MarketType.OSFuturesLMEVenue:
                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqOSFuturesLMEDetailDataPacket);
                    result[3] = typeof(ResOSFuturesLMEDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                #endregion

                #region 低频
                case MarketType.IB://银行间债券
                case MarketType.BC:
                    result[0] =typeof(ReqTrendOrgLowDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqInterBankDetailDataPacket);
                    result[3] = typeof(ResInterBankDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgLowDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                case MarketType.CSIINDEX://中证指数
                    result[0] = typeof(ReqTrendOrgLowDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqCSIIndexDetailDataPacket);
                    result[3] = typeof(ResCSIIndexDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgLowDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                case MarketType.CSINDEX://中债指数
                    result[0] = typeof(ReqTrendOrgLowDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqCSIndexDetailDataPacket);
                    result[3] = typeof(ResCSIndexDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgLowDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                case MarketType.RateSwap://利率互换
                    result[0] = typeof(ReqTrendOrgLowDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqRateSwapDetailDataPacket);
                    result[3] = typeof(ResRateSwapDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgLowDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                case MarketType.InterBankRepurchase://银行间回购与拆借
                    result[0] = typeof(ReqTrendOrgLowDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqInterBankRepurchaseDetailDataPacket);
                    result[3] = typeof(ResInterBankRepurchaseDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgLowDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                case MarketType.Chibor:
                    result[0] = typeof(ReqTrendOrgLowDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqShiborDetailDataPacket);
                    result[3] = typeof(ResShiborDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgLowDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = null;
                    result[9] = null;
                    break;
                case MarketType.InterestRate://净值线
                case MarketType.CIP:
                case MarketType.CIPMonetary:
                case MarketType.BFP:
                case MarketType.TRPm:
                case MarketType.TRPSun:
                case MarketType.MonetaryFund:
                case MarketType.NonMonetaryFund:
                    result[0] = null;
                    result[1] = null;
                    result[2] = null;
                    result[3] = null;
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = null;
                    result[7] = null;
                    result[8] = null;
                    result[9] = null;
                    break;
                #endregion
                case MarketType.OS:
                case MarketType.GT:

                    result[0] = typeof(ReqTrendOrgDataPacket);
                    result[1] = typeof(ResTrendOrgDataPacket);
                    result[2] = typeof(ReqEMIndexDetailDataPacket);
                    result[3] = typeof(ResEMIndexDetailDataPacket);
                    result[4] = typeof(ReqHisKLineOrgDataPacket);
                    result[5] = typeof(ResHisKLineOrgDataPacket);
                    result[6] = typeof(ReqMintKLineOrgDataPacket);
                    result[7] = typeof(ResMintKLineOrgDataPacket);
                    result[8] = typeof(ReqTrendOrgDataPacket);
                    result[9] = typeof(ResTrendOrgDataPacket);
                    break;
                default:
                    result[0] = typeof(ReqStockTrendDataPacket);
                    result[1] = typeof(ResStockTrendDataPacket);
                    result[2] = typeof(ReqStockDetailLev2DataPacket);
                    result[3] = typeof (ReqStockDetailLev2DataPacket);
                    result[4] = typeof(ReqHisKLineDataPacket);
                    result[5] = typeof(ResHisKLineDataPacket);
                    result[6] = typeof(ReqMinKLineDataPacket);
                    result[7] = typeof(ResMinKLineDataPacket);
                    result[8] = typeof(ReqHisTrendDataPacket);
                    result[9] = typeof(ResHisTrendDataPacket);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 绘制净值线的市场类型
        /// </summary>
        public static List<MarketType>  FundFinancingTypeList=new List<MarketType>();
        /// <summary>
        /// 有除复权功能的市场类型
        /// </summary>
        public static List<MarketType> HasDivideRightTypeList=new List<MarketType>(); 
        /// <summary>
        /// 有集合竞价功能的市场类型
        /// </summary>
        public static List<MarketType> HasCallAuctionTypeList=new List<MarketType>();
        /// <summary>
        /// 全球指数市场类型
        /// </summary>
        public static List<MarketType> GlobalIndexTypeList = new List<MarketType>();

        /// <summary>
        /// 上海市场lev2类型
        /// </summary>
        public static Dictionary<MarketType, int> SHL2MarketType = new Dictionary<MarketType, int>();
        /// <summary>
        /// 深圳市场Lev2类型
        /// </summary>
        public static Dictionary<MarketType, int> SZL2MarketType = new Dictionary<MarketType, int>();
        /// <summary>
        /// 上海市场Lev1类型
        /// </summary>
        public static Dictionary<MarketType,int> SHL1MarketType = new Dictionary<MarketType,int>();
        /// <summary>
        /// 深圳市场Lev1类型
        /// </summary>
        public static Dictionary<MarketType,int> SZL1MarketType = new Dictionary<MarketType,int>();
        /// <summary>
        /// 沪个股lev2
        /// </summary>
        public static List<MarketType> SHLevel2Stock = new List<MarketType>();
        /// <summary>
        /// 深个股lev2
        /// </summary>
        public static List<MarketType> SZLevel2Stock = new List<MarketType>();
        /// <summary>
        /// Level2权限的市场类型
        /// </summary>
        public static List<MarketType> Level2MarketType = new List<MarketType>();
        /// <summary>
        /// b股市场
        /// </summary>
        public static List<MarketType> BMarketType = new List<MarketType>();
        /// <summary>
        /// DDX指标
        /// </summary>
        public static Formula.Formula FDDX = new Formula.Formula();
        /// <summary>
        /// DDY指标
        /// </summary>
        public static Formula.Formula FDDY = new Formula.Formula();
        /// <summary>
        /// DDZ指标
        /// </summary>
        public static Formula.Formula FDDZ = new Formula.Formula();
        /// <summary>
        /// 资金博弈
        /// </summary>
        public static Formula.Formula FFinancialGame = new Formula.Formula();
        /// <summary>
        /// 资金趋势
        /// </summary>
        public static Formula.Formula FFinancialTrend = new Formula.Formula();
        /// <summary>
        /// 分时DDX指标
        /// </summary>
        public static Formula.Formula FTrendDDX = new Formula.Formula();
        /// <summary>
        /// 分时博弈
        /// </summary>
        public static Formula.Formula FTrendFinancialGame = new Formula.Formula();
        /// <summary>
        /// 指数品种
        /// </summary>
        public static List<MarketType> IndexTypes=new List<MarketType>();

        /// <summary>
        /// 初始化按市场分的markettype
        /// </summary>
        public static Dictionary<InitOrgStatus, List<MarketType>> InitMarketType = new Dictionary<InitOrgStatus, List<MarketType>>(10);

    }
}

