using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class ProductTests
    {
        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData(0.0, false,null)]
        [InlineData(1.0, true,1)]
        [InlineData(5.0, true,5)]
        [InlineData(5.1, false,null)]
        public void AddRatingTestCorrectnes(Double rate, Boolean expectedResult, Double? expectedRating)
        {
            Product product = new Product("Banana", 19.9, 10, "Fruit");
            Result<Double> result = product.AddRating(rate);
            Assert.Equal(expectedResult, result.ExecStatus);
            if (expectedResult)
            {
                Assert.Equal(product.Rating, expectedRating);
            }
        }
        [Fact()]
        [Trait("Category", "Unit")]
        public void AddRatingTestValue()
        {   
            Product product = new Product("Banana", 19.9, 10, "Fruit");
            for(int i = 1; i < 6; i++)
            {
                product.AddRating(i);
                // Returns the sum
                // for i from n (inclusive) to m (exclusive).
                Double sum = Enumerable.Range(1, i+1).Sum(j => j);
                Double r = sum / i;
                Assert.Equal(r, product.Rating);

            }
        }
    }
}