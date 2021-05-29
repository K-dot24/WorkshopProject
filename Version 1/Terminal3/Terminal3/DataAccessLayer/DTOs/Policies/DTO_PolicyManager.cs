using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs.Policies
{
    public class DTO_PolicyManager
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public List<String> DiscountRoot { get; set; } // List of IDiscountPolicy ids 

        [BsonElement]
        public String PurchaseRoot { get; set; }  //And policy id

        public DTO_PolicyManager(string id, List<String> discount, String purchase)
        {
            _id = id;
            DiscountRoot = discount;
            PurchaseRoot = purchase;
        }
    }
}
