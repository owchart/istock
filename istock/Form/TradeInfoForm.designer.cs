namespace dataquery
{
    partial class TradeInfoForm
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
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.mcCalendar = new System.Windows.Forms.MonthCalendar();
            this.btnQuery = new System.Windows.Forms.Button();
            this.rbBusinessDate = new System.Windows.Forms.RadioButton();
            this.rbShareChangeDate = new System.Windows.Forms.RadioButton();
            this.rbBigBuyDate = new System.Windows.Forms.RadioButton();
            this.rbPredictPriceDate = new System.Windows.Forms.RadioButton();
            this.rbFinancialShowDate = new System.Windows.Forms.RadioButton();
            this.rbHomeData = new System.Windows.Forms.RadioButton();
            this.rbIssueData = new System.Windows.Forms.RadioButton();
            this.rbLiftLimitDate = new System.Windows.Forms.RadioButton();
            this.rbDealOutDate = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rbPaymentData = new System.Windows.Forms.RadioButton();
            this.rbTransactionData = new System.Windows.Forms.RadioButton();
            this.rbRatingChangeData = new System.Windows.Forms.RadioButton();
            this.rbConvertibleBondsData = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbFundHomePage = new System.Windows.Forms.RadioButton();
            this.rbFundEnter = new System.Windows.Forms.RadioButton();
            this.rbFundShare = new System.Windows.Forms.RadioButton();
            this.rbFundIssue = new System.Windows.Forms.RadioButton();
            this.rbOtherNotice = new System.Windows.Forms.RadioButton();
            this.rbFundSplit = new System.Windows.Forms.RadioButton();
            this.rbIssueSale = new System.Windows.Forms.RadioButton();
            this.rbFundManager = new System.Windows.Forms.RadioButton();
            this.rbFundChange = new System.Windows.Forms.RadioButton();
            this.rbFundNameChange = new System.Windows.Forms.RadioButton();
            this.rbFinanceShow = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(6, 195);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(832, 196);
            this.dgvData.TabIndex = 2;
            // 
            // mcCalendar
            // 
            this.mcCalendar.Location = new System.Drawing.Point(6, 5);
            this.mcCalendar.Name = "mcCalendar";
            this.mcCalendar.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(238, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // rbBusinessDate
            // 
            this.rbBusinessDate.AutoSize = true;
            this.rbBusinessDate.Checked = true;
            this.rbBusinessDate.Location = new System.Drawing.Point(321, 19);
            this.rbBusinessDate.Name = "rbBusinessDate";
            this.rbBusinessDate.Size = new System.Drawing.Size(71, 16);
            this.rbBusinessDate.TabIndex = 5;
            this.rbBusinessDate.TabStop = true;
            this.rbBusinessDate.Text = "交易信息";
            this.rbBusinessDate.UseVisualStyleBackColor = true;
            // 
            // rbShareChangeDate
            // 
            this.rbShareChangeDate.AutoSize = true;
            this.rbShareChangeDate.Location = new System.Drawing.Point(398, 19);
            this.rbShareChangeDate.Name = "rbShareChangeDate";
            this.rbShareChangeDate.Size = new System.Drawing.Size(95, 16);
            this.rbShareChangeDate.TabIndex = 6;
            this.rbShareChangeDate.TabStop = true;
            this.rbShareChangeDate.Text = "持股变动信息";
            this.rbShareChangeDate.UseVisualStyleBackColor = true;
            // 
            // rbBigBuyDate
            // 
            this.rbBigBuyDate.AutoSize = true;
            this.rbBigBuyDate.Location = new System.Drawing.Point(499, 19);
            this.rbBigBuyDate.Name = "rbBigBuyDate";
            this.rbBigBuyDate.Size = new System.Drawing.Size(95, 16);
            this.rbBigBuyDate.TabIndex = 7;
            this.rbBigBuyDate.TabStop = true;
            this.rbBigBuyDate.Text = "大宗交易信息";
            this.rbBigBuyDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbBigBuyDate.UseVisualStyleBackColor = true;
            // 
            // rbPredictPriceDate
            // 
            this.rbPredictPriceDate.AutoSize = true;
            this.rbPredictPriceDate.Location = new System.Drawing.Point(600, 19);
            this.rbPredictPriceDate.Name = "rbPredictPriceDate";
            this.rbPredictPriceDate.Size = new System.Drawing.Size(95, 16);
            this.rbPredictPriceDate.TabIndex = 8;
            this.rbPredictPriceDate.TabStop = true;
            this.rbPredictPriceDate.Text = "业绩预测信息";
            this.rbPredictPriceDate.UseVisualStyleBackColor = true;
            // 
            // rbFinancialShowDate
            // 
            this.rbFinancialShowDate.AutoSize = true;
            this.rbFinancialShowDate.Location = new System.Drawing.Point(322, 41);
            this.rbFinancialShowDate.Name = "rbFinancialShowDate";
            this.rbFinancialShowDate.Size = new System.Drawing.Size(95, 16);
            this.rbFinancialShowDate.TabIndex = 9;
            this.rbFinancialShowDate.TabStop = true;
            this.rbFinancialShowDate.Text = "财报披露信息";
            this.rbFinancialShowDate.UseVisualStyleBackColor = true;
            // 
            // rbHomeData
            // 
            this.rbHomeData.AutoSize = true;
            this.rbHomeData.Location = new System.Drawing.Point(277, 67);
            this.rbHomeData.Name = "rbHomeData";
            this.rbHomeData.Size = new System.Drawing.Size(71, 16);
            this.rbHomeData.TabIndex = 0;
            this.rbHomeData.TabStop = true;
            this.rbHomeData.Text = "每日提醒";
            this.rbHomeData.UseVisualStyleBackColor = true;
            // 
            // rbIssueData
            // 
            this.rbIssueData.AutoSize = true;
            this.rbIssueData.Location = new System.Drawing.Point(354, 67);
            this.rbIssueData.Name = "rbIssueData";
            this.rbIssueData.Size = new System.Drawing.Size(95, 16);
            this.rbIssueData.TabIndex = 1;
            this.rbIssueData.TabStop = true;
            this.rbIssueData.Text = "新债发行数据";
            this.rbIssueData.UseVisualStyleBackColor = true;
            // 
            // rbLiftLimitDate
            // 
            this.rbLiftLimitDate.AutoSize = true;
            this.rbLiftLimitDate.Location = new System.Drawing.Point(422, 41);
            this.rbLiftLimitDate.Name = "rbLiftLimitDate";
            this.rbLiftLimitDate.Size = new System.Drawing.Size(71, 16);
            this.rbLiftLimitDate.TabIndex = 10;
            this.rbLiftLimitDate.TabStop = true;
            this.rbLiftLimitDate.Text = "股份流通";
            this.rbLiftLimitDate.UseVisualStyleBackColor = true;
            // 
            // rbDealOutDate
            // 
            this.rbDealOutDate.AutoSize = true;
            this.rbDealOutDate.Location = new System.Drawing.Point(499, 41);
            this.rbDealOutDate.Name = "rbDealOutDate";
            this.rbDealOutDate.Size = new System.Drawing.Size(47, 16);
            this.rbDealOutDate.TabIndex = 11;
            this.rbDealOutDate.TabStop = true;
            this.rbDealOutDate.Text = "分红";
            this.rbDealOutDate.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "债券";
            // 
            // rbPaymentData
            // 
            this.rbPaymentData.AutoSize = true;
            this.rbPaymentData.Location = new System.Drawing.Point(455, 67);
            this.rbPaymentData.Name = "rbPaymentData";
            this.rbPaymentData.Size = new System.Drawing.Size(95, 16);
            this.rbPaymentData.TabIndex = 15;
            this.rbPaymentData.TabStop = true;
            this.rbPaymentData.Text = "付息兑付数据";
            this.rbPaymentData.UseVisualStyleBackColor = true;
            // 
            // rbTransactionData
            // 
            this.rbTransactionData.AutoSize = true;
            this.rbTransactionData.Location = new System.Drawing.Point(556, 69);
            this.rbTransactionData.Name = "rbTransactionData";
            this.rbTransactionData.Size = new System.Drawing.Size(95, 16);
            this.rbTransactionData.TabIndex = 16;
            this.rbTransactionData.TabStop = true;
            this.rbTransactionData.Text = "交易结算数据";
            this.rbTransactionData.UseVisualStyleBackColor = true;
            // 
            // rbRatingChangeData
            // 
            this.rbRatingChangeData.AutoSize = true;
            this.rbRatingChangeData.Location = new System.Drawing.Point(657, 69);
            this.rbRatingChangeData.Name = "rbRatingChangeData";
            this.rbRatingChangeData.Size = new System.Drawing.Size(95, 16);
            this.rbRatingChangeData.TabIndex = 17;
            this.rbRatingChangeData.TabStop = true;
            this.rbRatingChangeData.Text = "评级变更数据";
            this.rbRatingChangeData.UseVisualStyleBackColor = true;
            // 
            // rbConvertibleBondsData
            // 
            this.rbConvertibleBondsData.AutoSize = true;
            this.rbConvertibleBondsData.Location = new System.Drawing.Point(277, 89);
            this.rbConvertibleBondsData.Name = "rbConvertibleBondsData";
            this.rbConvertibleBondsData.Size = new System.Drawing.Size(83, 16);
            this.rbConvertibleBondsData.TabIndex = 18;
            this.rbConvertibleBondsData.TabStop = true;
            this.rbConvertibleBondsData.Text = "可转债数据";
            this.rbConvertibleBondsData.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "基金";
            // 
            // rbFundHomePage
            // 
            this.rbFundHomePage.AutoSize = true;
            this.rbFundHomePage.Location = new System.Drawing.Point(277, 113);
            this.rbFundHomePage.Name = "rbFundHomePage";
            this.rbFundHomePage.Size = new System.Drawing.Size(119, 16);
            this.rbFundHomePage.TabIndex = 20;
            this.rbFundHomePage.TabStop = true;
            this.rbFundHomePage.Text = "开基和闭基的数据";
            this.rbFundHomePage.UseVisualStyleBackColor = true;
            // 
            // rbFundEnter
            // 
            this.rbFundEnter.AutoSize = true;
            this.rbFundEnter.Location = new System.Drawing.Point(402, 113);
            this.rbFundEnter.Name = "rbFundEnter";
            this.rbFundEnter.Size = new System.Drawing.Size(71, 16);
            this.rbFundEnter.TabIndex = 21;
            this.rbFundEnter.TabStop = true;
            this.rbFundEnter.Text = "基金上市";
            this.rbFundEnter.UseVisualStyleBackColor = true;
            // 
            // rbFundShare
            // 
            this.rbFundShare.AutoSize = true;
            this.rbFundShare.Location = new System.Drawing.Point(479, 113);
            this.rbFundShare.Name = "rbFundShare";
            this.rbFundShare.Size = new System.Drawing.Size(71, 16);
            this.rbFundShare.TabIndex = 22;
            this.rbFundShare.TabStop = true;
            this.rbFundShare.Text = "基金分红";
            this.rbFundShare.UseVisualStyleBackColor = true;
            // 
            // rbFundIssue
            // 
            this.rbFundIssue.AutoSize = true;
            this.rbFundIssue.Location = new System.Drawing.Point(556, 113);
            this.rbFundIssue.Name = "rbFundIssue";
            this.rbFundIssue.Size = new System.Drawing.Size(71, 16);
            this.rbFundIssue.TabIndex = 23;
            this.rbFundIssue.TabStop = true;
            this.rbFundIssue.Text = "基金发行";
            this.rbFundIssue.UseVisualStyleBackColor = true;
            // 
            // rbOtherNotice
            // 
            this.rbOtherNotice.AutoSize = true;
            this.rbOtherNotice.Location = new System.Drawing.Point(633, 113);
            this.rbOtherNotice.Name = "rbOtherNotice";
            this.rbOtherNotice.Size = new System.Drawing.Size(71, 16);
            this.rbOtherNotice.TabIndex = 24;
            this.rbOtherNotice.TabStop = true;
            this.rbOtherNotice.Text = "剩余信息";
            this.rbOtherNotice.UseVisualStyleBackColor = true;
            // 
            // rbFundSplit
            // 
            this.rbFundSplit.AutoSize = true;
            this.rbFundSplit.Location = new System.Drawing.Point(244, 135);
            this.rbFundSplit.Name = "rbFundSplit";
            this.rbFundSplit.Size = new System.Drawing.Size(71, 16);
            this.rbFundSplit.TabIndex = 25;
            this.rbFundSplit.TabStop = true;
            this.rbFundSplit.Text = "分红拆分";
            this.rbFundSplit.UseVisualStyleBackColor = true;
            // 
            // rbIssueSale
            // 
            this.rbIssueSale.AutoSize = true;
            this.rbIssueSale.Location = new System.Drawing.Point(322, 135);
            this.rbIssueSale.Name = "rbIssueSale";
            this.rbIssueSale.Size = new System.Drawing.Size(71, 16);
            this.rbIssueSale.TabIndex = 26;
            this.rbIssueSale.TabStop = true;
            this.rbIssueSale.Text = "发行上市";
            this.rbIssueSale.UseVisualStyleBackColor = true;
            // 
            // rbFundManager
            // 
            this.rbFundManager.AutoSize = true;
            this.rbFundManager.Location = new System.Drawing.Point(402, 135);
            this.rbFundManager.Name = "rbFundManager";
            this.rbFundManager.Size = new System.Drawing.Size(71, 16);
            this.rbFundManager.TabIndex = 27;
            this.rbFundManager.TabStop = true;
            this.rbFundManager.Text = "基金经理";
            this.rbFundManager.UseVisualStyleBackColor = true;
            // 
            // rbFundChange
            // 
            this.rbFundChange.AutoSize = true;
            this.rbFundChange.Location = new System.Drawing.Point(479, 135);
            this.rbFundChange.Name = "rbFundChange";
            this.rbFundChange.Size = new System.Drawing.Size(71, 16);
            this.rbFundChange.TabIndex = 28;
            this.rbFundChange.TabStop = true;
            this.rbFundChange.Text = "基金转型";
            this.rbFundChange.UseVisualStyleBackColor = true;
            // 
            // rbFundNameChange
            // 
            this.rbFundNameChange.AutoSize = true;
            this.rbFundNameChange.Location = new System.Drawing.Point(556, 135);
            this.rbFundNameChange.Name = "rbFundNameChange";
            this.rbFundNameChange.Size = new System.Drawing.Size(71, 16);
            this.rbFundNameChange.TabIndex = 29;
            this.rbFundNameChange.TabStop = true;
            this.rbFundNameChange.Text = "基金更名";
            this.rbFundNameChange.UseVisualStyleBackColor = true;
            // 
            // rbFinanceShow
            // 
            this.rbFinanceShow.AutoSize = true;
            this.rbFinanceShow.Location = new System.Drawing.Point(633, 135);
            this.rbFinanceShow.Name = "rbFinanceShow";
            this.rbFinanceShow.Size = new System.Drawing.Size(71, 16);
            this.rbFinanceShow.TabIndex = 30;
            this.rbFinanceShow.TabStop = true;
            this.rbFinanceShow.Text = "财报披露";
            this.rbFinanceShow.UseVisualStyleBackColor = true;
            // 
            // TradeInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 396);
            this.Controls.Add(this.rbFinanceShow);
            this.Controls.Add(this.rbFundNameChange);
            this.Controls.Add(this.rbFundChange);
            this.Controls.Add(this.rbFundManager);
            this.Controls.Add(this.rbIssueSale);
            this.Controls.Add(this.rbFundSplit);
            this.Controls.Add(this.rbOtherNotice);
            this.Controls.Add(this.rbFundIssue);
            this.Controls.Add(this.rbFundShare);
            this.Controls.Add(this.rbFundEnter);
            this.Controls.Add(this.rbFundHomePage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbConvertibleBondsData);
            this.Controls.Add(this.rbRatingChangeData);
            this.Controls.Add(this.rbTransactionData);
            this.Controls.Add(this.rbPaymentData);
            this.Controls.Add(this.rbIssueData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbHomeData);
            this.Controls.Add(this.rbDealOutDate);
            this.Controls.Add(this.rbBusinessDate);
            this.Controls.Add(this.rbLiftLimitDate);
            this.Controls.Add(this.rbShareChangeDate);
            this.Controls.Add(this.rbFinancialShowDate);
            this.Controls.Add(this.rbBigBuyDate);
            this.Controls.Add(this.rbPredictPriceDate);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.mcCalendar);
            this.Controls.Add(this.dgvData);
            this.Name = "TradeInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日期查询";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.MonthCalendar mcCalendar;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.RadioButton rbBusinessDate;
        private System.Windows.Forms.RadioButton rbShareChangeDate;
        private System.Windows.Forms.RadioButton rbBigBuyDate;
        private System.Windows.Forms.RadioButton rbPredictPriceDate;
        private System.Windows.Forms.RadioButton rbFinancialShowDate;
        private System.Windows.Forms.RadioButton rbIssueData;
        private System.Windows.Forms.RadioButton rbHomeData;
        private System.Windows.Forms.RadioButton rbLiftLimitDate;
        private System.Windows.Forms.RadioButton rbDealOutDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbPaymentData;
        private System.Windows.Forms.RadioButton rbTransactionData;
        private System.Windows.Forms.RadioButton rbRatingChangeData;
        private System.Windows.Forms.RadioButton rbConvertibleBondsData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbFundHomePage;
        private System.Windows.Forms.RadioButton rbFundEnter;
        private System.Windows.Forms.RadioButton rbFundShare;
        private System.Windows.Forms.RadioButton rbFundIssue;
        private System.Windows.Forms.RadioButton rbOtherNotice;
        private System.Windows.Forms.RadioButton rbFundSplit;
        private System.Windows.Forms.RadioButton rbIssueSale;
        private System.Windows.Forms.RadioButton rbFundManager;
        private System.Windows.Forms.RadioButton rbFundChange;
        private System.Windows.Forms.RadioButton rbFundNameChange;
        private System.Windows.Forms.RadioButton rbFinanceShow;
    }
}