using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dataquery
{
    /// <summary>
    /// 日历窗体
    /// </summary>
    public partial class TradeInfoForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public TradeInfoForm()
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
            string date = mcCalendar.SelectionRange.Start.ToString("yyyy-MM-dd");
            DataSet ds = null;
            if (rbBusinessDate.Checked)
            {
                ds = TradeInfoDataHelper.GetBusinessDate(date);
            }
            else if (rbShareChangeDate.Checked)
            {
                ds = TradeInfoDataHelper.GetShareChangeDate(date);
            }
            else if (rbBigBuyDate.Checked)
            {
                ds = TradeInfoDataHelper.GetBigBuyDate(date);
            }
            else if (rbPredictPriceDate.Checked)
            {
                ds = TradeInfoDataHelper.GetPredictPriceDate(date);
            }
            else if (rbFinancialShowDate.Checked)
            {
                ds = TradeInfoDataHelper.GetFinancialShowDate(date);
            }
            else if (rbLiftLimitDate.Checked)
            {
                ds = TradeInfoDataHelper.GetLiftLimitDate(date);
            }
            else if (rbDealOutDate.Checked)
            {
                ds = TradeInfoDataHelper.GetDealOutDate(date);
            }
            else if (rbHomeData.Checked)
            {
                ds = TradeInfoDataHelper.GetHomeData(date);
            }
            else if (rbIssueData.Checked)
            {
                ds = TradeInfoDataHelper.GetIssueData(date);
            }
            else if (rbPaymentData.Checked)
            {
                ds = TradeInfoDataHelper.GetPaymentData(date);
            }
            else if (rbTransactionData.Checked)
            {
                ds = TradeInfoDataHelper.GetTransactionData(date);
            }
            else if (rbRatingChangeData.Checked)
            {
                ds = TradeInfoDataHelper.GetRatingChangeData(date);
            }
            else if (rbConvertibleBondsData.Checked)
            {
                ds = TradeInfoDataHelper.GetConvertibleBondsData(date);
            }
            else if (rbFundHomePage.Checked)
            {
                ds = TradeInfoDataHelper.GetHomeData(date);
            }
            else if (rbFundEnter.Checked)
            {
                ds = TradeInfoDataHelper.GetFundEnter(date, "0");
            }
            else if (rbFundShare.Checked)
            {
                ds = TradeInfoDataHelper.GetFundShare(date, "0");
            }
            else if (rbFundIssue.Checked)
            {
                ds = TradeInfoDataHelper.GetFundIssue(date, "0");
            }
            else if (rbOtherNotice.Checked)
            {
                ds = TradeInfoDataHelper.GetOtherNotice(date, "0");
            }
            else if (rbFundSplit.Checked)
            {
                ds = TradeInfoDataHelper.GetFundSplit(date, "0", "1");
            }
            else if (rbIssueSale.Checked)
            {
                ds = TradeInfoDataHelper.GetIssueSale(date, "0", "1");
            }
            else if (rbFundManager.Checked)
            {
                ds = TradeInfoDataHelper.GetFundManager(date);
            }
            else if (rbFundChange.Checked)
            {
                ds = TradeInfoDataHelper.GetFundChange(date);
            }
            else if (rbFundNameChange.Checked)
            {
                ds = TradeInfoDataHelper.GetFundNameChange(date);
            }
            else if (rbFinanceShow.Checked)
            {
                ds = TradeInfoDataHelper.GetFinanceShow(date);
            }
            this.dgvData.DataSource = ds.Tables[0];
        }
    }
}
