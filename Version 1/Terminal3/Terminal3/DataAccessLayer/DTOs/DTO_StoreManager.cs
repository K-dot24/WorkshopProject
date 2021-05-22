using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    [BsonIgnoreExtraElements]
    public class DTO_StoreManager
    {
        [BsonElement]
        public String UserId { get; }
        [BsonElement]
        public Boolean[] Permission { get; }
        [BsonElement]
        public String AppointedBy { get; }
        [BsonElement]
        public String StoreId { get; }

        public DTO_StoreManager(String userId, Boolean[] permission, String appointedBy, String storeId)
        {
            UserId = userId;
            Permission = permission;
            AppointedBy = appointedBy;
            StoreId = storeId;
        }
    }
}
