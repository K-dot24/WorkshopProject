﻿using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreManager : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Permission Permission { get; }
        public StoreOwner Owner { get; }

        public StoreManager(RegisteredUser user, Permission permission , StoreOwner storeOwner)
        {
            this.User = user;
            this.Permission = permission;
            this.Owner = storeOwner;
        }
    }
}
