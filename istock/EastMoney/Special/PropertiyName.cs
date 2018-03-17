using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace OwLib
{
	/// <summary>
	/// 属性名，包括名称和标题
	/// </summary>
	[Serializable]
	public class PropertiyName:ICloneable
	{
        /// <summary>
        /// 获取或设置列名
        /// </summary>
		[XmlAttribute]
		public String NameOfCaption { get; set; }

        /// <summary>
        /// 获取或设置单元名
        /// </summary>
		[XmlAttribute]
		public String Unit { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute]
		public int xYShaft = 0;
        /// <summary>
        /// 获取或设置双轴
        /// </summary>
		public int XYShaft
		{
			get { return xYShaft; }
			set { xYShaft = value; }
		}

        /// <summary>
        /// 获取或设置类型
        /// </summary>
		[XmlAttribute]
		public String Style { get; set; }

        /// <summary>
        /// 获取或设置列名
        /// </summary>
		[XmlAttribute]
		public String BandName
		{ get; set; }
        /// <summary>
        /// 获取或设置过滤条件
        /// </summary>
        [XmlAttribute]
        public bool IsFilter { get; set; }
		//时间格式化  
        /// <summary>
        /// 获取或设置是否日期格式化
        /// </summary>
		[XmlAttribute]
				public bool BlnDateFormat { get; set; }

        /// <summary>
        /// 获取或设置数值格式化字符串
        /// </summary>
        public String FormatString { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        [XmlAttribute]
        public bool IsSelected { get; set; }
		/// <summary>
		/// 属性名称，用于传值
		/// </summary>
		[XmlElement]
		public String Name
		{ get; set; }
		/// <summary>
		/// 属性标题，用于显示
		/// </summary>
		[XmlElement]
		public String Caption
		{ get; set; }

		#region 行列转置设置

        /// <summary>
        /// 获取或设置是否行重组
        /// </summary>
		[XmlElement]
		public bool BlnRowGroup { get; set; }

        /// <summary>
        /// 获取或设置关键行
        /// </summary>
			[XmlElement]
		public bool BlnRowKey { get; set; }

        /// <summary>
        /// 获取或设置是否行转置
        /// </summary>
		[XmlElement]
		public bool BlnColumnRank { get; set; }

		#endregion

        /// <summary>
        /// 重写的转换成字符串方法
        /// </summary>
        /// <returns></returns>
		public override String ToString()
		{
			return Caption;
		}
		private bool _isDefaultChecked=false;

        /// <summary>
        /// 获取或设置是否默认勾选
        /// </summary>
		[XmlElement]
		public bool IsDefaultChecked
		{
			get { return _isDefaultChecked; }
			set { _isDefaultChecked = value; }
		}

		private int _index = -1;

        /// <summary>
        /// 获取或设置序号
        /// </summary>
		[XmlElement]
		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

		private System.Drawing. Color _lineColor = System.Drawing.Color.Empty;

        /// <summary>
        /// 获取或设置线色
        /// </summary>
		[XmlElement]
		public System.Drawing.Color LineColor
		{
			get { return _lineColor; }
			set { _lineColor = value; }
		}

        /// <summary>
        /// 对象克隆
        /// </summary>
        /// <returns>克隆返回的对象</returns>
		public object Clone()
		{
			PropertiyName propertiy = new PropertiyName();
			propertiy.NameOfCaption = this.NameOfCaption;
			propertiy.Unit = this.Unit;
			propertiy.xYShaft = this.xYShaft;
			propertiy.Style = this.Style;
			propertiy.BandName = this.BandName;
			propertiy.IsFilter = this.IsFilter;
			propertiy.IsSelected = this.IsSelected;
			propertiy.Name = this.Name;
			propertiy.Caption = this.Caption;
			propertiy.IsDefaultChecked = this.IsDefaultChecked;
			propertiy.Index = this.Index;
			propertiy._lineColor = this._lineColor;
			propertiy.BlnRowKey = this.BlnRowKey;
			propertiy.BlnColumnRank = this.BlnColumnRank;
			propertiy.BlnRowGroup = this.BlnRowGroup;
			propertiy.BlnDateFormat = this.BlnDateFormat;
            propertiy.FormatString = this.FormatString;
			return propertiy;
		}

        /// <summary>
        /// 属性名对象集合克隆
        /// </summary>
        /// <param name="filters">要克隆的对象集合</param>
        /// <returns>克隆返回的对象集合</returns>
		public static  List<PropertiyName> Clone(List<PropertiyName> filters)
		{
			List<PropertiyName> result = new List<PropertiyName>();
			foreach (PropertiyName filter in filters)
			{
				if (filter != null)
					result.Add(Clone(filter));
			}
			return result;
		}

        /// <summary>
        /// 属性名对象克隆
        /// </summary>
        /// <param name="filter">要克隆的对象</param>
        /// <returns>克隆完成的对象</returns>
		public static PropertiyName Clone(PropertiyName filter)
		{

			return (PropertiyName)filter.Clone();
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
                if (Doc.DocumentElement.GetElementsByTagName("Caption")[0] != null)
                    this.Caption = Doc.DocumentElement.GetElementsByTagName("Caption")[0].InnerText;
                if (Doc.DocumentElement.GetElementsByTagName("Name")[0] != null)
                    this.Name = Doc.DocumentElement.GetElementsByTagName("Name")[0].InnerText;
                if (Doc.DocumentElement.GetElementsByTagName("BlnRowGroup")[0] != null)
                    this.BlnRowGroup = Doc.DocumentElement.GetElementsByTagName("BlnRowGroup")[0].InnerText == "true";
                if (Doc.DocumentElement.GetElementsByTagName("BlnRowKey")[0] != null)
                    this.BlnRowKey = Doc.DocumentElement.GetElementsByTagName("BlnRowKey")[0].InnerText == "true";
                if (Doc.DocumentElement.GetElementsByTagName("BlnColumnRank")[0] != null)
                    this.BlnColumnRank = Doc.DocumentElement.GetElementsByTagName("BlnColumnRank")[0].InnerText == "true";
                if (Doc.DocumentElement.GetElementsByTagName("IsDefaultChecked")[0] != null)
                    this.IsDefaultChecked = Doc.DocumentElement.GetElementsByTagName("IsDefaultChecked")[0].InnerText == "true";
                if (Doc.DocumentElement.GetElementsByTagName("Index")[0] != null)
                    this.Index =int.Parse(Doc.DocumentElement.GetElementsByTagName("Index")[0].InnerText);
                if (Doc.DocumentElement.GetElementsByTagName("LineColor")[0] != null)
                {}
                if (Doc.DocumentElement.GetElementsByTagName("FormatString")[0] != null)
                    this.FormatString = Doc.DocumentElement.GetElementsByTagName("FormatString")[0].InnerText;
                //if (xml.Attributes["FormatString"] != null)
                //    this.FormatString = xml.Attributes["FormatString"].Value;
                   // this.LineColor =(Color)Enum.Parse(typeof(Color), Doc.DocumentElement.GetElementsByTagName("LineColor")[0].InnerText);
            }
            if(xml.Attributes !=null)
            {
                if (xml.Attributes["NameOfCaption"] != null)
                    this.NameOfCaption = xml.Attributes["NameOfCaption"].Value;
                if (xml.Attributes["Unit"] != null)
                    this.Unit = xml.Attributes["Unit"].Value;
                if (xml.Attributes["xYShaft"] != null)
                    this.xYShaft =int.Parse(xml.Attributes["xYShaft"].Value);
                if (xml.Attributes["Style"] != null)
                    this.Style = xml.Attributes["Style"].Value;
                if (xml.Attributes["BandName"] != null)
                    this.BandName = xml.Attributes["BandName"].Value;
                if (xml.Attributes["IsFilter"] != null)
                    this.IsFilter = xml.Attributes["IsFilter"].Value == "true";
                if (xml.Attributes["BlnDateFormat"] != null)
                    this.BlnDateFormat = xml.Attributes["BlnDateFormat"].Value=="true";
                //if (xml.Attributes["FormatString"] != null)
                //    this.FormatString = xml.Attributes["FormatString"].Value;
                if (xml.Attributes["IsSelected"] != null)
                    this.IsSelected = xml.Attributes["IsSelected"].Value == "true";
            }
        }
	}
}
