namespace dataquery
{
    partial class QuoteForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuoteForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnOneDayHis = new System.Windows.Forms.Button();
            this.btnIndexDetail = new System.Windows.Forms.Button();
            this.btnShortLineStrategy = new System.Windows.Forms.Button();
            this.btnTrendLine = new System.Windows.Forms.Button();
            this.btnN = new System.Windows.Forms.Button();
            this.rtbText = new System.Windows.Forms.RichTextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnLevel2Details = new System.Windows.Forms.Button();
            this.btnN100 = new System.Windows.Forms.Button();
            this.btnCustomReport = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "股票代码:";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(77, 14);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 21);
            this.txtCode.TabIndex = 1;
            this.txtCode.Text = "002214.SZ";
            // 
            // btnOneDayHis
            // 
            this.btnOneDayHis.Location = new System.Drawing.Point(265, 12);
            this.btnOneDayHis.Name = "btnOneDayHis";
            this.btnOneDayHis.Size = new System.Drawing.Size(75, 23);
            this.btnOneDayHis.TabIndex = 2;
            this.btnOneDayHis.Text = "日线";
            this.btnOneDayHis.UseVisualStyleBackColor = true;
            this.btnOneDayHis.Click += new System.EventHandler(this.btnOneDayHis_Click);
            // 
            // btnIndexDetail
            // 
            this.btnIndexDetail.Location = new System.Drawing.Point(508, 12);
            this.btnIndexDetail.Name = "btnIndexDetail";
            this.btnIndexDetail.Size = new System.Drawing.Size(75, 23);
            this.btnIndexDetail.TabIndex = 4;
            this.btnIndexDetail.Text = "指数";
            this.btnIndexDetail.UseVisualStyleBackColor = true;
            this.btnIndexDetail.Click += new System.EventHandler(this.btnIndexDetail_Click);
            // 
            // btnShortLineStrategy
            // 
            this.btnShortLineStrategy.Location = new System.Drawing.Point(589, 12);
            this.btnShortLineStrategy.Name = "btnShortLineStrategy";
            this.btnShortLineStrategy.Size = new System.Drawing.Size(75, 23);
            this.btnShortLineStrategy.TabIndex = 5;
            this.btnShortLineStrategy.Text = "短线精灵";
            this.btnShortLineStrategy.UseVisualStyleBackColor = true;
            this.btnShortLineStrategy.Click += new System.EventHandler(this.btnShortLineStrategy_Click);
            // 
            // btnTrendLine
            // 
            this.btnTrendLine.Location = new System.Drawing.Point(184, 12);
            this.btnTrendLine.Name = "btnTrendLine";
            this.btnTrendLine.Size = new System.Drawing.Size(75, 23);
            this.btnTrendLine.TabIndex = 6;
            this.btnTrendLine.Text = "分时线";
            this.btnTrendLine.UseVisualStyleBackColor = true;
            this.btnTrendLine.Click += new System.EventHandler(this.btnTrendLine_Click);
            // 
            // btnN
            // 
            this.btnN.Location = new System.Drawing.Point(670, 12);
            this.btnN.Name = "btnN";
            this.btnN.Size = new System.Drawing.Size(75, 23);
            this.btnN.TabIndex = 7;
            this.btnN.Text = "N档行情";
            this.btnN.UseVisualStyleBackColor = true;
            this.btnN.Click += new System.EventHandler(this.btnN_Click);
            // 
            // rtbText
            // 
            this.rtbText.Dock = System.Windows.Forms.DockStyle.Left;
            this.rtbText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbText.Location = new System.Drawing.Point(0, 0);
            this.rtbText.Name = "rtbText";
            this.rtbText.Size = new System.Drawing.Size(321, 433);
            this.rtbText.TabIndex = 8;
            this.rtbText.Text = "";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnLevel2Details
            // 
            this.btnLevel2Details.Location = new System.Drawing.Point(346, 12);
            this.btnLevel2Details.Name = "btnLevel2Details";
            this.btnLevel2Details.Size = new System.Drawing.Size(75, 23);
            this.btnLevel2Details.TabIndex = 9;
            this.btnLevel2Details.Text = "LV2数据";
            this.btnLevel2Details.UseVisualStyleBackColor = true;
            this.btnLevel2Details.Click += new System.EventHandler(this.btnLv2Details_Click);
            // 
            // btnN100
            // 
            this.btnN100.Location = new System.Drawing.Point(751, 12);
            this.btnN100.Name = "btnN100";
            this.btnN100.Size = new System.Drawing.Size(75, 23);
            this.btnN100.TabIndex = 10;
            this.btnN100.Text = "百档行情";
            this.btnN100.UseVisualStyleBackColor = true;
            this.btnN100.Click += new System.EventHandler(this.btnN100_Click);
            // 
            // btnCustomReport
            // 
            this.btnCustomReport.Location = new System.Drawing.Point(427, 12);
            this.btnCustomReport.Name = "btnCustomReport";
            this.btnCustomReport.Size = new System.Drawing.Size(75, 23);
            this.btnCustomReport.TabIndex = 12;
            this.btnCustomReport.Text = "报价";
            this.btnCustomReport.UseVisualStyleBackColor = true;
            this.btnCustomReport.Click += new System.EventHandler(this.btnCustomReport_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.rtbText);
            this.panel1.Location = new System.Drawing.Point(1, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(849, 433);
            this.panel1.TabIndex = 13;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(321, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 433);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // QuoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 476);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCustomReport);
            this.Controls.Add(this.btnN100);
            this.Controls.Add(this.btnLevel2Details);
            this.Controls.Add(this.btnN);
            this.Controls.Add(this.btnTrendLine);
            this.Controls.Add(this.btnShortLineStrategy);
            this.Controls.Add(this.btnIndexDetail);
            this.Controls.Add(this.btnOneDayHis);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label1);
            this.Name = "QuoteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "东方财富通行情";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.QuoteForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnOneDayHis;
        private System.Windows.Forms.Button btnIndexDetail;
        private System.Windows.Forms.Button btnShortLineStrategy;
        private System.Windows.Forms.Button btnTrendLine;
        private System.Windows.Forms.Button btnN;
        private System.Windows.Forms.RichTextBox rtbText;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnLevel2Details;
        private System.Windows.Forms.Button btnN100;
        private System.Windows.Forms.Button btnCustomReport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;

    }
}