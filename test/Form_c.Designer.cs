namespace practice.test
{
    partial class Form_c
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
            this.button_Cancel1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button_cancel2 = new System.Windows.Forms.Button();
            this.button_cancel4 = new System.Windows.Forms.Button();
            this.button_cancel3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Cancel1
            // 
            this.button_Cancel1.Location = new System.Drawing.Point(32, 23);
            this.button_Cancel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Cancel1.Name = "button_Cancel1";
            this.button_Cancel1.Size = new System.Drawing.Size(372, 36);
            this.button_Cancel1.TabIndex = 6;
            this.button_Cancel1.Text = "1个CancellationToken停止1个线程";
            this.button_Cancel1.UseVisualStyleBackColor = true;
            this.button_Cancel1.Click += new System.EventHandler(this.button_Cancel1_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(32, 339);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(372, 42);
            this.button5.TabIndex = 10;
            this.button5.Text = "回调函数 与 事件";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button_cancel2
            // 
            this.button_cancel2.Location = new System.Drawing.Point(32, 84);
            this.button_cancel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_cancel2.Name = "button_cancel2";
            this.button_cancel2.Size = new System.Drawing.Size(372, 50);
            this.button_cancel2.TabIndex = 7;
            this.button_cancel2.Text = "多个CancellationToken停止1个线程";
            this.button_cancel2.UseVisualStyleBackColor = true;
            this.button_cancel2.Click += new System.EventHandler(this.button_cancel2_Click);
            // 
            // button_cancel4
            // 
            this.button_cancel4.Location = new System.Drawing.Point(32, 219);
            this.button_cancel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_cancel4.Name = "button_cancel4";
            this.button_cancel4.Size = new System.Drawing.Size(392, 92);
            this.button_cancel4.TabIndex = 9;
            this.button_cancel4.Text = "等待两个线程-控制按钮状态写在包裹的线程里\r\n线程函数返回一个对象\r\n浏览器控件";
            this.button_cancel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_cancel4.UseVisualStyleBackColor = true;
            this.button_cancel4.Click += new System.EventHandler(this.button_cancel4_Click);
            // 
            // button_cancel3
            // 
            this.button_cancel3.Location = new System.Drawing.Point(32, 157);
            this.button_cancel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_cancel3.Name = "button_cancel3";
            this.button_cancel3.Size = new System.Drawing.Size(372, 42);
            this.button_cancel3.TabIndex = 8;
            this.button_cancel3.Text = "等待两个线程";
            this.button_cancel3.UseVisualStyleBackColor = true;
            this.button_cancel3.Click += new System.EventHandler(this.button_cancel3_Click);
            // 
            // Form_c
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 498);
            this.Controls.Add(this.button_Cancel1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button_cancel2);
            this.Controls.Add(this.button_cancel4);
            this.Controls.Add(this.button_cancel3);
            this.Name = "Form_c";
            this.Text = "Form_c";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button_cancel2;
        private System.Windows.Forms.Button button_cancel4;
        private System.Windows.Forms.Button button_cancel3;
    }
}