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
using dataquery.Web;

namespace dataquery
{
    /// <summary>
    /// 股票新闻
    /// </summary>
    public partial class NoticeForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public NoticeForm()
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
                string text = NoticeDataHelper.GetRealTimeInfoByCode(dgvData.Rows[e.RowIndex].Cells[6].Value.ToString());
                MessageBox.Show(text);
            }
            else
            {
                string url = dgvData.Rows[e.RowIndex].Cells[1].Value.ToString();
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
            object data = NoticeDataHelper.GetLeftTree("F004");
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
                    object data = NoticeDataHelper.GetNoticeById(id, "0", "100", "desc", "");
                    NoticeListRoot noticeRoot = JsonConvert.DeserializeObject<NoticeListRoot>(data.ToString());
                    List<NoticeListNodeBind> bindList = new List<NoticeListNodeBind>();
                    foreach (NoticeListNode noticeNode in noticeRoot.records)
                    {
                        NoticeListNodeBind bind = new NoticeListNodeBind();
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
                    object data = NoticeDataHelper.GetNoticeByParam(id, txtCodes.Text, "", "0", "100", "desc", "");
                    NoticeListRoot noticeRoot = JsonConvert.DeserializeObject<NoticeListRoot>(data.ToString());
                    List<NoticeListNodeBind> bindList = new List<NoticeListNodeBind>();
                    foreach (NoticeListNode noticeNode in noticeRoot.records)
                    {
                        NoticeListNodeBind bind = new NoticeListNodeBind();
                        bind.Copy(noticeNode);
                        bindList.Add(bind);
                    }
                    dgvData.DataSource = bindList;
                }
            }
        }
    }
}
