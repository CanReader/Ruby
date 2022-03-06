using Ruby.Cafe.Database;
using Ruby.Serialization;
using Ruby.Setup.Model;
using Ruby.Setup.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ruby.Setup
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public GeneralSettings generals;
        public DatabaseSettings databases;
        public FirstEmployees firsts;

        public String MachineName { get; set; }
        public String OrganizationName { get; set; }
        
        public String ConnectionString { get; set; }
        public String ServerName { get; set; }
        public String DatabaseName { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }

        public bool Option1 { get; set; }
        public bool Option2 { get; set; }

        public double AWidth { get; set; }
        public double AHeight { get; set; }

        public int OrganizationType { get; set; }
        public int LanguageValue { get; set; }
        
        public int DatabaseType { get; set; }

        public List<Role> Roles;
        public List<Employee> Employees;

        public SetupWindow()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InstalledUICulture;
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InstalledUICulture;
        }

        public void FadeIn(int Second)
        {
            this.Dispatcher.Invoke(() => WelcomeText.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Second))));
            this.Dispatcher.Invoke(() => WelcomeText.Opacity = 0);
        }

        public void FadeOut(int Second)
        {
            this.Dispatcher.Invoke(() => WelcomeText.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Second))));
            this.Dispatcher.Invoke(() => WelcomeText.Opacity = 1);
        }

        public bool SaveSettings()
        {
            if(File.Exists("Settings.ini"))
                File.Delete("Settings.ini");

            Settings str = new Settings(false);

            str.MachineName = MachineName;
            str.StartWithMachine = Option1;
            str.CloseBtnOption = Option2;
            str.Width = AWidth;
            str.Height = AHeight;
            str.OrganizationName = OrganizationName;
            str.OrganizationType = OrganizationType;
            str.Language = LanguageValue;
            str.dbType = DatabaseType;

            str.SerializeSettings();

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft");

            DatabaseSetting ds = new DatabaseSetting(false);
          
            try
            {
                ds.ServerName = ServerName;
                ds.DatabaseName = DatabaseName;
                ds.UID = UserName;
                ds.Password = Password;

                ds.SerializeSettings();

                IDatabase db = ds.InitializeDB((Cafe.Database.DatabaseType)str.dbType);

                this.Dispatcher.Invoke(() =>
                {
                    foreach (var role in Roles)
                        db.AddRole(role.Name, role.Perms);

                    foreach (var emp in Employees)
                        db.AddEmployee(emp.Name, emp.Surname, emp.Gender, emp.StartDate, emp.Mail, emp.Phone, emp.Adress, emp.role.Name, emp.AuthCode);
                });

                ds.SerializeSettings();
            }
            catch (Exception e)
            {
                File.Delete("Settings.ini");
                this.Dispatcher.Invoke(() => this.Content =  databases);
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        private void PlayLastAnimation()
        {

        }

        private void WelcomeText_Loaded(object sender, RoutedEventArgs e)
        {

            #region Initialize pages
            generals = new GeneralSettings(this);
            databases = new DatabaseSettings(this);
            firsts = new FirstEmployees(this);
            #endregion

            new Thread(() =>
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InstalledUICulture;
                Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InstalledUICulture;

                Task t1 = new Task(() =>
                     {
                         this.Dispatcher.Invoke(() => WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_WelcomeText);
                         FadeOut(2);
                         Thread.Sleep(4000);
                         FadeIn(2);
                         Thread.Sleep(3000);
                     });

                     Task t2 = new Task(() =>
                     {
                         this.Dispatcher.Invoke(() => WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_Purpose);
                         FadeOut(2);
                         Thread.Sleep(4000);
                         FadeIn(2);
                         Thread.Sleep(3000);
                     });

                     Task t3 = new Task(() =>
                     {
                         this.Dispatcher.Invoke(() => WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_SettingSettigsMessage);
                         FadeOut(2);
                         Thread.Sleep(10000);
                         FadeIn(2);
                         Thread.Sleep(4000);
                     });

                     Task t4 = new Task(() =>
                {
                    this.Dispatcher.Invoke(() => WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_Redirecting);
                    FadeOut(2);
                    Thread.Sleep(7000);
                    FadeIn(2);
                    Thread.Sleep(6000);
                    this.Dispatcher.Invoke(() => this.Content = generals);
                     });

                     t1.RunSynchronously();
                t1.Wait();
                     t2.RunSynchronously();
                t2.Wait();
                     t3.RunSynchronously();
                t3.Wait();
                t4.RunSynchronously();
                t4.Wait();
                     
            }).Start();
        }
    }
}
