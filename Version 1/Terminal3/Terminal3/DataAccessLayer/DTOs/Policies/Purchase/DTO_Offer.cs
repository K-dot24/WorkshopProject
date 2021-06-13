using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Offer : DTO_Policies
    {

        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public String UserID { get; set; }     
        [BsonElement]
        public String ProductID { get; set; }     
        [BsonElement]
        public String StoreID { get; set; }
        [BsonElement]
        public int Amount { get; }
        [BsonElement]
        public Double Price { get; }
        [BsonElement]
        public Double CounterOffer { get; set; }
        [BsonElement]
        public List<String> acceptedOwners { get; }

        public DTO_Offer(string id, string userID, string productID, string storeID, int amount, double price, double counterOffer, List<string> acceptedOwners)
        {
            _id = id;
            UserID = userID;
            ProductID = productID;
            StoreID = storeID;
            Amount = amount;
            Price = price;
            CounterOffer = counterOffer;
            this.acceptedOwners = acceptedOwners;
        }
    }
}
