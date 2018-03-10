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
    /// 股票深度资料
    /// </summary>
    public partial class FutureF9Form : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public FutureF9Form()
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
            string code = txtCode.Text.Trim();
            if (code.IndexOf('.') != -1)
            {
                code = code.Substring(0, code.IndexOf('.'));
            }
            DataSet ds = null;
            if (rbFuturesBaseInfo.Checked)
            {
                ds = FutureF9DataHelper.GetFuturesBaseInfo(code);
            }
            else if (rbContractBrief.Checked)
            {
                ds = FutureF9DataHelper.GetContractBrief(code);
            }
            else if (rbAllMemberAgencies.Checked)
            {
                ds = FutureF9DataHelper.GetAllMemberAgencies();
            }
            else if (rbPositionStructJson.Checked)
            {
                ds = FutureF9DataHelper.GetPositionPriceJson(code);
            }
            else if (rbPartyModel.Checked)
            {
                ds = FutureF9DataHelper.GetPartyModel();
            }
            else if (rbPartyInfoModel.Checked)
            {
                ds = FutureF9DataHelper.GetPartyInfoModel("");
            }
            else if (rbShibor.Checked)
            {
                ds = FutureF9DataHelper.GetRateShibor();
            }
            else if (rbRateBank.Checked)
            {
                ds = FutureF9DataHelper.GetRateBank();
            }
            else if (rbPositionPriceJson.Checked)
            {
                ds = FutureF9DataHelper.GetPositionPriceJson(code);
            }
            else if (rbProfitAnalysisJson.Checked)
            {
                ds = FutureF9DataHelper.GetProfitAnalysisJson(code);
            }
            this.dgvData.DataSource = ds.Tables[0];
        }
    }
}
