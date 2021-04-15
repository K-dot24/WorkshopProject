using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Users.Tests
{
    public class UsersAndPermissionsFacadeTests
    {
        //Properties
        public UsersAndPermissionsFacade Facade { get; }

        //Constructor
        public UsersAndPermissionsFacadeTests()
        {
            Facade = new UsersAndPermissionsFacade();
        }

        //Tests
        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "raz@gmail.com", true)]
        [InlineData("tomer@gmail.com", "tomer@gmail.com", false)]
        public void RegisterTest(String usedEmail, String requestedEmail, Boolean expectedResult)
        {
            Assert.True(Facade.Register(usedEmail, "password").ExecStatus);
            Assert.Equal(expectedResult, Facade.Register(requestedEmail, "password").ExecStatus);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "tomer@gmail.com", true)]
        [InlineData("tomer@gmail.com", "raz@gmail.com", false)]
        //System admin need to be registered user
        public void AddSystemAdminTest(String registeredEmail, String adminEmail, Boolean expectedResult)
        {
            Facade.Register(registeredEmail, "password");
            Assert.Equal(expectedResult, Facade.AddSystemAdmin(adminEmail).ExecStatus);
        }

        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "raz@gmail.com", "raz@gmail.com", true)]  //user is exist in SystemAdmins
        [InlineData("tomer@gmail.com", "raz@gmail.com", "tomer@gmail.com", true)] //user is exist in SystemAdmins
        [InlineData("tomer@gmail.com", "raz@gmail.com", "shaked@gmail.com", false)] //user is NOT exist in SystemAdmins
        public void RemoveSystemAdminTest(String admin1, String admin2, String adminToRemove, Boolean expectedResult)
        {
            Facade.Register(admin1, "password");
            Facade.Register(admin2, "password");
            Facade.AddSystemAdmin(admin1);
            Facade.AddSystemAdmin(admin2);
            Assert.Equal(expectedResult, Facade.RemoveSystemAdmin(adminToRemove).ExecStatus);

        }

        [Fact()]
        [Trait("Category", "Unit")]
        //at least one system admin in the system test
        public void RemoveSystemAdminTestAtLeastOne()
        {
            String email = "tomer@gmail.com";
            Facade.Register(email, "password");
            Facade.AddSystemAdmin(email);
            Assert.False(Facade.RemoveSystemAdmin(email).ExecStatus);

        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void LoginTest()
        {
            String email = "tomer@gmail.com";
            String password = "password";
            Facade.Register(email, password);
            Assert.True(Facade.Login(email, password).ExecStatus, "Fail to login");
            Assert.False(Facade.Login(email, password).ExecStatus, "Able to loggin twice");
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void LogOutTest()
        {
            String email = "tomer@gmail.com";
            String password = "password";
            Facade.Register(email, password);
            Assert.False(Facade.LogOut(email).ExecStatus, "Able to loggout twice");
            Assert.True(Facade.Login(email, password).ExecStatus, "Not able to log in");
            Assert.True(Facade.LogOut(email).ExecStatus, "Not able to log out");
        }
    }
}