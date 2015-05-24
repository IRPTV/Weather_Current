using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeatherForecast.MyDBTableAdapters;

namespace WeatherForecast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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



            richTextBox1.Text += "START JOB \n";
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
            Application.DoEvents();


            //Collect Weather Data
            CitiesTableAdapter Ta = new CitiesTableAdapter();
            MyDB.CitiesDataTable Dt = Ta.SelectAllCities();

            StringBuilder Data = new StringBuilder();
            //Generate XML file:
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                richTextBox1.Text += Dt[i]["CityName"].ToString().Trim() + ":\n";
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
                Application.DoEvents();
                //Math.Round(double.Parse(Dt[i]["Forecasts1Min"].ToString())-(273.15));

                //Weather Format: ["City","1st Day Min","1st Day Max","2nd Day Min","2nd Day Max"]	
                Data.AppendLine("City" + (i + 1).ToString() + "=[ \"" + Dt[i]["CityName"].ToString().Trim().Replace("\n", "") + "\",\"" + Math.Round(double.Parse(Dt[i]["TmpCurrent"].ToString()) - (273.15)) + "\"]");

                //Copy Status Video:
                string MainStatusDir = ConfigurationSettings.AppSettings["StatusSource"].ToString().Trim();
                string MainStatusDirDest = ConfigurationSettings.AppSettings["StatusDest"].ToString().Trim();


                if (!Directory.Exists(MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\"))
                    Directory.CreateDirectory(MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\");
                switch (Dt[i]["StatusMain"].ToString().Trim())
                {
                    case "Thunderstorm":
                        File.Copy(MainStatusDir + "Lightening\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    case "Drizzle":
                        File.Copy(MainStatusDir + "Rain\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    case "Rain":
                        File.Copy(MainStatusDir + "Rain\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    case "Snow":
                        File.Copy(MainStatusDir + "Snow\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    case "Atmosphere":
                        File.Copy(MainStatusDir + "Sunny\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    case "Clouds":
                        File.Copy(MainStatusDir + "Cloudy\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    case "Clear":
                        File.Copy(MainStatusDir + "Sunny\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                    default:
                        File.Copy(MainStatusDir + "Sunny\\WP.mov", MainStatusDirDest + "City (" + (i + 1).ToString() + ")" + "\\WP.mov", true);
                        break;
                }              
            }
            //Save Xml File:
            StreamWriter s = new StreamWriter(ConfigurationSettings.AppSettings["DataPath"].ToString().Trim());
            s.Write(Data);
            s.Dispose();

            //Render Video:
            render();

            button1.ForeColor = Color.White;
            button1.Text = "START";
            button1.BackColor = Color.Navy;

            timer1.Enabled = true;
        }
        protected void render()
        {

            Process proc = new Process();
            proc.StartInfo.FileName = "\"" + ConfigurationSettings.AppSettings["AeRenderPath"].ToString().Trim() + "\"";
            string DateTimeStr = string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month) + "-" + string.Format("{0:00}", DateTime.Now.Day) + "_" + string.Format("{0:00}", DateTime.Now.Hour) + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);

            DirectoryInfo Dir = new DirectoryInfo(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim());

            if (!Dir.Exists)
            {
                Dir.Create();
            }


            proc.StartInfo.Arguments = " -project " + "\"" + ConfigurationSettings.AppSettings["AeProjectFile"].ToString().Trim() + "\"" + "   -comp   \"" + ConfigurationSettings.AppSettings["Composition"].ToString().Trim() + "\" -output " + "\"" + ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim() + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + "_" + DateTimeStr + ".mp4" + "\"";
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

            try
            {
                string StaticDestFileName = ConfigurationSettings.AppSettings["ScheduleDestFileName"].ToString().Trim();
                // File.Delete(StaticDestFileName);
                File.Copy(ConfigurationSettings.AppSettings["OutputPath"].ToString().Trim() + ConfigurationSettings.AppSettings["OutPutFileName"].ToString().Trim() + "_" + DateTimeStr + ".mp4", StaticDestFileName, true);
                richTextBox1.Text += "COPY FINAL:" + StaticDestFileName + " \n";
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
                Application.DoEvents();
            }
            catch (Exception Ex)
            {
                richTextBox1.Text += Ex.Message + " \n";
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
                Application.DoEvents();
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
