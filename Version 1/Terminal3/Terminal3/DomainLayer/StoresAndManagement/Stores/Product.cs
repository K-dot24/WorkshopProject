using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public class Product
    {
        public String Id { get; }
        public String Name { get; }
        public Double Price { get; }
        public int Quantity { get; }
        public String Category { get; }

        public Product(string name, double price, int quantity , String category)
        {
            Id = Service.GenerateId();
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        //TODO: functions?
    }
}
