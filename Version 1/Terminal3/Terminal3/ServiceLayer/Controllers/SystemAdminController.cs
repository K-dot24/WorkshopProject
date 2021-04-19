﻿using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer.Controllers
{

    public class SystemAdminController
    {

        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public SystemAdminController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }

        #region Methods
        private bool isSystemAdmin(String userID)
        {
            return StoresAndManagementInterface.isSystemAdmin(userID).Data;
        }

        /// <summary>
        /// return user purchase history
        /// </summary>
        /// <param name="userID">ID of the requested user-user must be register</param>
        /// <param name="sysAdminID">ID of the sys admin that request - user must be register</param>
        public Result<HistoryDAL> GetUserPurchaseHistory(string sysAdminID ,String userID)
        {
            if (isSystemAdmin(sysAdminID)) {
                return StoresAndManagementInterface.GetUserPurchaseHistory(userID);
            }
            else
            {
                return new Result<HistoryDAL>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        /// <summary>
        /// return store phurcase history
        /// </summary>
        /// <param name="storeId"></param>
        public Result<HistoryDAL> GetStorePurchaseHistory(string sysAdminID, String storeId)
        {
            if (isSystemAdmin(sysAdminID))
            {
                return StoresAndManagementInterface.GetStorePurchaseHistory(sysAdminID, storeId,true);
            }
            else
            {
                return new Result<HistoryDAL>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        /// <summary>
        /// Adding new system admin to the system; First check if the user is registered, if so
        /// the new admin is added.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUserDAL> AddSystemAdmin(string sysAdminID, String email)
        {            
            if (isSystemAdmin(sysAdminID))
            {
                return  StoresAndManagementInterface.AddSystemAdmin(email);
            }
            else
            {
                return new Result<RegisteredUserDAL>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        /// <summary>
        /// Removing  system admin to the system; First check if the user is registered, if so
        /// the user is been removed from system admins list
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUserDAL> RemoveSystemAdmin(string sysAdminID, String email)
        {
            if (isSystemAdmin(sysAdminID))
            {
                return StoresAndManagementInterface.RemoveSystemAdmin(email);
            }
            else
            {
                return new Result<RegisteredUserDAL>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }
        #endregion
    }
}