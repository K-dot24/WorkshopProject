using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

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
            return new Result<IPurchasePolicyData>("", true, new ConditionalPolicyData(PreCond.GetData().Data, Cond.GetData().Data, Id));
        }
    }
}
