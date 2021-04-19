using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies
{
    public interface IPolicyManager
    {
    }

    public class PolicyManager : IPurchasePolicy
    {
        //TODO

        public ConcurrentDictionary<Product, LinkedList<>> MyProperty { get; set; }

        internal double GetCurrentProductPrice(Product product, int v)
        {
            throw new NotImplementedException();
        }
    }
}
