using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OwLib {
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public struct FormulaPara {

		/// char[50]
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public String name;
		//[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		//public byte[] name;

		/// double 
		public double minvalue;

		/// double
		public double maxvalue;

		/// double
		public double defvalue;

		/// double
		public double step;

		/// double[20]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.R8)]
		public double[] uservalue;
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public FormulaPara DeepCopy()
        {
            FormulaPara result=new FormulaPara();
            result.name = this.name;
            result.minvalue = this.minvalue;
            result.maxvalue = this.maxvalue;
            result.defvalue = this.defvalue;
            result.step = this.step;
            double[] tempUserValue=new double[uservalue.Length];
            for(int i=0;i<uservalue.Length;i++)
                tempUserValue[i] = uservalue[i];
            result.uservalue = tempUserValue;
            return result;
        }
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public struct Formula {

		/// int
		public int fid;

		/// int
		public int type;

		/// char[50]
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public String name;

		/// char[100]
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public String des;

		/// int
		public int subtype;
		
		/// <summary>
		/// 0主图 1副图 2主图叠加
		/// </summary>
		public int drawtype;

		/// int
		public int ynum;

		/// double[10]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.R8)]
		public double[] y;

		/// int
		public int y2num;

		/// double[4]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.R8)]
		public double[] y2;

		/// char*
		public IntPtr password;

		/// char*
		public IntPtr src;

		/// char*
		public IntPtr paramtip;

		/// char*
		public IntPtr help;

		/// char*
		public IntPtr flag;


		/// int
		public int paracount;

		/// FormulaPara*
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public FormulaPara[] para;

		//public IntPtr para;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public struct TestFormulaResult {

		/// boolean
		[MarshalAs(UnmanagedType.I1)]
		public bool result;

		/// char[2048]
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
		public String message;
	}

    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FORMULA_TIME {

		/// WORD->unsigned short
		public ushort year;

		/// BYTE->unsigned char
		public byte month;

		/// BYTE->unsigned char
		public byte day;

		/// BYTE->unsigned char
		public byte hour;

		/// BYTE->unsigned char
		public byte minute;

		/// BYTE->unsigned char
		public byte second;
	}
    /// <summary>
    /// 
    /// </summary>
	public enum MarketType2 {

		/// MARKET_SZ -> 0
		MARKET_SZ = 0,

		/// MARKET_SH -> 1
		MARKET_SH = 1,
	}
    /// <summary>
    /// 
    /// </summary>
	public enum KLINEPERIOD {

		/// PERIOD_TICK -> 0
		PERIOD_TICK = 0,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_1MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_3MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_5MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_15MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_30MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_60MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_120MIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_ANYMIN,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_DAY,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_WEEK,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_MONTH,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_SEASON,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_HALFYEAR,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_YEAR,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_ANYDAY,
        /// <summary>
        /// 
        /// </summary>
		PERIOD_RT,
	}
    /// <summary>
    /// 
    /// </summary>
	public enum FORMULA_LINETYPE {

		/// LINETYPE_DEFAULT -> 0
		LINETYPE_DEFAULT = 0,

		/// LINETYPE_VOLSTICK -> 1
		LINETYPE_VOLSTICK = 1,

		/// LINETYPE_COLORSTICK -> 2
		LINETYPE_COLORSTICK = 2,

		/// LINETYPE_CIRCLEDOT -> 3
		LINETYPE_CIRCLEDOT = 3,

		/// LINETYPE_STICK -> 4
		LINETYPE_STICK = 4,

		/// LINETYPE_LINESTICK -> 5
		LINETYPE_LINESTICK = 5,

		/// LINETYPE_CROSSDOT -> 6
		LINETYPE_CROSSDOT = 6,

		/// LINETYPE_POINTDOT -> 7
		LINETYPE_POINTDOT = 7,

		/// LINETYPE_DOTLINE -> 8
		LINETYPE_DOTLINE = 8,

		/// LINETYPE_COLOR3D -> 9
		LINETYPE_COLOR3D = 9,

		/// LINETYPE_NODRAW -> 100
		LINETYPE_NODRAW = 100,
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct DECORATION {

		/// FORMULA_LINETYPE
		public FORMULA_LINETYPE linetype;

		/// COLORREF->DWORD->unsigned int
		public uint clr;

		/// int
		public int linewidth;
	}
    /// <summary>
    /// 
    /// </summary>
	public enum FORMULA_FUNCTION_OUTPUT_TYPE {

		/// OUTPUT_TYPE_POLYLINE -> 0
		OUTPUT_TYPE_POLYLINE = 0,

		/// OUTPUT_TYPE_DRAWLINE -> 1
		OUTPUT_TYPE_DRAWLINE = 1,

		/// OUTPUT_TYPE_DRAWKLINE -> 2
		OUTPUT_TYPE_DRAWKLINE = 2,

		/// OUTPUT_TYPE_STICKLINE -> 3
		OUTPUT_TYPE_STICKLINE = 3,

		/// OUTPUT_TYPE_DRAWICON -> 4
		OUTPUT_TYPE_DRAWICON = 4,

		/// OUTPUT_TYPE_DRAWTEXT -> 5
		OUTPUT_TYPE_DRAWTEXT = 5,

		/// OUTPUT_TYPE_DRAWNUMBER -> 6
		OUTPUT_TYPE_DRAWNUMBER = 6,

		/// OUTPUT_TYPE_DRAWBAND -> 7
		OUTPUT_TYPE_DRAWBAND = 7,

		/// OUTPUT_TYPE_DRAWBARCHART -> 8
		OUTPUT_TYPE_DRAWBARCHART = 8,

		/// OUTPUT_TYPE_DRAWFLOATRGN -> 9
		OUTPUT_TYPE_DRAWFLOATRGN = 9,

		/// OUTPUT_TYPE_DRAWTWR -> 10
		OUTPUT_TYPE_DRAWTWR = 10,

		/// OUTPUT_TYPE_DRAWFILLRGN -> 11
		OUTPUT_TYPE_DRAWFILLRGN = 11,

		/// OUTPUT_TYPE_DRAWGBK -> 12
		OUTPUT_TYPE_DRAWGBK = 12,

        OUTPUT_TYPE_HORILINE = 13,          
        OUTPUT_TYPE_DRAWFLAGTEXT = 14,
        OUTPUT_TYPE_DRAWMOVETEXT = 15,
        OUTPUT_TYPE_DRAWTEXTABS = 16,
        OUTPUT_TYPE_DRAWTEXTREL = 17,  
        OUTPUT_TYPE_PARTLINE = 18,
        OUTPUT_TYPE_PERCENTBAR = 19,
        OUTPUT_TYPE_VERTLINE = 20,
        OUTPUT_TYPE_FLOATSTICK = 21,
        OUTPUT_TYPE_DRAWRECTABS = 22,
        OUTPUT_TYPE_DRAWRECTREL = 23,
        OUTPUT_TYPE_DRAWURL = 24,
        OUTPUT_TYPE_DRAWTEXT_FIX = 25,
        OUTPUT_TYPE_DRAWBMP = 26,
        OUTPUT_TYPE_STRIP = 27,
        OUTPUT_TYPE_DRAWNUMBER_FIX = 28,
		/// OUTPUT_TYPE_NULL -> 55
		OUTPUT_TYPE_NULL = 55,
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_POLYLINE_OUTPUT {

		/// double*
		public System.IntPtr price;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWLINE_OUTPUT {

		/// double*
		public System.IntPtr price;

		/// double*
		public System.IntPtr expand;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWKLINE_OUTPUT {

		/// double*
		public System.IntPtr high;

		/// double*
		public System.IntPtr open;

		/// double*
		public System.IntPtr low;

		/// double*
		public System.IntPtr close;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_STICKLINE_OUTPUT {

		/// double*
		public System.IntPtr price1;

		/// double*
		public System.IntPtr price2;

		/// double*
		public System.IntPtr width;

		/// double*
		public System.IntPtr empty;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWICON_OUTPUT {

		/// double*
		public System.IntPtr price;

		/// double*
		public System.IntPtr icon;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWTEXT_OUTPUT {

		/// double*
		public System.IntPtr price;

		/// char*
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)]
		public String text;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack =1)]
	public struct FORMULA_DRAWNUMBER_OUTPUT {

		/// double*
		public System.IntPtr price;

		/// double*
		public System.IntPtr number;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWBAND_OUTPUT {

		/// double*
		public System.IntPtr price1;

		/// double*
		public System.IntPtr price2;

		/// double*
		public System.IntPtr color;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FLOATRGN_PARA {

		/// double*
		public System.IntPtr cond;

		/// double*
		public System.IntPtr color;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWFLOATRGN_OUTPUT {

		/// double*
		public System.IntPtr price;

		/// double*
		public System.IntPtr width;

		/// FLOATRGN_PARA[20]
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
		public FLOATRGN_PARA[] para;

		/// int
		public int n;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack =1)]
	public struct DRAWTWR_DATA {

		/// char
		public byte up;

		/// double
		public double top;

		/// double
		public double center;

		/// double
		public double bottom;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWTWR_OUTPUT {

		/// DRAWTWR_DATA*
		public System.IntPtr data;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FILLRGN_PARA {

		/// double*
		public System.IntPtr cond;

		/// double*
		public System.IntPtr color;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_FILLRGN_OUTPUT {

		/// double*
		public System.IntPtr price1; 

		/// double*
		public System.IntPtr price2;

		/// FILLRGN_PARA[20]
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
		public FILLRGN_PARA[] para;

		/// double*
		public System.IntPtr color;

		/// int
		public int n;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct FORMULA_DRAWGBK_OUTPUT {

		/// double*
		public System.IntPtr color;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FM_FORMULA_FUNCTION_OUTPUT {

		/// FORMULA_FUNCTION_OUTPUT_TYPE
		public FORMULA_FUNCTION_OUTPUT_TYPE type;

		/// FORMULA_POLYLINE_OUTPUT
		public FORMULA_POLYLINE_OUTPUT polyline;

		/// FORMULA_DRAWLINE_OUTPUT
		public FORMULA_DRAWLINE_OUTPUT drawline;

		/// FORMULA_DRAWKLINE_OUTPUT
		public FORMULA_DRAWKLINE_OUTPUT drawkline;

		/// FORMULA_STICKLINE_OUTPUT
		public FORMULA_STICKLINE_OUTPUT stickline;

		/// FORMULA_DRAWICON_OUTPUT
		public FORMULA_DRAWICON_OUTPUT drawicon;

		/// FORMULA_DRAWTEXT_OUTPUT
		public FORMULA_DRAWTEXT_OUTPUT drawtext;

		/// FORMULA_DRAWNUMBER_OUTPUT
		public FORMULA_DRAWNUMBER_OUTPUT drawnumber;

		/// FORMULA_DRAWBAND_OUTPUT
		public FORMULA_DRAWBAND_OUTPUT drawband;

		/// FORMULA_DRAWFLOATRGN_OUTPUT
		public FORMULA_DRAWFLOATRGN_OUTPUT drawfloatrgn;

		/// FORMULA_DRAWTWR_OUTPUT
		public FORMULA_DRAWTWR_OUTPUT drawtwr;

		/// FORMULA_FILLRGN_OUTPUT
		public FORMULA_FILLRGN_OUTPUT drawfillrgn;

		/// FORMULA_DRAWGBK_OUTPUT
		public FORMULA_DRAWGBK_OUTPUT drawgbk;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FM_FORMULA_OUTPUT {

		/// FORMULA_TIME*
		public System.IntPtr t;

        /// <summary>
        /// 名称
        /// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
		public String name;
		/// DECORATION
		public DECORATION dec;

		/// double*
		public System.IntPtr normaloutput;

		/// FM_FORMULA_FUNCTION_OUTPUT*
		public System.IntPtr foutput;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FmFormulaOutput {
		/// int
		public int outputCount;

		/// FM_FORMULA_OUTPUT[20]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public FM_FORMULA_OUTPUT[] fmOutput;
	}
    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Kline {
		/// int
		public int Date;
		/// int
		public int Time;
		/// float
		public float Open;
		/// float
		public float Close;
		/// float
		public float High;
		/// float
		public float Low;
        /// double
		public double Volume;
		/// double
		public double Value;
	}

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct FM_FORMULA_EXTRA_CONST {

        /// <summary>
        /// 表达式的名称
        /// </summary>
        /// char*
        public IntPtr Name;

        /// <summary>
        /// 表达式值的数组
        /// </summary>
        public IntPtr pValue;

        /// <summary>
        /// 值数组的长度
        /// </summary>
        public int Length;
    }
}
