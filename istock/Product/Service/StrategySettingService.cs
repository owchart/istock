using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace OwLib
{  
    /// <summary>
    /// 交易策略服务
    /// </summary>
    public class StrategySettingService : IDisposable
    {
        /// <summary>
        /// 创建交易策略服务
        /// </summary>
        public StrategySettingService()
        {
            CreateTable();
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        private String m_connectStr = "";

        /// <summary>
        /// 建表SQL
        /// </summary>
        public const String CREATETABLESQL = "CREATE TABLE STRATEGYSETTING(ID INTEGER PRIMARY KEY AUTOINCREMENT, SECURITYCODE, "
            + " SECURITYNAME, STRATEGYTYPE INTEGER, STRATEGYSETTINGINFO TEXT)";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public const String DATABASENAME = "StrategySetting.db";

        /// <summary>
        /// 添加策略
        /// </summary>
        /// <param name="strategySetting">策略</param>
        /// <returns>状态</returns>
        public int AddStrategySetting(SecurityStrategySetting strategySetting)
        {
            String sql = String.Format("INSERT INTO STRATEGYSETTING(SECURITYCODE, SECURITYNAME, STRATEGYTYPE, STRATEGYSETTINGINFO) values ('{0}','{1}', {2}, '{3}')",
                CStrA.GetDBString(strategySetting.m_securityCode), CStrA.GetDBString(strategySetting.m_securityName), strategySetting.m_strategyType
                ,  CStrA.GetDBString(strategySetting.m_strategySettingInfo));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            int ret = cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// 获取或设置是否需要创建表
        /// </summary>
        public void CreateTable()
        {
            String dataDir = DataCenter.GetAppPath() + "\\data";
            if (!CFileA.IsDirectoryExist(dataDir))
            {
                CFileA.CreateDirectory(dataDir);
            }
            String dataBasePath = dataDir + "\\" + DATABASENAME;
            m_connectStr = "Data Source = " + dataBasePath;
            if (!CFileA.IsFileExist(dataBasePath))
            {
                //创建数据库文件
                SQLiteConnection.CreateFile(dataBasePath);
                //创建表
                SQLiteConnection conn = new SQLiteConnection(m_connectStr);
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = CREATETABLESQL;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// 获取策略数量
        /// </summary>
        /// <param name="strategySetting">分时数据列表</param>
        /// <returns>状态</returns>
        public int GetSecurityStrategySettingConut(SecurityStrategySetting strategySetting)
        {
            //String sql = String.Format("SELECT * FROM STRATEGYSETTING WHERE SECURITYCODE = '{0}' AND STRATEGYTYPE = {1}"
            //    , CStrA.GetDBString(strategySetting.m_securityCode), strategySetting.m_strategyType);
            String sql = String.Format("SELECT * FROM STRATEGYSETTING WHERE STRATEGYTYPE = {1}", strategySetting.m_strategyType);
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            int count = 0;
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                count++;
            }
            reader.Close();
            conn.Close();
            return count;
        }

        /// <summary>
        /// 获取策略
        /// </summary>
        /// <param name="strategySettings">策略列表</param>
        /// <returns>状态</returns>
        public int GetSecurityStrategySettings(List<SecurityStrategySetting> strategySettings)
        {
            String sql = "SELECT * FROM STRATEGYSETTING";
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int index = 1;
                SecurityStrategySetting setting = new SecurityStrategySetting();
                setting.m_securityCode = reader.GetString(index++);
                setting.m_securityName = reader.GetString(index++);
                setting.m_strategyType = reader.GetInt32(index++);
                setting.m_strategySettingInfo = reader.GetString(index++);
                strategySettings.Add(setting);
            }
            reader.Close();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// 获取策略
        /// </summary>
        /// <param name="strategyType">策略类型</param>
        /// <param name="strategySettings">策略列表</param>
        /// <returns>状态</returns>
        public int GetSecurityStrategySettings(int strategyType, List<SecurityStrategySetting> strategySettings)
        {
            String sql = String.Format("SELECT * FROM STRATEGYSETTING WHERE STRATEGYTYPE = {0}", strategyType);
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int index = 1;
                SecurityStrategySetting setting = new SecurityStrategySetting();
                setting.m_securityCode = reader.GetString(index++);
                setting.m_securityName = reader.GetString(index++);
                setting.m_strategyType = reader.GetInt32(index++);
                setting.m_strategySettingInfo = reader.GetString(index++);
                strategySettings.Add(setting);
            }
            reader.Close();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// 更新策略
        /// </summary>
        /// <param name="strategySetting">策略</param>
        /// <returns></returns>
        public int UpdateSecurityStrategySetting(SecurityStrategySetting strategySetting)
        {
            //String sql = String.Format("UPDATE STRATEGYSETTING SET STRATEGYSETTINGINFO = '{0}' WHERE SECURITYCODE = '{1}' AND STRATEGYTYPE = {2}",
            //    CStrA.GetDBString(strategySetting.m_strategySettingInfo), CStrA.GetDBString(strategySetting.m_securityCode), strategySetting.m_strategyType);
            String sql = String.Format("UPDATE STRATEGYSETTING SET STRATEGYSETTINGINFO = '{0}', SECURITYCODE = '{1}', SECURITYNAME = '{2}' WHERE  STRATEGYTYPE = {3}",
                CStrA.GetDBString(strategySetting.m_strategySettingInfo), CStrA.GetDBString(strategySetting.m_securityCode), 
                CStrA.GetDBString(strategySetting.m_securityName), strategySetting.m_strategyType);
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }
    }
}
