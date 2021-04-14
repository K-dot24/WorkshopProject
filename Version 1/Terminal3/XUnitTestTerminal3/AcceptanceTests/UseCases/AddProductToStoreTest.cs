using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Xunit; 


namespace XUnitTestTerminal3
{
    public class AddProductToStoreTest : XUnitTerminal3TestCase
    {

        [Fact]
        [Trait("Add Product to Store", "Version 1")]
        public void SerchProductNotExist()
        {
            sut.ResetSystem();
            Result<Object> res = sut.AddProductToStore();
            Assert.False(res.ExecStatus);
        }
    }
}
