using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinProductPolicyData : IPurchasePolicyData
    {
        public ProductService Product { get; }
        public int Min { get; }
        public string Id { get; }

        public MinProductPolicyData(ProductService product, int min, string id)
        {
            this.Product = product;
            this.Min = min;
            this.Id = id;
        }
    }
}
