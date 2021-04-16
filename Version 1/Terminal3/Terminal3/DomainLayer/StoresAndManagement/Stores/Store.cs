using System;
using System.Collections.Concurrent;
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
        Result<Object> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category);
        Result<Object> RemoveProduct(Product product);
        Result<Object> EditProduct(IDictionary<String,Object> attributes, Product product);
        #endregion

        #region Staff Management
        Result<Object> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner);
        Result<Object> AddStoreManager(RegisteredUser addedManager, User currentlyOwner);
        Result<Object> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner);
        Result<Object> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions);
        Result<Object> GetStoreStaff();
        #endregion

        #region Policies Management
        Result<Object> SetPurchasePolicyAtStore(IPurchasePolicy policy);
        Result<Object> GetPurchasePolicyAtStore();
        Result<Object> SetDiscountPolicyAtStore(IDiscountPolicy policy);
        Result<Object> GetDiscountPolicyAtStore();
        #endregion

        #region Information
        Result<Object> GetStorePurchaseHistory();
        #endregion
    }
    public class Store: IStoreOperations
    {
        public String StoreID { get; }
        public String Name { get; }
        public StoreOwner Founder { get; }
        public ConcurrentDictionary<String, StoreOwner> Owners { get; }
        public ConcurrentDictionary<String, StoreManager> Managers { get; }
        public InventoryManager InventoryManager { get; }
        public PolicyManager PolicyManager { get; }
        public History History { get; }

        public Store(String name, RegisteredUser founder)
        {
            StoreID = Service.GenerateId();
            Name = name;
            Founder = new StoreOwner(founder,this,null);
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            InventoryManager = new InventoryManager();
            PolicyManager = new PolicyManager();
            History = new History();
        }



        //TODO: Implement functions
        public Result<Product> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category)
        {
            throw new NotImplementedException();
        }

        public Result<Object> AddStoreManager(RegisteredUser addedManager, User currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<Object> AddStoreOwner(RegisteredUser addedOwner, User currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<Object> EditProduct(IDictionary<String, Object> attributes, Product product)
        {
            throw new NotImplementedException();
        }

        public Result<Object> GetDiscountPolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<Object> GetPurchasePolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<Object> GetStorePurchaseHistory()
        {
            throw new NotImplementedException();
        }

        public Result<Object> GetStoreStaff()
        {
            throw new NotImplementedException();
        }

        public Result<Object> RemoveProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Result<Object> RemoveStoreManager(RegisteredUser addedManager, RegisteredUser currentlyOwner)
        {
            throw new NotImplementedException();
        }

        public Result<Object> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<Object> SetPermissions(RegisteredUser manager, RegisteredUser owner, Permission permissions)
        {
            throw new NotImplementedException();
        }

        public Result<Object> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}
