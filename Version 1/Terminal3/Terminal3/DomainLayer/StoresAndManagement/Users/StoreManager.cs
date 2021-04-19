using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DALobjects;
using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreManager : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Permission Permission { get; }
        public StoreOwner AppointedBy { get; }
        public Store Store { get; }

        public StoreManager(RegisteredUser user, Store store, Permission permission , StoreOwner appointedBy)
        {
            User = user;
            Store = store;
            Permission = permission;
            AppointedBy = appointedBy;
        }

        public Result<Boolean> SetPermission(int method, Boolean active)
        {
            return Permission.SetPermission(method, active);
        }

        public Result<Boolean> SetPermission(Methods method, Boolean active)
        {
            return Permission.SetPermission(method, active);
        }

        public Result<object> GetDAL()
        {
            RegisteredUserDAL user = User.GetDAL().Data;
            PermissionDAL permission = Permission.GetDAL().Data;
            StoreOwnerDAL owner = (StoreOwnerDAL)AppointedBy.GetDAL().Data;

            return new Result<object>("Store manager DAL object", true, new StoreManagerDAL(user, permission, owner));
        }
    }
}
