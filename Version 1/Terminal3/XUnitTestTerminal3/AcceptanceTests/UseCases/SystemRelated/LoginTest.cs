using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Xunit;

namespace XUnitTestTerminal3
{
    public class LoginTest: XUnitTerminal3TestCase
    {
        public LoginTest()
        {
            sut.ResetSystem();
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void Login()
        {
            sut.Register("test@gmail.com", "test123");
            Result<Object> res = sut.Login("test@gmail.com", "test123");
            Assert.True(res.ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LoginIncorrectPass()
        {
            sut.Register("test@gmail.com", "test123");
            Result<Object> res = sut.Login("test@gmail.com", "worng_pass");
            Assert.False(res.ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LoginNotRegister()
        {
            Result<Object> res = sut.Login("test@gmail.com", "worng_pass");
            Assert.False(res.ExecStatus);
        }
    }
}
