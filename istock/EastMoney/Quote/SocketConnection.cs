using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EmCore;
using EmSerDataService;
using EmSocketClient;

namespace OwLib
{
    public class SocketConnection
    {
        Queue<CMRecvDataEventArgs> _DataPacketQueue;
        Thread _DataPacketPushSlave;

        ///<summary>
        /// 收到数据包的消息处理函数
        ///</summary>
        public event EventHandler<CMRecvDataEventArgs> OnReceiveData;

        ///<summary>
        /// 连接服务器成功的消息处理函数
        ///</summary>
        public event EventHandler<ConnectEventArgs> OnConnectServSuccess;


        ///<summary>
        /// 是否连接成功
        ///</summary>
        public bool HaveConnected
        {
            get
            {
                if (_socket != null)
                    return _socket.Connected;
                return false;
            }
        }


        private Socket _socket;
        private DataQuery _dataQuery;
        private TcpService _tcpService;
        private byte[] buffer;
        private const int _bufferSize = 1024 * 10;

        /// <summary>
        /// 是否收到心跳
        /// </summary>
        private bool _isLive = false;

        /// <summary>
        /// 服务器类型
        /// </summary>
        public TcpService ServerType { get { return _tcpService; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SocketConnection(DataQuery dq, TcpService ts)
        {
            _dataQuery = dq;
            _tcpService = ts;

            _DataPacketQueue = new Queue<CMRecvDataEventArgs>();
            _DataPacketPushSlave = new Thread(PushDataPacket);
            _DataPacketPushSlave.IsBackground = false;
            _DataPacketPushSlave.Start();
            while (!_DataPacketPushSlave.IsAlive) ;
        }

        /// <summary>
        /// 收到心跳
        /// </summary>
        public void RecHeart()
        {
            _isLive = true;
        }

        /// <summary>
        /// 检测
        /// </summary>
        /// <returns></returns>
        public bool CheckHeart()
        {
            if (_isLive)
            {
                _isLive = false;
                return true;
            }
            return false;
        }

        #region 连接

        public void Connect()
        {
            try
            {
                _socket = _dataQuery.ConnectService(_tcpService);
                if (_socket.Connected)
                {
                    buffer = new byte[_bufferSize];
                    _socket.BeginReceive(buffer, 0, _bufferSize, 0, new AsyncCallback(RecvEndCallBack), buffer);

                    if (OnConnectServSuccess != null)
                        OnConnectServSuccess(this, new ConnectEventArgs(_tcpService, this));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_tcpService.ToString() + "断开链接");
                //ReConnect();
            }

        }

        /// <summary>
        /// 重连
        /// </summary>
        public void ReConnect()
        {
            try
            {
                _dataQuery.ReConnectService(_tcpService, ref _socket);
                if (_socket.Connected)
                {
                    //sbuffer = new byte[_bufferSize];
                    _socket.BeginReceive(buffer, 0, _bufferSize, 0, new AsyncCallback(RecvEndCallBack), buffer);

                    if (OnConnectServSuccess != null)
                        OnConnectServSuccess(this, new ConnectEventArgs(_tcpService, this));
                }
            }
            catch (Exception)
            {

                //ReConnect();
            }

        }

        static Mutex _mutex = new Mutex();
        void RecvEndCallBack(IAsyncResult iAsyncResult)
        {
            try
            {
                int readBytes = _socket.EndReceive(iAsyncResult);
                if (readBytes > 0)
                {
                    _mutex.WaitOne();
                    try
                    {
                        RecvStreamEventArgs e = new RecvStreamEventArgs(
                                    (byte[])
                                    iAsyncResult.AsyncState,
                                    readBytes);

                        switch (_tcpService)
                        {
                            case TcpService.SSHQ:
                            case TcpService.LSHQ:
                            case TcpService.WPFW:
                                RecvRealTimeData(this, e);
                                break;
                            case TcpService.ZXCFT:
                                RecvInfoData(this, e);
                                break;
                            case TcpService.JGFW:
                            case TcpService.DPZS:
                              RecvOrgData(this, e);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtilities.LogMessage(ex.Message);
                    }
                    finally
                    {
                        _mutex.ReleaseMutex();
                    }

                    _socket.BeginReceive((byte[])iAsyncResult.AsyncState, 0, _bufferSize, 0, new AsyncCallback(RecvEndCallBack), iAsyncResult.AsyncState);
                }
                //else if (readBytes == 0)
                //{
                //    ReConnect();
                //}
            }
            catch (Exception ex)
            {
                //if(ex is SocketException)
                //{
                //    if((ex as SocketException).ErrorCode == 10060)
                //    {
                //        ReConnect();
                //    }
                //}

                LogUtilities.LogMessage(_tcpService.ToString() + ex.Message);
            }
        }

        #endregion

        #region 发送数据包
        /// <summary>
        /// 发送一个数据请求包
        /// </summary>
        /// <param name="dataPacket">要发送的一个数据包</param>
        /// <returns>发送数据的长度</returns>
        public long DoSendPacket(DataPacket dataPacket)
        {
            if (dataPacket == null)
                return 0;

            long sendBytes = 0;
            _socket.NoDelay = false;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        int len = dataPacket.CodePacket(bw);
                        if (len > 0)
                        {
                            //memoryStream.Flush();
                            DoSendDataWork(memoryStream.ToArray());
                            sendBytes = memoryStream.Length;
                            if (dataPacket is RealTimeDataPacket)
                            {
                                Debug.Print("SendPacket : " +
                                                        ((RealTimeDataPacket)dataPacket).RequestType.ToString());
                                //LogUtilities.LogMessage("SendPacket : " +
                                //                        ((RealTimeDataPacket)dataPacket).RequestType.ToString());
                            }
                            else if (dataPacket is InfoDataPacket)
                            {
                                Debug.Print("SendPacket : " +
                                                        ((InfoDataPacket)dataPacket).RequestType.ToString());
                                //LogUtilities.LogMessage("SendPacket : " +
                                //                        ((InfoDataPacket)dataPacket).RequestType.ToString());
                            }
                            else if (dataPacket is OrgDataPacket)
                            {
                                Debug.Print("SendPacket : " +
                                                        ((OrgDataPacket)dataPacket).RequestType.ToString());
                                //LogUtilities.LogMessage("SendPacket : " +
                                //                        ((OrgDataPacket)dataPacket).RequestType.ToString());
                            }
                        }
                        else
                        {
                            LogUtilities.LogMessage("DoResponsWork  Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtilities.LogMessage("DoResponsWork" + ex.Message);
                    }
                }
            }

            return sendBytes;
        }

        void DoSendDataWork(byte[] data)
        {
            if (_socket.Connected)
            {
                try
                {
                    _socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendEndCallBack), null);
                }
                catch (Exception e)
                {
                    MessageBox.Show(_tcpService.ToString() + "断开链接" + e.Message);
                    LogUtilities.LogMessage("TcpSend Error :  " + e.Message);
                }
            }

        }

        void SendEndCallBack(IAsyncResult iAsyncResult)
        {
            try
            {
                if (_socket != null)
                    _socket.EndSend(iAsyncResult);
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("SendEndCallBack" +e.Message);
            }

        }
        #endregion

        #region 接收数据

        #region 实时行情服务器的返回包
        private readonly MemoryStream _msRealTime = new MemoryStream();
        private int _bodyLenRealTime;
        private int _packetStartPosRealTime;//指向包头的第一个字节
        private int _msLengthRealTime;//_ms里的有效长度

        ///<summary>
        /// 网络中接收到数据的事件发生
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        public void RecvRealTimeData(object sender, RecvStreamEventArgs e)
        {
            try
            {
                while (_packetStartPosRealTime < e.Length)
                {
                    if (_bodyLenRealTime == 0)
                    {
                        if ((_packetStartPosRealTime + 3) < e.Length)
                        {
                            //获取包体长度
                            byte[] len = new byte[]
                                        {
                                            e.Data[_packetStartPosRealTime], e.Data[_packetStartPosRealTime + 1],
                                            e.Data[_packetStartPosRealTime + 2],
                                            e.Data[_packetStartPosRealTime + 3]
                                        };
                            _bodyLenRealTime = BitConverter.ToInt32(len, 0);
                        }
                        else
                        {
                            break;
                        }
                    }


                    //如果有一个完整包，解包
                    if (_packetStartPosRealTime + 16 + _bodyLenRealTime - _msLengthRealTime < e.Length)
                    {
                        _msRealTime.Write(e.Data, _packetStartPosRealTime, 17 + _bodyLenRealTime - _msLengthRealTime);
                        _packetStartPosRealTime += 17 + _bodyLenRealTime - _msLengthRealTime;

                        RealTimeDataPacket dataPacket = RealTimeDataPacket.DecodePacket(_msRealTime.ToArray(), 17 + _bodyLenRealTime);
                        if (dataPacket is ResHeartDataPacket)
                            RecHeart();
                        else if (dataPacket is ResOceanHeart)
                            RecHeart();
                        _bodyLenRealTime = 0;
                        _msRealTime.Position = 0;
                        _msLengthRealTime = 0;
                        if (dataPacket != null && (dataPacket.IsResult || dataPacket.RequestType == FuncTypeRealTime.Init))
                        {
                            (new Thread(ProcessDataPacket)).Start(new CMRecvDataEventArgs(_tcpService,
                                                                                    dataPacket, e.Length));
                        }
                    }
                    else
                    {
                        break;
                    }


                }
                //有多余的字节，且不是一个完整包，存起来
                if (e.Length > _packetStartPosRealTime)
                {
                    _msRealTime.Write(e.Data, _packetStartPosRealTime, e.Length - _packetStartPosRealTime);
                    _msLengthRealTime += e.Length - _packetStartPosRealTime;
                }
                else
                {
                    _msLengthRealTime = 0;
                }
                _packetStartPosRealTime = 0;
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage(ex.Message);
            }

        }
        #endregion

        #region 资讯服务器的返回包
        private readonly MemoryStream _msInfo = new MemoryStream();
        private int _bodyLenInfo;
        private uint _packetStartPosInfo;//指向包头的第一个字节
        private uint _msLengthInfo;//_ms里的有效长度

        ///<summary>
        /// 网络中接收到数据的事件发生
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        public void RecvInfoData(object sender, RecvStreamEventArgs e)
        {
            while (_packetStartPosInfo < e.Length)
            {
                if (_bodyLenInfo == 0)
                {
                    if ((_packetStartPosInfo + 3) < e.Length)
                    {
                        //获取包体长度
                        byte[] len = new byte[]
                                        {
                                            e.Data[_packetStartPosInfo], e.Data[_packetStartPosInfo + 1],
                                            e.Data[_packetStartPosInfo + 2], 
                                            e.Data[_packetStartPosInfo + 3]
                                        };
                        _bodyLenInfo = BitConverter.ToInt32(len, 0);
                    }
                    else
                    {
                        break;
                    }
                }

                //如果有一个完整包，解包
                if (_packetStartPosInfo + 16 + _bodyLenInfo - _msLengthInfo < e.Length)
                {

                    {
                        _msInfo.Write(e.Data, Convert.ToInt32(_packetStartPosInfo), Convert.ToInt32(17 + _bodyLenInfo - _msLengthInfo));
                        _packetStartPosInfo += 17 + (uint)_bodyLenInfo - _msLengthInfo;
                        InfoDataPacket dataPacket = InfoDataPacket.DecodePacket(_msInfo.ToArray(), Convert.ToInt32(17 + _bodyLenInfo));
                        if (dataPacket is ResInfoHeart)
                            RecHeart();
                        _bodyLenInfo = 0;
                        _msInfo.Position = 0;
                        _msLengthInfo = 0;
                        if (dataPacket != null && dataPacket.IsResult)
                        {
                            (new Thread(ProcessDataPacket)).Start(new CMRecvDataEventArgs(_tcpService,
                                                                                    dataPacket, e.Length));
                        }
                    }

                }
                else
                {
                    break;
                }


            }
            //有多余的字节，且不是一个完整包，存起来
            if (e.Length > _packetStartPosInfo)
            {
                _msInfo.Write(e.Data, Convert.ToInt32(_packetStartPosInfo), Convert.ToInt32(e.Length - _packetStartPosInfo));
                _msLengthInfo += (uint)(e.Length - _packetStartPosInfo);
            }
            else
            {
                _msLengthInfo = 0;
            }
            _packetStartPosInfo = 0;
        }
        #endregion

        #region 机构服务器返回包
        private readonly MemoryStream _msOrg = new MemoryStream();
        private int _bodyLenOrg;
        private int _packetStartPosOrg;//指向包头的第一个字节
        private int _msLengthOrg;//_ms里的有效长度

        ///<summary>
        /// 网络中接收到数据的事件发生
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        public void RecvOrgData(object sender, RecvStreamEventArgs e)
        {
            try
            {
                while (_packetStartPosOrg < e.Length)
                {
                    if (_bodyLenOrg == 0)
                    {
                        if ((_packetStartPosOrg + 3) < e.Length)
                        {
                            //获取包体长度
                            byte[] len = new byte[]
                                        {
                                            e.Data[_packetStartPosOrg], e.Data[_packetStartPosOrg + 1],
                                            e.Data[_packetStartPosOrg + 2],
                                            e.Data[_packetStartPosOrg + 3]
                                        };
                            _bodyLenOrg = BitConverter.ToInt32(len, 0);
                        }
                        else
                        {
                            break;
                        }
                    }


                    //如果有一个完整包，解包
                    if (_packetStartPosOrg + 16 + _bodyLenOrg - _msLengthOrg < e.Length)
                    {
                        _msOrg.Write(e.Data, _packetStartPosOrg, 17 + _bodyLenOrg - _msLengthOrg);
                        _packetStartPosOrg += 17 + _bodyLenOrg - _msLengthOrg;

                        OrgDataPacket dataPacket = OrgDataPacket.DecodePacket(_msOrg.ToArray(), 17 + _bodyLenOrg);
                        if (dataPacket is ResHeartOrgDataPacket)
                            RecHeart();

                        _bodyLenOrg = 0;
                        _msOrg.Position = 0;
                        _msLengthOrg = 0;
                        if (dataPacket != null && dataPacket.IsResult)
                        {
                            //(new Thread(ProcessDataPacket)).Start(new CMRecvDataEventArgs(_tcpService, dataPacket, e.Length));

                            lock (_DataPacketQueue)
                            {
                                _DataPacketQueue.Enqueue(new CMRecvDataEventArgs(_tcpService, dataPacket, e.Length));
                            }
                        }
                    }
                    else
                    {
                        break;
                    }


                }
                //有多余的字节，且不是一个完整包，存起来
                if (e.Length > _packetStartPosOrg)
                {
                    _msOrg.Write(e.Data, _packetStartPosOrg, e.Length - _packetStartPosOrg);
                    _msLengthOrg += e.Length - _packetStartPosOrg;
                }
                else
                {
                    _msLengthOrg = 0;
                }
                _packetStartPosOrg = 0;
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage(ex.Message);
            }

        }
        #endregion

        /// <summary>
        /// 触发收到新数据的消息
        /// </summary>
        /// <param name="args"></param>
        private void ProcessDataPacket(object args)
        {
            if (OnReceiveData != null)
                OnReceiveData(this, (CMRecvDataEventArgs)args);
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

        #endregion
    }
}
