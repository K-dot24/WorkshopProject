using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscreetDiscount
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public String DiscountCode { get; set; }
        [BsonElement]
        public ConcurrentDictionary<String, String> Discount { get; set; }   //  <IDiscountPolicy type , Id>

        public DTO_DiscreetDiscount(string id, string discountCode, ConcurrentDictionary<string, string> discount)
        {
            _id = id;
            DiscountCode = discountCode;
            Discount = discount;
        }
    }
}
