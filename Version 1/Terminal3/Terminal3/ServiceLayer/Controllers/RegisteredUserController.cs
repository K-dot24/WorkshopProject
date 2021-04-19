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
        public Result<UserDAL> Login(String email, String password) { }
        public Result<Boolean> LogOut(String email);
        public Result<Object> SearchStore(IDictionary<String, Object> details);
        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);
        public Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID);   // Redundent ?
        public Result<ShoppingCartDAL> GetUserShoppingCart(String userID);
        public Result<Dictionary<String, int>> GetUserShoppingBag(String userID, String shoppingBagID); //Dictionary<pid , countity>
        public Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity);
        public Result<Object> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);
        public Result<HistoryDAL> GetUserPurchaseHistory(String userID);
        public Result<int> GetTotalShoppingCartPrice(String userID);
        public Result<StoreDAL> OpenNewStore(String storeName, String userID);

        #endregion
    }
}
