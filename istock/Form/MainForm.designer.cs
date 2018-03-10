namespace dataquery
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.大数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStockBrower = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMacIndusty = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReportWatch = new System.Windows.Forms.ToolStripMenuItem();
            this.行情ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiQuote = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSecurities = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFormula = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStockF9 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewStock = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTradeInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEcomomy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFutureF9 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOther = new System.Windows.Forms.ToolStripMenuItem();
            this.资讯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotice = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStockNews = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSingleInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.大数据ToolStripMenuItem,
            this.行情ToolStripMenuItem,
            this.tsmiData,
            this.资讯ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(920, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 大数据ToolStripMenuItem
            // 
            this.大数据ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStockBrower,
            this.tsmiMacIndusty,
            this.tsmiReportWatch});
            this.大数据ToolStripMenuItem.Name = "大数据ToolStripMenuItem";
            this.大数据ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.大数据ToolStripMenuItem.Text = "大数据";
            // 
            // tsmiStockBrower
            // 
            this.tsmiStockBrower.Name = "tsmiStockBrower";
            this.tsmiStockBrower.Size = new System.Drawing.Size(136, 22);
            this.tsmiStockBrower.Text = "数据浏览器";
            this.tsmiStockBrower.Click += new System.EventHandler(this.tsmiStockBrower_Click);
            // 
            // tsmiMacIndusty
            // 
            this.tsmiMacIndusty.Name = "tsmiMacIndusty";
            this.tsmiMacIndusty.Size = new System.Drawing.Size(136, 22);
            this.tsmiMacIndusty.Text = "宏观数据";
            this.tsmiMacIndusty.Click += new System.EventHandler(this.tsmiEcomomy_Click);
            // 
            // tsmiReportWatch
            // 
            this.tsmiReportWatch.Name = "tsmiReportWatch";
            this.tsmiReportWatch.Size = new System.Drawing.Size(136, 22);
            this.tsmiReportWatch.Text = "专题统计";
            this.tsmiReportWatch.Click += new System.EventHandler(this.tsmiReportWatch_Click);
            // 
            // 行情ToolStripMenuItem
            // 
            this.行情ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiQuote,
            this.tsmiSecurities,
            this.tsmiFormula});
            this.行情ToolStripMenuItem.Name = "行情ToolStripMenuItem";
            this.行情ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.行情ToolStripMenuItem.Text = "行情";
            // 
            // tsmiQuote
            // 
            this.tsmiQuote.Name = "tsmiQuote";
            this.tsmiQuote.Size = new System.Drawing.Size(124, 22);
            this.tsmiQuote.Text = "实时行情";
            this.tsmiQuote.Click += new System.EventHandler(this.tsmiQuote_Click);
            // 
            // tsmiSecurities
            // 
            this.tsmiSecurities.Name = "tsmiSecurities";
            this.tsmiSecurities.Size = new System.Drawing.Size(124, 22);
            this.tsmiSecurities.Text = "码表信息";
            this.tsmiSecurities.Click += new System.EventHandler(this.tsmiSecurityBrower_Click);
            // 
            // tsmiFormula
            // 
            this.tsmiFormula.Name = "tsmiFormula";
            this.tsmiFormula.Size = new System.Drawing.Size(124, 22);
            this.tsmiFormula.Text = "股票公式";
            this.tsmiFormula.Click += new System.EventHandler(this.tsmiFormula_Click);
            // 
            // tsmiData
            // 
            this.tsmiData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStockF9,
            this.tsmiNewStock,
            this.tsmiTradeInfo,
            this.tsmiEcomomy,
            this.tsmiFutureF9,
            this.tsmiOther});
            this.tsmiData.Name = "tsmiData";
            this.tsmiData.Size = new System.Drawing.Size(44, 21);
            this.tsmiData.Text = "数据";
            // 
            // tsmiStockF9
            // 
            this.tsmiStockF9.Name = "tsmiStockF9";
            this.tsmiStockF9.Size = new System.Drawing.Size(152, 22);
            this.tsmiStockF9.Text = "股票深度资料";
            this.tsmiStockF9.Click += new System.EventHandler(this.tsmiStockF9_Click);
            // 
            // tsmiNewStock
            // 
            this.tsmiNewStock.Name = "tsmiNewStock";
            this.tsmiNewStock.Size = new System.Drawing.Size(152, 22);
            this.tsmiNewStock.Text = "新股信息";
            this.tsmiNewStock.Click += new System.EventHandler(this.tsmiNewStock_Click);
            // 
            // tsmiTradeInfo
            // 
            this.tsmiTradeInfo.Name = "tsmiTradeInfo";
            this.tsmiTradeInfo.Size = new System.Drawing.Size(152, 22);
            this.tsmiTradeInfo.Text = "交易所信息";
            this.tsmiTradeInfo.Click += new System.EventHandler(this.tsmiTradeInfo_Click);
            // 
            // tsmiEcomomy
            // 
            this.tsmiEcomomy.Name = "tsmiEcomomy";
            this.tsmiEcomomy.Size = new System.Drawing.Size(152, 22);
            this.tsmiEcomomy.Text = "经济业务大全";
            this.tsmiEcomomy.Click += new System.EventHandler(this.tsmiEcomomy_Click);
            // 
            // tsmiFutureF9
            // 
            this.tsmiFutureF9.Name = "tsmiFutureF9";
            this.tsmiFutureF9.Size = new System.Drawing.Size(152, 22);
            this.tsmiFutureF9.Text = "期货深度资料";
            this.tsmiFutureF9.Click += new System.EventHandler(this.tsmiFutureF9_Click);
            // 
            // tsmiOther
            // 
            this.tsmiOther.Name = "tsmiOther";
            this.tsmiOther.Size = new System.Drawing.Size(152, 22);
            this.tsmiOther.Text = "数据导出";
            this.tsmiOther.Click += new System.EventHandler(this.tsmiOther_Click_1);
            // 
            // 资讯ToolStripMenuItem
            // 
            this.资讯ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNotice,
            this.tsmiReport,
            this.tsmiStockNews,
            this.tsmiSingleInfo});
            this.资讯ToolStripMenuItem.Name = "资讯ToolStripMenuItem";
            this.资讯ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.资讯ToolStripMenuItem.Text = "资讯";
            // 
            // tsmiNotice
            // 
            this.tsmiNotice.Name = "tsmiNotice";
            this.tsmiNotice.Size = new System.Drawing.Size(124, 22);
            this.tsmiNotice.Text = "公告";
            this.tsmiNotice.Click += new System.EventHandler(this.tsmiNotice_Click);
            // 
            // tsmiReport
            // 
            this.tsmiReport.Name = "tsmiReport";
            this.tsmiReport.Size = new System.Drawing.Size(124, 22);
            this.tsmiReport.Text = "研报";
            this.tsmiReport.Click += new System.EventHandler(this.tsmiReport_Click);
            // 
            // tsmiStockNews
            // 
            this.tsmiStockNews.Name = "tsmiStockNews";
            this.tsmiStockNews.Size = new System.Drawing.Size(124, 22);
            this.tsmiStockNews.Text = "股票新闻";
            this.tsmiStockNews.Click += new System.EventHandler(this.tsmiStockNews_Click);
            // 
            // tsmiSingleInfo
            // 
            this.tsmiSingleInfo.Name = "tsmiSingleInfo";
            this.tsmiSingleInfo.Size = new System.Drawing.Size(124, 22);
            this.tsmiSingleInfo.Text = "个股资讯";
            this.tsmiSingleInfo.Click += new System.EventHandler(this.tsmiSingleInfo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(920, 451);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据查询";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiData;
        private System.Windows.Forms.ToolStripMenuItem tsmiStockF9;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewStock;
        private System.Windows.Forms.ToolStripMenuItem 资讯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotice;
        private System.Windows.Forms.ToolStripMenuItem tsmiTradeInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiStockNews;
        private System.Windows.Forms.ToolStripMenuItem tsmiReport;
        private System.Windows.Forms.ToolStripMenuItem tsmiSingleInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiEcomomy;
        private System.Windows.Forms.ToolStripMenuItem tsmiFutureF9;
        private System.Windows.Forms.ToolStripMenuItem 大数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiStockBrower;
        private System.Windows.Forms.ToolStripMenuItem tsmiMacIndusty;
        private System.Windows.Forms.ToolStripMenuItem tsmiReportWatch;
        private System.Windows.Forms.ToolStripMenuItem 行情ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiQuote;
        private System.Windows.Forms.ToolStripMenuItem tsmiSecurities;
        private System.Windows.Forms.ToolStripMenuItem tsmiFormula;
        private System.Windows.Forms.ToolStripMenuItem tsmiOther;

    }
}

