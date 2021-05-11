using Xunit;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Tests
{
    public class DiscountCalculationTests
    {

        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public DiscountCalculationTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 15, 100, "Dairy", new LinkedList<string>()));
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 5, 40)]
        [InlineData("Milk", 5, 75)]
        public void CalculatePrice(String productName, int sum, Double expectedPrice)
        {
            DiscountTargetProducts target = new DiscountTargetProducts(new List<Product>() { Products[productName] });
            PolicyManager.AddDiscountPolicy(new VisibleDiscount(DateTime.MaxValue, target, 0.2));
            ConcurrentDictionary<Product, int> bag = new ConcurrentDictionary<Product, int>();
            bag.TryAdd(Products[productName], sum);
            Double price =PolicyManager.GetTotalBagPrice(bag);
            Assert.True(price == expectedPrice);
        }

    }
}
