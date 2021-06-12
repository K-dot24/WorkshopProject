using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Terminal3.ServiceLayer;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer.ServiceObjects;

namespace XUnitTestTerminal3
{
    public class SearchProductTest: XUnitTerminal3TestCase
    {
        private string user_id; 
        private string store_id; 
        public SearchProductTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
        }


        [Fact(Skip = "Not relevent")]
        [Trait("Category", "acceptance")]
        public void SerchProductByName()
        {
            sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test");
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>(){{ "Name", "test_product" } }; 

            Assert.True(sut.SearchProduct(dictonary).ExecStatus);
        }


        [Fact(Skip = "Not relevent")]
        [Trait("Category", "acceptance")]
        public void SerchProductByPrice()
        {
            sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test");
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Price", 10 }};

            Assert.True(sut.SearchProduct(dictonary).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchProductNotExist()
        {
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test" }};
            Assert.False(sut.SearchProduct(dictonary).ExecStatus);
        }

    }
}
