

using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public interface IPurchasePolicy
    {
        //TODO: Complete properly

        Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user);
    }
}
