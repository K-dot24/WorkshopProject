using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_ShoppingBag
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public String UserId { get; set; }
        [BsonElement]
        public String StoreId { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, int> Products { get; set; }     // <Product id, Quantity>
        [BsonElement]
        public Double TotalBagPrice { get; set; }

        public DTO_ShoppingBag(String id, String userId, String storeId, ConcurrentDictionary<String, int> products, Double totalBagPrice)
        {
            _id = id;
            UserId = userId;
            StoreId = storeId;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }
    }
}
