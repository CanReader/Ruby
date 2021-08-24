using Ruby.Cafe.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Ruby.Cafe.Common.Screens
{
    public partial class SettingsPage : Page
    {
        #region Variables

        public static readonly String AdissionPath = "AdissionPage.xaml";

        private Ruby.Cafe.Common.History HistoryInstance;
        public Serialization.Settings Str;
        public Serialization.DatabaseSetting DBStr;
        public Employee AccessedEmployee;

        public bool Saved = true;
        public bool Error = false;

        private String ConnectionString
        {
            get
            {
                return ConnectionStringBlockTxt.Text.Split('=')[1];
            }

            set
            {
                ConnectionStringBlockTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt, value);
            }
        }

        // Database Properties
        private String ServerName { get; set; }
        private String DatabaseName { get; set; }
        private String UserName { get; set; }
        private String Password { get; set; }

        #endregion

        #region Methods

        public SettingsPage(History HistoryInstance)
        {
            this.HistoryInstance = HistoryInstance;

            InitializeComponent();

            this.Str = new Serialization.Settings(true);
            DBStr = new Serialization.DatabaseSetting(true);
        }

        public void ClearUI()
        {

            DefaultAdissionPageViewer.Text = Ruby.Resources.Localization.SettingsPage_DefaultAdissionPageTxt;
            ImportAdissionBtn.Content = Ruby.Resources.Localization.SettingsPage_ImportAdissionBtnTxt;
            DefaultGeneralSettingsTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultGeneralSettingsTxt;
            DefaultOrganizationSettingsTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultOrganizationSettingsTxt;
            DefaultDatabaseSettingsTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultDatabaseSettingsTxt;
            ConnectionStringBlockTxt.Text = string.Format(Ruby.Resources.Localization.SettingsPage_DefaultConnectionStringTxt, ConnectionString);
            MachineNameBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultMachineNameTxt;
            LanguageBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultLanguagrTxt;
            StartOption.Content = Ruby.Resources.Localization.SettingsPage_DefaultStartWithMachineTxt;
            CloseBtnOption.Content = Ruby.Resources.Localization.SettingsPage_DefaultShowBtnOptionTxt;
            WidthBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultWidthTxt;
            HeightBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultHeightTxt;
            OrgNameBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultOrganizationNameTxt;
            OrgTypeBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultOrganizationTypeTxt;
            DatabaseTypeBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultDatabaseTypeTxt;
            ServerNameBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultServerNameTxt;
            DatabaseNameBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultDatabaseNameTxt;
            UserNameBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultUserNameTxt;
            PasswordBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultPasswordTxt;
            CheckConnectionBtn.Content = Ruby.Resources.Localization.SettingsPage_DefaultCheckConnectionBtnTxt;
            PrintPageBtn.Content = Ruby.Resources.Localization.SettingsPage_PrintSampleBtnTxt;
            AdissionCode.Text = " ";
        }

        public Serialization.Settings GetSettings()
        {
            Serialization.Settings sets = new Serialization.Settings(false);

            sets.MachineName = MachineNameBox.Text;
            sets.Language = LanguageBox.SelectedIndex;
            sets.StartWithMachine = StartOption.IsChecked.Value;
            sets.CloseBtnOption = CloseBtnOption.IsChecked.Value;
            sets.Width = double.Parse(WidthBox.Text);
            sets.Height = double.Parse(HeightBox.Text);
            sets.OrganizationName = OrgNameBox.Text;
            sets.OrganizationType = OrgTypeBox.SelectedIndex;
            sets.dbType = DatabaseTypeBox.SelectedIndex;

            return sets;
        }

        public void SetSettings()
        {
            MachineNameBox.Text = Str.MachineName;
            LanguageBox.SelectedIndex = Str.Language;
            StartOption.IsChecked = Str.StartWithMachine;
            CloseBtnOption.IsChecked = Str.CloseBtnOption;
            WidthBox.Text = Str.Width.ToString();
            HeightBox.Text = Str.Height.ToString();
            OrgNameBox.Text = Str.OrganizationName;
            OrgTypeBox.SelectedIndex = Str.OrganizationType;
            DatabaseTypeBox.SelectedIndex = Str.dbType;
        }

        public Serialization.DatabaseSetting GetDatabaseSettings()
        {
            Serialization.DatabaseSetting ds = new Serialization.DatabaseSetting(true);
            ds.ServerName = ServerName;
            ds.DatabaseName = DatabaseName;
            ds.UID = UserName;
            ds.Password = Password;

            return ds;
        }

        public void SetDatabaseSettings()
        {
            ConnectionString = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.dat");
            ServerNameBox.Text = DBStr.ServerName;
            DatabaseNameBox.Text = DBStr.DatabaseName;
            UserNameBox.Text = DBStr.UID;
            PasswordBox.Text = DBStr.Password;
        }

        public void ViewPage(String FilePath)
        {
            Error = false;
            String Code = "";

            if (string.IsNullOrWhiteSpace(FilePath))
            {
                if (string.IsNullOrWhiteSpace(AdissionCode.Text))
                    return;

                Code = AdissionCode.Text;
            }
            else
                Code = System.IO.File.ReadAllText(FilePath);

            System.IO.StringReader stringReader = new System.IO.StringReader("<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" + Code + "</FlowDocument>");

            System.Xml.XmlReader xmlReader = System.Xml.XmlTextReader.Create(stringReader, new System.Xml.XmlReaderSettings());

            try
            {
                AdissionViewer.Document = System.Windows.Markup.XamlReader.Load(xmlReader) as FlowDocument;
            }
            catch (System.Windows.Markup.XamlParseException ex)
            {
                var fd = new FlowDocument();
                var p = new Paragraph();
                p.Inlines.Add(ex.Message.ToString());
                fd.Blocks.Add(p);

                AdissionViewer.Document = fd;

                Error = true;
            }
        }

        public void SavePage(String FilePath)
        {
            if (System.IO.File.Exists(FilePath))
                System.IO.File.Delete(FilePath);

            System.IO.File.Create(FilePath).Close();

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(FilePath))
                fs.Write(AdissionCode.Text); ;
        }

        #endregion

        #region Events

        private void PrintSample(object sender, RoutedEventArgs e)
        {

        }

        private void Import(object sender, RoutedEventArgs e)
        {
            String code = "";

            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
            fd.Filter = "XAML Files|*.xaml";
            fd.FilterIndex = 0;
            if (fd.ShowDialog() == DialogResult.OK)
                code = System.IO.File.ReadAllText(fd.FileName);

                System.IO.StringReader sr = new System.IO.StringReader("<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" + code + "</FlowDocument>");
                System.Xml.XmlReader xmlReader = System.Xml.XmlTextReader.Create(sr, new System.Xml.XmlReaderSettings());

                try{AdissionViewer.Document = System.Windows.Markup.XamlReader.Load(xmlReader) as FlowDocument;
                }
            catch (XamlParseException){return;}

            AdissionCode.Text = code;
        }

        private void CheckConnection(object sender, RoutedEventArgs e)
        {
            switch ((Database.DatabaseType)DatabaseTypeBox.SelectedIndex)
            {
                case Database.DatabaseType.None:
                    break;
                case Database.DatabaseType.Sql:
                    try
                    {
                        DBStr.InitializeDB((Database.DatabaseType)DatabaseTypeBox.SelectedIndex);
                    }
                    catch (Exception)
                    {
   Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_FailedConnectionTitle, Ruby.Resources.Localization.MB_FailedConnection,MessageBoxButton.OK,MessageBoxImage.Error);
                        break;
                    }
                    Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_SuccessfullConnectionTitle, Ruby.Resources.Localization.MB_SuccessfullConnection, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case Database.DatabaseType.MySql:
                    break;
                case Database.DatabaseType.Sqlite:
                    break;
                case Database.DatabaseType.Oracle:
                    break;
                default:
                    break;
            }
        }

        private void DSChanged(object sender, TextChangedEventArgs e)
        {
            Saved = false;

            UserName = "";
            Password = "";

            try
            {

                ServerName = ServerNameBox.Text;
                DatabaseName = DatabaseNameBox.Text;
                if (!string.IsNullOrWhiteSpace(UserNameBox.Text) && !UserNameBox.Text.Contains(Ruby.Resources.Localization.SettingsPage_DefaultUserNameTxt))
                    UserName = UserNameBox.Text;
                if (!string.IsNullOrWhiteSpace(PasswordBox.Text) && !PasswordBox.Text.Contains(Ruby.Resources.Localization.SettingsPage_DefaultPasswordTxt))
                    Password = PasswordBox.Text;


                if (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(UserName))
                    ConnectionString = string.Format("Server={0};Database={1};User ID={2};Password={3};Trusted_Connection=True;Encrypt=True;", ServerName, DatabaseName, UserName, Password);
                else
                    ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Connect Timeout=45;Encrypt=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", ServerName, DatabaseName, UserName, Password);
            }
            catch (Exception) { }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            Saved = false;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Saved = false;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClearUI();

            Str = new Serialization.Settings(true);
            DBStr = new Serialization.DatabaseSetting(true);

            SetSettings();
            SetDatabaseSettings();

            HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.SETTINGS, MessageType.NOTIFICATION, Ruby.Resources.Localization.NOTIF_AcessedToTable);

            if (System.IO.File.Exists("AdissionPage.xaml"))
            {
                AdissionCode.Text = System.IO.File.ReadAllText("AdissionPage.xaml");
                ViewPage("AdissionPage.xaml");
            }
        }

        private void AdissionCodeHasChanged(object sender, TextChangedEventArgs e)
        {
            {
                if (string.IsNullOrWhiteSpace(AdissionCode.Text))
                    return;

                ViewPage("");
            }

            #endregion
        }
    }
}