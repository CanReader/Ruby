using Ruby.Setup.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Ruby.Setup.Views
{
    /// <summary>
    /// Interaction logic for FirstEmployees.xaml
    /// </summary>
    public partial class FirstEmployees : Page
    {
        public SetupWindow MainWindow;

        public List<Role> Roles;
        public List<Employee> Employees;

        bool stop = false;

        public FirstEmployees(SetupWindow MainWindow)
        {
            InitializeComponent();

            this.Loaded += (ssender, ee) =>
                ClearUI();

            this.MainWindow = MainWindow;

            AddPerms();

           if(Roles == null) Roles = new List<Role>();
           if(Employees == null)Employees = new List<Employee>();
        }

        private void ClearUI()
        {
            if (Employees != null && Employees.Count > 0)
            {
                EmployeeList.Items.Clear();
                foreach (var employee in Employees)
                    EmployeeList.Items.Add(employee);
            }

            if (Roles != null && Roles.Count > 0)
            {
                RoleBox.Items.Clear();
                foreach (var role in Roles)
                    RoleBox.Items.Add(role);
            }

            PageTitle.Text = Ruby.Resources.Localization.SetupScreen_FirstEmployeesTitle;

            EmpNameBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultNameBoxTxt;
            EmpSurnameBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultSurnameBoxTxt;
            EmpGendrBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultGendertBoxTxt;
            EmpPhoneBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultPhoneBoxTxt;
            EmpMailBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultMailBoxTxt;
            EmpAdressBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultAdressBoxTxt;
            EmpAuthCodeBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultAuthCodeBoxTxt;
            EmpGendrBox.Items.Add(Ruby.Resources.Localization.EmployeeEditor_DefaultGenderOtherTxt);
            EmpGendrBox.Items.Add(Ruby.Resources.Localization.EmployeeEditor_DefaultGenderMaleTxt);
            EmpGendrBox.Items.Add(Ruby.Resources.Localization.EmployeeEditor_DefaultGenderFemaleTxt);

            RoleNameBox.Text = Ruby.Resources.Localization.EmployeeEditor_DefaultRoleNameTxt;

            AddEmpBtn.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultAddEmpBtnTxt;
            RemEmpBtn.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultRemEmpBtnTxt;
            AddRoleBtn.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultAddRoleBtnTxt;
            RemRoleBtn.Content = Ruby.Resources.Localization.EmployeeEditor_DefaultRemoveRoleBtnTxt;

            NextBtn.Content = Ruby.Resources.Localization.SetupPage_DefaultNextBtnTxt;
        }

        private void AddPerms() 
        {
            PermBox.Items.Clear();
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
            if(((CheckBox)obj).IsChecked.Value)
              for (int k = 0; k < PermBox.Items.Count; k++)
                    ((CheckBox)PermBox.Items[k]).IsChecked = true;
                    else
              for (int k = 0; k < PermBox.Items.Count; k++)
                    ((CheckBox)PermBox.Items[k]).IsChecked = false;
                    };
                }
                PermBox.Items.Add(cb);
            }

        }

        private Employee AddEmployee()
        {
            Employee emp = new Employee();

            if (!string.IsNullOrWhiteSpace(EmpNameBox.Text) && !string.IsNullOrWhiteSpace(EmpSurnameBox.Text) && !string.IsNullOrWhiteSpace(EmpMailBox.Text) && !string.IsNullOrWhiteSpace(EmpPhoneBox.Text) && !string.IsNullOrWhiteSpace(EmpAdressBox.Text) && !string.IsNullOrWhiteSpace(EmpAuthCodeBox.Text) &&
                int.TryParse(EmpAuthCodeBox.Text, out int auth) &&
                EmpGendrBox.SelectedIndex != -1 &&
                StartDateBox.SelectedDate != null &&
                RoleBox.SelectedItem != null && Employees.Find(em => em.Name == EmpNameBox.Text && em.Surname == EmpSurnameBox.Text) == null && Employees.Find(em => em.AuthCode == EmpAuthCodeBox.Text) == null)
            {
                emp.Name = EmpNameBox.Text;
                emp.Surname = EmpSurnameBox.Text;
                emp.Gender = EmpGendrBox.SelectedIndex;
                emp.StartDate = StartDateBox.SelectedDate.Value;
                emp.Mail = EmpMailBox.Text;
                emp.Phone = EmpPhoneBox.Text;
                emp.Adress = EmpAdressBox.Text;
                emp.AuthCode = EmpAuthCodeBox.Text;
                emp.role = ((Role)RoleBox.SelectedItem);
            }
            else return null;

            Employees.Add(emp);

            return emp;
        }

        private Role AddRole()
        {
            Role rule = new Role();

            if (!string.IsNullOrWhiteSpace(RoleNameBox.Text) && Roles.Find(r => r.Name == rule.Name).Perms == null)
            {
                bool[] perms = new bool[14];
                for (int i = 0; i < PermBox.Items.Count; i++)
                    perms[i] = ((CheckBox)PermBox.Items[i]).IsChecked.Value;

                rule.Name = RoleNameBox.Text;
                rule.Perms = perms;
            }
            else return new Role { ID = -1};

            Roles.Add(rule);

            return rule;
        }

        private void RemoveEmployee()
        {
            Employees.Remove((Employee)EmployeeList.SelectedItem);
            EmployeeList.Items.RemoveAt(EmployeeList.SelectedIndex);
        }

        private void RemoveRole()
        {
            
            Roles.Remove((Role)RoleBox.SelectedItem);
            RoleBox.Items.RemoveAt(RoleBox.SelectedIndex);
        }

        public void AddEmployee(object sender, RoutedEventArgs e)
        {
            if (RoleBox.SelectedItem == null)
                MessageBox.Show(Ruby.Resources.Localization.MB_RoleNotSelectedMessage);

            Employee emp = AddEmployee();

            if(emp != null)
            EmployeeList.Items.Add(emp);
        }

        public void RemoveEmployee(object sender, RoutedEventArgs e)
        {
            if (EmployeeList.SelectedItem == null)
                return;
            RemoveEmployee();
        }

        public void AddRole(object sender, RoutedEventArgs e)
        {
            Role rule = AddRole();

            if(rule.ID != -1)
            RoleBox.Items.Add(rule);
        }

        public void RemoveRole(object sender, RoutedEventArgs e)
        {
            if (RoleBox.SelectedItem == null)
                return;
            RemoveRole();
        }

        public void NextPage(object sender, RoutedEventArgs e)
        {
            if (Employees.Count == 0)
            { 
                MessageBox.Show(Ruby.Resources.Localization.SetupScreen_EmployeeRequired);
                return;
            }

            if (Employees.Find(x => x.role.Perms[0]) == null)
            {
                MessageBox.Show(Ruby.Resources.Localization.SetupScreen_EmployeeRequired);
                return;
            }

            LastPage ls = new LastPage(MainWindow,Roles,Employees);
            MainWindow.Content = ls;
            
        }

        private void FillEmployee(Employee e)
        {
            EmpNameBox.Text = e.Name;
            EmpSurnameBox.Text = e.Surname;
            EmpGendrBox.SelectedIndex = e.Gender;
            EmpMailBox.Text = e.Mail;
            EmpAdressBox.Text = e.Adress;
            StartDateBox.DisplayDate = e.StartDate;
            EmpPhoneBox.Text = e.Phone;
            EmpAuthCodeBox.Text = e.AuthCode;
            RoleBox.SelectedItem = e.role;
        }

        private void EmployeeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeList.SelectedItem == null)
                return;

            Employee emp = EmployeeList.SelectedItem as Employee;

            FillEmployee(emp);
            
        }
    }
}
