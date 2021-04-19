using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Xunit;

namespace XUnitTestTerminal3
{
    public class LoginTest: XUnitTerminal3TestCase
    {
        public LoginTest() : base() {}

        [Fact]
        [Trait("Category", "acceptance")]
        public void Login()
        {
            sut.Register("test@gmail.com", "test123");
            Assert.True(sut.Login("test@gmail.com", "test123").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LoginIncorrectPass()
        {
            sut.Register("test@gmail.com", "test123");
            Assert.False(sut.Login("test@gmail.com", "worng_pass").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LoginNotRegister()
        {
            Assert.False(sut.Login("test@gmail.com", "worng_pass").ExecStatus);
        }
    }
}
