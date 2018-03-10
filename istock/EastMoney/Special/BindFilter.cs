using System;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;

namespace OwLib
{
    /// <summary>
    /// 绑定过去条件
    /// </summary>
	[Serializable]
     [StructLayout(LayoutKind.Auto,CharSet = CharSet.Unicode)]
	public class BindFilter
	{
        /// <summary>
        /// 获取或设置源名
        /// </summary>
		[XmlElement]
		public string SourceName { get; set; }

        /// <summary>
        /// 获取或设置列名称
        /// </summary>
		[XmlElement]
		public string TextColumn { get; set; }

        /// <summary>
        /// 获取或设置列值
        /// </summary>
		[XmlElement]
		public string ValueColumn { get; set; }

        /// <summary>
        /// 获取或设置变化的过滤值名
        /// </summary>
		[XmlElement]
		public string ChangedFilterName { get; set; }

        /// <summary>
        /// 获取或设置默认的过滤值名
        /// </summary>
		[XmlElement]
		public string DefaultFilterName { get; set; }

        /// <summary>
        /// 获取或设置是否发送过滤条件
        /// </summary>
		[XmlElement]
		public bool SendFilter { get; set; }

        /// <summary>
        /// 获取或设置是否接收过滤条件
        /// </summary>
		[XmlElement]
		public bool RecFilter { get; set; }

        /// <summary>
        /// 获取或设置默认值
        /// </summary>
		[XmlElement]
		public object DefaultValue { get; set; }

        /// <summary>
        /// 获取或设置排序名
        /// </summary>
        [XmlElement]
        public string OrderName { get; set; }

        /// <summary>
        /// 读取xml配置信息给对象赋值
        /// </summary>
        /// <param name="n">xml配置文本</param>
        public void Deserialize(XmlNode n)
        {
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(n.OuterXml);
            if (Doc.DocumentElement.GetElementsByTagName("ChangedFilterName")[0] != null)
                this.ChangedFilterName = Doc.DocumentElement.GetElementsByTagName("ChangedFilterName")[0].InnerText ;
            if (Doc.DocumentElement.GetElementsByTagName("DefaultFilterName")[0] != null)
                this.DefaultFilterName = Doc.DocumentElement.GetElementsByTagName("DefaultFilterName")[0].InnerText;
            if (Doc.DocumentElement.GetElementsByTagName("DefaultValue")[0] != null)
                this.DefaultValue = Doc.DocumentElement.GetElementsByTagName("DefaultValue")[0].InnerText;
            if (Doc.DocumentElement.GetElementsByTagName("OrderName")[0] != null)
                this.OrderName = Doc.DocumentElement.GetElementsByTagName("OrderName")[0].InnerText;
            if (Doc.DocumentElement.GetElementsByTagName("RecFilter")[0] != null)
                this.RecFilter = Doc.DocumentElement.GetElementsByTagName("RecFilter")[0].InnerText == "true";
            if (Doc.DocumentElement.GetElementsByTagName("SendFilter")[0] != null)
                this.SendFilter = Doc.DocumentElement.GetElementsByTagName("SendFilter")[0].InnerText == "true";
            if (Doc.DocumentElement.GetElementsByTagName("SourceName")[0] != null)
                this.SourceName = Doc.DocumentElement.GetElementsByTagName("SourceName")[0].InnerText;
            if (Doc.DocumentElement.GetElementsByTagName("TextColumn")[0] != null)
                this.TextColumn = Doc.DocumentElement.GetElementsByTagName("TextColumn")[0].InnerText;
            if (Doc.DocumentElement.GetElementsByTagName("ValueColumn")[0] != null)
                this.ValueColumn = Doc.DocumentElement.GetElementsByTagName("ValueColumn")[0].InnerText;
        }
	}
}
