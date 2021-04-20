using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreManagerDAL : IStoreStaffDAL
    {
        //Properties
        public String UserId { get; }
        public PermissionDAL Permissions { get; }
        public String OwnerId { get; }

        //Constructor
        public StoreManagerDAL(String userID, PermissionDAL permissions, String ownerID):base(userID)
        {
            UserId = userID;
            Permissions = permissions;
            OwnerId = ownerID;
        }

    }
}
