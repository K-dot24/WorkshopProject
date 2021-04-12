using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagment.Users
{
    class ShoppingCart
    {
        private String ShoppingCartID;
        private Boolean active;
        private LinkedList<ShoppingBag> shoppingBags;

        public ShoppingCart()
        {
            ShoppingCartID = Service.GenerateID();
            active = true;
            shoppingBags = new LinkedList<ShoppingBag>();
        }
    }
}
