using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dataquery.Web;

namespace dataquery
{
    /// <summary>
    /// 股票新闻窗体
    /// </summary>
    public partial class NewStockForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public NewStockForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            if (rbNewStockName.Checked)
            {
                ds = NewStockDataHelper.GetNewStockName();
            }
            else if (rbNewStockCalendar.Checked)
            {
                ds = NewStockDataHelper.GetNewStockCalendar(0);
            }
            else if (rbStockIssueMsg.Checked)
            {
                ds = NewStockDataHelper.GetStockIssueMsg("0", "0");
            }
            else if (rbStockPredictMsg.Checked)
            {
                ds = NewStockDataHelper.GetStockPredictMsg();
            }
            else if (rbStockIPOPage.Checked)
            {
                ds = NewStockDataHelper.GetStockIPOPage();
            }
            else if (rbStockIPODataStatistics.Checked)
            {
                ds = NewStockDataHelper.GetStockIPODataStatistics();
            }
            else if (rbStockIPOHistoryStatistics.Checked)
            {
                ds = NewStockDataHelper.GetStockIPOHistoryStatistics();
            }
            else if (rbStockIPOMarketExpress.Checked)
            {
                ds = NewStockDataHelper.GetStockIPOMarketExpress();
            }
            else if (rbStockIPOZQRate.Checked)
            {
                ds = NewStockDataHelper.GetStockIPOZQRate();
            }
            this.dgvData.DataSource = ds.Tables[0];
        }
    }
}
