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
        public LinkedList<DTO_Notification> PendingNotification { get; set; }
        [BsonElement]
        public List<DTO_Offer> PendingOffers { get; set; }
        [BsonElement]
        public List<DTO_Offer> AcceptedOffers { get; set; }

        public DTO_RegisteredUser(string id, DTO_ShoppingCart shoppingCart, string email, string password, bool loggedIn, DTO_History history, LinkedList<DTO_Notification> pendingNotification, List<DTO_Offer> pendingOffers, List<DTO_Offer> acceptedOffers)
        {
            _id = id;
            ShoppingCart = shoppingCart;
            Email = email;
            Password = password;
            LoggedIn = loggedIn;
            History = history;
            PendingNotification = pendingNotification;
            PendingOffers = pendingOffers;
            AcceptedOffers = acceptedOffers;
        }
    }
}
