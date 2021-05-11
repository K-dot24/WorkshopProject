using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MinBagPriceCondition : AbstractDiscountCondition
    {

        public Double MinPrice { get; }

        public MinBagPriceCondition(Double minPrice, String id = "") : base(id)
        {
            MinPrice = minPrice;
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
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

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountConditionData> GetData()
        {
            return new Result<IDiscountConditionData>("", true, new MinBagPriceConditionData(MinPrice, Id));
        }
    }
}
