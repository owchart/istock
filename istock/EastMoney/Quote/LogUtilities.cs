using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
 
using System.Text;
using EmCore;

namespace OwLib
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class LogUtilities
    {
        static int _date = TimeUtilities.GetLastTradeDateInt();
        static readonly string FileName = PathUtilities.LogPath + @"\quote" + _date.ToString() + "Log.txt";
        static TextWriter _tw;
        static int _grade;

        static LogUtilities()
        {
            try
            {
                //_tw = new StreamWriter(FileName, true, Encoding.Default);
            }
            catch
            {
                _tw = null;
            }
        }

        /// <summary>
        /// [Conditional("DEBUG")] 进行条件编译
        /// </summary>
        public static void SetValue(int logGrade)
        {
            _grade = logGrade;
        }
        /// <summary>
        /// [Conditional("DEBUG")]
        /// </summary>
        public static void LogMessage(string message, int importentGrade)
        {
            if (importentGrade > _grade)
            {
                LogMessage(message);
            }
        }

        /// <summary>
        /// [Conditional("ISLOG")]
        /// </summary>
        public static void LogMessage(string message)
        {
            EmLog.Write("行情",message,LogScope.Debug);
            //if (_tw != null)
            //{
            //    if (_date != TimeUtilities.GetLastTradeDateInt())
            //    {
            //        _tw.Close();
            //        string fileName = PathUtilities.LogPath + @"\quote" + TimeUtilities.GetLastTradeDateInt().ToString() + "Log.txt";
            //        Debug.Print("path=" + fileName);
            //        _tw = new StreamWriter(fileName, true, Encoding.Default);
            //        _date = TimeUtilities.GetLastTradeDateInt();
            //    }
            //    _tw.WriteLine(TimeUtilities.StrNowTime + "   " + message);
            //    _tw.Flush();
            //}
        }
    }
}
