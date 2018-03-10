using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace OwLib
{
    /* Sample of the confige file.
==============================================================================     
<?xml version="1.0" encoding="UTF-8"?>
<indicators>
    <stock code="1000002165">
        <left>
            <indicator id=""/>
            <indicator id=""/>
        </left>
        <right>
        </right>
    </stock>
</indicators>
==============================================================================        
     */


    /// <summary>
    /// 股票对应的宏观指标对应的配置文件
    /// </summary>
    public class StockCustIndicatorFileIO
    {
        private static readonly string FilePath = PathUtilities.UserPath + @"Kline\" + "StockCustIndicatorCfg.xml";
        /// <summary>
        /// "/indicators"
        /// </summary>
        private const string RootNodeName = "/indicators";
        /// <summary>
        /// "/indicators/stock"
        /// </summary>
        private const string StockNode = "/indicators/stock";
        /// <summary>
        /// "/indicators/stock/left"
        /// </summary>
        private const string LeftIndicatorNodeName = "/indicators/stock/left";
        /// <summary>
        /// "/indicators/stock/right"
        /// </summary>
        private const string RightIndicatorNodeName = "/indicators/stock/right";
        /// <summary>
        /// "code"
        /// </summary>
        private const string StockAttr = "code";
        /// <summary>
        /// "id"
        /// </summary>
        private const string idAttr = "id";

        /// <summary>
        /// left
        /// </summary>
        private const string LeftPath = "left";
        /// <summary>
        /// right
        /// </summary>
        private const string RightPath = "right";
        /// <summary>
        /// LeafNode: indicator
        /// </summary>
        private const string FinaNode = "indicator";

        #region The configer file read interface.
        /// <summary>
        /// Try to Get the customlize macro-indicator Id list from user configer files.
        /// </summary>
        /// <param name="code"> Stock id.</param>
        /// <param name="indicateRequestType"> Indident which side type of request</param>
        /// <param name="custMacroIds"> Output the customlize macro-indicator Id list</param>
        /// <returns> True for the code have the customlize macro-indicator Id list, otherwise false.</returns>
        public static bool TryGetCustIndicatorIds(int code, IndicateRequestType indicateRequestType,
            out HashSet<string> custMacroIds)
        {
            custMacroIds = new HashSet<string>();
            Dictionary<IndicateRequestType, HashSet<string>> innerDic;

            return (DicStockCustIndicator.TryGetValue(code, out innerDic)
                && innerDic.TryGetValue(indicateRequestType, out custMacroIds));
        }

        /// <summary>
        /// Try to add a customlize macro-indicator Id in the user configer files.
        /// </summary>
        /// <param name="code"> Stock id.</param>
        /// <param name="indicateRequestType"> Indident which side type of request</param>
        /// <param name="custIndicatorId"> The macro-indicator id for adding.</param>
        /// <returns> True for the code have been added in the configer file successfully, otherwise false.</returns>
        public static bool TryAddCustIndicatorId(int code, IndicateRequestType indicateRequestType,
            string custIndicatorId)
        {
            bool success = false;
            // del actions.
            if (!File.Exists(FilePath))
                return false;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(FilePath);
            }
            catch (IOException ioe)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ioe.Message);
                throw;
            }
            catch (XmlException xe)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + xe.Message);
                throw;
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ex.Message);
                throw;
            }

            try
            {
                string path = string.Format("{0}[@{1}='{2}']", StockNode, StockAttr, code);
                XmlNode stockNode = doc.SelectSingleNode(path);
                if (stockNode == null || !stockNode.HasChildNodes)
                {
                    return false;
                }
                string checkNodePath;
                XmlNode checkNode;
                switch (indicateRequestType)
                {
                    case IndicateRequestType.LeftIndicatorsReport:
                        XmlNode leftNode = stockNode.SelectSingleNode(LeftPath);
                        if (leftNode == null)
                        {
                            leftNode = doc.CreateElement(LeftPath);
                            stockNode.AppendChild(leftNode);
                        }
                        // Check if the node exist or not
                        checkNodePath =
                            string.Format("{0}[@{1}='{2}']", FinaNode, idAttr, custIndicatorId);
                        checkNode = leftNode.SelectSingleNode(checkNodePath);

                        if (checkNode == null)
                        {
                            XmlNode newIndicatorNode = doc.CreateElement(FinaNode);
                            XmlAttribute newIdAttr = doc.CreateAttribute(idAttr);
                            newIdAttr.InnerXml = custIndicatorId;
                            newIndicatorNode.Attributes.Append(newIdAttr);

                            leftNode.AppendChild(newIndicatorNode);

                            success = true;
                        }
                        break;

                    case IndicateRequestType.RightIndicatorsReport:
                        XmlNode rightNode = stockNode.SelectSingleNode(RightPath);
                        if (rightNode == null)
                        {
                            rightNode = doc.CreateElement(RightPath);
                            stockNode.AppendChild(rightNode);
                        }
                        // Check if the node exist or not
                        checkNodePath =
                            string.Format("{0}[@{1}='{2}']", FinaNode, idAttr, custIndicatorId);
                        checkNode = rightNode.SelectSingleNode(checkNodePath);

                        // When the added node exist in the file, do nothing.
                        if (checkNode == null)
                        {
                            XmlNode newIndicatorNode = doc.CreateElement(FinaNode);

                            XmlAttribute newIdAttr = doc.CreateAttribute(idAttr);
                            newIdAttr.InnerXml = custIndicatorId;
                            newIndicatorNode.Attributes.Append(newIdAttr);

                            rightNode.AppendChild(newIndicatorNode);

                            success = true;
                        }
                        break;

                    case IndicateRequestType.IndicatorValuesReport:
                        break;

                }

                if (success)
                {
                    doc.Save(FilePath);

                    // Refesh the cache.
                    LoadConfig();
                }
            }
            catch { success = false; }
            return success;
        }

        /// <summary>
        /// Try to delete a customlize macro-indicator Id in the user configer files.
        /// </summary>
        /// <param name="code"> Stock id.</param>
        /// <param name="indicateRequestType"> Indident which side type of request</param>
        /// <param name="custIndicatorId"> The macro-indicator id for delete.</param>
        /// <returns> True for the code have been deleted in the configer file successfully,
        /// otherwise false.
        /// </returns>
        public static bool TryDelCustIndicatorId(int code, IndicateRequestType indicateRequestType,
            string custIndicatorId)
        {
            bool success = false;
            // del actions.
            if (!File.Exists(FilePath))
                return false;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(FilePath);
            }
            catch (IOException ioe)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ioe.Message);
                throw;
            }
            catch (XmlException xe)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + xe.Message);
                throw;
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ex.Message);
                throw;
            }

            try
            {
                string path = string.Format("{0}[@{1}='{2}']", StockNode, StockAttr, code);
                XmlNode stockNode = doc.SelectSingleNode(path);
                if (stockNode == null || !stockNode.HasChildNodes)
                {
                    return false;
                }
                string checkNodePath;
                XmlNode checkNode;

                switch (indicateRequestType)
                {
                    case IndicateRequestType.LeftIndicatorsReport:
                        XmlNode leftNode = stockNode.SelectSingleNode(LeftPath);
                        if (leftNode == null)
                            return false;

                        // Check if the node exist or not
                        checkNodePath =
                            string.Format("{0}[@{1}='{2}']", FinaNode, idAttr, custIndicatorId);
                        checkNode = leftNode.SelectSingleNode(checkNodePath);
                        // When the added node exist in the file, do deleting.
                        if (checkNode != null)
                        {
                            leftNode.RemoveChild(checkNode);
                            success = true;
                        }
                        break;

                    case IndicateRequestType.RightIndicatorsReport:
                        XmlNode rightNode = stockNode.SelectSingleNode(RightPath);
                        if (rightNode == null)
                            return false;

                        // Check if the node exist or not
                        checkNodePath =
                            string.Format("{0}[@{1}='{2}']", FinaNode, idAttr, custIndicatorId);
                        checkNode = rightNode.SelectSingleNode(checkNodePath);
                        // When the added node exist in the file, do deleting.
                        if (checkNode != null)
                        {
                            rightNode.RemoveChild(checkNode);
                            success = true;
                        }
                        break;

                    case IndicateRequestType.IndicatorValuesReport:
                        break;
                }


                if (success)
                {
                    doc.Save(FilePath);

                    // Refesh the cache.
                    LoadConfig();
                }
            }
            catch { success = false; }

            return success;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<int, Dictionary<IndicateRequestType, HashSet<string>>> DicStockCustIndicator;

        static StockCustIndicatorFileIO()
        {
            LoadConfig();
        }

        private static void LoadConfig()
        {
            if (!File.Exists(FilePath))
                return;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(FilePath);
            }
            catch (IOException ioe)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ioe.Message);
                throw;
            }
            catch (XmlException xe)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + xe.Message);
                throw;
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ex.Message);
                throw;
            }

            try
            {
                DicStockCustIndicator = GetStockCustIndicator(doc);
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("Load StockCustIndicatorCfg.xml Error : " + ex.Message);
                throw;
            }
        }

        private static Dictionary<int, Dictionary<IndicateRequestType, HashSet<string>>> GetStockCustIndicator(XmlDocument doc)
        {
            Dictionary<int, Dictionary<IndicateRequestType, HashSet<string>>> dic
                = new Dictionary<int, Dictionary<IndicateRequestType, HashSet<string>>>();
            // Set default dictionary:
            XmlNodeList stockNodes = doc.SelectNodes(StockNode);

            if (TryGetAllColFieldInfo(stockNodes, out dic))
                return dic;


            return dic;
        }

        /// <summary>
        /// 读取Column 叶子节点返回字典
        /// </summary>
        /// <param name="pareentNode"></param>
        ///  <param name="dic">返回字典</param>
        /// <returns></returns>
        private static bool TryGetAllColFieldInfo(XmlNodeList stockNodes,
            out Dictionary<int, Dictionary<IndicateRequestType, HashSet<string>>> dic)
        {
            dic = new Dictionary<int, Dictionary<IndicateRequestType, HashSet<string>>>();

            if (stockNodes == null)
                return false;
            if (stockNodes.Count == 0)
                return false;

            //int stockCount = stockNodes.Count;


            FieldInfo info;
            Dictionary<IndicateRequestType, HashSet<string>> innerDic;
            HashSet<string> set;
            foreach (XmlNode stockNode in stockNodes)
            {
                if (stockNode.NodeType == XmlNodeType.Element)
                {
                    string codeStr = stockNode.Attributes[StockAttr].Value;

                    int code;
                    if (int.TryParse(codeStr, out code) && stockNode.HasChildNodes)
                    {
                        XmlNode LeftIndicatorNode = stockNode.SelectSingleNode(LeftPath);
                        if (LeftIndicatorNode != null && LeftIndicatorNode.HasChildNodes)
                        {
                            int capacity = LeftIndicatorNode.ChildNodes.Count;

                            if (!dic.TryGetValue(code, out innerDic))
                            {
                                innerDic = new Dictionary<IndicateRequestType, HashSet<string>>(1);
                                dic[code] = innerDic;
                            }

                            if (!innerDic.TryGetValue(IndicateRequestType.LeftIndicatorsReport, out set))
                            {
                                set = new HashSet<string>();
                                innerDic[IndicateRequestType.LeftIndicatorsReport] = set;
                            }
                            foreach (XmlNode indicatorNode in LeftIndicatorNode.ChildNodes)
                            {
                                string indicatorId = indicatorNode.Attributes[idAttr].Value;
                                set.Add(indicatorId);
                            }
                        }

                        XmlNode RightIndicatorNode = stockNode.SelectSingleNode(RightPath);
                        if (RightIndicatorNode != null && RightIndicatorNode.HasChildNodes)
                        {
                            int capacity = RightIndicatorNode.ChildNodes.Count;
                            if (!dic.TryGetValue(code, out innerDic))
                            {
                                innerDic = new Dictionary<IndicateRequestType, HashSet<string>>(1);
                                dic[code] = innerDic;
                            }

                            if (!innerDic.TryGetValue(IndicateRequestType.RightIndicatorsReport, out set))
                            {
                                set = new HashSet<string>();
                                innerDic[IndicateRequestType.RightIndicatorsReport] = set;
                            }
                            foreach (XmlNode indicatorNode in RightIndicatorNode.ChildNodes)
                            {
                                string indicatorId = indicatorNode.Attributes[idAttr].Value;
                                set.Add(indicatorId);
                            }
                        }

                    }
                }
            }

            return true;
        }
    }
}
