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

        //TODO: Fix DAL

       /* public ShoppingCart(ShoppingCartDAL shoppingCart)
        {
            ShoppingCartId = shoppingCart.ShoppingCartId;
            ShoppingBags = new LinkedList<ShoppingBag>();
            foreach(ShoppingBagDAL shoppingBag in shoppingCart.ShoppingBags)
            {
                ShoppingBags.AddLast(new ShoppingBag(shoppingBag));
            }
        }

        public Result<ShoppingCartDAL> GetDAL()
        {
            LinkedList<ShoppingBagDAL> SBD = new LinkedList<ShoppingBagDAL>();
            foreach(ShoppingBag sb in ShoppingBags)
            {
                SBD.AddLast(sb.GetDAL().Data);
            }
            return new Result<ShoppingCartDAL>("shopping cart DAL object", true, new ShoppingCartDAL(ShoppingCartId, SBD));

        }*/
    }
}
