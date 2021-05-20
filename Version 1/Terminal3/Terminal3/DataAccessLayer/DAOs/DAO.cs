using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DataAccessLayer.DAOs
{
    public class DAO<T>
    {
        // Field
        public IMongoCollection<BsonDocument> collection;

        //Constructor
        public DAO(IMongoDatabase database , string collectinName)
        {
            collection = database.GetCollection<BsonDocument>(collectinName);
        }

        public void Create(T dto)
        {
            var doc = dto.ToBsonDocument();
            collection.InsertOne(doc);
        }

        public T Delete(FilterDefinition<BsonDocument> filter)
        {
            //collection.DeleteOne(filter);
            BsonDocument deletedDocument = collection.FindOneAndDelete(filter);
            T dto = JsonConvert.DeserializeObject<T>(deletedDocument.ToJson());
            return dto;
        }

        public T Load(FilterDefinition<BsonDocument> filter)
        {
            var Document = collection.Find(filter).FirstOrDefault();
            T dto = JsonConvert.DeserializeObject<T>(Document.ToJson());
            return dto;
        }

        public void Update(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            collection.UpdateOne(filter, update);
        }
    }
}
