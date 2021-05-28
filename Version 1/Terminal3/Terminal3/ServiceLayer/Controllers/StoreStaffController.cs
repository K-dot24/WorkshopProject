using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.ServiceLayer.Controllers
{
    public interface IStoreStaffInterface
    {
        Result<ProductService> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductService> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<List<Tuple<IStoreStaffService, PermissionService>>> GetStoreStaff(String ownerID, String storeID);
        Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID,Boolean isSystemAdmin=false);
        Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID);
        Result<Boolean> RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID);
        Result<Boolean> CloseStore(string storeId, string userID);
        Result<StoreService> ReOpenStore(string storeId, string userID);
        Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info);
        Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info, String id);
        Result<bool> AddDiscountCondition(string storeId, Dictionary<string, object> info, String id);
        Result<bool> RemoveDiscountPolicy(string storeId, String id);
        Result<bool> RemoveDiscountCondition(string storeId, String id);
        Result<bool> EditDiscountPolicy(string storeId, Dictionary<string, object> info, String id);
        Result<bool> EditDiscountCondition(string storeId, Dictionary<string, object> info, String id);
        Result<IDiscountPolicyData> GetDiscountPolicyData(string storeId);
        Result<IPurchasePolicyData> GetPurchasePolicyData(string storeId);
        Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info);
        Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info, String id);
        Result<bool> RemovePurchasePolicy(string storeId, String id);
        Result<bool> EditPurchasePolicy(string storeId, Dictionary<string, object> info, string id);

        Result<List<Tuple<DateTime, Double>>> GetIncomeAmountGroupByDay(String start_date, String end_date, String store_id, String owner_id); 

    }
    public class StoreStaffController : IStoreStaffInterface
    {
        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public StoreStaffController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }
        #region Methods

        public Result<ProductService> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null) { return StoresAndManagementInterface.AddProductToStore(userID, storeID, productName, price, initialQuantity, category,keywords); }
        public Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID) { return StoresAndManagementInterface.RemoveProductFromStore(userID,  storeID,  productID); }
        public Result<ProductService> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details) { return StoresAndManagementInterface.EditProductDetails(userID, storeID,  productID, details); }
        public Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID) { return StoresAndManagementInterface.AddStoreOwner(addedOwnerID, currentlyOwnerID, storeID); }
        public Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID) { return StoresAndManagementInterface.AddStoreManager(addedManagerID, currentlyOwnerID, storeID); }
        public Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions) { return StoresAndManagementInterface.SetPermissions(storeID, managerID, ownerID, permissions); }
        public Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions) { return StoresAndManagementInterface.RemovePermissions(storeID,  managerID,  ownerID,  permissions); }
        public Result<List<Tuple<IStoreStaffService, PermissionService>>> GetStoreStaff(String ownerID, String storeID) { return StoresAndManagementInterface.GetStoreStaff(ownerID, storeID); }
        public Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID, Boolean isSystemAdmin = false) { return StoresAndManagementInterface.GetStorePurchaseHistory(ownerID, storeID); }
        public Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID) { return StoresAndManagementInterface.RemoveStoreManager(removedManagerID, currentlyOwnerID, storeID); }
        public Result<Boolean> RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID) { return StoresAndManagementInterface.RemoveStoreOwner(removedOwnerID, currentlyOwnerID, storeID); }
        public Result<Boolean> CloseStore(string storeId, string userID)
        {
            return StoresAndManagementInterface.CloseStore(storeId, userID);
        }

        public Result<StoreService> ReOpenStore(string storeId, string userID)
        {
            return StoresAndManagementInterface.ReOpenStore(storeId, userID);
        }

        public Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info)
        {
            return StoresAndManagementInterface.AddDiscountPolicy(storeId, info);
        }

        public Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.AddDiscountPolicy(storeId, info, id);
        }

        public Result<bool> AddDiscountCondition(string storeId, Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.AddDiscountCondition(storeId, info, id);
        }

        public Result<bool> RemoveDiscountPolicy(string storeId, string id)
        {
            return StoresAndManagementInterface.RemoveDiscountPolicy(storeId, id);
        }

        public Result<bool> RemoveDiscountCondition(string storeId, string id)
        {
            return StoresAndManagementInterface.RemoveDiscountCondition(storeId, id);
        }

        public Result<bool> EditDiscountPolicy(string storeId, Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.EditDiscountPolicy(storeId, info, id);
        }

        public Result<bool> EditDiscountCondition(string storeId, Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.EditDiscountCondition(storeId, info, id);
        }

        public Result<IDiscountPolicyData> GetDiscountPolicyData(string storeId)
        {
            return StoresAndManagementInterface.GetPoliciesData(storeId);
        }

        public Result<IPurchasePolicyData> GetPurchasePolicyData(string storeId)
        {
            return StoresAndManagementInterface.GetPurchasePolicyData(storeId);
        }

        public Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info)
        {
            return StoresAndManagementInterface.AddPurchasePolicy(storeId, info);
        }

        public Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.AddPurchasePolicy(storeId, info, id);
        }

        public Result<bool> RemovePurchasePolicy(string storeId, string id)
        {
            return StoresAndManagementInterface.RemovePurchasePolicy(storeId, id);
        }

        public Result<bool> EditPurchasePolicy(string storeId, Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.EditPurchasePolicy(storeId, info, id);
        }

        public Result<List<Tuple<DateTime, Double>>> GetIncomeAmountGroupByDay(String start_date, String end_date, String store_id, String owner_id)
        {
            return StoresAndManagementInterface.GetIncomeAmountGroupByDay(start_date, end_date, store_id, owner_id);
        }

        #endregion
    }
}
