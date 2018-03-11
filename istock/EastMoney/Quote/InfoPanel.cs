using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OwLib {

	/// <summary>
	/// 信息栏布局类
	/// </summary>
	public class InfoPanelLayout {
        /// <summary>
        /// 
        /// </summary>
		public InfoPanelLayout() {
			Charts = new List<InfoPanelLayoutChart>();
		}
        private int _columns;
		/// <summary>
		/// 分栏数
		/// </summary>
        public int Columns { 
            get { return _columns; } 
            set { this._columns = value; }
        }

        private MarketType _market;
		/// <summary>
		/// 市场类型
		/// </summary>
        public MarketType Market { 
            get { return _market; }
            set { this._market = value; }
        }

        private float _topHalHeight;
		/// <summary>
		/// 上半部分的高度,大于1时为行数，上半部分固定高度
		/// </summary>
		public float TopHalfHeight {
            get { return _topHalHeight; }
            set { this._topHalHeight = value; }
        }

        private List<InfoPanelLayoutChart> _charts;
		/// <summary>
		/// 布局中的Chart
		/// </summary>
		public List<InfoPanelLayoutChart> Charts {
            get { return _charts; }
            set { this._charts = value; }
        }

        private InfoPanelLayoutTabs _bottomTab;
		/// <summary>
		/// 底部Tab
		/// </summary>
        public InfoPanelLayoutTabs BottomTab { 
            get { return _bottomTab; } 
            set { this._bottomTab = value; } 
        }

        private InfoPanelLayoutTabs _topTab;
		/// <summary>
		/// 顶部Tab
		/// </summary>
		public InfoPanelLayoutTabs TopTab {
            get { return _topTab; }
            set { this._topTab = value; }
        }

	}
    /// <summary>
    /// 
    /// </summary>
	public class InfoPanelLayoutChart{
        /// <summary>
        /// 构造函数
        /// </summary>
		public InfoPanelLayoutChart() {
			Location = InfoPanelLayoutChartLocation.Normal;
            IsSplitter = false;
		}

        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
		public string Name {
            get { return _name; }
            set { this._name = value; }
        }

        private float _height;

        /// <summary>
        /// 高度
        /// </summary>
        public float Height {
            get { return _height; }
            set { this._height = value; }
        }

        private int _colIndex;
		/// <summary>
		/// 所在列标示
		/// </summary>
		public int ColIndex {
            get { return _colIndex; }
            set { this._colIndex = value; }
        }

        private int _rowIndex;
		/// <summary>
		/// 所在行标示
		/// </summary>
		public int RowIndex {
            get { return _rowIndex; }
            set { this._rowIndex = value; }
        }

        private InfoPanelLayoutChartLocation _location;
		/// <summary>
		/// 位置
		/// </summary>
		public InfoPanelLayoutChartLocation Location {
            get { return _location; }
            set { this._location = value; }
        }

        private bool _isSpliter;

        /// <summary>
        /// 是否为分割线
        /// </summary>
        public bool IsSplitter {
            get { return _isSpliter; }
            set { this._isSpliter = value; }
        }
	}
    /// <summary>
    /// 
    /// </summary>
	public class InfoPanelLayoutChartComparer:IComparer<InfoPanelLayoutChart>{
        /// <summary>
        /// 所在行标记是否相同
        /// </summary>
		public int Compare(InfoPanelLayoutChart x, InfoPanelLayoutChart y){
			return x.RowIndex.CompareTo((y.RowIndex));
		}
	}
    /// <summary>
    /// 
    /// </summary>
	public enum InfoPanelLayoutChartLocation {
        /// <summary>
        /// 
        /// </summary>
		Normal,
        /// <summary>
        /// 
        /// </summary>
		TopTabChart,
        /// <summary>
        /// 
        /// </summary>
		BottomTabChart
	}

	/// <summary>
	/// 布局中的Tab控件
	/// </summary>
	public class InfoPanelLayoutTabs:InfoPanelLayoutChart {
        /// <summary>
        /// 构造函数
        /// </summary>
		public InfoPanelLayoutTabs() {
			Tabs = new List<InfoPanelLayoutTab>();
		}
        private List<InfoPanelLayoutTab> _tabs;
        /// <summary>
        /// tab集合
        /// </summary>
		public List<InfoPanelLayoutTab> Tabs {
            get { return _tabs; }
            set { this._tabs = value; }
        }

	}

	/// <summary>
	/// 布局中的Tab控件的Tab项
	/// </summary>
	public class InfoPanelLayoutTab {
        /// <summary>
        /// 构造函数
        /// </summary>
		public InfoPanelLayoutTab() {
			TabCharts = new List<InfoPanelLayoutChart>();
			VLine = false;
		}

        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
		public string Name {
            get { return _name; }
            set { this._name = value; }
        }

        private bool _isVline;
        /// <summary>
        /// 
        /// </summary>
		public bool VLine {
            get { return _isVline; }
            set { this._isVline = value; }
        }

        private List<InfoPanelLayoutChart> _tabCharts;
        /// <summary>
        /// 
        /// </summary>
		public List<InfoPanelLayoutChart> TabCharts {
            get { return _tabCharts; }
            set { this._tabCharts = value; }
        }
	}
    /// <summary>
    /// 
    /// </summary>
	public class InfoPanelChart {

        /// <summary>
        /// 构造函数
        /// </summary>
		public InfoPanelChart() {
			Rows = new List<InfoPanelChartRow>();
		}
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
		public string Name {
            get { return _name; }
            set { this._name = value; }
        }

        private List<InfoPanelChartRow> _rows;
        /// <summary>
        /// 行
        /// </summary>
		public List<InfoPanelChartRow> Rows {
            get { return _rows; }
            set { this._rows = value; }
        }
	}
    /// <summary>
    /// 
    /// </summary>
	public class InfoPanelChartRow {
        /// <summary>
        /// 构造函数
        /// </summary>
		public InfoPanelChartRow() {
			Colums = new List<InfoPanelChartColum>();
			Repeat = false;
			TopLine = false;
			BottomLine = false;
            LineColor = LineColor.Lite;
        }

        private bool _repeat;
        /// <summary>
        /// 
        /// </summary>
		public bool Repeat {
            get { return _repeat; }
            set { this._repeat = value; }
        }

        private bool _topLine;
		/// <summary>
		/// 上边框划线
		/// </summary>
		public bool TopLine {
            get { return _topLine; }
            set { this._topLine = value; }
        }

        private bool _bottomLine;
		/// <summary>
		/// 下边框划线
		/// </summary>
		public bool BottomLine {
            get { return _bottomLine; }
            set { this._bottomLine = value; }
        }

        private int _marginTop;
		/// <summary>
		/// 顶部边距
		/// </summary>
		public int MarginTop {
            get { return _marginTop; }
            set { this._marginTop = value; }
        }

        private int _marginBottom;
		/// <summary>
		/// 底部边距
		/// </summary>
		public int MarginBottom {
            get { return _marginBottom; }
            set { this._marginBottom = value; }
        }

        private LineColor _lineColor;
        /// <summary>
        /// 线条颜色
        /// </summary>
        public LineColor LineColor {
            get { return _lineColor; }
            set { this._lineColor = value; }
        }

        private IList<InfoPanelChartColum> _colums;
        /// <summary>
        /// 列
        /// </summary>
		public IList<InfoPanelChartColum> Colums {
            get { return _colums; }
            set { this._colums = value; }
        }
	}
    /// <summary>
    /// 
    /// </summary>
	public class InfoPanelChartColum{
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public InfoPanelChartColum(){
            ValueAlign = StringAlignment.Center;
            MarkLocation = MarkLocation.None;
            PaddingLeft = 0;
            PaddingRight = 0;
            MarginTop = 0;
            MarginBottom = 0;
        }

        private string _caption;
        /// <summary>
		/// 项名称
		/// </summary>
		public string Caption {
            get { return _caption; }
            set { this._caption = value; }
        }

        private StringAlignment _captionAlgin;
		/// <summary>
		/// 项名称对齐方式
		/// </summary>
		public StringAlignment CaptionAlgin {
            get { return _captionAlgin; }
            set { this._captionAlgin = value; }
        }

        private string _valueField;
		/// <summary>
		/// 值域
		/// </summary>
		public string ValueField {
            get { return _valueField; }
            set { this._valueField = value; }
        }

        private StringAlignment _valueAlgin;
		/// <summary>
		/// 值对齐方式
		/// </summary>
		public StringAlignment ValueAlign {
            get { return _valueAlgin; }
            set { this._valueAlgin = value; }
        }

        private float _x;
		/// <summary>
		/// X轴位置
		/// </summary>
		public float X {
            get { return _x; }
            set { this._x = value; }
        }

        private float _width;
		/// <summary>
		/// 宽度
		/// </summary>
		public float Width {
            get { return _width; }
            set { this._width = value; }
        }

        private int _marginTop;
		/// <summary>
		/// 顶部边距
		/// </summary>
		public int MarginTop {
            get { return _marginTop; }
            set { this._marginTop = value; }
        }

        private int _marginBottom;
		/// <summary>
		/// 底部边距
		/// </summary>
		public int MarginBottom {
            get { return _marginBottom; }
            set { this._marginBottom = value; }
        }

        private int _paddingLeft;
		/// <summary>
		/// 左边缩进
		/// </summary>
		public int PaddingLeft {
            get { return _paddingLeft; }
            set { this._paddingLeft = value; }
        }

        private int _paddingRight;
		/// <summary>
		/// 右边缩进
		/// </summary>
		public int PaddingRight {
            get { return _paddingRight; }
            set { this._paddingRight = value; }
        }

        private MarkLocation _markLocation;
        /// <summary>
        /// 角标位置
        /// </summary>
        public MarkLocation MarkLocation {
            get { return _markLocation; }
            set { this._markLocation = value; }
        }

        private string _markStr;
        /// <summary>
        /// 角标内容
        /// </summary>
        public string MarkValue{
            get{
                if (MarkLocation == MarkLocation.None){
                    return string.Empty;
                }
                return _markStr;
            }
            set { _markStr = value; }
        }
    }

    /// <summary>
    /// 角标位置
    /// </summary>
    public enum MarkLocation{
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 左上角
        /// </summary>
        TopLeft,
        /// <summary>
        /// 右上角
        /// </summary>
        TopRight,
        /// <summary>
        /// 左下角
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 右下角
        /// </summary>
        BottomRight
    }

    /// <summary>
    /// 线的颜色
    /// </summary>
    public enum LineColor{
        /// <summary>
        /// 深色
        /// </summary>
        Strong,
        /// <summary>
        /// 浅色
        /// </summary>
        Lite
    }

    /// <summary>
    /// 
    /// </summary>
	public enum InfoPanelChartHeightState {
        /// <summary>
        /// 
        /// </summary>
		Dynamic,
        /// <summary>
        /// 
        /// </summary>
		Fixed
	}


	/// <summary>
	/// 新闻研报项
	/// </summary>
	public class NewsReportItem {

        private string _date;
        /// <summary>
        /// 日期
        /// </summary>
		public string Date {
            get { return _date; }
            set { this._date = value; }
        }

        private string _category;
        /// <summary>
        /// 类别
        /// </summary>
		public string Category {
            get { return _category; }
            set { this._category = value; }
        }

        private string _title;
        /// <summary>
        /// 标题
        /// </summary>
		public string Title {
            get { return _title; }
            set { this._title = value; }
        }

        private string _content;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { 
            get { return _content; }
            set { this._content = value; }
        }
	}

    public class TopBannerMenuItem {
        public TopBannerMenuItem() {
            Caption = string.Empty;
            Url = string.Empty;
            UrlTitle = string.Empty;
            Width = 0;
            IsSelected = false;
        }

        private string _caption;
        public string Caption {
            get { return _caption; }
            set { this._caption = value; }
        }

        private string _url;
        public string Url {
            get { return _url; }
            set { this._url = value; }
        }

        private string _urlTitle;
        public string UrlTitle {
            get { return _urlTitle; }
            set { this._urlTitle = value; }
        }

        private int _width;
        public int Width {
            get { return _width; }
            set { this._width = value; }
        }

        private int _height;
        public int Height {
            get { return _height; }
            set { this._height = value; }
        }

        private Rectangle _contentRect;
        /// <summary>
        /// 绘图时使用
        /// </summary>
        public Rectangle ContentRectangle {
            get { return _contentRect; }
            set { this._contentRect = value;}
        }

        private bool _isSelected;

        public bool IsSelected {
            get { return _isSelected; }
            set { this._isSelected = value; }
        }
    }

    public class TopBannerMenuItemPair {

        public TopBannerMenuItemPair() {
            Item1 = new TopBannerMenuItem();
            Item2 = new TopBannerMenuItem();
        }

        private TopBannerMenuItem _item1;
        public TopBannerMenuItem Item1 {
            get { return _item1; }
            set { this._item1 = value; }
        }

        private TopBannerMenuItem _item2;
        public TopBannerMenuItem Item2 {
            get { return _item2; }
            set { this._item2 = value; }
        }
    }
}
