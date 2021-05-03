﻿using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer.Controllers
{
    public interface IRegisteredUserInterface
    {
        Result<RegisteredUserService> Login(String email, String password);
        Result<Boolean> LogOut(String email);
        Result<StoreService> OpenNewStore(String storeName, String userID);
        Result<Boolean> AddProductReview(String userID, String storeID, String productID, String review);
    }

    //Extend GuestUser functionality to the st of function that RegisterUser can do
    public class RegisteredUserController : GuestUserController , IRegisteredUserInterface
    {

        //Constructor
        public RegisteredUserController(IStoresAndManagementInterface storesAndManagementInterface):base(storesAndManagementInterface){}
        
        #region Methods
        public Result<RegisteredUserService> Login(String email, String password) {
            return StoresAndManagementInterface.Login(email,password);
        }
        public Result<Boolean> LogOut(String email) { return StoresAndManagementInterface.LogOut(email); }
        public Result<StoreService> OpenNewStore(String storeName, String userID) { return StoresAndManagementInterface.OpenNewStore(storeName, userID); }
        public Result<bool> AddProductReview(string userID, string storeID, string productID, string review)
        {
            return StoresAndManagementInterface.AddProductReview(userID, storeID, productID, review);
        }
        #endregion
    }
}
