using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MinBagPriceCondition : IDiscountCondition
    {

        public Double MinPrice { get; }

        public MinBagPriceCondition(Double minPrice)
        {
            MinPrice = minPrice;
        }

        public Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            double priceAcc = 0;
            foreach(KeyValuePair<Product, int> entry in products)
            {
                priceAcc += entry.Key.Price * entry.Value;
                if (priceAcc >= MinPrice)
                    return new Result<bool>("", true, true);
            }
            return new Result<bool>("", true, false);
        }

    }
}
