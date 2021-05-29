using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_HistoryShoppingBag
    {
        [BsonId]
        public String _id { get; }
        [BsonElement]
        public String UserId { get; }
        [BsonElement]
        public String StoreId { get; }
        [BsonElement]
        public LinkedList<DTO_HistoryProduct> Products { get; set; }
        [BsonElement]
        public Double TotalBagPrice { get; }

        public DTO_HistoryShoppingBag(String id, String userId, String storeId, LinkedList<DTO_HistoryProduct> products, Double totalBagPrice)
        {
            _id = id;
            UserId = userId;
            StoreId = storeId;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }
    }
}
