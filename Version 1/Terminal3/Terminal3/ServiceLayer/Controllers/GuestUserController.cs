﻿using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer.Controllers
{
    public interface IGuestUserInterface
    {
        Result<UserDAL> EnterSystem();
        void ExitSystem(String userID);
        Result<RegisteredUserDAL> Register(string email, string password);
        Result<StoreDAL> SearchStore(IDictionary<String, Object> details);
        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID);
        Result<ShoppingCartDAL> GetUserShoppingCart(String userID);
        Result<Boolean> UpdateShoppingCart(String userID, String storeID, String productID, int quantity);
        Result<ShoppingCartDAL> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);
        Result<HistoryDAL> GetUserPurchaseHistory(String userID);
        Result<double> GetTotalShoppingCartPrice(String userID);
    }
    //Basic functionality The every user can preform
    public class GuestUserController: IGuestUserInterface
    {
        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public GuestUserController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }
        #region Methods
        public void ExitSystem(String userID)
        {
            StoresAndManagementInterface.ExitSystem(userID);
        }
        public Result<UserDAL> EnterSystem()
        {
            return StoresAndManagementInterface.EnterSystem();
        }
        public Result<RegisteredUserDAL> Register(string email, string password){return StoresAndManagementInterface.Register(email,password); }
        public Result<StoreDAL> SearchStore(IDictionary<String, Object> details) { return StoresAndManagement.SearchStore(details); }
        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails) { return StoresAndManagementInterface.SearchProduct(productDetails); }
        public Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID) { return StoresAndManagementInterface.AddProductToCart(userID, ProductID, ProductQuantity, StoreID); }   // Redundent ?
        public Result<ShoppingCartDAL> GetUserShoppingCart(String userID) { return StoresAndManagementInterface.GetUserShoppingCart(userID); }
        public Result<Boolean> UpdateShoppingCart(String userID, String storeID, String productID, int quantity) { return StoresAndManagementInterface.UpdateShoppingCart(userID, storeID, productID, quantity); }
        public Result<ShoppingCartDAL> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            return StoresAndManagementInterface.Purchase(userID, paymentDetails, deliveryDetails);
        }
        public Result<HistoryDAL> GetUserPurchaseHistory(String userID) { return StoresAndManagementInterface.GetUserPurchaseHistory(userID); }
        public Result<double> GetTotalShoppingCartPrice(String userID) { return StoresAndManagementInterface.GetTotalShoppingCartPrice(userID); }
        #endregion

    }
}
