/*******************************************************************************************\
*                                                                                           *
* HttpPostService.cs -  Http post service functions, types, and definitions.                *
*                                                                                           *
*               Version 1.00  ★★★                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.                    *
*               Created by Todd 2016/10/17.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using OwLib;

namespace node.gs
{
    /// <summary>
    /// HTTP的POST服务
    /// </summary>
    public class HttpPostService : BaseService
    {
        /// <summary>
        /// 创建HTTP服务
        /// </summary>
        public HttpPostService()
        {
        }

        private bool m_isSyncSend;
        /// <summary>
        /// 获取或者设置是否同步发送
        /// </summary>
        public bool IsSyncSend
        {
            get { return m_isSyncSend; }
            set { m_isSyncSend = value; }
        }

        private String m_url;

        /// <summary>
        /// 获取或设置地址
        /// </summary>
        public String Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="obj"></param>
        public void AsynSend(Object obj)
        {
            CMessage message = obj as CMessage;
            if (message == null)
            {
                return;
            }
            SendRequest(message);
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            SendToListener(message);
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public static String Post(String url)
        {
            byte[] bytes = Post(url, null);
            if(bytes != null)
            {
                return Encoding.Default.GetString(bytes);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public static byte[] Post(String url, byte[] sendDatas)
        {
            HttpWebRequest request = null;
            Stream reader = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (sendDatas != null)
                {
                    Stream writer = request.GetRequestStream();
                    writer.Write(sendDatas, 0, sendDatas.Length);
                    writer.Close();
                }
                response = (HttpWebResponse)request.GetResponse();
                reader = response.GetResponseStream();
                long contentLength = response.ContentLength;
                byte[] recvDatas = new byte[contentLength];
                for (int i = 0; i < contentLength; i++)
                {
                    recvDatas[i] = (byte)reader.ReadByte();
                }
                return recvDatas;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>返回消息</returns>
        public override int Send(CMessage message)
        {
            if (!m_isSyncSend)
            {
                Thread thread = new Thread(AsynSend);
                thread.Start(message);
                return 1;
            }
            else
            {
                return SendRequest(message);
            }
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>返回消息</returns>
        public int SendRequest(CMessage message)
        {
            Binary bw = new Binary();
            byte[] body = message.m_body;
            int bodyLength = message.m_bodyLength;
            int uncBodyLength = bodyLength;
            if (message.m_compressType == COMPRESSTYPE_GZIP)
            {
                using (MemoryStream cms = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(cms, CompressionMode.Compress))
                    {
                        gzip.Write(body, 0, body.Length);
                    }
                    body = cms.ToArray();
                    bodyLength = body.Length;
                }
            }
            int len = sizeof(int) * 4 + bodyLength + sizeof(short) * 3 + sizeof(byte) * 2;
            bw.WriteInt(len);
            bw.WriteShort((short)message.m_groupID);
            bw.WriteShort((short)message.m_serviceID);
            bw.WriteShort((short)message.m_functionID);
            bw.WriteInt(message.m_sessionID);
            bw.WriteInt(message.m_requestID);
            bw.WriteByte((byte)message.m_state);
            bw.WriteByte((byte)message.m_compressType);
            bw.WriteInt(uncBodyLength);
            bw.WriteBytes(body);
            byte[] bytes = bw.GetBytes();
            int length = bytes.Length;
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(m_url);
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = bytes.Length;
            if (bytes != null)
            {
                Stream writer = webReq.GetRequestStream();
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();
            }
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            Stream reader = response.GetResponseStream();
            long contentLength = response.ContentLength;
            byte[] dataArray = new byte[contentLength];
            for (int i = 0; i < contentLength; i++)
            {
                dataArray[i] = (byte)reader.ReadByte();
            }
            response.Close();
            reader.Dispose();
            bw.Close();
            int ret = dataArray.Length;
            UpFlow += ret;
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(byte) * ret);
            for (int i = 0; i < ret; i++)
            {
                IntPtr iptr = (IntPtr)((int)ptr + i);
                Marshal.WriteByte(iptr, dataArray[i]);
            }
            BaseService.CallBack(message.m_socketID, 0, ptr, ret);
            Marshal.FreeHGlobal(ptr);
            return ret;
        }
    }
}
