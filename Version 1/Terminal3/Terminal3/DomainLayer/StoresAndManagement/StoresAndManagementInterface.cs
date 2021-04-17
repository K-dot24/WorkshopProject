using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        //TODO: add all relevant functions to interface

        Result<StoreDAL> OpenNewStore(String storeName, String userID);

        #region Inventory Management
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);

        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        #endregion

        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
    }
    public class StoresAndManagementInterface : IStoresAndManagementInterface
    {
        public StoresFacade StoresFacade { get; }
        public UsersAndPermissionsFacade UsersAndPermissionsFacade { get; }

        public StoresAndManagementInterface()
        {
            //TODO: Change constructor if needed (initializer?)
            StoresFacade = new StoresFacade();
            UsersAndPermissionsFacade = new UsersAndPermissionsFacade();
        }


        //TODO: Implement all functions

        Result<StoreDAL> OpenNewStore(String storeName, String userID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser founder))  // Check if userID is a registered user
            {
                // Open store
                return StoresFacade.OpenNewStore(founder, userID);
            }
            //else
            return new Result<StoreDAL>($"Failed to open store {storeName}: {userID} is not a registered user.\n", false, null);
        }

        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category)
        {
            return StoresFacade.AddProductToStore(userID, storeID, productName, price, initialQuantity, category);
        }

        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details)
        {
            return StoresFacade.EditProductDetails(userID, storeID, productID, details);
        }

        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails)
        {
            return StoresFacade.SearchProduct(productDetails);
        }


        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(addedOwnerID, out RegisteredUser futureOwner))  // Check if addedOwnerID is a registered user
            {
                return StoresFacade.AddStoreOwner(futureOwner, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to appoint store owner: {addedOwnerID} is not a registered user.\n", false, false);
        }

        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(addedManagerID, out RegisteredUser futureManager))  // Check if addedManagerID is a registered user
            {
                return StoresFacade.AddStoreOwner(futureManager, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to appoint store manager: {addedManagerID} is not a registered user.\n", false, false);
        }
        
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(removedManagerID, out RegisteredUser removedManager))  // Check if addedManagerID is a registered user
            {
                return StoresFacade.RemoveStoreManager(removedManager, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to remove store manager: {removedManagerID} is not a registered user.\n", false, false);
        }



    }
}
