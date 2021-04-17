using System;
using System.Collections.Generic;
using System.Text;
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

        public Boolean isPermitted(int method)
        {
            return functionsBitMask[method];
        }

        public Result<object> NoPermissionResult(String method)
        {
            return new Result<object>("Manager does not have permission to " + method, false, null);
        }

        public Result<object> AddNewProduct(Product product)
        {
            if (isPermitted((int)methods.AddNewProduct))
            {
                return ((IStoreOperations)Store).AddNewProduct(product);
            }
            return NoPermissionResult("add new product");
        }

        public Result<object> RemoveProduct(Product product)
        {
            if (isPermitted((int)methods.RemoveProduct))
            {
                return ((IStoreOperations)Store).RemoveProduct(product);
            }
            return NoPermissionResult("remove product");
        }

        public Result<object> EditProduct(IDictionary<string, object> attributes, Product product)
        {
            if (isPermitted((int)methods.EditProduct))
            {
                return ((IStoreOperations)Store).EditProduct(attributes, product);
            }
            return NoPermissionResult("edit product");
        }

        public Result<object> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner)
        {
            if (isPermitted((int)methods.AddStoreOwner))
            {
                return ((IStoreOperations)Store).AddStoreOwner(addedOwner, currentlyOwner);
            }
            return NoPermissionResult("add store owner");                
        }

        public Result<object> AddStoreManager(RegisteredUser addedManager, User currentlyOwner)
        {
            if (isPermitted((int)methods.AddStoreManager))
            {
                return ((IStoreOperations)Store).AddStoreManager(addedManager, currentlyOwner);
            }
            return NoPermissionResult("add store manager");                
        }

        public Result<object> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner)
        {
            if (isPermitted((int)methods.RemoveStoreManager))
            {
                return ((IStoreOperations)Store).RemoveStoreManager(addedManager, currentlyOwner);
            }
            return NoPermissionResult("remove store manager");                
        }

        public Result<object> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions)
        {
            if (isPermitted((int)methods.SetPermissions))
            {
                return ((IStoreOperations)Store).SetPermissions(manager, owner, permissions);
            }
            return NoPermissionResult("set permissions");              
        }

        public Result<object> GetStoreStaff()
        {
            if (isPermitted((int)methods.GetStoreStaff))
            {
                return ((IStoreOperations)Store).GetStoreStaff();
            }
            return NoPermissionResult("get store staff");                
        }

        public Result<object> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            if (isPermitted((int)methods.SetPurchasePolicyAtStore))
            {
                return ((IStoreOperations)Store).SetPurchasePolicyAtStore(policy);
            }
            return NoPermissionResult("set purchase policy at store");
        }

        public Result<object> GetPurchasePolicyAtStore()
        {
            if (isPermitted((int)methods.GetPurchasePolicyAtStore))
            {
                return ((IStoreOperations)Store).GetPurchasePolicyAtStore();
            }
            return NoPermissionResult("get purchase policy at store");
        }

        public Result<object> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            if (isPermitted((int)methods.SetDiscountPolicyAtStore))
            {
                return ((IStoreOperations)Store).SetDiscountPolicyAtStore(policy);
            }
            return NoPermissionResult("set discount policy at store");                
        }

        public Result<object> GetDiscountPolicyAtStore()
        {
            if (isPermitted((int)methods.GetDiscountPolicyAtStore))
            {
                return ((IStoreOperations)Store).GetDiscountPolicyAtStore();
            }
            return NoPermissionResult("get discount policy at store");
                
        }

        public Result<object> GetStorePurchaseHistory()
        {
            if (isPermitted((int)methods.GetStorePurchaseHistory))
            {
                return ((IStoreOperations)Store).GetStorePurchaseHistory();
            }
            return NoPermissionResult("get store purchase history");
                
        }
    }
}
