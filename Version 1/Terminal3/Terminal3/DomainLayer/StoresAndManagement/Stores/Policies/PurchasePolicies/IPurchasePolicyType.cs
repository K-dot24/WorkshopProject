using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public interface IPurchasePolicyType
    {
        string Id { get; }
        Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user);
        Result<bool> AddPolicy(IPurchasePolicy policy, string id);
        Result<bool> RemovePolicy(string id);
        Result<IPurchasePolicyData> GetData();
        Result<bool> EditPolicy(Dictionary<string, object> info, string id);

    }
}
