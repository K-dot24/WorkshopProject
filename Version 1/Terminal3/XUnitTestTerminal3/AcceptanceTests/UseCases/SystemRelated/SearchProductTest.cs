using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Terminal3.ServiceLayer;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace XUnitTestTerminal3
{
    public class SearchProductTest: XUnitTerminal3TestCase
    {
        private RegisteredUser user; 
        private Store store; 
        private Product product; 
        public SearchProductTest()
        {
            sut.ResetSystem();
            this.user = new RegisteredUser("test@gmail.com", "test123"); 
            this.store = new Store(user);
            this.product = new Product("test", 10, 10);
        }

/*
        public void SerchProductByName1()
        {
            sut.ResetSystem();
            sut.Register("test@gmail.com", "test123");
            sut.OpenNewStore("test_store", ); 
        }


        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchProductByName()
        {            
            sut.AddProductToStore()
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test" }}; 

            Result<Object> res = sut.SearchProduct(dictonary); 
            Assert.True(res.ExecStatus);
        }
*/

        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchProductByPrice()
        {
            store.AddNewProduct(product);
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Price", 10 }};

            Result<Object> res = sut.SearchProduct(dictonary); 
            Assert.True(res.ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SerchProductNotExist()
        {
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test" }};
            Result<Object> res = sut.SearchProduct(dictonary); 
            Assert.False(res.ExecStatus);
        }

    }
}
