using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;

namespace XUnitTestTerminal3.IntegrationTests
{
    public class Bridge
    {
        public static IECommerceSystemInterface getService()
        {
            IECommerceSystemProxy proxy = new IECommerceSystemProxy();
            // Uncomment when real application is ready
            //proxy.real(new IECommerceSystem());
            return proxy;
        }
    }
}
