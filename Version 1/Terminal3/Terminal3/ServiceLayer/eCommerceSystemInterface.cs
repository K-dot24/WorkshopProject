using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3.ServiceLayer.Controllers;
using XUnitTestTerminal3.AcceptanceTests.Utils;

namespace Terminal3.ServiceLayer
{   

    public class ECommerceSystem : IGuestUserInterface, IRegisteredUserInterface, IStoreStaffInterface, ISystemAdminInterface
    {
        //Properties
        public IGuestUserInterface GuestUserInterface { get; }
        public IRegisteredUserInterface RegisteredUserInterface { get; }
        public IStoreStaffInterface StoreStaffInterface { get; }
        public ISystemAdminInterface SystemAdminInterface { get; }

        //Constructor
        public ECommerceSystem()
        {
            //TODO: call initializer
            StoresAndManagementInterface StoresAndManagement = new StoresAndManagementInterface();
            GuestUserInterface = new GuestUserController(StoresAndManagement);
            RegisteredUserInterface = new RegisteredUserController(StoresAndManagement);
            StoreStaffInterface = new StoreStaffController(StoresAndManagement);
            SystemAdminInterface = new SystemAdminController(StoresAndManagement);
        }

        //Metohds
        #region Guest User Actions
        public Result<bool> Register(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Result<object> SearchStore(IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<string, object> productDetails)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            throw new NotImplementedException();
        }

        public Result<ShoppingCartDAL> GetUserShoppingCart(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateShoppingCart(string userID, string storeID, string productID, int quantity)
        {
            throw new NotImplementedException();
        }

        public Result<object> Purchase(string userID, IDictionary<string, object> paymentDetails, IDictionary<string, object> deliveryDetails)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetUserPurchaseHistory(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<double> GetTotalShoppingCartPrice(string userID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Register User Actions
        public Result<RegisteredUserDAL> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Result<bool> LogOut(string email)
        {
            throw new NotImplementedException();
        }

        public Result<StoreDAL> OpenNewStore(string storeName, string userID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Store Actions
        public Result<ProductDAL> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category, LinkedList<string> keywords = null)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            throw new NotImplementedException();
        }

        public Result<ProductDAL> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<IStoreStaffDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region System Admin Actions
        public Result<HistoryDAL> GetUserPurchaseHistory(string sysAdminID, string userID)
        {
            throw new NotImplementedException();
        }

        public Result<RegisteredUserDAL> AddSystemAdmin(string sysAdminID, string email)
        {
            throw new NotImplementedException();
        }

        public Result<RegisteredUserDAL> RemoveSystemAdmin(string sysAdminID, string email)
        {
            throw new NotImplementedException();
        }
        
        public Result<UserDAL> EnterSystem()
        {
            throw new NotImplementedException();
        }

        public void ExitSystem(String userID)
        {
            throw new NotImplementedException();

            //StoresAndManagement.ExitSystem(userID);
            ////TODO
            //System.Environment.Exit(0);
        }

        public Result<bool> ResetSystem(String sysAdminID)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
