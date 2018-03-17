using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace OwLib
{
    /// <summary>
    /// 过滤条件类
    /// </summary>
	[Serializable]
	public class FilterMode : ICloneable,IComparable
	{
        /// <summary>
        /// 控件类型枚举
        /// </summary>
		public enum ContnrolModeType
		{

			/// <summary>
			/// 下拉框
			/// </summary>
			ComBoxControl,

            /// <summary>
            /// 标签框
            /// </summary>
			LabbleControl,

			/// <summary>
			/// 文本输入框
			/// </summary>
			TextControl,

			/// <summary>
			/// 日期选择框
			/// </summary>
			DateControl,

			/// <summary>
			/// 按钮框
			/// </summary>
			ButtonControl,

			/// <summary>
			/// 数字选择框
			/// </summary>
			SpinnerControl,

			/// <summary>
			/// 普通复选框
			/// </summary>
			CheckedComBox,

			/// <summary>
			/// 过滤项分组框
			/// </summary>
			SCKeyCombox,

			/// <summary>
			/// 债券分类下拉框
			/// </summary>
			SCBondCombox,

			/// <summary>
			/// 下拉框过滤源
			/// </summary>
			SCGroupCombox,

			/// <summary>
			/// 选择框
			/// </summary>
			CheckBox

		}
		
        /// <summary>
        /// 值绑定类型枚举
        /// </summary>
		public enum ValueBindingType
		{
            /// <summary>
            /// 参数绑定
            /// </summary>
			BindParam,
            /// <summary>
            /// 值绑定
            /// </summary>
			BindValue
		}

		private int _rowIndex = 0;
        /// <summary>
        /// 获取或设置行数
        /// </summary>
		[XmlAttribute]
		public int RowIndex
		{
			get { return _rowIndex; }
			set { _rowIndex = value; }
		}

        /// <summary>
        /// 获取或设置控件序号
        /// </summary>
		public int Index { get; set; }

        /// <summary>
        /// 获取或设置显示文本值
        /// </summary>
		public String Text { get; set; }

        /// <summary>
        /// 获取或设置控件类型
        /// </summary>
		public ContnrolModeType ControlType { get; set; }

        /// <summary>
        /// 设置或获取是否终端显示按钮框
        /// </summary>
        public bool BlnIsshowSureButton { get; set; }

		/// <summary>
		/// 绑定的数据列名
		/// </summary>
		public String BindParam { get; set; }

		/// <summary>
		/// 绑定的数据列名(重命名)
		/// </summary>
		[XmlAttribute]
		public String OtherBindParam { get; set; }

		#region 项合并属性

		/// <summary>
		/// 绑定的数据列名
		/// </summary>
		public String Key { get; set; }

		/// <summary>
		/// 绑定的数据列名
		/// </summary>
		public String BindValue { get; set; }

		private int _mergeItems = -1;

		/// <summary>
		/// 绑定的数据列名
		/// </summary>
		public int MergeItems
		{
			get { return _mergeItems; }
			set { _mergeItems = value; }
		}

		/// <summary>
		/// 绑定的数据列名
		/// </summary>
		public ValueBindingType BindingType { get; set; }

		#endregion

		private int _xmlWidth = 160;

        /// <summary>
        /// 获取或设置默认宽度
        /// </summary>
		public int XmlWidth
		{
			get { return _xmlWidth; }
			set { _xmlWidth = value; }
		}

		/// <summary>
		/// 控件的大小(Size)
		/// </summary>
		public FILTER Filter { get; set; }

        /// <summary>
        /// 多报告期过滤框是否动态变化
        /// </summary>
        public bool BlnSCItemDynamic{ get; set; }

        /// <summary>
        /// 多报告期过滤框数据库中查询实际绑定的列值
        /// </summary>
        public String  SCItemComboColName{ get; set; }

        /// <summary>
        /// 多报告期过滤框报告期类型选择（四季度报，中报年报，年报）
        /// </summary>

        public String  SCItemComType { get; set; }

        /// <summary>
        /// 实际绑定值是否是当前时间的上一个时间单位
        /// </summary>
        public bool IsChangeToLastTime { get; set; }

		/// <summary>
		/// 设置默认值
		/// </summary>
		public object DefaultValue { get; set; }

        /// <summary>
        /// 获取或设置复杂时间默认值
        /// </summary>
		public CustomDate OtherDefaultValue { get; set; }

        /// <summary>
        /// 获取或设置是否选择最新报告期为默认内容
        /// </summary>
        public bool BlnNewReportMoren { get; set; }

        /// <summary>
        /// 获取或设置最新报告期类型
        /// </summary>
        public String ReportType { get; set; }

        /// <summary>
        /// 获取或设置最终时间值
        /// </summary>
		public CustomDate FinalCustomDateValue { get; set; }

        /// <summary>
        /// 获取或设置复杂默认内容设置
        /// </summary>
        public CustomDate FirstDefaultCustomDateValue { get; set; }

        
        /// <summary>
        /// 获取或设置报告期控件默认内容值的类型
        /// </summary>
        public int ReportValueType { get; set; }

        /// <summary>
        /// 获取或设置是否单季报
        /// </summary>
        public bool IsSingleQuarter { get; set; }

        /// <summary>
        /// 获取或设置报表类型
        /// </summary>
        public String  ReportTypeIndex { get; set; }

        /// <summary>
        /// 获取或设置从源绑定数据设置
        /// </summary>
		public BindFilter BindFilter { get; set; }

		private int _maxValue = 5000;

        /// <summary>
        /// 获取或设置设置数值范围最大值
        /// </summary>
		public int MaxValue
		{
			get { return _maxValue; }
			set { _maxValue = value; }
		}

		private bool isSendFilter = true;

        /// <summary>
        /// 获取或设置请求数据参数
        /// </summary>
		public bool IsSendFilter
		{
			get { return isSendFilter; }
			set { isSendFilter = value; }
		}
       
		private bool isHideCtrl = false;

        /// <summary>
        /// 获取或设置控制列显示
        /// </summary>
		public bool IsHideCtrl
		{
			get { return isHideCtrl; }
			set { isHideCtrl = value; }
		}
		private int _minValue = 0;

        /// <summary>
        /// 获取或设置设置数值范围最小值
        /// </summary>
		public int MinValue
		{
			get { return _minValue; }
			set { _minValue = value; }
		}

        /// <summary>
        /// 获取或设置图形过滤方式
        /// </summary>
		public FILTER ChartFilter { get; set; }


		private bool _blnDynamic;

		//根据条件为绑定列赋值
        /// <summary>
        /// 获取或设置是否动态列
        /// </summary>
		public bool BlnDynamicColumn
		{
			get { return _blnDynamic; }
			set { _blnDynamic = value; }
		}

		private bool _blnAll=true;
        /// <summary>
        /// 获取或设置是否添加全部
        /// </summary>
		public bool BlnAll
		{
			get { return _blnAll; }
			set { _blnAll = value; }
		}

        /// <summary>
        /// 获取或设置是否最大日期值取今天
        /// </summary>
        public bool BlnMaxDayIsToday { get; set; }

		private bool _blnDateToString;
        /// <summary>
        /// 是否将时间值格式化成文本
        /// </summary>
		[XmlAttribute]
		public bool BlnDateToString
		{
			get { return _blnDateToString; }
			set { _blnDateToString = value; }
		}

		private MonthOrQuarter _monthOrQuarter;

        /// <summary>
        /// 获取或设置月或季度实体
        /// </summary>
		[XmlAttribute]
		public MonthOrQuarter MonthOrQuarter
		{
			get { return _monthOrQuarter; }
			set { _monthOrQuarter = value; }
		}
        private bool _blnChangeComboValue=false;

        /// <summary>
        /// 获取或设置是否改变下拉框值
        /// </summary>
        [XmlAttribute]
        public bool BlnChangeComboValue { get { return _blnChangeComboValue; } set { _blnChangeComboValue = value; } }

        private bool _blnNeedToTradingDay = false;

        /// <summary>
        /// 获取或设置是否需要转换交易日
        /// </summary>
        [XmlAttribute]
        public bool BlnNeedToTradingDay { get { return _blnNeedToTradingDay; } set { _blnNeedToTradingDay = value; } }
	    
        private bool _blnControlCaption=true;

        /// <summary>
        /// 获取或设置是否需要控件名称
        /// </summary>
	    [XmlAttribute]
        public bool BlnControlCaption
	    {
	        get { return _blnControlCaption; }
	        set { _blnControlCaption = value; }
	    }

        /// <summary>
        /// 获取或设置运算逻辑
        /// </summary>
	    public object Filters { get; set; }
        
		/// <summary>
		/// 控件的值
		/// </summary>
		public object Value { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
		public object Tag;

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public object Clone()
		{
			FilterMode filter = new FilterMode();
			filter.BindParam = this.BindParam;
			filter.Filter = this.Filter;
			if (Value is DateTime)
				filter.Value = ((DateTime) Value).Date;
			else
				filter.Value = this.Value;
			filter.Text = this.Text;
			filter.DefaultValue = this.DefaultValue;
			filter.ControlType = this.ControlType;
			filter.Key = this.Key;
			filter.Tag = this.Tag;
			filter.isSendFilter = this.isSendFilter;
			filter.IsHideCtrl = this.IsHideCtrl;
			filter.ChartFilter = this.ChartFilter;
			filter._mergeItems = this.MergeItems;
			filter.OtherDefaultValue = this.OtherDefaultValue;
            filter.FirstDefaultCustomDateValue = this.FirstDefaultCustomDateValue;
			filter.BlnDynamicColumn = this.BlnDynamicColumn;
            filter.BlnNewReportMoren = this.BlnNewReportMoren;
            filter.ReportType = this.ReportType;
            filter.ReportValueType = this.ReportValueType;
            filter.IsSingleQuarter = this.IsSingleQuarter;
			//filter.DateFormat = this.DateFormat;
			filter._blnDateToString = this._blnDateToString;
			filter.RowIndex = this.RowIndex;
			filter._xmlWidth = this.XmlWidth;
			filter._minValue = this.MinValue;
			filter._maxValue = this.MaxValue;
			filter.BindFilter = this.BindFilter;
			filter.BindingType = this.BindingType;
			filter.BindValue = this.BindValue;
			filter.Index = this.Index;
			filter.OtherBindParam = this.OtherBindParam;
			filter.MonthOrQuarter = this.MonthOrQuarter;
			filter.BlnAll = this.BlnAll;
		    filter.BlnMaxDayIsToday = this.BlnMaxDayIsToday;
            filter._blnChangeComboValue = this._blnChangeComboValue;
            filter._blnNeedToTradingDay = this._blnNeedToTradingDay;
		    filter.BlnControlCaption = this.BlnControlCaption;
            filter.IsChangeToLastTime = this.IsChangeToLastTime;
            filter.SCItemComboColName = this.SCItemComboColName;
            filter.SCItemComType = this.SCItemComType;
            filter.BlnSCItemDynamic = this.BlnSCItemDynamic;
		    filter.ReportTypeIndex = this.ReportTypeIndex;
		    filter.BlnIsshowSureButton = this.BlnIsshowSureButton;
			return filter;
		}

        /// <summary>
        /// 转换为字符串方法
        /// </summary>
        /// <returns>转换成的字符串</returns>
		public override String ToString()
		{
			String result = String.Empty;
			switch (ControlType)
			{
				case ContnrolModeType.ButtonControl:
					result = "按钮框";
					break;
				case ContnrolModeType.CheckedComBox:
					result = "普通复选框";
					break;
				case ContnrolModeType.ComBoxControl:
					result = "下拉框";
					break;
				case ContnrolModeType.DateControl:
					result = "日期选择框";
					break;
				case ContnrolModeType.LabbleControl:
					result = "显示文本框";
					break;
				case ContnrolModeType.SCBondCombox:
					result = "远程取数据下拉框";
					break;
				case ContnrolModeType.SCGroupCombox:
					result = "统计筛选框";
					break;
				case ContnrolModeType.SCKeyCombox:
					result = "过滤项分组框";
					break;
				case ContnrolModeType.SpinnerControl:
					result = "数字选择框";
					break;
				case ContnrolModeType.TextControl:
					result = "可输入文本框";
					break;
				case ContnrolModeType.CheckBox:
					result = "普通选择框";
					break;
			}

			return result;
		}

        /// <summary>
        /// 对象对比
        /// </summary>
        /// <param name="obj">参照对象</param>
        /// <returns>对比结果</returns>
		public int CompareTo(object obj)
		{
			FilterMode filterMode = obj as FilterMode;
			if (filterMode!=null)
			{
				if (this.Index < filterMode.Index)
					return -1;
				if (this.Index==filterMode.Index)
				{
					return 0;
				}
				return 1;
			}
			return -1;
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
                XmlNodeList bindFilterNodes = Doc.DocumentElement.GetElementsByTagName("BindFilter");
                if (bindFilterNodes[0] != null)
                {
                    XmlNode n = bindFilterNodes[0];
                    BindFilter bindFilter = new BindFilter();
                    bindFilter.Deserialize(n);
                    this.BindFilter = bindFilter;

                }
                XmlNode otherDefaultValueNode = Doc.DocumentElement.GetElementsByTagName("OtherDefaultValue")[0];
                if (otherDefaultValueNode != null)
                {
                    CustomDate OtherDefaultValue = new CustomDate();
                    OtherDefaultValue.Deserialize(otherDefaultValueNode);
                    this.OtherDefaultValue = OtherDefaultValue;
                }
                XmlNode finalCustomDateValueNode = Doc.DocumentElement.GetElementsByTagName("FinalCustomDateValue")[0];
                if (finalCustomDateValueNode != null)
                {
                    CustomDate FinalCustomDateValue = new CustomDate();
                    FinalCustomDateValue.Deserialize(finalCustomDateValueNode);
                    this.FinalCustomDateValue = FinalCustomDateValue;
                }
                XmlNode FirstDefaultCustomDateValueNode =
                    Doc.DocumentElement.GetElementsByTagName("FirstDefaultCustomDateValue")[0];
                if (FirstDefaultCustomDateValueNode != null)
                {
                    CustomDate FirstDefaultCustomDateValue = new CustomDate();
                    FirstDefaultCustomDateValue.Deserialize(FirstDefaultCustomDateValueNode);
                    this.FirstDefaultCustomDateValue = FirstDefaultCustomDateValue;
                }
                if (Doc.DocumentElement.GetElementsByTagName("XmlWidth")[0] != null)
                    this.XmlWidth = int.Parse(Doc.DocumentElement.GetElementsByTagName("XmlWidth")[0].InnerText);
                if (Doc.DocumentElement.GetElementsByTagName("BlnNewReportMoren")[0] != null)
                    this.BlnNewReportMoren = Doc.DocumentElement.GetElementsByTagName("BlnNewReportMoren")[0].InnerText=="true";
                if (Doc.DocumentElement.GetElementsByTagName("ReportTypeIndex")[0] != null)
                    this.ReportTypeIndex = Doc.DocumentElement.GetElementsByTagName("ReportTypeIndex")[0].InnerText;
                if (Doc.DocumentElement.GetElementsByTagName("ReportValueType")[0] != null)
                    this.ReportValueType = int.Parse(Doc.DocumentElement.GetElementsByTagName("ReportValueType")[0].InnerText);
                if (Doc.DocumentElement.GetElementsByTagName("IsSingleQuarter")[0] != null)
                    this.IsSingleQuarter = Doc.DocumentElement.GetElementsByTagName("IsSingleQuarter")[0].InnerText == "true";
                XmlNode defaultvaluenode = Doc.DocumentElement.GetElementsByTagName("DefaultValue")[0];
                if (defaultvaluenode != null)
                {
                    foreach (XmlAttribute attribute in defaultvaluenode.Attributes)
                    {
                        if(attribute.Name.Contains("type"))
                        {
                            if (attribute.Value.Contains("dateTime"))
                            {
                                this.DefaultValue =
                                Convert.ToDateTime(defaultvaluenode.InnerText);
                            }
                            else if (attribute.Value.Contains("int") || attribute.Value.Contains("Int"))
                            {
                                this.DefaultValue =
                                int.Parse(defaultvaluenode.InnerText);
                            }
                            else
                            {
                                this.DefaultValue = defaultvaluenode.InnerText;
                            }

                        }
                    } 
                }
            }
           // BlnNewReportMoren
            if (xml.Attributes["BindParam"] != null)
                this.BindParam = xml.Attributes["BindParam"].Value;
            if (xml.Attributes["BindValue"] != null)
                this.BindValue = xml.Attributes["BindValue"].Value;
            if (xml.Attributes["BindingType"] != null)
                this.BindingType = (ValueBindingType)Enum.Parse(typeof(ValueBindingType), xml.Attributes["BindingType"].Value);
            if (xml.Attributes["BlnAll"] != null)
                this.BlnAll = xml.Attributes["BlnAll"].Value == "true";
            if (xml.Attributes["BlnChangeComboValue"] != null)
                this.BlnChangeComboValue = xml.Attributes["BlnChangeComboValue"].Value == "true";
            if (xml.Attributes["BlnControlCaption"] != null)
                this.BlnControlCaption = xml.Attributes["BlnControlCaption"].Value == "true";
            if (xml.Attributes["BlnDateToString"] != null)
                this.BlnDateToString = xml.Attributes["BlnDateToString"].Value == "true";
            if (xml.Attributes["BlnDynamicColumn"] != null)
                this.BlnDynamicColumn = xml.Attributes["BlnDynamicColumn"].Value == "true";
            if (xml.Attributes["BlnIsshowSureButton"] != null)
                this.BlnIsshowSureButton = xml.Attributes["BlnIsshowSureButton"].Value == "true";
            if (xml.Attributes["BlnMaxDayIsToday"] != null)
                this.BlnMaxDayIsToday = xml.Attributes["BlnMaxDayIsToday"].Value == "true";
            if (xml.Attributes["BlnNeedToTradingDay"] != null)
                this.BlnNeedToTradingDay = xml.Attributes["BlnNeedToTradingDay"].Value == "true";
            if (xml.Attributes["BlnSCItemDynamic"] != null)
                this.BlnSCItemDynamic = xml.Attributes["BlnSCItemDynamic"].Value == "true";
            if (xml.Attributes["ChartFilter"] != null)
                this.ChartFilter = (FILTER) Enum.Parse(typeof (FILTER), xml.Attributes["ChartFilter"].Value);
            if (xml.Attributes["ControlType"] != null)
                this.ControlType =
                    (ContnrolModeType) Enum.Parse(typeof (ContnrolModeType), xml.Attributes["ControlType"].Value);
            if (xml.Attributes["DefaultValue"] != null)
                this.DefaultValue = xml.Attributes["DefaultValue"].Value;
            if (xml.Attributes["Filter"] != null)
                this.Filter = (FILTER) Enum.Parse(typeof (FILTER), xml.Attributes["Filter"].Value);
            if (xml.Attributes["Index"] != null)
                this.Index = int.Parse(xml.Attributes["Index"].Value);
            if (xml.Attributes["IsChangeToLastTime"] != null)
                this.IsChangeToLastTime = xml.Attributes["IsChangeToLastTime"].Value == "true";
            if (xml.Attributes["IsHideCtrl"] != null)
                this.IsHideCtrl = xml.Attributes["IsHideCtrl"].Value == "true";
            if (xml.Attributes["IsSendFilter"] != null)
                this.IsSendFilter = xml.Attributes["IsSendFilter"].Value == "true";
            //if (xml.Attributes["IsSingleQuarter"] != null)
            //    this.IsSingleQuarter = xml.Attributes["IsSingleQuarter"].Value == "true";
            if (xml.Attributes["Key"] != null)
                this.Key = xml.Attributes["Key"].Value;
            if (xml.Attributes["MaxValue"] != null)
                this.MaxValue = int.Parse(xml.Attributes["MaxValue"].Value);
            if (xml.Attributes["MergeItems"] != null)
                this.MergeItems = int.Parse(xml.Attributes["MergeItems"].Value);
            if (xml.Attributes["MinValue"] != null)
                this.MinValue = int.Parse(xml.Attributes["MinValue"].Value);
            if (xml.Attributes["MonthOrQuarter"] != null)
                this.MonthOrQuarter =
                    (MonthOrQuarter) Enum.Parse(typeof (MonthOrQuarter), xml.Attributes["MonthOrQuarter"].Value);
            if (xml.Attributes["OtherBindParam"] != null)
                this.OtherBindParam = xml.Attributes["OtherBindParam"].Value;
            if (xml.Attributes["ReportType"] != null)
                this.ReportType = xml.Attributes["ReportType"].Value;
            if (xml.Attributes["RowIndex"] != null)
                this.RowIndex = int.Parse(xml.Attributes["RowIndex"].Value);
            if (xml.Attributes["SCItemComType"] != null)
                this.SCItemComType = xml.Attributes["SCItemComType"].Value;
            if (xml.Attributes["SCItemComboColName"] != null)
                this.SCItemComboColName = xml.Attributes["SCItemComboColName"].Value;
            if (xml.Attributes["Text"] != null)
                this.Text = xml.Attributes["Text"].Value;

        }
	}
}
