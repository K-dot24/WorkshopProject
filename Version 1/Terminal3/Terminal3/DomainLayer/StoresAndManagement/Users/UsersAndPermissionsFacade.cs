
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public interface IUsersAndPermissionsFacade
    {
        Result<RegisteredUser> Register(String email, String password);
        Result<RegisteredUser> AddSystemAdmin(String email); 
        Result<RegisteredUser> RemoveSystemAdmin(String email);
        Result<RegisteredUser> Login(String email, String password);
        Result<Boolean> LogOut(String email);
        Result<Boolean> AddProductReview(String userID, Store store, Product product, String review);
        Result<History> GetUserPurchaseHistory(String userID);
        Result<Boolean> AddProductToCart(string userID, Product product, int productQuantity, Store store);
        Result<Boolean> UpdateShoppingCart(string userID, string storeID, Product product, int quantity);
        Result<ShoppingCart> GetUserShoppingCart(string userID);
        Result<Boolean> ExitSystem(String userID);
        Result<double> GetTotalShoppingCartPrice(String userID);
        Result<ShoppingCart> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);


    }

    public class UsersAndPermissionsFacade : IUsersAndPermissionsFacade
    {
        //Properties
        public ConcurrentDictionary<String,RegisteredUser> RegisteredUsers { get; }
        public ConcurrentDictionary<String, RegisteredUser> SystemAdmins { get; }
        public ConcurrentDictionary<String, GuestUser> GuestUsers { get; }

        private readonly object my_lock = new object();

        //Constructor
        public UsersAndPermissionsFacade()
        {
            RegisteredUsers = new ConcurrentDictionary<String, RegisteredUser>();
            SystemAdmins = new ConcurrentDictionary<String, RegisteredUser>();
            GuestUsers = new ConcurrentDictionary<String, GuestUser>();


            //Add first system admin
            RegisteredUser defaultUser = new RegisteredUser("Admin@terminal3", "Admin");
            this.SystemAdmins.TryAdd(defaultUser.Id, defaultUser );
            this.RegisteredUsers.TryAdd(defaultUser.Id, defaultUser);

        }
        //Constructor for the initializer
        public UsersAndPermissionsFacade(ConcurrentDictionary<String, RegisteredUser>  registeredUsers,
                                           ConcurrentDictionary<String, RegisteredUser>  systemAdmins)
        {
            this.RegisteredUsers = registeredUsers;
            this.SystemAdmins = systemAdmins;
        }


        //Methods
        /// <summary>
        /// Regiter user to the system using given Email and Password.
        /// the registration will be success only if the enail is unique and not in use by other user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Result object hold the execStatus</returns>
        public Result<RegisteredUser> Register(String email, String password)
        {
            try
            {
                Monitor.TryEnter(my_lock);
                try
                {
                    if (isUniqueEmail(email))
                    {
                        RegisteredUser newUser = new RegisteredUser(email, password);
                        this.RegisteredUsers.TryAdd(newUser.Id, newUser);
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
        private Result<RegisteredUser> FindUserByEmail(String email, ConcurrentDictionary<String, RegisteredUser> table)
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
                Result<RegisteredUser> res = searchResult.Data.Login(password);
                if (res.ExecStatus)
                {
                    //Delete relevant guest user from list 
                    GuestUsers.TryRemove(guestId , out GuestUser guest);
                    guest.Active = false;
                    return res;
                }
                //else faild
                return new Result<RegisteredUser>(res.Message, false, null);
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
                return searchResult.Data.Login(password);
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
        public Result<Boolean> LogOut(String email)
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email, RegisteredUsers);
            if (searchResult.ExecStatus)
            {
                //User 
                Result<GuestUser> res = searchResult.Data.LogOut();
                if (res.ExecStatus)
                {
                    GuestUsers.TryAdd(res.Data.Id, res.Data);
                    return new Result<Boolean>($"There is not user using this email:{email}\n", true, true);
                }
                else
                    return new Result<Boolean>(res.Message, false, false);
            }
            else
            {
                //No user if found using the given email
                return new Result<Boolean>($"There is not user using this email:{email}\n", false, false);

            }
        }

        public Result<Boolean> AddProductReview(String userID, Store store, Product product , String review)
        {
            if(RegisteredUsers.TryGetValue(userID , out RegisteredUser user))
            {
                return user.AddProductReview(store, product , review);
            }
            return new Result<Boolean>("User does not exists\n", false, false);
        }

        public Result<History> GetUserPurchaseHistory(String userID)
        {
            if (RegisteredUsers.TryGetValue(userID , out RegisteredUser user))
            {
                return user.GetUserPurchaseHistory();
            }
            return new Result<History>("Not a registered user\n", false, null);
        }

        public Result<bool> AddProductToCart(string userID, Product product, int productQuantity, Store store)
        {
            if (RegisteredUsers.TryGetValue(userID, out RegisteredUser user))   // Check if user is registered
            {
                return user.AddProductToCart(product, productQuantity, store);
            }
            else if (GuestUsers.TryGetValue(userID, out GuestUser guest))   // Check if active guest
            {
                return guest.AddProductToCart(product, productQuantity, store);
            }
            //else failed
            return new Result<bool>($"User (ID: {userID}) does not exists.\n", false, false);
        }

        public Result<Boolean> UpdateShoppingCart(string userID, string storeID, Product product, int quantity)
        {
            if (GuestUsers.TryGetValue(userID, out GuestUser guest_user))
            {
                return guest_user.UpdateShoppingCart(storeID, product, quantity);
            }
            else if (RegisteredUsers.TryGetValue(userID, out RegisteredUser registerd_user))
            {
                return registerd_user.UpdateShoppingCart(storeID, product, quantity);
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
                Double TotalPrice = RegisteredUsers[userID].ShoppingCart.GetTotalShoppingCartPrice();
                return new Result<double>($"Total price of current shoppinh cart is: {TotalPrice}", true, TotalPrice);
            }
            else
            {
                //No user if found using the given email
                return new Result<double>($"There is no suck user with ID:{userID}\n", false, -1);

            }

        }

        public Result<ShoppingCart> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            if (GuestUsers.TryGetValue(userID, out GuestUser guest_user))
            {
                return guest_user.Purchase(paymentDetails , deliveryDetails);
            }
            else if (RegisteredUsers.TryGetValue(userID, out RegisteredUser registerd_user))
            {
                return registerd_user.Purchase(paymentDetails , deliveryDetails);
            }
            else
            {
                return new Result<ShoppingCart>("User does not exist\n", false, null);
            }
        }

    }
}
