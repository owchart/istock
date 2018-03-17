using System;
using System.Collections.Generic;
using System.Text;
using EmCore;
using System.Data;
using System.Web;

namespace OwLib
{
    public class EcomomyDataHelper
    {
        #region 基础数据
        /// <summary>
        /// 获取券商统计月度最新的市场
        /// </summary>
        public static DataSet GetBondNewMarket()
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SEC_MARKET, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondNewMarket出错：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 获取地区统计月度最新的市场
        /// </summary>
        public static DataSet GetAreaNewMarket()
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_REG_MARKET, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAreaNewMarket出错：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 获取营业部统计月度最新的市场
        /// </summary>
        public static DataSet GetDepartNewMarket()
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SAL_MARKET, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDepartNewMarket出错：", ex);
                throw ex;
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 首页

        /// <summary>
        /// 配合同花顺中国地图的格式 返回省市拼音码
        /// </summary>
        /// <param name="PROVINCE"></param>
        /// <returns></returns>
        private String GetPinYin(String PROVINCE)
        {
            switch (PROVINCE)
            {
                case "黑龙江":
                    return "heilongjiang";
                case "吉林":
                    return "jilin";
                case "辽宁":
                    return "liaoning";
                case "北京":
                    return "beijing";
                case "河北":
                    return "hebei";
                case "内蒙古":
                    return "neimenggu";
                case "山西":
                    return "shanxi";
                case "天津":
                    return "tianjin";
                case "安徽":
                    return "anhui";
                case "江苏":
                    return "jiangsu";
                case "山东":
                    return "shandong";
                case "江西":
                    return "jiangxi";
                case "浙江":
                    return "zhejiang";
                case "上海":
                    return "shanghai";
                case "福建":
                    return "fujian";
                case "甘肃":
                    return "gansu";
                case "宁夏":
                    return "ningxia";
                case "青海":
                    return "qinghai";
                case "陕西":
                    return "shanxi1";
                case "新疆":
                    return "xinjiang";
                case "贵州":
                    return "guizhou";
                case "四川":
                    return "sichuan";
                case "西藏":
                    return "xizang";
                case "云南":
                    return "yunnan";
                case "重庆":
                    return "chongqing";
                case "广东":
                    return "guangdong";
                case "广西":
                    return "guangxi";
                case "海南":
                    return "hainan";
                case "河南":
                    return "henan";
                case "湖北":
                    return "hubei";
                case "湖南":
                    return "hunan";
                case "澳门":
                    return "aomen";
                case "台湾":
                    return "taiwan";
                case "香港":
                    return "xianggang";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 按地区获取券商和营业集合
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCountInArea()
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] {
                };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_COUNTA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCountInArea出错：", ex);
                throw ex;
            }
        }
        #endregion

        #region 地区弹窗

        /// <summary>
        ///  注册证券公司
        /// </summary>
        public static DataSet GetRegBondCpy(String STR_REGCODE, int limit, int pageIndex, String sort, String order)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_REGCODE", STR_REGCODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                if (!String.IsNullOrEmpty(sort))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                    {
                        _sorts.Add(new Order(sort, true));
                    }
                    else
                    {
                        _sorts.Add(new Order(sort, false));
                    }
                }
                else
                {
                    _sorts.Add(new Order("DAT_ESTADATE", false));
                }

                String[] _fileds = new String[] { };
                PagingData _DS = DataAccess.QueryByStatisticForPaging(WebDataSource.CUST_REG_SECINFO, _fileds, _querys, _sorts, _selectors, pageIndex, limit, WebDataSource.CUST_REG_SECINFO.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(_DS.ResultData);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetRegBondCpy出错：", ex);
                return null;
            }
        }


        public static DataSet GetSwithType(String STR_REGCODE, String STR_MARKET, String type, WebDataSource source, String name, String code)
        {
            
            List<Expression> _querys = new List<Expression>();
            _querys.Add(Expression.Eq("STR_REGCODE", STR_REGCODE));
            _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
            List<Expression> _selectors = new List<Expression>();
            List<Order> _sorts = new List<Order>();
            String[] _fileds = new String[] { };

            ResultData _DS1 = null;

            switch (type)
            {
                case "1"://交易数据
                    _sorts.Add(new Order("INT_RANK_JYE3", true));
                    _DS1 = DataAccess.QueryByStatistic(source, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, source.ToString());
                    break;
                case "2"://市场份额
                    _sorts.Add(new Order("INT_RANK_FE3", true));
                    _DS1 = DataAccess.QueryByStatistic(source, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, source.ToString());
                    break;
                case "3"://相对地位
                    _sorts.Add(new Order("INT_RANK_DW3", true));
                    _DS1 = DataAccess.QueryByStatistic(source, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, source.ToString());
                    break;
                case "4"://部均
                    _sorts.Add(new Order("INT_RANK_BJ3", true));
                    _DS1 = DataAccess.QueryByStatistic(source, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, source.ToString());
                    break;
            }
            return _DS1.Data;
        }

        /// <summary>
        /// 某地区 营业部
        /// </summary>
        public static DataSet GetAreaDept(String STR_REGCODE, String sort, String order, int pageIndex, int limit)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_REGCODE", STR_REGCODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                if (!String.IsNullOrEmpty(sort))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                    {
                        _sorts.Add(new Order(sort, true));
                    }
                    else
                    {
                        _sorts.Add(new Order(sort, false));
                    }
                }
                else
                {
                    _sorts.Add(new Order("STR_REGCODE", true));
                }
                String[] _fileds = new String[] { };

                PagingData _PD = DataAccess.QueryByStatisticForPaging(WebDataSource.CUST_REG_SALINFO, _fileds, _querys, _sorts, _selectors, pageIndex, limit, WebDataSource.CUST_REG_SALINFO.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(_PD.ResultData);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAreaDept出错：", ex);
                return null;
            }
        }

        #endregion

        #region 交易排名

        /// <summary>
        /// 获取证券公司交易排名
        /// type:1 交易数据  2 市场份额  3 相对低位  4 均部
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBondTradeRand(String STR_MARKET, String STR_PROVINCECODE, String dateB, String dateE, String type)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_MARKET))
                {
                    _querys.Add(Expression.Eq("STR_MARKET", STR_MARKET));
                }
                if (!String.IsNullOrEmpty(STR_PROVINCECODE))
                    _querys.Add(Expression.Eq("STR_PROVINCECODE", STR_PROVINCECODE));
                else
                    _querys.Add(Expression.Eq("STR_PROVINCECODE", "000000"));
                _querys.Add(Expression.Eq("STR_STARTDATE", dateB));
                _querys.Add(Expression.Eq("STR_ENDDATE", dateE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };
                ResultData _DS = new ResultData(null, "", 0);
                switch (type)
                {
                    case "1":
                        _sorts.Add(new Order("INT_RANK_JYE3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_JYETRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_JYETRADE.ToString());
                        break;
                    case "2":
                        _sorts.Add(new Order("INT_RANK_FE3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_FETRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_FETRADE.ToString());
                        break;
                    case "3":
                        _sorts.Add(new Order("INT_RANK_DW3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_DWTRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_DWTRADE.ToString());
                        break;
                    case "4":
                        _sorts.Add(new Order("INT_RANK_BJ3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_BJTRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_BJTRADE.ToString());
                        break;
                }
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondTradeRand出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取证券公司交易排名  全部
        /// type:1 交易数据  2 市场份额  3 相对低位  4 均部
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllBondTrade(String STR_MARKET, String type, String STR_REGCODE)
        {
            try
            {
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };
                ResultData _DS = null;

                
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                if (!String.IsNullOrEmpty(STR_REGCODE))
                {
                    _querys.Add(Expression.Eq("STR_REGCODE", HttpUtility.UrlDecode(STR_REGCODE)));
                    switch (type)
                    {
                        case "1":
                            _sorts.Add(new Order("INT_RANK_JYE3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_SECJYERANKDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_SECJYERANKDETAIL.ToString());
                            break;
                        case "2":
                            _sorts.Add(new Order("INT_RANK_FE3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_SECFERANKDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_SECFERANKDETAIL.ToString());
                            break;
                        case "3":
                            _sorts.Add(new Order("INT_RANK_DW3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_SECDWRANKDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_SECDWRANKDETAIL.ToString());
                            break;
                        case "4":
                            _sorts.Add(new Order("INT_RANK_BJ3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_SECBJRANKDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_SECBJRANKDETAIL.ToString());
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case "1":
                            _sorts.Add(new Order("INT_RANK_JYE3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SECALL_JYENEWRANK, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SECALL_JYENEWRANK.ToString());
                            break;
                        case "2":
                            _sorts.Add(new Order("INT_RANK_FE3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SECALL_FENEWRANK, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SECALL_FENEWRANK.ToString());
                            break;
                        case "3":
                            _sorts.Add(new Order("INT_RANK_DW3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SECALL_DWNEWRANK, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SECALL_DWNEWRANK.ToString());
                            break;
                        case "4":
                            _sorts.Add(new Order("INT_RANK_BJ3", true));
                            _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SECALL_BJNEWRANK, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SECALL_BJNEWRANK.ToString());
                            break;
                    }
                }
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAllBondTrade出错：", ex);
                return null;
            }
        }
        /// <summary>
        /// 获取地区交易排名 
        /// type:1 交易数据  2 市场份额  3 相对低位  4 均部
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAreaTradeRand(String STR_MARKET, String dateB, String dateE, String type)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_MARKET))
                {
                    _querys.Add(Expression.Eq("STR_MARKET", STR_MARKET));
                }
                _querys.Add(Expression.Eq("STR_STARTDATE", dateB));
                _querys.Add(Expression.Eq("STR_ENDDATE", dateE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };
                ResultData _DS = new ResultData(null, "", 0);
                switch (type)
                {
                    case "1":
                        _sorts.Add(new Order("INT_RANK_JYE3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_JYETRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_JYETRADE.ToString());
                        break;
                    case "2":
                        _sorts.Add(new Order("INT_RANK_FE3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_FETRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_FETRADE.ToString());
                        break;
                    case "3":
                        _sorts.Add(new Order("INT_RANK_DW3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_DWTRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_DWTRADE.ToString());
                        break;
                    case "4":
                        _sorts.Add(new Order("INT_RANK_BJ3", true));
                        _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REG_BJTRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REG_BJTRADE.ToString());
                        break;
                }
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAreaTradeRand出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 获取地区交易排名  全部
        /// type:1 交易数据  2 市场份额  3 相对低位  4 均部
        /// </summary>
        public static DataSet AllBondArea(String STR_MARKET, String type)
        {
            try
            {
                switch (type)
                {
                    case "1":
                        #region 交易额
                        List<Expression> _querys = new List<Expression>();
                        _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                        List<Expression> _selectors = new List<Expression>();
                        List<Order> _sorts = new List<Order>();
                        String[] _fileds = new String[] { };
                        _sorts.Add(new Order("DEC_JYE3", false));
                        ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_REGALL_JYENEWRANK, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_REGALL_JYENEWRANK.ToString());
                        return _DS.Data;
                        #endregion
                        break;
                    case "2":
                        #region 市场份额
                        List<Expression> _querys1 = new List<Expression>();
                        _querys1.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                        List<Expression> _selectors1 = new List<Expression>();
                        List<Order> _sorts1 = new List<Order>();
                        _sorts1.Add(new Order("DEC_FE3", false));
                        String[] _fileds1 = new String[] { };
                        ResultData _DS1 = DataAccess.QueryByStatistic(WebDataSource.CUST_REGALL_FENEWRANK, _fileds1, _querys1, _sorts1, _selectors1, 1, int.MaxValue, WebDataSource.CUST_REGALL_FENEWRANK.ToString());
                        return _DS1.Data;
                        #endregion
                        break;
                    case "3":
                        #region 相对地位
                        List<Expression> _querys2 = new List<Expression>();
                        _querys2.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                        List<Expression> _selectors2 = new List<Expression>();
                        List<Order> _sorts2 = new List<Order>();
                        _sorts2.Add(new Order("DEC_DW3", false));
                        String[] _fileds2 = new String[] { };
                        ResultData _DS2 = DataAccess.QueryByStatistic(WebDataSource.CUST_REGALL_DWNEWRANK, _fileds2, _querys2, _sorts2, _selectors2, 1, int.MaxValue, WebDataSource.CUST_REGALL_DWNEWRANK.ToString());
                        return _DS2.Data;
                        #endregion
                        break;
                    case "4":
                        #region 部均
                        List<Expression> _querys3 = new List<Expression>();
                        _querys3.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                        List<Expression> _selectors3 = new List<Expression>();
                        List<Order> _sorts3 = new List<Order>();
                        _sorts3.Add(new Order("DEC_BJ3", false));
                        String[] _fileds3 = new String[] { };
                        ResultData _DS3 = DataAccess.QueryByStatistic(WebDataSource.CUST_REGALL_BJNEWRANK, _fileds3, _querys3, _sorts3, _selectors3, 1, int.MaxValue, WebDataSource.CUST_REGALL_BJNEWRANK.ToString());
                        return _DS3.Data;
                        #endregion
                        break;
                }

                return null;
                //return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("AllBondArea出错：", ex);
                return null;
                //return null;
            }
        }

        #endregion

        #region 营业部交易排名

        public static DataSet GetDepartTradeJson()
        {
            String DeptMarket = "";
            String time = "";
            String timeB = time.Substring(0, 4) + "-01";
            return GetDepartTrade(DeptMarket, "000000", "000000", "月报", timeB, time, "", "", 50, 1);
        }
        /// <summary>
        /// 交易数据
        /// </summary>
        public static DataSet GetDepartTrade(String STR_MARKET, String STR_PROVINCECODE, String STR_SECODE, String STR_REPORTTYPE, String STR_STARTDATE, String STR_ENDDATE,
            String sort, String order, int limit, int pageIndex)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", STR_MARKET));
                _querys.Add(Expression.Eq("STR_REPORTTYPE", STR_REPORTTYPE));
                _querys.Add(Expression.Eq("STR_PROVINCECODE", STR_PROVINCECODE));
                _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                _querys.Add(Expression.Eq("STR_STARTDATE", STR_STARTDATE));
                _querys.Add(Expression.Eq("STR_ENDDATE", STR_ENDDATE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                if (!String.IsNullOrEmpty(sort))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                    {
                        _sorts.Add(new Order(sort, true));
                    }
                    else
                    {
                        _sorts.Add(new Order(sort, false));
                    }
                }
                else
                {
                    _sorts.Add(new Order("DEC_JYE3", false));
                }

                String[] _fileds = new String[] { };
                PagingData _DS = DataAccess.QueryByStatisticForPaging(WebDataSource.CUST_SAL_JYETRADE, _fileds, _querys, _sorts, _selectors, pageIndex, limit, WebDataSource.CUST_SAL_JYETRADE.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(_DS.ResultData);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDepartTrade出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 市场份额
        /// </summary>
        public static DataSet GetDepartMarket(String STR_MARKET, String STR_PROVINCECODE, String STR_SECODE, String STR_REPORTTYPE, String STR_STARTDATE, String STR_ENDDATE,
            String sort, String order, int limit, int pageIndex)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_REPORTTYPE", HttpUtility.UrlDecode(STR_REPORTTYPE)));
                _querys.Add(Expression.Eq("STR_PROVINCECODE", STR_PROVINCECODE));
                _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                _querys.Add(Expression.Eq("STR_STARTDATE", STR_STARTDATE));
                _querys.Add(Expression.Eq("STR_ENDDATE", STR_ENDDATE));
                WebDataSource source = WebDataSource.CUST_SAL_QGFETRADE;
                if (!STR_PROVINCECODE.Equals("000000") && !STR_SECODE.Equals("000000")) //两者都不是全国
                {
                    source = WebDataSource.CUST_SAL_FETRADE;
                }
                else if (STR_PROVINCECODE.Equals("000000") && !STR_SECODE.Equals("000000")) //省份是全国
                {
                    source = WebDataSource.CUST_SAL_SECFETRADE;
                }
                else if (!STR_PROVINCECODE.Equals("000000") && STR_SECODE.Equals("000000")) //证券公司是全国
                {
                    source = WebDataSource.CUST_SAL_PVCFETRADE;
                }
                else //两者都是全国
                {
                    source = WebDataSource.CUST_SAL_QGFETRADE;
                }
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                if (!String.IsNullOrEmpty(sort))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                    {
                        _sorts.Add(new Order(sort, true));
                    }
                    else
                    {
                        _sorts.Add(new Order(sort, false));
                    }
                }
                else
                {
                    _sorts.Add(new Order("DEC_FE3", false));
                }

                String[] _fileds = new String[] { };
                PagingData _DS = DataAccess.QueryByStatisticForPaging(source, _fileds, _querys, _sorts, _selectors, pageIndex, limit, source.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(_DS.ResultData);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDepartMarket出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 相对地位
        /// </summary>
        public static DataSet GetDepartPlace(String STR_MARKET, String STR_PROVINCECODE, String STR_SECODE, String STR_REPORTTYPE, String STR_STARTDATE, String STR_ENDDATE,
            String sort, String order, int limit, int pageIndex)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_REPORTTYPE", HttpUtility.UrlDecode(STR_REPORTTYPE)));
                _querys.Add(Expression.Eq("STR_PROVINCECODE", STR_PROVINCECODE));
                _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                _querys.Add(Expression.Eq("STR_STARTDATE", STR_STARTDATE));
                _querys.Add(Expression.Eq("STR_ENDDATE", STR_ENDDATE));
                WebDataSource source = WebDataSource.CUST_SAL_QGDWTRADE;
                if (!STR_PROVINCECODE.Equals("000000") && !STR_SECODE.Equals("000000")) //两者都不是全国
                {
                    source = WebDataSource.CUST_SAL_DWTRADE;
                }
                else if (STR_PROVINCECODE.Equals("000000") && !STR_SECODE.Equals("000000")) //省份是全国
                {
                    source = WebDataSource.CUST_SAL_SECDWTRADE;
                }
                else if (!STR_PROVINCECODE.Equals("000000") && STR_SECODE.Equals("000000")) //证券公司是全国
                {
                    source = WebDataSource.CUST_SAL_PVCDWTRADE;
                }
                else //两者都是全国
                {
                    source = WebDataSource.CUST_SAL_QGDWTRADE;
                }
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                if (!String.IsNullOrEmpty(sort))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                    {
                        _sorts.Add(new Order(sort, true));
                    }
                    else
                    {
                        _sorts.Add(new Order(sort, false));
                    }
                }
                else
                {
                    _sorts.Add(new Order("DEC_DW3", false));
                }

                String[] _fileds = new String[] { };
                PagingData _DS = DataAccess.QueryByStatisticForPaging(source, _fileds, _querys, _sorts, _selectors, pageIndex, limit, source.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(_DS.ResultData);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDepartPlace出错：", ex);
                return null;
            }
        }
        #endregion

        #region 证券公司大全

        public static DataSet GetBondMsgListJson()
        {
            return GetBondMsgList("", "", "0");
        }
        /// <summary>
        /// 证券公司大全
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBondMsgList(String STR_SECNAME, String STR_REPLACE, String type)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();

                if (!String.IsNullOrEmpty(STR_SECNAME))
                    _querys.Add(Expression.Eq("STR_SECNAME", HttpUtility.UrlDecode(STR_SECNAME)));

                if (!String.IsNullOrEmpty(STR_REPLACE))
                    _querys.Add(Expression.Eq("STR_REPLACE", HttpUtility.UrlDecode(STR_REPLACE)));

                if (!String.IsNullOrEmpty(type))
                {
                    switch (type)
                    {
                        case "1"://小于等于1亿元
                            _querys.Add(Expression.Le("DEC_RECAPITAL", 1));
                            break;
                        case "2"://1-5亿元
                            _querys.Add(Expression.Le("DEC_RECAPITAL", 5));
                            _querys.Add(Expression.Gt("DEC_RECAPITAL", 1));
                            break;
                        case "3"://5-10亿元
                            _querys.Add(Expression.Le("DEC_RECAPITAL", 10));
                            _querys.Add(Expression.Gt("DEC_RECAPITAL", 5));
                            break;
                        case "4"://10-20亿元
                            _querys.Add(Expression.Le("DEC_RECAPITAL", 10));
                            _querys.Add(Expression.Gt("DEC_RECAPITAL", 20));
                            break;
                        case "5"://20-30亿元
                            _querys.Add(Expression.Le("DEC_RECAPITAL", 30));
                            _querys.Add(Expression.Gt("DEC_RECAPITAL", 20));
                            break;
                        case "6"://30-50亿元
                            _querys.Add(Expression.Le("DEC_RECAPITAL", 50));
                            _querys.Add(Expression.Gt("DEC_RECAPITAL", 30));
                            break;
                        case "7"://大于50亿元
                            _querys.Add(Expression.Gt("DEC_RECAPITAL", 50));
                            break;
                        default:
                            break;
                    }
                }
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_SECENAME", true));

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_INFO, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_INFO.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondMsgList出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 证券公司大全 比较
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBondCompare(String STR_MARKET, String STR_SECODEList)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();

                if (!String.IsNullOrEmpty(STR_MARKET))
                    _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));

                if (!String.IsNullOrEmpty(STR_SECODEList))
                    _querys.Add(Expression.In("STR_SECODE", STR_SECODEList.Split(',')));

                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_SECENAME", true));

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_COMPARE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_COMPARE.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondCompare出错：", ex);
                return null;
            }
        }
        /// <summary>
        /// 证券公司大全 绘图
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBondChart(String STR_MARKET, String STR_SECODEList)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();

                if (!String.IsNullOrEmpty(STR_MARKET))
                    _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                if (!String.IsNullOrEmpty(STR_SECODEList))
                    _querys.Add(Expression.In("STR_SECODE", STR_SECODEList.Split(',')));

                String dataE = CommDao.SafeToDateString(DateTime.Now, "yyyy-MM");
                String dataB = CommDao.SafeToDateString(DateTime.Now.AddMonths(-12), "yyyy-MM");
                _querys.Add(Expression.Ge("STR_STATISMONTH", dataB));
                _querys.Add(Expression.Le("STR_STATISMONTH", dataE));

                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", true));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SEC_JYE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondChart出错：", ex);
                return null;
            }
        }

        public static DataSet GetBondCmpyCount(String STR_SECENAME)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_SECENAME))
                    _querys.Add(Expression.Eq("STR_SECODE", HttpUtility.UrlDecode(STR_SECENAME)));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_SALINFO, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_SALINFO.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondCmpyCount出错：", ex);
                return null;
            }
        }
        #endregion

        #region 营业部大全

        public DataSet GetDeptMsgListJson()
        {
            return GetDeptMsgList("", "", "", "", "", "", 1, 100);
        }
        /// <summary>
        /// 营业部大全
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDeptMsgList(String STR_SECNAME, String STR_SALNAME, String STR_PROVINCE, String STR_CITY,
            String order, String sort, int pageIndex, int limit)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();

                if (!String.IsNullOrEmpty(STR_SECNAME))
                    _querys.Add(Expression.Eq("STR_SECNAME", HttpUtility.UrlDecode(STR_SECNAME)));

                if (!String.IsNullOrEmpty(STR_SALNAME))
                    _querys.Add(Expression.Like("STR_SALNAME", String.Format("%{0}%", HttpUtility.UrlDecode(STR_SALNAME))));

                if (!String.IsNullOrEmpty(STR_PROVINCE))
                    _querys.Add(Expression.Eq("STR_PROVINCE", HttpUtility.UrlDecode(STR_PROVINCE)));

                if (!String.IsNullOrEmpty(STR_CITY))
                    _querys.Add(Expression.Eq("STR_CITY", HttpUtility.UrlDecode(STR_CITY)));

                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                if (!String.IsNullOrEmpty(sort))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                    {
                        _sorts.Add(new Order(sort, true));
                    }
                    else
                    {
                        _sorts.Add(new Order(sort, false));
                    }
                }
                else
                {
                    _sorts.Add(new Order("STR_SECENAME", true));
                }

                String[] _fileds = new String[] { };
                PagingData _DS = DataAccess.QueryByStatisticForPaging(WebDataSource.CUST_SAL_INFO, _fileds, _querys, _sorts, _selectors, pageIndex, limit, WebDataSource.CUST_SAL_INFO.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(_DS.ResultData);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeptMsgList出错：", ex);
                throw ex;
                // return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 营业部大全 比较
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDeptCompare(String STR_SECODEList)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();

                if (!String.IsNullOrEmpty(STR_SECODEList))
                    _querys.Add(Expression.In("STR_SALCODE", STR_SECODEList.Split(',')));

                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SAL_COMPARE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SAL_COMPARE.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeptCompare出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 营业部大全 绘图
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDeptChart(String STR_SECODEList)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_SECODEList))
                    _querys.Add(Expression.In("STR_SALCODE", STR_SECODEList.Split(',')));

                String dataE = CommDao.SafeToDateString(DateTime.Now, "yyyy-MM");
                String dataB = CommDao.SafeToDateString(DateTime.Now.AddMonths(-12), "yyyy-MM");
                _querys.Add(Expression.Ge("STR_STATISMONTH", dataB));
                _querys.Add(Expression.Le("STR_STATISMONTH", dataE));

                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", true));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SAL_JYE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeptChart出错：", ex);
                return null;
            }
        }

        #endregion

        #region 证券公司弹窗

        /// <summary>
        /// 获取公司详细
        /// </summary>
        public static DataSet GetBondCompanyDetail(String STR_SECODE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SEC_INFODETAIL, _fileds, _querys, _sorts, _selectors, 1, 1);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondCompanyDetail出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 财务数据
        /// </summary>
        public static DataSet GetBondFinance(String STR_REPORTTYPE, String STR_SECODE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_REPORTTYPE))
                    _querys.Add(Expression.Eq("STR_REPORTTYPE", HttpUtility.UrlDecode(STR_REPORTTYPE)));
                if (!String.IsNullOrEmpty(STR_SECODE))
                    _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_REPORTDATE", false));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SEC_STM, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondFinance出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 资产负债表
        /// </summary>
        public static DataSet GetBondAssets(String STR_REPORTTYPE, String STR_SECODE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_REPORTTYPE))
                    _querys.Add(Expression.Eq("STR_REPORTTYPE", HttpUtility.UrlDecode(STR_REPORTTYPE)));
                if (!String.IsNullOrEmpty(STR_SECODE))
                    _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_REPORTDATE", false));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SEC_BLANCE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondAssets出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 基本报表
        /// </summary>
        public static DataSet GetBondBaseFin(String STR_MARKET, String STR_SECODE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_MARKET))
                    _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                if (!String.IsNullOrEmpty(STR_SECODE))
                    _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_BASIC, _fileds, _querys, _sorts, _selectors, 1, 1, WebDataSource.CUST_SEC_BASIC.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondBaseFin出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 历史表现
        /// </summary>
        public static DataSet GetBondHisList(String STR_MARKET, String STR_SECODE, String STR_STATISMONTH)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                DateTime dateTime = Convert.ToDateTime(STR_STATISMONTH + "-01");
                String monthLow = dateTime.AddMonths(-11).ToString("yyyy-MM");
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                _querys.Add(Expression.Le("STR_STATISMONTH", STR_STATISMONTH));
                _querys.Add(Expression.Ge("STR_STATISMONTH", monthLow));
                //Expression expA = Expression.And(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)), Expression.Eq("STR_SECODE", STR_SECODE));
                //_querys.Add(Expression.And(expA, Expression.Le("STR_STATISMONTH", STR_STATISMONTH)), Expression.Ge("STR_STATISMONTH", monthLow));

                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };

                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_OLDDATA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_OLDDATA.ToString());

                List<Expression> _querys1 = new List<Expression>();
                _querys1.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys1.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                _querys1.Add(Expression.Eq("STR_PROVINCE", "全国"));
                List<Expression> _selectors1 = new List<Expression>();
                List<Order> _sorts1 = new List<Order>();
                _sorts1.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds1 = new String[] { };
                DataSet _DSBJ = DataAccess.Query(WebDataSource.CUST_SEC_BJ, _fileds1, _querys1, _sorts1, _selectors1, 1, int.MaxValue);

                return _DSBJ;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondHisList出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 旗下比较 全部
        /// </summary>
        public static DataSet GetBondAllBootComp(String STR_SECODE, String type)
        {
            try
            {
                switch (type)
                {
                    case "1":
                        #region 交易数据
                        List<Expression> _querys = new List<Expression>();
                        _querys.Add(Expression.Eq("STR_MARKET", "深市"));
                        _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                        List<Order> _sorts = new List<Order>();
                        _sorts.Add(new Order("INT_RANK_JYE3", true));
                        String[] _fileds = new String[] { };
                        List<Expression> _selectors = new List<Expression>();
                        ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_SALJYETRADE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_SALJYETRADE.ToString());
                        return _DS.Data;
                        #endregion
                        break;

                    case "2":
                        #region 市场份额
                        List<Expression> _querys1 = new List<Expression>();
                        _querys1.Add(Expression.Eq("STR_MARKET", "深市"));
                        _querys1.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                        List<Order> _sorts1 = new List<Order>();
                        _sorts1.Add(new Order("INT_RANK_FE3", true));
                        String[] _fileds1 = new String[] { };
                        List<Expression> _selectors1 = new List<Expression>();
                        ResultData _DS1 = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_SALFETRADE, _fileds1, _querys1, _sorts1, _selectors1, 1, int.MaxValue, WebDataSource.CUST_SEC_SALFETRADE.ToString());
                        return _DS1.Data;
                        #endregion
                        break;

                    case "3":
                        #region 相对地位
                        List<Expression> _querys2 = new List<Expression>();
                        _querys2.Add(Expression.Eq("STR_MARKET", "深市"));
                        _querys2.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                        List<Order> _sorts2 = new List<Order>();
                        _sorts2.Add(new Order("INT_RANK_DW3", true));
                        String[] _fileds2 = new String[] { };
                        List<Expression> _selectors2 = new List<Expression>();
                        ResultData _DS2 = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_SALDWTRADE, _fileds2, _querys2, _sorts2, _selectors2, 1, int.MaxValue, WebDataSource.CUST_SEC_SALDWTRADE.ToString());
                        return _DS2.Data;
                        #endregion
                        break;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondAllBootComp出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 历史更多
        /// </summary>
        public static DataSet GetBondAllHis(String STR_SECODE, String STR_MARKET, String type)
        {
            try
            {
                if (type != "BJ")
                {
                    List<Expression> _querys = new List<Expression>();
                    _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                    _querys.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                    List<Expression> _selectors = new List<Expression>();
                    List<Order> _sorts = new List<Order>();
                    String[] _fileds = new String[] { };
                    _sorts.Add(new Order("STR_STATISMONTH", false));
                    ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_OLDDATA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_OLDDATA.ToString());
                    return _DS.Data;
                }
                else
                {
                    List<Expression> _querys1 = new List<Expression>();
                    _querys1.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                    _querys1.Add(Expression.Eq("STR_SECODE", STR_SECODE));
                    _querys1.Add(Expression.Eq("STR_PROVINCE", "全国"));
                    List<Expression> _selectors1 = new List<Expression>();
                    List<Order> _sorts1 = new List<Order>();
                    _sorts1.Add(new Order("STR_STATISMONTH", false));
                    String[] _fileds1 = new String[] { };
                    DataSet _DSBJ = DataAccess.Query(WebDataSource.CUST_SEC_BJ, _fileds1, _querys1, _sorts1, _selectors1, 1, int.MaxValue);
                    return _DSBJ;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondAllHis出错：", ex);
                return null;
            }
        }

        #endregion

        #region 营业部弹窗

        /// <summary>
        /// 基本信息
        /// </summary>
        public static DataSet GetBaseDeptMsg(String STR_SALCODE)
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_SALCODE", STR_SALCODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SAL_INFODETAIL, _fileds, _querys, _sorts, _selectors, 1, 1, WebDataSource.CUST_SAL_INFODETAIL.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBaseDeptMsg出错：", ex);
                return null;
            }
        }
        /// <summary>
        /// 基本报表
        /// </summary>
        public static DataSet GetDeptBaseFin(String STR_SALCODE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", "深市"));
                if (!String.IsNullOrEmpty(STR_SALCODE))
                    _querys.Add(Expression.Eq("STR_SALCODE", STR_SALCODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SAL_BASIC, _fileds, _querys, _sorts, _selectors, 1, 1, WebDataSource.CUST_SAL_BASIC.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeptBaseFin出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 历史表现
        /// </summary>
        public static DataSet GetDeptHisList(String STR_MARKET, String STR_SALCODE, String STR_STATISMONTH, String STR_REPORTTYPE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                DateTime dateTime = Convert.ToDateTime(STR_STATISMONTH + "-01");
                String monthLow = dateTime.AddMonths(-11).ToString("yyyy-MM");
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_REPORTTYPE", HttpUtility.UrlDecode(STR_REPORTTYPE)));
                _querys.Add(Expression.Eq("STR_SALCODE", STR_SALCODE));
                if (HttpUtility.UrlDecode(STR_REPORTTYPE).Equals("月报"))
                {
                    _querys.Add(Expression.Le("STR_STATISMONTH", STR_STATISMONTH));
                    _querys.Add(Expression.Ge("STR_STATISMONTH", monthLow));
                }
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };

                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SAL_OLDDATA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SAL_OLDDATA.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeptHisList出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 历史更多
        /// </summary>
        public static DataSet GetDeptAllHis(String STR_SALCODE, String STR_MARKET, String STR_REPORTTYPE, String type)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_REPORTTYPE", HttpUtility.UrlDecode(STR_REPORTTYPE)));
                _querys.Add(Expression.Eq("STR_SALCODE", STR_SALCODE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };
                _sorts.Add(new Order("STR_STATISMONTH", false));
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SAL_OLDDATA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SAL_OLDDATA.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDeptAllHis出错：", ex);
                return null;
            }
        }

        #endregion

        #region 证券开户统计

        public static object GetAcountJson()
        {
            List<object> result = new List<object>();
            result.Add(GetOpenAcount("沪市", "A股"));
            result.Add(GetAStockAge("沪市"));
            result.Add(GetAStockMoney("自然人", ""));
            result.Add(GetOpenArea("沪市"));
            return result;
        }
        /// <summary>
        /// 证券账户变动本期分析
        /// </summary>

        public static DataSet GetOpenAcount(String STR_MARKET, String STR_SECTYPE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", STR_MARKET));
                _querys.Add(Expression.Eq("STR_SECTYPE", STR_SECTYPE));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("INT_INVESTORRANK", true));
                String[] _fileds = new String[] { };

                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_ST_INVESTOROA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_ST_INVESTOROA.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenAcount出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 证券账户变动本期分析  开户主题详细信息
        /// </summary>
        public static DataSet GetOpenAcountDetail(String STR_MARKET, String STR_SECTYPE, String STR_INVESTORTYPE, String STR_STATISMONTH)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_SECTYPE", HttpUtility.UrlDecode(STR_SECTYPE)));
                _querys.Add(Expression.Eq("STR_INVESTORTYPE", HttpUtility.UrlDecode(STR_INVESTORTYPE)));
                _querys.Add(Expression.Le("STR_STATISMONTH", STR_STATISMONTH));
                _querys.Add(Expression.Ge("STR_STATISMONTH", Convert.ToDateTime(STR_STATISMONTH + "-01").AddMonths(-11).ToString("yyyy-MM")));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_INVESTOROADETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenAcountDetail出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 证券账户变动本期分析  开户主题详细信息  全部  弹窗
        /// </summary>
        public static DataSet GetOpenAcountAll(String STR_MARKET, String STR_SECTYPE, String STR_INVESTORTYPE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_SECTYPE", HttpUtility.UrlDecode(STR_SECTYPE)));
                _querys.Add(Expression.Eq("STR_INVESTORTYPE", HttpUtility.UrlDecode(STR_INVESTORTYPE)));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_INVESTOROADETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenAcountAll出错：", ex);
                return null;
            }
        }


        /// <summary>
        /// A股年龄分布
        /// </summary>
        public static DataSet GetAStockAge(String STR_MARKET)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", (STR_MARKET)));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_ASSETREDN, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAStockAge出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public static DataSet GetAStockMoney(String STR_MARKET, String STR_STATISMONTH)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", STR_MARKET));
                //if (String.IsNullOrEmpty(STR_STATISMONTH))
                //    STR_STATISMONTH = DateTime.Now.ToString("yyyy-MM");
                //_querys.Add(Expression.Le("STR_STATISMONTH", STR_STATISMONTH));
                //_querys.Add(Expression.Ge("STR_STATISMONTH", Convert.ToDateTime(STR_STATISMONTH + "-01").AddMonths(-11).ToString("yyyy-MM")));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_ASMVDIST, _fileds, _querys, _sorts, _selectors, 1, 12);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAStockMoney出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// A股市值分布
        /// </summary>
        public static DataSet GetAStockMoneyAll(String STR_MARKET)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));//开户主体
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_ASMVDIST, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetAStockMoneyAll出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// A股开户地区分布
        /// </summary>
        public static DataSet GetOpenArea(String STR_MARKET)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", STR_MARKET));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _fileds = new String[] { };

                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_ST_TREGIONFB, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_ST_TREGIONFB.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenArea出错：", ex);
                throw ex;
                //return Json("查询出错" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// A股开户地区分布 详细 
        /// </summary>
        public static DataSet GetOpenAreaDetail(String STR_MARKET, String STR_REGNANE, String STR_STATISMONTH)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_REGNANE", HttpUtility.UrlDecode(STR_REGNANE)));
                if (String.IsNullOrEmpty(STR_STATISMONTH))
                    STR_STATISMONTH = DateTime.Now.ToString("yyyy-MM");
                _querys.Add(Expression.Le("STR_STATISMONTH", STR_STATISMONTH));
                _querys.Add(Expression.Ge("STR_STATISMONTH", Convert.ToDateTime(STR_STATISMONTH + "-01").AddMonths(-11).ToString("yyyy-MM")));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_TREGIONFBDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenAreaDetail出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// A股开户地区分布 详细 
        /// </summary>
        public static DataSet GetOpenAreaAll(String STR_MARKET, String STR_REGNANE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("STR_MARKET", HttpUtility.UrlDecode(STR_MARKET)));
                _querys.Add(Expression.Eq("STR_REGNANE", HttpUtility.UrlDecode(STR_REGNANE)));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_STATISMONTH", false));
                String[] _fileds = new String[] { };

                DataSet _DS = DataAccess.Query(WebDataSource.CUST_ST_TREGIONFBDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenAreaAll出错：", ex);
                return null;
            }
        }

        #endregion

        #region 查询条件

        /// <summary>
        /// 获取证券公司
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBondCompanyInput()
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_SECENAME", true));

                String[] _fileds = new String[] { };
                ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.CUST_SEC_INFO, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue, WebDataSource.CUST_SEC_INFO.ToString());
                return _DS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondCompanyInput出错：", ex);
                throw ex;
                //return Json("查询出错", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取城市  
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCityList(String STR_PROVINCECODE)
        {
            try
            {
                List<Expression> _querys = new List<Expression>();
                if (!String.IsNullOrEmpty(STR_PROVINCECODE))
                {
                    _querys.Add(Expression.Eq("STR_PROVINCECODE", STR_PROVINCECODE));
                }
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_CITY", true));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_CITY, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCityList出错：", ex);
                throw ex;
                // return Json("查询出错", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 获取省份  
        /// </summary>
        /// <returns></returns>
        public static DataSet GeProvinceList()
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_CITY", true));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_PROVINCE, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GeProvinceList出错：", ex);
                throw ex;
                //return Json("查询出错", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取证券公司城市  
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBondCityList()
        {
            try
            {

                List<Expression> _querys = new List<Expression>();
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_CITY", true));

                String[] _fileds = new String[] { };
                DataSet _DS = DataAccess.Query(WebDataSource.CUST_SEC_CITY, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBondCityList出错：", ex);
                throw ex;
                //return Json("查询出错", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
