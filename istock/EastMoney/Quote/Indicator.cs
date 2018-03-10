using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using EmQComm.Formula;
using System.Drawing.Drawing2D;

namespace EmQInd {
	/// <summary>
	/// 指标基类
	/// </summary>
	public class Indicator
    {
	    private bool _flagIndicatorSelected;
	    private Formulatype _formtype;
	    private string _indicatorName;
	    private int _paraCount;
	    private FormulaPara[] _paraData;
	    private string[] _paraTipName;
	    private PicLocationType _picLoc;
	    private CoordinateType _picCor;
	    private List<QuoteDataStru> _quoteData;
	    private Region _indicatorRegion;
	    private Formula _formula;

	    /// <summary>
        /// 指标类型
        /// </summary>
	    public Formulatype Formtype
	    {
	        get { return _formtype; }
	        set { _formtype = value; }
	    }

	    /// <summary>
	    /// 类型
	    /// </summary>
	    public string IndicatorName
	    {
	        get { return _indicatorName; }
	        set { _indicatorName = value; }
	    }

	    /// <summary>
        /// 参数个数
        /// </summary>
        public int ParaCount
	    {
	        set { _paraCount = value; }
	        get { return _paraCount; }
	    }

	    /// <summary>
        /// 参数数据
        /// </summary>
        public FormulaPara[] ParaData
	    {
	        get { return _paraData; }
	        set { _paraData = value; }
	    }

	    /// <summary>
        /// 设置参数时,值后显示内容
        /// </summary>
        public string[] ParaTipName
	    {
	        get { return _paraTipName; }
	        set { _paraTipName = value; }
	    }

	    /// <summary>
        /// 图像定位类型
        /// </summary>
        public PicLocationType PicLoc
	    {
	        get { return _picLoc; }
	        set { _picLoc = value; }
	    }

	    /// <summary>
        /// 坐标类型
        /// </summary>
        public CoordinateType PicCor
	    {
	        get { return _picCor; }
	        set { _picCor = value; }
	    }

	    /// <summary>
        /// 指标数据
        /// </summary>
        public List<QuoteDataStru> QuoteData
	    {
	        get { return _quoteData; }
	        set { _quoteData = value; }
	    }

	    /// <summary>
        /// 指标路径（主要用于K线指标）
        /// </summary>
        public Region IndicatorRegion
	    {
	        get { return _indicatorRegion; }
	        set { _indicatorRegion = value; }
	    }

	    /// <summary>
        /// 指标是否选中
        /// </summary>
        public bool FlagIndicatorSelected
        {
            get { return _flagIndicatorSelected; }
            set { _flagIndicatorSelected = value; }
        }

	    /// <summary>
        /// 公式
        /// </summary>
        public Formula Formula
	    {
	        get { return _formula; }
	        set { _formula = value; }
	    }

	    /// <summary>
        /// 构造函数
        /// </summary>
        public Indicator(Formulatype formtype, string indicatorName)
        {
            IndicatorRegion = new Region();
            QuoteData=new List<QuoteDataStru>();
            Formtype = formtype;
            IndicatorName = indicatorName;
            Formula tempFormla = new Formula();
            FormulaProxy.GetDbFormulaByName((int)Formtype, IndicatorName, ref tempFormla);
            Formula = tempFormla;
        }

        public Indicator(Formula f)
        {
            Formula = f;
            IndicatorRegion = new Region();
            QuoteData=new List<QuoteDataStru>();
            IndicatorName = f.name;
            Formtype = (Formulatype)f.type;
            PicLoc = (PicLocationType)f.drawtype;
        }

        public Indicator()
        {
            IndicatorRegion = new Region();
            QuoteData = new List<QuoteDataStru>();
        }

        public FormulaPara[] ParaDataDeepCopy()
        {
            FormulaPara[] result = new FormulaPara[ParaData.Length];
            for (int i = 0; i < ParaData.Length; i++)
            {
                result[i] = ParaData[i].DeepCopy();
            }
            return result;
        }
	}

    /// <summary>
    /// 图像定位类型
    /// </summary>
    public enum PicLocationType
    {
        /// <summary>
        /// 主图
        /// </summary>
        MasterMap = 0,
        /// <summary>
        /// 副图
        /// </summary>
        QuoteChart = 1
    }
    /// <summary>
    /// 指标类型
    /// </summary>
    public enum Formulatype
    {
        /// <summary>
        /// 技术指标
        /// </summary>
        TechniqueIndicator=0,
        /// <summary>
        /// 条件选股
        /// </summary>
        EquityScreener =1,
        /// <summary>
        /// 交易系统
        /// </summary>
        TradingSystem=2,
        /// <summary>
        /// 五彩k线
        /// </summary>
        MultiColorKline=3,
        /// <summary>
        /// k线
        /// </summary>
        Kline=4
    }

    /// <summary>
    /// 坐标类型
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// 相对坐标
        /// </summary>
        RelativeCor = 0,
        /// <summary>
        ///绝对坐标 
        /// </summary>
         AbsoluteCor= 1
    }

    /// <summary>
    /// 图像类型
    /// </summary>
    public enum PicType
    {
        /// <summary>
        ///蜡烛线 
        /// </summary>
        CandleLine=1,
        /// <summary>
        /// 美国线
        /// </summary>
        AmericanLine=2,
        /// <summary>
        /// 收盘线
        /// </summary>
        ClosingLine=3,
        /// <summary>
        /// 走势线
        /// </summary>
        TrendLine=4,
        /// <summary>
        /// 加粗线
        /// </summary>
        LineStick=5,
        /// <summary>
        /// 点状线
        /// </summary>
        DotLine=6,
        /// <summary>
        /// 成交量柱状线
        /// </summary>
        VolumeLine=7,
        /// <summary>
        /// MACD直线类型
        /// </summary>
        MACDLine= 8,
        /// <summary>
        /// COLOR3D
        /// </summary>
        COLOR3D=9,
        
        //画线函数输出
        PolyLine = 10,
        DrawLine = 11,
        DrawKLine = 12,
        StickLine=13,
        DrawIcon=14,
        DrawText=15,
        DrawNumber=16,
        DrawBand=17,
        DrawFloatRGN=18,
        DrawTWR=19,
        FillRGN=20,
        DrawGBK=21
    }

    /// <summary>
    /// 指标数据类型
    /// </summary>
    public class QuoteDataStru:IDisposable
    {
        private bool _flagQuoteDataSelected;
        private GraphicsPath _quoteDataStruPath;
        private Region _quoteDataStruRegion;
        private Color _quoteColor;
        private List<float> _quoteDataList;
        private PicType _quotePicType;
        private string _quoteName;
        private FormulaFunctionOutput _quoteFunctionList;

        /// <summary>
        /// 名称
        /// </summary>
        public string QuoteName
        {
            get { return _quoteName; }
            set { _quoteName = value; }
        }

        /// <summary>
        /// 详细数据
        /// </summary>
        public List<float> QuoteDataList
        {
            get { return _quoteDataList; }
            set { _quoteDataList = value; }
        }
        /// <summary>
        /// 详细特殊类型指标数据
        /// </summary>
        public FormulaFunctionOutput QuoteFunctionList 
        {
            get { return _quoteFunctionList; }
            set { _quoteFunctionList = value; }
        }

        /// <summary>
        /// 指标图像类型
        /// </summary>
        public PicType QuotePicType
        {
            get { return _quotePicType; }
            set { _quotePicType = value; }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color QuoteColor
        {
            get { return _quoteColor; }
            set { _quoteColor = value; }
        }

        /// <summary>
        /// 详细指标路径
        /// </summary>
        public GraphicsPath QuoteDataStruPath
        {
            get { return _quoteDataStruPath; }
            set { _quoteDataStruPath = value; }
        }

        /// <summary>
        /// 详细指标Region
        /// </summary>
        public Region QuoteDataStruRegion
        {
            get { return _quoteDataStruRegion; }
            set { _quoteDataStruRegion = value; }
        }

        /// <summary>
        /// 详细指标是否选中
        /// </summary>
        public bool FlagQuoteDataSelected
        {
            get { return _flagQuoteDataSelected; }
            set { _flagQuoteDataSelected = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuoteDataStru()
        {
            QuoteDataStruPath=new GraphicsPath();
            QuoteDataStruRegion=new Region();
            QuoteDataList=new List<float>();
            QuoteFunctionList = new FormulaFunctionOutput();
        }

        public void Dispose()
        {
        }
    }

}
