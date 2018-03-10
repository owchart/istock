using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 
    /// </summary>
    public static class FieldInfoHelper
    {
        private static readonly DataCenterCore _Dc = DataCenterCore.CreateInstance();

        /// <summary>
        ///  [旧有]的根据code和fieldindex获得对应值（返回类型 object）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        [Obsolete("不推荐使用，因为该方法装箱一次，使用时你可能要拆箱")]
        public static object GetObjectValue(int code, FieldIndex fieldIndex)
        {
            object tempResult = null;
            int fieldIndexSeq = (short)fieldIndex;

            if (fieldIndexSeq < 300)
                tempResult = GetFieldValue(code, fieldIndex, DetailData.FieldIndexDataInt32);
            else if (fieldIndexSeq < 800)
                tempResult = GetFieldValue(code, fieldIndex, DetailData.FieldIndexDataSingle);
            else if (fieldIndexSeq < 1000)
                tempResult = GetFieldValue(code, fieldIndex, DetailData.FieldIndexDataDouble);
            else if (fieldIndexSeq < 1200)
                tempResult = GetFieldValue(code, fieldIndex, DetailData.FieldIndexDataInt64);
            else if (fieldIndexSeq < 9000)
                tempResult = GetFieldValue(code, fieldIndex, DetailData.FieldIndexDataString);
            else
                tempResult = GetFieldValue(code, fieldIndex, DetailData.FieldIndexDataObject);

            return tempResult;
        }

        /// <summary>
        /// 获得传入code的市场类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static MarketType GetMarketByCode(int code)
        {
            int market = 0;
            Dictionary<FieldIndex, int> dic;
            if (DetailData.FieldIndexDataInt32.TryGetValue(code, out  dic)
                && dic.TryGetValue(FieldIndex.Market, out market))
                return (MarketType)market;

            return (MarketType)market;
        }

        /// <summary>
        /// 获得字段信息
        /// </summary>
        /// <param name="market">市场类型</param>
        /// <param name="fieldName">界面字段名称</param>
        /// <returns></returns>
        public static FieldInfo GetFieldInfoByName(MarketType market, string fieldName)
        {
            FieldInfo result;
            Dictionary<string, FieldInfo> dic;
            if (FieldInfoCfgFileIO.DicMarketFieldInfo.TryGetValue(market, out dic)
                && dic.TryGetValue(fieldName, out result))
            {
                return result;
            }
            else
            {
                if (FieldInfoCfgFileIO.DicDefaultFieldInfo.TryGetValue(fieldName, out result))
                    return result;
            }
            return result;
        }

        /// <summary>
        /// 获得格式化后的值
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <param name="fieldIndex">整个字段的配置信息</param>
        /// <param name="format">输出格式</param>
        /// <param name="specialFieldIndexValue">颜色配置比较的对象（FieldIndex）</param>
        /// <param name="dic">该FieldIndex对应的字典</param>
        /// <returns></returns>
        public static string GetFieldFormattedValue<T>(int code, FieldIndex fieldIndex,
            Format format, float specialFieldIndexValue, Dictionary<int, Dictionary<FieldIndex, T>> dic)
        {
            string result = string.Empty;

            T tempResult = GetFieldValue(code, fieldIndex, dic);
            result = FormatField(tempResult, format, specialFieldIndexValue);

            return result;
        }

        /// <summary>
        /// 将原始数据按照Format内容进行格式化，并返回这个结果
        /// </summary>
        /// <typeparam name="T">原始数据的数据类型(Int32/Single/Int64/Double/String/Object)</typeparam>
        /// <param name="originalValue">原始数据</param>
        /// <param name="format">格式化的格式</param>
        /// <param name="zRuleComparedIndexValue">ZRule中比较的FieldIndex</param>
        /// <returns>进行格式化后的字符串</returns>
        private static string FormatField<T>(T originalValue, Format format, float zRuleComparedIndexValue)
        {
            if (originalValue == null)
                return string.Empty;
            string result = originalValue.ToString();

            // Run Z Rule.
            if (format.ZeroRule != null
                && format.ZeroRule.IsZero(result, zRuleComparedIndexValue))
            {
                return format.ZeroRule.ZeroStr;
            }

            // Run A/C/M/D rule.
            if (format.CalculateRules != null && format.CalculateRules.Count > 0)
            {
                foreach (CalculateRule cRule in format.CalculateRules)
                {
                    result = cRule.GetCalculatedString(result);
                }
            }

            // Run SRule.
            if (format.SRule != null)
            {
                result = format.SRule.GetSpecialedString(result);
                return result;
            }

            // Run NRule.
            if (format.NRule != null)
            {
                result = format.NRule.GetNFormatString(result);
            }

            // Run PRule.
            if (format.PRule != null)
            {
                result = format.PRule.GetProfixedString(result);
            }

            // Run QRule.
            if (format.QRule != null)
            {
                result = format.QRule.GetSuffixedString(result);
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(int code, FieldIndex fieldIndex,
            Dictionary<int, Dictionary<FieldIndex, T>> dic)
        {
            T result = default(T);
            Dictionary<FieldIndex, T> innerDic;
            if (dic.TryGetValue(code, out innerDic) && innerDic.TryGetValue(fieldIndex, out result))
                return result;

            return result;
        }

        /// <summary>
        /// 返回股票代码在指定FieldIndex上的文字输出颜色
        /// </summary>
        /// <param name="code">股票代码（包含市场代码，例如：600000.SH）</param>
        /// <param name="index">字段</param>
        /// <returns>画刷</returns>
        public static SolidBrush GetBrush(int code, FieldIndex index)
        {
            SolidBrush brush = QuoteDrawService.BrushColorNormal;
            switch (index)
            {
                case FieldIndex.Date:
                    break;
                case FieldIndex.Time:
                    break;
                case FieldIndex.LastVolume:
                    break;
                case FieldIndex.Evg5Volume:
                    break;
                case FieldIndex.GreenVolume:
                    brush = QuoteDrawService.BrushColorDown;
                    break;
                case FieldIndex.RedVolume:
                    brush = QuoteDrawService.BrushColorUp;
                    break;
                case FieldIndex.IndexAmount:
                    brush = QuoteDrawService.BrushColorAmount;
                    break;
                case FieldIndex.IndexVolume:
                    brush = QuoteDrawService.BrushColorVolumn;
                    break;
                case FieldIndex.UpNum:
                    brush = QuoteDrawService.BrushColorUp;
                    break;
                case FieldIndex.EqualNum:
                    brush = QuoteDrawService.BrushColorSame;
                    break;
                case FieldIndex.DownNum:
                    brush = QuoteDrawService.BrushColorDown;
                    break;
                case FieldIndex.SumBuyVolume:
                    break;
                case FieldIndex.SumSellVolume:
                    break;
                case FieldIndex.PreOpenInterest:
                    break;
                case FieldIndex.OpenInterest:
                    break;
                case FieldIndex.Amount:
                    brush = QuoteDrawService.BrushColorAmount;
                    break;
                case FieldIndex.Volume:
                case FieldIndex.VolumeRatio:
                case FieldIndex.Turnover:
                case FieldIndex.MGJZC:
                case FieldIndex.MGSY:
                case FieldIndex.PETTM:
                case FieldIndex.ZGB:
                case FieldIndex.ZSZ:
                case FieldIndex.NetAShare:
                case FieldIndex.LTSZ:
                    brush = QuoteDrawService.BrushColorInfopanelNormalWhite;
                    break;
                case FieldIndex.ZZC:
                case FieldIndex.Ltg:
                case FieldIndex.PE:
                    brush = QuoteDrawService.BrushKlineButtonTextSelectColor;
                    break;

                case FieldIndex.Turnover3D:
                case FieldIndex.Turnover6D:

                case FieldIndex.SellVolume10:
                case FieldIndex.SellVolume9:
                case FieldIndex.SellVolume8:
                case FieldIndex.SellVolume7:
                case FieldIndex.SellVolume6:
                case FieldIndex.SellVolume5:
                case FieldIndex.SellVolume4:
                case FieldIndex.SellVolume3:
                case FieldIndex.SellVolume2:
                case FieldIndex.SellVolume1:
                case FieldIndex.BuyVolume1:
                case FieldIndex.BuyVolume2:
                case FieldIndex.BuyVolume3:
                case FieldIndex.BuyVolume4:
                case FieldIndex.BuyVolume5:
                case FieldIndex.BuyVolume6:
                case FieldIndex.BuyVolume7:
                case FieldIndex.BuyVolume8:
                case FieldIndex.BuyVolume9:
                case FieldIndex.BuyVolume10:
                    brush = QuoteDrawService.BrushColorVolumn;
                    break;
                case FieldIndex.Market:
                    break;
                case FieldIndex.Open:
                case FieldIndex.High:
                case FieldIndex.Low:
                case FieldIndex.Now:
                case FieldIndex.AveragePrice:
                case FieldIndex.SellPrice10:
                case FieldIndex.SellPrice9:
                case FieldIndex.SellPrice8:
                case FieldIndex.SellPrice7:
                case FieldIndex.SellPrice6:
                case FieldIndex.SellPrice5:
                case FieldIndex.SellPrice4:
                case FieldIndex.SellPrice3:
                case FieldIndex.SellPrice2:
                case FieldIndex.SellPrice1:
                case FieldIndex.BuyPrice1:
                case FieldIndex.BuyPrice2:
                case FieldIndex.BuyPrice3:
                case FieldIndex.BuyPrice4:
                case FieldIndex.BuyPrice5:
                case FieldIndex.BuyPrice6:
                case FieldIndex.BuyPrice7:
                case FieldIndex.BuyPrice8:
                case FieldIndex.BuyPrice9:
                case FieldIndex.BuyPrice10:
                    {
                        MarketType market = GetMarketByCode(code);//(MarketType)_Dc.GetDetailData(code, FieldIndex.Market);
                        object objPreClose;
                        switch (market)
                        {
                            case MarketType.IF:
                            case MarketType.GoverFutures:
                            case MarketType.SHF:
                            case MarketType.CHFAG:
                            case MarketType.CHFCU:
                            case MarketType.CZC:
                            case MarketType.DCE:
                            case MarketType.OSFutures:
                            case MarketType.OSFuturesCBOT:
                            case MarketType.OSFuturesSGX:
                            case MarketType.OSFuturesLMEElec:
                            case MarketType.OSFuturesLMEVenue:
                                {
                                    objPreClose = GetObjectValue(code, FieldIndex.PreSettlementPrice);
                                    break;
                                }
                            default:
                                {
                                    objPreClose = GetObjectValue(code, FieldIndex.PreClose);
                                    break;
                                }
                        }

                        object objIndex = GetObjectValue(code, index);
                        if (objIndex == null || objPreClose == null)
                            break;
                        float preClose = Convert.ToSingle(objPreClose);
                        float price = Convert.ToSingle(objIndex);
                        if (price > preClose)
                        {
                            brush = QuoteDrawService.BrushColorUp;
                        }
                        else if (price == preClose)
                        {
                            brush = QuoteDrawService.BrushColorSame;
                        }
                        else if (price < preClose && price > 0)
                        {
                            brush = QuoteDrawService.BrushColorDown;
                        }
                        else
                        {
                            brush = QuoteDrawService.BrushColorNormal;
                        }
                        break;
                    }
                case FieldIndex.SellPriceYtm10:
                case FieldIndex.SellPriceYtm9:
                case FieldIndex.SellPriceYtm8:
                case FieldIndex.SellPriceYtm7:
                case FieldIndex.SellPriceYtm6:
                case FieldIndex.SellPriceYtm5:
                case FieldIndex.SellPriceYtm4:
                case FieldIndex.SellPriceYtm3:
                case FieldIndex.SellPriceYtm2:
                case FieldIndex.SellPriceYtm1:
                case FieldIndex.BuyPriceYtm1:
                case FieldIndex.BuyPriceYtm2:
                case FieldIndex.BuyPriceYtm3:
                case FieldIndex.BuyPriceYtm4:
                case FieldIndex.BuyPriceYtm5:
                case FieldIndex.BuyPriceYtm6:
                case FieldIndex.BuyPriceYtm7:
                case FieldIndex.BuyPriceYtm8:
                case FieldIndex.BuyPriceYtm9:
                case FieldIndex.BuyPriceYtm10:
                    {
                        MarketType market = GetMarketByCode(code);//(MarketType)_Dc.GetDetailData(code, FieldIndex.Market);
                        object objPreClose;
                        switch (market)
                        {
                            case MarketType.IF:
                            case MarketType.SHF:
                            case MarketType.CZC:
                            case MarketType.DCE:
                            case MarketType.OSFutures:
                            case MarketType.OSFuturesCBOT:
                            case MarketType.OSFuturesSGX:
                            case MarketType.CHFAG:
                            case MarketType.CHFCU:
                                {
                                    objPreClose = GetObjectValue(code, FieldIndex.PreSettlementPrice);
                                    break;
                                }
                            case MarketType.SHConvertBondLev1:
                            case MarketType.SHConvertBondLev2:
                            case MarketType.SHNonConvertBondLev1:
                            case MarketType.SHNonConvertBondLev2:
                            case MarketType.SZConvertBondLev1:
                            case MarketType.SZConvertBondLev2:
                            case MarketType.SZNonConvertBondLev1:
                            case MarketType.SZNonConvertBondLev2:
                                {
                                    objPreClose = GetObjectValue(code, FieldIndex.BondDecLcytm);
                                    break;
                                }
                            default:
                                {
                                    objPreClose = GetObjectValue(code, FieldIndex.PreClose);
                                    break;
                                }
                        }
                        object objIndex = GetObjectValue(code, index);
                        if (objIndex == null || objPreClose == null)
                            break;
                        float preClose = Convert.ToSingle(objPreClose);
                        try
                        {
                            float price = Convert.ToSingle(objIndex);



                            if (price > preClose)
                            {
                                brush = QuoteDrawService.BrushColorUp;
                            }
                            else if (price == preClose)
                            {
                                brush = QuoteDrawService.BrushColorSame;
                            }
                            else if (price < preClose)
                            {
                                brush = QuoteDrawService.BrushColorDown;
                            }
                            else
                            {
                                brush = QuoteDrawService.BrushColorNormal;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                case FieldIndex.DDXRedFollowDay:
                case FieldIndex.DDXRedFollowDay5:
                case FieldIndex.DDXRedFollowDay10:
                case FieldIndex.NetFlowRangeSuper:
                case FieldIndex.NetFlowRangeBig:
                case FieldIndex.NetFlowRangeMiddle:
                case FieldIndex.NetFlowRangeSmall:
                case FieldIndex.BuyFlowRangeSuper:
                case FieldIndex.BuyFlowRangeBig:
                case FieldIndex.SellFlowRangeSuper:
                case FieldIndex.SellFlowRangeBig:
                case FieldIndex.BuySuper:
                case FieldIndex.SellSuper:
                case FieldIndex.NetFlowSuper:
                case FieldIndex.BuyBig:
                case FieldIndex.SellBig:
                case FieldIndex.NetFlowBig:
                case FieldIndex.BuyMiddle:
                case FieldIndex.SellMiddle:
                case FieldIndex.NetFlowMiddle:
                case FieldIndex.BuySmall:
                case FieldIndex.SellSmall:
                case FieldIndex.NetFlowSmall:

                case FieldIndex.MainNetFlow:
                case FieldIndex.Weibi:
                case FieldIndex.Weicha:
                case FieldIndex.DifferSpeed:
                case FieldIndex.Difference:
                case FieldIndex.DifferRange:
                case FieldIndex.DifferRange5Mint:
                case FieldIndex.DifferRange3D:
                case FieldIndex.DifferRange5D:
                case FieldIndex.DifferRange6D:
                case FieldIndex.DifferRange60D:
                case FieldIndex.DifferRange10D:
                case FieldIndex.DifferRange20D:
                case FieldIndex.DifferRangeYTD:
                case FieldIndex.LowW52:
                case FieldIndex.HighW52:
                case FieldIndex.NetFlow:
                case FieldIndex.UpDay:
                case FieldIndex.DDX:
                case FieldIndex.DDY:
                case FieldIndex.DDZ:
                case FieldIndex.DDX5:
                case FieldIndex.DDY5:
                case FieldIndex.DDX10:
                case FieldIndex.DDY10:
                case FieldIndex.ZengCangRange:
                case FieldIndex.ZengCangRankChange:
                case FieldIndex.ZengCangRangeDay3:
                case FieldIndex.ZengCangRankChangeDay3:
                case FieldIndex.ZengCangRangeDay5:
                case FieldIndex.ZengCangRankChangeDay5:
                case FieldIndex.ZengCangRangeDay10:
                case FieldIndex.ZengCangRankChangeDay10:
                case FieldIndex.BuyVolume10Delta:
                case FieldIndex.BuyVolume9Delta:
                case FieldIndex.BuyVolume8Delta:
                case FieldIndex.BuyVolume7Delta:
                case FieldIndex.BuyVolume6Delta:
                case FieldIndex.BuyVolume5Delta:
                case FieldIndex.BuyVolume4Delta:
                case FieldIndex.BuyVolume3Delta:
                case FieldIndex.BuyVolume2Delta:
                case FieldIndex.BuyVolume1Delta:
                case FieldIndex.SellVolume10Delta:
                case FieldIndex.SellVolume9Delta:
                case FieldIndex.SellVolume8Delta:
                case FieldIndex.SellVolume7Delta:
                case FieldIndex.SellVolume6Delta:
                case FieldIndex.SellVolume5Delta:
                case FieldIndex.SellVolume4Delta:
                case FieldIndex.SellVolume3Delta:
                case FieldIndex.SellVolume2Delta:
                case FieldIndex.SellVolume1Delta:
                    {
                        float indexValue = Convert.ToSingle(GetObjectValue(code, index));
                        if (indexValue > 0)
                        {
                            brush = QuoteDrawService.BrushColorUp;
                        }
                        else if (indexValue == 0)
                        {
                            brush = QuoteDrawService.BrushColorSame;
                        }
                        else if (indexValue < 0)
                        {
                            brush = QuoteDrawService.BrushColorDown;
                        }
                        break;
                    }
                case FieldIndex.PreClose:
                    brush = QuoteDrawService.BrushColorNormal;
                    break;
                case FieldIndex.Delta:
                    break;
                case FieldIndex.UpLimit:
                    brush = QuoteDrawService.BrushColorUp;
                    break;
                case FieldIndex.DownLimit:
                    brush = QuoteDrawService.BrushColorDown;
                    break;
                case FieldIndex.NeiWaiBi:
                    brush = QuoteDrawService.BrushColorUp;
                    break;
                case FieldIndex.Code:
                    brush = QuoteDrawService.BrushColorCode;
                    break;
                case FieldIndex.Name:
                    brush = QuoteDrawService.BrushColorName;
                    break;
                case FieldIndex.SecuCode:
                    break;
                case FieldIndex.ChiSpelling:
                    break;
                case FieldIndex.SerialNumber:
                    break;
                case FieldIndex.AvgNetS:
                case FieldIndex.ProfitFO:
                case FieldIndex.InvIncome:
                case FieldIndex.PBTax:
                case FieldIndex.RProfotAA:
                case FieldIndex.FixAsset:
                case FieldIndex.IntAsset:
                case FieldIndex.TCurLiab:
                case FieldIndex.TLongLiab:
                case FieldIndex.Hshare:


                case FieldIndex.ZYYWSR:
                case FieldIndex.ZYYWLR:
                case FieldIndex.DRPRPAA:


                case FieldIndex.TotalLiab:
                case FieldIndex.OWnerEqu:
                case FieldIndex.CapRes:
                case FieldIndex.DRPCapRes:
                case FieldIndex.NetBShare:
                    brush = QuoteDrawService.BrushColorVolumn;
                    break;
                case FieldIndex.Na:
                    break;

                case FieldIndex.ZengCangRank:
                case FieldIndex.ZengCangRankDay3:
                case FieldIndex.ZengCangRankDay5:
                case FieldIndex.ZengCangRankDay10:
                    brush = QuoteDrawService.BrushColorVolumn;
                    break;

                #region 股指

                case FieldIndex.PreSettlementPrice:
                case FieldIndex.SettlementPrice:
                    brush = QuoteDrawService.BrushColorNormal;
                    break;
                case FieldIndex.OpenCloseStatus:
                    int flag = Convert.ToInt32(GetObjectValue(code, FieldIndex.BSFlag));
                    if (flag == 1)
                    {
                        brush = QuoteDrawService.BrushColorDown;
                    }
                    else
                    {
                        brush = QuoteDrawService.BrushColorUp;
                    }
                    break;
                #endregion

                #region 基金理财

                case FieldIndex.FundLatestDate:
                case FieldIndex.FundFounddate:
                case FieldIndex.FundEnddate:
                    brush = QuoteDrawService.BrushColorInfopanelNormalWhite;
                    break;
                case FieldIndex.FundPernav:
                    {
                        float result = Convert.ToSingle(GetObjectValue(code, FieldIndex.FundDecZdf));
                        if (result > 0)
                        {
                            brush = QuoteDrawService.BrushColorUp;
                        }
                        else if (result < 0)
                        {
                            brush = QuoteDrawService.BrushColorDown;
                        }
                        else
                        {
                            brush = QuoteDrawService.BrushColorSame;
                        }
                        break;
                    }
                case FieldIndex.FundIncomeYear:
                case FieldIndex.FundIncomeYear1:
                case FieldIndex.FundIncomeYear2:
                case FieldIndex.FundIncomeYear3:
                case FieldIndex.FundIncomeYear4:
                case FieldIndex.FundIncomeYear5:
                case FieldIndex.FundIncomeYear6:
                case FieldIndex.FundIncomeRankYear1:
                case FieldIndex.FundIncomeRankYear2:
                case FieldIndex.FundIncomeRankYear3:
                case FieldIndex.FundIncomeRankYear4:
                case FieldIndex.FundIncomeRankYear5:
                case FieldIndex.FundIncomeRankYear6:
                case FieldIndex.FundIncome10K:
                case FieldIndex.FundIncomeYear7D:
                case FieldIndex.FundDecZdf:
                case FieldIndex.FundNvgr4w:
                case FieldIndex.FundNvgr13w:
                case FieldIndex.FundNvgr26w:
                case FieldIndex.FundNvgr52w:
                case FieldIndex.FundNvgr156w:
                case FieldIndex.FundNvgr208w:
                case FieldIndex.FundNvgrf:
                    {
                        float result = Convert.ToSingle(GetObjectValue(code, index));
                        if (result > 0)
                        {
                            brush = QuoteDrawService.BrushColorUp;
                        }
                        else if (result < 0)
                        {
                            brush = QuoteDrawService.BrushColorDown;
                        }
                        else
                        {
                            brush = QuoteDrawService.BrushColorSame;
                        }
                        break;
                    }
                case FieldIndex.FundParaname:
                case FieldIndex.FundStrSgshzt:
                case FieldIndex.FundFmanager:
                case FieldIndex.FundManagername:
                case FieldIndex.FundStrTgrcom:
                    brush = QuoteDrawService.BrushColorInfopanelNormalWhite;
                    break;

                #endregion

                #region 债券利率

                case FieldIndex.RateDecAvgPrice:
                case FieldIndex.RatePreDecPrice:
                case FieldIndex.RateAvgPrice5D:
                case FieldIndex.RateAvgPrice10D:
                case FieldIndex.RateAvgPrice20D:
                case FieldIndex.RateAvgPrice60D:
                case FieldIndex.RateAvgPrice120D:
                case FieldIndex.RateAvgPrice250D:
                    float rateresult = Convert.ToSingle(GetObjectValue(code, index));
                    if (rateresult > 0)
                    {
                        brush = QuoteDrawService.BrushColorUp;
                    }
                    if (rateresult < 0)
                    {
                        brush = QuoteDrawService.BrushColorDown;
                    }
                    break;
                case FieldIndex.BondNetOpen:
                case FieldIndex.BondNetHigh:
                case FieldIndex.BondNetLow:
                case FieldIndex.BondCBNet:
                case FieldIndex.BondCBYtm:
                case FieldIndex.BondCSNet:
                case FieldIndex.BondCSYtm:
                case FieldIndex.BondSNNet:
                case FieldIndex.BondSNYtm:
                case FieldIndex.BondNetNow:
                case FieldIndex.BondFullNow:
                case FieldIndex.BondFullOpen:
                case FieldIndex.BondFullHigh:
                case FieldIndex.BondFullLow:
                case FieldIndex.BondBestSellNet:
                case FieldIndex.BondBestSellYtm:
                case FieldIndex.BondBestSellTotalFaceValue:
                case FieldIndex.BondBestBuyNet:
                case FieldIndex.BondBestBuyYtm:
                case FieldIndex.BondBestBuyTotalFaceValue:
                    {
                        float rateprice = Convert.ToSingle(GetObjectValue(code, index));
                        float preprice = Convert.ToSingle(GetObjectValue(code, FieldIndex.PreClose));
                        if (rateprice > preprice)
                        {
                            brush = QuoteDrawService.BrushColorUp;
                        }
                        else if (rateprice < preprice)
                        {
                            brush = QuoteDrawService.BrushColorDown;
                        }
                        else
                        {
                            brush = QuoteDrawService.BrushColorSame;
                        }
                    }
                    break;
                case FieldIndex.BondYTMOpen:
                case FieldIndex.BondYTMHigh:
                case FieldIndex.BondYTMAvg:
                case FieldIndex.BondYTMLow:
                    {
                        float rateprice = Convert.ToSingle(GetObjectValue(code, index));
                        float preprice = Convert.ToSingle(GetObjectValue(code, FieldIndex.BondDecLcytm));
                        if (rateprice > preprice)
                        {
                            brush = QuoteDrawService.BrushColorUp;
                        }
                        else if (rateprice < preprice)
                        {
                            brush = QuoteDrawService.BrushColorDown;
                        }
                        else
                        {
                            brush = QuoteDrawService.BrushColorSame;
                        }
                    }
                    break;
                case FieldIndex.YTMAndBP:
                    float bp = Convert.ToSingle(GetObjectValue(code, FieldIndex.BondDiffRangeYTM));
                    if (bp > 0)
                        brush = QuoteDrawService.BrushColorUp;
                    else if (bp == 0)
                        brush = QuoteDrawService.BrushColorSame;
                    else
                        brush = QuoteDrawService.BrushColorDown;
                    break;
                case FieldIndex.BondDuration:
                case FieldIndex.BondConvexity:
                case FieldIndex.BondStrZqpj:
                case FieldIndex.BondStrZtpj:
                case FieldIndex.BondBondperiod:
                case FieldIndex.BondTomrtyyear:
                case FieldIndex.BondNewrate:
                case FieldIndex.BondType:
                case FieldIndex.BondAI:
                case FieldIndex.BondIsVouch:
                case FieldIndex.BondStrTstk:
                case FieldIndex.BondExerciseDay:
                case FieldIndex.ZQYE:
                case FieldIndex.BondConversionRate:
                case FieldIndex.BondSwapPrice:
                case FieldIndex.CZJZ:
                case FieldIndex.ZGJZ:
                case FieldIndex.CZYJL:
                case FieldIndex.ZGYJL:
                case FieldIndex.BondTradeMarket:
                    brush = QuoteDrawService.BrushColorSame;
                    break;
                #endregion

                default:
                    brush = QuoteDrawService.BrushColorNormal;
                    break;
            }

            return brush;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReadFieldInfo
    {
        private static DataCenterCore _dc;

        static ReadFieldInfo()
        {
            _dc = DataCenterCore.CreateInstance();
        }

        /// <summary>
        /// 获得界面字段的格式化字符串以及颜色画刷
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <param name="name">界面配置实用的字段别名</param>
        /// <param name="formattedValue">返回的格式化字符串</param>
        /// <param name="brush">返回的颜色画刷</param>
        public static void GetFieldInfo(int code, string name,
            out string formattedValue, out SolidBrush brush)
        {
            formattedValue = string.Empty;
            brush = QuoteDrawService.BrushColorCode;
            FieldInfo fieldInfo = GetFieldInfoByName(code, name);
            if (fieldInfo == null)
                return;
            formattedValue = GetFieldValueByFieldInfo(code, fieldInfo.FieldIndex, fieldInfo.Format);
            if (formattedValue.Equals(ZeroRule.HorStr))
                brush = QuoteDrawService.BrushColorCode;
            else
                brush = GetFieldBrushByFieldInfo(code, fieldInfo);
        }
        /// <summary>
        /// 获得界面字段的格式化字符串
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <param name="name">界面配置实用的字段别名</param>   
        public static string GetFieldValue(int code, string name)
        {
            string formattedValue = string.Empty;
            FieldInfo fieldInfo = GetFieldInfoByName(code, name);
            formattedValue = GetFieldValueByFieldInfo(code, fieldInfo.FieldIndex, fieldInfo.Format);

            return formattedValue;
        }

         
        /// <summary>
        /// 获得界面字段的导出时的格式化字符串
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <param name="name">界面配置实用的字段别名</param>
        /// <param name="exportValue">返回导出的值</param>
        /// <param name="type">返回导出的值的类型</param>
        /// <returns>是否成功</returns>
        public static bool TryGetFieldExportValue(int code, string name,
            out object exportValue, out Type type)
        {
            bool success = false;
            string formattedExportValue = string.Empty;
            FieldInfo fieldInfo = GetFieldInfoByName(code, name);
            formattedExportValue = GetFieldValueByFieldInfo(code, fieldInfo.FieldIndex, fieldInfo.ExportFormat);
            type = fieldInfo.ExportType;


            switch (type.FullName)
            {
                case "System.Int32":
                    Int32 intDefault = default(Int32);
                    success = Int32.TryParse(formattedExportValue, out intDefault);
                    exportValue = intDefault;                     
                    break;

                case "System.Single":
                    Single singleDefault = default(Single);
                    success = Single.TryParse(formattedExportValue, out singleDefault);
                    exportValue = singleDefault;
                    break;

                case "System.Double":
                    Double doubleDefault = default(Double);
                    success = Double.TryParse(formattedExportValue, out doubleDefault);
                    exportValue = doubleDefault;
                    break;

                case "System.Int64":
                    Int64 int64Default = default(Int64);
                    success = Int64.TryParse(formattedExportValue, out int64Default);
                    exportValue = int64Default;
                    break;

                case "System.String":
                    exportValue = formattedExportValue;
                    success = true;
                    break;

                default:
                    exportValue = formattedExportValue;
                    success = true;
                    break;
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static FieldInfo GetFieldInfoByName(int code, string fieldName)
        {
            MarketType market = FieldInfoHelper.GetMarketByCode(code);
            return FieldInfoHelper.GetFieldInfoByName(market, fieldName);
        }

        /// <summary>
        /// 获得界面字段的格式化字符串
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <param name="fieldIndex">FieldIndex</param>
        /// <param name="format">格式</param>
        /// <returns>格式化后的字符串</returns>
        private static string GetFieldValueByFieldInfo(int code, FieldIndex fieldIndex, Format format)
        {
            string result = string.Empty;

            if (format == null)
                return string.Empty;

            float fieldIndexValue = 0.0f;
            if (format.ZeroRule != null
                && format.ZeroRule.CompareTarget == CompareTarget.SpecialFieldIndex)
            {
                FieldIndex index = format.ZeroRule.ComparedIndex;
                fieldIndexValue = Convert.ToSingle(FieldInfoHelper.GetObjectValue(code, index));
            }

            int fieldIndexSeq = (short)fieldIndex;

            if (fieldIndexSeq < 300)
                result = FieldInfoHelper.GetFieldFormattedValue(code, fieldIndex,
                    format, fieldIndexValue, DetailData.FieldIndexDataInt32);
            else if (fieldIndexSeq < 800)
                result = FieldInfoHelper.GetFieldFormattedValue(code, fieldIndex,
                    format, fieldIndexValue, DetailData.FieldIndexDataSingle);
            else if (fieldIndexSeq < 1000)
                result = FieldInfoHelper.GetFieldFormattedValue(code, fieldIndex,
                    format, fieldIndexValue, DetailData.FieldIndexDataDouble);
            else if (fieldIndexSeq < 1200)
                result = FieldInfoHelper.GetFieldFormattedValue(code, fieldIndex,
                    format, fieldIndexValue, DetailData.FieldIndexDataInt64);
            else if (fieldIndexSeq < 9000)
                result = FieldInfoHelper.GetFieldFormattedValue(code, fieldIndex,
                    format, fieldIndexValue, DetailData.FieldIndexDataString);
            else
                result = FieldInfoHelper.GetFieldFormattedValue(code, fieldIndex,
                    format, fieldIndexValue, DetailData.FieldIndexDataObject);

            return result;

        }

        #region 画刷
        private static SolidBrush GetFieldBrushByFieldInfo(int code, FieldInfo fieldInfo)
        {
            FieldIndex field = fieldInfo.FieldIndex;
            if (field >= 0 && (int)field <= 299)
            {
                int data = _dc.GetFieldDataInt32(code, field);
                return GetFieldBrushByFieldInfo(code, fieldInfo, data);
            }
            if ((int)field >= 300 && (int)field <= 799)
            {
                float data = _dc.GetFieldDataSingle(code, field);
                return GetFieldBrushByFieldInfo(code, fieldInfo, data);
            }
            if ((int)field >= 800 && (int)field <= 999)
            {
                double data = _dc.GetFieldDataDouble(code, field);
                return GetFieldBrushByFieldInfo(code, fieldInfo, data);
            }
            if ((int)field >= 1000 && (int)field <= 1199)
            {
                long data = _dc.GetFieldDataInt64(code, field);
                return GetFieldBrushByFieldInfo(code, fieldInfo, data);
            }
            if ((int)field >= 1200 && (int)field <= 8999)
            {
                string data = _dc.GetFieldDataString(code, field);
                return GetFieldBrushByFieldInfo(code, fieldInfo, data);
            }
            else
                return QuoteDrawService.BrushColorCode;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static SolidBrush GetFieldBrushByFieldInfo(int code, FieldInfo fieldInfo, int data)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorCode;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                    case "Name":
                        return QuoteDrawService.BrushColorName;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2)
            {
                if (fieldInfo.ColorSetting[0].Equals("$"))
                {
                    switch (fieldInfo.ColorSetting[1])
                    {
                        case "0":
                            if (data > 0)
                                return QuoteDrawService.BrushColorUp;
                            if (data == 0)
                                return QuoteDrawService.BrushColorSame;
                            if (data < 0)
                                return QuoteDrawService.BrushColorDown;
                            break;
                        default:
                            FieldIndex field = (FieldIndex)Enum.Parse(typeof(FieldIndex), fieldInfo.ColorSetting[1]);

                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }

                            break;
                    }
                }
                else if (fieldInfo.ColorSetting[0].Equals("#"))
                {
                    string aimName = fieldInfo.ColorSetting[1];
                    FieldInfo aimField = GetFieldInfoByName(code, aimName);
                    return GetFieldBrushByFieldInfo(code, aimField);
                }
            }
            return QuoteDrawService.BrushColorCode;
        }

        private static SolidBrush GetFieldBrushByFieldInfo(int code, FieldInfo fieldInfo, float data)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorCode;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                    case "Name":
                        return QuoteDrawService.BrushColorName;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2)
            {
                if (fieldInfo.ColorSetting[0].Equals("$"))
                {
                    switch (fieldInfo.ColorSetting[1])
                    {
                        case "0":
                            if (data > 0)
                                return QuoteDrawService.BrushColorUp;
                            if (data == 0)
                                return QuoteDrawService.BrushColorSame;
                            if (data < 0)
                                return QuoteDrawService.BrushColorDown;
                            break;
                        default:
                            FieldIndex field = (FieldIndex)Enum.Parse(typeof(FieldIndex), fieldInfo.ColorSetting[1]);

                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }

                            break;
                    }
                }
                else if (fieldInfo.ColorSetting[0].Equals("#"))
                {
                    string aimName = fieldInfo.ColorSetting[1];
                    FieldInfo aimField = GetFieldInfoByName(code, aimName);
                    return GetFieldBrushByFieldInfo(code, aimField);
                }
            }
            return QuoteDrawService.BrushColorCode;
        }

        private static SolidBrush GetFieldBrushByFieldInfo(int code, FieldInfo fieldInfo, double data)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorCode;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                    case "Name":
                        return QuoteDrawService.BrushColorName;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2)
            {
                if (fieldInfo.ColorSetting[0].Equals("$"))
                {
                    switch (fieldInfo.ColorSetting[1])
                    {
                        case "0":
                            if (data > 0)
                                return QuoteDrawService.BrushColorUp;
                            if (data == 0)
                                return QuoteDrawService.BrushColorSame;
                            if (data < 0)
                                return QuoteDrawService.BrushColorDown;
                            break;
                        default:
                            FieldIndex field = (FieldIndex)Enum.Parse(typeof(FieldIndex), fieldInfo.ColorSetting[1]);

                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }

                            break;
                    }
                }
                else if (fieldInfo.ColorSetting[0].Equals("#"))
                {
                    string aimName = fieldInfo.ColorSetting[1];
                    FieldInfo aimField = GetFieldInfoByName(code, aimName);
                    return GetFieldBrushByFieldInfo(code, aimField);
                }
            }
            return QuoteDrawService.BrushColorCode;
        }

        private static SolidBrush GetFieldBrushByFieldInfo(int code, FieldInfo fieldInfo, long data)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorCode;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                    case "Name":
                        return QuoteDrawService.BrushColorName;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2)
            {
                if (fieldInfo.ColorSetting[0].Equals("$"))
                {
                    switch (fieldInfo.ColorSetting[1])
                    {
                        case "0":
                            if (data > 0)
                                return QuoteDrawService.BrushColorUp;
                            if (data == 0)
                                return QuoteDrawService.BrushColorSame;
                            if (data < 0)
                                return QuoteDrawService.BrushColorDown;
                            break;
                        default:
                            FieldIndex field = (FieldIndex)Enum.Parse(typeof(FieldIndex), fieldInfo.ColorSetting[1]);

                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }

                            break;
                    }
                }
                else if (fieldInfo.ColorSetting[0].Equals("#"))
                {
                    string aimName = fieldInfo.ColorSetting[1];
                    FieldInfo aimField = GetFieldInfoByName(code, aimName);
                    return GetFieldBrushByFieldInfo(code, aimField);
                }
            }
            return QuoteDrawService.BrushColorCode;
        }

        private static SolidBrush GetFieldBrushByFieldInfo(int code, FieldInfo fieldInfo, string data)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorCode;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                    case "Name":
                        return QuoteDrawService.BrushColorName;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2)
            {
                if (fieldInfo.ColorSetting[0].Equals("$"))
                {
                    double dData;
                    if (double.TryParse(data, out dData))
                    {
                        switch (fieldInfo.ColorSetting[1])
                        {
                            case "0":
                                if (dData > 0)
                                    return QuoteDrawService.BrushColorUp;
                                if (dData == 0)
                                    return QuoteDrawService.BrushColorSame;
                                if (dData < 0)
                                    return QuoteDrawService.BrushColorDown;
                                break;
                            default:
                                FieldIndex field = (FieldIndex)Enum.Parse(typeof(FieldIndex), fieldInfo.ColorSetting[1]);

                                if (field >= 0 && (int)field <= 299)
                                {
                                    int compareData = _dc.GetFieldDataInt32(code, field);
                                    if (dData > compareData)
                                        return QuoteDrawService.BrushColorUp;
                                    if (dData == compareData)
                                        return QuoteDrawService.BrushColorSame;
                                    if (dData < compareData)
                                        return QuoteDrawService.BrushColorDown;
                                }
                                if ((int)field >= 300 && (int)field <= 799)
                                {
                                    float compareData = _dc.GetFieldDataSingle(code, field);
                                    if (dData > compareData)
                                        return QuoteDrawService.BrushColorUp;
                                    if (dData == compareData)
                                        return QuoteDrawService.BrushColorSame;
                                    if (dData < compareData)
                                        return QuoteDrawService.BrushColorDown;
                                }
                                if ((int)field >= 800 && (int)field <= 999)
                                {
                                    double compareData = _dc.GetFieldDataDouble(code, field);
                                    if (dData > compareData)
                                        return QuoteDrawService.BrushColorUp;
                                    if (dData == compareData)
                                        return QuoteDrawService.BrushColorSame;
                                    if (dData < compareData)
                                        return QuoteDrawService.BrushColorDown;
                                }
                                if ((int)field >= 1000 && (int)field <= 1199)
                                {
                                    long compareData = _dc.GetFieldDataInt64(code, field);
                                    if (dData > compareData)
                                        return QuoteDrawService.BrushColorUp;
                                    if (dData == compareData)
                                        return QuoteDrawService.BrushColorSame;
                                    if (dData < compareData)
                                        return QuoteDrawService.BrushColorDown;
                                }

                                break;
                        }
                    }
                }
                else if (fieldInfo.ColorSetting[0].Equals("#"))
                {
                    string aimName = fieldInfo.ColorSetting[1];
                    FieldInfo aimField = GetFieldInfoByName(code, aimName);
                    return GetFieldBrushByFieldInfo(code, aimField);
                }
            }
            return QuoteDrawService.BrushColorCode;
        }
        #endregion
    }
}
