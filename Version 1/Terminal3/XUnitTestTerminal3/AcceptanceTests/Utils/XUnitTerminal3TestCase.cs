using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Terminal3.ServiceLayer;

namespace XUnitTestTerminal3
{
    public abstract class XUnitTerminal3TestCase
    {
        private readonly IECommerceSystemInterface _sut = Bridge.getService();   
        

    }
}
