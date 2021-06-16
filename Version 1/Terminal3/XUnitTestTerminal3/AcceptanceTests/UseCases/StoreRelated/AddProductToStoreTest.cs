using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Xunit; 


namespace XUnitTestTerminal3
{
    public class AddProductToStoreTest : XUnitTerminal3TestCase
    {

        private string user_id;
        public AddProductToStoreTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToStore()
        {
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            string store_id =  sut.OpenNewStore("test_store", user_id).Data;
            Assert.True(sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToStoreNoStore()
        {
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            sut.OpenNewStore("test_store", user_id);
            Assert.False(sut.AddProductToStore(user_id, "0123", "test_product", 10, 10, "test").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddProductToStoreNoUser()
        {
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            string store_id = sut.OpenNewStore("test_store", user_id).Data;
            Assert.False(sut.AddProductToStore("0123", store_id, "test_product", 10, 10, "test").ExecStatus);

        }
    }
}
