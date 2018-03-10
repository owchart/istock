using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EastMoney.FM.Web.Data;
using EastMoney.FM.Web.Models.Enum;
using EmCore;

namespace dataquery
{
    /// <summary>
    /// 交易信息数据类
    /// </summary>
    public class TradeInfoDataHelper
    {
        public static DataSet GetBusinessDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                filter.Add(Expression.Eq("DATE_CHANGEDATE", Convert.ToDateTime(date)));
                List<Order> Order = new List<Order>();
                //Order s = new Core.Data.Order("STR_CODE", false);
                //Order.Add(s);
                string[] _fileds = new string[] { };

                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_WARNING, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBusinessDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取持股变动信息
        /// </summary>
        public static DataSet GetShareChangeDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                filter.Add(Expression.Eq("ENDDATE", Convert.ToDateTime(date)));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_CODE", false);
                Order.Add(s);
                string[] _fileds = new string[] { };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_SHARECHAGE, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetShareChangeDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取大宗交易信息
        /// </summary>
        public static DataSet GetBigBuyDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                filter.Add(Expression.Eq("TDATE",Convert.ToDateTime(date)));
                string[] _fileds = new string[] { };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_TRADING, _fileds, filter, new List<Order>(), new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBigBuyDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取业绩预测信息
        /// </summary>
        public static DataSet GetPredictPriceDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                filter.Add(Expression.Eq("NOTICEDATE", Convert.ToDateTime(date)));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_CODE", false);
                Order.Add(s);

                string[] _fileds = new string[] { };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_PREDICT, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPredictPriceDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取财报披露信息(筛选条件？？)
        /// </summary>
        public static DataSet GetFinancialShowDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                filter.Add(Expression.Eq("DATE_TDATE", Convert.ToDateTime(date)));
                List<Order> Order = new List<Order>();
                string[] _fileds = new string[] { "STR_NAME", "STR_CODE", "NOTICETITLE", "INFOCODE", "ATTACHTYPE", "DATE_TDATE", "REPORTTYPE", "REPORTYEAR" };

                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_CBPL, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetFinancialShowDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取股份流通
        /// </summary>
        public static DataSet GetLiftLimitDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                filter.Add(Expression.Eq("DATE_DATE1", Convert.ToDateTime(date)));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_CODE", false);
                Order.Add(s);
                string[] _fileds = new string[] { };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_LIFTING, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetLiftLimitDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取分红
        /// </summary>
        public static DataSet GetDealOutDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filter = new List<Expression>();
                Expression exp1 = Expression.IsNull("DATE_DATE1");
                Expression exp2 = Expression.Eq("DATE_DATE1", Convert.ToDateTime(date));
                Expression exp3 = Expression.Eq("DATE_DATE2", Convert.ToDateTime(date));
                filter.Add(Expression.Or(exp2, Expression.And(exp1, exp3)));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_CODE", false);
                Order.Add(s);
                string[] _fileds = new string[] { };
                DataSet pData = DataAccess.Query(WebDataSource.SPE_STOCKCALENDAR_ALLOT, _fileds, filter, Order, new List<Expression>(), 1, int.MaxValue);
                return pData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDealOutDate出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取每日提醒数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetHomeData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                string[] fileds = new string[] { };
                Expression exp1 = Expression.Ge("DAT_ISSUEEDATE", Convert.ToDateTime(date));
                Expression exp2 = Expression.Le("DAT_ISSUESDATE", Convert.ToDateTime(date));//addday
                filters.Add(Expression.And(exp2, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_BONDCODE", true);
                Order.Add(s);
                DataSet _DS = DataAccess.Query(WebDataSource.BOND_CALENDER_SPECIALREMIND, fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取首页每日提醒信息GetHomeData:", ex);
                return null;
            }
        }
        /// <summary>
        /// 获取新债发行数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetIssueData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                string[] fileds = new string[] { };
                Expression exp1 = Expression.Ge("DAT_ISSUEEDATE", Convert.ToDateTime(date));
                Expression exp2 = Expression.Le("DAT_ISSUESDATE", Convert.ToDateTime(date));
                filters.Add(Expression.And(exp2, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_BONDCODE", true);
                Order.Add(s);
                DataSet _DS = DataAccess.Query(WebDataSource.BOND_CALENDER_ISSUE, fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取新债发行信息GetIssueData:", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取付息兑付数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetPaymentData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                string[] fileds = new string[] { };
                Expression exp1 = Expression.Ge("DAT_ISSUEEDATE", Convert.ToDateTime(date));
                Expression exp2 = Expression.Le("DAT_ISSUESDATE", Convert.ToDateTime(date));//addday
                filters.Add(Expression.And(exp2, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_BONDCODE", true);
                Order.Add(s);
                DataSet _DS = DataAccess.Query(WebDataSource.BOND_CALENDER_PAYMENT, fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取付息兑付信息GetPaymentData:", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取交易结算数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetTransactionData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                string[] fileds = new string[] { };
                Expression exp1 = Expression.Ge("DAT_ISSUEEDATE", Convert.ToDateTime(date));
                Expression exp2 = Expression.Le("DAT_ISSUESDATE", Convert.ToDateTime(date));//addday
                filters.Add(Expression.And(exp2, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_BONDCODE", true);
                Order.Add(s);
                DataSet _DS = DataAccess.Query(WebDataSource.BOND_CALENDER_TRANSACTIONS, fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取交易结算信息GetTransactionData:", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取评级变更数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetRatingChangeData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                string[] fileds = new string[] { };
                Expression exp1 = Expression.Ge("DAT_ISSUEEDATE", Convert.ToDateTime(date));
                Expression exp2 = Expression.Le("DAT_ISSUESDATE", Convert.ToDateTime(date));//addday
                filters.Add(Expression.And(exp2, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_BONDCODE", true);
                Order.Add(s);
                DataSet _DS = DataAccess.Query(WebDataSource.BOND_CALENDER_RATIND_CHANGE, fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取评级变更信息GetRatingChangeData:", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取可转债数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetConvertibleBondsData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                string[] fileds = new string[] { };
                Expression exp1 = Expression.Ge("DAT_ISSUEEDATE", Convert.ToDateTime(date));
                Expression exp2 = Expression.Le("DAT_ISSUESDATE", Convert.ToDateTime(date));//addday
                filters.Add(Expression.And(exp2, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STR_BONDCODE", true);
                Order.Add(s);
                DataSet _DS = DataAccess.Query(WebDataSource.BOND_CALENDER_CONVERTIBLEBOND, fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取评级变更信息GetConvertibleBondsData:", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取基金日历首页开基和闭基的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataSet GetFundHomePageData(string date)
        {
            try
            {
                List<DataTransmission> _query = new List<DataTransmission>();
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();

                //基金上市（开放式和封闭式）
                List<Expression> filters = new List<Expression>();
                Expression exp = Expression.Eq("LISTDATE", Convert.ToDateTime(date));
                //Expression exp1 = Expression.Eq("STYPE", STYPE);
                filters.Add(exp);
                string[] _fileds = new string[] { "STYPE", "SECURITYCODE", "SECURITYSHORTNAME", "STR_TEXCH", "LISTDATE", "LISTSHARE" };
                _query.Add(DataAccess.GetData(WebDataSource.IND_FC_FUNDLIST, _fileds, filters, new List<Order>(), new List<Expression>(), 1, int.MaxValue, false, WebDataSource.IND_FC_FUNDLIST.ToString()));

                //基金发行
                filters = new List<Expression>();
                exp = Expression.Ge("ENDDATE", Convert.ToDateTime(date));//addday
                Expression exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(date));//addday
                if (Convert.ToDateTime(date) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    filters.Add(Expression.Or(Expression.And(exp, exp1), Expression.Ge("STARTDATE", Convert.ToDateTime(date))));
                else
                    filters.Add(Expression.And(exp, exp1));
                List<Order> Order = new List<Order>();
                Order s = new Order("STARTDATE", false);
                Order.Add(s);
                _fileds = new string[] { };
                _query.Add(DataAccess.GetData(WebDataSource.IND_FC_FUNDDIS, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue, false, WebDataSource.IND_FC_FUNDDIS.ToString()));
                _query.Add(DataAccess.GetData(WebDataSource.IND_FC_CFUNDDIV, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue, false, WebDataSource.IND_FC_CFUNDDIV.ToString()));

                //基金分红
                Order = new List<Order>();
                s = new Order("DAT_MINDATE", false);
                Order.Add(s);
                filters = new List<Expression>();
                filters.Add(Expression.Ge("DAT_MAXDATE", Convert.ToDateTime(date)));//addday
                filters.Add(Expression.Le("DAT_MINDATE", Convert.ToDateTime(date)));//addday
                _fileds = new string[] { "STYPE", "SECURITYCODE", "SECURITYSHORTNAME", "PERBBTAX", "REGISTERDATE", "EXDIVIDATEOTC", "DIVIDATEOTC" };
                _query.Add(DataAccess.GetData(WebDataSource.IND_DIVIDENDFUND, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue, false, WebDataSource.IND_DIVIDENDFUND.ToString()));

                //申购赎回
                filters = new List<Expression>();
                exp = Expression.Ge("ENDDATE", Convert.ToDateTime(date));//addday
                exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(date));//addday
                Expression exp2 = Expression.And(Expression.Or(Expression.IsNotNull("ENDDATE"), Expression.IsNotEmpty("ENDDATE")), Expression.And(exp, exp1));
                Expression exp3 = Expression.Eq("STARTDATE", Convert.ToDateTime(date));
                filters.Add(Expression.Or(exp2, exp3));
                Order = new List<Order>();
                s = new Order("STARTDATE", false);
                Order.Add(s);
                _fileds = new string[] { "STYPE", "SECURITYCODE", "SECURITYSHORTNAME", "STARTDATE", "ENDDATE", "REASONDES", "DATETYPE" };
                _query.Add(DataAccess.GetData(WebDataSource.IND_FC_TPAR, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue, false, WebDataSource.IND_FC_TPAR.ToString()));

                DataSet _DS = new DataSet();
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取首页每日提醒信息GetHomePageData:", ex);
                throw (ex);
            }
        }

        /// <summary>
        /// 获取每日提醒--基金上市------改
        /// </summary>
        /// <param name="date">日期 默认为当天</param>
        /// <param name="type">0：开放式 1：封闭式 默认为0</param>
        /// <returns></returns>  
        public static DataSet GetFundEnter(string date, string type)
        {
            if (string.IsNullOrEmpty(date))
                date = DateTime.Now.ToShortDateString();
            if (string.IsNullOrEmpty(type))
                type = "0";
            string STYPE = type == "0" ? "FDO" : "FDC";
            List<Expression> filters = new List<Expression>();
            Expression exp = Expression.Eq("LISTDATE", Convert.ToDateTime(date));
            Expression exp1 = Expression.Eq("STYPE", STYPE);
            filters.Add(Expression.And(exp, exp1));
            string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "STR_TEXCH", "LISTDATE", "LISTSHARE" };
            DataSet _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDLIST, _fileds, filters, new List<Order>(), new List<Expression>(), 1, int.MaxValue);
            return _DS;
        }

        /// <summary>
        /// 获取每日提醒--基金分红---------改
        /// </summary>
        /// <param name="date">日期 默认为当天</param>
        /// <param name="type">0：开放式 1：封闭式 默认为0</param>
        /// <returns></returns>
        public static DataSet GetFundShare(string date, string type)
        {
            if (string.IsNullOrEmpty(date))
                date = DateTime.Now.ToShortDateString();
            if (string.IsNullOrEmpty(type))
                type = "0";
            string STYPE = type == "0" ? "FDO" : "FDC";
            List<Order> Order = new List<Order>();
            Order s = new Order("DAT_MINDATE", false);
            Order.Add(s);
            List<Expression> filters = new List<Expression>();
            filters.Add(Expression.Eq("STYPE", STYPE));
            filters.Add(Expression.Ge("DAT_MAXDATE", Convert.ToDateTime(date)));//addday
            filters.Add(Expression.Le("DAT_MINDATE", Convert.ToDateTime(date)));//addday
            string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "PERBBTAX", "REGISTERDATE", "EXDIVIDATEOTC", "DIVIDATEOTC" };
            DataSet _DS = DataAccess.Query(WebDataSource.IND_DIVIDENDFUND, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
            return _DS;
        }

        /// <summary>
        /// 获取每日提醒--基金发行------改
        /// </summary>
        /// <param name="date">日期 默认为当天</param>
        /// <param name="type">0：开放式 1：封闭式 默认为0</param>
        /// <returns></returns>
        public static DataSet GetFundIssue(string date, string type)
        {
            if (string.IsNullOrEmpty(date))
                date = DateTime.Now.ToShortDateString();
            if (string.IsNullOrEmpty(type))
                type = "0";
            List<Expression> filters = new List<Expression>();
            Expression exp = Expression.Ge("ENDDATE", Convert.ToDateTime(date));//addday
            Expression exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(date));//addday
            if (Convert.ToDateTime(date) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                filters.Add(Expression.Or(Expression.And(exp, exp1), Expression.Ge("STARTDATE", Convert.ToDateTime(date))));
            else
                filters.Add(Expression.And(exp, exp1));
            List<Order> Order = new List<Order>();
            Order s = new Order("STARTDATE", false);
            Order.Add(s);
            string[] _fileds = new string[] { };
            DataSet _DS = new DataSet();
            if (type == "0")
                _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDDIS, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
            else if (type == "1")
                _DS = DataAccess.Query(WebDataSource.IND_FC_CFUNDDIV, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
            else { }
            return _DS;
        }

        /// <summary>
        /// 每日提醒，剩余信息(申购赎回)
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">0：开放式 1：封闭式 默认为0</param>
        /// <returns></returns>
        public static DataSet GetOtherNotice(string date, string type)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                List<Expression> filters = new List<Expression>();
                Expression exp = Expression.Ge("ENDDATE", Convert.ToDateTime(date));//addday
                Expression exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(date));//addday
                Expression exp2 = Expression.And(Expression.Or(Expression.IsNotNull("ENDDATE"), Expression.IsNotEmpty("ENDDATE")), Expression.And(exp, exp1));
                Expression exp3 = Expression.Eq("STARTDATE", Convert.ToDateTime(date));
                filters.Add(Expression.Or(exp2, exp3));

                if (string.IsNullOrEmpty(type))
                    type = "0";
                string STYPE = type == "0" ? "FDO" : "FDC";

                Expression exp4 = Expression.Eq("STYPE", STYPE);
                filters.Add(exp4);
                List<Order> Order = new List<Order>();
                Order s = new Order("STARTDATE", false);
                Order.Add(s);
                string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "STARTDATE", "ENDDATE", "REASONDES", "DATETYPE" };
                DataSet _DS = DataAccess.Query(WebDataSource.IND_FC_TPAR, _fileds, filters, Order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOtherNotice方法出错:", ex);
                return null;
            }
        }

        /// <summary>
        /// 分红拆分
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">0：开放式 1：封闭式 默认为0</param>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "CacheChangeByOneDay")]
        public static DataSet GetFundSplit(string date, string type, string show)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                if (string.IsNullOrEmpty(type))
                    type = "0";
                DataSet _DS = new DataSet();
                //基金拆分
                if (show == "1")
                {
                    List<Expression> filters = new List<Expression>();
                    int STYPE = type == "0" ? 102 : 101;
                    Expression exp1 = Expression.And(Expression.Eq("TYPECODE", STYPE), Expression.And(Expression.IsNotEmpty("ADDFORSPLIT"), Expression.And(Expression.IsNotNull("ADDFORSPLIT"), Expression.And(Expression.Eq("DECLAREDATE", Convert.ToDateTime(date + " 0:00:00")), Expression.Eq("STR_NB", "1")))));
                    Expression exp2 = Expression.And(Expression.Eq("TYPECODE", STYPE), Expression.And(Expression.IsNotEmpty("DEC_DIVIRATIO"), Expression.And(Expression.Eq("STR_NB", "0"), Expression.And(Expression.Ge("DECLAREDATE", Convert.ToDateTime(date)), Expression.Le("REPORTDATE", Convert.ToDateTime(date))))));
                    filters.Add(Expression.Or(exp1, exp2));
                    string[] _fileds = new string[] { };
                    DataSet ds = DataAccess.Query(WebDataSource.IND_FC_FUNDRESO, _fileds, filters, new List<Order>(), new List<Expression>(), 1, int.MaxValue);
                    return ds;
                }
                //基金分红
                else
                {
                    List<Expression> filters = new List<Expression>();
                    // Expression exp = Expression.Ge("DAT_MINDATE", Convert.ToDateTime(date));
                    string STYPE = type == "0" ? "FDO" : "FDC";
                    // Expression exp1 = Expression.Eq("STYPE", STYPE);
                    filters.Add(Expression.Ge("DAT_MAXDATE", Convert.ToDateTime(date)));//addday
                    filters.Add(Expression.Eq("STYPE", STYPE));
                    filters.Add(Expression.Le("DAT_MINDATE", Convert.ToDateTime(date)));//addday
                    string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "PERBBTAX", "REGISTERDATE", "EXDIVIDATEOTC", "DIVIDATEOTC" };
                    _DS = DataAccess.Query(WebDataSource.IND_DIVIDENDFUND, _fileds, filters, new List<Order>(), new List<Expression>(), 1, int.MaxValue);
                    return _DS;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetFundSplit.do发生错误:", ex);
                return null;
            }
        }

        /// <summary>
        /// 发行上市
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">0：开放式 1：封闭式 默认为0</param>
        /// <param name="show">show:0取发行，show:1取上市</param>
        /// <returns></returns>
        public static DataSet GetIssueSale(string date, string type, string show)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                if (string.IsNullOrEmpty(type))
                    type = "0";
                if (string.IsNullOrEmpty(show))
                    show = "0";
                DataSet _DS = new DataSet();
                //发行
                if (show == "0")
                {
                    List<Order> order = new List<Order>();
                    order.Add(new Order("STARTDATE", false));
                    List<Expression> filters = new List<Expression>();
                    Expression exp = Expression.Ge("ENDDATE", Convert.ToDateTime(date));//addday
                    Expression exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(date));//addday
                    if (Convert.ToDateTime(date) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                        filters.Add(Expression.Or(Expression.And(exp, exp1), Expression.Ge("STARTDATE", Convert.ToDateTime(date))));
                    else
                        filters.Add(Expression.And(exp, exp1));
                    string[] _fileds = new string[] { };
                    if (type == "0")
                        _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDDIS, _fileds, filters, order, new List<Expression>(), 1, int.MaxValue);
                    else if (type == "1")
                        _DS = DataAccess.Query(WebDataSource.IND_FC_CFUNDDIV, _fileds, filters, order, new List<Expression>(), 1, int.MaxValue);
                    else { }
                    return _DS;
                }
                //上市
                else
                {
                    List<Expression> filters = new List<Expression>();
                    Expression exp = Expression.Eq("LISTDATE", Convert.ToDateTime(date));
                    string STYPE = type == "0" ? "FDO" : "FDC";
                    Expression exp1 = Expression.Eq("STYPE", STYPE);
                    filters.Add(Expression.And(exp, exp1));
                    string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "STR_TEXCH", "LISTDATE", "LISTSHARE" };
                    _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDLIST, _fileds, filters, new List<Order>(), new List<Expression>(), 1, int.MaxValue);
                    return _DS;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("基金日历-发行上市：", ex);
                return null;
            }
        }

        /// <summary>
        /// 基金经理
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DataSet GetFundManager(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                DateTime beginDate = Convert.ToDateTime(date).AddDays(-2);
                DateTime endDate = Convert.ToDateTime(date).AddDays(+5);
                List<Expression> filters = new List<Expression>();
                Expression exp = Expression.Ge("CHANGEDATE", Convert.ToDateTime(beginDate));//addday
                Expression exp1 = Expression.Le("CHANGEDATE", Convert.ToDateTime(endDate));//addday
                filters.Add(Expression.And(exp, exp1));
                List<Order> order = new List<Order>();
                order.Add(new Order("CHANGEDATE", false));
                string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "CHANGEDATE", "NAME", "LEAVEREASON", "POST", "DATETYPE" };
                DataSet _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDMANAGER, _fileds, filters, order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("基金日历-基金经理：", ex);
                return null;
            }
        }
        /// <summary>
        /// 基金转型
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DataSet GetFundChange(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                DateTime beginDate = Convert.ToDateTime(date).AddDays(-2);
                DateTime endDate = Convert.ToDateTime(date).AddDays(+5);
                List<Expression> filters = new List<Expression>();
                Expression exp = Expression.Ge("STARTDATE", Convert.ToDateTime(beginDate));//addday
                Expression exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(endDate));//addday
                filters.Add(Expression.And(exp, exp1));
                string[] _fileds = new string[] { "SNAME", "SECUCODE", "STR_ZXLX", "CORRESCODE", "CORRESNAME", "STARTDATE" };
                List<Order> order = new List<Order>();
                order.Add(new Order("STARTDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDTRANSFORM, _fileds, filters, order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("基金日历-基金转型：", ex);
                return null;
            }
        }


        /// <summary>
        /// 基金更名
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DataSet GetFundNameChange(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                DateTime beginDate = Convert.ToDateTime(date).AddDays(-2);
                DateTime endDate = Convert.ToDateTime(date).AddDays(+5);
                List<Expression> filters = new List<Expression>();
                Expression exp = Expression.Ge("STARTDATE", Convert.ToDateTime(beginDate));//addday
                Expression exp1 = Expression.Le("STARTDATE", Convert.ToDateTime(endDate));//addday
                filters.Add(Expression.And(exp, exp1));
                string[] _fileds = new string[] { "SNAME", "SECUCODE", "STR_ZXLX", "CORRESCODE", "CORRESNAME", "STARTDATE" };
                List<Order> order = new List<Order>();
                order.Add(new Order("STARTDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.IND_FC_FUNDTRANSFORM, _fileds, filters, order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("基金日历-基金更名：", ex);
                return null;
            }
        }


        /// <summary>
        /// 财报披露
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DataSet GetFinanceShow(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    date = DateTime.Now.ToShortDateString();
                DateTime beginDate = Convert.ToDateTime(date).AddDays(-10);
                DateTime endDate = Convert.ToDateTime(date).AddDays(+10);
                List<Expression> filters = new List<Expression>();
                Expression exp = Expression.Ge("ENDDATE", Convert.ToDateTime(beginDate));//addday
                Expression exp1 = Expression.Le("ENDDATE", Convert.ToDateTime(endDate));//addday
                filters.Add(Expression.And(exp, exp1));
                string[] _fileds = new string[] { "SECURITYCODE", "SECURITYSHORTNAME", "STR_REPORTTYPE", "ENDDATE" };
                List<Order> order = new List<Order>();
                order.Add(new Order("ENDDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.IND_FC_FINADISCLOSE, _fileds, filters, order, new List<Expression>(), 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("基金日历-财报披露：", ex);
                return null;
            }
        }
    }
}
