using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

using Ruby.Cafe.Model;

namespace Ruby.Cafe.Common.Screens
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
    public Ruby.Cafe.Common.History HistoryInstance;
    private Ruby.Cafe.Database.IDatabase db;

    public Model.Employee AcessedEmployee { get; set; }
    private string AccessCodeString => AcessCodeTextBox.Text;

    public List<Employee> Employees;

    private AdminScreen Panel;
    private MainPage PageMain;
    private TicketPage PageTicket;
    

        /// <summary>
        /// Constructs the login page which navigates to admin panel
        /// </summary>
        /// <param name="panel">Admin panel's instance</param>
        /// <param name="main">Main page instance</param>
        public LoginPage(AdminScreen panel,MainPage main, Database.IDatabase db)
        {
            InitializeComponent();

            LoginTitle.Text = Ruby.Resources.Localization.LoginPage_LoginTitle;
            AcessToPanelBtn.Content = Ruby.Resources.Localization.LoginPage_LoginBtnText;

            ErrorText.Visibility = Visibility.Hidden;

            this.Panel = panel;
            this.PageMain = main;
            this.db = db;

            this.AcessCodeTextBox.Text = Ruby.Resources.Localization.LoginPage_LoginTextBoxTxt;
        }

        
        /// <summary>
        /// Constructs the login page which navigates to ticket page
        /// </summary>
        /// <param name="main">Main page instance</param>
        public LoginPage(MainPage main, TicketPage page, Database.IDatabase db)
        {
            InitializeComponent();

            LoginTitle.Text = Ruby.Resources.Localization.LoginPage_LoginTitle;
            AcessToPanelBtn.Content = Ruby.Resources.Localization.LoginPage_LoginBtnText;

            ErrorText.Visibility = Visibility.Hidden;

            PageTicket = page;
            PageMain = main;
            this.db = db;

            this.AcessCodeTextBox.Text = Ruby.Resources.Localization.LoginPage_LoginTextBoxTxt;
        }

        /// <summary>
        /// Shows error text when an unknown login happens
        /// </summary>
        private void ShowErrorText()
        {
            //Show a animated label which is written Ruby.Resources.Localization.LoginPage_LoginError1
            HistoryInstance.SendMessage(ScreenEnum.LOGINPAGE, MessageType.ERROR, Ruby.Resources.Localization.LoginPage_LoginError1,null,null);
        }

        /// <summary>
        /// Shows error text when unauthorized tries to enter
        /// </summary>
        /// <param name="e">Employee who had tried to enter</param>
        private void ShowErrorText(Employee e)
        {
            //Show a animated label which is written Ruby.Resources.Localization.LoginPage_LoginError2
            HistoryInstance.SendMessage(ScreenEnum.LOGINPAGE, MessageType.ERROR, Ruby.Resources.Localization.LoginPage_LoginError2,null,null);
        }

        private void RedirectTo(object sender, RoutedEventArgs e)
        {
           Employees = db.GetEmployeeList(db.GetRoleList());

           AcessedEmployee = Employees.Find(emp => emp.AuthCode == AccessCodeString);

            if (AcessedEmployee == null)
            {
                AcessCodeTextBox.Text = "";
                ShowErrorText();
              return;
            }

            if (Panel != null)
            {

                if (AcessedEmployee.role.Perms[1])
                {
                Panel.AccessedEmployee = AcessedEmployee;
                NavigationService.Navigate(Panel);
                }
                else
                {
                    ShowErrorText(AcessedEmployee);
                    AcessCodeTextBox.Text = "";
                    return;
                }
            }
            else
            {
                PageTicket.AcessedEmployee = this.AcessedEmployee;
                NavigationService.Navigate(PageTicket);
            }
        }

        private void NumpadUsed(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AcessCodeTextBox.Text, out int result))
                AcessCodeTextBox.Text = "";

                if (AcessCodeTextBox.Text.Length >= 4)
                return;


                Button buton = sender as Button;

            StringBuilder codetext = new StringBuilder(AcessCodeTextBox.Text);

            codetext.Append(buton.Content);

            AcessCodeTextBox.Text = codetext.ToString();
        }

        private void DeleteChar(object sender, MouseEventArgs e)
        {
            if (AcessCodeTextBox.Text.Length == 0) return;
            StringBuilder text = new StringBuilder(AcessCodeTextBox.Text);

            if (!int.TryParse(AcessCodeTextBox.Text, out int result))
                return;

            text.Remove(text.Length-1,1);

            AcessCodeTextBox.Text = text.ToString();

            if (string.IsNullOrWhiteSpace(AcessCodeTextBox.Text))
                AcessCodeTextBox.Text = Ruby.Resources.Localization.LoginPage_LoginTextBoxTxt;
        }

        private void ReturnBack(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(PageMain);
        }
    }
}