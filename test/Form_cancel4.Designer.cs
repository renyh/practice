namespace practice
{
    partial class Form_cancel4
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
            this.button_stop1 = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // button_stop1
            // 
            this.button_stop1.Location = new System.Drawing.Point(169, 13);
            this.button_stop1.Name = "button_stop1";
            this.button_stop1.Size = new System.Drawing.Size(96, 30);
            this.button_stop1.TabIndex = 4;
            this.button_stop1.Text = "停止";
            this.button_stop1.UseVisualStyleBackColor = true;
            this.button_stop1.Click += new System.EventHandler(this.button_stop1_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(26, 12);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(116, 31);
            this.button_start.TabIndex = 3;
            this.button_start.Text = "开始线程";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(26, 61);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(631, 468);
            this.webBrowser1.TabIndex = 6;
            // 
            // Form_cancel4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 541);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button_stop1);
            this.Controls.Add(this.button_start);
            this.Name = "Form_cancel4";
            this.Text = "浏览器控件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_cancel1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_stop1;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}