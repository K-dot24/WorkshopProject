using System;
using System.Collections.Generic;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public class Product
    {
        //Properties
        public String Id { get; }
        public String Name { get; }
        public Double Price { get; }
        public int Quantity { get; }
        public String Category { get; }
        public Double Rating { get; private set; }
        public int NumberOfRates { get; private set; }
        public LinkedList<String> Keywords { get; private set; }
        
        //Constructor
        public Product(String name, Double price, int quantity , String category, LinkedList<String> Keywords = null)
        {
            Id = Service.GenerateId();
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            if (Keywords == null) { this.Keywords = new LinkedList<String>(); }
        }
        
        public Product(ProductDAL productDAL)
        {
            Id = productDAL.Id;
            Name = productDAL.Name;
            Price = productDAL.Price;
            Quantity = productDAL.Quantity;
            Category = productDAL.Category;
        }

        //Method
        public Result<Double> AddRating(Double rate)
        {
            this.NumberOfRates=NumberOfRates+1;
            Rating = (Rating + rate) / NumberOfRates;
            return new Result<Double>($"Product {Name} rate is: {Rating}\n", true, Rating);
        }
        
        public Result<String> AddKeyword(String keyword)
        {
            this.Keywords.AddLast(keyword);
            return new Result<string>($"keyword:{keyword} has been added to product:{Name}", true, keyword);
        }

        public Result<ProductDAL> GetDAL()
        {
            return new Result<ProductDAL>("Product DAL object", true, new ProductDAL(this.Id, this.Name, this.Price, this.Quantity, this.Category));
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
