using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class ShoppingCartDAL
    {
        //Properties
        public string ShoppingCartId { get; }
        public LinkedList<ShoppingBagDAL> ShoppingBags { get; }

        public ShoppingCartDAL(string shoppingCartId, LinkedList<ShoppingBagDAL> shoppingBags)
        {
            ShoppingCartId = shoppingCartId;
            ShoppingBags = shoppingBags;
        }
    }
}
