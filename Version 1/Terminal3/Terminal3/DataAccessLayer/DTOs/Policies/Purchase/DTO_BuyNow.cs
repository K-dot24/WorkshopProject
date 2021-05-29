using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_BuyNow
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public DTO_AndPolicy Policy { get; set; }

        public DTO_BuyNow(string id, DTO_AndPolicy policy)
        {
            _id = id;
            Policy = policy;
        }
    }
}
