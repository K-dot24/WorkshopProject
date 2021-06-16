using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTestTerminal3.IntegrationTests;

namespace XUnitTestTerminal3
{
    public class ResetSystemTest: XUnitTerminal3TestCase
    {
        private string user_id;
        private string admin_id;

        public ResetSystemTest() : base()
        {
            //Admin
            this.admin_id = sut.Login("Admin@terminal3.com", "Admin123").Data;

            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemAdmin()
        {
            Assert.True(sut.ResetSystem(admin_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemNotAdmin()
        {
            Assert.False(sut.ResetSystem(user_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemLogin()
        {
            sut.ResetSystem(admin_id);
            sut = Bridge.getService();

            Assert.False(sut.Login("test@gmail.com", "test123").ExecStatus);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void ResetSystemRegidter()
        {
            sut.ResetSystem(admin_id);
            sut = Bridge.getService();

            Assert.True(sut.Register("test@gmail.com", "test123").ExecStatus);
        }


    }
}
