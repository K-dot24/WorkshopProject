using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_MinProductPolicy : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public String Product { get; set; }
        [BsonElement]
        public int Min { get; set; }

        public DTO_MinProductPolicy(string id, string product, int min)
        {
            _id = id;
            Product = product;
            Min = min;
        }
    }
}
