using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoresFacade
    {
        Result<Store> OpenNewStore(RegisteredUser founder, String storeName , String storeID);
        Result<Boolean> CloseStore(RegisteredUser founder, String storeId);
        Result<Store> ReOpenStore(RegisteredUser owner, String storeId);

        #region Inventory Management
        Result<Product> AddProductToStore(String userID, String storeID, String productName, double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<Product> EditProductDetails(String userID, String storeID, String productID, IDictionary<String, Object> details);
        Result<List<Product>> SearchProduct(IDictionary<String, Object> productDetails);
        Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);
        Result<List<Store>> SearchStore(IDictionary<String, Object> details);
        Result<Store> GetStore(String storeID);

        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID, String storeID);        
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(string ownerID, string storeID);
        Result<History> GetStorePurchaseHistory(String userID, String storeID, bool sysAdmin);
        #endregion

        #region Policies Management
        Result<Boolean> AddDiscountPolicy(string storeId, Dictionary<string, object> info);
        Result<Boolean> AddDiscountPolicy(string storeId, Dictionary<string, object> info, String id);
        Result<Boolean> AddDiscountCondition(string storeId, Dictionary<string, object> info, String id);
        Result<Boolean> RemoveDiscountPolicy(string storeId, String id);
        Result<Boolean> RemoveDiscountCondition(string storeId, String id);
        Result<IDiscountPolicyData> GetPoliciesData(string storeId);
        Result<IPurchasePolicyData> GetPurchasePolicyData(string storeId);
        Result<Boolean> AddPurchasePolicy(string storeId, Dictionary<string, object> info);
        Result<Boolean> AddPurchasePolicy(string storeId, Dictionary<string, object> info, string id);
        Result<Boolean> RemovePurchasePolicy(string storeId, string id);
        #endregion
    }

    public class StoresFacade : IStoresFacade
    {
        public ConcurrentDictionary<String, Store> Stores { get; }
        public ConcurrentDictionary<String, Store> ClosedStores { get; }

        //TODO: Change constructor if needed (initializer?)
        public StoresFacade()
        {
            Stores = new ConcurrentDictionary<String, Store>();
            ClosedStores = new ConcurrentDictionary<String, Store>();
        }

        //TODO: Implement all functions

        #region Inventory Management
        public Result<Product> AddProductToStore(String userID, String storeID, String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.AddNewProduct(userID, productName, price, initialQuantity, category, keywords);                
            }
            //else failed
            return new Result<Product>($"Store ID {storeID} not found.\n", false, null);
            
        }
        
        public Result<Boolean> RemoveProductFromStore(string userID, string storeID, string productID)
        {           
                if (Stores.TryGetValue(storeID, out Store store))
                {
                    Result<Product> res = store.RemoveProduct(userID, productID);
                    if (res.ExecStatus)
                    {
                        return new Result<Boolean>(res.Message, res.ExecStatus, true);
                    }
                    //else failed
                    return new Result<Boolean>(res.Message, res.ExecStatus, false);
                }
                //else failed
                return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);           
        }
        
        public Result<Product> EditProductDetails(string userID, string storeID, string productID, IDictionary<String, Object> details)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.EditProduct(userID, productID, details);          
            }
            //else failed
            return new Result<Product>($"Store ID {storeID} not found.\n", false, null);          
        }
        #endregion

        #region Staff Management
        public Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.AddStoreOwner(futureOwner, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }
        
        public Result<Boolean> AddStoreManager(RegisteredUser futureManager, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.AddStoreManager(futureManager, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<Boolean> RemoveStoreManager(String removedManagerID, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.RemoveStoreManager(removedManagerID, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<Boolean> RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.RemoveStoreOwner(removedOwnerID, currentlyOwnerID);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<List<Product>> SearchProduct(IDictionary<String, Object> searchAttributes)
        {
            List<Product> searchResult = new List<Product>();
            foreach(Store store in this.Stores.Values)
            {
                Result<List<Product>> storeResult = store.SearchProduct(searchAttributes);
                if (storeResult.ExecStatus)
                {
                    searchResult.AddRange(storeResult.Data);
                }
            }
            if (searchResult.Count > 0) {
                return new Result<List<Product>>($"{searchResult.Count } products have been found\n",true, searchResult); 
            }
            else{
                return new Result<List<Product>>($"No products have been found\n", false, null);
            }

        }

        public Result<List<Store>> SearchStore(IDictionary<String, Object> details)
        {
            List<Store> searchResult = new List<Store>();
            foreach(Store store in Stores.Values)
            {
                if (checkStoreAttributes(store,details))
                {
                    searchResult.Add(store);
                }
            }
            if (searchResult.Count > 0)
            {
                return new Result<List<Store>>($"{searchResult.Count } stores have been found\n", true, searchResult);
            }
            else
            {
                return new Result<List<Store>>($"No stores have been found\n", false, null);
            }
        }

        public Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(string userID, string storeID)
        {            
            if(Stores.TryGetValue(storeID, out Store store))
            {
                return store.GetStoreStaff(userID);
            }
            return new Result<Dictionary<IStoreStaff, Permission>>("The given store ID does not exists", false, null);
            
        }
        #endregion

        public Result<History> GetStorePurchaseHistory(string userID, string storeID,bool sysAdmin)
        {
            if(Stores.TryGetValue(storeID, out Store store))
            {
                return store.GetStorePurchaseHistory(userID,sysAdmin);
            }
            return new Result<History>("Store Id does not exists\n", false, null);
        }

        public Result<Store> OpenNewStore(RegisteredUser founder, string storeName, String storeID)
        {
            Store newStore = new Store(storeName, founder, storeID);
            Stores.TryAdd(newStore.Id, newStore);
            NotificationManager notificationManager = new NotificationManager(newStore);
            newStore.NotificationManager = notificationManager;
            newStore.NotificationManager.notifyStoreOpened();

            return new Result<Store>($"New store {storeName}, ID: {newStore.Id} was created successfully by {founder}\n", true, newStore);
        }

        public Result<Boolean> CloseStore(RegisteredUser founder, string storeId)
        {
            
            if(Stores.TryRemove(storeId , out Store store))
            {
                if(store.Founder.GetId() == founder.Id)
                {
                    ClosedStores.TryAdd(storeId, store);
                    store.NotificationManager.notifyStoreClosed();
                    return new Result<bool>($"The store {store.Name} is closed\n", true, true);
                }
                // else faild so return the store to the Stores list
                Stores.TryAdd(storeId, store);
                return new Result<bool>($"Registered user (Id:{founder.Id}) is not the store founder , therefore can not close the store\n", false, false);
            }
            //else faild
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<Store> ReOpenStore(RegisteredUser owner, string storeId)
        {
            if (ClosedStores.TryRemove(storeId, out Store store))
            {
                if (store.Owners.ContainsKey(owner.Id))
                {
                    Stores.TryAdd(storeId, store);
                    store.NotificationManager.notifyStoreOpened();
                    return new Result<Store>($"The store {store.Name} is reopened\n", true, store);
                }
                // else faild so return the store to the closed stores list
                ClosedStores.TryAdd(storeId, store);
                return new Result<Store>($"Registered user (Id:{owner.Id}) is not one of the store owners , therefore can not reopen the store\n", false, null);
            }
            //else faild
            return new Result<Store>("Store is not closed or does not exists, therefore can not reopen it\n", false, null);
        }


        public Result<bool> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.SetPermissions(managerID, ownerID, permissions);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<bool> RemovePermissions(string storeID, string managerID, string ownerID, LinkedList<int> permissions)
        {
            if (Stores.TryGetValue(storeID, out Store store))     // Check if storeID exists
            {
                return store.RemovePermissions(managerID, ownerID, permissions);
            }
            //else failed
            return new Result<Boolean>($"Store ID {storeID} not found.\n", false, false);
        }

        public Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID)
        {
            if(Stores.TryGetValue(storeID , out Store store))
            {
                return store.GetProductReview(productID);
            }
            return new Result<ConcurrentDictionary<string, string>>("Store does not exists\n", false, null);
        }

        public Result<Store> GetStore(String storeID)
        {
            if (Stores.TryGetValue(storeID, out Store store))
            {
                return new Result<Store>("", true, store);
            }
            return new Result<Store>("Store does not exists\n", false, null);
        }

        /// <summary>
        ///  Filter out product if its not meet the search criteria
        /// </summary>
        /// <param name="store"></param>
        /// <param name="searchAttributes"></param>
        /// <returns></returns>
        internal bool checkStoreAttributes(Store store, IDictionary<String, Object> searchAttributes)
        {
            Boolean result = true;
            ICollection<String> properties = searchAttributes.Keys;
            foreach (string property in properties)
            {
                var value = searchAttributes[property];
                switch (property.ToLower())
                {
                    case "name":
                        if (!store.Name.ToLower().Contains(((string)value).ToLower())) { result = false; }
                        break;
                    case "rating":
                        if (store.Rating < (Double)value) { result = false; }
                        break;
                }
            }
            return result;
        }

        public double GetTotalBagPrice(string storeId, ConcurrentDictionary<Product, int> products, string discountCode = "")
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.GetTotalBagPrice(products, discountCode);
            }
            new Result<Store>("Store does not exists\n", false, null);
            return -1;
        }

        public Result<bool> AdheresToPolicy(string storeId, ConcurrentDictionary<Product, int> products, User user)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.AdheresToPolicy(products, user);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.AddDiscountPolicy(info);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<bool> AddDiscountPolicy(string storeId, Dictionary<string, object> info, string id)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.AddDiscountPolicy(info, id);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<bool> AddDiscountCondition(string storeId, Dictionary<string, object> info, string id)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.AddDiscountCondition(info, id);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<bool> RemoveDiscountPolicy(string storeId, string id)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.RemoveDiscountPolicy(id);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<bool> RemoveDiscountCondition(string storeId, string id)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.RemoveDiscountCondition(id);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<IDiscountPolicyData> GetPoliciesData(string storeId)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.GetPoliciesData();
            }
            return new Result<IDiscountPolicyData>("Store does not exists\n", false, null);
        }

        public Result<bool> RemovePurchasePolicy(string storeId, string id)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.RemovePurchasePolicy(id);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<IPurchasePolicyData> GetPurchasePolicyData(string storeId)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.GetPurchasePolicyData();
            }
            return new Result<IPurchasePolicyData>("Store does not exists\n", false, null);
        }

        public Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.AddPurchasePolicy(info);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }

        public Result<bool> AddPurchasePolicy(string storeId, Dictionary<string, object> info, string id)
        {
            if (Stores.TryGetValue(storeId, out Store store))
            {
                return store.AddPurchasePolicy(info, id);
            }
            return new Result<bool>("Store does not exists\n", false, false);
        }
        public void resetSystem()
        {
            Stores.Clear();
        }
    }
}
