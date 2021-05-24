using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using Terminal3.DataAccessLayer.DTOs;

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
            var json = Document.ToJson();
            if(json.StartsWith("{ \"_id\" : ObjectId(")) { json = "{"+ json.Substring(47);  }
            T dto = JsonConvert.DeserializeObject<T>(json);
            return dto;
        }

        //public T LoadComplexPolicies(FilterDefinition<BsonDocument> filter)
        //{
        //    var Document = collection.Find(filter).FirstOrDefault();
        //    var Policies = Document.GetValue("Policies");
        //    var type = Policies[0]["_t"];

        //    DTO_AndPolicy and_dto = new DTO_AndPolicy(Document["_id"]);
        //    and_dto.Policies.Add()

        //    Document.Remove("Policies");
        //    var json = Document.ToJson();
        //    T dto = JsonConvert.DeserializeObject<T>(json);
        //    return dto;
        //}

        public void Update(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            collection.UpdateOne(filter, update);
        }
    }
}
