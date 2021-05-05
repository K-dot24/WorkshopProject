using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    class AndPolicy : IPurchasePolicy
    {
        public IPurchasePolicy Cond1 { get; }
        public IPurchasePolicy Cond2 { get; }

        public AndPolicy(IPurchasePolicy cond1, IPurchasePolicy cond2)
        {
            this.Cond1 = cond1;
            this.Cond2 = cond2;
        }
        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            return new Result<bool>("", true, this.Cond1.IsConditionMet(bag, user).Data && this.Cond2.IsConditionMet(bag, user).Data);
        }
    }
}
