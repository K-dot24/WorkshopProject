using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_AndPolicy : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Policies { get; set; }

        public DTO_AndPolicy(string id, ConcurrentDictionary<String, String> policies)
        {
            _id = id;
            Policies = policies;
        }
    }
}
