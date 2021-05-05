using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    class ConditionalPolicy : IPurchasePolicy
    {
        public IPurchasePolicy PreCond { get; }
        public IPurchasePolicy Cond { get; }

        public ConditionalPolicy(IPurchasePolicy preCond, IPurchasePolicy cond)
        {
            this.PreCond = preCond;
            this.Cond = cond;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            if (this.PreCond.IsConditionMet(bag, user).Data)
                if (!this.Cond.IsConditionMet(bag, user).Data)                
                    return new Result<bool>("", true, false);
            return new Result<bool>("", true, true);
        }
    }
}
