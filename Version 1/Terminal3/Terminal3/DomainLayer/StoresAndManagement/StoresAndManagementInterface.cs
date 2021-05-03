﻿using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DALobjects;
using System.Collections.Concurrent;
using System.Linq;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        Result<StoreService> OpenNewStore(String storeName, String userID);

        #region Inventory Management
        Result<ProductService> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductService> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<ProductService>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<List<StoreService>> SearchStore(IDictionary<String, Object> details);
        Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaffService, PermissionService>> GetStoreStaff(String ownerID, String storeID);
        #endregion

        #region User Actions
        Result<RegisteredUserService> Register(String email, String password);
        Result<RegisteredUserService> Login(String email, String password);
        Result<Boolean> LogOut(String email);
        Result<Boolean> AddProductToCart(String userID, String productID, int productQuantity, String storeID);
        Result<Boolean> UpdateShoppingCart(string userID, string storeID, string productID, int quantity);
        Result<ShoppingCartService> GetUserShoppingCart(String userID);
        Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID, bool systemAdmin=false);
        Result<HistoryService> GetUserPurchaseHistory(String userID);
        Result<Boolean> AddProductReview(String userID, String storeID, String productID, String review);
        Result<Boolean> ExitSystem(String userID);
        Result<UserService> EnterSystem();
        Result<ShoppingCartService> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);
        Result<double> GetTotalShoppingCartPrice(String userID);
        #endregion

        #region System Managment
        Result<RegisteredUserService> AddSystemAdmin(String email);
        Result<RegisteredUserService> RemoveSystemAdmin(String email);
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
            StoresFacade = new StoresFacade();
            UsersAndPermissionsFacade = new UsersAndPermissionsFacade();
        }

        // Methods
        public Result<StoreService> OpenNewStore(String storeName, String userID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser founder))  // Check if userID is a registered user
            {
                // Open store
                Result<Store> res = StoresFacade.OpenNewStore(founder, storeName);
                if (res.ExecStatus)
                {
                    return new Result<StoreService>(res.Message, true, res.Data.GetDAL().Data);
                }
                //else
                return new Result<StoreService>($"Failed to open store {storeName}", false, null);
            }
            //else
            return new Result<StoreService>($"Failed to open store {storeName}: {userID} is not a registered user.\n", false, null);
        }

        public Result<ProductService> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            Result<Product> res = StoresFacade.AddProductToStore(userID, storeID, productName, price, initialQuantity, category, keywords);
            if (res.ExecStatus)
            {
                return new Result<ProductService>(res.Message, true, res.Data.GetDAL().Data);
            }
            return new Result<ProductService>(res.Message, false, null);
        }

        public Result<ProductService> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details)
        {
            Result<Product> res = StoresFacade.EditProductDetails(userID, storeID, productID, details);
            if (res.ExecStatus)
            {
                return new Result<ProductService>(res.Message, true, res.Data.GetDAL().Data);
            }
            return new Result<ProductService>(res.Message, false, null);
        }

        public Result<List<StoreService>> SearchStore(IDictionary<String, Object> details)
        {
            Result<List<Store>> res = StoresFacade.SearchStore(details);
            List<StoreService> storeDALs = new List<StoreService>();
            if (res.ExecStatus)
            {
                foreach (Store store in res.Data)
                {
                    storeDALs.Add(store.GetDAL().Data);
                }
                return new Result<List<StoreService>>(res.Message, true, storeDALs);
            }
            return new Result<List<StoreService>>(res.Message, false, null);
        }

        public Result<List<ProductService>> SearchProduct(IDictionary<String, Object> productDetails)
        {
            Result<List<Product>> res = StoresFacade.SearchProduct(productDetails);
            List<ProductService> productDALs = new List<ProductService>();
            if (res.ExecStatus)
            {                
                foreach(Product product in res.Data)
                {
                    productDALs.Add(product.GetDAL().Data);
                }
                return new Result<List<ProductService>>(res.Message, true, productDALs);
            }
            return new Result<List<ProductService>>(res.Message, false, null);
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
                return StoresFacade.AddStoreManager(futureManager, currentlyOwnerID, storeID);
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

        public Result<Dictionary<IStoreStaffService, PermissionService>> GetStoreStaff(string userID, string storeID)
        {
            Result<Dictionary<IStoreStaff, Permission>> storeStaffResult = StoresFacade.GetStoreStaff(userID, storeID);
            if (storeStaffResult.ExecStatus)
            {
                Dictionary<IStoreStaff, Permission> storeStaff = storeStaffResult.Data;
                Dictionary<IStoreStaffService, PermissionService> storeStaffDAL = new Dictionary<IStoreStaffService, PermissionService>();

                foreach (var user in storeStaff)
                {
                    storeStaffDAL.Add((IStoreStaffService)user.Key.GetDAL().Data, user.Value.GetDAL().Data);
                }
                return new Result<Dictionary<IStoreStaffService, PermissionService>>(storeStaffResult.Message, true, storeStaffDAL);
            }

            return new Result<Dictionary<IStoreStaffService, PermissionService>>(storeStaffResult.Message , false , null);
        }

        public Result<HistoryService> GetStorePurchaseHistory(string userID, string storeID, bool systemAdmin=false)
        {
            Result<History> res = StoresFacade.GetStorePurchaseHistory(userID, storeID, systemAdmin);
            if (res.ExecStatus)
            {
                return new Result<HistoryService>("Store purchase history\n" , true ,res.Data.GetDAL().Data);
            }
            return new Result<HistoryService>(res.Message, false, null);
        }

        public Result<HistoryService> GetUserPurchaseHistory(String userID)
        {
            Result<History> res = UsersAndPermissionsFacade.GetUserPurchaseHistory(userID);
            if (res.ExecStatus)
            {
                return new Result<HistoryService>(res.Message , true , res.Data.GetDAL().Data);
            }
            return new Result<HistoryService>(res.Message, false, null);
        }
        
        public Result<Boolean> AddProductToCart(string userID, string productID, int productQuantity, string storeID)
        {
            Result<Store> resStore = StoresFacade.GetStore(storeID);
            if (resStore.ExecStatus)    // Check if store exists
            {
                Store store = resStore.Data;
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

        public Result<Boolean> UpdateShoppingCart(string userID, string storeID, string productID, int quantity)
        {
            Result<Store> resStore = StoresFacade.GetStore(storeID);
            if (resStore.ExecStatus)
            {
                Result<Product> resProduct = resStore.Data.GetProduct(productID);
                if (resProduct.ExecStatus)
                {
                    return UsersAndPermissionsFacade.UpdateShoppingCart(userID, resStore.Data.Id, resProduct.Data, quantity);
                }
                //else faild
                return new Result<Boolean>(resProduct.Message, false, false);
            }
            //else faild
            return new Result<Boolean>(resStore.Message, false, false);    
        }

        public Result<ShoppingCartService> GetUserShoppingCart(string userID)
        {
            Result<ShoppingCart> res = UsersAndPermissionsFacade.GetUserShoppingCart(userID);
            if (res.ExecStatus)
            {
                ShoppingCartService shoppingCartDAL = res.Data.GetDAL().Data;
                return new Result<ShoppingCartService>("User shopping cart\n", true, shoppingCartDAL);
            }
            //else faild
            return new Result<ShoppingCartService>(res.Message, false, null);
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
                Result<Product> productRes = storeRes.Data.GetProduct(productID);    
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

        public Result<UserService> EnterSystem()
        {
            Result<User> res = UsersAndPermissionsFacade.EnterSystem();
            if (res.ExecStatus)
            {
                UserService userDAL = res.Data.GetDAL().Data;
                return new Result<UserService>(res.Message, true, userDAL);
            }
            //else faild
            return new Result<UserService>(res.Message, false, null);
        }

        public Result<RegisteredUserService> AddSystemAdmin(string email)
        {
            Result<RegisteredUser> result =  UsersAndPermissionsFacade.AddSystemAdmin(email);
            if (result.ExecStatus) { return new Result<RegisteredUserService>(result.Message, result.ExecStatus, result.Data.GetDAL().Data); }
            else {return new Result<RegisteredUserService>(result.Message, result.ExecStatus, null);}
        }

        public Result<RegisteredUserService> RemoveSystemAdmin(string email)
        {
            Result<RegisteredUser> result = UsersAndPermissionsFacade.RemoveSystemAdmin(email);
            if (result.ExecStatus) { return new Result<RegisteredUserService>(result.Message, result.ExecStatus, result.Data.GetDAL().Data); }
            else { return new Result<RegisteredUserService>(result.Message, result.ExecStatus, null); }
        }

        public Result<Boolean> isSystemAdmin(String userID)
        {
            bool isContains = UsersAndPermissionsFacade.SystemAdmins.ContainsKey(userID);
            return new Result<Boolean>($"is {userID} is system admin? {isContains}\n", true, isContains);
        }

        public Result<RegisteredUserService> Register(string email, string password)
        {
            Result<RegisteredUser> res = UsersAndPermissionsFacade.Register(email, password);
            if (res.ExecStatus)
            {
                return new Result<RegisteredUserService>(res.Message, res.ExecStatus, res.Data.GetDAL().Data);
            }
            else
            {
                return new Result<RegisteredUserService>(res.Message, res.ExecStatus, null);
            }
        }

        public Result<RegisteredUserService> Login(string email, string password)
        {
            Result<RegisteredUser> res = UsersAndPermissionsFacade.Login(email, password);
            if (res.ExecStatus)
            {
                return new Result<RegisteredUserService>(res.Message, res.ExecStatus, res.Data.GetDAL().Data);
            }
            else
            {
                return new Result<RegisteredUserService>(res.Message, res.ExecStatus, null);
            }
        }

        public Result<Boolean> LogOut(string email)
        {
            return UsersAndPermissionsFacade.LogOut(email);
        }

        public Result<ShoppingCartService> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            // TODO - lock products ?
            Result<ShoppingCart> res = UsersAndPermissionsFacade.Purchase(userID, paymentDetails, deliveryDetails);
            if (res.ExecStatus)
            {
                ShoppingCart purchasedCart = res.Data;
                ConcurrentDictionary<String, ShoppingBag> purchasedBags = purchasedCart.ShoppingBags;
                foreach(var bag in purchasedBags)
                {
                    Store store = StoresFacade.GetStore(bag.Key).Data;
                    store.UpdateInventory(bag.Value);
                    store.History.AddPurchasedShoppingBag(bag.Value);
                }
                return new Result<ShoppingCartService>(res.Message, true, res.Data.GetDAL().Data);
            }
            //else faild
            return new Result<ShoppingCartService>(res.Message, false, null);
        }
       
        public Result<double> GetTotalShoppingCartPrice(String userID)
        {
            return UsersAndPermissionsFacade.GetTotalShoppingCartPrice(userID);
        }

       
    }
}
