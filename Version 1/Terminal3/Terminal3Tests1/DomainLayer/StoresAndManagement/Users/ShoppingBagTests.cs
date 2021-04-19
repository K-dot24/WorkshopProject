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
        public ShoppingBagTests()
        {
            RegisteredUser user = user = new RegisteredUser("test","password");
            Store store = new Store("TestStore", user);
            Product banana = new Product("Banana", 19.9, 10, "Fruit");
            Product mango = new Product("Mango", 34.9, 10, "Fruit");
            Products = new Product[] { banana, mango };
            ShoppingBag = new ShoppingBag(user,store);
        }

        //Constructor

        [Theory()]
        [Trait("Category","Unit")]
        [InlineData(10,true)]
        [InlineData(11,false)]
        [InlineData(1,true)]
        [InlineData(0,false)]
        public void AddProtuctToShoppingBagTest(int quantity, bool expectedResult)
        {
            Result<bool> res = ShoppingBag.AddProtuctToShoppingBag(Products[0], quantity);
            Assert.Equal(expectedResult, res.ExecStatus);
            Assert.Equal(expectedResult, ShoppingBag.Products.ContainsKey(Products[0]));

        }

        [Fact()]
        public void UpdateShoppingBagTest()
        {
            throw new NotImplementedException();
        }
    }
}