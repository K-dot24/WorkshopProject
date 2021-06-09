using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Threading;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DataAccessLayer.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;
using Terminal3.DataAccessLayer;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoreOperations
    {
        //TODO: Update functions sigs

        #region Inventory Management
        Result<Product> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Product> RemoveProduct(String userID, String productID);
        Result<Product> EditProduct(String userID, String productID, IDictionary<String, Object> details);
        Result<Boolean> UpdateInventory(ShoppingBag bag);
        #endregion

        #region Staff Management
        Result<StoreOwner> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID);
        Result<StoreManager> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID);        
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(String userID);
        #endregion

        #region Policies Management
        double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "");
        Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user);
        Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info);
        Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info, String id);
        Result<IDiscountCondition> AddDiscountCondition(Dictionary<string, object> info, String id);
        Result<IDiscountPolicy> RemoveDiscountPolicy(String id);
        Result<IDiscountCondition> RemoveDiscountCondition(String id);
        Result<Boolean> EditDiscountPolicy(Dictionary<string, object> info, String id);
        Result<Boolean> EditDiscountCondition(Dictionary<string, object> info, String id);
        Result<IDictionary<string, object>> GetPoliciesData();
        Result<IDictionary<string, object>> GetPurchasePolicyData();
        Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info);
        Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info, string id);
        Result<IPurchasePolicy> RemovePurchasePolicy(string id);
        Result<Boolean> EditPurchasePolicy(Dictionary<string, object> info, string id);
        #endregion

        #region Information        
        Result<ConcurrentDictionary<String, String>> GetProductReview(String productID);
        Result<History> GetStorePurchaseHistory(string ownerID,bool sysAdmin);
        #endregion
    }
    public class Store : IStoreOperations
    {
        public String Id { get; }
        public String Name { get; }
        public StoreOwner Founder { get; set; }
        public ConcurrentDictionary<String, StoreOwner> Owners { get; set; }
        public ConcurrentDictionary<String, StoreManager> Managers { get; set; }
        public InventoryManager InventoryManager { get; }
        public PolicyManager PolicyManager { get; set; }
        public History History { get; set; }
        public Double Rating { get; private set; }
        public int NumberOfRates { get; private set; }
        public NotificationManager NotificationManager { get; set; }
        public Boolean isClosed { get; set; }

        //Constructors
        public Store(String name, RegisteredUser founder , String storeID = "-1")
        {
            if (storeID.Equals("-1"))
                Id = Service.GenerateId();
            else
                Id = storeID;
            Name = name;
            Founder = new StoreOwner(founder, this, null);
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            InventoryManager = new InventoryManager();
            PolicyManager = new PolicyManager();
            History = new History();
            isClosed = false;

            //Add founder to list of owners
            Owners.TryAdd(founder.Id, Founder);

            //TODO: Complete when policies done properly
            //Add default policies
        }
       

        public Store(string id, string name, InventoryManager inventoryManager, History history, double rating, int numberOfRates, NotificationManager notificationManager , Boolean isClosed=false)
        {
            Id = id;
            Name = name;            
            InventoryManager = inventoryManager;     
            History = history;
            Rating = rating;
            NumberOfRates = numberOfRates;
            NotificationManager = notificationManager;
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            this.isClosed = isClosed;

        }
        public Store(string id, string name, InventoryManager inventoryManager, double rating, int numberOfRates, NotificationManager notificationManager, Boolean isClosed = false)
        {
            Id = id;
            Name = name;
            InventoryManager = inventoryManager;
            Rating = rating;
            NumberOfRates = numberOfRates;
            NotificationManager = notificationManager;
            History = new History();
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            this.isClosed = isClosed;
        }


        //For Testing ONLY
        public Store(string id, String name, RegisteredUser founder)
        {
            Id = id;
            Name = name;
            Founder = new StoreOwner(founder, this, null);
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            InventoryManager = new InventoryManager();
            PolicyManager = new PolicyManager();
            History = new History();
            isClosed = false;


            //Add founder to list of owners
            Owners.TryAdd(founder.Id, Founder);

            //TODO: Complete when policies done properly
            //Add default policies
            //PolicyManager.SetPurchasePolicy(Purchases.BuyNow, true);
            //PolicyManager.SetDiscountPolicy(Discounts.Visible, true);
        }

        //TODO: Implement all functions
        //Methods
        public Result<Double> AddRating(Double rate)
        {
            if (rate > 5 || rate < 1)
            {
                return new Result<Double>($"Store {Name} could not be rated. Please use number between 1 to 5\n", false, Rating);
            }
            else
            {
                this.NumberOfRates = NumberOfRates + 1;
                //Rating = (Rating + rate) / NumberOfRates;
                Rating = (Rating *(NumberOfRates-1)+rate) / NumberOfRates;

                return new Result<Double>($"Store {Name} rate is: {Rating}\n", true, Rating);
            }
        }
        public Result<List<Product>> SearchProduct(IDictionary<String, Object> searchAttributes)
        {
            return InventoryManager.SearchProduct(this.Rating, searchAttributes);
        }

        #region Inventory Management
        public Result<Product> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            if (CheckIfStoreOwner(userID) || CheckStoreManagerAndPermissions(userID, Methods.AddNewProduct))
            {
                Result<Product> res = InventoryManager.AddNewProduct(productName, price, initialQuantity, category, keywords);
                if (res.ExecStatus)
                {
                    res.Data.NotificationManager = this.NotificationManager;                    
                }
                return res;
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to add new product to {this.Name}\n", false, null);
            }
        }
        public Result<Product> RemoveProduct(String userID, String productID)
        {
            try
            {
                Monitor.Enter(productID);
                try
                {
                    if (CheckIfStoreOwner(userID) || CheckStoreManagerAndPermissions(userID, Methods.RemoveProduct))
                    {
                        return InventoryManager.RemoveProduct(productID);
                    }
                    else
                    {
                        return new Result<Product>($"{userID} does not have permissions to remove products from {this.Name}\n", false, null);
                    }
                }
                finally
                {
                    Monitor.Exit(productID);
                }
            }
            catch (SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<Product>(SyncEx.Message, false, null);
            }
        }
        public Result<Product> EditProduct(String userID, String productID, IDictionary<String, Object> details)
        {
            if (CheckIfStoreOwner(userID) || CheckStoreManagerAndPermissions(userID, Methods.EditProduct))
            {
                return InventoryManager.EditProduct(productID, details);
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to edit products' information in {this.Name}\n", false, null);
            }
        }
        public Result<bool> UpdateInventory(ShoppingBag bag)
        {
            ConcurrentDictionary<Product, int> product_quantity = bag.Products;     // <Product, Quantity user bought>
            foreach(var product in product_quantity)
            {
                product.Key.UpdatePurchasedProductQuantity(product.Value);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", product.Key.Id);
                var update_product = Builders<BsonDocument>.Update.Set("Quantity", product.Key.Quantity);
                Mapper.getInstance().UpdateProduct(filter , update_product);
            }

            return new Result<bool>("Store inventory updated successuly\n", true, true);

        }
        #endregion

        public Result<StoreOwner> AddStoreOwner(RegisteredUser futureOwner, string currentlyOwnerID)
        {
            try
            {
                Monitor.Enter(futureOwner);
                try
                {
                    // Check new owner not already an owner + appointing owner is not a fraud or the appointing user is a manager with the right permissions
                    if (!CheckIfStoreOwner(futureOwner.Id))
                    {
                        StoreOwner newOwner;
                        if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner))
                        {
                            newOwner = new StoreOwner(futureOwner, this, owner);
                            Owners.TryAdd(futureOwner.Id, newOwner);
                            owner.StoreOwners.AddLast(newOwner);
                        }
                        else
                        {
                            return new Result<StoreOwner>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}) " +
                                $"is not an owner at ${this.Name}.\n", false, null);
                        }


                        if (CheckIfStoreManager(futureOwner.Id)) //remove from managers list if needed
                        {
                            Managers.TryRemove(futureOwner.Id, out _);
                        }

                        return new Result<StoreOwner>("User successfuly added as the store owner\n", true, newOwner);
                    }
                    //else failed
                    return new Result<StoreOwner>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}). The user is already an owner.\n", false, null);
                }
                finally
                {
                    Monitor.Exit(futureOwner);
                }
            }
            catch (SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<StoreOwner>(SyncEx.Message, false, null);
            }
        }

        public Result<StoreManager> AddStoreManager(RegisteredUser futureManager, string currentlyOwnerID)
        {

            try
            {
                Monitor.Enter(futureManager);
                try
                {
                    // Check new manager not already an owner/manager + appointing owner is not a fraud or the appointing user is a manager with the right permissions
                    if (!CheckIfStoreManager(futureManager.Id) && !CheckIfStoreOwner(futureManager.Id))
                    {
                        StoreManager newManager;
                        if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner))
                        {
                            newManager = new StoreManager(futureManager, this, new Permission(), owner);
                            owner.StoreManagers.AddLast(newManager);
                            Managers.TryAdd(futureManager.Id, newManager);
                        }
                        else if (Managers.TryGetValue(currentlyOwnerID, out StoreManager manager) && CheckStoreManagerAndPermissions(currentlyOwnerID, Methods.AddStoreManager))
                        {
                            newManager = new StoreManager(futureManager, this, new Permission(), manager);
                            Managers.TryAdd(futureManager.Id, newManager);
                        }
                        else
                        {
                            return new Result<StoreManager>($"Failed to add store manager because appoitend user is not an owner or manager with relevant permissions at the store\n", false, null);
                        }

                        return new Result<StoreManager>("User successfuly added as the store manager\n", true, newManager);
                    }
                    //else failed
                    return new Result<StoreManager>($"Failed to add store manager. The user is already an manager or owner in the store.\n", false, null);
                }
                finally
                {
                    Monitor.Exit(futureManager);
                }
            }
            catch (SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<StoreManager>(SyncEx.Message, false, null);
            }
        }

        public Result<bool> RemoveStoreManager(String removedManagerID, string currentlyOwnerID)
        {
            if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner) && Managers.TryGetValue(removedManagerID, out StoreManager manager))
            {
                if (manager.AppointedBy.Equals(owner))
                {
                    Managers.TryRemove(removedManagerID, out _);
                    owner.StoreManagers.Remove(manager);
                    return new Result<bool>($"User (Id: {removedManagerID}) was successfully removed from store management at {this.Name}.\n", true, true);
                }
                //else failed
                return new Result<bool>($"Failed to remove user (Id: {removedManagerID}) from store management: Unauthorized owner (Email: {currentlyOwnerID}).\n", false, false);
            }
            //else failed
            return new Result<bool>($"Failed to remove user (Id: {removedManagerID}) from store management: Either not a manager or owner not found.\n", false, false);
        }

        public Result<bool> RemoveStoreOwner(String removedOwnerID, string currentlyOwnerID)
        {
            if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner ownerBoss) && Owners.TryGetValue(removedOwnerID, out StoreOwner ownerToRemove))
            {
                if (ownerToRemove.AppointedBy != null && ownerToRemove.AppointedBy.Equals(ownerBoss))
                {
                    Owners.TryRemove(removedOwnerID, out StoreOwner removedOwner);
                    ownerBoss.StoreOwners.Remove(ownerToRemove);
                    RemoveAllStaffAppointedByOwner(removedOwner);
                    return new Result<bool>($"User (Id: {removedOwnerID}) was successfully removed as store owner at {this.Name}.\n", true, true);
                }
                //else failed
                return new Result<bool>($"Failed to remove user (Id: {removedOwnerID}) as store owner: Unauthorized owner (Id: {currentlyOwnerID}).\n", false, false);
            }
            //else failed
            return new Result<bool>($"Failed to remove user (Id: {removedOwnerID}) as store owner: Either currently owner or owner to be romoved not found.\n", false, false);
        }

        private void RemoveAllStaffAppointedByOwner(StoreOwner owner)
        {
            NotificationManager.notifyOwnerSubscriptionRemoved(owner.GetId() , owner);
            if(Owners.Count != 0)
            {
                foreach (var staff_owner in Owners)
                {
                    if(staff_owner.Value.AppointedBy != null && staff_owner.Value.AppointedBy.GetId() == owner.GetId())
                    {
                        Owners.TryRemove(staff_owner.Value.AppointedBy.GetId(), out StoreOwner removedOwner);
                        RemoveAllStaffAppointedByOwner(removedOwner);
                    }
                }
            }

            if(Managers.Count != 0)
            {
                foreach (var staff_manager in Managers)
                {
                    if (staff_manager.Value.AppointedBy.GetId() == owner.GetId())
                    {
                        Managers.TryRemove(staff_manager.Value.GetId(), out _);
                    }
                }
            }
        }

        public Result<bool> SetPermissions(string managerID, string ownerID, LinkedList<int> permissions)   //TODO: OwnerID can be manager....
        {
            if ((CheckIfStoreOwner(ownerID) || CheckStoreManagerAndPermissions(ownerID, Methods.SetPermissions)) && Managers.TryGetValue(managerID, out StoreManager manager))
            {
                if (CheckAppointedBy(manager, ownerID))
                {
                    foreach (int per in permissions)
                    {
                        manager.SetPermission(per, true);    
                    }
                    return new Result<bool>($"Permissions for manager ({manager.User.Email} updated successfully.\n", true, true);
                }
                //else failed
                return new Result<bool>($"Can't set permissions: Manager (ID: {managerID}) was not appointed by given staff member (ID: {ownerID}).\n", false, false);
            }
            //else failed
            return new Result<bool>($"Staff ID not found in store.\n", false, false);
        }

        public Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(string userID)
        {
            Dictionary<IStoreStaff, Permission> storeStaff = new Dictionary<IStoreStaff, Permission>();
            Permission ownerPermission = new Permission(isOwner:true);
            ownerPermission.SetAllMethodesPermitted();

            if(CheckStoreManagerAndPermissions(userID, Methods.GetStoreStaff ) || CheckIfStoreOwner(userID))           
            {
                foreach(var owner in Owners)
                {
                    storeStaff.Add(owner.Value, ownerPermission);
                }

                foreach (var manager in Managers)
                {
                    storeStaff.Add(manager.Value, manager.Value.Permission);
                }

                return new Result<Dictionary<IStoreStaff, Permission>>("Store sfaffs details\n", true, storeStaff);
            }
            return new Result<Dictionary<IStoreStaff, Permission>>("The given store staff does not have permission to see the stores staff members\n", false, null);
        }

        public Result<History> GetStorePurchaseHistory(string userID,bool sysAdmin)
        {
            if(sysAdmin || CheckStoreManagerAndPermissions(userID, Methods.GetStorePurchaseHistory) || CheckIfStoreOwner(userID))
            {
                return new Result<History>("Store purchase history\n", true, History);
            }
            return new Result<History>("No permission to see store purchase history\n", false, null);
        }


        public Result<ConcurrentDictionary<String, String>> GetProductReview(String productID)
        {
            return InventoryManager.GetProductReview(productID);
        }
        
        public Result<bool> RemovePermissions(string managerID, string ownerID, LinkedList<int> permissions)
        {
            if ((CheckIfStoreOwner(ownerID) || CheckStoreManagerAndPermissions(ownerID, Methods.SetPermissions)) && Managers.TryGetValue(managerID, out StoreManager manager))
            {
                if (CheckAppointedBy(manager, ownerID))
                {
                    foreach (int per in permissions)
                    {
                        manager.SetPermission(per, false);
                    }
                    return new Result<bool>($"Permissions for manager ({manager.User.Email} updated successfully.\n", true, true);
                }
                //else failed
                return new Result<bool>($"Can't remove permissions: Manager (ID: {managerID}) was not appointed by given staff member (ID: {ownerID}).\n", false, false);
            }
            //else failed
            return new Result<bool>($"Staff ID not found in store.\n", false, false);
        }

        //Getter
        public Result<Product> GetProduct(String productID)
        {
            return InventoryManager.GetProduct(productID);
        }

        #region Private Functions
        private Boolean CheckIfStoreOwner(String userID)    // If user is Store Owner => has permissions
        {
            return Owners.ContainsKey(userID);
        }

        private Boolean CheckIfStoreManager(String userID)
        {
            return Managers.ContainsKey(userID);
        }

        private Boolean CheckStoreManagerAndPermissions(String userID, Methods method)    // Check if userID is Store Manager + has certain permission
        {
            return Managers.TryGetValue(userID, out StoreManager manager) && manager.Permission.functionsBitMask[(int)method];
        }

        private Boolean CheckPermissions(StoreManager manager, Methods method)
        {
            return manager.Permission.functionsBitMask[(int)method];
        }

        private Boolean CheckAppointedBy(StoreManager manager, String ownerID)
        {
            return manager.AppointedBy.GetId().Equals(ownerID);
        }
        #endregion

        public Result<StoreService> GetDAL()
        {
            LinkedList<String> owners = new LinkedList<String>();
            
            foreach (var so in Owners)
            {
                owners.AddLast(so.Key);
            }

            LinkedList<String> managers = new LinkedList<String>();
            foreach (var sm in Managers)
            {
                managers.AddLast(sm.Key);
            }
            // InventoryManagerDAL inventoryManager = InventoryManager.GetDAL().Data;  //TODO?
            // PolicyManagerDAL policyManager = PolicyManager.GetDAL().Data;   //TODO?
            HistoryService history = History.GetDAL().Data;

            StoreService store = new StoreService(this.Id, this.Name, Founder.User.Id, owners, managers, history, this.Rating, this.NumberOfRates);
            return new Result<StoreService>("Store DAL object", true, store);

        }

        internal Boolean[] GetPermission(string userID)
        {
            return getUserPermissionByID(userID);
        }

        public Boolean[] getUserPermissionByID (string userID)
        {
            Boolean[] per = new Boolean[13];
            foreach(var user in Owners)
            {
                if (user.Key == userID)
                {
                    StoreOwner owner = user.Value;
                    for(int i=0; i< per.Length; i++)
                    {
                        per[i] = true; 
                    }
                    return per; 
                }
            }

            foreach (var user in Managers)
            {
                if (user.Key == userID)
                {
                    StoreManager manager = user.Value;
                    return (Boolean[])manager.Permission.functionsBitMask;                  
                }
            }

            // No Permissions
            for (int i = 0; i < per.Length; i++)
            {
                per[i] = false;
            }
            return per;
        }

        public double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "")
        {
            return PolicyManager.GetTotalBagPrice(products, discountCode);
        }

        public Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user)
        {
            return PolicyManager.AdheresToPolicy(products, user);
        }

        public Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info)
        {
            return PolicyManager.AddDiscountPolicy(info);
        }

        public Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info, string id)
        {
            return PolicyManager.AddDiscountPolicy(info, id);
        }

        public Result<IDiscountCondition> AddDiscountCondition(Dictionary<string, object> info, string id)
        {
            return PolicyManager.AddDiscountCondition(info, id);
        }

        public Result<IDiscountPolicy> RemoveDiscountPolicy(string id)
        {
            return PolicyManager.RemoveDiscountPolicy(id);
        }

        public Result<IDiscountCondition> RemoveDiscountCondition(string id)
        {
            return PolicyManager.RemoveDiscountCondition(id);
        }

        public Result<IPurchasePolicy> RemovePurchasePolicy(string id)
        {
            return PolicyManager.RemovePurchasePolicy(id);
        }

        public Result<bool> EditDiscountPolicy(Dictionary<string, object> info, string id)
        {
            return PolicyManager.EditDiscountPolicy(info, id);
        }

        public Result<bool> EditDiscountCondition(Dictionary<string, object> info, string id)
        {
            return PolicyManager.EditDiscountCondition(info, id);
        }

        public Result<IDictionary<string, object>> GetPoliciesData()
        {
            return PolicyManager.GetDiscountPolicyData();
        }

        public Result<IDictionary<string, object>> GetPurchasePolicyData()
        {
            return PolicyManager.GetPurchasePolicyData();
        }

        public Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info)
        {
            return PolicyManager.AddPurchasePolicy(info);
        }

        public Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info, string id)
        {
            return PolicyManager.AddPurchasePolicy(info, id);
        }

        public Result<bool> EditPurchasePolicy(Dictionary<string, object> info, string id)
        {
            return PolicyManager.EditPurchasePolicy(info, id);
        }

        public DTO_Store getDTO()
        {
            LinkedList<String> owners_dto = new LinkedList<string>();
            foreach(var owner in Owners)
            {
                owners_dto.AddLast(owner.Key);
            }
            LinkedList<String> managers_dto = new LinkedList<string>();
            foreach (var manager in Managers)
            {
                managers_dto.AddLast(manager.Key);
            }
            LinkedList<String> inventoryManagerProducts_dto = new LinkedList<string>();
            ConcurrentDictionary<String, Product> Products = InventoryManager.Products;
            foreach(var p in Products)
            {
                inventoryManagerProducts_dto.AddLast(p.Key);
            }

            return new DTO_Store(Id, Name, Founder.User.Id, owners_dto, managers_dto,
                inventoryManagerProducts_dto, History.getDTO(), Rating, NumberOfRates, isClosed,
                PolicyManager.MainDiscount.getDTO(), PolicyManager.MainPolicy.getDTO());

        } 
    }
}