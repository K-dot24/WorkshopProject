using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class PurchaseTest: XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        private string product_id;
        public PurchaseTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
            this.product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void PaymentSystemTest()
        {
            sut.AddProductToCart(this.user_id, this.product_id, 5, this.store_id);
            IDictionary<String, Object> dictionaryDeliver = new Dictionary<String, Object>() { { "Name", "test_product" } };
            IDictionary<String, Object> dictionaryPay = new Dictionary<String, Object>();
            Assert.Equal("Attempt to purchase the shopping cart failed due to error in payment details\n", sut.Purchase(this.user_id, dictionaryPay, dictionaryDeliver).Message);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void DeliverySystemTest()
        {
            sut.AddProductToCart(this.user_id, this.product_id, 5, this.store_id);
            IDictionary<String, Object> dictionaryPay = new Dictionary<String, Object>() { { "Name", "test_product" } };
            IDictionary<String, Object> dictionaryDeliver = new Dictionary<String, Object>();
            Assert.Equal("Attempt to purchase the shopping cart failed due to error in delivery details\n", sut.Purchase(this.user_id, dictionaryPay, dictionaryDeliver).Message);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void FailPurchaseSystemTest()
        {
            IDictionary<String, Object> dictionaryPay = new Dictionary<String, Object>() { { "Name", "test_product" } };
            IDictionary<String, Object> dictionaryDeliver = new Dictionary<String, Object>() { { "Name", "test_product" } };
            Assert.Equal("The shopping cart is empty\n", sut.Purchase(this.user_id, dictionaryPay, dictionaryDeliver).Message);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SuccessPurchaseSystemTest()
        {
            sut.AddProductToCart(this.user_id, this.product_id, 5, this.store_id);
            IDictionary<String, Object> dictionaryPay = new Dictionary<String, Object>() { { "Name", "test_product" } };
            IDictionary<String, Object> dictionaryDeliver = new Dictionary<String, Object>() { { "Name", "test_product" } };
            Assert.NotNull(sut.Purchase(this.user_id, dictionaryPay, dictionaryDeliver).Data);            
        }

    }
}
