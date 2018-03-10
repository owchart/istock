using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class TimeParameter
    {
        public static String Get52WeekDateBeforeClosingDate()
        {
            return DateTime.Now.AddDays(-364.0).ToString("yyyy-MM-dd");
        }

        public static String GetBeforeSixMonthDay()
        {
            return DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
        }

        public static String GetBeforeThreeMonthDay()
        {
            return DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
        }

        public static String GetBeforeTwoYearDay()
        {
            return DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
        }

        public static String GetCurrentYear()
        {
            return DateTime.Today.ToString("yyyy");
        }

        public static String GetDateTime(DateTime inputDate, TimeSpan timeSpan)
        {
            return inputDate.Add(timeSpan).ToString("yyyy-MM-dd");
        }

        public static String GetLastestClosingDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static String GetLastFirstHalfYearStartDate()
        {
            DateTime time2 = new DateTime(DateTime.Now.Year, 6, 30);
            return time2.ToString("yyyy-MM-dd");
        }

        public static String GetLastMonthDay()
        {
            return DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        }

        public static String GetLastMonthEndDate()
        {
            return DateTime.Now.AddDays((double)(1 - DateTime.Now.Day)).AddDays(-1.0).ToString("yyyy-MM-dd");
        }

        public static String GetLastTradingDate()
        {
            return DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd");
        }

        public static String GetLastWeekDay()
        {
            return DateTime.Now.AddDays(-7.0).ToString("yyyy-MM-dd");
        }

        public static String GetLastWeekEndDate()
        {
            DateTime now = DateTime.Now;
            int num = Convert.ToInt32(now.DayOfWeek.ToString("d"));
            return now.AddDays((double)((1 - ((num == 0) ? 7 : num)) - 1)).ToString("yyyy-MM-dd");
        }

        public static String GetLastYear()
        {
            return DateTime.Today.AddYears(-1).ToString("yyyy-12-31");
        }

        public static String GetLastYearDay()
        {
            return DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
        }

        public static String GetLastYearEndDate()
        {
            DateTime time2 = new DateTime(DateTime.Now.Year - 1, 12, 0x1f);
            return time2.ToString("yyyy-MM-dd");
        }

        public static String GetLastYearFirstQuarter()
        {
            return DateTime.Today.AddYears(-1).ToString("yyyy-03-31");
        }

        public static String GetLastYearSecondQuarter()
        {
            return DateTime.Today.AddYears(-1).ToString("yyyy-06-30");
        }

        public static String GetLastYearThirdQuarter()
        {
            return DateTime.Today.AddYears(-1).ToString("yyyy-09-30");
        }

        public static String GetLatestDay()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static String GetLatestPhase()
        {
            return GetNowQuarterFirstDay().AddDays(-1.0).ToString("yyyy-MM-dd");
        }

        public static String GetListingFirstDate()
        {
            return DateTime.MinValue.ToString("yyyy-MM-dd");
        }

        public static String GetMonthEndDay(DateTime inputDate)
        {
            int day = inputDate.Day;
            int num2 = DateTime.DaysInMonth(inputDate.Year, inputDate.Month);
            return inputDate.AddDays((double)(num2 - day)).ToString("yyyy-MM-dd");
        }

        public static String GetMonthStartDay(DateTime inputDate)
        {
            int day = inputDate.Day;
            return inputDate.AddDays((double)(-day + 1)).ToString("yyyy-MM-dd");
        }

        private static DateTime GetNowQuarterFirstDay()
        {
            DateTime now = DateTime.Now;
            return now.AddMonths(-((now.Month - 1) % 3)).AddDays((double)(1 - now.Day));
        }

        public static String GetOneMonthDateBeforeClosingDate()
        {
            return DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        }

        public static String GetOneWeekDateBeforeClosingDate()
        {
            return DateTime.Now.AddDays(-7.0).ToString("yyyy-MM-dd");
        }

        public static String GetOneYearDateBeforeClosingDate()
        {
            return DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
        }

        public static String GetSecondHalfThisYearStartDate()
        {
            DateTime time2 = new DateTime(DateTime.Now.Year, 7, 1);
            return time2.ToString("yyyy-MM-dd");
        }

        public static String GetSixWeekDateBeforeClosingDate()
        {
            return DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
        }

        public static String GetThisMonthFirstDay()
        {
            return DateTime.Today.ToString("yyyy-MM-01");
        }

        public static String GetThisMonthStartDate()
        {
            DateTime time3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return time3.ToString("yyyy-MM-dd");
        }

        public static String GetThisWeekFirstDay()
        {
            DateTime today = DateTime.Today;
            int num = Convert.ToInt32(today.DayOfWeek.ToString("d"));
            return today.AddDays((double)(1 - ((num == 0) ? 7 : num))).ToString("yyyy-MM-dd");
        }

        public static String GetThisWeekMondayDate()
        {
            DateTime now = DateTime.Now;
            int num = Convert.ToInt32(now.DayOfWeek.ToString("d"));
            return now.AddDays((double)(1 - ((num == 0) ? 7 : num))).ToString("yyyy-MM-dd");
        }

        public static String GetThisYearFirstDay()
        {
            return DateTime.Today.ToString("yyyy-01-01");
        }

        public static String GetThisYearFirstQuarter()
        {
            return DateTime.Today.ToString("yyyy-03-31");
        }

        public static String GetThisYearSecondQuarter()
        {
            return DateTime.Today.ToString("yyyy-06-30");
        }

        public static String GetThisYearStartDate()
        {
            DateTime time2 = new DateTime(DateTime.Now.Year, 1, 1);
            return time2.ToString("yyyy-MM-dd");
        }

        public static String GetThisYearThirdQuarter()
        {
            return DateTime.Today.ToString("yyyy-09-30");
        }

        public static String GetThreeWeekDateBeforeClosingDate()
        {
            return DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
        }

        public static String GetThreeYearDateBeforeClosingDate()
        {
            return DateTime.Now.AddYears(-3).ToString("yyyy-MM-dd");
        }

        public static TimeSpan GetTimeSpan(DateTime today, DateTime thatDay)
        {
            return (TimeSpan)(thatDay - today);
        }

        public static String GetTradingDateBeforeClosingDate()
        {
            return DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd");
        }

        public static String GetTwoWeekDateBeforeClosingDate()
        {
            return DateTime.Now.AddDays(-14.0).ToString("yyyy-MM-dd");
        }

        public static String GetWeekEndDay(DateTime inputDate)
        {
            int num = Convert.ToInt32(inputDate.DayOfWeek.ToString("d"));
            return inputDate.AddDays((double)((1 - ((num == 0) ? 7 : num)) + 6)).ToString("yyyy-MM-dd");
        }

        public static String GetWeekStartDay(DateTime inputDate)
        {
            int num = Convert.ToInt32(inputDate.DayOfWeek.ToString("d"));
            return inputDate.AddDays((double)(1 - ((num == 0) ? 7 : num))).ToString("yyyy-MM-dd");
        }

        public static String GetYeadEndDay(DateTime inputDate)
        {
            int dayOfYear = inputDate.DayOfYear;
            int num2 = DateTime.IsLeapYear(inputDate.Year) ? 0x16e : 0x16d;
            DateTime.DaysInMonth(inputDate.Year, inputDate.Month);
            return inputDate.AddDays((double)(num2 - dayOfYear)).ToString("yyyy-MM-dd");
        }

        public static String GetYear(int year)
        {
            DateTime time = new DateTime(year, 1, 1);
            return time.ToString("yyyy-12-31");
        }

        public static String GetYearFirstQuarter(int year)
        {
            DateTime time = new DateTime(year, 1, 1);
            return time.ToString("yyyy-03-31");
        }

        public static String GetYearSecondQuarter(int year)
        {
            DateTime time = new DateTime(year, 1, 1);
            return time.ToString("yyyy-06-30");
        }

        public static String GetYearStartDay(DateTime inputDate)
        {
            int dayOfYear = inputDate.DayOfYear;
            DateTime.IsLeapYear(inputDate.Year);
            DateTime.DaysInMonth(inputDate.Year, inputDate.Month);
            return inputDate.AddDays((double)(-dayOfYear + 1)).ToString("yyyy-MM-dd");
        }

        public static String GetYearThirdQuarter(int year)
        {
            DateTime time = new DateTime(year, 1, 1);
            return time.ToString("yyyy-09-30");
        }

        public static bool IsRelativeDate(String parameter)
        {
            switch (parameter)
            {
                case "GetTradingDateBeforeClosingDate":
                case "GetOneYearDateBeforeClosingDate":
                case "GetOneWeekDateBeforeClosingDate":
                case "GetThreeYearDateBeforeClosingDate":
                case "GetTwoWeekDateBeforeClosingDate":
                case "Get52WeekDateBeforeClosingDate":
                case "GetOneMonthDateBeforeClosingDate":
                case "GetThreeWeekDateBeforeClosingDate":
                case "GetSixWeekDateBeforeClosingDate":
                    return true;
            }
            return false;
        }
    }
}
