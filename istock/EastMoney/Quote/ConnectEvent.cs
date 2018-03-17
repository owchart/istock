using System;
using System.Collections.Generic;
using System.Text;
using EmCore;
using EmSocketClient;

namespace OwLib
{
    /// <summary>
    /// 收到数据后的发生事件的参数,ConnectManager中用到
    /// </summary>
    public class CMRecvDataEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="dataPacket"></param>
        /// <param name="length"></param>
        public CMRecvDataEventArgs(TcpService tcpService, DataPacket dataPacket, int length)
        {
            ServiceType = tcpService;
            DataPacket = dataPacket;
            Length = length;
        }

        /// <summary>
        /// 服务器ip
        /// </summary>
        public TcpService ServiceType;

        private DataPacket _dataPacket;
        private int _length;

        /// <summary>
        /// 数据响应包
        /// </summary>
        public DataPacket DataPacket
        {
            get { return _dataPacket; }
            set { _dataPacket = value; }
        }

        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }


    ///<summary>
    /// Socket连接成功
    ///</summary>
    public class ConnectEventArgs : EventArgs
    {
        private SocketConnection _clientSocketConnection;
        private TcpService _serviceType;

        /// <summary>
        /// 连接参数
        /// </summary>
        /// <param name="serverMode"></param>
        /// <param name="tcpConnection"></param>
        /// <param name="ap"></param>
        public ConnectEventArgs(TcpService tcpService, SocketConnection socketConnection)
        {
            ServiceType = tcpService;
            ClientSocketConnection = socketConnection;
        }

        /// <summary>
        /// TcpConnection
        /// </summary>
        public SocketConnection ClientSocketConnection
        {
            get { return _clientSocketConnection; }
            set { _clientSocketConnection = value; }
        }


        /// <summary>
        /// 服务器类型
        /// </summary>
        public TcpService ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }
    }


    ///<summary>
    /// Socket连接失败
    ///</summary>
    public class ConnectFailEventArgs : EventArgs
    {
        private IpAddressPort _ap;
        private String _failReason;
        private TcpConnection _tcpConnection;

        ///<summary>
        ///</summary>
        ///<param name="tcpConnection"></param>
        ///<param name="ap"></param>
        ///<param name="reason"></param>
        public ConnectFailEventArgs(TcpConnection tcpConnection, IpAddressPort ap, String reason)
        {
            TcpConnection = tcpConnection;
            Ap = ap;
            FailReason = reason;
        }

        /// <summary>
        /// TcpConnection
        /// </summary>
        public TcpConnection TcpConnection
        {
            get { return _tcpConnection; }
            private set { _tcpConnection = value; }
        }

        /// <summary>
        /// IpAddressPort
        /// </summary>
        public IpAddressPort Ap
        {
            get { return _ap; }
            private set { _ap = value; }
        }

        /// <summary>
        /// 失败原因
        /// </summary>
        public String FailReason
        {
            get { return _failReason; }
            private set { _failReason = value; }
        }
    }

    /// <summary>
    /// 收到二进制流
    /// </summary>
    public class RecvStreamEventArgs　: EventArgs
    {
        private byte[] _data;
        private int _length;

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            private set { _data = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return _length; }
            private set { _length = value; }
        }

        public RecvStreamEventArgs(byte[] data, int length)
        {
            Data = data;
            Length = length;
        }
    }

    /*
    ///<summary>
    /// 收到数据后的消息参数
    ///</summary>
    public class ClientRecvDataEventArgs : EventArgs
    {
        ///<summary>
        /// ClientRecvDataEventArgs
        ///</summary>
        ///<param name="ap"></param>
        ///<param name="datePacket"></param>
        ///<param name="length"></param>
        public ClientRecvDataEventArgs(IpAddressPort ap, DataPacket datePacket, int length)
        {
            Ap = ap;
            DataPacket = datePacket;
            Length = length;
        }

        ///<summary>
        /// IpAddressPort
        ///</summary>
        public IpAddressPort Ap { get; private set; }

        ///<summary>
        /// 数据包
        ///</summary>
        public DataPacket DataPacket ;

        ///<summary>
        /// 长度
        ///</summary>
        public int Length ;
    }
     * 
     * */
}
