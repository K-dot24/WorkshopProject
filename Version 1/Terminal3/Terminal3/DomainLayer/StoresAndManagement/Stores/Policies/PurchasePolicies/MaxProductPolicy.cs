using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{    
    public class MaxProductPolicy : IPurchasePolicy
    {
        public string ProductId { get; set; }
        public int Max { get; set; }
        public string Id { get; }

        public MaxProductPolicy(string productId, int max, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.ProductId = productId;
            this.Max = max;
        }

        public static Result<IPurchasePolicy> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MaxProductPolicy: ";
            if (!info.ContainsKey("ProductId"))
                return new Result<IPurchasePolicy>(errorMsg + "ProductId not found", false, null);
            string productId = ((JsonElement)info["ProductId"]).GetString();

            if (!info.ContainsKey("Max"))
                return new Result<IPurchasePolicy>(errorMsg + "Max not found", false, null);
            int max = ((JsonElement)info["Max"]).GetInt32();

            return new Result<IPurchasePolicy>("", true, new MaxProductPolicy(productId, max));
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            int count;
            Product product = ContainsProduct(bag);
            if (product == null)
                return new Result<bool>("", true, false);
            bag.TryGetValue(product, out count);
            return new Result<bool>("", true, count <= this.Max);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
                return new Result<bool>("Cannot add a policy to this type of policy", false, false);
            return new Result<bool>("", true, false);
        }

        public Result<bool> RemovePolicy(string id)
        {
            return new Result<bool>("", true, false);
        }

        public Result<IPurchasePolicyData> GetData()
        {
            return new Result<IPurchasePolicyData>("", true, new MaxProductPolicyData(ProductId, Max, Id));
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("Max"))
                Max = ((JsonElement)info["Max"]).GetInt32();

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

        public DTO_MaxProductPolicy getDTO()
        {
            return new DTO_MaxProductPolicy(this.Id, this.ProductId, this.Max);
        }
    }

}
