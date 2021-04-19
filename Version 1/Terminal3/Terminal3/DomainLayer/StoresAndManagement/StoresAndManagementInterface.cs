using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DALobjects;
using System.Collections.Concurrent;


namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        Result<StoreDAL> OpenNewStore(String storeName, String userID);

        #region Inventory Management
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);


        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL, PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        #endregion

        Result<Boolean> AddProductReview(String userID, String storeID, String productID , String review);

        #region User Actions
        Result<RegisteredUser> Register(String email, String password);
        Result<UserDAL> Login(String email, String password);
        Result<Boolean> LogOut(String email);
        Result<Boolean> AddProductToCart(String userID, String productID, int productQuantity, String storeID);
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID, bool systemAdmin);
        Result<HistoryDAL> GetUserPurchaseHistory(String userID);
        Result<Boolean> ExitSystem(String userID);
        #endregion

        #region System Managment
        Result<RegisteredUserDAL> AddSystemAdmin(String email);
        Result<RegisteredUserDAL> RemoveSystemAdmin(String email);
        Result<Boolean> isSystemAdmin(String userID);
        #endregion
    }
    public class StoresAndManagementInterface : IStoresAndManagementInterface
    {
        // Properties
        public StoresFacade StoresFacade { get; }
        public UsersAndPermissionsFacade UsersAndPermissionsFacade { get; }

        public StoresAndManagementInterface()
        {
            //TODO: Change constructor if needed (initializer?)
            StoresFacade = new StoresFacade();
            UsersAndPermissionsFacade = new UsersAndPermissionsFacade();
        }


        //TODO: Implement all functions

        // Methods
        public Result<StoreDAL> OpenNewStore(String storeName, String userID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser founder))  // Check if userID is a registered user
            {
                // Open store
                return StoresFacade.OpenNewStore(founder, userID);
            }
            //else
            return new Result<StoreDAL>($"Failed to open store {storeName}: {userID} is not a registered user.\n", false, null);
        }

        public Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            return StoresFacade.AddProductToStore(userID, storeID, productName, price, initialQuantity, category, keywords);
        }

        public Result<ProductDAL> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details)
        {
            return StoresFacade.EditProductDetails(userID, storeID, productID, details);
        }

        public Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails)
        {
            return StoresFacade.SearchProduct(productDetails);
        }


        public Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(addedOwnerID, out RegisteredUser futureOwner))  // Check if addedOwnerID is a registered user
            {
                return StoresFacade.AddStoreOwner(futureOwner, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to appoint store owner: {addedOwnerID} is not a registered user.\n", false, false);
        }

        public Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(addedManagerID, out RegisteredUser futureManager))  // Check if addedManagerID is a registered user
            {
                return StoresFacade.AddStoreOwner(futureManager, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to appoint store manager: {addedManagerID} is not a registered user.\n", false, false);
        }

        public Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.ContainsKey(removedManagerID))  // Check if addedManagerID is a registered user
            {
                return StoresFacade.RemoveStoreManager(removedManagerID, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to remove store manager: {removedManagerID} is not a registered user.\n", false, false);
        }

        public Result<bool> RemoveProductFromStore(String userID, String storeID, String productID)
        {
            return StoresFacade.RemoveProductFromStore(userID, storeID, productID);
        }

        public Result<bool> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions)
        {
            return StoresFacade.SetPermissions(storeID, managerID, ownerID, permissions);
        }

        public Result<Dictionary<IStoreStaffDAL, PermissionDAL>> GetStoreStaff(string ownerID, string storeID)
        {
            Result<Dictionary<IStoreStaff, Permission>> storeStaffResult = StoresFacade.GetStoreStaff(ownerID, storeID);
            if (storeStaffResult.ExecStatus)
            {
                Dictionary<IStoreStaff, Permission> storeStaff = storeStaffResult.Data;
                Dictionary<IStoreStaffDAL, PermissionDAL> storeStaffDAL = new Dictionary<IStoreStaffDAL, PermissionDAL>();

                foreach (var user in storeStaff)
                {
                    storeStaffDAL.Add((IStoreStaffDAL)user.Key.GetDAL().Data, user.Value.GetDAL().Data);
                }
                return new Result<Dictionary<IStoreStaffDAL, PermissionDAL>>(storeStaffResult.Message, true, storeStaffDAL);
            }

            return new Result<Dictionary<IStoreStaffDAL, PermissionDAL>>(storeStaffResult.Message , false , null);
        }

        public Result<HistoryDAL> GetStorePurchaseHistory(string userID, string storeID, bool systemAdmin=false)
        {
            Result<History> res = StoresFacade.GetStorePurchaseHistory(userID, storeID, systemAdmin);
            if (res.ExecStatus)
            {
                return new Result<HistoryDAL>("Store purchase history\n" , true ,res.Data.GetDAL().Data);
            }
            return new Result<HistoryDAL>(res.Message, false, null);
        }

        public Result<HistoryDAL> GetUserPurchaseHistory(String userID)
        {
            Result<History> res = UsersAndPermissionsFacade.GetUserPurchaseHistory(userID);
            if (res.ExecStatus)
            {
                return new Result<HistoryDAL>(res.Message , true , res.Data.GetDAL().Data);
            }
            return new Result<HistoryDAL>(res.Message, false, null);
        }
        
        public Result<Boolean> AddProductToCart(string userID, string productID, int productQuantity, string storeID)
        {
            if (StoresFacade.Stores.TryGetValue(storeID, out Store store))  // Check if store exists
            {
                Result<Product> searchProductRes = store.GetProduct(productID);
                if (searchProductRes.ExecStatus)    // Check if product exists in store
                {
                    Product product = searchProductRes.Data;
                    return UsersAndPermissionsFacade.AddProductToCart(userID, product, productQuantity, store);
                }
                //else failed
                return new Result<Boolean>($"Product (ID: {productID}) was not found in {store.Name}\n", false, false);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            return StoresFacade.RemovePermissions(storeID, managerID, ownerID, permissions);
        }
       
        public Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID)
        {
            return StoresFacade.GetProductReview(storeID, productID);
        }

        public Result<Boolean> AddProductReview(String userID, String storeID, String productID , String review)
        {
            Result<Store> storeRes = StoresFacade.GetStore(storeID);
            if (storeRes.ExecStatus)
            {                
                Result<Product> productRes = storeRes.Data.GetProduct(productID);   // TODO - check updated :function exists in branch - AddProductToCart 
                if (productRes.ExecStatus)
                {
                    return UsersAndPermissionsFacade.AddProductReview(userID, storeRes.Data, productRes.Data , review);
                }
                return new Result<Boolean>(productRes.Message, false, false);                
            }
            return new Result<Boolean>(storeRes.Message, false, false);
            
        }


        public Result<Boolean> ExitSystem(String userID)
        {
            return UsersAndPermissionsFacade.ExitSystem(userID);
        }

        public Result<RegisteredUserDAL> AddSystemAdmin(string email)
        {
            Result<RegisteredUser> result =  UsersAndPermissionsFacade.AddSystemAdmin(email);
            if (result.ExecStatus) { return new Result<RegisteredUserDAL>(result.Message, result.ExecStatus, result.Data.GetDAL().Data); }
            else {return new Result<RegisteredUserDAL>(result.Message, result.ExecStatus, null);}
        }

        public Result<RegisteredUserDAL> RemoveSystemAdmin(string email)
        {
            Result<RegisteredUser> result = UsersAndPermissionsFacade.RemoveSystemAdmin(email);
            if (result.ExecStatus) { return new Result<RegisteredUserDAL>(result.Message, result.ExecStatus, result.Data.GetDAL().Data); }
            else { return new Result<RegisteredUserDAL>(result.Message, result.ExecStatus, null); }
        }

        public Result<Boolean> isSystemAdmin(String userID)
        {
            bool isContains = UsersAndPermissionsFacade.SystemAdmins.ContainsKey(userID);
            return new Result<Boolean>($"is {userID} is system admin? {isContains}\n", true, isContains);
        }
    }
}
