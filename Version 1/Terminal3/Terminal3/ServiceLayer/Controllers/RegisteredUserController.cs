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
        public Result<UserDAL> Login(String email, String password) { 
            return StoresAndManagementInterface.
        }
        public Result<Boolean> LogOut(String email) { throw new NotImplementedException(); }
        public Result<Object> SearchStore(IDictionary<String, Object> details) { throw new NotImplementedException(); }
        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails) { throw new NotImplementedException(); }
        public Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID) { throw new NotImplementedException(); }   // Redundent ?
        public Result<ShoppingCartDAL> GetUserShoppingCart(String userID) { throw new NotImplementedException(); }
        public Result<Dictionary<String, int>> GetUserShoppingBag(String userID, String shoppingBagID) { throw new NotImplementedException(); } //Dictionary<pid , countity>
        public Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity) { throw new NotImplementedException(); }
        public Result<Object> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails) { throw new NotImplementedException(); }
        public Result<HistoryDAL> GetUserPurchaseHistory(String userID) { throw new NotImplementedException(); }
        public Result<int> GetTotalShoppingCartPrice(String userID) { throw new NotImplementedException(); }
        public Result<StoreDAL> OpenNewStore(String storeName, String userID) { throw new NotImplementedException(); }

        #endregion
    }
}
