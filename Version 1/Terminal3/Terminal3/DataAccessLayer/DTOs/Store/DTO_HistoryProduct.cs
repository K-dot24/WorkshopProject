using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_HistoryProduct
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public String Name { get; set; }
        [BsonElement]
        public Double Price { get; set; }
        [BsonElement]
        public int ProductQuantity { get; set; }    // not store quantity
        [BsonElement]
        public String Category { get; set; }

        public DTO_HistoryProduct(String id, String name, Double price, int productQuantity, String category)
        {
            _id = id;
            Name = name;
            Price = price;
            ProductQuantity = productQuantity;
            Category = category;
        }
    }


}
