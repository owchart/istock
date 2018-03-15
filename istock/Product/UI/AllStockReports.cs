using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace OwLib
{
    /// <summary>
    /// 个股新闻
    /// </summary>
    public class AllStockReports
    {
        /// <summary>
        /// 创建新闻
        /// </summary>
        public AllStockReports(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            m_gridAllStockReports = mainFrame.GetGrid("gridAllStockReports");
            m_gridAllStockReports.GridLineColor = COLOR.EMPTY;
            m_gridAllStockReports.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridAllStockReports.RowStyle = new GridRowStyle();
            m_gridAllStockReports.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridAllStockReports.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            m_tvAllStockReports = mainFrame.GetTree("tvAllStockReports");
            m_tvAllStockReports.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvAllStockReports.ForeColor = COLOR.ARGB(255, 255, 255);
            m_tvAllStockReports.RowStyle = new GridRowStyle();
            m_tvAllStockReports.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvAllStockReports.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            object data = ReportDataHelper.GetLeftTree("F004|S004004");
            NewsTypeRoot newsRoot = JsonConvert.DeserializeObject<NewsTypeRoot>(data.ToString());
            foreach (NewsTypeNode node in newsRoot.NodeList)
            {
                if (node.NodeList != null && node.NodeList.Count > 0)
                {
                    TreeNodeA tn = new TreeNodeA();
                    tn.Text = node.Name;
                    tn.Style = new GridCellStyle();
                    tn.Style.ForeColor = COLOR.ARGB(255, 255, 255);
                    tn.Style.Font = new FONT("微软雅黑", 14, true, false, false);
                    m_tvAllStockReports.AppendNode(tn);
                    foreach (NewsTypeNode subNode in node.NodeList)
                    {
                        TreeNodeA subTn = new TreeNodeA();
                        subTn.Text = subNode.Name;
                        subTn.Style = new GridCellStyle();
                        subTn.Style.ForeColor = COLOR.ARGB(255, 255, 255);
                        subTn.Style.Font = new FONT("微软雅黑", 14, true, false, false);
                        tn.AppendNode(subTn);
                        subTn.Tag = subNode.Id;
                    }
                }
            }
            m_tvAllStockReports.Update();
            BindReports("S103001");
        }

        /// <summary>
        /// 所有个股新闻
        /// </summary>
        private GridA m_gridAllStockReports;

        /// <summary>
        /// 新闻树
        /// </summary>
        private TreeA m_tvAllStockReports;

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置主框架
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// 绑定研报
        /// </summary>
        /// <param name="id"></param>
        private void BindReports(String id)
        {
            object data = ReportDataHelper.GetReportByTreeNode("", "", id, "0", "100", "desc", "", "");
            ReportListRoot reportRoot = JsonConvert.DeserializeObject<ReportListRoot>(data.ToString());
            List<ReportListNodeBind> bindList = new List<ReportListNodeBind>();
            foreach (ReportListNode noticeNode in reportRoot.records)
            {
                ReportListNodeBind bind = new ReportListNodeBind();
                bind.Copy(noticeNode);
                bindList.Add(bind);
            }
            m_gridAllStockReports.ClearRows();
            foreach (ReportListNodeBind report in bindList)
            {
                GridRow row = new GridRow();
                m_gridAllStockReports.AddRow(row);
                row.Tag = report;
                row.AddCell("colN1", new GridStringCell(report.date));
                row.AddCell("colN2", new GridStringCell(report.Title));
                row.AddCell("colN3", new GridStringCell(report.Url));
                row.AddCell("colN4", new GridStringCell(report.id));
                row.GetCell("colN1").Style = new GridCellStyle();
                row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                row.GetCell("colN2").Style = new GridCellStyle();
                row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                row.GetCell("colN3").Style = new GridCellStyle();
                row.GetCell("colN3").Style.ForeColor = COLOR.ARGB(255, 80, 255);
            }
            m_gridAllStockReports.Update();
            m_gridAllStockReports.Invalidate();
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
            if (clicks == 1)
            {
                if (cell.Grid == m_tvAllStockReports)
                {
                    TreeNodeA tn = cell as TreeNodeA;
                    if (tn.m_nodes.Count == 0)
                    {
                        String id = tn.Tag.ToString();
                        BindReports(id);
                    }
                }
            }
            else if (clicks == 2)
            {
                if (cell.Grid == m_gridAllStockReports)
                {
                    ReportListNodeBind notice = cell.Row.Tag as ReportListNodeBind;
                    string url = notice.Url;
                    Process.Start(url);
                    //string text = StockNewsDataHelper.GetRealTimeInfoByCode(cell.Row.GetCell("colN4").GetString());
                    //MessageBox.Show(text);
                }
            }
        }
    }
}
