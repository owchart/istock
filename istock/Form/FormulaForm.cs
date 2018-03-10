using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using EmQComm.Formula;
using EmQComm;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using EmQDS.Data;
using EmQInd;

namespace dataquery
{
    /// <summary>
    /// 自选股窗体
    /// </summary>
    public partial class FormulaForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public FormulaForm()
        {
            InitializeComponent();
        }

        private static IList<Indicator> indicators = new List<Indicator>();

        /// <summary>
        /// 析构函数
        /// </summary>
        static FormulaForm()
        {
            FormulaProxy.FormulaInit();
            IList<Formula> formulas = FormulaProxy.GetSystemFormulas();
            int formulasSize = formulas.Count;
            for (int i = 0; i < formulasSize; i++)
            {
                indicators.Add(new Indicator(formulas[i]));
            }
        }

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

        /// <summary>
        /// 最新数据
        /// </summary>
        private static String dataText = "";

        private static String hisData = "";

        private static FuncTypeRealTime funcType;
        private static FuncTypeRealTime funcType2;

        public static void SetData(FuncTypeRealTime type, String data)
        {
            if (type == FuncTypeRealTime.StockTrend || type == FuncTypeRealTime.HisKLine)
            {
                hisData = data;
                funcType2 = type;
            }
            else
            {
                dataText = data;
                funcType = type;
            }
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (hisData.Length > 0)
            {
                List<OneDayDataRec> list = JsonConvert.DeserializeObject<List<OneDayDataRec>>(hisData);
                int listSize = list.Count;
                Indicator indicator = indicators[lbFormula.SelectedIndex];
                indicator.QuoteData.Clear();
                FORMULA_TIME begin, end;
                begin = new FORMULA_TIME();
                end = new FORMULA_TIME();
                int nlen = list.Count;
                Kline[] klines = new Kline[nlen];
                if (list == null || list.Count <= 0)
                    return;
                begin.year = (ushort)(list[0].Date / 10000);
                begin.month = (byte)((list[0].Date - begin.year * 10000) / 100);
                begin.day = (byte)(list[0].Date - begin.year * 10000 - begin.month * 100);
                end = new FORMULA_TIME();
                end.year = (ushort)(list[nlen - 1].Date / 10000);
                end.month = (byte)((list[nlen - 1].Date - end.year * 10000) / 100);
                end.day = (byte)(list[nlen - 1].Date - end.year * 10000 - end.month * 100);
                for (int i = 0; i < nlen; i++)
                {
                    klines[i].Date = list[i].Date;
                    //不复权
                    klines[i].Open = list[i].Open;
                    klines[i].Close = list[i].Close;
                    klines[i].High = list[i].High;
                    klines[i].Low = list[i].Low;
                    klines[i].Value = list[i].Amount;
                    klines[i].Volume = (uint)list[i].Volume;
                    klines[i].Time = list[i].Time;
                }
                string errmsg = string.Empty;
                FmFormulaOutput output = new FmFormulaOutput();
                Dictionary<int, Dictionary<FieldIndex, string>> dict = DetailData.FieldIndexDataString;
                //执行调用指标公式方法
                if (indicator.Formula.fid <= 0)
                {
                    FormulaProxy.ExecuteFormula(list[0].Date, (int)KLINEPERIOD.PERIOD_DAY, 1, txtCode.Text, begin,
                                                                end, 0, klines, indicator.IndicatorName, errmsg, ref output, nlen);
                }
                else
                {
                    FormulaProxy.ExecuteFormulaWithKLineAndFormula(list[0].Date,
                                                                   (int)KLINEPERIOD.PERIOD_DAY, 1, txtCode.Text, begin,
                                                                   end, klines, indicator.Formula, errmsg, ref output, nlen);
                }

                
                for (int n = 0; n < output.outputCount; n++)
                {
                    if (output.fmOutput[n].normaloutput.ToInt64() != 0)
                        GetNormalFormulaOutput(indicator, output, nlen, n, 0, EmQComm.Formula.MarketType.MARKET_SZ, KLineCycle.CycleDay);
                    else
                        GetFunctionFormulaOutput(indicator, output, nlen, n, 0);
                }
                DataTable dt = new DataTable(indicator.IndicatorName);
                foreach (QuoteDataStru stu in indicator.QuoteData)
                {
                    dt.Columns.Add(stu.QuoteName);
                }
                int idx = indicator.QuoteData[0].QuoteDataList.Count;
                int ix = indicator.QuoteData.Count;
                if (idx > 0)
                {
                    for (int i = 0; i < idx; i++)
                    {
                        DataRow row = dt.NewRow();
                        dt.Rows.Add(row);
                        for (int j = 0; j < ix; j++)
                        {
                            row[j] = indicator.QuoteData[j].QuoteDataList[i];
                        }
                    }
                }
                try
                {
                    dgvData.DataSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("1");
                }
                hisData = "";
            }
        }

        private static void GetNormalFormulaOutput(Indicator indicator, FmFormulaOutput output,
           int dataCount, int index, int selectIndex,
           EmQComm.Formula.MarketType Market, KLineCycle KLineCycle)
        {
            string nameTemp = output.fmOutput[index].name.Trim('\0');
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

        private void GetFunctionFormulaOutput(Indicator indicator, FmFormulaOutput output, int dataCount, int index, int selectIndex)
        {
            QuoteDataStru quoteDataStru = new QuoteDataStru();
            string nameTemp = output.fmOutput[index].name.Trim('\0');
            quoteDataStru.QuoteName = nameTemp;
            try
            {
                IntPtr prt = new IntPtr(output.fmOutput[index].foutput.ToInt64());
                FM_FORMULA_FUNCTION_OUTPUT outputtype = (FM_FORMULA_FUNCTION_OUTPUT)Marshal.PtrToStructure(prt, typeof(FM_FORMULA_FUNCTION_OUTPUT));
                quoteDataStru.QuotePicType = GetFunctionQuotePicType(outputtype.type);
                switch (outputtype.type)
                {
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_POLYLINE:
                        PolyLineOutput tempPolyLine = new PolyLineOutput();
                        tempPolyLine.Price = new double[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr priceptr = new IntPtr(outputtype.polyline.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempPolyLine.Price[j] = Convert.ToDouble(Marshal.PtrToStructure(priceptr, typeof(double)));
                        }
                        quoteDataStru.QuoteFunctionList = tempPolyLine;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWLINE:
                        DrawLineOutput tempDrawLine = new DrawLineOutput();
                        tempDrawLine.Price = new double[dataCount];
                        tempDrawLine.Expand = new double[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr priceptr = new IntPtr(outputtype.drawline.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawLine.Price[j] = Convert.ToDouble(Marshal.PtrToStructure(priceptr, typeof(double)));
                            IntPtr expandptr = new IntPtr(outputtype.drawline.expand.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawLine.Expand[j] = Convert.ToDouble(Marshal.PtrToStructure(expandptr, typeof(double)));
                        }
                        quoteDataStru.QuoteFunctionList = tempDrawLine;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWKLINE:
                        DrawKlineOutput tempDrawKLine = new DrawKlineOutput();
                        tempDrawKLine.High = new double[dataCount];
                        tempDrawKLine.Open = new double[dataCount];
                        tempDrawKLine.Low = new double[dataCount];
                        tempDrawKLine.Close = new double[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr highptr = new IntPtr(outputtype.drawkline.high.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawKLine.High[j] = Convert.ToDouble(Marshal.PtrToStructure(highptr, typeof(double)));
                            IntPtr openptr = new IntPtr(outputtype.drawkline.open.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawKLine.Open[j] = Convert.ToDouble(Marshal.PtrToStructure(openptr, typeof(double)));
                            IntPtr lowptr = new IntPtr(outputtype.drawkline.low.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawKLine.Low[j] = Convert.ToDouble(Marshal.PtrToStructure(lowptr, typeof(double)));
                            IntPtr closeptr = new IntPtr(outputtype.drawkline.close.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawKLine.Close[j] = Convert.ToDouble(Marshal.PtrToStructure(closeptr, typeof(double)));
                        }
                        quoteDataStru.QuoteFunctionList = tempDrawKLine;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_STICKLINE:
                        StickLineOutput tempStickLine = new StickLineOutput();
                        tempStickLine.Price1 = new double[dataCount];
                        tempStickLine.Price2 = new double[dataCount];
                        tempStickLine.Width = new double[dataCount];
                        tempStickLine.Empty = new double[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr price1ptr = new IntPtr(outputtype.stickline.price1.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempStickLine.Price1[j] = Convert.ToDouble(Marshal.PtrToStructure(price1ptr, typeof(double)));
                            IntPtr price2ptr = new IntPtr(outputtype.stickline.price2.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempStickLine.Price2[j] = Convert.ToDouble(Marshal.PtrToStructure(price2ptr, typeof(double)));
                            IntPtr widthptr = new IntPtr(outputtype.stickline.width.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempStickLine.Width[j] = Convert.ToDouble(Marshal.PtrToStructure(widthptr, typeof(double)));
                            IntPtr emptyptr = new IntPtr(outputtype.stickline.empty.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempStickLine.Empty[j] = Convert.ToDouble(Marshal.PtrToStructure(emptyptr, typeof(double)));
                        }
                        quoteDataStru.QuoteFunctionList = tempStickLine;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWICON:
                        DrawIconOutput tempDrawIcon = new DrawIconOutput();
                        tempDrawIcon.Price = new double[dataCount];
                        tempDrawIcon.Icon = new int[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr priceptr = new IntPtr(outputtype.drawicon.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawIcon.Price[j] = Convert.ToDouble(Marshal.PtrToStructure(priceptr, typeof(double)));
                            IntPtr iconptr = new IntPtr(outputtype.drawicon.icon.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawIcon.Icon[j] = Convert.ToInt32(Marshal.PtrToStructure(iconptr, typeof(double)));
                        }
                        quoteDataStru.QuoteFunctionList = tempDrawIcon;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWTEXT:
                        DrawTextOutput tempDrawText = new DrawTextOutput();
                        tempDrawText.Price = new double[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr priceptr = new IntPtr(outputtype.drawtext.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawText.Price[j] = Convert.ToDouble(Marshal.PtrToStructure(priceptr, typeof(double)));
                        }
                        tempDrawText.Text = outputtype.drawtext.text;
                        quoteDataStru.QuoteFunctionList = tempDrawText;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWNUMBER:
                        DrawNumberOutput tempDrawNum = new DrawNumberOutput();
                        tempDrawNum.Price = new double[dataCount];
                        tempDrawNum.Number = new double[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr priceptr = new IntPtr(outputtype.drawnumber.price.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawNum.Price[j] = Convert.ToDouble(Marshal.PtrToStructure(priceptr, typeof(double)));
                            IntPtr numptr = new IntPtr(outputtype.drawnumber.number.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawNum.Number[j] = Convert.ToDouble(Marshal.PtrToStructure(numptr, typeof(double)));
                        }
                        quoteDataStru.QuoteFunctionList = tempDrawNum;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWBAND:
                        DrawBandOutput tempDrawBand = new DrawBandOutput();
                        tempDrawBand.Price1 = new double[dataCount];
                        tempDrawBand.Price2 = new double[dataCount];
                        tempDrawBand.BandColor = new Color[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr price1ptr = new IntPtr(outputtype.drawband.price1.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawBand.Price1[j] = Convert.ToDouble(Marshal.PtrToStructure(price1ptr, typeof(double)));
                            IntPtr price2ptr = new IntPtr(outputtype.drawband.price2.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawBand.Price2[j] = Convert.ToDouble(Marshal.PtrToStructure(price2ptr, typeof(double)));
                            IntPtr bandcolorptr = new IntPtr(outputtype.drawband.color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempDrawBand.BandColor[j] = Color.FromArgb(Convert.ToInt32(Marshal.PtrToStructure(bandcolorptr, typeof(double))));
                        }
                        quoteDataStru.QuoteFunctionList = tempDrawBand;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWFLOATRGN:
                        DrawFloatRGNOutput tempFloatRGN = new DrawFloatRGNOutput();
                        tempFloatRGN.Width = new double[dataCount];
                        tempFloatRGN.Price = new double[dataCount];
                        for (int i = 0; i < dataCount; i++)
                        {
                            IntPtr widthptr = new IntPtr(outputtype.drawfloatrgn.width.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                            tempFloatRGN.Width[i] = Convert.ToDouble(Marshal.PtrToStructure(widthptr, typeof(double)));
                            IntPtr priceptr = new IntPtr(outputtype.drawfloatrgn.price.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                            tempFloatRGN.Price[i] = Convert.ToDouble(Marshal.PtrToStructure(priceptr, typeof(double)));
                        }
                        tempFloatRGN.N = outputtype.drawfloatrgn.n;
                        tempFloatRGN.para = new FloatRGNPara[outputtype.drawfloatrgn.n];
                        for (int i = 0; i < outputtype.drawfloatrgn.n; i++)
                        {
                            FloatRGNPara para = new FloatRGNPara();
                            para.Cond = new int[dataCount];
                            para.FloatRGNColor = new Color[dataCount];
                            for (int j = 0; j < dataCount; j++)
                            {
                                try
                                {
                                    IntPtr conPtr = new IntPtr(outputtype.drawfloatrgn.para[i].cond.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    para.Cond[j] = Convert.ToInt32(Marshal.PtrToStructure(conPtr, typeof(double)));
                                    IntPtr colorPtr = new IntPtr(outputtype.drawfloatrgn.para[i].color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                    para.FloatRGNColor[j] = Color.FromArgb(Convert.ToInt32(Marshal.PtrToStructure(colorPtr, typeof(double))));
                                }
                                catch (Exception e)
                                {
                                }

                            }
                            tempFloatRGN.para[i] = para;
                        }
                        quoteDataStru.QuoteFunctionList = tempFloatRGN;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWTWR:
                        DrawTWROutput tempDrawTWR = new DrawTWROutput();
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
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWFILLRGN:
                        FillRGNOutput tempFillRGN = new FillRGNOutput();

                        tempFillRGN.Price1 = new double[dataCount];
                        tempFillRGN.Price2 = new double[dataCount];
                        tempFillRGN.FillRGNColor = new Color[dataCount];
                        for (int i = 0; i < dataCount; i++)
                        {
                            IntPtr price1ptr = new IntPtr(outputtype.drawfillrgn.price1.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                            tempFillRGN.Price1[i] = Convert.ToDouble(Marshal.PtrToStructure(price1ptr, typeof(double)));
                            IntPtr price2ptr = new IntPtr(outputtype.drawfillrgn.price2.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                            tempFillRGN.Price2[i] = Convert.ToDouble(Marshal.PtrToStructure(price2ptr, typeof(double)));
                            IntPtr colorptr = new IntPtr(outputtype.drawfillrgn.color.ToInt64() + Marshal.SizeOf(typeof(double)) * i);
                            tempFillRGN.FillRGNColor[i] = Color.FromArgb(Convert.ToInt32(Marshal.PtrToStructure(colorptr, typeof(double))));
                        }
                        tempFillRGN.N = outputtype.drawfillrgn.n;
                        tempFillRGN.Para = new FillRGNPara[outputtype.drawfillrgn.n];
                        for (int i = 0; i < outputtype.drawfillrgn.n; i++)
                        {
                            FillRGNPara para = new FillRGNPara();
                            para.Cond = new int[dataCount];
                            para.FillRGNColor = new Color[dataCount];
                            for (int j = 0; j < dataCount; j++)
                            {
                                IntPtr conPtr = new IntPtr(outputtype.drawfillrgn.para[i].cond.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                para.Cond[j] = Convert.ToInt32(Marshal.PtrToStructure(conPtr, typeof(double)));
                                IntPtr colorPtr = new IntPtr(outputtype.drawfillrgn.para[i].color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                                para.FillRGNColor[j] = Color.FromArgb(Convert.ToInt32(Marshal.PtrToStructure(colorPtr, typeof(double))));
                            }
                            tempFillRGN.Para[i] = para;
                        }
                        quoteDataStru.QuoteFunctionList = tempFillRGN;
                        break;
                    case FORMULA_FUNCTION_OUTPUT_TYPE.OUTPUT_TYPE_DRAWGBK:
                        DrawGBKOutput tempGBK = new DrawGBKOutput();
                        tempGBK.DrawGBKColor = new Color[dataCount];
                        for (int j = 0; j < dataCount; j++)
                        {
                            IntPtr gbkcolorptr = new IntPtr(outputtype.drawgbk.color.ToInt64() + Marshal.SizeOf(typeof(double)) * j);
                            tempGBK.DrawGBKColor[j] = Color.FromArgb(Convert.ToInt32(Marshal.PtrToStructure(gbkcolorptr, typeof(double))));
                        }
                        quoteDataStru.QuoteFunctionList = tempGBK;
                        break;
                }

            }
            catch (Exception e)
            {

            }
            if (index == selectIndex)
                quoteDataStru.FlagQuoteDataSelected = true;
            indicator.QuoteData.Add(quoteDataStru);
        }

        /// <summary>
        /// 窗体加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormulaForm_Load(object sender, EventArgs e)
        {
            int indicatorsSize = indicators.Count;
            for (int i = 0; i < indicatorsSize; i++)
            {
                Indicator indicator = indicators[i];
                lbFormula.Items.Add(indicator.Formula.des.Trim() + "(" + indicator.IndicatorName + ")");
            }
        }

        /// <summary>
        /// 公式选中索引修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbFormula_SelectedIndexChanged(object sender, EventArgs e)
        {
            Formula formula = new Formula();
            FormulaProxy.GetDbFormula(lbFormula.SelectedIndex, ref formula);
            String text = Marshal.PtrToStringAnsi(formula.src);
            rtbText.Text = text;
        }

        /// <summary>
        /// 查询历史数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                //财富通历史数据
                OneStockHisKLineData oneStockHisKLineData = new OneStockHisKLineData(innerCode,
                    KLineCycle.CycleDay, 0, 0);
                oneStockHisKLineData._reqKLineDataRange = ReqKLineDataRange.All;
                oneStockHisKLineData.Start();
            }    
        }
    }
}
