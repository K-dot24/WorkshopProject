using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public class Product
    {
        //Properties
        public String Id { get; }
        public String Name { get; set; }
        public Double Price { get; set; }
        public int Quantity { get; set; }       // product quantity in store
        public String Category { get; set; }
        public Double Rating { get; set; }
        public int NumberOfRates { get; set; }
        public LinkedList<String> Keywords { get; set; }
        public ConcurrentDictionary<String, String> Review { get; set; }    //<userID , usersReview>
        public NotificationManager NotificationManager { get; set; }

        //Constructor
        public Product(String name, Double price, int quantity , String category, [OptionalAttribute]LinkedList<String> Keywords)
        {
            Id = Service.GenerateId();
            //Id = "1";  //TODO delete
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            if (Keywords == null) { this.Keywords = new LinkedList<String>(); }
            else { this.Keywords = Keywords; }
            Review = new ConcurrentDictionary<string, string>();
            this.NotificationManager = null;
        }

        public Product(String id , String name, Double price, int quantity, String category, LinkedList<String> Keywords , ConcurrentDictionary<string, string> review)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            this.Keywords = Keywords;
            Review = review;
            this.NotificationManager = null;
        }


        //Method
        public Result<Double> AddRating(Double rate)
        {
            if(rate>5 || rate< 1) {
                return new Result<Double>($"Product {Name} could not be rated. Please use number between 1 to 5\n", false, Rating);
            }
            else{
                this.NumberOfRates = NumberOfRates + 1;
                Rating = (Rating + rate) / (Double) NumberOfRates;
                return new Result<Double>($"Product {Name} rate is: {Rating}\n", true, Rating);
            }
        }
        
        public Result<String> AddKeyword(String keyword)
        {
            this.Keywords.AddLast(keyword);
            return new Result<string>($"keyword:{keyword} has been added to product:{Name}", true, keyword);
        }

        public Result<ConcurrentDictionary<String, String>> GetProductReview()
        {
            return new Result<ConcurrentDictionary<string, string>>("Products review\n", true, Review);
        }

        public Result<Boolean> AddProductReview(String userId , String review)
        {
            //TODO - check if user can add only one review and then overrride the last review ? or can add multiple reviews?
            Review.TryAdd(userId, review);
            return NotificationManager.notifyProductReview(this, review);
        }

        public Result<ProductService> GetDAL()
        {
            return new Result<ProductService>("Product DAL object", true, new ProductService(this.Id, this.Name, this.Price, this.Quantity, this.Category));
        }

        public Result<Boolean> UpdatePurchasedProductQuantity(int quantity)
        {
            if (this.NotificationManager == null)
            {
                return new Result<bool>("Error: No Notification Manager set for this product\n", false, false);
            }
            Quantity = Quantity - quantity;
            return NotificationManager.notifyStorePurchase(this, quantity);
        }

    }
}
