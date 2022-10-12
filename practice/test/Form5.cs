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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        // 用于停止线程工作
        CancellationTokenSource _cancel = new CancellationTokenSource();

        // 一个工作类
        Worker1 _worker1 = new Worker1();


        // 显示信息，工作线程会调这个函数做事
        public void ShowInfo(string text)
        {
            this.Invoke((Action)(() =>
            {
                this.textBox_info.Text = this.textBox_info.Text +text + "\r\n";
            }));
        }

        private async void button_start_Click(object sender, EventArgs e)
        {
            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();

            // 每次开始重new一下worker吗？要不要释放？目前这个类比较简单，没有内部对象
            this._worker1 = new Worker1();

            this.textBox_info.Text = "";

            EnableControls(false);
            try
            {
                Task <Item> t = Task.Run(() =>
                {
                    return this._worker1.doSomething(this._cancel.Token,"*",this.ShowInfo);//传了一个函数指针
                });

                await t;

                this.Invoke((Action)(() =>
                {
                    MessageBox.Show(this, "count =[" + t.Result.count.ToString() + "], text =[" + t.Result.text + "]" );
                }));
            }
            finally
            {
                EnableControls(true);
            }
        }



        // 设置控件是否可用
        void EnableControls(bool bEnable)
        {
            this.button_start.Enabled = bEnable;
            this.button_event.Enabled = bEnable;
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



        // 一个工作类
        Worker2 _worker2 = new Worker2();

        private async void button_event_Click(object sender, EventArgs e)
        {
            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();

            // 每次开始重new一下worker吗？
            this._worker2 = new Worker2();
            this._worker2.ShowInfoHandler += new ShowInfoDelegate(this.ShowInfo);

            this.textBox_info.Text = "";

            EnableControls(false);
            try
            {
                Task<Item> t = Task.Run(() =>
                {
                    return this._worker2.doSomething(this._cancel.Token,"-");
                });

                await t;

                this.Invoke((Action)(() =>
                {
                    MessageBox.Show(this, "count =[" + t.Result.count.ToString() + "], text =[" + t.Result.text + "]");
                }));
            }
            finally
            {
                EnableControls(true);
            }
        }
    }


}
