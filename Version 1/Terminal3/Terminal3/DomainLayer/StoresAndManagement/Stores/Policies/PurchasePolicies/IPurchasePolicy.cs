using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public interface IPurchasePolicy
    {
        string Id { get; }
        Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user);
        Result<bool> AddPolicy(IPurchasePolicy policy, string id);
        Result<IPurchasePolicy> RemovePolicy(string id);
        Result<IDictionary<string, object>> GetData();
        Result<bool> EditPolicy(Dictionary<string, object> info, string id);

    }
}
