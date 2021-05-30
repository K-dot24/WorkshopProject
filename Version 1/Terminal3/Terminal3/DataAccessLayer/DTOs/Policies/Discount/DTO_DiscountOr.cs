using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountOr
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Discounts { get; } // List <IDiscountPolicy id>         

        public DTO_DiscountOr(string id, ConcurrentDictionary<string, string> discounts)
        {
            _id = id;
            Discounts = discounts;
        }
    }
}
