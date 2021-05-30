using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_MaxProductCondition
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public int MaxQuantity { get; set; }
        [BsonElement]
        public String Product { get; set; }  // product id

        public DTO_MaxProductCondition(string id, int maxQuantity, string product)
        {
            _id = id;
            MaxQuantity = maxQuantity;
            Product = product;
        }
    }
}
