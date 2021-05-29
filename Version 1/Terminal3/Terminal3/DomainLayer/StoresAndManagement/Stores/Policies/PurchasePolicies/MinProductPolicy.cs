using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinProductPolicy : IPurchasePolicy
    {
        public Product Product { get; }
        public int Min { get; }
        public string Id { get; }

        public MinProductPolicy (Product product, int min, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Product = product;
            this.Min = min;
        }
        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            int count;
            bag.TryGetValue(this.Product, out count);
            return new Result<bool>("", true, bag.ContainsKey(this.Product) && count >= this.Min);
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
            return new Result<IPurchasePolicyData>("", true, new MinProductPolicyData(Product.GetDAL().Data, Min, Id));
        }

        public Result<bool> EditPolicy(IPurchasePolicy policy, string id)
        {
            return new Result<bool>("", true, false);
        }

        public DTO_MinProductPolicy getDTO()
        {
            return new DTO_MinProductPolicy(this.Id, this.Product.Id, this.Min);
        }

        public DTO_MinProductPolicy getDTO()
        {
            return new DTO_MinProductPolicy(this.Id, this.Product.Id, this.Min);
        }
    }
}
