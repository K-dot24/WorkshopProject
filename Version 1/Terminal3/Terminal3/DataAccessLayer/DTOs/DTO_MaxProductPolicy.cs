using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DataAccessLayer.DTOs
{
    [BsonIgnoreExtraElements]
    public class DTO_MaxProductPolicy : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public String Product { get; set; }
        [BsonElement]
        public int Max { get; set; }        

        
        public DTO_MaxProductPolicy(string id, String product, int max)
        {
            _id = id;
            Product = product;
            Max = max;
        }
    }
}
