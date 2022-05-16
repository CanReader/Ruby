using Ruby.Cafe.Common.Controls;
using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

       private UniformGrid _activegrid;
       private UniformGrid ActiveGrid
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

       private List<UniformGrid> ScenceControls;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a canvas for displaying tables over on...
        /// </summary>
        /// <param name="Scene">Scence object which will be matched with the canvas</param>
        /// <returns>Canvas</returns>
        private UniformGrid CreateScenceCanvas(Scence scence)
        {
            UniformGrid kanvas = new UniformGrid();
         
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

        private void PlayGridAnimation(UniformGrid grid)
        {
            
        }

        private void DisplayTableCards(UniformGrid grid, Scence scence)
        {
            grid.Children.Clear();

            TableList = TableList.Where(p => p.TableScence == scence.Name && p.Checks[0]).ToList();

            if (DisplayTableList.Count == 0)
                return;

            #region Add TableControls into cells

            foreach (var item in DisplayTableList)
            { 
                TableControl tb = new TableControl(item);
                grid.Children.Add(tb);
           
                tb.MouseDown += (ssender, ee) => OpenTicketPage(((TableControl)ssender).table);
            }
            #endregion

            PlayGridAnimation(grid);
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
            if(ScenceControls == null)ScenceControls = new List<UniformGrid>();
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

        /// <summary>
        /// Routes the user to add some tables in table editor
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event arg</param>
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

        /// <summary>
        /// Changes the current scene and refreshes the table list
        /// </summary>
        /// <param name="sender">Rectangle</param>
        /// <param name="e">MouseButtonEventArgs</param>
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

        /// <summary>
        /// Changes the current scene and refreshes the table list
        /// </summary>
        /// <param name="sender">Rectangle</param>
        /// <param name="e">MouseButtonEventArgs</param>
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