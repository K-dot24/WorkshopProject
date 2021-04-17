using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class StoresFacadeTests
    {
        public StoresFacade Facade { get; }
        public Store TestStore { get; }
        public RegisteredUser Founder { get; }

        public StoresFacadeTests()
        {
            Facade = new StoresFacade();

            //Initial data
            Founder = new RegisteredUser("aviD@hotmale.com", "qwerty1");
            TestStore = new Store("The Testore", Founder);
            Facade.Stores.TryAdd(TestStore.Id, TestStore);
        }

        //TODO: Complete tests after DAL objects sorted

        [Fact()]
        public void AddProductToStoreTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void RemoveProductFromStoreTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void EditProductDetailsTest()
        {
            throw new NotImplementedException();
        }

        [Theory()]
        [InlineData("tomer@gmail.com", "aviD@hotmale.com", true)]
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]
        [InlineData("aviD@hotmale.com", "aviD@hotmale.com", false)]
        public void AddStoreOwnerTest(string futureOwnerEmail, string currentlyOwnerEmail, Boolean expectedResult)
        {
            RegisteredUser futureOwner = new RegisteredUser(futureOwnerEmail, "password");
            Assert.Equal(expectedResult, Facade.AddStoreOwner(futureOwner, currentlyOwnerEmail, TestStore.Id).ExecStatus);
        }

        [Fact()]
        public void AddStoreManagerTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void RemoveStoreManagerTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void OpenNewStoreTest()
        {
            throw new NotImplementedException();
        }
    }
}