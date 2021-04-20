using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class SetPermissionsTest: XUnitTerminal3TestCase
    {
        private string owner_id;
        private string manager_id; 
        private string store_id; 
        public SetPermissionsTest() : base()
        {
            //owner
            sut.Register("owner@gmail.com", "owner123");
            this.owner_id = sut.Login("owner@gmail.com", "owner123").Data;
            this.store_id = sut.OpenNewStore("test_store", owner_id).Data;

            //manager
            sut.Register("manager@gmail.com", "manager123");
            this.manager_id = sut.Login("manager@gmail.com", "manager123").Data;
            sut.AddStoreManager(manager_id, owner_id, store_id);

            //permition
            LinkedList<int> permission = new LinkedList<int>();
            permission.AddLast(0);
            permission.AddLast(1);
            sut.SetPermissions(store_id ,manager_id, owner_id, permission);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SetPermissionsAdd()
        {  
            Assert.True(sut.AddProductToStore(manager_id, store_id, "test_product", 10, 10, "test").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SetPermissionsRemove()
        {
            string product_id = sut.AddProductToStore(manager_id, store_id, "test_product", 10, 10, "test").Data;
            Assert.True(sut.RemoveProductFromStore(manager_id, store_id, product_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SetPermissionsFalied()
        {
            sut.Register("new_manager@gmail.com", "new_manager123");
            string new_manager_id = sut.Login("new_manager@gmail.com", "new_manager123").Data;
            
            Assert.False(sut.AddStoreManager(new_manager_id, manager_id, store_id).ExecStatus);
        }

    }
}
