using System;
using System.Collections.Generic;
using System.Text;
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

        public Result<ProductDAL> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            if (Real == null)
                return new Result<ProductDAL>(true);

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
            throw new NotImplementedException();
        }

        public Result<ProductDAL> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<ProductDAL> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<int> GetTotalShoppingCartPrice(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetUserPurchaseHistory(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetUserPurchaseHistory(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<ShoppingCartDAL> GetUserShoppingCart(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<ShoppingCartDAL> GetUserShoppingCart(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> Login(string email, string password)
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

        public Result<StoreDAL> OpenNewStore(string storeName, string userID)
        {
            throw new NotImplementedException();
        }

        public Result<object> Purchase(string userID, IDictionary<string, object> paymentDetails, IDictionary<string, object> deliveryDetails)
        {
            throw new NotImplementedException();
        }

        public Result<bool> Register(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> ResetSystem()
        {
            throw new NotImplementedException();
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<string, object> productDetails)
        {
            throw new NotImplementedException();
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<string, object> productDetails)
        {
            throw new NotImplementedException();
        }

        public Result<object> SearchStore(IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateShoppingCart(string userID, string shoppingBagID, string productID, int quantity)
        {
            throw new NotImplementedException();
        }

        Result<bool> ISystemInterface.AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            throw new NotImplementedException();
        }

        Result<ProductDAL> ISystemInterface.AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            throw new NotImplementedException();
        }
    }
}
