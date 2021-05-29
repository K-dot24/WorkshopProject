using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Lottery : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public Double Price { get; }
        [BsonElement]
        public ConcurrentDictionary<String, Double> Participants { get; set; }  // <UserID, winning %>

        public DTO_Lottery(string id, double price, ConcurrentDictionary<string, double> participants)
        {
            _id = id;
            Price = price;
            Participants = participants;
        }
    }
}
