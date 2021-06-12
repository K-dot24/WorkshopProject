using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users.Tests
{
    public class ShoppingBagTests
    {
        //Properties
        public ShoppingBag ShoppingBag { get; }
        public Product[] Products { get; }
        
        //Constructor
        public ShoppingBagTests()
        {
            RegisteredUser user = user = new RegisteredUser("test","password");
            Store store = new Store("TestStore", user);
            Product banana = new Product("Banana", 19.9, 10, "Fruit");
            Product mango = new Product("Mango", 34.9, 10, "Fruit");
            Products = new Product[] { banana, mango };
            ShoppingBag = new ShoppingBag(user,store);
        }


        [Theory()]
        [Trait("Category","Unit")]
        [InlineData(10,true)]
        [InlineData(11,false)]
        [InlineData(1,true)]
        [InlineData(0,false)]
        public void AddProtuctToShoppingBagTest(int quantity, bool expectedResult)
        {
            Result<bool> res = ShoppingBag.AddProtuctToShoppingBag(Products[0], quantity, new List<Stores.Policies.Offer.Offer>());
            Assert.Equal(expectedResult, res.ExecStatus);
            Assert.Equal(expectedResult, ShoppingBag.Products.ContainsKey(Products[0]));

        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData(1,10, false)]
        [InlineData(0,0, true)]
        [InlineData(0,11, false)]
        public void UpdateShoppingBagTest(int productIndex,int quantity, bool expectedResult)
        {
            ShoppingBag.AddProtuctToShoppingBag(Products[0], 10, new List<Stores.Policies.Offer.Offer>());
            Result<ShoppingBag> res = ShoppingBag.UpdateShoppingBag(Products[productIndex], quantity);

            Assert.Equal(expectedResult, res.ExecStatus);
            if (expectedResult)
            {
                if (quantity <= 0)
                {
                    //Item was removed
                    Assert.False(ShoppingBag.Products.ContainsKey(Products[productIndex]));
                }
                else
                {
                    //Item was updated
                    Assert.True(ShoppingBag.Products.ContainsKey(Products[productIndex]));
                    Assert.Equal(quantity, ShoppingBag.Products[Products[productIndex]]);
                }
            }
            if (quantity > 10)
            {
                //Value not changed
                Assert.Equal(10,ShoppingBag.Products[Products[productIndex]]);
            }
        }
    }
}