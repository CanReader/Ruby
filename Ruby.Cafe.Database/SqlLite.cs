using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Windows;

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

        private SQLiteConnection connection { get; set; }

        public SqlLite()
        {
            ConnectionString = "Data Source=" +  Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.db;Version=3";

            connection = new SQLiteConnection(ConnectionString);

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
               SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.db");
               InitializeDBTables();
            }

            if(connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public void InitializeDBTables()
        {
            //Scences-Categories-Roles-Tables-Employees-Products-Servings-Tickets-TicketProducts-AccountArchives-AccountProducts-History
            if (!CheckDBTable("Scenes"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Scenes(_id INTEGER PRIMARY KEY,Name VARCHAR(15) NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Categories"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Categories(_id INTEGER PRIMARY KEY,Name VARCHAR(14) NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Roles"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Roles(_id INTEGER PRIMARY KEY,Name VARCHAR(18) NOT NULL,Perms CHAR(27) NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Tables"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Tables(_id INTEGER NOT NULL  PRIMARY KEY,Name VARCHAR(24) NOT NULL,TableScence INT NOT NULL,TableType VARCHAR(17) NOT NULL,TableColor VARCHAR(6) NOT NULL,MaxChair TINYINT NOT NULL,Checks VARCHAR(5) NOT NULL,StatueID TINYINT,Description Text)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Employees"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Employees(_id INTEGER NOT NULL  PRIMARY KEY,Name VARCHAR(18) NOT NULL,Surname VARCHAR(16) NOT NULL,Gender INT NOT NULL,StartDate DATE,Mail NVARCHAR(27) NOT NULL,Phone VARCHAR(11) NOT NULL,Adress TEXT,RoleID INT NOT NULL,AuthCode CHAR(4) NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Products"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Products(_id INTEGER NOT NULL  PRIMARY KEY,Name VARCHAR(17) NOT NULL,CategoryID INT NOT NULL,TaxRatio FLOAT NOT NULL,Barcode VARCHAR(13))", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Servings"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Servings(_id INTEGER NOT NULL  PRIMARY KEY,Serving VARCHAR(12) NOT NULL,Quantity INT NOT NULL,Price DECIMAL NOT NULL,ProductID INT NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("Tickets"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE Tickets(_id INTEGER NOT NULL PRIMARY KEY,TableID INT NOT NULL,EmployeeID INT NOT NULL,CreatedDate Date NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("TicketProducts"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE TicketProducts(_id INTEGER NOT NULL  PRIMARY KEY, TicketID INT NOT NULL, ProductID INT NOT NULL, ServingID INT NOT NULL, Multiplier INT NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("AccountArchives"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE AccountArchives(_id INTEGER NOT NULL  PRIMARY KEY,TableID INT NOT NULL,EmployeeID INT NOT NULL,TotalPrice DECIMAL NOT NULL,TotalTax DECIMAL NOT NULL,PaymentType INT NOT NULL,CreatedDate DATE NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("AccountProducts"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE AccountProducts(_id INTEGER NOT NULL ,ProductID INT NOT NULL ,Quantity INT NOT NULL,AccountID INT)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (!CheckDBTable("History"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("CREATE TABLE History(_id INTEGER  NOT NULL PRIMARY KEY,Message TEXT NOT NULL,Type INT NOT NULL,Date DateTime NOT NULL,EmployeeID INT NOT NULL,Screen INT NOT NULL)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool CheckDBTable(string TableName)
        {
            var cmd = new SQLiteCommand($"SELECT COUNT(*) FROM sqlite_master WHERE name = '{TableName}'", connection);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public void AddCategory(string CategoryName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand($"INSERT INTO Categories(Name) VALUES(CategoryName)", connection);
                cmd.Parameters.AddWithValue("CategoryName",CategoryName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddEmployee(string Name, string Surname, int Gender, DateTime StartDate, string Mail, string Phone, string Adress, string roleName, string AuthCode)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Employees(Name,Surname,Gender,StartDate,Mail,Phone,Adress,RoleID,AuthCode) VALUES(@Name,@Surname,@Gender,@StartDate,@Mail,@Phone,@Adress,@RoleID,@AuthCode)", connection);
                cmd.Parameters.AddWithValue("Name",Name);
                cmd.Parameters.AddWithValue("Surname",Surname);
                cmd.Parameters.AddWithValue("Gender",Gender);
                cmd.Parameters.AddWithValue("StartDate", StartDate);
                cmd.Parameters.AddWithValue("Mail", Mail);
                cmd.Parameters.AddWithValue("Phone", Phone);
                cmd.Parameters.AddWithValue("Adress",Adress);
                cmd.Parameters.AddWithValue("RoleID",GetRole(roleName).ID);
                cmd.Parameters.AddWithValue("AuthCode",Name);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddProduct(string ProductName, string CategoryName, double Tax, string Barcode)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Products(Name, CategoryID, TaxRatio, Barcode) VALUES(@Name,@CategoryID,@TaxRatio,@Barcode)", connection);
                cmd.Parameters.AddWithValue("Name", ProductName);
                cmd.Parameters.AddWithValue("CategoryID", GetCategory(CategoryName).ID);
                cmd.Parameters.AddWithValue("TaxRatio", Tax);
                cmd.Parameters.AddWithValue("Barcode", Barcode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddRole(string RoleName, bool[] Perms)
        {
            connection.Open();

            try
            {
                if (Perms[0])
                    for (int k = 0; k < Perms.Length; k++)
                        Perms[k] = true;

                int TotalVLines = Perms.Length - 1;

                int i = 0;

                var tbuilder = new StringBuilder(Perms.Length + TotalVLines);

                do
                {
                    tbuilder.AppendFormat("{0}|",Convert.ToInt32(Perms[i].ToString()));
                    i++;
                } while (i < Perms.Length);

                tbuilder.Remove(tbuilder.Length - 1,1);

                var cmd = new SQLiteCommand("INSERT INTO Sce(Name, Perms) VALUES(@Name,@Perms)", connection);
                cmd.Parameters.AddWithValue("@Name", RoleName);
                cmd.Parameters.AddWithValue("@Perms", tbuilder.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddScences(string ScenceName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Scenes(Name) VALUES(@Name)", connection);
                cmd.Parameters.AddWithValue("@Name", ScenceName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddServing(string Serving, int Quantity, decimal Price, string ProductName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Servings(Serving, Quantity, Price, ProductID) VALUES(@Serving, @Quantity, @Price, @ProductID)", connection);
                cmd.Parameters.AddWithValue("@Serving", Serving);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@ProductID", GetProduct(ProductName).ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddTable(string TableName, Scence scence, string TableType, string TableColor, int MaxChair, string Checks, int Statue, string Description)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Tables(Name,TableScence,TableType,TableColor,MaxChair,Checks,StatueID,Description) VALUES(@Name,@TableScence,@TableType,@TableColor,@MaxChair,@Checks,@StatueID,@Description)", connection);
                cmd.Parameters.AddWithValue("@Name", TableName);
                cmd.Parameters.AddWithValue("@TableScene", scence.ID);
                cmd.Parameters.AddWithValue("@TableType", TableType);
                cmd.Parameters.AddWithValue("@TableColor", TableColor);
                cmd.Parameters.AddWithValue("@MaxChair", MaxChair);
                cmd.Parameters.AddWithValue("@Checks", Checks);
                cmd.Parameters.AddWithValue("@StatueID", Statue);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddTicket(Table table, Employee employee, DateTime CreatedDate)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Tickets(TableID,EmployeeID,CreatedDate) VALUES(@TableID,@EmployeeID,@CreatedDate)", connection);
                cmd.Parameters.AddWithValue("@TableID", table.ID);
                cmd.Parameters.AddWithValue("@EmployeeID", employee.ID);
                cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void AddTicketProduct(int TicketID, Product product, int ServingID, int Multiplier)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO Tickets(TicketID,ProductID,ServingID,Multiplier) VALUES(@TicketID,@ProductID,@ServingID,@Multiplier)", connection);
                cmd.Parameters.AddWithValue("@TicketID", TicketID);
                cmd.Parameters.AddWithValue("@ProductID", product.ID);
                cmd.Parameters.AddWithValue("@ServingID", ServingID);
                cmd.Parameters.AddWithValue("@Multiplier", Multiplier);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteCategory(string CategoryName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Categories WHERE Name = @name",connection);
                cmd.Parameters.AddWithValue("@name",CategoryName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteEmployee(string AuthCode)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Employees WHERE Auth = @Code", connection);
                cmd.Parameters.AddWithValue("@Code", AuthCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteProduct(string ProductName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Products WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("@name", ProductName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteRole(string RoleName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Roles WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("@name", RoleName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteScence(string ScenceName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Scenes WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("@name", ScenceName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteServing(string ServingName, string ProductName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Servings WHERE Serving = @name AND ProductID = product", connection);
                cmd.Parameters.AddWithValue("@name", ServingName);
                cmd.Parameters.AddWithValue("@product", GetProduct(ProductName).ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteTable(string TableName)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Tables WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("@name", TableName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteTicket(Table table)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM Tables WHERE TableID = @Table", connection);
                cmd.Parameters.AddWithValue("@Table", table.ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteTicketProduct(int TableID, int ProductID)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM TicketProducts WHERE TicketID = @Ticket AND ProductID = @Product", connection);
                cmd.Parameters.AddWithValue("@Ticket", GetTicket(GetTable(TableID)).ID);
                cmd.Parameters.AddWithValue("@Product", ProductID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public void DeleteTicketProducts(int TicketID)
        {

            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("DELETE FROM TicketProducts WHERE TicketID = @Ticket", connection);
                cmd.Parameters.AddWithValue("@Ticket", TicketID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        //Get items

        public Scence GetScence(int SceneID)
        {
            Scence scene = new Scence();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Scenes WHERE _id = @ID", connection);
                cmd.Parameters.AddWithValue("ID", scene);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        scene = new Scence()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return scene;
        }
        
        public Scence GetScence(string ScenceName)
        {
            Scence scene = new Scence();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Scenes WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("name", ScenceName);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No role found!");
                }
                else
                {
                    while (reader.Read())
                        scene = new Scence()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return scene;
        }
        
        public Table GetTable(int TableID)
        {
            Table table = new Table();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Tables WHERE _id = @ID", connection);
                cmd.Parameters.AddWithValue("ID", TableID);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        table = new Table()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TableScence = GetScence(reader.GetInt32(2)).Name,
                            TableType = reader.GetString(3),
                            TableColor = reader.GetString(4),
                            MaxChair = reader.GetInt32(5),
                            Checks = table.ConvertStringToBoolArray(reader.GetString(6)),
                            CurrentStatue = (TableStatue)reader.GetInt32(7),
                            Description = reader.GetString(8),
                        };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return table;
        }
        
        public Table GetTable(string TableName)
        {
            Table table = new Table();

            SQLiteDataReader reader = null;

            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Tables WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("name", TableName);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No table found!");
                }
                else
                {
                    while (reader.Read())
                        table = new Table()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TableScence = GetScence(reader.GetInt32(2)).Name,
                            TableType = reader.GetString(3),
                            TableColor = reader.GetString(4),
                            MaxChair = reader.GetInt32(5),
                            Checks = table.ConvertStringToBoolArray(reader.GetString(6)),
                            CurrentStatue = (TableStatue)reader.GetInt32(7),
                            Description = reader.GetString(8),
                        };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return table;
        }
        
        public Role GetRole(int RoleID)
        {
            Role role = new Role();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Roles WHERE _id = @ID", connection);
                cmd.Parameters.AddWithValue("ID", RoleID);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No role found!");
                }
                else
                {
                    while (reader.Read())
                        role = new Role()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Perms = role.ConvertStringToPerms(reader.GetString(2)),
                        };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            reader.Close();

            connection.Close();

            return role;
        }
        
        public Role GetRole(string RoleName)
        {
            Role role = new Role();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Roles WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("name", RoleName);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No role found!");
                }
                else
                {
                    while (reader.Read())
                        role = new Role()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Perms = role.ConvertStringToPerms(reader.GetString(2)),
                        };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return role;
        }
        
        public Employee GetEmployee(int EmployeeID)
        {
            connection.Open();

            Employee e = null;

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Employees WHERE _id = @ID", connection);
                cmd.Parameters.AddWithValue("ID", EmployeeID);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No employee found!");
                }
                else
                {
                    while (reader.Read())
                        e = new Employee()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Gender = reader.GetInt32(3),
                            StartDate = reader.GetDateTime(4),
                            Mail = reader.GetString(5),
                            Phone = reader.GetString(6),
                            Adress = reader.GetString(7),
                            role = GetRole(reader.GetInt32(8)),
                            AuthCode = reader.GetString(9),
                        };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                e = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return e;
        }
    
        public Employee GetEmployee(string AuthCode)
        {
            connection.Open();

            Employee e = null;

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Employees WHERE AuthCode = @Auth", connection);
                cmd.Parameters.AddWithValue("Auth",AuthCode);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        e = new Employee() {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Gender = reader.GetInt32(3),
                            StartDate = reader.GetDateTime(4),
                            Mail = reader.GetString(5),
                            Phone = reader.GetString(6),
                            Adress = reader.GetString(7),
                            role = GetRole(reader.GetInt32(8)),
                            AuthCode = reader.GetString(9),
                            };
            }
            catch (Exception ex)
            {
                e = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return e;
        }

        public Category GetCategory(int CategoryID)
        {
            Category cat = new Category() { ID = -1 };

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Categories WHERE Name = @ID", connection);
                cmd.Parameters.AddWithValue("ID", CategoryID);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        cat.ID = reader.GetInt32(0);
                        cat.Name = reader.GetString(1);
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return cat;
        }
    
        public Category GetCategory(string CategoryName)
        {
            Category cat = new Category() { ID = -1 };

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Categories WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("name", CategoryName);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        cat.ID = reader.GetInt32(0);
                        cat.Name = reader.GetString(1);
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return cat;
        }

        public Product GetProduct(int ProductID)
        {
            Product p = null;
            
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Products WHERE _id = @ID", connection);
                cmd.Parameters.AddWithValue("ID", ProductID);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No product found!");
                }
                else
                {
                    while (reader.Read())
                        p = new Product()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            category = GetCategory(reader.GetInt32(2)),
                            Tax = reader.GetDouble(3),
                            Barcode = reader.GetString(4),
                        };
                }
            }
            catch (Exception ex)
            {
                p = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return p;
        }

        public Product GetProduct(string ProductName)
        {
            connection.Open();

            Product p = null;

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Products WHERE Name = @name", connection);
                cmd.Parameters.AddWithValue("name", ProductName);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No product found!");
                }
                else
                {
                    while (reader.Read())
                        p = new Product()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            category = GetCategory(reader.GetInt32(2)),
                            Tax = reader.GetDouble(3),
                            Barcode = reader.GetString(4),
                        };
                }
            }
            catch (Exception ex)
            {
                p = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return p;
        }

        public Ticket GetTicket(Table table)
        {
            Ticket ticket = new Ticket();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Tickets WHERE TableID = @TableID", connection);
                cmd.Parameters.AddWithValue("TableID", table.ID);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("No Ticket found!");
                }
                else
                {
                    while (reader.Read())
                        ticket = new Ticket()
                        {
                            ID = reader.GetInt32(0),
                            UsingTableID = reader.GetInt32(1),
                            CreatedDate = reader.GetDateTime(3)
                        };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return ticket;
        }

        //Get lists

        public List<Role> GetRoleList()
        {
            var Roles = new List<Role>();

            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Categories", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Roles.Add(new Role()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Perms = new Role().ConvertStringToPerms(reader.GetString(2))
                        });
            }
            catch (Exception e)
            {
                Roles = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Roles;
        }

        public List<Product> GetProductList()
        {
            var Products = new List<Product>();
            
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Categories", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Products.Add(new Product()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            category = GetCategory(reader.GetInt32(2)),
                            Tax = reader.GetDouble(3),
                            Barcode = reader.GetString(4),
                        });
            }
            catch (Exception e)
            {
                Products = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Products;
        }

        public List<Table> GetTableList(List<Scence> Scences)
        {
            var Tables = new List<Table>();
            
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Tables", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Tables.Add(new Table()
                        {

                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TableScence = GetScence(reader.GetInt32(2)).Name,
                            TableType = reader.GetString(3),
                            TableColor = reader.GetString(4),
                            MaxChair = reader.GetInt32(5),
                            Checks = new Table().ConvertStringToBoolArray(reader.GetString(6)),
                            CurrentStatue = (TableStatue)reader.GetInt32(7),
                            Description = reader.GetString(8),
                        });
            }
            catch (Exception e)
            {
                Tables = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Tables;
        }

        public List<Scence> GetScenceList()
        {
            var scenes = new List<Scence>();
            
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Scenes", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        scenes.Add(new Scence()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
            }
            catch (Exception e)
            {
                scenes = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return scenes;
        }

        public List<ServingItem> GetServings(int ProductID)
        {
            var Servings = new List<ServingItem>();
            
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Servings", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Servings.Add(new ServingItem()
                        {
                            Serving = reader.GetString(1),
                            Price = reader.GetDecimal(3),
                            Quantity = reader.GetInt32(2)
                        });
            }
            catch (Exception e)
            {
                Servings = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Servings;
        }

        public List<Employee> GetEmployeeList(List<Role> Roles)
        {
            var Employees = new List<Employee>();
        
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Categories", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Employees.Add(new Employee()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Gender = reader.GetInt32(3),
                            StartDate = reader.GetDateTime(4),
                            Mail = reader.GetString(5),
                            Phone = reader.GetString(6),
                            Adress = reader.GetString(7),
                            role = GetRole(reader.GetInt32(8)),
                            AuthCode = reader.GetString(9),
                        });
            }
            catch (Exception e)
            {
                Employees = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Employees;
        }

        public List<Message> GetMessages()
        {
            var Messages = new List<Message>();
            
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM History", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Messages.Add(new Message(reader.GetString(1), reader.GetDateTime(3), GetEmployee(reader.GetInt32(4)), (ScreenEnum)reader.GetInt32(5), (MessageType)reader.GetInt32(2), null, null));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                Messages = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Messages;
        }

        public List<Category> GetCategoryList()
        {
            List<Category> Categories = new List<Category>();
         
            connection.Open();

            SQLiteDataReader reader = null;

            try
            {
                var cmd = new SQLiteCommand("SELECT * FROM Categories", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        Categories.Add(new Category() { ID = (int)reader[0], Name = (string)reader[1] });
            }
            catch (Exception e)
            {
                Categories = null;
            }

            if (reader != null)
                reader.Close();

            connection.Close();

            return Categories;
        }

        public List<Product> GetTicketProducts(int TicketID, List<Product> Products)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetTickets()
        {
            connection.Open();

            List<Ticket> Tickets = new List<Ticket>();

            var cmd = new SQLiteCommand("SELECT * FROM Tickets",connection);
            var reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                    while (reader.Read())
                        Tickets.Add(new Ticket()
                        {
                            ID = reader.GetInt32(0),
                            UsingTableID = reader.GetInt32(1),
                            AccessedEmployee = GetEmployee(reader.GetInt32(2)),
                            CreatedDate = reader.GetDateTime(3)
                        });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            reader.Close();

            connection.Close();

            return Tickets;
        }

        //Update

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

        public void SendHistoryMessage(string Message, int MessageType, DateTime MessageDate, int EmployeeID, string AuthCode, int screen, int? ProductID, int? TableID)
        {
            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("INSERT INTO History VALUES(@Message,@Type,@Date,@Employee,@Screen)", connection);
                cmd.Parameters.AddWithValue("Message", Message);
                cmd.Parameters.AddWithValue("Type", MessageType);
                cmd.Parameters.AddWithValue("Date", MessageDate);
                cmd.Parameters.AddWithValue("Employee", EmployeeID);
                cmd.Parameters.AddWithValue("Screen", screen);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();
        }

        public int GetTotalActiveTables(DateTime date)
        {
            int Actives = 0;

            connection.Open();

            try
            {
                var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT TableID) FROM Tickets", connection);
                Actives = (int)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connection.Close();

            return Actives;
        }

        public int GetTotalSales(DateTime date)
        {
            throw new NotImplementedException();
        }

        public int GetTotalSoldProducts(DateTime date)
        {
            throw new NotImplementedException();
        }


    }
}
