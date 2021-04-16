using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Xunit;

namespace XUnitTestTerminal3
{
    public class LogOutTest: XUnitTerminal3TestCase
    {
        public LogOutTest()
        {
            sut.ResetSystem();
            sut.Register("test@gmail.com", "test123");
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LogOut()
        {            
            sut.Login("test@gmail.com", "test123");
            Result<Object> res = sut.LogOut("test@gmail.com", "test123");
            Assert.True(res.ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LogOutWithoutLogin()
        {
            Result<Object> res = sut.LogOut("test@gmail.com", "test123");
            Assert.False(res.ExecStatus);
        }
    }
}
