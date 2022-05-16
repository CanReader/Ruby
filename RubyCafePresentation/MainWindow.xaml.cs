using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Ruby.Cafe.Common;
using Ruby.Cafe.Common.Screens;
using Ruby.Cafe.Database;

namespace RubyCafePresentation
{
#pragma warning disable 0168, 0649

    public partial class MainWindow : NavigationWindow
    {
        private History HistoryInstance;
    
        public IDatabase Dbase = null;
        private MainPage mp = null;
        
        /// <summary>
        /// Checks connection of the Internet
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckConnection()
        {
            try
            {
                using (var checker = new System.Net.WebClient())
                using (checker.OpenRead((@"http://google.com"))) { 
                Console.WriteLine("Internet connection has been successfully established!");
                return true;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Internet connection could not be established, Please check your router.");
                return false;
            }
        }

        /// <summary>
        /// Loads dll to check their version so that it will upgrade the application if is there a different version than current's
        /// </summary>
        /// <param name="AssemblyName"></param>
        /// <param name="DownloadPath"></param>
        /// <returns>bool</returns>
        private bool CheckVersion(string AssemblyPath, string DownloadPath)
        {
            if (string.IsNullOrEmpty(AssemblyPath)) return false;

            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                var CurrentVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(AssemblyPath);

                return true;
            }

            catch (System.IO.FileNotFoundException FNFE)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowExceptionBox("Failed to find the specific file", " ", FNFE);
                return false;
            }
            catch (System.IO.PathTooLongException PTLE)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowExceptionBox("Failed to find file", " ", PTLE);
                return false;
            }
        }

        public MainWindow()
        {
            HistoryInstance = new History();

            Ruby.Serialization.Settings setter = null;

            String MachineName = "";

            //Check if it should enter in setup mode or not
            if (System.IO.File.Exists("Settings.ini") && 
                System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.dat"))
            {
                setter = new Ruby.Serialization.Settings(true);

                var Version = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

                Console.WriteLine("--------------------------------------------- Welcome to the RubySoft ---------------------------------------------");
                Console.WriteLine("Application name: " + "RubyCafe");
                Console.WriteLine("Current version: " + Version.ProductVersion);

#if DEBUG
                Console.WriteLine("Current working mode: DEBUG mode");
#elif BETA
            Console.WriteLine("Current working mode: BETA mode");
            // Nothing to do
#else
            Console.WriteLine("Current working mode: RELEASE mode");
#endif

                //Set the language for usage
                switch (setter.Language)
                {
                    case 0:
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                        break;
                    case 1:
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");
                        break;
                    case 2:
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
                        break;
                    case 3:
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ger-GER");
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ger-GER");
                        break;
                    default:
                        break;
                }
                Ruby.Resources.Localization.Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                
                Console.WriteLine("Attempting to connect the database, Database Type = " + ((DatabaseType)setter.dbType).ToString());

                //Connect to the database, show the error if an occurs
                try
                {
                    Dbase = new Ruby.Serialization.DatabaseSetting(true).InitializeDB(Ruby.Serialization.DatabaseSetting.type);
                }
                catch (Exception e)
                {
                    Ruby.Serialization.DatabaseSetting ds = new Ruby.Serialization.DatabaseSetting(true);

                    Ruby.Cafe.Common.Controls.MessageBox.ShowExceptionBox(Ruby.Resources.Localization.EXC_FailToConnectDatabaseTitle, Ruby.Resources.Localization.EXC_FailToConnectDatabaseMessage 
                        + Environment.NewLine + "Server Name = " + ds.ServerName 
                        + Environment.NewLine + "Database = " + ds.DatabaseName
                        + Environment.NewLine + "User Name = " + ds.UID
                        + Environment.NewLine + "Password = " + ds.Password,
                        e);
                    Application.Current.Shutdown();
                    return;
                }
       
                mp = new MainPage(Dbase,HistoryInstance);

                this.Navigate(mp);

            HistoryInstance.SendMessage(Ruby.Cafe.Model.ScreenEnum.MAINPAGE, Ruby.Cafe.Model.MessageType.NOTIFICATION, string.Format(Ruby.Resources.Localization.NOTIF_OpenedApp, MachineName));
            }
            else
            {
                Ruby.Setup.SetupWindow sw = new Ruby.Setup.SetupWindow();
                sw.Activate();
                sw.Show();

                this.Close();
            }

            //Release settings class
            if(setter != null)
            setter.Dispose();

        }

        private void NavigationWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}