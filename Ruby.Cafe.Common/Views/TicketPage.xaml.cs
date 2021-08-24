using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Ruby.Cafe.Common.Screens
{
    public partial class TicketPage : Page
    {
        #region Variables

        private Model.Ticket ticket;

        private TextBox ActiveTextBox;

        private Ruby.Cafe.Model.Table table;
        public Ruby.Cafe.Model.Employee AcessedEmployee;

        private Database.IDatabase db;

        private List<Model.Category> Categories;
        private List<Model.Product> Products;
        private List<Model.Product> ActiveProducts;

        private MainPage mp;

        private bool TicketExists = false;

        private decimal _TotalPrice;
        private decimal TotalPrice
        {
            get
            {
                return _TotalPrice;
            }
            set
            {
                PriceBlock.Text = value.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
                _TotalPrice = value;
            }
        }

        #endregion

        #region Methods

        public TicketPage(MainPage mainpage, Ruby.Cafe.Model.Table table, Database.IDatabase db)
        {
            this.table = table;
            this.db = db;
            this.mp = mainpage;

            InitializeComponent();

            ActiveTextBox = MultiplierBox;
        }

        private void ClearUI()
        {
            MenuListTxt.Text = Ruby.Resources.Localization.TicketPage_DefaultMenuListTxt;
            TableNameTxt.Text = string.Format(Ruby.Resources.Localization.TicketPage_DefaultTableNameTxt, table.Name);
            TotalPriceTxt.Text = Ruby.Resources.Localization.TicketPage_DefaultTotalPriceTxt;
            MultiplierBox.Text = Ruby.Resources.Localization.TicketPage_DefaultMultiplierBoxTxt;
            ServingBox.Text = Ruby.Resources.Localization.TicketPage_DefaultServingBoxTxt;
            PriceBox.Text = Ruby.Resources.Localization.TicketPage_DefaultPriceBoxTxt;
            CashBtn.Content = Ruby.Resources.Localization.TicketPage_DefaultCashBtnTxt;
            CreditBtn.Content = Ruby.Resources.Localization.TicketPage_DefaultCreditBtnTxt;
            PrintBtn.Content = Ruby.Resources.Localization.TicketPage_DefaultPrintTicketTxt;
            CloseTableBtn.Content = Ruby.Resources.Localization.TicketPage_DefaultCloseTableBtnTxt;
            TurnBackBtn.Content = Ruby.Resources.Localization.TicketPage_DefaultTurnBackBtnTxt;
            EmployeeNameTxt.Text = AcessedEmployee.Name + " " + AcessedEmployee.Surname;
        }

        private void CalculatePrice()
        {
            TotalPrice = 0.0m;
            if (ActiveProductLister.Items.Count > 0)
                foreach (var item in ActiveProductLister.Items)
                    TotalPrice += ((Model.Product)item).CurrentServing.Price * ((Model.Product)item).Multiplier;
        }

        private void RefreshMenu()
        {
            Menu.Items.Clear();

            foreach (var category in Categories)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = category };

                foreach (var product in Products.Where(p => p.category.ID == category.ID))
                {
                    tvi.Items.Add(product);
                }

                Menu.Items.Add(tvi);

            }
        }

        private void RefreshActiveProductList()
        {
            if (ActiveProducts.Count < 1) return;

            ActiveProductLister.Items.Clear();
            TotalPrice = 0.0m;

            foreach (var product in ActiveProducts)
            {
                ActiveProductLister.Items.Add(product);
            }

            CalculatePrice();
        }

        private void AddActiveProduct(Product product, int Multiplier)
        {
            ServingBox.Items.Clear();
            foreach (var serving in product.Servings)
                ServingBox.Items.Add(serving);

            ServingBox.SelectedItem = ServingBox.Items[0];

            product.CurrentServing = product.Servings[0];

            int index = -1;

            //Find product if exists in the listbox
            index = ActiveProducts.FindIndex(p => p.ID == product.ID);

            //If product exists then add multiplier
            if (index != -1)
            {
                ActiveProducts[index].Multiplier += Multiplier;
                RefreshActiveProductList();
            }
            //Otherwise add product to listbox and list
            else
            {
           product.Multiplier = Multiplier;
           ActiveProductLister.Items.Add(product);
           ActiveProducts.Add(product);
            }

            CalculatePrice();
        }

        private bool ChangeTable()
        {
            bool value = true;

            Window window = new Window();
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowInTaskbar = false;
            window.WindowState = WindowState.Normal;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowStyle = WindowStyle.None;

            using (Ruby.Serialization.Settings settr = new Serialization.Settings(true))
            {
                window.Width = 3 * settr.Width / 4;
                window.Height = settr.Height / 2;
            }

            ListBox listbox = new ListBox();
            listbox.FontSize = 43;
            listbox.HorizontalContentAlignment = HorizontalAlignment.Center;
            listbox.VerticalContentAlignment = VerticalAlignment.Center;
            listbox.ItemsSource = db.GetTableList(db.GetScenceList());
            listbox.SelectionChanged += (ssender, sce)
                =>
            { //||\\ 324
                if (listbox.SelectedItem != null && ((Table)listbox.SelectedItem).Name != table.Name)
                {
                    db.UpdateTicket(table, (Table)listbox.SelectedItem);
                    window.Close();
                }
                else
                {
                    window.Close();
                    value = false;
                }
            };

            window.Content = listbox;

            window.ShowDialog();

            return true;
        }

        private void CreateTicket(Ticket ticket,Table table)
        {
            if (!TicketExists)
            {
                db.AddTicket(table, AcessedEmployee, DateTime.Now);

                    ticket = db.GetTicket(table);

                foreach (Product item in ActiveProductLister.Items)
                {
                    db.AddTicketProduct(ticket.ID, item, item.Servings.IndexOf(item.CurrentServing), item.Multiplier);
                }

                mp.HistoryInstance.SendMessage(AcessedEmployee, ScreenEnum.TICKETPAGE, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_TicketCreated, table.Name));
            }
            else
            {
                if (ticket == null)
                    ticket = db.GetTicket(table);

                db.DeleteTicketProducts(ticket.ID);

                foreach (Product item in ActiveProductLister.Items)
               db.AddTicketProduct(ticket.ID, item, item.Servings.IndexOf(item.CurrentServing), item.Multiplier);
                
            }
                NavigationService.Navigate(mp);
        }

        private void DeleteTicket(Table table)
        {
            ticket = db.GetTicket(table);

            db.DeleteTicket(table);

            db.UpdateTable(table.Name,table.TableScence,table.TableType,table.TableColor,table.MaxChair,table.ConvertBoolArrayToString(table.Checks),0,table.Description,table.Name);

            foreach (Product product in ActiveProductLister.Items)
                db.DeleteTicketProducts(ticket.ID);
        }

        #endregion

        #region Events

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AcessedEmployee == null)
            {
                NavigationService.Navigate(new Common.Screens.LoginPage(mp, this, db));
                return;
            }

            ClearUI();

            Categories = db.GetCategoryList();
            Products = db.GetProductList(Categories);

            RefreshMenu();

            Model.Ticket ticket = db.GetTicket(table);

            if (ticket != null)
            {
                TicketExists = true;
                ActiveProducts = db.GetTicketProducts(ticket.ID, Products);
                RefreshActiveProductList();
            }
            else
            {
          ticket = new Model.Ticket();
          ActiveProducts = new List<Model.Product>();
            }

            mp.HistoryInstance.SendMessage(AcessedEmployee, Model.ScreenEnum.TICKETPAGE, Model.MessageType.NOTIFICATION, string.Format(Ruby.Resources.Localization.NOTIF_AcessedToTable, table.Name),null,table.ID);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Menu.Items.Clear();
            ActiveProductLister.Items.Clear();
        }

        private void KeyPress(object sender, RoutedEventArgs e)
        {
            if (ActiveTextBox == null) return;
            if (ActiveTextBox == MultiplierBox && ((Button)sender).Content.ToString() == ".") return;

            if (ActiveTextBox.Text.Contains(Ruby.Resources.Localization.TicketPage_DefaultPriceBoxTxt) || ActiveTextBox.Text.Contains(Ruby.Resources.Localization.TicketPage_DefaultMultiplierBoxTxt))
            {
                oldtext = ActiveTextBox.Text;
                ActiveTextBox.Text = "";
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder(ActiveTextBox.Text);
            sb.Append(((Button)sender).Content.ToString());

            ActiveTextBox.Text = sb.ToString();
        }

        private void DeleteChar(object sender, MouseButtonEventArgs e)
        {
            if (ActiveTextBox == null) return;

            if (!string.IsNullOrWhiteSpace(ActiveTextBox.Text) && ActiveTextBox.Text.Length != 0)
                ActiveTextBox.Text = ActiveTextBox.Text.Remove(ActiveTextBox.Text.Length - 1, 1);
        }

        private void RemoveActiveProduct(object sender, RoutedEventArgs e)
        {
            if (!AcessedEmployee.role.Perms[8])
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoPermissionToProcessTitle, Ruby.Resources.Localization.MB_NoPermissionToProcessMessage, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (ActiveProductLister.SelectedItem == null)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_ItemIsNotSelectedTitle, Ruby.Resources.Localization.MB_ItemIsNotSelectedMessage, MessageBoxButton.OK, MessageBoxImage.None);
                return;
            }

            Product p = (Product)ActiveProductLister.SelectedItem;

            ActiveProducts.Remove(p);
            ActiveProductLister.Items.Clear();
            foreach (var item in ActiveProducts) ActiveProductLister.Items.Add(item);
            CalculatePrice();
        }

        private string oldtext;
        private void FocusOn(object sender, RoutedEventArgs e)
        {
        }

        private void FocusOff(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ActiveTextBox.Text))
                ActiveTextBox.Text = oldtext;
        }

        private void PayAccount(object sender, RoutedEventArgs e)
        {
            if (!AcessedEmployee.role.Perms[14])
            {               Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoPermissionToProcessTitle, Ruby.Resources.Localization.MB_NoPermissionToProcessMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ChangeTable(object sender, RoutedEventArgs e)
        {
            if (!AcessedEmployee.role.Perms[10])
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoPermissionToProcessTitle, Ruby.Resources.Localization.MB_NoPermissionToProcessMessage, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (ChangeTable())
            {
            mp.HistoryInstance.SendMessage(AcessedEmployee, ScreenEnum.TICKETPAGE, MessageType.NOTIFICATION, string.Format(Ruby.Resources.Localization.NOTIF_TableChanged, this.table.Name, table.Name));

            NavigationService.Navigate(mp);
            }
        }

        private void PrintTicket(object sender, RoutedEventArgs e)
        {
            if (!AcessedEmployee.role.Perms[9])
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoPermissionToProcessTitle, Ruby.Resources.Localization.MB_NoPermissionToProcessMessage, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

        }

        private void CloseTable(object sender, RoutedEventArgs e)
        {
            if (!TicketExists) return;

            if (!AcessedEmployee.role.Perms[12])
            {               Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoPermissionToProcessTitle, Ruby.Resources.Localization.MB_NoPermissionToProcessMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DeleteTicket(table);

            mp.HistoryInstance.SendMessage(AcessedEmployee,ScreenEnum.TICKETPAGE,MessageType.NOTIFICATION,Ruby.Resources.Localization.NOTIF_TicketDeleted);

            this.NavigationService.Navigate(mp);
        }

        private void TurnBack(object sender, RoutedEventArgs e)
        {       if (AcessedEmployee.role.Perms[7])
                 if(ActiveProductLister.Items.Count > 0)
                  CreateTicket(ticket,table);

            NavigationService.Navigate(mp);
        }

        private void AddProductFromMenu(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!AcessedEmployee.role.Perms[7])
            {
   Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoPermissionToProcessTitle, Ruby.Resources.Localization.MB_NoPermissionToProcessMessage, MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (Menu.SelectedItem != null && Menu.SelectedItem is Model.Product)
            {
                int Multiplier = 1;
                Product product = (Product)Menu.SelectedItem;

                //Try parse string to it
                try
                {
                    Multiplier = int.Parse(MultiplierBox.Text);
                }
                catch (Exception)
                {
                    Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NumberedRequiredAreaTitle, Ruby.Resources.Localization.MB_NumberedRequiredAreaMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //Send error if there is no serving
                if (product.Servings == null || product.Servings.Count < 1)
                {
                    Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NoServingTitle, Ruby.Resources.Localization.MB_NoServingMessage,MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }

                AddActiveProduct(product, Multiplier);

                //Send message that product has been added
                mp.HistoryInstance.SendMessage(AcessedEmployee, ScreenEnum.TICKETPAGE, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.TicketPage_AddedItemToAccount, Multiplier.ToString(), product.Name, table.Name),product.ID,table.ID);
            }
        }

        private void ServingBox_DropDownOpened(object sender, EventArgs e)
        {
        }

        private void ServingChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActiveProductLister.SelectedItem == null || ServingBox.SelectedItem == null) return;

        ((Product)ActiveProductLister.SelectedItem).CurrentServing = ((Product)ActiveProductLister.SelectedItem).Servings[ServingBox.SelectedIndex];
        }

        private void ActiveProductSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ActiveProductLister.SelectedItem == null) return;

            ServingBox.Items.Clear();

            foreach (ServingItem item in ((Product)ActiveProductLister.SelectedItem).Servings)
                ServingBox.Items.Add(item);

            ServingBox.SelectedIndex = 0;
        }

        #endregion

    }
}
