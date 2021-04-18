using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class AddStoreOwnerTest : XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        public AddStoreOwnerTest()
        {
            sut.ResetSystem();
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
        }


        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreOwner()
        {
            sut.Register("newOwner@gmail.com", "newOwner123");
            string newOwnerID = sut.Login("newOwner@gmail.com", "newOwner123").Data;

            Assert.True(sut.AddStoreOwner(newOwnerID, user_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreOwnerSwapID()
        {
            sut.Register("newOwner@gmail.com", "newOwner123");
            string newOwnerID = sut.Login("newOwner@gmail.com", "newOwner123").Data;

            Assert.False(sut.AddStoreOwner(user_id, newOwnerID, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreOwnerSameID()
        {
            Assert.False(sut.AddStoreOwner(user_id, user_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreOwnerWrongID()
        {
            Assert.False(sut.AddStoreOwner("0123", user_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void AddStoreOwnerAlreadyOwner()
        {
            sut.Register("newOwner@gmail.com", "newOwner123");
            string newOwnerID = sut.Login("newOwner@gmail.com", "newOwner123").Data;
            sut.OpenNewStore("newOwner_store", newOwnerID);


            Assert.False(sut.AddStoreOwner(newOwnerID, user_id, store_id).ExecStatus);
        }

    }
}
