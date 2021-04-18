using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreOwner : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Store Store { get; }
        public StoreOwner AppointedBy { get; }
        public LinkedList<StoreManager> StoreManagers { get; }
        public LinkedList<StoreOwner> StoreOwners { get; }

        public StoreOwner(RegisteredUser user, Store store, StoreOwner appointedBy)
        {
            User = user;
            Store = store;
            AppointedBy = appointedBy;
            StoreManagers = new LinkedList<StoreManager>();
            StoreOwners = new LinkedList<StoreOwner>();
        }

        public Result<StoreOwnerDAL> GetDAL()
        {
            RegisteredUserDAL user = User.GetDAL().Data;
            StoreDAL store = Store.GetDAL().Data;
            StoreOwnerDAL owner = AppointedBy.GetDAL().Data;            
            LinkedList<StoreOwnerDAL> storeOwners = new LinkedList<StoreOwnerDAL>();
            LinkedList<StoreManagerDAL> storeManagers = new LinkedList<StoreManagerDAL>();

            foreach (StoreOwner so in StoreOwners)
            {
                storeOwners.AddLast(so.GetDAL().Data);
            }

            foreach (StoreManager sm in StoreManagers)
            {
                storeManagers.AddLast(sm.GetDAL().Data);
            }
            
            return new Result<StoreOwnerDAL>("Store owner DAL object", true, new StoreOwnerDAL(user, store, owner, storeOwners, storeManagers));
        }
    }
}
