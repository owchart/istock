namespace dataquery
{
    partial class StockF9Form
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.rbCompanyIntroductionInfo = new System.Windows.Forms.RadioButton();
            this.rbCompanyNameHistoryList = new System.Windows.Forms.RadioButton();
            this.rbStockStructureList = new System.Windows.Forms.RadioButton();
            this.rbTop10HolderList = new System.Windows.Forms.RadioButton();
            this.rbInstitutionInvestorList = new System.Windows.Forms.RadioButton();
            this.rbStockHolderNumberList = new System.Windows.Forms.RadioButton();
            this.rbStockUnlimitedTimeList = new System.Windows.Forms.RadioButton();
            this.rbTop10HolderList2 = new System.Windows.Forms.RadioButton();
            this.rbManagerInfo = new System.Windows.Forms.RadioButton();
            this.rbManagementRemuneration = new System.Windows.Forms.RadioButton();
            this.rbManagementStockChange = new System.Windows.Forms.RadioButton();
            this.rbStockMarketExpressList = new System.Windows.Forms.RadioButton();
            this.rbSecurityIntroduction = new System.Windows.Forms.RadioButton();
            this.rbSpecialProcessDelistedOpen = new System.Windows.Forms.RadioButton();
            this.rbIndustryInfo = new System.Windows.Forms.RadioButton();
            this.rbSecurityIndex = new System.Windows.Forms.RadioButton();
            this.rbConceptBoardList = new System.Windows.Forms.RadioButton();
            this.rbStageMarketDataList = new System.Windows.Forms.RadioButton();
            this.rbBargainOfficeList = new System.Windows.Forms.RadioButton();
            this.rbFundFlowList = new System.Windows.Forms.RadioButton();
            this.rbSecurityFinancingList = new System.Windows.Forms.RadioButton();
            this.rbOperationAbility = new System.Windows.Forms.RadioButton();
            this.rbProfitAbility = new System.Windows.Forms.RadioButton();
            this.rbCapitalStructure = new System.Windows.Forms.RadioButton();
            this.rbSingleFinanceIndex = new System.Windows.Forms.RadioButton();
            this.rbTransferDebt = new System.Windows.Forms.RadioButton();
            this.rbFinanceDetails = new System.Windows.Forms.RadioButton();
            this.rbGuaranteeCash = new System.Windows.Forms.RadioButton();
            this.rbStockDetails = new System.Windows.Forms.RadioButton();
            this.rbBargainExchange = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
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
            this.dgvData.Location = new System.Drawing.Point(4, 167);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(796, 191);
            this.dgvData.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "股票代码:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(78, 7);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 21);
            this.txtCode.TabIndex = 5;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(184, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // rbCompanyIntroductionInfo
            // 
            this.rbCompanyIntroductionInfo.AutoSize = true;
            this.rbCompanyIntroductionInfo.Checked = true;
            this.rbCompanyIntroductionInfo.Location = new System.Drawing.Point(15, 34);
            this.rbCompanyIntroductionInfo.Name = "rbCompanyIntroductionInfo";
            this.rbCompanyIntroductionInfo.Size = new System.Drawing.Size(71, 16);
            this.rbCompanyIntroductionInfo.TabIndex = 7;
            this.rbCompanyIntroductionInfo.TabStop = true;
            this.rbCompanyIntroductionInfo.Text = "公司介绍";
            this.rbCompanyIntroductionInfo.UseVisualStyleBackColor = true;
            // 
            // rbCompanyNameHistoryList
            // 
            this.rbCompanyNameHistoryList.AutoSize = true;
            this.rbCompanyNameHistoryList.Location = new System.Drawing.Point(92, 34);
            this.rbCompanyNameHistoryList.Name = "rbCompanyNameHistoryList";
            this.rbCompanyNameHistoryList.Size = new System.Drawing.Size(83, 16);
            this.rbCompanyNameHistoryList.TabIndex = 8;
            this.rbCompanyNameHistoryList.TabStop = true;
            this.rbCompanyNameHistoryList.Text = "曾用历史名";
            this.rbCompanyNameHistoryList.UseVisualStyleBackColor = true;
            // 
            // rbStockStructureList
            // 
            this.rbStockStructureList.AutoSize = true;
            this.rbStockStructureList.Location = new System.Drawing.Point(188, 34);
            this.rbStockStructureList.Name = "rbStockStructureList";
            this.rbStockStructureList.Size = new System.Drawing.Size(71, 16);
            this.rbStockStructureList.TabIndex = 9;
            this.rbStockStructureList.TabStop = true;
            this.rbStockStructureList.Text = "股本结构";
            this.rbStockStructureList.UseVisualStyleBackColor = true;
            // 
            // rbTop10HolderList
            // 
            this.rbTop10HolderList.AutoSize = true;
            this.rbTop10HolderList.Location = new System.Drawing.Point(265, 34);
            this.rbTop10HolderList.Name = "rbTop10HolderList";
            this.rbTop10HolderList.Size = new System.Drawing.Size(95, 16);
            this.rbTop10HolderList.TabIndex = 10;
            this.rbTop10HolderList.TabStop = true;
            this.rbTop10HolderList.Text = "十大股东明细";
            this.rbTop10HolderList.UseVisualStyleBackColor = true;
            // 
            // rbInstitutionInvestorList
            // 
            this.rbInstitutionInvestorList.AutoSize = true;
            this.rbInstitutionInvestorList.Location = new System.Drawing.Point(479, 34);
            this.rbInstitutionInvestorList.Name = "rbInstitutionInvestorList";
            this.rbInstitutionInvestorList.Size = new System.Drawing.Size(83, 16);
            this.rbInstitutionInvestorList.TabIndex = 11;
            this.rbInstitutionInvestorList.TabStop = true;
            this.rbInstitutionInvestorList.Text = "机构投资者";
            this.rbInstitutionInvestorList.UseVisualStyleBackColor = true;
            // 
            // rbStockHolderNumberList
            // 
            this.rbStockHolderNumberList.AutoSize = true;
            this.rbStockHolderNumberList.Location = new System.Drawing.Point(568, 34);
            this.rbStockHolderNumberList.Name = "rbStockHolderNumberList";
            this.rbStockHolderNumberList.Size = new System.Drawing.Size(71, 16);
            this.rbStockHolderNumberList.TabIndex = 13;
            this.rbStockHolderNumberList.TabStop = true;
            this.rbStockHolderNumberList.Text = "股东户数";
            this.rbStockHolderNumberList.UseVisualStyleBackColor = true;
            // 
            // rbStockUnlimitedTimeList
            // 
            this.rbStockUnlimitedTimeList.AutoSize = true;
            this.rbStockUnlimitedTimeList.Location = new System.Drawing.Point(645, 34);
            this.rbStockUnlimitedTimeList.Name = "rbStockUnlimitedTimeList";
            this.rbStockUnlimitedTimeList.Size = new System.Drawing.Size(119, 16);
            this.rbStockUnlimitedTimeList.TabIndex = 14;
            this.rbStockUnlimitedTimeList.TabStop = true;
            this.rbStockUnlimitedTimeList.Text = "限售股解禁时间表";
            this.rbStockUnlimitedTimeList.UseVisualStyleBackColor = true;
            // 
            // rbTop10HolderList2
            // 
            this.rbTop10HolderList2.AutoSize = true;
            this.rbTop10HolderList2.Location = new System.Drawing.Point(366, 34);
            this.rbTop10HolderList2.Name = "rbTop10HolderList2";
            this.rbTop10HolderList2.Size = new System.Drawing.Size(107, 16);
            this.rbTop10HolderList2.TabIndex = 15;
            this.rbTop10HolderList2.TabStop = true;
            this.rbTop10HolderList2.Text = "十大流通股明细";
            this.rbTop10HolderList2.UseVisualStyleBackColor = true;
            // 
            // rbManagerInfo
            // 
            this.rbManagerInfo.AutoSize = true;
            this.rbManagerInfo.Location = new System.Drawing.Point(15, 56);
            this.rbManagerInfo.Name = "rbManagerInfo";
            this.rbManagerInfo.Size = new System.Drawing.Size(47, 16);
            this.rbManagerInfo.TabIndex = 16;
            this.rbManagerInfo.TabStop = true;
            this.rbManagerInfo.Text = "高管";
            this.rbManagerInfo.UseVisualStyleBackColor = true;
            // 
            // rbManagementRemuneration
            // 
            this.rbManagementRemuneration.AutoSize = true;
            this.rbManagementRemuneration.Location = new System.Drawing.Point(68, 56);
            this.rbManagementRemuneration.Name = "rbManagementRemuneration";
            this.rbManagementRemuneration.Size = new System.Drawing.Size(119, 16);
            this.rbManagementRemuneration.TabIndex = 17;
            this.rbManagementRemuneration.TabStop = true;
            this.rbManagementRemuneration.Text = "管理层持股及报酬";
            this.rbManagementRemuneration.UseVisualStyleBackColor = true;
            // 
            // rbManagementStockChange
            // 
            this.rbManagementStockChange.AutoSize = true;
            this.rbManagementStockChange.Location = new System.Drawing.Point(193, 56);
            this.rbManagementStockChange.Name = "rbManagementStockChange";
            this.rbManagementStockChange.Size = new System.Drawing.Size(107, 16);
            this.rbManagementStockChange.TabIndex = 18;
            this.rbManagementStockChange.TabStop = true;
            this.rbManagementStockChange.Text = "管理层持股变化";
            this.rbManagementStockChange.UseVisualStyleBackColor = true;
            // 
            // rbStockMarketExpressList
            // 
            this.rbStockMarketExpressList.AutoSize = true;
            this.rbStockMarketExpressList.Location = new System.Drawing.Point(306, 56);
            this.rbStockMarketExpressList.Name = "rbStockMarketExpressList";
            this.rbStockMarketExpressList.Size = new System.Drawing.Size(95, 16);
            this.rbStockMarketExpressList.TabIndex = 19;
            this.rbStockMarketExpressList.TabStop = true;
            this.rbStockMarketExpressList.Text = "市场表现比较";
            this.rbStockMarketExpressList.UseVisualStyleBackColor = true;
            // 
            // rbSecurityIntroduction
            // 
            this.rbSecurityIntroduction.AutoSize = true;
            this.rbSecurityIntroduction.Location = new System.Drawing.Point(407, 56);
            this.rbSecurityIntroduction.Name = "rbSecurityIntroduction";
            this.rbSecurityIntroduction.Size = new System.Drawing.Size(71, 16);
            this.rbSecurityIntroduction.TabIndex = 20;
            this.rbSecurityIntroduction.TabStop = true;
            this.rbSecurityIntroduction.Text = "证券简介";
            this.rbSecurityIntroduction.UseVisualStyleBackColor = true;
            // 
            // rbSpecialProcessDelistedOpen
            // 
            this.rbSpecialProcessDelistedOpen.AutoSize = true;
            this.rbSpecialProcessDelistedOpen.Location = new System.Drawing.Point(479, 56);
            this.rbSpecialProcessDelistedOpen.Name = "rbSpecialProcessDelistedOpen";
            this.rbSpecialProcessDelistedOpen.Size = new System.Drawing.Size(107, 16);
            this.rbSpecialProcessDelistedOpen.TabIndex = 21;
            this.rbSpecialProcessDelistedOpen.TabStop = true;
            this.rbSpecialProcessDelistedOpen.Text = "特别处理和退市";
            this.rbSpecialProcessDelistedOpen.UseVisualStyleBackColor = true;
            // 
            // rbIndustryInfo
            // 
            this.rbIndustryInfo.AutoSize = true;
            this.rbIndustryInfo.Location = new System.Drawing.Point(592, 56);
            this.rbIndustryInfo.Name = "rbIndustryInfo";
            this.rbIndustryInfo.Size = new System.Drawing.Size(71, 16);
            this.rbIndustryInfo.TabIndex = 22;
            this.rbIndustryInfo.TabStop = true;
            this.rbIndustryInfo.Text = "所属行业";
            this.rbIndustryInfo.UseVisualStyleBackColor = true;
            // 
            // rbSecurityIndex
            // 
            this.rbSecurityIndex.AutoSize = true;
            this.rbSecurityIndex.Location = new System.Drawing.Point(669, 56);
            this.rbSecurityIndex.Name = "rbSecurityIndex";
            this.rbSecurityIndex.Size = new System.Drawing.Size(95, 16);
            this.rbSecurityIndex.TabIndex = 23;
            this.rbSecurityIndex.TabStop = true;
            this.rbSecurityIndex.Text = "证券所属指数";
            this.rbSecurityIndex.UseVisualStyleBackColor = true;
            // 
            // rbConceptBoardList
            // 
            this.rbConceptBoardList.AutoSize = true;
            this.rbConceptBoardList.Location = new System.Drawing.Point(15, 78);
            this.rbConceptBoardList.Name = "rbConceptBoardList";
            this.rbConceptBoardList.Size = new System.Drawing.Size(95, 16);
            this.rbConceptBoardList.TabIndex = 24;
            this.rbConceptBoardList.TabStop = true;
            this.rbConceptBoardList.Text = "所属概念板块";
            this.rbConceptBoardList.UseVisualStyleBackColor = true;
            // 
            // rbStageMarketDataList
            // 
            this.rbStageMarketDataList.AutoSize = true;
            this.rbStageMarketDataList.Location = new System.Drawing.Point(116, 78);
            this.rbStageMarketDataList.Name = "rbStageMarketDataList";
            this.rbStageMarketDataList.Size = new System.Drawing.Size(119, 16);
            this.rbStageMarketDataList.TabIndex = 26;
            this.rbStageMarketDataList.TabStop = true;
            this.rbStageMarketDataList.Text = "阶段行情数据统计";
            this.rbStageMarketDataList.UseVisualStyleBackColor = true;
            // 
            // rbBargainOfficeList
            // 
            this.rbBargainOfficeList.AutoSize = true;
            this.rbBargainOfficeList.Location = new System.Drawing.Point(241, 78);
            this.rbBargainOfficeList.Name = "rbBargainOfficeList";
            this.rbBargainOfficeList.Size = new System.Drawing.Size(131, 16);
            this.rbBargainOfficeList.TabIndex = 27;
            this.rbBargainOfficeList.TabStop = true;
            this.rbBargainOfficeList.Text = "交易异动成交营业部";
            this.rbBargainOfficeList.UseVisualStyleBackColor = true;
            // 
            // rbFundFlowList
            // 
            this.rbFundFlowList.AutoSize = true;
            this.rbFundFlowList.Location = new System.Drawing.Point(383, 78);
            this.rbFundFlowList.Name = "rbFundFlowList";
            this.rbFundFlowList.Size = new System.Drawing.Size(71, 16);
            this.rbFundFlowList.TabIndex = 28;
            this.rbFundFlowList.TabStop = true;
            this.rbFundFlowList.Text = "资金流向";
            this.rbFundFlowList.UseVisualStyleBackColor = true;
            // 
            // rbSecurityFinancingList
            // 
            this.rbSecurityFinancingList.AutoSize = true;
            this.rbSecurityFinancingList.Location = new System.Drawing.Point(460, 78);
            this.rbSecurityFinancingList.Name = "rbSecurityFinancingList";
            this.rbSecurityFinancingList.Size = new System.Drawing.Size(71, 16);
            this.rbSecurityFinancingList.TabIndex = 29;
            this.rbSecurityFinancingList.TabStop = true;
            this.rbSecurityFinancingList.Text = "融资融券";
            this.rbSecurityFinancingList.UseVisualStyleBackColor = true;
            // 
            // rbOperationAbility
            // 
            this.rbOperationAbility.AutoSize = true;
            this.rbOperationAbility.Location = new System.Drawing.Point(537, 78);
            this.rbOperationAbility.Name = "rbOperationAbility";
            this.rbOperationAbility.Size = new System.Drawing.Size(71, 16);
            this.rbOperationAbility.TabIndex = 30;
            this.rbOperationAbility.TabStop = true;
            this.rbOperationAbility.Text = "运营能力";
            this.rbOperationAbility.UseVisualStyleBackColor = true;
            // 
            // rbProfitAbility
            // 
            this.rbProfitAbility.AutoSize = true;
            this.rbProfitAbility.Location = new System.Drawing.Point(614, 78);
            this.rbProfitAbility.Name = "rbProfitAbility";
            this.rbProfitAbility.Size = new System.Drawing.Size(131, 16);
            this.rbProfitAbility.TabIndex = 31;
            this.rbProfitAbility.TabStop = true;
            this.rbProfitAbility.Text = "盈利能力与收益质量";
            this.rbProfitAbility.UseVisualStyleBackColor = true;
            // 
            // rbCapitalStructure
            // 
            this.rbCapitalStructure.AutoSize = true;
            this.rbCapitalStructure.Location = new System.Drawing.Point(15, 100);
            this.rbCapitalStructure.Name = "rbCapitalStructure";
            this.rbCapitalStructure.Size = new System.Drawing.Size(131, 16);
            this.rbCapitalStructure.TabIndex = 32;
            this.rbCapitalStructure.TabStop = true;
            this.rbCapitalStructure.Text = "资本结构与偿债能力";
            this.rbCapitalStructure.UseVisualStyleBackColor = true;
            // 
            // rbSingleFinanceIndex
            // 
            this.rbSingleFinanceIndex.AutoSize = true;
            this.rbSingleFinanceIndex.Location = new System.Drawing.Point(152, 100);
            this.rbSingleFinanceIndex.Name = "rbSingleFinanceIndex";
            this.rbSingleFinanceIndex.Size = new System.Drawing.Size(107, 16);
            this.rbSingleFinanceIndex.TabIndex = 33;
            this.rbSingleFinanceIndex.TabStop = true;
            this.rbSingleFinanceIndex.Text = "单季度财务指标";
            this.rbSingleFinanceIndex.UseVisualStyleBackColor = true;
            // 
            // rbTransferDebt
            // 
            this.rbTransferDebt.AutoSize = true;
            this.rbTransferDebt.Location = new System.Drawing.Point(258, 100);
            this.rbTransferDebt.Name = "rbTransferDebt";
            this.rbTransferDebt.Size = new System.Drawing.Size(215, 16);
            this.rbTransferDebt.TabIndex = 34;
            this.rbTransferDebt.TabStop = true;
            this.rbTransferDebt.Text = "新股发行 发行可转债 募集资金投向";
            this.rbTransferDebt.UseVisualStyleBackColor = true;
            // 
            // rbFinanceDetails
            // 
            this.rbFinanceDetails.AutoSize = true;
            this.rbFinanceDetails.Location = new System.Drawing.Point(479, 100);
            this.rbFinanceDetails.Name = "rbFinanceDetails";
            this.rbFinanceDetails.Size = new System.Drawing.Size(95, 16);
            this.rbFinanceDetails.TabIndex = 35;
            this.rbFinanceDetails.TabStop = true;
            this.rbFinanceDetails.Text = "财务费用明细";
            this.rbFinanceDetails.UseVisualStyleBackColor = true;
            // 
            // rbGuaranteeCash
            // 
            this.rbGuaranteeCash.AutoSize = true;
            this.rbGuaranteeCash.Location = new System.Drawing.Point(580, 100);
            this.rbGuaranteeCash.Name = "rbGuaranteeCash";
            this.rbGuaranteeCash.Size = new System.Drawing.Size(71, 16);
            this.rbGuaranteeCash.TabIndex = 36;
            this.rbGuaranteeCash.TabStop = true;
            this.rbGuaranteeCash.Text = "担保金额";
            this.rbGuaranteeCash.UseVisualStyleBackColor = true;
            // 
            // rbStockDetails
            // 
            this.rbStockDetails.AutoSize = true;
            this.rbStockDetails.Location = new System.Drawing.Point(657, 100);
            this.rbStockDetails.Name = "rbStockDetails";
            this.rbStockDetails.Size = new System.Drawing.Size(71, 16);
            this.rbStockDetails.TabIndex = 37;
            this.rbStockDetails.TabStop = true;
            this.rbStockDetails.Text = "存货明细";
            this.rbStockDetails.UseVisualStyleBackColor = true;
            // 
            // rbBargainExchange
            // 
            this.rbBargainExchange.AutoSize = true;
            this.rbBargainExchange.Location = new System.Drawing.Point(15, 122);
            this.rbBargainExchange.Name = "rbBargainExchange";
            this.rbBargainExchange.Size = new System.Drawing.Size(131, 16);
            this.rbBargainExchange.TabIndex = 38;
            this.rbBargainExchange.TabStop = true;
            this.rbBargainExchange.Text = "交易异动成交营业部";
            this.rbBargainExchange.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 39;
            this.label2.Text = "代码不要加后缀";
            // 
            // StockF9Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 361);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbBargainExchange);
            this.Controls.Add(this.rbStockDetails);
            this.Controls.Add(this.rbGuaranteeCash);
            this.Controls.Add(this.rbFinanceDetails);
            this.Controls.Add(this.rbTransferDebt);
            this.Controls.Add(this.rbSingleFinanceIndex);
            this.Controls.Add(this.rbCapitalStructure);
            this.Controls.Add(this.rbProfitAbility);
            this.Controls.Add(this.rbOperationAbility);
            this.Controls.Add(this.rbSecurityFinancingList);
            this.Controls.Add(this.rbFundFlowList);
            this.Controls.Add(this.rbBargainOfficeList);
            this.Controls.Add(this.rbStageMarketDataList);
            this.Controls.Add(this.rbConceptBoardList);
            this.Controls.Add(this.rbSecurityIndex);
            this.Controls.Add(this.rbIndustryInfo);
            this.Controls.Add(this.rbSpecialProcessDelistedOpen);
            this.Controls.Add(this.rbSecurityIntroduction);
            this.Controls.Add(this.rbStockMarketExpressList);
            this.Controls.Add(this.rbManagementStockChange);
            this.Controls.Add(this.rbManagementRemuneration);
            this.Controls.Add(this.rbManagerInfo);
            this.Controls.Add(this.rbTop10HolderList2);
            this.Controls.Add(this.rbStockUnlimitedTimeList);
            this.Controls.Add(this.rbStockHolderNumberList);
            this.Controls.Add(this.rbInstitutionInvestorList);
            this.Controls.Add(this.rbTop10HolderList);
            this.Controls.Add(this.rbStockStructureList);
            this.Controls.Add(this.rbCompanyNameHistoryList);
            this.Controls.Add(this.rbCompanyIntroductionInfo);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvData);
            this.Name = "StockF9Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "股票深度资料";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.RadioButton rbCompanyIntroductionInfo;
        private System.Windows.Forms.RadioButton rbCompanyNameHistoryList;
        private System.Windows.Forms.RadioButton rbStockStructureList;
        private System.Windows.Forms.RadioButton rbTop10HolderList;
        private System.Windows.Forms.RadioButton rbInstitutionInvestorList;
        private System.Windows.Forms.RadioButton rbStockHolderNumberList;
        private System.Windows.Forms.RadioButton rbStockUnlimitedTimeList;
        private System.Windows.Forms.RadioButton rbTop10HolderList2;
        private System.Windows.Forms.RadioButton rbManagerInfo;
        private System.Windows.Forms.RadioButton rbManagementRemuneration;
        private System.Windows.Forms.RadioButton rbManagementStockChange;
        private System.Windows.Forms.RadioButton rbStockMarketExpressList;
        private System.Windows.Forms.RadioButton rbSecurityIntroduction;
        private System.Windows.Forms.RadioButton rbSpecialProcessDelistedOpen;
        private System.Windows.Forms.RadioButton rbIndustryInfo;
        private System.Windows.Forms.RadioButton rbSecurityIndex;
        private System.Windows.Forms.RadioButton rbConceptBoardList;
        private System.Windows.Forms.RadioButton rbStageMarketDataList;
        private System.Windows.Forms.RadioButton rbBargainOfficeList;
        private System.Windows.Forms.RadioButton rbFundFlowList;
        private System.Windows.Forms.RadioButton rbSecurityFinancingList;
        private System.Windows.Forms.RadioButton rbOperationAbility;
        private System.Windows.Forms.RadioButton rbProfitAbility;
        private System.Windows.Forms.RadioButton rbCapitalStructure;
        private System.Windows.Forms.RadioButton rbSingleFinanceIndex;
        private System.Windows.Forms.RadioButton rbTransferDebt;
        private System.Windows.Forms.RadioButton rbFinanceDetails;
        private System.Windows.Forms.RadioButton rbGuaranteeCash;
        private System.Windows.Forms.RadioButton rbStockDetails;
        private System.Windows.Forms.RadioButton rbBargainExchange;
        private System.Windows.Forms.Label label2;
    }
}