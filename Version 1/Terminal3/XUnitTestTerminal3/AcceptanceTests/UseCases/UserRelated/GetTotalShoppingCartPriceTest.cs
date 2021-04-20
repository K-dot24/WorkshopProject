using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class GetTotalShoppingCartPriceTest: XUnitTerminal3TestCase
    {
        private string user_id;
        public GetTotalShoppingCartPriceTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetTotalShoppingCartPriceEmpty()
        {
            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;
            double price = sut.GetTotalShoppingCartPrice(user_id).Data;

            Assert.True(price == 0);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetTotalShoppingCartPrice()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 2, store_id);
            double price = sut.GetTotalShoppingCartPrice(user_id).Data;

            Assert.True(price == 20);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetTotalShoppingCartPrice2products()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id1 = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id1, 1, store_id);

            String product_id2 = sut.AddProductToStore(user_id, store_id, "test_product", 15, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id2, 1, store_id);

            double price = sut.GetTotalShoppingCartPrice(user_id).Data;

            Assert.True(price == 25);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetTotalShoppingCartPrice2Stores()
        {
            String store_id1 = sut.OpenNewStore("test_store1", user_id).Data;
            String product_id1 = sut.AddProductToStore(user_id, store_id1, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id1, 1, store_id1);

            String store_id2 = sut.OpenNewStore("test_store2", user_id).Data;
            String product_id2 = sut.AddProductToStore(user_id, store_id2, "test_product", 15, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id2, 1, store_id2);

            double price = sut.GetTotalShoppingCartPrice(user_id).Data;

            Assert.True(price == 25);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetTotalShoppingCartPriceDelete()
        {
            String store_id = sut.OpenNewStore("test_store", user_id).Data;
            String product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;
            sut.AddProductToCart(user_id, product_id, 1, store_id);

            List<String> shoppingBags = sut.GetUserShoppingCart(user_id).Data;
            sut.UpdateShoppingCart(user_id, shoppingBags[0], product_id, 0);
            double price = sut.GetTotalShoppingCartPrice(user_id).Data;

            Assert.True(price == 0);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetTotalShoppingCartPriceFail()
        {
            Assert.False(sut.GetTotalShoppingCartPrice("0123").ExecStatus);
        }
    }
}
