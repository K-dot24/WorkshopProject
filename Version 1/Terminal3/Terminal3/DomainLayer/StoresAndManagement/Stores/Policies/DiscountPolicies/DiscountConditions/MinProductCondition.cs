using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MinProductCondition : IDiscountCondition
    {

        public int MinQuantity { get; }
        public Product Product { get; }

        public MinProductCondition(Product product, int minQuantity)
        {
            Product = product;
            MinQuantity = minQuantity;
        }

        public Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            if (products.ContainsKey(Product) && products[Product] >= MinQuantity)
                return new Result<bool>("", true, true);
            return new Result<bool>("", true, false);
        }

    }
}
