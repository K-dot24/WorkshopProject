using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DataAccessLayer;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinAgePolicy : IPurchasePolicy
    {
        public int Age { get; set; }
        public string Id { get; }

        public MinAgePolicy(int age, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Age = age;
        }

        public static Result<IPurchasePolicy> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MinAgePolicy: ";        

            if (!info.ContainsKey("Age"))
                return new Result<IPurchasePolicy>(errorMsg + "Age not found", false, null);
            int age = ((JsonElement)info["Age"]).GetInt32();

            return new Result<IPurchasePolicy>("", true, new MinAgePolicy(age));
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            return new Result<bool>("", true, true);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
                return new Result<bool>("Cannot add a policy to this type of policy", false, false);
            return new Result<bool>("", true, false);
        }

        public Result<IPurchasePolicy> RemovePolicy(string id)
        {
            return new Result<IPurchasePolicy>("", true,null);
        }

        public Result<IDictionary<string, object>> GetData()
        {
            /*IDictionary<string, object> dict = new Dictionary<string, object>() { 
                { "Type", "MinAgePolicy" }, 
                { "Id", Id }, 
                { "Age", Age } 
            };
            return new Result<IDictionary<string, object>>("", true, dict);*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "Age >= " + Age },
                { "children", new Dictionary<String, object>[0] }
            };
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("Age"))
            {
                Age = ((JsonElement)info["Age"]).GetInt32();
                var update_discount = Builders<BsonDocument>.Update.Set("Age", Age);
                Mapper.getInstance().UpdatePolicy(this, update_discount);
            }

            return new Result<bool>("", true, true);
        }

        public DTO_MinAgePolicy getDTO()
        {
            return new DTO_MinAgePolicy(this.Id, this.Age);
        }
    }
}
