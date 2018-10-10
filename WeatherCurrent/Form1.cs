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
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://artgroup.feeds.meteonews.net/forecasts/id/" + ct.Code + ".xml?lang=en&cumulation=24h&end=2d");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://artgroup.feeds.meteonews.net/forecasts/geonames/" + ct.Code.Replace("G", "") + ".xml?cumulation=3h&begin=" + DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd HH:mm") + @"&end=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "&lang=en");
                request.Credentials = new System.Net.NetworkCredential("artgroup", "Ar+HIspGr0p");
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader readerResult = new StreamReader(stream);

                var result = readerResult.ReadToEnd();
                stream.Dispose();
                readerResult.Dispose();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);




                XmlNodeList elemListavg = xmlDoc.GetElementsByTagName("temp_avg");
                if (elemListavg.Count>0)
                {
                    ct.Avg = elemListavg[elemListavg.Count-1].InnerText;                    
                    Logger("Current: " + ct.Avg);
                }
                XmlNodeList elemListState = xmlDoc.GetElementsByTagName("txt");
                if (elemListState.Count > 0)
                {
                    ct.State = elemListState[elemListavg.Count - 1].InnerText;                  
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
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;


            button1.ForeColor = Color.White;
            button1.Text = "Started";
            button1.BackColor = Color.Red;
            richTextBox1.Text = "";


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
            Cts.Add(new Cities { Name = "ABUJA", Code = "G2352778" });
            Cts.Add(new Cities { Name = "BUENOS AIRES", Code = "G3435910" });
            Cts.Add(new Cities { Name = "Melborne", Code = "G4075766" });
            Cts.Add(new Cities { Name = "Brasilia", Code = "G3469058" });
            Cts.Add(new Cities { Name = "beijing", Code = "G1816670" });
            Cts.Add(new Cities { Name = "Havana", Code = "G3553478" });
            Cts.Add(new Cities { Name = "Cairo", Code = "G360630" });
            Cts.Add(new Cities { Name = "Paris", Code = "G2988507" });
            Cts.Add(new Cities { Name = "Berlin", Code = "G2950159" });
            Cts.Add(new Cities { Name = "New Delhi", Code = "G1261481" });
            Cts.Add(new Cities { Name = "Isfahan", Code = "G418863" });
            Cts.Add(new Cities { Name = "Tehran", Code = "G112931" });
            Cts.Add(new Cities { Name = "Baghdad", Code = "G98182" });
            Cts.Add(new Cities { Name = "Kuwait City", Code = "G285787" });
            Cts.Add(new Cities { Name = "Tokyo", Code = "G1850147" });
            Cts.Add(new Cities { Name = "Beirut", Code = "G276781" });
            Cts.Add(new Cities { Name = "Mexico City", Code = "G3530597" });
            Cts.Add(new Cities { Name = "Casablanca", Code = "G2553604" });
            Cts.Add(new Cities { Name = "Karachi", Code = "G1174872" });
            Cts.Add(new Cities { Name = "JERUSALEM (AL QUDS)", Code = "G7870654" });
            Cts.Add(new Cities { Name = "Muscat", Code = "G287286" });
            Cts.Add(new Cities { Name = "Moscow", Code = "G524901" });
            Cts.Add(new Cities { Name = "Mecca", Code = "G104515" });
            Cts.Add(new Cities { Name = "Pretoria", Code = "G964137" });
            Cts.Add(new Cities { Name = "Madrid", Code = "G3117735" });
            Cts.Add(new Cities { Name = "Damascus", Code = "G170654" });
            Cts.Add(new Cities { Name = "Tunis", Code = "G2464470" });
            Cts.Add(new Cities { Name = "London", Code = "G2643743" });
            Cts.Add(new Cities { Name = "Lisbon", Code = "G2267057" });
            Cts.Add(new Cities { Name = "Washington", Code = "G4140963" });
            Cts.Add(new Cities { Name = "Caracas", Code = "G3646738" });
            Cts.Add(new Cities { Name = "Sana'a", Code = "G7789599" });

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
                Data.AppendLine("City" + (i + 1).ToString() + "=[ \"" + CtsFinal[i].Name.Trim().Replace("\n", "") + "\",\"" +double.Parse(CtsFinal[i].Avg.ToString()) + "\"]");

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

            //Render Video:
            render();
            //Convert:
            Convert();

            button1.ForeColor = Color.White;
            button1.Text = "START";
            button1.BackColor = Color.Navy;

            timer1.Enabled = true;
        }
        public void Convert()
        {
            string DateTimeStr = string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month) + "-" + string.Format("{0:00}", DateTime.Now.Day) + "_" + string.Format("{0:00}", DateTime.Now.Hour) + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);
            DirectoryInfo Dir = new DirectoryInfo(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim());
            Dir.Create();
            string DestFile = ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim() + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + "_" + DateTimeStr + ".mp4";
            string SourceFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + ".avi";

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
        protected void render()
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
                File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + ".avi");
            }
            catch { }

            proc.StartInfo.Arguments = " -project " + "\"" + ConfigurationSettings.AppSettings["AeProjectFile"].ToString().Trim() + "\"" + "   -comp   \"" + ConfigurationSettings.AppSettings["Composition"].ToString().Trim() + "\" -output " + "\"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + ".avi" + "\"";
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
