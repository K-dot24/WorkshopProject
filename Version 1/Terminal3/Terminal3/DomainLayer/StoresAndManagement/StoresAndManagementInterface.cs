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
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
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
            RegisteredUser founder;
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out founder))  // Check if userID is a registered user
            {
                // Open store
                return StoresFacade.OpenNewStore(founder, userID);
            }
            else
            {
                return new Result<StoreDAL>($"{userID} is not a registered user. Unable to open store {storeName}\n", false, null);
            }
        }

        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category)
        {
            
        }


    }
}
