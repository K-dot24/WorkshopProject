using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_ConditionalPolicy
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> PreCond { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Cond { get; set; }

        public DTO_ConditionalPolicy(string id, ConcurrentDictionary<string, string> preCond, ConcurrentDictionary<string, string> cond)
        {
            _id = id;
            PreCond = preCond;
            Cond = cond;
        }
    }
}
