using System;

namespace OwLib
{
    /// <summary>
    /// 格式化方法类
    /// </summary>
    public abstract class FormatUtil : ConvertUtil
    {
        /// <summary>
        /// 格式化数字1
        /// 格式int
        /// 格式串
        ///  (C) Currency: . . . . . . . . ($123.00)
        ///  (D) Decimal:. . . . . . . . . -123
        ///  (E) Scientific: . . . . . . . -1.234500E+002
        ///  (F) Fixed point:. . . . . . . -123.45
        ///  (G) General:. . . . . . . . . -123
        ///  (N) Number: . . . . . . . . . -123.00
        ///  (P) Percent:. . . . . . . . . -12,345.00 %
        ///  (R) Round-trip: . . . . . . . -123.45
        ///  (X) Hexadecimal:. . . . . . . FFFFFF85
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(int obj, String formatstr)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }

        /// <summary>
        /// 小数格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(int obj, int n)
        {
            return Format(obj, "N" + n);
        }

        /// <summary>
        /// 百分比格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(int obj, int n)
        {
            return Format(obj, "P" + n);
        }

        /// <summary>
        /// 货币格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(int obj, int n)
        {
            return Format(obj, "C" + n);
        }

        /// <summary>
        /// 格式化数字2
        /// 格式short
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(String formatstr, short obj)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }

        /// <summary>
        /// 小数格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(short obj, int n)
        {
            return Format("N" + n, obj);
        }

        /// <summary>
        /// 百分比格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(short obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 货币格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(short obj, int n)
        {
            return Format("C" + n, obj);
        }


        /// <summary>
        /// 格式化数字3
        /// 格式long
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(String formatstr, long obj)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="obj">长整型数</param>
        /// <param name="n"></param>
        /// <returns>格式化结果</returns>
        public static String Format(long obj, int n)
        {
            return Format("N" + n, obj);
        }
        /// <summary>
        /// 格式化百分比
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(long obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 格式化货币类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(long obj, int n)
        {
            return Format("C" + n, obj);
        }
        /// <summary>
        /// 格式化数字3
        /// 格式Decimal
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(String formatstr, Decimal obj)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }

        /// <summary>
        /// 格式化十进制数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(Decimal obj, int n)
        {
            return Format("N" + n, obj);
        }

        /// <summary>
        /// 格式化百分比
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(Decimal obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 格式化货币类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(Decimal obj, int n)
        {
            return Format("C" + n, obj);
        }

        /// <summary>
        /// 格式化数字4
        /// 格式float
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(String formatstr, float obj)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }

        /// <summary>
        /// 格式化字符
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(float obj, int n)
        {
            return Format("N" + n, obj);
        }

        /// <summary>
        /// 格式化百分比
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(float obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 格式化货币类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(float obj, int n)
        {
            return Format("C" + n, obj);
        }

        /// <summary>
        /// 格式化字符5
        /// 格式double
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(String formatstr, double obj)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(double obj, int n)
        {
            return Format("N" + n, obj);
        }

        /// <summary>
        /// 百分比格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(double obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 货币类型格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(double obj, int n)
        {
            return Format("C" + n, obj);
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="formatstr"></param>
        /// <param name="obj"></param>
        /// <param name="defaultstr"></param>
        /// <returns></returns>
        public static String Format(String formatstr, object obj, String defaultstr)
        {
            String result = "";
            if (obj == null || obj == DBNull.Value)
                result = defaultstr;
            else
            {
                try
                {
                    result = Normal.ParseDouble(obj).ToString(formatstr);
                }
                catch
                {
                    result = obj.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="formatstr"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String Format(String formatstr, object obj)
        {
            return Format(formatstr, obj, "--");
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(object obj, int n)
        {
            return Format("N" + n, obj);
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <param name="defaultstr"></param>
        /// <returns></returns>
        public static String Format(object obj, int n, String defaultstr)
        {
            return Format("N" + n, obj, defaultstr);
        }

        /// <summary>
        /// 百分比格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(object obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 货币类型格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(object obj, int n)
        {
            return Format("C" + n, obj);
        }


        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="formatstr">格式化字符串</param>
        /// <param name="obj"></param>
        /// <param name="defaultstr"></param>
        /// <returns></returns>
        public static String Format(String formatstr, String obj, String defaultstr)
        {
            String result = "";
            if (obj == null)
                result = defaultstr;
            else
            {
                try
                {
                    result = Normal.ParseDouble(obj).ToString(formatstr);
                }
                catch
                {
                    result = obj.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="formatstr">格式化字符串</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String Format(String formatstr, String obj)
        {
            return Format(formatstr, obj, "--");
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Format(String obj, int n)
        {
            return Format("N" + n, obj);
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <param name="defaultstr"></param>
        /// <returns></returns>
        public static String Format(String obj, int n, String defaultstr)
        {
            return Format("N" + n, obj, defaultstr);
        }

        /// <summary>
        /// 百分比格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Percent(String obj, int n)
        {
            return Format("P" + n, obj);
        }

        /// <summary>
        /// 货币格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String Currency(String obj, int n)
        {
            return Format("C" + n, obj);
        }

        /// <summary>
        /// 格式化字符6
        /// 格式datetime
        /// 格式串
        ///  (yyyy-MM-dd hh:mm:ss) . . .   2004-06-26 20:11:04
        ///  (yyyy-MM-dd) . . . . . . . .  2004-06-26
        ///  (hh:mm:ss) . . . . . . . . .  20:11:04
        ///  (d) Short date: . . . . . . . 6/26/2004
        ///  (D) Long date:. . . . . . . . Saturday, June 26, 2004
        ///  (t) Short time: . . . . . . . 8:11 PM
        ///  (T) Long time:. . . . . . . . 8:11:04 PM
        ///  (f) Full date/short time: . . Saturday, June 26, 2004 8:11 PM
        ///  (R) RFC1123:. . . . . . . . . Sat, 26 Jun 2004 20:11:04 GMT
        ///  (s) Sortable: . . . . . . . . 2004-06-26T20:11:04
        ///  (u) Universal sortable: . . . 2004-06-26 20:11:04Z (invariant)
        ///  (U) Universal sortable: . . . Sunday, June 27, 2004 3:11:04 AM
        /// 举例:
        ///    Normal.Format("D",CommonContant.ServerDateTime);
        ///    Normal.Format("yyyy-MM-dd hh:mm:ss",CommonContant.ServerDateTime);
        ///    Normal.Format("yyyy年MM月dd日",CommonContant.ServerDateTime);
        ///    Normal.Format("hh:mm:ss",CommonContant.ServerDateTime);
        ///    Normal.Format("yyyy-MM-dd",CommonContant.ServerDateTime);
        ///    Normal.Format("hh:mm",CommonContant.ServerDateTime);
        /// 相当于   
        ///    CommonContant.ServerDateTime.ToLongDateString();
        ///    CommonContant.ServerDateTime.ToString("yyyy-MM-dd hh:mm:ss");  
        ///    CommonContant.ServerDateTime.ToLongDateString();
        ///    CommonContant.ServerDateTime.ToLongTimeString();
        ///    CommonContant.ServerDateTime.ToShortDateString();
        ///    CommonContant.ServerDateTime.ToShortTimeString();
        /// </summary>
        /// <param name="formatstr">格式</param>
        /// <param name="obj">要格式的数据</param>
        /// <returns>返回格式后的数据</returns>
        public static String Format(String formatstr, DateTime obj)
        {
            String result = "";
            try
            {
                result = obj.ToString(formatstr);
            }
            catch
            {
                result = obj.ToString();
            }
            return result;
        }


        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="formatstr"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDate(String formatstr, DateTime obj)
        {
            if (obj.Year == 1900)
                return "1900-01-01";
            return Format(formatstr, obj);
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="formatstr"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDate(String formatstr, String obj)
        {
            if (obj.IndexOf("1900") == 0)
                return "1900-01-01";
            return Format(formatstr, Normal.ParseDateTime(obj));
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="formatstr"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDate(String formatstr, object obj)
        {
            DateTime obj1 = Normal.ParseDateTime(obj);
            return FormatDate(formatstr, obj1);
        }
        
        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDate(DateTime obj)
        {
            return FormatDate("yyyy-MM-dd", obj);
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDate(String obj)
        {
            return FormatDate("yyyy-MM-dd", obj);
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDate(object obj)
        {
            return FormatDate("yyyy-MM-dd", obj);
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDateTime(DateTime obj)
        {
            return FormatDate("yyyy-MM-dd HH:mm:ss", obj);
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDateTime(String obj)
        {
            return FormatDate("yyyy-MM-dd HH:mm:ss", obj);
        }

        /// <summary>
        /// 格式化日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String FormatDateTime(object obj)
        {
            return FormatDate("yyyy-MM-dd HH:mm:ss", obj);
        }

    }
}
