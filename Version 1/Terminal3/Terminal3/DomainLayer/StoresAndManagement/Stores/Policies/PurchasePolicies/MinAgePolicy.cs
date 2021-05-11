using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinAgePolicy : IPurchasePolicy
    {
        public int Age { get; }
        public string Id { get; }

        public MinAgePolicy(int age, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Age = age;
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

        public Result<bool> RemovePolicy(string id)
        {
            return new Result<bool>("", true,false);
        }
    }
}
