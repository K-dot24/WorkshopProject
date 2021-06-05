using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition
{
    class DiscountOrData : AbstractDiscountPolicyData
    {

        public List<IDiscountPolicyData> Discounts { get; }

        public DiscountOrData(List<IDiscountPolicyData> discounts, String id = "") : base(id)
        {
            Discounts = discounts;
        }

    }
}
