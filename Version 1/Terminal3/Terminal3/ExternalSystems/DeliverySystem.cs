using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems
{
    //Mock for delivery system
    public static class DeliverySystem
    {
        public static int Supply(IDictionary<String, Object> deliveryDetails)
        {
            if (ExternalSystemsAPI.getInstance().Handshake())
            {
                String result = ExternalSystemsAPI.getInstance().Supply(deliveryDetails);
                if (Int32.TryParse(result, out int id))
                {
                    return id;
                }
            }
            return -1;
        }

        public static int CancelSupply(IDictionary<String, Object> deliveryDetails)
        {
            if (ExternalSystemsAPI.getInstance().Handshake())
            {
                String result = ExternalSystemsAPI.getInstance().CancelSupply(deliveryDetails);
                if (Int32.TryParse(result, out int id))
                {
                    return id;
                }
            }
            return -1;
        }


    }
}
