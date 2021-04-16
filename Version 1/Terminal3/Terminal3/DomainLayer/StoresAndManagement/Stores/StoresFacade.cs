﻿using System;
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
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedOwnerID, String currentlyOwnerID, String storeID);
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
            else
            {
                return new Result<ProductDAL>($"Store ID {storeID} not found.\n", false, null);
            }
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
            else
            {
                return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
            }
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
            else
            {
                return new Result<ProductDAL>($"Store ID {storeID} not found.\n", false, null);
            }
        }
        #endregion

        #region Staff Management
        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveStoreManager(string removedOwnerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
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
