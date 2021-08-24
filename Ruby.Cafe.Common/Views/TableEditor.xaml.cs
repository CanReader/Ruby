using Ruby.Cafe.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ruby.Cafe.Common.Screens
{
    public partial class TableEditor : Page
    {
        #region Variables

        private Database.IDatabase db;
        private History history;

        public History HistoryInstance;
        public Employee AccessedEmployee;

        public List<Scence> Scences;
        public List<Table> TableListInstance;

        /// <summary>
        /// Indicates selected table which have a getter and a setter methods. Getter method returns selected table and Setter method provides to show name of the table on Selected table named text block, demonstrates properties of the table on editor
        /// </summary>
        public Table ActiveTable
        {
            get
            {
                if (TableLister.SelectedItem != null)
                    return TableListInstance.Find(p => p.Name == TableLister.SelectedItem.ToString() && p.TableScence == ScenceText.Text);
                else
                    return null;
            }

            set
            {
                if (value == null)
                {
                    ClearUI();
                }
                else
                {
                    TableNameBox.Text = value.Name;
                    TableScence.Text = value.TableScence;
                    TableTypeBox.Text = value.TableType;
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#" + value.TableColor);
                    Picker.SelectedColor = System.Windows.Media.Color.FromRgb(color.R,color.G,color.B);
                    IsEnable.IsChecked = value.Checks[0];
                    IsLock.IsChecked = value.Checks[1];
                    IsReservable.IsChecked = value.Checks[2];
                    ChairNumberBox.Text = value.MaxChair.ToString();
                }
            }
        }

        /*
         * Doing operations in RubyCafe's codes
         * 1.Do operation in List
         * 2.Do operation in UI element
         * 4.Do operation in server-side
         */

        /// <summary>
        /// Indicates activated Scence. It has only a setter method which 
        /// </summary>
        public Scence ActiveScence
        {
            set
            {

                TableLister.Items.Clear();

                this.ScenceText.Text = value.Name;

                if (TableListInstance.Count != 0)
                    foreach (var table in TableListInstance.Where(p => p.TableScence == value.Name))
                        TableLister.Items.Add(table);
            }
            get
            {
                return Scences.Find(s => s.Name == ScenceText.Text);
            }
        }

        #endregion

        #region Methods

        public TableEditor(Database.IDatabase database, History HistoryInstance)
        {
            this.db = database;
            this.HistoryInstance = HistoryInstance;

            InitializeComponent();

            using (Ruby.Serialization.Settings setings = new Ruby.Serialization.Settings(true))
            {
                this.Width = setings.Width;
                this.Height = setings.Height - 140;
            }
        }

        private void ClearUI()
        {
            if (TableLister.Items.Count > 0) TableLister.Items.Clear();

            #region localization
            TableNameBox.Text = Ruby.Resources.Localization.TableEditor_DefaultTableNameTxt;
            TableScence.Text = Ruby.Resources.Localization.TableEditor_DefaultTableScenceTxt;
            ColorPickerText.Text = Ruby.Resources.Localization.TableEditor_DefaultTableColorTxt;
            TableTypeBox.Text = Ruby.Resources.Localization.TableEditor_DefaultTableTypeTxt;
            ChairNumberBox.Text = Ruby.Resources.Localization.TableEditor_DefaultTableMaxChairTxt;
            IsEnable.Content = Ruby.Resources.Localization.TableEditor_DefaultEnabledCheckTxt;
            IsLock.Content = Ruby.Resources.Localization.TableEditor_DefaultLockCheckTxt;
            IsReservable.Content = Ruby.Resources.Localization.TableEditor_DefaultReservableCheckTxt;
            CreateTableBtn.Content = Ruby.Resources.Localization.TableEditor_DefaultCreateTableBtnTxt;
            EditTableBtn.Content = Ruby.Resources.Localization.TableEditor_DefaultEditTableBtnTxt;
            RemoveTableBtn.Content = Ruby.Resources.Localization.TableEditor_DefaultRemoveTableBtnTxt;
            ScenceName.Text = Ruby.Resources.Localization.TableEditor_DefaultScenceAdderTxt;
            AddScenceBtn.Content = Ruby.Resources.Localization.TableEditor_DefaultAddScenceBtnTxt;
            RemoveScenceBtn.Content = Ruby.Resources.Localization.TableEditor_DefaultRemoveScenceBtnTxt;
            Picker.StandardTabHeader = Ruby.Resources.Localization.TableEditor_DefaultStandardColorsTxt;
            Picker.AdvancedTabHeader = Ruby.Resources.Localization.TableEditor_DefaultAdvancedColorsTxt;

            #endregion

            if (Scences == null || Scences.Count == 0) ScenceText.Text = Ruby.Resources.Localization.TableEditor_EmptyScenceText;
            else ActiveScence = Scences[0];

        }

        private Table CreateTable(String TableName, String ScenceName, String TableColor, String TableType, int MaxNumber, bool[] Checks)
        {
            Table table = new Table();

            try
            {
            table.Name = TableName;
            table.TableScence = ScenceName;
            table.TableColor = TableColor.Substring(1,TableColor.Length-1);
            table.TableType = TableType;
            table.MaxChair = MaxNumber;
            table.Checks = Checks;
            }
            catch (Exception e)
            {
                Controls.MessageBox.ShowExceptionBox(Ruby.Resources.Localization.EXC_FailToCreateTable, null, e);

                HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.TABLEEDITOR, MessageType.ERROR, Ruby.Resources.Localization.ERROR_FailToCreateTableMessage);
                return null;
            }
            return table;
        }

        private void DeleteTable(Table table)
        {
            db.DeleteTable(table.Name);

            HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.TABLEEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_TableDeleted, ActiveTable.Name));

            TableListInstance.RemoveAt(TableListInstance.IndexOf(table));
            TableLister.Items.Clear();
            foreach (var tabler in TableListInstance.Where(t => t.TableScence == ScenceText.Text))
                TableLister.Items.Add(tabler);
        }

        private void EditTable(String TableName, String ScenceName, String TableColor, String TableType, int MaxNumber, bool[] Checks,Table CurTable)
        {
            int index = TableListInstance.IndexOf(ActiveTable);

            String CTableName = CurTable.Name;
            TableColor = TableColor.Substring(1,TableColor.Length-1);
            
            db.UpdateTable(TableName,ScenceName,TableType,TableColor,MaxNumber,ActiveTable.ConvertBoolArrayToString(Checks),0,"",CTableName);

            TableListInstance[index] = new Table()
            {
                Name = TableName,
                TableScence = ScenceName,
                TableColor = TableColor,
                TableType = TableType,
                MaxChair = MaxNumber,
                Checks = Checks,
                CurrentStatue = 0,
                Description = ""
            };

            ActiveTable = TableListInstance[0];
            ActiveTable = TableListInstance[index];

            HistoryInstance.SendMessage(AccessedEmployee,ScreenEnum.TABLEEDITOR,MessageType.PROCESS,string.Format(Ruby.Resources.Localization.PROCESS_TableChanged,CTableName,TableName));

            TableLister.Items.Clear();
            foreach (var table in TableListInstance.Where(s=> s.TableScence == ScenceText.Text))
                TableLister.Items.Add(table);
        }

        private Scence CreateScence(String ScenceName)
        {
            if (!Scences.Exists(p => p.Name == ScenceName))
            {
            Scence scence = new Scence()
                {
                Name = ScenceName
                };
                return scence;
            }
            return new Scence() { Name = null };
        }

        private void RemoveScence(Scence scence)
        {
            foreach (Table table in TableListInstance.Where(p => p.TableScence == scence.Name))
            {
                db.DeleteTable(table.Name);
            }
                TableListInstance.RemoveAll(t => t.TableScence == scence.Name);

            db.DeleteScence(scence.Name);

            HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.TABLEEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_ScenceDeleted,scence.Name));

            Scences.Remove(scence);

            if (Scences.Exists(s => s.Name == Scences[0].Name))
                ActiveScence = Scences[0];
            else
            {
                ScenceText.Text = Ruby.Resources.Localization.TableEditor_EmptyScenceText;
                TableLister.Items.Clear();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Happens when page is opened
        /// </summary>
        /// <param name="sender">Page</param>
        /// <param name="e">Routed event argument</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.TABLEEDITOR, MessageType.NOTIFICATION, Ruby.Resources.Localization.NOTIF_AccessedToScreen);

            Scences = db.GetScenceList();
            TableListInstance = db.GetTableList(Scences);

            ClearUI();

            TableTypeBox.Items.Add("Type1");
            TableTypeBox.Items.Add("Type2");
            TableTypeBox.Items.Add("Type3");
            TableTypeBox.Items.Add("Type4");
        }

        /// <summary>
        /// Happens when the page is closed
        /// </summary>
        /// <param name="sender">page</param>
        /// <param name="e">Routed Event Args</param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearUI();
        }

        /// <summary>
        /// Happens when user interected on Create Scence named button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event argument</param>
        private void CreateScence(object sender, RoutedEventArgs e)
        {
            Scence scence = CreateScence(ScenceName.Text);

            if (scence.Name != null)
            {

            Scences.Add(scence);
            db.AddScences(scence.Name);

            ActiveScence = Scences[Scences.Count-1];

            this.HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.TABLEEDITOR, MessageType.PROCESS,string.Format(Ruby.Resources.Localization.PROCESS_ScenceCreated,ScenceName.Text));
            }
            else
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_AlreadyExistsTitle, Ruby.Resources.Localization.MB_AlreadyExistsMessage,MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///  Happens when user interected on remove table named button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event args</param>
        private void RemoveScence(object sender, RoutedEventArgs e)
        {
            if (ActiveScence.Name == null) return;
            if(Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_SureToDeleteTitle, string.Format(Ruby.Resources.Localization.MB_SureToDeleteMessage, ActiveScence.Name, ScenceText.Text),MessageBoxButton.YesNo,MessageBoxImage.Exclamation) == Controls.MB_ButtonTypes.Yes)
            RemoveScence(ActiveScence);

            
        }

        /// <summary>
        /// Happens when user interected on create table named button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event argument</param>
        public void CreateTable(object sender,RoutedEventArgs e)
        { 
            if (TableListInstance.Exists(p => p.Name == TableNameBox.Text)) return;
            if (!Scences.Exists(p=> p.Name == TableScence.Text.TrimEnd().TrimStart()))
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(string.Format(Ruby.Resources.Localization.MB_FillRequiredAreasTitle,Ruby.Resources.Localization.Entity_Table),string.Format(Ruby.Resources.Localization.MB_FillRequiredAreasMessage,TableNameBox.Text),MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }

            if(!string.IsNullOrWhiteSpace(TableNameBox.Text) &&
                !string.IsNullOrWhiteSpace(TableScence.Text) &&
                Picker.SelectedColor != null &&
                !string.IsNullOrWhiteSpace(TableTypeBox.Text) &&
                !string.IsNullOrWhiteSpace(ChairNumberBox.Text)
                   )
            {
                try { int.Parse(ChairNumberBox.Text); }
                catch (System.FormatException fe)
                {
                    Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_NumberedRequiredAreaTitle, Ruby.Resources.Localization.MB_NumberedRequiredAreaMessage,MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
                catch (System.Exception ex)
                {
                    return;
                }

                bool[] Checks = new bool[3] {IsEnable.IsChecked.Value,IsLock.IsChecked.Value,IsReservable.IsChecked.Value};

                System.Drawing.Color color = System.Drawing.Color.FromArgb(Picker.SelectedColor.Value.R, Picker.SelectedColor.Value.G, Picker.SelectedColor.Value.B);

                Table table = CreateTable(TableNameBox.Text.TrimEnd().TrimStart(), TableScence.Text, System.Drawing.ColorTranslator.ToHtml(color), TableTypeBox.Text,int.Parse(ChairNumberBox.Text),Checks);

                if (table != null)
                {
                    db.AddTable(table.Name,Scences.Find(p=> p.Name == table.TableScence),table.TableType,table.TableColor,table.MaxChair,table.ConvertBoolArrayToString(table.Checks),0,"");
                    TableListInstance.Add(table);
                    TableLister.Items.Add(table);
                    HistoryInstance.SendMessage(AccessedEmployee,ScreenEnum.TABLEEDITOR,MessageType.PROCESS,string.Format(Ruby.Resources.Localization.PROCESS_TableCreated,table.Name,table.TableScence));

                    ClearUI();

                    TableLister.Items.Clear();
                    foreach(var t in TableListInstance.Where(ta => ta.TableScence == ScenceText.Text)) TableLister.Items.Add(t);
                }

            }   
        }

        /// <summary>
        /// Happens when user interected on edit table named button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event argument</param>
        public void EditTable(object sender, RoutedEventArgs e)
        {
            if (ActiveTable == null)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_ItemIsNotSelectedTitle, Ruby.Resources.Localization.MB_ItemIsNotSelectedMessage,MessageBoxButton.OK,MessageBoxImage.Error);
            return;
            }

            if (Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_SureToEditTitle, string.Format(Ruby.Resources.Localization.MB_SureToEditMessage,ActiveTable.Name), MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == Controls.MB_ButtonTypes.No)
                return;

            if (!string.IsNullOrWhiteSpace(TableNameBox.Text) &&
                !string.IsNullOrWhiteSpace(TableScence.Text) &&
                Picker.SelectedColor != null &&
                !string.IsNullOrWhiteSpace(TableTypeBox.Text) &&
                !string.IsNullOrWhiteSpace(ChairNumberBox.Text)
                   )
            { 
                System.Drawing.Color color = System.Drawing.Color.FromArgb(Picker.SelectedColor.Value.R, Picker.SelectedColor.Value.G, Picker.SelectedColor.Value.B);

                EditTable(TableNameBox.Text.TrimStart().TrimEnd(),TableScence.Text, System.Drawing.ColorTranslator.ToHtml(color), TableTypeBox.Text,int.Parse(ChairNumberBox.Text),new bool[] {IsEnable.IsChecked.Value,IsLock.IsChecked.Value,IsReservable.IsChecked.Value},ActiveTable);


            }
            else
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_FillRequiredAreasTitle, Ruby.Resources.Localization.MB_FillRequiredAreasMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Happens when user interected on remove table named button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event argument</param>
        public void RemoveTable(object sender, RoutedEventArgs e)
        {
            if (ActiveTable == null)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_ItemIsNotSelectedTitle, Ruby.Resources.Localization.MB_ItemIsNotSelectedMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_SureToDeleteTitle, string.Format(Ruby.Resources.Localization.MB_SureToDeleteMessage, ActiveTable.Name), MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == Controls.MB_ButtonTypes.No) return;

            DeleteTable(ActiveTable);

            ActiveTable = null;
        }

        /// <summary>
        /// Happens when user interacted on next arrow icon
        /// </summary>
        /// <param name="sender">Rectangle</param>
        /// <param name="e">Mouse button event argunent</param>
        private void ShowNextScence(object sender, MouseButtonEventArgs e)
        {
            int TargetIndex = Scences.FindIndex(p => p.Name == ScenceText.Text) +1;

            if (Scences.Count != 0)
            {
                if (TargetIndex >= Scences.Count) { ActiveScence = Scences[0]; return; }

                if (!string.IsNullOrEmpty(Scences[TargetIndex].Name))
                    ActiveScence = Scences[TargetIndex];
            }
        }

        /// <summary>
        /// Happens when user interacted on back arrow icon
        /// </summary>
        /// <param name="sender">Rectangle</param>
        /// <param name="e">Mouse button evebt argunent</param>
        private void ShowPreviousScence(object sender, MouseButtonEventArgs e)
        {
            int TargetIndex = Scences.FindIndex(p => p.Name == ScenceText.Text) - 1;

            if (Scences.Count != 0)
            {
                if (TargetIndex >= 0) { ActiveScence = Scences[Scences.Count - 1]; return; }
                else
                  ActiveScence = Scences[TargetIndex];
            }
        }
   
        /// <summary>
        /// Happens when user opens on the table scence dropdown box
        /// </summary>
        /// <param name="sender">Combobox</param>
        /// <param name="e">Traditional Event argument</param>
        private void TableScence_DropDownOpened(object sender, EventArgs e)
        {
            TableScence.Items.Clear();

            foreach(var scence in Scences) TableScence.Items.Add(scence);
        }

        /// <summary>
        /// Set active table when table lister's selected value changed
        /// </summary>
        /// <param name="sender">Table Object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void TableLister_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableLister.SelectedItem != null)
                ActiveTable = (Table)TableLister.SelectedItem;
           // ActiveTable = TableListInstance.Find(p => p.Name == TableLister.SelectedItem.ToString() && p.TableScence == ScenceText.Text);
        }
        #endregion
    }

}