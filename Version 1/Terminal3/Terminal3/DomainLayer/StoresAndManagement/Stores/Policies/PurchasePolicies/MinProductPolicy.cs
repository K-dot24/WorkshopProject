using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinProductPolicy : IPurchasePolicy
    {
        public string ProductId { get; set; }
        public int Min { get; set; }
        public string Id { get; }

        public MinProductPolicy (string productId, int min, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.ProductId = productId;
            this.Min = min;
        }

        public static Result<IPurchasePolicy> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MinProductPolicy: ";
            if (!info.ContainsKey("ProductId"))
                return new Result<IPurchasePolicy>(errorMsg + "ProductId not found", false, null);
            string productId = ((JsonElement)info["ProductId"]).GetString();

            if (!info.ContainsKey("Min"))
                return new Result<IPurchasePolicy>(errorMsg + "Min not found", false, null);
            int min = ((JsonElement)info["Min"]).GetInt32();

            return new Result<IPurchasePolicy>("", true, new MinProductPolicy(productId, min));
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            int count;
            Product product = ContainsProduct(bag);
            if(product == null)
                return new Result<bool>("", true, true);
            bag.TryGetValue(product, out count);
            return new Result<bool>("", true, count >= this.Min);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
                return new Result<bool>("Cannot add a policy to this type of policy", false, false);
            return new Result<bool>("", true, false);
        }

        public Result<IPurchasePolicy> RemovePolicy(string id)
        {
            return new Result<IPurchasePolicy>("", true, null);
        }

        public Result<IDictionary<string, object>> GetData()
        {
            /*IDictionary<string, object> dict = new Dictionary<string, object>() { 
                { "Type", "MinProductPolicy" }, 
                { "Id", Id }, 
                { "Min", Min }, 
                { "ProductId", ProductId } 
            };
            return new Result<IDictionary<string, object>>("", true, dict);*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", ProductId + " >= " + Min },
                { "children", new Dictionary<String, object>[0] }
            };
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("Min"))
                Min = ((JsonElement)info["Min"]).GetInt32();

            if (info.ContainsKey("ProductId"))
                ProductId = ((JsonElement)info["ProductId"]).GetString();

            return new Result<bool>("", true, true);
        }

        private Product ContainsProduct(ConcurrentDictionary<Product, int> products)
        {
            foreach (KeyValuePair<Product, int> entry in products)
            {
                if (entry.Key.Id.Equals(ProductId))
                    return entry.Key;
            }
            return null;
        }

        public DTO_MinProductPolicy getDTO()
        {
            return new DTO_MinProductPolicy(this.Id, this.ProductId, this.Min);
        }
    }
}
