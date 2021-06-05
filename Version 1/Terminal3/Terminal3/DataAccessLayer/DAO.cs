using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer;

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
            try
            {
                var doc = dto.ToBsonDocument();
                collection.InsertOne(doc);
            }
            catch(MongoWriteException e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogError(e.ToString());
            }
            catch(Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        public T Delete(FilterDefinition<BsonDocument> filter)
        {
            try
            {
                //collection.DeleteOne(filter);
                BsonDocument deletedDocument = collection.FindOneAndDelete(filter);
                T dto = JsonConvert.DeserializeObject<T>(deletedDocument.ToJson());
                return dto;
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogError(e.ToString());
                return default(T);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return default(T);
            }
        }

        public T Load(FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var Document = collection.Find(filter).FirstOrDefault();
                var json = Document.ToJson();
                if (json.StartsWith("{ \"_id\" : ObjectId(")) { json = "{" + json.Substring(47); }
                T dto = JsonConvert.DeserializeObject<T>(json);
                return dto;
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogError(e.ToString());
                return default(T);

            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return default(T);

            }
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

        public void Update(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, Boolean upsert = false)
        {
            if (upsert)
                collection.UpdateOne(filter, update, new UpdateOptions(){ IsUpsert = upsert});
            else collection.UpdateOne(filter, update);
        }
    }
}
