using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class SearchStoreTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public SearchStoreTest()
        {
            sut.ResetSystem();
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchStore()
        {
            string store_id = sut.OpenNewStore("test_store", user_id).Data;
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test_store" }};

            Assert.True(sut.SearchStore(dictonary).ExecStatus);
        }


        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchStoreNotExist()
        {
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() { { "Name", "test_store" } };
            Assert.False(sut.SearchStore(dictonary).ExecStatus);
        }

    }
}
