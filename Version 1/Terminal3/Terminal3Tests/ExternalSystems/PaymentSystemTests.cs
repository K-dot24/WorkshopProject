using Xunit;
using Terminal3.ExternalSystems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems.Tests
{
    public class PaymentSystemTests
    {
        [Theory()]
        [InlineData(false)]
        [InlineData(true)]

        public void PayTest(bool isEmpty)
        {
            bool result;
            if(isEmpty)
                result = PaymentSystem.Pay(500.0 , new Dictionary<String, Object>());
            else
                result = PaymentSystem.Pay(500.0, new Dictionary<String, Object>() { { "number", new Object() } });
            Assert.Equal(!isEmpty, result);
        }
    }
}