using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Product
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public String Name { get; set; }
        [BsonElement]
        public Double Price { get; set; }
        [BsonElement]
        public int Quantity { get; set; }       // product quantity in store
        [BsonElement]
        public String Category { get; set; }
        [BsonElement]
        public Double Rating { get; set; }
        [BsonElement]
        public int NumberOfRates { get; set; }
        [BsonElement]
        public LinkedList<String> Keywords { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Review { get; set; }    //<userID , usersReview>

        public DTO_Product(string id, string name, double price, int quantity, string category, double rating, int numberOfRates, LinkedList<string> keywords, ConcurrentDictionary<string, string> review)
        {
            _id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            Rating = rating;
            NumberOfRates = numberOfRates;
            Keywords = keywords;
            Review = review;
        }
    }
}
