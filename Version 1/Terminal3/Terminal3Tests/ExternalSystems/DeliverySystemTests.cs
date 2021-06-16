using Xunit;
using System;
using System.Collections.Generic;

namespace Terminal3.ExternalSystems.Tests
{
    public class DeliverySystemTests
    {
        public IDictionary<String, Object> deliveryDetails;
        public bool isMock { get; }
        public DeliverySystemTests()
        {
            isMock = true;
            if(isMock)
                ExternalSystemsAPI.getInstance("");
            else
                ExternalSystemsAPI.getInstance("https://cs-bgu-wsep.herokuapp.com/");
            deliveryDetails = new Dictionary<String, Object>
                    {
                     { "name", "Israel Israelovice" },
                     { "address", "Rager Blvd 12" },
                     { "city", "Beer Sheva" },
                     { "country", "Israel" },
                     { "zip", "8458527" }
                    };
        }
        [Theory()]
        [InlineData(false)]
        [InlineData(true)]

        public void SupplyTest(bool isEmpty)
        {
            int result;
            if (isEmpty)
                result = DeliverySystem.Supply( new Dictionary<String, Object>());
            else                          
                result = DeliverySystem.Supply(deliveryDetails);

            if (isEmpty)
                Assert.Equal(-1, result);
            else
                Assert.InRange(result, 10000, 100000);
        }

        [Theory()]
        [InlineData(false)]
        [InlineData(true)]

        public void CancelSupplyTest(bool isEmpty)
        {
            int supplyId = DeliverySystem.Supply(deliveryDetails);
            int result;
            if (isEmpty)
                result = DeliverySystem.CancelSupply(new Dictionary<String, Object>());
            else
                result = DeliverySystem.CancelSupply(new Dictionary<String, Object>(){{"transaction_id", supplyId.ToString()}});
            
            if (isEmpty)
                Assert.Equal(-1, result);
            else
                Assert.True(result > 0);
        }
    }
}
