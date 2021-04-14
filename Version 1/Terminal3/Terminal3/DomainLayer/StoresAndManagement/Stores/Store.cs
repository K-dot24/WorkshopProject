using System;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoreOperations
    {
        #region Inventory Management
        Result<Boolean> AddNewProduct(Product product);
        Result<Boolean> RemoveProduct(Product product);
        Result<Boolean> EditProduct(IDictionary<String,Object> attributes, Product product);
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner);
        Result<Boolean> AddStoreManager(RegisteredUser addedManager, User currentlyOwner);
        Result<Boolean> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner);
        Result<Boolean> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions);
        Result<Boolean> GetStoreStaff();
        #endregion

        #region Policies Management
        Result<Boolean> SetPurchasePolicyAtStore(IPurchasePolicy policy);
        Result<Boolean> GetPurchasePolicyAtStore();
        Result<Boolean> SetDiscountPolicyAtStore(IDiscountPolicy policy);
        Result<Boolean> GetDiscountPolicyAtStore();
        #endregion

        #region Information
        Result<Boolean> GetStorePurchaseHistory();
        #endregion
    }
    public class Store: IStoreOperations
    {
        public StoreOwner Founder { get; }
        public LinkedList<StoreOwner> Owners { get; }
        public LinkedList<StoreManager> Managers { get; }
        public InventoryManager InventoryManager { get; }
        public PolicyManager PolicyManager { get; }
        public History History { get; }

        public Store(RegisteredUser founder)
        {
            Founder = new StoreOwner(founder,this,null);
            this.Owners = new LinkedList<StoreOwner>();
            this.Managers = new LinkedList<StoreManager>();
            this.InventoryManager = new InventoryManager();
            this.PolicyManager = new PolicyManager();
            this.History = new History();
        }



        //TODO: Implement functions
        public Result<bool> AddNewProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreManager(RegisteredUser addedManager, User currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<bool> EditProduct(IDictionary<string, object> attributes, Product product)
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetDiscountPolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetPurchasePolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetStorePurchaseHistory()
        {
            throw new NotImplementedException();
        }

        public Result<bool> GetStoreStaff()
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}
