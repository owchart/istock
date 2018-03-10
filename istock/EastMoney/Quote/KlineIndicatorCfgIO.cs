using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using EmQComm;

namespace EmQDataIO
{
    /// <summary>
    /// KlineIndicatorCfgIO
    /// </summary>
    public class KlineIndicatorCfgIO
    {
        
        private static readonly string xmlFilePath = PathUtilities.CfgPath + "KLineIndicatorConfig.xml";
        /// <summary>
        /// GetAllKLineIndicatorCfgData
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<KlineIndicator>> GetAllKLineIndicatorCfgData()
        {
            Dictionary<string, List<KlineIndicator>> result =
                new Dictionary<string, List<KlineIndicator>>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNode root = xmlDoc.SelectSingleNode("Index");
            List<KlineIndicator> list;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "#comment")
                    continue;
                string id = node.Attributes["id"].Value;
                string name = node.Attributes["name"].Value;

                list = new List<KlineIndicator>();

                //XmlNode child = node.FirstChild;

                foreach (XmlNode subNode in node.ChildNodes)
                {
                    KlineIndicator indicatorData = new KlineIndicator();
                    indicatorData.text= subNode.Attributes["name"].Value;
                    indicatorData.name = subNode.Attributes["id"].Value;
                    list.Add(indicatorData);
                }

                result.Add(node.Attributes["name"].Value, list);
            }
            return result;
        }
    }
    /// <summary>
    /// KlineIndicator
    /// </summary>
    public class KlineIndicator
    {
        private string _name;
        private string _text;

        /// <summary>
        /// name
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// text
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
