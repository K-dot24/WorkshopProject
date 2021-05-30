using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_DiscountTargetCategories 
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public List<string> Categories { get; set; }

        public DTO_DiscountTargetCategories(string id, List<string> categories)
        {
            _id = id;
            Categories = categories;
        }
    }
}
