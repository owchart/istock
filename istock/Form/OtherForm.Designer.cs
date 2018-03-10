namespace dataquery
{
    partial class OtherForm
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
            this.btnUserSecurity = new System.Windows.Forms.Button();
            this.btnQuoteSequenc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUserSecurity
            // 
            this.btnUserSecurity.Location = new System.Drawing.Point(12, 12);
            this.btnUserSecurity.Name = "btnUserSecurity";
            this.btnUserSecurity.Size = new System.Drawing.Size(74, 23);
            this.btnUserSecurity.TabIndex = 0;
            this.btnUserSecurity.Text = "自选股";
            this.btnUserSecurity.UseVisualStyleBackColor = true;
            this.btnUserSecurity.Click += new System.EventHandler(this.btnUserSecurity_Click);
            // 
            // btnQuoteSequenc
            // 
            this.btnQuoteSequenc.Location = new System.Drawing.Point(12, 50);
            this.btnQuoteSequenc.Name = "btnQuoteSequenc";
            this.btnQuoteSequenc.Size = new System.Drawing.Size(75, 23);
            this.btnQuoteSequenc.TabIndex = 1;
            this.btnQuoteSequenc.Text = "行情序列";
            this.btnQuoteSequenc.UseVisualStyleBackColor = true;
            this.btnQuoteSequenc.Click += new System.EventHandler(this.btnQuoteSequenc_Click);
            // 
            // OtherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 279);
            this.Controls.Add(this.btnQuoteSequenc);
            this.Controls.Add(this.btnUserSecurity);
            this.Name = "OtherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "其他";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUserSecurity;
        private System.Windows.Forms.Button btnQuoteSequenc;
    }
}