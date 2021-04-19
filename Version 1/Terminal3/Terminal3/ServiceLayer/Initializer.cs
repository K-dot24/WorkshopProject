using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;

namespace Terminal3.ServiceLayer
{
    public class Initializer
    {
        ECommerceSystem System;

        public Initializer()
        {
            System = new ECommerceSystem();
            System.DisplaySystem();
        }         

    }
}
