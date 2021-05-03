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
        Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID);
        Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID);

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
        public Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID) { return StoresAndManagementInterface.GetStorePurchaseHistory(ownerID, storeID); }
        public Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID) { return StoresAndManagementInterface.RemoveStoreManager(removedManagerID, currentlyOwnerID, storeID); }

        #endregion
    }
}
