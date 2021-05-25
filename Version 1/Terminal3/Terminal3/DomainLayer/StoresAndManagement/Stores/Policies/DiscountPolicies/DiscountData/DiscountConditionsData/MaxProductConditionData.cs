using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData
{
    class MaxProductConditionData : AbstractDiscountConditionData
    {

        public int MaxQuantity { get; }
        public ProductService Product { get; }

        public MaxProductConditionData(ProductService product, int maxQuantity, String id = "") : base(id)
        {
            Product = product;
            MaxQuantity = maxQuantity;
        }

    }
}
