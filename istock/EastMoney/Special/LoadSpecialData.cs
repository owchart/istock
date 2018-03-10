using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using EmCore;
using System.Data;
using System.Xml;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 文件下载完成委托
    /// </summary>
    /// <param name="fileName"></param>
    public delegate void DownFileCompleted(string fileName);

    /// <summary>
    /// 加载专题数据
    /// </summary>
    public class LoadSpecialData
    {

        public static void ClearDataCache()
        {
            modSpecialtopics.Clear();
            if (CatKeymodSpecialtopics.Count>0)
                foreach (var item in CatKeymodSpecialtopics.Values)
                {
                    item.Clear();
                }

            CatKeymodSpecialtopics.Clear();
            CatKeymodSpecialtopics = null;
            if (Dtspecial != null)
                CommonService.DisposeDataTable(Dtspecial);

            Dtspecial = null;
            if (Dtspecialcategory != null)
            {
          
                CommonService.DisposeDataTable(Dtspecialcategory);


            }
            Dtspecialcategory = null;
            foreach (var value in dictionaryTreeData.Values)
            {
               CommonService.DisposeDataTable(value);
            }

            dictionaryTreeData.Clear();
            dictionaryTreeData = null;
        }

        static void UIService_SpecialComboxIsChange(bool b)
        {
            CommonContant.BlnUpdateComboxData = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        static void UIService_SpecialTreeIsChange(bool b)
        {
            CommonContant.BlnUpdateSpecialTree = true;
            //   throw new NotImplementedException();
        }

        /// <summary>
        /// 文件下载完成事件
        /// </summary>
        public static event DownFileCompleted DownFileCompleted;

        /// <summary>
        /// 文件下载完成事件
        /// </summary>
        public static void OnDownFileCompleted()
        {
            DownFileCompleted handler = DownFileCompleted;
            if (handler != null) handler(null);
        }

        /// <summary>
        /// 专题树菜单缓存集合
        /// </summary>
        private static Dictionary<string, DataTable> dictionaryTreeData =
            new Dictionary<string,DataTable>();
        private static Dictionary<string, DataRow> modSpecialtopics = new Dictionary<string, DataRow>();
        private static Dictionary<string, List<DataRow>> CatKeymodSpecialtopics = new Dictionary<string, List<DataRow>>();

        /// <summary>
        /// 专题分类初始化
        /// </summary>
        /// <param name="iclist">专题类集合</param>
        /// <param name="dt">数据表</param>
        /// <param name="categrrycode">分类code</param>
        private static void InitSpecialTopicCategory(List<DataRow> iclist, DataTable dt,
                                                     string categrrycode)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;
            List<DataRow> results = new List<DataRow>();
            List<DataRow> res = new List<DataRow>();
            //DataTableExtentions.DataTableToArray(dt).Where(item => Normal.ParseString(item["CATEGORYCODE"]).Trim() == categrrycode.Trim()).ToList();
            foreach (DataRow dataRow in DataTableExtentions.DataTableToArray(dt))
            {
                if (Normal.ParseString(dataRow["CATEGORYCODE"]).Trim() == categrrycode.Trim())
                {
                    res.Add(dataRow);
                    break;
                }
            }
            if (res.Count > 0)
            {
                results.Add(res[0]);
                GetChildsSpecialtopicCategories(results, res.ToArray(), dt);
            }
            iclist.AddRange(results.ToArray());
            results.Clear();
            results = null;
            dt.AcceptChanges();
        }

        /// <summary>
        /// 得出子专题的分类号
        /// </summary>
        /// <param name="results">数据行集合</param>
        /// <param name="drs">数据行数组</param>
        /// <param name="dataTable">数据表</param>
        private static void GetChildsSpecialtopicCategories(List<DataRow> results, DataRow[] drs, DataTable dataTable)
        {

            if (drs != null && drs.Length > 0)
            {
                List<DataRow> rows = new List<DataRow>();
                foreach (var dataRow in drs)
                {
                    //var res = allRows.Where(item => Normal.ParseString(item["PCATEGORYCODE"]) ==Normal.ParseString( dataRow["CATEGORYCODE"])).ToArray();
                    //if (res.Length > 0)
                    //{
                    foreach (DataRow item in dataTable.Rows)
                    {
                        if (Normal.ParseString(item["PCATEGORYCODE"]) == Normal.ParseString(dataRow["CATEGORYCODE"]))
                        {
                            rows.Add(item);
                            results.Add(item);
                        }
                    }
                    //  rows.AddRange(res);
                    //  results.AddRange(res);
                    //}

                }

                if (rows.Count > 0)
                {
                    GetChildsSpecialtopicCategories(results, rows.ToArray(), dataTable);
                }
                rows.Clear();

            }

        }

        /// <summary>
        /// 总数
        /// </summary>
        private static int _index = 100000;

        /// <summary>
        /// 初始化专题树形集合
        /// </summary>
        /// <param name="iclist">专题List</param>
        /// <param name="dt">数据表</param>
        /// <param name="categories">专题统计分类实体类集合</param>
        private static void InitSpecialTopic(List<DataRow> iclist, DataTable dt,
                                             List<DataRow> categories)
        {

            InitNewSpecialTopic(iclist, categories);
            return;
        }



        /// <summary>
        /// 初始化专题树形集合
        /// </summary>
        /// <param name="iclist">专题List</param>
        /// <param name="dt">数据表</param>
        /// <param name="categories">专题统计分类实体类集合</param>
        private static void InitNewSpecialTopic(List<DataRow> iclist,
                                             List<DataRow> categories)
        {
            foreach (DataRow category in categories)
            {
                if (CatKeymodSpecialtopics.ContainsKey(category["CateGoryCode"].ToString()))
                {
                    List<DataRow> list = CatKeymodSpecialtopics[category["CateGoryCode"].ToString()];
                    iclist.AddRange(list);
                    CatKeymodSpecialtopics.Remove(category["CateGoryCode"].ToString());
                }
            }
        }
        public static void InitSpecialTopics(DataTable dt)
        {
            if (dt == null)
                return;
            modSpecialtopics.Clear();
            CatKeymodSpecialtopics.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                bool flag = false;
                if (modSpecialtopics.ContainsKey(dr["SpecialTopicCode"].ToString()))
                    continue;
                modSpecialtopics[dr["SpecialTopicCode"].ToString()] = dr;
                List<DataRow> modlist = null;
                if (CatKeymodSpecialtopics.ContainsKey(dr["CateGoryCode"].ToString()))
                {
                    modlist = CatKeymodSpecialtopics[dr["CateGoryCode"].ToString()];
                }
                else
                {
                    modlist = new List<DataRow>();
                    CatKeymodSpecialtopics[dr["CateGoryCode"].ToString()] = modlist;
                }
                modlist.Add(dr);
            }
            //dt.Clear();
            //dt.AcceptChanges();
        }

        /// <summary>
        /// 专题编号数据
        /// </summary>
        public static DataTable Dtspecialcategory = null;
        /// <summary>
        /// 专题数据
        /// </summary>
        public static DataTable Dtspecial = null;
        /// <summary>
        /// 是否完成数据请求
        /// </summary>
        //  public static bool ReadyDataComplete = false;

        /// <summary>
        /// 初始化专题树数据
        /// </summary>
        public static void InitSpecialTreeData()
        {
            //查询专题
            if (Dtspecial == null)
            {
                Dtspecial = DataCache.QuerySpecialTopic();
            }
            //查询专题分类
            if (Dtspecialcategory == null)
                Dtspecialcategory = DataCache.QuerySpeicalTopicCatory();
            InitSpecialTopics(Dtspecial);
        }

        /// <summary>
        /// 默认配置管理公司过滤的专题集合
        /// </summary>
        public static void InitSpecialTopicodes()
        {
            if (CommonContant.SpecialBlockForSpecialTopicCodes.Count > 0)
                return;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000014544"] = EBlockCategory.MANAGECOMPANY;
            //CommonContant.SpecialBlockForSpecialTopicCodes["100000000012864"] = EBlockCategory.MANAGECOMPANY;
            //CommonContant.SpecialBlockForSpecialTopicCodes["100000000012743"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013482"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012744"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000014725"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012987"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012761"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013101"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012764"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012763"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013318"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012765"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013319"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013186"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000014545"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012989"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000012663"] = EBlockCategory.MANAGECOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013662"] = EBlockCategory.MANAGEMONEYCOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013663"] = EBlockCategory.MANAGEMONEYCOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013661"] = EBlockCategory.MANAGEMONEYCOMPANY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013781"] = EBlockCategory.SUNSHINEADVISER;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000016962"] = EBlockCategory.SUNSHINEADVISER;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000016963"] = EBlockCategory.SUNSHINEADVISER;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000016964"] = EBlockCategory.SUNSHINEADVISER;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013783"] = EBlockCategory.RESERVESUNSHINE;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013321"] = EBlockCategory.FUND;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013221"] = EBlockCategory.FUND;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013982"] = EBlockCategory.FUND;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013981"] = EBlockCategory.FUND;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013665"] = EBlockCategory.MANAGEMONEY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013666"] = EBlockCategory.MANAGEMONEY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013683"] = EBlockCategory.MANAGEMONEY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013681"] = EBlockCategory.MANAGEMONEY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013684"] = EBlockCategory.MANAGEMONEY;
            CommonContant.SpecialBlockForSpecialTopicCodes["100000000013682"] = EBlockCategory.MANAGEMONEY;
        }

        /// <summary>
        /// 得到xml 相同专题文件的 Table 根节点
        /// </summary>
        public static XmlNodeList GetSameSpecialCodesRoot()
        {
            var xdoc = new XmlDocument();
            //string filename = string.Format("{0}\\NecessaryData\\{1}", pathHelper.DataDir, "SameSpecialCodesMapping.xml");
            string filename = Path.Combine("", @"SpecialCombox\SameSpecialCodesMapping.xml");
            if (!File.Exists(filename))
                filename = Path.Combine("", @"SpecialCombox\SameSpecialCodesMapping.xml");
            if (!File.Exists(filename))
                filename = Path.Combine("", @"SameSpecialCodesMapping.xml");
            xdoc.Load(filename);
            XmlNodeList listNodes = xdoc.DocumentElement.GetElementsByTagName("SpecialTopicCode");
            return listNodes;
        }

        /// <summary>
        /// 设置同名专题值
        /// </summary>
        public static void SetSameSpecialCodeValue()
        {
            //if (CommonService.ISCLIENT != ClientType.Client)
            //return;
            XmlNodeList SameSpecialCodelistNodes = null;
            if (CommonContant.SameSpecialCodes.Count == 0)
            {
                SameSpecialCodelistNodes = GetSameSpecialCodesRoot();
                for (int i = 0; i < SameSpecialCodelistNodes.Count; i++)
                {
                    string key = SameSpecialCodelistNodes[i].Attributes["CurCode"].Value;
                    CommonContant.SameSpecialCodes[key] = SameSpecialCodelistNodes[i].Attributes["SourceCode"].Value;
                }
            }
        }
        /// <summary>
        /// 加载初始化时专题的数据,由外壳服务调用
        /// </summary>
        public static void Loaddata()
        {
            SetSameSpecialCodeValue();
            InitSpecialTopicodes();
            if (Dtspecial != null)
                Dtspecial.Clear();
            Dtspecial = null;
            if (Dtspecialcategory != null)
                Dtspecialcategory.Clear();
            Dtspecialcategory = null;
            modSpecialtopics.Clear();
            if (CatKeymodSpecialtopics == null)
            {
                CatKeymodSpecialtopics = new Dictionary<string, List<DataRow>>();
            }
            InitSpecialTreeData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="codename">专题树编号</param>
        /// <returns>专题树节点组合</returns>
        public static DataTable Loaddata(string codename)
        {
            if (dictionaryTreeData == null)
            {
                dictionaryTreeData = new Dictionary<string, DataTable>();
            }
            if (dictionaryTreeData.ContainsKey(codename))
            {
                DataTable res = dictionaryTreeData[codename];
                return res;
            }
            if (CommonContant.BlnUpdateSpecialTree)
            {
                CommonContant.BlnUpdateSpecialTree = false;
                Loaddata();
            }

            var iclist = new List<DataRow>();
            var idlist = new List<DataRow>();
            DataTable dt = new DataTable();
            InitSpecialTopicCategory(iclist, Dtspecialcategory, codename);
            InitSpecialTopic(idlist, Dtspecial, iclist);
            InitBindingType(dt, idlist, iclist);
            //CommonService.SaveFile(codename, bindingTypeList);
            dictionaryTreeData[codename] = dt;
            return dt;
        }

        /// <summary>
        /// 根据SpecialTopicCode 获取实体 ModSpecialtopic
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DataRow GetModSpecialtopicForSpecialtopicCode(string code)
        {
            if (modSpecialtopics.ContainsKey(code))
                return modSpecialtopics[code];
            return null;
        }

        /// <summary>
        /// 转化成能直接绑定到TreeList中的数据结构
        /// </summary>
        /// <param name="bindingTypeList">绑定的数据结构</param>
        /// <param name="icList">专题实体类集合</param>
        /// <param name="indCategoryList">专题统计分类实体集合</param>
        public static void InitBindingType(DataTable res, List<DataRow> icList,
                                           List<DataRow> indCategoryList)
        {
            res.Columns.Add("ID", typeof (string));
            res.Columns.Add("ParentID", typeof (string));
            res.Columns.Add("Name", typeof (string));
            res.Columns.Add("Category", typeof (string));

            foreach (DataRow specialtopic in icList)
            {
                DataRow dr = res.NewRow();

                dr["ID"] = specialtopic["SpecialTopicCode"].ToString();
                dr["Name"] = specialtopic["SpecialTopicName"].ToString();
                dr["Category"] = "0";
                dr["ParentID"] = specialtopic["CateGoryCode"].ToString();
                modSpecialtopics[specialtopic["SpecialTopicCode"].ToString()] = specialtopic;
                res.Rows.Add(dr);
            }


            foreach (DataRow specialCategory in indCategoryList)
            {
                DataRow dr = res.NewRow();

                dr["ID"] = specialCategory["CateGoryCode"].ToString();
                dr["Name"] = specialCategory["CategoryName"].ToString();
                dr["Category"] = "1";
                dr["ParentID"] = specialCategory["PCateGoryCode"].ToString();
                res.Rows.Add(dr);
            }
            icList.Clear();
            indCategoryList.Clear();
        }
    }
}
