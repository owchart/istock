using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace dataquery
{
    /// <summary>
    /// 自选股窗体
    /// </summary>
    public partial class SecurityForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public SecurityForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 同步到Mongo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAsyncToMongo_Click(object sender, EventArgs e)
        {
            SecurityService securityService = new SecurityService();
        }

        private DataView dataView;

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            DataCenter dataCenter = new DataCenter();
            //查询版本号
            Dictionary<String, KwItem> items = SecurityService.KwItems;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Code");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Pingyin");
            dataTable.Columns.Add("Type");
            dataTable.Columns.Add("Marketcode");
            foreach(String key in items.Keys)
            {
                KwItem kwItem = items[key];
                DataRow row = dataTable.NewRow();
                row[0] = kwItem.Code;
                row[1] = kwItem.Name;
                row[2] = kwItem.Pingyin;
                row[3] = kwItem.Type;
                row[4] = kwItem.Marketcode;
                dataTable.Rows.Add(row);
            }
            dataView = dataTable.DefaultView;
            this.dgvSecurities.DataSource = dataView;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                dataView.RowFilter = txtSearch.Text;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
