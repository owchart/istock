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
    public class AllStockNotices
    {
        /// <summary>
        /// 创建新闻
        /// </summary>
        public AllStockNotices(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            m_gridAllStockNotices = mainFrame.GetGrid("gridAllStockNotices");
            m_gridAllStockNotices.GridLineColor = COLOR.EMPTY;
            m_gridAllStockNotices.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridAllStockNotices.RowStyle = new GridRowStyle();
            m_gridAllStockNotices.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridAllStockNotices.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            m_tvAllStockNotices = mainFrame.GetTree("tvAllStockNotices");
            m_tvAllStockNotices.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvAllStockNotices.ForeColor = COLOR.ARGB(255, 255, 255);
            m_tvAllStockNotices.RowStyle = new GridRowStyle();
            m_tvAllStockNotices.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvAllStockNotices.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            object data = NoticeDataHelper.GetLeftTree("F004");
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
                    m_tvAllStockNotices.AppendNode(tn);
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
            m_tvAllStockNotices.Update();
            BindNotices("S004002");
        }

        /// <summary>
        /// 所有个股新闻
        /// </summary>
        private GridA m_gridAllStockNotices;

        /// <summary>
        /// 新闻树
        /// </summary>
        private TreeA m_tvAllStockNotices;

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
        /// 绑定公告
        /// </summary>
        /// <param name="id"></param>
        private void BindNotices(String id)
        {
            object data = NoticeDataHelper.GetNoticeById(id, "0", "100", "desc", "");
            NoticeListRoot noticeRoot = JsonConvert.DeserializeObject<NoticeListRoot>(data.ToString());
            List<NoticeListNodeBind> bindList = new List<NoticeListNodeBind>();
            foreach (NoticeListNode noticeNode in noticeRoot.records)
            {
                NoticeListNodeBind bind = new NoticeListNodeBind();
                bind.Copy(noticeNode);
                bindList.Add(bind);
            }
            m_gridAllStockNotices.ClearRows();
            foreach (NoticeListNodeBind notice in bindList)
            {
                GridRow row = new GridRow();
                m_gridAllStockNotices.AddRow(row);
                row.Tag = notice;
                row.AddCell("colN1", new GridStringCell(notice.Date));
                row.AddCell("colN2", new GridStringCell(notice.Title));
                row.AddCell("colN3", new GridStringCell(notice.url));
                row.AddCell("colN4", new GridStringCell(notice.Id));
                row.GetCell("colN1").Style = new GridCellStyle();
                row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                row.GetCell("colN2").Style = new GridCellStyle();
                row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                row.GetCell("colN3").Style = new GridCellStyle();
                row.GetCell("colN3").Style.ForeColor = COLOR.ARGB(255, 80, 255);
            }
            m_gridAllStockNotices.Update();
            m_gridAllStockNotices.Invalidate();
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
                if (cell.Grid == m_tvAllStockNotices)
                {
                    TreeNodeA tn = cell as TreeNodeA;
                    if (tn.m_nodes.Count == 0)
                    {
                        String id = tn.Tag.ToString();
                        BindNotices(id);
                    }
                }
            }
            else if (clicks == 2)
            {
                if (cell.Grid == m_gridAllStockNotices)
                {
                    NoticeListNodeBind notice = cell.Row.Tag as NoticeListNodeBind;
                    string url = notice.url;
                    Process.Start(url);
                    //string text = StockNewsDataHelper.GetRealTimeInfoByCode(cell.Row.GetCell("colN4").GetString());
                    //MessageBox.Show(text);
                }
            }
        }
    }
}
