using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_MinAgePolicy : DTO_Policies
    {
        [BsonId]
        public string _id { get; set; }
        [BsonElement]
        public int Age { get; set; }

        public DTO_MinAgePolicy(string id, int age)
        {
            _id = id;
            Age = age;
        }
    }
}
