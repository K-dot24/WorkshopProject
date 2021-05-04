using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountOr : IDiscountPolicy
    {

        public IDiscountPolicy Discount1 { get; }
        public IDiscountPolicy Discount2 { get; }

        public DiscountOr(IDiscountPolicy discount1, IDiscountPolicy discount2)
        {
            Discount1 = discount1;
            Discount2 = discount2;
        }

        public Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<Dictionary<Product, double>> result1 = Discount1.CalculateDiscount(products);
            Result<Dictionary<Product, double>> result2 = Discount2.CalculateDiscount(products);

            if (result1.Data == null)
                result1.Data = new Dictionary<Product, double>();
            if (result2.Data == null)
                result2.Data = new Dictionary<Product, double>();

            return combineOrDiscounts(result1.Data, result2.Data);
        }

        private Result<Dictionary<Product, double>> combineOrDiscounts(Dictionary<Product, double> discounts1, Dictionary<Product, double> discounts2)
        {
            foreach (KeyValuePair<Product, double> entry in discounts2)
            {
                if (!discounts1.ContainsKey(entry.Key))
                    discounts1[entry.Key] = entry.Value;
                else if (discounts1[entry.Key] < entry.Value)
                    discounts1[entry.Key] = entry.Value;
            }
            return new Result<Dictionary<Product, double>>("", true, discounts1);
        }
    }
}
