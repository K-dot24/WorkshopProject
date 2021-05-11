using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ServiceLayer.ServiceObjects;
using System;
using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreManager : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Permission Permission { get; }
        public IStoreStaff AppointedBy { get; }
        public Store Store { get; }

        public StoreManager(RegisteredUser user, Store store, Permission permission , IStoreStaff appointedBy)
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
            PermissionService permission = Permission.GetDAL().Data;

            return new Result<object>("Store manager DAL object", true, new StoreManagerService(User.Id, permission, AppointedBy.GetId()));
        }

        public String GetId()
        {
            return User.Id;
        }

        public Result<Boolean> Update(Notification notification)
        {
            return User.Update(notification);
        }
    }
}
