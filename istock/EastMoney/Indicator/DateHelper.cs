using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class DateHelper
    {
        public static String GetHuanCompareReportDate(String dateTimeStr)
        {
            DateTime result = new DateTime();
            String str = String.Empty;
            if (!DateTime.TryParse(dateTimeStr, out result))
            {
                return dateTimeStr;
            }
            String str2 = result.ToString("MM-dd");
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "03-31"))
            {
                if (str2 != "06-30")
                {
                    if (str2 == "09-30")
                    {
                        return String.Format("{0}-{1}", result.Year, "06-30");
                    }
                    if (str2 != "12-31")
                    {
                        return str;
                    }
                    return String.Format("{0}-{1}", result.Year, "09-30");
                }
            }
            else
            {
                return String.Format("{0}-{1}", result.AddYears(-1).Year, "12-31");
            }
            return String.Format("{0}-{1}", result.Year, "03-31");
        }

        public static String GetSameCompareReportDate(String dateTimeStr)
        {
            DateTime result = new DateTime();
            String str = String.Empty;
            if (!DateTime.TryParse(dateTimeStr, out result))
            {
                return dateTimeStr;
            }
            String str2 = result.ToString("MM-dd");
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "03-31"))
            {
                if (str2 != "06-30")
                {
                    if (str2 == "09-30")
                    {
                        return String.Format("{0}-{1}", result.AddYears(-1).Year, "09-30");
                    }
                    if (str2 != "12-31")
                    {
                        return str;
                    }
                    return String.Format("{0}-{1}", result.AddYears(-1).Year, "12-31");
                }
            }
            else
            {
                return String.Format("{0}-{1}", result.AddYears(-1).Year, "03-31");
            }
            return String.Format("{0}-{1}", result.AddYears(-1).Year, "06-30");
        }

        public static String ResolveDate(String dateMark)
        {
            DateTime now = new DateTime();
            switch (dateMark.ToUpper())
            {
                case "E1W":
                    now = DateTime.Now.AddDays(-7.0);
                    break;

                case "E2W":
                    now = DateTime.Now.AddDays(-14.0);
                    break;

                case "E52W":
                    now = DateTime.Now.AddDays(-365.0);
                    break;

                case "E1M":
                    now = DateTime.Now.AddMonths(-1);
                    break;

                case "E3M":
                    now = DateTime.Now.AddMonths(-3);
                    break;

                case "E6M":
                    now = DateTime.Now.AddMonths(-6);
                    break;

                case "E1Y":
                    now = DateTime.Now.AddYears(-1);
                    break;

                case "E3Y":
                    now = DateTime.Now.AddYears(-3);
                    break;

                case "MS":
                    now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    break;

                case "YS":
                    now = new DateTime(DateTime.Now.Year, 1, 1);
                    break;

                case "N":
                    now = DateTime.Now;
                    break;

                default:
                    return dateMark;
            }
            return now.ToString("yyyy-MM-dd");
        }
    }
}
