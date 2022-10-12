namespace practice
{
    partial class Form_cancel3
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
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_stop1 = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_info.Location = new System.Drawing.Point(27, 64);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_info.Size = new System.Drawing.Size(320, 374);
            this.textBox_info.TabIndex = 5;
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
            // Form_cancel3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.button_stop1);
            this.Controls.Add(this.button_start);
            this.Name = "Form_cancel3";
            this.Text = "等待两个线程";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_cancel1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Button button_stop1;
        private System.Windows.Forms.Button button_start;
    }
}