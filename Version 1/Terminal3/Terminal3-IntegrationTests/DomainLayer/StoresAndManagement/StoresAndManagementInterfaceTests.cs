using Xunit;
using Terminal3.DomainLayer.StoresAndManagement;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System.Linq;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Tests
{
    public class StoresAndManagementInterfaceTests
    {
        public StoresAndManagementInterface Facade { get; }
        public StoresFacade StoresFacade { get; }
        public UsersAndPermissionsFacade UserFacade { get; }
        public Store TestStore { get; }
        public RegisteredUser Founder { get; }
        public RegisteredUser RegisteredUser { get; }
        public GuestUser GuestUser { get; }

        string adminEmail = "Admin@terminal3.com";
        string adminPassword = "Admin123";

        public StoresAndManagementInterfaceTests()
        {
            // Facade to check
            Facade = new StoresAndManagementInterface(adminEmail, adminPassword);

            // Facades to integrate
            StoresFacade = Facade.StoresFacade;
            UserFacade = Facade.UsersAndPermissionsFacade;

            // Initialize store for tests
            Founder = new RegisteredUser("papi@hotmale.com", "qwerty1");
            TestStore = new Store("The Testore", Founder);
            StoresFacade.Stores.TryAdd(TestStore.Id, TestStore);

            // Initialize users for testing
            RegisteredUser = new RegisteredUser("zoe@gmail.com", "StrongPassword");
            GuestUser = new GuestUser();

            // Add all users to UserFacade
            UserFacade.RegisteredUsers.TryAdd(Founder.Id, Founder);
            UserFacade.RegisteredUsers.TryAdd(RegisteredUser.Id, RegisteredUser);
            UserFacade.GuestUsers.TryAdd(GuestUser.Id, GuestUser);

        }


        [Fact()]
        public void OpenNewStoreTest()
        {
            // Registered User can open multiple stores and with the same name
            Assert.True(Facade.OpenNewStore("NiceToMeat", Founder.Id,"-1").ExecStatus);
            Assert.True(Facade.OpenNewStore("NiceToMeat", Founder.Id, "-1").ExecStatus);

            // Check both stores were added and with the same name
            Assert.Equal(3, StoresFacade.Stores.Count);

            int count = 0;
            foreach(var store in StoresFacade.Stores)
            {
                if(store.Value.Name == "NiceToMeat")
                {
                    count++;
                }
            }

            Assert.Equal(2, count);

            // Registered User can open store
            Assert.True(Facade.OpenNewStore("Store2", RegisteredUser.Id, "-1").ExecStatus);

            // Guest User cannot open a store
            Assert.False(Facade.OpenNewStore("NoCannot", GuestUser.Id, "-1").ExecStatus);

            // Check number of stores are correct
            Assert.Equal(4, StoresFacade.Stores.Count);
        }


        [Fact()]
        public void AddStoreOwnerTest()
        {            
            // Success - Founder can add registered user to be store owner
            Assert.True(Facade.AddStoreOwner(RegisteredUser.Id , Founder.Id , TestStore.Id).ExecStatus);

            // Fail - registered user is already a owner in the store
            Assert.False(Facade.AddStoreOwner(RegisteredUser.Id, Founder.Id, TestStore.Id).ExecStatus);

            // Check that founder and registered user are store owners
            Assert.True(TestStore.Owners.ContainsKey(Founder.Id));
            Assert.True(TestStore.Owners.ContainsKey(RegisteredUser.Id));

            // Fail - guest owner can not be a store owner
            Assert.False(Facade.AddStoreOwner(GuestUser.Id, Founder.Id, TestStore.Id).ExecStatus);
            Assert.False(TestStore.Owners.ContainsKey(GuestUser.Id));

        }

        [Fact()]
        public void AddStoreOwnerTest2()
        {
            StoreManager manager = new StoreManager(RegisteredUser, TestStore, new Permission(), TestStore.Founder);
            Assert.True(Facade.AddStoreOwner(RegisteredUser.Id, Founder.Id, TestStore.Id).ExecStatus);

            Assert.False(TestStore.Managers.ContainsKey(manager.GetId()));
        }

        [Fact()]
        public void AddStoreManagerTest()
        {
            // Success - Founder can add registered user to be store manager
            Assert.True(Facade.AddStoreManager(RegisteredUser.Id, Founder.Id, TestStore.Id).ExecStatus);

            // Fail - registered user is already a manager in the store
            Assert.False(Facade.AddStoreManager(RegisteredUser.Id, Founder.Id, TestStore.Id).ExecStatus);

            // Check that registered user is a store manager
            Assert.True(TestStore.Managers.ContainsKey(RegisteredUser.Id));

            // Fail - guest owner can not be a store manager
            Assert.False(Facade.AddStoreManager(GuestUser.Id, Founder.Id, TestStore.Id).ExecStatus);
            Assert.False(TestStore.Managers.ContainsKey(GuestUser.Id));
        }

        [Fact()]
        public void AddStoreManagerTest2()
        {
            StoreManager manager = new StoreManager(RegisteredUser, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);

            //Fail - manager does not have permission to apoint user as manager
            Assert.False(Facade.AddStoreManager(user.Id, manager.GetId(), TestStore.Id).ExecStatus);
            Assert.False(TestStore.Managers.ContainsKey(user.Id));

            //Success - manager can add user as store manager
            manager.SetPermission(Methods.AddStoreManager, true);
            Assert.True(Facade.AddStoreManager(user.Id, manager.GetId(), TestStore.Id).ExecStatus);
            Assert.True(TestStore.Managers.ContainsKey(user.Id));

        }


        [Fact()]
        public void RemoveStoreManagerTest()
        {            
            StoreManager manager = new StoreManager(RegisteredUser, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            // Success - founder appointed manager as store manager therefore can remove him
            Assert.True(Facade.RemoveStoreManager(manager.GetId(), Founder.Id, TestStore.Id).ExecStatus);
            Assert.False(TestStore.Managers.ContainsKey(manager.GetId()));
        }

        [Fact()]
        public void RemoveStoreManagerTest2()
        {
            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);

            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            // Fail - registered user did not appointe manager as store manager therefore cannot remove him
            Assert.False(Facade.RemoveStoreManager(manager.GetId(), RegisteredUser.Id, TestStore.Id).ExecStatus);
            Assert.True(TestStore.Managers.ContainsKey(manager.GetId()));
        }


        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData( 5, true, true)]      // Success
        [InlineData( 10, true, false)]      // Fail 2: Higher quantity than quantity in store
        [InlineData( 0, false, false)]      // Fail: Illegal quantity
        [InlineData( -1, false, false)]  // Fail: Illegal quantity
        public void AddProductToCartTest(int productQuantity , Boolean expectedResult, Boolean expectedResult2)
        {
            //string userID, string productID, int productQuantity, string storeID)

            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            Product product2 = new Product("Apple", 4.9, 5, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);
            TestStore.InventoryManager.Products.TryAdd(product2.Id, product2);

            Assert.Equal(expectedResult, Facade.AddProductToCart(RegisteredUser.Id , product.Id, productQuantity, TestStore.Id).ExecStatus);
            Result<ShoppingBag> getSB = RegisteredUser.ShoppingCart.GetShoppingBag(TestStore.Id);
            if (getSB.ExecStatus)//if the bag was created
            {
                Assert.Equal(expectedResult, getSB.Data.Products.ContainsKey(product));
            }
            else//if the bag wasn't created
            {
                Assert.False(expectedResult);
            }
            Assert.Equal(expectedResult2, Facade.AddProductToCart(RegisteredUser.Id, product2.Id, productQuantity, TestStore.Id).ExecStatus);
            Result<ShoppingBag> getSB2 = RegisteredUser.ShoppingCart.GetShoppingBag(TestStore.Id);
            if (getSB2.ExecStatus)//if the bag was created
            {
                Assert.Equal(expectedResult, getSB2.Data.Products.ContainsKey(product));
            }
            else//if the bag wasn't created
            {
                Assert.False(expectedResult2);
            }
        }

        [Fact()]
        public void AddProductToCartTest2()
        {
            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);

            Assert.False(Facade.AddProductToCart(RegisteredUser.Id, product.Id, 5,"No Such Store").ExecStatus);
            Assert.False(RegisteredUser.ShoppingCart.GetShoppingBag(TestStore.Id).ExecStatus);
            Assert.Empty(RegisteredUser.ShoppingCart.ShoppingBags);        
        }


        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData(5, true, 5)]      // Success
        [InlineData(10, false, 5)]      // Fail : Higher quantity than quantity in store
        [InlineData(0, true, 0)]      // Success
        [InlineData(-1, true, 0)]  // Success
        public void UpdateShoppingCartTest(int quantity, Boolean expectedResult, int expectedQuantity)
        {
            // Add products to store
            Product product = new Product("Banana", 5.7, 5, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);

            // Add product to user shopping bag
            Facade.AddProductToCart(RegisteredUser.Id, product.Id, 5, TestStore.Id);

            Assert.Equal(expectedResult, Facade.UpdateShoppingCart(RegisteredUser.Id, TestStore.Id, product.Id, quantity).ExecStatus);

            // Check bag
            RegisteredUser.ShoppingCart.ShoppingBags.TryGetValue(TestStore.Id, out ShoppingBag bag);
            bool res;
            int updatedQuantity;
            if (bag != null)
                res = bag.Products.TryGetValue(product, out updatedQuantity);
            else
            {
                res = false;
                updatedQuantity = 0;
            }


            if (res)
            {
                Assert.Equal(expectedQuantity, updatedQuantity);
            }
            else if (expectedQuantity == 0 && bag!=null)
            {
                Assert.False(bag.Products.ContainsKey(product));
            }
            else
            {
                Assert.Empty(RegisteredUser.ShoppingCart.ShoppingBags);
            }

        }


        [Fact()]
        public void AddProductReviewTest()
        {            
            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);

            // Add product to user shopping bag
            Facade.AddProductToCart(RegisteredUser.Id, product.Id, 5, TestStore.Id);

            // Add shopping bag to history
            RegisteredUser.ShoppingCart.ShoppingBags.TryGetValue(TestStore.Id, out ShoppingBag bag);
            RegisteredUser.History.ShoppingBags.AddLast(bag.GetDAL().Data);

            // Add review to product 
            Assert.True(Facade.AddProductReview(RegisteredUser.Id, TestStore.Id, product.Id, "The banana was awsome").ExecStatus);

            Assert.True(product.Review.TryGetValue(RegisteredUser.Id, out String msg));
            Assert.Equal("The banana was awsome", msg);
        }


        [Fact()]
        public void AddProductReviewTest2()
        { 
            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);

            // Add review to product 
            Assert.False(Facade.AddProductReview(RegisteredUser.Id, TestStore.Id, product.Id, "The banana was awsome").ExecStatus);
            Assert.False(product.Review.TryGetValue(RegisteredUser.Id, out String msg));
        }


        [Fact()]
        public void PurchaseTest()
        {
            // Add products to store
            Product product = new Product("Banana", 5.7, 5, "Fruits");
            Product product2 = new Product("Apple", 4.9, 5, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);
            TestStore.InventoryManager.Products.TryAdd(product2.Id, product2);

            // Add product to user shopping bag
            Facade.AddProductToCart(RegisteredUser.Id, product.Id, 2, TestStore.Id);
            Facade.AddProductToCart(RegisteredUser.Id, product2.Id, 1, TestStore.Id);

            IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            // The bag is not purchased yet
            Assert.Empty(RegisteredUser.History.ShoppingBags);

            //Result<ShoppingCartService> res = Facade.PurchaseAsync(RegisteredUser.Id, paymentDetails, deliveryDetails);
            //Assert.True(res.ExecStatus);
            System.Threading.Tasks.Task<Result<ShoppingCartService>> async_res = Facade.PurchaseAsync(RegisteredUser.Id, paymentDetails, deliveryDetails);
            Result<ShoppingCartService> res = async_res.Result;
            Assert.True(res.ExecStatus);

            ShoppingCartService cart = res.Data;
            Assert.Equal(16.3, cart.TotalCartPrice);

            Assert.Single(RegisteredUser.History.ShoppingBags);

            // Check User Historys bags
            History history = RegisteredUser.History;
            LinkedList<ShoppingBagService> bagsDAL = history.ShoppingBags;
            ShoppingBagService bagDAL = bagsDAL.First.Value;

            Assert.Equal(16.3, bagDAL.TotalBagPrice);

            //Check Store History
            History storeHistory = TestStore.History;
            LinkedList<ShoppingBagService> storeBagsDAL = storeHistory.ShoppingBags;
            ShoppingBagService storeBagDAL = storeBagsDAL.First.Value;

            Assert.Equal(16.3, storeBagDAL.TotalBagPrice);
        }

        [Fact()]
        public void PurchaseTest2()
        {
            // Open another store
            Store store2 = new Store("Testore2", Founder);
            StoresFacade.Stores.TryAdd(store2.Id, store2);

            // Add products to store
            Product product = new Product("Banana", 5.7, 5, "Fruits");
            Product product2 = new Product("Apple", 4.9, 5, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);
            store2.InventoryManager.Products.TryAdd(product2.Id, product2);

            // Add product to user shopping bag
            Facade.AddProductToCart(RegisteredUser.Id, product.Id, 2, TestStore.Id);
            Facade.AddProductToCart(RegisteredUser.Id, product2.Id, 1, store2.Id);

            IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            // The bag is not purchased yet
            Assert.Empty(RegisteredUser.History.ShoppingBags);

            //Result<ShoppingCartService> res = Facade.PurchaseAsync(RegisteredUser.Id, paymentDetails, deliveryDetails);
            System.Threading.Tasks.Task<Result<ShoppingCartService>> async_res = Facade.PurchaseAsync(RegisteredUser.Id, paymentDetails, deliveryDetails);
            Result<ShoppingCartService> res = async_res.Result;
            Assert.True(res.ExecStatus);

            ShoppingCartService cart = res.Data;
            Assert.Equal(16.3, cart.TotalCartPrice);

            Assert.Equal(2, RegisteredUser.History.ShoppingBags.Count);

            //Check Stores History
            History storeHistory = TestStore.History;
            LinkedList<ShoppingBagService> storeBagsDAL = storeHistory.ShoppingBags;
            ShoppingBagService storeBagDAL = storeBagsDAL.First.Value;

            Assert.Equal(11.4, storeBagDAL.TotalBagPrice);

            History store2History = store2.History;
            LinkedList<ShoppingBagService> store2BagsDAL = store2History.ShoppingBags;
            ShoppingBagService store2BagDAL = store2BagsDAL.First.Value;

            Assert.Equal(4.9, store2BagDAL.TotalBagPrice);
        }


        [Theory()]
        [InlineData(true)]      
        [InlineData(false)]    
        public void NotificationPurchaseTest(Boolean loggedin)
        {
            NotificationManager notificationManager = new NotificationManager(TestStore);
            TestStore.NotificationManager = notificationManager;

            // Add products to store
            Product product = new Product("Banana", 5.7, 5, "Fruits");
            Product product2 = new Product("Apple", 4.9, 5, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);
            TestStore.InventoryManager.Products.TryAdd(product2.Id, product2);

            // Update products Notification Manager manually
            product.NotificationManager = notificationManager;
            product2.NotificationManager = notificationManager;

            // Add product to user shopping bag
            Facade.AddProductToCart(RegisteredUser.Id, product.Id, 2, TestStore.Id);
            Facade.AddProductToCart(RegisteredUser.Id, product2.Id, 1, TestStore.Id);

            IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            Founder.LoggedIn = loggedin;

            Facade.PurchaseAsync(RegisteredUser.Id, paymentDetails, deliveryDetails);

            if (loggedin)
            {
                Assert.Empty(Founder.PendingNotification);
            }
            else
            {
                Assert.Equal(2, Founder.PendingNotification.Count); // one for each product and not as quantity

                foreach(Notification n in Founder.PendingNotification)
                {
                    Assert.False(n.isOpened);
                    Assert.True(n.isStoreStaff);
                    Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), n.Date.ToString("MM/dd/yyyy HH:mm"));
                    Assert.Equal(Founder.Id, n.ClientId);
                }
            }
        }



        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void NotificationCloseStoreTest(Boolean loggedin)
        {
            // Create Notification Manager for store
            NotificationManager notificationManager = new NotificationManager(TestStore);
            TestStore.NotificationManager = notificationManager;

            // Add 1 owner and 1 manager to store
            StoreOwner owner = new StoreOwner(RegisteredUser, TestStore, TestStore.Founder);
            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            TestStore.Owners.TryAdd(owner.GetId(), owner);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            Founder.LoggedIn = loggedin;
            owner.User.LoggedIn = loggedin;
            manager.User.LoggedIn = loggedin;

            Facade.CloseStore(TestStore.Id, Founder.Id);

            if (loggedin)
            {
                Assert.Empty(Founder.PendingNotification);
                Assert.Empty(owner.User.PendingNotification);
                Assert.Empty(manager.User.PendingNotification);
            }
            else
            {
                Assert.Single(Founder.PendingNotification); 
                Assert.Single(owner.User.PendingNotification); 
                Assert.Single(manager.User.PendingNotification); 

                foreach (Notification n in Founder.PendingNotification)
                {
                    Assert.False(n.isOpened);
                    Assert.True(n.isStoreStaff);
                    Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), n.Date.ToString("MM/dd/yyyy HH:mm"));
                    Assert.Equal(Founder.Id, n.ClientId);

                }

                foreach (Notification n in owner.User.PendingNotification)
                {
                    Assert.False(n.isOpened);
                    Assert.True(n.isStoreStaff);
                    Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), n.Date.ToString("MM/dd/yyyy HH:mm"));
                    Assert.Equal(owner.GetId(), n.ClientId);

                }

                foreach (Notification n in manager.User.PendingNotification)
                {
                    Assert.False(n.isOpened);
                    Assert.True(n.isStoreStaff);
                    Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), n.Date.ToString("MM/dd/yyyy HH:mm"));
                    Assert.Equal(manager.GetId(), n.ClientId);

                }
            }
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void NotificationOpenStoreTest(Boolean loggedin)
        {
            // Create Notification Manager for store
            NotificationManager notificationManager = new NotificationManager(TestStore);
            TestStore.NotificationManager = notificationManager;

            // Add 1 owner and 1 manager to store
            StoreOwner owner = new StoreOwner(RegisteredUser, TestStore, TestStore.Founder);
            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            TestStore.Owners.TryAdd(owner.GetId(), owner);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            Founder.LoggedIn = loggedin;
            owner.User.LoggedIn = loggedin;
            manager.User.LoggedIn = loggedin;

            Facade.CloseStore(TestStore.Id, Founder.Id);
            Facade.ReOpenStore(TestStore.Id, Founder.Id);

            if (loggedin)
            {
                Assert.Empty(Founder.PendingNotification);
                Assert.Empty(owner.User.PendingNotification);
                Assert.Empty(manager.User.PendingNotification);
            }
            else
            {
                Assert.Equal(2 , Founder.PendingNotification.Count);
                Assert.Equal(2 , owner.User.PendingNotification.Count);
                Assert.Equal(2 , manager.User.PendingNotification.Count);

                String firstmsg = $"Event : Store Closed\nStore Id : {TestStore.Id}\n";
                Assert.Equal(firstmsg, Founder.PendingNotification.First.Value.Message);
                Assert.Equal(firstmsg, owner.User.PendingNotification.First.Value.Message);
                Assert.Equal(firstmsg, manager.User.PendingNotification.First.Value.Message);

                String lastmsg = $"Event : Store Opened\nStore Id : {TestStore.Id}\n";
                Assert.Equal(lastmsg, Founder.PendingNotification.Last.Value.Message);
                Assert.Equal(lastmsg, owner.User.PendingNotification.Last.Value.Message);
                Assert.Equal(lastmsg, manager.User.PendingNotification.Last.Value.Message);

                Assert.Equal(Founder.Id, Founder.PendingNotification.First.Value.ClientId);
                Assert.Equal(owner.GetId(), owner.User.PendingNotification.First.Value.ClientId);
                Assert.Equal(manager.GetId(), manager.User.PendingNotification.First.Value.ClientId);

                Assert.Equal(Founder.Id, Founder.PendingNotification.Last.Value.ClientId);
                Assert.Equal(owner.GetId(), owner.User.PendingNotification.Last.Value.ClientId);
                Assert.Equal(manager.GetId(), manager.User.PendingNotification.Last.Value.ClientId);

            }
        }


        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void NotificationOwnerSubscriptionRemovedTest(Boolean loggedin)
        {
            // Create Notification Manager for store
            NotificationManager notificationManager = new NotificationManager(TestStore);
            TestStore.NotificationManager = notificationManager;

            // Add 1 owner and 1 manager to store
            StoreOwner owner = new StoreOwner(RegisteredUser, TestStore, TestStore.Founder);
            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            TestStore.Owners.TryAdd(owner.GetId(), owner);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            Founder.LoggedIn = loggedin;
            owner.User.LoggedIn = loggedin;
            manager.User.LoggedIn = loggedin;

            Facade.RemoveStoreOwner(owner.GetId(), Founder.Id, TestStore.Id);            

            if (loggedin)
            {
                Assert.Empty(Founder.PendingNotification);
                Assert.Empty(owner.User.PendingNotification);
                Assert.Empty(manager.User.PendingNotification);
            }
            else
            {
                Assert.Single(Founder.PendingNotification);
                Assert.Single(owner.User.PendingNotification);
                Assert.Single(manager.User.PendingNotification);

                String msg = $"Event : Owner Subscription Removed\nStore Id : {TestStore.Id}\nOwner Id : {owner.GetId()}";
                Assert.Equal(msg, Founder.PendingNotification.First.Value.Message);
                Assert.Equal(msg, owner.User.PendingNotification.First.Value.Message);
                Assert.Equal(msg, manager.User.PendingNotification.First.Value.Message);

                Assert.Equal(Founder.Id, Founder.PendingNotification.First.Value.ClientId);
                Assert.Equal(owner.GetId(), owner.User.PendingNotification.First.Value.ClientId);
                Assert.Equal(manager.GetId(), manager.User.PendingNotification.First.Value.ClientId);
            }
        }

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void NotificationOwnerSubscriptionRemovedTest2(Boolean loggedin)
        {
            // Create Notification Manager for store
            NotificationManager notificationManager = new NotificationManager(TestStore);
            TestStore.NotificationManager = notificationManager;

            // Add 1 owner and 1 manager to store
            StoreOwner owner = new StoreOwner(RegisteredUser, TestStore, TestStore.Founder);
            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), owner);  // Notice : this time the manager was appointed by the owner (and not by the founder)
            TestStore.Owners.TryAdd(owner.GetId(), owner);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            Founder.LoggedIn = loggedin;
            owner.User.LoggedIn = loggedin;
            manager.User.LoggedIn = loggedin;

            Facade.RemoveStoreOwner(owner.GetId(), Founder.Id, TestStore.Id);   // also the manager is removed by default - but there is no notification for that

            if (loggedin)
            {
                Assert.Empty(Founder.PendingNotification);
                Assert.Empty(owner.User.PendingNotification);
                Assert.Empty(manager.User.PendingNotification);
            }
            else
            {
                Assert.Single(Founder.PendingNotification);
                Assert.Single(owner.User.PendingNotification);
                Assert.Single(manager.User.PendingNotification);

                String msg = $"Event : Owner Subscription Removed\nStore Id : {TestStore.Id}\nOwner Id : {owner.GetId()}";
                Assert.Equal(msg, Founder.PendingNotification.First.Value.Message);
                Assert.Equal(msg, owner.User.PendingNotification.First.Value.Message);
                Assert.Equal(msg, manager.User.PendingNotification.First.Value.Message);

                Assert.Equal(Founder.Id, Founder.PendingNotification.First.Value.ClientId);
                Assert.Equal(owner.GetId(), owner.User.PendingNotification.First.Value.ClientId);
                Assert.Equal(manager.GetId(), manager.User.PendingNotification.First.Value.ClientId);
            }
        }


        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void NotificationProductReviewTest(Boolean loggedin)
        {
            // Create Notification Manager for store
            NotificationManager notificationManager = new NotificationManager(TestStore);
            TestStore.NotificationManager = notificationManager;

            // Add 1 owner and 1 manager to store
            StoreOwner owner = new StoreOwner(RegisteredUser, TestStore, TestStore.Founder);
            RegisteredUser user = new RegisteredUser("shmar@gmail.com", "Password");
            UserFacade.RegisteredUsers.TryAdd(user.Id, user);
            StoreManager manager = new StoreManager(user, TestStore, new Permission(), TestStore.Founder);
            TestStore.Owners.TryAdd(owner.GetId(), owner);
            TestStore.Managers.TryAdd(manager.GetId(), manager);

            Founder.LoggedIn = loggedin;
            owner.User.LoggedIn = loggedin;
            manager.User.LoggedIn = loggedin;

            // Add products to store
            Product product = new Product("Banana", 5.7, 100, "Fruits");
            TestStore.InventoryManager.Products.TryAdd(product.Id, product);

            // Update products Notification Manager manually
            product.NotificationManager = notificationManager;

            // Add product to user shopping bag
            Facade.AddProductToCart(RegisteredUser.Id, product.Id, 5, TestStore.Id);

            // Add shopping bag to history
            RegisteredUser.ShoppingCart.ShoppingBags.TryGetValue(TestStore.Id, out ShoppingBag bag);
            RegisteredUser.History.ShoppingBags.AddLast(bag.GetDAL().Data);

            // Add review to product 
            String review = "The banana was awsome";
            Facade.AddProductReview(RegisteredUser.Id, TestStore.Id, product.Id, review);

            if (loggedin)
            {
                Assert.Empty(Founder.PendingNotification);
                Assert.Empty(owner.User.PendingNotification);
                Assert.Empty(manager.User.PendingNotification);
            }
            else
            {
                Assert.Single(Founder.PendingNotification);
                Assert.Single(owner.User.PendingNotification);
                Assert.Empty(manager.User.PendingNotification);     // managers dont get this notification

                String msg = $"Event : A product review was added\nStore Id : {TestStore.Id}\nProduct Id : {product.Id}\nReview : {review}\n";
                Assert.Equal(msg, Founder.PendingNotification.First.Value.Message);
                Assert.Equal(msg, owner.User.PendingNotification.First.Value.Message);

                Assert.Equal(Founder.Id, Founder.PendingNotification.First.Value.ClientId);
                Assert.Equal(owner.GetId(), owner.User.PendingNotification.First.Value.ClientId);
            }
        }

    }
}