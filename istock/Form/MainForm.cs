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
    /// ������
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ����ҵ���ȫ
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiEcomomy_Click(object sender, EventArgs e)
        {
            EcomomyForm ecomomyForm = new EcomomyForm();
            ecomomyForm.TopLevel = false;
            this.Controls.Add(ecomomyForm);
            ecomomyForm.MdiParent = this;
            ecomomyForm.Show();
        }

        /// <summary>
        /// ��������Ϣ�鿴
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiTradeInfo_Click(object sender, EventArgs e)
        {
            TradeInfoForm tradeInfoForm = new TradeInfoForm();
            tradeInfoForm.TopLevel = false;
            this.Controls.Add(tradeInfoForm);
            tradeInfoForm.MdiParent = this;
            tradeInfoForm.Show();
        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiStockBrower_Click(object sender, EventArgs e)
        {
            IndicatorForm indicatorForm = new IndicatorForm();
            indicatorForm.TopLevel = false;
            this.Controls.Add(indicatorForm);
            indicatorForm.MdiParent = this;
            indicatorForm.Show();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiStockF9_Click(object sender, EventArgs e)
        {
            StockF9Form stockF9Form = new StockF9Form();
            stockF9Form.TopLevel = false;
            this.Controls.Add(stockF9Form);
            stockF9Form.MdiParent = this;
            stockF9Form.Show();
        }

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiNewStock_Click(object sender, EventArgs e)
        {
            NewStockForm stockNewsForm = new NewStockForm();
            stockNewsForm.TopLevel = false;
            this.Controls.Add(stockNewsForm);
            stockNewsForm.MdiParent = this;
            stockNewsForm.Show();
        }

        /// <summary>
        /// ����
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
        /// �б�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiReport_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.TopLevel = false;
            this.Controls.Add(reportForm);
            reportForm.MdiParent = this;
            reportForm.Show();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiSecurityBrower_Click(object sender, EventArgs e)
        {
            SecurityForm securityForm = new SecurityForm();
            securityForm.TopLevel = false;
            this.Controls.Add(securityForm);
            securityForm.MdiParent = this;
            securityForm.Show();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiOther_Click(object sender, EventArgs e)
        {
            FormulaForm otherForm = new FormulaForm();
            otherForm.TopLevel = false;
            this.Controls.Add(otherForm);
            otherForm.MdiParent = this;
            otherForm.Show();
        }

        /// <summary>
        /// ר��ͳ��
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiReportWatch_Click(object sender, EventArgs e)
        {
            SpecialForm specialForm = new SpecialForm();
            specialForm.TopLevel = false;
            this.Controls.Add(specialForm);
            specialForm.MdiParent = this;
            specialForm.Show();
        }

        /// <summary>
        /// ������Ѷ
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiSingleInfo_Click(object sender, EventArgs e)
        {
            SingleInfoForm singleInfoForm = new SingleInfoForm();
            singleInfoForm.TopLevel = false;
            this.Controls.Add(singleInfoForm);
            singleInfoForm.MdiParent = this;
            singleInfoForm.Show();
        }

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiStockNews_Click(object sender, EventArgs e)
        {
            StockNewsForm stockNewsForm = new StockNewsForm();
            stockNewsForm.TopLevel = false;
            this.Controls.Add(stockNewsForm);
            stockNewsForm.MdiParent = this;
            stockNewsForm.Show();
        }

        /// <summary>
        /// �����˳�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// �ڻ��������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiFutureF9_Click(object sender, EventArgs e)
        {
            FutureF9Form futueF9Form = new FutureF9Form();
            futueF9Form.TopLevel = false;
            this.Controls.Add(futueF9Form);
            futueF9Form.MdiParent = this;
            futueF9Form.Show();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiMacIndusty_Click(object sender, EventArgs e)
        {
            MacIndustyForm macIndustyForm = new MacIndustyForm();
            macIndustyForm.TopLevel = false;
            this.Controls.Add(macIndustyForm);
            macIndustyForm.MdiParent = this;
            macIndustyForm.Show();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tsmiQuote_Click(object sender, EventArgs e)
        {
            QuoteForm quoteForm = new QuoteForm();
            quoteForm.TopLevel = false;
            this.Controls.Add(quoteForm);
            quoteForm.MdiParent = this;
            quoteForm.Show();
        }

        /// <summary>
        /// ��Ʊ��ʽ
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