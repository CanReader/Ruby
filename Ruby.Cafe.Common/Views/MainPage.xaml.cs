using Ruby.Cafe.Common.Controls;
using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Ruby.Cafe.Common.Screens
{
    public partial class MainPage : Page
    {
        #region Variables

       public Database.IDatabase Dbase;

       private AdminScreen Panel;

       private Model.Scence _Scence;
       private Model.Scence ActiveScence
        {
            get
            {
                return _Scence;
            }
            set
            {
                _Scence = value;
                ScenceNameText.Text = value.Name;

                if (TableList.Count > 0)
                {
                ActiveGrid = ScenceControls[Scences.IndexOf(value)];

                DisplayTableCards(ActiveGrid,value);
                }
            }
        }

        private Grid _activegrid;
        private Grid ActiveGrid
        {
            get
            {
                return _activegrid;
            }
            set
            {
                if(_activegrid != null)
                    _activegrid.Visibility = Visibility.Hidden;

                _activegrid = value;

                value.Visibility = Visibility.Visible;
            }
        }

       public Ruby.Cafe.Common.History HistoryInstance;

       private Button NoScenceBtn;
       private TextBlock NoScenceText;

       public List<Table> TableList;
       public List<Table> DisplayTableList;
       public List<Scence> Scences;
       public List<Employee> EmployeeList;
       public List<Role> Roles;

       private List<Grid> ScenceControls;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a canvas for displaying tables over on...
        /// </summary>
        /// <param name="Scene">Scence object which will be matched with the canvas</param>
        /// <returns>Canvas</returns>
        private Grid CreateScenceCanvas(Scence scence)
        {
            Grid kanvas = new Grid();
         
           kanvas.Width = this.ActualWidth;
           kanvas.Height = this.ActualHeight - UpperPanel.ActualHeight;

            kanvas.Margin = new Thickness(0, 0, 0, 0);
            kanvas.Background = TablePanel.Background;
            kanvas.Visibility = Visibility.Hidden;

            TablePanel.Children.Add(kanvas);

            string ScenceName = scence.Name;

            int gh = 1;

            if (ScenceControls.Count != 0)
                gh = ScenceControls.Count;

            scence.GridHolder = gh;

            return kanvas;
        }

        /// <summary>
        /// Opens table viewer to setup the table, 
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ProductList"></param>
        /// <param name="Categories"></param>
        /// <param name="Categories"></param>
        private void OpenTicketPage(Table table)
        {
            if (!table.Checks[1])
                NavigationService.Navigate(new TicketPage(this, table, Dbase));
            else
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_LockedTableTitle, Ruby.Resources.Localization.MB_LockedTableMessage,MessageBoxButton.OK,MessageBoxImage.None);
        }

        //TODO: Will be edited
        private void DisplayTableCards(Grid grid, Scence scence)
        {

            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            List<Table> ConditionalTableList = TableList.Where(p => p.TableScence == scence.Name && p.Checks[0]).ToList();

            if (ConditionalTableList.Count == 0)
                return;

            int CellNumber = ConditionalTableList.Count;

            if (CellNumber % 2 == 1 && Math.Sqrt(CellNumber) % 1 != 0)
                CellNumber++;

            grid.ShowGridLines = true;

            #region Define Constant
            int c = 0;
            if (Math.Sqrt(CellNumber + 1) % 1 == 0)
                c = 1;
            else if (Math.Sqrt(CellNumber + 2) % 1 == 0)
                c = 2;
            else if (Math.Sqrt(CellNumber + 3) % 1 == 0)
                c = 3;
            else if (Math.Sqrt(CellNumber + 4) % 1 == 0)
                c = 4;
            #endregion

            // Calculate in square matrix
            if (Math.Sqrt(CellNumber + c) % 1 == 0)
            {
                for (int a = 0; a < Math.Sqrt(CellNumber); a++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }

            }
            //Find closest two integers' product
            else
            {
                int RowCells = CellNumber - 1;

                List<int[]> Multipliers = new List<int[]>();

                bool found = false;
                while (!found)
                {
                 Multipliers.Clear();
                 for (int m = CellNumber; m > 0; m--)
                 {
                        if ((Math.Abs(m - CellNumber / m) <= 6))
                        {
                            RowCells = m;
                            found = true;
                        }
                 }
                    CellNumber++;
                }

                foreach (var item in Multipliers)
                    if (Math.Abs(item[1] - item[0]) <= 6)
                    { RowCells = item[1]; break; }

                for (int ad = 0; ad < RowCells; ad++)
                    grid.RowDefinitions.Add(new RowDefinition());
                for (int ad = 0; ad < CellNumber/RowCells; ad++)
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

            }

            #region Add TableControls into cells
            int i = 0;
            int j = 0;
            for (int k = 0; k < ConditionalTableList.Count; k++)
            {
            TableControl tb = new TableControl(ConditionalTableList[k]);
            tb.Width = this.ActualWidth/grid.ColumnDefinitions.Count;
            tb.Height = (this.ActualHeight-UpperPanel.ActualHeight)/grid.RowDefinitions.Count;

            tb.MouseDown += (ssender, ee) => OpenTicketPage(((TableControl)ssender).table);

            grid.Children.Add(tb);
            Grid.SetRow(tb,i);
            Grid.SetColumn(tb,j);

                j++;

                if (j == grid.ColumnDefinitions.Count)
                {
                    j = 0;
                    i = i + 1;
                }
            }
            #endregion
        }

        /// <summary>
        /// Main page's constructor method
        /// </summary>
        /// <param name="dbase">Gets database instance for getting data from connected database</param>
        public MainPage(Database.IDatabase dbase, History HistoryInstance)
        {
           this.HistoryInstance = HistoryInstance;

            this.Panel = new AdminScreen(dbase,this.HistoryInstance);

            this.Dbase = dbase;

            InitializeComponent();

            Panel.PageMain = this;

        }

        #endregion

        #region Events

        /// <summary>
        /// Happens when window has been drawed
        /// </summary>
        /// <param name="sender">Indicates which thing is made this</param>
        /// <param name="e">Handles arguments</param>
        private void CreatedEvent(object sender, RoutedEventArgs e)
        {
            #region Initializations
            if(ScenceControls == null)ScenceControls = new List<Grid>();
            if(DisplayTableList == null)DisplayTableList = new List<Table>();

            Scences      = Dbase.GetScenceList();
            TableList    = Dbase.GetTableList(Scences);
            #endregion

            if(new Serialization.Settings(true).CloseBtnOption == false) 
                CloseAppBtn.Visibility = Visibility.Hidden;
            else
                CloseAppBtn.Visibility = Visibility.Visible;

            if (TableList.Count == 0)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                sp.VerticalAlignment = VerticalAlignment.Center;

                HistoryInstance.SendMessage(ScreenEnum.MAINPAGE,MessageType.WARNING, Ruby.Resources.Localization.MainPage_NoTables);

                NoScenceText = new TextBlock();
                NoScenceText.FontSize = 23;
                NoScenceText.Text = Ruby.Resources.Localization.MainPage_QuickAddTableLabelTxt;
                NoScenceText.TextAlignment = TextAlignment.Center;
                NoScenceText.TextWrapping = TextWrapping.Wrap;
                NoScenceText.Width = 600;
                NoScenceText.HorizontalAlignment = HorizontalAlignment.Center;
              //  NoScenceText.Margin = new System.Windows.Thickness(this.ActualWidth / 2 - 600/2, this.ActualHeight / 2 - NoScenceText.ActualHeight / 2 - 100, 0, 0);
                NoScenceText.Foreground = new System.Windows.Media.SolidColorBrush(Color.FromRgb(40, 30, 20));
                NoScenceText.Visibility = Visibility.Visible;

                NoScenceBtn = new Button();
                NoScenceBtn.Name = "AddTableBtn";
                NoScenceBtn.Width = 158;
                NoScenceBtn.Height = 42;
                NoScenceBtn.Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                NoScenceBtn.FontSize = 20;
                NoScenceBtn.Content = Ruby.Resources.Localization.MainPage_QuickAddTableBtnTxt;
                NoScenceBtn.HorizontalAlignment = HorizontalAlignment.Center;
                //NoScenceBtn.Margin = new System.Windows.Thickness(this.ActualWidth / 2 - 158/ 2, this.ActualHeight / 2 - 42/2, 0, 0);
                NoScenceBtn.Background = new System.Windows.Media.SolidColorBrush(Color.FromRgb(255, 162, 0));
                NoScenceBtn.Visibility = Visibility.Visible;
                NoScenceBtn.Click += QuickTableEditor;
     
                sp.Children.Add(NoScenceText);
                sp.Children.Add(NoScenceBtn);

                MainGrid.Children.Add(sp);
                Grid.SetRow(sp,1);
            }
            else
                foreach (var scence in Scences)
                    ScenceControls.Add(CreateScenceCanvas(scence));

            if (Scences != null && Scences.Count > 0)
                ActiveScence = Scences[0];
            else
            {
                HistoryInstance.SendMessage(ScreenEnum.MAINPAGE, MessageType.WARNING, Ruby.Resources.Localization.MainPage_NoScences);
                ScenceNameText.Text = Ruby.Resources.Localization.MainPage_EmptyScenceTxt;
            }
        }

        private void QuickTableEditor(object sender, RoutedEventArgs e)
        {
            Panel.toScreen = ScreenEnum.TABLEEDITOR;
            NavigationService.Navigate(Panel);
        }

        /// <summary>
        /// Enters to panel which is usable by admin users only!
        /// </summary>
        /// <param name="sender">Rectangle</param>
        /// <param name="e">MouseEventArgs</param>
        private void EnterPanel(object sender, MouseEventArgs e)
        {
            Panel.PageMain = this;
            NavigationService.Navigate(Panel);
        }

        /// <summary>
        /// Helps users to exit application
        /// </summary>
        /// <param name="sender">Rectangle</param>
        /// <param name="e">MouseEventArgs</param>
        private void SendQuitMessage(object sender, MouseEventArgs e)
        {
            if (Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MainPage_ExitTitle, Ruby.Resources.Localization.MainPage_ExitMessage, MessageBoxButton.YesNo, MessageBoxImage.Information)
                == Controls.MB_ButtonTypes.No)
                return;

            HistoryInstance.SendMessage(ScreenEnum.MAINPAGE,MessageType.NOTIFICATION,string.Format(Ruby.Resources.Localization.NOTIF_ClosedApp, new Serialization.Settings(true).MachineName));
            Application.Current.Shutdown();
        }

        private void PrevScenceBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            Scences = Dbase.GetScenceList();
            TableList = Dbase.GetTableList(Scences);
#endif

            if (Scences.Count <= 1) return;

            int i = Scences.FindIndex(p=> p.Name == ScenceNameText.Text);

            if (i-1 < 0)
            {
                ActiveScence = Scences[Scences.Count - 1];
            }
            else
            {
                ActiveScence = Scences[i - 1];
            }

        }

        private void NextScenceBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            Scences = Dbase.GetScenceList();
            TableList = Dbase.GetTableList(Scences);
#endif

            if (Scences.Count <= 1) return;

            int i = Scences.FindIndex(p => p.Name == ScenceNameText.Text);

            if (i + 1 > Scences.Count - 1)
            {
                ActiveScence = Scences[0];
            }
            else
            {
                ActiveScence = Scences[i + 1];
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if(NoScenceText != null) NoScenceText.Visibility = Visibility.Hidden;
            NoScenceText = null;
            
            if(NoScenceBtn != null) NoScenceBtn.Visibility = Visibility.Hidden;
            NoScenceBtn = null;
        }

        #endregion
    }

    class IntComparer : IComparer<int[]>
    {
        public int Compare(int[] x, int[] y)
        {
            List<int> lst = new List<int>();

            if (Math.Abs(x[0] - x[1]) < Math.Abs(y[0] - y[1]))
                return 1;
            else
                return 0;
        }
    }
}