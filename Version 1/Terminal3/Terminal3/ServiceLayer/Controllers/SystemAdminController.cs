using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer.Controllers
{
    public interface ISystemAdminInterface
    {
        Result<HistoryService> GetUserPurchaseHistory(string sysAdminID, String userID);
        Result<RegisteredUserService> AddSystemAdmin(string sysAdminID, String email);
        Result<RegisteredUserService> RemoveSystemAdmin(string sysAdminID, String email);
        Result<bool> ResetSystem(string sysAdminID);
        Result<List<Tuple<DateTime, Double>>> GetIncomeAmountGroupByDay(String start_date, String end_date, string admin_id);

    }
    public class SystemAdminController: ISystemAdminInterface
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
        public Result<HistoryService> GetUserPurchaseHistory(string sysAdminID ,String userID)
        {
            if (isSystemAdmin(sysAdminID)) {
                return StoresAndManagementInterface.GetUserPurchaseHistory(userID);
            }
            else
            {
                return new Result<HistoryService>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        /// <summary>
        /// return store phurcase history
        /// </summary>
        /// <param name="storeId"></param>
        public Result<HistoryService> GetStorePurchaseHistory(string sysAdminID, String storeId)
        {
            if (isSystemAdmin(sysAdminID))
            {
                return StoresAndManagementInterface.GetStorePurchaseHistory(sysAdminID, storeId,true);
            }
            else
            {
                return new Result<HistoryService>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        /// <summary>
        /// Adding new system admin to the system; First check if the user is registered, if so
        /// the new admin is added.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUserService> AddSystemAdmin(string sysAdminID, String email)
        {            
            if (isSystemAdmin(sysAdminID))
            {
                return  StoresAndManagementInterface.AddSystemAdmin(email);
            }
            else
            {
                return new Result<RegisteredUserService>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        /// <summary>
        /// Removing  system admin to the system; First check if the user is registered, if so
        /// the user is been removed from system admins list
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUserService> RemoveSystemAdmin(string sysAdminID, String email)
        {
            if (isSystemAdmin(sysAdminID))
            {
                return StoresAndManagementInterface.RemoveSystemAdmin(email);
            }
            else
            {
                return new Result<RegisteredUserService>($"user:{sysAdminID} is not system admin\n", false, null);
            }
        }

        public Result<bool> ResetSystem(string sysAdminID) 
        {
            if (isSystemAdmin(sysAdminID))
            {
                //return Service.ResetSystem();
                StoresAndManagementInterface.resetSystem();
                return new Result<bool>($"user:{sysAdminID} reset the system\n", true, true);
            }
            else
            {
                return new Result<bool>($"user:{sysAdminID} is not system admin\n", false, false);
            }
        }

        public Result<List<Tuple<DateTime, Double>>> GetIncomeAmountGroupByDay(String start_date, String end_date, string admin_id)
        {
            if (isSystemAdmin(admin_id))
            {
                return StoresAndManagementInterface.GetIncomeAmountGroupByDay(start_date, end_date);
            }
            else
            {
                return new Result<List<Tuple<DateTime, Double>>>($"user:{admin_id} is not system admin\n", false, null);
            }
        }

        #endregion
    }
}
