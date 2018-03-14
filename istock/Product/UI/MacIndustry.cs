using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace OwLib
{
    public class MacIndustry
    {
        public MacIndustry(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            m_cbMacIndustry = mainFrame.GetComboBox("cbMacIndustry");
            m_cbMacIndustry.RegisterEvent(new ControlEvent(SelectedIndexChanged), EVENTID.SELECTEDINDEXCHANGED);
            m_gridMacIndustry = mainFrame.GetGrid("gridMacIndustry");
            m_gridMacIndustry.GridLineColor = COLOR.EMPTY;
            m_gridMacIndustry.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridMacIndustry.RowStyle = new GridRowStyle();
            m_gridMacIndustry.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvMacIndustry = mainFrame.GetTree("tvMacIndustry");
            m_tvMacIndustry.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            String[] names = Enum.GetNames(typeof(MacroDataType));
            for (int i = 0; i < names.Length; i++)
            {
                MenuItemA item = new MenuItemA();
                item.Text = names[i];
                m_cbMacIndustry.AddItem(item);
            }
            m_cbMacIndustry.SelectedIndex = 0;
            m_fundCurve = mainFrame.FindControl("graphMacIndustry") as FundCurveDiv;
            m_fundCurve.BackColor = COLOR.ARGB(0, 0, 0);
        }

        private ComboBoxA m_cbMacIndustry;

        private FundCurveDiv m_fundCurve;

        private GridA m_gridMacIndustry;

        private TreeA m_tvMacIndustry;

        private MainFrame m_mainFrame;

        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            TreeNodeA tn = cell as TreeNodeA;
            if (tn.m_nodes.Count == 0)
            {
                if (tn.Tag == null)
                {
                    try
                    {
                        MacroDataType type = (MacroDataType)Enum.Parse(typeof(MacroDataType), m_cbMacIndustry.Text);
                        DataTable dt = MongoRetriver.GetDescendantDataByService(tn.Name, type);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[0].ToString().Length > 0 && row[3].ToString().Length > 0)
                            {
                                TreeNodeA node = new TreeNodeA();
                                node.Tag = "2";
                                node.Text = row["STR_MACRONAME"].ToString();
                                node.Name = row[0].ToString();
                                tn.AppendNode(node);
                            }
                        }
                        tn.Tag = "1";
                        m_tvMacIndustry.Update();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (tn.Tag.ToString() == "2")
                {
                    m_gridMacIndustry.ClearRows();
                    m_fundCurve.Clear();
                    FundCurveData data = new FundCurveData();
                    XAxisPointData xdata = new XAxisPointData();
                    Dictionary<String, YAxisCurveData> yDatas = new Dictionary<string, YAxisCurveData>();
                    YAxisStyle ySyle = new YAxisStyle();
                    YAxisStyle ySyle1 = new YAxisStyle();
                    YAxisStyle ySyle2 = new YAxisStyle();
                    XAxisStyle xStyle = new XAxisStyle();
                    xStyle.XAxisType = XAxisType.Date;
                    data.ChartTitle = "每日进展";
                    xStyle.XAxisTitle = "时间";
                    data.BackgroundColor = Color.Black;
                    DataTable dt = GetMacIndustyData(tn.Name, m_cbMacIndustry.Text);
                    YAxisCurveData yData = new YAxisCurveData();
                    yData.YPointValues = new List<double>();
                    yData.LineWidth = 3;
                    yData.Text = xdata.Text;
                    yData.ShowCycle = true;
                    data.LstYAxisPointDatas.Add(yData);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString().Length > 0 && dr[1].ToString().Length > 0)
                        {
                            GridRow row = new GridRow();
                            m_gridMacIndustry.AddRow(row);
                            DateTime date = Convert.ToDateTime(dr[0]);
                            row.AddCell("colN1", new GridStringCell(date.ToString("yyyy-MM-dd")));
                            row.AddCell("colN2", new GridStringCell(dr[1].ToString()));
                            row.GetCell("colN1").Style = new GridCellStyle();
                            row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                            row.GetCell("colN1").Style.Font = new FONT("微软雅黑", 14, true, false, false);
                            row.GetCell("colN2").Style = new GridCellStyle();
                            row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                            row.GetCell("colN2").Style.Font = new FONT("微软雅黑", 14, true, false, false);
                        }
                    }
                    int rowsSize = dt.Rows.Count;
                    for (int i = rowsSize - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr[0].ToString().Length > 0 && dr[1].ToString().Length > 0)
                        {
                            GridRow row = new GridRow();
                            m_gridMacIndustry.AddRow(row);
                            DateTime date = Convert.ToDateTime(dr[0]);
                            xdata.Value = date.Year * 10000 + date.Month * 100 + date.Day;
                            xdata.Text = date.ToString("yyyyMMdd");
                            data.LstXAxisPointDatas.Add(xdata);
                            yData.YPointValues.Add(CStr.ConvertStrToDouble(dr[1].ToString()));
                        }
                    }            
                    data.XAxisStyle = xStyle;
                    ySyle.YAxisTitleAlign = YAxisTitleAlign.Middle;
                    ySyle1.YAxisTitleAlign = YAxisTitleAlign.Middle;
                    ySyle2.YAxisTitleAlign = YAxisTitleAlign.Middle;
                    xStyle.XAxisTitleAlign = XAxisTitleAlign.Center;
                    data.LstYAxisStyle.Add(ySyle);
                    data.LstYAxisStyle.Add(ySyle1);
                    data.LstYAxisStyle.Add(ySyle2);
                    m_fundCurve.FundCurveData = data;
                    m_gridMacIndustry.Update();
                    m_gridMacIndustry.Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取宏观数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="macIndustyType"></param>
        /// <returns></returns>
        public static DataTable GetMacIndustyData(String code, String macIndustyType)
        {
            MacroDataType type = (MacroDataType)Enum.Parse(typeof(MacroDataType), macIndustyType);
            string[] ids = new string[] { code };
            DataSet ds = MongoRetriver.GetMutiIndicatorValuesFromService(ids, type);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 加载指标
        /// </summary>
        /// <param name="type">类型</param>
        private void LoadType(MacroDataType type)
        {
            m_tvMacIndustry.ClearNodes();
            m_tvMacIndustry.Update();
            DataTable treeData = LocalDataRetriver.GetTreeCategoryTableFromFile(type);
            DataView dv = treeData.DefaultView;
            dv.Sort = "STR_MACROCODE ASC";
            Dictionary<String, TreeNodeA> nodesMap = new Dictionary<String, TreeNodeA>();
            foreach (DataRowView dRow in dv)
            {
                DataRow row = dRow.Row;
                String code = row[0].ToString();
                String parentCode = row[1].ToString();
                String name = row[2].ToString();
                if (nodesMap.ContainsKey(parentCode))
                {
                    TreeNodeA treeNode = new TreeNodeA();
                    treeNode.Name = code;
                    treeNode.Text = name;
                    nodesMap[parentCode].AppendNode(treeNode);
                    nodesMap[code] = treeNode;
                }
                else
                {
                    TreeNodeA treeNode = new TreeNodeA();
                    treeNode.Name = code;
                    treeNode.Text = name;
                    m_tvMacIndustry.AppendNode(treeNode);
                    nodesMap[code] = treeNode;
                }
            }
        }

        /// <summary>
        /// 下拉列表选中改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void SelectedIndexChanged(object sender)
        {
            String text = m_cbMacIndustry.SelectedText;
            MacroDataType type = (MacroDataType)Enum.Parse(typeof(MacroDataType), text);
            LoadType(type);
        }
    }
}
