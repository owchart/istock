using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EmQComm {
    /// <summary>
    /// 颜色管理
    /// </summary>
	public static class ColorThemeManager {
        /// <summary>
        /// ColorTheme对象
        /// </summary>
		public static ColorTheme CurrentColorTheme
        {
            get { return _currentColorTheme; }
            private set { _currentColorTheme = value; }
        }

        private static readonly Dictionary<string, ColorTheme> ColorThemes = new Dictionary<string, ColorTheme>();
		private static readonly ColorTheme EmColorTheme = new ColorTheme("EM");
		private static readonly ColorTheme NetColorTheme = new ColorTheme("Net");
        private static ColorTheme _currentColorTheme;

        static ColorThemeManager() {
			Init();
			SetCurrentColorThem("EM");
		}

		private static void Init() {
			//EM配色
			EmColorTheme.ColorBoard = Color.FromArgb(120, 0, 0);
            EmColorTheme.ColorDDXDown = Color.FromArgb(66, 255, 255);
			EmColorTheme.ColorAmount = Color.FromArgb(0, 225, 225);
            EmColorTheme.ColorLiangbi = Color.FromArgb(225, 0, 225);
		    EmColorTheme.ColorLabelBG = Color.FromArgb(20, 20, 20);
		    EmColorTheme.ColorIndustryTitle = Color.FromArgb(140,140,140);
            EmColorTheme.ColorKlineTrendCor = Color.FromArgb(160, 160, 160);
			EmColorTheme.ColorVolume = Color.FromArgb(210, 210, 210);
            EmColorTheme.ColorKlineTrendVolume = Color.FromArgb(255, 255, 0);
		    EmColorTheme.ColorKlineDrawBox = Color.FromArgb(107, 107, 107);
			EmColorTheme.ColorFrame = Color.FromArgb(120, 0, 0);
			EmColorTheme.ColorSame = Color.FromArgb(225, 225, 225);
			EmColorTheme.ColorDownKline = Color.FromArgb(84, 255, 255);
			EmColorTheme.ColorDown = Color.FromArgb(80, 255, 80);
            EmColorTheme.ColorUp = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorEqual = Color.FromArgb(185, 185, 185);
			EmColorTheme.ColorBackGround = Color.Black;
			EmColorTheme.ColorSerialNumber = Color.FromArgb(192, 192, 192);
			EmColorTheme.ColorCode = Color.FromArgb(210, 210, 210);
            EmColorTheme.ColorMaimaiTitle = Color.FromArgb(110,110,100);
			EmColorTheme.ColorName = Color.FromArgb(247, 253, 166);
			EmColorTheme.ColorCode_Name = Color.FromArgb(240, 248, 126);
            EmColorTheme.ColorPieBuyXLarge = Color.FromArgb(63, 21, 22);
            EmColorTheme.ColorPieBuyLarge = Color.FromArgb(126, 42, 42);
            EmColorTheme.ColorPieBuyMiddle = Color.FromArgb(191, 63, 62);
            EmColorTheme.ColorPieBuySmall = Color.FromArgb(254, 84, 84);
            EmColorTheme.ColorPieSellXLarge = Color.FromArgb(21, 63, 62);
            EmColorTheme.ColorPieSellLarge = Color.FromArgb(46, 126, 127);
            EmColorTheme.ColorPieSellMiddle = Color.FromArgb(63, 191, 192);
            EmColorTheme.ColorPieSellSmall = Color.FromArgb(81, 255, 254);
			EmColorTheme.IndexAverageColor = new Color[]
                                                  {
                                                      Color.White, Color.Yellow, Color.Violet, Color.FromArgb(0, 230, 0),
                                                      Color.Blue, Color.Red
                                                  };
			EmColorTheme.ColorUpBackground = Color.FromArgb(28, 0, 0);
			EmColorTheme.ColorDownBackground = Color.FromArgb(0, 28, 0);
			EmColorTheme.ColorPlus = Color.FromArgb(255, 0, 255);
			EmColorTheme.ColorMultiTrendBackground = Color.FromArgb(0, 0, 100);
			EmColorTheme.ColorTitleBackground = Color.FromArgb(48, 48, 48);
			EmColorTheme.ColorTitleContext = Color.FromArgb(192, 192, 192);
			EmColorTheme.ColorCross = Color.FromArgb(100, 100, 100);
			EmColorTheme.ColorSplit = EmColorTheme.ColorBoard;
			EmColorTheme.ColorTrendPrice = Color.FromArgb(255, 255, 255);
			EmColorTheme.ColorTrendBackground = Color.FromArgb(120, 0, 0);
			EmColorTheme.ColorHTabBackground = Color.Black;
			EmColorTheme.ColorHTabItemNormalBk = Color.FromArgb(0, 0, 0);
			EmColorTheme.ColorHTabItemSelectBk = Color.FromArgb(0, 0, 0);
			EmColorTheme.ColorHTabFontNormal = Color.FromArgb(160, 160, 160);
			EmColorTheme.ColorHTabFontSelect = Color.FromArgb(255, 255, 255);
			

			EmColorTheme.ColorHTabItemNormalBKBegin = Color.FromArgb(135, 135, 135);
			EmColorTheme.ColorHTabItemNormalBKMiddle = Color.FromArgb(82, 82, 82);
			EmColorTheme.ColorHTabItemNormalBKMiddle2 = Color.FromArgb(82, 82, 82);
			EmColorTheme.ColorHTabItemNormalBKEnd = Color.FromArgb(37, 37, 37);
			EmColorTheme.ColorHTabItemSelectBKBegin = Color.FromArgb(255, 93, 95);
			EmColorTheme.ColorHTabItemSelectBKMiddle = Color.FromArgb(191, 0, 2);
			EmColorTheme.ColorHTabItemSelectBKMiddle2 = Color.FromArgb(191, 0, 2);
			EmColorTheme.ColorHTabItemSelectBKEnd = Color.FromArgb(88, 0, 0);
			EmColorTheme.DealFontColor = Color.White;
			EmColorTheme.DealBackGroundColor = Color.FromArgb(0, 0, 128);
			EmColorTheme.CancelFontBackGroundColor = Color.FromArgb(255, 255, 0);
			EmColorTheme.NewlyBackGroundColor = Color.FromArgb(0, 0, 128);
			EmColorTheme.LargeFontColor = Color.FromArgb(255, 127, 39);
			EmColorTheme.HugeFontColor = Color.FromArgb(136, 0, 21);
			EmColorTheme.ColorVTabBackground = Color.Black;
			EmColorTheme.ColorVTabItemBackGround = Color.FromArgb(48, 48, 48);
			EmColorTheme.ColorVTabSelected = Color.FromArgb(0, 0, 64);
			EmColorTheme.ColorVTabFontNormal = Color.FromArgb(192, 192, 192);
			EmColorTheme.ColorVTabFontSelect = Color.FromArgb(192, 192, 192);
			EmColorTheme.LightBlueBackground = Color.FromArgb(12,17,35);
			EmColorTheme.DarkBlueBackground = Color.FromArgb(24,35,69);
			EmColorTheme.ColorArrow = Color.FromArgb(192, 192, 192);
			EmColorTheme.ColorArrowGray = Color.FromArgb(192, 192, 192);
			EmColorTheme.ColorBar = Color.FromArgb(45, 45, 45);
			EmColorTheme.ColorBarLightColor = Color.FromArgb(100, 100, 100);
			EmColorTheme.PenCurrentDataRowLine = Color.FromArgb(249, 107, 39);
			EmColorTheme.ColorGray = Color.Gray;
			EmColorTheme.ColorCodeName = Color.FromArgb(240, 248, 136);
			EmColorTheme.ColorBlueName = Color.FromArgb(84, 255, 255);
			EmColorTheme.ColorRedName = Color.FromArgb(255, 0, 0);
			EmColorTheme.ColorSkyBlue = Color.SkyBlue;
			EmColorTheme.ColorPurple = Color.Purple;
			EmColorTheme.ColorBule = Color.Blue;
            EmColorTheme.ColorGrayWhite = Color.FromArgb(165, 165, 165);
            EmColorTheme.ColorYellow = Color.Yellow;

            EmColorTheme.ColorSurgedLimit = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorDeclineLimit = Color.FromArgb(80, 255, 80);
            EmColorTheme.ColorOpenSurgedLimit = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorOpenDeclineLimit = Color.FromArgb(80, 255, 80);
			EmColorTheme.ColorBiggerAskOrder = Color.FromArgb(192, 192, 0);
            EmColorTheme.ColorBiggerBidOrder = Color.FromArgb(192, 192, 0);
			EmColorTheme.ColorInstitutionAskOrder = Color.FromArgb(252, 112, 0);
			EmColorTheme.ColorInstitutionBidOrder = Color.FromArgb(58, 115, 35);
            EmColorTheme.ColorRocketLaunch = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorStrongRebound = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorHighDiving = Color.FromArgb(80, 255, 80);
			EmColorTheme.ColorSpeedupDown = Color.FromArgb(80, 255, 80);
            EmColorTheme.ColorCancelBigAskOrder = Color.FromArgb(80,255,80);
            EmColorTheme.ColorCancelBigBidOrder = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorInstitutionBidTrans = Color.FromArgb(80,255,80);
            EmColorTheme.ColorInstitutionAskTrans = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorMultiSameAskOrders = Color.FromArgb(255, 82, 82);
            EmColorTheme.ColorMultiSameBidOrders = Color.FromArgb(80, 255, 80);



            //Tab颜色
			EmColorTheme.CommonTextColor = Color.FromArgb(160, 160, 160);
			EmColorTheme.SelectedTextColor = Color.FromArgb(255, 255, 255);
			EmColorTheme.SelectedTextShadowColor = Color.FromArgb(139, 30, 0);
			EmColorTheme.CommonButtonBeginColor = Color.FromArgb(43, 43, 43);
			EmColorTheme.CommonButtonEndColor = Color.FromArgb(39, 39, 39);
			EmColorTheme.SelectedButtonBeginColor = Color.FromArgb(202, 44, 0);
			EmColorTheme.SelectedButtonEndColor = Color.FromArgb(191, 42, 0);
			EmColorTheme.CommonButtonUpHighLightColor = Color.FromArgb(62, 62, 62);
			EmColorTheme.CommonButtonDownLineColor = Color.FromArgb(25, 25, 25);
			EmColorTheme.SelectedButtonUpHighLightColor = Color.FromArgb(234, 67, 20);
			EmColorTheme.SelectedButtonDownLineColor = Color.FromArgb(26, 26, 26);
            EmColorTheme.SelectedButtonSideColor = Color.FromArgb(208, 50, 7);
			EmColorTheme.ButtonSplitColor1 = Color.FromArgb(0, 0, 0);
			EmColorTheme.ButtonSplitColor2 = Color.FromArgb(62, 62, 62);



			EmColorTheme.BarBeginColor = Color.FromArgb(79, 79, 79);
			EmColorTheme.BarEndColor = Color.FromArgb(47, 47, 47);
		    EmColorTheme.BarMouseOnBeginColor = Color.FromArgb(92,92,92);
		    EmColorTheme.BarMouseOnEndColor = Color.FromArgb(55,55,55);
			EmColorTheme.BarArrowUpColor = Color.FromArgb(184, 184, 184);
			EmColorTheme.BarArrowDownColor = Color.FromArgb(40, 40, 40);
			EmColorTheme.BarHighLightBeginColor = Color.FromArgb(99, 99,99);
			EmColorTheme.BarHighLightEndColor = Color.FromArgb(64,64,64);
		    EmColorTheme.BarHighLightMouseOnBeginColor = Color.FromArgb(114, 114, 114);
		    EmColorTheme.BarHighLightMouseOnEndColor = Color.FromArgb(73, 73, 73);
			EmColorTheme.BarThreeLineBeginColor = Color.FromArgb(183,183,183);
		    EmColorTheme.BarThreeLineEndColor = Color.FromArgb(111, 111, 111);
			EmColorTheme.BarThreeLineShadowColor = Color.FromArgb(40, 40, 40);
			EmColorTheme.BarBackGroundColor = Color.FromArgb(38, 38, 38);

			EmColorTheme.HideButtonBackGroundColor = Color.FromArgb(34, 34, 34);
			EmColorTheme.HideButtonHighLightColor1 = Color.FromArgb(18, 18, 18);
			EmColorTheme.HideButtonHighLightColor2 = Color.FromArgb(5, 5, 5);
			EmColorTheme.HideButtonHighLightColor3 = Color.FromArgb(42, 42, 42);

			EmColorTheme.ColumnSplitColor = Color.FromArgb(42, 42, 42);

            EmColorTheme.BarBorderColor = Color.FromArgb(26, 26, 26);

			EmColorTheme.ButtonTriangleNormalColor = Color.FromArgb(100, 100, 100);
			EmColorTheme.ButtonTriangleSelectedColor = Color.FromArgb(255, 255, 255);

			EmColorTheme.QuoteBouderColor = Color.FromArgb(26, 26, 26);
		    EmColorTheme.QuoteBouderDashColor = Color.FromArgb(26, 26, 26);


            EmColorTheme.NewsSelectedColor = Color.FromArgb(255, 50, 0);
            EmColorTheme.NewsViewedColor = Color.FromArgb(126, 126, 126);
			EmColorTheme.NewsTitleBackUpLightColor = Color.FromArgb(60, 60, 60);
			EmColorTheme.NewsTitleBackBeginColor = Color.FromArgb(41, 41, 41);
			EmColorTheme.NewsTitleBackEndColor = Color.FromArgb(26, 26, 26);
			EmColorTheme.NewsTitleTextColor = Color.FromArgb(254, 254, 254);
            EmColorTheme.NewsTitleSplitColor = Color.FromArgb(26, 26, 26);
            EmColorTheme.NewsTypeColor = Color.FromArgb(160, 160, 160);
            EmColorTheme.NewsTextWhiteColor = Color.FromArgb(210, 210, 210);


			EmColorTheme.NormalTabButtonBounderColor = Color.FromArgb(77, 77, 77);
			EmColorTheme.NormalTabButtonGrayBackColor = Color.FromArgb(60, 60, 60);
			EmColorTheme.NormalTabButtonBlackBackColor = Color.FromArgb(0, 0, 0);
			EmColorTheme.NormalTabButtonYellowTextColor = Color.FromArgb(254, 254, 0);
			EmColorTheme.NormalTabButtonYellowTextColor1 = Color.FromArgb(255, 255, 0);
			EmColorTheme.NormalTabButtonWhiteTextColor = Color.FromArgb(220, 221, 222);
			EmColorTheme.NormalTabButtonBlueTextColor = Color.FromArgb(0, 224, 224);

			//k线、走势button配色、
			EmColorTheme.KlineButtonTextSelectColor = Color.FromArgb(255, 255, 255);
			EmColorTheme.KlineButtonTextNormalColor = Color.FromArgb(169, 169, 169);
			EmColorTheme.KlineButtonNormalLowColor = Color.FromArgb(24, 24, 24);
			EmColorTheme.KlineButtonNormalHighColor = Color.FromArgb(35, 35, 35);
			EmColorTheme.KlineButtonSelectHighColor = Color.FromArgb(60, 60, 60);
			EmColorTheme.KlineButtonSelectLowColor = Color.FromArgb(48, 48, 48);
			EmColorTheme.KlineButtonFrameColor = Color.FromArgb(13, 13, 13);
			EmColorTheme.KlineButtonSpecialTextColor = Color.FromArgb(255, 50, 0);
			EmColorTheme.KlineIndexButtonFrameColor = Color.FromArgb(79, 77, 78);
			EmColorTheme.MultiTrendBgColor = Color.FromArgb(14, 14, 14);

			EmColorTheme.ZHTitleColor = Color.FromArgb(255, 225, 80);
			EmColorTheme.DealDetailTilteColor = Color.FromArgb(255, 255, 80);
			EmColorTheme.SelectedBackColor = Color.FromArgb(249, 107, 39);

			EmColorTheme.MultiSelectedBorderColor = Color.FromArgb(249, 107, 39);

			EmColorTheme.VTabHighLightColor = Color.FromArgb(62, 62, 62);
			EmColorTheme.VTabShadowColor = Color.FromArgb(26, 26, 26);
			EmColorTheme.VTabBeginColor = Color.FromArgb(43, 43, 43);
			EmColorTheme.VTabEndColor = Color.FromArgb(39, 39, 39);
			EmColorTheme.VTabBorderColor = Color.FromArgb(26, 26, 26);

            EmColorTheme.VTabTextSelectedColor = Color.FromArgb(255, 255, 255);
            EmColorTheme.VTabTextSelectedColor1 = Color.FromArgb(139, 30, 0);
		    EmColorTheme.VTabActiveBeginColor = Color.FromArgb(202,44,0);
		    EmColorTheme.VTabActiveEndColor = Color.FromArgb(191,42,0);
            EmColorTheme.VTabTextNormalColor = Color.FromArgb(160, 160,160);
			EmColorTheme.VTabTextNormalColor1 = Color.FromArgb(0, 0, 0);
		    EmColorTheme.VTabActiveInnerBorderColor = Color.FromArgb(208,50,7);
		    EmColorTheme.VTabActiveHighlightColor = Color.FromArgb(234,67,20);

			//订单占比
			EmColorTheme.ColorBigBuyOrder = Color.FromArgb(255, 0, 0);
			EmColorTheme.ColorMidBuyOrder = Color.FromArgb(255, 128, 0);
			EmColorTheme.ColorSmallBuyOrder = Color.FromArgb(255, 255, 0);
			EmColorTheme.ColorBigSellOrder = Color.FromArgb(0, 255, 0);
			EmColorTheme.ColorMidSellOrder = Color.FromArgb(0, 0, 255);
			EmColorTheme.ColorSmallSellOrder = Color.FromArgb(76, 255, 252);
			EmColorTheme.ColorMmdSpliter = Color.FromArgb(160, 160, 160);
		    EmColorTheme.ColorQuoteInnerSpliter = Color.FromArgb(25, 25, 25);

			EmColorTheme.InfopanelHighLightColor = Color.FromArgb(60, 60, 60);
			EmColorTheme.InfopanelInnerTitleBkgColor = Color.FromArgb(25, 25, 25);
		    EmColorTheme.InfopanelNormalWhiteColor = Color.FromArgb(210, 210, 210);
            EmColorTheme.TrendFrameColor = Color.FromArgb(76, 76, 76);
            EmColorTheme.TrendMaimaiDown = Color.FromArgb(0, 85, 0);
            EmColorTheme.TrendMaimaiUp = Color.FromArgb(128, 0, 0);
			EmColorTheme.F10BorderColor = Color.FromArgb(80, 1, 0);
			EmColorTheme.F10NormalTextColor = Color.FromArgb(254, 254, 78);
			EmColorTheme.F10SelectedBackColor = Color.FromArgb(249, 107, 39);
			EmColorTheme.F10SelectedTextColor = Color.FromArgb(255, 255, 255);
			EmColorTheme.OrderQueueNewColor = Color.FromArgb(0,0,128);
			EmColorTheme.OrderQueueCancelColor = Color.FromArgb(255,255,78);
			EmColorTheme.OrderQueuePartDealBorderColor = Color.FromArgb(0,0,77);
			EmColorTheme.OrderQueuePartDealFillColor = Color.FromArgb(192,56,56,56);
			EmColorTheme.OrderQueueKAboveColor = Color.FromArgb(2,59,63);
			EmColorTheme.OrderQueue5KAboveColor = Color.FromArgb(134,3,134);
		    EmColorTheme.OrderQueueSpliterColor = Color.FromArgb(25, 25, 25);
            EmColorTheme.OrderDetailSpliterColor = Color.FromArgb(108, 108, 108);

            EmColorTheme.BBSSplitLineColor = Color.FromArgb(204, 204, 204);
		    EmColorTheme.BBSButtonBackgroundBeginColor = Color.FromArgb(65,65,65);
		    EmColorTheme.BBSButtonBackgroundEndColor = Color.FromArgb(60,60,60);
		    EmColorTheme.BBSButtonMouseOnBackgroundBeginColor = Color.FromArgb(83,83,83);
		    EmColorTheme.BBSButtonMouseOnBackgroundEndColor = Color.FromArgb(78,78,78);
		    EmColorTheme.BBSButtonBorderColor = Color.FromArgb(27, 27, 27);
		    EmColorTheme.BBSButtonBorderHighlightColor = Color.FromArgb(90, 90, 90);
		    EmColorTheme.BBSButtonMouseOnBorderHighlightColor = Color.FromArgb(115, 115, 115);
		    EmColorTheme.BBSButtonCaptionColor = Color.FromArgb(200, 200, 200);
		    EmColorTheme.BBSButtonMouseOnCaptionColor = Color.FromArgb(255, 255, 255);
		    EmColorTheme.BBSTitleColor = Color.FromArgb(160, 160, 160);
		    EmColorTheme.BBSCaptionColor = Color.FromArgb(210, 210, 210);

		    EmColorTheme.TopBannerMenuOn = Color.FromArgb(255, 74, 0);
		    EmColorTheme.TopBannerMenuNormal = Color.FromArgb(224,224,224);

		    EmColorTheme.SplitColorHighActive = Color.FromArgb(103, 103, 103);
            EmColorTheme.SplitColorLowActive = Color.FromArgb(7,7,7);
            EmColorTheme.SplitColorHighNormal = Color.FromArgb(37, 37, 37);
            EmColorTheme.SplitColorLowNormal = Color.FromArgb(6, 6, 6);
		    EmColorTheme.SplitArrowColorNormal = Color.FromArgb(209, 209, 209);
            EmColorTheme.SplitArrowColorActive = Color.FromArgb(255, 255, 255);
		    EmColorTheme.BbsEvenRowBackColor = Color.FromArgb(0, 0, 0);
		    EmColorTheme.BbsOddRowBackColor = Color.FromArgb(18, 18, 18);

		    EmColorTheme.DeepAnalysisQ1 = Color.FromArgb(255, 168, 0);
		    EmColorTheme.DeepAnalysisQ2 = Color.FromArgb(255, 145, 20);
		    EmColorTheme.DeepAnalysisQ3 = Color.FromArgb(255, 115, 50);
		    EmColorTheme.DeepAnalysisQ4 = Color.FromArgb(255, 60, 60);


            EmColorTheme.NavigationBackgroundColor = Color.FromArgb(16, 16, 16);

			ColorThemes.Add(EmColorTheme.ColorPlan, EmColorTheme);



			//网页配色
			NetColorTheme.ColorBoard = Color.FromArgb(120, 120, 120);
			NetColorTheme.ColorAmount = Color.FromArgb(52, 52, 52);
			NetColorTheme.ColorVolume = Color.FromArgb(52, 52, 52);
			NetColorTheme.ColorFrame = Color.FromArgb(52, 52, 52);
			NetColorTheme.ColorSame = Color.FromArgb(108, 108, 108);
			NetColorTheme.ColorDownKline = Color.FromArgb(31, 151, 27);
			NetColorTheme.ColorDown = Color.FromArgb(2, 150, 14);
			NetColorTheme.ColorUp = Color.FromArgb(227, 0, 12);
			NetColorTheme.ColorBackGround = Color.FromArgb(237, 244, 252);
			NetColorTheme.ColorSerialNumber = Color.FromArgb(192, 192, 192);
			NetColorTheme.ColorCode = Color.Black;
			NetColorTheme.ColorName = Color.FromArgb(0, 255, 252);
			NetColorTheme.ColorCode_Name = Color.FromArgb(240, 248, 136);
			NetColorTheme.ColorPieBuyXLarge = Color.FromArgb(173, 0, 13);
			NetColorTheme.ColorPieBuyLarge = Color.FromArgb(191, 41, 53);
			NetColorTheme.ColorPieBuyMiddle = Color.FromArgb(214, 75, 87);
			NetColorTheme.ColorPieBuySmall = Color.FromArgb(227, 124, 132);
			NetColorTheme.ColorPieSellXLarge = Color.FromArgb(119, 178, 229);
			NetColorTheme.ColorPieSellLarge = Color.FromArgb(79, 152, 216);
			NetColorTheme.ColorPieSellMiddle = Color.FromArgb(51, 120, 179);
			NetColorTheme.ColorPieSellSmall = Color.FromArgb(30, 95, 151);
			NetColorTheme.ColorUpBackground = Color.FromArgb(237, 244, 252);
			NetColorTheme.ColorDownBackground = Color.FromArgb(237, 244, 252);
			NetColorTheme.ColorPlus = Color.FromArgb(255, 0, 255);
			NetColorTheme.ColorMultiTrendBackground = Color.FromArgb(191, 220, 252);
			NetColorTheme.ColorTitleBackground = Color.FromArgb(218, 218, 218);
			NetColorTheme.ColorTitleContext = Color.FromArgb(52, 52, 52);
			NetColorTheme.IndexAverageColor = new Color[]
                                                   {
                                                       Color.FromArgb(33, 0, 95), Color.FromArgb(120, 157, 148),
                                                       Color.FromArgb(145, 101, 146), Color.FromArgb(54, 0, 115),
                                                       Color.FromArgb(191, 148, 81), Color.FromArgb(83, 83, 84)
                                                   };
			NetColorTheme.ColorVTabBackground = Color.FromArgb(196, 196, 196);
			NetColorTheme.ColorVTabItemBackGround = Color.FromArgb(218, 218, 218);
			NetColorTheme.ColorVTabSelected = Color.FromArgb(236, 242, 249);
			NetColorTheme.ColorVTabFontNormal = Color.FromArgb(35, 35, 35);
			NetColorTheme.ColorVTabFontSelect = Color.FromArgb(35, 35, 35);
			NetColorTheme.ColorCross = Color.FromArgb(190, 190, 190);
			NetColorTheme.ColorSplit = NetColorTheme.ColorBoard;
			NetColorTheme.ColorTrendPrice = Color.FromArgb(30, 95, 151);
			NetColorTheme.ColorTrendBackground = Color.FromArgb(220, 220, 220);
			NetColorTheme.ColorHTabBackground = Color.FromArgb(196, 196, 196);
			NetColorTheme.ColorHTabFontNormal = Color.FromArgb(35, 35, 35);
			NetColorTheme.ColorHTabFontSelect = Color.FromArgb(35, 35, 35);
			NetColorTheme.ColorHTabFontNormal = Color.FromArgb(35, 35, 35);
			NetColorTheme.ColorHTabItemNormalBKBegin = Color.FromArgb(255, 255, 255);
			NetColorTheme.ColorHTabItemNormalBKMiddle = Color.FromArgb(241, 241, 241);
			NetColorTheme.ColorHTabItemNormalBKMiddle2 = Color.FromArgb(225, 225, 225);
			NetColorTheme.ColorHTabItemNormalBKEnd = Color.FromArgb(234, 234, 234);
			NetColorTheme.ColorHTabItemSelectBKBegin = Color.FromArgb(224, 243, 250);
			NetColorTheme.ColorHTabItemSelectBKMiddle = Color.FromArgb(216, 240, 252);
			NetColorTheme.ColorHTabItemSelectBKMiddle2 = Color.FromArgb(185, 226, 246);
			NetColorTheme.ColorHTabItemSelectBKEnd = Color.FromArgb(182, 223, 253);
			NetColorTheme.DealFontColor = Color.Black;
			NetColorTheme.DealBackGroundColor = Color.FromArgb(191, 220, 252);
			NetColorTheme.CancelFontBackGroundColor = Color.FromArgb(255, 255, 0);
			NetColorTheme.NewlyBackGroundColor = Color.FromArgb(191, 220, 252);
			NetColorTheme.LargeFontColor = Color.FromArgb(255, 79, 255);
			NetColorTheme.HugeFontColor = Color.FromArgb(191, 2, 232);
			NetColorTheme.ColorArrow = Color.FromArgb(75, 75, 75);
			NetColorTheme.ColorArrow = Color.FromArgb(75, 75, 75);
			NetColorTheme.ColorArrowGray = Color.FromArgb(75, 75, 75);
			NetColorTheme.ColorBar = Color.FromArgb(230, 230, 230);
			NetColorTheme.ColorBarLightColor = Color.FromArgb(252, 252, 252);
			NetColorTheme.PenCurrentDataRowLine = Color.FromArgb(249, 107, 39);
			NetColorTheme.ColorGray = Color.Gray;
			NetColorTheme.ColorCodeName = Color.FromArgb(52, 52, 52);
			NetColorTheme.ColorBlueName = Color.FromArgb(31, 151, 27);
			NetColorTheme.ColorRedName = Color.FromArgb(227, 0, 12);
			NetColorTheme.ColorSkyBlue = Color.SkyBlue;
			NetColorTheme.ColorPurple = Color.Purple;
			NetColorTheme.ColorBule = Color.Blue;
            NetColorTheme.ColorGrayWhite = Color.FromArgb(192, 192, 192);
            NetColorTheme.ColorYellow = Color.Yellow;
            NetColorTheme.ColorSurgedLimit = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorDeclineLimit = Color.FromArgb(80, 255, 80);
            NetColorTheme.ColorOpenSurgedLimit = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorOpenDeclineLimit = Color.FromArgb(80, 255, 80);
            NetColorTheme.ColorBiggerAskOrder = Color.FromArgb(192, 192, 0);
            NetColorTheme.ColorBiggerBidOrder = Color.FromArgb(192, 192, 0);
            NetColorTheme.ColorInstitutionAskOrder = Color.FromArgb(252, 112, 0);
            NetColorTheme.ColorInstitutionBidOrder = Color.FromArgb(58, 115, 35);
            NetColorTheme.ColorRocketLaunch = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorStrongRebound = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorHighDiving = Color.FromArgb(80, 255, 80);
            NetColorTheme.ColorSpeedupDown = Color.FromArgb(80, 255, 80);
            NetColorTheme.ColorCancelBigAskOrder = Color.FromArgb(80, 255, 80);
            NetColorTheme.ColorCancelBigBidOrder = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorInstitutionBidTrans = Color.FromArgb(80, 255, 80);
            NetColorTheme.ColorInstitutionAskTrans = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorMultiSameAskOrders = Color.FromArgb(255, 82, 82);
            NetColorTheme.ColorMultiSameBidOrders = Color.FromArgb(80, 255, 80);
			//订单占比
			NetColorTheme.ColorBigBuyOrder = Color.FromArgb(255, 0, 0);
			NetColorTheme.ColorMidBuyOrder = Color.FromArgb(255, 128, 0);
			NetColorTheme.ColorSmallBuyOrder = Color.FromArgb(255, 255, 0);
			NetColorTheme.ColorBigSellOrder = Color.FromArgb(0, 255, 0);
			NetColorTheme.ColorMidSellOrder = Color.FromArgb(0, 0, 255);
			NetColorTheme.ColorSmallSellOrder = Color.FromArgb(76, 255, 252);
			NetColorTheme.ColorMmdSpliter = Color.FromArgb(100, 100, 100);
			NetColorTheme.InfopanelHighLightColor = Color.FromArgb(60, 60, 60);
			NetColorTheme.InfopanelInnerTitleBkgColor = Color.FromArgb(12, 12, 12);
		    NetColorTheme.InfopanelNormalWhiteColor = Color.FromArgb(210, 210, 210);
			NetColorTheme.NewsSelectedColor = Color.FromArgb(126, 126, 126);

			NetColorTheme.OrderQueueNewColor = Color.FromArgb(0, 0, 128);
			NetColorTheme.OrderQueueCancelColor = Color.FromArgb(255, 255, 78);
			NetColorTheme.OrderQueuePartDealBorderColor = Color.FromArgb(0, 0, 77);
			NetColorTheme.OrderQueuePartDealFillColor = Color.FromArgb(192, 56, 56, 56);
			NetColorTheme.OrderQueueKAboveColor = Color.FromArgb(2, 59, 63);
			NetColorTheme.OrderQueue5KAboveColor = Color.FromArgb(134, 3, 134);
		    NetColorTheme.OrderQueueSpliterColor = Color.FromArgb(25, 25, 25);

		    NetColorTheme.TopBannerMenuNormal = Color.FromArgb(224, 224, 224);
		    NetColorTheme.TopBannerMenuOn = Color.FromArgb(255, 74, 0);
			ColorThemes.Add(NetColorTheme.ColorPlan, NetColorTheme);

		}

        /// <summary>
        /// 设置当前的颜色主题
        /// </summary>
		public static void SetCurrentColorThem(string colorThemeName) {
			if(ColorThemes.ContainsKey(colorThemeName))
				CurrentColorTheme = ColorThemes[colorThemeName];
		}


	}

    /// <summary>
    /// 
    /// </summary>
	public class ColorTheme {
		/// <summary>
		///   ColorTheme的名称
		/// </summary>
		public string ColorPlan
		{
		    get { return _colorPlan; }
		    private set { _colorPlan = value; }
		}

        /// <summary>
		///   根据一个名字创建一个ColorTheme
		/// </summary>
        /// <param name = "colorPlan"></param>
		public ColorTheme(string colorPlan) {
			ColorPlan = colorPlan;
		}

		#region 颜色定义

		#region 通用

		/// <summary>
		///   背景色
		/// </summary>
		public Color ColorBackGround ;

		/// <summary>
		///   框架颜色 红色边框
		/// </summary>
		public Color ColorBoard ;
        /// <summary>
        /// 
        /// </summary>
        public Color ColorDDXDown;

		/// <summary>
		///   边框的颜色  灰色边框
		/// </summary>
		public Color ColorFrame ;
		
        /// <summary>
		///   上涨显示颜色
		/// </summary>
		public Color ColorUp ;

		/// <summary>
		///   下跌的颜色
		/// </summary>
        public Color ColorDown ;

        /// <summary>
        ///   报价中使用的平盘显示颜色
        /// </summary>
        public Color ColorEqual ;

		/// <summary>
		///   平盘显示颜色
		/// </summary>
		public Color ColorSame ;

		/// <summary>
		///   成交金额的颜色
		/// </summary>
		public Color ColorAmount ;

        /// <summary>
        /// 走势k线坐标轴颜色
        /// </summary>
        public Color ColorKlineTrendCor;
		/// <summary>
		///   成交量的颜色
		/// </summary>
		public Color ColorVolume ;
        /// <summary>
        /// 走势k线成交量颜色
        /// </summary>
        public Color ColorKlineTrendVolume ;
        /// <summary>
        /// k线Drawbox颜色
        /// </summary>
        public Color ColorKlineDrawBox ;
        /// <summary>
        /// 量比颜色
        /// </summary>
        public Color ColorLiangbi ;
        /// <summary>
        /// 走势k线label背景色
        /// </summary>
        public Color ColorLabelBG ;
        /// <summary>
        /// 走势行业标题颜色
        /// </summary>
        public Color ColorIndustryTitle ;
		/// <summary>
		/// 十字光标的颜色
		/// </summary>
		public Color ColorCross ;

		/// <summary>
		/// 按钮的颜色
		/// </summary>
		public Color ColorGray ;
        /// <summary>
        /// 名称的颜色
        /// </summary>
		public Color ColorName ;
		/// <summary>
		/// 股票代码，名称颜色
		/// </summary>
		public Color ColorCodeName ;
		/// <summary>
		/// 股票名称颜色（蓝色）
		/// </summary>
		public Color ColorBlueName ;
		/// <summary>
		/// 股票名称颜色（红色）
		/// </summary>
		public Color ColorRedName ;
        /// <summary>
        /// 
        /// </summary>
		public Color ColorSkyBlue ;
        /// <summary>
        /// 
        /// </summary>
		public Color ColorPurple ;
        /// <summary>
        /// 
        /// </summary>
		public Color ColorBule ;
        /// <summary>
        /// 
        /// </summary>
		public Color ColorGrayWhite ;
        /// <summary>
        /// 
        /// </summary>
        public Color ColorYellow ;
		#endregion

		#region 自绘控件

        /// <summary>
        ///   Tab项背景色
        /// </summary>
        public Color ColorVTabItemBackGround;

        /// <summary>
        ///   Tab项选中的颜色
        /// </summary>
        public Color ColorVTabSelected;

		/// <summary>
		/// Tab条背景色
		/// </summary>
		public Color ColorVTabBackground ;

		/// <summary>
		/// Tab项未选中时字体的颜色
		/// </summary>
		public Color ColorVTabFontNormal ;

		/// <summary>
		/// Tab项选中时字体的颜色
		/// </summary>
		public Color ColorVTabFontSelect ;

		/// <summary>
		/// HTab控件背景色
		/// </summary>
		public Color ColorHTabBackground ;

		/// <summary>
		/// 水平Tab项未选中背景
		/// </summary>
		public Color ColorHTabItemNormalBk ;

		/// <summary>
		/// 水平Tab项选中背景
		/// </summary>
		public Color ColorHTabItemSelectBk ;

		/// <summary>
		/// 水平Tab项未选中字体颜色
		/// </summary>
		public Color ColorHTabFontNormal ;

		/// <summary>
		/// 水平Tab项选中字体颜色
		/// </summary>
		public Color ColorHTabFontSelect ;

		/// <summary>
		/// 箭头激活的颜色
		/// </summary>
		public Color ColorArrow ;

		/// <summary>
		/// Tab页渐变色1
		/// </summary>
		public Color ColorHTabItemNormalBKBegin;

		/// <summary>
		/// Tab页渐变色4
		/// </summary>
		public Color ColorHTabItemNormalBKEnd;

		/// <summary>
		/// Tab页渐变色2
		/// </summary>
		public Color ColorHTabItemNormalBKMiddle;

		/// <summary>
		/// Tab页渐变色3
		/// </summary>
		public Color ColorHTabItemNormalBKMiddle2;

		/// <summary>
		/// Tab页选中时渐变色1
		/// </summary>
		public Color ColorHTabItemSelectBKBegin;

		/// <summary>
		/// Tab页选中时渐变色4
		/// </summary>
		public Color ColorHTabItemSelectBKEnd;

		/// <summary>
		/// Tab页选中时渐变色2
		/// </summary>
		public Color ColorHTabItemSelectBKMiddle;

		/// <summary>
		/// Tab页选中时渐变色3
		/// </summary>
		public Color ColorHTabItemSelectBKMiddle2;

		/// <summary>
		/// 箭头失效的颜色
		/// </summary>
		public Color ColorArrowGray;
		/// <summary>
		/// 滚动条的颜色
		/// </summary>
		public Color ColorBar;
		/// <summary>
		/// 滚动条边框的颜色
		/// </summary>
		public Color ColorBarLightColor;
		/// <summary>
		/// 当前选中的数据行
		/// </summary>
		public Color PenCurrentDataRowLine;



		//===



		/// <summary>
		/// 常态文字颜色
		/// </summary>
		public Color CommonTextColor;

		/// <summary>
		/// 选中文字颜色
		/// </summary>
		public Color SelectedTextColor;
		/// <summary>
		/// 选中文字的阴影颜色
		/// </summary>
		public Color SelectedTextShadowColor;
		/// <summary>
		/// 按钮未选中渐变开始颜色
		/// </summary>
		public Color CommonButtonBeginColor;

		/// <summary>
		/// 按钮未选中渐变终止颜色
		/// </summary>
		public Color CommonButtonEndColor;

		/// <summary>
		/// 按钮选中渐变开始颜色
		/// </summary>
		public Color SelectedButtonBeginColor;

		/// <summary>
		/// 按钮选中渐变终止颜色
		/// </summary>
		public Color SelectedButtonEndColor;


		/// <summary>
		/// 按钮未选中上高光颜色
		/// </summary>
		public Color CommonButtonUpHighLightColor;

		/// <summary>
		/// 按钮未选中下压线颜色
		/// </summary>
		public Color CommonButtonDownLineColor;

		/// <summary>
		/// 按钮选中上高光颜色
		/// </summary>
		public Color SelectedButtonUpHighLightColor;

		/// <summary>
		/// 按钮选中下压线颜色
		/// </summary>
		public Color SelectedButtonDownLineColor;

		/// <summary>
		/// 按钮选中下压线颜色
		/// </summary>
		public Color SelectedButtonSideColor;
		/// <summary>
		/// 按钮分割线1
		/// </summary>
		public Color ButtonSplitColor1;

		/// <summary>
		/// 按钮分割线2
		/// </summary>
		public Color ButtonSplitColor2;


		/// <summary>
		/// 报价列的分割线
		/// </summary>
		public Color ColumnSplitColor;



		/// <summary>
		/// 滚动条起始色
		/// </summary>
		public Color BarBeginColor;
		/// <summary>
		/// 滚动条结束色
		/// </summary>
		public Color BarEndColor;
        /// <summary>
        /// 滚动条选中时开始颜色
        /// </summary>
        public Color BarMouseOnBeginColor;
        /// <summary>
        /// 滚动条选中时结束颜色
        /// </summary>
        public Color BarMouseOnEndColor;
		/// <summary>
		/// 小三角颜色上
		/// </summary>
		public Color BarArrowUpColor;
		/// <summary>
		/// 小三角颜色下
		/// </summary>
		public Color BarArrowDownColor;
		/// <summary>
		/// 高光起始色左
		/// </summary>
		public Color BarHighLightBeginColor;
		/// <summary>
		/// 高光结束色右
		/// </summary>
		public Color BarHighLightEndColor;
        /// <summary>
        /// 高光在mouseon时的开始颜色
        /// </summary>
        public Color BarHighLightMouseOnBeginColor;
        /// <summary>
        /// 高光在mouseon时的结束颜色
        /// </summary>
        public Color BarHighLightMouseOnEndColor;
		/// <summary>
		/// 滚动条三条线颜色一
		/// </summary>
		public Color BarThreeLineBeginColor;

        public Color BarThreeLineEndColor;
		/// <summary>
		/// 滚动条三条线颜色二
		/// </summary>
		public Color BarThreeLineShadowColor;
		/// <summary>
		/// 滚动条轨迹底色
		/// </summary>
		public Color BarBackGroundColor;
		/// <summary>
		/// 滚动条边框底色
		/// </summary>
		public Color BarBorderColor;



		/// <summary>
		/// 隐藏按钮底色
		/// </summary>
		public Color HideButtonBackGroundColor;
		/// <summary>
		/// 隐藏按钮高光1
		/// </summary>
		public Color HideButtonHighLightColor1;
		/// <summary>
		/// 隐藏按钮高光2
		/// </summary>
		public Color HideButtonHighLightColor2;
		/// <summary>
		/// 隐藏按钮高光3
		/// </summary>
		public Color HideButtonHighLightColor3;


		/// <summary>
		/// button三角未选中颜色
		/// </summary>
		public Color ButtonTriangleNormalColor;
		/// <summary>
		/// button三角选中颜色
		/// </summary>
		public Color ButtonTriangleSelectedColor;


		/// <summary>
		/// 走势K线container边界颜色
		/// </summary>
		public Color QuoteBouderColor;
        /// <summary>
        /// 虚线颜色
        /// </summary>
        public Color QuoteBouderDashColor;

		/// <summary>
		/// 新闻行的上高光色
		/// </summary>
		public Color NewsTitleBackUpLightColor;
		/// <summary>
		/// 新闻行的起始色
		/// </summary>
		public Color NewsTitleBackBeginColor;
		/// <summary>
		/// 新闻行的结束色
		/// </summary>
		public Color NewsTitleBackEndColor;
		/// <summary>
		/// 新闻标题的颜色
		/// </summary>
		public Color NewsTitleTextColor;
		/// <summary>
		/// 新闻标题分割线的颜色
		/// </summary>
        public Color NewsTitleSplitColor;
        /// <summary>
        /// 新闻类别的颜色
        /// </summary>
        public Color NewsTypeColor;
        /// <summary>
        /// 新闻标题的颜色
        /// </summary>
        public Color NewsTextWhiteColor;

		/// <summary>
		/// tab为NormalButton样式时的边框颜色
		/// </summary>
		public Color NormalTabButtonBounderColor;
		/// <summary>
		///  tab为NormalButton样式时的灰色背景颜色
		/// </summary>
		public Color NormalTabButtonGrayBackColor;
		/// <summary>
		/// tab为NormalButton样式时的黑色背景颜色
		/// </summary>
		public Color NormalTabButtonBlackBackColor;
		/// <summary>
		/// tab为黄色字体颜色1(254,254,0)
		/// </summary>
		public Color NormalTabButtonYellowTextColor;
		/// <summary>
		/// tab为黄色字体颜色1(255,255,0)
		/// </summary>
		public Color NormalTabButtonYellowTextColor1;
		/// <summary>
		/// tab为NormalButton样式时的白色字体颜色
		/// </summary>
		public Color NormalTabButtonWhiteTextColor;
		/// <summary>
		/// tab为NormalButton样式时的蓝色字体颜色
		/// </summary>
		public Color NormalTabButtonBlueTextColor;
		/// <summary>
		/// 综合排名标题的颜色
		/// </summary>
		public Color ZHTitleColor;
        /// <summary>
        /// 
        /// </summary>
		public Color DealDetailTilteColor;
		/// <summary>
		/// 选中的橘黄色
		/// </summary>
		public Color SelectedBackColor;
		/// <summary>
		/// 多股同列多周期中选中的边框颜色
		/// </summary>
		public Color MultiSelectedBorderColor;

		/// <summary>
		/// 左侧tab页高光色
		/// </summary>
		public Color VTabHighLightColor;
		/// <summary>
		/// 左侧tab页阴影颜色
		/// </summary>
		public Color VTabShadowColor;
		/// <summary>
		/// 左侧tab页渐变起始色
		/// </summary>
		public Color VTabBeginColor;
		/// <summary>
		/// 左侧tab页渐变结束色
		/// </summary>
		public Color VTabEndColor;
		/// <summary>
		/// 左侧tab页边框色
		/// </summary>
		public Color VTabBorderColor;
		/// <summary>
		/// 左侧tab页选中字体颜色
		/// </summary>
		public Color VTabTextSelectedColor;
        /// <summary>
        /// 左侧tab页选中字体阴影颜色
        /// </summary>
        public Color VTabTextSelectedColor1;
		/// <summary>
		/// 左侧tab页未选中字体前景颜色
		/// </summary>
		public Color VTabTextNormalColor;
		/// <summary>
		/// 左侧tab页未选中字体背景颜色
		/// </summary>
		public Color VTabTextNormalColor1;
        /// <summary>
        /// 垂直tab渐变开始色
        /// </summary>
        public Color VTabActiveBeginColor;
        /// <summary>
        /// 垂直tab渐变终止色
        /// </summary>
        public Color VTabActiveEndColor;

        public Color VTabActiveInnerBorderColor;

        public Color VTabActiveHighlightColor;

		/// <summary>
		/// k线、走势按钮选中文字颜色
		/// </summary>
		public Color KlineButtonTextSelectColor;
		/// <summary>
		/// k线、走势按钮正常文字颜色
		/// </summary>
		public Color KlineButtonTextNormalColor;
		/// <summary>
		/// k线走势按钮正常背景色低
		/// </summary>
		public Color KlineButtonNormalLowColor;
		/// <summary>
		/// k线走势按钮正常背景色高
		/// </summary>
		public Color KlineButtonNormalHighColor;
		/// <summary>
		/// k线走势按钮选中背景色高
		/// </summary>
		public Color KlineButtonSelectHighColor;
		/// <summary>
		/// k线走势按钮选中背景色低
		/// </summary>
		public Color KlineButtonSelectLowColor;
		/// <summary>
		/// k线走势按钮边框色
		/// </summary>
		public Color KlineButtonFrameColor;
		/// <summary>
		///k线指标按钮边框色
		/// </summary>
		public Color KlineIndexButtonFrameColor;
		/// <summary>
		/// k线走势加入自选按钮文字颜色
		/// </summary>
		public Color KlineButtonSpecialTextColor;
		/// <summary>
		/// 多日走势背景色
		/// </summary>
		public Color MultiTrendBgColor;

        /// <summary>
        /// split高亮颜色（未选中）
        /// </summary>
        public Color SplitColorHighNormal;
        /// <summary>
        /// split低亮颜色（未选中）
        /// </summary>
        public Color SplitColorLowNormal;
        /// <summary>
        /// split高亮颜色（选中）
        /// </summary>
        public Color SplitColorHighActive;
        /// <summary>
        /// split低亮颜色（选中）
        /// </summary>
        public Color SplitColorLowActive;
        /// <summary>
        /// split箭头选中色
        /// </summary>
        public Color SplitArrowColorActive;
        /// <summary>
        /// split箭头未选中色
        /// </summary>
        public Color SplitArrowColorNormal;
        /// <summary>
        /// 股吧奇数行背景色
        /// </summary>
        public Color BbsOddRowBackColor;
        /// <summary>
        /// 股吧偶数行背景色
        /// </summary>
        public Color BbsEvenRowBackColor;

		#endregion

		#region 走势
		/// <summary>
		/// 走势背景网格线颜色
		/// </summary>
		public Color ColorTrendBackground ;

		/// <summary>
		/// 分隔条的颜色
		/// </summary>
		public Color ColorSplit ;

		/// <summary>
		/// 走势价格线的颜色
		/// </summary>
		public Color ColorTrendPrice ;

		/// <summary>
		/// 上涨股票的背景色
		/// </summary>
		public Color ColorUpBackground ;

		/// <summary>
		/// 下跌股票的背景色
		/// </summary>
		public Color ColorDownBackground ;

		/// <summary>
		/// 走势叠加股票的颜色
		/// </summary>
		public Color ColorPlus ;

		/// <summary>
		/// 多日走势背景
		/// </summary>
		public Color ColorMultiTrendBackground ;
        /// <summary>
        /// k线走势边框颜色
        /// </summary>
        public Color TrendFrameColor;
        /// <summary>
        /// 走势买卖档绿色
        /// </summary>
        public Color TrendMaimaiDown;
        /// <summary>
        /// 走势买卖档红色
        /// </summary>
        public Color TrendMaimaiUp;
		#endregion

		#region K线

		/// <summary>
		///   K线下跌的颜色
		/// </summary>
		public Color ColorDownKline ;

		/// <summary>
		/// 均线颜色
		/// </summary>
		public Color[] IndexAverageColor ;


		#endregion

		#region 报价
		/// <summary>
		/// 浅蓝背景
		/// </summary>
		public Color LightBlueBackground ;
		/// <summary>
		/// 深蓝背景
		/// </summary>
		public Color DarkBlueBackground ;
		#endregion

		#region 买卖盘
        /// <summary>
        /// 走势买卖档标题颜色
        /// </summary>
        public Color ColorMaimaiTitle;
		/// <summary>
		///   报价列表序号的颜色
		/// </summary>
		public Color ColorSerialNumber ;

		/// <summary>
		///   代码的颜色
		/// </summary>
		public Color ColorCode ;

		/// <summary>
		///   报价列表代码和股票名称的颜色
		/// </summary>
		public Color ColorCode_Name ;


		/// <summary>
		/// Title的背景色
		/// </summary>
		public Color ColorTitleBackground ;

		/// <summary>
		/// Title文字的颜色
		/// </summary>
		public Color ColorTitleContext ;


		/// <summary>
		///   饼图中 大户买入的颜色
		/// </summary>
		public Color ColorPieBuyLarge ;

		/// <summary>
		///   饼图中 中户买入的颜色
		/// </summary>
		public Color ColorPieBuyMiddle ;

		/// <summary>
		///   饼图中 散户买入的颜色
		/// </summary>
		public Color ColorPieBuySmall ;

		/// <summary>
		///   饼图中 超级大户买入的颜色
		/// </summary>
		public Color ColorPieBuyXLarge ;

		/// <summary>
		///   饼图中 大户卖出的颜色
		/// </summary>
		public Color ColorPieSellLarge ;

		/// <summary>
		///   饼图中 中户卖出的颜色
		/// </summary>
		public Color ColorPieSellMiddle ;

		/// <summary>
		///   饼图中 散户卖出的颜色
		/// </summary>
		public Color ColorPieSellSmall ;

		/// <summary>
		///   饼图中 超级大户卖出的颜色
		/// </summary>
		public Color ColorPieSellXLarge ;

		#endregion

		#region 挂单撤单

		/// <summary>
		/// Level2挂单队列 新单的背景色
		/// </summary>
		public Color NewlyBackGroundColor ;

		/// <summary>
		/// Level2挂单队列 已成交的背景色
		/// </summary>
		public Color DealBackGroundColor ;

		/// <summary>
		/// Level2挂单队列 撤单的背景色
		/// </summary>
		public Color CancelFontBackGroundColor ;

		/// <summary>
		/// Level2挂单队列 已成交的字体颜色
		/// </summary>
		public Color DealFontColor ;

		/// <summary>
		/// Level2挂单队列 大单的字体颜色
		/// </summary>
		public Color LargeFontColor ;

		/// <summary>
		/// Level2挂单队列 特大单的字体颜色
		/// </summary>
		public Color HugeFontColor ;

		#endregion

        #region 导航Navigation
        /// <summary>
        /// 导航背景色
        /// </summary>
        public Color NavigationBackgroundColor;

        #endregion
        #endregion

        #region 短线精灵
        /// <summary>
		/// 封涨停板
		/// </summary>
		public Color ColorSurgedLimit ;
		/// <summary>
		/// 封跌停板
		/// </summary>
		public Color ColorDeclineLimit ;
		/// <summary>
		/// 打开涨停
		/// </summary>
		public Color ColorOpenSurgedLimit ;
		/// <summary>
		/// 打开跌停
		/// </summary>
		public Color ColorOpenDeclineLimit ;
		/// <summary>
		/// 有大买盘
		/// </summary>
		public Color ColorBiggerAskOrder ;
		/// <summary>
		/// 有大卖盘
		/// </summary>
		public Color ColorBiggerBidOrder ;
		/// <summary>
		/// 机构买单
		/// </summary>
		public Color ColorInstitutionAskOrder ;
		/// <summary>
		/// 机构卖单
		/// </summary>
		public Color ColorInstitutionBidOrder ;
		/// <summary>
		/// 火箭发射
		/// </summary>
		public Color ColorRocketLaunch ;
		/// <summary>
		/// 快速反弹
		/// </summary>
		public Color ColorStrongRebound ;
		/// <summary>
		/// 高台跳水
		/// </summary>
		public Color ColorHighDiving ;
		/// <summary>
		/// 加速下跌
		/// </summary>
		public Color ColorSpeedupDown ;
		/// <summary>
		/// 买入撤单
		/// </summary>
		public Color ColorCancelBigAskOrder ;
		/// <summary>
		/// 卖出撤单
		/// </summary>
		public Color ColorCancelBigBidOrder ;
		/// <summary>
		/// 大笔卖出
		/// </summary>
		public Color ColorInstitutionBidTrans ;
		/// <summary>
		/// 大笔买入
		/// </summary>
		public Color ColorInstitutionAskTrans ;
		/// <summary>
		/// 买单分单
		/// </summary>
		public Color ColorMultiSameAskOrders ;
		/// <summary>
		/// 卖单分单
		/// </summary>
		public Color ColorMultiSameBidOrders ;
		#endregion

		#region 订单占比
		/// <summary>
		/// 大买单
		/// </summary>
		public Color ColorBigBuyOrder ;
		/// <summary>
		/// 中买单
		/// </summary>
		public Color ColorMidBuyOrder ;
		/// <summary>
		/// 小买单
		/// </summary>
		public Color ColorSmallBuyOrder ;
		/// <summary>
		/// 大卖单
		/// </summary>
		public Color ColorBigSellOrder ;
		/// <summary>
		/// 中卖单
		/// </summary>
		public Color ColorMidSellOrder ;
		/// <summary>
		/// 小卖单
		/// </summary>
		public Color ColorSmallSellOrder ;
		#endregion

        /// <summary>
        /// 买卖档中间分隔线
        /// </summary>
        public Color ColorMmdSpliter;

        /// <summary>
        /// 行情内部分割线颜色
        /// </summary>
        public Color ColorQuoteInnerSpliter;

        /// <summary>
        /// 信息栏中标题栏高光
        /// </summary>
        public Color InfopanelHighLightColor;

        /// <summary>
        /// 信息栏中Chart内部Title的背景色
        /// </summary>
        public Color InfopanelInnerTitleBkgColor;

        /// <summary>
        /// 信息面板正常的白色
        /// </summary>
        public Color InfopanelNormalWhiteColor;

        /// <summary>
        /// 新闻中选择色
        /// </summary>
        public Color NewsSelectedColor;

        /// <summary>
        /// 新闻查看过后的颜色
        /// </summary>
        public Color NewsViewedColor;

        /// <summary>
        /// F10正常文本颜色
        /// </summary>
        public Color F10NormalTextColor;

        /// <summary>
        /// F10选中文本颜色
        /// </summary>
        public Color F10SelectedTextColor;

        /// <summary>
        /// F10选中背景色
        /// </summary>
        public Color F10SelectedBackColor;

        /// <summary>
        /// F10边框颜色
        /// </summary>
        public Color F10BorderColor;

        /// <summary>
        /// 委托队列新增颜色
        /// </summary>
        public Color OrderQueueNewColor;

        /// <summary>
        /// 委托队列取消颜色
        /// </summary>
        public Color OrderQueueCancelColor;

        /// <summary>
        /// 委托队列部分成交填充色
        /// </summary>
        public Color OrderQueuePartDealFillColor;

        /// <summary>
        /// 委托队列部分成交边框颜色
        /// </summary>
        public Color OrderQueuePartDealBorderColor;

        /// <summary>
        /// 委托队列超过1000手颜色
        /// </summary>
        public Color OrderQueueKAboveColor;

        /// <summary>
        /// 委托队列超过5000手颜色
        /// </summary>
        public Color OrderQueue5KAboveColor;

        /// <summary>
        /// 委托队列分割线颜色
        /// </summary>
        public Color OrderQueueSpliterColor;

        public Color OrderDetailSpliterColor;

        /// <summary>
        /// 股吧评论分割线色
        /// </summary>
        public Color BBSSplitLineColor;

        /// <summary>
        /// 股吧按钮渐变开始颜色
        /// </summary>
        public Color BBSButtonBackgroundBeginColor;

        /// <summary>
        /// 股吧按钮渐变结束颜色
        /// </summary>
        public Color BBSButtonBackgroundEndColor;

        /// <summary>
        /// 股吧按钮鼠标选中时开始颜色
        /// </summary>
        public Color BBSButtonMouseOnBackgroundBeginColor;

        /// <summary>
        /// 股吧按钮鼠标选中时结束颜色
        /// </summary>
        public Color BBSButtonMouseOnBackgroundEndColor;

        /// <summary>
        /// 股吧按钮边框颜色
        /// </summary>
        public Color BBSButtonBorderColor;

        /// <summary>
        /// 股吧按钮边框高光
        /// </summary>
        public Color BBSButtonBorderHighlightColor;

        /// <summary>
        /// 股吧按钮边框鼠标选中时高光颜色
        /// </summary>
        public Color BBSButtonMouseOnBorderHighlightColor;

        /// <summary>
        /// 股吧按钮文字颜色
        /// </summary>
        public Color BBSButtonCaptionColor;

        /// <summary>
        /// 股吧按钮鼠标选中时按钮文字颜色
        /// </summary>
        public Color BBSButtonMouseOnCaptionColor;

        /// <summary>
        /// 股吧标题栏颜色
        /// </summary>
        public Color BBSTitleColor;

        /// <summary>
        /// 股吧内容颜色
        /// </summary>
        public Color BBSCaptionColor;

        /// <summary>
        /// 个股界面菜单颜色常态
        /// </summary>
        public Color TopBannerMenuNormal;

        /// <summary>
        /// 个股界面菜单颜色选中
        /// </summary>
        public Color TopBannerMenuOn;

        /// <summary>
        /// 深度分析柱状图第一季度颜色
        /// </summary>
        public Color DeepAnalysisQ1;

        /// <summary>
        /// 深度分析柱状图第二季度颜色
        /// </summary>
        public Color DeepAnalysisQ2;

        /// <summary>
        /// 深度分析柱状图第三季度颜色
        /// </summary>
        public Color DeepAnalysisQ3;

        /// <summary>
        /// 深度分析柱状图第四季度颜色
        /// </summary>
        public Color DeepAnalysisQ4;

        private string _colorPlan;
	}
}
