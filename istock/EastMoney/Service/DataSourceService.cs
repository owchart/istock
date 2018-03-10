using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmCore;
using EastMoney.FM.Web.Data;

namespace dataquery.Service
{
    public class DataSourceService
    {
        /// <summary>
        /// 所有无风险利率表的名称
        /// </summary>
        public const string SEV_ALLRFINTRATE = "SEV_ALLRFINTRATE";

        /// <summary>
        /// 市场类型表的名称
        /// </summary>
        public const string SPTM_MARKETRELATION = "SPTM_MARKETRELATION";

        /// <summary>
        /// 交易日期表的名称
        /// </summary>
        public const string MOD_TRADEDATE = "MOD_TRADEDATE";

        /// <summary>
        /// 机构列表的源名称
        /// </summary>
        public const string BROKERCOM = "BROKERCOM";

        /// <summary>
        /// 行业类别的源名称
        /// </summary>
        public const string FND_HYLB = "FND_HYLB";

        /// <summary>
        /// 板块树
        /// </summary>
        public const string BLK_BLOCKTREE = "BLK_BLOCKTREE";

        /// <summary>
        /// 所有指标
        /// </summary>
        public const string ALLINDEX = "ALLINDEX";

        /// <summary>
        /// 专题源一
        /// </summary>
        public const string MOD_SS = "MOD_SS";

        /// <summary>
        /// 专题源二
        /// </summary>
        public const string MOD_STC_ALL = "MOD_STC_ALL";

        /// <summary>
        /// 中国宏观的源名称组
        /// </summary>
        public const string EDB_MACRO_INDICATOR = "EDB_MACRO_INDICATOR";
        public const string EDB_INDUSTRY_INDICATOR = "EDB_INDUSTRY_INDICATOR";
        public const string EDB_GLOBAL_INDICATOR = "EDB_GLOBAL_INDICATOR";
        public const string YIEL_STANDARDYC_TREE = "YIEL_STANDARDYC_TREE";

        /// <summary>
        /// 宏观预测源
        /// </summary>
        public const string MAC_FORECAST_TYPETREE = "MAC_FORECAST_TYPETREE";
        public const string IND_EB_INDUSTRYNAME = "IND_EB_INDUSTRYNAME";
        public const string IND_EB_INDUSTRYVALUEZB = "IND_EB_INDUSTRYVALUEZB";

        /// <summary>
        /// 宏观多图图表
        /// </summary>
        public const string MAC_JJZBTB = "MAC_JJZBTB";

        /// <summary>
        /// 宏观多图图表统计
        /// </summary>
        public const string MAC_JJZBTB_TJ = "MAC_JJZBTB_TJ";

        /// <summary>
        /// 宏观预测年份
        /// </summary>
        public const string IND_EB_INDUSTRYVALUEQ = "IND_EB_INDUSTRYVALUEQ";

        /// <summary>
        /// 新的键盘精灵
        /// </summary>
        public const string IND_JPJLNEW = "IND_JPJLNEW";

        /// <summary>
        /// 未知用途源
        /// </summary>
        public const string IND_EPESV_VALUE_H = "IND_EPESV_VALUE_H";

        /// <summary>
        /// 期货行业数据库
        /// </summary>
        public const string EDB_FUTURE_INDICATOR = "EDB_FUTURE_INDICATOR";

        /// <summary>
        /// 机构列表
        /// </summary>
        public const string IND_BROKERCOM = "IND_BROKERCOM";

        /// <summary>
        /// 东财指数
        /// </summary>
        public const string IND_EMINDEX = "IND_EMINDEX";

        /// <summary>
        /// 指数分析树
        /// </summary>
        public const string ISPE_BLOCKTREE = "ISPE_BLOCKTREE";

        /// <summary>
        /// 交易日查询条件对应源
        /// </summary>
        private const string MOD_DATERE = "MOD_DATERE";

        /// <summary>
        /// 键盘精灵新源
        /// </summary>
        private const string IND_JPJLTEST = "IND_JPJLTEST";

        /// <summary>
        /// 最新报告期
        /// </summary>
        private const string MOD_MAXREPORTDATE = "MOD_MAXREPORTDATE";

        /// <summary>
        /// 下拉框报告期
        /// </summary>
        private const string MOD_REPORTTYPEXLK = "MOD_REPORTTYPEXLK";

        /// <summary>
        ///板块监控成份
        /// </summary>
        private const string IND_MEINDEXBKJK = "IND_MEINDEXBKJK";

        /// <summary>
        /// 盈利预测专题研报搜索机构
        /// </summary>
        private const string SPE_RESA = "SPE_RESA";

        /// <summary>
        /// 宏观预测首页数据
        /// </summary>
        private const string MAC_FORECAST_SHOUYE = "MAC_FORECAST_SHOUYE";

        /// <summary>
        /// 获取所有的数据源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataSet GetDataSource(String name)
        {
            List<DataTransmission> requests = new List<DataTransmission>();
            DataTransmission dt1 = new DataTransmission();
            dt1.DataSource = name;
            dt1.DataTableName = name;
            dt1.Orders = new List<Order>();
            dt1.MaxResult = Int32.MaxValue;
            requests.Add(dt1);
            return DataAccess.IDataQuery.Query(requests);
        }
    }
}
