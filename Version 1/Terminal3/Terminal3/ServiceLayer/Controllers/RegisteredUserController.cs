using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer.Controllers
{
    public class RegisteredUserController
    {
        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public RegisteredUserController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }
        #region Methods
        public Result<RegisteredUserDAL> Login(String email, String password) {
            return StoresAndManagementInterface.Login(email,password);
        }
        public Result<Boolean> LogOut(String email) { return StoresAndManagementInterface.LogOut(email); }
        public Result<Object> SearchStore(IDictionary<String, Object> details) { throw new NotImplementedException(); }
        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails) { return StoresAndManagementInterface.SearchProduct(productDetails); }
        public Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID) { return StoresAndManagementInterface.AddProductToCart(userID, ProductID, ProductQuantity, StoreID); }   // Redundent ?
        public Result<ShoppingCartDAL> GetUserShoppingCart(String userID) { return GetUserShoppingCart(userID); }
        public Result<Boolean> UpdateShoppingCart(String userID, String storeID, String productID, int quantity) { return StoresAndManagementInterface.UpdateShoppingCart(userID,  storeID,  productID,  quantity); }
        public Result<Object> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails) { return StoresAndManagementInterface.Purchase(userID, paymentDetails, deliveryDetails); }
        public Result<HistoryDAL> GetUserPurchaseHistory(String userID) { return StoresAndManagementInterface.GetUserPurchaseHistory(userID); }
        public Result<double> GetTotalShoppingCartPrice(String userID) { return StoresAndManagementInterface.GetTotalShoppingCartPrice(userID); }
        public Result<StoreDAL> OpenNewStore(String storeName, String userID) { return StoresAndManagementInterface.OpenNewStore(storeName, userID); }

        #endregion
    }
}
