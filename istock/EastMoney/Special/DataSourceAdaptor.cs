#region Version info
/***********************************************************************\
 * 【本类功能概述】 
 * 
 * 作者：yll 时间：2012/2/13 14:36:06 
 * 文件名：DataSourceAdaptor 
 * 版本：V1.0.1 
 * 
 * 修改者： 时间： 
 * 修改说明： 
 * 
 * 
\***********************************************************************/


#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace OwLib
{
	/// <summary>
	/// 保存用户对数据源的配置。
	/// </summary>
	[Serializable]
	public class DataSourceAdaptor:ICloneable
	{
		#region Property
        /// <summary>
        /// 过滤条件名称集合
        /// </summary>
		[XmlAttribute] public List<String> FilterLists = new List<string>();

        /// <summary>
        /// 是否显示数值
        /// </summary>
		[XmlAttribute]
		public bool ShowLable { get; set; }

        /// <summary>
        /// 获取或设置是否显示图例
        /// </summary>
		[XmlAttribute]
		public bool ShowLegend { get; set; }

		/// <summary>
		/// Data source name
		/// </summary>
		[XmlElement]
		public string DataSourceName { get; set; }

		/// <summary>
		/// 获取或设置统计英文名
		/// </summary>
		[XmlElement]
		public string StatisticsEngName { get; set; }

		/// <summary>
		/// Propertis to show in chart.
		/// </summary>
		[XmlElement]
		public List<PropertiyName> PropertiesToShow { get; set; }

        /// <summary>
        /// 获取或设置取数据方式
        /// </summary>
		[XmlAttribute]
		public SOURCEPROVIDER SOURCEPROVIDER { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
		[XmlAttribute] public string Style;

        /// <summary>
        /// 是否XY转换
        /// </summary>
		[XmlAttribute] public bool YX;

		/// <summary>
		/// X轴排序 0 默认不排序;1 升序; 2 降序
		/// </summary>	
		[XmlAttribute] public int SortX;

		/// <summary>
		/// 是否存在多表头形式绘图,一对多
		/// </summary>
		[XmlAttribute] public bool IsBand;

		#endregion

		/// <summary>
		/// Limit the row count of data source.
		/// </summary>
		[XmlElement]
		public Limits Limits { get; set; }

		#region Constructor

        /// <summary>
        /// 构建图形数据源对象
        /// </summary>
		public DataSourceAdaptor()
		{
			this.PropertiesToShow = new List<PropertiyName>(10);
			this.DataSourceName = string.Empty;
			this.Limits = new Limits();
		}

		#endregion

		#region Private functions

		#endregion

		#region Protected functions

		#endregion

		#region Public functions
		//public void CompleteDataAdapter( )
		//{

		//  var res = (from item in PropertiesToShow
		//             where item.IsFilter
		//             select item.Name).ToList();
		//  FilterLists = res;
		//}
		#endregion

        /// <summary>
        /// 对象克隆方法
        /// </summary>
        /// <returns>克隆的对象结果</returns>
		public object Clone()
		{
		    DataSourceAdaptor adaptor =new DataSourceAdaptor() ;
			adaptor.FilterLists = this.FilterLists;
			adaptor.ShowLable=this .ShowLable;
			adaptor .ShowLegend =this .ShowLegend;
			adaptor.DataSourceName=this .DataSourceName;
			adaptor.StatisticsEngName=this .StatisticsEngName ;
			adaptor.PropertiesToShow=PropertiyName .Clone( this .PropertiesToShow );
	adaptor .SOURCEPROVIDER=this .SOURCEPROVIDER;
			adaptor.Style =this .Style ;
			adaptor .YX =this .YX ;
			adaptor .SortX =this .SortX ;
            adaptor.IsBand = this.IsBand;
	adaptor .Limits =new Limits {ColumnLimit =this .Limits .ColumnLimit ,Enable =this .Limits .Enable ,RowLimit =this.Limits .RowLimit };
			return adaptor;
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
                if (Doc.DocumentElement.GetElementsByTagName("DataSourceName")[0] != null)
                    this.DataSourceName = Doc.DocumentElement.GetElementsByTagName("DataSourceName")[0].InnerText;
                if (Doc.DocumentElement.GetElementsByTagName("StatisticsEngName")[0] != null)
                    this.StatisticsEngName = Doc.DocumentElement.GetElementsByTagName("StatisticsEngName")[0].InnerText;
                XmlNodeList PropertiesToShownodes = Doc.DocumentElement.GetElementsByTagName("PropertiesToShow");
                List<PropertiyName> showpros = new List<PropertiyName>();
                foreach (XmlNode node in PropertiesToShownodes)
                {
                    if (node != null)
                    {
                        PropertiyName speicalDate = new PropertiyName();
                        speicalDate.Deserialize(node);
                        showpros.Add(speicalDate);
                    }
                }
                this.PropertiesToShow = showpros;
                XmlNode XAxisnode = Doc.DocumentElement.GetElementsByTagName("XAxis")[0];
                if (XAxisnode!=null)
                {
                }
                XmlNode Limitsnode = Doc.DocumentElement.GetElementsByTagName("Limits")[0];
                if (Limitsnode != null)
                {
                    Limits _limits = new Limits();
                    _limits.Deserialize(Limitsnode);
                    this.Limits = _limits;
                }
            }
            if(xml.Attributes !=null)
            {
                if (xml.Attributes["FilterLists"] != null)
                    this.FilterLists.AddRange(xml.Attributes["FilterLists"].Value.Split(new string[]{" "},StringSplitOptions.RemoveEmptyEntries));
                if (xml.Attributes["ShowLable"] != null)
                    this.ShowLable = xml.Attributes["ShowLable"].Value == "true";
                if (xml.Attributes["ShowLegend"] != null)
                    this.ShowLegend = xml.Attributes["ShowLegend"].Value == "true";
                if (xml.Attributes["YX"] != null)
                    this.YX = xml.Attributes["YX"].Value == "true";
                if (xml.Attributes["IsBand"] != null)
                    this.IsBand = xml.Attributes["IsBand"].Value == "true";
                if (xml.Attributes["SortX"] != null)
                    this.SortX =int.Parse(  xml.Attributes["SortX"].Value);
                if (xml.Attributes["SOURCEPROVIDER"] != null)
                    this.SOURCEPROVIDER = (SOURCEPROVIDER)Enum.Parse(typeof(SOURCEPROVIDER), xml.Attributes["SOURCEPROVIDER"].Value);
                if (xml.Attributes["Style"] != null)
                    this.Style = xml.Attributes["Style"].Value;
                //if (xml.Attributes["FilterLists"] != null)
                //    this.FilterLists = xml.Attributes["FilterLists"].Value;
            }
        }
	}
}
