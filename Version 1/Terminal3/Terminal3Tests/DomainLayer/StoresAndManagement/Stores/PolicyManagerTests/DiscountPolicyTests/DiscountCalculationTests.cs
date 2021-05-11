using Xunit;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountComposition;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Tests
{
    public class DiscountCalculationTests
    {

        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public ConcurrentDictionary<Product, int> currProducts { get; }

        public DiscountCalculationTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 20, 100, "Dairy", new LinkedList<string>()));
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 5, 40)]
        [InlineData("Milk", 5, 100)]
        public void CalculatePrice(String productName, int sum, Double expectedPrice)
        {
            DiscountTargetProducts target = new DiscountTargetProducts(new List<Product>() { Products["Bread"] });
            PolicyManager.AddDiscountPolicy(new VisibleDiscount(DateTime.MaxValue, target, 20));
            currProducts.TryAdd(Products[productName], sum);
            Double price =PolicyManager.GetTotalBagPrice(currProducts);
            
            Assert.True(price == expectedPrice);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 20, 200)]
        [InlineData("Bread", 5, 40)]
        [InlineData("Milk", 20, 400)]
        [InlineData("Milk", 5, 100)]
        public void MaxProductPolicyTest(String productName, int count, Double expectedPrice)
        {
            DiscountTargetProducts target = new DiscountTargetProducts(new List<Product>() { Products["Bread"] });
            PolicyManager.AddDiscountPolicy(new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), new MaxProductCondition(Products["Bread"], 10)));
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedPrice);
        }

    }
}
