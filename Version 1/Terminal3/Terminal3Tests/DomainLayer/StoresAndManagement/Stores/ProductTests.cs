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
        //Properties
        public Product Product { get; }
        //Constructor
        public ProductTests()
        {
            Product= new Product("Banana", 19.9, 10, "Fruit");
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData(0.0, false,null)]
        [InlineData(1.0, true,1)]
        [InlineData(5.0, true,5)]
        [InlineData(5.1, false,null)]
        public void AddRatingTestCorrectnes(Double rate, Boolean expectedResult, Double? expectedRating)
        {
            Result<Double> result = Product.AddRating(rate);
            Assert.Equal(expectedResult, result.ExecStatus);
            if (expectedResult)
            {
                Assert.Equal(Product.Rating, expectedRating);
            }
        }
        [Fact()]
        [Trait("Category", "Unit")]
        public void AddRatingTestValue()
        {   
            double expectedRate = 0;
            for(int i = 1; i < 6; i++)
            {
                Product.AddRating(i);
                expectedRate = (expectedRate + i) / (double)i;
                Assert.Equal(expectedRate, Product.Rating);

            }
        }
    }
}