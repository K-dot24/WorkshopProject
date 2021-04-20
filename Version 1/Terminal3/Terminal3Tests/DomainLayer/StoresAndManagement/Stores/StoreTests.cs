using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class StoreTests
    {
        //Properties
        public Store Store { get; }

        //Constructor
        public StoreTests()
        {
            Store = new Store("TestStore", new Users.RegisteredUser("test@store", "password"));
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData(0.0, false, null)]
        [InlineData(1.0, true, 1)]
        [InlineData(5.0, true, 5)]
        [InlineData(5.1, false, null)]
        public void AddRatingTestCorrectnes(Double rate, Boolean expectedResult, Double? expectedRating)
        {
            Result<Double> result = Store.AddRating(rate);
            Assert.Equal(expectedResult, result.ExecStatus);
            if (expectedResult)
            {
                Assert.Equal(Store.Rating, expectedRating);
            }
        }
        [Fact()]
        [Trait("Category", "Unit")]
        public void AddRatingTestValue()
        {
            double expectedRate = 0;
            for (int i = 1; i < 6; i++)
            {
                Store.AddRating(i);
                expectedRate = (expectedRate * (i - 1) + i) / (double)i;
                Assert.Equal(expectedRate, Store.Rating);

            }
        }

     }
}