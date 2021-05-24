using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_OrPolicy
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Policies { get; set; }

        public DTO_OrPolicy(string id, ConcurrentDictionary<string, string> policies)
        {
            _id = id;
            Policies = policies;
        }
    }
}
