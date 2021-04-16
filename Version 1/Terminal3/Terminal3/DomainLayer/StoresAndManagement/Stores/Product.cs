using System;
using System.Collections.Generic;

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

        //TODO: functions?
    }
}
