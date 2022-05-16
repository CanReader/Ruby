using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Ruby.Cafe.Common.Screens
{
    /// <summary>
    /// History page (or HistoryManager) is the best spot to observe what happened in the business. The core reason of the application is making a good history management. This page presents the all analysis data to admin users.
    /// </summary>
    public partial class HistoryManager : Page
    {
        #region Variables

        private DateTime CurrentDateTime;

        public Database.IDatabase db;
        public Ruby.Cafe.Common.History HistoryInstance;
        public Employee AccessedEmployee;

        System.Windows.Threading.DispatcherTimer Timer;

        List<Employee> Employees;
        List<Table> Tables;
        List<Product> Products;

        #endregion

        #region Methods

        public HistoryManager(Database.IDatabase database, History HistoryInstance)
        {
            this.db = database;
            this.HistoryInstance = HistoryInstance;

            InitializeComponent();

            CurrentDateTime = DateTime.Now;

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        public void ClearUI()
        {
            HistoryViewer.Items.Clear();

            TotalSalesCard.PropertyName = Ruby.Resources.Localization.HistoryManager_TotalSalesDefaultText;
            ActiveTablesCard.PropertyName = Ruby.Resources.Localization.HistoryManager_ActiveTables;
            TotalSoldProducts.PropertyName = Ruby.Resources.Localization.HistoryManager_TotalSoldProducts;
            CriteriaCleaner.Content = Ruby.Resources.Localization.HistoryManager_CriteriaCleanerBtnTxt;

            TotalSalesCard.PropertyValue = db.GetTotalSales(CurrentDateTime).ToString();
            ActiveTablesCard.PropertyValue = db.GetTotalActiveTables(CurrentDateTime).ToString();
            TotalSoldProducts.PropertyValue = db.GetTotalSoldProducts(CurrentDateTime).ToString();

            Date.Text = CurrentDateTime.ToShortDateString();

            ClearCriteria();
        }

        private void ChangeCriteria()
        {
            Employee CriteriaEmployee = null;
            Table CriteriaTable = null;
            Product CriteriaProduct = null;
            int? MessageType = null;

            if (Dater.SelectedDate != null)
                CurrentDateTime = Dater.SelectedDate.Value;
            if (EmployeeCriteriaBox.SelectedIndex != -1 && EmployeeCriteriaBox.SelectedItem != null)
                CriteriaEmployee = (Employee)EmployeeCriteriaBox.SelectedItem;
            if (TableCriteriaBox.SelectedIndex != -1 && TableCriteriaBox.SelectedItem != null)
            CriteriaTable = (Table)TableCriteriaBox.SelectedItem;
            if (ProductCriteriaBox.SelectedIndex != -1 && ProductCriteriaBox.SelectedItem != null)
                CriteriaProduct = (Product)ProductCriteriaBox.SelectedItem;
            if (TypeCriteriaBox.SelectedIndex != -1 && TypeCriteriaBox.SelectedItem != null)
                MessageType = TypeCriteriaBox.SelectedIndex;

            HistoryViewer.Items.Clear();

            List<Message> Messages = HistoryInstance.Messages.FindAll(
                m =>
            m.MessageTime.ToShortDateString() == CurrentDateTime.ToShortDateString());

            if (CriteriaEmployee != null)
                Messages = Messages.FindAll(
                    m=> m.Sender != null &&
                    m.Sender.ID == CriteriaEmployee.ID);
            if(CriteriaTable != null)
                Messages = Messages.FindAll(
                    m => m.TableID != null &&
                    m.TableID == CriteriaTable.ID);
            if(CriteriaProduct != null)
                Messages = Messages.FindAll(
                    m => m.ProductID != null &&
                    m.ProductID == CriteriaProduct.ID);
            if(MessageType != null)
                Messages = Messages.FindAll(
                    m => m.ProductID != null &&
                    (int)m.Type == MessageType.Value);

            foreach (var item in Messages)
            HistoryViewer.Items.Add(item);
        }

        private void ClearCriteria()
        {
            CurrentDateTime = DateTime.Now;
            Dater.SelectedDate = DateTime.Now;

            EmployeeCriteriaBox.SelectedIndex = -1;
            TableCriteriaBox.SelectedIndex = -1;
            ProductCriteriaBox.SelectedIndex = -1;

            EmployeeCriteriaBox.Text = Ruby.Resources.Localization.HistoryManager_DefaultSelectEmployeeTxt;
            TableCriteriaBox.Text = Ruby.Resources.Localization.HistoryManager_DefaultSelectTableTxt;
            ProductCriteriaBox.Text = Ruby.Resources.Localization.HistoryManager_DefaultSelectProductTxt;
        }

        #endregion

        #region Events

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClearUI();

            ChangeCriteria();

            this.HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.HISTORYMANAGER, MessageType.NOTIFICATION, Ruby.Resources.Localization.NOTIF_AccessedToScreen);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Clock.Text = DateTime.Now.ToLongTimeString();
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearUI();
        }

        private void EmployeeCriteriaBox_DropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox)sender).Items.Clear();

            foreach(var item in Employees)
            ((ComboBox)sender).Items.Add(item);

        }

        private void TableCriteriaBox_DropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox)sender).Items.Clear();

            foreach (var item in Tables)
                ((ComboBox)sender).Items.Add(item);
        }

        private void ProductCriteriaBox_DropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox)sender).Items.Clear();

            foreach (var item in Products)
                ((ComboBox)sender).Items.Add(item);
        }

        private void TypeCriteriaBox_DropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox)sender).Items.Clear();

            if(System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("tr"))
            foreach (var item in Enum.GetValues(typeof(MessageTypeTR)))
                ((ComboBox)sender).Items.Add(item);
            else if(System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("fr"))
                foreach (var item in Enum.GetValues(typeof(MessageTypeFR)))
                    ((ComboBox)sender).Items.Add(item);
            else if(System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("ger"))
                foreach (var item in Enum.GetValues(typeof(MessageTypeGER)))
                    ((ComboBox)sender).Items.Add(item);
            else
                foreach (var item in Enum.GetValues(typeof(MessageType)))
                    ((ComboBox)sender).Items.Add(item);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearCriteria();
        }

        private void CriteriaChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeCriteria();
            Date.Text = CurrentDateTime.ToShortDateString();
        }

        #endregion
    }
}
