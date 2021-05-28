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
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using System.Reflection;

namespace Terminal3.DataAccessLayer
{
    public sealed class Mapper
    {
        //Fields
        private static Mapper Instance = null;
        public MongoClient dbClient;
        public IMongoDatabase database;

        // DAOs
        // public DAO<DTO_GuestUser> DAO_GuestUser;
        public DAO<DTO_RegisteredUser> DAO_RegisteredUser;
        public DAO<DTO_Product> DAO_Product;
        public DAO<DTO_StoreManager> DAO_StoreManager;
        public DAO<DTO_StoreOwner> DAO_StoreOwner;
        public DAO<DTO_Store> DAO_Store;
        public DAO<DTO_Auction> DAO_Auction;
        public DAO<DTO_Lottery> DAO_Lottery;
        public DAO<DTO_MaxProductPolicy> DAO_MaxProductPolicy;
        public DAO<DTO_MinAgePolicy> DAO_MinAgePolicy;
        public DAO<DTO_MinProductPolicy> DAO_MinProductPolicy;
        public DAO<DTO_Offer> DAO_Offer;
        public DAO<DTO_RestrictedHoursPolicy> DAO_RestrictedHoursPolicy;
        public DAO<DTO_AndPolicy> DAO_AndPolicy;
        public DAO<DTO_OrPolicy> DAO_OrPolicy;
        public DAO<DTO_BuyNow> DAO_BuyNow;
        public DAO<DTO_ConditionalPolicy> DAO_ConditionalPolicy;

        // IdentityMaps  <Id , object>
        public ConcurrentDictionary<String, RegisteredUser> RegisteredUsers;
        public ConcurrentDictionary<String, GuestUser> GuestUsers;
        public ConcurrentDictionary<String, Product> Products;
        public ConcurrentDictionary<String, LinkedList<StoreManager>> StoreManagers;
        public ConcurrentDictionary<String, LinkedList<StoreOwner>> StoreOwners;
        public ConcurrentDictionary<String, Store> Stores;
        public ConcurrentDictionary<String, Auction> Policy_Auctions;
        public ConcurrentDictionary<String, Lottery> Policy_Lotterys;
        public ConcurrentDictionary<String, MaxProductPolicy> Policy_MaxProductPolicys;
        public ConcurrentDictionary<String, MinAgePolicy> Policy_MinAgePolicys;
        public ConcurrentDictionary<String, MinProductPolicy> Policy_MinProductPolicys;
        public ConcurrentDictionary<String, Offer> Policy_Offers;
        public ConcurrentDictionary<String, RestrictedHoursPolicy> Policy_RestrictedHoursPolicys;
        public ConcurrentDictionary<String, AndPolicy> Policy_AndPolicys;
        public ConcurrentDictionary<String, OrPolicy> Policy_OrPolicys;
        public ConcurrentDictionary<String, BuyNow> Policy_BuyNows;
        public ConcurrentDictionary<String, ConditionalPolicy> Policy_ConditionalPolicys;


        //Constructor
        private Mapper(String connection_string)
        {
            dbClient = new MongoClient(connection_string);
            database = dbClient.GetDatabase("Terminal3-development");

            //DAOs
            //DAO_GuestUser = new DAO<DTO_GuestUser>(database, "Users");
            DAO_RegisteredUser = new DAO<DTO_RegisteredUser>(database, "Users");
            DAO_Product = new DAO<DTO_Product>(database, "Products");            
            DAO_StoreManager = new DAO<DTO_StoreManager>(database, "Users");
            DAO_StoreOwner = new DAO<DTO_StoreOwner>(database, "Users");
            DAO_Store = new DAO<DTO_Store>(database, "Stores");
            DAO_Auction = new DAO<DTO_Auction>(database, "Policies");
            DAO_Lottery = new DAO<DTO_Lottery>(database, "Policies");
            DAO_MaxProductPolicy = new DAO<DTO_MaxProductPolicy>(database, "Policies");
            DAO_MinAgePolicy = new DAO<DTO_MinAgePolicy>(database, "Policies");
            DAO_MinProductPolicy = new DAO<DTO_MinProductPolicy>(database, "Policies");
            DAO_Offer = new DAO<DTO_Offer>(database, "Policies");
            DAO_RestrictedHoursPolicy = new DAO<DTO_RestrictedHoursPolicy>(database, "Policies");
            DAO_AndPolicy = new DAO<DTO_AndPolicy>(database, "Policies");
            DAO_OrPolicy = new DAO<DTO_OrPolicy>(database, "Policies");
            DAO_BuyNow = new DAO<DTO_BuyNow>(database, "Policies");
            DAO_ConditionalPolicy = new DAO<DTO_ConditionalPolicy>(database, "Policies");

            // IdentityMaps  <Id , object>
            RegisteredUsers = new ConcurrentDictionary<String, RegisteredUser>();
            GuestUsers = new ConcurrentDictionary<String, GuestUser>();
            Products = new ConcurrentDictionary<String, Product>();
            StoreManagers = new ConcurrentDictionary<String, LinkedList<StoreManager>>();
            StoreOwners = new ConcurrentDictionary<String, LinkedList<StoreOwner>>();
            Stores = new ConcurrentDictionary<String, Store>();
            Policy_Auctions = new ConcurrentDictionary<String, Auction>();
            Policy_Lotterys = new ConcurrentDictionary<String, Lottery>();
            Policy_MaxProductPolicys = new ConcurrentDictionary<String, MaxProductPolicy>();
            Policy_MinAgePolicys = new ConcurrentDictionary<String, MinAgePolicy>();
            Policy_MinProductPolicys = new ConcurrentDictionary<String, MinProductPolicy>();
            Policy_Offers = new ConcurrentDictionary<String, Offer>();
            Policy_RestrictedHoursPolicys = new ConcurrentDictionary<String, RestrictedHoursPolicy>();
            Policy_AndPolicys = new ConcurrentDictionary<String, AndPolicy>();
            Policy_OrPolicys = new ConcurrentDictionary<String, OrPolicy>();
            Policy_BuyNows = new ConcurrentDictionary<String, BuyNow>();
            Policy_ConditionalPolicys = new ConcurrentDictionary<String, ConditionalPolicy>();

    }

        public static Mapper getInstance()
        {
            return Instance;
        }

        public static Mapper getInstance(String connection_string)
        {
            if (Instance == null)
            {
                Instance = new Mapper(connection_string);
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

        private ConcurrentDictionary<String, String> getPoliciesIDs(List<IPurchasePolicy> list)
        {
            ConcurrentDictionary<String, String> Policies = new ConcurrentDictionary<String, String>();
            foreach (IPurchasePolicy policy in list)
            {
                string[] type = policy.GetType().ToString().Split('.');
                string policy_type = type[type.Length - 1];
                AddToDB(policy_type, policy);
                Policies.TryAdd(policy_type, policy.Id);
            }
            return Policies;
        }
        private IPurchasePolicy LoadIPurchasePolicy(String tag, string policy_id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", policy_id);
            switch (tag)
            {
                case "AndPolicy":
                    return LoadAndPolicy(filter);

                case "ConditionalPolicy":
                    return LoadConditionalPolicy(filter);

                case "MaxProductPolicy":
                    return LoadMaxProductPolicy(filter);


                case "MinAgePolicy":
                    return LoadMinAgePolicy(filter);


                case "MinProductPolicy":
                    return LoadMinProductPolicy(filter);


                case "OrPolicy":
                    return LoadOrPolicy(filter);


                case "RestrictedHoursPolicy":
                    return LoadRestrictedHoursPolicy(filter);
            }

            return null;
        }

        private void DeleteIPurchasePolicy(String type, string policy_id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", policy_id);
            switch (type)
            {
                case "AndPolicy":
                    DeleteAndPolicy(filter);
                    break;

                case "Auction":
                    DeleteAuctionPolicy(filter);
                    break;

                case "BuyNow":
                    DeleteBuyNowPolicy(filter);
                    break;

                case "ConditionalPolicy":
                    DeleteConditionalPolicy(filter);
                    break;

                case "Lottery":
                    DeleteLotteryPolicy(filter);
                    break;

                case "MaxProductPolicy":
                    DeleteMaxProductPolicy(filter);
                    break;

                case "MinAgePolicy":
                    DeleteMinAgePolicy(filter);
                    break;

                case "MinProductPolicy":
                    DeleteMinProductPolicy(filter);
                    break;

                case "Offer":
                    DeleteOfferPolicy(filter);
                    break;

                case "OrPolicy":
                    DeleteOrPolicy(filter);
                    break;

                case "RestrictedHoursPolicy":
                    DeleteRestrictedHoursPolicy(filter);
                    break;
            }
        }
        private void AddToDB(String type , IPurchasePolicy policy)
        {
            switch (type)
            {
                case "AndPolicy":
                    Create((AndPolicy)policy);
                    break;

                case "Auction":
                    Create((Auction)policy);
                    break;

                case "BuyNow":
                    Create((BuyNow)policy);
                    break;

                case "ConditionalPolicy":
                    Create((ConditionalPolicy)policy);
                    break;

                case "Lottery":
                    Create((Lottery)policy);
                    break;

                case "MaxProductPolicy":
                    Create((MaxProductPolicy)policy);
                    break;

                case "MinAgePolicy":
                    Create((MinAgePolicy)policy);
                    break;

                case "MinProductPolicy":
                    Create((MinProductPolicy)policy);
                    break;

                case "Offer":
                    Create((Offer)policy);
                    break;

                case "OrPolicy":
                    Create((OrPolicy)policy);
                    break;

                case "RestrictedHoursPolicy":
                    Create((RestrictedHoursPolicy)policy);
                    break;
            }
        }

        #endregion

        #region User
        /*#region GuestUser
        public void Create(GuestUser gu)
        {            
            //DAO_GuestUser.Create(new DTO_GuestUser(gu.Id , Get_DTO_ShoppingCart(gu), gu.Active));
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


        #endregion GuestUser*/

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
                foreach (StoreManager manager in list)
                {
                    if (manager.Store.Id == dto.StoreId)
                    {
                        return manager;
                    }
                }

            }            
            
            return null;            
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

        private void AddManagerToIdentityMap(StoreManager manager)
        {
            LinkedList<StoreManager> list;
            bool listExists = StoreManagers.TryGetValue(manager.GetId(), out list);
            if (listExists)
            {
                list.AddLast(manager);
            }
            else
            {
                list = new LinkedList<StoreManager>();
                list.AddLast(manager);
                StoreManagers.TryAdd(manager.GetId(), list);
            }
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
            return null;
        }
        
        public StoreOwner getOwnershipTree(Store store , String founder_id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("UserId", founder_id) & Builders<BsonDocument>.Filter.Eq("StoreId", store.Id);
            DTO_StoreOwner founder_dto = DAO_StoreOwner.Load(filter);
            var filter2 = Builders<BsonDocument>.Filter.Eq("_id", founder_dto.UserId);
            StoreOwner founder = new StoreOwner(LoadRegisteredUser(filter2), store ,null);

            if (founder_dto.StoreOwners.Count > 0)
            {
                foreach (String owner_id in founder_dto.StoreOwners)
                {
                    StoreOwner storeowner = getOwnershipTree(store , owner_id);
                    storeowner.AppointedBy = founder;
                    founder.StoreOwners.AddLast(storeowner);
                }
            }
            if (founder_dto.StoreManagers.Count > 0)
            {
                foreach (String manager_id in founder_dto.StoreManagers)
                {
                    var manager_filter = Builders<BsonDocument>.Filter.Eq("UserId", manager_id) & Builders<BsonDocument>.Filter.Eq("StoreId", store.Id);
                    DTO_StoreManager manager_dto = DAO_StoreManager.Load(manager_filter);
                    var user_filter = Builders<BsonDocument>.Filter.Eq("_id", manager_id);
                    StoreManager manager = new StoreManager(LoadRegisteredUser(user_filter), store , new Permission(manager_dto.Permission) , founder );
                    founder.StoreManagers.AddLast(manager);
                    store.Managers.TryAdd(manager_id, manager);

                    // add manager to identity map
                    AddManagerToIdentityMap(manager);
                }
            }


            store.Owners.TryAdd(founder_id, founder);

            // add owner to identity map
            AddOwnerToIdentityMap(founder);

            return founder;
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

        private void AddOwnerToIdentityMap(StoreOwner owner)
        {
            LinkedList<StoreOwner> list;
            bool listExists = StoreOwners.TryGetValue(owner.GetId(), out list);
            if (listExists)
            {
                list.AddLast(owner);
            }
            else
            {
                list = new LinkedList<StoreOwner>();
                list.AddLast(owner);
                StoreOwners.TryAdd(owner.GetId(), list);
            }
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

            p = new Product(dto._id, dto.Name, dto.Price, dto.Quantity, dto.Category, dto.Keywords, dto.Review);
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

            ConcurrentDictionary<String, Product> products = new ConcurrentDictionary<String, Product>();
            NotificationManager notificationManager = new NotificationManager();

            foreach (String product in dto.InventoryManager)
            {
                var filter3 = Builders<BsonDocument>.Filter.Eq("_id", product);
                Product p = LoadProduct(filter3);
                p.NotificationManager = notificationManager;
                products.TryAdd(product, p);
            }

            s = new Store(dto._id, dto.Name, new InventoryManager(products), ToObject(dto.History), dto.Rating, dto.NumberOfRates, notificationManager);

            Stores.TryAdd(s.Id, s);

            StoreOwner founder = getOwnershipTree(s, dto.Founder);

            s.Founder = founder;

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

        #region Policies

        #region Purchase Policies

        public void Create(Auction auction)
        {
            DAO_Auction.Create(new DTO_Auction(auction.Id, auction.ClosingTime.ToString(), auction.StartingPrice, auction.LastOffer.Item1 , auction.LastOffer.Item2));
            Policy_Auctions.TryAdd(auction.Id, auction);
        }

        public Auction LoadAuctionPolicy(FilterDefinition<BsonDocument> filter)
        {
            Auction a;
            DTO_Auction dto = DAO_Auction.Load(filter);
            if (Policy_Auctions.TryGetValue(dto._id, out a))
            {
                return a;
            }

            a = new Auction(dto._id, dto.ClosingTime, dto.StartingPrice, new Tuple<double, string>(dto.LastOffer_Price , dto.LastOffer_UserId));
            Policy_Auctions.TryAdd(a.Id, a);
            return a;
        }

        public void UpdateAuctionPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_Auction.Update(filter, update);
        }

        public void DeleteAuctionPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_Auction deletedAuction = DAO_Auction.Delete(filter);
            Policy_Auctions.TryRemove(deletedAuction._id, out Auction a);
        }


        public void Create(Lottery lottery)
        {
            DAO_Lottery.Create(new DTO_Lottery(lottery.Id, lottery.Price, lottery.Participants));
            Policy_Lotterys.TryAdd(lottery.Id, lottery);
        }

        public Lottery LoadLotteryPolicy(FilterDefinition<BsonDocument> filter)
        {
            Lottery l;
            DTO_Lottery dto = DAO_Lottery.Load(filter);
            if (Policy_Lotterys.TryGetValue(dto._id, out l))
            {
                return l;
            }

            l = new Lottery(dto._id, dto.Price, dto.Participants);
            Policy_Lotterys.TryAdd(l.Id, l);
            return l;
        }

        public void UpdateLotteryPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_Lottery.Update(filter, update);
        }

        public void DeleteLotteryPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_Lottery deletedLottery = DAO_Lottery.Delete(filter);
            Policy_Lotterys.TryRemove(deletedLottery._id, out Lottery l);
        }


        public void Create(MaxProductPolicy maxProductPolicy)
        {            
            DAO_MaxProductPolicy.Create(new DTO_MaxProductPolicy(maxProductPolicy.Id, maxProductPolicy.Product.Id, maxProductPolicy.Max));
            Policy_MaxProductPolicys.TryAdd(maxProductPolicy.Id, maxProductPolicy);
        }

        public MaxProductPolicy LoadMaxProductPolicy(FilterDefinition<BsonDocument> filter)
        {
            MaxProductPolicy m;
            DTO_MaxProductPolicy dto = DAO_MaxProductPolicy.Load(filter);
            if (Policy_MaxProductPolicys.TryGetValue(dto._id, out m))
            {
                return m;
            }
            var product_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.Product);
            m = new MaxProductPolicy(LoadProduct(product_filter), dto.Max, dto._id);
            Policy_MaxProductPolicys.TryAdd(m.Id, m);
            return m;
        }

        public void UpdateMaxProductPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_MaxProductPolicy.Update(filter, update);
        }

        public void DeleteMaxProductPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_MaxProductPolicy deletedMaxProductPolicy = DAO_MaxProductPolicy.Delete(filter);
            Policy_MaxProductPolicys.TryRemove(deletedMaxProductPolicy._id, out MaxProductPolicy m);
        }


        public void Create(MinAgePolicy minAgePolicy)
        {
            DAO_MinAgePolicy.Create(new DTO_MinAgePolicy(minAgePolicy.Id, minAgePolicy.Age));
            Policy_MinAgePolicys.TryAdd(minAgePolicy.Id, minAgePolicy);
        }

        public MinAgePolicy LoadMinAgePolicy(FilterDefinition<BsonDocument> filter)
        {
            MinAgePolicy m;
            DTO_MinAgePolicy dto = DAO_MinAgePolicy.Load(filter);
            if (Policy_MinAgePolicys.TryGetValue(dto._id, out m))
            {
                return m;
            }
            m = new MinAgePolicy(dto.Age, dto._id);
            Policy_MinAgePolicys.TryAdd(m.Id, m);
            return m;
        }

        public void UpdateMinAgePolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_MinAgePolicy.Update(filter, update);
        }

        public void DeleteMinAgePolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_MinAgePolicy deletedMinAgePolicy = DAO_MinAgePolicy.Delete(filter);
            Policy_MinAgePolicys.TryRemove(deletedMinAgePolicy._id, out MinAgePolicy m);
        }


        public void Create(MinProductPolicy minProductPolicy)
        {
            DAO_MinProductPolicy.Create(new DTO_MinProductPolicy(minProductPolicy.Id, minProductPolicy.Product.Id , minProductPolicy.Min));
            Policy_MinProductPolicys.TryAdd(minProductPolicy.Id, minProductPolicy);
        }

        public MinProductPolicy LoadMinProductPolicy(FilterDefinition<BsonDocument> filter)
        {
            MinProductPolicy m;
            DTO_MinProductPolicy dto = DAO_MinProductPolicy.Load(filter);
            if (Policy_MinProductPolicys.TryGetValue(dto._id, out m))
            {
                return m;
            }
            var product_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.Product);
            m = new MinProductPolicy(LoadProduct(product_filter), dto.Min, dto._id);
            Policy_MinProductPolicys.TryAdd(m.Id, m);
            return m;
        }

        public void UpdateMinProductPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_MinProductPolicy.Update(filter, update);
        }

        public void DeleteMinProductPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_MinProductPolicy deletedMinProductPolicy = DAO_MinProductPolicy.Delete(filter);
            Policy_MinProductPolicys.TryRemove(deletedMinProductPolicy._id, out MinProductPolicy m);
        }


        public void Create(Offer offer)
        {
            DAO_Offer.Create(new DTO_Offer(offer.Id, offer.LastOffer.Item1, offer.LastOffer.Item2 , offer.CounterOffer , offer.Accepted));
            Policy_Offers.TryAdd(offer.Id, offer);
        }

        public Offer LoadOfferPolicy(FilterDefinition<BsonDocument> filter)
        {
            Offer o;
            DTO_Offer dto = DAO_Offer.Load(filter);
            if (Policy_Offers.TryGetValue(dto._id, out o))
            {
                return o;
            }

            o = new Offer(dto._id , new Tuple<Double , string>(dto.LastOffer_Price , dto.LastOffer_UserId) , dto.CounterOffer , dto.Accepted);
            Policy_Offers.TryAdd(o.Id, o);
            return o;
        }

        public void UpdateOfferPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_Offer.Update(filter, update);
        }

        public void DeleteOfferPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_Offer deletedOffer = DAO_Offer.Delete(filter);
            Policy_Offers.TryRemove(deletedOffer._id, out Offer o);
        }


        public void Create(RestrictedHoursPolicy restrictedHoursPolicy)
        {
            DAO_RestrictedHoursPolicy.Create(new DTO_RestrictedHoursPolicy(restrictedHoursPolicy.Id, restrictedHoursPolicy.StartRestrict.ToString(), restrictedHoursPolicy.EndRestrict.ToString(), restrictedHoursPolicy.Product.Id));
            Policy_RestrictedHoursPolicys.TryAdd(restrictedHoursPolicy.Id, restrictedHoursPolicy);
        }

        public RestrictedHoursPolicy LoadRestrictedHoursPolicy(FilterDefinition<BsonDocument> filter)
        {
            RestrictedHoursPolicy r;
            DTO_RestrictedHoursPolicy dto = DAO_RestrictedHoursPolicy.Load(filter);
            if (Policy_RestrictedHoursPolicys.TryGetValue(dto._id, out r))
            {
                return r;
            }

            var product_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.Product);
            r = new RestrictedHoursPolicy(TimeSpan.Parse(dto.StartRestrict), TimeSpan.Parse(dto.EndRestrict), LoadProduct(product_filter), dto._id);
            Policy_RestrictedHoursPolicys.TryAdd(r.Id, r);
            return r;
        }

        public void UpdateRestrictedHoursPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_RestrictedHoursPolicy.Update(filter, update);
        }

        public void DeleteRestrictedHoursPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_RestrictedHoursPolicy deletedRestrictedHoursPolicy = DAO_RestrictedHoursPolicy.Delete(filter);
            Policy_RestrictedHoursPolicys.TryRemove(deletedRestrictedHoursPolicy._id, out RestrictedHoursPolicy r);
        }

        

        public void Create(AndPolicy andPolicy)
        {          
            DAO_AndPolicy.Create(new DTO_AndPolicy(andPolicy.Id, getPoliciesIDs(andPolicy.Policies)));
            Policy_AndPolicys.TryAdd(andPolicy.Id, andPolicy);
        }

        public AndPolicy LoadAndPolicy(FilterDefinition<BsonDocument> filter)
        {
            AndPolicy a;
            DTO_AndPolicy dto = DAO_AndPolicy.Load(filter);
            if (Policy_AndPolicys.TryGetValue(dto._id, out a))
            {
                return a;
            }

            List<IPurchasePolicy> Policies = new List<IPurchasePolicy>();
            foreach (var policy in dto.Policies)
            {
                IPurchasePolicy p = LoadIPurchasePolicy(policy.Key, policy.Value);
                Policies.Add(p);
            }

            a = new AndPolicy(Policies, dto._id);
            Policy_AndPolicys.TryAdd(a.Id, a);
            return a;
        }       

        public void UpdateAndPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_AndPolicy.Update(filter, update);
        }

        public void DeleteAndPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_AndPolicy deletedAndPolicy = DAO_AndPolicy.Delete(filter);
            foreach (var policy in deletedAndPolicy.Policies)
            {
                DeleteIPurchasePolicy(policy.Key, policy.Value);
            }
            Policy_AndPolicys.TryRemove(deletedAndPolicy._id, out AndPolicy a);
        }

      

        public void Create(OrPolicy orPolicy)
        {            
            DAO_OrPolicy.Create(new DTO_OrPolicy(orPolicy.Id, getPoliciesIDs(orPolicy.Policies)));
            Policy_OrPolicys.TryAdd(orPolicy.Id, orPolicy);
        }

        public OrPolicy LoadOrPolicy(FilterDefinition<BsonDocument> filter)
        {
            OrPolicy o;
            DTO_OrPolicy dto = DAO_OrPolicy.Load(filter);
            if (Policy_OrPolicys.TryGetValue(dto._id, out o))
            {
                return o;
            }

            List<IPurchasePolicy> Policies = new List<IPurchasePolicy>();
            foreach (var policy in dto.Policies)
            {
                IPurchasePolicy p = LoadIPurchasePolicy(policy.Key, policy.Value);
                Policies.Add(p);
            }

            o = new OrPolicy(Policies, dto._id);
            Policy_OrPolicys.TryAdd(o.Id, o);
            return o;
        }

        public void UpdateOrPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_OrPolicy.Update(filter, update);
        }

        public void DeleteOrPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_OrPolicy deletedOrPolicy = DAO_OrPolicy.Delete(filter);
            foreach(var policy in deletedOrPolicy.Policies)
            {
                DeleteIPurchasePolicy(policy.Key, policy.Value);
            }
            Policy_OrPolicys.TryRemove(deletedOrPolicy._id, out OrPolicy o);
        }



        public void Create(BuyNow buyNow)
        {
            DAO_BuyNow.Create(new DTO_BuyNow(buyNow.Id, new DTO_AndPolicy(buyNow.Policy.Id , getPoliciesIDs(buyNow.Policy.Policies))));
            Policy_BuyNows.TryAdd(buyNow.Id, buyNow);
        }

        public BuyNow LoadBuyNowPolicy(FilterDefinition<BsonDocument> filter)
        {
            BuyNow b;
            DTO_BuyNow dto = DAO_BuyNow.Load(filter);
            if (Policy_BuyNows.TryGetValue(dto._id, out b))
            {
                return b;
            }

            List<IPurchasePolicy> Policies = new List<IPurchasePolicy>();
            foreach (var policy in dto.Policy.Policies)
            {
                IPurchasePolicy p = LoadIPurchasePolicy(policy.Key, policy.Value);
                Policies.Add(p);
            }

            AndPolicy andPolicy = new AndPolicy(Policies, dto.Policy._id);
            b = new BuyNow(andPolicy, dto._id);
            Policy_BuyNows.TryAdd(b.Id, b);
            return b;
        }

        public void UpdateBuyNowPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_BuyNow.Update(filter, update);
        }

        public void DeleteBuyNowPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_BuyNow deletedBuyNow = DAO_BuyNow.Delete(filter);
            foreach (var policy in deletedBuyNow.Policy.Policies)
            {
                DeleteIPurchasePolicy(policy.Key, policy.Value);
            }
            Policy_BuyNows.TryRemove(deletedBuyNow._id, out BuyNow b);
        }




        public void Create(ConditionalPolicy conditionalPolicy)
        {
            List<IPurchasePolicy> list = new List<IPurchasePolicy>();
            list.Add(conditionalPolicy.PreCond);
            ConcurrentDictionary<String, String> PreCond = getPoliciesIDs(list);

            List<IPurchasePolicy> list2 = new List<IPurchasePolicy>();            
            list2.Add(conditionalPolicy.Cond);
            ConcurrentDictionary<String, String> Cond = getPoliciesIDs(list2);


            DAO_ConditionalPolicy.Create(new DTO_ConditionalPolicy(conditionalPolicy.Id, PreCond , Cond) );
            Policy_ConditionalPolicys.TryAdd(conditionalPolicy.Id, conditionalPolicy);
        }

        public ConditionalPolicy LoadConditionalPolicy(FilterDefinition<BsonDocument> filter)
        {
            ConditionalPolicy c;
            DTO_ConditionalPolicy dto = DAO_ConditionalPolicy.Load(filter);
            if (Policy_ConditionalPolicys.TryGetValue(dto._id, out c))
            {
                return c;
            }
            IPurchasePolicy pre = null;
            foreach (var policy in dto.PreCond)
            {
                pre = LoadIPurchasePolicy(policy.Key, policy.Value);
            }

            IPurchasePolicy cond = null;
            foreach (var policy in dto.Cond)
            {
                cond = LoadIPurchasePolicy(policy.Key, policy.Value);
            }
            
            c = new ConditionalPolicy(pre, cond, dto._id);
            Policy_ConditionalPolicys.TryAdd(c.Id, c);
            return c;
        }

        public void UpdateConditionalPolicy(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
        {
            DAO_ConditionalPolicy.Update(filter, update);
        }

        public void DeleteConditionalPolicy(FilterDefinition<BsonDocument> filter)
        {
            DTO_ConditionalPolicy deletedConditionalPolicy = DAO_ConditionalPolicy.Delete(filter);
            foreach (var policy in deletedConditionalPolicy.PreCond)
            {
                DeleteIPurchasePolicy(policy.Key, policy.Value);
            }
            foreach (var policy in deletedConditionalPolicy.Cond)
            {
                DeleteIPurchasePolicy(policy.Key, policy.Value);
            }            

            Policy_ConditionalPolicys.TryRemove(deletedConditionalPolicy._id, out ConditionalPolicy c);
        }
        #endregion Purchase Policies

        #endregion Policies

        #region Utils
        public void clearDB()
        {
            if (!(Instance is null))
            {
                var emptyFilter = Builders<BsonDocument>.Filter.Empty;
                DAO_RegisteredUser.collection.DeleteMany(emptyFilter);
                DAO_Product.collection.DeleteMany(emptyFilter);
                DAO_StoreManager.collection.DeleteMany(emptyFilter);
                DAO_StoreOwner.collection.DeleteMany(emptyFilter);
                DAO_Store.collection.DeleteMany(emptyFilter);
                DAO_Auction.collection.DeleteMany(emptyFilter);
                DAO_Lottery.collection.DeleteMany(emptyFilter);
                DAO_MaxProductPolicy.collection.DeleteMany(emptyFilter);
                DAO_MinAgePolicy.collection.DeleteMany(emptyFilter);
                DAO_MinProductPolicy.collection.DeleteMany(emptyFilter);
                DAO_Offer.collection.DeleteMany(emptyFilter);
                DAO_RestrictedHoursPolicy.collection.DeleteMany(emptyFilter);
                DAO_AndPolicy.collection.DeleteMany(emptyFilter);
                DAO_OrPolicy.collection.DeleteMany(emptyFilter);
                DAO_BuyNow.collection.DeleteMany(emptyFilter);
                DAO_ConditionalPolicy.collection.DeleteMany(emptyFilter);

                RegisteredUsers.Clear();
                GuestUsers.Clear();
                Products.Clear();
                StoreManagers.Clear();
                StoreOwners.Clear();
                Stores.Clear();
                Policy_Auctions.Clear();
                Policy_Lotterys.Clear();
                Policy_MaxProductPolicys.Clear();
                Policy_MinAgePolicys.Clear();
                Policy_MinProductPolicys.Clear();
                Policy_Offers.Clear();
                Policy_RestrictedHoursPolicys.Clear();
                Policy_AndPolicys.Clear();
                Policy_OrPolicys.Clear();
                Policy_BuyNows.Clear();
                Policy_ConditionalPolicys.Clear();
    }

    }
        #endregion

        #region Methods TO Delete
        //TODO - delete
        //private StoreManager LoadStoreManager(FilterDefinition<BsonDocument> filter)
        //{
        //    StoreManager sm;
        //    LinkedList<StoreManager> list;
        //    DTO_StoreManager dto = DAO_StoreManager.Load(filter);       // TODO - need to make sure when loading store manager the filter includes user id and store id

        //    bool listExists = StoreManagers.TryGetValue(dto.UserId, out list);
        //    if (listExists)
        //    {
        //        foreach(StoreManager manager in list)
        //        {
        //            if(manager.Store.Id == dto.StoreId)
        //            {
        //                return manager;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        list = new LinkedList<StoreManager>();
        //    }

        //    var user_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.UserId);
        //    var store_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.StoreId);
        //    var owner_filter = Builders<BsonDocument>.Filter.Eq("UserId", dto.AppointedBy)& Builders<BsonDocument>.Filter.Eq("StoreId", dto.StoreId);

        //    sm = new StoreManager(LoadRegisteredUser(user_filter), LoadStore(store_filter), new Permission(dto.Permission), LoadStoreOwner(owner_filter));

        //    list.AddLast(sm);
        //    if (!listExists)
        //    {
        //        StoreManagers.TryAdd(sm.GetId(), list);
        //    }
        //    // else added to list pointer

        //    return sm;
        //}


        //// TODO - delete
        //private StoreOwner LoadStoreOwner(FilterDefinition<BsonDocument> filter)
        //{
        //    StoreOwner so;
        //    LinkedList<StoreOwner> list;
        //    DTO_StoreOwner dto = DAO_StoreOwner.Load(filter);       // TODO - need to make sure when loading store manager the filter includes user id and store id

        //    bool listExists = StoreOwners.TryGetValue(dto.UserId, out list);
        //    if (listExists)
        //    {
        //        foreach (StoreOwner owner in list) { if (owner.Store.Id == dto.StoreId) { return owner; } }

        //    }
        //    else { list = new LinkedList<StoreOwner>(); }

        //    var user_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.UserId);
        //    var store_filter = Builders<BsonDocument>.Filter.Eq("_id", dto.StoreId);
        //    var owner_filter = Builders<BsonDocument>.Filter.Eq("UserId", dto.AppointedBy) & Builders<BsonDocument>.Filter.Eq("StoreId", dto.StoreId);            

        //    LinkedList<StoreOwner> owners = new LinkedList<StoreOwner>();
        //    LinkedList<StoreManager> managers = new LinkedList<StoreManager>();

        //    foreach (String owner in dto.StoreOwners)
        //    {
        //        var filter1 = Builders<BsonDocument>.Filter.Eq("UserId", owner) & Builders<BsonDocument>.Filter.Eq("StoreId", dto.StoreId);
        //        owners.AddLast(LoadStoreOwner(filter1));
        //    }
        //    foreach (String manager in dto.StoreManagers)
        //    {
        //        var filter2 = Builders<BsonDocument>.Filter.Eq("UserId", manager) & Builders<BsonDocument>.Filter.Eq("StoreId", dto.StoreId);
        //        managers.AddLast(LoadStoreManager(filter2)); 
        //    }

        //    so = new StoreOwner(LoadRegisteredUser(user_filter), LoadStore(store_filter), LoadStoreOwner(owner_filter), managers, owners);

        //    list.AddLast(so);
        //    if (!listExists)  { StoreOwners.TryAdd(so.GetId(), list); }
        //    // else added to list pointer

        //    //TODO - inject owners and managers

        //    return so;
        //}        


        //private Store LoadStore(FilterDefinition<BsonDocument> filter)
        //{
        //    Store s;
        //    DTO_Store dto = DAO_Store.Load(filter);
        //    if (Stores.TryGetValue(dto._id, out s)) { return s; }

        //    var founder_filter = Builders<BsonDocument>.Filter.Eq("UserId", dto.Founder)&Builders<BsonDocument>.Filter.Eq("StoreId", dto._id);

        //    ConcurrentDictionary<String, StoreOwner> owners = new ConcurrentDictionary<String, StoreOwner>();
        //    ConcurrentDictionary<String, StoreManager> managers = new ConcurrentDictionary<String, StoreManager>();
        //    ConcurrentDictionary<String, Product> products = new ConcurrentDictionary<String, Product>();

        //    foreach (String owner in dto.Owners)
        //    {
        //        var filter1 = Builders<BsonDocument>.Filter.Eq("UserId", owner) & Builders<BsonDocument>.Filter.Eq("StoreId", dto._id);
        //        owners.TryAdd(owner , LoadStoreOwner(filter1));
        //    }
        //    foreach (String manager in dto.Managers)
        //    {
        //        var filter2 = Builders<BsonDocument>.Filter.Eq("UserId", manager) & Builders<BsonDocument>.Filter.Eq("StoreId", dto._id);
        //        managers.TryAdd(manager , LoadStoreManager(filter2));
        //    }

        //    NotificationManager notificationManager = new NotificationManager();

        //    foreach(String product in dto.InventoryManager)
        //    {
        //        var filter3 = Builders<BsonDocument>.Filter.Eq("_id", product);
        //        Product p = LoadProduct(filter3);
        //        p.NotificationManager = notificationManager;
        //        products.TryAdd(product, p );
        //    }

        //    InventoryManager inventory = new InventoryManager(products);

        //    s = new Store(dto._id, dto.Name, LoadStoreOwner(founder_filter), owners, managers, inventory, ToObject(dto.History), dto.Rating, dto.NumberOfRates , notificationManager);                            

        //    Stores.TryAdd(s.Id, s);

        //    // TODO - inject owners and managers

        //    return s;
        //}


        #endregion Methods TO Delete
    }
}

