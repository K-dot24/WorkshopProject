using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountXor
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Discount1 { get; set; }       // <IDiscountPolicy id , type >
        [BsonElement]
        public ConcurrentDictionary<String, String> Discount2 { get; set; }       // <IDiscountPolicy id , type >
        [BsonElement]
        public ConcurrentDictionary<String, String> ChoosingCondition { get; set; }        // <IDiscountCondition id , type>

        public DTO_DiscountXor(string id, ConcurrentDictionary<string, string> discount1, ConcurrentDictionary<string, string> discount2, ConcurrentDictionary<string, string> choosingCondition)
        {
            _id = id;
            Discount1 = discount1;
            Discount2 = discount2;
            ChoosingCondition = choosingCondition;
        }
    }
}
