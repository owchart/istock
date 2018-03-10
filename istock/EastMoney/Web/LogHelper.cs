using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EastMoney.FM.Web.Data
{
    /**/
    /// <summary>  
    /// LogHelper的摘要说明。  
    /// </summary>   
    public class LogHelper
    {
        private LogHelper()  
        {  
        }  
 
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");   //选择<logger name="loginfo">的配置
  
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");   //选择<logger name="logerror">的配置

        public static readonly log4net.ILog logdebug = log4net.LogManager.GetLogger("logdebug");   //选择<logger name="logdebug">的配置

        private static object infoObj = new object();
        private static object debugObj = new object();
        private static object errorObj = new object();
        /// <summary>
        /// 配置
        /// </summary>
        public static void SetConfig()  
        {  
            log4net.Config.XmlConfigurator.Configure();  
        }  
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="configFile"></param>
        public static void SetConfig(FileInfo configFile)  
        {  
            log4net.Config.XmlConfigurator.Configure(configFile);   
        }

        /// <summary>
        /// 写日志 记录调试日志 目录Log\LogDebug
        /// </summary>
        /// <param name="info">调试信息</param>
        public static void WriteDebugLog(string info)
        {
            if (logdebug.IsDebugEnabled)
            {
                lock (debugObj)
                {
                    logdebug.Debug(info);
                }
            }
        }
        public static void WriteDebugLog(string info,DateTime dt1,DateTime dt2,int tip)
        {
            TimeSpan sp1 = new TimeSpan(dt1.Ticks);
            TimeSpan sp2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = sp1.Subtract(sp2).Duration();
            int sec = ts.Milliseconds;
            if (sec>tip && logdebug.IsDebugEnabled)
            {
                lock (debugObj)
                {
                    logdebug.Debug(info);
                }
            }
        }
        /// <summary>
        /// 写日志 记录调试日志 目录Log\LogInfo
        /// </summary>
        /// <param name="info">调试信息</param>
        public static void WriteLog(string info)  
        {  
            if(loginfo.IsInfoEnabled)  
            {
                lock (infoObj)
                {
                    loginfo.Info(info);
                }
            }  
        }  
        
        /// <summary>
        /// 写日志 记录异常日志 目录Log\LogError
        /// </summary>
        /// <param name="info">调试信息</param>
        /// <param name="ex">异常</param>
        public static void WriteLog(string info,Exception ex)  
        {  
            if(logerror.IsErrorEnabled)  
            {
                lock (errorObj)
                {
                    logerror.Error(info, ex);
                }
            }  
        }   
    }
}
