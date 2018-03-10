namespace dataquery
{
    partial class FutureF9Form
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
            this.rbFuturesBaseInfo = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbContractBrief = new System.Windows.Forms.RadioButton();
            this.rbAllMemberAgencies = new System.Windows.Forms.RadioButton();
            this.rbPositionStructJson = new System.Windows.Forms.RadioButton();
            this.rbPartyModel = new System.Windows.Forms.RadioButton();
            this.rbPartyInfoModel = new System.Windows.Forms.RadioButton();
            this.rbShibor = new System.Windows.Forms.RadioButton();
            this.rbRateBank = new System.Windows.Forms.RadioButton();
            this.rbPositionPriceJson = new System.Windows.Forms.RadioButton();
            this.rbProfitAnalysisJson = new System.Windows.Forms.RadioButton();
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
            this.dgvData.Location = new System.Drawing.Point(4, 87);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(796, 271);
            this.dgvData.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "期货代码:";
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
            // rbFuturesBaseInfo
            // 
            this.rbFuturesBaseInfo.AutoSize = true;
            this.rbFuturesBaseInfo.Checked = true;
            this.rbFuturesBaseInfo.Location = new System.Drawing.Point(15, 34);
            this.rbFuturesBaseInfo.Name = "rbFuturesBaseInfo";
            this.rbFuturesBaseInfo.Size = new System.Drawing.Size(95, 16);
            this.rbFuturesBaseInfo.TabIndex = 7;
            this.rbFuturesBaseInfo.TabStop = true;
            this.rbFuturesBaseInfo.Text = "基础期货信息";
            this.rbFuturesBaseInfo.UseVisualStyleBackColor = true;
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
            // rbContractBrief
            // 
            this.rbContractBrief.AutoSize = true;
            this.rbContractBrief.Location = new System.Drawing.Point(116, 34);
            this.rbContractBrief.Name = "rbContractBrief";
            this.rbContractBrief.Size = new System.Drawing.Size(71, 16);
            this.rbContractBrief.TabIndex = 40;
            this.rbContractBrief.TabStop = true;
            this.rbContractBrief.Text = "合约简介";
            this.rbContractBrief.UseVisualStyleBackColor = true;
            // 
            // rbAllMemberAgencies
            // 
            this.rbAllMemberAgencies.AutoSize = true;
            this.rbAllMemberAgencies.Location = new System.Drawing.Point(193, 35);
            this.rbAllMemberAgencies.Name = "rbAllMemberAgencies";
            this.rbAllMemberAgencies.Size = new System.Drawing.Size(71, 16);
            this.rbAllMemberAgencies.TabIndex = 41;
            this.rbAllMemberAgencies.TabStop = true;
            this.rbAllMemberAgencies.Text = "会员机构";
            this.rbAllMemberAgencies.UseVisualStyleBackColor = true;
            // 
            // rbPositionStructJson
            // 
            this.rbPositionStructJson.AutoSize = true;
            this.rbPositionStructJson.Location = new System.Drawing.Point(270, 34);
            this.rbPositionStructJson.Name = "rbPositionStructJson";
            this.rbPositionStructJson.Size = new System.Drawing.Size(71, 16);
            this.rbPositionStructJson.TabIndex = 42;
            this.rbPositionStructJson.TabStop = true;
            this.rbPositionStructJson.Text = "持仓结构";
            this.rbPositionStructJson.UseVisualStyleBackColor = true;
            // 
            // rbPartyModel
            // 
            this.rbPartyModel.AutoSize = true;
            this.rbPartyModel.Location = new System.Drawing.Point(347, 34);
            this.rbPartyModel.Name = "rbPartyModel";
            this.rbPartyModel.Size = new System.Drawing.Size(71, 16);
            this.rbPartyModel.TabIndex = 43;
            this.rbPartyModel.TabStop = true;
            this.rbPartyModel.Text = "会员列表";
            this.rbPartyModel.UseVisualStyleBackColor = true;
            // 
            // rbPartyInfoModel
            // 
            this.rbPartyInfoModel.AutoSize = true;
            this.rbPartyInfoModel.Location = new System.Drawing.Point(424, 34);
            this.rbPartyInfoModel.Name = "rbPartyInfoModel";
            this.rbPartyInfoModel.Size = new System.Drawing.Size(71, 16);
            this.rbPartyInfoModel.TabIndex = 44;
            this.rbPartyInfoModel.TabStop = true;
            this.rbPartyInfoModel.Text = "会员简介";
            this.rbPartyInfoModel.UseVisualStyleBackColor = true;
            // 
            // rbShibor
            // 
            this.rbShibor.AutoSize = true;
            this.rbShibor.Location = new System.Drawing.Point(501, 35);
            this.rbShibor.Name = "rbShibor";
            this.rbShibor.Size = new System.Drawing.Size(59, 16);
            this.rbShibor.TabIndex = 45;
            this.rbShibor.TabStop = true;
            this.rbShibor.Text = "Shibor";
            this.rbShibor.UseVisualStyleBackColor = true;
            // 
            // rbRateBank
            // 
            this.rbRateBank.AutoSize = true;
            this.rbRateBank.Location = new System.Drawing.Point(566, 35);
            this.rbRateBank.Name = "rbRateBank";
            this.rbRateBank.Size = new System.Drawing.Size(167, 16);
            this.rbRateBank.TabIndex = 46;
            this.rbRateBank.TabStop = true;
            this.rbRateBank.Text = "银行间质押式回购加权利率";
            this.rbRateBank.UseVisualStyleBackColor = true;
            // 
            // rbPositionPriceJson
            // 
            this.rbPositionPriceJson.AutoSize = true;
            this.rbPositionPriceJson.Location = new System.Drawing.Point(15, 65);
            this.rbPositionPriceJson.Name = "rbPositionPriceJson";
            this.rbPositionPriceJson.Size = new System.Drawing.Size(71, 16);
            this.rbPositionPriceJson.TabIndex = 47;
            this.rbPositionPriceJson.TabStop = true;
            this.rbPositionPriceJson.Text = "持仓均价";
            this.rbPositionPriceJson.UseVisualStyleBackColor = true;
            // 
            // rbProfitAnalysisJson
            // 
            this.rbProfitAnalysisJson.AutoSize = true;
            this.rbProfitAnalysisJson.Location = new System.Drawing.Point(92, 65);
            this.rbProfitAnalysisJson.Name = "rbProfitAnalysisJson";
            this.rbProfitAnalysisJson.Size = new System.Drawing.Size(71, 16);
            this.rbProfitAnalysisJson.TabIndex = 48;
            this.rbProfitAnalysisJson.TabStop = true;
            this.rbProfitAnalysisJson.Text = "盈亏分析";
            this.rbProfitAnalysisJson.UseVisualStyleBackColor = true;
            // 
            // FutureF9Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 361);
            this.Controls.Add(this.rbProfitAnalysisJson);
            this.Controls.Add(this.rbPositionPriceJson);
            this.Controls.Add(this.rbRateBank);
            this.Controls.Add(this.rbShibor);
            this.Controls.Add(this.rbPartyInfoModel);
            this.Controls.Add(this.rbPartyModel);
            this.Controls.Add(this.rbPositionStructJson);
            this.Controls.Add(this.rbAllMemberAgencies);
            this.Controls.Add(this.rbContractBrief);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbFuturesBaseInfo);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvData);
            this.Name = "FutureF9Form";
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
        private System.Windows.Forms.RadioButton rbFuturesBaseInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbContractBrief;
        private System.Windows.Forms.RadioButton rbAllMemberAgencies;
        private System.Windows.Forms.RadioButton rbPositionStructJson;
        private System.Windows.Forms.RadioButton rbPartyModel;
        private System.Windows.Forms.RadioButton rbPartyInfoModel;
        private System.Windows.Forms.RadioButton rbShibor;
        private System.Windows.Forms.RadioButton rbRateBank;
        private System.Windows.Forms.RadioButton rbPositionPriceJson;
        private System.Windows.Forms.RadioButton rbProfitAnalysisJson;
    }
}