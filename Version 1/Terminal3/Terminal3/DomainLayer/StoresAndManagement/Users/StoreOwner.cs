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
        public LinkedList<IStoreStaff> StoreStaffs { get; }

        public StoreOwner(RegisteredUser user, Store store, StoreOwner appointedBy)
        {
            User = user;
            Store = store;
            AppointedBy = appointedBy;
            StoreStaffs = new LinkedList<IStoreStaff>();
        }

        public Result<StoreOwnerDAL> GetDAL()
        {
            RegisteredUserDAL user = User.GetDAL().Data;
            StoreDAL store = Store.GetDAL().Data;
            StoreOwnerDAL owner = AppointedBy.GetDAL().Data;
            LinkedList<StoreOwnerDAL> storeOwners = new LinkedList<StoreOwnerDAL>();
            LinkedList<StoreManagerDAL> storeManagers = new LinkedList<StoreManagerDAL>();

            //TODO ?!?! instanceOF ??
            foreach(IStoreStaff ISF in StoreStaffs)
            {
                if(ISF.GetType() == )
            }
    }
    }
}
