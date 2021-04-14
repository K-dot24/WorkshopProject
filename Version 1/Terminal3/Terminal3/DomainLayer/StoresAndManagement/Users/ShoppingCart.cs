using System;
using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class ShoppingCart
    {
        public string ShoppingCartId { get; }
        public LinkedList<ShoppingBag> ShoppingBags { get; }

        public ShoppingCart()
        {
            ShoppingCartId = Service.GenerateId();
            ShoppingBags = new LinkedList<ShoppingBag>();
        }
    }
}
