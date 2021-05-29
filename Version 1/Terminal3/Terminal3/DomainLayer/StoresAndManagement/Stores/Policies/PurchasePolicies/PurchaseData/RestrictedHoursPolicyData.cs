using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class RestrictedHoursPolicyData : IPurchasePolicyData
    {
        public DateTime StartRestrict { get; }
        public DateTime EndRestrict { get; }
        public string ProductId { get; }
        public string Id { get; }

        public RestrictedHoursPolicyData(DateTime startRestrict, DateTime endRestrict, string productId, string id)
        {
            this.StartRestrict = startRestrict;
            this.EndRestrict = endRestrict;
            this.ProductId = productId;
            this.Id = id;
        }
    }
}
