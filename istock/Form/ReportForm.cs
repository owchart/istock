using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace OwLib
{
    /// <summary>
    /// 股票新闻
    /// </summary>
    public partial class ReportForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public ReportForm()
        {
            InitializeComponent();
            tvList.AfterSelect += new TreeViewEventHandler(tvList_AfterSelect);
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (rbViewText.Checked)
            {
                string text = ReportDataHelper.GetRealTimeInfoByCode(dgvData.Rows[e.RowIndex].Cells[1].Value.ToString());
                MessageBox.Show(text);
            }
            else
            {
                string url = dgvData.Rows[e.RowIndex].Cells[6].Value.ToString();
                Process.Start(url);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockNewsForm_Load(object sender, EventArgs e)
        {
            object data = ReportDataHelper.GetLeftTree("F004|S004004");
            NewsTypeRoot newsRoot = JsonConvert.DeserializeObject<NewsTypeRoot>(data.ToString());
            foreach (NewsTypeNode node in newsRoot.NodeList)
            {
                if (node.NodeList != null && node.NodeList.Count > 0)
                {
                    TreeNode tn = new TreeNode(node.Name);
                    tvList.Nodes.Add(tn);
                    foreach (NewsTypeNode subNode in node.NodeList)
                    {
                        TreeNode subTn = new TreeNode(subNode.Name);
                        tn.Nodes.Add(subTn);
                        subTn.Tag = subNode.Id;
                    }
                }
            }
        }

        /// <summary>
        /// 树选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Nodes.Count == 0)
            {
                if (tn.Tag != null)
                {
                    String id = tn.Tag.ToString();
                    object data = ReportDataHelper.GetReportByTreeNode(tn.Name, "", id, "0", "100", "desc", "", "");
                    ReportListRoot reportRoot = JsonConvert.DeserializeObject<ReportListRoot>(data.ToString());
                    List<ReportListNodeBind> bindList = new List<ReportListNodeBind>();
                    foreach (ReportListNode noticeNode in reportRoot.records)
                    {
                        ReportListNodeBind bind = new ReportListNodeBind();
                        bind.Copy(noticeNode);
                        bindList.Add(bind);
                    }
                    dgvData.DataSource = bindList;
                }
            }
        }

        /// <summary>
        /// 简单搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            TreeNode node = tvList.SelectedNode;
            if (node.Nodes.Count == 0)
            {
                if (node.Tag != null)
                {
                    String id = node.Tag.ToString();
                    object data = ReportDataHelper.GetReportBySelectStock("", "", id, "0", "100", "desc", "", txtCodes.Text);
                    ReportListRoot reportRoot = JsonConvert.DeserializeObject<ReportListRoot>(data.ToString());
                    List<ReportListNodeBind> bindList = new List<ReportListNodeBind>();
                    foreach (ReportListNode noticeNode in reportRoot.records)
                    {
                        ReportListNodeBind bind = new ReportListNodeBind();
                        bind.Copy(noticeNode);
                        bindList.Add(bind);
                    }
                    dgvData.DataSource = bindList;
                }
            }
        }
    }
}
