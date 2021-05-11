using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    class MaxProductCondition : AbstractDiscountCondition
    {

        public int MaxQuantity { get; }
        public Product Product { get; }

        public MaxProductCondition(Product product, int maxQuantity, String id = "") : base(id)
        {
            Product = product;
            MaxQuantity = maxQuantity;
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            if (products.ContainsKey(Product) && products[Product] <= MaxQuantity)
                return new Result<bool>("", true, true);
            return new Result<bool>("", true, false);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            return new Result<bool>("", true, false);
        }
    }
}
