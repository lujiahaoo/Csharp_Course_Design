using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Selenium;
using Selenium.Internal;

namespace 爬虫
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]//标记对com可见
    public partial class Form2 : Form
    {
        myclass my = new myclass();
        public Form2()
        {
            InitializeComponent();
        }
        public string GetData()
        {
            string s = "";
            s = my.resultnum("select top(10) Title from WY order by cast(Num as int) desc",0);
            return s;
        }

        public string GetNum()
        {
            string s = my.resultnum("select top(10) Num from WY order by cast(Num as int) desc",2);
            return s;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //防止 WebBrowser 控件打开拖放到其上的文件。
            webBrowser1.AllowWebBrowserDrop = false;

            //防止 WebBrowser 控件在用户右击它时显示其快捷菜单.
            webBrowser1.IsWebBrowserContextMenuEnabled = false;

            //以防止 WebBrowser 控件响应快捷键。
            webBrowser1.WebBrowserShortcutsEnabled = false;

            //以防止 WebBrowser 控件显示脚本代码问题的错误信息。    
            //webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ObjectForScripting = this;//设置对象为当前BusinessDataExportFigure窗体
            string url = "C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\testPage.html";
            this.webBrowser1.Navigate(url);
        }
    }
}