using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_RestrictedHoursPolicy : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public String StartRestrict { get; set; }
        [BsonElement]
        public String EndRestrict { get; set; }
        [BsonElement]
        public String Product { get; set; }

        public DTO_RestrictedHoursPolicy(string id, string startRestrict, string endRestrict, string product)
        {
            _id = id;
            StartRestrict = startRestrict;
            EndRestrict = endRestrict;
            Product = product;
        }
    }
}
