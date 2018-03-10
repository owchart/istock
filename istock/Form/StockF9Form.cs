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
    /// 股票深度资料
    /// </summary>
    public partial class StockF9Form : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public StockF9Form()
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
            if (rbCompanyIntroductionInfo.Checked)
            {
                ds = StockF9DataHelper.GetCompanyIntroductionInfo(code);
            }
            else if (rbCompanyNameHistoryList.Checked)
            {
                ds = StockF9DataHelper.GetCompanyNameHistoryList(code);
            }
            else if (rbStockStructureList.Checked)
            {
                ds = StockF9DataHelper.GetStockStructureList(code, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbTop10HolderList.Checked)
            {
                ds = StockF9DataHelper.GetTop10HolderList(code, 1, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbTop10HolderList2.Checked)
            {
                ds = StockF9DataHelper.GetTop10HolderList(code, 2, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbInstitutionInvestorList.Checked)
            {
                ds = StockF9DataHelper.GetInstitutionInvestorList(code, "", "desc", "", "2017,2016,2015,2014", null, "1");
            }
            else if (rbStockHolderNumberList.Checked)
            {
                ds = StockF9DataHelper.GetStockHolderNumberList(code, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbStockUnlimitedTimeList.Checked)
            {
                ds = StockF9DataHelper.GetStockUnlimitedTimeList(code);
            }
            else if (rbManagerInfo.Checked)
            {
                ds = StockF9DataHelper.GetManagerInfo(code, 1);
            }
            else if (rbManagementRemuneration.Checked)
            {
                ds = StockF9DataHelper.ManagementRemuneration(code, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbManagementStockChange.Checked)
            {
                ds = StockF9DataHelper.ManagementStockChange(code, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbStockMarketExpressList.Checked)
            {
                ds = StockF9DataHelper.GetStockMarketExpressList(code, 0, 1);
            }
            else if (rbSecurityIntroduction.Checked)
            {
                ds = StockF9DataHelper.SecurityIntroduction(code);
            }
            else if (rbSpecialProcessDelistedOpen.Checked)
            {
                ds = StockF9DataHelper.SpecialProcessDelistedOpen(code);
            }
            else if (rbIndustryInfo.Checked)
            {
                ds = StockF9DataHelper.GetIndustryInfo(code, 2);
            }
            else if (rbSecurityIndex.Checked)
            {
                ds = StockF9DataHelper.GetSecurityIndex(code);
            }
            else if (rbConceptBoardList.Checked)
            {
                ds = StockF9DataHelper.GetConceptBoardList(code);
            }
            else if (rbStageMarketDataList.Checked)
            {
                ds = StockF9DataHelper.GetStageMarketDataList(code, "desc");
            }
            else if (rbBargainOfficeList.Checked)
            {
                ds = StockF9DataHelper.GetBargainOfficeList(code, "desc", "2017,2016,2015,2014", null, "1");
            }
            else if (rbFundFlowList.Checked)
            {
                ds = StockF9DataHelper.GetFundFlowListForChart(code);
            }
            else if (rbSecurityFinancingList.Checked)
            {
                ds = StockF9DataHelper.GetSecurityFinancingList(code, "2017,2016,2015,2014", null, "1", "100");
            }
            else if (rbOperationAbility.Checked)
            {
                ds = StockF9DataHelper.OperationAbility(code);
            }
            else if (rbProfitAbility.Checked)
            {
                ds = StockF9DataHelper.ProfitAbility(code);
            }
            else if (rbCapitalStructure.Checked)
            {
                ds = StockF9DataHelper.CapitalStructure(code);
            }
            else if (rbSingleFinanceIndex.Checked)
            {
                ds = StockF9DataHelper.SingleFinanceIndex(code);
            }
            else if (rbTransferDebt.Checked)
            {
                ds = StockF9DataHelper.TransferDebt(code);
            }
            else if (rbFinanceDetails.Checked)
            {
                ds = StockF9DataHelper.FinanceDetails(code);
            }
            else if (rbGuaranteeCash.Checked)
            {
                ds = StockF9DataHelper.GuaranteeCash(code);
            }
            else if (rbStockDetails.Checked)
            {
                ds = StockF9DataHelper.StockDetails(code);
            }
            else if (rbBargainExchange.Checked)
            {
                ds = StockF9DataHelper.BargainExchange(code);
            }
            this.dgvData.DataSource = ds.Tables[0];
        }
    }
}
