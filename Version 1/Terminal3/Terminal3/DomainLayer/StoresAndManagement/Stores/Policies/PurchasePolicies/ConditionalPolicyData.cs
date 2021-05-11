using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class ConditionalPolicyData : IPurchasePolicyData
    {
        public IPurchasePolicyData PreCond { get; }
        public IPurchasePolicyData Cond { get; }
        public string Id { get; }


        public ConditionalPolicyData(IPurchasePolicyData preCond, IPurchasePolicyData cond, string id)
        {
            this.PreCond = preCond;
            this.Cond = cond;
            this.Id = id;
        }
    }
}
