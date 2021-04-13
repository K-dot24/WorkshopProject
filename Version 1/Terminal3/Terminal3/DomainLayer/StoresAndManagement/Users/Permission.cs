using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{ 
     //Access proxy for store instance
public class Permission: IStoreOperations
    {
        //TODO: figure out how to set permissions flags for each operation and add machanisem for it

        public Store Store { get; }

        public Permission(Store store)
        {
            this.Store = store;
        }
        
        public Result<bool> AddNewProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Result<bool> EditProduct(IDictionary<string, object> attributes, Product product)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreManager(RegisteredUser addedManager, User currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetStoreStaff()
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetPurchasePolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetDiscountPolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetStorePurchaseHistory()
        {
            throw new NotImplementedException();
        }
    }
}
