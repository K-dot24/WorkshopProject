using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class UpdateShoppingCartTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public UpdateShoppingCartTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void UpdateShoppingCart()
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
        public void UpdateShoppingCartQuantity()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 10, store_id);

            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;
            sut.UpdateShoppingCart(user_id, shoppingBags[0], product_id, 5);
            Dictionary<String, int> pids = sut.GetUserShoppingBag(user_id, shoppingBags[0]).Data;

            int quantity;
            bool get = pids.TryGetValue(product_id, out quantity);

            Assert.True(get == true && quantity == 5);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingCartWrongShoppingBagID()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 1, store_id);

            Assert.False(sut.UpdateShoppingCart(user_id, "0123", product_id, 3).ExecStatus);
        }
    }
}
