using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Win32;
using Ruby.Cafe.Model;

namespace Ruby.Cafe.Database
{
    public class Sql : IDatabase
    {
        private bool Trusted_Connection { get; set; }

        private SqlConnection connection;

        public System.Data.ConnectionState State { get => connection.State; }

        public string ConnectionString { get; protected set; }

        public string Server { get; protected set; }

        public string DatabaseName { get; protected set; }

        public string UserID { get; protected set; }

        public string Password { get; protected set; }

        public bool Connected { get; private set; }

        /// <summary>
        /// Creates a instance of Sql class with standart security 
        /// </summary>
        /// <param name="Server">Server name which will be connected</param>
        /// <param name="Database">Database name which will be using</param>
        /// <param name="UserID">Which user is connecting to the database server</param>
        /// <param name="Password">Password of the user for connect</param>
        public Sql(string Server, string Database, string UserID,string Password)
        {
            this.ConnectionString = String.Format("Server={0};Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True",Server,Database,UserID,Password);

            this.Server = Server;
            this.DatabaseName = Database;
            this.UserID = UserID;
            this.Password = Password;
            this.Trusted_Connection = true;

            try
            {
                connection = new SqlConnection(String.Format("Server={0};Database=master;User ID={1};Password={2};Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True", Server.Trim(),UserID.Trim(),Password.Trim()));

                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = '" + Database + "'",connection);
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand("CREATE DATABASE " + Database, connection);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception e)
            {
                this.Connected = false;
                throw e;
            }

            connection = new SqlConnection(ConnectionString);
            connection.Open();
                if (connection.State == System.Data.ConnectionState.Open) { this.Connected = true; connection.Close(); }
            
            InitializeDBTables();
        }

        /// <summary>
        /// Creates a instance of Sql class with trusted connection
        /// </summary>
        /// <param name="Server">Server name which will be connected</param>
        /// <param name="Database">Database name which will be using</param>
        /// <param name="Trusted_Connection">Will it support trusted connection or nor (I don't know what it means :D)</param>
        public Sql(string Server, string Database, bool Trusted_Connection)
        {
            this.Connected = false;

            this.ConnectionString = $"Server={Server};Database={Database};Trusted_Connection={Trusted_Connection}";

            this.Server = Server;
            this.DatabaseName = Database;
            this.UserID = null;
            this.Password = null;
            this.Trusted_Connection = Trusted_Connection;

            try
            {
                connection = new SqlConnection($"Server={Server};Database=master;Trusted_Connection={Trusted_Connection}");

                connection.Open();

                SqlCommand cmd = new SqlCommand($"SELECT * FROM sys.databases WHERE name = '{Database}'", connection);
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand($"CREATE DATABASE {Database}", connection);
                    cmd.ExecuteNonQuery();

                }

                connection.Close();
            }
            catch (Exception e)
            {
                this.Connected = false;
                throw e;
            }

                connection = new SqlConnection(ConnectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open) { Connected = true; connection.Close(); }

            InitializeDBTables();
        }

        /// <summary>
        /// Creates a instance of Sql class with initial catalog
        /// </summary>
        /// <param name="Server">Server's name</param>
        /// <param name="InitialCatalog">Database's name to connect</param>
        public Sql(string Server, string InitialCatalog)
        {
            this.Server = Server;
            this.DatabaseName = InitialCatalog;

            ConnectionString = $"Data Source={Server};Initial Catalog={InitialCatalog};Integrated Security=SSPI;";

            try
            {
                connection = new SqlConnection($"Data Source={Server};Initial Catalog={InitialCatalog};Integrated Security=SSPI;");

                connection.Open();

                SqlCommand cmd = new SqlCommand($"SELECT * FROM sys.databases WHERE name = '{InitialCatalog}'", connection);
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand($"CREATE DATABASE {InitialCatalog}", connection);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception e)
            {
                this.Connected = false;
                throw e;
            }

            connection = new SqlConnection(ConnectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open) { this.Connected = true; connection.Close(); }

            InitializeDBTables();
        }
        
        public void InitializeDBTables()
        {
            //Scences-Categories-Roles-Tables-Employees-Products-Servings-Tickets-TicketProducts-AccountArchives-AccountProducts-History
            if (!CheckDBTable("Scences"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Scences(ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,Name VARCHAR(15) NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Categories"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Categories(ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,Name VARCHAR(14) NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Roles"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Roles(ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,Name VARCHAR(18) NOT NULL,Perms CHAR(27) NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Tables"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Tables(ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,Name VARCHAR(24) NOT NULL,TableScence INT NOT NULL,TableType VARCHAR(17) NOT NULL,TableColor VARCHAR(6) NOT NULL,MaxChair TINYINT NOT NULL,Checks VARCHAR(5) NOT NULL,StatueID TINYINT,Description Text)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Employees"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Employees(ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,Name VARCHAR(18) NOT NULL,Surname VARCHAR(16) NOT NULL,Gender INT NOT NULL,StartDate DATE,Mail NVARCHAR(27) NOT NULL,Phone VARCHAR(11) NOT NULL,Adress TEXT,RoleID INT NOT NULL,AuthCode CHAR(4) NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Products"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Products(ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,Name VARCHAR(17) NOT NULL,CategoryID INT NOT NULL,TaxRatio FLOAT NOT NULL,Barcode VARCHAR(13))",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Servings"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE Servings(ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,Serving VARCHAR(12) NOT NULL,Quantity INT NOT NULL,Price DECIMAL NOT NULL,ProductID INT NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Tickets"))
            {
                connection.Open();                                      
                SqlCommand command = new SqlCommand("CREATE TABLE Tickets(ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,TableID INT NOT NULL,EmployeeID INT NOT NULL,CreatedDate Date NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("TicketProducts"))
            {
                connection.Open();                       // @TicketID,@ProductID,@Serving,@Multiplier
                SqlCommand command = new SqlCommand("CREATE TABLE TicketProducts(ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, TicketID INT NOT NULL, ProductID INT NOT NULL, ServingID INT NOT NULL, Multiplier INT NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("AccountArchives"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE AccountArchives(ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,TableID INT NOT NULL,EmployeeID INT NOT NULL,TotalPrice DECIMAL NOT NULL,TotalTax DECIMAL NOT NULL,PaymentType INT NOT NULL,CreatedDate DATE NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("AccountProducts"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE AccountProducts(ID INT NOT NULL IDENTITY(1, 1),ProductID INT NOT NULL ,Quantity INT NOT NULL,AccountID INT)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("History"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE TABLE History(ID int IDENTITY(1, 1) NOT NULL PRIMARY KEY,Message TEXT NOT NULL,Type INT NOT NULL,Date DateTime NOT NULL,EmployeeID INT NOT NULL,Screen INT NOT NULL)",connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            
        }

        public void AddScences(string ScenceName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if(CheckDBTable("Scences"))
             {

            SqlCommand cmd = new SqlCommand("INSERT INTO Scences VALUES(@Name)", connection);
             cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = ScenceName;

            cmd.ExecuteNonQuery();

            }

            if(alreadyopened)
                connection.Close();
        }

        public void AddTable(string TableName, Scence scence, string TableType, string TableColor, int MaxChair, string Checks, int Statue, string Description)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Tables"))
            {
                    int ScenceID;

                    SqlCommand command = new SqlCommand("SELECT ID FROM Scences WHERE Name = '" + scence.Name + "'",connection);
                    try { ScenceID = (int)command.ExecuteScalar(); }
                    catch (Exception) { ScenceID = 0; }

                command = new SqlCommand("INSERT INTO Tables VALUES(@TableName,@TableScence,@TableType,@TableColor,@MaxChair,@Checks,@Statue,@Description)", connection);
                command.Parameters.Add("@TableName", System.Data.SqlDbType.VarChar).Value = TableName;
                command.Parameters.Add("@TableScence", System.Data.SqlDbType.Int).Value = ScenceID;
                command.Parameters.Add("@TableType", System.Data.SqlDbType.VarChar).Value = TableType;
                command.Parameters.Add("@TableColor", System.Data.SqlDbType.VarChar).Value = TableColor;
                command.Parameters.Add("@MaxChair", System.Data.SqlDbType.TinyInt).Value = MaxChair;
                command.Parameters.Add("@Checks", System.Data.SqlDbType.VarChar).Value = Checks;
                command.Parameters.Add("@Statue", System.Data.SqlDbType.TinyInt).Value = Statue;
                command.Parameters.Add("@Description", System.Data.SqlDbType.Text).Value = Description;

                command.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void AddEmployee(string Name, string Surname, int Gender, DateTime StartDate, string Mail, string Phone, string Adress, String roleName, string AuthCode)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

                int roleID;

                SqlCommand command = new SqlCommand("SELECT ID FROM Roles WHERE Name = '" + roleName + "'",connection);
                try { roleID = (int)command.ExecuteScalar(); }
                catch { roleID = 1;}

                command = new SqlCommand("INSERT INTO Employees VALUES(@Name,@Surname,@Gender,@StartDate,@Mail,@Phone,@Adress, @roleID, @AuthCode )", connection);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Surname", Surname);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@StartDate", StartDate);
                command.Parameters.AddWithValue("@Mail", Mail);
                command.Parameters.AddWithValue("@Phone", Phone);
                command.Parameters.AddWithValue("@Adress",Adress);
                command.Parameters.AddWithValue("@roleID",roleID);
                command.Parameters.AddWithValue("@AuthCode", AuthCode);

                command.ExecuteNonQuery();

            if(alreadyopened)
            connection.Close();
        }

        public void AddRole(string RoleName, bool[] Perms)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (Perms[0])
                for (int k = 0; k < Perms.Length; k++)
                    Perms[k] = true;

            int TotalVLines = Perms.Length - 1;

            System.Text.StringBuilder builder = new System.Text.StringBuilder(Perms.Length + TotalVLines);

            int i = 0;

            do
            {
                builder.AppendFormat("{0}|", Convert.ToInt32(Perms[i]).ToString());
                i++;
            } while (i < Perms.Length);

            builder.Remove(builder.Length - 1, 1);

            String PermString = builder.ToString();

            SqlCommand command = new SqlCommand("INSERT INTO Roles VALUES(@Name, @Perms)", connection);
            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = RoleName;
            command.Parameters.Add("@Perms", System.Data.SqlDbType.VarChar).Value = PermString;

            command.ExecuteNonQuery();

            if(!alreadyopened)
            connection.Close();
        }

        public void AddCategory(string CategoryName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CategoryName != null)
            {
                SqlCommand command = new SqlCommand("INSERT INTO Categories VALUES(@CategoryName)", connection);
                command.Parameters.Add("@CategoryName", System.Data.SqlDbType.VarChar).Value = CategoryName;

                command.ExecuteNonQuery();
            }

            if(connection.State == ConnectionState.Open)
            connection.Close();
        }

        public void AddProduct(string ProductName, string CategoryName, double Tax, string Barcode)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (GetProduct(ProductName) == null)
            {
                int CategoryID;

                SqlCommand command = new SqlCommand("SELECT ID FROM Categories WHERE Name = '" + CategoryName + "'",connection);

                 CategoryID = (int)command.ExecuteScalar();
               
                command = new SqlCommand("INSERT INTO Products VALUES(@ProductName,@CategoryID,@TaxRatio,@Barcode)", connection);
                command.Parameters.Add("@ProductName", System.Data.SqlDbType.VarChar).Value = ProductName;
                command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = CategoryID;
                command.Parameters.Add("@TaxRatio", System.Data.SqlDbType.Float).Value = Tax;
                command.Parameters.Add("@Barcode", System.Data.SqlDbType.VarChar).Value = Barcode;

                command.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void AddServing(string Serving, int Quantity, decimal Price, string ProductName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Servings"))
            {
                SqlCommand command = new SqlCommand("SELECT ID FROM Products  WHERE Name = '" + ProductName + "'", connection);

                int pindex = (int)command.ExecuteScalar();

                command = new SqlCommand("INSERT INTO Servings VALUES(@Serving,@Quantity,@Price,@ProductID)",connection);
                command.Parameters.Add("@Serving",System.Data.SqlDbType.VarChar).Value=Serving;
                command.Parameters.Add("@Quantity",System.Data.SqlDbType.Int).Value=Quantity;
                command.Parameters.Add("@Price",System.Data.SqlDbType.Decimal).Value=Price;
                command.Parameters.Add("@ProductID",System.Data.SqlDbType.Int).Value=pindex;

                command.ExecuteNonQuery();
            }

            if (alreadyopened)
                connection.Close();
        }

        public Model.Scence GetScence(string ScenceName)
        {
            Model.Scence scence = new Scence();

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

          SqlCommand command = new SqlCommand("SELECT * FROM Scences WHERE Name = @ScenceName", connection);
                command.Parameters.Add("@ScenceName", System.Data.SqlDbType.VarChar).Value = ScenceName;

       SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    scence = new Model.Scence();
                    while (reader.Read())
                    {
                        scence.ID = reader.GetInt32(0);
                        scence.Name = reader["Name"].ToString();
                    }
               
            }

            reader.Close();

            if(connection.State == ConnectionState.Open)
            connection.Close();

            return scence;
        }

        public Model.Table GetTable(string TableName)
        {
            Model.Table table = null;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            // Check if Database table exist
            if (CheckDBTable("Tables"))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Tables WHERE Name = '@TableName'", connection);
                command.Parameters.Add("@TableName", System.Data.SqlDbType.VarChar).Value = TableName;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    table = new Model.Table();
                    while (reader.Read())
                    {
                        SqlCommand comand = new SqlCommand("SELECT Name FROM Scences WHERE ID = '" + reader["TableScence"].ToString() + "'", connection);

                        string ScenceName = command.ExecuteScalar().ToString();

                        table.Name = reader["Name"].ToString();
                        table.TableScence = ScenceName;
                        table.TableType = reader["TableType"].ToString();
                        table.TableColor = reader["TableColor"].ToString();
                        table.MaxChair = Convert.ToInt32(reader["MaxChair"].ToString());
                        table.Checks = table.ConvertStringToBoolArray(reader["Checks"].ToString());
                        table.CurrentStatue = (TableStatue)Convert.ToInt32(reader["StatueID"]);
                        table.Description = reader["Description"].ToString();
                    }
                }
                else
                {
                    Console.WriteLine(table + " is not exist in the Database!");
                }
                reader.Close();
            }

            if(alreadyopened)
            connection.Close();

            return table;
        }

        public Model.Table GetTable(int ID)
        {
            Model.Table table = null;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            // Check if Database table exist
            if (CheckDBTable("Tables"))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Tables WHERE ID = '@ID'", connection);
                command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = ID;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    table = new Model.Table();
                    while (reader.Read())
                    {
                        SqlCommand comand = new SqlCommand("SELECT Name FROM Scences WHERE ID = '" + reader["TableScence"].ToString() + "'", connection);

                        string ScenceName = command.ExecuteScalar().ToString();

                        table.Name = reader["Name"].ToString();
                        table.TableScence = ScenceName;
                        table.TableType = reader["TableType"].ToString();
                        table.TableColor = reader["TableColor"].ToString();
                        table.MaxChair = Convert.ToInt32(reader["MaxChair"].ToString());
                        table.Checks = table.ConvertStringToBoolArray(reader["Checks"].ToString());
                        table.CurrentStatue = (TableStatue)Convert.ToInt32(reader["StatueID"]);
                        table.Description = reader["Description"].ToString();
                    }
                }
                else
                {
                    Console.WriteLine(table + " is not exist in the Database!");
                }
                reader.Close();
            }

            if (alreadyopened)
                connection.Close();

            return table;
        }

        public Model.Employee GetEmployee(string AuthCode)
        {
            Model.Employee employee = null;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            // Check if Database table exist
            if (CheckDBTable("Employees"))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Employees WHERE AuthCode = '@Auth'", connection);
                command.Parameters.Add("@Auth", System.Data.SqlDbType.VarChar).Value = AuthCode;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                employee = new Model.Employee();
                    while (reader.Read())
                    {
                        
                        employee.Name = reader["Name"].ToString();
                        employee.Surname = reader["Surname"].ToString();
                        employee.Gender = int.Parse(reader["Gender"].ToString());
                        employee.StartDate = (DateTime)reader["StartDate"];
                        employee.Mail = reader["Mail"].ToString();
                        employee.Phone = reader["Phone"].ToString();
                        employee.Adress = reader["Adress"].ToString();
                        employee.role = GetRole(int.Parse(reader["RoleID"].ToString()));
                        employee.AuthCode = reader["AuthCode"].ToString();
                    }
                }
                else
                {
                    Console.WriteLine(employee.ToString() + " is not exist in the Database!");
                }
                reader.Close();
            }

            if(alreadyopened)
            connection.Close();

            return employee;
        }

        public Model.Employee GetEmployee(int ID)
        {
            Model.Employee employee = null;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            // Check if Database table exist
            if (CheckDBTable("Employees"))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Employees WHERE ID = @ID", connection);
                command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = ID;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    employee = new Model.Employee();
                    while (reader.Read())
                    {

                        employee.Name = reader["Name"].ToString();
                        employee.Surname = reader["Surname"].ToString();
                        employee.Gender = int.Parse(reader["Gender"].ToString());
                        employee.StartDate = (DateTime)reader["StartDate"];
                        employee.Mail = reader["Mail"].ToString();
                        employee.Phone = reader["Phone"].ToString();
                        employee.Adress = reader["Adress"].ToString();
                        employee.role = GetRole(int.Parse(reader["RoleID"].ToString()));
                        employee.AuthCode = reader["AuthCode"].ToString();
                    }
                }
                else
                {
                    Console.WriteLine(employee.ToString() + " is not exist in the Database!");
                }
                reader.Close();
            }

            if (alreadyopened)
                connection.Close();

            return employee;
        }

        public Model.Role GetRole(string RoleName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            Model.Role role = new Model.Role();
            SqlCommand Command = new SqlCommand("SELECT * FROM Roles WHERE Name = '" + RoleName + "'", connection);
            SqlDataReader reader = Command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                {
                    role.Name = reader["Name"].ToString();
                    role.Perms = role.ConvertStringToPerms(reader["Perms"].ToString());
                }

            if (alreadyopened)
                connection.Close();

            return role;
        }

        public Model.Role GetRole(int ID)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            Model.Role role = new Model.Role();
            SqlCommand Command = new SqlCommand("SELECT * FROM Roles WHERE ID = " + ID.ToString(), connection);
            SqlDataReader reader = Command.ExecuteReader();
            if(reader.HasRows)
                while (reader.Read())
                {
                    role.Name = reader["Name"].ToString();
                    role.Perms = role.ConvertStringToPerms(reader["Perms"].ToString());
                }

            if (alreadyopened)
                connection.Close();

            return role;
        }

        public Model.Product GetProduct(string ProductName)
        {
            Model.Product product = null;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Name = '@Name'", connection);
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = ProductName;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                product = new Model.Product();
                    while (reader.Read())
                    {
                        SqlCommand comand = new SqlCommand("SELECT Name FROM Categories WHERE = " + reader["CategoryID"].ToString(), connection);
                        string CategoryName = comand.ExecuteScalar().ToString();

                        List<ServingItem> ServingList = GetServings(int.Parse(reader["ID"].ToString()));

                        product.Name = reader["Name"].ToString();
                        //product.CategoryName = CategoryName;
                        product.Tax = Convert.ToDouble(reader["TaxRatio"]);
                        product.Barcode = reader["Barcode"].ToString();
                        product.Servings = ServingList;
                    }
            }

            reader.Close();

            if(alreadyopened)
            connection.Close();

            return product;
        }

        public bool CheckDBTable(string TableName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE name = '"+TableName+"'", connection);

            int Value = (int)command.ExecuteScalar();

            if(alreadyopened)
                connection.Close();

            if (Value != 0) return true;
            else return false;
        }

        public List<Scence> GetScenceList()
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<Scence> ScenceList = new List<Scence>();

            System.Data.DataTable dt = new System.Data.DataTable();

            SqlCommand command = new SqlCommand("SELECT * FROM Scences", connection);

            SqlDataReader dr = command.ExecuteReader();

            if(dr.HasRows)
                while (dr.Read())
                    ScenceList.Add(new Scence()
                    {
                        ID = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });

            dr.Close();

            if(alreadyopened)
            connection.Close();

            return ScenceList;
        }

        public List<Table> GetTableList(List<Scence> Scences)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<Table> TableList = new List<Table>();

            if (CheckDBTable("Tables"))
            {

            SqlCommand command = new SqlCommand("SELECT * FROM Tables", connection);

            SqlDataReader dr = command.ExecuteReader();

             if(dr.HasRows)
               while (dr.Read())
               {
                  Table table = new Table();

                  table.ID = dr.GetInt32(0);
                  table.Name = dr["Name"].ToString();
                  table.TableScence = Scences.Find(p=> p.ID == dr.GetInt32(2)).Name;
                  table.TableType = dr["TableType"].ToString();
                  table.TableColor = dr["TableColor"].ToString();
                  table.MaxChair = Convert.ToInt32(dr["MaxChair"].ToString());
                  table.Checks = table.ConvertStringToBoolArray(dr["Checks"].ToString());
                  table.CurrentStatue = (TableStatue)Convert.ToInt32(dr["StatueID"]);
                  table.Description = dr["Description"].ToString();

                  TableList.Add(table);
               }

            dr.Close();

            if(alreadyopened)
            connection.Close();
            }

            return TableList;
        }

        public List<Category> GetCategoryList()
        {
            List<Category> Categories = new List<Category>();

            if (CheckDBTable("Categories"))
            {
                bool alreadyopened = false;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                    alreadyopened = true;
                }

                SqlCommand command = new SqlCommand("SELECT * FROM Categories",connection);

                SqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                    while (dr.Read())
                    {
                        Categories.Add(new Category()
                        {
                            ID = dr.GetInt32(0),
                            Name = dr.GetString(1)
                        });
                    }

                dr.Close();

                if (alreadyopened)
            connection.Close();

            }

            return Categories;
        }

        public List<Product> GetProductList(List<Category> Categories)
        {
            List<Product> ProductList = new List<Product>();

                bool alreadyopened = false;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                    alreadyopened = true;
                }


            SqlCommand command = new SqlCommand("SELECT * FROM Products", connection);

                SqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                    while (dr.Read())
                    {
                        ProductList.Add(new Product
                        {
                            ID = dr.GetInt32(0),
                            Name = dr.GetString(1),
                            category = Categories.Find(p => p.ID == dr.GetInt32(2)),
                            Tax = double.Parse(dr["TaxRatio"].ToString()),
                           Barcode = dr["Barcode"].ToString()
                        });
                    }

                dr.Close();

                foreach (var item in ProductList)
                {
                    item.Servings = new List<ServingItem>();

                command = new SqlCommand("SELECT * FROM Servings WHERE ProductID = " + item.ID,connection);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataTable dt = new DataTable(); 
                    sda.Fill(dt);

                if(dt.Rows.Count > 0)
                    foreach(DataRow ss in dt.Rows)
                     ProductList[ProductList.IndexOf(item)].Servings = GetServings(int.Parse(ss["ProductID"].ToString()));

                    sda.Dispose();
                    dt.Dispose();
                }

                if (alreadyopened)
            connection.Close();


            return ProductList;
        }

        public List<Role> GetRoleList()
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<Role> RoleList = new List<Role>();

            SqlCommand command = new SqlCommand("SELECT * FROM Roles", connection);

            SqlDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
                while (dr.Read())
                {
                    Role rule = new Role();

                    rule.ID = dr.GetInt32(0);
                    rule.Name = dr["Name"].ToString();
                    rule.Perms = rule.ConvertStringToPerms(dr["Perms"].ToString());
                    RoleList.Add(rule);
                }

            dr.Close();

            if (alreadyopened)
                connection.Close();

            return RoleList;
        }

        public List<Ticket> GetTickets()
        {
            List<Ticket> list = new List<Ticket>();

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("SELECT * FROM Tickets", connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
                while (dr.Read())
                {
                    Ticket ticket = new Ticket();

                    list.Add(ticket);
                }

            dr.Close();

            if (alreadyopened)
                connection.Close();

            return list;
        }

        public List<ServingItem> GetServings(int ProductID)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<ServingItem> list = new List<ServingItem>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Servings WHERE ProductID = " + ProductID, connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ServingItem si = new ServingItem();

                    si.Serving = dr["Serving"].ToString();
                    si.Quantity = int.Parse(dr["Quantity"].ToString());
                    si.Price = Convert.ToDecimal(dr["Price"]);

                    list.Add(si);
                }
            }

            dr.Close();

            if (alreadyopened)
                connection.Close();

            return list;
        }

        public List<Employee> GetEmployeeList(List<Role> Roles)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<Employee> EmployeeList = new List<Employee>();

            System.Data.DataTable dt = new System.Data.DataTable();

            SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection);

            SqlDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
                while (dr.Read())
                {
                    EmployeeList.Add(new Employee()
                    {
                        ID = dr.GetInt32(0),
                        Name = dr["Name"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        Gender = Convert.ToInt32(dr["Gender"]),
                        StartDate = dr.GetDateTime(4),
                        Mail = dr["Mail"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        Adress = dr["Adress"].ToString(),
                        AuthCode = dr["AuthCode"].ToString(),
                        role = Roles.Find(p=> p.ID == Convert.ToUInt32(dr["RoleID"]))
                    });
                }

            dr.Close();

            if (alreadyopened)
            connection.Close();

            return EmployeeList;
        }

        public void DeleteScence(string ScenceName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Scences"))
            {
            SqlCommand cmd = new SqlCommand("DELETE FROM Scences WHERE Name = '" + ScenceName + "'",connection);

            cmd.ExecuteNonQuery();

            }

            if(alreadyopened)
            connection.Close();
        }

        public void DeleteTable(string TableName)
        {

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Tables"))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Tables WHERE Name = '" + TableName+ "'",connection);

                cmd.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void DeleteEmployee(string AuthCode)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Employees"))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE AuthCode = '" + AuthCode + "'", connection);

                cmd.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void DeleteRole(string RoleName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Roles"))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Roles WHERE Name = '" + RoleName + "'", connection);

                cmd.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void DeleteProduct(string ProductName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Products"))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE Name = '" + ProductName + "'", connection);

                cmd.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void DeleteServing(string ServingName, string ProductName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Servings"))
            {
                SqlCommand cmd = new SqlCommand("SELECT Servings.ID FROM Servings INNER JOIN Products ON Servings.ProductID = Products.ID WHERE Products.Name = '" + ProductName + "' AND Servings.Serving = '" + ServingName + "'", connection);

                int index = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("DELETE FROM Servings WHERE ID = " + index, connection);

                cmd.ExecuteNonQuery();
            }

            if (alreadyopened)
                connection.Close();
        }

        public void DeleteCategory(string CategoryName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            if (CheckDBTable("Categories"))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE Name = '" + CategoryName + "'", connection);

                cmd.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public void UpdateTable(string TableName, String scence, string TableType, string TableColor, int MaxChair,string Checks, int Statue, string Description, string CTableName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("SELECT ID FROM Scences WHERE Name = '" + scence + "'",connection);
            int ScenceID = (int)cmd.ExecuteScalar();

 
            cmd = new SqlCommand("UPDATE Tables SET Name=@TableName,TableScence=@TableScence,TableType=@TableType,TableColor=@TableColor,MaxChair=@MaxChair,StatueID=@Statue,Checks=@Checks WHERE Name = @CName", connection);
                cmd.Parameters.Add("@TableName", System.Data.SqlDbType.VarChar).Value = TableName;
                cmd.Parameters.Add("@TableScence", System.Data.SqlDbType.Int).Value = ScenceID;
                cmd.Parameters.Add("@TableType", System.Data.SqlDbType.VarChar).Value = TableType;
                cmd.Parameters.Add("@TableColor", System.Data.SqlDbType.VarChar).Value = TableColor;
                cmd.Parameters.Add("@MaxChair", System.Data.SqlDbType.TinyInt).Value = MaxChair;
                cmd.Parameters.Add("@Statue", System.Data.SqlDbType.TinyInt).Value = Statue;
                cmd.Parameters.Add("@Checks", System.Data.SqlDbType.VarChar).Value = Checks;
                cmd.Parameters.Add("@Description", System.Data.SqlDbType.Text).Value = Description;
                cmd.Parameters.Add("@CName", System.Data.SqlDbType.VarChar).Value = CTableName;

                cmd.ExecuteNonQuery();

            if(alreadyopened)
            connection.Close();
        }

        public void UpdateEmployee(string Name, string Surname, int Gender, DateTime StartDate, string Mail, string Phone, string Adress, Role role, string AuthCode, string CAuthCode)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand command = new SqlCommand("UPDATE Employees SET Name= @Name, Surname = @Surname, Gender = @Gender, StartDate = @StartDate, Mail = @Mail, Phone = @Phone, Adress = @Adress,RoleID = " + role.ID+", AuthCode = @AuthCode WHERE AuthCode = '" + CAuthCode + "'", connection);
            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = Name;
            command.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar).Value = Surname;
            command.Parameters.Add("@Gender", System.Data.SqlDbType.Int).Value = Gender;
            command.Parameters.Add("@StartDate", System.Data.SqlDbType.DateTime).Value = StartDate;
            command.Parameters.Add("@Mail", System.Data.SqlDbType.NVarChar).Value = Mail;
            command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar).Value = Phone;
            command.Parameters.Add("@Adress", System.Data.SqlDbType.Text).Value = Adress;
            command.Parameters.Add("@roleID", System.Data.SqlDbType.Int).Value = role.ID;
            command.Parameters.Add("@AuthCode", System.Data.SqlDbType.Char).Value = AuthCode;

            command.ExecuteNonQuery();

            if (alreadyopened)
            connection.Close();
        }

        public void UpdateProduct(string ProductName, string CategoryName, double Tax, string Barcode, string CProdutName)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            int CategoryID = (int)new SqlCommand("SELECT ID FROM Categories WHERE Name = '" + CategoryName +"'",connection).ExecuteScalar();

            if(CheckDBTable("Products"))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Products SET Name='@ProductName',CategoryID=@CategoryID,TaxRatio=@TaxRatio,Barcode='@Barcode WHERE Name = '" + CProdutName + "'", connection);
                cmd.Parameters.Add("@ProductName", System.Data.SqlDbType.VarChar).Value = ProductName;
                cmd.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = CategoryID;
                cmd.Parameters.Add("@TaxRatio", System.Data.SqlDbType.Float).Value = Tax;
                cmd.Parameters.Add("@Barcode", System.Data.SqlDbType.VarChar).Value = Barcode;
                cmd.ExecuteNonQuery();
            }

            if(alreadyopened)
            connection.Close();
        }

        public List<Model.Message> GetMessages()
        {
            List<Employee> Employees = GetEmployeeList(GetRoleList());

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<Model.Message> Messages = new List<Model.Message>();

            System.Data.DataTable dt = new System.Data.DataTable();

            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM History", connection);
            if (((int)command.ExecuteScalar()) == 0)
                return Messages;

            command = new SqlCommand("SELECT * FROM History", connection);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int? TID = null; 
                    int? PID = null; 

                    string MessageT = reader["Message"].ToString();
                    int Type = (int)reader["Type"];
                    DateTime Date = reader.GetDateTime(3);
                    Employee employee = null;
                    if (reader["EmployeeID"].ToString() != "-1")
                        employee = Employees.Find(p=> p.ID == Convert.ToInt32(reader["EmployeeID"]));
                    int Screen = (int)reader["Screen"];

                    if (reader["ProductID"] == null || reader["ProductID"] == DBNull.Value)
                        PID = null;
                    else
                        PID = (int?)reader["ProductID"];

                    if (reader["TableID"] == null || reader["TableID"] == DBNull.Value)
                        TID = null;
                    else
                        TID = (int?)reader["TableID"];

                    if (employee == null)
                        Messages.Add(new Model.Message(MessageT,Date,(ScreenEnum)Screen,(MessageType)Type,PID,TID));
                   else
                        Messages.Add(new Model.Message(MessageT,Date,employee,(ScreenEnum)Screen,(MessageType)Type,PID,TID));
                }
            }

            reader.Close();

            if(alreadyopened)
                connection.Close();

            return Messages;
        }

        public void SendHistoryMessage(string Message, int MessageType, DateTime MessageDate, string AuthCode, int screen, int? ProductID, int? TableID)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

                int EmployeeID;

             if (!string.IsNullOrWhiteSpace(AuthCode) && AuthCode != "0" && AuthCode != "System")
                 EmployeeID = (int)new SqlCommand("SELECT ID FROM Employees WHERE AuthCode = '" + AuthCode + "'", connection).ExecuteScalar();
             else
                 EmployeeID = -1;

            SqlCommand command = new SqlCommand("INSERT INTO History VALUES(@Message,@Type,@Date,@EmployeeID,@Screen,@ProductID,@TableID)", connection);
            command.Parameters.Add("@Message", System.Data.SqlDbType.Text).Value = Message;
            command.Parameters.Add("@Type", System.Data.SqlDbType.Int).Value = MessageType;
            command.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = MessageDate;
            command.Parameters.Add("@EmployeeID", System.Data.SqlDbType.Int).Value = EmployeeID;
            command.Parameters.Add("@Screen", System.Data.SqlDbType.Int).Value = (int)screen;

            if(ProductID != null)
            command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int).Value = (int)ProductID;
            else
            command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int).Value = DBNull.Value;

            if(TableID != null)
            command.Parameters.Add("@TableID", System.Data.SqlDbType.Int).Value = (int)screen;
            else
            command.Parameters.Add("@TableID", System.Data.SqlDbType.Int).Value = DBNull.Value;

            command.ExecuteNonQuery();

            if(alreadyopened)
            connection.Close();


        }

        public void AddTicket(Table table, Employee employee, DateTime CreatedDate)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO Tickets VALUES(@TableID,@EmployeeID,@CreatedDate)",connection);
            cmd.Parameters.Add("@TableID",SqlDbType.Int).Value = table.ID;
            cmd.Parameters.Add("@EmployeeID",SqlDbType.Int).Value = employee.ID;
            cmd.Parameters.Add("@CreatedDate",SqlDbType.DateTime).Value = DateTime.Now;

            cmd.ExecuteNonQuery();

            UpdateTable(table.Name,table.TableScence,table.TableType.ToString(),table.TableColor,table.MaxChair,table.ConvertBoolArrayToString(table.Checks),1,table.Description,table.Name);

            if (alreadyopened)
                connection.Close();
        }

        public void DeleteTicket(Table table)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM Tickets WHERE TableID = '" + table.ID + "'",connection);

            if (alreadyopened)
                connection.Close();
        }

        public Ticket GetTicket(Table table)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            Ticket ticket = new Ticket();

SqlCommand cmd = new SqlCommand("SELECT * FROM Tickets WHERE TableID = " + table.ID,connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
                while (dr.Read())
                {
                    ticket.ID = dr.GetInt32(0);
                    ticket.UsingTableID = dr.GetInt32(1);
                    ticket.CreatedDate = dr.GetDateTime(3);
                }
            else
            {
                dr.Close();

                if (alreadyopened)
                    connection.Close();

                return null;
            }

            dr.Close();

            if (alreadyopened)
                connection.Close();

            return ticket;
        }

        public List<Model.Product> GetTicketProducts(int TicketID, List<Model.Product> Products)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            List<Product> TicketProducts = new List<Product>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM TicketProducts WHERE TicketID = " + TicketID, connection);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
                while (dr.Read())
                {
                    int pid = dr.GetInt32(2);
                        Product product = Products.FindLast(p => p.ID == pid);
                    product.Multiplier = Convert.ToInt32(dr["Multiplier"]);
                    TicketProducts.Add(product);
                    product.CurrentServing = product.Servings[(int)dr["ServingID"]];
                }

            dr.Close();

            if (alreadyopened)
                connection.Close();

            return TicketProducts;
        }

        public void DeleteTicketProducts(int TicketID)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM TicketProducts WHERE TicketID = " + TicketID,connection);
            cmd.ExecuteNonQuery();

            if (alreadyopened)
                connection.Close();
        }

        public void AddTicketProduct(int TicketID, Product product, int ServingID, int Multiplier)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO TicketProducts VALUES(@TicketID,@ProductID,@Serving,@Multiplier)",connection);
            cmd.Parameters.Add("@TicketID",SqlDbType.Int).Value = TicketID;
            cmd.Parameters.Add("@ProductID",SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@Serving",SqlDbType.Int).Value = ServingID;
            cmd.Parameters.Add("@Multiplier", SqlDbType.Int).Value = Multiplier;

            cmd.ExecuteNonQuery();

            if (alreadyopened)
                connection.Close();
        }

        public bool DeleteTicketProduct(int TableID, int ProductID)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("SELECT ID FROM Tickets WHERE TableID = " + TableID,connection);
            int TicketID = (int)cmd.ExecuteScalar();

            cmd = new SqlCommand("SELECT COUNT(*) FROM TicketProducts WHERE TicketID = " + TicketID + " AND ProductID = " + ProductID,connection);
            int exists = (int)cmd.ExecuteScalar();

            if (exists == 0)
            {
                connection.Close();
            return false;
            }

            cmd = new SqlCommand("DELETE FROM TicketProducts WHERE TicketID = " + TicketID + " AND ProductID = " + ProductID, connection);
            cmd.ExecuteNonQuery();

            if (alreadyopened)
                connection.Close();

            return true;
        }

        public void UpdateTicket(Table table, Table CTable)
        {
            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("UPDATE Tickets SET TableID = @TableID WHERE TableID = " + table.ID,connection);
            cmd.Parameters.Add("@TableID",SqlDbType.Int).Value = CTable.ID;

            cmd.ExecuteNonQuery();

            UpdateTable(table.Name,table.TableScence,table.TableType,table.TableColor,table.MaxChair,table.ConvertBoolArrayToString(table.Checks),0,table.Description,table.Name);
            UpdateTable(CTable.Name,CTable.TableScence, CTable.TableType, CTable.TableColor, CTable.MaxChair, CTable.ConvertBoolArrayToString(CTable.Checks),1, CTable.Description, CTable.Name);

            if (alreadyopened)
                connection.Close();
        }

        public int GetTotalSales(DateTime date)
        {
            int Total = 0;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM AccountArchives", connection);
            Total = (int)cmd.ExecuteScalar();

            if (alreadyopened)
                connection.Close();

            return Total;
        }

        public int GetTotalActiveTables(DateTime date)
        {
            int Total = 0;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Tickets",connection);
            Total = (int)cmd.ExecuteScalar();

            if (alreadyopened)
                connection.Close();

            return Total;
        }

        public int GetTotalSoldProducts(DateTime date)
        {
            int Total = 0;

            bool alreadyopened = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                alreadyopened = true;
            }

            SqlCommand cmd = new SqlCommand("SELECT SUM(AccountProducts.Quantity) FROM AccountArchives INNER JOIN AccountProducts ON AccountArchives.ID = AccountID", connection);
            try
            {
            Total = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                Total = 0;
            }

            if (alreadyopened)
                connection.Close();

            return Total;
        }

        public Category GetCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public void SendHistoryMessage(string Message, int MessageType, DateTime MessageDate, int EmployeeID, string AuthCode, int screen, int? ProductID, int? TableID)
        {
            throw new NotImplementedException();
        }

        public Scence GetScence(int SceneID)
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(int ProductID)
        {
            throw new NotImplementedException();
        }

        public Category GetCategory(int CategoryID)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductList()
        {
            throw new NotImplementedException();
        }

        void IDatabase.DeleteTicketProduct(int TableID, int ProductID)
        {
            throw new NotImplementedException();
        }
    }

    public static class SqlHelper
    {
        public static IEnumerable<string> ListLocalSqlInstances()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }

                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                foreach (string item in ListLocalSqlInstances(Registry.LocalMachine))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<string> ListLocalSqlInstances(RegistryKey hive)
        {
            const string keyName = @"Software\Microsoft\Microsoft SQL Server";
            const string valueName = "InstalledInstances";
            const string defaultName = "MSSQLSERVER";

            using (var key = hive.OpenSubKey(keyName, false))
            {
                if (key == null) return Enumerable.Empty<string>();

                var value = key.GetValue(valueName) as string[];
                if (value == null) return Enumerable.Empty<string>();

                for (int index = 0; index < value.Length; index++)
                {
                    if (string.Equals(value[index], defaultName, StringComparison.OrdinalIgnoreCase))
                    {
                        value[index] = ".";
                    }
                    else
                    {
                        value[index] = @".\" + value[index];
                    }
                }

                return value;
            }
        }
    }
}
