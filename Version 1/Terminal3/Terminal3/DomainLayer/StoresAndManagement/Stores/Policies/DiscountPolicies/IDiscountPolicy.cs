using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountPolicy
    {

        String Id { get; }

        Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "");

        Result<bool> AddDiscount(String id, IDiscountPolicy discount);

        Result<bool> AddCondition(String id, IDiscountCondition condition);

        Result<IDiscountPolicy> RemoveDiscount(String id);

        Result<IDiscountCondition> RemoveCondition(String id);

        Result<bool> EditDiscount(Dictionary<string, object> info, string id);

        Result<bool> EditCondition(Dictionary<string, object> info, string id);

        Result<IDiscountPolicyData> GetData();

    }
}
