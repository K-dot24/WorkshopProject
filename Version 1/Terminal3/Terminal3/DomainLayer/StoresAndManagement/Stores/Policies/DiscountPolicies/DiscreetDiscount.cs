using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscreetDiscount : IDiscountPolicy
    {
        //TODO: Complete properly

        public String DiscountCode { get; }
        public IDiscountPolicy Discount { get; }

        public DiscreetDiscount(IDiscountPolicy discount, String discountCode)
        {
            Discount = discount;
            DiscountCode = discountCode;
        }

        public Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            if (DiscountCode.Equals(code))
                return Discount.CalculateDiscount(products, code);
            return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());
        }
    }
}
