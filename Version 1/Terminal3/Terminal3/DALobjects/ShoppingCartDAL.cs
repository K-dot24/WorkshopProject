using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class ShoppingCartDAL
    {
        //Properties
        public String Id { get; }
        public LinkedList<ShoppingBagDAL> ShoppingBags { get; }
        public Double TotalCartPrice { get; }


        //Constructor
        public ShoppingCartDAL(string shoppingCartId, LinkedList<ShoppingBagDAL> shoppingBags , Double totalCartPrice)
        {
            Id = shoppingCartId;
            ShoppingBags = shoppingBags;
            TotalCartPrice = totalCartPrice;
        }
    }
}
