using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_RegisteredUser
    {

        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public DTO_ShoppingCart ShoppingCart { get; set; }
        [BsonElement]
        public String Email { get; set; }
        [BsonElement]
        public String Password { get; set; }
        [BsonElement]
        public Boolean LoggedIn { get; set; }
        [BsonElement]
        public DTO_History History { get; set; }
        [BsonElement]
        public LinkedList<DTO_Notification> PendingNotification { get; }

        public DTO_RegisteredUser(String id, DTO_ShoppingCart shoppingCart, String email, String password, Boolean loggedIn, DTO_History history, LinkedList<DTO_Notification> pendingNotification)
        {
            _id = id;
            ShoppingCart = shoppingCart;
            Email = email;
            Password = password;
            LoggedIn = loggedIn;
            History = history;
            PendingNotification = pendingNotification;
        }
    }
}
