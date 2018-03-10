using System;
using System.Collections.Generic;
 
using System.Text;
using System.Threading;
using System.Timers;
using System.Runtime.InteropServices;
using dataquery;

namespace EmQComm
{
    public class TrendTimeUtility
    {
        public int Close;
        public int Open;

        public TrendTimeUtility(int open, int close)
        {
            this.Open = open;
            this.Close = close;
        }
    }

    public enum TypeCode
    {
        Na = 0,
        TC_AUS = 0x43,
        TC_Austria = 0x2f,
        TC_BCE = 0x35,
        TC_CMX = 0x6f,
        TC_COptionBuy = 0x3f,
        TC_COptionSell = 0x40,
        TC_CRB = 0x4e,
        TC_DutchAEX = 0x2e,
        TC_FME = 0x36,
        TC_ForexNonSpot = 0x79,
        TC_ForexSpot = 0x7a,
        TC_FXInter = 0x7f,
        TC_FXInterWinterSummer = 0x7c,
        TC_HK = 0x47,
        TC_HKCreditIndex = 0x93,
        TC_IB = 8,
        TC_JK = 0x24,
        TC_Korea = 0x26,
        TC_LME = 0x73,
        TC_LMEELEC = 0x72,
        TC_Malaysia = 0x25,
        TC_NewZealand = 0x2c,
        TC_Nikkei = 0x27,
        TC_Norway = 0x30,
        TC_Philippines = 40,
        TC_Russia = 0x31,
        TC_Sensex = 0x29,
        TC_SG = 0x71,
        TC_SH = 1,
        TC_ShfAg = 50,
        TC_ShfCu = 0x34,
        TC_SHHKCreditIndex = 0x42,
        TC_Singapore = 0x2a,
        TC_TW = 0x2b,
        TC_US = 0x65
    }

    public class MarketTime
    {
        public int CallAuctionTime;
        public int ClearTime;
        public int DelayTime;
        public int KLineTime;
        public int LastTradeTime;
        public int PushTime;
        public int FirstOpenTime;
        public List<TrendTimeUtility> TrendTimeList;
    }

    public class OneMarketTradeDateDataRec
    {
        public byte DstStatus;
        public EmQComm.TypeCode MarketTypeCode;
        public List<TradeDateStruct> TradeDateDict = new List<TradeDateStruct>();
        public List<int> TradeDateList = new List<int>(1);

        [StructLayout(LayoutKind.Sequential)]
        public struct TradeDateStruct
        {
            public int Date;
            public byte Type;
        }
    }

    public class TimeUtilities
    {
        private static byte _austriaIsSummerTime;
        private static MarketTime _austriaMarketTime = new MarketTime();
        private static MarketTime _austriaWinterMarketTime = new MarketTime();
        private static byte _axatIsSummerTime;
        private static MarketTime _axatMarketTime = new MarketTime();
        private static MarketTime _axatWinterMarketTime = new MarketTime();
        private static MarketTime _BCEMarketTime = new MarketTime();
        private static MarketTime _crbMarketTime = new MarketTime();
        private static MarketTime _czcsrMarketTime = new MarketTime();
        private static MarketTime _dutchAEXMarketTime = new MarketTime();
        private static MarketTime _dutchAEXWinterMarketTime = new MarketTime();
        private static byte _dutchIsSummerTime;
        private static MarketTime _FMEMarketTime = new MarketTime();
        private static MarketTime _ForexInterMarketSummerTime = new MarketTime();
        private static MarketTime _ForexInterMarketTime = new MarketTime();
        private static MarketTime _ForexInterMarketWinterTime = new MarketTime();
        private static byte _forexIsSummerTime;
        private static MarketTime _forexNonSpotMarketTime = new MarketTime();
        private static MarketTime _forexSpotMarketTime = new MarketTime();
        private static MarketTime _futuresMarketTime = new MarketTime();
        private static MarketTime _hkCreditIndexMarketTime = new MarketTime();
        private static MarketTime _hkHalfMarketTime = new MarketTime();
        private static MarketTime _hkMarketTime = new MarketTime();
        private static MarketTime _hkWholeMarketTime = new MarketTime();
        private static MarketTime _hsiHalfMarketTime = new MarketTime();
        private static MarketTime _hsiMarketTime = new MarketTime();
        private static MarketTime _hsiWholeMarketTime = new MarketTime();
        private static MarketTime _ibMarketTime = new MarketTime();
        private static MarketTime _ifMarketTime = new MarketTime();
        private static MarketTime _indexFutureMarketTime = new MarketTime();
        private static byte _ixicIsSummerTime;
        private static MarketTime _ixicMarketTime = new MarketTime();
        private static MarketTime _ixicWinterMarketTime = new MarketTime();
        private static MarketTime _jkMarketTime = new MarketTime();
        private static MarketTime _koreaMarketTime = new MarketTime();
        private static MarketTime _malaysiaMarketTime = new MarketTime();
        private static byte _newZealandIsSummerTime;
        private static MarketTime _newZealandMarketTime = new MarketTime();
        private static MarketTime _newZealandWinterMarketTime = new MarketTime();
        private static MarketTime _nikkeiMarketTime = new MarketTime();
        private static byte _norwayIsSummerTime;
        private static MarketTime _norwayMarketTime = new MarketTime();
        private static MarketTime _norwayWinterMarketTime = new MarketTime();
        private static short _oldPoint;
        private static int _oldTime;
        private static byte _osCBOTIsSummerTime;
        private static MarketTime _osFuturesCBOTMarketTime = new MarketTime();
        private static MarketTime _osFuturesCBOTWinterMarketTime = new MarketTime();
        private static MarketTime _osFuturesLMEElec = new MarketTime();
        private static MarketTime _osFuturesMarketTime = new MarketTime();
        private static MarketTime _osFuturesSGXMarketTime = new MarketTime();
        private static byte _osFutureSummerTime;
        private static MarketTime _osFuturesVenue = new MarketTime();
        private static MarketTime _osFuturesWinterLMEElec = new MarketTime();
        private static MarketTime _osFuturesWinterVenue = new MarketTime();
        private static MarketTime _osFutureWinterMarketTime = new MarketTime();
        private static byte _osLMEElecIsSummerTime;
        private static byte _osVenueIsSummerTime;
        private static MarketTime _philippinesMarketTime = new MarketTime();
        private static MarketTime _rateSwapMarketTime = new MarketTime();
        private static MarketTime _russiaMarketTime = new MarketTime();
        private static MarketTime _sensexMarketTime = new MarketTime();
        private static MarketTime _shCreditIndexMarketTime = new MarketTime();
        private static MarketTime _shfAGMarketTIme = new MarketTime();
        private static MarketTime _shfCUMarketTime = new MarketTime();
        private static MarketTime _shfRuMarketTime = new MarketTime();
        private static MarketTime _shIndexMarketTime = new MarketTime();
        private static MarketTime _shMarketTime = new MarketTime();
        private static MarketTime _singaporeMarketTime = new MarketTime();
        private static MarketTime _taiwanMarketTime = new MarketTime();
        private static byte _usIsSummerTime;
        private static MarketTime _usMarketTime = new MarketTime();
        private static MarketTime _usWinterMarketTime = new MarketTime();
        public static Dictionary<MarketType, MarketTime> MarketStatus = new Dictionary<MarketType, MarketTime>();
        public static Dictionary<uint, Dictionary<float, int>> MinLenDict = new Dictionary<uint, Dictionary<float, int>>();
        public static int ServerDate = 0;
        public static int ServerTime = 0;
        public static SummerTimeEventHandler SummerTimeChangeEvent;
        public static int TradeDate = 0;
        public static Dictionary<EmQComm.TypeCode, OneMarketTradeDateDataRec> TypeCodeTradeDate;

        static TimeUtilities()
        {
            try
            {
                _shMarketTime.ClearTime = 0x14c08;
                _shMarketTime.CallAuctionTime = 0x1656c;
                _shMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _shMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1b968));
                _shMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x249f0));
                _shMarketTime.KLineTime = 0x1656c;
                _shMarketTime.PushTime = 1;
                _shIndexMarketTime.ClearTime = 0x14c08;
                _shIndexMarketTime.CallAuctionTime = 0x1656c;
                _shIndexMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _shIndexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1b968));
                _shIndexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x249f0));
                _shIndexMarketTime.KLineTime = 0x16954;
                _shIndexMarketTime.PushTime = 1;
                _hkCreditIndexMarketTime.ClearTime = 0x14c08;
                _hkCreditIndexMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hkCreditIndexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d4c0));
                _hkCreditIndexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x27100));
                _hkCreditIndexMarketTime.KLineTime = 0x16954;
                _hkCreditIndexMarketTime.PushTime = 1;
                _shCreditIndexMarketTime.ClearTime = 0x14c08;
                _shCreditIndexMarketTime.CallAuctionTime = 0x1656c;
                _shCreditIndexMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _shCreditIndexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1b968));
                _shCreditIndexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x249f0));
                _shCreditIndexMarketTime.KLineTime = 0x16954;
                _shCreditIndexMarketTime.PushTime = 1;
                _hkMarketTime.ClearTime = 0x14c08;
                _hkMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hkMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d4c0));
                _hkMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x27100));
                _hkMarketTime.KLineTime = _hkMarketTime.TrendTimeList[0].Open;
                _hkHalfMarketTime.ClearTime = 0x14c08;
                _hkHalfMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hkHalfMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d8a8));
                _hkHalfMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x27100));
                _hkHalfMarketTime.KLineTime = _hkHalfMarketTime.TrendTimeList[0].Open;
                _hkWholeMarketTime.ClearTime = 0x14c08;
                _hkWholeMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hkWholeMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d4c0));
                _hkWholeMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x274e8));
                _hkWholeMarketTime.KLineTime = _hkWholeMarketTime.TrendTimeList[0].Open;
                _hsiMarketTime.ClearTime = 0x14c08;
                _hsiMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hsiMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d4c0));
                _hsiMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x27100));
                _hsiMarketTime.DelayTime = 0;
                _hsiMarketTime.KLineTime = _hsiMarketTime.TrendTimeList[0].Open;
                _hsiHalfMarketTime.ClearTime = 0x14c08;
                _hsiHalfMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hsiHalfMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d8a8));
                _hsiHalfMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x27100));
                _hsiHalfMarketTime.DelayTime = 0;
                _hsiHalfMarketTime.KLineTime = _hsiHalfMarketTime.TrendTimeList[0].Open;
                _hsiWholeMarketTime.ClearTime = 0x14c08;
                _hsiWholeMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _hsiWholeMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d4c0));
                _hsiWholeMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x274e8));
                _hsiWholeMarketTime.DelayTime = 0;
                _hsiWholeMarketTime.KLineTime = _hsiWholeMarketTime.TrendTimeList[0].Open;
                _ifMarketTime.ClearTime = 0x14c08;
                _ifMarketTime.CallAuctionTime = 0x16378;
                _ifMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _ifMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1656c, 0x1b968));
                _ifMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x24fcc));
                _ifMarketTime.KLineTime = _ifMarketTime.TrendTimeList[0].Open;
                _ifMarketTime.PushTime = 1;
                _indexFutureMarketTime.ClearTime = 0x14c08;
                _indexFutureMarketTime.CallAuctionTime = 0x16954;
                _indexFutureMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _indexFutureMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1b968));
                _indexFutureMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1fbd0, 0x249f0));
                _indexFutureMarketTime.KLineTime = _indexFutureMarketTime.TrendTimeList[0].Open;
                _indexFutureMarketTime.PushTime = 1;
                _futuresMarketTime.ClearTime = 0x14c08;
                _futuresMarketTime.TrendTimeList = new List<TrendTimeUtility>(3);
                _futuresMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x18c7c));
                _futuresMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x19258, 0x1b968));
                _futuresMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x20788, 0x249f0));
                _futuresMarketTime.KLineTime = _futuresMarketTime.TrendTimeList[0].Open;
                _czcsrMarketTime.ClearTime = 0x31510;
                _czcsrMarketTime.TrendTimeList = new List<TrendTimeUtility>(4);
                _czcsrMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x33450, 0x38e28));
                _czcsrMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x50910, 0x535fc));
                _czcsrMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x53bd8, 0x562e8));
                _czcsrMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x5b108, 0x5f370));
                _czcsrMarketTime.KLineTime = _czcsrMarketTime.TrendTimeList[0].Open;
                _shfAGMarketTIme.ClearTime = 0x31510;
                _shfAGMarketTIme.TrendTimeList = new List<TrendTimeUtility>(4);
                _shfAGMarketTIme.TrendTimeList.Add(new TrendTimeUtility(0x33450, 0x40358));
                _shfAGMarketTIme.TrendTimeList.Add(new TrendTimeUtility(0x50910, 0x535fc));
                _shfAGMarketTIme.TrendTimeList.Add(new TrendTimeUtility(0x53bd8, 0x562e8));
                _shfAGMarketTIme.TrendTimeList.Add(new TrendTimeUtility(0x5b108, 0x5f370));
                _shfAGMarketTIme.KLineTime = _shfAGMarketTIme.TrendTimeList[0].Open;
                _shfCUMarketTime.ClearTime = 0x31510;
                _shfCUMarketTime.TrendTimeList = new List<TrendTimeUtility>(4);
                _shfCUMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x33450, 0x3d090));
                _shfCUMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x50910, 0x535fc));
                _shfCUMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x53bd8, 0x562e8));
                _shfCUMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x5b108, 0x5f370));
                _shfCUMarketTime.KLineTime = _shfCUMarketTime.TrendTimeList[0].Open;
                _shfRuMarketTime.ClearTime = 0x31510;
                _shfRuMarketTime.TrendTimeList = new List<TrendTimeUtility>(4);
                _shfRuMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x33450, 0x38270));
                _shfRuMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x50910, 0x535fc));
                _shfRuMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x53bd8, 0x562e8));
                _shfRuMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x5b108, 0x5f370));
                _shfRuMarketTime.KLineTime = _shfRuMarketTime.TrendTimeList[0].Open;
                _BCEMarketTime.ClearTime = 0x2c6f0;
                _BCEMarketTime.TrendTimeList = new List<TrendTimeUtility>(3);
                _BCEMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x2e630, 0x41eb0));
                _BCEMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x50910, 0x562e8));
                _BCEMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x5b108, 0x61a80));
                _BCEMarketTime.KLineTime = _BCEMarketTime.TrendTimeList[0].Open;
                _FMEMarketTime.ClearTime = 0x2c6f0;
                _FMEMarketTime.TrendTimeList = new List<TrendTimeUtility>(3);
                _FMEMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x318f8, 0x3b538));
                _FMEMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x50910, 0x562e8));
                _FMEMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x5b108, 0x61a80));
                _FMEMarketTime.KLineTime = _FMEMarketTime.TrendTimeList[0].Open;
                _ibMarketTime.ClearTime = 0x14c08;
                _ibMarketTime.TrendTimeList = new List<TrendTimeUtility>(2);
                _ibMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x1d4c0));
                _ibMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x20788, 0x29810));
                _ibMarketTime.KLineTime = _ibMarketTime.TrendTimeList[0].Open;
                _rateSwapMarketTime.ClearTime = 0x14c08;
                _rateSwapMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _rateSwapMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x29810));
                _rateSwapMarketTime.KLineTime = _rateSwapMarketTime.TrendTimeList[0].Open;
                _usMarketTime.ClearTime = 0x320c8;
                _usMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _usMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x34008, 0x445c0));
                _usMarketTime.DelayTime = 0;
                _usMarketTime.KLineTime = _usMarketTime.TrendTimeList[0].Open;
                _usWinterMarketTime.ClearTime = 0x347d8;
                _usWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _usWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x36718, 0x46cd0));
                _usWinterMarketTime.DelayTime = 0;
                _usWinterMarketTime.KLineTime = _usWinterMarketTime.TrendTimeList[0].Open;
                _osFuturesMarketTime.ClearTime = 0xea60;
                _osFuturesMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesMarketTime.TrendTimeList.Add(new TrendTimeUtility(0xea60, 0x472ac));
                _osFuturesMarketTime.DelayTime = 0x3e8;
                _osFuturesMarketTime.KLineTime = _osFuturesMarketTime.TrendTimeList[0].Open;
                _osFutureWinterMarketTime.ClearTime = 0x11170;
                _osFutureWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFutureWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x11170, 0x499bc));
                _osFutureWinterMarketTime.DelayTime = 0x3e8;
                _osFutureWinterMarketTime.KLineTime = _osFutureWinterMarketTime.TrendTimeList[0].Open;
                _osFuturesSGXMarketTime.ClearTime = 0x126ec;
                _osFuturesSGXMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesSGXMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x126ec, 0x42a68));
                _osFuturesSGXMarketTime.DelayTime = 0x3e8;
                _osFuturesSGXMarketTime.KLineTime = _osFuturesSGXMarketTime.TrendTimeList[0].Open;
                _osFuturesCBOTMarketTime.ClearTime = 0x13880;
                _osFuturesCBOTMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesCBOTMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x13880, 0x3fd7c));
                _osFuturesCBOTMarketTime.DelayTime = 0x3e8;
                _osFuturesCBOTMarketTime.KLineTime = _osFuturesCBOTMarketTime.TrendTimeList[0].Open;
                _osFuturesCBOTWinterMarketTime.ClearTime = 0x15f90;
                _osFuturesCBOTWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesCBOTWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x4248c));
                _osFuturesCBOTWinterMarketTime.DelayTime = 0x3e8;
                _osFuturesCBOTWinterMarketTime.KLineTime = _osFuturesCBOTWinterMarketTime.TrendTimeList[0].Open;
                _osFuturesLMEElec.ClearTime = 0x13880;
                _osFuturesLMEElec.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesLMEElec.TrendTimeList.Add(new TrendTimeUtility(0x13880, 0x3f7a0));
                _osFuturesWinterLMEElec.ClearTime = 0x15f90;
                _osFuturesWinterLMEElec.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesWinterLMEElec.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x41eb0));
                _osFuturesVenue.ClearTime = 0x2d0b4;
                _osFuturesVenue.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesVenue.TrendTimeList.Add(new TrendTimeUtility(0x2d0b4, 0x3a980));
                _osFuturesWinterVenue.ClearTime = 0x2f7c4;
                _osFuturesWinterVenue.TrendTimeList = new List<TrendTimeUtility>(1);
                _osFuturesWinterVenue.TrendTimeList.Add(new TrendTimeUtility(0x2f7c4, 0x3d090));
                _forexSpotMarketTime.ClearTime = 0x11170;
                _forexSpotMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _forexSpotMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x11170, 0x38e28));
                _forexSpotMarketTime.KLineTime = _forexSpotMarketTime.TrendTimeList[0].Open;
                _forexNonSpotMarketTime.ClearTime = 0x16b48;
                _forexNonSpotMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _forexNonSpotMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x38e28));
                _forexNonSpotMarketTime.KLineTime = _forexNonSpotMarketTime.TrendTimeList[0].Open;
                _ixicMarketTime.ClearTime = 0x320c8;
                _ixicMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _ixicMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x34008, 0x445c0));
                _ixicMarketTime.KLineTime = _ixicMarketTime.TrendTimeList[0].Open;
                _ixicMarketTime.DelayTime = 0x7d0;
                _ixicWinterMarketTime.ClearTime = 0x347d8;
                _ixicWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _ixicWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x36718, 0x46cd0));
                _ixicWinterMarketTime.KLineTime = _ixicWinterMarketTime.TrendTimeList[0].Open;
                _ixicWinterMarketTime.DelayTime = 0x7d0;
                _jkMarketTime.ClearTime = 0x1b968;
                _jkMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _jkMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1b968, 0x1fbd0));
                _jkMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x22e98, 0x2cad8));
                _jkMarketTime.KLineTime = _jkMarketTime.TrendTimeList[0].Open;
                _jkMarketTime.DelayTime = 0x7d0;
                _malaysiaMarketTime.ClearTime = 0x15f90;
                _malaysiaMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _malaysiaMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x1e078));
                _malaysiaMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x22e98, 0x29810));
                _malaysiaMarketTime.KLineTime = _malaysiaMarketTime.TrendTimeList[0].Open;
                _malaysiaMarketTime.DelayTime = 0x7d0;
                _koreaMarketTime.ClearTime = 0x13880;
                _koreaMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _koreaMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x13880, 0x222e0));
                _koreaMarketTime.KLineTime = _koreaMarketTime.TrendTimeList[0].Open;
                _koreaMarketTime.DelayTime = 0x7d0;
                _nikkeiMarketTime.ClearTime = 0x11940;
                _nikkeiMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _nikkeiMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x13880, 0x19258));
                _nikkeiMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1b968, 0x222e0));
                _nikkeiMarketTime.KLineTime = _nikkeiMarketTime.TrendTimeList[0].Open;
                _nikkeiMarketTime.DelayTime = 0x9c4;
                _philippinesMarketTime.ClearTime = 0x16b48;
                _philippinesMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _philippinesMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x16b48, 0x1d8a8));
                _philippinesMarketTime.KLineTime = _philippinesMarketTime.TrendTimeList[0].Open;
                _philippinesMarketTime.DelayTime = 0x7d0;
                _sensexMarketTime.ClearTime = 0x1b968;
                _sensexMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _sensexMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x1b968, 0x2cad8));
                _sensexMarketTime.KLineTime = _sensexMarketTime.TrendTimeList[0].Open;
                _sensexMarketTime.DelayTime = 0x7d0;
                _singaporeMarketTime.ClearTime = 0x15f90;
                _singaporeMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _singaporeMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x29810));
                _singaporeMarketTime.KLineTime = _singaporeMarketTime.TrendTimeList[0].Open;
                _singaporeMarketTime.DelayTime = 0x7d0;
                _taiwanMarketTime.ClearTime = 0x15f90;
                _taiwanMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _taiwanMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x15f90, 0x20788));
                _taiwanMarketTime.KLineTime = _taiwanMarketTime.TrendTimeList[0].Open;
                _taiwanMarketTime.DelayTime = 0x7d0;
                _newZealandMarketTime.ClearTime = 0xc350;
                _newZealandMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _newZealandMarketTime.TrendTimeList.Add(new TrendTimeUtility(0xc350, 0x1bf44));
                _newZealandMarketTime.KLineTime = _taiwanMarketTime.TrendTimeList[0].Open;
                _newZealandMarketTime.DelayTime = 0x7d0;
                _newZealandWinterMarketTime.ClearTime = 0xea60;
                _newZealandWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _newZealandWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0xea60, 0x1e654));
                _newZealandWinterMarketTime.KLineTime = _newZealandWinterMarketTime.TrendTimeList[0].Open;
                _newZealandWinterMarketTime.DelayTime = 0x7d0;
                _dutchAEXMarketTime.ClearTime = 0x249f0;
                _dutchAEXMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _dutchAEXMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x249f0, 0x38e28));
                _dutchAEXMarketTime.KLineTime = _dutchAEXMarketTime.TrendTimeList[0].Open;
                _dutchAEXMarketTime.DelayTime = 0x7d0;
                _dutchAEXWinterMarketTime.ClearTime = 0x27100;
                _dutchAEXWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _dutchAEXWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x27100, 0x3b538));
                _dutchAEXWinterMarketTime.KLineTime = _dutchAEXWinterMarketTime.TrendTimeList[0].Open;
                _dutchAEXWinterMarketTime.DelayTime = 0x7d0;
                _austriaMarketTime.ClearTime = 0x22ab0;
                _austriaMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _austriaMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x249f0, 0x38e28));
                _austriaMarketTime.KLineTime = _austriaMarketTime.TrendTimeList[0].Open;
                _austriaMarketTime.DelayTime = 0x7d0;
                _austriaWinterMarketTime.ClearTime = 0x251c0;
                _austriaWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _austriaWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x27100, 0x3b538));
                _austriaWinterMarketTime.KLineTime = _austriaWinterMarketTime.TrendTimeList[0].Open;
                _austriaWinterMarketTime.DelayTime = 0x7d0;
                _norwayMarketTime.ClearTime = 0x249f0;
                _norwayMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _norwayMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x249f0, 0x36524));
                _norwayMarketTime.KLineTime = _norwayMarketTime.TrendTimeList[0].Open;
                _norwayMarketTime.DelayTime = 0x7d0;
                _norwayWinterMarketTime.ClearTime = 0x27100;
                _norwayWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _norwayWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x27100, 0x38c34));
                _norwayWinterMarketTime.KLineTime = _norwayWinterMarketTime.TrendTimeList[0].Open;
                _norwayWinterMarketTime.DelayTime = 0x7d0;
                _russiaMarketTime.ClearTime = 0x222e0;
                _russiaMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _russiaMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x222e0, 0x36ee8));
                _russiaMarketTime.KLineTime = _russiaMarketTime.TrendTimeList[0].Open;
                _russiaMarketTime.DelayTime = 0x7d0;
                _ForexInterMarketTime.ClearTime = 0xc350;
                _ForexInterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _ForexInterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0xc351, 0x46cd0));
                _ForexInterMarketTime.KLineTime = _ForexInterMarketTime.TrendTimeList[0].Open;
                _ForexInterMarketSummerTime.ClearTime = 0xc350;
                _ForexInterMarketSummerTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _ForexInterMarketSummerTime.TrendTimeList.Add(new TrendTimeUtility(0xc351, 0x46cd0));
                _ForexInterMarketSummerTime.KLineTime = _ForexInterMarketSummerTime.TrendTimeList[0].Open;
                _ForexInterMarketWinterTime.ClearTime = 0xc350;
                _ForexInterMarketWinterTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _ForexInterMarketWinterTime.TrendTimeList.Add(new TrendTimeUtility(0xea61, 0x493e0));
                _ForexInterMarketWinterTime.KLineTime = _ForexInterMarketWinterTime.TrendTimeList[0].Open;
                _axatMarketTime.ClearTime = 0xf618;
                _axatMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _axatMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x11170, 0x1fbd0));
                _axatMarketTime.KLineTime = _axatMarketTime.TrendTimeList[0].Open;
                _axatMarketTime.DelayTime = 0x9c4;
                _axatWinterMarketTime.ClearTime = 0x11d28;
                _axatWinterMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _axatWinterMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x13880, 0x222e0));
                _axatWinterMarketTime.KLineTime = _axatWinterMarketTime.TrendTimeList[0].Open;
                _axatWinterMarketTime.DelayTime = 0x9c4;
                _crbMarketTime.ClearTime = 0x2f1e8;
                _crbMarketTime.TrendTimeList = new List<TrendTimeUtility>(1);
                _crbMarketTime.TrendTimeList.Add(new TrendTimeUtility(0x30d40, 0x41eb0));
                _crbMarketTime.KLineTime = _crbMarketTime.TrendTimeList[0].Open;
                _crbMarketTime.DelayTime = 0x9c4;
                MarketStatus.Add(MarketType.SHALev2, _shMarketTime);
                MarketStatus.Add(MarketType.SHALev1, _shMarketTime);
                MarketStatus.Add(MarketType.SHINDEX, _shIndexMarketTime);
                MarketStatus.Add(MarketType.SZINDEX, _shIndexMarketTime);
                MarketStatus.Add(MarketType.CircuitBreakerIndex, _shIndexMarketTime);
                MarketStatus.Add(MarketType.StockOption, _shIndexMarketTime);
                MarketStatus.Add(MarketType.StockOptionSell, _shIndexMarketTime);
                MarketStatus.Add(MarketType.EMINDEX, _shIndexMarketTime);
                MarketStatus.Add(MarketType.SHHKCreditIndex, _shCreditIndexMarketTime);
                MarketStatus.Add(MarketType.HKCreditIndex, _hkCreditIndexMarketTime);
                MarketStatus.Add(MarketType.Portfolios, _shIndexMarketTime);
                MarketStatus.Add(MarketType.PortfoliosAccount, _shIndexMarketTime);
                MarketStatus.Add(MarketType.HSIA, _hkMarketTime);
                MarketStatus.Add(MarketType.CSINDEX, _shIndexMarketTime);
                MarketStatus.Add(MarketType.CSIINDEX, _shIndexMarketTime);
                MarketStatus.Add(MarketType.CNINDEX, _shIndexMarketTime);
                MarketStatus.Add(MarketType.HK, _hkMarketTime);
                MarketStatus.Add(MarketType.SHHK, _hkMarketTime);
                MarketStatus.Add(MarketType.HKwarrentBuy, _hkMarketTime);
                MarketStatus.Add(MarketType.HKwarrentSell, _hkMarketTime);
                MarketStatus.Add(MarketType.CallableContractsBuy, _hkMarketTime);
                MarketStatus.Add(MarketType.CallableContractsSell, _hkMarketTime);
                MarketStatus.Add(MarketType.IF, _indexFutureMarketTime);
                MarketStatus.Add(MarketType.FuturesOption, _ifMarketTime);
                MarketStatus.Add(MarketType.FuturesOptionSell, _ifMarketTime);
                MarketStatus.Add(MarketType.GoverFutures, _ifMarketTime);
                MarketStatus.Add(MarketType.DCE, _futuresMarketTime);
                MarketStatus.Add(MarketType.CommodityOptionBuy, _futuresMarketTime);
                MarketStatus.Add(MarketType.CommodityOptionSell, _futuresMarketTime);
                MarketStatus.Add(MarketType.CHFAG, _shfAGMarketTIme);
                MarketStatus.Add(MarketType.CHFCU, _shfCUMarketTime);
                MarketStatus.Add(MarketType.SHFRU, _shfRuMarketTime);
                MarketStatus.Add(MarketType.BCE, _BCEMarketTime);
                MarketStatus.Add(MarketType.FME, _FMEMarketTime);
                MarketStatus.Add(MarketType.SHF, _futuresMarketTime);
                MarketStatus.Add(MarketType.CZC, _futuresMarketTime);
                MarketStatus.Add(MarketType.CZCSR, _czcsrMarketTime);
                MarketStatus.Add(MarketType.DZDCE, _czcsrMarketTime);
                MarketStatus.Add(MarketType.CZCFe, _czcsrMarketTime);
                MarketStatus.Add(MarketType.IB, _ibMarketTime);
                MarketStatus.Add(MarketType.BC, _ibMarketTime);
                MarketStatus.Add(MarketType.InterBankRepurchase, _ibMarketTime);
                MarketStatus.Add(MarketType.Chibor, _ibMarketTime);
                MarketStatus.Add(MarketType.OSFuturesSGX, _osFuturesSGXMarketTime);
                MarketStatus.Add(MarketType.FTSEA50, _osFuturesSGXMarketTime);
                MarketStatus.Add(MarketType.ForexSpot, _forexSpotMarketTime);
                MarketStatus.Add(MarketType.ForexNonSpot, _forexNonSpotMarketTime);
                MarketStatus.Add(MarketType.ForexInter, _ForexInterMarketTime);
                MarketStatus.Add(MarketType.ForexInterWinterSummer, _ForexInterMarketSummerTime);
                MarketStatus.Add(MarketType.JKINDEX, _jkMarketTime);
                MarketStatus.Add(MarketType.MalaysiaIndex, _malaysiaMarketTime);
                MarketStatus.Add(MarketType.KoreaIndex, _koreaMarketTime);
                MarketStatus.Add(MarketType.NikkeiIndex, _nikkeiMarketTime);
                MarketStatus.Add(MarketType.PhilippinesIndex, _philippinesMarketTime);
                MarketStatus.Add(MarketType.SensexIndex, _sensexMarketTime);
                MarketStatus.Add(MarketType.SingaporeIndex, _singaporeMarketTime);
                MarketStatus.Add(MarketType.TaiwanIndex, _taiwanMarketTime);
                MarketStatus.Add(MarketType.RussiaIndex, _russiaMarketTime);
                MarketStatus.Add(MarketType.RateSwap, _rateSwapMarketTime);
                MarketStatus.Add(MarketType.US, _usMarketTime);
                MarketStatus.Add(MarketType.NasdaqIndex, _ixicMarketTime);
                MarketStatus.Add(MarketType.OSFutures, _osFuturesMarketTime);
                MarketStatus.Add(MarketType.OSFuturesCBOT, _osFuturesCBOTMarketTime);
                MarketStatus.Add(MarketType.OSFuturesLMEElec, _osFuturesLMEElec);
                MarketStatus.Add(MarketType.OSFuturesLMEVenue, _osFuturesVenue);
                MarketStatus.Add(MarketType.DutchAEXIndex, _dutchAEXMarketTime);
                MarketStatus.Add(MarketType.AustriaIndex, _austriaMarketTime);
                MarketStatus.Add(MarketType.NorwayIndex, _norwayMarketTime);
                MarketStatus.Add(MarketType.NewZealandIndex, _newZealandMarketTime);
                MarketStatus.Add(MarketType.HSINDEX, _hsiMarketTime);
                MarketStatus.Add(MarketType.CRB, _crbMarketTime);
                MarketStatus.Add(MarketType.AXAT, _axatMarketTime);
                MarketStatus.Add(MarketType.SHSZINDEXLev2, _hsiHalfMarketTime);
                MarketStatus.Add(MarketType.HKOptionLev2, _hsiWholeMarketTime);
                MarketStatus.Add(MarketType.HKLev2, _hkHalfMarketTime);
                MarketStatus.Add(MarketType.HSIforMTime, _hkWholeMarketTime);
                TypeCodeTradeDate = new Dictionary<EmQComm.TypeCode, OneMarketTradeDateDataRec>();
            }
            catch (Exception exception)
            {
                LogUtilities.LogMessage(exception.Message);
            }
        }

        public static short GetCallAuctionPointFromTime(MarketType marketType)
        {
            bool flag;
            byte num;
            if (marketType == MarketType.NA)
            {
                return 0;
            }
            return GetCallAuctionPointFromTime(GetMarketTime(marketType).LastTradeTime, marketType, out flag, out num);
        }

        public static short GetCallAuctionPointFromTime(int code)
        {
            if (code == 0)
            {
                return 0;
            }
            return GetCallAuctionPointFromTime(GetMarketTime(DllImportHelper.GetMarketType(code)).LastTradeTime, code);
        }

        public static short GetCallAuctionPointFromTime(int time, int code)
        {
            bool flag;
            byte num;
            MarketType marketType = DllImportHelper.GetMarketType(code);
            short num2 = GetCallAuctionPointFromTime(time, marketType, out flag, out num);
            if (flag)
            {
                return (short)(num2 - 1);
            }
            return num2;
        }

        public static short GetCallAuctionPointFromTime(int time, MarketType marketType, out bool inTradeTime, out byte isClose)
        {
            isClose = 0;
            inTradeTime = false;
            MarketTime marketTime = GetMarketTime(marketType);
            if (marketTime.CallAuctionTime == 0)
            {
                return 0;
            }
            int num = time / 0x2710;
            int num2 = (time % 0x2710) / 100;
            short num3 = 0;
            int num4 = marketTime.CallAuctionTime / 0x2710;
            int num5 = (marketTime.CallAuctionTime % 0x2710) / 100;
            int num6 = marketTime.TrendTimeList[0].Open / 0x2710;
            int num7 = (marketTime.TrendTimeList[0].Open % 0x2710) / 100;
            if (time < marketTime.CallAuctionTime)
            {
                return 0;
            }
            if (time >= marketTime.TrendTimeList[0].Open)
            {
                return (short)(((((num6 - num4) * 60) + num7) - num5) - 1);
            }
            num3 = (short)(((((num - num4) * 60) + num2) - num5) + 1);
            if (num3 <= GetCallAuctionTotalPoint(marketType))
            {
                inTradeTime = true;
            }
            isClose = (time == (marketTime.TrendTimeList[0].Open - 100)) ? ((byte)1) : ((byte)0);
            return num3;
        }

        public static int GetCallAuctionTimeFromPoint(MarketType marketType, int point)
        {
            MarketTime marketTime = GetMarketTime(marketType);
            int num = marketTime.TrendTimeList[0].Open / 0x2710;
            int num2 = (marketTime.TrendTimeList[0].Open % 0x2710) / 100;
            int num3 = marketTime.CallAuctionTime / 0x2710;
            int num4 = (marketTime.CallAuctionTime % 0x2710) / 100;
            short num5 = (short)((((num - num3) * 60) + num2) - num4);
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            if (point <= num5)
            {
                num6 = point / 60;
                num7 = point % 60;
                int num9 = num7 + num4;
                int num10 = (num3 + num6) + (num9 / 60);
                num9 = num9 % 60;
                num8 = (num10 * 0x2710) + (num9 * 100);
            }
            return num8;
        }

        public static int GetCallAuctionTimeFromPoint(int code, int point)
        {
            MarketTime marketTime = GetMarketTime( DllImportHelper.GetMarketType(code));
            int num = marketTime.TrendTimeList[0].Open / 0x2710;
            int num2 = (marketTime.TrendTimeList[0].Open % 0x2710) / 100;
            int num3 = marketTime.CallAuctionTime / 0x2710;
            int num4 = (marketTime.CallAuctionTime % 0x2710) / 100;
            short num5 = (short)((((num - num3) * 60) + num2) - num4);
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            if (point <= num5)
            {
                num6 = point / 60;
                num7 = point % 60;
                int num9 = num7 + num4;
                int num10 = (num3 + num6) + (num9 / 60);
                num9 = num9 % 60;
                num8 = (num10 * 0x2710) + (num9 * 100);
            }
            return num8;
        }

        public static int GetCallAuctionTotalPoint(MarketType market)
        {
            int num = 0;
            MarketTime marketTime = GetMarketTime(market);
            if ((marketTime.CallAuctionTime != 0) && (marketTime.TrendTimeList.Count > 0))
            {
                num = (marketTime.TrendTimeList[0].Open - marketTime.CallAuctionTime) / 100;
            }
            return num;
        }

        public static List<OneMarketTradeDateDataRec.TradeDateStruct> GetHkClosingAuctionDict(MarketType market)
        {
            if (TypeCodeTradeDate != null)
            {
                OneMarketTradeDateDataRec rec = null;
                switch (market)
                {
                    case MarketType.HKwarrentBuy:
                    case MarketType.HKwarrentSell:
                    case MarketType.CallableContractsBuy:
                    case MarketType.CallableContractsSell:
                    case MarketType.HSINDEX:
                    case MarketType.HK:
                    case MarketType.SHHK:
                        TypeCodeTradeDate.TryGetValue(EmQComm.TypeCode.TC_HK, out rec);
                        if (rec != null)
                        {
                            return rec.TradeDateDict;
                        }
                        break;
                }
            }
            return null;
        }

        public static void GetIntTimeFromDateTime(DateTime dateTime, out int date, out int time)
        {
            date = ((dateTime.Year * 0x2710) + (dateTime.Month * 100)) + dateTime.Day;
            time = ((dateTime.Hour * 0x2710) + (dateTime.Minute * 100)) + dateTime.Second;
        }

        public static int GetKLineLastDate(int date, KLineCycle cycle)
        {
            int num = date;
            if (num == 0)
            {
                return 0;
            }
            DateTime time = new DateTime(num / 0x2710, (num % 0x2710) / 100, (num % 0x2710) % 100);
            switch (cycle)
            {
                case KLineCycle.CycleWeek:
                    {
                        DateTime time2 = time.AddDays(Convert.ToDouble((int)(1 - Convert.ToInt16(time.DayOfWeek))));
                        return (((time2.Year * 0x2710) + (time2.Month * 100)) + time2.Day);
                    }
                case KLineCycle.CycleMonth:
                    return (((date / 100) * 100) + 1);

                case KLineCycle.CycleSeason:
                    {
                        DateTime time3 = DateTime.Now.AddMonths(-((DateTime.Now.Month - 1) % 3));
                        return (((time3.Year * 0x2710) + (time3.Month * 100)) + 1);
                    }
                case KLineCycle.CycleYear:
                    return ((time.Year * 0x2710) + 0x65);
            }
            return num;
        }

        public static byte GetLastHkAuctionType(MarketType market)
        {
            List<OneMarketTradeDateDataRec.TradeDateStruct> hkClosingAuctionDict = GetHkClosingAuctionDict(market);
            if ((hkClosingAuctionDict != null) && (hkClosingAuctionDict.Count > 0))
            {
                return hkClosingAuctionDict[0].Type;
            }
            return 0;
        }

        public static int GetLastTradeDateInt()
        {
            DateTime now;
            if (((DateTime.Now.Hour * 100) + DateTime.Now.Minute) <= 900)
            {
                now = DateTime.Today.AddDays(-1.0);
            }
            else
            {
                now = DateTime.Now;
            }
            if (now.DayOfWeek == DayOfWeek.Saturday)
            {
                now = now.AddDays(-1.0);
            }
            else if (now.DayOfWeek == DayOfWeek.Sunday)
            {
                now = now.AddDays(-2.0);
            }
            return (((now.Year * 0x2710) + (now.Month * 100)) + now.Day);
        }

        public static MarketTime GetMarketTime(MarketType market)
        {
            if (!MarketStatus.ContainsKey(market))
            {
                return MarketStatus[MarketType.SHALev1];
            }
            return MarketStatus[market];
        }

        public static MarketTime GetMarketTime(int code)
        {
            MarketType mt = MarketType.SHALev1;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;
            return GetMarketTime(mt);
        }

        public static int GetMintKLinePointOneDay(MarketType marketType, KLineCycle cycle)
        {
            if (marketType == MarketType.NA)
            {
                return 0;
            }
            int num = GetTrendTotalPoint(marketType) - 1;
            int num2 = 0;
            switch (cycle)
            {
                case KLineCycle.CycleMint1:
                    break;

                case KLineCycle.CycleMint5:
                    num2 = num % 5;
                    num /= 5;
                    break;

                case KLineCycle.CycleMint15:
                    num2 = num % 15;
                    num /= 15;
                    break;

                case KLineCycle.CycleMint30:
                    num2 = num % 30;
                    num /= 30;
                    break;

                case KLineCycle.CycleMint60:
                    num2 = num % 60;
                    num /= 60;
                    break;

                case KLineCycle.CycleMint120:
                    num2 = num % 120;
                    num /= 120;
                    break;

                default:
                    num = 1;
                    break;
            }
            if (num2 != 0)
            {
                num++;
            }
            return num;
        }

        public static int GetMintKLinePointOneDay(int code, KLineCycle cycle)
        {
            if (code == 0)
            {
                return 0;
            }
            int num = GetTrendTotalPoint(DllImportHelper.GetMarketType(code) - 1);
            int num2 = 0;
            switch (cycle)
            {
                case KLineCycle.CycleMint1:
                    break;

                case KLineCycle.CycleMint5:
                    num2 = num % 5;
                    num /= 5;
                    break;

                case KLineCycle.CycleMint15:
                    num2 = num % 15;
                    num /= 15;
                    break;

                case KLineCycle.CycleMint30:
                    num2 = num % 30;
                    num /= 30;
                    break;

                case KLineCycle.CycleMint60:
                    num2 = num % 60;
                    num /= 60;
                    break;

                case KLineCycle.CycleMint120:
                    num2 = num % 120;
                    num /= 120;
                    break;

                default:
                    num = 1;
                    break;
            }
            if (num2 != 0)
            {
                num++;
            }
            return num;
        }

        public static int GetMintKLineTimeFromPoint(MarketType marketType, int point, KLineCycle cycle)
        {
            int num;
            switch (cycle)
            {
                case KLineCycle.CycleMint1:
                    num = 1;
                    break;

                case KLineCycle.CycleMint5:
                    num = 5;
                    break;

                case KLineCycle.CycleMint15:
                    num = 15;
                    break;

                case KLineCycle.CycleMint30:
                    num = 30;
                    break;

                case KLineCycle.CycleMint60:
                    num = 60;
                    break;

                case KLineCycle.CycleMint120:
                    num = 120;
                    break;

                default:
                    return 0;
            }
            point = (point + 1) * num;
            MarketTime marketTime = GetMarketTime(marketType);
            int trendTotalPoint = GetTrendTotalPoint(marketType);
            if (point > (trendTotalPoint - marketTime.TrendTimeList.Count))
            {
                point = (trendTotalPoint - marketTime.TrendTimeList.Count) + 1;
            }
            List<short> list = new List<short>();
            lock (list)
            {
                if (list == null)
                {
                    return 0;
                }
                list.Clear();
                foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
                {
                    int num4 = utility.Open / 0x2710;
                    int num5 = (utility.Open % 0x2710) / 100;
                    int num6 = utility.Close / 0x2710;
                    int num7 = (utility.Close % 0x2710) / 100;
                    short item = (short)((((num6 - num4) * 60) + num7) - num5);
                    list.Add(item);
                }
                int num9 = 0;
                int num10 = 0;
                short num11 = 0;
                short num12 = 0;
                for (short i = 1; i <= marketTime.TrendTimeList.Count; i = (short)(i + 1))
                {
                    if (((i - 1) < list.Count) && (i >= 1))
                    {
                        num11 += list[i - 1];
                    }
                    if (point <= num11)
                    {
                        num9 = (point - num12) / 60;
                        num10 = (point - num12) % 60;
                        int num14 = num10 + ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100);
                        int num15 = ((marketTime.TrendTimeList[i - 1].Open / 0x2710) + num9) + (num14 / 60);
                        num14 = num14 % 60;
                        return ((num15 * 0x2710) + (num14 * 100));
                    }
                    try
                    {
                        if (((i - 1) < list.Count) && (i >= 1))
                        {
                            num12 += list[i - 1];
                        }
                    }
                    catch (Exception exception)
                    {
                        LogUtilities.LogMessage("xwf Time " + exception.StackTrace);
                        throw;
                    }
                }
            }
            return 0;
        }

        public static int GetMintKLineTimeFromPoint(int code, int point, KLineCycle cycle)
        {
            return GetMintKLineTimeFromPoint(DllImportHelper.GetMarketType(code), point, cycle);
        }

        public static int GetMintKLineTimeFromPointOld(int code, int point, KLineCycle cycle)
        {
            return 0;
        }

        public static int GetPCTime()
        {
            int num = 0;
            try
            {
                DateTime now = DateTime.Now;
                num = ((now.Hour * 0x2710) + (now.Minute * 100)) + now.Second;
            }
            catch (Exception exception)
            {
                //LogUtilities.LogMessagePublishInfo(exception.StackTrace);
            }
            return num;
        }

        public static short GetPointFromTime(int code)
        {
            if (code == 0)
            {
                return 0;
            }
            return GetPointFromTime(GetMarketTime(code).LastTradeTime, code);
        }

        public static short GetPointFromTime(int time, int code)
        {
            bool flag;
            byte num;
            return GetPointFromTime(time, code, out flag, out num);
        }

        public static short GetPointFromTime(int time, MarketType market, out bool inTradeTime, out byte isClose)
        {
            try
            {
                MarketTime marketTime = GetMarketTime(market);
                List<short> list = new List<short>();
                int num = time / 0x2710;
                int num2 = (time % 0x2710) / 100;
                lock (list)
                {
                    list.Clear();
                    short num3 = 0;
                    foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
                    {
                        int num4 = utility.Open / 0x2710;
                        int num5 = (utility.Open % 0x2710) / 100;
                        int num6 = utility.Close / 0x2710;
                        int num7 = (utility.Close % 0x2710) / 100;
                        short item = (short)((((num6 - num4) * 60) + num7) - num5);
                        num3 = (short)(num3 + item);
                        list.Add(item);
                    }
                    if (time < marketTime.ClearTime)
                    {
                        inTradeTime = false;
                        isClose = 0;
                        return 0;
                    }
                    if (time >= marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close)
                    {
                        _oldPoint = num3;
                        inTradeTime = time == marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close;
                        isClose = (time == marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close) ? ((byte)1) : ((byte)0);
                        return _oldPoint;
                    }
                    inTradeTime = false;
                    for (short i = 1; i <= marketTime.TrendTimeList.Count; i = (short)(i + 1))
                    {
                        short num10 = 0;
                        short num11 = list[0];
                        for (short j = 1; j < i; j = (short)(j + 1))
                        {
                            if (((j - 1) >= 0) && ((j - 1) < list.Count))
                            {
                                num10 += list[j - 1];
                            }
                            if (j == (i - 1))
                            {
                                num11 = (short)(num10 + list[i - 1]);
                            }
                        }
                        if (time < marketTime.TrendTimeList[i - 1].Open)
                        {
                            if ((i - 1) == 0)
                            {
                                _oldPoint = 0;
                            }
                            else
                            {
                                _oldPoint = num10;
                            }
                            if ((i - 1) > 0)
                            {
                                inTradeTime = true;
                            }
                            isClose = 1;
                            return _oldPoint;
                        }
                        if (time <= marketTime.TrendTimeList[i - 1].Close)
                        {
                            _oldPoint = (short)(((num10 + ((num - (marketTime.TrendTimeList[i - 1].Open / 0x2710)) * 60)) + (num2 - ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100))) + 1);
                            if (_oldPoint > num11)
                            {
                                _oldPoint = num11;
                            }
                            inTradeTime = true;
                            isClose = (time == marketTime.TrendTimeList[i - 1].Close) ? ((byte)1) : ((byte)0);
                            return _oldPoint;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogUtilities.LogMessage("GetPointFromTime error" + exception.Message);
            }
            inTradeTime = false;
            isClose = 0;
            return 0;
        }

        public static short GetPointFromTime(int time, int code, out bool inTradeTime, out byte isClose)
        {
            try
            {
                MarketTime marketTime = GetMarketTime(code);
                List<short> list = new List<short>();
                int num = time / 0x2710;
                int num2 = (time % 0x2710) / 100;
                lock (list)
                {
                    list.Clear();
                    short num3 = 0;
                    foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
                    {
                        int num4 = utility.Open / 0x2710;
                        int num5 = (utility.Open % 0x2710) / 100;
                        int num6 = utility.Close / 0x2710;
                        int num7 = (utility.Close % 0x2710) / 100;
                        short item = (short)((((num6 - num4) * 60) + num7) - num5);
                        num3 = (short)(num3 + item);
                        list.Add(item);
                    }
                    if (time < marketTime.ClearTime)
                    {
                        inTradeTime = false;
                        isClose = 0;
                        return 0;
                    }
                    if (time >= marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close)
                    {
                        _oldPoint = num3;
                        inTradeTime = time == marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close;
                        isClose = (time == marketTime.TrendTimeList[marketTime.TrendTimeList.Count - 1].Close) ? ((byte)1) : ((byte)0);
                        return _oldPoint;
                    }
                    inTradeTime = false;
                    for (short i = 1; i <= marketTime.TrendTimeList.Count; i = (short)(i + 1))
                    {
                        short num10 = 0;
                        short num11 = list[0];
                        for (short j = 1; j < i; j = (short)(j + 1))
                        {
                            if (((j - 1) >= 0) && ((j - 1) < list.Count))
                            {
                                num10 += list[j - 1];
                            }
                            if (j == (i - 1))
                            {
                                num11 = (short)(num10 + list[i - 1]);
                            }
                        }
                        if (time < marketTime.TrendTimeList[i - 1].Open)
                        {
                            if ((i - 1) == 0)
                            {
                                _oldPoint = 0;
                            }
                            else
                            {
                                _oldPoint = num10;
                            }
                            if ((i - 1) > 0)
                            {
                                inTradeTime = true;
                            }
                            isClose = 1;
                            return _oldPoint;
                        }
                        if (time <= marketTime.TrendTimeList[i - 1].Close)
                        {
                            _oldPoint = (short)(((num10 + ((num - (marketTime.TrendTimeList[i - 1].Open / 0x2710)) * 60)) + (num2 - ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100))) + 1);
                            if (_oldPoint > num11)
                            {
                                _oldPoint = num11;
                            }
                            inTradeTime = true;
                            isClose = (time == marketTime.TrendTimeList[i - 1].Close) ? ((byte)1) : ((byte)0);
                            return _oldPoint;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogUtilities.LogMessage("GetPointFromTime error" + exception.Message);
            }
            inTradeTime = false;
            isClose = 0;
            return 0;
        }

        public static short GetPointFromTimeForTrend(int time, MarketType market)
        {
            bool flag;
            return GetPointFromTimeForTrend(time, market, out flag);
        }

        public static short GetPointFromTimeForTrend(int time, int code)
        {
            bool flag;
            byte num;
            if (code == 0)
            {
                return 0;
            }
            short num2 = GetPointFromTime(time, code, out flag, out num);
            if ((num != 1) && (flag && (num2 > 0)))
            {
                return (short)(num2 - 1);
            }
            return num2;
        }

        public static short GetPointFromTimeForTrend(int time, MarketType market, out bool inTradeTime)
        {
            byte num;
            short num2 = GetPointFromTime(time, market, out inTradeTime, out num);
            if ((num != 1) && (inTradeTime && (num2 > 0)))
            {
                return (short)(num2 - 1);
            }
            return num2;
        }

        public static short GetPointFromTimeOld(int time, int code)
        {
            return 0;
        }

        public static DateTime GetTimeFromInt(int date)
        {
            try
            {
                int year = date / 0x2710;
                int month = (date - (year * 0x2710)) / 100;
                return new DateTime(year, month, (date - (year * 0x2710)) - (month * 100));
            }
            catch (Exception)
            {
            }
            return DateTime.Today;
        }

        public static DateTime GetTimeFromInt(int date, int time)
        {
            if ((date == 0) && (time == 0))
            {
                return DateTime.MinValue;
            }
            int year = date / 0x2710;
            int month = (date - (year * 0x2710)) / 100;
            int day = (date - (year * 0x2710)) - (month * 100);
            int hour = time / 0x2710;
            int minute = (time - (0x2710 * hour)) / 100;
            int second = (time - (0x2710 * hour)) - (100 * minute);
            DateTime minValue = DateTime.MinValue;
            try
            {
                minValue = new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception exception)
            {
                //LogUtilities.LogMessagePublishException(exception.StackTrace);
            }
            return minValue;
        }

        public static int GetTimeFromPoint(MarketType marketType, int point)
        {
            MarketTime marketTime = GetMarketTime(marketType);
            bool flag = SecurityAttribute.CFTMarketTypes.Contains(marketType);
            List<short> list = new List<short>();
            lock (list)
            {
                try
                {
                    list.Clear();
                    foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
                    {
                        int num2 = utility.Open / 0x2710;
                        int num3 = (utility.Open % 0x2710) / 100;
                        int num4 = utility.Close / 0x2710;
                        int num5 = (utility.Close % 0x2710) / 100;
                        short item = (short)((((num4 - num2) * 60) + num5) - num3);
                        list.Add(item);
                    }
                }
                catch (Exception)
                {
                    LogUtilities.LogMessage("xwf GetTimeFromPoint");
                }
                int num7 = 0;
                int num8 = 0;
                short num9 = 0;
                short num10 = 0;
                for (short i = 1; i <= marketTime.TrendTimeList.Count; i = (short)(i + 1))
                {
                    if (((i - 1) >= 0) && ((i - 1) < list.Count))
                    {
                        try
                        {
                            if (((i - 1) < list.Count) && (i >= 1))
                            {
                                num9 += list[i - 1];
                            }
                        }
                        catch (Exception exception)
                        {
                            LogUtilities.LogMessage(exception.StackTrace);
                        }
                        int num12 = num9;
                        if (flag)
                        {
                            num12 = (num9 + i) - 1;
                            if (point <= num12)
                            {
                                num7 = (((point - num10) - i) + 1) / 60;
                                num8 = (((point - num10) - i) + 1) % 60;
                                int num13 = num8 + ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100);
                                int num14 = ((marketTime.TrendTimeList[i - 1].Open / 0x2710) + num7) + (num13 / 60);
                                num13 = num13 % 60;
                                return ((num14 * 0x2710) + (num13 * 100));
                            }
                        }
                        else if (point <= num12)
                        {
                            num7 = (point - num10) / 60;
                            num8 = (point - num10) % 60;
                            int num15 = num8 + ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100);
                            int num16 = ((marketTime.TrendTimeList[i - 1].Open / 0x2710) + num7) + (num15 / 60);
                            num15 = num15 % 60;
                            return ((num16 * 0x2710) + (num15 * 100));
                        }
                        try
                        {
                            if (((i - 1) < list.Count) && (i >= 1))
                            {
                                num10 += list[i - 1];
                            }
                        }
                        catch (Exception exception2)
                        {
                            LogUtilities.LogMessage(exception2.StackTrace);
                        }
                    }
                }
            }
            return 0;
        }

        public static int GetTimeFromPoint(int code, int point)
        {
            MarketType marketType = DllImportHelper.GetMarketType(code);
            MarketTime marketTime = GetMarketTime(code);
            bool flag = SecurityAttribute.CFTMarketTypes.Contains(marketType);
            List<short> list = new List<short>();
            foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
            {
                int num = utility.Open / 0x2710;
                int num2 = (utility.Open % 0x2710) / 100;
                int num3 = utility.Close / 0x2710;
                int num4 = (utility.Close % 0x2710) / 100;
                short item = (short)((((num3 - num) * 60) + num4) - num2);
                list.Add(item);
            }
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            lock (list)
            {
                short num9 = 0;
                short num10 = 0;
                for (short i = 1; i <= marketTime.TrendTimeList.Count; i = (short)(i + 1))
                {
                    if (((i - 1) >= 0) && ((i - 1) < list.Count))
                    {
                        try
                        {
                            if (((i - 1) < list.Count) && (i >= 1))
                            {
                                num9 += list[i - 1];
                            }
                            int num12 = num9;
                            if (flag)
                            {
                                num12 = (num9 + i) - 1;
                                if (point > num12)
                                {
                                    goto Label_0217;
                                }
                                num6 = (((point - num10) - i) + 1) / 60;
                                num7 = (((point - num10) - i) + 1) % 60;
                                int num13 = num7 + ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100);
                                int num14 = ((marketTime.TrendTimeList[i - 1].Open / 0x2710) + num6) + (num13 / 60);
                                num13 = num13 % 60;
                                num8 = (num14 * 0x2710) + (num13 * 100);
                                goto Label_026D;
                            }
                            if (point <= num12)
                            {
                                num6 = (point - num10) / 60;
                                num7 = (point - num10) % 60;
                                int num15 = num7 + ((marketTime.TrendTimeList[i - 1].Open % 0x2710) / 100);
                                int num16 = ((marketTime.TrendTimeList[i - 1].Open / 0x2710) + num6) + (num15 / 60);
                                num15 = num15 % 60;
                                num8 = (num16 * 0x2710) + (num15 * 100);
                                goto Label_026D;
                            }
                        Label_0217:
                            if (((i - 1) < list.Count) && (i >= 1))
                            {
                                num10 += list[i - 1];
                            }
                        }
                        catch (Exception exception)
                        {
                            LogUtilities.LogMessage(exception.StackTrace);
                        }
                    }
                }
            }
        Label_026D:
            if (num8 >= 0x3a980)
            {
                num8 -= 0x3a980;
            }
            return num8;
        }

        public static int GetTimeFromPointOld(int code, int point)
        {
            return 0;
        }

        public static short GetTrendTotalPoint(MarketType market)
        {
            short num = 0;
            MarketTime marketTime = GetMarketTime(market);
            foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
            {
                num = (short)(num + ((short)((((utility.Close / 0x2710) - (utility.Open / 0x2710)) * 60) + (((utility.Close % 0x2710) / 100) - ((utility.Open % 0x2710) / 100)))));
            }
            if (SecurityAttribute.CFTMarketTypes.Contains(market))
            {
                return (short)(num + ((short)marketTime.TrendTimeList.Count));
            }
            return (short)(num + 1);
        }

        public static short GetTrendTotalPoint(int code)
        {
            MarketType marketType = DllImportHelper.GetMarketType(code);
            short num = 0;
            MarketTime marketTime = GetMarketTime(code);
            foreach (TrendTimeUtility utility in marketTime.TrendTimeList)
            {
                num = (short)(num + ((short)((((utility.Close / 0x2710) - (utility.Open / 0x2710)) * 60) + (((utility.Close % 0x2710) / 100) - ((utility.Open % 0x2710) / 100)))));
            }
            if (SecurityAttribute.CFTMarketTypes.Contains(marketType))
            {
                return (short)(num + ((short)marketTime.TrendTimeList.Count));
            }
            return (short)(num + 1);
        }

        public static bool IsCallAuction(int code, int tradeDate, int serverDate, int serverTime)
        {
            if (code == 0)
            {
                return false;
            }
            MarketType marketType = DllImportHelper.GetMarketType(code);
            if (!SecurityAttribute.HasCallAuctionTypeList.Contains(marketType))
            {
                return false;
            }
            MarketTime marketTime = GetMarketTime(marketType);
            return (((serverDate == tradeDate) && (serverTime < marketTime.TrendTimeList[0].Open)) && (serverTime > marketTime.ClearTime));
        }

        public static bool IsSameMarketTime(MarketType market1, MarketType market2)
        {
            MarketTime marketTime = GetMarketTime(market1);
            MarketTime time2 = GetMarketTime(market2);
            return (marketTime == time2);
        }

        public static MarketType JudgeSuperposedStateByTwoMarket(MarketType market1, MarketType market2)
        {
            MarketType type = market1;
            if ((market1 != MarketType.NA) && (market2 != MarketType.NA))
            {
                if (market1 == market2)
                {
                    return type;
                }
                short trendTotalPoint = GetTrendTotalPoint(market1);
                short num2 = GetTrendTotalPoint(market2);
                if (trendTotalPoint >= num2)
                {
                    for (int j = 0; j < num2; j++)
                    {
                        bool flag;
                        byte num5;
                        GetPointFromTime(GetTimeFromPoint(market2, j), market1, out flag, out num5);
                        if (!flag)
                        {
                            return type;
                        }
                    }
                    return type;
                }
                type = market2;
                for (int i = 0; i < trendTotalPoint; i++)
                {
                    bool flag2;
                    byte num8;
                    GetPointFromTime(GetTimeFromPoint(market1, i), market2, out flag2, out num8);
                    if (!flag2)
                    {
                        return market1;
                    }
                }
            }
            return type;
        }

        public static void SetSummerFlag(EmQComm.TypeCode typeCode, byte flag)
        {
            switch (typeCode)
            {
                case EmQComm.TypeCode.TC_CMX:
                    OsCBOTIsSummerTime = flag;
                    OsFutureSummerTime = flag;
                    return;

                case (EmQComm.TypeCode.TC_COptionSell | EmQComm.TypeCode.TC_Norway):
                case EmQComm.TypeCode.TC_SG:
                case (EmQComm.TypeCode.TC_NewZealand | EmQComm.TypeCode.TC_SH):
                    break;

                case EmQComm.TypeCode.TC_LMEELEC:
                    OsVenueIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_LME:
                    OsLMEElecIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_FXInterWinterSummer:
                    ForexIsSummerTime = flag;
                    break;

                case EmQComm.TypeCode.TC_US:
                    UsIsSummerTime = flag;
                    IxicIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_NewZealand:
                    NewZealandIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_DutchAEX:
                    DutchIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_Austria:
                    AustriaIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_Norway:
                    NorwayIsSummerTime = flag;
                    return;

                case EmQComm.TypeCode.TC_AUS:
                    AXATIsSummerTime = flag;
                    return;

                default:
                    return;
            }
        }

        public static byte AustriaIsSummerTime
        {
            get
            {
                return _austriaIsSummerTime;
            }
            set
            {
                if (_austriaIsSummerTime != value)
                {
                    _austriaIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_austriaIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.AustriaIndex] = _austriaMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.AustriaIndex] = _austriaWinterMarketTime;
                    }
                }
            }
        }

        public static byte AXATIsSummerTime
        {
            get
            {
                return _axatIsSummerTime;
            }
            set
            {
                if (_axatIsSummerTime != value)
                {
                    _axatIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_axatIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.AXAT] = _axatMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.AXAT] = _axatWinterMarketTime;
                    }
                }
            }
        }

        public static byte DutchIsSummerTime
        {
            get
            {
                return _dutchIsSummerTime;
            }
            set
            {
                if (_dutchIsSummerTime != value)
                {
                    _dutchIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_dutchIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.DutchAEXIndex] = _dutchAEXMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.DutchAEXIndex] = _dutchAEXWinterMarketTime;
                    }
                }
            }
        }

        public static byte ForexIsSummerTime
        {
            get
            {
                return _forexIsSummerTime;
            }
            set
            {
                if (_forexIsSummerTime != value)
                {
                    _forexIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_forexIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.ForexInterWinterSummer] = _ForexInterMarketSummerTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.ForexInterWinterSummer] = _ForexInterMarketWinterTime;
                    }
                }
            }
        }

        public static byte IxicIsSummerTime
        {
            get
            {
                return _ixicIsSummerTime;
            }
            set
            {
                if (_ixicIsSummerTime != value)
                {
                    _ixicIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_ixicIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.NasdaqIndex] = _ixicMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.NasdaqIndex] = _ixicWinterMarketTime;
                    }
                }
            }
        }

        public static byte NewZealandIsSummerTime
        {
            get
            {
                return _newZealandIsSummerTime;
            }
            set
            {
                if (_newZealandIsSummerTime != value)
                {
                    _newZealandIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_newZealandIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.NewZealandIndex] = _newZealandMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.NewZealandIndex] = _newZealandWinterMarketTime;
                    }
                }
            }
        }

        public static byte NorwayIsSummerTime
        {
            get
            {
                return _norwayIsSummerTime;
            }
            set
            {
                if (_norwayIsSummerTime != value)
                {
                    _norwayIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_norwayIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.NorwayIndex] = _norwayMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.NorwayIndex] = _norwayWinterMarketTime;
                    }
                }
            }
        }

        public static byte OsCBOTIsSummerTime
        {
            get
            {
                return _osCBOTIsSummerTime;
            }
            set
            {
                if (_osCBOTIsSummerTime != value)
                {
                    _osCBOTIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_osCBOTIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.OSFuturesCBOT] = _osFuturesCBOTMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.OSFuturesCBOT] = _osFuturesCBOTWinterMarketTime;
                    }
                }
            }
        }

        public static byte OsFutureSummerTime
        {
            get
            {
                return _osFutureSummerTime;
            }
            set
            {
                if (_osFutureSummerTime != value)
                {
                    _osFutureSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_osFutureSummerTime != 2)
                    {
                        MarketStatus[MarketType.OSFutures] = _osFuturesMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.OSFutures] = _osFutureWinterMarketTime;
                    }
                }
            }
        }

        public static byte OsLMEElecIsSummerTime
        {
            get
            {
                return _osLMEElecIsSummerTime;
            }
            set
            {
                if (_osLMEElecIsSummerTime != value)
                {
                    _osLMEElecIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_osLMEElecIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.OSFuturesLMEElec] = _osFuturesLMEElec;
                    }
                    else
                    {
                        MarketStatus[MarketType.OSFuturesLMEElec] = _osFuturesWinterLMEElec;
                    }
                }
            }
        }

        public static byte OsVenueIsSummerTime
        {
            get
            {
                return _osVenueIsSummerTime;
            }
            set
            {
                if (_osVenueIsSummerTime != value)
                {
                    _osVenueIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_osVenueIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.OSFuturesLMEVenue] = _osFuturesVenue;
                    }
                    else
                    {
                        MarketStatus[MarketType.OSFuturesLMEVenue] = _osFuturesWinterVenue;
                    }
                }
            }
        }

        public static string StrNowTime
        {
            get
            {
                return DateTime.Now.ToString("HHmmss");
            }
        }

        public static byte UsIsSummerTime
        {
            get
            {
                return _usIsSummerTime;
            }
            set
            {
                if (_usIsSummerTime != value)
                {
                    _usIsSummerTime = value;
                    if (SummerTimeChangeEvent != null)
                    {
                        SummerTimeChangeEvent();
                    }
                    if (_usIsSummerTime != 2)
                    {
                        MarketStatus[MarketType.US] = _usMarketTime;
                    }
                    else
                    {
                        MarketStatus[MarketType.US] = _usWinterMarketTime;
                    }
                }
            }
        }

        public delegate void SummerTimeEventHandler();
    }
}
