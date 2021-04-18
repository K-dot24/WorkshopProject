using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;


namespace XUnitTestTerminal3.IntegrationTests
{
    public class SystemInterfaceProxy : ISystemInterface
    {
        public ISystemInterface Real { get; set; }

        public Result<Boolean> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            if (Real == null)   
                return new Result<Boolean>(true);

            return Real.AddProductToCart(userID, ProductID, ProductQuantity, StoreID);
        }

        public Result<string> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            if (Real == null)
                return new Result<string>(true);

            return Real.AddProductToStore(userID, storeID, productName, price, initialQuantity, category);
        }

        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.AddStoreManager(addedManagerID, currentlyOwnerID, storeID);
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            if(Real == null)
                return new Result<bool>(true);

            return Real.AddStoreOwner(addedOwnerID, currentlyOwnerID, storeID);
        }

        public Result<string> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<string>(true);

            return Real.EditProductDetails(userID, storeID, productID, details);
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            if (Real == null)
                return new Result<HistoryDAL>(true);

            return Real.GetStorePurchaseHistory(ownerID, storeID);
        }

        public Result<Dictionary<String, List<int>>> GetStoreStaff(string ownerID, string storeID)
        {
            if (Real == null)
                return new Result<Dictionary<string, List<int>>>(true);

            return Real.GetStoreStaff(ownerID, storeID);
        }

        public Result<int> GetTotalShoppingCartPrice(string userID)
        {
            if(Real == null)
                return new Result<int>(true);

            return Real.GetTotalShoppingCartPrice(userID);
        }

        public Result<HistoryDAL> GetUserPurchaseHistory(string userID)
        {
            if (Real == null)
                return new Result<HistoryDAL>(true);

            return Real.GetUserPurchaseHistory(userID);
        }

        public Result<LinkedList<String>> GetUserShoppingCart(string userID)
        {
            if (Real == null)
                return new Result<LinkedList<String>>(true);

            return Real.GetUserShoppingCart(userID);
        }

        public Result<String> Login(string email, string password)
        {
            if (Real == null)
                return new Result<string>(true);

            return Real.Login(email, password);
        }

        public Result<bool> LogOut(string email)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.LogOut(email);
        }

        public Result<String> OpenNewStore(string storeName, string userID)
        {
            if (Real == null)
                return new Result<string>(true);

            return Real.OpenNewStore(storeName, userID);
        }

        public Result<object> Purchase(string userID, IDictionary<string, object> paymentDetails, IDictionary<string, object> deliveryDetails)
        {
            if (Real == null)
                return new Result<object>(true);

            return Real.Purchase(userID, paymentDetails, deliveryDetails);
        }

        public Result<bool> Register(string email, string password)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.Register(email, password);
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.RemoveProductFromStore(userID, storeID, productID);
        }

        public Result<bool> ResetSystem()
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.ResetSystem();
        }

        public Result<List<string>> SearchProduct(IDictionary<string, object> productDetails)
        {
            if (Real == null)
                return new Result<List<string>>(true);

            return Real.SearchProduct(productDetails);
        }

        public Result<object> SearchStore(IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<object>(true);

            return Real.SearchStore(details);
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SetPermissions(managerID, ownerID, permissions);
        }

        public Result<bool> UpdateShoppingCart(string userID, string shoppingBagID, string productID, int quantity)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.UpdateShoppingCart(userID, shoppingBagID, productID, quantity);
        }

    }
}
