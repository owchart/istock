using System;
using System.Collections.Generic;
using System.Text;
using EmCore;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Web;

namespace OwLib
{
    /// <summary>
    /// 资讯新通道数据请求model
    /// </summary>
    public class SocketModel
    {
        public SocketModel()
        {
            this._cid = String.Empty;
            this._rid = String.Empty;
            this._pageno = String.Empty;
            this._pagesize = String.Empty;
            this._h = String.Empty;
            this._id = String.Empty;
            this._column = new String[0];
            this.otherparams = String.Empty;
        }
        public SocketModel(String h)
        {
            this._h = h;
            this._id = String.Empty;
            this._column = new String[0];
            this._cid = String.Empty;
            this._rid = String.Empty;
            this._pageno = String.Empty;
            this._pagesize = String.Empty;
            this.otherparams = String.Empty;
        }
        public SocketModel(String h, String id, String pageno, String pagesize)
        {
            this._h = h;
            this._id = id;
            this._column = new String[0];
            this._cid = String.Empty;
            this._rid = String.Empty;
            this._pageno = pageno;
            this._pagesize = pagesize;
            this.otherparams = String.Empty;
        }
        public SocketModel(String h, String id, String otherparams, String pageno, String pagesize)
        {
            this._h = h;
            this._id = id;
            this._column = new String[0];
            this._cid = String.Empty;
            this._rid = String.Empty;
            if (String.IsNullOrEmpty(pageno))
            {
                this.pageno = "1";
            }
            else
            {
                this.pageno = pageno;
            }
            if (String.IsNullOrEmpty(pagesize))
            {
                this.pagesize = int.MaxValue.ToString();
            }
            else
            {
                this.pagesize = pagesize;
            }
            this.otherparams = otherparams;
        }
        public SocketModel(String h, String cid)
        {
            this._h = h;
            this._id = String.Empty;
            this._column = new String[0];
            this._cid = cid;
            this._rid = String.Empty;
            this._pageno = String.Empty;
            this._pagesize = String.Empty;
            this.otherparams = String.Empty;
        }
        public SocketModel(String cid, String request, String h, String id, String otherparams,
                           String pageno, String pagesize, String sort, String order)
        {
            this._cid = String.Empty;
            this._rid = String.Empty;
            this._h = h;
            this._id = id;
            this._column = new String[0];
            this.sort = String.Empty;
            this.order = String.Empty;
            if (!String.IsNullOrEmpty(cid))
            {
                this.cid = cid;
            }
            if (!String.IsNullOrEmpty(request))
            {
                this.request = request;
            }
            if (String.IsNullOrEmpty(pageno))
            {
                this.pageno = "1";
            }
            else
            {
                this.pageno = pageno;
            }
            if (String.IsNullOrEmpty(pagesize))
            {
                this.pagesize = int.MaxValue.ToString();
            }
            else
            {
                this.pagesize = pagesize;
            }
            this.otherparams = otherparams;
            if (!String.IsNullOrEmpty(sort))
            {
                this.sort = sort;
            }
            if (!String.IsNullOrEmpty(order))
            {
                this.order = order;
            }
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        private String _cid;

        public String cid
        {
            get { return _cid; }
            set { _cid = value; }
        }
        /// <summary>
        /// 缓存标识
        /// </summary>
        private String _rid;

        public String rid
        {
            get { return _rid; }
            set { _rid = value; }
        }
        /// <summary>
        /// 第几页
        /// </summary>
        private String _pageno;

        public String pageno
        {
            get { return _pageno; }
            set { _pageno = value; }
        }
        /// <summary>
        /// 页面记录数
        /// </summary>
        private String _pagesize;

        public String pagesize
        {
            get { return _pagesize; }
            set { _pagesize = value; }
        }
        /// <summary>
        /// 头标识-资讯类型
        /// </summary>
        private String _h;

        public String h
        {
            get { return _h; }
            set { _h = value; }
        }
        /// <summary>
        /// 节点标识
        /// </summary>
        private String _id;

        public String id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 证券名称
        /// </summary>
        private String[] _column;

        public String[] column
        {
            get { return _column; }
            set { _column = value; }
        }
        private String _otherparams;

        public String otherparams
        {
            get { return _otherparams; }
            set { _otherparams = value; }
        }
        /// <summary>
        /// 请求类型
        /// </summary>
        public String request = "simpleSearch";//默认简单查询。subscribeSearch：查询订阅内容
        public String sort = String.Empty;
        public String order = String.Empty;
    }

    public class NewsDetailModel
    {
        /// <summary>
        /// CODE
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 收藏
        /// </summary>
        public String IfStore { get; set; }
    }

    public class SktByIdsModel
    {
        public SktByIdsModel(String h, String[] ids)
        {
            this.h = h;
            this.ids = ids;
            this.cid = String.Empty;
        }
        /// <summary>
        /// 请求
        /// </summary>
        public String request = "getInfoById";
        public String cid { get; set; }
        /// <summary>
        /// 大类别：H1:新闻,H2:公告,H3:研报,H4:法律法规
        /// </summary>
        public String h { get; set; }
        /// <summary>
        /// 查询的资讯Ids
        /// </summary>
        public String[] ids { get; set; }

        public String columnType = "detailColumn";

    }

    /// <summary>
    /// Socket高级搜索通信Model
    /// </summary>
    public class SktSrhModel<T>
    {
        public SktSrhModel(String h, T query)
        {
            this.comm = new Comm(h);
            this.query = query;
        }
        public SktSrhModel(String h, String pageno, String pagesize, T query)
        {
            this.comm = new Comm(h, pageno, pagesize);
            this.query = query;
        }
        public SktSrhModel(String h, String pageno, String pagesize, String sort, String order, T query)
        {
            this.comm = new Comm(h, pageno, pagesize, sort, order);
            this.query = query;
        }
        /// <summary>
        /// 公共参数
        /// </summary>
        public class Comm
        {
            public Comm(String h)
            {
                this.cid = String.Empty;
                this.h = h;
                this.sort = String.Empty;
                this.order = String.Empty;
                this.pageno = "1";
                this.pagesize = "50";
            }
            public Comm(String h, String pageno, String pagesize)
            {
                this.cid = String.Empty;
                this.h = h;
                this.sort = String.Empty;
                this.order = String.Empty;
                this.pageno = pageno;
                this.pagesize = pagesize;
            }
            public Comm(String h, String pageno, String pagesize, String sort, String order)
            {
                this.cid = String.Empty;
                this.h = h;
                this.sort = sort;
                this.order = order;
                this.pageno = pageno;
                this.pagesize = pagesize;
            }
            public String cid { get; set; }
            /// <summary>
            /// 大类别：H1:新闻,H2:公告,H3:研报,H4:法律法规
            /// </summary>
            public String h { get; set; }
            /// <summary>
            /// 排序字段
            /// </summary>
            public String sort { get; set; }
            /// <summary>
            /// 排序方式：desc,asc
            /// </summary>
            public String order { get; set; }
            /// <summary>
            /// 分页页码
            /// </summary>
            public String pageno { get; set; }
            /// <summary>
            /// 分页大小
            /// </summary>
            public String pagesize { get; set; }
        }
        public Comm comm { get; set; }
        /// <summary>
        /// 模块搜索参数
        /// </summary>
        public T query { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public String request = "advancedSearch";
    }


    /// <summary>
    /// 为了支持query数组的高级查询 特意将传入参数query改成list泛型来支持 add by lym 2014-4-28
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class SktSrhModelByMultiQuery<T>
    {
        public SktSrhModelByMultiQuery(String h, List<T> query)
        {
            this.comm = new Comm(h);
            this.query = query;
        }
        public SktSrhModelByMultiQuery(String h, String pageno, String pagesize, List<T> query)
        {
            this.comm = new Comm(h, pageno, pagesize);
            this.query = query;
        }
        public SktSrhModelByMultiQuery(String h, String pageno, String pagesize, String sort, String order, List<T> query)
        {
            this.comm = new Comm(h, pageno, pagesize, sort, order);
            this.query = query;
        }
        /// <summary>
        /// 公共参数
        /// </summary>
        public class Comm
        {
            public Comm(String h)
            {
                this.cid = String.Empty;
                this.h = h;
                this.sort = String.Empty;
                this.order = String.Empty;
                this.pageno = "1";
                this.pagesize = "50";
            }
            public Comm(String h, String pageno, String pagesize)
            {
                this.cid = String.Empty;
                this.h = h;
                this.sort = String.Empty;
                this.order = String.Empty;
                this.pageno = pageno;
                this.pagesize = pagesize;
            }
            public Comm(String h, String pageno, String pagesize, String sort, String order)
            {
                this.cid = String.Empty;
                this.h = h;
                this.sort = sort;
                this.order = order;
                this.pageno = pageno;
                this.pagesize = pagesize;
            }
            public String cid { get; set; }
            /// <summary>
            /// 大类别：H1:新闻,H2:公告,H3:研报,H4:法律法规
            /// </summary>
            public String h { get; set; }
            /// <summary>
            /// 排序字段
            /// </summary>
            public String sort { get; set; }
            /// <summary>
            /// 排序方式：desc,asc
            /// </summary>
            public String order { get; set; }
            /// <summary>
            /// 分页页码
            /// </summary>
            public String pageno { get; set; }
            /// <summary>
            /// 分页大小
            /// </summary>
            public String pagesize { get; set; }
            /// <summary>
            /// boolean型，可以为空，默认为false（true：query为数组，包含多个条件，false：query为单个对象）
            /// </summary>
            public bool multiQuery = true;
        }
        public Comm comm { get; set; }
        /// <summary>
        /// 模块搜索参数
        /// </summary>
        public List<T> query { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public String request = "advancedSearch";

    }


    /// <summary>
    /// 新闻的queryModel
    /// </summary>
    public class NewsSktSrhQryModel
    {
        public NewsSktSrhQryModel()
        {
            this.types = new String[0];
            this.securitycodes = new String[0];
            this.industrys = new String[0];
            this.terms = String.Empty;
            this.columnType = String.Empty;
        }
        public NewsSktSrhQryModel(String[] types, String[] securitycodes, String[] industrys, String terms, String columnType)
        {
            this.types = types;
            this.securitycodes = securitycodes;
            this.industrys = industrys;
            this.terms = terms;
            if (String.IsNullOrEmpty(columnType))
            {
                this.columnType = String.Empty;
            }
            else
            {
                this.columnType = columnType;
            }
        }

        public String[] types { get; set; }
        public String[] securitycodes { get; set; }
        public String[] industrys { get; set; }
        public String terms { get; set; }
        public String columnType { get; set; }
    }

    /// <summary>
    /// 研报的queryModel
    /// </summary>
    public class ReportSktSrhQryModel
    {
        public ReportSktSrhQryModel()
        {
            this.types = new String[0];
            this.orgs = new String[0];
            this.authors = new String[0];
            this.securitycodes = new String[0];
            this.industrys = new String[0];
            this.concepts = new String[0];
            this.ratings = new String[0];
            this.ratingchanges = new String[0];
            this.terms = String.Empty;
            this.columnType = String.Empty;
        }
        public ReportSktSrhQryModel(String[] types, String[] orgs, String[] authors, String[] securitycodes, String[] industrys, String[] concepts,
                                    String[] ratings, String[] ratingchanges, String terms, String columnType)
        {
            this.types = types;
            this.orgs = orgs;
            this.authors = authors;
            this.securitycodes = securitycodes;
            this.industrys = industrys;
            this.concepts = concepts;
            this.ratings = ratings;
            this.ratingchanges = ratingchanges;
            this.terms = terms;
            if (String.IsNullOrEmpty(columnType))
            {
                this.columnType = String.Empty;
            }
            else
            {
                this.columnType = columnType;
            }
        }
        public ReportSktSrhQryModel(String terms, String columnType)
        {
            this.types = new String[0];
            this.orgs = new String[0];
            this.authors = new String[0];
            this.securitycodes = new String[0];
            this.industrys = new String[0];
            this.concepts = new String[0];
            this.ratings = new String[0];
            this.ratingchanges = new String[0];
            this.terms = terms;
            this.columnType = columnType;
        }
        public String[] types { get; set; }
        public String[] orgs { get; set; }
        public String[] authors { get; set; }
        public String[] securitycodes { get; set; }
        public String[] industrys { get; set; }
        public String[] concepts { get; set; }
        public String[] ratings { get; set; }
        public String[] ratingchanges { get; set; }
        public String terms { get; set; }
        public String columnType { get; set; }
    }
    /// <summary>
    /// 公告的queryModel
    /// </summary>
    public class NoticeSktSrhQryModel
    {
        public NoticeSktSrhQryModel()
        {
            this.types = new String[0];
            this.securitycodes = new String[0];
            this.terms = String.Empty;
            this.columnType = String.Empty;
        }
        public NoticeSktSrhQryModel(String[] types, String[] securitycodes, String terms, String columnType)
        {
            this.types = types;
            this.securitycodes = securitycodes;
            this.terms = terms;
            if (String.IsNullOrEmpty(columnType))
            {
                this.columnType = String.Empty;
            }
            else
            {
                this.columnType = columnType;
            }
        }

        public String[] types { get; set; }
        public String[] securitycodes { get; set; }
        public String terms { get; set; }
        public String columnType { get; set; }
    }
    /// <summary>
    /// 法律法规的queryModel
    /// </summary>
    public class LawsSktSrhQryModel
    {
        public LawsSktSrhQryModel()
        {
            this.types = new String[0];
            this.terms = String.Empty;
            this.columnType = String.Empty;
        }
        public LawsSktSrhQryModel(String[] types, String terms, String columnType)
        {
            this.types = types;
            this.terms = terms;
            if (String.IsNullOrEmpty(columnType))
            {
                this.columnType = String.Empty;
            }
            else
            {
                this.columnType = columnType;
            }
        }

        public String[] types { get; set; }
        public String terms { get; set; }
        public String columnType { get; set; }
    }

    public class NewsTypeRoot
    {
        private String m_cid;

        public String Cid
        {
            get { return m_cid; }
            set { m_cid = value; }
        }

        private String m_id;

        public String Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_name;

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private List<NewsTypeNode> m_nodeList;

        public List<NewsTypeNode> NodeList
        {
            get { return m_nodeList; }
            set { m_nodeList = value; }
        }
    }

    public class NewsTypeNode
    {
        private String m_id;

        public String Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_model;

        public String Model
        {
            get { return m_model; }
            set { m_model = value; }
        }

        private String m_name;

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private List<NewsTypeNode> m_nodeList;

        public List<NewsTypeNode> NodeList
        {
            get { return m_nodeList; }
            set { m_nodeList = value; }
        }

        private String m_expandNode;

        public String expandNode
        {
            get { return m_expandNode; }
            set { m_expandNode = value; }
        }
    }

    public class NewsListRoot
    {
        private String m_h;

        public String H
        {
            get { return m_h; }
            set { m_h = value; }
        }

        private String m_nodeId;

        public String NodeId
        {
            get { return m_nodeId; }
            set { m_nodeId = value; }
        }

        private String m_nodeName;

        public String NodeName
        {
            get { return m_nodeName; }
            set { m_nodeName = value; }
        }

        private List<NewsListNode> m_records;

        public List<NewsListNode> records
        {
            get { return m_records; }
            set { m_records = value; }
        }

        private int m_total;

        public int total
        {
            get { return m_total; }
            set { m_total = value; }
        }
    }

    public class NewsListNode
    {
        private String m_date;

        public String Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private String m_from;

        public String From
        {
            get { return m_from; }
            set { m_from = value; }
        }

        private String m_id;

        public String Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_order;

        public String Order
        {
            get { return m_order; }
            set { m_order = value; }
        }

        private String m_title;

        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        private String m_docreader;

        public String docreader
        {
            get { return m_docreader; }
            set { m_docreader = value; }
        }

        private String m_medianame;

        public String medianame
        {
            get { return m_medianame; }
            set { m_medianame = value; }
        }

        private String m_newsid;

        public String newsid
        {
            get { return m_newsid; }
            set { m_newsid = value; }
        }

        private String m_url;

        public String url
        {
            get { return m_url; }
            set { m_url = value; }
        }
    }

    public class StockNewsDataHelper
    {
        /// <summary>
        /// 新通道-左侧菜单
        /// </summary>
        public static object GetLeftTree(String cid)
        {
            try
            {
                SocketModel obj = new SocketModel("H1");
                obj.cid = cid;
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(obj));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("新闻菜单获取出错：", ex);
                throw ex;
            }
        }

        /// <summary>
        /// 新通道-根据id获取新闻列表
        /// </summary>
        public static object GetNewsById(String id, String pageIndex, String limit, String order, String sort)
        {
            try
            {
                SocketModel obj = new SocketModel("H1", id, pageIndex, limit);
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(obj));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("根据id获取新闻列表数据获取出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新通道-根据id获取我的新闻
        /// </summary>
        public static object GetNewsBySubId(String id, String cid, String pageIndex, String limit, String order, String sort)
        {
            try
            {
                SocketModel obj = new SocketModel("H1", id, pageIndex, limit);
                obj.request = "subscribeSearch";
                if (!String.IsNullOrEmpty(cid))
                    obj.cid = cid;
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(obj));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("根据id获取我的新闻列表数据获取出错：", ex);
                return null;
            }
        }
        /// <summary>
        /// 新通道-简单搜索
        /// </summary>
        public static object GetNewsByParam(String id, String securityCodes, String term, String pageIndex, String limit, String order, String sort)
        {
            String errorquery = String.Empty;
            try
            {
                SktSrhModel<NewsSktSrhQryModel> queryModel = new SktSrhModel<NewsSktSrhQryModel>("H1", new NewsSktSrhQryModel());
                queryModel.comm.pageno = pageIndex;
                queryModel.comm.pagesize = limit;
                if (!String.IsNullOrEmpty(id))
                    queryModel.query.types = new String[1] { id };
                if (!String.IsNullOrEmpty(securityCodes))
                    queryModel.query.securitycodes = securityCodes.Split(',');
                queryModel.query.terms = HttpUtility.UrlDecode(term);//LQ 以前是"("+term+")"
                errorquery = JsonConvert.SerializeObject(queryModel);
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("请求条件为:" + errorquery + "新闻简单搜索出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 将以逗号分割的字符串转化为数组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private String[] GetParams(String param)
        {
            String[] result = new String[0];
            if (!String.IsNullOrEmpty(param))
            {
                result = param.Split(',');
            }
            return result;
        }

        /// <summary>
        /// 新通道高级搜索
        /// </summary>
        /// <param name="id"></param>
        /// <param name="term"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <param name="order"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public object GetNewsBySearch(String types, String securitycodes, String industrys,
                                String date, String title,
                                String text, String columnType, String pageIndex, String limit)
        {


            String terms = String.Empty;
            if (!String.IsNullOrEmpty(date))
            {
                //add by lym
                try
                {
                    date = date.Replace("99:99:99", "23:59:59");
                    String startdate = String.Empty;
                    String enddate = String.Empty;
                    startdate = Convert.ToDateTime(date.Substring(0, date.IndexOf(" TO ")).Split(new char[] { 'T' })[0]).ToString("yyyy-MM-dd") + "T00:00:00Z";
                    enddate = Convert.ToDateTime(date.Substring(date.IndexOf(" TO ") + 4).Split(new char[] { 'T' })[0]).ToString("yyyy-MM-dd") + "T23:59:59Z";
                    date = startdate + " TO " + enddate;
                }
                catch (Exception ex)
                {

                }
                terms += "datetime:[" + date + "]";
            }
            if (!String.IsNullOrEmpty(title))
            {
                if (!String.IsNullOrEmpty(terms))
                {
                    terms += "AND (title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(title)) + ")";
                }
                else
                {
                    terms += "(title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(title)) + ")";
                }
            }
            if (!String.IsNullOrEmpty(text))
            {
                if (!String.IsNullOrEmpty(terms))
                {
                    terms += "AND (text:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + " OR title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + ")";
                }
                else
                {
                    terms += "(text:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + " OR title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + ")";
                }

            }
            SktSrhModel<NewsSktSrhQryModel> queryModel = new SktSrhModel<NewsSktSrhQryModel>("H1", pageIndex, limit,
                                                                           new NewsSktSrhQryModel(GetParams(types), GetParams(securitycodes),
                                                                               GetParams(industrys), terms, columnType));
            String result = String.Empty;
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetNewsBySearch.do发生异常", ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取详细页的列表
        /// </summary>
        /// <param name="infocodes"></param>
        /// <returns></returns>
        public static String GetDetailList(String infocodes)
        {
            String result = String.Empty;
            SktByIdsModel queryModel = new SktByIdsModel("H1", infocodes.Split(','));
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetDetailList.do发生异常", ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取实时资讯详细信息
        /// </summary>
        public static String GetRealTimeInfoByCode(String infoCode)
        {
            String content = "";
            try
            {
                String url = "http://mainbody.jg.eastmoney.com/nrsweb/service.action?token=&serviceType=C&dataType=json&h=H1";
                url += "&id=" + infoCode;
                url += "&cid=" + "6615014352506680";
                WebRequest web = WebRequest.Create(url);
                HttpWebResponse res = (HttpWebResponse)web.GetResponse();
                res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream());
                content = sr.ReadToEnd();
                sr.Close();
                res.Close();
            }
            catch (Exception ex) { }
            return content;
        }
    }
}
