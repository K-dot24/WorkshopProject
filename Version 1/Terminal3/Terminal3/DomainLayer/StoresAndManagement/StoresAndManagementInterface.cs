using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DALobjects;
namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        //TODO: add all relevant functions to interface

        Result<StoreDAL> OpenNewStore(String storeName, String userID);

        #region Inventory Management
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
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

        public Result<StoreDAL> OpenNewStore(String storeName, String userID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser founder))  // Check if userID is a registered user
            {
                // Open store
                return StoresFacade.OpenNewStore(founder, userID);
            }
            //else
            return new Result<StoreDAL>($"Failed to open store {storeName}: {userID} is not a registered user.\n", false, null);
        }

        public Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            return StoresFacade.AddProductToStore(userID, storeID, productName, price, initialQuantity, category, keywords);
        }

        public Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details)
        {
            return StoresFacade.EditProductDetails(userID, storeID, productID, details);
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails)
        {
            return StoresFacade.SearchProduct(productDetails);
        }


        public Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(addedOwnerID, out RegisteredUser futureOwner))  // Check if addedOwnerID is a registered user
            {
                return StoresFacade.AddStoreOwner(futureOwner, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to appoint store owner: {addedOwnerID} is not a registered user.\n", false, false);
        }

        public Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(addedManagerID, out RegisteredUser futureManager))  // Check if addedManagerID is a registered user
            {
                return StoresFacade.AddStoreOwner(futureManager, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to appoint store manager: {addedManagerID} is not a registered user.\n", false, false);
        }

        public Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.ContainsKey(removedManagerID))  // Check if addedManagerID is a registered user
            {
                return StoresFacade.RemoveStoreManager(removedManagerID, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to remove store manager: {removedManagerID} is not a registered user.\n", false, false);
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            Result<History> res = StoresFacade.GetStorePurchaseHistory(ownerID, storeID);
            if (res.ExecStatus)
            {
                return new Result<HistoryDAL>("Store purchase history\n" , true ,res.Data.GetDAL().Data);
            }
            return new Result<HistoryDAL>(res.Message, false, null);
        }
    }
}
