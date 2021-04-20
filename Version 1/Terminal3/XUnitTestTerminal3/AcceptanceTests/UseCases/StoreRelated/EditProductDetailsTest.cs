using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class EditProductDetailsTest: XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        public EditProductDetailsTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void EditProductDetailsName()
        {
            string product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() { { "Name", "new_name" } };
            sut.EditProductDetails(user_id, store_id, product_id, dictonary);

            Assert.True(sut.SearchProduct(dictonary).ExecStatus);
        }
       
        [Fact]
        [Trait("Category", "acceptance")]
        public void EditProductDetailsPrice()
        {
            string product_id =  sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() { { "Price", 20 } };

            sut.EditProductDetails(user_id, store_id, product_id, dictonary);
            Assert.True(sut.SearchProduct(dictonary).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void EditProductDetailsNoPermition()
        {
            string product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            IDictionary<String, Object> dictonary = new Dictionary<String, Object>() { { "Name", "new_name" } };

            sut.Register("noPermission@gmail.com", "nop123");
            string nop_id = sut.Login("noPermission@gmail.com", "nop123").Data;

            sut.EditProductDetails(nop_id, store_id, product_id, dictonary);
            Assert.False(sut.SearchProduct(dictonary).ExecStatus);
        }



    }
}
