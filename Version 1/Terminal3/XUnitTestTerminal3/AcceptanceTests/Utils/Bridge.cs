using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;

namespace XUnitTestTerminal3.IntegrationTests
{
    public class Bridge
    {
        public static ISystemInterface getService()
        {
            SystemInterfaceProxy proxy = new SystemInterfaceProxy();
            // Uncomment when real application is ready
            //proxy.real(new IECommerceSystem());
            return proxy;
        }
    }
}
