using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        Result<RegisteredUser> Register(String email, String password);
        Result<RegisteredUser> Login(String email, String password);
        Result<RegisteredUser> LogOut(String email);
        Result<RegisteredUser> AddSystemAdmin(String email);
        Result<RegisteredUser> RemoveSystemAdmin(String email);
    }
    public class StoresAndManagementInterface : IStoresAndManagementInterface
    {
        public Result<RegisteredUser> AddSystemAdmin(String email)
        {
            throw new NotImplementedException();
        }

        public Result<RegisteredUser> Login(String email, String password)
        {
            throw new NotImplementedException();
        }

        public Result<RegisteredUser> LogOut(String email)
        {
            throw new NotImplementedException();
        }

        public Result<RegisteredUser> Register(String email, String password)
        {
            throw new NotImplementedException();
        }

        public Result<RegisteredUser> RemoveSystemAdmin(String email)
        {
            throw new NotImplementedException();
        }
    }
}
