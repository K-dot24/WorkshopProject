using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoresFacade
    {
        Result<Store> OpenNewStore(RegisteredUser founder, String storeName);

        #region Inventory Management
        Result<Product> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<Product> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<Product>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);

        Result<Store> GetStore(String storeID);

        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(string ownerID, string storeID);
        Result<History> GetStorePurchaseHistory(String userID, String storeID, bool sysAdmin);
        #endregion
    }

    public class StoresFacade : IStoresFacade
    {
        public ConcurrentDictionary<String, Store> Stores { get; }

        //TODO: Change constructor if needed (initializer?)
        public StoresFacade()
        {
            Stores = new ConcurrentDictionary<String, Store>();
        }

        //TODO: Implement all functions

        #region Inventory Management
        public Result<Product> AddProductToStore(String userID, String storeID, String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.AddNewProduct(userID, productName, price, initialQuantity, category, keywords);                
            }
            //else failed
            return new Result<Product>($"Store ID {storeID} not found.\n", false, null);
            
        }
        
        public Result<Boolean> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            if (Stores.TryGetValue(storeID, out Store store))
            {
                Result<Product> res = store.RemoveProduct(userID, productID);
                if (res.ExecStatus)
                {
                    return new Result<Boolean>(res.Message, res.ExecStatus, true);
                }
                //else failed
                return new Result<Boolean>(res.Message, res.ExecStatus, false);               
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }
        
        public Result<Product> EditProductDetails(string userID, string storeID, string productID, IDictionary<String, Object> details)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.EditProduct(userID, productID, details);          
            }
            //else failed
            return new Result<Product>($"Store ID {storeID} not found.\n", false, null);          
        }
        #endregion

        #region Staff Management
        public Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.AddStoreOwner(futureOwner, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }
        
        public Result<Boolean> AddStoreManager(RegisteredUser futureManager, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.AddStoreManager(futureManager, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<Boolean> RemoveStoreManager(String removedManagerID, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.RemoveStoreManager(removedManagerID, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<List<Product>> SearchProduct(IDictionary<String, Object> searchAttributes)
        {
            List<Product> searchResult = new List<Product>();
            foreach(Store store in this.Stores.Values)
            {
                Result<List<Product>> storeResult = store.SearchProduct(searchAttributes);
                if (storeResult.ExecStatus)
                {
                    searchResult.AddRange(storeResult.Data);
                }
            }
            if (searchResult.Count > 0) {
                return new Result<List<Product>>($"{searchResult.Count } items has been found\n",true, searchResult); 
            }
            else{
                return new Result<List<Product>>($"No has been found\n", false, null);
            }

        }

        public Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(string userID, string storeID)
        {            
            if(Stores.TryGetValue(storeID, out Store store))
            {
                return store.GetStoreStaff(userID);
            }
            return new Result<Dictionary<IStoreStaff, Permission>>("The given store ID does not exists", false, null);
            
        }
        #endregion

        public Result<History> GetStorePurchaseHistory(string userID, string storeID,bool sysAdmin)
        {
            if(Stores.TryGetValue(storeID, out Store store))
            {
                return store.GetStorePurchaseHistory(userID,sysAdmin);
            }
            return new Result<History>("Store Id does not exists\n", false, null);
        }

        public Result<Store> OpenNewStore(RegisteredUser founder, string storeName)
        {
            Store newStore = new Store(storeName, founder);
            Stores.TryAdd(newStore.Id, newStore);

            return new Result<Store>($"New store {storeName}, ID: {newStore.Id} was created successfully by {founder}\n", true, newStore);
        }


        public Result<bool> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.SetPermissions(managerID, ownerID, permissions);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.RemovePermissions(managerID, ownerID, permissions);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID)
        {
            if(Stores.TryGetValue(storeID , out Store store))
            {
                return store.GetProductReview(productID);
            }
            return new Result<ConcurrentDictionary<string, string>>("Store does not exists\n", false, null);
        }

        public Result<Store> GetStore(String storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))
            {
                return new Result<Store>("", true, store);
            }
            return new Result<Store>("Store does not exists\n", false, null);
        }

    }
}
