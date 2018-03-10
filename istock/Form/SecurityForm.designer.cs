namespace dataquery
{
    partial class SecurityForm
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
            this.btnStart = new System.Windows.Forms.Button();
            this.dgvSecurities = new System.Windows.Forms.DataGridView();
            this.btnAsyncToMongo = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSecurities)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(76, 26);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "查看";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dgvSecurities
            // 
            this.dgvSecurities.AllowUserToAddRows = false;
            this.dgvSecurities.AllowUserToDeleteRows = false;
            this.dgvSecurities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSecurities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSecurities.Location = new System.Drawing.Point(12, 53);
            this.dgvSecurities.Name = "dgvSecurities";
            this.dgvSecurities.RowHeadersVisible = false;
            this.dgvSecurities.RowTemplate.Height = 23;
            this.dgvSecurities.Size = new System.Drawing.Size(661, 357);
            this.dgvSecurities.TabIndex = 1;
            // 
            // btnAsyncToMongo
            // 
            this.btnAsyncToMongo.Location = new System.Drawing.Point(108, 15);
            this.btnAsyncToMongo.Name = "btnAsyncToMongo";
            this.btnAsyncToMongo.Size = new System.Drawing.Size(122, 23);
            this.btnAsyncToMongo.TabIndex = 2;
            this.btnAsyncToMongo.Text = "同步到Mongo";
            this.btnAsyncToMongo.UseVisualStyleBackColor = true;
            this.btnAsyncToMongo.Click += new System.EventHandler(this.btnAsyncToMongo_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(246, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(346, 21);
            this.txtSearch.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(598, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // SecurityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 422);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnAsyncToMongo);
            this.Controls.Add(this.dgvSecurities);
            this.Controls.Add(this.btnStart);
            this.Name = "SecurityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "码表浏览器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvSecurities)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataGridView dgvSecurities;
        private System.Windows.Forms.Button btnAsyncToMongo;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnQuery;

    }
}