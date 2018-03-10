/*****************************************************************************\
*                                                                             *
* DataCenter.cs -  Data center functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ����                                          *
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
using EmSerDataService;

namespace OwLib
{
    /// <summary>
    /// ������������
    /// </summary>
    public class DataCenter
    {
        #region Lord 2016/3/10
        private static ExportService m_exportService;

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public static ExportService ExportService
        {
            get { return m_exportService; }
        }

        private static bool m_isAppAlive = true;

        /// <summary>
        /// ��ȡ�����ó����Ƿ���
        /// </summary>
        public static bool IsAppAlive
        {
            get { return DataCenter.m_isAppAlive; }
            set { DataCenter.m_isAppAlive = value; }
        }

        private static UserCookieService m_userCookieService;

        /// <summary>
        /// �û�Cookie����
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
        /// ��ȡ��ѡ�ɷ���
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
        /// ��ȡ������֤ȯ����
        /// </summary>
        public static BlockService BlockService
        {
            get { return DataCenter.blockService; }
            set { DataCenter.blockService = value; }
        }

        private static DataQuery dataQuery = new DataQuery();

        /// <summary>
        /// ��ȡ�������������Ӷ���
        /// </summary>
        public static DataQuery DataQuery
        {
            get { return DataCenter.dataQuery; }
            set { DataCenter.dataQuery = value; }
        }

        private static QuoteSequencService quoteSequencService = new QuoteSequencService();

        /// <summary>
        /// ��ȡ�������������з���
        /// </summary>
        public static QuoteSequencService QuoteSequencService
        {
            get { return DataCenter.quoteSequencService; }
            set { DataCenter.quoteSequencService = value; }
        }

        /// <summary>
        /// ��ȡ�������û�ID
        /// </summary>
        public static String UserID
        {
            get { return users[rd.Next(0, users.Count)]; }
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns>����·��</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// ��ȡ�û�Ŀ¼
        /// </summary>
        /// <returns>�û�Ŀ¼</returns>
        public static String GetUserPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        public static void StartService()
        {
            m_userCookieService = new UserCookieService();
            m_exportService = new ExportService();
            m_userSecurityService = new UserSecurityService();
            m_eMSecurityService = new EMSecurityService();
            SecurityService.Start();
        }
        #endregion
    }
}
