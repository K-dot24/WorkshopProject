using System;
using System.Collections.Generic;
using System.Text;
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

        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
        Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID);

        #endregion
    }
}
