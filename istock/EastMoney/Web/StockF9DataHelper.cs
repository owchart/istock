using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmCore;

namespace OwLib
{
    /// <summary>
    /// 股票F9数据类
    /// </summary>
    public class StockF9DataHelper
    {
        /// <summary>
        /// 公司介绍
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetCompanyIntroductionInfo(String securityCode)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                String[] _field = new String[] { };


                _DS = DataAccess.Query(WebDataSource.F9_INSTINFO, _field, _query, _sort, _selector, 1, 20);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCompanyIntroductionInfo方法出现异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 曾用名历史
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetCompanyNameHistoryList(String securityCode)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("CHANGEDATE", false));
                String[] _field = new String[] { };

                _DS = DataAccess.Query(WebDataSource.SF9_NAMECHGB, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCompanyNameHistoryList方法出现异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 股本结构
        /// </summary>
        /// <param name="securityCode"></param>
        /// <param name="rotate">是否旋转</param>
        /// <param name="seperate">是否分隔</param>
        /// <param name="order">排序(desc or asc)</param>
        /// <returns></returns>
        public static DataSet GetStockStructureList(String securityCode, String order,
            String yearList, String reportTypeList, String dateSearchType)
        {
            DataSet _DS = new DataSet();

            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "DAT_CHANGEDATE"));
                //int count = CommonController.GetDateRangeQueryCount(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));

                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("DAT_CHANGEDATE", order == "asc"));
                String[] _field = new String[] { };

                _DS = DataAccess.Query(WebDataSource.SF9_CAPITALSTRUCT, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockStructureList方法出现异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 十大股东明细
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetTop10HolderList(String securityCode, int type, String order, String yearList, String reportTypeList, String dateSearchType)
        {
            StringBuilder holderList = new StringBuilder();
            WebDataSource source = new WebDataSource();
            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "ENDDATE"));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                bool ascending = order == "asc";
                _sorts.Add(new Order("ENDDATE", ascending));
                _sorts.Add(new Order("DEC_QSDGDCGS", false));

                switch (type)
                {
                    case 1:
                        source = WebDataSource.SF9_TTTSD;
                        break;
                    case 2:
                        source = WebDataSource.SF9_TTTTS;
                        break;
                    default:
                        break;
                }

                String[] _field = new String[] { };
                DataSet _DS = DataAccess.Query(source, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetTop10HolderList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 机构投资者
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "CacheChangeBySixHours")]
        public static DataSet GetInstitutionInvestorList(String securityCode, String order, String sort, String timeorder, String yearList,
            String reportTypeList, String dateSearchType)
        {
            DataSet _DS = new DataSet();
            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "REPORTDATE"));
                //int count = CommonController.GetDateRangeQueryCount(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));
                //_query.Add(Expression.Ge("REPORTDATE", DateTime.Now.AddYears(-3)));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("REPORTDATE", timeorder == "asc"));
                _sorts.Add(new Order(sort, order == "asc"));
                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_INSTINVESTOR, _field, _query, _sorts, _selector, 1, int.MaxValue);
                //_DS = CommonController.FilterDataByDateRange(_DS, "REPORTDATE", yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), timeorder == "asc");
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetInstitutionInvestorList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 股东户数
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetStockHolderNumberList(String securityCode, String order, String yearList,
            String reportTypeList, String dateSearchType)
        {
            DataSet _DS = new DataSet();
            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "ENDDATE"));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                bool ascending = order == "asc";
                _sorts.Add(new Order("ENDDATE", ascending));
                String[] _field = new String[] { };
                //ResultData _DS = DataAccess.QueryByStatistic(WebDataSource.F9_TNOSII_DETIAL, _field, _query, _sorts, _selector, 1, int.MaxValue, WebDataSource.F9_TNOSII_DETIAL.ToString());
                _DS = DataAccess.Query(WebDataSource.SF9_TNOSII, _field, _query, _sorts, _selector, 1, int.MaxValue);
                //_DS.Data = CommonController.FilterDataByDateRange(_DS.Data, "ENDDATE", yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), ascending);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockHolderNumberList方法异常" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 限售股解禁时间表
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GetStockUnlimitedTimeList(String securityCode)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_ZQDM", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("KLTSJ", false));
                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_XSJJDATE, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockHolderNumberList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 董事会 监事会 高级管理人员 历任管理层成员
        /// </summary>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "CacheChangeBySixHours")]
        public static DataSet GetManagerInfo(String securityCode, int type)
        {
            WebDataSource source = new WebDataSource();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                //_sorts.Add(new Order("POSITIONTYPECODE", false));

                switch (type)
                {
                    case 1://----董事会
                        source = WebDataSource.SF9_TBODI;
                        _sorts.Add(new Order("STR_PLEVEL", true));
                        _sorts.Add(new Order("POSITION", false));
                        break;
                    case 2://----监事会
                        source = WebDataSource.SF9_TBOSI;
                        _sorts.Add(new Order("STR_PLEVEL", true));
                        _sorts.Add(new Order("POSITION", false));
                        break;
                    case 3://----高级管理人员
                        source = WebDataSource.SF9_SMPC;
                        _sorts.Add(new Order("STR_PLEVEL", true));
                        _sorts.Add(new Order("POSITION", false));
                        break;
                    case 4://----历任管理层成员
                        source = WebDataSource.SF9_AAMOM;
                        _sorts.Add(new Order("POSITIONSTARTDATE", false));
                        break;
                }

                String[] _fileds = new String[] { };
                DateTime dt1 = DateTime.Now;
                _DS = DataAccess.Query(source, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                LogHelper.WriteDebugLog("getManagerInfo获取数据时间:" + (DateTime.Now - dt1));
                //MongoCursor<BsonDocument> pData = DataAccess.MyQuery(DataSource.SPE_COUNTINAREA, _querys, _sorts, fileds, 1, 24);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getManagerInfo方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 管理层持股及报酬
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet ManagementRemuneration(String securityCode, String order, String yearList,
            String reportTypeList, String dateSearchType)
        {
            DataSet _DS = new DataSet();

            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECUCODE", securityCode));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "ENDDATE"));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                bool ascending = order == "asc";
                _sort.Add(new Order("ENDDATE", ascending));
                _sort.Add(new Order("ANUALWAGE", false));

                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_MOAC, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getManagementRemunation方法异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 管理层持股变化
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet ManagementStockChange(String securityCode, String order, String yearList,
            String reportTypeList, String dateSearchType)
        {
            DataSet _DS = new DataSet();

            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECUCODE", securityCode));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "CHANGEDATE"));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("CHANGEDATE", order == "asc"));

                //String[] _field = new String[] { "CHANGEDATE", "SHAREHOLDER", "CHANNUM", "AFCHANGENUM", "AVPRICE", "CHANGECAUSE", "PNAME", "DUTY", "STR_RELATIONMAN", "STR_SFDXJY"};
                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_MOC, _field, _query, _sort, _selector, 1, int.MaxValue);
                //_DS = CommonController.FilterDataByDateRange(_DS, "CHANGEDATE", yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getManagementStockChange方法异常" + ex.ToString());
            }
            return null;
        }

        private const String STR_EMPTY = "-";

        /// <summary>
        ///  解析当前股票对应的证券类型 
        /// </summary>
        /// <param name=CommDao.SafeToString("dataSet")></param>
        /// <returns></returns>
        /// <summary>
        internal static String GetStockIndustryType(DataSet dataSet, String code)
        {
            String industry = null;
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow _row = dataSet.Tables[0].Rows[0];
                industry = CommDao.SafeToString(_row[code], STR_EMPTY);
            }
            return industry;
        }

        /// <summary>
        /// 市场表现比较
        /// </summary>
        /// <param name="securityCode">股票代码</param>
        /// <param name="industry">获取行业类型的代码(1为申万一级，2为申万二级(默认)，3为申万三级)</param>
        /// <param name="option">显示的条数(1为前20条(默认)，2为全部)</param>
        /// <returns></returns>
        public static DataSet GetStockMarketExpressList(String securityCode, int industry, int option)
        {
            DataSet _DS = new DataSet();
            String code = "";
            String industryType;
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                String[] _field = new String[] { };
                //请求数据获取股票代码的行业类型(申万一级，申万二级，申万三级)
                _DS = DataAccess.Query(WebDataSource.IND_DAILYPRICE, _field, _query, _sorts, _selector, 1, int.MaxValue);
                switch (industry)
                {
                    case 1:
                        code = "STR_PUBLISHCODESWYJDM";
                        break;
                    case 2:
                        code = "STR_PUBLISHCODESWEJDM";
                        break;
                    case 3:
                        code = "STR_PUBLISHCODESWSJDM";
                        break;
                    default:
                        code = "STR_PUBLISHCODESWEJDM";
                        break;
                }
                industryType = GetStockIndustryType(_DS, code);
                //把请求来的行业类型放入query中
                List<Expression> _query1 = new List<Expression>();
                _query1.Add(Expression.Eq(code, industryType));
                _DS.Clear();
                List<Expression> _selector1 = new List<Expression>();
                _sorts.Add(new Order("TDATE", false));
                _sorts.Add(new Order("CHG", false));
                //查询行业类型的所有股票信息
                _DS = DataAccess.Query(WebDataSource.IND_DAILYPRICE, _field, _query1, _sorts, _selector1, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetStockMarketExpressList方法异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 证券简介
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet SecurityIntroduction(String securityCode)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("DAT_GGRI", false));
                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_SECURITY, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getSecurityIntroduction方法异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 特别处理和退市
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet SpecialProcessDelistedOpen(String securityCode)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("DAT_GGRI", false));

                String[] _field = new String[] { "SECURITYCODE", "DAT_GGRI", "STR_SSFS", "STR_SSYY" };
                _DS = DataAccess.Query(WebDataSource.SF9_SECURITY, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SpecialProcessDelistedOpen方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 所属行业
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet GetIndustryInfo(String securityCode, int industryType)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                if (industryType == 2)
                {
                    _query.Add(Expression.Like("STR_ZT", "最新%"));
                }
                else if (industryType == 3)
                {
                    _query.Add(Expression.Like("STR_ZT", "历史%"));
                }
                List<Expression> _selector = new List<Expression>();
                List<Order> _sort = new List<Order>();
                _sort.Add(new Order("STR_HY", false));
                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_INDUSTRY, _field, _query, _sort, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getIndustryList方法异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 证券所属指数
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet GetSecurityIndex(String securityCode)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("OPDATE", false));

                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_THEINDEX, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetSecurityIndex方法异常" + ex.ToString());
            }
            return null; 
        }

        /// <summary>
        /// 证券所属概念板块
        /// </summary>
        /// <param name="securityCode">证券代码</param>
        /// <returns></returns>
        public static DataSet GetConceptBoardList(String securityCode)
        {
            DataSet _DS = new DataSet();

            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("DAT_JRRQ", false));

                String[] _field = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_TCOP, _field, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetConceptBoardList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 阶段行情数据统计
        /// </summary>
        /// <param name="securityCode">股票代码</param>
        /// <param name="rotate">旋转(1为旋转，0为不旋转)</param>
        /// <param name="order">排序(值为"desc"or"asc")</param>
        /// <returns></returns>
        public static DataSet GetStageMarketDataList(String securityCode, String order)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("STR_STOCKCODE", securityCode));
                //_query.Add(Expression.Gt("DAT_TDAY", DateTime.Now.AddMonths(-3)));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                //_sorts.Add(new Order("DAT_TDAY", order == "asc"));

                String[] _fields = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_INTVLQUOTESTAT, _fields, _query, _sorts, _selector, 1, int.MaxValue);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetRiskAndIncomeAnalysisList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 风险与收益分析/*$-rpt\r\n$name=EM_AB_RISK\r\n$secucode=000001.SZ\r\n$StartDate=2012-01-01,EndDate=2014-07-15\r\n*/
        /// </summary>
        /// <param name="securityCode">股票代码</param>
        /// <param name="rotate">旋转(0为旋转，1为不旋转)</param>
        /// <param name="order">排序(值为"desc"or"asc")</param>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "CacheChangeBySixHours")]
        public static DataSet GetRiskAndIncomeAnalysisList(String securityCode, int rotate, String order, String yearList,
            String reportTypeList, String dateSearchType)
        {
            DataSet DS = new DataSet();
            try
            {
                Dictionary<String, String> paramDic = CommonController.GetDateRangeDic(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));
                StockIndicateParam param = new StockIndicateParam();
                param.Name = "EM_AB_RISK";
                param.Secucode = securityCode;
                param.Dic = paramDic;
                DS = DataAccess.QueryStockIndicate(param.ToString());
                return DS;
                //LogHelper.WriteLog("GetDailyMarketDataListForReport方法请求时间:" + span.Seconds.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetRiskAndIncomeAnalysisList方法异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 交易异动成交营业部
        /// </summary>
        /// <param name="securityCode">股票代码</param>
        /// <param name="order">排序(值为"desc"or"asc")</param>
        /// <returns></returns>
        public static DataSet GetBargainOfficeList(String securityCode, String order, String yearList,
            String reportTypeList, String dateSearchType)
        {
            DataSet _DS = new DataSet();

            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SCODE", securityCode));
                //_query.Add(Expression.Gt("TDATE", DateTime.Now.AddYears(-3)));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "TDATE"));
                //int count = CommonController.GetDateRangeQueryCount(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("TDATE", order == "asc"));

                String[] _fields = new String[] { };
                _DS = DataAccess.Query(WebDataSource.SF9_JIAOYIYIDONG, _fields, _query, _sorts, _selector, 1, Int16.MaxValue);
                //_DS = CommonController.FilterDataByDateRange(_DS, "TDATE", yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBargainOfficeList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 资金流向--图表
        /// 资金流向主力构成
        /// $-rpt\r\n$name=EM_AB_MAINFUNDS\r\n$secucode=000001.SZ\r\n$StartDate=2013-01-01,EndDate=2014-07-15\r\n
        /// 资金流向阶段统计
        /// $-rpt\r\n$name=EM_AB_FUNDSDURINGSCOPE\r\n$secucode=000001.SZ\r\n
        /// 资金流向分类资金
        /// $-rpt\r\n$name=EM_AB_CLASSFYFUNDS\r\n$secucode=000001.SZ\r\n$StartDate=2013-01-01,EndDate=2014-07-15\r\n
        /// </summary>
        /// <param name="securityCode">股票代码</param>
        /// <param name="order">排序(值为"desc"or"asc")</param>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "CacheChangeBySixHours")]
        public static DataSet GetFundFlowListForChart(String securityCode)
        {
            object result = new object();
            //主力资金
            List<object> mainFundList = new List<object>();
            //阶段统计
            List<object> stageList = new List<object>();
            DataSet Main_DS = new DataSet();
            DataSet Classify_DS = new DataSet();
            DataSet _DS = new DataSet();

            try
            {
                //主力
                StockIndicateParam main_param = new StockIndicateParam();
                main_param.Name = "EM_AB_MAINFUNDS";
                main_param.Secucode = securityCode;
                Main_DS = DataAccess.QueryStockIndicate(main_param.ToString());
                //分类
                StockIndicateParam classify_param = new StockIndicateParam();
                classify_param.Name = "EM_AB_CLASSFYFUNDS";
                classify_param.Secucode = securityCode;
                Classify_DS = DataAccess.QueryStockIndicate(classify_param.ToString());
                //阶段
                StockIndicateParam param = new StockIndicateParam();
                param.Name = "EM_AB_FUNDSDURINGSCOPE";
                param.Secucode = securityCode;
                _DS = DataAccess.QueryStockIndicate(param.ToString());
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetFundFlowList方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 融资融券
        /// </summary>
        /// <param name="securityCode">股票代码</param>
        /// <param name="order">排序(值为"desc"or"asc")</param>
        /// <returns></returns>
        public static DataSet GetSecurityFinancingList(String securityCode, String yearList,
            String reportTypeList, String dateSearchType, String cycleType)
        {
            List<object> securityList = new List<object>();
            int count = 1;
            try
            {
                //reportTypeList = "1,3,5,6";
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECURITYCODE", securityCode));
                _query.Add(Expression.Eq("STR_QUJIAN", cycleType));
                //_query.Add(Expression.Gt("TDATE", DateTime.Now.AddYears(-3)));
                _query.AddRange(CommonController.GetDateRangeExpression(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType), "TDATE"));
                //int count = CommonController.GetDateRangeQueryCount(yearList, reportTypeList, CommDao.SafeToInt(dateSearchType));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("TDATE", false));

                String[] _fields = new String[] { };
                ResultData _RS = DataAccess.QueryByStatistic(WebDataSource.SF9_MARGINTRADING1, _fields, _query, _sorts, _selector, 1, Int16.MaxValue, WebDataSource.SF9_MARGINTRADING1.ToString());
                return _RS.Data;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetSecurityFinancingList方法异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 运营能力
        /// </summary>
        /// <returns></returns>
        public static DataSet OperationAbility(String securityCode)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("REPORTDATE", false));

                String[] _fileds = new String[] {
                    "REPORTDATE","DEC_BENYINGYEZHOUQI","DEC_BENCUNZHOUTIAN","DEC_BENZHANGZHOU","DEC_BENCUNZHOULV","DEC_BENZHANGLV","DEC_LDZCZZL","DEC_BENGUZILV","DEC_BENZONGZILV"
                };
                _DS = DataAccess.Query(WebDataSource.IND_OCI, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);

                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Lab方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 盈利能力与收益质量
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet ProfitAbility(String securityCode)
        {
            List<DataTransmission> data = new List<DataTransmission>();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query_IND_POTY = new List<Expression>();
                _query_IND_POTY.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_PAROE = new List<Expression>();
                _query_IND_PAROE.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_SPA = new List<Expression>();
                _query_IND_SPA.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_QOE = new List<Expression>();
                _query_IND_QOE.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_TPOA = new List<Expression>();
                _query_IND_QOE.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts_IND_POTY = new List<Order>();
                List<Order> _sorts_IND_PAROE = new List<Order>();
                List<Order> _sorts_IND_SPA = new List<Order>();
                List<Order> _sorts_IND_QOE = new List<Order>();
                List<Order> _sorts_IND_TPOA = new List<Order>();
                _sorts_IND_POTY.Add(new Order("REPORTDATE", false));
                _sorts_IND_PAROE.Add(new Order("REPORTDATE", false));
                _sorts_IND_SPA.Add(new Order("REPORTDATE", false));
                _sorts_IND_QOE.Add(new Order("REPORTDATE", false));
                _sorts_IND_TPOA.Add(new Order("REPORTDATE", false));
                String[] _field_IND_POTY = new String[] { "REPORTDATE", "DEC_JINGSHOU", "DEC_ZONGBAO", "DEC_ROA", "DEC_KC_JB", "DEC_MGJZCBPS", "NETOPERATECASHFLOWPS", "DEC_QMGBTB", "DEC_MGYYZSR", "DEC_MGYYSR", "DEC_MGXSQLR", "DEC_MGZBGJ", "DEC_MGYYGJ", "DEC_MGWFPLR", "DEC_MGYYLC", "DEC_MGXJLLJE" };
                String[] _field_IND_PAROE = new String[] { "REPORTDATE", "ROEFULLYDILUTED", "ROEWEIGHTED", "DEC_JZCSYLROEP", "DEC_JZCSYL_KC_TB", "DEC_JZCSYL_KC_JQ", "DEC_JZCSYL_KCFJCXSY", "DEC_ROLC" };
                String[] _field_IND_SPA = new String[] { "REPORTDATE", "DEC_XSJLR", "DEC_XSMLL", "DEC_XSCBL", "DEC_XSQJFYL", "DEC_JLR_YYZSR", "DEC_YYLR_YYZSR", "DEC_XSQLR_YYZSR", "DEC_YYZCB_YYZSR", "DEC_YYFY_YYZSR", "DEC_GLFY_YYZSR", "DEC_CWFY_YYZSR", "DEC_ZCJZSS_YYZSR", "DEC_MGWFPLR" };
                String[] _field_IND_QOE = new String[] { "REPORTDATE", "DEC_JYHDJSY_LRZE", "DEC_JZBDJSY_LRZE", "DEC_YYWSZJE_LRZE", "DEC_SDSL_LRZE", "DEC_KFMGSJLR_GSMGSJLR" };
                String[] _field_IND_TPOA = new String[] { "REPORTDATE", "DEC_ZZCBCL", "DEC_ZZCJLL" };
                data.Add(DataAccess.GetData(WebDataSource.IND_POTY, _field_IND_POTY, _query_IND_POTY, _sorts_IND_POTY, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_PAROE, _field_IND_PAROE, _query_IND_PAROE, _sorts_IND_PAROE, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_SPA, _field_IND_SPA, _query_IND_SPA, _sorts_IND_SPA, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_QOE, _field_IND_QOE, _query_IND_QOE, _sorts_IND_QOE, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_TPOA, _field_IND_TPOA, _query_IND_TPOA, _sorts_IND_TPOA, _selector, 1, 8, false, ""));

                _DS = DataAccess.Query(data);
                _DS.Tables["IND_POTY"].Columns.Add("ROEFULLYDILUTED");
                _DS.Tables["IND_POTY"].Columns.Add("ROEWEIGHTED");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JZCSYLROEP");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JZCSYL_KC_TB");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JZCSYL_KC_JQ");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JZCSYL_KCFJCXSY");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_ROLC");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_XSJLR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_XSMLL");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_XSCBL");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_XSQJFYL");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JLR_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_YYLR_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_XSQLR_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_YYZCB_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_YYFY_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_GLFY_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_CWFY_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_ZCJZSS_YYZSR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_MGWFPLR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JYHDJSY_LRZE");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_JZBDJSY_LRZE");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_YYWSZJE_LRZE");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_SDSL_LRZE");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_KFMGSJLR_GSMGSJLR");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_ZZCBCL");
                _DS.Tables["IND_POTY"].Columns.Add("DEC_ZZCJLL");
                foreach (DataRow dr in _DS.Tables["IND_POTY"].Rows)
                {
                    for (int i = 0; i < _DS.Tables["IND_PAROE"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_PAROE"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["ROEFULLYDILUTED"] = _DS.Tables["IND_PAROE"].Rows[i]["ROEFULLYDILUTED"];
                            dr["ROEWEIGHTED"] = _DS.Tables["IND_PAROE"].Rows[i]["ROEWEIGHTED"];
                            dr["DEC_JZCSYLROEP"] = _DS.Tables["IND_PAROE"].Rows[i]["DEC_JZCSYLROEP"];
                            dr["DEC_JZCSYL_KC_TB"] = _DS.Tables["IND_PAROE"].Rows[i]["DEC_JZCSYL_KC_TB"];
                            dr["DEC_JZCSYL_KC_JQ"] = _DS.Tables["IND_PAROE"].Rows[i]["DEC_JZCSYL_KC_JQ"];
                            dr["DEC_JZCSYL_KCFJCXSY"] = _DS.Tables["IND_PAROE"].Rows[i]["DEC_JZCSYL_KCFJCXSY"];
                            dr["DEC_ROLC"] = _DS.Tables["IND_PAROE"].Rows[i]["DEC_ROLC"];
                            break;
                        }
                    }
                    for (int i = 0; i < _DS.Tables["IND_SPA"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_SPA"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["DEC_XSJLR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_XSJLR"];
                            dr["DEC_XSMLL"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_XSMLL"];
                            dr["DEC_XSCBL"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_XSCBL"];
                            dr["DEC_XSQJFYL"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_XSQJFYL"];
                            dr["DEC_JLR_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_JLR_YYZSR"];
                            dr["DEC_YYLR_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_YYLR_YYZSR"];
                            dr["DEC_XSQLR_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_XSQLR_YYZSR"];
                            dr["DEC_YYZCB_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_YYZCB_YYZSR"];
                            dr["DEC_YYFY_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_YYFY_YYZSR"];
                            dr["DEC_GLFY_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_GLFY_YYZSR"];
                            dr["DEC_CWFY_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_CWFY_YYZSR"];

                            dr["DEC_ZCJZSS_YYZSR"] = _DS.Tables["IND_SPA"].Rows[i]["DEC_ZCJZSS_YYZSR"];

                            break;
                        }
                    }
                    for (int i = 0; i < _DS.Tables["IND_QOE"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_QOE"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["DEC_JYHDJSY_LRZE"] = _DS.Tables["IND_QOE"].Rows[i]["DEC_JYHDJSY_LRZE"];
                            dr["DEC_JZBDJSY_LRZE"] = _DS.Tables["IND_QOE"].Rows[i]["DEC_JZBDJSY_LRZE"];
                            dr["DEC_SDSL_LRZE"] = _DS.Tables["IND_QOE"].Rows[i]["DEC_SDSL_LRZE"];
                            dr["DEC_KFMGSJLR_GSMGSJLR"] = _DS.Tables["IND_QOE"].Rows[i]["DEC_KFMGSJLR_GSMGSJLR"];
                            break;
                        }
                    }
                    for (int i = 0; i < _DS.Tables["IND_TPOA"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_TPOA"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["DEC_ZZCJLL"] = _DS.Tables["IND_TPOA"].Rows[i]["DEC_ZZCJLL"];
                            dr["DEC_ZZCBCL"] = _DS.Tables["IND_TPOA"].Rows[i]["DEC_ZZCBCL"];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetConceptBoardList方法异常" + ex.ToString());
            }
            return _DS;
        }

        /// <summary>
        /// 资本结构与偿债能力
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet CapitalStructure(String securityCode)
        {
            List<DataTransmission> data = new List<DataTransmission>();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query_IND_CAPITALS = new List<Expression>();
                _query_IND_CAPITALS.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_DPA = new List<Expression>();
                _query_IND_DPA.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_DPAADD = new List<Expression>();
                _query_IND_DPAADD.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_BUSINESSS = new List<Expression>();
                _query_IND_BUSINESSS.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts_IND_CAPITALS = new List<Order>();
                List<Order> _sorts_IND_DPA = new List<Order>();
                List<Order> _sorts_IND_DPAADD = new List<Order>();
                List<Order> _sorts_IND_BUSINESSS = new List<Order>();
                _sorts_IND_CAPITALS.Add(new Order("REPORTDATE", false));
                _sorts_IND_DPA.Add(new Order("REPORTDATE", false));
                _sorts_IND_DPAADD.Add(new Order("REPORTDATE", false));
                _sorts_IND_BUSINESSS.Add(new Order("REPORTDATE", false));
                String[] _field_IND_CAPITALS = new String[] { "REPORTDATE", "DEC_ZCFZL", "DEC_QYCS", "DEC_LDZC_ZCHJ", "DEC_FLDZC_ZCHJ", "DEC_YXZC_ZZC", "DEC_GSMGSQY_QBTRZB", "DEC_DXZW_QBTRZB", "DEC_LDFZ_FZHJ", "DEC_FLDFZ_FZHJ" };
                String[] _field_IND_DPA = new String[] { "REPORTDATE", "DEC_LDBL", "DEC_SDBL", "DEC_BSSDBL", "DEC_CQBL", "DEC_GSMGSGDQY_FZHJ", "DEC_GSMGSGDQY_DXZW", "DEC_YXZC_FZHJ", "DEC_YXZC_DXZW", "DEC_YXZC_JZW" };
                String[] _field_IND_DPAADD = new String[] { "REPORTDATE", "DEC_XISHUI_FUZHAI", "DEC_YIHUOLIXI", "DEC_EBITDA_DXZW" };
                String[] _field_IND_BUSINESSS = new String[] { "REPORTDATE", "DEC_JYHDCSDXJLLJE_FZHJ", "DEC_JYHDCSDXJLLJE_DXZW", "DEC_JYHDCSDXJLLJE_LDFZ", "DEC_JYHDCSDXJLLJE_JZW" };
                data.Add(DataAccess.GetData(WebDataSource.IND_CAPITALS, _field_IND_CAPITALS, _query_IND_CAPITALS, _sorts_IND_CAPITALS, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_DPA, _field_IND_DPA, _query_IND_DPA, _sorts_IND_DPA, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_DPAADD, _field_IND_DPAADD, _query_IND_DPAADD, _sorts_IND_DPAADD, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_BUSINESSS, _field_IND_BUSINESSS, _query_IND_BUSINESSS, _sorts_IND_BUSINESSS, _selector, 1, 8, false, ""));

                _DS = DataAccess.Query(data);
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_LDBL");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_SDBL");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_BSSDBL");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_CQBL");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_GSMGSGDQY_FZHJ");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_GSMGSGDQY_DXZW");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_YXZC_FZHJ");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_YXZC_DXZW");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_YXZC_JZW");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_XISHUI_FUZHAI");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_JYHDCSDXJLLJE_FZHJ");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_JYHDCSDXJLLJE_DXZW");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_JYHDCSDXJLLJE_LDFZ");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_JYHDCSDXJLLJE_JZW");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_YIHUOLIXI");
                _DS.Tables["IND_CAPITALS"].Columns.Add("DEC_EBITDA_DXZW");
                foreach (DataRow dr in _DS.Tables["IND_CAPITALS"].Rows)
                {
                    for (int i = 0; i < _DS.Tables["IND_DPA"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_DPA"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["DEC_LDBL"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_LDBL"];
                            dr["DEC_SDBL"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_SDBL"];
                            dr["DEC_BSSDBL"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_BSSDBL"];
                            dr["DEC_CQBL"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_CQBL"];
                            dr["DEC_GSMGSGDQY_FZHJ"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_GSMGSGDQY_FZHJ"];
                            dr["DEC_GSMGSGDQY_DXZW"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_GSMGSGDQY_DXZW"];
                            dr["DEC_YXZC_FZHJ"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_YXZC_FZHJ"];
                            dr["DEC_YXZC_DXZW"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_YXZC_DXZW"];
                            dr["DEC_YXZC_JZW"] = _DS.Tables["IND_DPA"].Rows[i]["DEC_YXZC_JZW"];
                            break;
                        }
                    }
                    for (int i = 0; i < _DS.Tables["IND_DPAADD"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_DPAADD"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["DEC_XISHUI_FUZHAI"] = _DS.Tables["IND_DPAADD"].Rows[i]["DEC_XISHUI_FUZHAI"];
                            dr["DEC_YIHUOLIXI"] = _DS.Tables["IND_DPAADD"].Rows[i]["DEC_YIHUOLIXI"];
                            dr["DEC_EBITDA_DXZW"] = _DS.Tables["IND_DPAADD"].Rows[i]["DEC_EBITDA_DXZW"];


                            break;
                        }
                    }
                    for (int i = 0; i < _DS.Tables["IND_BUSINESSS"].Rows.Count; i++)
                    {
                        if (dr["REPORTDATE"].ToString() == _DS.Tables["IND_BUSINESSS"].Rows[i]["REPORTDATE"].ToString())
                        {
                            dr["DEC_JYHDCSDXJLLJE_FZHJ"] = _DS.Tables["IND_BUSINESSS"].Rows[i]["DEC_JYHDCSDXJLLJE_FZHJ"];
                            dr["DEC_JYHDCSDXJLLJE_DXZW"] = _DS.Tables["IND_BUSINESSS"].Rows[i]["DEC_JYHDCSDXJLLJE_DXZW"];
                            dr["DEC_JYHDCSDXJLLJE_LDFZ"] = _DS.Tables["IND_BUSINESSS"].Rows[i]["DEC_JYHDCSDXJLLJE_LDFZ"];
                            dr["DEC_JYHDCSDXJLLJE_JZW"] = _DS.Tables["IND_BUSINESSS"].Rows[i]["DEC_JYHDCSDXJLLJE_JZW"];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetConceptBoardList方法异常" + ex.ToString());
            }
            return _DS;
        }

        /// <summary>
        /// 单季度财务指标
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet SingleFinanceIndex(String securityCode)
        {
            List<DataTransmission> data = new List<DataTransmission>();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query_IND_QUARTERLYI = new List<Expression>();
                _query_IND_QUARTERLYI.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_ASQPFI = new List<Expression>();
                _query_IND_ASQPFI.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_QFIR = new List<Expression>();
                _query_IND_QFIR.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _query_IND_ASQYOYGOFI = new List<Expression>();
                _query_IND_ASQYOYGOFI.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts_IND_QUARTERLYI = new List<Order>();
                List<Order> _sorts_IND_ASQPFI = new List<Order>();
                List<Order> _sorts_IND_QFIR = new List<Order>();
                List<Order> _sorts_IND_ASQYOYGOFI = new List<Order>();
                _sorts_IND_QUARTERLYI.Add(new Order("REPORTDATE", false));
                _sorts_IND_ASQPFI.Add(new Order("REPORTDATE", false));
                _sorts_IND_QFIR.Add(new Order("REPORTDATE", false));
                _sorts_IND_ASQYOYGOFI.Add(new Order("REPORTDATE", false));
                String[] _field_IND_QUARTERLYI = new String[] { "REPORTDATE", "DEC_DJDMGSY", "DEC_DJDKFJLR", "DEC_DJDJZCSYLREO", "DEC_DJDKFJZCSYL", "DEC_DJDKFJLR_GSMGSJLR", "DEC_DJDSSSP_YYSR", "DEC_DJDJYHDJE_YYSR", "DEC_DJDJYHDJE_JYHDJSY" };
                String[] _field_IND_ASQPFI = new String[] { "REPORTDATE", "DEC_DJDJYHDJSY", "DEC_DJDJZBDJSY", "DEC_ZCCJLVROA", "DEC_DJDXSJLR", "DEC_DJDXSMLL", "DEC_DJDYYZCB_YYZSR", "DEC_DJDYYLR_YYZSR", "DEC_JLR_YYZSR", "DEC_YYFY_YYZSR", "DEC_GLFY_YYZSR", "DEC_DJDCWFY_YYZSR", "DEC_DJDJYHDJSY_LRZE", "DEC_DJDJZBDJSY_LRZE" };
                String[] _field_IND_QFIR = new String[] { "REPORTDATE", "DEC_YYZSRHBZZ", "DEC_YYSRHBZZ", "DEC_YYLRHBZZ", "DEC_JLRHBZZ", "DEC_GSMGSJLRHBZZ" };
                String[] _field_IND_ASQYOYGOFI = new String[] { "REPORTDATE", "DEC_DJDYYZSRTBZZ", "DEC_DJDYYSRTBZZ", "DEC_DJDYYLRTBZZ", "DEC_DJDJLRTBZZ", "DEC_DJDGSMGSJLRTBZZ" };
                data.Add(DataAccess.GetData(WebDataSource.IND_QUARTERLYI, _field_IND_QUARTERLYI, _query_IND_QUARTERLYI, _sorts_IND_QUARTERLYI, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_ASQPFI, _field_IND_ASQPFI, _query_IND_ASQPFI, _sorts_IND_ASQPFI, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_QFIR, _field_IND_QFIR, _query_IND_QFIR, _sorts_IND_QFIR, _selector, 1, 8, false, ""));
                data.Add(DataAccess.GetData(WebDataSource.IND_ASQYOYGOFI, _field_IND_ASQYOYGOFI, _query_IND_ASQYOYGOFI, _sorts_IND_ASQYOYGOFI, _selector, 1, 8, false, ""));

                _DS = DataAccess.Query(data);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetConceptBoardList方法异常" + ex.ToString());
            }
            return _DS;
        }

        /// <summary>
        /// 新股发行 发行可转债 募集资金投向
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet TransferDebt(String securityCode)
        {
            List<DataTransmission> data = new List<DataTransmission>();
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _query = new List<Expression>();
                _query.Add(Expression.Eq("SECUCODE", securityCode));

                List<Expression> _selector = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("REPORTDATE", false));

                String[] _field = new String[] { "STR_FAJD", "DEC_FXGM", "DEC_FXSL", "BONDPERIOD", "NEWRATE", "COUPONTYPE", "RATEDES", "PAYPERYEAR", "PAYTYPE", "FRSTVALUEDATE", "MRTYDATE", "LISTDATE", "DELISTDATE", "DAT_HUISHOURI", "DAT_SHENQQISR", "ENDATE" 
                ,"DEC_HSJGMZBI","SWAPPRICE","ISSUEDATE","AVALIPUB","DEC_XYGZGDPSBL","DEC_XYGZGDPSAP","DAT_YUANPEISRI","APLACVALHOID","DAT_GQDJR","DAT_YUANJIAOKUANRI","SIR_CSZGJSM","SPCHGSTD","STR_ZHQSM","STR_QJYTJHSSM","REVISECLAUSE","STR_ZHUCXS","STR_TUIJIANREN","ISSUEMETHOD","ISSUEFEE","PARVALUE","DAT_FXJGGGRI","STR_FENXIAOSHANG","BUYVALIPUB","DEC_YXSGHS","LUCKRATE"
                ,"DEC_LC","INBUYNUMPUB","DAT_YAGGR","DAT_GDDHGGR","PUBLISHDATE","DAT_SSGGR"};
                data.Add(DataAccess.GetData(WebDataSource.SP9_FAXINGKEZHUANZHAI, _field, _query, _sorts, _selector, 1, 8, false, ""));

                _DS = DataAccess.Query(data);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetConceptBoardList方法异常" + ex.ToString());
            }
            return _DS;
        }

        /// <summary>
        /// 财务费用明细
        /// </summary>
        /// <returns></returns>
        public static DataSet FinanceDetails(String securityCode)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("REPORTDATE", false));

                String[] _fileds = new String[] {
                    "REPORTDATE","COMBINETYPE","DEC_LXZC","DEC_LXSR","DEC_HDSY","DEC_SXF","DEC_QT","DEC_HEJI"
                };
                _DS = DataAccess.Query(WebDataSource.IND_FINANCECOSTS, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                //MongoCursor<BsonDocument> pData = DataAccess.MyQuery(DataSource.SPE_COUNTINAREA, _querys, _sorts, fileds, 1, 24);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Lab方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 担保金额
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet GuaranteeCash(String securityCode)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("STR_BAOGAOQI", false));

                String[] _fileds = new String[] {
                    "STR_BAOGAOQI","AAMTGUARANTEE","DEC_DBJEHJ","DEC_GLDBJEHJ","AAMTSUBSIDIARY","AMTILLEGAL","AAMTTONA","DEC_HEJI"
                };
                _DS = DataAccess.Query(WebDataSource.IND_GUARANTEEDATA, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                //MongoCursor<BsonDocument> pData = DataAccess.MyQuery(DataSource.SPE_COUNTINAREA, _querys, _sorts, fileds, 1, 24);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCountInArea方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 存货明细
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static DataSet StockDetails(String securityCode)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("SECURITYCODE", securityCode));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("REPORTDATE", false));

                String[] _fileds = new String[] {
                    "REPORTDATE","ACTUALITEM","BOOKAMTEND","FPREND"
                };
                _DS = DataAccess.Query(WebDataSource.IF9_INSTOCKDETAIL, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                //MongoCursor<BsonDocument> pData = DataAccess.MyQuery(DataSource.SPE_COUNTINAREA, _querys, _sorts, fileds, 1, 24);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCountInArea方法异常" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 以下分别是 交易异动成交营业部
        /// </summary>
        /// <returns></returns>
        public static DataSet BargainExchange(String securityCode)
        {
            DataSet _DS = new DataSet();
            try
            {
                List<Expression> _querys = new List<Expression>();
                _querys.Add(Expression.Eq("SCODE", securityCode));
                List<Expression> _selectors = new List<Expression>();
                List<Order> _sorts = new List<Order>();
                _sorts.Add(new Order("TDATE", false));

                String[] _fileds = new String[] {
                    "CTYPE","CSDATE","CEDATE","STADATE",
                    "SALESNAME","BMONEY","SMONEY","TVAL","DEC_YYBMRZB",
                    "DEC_YYBMCZB","DEC_YYBJYZB","CHGRADIO","TDATE"
                };
                _DS = DataAccess.Query(WebDataSource.SF9_TTBD, _fileds, _querys, _sorts, _selectors, 1, int.MaxValue);
                //MongoCursor<BsonDocument> pData = DataAccess.MyQuery(DataSource.SPE_COUNTINAREA, _querys, _sorts, fileds, 1, 24);
                return _DS;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCountInArea方法异常" + ex.ToString());
            }
            return _DS;
        }
    }
}
