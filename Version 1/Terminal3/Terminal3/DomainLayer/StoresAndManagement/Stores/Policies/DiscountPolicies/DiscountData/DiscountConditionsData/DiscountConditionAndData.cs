using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData
{
    class DiscountConditionAndData : AbstractDiscountConditionData
    {

        public List<IDiscountConditionData> Conditions { get; }

        public DiscountConditionAndData(List<IDiscountConditionData> conditions, String id = "") : base(id)
        {
            Conditions = conditions;
        }

    }
}
