using System.Collections.Generic;

namespace Me.App
{
    public class ProductData
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int InStock { get; set; }
        public double UnitPrice { get; set; }

        public ProductData(string category, string name, int inStock, double unitPrice)
        {
            Category = category;
            Name = name;
            InStock = inStock;
            UnitPrice = unitPrice;
        }

        
    }

    public class ProductRepo
    {
        public List<ProductData> InsertProducts()
        {
            return new List<ProductData>
            {
                new ProductData("Seafood", "Ikura", 31, 31.00),
                new ProductData("Seafood", "Konbu", 24, 6.00),
                new ProductData("Seafood", "Inlagd Sill", 112, 19.00),
                new ProductData("Produce", "Tofu", 35, 23.25),
                new ProductData("Produce", "Longlife Tofu", 4, 10.00),
                new ProductData("Produce", "Manjimup Dried Apples", 20, 53.00),
                new ProductData("Confections", "Pavlova", 29, 17.45),
                new ProductData("Confections", "Chocolade", 15, 12.75),
                new ProductData("Confections", "Maxilaku", 10, 20.00)
            };
        }
    }
}
