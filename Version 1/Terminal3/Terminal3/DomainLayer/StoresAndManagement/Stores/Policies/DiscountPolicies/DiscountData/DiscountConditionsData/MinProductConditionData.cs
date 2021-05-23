using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData
{
    class MinProductConditionData : AbstractDiscountConditionData
    {

        public int MinQuantity { get; }
        public ProductService Product { get; }

        public MinProductConditionData(ProductService product, int minQuantity, String id = "") : base(id)
        {
            Product = product;
            MinQuantity = minQuantity;
        }

    }
}
