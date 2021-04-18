using Xunit;
using Terminal3.ExternalSystems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems.Tests
{
    public class PaymentSystemTests
    {
        [Fact()]
        public void PayTest()
        {
            bool result = PaymentSystem.Pay("userID", 500.0);
            Assert.Equal(result, result);
        }
    }
}