using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{ 

    public enum Methods : int
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
        GetStorePurchaseHistory = 12,
        #endregion

        AllPermissions = 777
    }

    public class Permission
    {

        public Boolean[] functionsBitMask { get; }
        public bool isOwner { get; }

        public Permission(bool isOwner=false)
        {
            this.isOwner = isOwner;
            functionsBitMask = new Boolean[13];
            functionsBitMask[(int)Methods.GetStoreStaff] = true;    //requierment 4.5
        }

        // For loading from database
        public Permission(Boolean[] functionsBitMask)
        {
            this.isOwner = false;
            this.functionsBitMask = functionsBitMask;
        }

        public Result<Boolean> SetPermission(Methods method, Boolean active)
        {
            functionsBitMask[(int)method] = active;
            return new Result<Boolean>($"Permission for {method} set successfully to {active}\n", true, true);
        }

        public Result<Boolean> SetPermission(int method, Boolean active)
        {
            functionsBitMask[method] = active;
            return new Result<Boolean>($"Permission for {(Methods)method} set successfully to {active}\n", true, true);
        }

        public Result<Boolean> SetAllMethodesPermitted()
        {
            for(int i = 0; i<functionsBitMask.Length; i++)
            {
                functionsBitMask[i] = true;
            }
            
            return new Result<Boolean>("All methodes are permitted\n", true, true);
        }

        public Result<PermissionService> GetDAL()
        {
            return new Result<PermissionService>("Permission DAL object", true, new PermissionService(this.functionsBitMask,isOwner));
        }

    }
}
