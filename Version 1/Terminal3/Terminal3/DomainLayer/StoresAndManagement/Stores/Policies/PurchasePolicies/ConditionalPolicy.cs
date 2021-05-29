﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DataAccessLayer.DTOs;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class ConditionalPolicy : IPurchasePolicy
    {
        public IPurchasePolicy PreCond { get; set; }
        public IPurchasePolicy Cond { get; set; }
        public string Id { get;}

        public ConditionalPolicy(string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.PreCond = null;
            this.Cond = null;
        }

        public ConditionalPolicy(IPurchasePolicy preCond, IPurchasePolicy cond, string id = "") : this(id)
        {            
            this.PreCond = preCond;
            this.Cond = cond;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            if(this.PreCond == null || this.Cond == null)
                return new Result<bool>("", true, false);
            if (this.PreCond.IsConditionMet(bag, user).Data)
                if (!this.Cond.IsConditionMet(bag, user).Data)                
                    return new Result<bool>("", true, false);
            return new Result<bool>("", true, true);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
            {
                if (this.PreCond == null)
                    this.PreCond = policy;
                else if (this.Cond == null)
                    this.Cond = policy;
                else
                    return new Result<bool>("Cannot add a policy to this type of policy", false, false);
            }
            else
            {
                if (this.PreCond != null)
                {
                    Result<bool> curr = this.PreCond.AddPolicy(policy, id);
                    if (!curr.ExecStatus)
                        return curr;
                    if (curr.Data)
                        return new Result<bool>("", true, true);
                }
                if (this.Cond != null)
                {
                    Result<bool> curr = this.Cond.AddPolicy(policy, id);
                    if (!curr.ExecStatus)
                        return curr;
                    if (curr.Data)
                        return new Result<bool>("", true, true);
                }
            }
            return new Result<bool>("", true, false);
        }

        public Result<bool> RemovePolicy(string id)
        {
            if((PreCond != null && PreCond.Id.Equals(id)) || (Cond != null && Cond.Id.Equals(id)))
                return new Result<bool>("", false, false);

            Result<bool> res = PreCond.RemovePolicy(id);
            if (!res.ExecStatus)
                return res;
            if (res.Data)
                return res;

            res = Cond.RemovePolicy(id);
            if (!res.ExecStatus)
                return res;
            if (res.Data)
                return res;
            return new Result<bool>("", true, false);
        }

        public Result<IPurchasePolicyData> GetData()
        {
            IPurchasePolicyData pre = null;
            IPurchasePolicyData cond = null;
            if (PreCond != null)
                pre = PreCond.GetData().Data;
            if (Cond != null)
                cond = Cond.GetData().Data;
            return new Result<IPurchasePolicyData>("", true, new ConditionalPolicyData(pre, cond, Id));
        }

        public Result<bool> EditPolicy(IPurchasePolicy policy, string id)
        {
            if(PreCond != null)
            {
                if (PreCond.Id.Equals(id))
                {
                    PreCond = policy;
                    return new Result<bool>("", true, true);
                }
                Result<bool> res = PreCond.EditPolicy(policy, id);
                if (res.Data)
                    return res;
            }
            if (Cond != null)
            {
                if (Cond.Id.Equals(id))
                {
                    Cond = policy;
                    return new Result<bool>("", true, true);
                }
                Result<bool> res = Cond.EditPolicy(policy, id);
                if (res.Data)
                    return res;
            }                        
            return new Result<bool>("", true, false);
        }

        public DTO_ConditionalPolicy getDTO()
        {
            List<IPurchasePolicy> list = new List<IPurchasePolicy>();
            list.Add(this.PreCond);
            ConcurrentDictionary<String, String> PreCond = getPoliciesIDs(list);

            List<IPurchasePolicy> list2 = new List<IPurchasePolicy>();
            list2.Add(this.Cond);
            ConcurrentDictionary<String, String> Cond = getPoliciesIDs(list2);


            return new DTO_ConditionalPolicy(this.Id, PreCond, Cond);
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
