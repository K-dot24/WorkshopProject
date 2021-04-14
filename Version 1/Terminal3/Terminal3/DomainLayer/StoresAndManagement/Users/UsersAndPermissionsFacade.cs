
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public interface IUsersAndPermissionsFacade
    {
        Result<RegisteredUser> Register(String email, String password);
        bool isUniqueEmail(string email);
    }

    public class UsersAndPermissionsFacade : IUsersAndPermissionsFacade
    {
        LinkedList<RegisteredUser> registeredUsers;

        public UsersAndPermissionsFacade()
        {
            this.registeredUsers = new LinkedList<RegisteredUser>();
        }

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
                this.registeredUsers.AddLast(newUser);
                return new Result<RegisteredUser>($"{email} is registered as new user", true, newUser);
            }
            else {
                return new Result<RegisteredUser>($"{email} is aleady in user\n Please use different email", false,null);
            }
        }

        /// <summary>
        /// util function in order to check the the given email is not in use by other user
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// boolean wwther the email is unique or not
        /// </returns>
        public bool isUniqueEmail(string email)
        {
            foreach (RegisteredUser user in registeredUsers)
            {
                if (user.Email.Equals(email)) { return false; }
            }
            return true;
        }
    }
}
