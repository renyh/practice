using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice
{
    public partial class Form_cancel1 : Form
    {
        public Form_cancel1()
        {
            InitializeComponent();
        }


        // 名字以用途命名即可。TokenSource 这种类型名称可以不出现在名字中
        CancellationTokenSource _cancel = new CancellationTokenSource();

        private void button_start_Click(object sender, EventArgs e)
        {
            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();
            this.textBox_info.Text = "";

            Task.Run(() =>
            {
                doSomething(this._cancel.Token);
            });
        }

        // 做事
        void doSomething(CancellationToken token)
        {
            // 设置按钮状态
            this.Invoke((Action)(() =>
                EnableControls(false)
                ));
            try
            {
                int i = 0;
                while (token.IsCancellationRequested == false)
                {
                    /*
                    // 中断也可以用
                    token.ThrowIfCancellationRequested();
                    */

                    i++;

                    // 界面显示信息
                    this.Invoke((Action)(() =>
                    {
                        this.textBox_info.Text = this.textBox_info.Text + i.ToString() + "\r\n";

                        // 没起作用
                        this.textBox_info.ScrollToCaret();
                    }
                    ));
                }
            }
            finally
            {
                // 设置按置状态
                this.Invoke((Action)(() =>
                    EnableControls(true)
                    ));
            }
        }

        // 设置控件是否可用
        void EnableControls(bool bEnable)
        {
            this.button_start.Enabled = bEnable;
            this.button_stop.Enabled = !(bEnable);
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            // 停止
            this._cancel.Cancel();
        }

        private void Form_cancel1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 窗口关闭前让循环退出
            this._cancel.Cancel();

            // 不再使用时，调用Dispose
            this._cancel.Dispose();
        }
    }
}
