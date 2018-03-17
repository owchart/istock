using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using EmCore;
using EmSerDataService;
using EmSocketClient;
using System.Text;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 数据处理类
    /// </summary>
	public class DataHelper
	{
		//private static int count = 0;
		//static DataQuery dq = new DataQuery();
        /// <summary>
        /// 对象锁
        /// </summary>
		public static object lockObj = new object();

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <returns>查询到的数据集</returns>
		public static DataSet QueryData(String sourcename)
		{
			return QueryData(sourcename, null);
		}

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <returns>查询的数据集结果</returns>
		public static DataSet QueryData(String sourcename, List<FilterMode> modes)
		{
			return QueryData(sourcename, modes, null);
		}

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <returns>查询的数据集结果</returns>
		public static DataSet QueryData(String sourcename, List<FilterMode> modes, String[] fields)
		{
			return QueryData(sourcename, modes, fields, null);
		}

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">键值对</param>
        /// <returns>查询的数据集结果</returns>
		public static DataSet QueryData(String sourcename, List<FilterMode> modes, String[] fields,
		                                Dictionary<GROUPTYPE, String> map)
		{
			return QueryData(sourcename, modes, fields, map, null);
		}

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">键值对</param>
        /// <param name="_orders">排序键值对</param>
        /// <returns>查询的数据集结果</returns>
		public static DataSet QueryData(String sourcename, List<FilterMode> modes, String[] fields,
		                                Dictionary<GROUPTYPE, String> map, Dictionary<String, bool> _orders)
		{
			return QueryData(sourcename, modes, fields, map, _orders, false);
		}

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">键值对</param>
        /// <param name="_orders">排序键值对</param>
        /// <param name="_needToGroup">是否要组合列</param>
        /// <returns>查询的数据集结果</returns>
		public static DataSet QueryData(String sourcename, List<FilterMode> modes, String[] fields,
		                                Dictionary<GROUPTYPE, String> map, Dictionary<String, bool> _orders, bool _needToGroup)
		{
			return QueryData(sourcename, modes, fields, map, _orders, _needToGroup, null);
		}

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">键值对</param>
        /// <param name="_orders">排序键值对</param>
        /// <param name="_needToGroup">是否要组合列</param>
        /// <param name="statisticsEngName">统计英文名</param>
        /// <returns>查询的数据结果</returns>
		public static DataSet QueryData(String sourcename, List<FilterMode> modes, String[] fields,
		                                Dictionary<GROUPTYPE, String> map, Dictionary<String, bool> _orders, bool _needToGroup,
		                                String statisticsEngName)
		{
		//	int result;
			return QueryData(sourcename, modes, fields, map, _orders, _needToGroup, statisticsEngName, null);
		}

        /// <summary>
        /// 过滤后的datatable
        /// </summary>
		public static DataTable FilterTable = null;

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">键值对</param>
        /// <param name="_orders">排序键值对</param>
        /// <param name="_needToGroup">是否要组合列</param>
        /// <param name="statisticsEngName">统计英文名</param>
        /// <param name="handler">数据处理委托</param>
        /// <param name="form">父容器控件</param>
        /// <returns>查询到的数据集</returns>
        public static DataSet QueryData(String sourcename, List<FilterMode> modes, String[] fields,
                                        Dictionary<GROUPTYPE, String> map, Dictionary<String, bool> _orders, bool _needToGroup,
                                        String statisticsEngName, DelegateMgr.QueryHandle handler
            //,Dictionary<String,String[]> blockfilters
            )
        {

            String log = null;
            if (String.IsNullOrEmpty(sourcename))
            {
                //	res = 0;
                return null;
            }

            if (!_needToGroup)
                log = "\n\n\nQueryData请求数据描述开始-------------->\n专题:数据源名 " + sourcename + " ";
            else
            {
                log = "\n\n\nQueryData请求数据描述开始-------------->\n专题:统计名名 " + statisticsEngName + " " + _needToGroup + " SourceName: " +
                      sourcename;
            }
            log = "\n" + log + "\n请求参数描述:\n" + PrintFilterLog(modes) + "\nQueryData请求数据描述结束-------------->\n\n\n";
            if (_orders!=null)
            foreach (KeyValuePair<String, bool> keyValuePair in _orders)
            {
                log = log + "r\n" + "排序字段: " + keyValuePair.Key + keyValuePair.Value;
            }
            CommonService.Log(log);
            try
            {
                //业务平台调用;提取参数过滤排序信息
                if (CommonService.ISCLIENT == ClientType.Config)
                {
                    SetFilterLog(sourcename, modes, fields, map, _orders, _needToGroup, statisticsEngName);
                }
            }
            catch { }
            List<DataTransmission> requests = new List<DataTransmission>();
            DataTransmission dt = new DataTransmission();
            List<Expression> filters = new List<Expression>();
            //Expression bolckexp = null;
            //if (blockfilters != null)
            //  bolckexp = GetBlockExpression(blockfilters);
            dt.DataSource = sourcename;
            if (modes != null && modes.Count > 0)
            {
                Expression e = null;
                foreach (FilterMode mode in modes)
                {
                    if (!mode.IsSendFilter)
                    {
                        continue;
                    }
                    String[] values = mode.Value as String[];
                    if (values != null && values.Length >= 2)
                    {

                        Expression eps = null;
                        eps = Expression.In(mode.BindParam, values);
                        if (eps != null)
                            filters.Add(eps);
                        continue;
                    }
                    else
                    {
                        Expression exp = GetExpression(mode.BindParam, mode.Filter, mode.Value);
                        if (exp != null)
                            filters.Add(exp);
                    }
                }


            }
            //if (bolckexp != null)
            //  filters.Add(bolckexp);
            dt.Selector = filters;
            if (map != null && map.Count > 0)
            {
                Expression ep = Expression.Computing();
                foreach (GROUPTYPE g in map.Keys)
                {
                    switch (g)
                    {
                        case GROUPTYPE.GROUP:
                            ep = ep.GroupBy(map[g]);
                            break;
                        case GROUPTYPE.MAX:
                            ep = ep.Max(map[g]);
                            break;
                        case GROUPTYPE.MIN:
                            ep = ep.Min(map[g]);
                            break;
                        default:
                            break;
                    }
                }
                dt.Computing = ep;
            }
            if (_orders != null && _orders.Count > 0)
            {
                dt.Orders = GetOrders(_orders);
            }
            dt.FirstResult = 0;
            if (!(CommonService.ISCLIENT == ClientType.Client || CommonService.ISCLIENT == ClientType.Config))
            {
                dt.MaxResult = 500;
                dt.IsPaging = true;
            }
            //if (CommonService.ISCLIENT != "1"||CommonService.ISCLIENT!="2")
            //{
            //  dt.MaxResult = 500;
            //  dt.IsPaging = true;
            //}
            requests.Add(dt);
            DataQuery dq =new DataQuery();
            //System.Diagnostics.Stopwatch ss = new System.Diagnostics.Stopwatch();
            //ss.Start();
            DataSet result = null;
            lock (lockObj)
            {
                if (_needToGroup)
                {
                    try
                    {
                        dt.DataSource = sourcename;
                        if (String.IsNullOrEmpty(statisticsEngName))
                            dt.StatisticsEngName = sourcename;
                        else
                        {
                            dt.StatisticsEngName = statisticsEngName;
                        }
                        dt.Filters = filters;
                        CommonService.Log("开始请求数据DataQuery数据：\n 调用方法【ResultData QueryStatics(DataTransmission dt)】");
                        if (handler == null)
                            result = dq.QueryStatics(dt).Data;
                        else
                        {
                            dq.QueryStatics(dt, handler);
                            //res = 1;
                            CommonService.ClearMemory();
                            return null;
                        }
                        CommonService.Log("请求数据DataQuery数据：\n 调用方法【ResultData QueryStatics(DataTransmission dt)】成功");
                    }
                    catch 
                    {
                        //	res = 0;
                        CommonService.Log("请求数据DataQuery数据：\n 调用方法【ResultData QueryStatics(DataTransmission dt)】失败"); CommonService.ClearMemory();
                        return null;
                    }
                    finally
                    {

                    }
                }
                else
                {
                    try
                    {
                        if (handler == null)
                            result = dq.Query(requests);
                        else
                        {
                            dq.Query(requests, handler);
                            CommonService.ClearMemory();
                            return null;
                        }

                    }
                    catch 
                    {
                        CommonService.ClearMemory();
                        return null;
                    }
                    finally
                    {

                    }
                }
            }
            //dq.Dispose();
            //ss.Stop();
            //res = 0;
            CommonService.ClearMemory();
            return result;

        }

        /// <summary>
        /// 分页数据查询
        /// </summary>
        /// <param name="sourcename">数据源名</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">键值对</param>
        /// <param name="_orders">排序键值对</param>
        /// <param name="_needToGroup">是否要组合列</param>
        /// <param name="statisticsEngName">统计英文名</param>
        /// <param name="handler">数据处理委托</param>
        /// <param name="form">父容器控件</param>
        /// <param name="startindex">数据开始行数</param>
        /// <param name="pageCount">页数</param>
        /// <param name="queryHandle">分页数据查询委托</param>
        /// <returns>查询到的数据集</returns>
		public static DataSet QueryPageData(String sourcename, List<FilterMode> modes, String[] fields,
																Dictionary<GROUPTYPE, String> map, Dictionary<String, bool> _orders, bool _needToGroup,
																String statisticsEngName, DelegateMgr.SendBackHandle handler, int startindex,int pageCount, DelegateMgr.QueryHandle queryHandle
			//,Dictionary<String,String[]> blockfilters
	)
		{

			String log = null;
			if (String.IsNullOrEmpty(sourcename))
			{
				//res = 0;
				return null;
			}

            //记录数据源名
            log = "数据源: " + sourcename;
            if (_needToGroup)
                log = ",统计名: " + statisticsEngName;

            log = log + "\r\n";

            if (_orders != null)
            {
                log = log + "排序信息:";
                foreach (KeyValuePair<String, bool> keyValuePair in _orders)
                {
                    log = log + "\r\n    " + keyValuePair.Key;
                    if (keyValuePair.Value)
                    {
                        log = log + ": 升序";
                    }
                    else { log = log + ": 升序"; }



                }
                log = log + "\r\n";
            }
            log = log + PrintFilterLog(modes);
           
		    CommonService.Log(log);
			//业务平台调用;提取参数过滤排序信息
			if (CommonService.ISCLIENT== ClientType.Config )
			{
				SetFilterLog(sourcename, modes, fields, map, _orders, _needToGroup, statisticsEngName);
			}
		
			List<DataTransmission> requests = new List<DataTransmission>();
			DataTransmission dt = new DataTransmission();
			List<Expression> filters = new List<Expression>();
			//Expression bolckexp = null;
			//if (blockfilters != null)
			//  bolckexp = GetBlockExpression(blockfilters);
			dt.DataSource = sourcename;
		
			if (modes != null && modes.Count > 0)
			{
				Expression e = null;
				foreach (FilterMode mode in modes)
				{
					if (!mode.IsSendFilter)
					{
						continue;
					}
					String[] values = mode.Value as String[];
					if (values != null)
					{

						Expression eps = null;
						eps = Expression.In(mode.BindParam, values);
						if (eps != null)
							filters.Add(eps);
						continue;
					}
					else
					{
						Expression exp = GetExpression(mode.BindParam, mode.Filter, mode.Value);
						if (exp != null)
							filters.Add(exp);
					}
				}


			}
	
			//if (bolckexp != null)
			//  filters.Add(bolckexp);
			dt.Selector = filters;
			if (map != null && map.Count > 0)
			{
				Expression ep = Expression.Computing();
				foreach (GROUPTYPE g in map.Keys)
				{
					switch (g)
					{
						case GROUPTYPE.GROUP:
							ep = ep.GroupBy(map[g]);
							break;
						case GROUPTYPE.MAX:
							ep = ep.Max(map[g]);
							break;
						case GROUPTYPE.MIN:
							ep = ep.Min(map[g]);
							break;
						default:
							break;
					}
				}
				dt.Computing = ep;
			}
			
			if (_orders != null && _orders.Count > 0)
			{
				dt.Orders = GetOrders(_orders);
			}
			dt.FirstResult = startindex;
			//if (!(CommonService.ISCLIENT == ClientType.Client || CommonService.ISCLIENT == ClientType.Config))
			//{
			//  dt.MaxResult = 500;
			//  dt.IsPaging = true;
			//}
		    dt.Fields = fields;
            //dt.MaxResult = pageCount;
            dt.MaxResult = int.MaxValue;
			dt.IsPaging = true;
			//if (CommonService.ISCLIENT != "1"||CommonService.ISCLIENT!="2")
			//{
			//  dt.MaxResult = 500;
			//  dt.IsPaging = true;
			//}
			requests.Add(dt);
		    DataQuery dq = null;
            //try
            //{
                dq =new DataQuery();
            //}
            //catch
            //{

            //    dq = new DataQuery();
            //}
			 
			//System.Diagnostics.Stopwatch ss = new System.Diagnostics.Stopwatch();
			//ss.Start();
			DataSet result = null;
			//lock (lockObj)
			//{

				if (_needToGroup)
				{
					
					try
					{
						dt.DataSource = sourcename;
						if (String.IsNullOrEmpty(statisticsEngName))
							dt.StatisticsEngName = sourcename;
						else
						{
							dt.StatisticsEngName = statisticsEngName;
						}
						dt.Filters = filters;
						if (handler != null)
                        {
                            dq.QueryStaticsForPaging(dt, handler);
							
						} else if (queryHandle!=null)
						{
                            String threadid = "";
						    dq.QueryStatics(dt,out threadid,queryHandle);
                            CommonService.Log("开始执行QueryHandler ID:" + threadid);
                            CommonContant.QueryThreadIds.Add(threadid);
						}
						else
						{
                           // form.CurRequestCount++;
                             result= dq.QueryStatics(dt).Data;
							
							//return null;
						}
						CommonService.Log("请求数据DataQuery数据：\n 调用方法【ResultData QueryStatics(DataTransmission dt)】成功");
					}
					catch
					{
					    CommonService.ClearMemory();
					
						return null;
					}
					finally
					{

					}
				}
				else
				{
					try
					{
						
						if (handler == null)
						{
						
                            //result = dq.Query(requests);
						  //  String threadid;
						    result = dq.Query(requests);

						}
						else
						{
							if (startindex == 0)
							//dq.Query(requests, handler);
							{
                                String threadid = "";
								dq.QueryForPagging(dt,out threadid,handler);
                                CommonContant.QueryThreadIds.Add(threadid);
                                CommonService.Log(String.Format("发起数据请求: {0}", threadid));
							
							}

							//lock (CommonContant.LockRequestid)
							//{
							//  TaskHelper.CloseProgress(CommonContant.Requestid);
							//  TaskHelper.StartProgress(ref CommonContant.Requestid);
							//}

							//res = 1;
                            CommonService.ClearMemory();
							return null;
						}
					}
					catch (Exception e)
					{
					//	res = 0;
                        CommonService.Log(e);
                        CommonService.ClearMemory();
						return null;
					}
					finally
					{
						CommonService.Log(String.Format("执行到位置{0}", 17));
					}
				//}
			}
			//dq.Dispose();
			//ss.Stop();
		//	res = 0;
                CommonService.ClearMemory();
			return result;

		}
        /// <summary>
        /// 报表查询
        /// </summary>
        /// <param name="cmd">查询命令</param>
        /// <returns>结果集</returns>
        public static DataSet QueryIndicate(String cmd)
        {
            CommonService.ClearMemory();
            return DataAccess.IDataQuery.QueryIndicate(cmd) as DataSet;
        }

        public static void CancelQuery(String threadid)
        {
             new DataQuery().CancelRequest(threadid);
            CommonService.Log(String.Format("取消数据请求ID: {0}", threadid));
                   
            
        }
        /// <summary>
        /// 获取排序集合
        /// </summary>
        /// <param name="ordermaps">排序键值对</param>
        /// <returns>获取到的排序集合</returns>
		private static List<Order> GetOrders(Dictionary<String, bool> ordermaps)
		{

			if (ordermaps != null && ordermaps.Count > 0)
			{
				List<Order> orders = new List<Order>();
				foreach (String key in ordermaps.Keys)
				{
					bool value = ordermaps[key];
					if (!value)
					{
						Order order = Order.Asc(key);
						orders.Add(order);
					}
					else
					{
						Order order = Order.Desc(key);
						orders.Add(order);
					}
				}
				return orders;
			}
			return null;
		}

        /// <summary>
        /// 获取对象的解释文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filter">过滤</param>
        /// <param name="value">值</param>
        /// <returns>对象的解释文本</returns>
		private static Expression GetExpression(String obj, FILTER filter, object value)
		{
			Expression e = null;
			switch (filter)
			{
				case FILTER.DAYU:
					e = Expression.Gt(obj, value);

					break;
				case FILTER.DAYUDENGYU:
					e = Expression.Ge(obj, value);

					break;
				case FILTER.DENGYU:
					e = Expression.Eq(obj, value);

					break;
				case FILTER.XIAOYU:
					e = Expression.Lt(obj, value);

					break;
				case FILTER.XIAOYUDENGYU:
					e = Expression.Le(obj, value);

					break;
				case FILTER.LIKE:
					e = Expression.Like(obj, value);

					break;
				case FILTER.BAOHANS:
					e = Expression.Like(obj, "%" + (String) value + "%");
					break;
				case FILTER.IN:
					if (value is String[])
					{
						var res = value as String[];
						if (res.Length > 0)
							e = Expression.In(obj, (object[]) value);
					}
					else
					{
						var res1 = value.ToString().Split(new String[] {";"}, StringSplitOptions.RemoveEmptyEntries);
						if (res1.Length > 0)
							e = Expression.In(obj, res1);
					}
					break;

				default:
					break;
			}
			return e;
		}

        /// <summary>
        /// 写log
        /// </summary>
        /// <param name="filterModes">过滤值集合</param>
        /// <returns>log文本字符串</returns>
		private static String PrintFilterLog(List<FilterMode> filterModes)
		{
			if (filterModes == null)
				return "";
			String log = "";
			foreach (var filterMode in filterModes)
			{
				String value = null;
				if (filterMode.Value is String[])
				{
					var res = filterMode.Value as String[];
					foreach (String s in res)
					{
						value = value + s + ";";
					}
                }
                else if (filterMode.Value == null)
                {
                    value = "";
                }
                else
                    value = filterMode.Value.ToString();

                log = log + "\n" + filterMode.BindParam + " " + filterMode.Filter.ToString() + " " + value + " Type: ";
                if (filterMode.Value != null)
                    log = log + filterMode.Value.GetType().Name;
				    
			}
			return log;
		}

        /// <summary>
        /// 设置过滤log
        /// </summary>
        /// <param name="sourcename">数据源名称</param>
        /// <param name="modes">过滤条件集合</param>
        /// <param name="fields">数据列名</param>
        /// <param name="map">map</param>
        /// <param name="_orders">排序键值对</param>
        /// <param name="_needToGroup">是否要列重组</param>
        /// <param name="statisticsEngName">统计英文名</param>
		public static void SetFilterLog(String sourcename, List<FilterMode> modes, String[] fields,
		                                Dictionary<GROUPTYPE, String> map, Dictionary<String, bool> _orders, bool _needToGroup,
		                                String statisticsEngName)
		{
            if (FilterTable ==null)
                return;
			String id = "";
			String speicalname = "";
			if (modes != null)
			{
				foreach (var filter in modes)
				{
					bool findRes = false;
					foreach (DataRow item in DataTableExtentions.DataTableToArray(FilterTable))
					{
						if (item["DATASOURCENAME_EN"].ToString() == sourcename && item["STATICNAME_EN"].ToString() == statisticsEngName &&
							item["FILTER_EN"] != null && item["FILTER_EN"].ToString() == filter.BindParam)
						{
							findRes = true;
							break;
						}
					}
					if (!findRes)
					{
						DataRow dr = FilterTable.NewRow();
						dr["DATASOURCENAME_EN"] = sourcename;
						dr["DATASOURCENAME_CN"] = sourcename;
						dr["STATICNAME_CN"] = statisticsEngName ?? "";
						dr["STATICNAME_EN"] = statisticsEngName ?? "";
						dr["FILTER_EN"] = filter.BindParam;
						dr["ORDER_EN"] = "";
						dr["PROPERTY_CN"] = "";
						dr["ISSTATIC"] = _needToGroup;
						dr["ISBIGDATA"] = false;
						dr["SPECIALTOPICNAME"] = String.Format("{0}&{1}", id, speicalname);
						FilterTable.Rows.Add(dr);
					}
					//if (
					//  DataTableExtentions.DataTableToArray(FilterTable).Where(
					//    item =>
					//    item["DATASOURCENAME_EN"].ToString() == sourcename && item["STATICNAME_EN"].ToString() == statisticsEngName &&
					//    item["FILTER_EN"] != null && item["FILTER_EN"].ToString() == filter.BindParam).Count() == 0)
					//{
					//  DataRow dr = FilterTable.NewRow();
					//  dr["DATASOURCENAME_EN"] = sourcename;
					//  dr["DATASOURCENAME_CN"] = sourcename;
					//  dr["STATICNAME_CN"] = statisticsEngName ?? "";
					//  dr["STATICNAME_EN"] = statisticsEngName ?? "";
					//  dr["FILTER_EN"] = filter.BindParam;
					//  dr["ORDER_EN"] = "";
					//  dr["PROPERTY_CN"] = "";
					//  dr["ISSTATIC"] = _needToGroup;
					//  dr["ISBIGDATA"] = false;
					//  dr["SPECIALTOPICNAME"] = String.Format("{0}&{1}", id, speicalname);
					//  FilterTable.Rows.Add(dr);
					//}
				}
			}
			if (_orders !=null&&_orders .Count>0)
			{
				foreach (KeyValuePair<String, bool> keyValuePair in _orders)
				{
					bool findres = false;
                    if (FilterTable==null)
					foreach (DataRow item in DataTableExtentions.DataTableToArray(FilterTable))
					{
						if (item["DATASOURCENAME_EN"].ToString() == sourcename && item["STATICNAME_EN"].ToString() == statisticsEngName &&
						item["FILTER_EN"] != null && item["ORDER_EN"].ToString() == keyValuePair.Key)
						{
							findres = true;
							break;

						}
					}
					if (!findres)
					{
						DataRow dr = FilterTable.NewRow();
						dr["DATASOURCENAME_EN"] = sourcename ?? "";
						dr["DATASOURCENAME_CN"] = sourcename ?? "";
						dr["STATICNAME_CN"] = statisticsEngName ?? "";
						dr["STATICNAME_EN"] = statisticsEngName ?? "";
						dr["FILTER_EN"] = "";
						dr["ORDER_EN"] = keyValuePair.Key;
						dr["PROPERTY_CN"] = "";
						dr["ISSTATIC"] = _needToGroup;
						dr["ISBIGDATA"] = false;
						dr["SPECIALTOPICNAME"] = String.Format("{0}&{1}", id, speicalname);
						FilterTable.Rows.Add(dr);
					}
					//if (DataTableExtentions.DataTableToArray(FilterTable)
					//.Where(
					//  item =>
					//  item["DATASOURCENAME_EN"].ToString() == sourcename && item["STATICNAME_EN"].ToString() == statisticsEngName &&
					//  item["FILTER_EN"] != null && item["ORDER_EN"].ToString() == keyValuePair.Key).Count() == 0)
					//{
					//  DataRow dr = FilterTable.NewRow();
					//  dr["DATASOURCENAME_EN"] = sourcename ?? "";
					//  dr["DATASOURCENAME_CN"] = sourcename ?? "";
					//  dr["STATICNAME_CN"] = statisticsEngName ?? "";
					//  dr["STATICNAME_EN"] = statisticsEngName ?? "";
					//  dr["FILTER_EN"] = "";
					//  dr["ORDER_EN"] = keyValuePair.Key;
					//  dr["PROPERTY_CN"] = "";
					//  dr["ISSTATIC"] = _needToGroup;
					//  dr["ISBIGDATA"] = false;
						
					//  dr["SPECIALTOPICNAME"] = String.Format("{0}&{1}", id, speicalname);
					//  FilterTable.Rows.Add(dr);
					//}
				}
			}

		}

        /// <summary>
        /// 构建新的过滤数据表
        /// </summary>
		public static void NewFilterTable()
		{
		
			if (FilterTable ==null)
			{
				FilterTable = new DataTable();
				FilterTable.Columns.Add("DATASOURCENAME_CN");
				FilterTable.Columns.Add("DATASOURCENAME_EN");
				FilterTable.Columns.Add("STATICNAME_CN");
				FilterTable.Columns.Add("STATICNAME_EN");
				FilterTable.Columns.Add("FILTER_EN");
				FilterTable.Columns.Add("ORDER_EN");
				FilterTable.Columns.Add("PROPERTY_CN");
				FilterTable.Columns.Add("ISSTATIC");
				FilterTable.Columns.Add("ISBIGDATA");
				FilterTable.Columns.Add("SPECIALTOPICNAME");
				FilterTable.Columns["ISBIGDATA"].DataType = typeof (bool);
				FilterTable.Columns["ISSTATIC"].DataType = typeof(bool);
			}
			else
			{
				FilterTable = FilterTable.Clone();
			}
		}

        /// <summary>
        /// 获取板块成分(ListString)
        /// </summary>
        /// <param name="blockCodes"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static List<String> FormatListStringFormString(String blockCodes, String elements)
        {
            List<String> lstElements = new List<String>();

            if (elements.Length == 0)
                return lstElements;

            char[] separater1 = "$".ToCharArray(); // "$"
            char[] separater2 = "}".ToCharArray(); // "}"
            char[] separater4 = "|".ToCharArray(); // "|"

            String[] strBlocks = null;

            if (elements == "")
                return lstElements;

            List<String> lstCode = new List<String>();

            // 板块
            strBlocks = elements.Split(separater4, StringSplitOptions.RemoveEmptyEntries);

            IDictionary<String, String> EmCodeCompanyMapping = new Dictionary<String, String>();
            //String key = String.Format("{0}{1}", 
            if (blockCodes.Contains("511"))
                EmCodeCompanyMapping = GetEmCodeCompanyMapping();

            if (strBlocks == null)
                return lstElements;

            String blockCd = "";
            String[] strElements = null;
            String[] columns = null;
            StringBuilder sb = null;
            String companyCode = "";
            foreach (String block in strBlocks)
            {
                strElements = block.Split(separater2, StringSplitOptions.RemoveEmptyEntries);
                if (elements == null || elements.Length == 0)
                    continue;

                sb = new StringBuilder();
                blockCd = strElements[0];

                int i = 1;
                if (blockCd.StartsWith("511"))
                {
                    // 有基金的基金公司
                    if (strElements.Length > 1)
                    {
                        for (; i < strElements.Length; i++)
                        {
                            columns = strElements[i].Split(separater1, StringSplitOptions.RemoveEmptyEntries);
                            if (columns == null || columns.Length < 2)
                                continue;

                            if (EmCodeCompanyMapping.ContainsKey(columns[0]))
                                companyCode = EmCodeCompanyMapping[columns[0]];

                            sb.Append(columns[0]);
                            sb.Append("$");
                            sb.Append(columns[1]);
                            sb.Append("$");
                            sb.Append(companyCode);
                            sb.Append("}");
                        }
                    }
                    else
                    {
                        // 没有基金的基金公司
                        if (EmCodeCompanyMapping.ContainsKey(blockCd))
                            companyCode = EmCodeCompanyMapping[blockCd];

                        // 英大基金管理有限公司
                        else if (blockCd == "511073")
                        {
                            sb.Append(blockCd);
                            sb.Append("$");
                            sb.Append("");
                            sb.Append("$");
                            sb.Append("80175498");
                            sb.Append("}");
                        }
                        // 华宸未来基金管理有限公司
                        else if (blockCd == "511074")
                        {
                            sb.Append(blockCd);
                            sb.Append("$");
                            sb.Append("");
                            sb.Append("$");
                            sb.Append("80201857");
                            sb.Append("}");
                        }

                    }
                }
                else
                {
                    for (; i < strElements.Length; i++)
                    {
                        columns = strElements[i].Split(separater1, StringSplitOptions.RemoveEmptyEntries);
                        if (columns == null || columns.Length < 2)
                            continue;

                        sb.Append(columns[0]);
                        sb.Append("$");
                        sb.Append(columns[1]);
                        sb.Append("$");
                        sb.Append(companyCode);
                        sb.Append("}");
                    }
                }

                lstElements.Add(sb.ToString());
            }

            strBlocks = null;

            return lstElements;
        }

        /// <summary>
        /// 获取基金公司和基金Code的Mapping
        /// </summary>
        /// <returns></returns>
        public static IDictionary<String, String> GetEmCodeCompanyMapping()
        {
            IDictionary<String, String> EmCodeCompanyMapping = new Dictionary<String, String>();

            // 对应关系文件
            String filePath = Path.Combine(DataCenter.GetAppPath(), @"NecessaryData/FundCompcode");
            char[] separater1 = "$".ToCharArray(); // "$"
            char[] separater2 = "}".ToCharArray(); // "}"

            if (File.Exists(filePath))
            {
                String content = File.ReadAllText(filePath);
                String[] strRecords = content.Split(separater2, StringSplitOptions.RemoveEmptyEntries);

                if (strRecords == null || strRecords.Length == 0)
                    return EmCodeCompanyMapping;

                String[] strColumns = null;
                foreach (String record in strRecords)
                {
                    strColumns = record.Split(separater1, StringSplitOptions.RemoveEmptyEntries);

                    if (strColumns == null || strColumns.Length < 2)
                        continue;

                    if (!EmCodeCompanyMapping.ContainsKey(strColumns[0]))
                        EmCodeCompanyMapping.Add(strColumns[0], strColumns[1]);
                }
            }


            return EmCodeCompanyMapping;
        }
	}
}





