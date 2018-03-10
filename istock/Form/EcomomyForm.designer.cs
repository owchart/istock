namespace dataquery
{
    partial class EcomomyForm
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
            this.btnQuery = new System.Windows.Forms.Button();
            this.rbCountInArea = new System.Windows.Forms.RadioButton();
            this.rbBondNewMarket = new System.Windows.Forms.RadioButton();
            this.rbAreaNewMarket = new System.Windows.Forms.RadioButton();
            this.rbDepartNewMarket = new System.Windows.Forms.RadioButton();
            this.rbBondCityList = new System.Windows.Forms.RadioButton();
            this.rbProvinceList = new System.Windows.Forms.RadioButton();
            this.rbCityList = new System.Windows.Forms.RadioButton();
            this.rbBondCompanyInput = new System.Windows.Forms.RadioButton();
            this.rbOpenAcount = new System.Windows.Forms.RadioButton();
            this.rbAStockAge = new System.Windows.Forms.RadioButton();
            this.rbAStockMoneyAll = new System.Windows.Forms.RadioButton();
            this.rbOpenArea = new System.Windows.Forms.RadioButton();
            this.rbBondMsgList = new System.Windows.Forms.RadioButton();
            this.rbBondCompanyDetail = new System.Windows.Forms.RadioButton();
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
            this.dgvData.Location = new System.Drawing.Point(4, 101);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(796, 257);
            this.dgvData.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(15, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // rbCountInArea
            // 
            this.rbCountInArea.AutoSize = true;
            this.rbCountInArea.Checked = true;
            this.rbCountInArea.Location = new System.Drawing.Point(15, 34);
            this.rbCountInArea.Name = "rbCountInArea";
            this.rbCountInArea.Size = new System.Drawing.Size(119, 16);
            this.rbCountInArea.TabIndex = 7;
            this.rbCountInArea.TabStop = true;
            this.rbCountInArea.Text = "券商和营业部集合";
            this.rbCountInArea.UseVisualStyleBackColor = true;
            // 
            // rbBondNewMarket
            // 
            this.rbBondNewMarket.AutoSize = true;
            this.rbBondNewMarket.Location = new System.Drawing.Point(140, 34);
            this.rbBondNewMarket.Name = "rbBondNewMarket";
            this.rbBondNewMarket.Size = new System.Drawing.Size(155, 16);
            this.rbBondNewMarket.TabIndex = 8;
            this.rbBondNewMarket.TabStop = true;
            this.rbBondNewMarket.Text = "券商统计月度最新的市场";
            this.rbBondNewMarket.UseVisualStyleBackColor = true;
            // 
            // rbAreaNewMarket
            // 
            this.rbAreaNewMarket.AutoSize = true;
            this.rbAreaNewMarket.Location = new System.Drawing.Point(301, 34);
            this.rbAreaNewMarket.Name = "rbAreaNewMarket";
            this.rbAreaNewMarket.Size = new System.Drawing.Size(155, 16);
            this.rbAreaNewMarket.TabIndex = 9;
            this.rbAreaNewMarket.TabStop = true;
            this.rbAreaNewMarket.Text = "地区统计月度最新的市场";
            this.rbAreaNewMarket.UseVisualStyleBackColor = true;
            // 
            // rbDepartNewMarket
            // 
            this.rbDepartNewMarket.AutoSize = true;
            this.rbDepartNewMarket.Location = new System.Drawing.Point(462, 34);
            this.rbDepartNewMarket.Name = "rbDepartNewMarket";
            this.rbDepartNewMarket.Size = new System.Drawing.Size(167, 16);
            this.rbDepartNewMarket.TabIndex = 10;
            this.rbDepartNewMarket.TabStop = true;
            this.rbDepartNewMarket.Text = "营业部统计月度最新的市场";
            this.rbDepartNewMarket.UseVisualStyleBackColor = true;
            // 
            // rbBondCityList
            // 
            this.rbBondCityList.AutoSize = true;
            this.rbBondCityList.Location = new System.Drawing.Point(635, 34);
            this.rbBondCityList.Name = "rbBondCityList";
            this.rbBondCityList.Size = new System.Drawing.Size(107, 16);
            this.rbBondCityList.TabIndex = 11;
            this.rbBondCityList.TabStop = true;
            this.rbBondCityList.Text = "证券公司城市  ";
            this.rbBondCityList.UseVisualStyleBackColor = true;
            // 
            // rbProvinceList
            // 
            this.rbProvinceList.AutoSize = true;
            this.rbProvinceList.Location = new System.Drawing.Point(15, 56);
            this.rbProvinceList.Name = "rbProvinceList";
            this.rbProvinceList.Size = new System.Drawing.Size(47, 16);
            this.rbProvinceList.TabIndex = 12;
            this.rbProvinceList.TabStop = true;
            this.rbProvinceList.Text = "省份";
            this.rbProvinceList.UseVisualStyleBackColor = true;
            // 
            // rbCityList
            // 
            this.rbCityList.AutoSize = true;
            this.rbCityList.Location = new System.Drawing.Point(68, 56);
            this.rbCityList.Name = "rbCityList";
            this.rbCityList.Size = new System.Drawing.Size(47, 16);
            this.rbCityList.TabIndex = 13;
            this.rbCityList.TabStop = true;
            this.rbCityList.Text = "城市";
            this.rbCityList.UseVisualStyleBackColor = true;
            // 
            // rbBondCompanyInput
            // 
            this.rbBondCompanyInput.AutoSize = true;
            this.rbBondCompanyInput.Location = new System.Drawing.Point(121, 56);
            this.rbBondCompanyInput.Name = "rbBondCompanyInput";
            this.rbBondCompanyInput.Size = new System.Drawing.Size(71, 16);
            this.rbBondCompanyInput.TabIndex = 14;
            this.rbBondCompanyInput.TabStop = true;
            this.rbBondCompanyInput.Text = "证券公司";
            this.rbBondCompanyInput.UseVisualStyleBackColor = true;
            // 
            // rbOpenAcount
            // 
            this.rbOpenAcount.AutoSize = true;
            this.rbOpenAcount.Location = new System.Drawing.Point(200, 56);
            this.rbOpenAcount.Name = "rbOpenAcount";
            this.rbOpenAcount.Size = new System.Drawing.Size(143, 16);
            this.rbOpenAcount.TabIndex = 15;
            this.rbOpenAcount.TabStop = true;
            this.rbOpenAcount.Text = "证券账户变动本期分析";
            this.rbOpenAcount.UseVisualStyleBackColor = true;
            // 
            // rbAStockAge
            // 
            this.rbAStockAge.AutoSize = true;
            this.rbAStockAge.Location = new System.Drawing.Point(349, 56);
            this.rbAStockAge.Name = "rbAStockAge";
            this.rbAStockAge.Size = new System.Drawing.Size(89, 16);
            this.rbAStockAge.TabIndex = 16;
            this.rbAStockAge.TabStop = true;
            this.rbAStockAge.Text = "A股年龄分布";
            this.rbAStockAge.UseVisualStyleBackColor = true;
            // 
            // rbAStockMoneyAll
            // 
            this.rbAStockMoneyAll.AutoSize = true;
            this.rbAStockMoneyAll.Location = new System.Drawing.Point(444, 56);
            this.rbAStockMoneyAll.Name = "rbAStockMoneyAll";
            this.rbAStockMoneyAll.Size = new System.Drawing.Size(83, 16);
            this.rbAStockMoneyAll.TabIndex = 17;
            this.rbAStockMoneyAll.TabStop = true;
            this.rbAStockMoneyAll.Text = "股市值分布";
            this.rbAStockMoneyAll.UseVisualStyleBackColor = true;
            // 
            // rbOpenArea
            // 
            this.rbOpenArea.AutoSize = true;
            this.rbOpenArea.Location = new System.Drawing.Point(534, 56);
            this.rbOpenArea.Name = "rbOpenArea";
            this.rbOpenArea.Size = new System.Drawing.Size(113, 16);
            this.rbOpenArea.TabIndex = 18;
            this.rbOpenArea.TabStop = true;
            this.rbOpenArea.Text = "A股开户地区分布";
            this.rbOpenArea.UseVisualStyleBackColor = true;
            // 
            // rbBondMsgList
            // 
            this.rbBondMsgList.AutoSize = true;
            this.rbBondMsgList.Location = new System.Drawing.Point(15, 79);
            this.rbBondMsgList.Name = "rbBondMsgList";
            this.rbBondMsgList.Size = new System.Drawing.Size(95, 16);
            this.rbBondMsgList.TabIndex = 19;
            this.rbBondMsgList.TabStop = true;
            this.rbBondMsgList.Text = "证券公司大全";
            this.rbBondMsgList.UseVisualStyleBackColor = true;
            // 
            // rbBondCompanyDetail
            // 
            this.rbBondCompanyDetail.AutoSize = true;
            this.rbBondCompanyDetail.Location = new System.Drawing.Point(116, 79);
            this.rbBondCompanyDetail.Name = "rbBondCompanyDetail";
            this.rbBondCompanyDetail.Size = new System.Drawing.Size(71, 16);
            this.rbBondCompanyDetail.TabIndex = 20;
            this.rbBondCompanyDetail.TabStop = true;
            this.rbBondCompanyDetail.Text = "公司详情";
            this.rbBondCompanyDetail.UseVisualStyleBackColor = true;
            // 
            // EcomomyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 361);
            this.Controls.Add(this.rbBondCompanyDetail);
            this.Controls.Add(this.rbBondMsgList);
            this.Controls.Add(this.rbOpenArea);
            this.Controls.Add(this.rbAStockMoneyAll);
            this.Controls.Add(this.rbAStockAge);
            this.Controls.Add(this.rbOpenAcount);
            this.Controls.Add(this.rbBondCompanyInput);
            this.Controls.Add(this.rbCityList);
            this.Controls.Add(this.rbProvinceList);
            this.Controls.Add(this.rbBondCityList);
            this.Controls.Add(this.rbDepartNewMarket);
            this.Controls.Add(this.rbAreaNewMarket);
            this.Controls.Add(this.rbBondNewMarket);
            this.Controls.Add(this.rbCountInArea);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dgvData);
            this.Name = "EcomomyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "经济业务大全";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.RadioButton rbCountInArea;
        private System.Windows.Forms.RadioButton rbBondNewMarket;
        private System.Windows.Forms.RadioButton rbAreaNewMarket;
        private System.Windows.Forms.RadioButton rbDepartNewMarket;
        private System.Windows.Forms.RadioButton rbBondCityList;
        private System.Windows.Forms.RadioButton rbProvinceList;
        private System.Windows.Forms.RadioButton rbCityList;
        private System.Windows.Forms.RadioButton rbBondCompanyInput;
        private System.Windows.Forms.RadioButton rbOpenAcount;
        private System.Windows.Forms.RadioButton rbAStockAge;
        private System.Windows.Forms.RadioButton rbAStockMoneyAll;
        private System.Windows.Forms.RadioButton rbOpenArea;
        private System.Windows.Forms.RadioButton rbBondMsgList;
        private System.Windows.Forms.RadioButton rbBondCompanyDetail;
    }
}