using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;

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
            Console.WriteLine(json);
            T dto = JsonConvert.DeserializeObject<T>(json);
            return dto;
        }

        public void Update(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            collection.UpdateOne(filter, update);
        }
    }
}
