using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class ConditionalDiscount : IDiscountPolicy
    {

        public IDiscountCondition Condition { get; }
        public IDiscountPolicy Discount { get; }

        public ConditionalDiscount(IDiscountPolicy discount, IDiscountCondition condition)
        {
            Condition = condition;
            Discount = discount;
        }

        public Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<bool> isEligible = Condition.isConditionMet(products);
            if (isEligible.ExecStatus && isEligible.Data)
            {
                return Discount.CalculateDiscount(products);
            }
            return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
        }
    }
}
