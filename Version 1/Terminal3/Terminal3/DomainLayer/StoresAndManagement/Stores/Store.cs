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
        Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID);
        Result<Boolean> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID);
        Result<Boolean> RemoveStoreManager(RegisteredUser removedManager, String currentlyOwnerID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID);
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
        public String Id { get; }
        public String Name { get; }
        public StoreOwner Founder { get; }
        public ConcurrentDictionary<String, StoreOwner> Owners { get; }
        public ConcurrentDictionary<String, StoreManager> Managers { get; }
        public InventoryManager InventoryManager { get; }
        public PolicyManager PolicyManager { get; }
        public History History { get; }

        public Store(String name, RegisteredUser founder)
        {
            Id = Service.GenerateId();
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
            if (CheckIfStoreOwner(userID) || CheckStoreManagerAndPermissions(userID, Methods.AddNewProduct))
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
            if (CheckIfStoreOwner(userID) || CheckStoreManagerAndPermissions(userID, Methods.RemoveProduct))
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
            if (CheckIfStoreOwner(userID) || CheckStoreManagerAndPermissions(userID, Methods.EditProduct))
            {
                return InventoryManager.EditProduct(productID, details);
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to edit products' information in {this.Name}\n", false, null);
            }
        }
        #endregion

        public Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, string currentlyOwnerID)
        {
            if (!CheckIfStoreOwner(futureOwner.Email) && Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner)) // Check new owner not already an owner + appointing owner is not a fraud
            {
                StoreOwner newOwner = new StoreOwner(futureOwner, this, owner);
                Owners.TryAdd(futureOwner.Email, newOwner);

                if (CheckIfStoreManager(futureOwner.Email)) //remove from managers list if needed
                {
                    Managers.TryRemove(futureOwner.Email, out _);
                }
            }
            //else failed
            return new Result<Boolean>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}) " +
                $"is not an owner at ${this.Name}.\n", false, false);
        }

        public Result<Boolean> AddStoreManager(RegisteredUser futureManager, string currentlyOwnerID)
        {
            if (!CheckIfStoreManager(futureManager.Email) && !CheckIfStoreOwner(futureManager.Email) 
                    && Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner)) // Check new manager not already an owner/manager + appointing owner is not a fraud
            {
                StoreManager newManager = new StoreManager(futureManager, this, new Permission(this), owner);
                Managers.TryAdd(futureManager.Email, newManager);
            }
            //else failed
            return new Result<Boolean>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}) " +
                $"is not an owner at ${this.Name}.\n", false, false);
        }

        public Result<bool> RemoveStoreManager(RegisteredUser removedManager, string currentlyOwnerID)
        {
            if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner) && Managers.TryGetValue(removedManager.Email, out StoreManager manager))
            {
                if (manager.AppointedBy.Equals(owner))
                {
                    Managers.TryRemove(removedManager.Email, out _);
                    return new Result<bool>($"User (Email: {removedManager.Email}) was successfully removed from store management at {this.Name}.\n", true, true);
                }
                //else failed
                return new Result<bool>($"Failed to remove user (Email: {removedManager.Email}) from store management: Unauthorized owner (Email: {currentlyOwnerID}).\n", false, false);
            }
            //else failed
            return new Result<bool>($"Failed to remove user (Email: {removedManager.Email}) from store management: Either not a manager or owner not found.\n", false, false);
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(string ownerID)
        {
            throw new NotImplementedException();
        }

        public Result<object> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<object> GetPurchasePolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<object> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<object> GetDiscountPolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<object> GetStorePurchaseHistory()
        {
            throw new NotImplementedException();
        }



        #region Private Functions
        private Boolean CheckIfStoreOwner(String userID)    // If user is Store Owner => has permissions
        {
            return Owners.ContainsKey(userID);
        }

        private Boolean CheckIfStoreManager(String userID)
        {
            return Managers.ContainsKey(userID);
        }

        private Boolean CheckStoreManagerAndPermissions(String userID, Methods method)    // Check if userID is Store Manager + has certain permission
        {
            return Managers.TryGetValue(userID, out StoreManager manager) && manager.Permission.functionsBitMask[(int)method];
        }
        #endregion
    }
}
