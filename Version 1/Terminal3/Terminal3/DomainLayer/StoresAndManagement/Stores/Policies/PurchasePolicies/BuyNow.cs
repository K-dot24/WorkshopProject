using System;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class BuyNow : IPurchasePolicy
    {

        public IPurchasePolicy Policy { get; }

        public BuyNow( IPurchasePolicy policy)
        {
            this.Policy = policy;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            if(this.Policy.IsConditionMet(bag, user).Data)
                return new Result<bool>("",true,true);
            return new Result<bool>("Policy conditions not met", true, false); 
        }
    }
}
