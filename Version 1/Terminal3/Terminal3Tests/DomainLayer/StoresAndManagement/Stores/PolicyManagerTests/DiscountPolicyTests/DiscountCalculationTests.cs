using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class DiscountCalculationTests
    {

        public PolicyManager PolicyManager { get; }

        public Dictionary<String, Product> Products { get; }

        public DiscountCalculationTests()
        {
            PolicyManager = new PolicyManager();
            Products = new Dictionary<string, Product>();
            Products.Add("Bread", new Product("Bread", 10, 100, "Bakery", new LinkedList<string>()));
            Products.Add("Milk", new Product("Milk", 15, 100, "Dairy", new LinkedList<string>()));
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("Bread", 5, 40)]
        [InlineData("Bread", 1, 10)]
        [InlineData("Milk", 5, 75)]
        public void CalculatePriceMinProduct(String productname, int sum, Double expectedPrice)
        {
            
        }

    }
}
