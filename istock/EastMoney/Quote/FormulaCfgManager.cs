using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace EmQComm.Formula {
    /// <summary>
    /// 公式配置文件管理
    /// </summary>
    public class FormulaCfgManager {
        /// <summary>
        /// 获取公式字典
        /// </summary>
        public static FormulaDict GetFormulaDict() {
            FormulaDict dict = new FormulaDict();
            string filePath = PathUtilities.CfgPath + "formuladict.xml";
            if (File.Exists(filePath)) {
                try {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.SelectSingleNode(@"formuladict/formulatype");
                    foreach (XmlNode node in root.ChildNodes) {
                        if (node.NodeType!=XmlNodeType.Element)
                            continue;
                        FormulaDict.FormulaType formulatype = new FormulaDict.FormulaType();
                        formulatype.Id = Convert.ToInt32(node.Attributes["type"].Value);
                        formulatype.Name = node.Attributes["name"].Value;
                        foreach (XmlNode subnode in node.ChildNodes) {
                            if (subnode.NodeType!=XmlNodeType.Element)
                                continue;
                            FormulaDict.FormulaSubType subType = new FormulaDict.FormulaSubType();
                            subType.Id = Convert.ToInt32(subnode.Attributes["id"].Value);
                            subType.Name = subnode.Attributes["name"].Value;
                            formulatype.SubTypes.Add(subType);
                        }
                        dict.FormulaTypes.Add(formulatype);
                    }
                    root = doc.SelectSingleNode(@"formuladict/drawtype");
                    foreach (XmlNode node in root.ChildNodes) {
                        if (node.NodeType!=XmlNodeType.Element)
                            continue;
                        FormulaDict.DrawType drawType = new FormulaDict.DrawType();
                        drawType.Id = Convert.ToInt32(node.Attributes["id"].Value);
                        drawType.Name = node.Attributes["name"].Value;
                        dict.DrawTypes.Add(drawType);
                    }
                } catch (Exception e) {
                    LogUtilities.LogMessage("FormulaDict.xml Wrong:" + e.Message);
                }
            }

            return dict;
        }

        /// <summary>
        /// 获取公式的系统功能
        /// </summary>
        public static FormulaFunctions GetFormulaSystemFunctions() {
            FormulaFunctions functions = new FormulaFunctions();
            string filePath = PathUtilities.CfgPath + "formulafunctions.xml";
            if (File.Exists(filePath)) {
                try {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.SelectSingleNode(@"functions");
                    foreach (XmlNode categoryNode in root.ChildNodes) {
                        FormulaFunctions.Category category = new FormulaFunctions.Category();
                        category.Name = categoryNode.Attributes["name"].Value;
                        foreach (XmlNode functionNode in categoryNode) {
                            FormulaFunctions.Function function = new FormulaFunctions.Function();
                            function.Name = functionNode.Attributes["name"].Value;
                            function.Description = functionNode.Attributes["des"].Value;
                            foreach (XmlNode usagenode in functionNode.ChildNodes) {
                                if (usagenode.NodeType == XmlNodeType.CDATA) {
                                    function.Usage = usagenode.Value;
                                }
                            }
                            category.Functions.Add(function);
                        }
                        functions.FunctionCategories.Add(category);
                    }
                } catch (Exception e) {
                    LogUtilities.LogMessage("FormulaFunction.xml error:" + e.Message);
                }
            }
            return functions;
        }
    }
}
