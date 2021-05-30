using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountAnd
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Discounts { get; set; } // List <IDiscountPolicy id>         

        public DTO_DiscountAnd(string id, ConcurrentDictionary<string, string> discounts)
        {
            _id = id;
            Discounts = discounts;
        }
    }
}
