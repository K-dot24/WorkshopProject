using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Auction : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public String ClosingTime { get; set; }
        [BsonElement]
        public Double StartingPrice { get; set; }
        [BsonElement]
        public Double LastOffer_Price { get; set; }    // Customer offer <price, UserID>
        public String LastOffer_UserId { get; set; }    // Customer offer <price, UserID>

        public DTO_Auction(string id, string closingTime, double startingPrice, double lastOffer_Price, string lastOffer_UserId)
        {
            _id = id;
            ClosingTime = closingTime;
            StartingPrice = startingPrice;
            LastOffer_Price = lastOffer_Price;
            LastOffer_UserId = lastOffer_UserId;
        }
    }
}
