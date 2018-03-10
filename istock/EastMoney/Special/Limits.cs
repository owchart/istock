using System;
using System.Xml;
using System.Xml.Serialization;

namespace  EmReportWatch.SpecialAttribute
{
    /// <summary>
    /// 对数据源行数和列数进行限制，防止数据量太大图形无法显示。
    /// </summary>
    [Serializable]
    public class Limits
    {
        /// <summary>
        /// 可以显示的行数，用于限制图形series数
        /// </summary>
        [XmlElement]
        public int RowLimit
        { get; set; }

        /// <summary>
        /// 可以显示的列数，用于限制XAxis数
        /// </summary>
        [XmlElement]
        public int ColumnLimit
        { get; set; }

        /// <summary>
        /// 是否启用限制，false：则获取所有数据行。true: 则获取限制数量的数据行。此属性在对原数据的2次处理时使用。
        /// </summary>
        [XmlElement]
        public bool Enable
        {
            get;
            set;
        }

        /// <summary>
        /// 读取xml配置信息给对象赋值
        /// </summary>
        /// <param name="xml">xml配置文本</param>
        public void Deserialize(XmlNode xml)
        {
            if (xml == null) return;
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(xml.OuterXml);
            if (Doc.DocumentElement != null)
            {
                if (Doc.DocumentElement.GetElementsByTagName("RowLimit")[0] != null)
                    this.RowLimit =int.Parse(Doc.DocumentElement.GetElementsByTagName("RowLimit")[0].InnerText);
                if (Doc.DocumentElement.GetElementsByTagName("ColumnLimit")[0] != null)
                    this.ColumnLimit =int.Parse( Doc.DocumentElement.GetElementsByTagName("ColumnLimit")[0].InnerText);
                if (Doc.DocumentElement.GetElementsByTagName("Enable")[0] != null)
                    this.Enable = Doc.DocumentElement.GetElementsByTagName("Enable")[0].InnerText=="true";
            }

        }
    }
}
