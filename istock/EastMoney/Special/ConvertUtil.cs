using System;
using System.Text;

namespace  EmReportWatch.Util
{
    /// <summary>
    /// 转换类
    /// </summary>
    public abstract class ConvertUtil
    {

        //private static Regex doubleRegex=new Regex( @"^[-+]?\d{0,20}\.?\d{0,20}$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        //private static Regex intRegex = new Regex(@"^[-+]?\d{0,10}$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        /// <summary>
        /// 类型转换1
        /// 先处理null,再转换为string
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static string ParseString(object obj)
        {
            string result = string.Empty;
            if (obj != null)
                result = obj.ToString();
            return result;
        }

        /// <summary>
        /// 根据保留小数的位置将double型转化为string型
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="digit">保留位数</param>
        /// <param name="round">是否四舍五入</param>
        /// <returns></returns>
        public static string GetValueByDigit(double value, int digit, bool round)
        {
            if (!round)
            {
                StringBuilder sbFormat = new StringBuilder();
                string strValue = value.ToString();
                if (strValue.IndexOf(".") != -1)
                {
                    sbFormat.Append(strValue.Substring(0, strValue.IndexOf(".")));
                    if (digit > 0)
                    {
                        sbFormat.Append(".");
                    }
                    for (int i = 0; i < digit; i++)
                    {
                        int pos = strValue.IndexOf(".") + (i + 1);
                        if (pos <= strValue.Length - 1)
                        {
                            sbFormat.Append(strValue.Substring(pos, 1));
                        }
                        else
                        {
                            sbFormat.Append("0");
                        }
                    }
                }
                else
                {
                    sbFormat.Append(strValue);
                    if (digit > 0)
                    {
                        sbFormat.Append(".");
                    }
                    for (int i = 0; i < digit; i++)
                    {
                        sbFormat.Append("0");
                    }
                }
                return sbFormat.ToString();
            }
            else
            {
                StringBuilder format = new StringBuilder();
                format.Append("0");
                if (digit > 0)
                {
                    format.Append(".");
                    for (int i = 0; i < digit; i++)
                    {
                        format.Append("0");
                    }
                }
                return value.ToString(format.ToString());
            }
        }

        /// <summary>
        /// 生成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="str0">字符串</param>
        /// <returns>生成的字符串</returns>
        public static string ParseString(object obj, string str0)
        {
            string result = "";
            if (obj == null)
                result = str0;
            else
                result = obj.ToString();
            return result;
        }

        /// <summary>
        /// 生成二进制
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>生成的二进制符</returns>
        public static byte ParseByte(object obj)
        {
            return ParseByte(obj, byte.MinValue);
        }

        /// <summary>
        /// 生成二进制
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj0">二进制符</param>
        /// <returns>生成的二进制符</returns>
        public static byte ParseByte(object obj, byte obj0)
        {
            byte result = obj0;
            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                        result = byte.Parse(tempStr);
                }
            }
            catch
            {
            }
            return result;
        }
        /// <summary>
        /// 类型转换2
        /// 转换为short
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static short ParseShort(object obj)
        {

            return ParseShort(obj, short.MinValue); ;
        }

        /// <summary>
        /// 生成16位有符号的整数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj0"></param>
        /// <returns>返回的结果</returns>
        public static short ParseShort(object obj, short obj0)
        {
            short result = obj0;
            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                        result = Int16.Parse(tempStr);
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 类型转换3
        /// 转换为int
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static int ParseInt(object obj)
        {
            return ParseInt(obj, int.MinValue);
        }

        /// <summary>
        /// 生成整数型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj0"></param>
        /// <returns>返回的结果</returns>
        public static int ParseInt(object obj, int obj0)
        {
            int result = obj0;

            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                        result = Int32.Parse(tempStr);
                }
            }
            catch { }
            return result;
        }


        /// <summary>
        /// 类型转换4
        /// 转换为long
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static long ParseLong(object obj)
        {

            return ParseLong(obj, long.MinValue);
        }

        /// <summary>
        /// 类型转换为长整型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj0">长整型数</param>
        /// <returns>返回的结果</returns>
        public static long ParseLong(object obj, long obj0)
        {
            long result = obj0;
            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                        result = Int64.Parse(tempStr);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 类型转换5
        /// 转换为float
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static float ParseFloat(object obj)
        {

            return ParseFloat(obj, float.NaN);
        }
        /// <summary>
        /// 转换为单精度型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj0">单精度数</param>
        /// <returns>返回的结果</returns>
        public static float ParseFloat(object obj, float obj0)
        {
            float result = obj0;
            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        result = Single.Parse(tempStr);
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 类型转换6
        /// 转换为double
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static double ParseDouble(object obj)
        {
            return ParseDouble(obj, double.NaN);
        }

        /// <summary>
        /// 类型转换为double
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <param name="obj0"></param>
        /// <returns></returns>
        public static double ParseDouble(object obj, double obj0)
        {
            double result = obj0;
            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        result = double.Parse(tempStr);
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 类型转换7
        /// 转换为DateTime
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>结果</returns>
        public static DateTime ParseDateTime(object obj)
        {
            //DateTime result = new DateTime(1900, 1, 1);
            //result = DateTime.Parse(ParseString(obj));

            DateTime result;
            string timeStr = ParseString(obj);
            if (!DateTime.TryParse(timeStr, out result))
                result = new DateTime(1900, 1, 1);
            return result;
        }

        /// <summary>
        /// 转换为布尔型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>结果</returns>
        public static bool ParseBool(object obj)
        {
            return ParseBool(obj, false);
        }

        /// <summary>
        /// 转换为布尔型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj1">布尔型值</param>
        /// <returns>返回结果</returns>
        public static bool ParseBool(object obj, bool obj1)
        {
            bool result = obj1;
            try
            {
                result = bool.Parse(obj.ToString());
            }
            catch
            { }
            return result;
        }
        /// <summary>
        /// 转换为整数型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>整数型</returns>
        public static int ParseInt(bool obj)
        {
            return (obj == true) ? 1 : 0;
        }

        /// <summary>
        /// 类型转换8
        /// 转换为Decimal
        /// </summary>
        /// <param name="obj">要处理的数据</param>
        /// <returns>返回处理后的数据</returns>
        public static Decimal ParseDecimal(object obj)
        {
            return ParseDecimal(obj, Decimal.MinValue);
        }
        /// <summary>
        /// 转换为十进制数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="obj0">十进制数</param>
        /// <returns>返回的结果</returns>
        public static Decimal ParseDecimal(object obj, Decimal obj0)
        {
            Decimal result = obj0;
            try
            {
                if (!(obj == null))
                {
                    string tempStr = obj.ToString();
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        result = Decimal.Parse(tempStr);
                    }
                }
            }
            catch { }
            return result;
        }

    }
}
