using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountTarget
    {

        List<Product> getTargets(ConcurrentDictionary<Product, int> products);

        Result<IDictionary<string, object>> GetData();

        String getId();

    }
}
