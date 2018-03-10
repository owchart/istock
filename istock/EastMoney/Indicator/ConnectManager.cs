using System;
using System.Collections.Generic;
using System.Text;
using EmSerDataService;
using System.Threading;
using EmCore;
using EmSocketClient;

namespace OwLib
{
    public class ConnectManager
    {
        private static ConnectManager _connectManager;
        private DataQuery _dataQuery;
        private Dictionary<String, DataPacketBase> _dicMsgId;
        private String _idSend;
        private static bool _isCreating = false;
        private Thread _receivedDataPacketCallbackThread;
        private Queue<DataPacketEventArgs> _receivedDataPacketQueue;
        private Thread _sendDataPacketPushSlave;
        private Queue<DataPacketBase> _sendDataPacketQueue;
        private EventHandler<DataPacketEventArgs> dataPacketReceived;

        public event EventHandler<DataPacketEventArgs> DataPacketReceived
        {
            add
            {
                EventHandler<DataPacketEventArgs> handler2;
                EventHandler<DataPacketEventArgs> dataPacketReceived = this.dataPacketReceived;
                do
                {
                    handler2 = dataPacketReceived;
                    EventHandler<DataPacketEventArgs> handler3 = (EventHandler<DataPacketEventArgs>)Delegate.Combine(handler2, value);
                    dataPacketReceived = Interlocked.CompareExchange<EventHandler<DataPacketEventArgs>>(ref this.dataPacketReceived, handler3, handler2);
                }
                while (dataPacketReceived != handler2);
            }
            remove
            {
                EventHandler<DataPacketEventArgs> handler2;
                EventHandler<DataPacketEventArgs> dataPacketReceived = this.dataPacketReceived;
                do
                {
                    handler2 = dataPacketReceived;
                    EventHandler<DataPacketEventArgs> handler3 = (EventHandler<DataPacketEventArgs>)Delegate.Remove(handler2, value);
                    dataPacketReceived = Interlocked.CompareExchange<EventHandler<DataPacketEventArgs>>(ref this.dataPacketReceived, handler3, handler2);
                }
                while (dataPacketReceived != handler2);
            }
        }

        private ConnectManager()
        {
            this.InitQuery();
        }

        private void _OnError(DataPacketBase dataPacket)
        {
            lock (this._receivedDataPacketQueue)
            {
                this._receivedDataPacketQueue.Enqueue(new DataPacketEventArgs(dataPacket));
            }
        }

        public static ConnectManager CreateInstance()
        {
            if (_isCreating)
            {
                while (_connectManager == null)
                {
                    Thread.Sleep(10);
                }
            }
            _isCreating = true;
            if (_connectManager == null)
            {
                _connectManager = new ConnectManager();
            }
            return _connectManager;
        }

        private void DataPacketReceivedCallback()
        {
            while (true)
            {
                try
                {
                    DataPacketEventArgs e = null;
                    lock (this._receivedDataPacketQueue)
                    {
                        if (this._receivedDataPacketQueue.Count > 0)
                        {
                            e = this._receivedDataPacketQueue.Dequeue();
                        }
                    }
                    if ((e != null) && (this.dataPacketReceived != null))
                    {
                        this.dataPacketReceived(this, e);
                    }
                    Thread.Sleep(5);
                    continue;
                }
                catch (Exception exception)
                {
                    ////LogUtility.LogMessage("数据中心|请求回调报错," + exception.Message);
                    Thread.Sleep(5);
                    continue;
                }
            }
        }

        private void InitQuery()
        {
            this._dataQuery = DataCenter.DataQuery;
            this._idSend = String.Empty;
            this._dicMsgId = new Dictionary<String, DataPacketBase>(0);
            this._sendDataPacketQueue = new Queue<DataPacketBase>();
            this._sendDataPacketPushSlave = new Thread(new ThreadStart(this.PushSendDataPacket));
            this._sendDataPacketPushSlave.IsBackground = false;
            this._sendDataPacketPushSlave.Start();
            this._receivedDataPacketQueue = new Queue<DataPacketEventArgs>();
            this._receivedDataPacketCallbackThread = new Thread(new ThreadStart(this.DataPacketReceivedCallback));
            this._receivedDataPacketCallbackThread.IsBackground = true;
            this._receivedDataPacketCallbackThread.Start();
            while (!this._sendDataPacketPushSlave.IsAlive)
            {
            }
        }

        private void PushSendDataPacket()
        {
            while (true)
            {
                try
                {
                    DataPacketBase base2 = null;
                    lock (this._sendDataPacketQueue)
                    {
                        if (this._sendDataPacketQueue.Count > 0)
                        {
                            base2 = this._sendDataPacketQueue.Dequeue();
                        }
                    }
                    if ((base2 != null) && (this._dataQuery != null))
                    {
                        String str = base2.Coding();
                        ////LogUtility.LogTableMessage(String.Format("数据中心关键点|发送请求,  RequestId={0}, msgId={1}, requestCommand={2} ", base2.RequestId, base2.MsgId, str));
                        switch (base2.RequestId)
                        {
                            case RequestType.IndicatorCategory:
                            case RequestType.IndicatorEntity:
                            case RequestType.IndicatorLeaf:
                            case RequestType.IndicatorEntityList:
                                this._dataQuery.NewQueryGlobalData(str, out this._idSend, new EmSocketClient.DelegateMgr.SendBackHandle(this.SendDataCallBack));
                                break;

                            case RequestType.IndicatorData:
                                {
                                    IndicatorDataPacket2 packet = base2 as IndicatorDataPacket2;
                                    List<String> list = new List<String>();
                                    foreach (IndicatorEntity entity in packet.IndicatorList)
                                    {
                                        list.Add(entity.CategoryName);
                                    }
                                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|发送请求,  RequestId={0}, msgId={1}, 指标名称={2} ", base2.RequestId, base2.MsgId, String.Join("|", list.ToArray())));
                                    this._dataQuery.QueryIndicateStream(str, out this._idSend, new EmSocketClient.DelegateMgr.SendBackHandle(this.SendDataCallBack));
                                    break;
                                }
                            case RequestType.CustomIndicatorData:
                                {
                                    CustomIndicatorDataPacket packet2 = base2 as CustomIndicatorDataPacket;
                                    List<String> list2 = new List<String>();
                                    foreach (IndicatorEntity entity2 in packet2.IndicatorList)
                                    {
                                        list2.Add(entity2.CategoryName);
                                    }
                                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|发送请求,  RequestId={0}, msgId={1}, 指标名称={2} ", base2.RequestId, base2.MsgId, String.Join("|", list2.ToArray())));
                                    this._dataQuery.QueryIndicateStream(str, out this._idSend, new EmSocketClient.DelegateMgr.SendBackHandle(this.SendDataCallBack));
                                    break;
                                }
                            case RequestType.QuoteSeriesData:
                                {
                                    QuoteSeriesDataPacket packet3 = base2 as QuoteSeriesDataPacket;
                                    List<String> list3 = new List<String>();
                                    foreach (IndicatorEntity entity3 in packet3.IndicatorList)
                                    {
                                        list3.Add(entity3.CategoryName);
                                    }
                                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|发送请求,  RequestId={0}, msgId={1}, 指标名称={2} ", base2.RequestId, base2.MsgId, String.Join("|", list3.ToArray())));
                                    this._dataQuery.QueryIndicateStream(str, out this._idSend, new EmSocketClient.DelegateMgr.SendBackHandle(this.SendDataCallBack));
                                    break;
                                }
                            case RequestType.FinanceSeriesData:
                                {
                                    FinanceSeriesDataPacket packet4 = base2 as FinanceSeriesDataPacket;
                                    List<String> list4 = new List<String>();
                                    foreach (IndicatorEntity entity4 in packet4.IndicatorList)
                                    {
                                        list4.Add(entity4.CategoryName);
                                    }
                                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|发送请求,  RequestId={0}, msgId={1}, 指标名称={2} ", base2.RequestId, base2.MsgId, String.Join("|", list4.ToArray())));
                                    this._dataQuery.QueryIndicateStream(str, out this._idSend, new EmSocketClient.DelegateMgr.SendBackHandle(this.SendDataCallBack));
                                    break;
                                }
                            case RequestType.BlockQuoteSeriesData:
                                {
                                    BlockIndicatorDataPacket packet5 = base2 as BlockIndicatorDataPacket;
                                    List<String> list5 = new List<String>();
                                    foreach (IndicatorEntity entity5 in packet5.IndicatorList)
                                    {
                                        list5.Add(entity5.CategoryName);
                                    }
                                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|发送请求,  RequestId={0}, msgId={1}, 指标名称={2} ", base2.RequestId, base2.MsgId, String.Join("|", list5.ToArray())));
                                    this._dataQuery.QueryIndicateStream(str, out this._idSend, new EmSocketClient.DelegateMgr.SendBackHandle(this.SendDataCallBack));
                                    break;
                                }
                        }
                        lock (this._dicMsgId)
                        {
                            this._dicMsgId[this._idSend] = base2;
                        }
                    }
                    Thread.Sleep(5);
                    continue;
                }
                catch (Exception exception)
                {
                    ////LogUtility.LogTableMessage("数据中心|请求包报错," + exception.Message);
                    Thread.Sleep(5);
                    continue;
                }
            }
        }

        public void Request(List<DataPacketBase> packet)
        {
            if ((packet != null) && (packet.Count > 0))
            {
                foreach (DataPacketBase base2 in packet)
                {
                    this.Request(base2);
                }
            }
        }

        public void Request(DataPacketBase packet)
        {
            lock (this._sendDataPacketQueue)
            {
                this._sendDataPacketQueue.Enqueue(packet);
            }
        }

        private void SendDataCallBack(MessageEntity response)
        {
            String key = String.Empty;
            DataPacketBase dataPacket = null;
            try
            {
                key = response.Tag.ToString();
                if (!this._dicMsgId.ContainsKey(key))
                {
                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|数据请求失败,不存在该请求包", new object[0]));
                }
                else
                {
                    dataPacket = this._dicMsgId[key];
                    if (!response.IsSuccess)
                    {
                        ////LogUtility.LogTableMessage(String.Format("数据中心关键点|数据请求失败,RequestId={0}, msgId={1}, {2}", dataPacket.RequestId, dataPacket.MsgId, response.ServerException.ToString()));
                        this._OnError(dataPacket);
                    }
                    else
                    {
                        ////LogUtility.LogTableMessage(String.Format("数据中心关键点|收到数据响应,requestId={0},id={1}, msgId={2}", dataPacket.RequestId, key, dataPacket.MsgId));
                        byte[] msgBody = response.MsgBody as byte[];
                        dataPacket.Decoding(msgBody);
                        dataPacket.ReserveFlag = 1;
                        lock (this._dicMsgId)
                        {
                            this._dicMsgId.Remove(key);
                        }
                        lock (this._receivedDataPacketQueue)
                        {
                            this._receivedDataPacketQueue.Enqueue(new DataPacketEventArgs(dataPacket));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (dataPacket == null)
                {
                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|数据请求失败,RequestId={0}", exception.ToString()));
                }
                else
                {
                    ////LogUtility.LogTableMessage(String.Format("数据中心关键点|数据请求失败,RequestId={0}, msgId={1}, {2}", dataPacket.RequestId, dataPacket.MsgId, exception.ToString()));
                }
                lock (this._dicMsgId)
                {
                    this._dicMsgId.Remove(key);
                }
                this._OnError(dataPacket);
            }
        }
    }
}
