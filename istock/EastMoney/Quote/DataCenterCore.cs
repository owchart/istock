using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms.VisualStyles;
using EmCore;
using EmCore.Data.Entity;
using EmQComm;
using EmQComm.Formula;
using EmQTCP;
using EmSocketClient;
using Timer = System.Timers.Timer;
using System.Diagnostics;
using UserInfo = EmQComm.UserInfo;
using System.Drawing;
using MarketType = EmQComm.MarketType;
using EmQDataIO;
using dataquery;


namespace EmQDataCore
{
    /// <summary>
    /// DataCenterCore
    /// </summary>
    public class DataCenterCore
    {
        #region 单实例创建，构造函数，初始化
        public void SendLogonCft()
        {
            ReqLogonCftDataPacket dataPacket = new ReqLogonCftDataPacket();
            if (this._cm != null)
            {
                this._cm.Request(dataPacket);
            }
        }

        private static DataCenterCore _dataCenterCore;
        private static bool isCreating = false;

        /// <summary>
        /// 得到DataCenter的单实例对象
        /// </summary>
        /// <returns></returns>
        public static DataCenterCore CreateInstance()
        {
            return CreateInstance(false);
        }

        /// <summary>
        /// 得到DataCenter的单实例对象
        /// </summary>
        /// <returns></returns>
        public static DataCenterCore CreateInstance(bool isIndependece)
        {
            if (isCreating)
            {
                while (_dataCenterCore == null)
                {
                    Thread.Sleep(10);
                }
            }
            isCreating = true;
            if (_dataCenterCore == null)
            {
                if (isIndependece)
                    _dataCenterCore = new DataCenterCore(true);
                else
                    _dataCenterCore = new DataCenterCore(false);
            }
            return _dataCenterCore;
        }

        private ConnectManager2 _cm;
        /// <summary>
        /// 证券服务
        /// </summary>
        private readonly ISecurityHelper _secHelper;

        private static DataTable _blockTreeDataTable;
        private BlockDataRec _blockData;
        private List<int> _mainIndexCodeList;
        public List<int> CustomCodesList;

        /// <summary>
        /// 当前选中的板块id,默认全部AB股
        /// </summary>
        public string CurrentBlockId = "001005";


        private string _defaultBlockId =
            string.Format(
                @"001005,001006,001012,001013,001014,001015,001016,905008,905017,905010,905005,905007,903011,903009,902002,902003,902004,902008,904003,611002,611003,611004,611001,611005,621001,621002,507015,507004,507001,507022,507013,801001,801002,801003,806001,806002,807001,807002,807003,807004,701002,702011,703010,704001,717001,401001,401014,301001,302001,303001,304001,305001,305003,305002");


        /// <summary>
        /// 东财指数Unicode和PUBLISHCODE键值对
        /// </summary>
        private Dictionary<int, string> _dicEMIndexCodePublishCode;

        /// <summary>
        /// PUBLISHCODE和对应指数EMCode的键值对
        /// </summary>
        private Dictionary<string, string> _dicPublishCodeIndexCode;

        /// <summary>
        /// 键盘精灵文件
        /// </summary>
        private List<string> CodeNameFilePath;



        /// <summary>
        /// 用户板块，Key:BlockId, Value:BlockName
        /// </summary>
        public List<KeyValuePair<string, string>> UserBlock
        {
            get
            {
                if (_userBlock == null || _userBlock.Count <= 0)
                {
                    LogUtilities.LogMessage("自选股板块获取失败");
                    _userBlock = GetUserBlocks();
                }
                return _userBlock;
            }
            private set { _userBlock = value; }
        }

        /// <summary>
        /// 不读配置文件，只接收码表等
        /// </summary>
        /// <param name="isIndependence"></param>
        private DataCenterCore()
            : this(false)
        {
        }

        /// <summary>
        /// 不读配置文件，只接收码表等
        /// </summary>
        /// <param name="isIndependence"></param>
        private DataCenterCore(bool isIndependence)
        {
            #region 初始化集合
            CodeNameFilePath = new List<string>(31);
            CodeNameFilePath.Add("1");
            CodeNameFilePath.Add("2");
            CodeNameFilePath.Add("3");
            CodeNameFilePath.Add("4");
            CodeNameFilePath.Add("5");
            CodeNameFilePath.Add("6");
            CodeNameFilePath.Add("9");
            CodeNameFilePath.Add("10");
            CodeNameFilePath.Add("11");
            CodeNameFilePath.Add("12");
            CodeNameFilePath.Add("13");
            CodeNameFilePath.Add("14");
            CodeNameFilePath.Add("17");
            CodeNameFilePath.Add("19");
            CodeNameFilePath.Add("31");
            CodeNameFilePath.Add("71");
            CodeNameFilePath.Add("72");
            CodeNameFilePath.Add("35");
            CodeNameFilePath.Add("36");
            CodeNameFilePath.Add("37");
            CodeNameFilePath.Add("38");
            CodeNameFilePath.Add("39");
            CodeNameFilePath.Add("40");
            CodeNameFilePath.Add("41");
            CodeNameFilePath.Add("42");
            CodeNameFilePath.Add("43");
            CodeNameFilePath.Add("44");
            CodeNameFilePath.Add("45");
            CodeNameFilePath.Add("46");
            CodeNameFilePath.Add("47");
            CodeNameFilePath.Add("48");
            CodeNameFilePath.Add("49");
            CodeNameFilePath.Add("51");
            #endregion

            if (isIndependence)
            {
                SystemConfig.UserInfo = new UserInfo();
                SystemConfig.UserInfo.HaveSHLevel2Right = false;
                SystemConfig.UserInfo.HaveSZLevel2Right = false;
                InitDataTable(true);

                _cm = ConnectManager2.CreateInstance();
                _cm.DoCMReceiveData += new EventHandler<CMRecvDataEventArgs>(_cm_DoCMReceiveData);

                Debug.Print("Send Req true");
                SendInitRequest(true);


            }
            else
            {
                try
                {
                    #region 用户权限控制等操作
                    SystemConfig.UserInfo = SysCfgFileIO.GetUserInfo(); //从配置文件中获取用户的信息
#if DEBUG
                    SystemConfig.UserInfo.HaveSHLevel2Right = true;
                    SystemConfig.UserInfo.HaveSZLevel2Right = true;
                    SystemConfig.UserInfo.HaveDebtMemberRight = true;
#else
                    SystemConfig.UserInfo.HaveSHLevel2Right = Authentication.IsLV2Member;
                    SystemConfig.UserInfo.HaveSZLevel2Right = Authentication.IsLV2Member;
                    SystemConfig.UserInfo.HaveDebtMemberRight = Authentication.IsDebtMember;
#endif
                    CustomCodesList = new List<int>(0);
                    #endregion

                    #region 读配置文件

                    DateTime dtReadCfg = DateTime.Now;
                    TimeSpan tsReadCfg = DateTime.Now - dtReadCfg;
                    LogUtilities.LogMessage("【******行情初始化******】读配置文件，用时:" + tsReadCfg.TotalMilliseconds);
                    #endregion

                    #region 初始化数据

                    DateTime dtData = DateTime.Now;

                    //TradeDate = TimeUtilities.GetLastTradeDateInt();
                    InitDataTable();

                    _cm = ConnectManager2.CreateInstance();
                    _cm.DoCMReceiveData += new EventHandler<CMRecvDataEventArgs>(_cm_DoCMReceiveData);

                    TimeSpan tsData = DateTime.Now - dtData;
                    #endregion

                    #region 请求静态数据


                    _blockIdStocksCash = new Dictionary<string, List<string>>();

                    try
                    {

                        SetIndexData();

                        ReadMainIndex();
                        LoadQuoteData(true);
                        ReadIndexPublishCode();
                        //SendInitRequest();
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("板块数据加载失败" + e.Message);
                    }

                    #endregion

                    LoadImage();

                    #region 用户板块
                    //获取用户板块
                    UserBlock = GetUserBlocks();
                    #endregion

                    #region 加载用户股票标记信息至内存中

                    //加载用户的股票标记信息
                    StockMarkInfoMananger t = StockMarkInfoMananger.GetInstance();

                    Dictionary<FieldIndex, int> fieldInt32;
                    Dictionary<FieldIndex, object> fieldObject;
                    foreach (int code in t.StockMarkInfo.Keys)
                    {
                        if (!DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                        {
                            fieldInt32 = new Dictionary<FieldIndex, int>(1);
                            DetailData.FieldIndexDataInt32[code] = fieldInt32;
                        }
                        StockTag st = t.StockMarkInfo[code].StockTag;
                        fieldInt32[FieldIndex.StockTagEnum] = (int)st;
                        if (st == StockTag.Text)
                        {
                            if (!DetailData.FieldIndexDataObject.TryGetValue(code, out fieldObject))
                            {
                                fieldObject = new Dictionary<FieldIndex, object>(1);
                                DetailData.FieldIndexDataObject[code] = fieldObject;
                            }
                            fieldObject[FieldIndex.StockTagText] = t.StockMarkInfo[code].MarkInfo;
                        }
                    }

                    #endregion

                    TimeUtilities.UsIsSummerTime = 2;
                    TimeUtilities.IxicIsSummerTime = 2;
                    TimeUtilities.OsFutureSummerTime = 2;
                    TimeUtilities.OsCBOTIsSummerTime = 2;
                    TimeUtilities.OsLMEElecIsSummerTime = 2;
                    TimeUtilities.OsVenueIsSummerTime = 2;
                    TimeUtilities.DutchIsSummerTime = 2;
                    TimeUtilities.AustriaIsSummerTime = 2;
                    TimeUtilities.NorwayIsSummerTime = 2;
                    TimeUtilities.NewZealandIsSummerTime = 1;
                    //this._cm.DoAddOneClient += _cm_DoAddOneClient;
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("创建数据中心失败!! " + e.Message);
                }
            }
        }

        //void _blockService_OnBlockLoadSuccess()
        //{
        //    LoadQuoteData();
        //}

        //void _cm_DoAddOneClient(object sender, EmQTCP.ConnectEventArgs e)
        //{
        //    LoadQuoteData();
        //}

        /// <summary>
        /// 全局数据加载完以后，加载的行情数据
        /// </summary>
        public void LoadQuoteData
            (bool isLoadGlobal)
        {
            if (isLoadGlobal)
            {
                LogUtilities.LogMessage("【******行情初始化******】开始获取板块树");
                LogUtilities.LogMessage("【******行情初始化******】完成获取板块树");
            }


            ReadCodeNameFiles();

            if (isLoadGlobal)
                PreLoadBlockStocks();

            if (!isLoadGlobal)
            {
                ReadReadStockPublishDataTable();
                SendInitRequest();
            }
        }

        private void LoadImage()
        {
            try
            {
                #region 走势K线图像加载

                SingleInfoImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\infoMine.gif"));
                MultiInfoImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\multiInfoMine.gif"));
                PlusNormalImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\plusNormal.gif"));
                PlusSelectedImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\plusSelected.gif"));
                MinusNormalImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\minusNormal.gif"));
                MinusSelectedImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\minusSelected.gif"));
                RenameImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\rename.gif"));
                FuquanImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\fuquan.gif"));
                StockTagTitleImage = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTagTitleImage.gif"));
                StockTag_1_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_1.gif"));
                StockTag_2_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_2.gif"));
                StockTag_3_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_3.gif"));
                StockTag_4_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_4.gif"));
                StockTag_5_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_5.gif"));
                StockTag_6_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_6.gif"));
                StockTag_7_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_7.gif"));
                StockTag_8_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_8.gif"));
                StockTag_9_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_9.gif"));
                StockTag_Text_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\StockTag_Text.gif"));
                HelpBtn_01_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HelpBtn_01.png"));
                Arrow_SetUserStock_Image = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\arrow_setuserstock.png"));

                #endregion

                #region 画图工具图像加载

                #region 趋势线

                //线段
                SegmentNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\SegmentNormal.gif"));
                SegmentPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\SegmentPass.gif"));
                SegmentSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\SegmentSelected.gif"));
                //趋势线
                TrendLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TrendLineNormal.gif"));
                TrendLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TrendLinePass.gif"));
                TrendLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TrendLineSelected.gif"));
                //水平线
                HorizontalLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HorizontalLineNormal.gif"));
                HorizontalLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HorizontalLinePass.gif"));
                HorizontalSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HorizontalSelected.gif"));
                //平行线
                ParallelNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ParallelNormal.gif"));
                ParallelPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ParallelPass.gif"));
                ParallelSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ParallelSelected.gif"));
                //平行线段
                ParallelSegmentNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\ParallelSegmentNormal.gif"));
                ParallelSegmentPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ParallelSegmentPass.gif"));
                ParallelSegmentSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\ParallelSegmentSelected.gif"));
                //三浪线
                ThreePointLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ThreePointLineNormal.gif"));
                ThreePointLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ThreePointLinePass.gif"));
                ThreePointLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\ThreePointLineSelected.gif"));
                //五浪线
                FivePointLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\FivePointLineNormal.gif"));
                FivePointLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\FivePointLinePass.gif"));
                FivePointLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\FivePointLineSelected.gif"));
                //八浪线
                EightPointLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\EightPointLineNormal.gif"));
                EightPointLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\EightPointLinePass.gif"));
                EightPointLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\EightPointLineSelected.gif"));
                //头肩线
                MountainLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MountainLineNormal.gif"));
                MountainLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MountainLinePass.gif"));
                MountainLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MountainLineSelected.gif"));
                //W头M底
                WLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\WLineNormal.gif"));
                WLinePass = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\WLinePass.gif"));
                WLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\WLineSelected.gif"));

                #endregion

                #region 工具

                DeleteAllNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DeleteAllNormal.gif"));
                DeleteAllPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DeleteAllPass.gif"));
                DeleteAllSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DeleteAllSelected.gif"));

                DeleteNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DeleteNormal.gif"));
                DeletePass = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DeletePass.gif"));
                DeleteSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DeleteSelected.gif"));

                DownArrowNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DownArrowNormal.gif"));
                DownArrowPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DownArrowPass.gif"));
                DownArrowSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\DownArrowSelected.gif"));

                FixNormal = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\FixNormal.gif"));
                FixPass = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\FixPass.gif"));
                FixSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\FixSelected.gif"));

                MeasureNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MeasureNormal.gif"));
                MeasurePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MeasurePass.gif"));
                MeasureSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MeasureSelected.gif"));

                MouseNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MouseNormal.gif"));
                MousePass = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MousePass.gif"));
                MouseSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\MouseSelected.gif"));

                TextNormal = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TextNormal.gif"));
                TextPass = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TextPass.gif"));
                TextSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TextSelected.gif"));

                UpArrpwNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\UpArrpwNormal.gif"));
                UpArrpwPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\UpArrpwPass.gif"));
                UpArrpwSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\UpArrpwSelected.gif"));

                Up = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\Up.gif"));
                Down = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\Down.gif"));

                #endregion

                #region 空间

                GoldenSectionNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\GoldenSectionNormal.gif"));
                GoldenSectionPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\GoldenSectionPass.gif"));
                GoldenSectionSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\GoldenSectionSelected.gif"));

                HorizonSpaceNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HorizonSpaceNormal.gif"));
                HorizonSpacePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HorizonSpacePass.gif"));
                HorizonSpaceSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HorizonSpaceSelected.gif"));

                PercentageLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\PercentageLineNormal.gif"));
                PercentageLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\PercentageLinePass.gif"));
                PercentageLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\PercentageLineSelected.gif"));

                RangeRulerNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RangeRulerNormal.gif"));
                RangeRulerPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RangeRulerPass.gif"));
                RangeRulerSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RangeRulerSelected.gif"));

                TwistRulerNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TwistRulerNormal.gif"));
                TwistRulerPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TwistRulerPass.gif"));
                TwistRulerSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TwistRulerSelected.gif"));

                WaveRulerNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\WaveRulerNormal.gif"));
                WaveRulerPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\WaveRulerPass.gif"));
                WaveRulerSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\WaveRulerSelected.gif"));

                #endregion

                #region 时间

                FibonacciPeriodNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\FibonacciPeriodNormal.gif"));
                FibonacciPeriodPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\FibonacciPeriodPass.gif"));
                FibonacciPeriodSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\FibonacciPeriodSelected.gif"));

                FreeKarlFischerLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\FreeKarlFischerLineNormal.gif"));
                FreeKarlFischerLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\FreeKarlFischerLinePass.gif"));
                FreeKarlFischerLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\FreeKarlFischerLineSelected.gif"));

                PeriodLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\PeriodLineNormal.gif"));
                PeriodLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\PeriodLinePass.gif"));
                PeriodLineSlected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\PeriodLineSlected.gif"));

                SymmetricalAngleNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\SymmetricalAngleNormal.gif"));
                SymmetricalAnglePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\SymmetricalAnglePass.gif"));
                SymmetricalAngleSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\SymmetricalAngleSelected.gif"));

                SymmetricalLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\SymmetricalLineNormal.gif"));
                SymmetricalLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\SymmetricalLinePass.gif"));
                SymmetricalLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\SymmetricalLineSelected.gif"));

                TimeRulerNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TimeRulerNormal.gif"));
                TimeRulerPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TimeRulerPass.gif"));
                TimeRulerSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TimeRulerSelected.gif"));

                #endregion

                #region 时空

                ArcNormal = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ArcNormal.gif"));
                ArcPass = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ArcPass.gif"));
                ArcSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ArcSelected.gif"));

                BoxLineNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\BoxLineNormal.gif"));
                BoxLinePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\BoxLinePass.gif"));
                BoxLineSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\BoxLineSelected.gif"));

                GannAngleNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\GannAngleNormal.gif"));
                GannAnglePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\GannAnglePass.gif"));
                GannAngleSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\GannAngleSelected.gif"));

                LinearHuiGuiDaiNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\LinearHuiGuiDaiNormal.gif"));
                LinearHuiGuiDaiPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\LinearHuiGuiDaiPass.gif"));
                LinearHuiGuiDaiSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\LinearHuiGuiDaiSelected.gif"));

                LinearHuiGuiXianNoraml =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\LinearHuiGuiXianNoraml.gif"));
                LinearHuiGuiXianPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\LinearHuiGuiXianPass.gif"));
                LinearHuiGuiXianSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\LinearHuiGuiXianSelected.gif"));

                RectangleNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RectangleNormal.gif"));
                RectanglePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RectanglePass.gif"));
                RectangleSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RectangleSelected.gif"));

                RegressionChannelNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\RegressionChannelNormal.gif"));
                RegressionChannelPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\RegressionChannelPass.gif"));
                RegressionChannelSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\RegressionChannelSelected.gif"));

                ResistanceSpeedCurveNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\ResistanceSpeedCurveNormal.gif"));
                ResistanceSpeedCurvePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\ResistanceSpeedCurvePass.gif"));
                ResistanceSpeedCurveSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\ResistanceSpeedCurveSelected.gif"));

                RhomboidNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RhomboidNormal.gif"));
                RhomboidPass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RhomboidPass.gif"));
                RhomboidSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\RhomboidSelected.gif"));

                TriangleNormal =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TriangleNormal.gif"));
                TrianglePass =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TrianglePass.gif"));
                TriangleSelected =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\TriangleSelected.gif"));

                #endregion

                #region 垂直Htab

                Imagelist = new List<Image>();
                Imagelist.Add(Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\fenshitu.gif")));
                Imagelist.Add(
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jishufenxi.gif")));
                Imagelist.Add(
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jibenziliao.gif")));
                Imagelist.Add(
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\fenshichengjiao.gif")));
                Imagelist.Add(
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\fenjiabiao.gif")));
                Imagelist.Add(
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\zhubichengjiao.gif")));
                Imagelist.Add(Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\duozhouqi.gif")));
                Imagelist.Add(
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\zhubiweituo.gif")));

                #endregion

                #endregion

                #region 自选股tab图片

                CustTabCommomImage0 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabCommon0.gif"));
                CustTabCommomImage1 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabCommon1.gif"));
                CustTabCommomImage2 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabCommon2.gif"));
                CustTabCommomImage3 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabCommon3.gif"));

                CustTabSelectedImage0 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabSelected0.gif"));
                CustTabSelectedImage1 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabSelected1.gif"));
                CustTabSelectedImage2 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabSelected2.gif"));
                CustTabSelectedImage3 =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\CustTabSelected3.gif"));

                #endregion

                #region 成交明细箭头

                PriceUpImg = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\priceup.png"));
                PriceDownImg =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\pricedown.png"));

                #endregion

                #region 初始排序

                InitCustBlockOrderImg =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\InitCustBlockOrder.png"));
                InitCustBlockOrder_MouseOnImg =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath,
                                                @"Plugin\Images\Quote\InitCustBlockOrder_MouseOn.png"));

                #endregion

                #region 代码名称

                TopBannerAddUserStock =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiaruNormal.png"));
                TopBannerAddUserStockMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiaruMouseDown.png"));
                TopBannerAddUserStockMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiaruMouseOn.png"));
                TopBannerBStock =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\BNormal.png"));
                TopBannerBStockMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\BMouseDown.png"));
                TopBannerBStockMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\BMouseOn.png"));
                TopBannerHBStockB =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\bhBNormal.png"));
                TopBannerHBStockBMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\bhBMouseDown.png"));
                TopBannerHBStockBMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\bhBMouseOn.png"));
                TopBannerHBStockH =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\bhHNormal.png"));
                TopBannerHBStockHMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\bhHMouseDown.png"));
                TopBannerHBStockHMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\bhHMouseOn.png"));
                TopBannerHStock =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HNormal.png"));
                TopBannerHStockMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HMouseDown.png"));
                TopBannerHStockMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\HMouseOn.png"));
                TopBannerLeftArrow =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiantouLNormal.png"));
                TopBannerLeftArrowMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiantouLMouseDown.png"));
                TopBannerLeftArrowMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiantouLMouseOn.png"));
                TopBannerRemoveUserStock =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\shanchuNormal.png"));
                TopBannerRemoveUserStockMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\shanchuMouseDown.png"));
                TopBannerRemoveUserStockMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\shanchuMouseOn.png"));
                TopBannerRightArrow =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiantouRNormal.png"));
                TopBannerRightArrowMouseDown =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiantouRMouseDown.png"));
                TopBannerRightArrowMouseOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\jiantouRMouseOn.png"));
                TopBannerMenuExpand =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\zhankaiNormal.png"));
                TopBannerMenuExpandOn =
                    Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\zhankaiMouseOn.png"));
                #endregion

                FullViewUserStockArrow = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\fullViewUSArrow.png"));
                StockDescription = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\shuoming.png"));
                StockItemDot = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\yuandian.gif"));
                ImgAdd = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\add.gif"));
                ImgAdd_On = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\add_on.gif"));
                ImgOrderDetailNormal = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\ordernormal.png"));
                ImgOrderDetailBigSell = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\orderbigsell.png"));
                ImgOrderDetailBigBuy = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\orderbigbuy.png"));
                ImgClose = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\close.png"));
                ImgLev2Help = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\lev2help.jpg"));
                ImgGubaData = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\gubadata.png"));
                ImgGubaFix = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\gubafix.png"));
                ImgGubaNews = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\gubanews.png"));
                ImgGubaNotice = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\gubanotice.png"));
                ImgGubaResearch = Image.FromFile(Path.Combine(PathUtilities.MDataPath, @"Plugin\Images\Quote\gubaresearch.png"));
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("LoadImage Error" + e.Message);
            }
        }

        public void SendHeart()
        {
            ReqHeartDataPacket packet = new ReqHeartDataPacket();
            this._cm.Request(packet);
        }

        /// <summary>
        /// 自选股板块发生变化
        /// </summary>
        private void OnBlockUpdatedChanged()
        {
            UserBlock = GetUserBlocks();
            if (UserBlock != null && UserBlock.Count > 0)
            {
                SetDefaultCustomBlock();
            }
        }



        /// <summary>
        /// DC通知界面刷新
        /// </summary>
        public event EventHandler InvalidateControl;

        /// <summary>
        /// 自选股同步
        /// </summary>
        private void _blockService_OnBlockSyncSuccess()
        {
            UserBlock = GetUserBlocks();

            if (UserBlock != null && UserBlock.Count > 0)
            {
                SetDefaultCustomBlock();
            }
        }


        /// <summary>
        /// 设置自选股0
        /// </summary>
        public void SetDefaultCustomBlock()
        {
            //设置自选股0
            GetBlockStockListOrg("0.U", DefaultUserBlockCallBack);
        }

        void DefaultUserBlockCallBack(string block, List<int> blockStocks)
        {
            if (block == "0.U")
            {

                for (int i = 0; i < CustomCodesList.Count; i++)
                {
                    Dictionary<FieldIndex, object> fieldObject;
                    if (!DetailData.FieldIndexDataObject.TryGetValue(CustomCodesList[i], out fieldObject))
                    {
                        fieldObject = new Dictionary<FieldIndex, object>(1);
                        DetailData.FieldIndexDataObject[CustomCodesList[i]] = fieldObject;
                    }
                    fieldObject[FieldIndex.IsDefaultCustomStock] = false;
                }
                CustomCodesList.Clear();
                if (blockStocks != null && blockStocks.Count > 0)
                {
                    for (int i = 0; i < blockStocks.Count; i++)
                    {
                        Dictionary<FieldIndex, object> fieldObject;
                        if (!DetailData.FieldIndexDataObject.TryGetValue(blockStocks[i], out fieldObject))
                        {
                            fieldObject = new Dictionary<FieldIndex, object>(1);
                            DetailData.FieldIndexDataObject[blockStocks[i]] = fieldObject;
                        }
                        fieldObject[FieldIndex.IsDefaultCustomStock] = true;
                    }
                }
                if (InvalidateControl != null)
                    InvalidateControl(this, new EventArgs());
            }
        }



        #region 读板块树，获得行业，概念，区域板块,东财指数
        /// <summary>
        /// 预加载全部A股，沪A，深A，沪B，深B
        /// </summary>
        private void PreLoadBlockStocks()
        {
            LogUtilities.LogMessage("【******行情初始化******】开始获取板块成分");
            //GetBlockStockListOrg("001001", BlockCallBack);
            //GetBlockStockListOrg("905008033", BlockCallBack);
            //GetBlockStockListOrg("905005", BlockCallBack);
            List<string> blocks = new List<string>(3);
            blocks.Add("001001");//沪深重点指数
            blocks.Add("905008033");//沪深重点指数
            blocks.Add("905005");//全球指数
            GetBlockStockListOrg(blocks, BlockCallBack);
        }

        private void BlockCallBack(string blockid, List<int> stocks)
        {
            LogUtilities.LogMessage("【******行情初始化******】完成获取板块成分");
            ReadReadStockPublishDataTable();
            SendInitRequest();
        }


        private void ReadCodeNameFiles()
        {
            //设置码表
            Dictionary<String, KwItem> items = SecurityService.KwItems;
            foreach (KwItem item in items.Values)
            {
                DetailData.SetStockBasicField(item.Innercode, item.Code, item.Type, item.Name);
                DetailData.EmCodeToUnicode[item.Code] = item.Innercode;
            }
            string pathRoot = PathUtilities.SDataPath + @"KeySprite\";
            foreach (string file in CodeNameFilePath)
            {
                if (file == "11" || file == "12")//商品期货
                {
                    string path = pathRoot + file;
                    if (File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                            {
                                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                                string data = sr.ReadToEnd();
                                string[] allStocks = data.Split('}');
                                foreach (string oneStock in allStocks)
                                {
                                    string[] stock = oneStock.Split('$');
                                    if (!DetailData.EmCodeToUnicode.ContainsKey(stock[0]))
                                    {
                                        string strUnicode = stock[stock.Length - 1];
                                        int unicode = 0;
                                        if (!string.IsNullOrEmpty(strUnicode))
                                            unicode = Convert.ToInt32(strUnicode);
                                        DetailData.EmCodeToUnicode.Add(stock[0], unicode);
                                    }
                                    string shortcode = stock[0].Split('.')[0];
                                    if (!SecurityAttribute.FuturesCode.ContainsKey(shortcode))
                                        SecurityAttribute.FuturesCode.Add(shortcode, stock[0]);
                                }

                            }
                        }
                    }
                }
                else
                {
                    string path = pathRoot + file;
                    if (File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                            {
                                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                                string data = sr.ReadToEnd();
                                string[] allStocks = data.Split('}');
                                foreach (string oneStock in allStocks)
                                {
                                    string[] stock = oneStock.Split('$');
                                    if (!DetailData.EmCodeToUnicode.ContainsKey(stock[0]))
                                    {
                                        string strUnicode = stock[stock.Length - 1];
                                        int unicode = 0;
                                        if (!string.IsNullOrEmpty(strUnicode))
                                            unicode = Convert.ToInt32(strUnicode);
                                        DetailData.EmCodeToUnicode.Add(stock[0], unicode);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        private void ReadReadStockPublishDataTable()
        {
            LogUtilities.LogMessage("开始读所属板块文件，time=" + DateTime.Now.ToLongTimeString());
            try
            {
                if (_dicEMIndexCodePublishCode == null)
                    _dicEMIndexCodePublishCode = new Dictionary<int, string>(1);
                Dictionary<string, string> industry = new Dictionary<string, string>();
                Dictionary<string, string> area = new Dictionary<string, string>();
                Dictionary<string, string> concept = new Dictionary<string, string>();
                List<int> conceptItems = new List<int>();
                List<int> areaItems = new List<int>();
                List<int> industryItems = new List<int>();

                string fileName = PathUtilities.SDataPath + "NecessaryData\\" + "SPE_STOCKPUBLISH";
                if (File.Exists(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
                            sr.ReadLine();
                            string oneLine = string.Empty;
                            string[] oneStock = null;
                            bool hasConcept = true;

                            while (sr.Peek() > -1)
                            {
                                hasConcept = true;
                                oneLine = sr.ReadLine();
                                if (!string.IsNullOrEmpty(oneLine))
                                {
                                    oneStock = oneLine.Split('$');
                                    if (oneStock.Length == 14)
                                    {
                                        if (string.IsNullOrEmpty(oneStock[5]))
                                            continue;
                                        if (string.IsNullOrEmpty(oneStock[10]) || string.IsNullOrEmpty(oneStock[13]))
                                        {
                                            hasConcept = false;
                                        }

                                        _dicEMIndexCodePublishCode[Convert.ToInt32(oneStock[5])] = oneStock[2];
                                        _dicEMIndexCodePublishCode[Convert.ToInt32(oneStock[9])] = oneStock[6];
                                        if (hasConcept)
                                            _dicEMIndexCodePublishCode[Convert.ToInt32(oneStock[13])] = oneStock[10];

                                        industry[oneStock[6]] = oneStock[7];
                                        if (!industryItems.Contains(Convert.ToInt32(oneStock[9])))
                                            industryItems.Add(Convert.ToInt32(oneStock[9]));

                                        area[oneStock[2]] = oneStock[3];
                                        if (!areaItems.Contains(Convert.ToInt32(oneStock[5])))
                                            areaItems.Add(Convert.ToInt32(oneStock[5]));
                                        if (!string.IsNullOrEmpty(oneStock[11]) && !string.IsNullOrEmpty(oneStock[13]))
                                        {
                                            concept[oneStock[10]] = oneStock[11];
                                            if (!conceptItems.Contains(Convert.ToInt32(oneStock[13])))
                                                conceptItems.Add(Convert.ToInt32(oneStock[13]));
                                        }

                                        DetailData.SetStockBasicField(Convert.ToInt32(oneStock[5]), oneStock[4],
                                            MarketType.EMINDEX, oneStock[3]);

                                        DetailData.SetStockBasicField(Convert.ToInt32(oneStock[9]), oneStock[8],
                                            MarketType.EMINDEX, oneStock[7]);

                                        if (hasConcept)
                                        {
                                            if (!string.IsNullOrEmpty(oneStock[11]))
                                            {
                                                DetailData.SetStockBasicField(Convert.ToInt32(oneStock[13]),
                                                    oneStock[12],
                                                    MarketType.EMINDEX, oneStock[11]);
                                            }
                                        }
                                        DetailData.SetFieldData(Convert.ToInt32(oneStock[1]),
                                            FieldIndex.IndustryBlockCode, Fieldtype.TypeString,
                                            Convert.ToInt32(oneStock[9]), 0, 0, 0, null, null);
                                        DetailData.SetFieldData(Convert.ToInt32(oneStock[1]),
                                            FieldIndex.IndustryBlockName, Fieldtype.TypeString,
                                            0, 0, 0, 0, oneStock[7], null);

                                        Dictionary<FieldIndex, object> fieldObject;
                                        if (
                                            !DetailData.FieldIndexDataObject.TryGetValue(Convert.ToInt32(oneStock[1]),
                                                out fieldObject))
                                        {
                                            fieldObject = new Dictionary<FieldIndex, object>(1);
                                            DetailData.FieldIndexDataObject[Convert.ToInt32(oneStock[1])] = fieldObject;
                                        }
                                        if (fieldObject.ContainsKey(FieldIndex.AllBlockCode))
                                        {
                                            List<int> codes = fieldObject[FieldIndex.AllBlockCode] as List<int>;
                                            if (!codes.Contains(Convert.ToInt32(oneStock[9])))
                                                codes.Add(Convert.ToInt32(oneStock[9]));
                                            if (!codes.Contains(Convert.ToInt32(oneStock[5])))
                                                codes.Add(Convert.ToInt32(oneStock[5]));
                                            if (hasConcept && !codes.Contains(Convert.ToInt32(oneStock[13])))
                                                codes.Add(Convert.ToInt32(oneStock[13]));
                                        }
                                        else
                                        {
                                            if (hasConcept)
                                            {
                                                List<int> tmpList = new List<int>();
                                                tmpList.Add(Convert.ToInt32(oneStock[13]));
                                                tmpList.Add(Convert.ToInt32(oneStock[5]));
                                                tmpList.Add(Convert.ToInt32(oneStock[9]));
                                                fieldObject[FieldIndex.AllBlockCode] = tmpList;
                                            }
                                            else
                                            {
                                                List<int> tmpList = new List<int>();
                                                tmpList.Add(Convert.ToInt32(oneStock[5]));
                                                tmpList.Add(Convert.ToInt32(oneStock[9]));
                                                fieldObject[FieldIndex.AllBlockCode] = tmpList;
                                            }
                                        }
                                    }
                                }
                                oneLine = string.Empty;
                                oneLine = null;
                                oneStock = null;
                            }
                        }
                    }
                }
                _blockData = new BlockDataRec(industry, area, concept);
                _blockData.SetData(industryItems, areaItems, conceptItems);
                LogUtilities.LogMessage("完成读所属板块文件，time=" + DateTime.Now.ToLongTimeString());
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("ReadReadStockPublishDataTable Error" + e.Message);
            }
        }

        private void ReadMainIndex()
        {
            try
            {
                string fileName = PathUtilities.SDataPath + "NecessaryData\\" + "IND_MAININDEX";
                if (File.Exists(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
                            sr.ReadLine();
                            string oneLine = string.Empty;
                            string[] oneStock = null;
                            if (_mainIndexCodeList == null)
                                _mainIndexCodeList = new List<int>(20);
                            else
                                _mainIndexCodeList.Clear();

                            while (sr.Peek() > -1)
                            {
                                oneLine = sr.ReadLine();
                                if (!string.IsNullOrEmpty(oneLine))
                                {
                                    oneStock = oneLine.Split('$');
                                    if (oneStock.Length == 5)
                                    {
                                        _mainIndexCodeList.Add(Convert.ToInt32(oneStock[1]));
                                        DetailData.SetStockBasicField(Convert.ToInt32(oneStock[1]),
                                            Convert.ToString(oneStock[4]), Convert.ToInt32(oneStock[3]),
                                            Convert.ToString(oneStock[2]));
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("ReadMainIndex" + e.Message);
            }
        }

        private void ReadIndexPublishCode()
        {
            try
            {
                string fileName = PathUtilities.SDataPath + "NecessaryData\\" + "IND_BKZSDYGX";
                if (File.Exists(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
                            sr.ReadLine();
                            string oneLine = string.Empty;
                            string[] oneStock = null;
                            if (_dicEMIndexCodePublishCode == null)
                                _dicEMIndexCodePublishCode = new Dictionary<int, string>(1);

                            while (sr.Peek() > -1)
                            {
                                oneLine = sr.ReadLine();
                                if (!string.IsNullOrEmpty(oneLine))
                                {
                                    oneStock = oneLine.Split('$');
                                    if (oneStock.Length == 5)
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(oneStock[0])))
                                        {
                                            DetailData.SetStockBasicField(Convert.ToInt32(oneStock[3]),
                                                Convert.ToString(oneStock[1]), Convert.ToInt32(oneStock[4]),
                                                Convert.ToString(oneStock[2]));
                                            _dicEMIndexCodePublishCode[Convert.ToInt32(oneStock[3])] =
                                                Convert.ToString(oneStock[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
                (Exception e)
            {
                LogUtilities.LogMessage("ReadIndexPublishCode error :" + e.Message);
            }
        }

        #endregion

        /// <summary>
        /// SendInitRequest
        /// </summary>
        /// <param name="isDependence"></param>
        public void SendInitRequest()
        {
            SendInitRequest(false);
        }

        /// <summary>
        /// SendInitRequest
        /// </summary>
        /// <param name="isDependence"></param>
        public void SendInitRequest(bool isDependence)
        {
            SendHeart();
            LogUtilities.LogMessage("【******行情初始化******】向机构服务器请交易日表");
            //交易日表
            _cm.Request(new ReqTradeDateDataPacket());

            if (!isDependence)
            {
                LogUtilities.LogMessage("【******行情初始化******】向机构服务器请求报价初始化数据");
                _cm.Request(new ReqReportInitOrgDataPacket());

                LogUtilities.LogMessage("【******行情初始化******】向机构服务器请求股票名称变更数据");
                _cm.Request(new ReqChangeNameDataPacket());

                LogUtilities.LogMessage("【******行情初始化******】向机构服务器请求复权因子数据");
                _cm.Request(new ReqDivideRightOrgDataPacket());

                LogUtilities.LogMessage("【******行情初始化******】向机构服务器请求财务数据");
                _cm.Request(new ReqFinanceOrgDataPacket());
            }
        }

        private void SetIndexData()
        {
            DetailData.SetStockBasicField(1000158679, "000001.SH", MarketType.SHINDEX, "上证指数");
            DetailData.SetStockBasicField(1000158617, "399001.SZ", MarketType.SZINDEX, "深证成指");
            DetailData.SetStockBasicField(1000159295, "399005.SZ", MarketType.SZINDEX, "中小板指");
            DetailData.SetStockBasicField(1000159296, "399006.SZ", MarketType.SZINDEX, "创业板");
            DetailData.SetStockBasicField(1000158855, "399003.SZ", MarketType.SZINDEX, "成分B指");
            DetailData.SetStockBasicField(1000141996, "IF00C1.CFE", MarketType.IF, "IF当月连续");
        }

        #endregion

        #region 内存数据表，码表，配置文件等数据结构的定义

        /// <summary>
        /// 板块成分缓存
        /// </summary>
        private Dictionary<string, List<string>> _blockIdStocksCash;

        /// <summary>
        /// 数据结构表
        /// </summary>
        private Dictionary<string, DataTableBase> _dataTableCollecion;

        /// <summary>
        /// 右边信息栏所有的Charts
        /// </summary>
        public Dictionary<string, Dictionary<string, InfoPanelChart>> InfoPanelCharts
        {
            get { return _infoPanelCharts; }
            private set { _infoPanelCharts = value; }
        }

        /// <summary>
        /// 右边信息栏布局的定义
        /// </summary>
        public Dictionary<MarketType, InfoPanelLayout> InfoPanelLayout
        {
            get { return _infoPanelLayout; }
            private set { _infoPanelLayout = value; }
        }

        /// <summary>
        /// 个股界面的顶部菜单
        /// </summary>
        public Dictionary<MarketType, List<TopBannerMenuItemPair>> TopBannerMenu
        {
            get { return _topBannerMenu; }
            private set { _topBannerMenu = value; }
        }

        /// <summary>
        /// 公式类型字典
        /// </summary>
        public FormulaDict FormulaDict
        {
            get { return _formulaDict; }
            private set { _formulaDict = value; }
        }

        /// <summary>
        /// 公式指标系统中内置函数
        /// </summary>
        public FormulaFunctions FormulaFunctions
        {
            get { return _formulaFunctions; }
            private set { _formulaFunctions = value; }
        }

        private IDictionary<string, FormulaFunctions.Function> _functionDictionary;
        /// <summary>
        /// 公式指标系统中内置函数的字典
        /// </summary>
        public IDictionary<string, FormulaFunctions.Function> FormulaFunctionsDictionary
        {
            get
            {
                if (null == _functionDictionary)
                {
                    _functionDictionary = new Dictionary<string, FormulaFunctions.Function>();
                    if (null != FormulaFunctions)
                    {
                        foreach (FormulaFunctions.Category category in FormulaFunctions.FunctionCategories)
                        {
                            foreach (FormulaFunctions.Function function in category.Functions)
                            {
                                if (!_functionDictionary.ContainsKey(function.Name))
                                {
                                    _functionDictionary.Add(function.Name, function);
                                }
                            }
                        }
                    }
                }
                return _functionDictionary;
            }
        }

        /// <summary>
        /// 用户关注的盘口异动类型
        /// </summary>
        public IList<ShortLineType> UserShortLineTypes
        {
            get { return _userShortLineTypes; }
            set { _userShortLineTypes = value; }
        }

        #region K线走势图像加载
        /// <summary>
        /// SingleInfoImage
        /// </summary>
        public Image SingleInfoImage;
        /// <summary>
        /// MultiInfoImage
        /// </summary>
        public Image MultiInfoImage;
        /// <summary>
        /// PlusNormalImage
        /// </summary>
        public Image PlusNormalImage;
        /// <summary>
        /// PlusSelectedImage
        /// </summary>
        public Image PlusSelectedImage;
        /// <summary>
        /// MinusNormalImage
        /// </summary>
        public Image MinusNormalImage;
        /// <summary>
        /// MinusSelectedImage
        /// </summary>
        public Image MinusSelectedImage;
        /// <summary>
        /// RenameImage
        /// </summary>
        public Image RenameImage;
        /// <summary>
        /// FuquanImage
        /// </summary>
        public Image FuquanImage;
        /// <summary>
        /// StockTagTitleImage
        /// </summary>
        public Image StockTagTitleImage;
        /// <summary>
        /// StockTag_1_Image
        /// </summary>
        public Image StockTag_1_Image;
        /// <summary>
        /// StockTag_2_Image
        /// </summary>
        public Image StockTag_2_Image;
        /// <summary>
        /// StockTag_3_Image
        /// </summary>
        public Image StockTag_3_Image;
        /// <summary>
        /// StockTag_4_Image
        /// </summary>
        public Image StockTag_4_Image;
        /// <summary>
        /// StockTag_5_Image
        /// </summary>
        public Image StockTag_5_Image;
        /// <summary>
        /// StockTag_6_Image
        /// </summary>
        public Image StockTag_6_Image;
        /// <summary>
        /// StockTag_7_Image
        /// </summary>
        public Image StockTag_7_Image;
        /// <summary>
        /// StockTag_8_Image
        /// </summary>
        public Image StockTag_8_Image;
        /// <summary>
        /// StockTag_9_Image
        /// </summary>
        public Image StockTag_9_Image;
        /// <summary>
        /// StockTag_Text_Image
        /// </summary>
        public Image StockTag_Text_Image;
        /// <summary>
        /// HelpBtn_01_Image
        /// </summary>
        public Image HelpBtn_01_Image;
        /// <summary>
        /// 自选股板块页面调整股票顺序的鼠标箭头
        /// </summary>
        public Image Arrow_SetUserStock_Image;


        #endregion

        #region 画线工具图像加载
        #region 趋势线
        //线段
        /// <summary>
        /// SegmentNormal
        /// </summary>
        public Image SegmentNormal;
        /// <summary>
        /// SegmentPass
        /// </summary>
        public Image SegmentPass;
        /// <summary>
        /// SegmentSelected
        /// </summary>
        public Image SegmentSelected;
        //趋势线
        /// <summary>
        /// TrendLineNormal
        /// </summary>
        public Image TrendLineNormal;
        /// <summary>
        /// TrendLinePass
        /// </summary>
        public Image TrendLinePass;
        /// <summary>
        /// TrendLineSelected
        /// </summary>
        public Image TrendLineSelected;
        //水平线
        /// <summary>
        /// HorizontalLineNormal
        /// </summary>
        public Image HorizontalLineNormal;
        /// <summary>
        /// HorizontalLinePass
        /// </summary>
        public Image HorizontalLinePass;
        /// <summary>
        /// HorizontalSelected
        /// </summary>
        public Image HorizontalSelected;
        //平行线
        /// <summary>
        /// ParallelNormal
        /// </summary>
        public Image ParallelNormal;
        /// <summary>
        /// ParallelPass
        /// </summary>
        public Image ParallelPass;
        /// <summary>
        /// ParallelSelected
        /// </summary>
        public Image ParallelSelected;
        //平行线段
        /// <summary>
        /// ParallelSegmentNormal
        /// </summary>
        public Image ParallelSegmentNormal;
        /// <summary>
        /// ParallelSegmentPass
        /// </summary>
        public Image ParallelSegmentPass;
        /// <summary>
        /// ParallelSegmentSelected
        /// </summary>
        public Image ParallelSegmentSelected;
        //三浪线
        /// <summary>
        /// ThreePointLineNormal
        /// </summary>
        public Image ThreePointLineNormal;
        /// <summary>
        /// ThreePointLinePass
        /// </summary>
        public Image ThreePointLinePass;
        /// <summary>
        /// ThreePointLineSelected
        /// </summary>
        public Image ThreePointLineSelected;
        //五浪线
        /// <summary>
        /// FivePointLineNormal
        /// </summary>
        public Image FivePointLineNormal;
        /// <summary>
        /// FivePointLinePass
        /// </summary>
        public Image FivePointLinePass;
        /// <summary>
        /// FivePointLineSelected
        /// </summary>
        public Image FivePointLineSelected;
        //八浪线
        /// <summary>
        /// EightPointLineNormal
        /// </summary>
        public Image EightPointLineNormal;
        /// <summary>
        /// EightPointLinePass
        /// </summary>
        public Image EightPointLinePass;
        /// <summary>
        /// EightPointLineSelected
        /// </summary>
        public Image EightPointLineSelected;
        //头肩线
        /// <summary>
        /// MountainLineNormal
        /// </summary>
        public Image MountainLineNormal;
        /// <summary>
        /// MountainLineNormal
        /// </summary>
        public Image MountainLinePass;
        /// <summary>
        /// MountainLineSelected
        /// </summary>
        public Image MountainLineSelected;
        //W头M底
        /// <summary>
        /// WLineNormal
        /// </summary>
        public Image WLineNormal;
        /// <summary>
        /// WLinePass
        /// </summary>
        public Image WLinePass;
        /// <summary>
        /// WLineSelected
        /// </summary>
        public Image WLineSelected;
        #endregion

        #region 工具
        /// <summary>
        /// DeleteAllNormal
        /// </summary>
        public Image DeleteAllNormal;
        /// <summary>
        /// DeleteAllPass
        /// </summary>
        public Image DeleteAllPass;
        /// <summary>
        /// DeleteAllSelected
        /// </summary>
        public Image DeleteAllSelected;

        /// <summary>
        /// DeleteNormal
        /// </summary>
        public Image DeleteNormal;
        /// <summary>
        /// DeletePass
        /// </summary>
        public Image DeletePass;
        /// <summary>
        /// DeleteSelected
        /// </summary>
        public Image DeleteSelected;

        /// <summary>
        /// DownArrowNormal
        /// </summary>
        public Image DownArrowNormal;
        /// <summary>
        /// DownArrowPass
        /// </summary>
        public Image DownArrowPass;
        /// <summary>
        /// DownArrowSelected
        /// </summary>
        public Image DownArrowSelected;

        /// <summary>
        /// FixNormal
        /// </summary>
        public Image FixNormal;
        /// <summary>
        /// FixPass
        /// </summary>
        public Image FixPass;
        /// <summary>
        /// FixSelected
        /// </summary>
        public Image FixSelected;

        /// <summary>
        /// MeasureNormal
        /// </summary>
        public Image MeasureNormal;
        /// <summary>
        /// MeasurePass
        /// </summary>
        public Image MeasurePass;
        /// <summary>
        /// MeasureSelected
        /// </summary>
        public Image MeasureSelected;

        /// <summary>
        /// MouseNormal
        /// </summary>
        public Image MouseNormal;
        /// <summary>
        /// MousePass
        /// </summary>
        public Image MousePass;
        /// <summary>
        /// MouseSelected
        /// </summary>
        public Image MouseSelected;

        /// <summary>
        /// TextNormal
        /// </summary>
        public Image TextNormal;
        /// <summary>
        /// TextPass
        /// </summary>
        public Image TextPass;
        /// <summary>
        /// TextSelected
        /// </summary>
        public Image TextSelected;

        /// <summary>
        /// UpArrpwNormal
        /// </summary>
        public Image UpArrpwNormal;
        /// <summary>
        /// UpArrpwPass
        /// </summary>
        public Image UpArrpwPass;
        /// <summary>
        /// UpArrpwSelected
        /// </summary>
        public Image UpArrpwSelected;
        /// <summary>
        /// Up
        /// </summary>
        public Image Up;
        /// <summary>
        /// Down
        /// </summary>
        public Image Down;
        #endregion

        #region 空间
        /// <summary>
        /// GoldenSectionNormal
        /// </summary>
        public Image GoldenSectionNormal;
        /// <summary>
        /// GoldenSectionPass
        /// </summary>
        public Image GoldenSectionPass;
        /// <summary>
        /// GoldenSectionSelected
        /// </summary>
        public Image GoldenSectionSelected;

        /// <summary>
        /// HorizonSpaceNormal
        /// </summary>
        public Image HorizonSpaceNormal;
        /// <summary>
        /// HorizonSpacePass
        /// </summary>
        public Image HorizonSpacePass;
        /// <summary>
        /// HorizonSpaceSelected
        /// </summary>
        public Image HorizonSpaceSelected;

        /// <summary>
        /// PercentageLineNormal
        /// </summary>
        public Image PercentageLineNormal;
        /// <summary>
        /// PercentageLinePass
        /// </summary>
        public Image PercentageLinePass;
        /// <summary>
        /// PercentageLineSelected
        /// </summary>
        public Image PercentageLineSelected;

        /// <summary>
        /// RangeRulerNormal
        /// </summary>
        public Image RangeRulerNormal;
        /// <summary>
        /// RangeRulerPass
        /// </summary>
        public Image RangeRulerPass;
        /// <summary>
        /// RangeRulerSelected
        /// </summary>
        public Image RangeRulerSelected;

        /// <summary>
        /// TwistRulerNormal
        /// </summary>
        public Image TwistRulerNormal;
        /// <summary>
        /// TwistRulerPass
        /// </summary>
        public Image TwistRulerPass;
        /// <summary>
        /// TwistRulerSelected
        /// </summary>
        public Image TwistRulerSelected;

        /// <summary>
        /// WaveRulerNormal
        /// </summary>
        public Image WaveRulerNormal;
        /// <summary>
        /// WaveRulerPass
        /// </summary>
        public Image WaveRulerPass;
        /// <summary>
        /// WaveRulerSelected
        /// </summary>
        public Image WaveRulerSelected;
        #endregion

        #region 时间
        /// <summary>
        /// FibonacciPeriodNormal
        /// </summary>
        public Image FibonacciPeriodNormal;
        /// <summary>
        /// FibonacciPeriodPass
        /// </summary>
        public Image FibonacciPeriodPass;
        /// <summary>
        /// FibonacciPeriodSelected
        /// </summary>
        public Image FibonacciPeriodSelected;

        /// <summary>
        /// FreeKarlFischerLineNormal
        /// </summary>
        public Image FreeKarlFischerLineNormal;
        /// <summary>
        /// FreeKarlFischerLinePass
        /// </summary>
        public Image FreeKarlFischerLinePass;
        /// <summary>
        /// FreeKarlFischerLineSelected
        /// </summary>
        public Image FreeKarlFischerLineSelected;

        /// <summary>
        /// PeriodLineNormal
        /// </summary>
        public Image PeriodLineNormal;
        /// <summary>
        /// PeriodLinePass
        /// </summary>
        public Image PeriodLinePass;
        /// <summary>
        /// PeriodLineSlected
        /// </summary>
        public Image PeriodLineSlected;

        /// <summary>
        /// SymmetricalAngleNormal
        /// </summary>
        public Image SymmetricalAngleNormal;
        /// <summary>
        /// SymmetricalAnglePass
        /// </summary>
        public Image SymmetricalAnglePass;
        /// <summary>
        /// SymmetricalAngleSelected
        /// </summary>
        public Image SymmetricalAngleSelected;

        /// <summary>
        /// SymmetricalLineNormal
        /// </summary>
        public Image SymmetricalLineNormal;
        /// <summary>
        /// SymmetricalLinePass
        /// </summary>
        public Image SymmetricalLinePass;
        /// <summary>
        /// SymmetricalLineSelected
        /// </summary>
        public Image SymmetricalLineSelected;

        /// <summary>
        /// TimeRulerNormal
        /// </summary>
        public Image TimeRulerNormal;
        /// <summary>
        /// TimeRulerPass
        /// </summary>
        public Image TimeRulerPass;
        /// <summary>
        /// TimeRulerSelected
        /// </summary>
        public Image TimeRulerSelected;

        #endregion

        #region 时空
        /// <summary>
        /// ArcNormal
        /// </summary>
        public Image ArcNormal;
        /// <summary>
        /// ArcPass
        /// </summary>
        public Image ArcPass;
        /// <summary>
        /// ArcSelected
        /// </summary>
        public Image ArcSelected;

        /// <summary>
        /// BoxLineNormal
        /// </summary>
        public Image BoxLineNormal;
        /// <summary>
        /// BoxLinePass
        /// </summary>
        public Image BoxLinePass;
        /// <summary>
        /// BoxLineSelected
        /// </summary>
        public Image BoxLineSelected;

        /// <summary>
        /// GannAngleNormal
        /// </summary>
        public Image GannAngleNormal;
        /// <summary>
        /// GannAnglePass
        /// </summary>
        public Image GannAnglePass;
        /// <summary>
        /// GannAngleSelected
        /// </summary>
        public Image GannAngleSelected;

        /// <summary>
        /// LinearHuiGuiDaiNormal
        /// </summary>
        public Image LinearHuiGuiDaiNormal;
        /// <summary>
        /// LinearHuiGuiDaiPass
        /// </summary>
        public Image LinearHuiGuiDaiPass;
        /// <summary>
        /// LinearHuiGuiDaiSelected
        /// </summary>
        public Image LinearHuiGuiDaiSelected;

        /// <summary>
        /// LinearHuiGuiXianNoraml
        /// </summary>
        public Image LinearHuiGuiXianNoraml;
        /// <summary>
        /// LinearHuiGuiXianPass
        /// </summary>
        public Image LinearHuiGuiXianPass;
        /// <summary>
        /// LinearHuiGuiXianSelected
        /// </summary>
        public Image LinearHuiGuiXianSelected;

        /// <summary>
        /// RectangleNormal
        /// </summary>
        public Image RectangleNormal;
        /// <summary>
        /// RectanglePass
        /// </summary>
        public Image RectanglePass;
        /// <summary>
        /// RectangleSelected
        /// </summary>
        public Image RectangleSelected;

        /// <summary>
        /// RegressionChannelNormal
        /// </summary>
        public Image RegressionChannelNormal;
        /// <summary>
        /// RegressionChannelPass
        /// </summary>
        public Image RegressionChannelPass;
        /// <summary>
        /// RegressionChannelSelected
        /// </summary>
        public Image RegressionChannelSelected;

        /// <summary>
        /// ResistanceSpeedCurveNormal
        /// </summary>
        public Image ResistanceSpeedCurveNormal;
        /// <summary>
        /// ResistanceSpeedCurvePass
        /// </summary>
        public Image ResistanceSpeedCurvePass;
        /// <summary>
        /// ResistanceSpeedCurveSelected
        /// </summary>
        public Image ResistanceSpeedCurveSelected;

        /// <summary>
        /// RhomboidNormal
        /// </summary>
        public Image RhomboidNormal;
        /// <summary>
        /// RhomboidPass
        /// </summary>
        public Image RhomboidPass;
        /// <summary>
        /// RhomboidSelected
        /// </summary>
        public Image RhomboidSelected;

        /// <summary>
        /// TriangleNormal
        /// </summary>
        public Image TriangleNormal;
        /// <summary>
        /// TrianglePass
        /// </summary>
        public Image TrianglePass;
        /// <summary>
        /// TriangleSelected
        /// </summary>
        public Image TriangleSelected;
        #endregion

        #region 垂直Tab图片
        /// <summary>
        /// Imagelist
        /// </summary>
        public List<Image> Imagelist;
        #endregion
        #endregion
        /// <summary>
        /// 价格上涨图片
        /// </summary>
        public Image PriceUpImg
        {
            get { return _priceUpImg; }
            set { _priceUpImg = value; }
        }

        /// <summary>
        /// 价格下跌图片
        /// </summary>
        public Image PriceDownImg
        {
            get { return _priceDownImg; }
            set { _priceDownImg = value; }
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabCommomImage0;
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabCommomImage1;
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabCommomImage2;
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabCommomImage3;

        /// <summary>
        /// 
        /// </summary>
        public Image CustTabSelectedImage0;
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabSelectedImage1;
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabSelectedImage2;
        /// <summary>
        /// 
        /// </summary>
        public Image CustTabSelectedImage3;

        #endregion
        /// <summary>
        /// 初始化自选股顺序
        /// </summary>
        public Image InitCustBlockOrderImg
        {
            get { return _initCustBlockOrderImg; }
            set { _initCustBlockOrderImg = value; }
        }

        /// <summary>
        /// 初始化自选股顺序（鼠标移上）
        /// </summary>
        public Image InitCustBlockOrder_MouseOnImg
        {
            get { return _initCustBlockOrderMouseOnImg; }
            set { _initCustBlockOrderMouseOnImg = value; }
        }

        /// <summary>
        /// 个股上一个股箭头
        /// </summary>
        public Image TopBannerLeftArrow
        {
            get { return _topBannerLeftArrow; }
            set { _topBannerLeftArrow = value; }
        }

        public Image TopBannerLeftArrowMouseOn
        {
            get { return _topBannerLeftArrowMouseOn; }
            set { _topBannerLeftArrowMouseOn = value; }
        }

        public Image TopBannerLeftArrowMouseDown
        {
            get { return _topBannerLeftArrowMouseDown; }
            set { _topBannerLeftArrowMouseDown = value; }
        }

        /// <summary>
        /// 个股下一个股箭头
        /// </summary>
        public Image TopBannerRightArrow
        {
            get { return _topBannerRightArrow; }
            set { _topBannerRightArrow = value; }
        }

        public Image TopBannerRightArrowMouseOn
        {
            get { return _topBannerRightArrowMouseOn; }
            set { _topBannerRightArrowMouseOn = value; }
        }

        public Image TopBannerRightArrowMouseDown
        {
            get { return _topBannerRightArrowMouseDown; }
            set { _topBannerRightArrowMouseDown = value; }
        }

        /// <summary>
        /// 个股B股角标
        /// </summary>
        public Image TopBannerBStock
        {
            get { return _topBannerBStock; }
            set { _topBannerBStock = value; }
        }

        public Image TopBannerBStockMouseOn
        {
            get { return _topBannerBStockMouseOn; }
            set { _topBannerBStockMouseOn = value; }
        }

        public Image TopBannerBStockMouseDown
        {
            get { return _topBannerBStockMouseDown; }
            set { _topBannerBStockMouseDown = value; }
        }

        /// <summary>
        /// 个股H股角标
        /// </summary>
        public Image TopBannerHStock
        {
            get { return _topBannerHStock; }
            set { _topBannerHStock = value; }
        }

        public Image TopBannerHStockMouseOn
        {
            get { return _topBannerHStockMouseOn; }
            set { _topBannerHStockMouseOn = value; }
        }

        public Image TopBannerHStockMouseDown
        {
            get { return _topBannerHStockMouseDown; }
            set { _topBannerHStockMouseDown = value; }
        }

        /// <summary>
        /// 个股同时有HB股时B股角标
        /// </summary>
        public Image TopBannerHBStockB
        {
            get { return _topBannerHBStockB; }
            set { _topBannerHBStockB = value; }
        }

        public Image TopBannerHBStockBMouseOn
        {
            get { return _topBannerHBStockBMouseOn; }
            set { _topBannerHBStockBMouseOn = value; }
        }

        public Image TopBannerHBStockBMouseDown
        {
            get { return _topBannerHBStockBMouseDown; }
            set { _topBannerHBStockBMouseDown = value; }
        }

        /// <summary>
        /// 个股同时有HB股时H股角标
        /// </summary>
        public Image TopBannerHBStockH
        {
            get { return _topBannerHBStockH; }
            set { _topBannerHBStockH = value; }
        }

        public Image TopBannerHBStockHMouseOn
        {
            get { return _topBannerHBStockHMouseOn; }
            set { _topBannerHBStockHMouseOn = value; }
        }

        public Image TopBannerHBStockHMouseDown
        {
            get { return _topBannerHBStockHMouseDown; }
            set { _topBannerHBStockHMouseDown = value; }
        }

        /// <summary>
        /// 个股加入自选股
        /// </summary>
        public Image TopBannerAddUserStock
        {
            get { return _topBannerAddUserStock; }
            set { _topBannerAddUserStock = value; }
        }

        public Image TopBannerAddUserStockMouseOn
        {
            get { return _topBannerAddUserStockMouseOn; }
            set { _topBannerAddUserStockMouseOn = value; }
        }

        public Image TopBannerAddUserStockMouseDown
        {
            get { return _topBannerAddUserStockMouseDown; }
            set { _topBannerAddUserStockMouseDown = value; }
        }

        /// <summary>
        /// 个股移除自选股
        /// </summary>
        public Image TopBannerRemoveUserStock
        {
            get { return _topBannerRemoveUserStock; }
            set { _topBannerRemoveUserStock = value; }
        }

        public Image TopBannerRemoveUserStockMouseOn
        {
            get { return _topBannerRemoveUserStockMouseOn; }
            set { _topBannerRemoveUserStockMouseOn = value; }
        }

        public Image TopBannerRemoveUserStockMouseDown
        {
            get { return _topBannerRemoveUserStockMouseDown; }
            set { _topBannerRemoveUserStockMouseDown = value; }
        }

        /// <summary>
        /// 个股菜单展开
        /// </summary>
        public Image TopBannerMenuExpand
        {
            get { return _topBannerMenuExpand; }
            set { _topBannerMenuExpand = value; }
        }

        public Image TopBannerMenuExpandOn
        {
            get { return _topBannerMenuExpandOn; }
            set { _topBannerMenuExpandOn = value; }
        }

        /// <summary>
        /// 全景图自选股箭头
        /// </summary>
        public Image FullViewUserStockArrow
        {
            get { return _fullViewUserStockArrow; }
            set { _fullViewUserStockArrow = value; }
        }

        /// <summary>
        /// 股票说明
        /// </summary>
        public Image StockDescription
        {
            get { return _stockDescription; }
            set { _stockDescription = value; }
        }

        /// <summary>
        /// 新闻项的圆点
        /// </summary>
        public Image StockItemDot
        {
            get { return _stockItemDot; }
            set { _stockItemDot = value; }
        }
        /// <summary>
        /// 加号按钮
        /// </summary>
        public Image ImgAdd
        {
            get { return _imgAdd; }
            set { _imgAdd = value; }
        }
        /// <summary>
        /// 加号按钮选中
        /// </summary>
        public Image ImgAdd_On
        {
            get { return _imgAdd_On; }
            set { _imgAdd_On = value; }
        }

        /// <summary>
        /// 委托队列挂单情况
        /// </summary>
        public Image ImgOrderDetailNormal
        {
            get { return _imgOrderDetailNormal; }
            set { this._imgOrderDetailNormal = value; }
        }

        /// <summary>
        /// 委托队列有大卖单
        /// </summary>
        public Image ImgOrderDetailBigSell
        {
            get { return _imgOrderDetailBigSell; }
            set { this._imgOrderDetailBigSell = value; }
        }

        /// <summary>
        /// 委托队列有大买单
        /// </summary>
        public Image ImgOrderDetailBigBuy
        {
            get { return _imgOrderDetailBigBuy; }
            set { this._imgOrderDetailBigBuy = value; }
        }

        /// <summary>
        /// 关闭的图片
        /// </summary>
        public Image ImgClose
        {
            get { return _imgClose; }
            set { this._imgClose = value; }
        }

        /// <summary>
        /// lev2盘口帮助图片
        /// </summary>
        public Image ImgLev2Help
        {
            get { return _imgLev2Help; }
            set { this._imgLev2Help = value; }
        }

        /// <summary>
        /// 股吧数据类型图标
        /// </summary>
        public Image ImgGubaData {
            get { return _gubaData; }
            set { _gubaData = value; }
        }

        /// <summary>
        /// 股吧置顶图标
        /// </summary>
        public Image ImgGubaFix {
            get { return _gubaFix; }
            set { this._gubaFix = value; }
        }

        /// <summary>
        /// 股吧新闻图标
        /// </summary>
        public Image ImgGubaNews {
            get { return _gubaNews; }
            set { this._gubaNews = value; }
        }

        /// <summary>
        /// 股吧通知图标
        /// </summary>
        public Image ImgGubaNotice {
            get { return _gubaNotice; }
            set { this._gubaNotice = value; }
        }

        /// <summary>
        /// 股吧研报图标
        /// </summary>
        public Image ImgGubaResearch {
            get { return _gubaResearch; }
            set { this._gubaResearch = value; }
        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDataTable()
        {
            InitDataTable(false);
        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDataTable(bool isDepedence)
        {
            _dataTableCollecion = new Dictionary<string, DataTableBase>();

            DetailDataTable detailDataTable;
            if (isDepedence)
            {
                detailDataTable = new DetailDataTable(true);
                detailDataTable.Dc = this;
            }
            else
            {
                detailDataTable = new DetailDataTable();
                detailDataTable.Dc = this;
            }
            SetDefaultCustomBlock();
            Debug.Print("detailTable end");

            TrendDataTable trendDataTable = new TrendDataTable();
            trendDataTable.Dc = this;
            KLineDataTable klineDataTable = new KLineDataTable();
            klineDataTable.Dc = this;
            DealDataTable dealDataTable = new DealDataTable();
            dealDataTable.Dc = this;
            CapitalFlowTable capitalFlowDataTable = new CapitalFlowTable();
            capitalFlowDataTable.Dc = this;
            PriceStatusDataTable priceStatusDataTable = new PriceStatusDataTable();
            priceStatusDataTable.Dc = this;
            RankDataTable rankDataTable = new RankDataTable();
            rankDataTable.Dc = this;
            F10DataTable f10DataTable = new F10DataTable();
            f10DataTable.Dc = this;
            ProfitForecastDataTable profitDataTable = new ProfitForecastDataTable();
            profitDataTable.Dc = this;
            OrgRateDataTable orgRateDataTable = new OrgRateDataTable();
            orgRateDataTable.Dc = this;
            TickDataTable tickDataTable = new TickDataTable();
            tickDataTable.Dc = this;
            OrderDetailDataTable orderDetailDataTable = new OrderDetailDataTable();
            orderDetailDataTable.Dc = this;
            OrderQueueDataTable orderQueueDataTable = new OrderQueueDataTable();
            orderQueueDataTable.Dc = this;
            ShortLineStrategyDataTable shortlineDataTable = new ShortLineStrategyDataTable();
            shortlineDataTable.Dc = this;
            ContributionDataTable contributionDataTable = new ContributionDataTable();
            contributionDataTable.Dc = this;
            News24hDataTable news24hDataTable = new News24hDataTable();
            news24hDataTable.Dc = this;
            DepthAnalyseDataTable depthAnalyseDataTable = new DepthAnalyseDataTable();
            depthAnalyseDataTable.Dc = this;
            TradeDateDataTable tradeDateDataTable = new TradeDateDataTable();
            tradeDateDataTable.Dc = this;
            DetailLev2DataTable detailLev2DataTable = new DetailLev2DataTable();
            detailLev2DataTable.Dc = this;
            ResearchReportDatatable reschRptDataTable = new ResearchReportDatatable();
            reschRptDataTable.Dc = this;
            IndicatorDataTable indicatorDataTable = new IndicatorDataTable();

            _dataTableCollecion.Add("detail", detailDataTable);
            _dataTableCollecion.Add("trend", trendDataTable);
            _dataTableCollecion.Add("kline", klineDataTable);
            _dataTableCollecion.Add("deal", dealDataTable);
            _dataTableCollecion.Add("capitalflow", capitalFlowDataTable);
            _dataTableCollecion.Add("pricestatus", priceStatusDataTable);
            _dataTableCollecion.Add("rank", rankDataTable);
            _dataTableCollecion.Add("f10", f10DataTable);
            _dataTableCollecion.Add("profit", profitDataTable);
            _dataTableCollecion.Add("orgrate", orgRateDataTable);
            _dataTableCollecion.Add("tick", tickDataTable);
            _dataTableCollecion.Add("orderdetail", orderDetailDataTable);
            _dataTableCollecion.Add("orderqueue", orderQueueDataTable);
            _dataTableCollecion.Add("shortline", shortlineDataTable);
            _dataTableCollecion.Add("contribution", contributionDataTable);
            _dataTableCollecion.Add("news24h", news24hDataTable);
            _dataTableCollecion.Add("depthanalyse", depthAnalyseDataTable);
            _dataTableCollecion.Add("date", tradeDateDataTable);
            _dataTableCollecion.Add("detaillev2", detailLev2DataTable);
            _dataTableCollecion.Add("reschRpt", reschRptDataTable);
            _dataTableCollecion.Add("indicator", indicatorDataTable);
        }

        ///// <summary>
        ///// 清空内存中实时数据，初始化消息时用
        ///// </summary>
        //public void ClearRealTimeDataTable(InitOrgStatus status)
        //{
        //    _dataTableCollecion["detail"].ClearData(status);
        //    _dataTableCollecion["trend"].ClearData(status);
        //    _dataTableCollecion["deal"].ClearData(status);
        //    _dataTableCollecion["capitalflow"].ClearData(status);
        //    _dataTableCollecion["pricestatus"].ClearData(status);
        //    _dataTableCollecion["rank"].ClearData(status);
        //    _dataTableCollecion["profit"].ClearData(status);
        //    _dataTableCollecion["orgrate"].ClearData(status);
        //    _dataTableCollecion["tick"].ClearData(status);
        //    _dataTableCollecion["orderdetail"].ClearData(status);
        //    _dataTableCollecion["orderqueue"].ClearData(status);
        //    _dataTableCollecion["shortline"].ClearData(status);
        //    _dataTableCollecion["contribution"].ClearData(status);
        //    _dataTableCollecion["detaillev2"].ClearData(status);
        //}

        /// <summary>
        /// 各市场初始化清空数据
        /// </summary>
        /// <param name="status"></param>
        public void ClearDataTable(InitOrgStatus status)
        {
            if (_dataTableCollecion != null)
            {
                foreach (KeyValuePair<string, DataTableBase> table in _dataTableCollecion)
                {
                    table.Value.ClearData(status);
                }
            }
        }
       
        #endregion

        #region 网络层消息

        private void SetMarketTime()
        {
            int tmpLastTradeTime = 0;
            int shDate = GetTradeDate(MarketType.SZALev1);
            if (shDate == 0)
            {
                foreach (KeyValuePair<MarketType, MarketTime> mt in TimeUtilities.MarketStatus)
                    mt.Value.LastTradeTime = ServerTime;
                ((TradeDateDataTable)_dataTableCollecion["date"]).SetSHTradeDate(ServerDate);
            }
            else
            {
                foreach (KeyValuePair<MarketType, MarketTime> mt in TimeUtilities.MarketStatus)
                {
                    if (ServerDate > GetTradeDate(mt.Key))
                    {
                        int hour = (ServerTime + 240000) / 10000;
                        int mint = (ServerTime + 240000) % 10000 / 100;
                        int delayHour = mt.Value.DelayTime / 10000;
                        int delayMint = mt.Value.DelayTime % 10000 / 100;
                        int totalMints = (hour - delayHour) * 60 + (mint - delayMint);
                        tmpLastTradeTime = totalMints / 60 * 10000 + (totalMints % 60) * 100;
                        if (mt.Key == MarketType.IB)
                            Debug.Print("!!!!!!!!!!!!!!!!!!!!" + mt.Key.ToString() + " LastTradeTime + 24");
                    }
                    else if (ServerDate == GetTradeDate(mt.Key))
                    {
                        int hour = (ServerTime) / 10000;
                        int mint = (ServerTime) % 10000 / 100;
                        int delayHour = mt.Value.DelayTime / 10000;
                        int delayMint = mt.Value.DelayTime % 10000 / 100;
                        int totalMints = (hour - delayHour) * 60 + (mint - delayMint);
                        tmpLastTradeTime = totalMints / 60 * 10000 + (totalMints % 60) * 100;
                    }
                    mt.Value.LastTradeTime = tmpLastTradeTime;
                }
            }

            TimeUtilities.TradeDate = GetTradeDate(MarketType.SHALev1);
            TimeUtilities.ServerDate = ServerDate;
            TimeUtilities.ServerTime = ServerTime;
        }

        public delegate void KlineRecDataHandler(object sender, CMRecvDataEventArgs e);

        public event KlineRecDataHandler KlineRecDataEvents;

        void _cm_DoCMReceiveData(object sender, CMRecvDataEventArgs e)
        {
            if (e.DataPacket is RealTimeDataPacket)
            {
                switch (((RealTimeDataPacket)e.DataPacket).RequestType)
                {
                    case FuncTypeRealTime.Heart:
                        if (e.ServiceType == TcpService.SSHQ)
                        {
                            ServerDate = ((ResHeartDataPacket)e.DataPacket).Date;
                            ServerTime = ((ResHeartDataPacket)e.DataPacket).Time;

                            SetMarketTime();
                        }
                        break;
                    case FuncTypeRealTime.Init:
                        this._dataTableCollecion["date"].SetData(e.DataPacket);
                        SendLogonCft();
                        break;
                    case FuncTypeRealTime.InitLogon:
                        if (e.DataPacket.IsResult)
                        {
                            this.SendHeart();
                            //QuoteStart.SendSSHQ();
                        }
                        break;
                    case FuncTypeRealTime.StockDetail:
                    case FuncTypeRealTime.StockDetailLev2:
                    case FuncTypeRealTime.IndexDetail:
                    case FuncTypeRealTime.IndexFuturesDetail:
                    case FuncTypeRealTime.OceanRecord:
                    case FuncTypeRealTime.NOrderStockDetailLevel2:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        _dataTableCollecion["trend"].SetData(e.DataPacket);
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        _dataTableCollecion["profit"].SetData(e.DataPacket);
                        _dataTableCollecion["detaillev2"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.StockTrend:
                    case FuncTypeRealTime.StockTrendInOutDiff:
                    case FuncTypeRealTime.StockTrendAskBid:
                    case FuncTypeRealTime.HisTrend:
                    case FuncTypeRealTime.IndexFuturesTrend:
                    case FuncTypeRealTime.OceanTrend:
                    case FuncTypeRealTime.RedGreen:
                    case FuncTypeRealTime.TrendCapitalFlow:
                        _dataTableCollecion["trend"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.HisKLine:
                    case FuncTypeRealTime.MinKLine:
                    case FuncTypeRealTime.CapitalFlowDay:
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.SectorQuoteReport:
                    case FuncTypeRealTime.BlockQuoteReport:
                    case FuncTypeRealTime.BlockIndexReport:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.DealSubscribe:
                    case FuncTypeRealTime.DealRequest:
                        _dataTableCollecion["deal"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.CapitalFlow:
                        _dataTableCollecion["capitalflow"].SetData(e.DataPacket);
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.PriceStatus:
                        _dataTableCollecion["pricestatus"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.Rank:
                        _dataTableCollecion["rank"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.ReqF10:
                        _dataTableCollecion["f10"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.StockDict:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.TickTrade:
                        _dataTableCollecion["tick"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.OrderDetail:
                        _dataTableCollecion["orderdetail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.OrderQueue:
                        _dataTableCollecion["orderqueue"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.ShortLineStrategy:
                        _dataTableCollecion["shortline"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.LimitedPrice:
                        _dataTableCollecion["limitedprice"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.ContributionStock:
                    case FuncTypeRealTime.ContributionBlock:
                        _dataTableCollecion["contribution"].SetData(e.DataPacket);
                        break;
                    case FuncTypeRealTime.AllOrderStockDetailLevel2:
                    case FuncTypeRealTime.StockDetailOrderQueue:
                    case FuncTypeRealTime.StatisticsAnalysis:
                        _dataTableCollecion["detaillev2"].SetData(e.DataPacket);
                        break;
                }
            }
            else if (e.DataPacket is InfoDataPacket)
            {
                switch (((InfoDataPacket)e.DataPacket).RequestType)
                {
                    case FuncTypeInfo.Block:
                        _dataTableCollecion["block"].SetData(e.DataPacket);
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfo.NewsReport:
                    case FuncTypeInfo.Finance:
                    case FuncTypeInfo.CustomStockNewsReport:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfo.ProfitForecast:
                        _dataTableCollecion["profit"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfo.OrgRate:
                        _dataTableCollecion["orgrate"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfo.News24:
                        _dataTableCollecion["news24h"].SetData(e.DataPacket);
                        break;

                }
            }
            else if (e.DataPacket is OrgDataPacket)
            {
                switch (((OrgDataPacket)e.DataPacket).RequestType)
                {
                    case FuncTypeOrg.BlockIndexReport:
                    case FuncTypeOrg.BlockReport:
                    case FuncTypeOrg.BlockStockReport:
                    case FuncTypeOrg.HKStockReport:
                    case FuncTypeOrg.BondStockReport:
                    case FuncTypeOrg.FundStockReport:
                    case FuncTypeOrg.FuturesStockReport:
                    case FuncTypeOrg.CustomReport:
                    case FuncTypeOrg.GlobalIndexReport:
                    case FuncTypeOrg.EmIndexReport:
                    case FuncTypeOrg.IndexFuturesReport:
                    case FuncTypeOrg.RateReport:
                    case FuncTypeOrg.FinanceReport:
                    case FuncTypeOrg.FinanceOrg:
                    case FuncTypeOrg.DDEReport:
                    case FuncTypeOrg.NetInFlowReport:
                    case FuncTypeOrg.CapitalFlowReport:
                    case FuncTypeOrg.ProfitForecastReport:
                    case FuncTypeOrg.CustomDDEReport:
                    case FuncTypeOrg.CustomNetInFlowReport:
                    case FuncTypeOrg.CustomCapitalFlowReport:
                    case FuncTypeOrg.CustomProfitForecastReport:
                    case FuncTypeOrg.CustomFinanceStockReport:
                    case FuncTypeOrg.IndexStatic:
                    case FuncTypeOrg.ForexReport:
                    case FuncTypeOrg.USStockReport:
                    case FuncTypeOrg.OSFuturesReport:
                    case FuncTypeOrg.OSFuturesReportNew:
                    case FuncTypeOrg.FinanceStockReport:
                    case FuncTypeOrg.OsFuturesLMEReport:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.EMIndexDetail:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        _dataTableCollecion["trend"].SetData(e.DataPacket);
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        break;

                    case FuncTypeOrg.BondPublicOpeartion:
                        _dataTableCollecion["reschRpt"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.USStockDetail:
                    case FuncTypeOrg.CNIndexDetail:
                    case FuncTypeOrg.CSIIndexDetail:
                    case FuncTypeOrg.CSIndexDetail:
                    case FuncTypeOrg.ForexDetail:
                    case FuncTypeOrg.InterBankDetail:
                    case FuncTypeOrg.InterBankRepurchaseDetail:
                    case FuncTypeOrg.OSFuturesDetail:
                    case FuncTypeOrg.RateSwapDetail:
                    case FuncTypeOrg.ShiborDetail:
                        _dataTableCollecion["trend"].SetData(e.DataPacket);
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.InitOrg:
                        try
                        {
                            if (e.DataPacket is ResInitOrgDataPacket)
                            {
                                ResInitOrgDataPacket packet = e.DataPacket as ResInitOrgDataPacket;
                                switch (packet.InitStatus)
                                {
                                    case InitOrgStatus.StaticData:
                                        SendInitRequest();
                                        break;
                                    default:
                                        ClearDataTable(packet.InitStatus);
                                        SetIndexData();
                                        ReadMainIndex();
                                        LoadQuoteData(true);
                                        ReadIndexPublishCode();
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtilities.LogMessage("初始化清空数据出错" + ex.Message);
                        }

                        break;
                    case FuncTypeOrg.TradeDate:
                        _dataTableCollecion["date"].SetData(e.DataPacket);
                        SetMarketTime();
                        break;
                    case FuncTypeOrg.HisKLineOrg:
                    case FuncTypeOrg.MinKLineOrg:
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        break;
                    case FuncTypeOrg.DivideRightOrg:
                    case FuncTypeOrg.FundKlineAfterDivide:
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.TrendOrg:
                    case FuncTypeOrg.TrendOrgDP:
                        _dataTableCollecion["trend"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.DepthAnalyse:
                        _dataTableCollecion["depthanalyse"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.Rank:
                    case FuncTypeOrg.NetInFlowRank:
                        _dataTableCollecion["rank"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.OSFuturesLMEDeal:
                    case FuncTypeOrg.LowFrequencyTBY:
                    case FuncTypeOrg.BankBondReport:
                    case FuncTypeOrg.ShiborReport:
                        _dataTableCollecion["deal"].SetData(e.DataPacket);
                        break;
                    case FuncTypeOrg.OSFuturesLMEDetail:
                        _dataTableCollecion["trend"].SetData(e.DataPacket);
                        //设置k线数据
                        if (KlineRecDataEvents != null)
                            KlineRecDataEvents(sender, e);
                        else
                            _dataTableCollecion["kline"].SetData(e.DataPacket);
                        break;

                    case FuncTypeOrg.NewProfitForcast:
                        _dataTableCollecion["profit"].SetData(e.DataPacket);
                        break;


                }
            }
            else if (e.DataPacket is InfoOrgBaseDataPacket)
            {
                switch (((InfoOrgBaseDataPacket)e.DataPacket).RequestId)
                {
                    case FuncTypeInfoOrg.InfoMineOrg:
                    case FuncTypeInfoOrg.NewInfoMineOrg:
                    case FuncTypeInfoOrg.InfoMineOrgByIds:
                        _dataTableCollecion["detail"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfoOrg.News24H:
                    case FuncTypeInfoOrg.NewsFlash:
                    case FuncTypeInfoOrg.ImportantNews:
                        _dataTableCollecion["news24h"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfoOrg.ProfitForecast:
                        _dataTableCollecion["profit"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfoOrg.OrgRate:
                        _dataTableCollecion["orgrate"].SetData(e.DataPacket);
                        break;
                    case FuncTypeInfoOrg.ResearchReport:
                        _dataTableCollecion["reschRpt"].SetData(e.DataPacket);
                        break;
                }
            }
            else if (e.DataPacket is IndicatorDataPacket)
            {
                switch (((IndicatorDataPacket)e.DataPacket).RequestId)
                {
                    case IndicateRequestType.LeftIndicatorsReport:
                    case IndicateRequestType.RightIndicatorsReport:
                    case IndicateRequestType.IndicatorValuesReport:
                        _dataTableCollecion["indicator"].SetData(e.DataPacket);
                        break;
                }
            }
        }

        public void SetKLinePacketData(DataPacket datapacket)
        {
            ((KLineDataTable)_dataTableCollecion["kline"]).SetData(datapacket);
        }

        #endregion

        #region 定时请求数据

        private Timer _timer;
        private static int Interval = 60000;

        //void _timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    DoTimerElapsed();
        //}


        //public void TimerStart()
        //{
        //    _timer.Start();

        //}
        /// <summary>
        /// DoTimerElapsed
        /// </summary>
        public void DoTimerElapsed()
        {
            try
            {
                //实时，历史服务器发心跳包
                ReqHeartDataPacket packet = new ReqHeartDataPacket();
                _cm.Request(packet);

                //资讯服务器心跳包
                ReqInfoHeart infoPacket = new ReqInfoHeart();
                _cm.Request(infoPacket);


            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
                throw;
            }


        }

        #endregion

        #region 为界面层提供数据的方法
        /// <summary>
        /// 自定义指数添加K线数据(慎用)
        /// </summary>
        /// <param name="data"></param>
        public void SetKLineData(OneStockKLineDataRec data)
        {
            ((KLineDataTable)_dataTableCollecion["kline"]).AddKLineData(data);
        }

        /// <summary>
        /// 获得code所在的列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="blockId"></param>
        /// <param name="codeList"></param>
        /// <param name="index"></param>
        public void GetCurrentCodeIndex(int code, string blockId, out List<int> codeList, out int index)
        {
            codeList = new List<int>();
            index = 0;
            //DateTime dt = DateTime.Now;
            //GetBlockStockListOrg(blockId, GetCurrentCodeIndexCallBack);
            //index = codeList.IndexOf(code);
            //try
            //{
            //    if (index < 0)
            //    {
            //        for (int i = 0; i < DefaultBlockIdList.Count; i++)
            //        {
            //            codeList = GetBlockStockListOrg(DefaultBlockIdList[i]);
            //            index = codeList.IndexOf(code);
            //            if (index >= 0)
            //                return;
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    LogUtilities.LogMessage(e.Message);
            //}
            //TimeSpan ts = DateTime.Now - dt;
            //Debug.Print("GetCurrentCodeIndex = " + ts.TotalMilliseconds);
        }


        ///// <summary>
        ///// SHSZ最近一个交易日
        ///// </summary>
        //public int TradeDate ;

        /// <summary>
        /// 服务器日期
        /// </summary>
        public int ServerDate
        {
            get { return _serverDate; }
            set { _serverDate = value; }
        }

        /// <summary>
        /// 服务器的时间
        /// </summary>
        public int ServerTime
        {
            get { return _serverTime; }
            set { _serverTime = value; }
        }

        ///// <summary>
        ///// 历史交易日（10个）
        ///// </summary>
        //public List<int> HisTradeDate ;


        #region 交易日
        /// <summary>
        /// 获取一个市场的历史交易日
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        public List<int> GetHisTradeDate(MarketType mt)
        {
            TradeDateDataTable tradeDataTable = (TradeDateDataTable)_dataTableCollecion["date"];
            List<int> result = null;
            if (tradeDataTable.MarketTradeDate != null)
            {
                switch (mt)
                {
                    case MarketType.HK:
                    case MarketType.HSINDEX:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.HK, out result);
                        break;
                    case MarketType.IB:
                    case MarketType.InterBankRepurchase:
                    case MarketType.RateSwap:
                    case MarketType.Chibor:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.IB, out result);
                        break;
                    case MarketType.US:
                    case MarketType.NasdaqIndex:
                    case MarketType.DutchAEXIndex:
                    case MarketType.AustriaIndex:
                    case MarketType.NorwayIndex:
                    case MarketType.RussiaIndex:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.US, out result);
                        break;
                    case MarketType.OSFutures:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.OSFutures, out result);
                        break;
                    case MarketType.OSFuturesCBOT:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.OSFuturesCBOT, out result);
                        break;
                    case MarketType.OSFuturesSGX:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.OSFuturesSGX, out result);
                        break;
                    case MarketType.OSFuturesLMEElec:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.OSFuturesLMEElec, out result);
                        break;
                    case MarketType.OSFuturesLMEVenue:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.OSFuturesLMEVenue, out result);
                        break;
                    case MarketType.ForexSpot:
                    case MarketType.ForexNonSpot:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.ForexSpot, out result);
                        break;
                    default:
                        tradeDataTable.MarketTradeDate.TryGetValue(MarketType.SHALev1, out result);
                        break;
                }
            }
            if (result == null)
                result = new List<int>(0);
            return result;
        }

        /// <summary>
        /// 获取一只股票的历史交易日
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<int> GetHisTradeDate(int code)
        {
            return GetHisTradeDate(GetMarketType(code));
        }

        /// <summary>
        /// 获取一个市场的最近一个交易日
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        public int GetTradeDate(MarketType mt)
        {
            List<int> temp = GetHisTradeDate(mt);
            if (temp != null && temp.Count > 0)
                return temp[0];
            return 0;
        }

        /// <summary>
        /// 获取一只股票最近一个交易日
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetTradeDate(int code)
        {
            return GetTradeDate(GetMarketType(code));
        }
        #endregion


        #region 获取字段的值
        /// <summary>
        /// 获取一个字段的类型
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Type GetFieldType(FieldIndex field)
        {
            if (field >= 0 && (int)field <= 299)
                return typeof(int);
            if ((int)field >= 300 && (int)field <= 799)
                return typeof(float);
            if ((int)field >= 800 && (int)field <= 999)
                return typeof(double);
            if ((int)field >= 1000 && (int)field <= 1199)
                return typeof(long);
            if ((int)field >= 1200 && (int)field <= 8999)
                return typeof(string);
            return typeof(object);
        }

        /// <summary>
        /// 获取int值的字段
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public int GetFieldDataInt32(int code, FieldIndex fieldIndex)
        {
            int result = 0;
            Dictionary<FieldIndex, int> fieldInt32;
            if (DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                fieldInt32.TryGetValue(fieldIndex, out result);
            return result;
        }

        /// <summary>
        /// 获取long值的字段
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public long GetFieldDataInt64(int code, FieldIndex fieldIndex)
        {
            long result = 0;
            Dictionary<FieldIndex, long> fieldInt64;
            if (DetailData.FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                fieldInt64.TryGetValue(fieldIndex, out result);
            return result;
        }

        /// <summary>
        /// 获取float值的字段
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public float GetFieldDataSingle(int code, FieldIndex fieldIndex)
        {
            float result = 0;
            Dictionary<FieldIndex, float> fieldSingle;
            if (DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                fieldSingle.TryGetValue(fieldIndex, out result);
            return result;
        }

        /// <summary>
        /// 获取double值的字段
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public double GetFieldDataDouble(int code, FieldIndex fieldIndex)
        {
            double result = 0;
            Dictionary<FieldIndex, double> fieldDouble;
            if (DetailData.FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                fieldDouble.TryGetValue(fieldIndex, out result);
            return result;
        }


        /// <summary>
        /// 获取string值的字段
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public string GetFieldDataString(int code, FieldIndex fieldIndex)
        {
            string result = string.Empty;
            Dictionary<FieldIndex, string> fieldString;
            if (DetailData.FieldIndexDataString.TryGetValue(code, out fieldString))
            {
                if (!fieldString.TryGetValue(fieldIndex, out result))
                    result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// 获取object值的字段
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public object GetFieldDataObject(int code, FieldIndex fieldIndex)
        {
            object result = null;
            Dictionary<FieldIndex, object> fieldObject;
            if (DetailData.FieldIndexDataObject.TryGetValue(code, out fieldObject))
                fieldObject.TryGetValue(fieldIndex, out result);
            return result;
        }

        /// <summary>
        /// 获取一只股票的MarketType
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MarketType GetMarketType(int code)
        {
            MarketType mt = MarketType.NA;
            int mtInt = 0;
            if (DetailData.FieldIndexDataInt32.ContainsKey(code))
                DetailData.FieldIndexDataInt32[code].TryGetValue(FieldIndex.Market, out mtInt);
            mt = (MarketType)mtInt;
            return mt;
        }


        #endregion


        private DateTime dt1;
        private DateTime dt2;
        private static int _blockFlag = 1;
        public delegate void RecBlockElementHandle(string block, List<int> blockStocks);
        Dictionary<string, RecBlockElementHandle> _dicCallBack = new Dictionary<string, RecBlockElementHandle>();
        private Dictionary<string, Dictionary<string, InfoPanelChart>> _infoPanelCharts;
        private Dictionary<MarketType, InfoPanelLayout> _infoPanelLayout;
        private Dictionary<MarketType, List<TopBannerMenuItemPair>> _topBannerMenu;
        private FormulaDict _formulaDict;
        private FormulaFunctions _formulaFunctions;
        private IList<ShortLineType> _userShortLineTypes;
        private Image _priceUpImg;
        private Image _priceDownImg;
        private Image _initCustBlockOrderImg;
        private Image _initCustBlockOrderMouseOnImg;
        private Image _topBannerLeftArrow;
        private Image _topBannerLeftArrowMouseOn;
        private Image _topBannerLeftArrowMouseDown;
        private Image _topBannerRightArrow;
        private Image _topBannerRightArrowMouseOn;
        private Image _topBannerRightArrowMouseDown;
        private Image _topBannerBStock;
        private Image _topBannerBStockMouseOn;
        private Image _topBannerBStockMouseDown;
        private Image _topBannerHStock;
        private Image _topBannerHStockMouseOn;
        private Image _topBannerHStockMouseDown;
        private Image _topBannerHBStockB;
        private Image _topBannerHBStockBMouseOn;
        private Image _topBannerHBStockBMouseDown;
        private Image _topBannerHBStockH;
        private Image _topBannerHBStockHMouseOn;
        private Image _topBannerHBStockHMouseDown;
        private Image _topBannerAddUserStock;
        private Image _topBannerAddUserStockMouseOn;
        private Image _topBannerAddUserStockMouseDown;
        private Image _topBannerRemoveUserStock;
        private Image _topBannerRemoveUserStockMouseOn;
        private Image _topBannerRemoveUserStockMouseDown;
        private Image _topBannerMenuExpand;
        private Image _topBannerMenuExpandOn;
        private Image _fullViewUserStockArrow;
        private Image _stockDescription;
        private Image _stockItemDot;
        private Image _imgAdd;
        private Image _imgAdd_On;
        private Image _imgOrderDetailNormal;
        private Image _imgOrderDetailBigSell;
        private Image _imgOrderDetailBigBuy;
        private Image _imgClose;
        private Image _imgLev2Help;
        private Image _gubaData;
        private Image _gubaFix;
        private Image _gubaNews;
        private Image _gubaNotice;
        private Image _gubaResearch;
        private int _serverDate;
        private int _serverTime;
        private List<KeyValuePair<string, string>> _userBlock;

        /// <summary>
        /// 获取多个板块的成分内码（机构版）
        /// </summary>
        /// <param name="blockIds"></param>
        /// <param name="FunCallBack"></param>
        public void GetBlockStockListOrg(List<string> blockIds, RecBlockElementHandle FunCallBack)
        {
        }

        /// <summary>
        /// 获取一个板块的成分内码(机构版)
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="FunCallBack"></param>
        public void GetBlockStockListOrg(string blockId, RecBlockElementHandle FunCallBack)
        {

        }

        private void BlockElementsCallBack(string blockCode, object dtElemets)
        {
            //dt2 = DateTime.Now;
            //TimeSpan ts = dt2 - dt1;
            //Debug.Print("收到数据时间=" + ts.TotalMilliseconds);
            LogUtilities.LogMessage(blockCode + "收到成分，开始处理。time=" + DateTime.Now.ToLongTimeString());
            try
            {
                QuotoBlockElemenetResultData data = dtElemets as QuotoBlockElemenetResultData;
                if (data != null)
                {
                    Dictionary<string, List<BlockElementInfo>> elements = data.DicBlockElementsMarshal as Dictionary<string, List<BlockElementInfo>>;
                    string[] oneStock;
                    int unicode = 0;
                    if (elements.Count == 0)
                    {
                        RecBlockElementHandle handle = null;
                        if (_dicCallBack.ContainsKey(data.Guid))
                        {
                            handle = _dicCallBack[data.Guid];
                            _dicCallBack.Remove(data.Guid);
                        }
                        if (handle != null)
                            handle(blockCode, new List<int>(0));
                    }
                    foreach (KeyValuePair<string, List<BlockElementInfo>> oneBlock in elements)
                    {
                        List<int> oneBlockResult = new List<int>(oneBlock.Value.Count);
                        foreach (BlockElementInfo oneStockInfo in oneBlock.Value)
                        {
                            unicode = oneStockInfo.PublishCode;
                            if (unicode != 0)
                                oneBlockResult.Add(unicode);
                            Dictionary<FieldIndex, object> dicFieldObj = new Dictionary<FieldIndex, object>();

                            try
                            {
                                DetailData.SetStockBasicField(unicode, oneStockInfo.SecurityCode,
                                    oneStockInfo.CategoryType, oneStockInfo.SecurityChineseName);
                            }
                            catch (Exception e)
                            {
                                LogUtilities.LogMessage(e.Message);
                            }

                        }
                        RecBlockElementHandle handle = null;
                        if (_dicCallBack.ContainsKey(data.Guid))
                        {
                            handle = _dicCallBack[data.Guid];
                            _dicCallBack.Remove(data.Guid);
                        }
                        if (handle != null)
                            handle(oneBlock.Key, oneBlockResult);
                    }
                }
                LogUtilities.LogMessage(blockCode + "完成的成分处理。time=" + DateTime.Now.ToLongTimeString());
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(e.Message);
            }
            //TimeSpan ts2 = DateTime.Now - dt2;
            //Debug.Print("解析时间=" + ts2.TotalMilliseconds);
        }


        /// <summary>
        /// 获取多个板块的成分,不取缓存,直接问服务器拿的
        /// </summary>
        /// <param name="blockIds"></param>
        /// <returns></returns>
        //public Dictionary<string, List<string>> GetBlockStockListOrg(List<string> blockIds)
        //{
        //    DateTime dt = DateTime.Now;
        //    //Dictionary<string, List<string>> result = _blockService.GetBlockElementCdsByPublishCodes(new List<string>() { "007100", "007099" });
        //    Dictionary<string, List<string>> result = _blockService.GetBlockElementCdsByPublishCodes(blockIds);
        //    TimeSpan ts = DateTime.Now - dt;
        //    Debug.Print("获取多个板块成分 = " + ts.TotalMilliseconds);
        //    return result;
        //}

        /// <summary>
        /// 对制定codes排序
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="field"></param>
        /// <param name="sortMode"></param>
        /// <returns></returns>
        public List<int> GetQuoteCodeList(List<int> codes, FieldIndex field)
        {
            return GetQuoteCodeList(codes, field, SortMode.Mode_Code);
        }

        /// <summary>
        /// 获取多个板块的成分,不取缓存,直接问服务器拿的
        /// </summary>
        /// <param name="blockIds"></param>
        /// <returns></returns>
        //public Dictionary<string, List<string>> GetBlockStockListOrg(List<string> blockIds)
        //{
        //    DateTime dt = DateTime.Now;
        //    //Dictionary<string, List<string>> result = _blockService.GetBlockElementCdsByPublishCodes(new List<string>() { "007100", "007099" });
        //    Dictionary<string, List<string>> result = _blockService.GetBlockElementCdsByPublishCodes(blockIds);
        //    TimeSpan ts = DateTime.Now - dt;
        //    Debug.Print("获取多个板块成分 = " + ts.TotalMilliseconds);
        //    return result;
        //}

        /// <summary>
        /// 对制定codes排序
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="field"></param>
        /// <param name="sortMode"></param>
        /// <returns></returns>
        public List<int> GetQuoteCodeList(List<int> codes, FieldIndex field, SortMode sortMode)
        {
            if (codes != null)
                return QuoteSortService.SortFieldValue(codes, field, sortMode);
            return new List<int>();
        }

        /// <summary>
        /// 获取行业/地区/概念板块成分
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<int> GetBlockItems(EnumBlockType type)
        {
            return _blockData.GetBlockItems(type);
        }

        /// <summary>
        /// 获取一个市场，按一个字段排序后的，股票代码的集合
        /// </summary>
        /// <param name="sectorID"></param>
        /// <param name="field">排序的字段索引</param>
        /// <param name="sortMode">排序方式，默认按代码排</param>
        /// <returns></returns>
        //public List<int> GetQuoteCodeList(string sectorID, FieldIndex field, SortMode sortMode = SortMode.Mode_Code)
        //{
        //    List<string> result;
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    //List<string> codes = GetSecurityCodeList(sectorID);
        //    List<string> codes = GetBlockStockListOrg(sectorID);

        //    if (sortMode == SortMode.Mode_Code)
        //        result = codes;

        //    DetailDataTable detailDataTable = (DetailDataTable)_dataTableCollecion["detail"];

        //    if (codes != null)
        //        result = SortService.SortFieldValue(codes, detailDataTable.AllDetailDataRec, field, sortMode);
        //    else
        //        result = null;

        //    watch.Stop();
        //    Debug.Print(string.Format("GetQuoteCodeList :{0}", watch.ElapsedMilliseconds));

        //    return result;
        //}


        /// <summary>
        /// 取历史K线
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetHisKLineData(int code)
        {
            return GetHisKLineData(code, KLineCycle.CycleDay);
        }

        /// <summary>
        /// 获取一个市场，按一个字段排序后的，股票代码的集合
        /// </summary>
        /// <param name="sectorID"></param>
        /// <param name="field">排序的字段索引</param>
        /// <param name="sortMode">排序方式，默认按代码排</param>
        /// <returns></returns>
        //public List<int> GetQuoteCodeList(string sectorID, FieldIndex field, SortMode sortMode = SortMode.Mode_Code)
        //{
        //    List<string> result;
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    //List<string> codes = GetSecurityCodeList(sectorID);
        //    List<string> codes = GetBlockStockListOrg(sectorID);

        //    if (sortMode == SortMode.Mode_Code)
        //        result = codes;

        //    DetailDataTable detailDataTable = (DetailDataTable)_dataTableCollecion["detail"];

        //    if (codes != null)
        //        result = SortService.SortFieldValue(codes, detailDataTable.AllDetailDataRec, field, sortMode);
        //    else
        //        result = null;

        //    watch.Stop();
        //    Debug.Print(string.Format("GetQuoteCodeList :{0}", watch.ElapsedMilliseconds));

        //    return result;
        //}


        /// <summary>
        /// 取历史K线
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetHisKLineData(int code, KLineCycle cycle)
        {
            OneStockKLineDataRec result = null;

            Dictionary<KLineCycle, OneStockKLineDataRec> dicCycleKLine;
            if (((KLineDataTable)_dataTableCollecion["kline"]).AllKLineData.TryGetValue(code, out dicCycleKLine))
                dicCycleKLine.TryGetValue(cycle, out result);

            return result;
        }

        /// <summary>
        /// 获取当日K线
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetTodayKLineData(int code)
        {
            return GetTodayKLineData(code, KLineCycle.CycleDay);
        }

        /// <summary>
        /// 获取当日K线
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetTodayKLineData(int code, KLineCycle cycle)
        {
            OneStockKLineDataRec result = null;

            Dictionary<KLineCycle, OneStockKLineDataRec> dicCycleKLine;
            if (((KLineDataTable)_dataTableCollecion["kline"]).TodayKLineData.TryGetValue(code, out dicCycleKLine))
                dicCycleKLine.TryGetValue(cycle, out result);

            return result;
        }

        /// <summary>
        /// 获取净值后复权
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetFundAfterDivideKLineData(int code)
        {
            return GetFundAfterDivideKLineData(code, KLineCycle.CycleDay);
        }

        /// <summary>
        /// 获取净值后复权
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetFundAfterDivideKLineData(int code, KLineCycle cycle)
        {
            OneStockKLineDataRec result = null;

            Dictionary<KLineCycle, OneStockKLineDataRec> dicCycleKLine;
            if (((KLineDataTable)_dataTableCollecion["kline"]).FundAfterDivideKlineData.TryGetValue(code, out dicCycleKLine))
                dicCycleKLine.TryGetValue(cycle, out result);

            return result;
        }

        /// <summary>
        /// 获取K线向后请求标识
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public bool GetKLineReqBackFlag(int code, KLineCycle cycle)
        {
            Dictionary<KLineCycle, bool> cycleFlag;
            if (((KLineDataTable)_dataTableCollecion["kline"]).KLineReqBack.TryGetValue(code, out cycleFlag))
            {
                bool flag;
                if (cycleFlag.TryGetValue(cycle, out flag))
                    return flag;
                return true;
            }
            return true;
        }

        /// <summary>
        /// 设置K线向后请求标识 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public void SetKLineReqBackFlag(int code, KLineCycle cycle, bool setValue)
        {
            Dictionary<KLineCycle, bool> cycleFlag;
            if (((KLineDataTable)_dataTableCollecion["kline"]).KLineReqBack.TryGetValue(code, out cycleFlag))
            {
                cycleFlag[cycle] = setValue;
            }
            else
            {
                cycleFlag = new Dictionary<KLineCycle, bool>(1);
                cycleFlag[cycle] = setValue;
                ((KLineDataTable)_dataTableCollecion["kline"]).KLineReqBack[code] = cycleFlag;
            }
        }

        /// <summary>
        /// 获得当前走势最后一个分钟点的下标
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public short GetLastTrendPoint(int code)
        {
            return TimeUtilities.GetPointFromTime(code);
        }
        /// <summary>
        /// 获得当前集合竞价最后一个分钟点的下标
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public short GetCallAuctionLastPoint(int code)
        {
            return TimeUtilities.GetCallAuctionPointFromTime(code);
        }

        /// <summary>
        /// 获取走势数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public OneDayTrendDataRec GetTrendData(int code, int date)
        {
            OneDayTrendDataRec result = null;
            if (code != 0)
            {
                Dictionary<int, OneDayTrendDataRec> dicDateTrend;
                if (((TrendDataTable)_dataTableCollecion["trend"]).AllTrendData.TryGetValue(code, out dicDateTrend))
                {
                    if (!dicDateTrend.TryGetValue(date, out result))
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当日走势数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OneDayTrendDataRec GetTrendData(int code)
        {
            return GetTrendData(code, GetTradeDate(code));
        }

        /// <summary>
        /// 获取一只股票的成交明细数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<OneDealDataRec> GetDealData(int code)
        {
            List<OneDealDataRec> dealDatas;
            ((DealDataTable)_dataTableCollecion["deal"]).AllDealData.TryGetValue(code, out dealDatas);
            return dealDatas;
        }

        /// <summary>
        /// 获取一只股票的逐笔成交数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<OneTickDataRec> GetTickData(int code)
        {
            List<OneTickDataRec> dealDatas;
            ((TickDataTable)_dataTableCollecion["tick"]).AllDealData.TryGetValue(code, out dealDatas);
            return dealDatas;
        }

        /// <summary>
        /// 获取一只股票的委托明细数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<OneOrderDetailDataRec> GetOrderDetailData(int code)
        {
            List<OneOrderDetailDataRec> dealDatas;
            ((OrderDetailDataTable)_dataTableCollecion["orderdetail"]).AllOrderDetailData.TryGetValue(code, out dealDatas);
            return dealDatas;
        }

        /// <summary>
        /// 获取一只股票的资金流向数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CapitalFlowDataRec GetCapitalFlowData(int code)
        {
            CapitalFlowDataRec result;
            ((CapitalFlowTable)_dataTableCollecion["capitalflow"]).CapitalFlowData.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 获取一只股票分价表的数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public PriceStatusDataRec GetPriceStatusData(int code)
        {
            if (code == 0)
                return null;
            PriceStatusDataRec result;
            ((PriceStatusDataTable)_dataTableCollecion["pricestatus"]).PriceStatusData.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 获取某市场综合排名数据
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        public RankDataRec GetRankData(ReqSecurityType sType)
        {
            RankDataRec result;
            ((RankDataTable)_dataTableCollecion["rank"]).RankData.TryGetValue(sType, out result);
            return result;
        }

        /// <summary>
        /// 资金流向排名
        /// </summary>
        /// <param name="id">板块id</param>
        /// <param name="isTop">是否前10</param>
        /// <returns></returns>
        public List<int> GetNetInflowData(string id, bool isTop)
        {
            NetInflowRankType data;
            if (((RankDataTable)_dataTableCollecion["rank"]).NetInflowRankData.TryGetValue(id, out data))
            {
                if (isTop)
                    return data.TopStocks;
                return data.BottomStocks;
            }
            return new List<int>(0);
        }

        /// <summary>
        /// 资金流向排名(沪深AB)
        /// </summary>
        /// <param name="id">板块id</param>
        /// <param name="isTop">是否前10</param>
        /// <returns></returns>
        public List<int> GetNetInflowData(bool isTop)
        {
            NetInflowRankType data;
            if (((RankDataTable)_dataTableCollecion["rank"]).NetInflowRankData.TryGetValue("91001001", out data))
            {
                if (isTop)
                    return data.TopStocks;
                return data.BottomStocks;
            }
            return new List<int>(0);
        }

        /// <summary>
        /// 获取某板块综合排名数据
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<int> GetRankOrgData(string blockId, RankType type)
        {
            RankOrgDataRec memData;
            List<int> result = new List<int>();
            if (((RankDataTable)_dataTableCollecion["rank"]).RankOrgData.TryGetValue(blockId, out memData))
            {
                if (!memData.RankData.TryGetValue(type, out result))
                    result = new List<int>();
            }
            return result;
        }

        /// <summary>
        /// 获取某板块综合排名数据
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        public RankOrgDataRec GetRankOrgData(string blockId)
        {
            RankOrgDataRec result;
            ((RankDataTable)_dataTableCollecion["rank"]).RankOrgData.TryGetValue(blockId, out result);
            return result;
        }

        /// <summary>
        /// 获取一只股票F10的数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public F10DataRec GetF10Data(int code)
        {
            F10DataRec result;
            ((F10DataTable)_dataTableCollecion["f10"]).AllF10Data.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 获取板块信息
        /// </summary>
        /// <returns></returns>
        public BlockDataRec GetBlockData()
        {
            return _blockData;
        }


        /// <summary>
        /// 获取信息雷达
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<InfoMine, List<OneInfoMineDataRec>> GetInfoMine(int code)
        {
            Dictionary<InfoMine, List<OneInfoMineDataRec>> result;
            ((DetailDataTable)_dataTableCollecion["detail"]).InfoMineData.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 获取信息雷达
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> GetInfoMineOrg(int code)
        {
            Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> result;
            ((DetailDataTable)_dataTableCollecion["detail"]).InfoMineOrgData.TryGetValue(code, out result);
            return result;
        }
        /// <summary>
        /// 获取新资讯列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> GetNewInfoMineOrg(int code)
        {
            Dictionary<InfoMineOrg, List<OneInfoMineOrgDataRec>> result;
            ((DetailDataTable)_dataTableCollecion["detail"]).InfoMineOrgData.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 盈利预测数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OneProfitForecastDataRec GetProfitForecast(int code)
        {
            OneProfitForecastDataRec result;
            ((ProfitForecastDataTable)_dataTableCollecion["profit"]).AllProfitForecastData.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 盈利预测（机构版）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OneProfitForecastOrgDataRec GetProfitForecastOrg(int code)
        {
            OneProfitForecastOrgDataRec result;
            ((ProfitForecastDataTable)_dataTableCollecion["profit"]).AllProfitForecastOrgData.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 机构评级
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OrgRateDataRec GetOrgRate(int code)
        {
            OrgRateDataRec result;
            ((OrgRateDataTable)_dataTableCollecion["orgrate"]).OrgRateDatas.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 机构评级机构版
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<OneInfoRateOrgItem> GetInfoRateOrg(int code)
        {
            List<OneInfoRateOrgItem> result;
            ((OrgRateDataTable)_dataTableCollecion["orgrate"]).InfoRateOrgData.TryGetValue(code, out result);
            return result;
        }


        /// <summary>
        /// 获得债券综合屏-新闻
        /// 债券新闻，对应债券频道——债券新闻 ———— H1 F009
        /// 请求参数：typeLevel1为1，typeLevel2为F009
        /// </summary>
        /// <param name="typeLevel2">小类，比如F009，S001001</param>
        /// <returns></returns>
        public List<OneInfoMineOrgDataRec> GetBondDashboardNews(string typeLevel2)
        {
            List<OneInfoMineOrgDataRec> result = new List<OneInfoMineOrgDataRec>();
            if (((DetailDataTable)_dataTableCollecion["detail"]).DicNewsInfoByBlock.TryGetValue(typeLevel2, out result))
            {
                return result;
            }
            return result;
        }
        /// <summary>
        /// 获得债券综合屏-研报
        /// 研究报告，对应期货研究——国债期货研究 ———— H3 T004007003
        /// 请求参数：typeLevel1为3，typeLevel2为T004007003
        /// </summary>
        /// <param name="typeLevel2">小类，比如F009，S001001</param>
        /// <returns></returns>
        public List<OneInfoMineOrgDataRec> GetBondDashboardNewsReport(string typeLevel2)
        {
            List<OneInfoMineOrgDataRec> result = new List<OneInfoMineOrgDataRec>();
            if (((DetailDataTable)_dataTableCollecion["detail"]).DicNewsReportInfoByBlock.TryGetValue(typeLevel2, out result))
            {
                return result;
            }
            return result;
        }
        /// <summary>
        /// 委托队列数据
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <param name="isBuy">是否为买一</param>
        /// <returns></returns>
        public OrderQueueDataRec GetOrderQueueData(int code, bool isBuy)
        {
            OrderQueueDataRec result = null;
            if (isBuy)
            {
                if (((OrderQueueDataTable)_dataTableCollecion["orderqueue"]).OrderQueueDataBuy != null)
                    ((OrderQueueDataTable)_dataTableCollecion["orderqueue"]).OrderQueueDataBuy.TryGetValue(code,
                                                                                                            out result);
            }
            else
            {
                if (((OrderQueueDataTable)_dataTableCollecion["orderqueue"]).OrderQueueDataSell != null)
                    ((OrderQueueDataTable)_dataTableCollecion["orderqueue"]).OrderQueueDataSell.TryGetValue(code,
                                                                                                            out result);
            }
            return result;
        }

        /// <summary>
        /// 获取全部短线精灵数据
        /// </summary>
        /// <returns></returns>
        public List<OneShortLineDataRec> GetShortLineData()
        {
            if (_dataTableCollecion.ContainsKey("shortline"))
                return ((ShortLineStrategyDataTable)_dataTableCollecion["shortline"]).GetShortLineData();
            else
            {
                return new List<OneShortLineDataRec>();
            }
        }

        /// <summary>
        /// 获取指定类型的板块精灵数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<OneShortLineDataRec> GetShortLineData(ShortLineType type)
        {
            return ((ShortLineStrategyDataTable)_dataTableCollecion["shortline"]).GetShortLineData(type);
        }

        /// <summary>
        /// 获取指定类型的板块精灵数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<OneShortLineDataRec> GetShortLineData(IList<ShortLineType> types)
        {
            return ((ShortLineStrategyDataTable)_dataTableCollecion["shortline"]).GetShortLineData(types);
        }

        /// <summary>
        /// 获取指数贡献点数,返回值按降序排序
        /// </summary>
        /// <param name="mt">市场类型</param>
        /// <param name="isStock">是否个股贡献点</param>
        /// <returns>返回值按降序排序</returns>
        public List<ContributionDataRec> GetContributionData(ReqMarketType mt, bool isStock)
        {
            List<ContributionDataRec> result;
            if (isStock)
                ((ContributionDataTable)_dataTableCollecion["contribution"]).ContributionStock.TryGetValue(mt,
                                                                                                            out result);
            else
                ((ContributionDataTable)_dataTableCollecion["contribution"]).ContributionBlock.TryGetValue(mt,
                                                                                                            out result);
            return result;
        }

        /// <summary>
        /// 获取东财指数的Code和PublishCode
        /// </summary>
        /// <returns></returns>
        public string GetEmIndexPublishCode(int code)
        {
            string result = string.Empty;
            _dicEMIndexCodePublishCode.TryGetValue(code, out result);
            return result;
        }

        /// <summary>
        /// 获取24小时滚动资讯
        /// </summary>
        /// <returns></returns>
        public List<OneNews24HDataRec> GetNews24HData()
        {
            return ((News24hDataTable)_dataTableCollecion["news24h"]).News24HData;
        }

        /// <summary>
        /// 获取24小时滚动资讯(机构版)
        /// </summary>
        /// <returns></returns>
        public List<OneNews24HOrgDataRec> GetNews24HOrgData()
        {
            return ((News24hDataTable)_dataTableCollecion["news24h"]).News24HOrgData;
        }

        /// <summary>
        /// 获取要闻精华(机构版)
        /// </summary>
        /// <returns></returns>
        public List<OneNews24HOrgDataRec> GetImportantNewsOrgData()
        {
            return ((News24hDataTable)_dataTableCollecion["news24h"]).ImportantNewsData;
        }

        /// <summary>
        /// 获取公司快讯(机构版)
        /// </summary>
        /// <returns></returns>
        public List<OneNews24HOrgDataRec> GetNewsFlashOrgData()
        {
            return ((News24hDataTable)_dataTableCollecion["news24h"]).NewsFlashData;
        }

        /// <summary>
        /// 获取某只股票所在的板块成分，以及下标值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="blockStocks"></param>
        /// <param name="index"></param>
        public void GetStockInBlock(int code, out List<int> blockStocks, out int index)
        {
            List<int> result1 = new List<int>();
            int result2 = 0;
            blockStocks = result1;
            index = result2;
        }

        /// <summary>
        /// 获取板块树节点的种类
        /// </summary>
        /// <param name="publishCode"></param>
        /// <returns></returns>
        public static BlockTreeCategory GetBlockCategory(string publishCode)
        {
            if (string.IsNullOrEmpty(publishCode))
                return BlockTreeCategory.SHSZ;

            BlockTreeCategory result = BlockTreeCategory.SHSZ;
            //if (IsCustomerBlock(publishCode))
            //    return result;
            try
            {
                DataView dv = new DataView(_blockTreeDataTable);
                string filter = string.Format("PUBLISHCODE = '{0}'", publishCode);
                dv.RowFilter = filter;
                if (dv.Count > 0)
                {
                    string tmpl = dv[0]["TMPL"].ToString();
                    if (!string.IsNullOrEmpty(tmpl))
                    {
                        int tmplInt = Convert.ToInt32(tmpl);
                        result = (BlockTreeCategory)tmplInt;
                    }
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("DC GetBlockCategory Error:  " + e.Message);
            }

            return result;
        }
        /// <summary>
        /// 获取板块树节点的名称
        /// </summary>
        /// <param name="publishCode"></param>
        /// <returns></returns>
        public string GetBlockPublishName(string publishCode)
        {
            if (string.IsNullOrEmpty(publishCode))
                return string.Empty;

            string result = string.Empty;
            DataView dv = new DataView(_blockTreeDataTable);
            string filter = string.Format("PUBLISHCODE = '{0}'", publishCode);
            dv.RowFilter = filter;
            if (dv.Count > 0)
            {
                string tmpl = dv[0]["PUBLISHNAME"].ToString();
                if (!string.IsNullOrEmpty(tmpl))
                {
                    result = tmpl;
                }
            }
            return result;
        }
        #region ☆☆☆★★★很重要★★★☆☆☆。修改BlockTreeCategory时，涉及报价相关的修改部分
        /// <summary>
        /// 获取报价模版标识
        /// </summary>
        /// <param name="publishCode"></param>
        /// <returns></returns>
        public static string SectorType(string publishCode)
        {
            string result;

            BlockTreeCategory category = GetBlockCategory(publishCode);
            switch (category)
            {
                case BlockTreeCategory.SHSZ:
                case BlockTreeCategory.BStock:
                case BlockTreeCategory.Industry:
                    result = ReportConfigDefines.ST_AStock;
                    break;
                case BlockTreeCategory.HK:
                    result = ReportConfigDefines.ST_HKStock;
                    break;
                case BlockTreeCategory.USA:
                    result = ReportConfigDefines.ST_USAStock;
                    break;
                case BlockTreeCategory.OpenFund:
                    result = ReportConfigDefines.ST_OpenFund;
                    break;
                case BlockTreeCategory.FinancialManager:
                    result = ReportConfigDefines.ST_FinancialManager;
                    break;
                case BlockTreeCategory.CloseFund:
                    result = ReportConfigDefines.ST_CloseFund;
                    break;
                case BlockTreeCategory.Bond:
                    result = ReportConfigDefines.ST_Bond;
                    break;
                case BlockTreeCategory.InterestRate:
                    result = ReportConfigDefines.ST_InterestRate;
                    break;
                case BlockTreeCategory.Exchange:
                    result = ReportConfigDefines.ST_Exchange;
                    break;
                case BlockTreeCategory.Index:
                case BlockTreeCategory.Global:
                case BlockTreeCategory.EmIndex:
                case BlockTreeCategory.ForeignIndex:
                    result = ReportConfigDefines.ST_Index;
                    break;
                case BlockTreeCategory.IndexFutures:
                    result = ReportConfigDefines.ST_IndexFutures;
                    break;
                case BlockTreeCategory.Futures:
                case BlockTreeCategory.ChaosFutures:
                    result = ReportConfigDefines.ST_Futures;
                    break;
                case BlockTreeCategory.OverSeaFutures:
                case BlockTreeCategory.OSFuturesLME:
                    result = ReportConfigDefines.ST_OverSeaFutures;
                    break;
                default:
                    result = ReportConfigDefines.ST_AStock;
                    break;
            }

            return result;
        }
        /// <summary>
        /// 根据板块类别，判断其使用的报价模块（板块报价、多股同列、综合排名、资金流向、DDE）
        /// </summary>
        /// <param name="publishCode">板块code</param>
        /// <returns></returns>
        public static string GetQuoteReqortsHTableConfig(string publishCode)
        {
            string result = "QuoteReport_1R";

            BlockTreeCategory category = GetBlockCategory(publishCode);

            switch (category)
            {
                case BlockTreeCategory.SHSZ:
                    result = "QuoteReport_8R";
                    break;
                case BlockTreeCategory.BStock:
                    result = "QuoteReport_BStock";
                    break;
                case BlockTreeCategory.Index:
                case BlockTreeCategory.Global:
                case BlockTreeCategory.EmIndex:
                case BlockTreeCategory.ForeignIndex:
                case BlockTreeCategory.HK:
                case BlockTreeCategory.USA:
                    result = "QuoteReport_3R";
                    break;
                case BlockTreeCategory.Bond:
                case BlockTreeCategory.InterestRate:
                case BlockTreeCategory.IndexFutures:
                case BlockTreeCategory.Futures:
                case BlockTreeCategory.ChaosFutures:
                case BlockTreeCategory.OverSeaFutures:
                case BlockTreeCategory.OSFuturesLME:
                case BlockTreeCategory.CloseFund:
                case BlockTreeCategory.OpenFund:
                    result = "QuoteReport_2R";
                    break;
                case BlockTreeCategory.Exchange:
                case BlockTreeCategory.FinancialManager:
                    result = "QuoteReport_1R";
                    break;
                case BlockTreeCategory.Industry:
                default:
                    result = "QuoteReport_1R";
                    break;
            }

            return result;
        }
        /// <summary>
        /// 根据板块ID，返回当前所使用的SectorBar类别
        /// </summary>
        /// <param name="publishCode">板块ID</param>
        /// <returns>SectorBar类别</returns>
        public static string SectorBarType(string publishCode)
        {
            string result;

            BlockTreeCategory category = GetBlockCategory(publishCode);
            switch (category)
            {
                case BlockTreeCategory.SHSZ:
                case BlockTreeCategory.BStock:
                    result = SectorBarConstantDefines.SB_AStock;
                    break;
                case BlockTreeCategory.Index:
                case BlockTreeCategory.Global:
                case BlockTreeCategory.EmIndex:
                case BlockTreeCategory.ForeignIndex:
                    result = SectorBarConstantDefines.SB_Index;
                    break;
                case BlockTreeCategory.HK:
                    result = SectorBarConstantDefines.SB_HKStock;
                    break;
                case BlockTreeCategory.USA:
                    result = SectorBarConstantDefines.SB_USAStock;
                    break;
                case BlockTreeCategory.Bond:
                    result = SectorBarConstantDefines.SB_Bond;
                    break;
                case BlockTreeCategory.InterestRate:
                    result = SectorBarConstantDefines.SB_InterestRate;
                    break;
                case BlockTreeCategory.Exchange:
                    result = SectorBarConstantDefines.SB_Exchange;
                    break;
                case BlockTreeCategory.IndexFutures:
                    result = SectorBarConstantDefines.SB_IndexFuture;
                    break;
                case BlockTreeCategory.Futures:
                case BlockTreeCategory.ChaosFutures:
                case BlockTreeCategory.OverSeaFutures:
                case BlockTreeCategory.OSFuturesLME:
                    result = SectorBarConstantDefines.SB_Future;
                    break;
                case BlockTreeCategory.CloseFund:
                case BlockTreeCategory.OpenFund:
                    result = SectorBarConstantDefines.SB_Fund;
                    break;
                case BlockTreeCategory.FinancialManager:
                    result = SectorBarConstantDefines.SB_FinancialManager;
                    break;
                case BlockTreeCategory.Industry:
                default:
                    result = SectorBarConstantDefines.SB_Default;
                    break;
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 根据publishcode获取marketype
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MarketType GetMarketByPublishCode(string id)
        {
            MarketType result = MarketType.SHALev1;
            BlockTreeCategory category = GetBlockCategory(id);
            switch (category)
            {
                case BlockTreeCategory.HK:
                    result = MarketType.HK;
                    break;
                case BlockTreeCategory.Futures:
                case BlockTreeCategory.ChaosFutures:
                    result = MarketType.SHF;
                    break;
                case BlockTreeCategory.IndexFutures:
                    result = MarketType.IF;
                    break;
            }
            return result;
        }

        public int SaveString(string strValue)
        {
            return 0;
        }

        /// <summary>
        /// 指数红绿柱
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OneDayRedGreenDataRec GetRedGreen(int code)
        {
            OneDayRedGreenDataRec result = null;
            if (code != 0)
            {
                ((TrendDataTable)_dataTableCollecion["trend"]).AllRedGreenData.TryGetValue(code, out result);
            }
            return result;
        }

        /// <summary>
        /// 获取一只股票的除复权信息
        /// </summary>
        /// <returns></returns>
        public List<List<OneDivideRightBase>> GetDivideRightData(int code)
        {
            List<List<OneDivideRightBase>> result = null;
            if (code != 0)
            {
                if (!((KLineDataTable)_dataTableCollecion["kline"]).DivideRightData.TryGetValue(code, out result))
                    result = new List<List<OneDivideRightBase>>(0);
            }
            return result;
        }

        /// <summary>
        /// 计算复权
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="isForward">是否前复权</param>
        //public List<DrawKlineDataStru> CaculateDivideKLineData(int code,  List<DrawKlineDataStru> data, bool isCycleYear, bool isForward)
        //{
        //    if (data == null)
        //        return null;

        //    List<List<OneDivideRightBase>> divideData = GetDivideRightData(code);
        //    float factor = 1;
        //    int indexLast = 0;
        //    if(isForward)
        //    {
        //        indexLast = -1;
        //        for (int i = 0; i < divideData.Count; i++)
        //        {

        //            factor *= divideData[i][0].Factor;
        //            if (factor == 0 || factor == 1 || divideData[i][0].DivideType == DivideRightType.GengMing)
        //                continue;
        //            for (int j = indexLast + 1; j < data.Count; j++)
        //            {
        //                if (data[j].KlineData.Date < divideData[i][0].Date)
        //                {
        //                    data[j].KlineData.High *= factor;
        //                    indexLast = j;
        //                }
        //                else if(data[j].KlineData.Date == divideData[i][0].Date)
        //                {
        //                    indexLast = j;
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        indexLast = data.Count;
        //        for (int i = divideData.Count - 1; i >= 0; i--)
        //        {
        //            if (divideData[i][0].Factor == 0 || divideData[i][0].Factor == 1 || divideData[i][0].DivideType == DivideRightType.GengMing)
        //                continue;
        //            factor /= divideData[i][0].Factor;

        //            for (int j = indexLast - 1; j >= 0; j--)
        //            {
        //                if (data[j].KlineData.Date > divideData[i][0].Date)
        //                {
        //                    data[j].KlineData.High /= factor;
        //                    indexLast = j;
        //                }
        //                else if (data[j].KlineData.Date == divideData[i][0].Date)
        //                {
        //                    indexLast = j;
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 计算复权，只改变复权因子
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="isCycleYear"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        public void CaculateDivideKLineData(int code, List<DrawKlineDataStru> data, bool isCycleYear, bool isForward)
        {
            List<List<OneDivideRightBase>> divideData = GetDivideRightData(code);
            float factor = 1;
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
                        if (data[j].KlineData.Date < divideData[i][0].Date)
                        {
                            data[j].ForwardFactor = factor;
                            indexLast = j;
                        }
                        else
                        {
                            if (i == divideData.Count - 1)
                                data[j].ForwardFactor = 1.0f;
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
                        if (data[j].KlineData.Date >= divideData[i][0].Date)
                        {
                            data[j].BackFactor = factor;
                            indexLast = j;
                        }
                        else
                        {
                            if (i == 0)
                                data[j].BackFactor = 1.0f;
                            else
                                break;
                        }
                    }
                    factor /= divideData[i][0].Factor;
                }
            }
        }

        ///// <summary>
        ///// 设置股票标记
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="tag"></param>
        //public void SetStockTag(int code, StockTag tag)
        //{
        //    if (DetailData.AllStockDetailData.ContainsKey(code))
        //    {
        //        DetailData.AllStockDetailData[code][FieldIndex.StockTagEnum] = tag;
        //    }
        //}

        ///// <summary>
        ///// 设置股票标记
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="tag"></param>
        //public void SetStockTag(int code, string text)
        //{
        //    if (DetailData.AllStockDetailData.ContainsKey(code))
        //    {
        //        DetailData.AllStockDetailData[code][FieldIndex.StockTagEnum] = StockTag.Text;
        //        DetailData.AllStockDetailData[code][FieldIndex.StockTagText] = text;
        //    }
        //}

        /// <summary>
        /// 设置股票标记
        /// </summary>
        /// <param name="code"></param>
        /// <param name="tag"></param>
        public void SetStockTag(int code, StockTag tag, string text)
        {
            Dictionary<FieldIndex, object> fieldObject;
            Dictionary<FieldIndex, int> fieldInt32;
            if (!DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
            {
                fieldInt32 = new Dictionary<FieldIndex, int>(1);
                DetailData.FieldIndexDataInt32[code] = fieldInt32;
            }
            if (!DetailData.FieldIndexDataObject.TryGetValue(code, out fieldObject))
            {
                fieldObject = new Dictionary<FieldIndex, object>(1);
                DetailData.FieldIndexDataObject[code] = fieldObject;
            }
            fieldInt32[FieldIndex.StockTagEnum] = (int)tag;
            if (tag == StockTag.Text)
                fieldObject[FieldIndex.StockTagText] = text;
            else
                fieldObject[FieldIndex.StockTagText] = string.Empty;
            StockMarkInfoMananger.GetInstance().SetStockTag(code, tag, text);
        }

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <param name="code"></param>
        public void DeletStockTag(int code)
        {
            Dictionary<FieldIndex, object> fieldObject;
            Dictionary<FieldIndex, int> fieldInt32;
            if (DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
            {
                if (fieldInt32.ContainsKey(FieldIndex.StockTagEnum))
                    fieldInt32.Remove(FieldIndex.StockTagEnum);
                if (DetailData.FieldIndexDataObject.TryGetValue(code, out fieldObject))
                {
                    if (fieldObject.ContainsKey(FieldIndex.StockTagText))
                        fieldObject.Remove(FieldIndex.StockTagText);
                }
                StockMarkInfoMananger.GetInstance().DeleteStockTag(code);
            }

        }
        /// <summary>
        /// 返回股票标记Image
        /// </summary>
        /// <param name="code">股票code</param>
        /// <param name="stockTag">标记</param>
        /// <returns>标记Image</returns>
        public Image GetStockFlagImage(int code, StockTag stockTag)
        {
            switch (stockTag)
            {
                case StockTag.One:
                    return StockTag_1_Image;
                case StockTag.Two:
                    return StockTag_2_Image;
                case StockTag.Three:
                    return StockTag_3_Image;
                case StockTag.Four:
                    return StockTag_4_Image;
                case StockTag.Five:
                    return StockTag_5_Image;
                case StockTag.Six:
                    return StockTag_6_Image;
                case StockTag.Seven:
                    return StockTag_7_Image;
                case StockTag.Eight:
                    return StockTag_8_Image;
                case StockTag.Nine:
                    return StockTag_9_Image;
                case StockTag.Text:
                    return StockTag_Text_Image;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取十大股东数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ShareHolderDataRec GetShareHolder(int code)
        {
            ShareHolderDataRec result = new ShareHolderDataRec();
            if (code != 0)
            {
                if (!((DepthAnalyseDataTable)_dataTableCollecion["depthanalyse"]).AllShareHolderDataRec.TryGetValue(code, out result))
                    result = new ShareHolderDataRec();
            }
            return result;
        }

        /// <summary>
        /// 利润趋势
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ProfitTrendDataRec GetProfitTrend(int code)
        {
            ProfitTrendDataRec result = new ProfitTrendDataRec();
            if (code != 0)
            {
                if (!((DepthAnalyseDataTable)_dataTableCollecion["depthanalyse"]).AllProfitTrendDataRec.TryGetValue(code, out result))
                    result = new ProfitTrendDataRec();
            }
            return result;
        }

        /// <summary>
        /// 获取重点指数的内码集合
        /// </summary>
        /// <returns></returns>
        public List<int> GetMainIndexCodes()
        {
            return _mainIndexCodeList;
        }


        /// <summary>
        /// 获取一只股票百档行情数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StockOrderDetailDataRec GetStockOrderDetailLev2Data(int code)
        {
            StockOrderDetailDataRec result = null;
            if (code != 0)
            {
                if (!((DetailLev2DataTable)_dataTableCollecion["detaillev2"]).AllStockOrderDetailDataRec.TryGetValue(code, out result))
                    result = new StockOrderDetailDataRec();
            }
            return result;
        }
        /// <summary>
        /// 获取一只股票N档(默认20档)行情数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StockOrderDetailDataRec GetNStockOrderDetailLev2Data(int code)
        {
            StockOrderDetailDataRec result = null;
            if (code != 0)
            {
                if (!((DetailLev2DataTable)_dataTableCollecion["detaillev2"]).NStockOrderDetailDataRec.TryGetValue(code, out result))
                    result = new StockOrderDetailDataRec();
            }
            return result;
        }


        /// <summary>
        /// 获取百档行情挂单队列
        /// </summary>
        /// <param name="code">内码</param>
        /// <param name="price">价格</param>
        /// <param name="bsFlag">买卖方向，0-buy,1-sell</param>
        /// <returns></returns>
        public StockPriceOrderQueueDataRec GetPriceOrderQueueData(int code, float price, byte bsFlag)
        {
            if (code != 0)
            {
                if (bsFlag == 0)
                {
                    Dictionary<float, StockPriceOrderQueueDataRec> memData;
                    if (
                        ((DetailLev2DataTable)_dataTableCollecion["detaillev2"]).AllStockBuyOrderQueueDataRec
                            .TryGetValue(code, out memData))
                    {
                        if (memData.ContainsKey(price))
                            return memData[price];
                    }
                }
                else if (bsFlag == 1)
                {
                    Dictionary<float, StockPriceOrderQueueDataRec> memData;
                    if (
                        ((DetailLev2DataTable)_dataTableCollecion["detaillev2"]).AllStockSellOrderQueueDataRec
                            .TryGetValue(code, out memData))
                    {
                        if (memData.ContainsKey(price))
                            return memData[price];
                    }
                }
            }
            return new StockPriceOrderQueueDataRec();
        }

        /// <summary>
        /// 统计明细
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StatisticsAnalysisDataRec GetStatisticsAnalysisData(int code)
        {
            StatisticsAnalysisDataRec result = null;
            if (code != 0)
            {
                if (!((DetailLev2DataTable)_dataTableCollecion["detaillev2"]).AllStatisticsAnalysisDataRec.TryGetValue(code, out result))
                    result = new StatisticsAnalysisDataRec();
            }
            return result;
        }

        /// <summary>
        /// 获取一只股票一天的走势资金流数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public StockTrendCaptialFlowDataRec GetTrendCaptialFlowData(int code, int date)
        {
            StockTrendCaptialFlowDataRec result = new StockTrendCaptialFlowDataRec(code);
            if (code != 0)
            {
                Dictionary<int, StockTrendCaptialFlowDataRec> memData;
                if (((TrendDataTable)_dataTableCollecion["trend"]).AllTrendCaptialFlowData.TryGetValue(code,
                    out memData))
                {
                    if (!memData.TryGetValue(date, out result))
                        result = new StockTrendCaptialFlowDataRec(code);
                }
            }
            return result;
        }
        /// <summary>
        /// 获得全景图中研究报告
        /// </summary>
        /// <returns></returns>
        public List<ResearchReportItem> GetResearchReportItems()
        {
            List<ResearchReportItem> result = new List<ResearchReportItem>();
            DataTableBase table;
            if (_dataTableCollecion.TryGetValue("reschRpt", out table))
            {
                ResearchReportDatatable reschRptDT = table as ResearchReportDatatable;
                result = reschRptDT.InfoResearchReportList;
            }

            return result;
        }
        /// <summary>
        /// 获得债券综合屏-公开市场操作模块的静态数据列表
        /// </summary>
        /// <returns></returns>
        public List<BondPublicOpeartionItem> GetBondPublicOpeartionItems()
        {
            List<BondPublicOpeartionItem> result = new List<BondPublicOpeartionItem>();
            DataTableBase table;
            if (_dataTableCollecion.TryGetValue("reschRpt", out table))
            {
                ResearchReportDatatable reschRptDT = table as ResearchReportDatatable;
                result = reschRptDT.BondPublicOpeartionList;
            }

            return result;
        }
        /// <summary>
        ///获得资金流日K线数据
        /// </summary>
        /// <param name="code">股票内码</param>
        /// <param name="cycle">K 线周期类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>获得资金流日K线数据</returns>
        public CapitalFlowDayKLineDataRecs GetCaptialFlowDayData(int code, KLineCycle cycle, int startDate, int endDate)
        {
            CapitalFlowDayKLineDataRecs result = new CapitalFlowDayKLineDataRecs();
            CapitalFlowDayKLineDataRecs tempResult = new CapitalFlowDayKLineDataRecs();
            Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs> innerDic;
            DataTableBase table;
            if (_dataTableCollecion.TryGetValue("kline", out table))
            {
                Dictionary<int, Dictionary<KLineCycle, CapitalFlowDayKLineDataRecs>> allDic = ((KLineDataTable)table).CapitalFlowDayKLineData;
                if (!allDic.TryGetValue(code, out innerDic))
                    return tempResult;

                if (!innerDic.TryGetValue(cycle, out tempResult))
                    return tempResult;
            }

            SortedDictionary<int, TrendCaptialFlowDataRec> tempDic = new SortedDictionary<int, TrendCaptialFlowDataRec>();
            foreach (KeyValuePair<int, TrendCaptialFlowDataRec> oldItem in tempResult.SortDicCaptialFlowList)
            {
                if (oldItem.Key >= startDate && oldItem.Key <= endDate)
                {
                    tempDic[oldItem.Key] = oldItem.Value;
                }
            }
            result.Code = tempResult.Code;
            result.Cycle = tempResult.Cycle;
            result.SortDicCaptialFlowList = tempDic;
            return result;
        }

        /// <summary>
        /// 获得Shibor报价行明细
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<OfferBankDetail> GetShiborReportDetail(int code)
        {
            List<OfferBankDetail> dealDatas = new List<OfferBankDetail>();
            if (((DealDataTable)_dataTableCollecion["deal"]).AllOfferBankDetailData.TryGetValue(code, out dealDatas))
                return dealDatas;

            return dealDatas;
        }


        /// <summary>
        /// 获得银行间债券报价明细
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<BidDetail> GetBankBondReportDetail(int code)
        {
            List<BidDetail> dealDatas = new List<BidDetail>();
            if (((DealDataTable)_dataTableCollecion["deal"]).AllBidDetailData.TryGetValue(code, out dealDatas))
                return dealDatas;

            return dealDatas;
        }

        #endregion

        #region 自选股



        /// <summary>
        /// 获取用户板块
        /// Key:BlockId
        /// Value:BlockName
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GetUserBlocks()
        {
            List<KeyValuePair<string, string>> result = null;

            if (result == null)
            {
                result = new List<KeyValuePair<string, string>>(0);
            }

            return result;
        }

        /// <summary>
        /// 添加自选股
        /// </summary>
        public bool AddUserSecurity(string blockId, int unicode)
        {
            

            return false;
        }

        /// <summary>
        /// 删除自选股
        /// </summary>
        public void DeleteUserSecurity(string blockid, int unicode)
        {

        }

        /// <summary>
        /// 是否是自选股
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsUserSecurity(int unicode)
        {
            return true;
        }

        /// <summary>
        /// 是否是自选股
        /// </summary>
        /// <param name="code"></param>
        /// <param name="blockid"></param>
        /// <returns></returns>
        public bool IsUserSecurity(int unicode, string blockid)
        {

            return false;
        }

        /// <summary>
        /// 判断指定板块是否为“我的自选股”板块
        /// </summary>
        /// <param name="blockID">板块ID</param>
        /// <returns></returns>
        public static bool IsCustomerHoldStockBlock(string blockID)
        {
            if (string.IsNullOrEmpty(blockID))
                return false;
            return blockID.ToLower().Equals("0.u");
        }

        /// <summary>
        /// 判断指定板块是否为“自选股”板块
        /// </summary>
        /// <param name="blockID">板块ID</param>
        /// <returns>true: 自选股板块；false: 系统板块</returns>
        public static bool IsCustomerBlock(string blockID)
        {
            if (string.IsNullOrEmpty(blockID))
                return false;
            return blockID.ToLower().EndsWith(".u");
        }

        /// <summary>
        /// 检查传入的自选股板块ID，是否存在于自选股板块列表中，
        /// 如果存在直接返回当前传入的板块ID，
        /// 如果不存在则返回“我的自选股”板块ID
        /// </summary>
        /// <param name="blockID">需要检查的自选股板块ID</param>
        /// <returns>核实后的自选股板块ID</returns>
        public string CheckCustomerBlockID(string blockID)
        {
            string result = string.Empty;

            List<KeyValuePair<string, string>> blocks = GetUserBlocks();

            if (blocks != null && blocks.Count > 0)
            {
                int locationIndex = -1;
                for (int index = 0; index < blocks.Count; index++)
                {
                    KeyValuePair<string, string> blockElements = blocks[index];
                    if (string.Equals(blockElements.Key, blockID))
                    {
                        locationIndex = index;
                        break;
                    }
                }
                if (locationIndex != -1)
                {
                    result = blockID;
                }
                else
                {
                    result = blocks[0].Key;
                }
            }

            return result;
        }
        #endregion


        #region 宏观指标
        /// <summary>
        /// 获得宏观指标左侧报表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public  List<StockIndicatorLeftItem> GetIndicatorLeftReport(int code)
        {
            List<StockIndicatorLeftItem> result = new List<StockIndicatorLeftItem>();
            EmQComm.HashSet<string> macroIds;
            IndicatorDataTable dataTable = (IndicatorDataTable)_dataTableCollecion["indicator"];
            if (dataTable.DicLeftIndicatorOfStock.TryGetValue(code, out macroIds))
            {
                StockIndicatorLeftItem item;
                foreach (string macroId in macroIds) 
                {
                    if (dataTable.AllLeftItems.TryGetValue(macroId, out item))
                    {
                        result.Add(item);
                    }
                }
                
            }

            return result;
        }

        /// <summary>
        /// 获得宏观指标右侧报表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public  List<StockIndicatorRightItem> GetIndicatorRightReport(int code)
        {
            List<StockIndicatorRightItem> result = new List<StockIndicatorRightItem>();
            EmQComm.HashSet<string> macroIds;
            IndicatorDataTable dataTable = (IndicatorDataTable)_dataTableCollecion["indicator"];
            if (dataTable.DicRightIndicatorOfStock.TryGetValue(code, out macroIds))
            {
                StockIndicatorRightItem item;
                foreach (string macroId in macroIds)
                {
                    if (dataTable.AllRightItems.TryGetValue(macroId, out item))
                    {
                        result.Add(item);
                    }
                }

            }

            return result;
        }
       
        #endregion
    }
}