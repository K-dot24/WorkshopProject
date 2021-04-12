using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    class StoreOwner : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Store Store { get; }
        public StoreOwner Owner { get; }
        public LinkedList<IStoreStaff> StoreStaffs { get; }

        public StoreOwner(RegisteredUser user, Store store, StoreOwner storeOwner)
        {
            this.User = user;
            this.Store = store;
            this.Owner = storeOwner;
            this.StoreStaffs = new LinkedList<IStoreStaff>();
        }
    }
}
