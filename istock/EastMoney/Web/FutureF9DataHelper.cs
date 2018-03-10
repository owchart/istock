using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmCore;
using EastMoney.FM.Web.Data;
using EastMoney.FM.Web.Models.Enum;
using System.Web;
using EastMoney.FM.Web.Data.Model;

namespace dataquery.Web
{
    /// <summary>
    /// 期货深度资料类
    /// </summary>
    public class FutureF9DataHelper
    {
        public static String GetLastDay()
        {

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                return DateTime.Now.AddDays(-3).ToShortDateString();
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                return DateTime.Now.AddDays(-2).ToShortDateString();
            else
            {
                if (DateTime.Now.Hour >= 17)
                    return DateTime.Now.ToShortDateString();
                else
                    return DateTime.Now.AddDays(-1).ToShortDateString();
            }
        }
        /// <summary>
        /// 获取基础期货信息
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetFuturesBaseInfo(string securityCode)
        {
            object result = new object();
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_FUTURECODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                DataSet dataSet = DataAccess.Query(WebDataSource.FUTURE_HYJJ, _field, _query, _sort, _selector, 1, int.MaxValue);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetFuturesBaseInfo方法异常，异常信息：", ex);
            }
            return null;
        }
        /// <summary>
        /// 获取基础期货信息
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetFuturesBaseInfoByTransCode(string transCode)
        {
            object result = new object();
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_TRANSCODE", transCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                ResultData resultData = DataAccess.QueryByStatistic(WebDataSource.FUTURE_HYJJ1, _field, _query, _sort, _selector, 1, int.MaxValue,
                    WebDataSource.FUTURE_HYJJ1.ToString());
                return resultData.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetFuturesBaseInfoByTransCode方法异常，异常信息：", ex);
            }
            return null;
        }
        /// <summary>
        /// 合约简介
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetContractBrief(string securityCode)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_FUTURECODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                //_sort.Add(new Order("DAT_LDELVDATE", true));

                string[] _field = new string[] { };
                ResultData resultData = DataAccess.QueryByStatistic(WebDataSource.FUTURE_HYJJ2, _field, _query, _sort, _selector, 1, int.MaxValue,
                    WebDataSource.FUTURE_HYJJ2.ToString());
                return resultData.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetContractBrief方法异常，异常信息：", ex);
            }
            return null;
        }
        /// <summary>
        /// 价差矩阵
        /// </summary>
        /// <param name="transCode"></param>
        /// <returns></returns>
        public static DataSet GetDiffPriceMatrix(string transCode, string date)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_TRANSCODE", transCode));
                _query.Add(Expression.Eq("DAT_TRANSDATE", Convert.ToDateTime(date)));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("STR_FUTURECODE", true));

                string[] _field = new string[] { };
                DataSet dataSet = DataAccess.Query(WebDataSource.FUTURE_JCJZ, _field, _query, _sort, _selector, 1, int.MaxValue);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDiffPriceMatrix方法异常，异常信息：", ex);
            }
            return null;
        }
        /// <summary>
        /// 套利分析
        /// </summary>
        /// <param name="securityCodeA"></param>
        /// <param name="securityCodeB"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataSet GetArbitrageAnalysis(string securityCodeA, string securityCodeB, string startDate, string endDate)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_FUTURECODEA", securityCodeA));
                _query.Add(Expression.Eq("STR_FUTURECODEB", securityCodeB));
                _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(startDate)));
                _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(endDate)));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("DAT_TRANSDATE", false));
                string[] _field = new string[] { };
                DataSet dataSet = DataAccess.Query(WebDataSource.FUTURE_TLFX, _field, _query, _sort, _selector, 1, int.MaxValue);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetArbitrageAnalysis方法异常,securityCodeA:" + securityCodeA +
                    ",securityCodeB:" + securityCodeB + ",startDate:" + startDate + ",endDate:" + endDate
                    + "，异常信息：" + ex);
            }
            return null;
        }
        /// <summary>
        /// 获取相关的套利合约
        /// </summary>
        /// <param name="transCode"></param>
        /// <param name="deliveryDate"></param>
        /// <returns></returns>
        public static DataSet GetRelatedContract(string transCode, string deliveryDate)
        {
            List<String> result = new List<string>();
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_TRANSCODE", transCode));
                _query.Add(Expression.Gt("DAT_LDELVDATE", Convert.ToDateTime(deliveryDate)));
                _query.Add(Expression.Eq("STR_TRNASTATUS", "0"));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("DAT_LDELVDATE", true));
                string[] _field = new string[] { };
                DataSet dataSet = DataAccess.Query(WebDataSource.FUTURE_HYJJ, _field, _query, _sort, _selector, 1, int.MaxValue);
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    result.Add(CommDao.SafeToString(row["STR_FUTURECODE"]));
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetRelatedContract方法异常，异常信息：", ex);
            }
            return null;
        }
        /// <summary>
        /// 库存报告
        /// </summary>
        /// <param name="securityCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataSet GetInventoryReport(string securityCode, string startDate, string endDate)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_FUTURECODE", securityCode));
                _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(startDate)));
                _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(endDate)));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("DAT_TRANSDATE", false));
                string[] _field = new string[] { };
                DataSet dataSet = DataAccess.Query(WebDataSource.FUTURE_KCBG, _field, _query, _sort, _selector, 1, int.MaxValue);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetInventoryReport方法异常，异常信息：", ex);
            }
            return null;
        }
        /// <summary>
        /// 会员机构
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllMemberAgencies()
        {
            List<object> result = new List<object>();
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("STR_MEMSNAME", true));
                string[] _field = new string[] { };
                DataSet dataSet = DataAccess.Query(WebDataSource.FUTURE_HYJG, _field, _query, _sort, _selector, 1, int.MaxValue);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllMemberAgencies方法异常，异常信息：", ex);
            }
            return null;
        }
        public static DataSet GetPositionStructJson(string futureCode)
        {
            List<Expression> _query = new List<Expression>();
            List<Expression> _selector = new List<Expression>();
            List<Order> _sort = new List<Order>();
            _sort.Add(new Order("STR_MEMSNAME", true));
            string[] _field = new string[] { "STR_MEMSNAME", "STR_MEMENAME" };
            DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_HYJG, _field, _query, _sort, _selector, 1, 1);
            string party = _DS.Tables[0].Rows[0]["STR_MEMSNAME"].ToString();
            return _DS;

        }
        /// <summary>
        /// 持仓结构
        /// </summary>
        public static DataSet GetPositionStructModel(string futureCode, string date, string party)
        {
            try
            {

                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                if (!string.IsNullOrEmpty(futureCode))
                    _query.Add(Expression.Eq("STR_TRANSCODE", futureCode));//-----
                if (!string.IsNullOrEmpty(date))
                {
                    _query.Add(Expression.Eq("DAT_TRANSDATE", Convert.ToDateTime(date)));
                }
                else
                {
                    _query.Add(Expression.Eq("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(party))
                {
                    _query.Add(Expression.Eq("STR_MEMSNAME", party));
                }
                _sort.Add(new Order("STR_FUTURECODE", true));
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_CCJG, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPositionStructModel方法异常，异常信息：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public static DataSet GetOpenPositionJson(string futureCode)
        {
            List<Expression> _query = new List<Expression>();
            List<Expression> _selector = new List<Expression>();
            List<Order> _sort = new List<Order>();
            _sort.Add(new Order("STR_MEMSNAME", true));
            string[] _field = new string[] { "STR_MEMSNAME", "STR_MEMENAME" };
            DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_HYJG, _field, _query, _sort, _selector, 1, 1);
            string party = _DS.Tables[0].Rows[0]["STR_MEMSNAME"].ToString();
            return _DS;

        }
        /// <summary>
        /// 建仓过程
        /// </summary
        public static DataSet GetOpenPositionModel(string futureCode, string datebegin, string dateEnd, string party)
        {
            try
            {

                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                if (!string.IsNullOrEmpty(futureCode))
                    _query.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                if (!string.IsNullOrEmpty(datebegin))
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(datebegin)));
                }
                else
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.AddMonths(-6).ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(dateEnd)));
                }
                else
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(party))
                {
                    _query.Add(Expression.Eq("STR_MEMSNAME", party));
                }
                _sort.Add(new Order("DAT_TRANSDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_JCGC, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenPositionModel方法异常，异常信息：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 会员列表
        /// </summary>
        public static DataSet GetPartyModel()
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("STR_MEMSNAME", true));
                string[] _field = new string[] { "STR_MEMSNAME", "STR_MEMENAME" };
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_HYJG, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPartyModel方法异常，异常信息：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public static DataSet GetDealPositionJson(string STR_FUTURECODE)
        {
            string date = GetLastDay();
            return GetDealPositionModel(date, STR_FUTURECODE, "");
        }
        /// <summary>
        /// 成交持仓
        /// </summary>
        public static DataSet GetDealPositionModel(string datetime, string futureCode, string num)
        {
            try
            {
                List<DataTransmission> dtList = new List<DataTransmission>();
                DateTime date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                if (!string.IsNullOrEmpty(datetime))
                {
                    date = Convert.ToDateTime(datetime);
                }

                //成交持仓比例图
                string[] fileds_1 = new string[] { "DAT_TRANSDATE", "STR_FUTURECODE", "STR_MEMSNAME", "DEC_CJLZB", "DEC_QTCJLZB" };
                List<Expression> filters_1 = new List<Expression>();
                filters_1.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_1.Add(Expression.IsNotNull("DEC_CJLZB"));
                filters_1.Add(Expression.IsNotEmpty("DEC_CJLZB"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_1.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                List<Order> orders_1 = new List<Order>();
                orders_1.Add(new Order("DEC_CJLZB", false));
                DataTransmission dt1 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_1", fileds_1, filters_1, orders_1, new List<Expression>(), 1, 7, false, "");
                dtList.Add(dt1);

                //多单持仓比例图
                string[] fileds_2 = new string[] { "DAT_TRANSDATE", "STR_FUTURECODE", "STR_MEMSNAME", "DEC_DTLZB", "DEC_QTDTLZB" };
                List<Expression> filters_2 = new List<Expression>();
                filters_2.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_2.Add(Expression.IsNotNull("DEC_DTLZB"));
                filters_2.Add(Expression.IsNotEmpty("DEC_DTLZB"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_2.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                List<Order> orders_2 = new List<Order>();
                orders_2.Add(new Order("DEC_DTLZB", false));
                DataTransmission dt2 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_2", fileds_2, filters_2, orders_2, new List<Expression>(), 1, 7, false, "");
                dtList.Add(dt2);

                //空单持仓比例图
                string[] fileds_3 = new string[] { "DAT_TRANSDATE", "STR_FUTURECODE", "STR_MEMSNAME", "DEC_KTLZB", "DEC_QTKTLZB" };
                List<Expression> filters_3 = new List<Expression>();
                filters_3.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_3.Add(Expression.IsNotNull("DEC_KTLZB"));
                filters_3.Add(Expression.IsNotEmpty("DEC_KTLZB"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_3.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                List<Order> orders_3 = new List<Order>();
                orders_3.Add(new Order("DEC_KTLZB", false));
                DataTransmission dt3 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_3", fileds_3, filters_3, orders_3, new List<Expression>(), 1, 7, false, "");
                dtList.Add(dt3);

                //九大表格----1
                string[] fileds_4 = new string[] { "STR_MEMSNAME", "DEC_VLOUME", "DEC_VCHANGE", "DEC_CJLHJ", "DEC_CJLZJHJ", "DEC_QCJLHJ", "DEC_CJLHJZJ" };
                List<Expression> filters_4 = new List<Expression>();
                filters_4.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_4.Add(Expression.IsNotNull("DEC_VLOUME"));
                filters_4.Add(Expression.IsNotEmpty("DEC_VLOUME"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_4.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                int pageSize = 20;
                if (!string.IsNullOrEmpty(num))
                {
                    int.TryParse(num, out pageSize);
                }
                if (pageSize == -1)
                    pageSize = int.MaxValue;
                List<Order> orders_4 = new List<Order>();
                orders_4.Add(new Order("DEC_VLOUME", false));
                DataTransmission dt4 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_six_1", fileds_4, filters_4, orders_4, new List<Expression>(), 1, pageSize, false, "");
                dtList.Add(dt4);

                //九大表格----2
                string[] fileds_5 = new string[] { "STR_MEMSNAME", "DEC_LONGNUM", "DEC_LCHANGE", "DEC_DTLHJ", "DEC_QDTLHJ", "DEC_DTLHJZJ",
                    "DEC_DTLZJHJ","DEC_ZCDTCDDHJ","DEC_ZCDTZDDHJ","DEC_JCDTCDDHJ","DEC_JCDTZDDHJ" };
                List<Expression> filters_5 = new List<Expression>();
                filters_5.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_5.Add(Expression.IsNotNull("DEC_LONGNUM"));
                filters_5.Add(Expression.IsNotEmpty("DEC_LONGNUM"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_5.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                int pageSize5 = 20;
                if (!string.IsNullOrEmpty(num))
                {
                    int.TryParse(num, out pageSize5);
                }
                if (pageSize5 == -1)
                    pageSize5 = int.MaxValue;
                List<Order> orders_5 = new List<Order>();
                orders_5.Add(new Order("DEC_LONGNUM", false));
                DataTransmission dt5 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_six_2", fileds_5, filters_5, orders_5, new List<Expression>(), 1, pageSize5, false, "");
                dtList.Add(dt5);

                //九大表格----3
                string[] fileds_6 = new string[] { "STR_MEMSNAME", "DEC_SHORTNUM", "DEC_SCHANGE", "DEC_KTLHJ", "DEC_QKTLHJ", "DEC_KTLHJZJ",
                    "DEC_KTLZJHJ","DEC_ZCKTCKDHJ","DEC_ZCKTZKDHJ","DEC_JCKTCKDHJ","DEC_JCKTZKDHJ" };
                List<Expression> filters_6 = new List<Expression>();
                filters_6.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_6.Add(Expression.IsNotNull("DEC_SHORTNUM"));
                filters_6.Add(Expression.IsNotEmpty("DEC_SHORTNUM"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_6.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                int pageSize6 = 20;
                if (!string.IsNullOrEmpty(num))
                {
                    int.TryParse(num, out pageSize6);
                }
                if (pageSize6 == -1)
                    pageSize6 = int.MaxValue;
                List<Order> orders_6 = new List<Order>();
                orders_6.Add(new Order("DEC_SHORTNUM", false));
                DataTransmission dt6 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_six_3", fileds_6, filters_6, orders_6, new List<Expression>(), 1, pageSize6, false, "");
                dtList.Add(dt6);

                //九大表格----4
                string[] fileds_7 = new string[] { "STR_MEMSNAME", "DEC_NETLONG", "DEC_JDDZJ", "DEC_JDDHJ", "DEC_JDDZJHJ" };
                List<Expression> filters_7 = new List<Expression>();
                filters_7.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_7.Add(Expression.IsNotNull("DEC_NETLONG"));
                filters_7.Add(Expression.IsNotEmpty("DEC_NETLONG"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_7.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                int pageSize7 = 20;
                if (!string.IsNullOrEmpty(num))
                {
                    int.TryParse(num, out pageSize7);
                }
                if (pageSize7 == -1)
                    pageSize7 = int.MaxValue;
                List<Order> orders_7 = new List<Order>();
                orders_7.Add(new Order("DEC_NETLONG", false));
                DataTransmission dt7 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_six_4", fileds_7, filters_7, orders_7, new List<Expression>(), 1, pageSize7, false, "");
                dtList.Add(dt7);

                //九大表格----5
                string[] fileds_8 = new string[] { "STR_MEMSNAME", "DEC_NETSHORT", "DEC_JKDZJ", "DEC_JKDHJ", "DEC_JKDZJHJ" };
                List<Expression> filters_8 = new List<Expression>();
                filters_8.Add(Expression.Eq("DAT_TRANSDATE", date));
                filters_8.Add(Expression.IsNotNull("DEC_NETSHORT"));
                filters_8.Add(Expression.IsNotEmpty("DEC_NETSHORT"));
                if (!string.IsNullOrEmpty(futureCode))
                    filters_8.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                int pageSize8 = 20;
                if (!string.IsNullOrEmpty(num))
                {
                    int.TryParse(num, out pageSize8);
                }
                if (pageSize8 == -1)
                    pageSize8 = int.MaxValue;
                List<Order> orders_8 = new List<Order>();
                orders_8.Add(new Order("DEC_NETSHORT", false));
                DataTransmission dt8 = DataAccess.GetData(WebDataSource.FUTURE_CJCC.ToString(), "tabel_six_5", fileds_8, filters_8, orders_8, new List<Expression>(), 1, pageSize8, false, "");
                dtList.Add(dt8);

                DataSet _DS = DataAccess.Query(dtList);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDealPositionModel方法异常，异常信息：", ex);
                throw ex;
                //return Json("",JsonRequestBehavior.AllowGet);
            }
        }
        public static DataSet GetPositionPriceJson(string futureCode)
        {
            List<Expression> _query = new List<Expression>();
            List<Expression> _selector = new List<Expression>();
            List<Order> _sort = new List<Order>();
            _sort.Add(new Order("STR_MEMSNAME", true));
            DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_HYJG, null, _query, _sort, _selector, 1, 1);
            string party = _DS.Tables[0].Rows[0]["STR_MEMSNAME"].ToString();
            return _DS;

        }
        /// <summary>
        /// 持仓均价
        /// </summary>
        public static DataSet GetPositionPriceModel(string futureCode, string datebegin, string dateEnd, string party)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { "DAT_TRANSDATE", "DEC_CLEAR", "DEC_DTCCJJ", "DEC_KTCCJJ", "DEC_LONGNUM", "DEC_SHORTNUM", "DEC_NETLONG", "DEC_NETSHORT" };
                if (!string.IsNullOrEmpty(futureCode))
                    _query.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                if (!string.IsNullOrEmpty(datebegin))
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(datebegin)));
                }
                else
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.AddMonths(-6).ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(dateEnd)));
                }
                else
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(party))
                {
                    _query.Add(Expression.Eq("STR_MEMSNAME", party));
                }
                _sort.Add(new Order("DAT_TRANSDATE", true));
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_JCGC, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPositionPriceModel方法异常，异常信息：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public static DataSet GetProfitAnalysisJson(string futureCode)
        {
            List<Expression> _query = new List<Expression>();
            List<Expression> _selector = new List<Expression>();
            List<Order> _sort = new List<Order>();
            _sort.Add(new Order("STR_MEMSNAME", true));
            DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_HYJG, null, _query, _sort, _selector, 1, 1);
            string party = _DS.Tables[0].Rows[0]["STR_MEMSNAME"].ToString();
            return _DS;

        }
        /// <summary>
        /// 盈亏分析
        /// </summary>
        public static DataSet GetProfitAnalysisModel(string futureCode, string datebegin, string dateEnd, string party)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                if (!string.IsNullOrEmpty(futureCode))
                    _query.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                if (!string.IsNullOrEmpty(datebegin))
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(datebegin)));
                }
                else
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.AddMonths(-6).ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(dateEnd)));
                }
                else
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(party))
                {
                    _query.Add(Expression.Eq("STR_MEMSNAME", party));
                }
                _sort.Add(new Order("DAT_TRANSDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_YKFX, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetProfitAnalysisModel方法异常，异常信息：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 会员简介
        /// </summary>
        public static DataSet GetPartyInfoModel(string party)
        {
            try
            {

                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                if (!string.IsNullOrEmpty(party))
                    _query.Add(Expression.Eq("STR_MEMSNAME", HttpUtility.UrlDecode(party)));
                _sort.Add(new Order("DAT_TRANSDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_HYJG, _field, _query, _sort, _selector, 1, 1);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPartyInfoModel方法异常，异常信息：", ex);
                return null; 
            }
        }
        /// <summary>
        /// 净仓位
        /// </summary>
        public static DataSet GetNetExposureModel(string futureCode, string datebegin, string dateEnd)
        {
            try
            {
                List<Expression> _query = new List<Expression>();
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                string[] _field = new string[] { };
                if (!string.IsNullOrEmpty(futureCode))
                    _query.Add(Expression.Eq("STR_FUTURECODE", futureCode));
                if (!string.IsNullOrEmpty(datebegin))
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(datebegin)));
                }
                else
                {
                    _query.Add(Expression.Ge("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.AddMonths(-6).ToShortDateString())));
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(dateEnd)));
                }
                else
                {
                    _query.Add(Expression.Le("DAT_TRANSDATE", Convert.ToDateTime(DateTime.Now.ToShortDateString())));
                }
                _sort.Add(new Order("DAT_TRANSDATE", false));
                DataSet _DS = DataAccess.Query(WebDataSource.FUTURE_JCC, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetNetExposureModel方法异常，异常信息：", ex);
                return null;
            }
        }

        /// <summary>
        /// Shibor
        /// </summary>
        /// <returns></returns>
        public static DataSet GetRateShibor()
        {
            try
            {
                MacroIndicateParam param = new MacroIndicateParam();
                param.Name = "RPT01374";
                List<string> strList = DataAccess.QueryMacroParentReportDate(param);
                param.Name += "_" + strList[0];
                DataSet dataSet = DataAccess.QueryMacroByParentReportDate(param);
                string result = "<thead><tr><th>期限</th><th>" + strList[0] + "</th><th>" + strList[1] + "</th><th>涨跌(BP)</th><th>涨跌幅(%)</th><tr></thead>";
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetRateShibor方法异常，异常信息：", ex);
                return null;
            }
        }

        /// <summary>
        /// 银行间质押式回购加权利率
        /// </summary>
        /// <returns></returns>
        public static DataSet GetRateBank()
        {
            try
            {
                MacroIndicateParam param = new MacroIndicateParam();
                param.Name = "RPT01375";
                List<string> strList = DataAccess.QueryMacroParentReportDate(param);
                param.Name += "_" + strList[0];
                DataSet dataSet = DataAccess.QueryMacroByParentReportDate(param);
                string result = "<thead><tr><th>回购代码</th><th>" + strList[0] + "</th><th>" + strList[1] + "</th><th>涨跌(BP)</th><th>涨跌幅(%)</th><tr></thead>";
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetRateBank方法异常，异常信息：", ex);
                return null;
            }
        }

        /// <summary>
        /// 国债首页 价差分析 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexFuturesPrice(string code)
        {
            string table = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_DQ_CLOSE";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", "1900-01-01");
                sp.Dic.Add("EndDate", DateTime.Now.ToShortDateString());
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexFuturesPrice方法异常", ex);
            }
            return null; 
        }

        /// <summary>
        /// 国债首页 价差分析 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexFuturesDiffer(string code)
        {
            string table = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_CLOSESPREAD";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                //sp.Dic.Add("StartDate", DateTime.Now.AddMonths(-3).ToShortDateString());
                sp.Dic.Add("StartDate", "1900-01-01");
                sp.Dic.Add("EndDate", DateTime.Now.ToShortDateString());
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexFuturesDiffer方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 理论价差 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexTheoryDiffer(string type, string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_THEOSPREAD";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", "1900-01-01");
                //sp.Dic.Add("StartDate", DateTime.Now.AddDays(-10).ToShortDateString());//性能问题暂时展示10天
                sp.Dic.Add("EndDate", DateTime.Now.ToShortDateString());
                sp.Dic.Add("BondType", type);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexTheoryDiffer方法异常", ex);
            }
            return null;
        }
        /// <summary>
        /// 理论价差历史数据
        /// </summary>
        public static DataSet GetAllTheoryDiffer(string type, string StartDate, string EndDate, string code)
        {
            string tbody = "";
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_THEOSPREAD";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                sp.Dic.Add("BondType", type);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllTheoryDiffer方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 可交割券总成交量 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexDeliveryCount(string code)
        {
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_BONDVOL";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", "1900-01-01");
                sp.Dic.Add("EndDate", DateTime.Now.ToShortDateString());
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexDeliveryCount方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 国债期货成交持仓图 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexDealPostion(string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_CCMX";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", "1900-01-01");
                sp.Dic.Add("EndDate", DateTime.Now.ToShortDateString());
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexDealPostion方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 可交割券成交明细 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexDealDetail(string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_ZXCJMX";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;


            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexDealDetail方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 历史理论CTD 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHistoryCTD(string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "100000000015151";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("CloseDate", "N");
                //sp.Dic.Add("top", "5");
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString("$-fun"));
                return ds;


            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetHistoryCTD方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 活跃CTD 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetActiveCTD(string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_HYCTD";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetActiveCTD方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 CTD券基差走势
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexCTDTrend(string code)
        {
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "100000000015148";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                //sp.Dic.Add("StartDate", "1900-01-01");
                //sp.Dic.Add("EndDate", DateTime.Now.ToShortDateString());
                sp.Dic.Add("TradeDate", "N");
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString("$-fun"));
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexCTDTrend方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 货币投放回笼历史数据
        /// </summary>
        /// <returns></returns>
        public static DataSet GetMoneyBackDetail(string StartDate, string EndDate, string code)
        {
            string tbody = "";
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "100000000015155";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString("$-fun"));
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetMoneyBackDetail方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 合约价格历史数据 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllFuturesPrice(string StartDate, string EndDate, string code)
        {
            string table = "";
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_DQ_CLOSE";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllFuturesPrice方法异常", ex);
            }
            return null;
        }
        /// <summary>
        /// 国债首页 价差分析 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllFuturesDiffer(string StartDate, string EndDate, string code)
        {
            string table = "";
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_CLOSESPREAD";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllFuturesDiffer方法异常", ex);
            }
            return null;
        }
        /// <summary>
        /// 国债首页 可交割券成交明细  全部 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllDeliveryDetail(string StartDate, string EndDate, string code)
        {
            string tbody = "";
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_CJMX";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllDeliveryDetail方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 国债期货成交持仓图 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllDealPostion(string StartDate, string EndDate, string code)
        {
            string tbody = "";
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_CCMX";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllDealPostion方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 银行间固定利率国债到期收益率（中债）曲线日行情
        /// </summary>
        public static DataSet GetMarketRateDate()
        {
            try
            {
                MacroIndicateParam param = new MacroIndicateParam();
                param.Name = "BRPT01817";
                List<string> strList = DataAccess.QueryMacroParentReportDate(param);
                param.Name += "_" + strList[0];
                DataSet dataSet = DataAccess.QueryMacroByParentReportDate(param);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetMarketRateDate方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 银行间固定利率国债到期收益率(中债)行情走势图
        /// </summary>
        public static DataSet GetMarketRateHis()
        {
            try
            {
                MacroIndicateParam param = new MacroIndicateParam();
                param.Name = "BRPT001816";
                param.Sdate = DateTime.Now.AddYears(-1);
                param.Edate = DateTime.Now;
                param.Top = int.MaxValue;
                DataSet dataSet = DataAccess.QueryMacroIndicate(param);
                return dataSet;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetMarketRateHis方法异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 国债首页 头信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndexHead(string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_INFO";
                sp.Secucode = code + ".CFE"; ;
                sp.Dic = new Dictionary<string, string>();
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetIndexHead方法异常", ex);
            }
            return null; 
        }

        /// <summary>
        /// 可交割券分析
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDeliveryAnalysis(string MarketType, string BondType, string code)
        {
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_CTDINFO";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("SecMarket", MarketType);
                sp.Dic.Add("BondType", BondType);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeliveryAnalysis方法异常", ex);
            }
            return null; 
        }

        public static DataSet GetMarketTrend(string code, string bondCode, string StartDate, string EndDate)
        {
            if (string.IsNullOrEmpty(StartDate))
                StartDate = DateTime.Now.AddYears(-1).ToShortDateString();
            if (string.IsNullOrEmpty(EndDate))
                EndDate = DateTime.Now.ToShortDateString();
           
            string tbody = "";
            try
            {
                StockIndicateParam sp = new StockIndicateParam();
                sp.Name = "TREAFUTU_F9_HQSJ";
                sp.Secucode = code + ".CFE";
                sp.Dic = new Dictionary<string, string>();
                sp.Dic.Add("BondCode", bondCode);
                sp.Dic.Add("StartDate", StartDate);
                sp.Dic.Add("EndDate", EndDate);
                DataSet ds = (DataSet)DataAccess.QueryIndicate(sp.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetMarketTrend方法异常", ex);
            }
            return null;
        }
    }
}
