using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class GetUserShoppingCartTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public GetUserShoppingCartTest()
        {
            sut.ResetSystem();
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingCartEmpty()
        {
            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data; 

            Assert.True(shoppingBags.Count == 0);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingCartNotEmpty()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id  = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 1, store_id); 
            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;

            Assert.True(shoppingBags.Count == 1);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingCartDelete()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 1, store_id);

            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;
            sut.UpdateShoppingCart(user_id, shoppingBags[0], product_id, 0);

            Assert.True(shoppingBags.Count == 0);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingCartWrongPid()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            sut.AddProductToCart(user_id, "0123", 1, store_id);
            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;

            Assert.True(shoppingBags.Count == 0);
        }
    }
