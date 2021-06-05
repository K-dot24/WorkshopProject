using Xunit;
using System;
using System.Collections.Generic;

namespace Terminal3.ExternalSystems.Tests
{
    public class PaymentSystemTests
    {

        public IDictionary<String, Object> paymentDetails;
        public PaymentSystemTests()
        {
            ExternalSystemsAPI.getInstance("https://cs-bgu-wsep.herokuapp.com/");
            paymentDetails = new Dictionary<String, Object>
                    {
                     { "card_number", "2222333344445555" },
                     { "month", "4" },
                     { "year", "2021" },
                     { "holder", "Israel Israelovice" },
                     { "ccv", "262" },
                     { "id", "20444444" }
                    };
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]

        public void PayTest(bool isEmpty)
        {
            int result;

            if (isEmpty)
                result = PaymentSystem.Pay(500.0, new Dictionary<String, Object>());
            else
                result = PaymentSystem.Pay(500.0, paymentDetails);

            if(isEmpty)
                Assert.Equal(-1, result);
            else
                Assert.InRange(result, 10000, 100000);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]

        public void CancelPayTest(bool isEmpty)
        {
            int purchaseId = PaymentSystem.Pay(500.0, paymentDetails);
            int result;
            if (isEmpty)
                result = PaymentSystem.CancelPay(new Dictionary<String, Object>());
            else
                result = PaymentSystem.CancelPay(new Dictionary<String, Object>() { { "transaction_id", purchaseId.ToString() } });

            if (isEmpty)
                Assert.Equal(-1, result);
            else
                Assert.Equal(1, result);
        }
    }
}