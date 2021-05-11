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

        [Fact()]
        [Trait("Category", "Unit")]      
        public void AddPolicyTest()
        {
            IPurchasePolicy policy = new MaxProductPolicy(Products["Bread"], 10);            
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policy);
            Assert.Equal(PolicyManager.MainPolicy.Policy.Policies[0], policy);
        }

        [Fact()]
        [Trait("Category", "Unit")]        
        public void AddPolicyToTest()
        {
            IPurchasePolicy policy = new MinProductPolicy(Products["Bread"], 10);
            AndPolicy andPolicy = new AndPolicy();
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(andPolicy);
            PolicyManager.AddPurchasePolicy(policy, andPolicy.Id);
            Assert.Equal(andPolicy.Policies[0], policy);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddPolicyToNonexistantTest()
        {
            IPurchasePolicy policy = new MinProductPolicy(Products["Bread"], 10);
            AndPolicy andPolicy = new AndPolicy();
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            Result<bool> res =  PolicyManager.AddPurchasePolicy(policy, andPolicy.Id);
            Assert.False(res.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void AddPolicyToIllegalPolicyTest()
        {
            IPurchasePolicy policy1 = new MinProductPolicy(Products["Bread"], 5);
            IPurchasePolicy policy2 = new MaxProductPolicy(Products["Bread"], 10);
            Assert.Empty(PolicyManager.MainPolicy.Policy.Policies);
            PolicyManager.AddPurchasePolicy(policy1);
            Result<bool> res = PolicyManager.AddPurchasePolicy(policy2, policy1.Id);
            Assert.False(res.ExecStatus);
        }        
    }
}
