using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;
using XUnitTestTerminal3.AcceptanceTests.Utils;

namespace Terminal3.ServiceLayer
{   
    //System Real interface with DAL
    public interface IECommerceSystemInterface
    {

        #region System related operations
        Result<Boolean> ResetSystem();

        void ExitSystem(String userID); 

        #endregion

        #region User related operations
        Result<Boolean> Register(String email, String password);


        Result<RegisteredUserDAL> Login(String email, String password);

        Result<Boolean> LogOut(String email);

        //TODO: refine requirement
        Result<Object> SearchStore(IDictionary<String, Object> details);

        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);

        Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID);   // Redundent ?

        Result<ShoppingCartDAL> GetUserShoppingCart(String userID);

        Result<Dictionary<String, int>> GetUserShoppingBag(String userID, String shoppingBagID); //Dictionary<pid , quantity>

        Result<Boolean> UpdateShoppingCart(String userID, String storeID, String productID, int quantity); // this quantity will be the updated quantity of the product in the bag . if negative or zero then the product will be removed

        Result<Object> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);

        Result<HistoryDAL> GetUserPurchaseHistory(String userID);

        Result<double> GetTotalShoppingCartPrice(String userID);
        #endregion

        #region Store related operations
        Result<StoreDAL> OpenNewStore(String storeName, String userID);
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaffDAL, PermissionDAL>> GetStoreStaff(String userID, String storeID);
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
        Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID);

        #endregion
    }
    public class ECommerceSystem : IECommerceSystemInterface
    {
        public IStoresAndManagementInterface StoresAndManagement { get; }
        public ECommerceSystem()
        {
            StoresAndManagement = new StoresAndManagementInterface(); 
        }

        public Result<bool> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            throw new NotImplementedException();
        }

        public Result<ProductDAL> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
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

        public Result<ProductDAL> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string ownerID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<Dictionary<IStoreStaffDAL, PermissionDAL>> GetStoreStaff(string userID, string storeID)
        {
            throw new NotImplementedException();
        }

        public Result<double> GetTotalShoppingCartPrice(string userID)
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

        public Result<object> SearchStore(IDictionary<string, object> details)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetPermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateShoppingCart(string userID, string storeID, string productID, int quantity)
        {
            throw new NotImplementedException();
        }

        public Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID)
        {
            throw new NotImplementedException();

        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public void ExitSystem(String userID)
        {
            StoresAndManagement.ExitSystem(userID);
            //TODO
            System.Environment.Exit(0);
        }

    }
}
