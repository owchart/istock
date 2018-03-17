using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Web;
using System.Net;

namespace OwLib
{
    /// <summary>
    /// 公告数据类
    /// </summary>
    public class NoticeDataHelper
    {
        /// <summary>
        /// 新通道-左侧菜单
        /// </summary>
        public static object GetLeftTree(String cid)
        {
            try
            {
                SocketModel obj = new SocketModel("H2");
                obj.cid = cid;
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(obj));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取公告菜单数据出错：", ex);
                throw ex;
            }
        }

        /// <summary>
        /// 新通道-根据id获取今日公告列表
        /// </summary>
        public static object GetDailyNotice(String id, String pageIndex, String limit, String order, String sort)
        {
            try
            {
                String date = CommDao.SafeToDateString(DateTime.Now.ToShortDateString(), "yyyy-MM-dd");
                String terms = "datetime: " + date + "T00:00:00Z TO " + date + "T99:99:99Z";
                return GetNoticeByParam(id, "", terms, pageIndex, limit, order, sort);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取今日公告列表数据获取出错：", ex);
                return null;
            }
        }
        /// <summary>
        /// 新通道-根据id获取公告列表
        /// </summary>
        public static object GetNoticeById(String id, String pageIndex, String limit, String order, String sort)
        {
            try
            {
                SocketModel obj = new SocketModel("H2", id, pageIndex, limit);
                obj.pageno = pageIndex.ToString();
                obj.pagesize = limit.ToString();
                obj.id = id;
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(obj));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("根据id获取公告列表数据获取出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新通道-简单搜索
        /// </summary>
        public static object GetNoticeByParam(String id, String securityCodes, String terms, String pageIndex, String limit, String order, String sort)
        {
            try
            {
                SktSrhModel<NoticeSktSrhQryModel> queryModel = new SktSrhModel<NoticeSktSrhQryModel>("H2", new NoticeSktSrhQryModel());
                queryModel.comm.pageno = pageIndex;
                queryModel.comm.pagesize = limit;
                if (!String.IsNullOrEmpty(id))
                    queryModel.query.types = new String[1] { id };
                if (!String.IsNullOrEmpty(securityCodes))
                    queryModel.query.securitycodes = securityCodes.Split(',');
                queryModel.query.terms = terms;
                object objRs = DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
                return objRs;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("公告简单搜索出错：", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取详细页的列表
        /// </summary>
        /// <param name="infocodes"></param>
        /// <returns></returns>
        public String GetDetailList(String infocodes)
        {
            String result = String.Empty;
            SktByIdsModel queryModel = new SktByIdsModel("H2", infocodes.Split(','));
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
        /// 新通道的高级搜索
        /// </summary>
        /// <param name="types">栏目id集合</param>
        /// <param name="securitycodes">证券代码集合</param>
        /// <param name="date">日期</param>
        /// <param name="title">标题</param>
        /// <param name="text">正文</param>
        /// <param name="columnType">栏目类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="limit">记录数</param>
        /// <returns></returns>
        public object GetNoticeBySearch(String types, String securitycodes,
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
            SktSrhModel<NoticeSktSrhQryModel> queryModel = new SktSrhModel<NoticeSktSrhQryModel>("H2", pageIndex, limit,
                                                                           new NoticeSktSrhQryModel(GetParams(types), GetParams(securitycodes), terms, columnType));
            String result = String.Empty;
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetNoticeBySearch.do发生异常", ex);
                throw ex;
            }
            return result;
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
        /// 获取实时资讯详细信息
        /// </summary>
        public static String GetRealTimeInfoByCode(String infoCode)
        {
            String content = "";
            try
            {
                String url = "http://mainbody.jg.eastmoney.com/nrsweb/service.action?token=&serviceType=C&dataType=json&h=H2";
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

    public class NoticeListRoot
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

        private List<NoticeListNode> m_records;

        public List<NoticeListNode> records
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

    public class NoticeListNodeBind
    {
        private String m_name;

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private String m_url;

        public String url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        private String m_date;

        public String Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private String m_id;

        public String Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_secuFullCode;

        public String secuFullCode
        {
            get { return m_secuFullCode; }
            set { m_secuFullCode = value; }
        }

        private String m_secuSName;

        public String secuSName
        {
            get { return m_secuSName; }
            set { m_secuSName = value; }
        }

        private String m_title;

        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public void Copy(NoticeListNode noticeNode)
        {
            try
            {
                this.Date = noticeNode.Date;
                this.Id = noticeNode.Id;
                this.Name = noticeNode.attach[0].Name;
                this.secuFullCode = noticeNode.secuList[0].secuFullCode;
                this.secuSName = noticeNode.secuList[0].secuSName;
                this.Title = noticeNode.Title;
                this.url = noticeNode.attach[0].url;
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class NoticeListNode
    {
        private List<NoticeAttach> m_attach;

        public List<NoticeAttach> attach
        {
            get { return m_attach; }
            set { m_attach = value; }
        }

        private String m_date;

        public String Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private String m_id;

        public String Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_importLevel;

        public String importLevel
        {
            get { return m_importLevel; }
            set { m_importLevel = value; }
        }

        private String m_order;

        public String Order
        {
            get { return m_order; }
            set { m_order = value; }
        }

        private List<NoticeSecu> m_secuList;

        public List<NoticeSecu> secuList
        {
            get { return m_secuList; }
            set { m_secuList = value; }
        }

        private String m_title;

        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }
    }

    public class NoticeAttach
    {
        private String m_filetype;

        public String filetype
        {
            get { return m_filetype; }
            set { m_filetype = value; }
        }

        private String m_name;

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private String m_pagenum;

        public String pagenum
        {
            get { return m_pagenum; }
            set { m_pagenum = value; }
        }

        private int m_seq;

        public int seq
        {
            get { return m_seq; }
            set { m_seq = value; }
        }

        private String m_url;

        public String url
        {
            get { return m_url; }
            set { m_url = value; }
        }
    }

    public class NoticeSecu
    {
        private String m_secuFullCode;

        public String secuFullCode
        {
            get { return m_secuFullCode; }
            set { m_secuFullCode = value; }
        }

        private String m_secuSName;

        public String secuSName
        {
            get { return m_secuSName; }
            set { m_secuSName = value; }
        }

        private String m_secuTypeCode;

        public String secuTypeCode
        {
            get { return m_secuTypeCode; }
            set { m_secuTypeCode = value; }
        }
    }
}
