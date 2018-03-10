using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using EmCore;
using EmQComm;
using EmSerDataService;
using EmSocketClient;
using Timer = System.Timers.Timer;
using EastMoney.FM.Web.Data;
using EmQDataCore;

namespace EmQTCP
{
    /// <summary>
    /// 连接方式，http方式和socket方式
    /// </summary>
    public enum ConnectMode
    {
        ///<summary>
        /// HTTP连接方式
        ///</summary>
        HpptMode,

        ///<summary>
        /// TCP连接方式
        ///</summary>
        SocketMode,
    }

    /// <summary>
    /// 服务器类型
    /// </summary>
    public enum ServerMode
    {
        /// <summary>
        /// 实时行情
        /// </summary>
        RealTime,
        /// <summary>
        /// 历史数据
        /// </summary>
        History,
        /// <summary>
        /// 资讯
        /// </summary>
        Information,
        /// <summary>
        /// 外盘(海外，港股等)
        /// </summary>
        Oversea,
        /// <summary>
        /// 机构
        /// </summary>
        Org,

    }
    /// <summary>
    /// ConnectManager
    /// </summary>
    public class ConnectManager2
    {
        private static bool isCreating = false;
        private static ConnectManager2 _connectManager;
        /// <summary>
        /// 生成实例
        /// </summary>
        /// <returns></returns>
        public static ConnectManager2 CreateInstance()
        {
            if (isCreating)
            {
                while (_connectManager == null)
                {
                    Thread.Sleep(10);
                }
            }
            isCreating = true;
            if (_connectManager == null)
                _connectManager = new ConnectManager2();
            return _connectManager;
        }


        /// <summary>
        /// 连接模式
        /// </summary>
        public ConnectMode ConnectMode
        {
            get { return _connectMode; }
            set { _connectMode = value; }
        }

        /// <summary>
        /// 当收到数据后的事件
        /// </summary>
        public event EventHandler<CMRecvDataEventArgs> DoCMReceiveData;//收到数据后事件  支持HTTP和socket方式

        /// <summary>
        /// 新增了一个tcp用户事件
        /// </summary>
        public event EventHandler<ConnectEventArgs> DoAddOneClient;    //用户连接上了网络的事件 支持HTTP和socket方式

        private ConnectManager2()
        {
            _pushPackets = new Dictionary<int, DataPacket>(3);
            _sendDataPacketQueue = new Queue<DataPacket>();
            _sendDataPacketPushSlave = new Thread(PushSendDataPacket);
            _sendDataPacketPushSlave.IsBackground = false;
            _sendDataPacketPushSlave.Start();
            while (!_sendDataPacketPushSlave.IsAlive) ;
        }

        /// <summary>
        /// 建立网络连接,进行底层的连接，直到能够连接上或连接失败，如果连接失败不发消息，如果连接成功则发出连接成功的消息。
        /// </summary>
        public void DoNetConnect()
        {
            bool isTcp = true;
            if (isTcp)
                ConnectSocketServer(true);
            else
                ConnectHttpServer();
        }

        #region Http连接模块
        /// <summary>
        /// http连接的对象
        /// </summary>
        private HttpConnection _httpConnection;

        /// <summary>
        /// 连接到http服务器后产生一个连接成功的事件
        /// </summary>
        private void ConnectHttpServer()
        {
            _httpConnection = new HttpConnection();
            _httpConnection.OnConnectServSuccess += new EventHandler<EventArgs>(HttpConnectionOnConnectServSuccess);
            _httpConnection.OnReceiveData += new EventHandler<CMRecvDataEventArgs>(_httpConnection_OnReceiveData);
            // _httpConnection.Connect(httpUrl);
        }

        void _httpConnection_OnReceiveData(object sender, CMRecvDataEventArgs e)
        {

            if (SystemConfig.UserInfo.IsSingle)//有新Level2用户登入服务器，则不对行情数据进行处理
            {
                try
                {
                    if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                    {
                        DoCMReceiveData(this,
                                        new CMRecvDataEventArgs(e.ServiceType, e.DataPacket, e.Length));
                    }
                }
                catch (Exception ex)
                {
                    LogUtilities.LogMessage("Err OneTcpConnection_DoReceiveData" + ex.Message);
                }
            }
        }


        /// <summary>
        /// 连接成功的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HttpConnectionOnConnectServSuccess(object sender, EventArgs e)
        {
            if (DoAddOneClient != null)
            {
                DoAddOneClient(this, null);
            }
        }
        #endregion

        #region Socket连接模块

        private DataQuery _dataQuery;
        private DataQueryConnections _queryConnnection;
        private IndicatorQueryConnectiuon _indicatorQueryConnectiuon;

        #region DqtaQuery

        /// <summary>
        /// 统计明细url
        /// </summary>
        private string StatisticAnalyUrl = string.Format(@"http://mineapi.eastmoney.com/hq/publish/");

        private void ConnectSocketServer(bool isDataQuery)
        {
            DateTime dtStart = DateTime.Now;
            _dataQuery = DataAccess.IDataQuery;

            DataQuery.InitQuoteReceiveDataEvent(ReceiveDataCallBack);
            DataQuery.InitQuoteAddConnectHandle(ConnectCallBack);
            DataQuery.InitQuoteConnection();
            //DataQuery.InitQuoteConnectionMult();

            _queryConnnection = new DataQueryConnections(_dataQuery);
            _queryConnnection.OnReceiveData += _queryConnnection_OnReceiveData;

            _indicatorQueryConnectiuon = new IndicatorQueryConnectiuon(_dataQuery);
            _indicatorQueryConnectiuon.OnReceiveData += _indicatorQueryConnectiuon_OnReceiveData;


        }
        private Queue<DataPacket> _sendDataPacketQueue;
        private Thread _sendDataPacketPushSlave;


        public void Request(DataPacket dataPacket)
        {
            lock (_sendDataPacketQueue)
                _sendDataPacketQueue.Enqueue(dataPacket);
        }

        public void ReceiveDataCallBack(TcpService tcpService, byte[] data)
        {
            try
            {
                DataPacket dataPacket = null;
                if (tcpService == TcpService.JGFW)
                {
                    Console.WriteLine("1");
                }
                switch (tcpService)
                {
                    case TcpService.SSHQ:
                    case TcpService.LSHQ:
                    case TcpService.WPFW:
                        dataPacket = RealTimeDataPacket.DecodePacket(data, data.Length);
                        break;
                    case TcpService.DPZS:
                    case TcpService.JGFW:
                    case TcpService.GPZS:
                    case TcpService.JGLS:
                        dataPacket = OrgDataPacket.DecodePacket(data, data.Length);
                        break;
                    case TcpService.HQZX:
                        dataPacket = InfoDataPacket.DecodePacket(data, data.Length);
                        break;
                }

                if (dataPacket != null && dataPacket.IsResult)
                {
                    if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                    {
                        DoCMReceiveData(this,
                                        new CMRecvDataEventArgs(tcpService, dataPacket, data.Length));
                    }
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("ReceiveDataCallBack Error" + e.Message);
            }

        }

        bool a = false;


        private void ConnectCallBack(TcpService tcpService, object stata)
        {
            if (tcpService == TcpService.SSHQ)
            {
                if (!a)
                {
                    DataCenterCore.CreateInstance().SendLogonCft();
                    a = true;
                }
            }
            Debug.Print(tcpService.ToString() + "连接成功");
            //if (tcpService == TcpService.ALL &&  _dataQuery.IsAliving)
            if (_dataQuery.IsAliving)
            {
                //ReqHeartDataPacket packet = new ReqHeartDataPacket();
                ////Request(packet);
                //if (DoAddOneClient != null)
                //    DoAddOneClient(this, new ConnectEventArgs(tcpService, null));
            }
        }

        private Dictionary<int, DataPacket> _pushPackets;
        void PushSendDataPacket()
        {
            while (true)
            {
                DataPacket dataPacket = null;
                lock (_sendDataPacketQueue)
                {
                    if (_sendDataPacketQueue.Count > 0)
                    {
                        dataPacket = _sendDataPacketQueue.Dequeue();
                    }
                }
                try
                {
                    if (dataPacket != null)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (BinaryWriter bw = new BinaryWriter(memoryStream))
                            {
                                int len = dataPacket.CodePacket(bw);
                                if (dataPacket is RealTimeDataPacket)
                                {
                                    switch (((RealTimeDataPacket)dataPacket).RequestType)
                                    {
                                        case FuncTypeRealTime.StatisticsAnalysis:
                                            if (string.IsNullOrEmpty((dataPacket as ReqStatisticsAnalysisDataPacket).Url))
                                                break;
                                            string url = StatisticAnalyUrl + (dataPacket as ReqStatisticsAnalysisDataPacket).Url;
                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                            request.BeginGetResponse(new AsyncCallback(OnResponse), request);
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeRealTime.Heart:
                                            byte[] bytes = new byte[] {0x00,0x00,0x00,0x00,0x01,0x02,0x14,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
                                            QuoteStart.Send(TcpService.SSHQ, bytes);
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            _dataQuery.QueryQuote(TcpService.LSHQ, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeRealTime.IndexDetail:
                                        case FuncTypeRealTime.StockDetail:
                                        case FuncTypeRealTime.StockDetailLev2:
                                        case FuncTypeRealTime.StockTrend:
                                        case FuncTypeRealTime.StockTrendPush:
                                        case FuncTypeRealTime.StockTrendAskBid:
                                        case FuncTypeRealTime.StockTrendInOutDiff:
                                        case FuncTypeRealTime.IndexFuturesTrend:
                                        case FuncTypeRealTime.RedGreen:
                                        case FuncTypeRealTime.StockDict:
                                        case FuncTypeRealTime.DealSubscribe:
                                        case FuncTypeRealTime.DealRequest:
                                        case FuncTypeRealTime.BlockOverViewList:
                                        case FuncTypeRealTime.BlockSimpleQuote:
                                        case FuncTypeRealTime.BlockIndexReport:
                                        case FuncTypeRealTime.SectorQuoteReport:
                                        case FuncTypeRealTime.BlockQuoteReport:
                                        case FuncTypeRealTime.CapitalFlow:
                                        case FuncTypeRealTime.PriceStatus:
                                        case FuncTypeRealTime.Rank:
                                        case FuncTypeRealTime.TickTrade:
                                        case FuncTypeRealTime.OrderDetail:
                                        case FuncTypeRealTime.OrderQueue:
                                        case FuncTypeRealTime.ShortLineStrategy:
                                        case FuncTypeRealTime.ContributionStock:
                                        case FuncTypeRealTime.ContributionBlock:
                                        case FuncTypeRealTime.IndexFuturesDetail:
                                        case FuncTypeRealTime.MinKLine:
                                        case FuncTypeRealTime.LimitedPrice:
                                            QuoteStart.Send(TcpService.SSHQ, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeRealTime.AllOrderStockDetailLevel2:
                                        case FuncTypeRealTime.StockDetailOrderQueue:
                                        case FuncTypeRealTime.NOrderStockDetailLevel2:
                                            if (dataPacket.IsPush)
                                            {
                                                if (
                                                    _pushPackets.ContainsKey((int)((RealTimeDataPacket)dataPacket).RequestType))
                                                {
                                                    DataPacket tmp = _pushPackets[(int)((RealTimeDataPacket)dataPacket).RequestType];
                                                    tmp.IsPush = false;

                                                    using (MemoryStream memoryStream1 = new MemoryStream())
                                                    {
                                                        using (BinaryWriter bw1 = new BinaryWriter(memoryStream1))
                                                        {
                                                            tmp.CodePacket(bw1);
                                                            QuoteStart.Send(TcpService.SSHQ, memoryStream1.ToArray());
                                                            Debug.Print("SendPacket cancel " + ((RealTimeDataPacket)dataPacket).RequestType);
                                                        }
                                                    }
                                                }
                                                _pushPackets[((int)((RealTimeDataPacket)dataPacket).RequestType)]
                                                       = dataPacket;
                                                QuoteStart.Send(TcpService.SSHQ, memoryStream.ToArray());
                                                Debug.Print("SendPacket request " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            }
                                            else
                                            {
                                                if (
                                                    _pushPackets.ContainsKey(
                                                        (int)((RealTimeDataPacket)dataPacket).RequestType))
                                                    _pushPackets.Remove(
                                                        (int)((RealTimeDataPacket)dataPacket).RequestType);
                                                QuoteStart.Send(TcpService.SSHQ, memoryStream.ToArray());
                                                Debug.Print("SendPacket cancel " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            }
                                            break;
                                        case FuncTypeRealTime.TrendCapitalFlow:

                                            //using (FileStream fs = new FileStream("d:\\trendCaptial.txt", FileMode.Create))
                                            //{
                                            //    byte[] data = memoryStream.ToArray();
                                            //    fs.Write(data, 0, data.Length);
                                            //    fs.Close();

                                            //}

                                            QuoteStart.Send(TcpService.SSHQ, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeRealTime.HisTrend:
                                        case FuncTypeRealTime.HisKLine:
                                        case FuncTypeRealTime.ReqF10:
                                        case FuncTypeRealTime.HisTrendlinecfs:
                                        case FuncTypeRealTime.CapitalFlowDay:
                                            _dataQuery.QueryQuote(TcpService.LSHQ, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeRealTime.InitLogon:

                                            QuoteStart.Send(TcpService.SSHQ, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeRealTime.OceanHeart:
                                        case FuncTypeRealTime.OceanRecord:
                                        case FuncTypeRealTime.OceanTrend:
                                            _dataQuery.QueryQuote(TcpService.WPFW, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((RealTimeDataPacket)dataPacket).RequestType);
                                            break;
                                    }
                                }
                                else if (dataPacket is OrgDataPacket)
                                {
                                    switch (((OrgDataPacket)dataPacket).RequestType)
                                    {
                                        case FuncTypeOrg.HeartOrg:
                                            _dataQuery.QueryQuote(TcpService.JGFW, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((OrgDataPacket)dataPacket).RequestType);
                                            _dataQuery.QueryQuote(TcpService.DPZS, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((OrgDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeOrg.BlockReport:
                                        case FuncTypeOrg.BlockIndexReport:
                                        case FuncTypeOrg.GlobalIndexReport:
                                        case FuncTypeOrg.EmIndexReport:
                                        case FuncTypeOrg.BlockStockReport:
                                        case FuncTypeOrg.HKStockReport:
                                        case FuncTypeOrg.FundStockReport:
                                        case FuncTypeOrg.BondStockReport:
                                        case FuncTypeOrg.FuturesStockReport:
                                        case FuncTypeOrg.IndexFuturesReport:
                                        case FuncTypeOrg.CustomReport:
                                        case FuncTypeOrg.InitReportData:
                                        case FuncTypeOrg.RateReport:
                                        case FuncTypeOrg.FinanceReport:
                                        case FuncTypeOrg.FinanceOrg:
                                        case FuncTypeOrg.DDEReport:
                                        case FuncTypeOrg.CapitalFlowReport:
                                        case FuncTypeOrg.NetInFlowReport:
                                        case FuncTypeOrg.ProfitForecastReport:
                                        case FuncTypeOrg.CustomDDEReport:
                                        case FuncTypeOrg.CustomCapitalFlowReport:
                                        case FuncTypeOrg.CustomNetInFlowReport:
                                        case FuncTypeOrg.CustomProfitForecastReport:
                                        case FuncTypeOrg.Rank:
                                        case FuncTypeOrg.NetInFlowRank:
                                        case FuncTypeOrg.OSFuturesReport:
                                        case FuncTypeOrg.OSFuturesReportNew:
                                        case FuncTypeOrg.OsFuturesLMEReport:
                                        case FuncTypeOrg.ForexReport:
                                        case FuncTypeOrg.USStockReport:
                                        case FuncTypeOrg.FinanceStockReport:
                                        case FuncTypeOrg.CustomFinanceStockReport:
                                        case FuncTypeOrg.BondPublicOpeartion:
                                        case FuncTypeOrg.ChangeName:
                                            _dataQuery.QueryQuote(TcpService.JGFW, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((OrgDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeOrg.MonetaryFundDetail:
                                        case FuncTypeOrg.NonMonetaryFundDetail:
                                        case FuncTypeOrg.FundTrpAndSunDetail:
                                        case FuncTypeOrg.FundCIPMonetaryDetail:
                                        case FuncTypeOrg.FundCIPNonMonetaryDetail:
                                        case FuncTypeOrg.FundBFPDetail:
                                        case FuncTypeOrg.FundHeaveStockReport:
                                        case FuncTypeOrg.FundHYReport:
                                        case FuncTypeOrg.FundKeyBondReport:
                                        case FuncTypeOrg.FundManager:
                                        case FuncTypeOrg.FinanceHeaveFundReport:
                                        case FuncTypeOrg.FinanceHeaveStockReport:
                                        case FuncTypeOrg.FinanceHeaveHYReport:
                                        case FuncTypeOrg.FinanceHeaveBondReport:
                                        case FuncTypeOrg.FinanceHeaveManagerReport:
                                        case FuncTypeOrg.CNIndexDetail:
                                        case FuncTypeOrg.CSIIndexDetail:
                                        case FuncTypeOrg.CSIndexDetail:
                                        case FuncTypeOrg.GlobalIndexDetail:
                                        case FuncTypeOrg.InterBankDetail:
                                        case FuncTypeOrg.RateSwapDetail:
                                        case FuncTypeOrg.InterBankRepurchaseDetail:
                                        case FuncTypeOrg.ShiborDetail:
                                        case FuncTypeOrg.TrendOrgDP:
                                        case FuncTypeOrg.MinKLineOrgDP:
                                        case FuncTypeOrg.LowFrequencyTBY:
                                        case FuncTypeOrg.BankBondReport:
                                        case FuncTypeOrg.ShiborReport:
                                            _dataQuery.QueryQuote(TcpService.DPZS, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((OrgDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeOrg.DepthAnalyse:
                                        case FuncTypeOrg.EMIndexDetail:
                                        case FuncTypeOrg.MinKLineOrg:
                                        case FuncTypeOrg.TrendOrg:
                                        case FuncTypeOrg.IndexStatic:
                                        case FuncTypeOrg.USStockDetail:
                                        case FuncTypeOrg.OSFuturesDetail:
                                        case FuncTypeOrg.ForexDetail:
                                        case FuncTypeOrg.ConvertBondDetail:
                                        case FuncTypeOrg.NonConvertBondDetail:
                                        case FuncTypeOrg.OSFuturesLMEDetail:
                                        case FuncTypeOrg.OSFuturesLMEDeal:
                                        case FuncTypeOrg.NewProfitForcast:
                                            _dataQuery.QueryQuote(TcpService.GPZS, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((OrgDataPacket)dataPacket).RequestType);
                                            break;
                                        case FuncTypeOrg.TradeDate:
                                        case FuncTypeOrg.HisKLineOrg:
                                        case FuncTypeOrg.FundKlineAfterDivide:
                                        case FuncTypeOrg.DivideRightOrg:
                                            _dataQuery.QueryQuote(TcpService.JGLS, memoryStream.ToArray());
                                            Debug.Print("SendPacket " + ((OrgDataPacket)dataPacket).RequestType);
                                            break;
                                    }
                                }
                                else if (dataPacket is InfoOrgBaseDataPacket)
                                {
                                    _queryConnnection.DoSendPacket(
                                        ((InfoOrgBaseDataPacket)dataPacket).CodeInfoPacket(), dataPacket.MsgId);
                                    Debug.Print("SendPacket " + ((InfoOrgBaseDataPacket)dataPacket).RequestId);
                                }
                                else if (dataPacket is IndicatorDataPacket)
                                {
                                    IndicatorDataPacket iPacket = dataPacket as IndicatorDataPacket;
                                    _indicatorQueryConnectiuon.DoSendPacket(iPacket.CreateCommand(), 
                                        dataPacket.MsgId);
                                    Debug.Print("SendPacket " + ((IndicatorDataPacket)dataPacket).RequestId);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
                Thread.Sleep(2);
            }
        }

        void OnResponse(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            Stream stream = response.GetResponseStream();
            bool result = false;
            ResStatisticsAnalysisDataPacket packet = null;

            string[] urlParams = response.ResponseUri.Segments;
            if (urlParams.Length < 2)
                return;
            string shortcode = urlParams[urlParams.Length - 1];
            string markt = urlParams[urlParams.Length - 2];
            string emcode = string.Format(@"{0}.{1}", shortcode.Substring(0, shortcode.Length - 4),
                markt.Substring(0, markt.Length - 1).ToUpper());
            if (!DetailData.EmCodeToUnicode.ContainsKey(emcode))
                return;

            if (stream != null)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    packet = new ResStatisticsAnalysisDataPacket();
                    packet.RequestType = FuncTypeRealTime.StatisticsAnalysis;
                    result = packet.Decoding(br);
                    packet.Code = DetailData.EmCodeToUnicode[emcode];
                }
                stream.Dispose();
            }

            if (result)
            {
                if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                {
                    DoCMReceiveData(this,
                                    new CMRecvDataEventArgs(TcpService.SSHQ, packet, 0));
                }
            }
        }


        #endregion
        /// <summary>
        /// 建立socket连接
        /// </summary>
        private void ConnectSocketServer()
        {
            DateTime dtStart = DateTime.Now;
            _dataQuery = DataAccess.IDataQuery;
            _heartRealTime = new ReqHeartDataPacket();
            _heartInfo = new ReqInfoHeart();
            _heartOrg = new ReqHeartOrgDataPacket();
            _heartOcean = new ReqOceanHeartDataPacket();
            SocketConnections = new Dictionary<TcpService, SocketConnection>(5);
            CreateRealTimeSocket();
            CreateHistorySocket();
            //CreateInfoSocket();
            CreateOceanSocket();
            CreateOrgSocket();
            CreateLowsOrgSocket();
            Timer timerCheck = new Timer(65000);
            timerCheck.Elapsed += timerCheck_Elapsed;
            timerCheck.Start();
            _queryConnnection = new DataQueryConnections(_dataQuery);
            _queryConnnection.OnReceiveData += _queryConnnection_OnReceiveData;
            _indicatorQueryConnectiuon.OnReceiveData += _indicatorQueryConnectiuon_OnReceiveData;
            //TcpConnections = new Dictionary<short, TcpConnection>(5);
            //CreateRealTimeTcp();
            //CreateHistoryTcp();
            //CreateInfoTcp();
            //CreateOrgTcp();
            //CreateOceanTcp();

            TimeSpan ts = DateTime.Now - dtStart;
            LogUtilities.LogMessage("连接服务器总共用时:" + ts.TotalMilliseconds);
        }



        void timerCheck_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SocketConnections != null)
            {
                foreach (KeyValuePair<TcpService, SocketConnection> connect in SocketConnections)
                {
                    if (!connect.Value.CheckHeart())
                    {
                        MessageBox.Show(connect.ToString() + "断开链接");
                        connect.Value.ReConnect();
                    }

                }
            }
        }


        #region tcp连接测试服务器
        private Dictionary<short, TcpConnection> TcpConnections;

        //202.104.236.15：1865
        //杭州 115.236.99.43 1865
        //我们自己的服务器 115.236.102.248 1865

        private static string IpServerRealTime = "202.104.236.15";
        private static int PortServerRealTime = 1865;

        //72--测试
        //44--相对稳定
        //正式202.104.236.245 1861
        private static string IpServerHistory = "202.104.236.245";
        private static int PortServerHistory = 1861;

        //202.104.236.68:80----临时
        //115.236.99.52:80----正式
        private static string IpServerInfo = "115.236.99.52";
        private static int PortServerInfo = 80;

        //外盘 弃用202.104.236.205 1860
        //调试122.224.82.152  7788
        //测试 202.104.236.242 1860
        private static string IpServerOcean = "202.104.236.242";
        private static int PortServerOcean = 1860;

        //机构版服务器
        //172.16.10.52：1860 科宇
        //115.236.103.78 1860
        //172.16.10.54 存超
        private static string IpServerOrg = "115.236.103.78";
        private static int PortServerOrg = 1870;

        private ReqHeartDataPacket _heartRealTime;
        private ReqInfoHeart _heartInfo;
        private ReqHeartOrgDataPacket _heartOrg;
        private ReqOceanHeartDataPacket _heartOcean;
        private System.Timers.Timer _timerRealTime;
        private System.Timers.Timer _timeInfo;
        private System.Timers.Timer _timeOcean;
        private System.Timers.Timer _timeOrg;

        /// <summary>
        /// GetRealTimeAp
        /// </summary>
        public IpAddressPort GetRealTimeAp()
        {
            return new IpAddressPort(IpServerRealTime, PortServerRealTime);
        }
        /// <summary>
        /// CreateRealTimeTcp
        /// </summary>
        public void CreateRealTimeTcp()
        {

            TcpConnection tcpRealTime = new TcpConnection();
            tcpRealTime.SerMode = ServerMode.RealTime;
            tcpRealTime.OnConnectServSuccess += tcpRealTime_OnConnectServSuccess;
            tcpRealTime.OnReceiveData += tcpHistory_OnReceiveData;
            tcpRealTime.Connect(new IpAddressPort(IpServerRealTime, PortServerRealTime));
            TcpConnections.Add((short)ServerMode.RealTime, tcpRealTime);
            Request(_heartRealTime);
            if (_timerRealTime == null)
            {
                _timerRealTime = new System.Timers.Timer(60000);
                _timerRealTime.Elapsed += _timerRealTime_Elapsed;
                _timerRealTime.AutoReset = true;
                _timerRealTime.Enabled = true;
                _timerRealTime.Start();
            }
        }



        /// <summary>
        /// CreateOceanTcp
        /// </summary>
        public void CreateOceanTcp()
        {
            TcpConnection tcpOcean = new TcpConnection();
            tcpOcean.SerMode = ServerMode.Oversea;
            tcpOcean.OnConnectServSuccess += tcpOcean_OnConnectServSuccess;
            tcpOcean.OnReceiveData += tcpHistory_OnReceiveData;
            tcpOcean.Connect(new IpAddressPort(IpServerOcean, PortServerOcean));
            TcpConnections.Add((short)ServerMode.Oversea, tcpOcean);

            ReqLogonDataPacket packet = new ReqLogonDataPacket();
            Request(packet);
            Request(_heartRealTime);
            if (_timerRealTime == null)
            {
                _timerRealTime = new System.Timers.Timer(60000);
                _timerRealTime.Elapsed += _timerRealTime_Elapsed;
                _timerRealTime.AutoReset = true;
                _timerRealTime.Enabled = true;
                _timerRealTime.Start();
            }
        }

        private void CreateHistoryTcp()
        {
            TcpConnection tcpHistory = new TcpConnection();
            tcpHistory.SerMode = ServerMode.History;
            tcpHistory.OnConnectServSuccess += tcpHistory_OnConnectServSuccess;
            tcpHistory.OnReceiveData += tcpHistory_OnReceiveData;
            tcpHistory.Connect(new IpAddressPort(IpServerHistory, PortServerHistory));
            TcpConnections.Add((short)ServerMode.History, tcpHistory);
            Request(_heartRealTime);
            //if (_timerRealTime == null)
            //{
            //    _timerRealTime = new System.Timers.Timer(60000);
            //    _timerRealTime.Elapsed += _timerRealTime_Elapsed;
            //    _timerRealTime.Start();
            //}
        }

        private void CreateInfoTcp()
        {
            TcpConnection tcpInfo = new TcpConnection();
            tcpInfo.SerMode = ServerMode.Information;
            tcpInfo.OnConnectServSuccess += new EventHandler<ConnectEventArgs>(tcpInfo_OnConnectServSuccess);
            tcpInfo.OnReceiveData += tcpHistory_OnReceiveData;
            tcpInfo.Connect(new IpAddressPort(IpServerInfo, PortServerInfo));
            TcpConnections.Add((short)ServerMode.Information, tcpInfo);
            Request(_heartInfo);
            if (_timeInfo == null)
            {
                _timeInfo = new System.Timers.Timer(60000);
                _timeInfo.Elapsed += _timeInfo_Elapsed;
                _timeInfo.Start();
            }
        }

        private void CreateOrgTcp()
        {
            TcpConnection tcpOrg = new TcpConnection();
            tcpOrg.SerMode = ServerMode.Org;
            tcpOrg.OnConnectServSuccess += tcpOrg_OnConnectServSuccess;
            tcpOrg.OnReceiveData += tcpHistory_OnReceiveData;
            tcpOrg.Connect(new IpAddressPort(IpServerOrg, PortServerOrg));
            TcpConnections.Add((short)ServerMode.Org, tcpOrg);
            Request(_heartOrg);
            if (_timeOrg == null)
            {
                _timeOrg = new System.Timers.Timer(40000);
                _timeOrg.Elapsed += _timeOrg_Elapsed;
                _timeOrg.Start();
            }
        }

        void _timerRealTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            Request(_heartRealTime);
        }

        void _timeInfo_Elapsed(object sender, ElapsedEventArgs e)
        {
            Request(_heartInfo);
        }

        void _timeOrg_Elapsed(object sender, ElapsedEventArgs e)
        {
            Request(_heartOrg);
        }

        void _timerOcean_Elapsed(object sender, ElapsedEventArgs e)
        {
            Request(_heartOcean);
        }

        void tcpHistory_OnReceiveData(object sender, CMRecvDataEventArgs e)
        {
            try
            {
                if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                {
                    DoCMReceiveData(this,
                                    new CMRecvDataEventArgs(e.ServiceType, e.DataPacket, e.Length));
                }
            }
            catch (Exception ex)
            {

                LogUtilities.LogMessage("Err OneTcpConnection_DoReceiveData" + ex.Message);
            }
        }


        void tcpOrg_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("机构行情服务器连接成功!");
            LogUtilities.LogMessage("机构行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }


        void tcpOcean_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("外盘行情服务器连接成功!");
            LogUtilities.LogMessage("外盘行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }


        void tcpInfo_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("资讯行情服务器连接成功!");
            LogUtilities.LogMessage("资讯数据服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        void tcpHistory_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("历史行情服务器连接成功!");
            LogUtilities.LogMessage("历史数据服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }


        void tcpRealTime_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("实时行情服务器连接成功!");
            LogUtilities.LogMessage("实时行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        #endregion

        #region socket连接服务器

        private Dictionary<TcpService, SocketConnection> SocketConnections;

        public void CreateRealTimeSocket()
        {
            SocketConnection skRealTime = new SocketConnection(_dataQuery, TcpService.SSHQ);
            skRealTime.OnConnectServSuccess += skRealTime_OnConnectServSuccess;
            skRealTime.OnReceiveData += skRealTime_OnReceiveData;
            skRealTime.Connect();
            SocketConnections[TcpService.SSHQ] = skRealTime;
            Request(_heartRealTime);
            if (_timerRealTime == null)
            {
                _timerRealTime = new System.Timers.Timer(60000);
                _timerRealTime.Elapsed += _timerRealTime_Elapsed;
                _timerRealTime.AutoReset = true;
                _timerRealTime.Start();
            }
        }

        public void CreateHistorySocket()
        {
            SocketConnection skHistory = new SocketConnection(_dataQuery, TcpService.LSHQ);
            skHistory.OnConnectServSuccess += skHistory_OnConnectServSuccess;
            skHistory.OnReceiveData += skRealTime_OnReceiveData;
            skHistory.Connect();
            SocketConnections[TcpService.LSHQ] = skHistory;
            Request(_heartRealTime);
        }

        public void CreateOceanSocket()
        {
            SocketConnection skOcean = new SocketConnection(_dataQuery, TcpService.WPFW);
            skOcean.OnConnectServSuccess += skOcean_OnConnectServSuccess;
            skOcean.OnReceiveData += skRealTime_OnReceiveData;
            skOcean.Connect();
            SocketConnections[TcpService.WPFW] = skOcean;
            ReqLogonDataPacket packet = new ReqLogonDataPacket();
            Request(packet);
            Request(_heartOcean);
            if (_timeOcean == null)
            {
                _timeOcean = new System.Timers.Timer(60000);
                _timeOcean.Elapsed += _timerOcean_Elapsed;
                _timeOcean.Start();
            }
        }

        public void CreateInfoSocket()
        {
            SocketConnection skInfo = new SocketConnection(_dataQuery, TcpService.ZXCFT);
            skInfo.OnConnectServSuccess += skInfo_OnConnectServSuccess;
            skInfo.OnReceiveData += skRealTime_OnReceiveData;
            skInfo.Connect();
            SocketConnections[TcpService.ZXCFT] = skInfo;
            Request(_heartInfo);
            if (_timeInfo == null)
            {
                _timeInfo = new System.Timers.Timer(60000);
                _timeInfo.Elapsed += _timeInfo_Elapsed;
                _timeInfo.Start();
            }
        }

        public void CreateOrgSocket()
        {
            SocketConnection skOrg = new SocketConnection(_dataQuery, TcpService.JGFW);
            skOrg.OnConnectServSuccess += skOrg_OnConnectServSuccess;
            skOrg.OnReceiveData += skRealTime_OnReceiveData;
            skOrg.Connect();
            SocketConnections[TcpService.JGFW] = skOrg;
            Request(_heartOrg);
            if (_timeOrg == null)
            {
                _timeOrg = new System.Timers.Timer(40000);
                _timeOrg.Elapsed += _timeOrg_Elapsed;
                _timeOrg.Start();
            }
        }

        public void CreateLowsOrgSocket()
        {
            SocketConnection skLowsOrg = new SocketConnection(_dataQuery, TcpService.DPZS);
            skLowsOrg.OnConnectServSuccess += skLowsOrg_OnConnectServSuccess;
            skLowsOrg.OnReceiveData += skRealTime_OnReceiveData;
            skLowsOrg.Connect();
            SocketConnections[TcpService.DPZS] = skLowsOrg;
            Request(_heartOrg);
            if (_timeOrg == null)
            {
                _timeOrg = new System.Timers.Timer(40000);
                _timeOrg.Elapsed += _timeOrg_Elapsed;
                _timeOrg.Start();
            }
        }

        void _queryConnnection_OnReceiveData(object sender, CMRecvDataEventArgs e)
        {
            try
            {
                if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                {
                    DoCMReceiveData(this,
                                    new CMRecvDataEventArgs(e.ServiceType, e.DataPacket, e.Length));
                }
            }
            catch (Exception ex)
            {

                LogUtilities.LogMessage("Err OneTcpConnection_DoReceiveData" + ex.Message);
            }
        }
        void _indicatorQueryConnectiuon_OnReceiveData(object sender, CMRecvDataEventArgs e)
        {
            try
            {
                if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                {
                    DoCMReceiveData(this,
                                    new CMRecvDataEventArgs(e.ServiceType, e.DataPacket, e.Length));
                }
            }
            catch (Exception ex)
            {

                LogUtilities.LogMessage("Err _indicatorQueryConnectiuon_OnReceiveData" + ex.Message);
            }
        }
        void skRealTime_OnReceiveData(object sender, CMRecvDataEventArgs e)
        {
            try
            {
                if (DoCMReceiveData != null) //通知界面而已，对于数据的响应在这个类中完成。
                {
                    DoCMReceiveData(this,
                                    new CMRecvDataEventArgs(e.ServiceType, e.DataPacket, e.Length));
                }
            }
            catch (Exception ex)
            {

                LogUtilities.LogMessage("Err OneTcpConnection_DoReceiveData" + ex.Message);
            }
        }

        void skRealTime_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("实时行情服务器连接成功!");
            LogUtilities.LogMessage("实时行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        void skHistory_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("历史行情服务器连接成功!");
            LogUtilities.LogMessage("历史行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        void skOcean_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("外盘行情服务器连接成功!");
            LogUtilities.LogMessage("外盘行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        void skInfo_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("资讯行情服务器连接成功!");
            LogUtilities.LogMessage("资讯行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        void skOrg_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("机构行情服务器连接成功!");
            LogUtilities.LogMessage("机构行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        void skLowsOrg_OnConnectServSuccess(object sender, ConnectEventArgs e)
        {
            Debug.Print("低频行情服务器连接成功!");
            LogUtilities.LogMessage("低频行情服务器连接成功！");
            if (DoAddOneClient != null)
                DoAddOneClient(this, e);
        }

        /// <summary>
        /// 收到心跳
        /// </summary>
        /// <param name="tcpService"></param>
        public void RecHeart(TcpService tcpService)
        {
            if (SocketConnections != null && SocketConnections.ContainsKey(tcpService))
            {
                SocketConnections[tcpService].RecHeart();
            }
        }
        #endregion      }

        /// <summary>
        /// DicSectorQuoteReport
        /// </summary>
        public Dictionary<int, ReqSecurityType> DicSectorQuoteReport = new Dictionary<int, ReqSecurityType>();

        private ConnectMode _connectMode;

        /// <summary>
        /// 发送多个数据包
        /// </summary>
        /// <param name="packets"></param>
        public void Request(List<DataPacket> packets)
        {
            if (packets == null || packets.Count <= 0)
                return;
            try
            {
                foreach (DataPacket dataPacket in packets)
                    Request(dataPacket);
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage(ex.Message);
            }
        }

        #endregion
    }
}
