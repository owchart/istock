using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;

namespace EmQComm {
    /// <summary>
    /// 绘图管理
    /// </summary>
	public static class QuoteDrawService {
		#region 对其方式
		/// <summary>
		///   垂直居中，水平居中
		/// </summary>
		public static StringFormat StringFormat_VCenter_HCenter;

		/// <summary>
		///   垂直居中，水平居左
		/// </summary>
		public static StringFormat StringFormat_VCenter_HLeft;

		/// <summary>
		///   垂直居中，水平居右
		/// </summary>
		public static StringFormat StringFormat_VCenter_HRight;

		/// <summary>
		///   垂直居上，水平居中
		/// </summary>
		public static StringFormat StringFormat_VTop_HCenter;

		/// <summary>
		///   垂直居上，水平居左
		/// </summary>
		public static StringFormat StringFormat_VTop_HLeft;

		/// <summary>
		///   垂直居上，水平居右
		/// </summary>
		public static StringFormat StringFormat_VTop_HRight;

		/// <summary>
		/// 垂直居下，水平居中
		/// </summary>
		public static StringFormat StringFormat_VBottom_HCenter;

        /// <summary>
        /// 
        /// </summary>
		public static StringFormat StringFormat_VBottom_HLeft;

        /// <summary>
        /// 
        /// </summary>
		public static StringFormat StringFormat_VBottom_HRight;

		#endregion

		#region Pen
        /// <summary>
        /// 边框的颜色  灰色边框
        /// </summary>
		public static Pen PenFrame;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenRedName;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenAmount;
        /// <summary>
        /// 边框的颜色  灰色边框
        /// </summary>
		public static Pen PenFrameDash;
        /// <summary>
        /// 上涨显示颜色
        /// </summary>
		public static Pen PenFrameDashRed;
        /// <summary>
        /// 框架颜色 红色边框
        /// </summary>
		public static Pen PenBoard;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorCode;
        /// <summary>
        /// 框架颜色 红色边框
        /// </summary>
		public static Pen PenBoardDash;
        /// <summary>
        /// 成交量的颜色
        /// </summary>
		public static Pen PenVolume;
        /// <summary>
        /// 成交量的颜色
        /// </summary>
		public static Pen PenVolumeDash;
        /// <summary>
        /// 量比颜色
        /// </summary>
        public static Pen PenLiangbi;
        /// <summary>
        /// 平盘显示颜色
        /// </summary>
		public static Pen PenSame;
        /// <summary>
        /// 背景画笔
        /// </summary>
        public static Pen PenBackground;
        /// <summary>
        /// 上涨显示颜色
        /// </summary>
		public static Pen PenUpDash;
        /// <summary>
        /// 上涨显示颜色
        /// </summary>
		public static Pen PenUp;
        /// <summary>
        /// K线下跌的颜色
        /// </summary>
		public static Pen PenDownKLine;
        /// <summary>
        /// k线DrawBox颜色
        /// </summary>
        public static Pen PenKlineDrawBox;
        /// <summary>
        /// 十字光标的颜色
        /// </summary>
		public static Pen PenCross;
        /// <summary>
        /// 走势背景网格线颜色
        /// </summary>
		public static Pen PenTrendBackground;
        /// <summary>
        /// 走势背景网格线颜色
        /// </summary>
		public static Pen PenDashTrendBackground;
        /// <summary>
        /// 滚动条边框的颜色
        /// </summary>
		public static Pen PenColorBarLight;
        /// <summary>
        /// 当前选中的数据行
        /// </summary>
		public static Pen PenCurrentDataRowLine;

        /// <summary>
        /// 
        /// </summary>
		public static Pen PenNewsTitleBackUpLightColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenButtonBorder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenQuoteBorderColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenQuoteBorderDashColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenMmdSpliterColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenMmdSpliterDashColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenQuoteInnerSpliterColor;

        public static Pen PenQuoteInnerSpliterDashColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarBorderColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColumnSplitColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarArrowDownColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarArrowUpColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarBeginColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarEndColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenHideButtonHighLightColor1;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenHideButtonHighLightColor2;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenHideButtonHighLightColor3;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarThreeLineBeginColor;
        public static Pen PenBarThreeLineEndColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarThreeLineShadowColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenBarBackGroundColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenCommonButtonUpHighLightColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenCommonButtonDownLineColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenSelectedButtonUpHighLightColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenSelectedButtonDownLineColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenButtonSplitColor1;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenButtonSplitColor2;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenNewsTitleSplitColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenSelectedButtonSideColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenNormalTabButtonBounderColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenSelectedTextColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenSelectedOrangeColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenMultiSelectedBorderColor;
        /// <summary>
        /// 
        /// </summary>
		public static Pen PenVTabHighLightColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenVTabShadowColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenVTabBorderColor;

        public static Pen PenVTabActiveInnerBorderColor;

        public static Pen PenVTabActiveHighlightColor;
        /// <summary>
        /// 
        /// </summary>
		public static Pen PenKlineButtonFrameColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenKlineIndexButtonFrameColor;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenDealDetailFrameColor;
		//
		//订单占比
        /// <summary>
        /// 
        /// </summary>
		public static Pen PenColorBigBuyOrder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorMidBuyOrder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorSmallBuyOrder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorBigSellOrder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorMidSellOrder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorSmallSellOrder;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorInfopanelHighLight;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorInfopanelInnerTitleBkg;
        /// <summary>
        /// 
        /// </summary>
		public static Pen PenColorNewsSelected;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorNewsViewed;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenTrendFrame;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenTrendFrameDash;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenThickTrendFrame;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorF10Border;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorF10NormalText;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorF10SelectedBack;
        /// <summary>
        /// 
        /// </summary>
        public static Pen PenColorF10SelectedText;

		/// <summary>
		/// 新增
		/// </summary>
		public static Pen PenColorOrderQueueNew;
		/// <summary>
		/// 撤单
		/// </summary>
		public static Pen PenColorOrderQueueCancel;
		/// <summary>
		/// 部分成交填充色
		/// </summary>
		public static Pen PenColorOrderQueuePartDealFill;
		/// <summary>
		/// 部分成交边框
		/// </summary>
		public static Pen PenColorOrderQueuePartDealBorder;
		/// <summary>
		/// 超过1000手
		/// </summary>
		public static Pen PenColorOrderQueueKAbove;
		/// <summary>
		/// 超过5000手
		/// </summary>
		public static Pen PenColorOrderQueue5KAbove;
        /// <summary>
        /// 分割线
        /// </summary>
        public static Pen PenColorOrderQueueSpliter;
        /// <summary>
        /// 百档委托的分割线
        /// </summary>
        public static Pen PenColorDashOrderDetailSpliter;

        /// <summary>
        /// 股吧评论分割线色Pen
		/// </summary>
        public static Pen PenBBSSplitLineColor;
        /// <summary>
        /// 股吧按钮的边框颜色
        /// </summary>
        public static Pen PenBBSButtonBorderColor;
        /// <summary>
        /// 股吧按钮的边框高光颜色
        /// </summary>
        public static Pen PenBBSButtonBorderHighlightColor;
        /// <summary>
        /// 股吧按钮选中时边框高光颜色
        /// </summary>
        public static Pen PenBBSButtonMouseOnBorderHighlightColor;
		#endregion

		#region Brush
        /// <summary>
        /// 背景色
        /// </summary>
		public static SolidBrush BrushColorBackGround;
        /// <summary>
        /// 成交金额的颜色
        /// </summary>
        public static SolidBrush BrushColorAmount;
        /// <summary>
        /// 下跌的颜色
        /// </summary>
        public static SolidBrush BrushColorDown;
        /// <summary>
        /// 边框的颜色  灰色边框
        /// </summary>
        public static SolidBrush BrushColorFrame;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorNormal;
        /// <summary>
        /// GridCloumn排序标记"↑"，"↓"
        /// </summary>
        public static SolidBrush BrushColorGridCloumnOrderFlag;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorLabelBG;
        /// <summary>
        /// 平盘显示颜色
        /// </summary>
        public static SolidBrush BrushColorSame;
        /// <summary>
        /// 报价中使用的平盘显示颜色
        /// </summary>
        public static SolidBrush BrushColorEqual;
        /// <summary>
        /// 上涨显示颜色
        /// </summary>
        public static SolidBrush BrushColorUp;
        /// <summary>
        /// 成交量的颜色
        /// </summary>
        public static SolidBrush BrushColorVolumn;
        /// <summary>
        /// 走势行业标题颜色
        /// </summary>
        public static SolidBrush BrushColorIndustryTitle;
        /// <summary>
        /// 量比颜色
        /// </summary>
        public static SolidBrush BrushColorLiangbi;
        /// <summary>
        /// 走势k线成交量颜色
        /// </summary>
        public static SolidBrush BrushColorKlineTrendVolume;
        /// <summary>
        /// 走势k线坐标颜色
        /// </summary>
        public static SolidBrush BrushColorKlineTrendCor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorQuoteBorder;
        /// <summary>
        /// 上涨股票的背景色
        /// </summary>
        public static SolidBrush BrushColorUpBackground;
        /// <summary>
        /// 下跌股票的背景色
        /// </summary>
        public static SolidBrush BrushColorDownBackground;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorSerialNumber;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorName;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorCode;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorMaimaiTitle;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorCode_Name;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorMultiTrendBackground;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorDownKline;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPlus;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBoard;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorDDXDown;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorTitleBackground;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorTitleContext;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieBuyXL;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieBuyL;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieBuyM;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieBuyS;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieSellXL;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieSellL;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieSellM;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPieSellS;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorVTabItemBackgournd;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorVTabSelect;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorVTabBackground;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorVFontNormal;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorVFontSelect;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorHTabItemBackgournd;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorHTabSelect;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorHTabFontNormal;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorHTabFontSelect;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorArrow;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushLightBlueBackground;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushDarkBlueBackground;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBar;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorGray;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorSkyBlue;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorPurple;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBlue;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorSurgedLimit;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorDeclineLimit;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorOpenSurgedLimit;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorOpenDeclineLimit;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBiggerAskOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBiggerBidOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorInstitutionAskOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorInstitutionBidOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorRocketLaunch;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorStrongRebound;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorHighDiving;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorSpeedupDown;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorCancelBigAskOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorCancelBigBidOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorInstitutionBidTrans;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorInstitutionAskTrans;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorMultiSameAskOrders;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorMultiSameBidOrders;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBlueName;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorRedName;

		//
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushCommonTextColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushSelectedTextColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushSelectedTextShadowColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushBarArrowUpColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushHideButtonBackGroundColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushButtonTriangleNormalColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushButtonTriangleSelectedColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushBarBackGroundColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushBarArrowDownColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushNewsTitleTextColor;

        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushNormalTabButtonGrayBackColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushNormalTabButtonBlackBackColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushNormalTabButtonYellowTextColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushNormalTabButtonWhiteTextColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushNormalTabButtonBlueTextColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushZHTitleColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushSelectedOrangeColor;
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushVTabTextSelectedColor;

        public static SolidBrush BrushVTabTextSelectedColor1;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushVTabTextNormalColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushVTabTextNormalColor1;
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushKlineButtonTextNormalColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushKlineButtonTextSelectColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushKlineButtonSpecialTextColor;
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BruMultiTrendBgColor;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BruDealDetailTextColor;
		//订单占比
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushColorBigBuyOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorMidBuyOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorSmallBuyOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorBigSellOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorMidSellOrder;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorSmallSellOrder;
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushColorInfopanelHighLight;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorInfopanelInnerTitleBkg;
        /// <summary>
        /// 信息面板正常白色
        /// </summary>
        public static SolidBrush BrushColorInfopanelNormalWhite;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorNewsType;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorNewsTextWhite;
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushColorNewsSelected;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorNewsViewed;
        /// <summary>
        /// 
        /// </summary>
		public static SolidBrush BrushColorF10Border;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorF10NormalText;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorF10SelectedBack;
        /// <summary>
        /// 
        /// </summary>
        public static SolidBrush BrushColorF10SelectedText;
		/// <summary>
		/// 新增
		/// </summary>
		public static SolidBrush BrushColorOrderQueueNew;
		/// <summary>
		/// 撤单
		/// </summary>
		public static SolidBrush BrushColorOrderQueueCancel;
		/// <summary>
		/// 部分成交填充色
		/// </summary>
		public static SolidBrush BrushColorOrderQueuePartDealFill;
		/// <summary>
		/// 部分成交边框
		/// </summary>
		public static SolidBrush BrushColorOrderQueuePartDealBorder;
		/// <summary>
		/// 超过1000手
		/// </summary>
		public static SolidBrush BrushColorOrderQueueKAbove;
		/// <summary>
		/// 超过5000手
		/// </summary>
		public static SolidBrush BrushColorOrderQueue5KAbove;

        public static SolidBrush BrushColorTopBannerMenuNormal;

        public static SolidBrush BrushColorTopBannerMenuOn;

        /// <summary>
        /// split箭头选中色
        /// </summary>
        public static SolidBrush BrushColorSplitArrowActive;

        /// <summary>
        /// split箭头未选中色
        /// </summary>
        public static SolidBrush BrushSplitArrowNormal;

        /// <summary>
        /// 股吧列表奇数行背景
        /// </summary>
        public static SolidBrush BrushBbsOddRowBg;
        /// <summary>
        /// 股吧列表偶数行背景
        /// </summary>
        public static SolidBrush BrushBbsEvenRowBg;
        /// <summary>
        /// 股吧按钮内文字颜色
        /// </summary>
        public static SolidBrush BrushBBSButtonCaption;
        /// <summary>
        /// 股吧按钮选中时文字颜色
        /// </summary>
        public static SolidBrush BrushBBSButtonCaptionMouseOn;
        /// <summary>
        /// 股吧标题栏文字颜色
        /// </summary>
        public static SolidBrush BrushBBSTitle;
        /// <summary>
        /// 股吧列表文字颜色
        /// </summary>
        public static SolidBrush BrushBBSCaption;

        /// <summary>
        /// 导航背景色
        /// </summary>
        public static SolidBrush BrushNavigationBackground;

        public static SolidBrush BrushDeepAnalysisQ1;

        public static SolidBrush BrushDeepAnalysisQ2;

        public static SolidBrush BrushDeepAnalysisQ3;

        public static SolidBrush BrushDeepAnalysisQ4;

        public static SolidBrush BrushTrendMaimaiDown;

        public static SolidBrush BrushTrendMaimaiUp;
		#endregion

		static QuoteDrawService() {
			InitStringFormat();
			InitPen();
			InitBrush();
		}

		private static void InitStringFormat() {
			#region 对其方式
			StringFormat_VCenter_HCenter = new StringFormat();
            StringFormat_VCenter_HCenter.Alignment = StringAlignment.Center;
			StringFormat_VCenter_HCenter.LineAlignment = StringAlignment.Center;
			StringFormat_VCenter_HCenter.FormatFlags = StringFormatFlags.LineLimit;
			StringFormat_VCenter_HCenter.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VCenter_HLeft = new StringFormat();
            StringFormat_VCenter_HLeft.Alignment = StringAlignment.Near;
            StringFormat_VCenter_HLeft.LineAlignment = StringAlignment.Center;
            StringFormat_VCenter_HLeft.FormatFlags = StringFormatFlags.NoWrap;
            StringFormat_VCenter_HLeft.Trimming = StringTrimming.None;

			StringFormat_VCenter_HRight = new StringFormat();
            StringFormat_VCenter_HRight.Alignment = StringAlignment.Far;
            StringFormat_VCenter_HRight.LineAlignment = StringAlignment.Center;
            StringFormat_VCenter_HRight.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VCenter_HRight.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VTop_HCenter = new StringFormat();
            StringFormat_VTop_HCenter.Alignment = StringAlignment.Center;
            StringFormat_VTop_HCenter.LineAlignment = StringAlignment.Near;
            StringFormat_VTop_HCenter.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VTop_HCenter.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VTop_HLeft = new StringFormat();
            StringFormat_VTop_HLeft.Alignment = StringAlignment.Near;
            StringFormat_VTop_HLeft.LineAlignment = StringAlignment.Near;
            StringFormat_VTop_HLeft.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VTop_HLeft.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VTop_HRight = new StringFormat();
            StringFormat_VTop_HRight.Alignment = StringAlignment.Far;
            StringFormat_VTop_HRight.LineAlignment = StringAlignment.Near;
            StringFormat_VTop_HRight.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VTop_HRight.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VBottom_HCenter = new StringFormat();
            StringFormat_VBottom_HCenter.Alignment = StringAlignment.Center;
            StringFormat_VBottom_HCenter.LineAlignment = StringAlignment.Far;
            StringFormat_VBottom_HCenter.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VBottom_HCenter.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VBottom_HLeft = new StringFormat();
            StringFormat_VBottom_HLeft.Alignment = StringAlignment.Near;
            StringFormat_VBottom_HLeft.LineAlignment = StringAlignment.Far;
            StringFormat_VBottom_HLeft.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VBottom_HLeft.Trimming = StringTrimming.EllipsisCharacter;

			StringFormat_VBottom_HRight = new StringFormat();
            StringFormat_VBottom_HRight.Alignment = StringAlignment.Far;
            StringFormat_VBottom_HRight.LineAlignment = StringAlignment.Far;
            StringFormat_VBottom_HRight.FormatFlags = StringFormatFlags.LineLimit;
            StringFormat_VBottom_HRight.Trimming = StringTrimming.EllipsisCharacter;

			#endregion
		}

		private static void InitPen() {
			PenFrame = new Pen(ColorThemeManager.CurrentColorTheme.ColorFrame);
            PenRedName = new Pen(ColorThemeManager.CurrentColorTheme.ColorRedName);
            PenAmount = new Pen(ColorThemeManager.CurrentColorTheme.ColorAmount);
            PenBackground=new Pen(ColorThemeManager.CurrentColorTheme.ColorBackGround);
			PenFrameDash = new Pen(ColorThemeManager.CurrentColorTheme.ColorFrame);
            PenFrameDash.DashStyle = DashStyle.Dash;
			PenFrameDashRed = new Pen(ColorThemeManager.CurrentColorTheme.ColorUp);
            PenFrameDashRed.DashStyle = DashStyle.Dash;
			PenBoard = new Pen(ColorThemeManager.CurrentColorTheme.ColorBoard);
            PenColorCode = new Pen(ColorThemeManager.CurrentColorTheme.ColorCode);
			PenBoardDash = new Pen(ColorThemeManager.CurrentColorTheme.ColorBoard);
			PenBoardDash.DashStyle = DashStyle.Dash;
            PenLiangbi = new Pen(ColorThemeManager.CurrentColorTheme.ColorLiangbi);
            PenVolume = new Pen(ColorThemeManager.CurrentColorTheme.ColorKlineTrendVolume);
            PenVolumeDash = new Pen(ColorThemeManager.CurrentColorTheme.ColorKlineTrendVolume);
            PenKlineDrawBox = new Pen(ColorThemeManager.CurrentColorTheme.ColorKlineDrawBox);
			PenVolumeDash.DashStyle = DashStyle.Dash;
			PenSame = new Pen(ColorThemeManager.CurrentColorTheme.ColorSame);
			PenUpDash = new Pen(ColorThemeManager.CurrentColorTheme.ColorUp);
			PenUpDash.DashStyle = DashStyle.Dash;
			PenUp = new Pen(ColorThemeManager.CurrentColorTheme.ColorUp);
			PenCross = new Pen(ColorThemeManager.CurrentColorTheme.ColorCross);
			PenTrendBackground = new Pen(ColorThemeManager.CurrentColorTheme.ColorTrendBackground);
			PenDashTrendBackground = new Pen(ColorThemeManager.CurrentColorTheme.ColorTrendBackground);
			PenDashTrendBackground.DashStyle = DashStyle.Dash;
			PenDownKLine = new Pen(ColorThemeManager.CurrentColorTheme.ColorDownKline);
			PenColorBarLight = new Pen(ColorThemeManager.CurrentColorTheme.ColorBarLightColor);
			PenCurrentDataRowLine = new Pen(ColorThemeManager.CurrentColorTheme.PenCurrentDataRowLine);
			PenMmdSpliterColor = new Pen(ColorThemeManager.CurrentColorTheme.ColorMmdSpliter);
			PenMmdSpliterDashColor = new Pen(ColorThemeManager.CurrentColorTheme.ColorMmdSpliter);
            PenMmdSpliterDashColor.DashStyle = DashStyle.Dash;
		    PenQuoteInnerSpliterColor = new Pen(ColorThemeManager.CurrentColorTheme.ColorQuoteInnerSpliter);
            PenQuoteInnerSpliterDashColor = new Pen(ColorThemeManager.CurrentColorTheme.ColorQuoteInnerSpliter);
            PenQuoteInnerSpliterDashColor.DashStyle = DashStyle.Dash;
		    
            //
			PenNewsTitleBackUpLightColor = new Pen(ColorThemeManager.CurrentColorTheme.NewsTitleBackUpLightColor);
            PenQuoteBorderColor = new Pen(ColorThemeManager.CurrentColorTheme.QuoteBouderColor);
            PenButtonBorder = new Pen(ColorThemeManager.CurrentColorTheme.ColorQuoteInnerSpliter);
            PenQuoteBorderDashColor = new Pen(ColorThemeManager.CurrentColorTheme.QuoteBouderDashColor);
            PenQuoteBorderDashColor.DashStyle = DashStyle.Dash;
            PenBarBorderColor = new Pen(ColorThemeManager.CurrentColorTheme.BarBorderColor);
			PenColumnSplitColor = new Pen(ColorThemeManager.CurrentColorTheme.ColumnSplitColor);
			PenBarArrowDownColor = new Pen(ColorThemeManager.CurrentColorTheme.BarArrowDownColor);
			PenBarArrowUpColor = new Pen(ColorThemeManager.CurrentColorTheme.BarArrowUpColor);
			PenBarBeginColor = new Pen(ColorThemeManager.CurrentColorTheme.BarBeginColor);
			PenBarEndColor = new Pen(ColorThemeManager.CurrentColorTheme.BarEndColor);
			PenHideButtonHighLightColor1 = new Pen(ColorThemeManager.CurrentColorTheme.HideButtonHighLightColor1);
			PenHideButtonHighLightColor2 = new Pen(ColorThemeManager.CurrentColorTheme.HideButtonHighLightColor2);
			PenHideButtonHighLightColor3 = new Pen(ColorThemeManager.CurrentColorTheme.HideButtonHighLightColor3);
			PenBarThreeLineBeginColor = new Pen(ColorThemeManager.CurrentColorTheme.BarThreeLineBeginColor);
		    PenBarThreeLineEndColor = new Pen(ColorThemeManager.CurrentColorTheme.BarThreeLineEndColor);
			PenBarThreeLineShadowColor = new Pen(ColorThemeManager.CurrentColorTheme.BarThreeLineShadowColor);
			PenBarBackGroundColor = new Pen(ColorThemeManager.CurrentColorTheme.BarBackGroundColor);
			PenCommonButtonUpHighLightColor = new Pen(ColorThemeManager.CurrentColorTheme.CommonButtonUpHighLightColor);
			PenCommonButtonDownLineColor = new Pen(ColorThemeManager.CurrentColorTheme.CommonButtonDownLineColor);
			PenSelectedButtonUpHighLightColor = new Pen(ColorThemeManager.CurrentColorTheme.SelectedButtonUpHighLightColor);
			PenSelectedButtonDownLineColor = new Pen(ColorThemeManager.CurrentColorTheme.SelectedButtonDownLineColor);
			PenButtonSplitColor1 = new Pen(ColorThemeManager.CurrentColorTheme.ButtonSplitColor1);
			PenButtonSplitColor2 = new Pen(ColorThemeManager.CurrentColorTheme.ButtonSplitColor2);
			PenNewsTitleSplitColor = new Pen(ColorThemeManager.CurrentColorTheme.NewsTitleSplitColor);
			PenSelectedButtonSideColor = new Pen(ColorThemeManager.CurrentColorTheme.SelectedButtonSideColor);

			PenNormalTabButtonBounderColor = new Pen(ColorThemeManager.CurrentColorTheme.NormalTabButtonBounderColor);

			PenSelectedTextColor = new Pen(ColorThemeManager.CurrentColorTheme.SelectedTextColor);
			PenSelectedOrangeColor = new Pen(ColorThemeManager.CurrentColorTheme.SelectedBackColor);
			PenMultiSelectedBorderColor = new Pen(ColorThemeManager.CurrentColorTheme.MultiSelectedBorderColor);
			PenVTabHighLightColor = new Pen(ColorThemeManager.CurrentColorTheme.VTabHighLightColor);
			PenVTabShadowColor = new Pen(ColorThemeManager.CurrentColorTheme.VTabShadowColor);
			PenVTabBorderColor = new Pen(ColorThemeManager.CurrentColorTheme.VTabBorderColor);
		    PenVTabActiveInnerBorderColor = new Pen(ColorThemeManager.CurrentColorTheme.VTabActiveInnerBorderColor);
		    PenVTabActiveHighlightColor = new Pen(ColorThemeManager.CurrentColorTheme.VTabActiveHighlightColor);
			PenKlineButtonFrameColor = new Pen(ColorThemeManager.CurrentColorTheme.KlineButtonFrameColor);
			PenKlineIndexButtonFrameColor = new Pen(ColorThemeManager.CurrentColorTheme.KlineIndexButtonFrameColor);
			PenDealDetailFrameColor = new Pen(ColorThemeManager.CurrentColorTheme.VTabBorderColor);
			PenColorBigBuyOrder = new Pen(ColorThemeManager.CurrentColorTheme.ColorBigBuyOrder);
			PenColorMidBuyOrder = new Pen(ColorThemeManager.CurrentColorTheme.ColorMidBuyOrder);
			PenColorSmallBuyOrder = new Pen(ColorThemeManager.CurrentColorTheme.ColorSmallBuyOrder);
			PenColorBigSellOrder = new Pen(ColorThemeManager.CurrentColorTheme.ColorBigSellOrder);
			PenColorMidSellOrder = new Pen(ColorThemeManager.CurrentColorTheme.ColorMidSellOrder);
			PenColorSmallSellOrder = new Pen(ColorThemeManager.CurrentColorTheme.ColorSmallSellOrder);
			PenColorInfopanelHighLight = new Pen(ColorThemeManager.CurrentColorTheme.InfopanelHighLightColor);
			PenColorInfopanelInnerTitleBkg = new Pen(ColorThemeManager.CurrentColorTheme.InfopanelInnerTitleBkgColor);

			PenColorNewsSelected = new Pen(ColorThemeManager.CurrentColorTheme.NewsSelectedColor);
			PenColorNewsViewed = new Pen(ColorThemeManager.CurrentColorTheme.NewsViewedColor);

			PenColorF10Border = new Pen(ColorThemeManager.CurrentColorTheme.F10BorderColor);
			PenColorF10NormalText = new Pen(ColorThemeManager.CurrentColorTheme.F10NormalTextColor);
			PenColorF10SelectedBack = new Pen(ColorThemeManager.CurrentColorTheme.F10SelectedBackColor);
			PenColorF10SelectedText = new Pen(ColorThemeManager.CurrentColorTheme.F10SelectedTextColor);
            PenTrendFrame = new Pen(ColorThemeManager.CurrentColorTheme.TrendFrameColor);
            PenTrendFrameDash = new Pen(ColorThemeManager.CurrentColorTheme.TrendFrameColor);
            PenTrendFrameDash.DashStyle = DashStyle.Dash;
            PenTrendFrameDash.DashPattern = new float[2]{1,3};

            PenThickTrendFrame = new Pen(ColorThemeManager.CurrentColorTheme.TrendFrameColor, 2);

			PenColorOrderQueue5KAbove = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueue5KAboveColor);
			PenColorOrderQueueCancel = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueueCancelColor);
			PenColorOrderQueueKAbove = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueueKAboveColor);
			PenColorOrderQueueNew = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueueNewColor);
			PenColorOrderQueuePartDealBorder = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueuePartDealBorderColor);
			PenColorOrderQueuePartDealFill = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueuePartDealFillColor);
		    PenColorOrderQueueSpliter = new Pen(ColorThemeManager.CurrentColorTheme.OrderQueueSpliterColor);
		    PenColorDashOrderDetailSpliter = new Pen(ColorThemeManager.CurrentColorTheme.OrderDetailSpliterColor);
		    PenColorDashOrderDetailSpliter.DashStyle = DashStyle.Dash;
            PenBBSSplitLineColor = new Pen(ColorThemeManager.CurrentColorTheme.BBSSplitLineColor);
		    PenBBSButtonBorderColor = new Pen(ColorThemeManager.CurrentColorTheme.BBSButtonBorderColor);
		    PenBBSButtonBorderHighlightColor = new Pen(ColorThemeManager.CurrentColorTheme.BBSButtonBorderHighlightColor);
		    PenBBSButtonMouseOnBorderHighlightColor =
		        new Pen(ColorThemeManager.CurrentColorTheme.BBSButtonMouseOnBorderHighlightColor);
		}

		private static void InitBrush() {
			BrushColorUp = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorUp);
			BrushColorDown = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorDown);
            BrushColorSame = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSame);
            BrushColorEqual = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorEqual);
            BrushColorNormal = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorGrayWhite);
            BrushColorGridCloumnOrderFlag = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorYellow);
			BrushColorAmount = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorAmount);
			BrushColorVolumn = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorVolume);
            BrushColorKlineTrendCor = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorKlineTrendCor);
            BrushColorQuoteBorder = new SolidBrush(ColorThemeManager.CurrentColorTheme.QuoteBouderColor);
            BrushColorLiangbi = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorLiangbi);
            BrushColorIndustryTitle = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorIndustryTitle);
            BrushColorKlineTrendVolume = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorKlineTrendVolume);
			BrushColorFrame = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorFrame);
			BrushColorBackGround = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBackGround);
            BrushColorLabelBG = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorLabelBG);
			BrushColorUpBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorUpBackground);
			BrushColorDownBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorDownBackground);
			BrushColorMultiTrendBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorMultiTrendBackground);
			BrushColorDownKline = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorDownKline);
			BrushColorPlus = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPlus);
			BrushColorBoard = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBoard);
            BrushColorDDXDown = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorDDXDown);
			BrushColorTitleBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorTitleBackground);
			BrushColorTitleContext = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorTitleContext);
			BrushColorPieBuyXL = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieBuyXLarge);
			BrushColorPieBuyL = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieBuyLarge);
			BrushColorPieBuyM = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieBuyMiddle);
			BrushColorPieBuyS = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieBuySmall);
			BrushColorPieSellXL = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieSellXLarge);
			BrushColorPieSellL = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieSellLarge);
			BrushColorPieSellM = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieSellMiddle);
			BrushColorPieSellS = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPieSellSmall);
			BrushColorVTabItemBackgournd = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorVTabItemBackGround);
			BrushColorVTabSelect = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorVTabSelected);
			BrushColorHTabItemBackgournd = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorHTabItemNormalBk);
			BrushColorHTabSelect = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorHTabItemSelectBk);
			BrushColorHTabFontNormal = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorHTabFontNormal);
			BrushColorHTabFontSelect = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorHTabFontSelect);
			BrushColorVTabBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorVTabBackground);
			BrushColorVFontNormal = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorVTabFontNormal);
			BrushColorVFontSelect = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorVTabFontSelect);
			BrushColorArrow = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorArrow);
			BrushColorSerialNumber = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSerialNumber);
			BrushColorName = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorName);
			BrushColorCode = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorCode);
            BrushColorMaimaiTitle = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorMaimaiTitle);
			BrushColorCode_Name = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorCode_Name);
			
			BrushLightBlueBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.LightBlueBackground);
			BrushDarkBlueBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.DarkBlueBackground);
			BrushColorBar = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBar);
			BrushColorGray = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorGray);
			BrushColorSkyBlue = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSkyBlue);
			BrushColorPurple = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorPurple);
			BrushColorBlue = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBule);
			BrushColorSurgedLimit = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSurgedLimit);
			BrushColorDeclineLimit = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorDeclineLimit);
			BrushColorOpenSurgedLimit = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorOpenSurgedLimit);
			BrushColorOpenDeclineLimit = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorOpenDeclineLimit);
			BrushColorBiggerAskOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBiggerAskOrder);
			BrushColorBiggerBidOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBiggerBidOrder);
			BrushColorInstitutionAskOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorInstitutionAskOrder);
			BrushColorInstitutionBidOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorInstitutionBidOrder);
			BrushColorRocketLaunch = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorRocketLaunch);
			BrushColorStrongRebound = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorStrongRebound);
			BrushColorHighDiving = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorHighDiving);
			BrushColorSpeedupDown = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSpeedupDown);
			BrushColorCancelBigAskOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorCancelBigAskOrder);
			BrushColorCancelBigBidOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorCancelBigBidOrder);
			BrushColorInstitutionBidTrans = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorInstitutionBidTrans);
			BrushColorInstitutionAskTrans = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorInstitutionAskTrans);
			BrushColorMultiSameAskOrders = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorMultiSameAskOrders);
			BrushColorMultiSameBidOrders = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorMultiSameBidOrders);
			BrushColorBlueName = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBlueName);
			BrushColorRedName = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorRedName);


			//
			BrushCommonTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.CommonTextColor);
			BrushSelectedTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.SelectedTextColor);
			BrushSelectedTextShadowColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.SelectedTextShadowColor);
			BrushBarArrowUpColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.BarArrowUpColor);
			BrushHideButtonBackGroundColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.HideButtonBackGroundColor);
			BrushButtonTriangleNormalColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.ButtonTriangleNormalColor);
			BrushButtonTriangleSelectedColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.ButtonTriangleSelectedColor);
			BrushBarBackGroundColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.BarBackGroundColor);
			BrushBarArrowDownColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.BarArrowDownColor);
			BrushNewsTitleTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.NewsTitleTextColor);




			BrushNormalTabButtonGrayBackColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.NormalTabButtonGrayBackColor);
			BrushNormalTabButtonBlackBackColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.NormalTabButtonBlackBackColor);
			BrushNormalTabButtonYellowTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.NormalTabButtonYellowTextColor);
			BrushNormalTabButtonWhiteTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.NormalTabButtonWhiteTextColor);
			BrushNormalTabButtonBlueTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.NormalTabButtonBlueTextColor);
			BrushZHTitleColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.ZHTitleColor);
			BrushSelectedOrangeColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.SelectedBackColor);

			BrushVTabTextSelectedColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.VTabTextSelectedColor);
            BrushVTabTextSelectedColor1 = new SolidBrush(ColorThemeManager.CurrentColorTheme.VTabTextSelectedColor1);
			BrushVTabTextNormalColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.VTabTextNormalColor);
			BrushVTabTextNormalColor1 = new SolidBrush(ColorThemeManager.CurrentColorTheme.VTabTextNormalColor1);
			//
			BrushKlineButtonTextNormalColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.KlineButtonTextNormalColor);
			BrushKlineButtonTextSelectColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.KlineButtonTextSelectColor);
			BrushKlineButtonSpecialTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.KlineButtonSpecialTextColor);
			BruMultiTrendBgColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.MultiTrendBgColor);
			BruDealDetailTextColor = new SolidBrush(ColorThemeManager.CurrentColorTheme.DealDetailTilteColor);
			BrushColorBigBuyOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBigBuyOrder);
			BrushColorMidBuyOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorMidBuyOrder);
			BrushColorSmallBuyOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSmallBuyOrder);
			BrushColorBigSellOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorBigSellOrder);
			BrushColorMidSellOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorMidSellOrder);
			BrushColorSmallSellOrder = new SolidBrush(ColorThemeManager.CurrentColorTheme.ColorSmallSellOrder);
			BrushColorInfopanelHighLight = new SolidBrush(ColorThemeManager.CurrentColorTheme.InfopanelHighLightColor);
			BrushColorInfopanelInnerTitleBkg = new SolidBrush(ColorThemeManager.CurrentColorTheme.InfopanelInnerTitleBkgColor);
		    BrushColorInfopanelNormalWhite = new SolidBrush(ColorThemeManager.CurrentColorTheme.InfopanelNormalWhiteColor);

            BrushColorNewsType = new SolidBrush(ColorThemeManager.CurrentColorTheme.NewsTypeColor);
            BrushColorNewsTextWhite = new SolidBrush(ColorThemeManager.CurrentColorTheme.NewsTextWhiteColor);
			BrushColorNewsSelected = new SolidBrush(ColorThemeManager.CurrentColorTheme.NewsSelectedColor);
			BrushColorNewsViewed = new SolidBrush(ColorThemeManager.CurrentColorTheme.NewsViewedColor);

			BrushColorF10Border = new SolidBrush(ColorThemeManager.CurrentColorTheme.F10BorderColor);
			BrushColorF10NormalText = new SolidBrush(ColorThemeManager.CurrentColorTheme.F10NormalTextColor);
			BrushColorF10SelectedBack = new SolidBrush(ColorThemeManager.CurrentColorTheme.F10SelectedBackColor);
			BrushColorF10SelectedText = new SolidBrush(ColorThemeManager.CurrentColorTheme.F10SelectedTextColor);

			BrushColorOrderQueue5KAbove = new SolidBrush(ColorThemeManager.CurrentColorTheme.OrderQueue5KAboveColor);
			BrushColorOrderQueueCancel = new SolidBrush(ColorThemeManager.CurrentColorTheme.OrderQueueCancelColor);
			BrushColorOrderQueueKAbove = new SolidBrush(ColorThemeManager.CurrentColorTheme.OrderQueueKAboveColor);
			BrushColorOrderQueueNew = new SolidBrush(ColorThemeManager.CurrentColorTheme.OrderQueueNewColor);
			BrushColorOrderQueuePartDealBorder = new SolidBrush(ColorThemeManager.CurrentColorTheme.OrderQueuePartDealBorderColor);
			BrushColorOrderQueuePartDealFill = new SolidBrush(ColorThemeManager.CurrentColorTheme.OrderQueuePartDealFillColor);

		    BrushColorTopBannerMenuNormal = new SolidBrush(ColorThemeManager.CurrentColorTheme.TopBannerMenuNormal);
		    BrushColorTopBannerMenuOn = new SolidBrush(ColorThemeManager.CurrentColorTheme.TopBannerMenuOn);
            BrushColorSplitArrowActive = new SolidBrush(ColorThemeManager.CurrentColorTheme.SplitArrowColorActive);
            BrushSplitArrowNormal = new SolidBrush(ColorThemeManager.CurrentColorTheme.SplitArrowColorNormal);
		    BrushBbsOddRowBg = new SolidBrush(ColorThemeManager.CurrentColorTheme.BbsOddRowBackColor);
		    BrushBbsEvenRowBg = new SolidBrush(ColorThemeManager.CurrentColorTheme.BbsEvenRowBackColor);
		    BrushBBSButtonCaption = new SolidBrush(ColorThemeManager.CurrentColorTheme.BBSButtonCaptionColor);
		    BrushBBSButtonCaptionMouseOn = new SolidBrush(ColorThemeManager.CurrentColorTheme.BBSButtonMouseOnCaptionColor);
		    BrushBBSTitle = new SolidBrush(ColorThemeManager.CurrentColorTheme.BBSTitleColor);
		    BrushBBSCaption = new SolidBrush(ColorThemeManager.CurrentColorTheme.BBSCaptionColor);

            BrushNavigationBackground = new SolidBrush(ColorThemeManager.CurrentColorTheme.NavigationBackgroundColor);

		    BrushDeepAnalysisQ1 = new SolidBrush(ColorThemeManager.CurrentColorTheme.DeepAnalysisQ1);
		    BrushDeepAnalysisQ2 = new SolidBrush(ColorThemeManager.CurrentColorTheme.DeepAnalysisQ2);
		    BrushDeepAnalysisQ3 = new SolidBrush(ColorThemeManager.CurrentColorTheme.DeepAnalysisQ3);
		    BrushDeepAnalysisQ4 = new SolidBrush(ColorThemeManager.CurrentColorTheme.DeepAnalysisQ4);

            BrushTrendMaimaiDown = new SolidBrush(ColorThemeManager.CurrentColorTheme.TrendMaimaiDown);
            BrushTrendMaimaiUp = new SolidBrush(ColorThemeManager.CurrentColorTheme.TrendMaimaiUp);
		}

        /// <summary>
        /// 由ShortLineType获得对应画刷
        /// </summary>
		public static SolidBrush GetShortLineBrush(ShortLineType type) {
			switch(type) {
				case ShortLineType.MultiSameBidOrders:
					return BrushColorMultiSameBidOrders;
				case ShortLineType.MultiSameAskOrders:
					return BrushColorMultiSameAskOrders;
				case ShortLineType.SurgedLimit:
					return BrushColorSurgedLimit;
				case ShortLineType.DeclineLimit:
					return BrushColorDeclineLimit;
				case ShortLineType.OpenSurgedLimit:
					return BrushColorOpenSurgedLimit;
				case ShortLineType.OpenDeclineLimit:
					return BrushColorOpenDeclineLimit;
				case ShortLineType.BiggerBidOrder:
					return BrushColorBiggerBidOrder; 
				case ShortLineType.InstitutionAskOrder:
					return BrushColorInstitutionAskOrder;
				case ShortLineType.InstitutionBidOrder:
					return BrushColorInstitutionBidOrder;
				case ShortLineType.BiggerAskOrder:
					return BrushColorBiggerAskOrder;
				case ShortLineType.StrongRebound:
					return BrushColorStrongRebound;
				case ShortLineType.HighDiving:
					return BrushColorHighDiving;
				case ShortLineType.SpeedupDown:
					return BrushColorSpeedupDown;
				case ShortLineType.CancelBigAskOrder:
					return BrushColorCancelBigAskOrder;
				case ShortLineType.CancelBigBidOrder:
					return BrushColorCancelBigBidOrder;
				case ShortLineType.InstitutionBidTrans:
					return BrushColorInstitutionBidTrans;
				case ShortLineType.InstitutionAskTrans:
					return BrushColorInstitutionAskTrans;
				case ShortLineType.RocketLaunch:
					return BrushColorRocketLaunch;
			}
			return BrushColorMultiSameBidOrders;
		}
	}
}
