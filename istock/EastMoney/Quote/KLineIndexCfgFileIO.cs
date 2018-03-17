using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OwLib
{
    /// <summary>
    /// KLineIndexCfgFileIO
    /// </summary>
    public static class KLineIndexCfgFileIO
    {
        private static readonly String _filePath = PathUtilities.CfgPath + "KLineIndexConfig.xml";
        /// <summary>
        /// GetAllKLineCfgData
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, List<KLineIndexData>> GetAllKLineCfgData()
        {
            Dictionary<String, List<KLineIndexData>> result =
                new Dictionary<String, List<KLineIndexData>>();
            XmlDocument doc = new XmlDocument(); 
            doc.Load(_filePath);

            XmlNode root = doc.SelectSingleNode("IndexParams");

            List<KLineIndexData> list;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "#comment")
                    continue;
                String id = node.Attributes["id"].Value;
                String name = node.Attributes["caption"].Value;

                list = new List<KLineIndexData>();

                XmlNode child = node.FirstChild;

                foreach (XmlNode subNode in child.ChildNodes)
                {
                    KLineIndexData indexData = new KLineIndexData();
                    indexData.Name = subNode.Attributes["name"].Value;
                    indexData.Default = Convert.ToInt32(subNode.Attributes["default"].Value);
                    indexData.Step = (subNode.Attributes["step"].Value == "") ? 1 : Convert.ToInt32(subNode.Attributes["step"].Value);
                    indexData.Min = (subNode.Attributes["min"].Value == "") ? 1 : Convert.ToInt32(subNode.Attributes["min"].Value);
                    indexData.Max = (subNode.Attributes["max"].Value == "") ? 1 : Convert.ToInt32(subNode.Attributes["max"].Value);
                    list.Add(indexData);
                }

                result.Add(node.Attributes["caption"].Value, list);
            }
            
            return result;
        }
    }
    /// <summary>
    /// KLineIndexData
    /// </summary>
    public struct KLineIndexData
    {
        private String _name;
        private int _default;
        private int _step;
        private int _min;
        private int _max;

        /// <summary>
        /// Name
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Default
        /// </summary>
        public int Default
        {
            get { return _default; }
            set { _default = value; }
        }

        /// <summary>
        /// Step
        /// </summary>
        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }

        /// <summary>
        /// Min
        /// </summary>
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Max
        /// </summary>
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }
    }
}
