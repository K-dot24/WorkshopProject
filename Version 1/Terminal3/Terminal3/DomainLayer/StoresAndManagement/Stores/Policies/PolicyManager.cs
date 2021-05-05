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
        private const int PURCHASE_SIZE = 4;
        private const int DISCOUNT_SIZE = 3;

        //TODO: Complete properly

        public bool[] DiscountPolicies { get; }
        public bool[] PurchasePolicies { get; }

        public DiscountAddition Discounts { get; }

        public PolicyManager()
        {
            DiscountPolicies = new bool[DISCOUNT_SIZE];
            PurchasePolicies = new bool[PURCHASE_SIZE];
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


        public double GetCurrentProductPrice(Product product, int quantity)
        {
            //throw new NotImplementedException();
            return product.Price*quantity;
        }

        public Result<Double> CalculatebagPrice(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<Dictionary<Product, Double>> discountsResult = Discounts.CalculateDiscount(products, code);
            if (!discountsResult.ExecStatus)
                return new Result<Double>("Failed to calculate the bag price", false, 0);

            Dictionary<Product, Double> discounts = discountsResult.Data;
            Double price = 0;
            foreach(KeyValuePair<Product, int> entry in products)
            {
                if (discounts.ContainsKey(entry.Key))
                    price += entry.Key.Price * entry.Value * (100 - discounts[entry.Key]) / 100;
                else
                    price += entry.Key.Price * entry.Value;
            }

            return new Result<double>("", true, price);
        }
    }
}
