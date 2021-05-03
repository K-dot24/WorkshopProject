using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class ShoppingCartService
    {
        //Properties
        public String Id { get; }
        public LinkedList<ShoppingBagService> ShoppingBags { get; }
        public Double TotalCartPrice { get; }


        //Constructor
        public ShoppingCartService(string shoppingCartId, LinkedList<ShoppingBagService> shoppingBags , Double totalCartPrice)
        {
            Id = shoppingCartId;
            ShoppingBags = shoppingBags;
            TotalCartPrice = totalCartPrice;
        }
    }
}
