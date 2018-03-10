using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using EmCore;
using System.Collections;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;
using System.IO;
using System.Linq;

namespace OwLib
{
    public class CommonController
    {
        /// <summary>
        /// 获取过滤日期的条件表达式
        /// </summary>
        /// <param name="yearList"></param>
        /// <param name="reportTypeList"></param>
        /// <param name="type">0：没有输入查询类型，默认查询近三年;1:常用查询;2:自定义查询;3:常用查询（不包含报告期）<</param>
        /// <param name="reportDateField"></param>
        /// <param name="dateRangeType">//0:按照自然年度;1:按照最近x期数(针对近x年的情况);2:多取一年数据，然后过滤(针对近x年的情况),int dateRangeType=0</param>
        /// <returns></returns>
        public static List<Expression> GetDateRangeExpression(string yearList, string reportTypeList, int type = 0, string reportDateField = "ENDDATE")
        {
            List<Expression> expressionList = new List<Expression>();
            List<object> rangeList = new List<object>();
            int yearCount = String.IsNullOrEmpty(yearList) ? 0 : yearList.Split(',').Length;

            if (type == 0)
            {
                expressionList.Add(Expression.Ge(reportDateField, Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")).AddYears(-3)));
            }
            else if (type == 1)
            {
                if (!String.IsNullOrEmpty(reportTypeList))
                {
                    expressionList.Add(Expression.In("STR_YUEFEN", reportTypeList.Split(',')));
                }
                if ((yearCount == 3 || yearCount == 4 || yearCount == 6))
                {
                    expressionList.Add(Expression.Ge(reportDateField, Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")).AddYears(-(yearCount - 1))));
                }
                else
                {
                    expressionList.Add(Expression.In("STR_YEAR", yearList.Split(',')));

                }
                #region 淘汰的查询逻辑（近几期）
                //if (dateRangeType == 0)
                //{
                //    expressionList.Add(Expression.In("STR_YEAR", yearList.Split(',')));
                //}else if (dateRangeType == 1)
                //{
                //    if ((yearCount == 3 || yearCount == 4 || yearCount == 6))
                //    {
                //        return expressionList;
                //    }
                //    else
                //    {
                //        expressionList.Add(Expression.In("STR_YEAR", yearList.Split(',')));

                //    }
                //}
                //else if (dateRangeType == 2)
                //{
                //    if ((yearCount == 3 || yearCount == 4 || yearCount == 6))
                //    {
                //        yearList += "," + (DateTime.Now.Year - yearCount).ToString();//往前多加一年，可以多查询一年的数据，然后程序过滤
                //    }
                //    expressionList.Add(Expression.In("STR_YEAR", yearList.Split(',')));
                //}
                #endregion
            }
            else if (type == 2)
            {
                reportTypeList.Split(',').ToList().ForEach(c =>
                {
                    rangeList.Add(new DateTime(int.Parse(c.Split('_')[0]), GetMonth(c.Split('_')[1]), GetDay(c.Split('_')[1])));
                }
                );
                expressionList.Add(Expression.In(reportDateField, rangeList.ToArray()));
            }
            else if (type == 3)
            {
                if (!String.IsNullOrEmpty(yearList))
                {
                    //expressionList.Add(Expression.Between(reportDateField, Convert.ToDateTime(yearList.Split(',')[0]),
                    //    Convert.ToDateTime(yearList.Split(',')[1])));
                    expressionList.Add(Expression.Ge(reportDateField, Convert.ToDateTime(yearList.Split(',')[0])));
                    expressionList.Add(Expression.Le(reportDateField, Convert.ToDateTime(yearList.Split(',')[1])));
                }
            }


            return expressionList;
        }


        /// <summary>
        /// 获取报告期的报告月
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static int GetMonth(string p)
        {
            int result = 3;

            switch (CommDao.SafeToInt(p))
            {
                case 1:
                    result = 3;
                    break;
                case 2:
                    result = 6;
                    break;
                case 3:
                    result = 9;
                    break;
                case 4:
                    result = 12;
                    break;
                case 5:
                    result = 6;
                    break;
                case 6:
                    result = 12;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取报告期的报告日
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static int GetDay(string p)
        {
            int result = 30;

            switch (CommDao.SafeToInt(p))
            {
                case 1:
                    result = 31;
                    break;
                case 2:
                    result = 30;
                    break;
                case 3:
                    result = 30;
                    break;
                case 4:
                    result = 31;
                    break;
                case 5:
                    result = 30;
                    break;
                case 6:
                    result = 31;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取过滤日期的条件表达式 范围选择templateMode为1和5的时候用 命令格式应为$-rpt\r\n$name=EM_AB_RISK\r\n$secucode=600000.SH\r\n$StartDate=2012-01-01,EndDate=2014-07-15\r\n
        /// </summary>
        /// <param name="yearList"></param>
        /// <param name="reportTypeList"></param>
        /// <param name="type">0：没有输入查询类型，默认查询近三年;1:常用查询;3:自定义查询;2:常用查询（不包含报告期）<</param>
        /// <param name="reportDateField"></param>
        /// <param name="dateRangeType">//0:按照自然年度;1:按照最近x期数(针对近x年的情况);2:多取一年数据，然后过滤(针对近x年的情况),int dateRangeType=0</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDateRangeExpressionForReport(string yearList, string reportTypeList, int type = 0, string sort = "desc", string reportDateField = "ReportDate")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            int yearCount = String.IsNullOrEmpty(yearList) ? 0 : yearList.Split(',').Length;

            if (type == 0)
            {
                result.Add(reportDateField, transferParamToDateTimeString((DateTime.Now.Year - 3).ToString(), "1,2,3,4", sort));
            }
            else if (type == 1)
            {
                result.Add(reportDateField, transferParamToDateTimeString(yearList, reportTypeList, sort));
            }
            else if (type == 2)
            {
                string temp_years = string.Empty;
                string temp_reports = string.Empty;
                reportTypeList.Split(',').ToList().ForEach(c =>
                {
                    temp_years += c.Split('_')[0] + ",";
                    temp_reports += c.Split('_')[1] + ",";
                }
                );
                result.Add(reportDateField, transferParamToDateTimeString(temp_years.Substring(0, temp_years.Length), temp_reports.Substring(0, temp_reports.Length), sort));
            }
            else if (type == 3)
            {
                //当type为3是不应该走此函数 应该为GetDateRangeDic方法
            }


            return result;
        }


        public static string transferParamToDateTimeString(string yearList, string reportType, string sort)
        {
            string result = string.Empty;
            String[] years, reports;
            if (!string.IsNullOrEmpty(yearList) && !string.IsNullOrEmpty(reportType))
            {
                if (sort.Equals("desc"))
                {
                    years = yearList.Split(',');
                    reports = reportType.Split(',');
                }
                else
                {
                    years = yearList.Split(',');
                    Array.Reverse(years);
                    reports = reportType.Split(',');
                    Array.Reverse(reports);
                }
                foreach (string item in years)
                {
                    foreach (string reportItem in reports)
                    {
                        switch (reportItem)
                        {
                            case "1":
                                result += item + "-03-31,";
                                break;
                            case "2":
                            case "5":
                                result += item + "-06-30,";
                                break;
                            case "3":
                                result += item + "-09-30,";
                                break;
                            case "4":
                            case "6":
                                result += item + "-12-31,";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return result.Substring(0, result.Length - 1);
        }

        /// <summary>
        /// 获取过滤日期的条件表达式 范围选择templateMode为1和5的时候用 命令格式应为$-rpt\r\n$name=EM_AB_RISK\r\n$secucode=600000.SH\r\n$StartDate=2012-01-01,EndDate=2014-07-15\r\n
        /// </summary>
        /// <param name="yearList"></param>
        /// <param name="reportTypeList"></param>
        /// <param name="type">0：没有输入查询类型，默认查询近三年;1:常用查询;3:自定义查询;2:常用查询（不包含报告期）<</param>
        /// <param name="reportDateField"></param>
        /// <param name="dateRangeType">//0:按照自然年度;1:按照最近x期数(针对近x年的情况);2:多取一年数据，然后过滤(针对近x年的情况),int dateRangeType=0</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDateRangeDic(string yearList, string reportTypeList, int type = 0, string reportDateField = "TradeDate")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            int yearCount = String.IsNullOrEmpty(yearList) ? 0 : yearList.Split(',').Length;

            if (type == 0)
            {
                //expressionList.Add(Expression.Ge(reportDateField, Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")).AddYears(-3)));
                result.Add("StartDate", DateTime.Now.AddYears(-3).ToString("yyyy-01-01"));
                result.Add("EndDate", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (type == 1)
            {
                if (yearCount > 1)
                {
                    //expressionList.Add(Expression.Ge(reportDateField, Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")).AddYears(-(yearCount - 1))));
                    result.Add("StartDate", DateTime.Now.AddYears(-(yearCount - 1)).ToString("yyyy-01-01"));
                    result.Add("EndDate", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (yearCount == 1)
                {
                    //expressionList.Add(Expression.In("STR_YEAR", yearList.Split(',')));
                    result.Add("StartDate", yearList + "-01-01");
                    result.Add("EndDate", DateTime.Now.ToString(yearList + "-12-31"));
                }
                else
                {

                }
            }
            else if (type == 2)
            {
                //reportTypeList.Split(',').ToList().ForEach(c =>
                //{
                //    rangeList.Add(new DateTime(int.Parse(c.Split('_')[0]), GetMonth(c.Split('_')[1]), GetDay(c.Split('_')[1])));
                //}
                //);
                //expressionList.Add(Expression.In(reportDateField, rangeList.ToArray()));
            }
            else if (type == 3)
            {
                if (!String.IsNullOrEmpty(yearList))
                {
                    //expressionList.Add(Expression.Between(reportDateField, Convert.ToDateTime(yearList.Split(',')[0]),
                    //    Convert.ToDateTime(yearList.Split(',')[1])));
                    //expressionList.Add(Expression.Ge(reportDateField, Convert.ToDateTime(yearList.Split(',')[0])));
                    //expressionList.Add(Expression.Le(reportDateField, Convert.ToDateTime(yearList.Split(',')[1])));
                    result.Add("StartDate", yearList.Split(',')[0]);
                    result.Add("EndDate", yearList.Split(',')[1]);
                }
            }


            return result;
        }
    }
}
