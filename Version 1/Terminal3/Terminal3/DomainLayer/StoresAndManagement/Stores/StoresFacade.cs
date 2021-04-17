using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoresFacade
    {
        Result<StoreDAL> OpenNewStore(RegisteredUser founder, String storeName);

        #region Inventory Management
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);

        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(RegisteredUser removedManager, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        #endregion

        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
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
        public Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, Double price, int initialQuantity, String category)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                Result<Product> res = store.AddNewProduct(userID, productName, price, initialQuantity, category);
                if (res.ExecStatus)
                {
                    //TODO: Complete with DAL object
                    return new Result<ProductDAL>(res.Message, res.ExecStatus, ...);
                }
                else
                {
                    return new Result<ProductDAL>(res.Message, res.ExecStatus, null);
                }
            }
            //else
            return new Result<ProductDAL>($"Store ID {storeID} not found.\n", false, null);
            
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
                else
                {
                    return new Result<Boolean>(res.Message, res.ExecStatus, false);
                }
            }
            //else
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }
        
        public Result<ProductDAL> EditProductDetails(string userID, string storeID, string productID, IDictionary<String, Object> details)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                Result<Product> res = store.EditProduct(userID, productID, details);
                if (res.ExecStatus)
                {
                    //TODO: Complete with DAL object
                    return new Result<ProductDAL>(res.Message, res.ExecStatus, ...);
                }
                else
                {
                    return new Result<ProductDAL>(res.Message, res.ExecStatus, null);
                }
            }
            //else
            return new Result<ProductDAL>($"Store ID {storeID} not found.\n", false, null);           
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

        public Result<Boolean> RemoveStoreManager(RegisteredUser removedManager, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.RemoveStoreManager(removedManager, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails)
        {
            ProductSearchAttributes searchAttributes = ObjectDictionaryMapper<ProductSearchAttributes>.GetObject(productDetails);
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
                return new Result<List<ProductDAL>>($"{searchResult.Count } items has been found\n",true,Service.ConvertToDAL<Product>(searchResult));
            }
            else{
                return new Result<List<ProductDAL>>($"No has been found\n", false, null);
            }

        }

        public Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }


        public Result<StoreDAL> OpenNewStore(RegisteredUser founder, string storeName)
        {
            String id = Service.GenerateId();
            Store newStore = new Store(storeName, founder);
            Stores.TryAdd(id, newStore);

            //TODO: Complete with DAL object
            return new Result<StoreDAL>($"New store {storeName}, ID: {id} was created successfully by {founder}\n", true, StoreDAL);
        }


        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }
    }
}
