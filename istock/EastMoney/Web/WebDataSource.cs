using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 数据源
    /// </summary> 
    public enum WebDataSource
    {
        #region 研报
        /// <summary>
        /// 研报数据源
        /// </summary>
        REP_SRTTBOI,
        /// <summary>
        /// 证券码表数据源
        /// </summary>
        IND_SECUINFO,
        /// <summary>
        /// 作者
        /// </summary>
        REP_RRSO,
        /// <summary>
        /// 机构
        /// </summary>
        REP_RRSA,
        /// <summary>
        /// 研报搜索股票
        /// </summary>
        REP_RRSS,
        /// <summary>
        /// 研报基础信息概念
        /// </summary>
        REP_SRTTBOTCOI,
        /// <summary>
        /// 摘要
        /// </summary>
        REP_SRSUMARY,
        /// <summary>
        /// 概念基础表
        /// </summary>
        REP_CONCEPTBASE,
        /// <summary>
        /// 公司研报预测
        /// </summary>
        REP_COMSTRFORE,
        /// <summary>
        /// 股票日交易收盘价
        /// </summary>
        SP9_QUOTATIONS,
        /// <summary>
        /// 板块树 (行业)
        /// </summary>
        BLK_BLOCKTREE,
        /// <summary>
        /// 机构关注源  188
        /// </summary>
        REP_STATIC_PUBIMP,
        REP_SRTTBOI5,
        /// <summary>
        /// 单个机构研究分布 188
        /// </summary>
        REP_STATIC_COMIMP,
        REP_SRTTBOI6,
        /// <summary>
        /// 个股机构关注
        /// </summary>
        REP_STATIC_STOCKIMP,
        REP_SRTTBOI1,
        /// <summary>
        /// 一直看多
        /// </summary>
        REP_STATIC_PINGJI,
        REP_SRTTBOI2,
        /// <summary>
        /// 一直看空
        /// </summary>
        REP_STATIC_PINGJIDI,
        REP_SRTTBOI3,
        /// <summary>
        /// 机构分歧
        /// </summary>
        REP_STATIC_PINGJIGAODI,
        REP_SRTTBOI4,
        /// <summary>
        /// 研报的点击量
        /// </summary>
        REP_Hits,
        /// <summary>
        /// 研报的点击量统计表
        /// </summary>
        REP_HitsStatistics,
        /// <summary>
        /// 研报的点击量
        /// </summary>
        Web_REP_Hits,
        /// <summary>
        /// 研报的点击量统计表
        /// </summary>
        Web_REP_HitsStatistics,
        /// <summary>
        /// 新财富
        /// </summary>
        REP_NEWFORTUNE,
        /// <summary>
        /// 个股最新收盘价
        /// </summary>
        REP_STOCKPRICE,
        #endregion
        #region 公告
        /// <summary>
        /// 公告列表
        /// </summary>
        NOT_NOTICEBASE,
        /// <summary>
        /// 公告正文
        /// </summary>
        NOT_NOTICECONTENT,
        /// <summary>
        /// 公告收藏
        /// </summary>
        NOT_Store,
        Web_Notice_Store,
        Web_News_Store,
        Web_News_UserConfig,
        Web_News_Hits,
        /// <summary>
        /// 公告订阅
        /// </summary>
        NOT_Sub,
        Web_Notice_Sub,
        Web_News_Sub,
        /// <summary>
        /// 公告树
        /// </summary>
        NOT_NOTICECOLUMN,
        /// <summary>
        /// 板块股票对应关系
        /// </summary>
        BLK_BLOCKSECURITY,
        /// <summary>
        /// 证券代码表
        /// </summary>
        NOT_SECUCODE,
        /// <summary>
        /// 新闻栏目
        /// </summary>
        NEWS_CHANELRELA,
        /// <summary>
        /// 行业新闻条件
        /// </summary>
        INDUSTRY_STEEL_XWZX,
        /// <summary>
        /// 行业新闻条件(改版)
        /// </summary>
        NEWS_INDUTONEWSCODE,
        #endregion
        #region 法律法规
        /// <summary>
        /// 法律法规基础表
        /// </summary>
        LAWS_SOURCEBASE,
        /// <summary>
        /// 法律法规信息表
        /// </summary>
        LAWS_LAWSBASE,
        /// <summary>
        /// 法律法规收藏
        /// </summary>
        LAWS_STORE,
        Web_Law_Store,
        #endregion
        #region 股市日历
        /// <summary>
        /// 首页日历
        /// </summary>
        SPE_STOCKCALENDAR_CAL,
        /// <summary>
        /// 首页日历图标
        /// </summary>
        SPE_STOCKCALENDAR_ICON,
        /// <summary>
        /// 停牌
        /// </summary>
        SPE_STOCKCALENDAR_WARNING,
        /// <summary>
        /// 持股变动
        /// </summary>
        SPE_STOCKCALENDAR_SHARECHAGE,
        /// <summary>
        /// 大宗交易
        /// </summary>
        SPE_STOCKCALENDAR_TRADING,
        /// <summary>
        /// 业绩预测
        /// </summary>
        SPE_STOCKCALENDAR_PREDICT,
        /// <summary>
        /// 财报披露
        /// </summary>
        SPE_STOCKCALENDAR_YG,//旧
        SPE_STOCKCALENDAR_CBPL,//新
        /// <summary>
        /// 限售股解禁
        /// </summary>
        SPE_STOCKCALENDAR_LIFTING,
        /// <summary>
        /// 发行分配
        /// </summary>
        SPE_STOCKCALENDAR_ALLOT,
        /// <summary>
        /// 公告
        /// </summary>
        SPE_STOCKCALENDER_INFO,
        /// <summary>
        /// 事件订阅
        /// </summary>
        SPE_Event_Sub,
        #endregion
        #region 行业
        #region 房地产
        #region 宏观影响研究
        /// <summary>
        /// 全国城镇化率走势
        /// </summary>
        MAC_ESTATE_CITYRATE,
        /// <summary>
        /// 地区性城镇化发展研究
        /// </summary>
        MAC_ESTATE_REGIONCITYRATE,
        /// <summary>
        /// 开发投资与资金来源累计同比走势
        /// </summary>
        MAC_ESTATE_INVESTANDMONEY,
        /// <summary>
        /// 开发投资与商品房施工面积、新开工面积累计同比走势
        /// </summary>
        MAC_ESTATE_INVESTWITHINSTRUCT,
        /// <summary>
        /// 土地购置费用占开发投资比重走势
        /// </summary>
        //MAC_ESTATE_FIELDTURNOVERPRECENTAGE,
        MAC_ESTATE_RFIELDTURNOVER,
        /// <summary>
        /// 商品房新开工面积与资金来源累计同比走势
        /// </summary>
        MAC_ESTATE_NEWAREAANDMONEY,
        /// <summary>
        /// 商品房新开工面积与销售面积累计同比走势
        /// </summary>
        MAC_ESTATE_INSTRUCTANDSALE,
        /// <summary>
        /// 商品房销售金额与M2同比走势/商品房销售金额与M1同比走势
        /// </summary>
        MAC_ESTATE_SALEANDMOMT,
        /// <summary>
        /// 指标——房地产开发投资完成额
        /// </summary>
        MAC_ESTATE_INVESTMENT,
        /// <summary>
        /// 房地产开发投资完成额:住宅
        /// </summary>
        MAC_ESTATE_HOUSEINVESTMENT,
        /// <summary>
        /// 商品房新开工面积
        /// </summary>
        MAC_ESTATE_NEWAREA,
        /// <summary>
        /// 商品住宅新开工面积
        /// </summary>
        MAC_ESTATE_NEWHOUSEAREA,
        /// <summary>
        /// 商品房施工面积
        /// </summary>
        MAC_ESTATE_INSTRUCTAREA,
        /// <summary>
        /// 商品住宅施工面积
        /// </summary>
        MAC_ESTATE_INSTRUCTHOUSEAREA,
        /// <summary>
        /// 商品房竣工面积
        /// </summary>
        MAC_ESTATE_ENDAREA,
        /// <summary>
        /// 商品住宅竣工面积
        /// </summary>
        MAC_ESTATE_ENDHOUSEAREA,
        /// <summary>
        /// 资金来源
        /// </summary>
        MAC_ESTATE_MONEY,
        /// <summary>
        /// 土地购置费用
        /// </summary>
        MAC_ESTATE_FIELDMONEY,
        /// <summary>
        /// 土地购置面积
        /// </summary>
        MAC_ESTATE_FIELDAREA,
        /// <summary>
        /// 商品房销售金额
        /// </summary>
        MAC_ESTATE_TURNOVERB,
        /// <summary>
        /// 商品住宅销售金额
        /// </summary>
        MAC_ESTATE_HOUSETURNOVER,
        /// <summary>
        /// 商品房销售面积
        /// </summary>
        MAC_ESTATE_SALEAREAB,
        /// <summary>
        /// 商品住宅销售面积
        /// </summary>
        MAC_ESTATE_SALEHOUSEAREAB,

        #endregion

        #region 证券市场跟踪
        /// <summary>
        /// 市场表现
        /// </summary>
        MAC_ESTATE_INDEX,
        /// <summary>
        /// 行业涨幅排名
        /// </summary>
        MAC_ESTATE_INDEXT,
        /// <summary>
        /// 成分股涨幅排名
        /// </summary>
        MAC_ESTATE_SECURITYRANKING,
        #endregion

        #region 行业深度分析
        /// <summary>
        /// 销售基本面—成交面积
        /// </summary>
        INDUSTRY_ESTATE_AREA,
        /// <summary>
        /// 销售基本面—成交价格1
        /// </summary>
        INDUSTRY_ESTATE_PRICE1,
        /// <summary>
        /// 销售基本面—成交价格2
        /// </summary>
        INDUSTRY_ESTATE_PRICE2,

        #endregion 土地市场

        /// <summary>
        /// 土地市场总览
        /// </summary>
        MAC_ESTATE_FIELD,
        /// <summary>
        /// 推出土地拓展页面
        /// </summary>
        MAC_ESTATE_PROVIDEDFIELDMORE,
        #region
        #endregion

        #endregion

        #region 钢铁行业
        /// <summary>
        /// 行业板块统计
        /// </summary>
        INDUSTRY_STEEL_HYBKTJ,
        /// <summary>
        /// 行业相关个股
        /// </summary>
        INDUSTRY_STEEL_XGGG,
        #region 宏观经济概览
        /// <summary>
        /// 宏观经济指标
        /// </summary>
        INDUSTRY_STEEL_MFI_INDEX,
        /// <summary>
        /// 宏观金融指标_货币供应量
        /// </summary>
        INDUSTRY_STEEL_MFI_CURRENCY,
        /// <summary>
        /// 宏观金融指标_贷款利率
        /// </summary>
        INDUSTRY_STEEL_MFI_LOANRATE,
        /// <summary>
        /// 宏观金融指标_贴现利率
        /// </summary>
        INDUSTRY_STEEL_MFI_BILL,
        #endregion

        #region 行业中观
        /// <summary>
        /// 钢铁行业固定资产投资完成额
        /// </summary>
        INDUSTRY_STEEL_MEDIUM_FAI,
        /// <summary>
        /// 钢铁行业工业增加值
        /// </summary>
        INDUSTRY_STEEL_MEDIUM_IAV,
        /// <summary>
        /// 钢铁行业施工投产项目
        /// </summary>
        INDUSTRY_STEEL_MEDIUM_PROJECT,
        /// <summary>
        /// 钢铁行业城镇固定资产投资
        /// </summary>
        INDUSTRY_STEEL_MEDIUM_TFAI,
        /// <summary>
        /// PMI指数
        /// </summary>
        INDUSTRY_STEEL_PMI,
        #endregion

        #region 行业财务指标
        /// <summary>
        /// 钢铁行业财务指标
        /// </summary>
        INDUSTRY_STEEL_FINANCIALIND,
        /// <summary>
        /// 钢铁行业财务比率
        /// </summary>
        INDUSTRY_STEEL_FINANCIALRAT,
        #endregion

        #region  行业产品量价数据
        #region 产品价格
        /// <summary>
        /// MYIPIC铁矿石价格指数
        /// </summary>
        INDUSTRY_STEEL_MYIPIC,
        /// <summary>
        /// 中国铁矿石价格指数
        /// </summary>
        INDUSTRY_STEEL_CIOPI,
        /// <summary>
        /// 进口铁矿石价格
        /// </summary>
        INDUSTRY_STEEL_IMPORTIOP,
        /// <summary>
        /// 国产铁矿石价格
        /// </summary>
        INDUSTRY_STEEL_MIOPRICE,
        /// <summary>
        /// 交易所钢材价格
        /// </summary>
        INDUSTRY_STEEL_EXCHANGEPRICE,
        /// <summary>
        /// 国际钢铁价格指数
        /// </summary>
        INDUSTRY_STEEL_GSPRICEINDEX,
        /// <summary>
        /// 国内钢材价格指数
        /// </summary>
        INDUSTRY_STEEL_CSPI,
        /// <summary>
        /// 钢材现货价格
        /// </summary>
        INDUSTRY_STEEL_CSSPOTPRICE,
        /// <summary>
        /// 主要城市钢材价格_线材
        /// </summary>
        INDUSTRY_STEEL_SCPRICE_XC,
        /// <summary>
        /// 主要城市钢材价格_板材
        /// </summary>
        INDUSTRY_STEEL_CSPRICE_BC,
        /// <summary>
        /// 主要城市钢材价格_型材
        /// </summary>
        INDUSTRY_STEEL_CSPRICE_XINGC,
        /// <summary>
        /// 主要城市钢材价格_钢坯
        /// </summary>
        INDUSTRY_STEEL_CSPRICE_GP,
        /// <summary>
        /// 主要城市钢材价格_废钢
        /// </summary>
        INDUSTRY_STEEL_CSPRICE_FG,
        #endregion

        #region 产品产销量
        /// <summary>
        /// 钢铁主要产品产量
        /// </summary>
        INDUSTRY_STEEL_MPRODUCTION,
        /// <summary>
        /// 钢材销量月度统计
        /// </summary>
        INDUSTRY_STEEL_MONTHSALE,
        /// <summary>
        /// 销量季度统计
        /// </summary>
        INDUSTRY_STEEL_QUARTERSALE,
        /// <summary>
        /// 钢材进口
        /// </summary>
        INDUSTRY_STEEL_INPORT,
        /// <summary>
        /// 钢材出口
        /// </summary>
        INDUSTRY_STEEL_EXPORT,
        #endregion

        #region 产品库存
        /// <summary>
        /// 港口铁矿石总库存
        /// </summary>
        INDUSTRY_STEEL_TIOEVENTORY,
        /// <summary>
        /// 港口印度铁矿石库存
        /// </summary>
        INDUSTRY_STEEL_YIOEVENTORY,
        /// <summary>
        /// 港口澳洲铁矿石库存
        /// </summary>
        INDUSTRY_STEEL_AIOEVENTORY,
        /// <summary>
        /// 港口巴西铁矿石库存
        /// </summary>
        INDUSTRY_STEEL_BIOEVENTORY,
        /// <summary>
        /// 钢材社会库存
        /// </summary>
        INDUSTRY_STEEL_SOEVENTORY,

        /// <summary>
        /// 钢材企业库存
        /// </summary>
        INDUSTRY_STEEL_ENTEREVENTORY,
        #endregion

        #endregion

        #region 钢铁行业上下游
        /// <summary>
        /// 上游铁矿石运费
        /// </summary>
        INDUSTRY_STEEL_UP_IOTRANS,
        /// <summary>
        /// 上游焦炭价格
        /// </summary>
        INDUSTRY_UP_COKEPRICE,
        /// <summary>
        /// 钢铁下游监测_下游产品产量
        /// </summary>
        INDUSTRY_STEEL_DOWN_PRODUCT,
        /// <summary>
        /// 钢铁下游监测_房地产行业投资情况
        /// </summary>
        INDUSTRY_STEEL_DOWN_REAL,
        #endregion
        /// <summary>
        /// 钢铁行业研究报告
        /// </summary>
        INDUSTRY_STEEL_REPORT,
        #endregion

        #region 核心指标跟踪
        /// <summary>
        /// 固定资产投资完成额
        /// </summary>
        MAC_ESTATE_GTWCEZS,
        /// <summary>
        /// M1与M2
        /// </summary>
        MAC_ESTATE_MOMTZS,
        /// <summary>
        /// CPI同环比
        /// </summary>
        MAC_ESTATE_CPITHBZS,
        /// <summary>
        /// 当月新增人民币贷款
        /// </summary>
        MAC_ESTATE_DYXZRMBDKZS,
        /// <summary>
        /// 外汇占款余额
        /// </summary>
        MAC_ESTATE_WHZKYEZS,
        /// <summary>
        /// 进出口总额
        /// </summary>
        MAC_ESTATE_JCKZEZS,
        /// <summary>
        /// 社会消费品总额
        /// </summary>
        MAC_ESTATE_SHXFPZEZS,
        #endregion
        #endregion
        #region 经济业务
        #region 全景速览
        /// <summary>
        /// 券商营业部分布
        /// </summary>
        SPE_COUNTINAREA,
        #region 证券公司整体业务情况
        /// <summary>
        /// 交易数据   市场份额
        /// </summary>
        SPE_SE_TRADEINFO,
        /// <summary>
        /// 券商交易量 市场份额 中均值
        /// </summary>
        SPE_SE_TRADEASCFEAVGANDMEDIAN,
        /// <summary>
        /// 相对地位
        /// </summary>
        SPE_SE_XDDWRANK,
        /// <summary>
        /// 券商相对地位中均值
        /// </summary>
        SPE_SE_XXDWAVGANDMEDIAN,
        /// <summary>
        /// 部均
        /// </summary>
        SPE_SE_BJANDRANK,
        /// <summary>
        /// 券商部均中均值
        /// </summary>
        SPE_SE_BJAVGANDMEDIAN,

        #endregion
        #region  营业部整体情况
        /// <summary>
        /// 交易数据	 市场份额
        /// </summary>
        SPE_SAL_TRADESHARERANK,
        /// <summary>
        /// 营业部 交易数据中均值
        /// </summary>
        SPE_SAL_TRADEAVGANDMEDIAN,
        /// <summary>
        /// 营业部市场份额中均值
        /// </summary>
        SPE_SAL_SCFEAVGANDMEDIAN,
        /// <summary>
        /// 相对地位
        /// </summary>
        SPE_SAL_XDDWRANK,
        /// <summary>
        /// 营业部相对地位中均值
        /// </summary>
        SPE_SAL_XXDWANGANDMEDIAN,

        #endregion
        #region 地区整体业务分布
        /// <summary>
        /// 地区交易量数据及排名
        /// </summary>
        SPE_REG_TRADEANRANK,
        /// <summary>
        /// 地区交易量中均值
        /// </summary>
        SPE_REG_AVGANDMEDIANTRADE,
        /// <summary>
        /// 地区市场份额及排名
        /// </summary>
        SPE_REG_FEANDRANK,
        /// <summary>
        /// 地区市场份额中均值
        /// </summary>
        SPE_REG_SCFEAVGANDMEDIAN,
        /// <summary>
        /// 地区相对地位及排名
        /// </summary>
        SPE_REG_XXDWANDRANK,
        /// <summary>
        /// 地区相对地位中均值
        /// </summary>
        SPE_REG_XXDWAVGANDMEDIAN,
        /// <summary>
        /// 地区部均基排名
        /// </summary>
        SPE_REG_BJANDRANK,
        /// <summary>
        /// 地区部均中均值
        /// </summary>
        SPE_REG_BJAVGMEDIAN,


        #endregion

        #endregion

        #region 证券公司大全
        /// <summary>
        /// 券商基本资料表 主表
        /// </summary>
        SPE_SE_SEINFO,
        /// <summary>
        /// 营业部基本信息表  子表    营业部大全
        /// </summary>
        SPE_SAL_SALDINFO,
        /// <summary>
        /// 营业部城市分布统计图
        /// </summary>
        SPE_SAL_PROVINCECOUNT,
        /// <summary>
        /// 经纪业务地域源
        /// </summary>
        SPE_SEC_PROVINCEANDCITY,
        /// <summary>
        /// 营业部城市
        /// </summary>
        SPE_SAL_YINGYEBUCITY,
        /// <summary>
        /// 经纪业务省份源
        /// </summary>
        SPE_SEC_PROVINCE,

        #endregion

        #region 券商交易排名
        /// <summary>
        /// 券商各交易数据及全国排名
        /// </summary>
        SPE_SE_TRADEANDRANKINPROVINCE,
        /// <summary>
        /// 券商基本报表 统计
        /// </summary>
        SPE_COMPANYBSINFO1,
        /// <summary>
        /// 券商部均及排名
        /// </summary>
        SPE_SE_SECBJOFRANK,
        #endregion
        #region 营业部交易排名 统计
        /// <summary>
        /// 营业部交易数据市场份额在省份中的排名
        /// </summary>
        SPE_SAL_TRADEFERANKINPROVINCE,
        /// <summary>
        /// 营业部相对地位在省份中的排名
        /// </summary>
        SPE_SAL_XXDWRANKINPROVINCE,
        #endregion


        #region 地区交易排名  统计
        /// <summary>
        /// 地区交易量/市场份额整体统计
        /// </summary>
        SPE_SE_TRADEINAREAOFALL,
        /// <summary>
        /// 地区相对地位数据统计
        /// </summary>
        SPE_SE_TRADEINAREAXDDWRANK,
        /// <summary>
        /// 地区部均交易统计
        /// </summary>
        SPE_SE_TRADEINAREABJRANK,



        #endregion


        #region  证券公司跳转
        /// <summary>
        /// 券商利润表
        /// </summary>
        SPE_SE_INCOMESHEET,
        /// <summary>
        /// 券商资产负债表
        /// </summary>
        SPE_SE_BALANCESHEET,

        #endregion

        #region 营业部跳转
        /// <summary>
        /// 营业部交易数据市场份额在券商间排名
        /// </summary>
        SPE_SAL_TRADEFERANKINSEC,
        /// <summary>
        /// 营业部相对地位在券商间排名
        /// </summary>
        SPE_SAL_XXDWRANKINSEC,
        /// <summary>
        /// 营业部交易情况 统计
        /// </summary>
        SPE_CUSTTRADEINFO1,


        #endregion

        #region 我关注的证券公司比较
        /// <summary>
        /// 券商同环比以及营业部个数统计
        /// </summary>
        SPE_SE_SECTBHBANDSALCOUNT,
        /// <summary>
        /// 券商累计成交量比较
        /// </summary>
        SPE_SE_SECTRADE2,
        #endregion
        #region 我关注的营业部 比较
        /// <summary>
        /// 营业部同环比统计
        /// </summary>
        SPE_SAL_YJTBHB,

        #endregion

        #endregion
        #region 股票f9
        /// <summary>
        /// 交易日期
        /// </summary>
        MOD_DATERE,
        /// <summary>
        /// 日行情
        /// </summary>
        IND_DAILYPRICE,
        /// <summary>
        /// 行情报价
        /// </summary>
        SF9_MARKETQ,
        SPE_MARGINSTOCKTRADE,
        /// <summary>
        /// 重要指标
        /// </summary>
        F9_IMPORTANTINDICATOR,
        /// <summary>
        /// 行业内个股(统计)
        /// </summary>
        F9_ISHAREDAILY_NEW,
        F9_MARKETRANKING_DAILY,
        /// <summary>
        /// 行业内个股
        /// </summary>
        F9_ISHAREDAILY,
        F9_MARKETRANKING,
        /// <summary>
        /// 行业个股(最新报告期)
        /// </summary>
        F9_ISHAREREPORT_NEW,
        F9_MARKETRANKING_NEW,
        F9_MARKETRANKING_REPORT,
        F9_MARKETRANKING_IND,
        F9_MARKETRANKING_IND_NEW,
        /// <summary>
        /// 申万二级代码
        /// </summary>
        F9_PUBLISHCODESWEJDM,
        /// <summary>
        /// 市场均值
        /// </summary>
        F9_MARKETAVERAGE,
        /// <summary>
        /// 市场均值(交易日)
        /// </summary>
        F9_MARKETRANKING_MAR,
        /// <summary>
        /// 市场排名表现_市场均值
        /// </summary>
        F9_MARKETRANKING_MAR_NEW,
        /// <summary>
        /// 行业均值
        /// </summary>
        F9_INDUSTRYAVERAGE,
        /// <summary>
        /// 限售解禁时间表
        /// </summary>
        SF9_XSJJDATE,
        /// <summary>
        /// 股东户数指标
        /// </summary>
        F9_TNOSII,
        /// <summary>
        /// 股东户数
        /// </summary>
        SF9_TNOSII,
        /// <summary>
        /// 股东户数
        /// </summary>
        F9_TNOSII_DETIAL,
        /// <summary>
        /// 机构投资者
        /// </summary>
        SF9_INSTINVESTOR,
        /// <summary>
        /// 基金持股统计
        /// </summary>
        SF9_FUNDHOLDSTOCK,
        /// <summary>
        /// 前十大流通股股东(统计)
        /// </summary>
        F9_TTTTS1,
        /// <summary>
        /// 前十大股东明细(统计)
        /// </summary>
        F9_TTTSD1,
        /// <summary>
        /// 前十大流通股股东
        /// </summary>
        F9_TTTTS,
        SF9_TTTTS,
        /// <summary>
        /// 前十大股东明细
        /// </summary>
        F9_TTTSD,
        SF9_TTTSD,//测试
        /// <summary>
        /// 股本结构
        /// </summary>
        SF9_CAPITALSTRUCT,
        /// <summary>
        /// 公司简介
        /// </summary>
        SF9_COMPANYPROFILE,
        ///<summary>
        ///高级管理人员现任
        ///</summary>
        SF9_SMPC,
        /// <summary>
        /// 公司高管 --首页
        /// </summary>
        SF9_GAOGUAN,
        /// <summary>
        /// 相关证券
        /// </summary>
        F9_RELATEDSECURITIES,
        /// <summary>
        /// 财务摘要
        /// </summary>
        SF9_FS,
        /// <summary>
        /// 财务摘要（单季度）
        /// </summary>
        SF9_QFS,
        /// <summary>
        /// 新股发行
        /// </summary>
        SF9_TIONS,
        /// <summary>
        /// 新股发行(首页)
        /// </summary>
        SF9_NPUB,
        /// <summary>
        /// 分红
        /// </summary>
        SF9_ABONUS,
        /// <summary>
        /// 配股
        /// </summary>
        SF9_AOS,
        /// <summary>
        /// 增发
        /// </summary>
        SF9_SEO,
        /// <summary>
        /// 首页->股本结构
        /// </summary>
        SF9_EQUITYS,
        /// <summary>
        /// 主力持仓
        /// </summary>
        F9_SVZL_NEW,
        /// <summary>
        /// 业绩预告
        /// </summary>
        F9_SALESFORECAST,
        /// <summary>
        /// 收入趋势
        /// </summary>
        SF9_IT,
        /// <summary>
        /// 盈利趋势
        /// </summary>
        SF9_PT,
        /// <summary>
        /// 机构评级
        /// </summary>
        SP9_PUBRATING,
        /// <summary>
        /// 派现与分配统计
        /// </summary>
        SF9_DIVIDEND_EX,
        /// <summary>
        /// 限售股份解禁时间表
        /// </summary>
        SF9_LSOSOTLOTBS,
        ///<summary>
        ///董事会现任
        ///</summary>
        SF9_TBODI,
        ///<summary>
        ///监事会现任
        ///</summary>
        SF9_TBOSI,
        ///<summary>
        ///管理层持股及报酬
        ///</summary>
        SF9_MOAC,
        ///<summary>
        ///历届管理成员
        ///</summary>
        SF9_AAMOM,
        ///<summary>
        ///管理层持股变化
        ///</summary>
        SF9_MOC,
        ///<summary>
        ///诉讼仲裁
        ///</summary>
        SF9_LAB,
        ///<summary>
        ///担保
        ///</summary>
        SF9_GUARANTEE,
        ///<summary>
        ///违规
        ///</summary>
        SF9_LLLEGAL,
        ///<summary>
        ///关联交易
        ///</summary>
        SF9_RELATEDT,
        /// <summary>
        /// 同公司其他证券
        /// </summary>
        SF9_WTCAOS,
        /// <summary>
        /// 交易异动成交营业部
        /// </summary>
        SF9_JIAOYIYIDONG,
        /// <summary>
        /// 资金流向
        /// </summary>
        SF9_FUNDFLOW,
        /// <summary>
        /// 资金流向--阶段统计
        /// </summary>
        SF9_FUNDFLOW_HY2,
        SF9_FUNDFLOW_HY1,
        /// <summary>
        /// 资金流向
        /// </summary>
        SF9_FUNDFLOW_HY,
        /// <summary>
        /// 融资融券表格
        /// </summary>
        SF9_MARGINTRADING1,
        /// <summary>
        /// 风险与收益分析
        /// </summary>
        SF9_RISKANDRETURN,
        /// <summary>
        /// 每日行情数据统计
        /// </summary>
        SF9_DAYQUOTESTAT,
        /// <summary>
        /// 阶段行情数据统计(统计源)
        /// </summary>
        SF9_INTVLQUOTESTAT1,
        /// <summary>
        /// 阶段行情数据统计（非统计源）
        /// </summary>
        SF9_INTVLQUOTESTAT,
        /// <summary>
        /// 证券简介
        /// </summary>
        SF9_SECURITY,
        /// <summary>
        /// 所属行业
        /// </summary>
        SF9_INDUSTRY,
        /// <summary>
        /// 所属指数
        /// </summary>
        SF9_THEINDEX,
        /// <summary>
        /// 所属概念板块
        /// </summary>
        SF9_TCOP,
        SPE_NS_HYBK,
        /// <summary>
        /// 主营收入 按行业分类
        /// </summary>
        IND_MAINFORMINDUSTRY,
        /// <summary>
        /// 主营收入 按产品（项目分类）
        /// </summary>
        IND_MAINFORMPRODUCT,
        /// <summary>
        /// 主营收入 按地区分类
        /// </summary>
        IND_MAINFORMREGION,
        /// <summary>
        /// 银行业专项指标
        /// </summary>
        SF9_TBSI,
        /// <summary>
        /// 成长能力
        /// </summary>
        IND_YOYGR,
        /// <summary>
        /// 成长能力
        /// </summary>
        IND_ABORGR,
        /// <summary>
        /// 运营能力
        /// </summary>
        IND_OCI,
        /// <summary>
        /// 现金流量
        /// </summary>
        IND_CASHF,
        /// <summary>
        /// 每股指标
        /// </summary>
        IND_PSD,
        /// <summary>
        /// 每股指标
        /// </summary>
        IND_CFPI,
        /// <summary>
        /// 盈利能力与收益能力
        /// </summary>
        IND_PAROE,
        /// <summary>
        /// 盈利能力与收益能力
        /// </summary>
        IND_POTY,
        /// <summary>
        /// 盈利能力与收益能力
        /// </summary>
        IND_SPA,
        /// <summary>
        /// 盈利能力与收益能力
        /// </summary>
        IND_QOE,
        /// <summary>
        /// 盈利能力与收益能力
        /// </summary>
        IND_TPOA,
        /// <summary>
        /// 资本结构与偿债能力
        /// </summary>
        IND_CAPITALS,
        /// <summary>
        /// 资本结构与偿债能力
        /// </summary>
        IND_DPA,
        /// <summary>
        /// 资本结构与偿债能力
        /// </summary>
        IND_DPAADD,
        /// <summary>
        /// 资本结构与偿债能力
        /// </summary>
        IND_BUSINESSS,
        /// <summary>
        /// 杜邦分析
        /// </summary>
        IND_DUPONTA,
        /// <summary>
        /// 杜邦分析
        /// </summary>
        IND_DAOE,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        IND_QUARTERLYI,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        IND_ASQPFI,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        IND_QFIR,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        IND_ASQYOYGOFI,
        /// <summary>
        /// 分红
        /// </summary>
        IND_DDD,
        /// <summary>
        /// 分红
        /// </summary>
        IND_BONUSPLAN,
        /// <summary>
        /// 分红
        /// </summary>
        IND_DIVIDENDINDEX,
        /// <summary>
        /// 发行可转债
        /// </summary>
        SP9_FAXINGKEZHUANZHAI,
        /// <summary>
        /// 募集资金投向
        /// </summary>
        SF9_TRFTII,
        /// <summary>
        /// 担保金额
        /// </summary>
        IND_GUARANTEEDATA,
        /// <summary>
        /// 应交税金明细
        /// </summary>
        SF9_SPT,
        /// <summary>
        /// 存货明细
        /// </summary>
        IF9_INSTOCKDETAIL,
        /// <summary>
        /// 财务费用明细
        /// </summary>
        IND_FINANCECOSTS,
        /// <summary>
        /// 交易异动成交营业部
        /// </summary>
        SF9_TTBD,
        /// <summary>
        /// 财务摘要（报告期）
        /// </summary>
        IF9_SFRP,
        /// <summary>
        /// 财务摘要（报告期）续
        /// </summary>
        IF9_SFRPC,
        /// <summary>
        /// 财务摘要(单季度)
        /// </summary>
        SF9_ASQF,
        /// <summary>
        /// 现金流量表（非金融）
        /// </summary>
        SF9_NFSOCF,
        /// <summary>
        /// 非金融类现金流量表(同比增长率)
        /// </summary>
        SF9_NFSOCF_ANNUALGROWTH,
        /// <summary>
        /// 现金流量表（银行类）
        /// </summary>
        SF9_BSOCF,
        /// <summary>
        ///     银行类现金流量表(同比增长率)
        /// </summary>
        SF9_BSOCF_ANNUALGROWTH,
        /// <summary>
        ///现金流量表（保险类） 
        /// </summary>
        SF9_ISOCF,
        /// <summary>
        ///     保险类现金流量表(同比增长率)
        /// </summary>
        SF9_ISOCF_ANNUALGROWTH,
        /// <summary>
        /// 现金流量表（证券类）
        /// </summary>
        SF9_SCFT,
        /// <summary>
        /// 证券类现金流量表(同比增长率)
        /// </summary>
        SF9_SCFT_ANNUALGROWTH,
        /// <summary>
        /// 现金流量表 单季度（银行类）
        /// </summary>
        SF9_BQCFB,
        /// <summary>
        /// 现金流量表 单季度（银行类）同比增长率
        /// </summary>
        SF9_BQCFB_TB,
        /// <summary>
        /// 现金流量表 单季度（保险类）
        /// </summary>
        SF9_IQCFB,
        /// <summary>
        /// 现金流量表 单季度（证券类）
        /// </summary>
        SF9_SQCFB,
        /// <summary>
        ///现金流量表 单季度（非金融类） 
        /// </summary>
        SF9_NFQCFB,
        /// <summary>
        /// 资产负债表（非金融类）
        /// </summary>
        IF9_NFSOAAL,
        /// <summary>
        /// 非金融类资产负债表(同比增长率)
        /// </summary>
        IF9_NFSOAAL_ANNUALGROWTH,
        /// <summary>
        /// 证券类资产负债表(同比增长率)
        /// </summary>
        IF9_SABS_ANNUALGROWTH,
        /// <summary>
        /// 资产负债表（非金融类）销售百分比
        /// </summary>
        IF9_NFSOAAL_SALESPERCENTAGE,
        /// <summary>
        /// 资产负债表（银行类）
        /// </summary>
        IF9_BBS,
        /// <summary>
        /// 资产负债表（银行类）销售百分比
        /// </summary>
        IF9_BBS_SALESPERCENTAGE,
        /// <summary>
        /// 资产负债表（银行类）资产百分比
        /// </summary>
        IF9_BBS_ASSETSPERCENTAGE,
        /// <summary>
        /// 资产负债表（银行类）同比增长率
        /// </summary>
        IF9_BBS_ANNUALGROWTH,
        /// <summary>
        /// 资产负债表（证券类）
        /// </summary>
        IF9_SABS,
        /// 资产负债表(保险类)
        /// </summary>
        IF9_IBS,
        /// <summary>
        /// 保险类资产负债表(资产百分比)
        /// </summary>
        IF9_IBS_ASSETSPERCENTAGE,
        /// <summary>
        /// 非金融类资产负债表(资产百分比)
        /// </summary>
        IF9_NFSOAAL_ASSETSPERCENTAGE,
        /// <summary>
        /// 证券类资产负债表(资产百分比)
        /// </summary>
        IF9_SABS_ASSETSPERCENTAGE,
        /// <summary>
        /// 资产负债表(保险类)销售百分比
        /// </summary>
        IF9_IBS_SALESPERCENTAGE,
        /// <summary>
        /// 保险类资产负债表(同比增长率)
        /// </summary>
        IF9_IBS_ANNUALGROWTH,
        /// <summary>
        /// 股本结构
        /// </summary>
        F9_INSTINFO,
        /// <summary>
        /// 表头概况
        /// </summary>
        SP9_TITLE,
        /// <summary>
        /// 融资融券个券交易统计
        /// </summary>
        SF9_MARGINTRADING,
        /// <summary>
        /// 公司介绍（曾用名）
        /// </summary>
        SF9_NAMECHGB,
        /// <summary>
        /// 利润表（银行类）
        /// </summary>
        SF9_BP,
        /// <summary>
        /// 银行类利润表(同比增长率)
        /// </summary>
        SF9_BP_ANNUALGROWTH,
        /// <summary>
        /// 银行类利润表(销售百分比)
        /// </summary>
        SF9_BP_SALESPERCENTAGE,
        /// <summary>
        ///利润表(证券类） 
        /// </summary>
        SF9_SIS,
        /// <summary>
        /// 证券类资产负债表(销售百分比)
        /// </summary>
        IF9_SABS_SALESPERCENTAGE,
        /// <summary>
        /// 保险类利润表(同比增长率)
        /// </summary>
        SF9_IIS_ANNUALGROWTH,
        /// <summary>
        /// 非金融类利润表(同比增长率)
        /// </summary>
        SF9_NFPF_ANNUALGROWTH,
        /// <summary>
        /// 非金融单季度现金流量同比增长
        /// </summary>
        SF9_NFQCFB_TB,
        /// <summary>
        /// 非金融单季度利润同比增长
        /// </summary>
        SF9_NFQPF_TB,
        /// <summary>
        /// 单季度利润同比增长
        /// </summary>
        SF9_IQCFB_TB,
        /// <summary>
        /// 证券类单季度现金流量表同比增长
        /// </summary>
        SF9_SQCFB_TB,
        /// <summary>
        /// 证券类单季度利润表同比增长
        /// </summary>
        SF9_SQPF_TB,
        /// <summary>
        /// 银行类单季度利润表销售比-
        /// </summary>
        SF9_BQPF_XS,
        /// <summary>
        /// 银行类单季度利润表同比增加-
        /// </summary>
        SF9_BQPF_TB,
        /// <summary>
        /// 保险类单季度利润表销售比-
        /// </summary>
        SF9_IQPF_XS,
        /// <summary>
        /// 保险类单季度利润表同比增长
        /// </summary>
        SF9_IQPF_TB,
        /// <summary>
        /// 非金融类单季度利润表销售比-
        /// </summary>
        SF9_NFQPF_XS,
        /// <summary>
        /// 证券类单季度利润表销售比
        /// </summary>
        SF9_SQPF_XS,
        /// <summary>
        /// 银行类单季度现金流量环比增长-
        /// </summary>
        SF9_BQCFB_HB,
        /// <summary>
        /// 银行类单季度利润表环比增长-
        /// </summary>
        SF9_BQPF_HB,
        /// <summary>
        /// 保险类单季度现金流量环比增长-
        /// </summary>
        SF9_IQCFB_HB,
        /// <summary>
        /// 保险类单季度利润环比增长-
        /// </summary>
        SF9_IQPF_HB,
        /// <summary>
        /// 非金融类单季度现金流量环比增长-
        /// </summary>
        SF9_NFQCFB_HB,
        /// <summary>
        /// 非金融单季度利润环比增长-
        /// </summary>
        SF9_NFQPF_HB,
        /// <summary>
        /// 证券类单季度现金流量环比增长-
        /// </summary>
        SF9_SQCFB_HB,
        /// <summary>
        /// 证券类单季度利润环比增长-
        /// </summary>
        SF9_SQPF_HB,
        /// <summary>
        /// 证券类利润表(同比增长率)
        /// </summary>
        SF9_SIS_ANNUALGROWTH,
        /// <summary>
        /// 证券类利润表(销售百分比)
        /// </summary>
        SF9_SIS_SALESPERCENTAGE,
        /// <summary>
        /// 利润表(保险类） 
        /// </summary>
        SF9_IIS,
        /// <summary>
        ///保险类利润表(销售百分比) 
        /// </summary>
        SF9_IIS_SALESPERCENTAGE,
        /// <summary>
        ///利润表(非金融类） 
        /// </summary>
        SF9_NFPF,
        /// <summary>
        /// 非金融类利润表(销售百分比)
        /// </summary>
        SF9_NFPF_SALESPERCENTAGE,
        /// <summary>
        /// 利润表单季度（银行类）
        /// </summary>
        SF9_BQPF,
        /// <summary>
        ///利润表单季度(证券类） 
        /// </summary>
        SF9_SQPF,
        /// <summary>
        ///利润表单季度(保险类）  
        /// </summary>
        SF9_IQPF,
        /// <summary>
        ///利润表单季度(非金融类） 
        /// </summary>
        SF9_NFQPF,
        /// <summary>
        /// 资产减值准备明细
        /// </summary>
        F9_IOA,
        /// <summary>
        /// 坏账准备比例明细
        /// </summary>
        F9_HZMX,
        /// <summary>
        /// 关联方债权债务
        /// </summary>
        F9_GLFZQZW,
        /// <summary>
        /// 应收账款账龄结构
        /// </summary>
        F9_XSZKZLJG,
        /// <summary>
        /// 应收账款大股东欠款
        /// </summary>
        F9_YSDGDQK,
        /// <summary>
        /// 应收账款主要欠款人
        /// </summary>
        F9_YSZKZYQKR,
        /// <summary>
        /// 其它应收账款账龄结构
        /// </summary>
        F9_QTYSZLJG,
        /// <summary>
        /// 其它应收账款大股东欠款
        /// </summary>
        F9_QTYSDGDQK,
        /// <summary>
        /// 主要其他应收款明细
        /// </summary>
        F9_ZYQTYSKMX,
        /// <summary>
        /// 财务费用明细
        /// </summary>
        F9_FINANCECOSTS,
        /// <summary>
        /// 担保金额
        /// </summary>
        F9_GUARANTEEDATA,
        /// <summary>
        /// 每股指标
        /// </summary>
        SF9_MEIGUZB,
        /// <summary>
        /// 资本结构与偿债能力
        /// </summary>
        SF9_CAPSTRUCTANDSOLVENCY,
        /// <summary>
        /// 营运能力
        /// </summary>
        SF9_OPERATINGCAP,
        /// <summary>
        /// 成长能力
        /// </summary>
        SF9_GROWTHINGCAP,
        /// <summary>
        /// 现金流量
        /// </summary>
        SF9_CASHFLOW,

        /// <summary>
        /// 杜邦分析
        /// </summary>
        SF9_DUPOND,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        SF9_SINGLEFINANCEIND,
        /// <summary>
        /// 募集资金投向（弹窗）
        /// </summary>
        SF9_ZJTX,
        /// <summary>
        /// 杜邦分析弹窗
        /// </summary>
        SF9_DUPONTWINDOW,
        /// <summary>
        /// 主营构成-按行业分类
        /// </summary>
        SF9_TMCBC,
        /// <summary>
        /// 主营构成-按行业分类(弹窗)
        /// </summary>
        SF9_TMCBC1,
        SF9_TMCBC_TJ,
        /// <summary>
        /// 主营构成-按产品（项目）分类
        /// </summary>
        SF9_MFBPC,
        /// <summary>
        /// 主营构成-按产品（项目）分类(弹窗)
        /// </summary>
        SF9_MFBPC1,
        SF9_MFBPC_TJ,
        /// <summary>
        /// 主营构成-按产地区分类
        /// </summary>
        SF9_TMCCBR,
        /// <summary>
        /// 主营构成-按产地区分类(统计)
        /// </summary>
        SF9_TMCCBR1,
        SF9_TMCCBR_TJ,
        /// <summary>
        /// 盈利能力与收益质量
        /// </summary>
        SF9_PROFITANDEARNING,
        /// <summary>
        /// 最新机构预测值与综合值比较
        /// </summary>
        SF9_INST_INDICATORVALUE1,
        /// <summary>
        /// 机构最近一次预测
        /// </summary>
        SP9_PUBLISHINST_PREDICT_NEW,
        /// <summary>
        /// 历史预测值与综合值比较
        /// </summary>
        SF9_INST_INDICATORVALUE2,
        /// <summary>
        /// 市场表现
        /// </summary>
        SF9_MARKETPERFORMANCE,
        /// <summary>
        /// 盈利预测与研究报告--盈利预测-个股盈利预测-图
        /// </summary>
        SP9_RATING_EPSGF,
        /// <summary>
        /// 预测综合值与实际业绩比较
        /// </summary>
        SF9_INST_INDICATORVALUE3,
        /// <summary>
        /// 投资评级阶段变化
        /// </summary>
        SP9_RATING_EMRATING,
        /// <summary>
        /// 投资评级预测
        /// </summary>
        SP9_RATING_PREDICT_TJ,
        /// <summary>
        /// 价值分析
        /// </summary>
        SF9_VALUEPREDICTION,
        /// <summary>
        /// 盈利预测
        /// </summary>
        SFP_PREDICTION,
        /// <summary>
        /// 股价与综合评级比较统计
        /// </summary>
        SP9_RATING_GF,
        /// <summary>
        /// 财务分析
        /// </summary>
        SF9_FINANCIALANALYSIS,
        /// <summary>
        /// 财务数据
        /// </summary>
        SF9_FINANCIALDATA,
        /// <summary>
        /// 预测数据趋势与变动
        /// </summary>
        SP9_PREDICTEDDATAREND_YEAR,
        /// <summary>
        /// 预测数据趋势与变动（月度）
        /// </summary>
        SP9_PREDICTEDDATAREND_MONTH,
        /// <summary>
        /// 投资评级机构历史预测明细
        /// </summary>
        SP9_RATING_PREDICT,
        /// <summary>
        /// 行业列表
        /// </summary>
        SF9_INDUSTRY_LIST,
        /// <summary>
        /// 股价与预测EPS比较基础源
        /// </summary>
        SP9_EPSCOMPARE,
        /// <summary>
        /// 股价与综合评级比较基础源
        /// </summary>
        SP9_RATING,
        #endregion
        #region 基金F9
        /// <summary>
        /// 阶段基金行情与净值(封闭式\LOF\ETF)
        /// </summary>
        FND_JDHQJZCLE,
        /// <summary>
        /// 阶段基金行情与净值(货币式)
        /// </summary>
        FND_JDHQJZHB,
        /// <summary>
        /// 阶段基金行情与净值(除了LOF\ETF\货币式)
        /// </summary>
        FND_JDHQJZOPEN,
        /// <summary>
        /// 周K线
        /// </summary>
        FND_WEEKKLINE,
        /// <summary>
        /// 发行(适用开放式基金)
        /// </summary>
        FND_OPENFUNDS,
        /// <summary>
        /// 财务摘要指标
        /// </summary>
        FND_CAIWUZY,
        /// <summary>
        /// 基金每日行情及上证指数
        /// </summary>
        FND_FUNDHQSZ,
        /// <summary>
        /// 资产配置指标
        /// </summary>
        FND_ZCPZ,
        /// <summary>
        /// 单季度申购与赎回
        /// </summary>
        FND_SINGLESGSH,
        /// <summary>
        /// 开放式基金规模变动指标
        /// </summary>
        FND_FUNDGM,
        /// <summary>
        /// 封闭式基金规模变动指标
        /// </summary>
        FND_CLOSEFUNDBD,
        /// <summary>
        /// 重仓股票
        /// </summary>
        FND_KEYSTOCK,
        /// <summary>
        /// 资产规模
        /// </summary>
        FND_ZICGUIMO,
        /// <summary>
        /// 基金份额
        /// </summary>
        FND_FUNDFE,
        /// <summary>
        /// 基金介绍
        /// </summary>
        FND_F9_FUNDJS,
        /// <summary>
        /// 基金分类
        /// </summary>
        IND_F9_TYPE,
        /// <summary>
        /// 外汇牌价
        /// </summary>
        FND_WAIHUI,
        /// <summary>
        /// 港股美股所用外汇统计
        /// </summary>
        SF9_WAIHUI1,
        /// <summary>
        /// 每日基金净值
        /// </summary>
        FND_FUNDOPENJZ,
        /// <summary>
        /// 货币式基金收益(画图用)
        /// </summary>
        FND_HBSHT,
        /// <summary>
        /// 分红
        /// </summary>
        FND_FENHONG1,
        /// <summary>
        /// 与管理人旗下基金共同持仓品种
        /// </summary>
        FND_FUNDCOMPANYLX1,
        /// <summary>
        /// 与管理人旗下基金共同持仓品种
        /// </summary>
        FND_FUNDCOMPANYLX,
        /// <summary>
        /// 封闭式基金净值与行情
        /// </summary>
        FND_CLOSEFUNDHQ,
        /// <summary>
        /// 封闭式基金发行指标
        /// </summary>
        FND_CLOSEFUNDS,
        /// <summary>
        /// 基金前端申购费
        /// </summary>
        FND_FUNDQDFEE1,
        /// <summary>
        /// 基金后端申购费
        /// </summary>
        FND_FUNDHDFEE1,
        /// <summary>
        /// 基金赎回费
        /// </summary>
        FND_FUNDSHFEE1,
        /// <summary>
        /// 基金重仓股表现
        /// </summary>
        FND_FUNDKEYSTOCK,
        /// <summary>
        /// 收益数据(适用于货币型基金)
        /// </summary>
        FND_HBSYSJ,
        /// <summary>
        /// 累计买入
        /// </summary>
        FND_LJMR,
        /// <summary>
        /// 累计买入(QDII)
        /// </summary>
        FND_LJMRQDII,
        /// <summary>
        /// 累计卖出
        /// </summary>
        FND_LJMC,
        /// <summary>
        /// 累计卖出(QDII)
        /// </summary>
        FND_LJMCQDII,
        /// <summary>
        /// 代销基金列表
        /// </summary>
        FND_DXFUND,
        /// <summary>
        /// 代销机构统计
        /// </summary>
        FND_DXFUND1,
        /// <summary>
        /// 基金速览行情报价
        /// </summary>
        IND_F9_MARKETPRICE,
        /// <summary>
        /// 封闭式基金折价率（web画图）
        /// </summary>
        FND_CLOSEFUNDZJL1,
        /// <summary>
        /// 货币基金每日收益
        /// </summary>
        FND_HBFUND,
        /// <summary>
        /// ETF基金净值与行情
        /// </summary>
        FND_LETFFUND,
        /// <summary>
        /// 流通受限股票
        /// </summary>
        FND_XZSTOCK,
        /// <summary>
        /// 流通受限债券
        /// </summary>
        FND_XZBOND,
        /// <summary>
        /// 相同管理人
        /// </summary>
        FND_SAMECOMPANY,
        /// <summary>
        /// 相同管理人(货币web查询用)
        /// </summary>
        FND_HB,
        /// <summary>
        /// 相同管理人（货币市场基金）
        /// </summary>
        FND_SMAECOMPANYHB,
        /// <summary>
        /// 相同管理人(非货币web查询用)
        /// </summary>
        FND_FHB,
        /// <summary>
        /// 封闭式基金资料
        /// </summary>
        FND_FUNDABFEE1,
        /// <summary>
        /// 封闭式基金资料
        /// </summary>
        FND_CLOSEFUND,
        /// <summary>
        /// 开放式基金资料
        /// </summary>
        IND_OPENFUNDINFO,
        /// <summary>
        /// 开放式基金资料
        /// </summary>
        FND_OPENFUND,
        /// <summary>
        /// 开放式基金申购赎回
        /// </summary>
        IND_LOFPR,
        /// <summary>
        /// 开放式基金申购赎回
        /// </summary>
        IND_OFUNDISSUEDINDEX,
        /// <summary>
        /// 报告期基金份额
        /// </summary>
        IND_FUNDSHARE,
        /// <summary>
        /// 资产配置
        /// </summary>
        IND_ASSETALLOCATION,
        /// <summary>
        /// 基金持有人结构
        /// </summary>
        FND_CYRHS,
        /// <summary>
        /// 基金净值变动表
        /// </summary>
        FND_JZBD,
        /// <summary>
        /// 利润表
        /// </summary>
        FND_LRBZB,
        /// <summary>
        /// 基金资产负债表
        /// </summary>
        FND_ZCHZB,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        FND_SINGLEZB,
        /// <summary>
        /// 基金资产净值的偏离
        /// </summary>
        FND_ZCJZPL,
        /// <summary>
        /// 报告期债券回购融资情况
        /// </summary>
        FND_ZQHGRZ,
        /// <summary>
        /// 持有权证明细
        /// </summary>
        FND_HOLDWW,
        /// <summary>
        /// 券种组合
        /// </summary>
        FND_QZPZ,
        /// <summary>
        /// 持股明细
        /// </summary>
        FND_ALLSTOCK1,
        /// <summary>
        /// QDII持仓明细
        /// </summary>
        FND_ALLSTOCKQDII,
        /// <summary>
        /// 封闭式基金十大持有人
        /// </summary>
        FND_SDCYR,
        /// <summary>
        /// 阶段基金净值与行情
        /// </summary>
        IND_F9_SOFNVOM,
        /// <summary>
        /// 报告期净值表现
        /// </summary>
        FND_FUNDBX,
        /// <summary>
        /// QDII基金区域配置
        /// </summary>
        FND_ARERPZ,
        /// <summary>
        /// 持债明细
        /// </summary>
        FND_KEYBONDZB,
        /// <summary>
        /// 行业分布
        /// </summary>
        FND_HYPZZB,
        /// <summary>
        /// 首页-股票行业配置
        /// </summary>
        FND_STOCKHYPZ,
        /// <summary>
        /// QDII行业分布
        /// </summary>
        FND_HYQDII,
        /// <summary>
        /// 重仓持股统计
        /// </summary>
        FND_KEYSTOCKZB2,
        /// <summary>
        /// QDII持仓明细
        /// </summary>
        FND_KEYQDSTOCK,
        /// <summary>
        /// 开放式基金发行
        /// </summary>
        IND_OFUNDISSUE,
        /// <summary>
        /// 积极投资统计
        /// </summary>
        IND_STOCKLIST3,
        /// <summary>
        /// 投资组合剩余期限
        /// </summary>
        FND_TZSYQX,
        /// <summary>
        /// 基金分仓数据
        /// </summary>
        FND_FUDNFC1,
        /// <summary>
        /// 基金管理人资料
        /// </summary>
        FND_FUNDCOMPANY,
        /// <summary>
        /// 旗下基金股票型
        /// </summary>
        FND_COMPANYFUND1,
        /// <summary>
        /// 旗下基金指数型
        /// </summary>
        IND_F9_TFIT,
        /// <summary>
        /// 旗下货币式基金
        /// </summary>
        FND_CURRENY,
        /// <summary>
        /// 旗下封闭式基金
        /// </summary>
        FND_FUNDFB,
        /// <summary>
        /// 银河证券基金评价
        /// </summary>
        FND_YHRANK,
        /// <summary>
        /// 招商证券基金评价
        /// </summary>
        FND_ZSRANK,
        /// <summary>
        /// 海通证券基金评价
        /// </summary>
        FND_HTRANK,
        /// <summary>
        /// 上海证券基金评价
        /// </summary>
        FND_SHRANK,
        /// <summary>
        /// 晨星评级
        /// </summary>
        FND_CXRANK,
        /// <summary>
        /// 济安金信
        /// </summary>
        FND_JAJXRANK,
        /// <summary>
        /// 报告期利润
        /// </summary>
        IND_REPORTPROFIT,
        /// <summary>
        /// 重仓股持债
        /// </summary>
        FND_KEYBOND,
        /// <summary>
        /// 首页-券种组合
        /// </summary>
        FND_QZZUHE,
        /// <summary>
        /// 基金经理(现任非货币)
        /// </summary>
        FND_MANAGERFHB,
        /// <summary>
        /// 非货币基金经理统计
        /// </summary>
        FND_MANAGERFHB1,
        /// <summary>
        /// 非货币基金经理统计（web查询用）2
        /// </summary>
        FND_MANAGERFHB2,
        /// <summary>
        /// 同期同类基金比较
        /// </summary>
        IND_F9_FMANAGERC1,
        /// <summary>
        /// 基金经理(历任)货币性
        /// </summary>
        FND_FUNDLRHB,
        /// <summary>
        /// 货币基金同类同期收益
        /// </summary>
        IND_F9_FUNDMANAGERCC1,
        /// <summary>
        /// 基金经理(现任货币)
        /// </summary>
        FND_MANAGERHB,
        /// <summary>
        /// 基金经理货币
        /// </summary>
        FND_MANAGERHB1,
        /// <summary>
        /// 基金经理货币（web查询用）
        /// </summary>
        FND_MANAGERHB2,
        /// <summary>
        /// 基金经理(历任非货币)
        /// </summary>
        FND_MANAGERLR,
        /// <summary>
        /// 历任非货币（web查询展示）
        /// </summary>
        FND_MANAGERLR1,
        /// <summary>
        /// 基金经理（历任）货币型
        /// </summary>
        FND_MANAGERLRHB,
        /// <summary>
        /// 历任非货币基金经理（web查询）
        /// </summary>
        FND_MANAGERLRHB1,
        /// <summary>
        /// 阶段净值(非货币)
        /// </summary>
        IND_F9_TNOMTOF,
        /// <summary>
        /// 开基除LOF\ETF\货币式
        /// </summary>
        FND_JDHQJZOPEN1,
        /// <summary>
        /// 阶段基金行情与净值(货币式)统计
        /// </summary>
        FND_JDHQJZHB1,
        /// <summary>
        /// 阶段基金行情与净值(封闭式\LOF\ETF)统计
        /// </summary>
        FND_JDHQJZCLE1,
        /// <summary>
        /// 阶段基金净值与行情（货币基金）
        /// </summary>
        IND_F9_SOFNVAMC,
        /// <summary>
        /// 公司股东
        /// </summary>
        IND_F9_SHAREHOLDER,
        /// <summary>
        /// 首页-基金日数据
        /// </summary>
        FND_JZBXIAN,
        /// <summary>
        /// 天相投顾
        /// </summary>
        FND_TXRANK,
        #endregion
        #region 基金比较
        /// <summary>
        /// 绩效比较/评级信息/资产组合
        /// </summary>
        FND_FUNDJXPJ,
        /// <summary>
        /// 基本资料/基金公司/基金经理信息
        /// </summary>
        FND_FUNDCOMINFO,
        /// <summary>
        /// 基金比较行业配置
        /// </summary>
        FND_FUNDBJHYPZ,
        /// <summary>
        /// 基金比较十大重仓证券
        /// </summary>
        FND_FUNDKEYTENSTOCK,
        /// <summary>
        /// 基金比较五大重仓债券
        /// </summary>
        FND_FUNDBJWUBOND,
        /// <summary>
        /// 份额变动(图)/在管资产变动(图形)/在管份额变动(图形)
        /// </summary>
        FND_FEJZPICTURE,
        /// <summary>
        /// 投资者结构(图)
        /// </summary>
        FND_TZSJGPICTURE,
        /// <summary>
        /// 基金比较资产配置图
        /// </summary>
        FND_FUNDBJZCPZ,
        /// <summary>
        /// 基金比较业绩走势(一年/两年/三年)
        /// </summary>
        FND_FUNDBJYJZS,
        /// <summary>
        /// 基金比较业绩走势(一年)
        /// </summary>
        FND_FUNDBJONEYEAR,
        /// <summary>
        /// 基金比较业绩走势(两年)
        /// </summary>
        FND_FUNDBJTWOYEAR,
        /// <summary>
        /// 基金比较业绩走势(三年)
        /// </summary>
        FND_FUNDBJTHREEYEAR,
        /// <summary>
        /// 基金比较季度业绩表现(一年/两年/三年)
        /// </summary>
        FND_FUNDBXONETWOT,
        #endregion
        #region 债券F9

        #region 债券F9 财务数据（新准则） 所以统计
        /// <summary>
        /// 银行类单季度现金流量表环比增长
        /// </summary>
        BSPE_F9_BQCFB_HB,
        /// <summary>
        /// 银行类单季度现金流量同比增长
        /// </summary>
        BSPE_F9_BQCFB_TB,
        /// <summary>
        /// 银行类单季度利润表销售比
        /// </summary>
        BSPE_F9_BQPF_XS,
        /// <summary>
        /// 非金融类单季度现金流量同比增长
        /// </summary>
        BSPE_F9_NFQCFB_TB,
        /// <summary>
        /// 银行类单季度利润表环比增长
        /// </summary>
        BSPE_F9_BQPF_HB,
        /// <summary>
        /// 银行类单季度利润表同比增加
        /// </summary>
        BSPE_F9_BQPF_TB,
        /// <summary>
        /// 保险类单季度现金流量环比增长
        /// </summary>
        BSPE_F9_IQCFB_HB,
        /// <summary>
        /// 保险类单季度现金流量同比增长
        /// </summary>
        BSPE_F9_IQCFB_TB,
        /// <summary>
        /// 保险类单季度利润表销售比
        /// </summary>
        BSPE_F9_IQPF_XS,
        /// <summary>
        /// 保险类单季度利润表同比增长
        /// </summary>
        BSPE_F9_IQPF_TB,
        /// <summary>
        /// 保险类单季度利润表环比增长
        /// </summary>
        BSPE_F9_IQPF_HB,
        /// <summary>
        /// 产品
        /// </summary>
        BSPE_F9_MFBPC1,
        /// <summary>
        /// 主营构成按产品统计
        /// </summary>
        BSPE_F9_MFBPC_TJ,
        /// <summary>
        /// 非金融类单季度现金流量环比增长
        /// </summary>
        BSPE_F9_NFQCFB_HB,
        /// <summary>
        /// 非金融类单季度利润表环比增长
        /// </summary>
        BSPE_F9_NFQPF_HB,
        /// <summary>
        /// 非金融类单季度利润销售比
        /// </summary>
        BSPE_F9_NFQPF_XS,
        /// <summary>
        /// 非金融类单季度利润同比增长
        /// </summary>
        BSPE_F9_NFQPF_TB,
        /// <summary>
        /// 证券类单季度现金流量环比增长
        /// </summary>

        BSPE_F9_SQCFB_HB,
        /// <summary>
        /// 证券类单季度现金流量表同比增长
        /// </summary>
        BSPE_F9_SQCFB_TB,
        /// <summary>
        /// 证券类单季度利润表同比增长
        /// </summary>
        BSPE_F9_SQPF_TB,
        /// <summary>
        /// 证券类单季度利润表销售比
        /// </summary>
        BSPE_F9_SQPF_XS,
        /// <summary>
        /// 证券类单季度利润环比增长
        /// </summary>
        BSPE_F9_SQPF_HB,
        /// <summary>
        /// 主营构成按行业统计
        /// </summary>
        BSPE_F9_TMCBC_TJ,
        /// <summary>
        /// 行业
        /// </summary>
        BSPE_F9_TMCBC1,
        /// <summary>
        /// 主营构成按地区统计
        /// </summary>
        BSPE_F9_TMCCBR_TJ,
        /// <summary>
        /// 地区
        /// </summary>
        BSPE_F9_TMCCBR1,
        /// <summary>
        /// 银行类资产负债表(资产负债表)
        /// </summary>
        BSPE_F9_BBS_ASSETSPERCENTAGE,
        /// <summary>
        /// 银行类资产负债表(同比增长率)
        /// </summary>
        BSPE_F9_BBS_ANNUALGROWTH,
        /// <summary>
        /// 银行类资产负债表(销售百分比)	
        /// </summary>
        BSPE_F9_BBS_SALESPERCENTAGE,
        /// <summary>
        /// 保险类资产负债表(销售百分比)
        /// </summary>
        BSPE_F9_IBS_SALESPERCENTAGE,
        /// <summary>
        /// 保险类资产负债表(同比增长率)
        /// </summary>
        BSPE_F9_IBS_ANNUALGROWTH,
        /// <summary>
        /// 保险类资产负债表(资产百分比)
        /// </summary>
        BSPE_F9_IBS_ASSETSPERCENTAGE,
        /// <summary>
        /// 非金融类资产负债表(销售百分比)
        /// </summary>
        BSPE_F9_NFSOAAL_SALESPERCENTAGE,
        /// <summary>
        ///非金融类资产负债表(同比增长率) 
        /// </summary>
        BSPE_F9_NFSOAAL_ANNUALGROWTH,
        /// <summary>
        /// 非金融类资产负债表(资产百分比)
        /// </summary>
        BSPE_F9_NFSOAAL_ASSETSPERCENTAGE,
        /// <summary>
        /// 证券类资产负债表(销售百分比)
        /// </summary>
        BSPE_F9_SABS_SALESPERCENTAGE,
        /// <summary>
        /// 证券类资产负债表(同比增长率)
        /// </summary>
        BSPE_F9_SABS_ANNUALGROWTH,
        /// <summary>
        /// 证券类资产负债表(资产百分比)	
        /// </summary>
        BSPE_F9_SABS_ASSETSPERCENTAGE,
        /// <summary>
        /// 银行类利润表(销售百分比)
        /// </summary>
        BSPE_F9_BP_SALESPERCENTAGE,
        /// <summary>
        /// 银行类利润表(同比增长率)
        /// </summary>
        BSPE_F9_BP_ANNUALGROWTH,
        /// <summary>
        /// 银行类现金流量表(同比增长率)
        /// </summary>
        BSPE_F9_BSOCF_TB,
        /// <summary>
        /// 保险类利润表(同比增长率)
        /// </summary>
        BSPE_F9_IIS_ANNUALGROWTH,
        /// <summary>
        /// 保险类利润表(销售百分比)
        /// </summary>
        BSPE_F9_IIS_SALESPERCENTAGE,
        /// <summary>
        ///保险类现金流量表(同比增长率) 
        /// </summary>
        BSPE_F9_ISOCF_ANNUALGROWTH,
        /// <summary>
        /// 非金融类利润表(销售百分比)
        /// </summary>
        BSPE_F9_NFPF_SALESPERCENTAGE,
        /// <summary>
        ///非金融类利润表(同比增长率) 
        /// </summary>
        BSPE_F9_NFPF_ANNUALGROWTH,
        /// <summary>
        /// 非金融类现金流量表(同比增长率)	
        /// </summary>
        BSPE_F9_NFSOCF_ANNUALGROWTH,
        /// <summary>
        /// 证券类现金流量表(同比增长率)
        /// </summary>
        BSPE_F9_SCFT_ANNUALGROWTH,
        /// <summary>
        /// 证券类利润表(销售百分比)
        /// </summary>
        BSPE_F9_SIS_SALESPERCENTAGE,
        /// <summary>
        /// 证券类利润表(同比增长率)
        /// </summary>
        BSPE_F9_SIS_ANNUALGROWTH,

        #endregion

        /// <summary>
        /// F9基本条款
        /// </summary>
        BSPE_F9_BASIC_TERMS,
        /// <summary>
        /// F9发行情况
        /// </summary>
        BSPE_F9_ISSUE_RESULT,
        /// <summary>
        /// F9续发情况
        /// </summary>
        BSPE_F9_ADD_ISSUE,
        /// <summary>
        /// F9兑付情况
        /// </summary>
        BSPE_F9_PAYMENT,
        /// <summary>
        /// F9机构投资者
        /// </summary>
        BSPE_F9_INVESTOR,
        /// <summary>
        /// F9机构投资者统计
        /// </summary>
        BSPE_F9_INVESTOR_N,
        /// <summary>
        /// 基本条款_统计
        /// </summary>
        BSPE_F9_BASIC_TERMS_STA,
        /// <summary>
        /// 中证估值
        /// </summary>
        //BSPE_F9_VAL_ZHONGZHENG,
        BSPE_ZHONGZHENG_GUZHI,
        /// <summary>
        /// 中证估值统计
        /// </summary>
        //BSPE_F9_VAL_ZHONGZHENG_GZ,
        BSPE_ZHONGZHENG_GUZHI_STAT,
        /// <summary>
        /// 中债估值
        /// </summary>
        //BSPE_F9_VAL_ZHONGZHAI,
        BSPE_ZHONGZHAI_GUZHI,
        /// <summary>
        /// 清算所估值
        /// </summary>
        BSPE_F9_VAL_QINGSUANSUO,
        /// <summary>
        /// F9发行人资料
        /// </summary>
        BSPE_F9_ISSUER,
        /// <summary>
        ///转债发行人资料--统计
        /// </summary>
        BSPE_F9_SWAPBOND_ISSUER_N,
        /// <summary>
        /// 明细表
        /// </summary>
        BSPE_BOND_YUER_FENXI_MX,
        /// <summary>
        /// 财务状况统计
        /// </summary>
        BSPE_CAIWU_ZHUANGKUANG_THREE_TJ,
        /// <summary>
        /// F9发行人股东情况
        /// </summary>
        BSPE_F9_ISSUER_HOLDER,
        /// <summary>
        /// F9信用评级
        /// </summary>
        BSPE_F9_RATING,
        /// <summary>
        /// F9担保人资料
        /// </summary>
        BSPE_F9_GUARANTOR,
        /// <summary>
        /// F9可转债条款
        /// </summary>
        BSPE_F9_CONV_CLAUSE,
        /// <summary>
        /// 债券F9债券类型
        /// </summary>
        BSPE_F9_BOND_TYPE,
        /// <summary>
        /// F9现金流
        /// </summary>
        BSPE_F9_CASH_FLOW,
        /// <summary>
        /// 可转债条款
        /// </summary>
        BSPE_CONV_CLAUSE,
        /// <summary>
        /// 转股价格调整
        /// </summary>
        BSPE_CONV_PRICE_ADJUST,
        /// <summary>
        /// 未转股余额
        /// </summary>
        BSPE_NONCONV_BAL,
        /// <summary>
        /// 转债发行人资料
        /// </summary>
        BSPE_F9_SWAPBOND_ISSUER,
        /// <summary>
        /// 可转债发行
        /// </summary>
        BSPE_F9_SWAPBOND_ISSUE,
        /// <summary>
        /// 可转债发行结果
        /// </summary>
        BSPE_F9_SWAPBOND_ISSUE_RESULT,
        /// <summary>
        /// 申购者名单
        /// </summary>
        BSPE_F9_SWAPBOND_INVESTOR,
        /// <summary>
        /// 募集资金投向
        /// </summary>
        BSPE_F9_SWAPBOND_PROJECT,
        /// <summary>
        /// 十大持有人
        /// </summary>
        BSPE_SHIDA_CHIYOUREN_TONGJI,
        /// <summary>
        /// 相同发行人统计
        /// </summary>
        BSPE_SAME_FAXINGREN_TONGJI,
        /// <summary>
        /// 相同信用等级统计
        /// </summary>
        BSPE_SAME_RANKING_TONGJI,
        /// <summary>
        /// 相同行业统计
        /// </summary>
        BSPE_SAME_HANGYE_TONGJI,
        /// <summary>
        /// 估值数据
        /// </summary>
        BSPE_F9_BOND_VALUATIONS,
        /// <summary>
        /// 债券余额分析统计
        /// </summary>
        BSPE_BOND_YUER_FENXI_TONGJI,
        /// <summary>
        /// 担保状况
        /// </summary>
        BSPE_DANBAO_ZHUANGKUANG_TJ,
        /// <summary>
        /// 历史信贷额度
        /// </summary>
        BSPE_LISHI_XINDAI_EDU,
        /// <summary>
        /// 财务状况
        /// </summary>
        BSPE_CAIWU_ZHUANGKUANG_THREE,
        /// <summary>
        /// 未来偿债现金流明细
        /// </summary>
        BSPE_F9_CASH_D,
        /// <summary>
        /// 未来偿债现金流(图)
        /// </summary>
        BSPE_F9_CASH_P,
        /// <summary>
        /// 财务摘要（单季度）
        /// </summary>
        BSPE_F9_ASQF,
        /// <summary>
        /// 银行类资产负债表
        /// </summary>
        BSPE_F9_BBS,
        /// <summary>
        /// 保险类资产负债表
        /// </summary>
        BSPE_F9_IBS,
        /// <summary>
        /// 非金融类资产负债表
        /// </summary>
        BSPE_F9_NFSOAAL,
        /// <summary>
        /// 证券类资产负债表
        /// </summary>
        BSPE_F9_SABS,
        /// <summary>
        /// 银行类利润表
        /// </summary>
        BSPE_F9_BP,
        /// <summary>
        /// 保险类利润表
        /// </summary>
        BSPE_F9_IIS,
        /// <summary>
        /// 非金融类利润表
        /// </summary>
        BSPE_F9_NFPF,
        /// <summary>
        /// 证券类利润表
        /// </summary>
        BSPE_F9_SIS,
        /// <summary>
        /// 银行类单季度现金流量表
        /// </summary>
        BSPE_F9_BQCFB,
        /// <summary>
        /// 非金融类现金流量表
        /// </summary>
        BSPE_F9_NFSOCF,
        /// <summary>
        /// 证券类现金流量表
        /// </summary>
        BSPE_F9_SQCFB,
        /// <summary>
        /// 银行类现金流量表
        /// </summary>
        BSPE_F9_BSOCF,
        /// <summary>
        /// 保险类现金流量表
        /// </summary>
        BSPE_F9_ISOCF,
        /// <summary>
        /// 非金融类单季度现金流量表
        /// </summary>
        BSPE_F9_NFQCFB,
        /// <summary>
        /// 证券类单季度现金流量表
        /// </summary>
        BSPE_F9_SCFT,
        /// <summary>
        /// 保险类单季度现金流量表
        /// </summary>
        BSPE_F9_IQCFB,
        /// <summary>
        /// 银行类单季度利润表
        /// </summary>
        BSPE_F9_BQPF,
        /// <summary>
        /// 非金融类单季度利润表
        /// </summary>
        BSPE_F9_NFQPF,
        /// <summary>
        /// 保险类单季度利润表
        /// </summary>
        BSPE_F9_IQPF,
        /// <summary>
        /// 证券类单季度利润表
        /// </summary>
        BSPE_F9_SQPF,
        /// <summary>
        /// 存货明细
        /// </summary>
        BSPE_F9_INSTOCKDETAIL,
        /// <summary>
        /// 主营构成按产品分类
        /// </summary>
        BSPE_F9_MFBPC,
        /// <summary>
        /// 财务费用明细
        /// </summary>
        BSPE_F9_FINANCECOSTS,
        /// <summary>
        /// 主营构成按行业分类
        /// </summary>
        BSPE_F9_TMCBC,
        /// <summary>
        /// 主营构成按地区分类
        /// </summary>
        BSPE_F9_TMCCBR,
        /// <summary>
        /// 财务摘要（报告期）
        /// </summary>
        BSPE_F9_SFRP,
        /// <summary>
        /// 应交税金明细
        /// </summary>
        BSPE_F9_SPT,
        /// <summary>
        /// 资产减值准备明细
        /// </summary>
        BSPE_F9_IOA,
        /// <summary>
        /// 关联方债权债务
        /// </summary>
        BSPE_F9_GLFZQZW,
        /// <summary>
        /// 应收账款大股东欠款
        /// </summary>
        BSPE_F9_YSDGDQK,
        /// <summary>
        /// 其他应收帐款帐龄结构
        /// </summary>
        BSPE_F9_QTYSZLJG,
        /// <summary>
        /// 其它应收账款大股东欠款
        /// </summary>
        BSPE_F9_QTYSDGDQK,
        /// <summary>
        /// 担保金额
        /// </summary>
        BSPE_F9_GUARANTEEDATA,
        /// <summary>
        /// 坏账准备比例明细
        /// </summary>
        BSPE_F9_HZMX,
        /// <summary>
        /// 应收账款账龄结构
        /// </summary>
        BSPE_F9_XSZKZLJG,
        /// <summary>
        /// 应收账款主要欠款人
        /// </summary>
        BSPE_F9_YSZKZYQKR,
        /// <summary>
        /// 主要其他应收款明细
        /// </summary>
        BSPE_F9_ZYQTYSKMX,
        /// <summary>
        /// 每股指标
        /// </summary>
        BSPE_F9_MEIGUZB,
        /// <summary>
        /// 资本结构和偿债能力
        /// </summary>
        BSPE_F9_CAPSTRUCTANDSOLVENCY,
        /// <summary>
        /// 现金流
        /// </summary>
        BSPE_F9_CASHFLOW,
        /// <summary>
        /// 杜邦分析
        /// </summary>
        BSPE_F9_DUPOND,
        /// <summary>
        /// 杜邦分析弹窗
        /// </summary>
        BSPE_F9_DUPONTWINDOW,
        /// <summary>
        /// 成长能力
        /// </summary>
        BSPE_F9_GROWTHINGCAP,
        /// <summary>
        /// 营运能力
        /// </summary>
        BSPE_F9_OPERATINGCAP,
        /// <summary>
        /// 盈利能力和收益质量
        /// </summary>
        BSPE_F9_PROFITANDEARNING,
        /// <summary>
        /// 单季度财务指标
        /// </summary>
        BSPE_F9_SINGLEFINANCEIND,
        /// <summary>
        /// 银行业专项指标
        /// </summary>
        BSPE_F9_TBSI,
        /// <summary>
        /// 行情分析--可转债
        /// </summary>
        BSPE_F9_CHANGE_BOND_HQFX,
        /// <summary>
        /// 行情分析--非可转债
        /// </summary>
        BSPE_F9_BOND_ANALYST,
        #endregion
        #region 指数F9
        /// <summary>
        /// 指数代码名称对应
        /// </summary>
        ISPE_INDEXCN,
        /// <summary>
        /// 历史价格日统计
        /// </summary>
        ISPE_LISHIJIAGERI1,
        /// <summary>
        /// 历史价格周统计
        /// </summary>
        ISPE_LISHIJIAGEZHOU1,
        /// <summary>
        /// 历史价格月统计
        /// </summary>
        ISPE_LISHIJIAGEYUE1,
        /// <summary>
        /// 历史价格季统计
        /// </summary>
        ISPE_LISHIJIAGEJI1,
        /// <summary>
        /// 历史价格年统计
        /// </summary>
        ISPE_LISHIJIAGENIAN1,
        /// <summary>
        /// 风险分析_日
        /// </summary>
        ISPE_FENGXIANFENXIRI,
        /// <summary>
        /// 风险分析_月
        /// </summary>
        ISPE_FENGXIANFENXIYUE,
        /// <summary>
        /// 风险分析_周
        /// </summary>
        ISPE_FENGXIANFENXIZHOU,
        /// <summary>
        /// 最新成分股
        /// </summary>
        IF9_TLI,
        /// <summary>
        /// 历史成分股
        /// </summary>
        IF9_HOS,
        /// <summary>
        /// 成分股行业分布
        /// </summary>
        IF9_CDI1,
        /// <summary>
        /// 成分股权重
        /// </summary>
        IF9_CSW,
        /// <summary>
        /// PE-Bands 6个月
        /// </summary>
        ISPE_PEBANDS1,
        /// <summary>
        /// PE-Bands 一年
        /// </summary>
        ISPE_PEBANDS2,
        /// <summary>
        /// PE-Bands 两年
        /// </summary>
        ISPE_PEBANDS3,
        /// <summary>
        /// PE-Bands 三年
        /// </summary>
        ISPE_PEBANDS4,
        /// <summary>
        /// 历史PEPB_日
        /// </summary>
        ISPE_LISHIPEPBRI,
        /// <summary>
        /// 历史PEPB_周
        /// </summary>
        ISPE_LISHIPEPBZHOU,
        /// <summary>
        /// 历史PEPB_月
        /// </summary>
        ISPE_LISHIPEPBYUE,
        /// <summary>
        /// 历史PEPB_季
        /// </summary>
        ISPE_LISHIPEPBJI,
        /// <summary>
        /// 历史PEPB_年
        /// </summary>
        ISPE_LISHIPEPBNIAN,
        /// <summary>
        /// 盈利预测统计1
        /// </summary>
        ISPE_YLYCSJZ1,
        /// <summary>
        /// 盈利预测统计2
        /// </summary>
        ISPE_YLYCYCZ1,
        /// <summary>
        /// 现金分红统计近一年
        /// </summary>
        ISPE_XIANJINFENHONG1,
        /// <summary>
        /// 现金分红统计近两年
        /// </summary>
        ISPE_XIANJINFENHONG2,
        /// <summary>
        /// 现金分红统计近三年
        /// </summary>
        ISPE_XIANJINFENHONG3,
        /// <summary>
        /// 现金分红统计近五年
        /// </summary>
        ISPE_XIANJINFENHONG4,
        /// <summary>
        /// 现金分红统计
        /// </summary>
        ISPE_XIANJINFENHONG,
        /// <summary>
        /// 首页-市场表现统计
        /// </summary>
        ISPE_FSCBIAOXIAN1,
        /// <summary>
        /// 首页-财务指标统计
        /// </summary>
        ISPE_FCAIWUZHIBIAO1,
        /// <summary>
        /// 更多市场表现统计
        /// </summary>
        ISPE_MORECSBIAOIAN1,
        /// <summary>
        /// 指数基本资料
        /// </summary>
        ISPE_INDEXBASICINFO,
        /// <summary>
        /// 任意日期区间
        /// </summary>
        ISPE_FENLEIHUIBAOPH1,
        /// <summary>
        /// 财务比率比较_成分股
        /// </summary>
        IF9_CAIWUBILVBJ1,
        /// <summary>
        /// 财务比率比较_平均值
        /// </summary>
        IF9_CAIWUBILVBJ2,
        /// <summary>
        /// 财务比率比较_中值
        /// </summary>
        IF9_CAIWUBILVBJ3,
        /// <summary>
        /// 财务数据比较_成分股
        /// </summary>
        IF9_FD1,
        /// <summary>
        /// 财务数据比较_平均值
        /// </summary>
        IF9_FD2,
        /// <summary>
        /// 财务数据比较_中值
        /// </summary>
        IF9_FD3,
        /// <summary>
        /// 市场表现对比统计
        /// </summary>
        ISPE_SCBXDUIBI1,
        /// <summary>
        /// 市场表现对比统计2
        /// </summary>
        ISPE_SCBXDUIBI2,
        /// <summary>
        /// 价值分对比统计
        /// </summary>
        ISPE_JIAZHIFENXIDUIBI1,
        /// <summary>
        /// 价值分对比统计2
        /// </summary>
        ISPE_JIAZHIFENXIDUIBI2,
        /// <summary>
        /// 盈利预测对比统计
        /// </summary>
        ISPE_YLYCDUIBI1,
        /// <summary>
        /// 盈利预测对比统计2
        /// </summary>
        ISPE_YLYCDUIBI2,
        /// <summary>
        /// 财务比率对比统计
        /// </summary>
        ISPE_CAIWUBILVDUIBI1,
        /// <summary>
        /// 财务比率对比统计2
        /// </summary>
        ISPE_CAIWUBILVDUIBI2,
        /// <summary>
        /// 财务数据对比统计
        /// </summary>
        ISPE_CAIWUSHUJUDUIBI1,
        /// <summary>
        /// 财务数据对比统计2
        /// </summary>
        ISPE_CAIWUSHUJUDUIBI2,
        /// <summary>
        /// 成分股排名-任意日期区间
        /// </summary>
        IF9_CR1,
        /// <summary>
        /// 成份股相对价值市场表现比较
        /// </summary>
        IF9_SMPCORV,
        /// <summary>
        /// 最新成分股统计
        /// </summary>
        IF9_TLI1,
        #endregion
        #region 新股日历
        SPE_NS_CALENDAR,
        SPE_NS_XGFX,
        SPE_NS_PRICEFORCAST,
        SPE_NS_SECONDNEWSTOCK,
        SPE_NS_XGSH,
        SPE_NS_MAINHOLD,
        SPE_NS_MZ,
        SPE_NS_XGBX,
        SPE_NS_INFO,
        SPE_NS_JSCW,
        SPE_NS_PR,
        SPE_NS_XGFXIPOZL,//临时解决方案 只存放当天的新股
        #endregion
        #region 基金日历
        IND_FC_FUNDLIST,
        IND_FC_CFUNDDIV,
        IND_FC_FUNDDIS,
        IND_DIVIDENDFUND,
        IND_FC_TPAR,
        IND_FC_FUNDBULL,
        IND_FC_FUNDRESO,
        IND_FUNDCFRATE,
        IND_FC_FUNDMANAGER,
        IND_FC_FUNDTRANSFORM,
        IND_FC_FINADISCLOSE,
        IND_FC_FINADISCLOSE1,
        #endregion
        #region 债券日历
        BOND_CALENDER_SPECIALREMIND,
        BOND_CALENDER_ISSUE,
        BOND_CALENDER_PAYMENT,
        BOND_CALENDER_TRANSACTIONS,
        BOND_CALENDER_RATIND_CHANGE,
        BOND_CALENDER_CONVERTIBLEBOND,
        #endregion
        #region 理财日历
        FSPE_CNYFINPRODUCT,
        FSPE_FOREXFINPRODUCT,
        FSPE_FOREXFINPRODUCTDAOQI1,
        FSPE_CNYFINPRODUCTDAOQI1,
        FSPE_QSLCCPXX,
        FSPE_QSLCCPXX1,
        FSPE_XTCPXX,
        FSPE_XTCPXX1,
        FSPE_SUNCPXX,
        FSPE_SUNCPXX1,
        #endregion
        #region 银行保险
        /// <summary>
        /// 案例设计
        /// </summary>
        FSPE_ANLISHEJI,
        /// <summary>
        /// 保险产品大全
        /// </summary>
        FSPE_BAOXIANDAQUAN,
        /// <summary>
        /// 产品资料
        /// </summary>
        FSPE_CHANPINZHILIAO,
        /// <summary>
        /// 保险公司
        /// </summary>
        FSPE_BAOXIANGONGSIZILIAO,
        /// <summary>
        /// 投资单位价格
        /// </summary>
        FSPE_TOUZHIDANWEIJIAOGE,
        /// <summary>
        /// 结算利率
        /// </summary>
        FSPE_JIESUANLILV,
        /// <summary>
        /// 产品账户关联信息
        /// </summary>
        FSPE_CHANPINZHANGHUINFO,
        #endregion
        #region 银行理财
        /// <summary>
        /// 基本资料
        /// </summary>
        FSPE_BANKPRODUCTDETAIL,
        FSPE_BANKFSYXG,
        FSPE_BANKFSYXG1,
        FSPE_BANKFSYXG2,
        FSPE_BANKFSYXG3,
        FSPE_BANKFSYXG4,
        FSPE_BANKFSYXG5,
        FSPE_BANKFSYXG6,
        FSPE_YJHBDWJZ1,
        /// <summary>
        /// 公告
        /// </summary>
        FSPE_INCOMEZHIBIAOTELL,
        /// <summary>
        /// 每日收益
        /// </summary>
        FSPE_INCOMEDAILY,
        FSPE_BANKFNINEHB,
        /// <summary>
        /// 阶段收益
        /// </summary>
        FSPE_INCOMEREGION,
        /// <summary>
        /// 产品筛选相关
        /// </summary>
        FSPE_PRODUCTCONTRAST,
        FSPE_PRODUCTCONTRAST1,
        /// <summary>
        /// 银行信息
        /// </summary>
        FSPE_BANKINFORMATION,
        /// <summary>
        /// 旗下理财产品
        /// </summary>
        FSPE_BANKPRODUCT,
        FSPE_BANKPRODUCT1,
        #endregion
        #region 基准利率
        /// <summary>
        /// 中央银行基准利率
        /// </summary>
        MMKT_CENTER_JIZHUN_LILV,
        /// <summary>
        /// 金融机构人民币存款基准利率
        /// </summary>
        MMKT_CUNKUAN_JIZHUNLILU,
        /// <summary>
        /// 金融机构人民币贷款基准利率
        /// </summary>
        MMKT_CNY_DAIKUAI_LILV,
        /// <summary>
        /// 法定存款准备金率
        /// </summary>
        MMKT_CUNKUAN_ZHUNBEIJINLU,
        /// <summary>
        /// 境内美元存款利率
        /// </summary>
        MMKT_USD_CUNKUAN_LILV,
        /// <summary>
        /// Shibor历史数据
        /// </summary>
        MMKT_SHIBOR,
        /// <summary>
        /// Shibor报价明细
        /// </summary>
        MMKT_SHIBORQUOTE,
        /// <summary>
        /// 央行公开操作
        /// </summary>
        MMKT_CB_OMO,
        /// <summary>
        /// 银行存贷利率
        /// </summary>
        MMKT_BANK_DLRATE,
        /// <summary>
        /// 市场利率
        /// </summary>
        MMKT_MAKRATE_SHIBOR,
        /// <summary>
        /// 回购利率
        /// </summary>
        MMKT_REPORATE_SH,
        #endregion
        #region 产权交易
        /// <summary>
        /// 挂牌项目
        /// </summary>
        PR_GUAPAIXIANGMU,
        /// <summary>
        /// 投资意向
        /// </summary>
        PR_TOUZIYIXIANG,
        /// <summary>
        /// 公告与动态
        /// </summary>
        PR_GONGGAODT,
        /// <summary>
        /// 法律与法规
        /// </summary>
        PR_FALVFAGUI,
        /// <summary>
        /// 行业类别
        /// </summary>
        PR_TOUZILEIBIE,
        PR_GPHY,
        #endregion
        #region 信托产品
        /// <summary>
        /// 信托公司
        /// </summary>
        FSPE_XINTUOGONGSI,
        /// <summary>
        /// 投资顾问
        /// </summary>
        FSPE_TOUZIGUWEN,
        /// <summary>
        /// 信托产品资料
        /// </summary>
        FSPE_XINTUOINFO,
        FSPE_SUNBASINF9,
        FSPE_SUNSUPERSTARTZ,
        FSPE_QSLCSUPERSTAR,
        FSPE_QSLCFBASIC,
        FND_SUNSMBJMANAGER,
        FND_SUNSMBJBASICTZGW,
        FND_SUNSMBJJXBJ,
        FND_SUNSMBJYJBX,
        FND_SUNSMJDYJBX,
        /// <summary>
        /// 信托F9首页
        /// </summary>
        FSPE_XTLCFNINECPSY1,
        FSPE_XTLCFNINECPSY2,
        FSPE_XTLCFNINECPSY3,
        FSPE_XTLCFNINECPSY4,
        FSPE_XTLCFNINECPSY5,
        FSPE_XTLCFNINECPSY6,
        /// <summary>
        /// 产品筛选
        /// </summary>
        FSPE_CHANPINSX,
        FSPE_CHANPINSX1,
        /// <summary>
        /// 产品收益
        /// </summary>
        FSPE_XINTUOSHOUYI,
        FSPE_XINTUOJINGLI,
        FSPE_XINTUOJINGLI1,
        FSPE_MANAGERPROJECTXR1,
        FSPE_MANAGERPROJECTXR,
        FSPE_SUNF9JZ,
        FSPE_QSLCSYJZ,
        FSPE_QSLCHBJZ,
        FSPE_QSCPFE,
        FSPE_QSZCGM,
        FSPE_QSSYZCPZ,
        FND_QSLCBJKEYSTOCK,
        FND_QSLCBJHSZS,
        FND_QSLCONEYEARJZ,
        FSPE_QSSYKEYZQ,
        FSPE_QSSYMANAGER,
        FSPE_SUNTIMESYHB,
        FSPE_QSSYJDHBNOTHB,
        FSPE_QSSYJDHBYESHB,
        FSPE_QSSYCOMPANY,
        FSPE_SUNYEARHB,
        FSPE_QSSYNDNOTHB,
        FSPE_QSSYNDHBYESHB,
        FSPE_SUNKEYSTOCK,
        FSPE_SUNKEYSTOCKINNER,
        FSPE_SUNMANNAGERF9,
        FSPE_SUNTZGWF9,
        /// <summary>
        /// 历任投资经理
        /// </summary>
        FSPE_LRTZMANAGER,
        FSPE_LRXMMANANGER,
        /// <summary>
        /// 经理现任产品
        /// </summary>
        FSPE_XINTUOXRJL,
        #endregion
        #region 债券理财
        FSPE_SZZSJZ,
        /// <summary>
        /// 投资经理业绩表现(现任非货币)
        /// </summary>
        FSPE_MANAGERFEICURRENY1,
        FSPE_MANAGERFEICURRENY,
        /// <summary>
        /// 投资经理业绩表现(历任非货币)
        /// </summary>
        FSPE_MANAGERHISFHB1,
        FSPE_MANAGERHISFHB,
        /// <summary>
        /// 投资经理业绩表现(现任货币型)
        /// </summary>
        FSPE_MANAGERCURRENT1,
        FSPE_MANAGERCURRENT,
        /// <summary>
        /// 投资经理业绩表现（历任货币）
        /// </summary>
        FSPE_MANAGERLRHB1,
        FSPE_MANAGERLRHB,
        /// <summary>
        /// 产品筛选
        /// </summary>
        FSPE_PRODUCTINDEX,
        FSPE_PRODUCTINDEX1,
        /// <summary>
        /// 债券公司
        /// </summary>
        FSPE_MANAGECOMPANY,
        /// <summary>
        /// 产品对比
        /// </summary>
        FSPE_PRODUCTRESULT,
        FND_QSLCBJBASIC,
        FND_QSLCBJSUMFEBD,
        FND_QSLCBJJDNEWYEAR,
        FND_QSLCBJMANAGERXX,
        FND_QSLCJXBJ,
        /// <summary>
        /// 产品资料
        /// </summary>
        FSPE_FINDETAIL,
        FSPE_FINDETAIL1,
        /// <summary>
        /// 产品份额
        /// </summary>
        FSPE_PRODUCTFENE,
        /// <summary>
        /// 管理公司旗下理财产品---非货币
        /// </summary>
        FSPE_MANAGERCOMPANYFEI,
        /// <summary>
        /// 管理公司旗下理财产品---货币
        /// </summary>
        FSPE_MANAGERCOMPANY,
        /// <summary>
        /// 产品收益货币型
        /// </summary>
        FSPE_CHANPINSHOUYIHUOBI,
        /// <summary>
        /// 产品收益非货币型
        /// </summary>
        FSPE_CHANPINSHOUYIFEIHUOBI,
        /// <summary>
        /// 收益分配
        /// </summary>
        FSPE_SHOUYIFENPEI,
        /// <summary>
        /// 产品费率
        /// </summary>
        FSPE_PRODUCTFEE,
        /// <summary>
        /// 行业分布
        /// </summary>
        FSPE_HANGYEFENBU,
        /// <summary>
        /// 股票组合
        /// </summary>
        FSPE_STOCKZUHE,
        /// <summary>
        /// 基金组合
        /// </summary>
        FSPE_FUNDZUHE,
        /// <summary>
        /// 债券组合
        /// </summary>
        FSPE_BONDZUHE,
        /// <summary>
        /// 资产配置
        /// </summary>
        FSPE_ZICANPEIZHI,
        /// <summary>
        /// 财务指标
        /// </summary>
        FSPE_CAIWUZHIBIAO,
        /// <summary>
        /// 资产负债表
        /// </summary>
        FSPE_ZHICHANFUZAIBIAO,
        /// <summary>
        /// 经营业绩
        /// </summary>
        FSPE_JINGYINGYEJIBIAO,
        /// <summary>
        /// 分级基金
        /// </summary>
        FND_FUNDFJ,
        /// <summary>
        /// 全部持股 管理人旗下共同持有基金家数 列表指标
        /// </summary>
        FND_ALLSTOCKLIST,
        /// <summary>
        /// 重仓持股 管理人旗下共同持有基金家数 列表指标
        /// </summary>
        FND_COMPANYLIST,
        /// <summary>
        /// 成分股相对价值--价值分析比较统计
        /// </summary>
        ISPE_JIAZHIFENXIHQ1,
        /// <summary>
        /// 成分股数据-成分股排名-当前成分股
        /// </summary>
        ISPE_FENLEIHUIBAOPH03,
        /// <summary>
        /// 成分股数据-成分股行业分布-指数贡献点图
        /// </summary>
        IF9_CDI2,
        /// <summary>
        /// 成分股相对价值-盈利预测比较
        /// </summary>
        IF9_RTRVOEFC1,
        /// <summary>
        /// 成分股数据-成分股行业分布-行业权重点图
        /// </summary>
        IF9_CDI3,
        /// <summary>
        /// 盈利预测与研究报告--盈利预测-个股盈利预测-基本信息
        /// </summary>
        SP9_RATING_HOMEPAGE,
        SP9_RATING_HOMEPAGEDETAIL,
        /// <summary>
        /// 盈利预测与研究报告--盈利预测-个股盈利预测-盈利预测综合值一览
        /// </summary>
        SP9_AVG_PREDICTION,
        /// <summary>
        /// 股票的相关评级机构
        /// </summary>
        SP9_SECURITYCODE_INST,
        /// <summary>
        /// 历史预测明细
        /// </summary>
        SP9_PUBLISHINST_PREDICT,
        #endregion
        #region 全球经济日历
        /// <summary>
        /// 经济日历指标
        /// </summary>
        MAC_MACROCALENDER_VALUE,
        MAC_CALENDER_TDVALUE,
        /// <summary>
        /// 经济日历_国家地区筛选条件
        /// </summary>
        MAC_MACROCALENDER_SXTJ,
        #endregion
        #region 期货F9
        /// <summary>
        /// 持仓结构
        /// </summary>
        FUTURE_CCJG,
        /// <summary>
        /// 合约简介
        /// </summary>
        FUTURE_HYJJ,
        /// <summary>
        /// 会员机构
        /// </summary>
        FUTURE_HYJG,
        /// <summary>
        /// 价差矩阵
        /// </summary>
        FUTURE_JCJZ,
        /// <summary>
        /// 套利分析
        /// </summary>
        FUTURE_TLFX,
        /// <summary>
        /// 建仓过程
        /// </summary>
        FUTURE_JCGC,
        /// <summary>
        /// 成交持仓
        /// </summary>
        FUTURE_CJCC,
        /// <summary>
        /// 库存报告
        /// </summary>
        FUTURE_KCBG,
        /// <summary>
        /// 盈亏分析
        /// </summary>
        FUTURE_YKFX,
        /// <summary>
        /// 净仓位
        /// </summary>
        FUTURE_JCC,
        /// <summary>
        /// 合约简介统计1
        /// </summary>
        FUTURE_HYJJ1,
        /// <summary>
        /// 合约简介统计2
        /// </summary>
        FUTURE_HYJJ2,
        #endregion
        #region 期货经纪业务大全
        /// <summary>
        /// 全景速览
        /// </summary>
        FUTURECOMPANY_QJSL,
        /// <summary>
        /// 期货公司大全
        /// </summary>
        FUTURECOMPANY_QHGSDQ,
        /// <summary>
        /// 期货营业部大全
        /// </summary>
        FUTURECOMPANY_QHYYBDQ,
        /// <summary>
        /// 证券公司大全
        /// </summary>
        FUTURECOMPANY_ZQGSDQ,
        /// <summary>
        /// 证券公司名称
        /// </summary>
        FUTURECOMPANY_ZQGSMC,
        /// <summary>
        /// 期货公司名称
        /// </summary>
        FUTURECOMPANY_QHGSMC,
        /// <summary>
        /// 证券营业部大全
        /// </summary>
        FUTURECOMPANY_ZQYYBDQ,
        /// <summary>
        /// 交易排名成交持仓
        /// </summary>
        FUTURECOMPANY_JYPMCJCC,
        /// <summary>
        /// 交易排名均部
        /// </summary>
        FUTURECOMPANY_JYPMBJ,
        /// <summary>
        /// 交易排名相对地位
        /// </summary>
        FUTURECOMPANY_JYPMXDDW,
        /// <summary>
        /// 交易排名市场份额
        /// </summary>
        FUTURECOMPANY_JYPMSCFE,
        /// <summary>
        /// 期货品种
        /// </summary>
        FUTURECOMPANY_TRANSETYPE,
        /// <summary>
        /// 财务排名
        /// </summary>
        FUTURECOMPANY_CWPM,
        /// <summary>
        /// 从业人数
        /// </summary>
        FUTURECOMPANY_CYRS,
        /// <summary>
        /// 期货公司城市
        /// </summary>
        FUTURECOMPANY_CITY,
        /// <summary>
        /// 期货省份
        /// </summary>
        FUTURECOMPANY_PROVINCE,
        /// <summary>
        /// 期货省份城市对应表
        /// </summary>
        FUTURECOMPANY_QHYYBCITY,
        /// <summary>
        /// 债券省份城市对应表
        /// </summary>
        FUTURECOMPANY_ZQYYBCITY,
        /// <summary>
        /// 省份信息弹窗
        /// </summary>
        FUTURECOMPANY_SFTC,
        /// <summary>
        /// 证券公司财务
        /// </summary>
        FUTURECOMPANY_ZQGSCW,
        /// <summary>
        /// 证券公司TC
        /// </summary>
        FUTURECOMPANY_ZQGSTC,
        /// <summary>
        /// 期货公司弹出
        /// </summary>
        FUTURE_QHGSTC,
        #endregion
        #region 经纪业务大全改版
        /// <summary>
        /// 证券交易公司排名  交易数据 统计
        /// </summary>
        CUST_SEC_JYETRADE,
        /// <summary>
        /// 证券交易公司排名  市场份额 统计
        /// </summary>
        CUST_SEC_FETRADE,
        /// <summary>
        /// 证券交易公司排名  相对低位 统计
        /// </summary>
        CUST_SEC_DWTRADE,
        /// <summary>
        /// 证券交易公司排名  均部 统计
        /// </summary>
        CUST_SEC_BJTRADE,
        /// <summary>
        /// 证券公司详细
        /// </summary>
        CUST_SEC_INFODETAIL,
        /// <summary>
        /// 城市列表
        /// </summary>
        CUST_CITY,
        /// <summary>
        /// 省份列表
        /// </summary>
        CUST_PROVINCE,
        /// <summary>
        /// 证券公司大全注册地
        /// </summary>
        CUST_SEC_CITY,
        /// <summary>
        /// 证券公司大全 列表 统计
        /// </summary>
        CUST_SEC_INFO,
        /// <summary>
        /// 证券公司大全 对比 统计
        /// </summary>
        CUST_SEC_COMPARE,
        /// <summary>
        /// 券商交易额
        /// </summary>
        CUST_SEC_JYE,
        /// <summary>
        /// 营业部交易额
        /// </summary>
        CUST_SAL_JYE,
        /// <summary>
        /// 券商市场下拉框
        /// </summary>
        CUST_SEC_MARKET,
        /// <summary>
        /// 地区市场下拉框
        /// </summary>
        CUST_REG_MARKET,
        /// <summary>
        /// 营业部下拉框
        /// </summary>
        CUST_SAL_MARKET,
        /// <summary>
        /// 证券公司交易数据 首页前十 统计
        /// </summary>
        CUST_SEC_JYENEWRANK,
        /// <summary>
        /// 证券公司市场份额 首页前十 统计
        /// </summary>
        CUST_SEC_FENEWRANK,
        /// <summary>
        /// 证券公司相对地位 首页前十 统计
        /// </summary>
        CUST_SEC_DWNEWRANK,
        /// <summary>
        /// 证券公司部均 首页前十 统计
        /// </summary>
        CUST_SEC_BJNEWRANK,
        /// <summary>
        /// 券商交易数据 全部 统计
        /// </summary>
        CUST_SECALL_JYENEWRANK,
        CUST_REG_SECJYERANKDETAIL,
        /// <summary>
        /// 券商市场份额 全部 统计
        /// </summary>
        CUST_SECALL_FENEWRANK,
        CUST_REG_SECFERANKDETAIL,
        /// <summary>
        /// 券商相对地位 全部 统计
        /// </summary>
        CUST_SECALL_DWNEWRANK,
        CUST_REG_SECDWRANKDETAIL,
        /// <summary>
        /// 券商部均 全部 统计
        /// </summary>
        CUST_SECALL_BJNEWRANK,
        CUST_REG_SECBJRANKDETAIL,
        /// <summary>
        /// 地区交易额 统计
        /// </summary>
        CUST_REG_JYETRADE,
        /// <summary>
        /// 地区市场份额 统计
        /// </summary>
        CUST_REG_FETRADE,
        /// <summary>
        /// 地区相对地位 统计
        /// </summary>
        CUST_REG_DWTRADE,
        /// <summary>
        /// 地区部均 统计
        /// </summary>
        CUST_REG_BJTRADE,
        /// <summary>
        /// 券商财务数据
        /// </summary>
        CUST_SEC_STM,
        /// <summary>
        /// 证券公司大全
        /// </summary>
        CUST_SEC_INFOMATION,
        /// <summary>
        /// 资产负债表
        /// </summary>
        CUST_SEC_BLANCE,
        /// <summary>
        /// 券商基本报表
        /// </summary>
        CUST_SEC_BASIC,
        /// <summary>
        /// 历史表现
        /// </summary>
        CUST_SEC_OLDDATA,
        /// <summary>
        /// 券商部均
        /// </summary>
        CUST_SEC_BJ,
        /// <summary>
        /// 证券公司及营业部分布图
        /// </summary>
        CUST_COUNTA,
        /// <summary>
        /// 首页 地区  交易数据前十 统计
        /// </summary>
        CUST_REG_JYENEWRANK,
        /// <summary>
        /// 首页 地区 市场份额前十 统计 
        /// </summary>
        CUST_REG_FENEWRANK,
        /// <summary>
        /// 首页 地区 相对地位前十 统计
        /// </summary>
        CUST_REG_DWNEWRANK,
        /// <summary>
        /// 首页地区 部均前十 统计
        /// </summary>
        CUST_REG_BJNEWRANK,
        /// <summary>
        /// 首页营业部  交易数据前十 统计
        /// </summary>
        CUST_SAL_NEWRANK,
        CUST_SALALL_JYENEWRANK,
        CUST_REG_SALJYERANKDETAIL,
        /// <summary>
        /// 首页营业部 市场份额前十 统计
        /// </summary>
        CUST_SAL_FENEWRANK,
        CUST_SALALL_FENEWRANK,
        CUST_REG_SALFERANKDETAIL,
        /// <summary>
        /// 首页营业部 相对地位 统计
        /// </summary>
        CUST_SAL_DWNEWRANK,
        CUST_SALALL_DWNEWRANK,
        CUST_REG_SALDWRANKDETAIL,
        /// <summary>
        /// 地区弹窗 注册证券公司
        /// </summary>
        CUST_REG_SECINFO,
        /// <summary>
        /// 地区弹窗 证券公司某地区交易额
        /// </summary>
        CUST_REG_SECJYERANK,
        /// <summary>
        /// 地区弹窗 证券公司某地市场份额
        /// </summary>
        CUST_REG_SECFERANK,
        /// <summary>
        /// 地区弹窗 证券公司某地区相对地位
        /// </summary>
        CUST_REG_SECDWRANK,
        /// <summary>
        /// 地区弹窗 证券公司某地区部均
        /// </summary>
        CUST_REG_SECBJRANK,
        /// <summary>
        /// 地区的营业部
        /// </summary>
        CUST_REG_SALINFO,
        /// <summary>
        /// 地区弹窗 营业部某地区交易数据
        /// </summary>
        CUST_REG_SALJYERANK,
        /// <summary>
        /// 地区弹窗 营业部某地区市场份额
        /// </summary>
        CUST_REG_SALFERANK,
        /// <summary>
        /// 地区弹窗 营业部某地区相对地位
        /// </summary>
        CUST_REG_SALDWRANK,
        /// <summary>
        /// 营业部大全
        /// </summary>
        CUST_SAL_INFO,
        CUST_SEC_SALINFO,
        /// <summary>
        /// 营业部比较
        /// </summary>
        CUST_SAL_COMPARE,
        /// <summary>
        /// 旗下营业部交易对比
        /// </summary>
        CUST_SEC_SALJYETRADE,
        CUST_SEC_SALDWTRADE,
        CUST_SEC_SALFETRADE,
        /// <summary>
        /// 营业部基本资料
        /// </summary>
        CUST_SAL_INFODETAIL,
        /// <summary>
        /// 营业部基本报表
        /// </summary>
        CUST_SAL_BASIC,
        /// <summary>
        /// 营业部历史表现
        /// </summary>
        CUST_SAL_OLDDATA,
        /// <summary>
        /// 营业部交易排名 交易数据
        /// </summary>
        CUST_SAL_JYETRADE,
        /// <summary>
        /// 营业部交易排名 市场份额 两个全国
        /// </summary>
        CUST_SAL_QGFETRADE,
        /// <summary>
        /// 营业部交易排名 市场份额 省份全国
        /// </summary>
        CUST_SAL_SECFETRADE,
        /// <summary>
        /// 营业部交易排名 市场份额 证券公司全国
        /// </summary>
        CUST_SAL_PVCFETRADE,
        /// <summary>
        /// 营业部交易排名 市场份额 两个不是全国
        /// </summary>
        CUST_SAL_FETRADE,
        /// <summary>
        /// 营业部交易排名 相对地位 两个全国
        /// </summary>
        CUST_SAL_QGDWTRADE,
        /// <summary>
        /// 营业部交易排名 相对地位 两个都非全国
        /// </summary>
        CUST_SAL_DWTRADE,
        /// <summary>
        /// 营业部交易排名 相对地位 省份非全国
        /// </summary>
        CUST_SAL_PVCDWTRADE,
        /// <summary>
        /// 营业部交易排名 相对地位 证券公司非全国
        /// </summary>
        CUST_SAL_SECDWTRADE,
        /// <summary>
        /// 证券账户本期变动分析
        /// </summary>
        CUST_ST_INVESTOROA,
        /// <summary>
        /// 证券账户本期变动分析更多
        /// </summary>
        CUST_ST_INVESTOROADETAIL,
        /// <summary>
        /// A股持有人年龄分布
        /// </summary>
        CUST_ST_ASSETREDN,
        /// <summary>
        /// A股持有人市值分布
        /// </summary>
        CUST_ST_ASMVDIST,
        /// <summary>
        /// A股开户地区分布
        /// </summary>
        CUST_ST_TREGIONFB,
        /// <summary>
        /// A股开户地区分布更多
        /// </summary>
        CUST_ST_TREGIONFBDETAIL,
        /// <summary>
        /// 地区交易额全部
        /// </summary>
        CUST_REGALL_JYENEWRANK,
        /// <summary>
        /// 地区市场份额全部
        /// </summary>
        CUST_REGALL_FENEWRANK,
        CUST_REGALL_DWNEWRANK,
        CUST_REGALL_BJNEWRANK,
        

        #endregion
        #region 铜产业链
        /// <summary>
        /// 公司分析对应证券名称
        /// </summary>
        IC_COPPER_SECURITYSHORTNAME,
        /// <summary>
        /// 公司产业链概况
        /// </summary>
        IC_COPPER_LOCATION,
        /// <summary>
        /// 公司所属矿山
        /// </summary>
        IC_COPPER_COMPANY_MINES,
        /// <summary>
        /// 产品收入贡献
        /// </summary>
        IC_COPPER_PRODUCT_REVENUE,
        /// <summary>
        /// 铜矿信息
        /// </summary>
        IC_COPPER_MINE_INFORMATION, 
        /// <summary>
        /// 采掘上市公司
        /// </summary>
        IC_COPPER_MINING_COMPANY,
        /// <summary>
        /// 冶炼上市公司
        /// </summary>
        IC_COPPER_SMELT_COMPANY,
        /// <summary>
        /// 加工上市公司
        /// </summary>
        IC_COPPER_MACHINING_COMPANY,
        /// <summary>
        /// 资源排名
        /// </summary>
        IC_COPPER_COPPER_RANKING,
        /// <summary>
        /// 产量排名
        /// </summary>
        IC_COPPER_COPPER_GOLD,
        /// <summary>
        /// 公司产能
        /// </summary>
        IC_COPPER_CAPACITY_CHANGES,
        /// <summary>
        /// 公司在建项目
        /// </summary>
        IC_COPPER_PROJECT,
        /// <summary>
        /// 公司收益预测
        /// </summary>
        IC_COPPER_FORECAST,
        IC_COPPER_FORECASTA,
        /// <summary>
        /// 同行对比-产能对比
        /// </summary>
        IC_COPPER_CONTRAST,
        /// <summary>
        /// 同行对比-收入对比
        /// </summary>
        IC_COPPER_CONTRAST2,
        IC_COPPER_RESOURCES_ZGCLYQYSL,
        /// <summary>
        /// 查看详细资料—采选资源
        /// </summary>
        IC_COPPER_RESOURCES_MINING,
        /// <summary>
        /// 查看详细资料—冶炼产能
        /// </summary>
        IC_COPPER_RESOURCES_SMELT,
        /// <summary>
        /// 查看详细资料—加工产能
        /// </summary>
        IC_COPPER_RESOURCES_MACHINING,
        #endregion
        #region 沥青产业链
        /// <summary>
        /// 沥青产业链-相关公司
        /// </summary>
        IC_ASPHALT_CONTRAST,
        #endregion
        #region 铝产业链
        /// <summary>
        /// 公司分析对应证券名称
        /// </summary>
        IC_AL_SECURITYSHORTNAME,
        /// <summary>
        /// 氧化铝相关公司
        /// </summary>
        IC_ALUMINUM_CONTRAST,
        /// <summary>
        /// 冶炼相关公司
        /// </summary>
        IC_ALUMINUM_CONTRAST2,
        /// <summary>
        /// 加工相关公司
        /// </summary>
        IC_ALUMINUM_CONTRAST3,
        /// <summary>
        /// 铝土矿信息
        /// </summary>
        IC_AL_BAUXITE,
        /// <summary>
        /// 公司产业链概况
        /// </summary>
        IC_AL_LOCATION,
        /// <summary>
        /// 产品收入贡献
        /// </summary>
        IC_AL_PRODUCT_REVENUE,
        /// <summary>
        /// 同行对比
        /// </summary>
        IC_AL_INCOME,
        /// <summary>
        /// 公司在建项目
        /// </summary>
        IC_AL_PROJECT,
        /// <summary>
        /// 查看详细资料—采选资源
        /// </summary>
        IC_AL_RESOURCES_MINING,
        /// <summary>
        /// 查看详细资料—冶炼产能
        /// </summary>
        IC_AL_RESOURCES_SMELT,
        /// <summary>
        /// 查看详细资料—加工产能
        /// </summary>
        IC_AL_RESOURCES_MACHINING,
        #endregion
        #region 动力煤产业链
        IC_COAL_SECURITYSHORTNAME,
        IC_COAL_PRODUCT_REVENUE,
        IC_COAL_INCOME,
        IC_COAL_RESERVE,
        IC_COAL_YIELD,
        IC_COAL_PROJECT,
        IC_COAL_PROPORTION_REVENUE,
        #endregion
        #region 沥青产业链
        /// <summary>
        /// 公司分析对应证券名称
        /// </summary>
        IC_ASPHALT_SECURITYSHORTNAME,
        /// <summary>
        /// 产品收入贡献
        /// </summary>
        IC_ASPHALT_PRODUCT_REVENUE,
        /// <summary>
        /// 公司在建项目
        /// </summary>
        IC_ASPHALT_PROJECT,
        /// <summary>
        /// 同行业收入对比
        /// </summary>
        IC_ASPHALT_INCOME,
        #endregion

        #region 有色金属
        /// <summary>
        /// 铜需求结构
        /// </summary>
        MAC_NONFER_TONGXQJG,
        /// <summary>
        /// 铝需求结构
        /// </summary>
        MAC_NONFER_LVXQJG,
        /// <summary>
        /// 铅需求结构
        /// </summary>
        MAC_NONFER_QIANXQJG,
        /// <summary>
        /// 锌需求结构
        /// </summary>
        MAC_NONFER_XINXQJG,
        /// <summary>
        /// 锡需求结构
        /// </summary>
        MAC_NONFER_XIXQJG,
        /// <summary>
        /// 镍需求结构
        /// </summary>
        MAC_NONFER_NIEXQJG,
        /// <summary>
        /// 钛需求结构
        /// </summary>
        MAC_NONFER_TAIXQJG,
        /// <summary>
        /// 钨需求结构
        /// </summary>
        MAC_NONFER_WUXQJG,
        /// <summary>
        /// 锗需求结构
        /// </summary>
        MAC_NONFER_ZHEXQJG,
        /// <summary>
        /// 钼需求结构
        /// </summary>
        MAC_NONFER_MUXQJG,
        /// <summary>
        /// 钽需求结构
        /// </summary>
        MAC_NONFER_TANXQJG,
        /// <summary>
        /// 钴需求结构
        /// </summary>
        MAC_NONFER_GUXQJG,
        /// <summary>
        /// 稀土需求结构
        /// </summary>
        MAC_NONFER_XITUXQJG,
        #endregion 

        #region 信息技术
        /// <summary>
        /// 网游公司信息简介
        /// </summary>
        MAC_IT_WYGSXXJJ,
        /// <summary>
        /// 各批次支付机构数量
        /// </summary>
        MAC_IT_GPCZFJGSL,
        /// <summary>
        /// 支付机构信息
        /// </summary>
        MAC_IT_ZFJGXX,
        /// <summary>
        /// 主流收益工具分析
        /// </summary>
        FND_INTERTOOLFX,
        /// <summary>
        /// 主流收益工具分析(修正)
        /// </summary>
        FND_MAJORSYGJ,

        /// <summary>
        /// 主流收益工具分析(统计)
        /// </summary>
        FND_INTERTOOLFX1,
        /// <summary>
        /// 申购规模开户数变动情况
        /// </summary>
        FND_INTERGMKHBDQK,
        #endregion
        #region 互联网基金理财
        /// <summary>
        /// 互联网理财收益播报
        /// </summary>
        FSPE_SHOUYIBB,
        #endregion
        #region 新股改革IPO
        /// <summary>
        /// 股权分置改革后IPO数据统计
        /// </summary>
        SPE_XGGG_IPOSTATISTICS1,
        /// <summary>
        /// 新股改革制度
        /// </summary>
        SPE_XGGG_REFORMSYSTEM,
        /// <summary>
        /// 历次改革市场指数表现统计(图)
        /// </summary>
        SPE_XGGG_INDEX_ZDF,
        /// <summary>
        /// 历史IPO暂停重启统计
        /// </summary>
        SPE_XXGG_RESTRAT,
        /// <summary>
        /// 历次改革市场指数表现统计(表格)
        /// </summary>
        SPE_XGGG_INDEX_TABLE,
        /// <summary>
        /// 申购收益率试算器_下拉框
        /// </summary>
        SPE_XGGG_RETURNRATE4,
        /// <summary>
        /// 申购收益率试算器_资金成本参照收益率_下拉框
        /// </summary>
        SPE_XGGG_SSQ_SYL_DROPLIST,
        /// <summary>
        /// 近期新股申购一览
        /// </summary>
        SPE_XGGG_RETURNRATE5,
        /// <summary>
        /// 历史申购资金要求统计
        /// </summary>
        SPE_XGGG_RETURNRATE3,
        #endregion
        #region 市值监控
        /// <summary>
        /// 大股东增持一览
        /// </summary>
        SPE_DGDZCYL,
        /// <summary>
        /// 大宗交易
        /// </summary>
        SPE_DAZONG,
        /// <summary>
        /// 大小非减持一览
        /// </summary>
        SPE_DXFJCYL,
        /// <summary>
        /// 交易提醒
        /// </summary>
        SPE_JIAOYITIXING,
        #endregion
    }
}
