using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class AndPolicy : IPurchasePolicy
    {
        
        public List<IPurchasePolicy> Policies { get; }
        public string Id { get; }

        public AndPolicy(string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Policies = new List<IPurchasePolicy>();
        }

        public AndPolicy(List<IPurchasePolicy> policies, string id = "") : this(id)
        {
            if (policies != null)
                this.Policies = policies;
        }

        public static Result<IPurchasePolicy> create(Dictionary<string, object> info)
        {        
            return new Result<IPurchasePolicy>("", true, new AndPolicy());
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            bool res = true;
            foreach(IPurchasePolicy policy in Policies)
            {
                if (res && !policy.IsConditionMet(bag,user).Data)
                {
                    res = false;
                }
            }
            return new Result<bool>("", true, res);
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
                Result<bool> res = p.AddPolicy(policy, id);
                if (!res.ExecStatus)
                    return res;
                if (res.Data)
                    return res;
            }
            return new Result<bool>("", true , false);
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
            foreach(IPurchasePolicy policy in Policies)
            {
                dataPolicies.Add(policy.GetData().Data);
            }
            return new Result<IPurchasePolicyData>("",true, new AndPolicyData(dataPolicies, Id));
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

        public DTO_AndPolicy getDTO()
        {           
            return new DTO_AndPolicy(this.Id, getPolicis_dto());
        }

        public ConcurrentDictionary<String, String> getPolicis_dto()
        {
            ConcurrentDictionary<String, String> policies_dto = new ConcurrentDictionary<String, String>();
            foreach (IPurchasePolicy policy in Policies)
            {
                string[] type = policy.GetType().ToString().Split('.');
                string policy_type = type[type.Length - 1];
                policies_dto.TryAdd(policy.Id, policy_type);
            }

            return policies_dto;
        }

        public DTO_AndPolicy getDTO()
        {           
            return new DTO_AndPolicy(this.Id, getPolicis_dto());
        }

        public ConcurrentDictionary<String, String> getPolicis_dto()
        {
            ConcurrentDictionary<String, String> policies_dto = new ConcurrentDictionary<String, String>();
            foreach (IPurchasePolicy policy in Policies)
            {
                string[] type = policy.GetType().ToString().Split('.');
                string policy_type = type[type.Length - 1];
                policies_dto.TryAdd(policy.Id, policy_type);
            }

            return policies_dto;
        }
    }
}
