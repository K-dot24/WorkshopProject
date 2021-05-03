using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class HistoryService
    {
        //Properties
        public LinkedList<ShoppingBagService> ShoppingBags { get; }

        //Constructor
        public HistoryService(LinkedList<ShoppingBagService> shoppingBags)
        {
            ShoppingBags = shoppingBags;
        }
    }
}
