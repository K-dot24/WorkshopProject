using System;
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
        /// <summary>
        /// return user purchase history
        /// </summary>
        /// <param name="userID">user must be register</param>
        public Result<HistoryDAL> getUserPurchaseHistory(String userID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// return store phurcase history
        /// </summary>
        /// <param name="storeId"></param>
        public Result<HistoryDAL> getStorePurchaseHistory(string storeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adding new system admin to the system; First check if the user is registered, if so
        /// the new admin is added.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUserDAL> AddSystemAdmin(String email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removing  system admin to the system; First check if the user is registered, if so
        /// the user is been removed from system admins list
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUserDAL> RemoveSystemAdmin(String email)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
