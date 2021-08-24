using Ruby.Cafe.Model;
using System;
using System.Collections.Generic;


namespace Ruby.Cafe.Model
{
     public struct Message
    {
        public Message(String MessageText, DateTime MessageTime, Employee Sender,ScreenEnum Environment, MessageType Type, int? ProductID, int? TableID)
        {
            this.FullMessage = MessageText;
            this.MessageTime = MessageTime;
            this.Sender = Sender;
            this.Environment = Environment;
            this.Type = Type;
            this.ProductID = ProductID;
            this.TableID = TableID;
        }

        public Message(String MessageText, DateTime MessageTime, ScreenEnum Environment, MessageType Type, int? ProductID, int? TableID)
        {
            this.FullMessage = MessageText;
            this.MessageTime = MessageTime;
            Sender = null;
            this.Environment = Environment;
            this.Type = Type;
            this.ProductID = ProductID;
            this.TableID = TableID;
        }

        public String FullMessage{ set; get; }
        public DateTime MessageTime { get; set; }
        public Employee Sender { get; set; }
        public int? ProductID { get; set; }
        public int? TableID { get; set; }
        public ScreenEnum Environment { get; set; }
        public MessageType Type { get; set; }

        public override string ToString()
        {
            int TypeInt = (int)Type;
            int PageInt = (int)Environment;
            object LocalizedType = Type;
            object LocalizedPage = Environment;

            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("tr")) { LocalizedType = new MessageTypeTR[TypeInt]; LocalizedPage = new ScreenEnumTR[PageInt]; }
            else if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("fr")) { LocalizedType = new MessageTypeFR[TypeInt]; LocalizedPage = new ScreenEnumFR[PageInt]; }
            else if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("ger")) { LocalizedType = new MessageTypeGER[TypeInt]; LocalizedPage = new ScreenEnumGER[PageInt]; }


            if (Sender != null)
                return "[ " + MessageTime.ToShortTimeString() + @"\ " + Sender.ToString() + @" \" + LocalizedType.ToString() + @"\" + LocalizedPage.ToString() + "]" + ": " + FullMessage;
            else
                return "[ " + MessageTime.ToShortTimeString() + @"\ " + LocalizedType.ToString() + @"\ " + LocalizedPage.ToString() + "]" + ": " + FullMessage;
        }
    }

    #region MessageType
    public enum MessageType
{
        ERROR = 0,
        WARNING = 1,
        NOTIFICATION = 2,
        PROCESS = 3
}
    public enum MessageTypeTR
        {
            HATA = 0,
            UYARI = 1,
            BİLDİRİ = 2,
            İŞLEM = 3
        }
    public enum MessageTypeFR
        {
            ERREUR = 0,
            ATTENTION = 1,
            NOTIFICATION = 2,
            TRAITER = 3
        }
    public enum MessageTypeGER
        {
            ERROR = 0,
            WARNUNG = 1,
            BENACHRICHTIGUNG = 2,
            PROZESS = 3
        }
    #endregion

    #region ScreenEnum
    public enum ScreenEnumTR
        {
            ANASAYFA = 0,
            GIRISSAYFASI = 1,
            GECMIS = 2,
            MASAEDITORU = 3,
            CALISANEDITORU = 4,
            URUNEDITORU = 5,
            AYARLAR = 6
        }
    public enum ScreenEnumGER
        {
            HAUPTSEITE = 0,
            LOGINSEITE = 1,
            GESCHICHTE = 2,
            TABELLENEDITOR = 3,
            MITARBEITEREDITOR = 4,
            PRODUKTEDITOR = 5,
            EINSTELLSPAGE = 6
        }
    public enum ScreenEnumFR
        {
            DACCUEILPAGE = 0,
            CONNEXIONPAGE = 1,
            HISTOIRE = 2,
            EDITEURTABLE = 3,
            EMPLOYÉÉDITEUR = 4,
            PRODUITSEDITEUR = 5,
            PARAMÈTRES = 6
        }
    #endregion

}

