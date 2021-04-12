using System;
using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class ShoppingCart
    {
        public string ShoppingCartId { get; }
        public bool Active { get; }
        public LinkedList<ShoppingBag> ShoppingBags { get; }

        public ShoppingCart()
        {
            ShoppingCartId = Service.GenerateId();
            Active = true;
            ShoppingBags = new LinkedList<ShoppingBag>();
        }
    }
}
