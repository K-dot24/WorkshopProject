using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{ 

    public enum methods : int
    {
        #region Inventory Management
        AddNewProduct = 0,
        RemoveProduct = 1,
        EditProduct = 2,
        #endregion

        #region Staff Management
        AddStoreOwner = 3,
        AddStoreManager = 4,
        RemoveStoreManager = 5,
        SetPermissions = 6,
        GetStoreStaff = 7,
        #endregion

        #region Policies Management
        SetPurchasePolicyAtStore = 8,
        GetPurchasePolicyAtStore = 9,
        SetDiscountPolicyAtStore = 10,
        GetDiscountPolicyAtStore = 11,
        #endregion

        #region Information
        GetStorePurchaseHistory = 12
        #endregion

    }
    //Access proxy for store instance
    public class Permission: IStoreOperations
    {
        //TODO: figure out how to set permissions flags for each operation and add machanisem for it

        public Store Store { get; }
        public Boolean[] functionsBitMask { get; }

        public Permission(Store store)
        {
            this.Store = store;
            functionsBitMask = new Boolean[13];
            functionsBitMask[(int)methods.GetStoreStaff] = true;    //requierment 4.5
        }

        public Permission(PermissionDAL permissionDAL)
        {
            this.Store = Mapper.GetStore(permissionDAL.Store);     //TODO
            this.functionsBitMask = permissionDAL.functionsBitMask;
        }

        public Result<object> AddNewProduct(Product product)
        {
            return ((IStoreOperations)Store).AddNewProduct(product);
        }

        public Result<object> RemoveProduct(Product product)
        {
            return ((IStoreOperations)Store).RemoveProduct(product);
        }

        public Result<object> EditProduct(IDictionary<string, object> attributes, Product product)
        {
            return ((IStoreOperations)Store).EditProduct(attributes, product);
        }

        public Result<object> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner)
        {
            return ((IStoreOperations)Store).AddStoreOwner(addedOwner, currentlyOwner);
        }

        public Result<object> AddStoreManager(RegisteredUser addedManager, User currentlyOwner)
        {
            return ((IStoreOperations)Store).AddStoreManager(addedManager, currentlyOwner);
        }

        public Result<object> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner)
        {
            return ((IStoreOperations)Store).RemoveStoreManager(addedManager, currentlyOwner);
        }

        public Result<object> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions)
        {
            return ((IStoreOperations)Store).SetPermissions(manager, owner, permissions);
        }

        public Result<object> GetStoreStaff()
        {
            return ((IStoreOperations)Store).GetStoreStaff();
        }

        public Result<object> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            return ((IStoreOperations)Store).SetPurchasePolicyAtStore(policy);
        }

        public Result<object> GetPurchasePolicyAtStore()
        {
            return ((IStoreOperations)Store).GetPurchasePolicyAtStore();
        }

        public Result<object> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            return ((IStoreOperations)Store).SetDiscountPolicyAtStore(policy);
        }

        public Result<object> GetDiscountPolicyAtStore()
        {
            return ((IStoreOperations)Store).GetDiscountPolicyAtStore();
        }

        public Result<object> GetStorePurchaseHistory()
        {
            return ((IStoreOperations)Store).GetStorePurchaseHistory();
        }

        public Result<PermissionDAL> GetDAL()
        {
            StoreDAL storeDAL = Store.GetDAL().Data;
            return new Result<PermissionDAL>("Permmision DAL object", true, new PermissionDAL(storeDAL, functionsBitMask));
        }
    }
}
