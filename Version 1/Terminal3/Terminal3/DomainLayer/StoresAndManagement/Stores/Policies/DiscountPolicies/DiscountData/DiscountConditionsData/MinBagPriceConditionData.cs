using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData
{
    class MinBagPriceConditionData : AbstractDiscountConditionData
    {

        public Double MinPrice { get; }

        public MinBagPriceConditionData(Double minPrice, String id = "") : base(id)
        {
            MinPrice = minPrice;
        }

    }
}
