﻿using System.Collections.Generic;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class History
    {
        public LinkedList<ShoppingBag> ShoppingBags { get; }

        public History(LinkedList<ShoppingBag> shoppingBags)
        {
            this.ShoppingBags = shoppingBags;
        }

        public History()
        {
            throw new System.NotImplementedException();
        }

        public Result<HistoryDAL> GetDAL()
        {
            return new Result<HistoryDAL>("History DAL object", true, new HistoryDAL(this.ShoppingBagsDAL));
        }
    }
}
