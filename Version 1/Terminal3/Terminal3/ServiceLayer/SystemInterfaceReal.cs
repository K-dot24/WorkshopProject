using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;


namespace XUnitTestTerminal3.AcceptanceTests.Utils
{
    public class SystemInterfaceReal : ISystemInterface
    {
        public Result<bool> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            throw new NotImplementedException();
        }

        public Result<string> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<string> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<List<string>> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<string, List<int>>> GetStoreStaff(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<int> GetTotalShoppingCartPrice(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<List<string>> GetUserPurchaseHistory(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<string, int>> GetUserShoppingBag(string userID, string shoppingBagID)
        {
            throw new NotImplementedException();
        }

        public Result<List<string>> GetUserShoppingCart(string userID)
        {
            throw new NotImplementedException();
        }

        public Result<string> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Result<bool> LogOut(string email)
        {
            throw new NotImplementedException();
        }

        public Result<string> OpenNewStore(string storeName, string userID)
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

        public Result<List<string>> SearchProduct(IDictionary<string, object> productDetails)
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
    }
}
