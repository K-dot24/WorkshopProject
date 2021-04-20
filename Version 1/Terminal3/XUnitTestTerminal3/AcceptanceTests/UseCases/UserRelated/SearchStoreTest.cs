using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class SearchStoreTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public SearchStoreTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchStore()
        {
            string store_id = sut.OpenNewStore("test_store", user_id).Data;
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test_store" }};
            List<String> store_names = sut.SearchStore(dictonary).Data;

            Assert.True(sut.SearchStore(dictonary).ExecStatus);
            Assert.True(store_names[0].Equals("test_store"));
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
