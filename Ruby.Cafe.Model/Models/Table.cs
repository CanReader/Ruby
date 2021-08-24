using System;
using System.Runtime.Serialization;

namespace Ruby.Cafe.Model
{
    public struct Scence
    {
        public int ID { get; set; }
        public string Name { get; set; } 
        public int GridHolder { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public enum TableStatue
    {
        Empty = 0,
        In_Use = 1,
        Reserved = 2
    }

    public enum TableStatueTR
    {
        Boş = 0,
        Dolu = 1,
        Rezerveli = 2
    }

    public enum TableStatueFR
    {
        Vide = 0,
        Utilisé = 1,
        Réservé = 2
    }

    public enum TableStatueGER
    {
        Leer = 0,
        In_Benutzung = 1,
        Reserviert = 2
    }

    [Serializable()]
    public class Table : System.Runtime.Serialization.ISerializable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TableScence { get; set; }
        public string TableColor { get; set; }
        public string TableType { get; set; }
        public int MaxChair { get; set; }
        public TableStatue CurrentStatue { get; set; }
        public string Description { get; set; }
        public bool[] Checks { get; set; }

        public String ConvertBoolArrayToString(bool[] Value)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < Value.Length; i++)
            {
                sb.Append(Convert.ToInt32(Value[i]).ToString());
                if (i != Value.Length - 1)
                    sb.Append('|');
            }
            return sb.ToString();
        }

        public bool[] ConvertStringToBoolArray(String Value)
        {
            string[] Splitted = Value.Split('|');

            System.Collections.Generic.List<bool> list = new System.Collections.Generic.List<bool>();

            for (int i = 0; i < Splitted.Length; i++)
                if (Splitted[i] == "1") list.Add(true);
                else list.Add(false);

                    return list.ToArray();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
