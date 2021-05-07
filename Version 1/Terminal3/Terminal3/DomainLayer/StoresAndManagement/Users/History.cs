using System.Collections.Generic;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Concurrent;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class History
    {
        public LinkedList<ShoppingBagService> ShoppingBags { get; }

        public History()
        {
            ShoppingBags = new LinkedList<ShoppingBagService>();
        }

        public History(LinkedList<ShoppingBagService> shoppingBags)
        {
            this.ShoppingBags = shoppingBags;
        }


        public void AddPurchasedShoppingCart(ShoppingCart shoppingCart)
        {
            ConcurrentDictionary<String, ShoppingBag> bags = shoppingCart.ShoppingBags;

            foreach (var bag in bags)
            {
                ShoppingBags.AddLast(bag.Value.GetDAL().Data);
            }
        }

        public void AddPurchasedShoppingBag(ShoppingBag shoppingBag)
        {
            ShoppingBags.AddLast(shoppingBag.GetDAL().Data);
        }

        public Result<HistoryService> GetDAL()
        {
            return new Result<HistoryService>("History DAL object", true, new HistoryService(ShoppingBags));
        }
    }
}
