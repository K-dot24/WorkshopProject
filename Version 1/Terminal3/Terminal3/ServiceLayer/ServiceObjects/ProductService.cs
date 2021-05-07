using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ServiceLayer.ServiceObjects
{
    public class ProductService
    {
        //Properties
        public String Id { get; }
        public String Name { get; }
        public Double Price { get; }
        public int Quantity { get; }
        public String Category { get; }

        //Constructor
        public ProductService(string id, string name, double price, int quantity, string category)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }
    }
}
