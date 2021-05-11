using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData
{
    class ConditionalDiscountData : AbstractDiscountPolicyData
    {

        public IDiscountConditionData Condition { get; }
        public IDiscountPolicyData Discount { get; }

        public ConditionalDiscountData(IDiscountPolicyData discount, IDiscountConditionData condition, String id = "") : base(id)
        {
            Condition = condition;
            Discount = discount;
        }

    }
}
