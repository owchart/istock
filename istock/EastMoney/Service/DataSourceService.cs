using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmCore;

namespace OwLib
{
    public class DataSourceService
    {
        /// <summary>
        /// 所有无风险利率表的名称
        /// </summary>
        public const String SEV_ALLRFINTRATE = "SEV_ALLRFINTRATE";

        /// <summary>
        /// 市场类型表的名称
        /// </summary>
        public const String SPTM_MARKETRELATION = "SPTM_MARKETRELATION";

        /// <summary>
        /// 交易日期表的名称
        /// </summary>
        public const String MOD_TRADEDATE = "MOD_TRADEDATE";

        /// <summary>
        /// 机构列表的源名称
        /// </summary>
        public const String BROKERCOM = "BROKERCOM";

        /// <summary>
        /// 行业类别的源名称
        /// </summary>
        public const String FND_HYLB = "FND_HYLB";

        /// <summary>
        /// 板块树
        /// </summary>
        public const String BLK_BLOCKTREE = "BLK_BLOCKTREE";

        /// <summary>
        /// 所有指标
        /// </summary>
        public const String ALLINDEX = "ALLINDEX";

        /// <summary>
        /// 专题源一
        /// </summary>
        public const String MOD_SS = "MOD_SS";

        /// <summary>
        /// 专题源二
        /// </summary>
        public const String MOD_STC_ALL = "MOD_STC_ALL";

        /// <summary>
        /// 中国宏观的源名称组
        /// </summary>
        public const String EDB_MACRO_INDICATOR = "EDB_MACRO_INDICATOR";
        public const String EDB_INDUSTRY_INDICATOR = "EDB_INDUSTRY_INDICATOR";
        public const String EDB_GLOBAL_INDICATOR = "EDB_GLOBAL_INDICATOR";
        public const String YIEL_STANDARDYC_TREE = "YIEL_STANDARDYC_TREE";

        /// <summary>
        /// 宏观预测源
        /// </summary>
        public const String MAC_FORECAST_TYPETREE = "MAC_FORECAST_TYPETREE";
        public const String IND_EB_INDUSTRYNAME = "IND_EB_INDUSTRYNAME";
        public const String IND_EB_INDUSTRYVALUEZB = "IND_EB_INDUSTRYVALUEZB";

        /// <summary>
        /// 宏观多图图表
        /// </summary>
        public const String MAC_JJZBTB = "MAC_JJZBTB";

        /// <summary>
        /// 宏观多图图表统计
        /// </summary>
        public const String MAC_JJZBTB_TJ = "MAC_JJZBTB_TJ";

        /// <summary>
        /// 宏观预测年份
        /// </summary>
        public const String IND_EB_INDUSTRYVALUEQ = "IND_EB_INDUSTRYVALUEQ";

        /// <summary>
        /// 新的键盘精灵
        /// </summary>
        public const String IND_JPJLNEW = "IND_JPJLNEW";

        /// <summary>
        /// 未知用途源
        /// </summary>
        public const String IND_EPESV_VALUE_H = "IND_EPESV_VALUE_H";

        /// <summary>
        /// 期货行业数据库
        /// </summary>
        public const String EDB_FUTURE_INDICATOR = "EDB_FUTURE_INDICATOR";

        /// <summary>
        /// 机构列表
        /// </summary>
        public const String IND_BROKERCOM = "IND_BROKERCOM";

        /// <summary>
        /// 东财指数
        /// </summary>
        public const String IND_EMINDEX = "IND_EMINDEX";

        /// <summary>
        /// 指数分析树
        /// </summary>
        public const String ISPE_BLOCKTREE = "ISPE_BLOCKTREE";

        /// <summary>
        /// 交易日查询条件对应源
        /// </summary>
        private const String MOD_DATERE = "MOD_DATERE";

        /// <summary>
        /// 键盘精灵新源
        /// </summary>
        private const String IND_JPJLTEST = "IND_JPJLTEST";

        /// <summary>
        /// 最新报告期
        /// </summary>
        private const String MOD_MAXREPORTDATE = "MOD_MAXREPORTDATE";

        /// <summary>
        /// 下拉框报告期
        /// </summary>
        private const String MOD_REPORTTYPEXLK = "MOD_REPORTTYPEXLK";

        /// <summary>
        ///板块监控成份
        /// </summary>
        private const String IND_MEINDEXBKJK = "IND_MEINDEXBKJK";

        /// <summary>
        /// 盈利预测专题研报搜索机构
        /// </summary>
        private const String SPE_RESA = "SPE_RESA";

        /// <summary>
        /// 宏观预测首页数据
        /// </summary>
        private const String MAC_FORECAST_SHOUYE = "MAC_FORECAST_SHOUYE";

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
