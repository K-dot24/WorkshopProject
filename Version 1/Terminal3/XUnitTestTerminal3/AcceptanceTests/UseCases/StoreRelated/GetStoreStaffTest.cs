using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class GetStoreStaffTest: XUnitTerminal3TestCase
    {
        private string owner_id;
        private string manager_id;
        private string store_id;
        public GetStoreStaffTest()
        {
            //owner
            sut.ResetSystem();
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
            sut.SetPermissions(manager_id, owner_id, permission);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetStoreStaffOwner()
        {
            Dictionary<String, List<int>> storeStaff = sut.GetStoreStaff(owner_id, store_id).Data;

            List<int> owner_per;
            storeStaff.TryGetValue(owner_id, out owner_per);

            Assert.True(owner_per[0] == 777);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void GetStoreStaffManager()
        {
            Dictionary<String, List<int>> storeStaff = sut.GetStoreStaff(owner_id, store_id).Data;

            List<int> manager_per;
            storeStaff.TryGetValue(owner_id, out manager_per);

            Assert.True(manager_per.Count == 2 && manager_per[0] == 0 && manager_per[1] == 1);
        }
    }
}
