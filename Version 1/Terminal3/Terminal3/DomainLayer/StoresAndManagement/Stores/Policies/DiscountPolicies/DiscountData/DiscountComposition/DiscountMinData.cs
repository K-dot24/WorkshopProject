using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition
{
    class DiscountMinData : AbstractDiscountPolicyData
    {

        public List<IDiscountPolicyData> Discounts { get; }

        public DiscountMinData(List<IDiscountPolicyData> discounts, String id = "") : base(id)
        {
            Discounts = discounts;
        }

    }
}
