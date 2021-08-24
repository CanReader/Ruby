namespace Ruby.Cafe.Model
{
    public class ServingItem
    {
        public string Serving { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return Serving;
        }
    }

    public struct Category
    {
        public int ID { get; set; }
        public string Name {get;set;}

        public override string ToString()
        {
            return Name;
        }
    }

    public class Product
    { 
        public int ID { get; set; }
        public string Name { get; set; }    
        public Category category { get; set; }
        public double Tax { get; set; }
        public string Barcode { get; set; }
        public System.Collections.Generic.List<ServingItem> Servings { get; set; }

        public ServingItem CurrentServing { get; set; }
        public int Multiplier { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
