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

        public void Create(T dto , MongoDB.Driver.IClientSessionHandle session = null)
        {
            try
            {
                var doc = dto.ToBsonDocument();
                if(session != null)
                {
                    collection.InsertOne(session, doc);
                }
                else
                {
                    collection.InsertOne(doc);
                }
                Mapper.connectionStatus = ConnectionStatus.OK;
            }
            catch(MongoWriteException e)
            {
                if (!e.ToString().Contains("E11000"))
                {
                    Console.WriteLine(e.ToString());
                    Logger.LogError(e.ToString());
                    updateConnectiviyError();
                }

            }
            catch(Exception e)
            {
                Logger.LogError(e.ToString());
                updateConnectiviyError();
            }
        }

        public T Delete(FilterDefinition<BsonDocument> filter, MongoDB.Driver.IClientSessionHandle session = null)
        {
            try
            {
                BsonDocument deletedDocument;
                if (session!=null)
                {
                    //collection.DeleteOne(mapper.session , filter);
                    deletedDocument = collection.FindOneAndDelete(session ,filter);
                }
                else
                {
                    //collection.DeleteOne(filter);
                    deletedDocument = collection.FindOneAndDelete(filter);
                }
                T dto = JsonConvert.DeserializeObject<T>(deletedDocument.ToJson());
                Mapper.connectionStatus = ConnectionStatus.OK;
                return dto;
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogError(e.ToString());
                updateConnectiviyError();
                return default(T);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                updateConnectiviyError();
                return default(T);
            }
        }

        public T Load(FilterDefinition<BsonDocument> filter , MongoDB.Driver.IClientSessionHandle session = null)
        {
            try
            {
                BsonDocument Document; 

                if (session!=null)
                {
                    Document = collection.Find(session,filter).FirstOrDefault();
                }
                else
                {
                    Document = collection.Find(filter).FirstOrDefault();
                }
                if (Document is null) { return default; }
                var json = Document.ToJson();
                if (json.StartsWith("{ \"_id\" : ObjectId(")) { json = "{" + json.Substring(47); }
                T dto = JsonConvert.DeserializeObject<T>(json);
                Mapper.connectionStatus = ConnectionStatus.OK;
                return dto;
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogError(e.ToString());
                updateConnectiviyError();
                return default(T);

            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                updateConnectiviyError();
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

        public void Update(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, Boolean upsert = false , MongoDB.Driver.IClientSessionHandle session = null)
        {
            try
            {
                if (upsert)
                {
                    if (session!=null)
                    {
                        collection.UpdateOne(session, filter, update, new UpdateOptions() { IsUpsert = upsert });
                    }
                    else
                    {
                        collection.UpdateOne(filter, update, new UpdateOptions() { IsUpsert = upsert });
                    }

                }
                else
                {
                    if (session!=null)
                    {
                        collection.UpdateOne(session,filter, update);
                    }
                    else
                    {
                        collection.UpdateOne(filter, update);
                    }
                }

                Mapper.connectionStatus = ConnectionStatus.OK;

            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogError(e.ToString());
                updateConnectiviyError();
            }
            //catch (Exception e)
            //{
            //    Logger.LogError(e.ToString());
            //    updateConnectiviyError();
            //}
        }

        public void updateConnectiviyError()
        {
            if (!Mapper.connectionStatus.Equals(ConnectionStatus.Error))
            {
                Mapper.connectionStatus = ConnectionStatus.Error;
                Mapper.NotifyConnectionError();

            }
        }
    }
}
