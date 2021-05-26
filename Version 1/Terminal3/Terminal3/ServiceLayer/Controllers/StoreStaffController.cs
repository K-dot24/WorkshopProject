using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

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
        Result<bool> AddDiscountPolicy(Dictionary<string, object> info);
        Result<bool> AddDiscountPolicy(Dictionary<string, object> info, String id);
        Result<bool> AddDiscountCondition(Dictionary<string, object> info, String id);
        Result<bool> RemoveDiscountPolicy(String id);
        Result<bool> RemoveDiscountCondition(String id);
        Result<bool> EditDiscountPolicy(Dictionary<string, object> info, String id);
        Result<bool> EditDiscountCondition(Dictionary<string, object> info, String id);
        Result<IDiscountPolicyData> GetDiscountPolicyData();
        Result<IPurchasePolicyData> GetPurchasePolicyData();
        Result<bool> AddPurchasePolicy(Dictionary<string, object> info);
        Result<bool> AddPurchasePolicy(Dictionary<string, object> info, String id);
        Result<bool> RemovePurchasePolicy(String id);


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

        public Result<bool> AddDiscountPolicy(Dictionary<string, object> info)
        {
            return StoresAndManagementInterface.AddDiscountPolicy(info);
        }

        public Result<bool> AddDiscountPolicy(Dictionary<string, object> info, string id)
        {
            return StoresAndManagementInterface.AddDiscountPolicy(info, id);
        }

        public Result<bool> AddDiscountCondition(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveDiscountPolicy(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveDiscountCondition(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> EditDiscountPolicy(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> EditDiscountCondition(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public Result<IDiscountPolicyData> GetDiscountPolicyData()
        {
            throw new NotImplementedException();
        }

        public Result<IPurchasePolicyData> GetPurchasePolicyData()
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddPurchasePolicy(Dictionary<string, object> info)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddPurchasePolicy(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemovePurchasePolicy(string id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
