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

        public Result<ShoppingCartDAL> GetDAL()
        {
            LinkedList<ShoppingBagDAL> SBD = new LinkedList<ShoppingBagDAL>();
            foreach(ShoppingBag sb in ShoppingBags)
            {
                SBD.AddLast(sb.GetDAL().Data);
            }
            return new Result<ShoppingCartDAL>("shopping cart DAL object", true, new ShoppingCartDAL(ShoppingCartId, SBD));

        }
    }
}
