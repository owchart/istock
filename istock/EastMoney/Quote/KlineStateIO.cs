using EmQComm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EmQDataIO
{
    public class KlineStateIO
    {
        static readonly string ConfigFile = PathUtilities.CfgPath + "KlineStateConfig.xml";
        static readonly string CustomerConfigFile = PathUtilities.UserPath + @"Kline\" + "KlineStateConfig.xml";

        /// <summary>
        /// GetKlineStateCfgData
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, KlineState> GetData()
        {
            Dictionary<string, KlineState> result =new Dictionary<string, KlineState>();
            XmlDocument xmlDoc = new XmlDocument();
            bool flagReadData = false;
            if (File.Exists(CustomerConfigFile))
            {
                try
                {
                    xmlDoc.Load(CustomerConfigFile);
                    flagReadData = true;
                }
                catch (Exception e1) 
                {
                    flagReadData = false;
                }
            }
            if (File.Exists(ConfigFile) && !flagReadData)
            {
                try
                {
                    xmlDoc.Load(ConfigFile);
                    flagReadData = true;
                }
                catch (Exception e2) 
                {
                    flagReadData = false;
                }
            }
            if (!flagReadData)
                return null;
            XmlNode root = xmlDoc.SelectSingleNode("KlineState");
            if (root == null)
                return null;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "#comment" || node.Attributes==null)
                    continue;
                KlineState tempObj = new KlineState();
                tempObj.Key = node.Attributes["Key"].Value;
                tempObj.ObjValue = node.Attributes["ObjectValue"].Value;
                result.Add(tempObj.Key, tempObj);
            }
            return result;
        }

        public static void WriteDataXML(Dictionary<string, KlineState> data)
        {
            XmlDocument newDoc = new XmlDocument();
            XmlElement root = newDoc.CreateElement("KlineState");
            newDoc.AppendChild(root);
            foreach (KeyValuePair<string, KlineState> temp in data)
            {
                XmlElement rootChild = newDoc.CreateElement("State");
                rootChild.SetAttribute("Key", temp.Key);
                rootChild.SetAttribute("ObjectValue", temp.Value.ObjValue);
                root.AppendChild(rootChild);
            }
            if (!Directory.Exists(PathUtilities.UserPath + @"Kline\"))
                Directory.CreateDirectory(PathUtilities.UserPath + @"Kline\");
            newDoc.Save(CustomerConfigFile);
        }
    }

    public class KlineState
    {
        private string _key;
        private string _objValue;

        public string Key
        {
            get { return _key; }
            set { this._key = value; }
        }

        public string ObjValue
        {
            get { return _objValue; }
            set { this._objValue = value; }
        }
    }
}
