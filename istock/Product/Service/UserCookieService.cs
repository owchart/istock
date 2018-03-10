/**************************************************************************************\
*                                                                                      *
* UserCookieService.cs -  User coolie service functions, types, and definitions.       *
*                                                                                      *
*               Version 1.00 ��                                                        *
*                                                                                      *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.               *
*               Created by Todd.                                                 *
*                                                                                      *
***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace OwLib
{
    /// <summary>
    /// �û�Cookie
    /// </summary>
    public class UserCookie
    {
        /// <summary>
        /// ��
        /// </summary>
        public String m_key = "";

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID; 

        /// <summary>
        /// ֵ
        /// </summary>
        public String m_value = ""; 
    }

    /// <summary>
    /// �û�Cookie����
    /// </summary>
    public class UserCookieService
    {
        #region ������ 2016/5/28
        /// <summary>
        /// �����û�״̬����
        /// </summary>
        public UserCookieService()
        {
            String dataDir = DataCenter.GetUserPath();
            if (!CFileA.IsDirectoryExist(dataDir))
            {
                CFileA.CreateDirectory(dataDir);
            }
            String dataBasePath = DataCenter.GetUserPath() + "\\usercookies.db";
            m_connectStr = "Data Source = " + dataBasePath;
            if (!CFileA.IsFileExist(dataBasePath))
            {
                CreateTable();
            }
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        private String m_connectStr = "";

        /// <summary>
        /// ��
        /// </summary>
        private object m_lock = new object();

        /// <summary>
        /// ����SQL
        /// </summary>
        public const String CREATETABLESQL = "CREATE TABLE USERCOOKIE(USERID INTEGER, KEY, VALUE, MODIFYTIME DATE, CREATETIME DATE)";

        /// <summary>
        /// �����ַ���
        /// </summary>
        public const String DATABASENAME = "usercookies.db";

        private int m_userID;

        /// <summary>
        /// ��ȡ�������û�ID
        /// </summary>
        public int UserID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }

        /// <summary>
        /// ����û�Cookie
        /// </summary>
        /// <param name="cookie">��Ϣ</param>
        /// <returns>״̬</returns>
        public int AddCookie(UserCookie cookie)
        {
            UserCookie oldCookie = new UserCookie();
            if (GetCookie(cookie.m_key, ref oldCookie) > 0)
            {
                UpdateCookie(cookie);
            }
            else
            {
                String sql = String.Format("INSERT INTO USERCOOKIE(USERID, KEY, VALUE, MODIFYTIME, CREATETIME) values ({0}, '{1}', '{2}','1970-1-1','1970-1-1')",
                m_userID, CStrA.GetDBString(cookie.m_key), CStrA.GetDBString(cookie.m_value));
                SQLiteConnection conn = new SQLiteConnection(m_connectStr);
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ�������Ƿ���Ҫ������
        /// </summary>
        public void CreateTable()
        {
            String dataBasePath = DataCenter.GetUserPath() + "\\" + DATABASENAME;
            if (!CFileA.IsFileExist(dataBasePath))
            {
                //�������ݿ��ļ�
                SQLiteConnection.CreateFile(dataBasePath);
            }
            //������
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = CREATETABLESQL;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// ɾ���û�Cookie
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>״̬</returns>
        public int DeleteCookie(String key)
        {
            String sql = String.Format("DELETE FROM USERCOOKIE WHERE USERID = {0} AND KEY = '{1}'", m_userID, CStrA.GetDBString(key));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }


        /// <summary>
        /// ��ȡ�û�Cookie
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="cookie">�Ự</param>
        /// <returns>״̬</returns>
        public int GetCookie(String key, ref UserCookie cookie)
        {
            int state = 0;
            String sql = String.Format("SELECT * FROM USERCOOKIE WHERE USERID = {0} AND KEY = '{1}'", m_userID, CStrA.GetDBString(key));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cookie.m_userID = reader.GetInt32(0);
                cookie.m_key = reader.GetString(1);
                cookie.m_value = reader.GetString(2);
                state = 1;
            }
            reader.Close();
            conn.Close();
            return state;
        }

        /// <summary>
        /// ���»Ự
        /// </summary>
        /// <param name="cookie">�Ự</param>
        /// <returns>״̬</returns>
        public int UpdateCookie(UserCookie cookie)
        {
            String sql = String.Format("UPDATE USERCOOKIE SET VALUE = '{0}' WHERE USERID = {1} AND KEY = '{2}'",
            CStrA.GetDBString(cookie.m_value), m_userID, CStrA.GetDBString(cookie.m_key));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }
        #endregion
    }
}
