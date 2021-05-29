using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class RestrictedHoursPolicyData : IPurchasePolicyData
    {
        public TimeSpan StartRestrict { get; }
        public TimeSpan EndRestrict { get; }
        public ProductService Product { get; }
        public string Id { get; }

        public RestrictedHoursPolicyData(TimeSpan startRestrict, TimeSpan endRestrict, ProductService product, string id)
        {
            this.StartRestrict = startRestrict;
            this.EndRestrict = endRestrict;
            this.Product = product;
            this.Id = id;
        }
    }
}
