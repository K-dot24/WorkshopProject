
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public interface IUsersAndPermissionsFacade
    {
    }

    public class UsersAndPermissionsFacade : IUsersAndPermissionsFacade
    {
        LinkedList<RegisteredUser> registeredUsers;

        public UsersAndPermissionsFacade()
        {
            this.registeredUsers = new LinkedList<RegisteredUser>();
        }

        Result<Object> Register(String email, String password)
        {
            if (isUniqueEmail(email)) 
            {
                RegisteredUser newUser = new RegisteredUser(email, password);
                this.registeredUsers.AddLast(newUser);
                return new Result<Object>($"{email} is registered as new user", true, newUser);
            }
        }

        private bool isUniqueEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
