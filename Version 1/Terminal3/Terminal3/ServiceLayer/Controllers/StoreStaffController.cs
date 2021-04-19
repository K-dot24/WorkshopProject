using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer.Controllers
{
    public class StoreStaffController
    {
        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public StoreStaffController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }
        #region Methods

        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null) { return StoresAndManagementInterface.AddProductToStore(userID, storeID, productName, price, initialQuantity, category,keywords); }
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID) { return StoresAndManagementInterface.RemoveProductFromStore(userID,  storeID,  productID); }
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details) { return StoresAndManagementInterface.EditProductDetails(userID, storeID,  productID, details); }
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID) { return StoresAndManagementInterface.AddStoreOwner(addedOwnerID, currentlyOwnerID, storeID); }
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID) { return StoresAndManagementInterface.AddStoreManager(addedManagerID, currentlyOwnerID, storeID); }
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions) { return StoresAndManagementInterface.SetPermissions(storeID, managerID, ownerID, permissions); }
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions) { return StoresAndManagementInterface.RemovePermissions(storeID,  managerID,  ownerID,  permissions); }
        Result<Dictionary<IStoreStaffDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID) { throw new NotImplementedException(); }
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID) { throw new NotImplementedException(); }
        Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID) { throw new NotImplementedException(); }

        #endregion
    }
}
