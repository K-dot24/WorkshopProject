using Xunit;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Tests
{
    public class PolicyRemoveTests
    {
        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public ConcurrentDictionary<Product, int> currProducts { get; }

        public PolicyRemoveTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            currProducts = new ConcurrentDictionary<Product, int>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 15, 100, "Dairy", new LinkedList<string>()));
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6)]
        [InlineData("Bread", 4)]
        [InlineData("Milk", 50)]
        [InlineData("Milk", 1)]
        public void RemoveMaxProductPolicyTest(String productName, int count)
        {
            IPurchasePolicy policy = new MaxProductPolicy(Products[productName], 10);
            currProducts.TryAdd(Products[productName], count);
            PolicyManager.AddPurchasePolicy(policy);
            PolicyManager.RemovePurchasePolicy(policy.Id);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);

        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6)]
        [InlineData("Bread", 4)]
        [InlineData("Milk", 50)]
        [InlineData("Milk", 1)]
        public void RemoveMinProductPolicyTest(String productName, int count)
        {
            IPurchasePolicy policy = new MinProductPolicy(Products[productName], 10);
            currProducts.TryAdd(Products[productName], count);
            PolicyManager.AddPurchasePolicy(policy);
            PolicyManager.RemovePurchasePolicy(policy.Id);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);

        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 11)]
        [InlineData("Bread", 6)]
        [InlineData("Milk", 50)]
        [InlineData("Milk", 8)]
        public void RemoveAndPolicyTest(String productName, int count)
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products[productName], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products[productName], 10);
            List<IPurchasePolicy> policies = new List<IPurchasePolicy>();
            policies.Add(policy1);
            policies.Add(policy2);
            IPurchasePolicy policyAnd = new AndPolicy(policies);
            currProducts.TryAdd(Products[productName], count);
            PolicyManager.AddPurchasePolicy(policyAnd);
            PolicyManager.RemovePurchasePolicy(policyAnd.Id);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, "Milk", 8)]
        [InlineData("Bread", 4, "Milk", 1)]
        [InlineData("Bread", 11, "Milk", 20)]
        [InlineData("Bread", 4, "Milk", 11)]
        public void RemoveOrPolicyTest(String productName1, int count1, String productName2, int count2)
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products[productName1], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products[productName2], 10);
            List<IPurchasePolicy> policies = new List<IPurchasePolicy>();
            policies.Add(policy1);
            policies.Add(policy2);
            IPurchasePolicy policyOr = new OrPolicy(policies);
            currProducts.TryAdd(Products[productName1], count1);
            currProducts.TryAdd(Products[productName2], count2);
            PolicyManager.AddPurchasePolicy(policyOr);
            PolicyManager.RemovePurchasePolicy(policyOr.Id);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, "Milk", 8)]
        [InlineData("Bread", 4, "Milk", 11)]
        [InlineData("Bread", 11, "Milk", 20)]
        [InlineData("Bread", 3, "Milk", 10)]
        public void RemoveConditionalPolicyTest(String productName1, int count1, String productName2, int count2)
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products[productName1], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products[productName2], 10);
            IPurchasePolicy policyCond = new ConditionalPolicy(policy1, policy2);
            currProducts.TryAdd(Products[productName1], count1);
            currProducts.TryAdd(Products[productName2], count2);
            PolicyManager.AddPurchasePolicy(policyCond);
            PolicyManager.RemovePurchasePolicy(policyCond.Id);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
        }
    }
}
