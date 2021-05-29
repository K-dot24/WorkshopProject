using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Discounts.Tests
{
    public class DiscountRemoveTests
    {
        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public ConcurrentDictionary<Product, int> currProducts { get; }

        public DiscountRemoveTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            currProducts = new ConcurrentDictionary<Product, int>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 15, 100, "Dairy", new LinkedList<string>()));
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveMainDiscountTest()
        {
            DiscountOr p1 = new DiscountOr();
            PolicyManager.AddDiscountPolicy(p1);
            PolicyManager.RemoveDiscountPolicy(p1.Id);
            Assert.True(PolicyManager.DiscountRoot.Discounts.Count == 0);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveDiscountTest()
        {
            DiscountOr p1 = new DiscountOr();
            PolicyManager.AddDiscountPolicy(p1);
            IDiscountPolicy p2 = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 10);
            PolicyManager.AddDiscountPolicy(p2, p1.Id);
            PolicyManager.RemoveDiscountPolicy(p2.Id);
            Assert.True(p1.Discounts.Count == 0);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveNonExistantDiscountTest()
        {
            Result<bool> result = PolicyManager.RemoveDiscountPolicy("Non existant Id");
            Assert.False(result.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void IllegalRemoveDiscountTest()
        {
            VisibleDiscount d = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 20);
            DiscountXor xor = new DiscountXor(d, d, new MinBagPriceCondition(0));
            PolicyManager.AddDiscountPolicy(xor);
            Result<bool> result = PolicyManager.RemoveDiscountPolicy(d.Id);
            Assert.False(result.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveConditionTest()
        {
            MinBagPriceCondition c = new MinBagPriceCondition(0);
            DiscountConditionOr cor = new DiscountConditionOr(new List<IDiscountCondition>() { c });
            ConditionalDiscount cd = new ConditionalDiscount(new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 20), cor);
            PolicyManager.AddDiscountPolicy(cd);
            PolicyManager.RemoveDiscountCondition(c.Id);
            Assert.True(cor.Conditions.Count == 0);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveNonExistantConditionTest()
        {
            Result<bool> result = PolicyManager.RemoveDiscountCondition("Non existant Id");
            Assert.False(result.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void IllegalRemoveConditionTest()
        {
            VisibleDiscount vd = new VisibleDiscount(DateTime.MaxValue, new DiscountTargetShop(), 20);
            MinBagPriceCondition c = new MinBagPriceCondition(0);
            ConditionalDiscount cd = new ConditionalDiscount(vd, c);
            PolicyManager.AddDiscountPolicy(cd);
            Result<bool> result = PolicyManager.RemoveDiscountCondition(c.Id);
            Assert.False(result.ExecStatus);
        }

    }
}
