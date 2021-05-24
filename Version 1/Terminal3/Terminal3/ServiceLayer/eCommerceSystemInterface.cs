using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3.ServiceLayer.Controllers;
using XUnitTestTerminal3.AcceptanceTests.Utils;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Microsoft.AspNetCore.SignalR.Client;
using signalRgateway.Models;

namespace Terminal3.ServiceLayer
{   
    public interface IECommerceSystem : IGuestUserInterface, IRegisteredUserInterface, IStoreStaffInterface, ISystemAdminInterface, IDataController
    {
        List<Notification> GetNotificationByEvent(Event eventEnum);
        List<Notification> GetPendingMessagesByUserID(string userId);
    }
    //try git action
    public class ECommerceSystem : IECommerceSystem
    {
        //Properties
        public StoresAndManagementInterface StoresAndManagement  { get;set;}
        public IGuestUserInterface GuestUserInterface { get; set; }
        public IRegisteredUserInterface RegisteredUserInterface { get; set; }
        public IStoreStaffInterface StoreStaffInterface { get; set;  }
        public SystemAdminController SystemAdminInterface { get; set; }
        public IDataController DataController{ get; set; }
        public NotificationService NotificationService{ get; set; }
        public HubConnection connection { get; set; }


        //Constructor
        public ECommerceSystem()
        {

            Initializer.init(StoresAndManagement, GuestUserInterface, RegisteredUserInterface, StoreStaffInterface,
                SystemAdminInterface, DataController, NotificationService, this.connection);
            
        }

        public void DisplaySystem()
        {
            // TODO - when GUI exists then display all functions according to current user role
        }

        //Metohds
        #region Guest User Actions
        public Result<UserService> EnterSystem()
        {
            return GuestUserInterface.EnterSystem();
        }
        public void ExitSystem(String userID)
        {
            GuestUserInterface.ExitSystem(userID);
        }
        public Result<RegisteredUserService> Register(string email, string password, string optionalID = "-1")
        {
            return GuestUserInterface.Register(email, password, optionalID);
        }

        public Result<List<StoreService>> SearchStore(IDictionary<string, object> details)
        {
            return GuestUserInterface.SearchStore(details);
        }

        public Result<List<ProductService>> SearchProduct(IDictionary<string, object> searchAttributes)
        {
            return GuestUserInterface.SearchProduct(searchAttributes);
        }

        public Result<bool> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            return GuestUserInterface.AddProductToCart(userID, ProductID, ProductQuantity, StoreID);
        }

        public Result<ShoppingCartService> GetUserShoppingCart(string userID)
        {
            return GuestUserInterface.GetUserShoppingCart(userID);
        }

        public Result<bool> UpdateShoppingCart(string userID, string shoppingBagID, string productID, int quantity)
        {
            return GuestUserInterface.UpdateShoppingCart(userID, shoppingBagID, productID, quantity);
        }

        public Result<ShoppingCartService> Purchase(string userID, IDictionary<string, object> paymentDetails, IDictionary<string, object> deliveryDetails)
        {
            return GuestUserInterface.Purchase(userID, paymentDetails, deliveryDetails);
        }

        public Result<HistoryService> GetUserPurchaseHistory(string userID)
        {
            return GuestUserInterface.GetUserPurchaseHistory(userID);
        }

        public Result<double> GetTotalShoppingCartPrice(string userID)
        {
            return GuestUserInterface.GetTotalShoppingCartPrice(userID);
        }
        public Result<List<Tuple<String, String>>> GetProductReview(String storeID, String productID) {
            return GuestUserInterface.GetProductReview(storeID, productID);
        }
        #endregion

        #region Register User Actions
        public Result<RegisteredUserService> Login(string email, string password)
        {
            return RegisteredUserInterface.Login(email, password);
        }

        public Result<RegisteredUserService> Login(string email, string password,string guestUserID)
        {
            Result<RegisteredUserService> result =  RegisteredUserInterface.Login(email, password, guestUserID);
            if (result.ExecStatus)
            {
                SignalRLoginModel message = new SignalRLoginModel(guestUserID, result.Data.Id);
                //hubProxy.Invoke("Login", message);
                connection.InvokeAsync("Login", message);
            }
            return result;
        }

        public Result<UserService> LogOut(string email)
        {
            Result<RegisteredUser> registeredUser = StoresAndManagement.FindUserByEmail(email);
            Result<UserService> result =  RegisteredUserInterface.LogOut(email);
            if (result.ExecStatus && registeredUser.ExecStatus) 
            {
                SignalRLoginModel message = new SignalRLoginModel(registeredUser.Data.Id, result.Data.Id);
                //hubProxy.Invoke("Logout", message);
                connection.InvokeAsync("Logout", message);

            }
            return result;
        }

        public Result<StoreService> OpenNewStore(string storeName, string userID, String storeID = "-1")
        {
            return RegisteredUserInterface.OpenNewStore(storeName, userID);
        }

        public Result<Boolean> CloseStore(string storeId, string userID)
        {
            return StoreStaffInterface.CloseStore(storeId, userID);
        }


        public Result<StoreService> ReOpenStore(string storeId, string userID)
        {
            return StoreStaffInterface.ReOpenStore(storeId, userID);
        }

        public Result<ProductService> AddProductReview(String userID, String storeID, String productID, String review) {
            return RegisteredUserInterface.AddProductReview(userID, storeID, productID, review);
        }
        #endregion

        #region Store Actions
        public Result<ProductService> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category, LinkedList<string> keywords = null)
        {
            return StoreStaffInterface.AddProductToStore(userID, storeID, productName, price, initialQuantity, category, keywords);
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            return StoreStaffInterface.RemoveProductFromStore(userID, storeID, productID);
        }

        public Result<ProductService> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            return StoreStaffInterface.EditProductDetails(userID,storeID,productID, details);
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            return StoreStaffInterface.AddStoreOwner(addedOwnerID, currentlyOwnerID, storeID);
        }

        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            return StoreStaffInterface.AddStoreManager(addedManagerID,currentlyOwnerID, storeID);

        }

        public Result<bool> SetPermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            return StoreStaffInterface.SetPermissions(storeID, managerID, ownerID, permissions);

        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            return StoreStaffInterface.RemovePermissions(storeID, managerID, ownerID, permissions);

        }

        public Result<List<Tuple<IStoreStaffService, PermissionService>>> GetStoreStaff(string ownerID, string storeID)
        {
            return StoreStaffInterface.GetStoreStaff(ownerID, storeID);

        }

        public Result<HistoryService> GetStorePurchaseHistory(string ownerID, string storeID,Boolean isSysAdmin=false)
        {
            return !isSysAdmin ? StoreStaffInterface.GetStorePurchaseHistory(ownerID, storeID) :
                                SystemAdminInterface.GetStorePurchaseHistory(ownerID, storeID);

        }

        public Result<bool> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID)
        {
            return StoreStaffInterface.RemoveStoreManager(removedManagerID, currentlyOwnerID, storeID);
        }

        public Result<bool> RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID)
        {
            return StoreStaffInterface.RemoveStoreOwner(removedOwnerID, currentlyOwnerID, storeID);
        }
        #endregion

        #region System Admin Actions
        public Result<HistoryService> GetUserPurchaseHistory(string sysAdminID, string userID)
        {
            return SystemAdminInterface.GetUserPurchaseHistory(sysAdminID, userID);
        }

        public Result<RegisteredUserService> AddSystemAdmin(string sysAdminID, string email)
        {
            return SystemAdminInterface.AddSystemAdmin(sysAdminID, email);

        }

        public Result<RegisteredUserService> RemoveSystemAdmin(string sysAdminID, string email)
        {
            return SystemAdminInterface.RemoveSystemAdmin(sysAdminID, email);

        }

        public Result<bool> ResetSystem(String sysAdminID)
        {
            Result<Boolean> res = SystemAdminInterface.ResetSystem(sysAdminID);
            if (res.ExecStatus)
            {
                //StoresAndManagementInterface StoresAndManagement = new StoresAndManagementInterface();
                GuestUserInterface = new GuestUserController(StoresAndManagement);
                RegisteredUserInterface = new RegisteredUserController(StoresAndManagement);
                StoreStaffInterface = new StoreStaffController(StoresAndManagement);
                SystemAdminInterface = new SystemAdminController(StoresAndManagement);
            }
            return res;            
        }

        #endregion

        #region Data to display 
        public List<StoreService> GetAllStoresToDisplay()
        {
            return DataController.GetAllStoresToDisplay();
        }
        public List<ProductService> GetAllProductByStoreIDToDisplay(string storeID)
        {
            return DataController.GetAllProductByStoreIDToDisplay(storeID);
        }

        public Boolean[] GetPermission(string userID, string storeID)
        {
            return DataController.GetPermission(userID, storeID);
        }
        #endregion

        #region Notification

        public List<Notification> GetNotificationByEvent(Event eventEnum) 
        {
            return NotificationService.GetNotificationByEvent(eventEnum);
        }
        public List<Notification> GetPendingMessagesByUserID(string userId) {
            return NotificationService.GetPendingMessagesByUserID(userId);
        }
        #endregion
    }
}
