using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class BuyNowData : IPurchasePolicyData
    {
        public AndPolicyData Policy { get; }
        public string Id { get; }

        public BuyNowData(AndPolicyData policy, string id)
        {
            this.Policy = policy;
            this.Id = id;
        }
    }
}
