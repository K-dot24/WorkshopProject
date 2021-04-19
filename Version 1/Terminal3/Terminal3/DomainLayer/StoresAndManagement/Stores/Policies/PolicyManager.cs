using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies
{
    public interface IPolicyManager
    {
    }

    public class PolicyManager : IPolicyManager
    {
        //TODO
        public LinkedList<IDiscountPolicy> DiscountPolicies { get; }
        public LinkedList<IPurchasePolicy> PurchasePolicies { get; }
        public ConcurrentDictionary<Product, Tuple<LinkedList<IPurchasePolicy>, LinkedList<IDiscountPolicy>>> ProductsPolicies { get; }

        public PolicyManager()
        {
            DiscountPolicies = new LinkedList<IDiscountPolicy>();
            PurchasePolicies = new LinkedList<IPurchasePolicy>();

        }

        public double GetCurrentProductPrice(Product product, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
