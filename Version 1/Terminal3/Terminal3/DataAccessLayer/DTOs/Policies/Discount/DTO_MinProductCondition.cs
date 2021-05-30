using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_MinProductCondition
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public int MinQuantity { get; set; }
        [BsonElement]
        public String Product { get; set; }  //product id

        public DTO_MinProductCondition(string id, int minQuantity, string product)
        {
            _id = id;
            MinQuantity = minQuantity;
            Product = product;
        }
    }
}
