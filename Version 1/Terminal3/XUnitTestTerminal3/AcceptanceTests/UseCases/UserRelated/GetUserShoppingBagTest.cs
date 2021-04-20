using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3.AcceptanceTests.UseCases
{
    public class GetUserShoppingBagTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public GetUserShoppingBagTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }


        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingBag()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 1, store_id);

            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;
            Dictionary<String, int> pids = sut.GetUserShoppingBag(user_id, shoppingBags[0]).Data;

            int quantity;
            bool get = pids.TryGetValue(product_id, out quantity);

            Assert.True(get == true && quantity == 1);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetUserShoppingBagUpdated()
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
        public void GetUserShoppingBagEmpty()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;

            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;
            Assert.False(sut.GetUserShoppingBag(user_id, shoppingBags[0]).ExecStatus);
        }
    }
}
