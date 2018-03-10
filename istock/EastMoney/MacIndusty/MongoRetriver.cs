namespace EmMacIndustry.DAL
{
    using EmCore;
    using EmMacIndustry.Entity;
    using EmMacIndustry.Model.Constant;
    using EmMacIndustry.Model.Enum;
    using EmSerDataService;
    using EmSocketClient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
using EastMoney.FM.Web.Data;
    using dataquery;

    public sealed class MongoRetriver
    {
        internal static readonly DataQuery _uniqueQuery = DataAccess.IDataQuery;

        private static DataSet GetDataFromIndicatorServer(string formatCommond, params object[] parameters)
        {
            DataSet set = new DataSet();
            string cmd = string.Empty;
            try
            {
                cmd = string.Format(formatCommond, parameters);
                set = new DataQuery().QueryMacroIndicate(cmd) as DataSet;
            }
            catch (Exception exception)
            {
            }
            return set;
        }

        public static DataTable GetDescendantDataByService(string parentCode, MacroDataType type)
        {
            DataTable table = new DataTable();
            try
            {
                string str = string.Join(",", MongoDBConstant.TreeColumnFileds[type]);
                DataSet dataFromIndicatorServer = GetDataFromIndicatorServer(MongoDBConstant.DictTreeCmd[type], new object[] { parentCode, str });
                if (dataFromIndicatorServer == null)
                {
                    return table;
                }
                if (dataFromIndicatorServer.Tables.Count == 0)
                {
                    return table;
                }
                if (dataFromIndicatorServer.Tables[0].Rows.Count == 0)
                {
                    return table;
                }
                table = dataFromIndicatorServer.Tables[0].Copy();
            }
            catch (Exception exception)
            {
                //EMLoggerHelper.Write(exception);
            }
            return table;
        }

        public static DataSet GetExcelMutiIndicatorValuesFromService(string[] macroIDList, MacroDataType treeType)
        {
            DataSet dataFromIndicatorServer = new DataSet();
            try
            {
                string str = string.Join(",", macroIDList);
                new Stopwatch();
                string formatCommond = "$-edb\r\n$indicateupdate(name={0})\r\n";
                dataFromIndicatorServer = GetDataFromIndicatorServer(formatCommond, new object[] { str, DataCenter.UserID });
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("{0}{1}{2}", exception.Message, Environment.NewLine, exception.StackTrace));
            }
            return dataFromIndicatorServer;
        }

        private static List<Expression> GetFilterExpress(FindIndicator queryContditon)
        {
            List<Expression> list = new List<Expression>();
            Expression lhs = null;
            switch (queryContditon.findOptionFlag)
            {
                case FindOptionFlag.None:
                    if (!string.IsNullOrEmpty(queryContditon.content))
                    {
                        string[] strArray = queryContditon.content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        StringBuilder builder = new StringBuilder();
                        foreach (string str in strArray)
                        {
                            builder.AppendFormat("%{0}%", str.ToUpper());
                            if (lhs != null)
                            {
                                lhs = Expression.And(lhs, Expression.Like("STR_MACRONAME", builder.ToString()));
                            }
                            else
                            {
                                lhs = Expression.Like("STR_MACRONAME", builder.ToString());
                            }
                            builder.Length = 0;
                        }
                    }
                    break;

                case FindOptionFlag.CaseSensitive:
                    if (!string.IsNullOrEmpty(queryContditon.content))
                    {
                        string[] strArray2 = queryContditon.content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        StringBuilder builder2 = new StringBuilder();
                        foreach (string str2 in strArray2)
                        {
                            builder2.AppendFormat("%{0}%", str2);
                            if (lhs != null)
                            {
                                lhs = Expression.And(lhs, Expression.Like("STR_MACRONAME", builder2.ToString()));
                            }
                            else
                            {
                                lhs = Expression.Like("STR_MACRONAME", builder2.ToString());
                            }
                            builder2.Length = 0;
                        }
                    }
                    break;

                case FindOptionFlag.FullText:
                    if (!string.IsNullOrEmpty(queryContditon.content))
                    {
                        string[] strArray7 = queryContditon.content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < strArray7.Length; i++)
                        {
                            string text1 = strArray7[i];
                            if (lhs != null)
                            {
                                lhs = Expression.And(lhs, Expression.Eq("STR_MACRONAME", queryContditon.content.ToUpper()));
                            }
                            else
                            {
                                lhs = Expression.Eq("STR_MACRONAME", queryContditon.content.ToUpper());
                            }
                        }
                    }
                    break;

                case FindOptionFlag.All:
                    if (!string.IsNullOrEmpty(queryContditon.content))
                    {
                        string[] strArray8 = queryContditon.content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < strArray8.Length; j++)
                        {
                            string text2 = strArray8[j];
                            if (lhs != null)
                            {
                                lhs = Expression.And(lhs, Expression.Eq("STR_MACRONAME", queryContditon.content));
                            }
                            else
                            {
                                lhs = Expression.Eq("STR_MACRONAME", queryContditon.content);
                            }
                        }
                    }
                    break;
            }
            list.Add(lhs);
            if (queryContditon.dataSource.Length > 0)
            {
                list.Add(Expression.Like("STR_DATASOURCE", string.Format("%{0}%", queryContditon.dataSource)));
            }
            switch (queryContditon.findContentFlag)
            {
                case FindContentFlag.NodeName:
                {
                    int num5 = 1;
                    list.Add(Expression.Eq("STR_FLAG", num5.ToString()));
                    break;
                }
                case FindContentFlag.IndicatorName:
                {
                    int num6 = 0;
                    list.Add(Expression.Eq("STR_FLAG", num6.ToString()));
                    break;
                }
            }
            if (queryContditon.excludeContent.Length > 0)
            {
                list.Add(Expression.NotLike("STR_MACRONAME", string.Format("%{0}%", queryContditon.excludeContent)));
            }
            return list;
        }

        public static DataSet GetGivenFieldMacroData(FindIndicator queryContditon, string[] fields)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataSet set = new DataSet();
            List<DataTransmission> dtList = null;
            DataTransmission item = null;
            try
            {
                item = new DataTransmission();
                item.MaxResult = 0x7fffffff;
                if ((fields != null) && (fields.Length > 0))
                {
                    item.Fields = fields;
                }
                item.Filters = GetFilterExpress(queryContditon);
                dtList = new List<DataTransmission>();
                if (queryContditon.findRange == MacroDataType.All)
                {
                    item.DataSource = "EDB_MACRO_INDICATOR_TEST";
                    dtList.Add(item);
                    DataTransmission transmission2 = new DataTransmission();
                    transmission2.MaxResult = 0x7fffffff;
                    transmission2.Fields = fields;
                    transmission2.DataSource = "EDB_GLOBAL_INDICATOR_TEST";
                    transmission2.Filters = item.Filters;
                    dtList.Add(transmission2);
                    DataTransmission transmission3 = new DataTransmission();
                    transmission3.MaxResult = 0x7fffffff;
                    transmission3.Fields = fields;
                    transmission3.DataSource = "EDB_INDUSTRY_INDICATOR_TEST";
                    transmission3.Filters = item.Filters;
                    dtList.Add(transmission3);
                }
                else
                {
                    item.DataSource = MongoDBConstant.DicTreeSource[queryContditon.findRange];
                    dtList.Add(item);
                }
                set = _uniqueQuery.Query(dtList);
            }
            catch (Exception exception)
            {
                //EMLoggerHelper.Write(exception);
            }
            finally
            {
                stopwatch.Stop();
                //EMLoggerHelper.Write("搜索耗时{0}", new object[] { stopwatch.ElapsedMilliseconds });
                if (item != null)
                {
                    item.Clear();
                }
                if (dtList != null)
                {
                    dtList.Clear();
                }
            }
            return set;
        }

        public static DataTable GetListData(string[] macroIDs, MacroDataType type)
        {
            DataTable table = new DataTable();
            DataTransmission item = null;
            List<DataTransmission> dtList = null;
            try
            {
                item = new DataTransmission();
                item.Filters = new List<Expression>(2);
                item.Filters.Add(Expression.In("STR_MACROID", macroIDs));
                item.Filters.Add(Expression.Eq("STR_FLAG", "0"));
                item.DataSource = MongoDBConstant.DicTreeSource[type];
                item.MaxResult = 0x7fffffff;
                item.Fields = new string[] { "STR_MACROID", "STR_STARTDATE", "STR_ENDDATE", "STR_GXDATE" };
                dtList = new List<DataTransmission>(1);
                dtList.Add(item);
                table = _uniqueQuery.Query(dtList).Tables[0];
            }
            catch (Exception exception)
            {
                //EMLoggerHelper.Write(exception);
            }
            finally
            {
                if (item != null)
                {
                    if ((item.Fields != null) && (item.Fields.Length > 0))
                    {
                        Array.Clear(item.Fields, 0, item.Fields.Length);
                        item.Fields = null;
                    }
                    item.Clear();
                    item = null;
                }
                if (dtList != null)
                {
                    dtList.Clear();
                    dtList = null;
                }
            }
            return table;
        }

        public static DataSet GetMutiIndicatorsFromService(string[] macroIDList)
        {
            string str = string.Join(",", MongoDBConstant.EDBTreeFields);
            DataSet set = null;
            for (int i = 0; i < macroIDList.Length; i++)
            {
                if (macroIDList[i].StartsWith("EM"))
                {
                    string formatCommond = "$-edb\r\n$indicatedetaile2(name={0}|CODES={1}|columns={2})";
                    DataSet dataFromIndicatorServer = GetDataFromIndicatorServer(formatCommond, new object[] { macroIDList[i], str });
                    if (set == null)
                    {
                        set = dataFromIndicatorServer;
                    }
                    else
                    {
                        MergeTable(dataFromIndicatorServer.Tables[0], set.Tables[0]);
                    }
                }
                else
                {
                    string str3 = "$-bond\r\nindicatetree(name={0}|columns={1})";
                    DataSet set3 = GetDataFromIndicatorServer(str3, new object[] { macroIDList[i], str });
                    if (set == null)
                    {
                        set = set3;
                    }
                    else
                    {
                        MergeTable(set3.Tables[0], set.Tables[0]);
                    }
                }
            }
            return set;
        }

        public static DataSet GetMutiIndicatorsFromService(string[] macroIDList, string[] codeslist)
        {
            string str = string.Join(",", MongoDBConstant.EDBTreeFields);
            DataSet set = null;
            for (int i = 0; i < macroIDList.Length; i++)
            {
                if (macroIDList[i].StartsWith("EM"))
                {
                    string formatCommond = "$-edb\r\n$indicatedetaile2(name={0}|CODES={1}|columns={2})";
                    DataSet dataFromIndicatorServer = GetDataFromIndicatorServer(formatCommond, new object[] { macroIDList[i], codeslist[i], str });
                    if (set == null)
                    {
                        set = dataFromIndicatorServer;
                    }
                    else
                    {
                        MergeTable(dataFromIndicatorServer.Tables[0], set.Tables[0]);
                    }
                }
                else
                {
                    string str3 = "$-bond\r\nindicatetree(name={0}|columns={1})";
                    DataSet set3 = GetDataFromIndicatorServer(str3, new object[] { macroIDList[i], codeslist[i], str });
                    if (set == null)
                    {
                        set = set3;
                    }
                    else
                    {
                        MergeTable(set3.Tables[0], set.Tables[0]);
                    }
                }
            }
            return set;
        }

        public static DataSet GetMutiIndicatorValuesFromService(string[] macroIDList, MacroDataType treeType)
        {
            DataSet dataFromIndicatorServer = new DataSet();
            try
            {
                string str = string.Join(",", macroIDList);
                new Stopwatch();
                string formatCommond = MongoDBConstant.DicCommand[treeType];
                dataFromIndicatorServer = GetDataFromIndicatorServer(formatCommond, new object[] { str, DataCenter.UserID });
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("{0}{1}{2}", exception.Message, Environment.NewLine, exception.StackTrace));
            }
            return dataFromIndicatorServer;
        }

        public static void GetMutiIndicatorValuesFromService(string[] macroIDList, MacroDataType treeType, DealWithReturnData ActionAfterGetValueData)
        {
            try
            {
                string str = string.Join(",", macroIDList);
                string formatCommond = MongoDBConstant.DicCommand[treeType];
                DataSet dataFromIndicatorServer = GetDataFromIndicatorServer(formatCommond, new object[] { str, DataCenter.UserID });
                ActionAfterGetValueData(dataFromIndicatorServer);
            }
            catch (Exception exception)
            {
                //EMLoggerHelper.Write(exception);
                ActionAfterGetValueData(null);
            }
        }

        private static void MergeTable(DataTable source, DataTable target)
        {
            for (int i = 0; i < source.Rows.Count; i++)
            {
                DataRow row = target.NewRow();
                row.ItemArray = source.Rows[i].ItemArray;
                target.Rows.Add(row);
            }
        }

        internal static T StringToObj<T>(string str)
        {
            try
            {
                return JSONHelper.DeserializeObject<T>(str);
            }
            catch
            {
                return default(T);
            }
        }
    }
}

