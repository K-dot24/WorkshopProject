using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Xunit;

namespace XUnitTestTerminal3.AcceptanceTests.UseCases.StoreRelated
{
    public class AddProductReview : XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        private string product_id;
        public AddProductReview() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
            this.product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductReviewOk()
        {
            sut.Register("buyer@gmail.com", "buyer123");
            String user = sut.Login("buyer@gmail.com", "buyer123").Data;
            sut.AddProductToCart(user, product_id, 2, store_id);
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

            Result<List<String>> res = sut.Purchase(user, paymentDetails, deliveryDetails);
            Assert.True(res.ExecStatus);
            Assert.True(sut.AddProductReview(user, store_id, product_id, "Great product !").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductReviewNoPurchase()
        {
            sut.Register("buyer@gmail.com", "buyer123");
            String user = sut.Login("buyer@gmail.com", "buyer123").Data;

            Assert.False(sut.AddProductReview(user, store_id, product_id, "Great product !").ExecStatus);
        }
    }
}
