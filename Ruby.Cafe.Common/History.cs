using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;

namespace Ruby.Cafe.Common
{
    public class History
    {
        public List<Model.Message> Messages;

        public Employee AcessedEmployee;

        Database.IDatabase db;

        public History()
        {
            if (Messages == null)
                Messages = new List<Model.Message>();
        }

        public List<Model.Message> GetMessages() => new Ruby.Serialization.DatabaseSetting(true).InitializeDB(Ruby.Serialization.DatabaseSetting.type).GetMessages();

        public String SendMessage(Employee e, ScreenEnum Environment, MessageType Type, string Message)
        {
            Message Messenger = new Model.Message(Message,DateTime.Now,e ,Environment, Type, null, null);
            Messages.Add(Messenger);

            if(db==null)db = new Ruby.Serialization.DatabaseSetting(true).InitializeDB(Ruby.Serialization.DatabaseSetting.type);

            db.SendHistoryMessage(Message,(int)Type,DateTime.Now,e.AuthCode, (int)Environment,null,null);

            return Messenger.ToString();
        }

        public String SendMessage(ScreenEnum Environment, MessageType Type, string Message)
        {
            Message Messenger = new Model.Message(Message, DateTime.Now, Environment, Type,null,null);
            Messages.Add(Messenger);

            Database.IDatabase db = new Ruby.Serialization.DatabaseSetting(true).InitializeDB(Ruby.Serialization.DatabaseSetting.type);
            db.SendHistoryMessage(Message, (int)Type, DateTime.Now, "System" , (int)Environment,null,null);

            return Messenger.ToString();
        }

        public String SendMessage(Employee e, ScreenEnum Environment, MessageType Type, string Message,int? ProductID,int? TableID)
        {
            Message Messenger = new Model.Message(Message, DateTime.Now, e, Environment, Type, ProductID, TableID);
            Messages.Add(Messenger);

            if (db == null) db = new Ruby.Serialization.DatabaseSetting(true).InitializeDB(Ruby.Serialization.DatabaseSetting.type);

            db.SendHistoryMessage(Message, (int)Type, DateTime.Now, e.AuthCode, (int)Environment, null, null);

            return Messenger.ToString();
        }

        public String SendMessage(ScreenEnum Environment, MessageType Type, string Message, int? ProductID, int? TableID)
        {
            Message Messenger = new Model.Message(Message, DateTime.Now, Environment, Type, ProductID, TableID);
            Messages.Add(Messenger);

            Database.IDatabase db = new Ruby.Serialization.DatabaseSetting(true).InitializeDB(Ruby.Serialization.DatabaseSetting.type);
            db.SendHistoryMessage(Message, (int)Type, DateTime.Now, "System", (int)Environment, null, null);

            return Messenger.ToString();
        }
    }
}
