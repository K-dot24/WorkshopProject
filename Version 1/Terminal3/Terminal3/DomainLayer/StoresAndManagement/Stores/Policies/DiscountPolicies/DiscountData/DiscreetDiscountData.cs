using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData
{
    class DiscreetDiscountData : AbstractDiscountPolicyData
    {

        public String DiscountCode { get; }
        public IDiscountPolicyData Discount { get; }

        public DiscreetDiscountData(IDiscountPolicyData discount, String discountCode, String id = "") : base(id)
        {
            Discount = discount;
            DiscountCode = discountCode;
        }

    }
}
