/*****************************************************************************\
*                                                                             *
* Str.cs -    Str functions, types, and definitions.                          *
*                                                                             *
*               Version 1.00 ★★★★★                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Server. All rights reserved.         *
*               Created by Todd.                                              *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OwLib
{
    public class CStrA
    {
        #region Lord 2016/1/20
        /// <summary>
        /// 获取证券代码的文件名称
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>文件名称</returns>
        public static String ConvertDBCodeToFileName(String code)
        {
            String fileName = code;
            if (fileName.IndexOf(".") != -1)
            {
                fileName = fileName.Substring(fileName.IndexOf('.') + 1) + fileName.Substring(0, fileName.IndexOf('.'));
            }
            fileName += ".txt";
            return fileName;
        }

        /// <summary>
        /// 将股票代码转化为成交代码
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>新浪代码</returns>
        public static String ConvertDBCodeToDealCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".");
            if (index > 0)
            {
                securityCode = securityCode.Substring(0, index);
            }
            return securityCode;
        }

        /// <summary>
        /// 东财代码转化为代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static String ConvertEMCodeToDBCode(String code)
        {
            return code.Substring(code.IndexOf(".") + 1) + code.Substring(0, code.IndexOf("."));
        }

        /// <summary>
        /// 将股票代码转化为新浪代码
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>新浪代码</returns>
        public static String ConvertDBCodeToSinaCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".SH");
            if (index > 0)
            {
                securityCode = "sh" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            else
            {
                securityCode = "sz" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            return securityCode;
        }

        /// <summary>
        /// 将文本文件中的股票代码转换成内存中的股票代码
        /// </summary>
        /// <param name="code">文件中的股票代码</param>
        /// <returns>内存中的股票代码</returns>
        public static String ConvertFileCodeToMemoryCode(String code)
        {
            int a = (code.IndexOf("."));
            return code.Substring(code.IndexOf(".") + 1, 2) + code.Substring(0, code.IndexOf(".")).ToLower();
        }

        /// <summary>
        /// 将新浪代码转化为股票代码
        /// </summary>
        /// <param name="code">新浪代码</param>
        /// <returns>股票代码</returns>
        public static String ConvertSinaCodeToDBCode(String code)
        {
            int equalIndex = code.IndexOf('=');
            int startIndex = code.IndexOf("var hq_str_") + 11;
            String securityCode = equalIndex > 0 ? code.Substring(startIndex, equalIndex - startIndex) : code;
            securityCode = securityCode.Substring(2) + "." + securityCode.Substring(0, 2).ToUpper();
            return securityCode;
        }

        /// <summary>
        /// 字符串转换为浮点型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数值</returns>
        public static double ConvertStrToDouble(String str)
        {
            double value = 0;
            double.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// 字符串转换为整形
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数值</returns>
        public static int ConvertStrToInt(String str)
        {
            int value = 0;
            int.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// 获取数据库转义字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>转义字符串</returns>
        public static String GetDBString(String str)
        {
            return str.Replace("'", "''");
        }

        /// <summary>
        /// 获取时间数值
        /// </summary>
        /// <param name="tm_year">年</param>
        /// <param name="tm_mon">月</param>
        /// <param name="tm_mday">日</param>
        /// <param name="tm_hour">小时</param>
        /// <param name="tm_min">分钟</param>
        /// <param name="tm_sec">秒</param>
        /// <param name="tm_msec">毫秒</param>
        /// <returns>时间数值</returns>
        public static double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec)
        {
            return (new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec) - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="num">数值</param>
        /// <param name="tm_year">年</param>
        /// <param name="tm_mon">月</param>
        /// <param name="tm_mday">日</param>
        /// <param name="tm_hour">小时</param>
        /// <param name="tm_min">分钟</param>
        /// <param name="tm_sec">秒</param>
        /// <param name="tm_msec">毫秒</param>
        public static void M130(double num, ref int tm_year, ref int tm_mon, ref int tm_mday, ref int tm_hour, ref int tm_min, ref int tm_sec, ref int tm_msec)
        {
            DateTime date = new DateTime(new DateTime(1970, 1, 1).Ticks + (long)num * 10000000);
            tm_year = date.Year;
            tm_mon = date.Month;
            tm_mday = date.Day;
            tm_hour = date.Hour;
            tm_min = date.Minute;
            tm_sec = date.Second;
        }
        #endregion
    }
}
