using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_ConditionalDiscount
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Condition { get; set; }         // <id , type IDiscountCondition >
        [BsonElement]
        public ConcurrentDictionary<String, String> Discount { get; set; }          // <id , type IDiscountPolicy >

        public DTO_ConditionalDiscount(string id, ConcurrentDictionary<string, string> condition, ConcurrentDictionary<string, string> discount)
        {            
            _id = id;
            Condition = condition;
            Discount = discount;
        }
    }
}
