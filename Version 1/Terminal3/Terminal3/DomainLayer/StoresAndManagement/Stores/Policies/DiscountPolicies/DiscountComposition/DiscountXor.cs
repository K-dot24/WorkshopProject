using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountXor : IDiscountPolicy
    {

        public IDiscountPolicy Discount1 { get; }
        public IDiscountPolicy Discount2 { get; }
        public IDiscountCondition ChoosingCondition { get; }

        public DiscountXor(IDiscountPolicy discount1, IDiscountPolicy discount2, IDiscountCondition choosingCondition)
        {
            Discount1 = discount1;
            Discount2 = discount2;
            ChoosingCondition = choosingCondition;
        }

        public Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<Dictionary<Product, double>> result1 = Discount1.CalculateDiscount(products);
            Result<Dictionary<Product, double>> result2 = Discount2.CalculateDiscount(products);

            if (result1.Data == null)
                return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());
            if (result2.Data == null)
                return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());

            if (result1.Data.Count == 0)
                return result2;
            if (result2.Data.Count == 0)
                return result1;

            Result<bool> conditionResult = ChoosingCondition.isConditionMet(products);
            if (conditionResult.ExecStatus && conditionResult.Data)
                return result1;
            return result2;
        }
    }
}
