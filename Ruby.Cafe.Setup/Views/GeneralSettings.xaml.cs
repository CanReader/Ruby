using System;
using System.Windows.Controls;

namespace Ruby.Setup.Views
{
    /// <summary>
    /// Interaction logic for GeneralSettings.xaml
    /// </summary>
    public partial class GeneralSettings : Page
    {
        SetupWindow MainWindow;

        public GeneralSettings(SetupWindow MainWindow)
        {
            InitializeComponent();

            this.MainWindow = MainWindow;

            this.Loaded += (ssender, ee) =>
            ClearUI();
            LanguageBox.SelectionChanged += (ssender, ee) =>
            {
                if (LanguageBox.SelectedItem == null || LanguageBox.SelectedIndex == -1)
                    return;
                else if (LanguageBox.SelectedIndex == 0)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    ClearUI();
                    LanguageBox.Text = "English";
                }
                else if (LanguageBox.SelectedIndex == 1)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr");
                    ClearUI();
                    LanguageBox.Text = "Türkçe";
                }
                else if (LanguageBox.SelectedIndex == 2)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
                    ClearUI();
                    LanguageBox.Text = "Français";
                }
                else if (LanguageBox.SelectedIndex == 3)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ger");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ger");
                    ClearUI();
                    LanguageBox.Text = "Deutsche";
                }

            };
        }

        private void ClearUI()
        {
            PageTitle.Text = Ruby.Resources.Localization.SetupScreen_GeneralSettingsTitle;

            MachNameDefTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultMachineNameTxt;   
            Option1Box.Content = Ruby.Resources.Localization.SettingsPage_DefaultStartWithMachineTxt;   
            Option2Box.Content = Ruby.Resources.Localization.SettingsPage_DefaultShowBtnOptionTxt; Option2Box.IsChecked = true;
            DefWidthTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultWidthTxt;
            DefHeightTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultHeightTxt;
            DefOrgNameTxt.Text = Ruby.Resources.Localization.SettingsPage_DefaultOrganizationNameTxt;
            LanguageBox.Text = Ruby.Resources.Localization.SettingsPage_DefaultLanguagrTxt;

            NextBtn.Content = Ruby.Resources.Localization.SetupPage_DefaultNextBtnTxt;

            MachNameBox.Text = Environment.MachineName;
            Option1Box.IsChecked = true;

            WidthBox.Text = System.Windows.SystemParameters.PrimaryScreenWidth.ToString();
            HeightBox.Text = System.Windows.SystemParameters.PrimaryScreenHeight.ToString();
        }

        private void NextPage(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MachNameBox.Text) || string.IsNullOrWhiteSpace(OrgNameBox.Text) || string.IsNullOrWhiteSpace(WidthBox.Text) || string.IsNullOrWhiteSpace(HeightBox.Text) || OrgTypeBox.SelectedIndex == -1 || LanguageBox.SelectedIndex == -1)
                return;

            System.Threading.Thread.Sleep(2000);

            try
            {
                MainWindow.AWidth = double.Parse(WidthBox.Text);
                MainWindow.AHeight = double.Parse(HeightBox.Text);
            }
            catch (Exception ee) { return; }
            MainWindow.MachineName = MachNameBox.Text;
            MainWindow.Option1 = Option1Box.IsChecked.Value;
            MainWindow.Option2 = Option2Box.IsChecked.Value;
            MainWindow.OrganizationName = OrgNameBox.Text;
            MainWindow.OrganizationType = OrgTypeBox.SelectedIndex;
            MainWindow.LanguageValue = LanguageBox.SelectedIndex;

            if (LanguageBox.SelectedIndex == 0)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }
            else if (LanguageBox.SelectedIndex == 1)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr");
            }
            else if (LanguageBox.SelectedIndex == 2)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            }
            else
            {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ger");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ger");
            }

            MainWindow.Content = MainWindow.databases;
        }
    }
}
