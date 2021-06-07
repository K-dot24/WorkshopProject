using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Recipt
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public String store_id { get; set; }
        [BsonElement]
        public Double amount { get; set; }
        [BsonElement]
        public String date { get; set; }

        public DTO_Recipt(string store_id, double amount, String date)
        {
            _id = Service.GenerateId();
            this.store_id = store_id;
            this.amount = amount;
            this.date = date;
        }

    }
}
