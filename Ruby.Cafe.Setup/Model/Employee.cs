using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruby.Setup.Model
{
    [Serializable()]
    public struct Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool[] Perms { get; set; }

        #region Converter methods
        public string ConvertPermsToString(bool[] Perms)
        {
            int TotalVLines = Perms.Length - 1;

            System.Text.StringBuilder builder = new System.Text.StringBuilder(Perms.Length + TotalVLines);

            int i = 0;

            do
            {
                builder.AppendFormat("{0}|", Convert.ToInt32(Perms[i]).ToString());
                i++;
            } while (i < Perms.Length);

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        public bool[] ConvertStringToPerms(string Value)
        {
            System.Collections.Generic.List<bool> boolList = new System.Collections.Generic.List<bool>();

            foreach (var val in Value.Split('|'))
            {
                boolList.Add(val == "1");
            }

            return boolList.ToArray();
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

    }
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Gender { get; set; }
        public DateTime StartDate { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public Role role { get; set; }
        public string AuthCode { get; set; }

        public override string ToString()
        {
            return this.Name + " " + this.Surname;
        }
    }
}