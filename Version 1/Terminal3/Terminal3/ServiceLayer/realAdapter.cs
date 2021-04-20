﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer;
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
            return system.AddProductToCart(userID, ProductID, ProductQuantity, StoreID);
        }

        public Result<string> AddProductToStore(string userID, string storeID, string productName, double price, int initialQuantity, string category)
        {
            Result<ProductDAL> fromSystem = system.AddProductToStore(userID, storeID, productName, price, initialQuantity, category);
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
            return system.AddStoreManager(addedManagerID, currentlyOwnerID, storeID);
        }

        public Result<bool> AddStoreOwner(string addedOwnerID, string currentlyOwnerID, string storeID)
        {
            return system.AddStoreOwner(addedOwnerID, currentlyOwnerID, storeID);
        }

        public Result<string> EditProductDetails(string userID, string storeID, string productID, IDictionary<string, object> details)
        {
            Result<ProductDAL> fromSystem = system.EditProductDetails(userID, storeID, productID, details);
            return new Result<string>("", fromSystem.ExecStatus, fromSystem.Data.Id);
        }

        //TODO
        public Result<List<String>> GetStorePurchaseHistory(String ownerID, String storeID)
        {
            Result<HistoryDAL> fromSystem = system.GetStorePurchaseHistory(ownerID, storeID);
            if (fromSystem.ExecStatus)
            {
                //List<string> Ids = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags).ForEach(bag => bag.Id);
                List<ShoppingBagDAL> ShoppingBags = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags);
                List<string> Ids = new List<string>();
                foreach (ShoppingBagDAL dal in ShoppingBags) { Ids.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, Ids);
            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, new List<string>());
            }
        }

        public Result<Dictionary<String, List<int>>> GetStoreStaff(String ownerID, String storeID)
        {
            Result<Dictionary<IStoreStaffDAL, PermissionDAL>> fromSystem = system.GetStoreStaff(ownerID, storeID);
            if (fromSystem.ExecStatus)
            {
                // List<string> userIDS = (new List<IStoreStaffDAL>(fromSystem.Data.Keys)).Select(userdal => userdal.Id)
                List<IStoreStaffDAL> StoreStaffDals = new List<IStoreStaffDAL>(fromSystem.Data.Keys);
                List<string> userIDS = new List<string>();
                foreach (IStoreStaffDAL dal in StoreStaffDals) { userIDS.Add(dal.Id); }
                List<List<int>> userPermisions = new List<List<int>>();
                foreach (PermissionDAL permission in fromSystem.Data.Values)
                {
                    List<int> permissionList = new List<int>();
                    for (int i = 0; i < permission.functionsBitMask.Length; i++)
                    {
                        if (permission.functionsBitMask[i]) { permissionList.Add(i); }
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
            return system.GetTotalShoppingCartPrice(userID);
        }

        //TODO
        public Result<List<string>> GetUserPurchaseHistory(string userID)
        {
            Result<HistoryDAL> fromSystem = system.GetUserPurchaseHistory(userID);
            if (fromSystem.ExecStatus)
            {
                //List<string> Ids = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags).ForEach(bag => bag.Id);
                List<ShoppingBagDAL> ShoppingBags = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags);
                List<string> Ids = new List<string>();
                foreach (ShoppingBagDAL dal in ShoppingBags) { Ids.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, Ids);
            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, new List<string>());
            }
        }

        public Result<Dictionary<string, int>> GetUserShoppingBag(string userID, string shoppingBagID)
        {
            throw new NotImplementedException();
        }

        public Result<List<string>> GetUserShoppingCart(string userID)
        {
            Result<ShoppingCartDAL> fromSystem = system.GetUserShoppingCart(userID);
            if (fromSystem.ExecStatus)
            {
                //List<string> shoppingBagsIds = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags).ForEach(bag => bag.Id);
                List<ShoppingBagDAL> ShoppingBags = new List<ShoppingBagDAL>(fromSystem.Data.ShoppingBags);
                List<string> Ids = new List<string>();
                foreach (ShoppingBagDAL dal in ShoppingBags) { Ids.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, Ids);

            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, null);

            }
        }

        public Result<string> Login(string email, string password)
        {
            Result<RegisteredUserDAL> fromSystem = system.Login(email, password);
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
            return system.LogOut(email);
        }

        public Result<string> OpenNewStore(string storeName, string userID)
        {
            Result<StoreDAL> fromSystem = system.OpenNewStore(storeName, userID);
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
            Result<ShoppingCartDAL> fromSystem = system.Purchase(userID, paymentDetails, deliveryDetails);
            if (fromSystem.ExecStatus)
            {
                List<string> bagsIDS = new List<string>();
                foreach (ShoppingBagDAL dal in fromSystem.Data.ShoppingBags) { bagsIDS.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, bagsIDS);

            }
            else
                return new Result<List<string>>(fromSystem.Message, fromSystem.ExecStatus, null);

        }

        public Result<bool> Register(string email, string password)
        {
            return new Result<bool>(system.Register(email, password).ExecStatus) ;
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemoveProductFromStore(string userID, string storeID, string productID)
        {
            return system.RemoveProductFromStore(userID, storeID, productID);
        }

        public Result<bool> ResetSystem(string sysAdminID)
        {
            return system.ResetSystem(sysAdminID);
        }

        public Result<List<string>> SearchProduct(IDictionary<string, object> productDetails)
        {
            Result<List<ProductDAL>> fromSystem = system.SearchProduct(productDetails);
            if (fromSystem.ExecStatus)
            {
                List<string> productIDS = new List<string>();
                foreach (ProductDAL dal in fromSystem.Data) { productIDS.Add(dal.Id); }
                return new Result<List<string>>("", fromSystem.ExecStatus, productIDS);

            }
            else
            {
                return new Result<List<string>>("", fromSystem.ExecStatus, null);
            }
        }

        public Result<List<String>> SearchStore(IDictionary<string, object> details)
        {
            Result<List<StoreDAL>> fromSystem = system.SearchStore(details);
            if (fromSystem.ExecStatus) { 
                List<string> storesNames = new List<string>();
                foreach (StoreDAL dal in fromSystem.Data) { storesNames.Add(dal.Name); }
                return new Result<List<String>>(fromSystem.Message, fromSystem.ExecStatus, storesNames);
            }
            else
                return new Result<List<String>>("", fromSystem.ExecStatus, null);
        }
         
        public Result<bool> SetPermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            return system.SetPermissions(storeID, managerID, ownerID, permissions);
        }

        public Result<bool> UpdateShoppingCart(string userID, string shoppingBagID, string productID, int quantity)
        {
            return system.UpdateShoppingCart(userID, shoppingBagID, productID, quantity);
        }

    }
}
