using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Users.Tests
{
    public class UsersAndPermissionsFacadeTests
    {
        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData("tomer@gmail.com", "raz@gmail.com", true)]
        [InlineData("tomer@gmail.com", "tomer@gmail.com", false)]
        public void RegisterTest(String usedEmail, String requestedEmail, Boolean expectedResult)
        {
            UsersAndPermissionsFacade facade = new UsersAndPermissionsFacade();
            Assert.True(facade.Register(usedEmail, "123123").ExecStatus);
            Assert.Equal(facade.Register(requestedEmail, "123123").ExecStatus, expectedResult);
        }
    }
}