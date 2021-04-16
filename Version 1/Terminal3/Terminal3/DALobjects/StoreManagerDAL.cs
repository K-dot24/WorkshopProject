using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreManagerDAL
    {
        //Properties
        public RegisteredUserDAL User { get; }
        public PermissionDAL Permissions { get; }
        public StoreOwnerDAL Owner { get; }

        public StoreManagerDAL(RegisteredUserDAL user, PermissionDAL permissions, StoreOwnerDAL owner)
        {
            User = user;
            Permissions = permissions;
            Owner = owner;
        }
    }
}
