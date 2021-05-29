using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData
{
    class MaxProductConditionData : AbstractDiscountConditionData
    {

        public int MaxQuantity { get; }
        public String ProductId { get; }

        public MaxProductConditionData(String productId, int maxQuantity, String id = "") : base(id)
        {
            ProductId = productId;
            MaxQuantity = maxQuantity;
        }

    }
}
