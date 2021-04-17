using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class HistoryDAL
    {
        //Properties
        public LinkedList<ShoppingBagDAL> ShoppingBags { get; }

        //Constructor
        public HistoryDAL(LinkedList<ShoppingBagDAL> shoppingBags)
        {
            ShoppingBags = shoppingBags;
        }
    }
}
