using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies
{
    public interface IPolicyManager
    {
        double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "");
        Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user);
    }    

    public class PolicyManager : IPolicyManager
    {

        //TODO: Complete properly


        public DiscountAddition Discounts { get; }
        public BuyNow Policies { get; set; }
        public PolicyManager()
        {
            Policies = new BuyNow();
            Discounts = new DiscountAddition();
        }

        public Result<Boolean> AddDiscountPolicy(IDiscountPolicy discount)
        {
            Discounts.AddDiscount(discount);
            return new Result<bool>($"Discount policy was succesfully added.\n", true, true);
        }        

        public Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy, string id)
        {
            Policies.AddPolicy(policy, id);
            return new Result<bool>($"Purchase policy was successfully added.\n", true, true);
        }

        public double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "")
        {
            Result<Dictionary<Product, Double>> discountsResult = Discounts.CalculateDiscount(products, discountCode);
            if (!discountsResult.ExecStatus)
                return -1;

            Dictionary<Product, Double> discounts = discountsResult.Data;
            Double price = 0;
            foreach (KeyValuePair<Product, int> entry in products)
            {
                if (discounts.ContainsKey(entry.Key))
                    price += entry.Key.Price * entry.Value * (100 - discounts[entry.Key]) / 100;
                else
                    price += entry.Key.Price * entry.Value;
            }

            return price;
        }
        public Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user)
        {
            return Policies.IsConditionMet(products, user);
        }
    }
}
