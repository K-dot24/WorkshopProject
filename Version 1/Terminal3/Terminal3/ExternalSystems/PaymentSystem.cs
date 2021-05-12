using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems
{
    //Mock for payment system
    public static class PaymentSystem
    {
        public static bool Pay(double amount , IDictionary<String, Object> paymentDetails)
        {
            //bool[] bools = { true, false };
            //return bools[new Random().Next(0, 2)];
            return paymentDetails.Count != 0;
        }

        public static void CancelTransaction(IDictionary<String, Object> paymentDetails)
        {
            // Users transaction is canceled

        }
    }
}
