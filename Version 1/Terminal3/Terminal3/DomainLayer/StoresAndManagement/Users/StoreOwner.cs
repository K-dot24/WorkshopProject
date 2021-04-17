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

        public StoreOwner(StoreOwnerDAL storeOwner)
        {
            this.User = Mapper.GetRegisteredUser(storeOwner.User);
            this.Store = Mapper.GetStore(storeOwner.Store);
            this.Owner = Mapper.GetStoreOwner(storeOwner.Owner); 
            this.StoreStaffs = new LinkedList<IStoreStaff>();
            foreach(StoreOwnerDAL so in storeOwner.StoreOwners)
            {
                StoreStaffs.AddLast(Mapper.GetStoreOwner(so));
            }
            foreach (StoreManagerDAL sm in storeOwner.StoreManager)
            {
                StoreStaffs.AddLast(Mapper.GetStoreManager(sm));
            }
        }

        public Result<StoreOwnerDAL> GetDAL()
        {
            RegisteredUserDAL user = (RegisteredUser)user.GetDAL().Data;
            StoreDAL store = store.GetDAL().Data;
            StoreOwnerDAL owner = Owner.GetDAL().Data;
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
