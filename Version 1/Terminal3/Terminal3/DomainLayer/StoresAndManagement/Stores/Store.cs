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
        //TODO: Update functions sigs

        #region Inventory Management
        Result<Product> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category);
        Result<Product> RemoveProduct(String userID, String productID);
        Result<Product> EditProduct(String userID, String productID, IDictionary<String, Object> details);
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
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
            Founder = new StoreOwner(founder, this, null);
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            InventoryManager = new InventoryManager();
            PolicyManager = new PolicyManager();
            History = new History();

            //Add founder to list of owners
            Owners.TryAdd(founder.Email, Founder);
        }

        //TODO: Implement all functions

        #region Inventory Management
        public Result<Product> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category)
        {
            if (CheckStoreOwner(userID) || CheckStoreManager(userID, Methods.AddNewProduct))
            {
                return InventoryManager.AddNewProduct(productName, price, initialQuantity, category);
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to add new product to {this.Name}\n", false, null);
            }
        }
        public Result<Product> RemoveProduct(String userID, String productID)
        {
            if (CheckStoreOwner(userID) || CheckStoreManager(userID, Methods.RemoveProduct))
            {
                return InventoryManager.RemoveProduct(productID);
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to remove products from {this.Name}\n", false, null);
            }
        }
        public Result<Product> EditProduct(String userID, String productID, IDictionary<String, Object> details)
        {
            if (CheckStoreOwner(userID) || CheckStoreManager(userID, Methods.EditProduct))
            {
                return InventoryManager.EditProduct(productID, details);
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to edit products' information in {this.Name}\n", false, null);
            }
        }
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID)
        {

        }

        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedOwnerID, String currentlyOwnerID, String storeID);
        #endregion



        #region Private Functions
        private Boolean CheckStoreOwner(String userID)    // If user is Store Owner => has permissions
        {
            return Owners.TryGetValue(userID, out _);
        }

        private Boolean CheckStoreManager(String userID, Methods method)    // Check if userID is Store Manager + has certain permission
        {
            return Managers.TryGetValue(userID, out StoreManager manager) && CheckPermission(manager, method);
        }
        
        private Boolean CheckPermission(StoreManager manager, Methods method)
        {
            return manager.Permission.functionsBitMask[(int)method];
        }
        #endregion
    }
}
