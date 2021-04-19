using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public interface IStoreOperations
    {
        //TODO: Update functions sigs

        #region Inventory Management
        Result<Product> AddNewProduct(String userID, String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Product> RemoveProduct(String userID, String productID);
        Result<Product> EditProduct(String userID, String productID, IDictionary<String, Object> details);
        #endregion

        #region Staff Management
        Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, String currentlyOwnerID);
        Result<Boolean> AddStoreManager(RegisteredUser futureManager, String currentlyOwnerID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(String ownerID);
        #endregion

        #region Policies Management
        Result<Object> SetPurchasePolicyAtStore(IPurchasePolicy policy);
        Result<Object> GetPurchasePolicyAtStore();
        Result<Object> SetDiscountPolicyAtStore(IDiscountPolicy policy);
        Result<Object> GetDiscountPolicyAtStore();
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
                Rating = (Rating + rate) / NumberOfRates;
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
                return InventoryManager.AddNewProduct(productName, price, initialQuantity, category, keywords);
            }
            else
            {
                return new Result<Product>($"{userID} does not have permissions to add new product to {this.Name}\n", false, null);
            }
        }
        public Result<Product> RemoveProduct(String userID, String productID)
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
        #endregion

        public Result<Boolean> AddStoreOwner(RegisteredUser futureOwner, string currentlyOwnerID)
        {
            if (!CheckIfStoreOwner(futureOwner.Id) && Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner)) // Check new owner not already an owner + appointing owner is not a fraud
            {
                StoreOwner newOwner = new StoreOwner(futureOwner, this, owner);
                Owners.TryAdd(futureOwner.Id, newOwner);

                if (CheckIfStoreManager(futureOwner.Id)) //remove from managers list if needed
                {
                    Managers.TryRemove(futureOwner.Id, out _);
                }
            }
            //else failed
            return new Result<Boolean>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}) " +
                $"is not an owner at ${this.Name}.\n", false, false);
        }

        public Result<Boolean> AddStoreManager(RegisteredUser futureManager, string currentlyOwnerID)
        {
            if (!CheckIfStoreManager(futureManager.Id) && !CheckIfStoreOwner(futureManager.Id)
                    && Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner)) // Check new manager not already an owner/manager + appointing owner is not a fraud
            {
                StoreManager newManager = new StoreManager(futureManager, this, new Permission(), owner);
                Managers.TryAdd(futureManager.Id, newManager);
            }
            //else failed
            return new Result<Boolean>($"Failed to add store owner: Appointing owner (Email: {currentlyOwnerID}) " +
                $"is not an owner at ${this.Name}.\n", false, false);
        }

        public Result<bool> RemoveStoreManager(String removedManagerID, string currentlyOwnerID)
        {
            if (Owners.TryGetValue(currentlyOwnerID, out StoreOwner owner) && Managers.TryGetValue(removedManagerID, out StoreManager manager))
            {
                if (manager.AppointedBy.Equals(owner))
                {
                    Managers.TryRemove(removedManagerID, out _);
                    return new Result<bool>($"User (Email: {removedManagerID}) was successfully removed from store management at {this.Name}.\n", true, true);
                }
                //else failed
                return new Result<bool>($"Failed to remove user (Email: {removedManagerID}) from store management: Unauthorized owner (Email: {currentlyOwnerID}).\n", false, false);
            }
            //else failed
            return new Result<bool>($"Failed to remove user (Email: {removedManagerID}) from store management: Either not a manager or owner not found.\n", false, false);
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

        public Result<Dictionary<IStoreStaff, Permission>> GetStoreStaff(string ownerID)
        {
            Dictionary<IStoreStaff, Permission> storeStaff = new Dictionary<IStoreStaff, Permission>();
            Permission ownerPermission = new Permission();
            ownerPermission.SetAllMethodesPermitted();

            if(CheckStoreManagerAndPermissions(ownerID, Methods.GetStoreStaff ) || CheckIfStoreOwner(ownerID))           
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

        public Result<object> SetPurchasePolicyAtStore(IPurchasePolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<object> GetPurchasePolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<object> SetDiscountPolicyAtStore(IDiscountPolicy policy)
        {
            throw new NotImplementedException();
        }

        public Result<object> GetDiscountPolicyAtStore()
        {
            throw new NotImplementedException();
        }

        public Result<Object> RemoveProduct(Product product)
        {
            throw new NotImplementedException();
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
            return manager.AppointedBy.User.Id.Equals(ownerID);
        }
        #endregion

        public Result<StoreDAL> GetDAL()
        {
            StoreOwnerDAL founder = (StoreOwnerDAL)Founder.GetDAL().Data;
            ConcurrentDictionary<String, StoreOwnerDAL> owners = new ConcurrentDictionary<String, StoreOwnerDAL>();
            foreach (var so in Owners)
            {
                owners.TryAdd(so.Key, (StoreOwnerDAL)so.Value.GetDAL().Data);
            }
            ConcurrentDictionary<String, StoreManagerDAL> managers = new ConcurrentDictionary<String, StoreManagerDAL>();
            foreach (var sm in Managers)
            {
                managers.TryAdd(sm.Key, (StoreManagerDAL)sm.Value.GetDAL().Data);
            }
            // InventoryManagerDAL inventoryManager = InventoryManager.GetDAL().Data;  //TODO?
            // PolicyManagerDAL policyManager = PolicyManager.GetDAL().Data;   //TODO?
            HistoryDAL history = History.GetDAL().Data;

            StoreDAL store = new StoreDAL(this.Id, this.Name, founder, owners, managers, history, this.Rating, this.NumberOfRates);
            return new Result<StoreDAL>("Store DAL object", true, store);

        }
    }
}