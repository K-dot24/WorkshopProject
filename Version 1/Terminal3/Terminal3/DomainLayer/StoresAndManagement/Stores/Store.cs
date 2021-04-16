using System;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoreOperations
    {
        #region Inventory Management
        Result<Object> AddNewProduct(Product product);
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

        public Store(StoreDAL store)
        {
            Founder = new StoreOwner(store.Founder);
            this.Owners = new LinkedList<StoreOwner>();
            foreach(StoreOwnerDAL storeOwner in store.Owners)
            {
                this.Owners.AddLast(new StoreOwner(storeOwner));
            }
            this.Managers = new LinkedList<StoreManager>();
            foreach (StoreManagerDAL storeManager in store.Managers)
            {
                this.Managers.AddLast(new StoreManager(storeManager));
            }
            this.InventoryManager = new InventoryManager(); //TODO??
            this.PolicyManager = new PolicyManager();       //TODO??
            this.History = new History(store.History);
        }


        //TODO: Implement functions
        public Result<Object> AddNewProduct(Product product)
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

        public Result<Product> GetProduct(ProductDAL productDAL)
        {
            // this function is for the shoppingBag constructor that converts DAL object to domain objects
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
