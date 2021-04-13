using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;


namespace XUnitTestTerminal3.IntegrationTests
{
    public class IECommerceSystemProxy : IECommerceSystemInterface
    {
        private IECommerceSystemInterface real;

        public IECommerceSystemInterface Real { get => real; set => real = value; }
    }
}
