using Xunit;
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
        [InlineData("tomer@gmail.com", "raz@gmail.com", true)]
        [InlineData("tomer@gmail.com", "tomer@gmail.com", false)]
        public void RegisterTest(String usedEmail, String requestedEmail, Boolean expectedResult)
        {
            Assert.True(Facade.Register(usedEmail, "password").ExecStatus);
            Assert.Equal(expectedResult, Facade.Register(requestedEmail, "password").ExecStatus);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "tomer@gmail.com", true)]
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]
        //System admin need to be registered user
        public void AddSystemAdminTest(String registeredEmail, String adminEmail, Boolean expectedResult)
        {
            Facade.Register(registeredEmail, "password");
            Assert.Equal(expectedResult, Facade.AddSystemAdmin(adminEmail).ExecStatus);
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

        }

        [Fact()]
        [Trait("Category", "Unit")]
        //at least one system admin in the system test
        public void RemoveSystemAdminTestAtLeastOne()
        {
            String email = "tomer@gmail.com";
            Facade.Register(email, "password");
            Facade.AddSystemAdmin(email);
            Assert.False(Facade.RemoveSystemAdmin(email).ExecStatus);

        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void LoginTest()
        {
            String email = "tomer@gmail.com";
            String password = "password";
            Facade.Register(email, password);
            Assert.True(Facade.Login(email, password).ExecStatus, "Fail to login");
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