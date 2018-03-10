/*****************************************************************************\
*                                                                             *
* DataCenter.cs -  Data center functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, OwPlan. All rights reserved.      *
*               Created by Lord 2016/3/10.                                    *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using OwLibSV;

namespace OwLib
{
    /// <summary>
    /// 处理行情数据
    /// </summary>
    public class DataCenter
    {
        #region Lord 2016/3/10
        private static ExportService m_exportService;

        /// <summary>
        /// 获取导出服务
        /// </summary>
        public static ExportService ExportService
        {
            get { return m_exportService; }
        }

        private static bool m_isAppAlive = true;

        /// <summary>
        /// 获取或设置程序是否存活
        /// </summary>
        public static bool IsAppAlive
        {
            get { return DataCenter.m_isAppAlive; }
            set { DataCenter.m_isAppAlive = value; }
        }

        private static UserCookieService m_userCookieService;

        /// <summary>
        /// 用户Cookie服务
        /// </summary>
        public static UserCookieService UserCookieService
        {
            get { return DataCenter.m_userCookieService; }
        }

        private static UserSecurityService m_userSecurityService;

        /// <summary>
        /// 获取自选股服务
        /// </summary>
        public static UserSecurityService UserSecurityService
        {
            get { return DataCenter.m_userSecurityService; }
        }


        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// 获取用户目录
        /// </summary>
        /// <returns>用户目录</returns>
        public static String GetUserPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void StartService()
        {
            m_userCookieService = new UserCookieService();
            m_exportService = new ExportService();
            m_userSecurityService = new UserSecurityService();
            SecurityService.Start();
        }
        #endregion
    }
}
