using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagment.Users
{
    class History
    {
        private LinkedList<ShoppingBag> shoppingBags;

        public History(LinkedList<ShoppingBag> shoppingBags)
        {
            this.shoppingBags = shoppingBags;
        }
    }
}
