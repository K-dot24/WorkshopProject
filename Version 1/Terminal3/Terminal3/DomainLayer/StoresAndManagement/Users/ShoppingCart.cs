using System;
using Terminal3.DALobjects;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class ShoppingCart
    {
        public string ShoppingCartId { get; }
        public ConcurrentDictionary<String, ShoppingBag> ShoppingBags { get; }  // <StoreID, ShoppingBag>

        public ShoppingCart()
        {
            ShoppingCartId = Service.GenerateId();
            ShoppingBags = new ConcurrentDictionary<string, ShoppingBag>();
        }

        public Result<ShoppingBag> GetShoppingBag(string storeID)
        {
            if (ShoppingBags.TryGetValue(storeID, out ShoppingBag sb))  // Check if shopping bag for store exists
            {
                return new Result<ShoppingBag>("Found shopping bag.\n", true, sb);
            }
            //else failed
            return new Result<ShoppingBag>($"Shopping bag not found for {storeID}.\n", false, null);
        }

        public Result<Boolean> AddShoppingBagToCart(ShoppingBag sb)
        {
            ShoppingBags.TryAdd(sb.Store.Id, sb);
            return new Result<Boolean>("Shopping bag added to cart.\n", true, true);
        }

        public Result<ShoppingCartDAL> GetDAL()
        {
            LinkedList<ShoppingBagDAL> SBD = new LinkedList<ShoppingBagDAL>();
            foreach (var sb in ShoppingBags)
            {
                SBD.AddLast(sb.Value.GetDAL().Data);
            }
            return new Result<ShoppingCartDAL>("shopping cart DAL object", true, new ShoppingCartDAL(ShoppingCartId, SBD));

        }
    }
}
