using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


using Ruby.Cafe.Model;

namespace Ruby.Cafe.Common.Controls
{


    /// <summary>
    /// Interaction logic for Navigator.xaml
    /// </summary>
    public partial class Navigator : UserControl
    {
        #region Variables

        public NavigationService nv;

        public Window MainInstance;
        public Frame FrameInstance;

        public Employee AccessedEmployee;

        public ScreenEnum CurrentScreen;

        public Ruby.Cafe.Common.Screens.MainPage       TablePage;
        public Ruby.Cafe.Common.Screens.HistoryManager HMan;
        public Ruby.Cafe.Common.Screens.TableEditor    TEdit;
        public Ruby.Cafe.Common.Screens.EmployeeEditor EEdit;
        public Ruby.Cafe.Common.Screens.ProductEditor  PEdit;
        public Ruby.Cafe.Common.Screens.SettingsPage  SetScr;

        public List<Model.Scence> Scences;
        public List<Model.Table> Tables;
        public List<Model.Role> Roles;
        public List<Model.Employee> Employees;
        public List<Model.Category> Categories;
        public List<Model.Product> Products;

        #endregion

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public Navigator()
        {
            InitializeComponent();
        }

        private void StartElapsingEmployee(Employee employee)
        {
            
        }

        /// <summary>
        /// A function to set given typed page's size
        /// </summary>
        /// <param name="TargetType">Type of object so it can find the object that wanted to resize</param>
        public void ReSizeScren(Type TargetType)
        {
                 if (TargetType == HMan.GetType())  { HMan.Height   = FrameInstance.Height;  FrameInstance.Content  =  HMan;    }
            else if (TargetType == TEdit.GetType()) { TEdit.Height  = FrameInstance.Height;  FrameInstance.Content  =  TEdit;   }
            else if (TargetType == EEdit.GetType()) { EEdit.Height  = FrameInstance.Height;  FrameInstance.Content  =  EEdit;   }
            else if (TargetType == PEdit.GetType()) { PEdit.Height  = FrameInstance.Height;  FrameInstance.Content  =  PEdit;   }
            else if (TargetType == SetScr.GetType()){ SetScr.Height = FrameInstance.Height;  FrameInstance.Content  =  SetScr;  }
        }
        /// <summary>
        /// A overloaded function to set given typed page's size with specific values
        /// </summary>
        /// <param name="TargetType">Type of object so it can find the object that wanted to resize</param>
        /// <param name="Width">integer value to set page's width</param>
        /// <param name="Height">integer value to set page's height</param>
        public void ReSizeScren(Type TargetType,int Width, int Height)
        {
            if (TargetType == HMan.GetType()) { HMan.Width = Width; HMan.Height = Height; FrameInstance.Content = HMan; }
            else if (TargetType == TEdit.GetType()) { TEdit.Width = Width; TEdit.Height = Height; FrameInstance.Content = TEdit; }
            else if (TargetType == EEdit.GetType()) { EEdit.Width = Width;  EEdit.Height = Height; FrameInstance.Content = EEdit; }
            else if (TargetType == PEdit.GetType()) { PEdit.Width = Width; PEdit.Height = Height; FrameInstance.Content = PEdit; }
            else if (TargetType == SetScr.GetType()) { SetScr.Width = Width; SetScr.Height = Height; FrameInstance.Content = SetScr; }
        }
        /// <summary>
        /// Happens when user press settings rectangle
        /// </summary>
        /// <param name="sender">pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void OpenSettings(object sender, MouseButtonEventArgs e)
        {
            if (SetScr == null)
            {
                SetScr = new Screens.SettingsPage(new History());
            }
            FrameInstance.Content = SetScr;
        }
        /// <summary>
        /// Happens when user press back rectangle
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void Return(object sender, MouseButtonEventArgs e)
        {
            if (FrameInstance.Content == SetScr)
            {
                if (SetScr.Error)
                    if (Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_ErrorAdissionTitle, Ruby.Resources.Localization.MB_ErrorAdissionMessage, MessageBoxButton.OK, MessageBoxImage.Error) == MB_ButtonTypes.OK)
                        return;

try{SetScr.Str = SetScr.GetSettings();
SetScr.DBStr = SetScr.GetDatabaseSettings();}catch (Exception ex){ MessageBox.ShowExceptionBox("","",ex); return;}

                if (System.IO.File.Exists("Settings.ini"))
                    System.IO.File.Delete("Settings.ini");
                System.IO.File.Create("Settings.ini").Close();

                if (System.IO.File.Exists(Ruby.Serialization.DatabaseSetting.Path))
                    System.IO.File.Delete(Ruby.Serialization.DatabaseSetting.Path);
                System.IO.File.Create(Ruby.Serialization.DatabaseSetting.Path).Close();

                SetScr.Str.SerializeSettings();
                SetScr.DBStr.SerializeSettings((Database.DatabaseType)SetScr.Str.dbType);
                SetScr.SavePage(Screens.SettingsPage.AdissionPath);
            }

            nv.Navigate(TablePage);
        }
        /// <summary>
        /// Happens when user click to button card
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void TableCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameInstance.Content = TEdit;
        }
        /// <summary>
        /// Happens when user click to button card
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void EmployeeCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameInstance.Content = EEdit;
        }
        /// <summary>
        /// Happens when user click to button card
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void HistoryCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameInstance.Content = HMan;
        }
        /// <summary>
        /// Happens when user click to button card
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void ProductCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameInstance.Content = PEdit;
        }
        double ReturnBtnOrgWidth;
        double ReturnBtnOrgHeight;
        /// <summary>
        /// Happens when mouse enters to rectangle's area 
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void ReturnMouseEnter(object sender, MouseEventArgs e)
        {
           ReturnBtnOrgWidth = ReturnBtn.Width;
           ReturnBtnOrgHeight = ReturnBtn.Height;

            ReturnBtn.Width = ReturnBtn.Width + (ReturnBtn.Width * 1 / 20);
            ReturnBtn.Height = ReturnBtn.Height + (ReturnBtn.Width * 1 / 20);

        }
        /// <summary>
        /// Happens when mouse leaves to rectangle's area 
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void ReturnMouseLeave(object sender, MouseEventArgs e)
        {
            ReturnBtn.Width = ReturnBtnOrgWidth;
            ReturnBtn.Height= ReturnBtnOrgHeight;
        }
        /// <summary>
        /// Happens when mouse enters to rectangle's area 
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void SettingsMouseEnter(object sender, MouseEventArgs e)
        {
            ReturnBtnOrgWidth =  SettingsBtn.Width;
            ReturnBtnOrgHeight = SettingsBtn.Height;

            SettingsBtn.Width =  SettingsBtn.Width + (ReturnBtn.Width * 1 / 20);
            SettingsBtn.Height = SettingsBtn.Height + (ReturnBtn.Width * 1 / 20);
        }
        /// <summary>
        /// Happens when mouse leaves to rectangle's area 
        /// </summary>
        /// <param name="sender">Pressed button's object</param>
        /// <param name="e">Event argument for interacting with mouse</param>
        private void SettingsMouseLeave(object sender, MouseEventArgs e)
        {
            SettingsBtn.Width = ReturnBtnOrgWidth;  
            SettingsBtn.Height= ReturnBtnOrgHeight;
        }
        /// <summary>
        /// Happens when navigator panel loads
        /// </summary>
        /// <param name="sender">User Control</param>
        /// <param name="e">Event argument</param>
        private void Navigator_Loaded(object sender, RoutedEventArgs e)
        {
            TableEditorCard.TTitle = Ruby.Resources.Localization.Navigator_TableETitle;
            EmployeeEditorCard.TTitle = Ruby.Resources.Localization.Navigator_EmployeeETitle;
            HistoryManagerCard.TTitle = Ruby.Resources.Localization.Navigator_HistoryMTitle;
            ProductEditorCard.TTitle = Ruby.Resources.Localization.Navigator_ProductETitle;

            TableEditorCard.ImagePath = "pack://application:,,,/Ruby.Resources;Component/IMG/IMG_TableBtnCard_Logo.png";
            EmployeeEditorCard.ImagePath = "pack://application:,,,/Ruby.Resources;Component/IMG/IMG_EmployeeBtnCard_Logo.png";
            HistoryManagerCard.ImagePath = "pack://application:,,,/Ruby.Resources;Component/IMG/IMG_HistoryBtnCard_Logo.png";
            ProductEditorCard.ImagePath = "pack://application:,,,/Ruby.Resources;Component/IMG/IMG_ProductBtnCard_Logo.png";

        }
    }
}
