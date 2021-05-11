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
            currProducts = new ConcurrentDictionary<Product, int>();
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
        public void MaxProductDiscountTest(String productName, int count, Double expectedPrice)
        {
            DiscountTargetProducts target = new DiscountTargetProducts(new List<Product>() { Products["Bread"] });
            PolicyManager.AddDiscountPolicy(new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), new MaxProductCondition(Products["Bread"], 10)));
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedPrice);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 20, 160)]
        [InlineData("Bread", 5, 50)]
        [InlineData("Milk", 20, 400)]
        [InlineData("Milk", 5, 100)]
        public void MinProductDiscountTest(String productName, int count, Double expectedPrice)
        {
            DiscountTargetProducts target = new DiscountTargetProducts(new List<Product>() { Products["Bread"] });
            PolicyManager.AddDiscountPolicy(new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), new MinProductCondition(Products["Bread"], 10)));
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedPrice);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, 48)]
        [InlineData("Bread", 4, 40)]
        [InlineData("Bread", 11, 110)]
        [InlineData("Milk", 8, 160)]
        [InlineData("Milk", 1, 20)]
        [InlineData("Milk", 20, 400)]
        public void AndDiscountTest(String productName, int count, Double expectedResult)
        {
            IDiscountTarget target = new DiscountTargetProducts(new List<Product>() { Products[productName] });
            IDiscountCondition c1 = new MinProductCondition(Products["Bread"], 5);
            IDiscountPolicy p1 = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), c1);
            IDiscountCondition c2 = new MaxProductCondition(Products["Bread"], 10);
            IDiscountPolicy p2 = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), c2);
            PolicyManager.AddDiscountPolicy(new DiscountAnd(new List<IDiscountPolicy>() { p1, p2 }));
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedResult);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, 48)]
        [InlineData("Bread", 4, 40)]
        [InlineData("Bread", 11, 110)]
        [InlineData("Milk", 8, 160)]
        [InlineData("Milk", 1, 20)]
        [InlineData("Milk", 20, 400)]
        public void AndDiscountConditionTest(String productName, int count, Double expectedResult)
        {
            IDiscountTarget target = new DiscountTargetProducts(new List<Product>() { Products[productName] });
            IDiscountCondition c1 = new MinProductCondition(Products["Bread"], 5);
            IDiscountCondition c2 = new MaxProductCondition(Products["Bread"], 10);
            IDiscountCondition c = new DiscountConditionAnd(new List<IDiscountCondition>() { c1, c2 });
            IDiscountPolicy p = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), c);
            PolicyManager.AddDiscountPolicy(p);
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedResult);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, 60)]
        [InlineData("Bread", 4, 32)]
        [InlineData("Bread", 11, 88)]
        [InlineData("Milk", 8, 160)]
        [InlineData("Milk", 1, 20)]
        [InlineData("Milk", 20, 400)]
        public void OrDiscountTest(String productName, int count, Double expectedResult)
        {
            IDiscountTarget target = new DiscountTargetProducts(new List<Product>() { Products[productName] });
            IDiscountCondition c1 = new MinProductCondition(Products[productName], 10);
            IDiscountPolicy p1 = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), c1);
            IDiscountCondition c2 = new MaxProductCondition(Products[productName], 5);
            IDiscountPolicy p2 = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), c2);
            PolicyManager.AddDiscountPolicy(new DiscountOr(new List<IDiscountPolicy>() { p1, p2 }));
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedResult);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, 60)]
        [InlineData("Bread", 4, 32)]
        [InlineData("Bread", 11, 88)]
        [InlineData("Milk", 8, 160)]
        [InlineData("Milk", 1, 20)]
        [InlineData("Milk", 20, 400)]
        public void OrDiscountConditionTest(String productName, int count, Double expectedResult)
        {
            IDiscountTarget target = new DiscountTargetProducts(new List<Product>() { Products[productName] });
            IDiscountCondition c1 = new MinProductCondition(Products[productName], 10);
            IDiscountCondition c2 = new MaxProductCondition(Products[productName], 5);
            IDiscountCondition c = new DiscountConditionOr(new List<IDiscountCondition>() { c1, c2 });
            IDiscountPolicy p = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, target, 20), c);
            PolicyManager.AddDiscountPolicy(p);
            currProducts.TryAdd(Products[productName], count);
            Assert.Equal(PolicyManager.GetTotalBagPrice(currProducts), expectedResult);
        }

    }
}
