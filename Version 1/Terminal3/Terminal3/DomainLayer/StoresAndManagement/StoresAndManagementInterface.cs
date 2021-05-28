using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Concurrent;
using System.Linq;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DataAccessLayer;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        void resetSystem();
        Result<StoreService> OpenNewStore(String storeName, String userID , String storeID);
        Result<Boolean> CloseStore(String storeId, String userID);
        Result<StoreService> ReOpenStore(string storeId, string userID);
        Result<RegisteredUser> FindUserByEmail(String email);

        #region Inventory Management
        Result<ProductService> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductService> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<ProductService>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<List<StoreService>> SearchStore(IDictionary<String, Object> details);
        Result<List<Tuple<string, string>>> GetProductReview(String storeID, String productID);
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID, String storeID);        
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<List<Tuple<IStoreStaffService, PermissionService>>> GetStoreStaff(String ownerID, String storeID);
        #endregion

        #region User Actions
        Result<RegisteredUserService> Register(String email, String password, string Id);
        Result<RegisteredUserService> Login(String email, String password);
        Result<RegisteredUserService> Login(String email, String password, String guestUserID);
        Result<UserService> LogOut(String email);
        Result<Boolean> AddProductToCart(String userID, String productID, int productQuantity, String storeID);
        Result<Boolean> UpdateShoppingCart(string userID, string storeID, string productID, int quantity);
        Result<ShoppingCartService> GetUserShoppingCart(String userID);
        Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID, bool systemAdmin=false);
        Result<HistoryService> GetUserPurchaseHistory(String userID);
        Result<ProductService> AddProductReview(String userID, String storeID, String productID, String review);
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

        #region Display data
        List<StoreService> GetAllStoresToDisplay();
        List<ProductService> GetAllProductByStoreIDToDisplay(string storeID);
        Boolean[] GetPermission(string userID, string storeID);
        #endregion

        #region Policies Management
        Result<Boolean> AddDiscountPolicy(string storeId, Dictionary<string, object> info);
        Result<Boolean> AddDiscountPolicy(string storeId, Dictionary<string, object> info, String id);
        Result<Boolean> AddDiscountCondition(string storeId, Dictionary<string, object> info, String id);
        Result<Boolean> RemoveDiscountPolicy(string storeId, String id);
        Result<Boolean> RemoveDiscountCondition(string storeId, String id);
        Result<bool> EditDiscountPolicy(string storeId, Dictionary<string, object> info, String id);
        Result<bool> EditDiscountCondition(string storeId, Dictionary<string, object> info, String id);
        Result<IDiscountPolicyData> GetPoliciesData(string storeId);
        Result<IPurchasePolicyData> GetPurchasePolicyData(string storeId);
        Result<Boolean> AddPurchasePolicy(string storeId, Dictionary<string, object> info);
        Result<Boolean> AddPurchasePolicy(string storeId, Dictionary<string, object> info, string id);
        Result<Boolean> RemovePurchasePolicy(string storeId, string id);
        Result<bool> EditPurchasePolicy(string storeId, Dictionary<string, object> info, string id);
        #endregion
    }
    public class StoresAndManagementInterface : IStoresAndManagementInterface
    {
        // Properties
        public StoresFacade StoresFacade { get; }
        public UsersAndPermissionsFacade UsersAndPermissionsFacade { get; }

        public StoresAndManagementInterface(String admin_email, String admin_password)
        {
            StoresFacade = new StoresFacade();
            UsersAndPermissionsFacade = new UsersAndPermissionsFacade(admin_email, admin_password);
        }

        // Methods
        public Result<StoreService> OpenNewStore(String storeName, String userID , String storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser founder))  // Check if userID is a registered user
            {
                // Open store
                Result<Store> res = StoresFacade.OpenNewStore(founder, storeName, storeID);
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

        public Result<Boolean> CloseStore(string storeId, string userID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser founder))  // Check if userID is a registered user
            {
                // Close store
                return StoresFacade.CloseStore(founder, storeId);
            }
            //else
            return new Result<Boolean>($"Failed to close store (Id: {storeId}): {userID} is not a registered user.\n", false, false);
        }

        public Result<StoreService> ReOpenStore(string storeId, string userID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.TryGetValue(userID, out RegisteredUser owner))  // Check if userID is a registered user
            {
                // ReOpen store
                Result<Store> res = StoresFacade.ReOpenStore(owner, storeId);
                if (res.ExecStatus)
                {
                    return new Result<StoreService>(res.Message, true, res.Data.GetDAL().Data);
                }
                //else
                return new Result<StoreService>($"Failed to open store (Id: {storeId})", false, null);
            }
            //else
            return new Result<StoreService>($"Failed to reopen store (Id: {storeId}): {userID} is not a registered user.\n", false, null);
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

        public Result<Boolean> RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID)
        {
            if (UsersAndPermissionsFacade.RegisteredUsers.ContainsKey(removedOwnerID))  // Check if addedOwnerID is a registered user
            {
                return StoresFacade.RemoveStoreOwner(removedOwnerID, currentlyOwnerID, storeID);
            }
            //else
            return new Result<Boolean>($"Failed to remove store owner: {removedOwnerID} is not a registered user.\n", false, false);

        }

        public Result<bool> RemoveProductFromStore(String userID, String storeID, String productID)
        {
            return StoresFacade.RemoveProductFromStore(userID, storeID, productID);
        }

        public Result<bool> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions)
        {
            return StoresFacade.SetPermissions(storeID, managerID, ownerID, permissions);
        }

        public Result<List<Tuple<IStoreStaffService, PermissionService>>> GetStoreStaff(string userID, string storeID)
        {
            Result<Dictionary<IStoreStaff, Permission>> storeStaffResult = StoresFacade.GetStoreStaff(userID, storeID);
            if (storeStaffResult.ExecStatus)
            {
                Dictionary<IStoreStaff, Permission> storeStaff = storeStaffResult.Data;
                List<Tuple<IStoreStaffService, PermissionService>> storeStaffDAL = new List<Tuple<IStoreStaffService, PermissionService>>();

                foreach (var user in storeStaff)
                {
                    storeStaffDAL.Add( new Tuple<IStoreStaffService, PermissionService>((IStoreStaffService)user.Key.GetDAL().Data, user.Value.GetDAL().Data));
                }
                return new Result<List<Tuple<IStoreStaffService, PermissionService>>>(storeStaffResult.Message, true, storeStaffDAL);
            }

            return new Result<List<Tuple<IStoreStaffService, PermissionService>>>(storeStaffResult.Message , false , null);
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
       
        public Result<List<Tuple<string, string>>> GetProductReview(String storeID, String productID)
        {
            Result<ConcurrentDictionary<string,string>> res = StoresFacade.GetProductReview(storeID, productID);
            List<Tuple<string, string>> converted = new List<Tuple<string, string>>();
            if (res.Data != null)
            {
                foreach (string userId in res.Data.Keys)
                {
                    converted.Add(new Tuple<string, string>(userId, res.Data[userId]));
                }
            }
            return new Result<List<Tuple<string, string>>>(res.Message,res.ExecStatus,converted) ;
        }

        public Result<ProductService> AddProductReview(String userID, String storeID, String productID , String review)
        {
            Result<Store> storeRes = StoresFacade.GetStore(storeID);
            if (storeRes.ExecStatus)
            {                
                Result<Product> productRes = storeRes.Data.GetProduct(productID);    
                if (productRes.ExecStatus)
                {
                    Result<Product> result =  UsersAndPermissionsFacade.AddProductReview(userID, storeRes.Data, productRes.Data , review);
                    return result.ExecStatus ? new Result<ProductService>(result.Message, result.ExecStatus, result.Data.GetDAL().Data) :
                        new Result<ProductService>(result.Message, result.ExecStatus,null);
                }
                return new Result<ProductService>(productRes.Message, false, null);                
            }
            return new Result<ProductService>(storeRes.Message, false, null);
            
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

        public Result<RegisteredUserService> Register(string email, string password , string Id)
        {
            Result<RegisteredUser> res = UsersAndPermissionsFacade.Register(email, password, Id);
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
        public Result<RegisteredUserService> Login(string email, string password,string guestUserID)
        {
            Result<RegisteredUser> res = UsersAndPermissionsFacade.Login(email, password, guestUserID);
            if (res.ExecStatus)
            {
                return new Result<RegisteredUserService>(res.Message, res.ExecStatus, res.Data.GetDAL().Data);
            }
            else
            {
                return new Result<RegisteredUserService>(res.Message, res.ExecStatus, null);
            }
        }

        public Result<UserService> LogOut(string email)
        {
            Result<GuestUser> result = UsersAndPermissionsFacade.LogOut(email);
            if (result.ExecStatus)
            {
                return new Result<UserService>(result.Message, result.ExecStatus, result.Data.GetDAL().Data);
            }
            return new Result<UserService>(result.Message, result.ExecStatus,null);
        }

        public Result<ShoppingCartService> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            // TODO - lock products ?
            Result<ShoppingCart> res = UsersAndPermissionsFacade.Purchase(userID, paymentDetails, deliveryDetails);
            if (res.Data != null)
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

            //else failed
            return new Result<ShoppingCartService>(res.Message, false, null);
        }
       
        public Result<double> GetTotalShoppingCartPrice(String userID)
        {
            return UsersAndPermissionsFacade.GetTotalShoppingCartPrice(userID);
        }

        public List<StoreService> GetAllStoresToDisplay()
        {
            List<Store> stores = new List<Store>(StoresFacade.Stores.Values);
            List<StoreService> storesService = new List<StoreService>();
            foreach (Store store in stores)
            {
                StoreService storeService = store.GetDAL().Data;

                //clearing out unnecessary data 
                storeService.Founder = null;
                storeService.Owners = null;
                storeService.Managers = null;
                storeService.History = null;

                storesService.Add(storeService);
            }
            return storesService;
        }
        public List<ProductService> GetAllProductByStoreIDToDisplay(string storeID)
        {
            Store store = StoresFacade.Stores[storeID];
            List<Product> products = new List<Product>(store.InventoryManager.Products.Values);
            List<ProductService> productsService = new List<ProductService>();
            foreach (Product product  in products)
            {
                ProductService productService = product.GetDAL().Data;
                productsService.Add(productService);
            }
            return productsService;
        }

        public Boolean[] GetPermission(string userID, string storeID)
        {
            Store store = StoresFacade.Stores[storeID];
            return store.GetPermission(userID);
        }

        public Result<RegisteredUser> FindUserByEmail(String email)
        {
            return UsersAndPermissionsFacade.FindUserByEmail(email, UsersAndPermissionsFacade.RegisteredUsers);
        }

        public Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.AddDiscountPolicy(info);
        }

        public Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.AddDiscountPolicy(info, id);
        }

        public Result<bool> AddDiscountCondition(string storeId, Dictionary<string, object> info, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.AddDiscountCondition(info, id);
        }

        public Result<bool> RemoveDiscountPolicy(string storeId, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.RemoveDiscountPolicy(id);
        }

        public Result<bool> RemoveDiscountCondition(string storeId, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.RemoveDiscountCondition(id);
        }

        public Result<bool> EditDiscountPolicy(string storeId, Dictionary<string, object> info, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.EditDiscountPolicy(info, id);
        }

        public Result<bool> EditDiscountCondition(string storeId, Dictionary<string, object> info, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.EditDiscountCondition(info, id);
        }

        public Result<IDiscountPolicyData> GetPoliciesData(string storeId)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.GetPoliciesData();
        }

        public Result<IPurchasePolicyData> GetPurchasePolicyData(string storeId)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.GetPurchasePolicyData();
        }

        public Result<bool> RemovePurchasePolicy(string storeId, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.RemovePurchasePolicy(id);
        }

        public Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.AddPurchasePolicy(info);
        }

        public Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.AddPurchasePolicy(info, id);
        }

        public Result<bool> EditPurchasePolicy(string storeId, Dictionary<string, object> info, string id)
        {
            Store store = StoresFacade.Stores[storeId];
            return store.EditPurchasePolicy(info, id);
        }

        public void resetSystem()
        {
            Mapper.getInstance().clearDB();
            UsersAndPermissionsFacade.resetSystem();
            StoresFacade.resetSystem();
        }
    }
}
