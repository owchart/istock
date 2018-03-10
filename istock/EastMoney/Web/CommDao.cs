using System;
using System.Collections.Generic;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Util;

namespace OwLib
{
    public class CommDao
    {
        //保留小数位数
        const int _digit = 2;

        /// <summary>
        /// 转换成浮点型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static double SafeToDouble(object str)
        {
            if (str != null)
            {
                try
                {
                    return Convert.ToDouble(Math.Round(Convert.ToDecimal(str), _digit));
                }
                catch
                {
                    return 0;
                }
            }
            else return 0;
        }

        /// <summary>
        /// 转换成int型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static int SafeToInt(object str)
        {
            if (str != DBNull.Value)
            {
                try
                {
                    return Convert.ToInt32(str);
                }
                catch
                {
                    return 0;
                }
            }
            else return 0;
        }

        /// <summary>
        /// 安全转换字符串(重载，空的时候返回str_empty)
        /// </summary>
        /// <param name="str">需要处理的对象</param>
        /// <param name="str_empty">空和异常显示指定字符串str_empty</param>
        /// <returns></returns>
        internal static string SafeToString(object str, string str_empty)
        {
            try
            {
                if (str.ToString() != "")
                {
                    return Convert.ToString(str);
                }
                else
                    return str_empty;
            }
            catch (Exception ex)
            {
                return str_empty;
            }
        }

        /// <summary>
        /// 将系统时间强制转换成utc时间 且保证时间不变 为了解决的mongo中查询表达式的会强行转换成utc
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static DateTime DateToUTC(DateTime dateTime)
        {
            //return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Utc);
            return dateTime;
        }

        /// <summary>
        /// 返回当前星期几  add  liupf
        /// </summary>
        /// <param name="date"></param>
        /// <returns>数字1-7</returns>
        public static int CaculateWeekDayInt(DateTime date)
        {
            DayOfWeek week = date.DayOfWeek;
            //if (date == null)
            //    return 0;
            //double y = 0; double m = 0; double d = 0;
            //y = date.Year;
            //m = date.Month;
            //d = date.Day;
            //if (m == 1) m = 13;
            //if (m == 2) m = 14;
            //double week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
            //return (int)week + 1;
            switch (week)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 安全转换字符串
        /// </summary>
        /// <param name="str">需要处理的对象</param>
        /// <returns></returns>
        internal static string SafeToString(object str)
        {
            try
            {
                if (str != null)
                {
                    return Convert.ToString(str);
                }
                else return "";
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// 移除 html 某些标签 using 新闻 by LYM
        /// </summary>
        /// <param name="html">html内容</param>
        /// <returns>清理后的html内容</returns>
        public static string HtmlFilter(string html)
        {
            Parser parser = Parser.CreateParser(html, "utf-8");
            NodeFilter scriptNode = new TagNameFilter("script");
            NodeList nodes = parser.Parse(scriptNode);
            if (nodes.Count > 0)
            {
                HtmlFilter(nodes[0].Page.GetText().Replace(nodes[0].ToHtml(), ""));
                return nodes[0].Page.GetText().Replace(nodes[0].ToHtml(), "");
            }
            else
            {
                return html;
            }
        }

        /// <summary>
        /// 索引特殊字符替换
        /// </summary>
        internal static string SafeToIndexString(string str)
        {
            try
            {
                return str.Replace("\\", "\\\\").Replace(":", "\\:").Replace("(", "\\(").Replace(")", "\\)").Replace("-", "\\-").Replace("[", "\\[").Replace("]", "\\]").Replace("*", "\\*").Replace(".", "\\.").Replace(",", "\\,").Replace("{", "\\{").Replace("}", "\\}");
            }
            catch (Exception ex)
            {
                return str;
            }
        }

        /// <summary>
        /// 安全转换日期字符串
        /// </summary>
        /// <param name="str">需要处理的对象</param>
        /// <param name="type">日期格式</param>
        /// <returns></returns>
        internal static string SafeToDateString(object str, string type)
        {
            try
            {
                if (str != null)
                {
                    return Convert.ToDateTime(str).ToString(type);
                }
                else return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 转换成形如000,000,000.00的类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num">小数位</param>
        /// <returns></returns>
        internal static string SafeToCurrency(object str, int num)
        {
            if (str != DBNull.Value)
            {
                try
                {
                    return string.Format("{0:N" + num.ToString() + "}", str) == "" ? "-" : string.Format("{0:N" + num.ToString() + "}", str);
                }
                catch
                {
                    return "-";
                }
            }
            else return "-";
        }
        /// <summary>
        /// 转换成形如000,000,000.00的类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rate">汇率</param>
        /// <param name="num">小数位</param>
        /// <returns></returns>
        internal static string SafeToCurrency(object str, double rate, int num, int precision)
        {
            if (str != null)
            {

                try
                {
                    if (!string.IsNullOrEmpty(str.ToString()))
                    {
                        str = Math.Round(Convert.ToDouble(str) / rate, precision);
                        if (string.Format("{0:N" + num.ToString() + "}", str) == "")
                            return "-";
                        else
                        {
                            return string.Format("{0:N" + num.ToString() + "}", str);
                        }
                    }
                    else
                    {
                        return "-";
                    }
                    //return string.Format("{0:N" + num.ToString() + "}", str) == "" ? "-" : string.Format("{0:N" + num.ToString() + "}", str);
                }
                catch
                {
                    return "-";
                }
            }
            else return "-";
        }
        /// <summary>
        /// 转换成形如000,000,000.00的类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rate">汇率</param>
        /// <param name="num">小数位</param>
        /// <returns></returns>
        internal static string SafeToCurrency(object str, double rate, int num)
        {
            if (str != null)
            {

                try
                {
                    if (!string.IsNullOrEmpty(str.ToString()))
                    {
                        str = Math.Round(Convert.ToDouble(str) / rate, _digit);
                        if (string.Format("{0:N" + num.ToString() + "}", str) == "")
                            return "-";
                        else
                        {
                            return string.Format("{0:N" + num.ToString() + "}", str);
                        }
                    }
                    else
                    {
                        return "-";
                    }
                    //return string.Format("{0:N" + num.ToString() + "}", str) == "" ? "-" : string.Format("{0:N" + num.ToString() + "}", str);
                }
                catch
                {
                    return "-";
                }
            }
            else return "-";
        }

        /// <summary>
        /// 特殊类型， 如果是一个浮点型 那就转换成浮点型 否则转换成“-”  全局保留2位小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static object SafeToDoubleOrString(object str)
        {
            if (str != DBNull.Value)
            {
                try
                {
                    return Convert.ToDouble(Math.Round(Convert.ToDecimal(str), _digit));
                }
                catch
                {
                    return "-";
                }
            }
            else return "-";
        }
    }
}
