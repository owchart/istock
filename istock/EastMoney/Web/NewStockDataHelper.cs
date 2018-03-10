using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmCore;
using EastMoney.FM.Web.Data;
using EastMoney.FM.Web.Models.Enum;

namespace dataquery.Web
{
    /// <summary>
    /// 新股数据类
    /// </summary>
    public class NewStockDataHelper
    {
        /// <summary>
        /// 获取新股速览上部需要显示的新股名称
        /// </summary>
        /// <returns></returns>
        public static DataSet GetNewStockName()
        {
            try
            {
                //DateTime date = DateTime.Now;
                //int num = CONSTANT.CaculateWeekDayInt(date);
                //DateTime date1 = date.AddDays(1 - num);
                //DateTime date2 = date.AddDays(7 - num);
                List<Order> order = new List<Order>();
                order.Add(new Order("DAT_ZGR", false));
                List<Expression> filter = new List<Expression>();
                //Expression exp1 = Expression.And(Expression.Ge("DAT_SSR", date.AddDays(1 - num)), Expression.Le("DAT_SSR", date.AddDays(7 - num)));
                //Expression exp2 = Expression.And(Expression.Ge("DAT_FXR", date.AddDays(1 - num)), Expression.Le("DAT_FXR", date.AddDays(7 - num)));
                //filter.Add(Expression.Or(exp1, exp2));
                filter.Add(Expression.And(Expression.IsNotEmpty("SECURITYCODE"), Expression.IsNotNull("SECURITYCODE")));
                filter.Add(Expression.And(Expression.IsNotEmpty("DAT_ZGR"), Expression.IsNotNull("DAT_ZGR")));
                string[] _fileds = new string[] { "SECURITYSHORTNAME", "SECURITYCODE", "MSECUCODE" };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_NS_XGFX, _fileds, filter, order, new List<Expression>(), 1, 8);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("新股中心-新股速览股票：", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取新股日历近五日数据
        /// num=0取今天向后五日数据，过滤双休日
        /// num=-1取今天前五日数据，过滤双休日
        /// num=1取已今天向后五日（过滤双休日）为起点的后五日数据，过滤双休日
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static DataSet GetNewStockCalendar(int num)
        {
            try
            {
                string date = DateTime.Now.AddDays(num * 7).ToShortDateString();
                List<Expression> filter = new List<Expression>();
                DateTime begin = CommDao.DateToUTC(Convert.ToDateTime(date));
                DateTime end = CommDao.DateToUTC(Convert.ToDateTime(date).AddDays(5));

                Expression exp1 = new Expression();
                int week = CommDao.CaculateWeekDayInt(begin);
                if (week == 6 || week == 7 || week == 1)
                {
                    if (week != 1)
                        begin = begin.AddDays(7 - week + 1);
                    end = begin.AddDays(4);
                    exp1 = Expression.And(Expression.Ge("STARTDATE", begin), Expression.Le("STARTDATE", end));
                }
                else
                {
                    end = begin.AddDays(5 - week);
                    DateTime begin2 = end.AddDays(3);
                    DateTime end2 = begin2.AddDays(week - 2);
                    exp1 = Expression.Or(Expression.And(Expression.Ge("STARTDATE", begin), Expression.Le("STARTDATE", end)), Expression.And(Expression.Ge("STARTDATE", begin2), Expression.Le("STARTDATE", end2)));
                }
                DateTime sunday = new DateTime();
                if (week == 6 || week == 7 || week == 0)
                {
                    sunday = DateTime.Now.AddYears(999);
                }
                else
                {
                    sunday = Convert.ToDateTime(date).AddDays(7 - week);
                }
                Expression exp2 = Expression.And(Expression.IsNotNull("STARTDATE"), Expression.IsNotEmpty("STARTDATE"));
                Expression exp3 = Expression.And(Expression.IsNotEmpty("SECURITYCODE"), Expression.IsNotNull("SECURITYCODE"));
                filter.Add(Expression.And(Expression.And(exp1, exp2), exp3));
                List<Order> Order = new List<Order>();
                Order.Add(new Order("STARTDATE", true));
                string[] _fileds = new string[] { "SECURITYCODE", "MSECUCODE", "SECURITYSHORTNAME", "DATETYPE", "STARTDATE" };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_NS_CALENDAR, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("新股中心-新股日历：", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取新股发行与上市信息
        /// </summary>
        /// <param name="type">type = 0获取发行新股信息，type = 1获取新股上市信息</param>
        /// <returns></returns>
        public static DataSet GetStockIssueMsg(string type, string trademarketcode)
        {
            try
            {
                List<Order> Order = new List<Order>();
                List<Expression> filter = new List<Expression>();
                Expression exp1 = Expression.And(Expression.IsNotEmpty("DAT_FXR"), Expression.IsNotNull("DAT_FXR"));
                Expression exp2 = Expression.And(Expression.IsNotEmpty("DAT_SSR"), Expression.IsNotNull("DAT_SSR"));
                if (string.IsNullOrEmpty(trademarketcode)) { trademarketcode = "0"; }
                if (trademarketcode.Equals("0")) { }
                if (trademarketcode.Equals("1")) { filter.Add(Expression.Eq("TRADEMARKETCODE", "069001001001")); }
                if (trademarketcode.Equals("2")) { filter.Add(Expression.Eq("TRADEMARKETCODE", "069001002002")); }
                if (trademarketcode.Equals("3")) { filter.Add(Expression.Eq("TRADEMARKETCODE", "069001002003")); }

                if (type.Equals("0"))
                {
                    filter.Add(Expression.And(exp1, Expression.Or(Expression.IsEmpty("DAT_SSR"), Expression.IsNull("DAT_SSR"))));
                    string str = DateTime.Now.AddDays(-1).ToShortDateString() + " 23:59:59";
                    filter.Add(Expression.Ge("DAT_FXR", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                    //filter.Add(Expression.Ge("DAT_FXR", DateTime.Now));
                    Order.Add(new Order("DAT_FXR", true));
                }
                if (type.Equals("1"))
                {
                    filter.Add(Expression.And(exp1, exp2));
                    string str = DateTime.Now.AddDays(-1).ToShortDateString() + " 23:59:59";
                    filter.Add(Expression.Ge("DAT_SSR", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                    Order.Add(new Order("DAT_SSR", true));
                }
                string[] _fileds = new string[] { };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_NS_XGFX, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("新股中心-新股发行上市信息：", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取新股定价预测
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockPredictMsg()
        {
            try
            {
                List<Expression> filter1 = new List<Expression>();
                string[] _fileds1 = new string[] { "SECURITYCODE_HIDE", "SECURITYSHORTNAME", "PREDICTINST",
                    "ISSUEPRICE", "SHAREISSUED", "PREDICTPRICEL", "PREDICTPRICET", "YUCEZHAIYAO","MSECUCODE"};
                List<Order> Order1 = new List<Order>();
                Order1.Add(new Order("SECURITYCODE_HIDE", true));
                ResultData pData1 = DataAccess.QueryByStatistic(WebDataSource.SPE_NS_PR, _fileds1, filter1, Order1, new List<Expression>(), 1, int.MaxValue, WebDataSource.SPE_NS_PR.ToString());
                return pData1.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("新股中心-新股定价预测：", ex);
                return null;
            }
        }

        /// <summary>
        /// 首页 新股改革
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockIPOPage()
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("FLAG1", true));
                string[] _field = new string[] { };
                _DS = DataAccess.Query(WebDataSource.SPE_XGGG_REFORMSYSTEM, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockIPOPage方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 股权分置改革后IPO数据统计
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockIPODataStatistics()
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("FLAG", true));
                string[] _field = new string[] { };
                ResultData _RS = DataAccess.QueryByStatistic(WebDataSource.SPE_XGGG_IPOSTATISTICS1, _field, _query, _sorts, _selector, 1, int.MaxValue, WebDataSource.SPE_XGGG_IPOSTATISTICS1.ToString());
                return _RS.Data;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockIPODataStatistics方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 历史IPO暂停重启图形统计
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockIPOHistoryStatistics()
        {
            List<object> list = new List<object>();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("FLAG", true));
                string[] _field = new string[] { };
                _DS = DataAccess.Query(WebDataSource.SPE_XXGG_RESTRAT, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockIPOHistoryStatistics方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 历次改革市场指数表现
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockIPOMarketExpress()
        {
            object result = new object();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("FLAG", true));
                string[] _field = new string[] { };
                _DS = DataAccess.Query(WebDataSource.SPE_XGGG_INDEX_TABLE, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockIPOMarketExpress方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 历次改革市场指数表现
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockIPOIncomeCalculatorParam()
        {
            object result = new object();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                string[] _field = new string[] { };
                ResultData _RS = DataAccess.QueryByStatistic(WebDataSource.SPE_XGGG_RETURNRATE4, _field, _query, _sorts, _selector, 1, int.MaxValue, WebDataSource.SPE_XGGG_RETURNRATE4.ToString());

                List<Expression> sy_query = new List<Expression>();
                List<Expression> sy_selector = new List<Expression>();
                List<Order> sy_sorts = new List<Order>();
                sy_sorts.Add(new Order("ID", true));
                string[] sy_field = new string[] { };
                _DS = DataAccess.Query(WebDataSource.SPE_XGGG_SSQ_SYL_DROPLIST, sy_field, sy_query, sy_sorts, sy_selector, 1, int.MaxValue);
                //近期新股申购一览
                List<Expression> new_query = new List<Expression>();
                List<Expression> new_selector = new List<Expression>();
                List<Order> new_sorts = new List<Order>();
                string[] new_field = new string[] { };
                new_sorts.Add(new Order("DAT_WSSGR", false));
                ResultData Stock_RS = DataAccess.QueryByStatistic(WebDataSource.SPE_XGGG_RETURNRATE5, new_field, new_query, new_sorts, new_selector, 1, int.MaxValue, WebDataSource.SPE_XGGG_RETURNRATE5.ToString());

                return Stock_RS.Data;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockIPOIncomeCalculatorParam方法异常", ex);
            }
            return null;
        }


        /// <summary>
        /// 至少中一签----获取中签率
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockIPOZQRate()
        {
            List<object> result = new List<object>();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                string[] _field = new string[] { };
                ResultData _RS = DataAccess.QueryByStatistic(WebDataSource.SPE_XGGG_RETURNRATE4, _field, _query, _sorts, _selector, 1, int.MaxValue, WebDataSource.SPE_XGGG_RETURNRATE4.ToString());
                return _RS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("至少中一签----获取中签率GetStockIPOZQRate方法异常", ex);
            }
            return null;
        }
    }
}
