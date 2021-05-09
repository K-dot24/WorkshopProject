using Xunit;
using System;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System.Text;
using System.Collections.Concurrent;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Tests
{
    public class PolicyConditionTests
    {
        public PolicyManager PolicyManager { get; }

        public RegisteredUser Customer { get; }

        public Store Store { get; }

        public ConcurrentDictionary<String, Product> Products { get; }

        public PolicyConditionTests()
        {
            PolicyManager = new PolicyManager();
            Products = new ConcurrentDictionary<string, Product>();            
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 5, 40)]
        [InlineData("Milk", 5, 75)]
        public void CalculatePrice(String productName, int sum, Double expectedPrice)
        {
            PolicyManager.AddPurchasePolicy(new MaxProductPolicy(Products["Bread"], 5));
        }

    }
}
