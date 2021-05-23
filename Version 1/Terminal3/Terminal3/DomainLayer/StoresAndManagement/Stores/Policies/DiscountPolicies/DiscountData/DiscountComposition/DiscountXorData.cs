using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition
{
    class DiscountXorData : AbstractDiscountPolicyData
    {

        public IDiscountPolicyData Discount1 { get; }
        public IDiscountPolicyData Discount2 { get; }
        public IDiscountConditionData ChoosingCondition { get; }

        public DiscountXorData(IDiscountPolicyData discount1, IDiscountPolicyData discount2, IDiscountConditionData choosingCondition, String id = "") : base(id)
        {
            Discount1 = discount1;
            Discount2 = discount2;
            ChoosingCondition = choosingCondition;
        }

    }
}
