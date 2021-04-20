using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreOwnerDAL : IStoreStaffDAL
    {
        //Properties
        public String UserId { get; }
        public String StoreId { get; }
        public String OwnerId { get; }
        public LinkedList<String> StoreOwners { get; }
        public LinkedList<String> StoreManager { get; }

        //Constructor
        public StoreOwnerDAL(String userID, String storeID, String ownerID, LinkedList<String> storeOwners, LinkedList<String> storeManager):base(userID)
        {
            UserId = userID;
            StoreId = storeID;
            OwnerId = ownerID;
            StoreOwners = storeOwners;
            StoreManager = storeManager;
        }

    }

    
}
