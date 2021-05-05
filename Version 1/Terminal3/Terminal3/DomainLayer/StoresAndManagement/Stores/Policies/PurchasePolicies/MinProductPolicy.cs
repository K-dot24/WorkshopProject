using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    class MinProductPolicy : IPurchasePolicy
    {
        public Product Product { get; }
        public int Min { get; }
        public MinProductPolicy (Product product, int min)
        {
            this.Product = product;
            this.Min = min;
        }
        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            int count;
            bag.TryGetValue(this.Product, out count);
            return new Result<bool>("", true, bag.ContainsKey(this.Product) && count >= this.Min);
        }
    }
}
