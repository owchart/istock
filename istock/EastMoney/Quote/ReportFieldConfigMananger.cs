using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportConfigDefines
    {
        #region 板块类别定义常量
        /// <summary>
        /// 沪深股票（A股、B股）: AStock
        /// </summary>
        public static readonly String ST_AStock = "AStock";
        /// <summary>
        /// 指数: Index
        /// </summary>
        public static readonly String ST_Index = "Index";
        /// <summary>
        /// 封闭式基金: CloseFund
        /// </summary>
        public static readonly String ST_CloseFund = "CloseFund";
        /// <summary>
        /// 开放式基金: OpenFund
        /// </summary>
        public static readonly String ST_OpenFund = "OpenFund";
        /// <summary>
        /// 理财: FinancialManager
        /// </summary>
        public static readonly String ST_FinancialManager = "FinancialManager";
        /// <summary>
        /// 债券: Bond
        /// </summary>
        public static readonly String ST_Bond = "Bond";
        /// <summary>
        /// 港股: HK
        /// </summary>
        public static readonly String ST_HKStock = "HKStock";
        /// <summary>
        /// 美股: USA
        /// </summary>
        public static readonly String ST_USAStock = "USAStock";
        /// <summary>
        /// 股指期货: IndexFuture
        /// </summary>
        public static readonly String ST_IndexFutures = "IndexFutures";
        /// <summary>
        /// 商品期货: Future
        /// </summary>
        public static readonly String ST_Futures = "Futures";
        /// <summary>
        /// 海外期货: OverSeaFuture
        /// </summary>
        public static readonly String ST_OverSeaFutures = "OverSeaFutures";
        /// <summary>
        /// 利率: InterestRate
        /// </summary>
        public static readonly String ST_InterestRate = "InterestRate";
        /// <summary>
        /// 外汇: Exchange
        /// </summary>
        public static readonly String ST_Exchange = "Exchange";
        /// <summary>
        /// 基金相关: FundHeave
        /// </summary>
        public static readonly String ST_FundHeave = "FundHeave";
        #endregion

        #region Report模板定义常量
        /// <summary>
        /// 行情报价
        /// </summary>
        public static readonly String Rep_MarketQuote = "MarketQuote";
        /// <summary>
        /// 行情报价（简览）
        /// </summary>
        public static readonly String Rep_MarketQuoteSimple = "MarketQuoteSimple";
        /// <summary>
        /// 行情报价（全景图-简览）
        /// </summary>
        public static readonly String Rep_MarketQuoteZHP_Simple = "MarketQuoteZHP_Simple";
        /// <summary>
        /// 行情报价（全景图-自选股栏目报价）
        /// </summary>
        public static readonly String Rep_MarketQuoteZHP_Cust = "MarketQuoteZHP_Cust";
        /// <summary>
        /// 行情报价（监控屏）
        /// </summary>
        public static readonly String Rep_MarketQuoteMonitor = "MarketQuoteMonitor";
        /// <summary>
        /// 行情报价（全景图1）
        /// </summary>
        public static readonly String Rep_ZHP_MarketQuote_1 = "ZHP_MarketQuote_1";
        /// <summary>
        /// 行情报价（全景图2）
        /// </summary>
        public static readonly String Rep_ZHP_MarketQuote_2 = "ZHP_MarketQuote_2";
        /// <summary>
        /// 行情报价（全景图3）
        /// </summary>
        public static readonly String Rep_ZHP_MarketQuote_3 = "ZHP_MarketQuote_3";
        /// <summary>
        /// 行情报价（全景图4）
        /// </summary>
        public static readonly String Rep_ZHP_MarketQuote_4 = "ZHP_MarketQuote_4";
        /// <summary>
        /// 可转债分析
        /// </summary>
        public static readonly String Rep_EBondAnalysis = "EBondAnalysis";
        /// <summary>
        /// 指数管理页报价
        /// </summary>
        public static readonly String Rep_IndexManangerQuote = "IndexManangerQuote";
        /// <summary>
        /// 增仓排名
        /// </summary>
        public static readonly String Rep_PositionChange = "PositionChange";
        /// <summary>
        /// 资金流向
        /// </summary>
        public static readonly String Rep_MoneyFlow = "MoneyFlow";
        /// <summary>
        /// DDE决策
        /// </summary>
        public static readonly String Rep_DDEDecision = "DDEDecision";
        /// <summary>
        /// 盈利预测
        /// </summary>
        public static readonly String Rep_ProfitForecast = "ProfitForecast";
        /// <summary>
        /// 研究报告
        /// </summary>
        public static readonly String Rep_ResearchReport = "ResearchReport";
        /// <summary>
        /// 财务数据
        /// </summary>
        public static readonly String Rep_FinancialData = "FinancialData";
        /// <summary>
        /// 重仓持股
        /// </summary>
        public static readonly String Rep_HeaveStock = "HeaveStock";
        /// <summary>
        /// 重仓行业
        /// </summary>
        public static readonly String Rep_HeaveIndustry = "HeaveIndustry";
        /// <summary>
        /// 重仓债券
        /// </summary>
        public static readonly String Rep_HeaveBond = "HeaveBond";
        /// <summary>
        /// 重仓基金
        /// </summary>
        public static readonly String Rep_HeaveFund = "HeaveFund";
        /// <summary>
        /// 基金经理
        /// </summary>
        public static readonly String Rep_FundManager = "FundManager";
        /// <summary>
        /// 国债期货公开市场操作
        /// </summary>
        public static readonly String Rep_BondFuturePublicOperate = "BondFuturePublicOperate";
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportConfigMananger
    {
        private static ReportConfigMananger _Instance;
        private ReportFileCollection _ReportFiles;
        private ReportColumnFormatCollection _ReportColumnFormatCollection;
        private ReportCustomerConfigCollection _ReportCustomerConfigCollection;
        private Dictionary<String, Dictionary<String, ReportGridConfigAdapter>> _ReportGridConfigs;

        private ReportConfigMananger()
        {
            _ReportFiles = ReportFileCollection.GetInstance();
            _ReportColumnFormatCollection = ReportColumnFormatCollection.GetInstance();
            _ReportCustomerConfigCollection = ReportCustomerConfigCollection.GetInstance();

            _ReportGridConfigs = new Dictionary<String, Dictionary<String, ReportGridConfigAdapter>>();
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ReportConfigMananger GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ReportConfigMananger();
            }

            return _Instance;
        }
        /// <summary>
        /// 重新加载系统配置文件
        /// </summary>
        public void ReloadConfig()
        {
            ReportFieldCollection.GetInstance().Reload();
            _ReportFiles.Reload();
            _ReportColumnFormatCollection.Reload();

            FieldInfoCfgFileIO.LoadConfig();
        }
        /// <summary>
        /// 获取指定板块类别下的指定报价列表（系统定义）
        /// </summary>
        public ReportGridConfigAdapter GetSystemReportConfig(String sectoryType, String reportType)
        {
            ReportFile repFile = null;
            ReportConfig repCfg = null;

            ReportGridConfigAdapter result = null;

            if (_ReportFiles.ReportFiles.TryGetValue(sectoryType, out repFile)
                && repFile.ReportConfigs.TryGetValue(reportType, out repCfg))
            {
                result = new ReportGridConfigAdapter(repFile.Name, repCfg.Name);
                result.ReportCaption = repCfg.Caption;
                result.SortCol = repCfg.SortCol;
                result.StorType = repCfg.SortType;
                result.Localized = repCfg.Localized;
                result.ColumnSequenceCanChange = repCfg.CanMoveCol;

                if (repCfg.FieldsLocked != null)
                {
                    foreach (ReportField rf in repCfg.FieldsLocked)
                    {
                        RGridColumn gridCol = BuildGridColumnByReportField(rf);
                        gridCol.Frozen = true;

                        result.ColSelected.Add(gridCol);
                    }
                }
                if (repCfg.FieldsSelected != null)
                {
                    foreach (ReportField rf in repCfg.FieldsSelected)
                    {
                        RGridColumn gridCol = BuildGridColumnByReportField(rf);

                        result.ColSelected.Add(gridCol);
                    }
                }
                if (repCfg.FieldsExpand != null)
                {
                    foreach (ReportField rf in repCfg.FieldsExpand)
                    {
                        RGridColumn gridCol = BuildGridColumnByReportField(rf);

                        result.ColExpand.Add(gridCol);
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// 重置指定板块类别下的指定报价列表
        /// </summary>
        public ReportGridConfigAdapter ResetReportConfig(String sectoryType, String reportType)
        {
            ReportFile repFile = null;
            ReportConfig repCfg = null;

            Dictionary<String, ReportGridConfigAdapter> temp = null;
            ReportGridConfigAdapter result = null;

            if (_ReportFiles.ReportFiles.TryGetValue(sectoryType, out repFile)
                && repFile.ReportConfigs.TryGetValue(reportType, out repCfg))
            {
                if (!_ReportGridConfigs.TryGetValue(sectoryType, out temp))
                {
                    temp = new Dictionary<String, ReportGridConfigAdapter>();

                    _ReportGridConfigs[sectoryType] = temp;
                }

                if (temp.TryGetValue(reportType, out result))
                {
                    result.ColSelected.Clear();
                    result.ColExpand.Clear();
                }
                else
                {
                    result = new ReportGridConfigAdapter(repFile.Name, repCfg.Name);
                }

                result.ReportCaption = repCfg.Caption;
                result.SortCol = repCfg.SortCol;
                result.StorType = repCfg.SortType;
                result.Localized = repCfg.Localized;
                result.ColumnSequenceCanChange = repCfg.CanMoveCol;

                if (repCfg.FieldsLocked != null)
                {
                    foreach (ReportField rf in repCfg.FieldsLocked)
                    {
                        RGridColumn gridCol = BuildGridColumnByReportField(rf);
                        gridCol.Frozen = true;

                        result.ColSelected.Add(gridCol);
                    }
                }
                if (repCfg.FieldsSelected != null)
                {
                    foreach (ReportField rf in repCfg.FieldsSelected)
                    {
                        RGridColumn gridCol = BuildGridColumnByReportField(rf);

                        result.ColSelected.Add(gridCol);
                    }
                }
                if (repCfg.FieldsExpand != null)
                {
                    foreach (ReportField rf in repCfg.FieldsExpand)
                    {
                        RGridColumn gridCol = BuildGridColumnByReportField(rf);

                        result.ColExpand.Add(gridCol);
                    }
                }
            }
            temp[reportType] = result;

            return result;
        }
        /// <summary>
        /// 获取指定板块类别下的指定报价列表
        /// </summary>
        public ReportGridConfigAdapter GetReportConfig(String sectoryType, String reportType)
        {
            ReportFile repFile = null;
            ReportConfig repCfg = null;
            Dictionary<String, ReportCustomerConfig> custCfgPool = null;
            ReportCustomerConfig custCfg = null;

            Dictionary<String, ReportGridConfigAdapter> temp = null;
            ReportGridConfigAdapter result = null;

            if (_ReportFiles.ReportFiles.TryGetValue(sectoryType, out repFile)
                && repFile.ReportConfigs.TryGetValue(reportType, out repCfg))
            {
                if (!_ReportGridConfigs.TryGetValue(sectoryType, out temp))
                {
                    temp = new Dictionary<String, ReportGridConfigAdapter>();

                    _ReportGridConfigs[sectoryType] = temp;
                }

                if (!temp.TryGetValue(reportType, out result))
                {
                    if (_ReportCustomerConfigCollection.CustomerConfig.TryGetValue(sectoryType, out custCfgPool)
                        && custCfgPool.TryGetValue(reportType, out custCfg))
                    {
                        result = new ReportGridConfigAdapter(repFile.Name, repCfg.Name);
                        result.ReportCaption = repCfg.Caption;

                        #region 排序字段不再落地
                        //bool custSort = false;
                        //if (repCfg.GetField(custCfg.SortColumn) != null)
                        //{
                        //    if (custCfg.ShowColumns.ContainsKey(custCfg.SortColumn))
                        //    {
                        //        custSort = true;
                        //    }
                        //    else
                        //    {
                        //        foreach (ReportField item in repCfg.FieldsLocked)
                        //        {
                        //            if (item.Name == custCfg.SortColumn)
                        //            {
                        //                custSort = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        //if (custSort)
                        //{
                        //    result.SortCol = custCfg.SortColumn;
                        //    result.StorType = custCfg.StorType;
                        //}
                        //else
                        //{
                            result.SortCol = repCfg.SortCol;
                            result.StorType = repCfg.SortType;
                        //}
                        #endregion
                        result.Localized = repCfg.Localized;
                        result.ColumnSequenceCanChange = repCfg.CanMoveCol;

                        #region 读取系统锁定字段
                        if (repCfg.FieldsLocked != null)
                        {
                            foreach (ReportField rf in repCfg.FieldsLocked)
                            {
                                RGridColumn gridCol = BuildGridColumnByReportField(rf);
                                gridCol.Frozen = true;

                                result.ColSelected.Add(gridCol);
                            }
                        }
                        #endregion
                        List<ReportField> rfPool = new List<ReportField>();
                        if (repCfg.FieldsSelected != null)
                        {
                            foreach (ReportField item in repCfg.FieldsSelected)
                            {
                                rfPool.Add(item);
                            }
                        }
                        if (repCfg.FieldsExpand != null)
                        {
                            foreach (ReportField item in repCfg.FieldsExpand)
                            {
                                rfPool.Add(item);
                            }
                        }
                        #region 用户自定义字段
                        foreach (String columnStr in custCfg.ShowColumns.Keys)
                        {
                            ReportField rf = GetReportFieldFromPool(rfPool, columnStr);

                            if (rf != null)
                            {
                                RGridColumn gridCol = BuildGridColumnByReportField(rf);
                                gridCol.Frozen = false;
                                gridCol.ColumnWidth = custCfg.ShowColumns[columnStr];

                                result.ColSelected.Add(gridCol);
                                rfPool.Remove(rf);
                            }
                        }
                        foreach (ReportField rf in rfPool)
                        {
                            RGridColumn gridCol = BuildGridColumnByReportField(rf);
                            gridCol.Frozen = false;

                            result.ColExpand.Add(gridCol);
                        }
                        #endregion
                    }
                    else
                    {
                        result = ResetReportConfig(sectoryType, reportType);
                    }
                    temp[reportType] = result;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取指定列在特定市场的格式化配置
        /// </summary>
        public ReportColumnFormat GetReportColumnFormat(MarketType marketType, String formatName)
        {
            ReportColumnFormat result = null;
            Dictionary<String, ReportColumnFormat> colFormatCollection;
            if (_ReportColumnFormatCollection.ColFormatCollection.TryGetValue(marketType, out colFormatCollection))
            {
                colFormatCollection.TryGetValue(formatName, out result);
            }

            return result;
        }
        /// <summary>
        /// 保存客户板块报价自定义列设置
        /// </summary>
        public void SaveCustomerConfig()
        {
            foreach (String sectorType in _ReportGridConfigs.Keys)
            {
                foreach (String reportType in _ReportGridConfigs[sectorType].Keys)
                {
                    ReportGridConfigAdapter reportConfig = _ReportGridConfigs[sectorType][reportType];

                    if (reportConfig.Localized)
                    {
                        Dictionary<String, ReportCustomerConfig> custCfgPool = null;
                        ReportCustomerConfig custCfg = null;
                        if (!_ReportCustomerConfigCollection.CustomerConfig.TryGetValue(sectorType, out custCfgPool))
                        {
                            custCfgPool = new Dictionary<String, ReportCustomerConfig>();
                            _ReportCustomerConfigCollection.CustomerConfig[sectorType] = custCfgPool;
                        }

                        if (!custCfgPool.TryGetValue(reportType, out custCfg))
                        {
                            custCfg = new ReportCustomerConfig(sectorType);
                            custCfg.ReportType = reportType;

                            custCfgPool[reportType] = custCfg;
                        }
                        else
                        {
                            custCfg.ShowColumns.Clear();
                        }

                        foreach (RGridColumn item in reportConfig.ColSelected)
                        {
                            if (!item.Frozen)
                            {
                                custCfg.ShowColumns[item.Name] = item.ColumnWidth;
                            }
                        }
                        custCfg.SortColumn = reportConfig.SortCol;
                        custCfg.StorType = reportConfig.StorType;
                    }
                }
            }
            _ReportCustomerConfigCollection.SaveCustomerConfig();
        }

        private ReportField GetReportFieldFromPool(List<ReportField> rfPool, String colName)
        {
            ReportField result = null;

            foreach (ReportField item in rfPool)
            {
                if (item.Name == colName)
                {
                    result = item;
                    break;
                }
            }

            return result;
        }
        private RGridColumn BuildGridColumnByReportField(ReportField rf)
        {
            RGridColumn gridCol = new RGridColumn();
            gridCol.Name = rf.Name;
            gridCol.Caption = rf.Caption;
            gridCol.BindIndex = rf.MapName;
            gridCol.Frozen = false;
            gridCol.Display = true;
            gridCol.CanOrder = rf.ContainsSubField ? false : rf.CanSort;
            gridCol.CanWidthChange = rf.CanWidthChange;
            gridCol.ColumnWidth = rf.Width;
            gridCol.ColumnHeight = 20;
            gridCol.Align = rf.Align;
            gridCol.CheckNull = rf.CheckNull;
            gridCol.ExportFlag = rf.ExportFlag;
            gridCol.ExportType = rf.ExportType;
            gridCol.DataType = rf.DataType;
            gridCol.Format = rf.Format;
            gridCol.ShowTip = rf.ShowTip;
            gridCol.TipID = rf.TipID;
            gridCol.HelpButtonID = rf.HelpButtonID;
            gridCol.ReportFieldConfig = rf;

            if (rf.ContainsSubField)
            {
                gridCol.SubColumns = new List<RGridColumn>();

                for (int subIndex = 0; subIndex < rf.SubFields.Count; subIndex++)
                {
                    ReportField subRF = rf.SubFields[subIndex];

                    RGridColumn subCol = new RGridColumn();
                    subCol.Name = subRF.Name;
                    subCol.Caption = subRF.Caption;
                    subCol.BindIndex = subRF.MapName;
                    subCol.Frozen = false;
                    subCol.Display = true;
                    subCol.CanOrder = subRF.ContainsSubField ? false : subRF.CanSort;
                    subCol.CanWidthChange = subRF.CanWidthChange;
                    subCol.ColumnWidth = subRF.Width;
                    subCol.ColumnHeight = 20;
                    subCol.Align = subRF.Align;
                    subCol.CheckNull = subRF.CheckNull;
                    subCol.ExportFlag = subRF.ExportFlag;
                    subCol.ExportType = subRF.ExportType;
                    subCol.DataType = subRF.DataType;
                    subCol.Format = subRF.Format;
                    subCol.ShowTip = subRF.ShowTip;
                    subCol.TipID = subRF.TipID;
                    subCol.HelpButtonID = subRF.HelpButtonID;
                    subCol.ReportFieldConfig = subRF;

                    gridCol.SubColumns.Add(subCol);
                }
            }

            return gridCol;
        }
    }
    /// <summary>
    /// 报价配置适配器，用于将外部系统配置文件中的配置信息转换为内部可识别配置
    /// </summary>
    public class ReportGridConfigAdapter
    {
        private List<RGridColumn> _ColSelected;
        private List<RGridColumn> _ColExpand;

        private String _reportName;
        /// <summary>
        /// Report名称（与配置文件中的ReportConfig[Name]相对应，例如：AStock，Index）
        /// </summary>
        public String ReportName
        {
            get { return _reportName; }
            set { this._reportName = value; }
        }

        private String _reportType;
        /// <summary>
        /// Report类别（与配置文件中的ReportConfig->Report[Name]相对应，例如AStock中的：MarketQuote，MarketQuoteSimple，MarketQuoteZHP）
        /// </summary>
        public String ReportType
        {
            get { return _reportType; }
            set { this._reportType = value; }
        }

        private String _reportCaption;
        /// <summary>
        /// 报表描述（与配置文件中的ReportConfig->Report[Caption]相对应，例如：行情报价，增仓排名，资金流向）
        /// </summary>
        public String ReportCaption
        {
            get { return _reportCaption; }
            set { this._reportCaption = value; }
        }

        /// <summary>
        /// ReportGridConfig的唯一标识，由ReportName和ReportType联合组成
        /// </summary>
        public String ReportIdentity { get { return ReportName + "_" + ReportType; } }

        private String _sortCol;
        /// <summary>
        /// 排序列（默认：StockCode）
        /// </summary>
        public String SortCol
        {
            get { return _sortCol; }
            set { this._sortCol = value; }
        }

        private EStorType _sortType;
        /// <summary>
        /// 排序类型（默认：EOrder.ASC）
        /// </summary>
        public EStorType StorType
        {
            get { return _sortType; }
            set { this._sortType = value; }
        }

        private bool _localiezd;
        /// <summary>
        /// ReportGrid是否可以保存的用户自定义配置中（默认：false）
        /// </summary>
        public bool Localized
        {
            get { return _localiezd; }
            set { this._localiezd = value; }
        }

        private bool _columeSequenceCanChanged;
        /// <summary>
        /// （只读）ReportGrid是否可以调整列的顺序（默认：false）
        /// </summary>
        public bool ColumnSequenceCanChange
        {
            get { return _columeSequenceCanChanged; }
            set { this._columeSequenceCanChanged = value; }
        }

        /// <summary>
        /// 显示列
        /// </summary>
        public List<RGridColumn> ColSelected
        {
            get { return _ColSelected; }
        }
        /// <summary>
        /// 备选列
        /// </summary>
        public List<RGridColumn> ColExpand
        {
            get { return _ColExpand; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReportGridConfigAdapter(String reportName, String reportType)
        {
            ReportName = reportName;
            ReportType = reportType;
            _ColSelected = new List<RGridColumn>();
            _ColExpand = new List<RGridColumn>();
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetGridColumn(List<RGridColumn> colSelected, List<RGridColumn> colUnselected)
        {
            List<RGridColumn> colFrozen = new List<RGridColumn>();
            foreach (RGridColumn item in _ColSelected)
            {
                if (item.Frozen)
                    colFrozen.Add(item);
            }

            _ColSelected.Clear();
            _ColSelected.AddRange(colFrozen);
            _ColSelected.AddRange(colSelected);

            _ColExpand.Clear();
            _ColExpand.AddRange(colUnselected);

            bool noOrderIndex = true;
            foreach (RGridColumn item in _ColSelected)
            {
                if (item.Name == SortCol)
                {
                    noOrderIndex = false;
                    break;
                }
            }
            if (noOrderIndex)
            {
                SortCol = "StockCode";
                StorType = EStorType.Asc;
            }
        }
        /// <summary>
        /// 检查是否存在InfoMine
        /// </summary>
        public bool ContainsInfoMine()
        {
            bool result = false;

            foreach (RGridColumn item in _ColSelected)
            {
                if (item.BindIndex == FieldIndex.InfoMine)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportFieldCollection : PersistableObject
    {
        private static readonly String _ConfigFile = PathUtilities.CfgPath + @"Report\" + "ReportFieldCollection-new.xml";
        private static ReportFieldCollection _Instance;

        private Dictionary<String, ReportField> _reportfields;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, ReportField> ReportFields
        {
            get { return _reportfields; }
            private set { this._reportfields = value; }
        }

        private ReportFieldCollection()
        {
            _reportfields = new Dictionary<String, ReportField>();
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ReportFieldCollection GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ReportFieldCollection();

                new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
            }
            return _Instance;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            ReportFields.Clear();
            for (int Index = 0; Index < memento.ChildCount; Index++)
            {
                IMemento childMemento = memento.GetChild(Index);
                ReportField field = new ReportField();
                field.LoadState(childMemento);
                ReportFields[field.Name] = field;
            }
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportField : PersistableObject
    {
        private String _name;
        /// <summary>
        /// 名称
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { this._name = value; }
        }

        private String _caption;
        /// <summary>
        /// 
        /// </summary>
        public String Caption
        {
            get { return _caption; }
            set { this._caption = value; }
        }

        private FieldIndex _mapName;
        /// <summary>
        /// 
        /// </summary>
        public FieldIndex MapName
        {
            get { return _mapName; }
            set { this._mapName = value; }
        }

        private StringAlignment _align;
        /// <summary>
        /// 
        /// </summary>
        public StringAlignment Align
        {
            get { return _align; }
            set { this._align = value; }
        }

        private int _width;
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { this._width = value; }
        }

        private bool _isCheckNull;
        /// <summary>
        /// 
        /// </summary>
        public bool CheckNull
        {
            get { return _isCheckNull; }
            set { this._isCheckNull = value; }
        }

        private bool _isCanSort;
        /// <summary>
        /// 是否可排序
        /// </summary>
        public bool CanSort
        {
            get { return _isCanSort; }
            set { this._isCanSort = value; }
        }

        private bool _isCanWidthChanged;
        /// <summary>
        /// 宽度能否改变
        /// </summary>
        public bool CanWidthChange
        {
            get { return _isCanWidthChanged; }
            set { this._isCanWidthChanged = value; }
        }

        private bool _isShowTip;
        /// <summary>
        /// 是否包含Tip
        /// </summary>
        public bool ShowTip
        {
            get { return _isShowTip; }
            set { this._isShowTip = value; }
        }

        private String _tipId;
        /// <summary>
        /// 需要展示的TipID
        /// </summary>
        public String TipID
        {
            get { return _tipId; }
            set { this._tipId = value; }
        }

        private String _helpButtonId;
        /// <summary>
        /// 需要展示的HelpButtonID
        /// </summary>
        public String HelpButtonID
        {
            get { return _helpButtonId; }
            set { this._helpButtonId = value; }
        }

        private String _format;
        /// <summary>
        /// 
        /// </summary>
        public String Format
        {
            get { return _format; }
            set { this._format = value; }
        }

        private String _dataTypeStr;
        /// <summary>
        /// 
        /// </summary>
        public String DataTypeStr
        {
            get { return _dataTypeStr; }
            set { this._dataTypeStr = value; }
        }

        private Type _dataType;
        /// <summary>
        /// 
        /// </summary>
        public Type DataType
        {
            get { return _dataType; }
            set { this._dataType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _exportFlag;
        /// <summary>
        /// 
        /// </summary>
        public bool ExportFlag
        {
            get { return _exportFlag; }
            set { _exportFlag = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private String _exportTypeStr;
        /// <summary>
        /// 
        /// </summary>
        public String ExportTypeStr
        {
            get { return _exportTypeStr; }
            set { _exportTypeStr = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private Type _ExportType;
        /// <summary>
        /// 
        /// </summary>
        public Type ExportType
        {
            get { return _ExportType; }
            set { _ExportType = value; }
        }

        private List<ReportField> _subFields;
        /// <summary>
        /// 
        /// </summary>
        public List<ReportField> SubFields
        {
            get { return _subFields; }
            set { this._subFields = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ContainsSubField
        {
            get { return SubFields != null && SubFields.Count > 0; }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            try
            {
                Name = memento.GetString("Name");
                Caption = memento.GetString("Caption");
                MapName = (FieldIndex)memento.GetEnumValue("MapName", typeof(FieldIndex));
                Align = (StringAlignment)memento.GetEnumValue("Align", typeof(StringAlignment));
                Width = memento.GetInteger("Width");
                CheckNull = memento.GetBoolean("CheckNull");
                CanSort = memento.GetBoolean("CanSort");
                CanWidthChange = memento.GetBoolean("CanWidthChange");
                TipID = memento.GetString("TipID") ?? String.Empty;
                ShowTip = !String.IsNullOrEmpty(TipID);
                HelpButtonID = memento.GetString("HelpButtonID") ?? String.Empty;
                Format = memento.GetString("Format");
                DataTypeStr = memento.GetString("DataType");
                DataType = GetType(DataTypeStr);
                ExportFlag = memento.GetBoolean("ExportFlag");
                ExportTypeStr = memento.GetString("ExportType") ?? DataTypeStr;
                ExportType = GetType(ExportTypeStr);
                if (memento.ChildCount > 0)
                {
                    SubFields = new List<ReportField>();
                    for (int index = 0; index < memento.ChildCount; index++)
                    {
                        ReportField subField = new ReportField();
                        subField.LoadState(memento.GetChild(index));

                        if (String.IsNullOrEmpty(subField.ExportTypeStr))
                        {
                            subField.ExportTypeStr = subField.DataTypeStr;
                            subField.ExportType = GetType(subField.ExportTypeStr);
                        }

                        SubFields.Add(subField);
                    }
                }
            }
            catch (Exception e)
            {
                String err = e.ToString();

                LogUtilities.LogMessage("报价字段配置文件读取出错，error: " + err);
            }
        }

        /// <summary>
        /// 自身的深拷贝
        /// </summary>
        /// <returns></returns>
        public ReportField DeepCopy()
        {
            ReportField result = new ReportField();

            result.Name = this.Name;
            result.Caption = this.Caption;
            result.MapName = this.MapName;
            result.Align = this.Align;
            result.Width = this.Width;
            result.CheckNull = this.CheckNull;
            result.CanSort = this.CanSort;
            result.CanWidthChange = this.CanWidthChange;
            result.ShowTip = this.ShowTip;
            result.TipID = this.TipID;
            result.HelpButtonID = this.HelpButtonID;
            result.Format = this.Format;
            result.DataTypeStr = this.DataTypeStr;
            result.DataType = this.DataType;
            result.ExportFlag = this.ExportFlag;
            result.ExportTypeStr = this.ExportTypeStr;
            result.ExportType = this.ExportType;

            if (ContainsSubField)
            {
                result.SubFields = new List<ReportField>();

                foreach (ReportField item in SubFields)
                {
                    result.SubFields.Add(item.DeepCopy());
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public Type GetType(String objTypeStr)
        {
            if (String.IsNullOrEmpty(objTypeStr))
                return null;

            switch (objTypeStr)
            {
                case "String":
                    return typeof(String);
                case "Single":
                    return typeof(Single);
                case "Double":
                    return typeof(Double);
                case "Int16":
                    return typeof(Int16);
                case "Int32":
                    return typeof(Int32);
                case "Int64":
                    return typeof(Int64);
                default:
                    return typeof(object);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportFileCollection : PersistableObject
    {
        private static readonly String _ConfigFile = PathUtilities.CfgPath + @"Report\" + "ReportFile.xml";
        private static readonly String _ReportDir = PathUtilities.CfgPath + @"Report\";
        private static ReportFileCollection _Instance;

        private Dictionary<String, ReportFile> _reportFiles;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, ReportFile> ReportFiles
        {
            get { return _reportFiles; }
            set { this._reportFiles = value; }
        }

        private ReportFileCollection()
        {
            ReportFiles = new Dictionary<String, ReportFile>();
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ReportFileCollection GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ReportFileCollection();
                new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
            }
            return _Instance;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            ReportFiles.Clear();
            for (int index = 0; index < memento.ChildCount; index++)
            {
                IMemento childMemento = memento.GetChild(index);

                String fileName = childMemento.GetString("Name");

                ReportFile reportFile = new ReportFile();
                new XmlFileSerializer(_ReportDir + fileName).Deserialization(reportFile);

                for (int subIndex = 0; subIndex < childMemento.ChildCount; subIndex++)
                {
                    IMemento subChildMemento = childMemento.GetChild(subIndex);
                    String report = subChildMemento.GetString("Name");
                    ReportFiles[report] = reportFile;
                }
            }
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportFile : PersistableObject
    {
        private String _name;
        /// <summary>
        /// 名称
        /// </summary>
        public String Name
        {
            get { return _name; }
            private set { this._name = value; }
        }

        private String _caption;
        /// <summary>
        /// 
        /// </summary>
        public String Caption
        {
            get { return _caption; }
            private set { _caption = value; }
        }

        private Dictionary<String, ReportConfig> _reportConfigs;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, ReportConfig> ReportConfigs
        {
            get { return _reportConfigs; }
            private set { _reportConfigs = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportFile()
        {
            _reportConfigs = new Dictionary<String, ReportConfig>();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            Name = memento.GetString("Name");
            Caption = memento.GetString("Caption");

            ReportConfigs.Clear();

            for (int index = 0; index < memento.ChildCount; index++)
            {
                IMemento childMemento = memento.GetChild(index);
                String reportName = childMemento.GetString("Name");

                ReportConfig rc = new ReportConfig();
                rc.LoadState(childMemento);
                ReportConfigs[reportName] = rc;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportConfig : PersistableObject
    {
        private Dictionary<String, ReportField> _ReportFieldCollection;

        private String _controlId;
        /// <summary>
        /// 
        /// </summary>
        public String ControlID
        {
            get { return _controlId; }
            private set { this._controlId = value; }
        }

        private String _name;
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        private String _caption;
        /// <summary>
        /// 
        /// </summary>
        public String Caption
        {
            get { return _caption; }
            private set { _caption = value; }
        }

        private String _sortCol;
        /// <summary>
        /// 
        /// </summary>
        public String SortCol
        {
            get { return _sortCol; }
            private set { this._sortCol = value; }
        }

        private EStorType _sortType;
        /// <summary>
        /// 
        /// </summary>
        public EStorType SortType
        {
            get { return _sortType; }
            private set { this._sortType = value; }
        }

        private bool _localized;
        /// <summary>
        /// 
        /// </summary>
        public bool Localized
        {
            get { return _localized; }
            private set { this._localized = value; }
        }

        private bool _canMoveCol;
        /// <summary>
        /// 
        /// </summary>
        public bool CanMoveCol
        {
            get { return _canMoveCol; }
            private set { _canMoveCol = value; }
        }

        private List<ReportField> _fieldsLocked;
        /// <summary>
        /// 
        /// </summary>
        public List<ReportField> FieldsLocked
        {
            get { return _fieldsLocked; }
            set { this._fieldsLocked = value; }
        }

        private List<ReportField> _feldsSelected;
        /// <summary>
        /// 
        /// </summary>
        public List<ReportField> FieldsSelected
        {
            get { return _feldsSelected; }
            set { this._feldsSelected = value; }
        }

        private List<ReportField> _fiedldsExpand;
        /// <summary>
        /// 
        /// </summary>
        public List<ReportField> FieldsExpand
        {
            get { return _fiedldsExpand; }
            set { this._fiedldsExpand = value; }
        }

        private int _lockCol;
        /// <summary>
        /// 
        /// </summary>
        public int LockCol
        {
            get { return _lockCol; }
            set { this._lockCol = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportConfig()
        {
            _ReportFieldCollection = ReportFieldCollection.GetInstance().ReportFields;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            ControlID = memento.GetString("ControlID");
            Name = memento.GetString("Name");
            Caption = memento.GetString("Caption");
            SortCol = memento.GetString("SortCol");
            SortType = (EStorType)memento.GetEnumValue("SortType", typeof(EStorType));
            Localized = memento.GetBoolean("Localized");
            CanMoveCol = memento.GetBoolean("CanMoveCol");

            for (int index = 0; index < memento.ChildCount; index++)
            {
                IMemento childMemento = memento.GetChild(index);
                if (childMemento.Name == "ColLocked")
                {
                    FieldsLocked = LoadFieldCollection(childMemento);
                }
                else if (childMemento.Name == "ColSelected")
                {
                    FieldsSelected = LoadFieldCollection(childMemento);
                }
                else if (childMemento.Name == "ColExpand")
                {
                    FieldsExpand = LoadFieldCollection(childMemento);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReportField GetField(String fieldName)
        {
            ReportField result = null;

            if (FieldsLocked != null)
            {
                foreach (ReportField item in FieldsLocked)
                {
                    if (item.Name == fieldName)
                    {
                        result = item;

                        break;
                    }
                }
            }
            if (result == null && FieldsSelected != null)
            {
                foreach (ReportField item in FieldsSelected)
                {
                    if (item.Name == fieldName)
                    {
                        result = item;

                        break;
                    }
                }
            }
            if (result == null && FieldsExpand != null)
            {
                foreach (ReportField item in FieldsExpand)
                {
                    if (item.Name == fieldName)
                    {
                        result = item;

                        break;
                    }
                }
            }

            return result;
        }

        private List<ReportField> LoadFieldCollection(IMemento memento)
        {
            List<ReportField> collection = new List<ReportField>();

            for (int index = 0; index < memento.ChildCount; index++)
            {
                IMemento childMemento = memento.GetChild(index);

                //保证不重复添加相同列
                if (GetField(childMemento.GetString("Name")) == null)
                {
                    ReportField rf = new ReportField();

                    LoadSingleField(childMemento, ref rf);
                    if (rf != null)
                    {
                        collection.Add(rf);
                    }
                }
            }

            return collection;
        }
        private void LoadSingleField(IMemento memento, ref ReportField field)
        {
            field.Name = memento.GetString("Name") ?? null;
            if (field.Name != null)
            {
                ReportField model;
                if (_ReportFieldCollection.TryGetValue(field.Name, out model))
                {
                    field.Caption = memento.GetString("Caption") ?? model.Caption;

                    if (memento.ContainsAttribute("MapName"))
                        field.MapName = (FieldIndex)memento.GetEnumValue("MapName", typeof(FieldIndex));
                    else
                        field.MapName = model.MapName;

                    if (memento.ContainsAttribute("Align"))
                        field.Align = (StringAlignment)memento.GetEnumValue("Align", typeof(StringAlignment));
                    else
                        field.Align = model.Align;

                    if (memento.ContainsAttribute("Width"))
                        field.Width = memento.GetInteger("Width");
                    else
                        field.Width = model.Width;

                    if (memento.ContainsAttribute("CheckNull"))
                        field.CheckNull = memento.GetBoolean("CheckNull");
                    else
                        field.CheckNull = model.CheckNull;

                    if (memento.ContainsAttribute("CanSort"))
                        field.CanSort = memento.GetBoolean("CanSort");
                    else
                        field.CanSort = model.CanSort;

                    if (memento.ContainsAttribute("CanWidthChange"))
                        field.CanWidthChange = memento.GetBoolean("CanWidthChange");
                    else
                        field.CanWidthChange = model.CanWidthChange;

                    if (memento.ContainsAttribute("TipID"))
                    {
                        field.TipID = memento.GetString("TipID") ?? String.Empty;
                        field.ShowTip = !String.IsNullOrEmpty(field.TipID);
                    }
                    else
                    {
                        field.TipID = model.TipID;
                        field.ShowTip = model.ShowTip;
                    }
                    if (memento.ContainsAttribute("HelpButtonID"))
                    {
                        field.HelpButtonID = memento.GetString("HelpButtonID") ?? String.Empty;
                    }
                    else
                    {
                        field.HelpButtonID = model.HelpButtonID;
                    }

                    if (memento.ContainsAttribute("Format"))
                        field.Format = memento.GetString("Format");
                    else
                        field.Format = model.Format;

                    if (memento.ContainsAttribute("DataType"))
                    {
                        field.DataTypeStr = memento.GetString("DataType");
                        field.DataType = model.GetType(field.DataTypeStr);
                    }
                    else
                    {
                        field.DataTypeStr = model.DataTypeStr;
                        field.DataType = model.DataType;
                    }

                    if (memento.ContainsAttribute("ExportFlag"))
                    {
                        field.ExportFlag = memento.GetBoolean("ExportFlag");
                    }
                    else
                    {
                        field.ExportFlag = model.ExportFlag;
                    }
                    //注意，“ExportType”继承自“DataType”，所以需要写在它的后面
                    if (memento.ContainsAttribute("ExportType"))
                    {
                        field.ExportTypeStr = memento.GetString("ExportType");
                        field.ExportType = model.GetType(field.ExportTypeStr);
                    }
                    else
                    {
                        field.ExportTypeStr = model.ExportTypeStr;
                        field.ExportType = model.ExportType;
                    }

                    if (model.ContainsSubField)
                    {
                        field.SubFields = new List<ReportField>();

                        foreach (ReportField subItemModel in model.SubFields)
                        {
                            field.SubFields.Add(subItemModel.DeepCopy());
                        }
                    }
                }
                else
                {
                    field = null;
                }
            }
            else
            {
                field = null;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportColumnFormatCollection : PersistableObject
    {
        private static readonly String _ConfigFile = PathUtilities.CfgPath + @"Report\" + "ReportFieldFormat.xml";
        private static ReportColumnFormatCollection _Instance;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<MarketType, Dictionary<String, ReportColumnFormat>> _colFormatCollection;

        public Dictionary<MarketType, Dictionary<String, ReportColumnFormat>> ColFormatCollection
        {
            get { return _colFormatCollection; }
            set { _colFormatCollection = value; }
        }

        private ReportColumnFormatCollection()
        {
            ColFormatCollection = new Dictionary<MarketType, Dictionary<String, ReportColumnFormat>>();
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ReportColumnFormatCollection GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ReportColumnFormatCollection();

                new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
            }

            return _Instance;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            for (int index = 0; index < memento.ChildCount; index++)
            {
                IMemento childMemento = memento.GetChild(index);
                String[] marketTypes = childMemento.GetString("Name").Split(',');

                //MarketType market = (MarketType)childMemento.GetEnumValue("Name", typeof(MarketType));

                Dictionary<String, ReportColumnFormat> colFormatCollection = new Dictionary<String, ReportColumnFormat>();

                for (int subIndex = 0; subIndex < childMemento.ChildCount; subIndex++)
                {
                    IMemento subChildMemento = childMemento.GetChild(subIndex);

                    ReportColumnFormat colFormat = new ReportColumnFormat();
                    colFormat.LoadState(subChildMemento);

                    colFormatCollection[colFormat.Name] = colFormat;
                }
                //ColFormatCollection[market] = colFormatCollection;

                for (int i = 0; i < marketTypes.Length; i++)
                {
                    try
                    {
                        MarketType market = (MarketType) Enum.Parse(typeof (MarketType), marketTypes[i]);

                        ColFormatCollection[market] = colFormatCollection;
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("LoadState error," + e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportColumnFormat : PersistableObject
    {
        private String _name;
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { this._name = value; }
        }

        private String _caption;
        /// <summary>
        /// 
        /// </summary>
        public String Caption
        {
            get { return _caption; }
            set { this._caption = value; }
        }

        private String _format;
        /// <summary>
        /// 
        /// </summary>
        public String Format
        {
            get { return _format; }
            set { this._format = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private String _exportFormat;
        /// <summary>
        /// 
        /// </summary>
        public String ExportFormat
        {
            get { return _exportFormat; }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Equals("None"))
                {
                    _exportFormat = null;
                }
                else
                {
                    _exportFormat = value;
                }
            }
        }

        private String _suffix;
        /// <summary>
        /// 
        /// </summary>
        public String Suffix
        {
            get { return _suffix; }
            set { this._suffix = value; }
        }

        private String _compareTo;
        /// <summary>
        /// 
        /// </summary>
        public String CompareTo
        {
            get { return _compareTo; }
            set { this._compareTo = value; }
        }

        private String _defColor;
        /// <summary>
        /// 
        /// </summary>
        public String DefColor
        {
            get { return _defColor; }
            set { this._defColor = value; }
        }

        private String _gtColor;
        /// <summary>
        /// 
        /// </summary>
        public String GtColor
        {
            get { return _gtColor; }
            set { this._gtColor = value; }
        }

        private String _ltColor;
        /// <summary>
        /// 
        /// </summary>
        public String LtColor
        {
            get { return _ltColor; }
            set { this._ltColor = value; }
        }

        private String _eqColor;
        /// <summary>
        /// 
        /// </summary>
        public String EqColor
        {
            get { return _eqColor; }
            set { this._eqColor = value; }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            Name = memento.GetString("Name");
            Caption = memento.GetString("Caption");
            Format = memento.GetString("Format");
            ExportFormat = memento.GetString("ExportFormat") ?? Format;
            Suffix = memento.GetString("Suffix");
            CompareTo = memento.GetString("CompareTo");
            DefColor = memento.GetString("DefColor");
            GtColor = memento.GetString("GtColor");
            LtColor = memento.GetString("LtColor");
            EqColor = memento.GetString("EqColor");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportCustomerConfigCollection : PersistableObject
    {
        private static readonly String _CustomerConfigFile = PathUtilities.UserPath + @"Report\" + "ReportColumnConfig.xml";
        private static ReportCustomerConfigCollection _Instance;

        /// <summary>
        /// key1=SectorType，key2=ReportType，例如：key1=AStock，key2=MarketQuote
        /// </summary>
        private Dictionary<String, Dictionary<String, ReportCustomerConfig>> _customerConfig;

        public Dictionary<String, Dictionary<String, ReportCustomerConfig>> CustomerConfig
        {
            get { return _customerConfig; }
            private set { _customerConfig = value; }
        }

        private ReportCustomerConfigCollection()
        {
            CustomerConfig = new Dictionary<String, Dictionary<String, ReportCustomerConfig>>();
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ReportCustomerConfigCollection GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ReportCustomerConfigCollection();
                if (System.IO.File.Exists(_CustomerConfigFile))
                    new XmlFileSerializer(_CustomerConfigFile).Deserialization(_Instance);
            }
            return _Instance;
        }
        /// <summary>
        /// 保存用户配置
        /// </summary>
        public void SaveCustomerConfig()
        {
            if (!System.IO.Directory.Exists(PathUtilities.UserPath + @"Report\"))
            {
                System.IO.Directory.CreateDirectory(PathUtilities.UserPath + @"Report\");
            }
            new XmlFileSerializer(_CustomerConfigFile).Serialization(_Instance);
        }
        /// <summary>
        /// 保存用户配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
            //memento.Name = this.GetType().Name;
            memento.Name = "ReportColumnConfig";

            foreach (String sectorType in CustomerConfig.Keys)
            {
                if (CustomerConfig[sectorType].Values.Count > 0)
                {
                    IMemento childMemento = memento.CreateChild("ConfigFile");
                    childMemento.SetString("Name", sectorType);

                    foreach (String reportType in CustomerConfig[sectorType].Keys)
                    {
                        ReportCustomerConfig custCfg = CustomerConfig[sectorType][reportType];

                        //TODO: 保存用户配置
                        IMemento subChildMemento = childMemento.CreateChild("ConfigColumn");
                        subChildMemento.SetString("Report", reportType);
                        subChildMemento.SetString("ShowColumns", custCfg.GetShowColumns());
                        subChildMemento.SetString("SortColumn", custCfg.GetSortColumn());
                    }
                }
            }
        }
        /// <summary>
        /// 加载用户配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            CustomerConfig.Clear();

            for (int index = 0; index < memento.ChildCount; index++)
            {
                IMemento childMemento = memento.GetChild(index);

                String sectorName = childMemento.GetString("Name");
                Dictionary<String, ReportCustomerConfig> cfgPool = null;

                if (!CustomerConfig.TryGetValue(sectorName, out cfgPool))
                {
                    cfgPool = new Dictionary<String, ReportCustomerConfig>();
                    CustomerConfig[sectorName] = cfgPool;
                }

                for (int subIndex = 0; subIndex < childMemento.ChildCount; subIndex++)
                {
                    IMemento subChildMemento = childMemento.GetChild(subIndex);

                    ReportCustomerConfig repCustCfg = new ReportCustomerConfig(sectorName);
                    repCustCfg.LoadState(subChildMemento);

                    cfgPool[repCustCfg.ReportType] = repCustCfg;
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportCustomerConfig : PersistableObject
    {
        private String _reportName;
        /// <summary>
        /// 名称
        /// </summary>
        public String ReprotName
        {
            get { return _reportName; }
            set { this._reportName = value; }
        }

        private String _reportType;
        /// <summary>
        /// 类型
        /// </summary>
        public String ReportType
        {
            get { return _reportType; }
            set { this._reportType = value; }
        }

        private Dictionary<String, int> _showColumns;
        /// <summary>
        /// 显示的列
        /// </summary>
        public Dictionary<String, int> ShowColumns
        {
            get { return _showColumns; }
            set { this._showColumns = value; }
        }//<columnName, width>

        private String _sortColumn;
        /// <summary>
        /// 排序列
        /// </summary>
        public String SortColumn
        {
            get { return _sortColumn; }
            set { this._sortColumn = value; }
        }

        private EStorType _sortType;
        /// <summary>
        /// 排序类型
        /// </summary>
        public EStorType StorType
        {
            get { return _sortType; }
            set { this._sortType = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportCustomerConfig(String reportName)
        {
            ReprotName = reportName;

            ShowColumns = new Dictionary<String, int>();
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            ShowColumns.Clear();

            ReportType = memento.GetString("Report");

            String strShowColumns = memento.GetString("ShowColumns");
            if (!String.IsNullOrEmpty(strShowColumns))
            {
                String[] showColumns = strShowColumns.Split(',');
                foreach (String showColumn in showColumns)
                {
                    if (showColumn.Contains(":"))
                    {
                        String[] columnAndWidth = showColumn.Split(':');

                        ShowColumns[columnAndWidth[0]] = Convert.ToInt32(columnAndWidth[1]);
                    }
                }
            }

            String strSortColumn = memento.GetString("SortColumn");
            if (!String.IsNullOrEmpty(strSortColumn))
            {
                if (strSortColumn.Contains(":"))
                {
                    String[] sortCfg = strSortColumn.Split(':');
                    SortColumn = sortCfg[0];
                    StorType = (EStorType)Enum.Parse(typeof(EStorType), sortCfg[1]);
                }
            }
        }
        /// <summary>
        /// 获取显示的列
        /// </summary>
        public String GetShowColumns()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String columnName in ShowColumns.Keys)
            {
                sb.AppendFormat("{0}:{1},", columnName, ShowColumns[columnName]);
            }

            if (sb.Length > 0)
            {
                sb.Length = sb.Length - 1;
            }

            return sb.ToString();
        }
        /// <summary>
        /// 获取排序列
        /// </summary>
        public String GetSortColumn()
        {
            return SortColumn + ":" + StorType.ToString();
        }
    }
}
