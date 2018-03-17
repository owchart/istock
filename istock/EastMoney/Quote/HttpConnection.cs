using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using EmCore;

namespace OwLib
{
    /// <summary>
    /// http连接下用来收发数据的类
    /// </summary>
    public class HttpConnection
    {
        /// <summary>
        /// 连接的服务器地址
        /// </summary>
        private String _httpUrl;

        private IServerInfo _serverHelper;

        /*
        /// <summary>
        /// 连接地址
        /// </summary>
        /// <remarks></remarks>
        private Uri _httpSite;
        */

        /// <summary>
        /// 当连接成功的事件
        /// </summary>
        public event EventHandler<EventArgs> OnConnectServSuccess;

        /// <summary>
        /// 当收到数据时的事件
        /// </summary>
        public event EventHandler<CMRecvDataEventArgs> OnReceiveData;

        /// <summary>
        /// 连接服务器接口
        /// </summary>
        public IServerInfo ServerHelper
        {
            get { return _serverHelper; }
            set { _serverHelper = value; }
        }

        /// <summary>
        /// 模拟一个连接成功
        /// </summary>
        public void Connect(String httpUrl)
        {
            _httpUrl = httpUrl;
            //_httpSite = new Uri(httpUrl);
            if (OnConnectServSuccess != null)
                OnConnectServSuccess(this, null);
        }

        #region 发送数据子模块

        /// <summary>
        /// 发送一个数据包
        /// </summary>
        /// <param name="dataPacket"></param>
        /// <returns></returns>
        public long DoSendPacket(DataPacket dataPacket)
        {
            if (dataPacket == null)
                return 0;
            long sendBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    sendBytes = 0;
                    try
                    {
//                         DataPacket.CodePacket(bw, dataPacket);
//                         if (dataPacket.Len > 0)
//                         {
//                             ServerHelper.GetResponseByPostAsync(_httpUrl, ms.ToArray(), OnRecvResponse);
//                             sendBytes = ms.Length;
//                         }
                    }
                    catch (Exception ex)
                    {
                        LogUtilities.LogMessage("DoSendPacket Error " + ex.Message);
                    }
                }
            }
            return sendBytes;
        }


        #endregion

        #region 接收数据子模块

        /// <summary>
        /// 异步接收到新的数据的处理
        /// </summary>
        /// <param name="ar"></param>
        protected void OnRecvResponse(object ar)
        {
            try
            {
                using (HttpWebResponse webre = (HttpWebResponse) (ar))
                {
                    byte[] buffer;
                    int readCount;
                    using (Stream response = webre.GetResponseStream())
                    {
                        int contentLen = Convert.ToInt32 (webre.ContentLength);
                        buffer = new byte[contentLen];
                        readCount = ReadAllBytesFromStream(response, buffer, contentLen);
                    }
                    if (readCount > 0)
                    {
//                         DataPacket dataPacket = DataPacket.DecodePacket(buffer, readCount); //对数据进行解包
//                         if (dataPacket != null)
//                             (new Thread(ProcessDataPacket)).Start(new ClientRecvDataEventArgs(null, dataPacket, readCount));
                    }
                    else
                        LogUtilities.LogMessage("HTTP Recv 0 byte");
                }

            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("recv data error: " + ex.Message);
            }
        }

        /// <summary>
        /// 从流中读取字节
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="buffer">字节数组</param>
        /// <param name="len">要读取的字节长度</param>
        /// <returns></returns>
        private static int ReadAllBytesFromStream(Stream stream, byte[] buffer, int len)
        {
            int readCount = 0;
            while (readCount < len)
            {
                int length = stream.Read(buffer, readCount, len - readCount);
                if (length <= 0)
                    break;
                readCount += length;
            }
            return readCount;
        }

        /// <summary>
        /// 触发收到新数据的消息
        /// </summary>
        /// <param name="args"></param>
        private void ProcessDataPacket(object args)
        {
            if (OnReceiveData != null)
                OnReceiveData(this, (CMRecvDataEventArgs)args);
        }

        #endregion
    }
}
