using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class OpenNewStoreTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public OpenNewStoreTest()
        {
            sut.Register("test@gmail.com", "test123");
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void OpenNewStore()
        {
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            sut.OpenNewStore("test_store" , user_id);
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() { { "Name", "test_store" } };

            Assert.True(sut.SearchStore(dictonary).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void OpenNewStoreNotRegister()
        {
            this.user_id = sut.Login("test@gmail.com", "0123").Data;
            sut.OpenNewStore("test_store", "0123");
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test_store" }};

            Assert.False(sut.SearchStore(dictonary).ExecStatus);
        }
    }
}
