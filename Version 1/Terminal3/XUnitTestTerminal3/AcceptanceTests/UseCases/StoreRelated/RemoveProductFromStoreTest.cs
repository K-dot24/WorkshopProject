using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class RemoveProductFromStoreTest: XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;

        public RemoveProductFromStoreTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void RemoveProductFromStore()
        {
            string product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            Assert.True(sut.RemoveProductFromStore(user_id, store_id, product_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void RemoveProductFromStorePNotExist()
        {
            string store_id_add = sut.OpenNewStore("test_store", user_id).Data;
            string product_id = sut.AddProductToStore(user_id, store_id_add, "test_product", 10, 10, "test").Data;

            Assert.False(sut.RemoveProductFromStore(user_id, store_id, product_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void RemoveProductFromStoreSNotExist()
        {
            string product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;

            Assert.False(sut.RemoveProductFromStore(user_id, store_id, product_id).ExecStatus);
        }
    }
}
