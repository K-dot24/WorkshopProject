using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;
using XUnitTestTerminal3.AcceptanceTests.Utils;

namespace XUnitTestTerminal3.IntegrationTests
{
    public class Bridge
    {
        public static ISystemInterface getService()
        {
            SystemInterfaceProxy proxy = new SystemInterfaceProxy();
            // Uncomment when real application is ready
            proxy.Real = new RealAdapter();
            return proxy;
        }
    }
}
