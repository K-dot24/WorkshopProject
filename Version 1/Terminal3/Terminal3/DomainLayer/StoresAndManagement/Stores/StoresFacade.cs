using System;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoresFacade
    {
        Result<StoreDAL> OpenNewStore(RegisteredUser founder, String storeName);
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
    }

    public class StoresFacade : IStoresFacade
    {
        //TODO

        public ConcurrentDictionary<String, Store> Stores { get; }

        public StoresFacade()
        {
            //TODO: Change constructor if needed (initializer?)
            Stores = new ConcurrentDictionary<String, Store>();
        }

        public Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, Double price, int initialQuantity, String category)
        {
            Store store;
            if (Stores.TryGetValue(storeID, out store))     // Check if storeID exists
            {
                 store.AddNewProduct(userID, productName, price, initialQuantity, category);
            }
            else
            {
                return new Result<ProductDAL>($"StoreID {storeID} not found. Operation canceled.\n", false, null);
            }
        }

        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<ProductDAL> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<StoreDAL> OpenNewStore(RegisteredUser founder, string storeName)
        {
            String id = Service.GenerateId();
            Store newStore = new Store(storeName, founder);
            Stores.TryAdd(id, newStore);

            return new Result<StoreDAL>($"New store {storeName}, ID: {id} was created successfully by {founder}\n", true, StoreDAL);
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }
    }
}
