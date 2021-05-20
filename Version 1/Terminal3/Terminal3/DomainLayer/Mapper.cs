using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using MongoDB.Driver;
using MongoDB.Bson;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DataAccessLayer.DAOs;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.ServiceLayer;
using Terminal3.DomainLayer.StoresAndManagement;



namespace Terminal3.DataAccessLayer
{
    public sealed class Mapper
    {
        //Fields
        private static Mapper Instance = null;
        public MongoClient dbClient;
        public IMongoDatabase database;

        // DAOs
        public DAO<DTO_GuestUser> DAO_GuestUser;
        public DAO<DTO_RegisteredUser> DAO_RegisteredUser;
        public DAO<DTO_Product> DAO_Product;
        public DAO<DTO_StoreManager> DAO_StoreManager;
        public DAO<DTO_StoreOwner> DAO_StoreOwner;
        public DAO<DTO_Store> DAO_Store;

        // IdentityMaps  <Id , object>
        public ConcurrentDictionary<String, RegisteredUser> RegisteredUsers;
        public ConcurrentDictionary<String, GuestUser> GuestUsers;
        public ConcurrentDictionary<String, Product> Products;
        public ConcurrentDictionary<String, LinkedList<StoreManager>> StoreManagers;
        public ConcurrentDictionary<String, LinkedList<StoreOwner>> StoreOwners;
        public ConcurrentDictionary<String, Store> Stores;

        //Constructor
        private Mapper()
        {
            dbClient = new MongoClient("mongodb+srv://admin:terminal3@cluster0.cbdpv.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            database = dbClient.GetDatabase("Terminal3-development");

            //DAOs
            DAO_GuestUser = new DAO<DTO_GuestUser>(database, "Users");
            DAO_RegisteredUser = new DAO<DTO_RegisteredUser>(database, "Users");
            DAO_Product = new DAO<DTO_Product>(database, "Stores");            
            DAO_StoreManager = new DAO<DTO_StoreManager>(database, "Users");
            DAO_StoreOwner = new DAO<DTO_StoreOwner>(database, "Users");
            DAO_Store = new DAO<DTO_Store>(database, "Stores");

            // IdentityMaps  <Id , object>
            RegisteredUsers = new ConcurrentDictionary<String, RegisteredUser>();
            GuestUsers = new ConcurrentDictionary<String, GuestUser>();
            Products = new ConcurrentDictionary<String, Product>();
            StoreManagers = new ConcurrentDictionary<String, LinkedList<StoreManager>>();
            StoreOwners = new ConcurrentDictionary<String, LinkedList<StoreOwner>>();
            Stores = new ConcurrentDictionary<String, Store>();
    }

        public static Mapper getInstance()
        {
            if (Instance == null)
            {
                Instance = new Mapper();
            }
            return Instance;
        }


        #region Private Methods

        #region Convert to DTO
        public DTO_ShoppingCart Get_DTO_ShoppingCart(User user)
        {
            ConcurrentDictionary<String, DTO_ShoppingBag> dto_sb = new ConcurrentDictionary<String, DTO_ShoppingBag>();
            foreach (var sb in user.ShoppingCart.ShoppingBags)
            {
                ConcurrentDictionary<String, int> dto_products = new ConcurrentDictionary<string, int>();
                foreach (var p in sb.Value.Products)
                {
                    dto_products.TryAdd(p.Key.Id, p.Value); //<Product id , quantity>
                }
                dto_sb.TryAdd(sb.Key, new DTO_ShoppingBag(sb.Value.Id, sb.Value.User.Id, sb.Value.Store.Id, dto_products, sb.Value.TotalBagPrice));
            }
            DTO_ShoppingCart dto_sc = new DTO_ShoppingCart(user.ShoppingCart.Id, dto_sb, user.ShoppingCart.TotalCartPrice);

            return dto_sc;
        }

        private DTO_ShoppingBag Get_DTO_ShoppingBag(ShoppingBag sb)
        {
            ConcurrentDictionary<String, int> dto_products = new ConcurrentDictionary<string, int>();
            foreach (var p in sb.Products)
            {
                dto_products.TryAdd(p.Key.Id, p.Value); //<Product id , quantity>
            }
            return new DTO_ShoppingBag(sb.Id, sb.User.Id, sb.Store.Id, dto_products, sb.TotalBagPrice);
        }

        public DTO_History Get_DTO_History(History history)
        {
            LinkedList<DTO_HistoryShoppingBag> dto_sb = new LinkedList<DTO_HistoryShoppingBag>();
            foreach (ShoppingBagService bag in history.ShoppingBags)
            {
                LinkedList<DTO_HistoryProduct> dto_products = new LinkedList<DTO_HistoryProduct>();
                foreach (Tuple<ProductService, int> tuple in bag.Products)
                {
                    dto_products.AddLast(new DTO_HistoryProduct(tuple.Item1.Id, tuple.Item1.Name, tuple.Item1.Price, tuple.Item2, tuple.Item1.Category));
                }

                DTO_HistoryShoppingBag dto_bag = new DTO_HistoryShoppingBag(bag.Id, bag.UserId, bag.StoreId, dto_products, bag.TotalBagPrice);
                dto_sb.AddLast(dto_bag);
            }
            return new DTO_History(dto_sb);
        }
        
        private LinkedList<DTO_Notification> Get_DTO_Notifications(LinkedList<Notification> pendingNotifications)
        {
            LinkedList<DTO_Notification> dto_pendingNotifications = new LinkedList<DTO_Notification>();
            foreach (Notification n in pendingNotifications)
            {
                dto_pendingNotifications.AddLast(new DTO_Notification((int)(n.EventName), n.Message, n.Date.ToString(), n.isOpened, n.isStoreStaff, n.ClientId));
            }

            return dto_pendingNotifications;
        }

        public LinkedList<String> Get_DTO_ManagerList(LinkedList<StoreManager> list)
        {            
            LinkedList<String> managers = new LinkedList<String>();
            
            foreach (StoreManager manager in list)
            {
                managers.AddLast(manager.User.Id); 
            }

            return managers;
        }

        public LinkedList<String> Get_DTO_OwnerList(LinkedList<StoreOwner> list)
        {
            LinkedList<String> owners = new LinkedList<String>();

            foreach (StoreOwner owner in list)
            { 
                owners.AddLast(owner.User.Id);
            }            

            return owners;
        }
        #endregion Convert to DTO

        #region Convert to Object
        private ShoppingCart ToObject(DTO_ShoppingCart dto , User user)
        {
            ConcurrentDictionary<String, ShoppingBag> sb = new ConcurrentDictionary<String, ShoppingBag>();
            foreach (var bag in dto.ShoppingBags)
            {
                ConcurrentDictionary<Product, int> products = new ConcurrentDictionary<Product, int>();
                foreach (var p in bag.Value.Products)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", p.Key);
                    products.TryAdd(LoadProduct(filter), p.Value);
                }
                //sb.TryAdd(bag.Key, new ShoppingBag(bag.Key, user, LoadStore(), products, bag.Value.TotalBagPrice)); - TODO
                sb.TryAdd(bag.Key, new ShoppingBag(bag.Key, user, products, bag.Value.TotalBagPrice));
            }
            ShoppingCart sc = new ShoppingCart(dto._id, sb, dto.TotalCartPrice);
            return sc;
        }

        private History ToObject(DTO_History dto)
        {
            LinkedList<ShoppingBagService> shoppingBags = new LinkedList<ShoppingBagService>();
            foreach (DTO_HistoryShoppingBag bag in dto.ShoppingBags)
            {
                LinkedList<Tuple<ProductService, int>> products = new LinkedList<Tuple<ProductService, int>>();
                foreach (DTO_HistoryProduct p in bag.Products)
                {
                    products.AddLast(new Tuple<ProductService, int>(new ProductService(p._id, p.Name, p.Price, p.ProductQuantity, p.Category), p.ProductQuantity));
                }
                shoppingBags.AddLast(new ShoppingBagService(bag._id, bag.UserId, bag.StoreId, products, bag.TotalBagPrice));
            }
            return new History(shoppingBags);
        }
        
        private LinkedList<Notification> ToObject(LinkedList<DTO_Notification> dto_list)
        {
            LinkedList<Notification> pendingNotifications = new LinkedList<Notification>();
            foreach (DTO_Notification n in dto_list)
            {
                pendingNotifications.AddLast(new Notification((Event)n.EventName, n.ClientId, n.Message, n.isStoreStaff , n.isOpened , n.Date));
            }

            return pendingNotifications;
        }
        #endregion Convert to Object

        #endregion

        #region User
        #region GuestUser
        public void Create(GuestUser gu)
        {            
            DAO_GuestUser.Create(new DTO_GuestUser(gu.Id , Get_DTO_ShoppingCart(gu), gu.Active));
            GuestUsers.TryAdd(gu.Id, gu);
        }

        public GuestUser LoadGuestUser(FilterDefinition<BsonDocument> filter)
        {
            GuestUser gu;
            DTO_GuestUser dto = DAO_GuestUser.Load(filter);
            if (GuestUsers.TryGetValue(dto._id, out gu))
            {
                return gu;
            }

            gu = new GuestUser(dto._id, dto.Active);
            
            gu.ShoppingCart = ToObject(dto.ShoppingCart, gu);
            GuestUsers.TryAdd(gu.Id, gu);
            return gu;
        }

        public void UpdateGuestUser(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_GuestUser.Update(filter, update);
        }

        public void DeleteGuestUser(FilterDefinition<BsonDocument> filter)
        {
            DTO_GuestUser deletedGuestUser = DAO_GuestUser.Delete(filter);
            GuestUsers.TryRemove(deletedGuestUser._id, out GuestUser gu);
        }


        #endregion GuestUser

        #region RegisteredUser
        public void Create(RegisteredUser ru)
        {            
            DAO_RegisteredUser.Create(new DTO_RegisteredUser(ru.Id, Get_DTO_ShoppingCart(ru), ru.Email, ru.Password, ru.LoggedIn, Get_DTO_History(ru.History), Get_DTO_Notifications(ru.PendingNotification)));
            RegisteredUsers.TryAdd(ru.Id , ru);
        }        

        public RegisteredUser LoadRegisteredUser(FilterDefinition<BsonDocument> filter)
        {
            RegisteredUser ru;
            DTO_RegisteredUser dto = DAO_RegisteredUser.Load(filter);
            if(dto != null && RegisteredUsers.TryGetValue(dto._id , out ru))
            {
                return ru;
            }
            
            ru = new RegisteredUser(dto._id, dto.Email, dto.Password, dto.LoggedIn, ToObject(dto.History), ToObject(dto.PendingNotification));
            ru.ShoppingCart = ToObject(dto.ShoppingCart, ru);
            RegisteredUsers.TryAdd(ru.Id, ru);
            return ru;
        }

        public void UpdateRegisteredUser(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_RegisteredUser.Update(filter, update);
        }

        public void DeleteRegisteredUser(FilterDefinition<BsonDocument> filter)
        {
            DTO_RegisteredUser deletedRegisteredUser = DAO_RegisteredUser.Delete(filter);
            RegisteredUsers.TryRemove(deletedRegisteredUser._id, out RegisteredUser ru);
        }

        #endregion RegisteredUser

        #region Store Manager
        public void Create(StoreManager sm)
        {
            DAO_StoreManager.Create(new DTO_StoreManager(sm.GetId(), sm.Permission.functionsBitMask, sm.AppointedBy.GetId(), sm.Store.Id));
            LinkedList<StoreManager> list;
            if (StoreManagers.ContainsKey(sm.GetId()))
            {
                StoreManagers.TryGetValue(sm.GetId(), out list);
                list.AddLast(sm);
            }
            else
            {
                list = new LinkedList<StoreManager>();
                list.AddLast(sm);
                StoreManagers.TryAdd(sm.GetId(), list);
            }
        }

        public StoreManager LoadStoreManager(FilterDefinition<BsonDocument> filter)
        {
            StoreManager sm;
            LinkedList<StoreManager> list;
            DTO_StoreManager dto = DAO_StoreManager.Load(filter);       // TODO - need to make sure when loading store manager the filter includes user id and store id

            bool listExists = StoreManagers.TryGetValue(dto.UserId, out list);
            if (listExists)
            {
                foreach(StoreManager manager in list)
                {
                    if(manager.Store.Id == dto.StoreId)
                    {
                        return manager;
                    }
                }
                
            }
            else
            {
                list = new LinkedList<StoreManager>();
            }

            var user_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.UserId);
            var store_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.StoreId);
            var owner_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.AppointedBy);

            sm = new StoreManager(LoadRegisteredUser(user_filter), LoadStore(store_filter), new Permission(dto.Permission), LoadStoreOwner(owner_filter));

            list.AddLast(sm);
            if (!listExists)
            {
                StoreManagers.TryAdd(sm.GetId(), list);
            }
            // else added to list pointer

            return sm;
        }

        public void UpdateStoreManager(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_StoreManager.Update(filter, update);
        }

        public void DeleteStoreManager(FilterDefinition<BsonDocument> filter)
        {
            DTO_StoreManager deletedStoreManager = DAO_StoreManager.Delete(filter); // TODO - need to make sure when loading store manager the filter includes user id and store id
            StoreManager sm = null;
            LinkedList<StoreManager> list;            

            if (StoreManagers.TryGetValue(deletedStoreManager.UserId, out list))
            {
                foreach (StoreManager manager in list)
                {
                    if (manager.Store.Id == deletedStoreManager.StoreId)
                    {
                        sm = manager;
                    }
                }
            }

            list.Remove(sm);
            if (list.Count == 0)
            {
                StoreManagers.TryRemove(sm.GetId() , out _);
            }
            // else removed from list pointer
        }
        #endregion Store Manager

        #region Store Owner
        public void Create(StoreOwner so)
        {

            String appointedById = "0";
            if(so.AppointedBy != null)
            {
                appointedById = so.AppointedBy.GetId();
            }

            DAO_StoreOwner.Create(new DTO_StoreOwner(so.GetId(), so.Store.Id, appointedById, Get_DTO_ManagerList(so.StoreManagers), Get_DTO_OwnerList(so.StoreOwners)));
            LinkedList<StoreOwner> list;
            if (StoreOwners.ContainsKey(so.GetId()))
            {
                StoreOwners.TryGetValue(so.GetId(), out list);
                list.AddLast(so);
            }
            else
            {
                list = new LinkedList<StoreOwner>();
                list.AddLast(so);
                StoreOwners.TryAdd(so.GetId(), list);
            }
        }

        public StoreOwner LoadStoreOwner(FilterDefinition<BsonDocument> filter)
        {
            StoreOwner so;
            LinkedList<StoreOwner> list;
            DTO_StoreOwner dto = DAO_StoreOwner.Load(filter);       // TODO - need to make sure when loading store manager the filter includes user id and store id

            bool listExists = StoreOwners.TryGetValue(dto.UserId, out list);
            if (listExists)
            {
                foreach (StoreOwner owner in list) { if (owner.Store.Id == dto.StoreId) { return owner; } }

            }
            else { list = new LinkedList<StoreOwner>(); }

            var user_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.UserId);
            var store_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.StoreId);
            var owner_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.AppointedBy);

            LinkedList<StoreOwner> owners = new LinkedList<StoreOwner>();
            LinkedList<StoreManager> managers = new LinkedList<StoreManager>();

            foreach (String owner in dto.StoreOwners)
            {
                var filter1 = Builders<BsonDocument>.Filter.Eq("_id", owner);
                owners.AddLast(LoadStoreOwner(filter1));
            }
            foreach (String manager in dto.StoreManagers)
            {
                var filter2 = Builders<BsonDocument>.Filter.Eq("_id", manager);
                managers.AddLast(LoadStoreManager(filter2)); 
            }

            so = new StoreOwner(LoadRegisteredUser(user_filter), LoadStore(store_filter), LoadStoreOwner(owner_filter), managers, owners);

            list.AddLast(so);
            if (!listExists)  { StoreOwners.TryAdd(so.GetId(), list); }
            // else added to list pointer

            return so;
        }

        public void UpdateStoreOwner(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_StoreOwner.Update(filter, update);
        }

        public void DeleteStoreOwner(FilterDefinition<BsonDocument> filter)
        {
            DTO_StoreOwner deletedStoreOwner = DAO_StoreOwner.Delete(filter); // TODO - need to make sure when loading store manager the filter includes user id and store id
            StoreOwner so = null;
            LinkedList<StoreOwner> list;

            if (StoreOwners.TryGetValue(deletedStoreOwner.UserId, out list))
            {
                foreach (StoreOwner owner in list)
                {
                    if (owner.Store.Id == deletedStoreOwner.StoreId) { so = owner;  }
                }
            }

            list.Remove(so);
            if (list.Count == 0)
            {
                StoreOwners.TryRemove(so.GetId(), out _);
            }
            // else removed from list pointer
        }
        #endregion Store Owner

        #endregion User

        #region Shop till you drop



        #endregion Shop till you drop

        #region Stores

        #region Product
        public void Create(Product p)
        {
            DAO_Product.Create(new DTO_Product(p.Id, p.Name, p.Price, p.Quantity, p.Category, p.Rating, p.NumberOfRates, p.Keywords, p.Review));
            Products.TryAdd(p.Id, p);
        }

        public Product LoadProduct(FilterDefinition<BsonDocument> filter)
        {
            Product p;
            DTO_Product dto = DAO_Product.Load(filter);
            if (Products.TryGetValue(dto._id, out p))
            {
                return p;
            }

            p = new Product(p.Id, p.Name, p.Price, p.Quantity, p.Category, p.Keywords, p.Review);
            // TODO - get product notification Manager and inject it
            Products.TryAdd(p.Id, p);
            return p;
        }

        public void UpdateProduct(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_Product.Update(filter, update);
        }

        public void DeleteProduct(FilterDefinition<BsonDocument> filter)
        {
            DTO_Product deletedProduct = DAO_Product.Delete(filter);
            Products.TryRemove(deletedProduct._id, out Product p);
        }

        #endregion Product

        #region Store
        public void Create(Store s)
        {
            LinkedList<String> owners = new LinkedList<String>();
            LinkedList<String> managers = new LinkedList<String>();
            LinkedList<String> inventory = new LinkedList<String>();

            foreach (var owner in s.Owners) { owners.AddLast(owner.Key); }
            foreach (var manager in s.Managers) { managers.AddLast(manager.Key); }
            foreach(var product in s.InventoryManager.Products) { inventory.AddLast(product.Key); }

            DAO_Store.Create(new DTO_Store(s.Id, s.Name, s.Founder.GetId(), owners, managers, inventory, Get_DTO_History(s.History), s.Rating, s.NumberOfRates));
            Stores.TryAdd(s.Id, s);
        }

        public Store LoadStore(FilterDefinition<BsonDocument> filter)
        {
            Store s;
            DTO_Store dto = DAO_Store.Load(filter);
            if (Stores.TryGetValue(dto._id, out s)) { return s; }

            var founder_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.Founder);

            ConcurrentDictionary<String, StoreOwner> owners = new ConcurrentDictionary<String, StoreOwner>();
            ConcurrentDictionary<String, StoreManager> managers = new ConcurrentDictionary<String, StoreManager>();
            ConcurrentDictionary<String, Product> products = new ConcurrentDictionary<String, Product>();

            foreach (String owner in dto.Owners)
            {
                var filter1 = Builders<BsonDocument>.Filter.Eq("_id", owner);
                owners.TryAdd(owner , LoadStoreOwner(filter1));
            }
            foreach (String manager in dto.Managers)
            {
                var filter2 = Builders<BsonDocument>.Filter.Eq("_id", manager);
                managers.TryAdd(manager , LoadStoreManager(filter2));
            }

            NotificationManager notificationManager = new NotificationManager();

            foreach(String product in dto.InventoryManager)
            {
                var filter3 = Builders<BsonDocument>.Filter.Eq("_id", product);
                Product p = LoadProduct(filter3);
                p.NotificationManager = notificationManager;
                products.TryAdd(product, p );
            }

            InventoryManager inventory = new InventoryManager(products);

            s = new Store(dto._id, dto.Name, LoadStoreOwner(founder_filter), owners, managers, inventory, ToObject(dto.History), dto.Rating, dto.NumberOfRates , notificationManager);                            

            Stores.TryAdd(s.Id, s);

            return s;
        }

        public void UpdateStore(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_Store.Update(filter, update);
        }

        public void DeleteStore(FilterDefinition<BsonDocument> filter)
        {
            DTO_Store deletedStore = DAO_Store.Delete(filter);
            Stores.TryRemove(deletedStore._id, out Store s);
        }

        #endregion Store

        #endregion Stores


    }
}

