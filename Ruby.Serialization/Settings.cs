using Newtonsoft.Json;
using Ruby.Cafe.Database;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Ruby.Serialization
{
    public class DatabaseSetting
    {
        public static readonly String Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\RubySoft\Database.dat";

        public StringBuilder constring;

        public String ServerName { get; set; }
        public String DatabaseName { get; set; }
        public String UID { get; set; }
        public String Password { get; set; }
        public bool TrustedConnection;

        public static DatabaseType type => (DatabaseType)(new Settings(true).dbType);

        public IDatabase InitializeDB(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.None: return null;
                case DatabaseType.Sql:
                    if (!string.IsNullOrWhiteSpace(UID) && !string.IsNullOrWhiteSpace(Password))
                        return new Sql(ServerName, DatabaseName, UID, Password);
                    else
                        return new Sql(ServerName, DatabaseName);
                case DatabaseType.MySql: return null;
                case DatabaseType.Sqlite: return new SqlLite();
                case DatabaseType.Oracle: return null;
                default: return null;
            }
        }

        public void SerializeSettings(DatabaseType type)
        {
            constring.Clear();

            switch (type)
            {
                case DatabaseType.None:
                    return;
                case DatabaseType.Sql:
                    if (!string.IsNullOrWhiteSpace(UID) && !string.IsNullOrWhiteSpace(Password))
                        constring = new StringBuilder($"Server={ServerName};Database={DatabaseName};User ID={UID};Password={Password};Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True");
                    else
                        constring = new StringBuilder($"Data Source = {ServerName}; Initial Catalog = {DatabaseName};Integrated Security=true;");
                    return;
                case DatabaseType.MySql:
                    return;
                case DatabaseType.Sqlite:
                    return;
                case DatabaseType.Oracle:
                    return;
                default:
                    break;
            }

            if (File.Exists(Path))
                File.Delete(Path);

            using (StreamWriter sw = new StreamWriter(Path))
                sw.Write(constring.ToString());
        }

        public void SerializeSettings()
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                switch (type)
                {
                    case DatabaseType.None:
                        break;
                    case DatabaseType.Sql:
                        if (string.IsNullOrWhiteSpace(UID) && string.IsNullOrWhiteSpace(Password))
                            sw.WriteLine(SqlConnectionStringWin);
                        else
                            sw.WriteLine(SqlConnectionStringSql);
                        break;
                    case DatabaseType.MySql:
                        break;
                    case DatabaseType.Sqlite:
                        break;
                    case DatabaseType.Oracle:
                        break;
                    default:
                        break;
                }
            }
        }

        public bool TestConnection()
        {
            bool successful = true;
            try
            {
                switch (type)
                {
                    case DatabaseType.None: return true;
                    case DatabaseType.Sql:
                        if (!string.IsNullOrWhiteSpace(UID) && !string.IsNullOrWhiteSpace(Password))
                            return new Sql(ServerName, DatabaseName, UID, Password).Connected;
                        else
                            return new Sql(ServerName, DatabaseName).Connected;
                    case DatabaseType.MySql: return true;
                    case DatabaseType.Sqlite: return true;
                    case DatabaseType.Oracle: return true;
                    default: return true;
                }
            }
            catch (Exception)
            {
                successful = false;
            }

            return successful;
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public DatabaseSetting(bool Deserialize)
        {
            if (Deserialize)
            {
                constring = new StringBuilder(File.ReadAllText(Path));
                string[] text = constring.ToString().Split(';');

                foreach (var item in text)
                {
                    string[] splitedstr = item.Split('=');
                    if (splitedstr[0].Trim().Equals("Server") || splitedstr[0].Contains("Data Source")) ServerName = splitedstr[1].Trim();
                    else if (splitedstr[0].Contains("Database") || splitedstr[0].Contains("Initial Catalog")) DatabaseName = splitedstr[1].Trim();
                    else if (splitedstr[0].Contains("User")) UID = splitedstr[1].Trim();
                    else if (splitedstr[0].Contains("Password")) Password = splitedstr[1].Trim();
                }
            }
        }

        public String SqlConnectionStringSql => $"Server={ServerName};Initial Catalog={DatabaseName};Persist Security Info=False;User ID={UID};Password={Password};MultipleActiveResultSets=False;Encrypt=True;Connection Timeout=30;";

        public String SqlConnectionStringWin => $"Server={ServerName};Database={DatabaseName};Trusted_Connection=true";
    }

    [Serializable()]
    public class Settings : IDisposable
    {
        public string MachineName { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationType { get; set; }
        public int Language { get; set; }
        public bool StartWithMachine { get; set; }
        public bool CloseBtnOption { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int PanelScreen { get; set; }
        public int dbType { get; set; }

        public Settings(bool Deserialize)
        {
            if (Deserialize)
            {
                Settings sets = DeserializeSettings();

                this.MachineName = sets.MachineName;
                this.Language = sets.Language;
                this.OrganizationName = sets.OrganizationName;
                this.OrganizationType = sets.OrganizationType;
                this.StartWithMachine = sets.StartWithMachine;
                this.CloseBtnOption = sets.CloseBtnOption;
                this.Width = sets.Width;
                this.Height = sets.Height;
                this.dbType = sets.dbType;
            }
        }

        public static Settings DebugInitializer()
        {
            Settings sets = new Settings(false);

            sets.MachineName = "Ruby Machine";
            sets.OrganizationName = "Ruby Soft Corperation";
            sets.OrganizationType = 0;
            sets.Language = 0;
            sets.StartWithMachine = false;
            sets.CloseBtnOption = true;
            sets.Width = 1920;
            sets.Height = 1080;
            sets.dbType = 1;

            return sets;
        }
        /*
         * "{\"MachineName\":\"Ruby Machine\",\"BusinessName\":\"Ruby Soft Corperation\",\"Language\":0,\"Title\":\"Title\",\"Width\":1920.0,\"Height\":1080.0,\"PanelScreen\":1,\"dbType\":1}"
         */
        private Settings DeserializeSettings()
        {
            Settings sets = null;

            if (!File.Exists("Settings.ini"))
                return null;

            Newtonsoft.Json.JsonSerializer js = new JsonSerializer();
            using (StreamReader sr = File.OpenText("Settings.ini"))
                sets = (Settings)js.Deserialize(sr, typeof(Settings));

            return sets;
        }

        public void SerializeSettings()
        {
            if (File.Exists("Settings.ini"))
                File.Delete("Settings.ini");

            Newtonsoft.Json.JsonSerializer js = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("Settings.ini"))
            {
                js.Culture = Thread.CurrentThread.CurrentCulture;
                js.Serialize(sw, this);
            }
        }

        public void Dispose()
        {
            this.OrganizationName = null;
            this.dbType = 0;
            this.OrganizationType = 0;
            this.Width = 0;
            this.Height = 0;
            this.Language = 0;
            this.MachineName = null;
            this.PanelScreen = 0;
        }
    }
}
