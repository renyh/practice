using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace practice
{
    // 定义委托协议
    public delegate void ShowInfoDelegate(string text);

    // 采用回调函数方式
    public class Worker1
    {      
        public Item doSomething(CancellationToken token, string prefix, ShowInfoDelegate myDele)
        {
            int i = 0;
            while (token.IsCancellationRequested == false)
            {

                i++;

                // 回调函数
                string text = prefix + i.ToString();
                myDele(text);
            }

            Item item = new Item
            {
                text = "合计" + i.ToString(),
                count = i
            };

            return item;
        }
    }


    // 采用事件方式
    public class Worker2
    {
        //声明事件
        public event ShowInfoDelegate ShowInfoHandler;

        // 做事
        public Item doSomething(CancellationToken token,string prefix)
        {
            int i = 0;
            while (token.IsCancellationRequested == false)
            {

                i++;

                // 通知接管了该事件的外面调用者
                if (ShowInfoHandler != null)
                {
                    string text= prefix+ i.ToString();
                    ShowInfoHandler(text);
                }
            }

            Item item = new Item
            {
                text = "合计" + i.ToString(),
                count = i
            };

            return item;
        }
    }


    public class Item
    {
        public string text { get; set; }
        public int count { get; set; }
    }
}
