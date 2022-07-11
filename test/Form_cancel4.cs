using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice
{
    public partial class Form_cancel4 : Form
    {
        public Form_cancel4()
        {
            InitializeComponent();
        }

        // 用于停止线程函数里的工作
        CancellationTokenSource _cancel = new CancellationTokenSource();

        private async void button_start_Click(object sender, EventArgs e)
        {
            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();

            // 清空浏览器控件
            this.ClearHtml();

            // 用 Task.Run() 调用一个平凡函数
            List<Item> items = await Task.Run(() =>
            {
                // 设置按钮状态
                this.Invoke((Action)(() =>
                    EnableControls(false)
                    ));
                try
                {
                    return RunTwo();
                }
                finally
                {
                    // 设置按钮状态
                    this.Invoke((Action)(() =>
                        EnableControls(true)
                        ));
                }

            });

            string info = "";
            foreach (Item item in items)
            {
                info += "线程" + items.IndexOf(item) + ":count=[" + item.count.ToString() + "],text=[" + item.text + "]" + "<br/>";
            }
            this.AppendHtml(info);
        }


        // 平凡的函数，要根据调主的意愿才能决定到底运行在什么线程
        List<Item> RunTwo()
        {
            Task<Item> t1 = Task.Run(() =>
            {
                return doSomething(this._cancel.Token, "*");
            });

            Task<Item> t2 = Task.Run(() =>
            {
                return doSomething(this._cancel.Token, "-");
            });

            // 等待2个线程都返回
            Task.WaitAll(new Task[] { t1, t2 });

            // 处理线程返回结果 
            List<Item> items = new List<Item>();
            items.Add(t1.Result);
            items.Add(t2.Result);

            return items;
        }

        // 做事，返回一个对象
        Item doSomething(CancellationToken token, string preprefix)
        {
            int i = 0;
            while (token.IsCancellationRequested == false)
            {
                // 没有这个语句，界面会冻结，如果秒数太少，界面也反应不过来
                Thread.Sleep(100);

                i++;

                string text =  preprefix + i.ToString();
                if (preprefix == "*")
                    text = "<div class='debug error'>" + text + "</div>";
                else
                    text = "<div class='debug green'>" + text + "</div>";
                this.AppendHtml(text);
            }

            Item item = new Item
            {
                text = "合计" + i.ToString(),
                count = i
            };

            return item;
        }

        // 设置控件是否可用
        void EnableControls(bool bEnable)
        {
            this.button_start.Enabled = bEnable;
            this.button_stop1.Enabled = !(bEnable);
        }

        private void Form_cancel1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 窗口关闭前让循环退出，由于后面不再用_cancel，所以调dispose
            this._cancel.Cancel();
            this._cancel.Dispose();
        }

        // 停止按钮触发cancel，让循环退出
        private void button_stop1_Click(object sender, EventArgs e)
        {
            // 停止
            this._cancel.Cancel();
        }

        #region 浏览器控件的相关函数

        delegate void Delegate_AppendHtml(string strText);
        /// <summary>
        /// 向 IE 控件中追加一段 HTML 内容
        /// </summary>
        /// <param name="strText">HTML 内容</param>
        public void AppendHtml(string strText)
        {
            if (this.webBrowser1.InvokeRequired)
            {
                Delegate_AppendHtml d = new Delegate_AppendHtml(AppendHtml);

                this.webBrowser1.BeginInvoke(d, new object[] { strText });
                return;
            }

            WriteHtml(this.webBrowser1,
                strText);

            // 因为HTML元素总是没有收尾，其他有些方法可能不奏效
            this.webBrowser1.Document.Window.ScrollTo(0,
               this.webBrowser1.Document.Body.ScrollRectangle.Height);
        }


        // 不支持异步调用
        /// <summary>
        /// 向一个浏览器控件中追加写入 HTML 字符串
        /// 不支持异步调用
        /// </summary>
        /// <param name="webBrowser">浏览器控件</param>
        /// <param name="strHtml">HTML 字符串</param>
        public static void WriteHtml(WebBrowser webBrowser,
             string strHtml)
        {
            HtmlDocument doc = webBrowser.Document;
            if (doc == null)
            {
                Navigate(webBrowser, "about:blank");
                doc = webBrowser.Document;
            }
            doc.Write(strHtml);
        }

        // 2015/7/28 
        // 能处理异常的 Navigate
        internal static void Navigate(WebBrowser webBrowser, string urlString)
        {
            int nRedoCount = 0;
            REDO:
            try
            {
                webBrowser.Navigate(urlString);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                /*
System.Runtime.InteropServices.COMException (0x800700AA): 请求的资源在使用中。 (异常来自 HRESULT:0x800700AA)
   在 System.Windows.Forms.UnsafeNativeMethods.IWebBrowser2.Navigate2(Object& URL, Object& flags, Object& targetFrameName, Object& postData, Object& headers)
   在 System.Windows.Forms.WebBrowser.PerformNavigate2(Object& URL, Object& flags, Object& targetFrameName, Object& postData, Object& headers)
   在 System.Windows.Forms.WebBrowser.Navigate(String urlString)
   在 dp2Circulation.QuickChargingForm._setReaderRenderString(String strText) 位置 F:\cs4.0\dp2Circulation\Charging\QuickChargingForm.cs:行号 394
                 * */
                if ((uint)ex.ErrorCode == 0x800700AA)
                {
                    nRedoCount++;
                    if (nRedoCount < 5)
                    {
                        Application.DoEvents(); // 2015/8/13
                        Thread.Sleep(200);
                        goto REDO;
                    }
                }

                throw ex;
            }
        }


        /// <summary>
        /// 清除已有的 HTML 显示
        /// </summary>
        public void ClearHtml()
        {
            string strCssUrl = Application.StartupPath + "/history.css"; //PathUtil.MergePath(Program.MainForm.DataDir, "/history.css");
            if (File.Exists(strCssUrl) == false)
            {
                throw new Exception("文件["+strCssUrl+"]不存在");
            }
            string strLink = "<link href='" + strCssUrl + "' type='text/css' rel='stylesheet' />";

            HtmlDocument doc = this.webBrowser1.Document;
            if (doc == null)
            {
                this.webBrowser1.Navigate("about:blank");
                doc = this.webBrowser1.Document;
            }
            doc = doc.OpenNew(true);

            WriteHtml(this.webBrowser1,
                "<html><head>" + strLink + "</head><body>");
        }

        #endregion
    }


    

}
