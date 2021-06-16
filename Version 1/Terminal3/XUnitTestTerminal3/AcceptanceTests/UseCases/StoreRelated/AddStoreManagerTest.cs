using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class AddStoreManagerTest : XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        public AddStoreManagerTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreManager()
        {
            sut.Register("manager@gmail.com", "manager123");
            string managerID = sut.Login("manager@gmail.com", "manager123").Data;

            Assert.True(sut.AddStoreManager("manager@gmail.com", user_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreManagerSwapID()
        {
            sut.Register("manager@gmail.com", "manager123");
            string managerID = sut.Login("manager@gmail.com", "manager123").Data;

            Assert.False(sut.AddStoreManager("test@gmail.com", managerID, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreManagerSameID()
        {
            Assert.False(sut.AddStoreManager("test@gmail.com", user_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreManagerWrongID()
        {
            Assert.False(sut.AddStoreManager("0123", user_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreManagerAlreadyOwnerAtAnotherStore()
        {
            sut.Register("manager@gmail.com", "manager123");
            string managerID = sut.Login("manager@gmail.com", "manager123").Data;
            sut.OpenNewStore("newOwner_store", managerID);


            Assert.True(sut.AddStoreManager("manager@gmail.com", user_id, store_id).ExecStatus);
        }

    }
}
