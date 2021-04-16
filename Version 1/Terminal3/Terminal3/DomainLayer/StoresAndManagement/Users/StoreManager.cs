using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreManager : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Store Store { get; }
        public Permission Permission { get; }
        public StoreOwner AppointedBy { get; }

        public StoreManager(RegisteredUser user, Store store, Permission permission , StoreOwner storeOwner)
        {
            User = user;
            Store = store;
            Permission = permission;
            AppointedBy = storeOwner;
        }
    }
}
