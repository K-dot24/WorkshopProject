﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ExternalSystems
{
    //Mock for payment system
    public static class PaymentSystem
    {
        public static bool Pay(string userID, double amount)
        {
            bool[] bools = { true, false };
            return bools[new Random().Next(0, 2)];
        }
    }
}