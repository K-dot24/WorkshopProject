using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountCondition
    {

        Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products);

    }
}
