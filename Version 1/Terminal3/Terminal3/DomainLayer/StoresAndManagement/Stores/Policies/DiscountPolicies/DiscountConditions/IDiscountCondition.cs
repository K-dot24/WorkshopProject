using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountCondition
    {

        String Id { get; }

        Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products);
        Result<bool> AddCondition(String id, IDiscountCondition condition);
        Result<bool> RemoveCondition(String id);

    }
}
