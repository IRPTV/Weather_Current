using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeatherForecast.MyDBTableAdapters;

namespace WeatherForecast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                if (elemListMin.Count >0)
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
        string DateTimeStr = string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month) + "-" + string.Format("{0:00}", DateTime.Now.Day) + "_" + string.Format("{0:00}", DateTime.Now.Hour) + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;


            button1.ForeColor = Color.White;
            button1.Text = "Started";
            button1.BackColor = Color.Red;
            richTextBox1.Text = "";

            try
            {

                string[] FilesList = Directory.GetFiles(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim());
                foreach (string item in FilesList)
                {
                    try
                    {
                        if (File.GetLastAccessTime(item) < DateTime.Now.AddHours(-48))
                        {
                            File.Delete(item);
                            richTextBox1.Text += (item) + " *Deleted* \n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                            Application.DoEvents();
                        }
                    }
                    catch (Exception Exp)
                    {
                        richTextBox1.Text += (Exp) + " \n";
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();
                        Application.DoEvents();
                    }

                }



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

                Logger("Start Job");


                //Collect Weather Data
                //CitiesTableAdapter Ta = new CitiesTableAdapter();
                //MyDB.CitiesDataTable Dt = Ta.SelectAllCities();

                StringBuilder Data = new StringBuilder();
                //Generate XML file:
                for (int i = 0; i < CtsFinal.Count; i++)
                {
                    richTextBox1.Text += CtsFinal[i].Name.ToString().Trim() + ":\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    Application.DoEvents();
                    //Math.Round(double.Parse(Dt[i]["Forecasts1Min"].ToString())-(273.15));

                    //Weather Format: ["City","1st Day Min","1st Day Max","2nd Day Min","2nd Day Max"]	
                    Data.AppendLine("City" + (i + 1).ToString() + "=[ \"" + CtsFinal[i].Name.Trim().Replace("\n", "") + "\",\"" + double.Parse(CtsFinal[i].Min.ToString()) + "\"]");

                    //Copy Status Video:
                    string MainStatusDir = ConfigurationSettings.AppSettings["StatusSource"].ToString().Trim();
                    string MainStatusDirDest = ConfigurationSettings.AppSettings["StatusDest"].ToString().Trim();


                    if (!Directory.Exists(MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\"))
                        Directory.CreateDirectory(MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\");


                    if (CtsFinal[i].State.ToLower().Contains("thunderstorm"))
                        File.Copy(MainStatusDir + "Lightening\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                     if (CtsFinal[i].State.ToLower().Contains("thundersdrizzletorm"))
                        File.Copy(MainStatusDir + "Rain\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                      if (CtsFinal[i].State.ToLower().Contains("rain"))
                        File.Copy(MainStatusDir + "Rain\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                      if (CtsFinal[i].State.ToLower().Contains("snow"))
                        File.Copy(MainStatusDir + "Snow\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                      if (CtsFinal[i].State.ToLower().Contains("atmosphere"))
                        File.Copy(MainStatusDir + "Sunny\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                      if (CtsFinal[i].State.ToLower().Contains("cloud"))
                        File.Copy(MainStatusDir + "Cloudy\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                      if (CtsFinal[i].State.ToLower().Contains("clear"))
                        File.Copy(MainStatusDir + "Sunny\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                    else
                        File.Copy(MainStatusDir + "Sunny\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                }
                //Save Xml File:
                StreamWriter s = new StreamWriter(ConfigurationSettings.AppSettings["DataPath"].ToString().Trim());
                s.Write(Data);
                s.Dispose();
                DateTimeStr = string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month) + "-" + string.Format("{0:00}", DateTime.Now.Day) + "_" + string.Format("{0:00}", DateTime.Now.Hour) + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);

                //Render Video:
                render();
                //Convert:
                Convert();
            }
            catch (Exception Ex)
            {
                Logger(Ex.Message);
            }

            button1.ForeColor = Color.White;
            button1.Text = "START";
            button1.BackColor = Color.Navy;

            timer1.Enabled = true;
        }
        public void Convert()
        {
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim());
                Dir.Create();
                string DestFile = ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim() + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + "_" + DateTimeStr + ".mp4";
                string SourceFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + ".mov";

                Process proc = new Process();
                proc.StartInfo.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\ffmpeg";
                proc.StartInfo.Arguments = "-y -i " + SourceFile + "      \"" + DestFile + "\"";
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.EnableRaisingEvents = true;
                proc.Start();
                StreamReader reader = proc.StandardError;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Logger(line);
                }
                try
                {
                    string StaticDestFileName = ConfigurationSettings.AppSettings["ScheduleDestFileName"].ToString().Trim();
                    // File.Delete(StaticDestFileName);
                    File.Copy(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim() + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + "_" + DateTimeStr + ".mp4", StaticDestFileName, true);
                    Logger("COPY FINAL:" + StaticDestFileName);

                }
                catch (Exception Ex)
                {
                    Logger(Ex.Message);
                }
            }
            catch (Exception Ex)
            {
                Logger(Ex.Message);
            }
        }
        protected void render()
        {
            try
            {
                Logger("Start Render:");
                Process proc = new Process();
                proc.StartInfo.FileName = "\"" + ConfigurationSettings.AppSettings["AeRenderPath"].ToString().Trim() + "\"";

                // string DateTimeStr = string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month) + "-" + string.Format("{0:00}", DateTime.Now.Day) + "_" + string.Format("{0:00}", DateTime.Now.Hour) + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);

                DirectoryInfo Dir = new DirectoryInfo(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim());

                if (!Dir.Exists)
                {
                    Dir.Create();
                }
                try
                {
                    File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + ".mov");
                }
                catch { }

                proc.StartInfo.Arguments = " -project " + "\"" + ConfigurationSettings.AppSettings["AeProjectFile"].ToString().Trim() + "\"" + "   -comp   \"" + ConfigurationSettings.AppSettings["Composition"].ToString().Trim() + "\" -output " + "\"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + ".mov" + "\"";
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;

                if (!proc.Start())
                {
                    return;
                }

                proc.PriorityClass = ProcessPriorityClass.Normal;
                StreamReader reader = proc.StandardOutput;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (richTextBox1.Lines.Length > 3)
                    {
                        richTextBox1.Text = "";
                    }
                    richTextBox1.Text += (line) + " \n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    Application.DoEvents();
                }
                proc.Close();

                //try
                //{
                //    string StaticDestFileName = ConfigurationSettings.AppSettings["ScheduleDestFileName"].ToString().Trim();
                //    // File.Delete(StaticDestFileName);
                //    File.Copy(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim() + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + "_" + DateTimeStr + ".mp4", StaticDestFileName, true);
                //    richTextBox1.Text += "COPY FINAL:" + StaticDestFileName + " \n";
                //    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                //    richTextBox1.ScrollToCaret();
                //    Application.DoEvents();
                //}
                //catch (Exception Ex)
                //{
                //    richTextBox1.Text += Ex.Message + " \n";
                //    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                //    richTextBox1.ScrollToCaret();
                //    Application.DoEvents();
                //}
            }
            catch (Exception Ex)
            {
                Logger(Ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(null,null);
        }
    }
}
