using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    class MinAgePolicy : IPurchasePolicy
    {
        public int Age { get; }
        public MinAgePolicy(int age)
        {
            this.Age = age;
        }
        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            return new Result<bool>("", true, true);
        }
    }
}
