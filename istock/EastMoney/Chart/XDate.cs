/********************************************************************************\
*                                                                                *
* XDate.cs -    XDate functions, types, and definitions                          *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;

namespace OwLib
{
    /// <summary>
    /// 日期
    /// </summary>
    public struct XDate : IComparable
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="xlDate">日期</param>
        public XDate(double xlDate)
        {
            m_xlDate = xlDate;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="dateTime">日期</param>
        public XDate(DateTime dateTime)
        {
            m_xlDate = CalendarDateToXLDate(dateTime.Year, dateTime.Month,
                            dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second,
                            dateTime.Millisecond);
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public XDate(int year, int month, int day)
        {
            m_xlDate = CalendarDateToXLDate(year, month, day, 0, 0, 0);
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public XDate(int year, int month, int day, int hour, int minute, int second)
        {
            m_xlDate = CalendarDateToXLDate(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public XDate(int year, int month, int day, int hour, int minute, double second)
        {
            m_xlDate = CalendarDateToXLDate(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        public XDate(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            m_xlDate = CalendarDateToXLDate(year, month, day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public XDate(XDate rhs)
        {
            m_xlDate = rhs.m_xlDate;
        }

        public const double XLDay1 = 2415018.5;
        public const double JulDayMin = 0.0;
        public const double JulDayMax = 5373483.5;
        public const double XLDayMin = JulDayMin - XLDay1;
        public const double XLDayMax = JulDayMax - XLDay1;
        public const double MonthsPerYear = 12.0;
        public const double HoursPerDay = 24.0;
        public const double MinutesPerHour = 60.0;
        public const double SecondsPerMinute = 60.0;
        public const double MinutesPerDay = 1440.0;
        public const double SecondsPerDay = 86400.0;
        public const double MillisecondsPerSecond = 1000.0;
        public const double MillisecondsPerDay = 86400000.0;
        public const String DefaultFormatStr = "g";

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTime
        {
            get { return XLDateToDateTime(m_xlDate); }
            set { m_xlDate = DateTimeToXLDate(value); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double DecimalYear
        {
            get { return XLDateToDecimalYear(m_xlDate); }
            set { m_xlDate = DecimalYearToXLDate(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValidDate
        {
            get { return m_xlDate >= XLDayMin && m_xlDate <= XLDayMax; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double JulianDay
        {
            get { return XLDateToJulianDay(m_xlDate); }
            set { m_xlDate = JulianDayToXLDate(value); }
        }

        private double m_xlDate;

        /// <summary>
        /// 
        /// </summary>
        public double XLDate
        {
            get { return m_xlDate; }
            set { m_xlDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dMilliseconds">毫秒</param>
        public void AddMilliseconds(double dMilliseconds)
        {
            m_xlDate += dMilliseconds / MillisecondsPerDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dSeconds">秒</param>
        public void AddSeconds(double dSeconds)
        {
            m_xlDate += dSeconds / SecondsPerDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dMinutes">分钟</param>
        public void AddMinutes(double dMinutes)
        {
            m_xlDate += dMinutes / MinutesPerDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dHours">小时</param>
        public void AddHours(double dHours)
        {
            m_xlDate += dHours / HoursPerDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dDays">日</param>
        public void AddDays(double dDays)
        {
            m_xlDate += dDays;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dMonths">月</param>
        public void AddMonths(double dMonths)
        {
            int iMon = (int)dMonths;
            double monFrac = Math.Abs(dMonths - (double)iMon);
            int sMon = Math.Sign(dMonths);
            int year, month, day, hour, minute, second;
            XLDateToCalendarDate(m_xlDate, out year, out month, out day, out hour, out minute, out second);
            if (iMon != 0)
            {
                month += iMon;
                m_xlDate = CalendarDateToXLDate(year, month, day, hour, minute, second);
            }
            if (sMon != 0)
            {
                double xlDate2 = CalendarDateToXLDate(year, month + sMon, day, hour, minute, second);
                m_xlDate += (xlDate2 - m_xlDate) * monFrac;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dYears">年</param>
        public void AddYears(double dYears)
        {
            int iYear = (int)dYears;
            double yearFrac = Math.Abs(dYears - (double)iYear);
            int sYear = Math.Sign(dYears);
            int year, month, day, hour, minute, second;
            XLDateToCalendarDate(m_xlDate, out year, out month, out day, out hour, out minute, out second);
            if (iYear != 0)
            {
                year += iYear;
                m_xlDate = CalendarDateToXLDate(year, month, day, hour, minute, second);
            }
            if (sYear != 0)
            {
                double xlDate2 = CalendarDateToXLDate(year + sYear, month, day, hour, minute, second);
                m_xlDate += (xlDate2 - m_xlDate) * yearFrac;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        /// <returns></returns>
        public static double CalendarDateToXLDate(int year, int month, int day,
    int hour, int minute, int second, int millisecond)
        {
            double ms = millisecond;
            NormalizeCalendarDate(ref year, ref month, ref day, ref hour, ref minute, ref second,
                        ref ms);
            return CalendarDateToXLDate(year, month, day, hour, minute, second, ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <returns></returns>
        public static double CalendarDateToXLDate(int year, int month, int day,
            int hour, int minute, int second)
        {
            double ms = 0;
            NormalizeCalendarDate(ref year, ref month, ref day, ref hour, ref minute,
                    ref second, ref ms);
            return CalendarDateToXLDate(year, month, day, hour, minute, second, ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <returns></returns>
        public static double CalendarDateToXLDate(int year, int month, int day,
            int hour, int minute, double second)
        {
            int sec = (int)second;
            double ms = (second - sec) * MillisecondsPerSecond;
            NormalizeCalendarDate(ref year, ref month, ref day, ref hour, ref minute, ref sec,
                    ref ms);
            return CalendarDateToXLDate(year, month, day, hour, minute, sec, ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <returns></returns>
        public static double CalendarDateToJulianDay(int year, int month, int day,
            int hour, int minute, int second)
        {
            double ms = 0;
            NormalizeCalendarDate(ref year, ref month, ref day, ref hour, ref minute,
                ref second, ref ms);
            return CalendarDateToJulianDay(year, month, day, hour, minute, second, ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        /// <returns></returns>
        public static double CalendarDateToJulianDay(int year, int month, int day,
            int hour, int minute, int second, int millisecond)
        {
            double ms = millisecond;
            NormalizeCalendarDate(ref year, ref month, ref day, ref hour, ref minute,
                        ref second, ref ms);
            return CalendarDateToJulianDay(year, month, day, hour, minute, second, ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        /// <returns></returns>
        private static double CalendarDateToXLDate(int year, int month, int day, int hour,
            int minute, int second, double millisecond)
        {
            return JulianDayToXLDate(CalendarDateToJulianDay(year, month, day, hour, minute,
                        second, millisecond));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        /// <returns></returns>
        private static double CalendarDateToJulianDay(int year, int month, int day, int hour,
                    int minute, int second, double millisecond)
        {
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }
            double A = Math.Floor((double)year / 100.0);
            double B = 2 - A + Math.Floor(A / 4.0);
            return Math.Floor(365.25 * ((double)year + 4716.0)) +
                    Math.Floor(30.6001 * (double)(month + 1)) +
                    (double)day + B - 1524.5 +
                    hour / HoursPerDay + minute / MinutesPerDay + second / SecondsPerDay +
                    millisecond / MillisecondsPerDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        private static bool CheckValidDate(double xlDate)
        {
            return xlDate >= XLDayMin && xlDate <= XLDayMax;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns></returns>
        public int CompareTo(object target)
        {
            if (!(target is XDate))
                throw new ArgumentException();
            return (this.XLDate).CompareTo(((XDate)target).XLDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static double DateTimeToXLDate(DateTime dt)
        {
            return CalendarDateToXLDate(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second,
                                        dt.Millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearDec">年</param>
        /// <returns></returns>
        public static double DecimalYearToXLDate(double yearDec)
        {
            int year = (int)yearDec;
            double jDay1 = CalendarDateToJulianDay(year, 1, 1, 0, 0, 0);
            double jDay2 = CalendarDateToJulianDay(year + 1, 1, 1, 0, 0, 0);
            return JulianDayToXLDate((yearDec - (double)year) * (jDay2 - jDay1) + jDay1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public void GetDate(out int year, out int month, out int day)
        {
            int hour, minute, second;
            XLDateToCalendarDate(m_xlDate, out year, out month, out day, out hour, out minute, out second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public void GetDate(out int year, out int month, out int day,
                out int hour, out int minute, out int second)
        {
            XLDateToCalendarDate(m_xlDate, out year, out month, out day, out hour, out minute, out second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetDayOfYear()
        {
            return XLDateToDayOfYear(m_xlDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jDay">日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public static void JulianDayToCalendarDate(double jDay, out int year, out int month,
    out int day, out int hour, out int minute, out int second)
        {
            double ms = 0;
            JulianDayToCalendarDate(jDay, out year, out month,
                    out day, out hour, out minute, out second, out ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jDay">日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public static void JulianDayToCalendarDate(double jDay, out int year, out int month,
            out int day, out int hour, out int minute, out double second)
        {
            int sec;
            double ms;
            JulianDayToCalendarDate(jDay, out year, out month,
                    out day, out hour, out minute, out sec, out ms);
            second = sec + ms / MillisecondsPerSecond;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jDay">日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        public static void JulianDayToCalendarDate(double jDay, out int year, out int month,
            out int day, out int hour, out int minute, out int second, out double millisecond)
        {
            jDay += 0.0005 / SecondsPerDay;
            double z = Math.Floor(jDay + 0.5);
            double f = jDay + 0.5 - z;
            double alpha = Math.Floor((z - 1867216.25) / 36524.25);
            double A = z + 1.0 + alpha - Math.Floor(alpha / 4);
            double B = A + 1524.0;
            double C = Math.Floor((B - 122.1) / 365.25);
            double D = Math.Floor(365.25 * C);
            double E = Math.Floor((B - D) / 30.6001);
            day = (int)Math.Floor(B - D - Math.Floor(30.6001 * E) + f);
            month = (int)((E < 14.0) ? E - 1.0 : E - 13.0);
            year = (int)((month > 2) ? C - 4716 : C - 4715);
            double fday = (jDay - 0.5) - Math.Floor(jDay - 0.5);
            fday = (fday - (long)fday) * HoursPerDay;
            hour = (int)fday;
            fday = (fday - (long)fday) * MinutesPerHour;
            minute = (int)fday;
            fday = (fday - (long)fday) * SecondsPerMinute;
            second = (int)fday;
            fday = (fday - (long)fday) * MillisecondsPerSecond;
            millisecond = fday;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static double MakeValidDate(double xlDate)
        {
            if (xlDate < XLDayMin)
                xlDate = XLDayMin;
            if (xlDate > XLDayMax)
                xlDate = XLDayMax;
            return xlDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        private static void NormalizeCalendarDate(ref int year, ref int month, ref int day,
                                    ref int hour, ref int minute, ref int second,
                                    ref double millisecond)
        {
            int carry = (int)Math.Floor(millisecond / MillisecondsPerSecond);
            millisecond -= carry * (int)MillisecondsPerSecond;
            second += carry;
            carry = (int)Math.Floor(second / SecondsPerMinute);
            second -= carry * (int)SecondsPerMinute;
            minute += carry;
            carry = (int)Math.Floor((double)minute / MinutesPerHour);
            minute -= carry * (int)MinutesPerHour;
            hour += carry;
            carry = (int)Math.Floor((double)hour / HoursPerDay);
            hour -= carry * (int)HoursPerDay;
            day += carry;
            carry = (int)Math.Floor((double)month / MonthsPerYear);
            month -= carry * (int)MonthsPerYear;
            year += carry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public void SetDate(int year, int month, int day)
        {
            m_xlDate = CalendarDateToXLDate(year, month, day, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public void SetDate(int year, int month, int day, int hour, int minute, int second)
        {
            m_xlDate = CalendarDateToXLDate(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public static void XLDateToCalendarDate(double xlDate, out int year, out int month,
            out int day, out int hour, out int minute, out int second)
        {
            double jDay = XLDateToJulianDay(xlDate);
            JulianDayToCalendarDate(jDay, out year, out month, out day, out hour,
                out minute, out second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="millisecond">毫秒</param>
        public static void XLDateToCalendarDate(double xlDate, out int year, out int month,
            out int day, out int hour, out int minute, out int second, out int millisecond)
        {
            double jDay = XLDateToJulianDay(xlDate);
            double ms;
            JulianDayToCalendarDate(jDay, out year, out month, out day, out hour,
                out minute, out second, out ms);
            millisecond = (int)ms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public static void XLDateToCalendarDate(double xlDate, out int year, out int month,
            out int day, out int hour, out int minute, out double second)
        {
            double jDay = XLDateToJulianDay(xlDate);
            JulianDayToCalendarDate(jDay, out year, out month, out day, out hour,
                out minute, out second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static double XLDateToJulianDay(double xlDate)
        {
            return xlDate + XLDay1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jDay">日期</param>
        /// <returns></returns>
        public static double JulianDayToXLDate(double jDay)
        {
            return jDay - XLDay1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static double XLDateToDecimalYear(double xlDate)
        {
            int year, month, day, hour, minute, second;
            XLDateToCalendarDate(xlDate, out year, out month, out day, out hour, out minute, out second);
            double jDay1 = CalendarDateToJulianDay(year, 1, 1, 0, 0, 0);
            double jDay2 = CalendarDateToJulianDay(year + 1, 1, 1, 0, 0, 0);
            double jDayMid = CalendarDateToJulianDay(year, month, day, hour, minute, second);
            return (double)year + (jDayMid - jDay1) / (jDay2 - jDay1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <param name="fmtStr">格式化</param>
        /// <returns></returns>
        public static String ToString(double xlDate, String fmtStr)
        {
            int year, month, day, hour, minute, second, millisecond;
            if (!CheckValidDate(xlDate))
                return "Date Error";
            XLDateToCalendarDate(xlDate, out year, out month, out day, out hour, out minute,
                                            out second, out millisecond);
            if (year <= 0)
            {
                year = 1 - year;
                fmtStr = fmtStr + " (BC)";
            }
            if (fmtStr.IndexOf("[d]") >= 0)
            {
                fmtStr = fmtStr.Replace("[d]", ((int)xlDate).ToString());
                xlDate -= (int)xlDate;
            }
            if (fmtStr.IndexOf("[h]") >= 0 || fmtStr.IndexOf("[hh]") >= 0)
            {
                fmtStr = fmtStr.Replace("[h]", ((int)(xlDate * 24)).ToString("d"));
                fmtStr = fmtStr.Replace("[hh]", ((int)(xlDate * 24)).ToString("d2"));
                xlDate = (xlDate * 24 - (int)(xlDate * 24)) / 24.0;
            }
            if (fmtStr.IndexOf("[m]") >= 0 || fmtStr.IndexOf("[mm]") >= 0)
            {
                fmtStr = fmtStr.Replace("[m]", ((int)(xlDate * 1440)).ToString("d"));
                fmtStr = fmtStr.Replace("[mm]", ((int)(xlDate * 1440)).ToString("d2"));
                xlDate = (xlDate * 1440 - (int)(xlDate * 1440)) / 1440.0;
            }
            if (fmtStr.IndexOf("[s]") >= 0 || fmtStr.IndexOf("[ss]") >= 0)
            {
                fmtStr = fmtStr.Replace("[s]", ((int)(xlDate * 86400)).ToString("d"));
                fmtStr = fmtStr.Replace("[ss]", ((int)(xlDate * 86400)).ToString("d2"));
                xlDate = (xlDate * 86400 - (int)(xlDate * 86400)) / 86400.0;
            }
            if (fmtStr.IndexOf("[f]") >= 0)
                fmtStr = fmtStr.Replace("[f]", ((int)(xlDate * 864000)).ToString("d"));
            if (fmtStr.IndexOf("[ff]") >= 0)
                fmtStr = fmtStr.Replace("[ff]", ((int)(xlDate * 8640000)).ToString("d"));
            if (fmtStr.IndexOf("[fff]") >= 0)
                fmtStr = fmtStr.Replace("[fff]", ((int)(xlDate * 86400000)).ToString("d"));
            if (fmtStr.IndexOf("[ffff]") >= 0)
                fmtStr = fmtStr.Replace("[ffff]", ((int)(xlDate * 864000000)).ToString("d"));
            if (fmtStr.IndexOf("[fffff]") >= 0)
                fmtStr = fmtStr.Replace("[fffff]", ((int)(xlDate * 8640000000)).ToString("d"));
            if (year > 9999)
                year = 9999;
            DateTime dt = new DateTime(year, month, day, hour, minute, second, millisecond);
            return dt.ToString(fmtStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static double XLDateToDayOfYear(double xlDate)
        {
            int year, month, day, hour, minute, second;
            XLDateToCalendarDate(xlDate, out year, out month, out day,
                                    out hour, out minute, out second);
            return XLDateToJulianDay(xlDate) - CalendarDateToJulianDay(year, 1, 1, 0, 0, 0) + 1.0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static int XLDateToDayOfWeek(double xlDate)
        {
            return (int)(XLDateToJulianDay(xlDate) + 1.5) % 7;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static DateTime XLDateToDateTime(double xlDate)
        {
            int year, month, day, hour, minute, second, millisecond;
            XLDateToCalendarDate(xlDate, out year, out month, out day,
                                    out hour, out minute, out second, out millisecond);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs">其他对象1</param>
        /// <param name="rhs">其他对象2</param>
        /// <returns></returns>
        public static double operator -(XDate lhs, XDate rhs)
        {
            return lhs.XLDate - rhs.XLDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs">其他对象1</param>
        /// <param name="rhs">其他对象2</param>
        /// <returns></returns>
        public static XDate operator -(XDate lhs, double rhs)
        {
            lhs.m_xlDate -= rhs;
            return lhs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs">其他对象1</param>
        /// <param name="rhs">其他对象2</param>
        /// <returns></returns>
        public static XDate operator +(XDate lhs, double rhs)
        {
            lhs.m_xlDate += rhs;
            return lhs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDate">日期</param>
        /// <returns></returns>
        public static XDate operator ++(XDate xDate)
        {
            xDate.m_xlDate += 1.0;
            return xDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDate">日期</param>
        /// <returns></returns>
        public static XDate operator --(XDate xDate)
        {
            xDate.m_xlDate -= 1.0;
            return xDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDate">日期</param>
        /// <returns></returns>
        public static implicit operator double(XDate xDate)
        {
            return xDate.m_xlDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDate">日期</param>
        /// <returns></returns>
        public static implicit operator float(XDate xDate)
        {
            return (float)xDate.m_xlDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlDate">日期</param>
        /// <returns></returns>
        public static implicit operator XDate(double xlDate)
        {
            return new XDate(xlDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDate">日期</param>
        /// <returns></returns>
        public static implicit operator DateTime(XDate xDate)
        {
            return XLDateToDateTime(xDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static implicit operator XDate(DateTime dt)
        {
            return new XDate(DateTimeToXLDate(dt));
        }
        #endregion
    }
}
