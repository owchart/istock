using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using EmCore;
using EmQComm;
using EmSerDataService;
using EmSocketClient;

namespace EmQTCP
{
    public class SendPacket
    {
        private byte[] _data;
        private int _msgId;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public int MsgId
        {
            get { return _msgId; }
            set { _msgId = value; }
        }
    }
   
    /// <summary>
    /// 通道
    /// </summary>
    public class DataQueryConnections
    {
        private DataQuery _dataQuery;
        private string _id;
        private Dictionary<string, int> _dicMsgId;
        private Queue<CMRecvDataEventArgs> _DataPacketQueue;
        private Thread _DataPacketPushSlave;

        private Queue<SendPacket> _SendDataPacketQueue;
        private Thread _SendDataPacketPushSlave;

        ///<summary>
        /// 收到数据包的消息处理函数
        ///</summary>
        public event EventHandler<CMRecvDataEventArgs> OnReceiveData;

        /// <summary>
        /// construct
        /// </summary>
        public DataQueryConnections(DataQuery dataQuery)
        {
            _dataQuery = dataQuery;
            _dicMsgId = new Dictionary<string, int>();
            _DataPacketQueue = new Queue<CMRecvDataEventArgs>();
            _DataPacketPushSlave = new Thread(PushDataPacket);
            _DataPacketPushSlave.IsBackground = false;
            _DataPacketPushSlave.Start();
            while (!_DataPacketPushSlave.IsAlive) ;

            _SendDataPacketQueue = new Queue<SendPacket>();
            _SendDataPacketPushSlave = new Thread(PushSendDataPacket);
            _SendDataPacketPushSlave.IsBackground = false;
            _SendDataPacketPushSlave.Start();
            while (!_SendDataPacketPushSlave.IsAlive) ;
        }

        /// <summary>
        /// 发送请求包
        /// </summary>
        /// <param name="packet"></param>
        public void DoSendPacket(byte[] packet,int msgId)
        {
            SendPacket sendPacket = new SendPacket();
            sendPacket.Data = packet;
            sendPacket.MsgId = msgId;
            lock (_SendDataPacketQueue)
                _SendDataPacketQueue.Enqueue(sendPacket);
        }

        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="response"></param>
        public void SendDataCallBack(MessageEntity response)
        {
           if(response.MsgBody is byte[])
           {
               InfoOrgBaseDataPacket dataPacket = null;
               using (MemoryStream ms = new MemoryStream(response.MsgBody as byte[]))
               {
                   lock(ms)
                   {
                       using (BinaryReader br = new BinaryReader(ms))
                       {
                           FuncTypeInfoOrg requestId = (FuncTypeInfoOrg)br.ReadByte();
                           switch (requestId)
                           {
                               case FuncTypeInfoOrg.InfoMineOrg:
                                   dataPacket = new ResInfoOrgDataPacket();
                                   break;
                               case FuncTypeInfoOrg.News24H:
                                   dataPacket = new ResNews24HOrgDataPacket();
                                   break;
                               case FuncTypeInfoOrg.ProfitForecast:
                                   dataPacket = new ResProfitForecastOrgDataPacket();
                                   break;
                               case FuncTypeInfoOrg.ImportantNews:
                                   dataPacket = new ResImportantNewsDataPacket();
                                   break;
                               case FuncTypeInfoOrg.NewsFlash:
                                   dataPacket = new ResNewsFlashDataPacket();
                                   break;
                               case FuncTypeInfoOrg.OrgRate:
                                   dataPacket = new ResInfoRateOrgDataPacket();
                                   break;
                               case FuncTypeInfoOrg.ResearchReport:
                                   dataPacket = new ResResearchReportOrgDataPacket();
                                   break;
                               case FuncTypeInfoOrg.NewInfoMineOrg:
                                   dataPacket = new ResNewInfoOrgDataPacket();
                                   break;
                               case FuncTypeInfoOrg.InfoMineOrgByIds:
                                   dataPacket = new ResInfoOrgByIdsDataPacket();
                                   break;

                           }
                           if (dataPacket != null)
                           {
                               dataPacket.RequestId = requestId;
                               if (_dicMsgId.ContainsKey((string)response.Tag))
                               {
                                   dataPacket.MsgId = _dicMsgId[(string)response.Tag];
                                   LogUtilities.LogMessage("收到响应, id=" + (string)response.Tag + ", msgId=" + dataPacket.MsgId);
                                   lock (_dicMsgId)
                                       _dicMsgId.Remove((string)response.Tag);
                               }
                               dataPacket.Decoding(br);
                           }
                       }
                   }
                  
               }
               if (dataPacket != null)
               {
                   lock (_DataPacketQueue)
                       _DataPacketQueue.Enqueue(new CMRecvDataEventArgs(TcpService.ZXCFT, dataPacket, ((byte[])response.MsgBody).Length));
               }
           }
        }

        void PushDataPacket()
        {
            while (true)
            {
                CMRecvDataEventArgs dataPacket = null;
                lock (_DataPacketQueue)
                {
                    if (_DataPacketQueue.Count > 0)
                    {
                        dataPacket = _DataPacketQueue.Dequeue();
                    }
                }
                if (dataPacket != null)
                {
                    if (OnReceiveData != null)
                        OnReceiveData(this, dataPacket);
                }
                Thread.Sleep(2);
            }
        }

        void PushSendDataPacket()
        {
            
                while (true)
                {
                    try
                    {
                        SendPacket dataPacket = null;
                        lock (_SendDataPacketQueue)
                        {
                            if (_SendDataPacketQueue.Count > 0)
                            {
                                dataPacket = _SendDataPacketQueue.Dequeue();
                            }
                        }
                        if (dataPacket != null)
                        {
                            if (_dataQuery != null)
                            {
                                _dataQuery.QueryQuoteNews(dataPacket.Data, out _id, false, SendDataCallBack);
                       
                                lock (_dicMsgId)
                                {
                                    _dicMsgId.Add(_id, dataPacket.MsgId);
                                    LogUtilities.LogMessage("发送请求, id=" + _id + ", msgId=" + dataPacket.MsgId);
                                }
                            }
                        }
                        Thread.Sleep(2);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("资讯请求报错," + e.Message);
                        Thread.Sleep(2);
                    }
                    
                }
        }
    }
}
