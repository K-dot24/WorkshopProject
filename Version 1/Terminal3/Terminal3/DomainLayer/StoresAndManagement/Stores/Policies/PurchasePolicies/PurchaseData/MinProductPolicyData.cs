using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinProductPolicyData : IPurchasePolicyData
    {
        public string ProductId { get; }
        public int Min { get; }
        public string Id { get; }

        public MinProductPolicyData(string productId, int min, string id)
        {
            this.ProductId = productId;
            this.Min = min;
            this.Id = id;
        }
    }
}
