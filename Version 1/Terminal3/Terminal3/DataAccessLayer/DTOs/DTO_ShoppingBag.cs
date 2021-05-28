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
        public String _id { get; }
        [BsonElement]
        public String UserId { get; }
        [BsonElement]
        public String StoreId { get; }
        [BsonElement]
        public ConcurrentDictionary<String, int> Products { get; }     // <Product id, Quantity>
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
