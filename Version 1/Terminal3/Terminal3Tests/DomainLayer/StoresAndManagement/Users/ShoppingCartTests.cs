using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users.Tests
{
    public class ShoppingCartTests
    {
        //Properties
        public ShoppingBag ShoppingBag { get; }
        public ShoppingCart ShoppingCart { get; }
        public Product[] Products { get; }
        public Store Store { get; }

        //Constructor
        public ShoppingCartTests()
        {
            RegisteredUser user = user = new RegisteredUser("test", "password");
            Store= new Store("TestStore", user);
            Product banana = new Product("Banana", 19.9, 10, "Fruit");
            Product mango = new Product("Mango", 34.9, 10, "Fruit");
            Products = new Product[] { banana, mango };
            ShoppingBag = new ShoppingBag(user, Store);
            ShoppingCart = new ShoppingCart();
        }

        [Fact()]
        [Trait("Category","Unit")]
        public void AddShoppingBagToCartTest()
        {
            ShoppingCart.AddShoppingBagToCart(ShoppingBag);
            Assert.True(ShoppingCart.ShoppingBags.ContainsKey(Store.Id));
            Assert.Equal(ShoppingBag, ShoppingCart.ShoppingBags[Store.Id]);
        }
    }
}