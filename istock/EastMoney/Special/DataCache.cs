using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using EmCore;

namespace OwLib
{
    /// <summary>
    /// 数据请求处理类
    /// </summary>
    public class DataCache
    {
        public static void ClearCache()
        {
            if (_modMaxreportdateDataTable != null)
            {
                _modMaxreportdateDataTable.Clear();
                _modMaxreportdateDataTable.Dispose();
                _modMaxreportdateDataTable = null;
            }
            if (_modReporttypexlkDataTable != null)
            {
                _modReporttypexlkDataTable.Clear();
                _modReporttypexlkDataTable.Dispose();
                _modReporttypexlkDataTable = null;
            }

        }
        //同步查询
        /// <summary>
        /// 同步查询
        /// </summary>
        /// <param name="datasourcename">数据源名称</param>
        /// <returns>得到的数据集</returns>
        private static DataSet QueryData(String datasourcename)
        {
            List<FilterMode> list = new List<FilterMode>();
            FilterMode filterMode = new FilterMode();
            filterMode.BindParam = "ISUSABLE";
            filterMode.Filter = FILTER.DENGYU;
            filterMode.Value = 1;
            list.Add(filterMode);
            List<String> feilds = new List<String>() { "SPECIALTOPICCODE", "SPECIALTOPICNAME", "CATEGORYCODE", "SORTCODE", "IMPORTANT", "ISSHOWBLOCK" };
            DataSet result = DataHelper.QueryData(datasourcename, list, feilds.ToArray());

            return result;
        }
        /// <summary>
        /// 同步查询
        /// </summary>
        /// <param name="datasourcename">数据源名称</param>
        /// <returns>得到的数据集</returns>
        private static DataSet QueryData(String datasourcename, String[] feilds)
        {
            try
            {
                List<FilterMode> list = new List<FilterMode>();
                FilterMode filterMode = new FilterMode();
                filterMode.BindParam = "ISUSABLE";
                filterMode.Filter = FILTER.DENGYU;
                filterMode.Value = 1;
                list.Add(filterMode);
                Dictionary<String, bool> orders = new Dictionary<String, bool>();
                orders["SORTCODE"] = false;
                DataSet result = DataHelper.QueryData(datasourcename, list, feilds, null, orders);

                return result;
            }
            catch (Exception exception)
            {
              //  MessageBox.Show(exception.ToString());
                throw exception;
            }
        }

        ///// <summary>
        ///// 通过数据源名称进行数据查询
        ///// </summary>
        ///// <param name="datasourcename">数据源名称</param>
        ///// <returns>数据表</returns>
        //public static DataTable QueryByDatasoureName(String datasourcename)
        //{
        //    DataSet ds = QueryData(datasourcename);
        //    if (ds != null && ds.Tables.Count > 0)
        //        return ds.Tables[0];
        //    else
        //        return null;
        //}

        /// <summary>
        /// 查询专题分类
        /// </summary>
        /// <returns>专题分类数据表</returns>
        public static DataTable QuerySpeicalTopicCatory()
        {
            //if (EmReportWatch.SpecialCommon.CommonService.ISCLIENT == ClientType.Config)
            //{

              //  return QueryByDatasoureName("MOD_STC_ALL");
            if (CommonService.ISCLIENT == ClientType.Config)
            {
                List<String> feilds = new List<String>() {"CATEGORYCODE", "CATEGORYNAME", "PCATEGORYCODE", "SORTCODE"};
                return QueryData("MOD_STC_ALL", feilds.ToArray()).Tables[0];
            }
            //}
            try
            {
                //定义文件夹
                String fileName = DataCenter.GetAppPath() + "\\NecessaryData\\MOD_STC_ALL";
                if (File.Exists(fileName))
                {
                    return JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 查询专题
        /// </summary>
        /// <returns>专题树数据源</returns>
        public static DataTable QuerySpecialTopic()
        {
            //if (EmReportWatch.SpecialCommon.CommonService.ISCLIENT == ClientType.Config)
            //{
            //return QueryByDatasoureName("MOD_SS");
            if (CommonService.ISCLIENT == ClientType.Config)
                try
                {
                    List<String> feilds = new List<String>()
                        {
                            "SPECIALTOPICCODE",
                            "SPECIALTOPICNAME",
                            "CATEGORYCODE",
                            "SORTCODE",
                            "IMPORTANT",
                            "ISSHOWBLOCK",
                            "CONFIGFILENAME"
                        };
                    DataSet ds = QueryData("MOD_SS", feilds.ToArray());
                    if (ds.Tables.Count == 0)
                        return null;
                    return ds.Tables[0];
                }
                catch (Exception e)
                {
                    CommonService.Log(e.ToString());
                    MessageBox.Show(e.ToString());

                }
            //}
            //while (!AppBaseServices.NecessaryDataIsLoaded)
            //{
            //  Thread.Sleep(10);
            //}
            try
            {
                //定义文件夹
                String fileName = DataCenter.GetAppPath() + "\\NecessaryData\\MOD_SS";
                if (File.Exists(fileName))
                {
                    return JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                }
                return null;
            }
            catch
            {
                return null; // QueryByDatasoureName("MOD_SS");
            }
        }

        //查询专题
        private static DataTable _modMaxreportdateDataTable;

        /// <summary>
        /// 查询专题最大报告期数据源
        /// </summary>
        /// <returns>专题最大报告期数据源</returns>
        public static DataTable QueryModMaxreportdate()
        {
            if (_modMaxreportdateDataTable != null)
                return _modMaxreportdateDataTable;
            //while (!AppBaseServices.NecessaryDataIsLoaded)
            //{
            //  Thread.Sleep(10);
            //}
            try
            {
                if (CommonService.ISCLIENT == ClientType.Config)
                {
                    return DataHelper.QueryData("MOD_MAXREPORTDATE").Tables[0];
                }
                //定义文件夹
                String fileName = DataCenter.GetAppPath() + "\\NecessaryData\\MOD_MAXREPORTDATE";
                if (File.Exists(fileName))
                {
                    _modMaxreportdateDataTable =
                        JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                    return _modMaxreportdateDataTable;
                }
                return null;
            }
            catch
            {
                return DataHelper.QueryData("MOD_MAXREPORTDATE").Tables[0];
            }
        }

        //查询专题
        private static DataTable _modReporttypexlkDataTable;

        /// <summary>
        /// 下拉框报告期数据源
        /// </summary>
        /// <returns>下拉框报告期数据源</returns>
        public static DataTable QueryModReporttypexlk()
        {
            if (_modReporttypexlkDataTable != null)
                return _modReporttypexlkDataTable;
            try
            {
                if (CommonService.ISCLIENT == ClientType.Config)
                {
                     _modReporttypexlkDataTable= DataHelper.QueryData("MOD_REPORTTYPEXLK", null, null, null, null).Tables[0];
                     return _modReporttypexlkDataTable;
                }
                //定义文件夹
                String fileName = DataCenter.GetAppPath() + "\\NecessaryData\\MOD_REPORTTYPEXLK";
                if (File.Exists(fileName))
                {
                    _modReporttypexlkDataTable =
                        JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                    return _modReporttypexlkDataTable;
                }
                return DataHelper.QueryData("MOD_REPORTTYPEXLK", null, null, null, null).Tables[0];
            }
            catch
            {
                return DataHelper.QueryData("MOD_REPORTTYPEXLK", null, null, null, null).Tables[0];

            }
        }

        /// <summary>
        /// 经济指标图表数据源
        /// </summary>
        /// <returns>经济指标图表数据源</returns>
        public static DataTable QueryMAC_JJZBTB_TJ()
        {
            try
            {
              // return SpecialCommon.DataHelper.QueryData("MAC_JJZBTB", null, null, null, null, true, "MAC_JJZBTB_TJ").Tables[0];
                //定义文件夹
                String fileName = DataCenter.GetAppPath() + "\\NecessaryData\\MAC_JJZBTB_TJ";
                if (File.Exists(fileName))
                {
                    return JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        //查询专题
        /// <summary>
        /// 经济指标图表数据源
        /// </summary>
        /// <returns>经济指标图表数据源</returns>
        public static DataTable QueryMAC_JJZBTB()
        {
            //while (!AppBaseServices.NecessaryDataIsLoaded)
            //{
            //    Thread.Sleep(10);
            //}
            //return SpecialCommon.DataHelper.QueryData("MAC_JJZBTB", null, null, null, null).Tables[0];
            try
            {
                //定义文件夹
                String fileName = DataCenter.GetAppPath() + "\\NecessaryData\\MAC_JJZBTB";
                if (File.Exists(fileName))
                {
                    return JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 取得本地MOD_DATERE文件夹中的数据
        /// </summary>
        /// <returns>取到的本地MOD_DATERE数据</returns>
        public static DataTable QueryModDatere()
        {
            //while (!AppBaseServices.NecessaryDataIsLoaded)
            //{
            //    Thread.Sleep(10);
            //}
            try
            {
               //return DataHelper.QueryData("MOD_DATERE", null, null, null, null).Tables[0];
                //if (CommonService.ISCLIENT == ClientType.Config)
                //{
                    //Dictionary<String, bool> order = new Dictionary<String, bool>();
                    //order["SID"] = false;
                    return DataHelper.QueryData("MOD_DATERE", null, null, null, null).Tables[0];
                //}
                //IApp app = ServiceHelper.GetService<IApp>();
                ////定义文件夹
                //String fileName = app.SdataDir + "\\NecessaryData\\MOD_DATERE";
                //if (File.Exists(fileName))
                //{
                //    DataTable dt = JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                //    return JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(fileName)).Tables[0];
                //}
                //return DataHelper.QueryData("MOD_DATERE", null, null, null, null).Tables[0];

            }
            catch
            {
                return DataHelper.QueryData("MOD_DATERE", null, null, null, null).Tables[0];

            }
        }
    }
}
