
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DALobjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public interface IUsersAndPermissionsFacade
    {
        Result<RegisteredUser> Register(String email, String password);
        Result<RegisteredUser> AddSystemAdmin(String email); 
        Result<RegisteredUser> RemoveSystemAdmin(String email);
        Result<RegisteredUser> Login(String email, String password);
        Result<RegisteredUser> LogOut(String email);
        Result<History> GetUserPurchaseHistory(String userID);
        Result<Boolean> AddProductToCart(string userID, Product product, int productQuantity, Store store);
    }

    public class UsersAndPermissionsFacade : IUsersAndPermissionsFacade
    {
        //Properties
        public ConcurrentDictionary<String,RegisteredUser> RegisteredUsers { get; }
        public ConcurrentDictionary<String, RegisteredUser> SystemAdmins { get; }
        public ConcurrentDictionary<String, GuestUser> GuestUsers { get; }

        //Constructor
        public UsersAndPermissionsFacade()
        {
            RegisteredUsers = new ConcurrentDictionary<String, RegisteredUser>();
            SystemAdmins = new ConcurrentDictionary<String, RegisteredUser>();
            GuestUsers = new ConcurrentDictionary<String, GuestUser>();
            

            //Add first system admin
            //this.SystemAdmins.TryAdd("Admin@terminal3", new RegisteredUser("Admin@terminal3", "Admin"));

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
            if (isUniqueEmail(email))
            {
                RegisteredUser newUser = new RegisteredUser(email, password);
                this.RegisteredUsers.TryAdd(email, newUser);
                return new Result<RegisteredUser>($"{email} is registered as new user", true, newUser);
            }
            else {
                return new Result<RegisteredUser>($"{email} is aleady in user\n Please use different email", false,null);
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
                this.SystemAdmins.TryAdd(email, searchResult.Data);
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
                    this.SystemAdmins.TryRemove(email, out removedUser);
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
            return !RegisteredUsers.ContainsKey(email);
        }

        /// <summary>
        /// Util function to get RegisterUser object if tis exist
        /// </summary>
        /// <param name="email"></param>
        /// <param name="table">from which table source to find the user [RegisteredUser/ SystemAdmins]</param>
        /// <returns></returns>
        private Result<RegisteredUser> FindUserByEmail(String email, ConcurrentDictionary<String, RegisteredUser> table)
        {
            RegisteredUser requestedUser;
            if (table.TryGetValue(email, out requestedUser)) 
            {
                return new Result<RegisteredUser>($"found user with email:{email}\n",true, requestedUser);
            }
            else
            {
                return new Result<RegisteredUser>($"could not find user with email:{email}\n", false, null);
            } 
        }

        /// <summary>
        /// Login function - this function will look for the user with the given email address
        /// and try to verify the given password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        public Result<RegisteredUser> LogOut(String email)
        {
            Result<RegisteredUser> searchResult = FindUserByEmail(email, RegisteredUsers);
            if (searchResult.ExecStatus)
            {
                //User found
                return searchResult.Data.LogOut();
            }
            else
            {
                //No user if found using the given email
                return new Result<RegisteredUser>($"There is not user using this email:{email}\n", false, null);

            }
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
    }
}
