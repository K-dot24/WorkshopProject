using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System.Collections.Concurrent;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class StoresFacadeTests
    {
        public StoresFacade Facade { get; }
        public Store TestStore { get; }
        public RegisteredUser Founder { get; }
        public ConcurrentDictionary<String, String> EmailToID { get; set; }

        public StoresFacadeTests()
        {
            Facade = new StoresFacade();
            EmailToID = new ConcurrentDictionary<string, string>();

            //Initial data
            Founder = new RegisteredUser("papi@hotmale.com", "qwerty1");
            EmailToID.TryAdd("papi@hotmale.com", Founder.Id);
            TestStore = new Store("The Testore", Founder);
            Facade.Stores.TryAdd(TestStore.Id, TestStore);
        }

        [Theory()]
        [InlineData("papi@hotmale.com", "Milk 3%", 9.90, 100, "Dairy", true)]   // Success: Store Owner
        [InlineData("tomer@gmail.com", "Milk 3%", 9.90, 100, "Dairy", true)]    // Success: Manager with permissions
        [InlineData("raz@gmail.com", "Milk 3%", 9.90, 100, "Dairy", false)]     // Fail: Manager without permissions
        [InlineData("shaked@gmail.com", "Milk 3%", 9.90, 100, "Dairy", false)]  // Fail: Not staff
        public void AddProductToStoreTest(String userID, String productName, Double price, int initialQuantity, String category, Boolean expectedResult)
        {
            // Manager with permissions
            RegisteredUser user = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            manager.Permission.SetPermission(Methods.AddNewProduct, true);
            TestStore.Managers.TryAdd(manager.User.Email, manager);

            // Manager without permissions
            RegisteredUser user2 = new RegisteredUser("raz@gmail.com", "Because789");
            StoreManager manager2 = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager2.User.Email, manager2);

            Result<Product> productResult = Facade.AddProductToStore(userID, TestStore.Id, productName, price, initialQuantity, category);
            Assert.Equal(expectedResult, productResult.ExecStatus);
            if (productResult != null)
                Assert.Equal(expectedResult, TestStore.InventoryManager.Products.ContainsKey(productResult.Data.Id));
        }

        [Theory()]
        [InlineData("papi@hotmale.com", true)]  // Success: Store Owner
        [InlineData("tomer@gmail.com", true)]   // Success: Manager with permissions
        [InlineData("raz@gmail.com", false)]    // Fail: Manager without permissions
        [InlineData("zoe@gmail.com", false)]    // Fail: Not staff
        public void RemoveProductFromStoreTest(String userID, Boolean expectedResult)
        {
            // Manager with permissions
            RegisteredUser user = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            manager.Permission.SetPermission(Methods.AddNewProduct, true);
            manager.Permission.SetPermission(Methods.RemoveProduct, true);
            TestStore.Managers.TryAdd(manager.User.Email, manager);

            // Manager without permissions
            RegisteredUser user2 = new RegisteredUser("raz@gmail.com", "Because789");
            StoreManager manager2 = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            manager2.Permission.SetPermission(Methods.AddNewProduct, true);
            TestStore.Managers.TryAdd(manager2.User.Email, manager2);

            // Add product
            Product product = TestStore.AddNewProduct(Founder.Email, "Shampoo", 18.50, 10, "Hygiene").Data;

            // Try to remove
            Assert.Equal(expectedResult, Facade.RemoveProductFromStore(userID, TestStore.Id, product.Id).ExecStatus);
            if (expectedResult)
                Assert.False(TestStore.InventoryManager.Products.ContainsKey(product.Id));
            // Wrong product ID
            Assert.False(Facade.RemoveProductFromStore(userID, TestStore.Id, "stam_id").ExecStatus);
            // Wrong store ID
            Assert.False(Facade.RemoveProductFromStore(userID, "stam_id", product.Id).ExecStatus);
        }

        [Theory()]
        [InlineData("papi@hotmale.com", true)]  // Success: Store Owner
        [InlineData("tomer@gmail.com", true)]   // Success: Manager with permissions
        [InlineData("raz@gmail.com", false)]    // Fail: Manager without permissions
        [InlineData("zoe@gmail.com", false)]    // Fail: Not staff
        public void EditProductDetailsTest(string userID, Boolean expectedResult)
        {
            // Manager with permissions
            RegisteredUser user = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            manager.Permission.SetPermission(Methods.AddNewProduct, true);
            manager.Permission.SetPermission(Methods.EditProduct, true);
            TestStore.Managers.TryAdd(manager.User.Email, manager);

            // Manager without permissions
            RegisteredUser user2 = new RegisteredUser("raz@gmail.com", "Because789");
            StoreManager manager2 = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            manager2.Permission.SetPermission(Methods.AddNewProduct, true);
            TestStore.Managers.TryAdd(manager2.User.Email, manager2);

            // Add product
            Product product = TestStore.AddNewProduct(Founder.Email, "Shampoo", 18.50, 10, "Hygiene").Data;

            IDictionary<String, Object> searchAttributes = new Dictionary<String, Object>()
                                                            {{ "Name", "Soap" }, { "Price", 10 } };

            Assert.Equal(expectedResult, Facade.EditProductDetails(userID, TestStore.Id, product.Id, searchAttributes).ExecStatus);

            if (expectedResult)
            {
                TestStore.InventoryManager.Products.TryGetValue(product.Id, out Product newName);
                Assert.Equal("Soap", newName.Name);
                Assert.Equal(10, newName.Price);
            }
        }

        [Theory()]
        [InlineData("tomer@gmail.com", "papi@hotmale.com", true)]   // Success
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]     // Fail: appointer is not an owner
        [InlineData("papi@hotmale.com", "papi@hotmale.com", false)] // Fail: new owner already an owner
        public void AddStoreOwnerTest(string futureOwnerEmail, string currentlyOwnerEmail, Boolean expectedResult)
        {
            /*RegisteredUser founder = new RegisteredUser("aviD@hotmale.com", "qwerty1");
            Store testStore = new Store("The Testore", founder);
            Facade.Stores.TryAdd(testStore.Id, testStore);*/

            RegisteredUser futureOwner = new RegisteredUser(futureOwnerEmail, "password");
            Assert.Equal(expectedResult, Facade.AddStoreOwner(futureOwner, currentlyOwnerEmail, TestStore.Id).ExecStatus);
            Assert.Equal(expectedResult, TestStore.Owners.ContainsKey(futureOwner.Id));
            // Wrong store ID
            Assert.False(Facade.AddStoreOwner(futureOwner, currentlyOwnerEmail, "stam_id").ExecStatus);
        }

        [Theory()]
        [InlineData("tomer@gmail.com", "papi@hotmale.com", true)]   // Success
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]     // Fail: appointer is not an owner
        [InlineData("papi@hotmale.com", "papi@hotmale.com", false)] // Fail: new manager already an owner
        public void AddStoreManagerTest(string futureManagerEmail, string currentlyOwnerEmail, Boolean expectedResult)
        {
            RegisteredUser futureManager = new RegisteredUser(futureManagerEmail, "badpassword");
            Assert.Equal(expectedResult, Facade.AddStoreManager(futureManager, currentlyOwnerEmail, TestStore.Id).ExecStatus);
            Assert.Equal(expectedResult, TestStore.Managers.ContainsKey(futureManager.Id));
        }

        [Theory()]
        [InlineData("tomer@gmail.com", "papi@hotmale.com", true)]   // Success
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]     // Fail: Trying to remove by not the appointer
        [InlineData("raz@gmail.com", "papi@hotmale.com", false)]    // Fail: not a manager
        public void RemoveStoreManagerTest(string removedManagerEmail, string currentlyOwnerEmail, Boolean expectedResult)
        {
            // Prepare new Store Manager, appointed by founder
            RegisteredUser user2 = new RegisteredUser("tomer@gmail.com", "SassyMoodyNasty");
            RegisteredUser user3 = new RegisteredUser("raz@gmail.com", "SassyMoodyNasty");
            EmailToID.TryAdd(user2.Email, user2.Id);
            EmailToID.TryAdd(user3.Email, user3.Id);
            StoreManager manager = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.User.Email, manager);

            EmailToID.TryGetValue(removedManagerEmail, out string removedManagerId);
            EmailToID.TryGetValue(currentlyOwnerEmail, out string currentlyOwnerId);
            Assert.Equal(expectedResult, Facade.RemoveStoreManager(removedManagerId, currentlyOwnerId, TestStore.Id).ExecStatus);
            if (expectedResult)
                Assert.False(TestStore.Managers.ContainsKey(removedManagerId));

        }

        [Theory()]
        [InlineData("papi@hotmale.com", "My Second Store", true)]   // Success: Owner
        [InlineData("tomer@gmail.com", "TOMER HAIR DESIGN", true)]  // Success: Manager
        [InlineData("raz@gmail.com", "Story", true)]                // Success: RegisteredUser
        public void OpenNewStoreTest(string userID, string storeName, Boolean expectedResult)
        {
            RegisteredUser user2 = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            StoreManager manager = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.User.Email, manager);

            RegisteredUser user = new RegisteredUser(userID, "ManInTheMiddle");
            Assert.Equal(expectedResult, Facade.OpenNewStore(user, storeName).ExecStatus);
            if (expectedResult)
                Assert.Equal(expectedResult, Facade.Stores.ContainsKey(storeName));
        }

        [Theory()]
        [InlineData("papi@hotmale.com", "The Testor", true)]  // Success
        [InlineData("papi@hotmale.com", "My Second Store", false)]   // Fail: Store does not exsist
        [InlineData("tomer@gmail.com", "The Testor", false)]  // Fail: Manager without permissions
        public void GetStoreStaffTest1(string ownerMail, string storeName, Boolean expectedResult)
        {
            RegisteredUser user2 = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            StoreManager manager = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.User.Email, manager);

            Dictionary<string, string> storeIds = new Dictionary<string, string>()
                                                               { { "My Second Store", "null" }, { TestStore.Name, TestStore.Id } };
            EmailToID.TryAdd(user2.Email, user2.Id);

            EmailToID.TryGetValue(ownerMail, out string ownerId);
            storeIds.TryGetValue(storeName, out string storeId);
            Result<Dictionary<IStoreStaff, Permission>> res = Facade.GetStoreStaff(ownerId, storeId);

            Assert.Equal(expectedResult, res.ExecStatus);
            if (expectedResult)
            {
                Assert.True(res.Data.ContainsKey(manager));
                Assert.True(res.Data.ContainsKey(TestStore.Founder));
                Assert.True(res.Data.Count == 2);
                Facade.Stores.TryGetValue(storeId, out Store store);
                store.Managers.TryGetValue(manager.User.Id, out StoreManager actualManager);
                Assert.Equal(actualManager, manager);
                store.Owners.TryGetValue(TestStore.Founder.User.Id, out StoreOwner actualOwner);
                Assert.Equal(actualOwner, TestStore.Founder);
                Assert.True(store.Managers.Count == 1);
                Assert.True(store.Owners.Count == 1);
            }
        }

        [Theory()]
        [InlineData("tomer@gmail.com", "papi@hotmale.com", true, new bool[13] { true, true, true, false, false, false, false, true, false, false, false, false, false })]   // Success
        [InlineData("tomer@gmail.com", "raz@gmail.com", false, new bool[13] { false, false, false, false, false, false, false, true, false, false, false, false, false })]     // Fail: manager without permissions
        [InlineData("raz@gmail.com", "zoe@gmail.com", false, new bool[13] { false, false, false, false, false, false, false, true, false, false, false, false, false })]    // Fail: not staff
        public void SetPermissionsTest(String managerEmail, String ownerEmail, Boolean expectedResult, bool[] pers)
        {
            // Manager
            RegisteredUser user = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            EmailToID.TryAdd("tomer@gmail.com", user.Id);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.User.Id, manager);

            // Manager2
            RegisteredUser user2 = new RegisteredUser("raz@gmail.com", "Because789");
            EmailToID.TryAdd("raz@gmail.com", user2.Id);
            StoreManager manager2 = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager2.User.Id, manager2);

            // RegisteredUser
            RegisteredUser user3 = new RegisteredUser("zoe@gmail.com", "Because789");
            EmailToID.TryAdd("zoe@gmail.com", user3.Id);

            // Permissions
            LinkedList<int> permissions = new LinkedList<int>();
            permissions.AddLast(0);
            permissions.AddLast(1);
            permissions.AddLast(2);

            EmailToID.TryGetValue(ownerEmail, out String ownerID);
            EmailToID.TryGetValue(managerEmail, out String managerID);
            Assert.Equal(expectedResult, Facade.SetPermissions(TestStore.Id, managerID, ownerID, permissions).ExecStatus);

            TestStore.Managers.TryGetValue(managerID, out StoreManager sm);
            bool[] current_permissions = sm.Permission.functionsBitMask;
            Assert.Equal(pers, current_permissions);
        }

        [Theory()]
        [InlineData("tomer@gmail.com", "papi@hotmale.com", true, new bool[13] { false, false, false, false, false, false, false, true, false, false, false, false, false })]   // Success
        [InlineData("raz@gmail.com", "tomer@gmail.com", false, new bool[13] { true, true, true, false, false, false, true, true, false, false, false, false, false })]     // Fail: manager without SetPermissions
        [InlineData("tomer@gmail.com", "raz@gmail.com", false, new bool[13] { true, true, true, false, false, false, false, true, false, false, false, false, false })]    // Fail: manager tries to remove permissions of a manager not appointed by him
        [InlineData("zoe@gmail.com", "papi@hotmail.com", false, new bool[13] { false, false, false, false, false, false, false, false, false, false, false, false, false })]    // Fail: trying to remove permissions of someone who isn't a manager
        public void RemovePermissionsTest(String managerEmail, String ownerEmail, Boolean expectedResult, bool[] pers)
        {
            // Manager with permissions 0,1,2,7
            RegisteredUser user = new RegisteredUser("tomer@gmail.com", "Why6AfraidOf7?");
            EmailToID.TryAdd("tomer@gmail.com", user.Id);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            manager.Permission.SetPermission(Methods.AddNewProduct, true);
            manager.Permission.SetPermission(Methods.RemoveProduct, true);
            manager.Permission.SetPermission(Methods.EditProduct, true);
            TestStore.Managers.TryAdd(manager.User.Id, manager);

            // Manager2 with permissions 0,1,2,6,7
            RegisteredUser user2 = new RegisteredUser("raz@gmail.com", "Because789");
            EmailToID.TryAdd("raz@gmail.com", user2.Id);
            StoreManager manager2 = new StoreManager(user2, TestStore, new Permission(), TestStore.Founder);
            manager2.Permission.SetPermission(Methods.AddNewProduct, true);
            manager2.Permission.SetPermission(Methods.RemoveProduct, true);
            manager2.Permission.SetPermission(Methods.EditProduct, true);
            manager2.Permission.SetPermission(Methods.SetPermissions, true);
            TestStore.Managers.TryAdd(manager2.User.Id, manager2);

            // RegisteredUser
            RegisteredUser user3 = new RegisteredUser("zoe@gmail.com", "Because789");
            EmailToID.TryAdd("zoe@gmail.com", user3.Id);

            // Permissions
            LinkedList<int> permissions = new LinkedList<int>();
            permissions.AddLast(0);
            permissions.AddLast(1);
            permissions.AddLast(2);

            EmailToID.TryGetValue(ownerEmail, out String ownerID);
            EmailToID.TryGetValue(managerEmail, out String managerID);
            Assert.Equal(expectedResult, Facade.RemovePermissions(TestStore.Id, managerID, ownerID, permissions).ExecStatus);

            if (TestStore.Managers.TryGetValue(managerID, out StoreManager sm))
            {
                bool[] current_permissions = sm.Permission.functionsBitMask;
                Assert.Equal(pers, current_permissions);
            }
        }

        [Fact()]
        [Trait("category","Unit")]
        public void SearchProductTestByAttributes(string attribute ,string name, double rating, bool expectedResult)
        {
            IDictionary<String, Object> attributes = new Dictionary<string, object>() { {"name", name }, {"rating", rating } };
            TestStore.AddRating(4.0);
            Result<List<Store>> result = Facade.SearchStore(attributes);
        }
    }
}