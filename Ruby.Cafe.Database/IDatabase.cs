using System;
using System.Collections.Generic;

namespace Ruby.Cafe.Database
{
    public interface IDatabase
    {
        string ConnectionString { get; }

        string Server { get;}
        string DatabaseName { get;}
        string UserID { get;}
        string Password { get;}
        bool Connected { get;}

        void InitializeDBTables();

        bool CheckDBTable(string TableName);

        void AddScences(string ScenceName);

        void AddTable(string TableName, Model.Scence scence, string TableType, string TableColor, int MaxChair,string Checks, int Statue, string Description);

        void AddEmployee(string Name, string Surname, int Gender, DateTime StartDate, string Mail, string Phone, string Adress, String roleName, string AuthCode);

        void AddCategory(string CategoryName);

        void AddProduct(string ProductName, string CategoryName, double Tax, string Barcode);

        void AddServing(string Serving,int Quantity,decimal Price, string ProductName);

        void AddTicket(Model.Table table, Model.Employee employee, System.DateTime CreatedDate);

        void AddRole(string RoleName, bool[] Perms);

        void SendHistoryMessage(string Message, int MessageType, System.DateTime MessageDate,int EmployeeID, string AuthCode, int screen, int? ProductID, int? TableID);

        Model.Scence GetScence(int SceneID);
        
        Model.Scence GetScence(string ScenceName);

        Model.Table GetTable(int TableID);

        Model.Table GetTable(string TableName);

        Model.Role GetRole(int RoleID);
        
        Model.Role GetRole(string RoleName);
        
        Model.Employee GetEmployee(int EmployeeID);

        Model.Employee GetEmployee(string AuthCode);

        Model.Product GetProduct(int ProductID);

        Model.Product GetProduct(string ProductName);

        Model.Ticket GetTicket(Model.Table table);

        Model.Category GetCategory(int CategoryID);

        Model.Category GetCategory(string CategoryName);

        void DeleteScence(string ScenceName);

        void DeleteTable(string TableName);

        void DeleteEmployee(string AuthCode);

        void DeleteRole(string RoleName);

        void DeleteProduct(string ProductName);

        void DeleteServing(string ServingName, string ProductName);

        void DeleteCategory(string CategoryName);

        void DeleteTicket(Model.Table table);

        List<Model.Scence> GetScenceList();

        List<Model.Table> GetTableList(List<Model.Scence> Scences);

        List<Model.Category> GetCategoryList();

        List<Model.Role> GetRoleList();

        List<Model.Product> GetProductList();

        List<Model.Employee> GetEmployeeList(List<Model.Role> Roles);

        List<Model.ServingItem> GetServings(int ProductID);

        List<Model.Message> GetMessages();

        List<Model.Ticket> GetTickets();

        List<Model.Product> GetTicketProducts(int TicketID, List<Model.Product> Products);

        void DeleteTicketProducts(int TicketID);

        void AddTicketProduct(int TicketID, Model.Product product, int ServingID, int Multiplier);

        void DeleteTicketProduct(int TableID, int ProductID);

        void UpdateTable(string TableName, String scence, string TableType, string TableColor, int MaxChair,string Checks, int Statue, string Description, string CTableName);

        void UpdateEmployee(string Name, string Surname, int Gender, System.DateTime StartDate, string Mail, string Phone, string Adress, Model.Role role, string AuthCode, string CAuthCode);

        void UpdateProduct(string ProductName, string CategoryName, double Tax, string Barcode, string CProductName);

        void UpdateTicket(Model.Table table, Model.Table CTable);

        int GetTotalSales(System.DateTime date);

        int GetTotalActiveTables(System.DateTime date);

        int GetTotalSoldProducts(System.DateTime date);

        #region SQL table codes
        /*
         CREATE TABLE Tables(
                 ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                 Name VARCHAR(9) NOT NULL,
                 TableScence INT NOT NULL FOREIGN KEY,
                 TableType VARCHAR(17) NOT NULL,
                 TableColor VARCHAR(6) NOT NULL,
                 MaxChair TINYINT NOT NULL,
                 StatueID TINYINT,
                 Description Text )

  GO

  CREATE TABLE Scences(
                 ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                 Name VARCHAR(15) NOT NULL)

            GO
                   CREATE TABLE Employees(
                 ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
                 Name VARCHAR(13) NOT NULL,
                 Surname VARCHAR(12) NOT NULL,
                 Gender INT  NOT NULL,
                 StartDate DATE,
                 Mail NVARCHAR(27) NOT NULL,
                 Phone VARCHAR(11) NOT NULL,
                 Adress TEXT,
                 RoleID INT NOT NULL FOREIGN KEY,
                 AuthCode CHAR(4) NOT NULL)

            GO

                   CREATE TABLE Categories(
                 ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                 Name VARCHAR(14) NOT NULL)

            GO

                  CREATE TABLE Products(
                ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                Name VARCHAR(17) NOT NULL,
                CategoryID INT NOT NULL FOREIGN KEY,
                TaxRatio FLOAT NOT NULL,
                Barcode VARCHAR(13))

                GO

                CREATE TABLE Servings(
                ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                Serving VARCHAR(12) NOT NULL,
                Quantity INT NOT NULL,
                Price DECIMAL NOT NULL,
                ProductID INT NOT NULL FOREIGN KEY
                )

                 GO

                CREATE TABLE Roles(
                ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                Name VARCHAR(18) NOT NULL,
                Perms CHAR(17) NOT NULL
                )

                GO

                CREATE TABLE Tickets(
                ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                TableID INT NOT NULL FOREIGN KEY,
                EmployeeID INT NOT NULL FOREIGN KEY,
                TotalPrice DECIMAL,
                CreatedDate Date NOT NULL,
                Statue INT NOT NULL
                )

                GO

                CREATE TABLE TicketProducts(
                ID INT NOT NULL IDENTITY(1,1),
                ProductID INT NOT NULL FOREIGN KEY,
                TicketID INT FOREIGN KEY,
                Quantity INT
                )

                GO

                CREATE TABLE AccountArchives(  
                ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                TableID INT NOT NULL FOREIGN KEY,
                EmployeeID INT NOT NULL FOREIGN KEY,
                TotalPrice DECIMAL NOT NULL, 
                TotalTax DECIMAL NOT NULL,
                PaymentType INT NOT NULL,
                CreatedDate DATE NOT NULL)

                GO

                CREATE TABLE AccountProducts
                (
                 ID INT NOT NULL IDENTITY(1,1),
                 ProductID INT NOT NULL FOREIGN KEY,
                 Quantity INT NOT NULL,
                 AccountID INT FOREIGN KEY
                )

                GO

                CREATE TABLE History(
                ID int NOT NULL IDENTITY(1,1),
                HistoryMessage TEXT NOT NULL,
                MessageType INT NOT NULL,
                MessageDate DATE NOT NULL,
                EmployeeID INT NOT NULL FOREIGN KEY,
                Screen INT NOT NULL,
                )
           */
        #endregion
    }
}
