using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ApiLogger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        protected void Logger(string log)
        {
            try
            {
                if (richTextBox1.Lines.Length > 400)
                {
                    richTextBox1.Text = "";
                }
                richTextBox1.Text += "[" + DateTime.Now.ToString() + "] " + (log) + " \n ======================= \n";
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
                Application.DoEvents();
            }
            catch { }
        }
        public Cities getData(Cities ct)
        {

            try
            {
                Logger(ct.Name);
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://artgroup.feeds.meteonews.net/forecasts/geonames/" + ct.Code.Replace("G", "") + ".xml?cumulation=3h&begin=" + DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd HH:mm") + @"&end=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "&lang=en");
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://artgroup.feeds.meteonews.net/observations/geonames/" + ct.Code.Replace("G", "") + ".xmll?lang=en");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://artgroup.feeds.meteonews.net/observations/mexs/" + ct.Code.Replace("G", "") + ".xml?lang=en");
                request.Credentials = new System.Net.NetworkCredential("artgroup", "Ar+HIspGr0p");
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader readerResult = new StreamReader(stream);

                var result = readerResult.ReadToEnd();
                stream.Dispose();
                readerResult.Dispose();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);


                XmlNodeList elemListMin = xmlDoc.GetElementsByTagName("temp");
                if (elemListMin.Count > 0)
                {
                    ct.Min = elemListMin[0].InnerText;
                    Logger("temp:" + ct.Min);
                }

                XmlNodeList elemListState = xmlDoc.GetElementsByTagName("txt");
                if (elemListState.Count > 0)
                {
                    ct.State = elemListState[elemListState.Count - 1].InnerText;
                    Logger("State:" + ct.State);
                }

            }
            catch (Exception Exp)
            {
                ct.Max = "0";
                ct.Max2 = "0";
                ct.Min = "0";
                ct.Min2 = "0";
                ct.State = "0";
                ct.State2 = "0";
                ct.Avg = "0"; ;
                richTextBox1.Text += Exp.Message + "  \n";
            }


            return ct;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button1.ForeColor = Color.White;
            button1.Text = "Started";
            button1.BackColor = Color.Red;
            richTextBox1.Text = "";
            try
            {
                List<Cities> Cts = new List<Cities>();
                Cts.Add(new Cities { Name = "ABUJA", Code = "61080000" });
                Cts.Add(new Cities { Name = "Baghdad", Code = "40623000" });
                Cts.Add(new Cities { Name = "beijing", Code = "54527000" });
                Cts.Add(new Cities { Name = "Beirut", Code = "40155000" });
                Cts.Add(new Cities { Name = "Berlin", Code = "10476000" });
                Cts.Add(new Cities { Name = "Brasilia", Code = "83424000" });
                Cts.Add(new Cities { Name = "BUENOS AIRES", Code = "87576000" });
                Cts.Add(new Cities { Name = "Cairo", Code = "62324001" });
                Cts.Add(new Cities { Name = "Caracas", Code = "78988000" });
                Cts.Add(new Cities { Name = "Casablanca", Code = "60135000" });
                Cts.Add(new Cities { Name = "Damascus", Code = "40080000" });
                Cts.Add(new Cities { Name = "Havana", Code = "78229000" });
                Cts.Add(new Cities { Name = "Jerusalem (AL QUDS)", Code = "40176000" });
                Cts.Add(new Cities { Name = "Karachi", Code = "42734000" });
                Cts.Add(new Cities { Name = "Kuwait City", Code = "40373000" });
                Cts.Add(new Cities { Name = "Lisbon", Code = "08330000" });
                Cts.Add(new Cities { Name = "London", Code = "03772000" });
                Cts.Add(new Cities { Name = "Madrid", Code = "08221000" });
                Cts.Add(new Cities { Name = "Mashhad", Code = "40723000" });
                Cts.Add(new Cities { Name = "Mecca", Code = "41084000" });
                Cts.Add(new Cities { Name = "Melbourne", Code = "94864000" });
                Cts.Add(new Cities { Name = "Mexico City", Code = "76679000" });
                Cts.Add(new Cities { Name = "Moscow", Code = "27614000" });
                Cts.Add(new Cities { Name = "Muscat", Code = "41246000" });
                Cts.Add(new Cities { Name = "New Delhi", Code = "42181000" });
                Cts.Add(new Cities { Name = "Paris", Code = "07156000" });
                Cts.Add(new Cities { Name = "Pretoria", Code = "68263000" });
                Cts.Add(new Cities { Name = "Sana'a", Code = "41128000" });
                Cts.Add(new Cities { Name = "Tehran", Code = "40754000" });
                Cts.Add(new Cities { Name = "Tokyo", Code = "47686000" });
                Cts.Add(new Cities { Name = "Tunis", Code = "60720000" });
                Cts.Add(new Cities { Name = "Washington, D.C.", Code = "72405003" });

                List<Cities> CtsFinal = new List<Cities>();
                foreach (var item in Cts)
                {
                    CtsFinal.Add(getData(item));
                }
                StringBuilder Data = new StringBuilder();
                for (int i = 0; i < CtsFinal.Count; i++)
                {
                    richTextBox1.Text += CtsFinal[i].Name.ToString().Trim() + ":\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    Application.DoEvents();
                    Data.AppendLine("City" + (i + 1).ToString() + "=[ \"" + CtsFinal[i].Name.Trim().Replace("\n", "") + "\",\"" + double.Parse(CtsFinal[i].Min.ToString()) + "\"]");
                }
                StreamWriter s = new StreamWriter(@"E:\Output\Weather_CurrentNew\Log\" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".txt");
                s.Write(Data);
                s.Dispose();
                Logger("Finished");
            }
            catch (Exception Exp)
            {
                StreamWriter s = new StreamWriter(@"E:\Output\Weather_CurrentNew\Log\" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + "-Error.txt");
                s.Write(Exp.Message);
                s.Dispose();
            }

            button1.ForeColor = Color.White;
            button1.Text = "START";
            button1.BackColor = Color.Navy;

            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
        }
    }
}
