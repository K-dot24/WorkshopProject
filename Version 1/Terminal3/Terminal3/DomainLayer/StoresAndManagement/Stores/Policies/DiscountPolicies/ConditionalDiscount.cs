using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class ConditionalDiscount : AbstractDiscountPolicy
    {

        public IDiscountCondition Condition { get; }
        public IDiscountPolicy Discount { get; }

        public ConditionalDiscount(IDiscountPolicy discount, IDiscountCondition condition, String id = "") : base(id)
        {
            Condition = condition;
            Discount = discount;
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<bool> isEligible = Condition.isConditionMet(products);
            if (isEligible.ExecStatus && isEligible.Data)
            {
                return Discount.CalculateDiscount(products);
            }
            return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            return Discount.AddDiscount(id, discount);
        }

        public override Result<bool> RemoveDiscount(String id)
        {
            return Discount.RemoveDiscount(id);
        }
    }
}
