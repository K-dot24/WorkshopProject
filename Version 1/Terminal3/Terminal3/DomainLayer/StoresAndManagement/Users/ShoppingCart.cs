using System;
using Terminal3.DALobjects;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class ShoppingCart
    {
        public string Id { get; set; }
        public ConcurrentDictionary<String, ShoppingBag> ShoppingBags { get; set; }  // <StoreID, ShoppingBag>
        public Double TotalCartPrice { get; set; }

        public ShoppingCart()
        {
            Id = Service.GenerateId();
            ShoppingBags = new ConcurrentDictionary<string, ShoppingBag>();
            TotalCartPrice = 0;
        }

        public ShoppingCart(ShoppingCart original)
        {
            Id = original.Id;
            ShoppingBags = original.ShoppingBags;
            TotalCartPrice = original.TotalCartPrice;
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

        public Result<ShoppingCartService> GetDAL()
        {
            LinkedList<ShoppingBagService> SBD = new LinkedList<ShoppingBagService>();
            foreach (var sb in ShoppingBags)
            {
                SBD.AddLast(sb.Value.GetDAL().Data);
            }
            return new Result<ShoppingCartService>("shopping cart DAL object", true, new ShoppingCartService(Id, SBD , TotalCartPrice));

        }

        public Double GetTotalShoppingCartPrice()
        {
            Double sum=0;
            foreach(ShoppingBag bag in ShoppingBags.Values)
            {
                sum = sum + bag.GetTotalPrice();
            }
            TotalCartPrice = sum;
            return sum;
        }
    }
}
