using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Ruby.Cafe.Model;

namespace Ruby.Cafe.Common.Screens
{
    /// <summary>
    /// Purpose:
    /// Admin screen is core part of the RubyCafe. This class lives until program is shutted down. The main responsibility of the Admin screen/panel is carrying the pages that manages the application. The panel is only reachable by admin users, and when a user attempts to log in the panel, firstly permissions is checked. If they have correct permission for that they can enter. When a user enters, automatically gets redirect to history page.
    /// 
    /// The navigation events of the admin screen happens thanks to navigation panel at top of the content part of the page. Navigation panel just contains return button, page cards, settings tab.
    /// </summary>
    public partial class AdminScreen : Page
    {
        public History HistoryManager;

        public ScreenEnum toScreen = ScreenEnum.MAINPAGE;

        public Employee AccessedEmployee;

        public NavigationService nv;

        public Page DefaultPage;
        public MainPage PageMain;

        Database.IDatabase db;

        public List<Category> Categories;
        public List<Product> Products;
        public List<Role> Roles;
        public List<Employee> Employees;
        public List<Scence> Scences;
        public List<Table> Tables;

        /// <summary>
        /// Constructor of the AdminScreen
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="HistoryInstance"></param>
        public AdminScreen(Database.IDatabase database, History HistoryInstance)
        {
            InitializeComponent();
            
            this.db = database;

            this.HistoryManager = HistoryInstance;

            using (Serialization.Settings str = new Serialization.Settings(true)){ this.Width = str.Width; this.Height = str.Height; }

            HistoryManager.Messages = HistoryManager.GetMessages();

            Navigator.EEdit = new EmployeeEditor(db, HistoryManager); 
            Navigator.HMan = new HistoryManager(db, HistoryManager);
            Navigator.PEdit = new ProductEditor(db, HistoryManager);
            Navigator.TEdit = new TableEditor(db, HistoryManager);
            Navigator.SetScr = new SettingsPage(HistoryManager);

            Navigator.TablePage = this.PageMain;

            ContentFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            Navigator.FrameInstance = ContentFrame;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            nv = NavigationService.GetNavigationService(this);
            Navigator.nv = nv;

            if (AccessedEmployee == null)
                nv.Navigate(new LoginPage(this, PageMain,db) {HistoryInstance = this.HistoryManager }); 
            else
            {
                if (PageMain == null) PageMain = new MainPage(db, HistoryManager);

                Scences = PageMain.Scences;
                Tables = PageMain.TableList;

                Navigator.TablePage = this.PageMain;

                if (toScreen == ScreenEnum.MAINPAGE)
                    using (Serialization.Settings sr = new Serialization.Settings(true)) toScreen = ScreenEnum.HISTORYMANAGER;

                    HistoryManager.SendMessage(AccessedEmployee, toScreen, MessageType.NOTIFICATION,Ruby.Resources.Localization.NOTIF_AccessedToScreen);

                switch (toScreen)
                {
                    case ScreenEnum.HISTORYMANAGER:
                        DefaultPage = Navigator.HMan;
                        break;
                    case ScreenEnum.TABLEEDITOR:
                        DefaultPage = Navigator.TEdit;
                        break;
                    case ScreenEnum.EMPLOYEEEDITOR:
                        DefaultPage = Navigator.EEdit;
                        break;
                    case ScreenEnum.PRODUCTEDITOR:
                        DefaultPage = Navigator.PEdit;
                        break;
                    case ScreenEnum.SETTINGS:
                        DefaultPage = Navigator.SetScr;
                        break;
                    default:
                        nv.Navigate(PageMain);
                        break;
                }

            ContentFrame.Content = DefaultPage;

            }

            #region Referencing classes
            Navigator.TEdit.AccessedEmployee = this.AccessedEmployee;
            Navigator.EEdit.AccessedEmployee = this.AccessedEmployee;
            Navigator.HMan.AccessedEmployee = this.AccessedEmployee; 
            Navigator.PEdit.AccessedEmployee = this.AccessedEmployee;
            Navigator.SetScr.AccessedEmployee = this.AccessedEmployee;
            #endregion

            this.ContentFrame.Width  = this.ActualWidth;
            this.ContentFrame.Height = this.ActualHeight - 140;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AccessedEmployee = null;
            ContentFrame.Content = null;
        }
    }
 }