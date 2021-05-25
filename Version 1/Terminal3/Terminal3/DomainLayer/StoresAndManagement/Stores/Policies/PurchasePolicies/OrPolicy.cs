using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class OrPolicy : IPurchasePolicy
    {
        public List<IPurchasePolicy> Policies { get; }
        public string Id { get; }

        public OrPolicy(string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Policies = new List<IPurchasePolicy>();
        }
        public OrPolicy(List<IPurchasePolicy> policies, string id = "") : this(id)
        {
            if (policies != null)
                this.Policies = policies;
        }
        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {       
            foreach (IPurchasePolicy policy in Policies)
            {
                if (policy.IsConditionMet(bag, user).Data)
                {
                    return new Result<bool>("", true, true);
                }
            }
            return new Result<bool>("", true, false);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
            {
                Policies.Add(policy);
                return new Result<bool>("", true, true);
            }
            foreach (IPurchasePolicy p in Policies)
            {
                Result<bool> curr = p.AddPolicy(policy, id);
                if (!curr.ExecStatus)
                    return curr;
                if (curr.Data)
                    return new Result<bool>("", true, true);
            }
            return new Result<bool>("", true, false);

        }

        public Result<bool> RemovePolicy(string id)
        {
            if (Policies.RemoveAll(policy => policy.Id.Equals(id)) >= 1)
                return new Result<bool>("", true, true);

            foreach (IPurchasePolicy policy in Policies)
            {
                Result<bool> res = policy.RemovePolicy(id);
                if (!res.ExecStatus)
                    return res;
                if (res.Data)
                    return res;
            }
            return new Result<bool>("", true, false);
        }

        public Result<IPurchasePolicyData> GetData()
        {
            List<IPurchasePolicyData> dataPolicies = new List<IPurchasePolicyData>();
            foreach (IPurchasePolicy policy in Policies)
            {
                dataPolicies.Add(policy.GetData().Data);
            }
            return new Result<IPurchasePolicyData>("", true, new OrPolicyData(dataPolicies, Id));
        }

        public Result<bool> EditPolicy(IPurchasePolicy policy, string id)
        {
            if (Policies.RemoveAll(policy => policy.Id.Equals(id)) >= 1)
            {
                Policies.Add(policy);
                return new Result<bool>("", true, true);
            }

            foreach (IPurchasePolicy p in Policies)
            {
                Result<bool> res = p.EditPolicy(policy, id);                
                if (res.Data)
                    return res;
            }
            return new Result<bool>("", true, false);
        }
    }
}
