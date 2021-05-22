using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    [BsonIgnoreExtraElements]
    public class DTO_StoreOwner
    {
        [BsonElement]
        public String UserId { get; set; }
        [BsonElement]
        public String StoreId { get; set; }
        [BsonElement]
        public String AppointedBy { get; set; }
        [BsonElement]
        public LinkedList<String> StoreManagers { get; set; }
        [BsonElement]
        public LinkedList<String> StoreOwners { get; set; }

        public DTO_StoreOwner(String userId, String storeId, String appointedBy, LinkedList<String> storeManagers, LinkedList<String> storeOwners)
        {
            UserId = userId;
            StoreId = storeId;
            AppointedBy = appointedBy;
            StoreManagers = storeManagers;
            StoreOwners = storeOwners;
        }
    }
}
