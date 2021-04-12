using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagment.Stores;


namespace Terminal3.DomainLayer.StoresAndManagment.Users
{
    class StoreOwner : StoreStaff
    {
        private RegisteredUser user;
        private Store store;
        private StoreOwner storeOwner;
        private LinkedList<StoreStaff> storeStaffs; 

        public StoreOwner(RegisteredUser user, Store store, StoreOwner storeOwner)
        {
            this.user = user;
            this.store = store;
            this.storeOwner = storeOwner;
            storeStaffs = new LinkedList<StoreStaff>();
        }
    }
}
