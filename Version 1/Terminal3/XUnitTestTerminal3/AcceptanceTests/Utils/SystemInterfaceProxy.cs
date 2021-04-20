using System;
using System.Collections.Concurrent;
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

        public Result<List<String>> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            if (Real == null)
                return new Result<List<String>>(true);

            return Real.GetStorePurchaseHistory(ownerID, storeID);
        }

        public Result<Dictionary<String, int>> GetUserShoppingBag(String userID, String shoppingBagID)
        {
            if (Real == null)
                return new Result<Dictionary<String, int>>(true);

            return Real.GetUserShoppingBag(userID, shoppingBagID);
        }

        public Result<Dictionary<String, List<int>>> GetStoreStaff(string ownerID, string storeID)
        {
            if (Real == null)
                return new Result<Dictionary<string, List<int>>>(true);

            return Real.GetStoreStaff(ownerID, storeID);
        }

        public Result<double> GetTotalShoppingCartPrice(string userID)
        {
            if(Real == null)
                return new Result<double>(true);

            return Real.GetTotalShoppingCartPrice(userID);
        }

        public Result<List<String>> GetUserPurchaseHistory(string userID)
        {
            if (Real == null)
                return new Result<List<String>>(true);

            return Real.GetUserPurchaseHistory(userID);
        }

        public Result<List<String>> GetUserShoppingCart(string userID)
        {
            if (Real == null)
                return new Result<List<String>>(true);

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

        public Result<bool> ResetSystem(string sysAdminID)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.ResetSystem(sysAdminID);
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

        public Result<bool> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SetPermissions(storeID , managerID, ownerID, permissions);
        }

        public Result<bool> UpdateShoppingCart(string userID, string shoppingBagID, string productID, int quantity)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.UpdateShoppingCart(userID, shoppingBagID, productID, quantity);
        }

        public Result<bool> AddProductReview(string userID, string storeID, string productID, string review)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.AddProductReview(userID, storeID, productID, review);
        }

        public Result<bool> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.RemoveStoreManager(removedManagerID, currentlyOwnerID, storeID);
        }

        public Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID)
        {
            if (Real == null)
                return new Result<ConcurrentDictionary<String, String>>(true);

            return Real.GetProductReview(storeID, productID);
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.RemoveStoreManager(storeID, managerID, storeID);
        }
    }
}
