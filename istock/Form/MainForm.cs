using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EmCore;
using System.Threading;
using System.IO;
using EmSerDataService;

namespace dataquery
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 经济业务大全
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiEcomomy_Click(object sender, EventArgs e)
        {
            EcomomyForm ecomomyForm = new EcomomyForm();
            ecomomyForm.TopLevel = false;
            this.Controls.Add(ecomomyForm);
            ecomomyForm.MdiParent = this;
            ecomomyForm.Show();
        }

        /// <summary>
        /// 交易所信息查看
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiTradeInfo_Click(object sender, EventArgs e)
        {
            TradeInfoForm tradeInfoForm = new TradeInfoForm();
            tradeInfoForm.TopLevel = false;
            this.Controls.Add(tradeInfoForm);
            tradeInfoForm.MdiParent = this;
            tradeInfoForm.Show();
        }

        /// <summary>
        /// 数据浏览器
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiStockBrower_Click(object sender, EventArgs e)
        {
            IndicatorForm indicatorForm = new IndicatorForm();
            indicatorForm.TopLevel = false;
            this.Controls.Add(indicatorForm);
            indicatorForm.MdiParent = this;
            indicatorForm.Show();
        }

        /// <summary>
        /// 深度资料
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiStockF9_Click(object sender, EventArgs e)
        {
            StockF9Form stockF9Form = new StockF9Form();
            stockF9Form.TopLevel = false;
            this.Controls.Add(stockF9Form);
            stockF9Form.MdiParent = this;
            stockF9Form.Show();
        }

        /// <summary>
        /// 股票新闻
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiNewStock_Click(object sender, EventArgs e)
        {
            NewStockForm stockNewsForm = new NewStockForm();
            stockNewsForm.TopLevel = false;
            this.Controls.Add(stockNewsForm);
            stockNewsForm.MdiParent = this;
            stockNewsForm.Show();
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiNotice_Click(object sender, EventArgs e)
        {
            NoticeForm noticeForm = new NoticeForm();
            noticeForm.TopLevel = false;
            this.Controls.Add(noticeForm);
            noticeForm.MdiParent = this;
            noticeForm.Show();
        }

        /// <summary>
        /// 研报
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiReport_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.TopLevel = false;
            this.Controls.Add(reportForm);
            reportForm.MdiParent = this;
            reportForm.Show();
        }

        /// <summary>
        /// 码表浏览器
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiSecurityBrower_Click(object sender, EventArgs e)
        {
            SecurityForm securityForm = new SecurityForm();
            securityForm.TopLevel = false;
            this.Controls.Add(securityForm);
            securityForm.MdiParent = this;
            securityForm.Show();
        }

        /// <summary>
        /// 其他
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiOther_Click(object sender, EventArgs e)
        {
            FormulaForm otherForm = new FormulaForm();
            otherForm.TopLevel = false;
            this.Controls.Add(otherForm);
            otherForm.MdiParent = this;
            otherForm.Show();
        }

        /// <summary>
        /// 专题统计
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiReportWatch_Click(object sender, EventArgs e)
        {
            SpecialForm specialForm = new SpecialForm();
            specialForm.TopLevel = false;
            this.Controls.Add(specialForm);
            specialForm.MdiParent = this;
            specialForm.Show();
        }

        /// <summary>
        /// 个股资讯
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiSingleInfo_Click(object sender, EventArgs e)
        {
            SingleInfoForm singleInfoForm = new SingleInfoForm();
            singleInfoForm.TopLevel = false;
            this.Controls.Add(singleInfoForm);
            singleInfoForm.MdiParent = this;
            singleInfoForm.Show();
        }

        /// <summary>
        /// 股票新闻
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiStockNews_Click(object sender, EventArgs e)
        {
            StockNewsForm stockNewsForm = new StockNewsForm();
            stockNewsForm.TopLevel = false;
            this.Controls.Add(stockNewsForm);
            stockNewsForm.MdiParent = this;
            stockNewsForm.Show();
        }

        /// <summary>
        /// 窗体退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 期货深度资料
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiFutureF9_Click(object sender, EventArgs e)
        {
            FutureF9Form futueF9Form = new FutureF9Form();
            futueF9Form.TopLevel = false;
            this.Controls.Add(futueF9Form);
            futueF9Form.MdiParent = this;
            futueF9Form.Show();
        }

        /// <summary>
        /// 宏观数据
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiMacIndusty_Click(object sender, EventArgs e)
        {
            MacIndustyForm macIndustyForm = new MacIndustyForm();
            macIndustyForm.TopLevel = false;
            this.Controls.Add(macIndustyForm);
            macIndustyForm.MdiParent = this;
            macIndustyForm.Show();
        }

        /// <summary>
        /// 行情
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tsmiQuote_Click(object sender, EventArgs e)
        {
            QuoteForm quoteForm = new QuoteForm();
            quoteForm.TopLevel = false;
            this.Controls.Add(quoteForm);
            quoteForm.MdiParent = this;
            quoteForm.Show();
        }

        /// <summary>
        /// 股票公式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiFormula_Click(object sender, EventArgs e)
        {
            FormulaForm formulaForm = new FormulaForm();
            formulaForm.TopLevel = false;
            this.Controls.Add(formulaForm);
            formulaForm.MdiParent = this;
            formulaForm.Show();
        }

        private void tsmiOther_Click_1(object sender, EventArgs e)
        {
            OtherForm otherForm = new OtherForm();
            otherForm.TopLevel = false;
            this.Controls.Add(otherForm);
            otherForm.MdiParent = this;
            otherForm.Show();
        }
    }
}