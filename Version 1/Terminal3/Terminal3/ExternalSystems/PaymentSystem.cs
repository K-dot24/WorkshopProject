using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems
{
    //Mock for payment system
    public static class PaymentSystem
    {
        public static int Pay(double amount , IDictionary<String, Object> paymentDetails)
        {
            if (ExternalSystemsAPI.getInstance().Handshake())
            {                
                String result = ExternalSystemsAPI.getInstance().Pay(paymentDetails);
                if(Int32.TryParse(result, out int id))
                {
                    return id;
                }
            }
            return -1;
        }

        public static int CancelPay(IDictionary<String, Object> paymentDetails)
        {
            // Users transaction is canceled
            if (ExternalSystemsAPI.getInstance().Handshake())
            {
                String result = ExternalSystemsAPI.getInstance().CancelPay(paymentDetails);
                if (Int32.TryParse(result, out int id))
                {
                    return id;
                }
            }
            return -1;
        }
    }
}
