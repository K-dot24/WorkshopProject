using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagment.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    class StoreManager : StoreStaff
    {
        private RegisteredUser user;
        private Store store;
        private Permission permission;
        private StoreOwner storeOwner;

        public StoreManager(RegisteredUser user, Store store, Permission permission , StoreOwner storeOwner)
        {
            this.user = user;
            this.store = store;
            this.permission = permission;
            this.storeOwner = storeOwner;
        }
    }
}
