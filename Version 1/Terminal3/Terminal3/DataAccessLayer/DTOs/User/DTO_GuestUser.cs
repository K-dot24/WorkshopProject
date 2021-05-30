using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_GuestUser
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public DTO_ShoppingCart ShoppingCart { get; set; }
        [BsonElement]
        public Boolean Active { get; set; }

        public DTO_GuestUser(string id, DTO_ShoppingCart shoppingCart, bool active)
        {
            _id = id;
            ShoppingCart = shoppingCart;
            Active = active;
        }
    }
}
