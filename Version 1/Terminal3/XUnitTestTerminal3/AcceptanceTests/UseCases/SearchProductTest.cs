using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Terminal3.ServiceLayer;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace XUnitTestTerminal3
{
    public class SearchProductTest: XUnitTerminal3TestCase
    {
        private Store store; // TODO before each
        private Product product; 

        public SearchProductTest()
        {
            this.store = new Store();
            this.product = new Product("test", 10, 10);
        }

        [Fact]
        [Trait("Search Product", "Version 1")]
        public void SerchProductByName()
        {            
            store.AddNewProduct(product);
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Name", "test" }}; 

            Result<Boolean> res = sut.SearchProduct(dictonary); 
            Assert.True(res.ExecStatus);
        }

        [Fact]
        [Trait("Search Product", "Version 1")]
        public void SerchProductByPrice()
        {
            store.AddNewProduct(product);
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() {{ "Price", 10 }};

            Result<Boolean> res = sut.SearchProduct(dictonary); 
            Assert.True(res.ExecStatus);
        }

        [Fact]
        [Trait("Search Product", "Version 1")]
        public void SerchProductNotExist()
        {
            sut.ResetSystem();
            Result<Boolean> res = sut.SearchProduct(store, product); //TODO
            Assert.False(res.ExecStatus);
        }

        [Fact]
        [Trait("Search Product", "Version 1")]
        public void SerchProductOtherStore()
        {
            sut.ResetSystem();
            Store other_store = new Store();
            other_store.AddNewProduct(product);

            Result<Boolean> res = sut.SearchProduct(store, product); //TODO
            Assert.False(res.ExecStatus);
        }

        private class Dictionary<T> : IDictionary<string, object>
        {
        }
    }
}
