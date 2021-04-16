using System;
using Terminal3.DALobjects;
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

        public ShoppingCart(ShoppingCartDAL shoppingCart)
        {
            ShoppingCartId = shoppingCart.ShoppingCartId;
            ShoppingBags = new LinkedList<ShoppingBag>();
            foreach(ShoppingBagDAL shoppingBag in shoppingCart.ShoppingBags)
            {
                ShoppingBags.AddLast(new ShoppingBag(shoppingBag));
            }
        }
    }
}
