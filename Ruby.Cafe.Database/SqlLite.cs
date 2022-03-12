using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruby.Cafe.Database
{
    public class SqlLite : IDatabase
    {
        public string ConnectionString { get; protected set; }

        public string Server { get; protected set; }

        public string DatabaseName { get; protected set; }

        public string UserID { get; protected set; }

        public string Password { get; protected set; }

        public bool Connected { get; protected set; }

        public SqlLite(string Server, string Database, string UserID, string Password)
        {
            var con = new SQLiteConnection(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.db;Version=3");

            try
            {
                con.Open();
            }
            catch (Exception e)
            {
                throw e;
            }

            con.Close();

            ConnectionString = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.db;Version=3";

            InitializeDBTables();
        }

        public bool CheckDBTable(string TableName)
        {
            using (var con = new SQLiteConnection(ConnectionString))
            {
                var cmd = new SQLiteCommand(con);
                int result = (int)cmd.ExecuteScalar();



            return true;
            }
        }

        public void InitializeDBTables()
        {

        }

        public void AddCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public void AddEmployee(string Name, string Surname, int Gender, DateTime StartDate, string Mail, string Phone, string Adress, string roleName, string AuthCode)
        {
            throw new NotImplementedException();
        }

        public void AddProduct(string ProductName, string CategoryName, double Tax, string Barcode)
        {
            throw new NotImplementedException();
        }

        public void AddRole(string RoleName, bool[] Perms)
        {
            throw new NotImplementedException();
        }

        public void AddScences(string ScenceName)
        {
            throw new NotImplementedException();
        }

        public void AddServing(string Serving, int Quantity, decimal Price, string ProductName)
        {
            throw new NotImplementedException();
        }

        public void AddTable(string TableName, Scence scence, string TableType, string TableColor, int MaxChair, string Checks, int Statue, string Description)
        {
            throw new NotImplementedException();
        }

        public void AddTicket(Table table, Employee employee, DateTime CreatedDate)
        {
            throw new NotImplementedException();
        }

        public void AddTicketProduct(int TicketID, Product product, int ServingID, int Multiplier)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public void DeleteEmployee(string AuthCode)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(string ProductName)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(string RoleName)
        {
            throw new NotImplementedException();
        }

        public void DeleteScence(string ScenceName)
        {
            throw new NotImplementedException();
        }

        public void DeleteServing(string ServingName, string ProductName)
        {
            throw new NotImplementedException();
        }

        public void DeleteTable(string TableName)
        {
            throw new NotImplementedException();
        }

        public void DeleteTicket(Table table)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTicketProduct(int TableID, int ProductID)
        {
            throw new NotImplementedException();
        }

        public void DeleteTicketProducts(int TicketID)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoryList()
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployee(string AuthCode)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetEmployeeList(List<Role> Roles)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessages()
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(string ProductName)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductList(List<Category> Categories)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(string RoleName)
        {
            throw new NotImplementedException();
        }

        public List<Role> GetRoleList()
        {
            throw new NotImplementedException();
        }

        public Scence GetScence(string ScenceName)
        {
            throw new NotImplementedException();
        }

        public List<Scence> GetScenceList()
        {
            throw new NotImplementedException();
        }

        public List<ServingItem> GetServings(int ProductID)
        {
            throw new NotImplementedException();
        }

        public Table GetTable(string TableName)
        {
            throw new NotImplementedException();
        }

        public List<Table> GetTableList(List<Scence> Scences)
        {
            throw new NotImplementedException();
        }

        public Ticket GetTicket(Table table)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetTicketProducts(int TicketID, List<Product> Products)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetTickets()
        {
            throw new NotImplementedException();
        }

        public int GetTotalActiveTables(DateTime date)
        {
            throw new NotImplementedException();
        }

        public int GetTotalSales(DateTime date)
        {
            throw new NotImplementedException();
        }

        public int GetTotalSoldProducts(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void SendHistoryMessage(string Message, int MessageType, DateTime MessageDate, string AuthCode, int screen, int? ProductID, int? TableID)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployee(string Name, string Surname, int Gender, DateTime StartDate, string Mail, string Phone, string Adress, Role role, string AuthCode, string CAuthCode)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(string ProductName, string CategoryName, double Tax, string Barcode, string CProductName)
        {
            throw new NotImplementedException();
        }

        public void UpdateTable(string TableName, string scence, string TableType, string TableColor, int MaxChair, string Checks, int Statue, string Description, string CTableName)
        {
            throw new NotImplementedException();
        }

        public void UpdateTicket(Table table, Table CTable)
        {
            throw new NotImplementedException();
        }
    }
}
