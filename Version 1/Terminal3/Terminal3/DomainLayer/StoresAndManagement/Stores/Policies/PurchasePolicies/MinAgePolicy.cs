using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
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
            int age = (int)info["Age"];

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

        public Result<bool> RemovePolicy(string id)
        {
            return new Result<bool>("", true,false);
        }

        public Result<IPurchasePolicyData> GetData()
        {
            return new Result<IPurchasePolicyData>("", true, new MinAgePolicyData(Age, Id));
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("Age"))
                Age = (int)info["Age"];            

            return new Result<bool>("", true, true);
        }
    }
}
