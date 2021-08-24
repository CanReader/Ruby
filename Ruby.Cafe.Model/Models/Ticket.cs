using System;
using System.Collections.Generic;
using System.Text;

namespace Ruby.Cafe.Model
{
    public class Ticket
    {
        public bool Opened = false;

        public int ID;

        public Employee AccessedEmployee { get; set; }
        public List<Product> Products { get; set; }
        public DateTime CreatedDate { get; set; }
        public Decimal TotalPrice { get; set; }
        public String PaymentType { get; set; }
        public int UsingTableID { get; set; }

        private DateTime CurrentDate => DateTime.Today;
        private String BName { get; set; }
        private String BPhone { get; set; }
        private Decimal TotalTax { get; set; }

        public Ticket()
        {
        }

    }
}
