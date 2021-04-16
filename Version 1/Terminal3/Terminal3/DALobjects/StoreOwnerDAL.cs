using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreOwnerDAL
    {
        //Properties
        public RegisteredUserDAL User { get; }
        public StoreDAL Store { get; }
        public StoreOwnerDAL Owner { get; }
        public LinkedList<StoreOwnerDAL> StoreOwners { get; }
        public LinkedList<StoreManagerDAL> StoreManager { get; }

        public StoreOwnerDAL(RegisteredUserDAL user, StoreDAL store, StoreOwnerDAL owner, LinkedList<StoreOwnerDAL> storeOwners, LinkedList<StoreManagerDAL> storeManager)
        {
            User = user;
            Store = store;
            Owner = owner;
            StoreOwners = storeOwners;
            StoreManager = storeManager;
        }
    }

    
}
