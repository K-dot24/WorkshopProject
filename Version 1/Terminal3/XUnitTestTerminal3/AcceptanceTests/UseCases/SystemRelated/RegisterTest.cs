using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Xunit;

namespace XUnitTestTerminal3
{
    public class RegisterTest: XUnitTerminal3TestCase
    {     
        public RegisterTest()
        {
            sut.ResetSystem();
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void Register()
        {
            sut.Register("test@gmail.com", "test123");
            Result<Object> res = sut.Login("test@gmail.com", "test123");
            Assert.True(res.ExecStatus);
        }


        [Fact]
        [Trait("Category", "acceptance")]
        public void RegisterDupplicateName()
        {
            sut.Register("test@gmail.com", "test_1");  //true
            sut.Register("test@gmail.com", "test_2");  //false - same email 

            Result<Object> res = sut.Login("test@gmail.com", "test_2");
            Assert.False(res.ExecStatus);
        }
    }
}
