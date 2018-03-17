using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using EmCore;
using EmSerDataService;
using System.Net;
using System.IO;

namespace OwLib
{
    public class DataAccess
    {
        private static DataQuery _IDataQuery;
        private static Object dataqueryobject=new object();
        public static DataQuery IDataQuery
        {
            get
            {
                _IDataQuery = DataQuery.CreateNewInstance();
                return _IDataQuery;
            }
        }
        
        /// <summary>
        /// 通道的获取数据的方法
        /// </summary>
        /// <param name="dataSource">数据源id</param>
        /// <param name="fileds">选择列</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序条件</param>
        /// <param name="selectors">选择条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <returns></returns>
        public static DataSet Query(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize)
        {
            try
            {
                DataTransmission _data = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, false, "");
                List<DataTransmission> _dataList = new List<DataTransmission>();
                _dataList.Add(_data);
                DataSet _DS = Query(_dataList);
                return _DS;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("源名：" + dataSource.ToString() + "，Query方法异常：", ex);
                throw new Exception(dataSource.ToString() + ex.ToString());
                //throw ex;
            }
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="dataSource">数据源id</param>
        /// <param name="fileds">选择列</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序条件</param>
        /// <param name="selectors">选择条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <param name="statisticsEngName">统计的名称</param>
        /// <returns></returns>
        public static ResultData QueryByStatistic(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, String statisticsEngName)
        {
            try
            {
                DataTransmission _data = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, false, statisticsEngName);
                ResultData _DS = QueryStatics(_data);
                return _DS;
            }
            catch (Exception ex)
            {
                throw new Exception(dataSource.ToString() + ex.ToString());
            }
        }

        /// <summary>
        /// 调用统计
        /// </summary>
        /// <param name="dataSource">数据源id</param>
        /// <param name="fileds">选择列</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序条件</param>
        /// <param name="selectors">选择条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <returns></returns>
        public static ResultData QueryByStatistic(String dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize)
        {
            try
            {
                DataTransmission _data = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, false, dataSource);
                ResultData _DS = QueryStatics(_data);
                return _DS;
            }
            catch (Exception ex)
            {
                throw new Exception(dataSource + ex.ToString());
            }
        }

        /// <summary>
        /// 统计的分页方法
        /// </summary>
        /// <param name="dataSource">统计的名称</param>
        /// <param name="fileds">选择列</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序条件</param>
        /// <param name="selectors">选择条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <param name="statisticsEngName">统计的名称</param>
        /// <returns></returns>
        public static PagingData QueryByStatisticForPaging(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, String statisticsEngName)
        {
            try
            {
                DataTransmission _data = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, true, statisticsEngName);
                PagingData _DS = QueryStaticsForPaging(_data);
                return _DS;
            }
            catch (Exception ex)
            {
                String errMsg = String.Empty;
                errMsg += "dataSource：" + dataSource.ToString();
                errMsg += ",fileds：";
                for (int i = 0; i < fileds.Length; i++)
                {
                    errMsg += fileds[i].ToString() + "||";
                }
                for (int i = 0; i < filters.Count; i++)
                {
                    errMsg += filters[i].GetString + "||";
                }
                errMsg += ",orders：";
                for (int i = 0; i < orders.Count; i++)
                {
                    errMsg += orders[i].ToQueryString() + "||";
                }
                errMsg += ",page：" + page;
                errMsg += ",pageSize：" + pageSize;
                LogHelper.WriteLog(errMsg + "，QueryByStatisticForPaging方法异常：", ex);
                throw new Exception(dataSource.ToString() + ex.ToString());
                //throw ex;
            }
        }

        /// <summary>
        /// 通过出入多个条件 一次获取多个源的数据
        /// </summary>
        /// <param name="DTList"></param>
        /// <returns></returns>
        public static DataSet Query(List<DataTransmission> DTList)
        {
            try
            {
                DataSet _DS = IDataQuery.Query(DTList);
                return _DS;
            }
            catch (Exception ex)
            {
                String errMsg = String.Empty;
                foreach (DataTransmission DT in DTList)
                {
                    errMsg += "dataSource：" + DT.DataSource.ToString();
                    errMsg += ",fileds：";
                    for (int i = 0; i < DT.Fields.Length; i++)
                    {
                        errMsg += DT.Fields[i].ToString() + "||";
                    }
                    for (int i = 0; i < DT.Filters.Count; i++)
                    {
                        errMsg += DT.Filters[i].GetString + "||";
                    }
                    errMsg += ",orders：";
                    for (int i = 0; i < DT.Orders.Count; i++)
                    {
                        errMsg += DT.Orders[i].ToQueryString() + "||";
                    }
                    errMsg += ",FirstResult：" + DT.FirstResult;
                    errMsg += ",MaxResult：" + DT.MaxResult+"\r\n";
                }

                LogHelper.WriteLog(errMsg + "，Query方法异常：", ex);
                throw new Exception(errMsg + ex.ToString());
                //throw ex;
            }
        }
        /// <summary>
        /// 统计的方法 获取所有数据
        /// </summary>
        /// <param name="DTList"></param>
        /// <returns></returns>
        public static ResultData QueryStatics(DataTransmission DT)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                ResultData _DS = IDataQuery.QueryStatics(DT);
                return _DS;
            }
            catch (Exception ex)
            {
                String errMsg = String.Empty;
                errMsg += "dataSource：" + DT.DataSource.ToString();
                errMsg += ",fileds：";
                for (int i = 0; i < DT.Fields.Length; i++)
                {
                    errMsg += DT.Fields[i].ToString() + "||";
                }
                for (int i = 0; i < DT.Filters.Count; i++)
                {
                    errMsg += DT.Filters[i].GetString + "||";
                }
                errMsg += ",orders：";
                for (int i = 0; i < DT.Orders.Count; i++)
                {
                    errMsg += DT.Orders[i].ToQueryString() + "||";
                }
                errMsg += ",FirstResult：" + DT.FirstResult;
                errMsg += ",MaxResult：" + DT.MaxResult;
                LogHelper.WriteLog(errMsg + "，QueryStatics方法异常：", ex);
                throw new Exception(DT.DataSource.ToString() + ex.ToString());
                //throw ex;
            }
        }


        /// <summary>
        /// 统计的分页方法
        /// </summary>
        /// <param name="DT"></param>
        /// <returns></returns>
        public static PagingData QueryStaticsForPaging(DataTransmission DT)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                PagingData _DS = IDataQuery.QueryStaticsForPaging(DT);
                return _DS;
            }
            catch (Exception ex)
            {
                String errMsg = String.Empty;
                errMsg += "dataSource：" + DT.DataSource.ToString();
                errMsg += ",fileds：";
                for (int i = 0; i < DT.Fields.Length; i++)
                {
                    errMsg += DT.Fields[i].ToString() + "||";
                }
                for (int i = 0; i < DT.Filters.Count; i++)
                {
                    errMsg += DT.Filters[i].GetString + "||";
                }
                errMsg += ",orders：";
                for (int i = 0; i < DT.Orders.Count; i++)
                {
                    errMsg += DT.Orders[i].ToQueryString() + "||";
                }
                errMsg += ",FirstResult：" + DT.FirstResult;
                errMsg += ",MaxResult：" + DT.MaxResult;
                LogHelper.WriteLog(errMsg + "，QueryStaticsForPaging方法异常：", ex);
                //LogHelper.WriteLog("源名：" + DT.DataSource.ToString() + "，QueryStaticsForPaging方法异常：", ex);
                throw new Exception(DT.DataSource.ToString() + ex.ToString());
                //throw ex;
            }
        }

        /// <summary>
        /// 通道的分页方法
        /// </summary>
        /// <param name="dataSource">数据源id</param>
        /// <param name="fileds">选择列</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序条件</param>
        /// <param name="selectors">选择条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <returns></returns>
        public static PagingData PQuery(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize)
        {
            try
            {
                DataTransmission _data = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, false, "");
                //DataQuery _DQ = new DataQuery();
                return IDataQuery.QueryForPagging(_data);
            }
            catch (Exception ex)
            {

                String errMsg = String.Empty;
                errMsg += "dataSource：" + dataSource.ToString();
                errMsg += ",fileds：";
                for (int i = 0; i < fileds.Length; i++)
                {
                    errMsg += fileds[i].ToString() + "||";
                }
                for (int i = 0; i < filters.Count; i++)
                {
                    errMsg += filters[i].GetString + "||";
                }
                errMsg += ",orders：";
                for (int i = 0; i < orders.Count; i++)
                {
                    errMsg += orders[i].ToQueryString() + "||";
                }
                errMsg += ",page：" + page;
                errMsg += ",pageSize：" + pageSize;
                LogHelper.WriteLog(errMsg + "，PQuery方法异常：", ex);
                
                //LogHelper.WriteLog("参数：" + dataSource.ToString() + "PQuery方法异常：", ex);
                throw new Exception(dataSource.ToString() + ex.ToString());
                //throw ex;
            }
        }

        /// <summary>
        /// 通过索引来获取数据 该方法在新的通道中已失效
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object QueryIndex(Dictionary<String, String> obj)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                //return IDataQuery.QueryIndex("测试");
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><response><lst name=\"response\"><int name=\"Status\">0</int><int name=\"QTime\">30</int><lst name=\"params\"><str name=\"start\">0</str><str name=\"rows\">8</str><str name=\"q\"></str></lst><result name=\"response\" numFound=\"8108\" start=\"0\"></result></lst></response>";
            }
            catch (Exception ex)
            {
                String _str = String.Empty;
                foreach (KeyValuePair<String, String> entry in obj)
                {
                    _str += "{[" + entry.Key.ToString();
                    _str += entry.Value.ToString() + "]}\n";
                }
                LogHelper.WriteLog("参数：" + _str + "索引服务异常：", ex);
                throw new Exception("索引服务异常：" + _str + ex.ToString());
                //throw ex;
            }
        }

        /// <summary>
        /// 新的资讯通道
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object QueryIndex(String obj)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                DateTime dt = DateTime.Now;
                object retobj = IDataQuery.QueryIndex(obj);
                LogHelper.WriteDebugLog("资讯服务获取数据条件：" + obj + ";查询时间：" + (DateTime.Now - dt));
                return retobj;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("参数：" + obj + "资讯服务异常：", ex);
                throw new Exception("资讯服务异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 宏观指标
        /// </summary>
        /// <param name="cmd">$-edbrpt\r\n$rpt(name=RPT000002)\r\n</param>
        /// <returns></returns>
        public static DataSet QueryMacroIndicate(MacroIndicateParam param)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                DateTime dt = DateTime.Now;
                DataSet ds = (DataSet)IDataQuery.QueryMacroIndicate("$-edbrpt\r\n$rpt(" + param.ToString() + ")\r\n");
                LogHelper.WriteDebugLog("宏观指标服务获取数据条件：$-edbrpt\r\n$rpt(" + param.ToString() + ")\r\n;查询时间：" + (DateTime.Now - dt));
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("宏观指标异常:", ex);
                throw new Exception("宏观指标异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 获取二维表的报告期
        /// 获取二维表父报表下所有时间报告期：
        ///String tcmd = "$-edbrpt\r\n$BIVRPTIDS(name=PARENTREPORTID)\r\n";
        ///BIVRPTIDS 为获取父报表的命令，返回为所有子报表的时间，返回DataTable,table名PARENTREPORTID
        ///例如：$-edbrpt\r\n$BIVRPTIDS(name=RPT00004)\r\n
        ///返回datatable  里面值为  2012-06-01
        /// </summary>
        /// <param name="cmd">$-edbrpt\r\n$BIVRPTIDS(name=RPT00004)\r\n</param>
        /// <returns>2012-06-01</returns>
        public static List<String> QueryMacroParentReportDate(MacroIndicateParam param)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                DateTime dt = DateTime.Now;
                List<String> strList=new List<String>();
                DataSet ds=(DataSet)IDataQuery.QueryMacroIndicate("$-edbrpt\r\n$BIVRPTIDS(name=" + param.ToString() + ")\r\n");
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    strList.Add(row[0].ToString());
                }
                LogHelper.WriteDebugLog("宏观指标服务获取获取二维表的报告期：$-edbrpt\r\n$BIVRPTIDS(name=" + param.ToString() + ")\r\n;查询时间：" + (DateTime.Now - dt));
                return strList;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("查询条件：" + param.ToString() + "获取宏观二维表异常:", ex);
                throw new Exception("获取宏观二维表异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 通过报告期获取二维表的数数据
        /// 获取某一个子报表的的报表
        ///命令：
        ///String tcmd = "$-edbrpt\r\n$BIVRPT(name=SONREPORTID)\r\n"; 
        ///SONREPORTID 是子报表的ID，格式是PARENTREPORTID_DATE  例如:RPT00004_2012-06-01
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet QueryMacroByParentReportDate(MacroIndicateParam param)
        {
            try
            {
                DateTime dt = DateTime.Now;
                DataSet ds = (DataSet)IDataQuery.QueryMacroIndicate("$-edbrpt\r\n$BIVRPT(name=" + param.ToString() + ")\r\n");
                LogHelper.WriteDebugLog("宏观指标服务---通过报告期获取二维表的数数据：$-edbrpt\r\n$BIVRPT(name=" + param.ToString() + ")\r\n;查询时间：" + (DateTime.Now - dt));
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("查询条件：$-edbrpt\r\n$BIVRPT(name=" + param.ToString() + ")\r\n,获取宏观二维表异常:", ex);
                throw new Exception("获取宏观二维表异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 每日行情获取指标
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object QueryIndicate(String obj)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();

                //DateTime dt = DateTime.Now;
                return IDataQuery.QueryIndicate(obj);
                //LogHelper.WriteDebugLog("函数报表---参数：" + obj.ToString() + "\r\n;查询时间：" + (DateTime.Now - dt));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("参数：" + obj + "指标服务异常:", ex);
                throw new Exception("指标服务异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 获取港股、美股的报表服务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet QueryStockIndicate(StockIndicateParam param)
        {
            try
            {
                DateTime dt = DateTime.Now;
                DataSet ds = (DataSet)IDataQuery.QueryIndicate(param.ToString());
                LogHelper.WriteDebugLog("获取港股、美股的报表服务 命令：" + param.ToString() + ";查询时间：" + (DateTime.Now - dt));
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("参数：" + param .ToString()+ "获取港股、美股报表异常：", ex);
                throw new Exception("获取港股、美股报表异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 获取理财首页的报表服务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet QueryFinacingIndicate(FinancingIndexParam param)
        {
            try
            {
                DateTime dt = DateTime.Now;
                DataSet ds = (DataSet)IDataQuery.QueryIndicate(param.ToString());
                LogHelper.WriteDebugLog("获取理财首页的报表服务 命令：" + param.ToString() + ";查询时间：" + (DateTime.Now - dt));
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("参数：" + param.ToString() + "获取理财首页报表异常：", ex);
                throw new Exception("获取理财首页报表异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 获取港股、美股的报表服务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet QueryStockIndicate(String param)
        {
            try
            {
                DateTime dt = DateTime.Now;
                DataSet ds = (DataSet)IDataQuery.QueryIndicate(param);
                LogHelper.WriteDebugLog("获取港股、美股的报表服务 命令：" + param.ToString() + ";查询时间：" + (DateTime.Now - dt));
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("参数："+param.ToString()+"获取港股、美股报表异常：", ex);
                throw new Exception("获取港股、美股报表异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 通过通道 往MongoDB里面写数据
        /// </summary>
        /// <param name="sourceName">源名</param>
        /// <param name="objList">写入的对象集合</param>
        /// <returns></returns>
        public static bool MongoInsert(String sourceName, List<object> objList)
        {
            bool ret = false;
            try
            {
                //DataQuery _DQ = new DataQuery();
                IDataQuery.MongoInsert(sourceName, objList);
                ret = true;
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过通道 删除MongoDB里面写数据
        /// </summary>
        /// <param name="sourceName">源名</param>
        /// <param name="exp">expressiong 条件</param>
        /// <returns></returns>
        public static bool MongoDelete(String sourceName, List<Expression> exp)
        {
            bool ret = false;
            try
            {
                //DataQuery _DQ = new DataQuery();
                //IDataQuery.MongoDelete(sourceName, exp);
                ret = true;
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过通道 查询MongoDB里面写数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="fileds"></param>
        /// <param name="filters"></param>
        /// <param name="orders"></param>
        /// <param name="selectors"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="isPaging"></param>
        /// <returns></returns>
        public static List<T> MongoQuery<T>(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, bool isPaging)
        {
            try
            {
                DataTransmission _dt = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, isPaging, "");
                //DataQuery _DQ = new DataQuery();
                //return IDataQuery.MongoQuery<T>(_dt);
                return new List<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用户信息查询 返回总记录数
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="fileds"></param>
        /// <param name="filters"></param>
        /// <param name="orders"></param>
        /// <param name="selectors"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="isPaging"></param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        public static List<T> MongoQueryPaging<T>(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, bool isPaging, out long total)
        {
            try
            {
                DataTransmission _dt = GetData(dataSource, fileds, filters, orders, selectors, page, pageSize, isPaging, "");
                //DataQuery _DQ = new DataQuery();
                //return IDataQuery.MongoQueryPaging<T>(_dt, out total);
                total = 0;
                return new List<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过通道 查询MongoDB里面写数据
        /// </summary>
        /// <param name="dt">DataTransmission</param>
        /// <returns></returns>
        public static List<T> MongoQuery<T>(DataTransmission dt)
        {
            try
            {
                //DataQuery _DQ = new DataQuery();
                //return IDataQuery.MongoQuery<T>(dt);
                return new List<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过通道 更新MongoDB里面写数据
        /// </summary>
        /// <param name="sourceName">源名</param>
        /// <param name="exp">List<Expression></param>
        /// <param name="obj">更新对象<object></param>
        /// <returns></returns>
        public static bool MongoUpdate(String sourceName, List<Expression> exp, object obj)
        {
            bool ret = false;
            try
            {
                //DataQuery _DQ = new DataQuery();
                //IDataQuery.MongoUpdate(sourceName, exp, obj);
                ret = true;
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成 DataTransmission
        /// </summary>
        /// <param name="dataSource">源或统计的名称</param>
        /// <param name="fileds">查询的字段</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序</param>
        /// <param name="selectors"></param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="IsPaging">是否分页（只在统计分页方法的时候有效）</param>
        /// <param name="statisticsEngName">统计的名称</param>
        /// <returns></returns>
        public static DataTransmission GetData(WebDataSource dataSource, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, bool IsPaging, String statisticsEngName)
        {
            DataTransmission data = GetData(dataSource.ToString(), fileds, filters, orders, selectors, page, pageSize, IsPaging, statisticsEngName);
            return data;
        }
        /// <summary>
        /// 生成 DataTransmission
        /// </summary>
        /// <param name="dataSource">源或统计的名称</param>
        /// <param name="fileds">查询的字段</param>
        /// <param name="filters">过滤条件</param>
        /// <param name="orders">排序</param>
        /// <param name="selectors"></param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="IsPaging">是否分页（只在统计分页方法的时候有效）</param>
        /// <param name="statisticsEngName">统计的名称</param>
        /// <returns></returns>
        public static DataTransmission GetData(String dataSourceStr, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, bool IsPaging, String statisticsEngName)
        {
            DataTransmission data = new DataTransmission();
            data.DataSource = dataSourceStr;
            data.DataTableName = dataSourceStr;
            data.Fields = fileds;
            data.Filters = filters;
            data.FirstResult = (page - 1) * pageSize;
            data.IsPaging = IsPaging;
            data.MaxResult = pageSize;
            data.Orders = orders;
            data.Selector = selectors;
            data.StatisticsEngName = statisticsEngName;
            return data;
        }
        public static DataTransmission GetData(String dataSourceStr, String dataTableName, String[] fileds, List<Expression> filters, List<Order> orders, List<Expression> selectors, int page, int pageSize, bool IsPaging, String statisticsEngName)
        {
            DataTransmission data = new DataTransmission();
            data.DataSource = dataSourceStr;
            data.DataTableName = dataTableName;
            data.Fields = fileds;
            data.Filters = filters;
            data.FirstResult = (page - 1) * pageSize;
            data.IsPaging = IsPaging;
            data.MaxResult = pageSize;
            data.Orders = orders;
            data.Selector = selectors;
            data.StatisticsEngName = statisticsEngName;
            return data;
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String GetRealTimeInfo(String url)
        {
            try
            {
                //DateTime dt = DateTime.Now;
                String result = String.Empty;
                System.Net.ServicePointManager.Expect100Continue = false;
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.ContentType = "text/plain";
                
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                StreamReader streamReader = new StreamReader(stream, encode);
                result = streamReader.ReadToEnd();
                //LogHelper.WriteDebugLog("GetRealTimeInfo:url=" + url + "\r\n加载时间：" + (DateTime.Now - dt));
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Url:" + url + "GetRealTimeInfo方法异常:", ex);
                throw new Exception("GetRealTimeInfo方法异常："+ex);
            }
        }
    }
}
