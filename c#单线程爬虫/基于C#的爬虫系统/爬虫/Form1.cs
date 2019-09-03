using System;
using System.Collections;//哈希表
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using NSoup;
using NSoup.Select;
using System.Threading;
using System.Drawing;
using CCWin;
using WordCloud;
using JiebaNet;
using JiebaNet.Segmenter;
using JiebaNet.Analyser;
using Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
namespace 爬虫
{
    public partial class Form1 : Skin_Mac
    {
        myclass myClass = new myclass();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
if (search1.Text != "" && page.Text != "" && reg1.IsMatch(page.Text))//判断是否输入用户和页数
{
    allelements.Text = null;
    string result = "";
    try
    {
        int count = 0;
        for (int i = 1; i <= Convert.ToInt16(page.Text); i++)
        {
            int k = Convert.ToInt16(page.Text);
            WebRequest webrq = WebRequest.Create("http://blog.csdn.net/" + search1.Text + "/article/list/" + i);
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webrq.GetResponse().GetResponseStream(), "utf-8");
            Elements elements = doc.Select("div.article-item-box.csdn-tracking-statistics");//相当于一个集合
            for (int j = 1; j < elements.Count; j++)//多了一行所以不用下面的方法 （帝都的凛冬）
            {
                string title = elements[j].GetElementsByTag("h4").Text.ToString() + "\r\n";
                string url = elements[j].Select("a").Attr("href").ToString() + "\r\n";
                string date = elements[j].GetElementsByClass("date").Text + "\r\n";
                result = title + url + date + "- - - - - - - - - - - - - - - -\r\n";
                allelements.SkinTxt.AppendText(result);
                count += 1;
            }
            Thread.Sleep(200);
        }
        allelements.SkinTxt.AppendText("总共:" + count + "篇文章");
        allelements.SkinTxt.SelectionStart = allelements.SkinTxt.TextLength;//获取文本起始点位置为文本长度也就是最后一个字符（光标在最后一个字符闪烁）
        allelements.SkinTxt.ScrollToCaret();//滚动到文本插入的位置（光标闪烁处）
        Application.DoEvents();
    }
    catch
    {
        allelements.SkinTxt.AppendText("请确认用户名无误或页数是否超过最大页数");//如果访问不到404就会返回这个
    }
}
else
{
    MessageBox.Show("请输入正确用户ID和用户文章页数");
}
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
string result = "";
string[] all = Regex.Split(allelements.Text.ToString(), "- - - - - - - - - - - - - - - -\r\n", RegexOptions.IgnoreCase);
for (int i = 0; i < all.Length - 1; i++)//减一是因为最后面多了一行- - - - -然后多分了一组值为""的组
{
    string lower = all[i].ToLower();
    if (lower.Contains(search2.Text.ToLower()))
    {
        //result += all[i] + "\n" + "- - - - - - - - - - - - - - - -\r\n";
        resultelements.SkinTxt.AppendText(all[i] + "\n" + "- - - - - - - - - - - - - - - -\r\n");
        resultelements.SkinTxt.SelectionStart = resultelements.SkinTxt.TextLength;//获取文本起始点位置为文本长度也就是最后一个字符（光标在最后一个字符闪烁）
        resultelements.SkinTxt.ScrollToCaret();//滚动到文本插入的位置（光标闪烁处）
        Application.DoEvents();
    }
}
resultelements.SkinTxt.SelectionStart = 0;
resultelements.SkinTxt.ScrollToCaret();
if (resultelements.SkinTxt.Text == string.Empty || resultelements.SkinTxt.Text == null)
{
    resultelements.Text = "未找到相关文章";
}
else
{
    //打印前10个包含了名词和动词的关键词
    string keywords = myClass.Getkeywords(resultelements.Text,15);
    string[] arrary = keywords.Split(' ');
    foreach (var ke in arrary)
    {
        keyTextBox1.SkinTxt.AppendText(ke + "|");
        Application.DoEvents();
    }
}
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string frequancy = myClass.WordFrequancy(resultelements.SkinTxt.Text, frequancysearch.Text);
            MessageBox.Show("在文章中共出现:" + frequancy + "次");
        }

        //private void Button4_Click(object sender, EventArgs e)
        //{
        //    if (resultelements.SkinTxt.Text != string.Empty && resultelements.SkinTxt.Text != " " && resultelements.SkinTxt.Text != null)
        //    {
        //        string ss = myClass.Getkeywords(resultelements.Text,15);
        //        string[] sss = ss.Split(' ');
        //        string name = myClass.Pic2(sss, skinPictureBox1.Width, skinPictureBox1.Height);
        //        skinPictureBox1.Image = Image.FromFile("C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\image\\" + name + ".png");
        //    }
        //    else
        //    {
        //        MessageBox.Show("暂无需要查找关键字的文章");
        //    }
        //}

        //private void Button5_Click(object sender, EventArgs e)
        //{
        //    //调用词云图
        //    if (resultelements.Text.ToString() != string.Empty && resultelements.Text.ToString() != "" && resultelements.Text.ToString() != null)
        //    {
        //        string keywords = myClass.Getkeywords(resultelements.Text,15);
        //        skinPictureBox1.BackColor = Color.White;
        //        skinPictureBox1.Image = myClass.Picwords(keywords, skinPictureBox1.Width, skinPictureBox1.Height);
        //    }
        //    else
        //    {
        //        MessageBox.Show("暂无需要查找关键字的文章");
        //    }
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void t2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void panelskinButton2_Click(object sender, EventArgs e)
        {
            skinPanel1.Visible = true;
            skinPanel2.Visible = false;
            panel3skinPanel3.Visible = false;
        }

        private void skinPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {


        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            if (skinTextpage.Text != "" && skinTextpage.Text != "" && reg1.IsMatch(skinTextpage.Text))//判断是否输入用户和页数
            {
                List<string> title = new List<string>();
                List<string> src = new List<string>();
                List<string> num = new List<string>();
                ChromeOptions options = new ChromeOptions();
                IWebDriver driver = new ChromeDriver(System.Environment.CurrentDirectory, options);
                driver.Url = "https://www.baidu.com";
                Thread.Sleep(2000);
                string url = "https://music.163.com/#/discover/playlist";
                int pagecount = 1;
                while (pagecount <= Convert.ToInt16(skinTextpage.Text))
                {
                    driver.Navigate().GoToUrl(url);
                    Thread.Sleep(500);
                    driver.SwitchTo().Frame("g_iframe");//找到内嵌iframe
                    IList<IWebElement> msk = driver.FindElements(By.ClassName("msk"));
                    IList<IWebElement> nb = driver.FindElements(By.ClassName("nb"));
                    for (int i = 0; i < msk.Count; i++)
                    {
                        title.Add(msk[i].GetAttribute("title"));
                        src.Add(msk[i].GetAttribute("href"));
                        num.Add(nb[i].Text);
                    }
                    url = driver.FindElement(By.CssSelector("[class='zbtn znxt']")).GetAttribute("href");//复合属性所以直接查找class名
                    wyt1.SkinTxt.AppendText("正在读取下一页...\r\n");
                    pagecount += 1;
                    Application.DoEvents();
                }
                driver.Quit();
                string[] a1 = title.ToArray();
                string[] a2 = src.ToArray();
                string[] a3 = num.ToArray();

                for (int j = 0; j < a1.Length; j++)
                {
                    wyt1.SkinTxt.AppendText("标题：" + a1[j] + "\r\n" + "链接：" + a2[j] + "\r\n" + "播放数：" + a3[j] + "\r\n" + "- - - - - - - - - - - - \r\n");
                    Application.DoEvents();
                    if (a3[j].Contains("万"))
                    {
                        a3[j] = a3[j].Replace("万", "0000");
                    }
                }
                wyt1.SkinTxt.AppendText("正在插入数据库...\r\n");
                if (myClass.WY(a1, a2, a3) == 1)
                {
                    MessageBox.Show("插入成功...");
                    wyt1.SkinTxt.AppendText("插入完成");
                }
                else
                {
                    MessageBox.Show("保存失败...");
                    wyt1.SkinTxt.AppendText("插入失败.");
                }

            }
            else
            {
                MessageBox.Show("请输入正确页数");
            }
        }

        private void skinButton1_Click_1(object sender, EventArgs e)
        {
            skinPanel2.Visible = true;
            skinPanel1.Visible = false;
            panel3skinPanel3.Visible = false;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");      
if (danmupage.Text!= ""&&danmupage.Text!=" " &&  reg1.IsMatch(danmupage.Text))
{
int count = Convert.ToInt32(danmupage.Text);
string danmu = "";
danmutext.Text = "";
danmukeywordstext.Text = "";
for (int i = 1; i <= count; i++)
{
    WebRequest webrq = WebRequest.Create("https://service.danmu.youku.com/list?jsoncallback=jQuery111205862589427617528_1554000382825&mat=" + i + "&mcount=1&ct=1001&iid=1019832795&aid=327088&cid=97&lid=0&ouid=0&_=1554000382861");
    NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webrq.GetResponse().GetResponseStream(), "utf-8");
    object htmlele = doc;
    string htmletes = htmlele.ToString();

    Regex reg = new Regex("[\u4e00-\u9fa5]+");
    foreach (Match v in reg.Matches(htmletes))
    {
        danmu += v;
    }

    danmutext.SkinTxt.AppendText(danmu);

    Application.DoEvents();

    Thread.Sleep(100);
}

string keywords = myClass.Getkeywords(danmu, 45);

danmukeywordstext.SkinTxt.AppendText(keywords);
}
else
{
MessageBox.Show("输入正确分钟数");
}
        }

        private void panel3skinButton1_Click(object sender, EventArgs e)
        {
            skinPanel2.Visible = false;
            skinPanel1.Visible = false;
            panel3skinPanel3.Visible = true;
        }

        private void danmucloud_Click(object sender, EventArgs e)
        {
            if (danmukeywordstext.Text != "")
            {
                skinPictureBox2.BackColor = Color.White;
                skinPictureBox2.Image = myClass.Picwords(danmukeywordstext.Text, skinPictureBox2.Width, skinPictureBox2.Height);
            }
            else
            {
                MessageBox.Show("没有找到关键字");
            }
        }

        private void skinButton1_Click_2(object sender, EventArgs e)
        {
            Form nextform = new Form2();
            nextform.Show();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            int i = myClass.delet();
            if (i != 1)
            {
                MessageBox.Show("出错了");
            }
            else
            {
                MessageBox.Show("清空成功");
            }
        }
    }
}
