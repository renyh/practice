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
    public partial class Form_cancel3 : Form
    {
        public Form_cancel3()
        {
            InitializeComponent();
        }

        // 名字以用途命名即可。TokenSource 这种类型名称可以不出现在名字中
        CancellationTokenSource _cancel = new CancellationTokenSource();


        private async void button_start_Click(object sender, EventArgs e)
        {
            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();
            this.textBox_info.Text = "";

            // 设置按钮状态
            EnableControls(false);
            try
            {
                /*
                // 第一种写法，匿名函数。代码较少还可以。代码多了最好写入一个单独的函数
                await Task.Run(() =>
                {
                    Task t1 = Task.Run(() =>
                    {
                        doSomething(this._cancel.Token, "*");
                    });

                    Task t2 = Task.Run(() =>
                    {
                        doSomething(this._cancel.Token, "-");
                    });

                    Task.WaitAll(new Task[] { t1, t2 });
                });
                */

                /*
                // 第二种写法
                // 代码写入一个具名函数
                int count = await RunTwoTask();
                this.Invoke((Action)(() =>
                {
                    MessageBox.Show(this, $"count={count}");
                }));
                */


                
                // 第三种写法
                // 用 Task.Run() 调用一个平凡函数
                int count = await Task.Run(() => {
                    return RunTwo();
                });
                // 保险起见，这里还是用invoke在界面显示 信息
                this.Invoke((Action)(() =>
                {
                    MessageBox.Show(this, $"count={count}");
                }));
                 
            }
            finally
            {
                EnableControls(true);
            }
        }

        // 专门函数，明确用一个单独的线程来运行
        Task<int> RunTwoTask()
        {
            return Task.Run(() =>
            {
                Task<int> t1 = Task.Run(() =>
                {
                    return doSomething(this._cancel.Token, "*");
                });

                Task<int> t2 = Task.Run(() =>
                {
                    return doSomething(this._cancel.Token, "-");
                });

                Task.WaitAll(new Task[] { t1, t2 });

                return t1.Result + t2.Result;
            });
        }

        // 平凡的函数，要根据调主的意愿才能决定到底运行在什么线程
        int RunTwo()
        {
            Task<int> t1 = Task.Run(() =>
            {
                return doSomething(this._cancel.Token, "*");
            });

            Task<int> t2 = Task.Run(() =>
            {
                return doSomething(this._cancel.Token, "-");
            });

            Task.WaitAll(new Task[] { t1, t2 });

            return t1.Result + t2.Result;
        }

        // 做事
        int doSomething(CancellationToken token, string preprefix)
        {
            int i = 0;
            while (token.IsCancellationRequested == false)
            {
                // 没有这个语句，界面会冻结
                Thread.Sleep(100);

                i++;

                // 界面显示信息
                this.Invoke((Action)(() =>
                {
                    this.textBox_info.Text = this.textBox_info.Text + preprefix + i.ToString() + "\r\n";
                }));
            }

            return i;
        }

        // 设置控件是否可用
        void EnableControls(bool bEnable)
        {
            this.button_start.Enabled = bEnable;
            this.button_stop1.Enabled = !(bEnable);
        }


        private void Form_cancel1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 窗口关闭前让循环退出
            this._cancel.Cancel();
            this._cancel.Dispose();
        }

        // 停止1按钮触发
        private void button_stop1_Click(object sender, EventArgs e)
        {
            // 停止
            this._cancel.Cancel();
        }
    }
}
