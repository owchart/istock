using System;

namespace  EmReportWatch.Util
{
    /// <summary>
    /// 格式化类
    /// </summary>
    public class Normal : FormatUtil
    {
        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static string Trim(object obj)
        {
            return ParseString(obj).Trim();
        }


        /// <summary>
        /// 单引号处理,避免数据库操作时出现错误
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static string CheckPoint(object obj)
        {
            return ParseString(obj).Replace("'", "''");
        }

        /// <summary>
        /// 文本显示(单行)
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static string ListStr(object obj)
        {
            return ParseString(obj).Replace("<", "&lt;")
                .Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        /// <summary>
        /// 文本显示 (多行)
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static string ListStrs(object obj)
        {
            char ch1 = (char)13; // 换行
            char ch2 = (char)32; // 空格
            return ParseString(obj).Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace(ParseString(ch1), "<br>").Replace(
                Normal.ParseString(ch2), "&nbsp;");
        }

        /// <summary>
        /// IE解析
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static string ListStrsRev(object obj)
        {
            return ParseString(obj).Replace("&lt;", "<")
                .Replace("&gt;", ">");
        }
        /// <summary>
        /// 对sql server入库的日期值进行区分，小于1900-01-02的按null值来处理
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string formatSqlDate(DateTime d)
        {
            DateTime d1 = ParseDateTime("1900-01-02");
            if (d < d1)
                return "null";
            else
                return "'" + FormatDate(d) + "'";
        }

        /// <summary>
        /// 对sql server入库的日期值进行区分，小于1900-01-02的按null值来处理
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string formatSqlDateTime(DateTime d)
        {
            DateTime d1 = ParseDateTime("1900-01-02");
            if (d < d1)
                return "null";
            else
                return "'" + FormatDateTime(d) + "'";
        }


    }
}