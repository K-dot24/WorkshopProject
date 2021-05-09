using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MaxProductCondition : IDiscountCondition
    {

        public int MaxQuantity { get; }
        public Product Product { get; }

        public MaxProductCondition(Product product, int maxQuantity)
        {
            Product = product;
            MaxQuantity = maxQuantity;
        }

        public Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            if (products.ContainsKey(Product) && products[Product] <= MaxQuantity)
                return new Result<bool>("", true, true);
            return new Result<bool>("", true, false);
        }

    }
}
