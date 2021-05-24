using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Offer : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public Double LastOffer_Price { get; set; }    // Customer offer <price, UserID>
        [BsonElement]
        public String LastOffer_UserId { get; set; }    // Customer offer <price, UserID>
        [BsonElement]
        public Double CounterOffer { get; set; }    // Store offer
        [BsonElement]
        public Boolean Accepted { get; set; }

        public DTO_Offer(string id, double lastOffer_Price, string lastOffer_UserId, double counterOffer, bool accepted)
        {
            _id = id;
            LastOffer_Price = lastOffer_Price;
            LastOffer_UserId = lastOffer_UserId;
            CounterOffer = counterOffer;
            Accepted = accepted;
        }
    }
}
