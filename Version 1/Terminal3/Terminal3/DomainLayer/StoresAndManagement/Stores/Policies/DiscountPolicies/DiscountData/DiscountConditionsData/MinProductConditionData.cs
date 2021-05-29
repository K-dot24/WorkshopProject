using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData
{
    class MinProductConditionData : AbstractDiscountConditionData
    {

        public int MinQuantity { get; }
        public String ProductId { get; }

        public MinProductConditionData(String productId, int minQuantity, String id = "") : base(id)
        {
            ProductId = productId;
            MinQuantity = minQuantity;
        }

    }
}
