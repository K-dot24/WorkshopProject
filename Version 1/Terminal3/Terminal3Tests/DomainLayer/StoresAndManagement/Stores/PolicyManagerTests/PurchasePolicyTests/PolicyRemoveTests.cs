using Xunit;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Tests
{
    public class PolicyRemoveTests
    {
        public PolicyManager PolicyManager { get; }

        public PolicyRemoveTests()
        {
            PolicyManager = new PolicyManager();  
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 6, false)]
        [InlineData("Bread", 4, true)]
        [InlineData("Milk", 50, false)]
        [InlineData("Milk", 1, true)]
        public void MaxProductPolicyTest(String productName, int count, bool expectedResult)
        {
            
        }        
    }
}
