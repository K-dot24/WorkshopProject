using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{    
    public class MaxProductPolicy : IPurchasePolicy
    {
        public Product Product { get; }
        public int Max { get; }
        public string Id { get; }

        public MaxProductPolicy(Product product, int max, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Product = product;
            this.Max = max;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            int count;
            bag.TryGetValue(this.Product, out count);
            return new Result<bool>("", true, bag.ContainsKey(this.Product) && count <= this.Max);
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
    }
}
