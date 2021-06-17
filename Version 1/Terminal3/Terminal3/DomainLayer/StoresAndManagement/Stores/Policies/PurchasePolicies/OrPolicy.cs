using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
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

        public static Result<IPurchasePolicy> create(Dictionary<string, object> info)
        {
            return new Result<IPurchasePolicy>("", true, new OrPolicy());
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            if (Policies.Count == 0)
            {
                return new Result<bool>("", true, true);

            }
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

        public Result<IPurchasePolicy> RemovePolicy(string id)
        {
            IPurchasePolicy policy = Policies.Find(policy => policy.Id.Equals(id));
            if (policy != null)
            {
                Policies.Remove(policy);
                return new Result<IPurchasePolicy>("", true, policy);
            }

            foreach (IPurchasePolicy curr in Policies)
            {
                Result<IPurchasePolicy> res = curr.RemovePolicy(id);
                if (!res.ExecStatus)
                    return res;
                if (res.Data != null)
                    return res;
            }
            return new Result<IPurchasePolicy>("", true, null);
        }

        public Result<IDictionary<string, object>> GetData()
        {
            /*List<IDictionary<string, object>> dataPolicies = new List<IDictionary<string, object>>();
            foreach (IPurchasePolicy policy in Policies)
            {
                dataPolicies.Add(policy.GetData().Data);
            }
            IDictionary<string, object> dict = new Dictionary<string, object>() { 
                { "Type", "OrPolicy" }, 
                { "Id", Id}, 
                {"Policies", dataPolicies} 
            };
            return new Result<IDictionary<string, object>>("", true, dict);*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "Or"},
                { "children", new Dictionary<String, object>[0] }
            };
            List<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
            foreach (IPurchasePolicy myPolicy in Policies)
            {
                Result<IDictionary<string, object>> purchasePolicyResult = myPolicy.GetData();
                if (!purchasePolicyResult.ExecStatus)
                    return purchasePolicyResult;
                children.Add(purchasePolicyResult.Data);
            }
            dict["children"] = children.ToArray();
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                foreach (IPurchasePolicy myDiscount in Policies)
                {
                    Result<bool> result = myDiscount.EditPolicy(info, id);
                    if (result.ExecStatus && result.Data)
                        return result;
                    if (!result.ExecStatus)
                        return result;
                }
                return new Result<bool>("", true, false);
            }

            return new Result<bool>("", true, true);
        }

        public DTO_OrPolicy getDTO()
        {
            return new DTO_OrPolicy(this.Id, getPoliciesIDs(this.Policies));
        }

        private ConcurrentDictionary<String, String> getPoliciesIDs(List<IPurchasePolicy> list)
        {
            ConcurrentDictionary<String, String> Policies = new ConcurrentDictionary<String, String>();
            foreach (IPurchasePolicy policy in list)
            {
                string[] type = policy.GetType().ToString().Split('.');
                string policy_type = type[type.Length - 1];
                Policies.TryAdd(policy_type, policy.Id);
            }
            return Policies;
        }
    }
}
