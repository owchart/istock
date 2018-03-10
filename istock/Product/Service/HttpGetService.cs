/*******************************************************************************************\
*                                                                                           *
* HttpGetService.cs -  Http get service functions, types, and definitions.                  *
*                                                                                           *
*               Version 1.00  ★★★                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/10/17.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using OwLib;

namespace node.gs
{
    /// <summary>
    /// HTTP的GET服务
    /// </summary>
    public class HttpGetService
    {
        /// <summary>
        /// 创建HTTP服务
        /// </summary>
        public HttpGetService()
        {
        }

        /// <summary>
        /// 获取网页数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns>页面源码</returns>
        public static String Get(String url)
        {
            String content = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            Stream resStream = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = 10000;
                ServicePointManager.DefaultConnectionLimit = 50;
                response = (HttpWebResponse)request.GetResponse();
                resStream = response.GetResponseStream();
                streamReader = new StreamReader(resStream, Encoding.Default);
                content = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (resStream != null)
                {
                    resStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            return content;
        }
    }
}
