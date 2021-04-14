using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Terminal3.ServiceLayer;
using XUnitTestTerminal3.IntegrationTests;

namespace XUnitTestTerminal3
{
    public abstract class XUnitTerminal3TestCase
    {
        protected IECommerceSystemInterface sut = Bridge.getService();         

    }
}
