using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Store
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public String Name { get; set; }
        [BsonElement]
        public String Founder { get; set; }
        [BsonElement]
        public LinkedList<String> Owners { get; set; }
        [BsonElement]
        public LinkedList<String> Managers { get; set; }
        [BsonElement]
        public LinkedList<String> InventoryManager { get; set; }     //List of store products

        //[BsonElement]
        //public DTO_PolicyManager PolicyManager { get; set;}           // TODO - need to be updated
        [BsonElement]
        public LinkedList<String> DiscountRoot { get; set; }            //list of discount policies id
        [BsonElement]
        public LinkedList<String> PurchaseRoot { get; set; }
        [BsonElement]
        public DTO_History History { get; set; }
        [BsonElement]
        public Double Rating { get; set; }
        [BsonElement]
        public int NumberOfRates { get; set; }
        [BsonElement]
        public Boolean isClosed { get; set; }


        public DTO_Store(String id, String name, String founder, LinkedList<String> owners, LinkedList<String> managers, LinkedList<String> inventoryManager, DTO_History history, Double rating, int numberOfRates, Boolean isclosed)
        {
            _id = id;
            Name = name;
            Founder = founder;
            Owners = owners;
            Managers = managers;
            InventoryManager = inventoryManager;
            History = history;
            Rating = rating;
            NumberOfRates = numberOfRates;
            isClosed = isclosed;
        }
    }
}
