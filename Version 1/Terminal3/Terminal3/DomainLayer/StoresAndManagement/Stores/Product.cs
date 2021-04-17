using System;
using Terminal3.DALobjects;

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

        public Product(ProductDAL productDAL)
        {
            Id = productDAL.Id;
            Name = productDAL.Name;
            Price = productDAL.Price;
            Quantity = productDAL.Quantity;
            Category = productDAL.Category
        }

        public Result<ProductDAL> GetDAL()
        {
            return new Result<ProductDAL>("Product DAL object", true, new ProductDAL(this.Id, this.Name, this.Price, this.Quantity, this.Category));
        }

        //TODO: functions?
    }
}
