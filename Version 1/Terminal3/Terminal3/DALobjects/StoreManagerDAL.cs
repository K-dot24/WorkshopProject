using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreManagerDAL : IStoreStaffDAL
    {
        //Properties
        public RegisteredUserDAL User { get; }
        public PermissionDAL Permissions { get; }
        public StoreOwnerDAL Owner { get; }

        //Constructor
        public StoreManagerDAL(RegisteredUserDAL user, PermissionDAL permissions, StoreOwnerDAL owner):base(user.Id)
        {
            User = user;
            Permissions = permissions;
            Owner = owner;
        }

    }
}
