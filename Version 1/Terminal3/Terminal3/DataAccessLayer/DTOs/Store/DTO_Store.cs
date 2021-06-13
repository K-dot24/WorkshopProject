using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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
        public DTO_DiscountAddition MainDiscount { get; set; } // List of IDiscountPolicy ids 
        [BsonElement]
        public DTO_BuyNow MainPolicy { get; set; }
        public List<DTO_Offer> OfferManager { get; set; }

        public DTO_Store(string id, string name, string founder, LinkedList<string> owners, LinkedList<string> managers, LinkedList<string> inventoryManager, DTO_History history, double rating, int numberOfRates, bool isClosed, DTO_DiscountAddition mainDiscount, DTO_BuyNow mainPolicy, List<DTO_Offer> offerManager)
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
            this.isClosed = isClosed;
            MainDiscount = mainDiscount;
            MainPolicy = mainPolicy;
            OfferManager = offerManager;
        }
    }
}
