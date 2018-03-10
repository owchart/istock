using System;
using System.Xml.Serialization;

namespace OwLib
{
    /// <summary>
    /// 时间对象实体
    /// </summary>
	[Serializable]
	public class CustomDate
	{
        /// <summary>
        /// 时间单位类型，如年，月，日
        /// </summary>
		[XmlAttribute]
		public string CalType { get; set; }

        /// <summary>
        /// 对应的时间单位的值，如年：2013，月：12
        /// </summary>
		[XmlAttribute]
		public int Value { get; set; }

        /// <summary>
        /// 时间值
        /// </summary>
		[XmlAttribute]
		public DateTime DateTime { get; set; }


        /// <summary>
        /// 获取或设置时间数据类型：1：表示天；2：表示季度；3：表示年
        /// </summary>
		[XmlAttribute]
		public int BindDateType { get; set; }

        /// <summary>
        /// 偏移量的值，用于通过动态的当前时间算出过滤值的值，如偏移量为1，
        /// 则表示值为2013-1=2012（假设数字过滤框的值为年份，且当前时间为作者写此段文字时的时间（2013-1-5 15:12 added by JP））
        /// </summary>
        [XmlAttribute]
        public int PianyiNum { get; set; }

        /// <summary>
        /// 偏移量的单位类型（年，季，月，周，日）
        /// </summary>
        [XmlAttribute]
        public string MaxValueType { get; set; }

        /// <summary>
        /// 过滤值的类型（时间(0)，整型(1)or字符串类型(2)），通过该值来确定过滤值
        /// </summary>
        [XmlAttribute]
        public int ValueType { get; set; }

        internal void Deserialize(System.Xml.XmlNode n)
        {
            if (n.Attributes["BindDateType"] != null)
                this.BindDateType = int.Parse(n.Attributes["BindDateType"].Value);
            if (n.Attributes["CalType"] != null)
                this.CalType = n.Attributes["CalType"].Value;
            if (n.Attributes["DateTime"] != null)
                this.DateTime = Convert.ToDateTime(n.Attributes["DateTime"].Value);
            if (n.Attributes["MaxValueType"] != null)
                this.MaxValueType = n.Attributes["MaxValueType"].Value;
            if (n.Attributes["PianyiNum"] != null)
                this.PianyiNum = int.Parse(n.Attributes["PianyiNum"].Value);
            if (n.Attributes["Value"] != null)
                this.Value = int.Parse(n.Attributes["Value"].Value);
            if (n.Attributes["ValueType"] != null)
                this.ValueType = int.Parse(n.Attributes["ValueType"].Value);
        }
	}
}