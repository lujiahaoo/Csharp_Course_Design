using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiebaNet;
using JiebaNet.Segmenter;//分割
using JiebaNet.Analyser;//关键字
using System.Collections;//哈希表
using NSoup;
using NSoup.Select;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WordCloud;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
namespace 爬虫
{
    class myclass
    {

        //返回关键字
        public string Getkeywords(string text,int count)
        {
            var kk = new JiebaSegmenter();
            var extractor = new TfidfExtractor();
            var keywords = extractor.ExtractTags(text,count, Constants.NounAndVerbPos);
            string key = string.Join(" ", keywords);
            return key;
        }

        //查找词频
        public string WordFrequancy(string text, string findword)
        {
            //对文本分割
            var jieba = new JiebaSegmenter();
            string afterjieba = string.Join(" ", jieba.Cut(text));
            string[] array = afterjieba.Split(' ');
            int findwordcount = 0;
            foreach (string compare in array)
            {
                if (compare == findword)
                {
                    findwordcount += 1;
                }
            }
            return findwordcount.ToString();
        }
        //所有词频
        public string[,] Allfrequancy(string text)
        {
            //对文本分割
            var jieba = new JiebaSegmenter();
            string afterjieba = string.Join(" ", jieba.Cut(text));
            string[] array = afterjieba.Split(' ');


            Hashtable ht = new Hashtable();
            for (int i = 0; i < array.Length; i++)
            {
                if (ht.ContainsKey(array[i]))
                {
                    ht[array[i]] = (int)ht[array[i]] + 1;
                }
                else
                    ht.Add(array[i], 1);
            }
            string key = null;
            string frequency = null;
            string[,] result = new string[2, 1];
            foreach (DictionaryEntry de in ht)
            {
                key += de.Key + " ";
                frequency += de.Value + " ";
            }
            result[0, 0] = key;
            result[1, 0] = frequency;
            return result;
        }
        public string Pic2(string[] Picwords, int width, int height)
        {
            if (Picwords == null)
                return null;
            string name = DateTime.Now.ToString("yyyyMMddhhmmss");
            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);
            Random rd = new Random();
            try
            {
                Random r = new Random();
                g.Clear(Color.White);
                //for (int i = 0; i < 25; i++)
                //{
                //    int x1 = r.Next(image.Width);
                //    int x2 = r.Next(image.Width);
                //    int y1 = r.Next(image.Height);
                //    int y2 = r.Next(image.Height);
                //    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                //}
                for (int i = 0; i < Picwords.Length; i++)
                {
                    int size = 20 - i;
                    Font font = new Font("Arial", size);
                    Brush b = new SolidBrush(Randomcolor());
                    g.DrawString(Picwords[i], font, b, rd.Next(0, 240), rd.Next(4, 120));
                }

                for (int j = 0; j < 100; j++)
                {
                    int x = r.Next(image.Width);
                    int y = r.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(r.Next()));
                }
                g.DrawRectangle(new Pen(Color.Wheat), 0, 0, image.Width, image.Height);

                if (!System.IO.Directory.Exists(@"C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\image"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\image");//不存在就创建文件夹 } 
                    //判断某文件是否存在 
                    if (File.Exists(@"C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\image" + name + ".png"))
                    {
                        return name;
                    }
                    else
                    {
                        image.Save("C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\image\\" + name + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        return name;
                    }
                }
                else
                {
                    image.Save("C:\\Users\\卢本伟\\Desktop\\爬虫\\爬虫\\image\\" + name + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    return name;
                }
            }
            finally
            {
                image.Dispose();
                g.Dispose();
            }
        }
        public Color Randomcolor()
        {
            Random rd = new Random();
            return Color.FromArgb(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255));
        }

        //返回词云图
        public Image Picwords(string keywords, int width, int height)
        {
            List<string> words = new List<string>();
            List<int> fre = new List<int>();
            string[] keyarray = keywords.Split(' ');
            int j = 30;
            for (int i = 0; i < keyarray.Length; i++)
            {
                words.Add(keyarray[i]);
                fre.Add(keyarray.Length-i);
            }
            var wc = new WordCloud.WordCloud(width, height);
            var img = wc.Draw(words, fre);
            return img;
        }

        public int WY(string[] title,string[] src,string[] num)
        {
            SqlConnection conn = null;
            string sql = "select * From WY";

            DataTable dt = null;    
            DataSet ds = new DataSet();
              try
            {
                conn = new SqlConnection("Server=localhost;Initial Catalog=C#;Trusted_Connection=yes;");
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = new SqlCommand(sql, conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(sda);//自动生成相应的命令，这句很重要
 
                conn.Open();
 
                sda.Fill(ds);
                dt = ds.Tables[0];

                for (int i = 0; i < title.Length; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Title"] =title[i] ;
                    dr["Src"] =src[i];
                    dr["Num"] = num[i];
                    dt.Rows.Add(dr);
                }
 
                sda.Update(dt);//对表的更新提交到数据库
 
                dt.AcceptChanges();
                return 1;
            }
            catch (SqlException ex)
            {
                return 2;
            }
            finally
            {
                conn.Close();
            }
        }

        public string resultnum(string cmdtext,int loc)
        {
            SqlConnection conn = null;

            DataTable dt = new DataTable();

            string array="";
            try
            {
                conn = new SqlConnection("Server=localhost;Initial Catalog=C#;Trusted_Connection=yes;");
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmdtext,conn);
                sda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    array+= dt.Rows[i][0].ToString()+",";
                }
                array = array.Remove(array.Length - 1, 1);
                return array;
            }
            catch (SqlException ex)
            {
                array=null;
                return array;
            }
            finally
            {
                conn.Close();
            }
        }

        public int delet()
        {
            SqlConnection conn = null;

            DataTable dt = new DataTable();

            string array = "";
            try
            {
                conn = new SqlConnection("Server=localhost;Initial Catalog=C#;Trusted_Connection=yes;");
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from WY",conn);
                int i = cmd.ExecuteNonQuery();
                return 1;
                
            }
            catch (SqlException ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        
    }
}
