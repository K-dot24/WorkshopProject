using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class AddProductToCartTest : XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        public AddProductToCartTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToCart()
        {

            string product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            Assert.True(sut.AddProductToCart(user_id, product_id, 1, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToCart_f1() //store id invalid
        {
            string product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            Assert.False(sut.AddProductToCart(user_id, product_id, 1, "0123").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToCart_f2() //product id invalid
        {
            Assert.False(sut.AddProductToCart(user_id, "0123", 1, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToCart_f3() //product not in store
        {
            string store_id_add = sut.OpenNewStore("test_store", user_id).Data;
            string product_id = sut.AddProductToStore(user_id, store_id_add, "test_product", 10, 10, "test").Data;

            Assert.False(sut.AddProductToCart(user_id, product_id, 1, store_id).ExecStatus);
        }
    }
}

