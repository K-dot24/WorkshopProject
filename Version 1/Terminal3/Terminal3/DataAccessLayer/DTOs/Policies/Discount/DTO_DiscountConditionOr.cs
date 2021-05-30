using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountConditionOr
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Conditions { get; } //List <id of IDiscountCondition>

        public DTO_DiscountConditionOr(string id, ConcurrentDictionary<string, string> conditions)
        {
            _id = id;
            Conditions = conditions;
        }
    }
}
