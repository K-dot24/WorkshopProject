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

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemovePolicyTest()
        {
            IPurchasePolicy policy = new MaxProductPolicy(Products["Bread"], 10);
            PolicyManager.AddPurchasePolicy(policy);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policy);
            PolicyManager.RemovePurchasePolicy(policy.Id);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemovePolicyToTest()
        {
            IPurchasePolicy policy = new MinProductPolicy(Products["Bread"], 10);
            AndPolicy andPolicy = new AndPolicy();
            PolicyManager.AddPurchasePolicy(andPolicy);
            PolicyManager.AddPurchasePolicy(policy, andPolicy.Id);
            Result<bool> res = PolicyManager.RemovePurchasePolicy(policy.Id);
            Assert.Empty(andPolicy.Policies);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveNonexistantPolicyTest()
        {
            IPurchasePolicy policy = new MinProductPolicy(Products["Bread"], 10);
            AndPolicy andPolicy = new AndPolicy();
            PolicyManager.AddPurchasePolicy(andPolicy);
            Result<bool> res = PolicyManager.RemovePurchasePolicy(policy.Id);
            Assert.False(res.Data);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveIllegalPolicyTest()
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products["Bread"], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products["Bread"], 10);
            IPurchasePolicy condPolicy = new ConditionalPolicy(policy1, policy2);            
            PolicyManager.AddPurchasePolicy(condPolicy);
            Result<bool> res = PolicyManager.RemovePurchasePolicy(policy1.Id);

            Assert.False(res.ExecStatus);
        }
    }
}
