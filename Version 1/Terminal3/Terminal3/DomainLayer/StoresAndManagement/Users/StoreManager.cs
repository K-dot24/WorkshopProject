using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreManager : IStoreStaff , IStoreOperations
    {
        public RegisteredUser User { get; }
        public Store Store { get; }
        public Permission Permission { get; }
        public StoreOwner Owner { get; }

        public StoreManager(RegisteredUser user, Store store, Permission permission , StoreOwner storeOwner)
        {
            this.User = user;
            this.Store = store;
            this.Permission = permission;
            this.Owner = storeOwner;
        }        

        public Result<object> AddNewProduct(Product product)
        {
            return Permission.AddNewProduct(product);
        }

        public Result<object> RemoveProduct(Product product)
        {
            return Permission.RemoveProduct(product);
        }

        public Result<object> EditProduct(IDictionary<string, object> attributes, Product product)
        {
            return Permission.EditProduct(attributes, product);
        }

        public Result<object> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner)
        {
            return Permission.AddStoreOwner(addedOwner, currentlyOwner);
        }

        public Result<object> AddStoreManager(RegisteredUser addedManager, User currentlyOwner)
        {
            return Permission.AddStoreManager(addedManager, currentlyOwner);
        }

        public Result<object> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner)
        {
            return Permission.RemoveStoreManager(addedManager, currentlyOwner);
        }

        public Result<object> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions)
        {
            return Permission.SetPermissions(manager, owner, permissions);
        }

        public Result<object> GetStoreStaff()
        {
            return Permission.GetStoreStaff();
        }

        public Result<object> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            return Permission.SetPurchasePolicyAtStore(policy);
        }

        public Result<object> GetPurchasePolicyAtStore()
        {
            return Permission.GetPurchasePolicyAtStore();
        }

        public Result<object> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            return Permission.SetDiscountPolicyAtStore(policy);
        }

        public Result<object> GetDiscountPolicyAtStore()
        {
            return Permission.GetDiscountPolicyAtStore();
        }

        public Result<object> GetStorePurchaseHistory()
        {
            return Permission.GetStorePurchaseHistory();
        }
    }
}
