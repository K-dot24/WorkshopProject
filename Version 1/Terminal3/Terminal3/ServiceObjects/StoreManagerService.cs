using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreManagerService : IStoreStaffService
    {
        //Properties
        public String UserId { get; }
        public PermissionService Permissions { get; }
        public String OwnerId { get; }

        //Constructor
        public StoreManagerService(String userID, PermissionService permissions, String ownerID):base(userID)
        {
            UserId = userID;
            Permissions = permissions;
            OwnerId = ownerID;
        }

    }
}
