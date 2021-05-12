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
        Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID);
        Result<Boolean> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID);
        Result<Boolean> RemoveStoreOwner(String removedOwnerID, String currentlyOwnerID);        
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(String userID);
        #endregion

        #region Policies Management
        double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "");
        Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user);
        Result<Boolean> AddDiscountPolicy(IDiscountPolicy discount);
        Result<Boolean> AddDiscountPolicy(IDiscountPolicy discount, String id);
        Result<Boolean> AddDiscountCondition(IDiscountCondition condition, String id);
        Result<Boolean> RemoveDiscountPolicy(String id);
        Result<Boolean> RemoveDiscountCondition(String id);
        Result<IDiscountPolicyData> GetPoliciesData();
        Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy);
        Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy, string id);
        Result<Boolean> RemovePurchasePolicy(string id);
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
        public StoreOwner Founder { get; }
        public ConcurrentDictionary<String, StoreOwner> Owners { get; }
        public ConcurrentDictionary<String, StoreManager> Managers { get; }
        public InventoryManager InventoryManager { get; }
        public PolicyManager PolicyManager { get; }
        public History History { get; }
        public Double Rating { get; private set; }
        public int NumberOfRates { get; private set; }
        public NotificationManager NotificationManager { get; set; }

        //Constructors
        public Store(String name, RegisteredUser founder)
        {
            Id = Service.GenerateId();
            Name = name;
            Founder = new StoreOwner(founder, this, null);
            Owners = new ConcurrentDictionary<String, StoreOwner>();
            Managers = new ConcurrentDictionary<String, StoreManager>();
            InventoryManager = new InventoryManager();
            PolicyManager = new PolicyManager();
            History = new History();

            //Add founder to list of owners
            Owners.TryAdd(founder.Id, Founder);

            //TODO: Complete when policies done properly
            //Add default policies
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
                Monitor.TryEnter(productID);
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
            }

            return new Result<bool>("Store inventory updated successuly\n", true, true);

    }
        #endregion

        public Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, string currentlyOwnerID)
        {
            try
            {
                Monitor.TryEnter(futureOwner);
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
                        }
                        else if (Managers.TryGetValue(currentlyOwnerID, out StoreManager manager) && CheckStoreManagerAndPermissions(currentlyOwnerID, Methods.AddStoreOwner))
                        {
                            newOwner = new StoreOwner(futureOwner, this, manager);
                            Owners.TryAdd(futureOwner.Id, newOwner);
                        }
                        else
                        {
                            return new Result<Boolean>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}) " +
                                $"is not an owner at ${this.Name}.\n", false, false);
                        }


                        if (CheckIfStoreManager(futureOwner.Id)) //remove from managers list if needed
                        {
                            Managers.TryRemove(futureOwner.Id, out _);
                        }

                        return new Result<Boolean>("User successfuly added as the store owner\n", true, true);
                    }
                    //else failed
                    return new Result<Boolean>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}). The user is already an owner.\n", false, false);
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
                return new Result<Boolean>(SyncEx.Message, false, false);
            }
        }

        public Result<Boolean> AddStoreManager(RegisteredUser futureManager, string currentlyOwnerID)
        {

            try
            {
                Monitor.TryEnter(futureManager);
                try
                {
                    // Check new manager not already an owner/manager + appointing owner is not a fraud or the appointing user is a manager with the right permissions
                    if (!CheckIfStoreManager(futureManager.Id) && !CheckIfStoreOwner(futureManager.Id))
                    {
                        StoreManager newManager;
                        if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner))
                        {
                            newManager = new StoreManager(futureManager, this, new Permission(), owner);
                            Managers.TryAdd(futureManager.Id, newManager);
                        }
                        else if (Managers.TryGetValue(currentlyOwnerID, out StoreManager manager) && CheckStoreManagerAndPermissions(currentlyOwnerID, Methods.AddStoreManager))
                        {
                            newManager = new StoreManager(futureManager, this, new Permission(), manager);
                            Managers.TryAdd(futureManager.Id, newManager);
                        }
                        else
                        {
                            return new Result<Boolean>($"Failed to add store manager because appoitend user is not an owner or manager with relevant permissions at the store\n", false, false);
                        }

                        return new Result<Boolean>("User successfuly added as the store manager\n", true, true);
                    }
                    //else failed
                    return new Result<Boolean>($"Failed to add store manager. The user is already an manager or owner in the store.\n", false, false);
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
                return new Result<Boolean>(SyncEx.Message, false, false);
            }
        }

        public Result<bool> RemoveStoreManager(String removedManagerID, string currentlyOwnerID)
        {
            if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner) && Managers.TryGetValue(removedManagerID, out StoreManager manager))
            {
                if (manager.AppointedBy.Equals(owner))
                {
                    Managers.TryRemove(removedManagerID, out _);
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

        public Result<bool> AddDiscountPolicy(IDiscountPolicy discount)
        {
            return PolicyManager.AddDiscountPolicy(discount);
        }

        public Result<bool> AddDiscountPolicy(IDiscountPolicy discount, string id)
        {
            return PolicyManager.AddDiscountPolicy(discount, id);
        }

        public Result<bool> AddDiscountCondition(IDiscountCondition condition, string id)
        {
            return PolicyManager.AddDiscountCondition(condition, id);
        }

        public Result<bool> RemoveDiscountPolicy(string id)
        {
            return PolicyManager.RemoveDiscountPolicy(id);
        }

        public Result<bool> RemoveDiscountCondition(string id)
        {
            return PolicyManager.RemoveDiscountCondition(id);
        }

        public Result<IDiscountPolicyData> GetPoliciesData()
        {
            return PolicyManager.GetData();
        }

        public Result<bool> AddPurchasePolicy(IPurchasePolicy policy)
        {
            return PolicyManager.AddPurchasePolicy(policy);
        }

        public Result<bool> AddPurchasePolicy(IPurchasePolicy policy, string id)
        {
            return PolicyManager.AddPurchasePolicy(policy, id);
        }

        public Result<bool> RemovePurchasePolicy(string id)
        {
            return PolicyManager.RemovePurchasePolicy(id);
        }
    }
}