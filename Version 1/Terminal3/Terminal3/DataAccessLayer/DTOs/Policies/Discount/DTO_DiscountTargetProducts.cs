using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountTargetProducts
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public List<String> Products { get; set; }      // List<ProductID>

        public DTO_DiscountTargetProducts(string id, List<string> products)
        {
            _id = id;
            Products = products;
        }
    }
}
