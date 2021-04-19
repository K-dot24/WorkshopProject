using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems
{
    //Mock for delivery system
    public static class DeliverySystem
    {
        public static Boolean Deliver(IDictionary<String, Object> deliveryDetails)
        {
            return true;
        }
    }
}
