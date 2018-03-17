using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace OwLib
{
    /// <summary>
    /// 提供K线相关数据的接口
    /// </summary>
    public class GetKlineData
    {
        #region 常量和静态数组
        /// <summary>
        /// DataCenter 层的一个单例
        /// </summary>
        private static readonly DataCenterCore Dc = DataCenterCore.CreateInstance();
        /// <summary>
        /// 默认指标颜色数组
        /// </summary>
        private static readonly Color[] IndicatorColors =
        {
            Color.White,
            Color.Yellow,
            Color.FromArgb(255, 0, 255),
            Color.FromArgb(0, 255, 0),
            Color.FromArgb(192, 192, 192),
            Color.FromArgb(2, 226, 244),
            Color.FromArgb(255, 255, 154),
            Color.FromArgb(255, 50, 50),
            Color.FromArgb(174, 251, 174),
            Color.FromArgb(173, 216, 30)
        }; 
        #endregion

        #region 对外提供的接口
        /// <summary>
        /// 获取K线数据
        /// </summary>
        /// <param name="code">股票代码（内码）</param>
        /// <param name="cycle">请求周期</param>
        /// <param name="market">市场类型</param>
        /// <param name="divideType">复权类型</param>
        /// <returns>K线数据列表</returns>
        public static List<OneDayDataRec> GetKlineDataAfterDeal(int code, MarketType market,
            KLineCycle cycle, IsDivideRightType divideType)
        {
            List<OneDayDataRec> result;
            float[] devides;
            bool isCycleYear = cycle == KLineCycle.CycleYear;
            bool isForward = divideType == IsDivideRightType.Forward;
            if (SecurityAttribute.FundFinancingTypeList.Contains(market))
            {
                if (divideType == IsDivideRightType.Backward)// 基金K线也需要自己处理复权吗？
                {
                    // 1、获得原始数据
                    result = GetFundFinancingBackwardKLineData(code, cycle);
                    // 2、获得复权因子
                    devides = CaculateDivideKLineData(code, result, isCycleYear, isForward);
                    // 3、计算 
                    GetFinallyCalculDevidedKLine(devides, result, divideType);
                }
                else
                {
                    // 1、获得原始数据
                    result = GetNormalKLineData(code, cycle);
                    // 2、获得复权因子
                    devides = CaculateDivideKLineData(code, result, isCycleYear, isForward);
                    // 3、计算 
                    GetFinallyCalculDevidedKLine(devides, result, divideType);
                }
            }
            else
            {
                if (divideType != IsDivideRightType.Non
                    && SecurityAttribute.HasDivideRightTypeList.Contains(market)
                    && (int)cycle > (int)KLineCycle.CycleDay)
                {
                    result = GetDevidedKLineData(code, market, cycle, divideType);
                }
                else
                {
                    if (divideType == IsDivideRightType.Non)
                    {
                        result = GetNormalKLineData(code, cycle);
                    }
                    else// 日线及以下周期的复权状态
                    {
                        // 1、获得原始数据
                        result = GetNormalKLineData(code, cycle);
                        // 2、获得复权因子
                        devides = CaculateDivideKLineData(code, result, isCycleYear, isForward);
                        // 3、计算 
                        GetFinallyCalculDevidedKLine(devides, result, divideType);
                    }
                }
            }

            return result;
        }
        
        /// <summary>
        /// 获得指标数据
        /// </summary>
        /// <param name="code">股票代码（内码）</param>
        /// <param name="cycle">请求周期</param>
        /// <param name="market">请求市场类型</param>
        /// <param name="DivideType">请求的除复权类型</param>
        /// <param name="indicator">请求指标类型</param>
        /// <param name="kLineDataList">K线基础数据</param>
        /// <param name="IndexCycle">指标周期</param>
        /// <param name="trendCaptialFlowDataRecs">财务数据</param>
        public static void GetIndicatorData(int code, KLineCycle cycle, MarketType market, IsDivideRightType divideType,
             EMIndicator indicator, List<OneDayDataRec> kLineDataList, KLINEPERIOD indexCycle)
        {
            indicator.QuoteData.Clear();
            CalculateIndicatorDataByName(code, indicator, kLineDataList, divideType, indexCycle, market, cycle);
        }

        public static void GetFinancialIndicatorData(int code, KLineCycle cycle, MarketType market, EMIndicator indicator, TrendCaptialFlowDataRec[] trendCaptialFlowDataRecs) 
        {
            CalculateFinancialIndicatorDataByName(code,cycle, indicator,market, trendCaptialFlowDataRecs);
        }
        #endregion       

        #region 私有方法
        private static void CalculateFinancialIndicatorDataByName(int code, KLineCycle cycle,
            EMIndicator indicator, MarketType market, TrendCaptialFlowDataRec[] trendCaptialFlowDataRecs)
        {           
            try
            {
                //判断之前指标数据中有无选中状态
                    int selectIndex = -1;
                    if (indicator.QuoteData.Count > 0)
                    {
                        for (int i = 0; i < indicator.QuoteData.Count; i++)
                        {
                            if (indicator.QuoteData[i].FlagQuoteDataSelected)
                            {
                                selectIndex = i;
                                break;
                            }
                        }
                    }
                    //先将原指标数据清空，重新计算
                    if (indicator.IndicatorName == "DDX")
                        CalculateDDX(code, cycle, indicator, selectIndex, market, trendCaptialFlowDataRecs);
                    else if (indicator.IndicatorName == "DDY")
                        CalculateDDY(code, cycle, indicator, selectIndex, market, trendCaptialFlowDataRecs);
                    else if (indicator.IndicatorName == "DDZ")
                        CalculateDDZ(code, cycle, indicator, selectIndex, market, trendCaptialFlowDataRecs);
                    else if (indicator.IndicatorName == "ZJBY")
                        CalculateZJBY(code, cycle, indicator, selectIndex, market, trendCaptialFlowDataRecs);
                    else if (indicator.IndicatorName == "ZJQS")
                        CalculateZJQS(code, cycle, indicator, selectIndex, market, trendCaptialFlowDataRecs);
            }
            catch  
            {
            }
        }
        #region 财务指标计算公式
        private static double[] GetVolumeCalculate(TrendCaptialFlowDataRec[] data)
        {
            if (data == null || data.Length == 0)
                return new double[0];
            double[] result = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    result[i] = 1;
                    continue;
                }
                double tempTotalVolume = data[i].BuyVolume[0] + data[i].BuyVolume[1] + data[i].BuyVolume[2] + data[i].BuyVolume[3];
                if (tempTotalVolume == 0)
                    tempTotalVolume = 1;
                result[i] = tempTotalVolume;
            }
            return result;
        }
        /// <summary>
        /// 计算BigOrder数据
        /// </summary>
        /// <param name="n">N=1:买入委托 N=2:卖出委托</param>
        /// <param name="m">m=0:小单 m=1:中单 m=2:大单 m=3:特大单 m=-1:（缺省状态）默认为大单 ps:中单包含大单、特大单，大单包含特大单</param>
        /// <param name="data">原始资金流数据</param>
        /// <returns></returns>
        private static double[] KlineBigOrderCalculate(int n, int m, TrendCaptialFlowDataRec[] data)
        {
            if (data == null || data.Length == 0)
                return new double[0];
            double[] result = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    result[i] = 0;
                    continue;
                }
                ulong tempTotalVolume = data[i].BuyVolume[0] + data[i].BuyVolume[1] + data[i].BuyVolume[2] + data[i].BuyVolume[3];
                //ulong tempSellVolume = data[i].SellVolume[0] + data[i].SellVolume[1] + data[i].SellVolume[2] + data[i].SellVolume[3];
                //ulong tempTotalVolume = tempBuyVolume + tempSellVolume;
                ulong tempVolume = 0;
                float tempRate = 0F;
                if (n == 1)//买入
                {
                    switch (m)
                    {
                        case 0:
                            tempVolume = data[i].BuyVolume[0] + data[i].BuyVolume[1] + data[i].BuyVolume[2] + data[i].BuyVolume[3];
                            break;
                        case 1:
                            tempVolume = data[i].BuyVolume[1] + data[i].BuyVolume[2] + data[i].BuyVolume[3];
                            break;
                        case 2:
                            tempVolume = data[i].BuyVolume[2] + data[i].BuyVolume[3];
                            break;
                        case 3:
                            tempVolume = data[i].BuyVolume[3];
                            break;
                        case -1:
                            tempVolume = data[i].BuyVolume[2] + data[i].BuyVolume[3];
                            break;
                    }
                }
                else if (n == 2)//卖出
                {
                    switch (m)
                    {
                        case 0:
                            tempVolume = data[i].SellVolume[0] + data[i].SellVolume[1] + data[i].SellVolume[2] + data[i].SellVolume[3];
                            break;
                        case 1:
                            tempVolume = data[i].SellVolume[1] + data[i].SellVolume[2] + data[i].SellVolume[3];
                            break;
                        case 2:
                            tempVolume = data[i].SellVolume[2] + data[i].SellVolume[3];
                            break;
                        case 3:
                            tempVolume = data[i].SellVolume[3];
                            break;
                        case -1:
                            tempVolume = data[i].SellVolume[2] + data[i].SellVolume[3];
                            break;
                    }
                }
                if (tempTotalVolume.Equals(0))
                    tempRate = 0F;
                else
                    tempRate = tempVolume * 1.0F / tempTotalVolume;
                result[i] = tempRate;
            }
            return result;
        }

        private static double[] GetOrderCalculate(int para, TrendCaptialFlowDataRec[] data)
        {
            if (data == null || data.Length == 0)
                return new double[0];
            double[] result = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    result[i] = 1;
                    continue;
                }
                double tempNum = 0;
                switch (para)
                {
                    case 1:
                        tempNum = data[i].BuyNum[0] + data[i].BuyNum[1] + data[i].BuyNum[2] + data[i].BuyNum[3];
                        break;
                    case 2:
                        tempNum = data[i].SellNum[0] + data[i].SellNum[1] + data[i].SellNum[2] + data[i].SellNum[3];
                        break;
                }
                if (tempNum == 0)
                    tempNum = 1;
                result[i] = tempNum;
            }
            return result;
        }

        /// <summary>
        /// BIGAMOUNT(1)-BIGAMOUNT(2)表示：超大单流入额+大单流入额-超大单流出额-大单流出额
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double[] GetBigAmountCalculate(TrendCaptialFlowDataRec[] data)
        {
            if (data == null || data.Length == 0)
                return new double[0];
            double[] result = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    result[i] = 0;
                    continue;
                }
                double amountIn = data[i].BuyAmount[2] + data[i].BuyAmount[3];
                double amountOut = data[i].SellAmount[2] + data[i].SellAmount[3];
                double amount = amountIn - amountOut;
                result[i] = amount;
            }
            return result;
        }

        private static void CalculateDDX(int Code, KLineCycle cycle,
            EMIndicator indicator, int selectIndex, MarketType market, TrendCaptialFlowDataRec[] trendCaptialFlowDataRec)
        {
            FM_FORMULA_EXTRA_CONST[] paraArr = new FM_FORMULA_EXTRA_CONST[4];
            paraArr[0] = new FM_FORMULA_EXTRA_CONST();
            paraArr[0].Name = Marshal.StringToHGlobalAnsi("BIGORDER1");
            double[] bigOrder1 = KlineBigOrderCalculate(1, -1, trendCaptialFlowDataRec);
            paraArr[0].Length = bigOrder1.Length;
            int size = Marshal.SizeOf(typeof(double));
            IntPtr pbigOrder1 = Marshal.AllocHGlobal(size * bigOrder1.Length);
            Marshal.Copy(bigOrder1, 0, pbigOrder1, bigOrder1.Length);
            paraArr[0].pValue = pbigOrder1;


            paraArr[1] = new FM_FORMULA_EXTRA_CONST();
            paraArr[1].Name = Marshal.StringToHGlobalAnsi("BIGORDER2");
            double[] BigOrder2 = KlineBigOrderCalculate(2, -1, trendCaptialFlowDataRec);
            paraArr[1].Length = BigOrder2.Length;
            IntPtr pBigOrder2 = Marshal.AllocHGlobal(size * BigOrder2.Length);
            Marshal.Copy(BigOrder2, 0, pBigOrder2, BigOrder2.Length);
            paraArr[1].pValue = pBigOrder2;


            paraArr[2] = new FM_FORMULA_EXTRA_CONST();
            paraArr[2].Name = Marshal.StringToHGlobalAnsi("volflow");
            double[] volflowdata = GetVolumeCalculate(trendCaptialFlowDataRec);
            paraArr[2].Length = volflowdata.Length;
            IntPtr pvolflowdata = Marshal.AllocHGlobal(size * volflowdata.Length);
            Marshal.Copy(volflowdata, 0, pvolflowdata, volflowdata.Length);
            paraArr[2].pValue = pvolflowdata;


            paraArr[3] = new FM_FORMULA_EXTRA_CONST();
            paraArr[3].Name = Marshal.StringToHGlobalAnsi("capitalflow");
            paraArr[3].Length = BigOrder2.Length;
            double[] capitalArr = new double[BigOrder2.Length];
            double tempCapital = 0;
            if (SecurityAttribute.BMarketType.Contains(market)) //b股流通股本
                tempCapital = Dc.GetFieldDataDouble(Code, FieldIndex.NetBShare);
            else
                tempCapital = Dc.GetFieldDataDouble(Code, FieldIndex.NetAShare);
            for (int m = 0; m < BigOrder2.Length; m++)
            {
                capitalArr[m] = tempCapital;
            }
            IntPtr pcapitalArr = Marshal.AllocHGlobal(size * BigOrder2.Length);
            Marshal.Copy(capitalArr, 0, pcapitalArr, capitalArr.Length);
            paraArr[3].pValue = pcapitalArr;

            String errmsg = String.Empty;
            FmFormulaOutput output = new FmFormulaOutput();
            try
            {
                FormulaProxy.ExecuteFormulaWithExtraConst(indicator.Formula, paraArr, 4, ref output, BigOrder2.Length, errmsg);
                for (int n = 0; n < output.outputCount; n++)
                {
                    if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                        GetNormalFormulaOutput(indicator, output, BigOrder2.Length, n, selectIndex,market,cycle);
                    else
                        GetFunctionFormulaOutput(indicator, output, BigOrder2.Length, n, selectIndex);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void CalculateDDY(int Code, KLineCycle cycle,
            EMIndicator indicator, int selectIndex, MarketType market, TrendCaptialFlowDataRec[] trendCaptialFlowDataRec)
        {
            FM_FORMULA_EXTRA_CONST[] paraArr = new FM_FORMULA_EXTRA_CONST[6];
            paraArr[0] = new FM_FORMULA_EXTRA_CONST();
            paraArr[0].Name = Marshal.StringToHGlobalAnsi("BIGORDER1");
            double[] bigOrder1 = KlineBigOrderCalculate(1, -1, trendCaptialFlowDataRec);
            paraArr[0].Length = bigOrder1.Length;
            int size = Marshal.SizeOf(typeof(double));
            IntPtr pbigOrder1 = Marshal.AllocHGlobal(size * bigOrder1.Length);
            Marshal.Copy(bigOrder1, 0, pbigOrder1, bigOrder1.Length);
            paraArr[0].pValue = pbigOrder1;


            paraArr[1] = new FM_FORMULA_EXTRA_CONST();
            paraArr[1].Name = Marshal.StringToHGlobalAnsi("BIGORDER2");
            double[] bigOrder2 = KlineBigOrderCalculate(2, -1, trendCaptialFlowDataRec);
            paraArr[1].Length = bigOrder2.Length;
            IntPtr pBigOrder2 = Marshal.AllocHGlobal(size * bigOrder2.Length);
            Marshal.Copy(bigOrder2, 0, pBigOrder2, bigOrder2.Length);
            paraArr[1].pValue = pBigOrder2;


            paraArr[2] = new FM_FORMULA_EXTRA_CONST();
            paraArr[2].Name = Marshal.StringToHGlobalAnsi("volflow");
            double[] volflowdata = GetVolumeCalculate(trendCaptialFlowDataRec);
            paraArr[2].Length = volflowdata.Length;
            IntPtr pvolflowdata = Marshal.AllocHGlobal(size * volflowdata.Length);
            Marshal.Copy(volflowdata, 0, pvolflowdata, volflowdata.Length);
            paraArr[2].pValue = pvolflowdata;


            paraArr[3] = new FM_FORMULA_EXTRA_CONST();
            paraArr[3].Name = Marshal.StringToHGlobalAnsi("capitalflow");
            paraArr[3].Length = bigOrder2.Length;
            double[] capitalArr = new double[bigOrder2.Length];
            double tempCapital = 0;
            if (SecurityAttribute.BMarketType.Contains(market)) //b股流通股本
                tempCapital = Dc.GetFieldDataDouble(Code, FieldIndex.NetBShare);
            else
                tempCapital = Dc.GetFieldDataDouble(Code, FieldIndex.NetAShare);
            for (int m = 0; m < bigOrder2.Length; m++)
            {
                capitalArr[m] = tempCapital;
            }
            IntPtr pcapitalArr = Marshal.AllocHGlobal(size * bigOrder2.Length);
            Marshal.Copy(capitalArr, 0, pcapitalArr, capitalArr.Length);
            paraArr[3].pValue = pcapitalArr;

            paraArr[4] = new FM_FORMULA_EXTRA_CONST();
            paraArr[4].Name = Marshal.StringToHGlobalAnsi("ORDER1");
            double[] Order1 = GetOrderCalculate(1, trendCaptialFlowDataRec);
            paraArr[4].Length = Order1.Length;
            IntPtr pOrder1 = Marshal.AllocHGlobal(size * Order1.Length);
            Marshal.Copy(Order1, 0, pOrder1, Order1.Length);
            paraArr[4].pValue = pOrder1;


            paraArr[5] = new FM_FORMULA_EXTRA_CONST();
            paraArr[5].Name = Marshal.StringToHGlobalAnsi("ORDER2");
            double[] Order2 = GetOrderCalculate(2, trendCaptialFlowDataRec);
            paraArr[5].Length = Order2.Length;
            IntPtr pOrder2 = Marshal.AllocHGlobal(size * Order2.Length);
            Marshal.Copy(Order2, 0, pOrder2, Order2.Length);
            paraArr[5].pValue = pOrder2;

            String errmsg = String.Empty;
            FmFormulaOutput output = new FmFormulaOutput();
            try
            {
                FormulaProxy.ExecuteFormulaWithExtraConst(indicator.Formula, paraArr, 6, ref output, bigOrder2.Length, errmsg);
                for (int n = 0; n < output.outputCount; n++)
                {
                    if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                        GetNormalFormulaOutput(indicator, output, Order2.Length, n, selectIndex, market, cycle);
                    else
                        GetFunctionFormulaOutput(indicator, output, Order2.Length, n, selectIndex);
                }
            }
            catch  
            {
                throw;
            }
        }

        private static void CalculateDDZ(int Code, KLineCycle cycle,
            EMIndicator indicator, int selectIndex, MarketType market, TrendCaptialFlowDataRec[] trendCaptialFlowDataRec)
        {
            FM_FORMULA_EXTRA_CONST[] paraArr = new FM_FORMULA_EXTRA_CONST[5];
            paraArr[0] = new FM_FORMULA_EXTRA_CONST();
            paraArr[0].Name = Marshal.StringToHGlobalAnsi("BIGORDER1");
            double[] bigOrder1 = KlineBigOrderCalculate(1, -1, trendCaptialFlowDataRec);
            paraArr[0].Length = bigOrder1.Length;
            int size = Marshal.SizeOf(typeof(double));
            IntPtr pbigOrder1 = Marshal.AllocHGlobal(size * bigOrder1.Length);
            Marshal.Copy(bigOrder1, 0, pbigOrder1, bigOrder1.Length);
            paraArr[0].pValue = pbigOrder1;

            paraArr[1] = new FM_FORMULA_EXTRA_CONST();
            paraArr[1].Name = Marshal.StringToHGlobalAnsi("BIGORDER2");
            double[] BigOrder2 = KlineBigOrderCalculate(2, -1, trendCaptialFlowDataRec);
            paraArr[1].Length = BigOrder2.Length;
            IntPtr pBigOrder2 = Marshal.AllocHGlobal(size * BigOrder2.Length);
            Marshal.Copy(BigOrder2, 0, pBigOrder2, BigOrder2.Length);
            paraArr[1].pValue = pBigOrder2;

            paraArr[2] = new FM_FORMULA_EXTRA_CONST();
            paraArr[2].Name = Marshal.StringToHGlobalAnsi("volflow");
            double[] volflowdata = GetVolumeCalculate(trendCaptialFlowDataRec);
            paraArr[2].Length = volflowdata.Length;
            IntPtr pvolflowdata = Marshal.AllocHGlobal(size * volflowdata.Length);
            Marshal.Copy(volflowdata, 0, pvolflowdata, volflowdata.Length);
            paraArr[2].pValue = pvolflowdata;

            paraArr[3] = new FM_FORMULA_EXTRA_CONST();
            paraArr[3].Name = Marshal.StringToHGlobalAnsi("ORDER1");
            double[] Order1 = GetOrderCalculate(1, trendCaptialFlowDataRec);
            paraArr[3].Length = Order1.Length;
            IntPtr pOrder1 = Marshal.AllocHGlobal(size * Order1.Length);
            Marshal.Copy(Order1, 0, pOrder1, Order1.Length);
            paraArr[3].pValue = pOrder1;

            paraArr[4] = new FM_FORMULA_EXTRA_CONST();
            paraArr[4].Name = Marshal.StringToHGlobalAnsi("ORDER2");
            double[] Order2 = GetOrderCalculate(2, trendCaptialFlowDataRec);
            paraArr[4].Length = Order2.Length;
            IntPtr pOrder2 = Marshal.AllocHGlobal(size * Order2.Length);
            Marshal.Copy(Order2, 0, pOrder2, Order2.Length);
            paraArr[4].pValue = pOrder2;

            String errmsg = String.Empty;
            FmFormulaOutput output = new FmFormulaOutput();
            try
            {
                FormulaProxy.ExecuteFormulaWithExtraConst(indicator.Formula, paraArr, 5, ref output, BigOrder2.Length, errmsg);
                for (int n = 0; n < output.outputCount; n++)
                {
                    if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                        GetNormalFormulaOutput(indicator, output, BigOrder2.Length, n, selectIndex, market,cycle);
                    else
                        GetFunctionFormulaOutput(indicator, output, BigOrder2.Length, n, selectIndex);
                }
            }
            catch
            {
                throw;
            }
        }

        private static void CalculateZJQS(int Code, KLineCycle cycle,
            EMIndicator indicator, int selectIndex, MarketType market, TrendCaptialFlowDataRec[] trendCaptialFlowDataRec)
        {
            FM_FORMULA_EXTRA_CONST[] paraArr = new FM_FORMULA_EXTRA_CONST[1];
            paraArr[0] = new FM_FORMULA_EXTRA_CONST();
            paraArr[0].Name = Marshal.StringToHGlobalAnsi("BIGAMOUNT12");
            double[] bigAmount = GetBigAmountCalculate(trendCaptialFlowDataRec);
            paraArr[0].Length = bigAmount.Length;
            int size = Marshal.SizeOf(typeof(double));
            IntPtr pbigAmount = Marshal.AllocHGlobal(size * bigAmount.Length);
            Marshal.Copy(bigAmount, 0, pbigAmount, bigAmount.Length);
            paraArr[0].pValue = pbigAmount;

            String errmsg = String.Empty;
            FmFormulaOutput output = new FmFormulaOutput();
            try
            {
                FormulaProxy.ExecuteFormulaWithExtraConst(indicator.Formula, paraArr, 1, ref output, bigAmount.Length, errmsg);
                for (int n = 0; n < output.outputCount; n++)
                {
                    if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                        GetNormalFormulaOutput(indicator, output, bigAmount.Length, n, selectIndex,market,cycle);
                    else
                        GetFunctionFormulaOutput(indicator, output, bigAmount.Length, n, selectIndex);
                }
            }
            catch
            {
                throw;
            }
        }

        private static void CalculateZJBY(int Code, KLineCycle cycle,
            EMIndicator indicator, int selectIndex, MarketType market, TrendCaptialFlowDataRec[] trendCaptialFlowDataRec)
        {
            FM_FORMULA_EXTRA_CONST[] paraArr = new FM_FORMULA_EXTRA_CONST[9];
            paraArr[0] = new FM_FORMULA_EXTRA_CONST();
            paraArr[0].Name = Marshal.StringToHGlobalAnsi("BIGORDER13");
            double[] bigOrder13 = KlineBigOrderCalculate(1, 3, trendCaptialFlowDataRec);
            paraArr[0].Length = bigOrder13.Length;
            int size = Marshal.SizeOf(typeof(double));
            IntPtr pbigOrder13 = Marshal.AllocHGlobal(size * bigOrder13.Length);
            Marshal.Copy(bigOrder13, 0, pbigOrder13, bigOrder13.Length);
            paraArr[0].pValue = pbigOrder13;

            paraArr[1] = new FM_FORMULA_EXTRA_CONST();
            paraArr[1].Name = Marshal.StringToHGlobalAnsi("BIGORDER23");
            double[] BigOrder23 = KlineBigOrderCalculate(2, 3, trendCaptialFlowDataRec);
            paraArr[1].Length = BigOrder23.Length;
            IntPtr pBigOrder23 = Marshal.AllocHGlobal(size * BigOrder23.Length);
            Marshal.Copy(BigOrder23, 0, pBigOrder23, BigOrder23.Length);
            paraArr[1].pValue = pBigOrder23;


            paraArr[2] = new FM_FORMULA_EXTRA_CONST();
            paraArr[2].Name = Marshal.StringToHGlobalAnsi("volflow");
            double[] volflowdata = GetVolumeCalculate(trendCaptialFlowDataRec);
            paraArr[2].Length = volflowdata.Length;
            IntPtr pvolflowdata = Marshal.AllocHGlobal(size * volflowdata.Length);
            Marshal.Copy(volflowdata, 0, pvolflowdata, volflowdata.Length);
            paraArr[2].pValue = pvolflowdata;


            paraArr[3] = new FM_FORMULA_EXTRA_CONST();
            paraArr[3].Name = Marshal.StringToHGlobalAnsi("capitalflow");
            paraArr[3].Length = BigOrder23.Length;
            double[] capitalArr = new double[BigOrder23.Length];
            double tempCapital = 0;
            if (SecurityAttribute.BMarketType.Contains(market)) //b股流通股本
                tempCapital = Dc.GetFieldDataDouble(Code, FieldIndex.NetBShare);
            else
                tempCapital = Dc.GetFieldDataDouble(Code, FieldIndex.NetAShare);
            for (int m = 0; m < BigOrder23.Length; m++)
            {
                capitalArr[m] = tempCapital;
            }
            IntPtr pcapitalArr = Marshal.AllocHGlobal(size * BigOrder23.Length);
            Marshal.Copy(capitalArr, 0, pcapitalArr, capitalArr.Length);
            paraArr[3].pValue = pcapitalArr;

            paraArr[4] = new FM_FORMULA_EXTRA_CONST();
            paraArr[4].Name = Marshal.StringToHGlobalAnsi("BIGORDER12");
            double[] BigOrder12 = KlineBigOrderCalculate(1, 2, trendCaptialFlowDataRec);
            paraArr[4].Length = BigOrder12.Length;
            IntPtr pBigOrder12 = Marshal.AllocHGlobal(size * BigOrder12.Length);
            Marshal.Copy(BigOrder12, 0, pBigOrder12, BigOrder12.Length);
            paraArr[4].pValue = pBigOrder12;

            paraArr[5] = new FM_FORMULA_EXTRA_CONST();
            paraArr[5].Name = Marshal.StringToHGlobalAnsi("BIGORDER22");
            double[] BigOrder22 = KlineBigOrderCalculate(2, 2, trendCaptialFlowDataRec);
            paraArr[5].Length = BigOrder22.Length;
            IntPtr pBigOrder22 = Marshal.AllocHGlobal(size * BigOrder22.Length);
            Marshal.Copy(BigOrder22, 0, pBigOrder22, BigOrder22.Length);
            paraArr[5].pValue = pBigOrder22;

            paraArr[6] = new FM_FORMULA_EXTRA_CONST();
            paraArr[6].Name = Marshal.StringToHGlobalAnsi("BIGORDER11");
            double[] BigOrder11 = KlineBigOrderCalculate(1, 1, trendCaptialFlowDataRec);
            paraArr[6].Length = BigOrder11.Length;
            IntPtr pBigOrder11 = Marshal.AllocHGlobal(size * BigOrder11.Length);
            Marshal.Copy(BigOrder11, 0, pBigOrder11, BigOrder11.Length);
            paraArr[6].pValue = pBigOrder11;

            paraArr[7] = new FM_FORMULA_EXTRA_CONST();
            paraArr[7].Name = Marshal.StringToHGlobalAnsi("BIGORDER21");
            double[] BigOrder21 = KlineBigOrderCalculate(2, 1, trendCaptialFlowDataRec);
            paraArr[7].Length = BigOrder21.Length;
            IntPtr pBigOrder21 = Marshal.AllocHGlobal(size * BigOrder21.Length);
            Marshal.Copy(BigOrder21, 0, pBigOrder21, BigOrder21.Length);
            paraArr[7].pValue = pBigOrder21;

            paraArr[8] = new FM_FORMULA_EXTRA_CONST();
            paraArr[8].Name = Marshal.StringToHGlobalAnsi("BIGORDER10");
            double[] BigOrder10 = KlineBigOrderCalculate(1, 0, trendCaptialFlowDataRec);
            paraArr[8].Length = BigOrder10.Length;
            IntPtr pBigOrder10 = Marshal.AllocHGlobal(size * BigOrder10.Length);
            Marshal.Copy(BigOrder10, 0, pBigOrder10, BigOrder10.Length);
            paraArr[8].pValue = pBigOrder10;

            String errmsg = String.Empty;
            FmFormulaOutput output = new FmFormulaOutput();
            try
            {
                FormulaProxy.ExecuteFormulaWithExtraConst(indicator.Formula, paraArr, 9, ref output, BigOrder10.Length, errmsg);
                for (int n = 0; n < output.outputCount; n++)
                {
                    if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                        GetNormalFormulaOutput(indicator, output, bigOrder13.Length, n, selectIndex,market,cycle);
                    else
                        GetFunctionFormulaOutput(indicator, output, bigOrder13.Length, n, selectIndex);
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        private static void GetFinallyCalculDevidedKLine(float[] devides, List<OneDayDataRec> originalRecs,
         IsDivideRightType devideType)
        {
            if (devides == null)
                return ;
            if (originalRecs == null)
                return ;
            if (devides.Length == 0 || originalRecs.Count == 0)
                return ;
            if (devides.Length != originalRecs.Count)
                return ;

            if (devideType == IsDivideRightType.Non)
                return ;

            for (int i = 0; i < devides.Length; i++)
            {
                float currDev = devides[i];
                GetDataAfterDivide(devideType, originalRecs[i], currDev);
            }
        }

        /// <summary>
        /// 获得周K以上复权后的K线数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <param name="divideType"></param>
        /// <returns></returns>
        private static List<OneDayDataRec> GetDevidedKLineData(int code, MarketType market, KLineCycle cycle,
            IsDivideRightType divideType)
        {
            //1、获取对应周期的复权数据
            List<OneDayDataRec> result = GetNormalKLineData(code, cycle);
            bool isCycleYear = cycle == KLineCycle.CycleYear;
            bool isForward = divideType == IsDivideRightType.Forward;
            float[] devides = CaculateDivideKLineData(code, result, isCycleYear, isForward);
            GetFinallyCalculDevidedKLine(devides, result, divideType);

            //2、针对复权日所在周期的特殊处理
            //2-1：取得对应的日期起讫时间
            List<List<OneDivideRightBase>> divideData = Dc.GetDivideRightData(code);
            if(divideData==null)
                return result;

            //2-2: 获得日K线及日K线复权后数据, 用以推断出复权日所在周（月/季/年）的复权数据
            float[] dayKLineDevides;
            List<OneDayDataRec> dayKLineData;
            OneStockKLineDataRec allDayKLineData = Dc.GetHisKLineData(code, KLineCycle.CycleDay);
            if (allDayKLineData == null || allDayKLineData.OneDayDataList == null
                && allDayKLineData.OneDayDataList.Count == 0)
                return result;
            dayKLineData = GetNormalKLineData(code, KLineCycle.CycleDay);
            dayKLineDevides = CaculateDivideKLineData(code, dayKLineData, false, isForward);
            GetFinallyCalculDevidedKLine(dayKLineDevides, dayKLineData, divideType);
            CombineData(code, market, cycle, divideData, divideType, result, dayKLineData, dayKLineDevides);
            return result;
        }

        /// <summary>
        /// 拼接出复权数据（日线周期以上）
        /// </summary>
        /// <param name="code">股票编码</param>
        /// <param name="market">市场类型</param>
        /// <param name="kLineCycle">请求的K线周期类型</param>
        /// <param name="divideData">复权数据</param>
        /// <param name="divideType">复权类型</param>
        /// <param name="originalKLineList">原始K线复权数据（复权日所在周期未处理）</param>
        /// <param name="dayCycleKLinelist">日K线复权数据</param>
        /// <param name="factors">复权因子</param>
        private static void CombineData(int code, MarketType market, KLineCycle kLineCycle,
            List<List<OneDivideRightBase>> divideData, IsDivideRightType divideType,
           List<OneDayDataRec> originalKLineList, List<OneDayDataRec> dayCycleKLinelist, float[] factors)
        {

            if (!SecurityAttribute.HasDivideRightTypeList.Contains(market)
                || SecurityAttribute.FundFinancingTypeList.Contains(market)
                || (int)kLineCycle <= (int)KLineCycle.CycleDay)
                return;

            //计算复权日在周线及以上周期数据中的index
            List<int> divideIndex = new List<int>();
            bool flag = false;
            int tempIndex = 0;
            foreach (List<OneDivideRightBase> tempList in divideData)
            {
                tempIndex = GetIndexByDate(tempList[0].Date, out flag, originalKLineList);
                if (flag && !divideIndex.Contains(tempIndex))
                    divideIndex.Add(tempIndex);
            }
            //获取index及index-1对应数据的日期
            int startDate;
            int endDate;
            int startIndex;
            int endIndex;
            bool startIndexFlag = true;
            bool endIndexFlag = true;
            foreach (int rawIndex in divideIndex)
            {
                startDate = 0;
                endDate = originalKLineList[rawIndex].Date;
                if (rawIndex > 0)
                    startDate = originalKLineList[rawIndex - 1].Date;
                //根据对应日期计算日线中对应的startindex和endindex
                if (startDate != 0)
                    startIndex = GetIndexByDate(startDate, out startIndexFlag, dayCycleKLinelist) + 1;
                else
                    startIndex = 0;
                endIndex = GetIndexByDate(endDate, out endIndexFlag, dayCycleKLinelist);
                //特殊处理（当前交易日有出复权信息）
                if (endDate == Dc.GetTradeDate(code))
                {
                    endIndex = dayCycleKLinelist.Count - 1;
                    endIndexFlag = true;
                }
                if (!startIndexFlag || !endIndexFlag)
                    continue;
                //将日线中startindex和endindex范围内的数据拼接成对应周期的数据并替换
                GenerateCycleKlineData(divideType, startIndex, endIndex, dayCycleKLinelist, factors, rawIndex, originalKLineList);
            }
        }

        /// <summary>
        /// 根据新闻研报时间获取其在k线数据中的Index
        /// </summary>
        /// <param name="date"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private static int GetIndexByDate(int date, out bool flag, List<OneDayDataRec> rawList)
        {
            if (rawList == null || rawList.Count == 0 || (date > rawList[rawList.Count - 1].Date || date < rawList[0].Date))
            {
                flag = false;
                return -1;
            }
            int newsIndex;
            int lower = 0;
            int upper = rawList.Count - 1;
            while (true)
            {
                int currentNum = (lower + upper) / 2;
                if (rawList[currentNum].Date == date)
                {
                    newsIndex = currentNum;
                    break;
                }
                else if (lower > upper)
                {
                    newsIndex = lower;
                    break;
                }
                else if (rawList[currentNum].Date > date)
                {
                    upper = currentNum - 1;
                }
                else if (rawList[currentNum].Date < date)
                {
                    lower = currentNum + 1;
                }
            }
            flag = true;
            return newsIndex;
        }

        /// <summary>
        /// 由日k线复权数据生产复权日所在周期的（周/月等）k线复权数据
        /// </summary>
        /// <param name="divideType">复权类型</param>
        /// <param name="startIndex">第一个含有复权（周/月/季/年）的位置</param>
        /// <param name="endIndex">最后一个含有复权（周/月/季/年）的位置</param>
        /// <param name="dayCycleKLinelist">日K线复权数据</param>
        /// <param name="factors">日K线复权因子数组</param>
        /// <param name="rawIndex"></param>
        /// <param name="originalKLineList">原始K线复权数据（复权日所在周期未处理）</param>
        private static void GenerateCycleKlineData(IsDivideRightType divideType, int startIndex, int endIndex,
            List<OneDayDataRec> dayCycleKLinelist, float[] factors, int rawIndex,
            List<OneDayDataRec> originalKLineList)
        {
            if (dayCycleKLinelist == null || dayCycleKLinelist.Count == 0
                || startIndex >= dayCycleKLinelist.Count || endIndex >= dayCycleKLinelist.Count
                || originalKLineList == null || originalKLineList.Count == 0 || rawIndex >= originalKLineList.Count)
                return;
            float tempOpen =dayCycleKLinelist[startIndex].Open;
            float tempClose = dayCycleKLinelist[endIndex].Close;
            float max = 0.00f;
            float min = 0.00f;
            for (int i = startIndex; i <= endIndex; i++)
            {
                OneDayDataRec tempData = dayCycleKLinelist[i];

                if (min.Equals(0.00f))
                    min = tempData.Low;
                if (tempData.Low < min)
                    min = tempData.Low;
                if (tempData.High > max)
                    max = tempData.High;
            }
            originalKLineList[rawIndex].Open = tempOpen;
            originalKLineList[rawIndex].Close = tempClose;
            originalKLineList[rawIndex].High = max;
            originalKLineList[rawIndex].Low = min;
        }

        /// <summary>
        /// 获得复权数据
        /// </summary>
        /// <param name="divideType">复权类型</param>
        /// <param name="data">原始数据</param>
        /// <param name="factor">复权因子</param>
        /// <returns>复权后的k线数据</returns>
        private static void GetDataAfterDivide(IsDivideRightType divideType, OneDayDataRec data, float factor)
        {
            if (data == null)
                return;
            float tempFactor = 1.0F;
            if (divideType == IsDivideRightType.Forward)
                tempFactor = factor;
            else if (divideType == IsDivideRightType.Backward)
                tempFactor = 1.0F / factor;

            data.High = data.High * tempFactor;
            data.Open = data.Open * tempFactor;
            data.Low = data.Low * tempFactor;
            data.Close = data.Close * tempFactor;
        }

        private static List<OneDayDataRec> GetFundFinancingBackwardKLineData(int code, KLineCycle cycle)
        {
            List<OneDayDataRec> result;
            OneStockKLineDataRec DataRec = Dc.GetFundAfterDivideKLineData(code, cycle);
            if (DataRec == null)
                result = new List<OneDayDataRec>(0);
            else
                result = DataRec.OneDayDataListCopy();

            //历史K线、当日k线组合
            // 当日（周/月/年/...）K线数据
            OneStockKLineDataRec TodayDataRec = Dc.GetTodayKLineData(code, cycle);
            if (TodayDataRec == null || TodayDataRec.OneDayDataList==null)
                return result;

            SetKlineList(result, TodayDataRec);
            return result;
        }

        /// <summary>
        /// 获取普通型的K线数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        private static List<OneDayDataRec> GetNormalKLineData(int code, KLineCycle cycle)
        {
            List<OneDayDataRec> result=null;
            OneStockKLineDataRec DataRec = Dc.GetHisKLineData(code, cycle);

            try 
            {
                if (DataRec == null)
                    result = new List<OneDayDataRec>(0);
                else
                    result = DataRec.OneDayDataListCopy();
            }catch(Exception e)
            {

            }
            // 当日（周/月/年/...）K线数据
            OneStockKLineDataRec TodayDataRec = Dc.GetTodayKLineData(code, cycle);
            if (TodayDataRec == null || TodayDataRec.OneDayDataList==null)
                return result;

            SetKlineList(result, TodayDataRec); 
            return result;
        }
        /// <summary>
        ///  拼接（历史和当日的）K线数据，使之连续
        /// </summary>
        /// <param name="hisRecs">历史K线数据</param>
        /// <param name="todayRecs">当日K线数据</param>
        private static void SetKlineList(List<OneDayDataRec> hisRecs, OneStockKLineDataRec todayRec)
        {
            if (hisRecs == null || todayRec == null
               || todayRec.OneDayDataList == null || todayRec.OneDayDataList.Count == 0)
                return;
            List<OneDayDataRec> todayList = todayRec.OneDayDataListCopy();
            if (hisRecs.Count == 0)
            {
                for (int i = 0; i < todayList.Count; i++)
                {
                    hisRecs.Add(todayList[i]);
                }
            }
            else
            {
                if (hisRecs[hisRecs.Count - 1].Date == todayList[0].Date)
                    hisRecs.RemoveAt(hisRecs.Count - 1);
                for (int i = 0; i < todayList.Count; i++)
                {
                    if ((hisRecs.Count != 0 && (todayList[i].Date > hisRecs[hisRecs.Count - 1].Date
                        || (todayList[i].Date == hisRecs[hisRecs.Count - 1].Date
                        && todayList[i].Time > hisRecs[hisRecs.Count - 1].Time)))
                        || hisRecs.Count == 0)
                        hisRecs.Add(todayList[i]);
                }
            }
        }

        private static void CalculateIndicatorDataByName(int Code, EMIndicator indicator,
            List<OneDayDataRec> DrawKlineDataStruList,
            IsDivideRightType DivideType, KLINEPERIOD IndexCycle, MarketType market, KLineCycle cycle)
        {
            //判断之前指标数据中有无选中状态
            int selectIndex = -1;
            if (indicator.QuoteData.Count > 0)
            {
                for (int i = 0; i < indicator.QuoteData.Count; i++)
                {
                    if (indicator.QuoteData[i].FlagQuoteDataSelected)
                    {
                        selectIndex = i;
                        break;
                    }
                }
            }
            //先将原指标数据清空，重新计算
            FORMULA_TIME begin, end;
            begin = new FORMULA_TIME();
            int nlen = DrawKlineDataStruList.Count;
            Kline[] klines = new Kline[nlen];

            #region 设置指标公式参数
            if (DrawKlineDataStruList == null || DrawKlineDataStruList.Count <= 0)
                return;
            begin.year = (ushort)(DrawKlineDataStruList[0].Date / 10000);
            begin.month = (byte)((DrawKlineDataStruList[0].Date - begin.year * 10000) / 100);
            begin.day = (byte)(DrawKlineDataStruList[0].Date - begin.year * 10000 - begin.month * 100);
            end = new FORMULA_TIME();
            end.year = (ushort)(DrawKlineDataStruList[DrawKlineDataStruList.Count - 1].Date / 10000);
            end.month = (byte)((DrawKlineDataStruList[DrawKlineDataStruList.Count - 1].Date - end.year * 10000) / 100);
            end.day = (byte)(DrawKlineDataStruList[DrawKlineDataStruList.Count - 1].Date - end.year * 10000 - end.month * 100);
            for (int i = 0; i < nlen; i++)
            {
                klines[i].Date = DrawKlineDataStruList[i].Date;
                klines[i].Open = DrawKlineDataStruList[i].Open;
                klines[i].Close = DrawKlineDataStruList[i].Close;
                klines[i].High = DrawKlineDataStruList[i].High;
                klines[i].Low = DrawKlineDataStruList[i].Low;
                klines[i].Value = DrawKlineDataStruList[i].Amount;
                klines[i].Volume = DrawKlineDataStruList[i].Volume;
                klines[i].Time = DrawKlineDataStruList[i].Time;
            }
            #endregion

            String errmsg = String.Empty;
            FmFormulaOutput output = new FmFormulaOutput();

            String emcode = String.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(Code))
                DetailData.FieldIndexDataString[Code].TryGetValue(FieldIndex.EMCode, out emcode);

            //执行调用指标公式方法
            if (indicator.Formula.fid <= 0)
            {

                FormulaProxy.ExecuteFormula(DrawKlineDataStruList[0].Date, (int)IndexCycle, 1,
                                            emcode, begin,
                                            end, 0, klines, indicator.IndicatorName, errmsg, ref output, nlen);
            }
            else
            {
                FormulaProxy.ExecuteFormulaWithKLineAndFormula(DrawKlineDataStruList[0].Date,
                                                               (int)IndexCycle, 1, emcode, begin,
                                                               end, klines, indicator.Formula, errmsg, ref output, nlen);
            }

            for (int n = 0; n < output.outputCount; n++)
            { 
                if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                    GetNormalFormulaOutput(indicator, output, nlen, n, selectIndex, market, cycle);
                else
                    GetFunctionFormulaOutput(indicator, output, nlen, n, selectIndex);
            }
        }

        private static void GetFunctionFormulaOutput(EMIndicator indicator, FmFormulaOutput output,
            int dataCount, int index, int selectIndex)
        {
            using (QuoteDataStru quoteDataStru = new QuoteDataStru())
            {
                String nameTemp = output.fmOutput[index].name.Trim('\0');
                quoteDataStru.QuoteName = nameTemp;
                try
                {
                    IntPtr prt = new IntPtr(output.fmOutput[index].foutput.ToInt64());
                    FM_FORMULA_FUNCTION_OUTPUT outputtype = (FM_FORMULA_FUNCTION_OUTPUT)Marshal.PtrToStructure(prt, typeof(FM_FORMULA_FUNCTION_OUTPUT));
                    quoteDataStru.QuotePicType = GetFunctionQuotePicType(outputtype.type);
                    switch (outputtype.type)
                    {
                        #region OUTPUT_TYPE_POLYLINE
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_POLYLINE:
                            using (PolyLineOutput tempPolyLine = new PolyLineOutput())
                            {
                                tempPolyLine.Price = new double[dataCount];
                                double[] doublePoly = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr priceptr = new IntPtr(outputtype.polyline.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(priceptr, doublePoly, 0, 1);
                                    tempPolyLine.Price[j] = doublePoly[0];
                                }
                                quoteDataStru.QuoteFunctionList = tempPolyLine;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWLINE
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWLINE:
                            using (DrawLineOutput tempDrawLine = new DrawLineOutput())
                            {
                                tempDrawLine.Price = new double[dataCount];
                                tempDrawLine.Expand = new double[dataCount];
                                double[] valuePrice = new double[1];
                                double[] valueExpand = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr priceptr = new IntPtr(outputtype.drawline.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(priceptr, valuePrice, 0, 1);
                                    tempDrawLine.Price[j] = valuePrice[0];
                                    IntPtr expandptr = new IntPtr(outputtype.drawline.expand.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(expandptr, valueExpand, 0, 1);
                                    tempDrawLine.Expand[j] = valueExpand[0];
                                }
                                quoteDataStru.QuoteFunctionList = tempDrawLine;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWKLINE
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWKLINE:
                            using (DrawKlineOutput tempDrawKLine = new DrawKlineOutput())
                            {
                                tempDrawKLine.High = new double[dataCount];
                                tempDrawKLine.Open = new double[dataCount];
                                tempDrawKLine.Low = new double[dataCount];
                                tempDrawKLine.Close = new double[dataCount];
                                double[] valueHigh = new double[1];
                                double[] valueOpen = new double[1];
                                double[] valueLow = new double[1];
                                double[] valueClose = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr highptr = new IntPtr(outputtype.drawkline.high.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(highptr, valueHigh, 0, 1);
                                    tempDrawKLine.High[j] = valueHigh[0];
                                    IntPtr openptr = new IntPtr(outputtype.drawkline.open.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(openptr, valueOpen, 0, 1);
                                    tempDrawKLine.Open[j] = valueOpen[0];
                                    IntPtr lowptr = new IntPtr(outputtype.drawkline.low.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(lowptr, valueLow, 0, 1);
                                    tempDrawKLine.Low[j] = valueLow[0];
                                    IntPtr closeptr = new IntPtr(outputtype.drawkline.close.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(closeptr, valueClose, 0, 1);
                                    tempDrawKLine.Close[j] = valueClose[0];
                                }
                                quoteDataStru.QuoteFunctionList = tempDrawKLine;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_STICKLINE
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_STICKLINE:
                            using (StickLineOutput tempStickLine = new StickLineOutput())
                            {
                                tempStickLine.Price1 = new double[dataCount];
                                tempStickLine.Price2 = new double[dataCount];
                                tempStickLine.Width = new double[dataCount];
                                tempStickLine.Empty = new double[dataCount];
                                double[] valuePrice1 = new double[1];
                                double[] valuePrice2 = new double[1];
                                double[] valueWidth = new double[1];
                                double[] valueEmpty = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr price1ptr = new IntPtr(outputtype.stickline.price1.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(price1ptr, valuePrice1, 0, 1);
                                    tempStickLine.Price1[j] = valuePrice1[0];
                                    IntPtr price2ptr = new IntPtr(outputtype.stickline.price2.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(price2ptr, valuePrice2, 0, 1);
                                    tempStickLine.Price2[j] = valuePrice2[0];
                                    IntPtr widthptr = new IntPtr(outputtype.stickline.width.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(widthptr, valueWidth, 0, 1);
                                    tempStickLine.Width[j] = valueWidth[0];
                                    IntPtr emptyptr = new IntPtr(outputtype.stickline.empty.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(emptyptr, valueEmpty, 0, 1);
                                    tempStickLine.Empty[j] = valueEmpty[0];
                                }
                                quoteDataStru.QuoteFunctionList = tempStickLine;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWICON
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWICON:
                            using (DrawIconOutput tempDrawIcon = new DrawIconOutput())
                            {
                                tempDrawIcon.Price = new double[dataCount];
                                tempDrawIcon.Icon = new int[dataCount];
                                double[] valueIconPrice = new double[1];
                                double[] valueIcon = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr priceptr = new IntPtr(outputtype.drawicon.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(priceptr, valueIconPrice, 0, 1);
                                    tempDrawIcon.Price[j] = valueIconPrice[0];
                                    IntPtr iconptr = new IntPtr(outputtype.drawicon.icon.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(iconptr, valueIcon, 0, 1);
                                    tempDrawIcon.Icon[j] = Convert.ToInt32(valueIcon[0]);
                                }
                                quoteDataStru.QuoteFunctionList = tempDrawIcon;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWTEXT
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWTEXT:
                            using (DrawTextOutput tempDrawText = new DrawTextOutput())
                            {
                                tempDrawText.Price = new double[dataCount];
                                double[] valueTextPrice = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr priceptr = new IntPtr(outputtype.drawtext.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(priceptr, valueTextPrice, 0, 1);
                                    tempDrawText.Price[j] = valueTextPrice[0];
                                }
                                tempDrawText.Text = outputtype.drawtext.text;
                                quoteDataStru.QuoteFunctionList = tempDrawText;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWNUMBER
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWNUMBER:
                            using (DrawNumberOutput tempDrawNum = new DrawNumberOutput())
                            {
                                tempDrawNum.Price = new double[dataCount];
                                tempDrawNum.Number = new double[dataCount];
                                double[] valueNumPrice = new double[1];
                                double[] valueNumber = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr priceptr = new IntPtr(outputtype.drawnumber.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(priceptr, valueNumPrice, 0, 1);
                                    tempDrawNum.Price[j] = valueNumPrice[0];
                                    IntPtr numptr = new IntPtr(outputtype.drawnumber.number.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(numptr, valueNumber, 0, 1);
                                    tempDrawNum.Number[j] = valueNumber[0];
                                }
                                quoteDataStru.QuoteFunctionList = tempDrawNum;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWBAND
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWBAND:
                            using (DrawBandOutput tempDrawBand = new DrawBandOutput())
                            {
                                tempDrawBand.Price1 = new double[dataCount];
                                tempDrawBand.Price2 = new double[dataCount];
                                tempDrawBand.BandColor = new Color[dataCount];
                                double[] valueBandPrice1 = new double[1];
                                double[] valueBandPrice2 = new double[1];
                                double[] valueBandColor = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr price1ptr = new IntPtr(outputtype.drawband.price1.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(price1ptr, valueBandPrice1, 0, 1);
                                    tempDrawBand.Price1[j] = valueBandPrice1[0];
                                    IntPtr price2ptr = new IntPtr(outputtype.drawband.price2.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(price2ptr, valueBandPrice2, 0, 1);
                                    tempDrawBand.Price2[j] = valueBandPrice2[0];
                                    IntPtr bandcolorptr = new IntPtr(outputtype.drawband.color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(bandcolorptr, valueBandColor, 0, 1);
                                    tempDrawBand.BandColor[j] = Color.FromArgb(Convert.ToInt32(valueBandColor[0]));
                                }
                                quoteDataStru.QuoteFunctionList = tempDrawBand;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWFLOATRGN
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWFLOATRGN:
                            using (DrawFloatRGNOutput tempFloatRGN = new DrawFloatRGNOutput())
                            {
                                tempFloatRGN.Width = new double[dataCount];
                                tempFloatRGN.Price = new double[dataCount];
                                double[] valueRGNWidth = new double[1];
                                double[] valueRGNPrice = new double[1];
                                for (int i = 0; i < dataCount; i++)
                                {
                                    IntPtr widthptr = new IntPtr(outputtype.drawfloatrgn.width.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                                    Marshal.Copy(widthptr, valueRGNWidth, 0, 1);
                                    tempFloatRGN.Width[i] = valueRGNWidth[0];
                                    IntPtr priceptr = new IntPtr(outputtype.drawfloatrgn.price.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                                    Marshal.Copy(priceptr, valueRGNPrice, 0, 1);
                                    tempFloatRGN.Price[i] = valueRGNPrice[0];
                                }
                                tempFloatRGN.N = outputtype.drawfloatrgn.n;
                                tempFloatRGN.para = new FloatRGNPara[outputtype.drawfloatrgn.n];
                                for (int i = 0; i < outputtype.drawfloatrgn.n; i++)
                                {
                                    FloatRGNPara para = new FloatRGNPara();
                                    para.Cond = new int[dataCount];
                                    para.FloatRGNColor = new Color[dataCount];
                                    double[] valueCond = new double[1];
                                    double[] valueColor = new double[1];
                                    for (int j = 0; j < dataCount; j++)
                                    {
                                        try
                                        {
                                            IntPtr conPtr = new IntPtr(outputtype.drawfloatrgn.para[i].cond.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                            Marshal.Copy(conPtr, valueCond, 0, 1);
                                            para.Cond[j] = Convert.ToInt32(valueCond[0]);
                                            IntPtr colorPtr = new IntPtr(outputtype.drawfloatrgn.para[i].color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                            Marshal.Copy(colorPtr, valueColor, 0, 1);
                                            para.FloatRGNColor[j] = Color.FromArgb(Convert.ToInt32(valueColor[0]));
                                        }
                                        catch (Exception e)
                                        {
                                        }

                                    }
                                    tempFloatRGN.para[i] = para;
                                }
                                quoteDataStru.QuoteFunctionList = tempFloatRGN;
                            }
                                
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWTWR
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWTWR:
                            using (DrawTWROutput tempDrawTWR = new DrawTWROutput())
                            {
                                tempDrawTWR.data = new DrawTWRData[dataCount];
                                for (int i = 0; i < dataCount; i++)
                                {
                                    DrawTWRData tempdata = new DrawTWRData();
                                    DRAWTWR_DATA tempRawData;
                                    IntPtr rawdataptr = new IntPtr(outputtype.drawtwr.data.ToInt64() + Marshal.SizeOf(typeof(DRAWTWR_DATA)) * i);
                                    tempRawData = (DRAWTWR_DATA)Marshal.PtrToStructure(rawdataptr, typeof(DRAWTWR_DATA));
                                    tempdata.Up = Convert.ToChar(tempRawData.up);
                                    tempdata.Top = tempRawData.top;
                                    tempdata.Center = tempRawData.center;
                                    tempdata.Bottom = tempRawData.bottom;
                                    tempDrawTWR.data[i] = tempdata;
                                }
                                quoteDataStru.QuoteFunctionList = tempDrawTWR;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWFILLRGN
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWFILLRGN:
                            using (FillRGNOutput tempFillRGN = new FillRGNOutput())
                            {
                                tempFillRGN.Price1 = new double[dataCount];
                                tempFillRGN.Price2 = new double[dataCount];
                                tempFillRGN.FillRGNColor = new Color[dataCount];
                                double[] valueRGNPrice1 = new double[1];
                                double[] valueRGNPrice2 = new double[1];
                                double[] valueFillRGNColor = new double[1];
                                for (int i = 0; i < dataCount; i++)
                                {
                                    IntPtr price1ptr = new IntPtr(outputtype.drawfillrgn.price1.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                                    Marshal.Copy(price1ptr, valueRGNPrice1, 0, 1);
                                    tempFillRGN.Price1[i] = valueRGNPrice1[0];
                                    IntPtr price2ptr = new IntPtr(outputtype.drawfillrgn.price2.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                                    Marshal.Copy(price2ptr, valueRGNPrice2, 0, 1);
                                    tempFillRGN.Price2[i] = valueRGNPrice2[0];
                                    IntPtr colorptr = new IntPtr(outputtype.drawfillrgn.color.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                                    Marshal.Copy(colorptr, valueFillRGNColor, 0, 1);
                                    tempFillRGN.FillRGNColor[i] = Color.FromArgb(Convert.ToInt32(valueFillRGNColor[0]));
                                }
                                tempFillRGN.N = outputtype.drawfillrgn.n;
                                tempFillRGN.Para = new FillRGNPara[outputtype.drawfillrgn.n];
                                for (int i = 0; i < outputtype.drawfillrgn.n; i++)
                                {
                                    FillRGNPara para = new FillRGNPara();
                                    para.Cond = new int[dataCount];
                                    para.FillRGNColor = new Color[dataCount];
                                    double[] valueCond = new double[1];
                                    double[] valueColor = new double[1];
                                    for (int j = 0; j < dataCount; j++)
                                    {
                                        IntPtr conPtr = new IntPtr(outputtype.drawfillrgn.para[i].cond.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                        Marshal.Copy(conPtr, valueCond, 0, 1);
                                        para.Cond[j] = Convert.ToInt32(valueCond[0]);
                                        IntPtr colorPtr = new IntPtr(outputtype.drawfillrgn.para[i].color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                        Marshal.Copy(colorPtr, valueColor, 0, 1);
                                        para.FillRGNColor[j] = Color.FromArgb(Convert.ToInt32(valueColor[0]));
                                    }
                                    tempFillRGN.Para[i] = para;
                                }
                                quoteDataStru.QuoteFunctionList = tempFillRGN;
                            }
                            break;
                        #endregion

                        #region OUTPUT_TYPE_DRAWGBK
                        case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWGBK:
                            using (DrawGBKOutput tempGBK = new DrawGBKOutput())
                            {
                                tempGBK.DrawGBKColor = new Color[dataCount];
                                double[] valueGBKColor = new double[1];
                                for (int j = 0; j < dataCount; j++)
                                {
                                    IntPtr gbkcolorptr = new IntPtr(outputtype.drawgbk.color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    Marshal.Copy(gbkcolorptr, valueGBKColor, 0, 1);
                                    tempGBK.DrawGBKColor[j] = Color.FromArgb(Convert.ToInt32(valueGBKColor[0]));
                                }
                                quoteDataStru.QuoteFunctionList = tempGBK;
                            }
                            break;
                        #endregion    
                    }
                }
                catch (Exception e)
                {

                }
                if (index == selectIndex)
                    quoteDataStru.FlagQuoteDataSelected = true;
                indicator.QuoteData.Add(quoteDataStru);
            }
        }
        
        private static PicType GetFunctionQuotePicType(FORMULA_FUNCTION_OUTPUT_TYPE linetype)
        {
            PicType result = PicType.TrendLine;
            switch (linetype)
            {
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_POLYLINE:
                    result = PicType.PolyLine;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWLINE:
                    result = PicType.DrawLine;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWKLINE:
                    result = PicType.DrawKLine;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_STICKLINE:
                    result = PicType.StickLine;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWICON:
                    result = PicType.DrawIcon;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWTEXT:
                    result = PicType.DrawText;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWNUMBER:
                    result = PicType.DrawNumber;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWBAND:
                    result = PicType.DrawBand;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWFLOATRGN:
                    result = PicType.DrawFloatRGN;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWTWR:
                    result = PicType.DrawTWR;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWFILLRGN:
                    result = PicType.FillRGN;
                    break;
                case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWGBK:
                    result = PicType.DrawGBK;
                    break;
            }
            return result;
        }

        private static void GetNormalFormulaOutput(EMIndicator indicator, FmFormulaOutput output, 
            int dataCount, int index, int selectIndex,
            MarketType Market, KLineCycle KLineCycle)
        {
            String nameTemp = output.fmOutput[index].name.Trim('\0');
            List<float> outputList = new List<float>(0);
            double[] tempOutput = new double[1];
            for (int j = 0; j < dataCount; j++)
            {
                IntPtr prt = new IntPtr(output.fmOutput[index].normaloutput.ToInt64() + sizeof(double) * j);
                Marshal.Copy(prt, tempOutput, 0, 1);
                float temp = Convert.ToSingle(tempOutput[0]);
                #region 数据特殊处理
                if ((float.IsInfinity(temp) || float.IsNaN(temp))
                && (indicator.IndicatorName == "VOL" || indicator.IndicatorName == "DDX" || indicator.IndicatorName == "DDY"
                || indicator.IndicatorName == "DDZ" || indicator.IndicatorName == "ZJBY" || indicator.IndicatorName == "ZJQS"))
                    temp = 0F;
                //对财富通服务器成交量进行特殊处理
                if (indicator.IndicatorName == "VOL")
                    temp = temp * 100F;
                else if (indicator.IndicatorName == "ZJQS")
                    temp = temp / 10000F;
                #endregion
                outputList.Add(temp);
            }
            using (QuoteDataStru quoteDataStru = new QuoteDataStru())
            {
                quoteDataStru.QuoteName = nameTemp;
                quoteDataStru.QuoteDataList = outputList;
                if (index == selectIndex)
                    quoteDataStru.FlagQuoteDataSelected = true;
                //根据FORMULA_LINETYPE获取对应的线型
                quoteDataStru.QuotePicType = GetQuotePicType(output.fmOutput[index].dec.linetype);
                //为指标赋绘图颜色
                if (Color.FromArgb((int)output.fmOutput[index].dec.clr) != Color.FromArgb(255, 255, 255))
                    quoteDataStru.QuoteColor = Color.FromArgb(Convert.ToInt32(output.fmOutput[index].dec.clr));
                else
                    quoteDataStru.QuoteColor = IndicatorColors[index]; 
                indicator.QuoteData.Add(quoteDataStru);
            }
        }

        private static PicType GetQuotePicType(FORMULA_LINETYPE linetype)
        {
            PicType result = PicType.TrendLine;
            if (linetype == FORMULA_LINETYPE.LINETYPE_DEFAULT)
                result = PicType.TrendLine;
            if (linetype == FORMULA_LINETYPE.LINETYPE_LINESTICK)
                result = PicType.LineStick;
            if (linetype == FORMULA_LINETYPE.LINETYPE_DOTLINE)
                result = PicType.DotLine;
            if (linetype == FORMULA_LINETYPE.LINETYPE_COLORSTICK)
                result = PicType.MACDLine;
            if (linetype == FORMULA_LINETYPE.LINETYPE_VOLSTICK)
                result = PicType.VolumeLine;
            if (linetype == FORMULA_LINETYPE.LINETYPE_COLOR3D)
                result = PicType.COLOR3D;
            return result;
        }

        /// <summary>
        /// 沪深市场k线成交量处理
        /// </summary>
        /// <param name="tempVolume"></param>
        /// <param name="tempMarket"></param>
        /// <param name="tempCycle"></param>
        /// <returns></returns>
        private static float GetRealVoumeByMarket( MarketType tempMarket, KLineCycle tempCycle)
        {
            float result = 1;
            //股票：日线正常，周月季年K线小1万倍
            if ((SecurityAttribute.SHL1MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SHRepurchaseLevel1)
            || (SecurityAttribute.SHL2MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SHRepurchaseLevel2)
            || (SecurityAttribute.SZL1MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SZRepurchaseLevel1)
            || (SecurityAttribute.SZL2MarketType.ContainsKey(tempMarket) && tempMarket != MarketType.SZRepurchaseLevel2))
            {
                if ((int)tempCycle > (int)KLineCycle.CycleDay)
                    result = 10000;
            }
            //沪深指数：日K线小100倍，周月季年k线小100万倍
            if (tempMarket == MarketType.SHINDEX || tempMarket == MarketType.SZINDEX)
            {
                if ((int)tempCycle > (int)KLineCycle.CycleDay)
                    result =1000000;
                else if ((int)tempCycle == (int)KLineCycle.CycleDay)
                    result = 100;
            }

            //沪深交易所回购：分钟线日线缩小100，周月季年扩大100
            if (tempMarket == MarketType.SHRepurchaseLevel1 || tempMarket == MarketType.SHRepurchaseLevel2
                || tempMarket == MarketType.SZRepurchaseLevel1 || tempMarket == MarketType.SZRepurchaseLevel2)
            {
                if ((int)tempCycle > (int)KLineCycle.CycleDay)
                    result = 100;
                else if ((int)tempCycle <= (int)KLineCycle.CycleDay)
                    result = 1F /100F;
            }
            return result;
        }

        /// <summary>
        /// 计算复权，只改变复权因子
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="isCycleYear"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        private static float[] CaculateDivideKLineData(int code, List<OneDayDataRec> data, bool isCycleYear, bool isForward)
        {
            List<List<OneDivideRightBase>> divideData = Dc.GetDivideRightData(code);
            if (divideData == null || data==null)
                return null;
            float[] ArrResult = new float[data.Count];
            float factor = 1;
            for (int index = 0; index < data.Count;index++ )
            {
                if (index < ArrResult.Length)
                    ArrResult[index] = factor;
            }
            if (divideData.Count == 0)
                return ArrResult;
            for (int i = 0; i < divideData.Count; i++)
                factor *= divideData[i][0].Factor;

            int indexLast = 0;
            if (isForward)
            {
                indexLast = -1;
                for (int i = 0; i < divideData.Count; i++)
                {
                    if (factor == 0 || factor == 1)
                        continue;
                    for (int j = indexLast + 1; j < data.Count; j++)
                    {
                        if (j>= ArrResult.Length)
                            continue;
                        if (data == null || divideData == null || data[j]==null
                            || divideData[i] == null || divideData[i][0]==null)
                        {
                            int s = 0;
                        }
                        if (data[j].Date < divideData[i][0].Date)
                        {
                            ArrResult[j] = factor;
                            indexLast = j;
                        }
                        else
                        {
                            if (i == divideData.Count - 1)
                                ArrResult[j] = 1.0f;
                            else
                                break;
                        }
                    }
                    factor /= divideData[i][0].Factor;
                }
            }
            else
            {
                indexLast = data.Count;
                for (int i = divideData.Count - 1; i >= 0; i--)
                {
                    if (divideData[i][0].Factor == 0 || divideData[i][0].Factor == 1)
                        continue;

                    for (int j = indexLast - 1; j >= 0; j--)
                    {
                        if (j >= ArrResult.Length)
                            continue;
                        if (data[j].Date >= divideData[i][0].Date)
                        {
                            ArrResult[j] = factor;
                            indexLast = j;
                        }
                        else
                        {
                            if (i == 0)
                                ArrResult[j] = 1.0f;
                            else
                                break;
                        }
                    }
                    factor /= divideData[i][0].Factor;
                }
            }

            return ArrResult;
        }
        #endregion
    }
}
