using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
 
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using EmCore;
using EmQComm;

namespace EmQTCP
{
    /// <summary>
    /// 发送数据和接收数据的TCP底层类.
    /// </summary>
    public class TcpConnection
    {
       
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
            get { return _haveConnected; }
            private set { _haveConnected = value; }
        }

        /// <summary>
        /// 服务器类型
        /// </summary>
        public ServerMode SerMode
        {
            get { return _serMode; }
            set { _serMode = value; }
        }


        private IpAddressPort _ipAddressPort;
        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpConnection()
        {
            HaveConnected = false;
        }


        #region 发送数据子模块

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
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        int len = dataPacket.CodePacket(bw);
                        if (len > 0)
                        {
                            DoSendDataWork(memoryStream.ToArray());
                            sendBytes = memoryStream.Length;
                            if (dataPacket is RealTimeDataPacket)
                            {
                                Debug.Print("SendPacket : " +
                                                        ((RealTimeDataPacket)dataPacket).RequestType.ToString());
                                LogUtilities.LogMessage("SendPacket : " +
                                                        ((RealTimeDataPacket) dataPacket).RequestType.ToString());
                            }
                            else if (dataPacket is InfoDataPacket)
                            {
                                Debug.Print("SendPacket : " +
                                                        ((InfoDataPacket)dataPacket).RequestType.ToString());
                                LogUtilities.LogMessage("SendPacket : " +
                                                        ((InfoDataPacket) dataPacket).RequestType.ToString());
                            }
                            else if (dataPacket is OrgDataPacket)
                            {
                                Debug.Print("SendPacket : " +
                                                        ((OrgDataPacket)dataPacket).RequestType.ToString());
                                LogUtilities.LogMessage("SendPacket : " +
                                                        ((OrgDataPacket) dataPacket).RequestType.ToString());
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

        #endregion

        #region 接收数据子模块

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
        public void RecvRealTimeData(object sender, RecvDataEventArgs e)
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

                        _bodyLenRealTime = 0;
                        _msRealTime.Position = 0;
                        _msLengthRealTime = 0;
                        if (dataPacket != null && (dataPacket.IsResult || dataPacket.RequestType == FuncTypeRealTime.Init))
                        {
                            //(new Thread(ProcessDataPacket)).Start(new CMRecvDataEventArgs(_ipAddressPort,
                            //                                                        dataPacket, e.Length));
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
        public void RecvInfoData(object sender, RecvDataEventArgs e)
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

                        _bodyLenInfo = 0;
                        _msInfo.Position = 0;
                        _msLengthInfo = 0;
                        if (dataPacket != null && dataPacket.IsResult)
                        {
                            //(new Thread(ProcessDataPacket)).Start(new CMRecvDataEventArgs(_ipAddressPort,
                            //                                                        dataPacket, e.Length));
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
        public void RecvOrgData(object sender, RecvDataEventArgs e)
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

                        _bodyLenOrg = 0;
                        _msOrg.Position = 0;
                        _msLengthOrg = 0;
                        if (dataPacket != null && dataPacket.IsResult)
                        {
                            //(new Thread(ProcessDataPacket)).Start(new ClientRecvDataEventArgs(_ipAddressPort,
                            //                                                        dataPacket, e.Length));
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
            //if (OnReceiveData != null)
            //    OnReceiveData(this, (ClientRecvDataEventArgs) args);
        }

        #endregion

        ///<summary>
        /// 释放资源
        ///</summary>
        public void Dispose()
        {
            if (_msRealTime != null)
                _msRealTime.Close();
            if(_tcpClient != null)
                _tcpClient.Close();

        }

        #region 临时用

        private TcpClient _tcpClient;
        private bool _haveConnected;
        private ServerMode _serMode;
        private const int _bufferSize = 1024*10;
        //private const int _bufferSize = 512;

        /// <summary>
        /// 临时用，建立tcp连接
        /// </summary>
        /// <param name="ipAddressPort"></param>
        public void Connect(IpAddressPort ipAddressPort)
        {
            try
            {
                _ipAddressPort = ipAddressPort;
                _tcpClient = new TcpClient();
                _tcpClient.Connect(ipAddressPort.HostName, ipAddressPort.Port);
                if (_tcpClient.Connected)
                {
                    HaveConnected = true;
                    byte[] buffer = new byte[_bufferSize];
                    _tcpClient.Client.BeginReceive(buffer, 0, _bufferSize, 0, new AsyncCallback(RecvEndCallBack), buffer);
                    //if (OnConnectServSuccess != null)
                    //    OnConnectServSuccess(this, new ConnectEventArgs(SerMode, this, ipAddressPort));
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage(this.SerMode.ToString() + "服务器连接失败..." + e.Message);
            }

        }

        static Mutex _mutex = new Mutex();
        void RecvEndCallBack(IAsyncResult iAsyncResult)
        {
            try
            {
                //lock (this)
                {
                    int readBytes = _tcpClient.Client.EndReceive(iAsyncResult);
                    if (readBytes > 0)
                    {
                        _mutex.WaitOne();
                        try
                        {
                            RecvDataEventArgs e = new RecvDataEventArgs(_ipAddressPort,
                                        (byte[])
                                        iAsyncResult.AsyncState,
                                        readBytes);

                            switch (SerMode)
                            {
                                case ServerMode.RealTime:
                                case ServerMode.History:
                                   
                                    RecvRealTimeData(this, e);
                                    break;
                                case ServerMode.Information:
                                    RecvInfoData(this, e);
                                    break;
                                case ServerMode.Org:
                                    RecvOrgData(this, e);
                                    break;
                                case ServerMode.Oversea:
                                    
                                    RecvRealTimeData(this, e);
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


                    }
                    _tcpClient.Client.BeginReceive((byte[]) iAsyncResult.AsyncState, 0, _bufferSize, 0,
                                                   new AsyncCallback(RecvEndCallBack), iAsyncResult.AsyncState);
                }
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage(SerMode.ToString() + ex.Message);
            }
        }



        void DoSendDataWork(byte[] data)
        {
            if (!_tcpClient.Connected)
                return;
            
            try
            {
                _tcpClient.Client.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendEndCallBack), null);
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("TcpSend Error :  " + e.Message);
                //throw;
            }
        }

        void SendEndCallBack(IAsyncResult iAsyncResult)
        {
            _tcpClient.Client.EndSend(iAsyncResult);
        }

        #endregion
    }
}
