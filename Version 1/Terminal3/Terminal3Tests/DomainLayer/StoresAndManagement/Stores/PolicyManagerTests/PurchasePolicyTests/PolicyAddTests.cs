using Xunit;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Tests
{
    public class PolicyAddTests
    {
        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public ConcurrentDictionary<Product, int> currProducts { get; }

        public PolicyAddTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            currProducts = new ConcurrentDictionary<Product, int>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 15, 100, "Dairy", new LinkedList<string>()));            
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 11)]
        [InlineData("Bread", 6)]
        [InlineData("Milk", 50)]
        [InlineData("Milk", 1)]
        public void AddMaxProductPolicyTest(String productName, int count)
        {
            IPurchasePolicy policy = new MaxProductPolicy(Products[productName], 10);
            currProducts.TryAdd(Products[productName], count);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policy);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policy);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 11)]
        [InlineData("Bread", 6)]
        [InlineData("Milk", 50)]
        [InlineData("Milk", 1)]
        public void AddMinProductPolicyTest(String productName, int count)
        {
            IPurchasePolicy policy = new MinProductPolicy(Products[productName], 10);
            currProducts.TryAdd(Products[productName], count);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policy);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policy);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 11)]
        [InlineData("Bread", 6)]
        [InlineData("Milk", 50)]
        [InlineData("Milk", 8)]
        public void AddAndPolicyTest(String productName, int count)
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products[productName], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products[productName], 10);
            List<IPurchasePolicy> policies = new List<IPurchasePolicy>();
            policies.Add(policy1);
            policies.Add(policy2);
            IPurchasePolicy policyAnd = new AndPolicy(policies);
            currProducts.TryAdd(Products[productName], count);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policyAnd);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policyAnd);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, "Milk", 8)]
        [InlineData("Bread", 4, "Milk", 1)]
        [InlineData("Bread", 11, "Milk", 20)]
        [InlineData("Bread", 4, "Milk", 11)]
        public void AddOrPolicyTest(String productName1, int count1, String productName2, int count2)
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products[productName1], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products[productName2], 10);
            List<IPurchasePolicy> policies = new List<IPurchasePolicy>();
            policies.Add(policy1);
            policies.Add(policy2);
            IPurchasePolicy policyOr = new OrPolicy(policies);
            currProducts.TryAdd(Products[productName1], count1);
            currProducts.TryAdd(Products[productName2], count2);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policyOr);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policyOr);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, "Milk", 8)]
        [InlineData("Bread", 4, "Milk", 11)]
        [InlineData("Bread", 11, "Milk", 20)]
        [InlineData("Bread", 3, "Milk", 10)]
        public void AddConditionalPolicyTest(String productName1, int count1, String productName2, int count2)
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products[productName1], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products[productName2], 10);
            IPurchasePolicy policyCond = new ConditionalPolicy(policy1, policy2);
            currProducts.TryAdd(Products[productName1], count1);
            currProducts.TryAdd(Products[productName2], count2);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policyCond);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policyCond);
        }
    }
}
