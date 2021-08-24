using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ruby.Cafe.Common.Controls
{
    [System.Windows.Markup.ContentPropertyAttribute("Children")]
    public partial class MB_ExceptionBox : Window
    {
        private String[] ExceptionString;
        private Dictionary<String,String> SystemInfo;
        private Dictionary<String,String> Versions;

        private String LogString;

        private bool Clicked = false;

        public MB_ExceptionBox(string Caption, String Message, Exception e)
        {
            InitializeComponent();

            OkayButton.Content = Ruby.Resources.Localization.MB_OKAY;

            this.MouseDown += (ssender, ee) =>
                 this.DragMove();

            SystemInfo = new Dictionary<String, String>();

            SystemInfo.Add("Date: ", DateTime.Now.ToLongDateString());
            SystemInfo.Add("Machine Name: ", System.Windows.Forms.SystemInformation.ComputerName);
            SystemInfo.Add("Operating System: ", System.Environment.OSVersion.VersionString);
            SystemInfo.Add("Network Status: ", System.Windows.Forms.SystemInformation.Network.ToString());
            SystemInfo.Add("Is Windows pen aviable: ", System.Windows.Forms.SystemInformation.PenWindows.ToString());
            SystemInfo.Add("Power Status: ", System.Windows.Forms.SystemInformation.PowerStatus.BatteryChargeStatus.ToString() + " " + System.Windows.Forms.SystemInformation.PowerStatus.BatteryFullLifetime + " " + System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifePercent + " " + System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus);

            Versions = new Dictionary<string, string>();

            foreach (var p in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory()).Where(a => a.Contains(".dll")))
            {
                string Version = System.Diagnostics.FileVersionInfo.GetVersionInfo(p).FileVersion;

                Versions.Add(System.IO.Path.GetFileNameWithoutExtension(p),Version);
            }

            ExceptionString = new string[5];

            ExceptionString[0] = e.Message;
            ExceptionString[1] = e.HResult.ToString();
           if(e.InnerException != null) ExceptionString[2] = e.InnerException.ToString();
            ExceptionString[3] = e.TargetSite.ToString();
            ExceptionString[4] = e.StackTrace;

            if (string.IsNullOrEmpty(Caption))
                throw new Exception("Caption cannot be null");
            if (Message == null) Message = " ";
            this.Visibility = Visibility.Visible;

            ExceptionName.Text = Caption;

            ExceptionMessage.Text = Message;

            for (int i = 0; i < ExceptionString.Length; i++)
            {
                string ExceptionText = ExceptionMessage.Text + Environment.NewLine;
                ExceptionMessage.Text = ExceptionText + ExceptionString[i];
            }
        }

        private string GenerateLogFileName(DateTime date)
        {
            return "log-" + date.Hour + date.Minute + date.Day + date.Year.ToString().Substring(0,2) + date.Month;
        }

        private void Okay(object sender, RoutedEventArgs e)
        {
            string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +"/RubySoft/RubyCafe/" + GenerateLogFileName(DateTime.Now) + ".txt";

            List<string> FileStrings = new List<string>();

            var stream = new System.IO.FileStream(FolderPath, System.IO.FileMode.OpenOrCreate);

                System.IO.StreamWriter sw = new System.IO.StreamWriter(stream);

                sw.WriteLine("[System Info]");
                foreach (var p in SystemInfo) sw.WriteLine(p.Key + ": " + p.Value);
                sw.WriteLine();
                sw.WriteLine("[Assembly Versions]");
                foreach (var p in Versions)sw.WriteLine(p.Key + ": " + p.Value);
                sw.WriteLine("[Exception]");
                for (int i = 0; i < ExceptionString.Length - 1; i++) { sw.WriteLine(ExceptionString[i]); sw.WriteLine(""); }
                sw.WriteLine();

            Clicked = true;

                sw.Close();
                sw.Dispose();

            this.DialogResult = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Clicked)
                e.Cancel = true;
            else
                e.Cancel = false;
        }
    }
}
