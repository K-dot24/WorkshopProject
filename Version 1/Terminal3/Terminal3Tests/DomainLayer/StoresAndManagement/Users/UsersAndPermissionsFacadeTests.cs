﻿using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users.Tests
{
    public class UsersAndPermissionsFacadeTests
    {
        //Properties
        public UsersAndPermissionsFacade Facade { get; }

        //Constructor
        public UsersAndPermissionsFacadeTests()
        {
            Facade = new UsersAndPermissionsFacade();

        }

        //Tests
        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "raz@gmail.com", true)] //trying to register with a fresh email
        [InlineData("tomer@gmail.com", "tomer@gmail.com", false)] //trying to register with an existing email
        public void RegisterTest(String usedEmail, String requestedEmail, Boolean expectedResult)
        {
            Assert.True(Facade.Register(usedEmail, "password").ExecStatus);
            Result<RegisteredUser> registerResult = Facade.Register(requestedEmail, "password");
            Assert.Equal(expectedResult, registerResult.ExecStatus);

            if (expectedResult)
                Assert.True(Facade.RegisteredUsers.ContainsKey(registerResult.Data.Id), "Register returned true but user wasn't added to the dict");
            else
                Assert.Single(Facade.RegisteredUsers, "Register returned false but the user was added to the dict");

        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "tomer@gmail.com", true)]// trying to add a registered user to the system admins
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]// trying to add an unregistered user to the system admins
        //System admin need to be registered user
        public void AddSystemAdminTest(String registeredEmail, String adminEmail, Boolean expectedResult)
        {
            Result<RegisteredUser> registerResult = Facade.Register(registeredEmail, "password");
            Assert.Equal(expectedResult, Facade.AddSystemAdmin(adminEmail).ExecStatus);

            if (expectedResult)
                Assert.True(Facade.SystemAdmins.ContainsKey(registerResult.Data.Id), "AddSystemAdmin returned true but the user wasn't added to the admins dict");
            else
                Assert.Empty(Facade.SystemAdmins, "AddSystemAdmin returned false but the user was added to the admins dict");
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "raz@gmail.com", "raz@gmail.com", true)]  //user is exist in SystemAdmins
        [InlineData("tomer@gmail.com", "raz@gmail.com", "tomer@gmail.com", true)] //user is exist in SystemAdmins
        [InlineData("tomer@gmail.com", "raz@gmail.com", "shaked@gmail.com", false)] //user is NOT exist in SystemAdmins
        public void RemoveSystemAdminTest(String admin1, String admin2, String adminToRemove, Boolean expectedResult)
        {
            Facade.Register(admin1, "password");
            Facade.Register(admin2, "password");
            Facade.AddSystemAdmin(admin1);
            Facade.AddSystemAdmin(admin2);
            Assert.Equal(expectedResult, Facade.RemoveSystemAdmin(adminToRemove).ExecStatus);
            if (expectedResult)
                Assert.False(Facade.SystemAdmins.ContainsKey(adminToRemove), "RemoveSystemAdmin returned true but the user wasn't removed from the admins dict");
        }

        [Fact()]
        [Trait("Category", "Unit")]
        //at least one system admin in the system test
        public void RemoveSystemAdminTestAtLeastOne()
        {
            String email = "tomer@gmail.com";
            Result<RegisteredUser> registerResult = Facade.Register(email, "password");
            Facade.AddSystemAdmin(email);
            bool removedResultStatus = Facade.RemoveSystemAdmin(email).ExecStatus;
            Assert.False(removedResultStatus);
            if (!removedResultStatus)
                Assert.True(Facade.SystemAdmins.ContainsKey(email), "RemoveSystemAdmin returned false but still removed the user from the admins dict");

        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void LoginTest()
        {
            String email = "tomer@gmail.com";
            String password = "password";
            Facade.Register(email, password);
            Result<RegisteredUser> loginResult = Facade.Login(email, password);
            Assert.True(loginResult.ExecStatus, "Fail to login");
            if (loginResult.ExecStatus)
                Assert.True(loginResult.Data.LoggedIn, "User logged in but his LoggedIn attribute hasn't changed");
            Assert.False(Facade.Login(email, password).ExecStatus, "Able to loggin twice");
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void LogOutTest()
        {
            String email = "tomer@gmail.com";
            String password = "password";
            Facade.Register(email, password);
            Assert.False(Facade.LogOut(email).ExecStatus, "Able to loggout twice");
            Assert.True(Facade.Login(email, password).ExecStatus, "Not able to log in");
            Assert.True(Facade.LogOut(email).ExecStatus, "Not able to log out");

        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", 5, true, true)]      // Success
        [InlineData("raz@gmail.com", 10, true, false)]      // Fail 2: Higher quantity than quantity in store
        [InlineData("zoe@gmail.com", 0, false, false)]      // Fail: Illegal quantity
        [InlineData("shaked@gmail.com", -1, false, false)]  // Fail: Illegal quantity
        public void AddProductToCartTest(string userID, int productQuantity, Boolean expectedResult, Boolean expectedResult2)
        {
            // Open store
            RegisteredUser founder = new RegisteredUser(userID, "password");
            Facade.RegisteredUsers.TryAdd(userID, founder);
            Store store = new Store("Testore", founder);

            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            Product product2 = new Product("Apple", 4.9, 5, "Fruits");
            store.InventoryManager.Products.TryAdd(product.Id, product);
            store.InventoryManager.Products.TryAdd(product2.Id, product2);

            Assert.Equal(expectedResult, Facade.AddProductToCart(userID, product, productQuantity, store).ExecStatus);
            Assert.Equal(expectedResult2, Facade.AddProductToCart(userID, product2, productQuantity, store).ExecStatus);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "The banana was awsome")]  //user is exist in SystemAdmins
        public void AddProductReviewTest1(String email , String review)
        {
            // Open store
            RegisteredUser founder = new RegisteredUser(email, "password");
            Facade.RegisteredUsers.TryAdd(founder.Id, founder);
            Store store = new Store("Testore", founder);

            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            store.InventoryManager.Products.TryAdd(product.Id, product);

            // Add product to user shopping bag
            founder.ShoppingCart.ShoppingBags.TryAdd(store.Id, new ShoppingBag(founder, store));
            founder.ShoppingCart.ShoppingBags.TryGetValue(store.Id, out ShoppingBag bag);
            bag.Products.TryAdd(product, 2);

            // Add shopping bag to history
            founder.History.ShoppingBags.AddLast(bag);

            // Add review to product 
            Facade.AddProductReview(founder.Id, store, product, review);

            product.Review.TryGetValue(founder.Id, out String msg);

            Assert.Equal("The banana was awsome", msg);
        }


        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "The banana was awsome")]  //user is exist in SystemAdmins
        public void AddProductReviewTest2(String email, String review)
        {
            // Open store
            RegisteredUser founder = new RegisteredUser(email, "password");
            Facade.RegisteredUsers.TryAdd(founder.Id, founder);
            Store store = new Store("Testore", founder);

            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            store.InventoryManager.Products.TryAdd(product.Id, product);

            // Add review to product 
            Assert.False(Facade.AddProductReview(founder.Id, store, product, review).ExecStatus);
            Assert.False(product.Review.TryGetValue(founder.Id, out String msg));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ExitSystemTestGuest()
        {
            GuestUser user = new GuestUser();
            Facade.GuestUsers.TryAdd(user.Id, user);
            Facade.ExitSystem(user.Id);

            Assert.False(user.Active);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ExitSystemTestResisterd()
        {
            RegisteredUser user = Facade.Login("raz@gmail.com", "123").Data;
            Facade.RegisteredUsers.TryAdd(user.Id, user);
            Facade.ExitSystem(user.Id);

            Assert.False(user.LoggedIn);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ExitSystemTestNotExist()
        {
            Assert.False(Facade.ExitSystem("0123").ExecStatus);
        }
    }
}