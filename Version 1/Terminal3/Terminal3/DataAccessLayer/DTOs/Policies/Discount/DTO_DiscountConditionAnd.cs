using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountConditionAnd
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Conditions { get; set; } //List <id of IDiscountCondition>

        public DTO_DiscountConditionAnd(string id, ConcurrentDictionary<string, string> conditions)
        {
            _id = id;
            Conditions = conditions;
        }
    }
}
