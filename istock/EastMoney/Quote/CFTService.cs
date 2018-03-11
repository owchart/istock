using System;
using System.Collections.Generic;
using System.Diagnostics;
 
using System.Text;
using EmCore;
using System.Threading;
using EmSocketClient;
using System.Net;
using System.Net.Sockets;

namespace OwLib
{
    /// <summary>
    /// 程序启动时，行情需要执行的动作
    /// </summary>
    public class CFTService
    {
        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
            try
            {
                LogUtilities.LogMessage("行情开始加载");
                DateTime dtStart = DateTime.Now;
                ConnectManager2.CreateInstance().DoNetConnect();
                DataCenterCore dc = DataCenterCore.CreateInstance();
                Thread.Sleep(2000);
                //dc.TimerStart();
                //dc.DoTimerElapsed();


                //var codes = dc.GetQuoteCodeList(new List<string>() {"600000.SH"}, FieldIndex.Code);
                //订阅一个指数行情

                //……
            }
            catch (Exception e)
            {
                LoggerHelper.Log.Debug(e.Message + "\r\n" + e.StackTrace);
            }

        }

        private static Socket clientSocket;

        public static void Send(TcpService tcpService, byte[] bytes)
        {
            if (clientSocket != null)
            {
                clientSocket.Send(bytes);
            }
            else
            {
                DataAccess.IDataQuery.QueryQuote(tcpService,  bytes);
            }
        }

        private static void LoadEM()
        {
            int port = 1860;
            string host = "114.80.230.29";//服务器端ip地址

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);
            //send message   
            byte[] sendBytes = new byte[]
                                            { 
                                                0x96,0x00,0x00,0x00,0xC7,0x02,0xB8,0x03,0x1F,0x00,0x00,0x00,0x00,0x00,0x02,0x11,0x00,0x00,0x0B,0x01,0x00,0x00,0x00,0xFB,0xFF,0xFF,0xFF,0x73,0x9E,0x18,0xEB,0x3F,0xB3,0x4C,0x13,0x66,0x84,0x95,0x06,0x8B,0x19,0xAE,0xB4,0x28,0x08,0xC2,0x1C,0x77,0xA9,0x6B,0xCB,0xD8,0x64,0x87,0x4E,0x9F,0x63,0xE6,0xDF,0xF3,0x08,0xA2,0x99,0x0A,0x3A,0x29,0x86,0xF5,0x54,0x4E,0x6D,0xB3,0x53,0x5E,0x56,0xC1,0x85,0x8C,0x2A,0x28,0x08,0xC2,0x1C,0x77,0xA9,0x6B,0xCB,0xB2,0x93,0x90,0x01,0x64,0x8C,0xD4,0x3C,0x00,0x00,0x00,0x00,0x00,0x32,0x30,0x31,0x33,0x31,0x30,0x30,0x33,0x7B,0x39,0x35,0x34,0x32,0x41,0x33,0x41,0x33,0x2D,0x33,0x44,0x31,0x35,0x2D,0x34,0x43,0x38,0x44,0x2D,0x42,0x32,0x41,0x32,0x2D,0x44,0x35,0x46,0x35,0x33,0x44,0x31,0x41,0x31,0x46,0x30,0x30,0x7D,0x7C,0x31,0x32,0x38,0x30,0x2A,0x38,0x30,0x30,0x7C,0x30,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                                            };
            clientSocket.Send(sendBytes);
            //receive message
            while (true)
            {
                byte[] recBytes = new byte[102400];
                int len = clientSocket.Receive(recBytes, recBytes.Length, 0);
                ConnectManager2.CreateInstance().ReceiveDataCallBack(TcpService.SSHQ, recBytes);
            }
        }
        /// <summary>
        /// Stop
        /// </summary>
        public static void Stop()
        {
            
        }

        /// <summary>
        /// K线
        /// </summary>
        /// <param name="code"></param>
        public static void QueryHistoryDatas(String code, KLineCycle cycle)
        {
            if (EMSecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = EMSecurityService.KwItems[code];
                int innerCode = item.Innercode;
                //财富通历史数据
                OneStockHisKLineData oneStockHisKLineData = new OneStockHisKLineData(innerCode,
                    cycle, 0, 0);
                oneStockHisKLineData._reqKLineDataRange = ReqKLineDataRange.All;
                oneStockHisKLineData.Start();
            }   
        }

        /// <summary>
        /// 指数
        /// </summary>
        public static void QueryIndexDetail()
        {
            List<int> codes = new List<int>();
            codes.Add(EMSecurityService.KwItems["000001.SH"].Innercode);
            IndexDetailData indexDetailData = new IndexDetailData(codes);
            indexDetailData.Start();
        }

        /// <summary>
        /// 断线精灵
        /// </summary>
        public static void QueryShortLineStrategy()
        {
            ReqShortLineStrategyDataPacket reqShortLine = new ReqShortLineStrategyDataPacket();
            ConnectManager2.CreateInstance().Request(reqShortLine);
        }

        /// <summary>
        /// 趋势线
        /// </summary>
        /// <param name="code"></param>
        public static void QueryTrendLine(String code)
        {
            if (EMSecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = EMSecurityService.KwItems[code];
                int innerCode = item.Innercode;
                TrendData trendData = new TrendData();
                trendData.Code = innerCode;
                trendData.Start();
            }
        }

        /// <summary>
        /// N档行情
        /// </summary>
        /// <param name="code">代码</param>
        public static void QueryNQuote(String code)
        {
            if (EMSecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = EMSecurityService.KwItems[code];
                int innerCode = item.Innercode;
                ReqNOrderStockDetailLevel2DataPacket req = new ReqNOrderStockDetailLevel2DataPacket();
                req.Code = innerCode;
                ConnectManager2.CreateInstance().Request(req);
            }
        }

        /// <summary>
        /// 报价列表
        /// </summary>
        public static void QueryCustomReport()
        {
            ReqCustomReportOrgDataPacket reqCustom = new ReqCustomReportOrgDataPacket();
            reqCustom.FieldFlag = 1;
            reqCustom.FieldIndexList = new List<short>(2);
            reqCustom.FieldIndexList.Add(0x1f42);
            reqCustom.FieldIndexList.Add(0x1f6c);
            reqCustom.CustomCodeList = new List<int>();
            int count = 0;
            foreach (int key in EMSecurityService.KwItems2.Keys)
            {
                reqCustom.CustomCodeList.Add(key);
                count++;
                if (count > 100)
                {
                    break;
                }
            }
            ConnectManager2.CreateInstance().Request(reqCustom);
        }

        /// <summary>
        /// 查询LV1行情
        /// </summary>
        /// <param name="code"></param>
        public static void QueryLV1(String code)
        {
            if (EMSecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = EMSecurityService.KwItems[code];
                int innerCode = item.Innercode;
                OneStockDetailData detailDate = new OneStockDetailData();
                detailDate.Code = innerCode;
                detailDate.Start();
            }
        }

        /// <summary>
        /// 查询LV2行情
        /// </summary>
        /// <param name="code"></param>
        public static void QueryLV2(String code)
        {
            if (EMSecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = EMSecurityService.KwItems[code];
                int innerCode = item.Innercode;
                OneStockDetailData detailDate = new OneStockDetailData();
                detailDate.Code = innerCode;
                detailDate.Start();
            }
        }

        /// <summary>
        /// 查询百档行情
        /// </summary>
        /// <param name="code"></param>
        public static void QueryN100(String code)
        {
            if (EMSecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = EMSecurityService.KwItems[code];
                int innerCode = item.Innercode;
                ReqAllOrderStockDetailLev2DataPacket req = new ReqAllOrderStockDetailLev2DataPacket();
                req.Code = innerCode;
                ConnectManager2.CreateInstance().Request(req);
            }
        }

        /// <summary>
        /// 数据回调
        /// </summary>
        /// <param name="funcType"></param>
        /// <param name="data"></param>
        public static void CallBack(FuncTypeRealTime funcType, Object data)
        {
        }
    }
}
