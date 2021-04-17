using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DALobjects;

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

        public StoreManager(StoreManagerDAL storeManagerDAL)
        {
            this.User = Mapper.GetStoreManager(storeManagerDAL.User);
            this.Permission = Mapper.GetPermission(storeManagerDAL.Permissions);
            this.Owner = Mapper.GetOwner(storeManagerDAL.Owner);
        }

        public Result<StoreManagerDAL> GetDAL()
        {
            RegisteredUserDAL user = User.GetDAL().Data;
            PermissionDAL permission = Permission.GetDAL().Data;
            StoreOwnerDAL owner = Owner.GetDAL().Data;

            return new Result<StoreManagerDAL>("Store manager DAL object", true, new StoreManagerDAL(user, permission, owner));
        }
    }
}
