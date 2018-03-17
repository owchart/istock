using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace OwLib
{
    /// <summary>
    /// 表格列
    /// </summary>
    [Serializable]
    public class EMGridColumn : IComparable, ICloneable
    {
        /// <summary>
        /// 日期控件值
        /// </summary>
        [Serializable]
        public class SpeicalDateControlValue
        {
            /// <summary>
            /// 获取或设置列的日期
            /// </summary>
            [XmlAttribute]
            public String DateFromColumn { get; set; }

            /// <summary>
            /// 获取或设置过滤条件
            /// </summary>
            [XmlAttribute]
            public FILTER Filter { get; set; }

            /// <summary>
            /// 获取或设置值
            /// </summary>
            [XmlAttribute]
            public String Value { get; set; }

            internal void Deserialize(XmlNode n)
            {
                if (n.Attributes["DateFromColumn"] != null)
                    this.DateFromColumn = n.Attributes["DateFromColumn"].Value;
                if (n.Attributes["Filter"] != null)
                    this.Filter = (FILTER)Enum.Parse(typeof(FILTER), n.Attributes["Filter"].Value);
                if (n.Attributes["Value"] != null)
                    this.Value = n.Attributes["Value"].Value;
            }
        }

        /// <summary>
        /// 源名
        /// </summary>
        public String SourceName { get; set; }

        private String _BandName;
        /// <summary>
        /// 列名
        /// </summary>
        public String BandName
        {
            get { return _BandName; }
            set
            {
                if (value == null)
                    return;
                if (value.Contains("（"))
                    value = value.Replace('（', '(');
                if (value.Contains("）"))
                    value = value.Replace('）', ')');
                _BandName = value;
            }
        }

        private String _Caption;
        /// <summary>
        /// 列标题
        /// </summary>
        public String Caption
        {
            get { return _Caption; }
            set
            {
                if (value == null)
                    return;
                if (value.Contains("（"))
                    value = value.Replace('（', '(');
                if (value.Contains("）"))
                    value = value.Replace('）', ')');
                _Caption = value;
            }
        }

        /// <summary>
        /// 列名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// True 显示此数据列,false表示不显示
        /// </summary>
        public bool Visible { get; set; }

        private bool _orderBy = false;

        /// <summary>
        /// 获取或设置排序方案
        /// </summary>
        public bool OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }

        private bool _isAsc = false;
        /// <summary>
        /// 获取或设置是否正向排序
        /// </summary>
        public bool IsAsc
        {
            get { return _isAsc; }
            set { _isAsc = value; }
        }

        /// <summary>
        /// 获取或设置日期过滤值格式化字符串
        /// </summary>
        [XmlAttribute]
        public String FilterDateValueFormat { get; set; }

        /// <summary>
        /// 显示字符串格式化输出样式
        /// </summary>
        public String FormatString { get; set; }


        
        /// <summary>
        /// url标题显示名称
        /// </summary>
        public String UrlShowName { get; set; }

        /// <summary>
        /// url标题指向列名
        /// </summary>
        public String UrlCaptionCol { get; set; }

        #region 响应过滤栏的过滤

        /// <summary>
        /// 是否用于过滤条件
        /// </summary>
        public bool ResponseFilter { get; set; }

        /// <summary>
        /// 过滤列使用参数
        /// </summary>
        public FILTER ResponseColumnFilter { get; set; }

        /// <summary>
        /// 绑定的过滤条件源列名
        /// </summary>
        public String ResponseFilterName { get; set; }

        #endregion

        #region 响应联动效应的过滤

        /// <summary>
        /// 是否用于过滤条件
        /// </summary>
        public bool ResponseNative { get; set; }

        /// <summary>
        /// 过滤列使用参数
        /// </summary>
        public FILTER ResponseColumnNative { get; set; }

        /// <summary>
        /// 绑定的过滤条件源列名
        /// </summary>
        public String ResponseFilterNativeName { get; set; }

        #endregion

        /// <summary>
        /// 获取或设置控制显示组
        /// </summary>
        public String ShowControl { get; set; }

        /// <summary>
        /// 获取或设置日期控件值
        /// </summary>
        [XmlElement]
        public SpeicalDateControlValue[] ControlValues { get; set; }

        /// <summary>
        /// 获取或设置自定义日期转换
        /// </summary>
        public CustomDate CustomDate { get; set; }

        /// <summary>
        /// 获取或设置绑定下拉框项的Key
        /// </summary>
        public String BindFilterKey { get; set; }

        #region 行列转置设置

        /// <summary>
        /// 一个报表最多只有一个此值为True
        /// </summary>
        public bool BlnRowGroup { get; set; }

        /// <summary>
        /// 行主键,列合并时,按照此值相等,合并;可多个主键
        /// </summary>
        public bool BlnRowKey { get; set; }

        /// <summary>
        /// 列转置标记
        /// </summary>
        public bool BlnColumnRank { get; set; }

        /// <summary>
        /// 获取或设置列名扩展
        /// </summary>
        public String StrCaptionExpress { get; set; }

        /// <summary>
        /// 获取或设置五星标记列名
        /// </summary>
        public String StrStarName { get; set; }

        #endregion

        #region 通常默认值列

        private bool _isBigText = false;

        /// <summary>
        /// 列序号
        /// </summary>
        public bool IsBigText
        {
            get { return _isBigText; }
            set { _isBigText = value; }
        }

        /// <summary>
        /// 列序号
        /// </summary>
        public int VisibleIndex { get; set; }

        private bool _isFields = true;

        /// <summary>
        /// 获取或设置是否查询此列数据
        /// </summary>
        public bool IsFields
        {
            get { return _isFields; }
            set { _isFields = value; }
        }

        /// <summary>
        /// 获取或设置分组方式
        /// </summary>
        public GROUPTYPE IsGroup { get; set; }

        /// <summary>
        /// 显示字符串格式化输出样式
        /// </summary>
        public DATATYPE ColType { get; set; }

        private int _width = 120;

        /// <summary>
        /// 列宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private bool _isWeb;

        /// <summary>
        /// 获取或设置是否web链接
        /// </summary>
        public bool IsWeb
        {
            get { return _isWeb; }
            set { _isWeb = value; }
        }

        /// <summary>
        /// 获取或设置浏览器方式
        /// </summary>
        public BrowseWay BrowseWay { get; set; }

        /// <summary>
        /// 获取或设置链接显示文字
        /// </summary>
        public String WebColShowCaption { get; set; }

        /// <summary>
        /// 获取或设置保存链接的列的列名
        /// </summary>
        public String SaveUrlColumnName { get; set; }

        /// <summary>
        /// 显示字符串格式化输出样式
        /// </summary>
        public String CaptionFilterName { get; set; }
        private String _CaptionFormat;

        
        /// <summary>
        /// 显示字符串格式化输出样式
        /// </summary>
        public String CaptionFormat
        {
            get { return _CaptionFormat; }
            set
            {
                if (value == null)
                    return;
                if (value.Contains("（"))
                    value = value.Replace('（', '(');
                if (value.Contains("）"))
                    value = value.Replace('）', ')');
                _CaptionFormat = value;
            }
        }

        /// <summary>
        /// 显示字符串格式化输出样式
        /// </summary>
        public String BandCaptionFilterName { get; set; }
        private String _BandCaptionFormat;
        /// <summary>
        /// 显示字符串格式化输出样式
        /// </summary>
        public String BandCaptionFormat
        {
            get { return _BandCaptionFormat; }
            set
            {
                if (value == null)
                    return;
                if (value.Contains("（"))
                    value = value.Replace('（', '(');
                if (value.Contains("）"))
                    value = value.Replace('）', ')');
                _BandCaptionFormat = value;
            } }

        /// <summary>
        /// 获取或设置是否汇总
        /// </summary>
        public bool Total { get; set; }

        /// <summary>
        /// 获取或设置汇总的列名
        /// </summary>
        public String TotalCaption { get; set; }

        private int _postion = 0;

        /// <summary>
        /// 获取或设置汇总列位置
        /// </summary>
        public int Postion
        {
            get { return _postion; }
            set { _postion = value; }
        }
   
        /// <summary>
        /// 获取或设置绑定过滤源名
        /// </summary>
        public String BindingDynamicName { get; set; }

        #endregion

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {

            EMGridColumn o = obj as EMGridColumn;
            if (o == null)
                return 0;
            if (VisibleIndex < o.VisibleIndex)
                return -1;
            else if (o.VisibleIndex == VisibleIndex)
                return 0;
            return 1;
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="column">数据列</param>
        public void CopyTo(ref EMGridColumn column)
        {
            column.SourceName = SourceName;
            column.Caption = Caption;
            column.Visible = Visible;
            column._orderBy = _orderBy;
            column.IsAsc = IsAsc;
            column.FormatString = FormatString;
            column.ResponseFilter = ResponseFilter;
            column.ResponseColumnFilter = ResponseColumnFilter;
            column.ResponseFilterName = ResponseFilterName;
            column.ResponseNative = ResponseNative;
            column.ResponseColumnNative = ResponseColumnNative;
            column.ResponseFilterNativeName = ResponseFilterNativeName;
            column.ShowControl = ShowControl;
            column.ControlValues = ControlValues;
            column.BlnRowGroup = BlnRowGroup;
            column.BlnRowKey = BlnRowKey;
            column.BlnColumnRank = BlnColumnRank;
            column.IsBigText = IsBigText;
            column.VisibleIndex = VisibleIndex;
            column.IsFields = IsFields;
            column.IsGroup = IsGroup;
            column.ColType = ColType;
            column.Width = Width;
            column.IsWeb = IsWeb;
            column.BrowseWay = BrowseWay;
            column.WebColShowCaption = WebColShowCaption;
            column.CaptionFilterName = CaptionFilterName;
            column.CaptionFormat = CaptionFormat;
            column.BandCaptionFilterName = BandCaptionFilterName;
            column.BandCaptionFormat = BandCaptionFormat;
            column.Total = Total;
            column.TotalCaption = TotalCaption;
            column.Postion = Postion;
            column.BindingDynamicName = BindingDynamicName;
            column.StrStarName = StrStarName;
            column.SaveUrlColumnName = SaveUrlColumnName;
            column.StrCaptionExpress = StrCaptionExpress;
            column.BindFilterKey = BindFilterKey;
          //  column.BlnRowToColSendToChart = BlnRowToColSendToChart;

        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            EMGridColumn column = new EMGridColumn
                                    {
                                        UrlCaptionCol =this.UrlCaptionCol ,
                                        UrlShowName =this.UrlShowName ,
                                        Name = this.Name,
                                        BandName = this.BandName,
                                        SourceName = this.SourceName,
                                        Caption = this.Caption,
                                        Visible = this.Visible,
                                        _orderBy = this._orderBy,
                                        IsAsc = this.IsAsc,
                                        FormatString = this.FormatString,
                                        ResponseFilter = this.ResponseFilter,
                                        ResponseColumnFilter = this.ResponseColumnFilter,
                                        ResponseFilterName = this.ResponseFilterName,
                                        ResponseNative = this.ResponseNative,
                                        ResponseColumnNative = this.ResponseColumnNative,
                                        ResponseFilterNativeName = this.ResponseFilterNativeName,
                                        ShowControl = this.ShowControl,
                                        ControlValues = this.ControlValues,
                                        BlnRowGroup = this.BlnRowGroup,
                                        BlnRowKey = this.BlnRowKey,
                                        BlnColumnRank = this.BlnColumnRank,
                                        IsBigText = this.IsBigText,
                                        VisibleIndex = this.VisibleIndex,
                                        IsFields = this.IsFields,
                                        IsGroup = this.IsGroup,
                                        ColType = this.ColType,
                                        Width = this.Width,
                                        IsWeb = this.IsWeb,
                                        BrowseWay = this.BrowseWay,
                                        WebColShowCaption = this.WebColShowCaption,
                                        SaveUrlColumnName = this.SaveUrlColumnName,
                                        CaptionFilterName = this.CaptionFilterName,
                                        CaptionFormat = this.CaptionFormat,
                                        BandCaptionFilterName = this.BandCaptionFilterName,
                                        BandCaptionFormat = this.BandCaptionFormat,
                                        Total = this.Total,
                                        TotalCaption = this.TotalCaption,
                                        Postion = this.Postion,
                                        BindingDynamicName = this.BindingDynamicName,
                                        FilterDateValueFormat = this.FilterDateValueFormat,
                                        StrStarName = this.StrStarName,
                                        StrCaptionExpress = this.StrCaptionExpress,
                                        BindFilterKey =this. BindFilterKey
                                     //   ,BlnRowToColSendToChart = this.BlnRowToColSendToChart
                                    };
            if (CustomDate != null)
            {
                column.CustomDate = new CustomDate()
                                        {
                                            BindDateType = this.CustomDate.BindDateType,
                                            CalType = this.CustomDate.CalType,
                                            DateTime = this.CustomDate.DateTime,
                                            Value = this.CustomDate.Value
                                        };


            }
            return column;
        }

        /// <summary>
        /// 根据xml配置信息给过滤对象赋值
        /// </summary>
        /// <param name="xml">文本节点</param>
        public void Deserialize(XmlNode xml)
        {
            if (xml == null) return;
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(xml.OuterXml);
            if (Doc.DocumentElement != null)
            {
                XmlNodeList ControlValuesNodes = Doc.DocumentElement.GetElementsByTagName("ControlValues");
                List<SpeicalDateControlValue> values=new List<SpeicalDateControlValue>(); 
                foreach (XmlNode node in ControlValuesNodes)
                {
                    if (node!= null)
                    {
                        SpeicalDateControlValue speicalDate = new SpeicalDateControlValue();
                        speicalDate.Deserialize(node);
                        values.Add(speicalDate);
                    }
                    
                }
                this.ControlValues = values.ToArray();
                XmlNode CustomDateNode = Doc.DocumentElement.GetElementsByTagName("CustomDate")[0];
                if (CustomDateNode != null)
                {
                    CustomDate Value = new CustomDate();
                    Value.Deserialize(CustomDateNode);
                    this.CustomDate = CustomDate;
                }
                if (Doc.DocumentElement.GetElementsByTagName("BindFilterKey")[0] != null)
                    this.BindFilterKey = Doc.DocumentElement.GetElementsByTagName("BindFilterKey")[0].InnerText;
            }
            if (xml.Attributes != null)
            {
                if (xml.Attributes["UrlCaptionCol"] != null)
                    this.UrlCaptionCol = xml.Attributes["UrlCaptionCol"].Value;
                if (xml.Attributes["UrlShowName"] != null)
                    this.UrlShowName = xml.Attributes["UrlShowName"].Value;
                if (xml.Attributes["BandCaptionFilterName"] != null)
                    this.BandCaptionFilterName = xml.Attributes["BandCaptionFilterName"].Value;
                if (xml.Attributes["BandCaptionFormat"] != null)
                    this.BandCaptionFormat = xml.Attributes["BandCaptionFormat"].Value;
                if (xml.Attributes["BandName"] != null)
                    this.BandName = xml.Attributes["BandName"].Value;
                if (xml.Attributes["BindingDynamicName"] != null)
                    this.BindingDynamicName = xml.Attributes["BindingDynamicName"].Value;
                if (xml.Attributes["BlnColumnRank"] != null)
                    this.BlnColumnRank = xml.Attributes["BlnColumnRank"].Value == "true";
                if (xml.Attributes["BlnRowGroup"] != null)
                    this.BlnRowGroup = xml.Attributes["BlnRowGroup"].Value == "true";
                if (xml.Attributes["BlnRowKey"] != null)
                    this.BlnRowKey = xml.Attributes["BlnRowKey"].Value == "true";
                if (xml.Attributes["Caption"] != null)
                    this.Caption = xml.Attributes["Caption"].Value;
                if (xml.Attributes["BrowseWay"] != null)
                    this.BrowseWay =
                        (BrowseWay) Enum.Parse(typeof (BrowseWay), xml.Attributes["BrowseWay"].Value);
                if (xml.Attributes["CaptionFilterName"] != null)
                    this.CaptionFilterName = xml.Attributes["CaptionFilterName"].Value;
                if (xml.Attributes["CaptionFormat"] != null)
                    this.CaptionFormat = xml.Attributes["CaptionFormat"].Value;
                if (xml.Attributes["ColType"] != null)
                    this.ColType = (DATATYPE) Enum.Parse(typeof (DATATYPE), xml.Attributes["ColType"].Value);
                if (xml.Attributes["FilterDateValueFormat"] != null)
                    this.FilterDateValueFormat = xml.Attributes["FilterDateValueFormat"].Value;
                if (xml.Attributes["FormatString"] != null)
                    this.FormatString = xml.Attributes["FormatString"].Value;
                if (xml.Attributes["IsAsc"] != null)
                    this.IsAsc = xml.Attributes["IsAsc"].Value == "true";
                if (xml.Attributes["IsBigText"] != null)
                    this.IsBigText = xml.Attributes["IsBigText"].Value == "true";
                if (xml.Attributes["IsFields"] != null)
                    this.IsFields = xml.Attributes["IsFields"].Value == "true";
                if (xml.Attributes["IsGroup"] != null)
                    this.IsGroup = (GROUPTYPE) Enum.Parse(typeof (GROUPTYPE), xml.Attributes["IsGroup"].Value);
                if (xml.Attributes["IsWeb"] != null)
                    this.IsWeb = xml.Attributes["IsWeb"].Value == "true";
                if (xml.Attributes["Name"] != null)
                    this.Name = xml.Attributes["Name"].Value;
                if (xml.Attributes["OrderBy"] != null)
                    this.OrderBy = xml.Attributes["OrderBy"].Value == "true";
                if (xml.Attributes["Postion"] != null)
                    this.Postion = int.Parse(xml.Attributes["Postion"].Value);
                if (xml.Attributes["ResponseColumnFilter"] != null)
                    this.ResponseColumnFilter =
                        (FILTER) Enum.Parse(typeof (FILTER), xml.Attributes["ResponseColumnFilter"].Value);
                if (xml.Attributes["ResponseColumnNative"] != null)
                    this.ResponseColumnNative =
                        (FILTER) Enum.Parse(typeof (FILTER), xml.Attributes["ResponseColumnNative"].Value);
                if (xml.Attributes["ResponseFilter"] != null)
                    this.ResponseFilter = xml.Attributes["ResponseFilter"].Value == "true";
                if (xml.Attributes["ResponseFilterName"] != null)
                    this.ResponseFilterName = xml.Attributes["ResponseFilterName"].Value;
                if (xml.Attributes["ResponseFilterNativeName"] != null)
                    this.ResponseFilterNativeName = xml.Attributes["ResponseFilterNativeName"].Value;
                if (xml.Attributes["ResponseNative"] != null)
                    this.ResponseNative = xml.Attributes["ResponseNative"].Value == "true";
                if (xml.Attributes["SaveUrlColumnName"] != null)
                    this.SaveUrlColumnName = xml.Attributes["SaveUrlColumnName"].Value;
                if (xml.Attributes["ShowControl"] != null)
                    this.ShowControl = xml.Attributes["ShowControl"].Value;
                if (xml.Attributes["SourceName"] != null)
                    this.SourceName = xml.Attributes["SourceName"].Value;
                if (xml.Attributes["StrCaptionExpress"] != null)
                    this.StrCaptionExpress = xml.Attributes["StrCaptionExpress"].Value;
                if (xml.Attributes["StrStarName"] != null)
                    this.StrStarName = xml.Attributes["StrStarName"].Value;
                if (xml.Attributes["Total"] != null)
                    this.Total = xml.Attributes["Total"].Value == "true";
                if (xml.Attributes["TotalCaption"] != null)
                    this.TotalCaption = xml.Attributes["TotalCaption"].Value;
                if (xml.Attributes["Visible"] != null)
                    this.Visible = xml.Attributes["Visible"].Value == "true";
                if (xml.Attributes["VisibleIndex"] != null)
                    this.VisibleIndex = int.Parse(xml.Attributes["VisibleIndex"].Value);
                if (xml.Attributes["WebColShowCaption"] != null)
                    this.WebColShowCaption = xml.Attributes["WebColShowCaption"].Value;
                if (xml.Attributes["Width"] != null)
                    this.Width = int.Parse(xml.Attributes["Width"].Value);
            }
        }
    }
}