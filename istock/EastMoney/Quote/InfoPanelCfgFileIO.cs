using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;

namespace OwLib {
    /// <summary>
    /// 右边信息栏配置文件读取
    /// </summary>
    public class InfoPanelCfgFileIo {
        /// <summary>
        /// 获取InfoPanel Chart的定义
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, Dictionary<String, InfoPanelChart>> GetInfoPanelCharts() {
            String filePath = PathUtilities.CfgPath + "InfoPanelCharts.xml";
            return GetInfoPanelCharts(filePath);
        }

        private static Dictionary<String, Dictionary<String, InfoPanelChart>> GetInfoPanelCharts(String filePath) {
            if (File.Exists(filePath)) {
                try {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.SelectSingleNode(@"InfoPanel/Charts");
                    Dictionary<String, Dictionary<String, InfoPanelChart>> charts = new Dictionary<String, Dictionary<String, InfoPanelChart>>();
                    foreach (XmlNode levelnode in root.ChildNodes) {
                        Dictionary<String, InfoPanelChart> controls = new Dictionary<String, InfoPanelChart>();
                        if (levelnode.NodeType == XmlNodeType.Comment)
                            continue;
                        foreach (XmlNode node in levelnode.ChildNodes) {
                            if (node.NodeType == XmlNodeType.Comment)
                                continue;
                            InfoPanelChart chart = new InfoPanelChart();
                            chart.Name = node.Attributes["Name"].Value;
                            foreach (XmlNode rownode in node.ChildNodes) {
                                if (rownode.NodeType == XmlNodeType.Comment)
                                    continue;
                                InfoPanelChartRow row = new InfoPanelChartRow();
                                if (null != rownode.Attributes["Repeat"]) {
                                    row.Repeat = Convert.ToBoolean(rownode.Attributes["Repeat"].Value);
                                }
                                if (null != rownode.Attributes["TopLine"]) {
                                    row.TopLine = Convert.ToBoolean(rownode.Attributes["TopLine"].Value);
                                }
                                if (null != rownode.Attributes["BottomLine"]) {
                                    row.BottomLine = Convert.ToBoolean(rownode.Attributes["BottomLine"].Value);
                                }
                                if (null != rownode.Attributes["LineColor"]) {
                                    row.LineColor = Enum.IsDefined(
                                        typeof (LineColor), rownode.Attributes["LineColor"].Value)
                                                        ? (LineColor)
                                                          Enum.Parse(
                                                              typeof (LineColor), rownode.Attributes["LineColor"].Value)
                                                        : LineColor.Lite;
                                }
                                if (null != rownode.Attributes["Margin"]) {
                                    String[] temp = rownode.Attributes["Margin"].Value.Split(',');
                                    if (temp.Length > 0) {
                                        if (temp.Length == 1) {
                                            row.MarginTop = row.MarginBottom = Convert.ToInt32(temp[0]);
                                        } else {
                                            row.MarginTop = Convert.ToInt32(temp[0]);
                                            row.MarginBottom = Convert.ToInt32(temp[1]);
                                        }
                                    }
                                }
                                foreach (XmlNode columnode in rownode.ChildNodes) {
                                    if (columnode.NodeType == XmlNodeType.Comment)
                                        continue;
                                    InfoPanelChartColum colum = new InfoPanelChartColum();
                                    if (null != columnode.Attributes["Caption"]) {
                                        colum.Caption = columnode.Attributes["Caption"].Value;
                                    }
                                    if (null != columnode.Attributes["ValueField"]) {
                                        colum.ValueField = columnode.Attributes["ValueField"].Value;
                                    }
                                    colum.X = Convert.ToSingle(columnode.Attributes["X"].Value);
                                    colum.Width = Convert.ToSingle(columnode.Attributes["Width"].Value);
                                    switch (columnode.Attributes["CaptionAlgin"].Value.ToUpper()) {
                                        case "LEFT":
                                            colum.CaptionAlgin = StringAlignment.Near;
                                            break;
                                        case "RIGHT":
                                            colum.CaptionAlgin = StringAlignment.Far;
                                            break;
                                        case "CENTER":
                                            colum.CaptionAlgin = StringAlignment.Center;
                                            break;
                                        default:
                                            colum.CaptionAlgin = StringAlignment.Near;
                                            break;
                                    }
                                    switch (columnode.Attributes["ValueAlign"].Value.ToUpper()) {
                                        case "LEFT":
                                            colum.ValueAlign = StringAlignment.Near;
                                            break;
                                        case "CENTER":
                                            colum.ValueAlign = StringAlignment.Center;
                                            break;
                                        case "RIGHT":
                                            colum.ValueAlign = StringAlignment.Far;
                                            break;
                                        default:
                                            colum.ValueAlign = StringAlignment.Far;
                                            break;
                                    }
                                    if (null != columnode.Attributes["Margin"]) {
                                        String[] temp = columnode.Attributes["Margin"].Value.Split(',');
                                        if (temp.Length > 0) {
                                            if (temp.Length == 1) {
                                                colum.MarginTop = colum.MarginBottom = Convert.ToInt32(temp[0]);
                                            } else {
                                                colum.MarginTop = Convert.ToInt32(temp[0]);
                                                colum.MarginBottom = Convert.ToInt32(temp[1]);
                                            }
                                        }
                                    }
                                    if (null != columnode.Attributes["Padding"]) {
                                        String[] temp = columnode.Attributes["Padding"].Value.Split(',');
                                        if (temp.Length > 0) {
                                            if (temp.Length == 1) {
                                                colum.PaddingLeft = colum.PaddingRight = Convert.ToInt32(temp[0]);
                                            } else {
                                                colum.PaddingLeft = Convert.ToInt32(temp[0]);
                                                colum.PaddingRight = Convert.ToInt32(temp[1]);
                                            }
                                        }
                                    }
                                    if (null != columnode.Attributes["MarkLocation"]) {
                                        String location = columnode.Attributes["MarkLocation"].Value;
                                        switch (location) {
                                            case "TopLeft":
                                                colum.MarkLocation = MarkLocation.TopLeft;
                                                break;
                                            case "TopRight":
                                                colum.MarkLocation = MarkLocation.TopRight;
                                                break;
                                            case "BottomLeft":
                                                colum.MarkLocation = MarkLocation.BottomLeft;
                                                break;
                                            case "BottomRight":
                                                colum.MarkLocation = MarkLocation.BottomRight;
                                                break;
                                            default:
                                                colum.MarkLocation = MarkLocation.None;
                                                break;
                                        }
                                    }
                                    if (null != columnode.Attributes["MarkValue"]) {
                                        colum.MarkValue = columnode.Attributes["MarkValue"].Value;
                                    }
                                    row.Colums.Add(colum);
                                }
                                chart.Rows.Add(row);
                            }
                            controls.Add(chart.Name, chart);

                        }
                        charts.Add(levelnode.Name, controls);
                    }
                    return charts;
                } catch (Exception ex) {
                    LogUtilities.LogMessage("Load infopanel charts config error : " + ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取InfoPanel 布局的定义
        /// </summary>
        /// <returns></returns>
        public static Dictionary<MarketType, InfoPanelLayout> GetInfoPanelLayout() {
            String filePath = PathUtilities.CfgPath + "InfoPanelLayouts.xml";
            return GetInfoPanelLayout(filePath);
        }


        private static Dictionary<MarketType, InfoPanelLayout> GetInfoPanelLayout(String filePath) {
            if (File.Exists(filePath)) {
                try {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.SelectSingleNode(@"InfoPanel/Layouts");
                    Dictionary<MarketType, InfoPanelLayout> result = new Dictionary<MarketType, InfoPanelLayout>();
                    if (null == root)
                        return result;
                    foreach (XmlNode layoutnode in root.ChildNodes) {
                        if (layoutnode.NodeType == XmlNodeType.Comment)
                            continue;
                        InfoPanelLayout layout = new InfoPanelLayout();
                        if (null != layoutnode.Attributes["MarketType"]) {
                            MarketType type;
                            if (Enum.IsDefined(typeof (MarketType), layoutnode.Attributes["MarketType"].Value)) {
                                layout.Market =
                                    (MarketType)
                                    Enum.Parse(typeof (MarketType), layoutnode.Attributes["MarketType"].Value);
                                if (null != layoutnode.Attributes["Columns"]) {
                                    layout.Columns = Convert.ToInt32(layoutnode.Attributes["Columns"].Value);
                                }
                                if (null != layoutnode.Attributes["TopHalfHeight"]) {
                                    layout.TopHalfHeight = Convert.ToSingle(
                                        layoutnode.Attributes["TopHalfHeight"].Value);
                                }
                                foreach (XmlNode chartnode in layoutnode.ChildNodes) {
                                    if (chartnode.NodeType == XmlNodeType.Comment)
                                        continue;
                                    switch (chartnode.Name) {
                                        case "Chart":
                                            {
                                                InfoPanelLayoutChart chart = new InfoPanelLayoutChart();
                                                if (null != chartnode.Attributes["Name"]) {
                                                    chart.Name = chartnode.Attributes["Name"].Value;
                                                }
                                                if (null != chartnode.Attributes["ColIndex"]) {
                                                    chart.ColIndex =
                                                        Convert.ToInt32(chartnode.Attributes["ColIndex"].Value);
                                                }
                                                if (null != chartnode.Attributes["RowIndex"]) {
                                                    chart.RowIndex =
                                                        Convert.ToInt32(chartnode.Attributes["RowIndex"].Value);
                                                }
                                                if (null != chartnode.Attributes["RowHeight"]) {
                                                    chart.Height =
                                                        Convert.ToSingle(chartnode.Attributes["RowHeight"].Value);
                                                }
                                                if (null != chartnode.Attributes["IsSplitter"]) {
                                                    chart.IsSplitter = Convert.ToBoolean(chartnode.Attributes["IsSplitter"].Value);
                                                }
                                                layout.Charts.Add(chart);
                                                break;
                                            }
                                        case "BottomTab":
                                            {
                                                InfoPanelLayoutTabs chart = new InfoPanelLayoutTabs();
                                                if (null != chartnode.Attributes["Name"]) {
                                                    chart.Name = chartnode.Attributes["Name"].Value;
                                                }
                                                if (null != chartnode.Attributes["ColIndex"]) {
                                                    chart.ColIndex =
                                                        Convert.ToInt32(chartnode.Attributes["ColIndex"].Value);
                                                }
                                                if (null != chartnode.Attributes["RowIndex"]) {
                                                    chart.RowIndex =
                                                        Convert.ToInt32(chartnode.Attributes["RowIndex"].Value);
                                                }

                                                foreach (XmlNode tabnode in chartnode.ChildNodes) {
                                                    if (tabnode.NodeType == XmlNodeType.Comment)
                                                        continue;
                                                    InfoPanelLayoutTab tab = new InfoPanelLayoutTab();
                                                    if (null != tabnode.Attributes["Name"]) {
                                                        tab.Name = tabnode.Attributes["Name"].Value;
                                                    }

                                                    foreach (XmlNode tabchartnode in tabnode.ChildNodes) {
                                                        if (tabchartnode.NodeType == XmlNodeType.Comment)
                                                            continue;
                                                        InfoPanelLayoutChart tabChart = new InfoPanelLayoutChart();
                                                        tabChart.Location = InfoPanelLayoutChartLocation.BottomTabChart;
                                                        if (null != tabchartnode.Attributes["Name"]) {
                                                            tabChart.Name = tabchartnode.Attributes["Name"].Value;
                                                        }
                                                        if (null != tabchartnode.Attributes["ColIndex"]) {
                                                            tabChart.ColIndex =
                                                                Convert.ToInt32(
                                                                    tabchartnode.Attributes["ColIndex"].Value);
                                                        }
                                                        if (null != tabchartnode.Attributes["RowIndex"]) {
                                                            tabChart.RowIndex =
                                                                Convert.ToInt32(
                                                                    tabchartnode.Attributes["RowIndex"].Value);
                                                        }
                                                        if (null != tabchartnode.Attributes["RowHeight"]) {
                                                            tabChart.Height =
                                                                Convert.ToSingle(tabchartnode.Attributes["RowHeight"].Value);
                                                        }
                                                        if (null != tabchartnode.Attributes["IsSplitter"]) {
                                                            tabChart.IsSplitter = Convert.ToBoolean(tabchartnode.Attributes["IsSplitter"].Value);
                                                        }
                                                        tab.TabCharts.Add(tabChart);
                                                    }
                                                    chart.Tabs.Add(tab);
                                                }
                                                layout.BottomTab = chart;
                                                break;
                                            }
                                        case "TopTab":
                                            {
                                                InfoPanelLayoutTabs chart = new InfoPanelLayoutTabs();
                                                if (null != chartnode.Attributes["Name"]) {
                                                    chart.Name = chartnode.Attributes["Name"].Value;
                                                }
                                                if (null != chartnode.Attributes["ColIndex"]) {
                                                    chart.ColIndex =
                                                        Convert.ToInt32(chartnode.Attributes["ColIndex"].Value);
                                                }
                                                if (null != chartnode.Attributes["RowIndex"]) {
                                                    chart.RowIndex =
                                                        Convert.ToInt32(chartnode.Attributes["RowIndex"].Value);
                                                }
                                                foreach (XmlNode tabnode in chartnode.ChildNodes) {
                                                    if (tabnode.NodeType == XmlNodeType.Comment)
                                                        continue;
                                                    InfoPanelLayoutTab tab = new InfoPanelLayoutTab();
                                                    if (null != tabnode.Attributes["Name"]) {
                                                        tab.Name = tabnode.Attributes["Name"].Value;
                                                    }
                                                    if (null != tabnode.Attributes["Vline"]) {
                                                        tab.VLine = Convert.ToBoolean(tabnode.Attributes["Vline"].Value);
                                                    }

                                                    foreach (XmlNode tabchartnode in tabnode.ChildNodes) {
                                                        if (tabchartnode.NodeType == XmlNodeType.Comment)
                                                            continue;
                                                        InfoPanelLayoutChart tabChart = new InfoPanelLayoutChart();
                                                        tabChart.Location = InfoPanelLayoutChartLocation.TopTabChart;
                                                        if (null != tabchartnode.Attributes["Name"]) {
                                                            tabChart.Name = tabchartnode.Attributes["Name"].Value;
                                                        }
                                                        if (null != tabchartnode.Attributes["ColIndex"]) {
                                                            tabChart.ColIndex =
                                                                Convert.ToInt32(
                                                                    tabchartnode.Attributes["ColIndex"].Value);
                                                        }
                                                        if (null != tabchartnode.Attributes["RowIndex"]) {
                                                            tabChart.RowIndex =
                                                                Convert.ToInt32(
                                                                    tabchartnode.Attributes["RowIndex"].Value);
                                                        }
                                                        if (null != tabchartnode.Attributes["RowHeight"]) {
                                                            tabChart.Height =
                                                                Convert.ToSingle(tabchartnode.Attributes["RowHeight"].Value);
                                                        }
                                                        if (null != tabchartnode.Attributes["IsSplitter"]) {
                                                            tabChart.IsSplitter = Convert.ToBoolean(tabchartnode.Attributes["IsSplitter"].Value);
                                                        }
                                                        tab.TabCharts.Add(tabChart);
                                                    }
                                                    chart.Tabs.Add(tab);
                                                }
                                                layout.TopTab = chart;
                                                break;
                                            }
                                    }
                                }
                                result.Add(layout.Market, layout);
                            }
                        }
                    }
                    return result;
                } catch (Exception ex) {
                    LogUtilities.LogMessage("Load infopanel layout config error : " + ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取用户关注的短线精灵类型
        /// </summary>
        /// <returns></returns>
        public static IList<ShortLineType> GetUserShortLineTypes() {
            IList<ShortLineType> result = new List<ShortLineType>();
            String filePathUser = PathUtilities.UserPath + "shortlines.xml";
            String filePathNomal = PathUtilities.CfgPath + "shortlines.xml";

            XmlDocument doc = new XmlDocument();
            try {
                if (File.Exists(filePathUser)) {
                    doc.Load(filePathUser);
                } else if (File.Exists(filePathNomal)) {
                    doc.Load(filePathNomal);
                }
                XmlNode root = doc.SelectSingleNode("ShortLines");
                if (null != root) {
                    foreach (XmlNode itemNode in root.ChildNodes) {

                        if (Enum.IsDefined(typeof (ShortLineType), itemNode.Name)) {
                            ShortLineType tmp = (ShortLineType) (Enum.Parse(typeof (ShortLineType), itemNode.Name));
                            result.Add(tmp);
                        }
                    }
                }

            } catch (Exception e) {
                LogUtilities.LogMessage(e.Message);
            }

            if (result.Count == 0) {
                Array arr = Enum.GetValues(typeof (ShortLineType));
                foreach (ShortLineType item in arr)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 存储用户关注的短线精灵类型
        /// </summary>
        /// <param name="lines"></param>
        public static void RestoreUserShortLineTypes(IList<ShortLineType> lines) {
            if (lines.Count == 0)
                return;
            String filePath = PathUtilities.UserPath + "shortlines.xml";
            XmlDocument doc = new XmlDocument();
            XmlNode declare = doc.CreateXmlDeclaration("1.0", "utf-8", "");

            XmlNode root = doc.CreateElement("ShortLines");
            foreach (ShortLineType lineType in lines) {
                XmlNode element = doc.CreateElement(lineType.ToString());
                root.AppendChild(element);
            }
            doc.AppendChild(declare);
            doc.AppendChild(root);
            doc.Save(filePath);
        }

        public static Dictionary<MarketType, List<TopBannerMenuItemPair>> GetTopBannerMenu() {
            String filePath = PathUtilities.CfgPath + "TopBannerMenu.xml";
            if (File.Exists(filePath)) {
                try {
                    Dictionary<MarketType, List<TopBannerMenuItemPair>> result =
                        new Dictionary<MarketType, List<TopBannerMenuItemPair>>();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.SelectSingleNode("Menu");
                    foreach (XmlNode marketNode in root.ChildNodes) {
                        if (marketNode.NodeType == XmlNodeType.Comment)
                            continue;
                        if (Enum.IsDefined(typeof (MarketType), marketNode.Attributes["Type"].Value)) {
                            MarketType market =
                                (MarketType) Enum.Parse(typeof (MarketType), marketNode.Attributes["Type"].Value);
                            List<TopBannerMenuItemPair> pairs = new List<TopBannerMenuItemPair>();
                            foreach (XmlNode pairNode in marketNode.ChildNodes) {
                                int count = pairNode.ChildNodes.Count;
                                if (count > 0) {
                                    TopBannerMenuItemPair pair = new TopBannerMenuItemPair();
                                    if (count == 1)
                                    {
                                        TopBannerMenuItem item = new TopBannerMenuItem();
                                        item.Caption = pairNode.FirstChild.Attributes["Caption"].Value ?? String.Empty;
                                        item.Url = pairNode.FirstChild.Attributes["Url"].Value ?? String.Empty;
                                        item.UrlTitle = pairNode.FirstChild.Attributes["UrlTitle"].Value ?? String.Empty;

                                        pair.Item1 = item;
                                    } else {
                                        TopBannerMenuItem item1 = new TopBannerMenuItem();
                                        item1.Caption = pairNode.FirstChild.Attributes["Caption"].Value ?? String.Empty;
                                        item1.Url = pairNode.FirstChild.Attributes["Url"].Value ?? String.Empty;
                                        item1.UrlTitle = pairNode.FirstChild.Attributes["UrlTitle"].Value ?? String.Empty;
                                        TopBannerMenuItem item2 = new TopBannerMenuItem();
                                        item2.UrlTitle = pairNode.LastChild.Attributes["UrlTitle"].Value ?? String.Empty;
                                        item2.Url = pairNode.LastChild.Attributes["Url"].Value ?? String.Empty;
                                        item2.Caption = pairNode.LastChild.Attributes["Caption"].Value ?? String.Empty;
                                        pair.Item1 = item1;
                                        pair.Item2 = item2;
                                    }
                                    pairs.Add(pair);
                                }
                            }
                            result.Add(market, pairs);
                        }
                    }
                    return result;
                } catch (Exception e) {
                    LogUtilities.LogMessage(e.Message);
                    return null;
                }
            } else {
                return null;
            }
        }
    }


}
