using Xunit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Discounts.Tests
{
    public class DiscountAddTests
    {
        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public ConcurrentDictionary<Product, int> currProducts { get; }

        public DiscountAddTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            currProducts = new ConcurrentDictionary<Product, int>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 15, 100, "Dairy", new LinkedList<string>()));
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddDiscountTest()
        {
            IDiscountPolicy p = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 10);
            PolicyManager.AddDiscountPolicy(p);
            Assert.Equal(PolicyManager.DiscountRoot.Discounts[0], p);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddDiscountToTest()
        {
            DiscountOr p1 = new DiscountOr();
            PolicyManager.AddDiscountPolicy(p1);
            IDiscountPolicy p2 = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 10);
            PolicyManager.AddDiscountPolicy(p2, p1.Id);
            Assert.Equal(p1.Discounts[0], p2);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddDiscountToNonExistantTest()
        {
            IDiscountPolicy p = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 10);
            Result<bool> result = PolicyManager.AddDiscountPolicy(p, "Non existant Id");
            Assert.False(result.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddDiscountToIllegalDiscountTest()
        {
            IDiscountPolicy p1 = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 20);
            DiscountXor p2 = new DiscountXor(p1, p1, new MinBagPriceCondition(0));
            PolicyManager.AddDiscountPolicy(p2);
            IDiscountPolicy p3 = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 10);
            Result<bool> result = PolicyManager.AddDiscountPolicy(p3, p2.Id);
            Assert.False(result.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddConditionToTest()
        {
            DiscountConditionOr c = new DiscountConditionOr();
            ConditionalDiscount p = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 20), c);
            PolicyManager.AddDiscountPolicy(p);
            MinBagPriceCondition c1 = new MinBagPriceCondition(0);
            PolicyManager.AddDiscountCondition(c1, c.Id);
            Assert.Equal(c.Conditions[0], c1);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddConditionToNonExistantTest()
        {
            IDiscountCondition p = new MinBagPriceCondition(0);
            Result<bool> result = PolicyManager.AddDiscountCondition(p, "Non existant Id");
            Assert.False(result.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddConditionToIllegalConditionTest()
        {
            IDiscountCondition c = new MinBagPriceCondition(0);
            IDiscountPolicy p = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 20), c);
            PolicyManager.AddDiscountPolicy(p);
            Result<bool> result = PolicyManager.AddDiscountCondition(c, c.Id);
            Assert.False(result.ExecStatus);
        }

    }
}
