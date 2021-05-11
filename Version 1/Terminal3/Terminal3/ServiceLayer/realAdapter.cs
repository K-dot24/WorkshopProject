using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;


namespace XUnitTestTerminal3.AcceptanceTests.Utils
{
    public class RealAdapter : ISystemInterface
    {
        public ECommerceSystem system = new ECommerceSystem();

        //TODO
        public Result<bool> AddProductReview(string userID, string storeID, string productID, string review)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddProductToCart(string userID, string ProductID, int ProductQuantity, string StoreID)
        {
            Result<bool> fromSystem = system.AddProductToCart(userID, ProductID, ProductQuantity, StoreID);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<string> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            Result<ProductService> fromSystem = system.AddProductToStore(userID, storeID, productName, price, initialQuantity, category);
            if (fromSystem.ExecStatus)
            {
                return new Result<string>("", fromSystem.ExecStatus, fromSystem.Data.Id);
            }
            else
            {
                return new Result<string>("", fromSystem.ExecStatus, null);
            }
        }

        public Result<bool> AddStoreManager(string addedManagerID, string currentlyOwnerID, string storeID)
        {
            Result<bool> fromSystem = system.AddStoreManager(addedManagerID, currentlyOwnerID, storeID);         
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            Result <bool> fromSystem = system.AddStoreOwner(addedOwnerID, currentlyOwnerID, storeID);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<string> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            Result<ProductService> fromSystem = system.EditProductDetails(userID, storeID, productID, details);
            if(fromSystem.ExecStatus)
                return new Result<string>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.Data.Id);
            return new Result<string>(fromSystem.Message, fromSystem.ExecStatus, null);
        }

        //TODO
        public Result<List<String>> GetStorePurchaseHistory(String ownerID, String storeID)
        {
            Result<HistoryService> fromSystem = system.GetStorePurchaseHistory(ownerID, storeID);
            if (fromSystem.ExecStatus)
            {
                //List<string> Ids = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags).ForEach(bag => bag.Id);
                List<ShoppingBagService> ShoppingBags = new List<ShoppingBagService>(fromSystem.Data.ShoppingBags);
                List<string> Ids = new List<string>();
                foreach (ShoppingBagService dal in ShoppingBags) { Ids.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, Ids);
            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, new List<string>());
            }
        }

        public Result<Dictionary<String, List<int>>> GetStoreStaff(String ownerID, String storeID)
        {
            Result<List<Tuple<IStoreStaffService, PermissionService>>> fromSystem = system.GetStoreStaff(ownerID, storeID);
            //Result<Dictionary<IStoreStaffService, PermissionService>> fromSystem = system.GetStoreStaff(ownerID, storeID);
            if (fromSystem.ExecStatus)
            {
                // List<string> userIDS = (new List<IStoreStaffDAL>(fromSystem.Data.Keys)).Select(userdal => userdal.Id)
                //List<IStoreStaffService> StoreStaffDals = new List<IStoreStaffService>(fromSystem.Data.Keys);
                List<IStoreStaffService> StoreStaffDals = new List<IStoreStaffService>();
                List<PermissionService> values = new List<PermissionService>();
                foreach (Tuple<IStoreStaffService,PermissionService> tuple in fromSystem.Data)
                {
                    StoreStaffDals.Add(tuple.Item1);
                    values.Add(tuple.Item2);
                }
                List<string> userIDS = new List<string>();
                foreach (IStoreStaffService dal in StoreStaffDals) { userIDS.Add(dal.Id); }
                List<List<int>> userPermisions = new List<List<int>>();
                foreach (PermissionService permission in values)
                {
                    List<int> permissionList = new List<int>();
                    if (permission.isOwner) { permissionList.Add((int)Methods.AllPermissions); }
                    else
                    {
                        for (int i = 0; i < permission.functionsBitMask.Length; i++)
                        {
                            if (permission.functionsBitMask[i]) { permissionList.Add(i); }
                        }
                    }
                    userPermisions.Add(permissionList); 
                }
                Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>();

                dic = userIDS.Zip(userPermisions, (k, v) => new { k, v })
                                        .ToDictionary(x => x.k, x => x.v);
                return new Result<Dictionary<string, List<int>>>("", fromSystem.ExecStatus, dic);
            }
            else
            {
                return new Result<Dictionary<string, List<int>>>("", fromSystem.ExecStatus, null);
            }

        }

        public Result<double> GetTotalShoppingCartPrice(string userID)
        {
            Result<double> fromSystem = system.GetTotalShoppingCartPrice(userID);
            if (fromSystem.ExecStatus)
                return new Result<double>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.Data);
            return new Result<double>(fromSystem.Message, fromSystem.ExecStatus, -1.0);
        }

        //TODO
        public Result<List<string>> GetUserPurchaseHistory(string userID)
        {
            Result<HistoryService> fromSystem = system.GetUserPurchaseHistory(userID);
            if (fromSystem.ExecStatus)
            {
                //List<string> Ids = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags).ForEach(bag => bag.Id);
                List<ShoppingBagService> ShoppingBags = new List<ShoppingBagService>(fromSystem.Data.ShoppingBags);
                List<string> Ids = new List<string>();
                foreach (ShoppingBagService dal in ShoppingBags) { Ids.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, Ids);
            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, new List<string>());
            }
        }

        public Result<Dictionary<string, int>> GetUserShoppingBag(string userID, string shoppingBagID)
        {
            ShoppingCartService shoppingCart = system.GetUserShoppingCart(userID).Data;
            if (shoppingCart == null)
                return new Result<Dictionary< string, int>>("Failed to find the shopping cart", false, null);
            foreach(ShoppingBagService shoppingBag in shoppingCart.ShoppingBags)
            {
                if(shoppingBag.Id == shoppingBagID)
                    return new Result<Dictionary<string, int>>("", true, ConvertObjectToID(shoppingBag.Products));
            }
            return new Result<Dictionary<string, int>>("Failed to find the shopping bag", false, null);
        }

        private Dictionary<string, int> ConvertObjectToID(LinkedList<Tuple<ProductService, int>> list)
        {
            Dictionary<string, int>  result = new Dictionary<string, int>();
            foreach(Tuple<ProductService,int> pair in list)
            {
                result.Add(pair.Item1.Id,pair.Item2);
            }
            return result;
        }

        public Result<List<string>> GetUserShoppingCart(string userID)
        {
            Result<ShoppingCartService> fromSystem = system.GetUserShoppingCart(userID);
            if (fromSystem.ExecStatus)
            {
                //List<string> shoppingBagsIds = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags).ForEach(bag => bag.Id);
                List<ShoppingBagService> ShoppingBags = new List<ShoppingBagService>(fromSystem.Data.ShoppingBags);
                List<string> Ids = new List<string>();
                foreach (ShoppingBagService dal in ShoppingBags) { Ids.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, Ids);

            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, null);

            }
        }

        public Result<string> Login(string email, string password)
        {
            Result<RegisteredUserService> fromSystem = system.Login(email, password);
            if (fromSystem.ExecStatus)
            {
                return new Result<string>("", fromSystem.ExecStatus, fromSystem.Data.Id);
            }
            else
            {
                return new Result<string>("", fromSystem.ExecStatus, null);
            }
        }

        public Result<bool> LogOut(string email)
        {
            Result<UserService> fromSystem = system.LogOut(email);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<string> OpenNewStore(string storeName, string userID)
        {
            Result<StoreService> fromSystem = system.OpenNewStore(storeName, userID);
            if (fromSystem.ExecStatus)
            {
                return new Result<string>("", fromSystem.ExecStatus, fromSystem.Data.Id);
            }
            else
            {
                return new Result<string>("", fromSystem.ExecStatus, null);
            }
        }

        //TODO
        public Result<List<String>> Purchase(string userID, IDictionary<string, object> paymentDetails, IDictionary<string, object> deliveryDetails)
        {
            Result<ShoppingCartService> fromSystem = system.Purchase(userID, paymentDetails, deliveryDetails);
            if (fromSystem.ExecStatus)
            {
                List<string> bagsIDS = new List<string>();
                foreach (ShoppingBagService dal in fromSystem.Data.ShoppingBags) { bagsIDS.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, bagsIDS);

            }
            else
                return new Result<List<string>>(fromSystem.Message, fromSystem.ExecStatus, null);

        }

        public Result<bool> Register(string email, string password)
        {
            Result<bool> fromSystem = new Result<bool>(system.Register(email, password).ExecStatus) ;
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            Result <bool> fromSystem = system.RemoveProductFromStore(userID, storeID, productID);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<bool> ResetSystem(string sysAdminID)
        {
            Result<bool> fromSystem = system.ResetSystem(sysAdminID);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<List<string>> SearchProduct(IDictionary<string, object> productDetails)
        {
            Result<List<ProductService>> fromSystem = system.SearchProduct(productDetails);
            if (fromSystem.ExecStatus)
            {
                List<string> productIDS = new List<string>();
                foreach (ProductService dal in fromSystem.Data) { productIDS.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, productIDS);

            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, null);
            }
        }

        public Result<List<String>> SearchStore(IDictionary<string, object> details)
        {
            Result<List<StoreService>> fromSystem = system.SearchStore(details);
            if (fromSystem.ExecStatus) { 
                List<string> storesNames = new List<string>();
                foreach (StoreService dal in fromSystem.Data) { storesNames.Add(dal.Name); }
                return new Result<List<String>>(fromSystem.Message, fromSystem.ExecStatus, storesNames);
            }
            else
                return new Result<List<String>>("", fromSystem.ExecStatus, null);
        }
         
        public Result<bool> SetPermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            Result<bool> fromSystem = system.SetPermissions(storeID, managerID, ownerID, permissions);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

        public Result<bool> UpdateShoppingCart(string userID, string storeID, string productID, int quantity)
        {
            Result<bool> fromSystem = system.UpdateShoppingCart(userID, storeID, productID, quantity);
            if (fromSystem.ExecStatus)
                return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
            return new Result<bool>(fromSystem.Message, fromSystem.ExecStatus, fromSystem.ExecStatus);
        }

    }
}
