using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class OrPolicyData : IPurchasePolicyData
    {
        public List<IPurchasePolicyData> Policies { get; }
        public string Id { get; }

        public OrPolicyData(List<IPurchasePolicyData> policies, string id)
        {
            this.Policies = policies;
            this.Id = id;
        }
    }
}
