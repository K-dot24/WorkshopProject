using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;
using System.Runtime.InteropServices;

namespace Terminal3.ServiceLayer.Controllers
{
    public interface IGuestUserInterface
    {
        Result<UserService> EnterSystem();
        void ExitSystem(String userID);
        Result<RegisteredUserService> Register(string email, string password, string optionalID = "-1");
        Result<List<StoreService>> SearchStore(IDictionary<String, Object> details);
        Result<List<ProductService>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID);
        Result<ShoppingCartService> GetUserShoppingCart(String userID);
        Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity);
        Result<ShoppingCartService> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);
        Result<HistoryService> GetUserPurchaseHistory(String userID);
        Result<double> GetTotalShoppingCartPrice(String userID);
        Result<List<Tuple<String, String>>> GetProductReview(String storeID, String productID);

    }
    //Basic functionality The every user can preform
    public class GuestUserController: IGuestUserInterface
    {
        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public GuestUserController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }
        #region Methods
        public void ExitSystem(String userID)
        {
            StoresAndManagementInterface.ExitSystem(userID);
        }
        public Result<UserService> EnterSystem()
        {
            return StoresAndManagementInterface.EnterSystem();
        }
        public Result<RegisteredUserService> Register(string email, string password , string optionalID = "-1") {return StoresAndManagementInterface.Register(email,password, optionalID); }
        public Result<List<StoreService>> SearchStore(IDictionary<String, Object> details) { return StoresAndManagementInterface.SearchStore(details); }
        public Result<List<ProductService>> SearchProduct(IDictionary<String, Object> productDetails) { return StoresAndManagementInterface.SearchProduct(productDetails); }
        public Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID) { return StoresAndManagementInterface.AddProductToCart(userID, ProductID, ProductQuantity, StoreID); }   // Redundent ?
        public Result<ShoppingCartService> GetUserShoppingCart(String userID) { return StoresAndManagementInterface.GetUserShoppingCart(userID); }
        public Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity) { return StoresAndManagementInterface.UpdateShoppingCart(userID, shoppingBagID, productID, quantity); }
        public Result<ShoppingCartService> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            return StoresAndManagementInterface.Purchase(userID, paymentDetails, deliveryDetails);
        }
        public Result<HistoryService> GetUserPurchaseHistory(String userID) { return StoresAndManagementInterface.GetUserPurchaseHistory(userID); }
        public Result<double> GetTotalShoppingCartPrice(String userID) { return StoresAndManagementInterface.GetTotalShoppingCartPrice(userID); }
        public Result<List<Tuple<string, string>>> GetProductReview(string storeID, string productID)
        {
            return StoresAndManagementInterface.GetProductReview(storeID, productID);
        }
        #endregion

    }
}
