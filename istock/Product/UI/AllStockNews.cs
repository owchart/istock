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
    public class AllStockNews
    {
        /// <summary>
        /// 创建新闻
        /// </summary>
        public AllStockNews(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            m_gridAllStockNews = mainFrame.GetGrid("gridAllStockNews");
            m_gridAllStockNews.GridLineColor = COLOR.EMPTY;
            m_gridAllStockNews.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridAllStockNews.RowStyle = new GridRowStyle();
            m_gridAllStockNews.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridAllStockNews.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            m_tvAllStockNews = mainFrame.GetTree("tvAllStockNews");
            m_tvAllStockNews.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvAllStockNews.ForeColor = COLOR.ARGB(255, 255, 255);
            m_tvAllStockNews.RowStyle = new GridRowStyle();
            m_tvAllStockNews.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvAllStockNews.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            object data = StockNewsDataHelper.GetLeftTree("F888011");
            NewsTypeRoot newsRoot = JsonConvert.DeserializeObject<NewsTypeRoot>(data.ToString());
            foreach (NewsTypeNode node in newsRoot.NodeList)
            {
                if (node.NodeList != null && node.NodeList.Count > 0)
                {
                    TreeNodeA tn = new TreeNodeA();
                    tn.Style = new GridCellStyle();
                    tn.Style.ForeColor = COLOR.ARGB(255, 255, 255);
                    tn.Style.Font = new FONT("微软雅黑", 14, true, false, false);
                    tn.Text = node.Name;
                    m_tvAllStockNews.AppendNode(tn);
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
            BindStockNews("S888005001");
            m_tvAllStockNews.Update();
        }

        /// <summary>
        /// 所有个股新闻
        /// </summary>
        private GridA m_gridAllStockNews;

        /// <summary>
        /// 新闻树
        /// </summary>
        private TreeA m_tvAllStockNews;

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
        /// 绑定新闻
        /// </summary>
        /// <param name="id"></param>
        private void BindStockNews(String id)
        {
            object data = StockNewsDataHelper.GetNewsById(id, "0", "100", "desc", "");
            NewsListRoot newsRoot = JsonConvert.DeserializeObject<NewsListRoot>(data.ToString());
            m_gridAllStockNews.ClearRows();
            foreach (NewsListNode stockNew in newsRoot.records)
            {
                GridRow row = new GridRow();
                m_gridAllStockNews.AddRow(row);
                row.Tag = stockNew;
                row.AddCell("colN1", new GridStringCell(stockNew.Date));
                row.AddCell("colN2", new GridStringCell(stockNew.Title));
                row.AddCell("colN3", new GridStringCell(stockNew.url));
                row.AddCell("colN4", new GridStringCell(stockNew.Id));
                row.GetCell("colN1").Style = new GridCellStyle();
                row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                row.GetCell("colN2").Style = new GridCellStyle();
                row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                row.GetCell("colN3").Style = new GridCellStyle();
                row.GetCell("colN3").Style.ForeColor = COLOR.ARGB(255, 80, 255);
            }
            m_gridAllStockNews.Update();
            m_gridAllStockNews.Invalidate();
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
                if (cell.Grid == m_tvAllStockNews)
                {
                    TreeNodeA tn = cell as TreeNodeA;
                    if (tn.m_nodes.Count == 0)
                    {
                        String id = tn.Tag.ToString();
                        BindStockNews(id);
                    }
                }
            }
            else if (clicks == 2)
            {
                if (cell.Grid == m_gridAllStockNews)
                {
                    NewsListNode stockNew = cell.Row.Tag as NewsListNode;
                    string url = stockNew.url;
                    Process.Start(url);
                    //string text = StockNewsDataHelper.GetRealTimeInfoByCode(cell.Row.GetCell("colN4").GetString());
                    //MessageBox.Show(text);
                }
            }
        }
    }
}
