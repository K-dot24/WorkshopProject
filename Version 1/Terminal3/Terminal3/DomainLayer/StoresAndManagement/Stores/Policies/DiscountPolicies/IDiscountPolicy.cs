using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountPolicy
    {

        Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "");

    }
}
