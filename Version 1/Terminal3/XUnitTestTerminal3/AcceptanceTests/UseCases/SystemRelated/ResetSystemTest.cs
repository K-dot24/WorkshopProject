using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class ResetSystemTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public ResetSystemTest()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemUser()
        {
            sut.Register("test@gmail.com", "test123");
            sut.ResetSystem();

            Assert.False(sut.Login("test@gmail.com", "test123").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemProduct()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;

            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;

            sut.ResetSystem();
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() { { "Name", "test_product" } };

            Assert.False(sut.SearchProduct(dictonary).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemCart()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;

            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 1, store_id);

            sut.ResetSystem();
            Assert.False(sut.GetUserShoppingCart(user_id).ExecStatus);
        }
    }
}
