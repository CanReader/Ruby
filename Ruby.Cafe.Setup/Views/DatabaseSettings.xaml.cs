using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ruby.Setup.Views
{
    /// <summary>
    /// Interaction logic for DatabaseSettings.xaml
    /// </summary>
    public partial class DatabaseSettings : Page
    {

        public String ConnectionString {
            get
            {
                if (DBTypeBox.SelectedIndex == 1) //Sql
                    if (!string.IsNullOrWhiteSpace(UserNameBox.Text) && !string.IsNullOrWhiteSpace(PasswordBox.Text))
                        return $"Server=tcp:{ServerNameBox.Text};Initial Catalog={DatabaseNameBox.Text};Persist Security Info=False;User ID={UserNameBox.Text};Password={PasswordBox.Text};MultipleActiveResultSets=True;Encrypt=True;Connection Timeout=30;";
                    else
                        return $"Server={ServerNameBox.Text};Database={DatabaseNameBox.Text};Trusted_Connection=true";

                else if (DBTypeBox.SelectedIndex == 2) //MySql
                    return $"server={ServerNameBox.Text};uid={UserNameBox.Text};pwd={PasswordBox.Text};database={DatabaseNameBox.Text}";

                else if (DBTypeBox.SelectedIndex == 3) //SqlLite
                    return $"Data Source={ServerNameBox.Text}; Version={UserNameBox.Text}";

                else if (DBTypeBox.SelectedIndex == 4) //Oracle
                    return "";
                else
                    return "";
            }
        }

        SetupWindow MainWindow;

        public DatabaseSettings(SetupWindow MainWindow)
        {
            InitializeComponent();

            this.Loaded += (ssender, ee) =>
            ClearUI();

            this.MainWindow = MainWindow;

            ServerNameBox.IsReadOnly = true;
            DatabaseNameBox.IsReadOnly = true;
            UserNameBox.IsReadOnly = true;
            PasswordBox.IsReadOnly = true;
        }

        private void ClearUI()
        {
            PageTitle.Text = Ruby.Resources.Localization.SetupScreen_DatabaseSettingsTitle;

            DBTypeBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultDatabaseTypeTxt;

            DefServerNameTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultServerNameTxt;
            DefDatabaseNameTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultDatabaseNameTxt;
            DefUserNameTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultUserNameTxt;
            DefPasswordTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultPasswordTxt;
            ConnectionStringTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt," ");
            NextBtn.Content = Ruby.Resources.Localization.SetupPage_DefaultNextBtnTxt;
            
        }

        public void NextPage(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            if (DBTypeBox.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(ServerNameBox.Text) && !string.IsNullOrWhiteSpace(DatabaseNameBox.Text) && !string.IsNullOrWhiteSpace(UserNameBox.Text) && !string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
            MainWindow.ConnectionString = ConnectionStringTxt.Text;
            MainWindow.DatabaseType = DBTypeBox.SelectedIndex;

            MainWindow.ServerName = ServerNameBox.Text;
            MainWindow.DatabaseName = DatabaseNameBox.Text;
            MainWindow.UserName = UserNameBox.Text;
            MainWindow.Password = PasswordBox.Text;


            MainWindow.Content = MainWindow.firsts;
            }
            else
            {
                //Do nothing
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ServerNameBox.Text) && !string.IsNullOrWhiteSpace(DatabaseNameBox.Text))
                ConnectionStringTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt,ConnectionString);
            else
                ConnectionStringTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt,"");
            
        }

        private void TypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DBTypeBox.SelectedIndex == -1 || DBTypeBox.SelectedIndex == 0)
            {
                ServerNameBox.IsReadOnly = true;
                DatabaseNameBox.IsReadOnly = true;
                UserNameBox.IsReadOnly = true;
                PasswordBox.IsReadOnly = true;

                DefServerNameTxt.Visibility = Visibility.Visible;
                DatabaseNameBox.Visibility = Visibility.Visible;
                DefUserNameTxt.Visibility = Visibility.Visible;
                DefPasswordTxt.Visibility = Visibility.Visible;

                ServerNameBox.Visibility = Visibility.Visible;
                DatabaseNameBox.Visibility = Visibility.Visible;
                UserNameBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Visible;
                ConnectionStringTxt.Visibility = Visibility.Visible;

                ServerNameBox.Text = "";
                DatabaseNameBox.Text = "";
                UserNameBox.Text = "";
                PasswordBox.Text = "";

                ConnectionStringTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt, "");
            }
            else if(DBTypeBox.SelectedIndex == 3)
            {
                DefServerNameTxt.Visibility = Visibility.Hidden;
                DefDatabaseNameTxt.Visibility = Visibility.Hidden;
                DefUserNameTxt.Visibility = Visibility.Hidden;
                DefPasswordTxt.Visibility = Visibility.Hidden;

                ServerNameBox.Visibility = Visibility.Hidden;
                DatabaseNameBox.Visibility = Visibility.Hidden;
                UserNameBox.Visibility = Visibility.Hidden;
                PasswordBox.Visibility = Visibility.Hidden;
                ConnectionStringTxt.Visibility = Visibility.Hidden;

                ServerNameBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/RubySoft/RubyCafe";
                UserNameBox.Text = "3";

                ConnectionStringTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt, ConnectionString);
            }
            else
            {
                ServerNameBox.IsReadOnly = false;
                DatabaseNameBox.IsReadOnly = false;
                UserNameBox.IsReadOnly = false;
                PasswordBox.IsReadOnly = false;

                DefServerNameTxt.Visibility = Visibility.Visible;
                DefDatabaseNameTxt.Visibility = Visibility.Visible;
                DefUserNameTxt.Visibility = Visibility.Visible;
                DefPasswordTxt.Visibility = Visibility.Visible;

                ServerNameBox.Visibility = Visibility.Visible;
                DatabaseNameBox.Visibility = Visibility.Visible;
                UserNameBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Visible;
                ConnectionStringTxt.Visibility = Visibility.Visible;

                ServerNameBox.Text = "";
                DatabaseNameBox.Text = "";
                UserNameBox.Text = "";
                PasswordBox.Text = "";

                ConnectionStringTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt, "");
            }
        }
    }
}
