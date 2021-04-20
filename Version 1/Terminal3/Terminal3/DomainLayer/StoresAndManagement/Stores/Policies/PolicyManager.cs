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

    public enum Purchases : int
    {        
        BuyNow = 0,
        Offer = 1,
        Auction = 2,
        Lottery = 3
    }

    public enum Discounts : int
    {
        Visible = 0,
        Conditional = 1,
        Discreet = 2
    }

    public class PolicyManager : IPolicyManager
    {
        //TODO
        public bool[] DiscountPolicies { get; }
        public bool[] PurchasePolicies { get; }
        /*public ConcurrentDictionary<Product, IPurchasePolicy> ProductsPurchasePolicies { get; }
        public ConcurrentDictionary<Product, IDiscountPolicy> ProductsDiscountPolicies { get; }*/

        public PolicyManager()
        {
            DiscountPolicies = new bool[3];
            PurchasePolicies = new bool[4];
            /*ProductsPurchasePolicies = new ConcurrentDictionary<Product, IPurchasePolicy>();
            ProductsDiscountPolicies = new ConcurrentDictionary<Product, IDiscountPolicy>();*/
        }

        public Result<Boolean> SetDiscountPolicy(Discounts policy, Boolean active)
        {
            DiscountPolicies[(int)policy] = active;
            return new Result<bool>($"Discount policy was set to {active}.\n", true, true);
        }

        public Result<Boolean> SetPurchasePolicy(Purchases policy, Boolean active)
        {
            PurchasePolicies[(int)policy] = active;
            return new Result<bool>($"Purchase policy was set to {active}.\n", true, true);
        }

/*        public Result<Boolean> AddNewPurchasePolicyToProduct(Product product, IPurchasePolicy policy)
        {
            throw new NotImplementedException();
        }*/

        public double GetCurrentProductPrice(Product product, int quantity)
        {
            //TODO
            //throw new NotImplementedException();
            return product.Price*quantity;
        }
    }
}
