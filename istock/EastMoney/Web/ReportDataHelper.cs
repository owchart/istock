using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Web;
using System.Net;

namespace OwLib
{
    /// <summary>
    /// 基础数据请求模板
    /// </summary>
    public class BaseDataSocketModel
    {
        public BaseDataSocketModel(String type)
        {
            this.request = "getBaseData";
            this.type = type;
        }
        public BaseDataSocketModel(String request, String type)
        {
            this.request = request;
            this.type = type;
        }

        public String request { get; set; }
        public String type { get; set; }
    }

    /// <summary>
    /// 研报数据类
    /// </summary>
    public class ReportDataHelper
    {
        /// <summary>
        /// 获取左侧研报树
        /// </summary>
        /// <returns></returns>
        public static String GetLeftTree(String cid)
        {
            String result = String.Empty;
            SocketModel queryModel = new SocketModel("H3", cid);
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetLeftTree.do发生异常", ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 根据树ID获取内页数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static String GetReportByTreeNode(String cid, String request, String id, String pageIndex, String limit, String sort,
                                        String order, String otherparams)
        {
            String result = String.Empty;
            //String terms = String.Empty;
            //if (!String.IsNullOrEmpty(date))
            //{
            //    terms += "datetime:[" + date + "T00:00:00Z TO 9999-99-99T99:99:99Z]";
            //}
            if (String.IsNullOrEmpty(otherparams))
            {
                otherparams = String.Empty;
            }
            SocketModel queryModel = new SocketModel(cid, request, "H3", id, otherparams, pageIndex, limit, sort, order);
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetReportByTreeNode.do发生异常", ex);
                throw ex;
            }

            return result;
        }



        /// <summary>
        /// 返回资讯高级搜索的Terms
        /// </summary>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="authornames"></param>
        /// <param name="authorgroup"></param>
        /// <returns></returns>
        private static String GetTerms(String date, String title, String text, String authornames, String authorgroup)
        {
            String terms = String.Empty;
            String authornamesstr = String.Empty;
            String authorgroupsstr = String.Empty;
            String authquery = String.Empty;
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
                    terms += " AND (title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(title)) + ")";
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
                    terms += " AND (text:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + " OR title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + ")";
                }
                else
                {
                    terms += "(text:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + " OR title:" + CommDao.SafeToIndexString(HttpUtility.UrlDecode(text)) + ")";
                }

            }
            foreach (String item in GetParams(authornames))
            {
                authornamesstr += " OR author:" + item;
            }
            foreach (String item in GetParams(authorgroup))
            {
                authorgroupsstr += " OR (" + item + ")";
            }
            if (!String.IsNullOrEmpty(authornamesstr))
            {
                authornamesstr = authornamesstr.Remove(0, 4);
            }
            if (!String.IsNullOrEmpty(authorgroupsstr))
            {
                if (!String.IsNullOrEmpty(authornamesstr))
                {
                    authquery = "(" + authornamesstr + ")" + authorgroupsstr;
                }
                else
                {
                    authquery = authorgroupsstr.Remove(0, 4); ;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(authornamesstr))
                {
                    authquery = authornamesstr;
                }
                else
                {
                    authquery = String.Empty;
                }
            }

            if (!String.IsNullOrEmpty(authquery))
            {
                if (!String.IsNullOrEmpty(terms))
                {
                    terms += " AND (" + authquery + ")";
                }
                else
                {
                    terms += authquery;
                }

            }
            return terms;
        }

        /// <summary>
        /// 支持query数组的高级查询 query里面参数用|作为分隔符
        /// </summary>
        /// <param name="types"></param>
        /// <param name="orgs"></param>
        /// <param name="authors"></param>
        /// <param name="authornames"></param>
        /// <param name="authorgroup"></param>
        /// <param name="securitycodes"></param>
        /// <param name="industrys"></param>
        /// <param name="concepts"></param>
        /// <param name="ratings"></param>
        /// <param name="ratingchanges"></param>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="columnType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <param name="MultiQueryLength">高级搜索的条件个数</param>
        /// <returns></returns>
        public static String SearchByMultiQuery(String types, String orgs, String authors, String authornames, String authorgroup, String securitycodes, String industrys,
                                String concepts, String ratings, String ratingchanges, String date, String title,
                                String text, String columnType, String pageIndex, String limit, int MultiQueryLength)
        {
            List<ReportSktSrhQryModel> reportQryList = new List<ReportSktSrhQryModel>();
            String[] terms = new String[MultiQueryLength];
            for (int i = 0; i < MultiQueryLength; i++)
            {
                terms[i] = GetTerms(GetParams(date, i), GetParams(title, i), GetParams(text, i), GetParams(authornames, i), GetParams(authorgroup, i));
                reportQryList.Add(new ReportSktSrhQryModel(GetParamsByMultiQuery(types, i), GetParamsByMultiQuery(orgs, i),
                                                                                   GetParamsByMultiQuery(authors, i), GetParamsByMultiQuery(securitycodes, i),
                                                                                   GetParamsByMultiQuery(industrys, i), GetParamsByMultiQuery(concepts, i),
                                                                                   GetParamsByMultiQuery(ratings, i), GetParamsByMultiQuery(ratingchanges, i), terms[i], GetParams(columnType, i)));
            }
            SktSrhModelByMultiQuery<ReportSktSrhQryModel> queryModel = new SktSrhModelByMultiQuery<ReportSktSrhQryModel>("H3", pageIndex, limit, reportQryList);
            String result = String.Empty;
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SearchByMultiQuery.do发生异常,参数信息：types=" + types + ",orgs=" + orgs +
                    ",authors=" + authors + ",authornames=" + authornames + ",authorgroup=" + authorgroup +
                    ",securitycodes=" + securitycodes + ",industrys=" + industrys + ",concepts" + concepts +
                    ",ratings=" + ratings + ",ratingchanges=" + ratingchanges + ",date=" + date + ",title=" + title +
                    ",text=" + text + ",columnType=" + columnType + ",pageIndex=" + pageIndex + ",limit=" + limit +
                    ",MultiQueryLength=" + MultiQueryLength, ex);
                //throw ex;
            }

            return result;
        }

        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <param name="types"></param>
        /// <param name="orgs"></param>
        /// <param name="authors"></param>
        /// <param name="securitycodes"></param>
        /// <param name="industrys"></param>
        /// <param name="concepts"></param>
        /// <param name="ratings"></param>
        /// <param name="ratingchanges"></param>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static String Search(String types, String orgs, String authors, String authornames, String authorgroup, String securitycodes, String industrys,
                                String concepts, String ratings, String ratingchanges, String date, String title,
                                String text, String columnType, String pageIndex, String limit)
        {
            String terms = String.Empty;
            terms = GetTerms(date, title, text, authornames, authorgroup);
            SktSrhModel<ReportSktSrhQryModel> queryModel = new SktSrhModel<ReportSktSrhQryModel>("H3", pageIndex, limit,
                                                                           new ReportSktSrhQryModel(GetParams(types), GetParams(orgs),
                                                                               GetParams(authors), GetParams(securitycodes),
                                                                               GetParams(industrys), GetParams(concepts),
                                                                               GetParams(ratings), GetParams(ratingchanges), terms, columnType));
            String result = String.Empty;
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Search.do发生异常", ex);
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
            //String terms = String.Empty;
            //foreach (String item in infocodes.Split(','))
            //{
            //    terms += " OR infocode:" + item;
            //}
            //if (!String.IsNullOrEmpty(terms))
            //{
            //    terms = terms.Remove(0, 4);
            //    terms = "(" + terms + ")";
            //}

            //SktSrhModel<ReportSktSrhQryModel> queryModel = new SktSrhModel<ReportSktSrhQryModel>("H3", "1", "50", "datetime", "desc",
            //                                                            new ReportSktSrhQryModel(terms, "detailColumn"));
            SktByIdsModel queryModel = new SktByIdsModel("H3", infocodes.Split(','));
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
        /// 获取基础数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String GetBaseData(String type)
        {
            String result = String.Empty;

            BaseDataSocketModel queryModel = new BaseDataSocketModel(type);
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
                result = result.Replace("\t", String.Empty);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetBaseData.do发生异常", ex);
                throw ex;
            }
            //return Json(result, JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// 获取统计下面的研报
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="otherparams"></param>
        /// <returns></returns>
        public static String GetReportStatisticList(String id, String code, String industrycode, String orgcode, String timestamp,
            String pageIndex, String limit, String sort, String order, String otherparams)
        {
            String result = String.Empty;

            object queryModel = new
            {
                request = "getReportStatistic",
                id = String.IsNullOrEmpty(id) ? "" : id,
                code = String.IsNullOrEmpty(code) ? "" : code,
                industrycode = String.IsNullOrEmpty(industrycode) ? "" : industrycode,
                orgcode = String.IsNullOrEmpty(orgcode) ? "" : orgcode,
                otherparams = String.IsNullOrEmpty(otherparams) ? "" : otherparams,
                timestamp = String.IsNullOrEmpty(timestamp) ? "" : timestamp,
                pageno = pageIndex,
                pagesize = limit
            };
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
                result = result.Replace("\t", String.Empty);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetReportStatisticList.do发生异常", ex);
                throw ex;
            }
            return result;

        }

        /// <summary>
        /// 获取自选股研报列表
        /// </summary>
        public static String GetReportBySelectStock(String cid, String request, String id, String pageIndex, String limit, String sort,
                                        String order, String codes)
        {
            String result = String.Empty;
            var queryModel = new { request = "getQueryUserSecu", h = "H3", pageNo = pageIndex, pageSize = limit, codes = codes.Split(',') };
            try
            {
                result = (String)DataAccess.QueryIndex(JsonConvert.SerializeObject(queryModel));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetReportBySelectStock.do发生异常", ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 将以逗号分割的字符串转化为数组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static String[] GetParams(String param)
        {
            String[] result = new String[0];
            if (!String.IsNullOrEmpty(param))
            {
                result = param.Split(',');
            }
            return result;
        }

        /// <summary>
        /// 通过索引位置以及分隔符来获取参数
        /// </summary>
        /// <param name="param">原始内容包含分隔符的字符串</param>
        /// <param name="index">返回分割后索引位置数据</param>
        /// <param name="splitchar">分隔符 默认|</param>
        /// <returns></returns>
        private static String GetParams(String param, int index, char splitchar = '|')
        {
            String result = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(param))
                {
                    result = param.Split(splitchar)[index];
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetParams.do发生异常", ex);
            }
            return result;
        }

        /// <summary>
        /// 为了将高级搜索中MultiQuery先做|分割处理，再将以逗号分割的字符串转化为数组
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="index">获取索引位置</param>
        /// <param name="splitchar">分隔符 默认|</param>
        /// <returns></returns>
        private static String[] GetParamsByMultiQuery(String param, int index, char splitchar = '|')
        {
            String[] result = new String[0];
            try
            {
                if (!String.IsNullOrEmpty(param))
                {
                    String[] paramArray = param.Split(splitchar);
                    if (paramArray.Length >= index && !String.IsNullOrEmpty(paramArray[index]))
                    {
                        result = paramArray[index].Split(',');
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetParamsByMultiQuery.do发生异常,参数信息：param=" + param + ",index=" + index + ",splitchar=" + splitchar, ex);
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
                String url = "http://mainbody.jg.eastmoney.com/nrsweb/service.action?token=&serviceType=C&dataType=json&h=H3";
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

    public class ReportListRoot
    {
        private String m_h;

        public String H
        {
            get { return m_h; }
            set { m_h = value; }
        }

        private List<ReportListNode> m_records;

        public List<ReportListNode> records
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

    public class ReportListNodeBind
    {
        private String m_date;

        public String date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private String m_id;

        public String id
        {
            get { return m_id; }
            set { m_id = value; }
        }


        private String m_org;

        public String org
        {
            get { return m_org; }
            set { m_org = value; }
        }

        private String m_reportDate;

        public String reportDate
        {
            get { return m_reportDate; }
            set { m_reportDate = value; }
        }

        private String m_rtype;

        public String rtype
        {
            get { return m_rtype; }
            set { m_rtype = value; }
        }
        private String m_title;

        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        private String m_url;

        public String Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public void Copy(ReportListNode reportNode)
        {
            try
            {
                this.id = reportNode.id;
                this.date = reportNode.date;
                this.org = reportNode.org;
                this.reportDate = reportNode.reportDate;
                this.rtype = reportNode.rtype;
                this.Title = reportNode.Title;
                this.Url = reportNode.attach[0].url;
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class ReportListNode
    {
        private List<ReportAttach> m_attach;

        public List<ReportAttach> attach
        {
            get { return m_attach; }
            set { m_attach = value; }
        }

        private List<ReportAuth> m_authorList;

        public List<ReportAuth> authorList
        {
            get { return m_authorList; }
            set { m_authorList = value; }
        }

        private String m_change;

        public String change
        {
            get { return m_change; }
            set { m_change = value; }
        }

        private String m_code;

        public String code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        private String m_codename;

        public String codename
        {
            get { return m_codename; }
            set { m_codename = value; }
        }

        private String m_cprice;

        public String cprice
        {
            get { return m_cprice; }
            set { m_cprice = value; }
        }

        private String m_date;

        public String date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private String m_dprice;

        public String dprice
        {
            get { return m_dprice; }
            set { m_dprice = value; }
        }

        private String m_id;

        public String id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_industry;

        public String industry
        {
            get { return m_industry; }
            set { m_industry = value; }
        }

        private String m_industrycode;

        public String industrycode
        {
            get { return m_industrycode; }
            set { m_industrycode = value; }
        }

        private String m_isread;

        public String isread
        {
            get { return m_isread; }
            set { m_isread = value; }
        }

        private String m_kcode;

        public String kcode
        {
            get { return m_kcode; }
            set { m_kcode = value; }
        }

        private String m_kname;

        public String kname
        {
            get { return m_kname; }
            set { m_kname = value; }
        }

        private String m_ktype;

        public String ktype
        {
            get { return m_ktype; }
            set { m_ktype = value; }
        }

        private String m_order;

        public String order
        {
            get { return m_order; }
            set { m_order = value; }
        }

        private String m_org;

        public String org
        {
            get { return m_org; }
            set { m_org = value; }
        }

        private String m_orgcode;

        public String orgcode
        {
            get { return m_orgcode; }
            set { m_orgcode = value; }
        }

        private String m_orgprizeinfo;

        public String orgprizeinfo
        {
            get { return m_orgprizeinfo; }
            set { m_orgprizeinfo = value; }
        }

        private String m_rate;

        public String rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }

        private String m_reportDate;

        public String reportDate
        {
            get { return m_reportDate; }
            set { m_reportDate = value; }
        }

        private String m_rtype;

        public String rtype
        {
            get { return m_rtype; }
            set { m_rtype = value; }
        }

        private String m_rtypecode;

        public String rtypecode
        {
            get { return m_rtypecode; }
            set { m_rtypecode = value; }
        }

        private String m_space;

        public String space
        {
            get { return m_space; }
            set { m_space = value; }
        }

        private String m_sratingName;

        public String sratingName
        {
            get { return m_sratingName; }
            set { m_sratingName = value; }
        }

        private String m_title;

        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }
    }

    public class ReportAttach
    {
        private String m_fileSize;

        public String fileSize
        {
            get { return m_fileSize; }
            set { m_fileSize = value; }
        }

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

    public class ReportAuth
    {
        private String m_auth;

        public String auth
        {
            get { return m_auth; }
            set { m_auth = value; }
        }

        private String m_authcode;

        public String authcode
        {
            get { return m_authcode; }
            set { m_authcode = value; }
        }

        private String m_authprizeinfo;

        public String authprizeinfo
        {
            get { return m_authprizeinfo; }
            set { m_authprizeinfo = value; }
        }
    }
}
