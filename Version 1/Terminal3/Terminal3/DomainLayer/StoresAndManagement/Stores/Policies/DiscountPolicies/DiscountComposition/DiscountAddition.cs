using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountAddition : IDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountAddition()
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public DiscountAddition(List<IDiscountPolicy> discounts)
        {
            Discounts = discounts;
            if (Discounts == null)
                Discounts = new List<IDiscountPolicy>();
        }

        public void AddDiscount(IDiscountPolicy discount)
        {
            Discounts.Add(discount);
        }

        public Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Dictionary<Product, Double> result = new Dictionary<Product, Double>();

            foreach(IDiscountPolicy discountPolicy in Discounts)
            {
                Dictionary<Product, Double> discountResult = discountPolicy.CalculateDiscount(products).Data;
                if (discountResult == null)
                    continue;
                foreach(KeyValuePair<Product, Double> entry in discountResult)
                {
                    if (result.ContainsKey(entry.Key))
                        result[entry.Key] = result[entry.Key] + entry.Value;
                    else
                        result[entry.Key] = entry.Value;
                }
            }

            return new Result<Dictionary<Product, Double>>("", true, result);
        }
    }
}
