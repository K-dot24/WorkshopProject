using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs.Policies.Discount;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Store
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public String Name { get; set; }
        [BsonElement]
        public String Founder { get; set; }
        [BsonElement]
        public LinkedList<String> Owners { get; set; }
        [BsonElement]
        public LinkedList<String> Managers { get; set; }
        [BsonElement]
        public LinkedList<String> InventoryManager { get; set; }     //List of store products
        [BsonElement]
        public DTO_History History { get; set; }
        [BsonElement]
        public Double Rating { get; set; }
        [BsonElement]
        public int NumberOfRates { get; set; }
        [BsonElement]
        public Boolean isClosed { get; set; }
        [BsonElement]
        public DTO_DiscountAddition DiscountRoot { get; set; } // List of IDiscountPolicy ids 
        [BsonElement]
        public DTO_BuyNow PurchaseRoot { get; set; }



        public DTO_Store(String id, String name, String founder, LinkedList<String> owners, LinkedList<String> managers, LinkedList<String> inventoryManager, DTO_History history, 
                            Double rating, int numberOfRates, Boolean isclosed , DTO_DiscountAddition discount , DTO_BuyNow by_now)
        {
            _id = id;
            Name = name;
            Founder = founder;
            Owners = owners;
            Managers = managers;
            InventoryManager = inventoryManager;
            History = history;
            Rating = rating;
            NumberOfRates = numberOfRates;
            isClosed = isclosed;
            DiscountRoot = discount;
            PurchaseRoot = by_now;
        }
    }
}
