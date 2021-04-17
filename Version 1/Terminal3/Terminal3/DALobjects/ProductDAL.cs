using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class ProductDAL
    {
        //Properties
        public String Id { get; }
        public String Name { get; }
        public Double Price { get; }
        public int Quantity { get; }
        public String Category { get; }

        //Constructor
        public ProductDAL(string id, string name, double price, int quantity, string category)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }
    }
}
