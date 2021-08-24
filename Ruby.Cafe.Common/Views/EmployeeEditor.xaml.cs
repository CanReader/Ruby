using Ruby.Cafe.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Ruby.Cafe.Common.Screens
{
    public partial class EmployeeEditor : Page
    {
        #region Variables
        private const int PermsLimit = 14;

        public Ruby.Cafe.Common.History HistoryInstance;
        public Employee AccessedEmployee;

        public List<Employee> EmployeesInstance;
        public List<Role> RolesInstance;

        public Employee ActiveEmployee
        {
            get
            {
                if (EmployeeList.SelectedItem != null)
                {
                    string[] FullName = EmployeeList.SelectedItem.ToString().Split();
                    String Name;
                    String Surname;

                    if (FullName.Length == 2)
                    {
                        Name = FullName[0];
                        Surname = FullName[1];
                    }
                    else
                    {
                        Name = FullName[0] + " " + FullName[1];
                        Surname = FullName[2];
                    }

                    Employee emp = EmployeesInstance.Find(p => p.Name == Name && p.Surname == Surname);

                    return emp;
                }
                else return null;
            }

            set
            {
                EmployeeNameBox.Text = value.Name;
                EmployeeSurnameBox.Text = value.Surname;
                EmployeeGenderBox.SelectedIndex = value.Gender;
                EmployeeMailBox.Text = value.Mail;
                EmployeeStartDateBox.SelectedDate = value.StartDate;
                EmployeePhoneBox.Text = value.Phone;
                EmployeeAdressBox.Text = value.Adress;
                AuthBox.Text = value.AuthCode;
            }
        }

        public Role ActiveRole
        {
            get
            {
                if (RolesListbox.SelectedItem != null)
                    return RolesInstance.Find(p=> p.Name == RolesListbox.SelectedItem.ToString());
                else
                    return RolesInstance[0];
            }

            set
            {
                RoleNameBox.Text = value.Name;

                for (int pindex = 0; pindex < value.Perms.Length; pindex++)
                    ((CheckBox)PermissionLister.Items[pindex]).IsChecked = value.Perms[pindex];
            }
        }

        private Database.IDatabase db;
        
        #endregion

        #region Methods

        //<summary>
        // Constructor of the class
        //</summary>
        public EmployeeEditor(Database.IDatabase database, History HistoryInstance)
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

        /// <summary>
        /// Creates a employee for the list
        /// </summary>
        /// <param name="employee">Represents a reference class to create the employee. If this pass null, then it creates the object</param>
        /// <param name="Name">Represents the name of the employee</param>
        /// <param name="Surname">Represents the surname of the employee</param>
        /// <param name="Gender">Represents the gender of the employee</param>
        /// <param name="StartDate">Indicates that when the created employee started to work on the company</param>
        /// <param name="Mail">Represents the employee's mail adress to contact with them</param>
        /// <param name="Phone">Represents the employee's phone adress to contact with them</param>
        /// <param name="Adress">Represents the home adress of the employee to contact with them</param>
        /// <param name="role">Sets the employee's role</param>
        /// <param name="AuthCode">Indicates the employee's private code to acess any actions</param>
        /// <returns>Employee</returns>
        public Employee CreateEmployee(string Name,string Surname, int Gender,DateTime StartDate,string Mail,string Phone,string Adress,Role role,string AuthCode)
        {
            Employee employee = new Employee();

            try
            {
            employee.Name = Name;
            employee.Surname = Surname;
            employee.Gender = Gender;
            employee.StartDate = StartDate.Date;
            employee.Mail = Mail;
            employee.Phone = Phone;
            employee.Adress = Adress;
            employee.role = role;
            employee.AuthCode = AuthCode;
            }
            catch (Exception e)
            {
                Controls.MessageBox.ShowExceptionBox(Ruby.Resources.Localization.EXC_FailToCreateEmp,null,e);
                HistoryInstance.SendMessage(AccessedEmployee,ScreenEnum.EMPLOYEEEDITOR,MessageType.ERROR, Ruby.Resources.Localization.ERROR_FailToCreateEmpMessage);
                return null;
            }
            return employee;
        }

        /// <summary>
        /// Removes employee from the system and database
        /// </summary>
        public void RemoveEmployee()
        {
                HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.EMPLOYEEEDITOR, MessageType.PROCESS,string.Format( Ruby.Resources.Localization.PROCESS_EmpDeleted, ActiveEmployee.Name, ActiveEmployee.Surname));

            db.DeleteEmployee(ActiveEmployee.AuthCode);

            EmployeesInstance.Remove(ActiveEmployee);

            EmployeeList.Items.RemoveAt(EmployeeList.SelectedIndex);

             RefreshEmployeeList();
        }
        /// <summary>
        /// Creates a role to assign employees permissions to access determined parts.
        /// </summary>
        /// <param name="Name">Name of the role</param>
        /// <param name="Values">Permissions values in type of boolean</param>
        /// <returns>Role</returns>
        public Role CreateRole(string Name, bool[] Values)
        {
            if (Values.Length > PermsLimit) throw new Exception("Max perm limit is 14...");

            Role rule = new Role();

            rule.Name = Name;
            if (Values == null) Values = new bool[PermsLimit];
            rule.Perms = Values;

            if (RolesInstance.Exists(r=> r.Name == rule.Name)) return new Role() { Name = null };

            return rule;
        }

        /// <summary>
        /// Removes selected role
        /// </summary>
        public void RemoveRole()
        {
            if (RolesListbox.SelectedItem == null)
                return;

            if (EmployeesInstance.FindAll(e => e.role.Name == ((Role)RolesListbox.SelectedItem).Name).Count > 0)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_RoleHasEmployeesTitle, Ruby.Resources.Localization.MB_RoleHasEmployeesMessage,MessageBoxButton.OK,MessageBoxImage.Error);

                return;
            }

            String RoleName = RolesListbox.SelectedItem.ToString();

            db.DeleteRole(RolesListbox.SelectedItem.ToString());
            RolesInstance.RemoveAt(RolesListbox.SelectedIndex);
            RolesListbox.Items.RemoveAt(RolesListbox.SelectedIndex);

            HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.EMPLOYEEEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_RoleDeleted,RoleName));
        }

        /// <summary>
        /// Edits employee which is already selected from listbox
        /// </summary>
        /// <param name="NewName"></param>
        /// <param name="NewSurname"></param>
        /// <param name="GenderIndex"></param>
        /// <param name="NewRole"></param>
        /// <param name="NewDate"></param>
        /// <param name="NewMail"></param>
        /// <param name="NewPhone"></param>
        /// <param name="NewAdress"></param>
        /// <param name="NewAuth"></param>
        public void EditEmployee(String NewName,String NewSurname,int GenderIndex,Role NewRole,DateTime NewDate,String NewMail,String NewPhone,String NewAdress, String NewAuth)
        {
            db.UpdateEmployee(NewName, NewSurname, GenderIndex, NewDate, NewMail, NewPhone, NewAdress, NewRole, NewAuth, ActiveEmployee.AuthCode);

            int index = EmployeesInstance.IndexOf(ActiveEmployee);

            bool changeit = false;
            if (ActiveEmployee.AuthCode == AccessedEmployee.AuthCode)
                changeit = true;

            if (string.IsNullOrWhiteSpace(AuthBox.Text))
                NewAuth = EmployeesInstance[index].AuthCode;

            EmployeesInstance[index] = new Employee() { Name = NewName, Surname = NewSurname, Gender = GenderIndex, StartDate = NewDate, Mail = NewMail, Adress = NewAdress, Phone = NewPhone, AuthCode = NewAuth, role = NewRole };

            if (changeit)
                AccessedEmployee = EmployeesInstance[index];
        }

        /// <summary>
        /// Refreshes employeeinstance, rolesinstance and listboxes. Especially requires to use when window has loaded
        /// </summary>
        private void RefreshEmployeeList()
        {
            RolesInstance = db.GetRoleList();

            RolesListbox.Items.Clear();

            foreach (var item in RolesInstance)
                RolesListbox.Items.Add(item);

            EmployeesInstance = db.GetEmployeeList(RolesInstance);

            EmployeeList.Items.Clear();

            foreach (var item in EmployeesInstance)
                EmployeeList.Items.Add(item);


        }

        /// <summary>
        /// Refreshes UI elements with their localized values
        /// </summary>
        private void ClearUI()
        {
            EmployeeNameBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultNameBoxTxt;
            EmployeeSurnameBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultSurnameBoxTxt;
            EmployeeGenderBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultGendertBoxTxt;
            EmployeeStartDateBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultDateBoxTxt;
            EmployeeMailBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultMailBoxTxt;
            EmployeePhoneBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultPhoneBoxTxt;
            EmployeeAdressBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultAdressBoxTxt;
            AuthBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultAuthCodeBoxTxt;

            RoleNameBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultRoleNameTxt;
            RoleAdder.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultAddRoleBtnTxt;
            RoleRemover.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultRemoveRoleBtnTxt;

            EmployeeGenderBox.Items.Add(Ruby.Resources.Localization.EmployeeEditor_DefaultGenderOtherTxt);
            EmployeeGenderBox.Items.Add(Ruby.Resources.Localization.EmployeeEditor_DefaultGenderMaleTxt);
            EmployeeGenderBox.Items.Add(Ruby.Resources.Localization.EmployeeEditor_DefaultGenderFemaleTxt);

            EmployeAdder.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultAddEmpBtnTxt;
            EmployeeEditer.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultEditEmpBtnTxt;
            EmployeRemover.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultRemEmpBtnTxt;

            if (EmployeeList.Items.Count > 0)EmployeeList.Items.Clear();

            AddPermissionItems();
        }

        /// <summary>
        /// Adds PermListBoxItem named checkboxes to PermissionLister, and gets ready to use it
        /// </summary>
        private void AddPermissionItems()
        {
            PermissionLister.Items.Clear();

            String[] Perms = new String[13]
               {
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent00,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent02,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent03,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent04,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent05,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent06,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent07,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent08,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent09,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent10,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent11,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent12,
              Ruby.Resources.Localization.EmployeeEditor_PermissionContent13,
               };

            for (int i = 0; i < Perms.Length; i++)
            {
                CheckBox cb = new CheckBox();
                cb.Content = Perms[i];
                if (i == 0)
                {
                    cb.Checked += (obj, ee)
                            =>
                    {
                        if (((CheckBox)obj).IsChecked.Value)
                            for (int k = 0; k < PermissionLister.Items.Count; k++)
                                ((CheckBox)PermissionLister.Items[k]).IsChecked = true;
                        else
                            for (int k = 0; k < PermissionLister.Items.Count; k++)
                                ((CheckBox)PermissionLister.Items[k]).IsChecked = false;
                    };
                }
                PermissionLister.Items.Add(cb);
            }

        }

        #endregion

        #region Events

        private void AddEmployeeBtn(object sender, RoutedEventArgs e)
        {
            if (RolesListbox.SelectedItem == null && RolesListbox.SelectedIndex == -1)
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_RoleNotSelectedTitle,Ruby.Resources.Localization.MB_RoleNotSelectedMessage, MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (!(string.IsNullOrWhiteSpace(EmployeeNameBox.Text) && string.IsNullOrWhiteSpace(EmployeeSurnameBox.Text) && string.IsNullOrWhiteSpace(EmployeeGenderBox.Text) || EmployeeGenderBox.SelectedIndex != 0 && string.IsNullOrWhiteSpace(EmployeeStartDateBox.Text) && string.IsNullOrWhiteSpace(EmployeeMailBox.Text) && string.IsNullOrWhiteSpace(EmployeePhoneBox.Text) && string.IsNullOrWhiteSpace(EmployeeAdressBox.Text) && string.IsNullOrWhiteSpace(AuthBox.Text)))
            {
                Employee emp = CreateEmployee(
                    EmployeeNameBox.Text.TrimEnd(' ').TrimStart(' '), EmployeeSurnameBox.Text.Trim(' '),
                    EmployeeGenderBox.SelectedIndex,
                    EmployeeStartDateBox.DisplayDate, EmployeeMailBox.Text, EmployeePhoneBox.Text,
                    EmployeeAdressBox.Text, ActiveRole,
                    AuthBox.Text);

                if (emp == null || EmployeesInstance.Find(em => em.AuthCode == emp.AuthCode) != null)
                    return;

                EmployeeList.Items.Add(emp);

                EmployeesInstance.Add(emp);

                db.AddEmployee(emp.Name, emp.Surname, emp.Gender, emp.StartDate, emp.Mail, emp.Phone, emp.Adress, emp.role.Name, emp.AuthCode);

                HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.EMPLOYEEEDITOR, MessageType.PROCESS,string.Format( Ruby.Resources.Localization.PROCESS_EmpCreated,emp.Name, emp.Surname));
            }
            else
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_FillRequiredAreasTitle, Ruby.Resources.Localization.MB_FillRequiredAreasMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            EmployeeList.SelectedIndex = EmployeeList.Items.Count - 1;
        }

        private void EditEmployeeBtn(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(EmployeeNameBox.Text) && string.IsNullOrWhiteSpace(EmployeeSurnameBox.Text) && string.IsNullOrWhiteSpace(EmployeeGenderBox.Text) && string.IsNullOrWhiteSpace(EmployeeStartDateBox.Text) && string.IsNullOrWhiteSpace(EmployeeMailBox.Text) && string.IsNullOrWhiteSpace(EmployeePhoneBox.Text) && string.IsNullOrWhiteSpace(EmployeeAdressBox.Text) && string.IsNullOrWhiteSpace(AuthBox.Text)) && EmployeeList.SelectedIndex != -1 && EmployeeList.SelectedItem != null && RolesListbox.SelectedIndex != -1 && RolesListbox.SelectedItem != null)
            {
                var OldEmpFullName = EmployeeList.SelectedItem;

                HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.EMPLOYEEEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_EmpChanged, OldEmpFullName, EmployeeNameBox.Text + EmployeeSurnameBox.Text));

                try
                {
                    EditEmployee(EmployeeNameBox.Text, EmployeeSurnameBox.Text, EmployeeGenderBox.SelectedIndex, ActiveRole, EmployeeStartDateBox.DisplayDate, EmployeeMailBox.Text, EmployeePhoneBox.Text, EmployeeAdressBox.Text, AuthBox.Text);

                }
                catch (Exception ex)
                {
                    Ruby.Cafe.Common.Controls.MessageBox.ShowExceptionBox(Ruby.Resources.Localization.EXC_FailToChangeEmp, null, ex);
                    return;
                }
               

                RefreshEmployeeList();
            }
            else
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_FillRequiredAreasTitle, Ruby.Resources.Localization.MB_FillRequiredAreasMessage, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveEmployeeBtn(object sender, RoutedEventArgs e)
        {
            if(ActiveEmployee != null)
                RemoveEmployee();
        }

        private void RemoveRoleBtn(object sender, RoutedEventArgs e)
        {
            if (RolesListbox.SelectedIndex != -1 && RolesListbox.SelectedItem != null)
                RemoveRole();
        }

        private void AddRoleBtn(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoleNameBox.Text))
                return;

            if (!RolesInstance.Exists(r => r.Name == RoleNameBox.Text))
            {
                bool[] checks = new bool[14];

                for (int i = 0; i < 14; i++)
                    checks[i] = ((CheckBox)PermissionLister.Items[i]).IsChecked.Value;

                Role rule = CreateRole(RoleNameBox.Text,checks);

                RolesInstance.Add(rule);
                db.AddRole(rule.Name, rule.Perms);
                RolesListbox.Items.Add(RolesInstance[RolesInstance.Count - 1]);
            }

            else
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_AlreadyExistsTitle, Ruby.Resources.Localization.MB_AlreadyExistsMessage, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private String BoxText;
        /// <summary>
        /// Happens when user focused in the specific text box
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">Routed event argument</param>
        private void GotFocus(object sender, RoutedEventArgs e)
        {
            BoxText = ((TextBox)sender).Text;
            ((TextBox)sender).Text = " ";
        }

        /// <summary>
        /// Happens when user changed focus on another UI item
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">Routed event argument</param>
        private void LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                ((TextBox)sender).Text = BoxText;
        }

        /// <summary>
        /// Happens when user changed focus on another UI item
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">Routed event argument</param>
        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                ((TextBox)sender).Text = BoxText;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RolesInstance = db.GetRoleList();
            EmployeesInstance = db.GetEmployeeList(RolesInstance);

        this.HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.EMPLOYEEEDITOR, MessageType.NOTIFICATION,Ruby.Resources.Localization.NOTIF_AccessedToScreen);

         ClearUI();

         RefreshEmployeeList();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            EmployeeList.Items.Clear();

            PermissionLister.Items.Clear();

            RolesListbox.Items.Clear();
        }

        private void EmployeeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeList.SelectedItem == null) return;

            ActiveEmployee = (Employee)EmployeeList.SelectedItem;
        }

        private void RoleChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RolesListbox.SelectedItem == null) return;

            ActiveRole = ActiveRole;
        }

        #endregion

    }
}
