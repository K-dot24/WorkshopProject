using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DAOs
{
    public interface DAOInterface<T>
    {
        public void Create(T dto);
        public T Load(FilterDefinition<BsonDocument> filter);
        public void Update(FilterDefinition<BsonDocument> filter , UpdateDefinition<BsonDocument> update);
        public void Delete(FilterDefinition<BsonDocument> filter);
    }
}
