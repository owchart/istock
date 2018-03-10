namespace dataquery
{
    partial class NewStockForm
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
            this.rbNewStockName = new System.Windows.Forms.RadioButton();
            this.rbNewStockCalendar = new System.Windows.Forms.RadioButton();
            this.rbStockIssueMsg = new System.Windows.Forms.RadioButton();
            this.rbStockPredictMsg = new System.Windows.Forms.RadioButton();
            this.btnQuery = new System.Windows.Forms.Button();
            this.rbStockIPOPage = new System.Windows.Forms.RadioButton();
            this.rbStockIPODataStatistics = new System.Windows.Forms.RadioButton();
            this.rbStockIPOHistoryStatistics = new System.Windows.Forms.RadioButton();
            this.rbStockIPOMarketExpress = new System.Windows.Forms.RadioButton();
            this.rbStockIPOZQRate = new System.Windows.Forms.RadioButton();
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
            this.dgvData.Location = new System.Drawing.Point(3, 78);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(615, 181);
            this.dgvData.TabIndex = 4;
            // 
            // rbNewStockName
            // 
            this.rbNewStockName.AutoSize = true;
            this.rbNewStockName.Checked = true;
            this.rbNewStockName.Location = new System.Drawing.Point(14, 34);
            this.rbNewStockName.Name = "rbNewStockName";
            this.rbNewStockName.Size = new System.Drawing.Size(71, 16);
            this.rbNewStockName.TabIndex = 8;
            this.rbNewStockName.TabStop = true;
            this.rbNewStockName.Text = "新股名称";
            this.rbNewStockName.UseVisualStyleBackColor = true;
            // 
            // rbNewStockCalendar
            // 
            this.rbNewStockCalendar.AutoSize = true;
            this.rbNewStockCalendar.Location = new System.Drawing.Point(91, 34);
            this.rbNewStockCalendar.Name = "rbNewStockCalendar";
            this.rbNewStockCalendar.Size = new System.Drawing.Size(71, 16);
            this.rbNewStockCalendar.TabIndex = 9;
            this.rbNewStockCalendar.TabStop = true;
            this.rbNewStockCalendar.Text = "新股日历";
            this.rbNewStockCalendar.UseVisualStyleBackColor = true;
            // 
            // rbStockIssueMsg
            // 
            this.rbStockIssueMsg.AutoSize = true;
            this.rbStockIssueMsg.Location = new System.Drawing.Point(168, 34);
            this.rbStockIssueMsg.Name = "rbStockIssueMsg";
            this.rbStockIssueMsg.Size = new System.Drawing.Size(131, 16);
            this.rbStockIssueMsg.TabIndex = 10;
            this.rbStockIssueMsg.TabStop = true;
            this.rbStockIssueMsg.Text = "新股发行与上市信息";
            this.rbStockIssueMsg.UseVisualStyleBackColor = true;
            // 
            // rbStockPredictMsg
            // 
            this.rbStockPredictMsg.AutoSize = true;
            this.rbStockPredictMsg.Location = new System.Drawing.Point(305, 34);
            this.rbStockPredictMsg.Name = "rbStockPredictMsg";
            this.rbStockPredictMsg.Size = new System.Drawing.Size(95, 16);
            this.rbStockPredictMsg.TabIndex = 11;
            this.rbStockPredictMsg.TabStop = true;
            this.rbStockPredictMsg.Text = "新股定价预测";
            this.rbStockPredictMsg.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(14, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // rbStockIPOPage
            // 
            this.rbStockIPOPage.AutoSize = true;
            this.rbStockIPOPage.Location = new System.Drawing.Point(406, 34);
            this.rbStockIPOPage.Name = "rbStockIPOPage";
            this.rbStockIPOPage.Size = new System.Drawing.Size(71, 16);
            this.rbStockIPOPage.TabIndex = 12;
            this.rbStockIPOPage.TabStop = true;
            this.rbStockIPOPage.Text = "新股改革";
            this.rbStockIPOPage.UseVisualStyleBackColor = true;
            // 
            // rbStockIPODataStatistics
            // 
            this.rbStockIPODataStatistics.AutoSize = true;
            this.rbStockIPODataStatistics.Location = new System.Drawing.Point(14, 57);
            this.rbStockIPODataStatistics.Name = "rbStockIPODataStatistics";
            this.rbStockIPODataStatistics.Size = new System.Drawing.Size(173, 16);
            this.rbStockIPODataStatistics.TabIndex = 13;
            this.rbStockIPODataStatistics.TabStop = true;
            this.rbStockIPODataStatistics.Text = "股权分置改革后IPO数据统计";
            this.rbStockIPODataStatistics.UseVisualStyleBackColor = true;
            // 
            // rbStockIPOHistoryStatistics
            // 
            this.rbStockIPOHistoryStatistics.AutoSize = true;
            this.rbStockIPOHistoryStatistics.Location = new System.Drawing.Point(193, 57);
            this.rbStockIPOHistoryStatistics.Name = "rbStockIPOHistoryStatistics";
            this.rbStockIPOHistoryStatistics.Size = new System.Drawing.Size(113, 16);
            this.rbStockIPOHistoryStatistics.TabIndex = 14;
            this.rbStockIPOHistoryStatistics.TabStop = true;
            this.rbStockIPOHistoryStatistics.Text = "历史IPO暂停重启";
            this.rbStockIPOHistoryStatistics.UseVisualStyleBackColor = true;
            // 
            // rbStockIPOMarketExpress
            // 
            this.rbStockIPOMarketExpress.AutoSize = true;
            this.rbStockIPOMarketExpress.Location = new System.Drawing.Point(313, 56);
            this.rbStockIPOMarketExpress.Name = "rbStockIPOMarketExpress";
            this.rbStockIPOMarketExpress.Size = new System.Drawing.Size(143, 16);
            this.rbStockIPOMarketExpress.TabIndex = 15;
            this.rbStockIPOMarketExpress.TabStop = true;
            this.rbStockIPOMarketExpress.Text = "历次改革市场指数表现";
            this.rbStockIPOMarketExpress.UseVisualStyleBackColor = true;
            // 
            // rbStockIPOZQRate
            // 
            this.rbStockIPOZQRate.AutoSize = true;
            this.rbStockIPOZQRate.Location = new System.Drawing.Point(483, 34);
            this.rbStockIPOZQRate.Name = "rbStockIPOZQRate";
            this.rbStockIPOZQRate.Size = new System.Drawing.Size(59, 16);
            this.rbStockIPOZQRate.TabIndex = 16;
            this.rbStockIPOZQRate.TabStop = true;
            this.rbStockIPOZQRate.Text = "中签率";
            this.rbStockIPOZQRate.UseVisualStyleBackColor = true;
            // 
            // NewStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 261);
            this.Controls.Add(this.rbStockIPOZQRate);
            this.Controls.Add(this.rbStockIPOMarketExpress);
            this.Controls.Add(this.rbStockIPOHistoryStatistics);
            this.Controls.Add(this.rbStockIPODataStatistics);
            this.Controls.Add(this.rbStockIPOPage);
            this.Controls.Add(this.rbStockPredictMsg);
            this.Controls.Add(this.rbStockIssueMsg);
            this.Controls.Add(this.rbNewStockCalendar);
            this.Controls.Add(this.rbNewStockName);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dgvData);
            this.Name = "NewStockForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新股";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.RadioButton rbNewStockName;
        private System.Windows.Forms.RadioButton rbNewStockCalendar;
        private System.Windows.Forms.RadioButton rbStockIssueMsg;
        private System.Windows.Forms.RadioButton rbStockPredictMsg;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.RadioButton rbStockIPOPage;
        private System.Windows.Forms.RadioButton rbStockIPODataStatistics;
        private System.Windows.Forms.RadioButton rbStockIPOHistoryStatistics;
        private System.Windows.Forms.RadioButton rbStockIPOMarketExpress;
        private System.Windows.Forms.RadioButton rbStockIPOZQRate;
    }
}