﻿using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using XUnitTestTerminal3.AcceptanceTests.Utils;

namespace Terminal3.ServiceLayer
{   
    //System Real interface with DAL
    public interface IECommerceSystemInterface
    {

        #region System related operations
        Result<Boolean> ResetSystem();

        #endregion

        #region User related operations
        Result<Boolean> Register(String email, String password);


        Result<UserDAL> Login(String email, String password);

        Result<Boolean> LogOut(String email);

        //TODO: refine requirement
        Result<Object> SearchStore(IDictionary<String, Object> details);

        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);

        Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID);   // Redundent ?

        Result<ShoppingCartDAL> GetUserShoppingCart(String userID);

        Result<Dictionary<String, int>> GetUserShoppingBag(String userID, String shoppingBagID); //Dictionary<pid , countity>

        Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity);

        Result<Object> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);

        Result<HistoryDAL> GetUserPurchaseHistory(String userID);

        Result<int> GetTotalShoppingCartPrice(String userID);
        #endregion

        #region Store related operations
        Result<StoreDAL> OpenNewStore(String storeName, String userID);
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
        Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID);

        #endregion
    }
    public class ECommerceSystem : IECommerceSystemInterface
    {
        public Result<bool> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            throw new NotImplementedException();
        }

        public Result<ProductDAL> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            throw new NotImplementedException();
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

        public Result<int> GetTotalShoppingCartPrice(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetUserPurchaseHistory(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<string, int>> GetUserShoppingBag(string userID, string shoppingBagID)
        {
            throw new NotImplementedException();
        }

        public Result<ShoppingCartDAL> GetUserShoppingCart(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<UserDAL> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Result<bool> LogOut(string email)
        {
            throw new NotImplementedException();
        }

        public Result<StoreDAL> OpenNewStore(string storeName, string userID)
        {
            throw new NotImplementedException();
        }

        public Result<object> Purchase(string userID, IDictionary<string, object> paymentDetails, IDictionary<string, object> deliveryDetails)
        {
            throw new NotImplementedException();
        }

        public Result<bool> Register(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> ResetSystem()
        {
            throw new NotImplementedException();
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<string, object> productDetails)
        {
            throw new NotImplementedException();
        }

        public Result<object> SearchStore(IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateShoppingCart(string userID, string shoppingBagID, string productID, int quantity)
        {
            throw new NotImplementedException();
        }

        public Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();

        }
    }
}
