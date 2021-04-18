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
            Assert.True(sut.LogOut("test@gmail.com", "test123").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void LogOutWithoutLogin()
        {
            Assert.False(sut.LogOut("test@gmail.com", "test123").ExecStatus);
        }
    }
}
