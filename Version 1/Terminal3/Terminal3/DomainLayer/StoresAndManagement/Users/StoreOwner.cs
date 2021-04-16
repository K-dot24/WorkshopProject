using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreOwner : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Store Store { get; }
        public StoreOwner AppointedBy { get; }
        public LinkedList<IStoreStaff> StoreStaffs { get; }

        public StoreOwner(RegisteredUser user, Store store, StoreOwner appointedBy)
        {
            User = user;
            Store = store;
            AppointedBy = appointedBy;
            StoreStaffs = new LinkedList<IStoreStaff>();
        }
    }
}
