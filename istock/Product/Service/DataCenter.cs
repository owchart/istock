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
using EmSerDataService;

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

        /// <summary>
        /// 画线工具
        /// </summary>
        private static Dictionary<String, String> m_plots = new Dictionary<String, String>();

        /// <summary>
        /// 获取画线工具
        /// </summary>
        public static Dictionary<String, String> Plots
        {
            get { return m_plots; }
        }

        private static UserCookieService m_userCookieService;

        /// <summary>
        /// 用户Cookie服务
        /// </summary>
        public static UserCookieService UserCookieService
        {
            get { return DataCenter.m_userCookieService; }
        }

        private static EMSecurityService m_eMSecurityService;

        public static EMSecurityService EMSecurityService
        {
            get { return DataCenter.m_eMSecurityService; }
        }

        private static UserSecurityService m_userSecurityService;

        /// <summary>
        /// 获取自选股服务
        /// </summary>
        public static UserSecurityService UserSecurityService
        {
            get { return DataCenter.m_userSecurityService; }
        }

        static DataCenter()
        {
            String idFile = DataCenter.GetAppPath() + "\\config\\userid.txt";
            String content = File.ReadAllText(idFile);
            users.AddRange(content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
        }

        private static Random rd = new Random();

        private static List<String> users = new List<String>();

        private static BlockService blockService = new BlockService();

        /// <summary>
        /// 获取或设置证券服务
        /// </summary>
        public static BlockService BlockService
        {
            get { return DataCenter.blockService; }
            set { DataCenter.blockService = value; }
        }

        private static DataQuery dataQuery = new DataQuery();

        /// <summary>
        /// 获取或设置数据连接对象
        /// </summary>
        public static DataQuery DataQuery
        {
            get { return DataCenter.dataQuery; }
            set { DataCenter.dataQuery = value; }
        }

        private static MainFrame m_mainUI;

        /// <summary>
        /// 获取或设置主界面
        /// </summary>
        public static MainFrame MainUI
        {
            get { return DataCenter.m_mainUI; }
            set { DataCenter.m_mainUI = value; }
        }

        private static QuoteSequencService quoteSequencService = new QuoteSequencService();

        /// <summary>
        /// 获取或设置行情序列服务
        /// </summary>
        public static QuoteSequencService QuoteSequencService
        {
            get { return DataCenter.quoteSequencService; }
            set { DataCenter.quoteSequencService = value; }
        }

        private static StrategySettingService m_strategySettingService = new StrategySettingService();

        /// <summary>
        /// 获取策略服务
        /// </summary>
        public static StrategySettingService StrategySettingService
        {
            get { return DataCenter.m_strategySettingService; }
        }

        private static THSDealService m_thsDealService = new THSDealService();

        /// <summary>
        /// 获取同花顺交易服务
        /// </summary>
        public static THSDealService ThsDealService
        {
            get { return m_thsDealService; }
        }

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public static String UserID
        {
            get { return users[rd.Next(0, users.Count)]; }
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
        /// 读取所有的画线工具
        /// </summary>
        private static void ReadPlots()
        {
            String xmlPath = Path.Combine(GetAppPath(), "config\\Plots.xml");
            m_plots.Clear();
            if (File.Exists(xmlPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode rootNode = xmlDoc.DocumentElement;
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name.ToUpper() == "PLOT")
                    {
                        String name = String.Empty;
                        String text = String.Empty;
                        foreach (XmlNode childeNode in node.ChildNodes)
                        {
                            if (childeNode.Name.ToUpper() == "NAME")
                            {
                                name = childeNode.InnerText;
                            }
                            else if (childeNode.Name.ToUpper() == "TEXT")
                            {
                                text = childeNode.InnerText;
                            }
                        }
                        m_plots[name] = text;
                    }
                }
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void StartService()
        {
            ReadPlots();
            m_userCookieService = new UserCookieService();
            m_exportService = new ExportService();
            m_userSecurityService = new UserSecurityService();
            m_eMSecurityService = new EMSecurityService();
            bool loadAll = EMSecurityService.Load();
            //BlockService.Load(loadAll);
            CFTService.Start();
            SecurityService.Start();
        }
        #endregion
    }
}
