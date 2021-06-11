
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DataAccessLayer;
using MongoDB.Driver;
using MongoDB.Bson;
using Terminal3.DataAccessLayer.DTOs;
using System.Security.Cryptography;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public interface IUsersAndPermissionsFacade
    {
        Result<RegisteredUser> Register(String email, String password, String Id);
        Result<RegisteredUser> AddSystemAdmin(String email); 
        Result<RegisteredUser> RemoveSystemAdmin(String email);
        Result<RegisteredUser> Login(String email, String password);
        Result<RegisteredUser> Login(String email, String password,String GuestUserID);
        Result<GuestUser> LogOut(String email);
        Result<Product> AddProductReview(String userID, Store store, Product product, String review);
        Result<History> GetUserPurchaseHistory(String userID);
        Result<Boolean> AddProductToCart(string userID, Product product, int productQuantity, Store store);
        Result<Boolean> UpdateShoppingCart(string userID, string storeID, Product product, int quantity);
        Result<ShoppingCart> GetUserShoppingCart(string userID);
        Result<Boolean> ExitSystem(String userID);
        Result<double> GetTotalShoppingCartPrice(String userID);
        Result<ShoppingCart> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails, MongoDB.Driver.IClientSessionHandle session = null);


    }

    public class UsersAndPermissionsFacade : IUsersAndPermissionsFacade
    {
        //Properties
        public ConcurrentDictionary<String,RegisteredUser> RegisteredUsers { get; }
        public ConcurrentDictionary<String, RegisteredUser> SystemAdmins { get; }
        public ConcurrentDictionary<String, GuestUser> GuestUsers { get; }
        public Mapper mapper;

        private readonly object my_lock = new object();
        private RegisteredUser defaultUser;

        //Constructor
        public UsersAndPermissionsFacade(String admin_email, String admin_password)
        {
            RegisteredUsers = new ConcurrentDictionary<String, RegisteredUser>();
            SystemAdmins = new ConcurrentDictionary<String, RegisteredUser>();
            GuestUsers = new ConcurrentDictionary<String, GuestUser>();
            mapper = Mapper.getInstance();
            //Add first system admin
            LoadAllRegisterUsers();
            LoadSystemAdmins();
            if (SystemAdmins.IsEmpty)
            {
                defaultUser = new RegisteredUser("-777", admin_email, admin_password);
                insertInitializeData(defaultUser);
            }
        }

        //Methods
        /// <summary>
        /// Regiter user to the system using given Email and Password.
        /// the registration will be success only if the enail is unique and not in use by other user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Result object hold the execStatus</returns>
        public Result<RegisteredUser> Register(String email, String password , String Id)
        {
            try
            {
                Monitor.Enter(my_lock);
                try
                {
                    if (isUniqueEmail(email))
                    {
                        RegisteredUser newUser;                  
                        if (Id == "-1")
                            newUser = new RegisteredUser(email, password);
                        else
                            newUser = new RegisteredUser(Id , email, password);

                        this.RegisteredUsers.TryAdd(newUser.Id, newUser);

                        //create in DB
                        mapper.Create(newUser);
                        
                        return new Result<RegisteredUser>($"{email} is registered as new user", true, newUser);
                    }
                    else
                    {
                        return new Result<RegisteredUser>($"{email} is aleady in user\n Please use different email", false, null);
                    }
                }
                finally
                {
                    Monitor.Exit(my_lock);
                }
            }
            catch (SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<RegisteredUser>(SyncEx.Message, false, null);
            }
        }

        /// <summary>
        /// Adding new system admin to the system; First check if the user is registered, if so
        /// the new admin is added.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
        public Result<RegisteredUser> AddSystemAdmin(String email) 
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email,RegisteredUsers);
            //registered user has been found
            if (searchResult.ExecStatus)
            {
                this.SystemAdmins.TryAdd(searchResult.Data.Id, searchResult.Data);

                // Update DB
                var filter = Builders<BsonDocument>.Filter.Eq("_id", "");  //TODO Mongo _id empty 
                var update = Builders<BsonDocument>.Update.Set("SystemAdmins", getDTO_admins().SystemAdmins);
                mapper.UpdateSystemAdmins(filter, update);

                return new Result<RegisteredUser>($"{email} has been added as system admin\n", true, searchResult.Data);
            }
            else
            {
                return new Result<RegisteredUser>($"could not found registerd user with email: {email}\n", false, null);

            }

        }

        /// <summary>
        /// Removing  system admin to the system; First check if the user is registered, if so
        /// the user is been removed from system admins list
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>result of the operation</returns>
         
        public Result<RegisteredUser> RemoveSystemAdmin(String email)
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email,SystemAdmins);
            RegisteredUser removedUser;
            if (searchResult.ExecStatus)
            {
                //registered user has been found
                //Check the constrain for at least one system admin
                if (this.SystemAdmins.Count > 1)
                {
                    this.SystemAdmins.TryRemove(searchResult.Data.Id, out removedUser);

                    // Update DB
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", "");  //TODO Mongo _id empty 
                    var update = Builders<BsonDocument>.Update.Set("SystemAdmins", getDTO_admins().SystemAdmins);
                    mapper.UpdateSystemAdmins(filter, update);

                    return new Result<RegisteredUser>($"{removedUser.Email} has been removed as system admin\n", true, removedUser);
                }

                //there is only one system admin
                else
                {
                    return new Result<RegisteredUser>($"{email} could not be removed as system admin\n The system need at least one system admin\n", false, null);

                }
            }
            //register user could not be found
            else
            {
                return new Result<RegisteredUser>($"could not found registerd user with email: {email}\n", false, null);

            }
        }

        /// <summary>
        /// util function in order to check the the given email is not in use by other user
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// boolean wwther the email is unique or not
        /// </returns>
        private bool isUniqueEmail(string email)
        {
           foreach(RegisteredUser registerUser in RegisteredUsers.Values)
            {
                if (registerUser.Email.Equals(email))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Util function to get RegisterUser object if tis exist
        /// </summary>
        /// <param name="email"></param>
        /// <param name="table">from which table source to find the user [RegisteredUser/ SystemAdmins]</param>
        /// <returns></returns>
        public Result<RegisteredUser> FindUserByEmail(String email, ConcurrentDictionary<String, RegisteredUser> table)
        {
            foreach (RegisteredUser registeredUser in table.Values)
            {
                if (registeredUser.Email.Equals(email))
                {
                    return new Result<RegisteredUser>($"found user with email:{email}\n", true, registeredUser);
                }
            }

            return new Result<RegisteredUser>($"could not find user with email:{email}\n", false, null);
        }

        /// <summary>
        /// Login function - this function will look for the user with the given email address
        /// and try to verify the given password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Result<RegisteredUser> Login(String email, String password , String guestId)
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email, RegisteredUsers);
            if (searchResult.ExecStatus)
            {
                //User Found
                Result<RegisteredUser> res_ru = searchResult.Data.Login(password);
                if (res_ru.ExecStatus)
                {
                    //Delete relevant guest user from list 
                    GuestUsers.TryRemove(guestId , out GuestUser guest);

                    // Update DB
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", res_ru.Data.Id);
                    var update = Builders<BsonDocument>.Update.Set("LoggedIn", true);
                    mapper.UpdateRegisteredUser(filter, update);
                    mapper.Load_RegisteredUserNotifications(res_ru.Data);
                    mapper.Load_RegisteredUserShoppingCart(res_ru.Data);
                    return res_ru;
                }

                //else faild
                return new Result<RegisteredUser>(res_ru.Message, false, null);
            }
            else
            {
                //No user if found using the given email
                return new Result<RegisteredUser>($"There is not user using this email:{email}\n", false, null);

            }
        }

        public Result<RegisteredUser> Login(String email, String password)
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email, RegisteredUsers);
            if (searchResult.ExecStatus)
            {
                //User Found
                Result<RegisteredUser> res_ru =  searchResult.Data.Login(password);

                // Update DB
                if (res_ru.ExecStatus)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", res_ru.Data.Id);
                    var update = Builders<BsonDocument>.Update.Set("LoggedIn", true);
                    mapper.UpdateRegisteredUser(filter, update);
                    mapper.Load_RegisteredUserNotifications(res_ru.Data);
                    mapper.Load_RegisteredUserShoppingCart(res_ru.Data);
                }
                return res_ru;

            }
            else
            {
                //No user if found using the given email
                return new Result<RegisteredUser>($"There is not user using this email:{email}\n", false, null);

            }
        }

        /// <summary>
        /// Logout function - the function will look for the user with the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Result<GuestUser> LogOut(String email)
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email, RegisteredUsers);
            if (searchResult.ExecStatus)
            {
                //User 
                Result<GuestUser> res = searchResult.Data.LogOut();
                if (res.ExecStatus)
                {
                    GuestUsers.TryAdd(res.Data.Id, res.Data);

                    // Update DB
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", searchResult.Data.Id);
                    var update = Builders<BsonDocument>.Update.Set("LoggedIn", false);
                    mapper.UpdateRegisteredUser(filter, update);

                    return new Result<GuestUser>($"{email} logged out\n", true, res.Data);
                }
                else
                    return new Result<GuestUser>(res.Message, false, null);
            }
            else
            {
                //No user if found using the given email
                return new Result<GuestUser>($"There is not user using this email:{email}\n", false, null);

            }
        }

        public Result<Product> AddProductReview(String userID, Store store, Product product , String review)
        {
            if(RegisteredUsers.TryGetValue(userID , out RegisteredUser user))
            {
                Result<Product>  res_p = user.AddProductReview(store, product, review);
                // Update Product in DB
                if (res_p.ExecStatus)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", product.Id);
                    var update = Builders<BsonDocument>.Update.Set("Review", res_p.Data.Review);
                    mapper.UpdateProduct(filter, update);
                }
                return res_p; 
            }
            return new Result<Product>("User does not exists\n", false, null);
        }

        public Result<History> GetUserPurchaseHistory(String userID)
        {
            if (RegisteredUsers.TryGetValue(userID , out RegisteredUser user))
            {
                mapper.Load_RegisteredUserHistory(user);
                return user.GetUserPurchaseHistory();
            }
            return new Result<History>("Not a registered user\n", false, null);
        }

        public Result<bool> AddProductToCart(string userID, Product product, int productQuantity, Store store)
        {
            if (RegisteredUsers.TryGetValue(userID, out RegisteredUser user))   // Check if user is registered
            {
                mapper.Load_StorePolicyManager(store);
                Result<ShoppingCart> res_sc = user.AddProductToCart(product, productQuantity, store);

                // Update DB
                if (res_sc.ExecStatus)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", user.Id);
                    var update = Builders<BsonDocument>.Update.Set("ShoppingCart", res_sc.Data.getDTO());
                    mapper.UpdateRegisteredUser(filter, update);
                    return new Result<Boolean>(res_sc.Message, res_sc.ExecStatus, true);
                }
                return new Result<Boolean>(res_sc.Message, res_sc.ExecStatus, false);
            }
            else if (GuestUsers.TryGetValue(userID, out GuestUser guest))   // Check if active guest
            {
                Result<ShoppingCart> res_sc = guest.AddProductToCart(product, productQuantity, store);
                return new Result<Boolean>(res_sc.Message, res_sc.ExecStatus, true);
            }
            //else failed
            return new Result<bool>($"User (ID: {userID}) does not exists.\n", false, false);
        }

        public Result<Boolean> UpdateShoppingCart(string userID, string storeID, Product product, int quantity)
        {
            if (GuestUsers.TryGetValue(userID, out GuestUser guest_user))
            {
                Result <ShoppingCart> res_sc = guest_user.UpdateShoppingCart(storeID, product, quantity);
                return new Result<Boolean>(res_sc.Message, res_sc.ExecStatus, true);
            }
            else if (RegisteredUsers.TryGetValue(userID, out RegisteredUser registerd_user))
            {
                Result<ShoppingCart> res_sc = registerd_user.UpdateShoppingCart(storeID, product, quantity);

                // Update DB
                if (res_sc.ExecStatus)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", registerd_user.Id);
                    var update = Builders<BsonDocument>.Update.Set("ShoppingCart", res_sc.Data.getDTO());
                    mapper.UpdateRegisteredUser(filter, update);
                    return new Result<Boolean>(res_sc.Message, res_sc.ExecStatus, true);
                }
                return new Result<Boolean>(res_sc.Message, res_sc.ExecStatus, false);
            }
            else
            {
                return new Result<Boolean>("User does not exist\n", false, false);
            }

        }

        public Result<ShoppingCart> GetUserShoppingCart(string userID)
        {
            if (GuestUsers.TryGetValue(userID, out GuestUser guest_user))
            {
                return guest_user.GetUserShoppingCart();
            }
            else if (RegisteredUsers.TryGetValue(userID, out RegisteredUser registerd_user))
            {                
                return registerd_user.GetUserShoppingCart();
            }
            else
            {
                return new Result<ShoppingCart>("User does not exist\n", false, null);
            }
        }

        public Result<Boolean> ExitSystem(String userID)
        {
            if (GuestUsers.TryGetValue(userID, out GuestUser gest_user))
            {
                Result<Boolean> res = gest_user.ExitSystem();
                GuestUsers.Remove(userID, out GuestUser gu);
                return res;
            }
            else if (RegisteredUsers.TryGetValue(userID, out RegisteredUser register_user))
            {
                Result<Boolean> res = register_user.ExitSystem();
                return res;
            }
            else
            {
                return new Result<Boolean>("User does not exist\n", false, false);
            }

        }

        public Result<User> EnterSystem()
        {
            GuestUser guest = new GuestUser();
            GuestUsers.TryAdd(guest.Id, guest);

            //TODO - When adding Service Layer
            //checkIfUserWantsToRegister();            
            //checkIfUserWantsTologin(guest.Id);

            return new Result<User>("New guest user has enterd the system\n", true, guest);
        }

        public Result<double> GetTotalShoppingCartPrice(String userID) {

            if (RegisteredUsers.ContainsKey(userID))
            {
                //User Found
                Double TotalPrice = RegisteredUsers[userID].ShoppingCart.GetTotalShoppingCartPrice().Data;
                return new Result<double>($"Total price of current shoppinh cart is: {TotalPrice}", true, TotalPrice);
            }
            else if (GuestUsers.ContainsKey(userID))
            {
                //Guest User Found
                Double TotalPrice = GuestUsers[userID].ShoppingCart.GetTotalShoppingCartPrice().Data;
                return new Result<double>($"Total price of current shoppinh cart is: {TotalPrice}", true, TotalPrice);
            }
            else
            {
                //No user if found using the given email
                return new Result<double>($"There is no suck user with ID:{userID}\n", false, -1);

            }

        }

        public Result<ShoppingCart> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails , MongoDB.Driver.IClientSessionHandle session = null)
        {
            if (GuestUsers.TryGetValue(userID, out GuestUser guest_user))
            {
                Result<ShoppingCart> ShoppingCart = guest_user.Purchase(paymentDetails, deliveryDetails , session);
                return ShoppingCart;
            }
            else if (RegisteredUsers.TryGetValue(userID, out RegisteredUser registerd_user))
            {
                Result<ShoppingCart> ShoppingCart = registerd_user.Purchase(paymentDetails, deliveryDetails , session);
                if (ShoppingCart.ExecStatus)
                {
                    // Update DB
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", registerd_user.Id);
                    ShoppingCart sc = registerd_user.ShoppingCart;
                    var update_shoppingcart = Builders<BsonDocument>.Update.Set("ShoppingCart", sc.getDTO());
                    mapper.UpdateRegisteredUser(filter, update_shoppingcart , session: session);
                }

                return ShoppingCart; 
            }
            else { return new Result<ShoppingCart>("User does not exist\n", false, null); }
        }


        public DTO_SystemAdmins getDTO_admins()
        {
            LinkedList<String> admins_dto = new LinkedList<string>(); 

            foreach(var admin in SystemAdmins)
            {
                admins_dto.AddLast(admin.Key);
            }
            return new DTO_SystemAdmins(admins_dto);
        }

        public void resetSystem()
        {
            GuestUsers.Clear();
            SystemAdmins.Clear();
            RegisteredUsers.Clear();
            insertInitializeData(defaultUser);
        }
        private void insertInitializeData(RegisteredUser defaultUser)
        {
            this.SystemAdmins.TryAdd(defaultUser.Id, defaultUser);
            this.RegisteredUsers.TryAdd(defaultUser.Id, defaultUser);
            mapper = Mapper.getInstance();
            // Update DB
            DTO_RegisteredUser user_dto = defaultUser.getDTO();
            var filter_gu = Builders<BsonDocument>.Filter.Eq("_id", "-777");
            var update_gu = Builders<BsonDocument>.Update.Set("ShoppingCart", user_dto.ShoppingCart)
                                                         .Set("Email", user_dto.Email)
                                                         .Set("Password", user_dto.Password)
                                                         .Set("LoggedIn", user_dto.LoggedIn)
                                                         .Set("History", user_dto.History)
                                                         .Set("PendingNotification", user_dto.PendingNotification);
            mapper.UpdateRegisteredUser(filter_gu, update_gu, true);

            var filter_admin = Builders<BsonDocument>.Filter.Eq("_id", "");
            var update_admin = Builders<BsonDocument>.Update.Set("SystemAdmins", getDTO_admins().SystemAdmins);
            mapper.UpdateSystemAdmins(filter_admin, update_admin, true);

        }

        public void LoadAllRegisterUsers()
        {
            List<RegisteredUser> _registeredUsers = mapper.LoadAllRegisterUsers();
            foreach(RegisteredUser registered in _registeredUsers)
            {
                RegisteredUsers.TryAdd(registered.Id, registered);
            }
        }
        public void LoadSystemAdmins()
        {
            LinkedList<String> systemAdminsIDs = mapper.LoadAllSystemAdmins();
            if(!(systemAdminsIDs is null))
            {
                foreach (string id in systemAdminsIDs)
                {
                    RegisteredUser user;
                    RegisteredUsers.TryGetValue(id, out user);
                    SystemAdmins.TryAdd(id, user);
                }
            }

        }
    }
}
