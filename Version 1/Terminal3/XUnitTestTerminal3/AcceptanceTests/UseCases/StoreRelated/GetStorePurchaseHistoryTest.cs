using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class GetStorePurchaseHistoryTest: XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        private string product_id;
        public GetStorePurchaseHistoryTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
            this.product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
        }
        [Fact()]
        [Trait("Category", "acceptance")]
        public void EmptyHistoryTest()
        {
            List<string> result =  sut.GetStorePurchaseHistory(user_id,store_id).Data;
            Assert.Empty(result);
        }
        [Fact()]
        [Trait("Category", "acceptance")]
        public void NonEmptyHistoryTest()
        {
            IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>
                    {
                     { "card_number", "2222333344445555" },
                     { "month", "4" },
                     { "year", "2021" },
                     { "holder", "Israel Israelovice" },
                     { "ccv", "262" },
                     { "id", "20444444" }
                    };
            IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>
                    {
                     { "name", "Israel Israelovice" },
                     { "address", "Rager Blvd 12" },
                     { "city", "Beer Sheva" },
                     { "country", "Israel" },
                     { "zip", "8458527" }
                    };

            sut.AddProductToCart(user_id, product_id, 2, store_id);
            Assert.True(sut.Purchase(user_id, paymentDetails, deliveryDetails).ExecStatus);
            List<string> result = sut.GetStorePurchaseHistory(user_id, store_id).Data;
            Assert.NotEmpty(result);
        }
    }
}
