/**************************************************************************************\
*                                                                                      *
* UserCookieService.cs -  User coolie service functions, types, and definitions.       *
*                                                                                      *
*               Version 1.00 ★                                                        *
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
    /// 用户Cookie
    /// </summary>
    public class UserCookie
    {
        /// <summary>
        /// 键
        /// </summary>
        public String m_key = "";

        /// <summary>
        /// 用户ID
        /// </summary>
        public int m_userID; 

        /// <summary>
        /// 值
        /// </summary>
        public String m_value = ""; 
    }

    /// <summary>
    /// 用户Cookie服务
    /// </summary>
    public class UserCookieService
    {
        #region 王绍旭 2016/5/28
        /// <summary>
        /// 创建用户状态服务
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
        /// 连接字符串
        /// </summary>
        private String m_connectStr = "";

        /// <summary>
        /// 锁
        /// </summary>
        private object m_lock = new object();

        /// <summary>
        /// 建表SQL
        /// </summary>
        public const String CREATETABLESQL = "CREATE TABLE USERCOOKIE(USERID INTEGER, KEY, VALUE, MODIFYTIME DATE, CREATETIME DATE)";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public const String DATABASENAME = "usercookies.db";

        private int m_userID;

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public int UserID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }

        /// <summary>
        /// 添加用户Cookie
        /// </summary>
        /// <param name="cookie">消息</param>
        /// <returns>状态</returns>
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
        /// 获取或设置是否需要创建表
        /// </summary>
        public void CreateTable()
        {
            String dataBasePath = DataCenter.GetUserPath() + "\\" + DATABASENAME;
            if (!CFileA.IsFileExist(dataBasePath))
            {
                //创建数据库文件
                SQLiteConnection.CreateFile(dataBasePath);
            }
            //创建表
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = CREATETABLESQL;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// 删除用户Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>状态</returns>
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
        /// 获取用户Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="cookie">会话</param>
        /// <returns>状态</returns>
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
        /// 更新会话
        /// </summary>
        /// <param name="cookie">会话</param>
        /// <returns>状态</returns>
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
