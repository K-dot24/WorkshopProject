using System;
using Terminal3.DALobjects;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ExternalSystems;

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

        public Result<ShoppingCartDAL> GetDAL()
        {
            LinkedList<ShoppingBagDAL> SBD = new LinkedList<ShoppingBagDAL>();
            foreach (var sb in ShoppingBags)
            {
                SBD.AddLast(sb.Value.GetDAL().Data);
            }
            return new Result<ShoppingCartDAL>("shopping cart DAL object", true, new ShoppingCartDAL(Id, SBD , TotalCartPrice));

        }

        public Result<Double> GetTotalShoppingCartPrice()
        {
            Double sum = 0;
            foreach (ShoppingBag bag in ShoppingBags.Values)
            {
                sum += bag.GetTotalPrice();
            }
            return new Result<double>($"Total shopping cart price calculated, price = {sum}", true, sum);
        }

        public Result<bool> AdheresToPolicy()
        {
            foreach(ShoppingBag bag in ShoppingBags.Values)
            {
                Result<bool> result = bag.AdheresToPolicy();
                if (!result.Data)
                    return result;
            }

            return new Result<bool>("All bags adhere to their respective store policy", true, true);
        }

        public Result<ShoppingCart> Purchase(IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            if (!AdheresToPolicy().Data)
                return new Result<ShoppingCart>("A bag in the Shopping cart doesn't adhere to it's respective store's policy", true, null);

            Double amount = GetTotalShoppingCartPrice().Data;

            bool paymentSuccess = PaymentSystem.Pay(amount, paymentDetails);

            if (!paymentSuccess)
            {
                return new Result<ShoppingCart>("Attempt to purchase the shopping cart failed due to error in payment details\n", true, null);

            }

            bool deliverySuccess = DeliverySystem.Deliver(deliveryDetails);
            if (!deliverySuccess)
            {
                PaymentSystem.CancelTransaction(paymentDetails);
                return new Result<ShoppingCart>("Attempt to purchase the shopping cart failed due to error in delivery details\n", true, null);
            }
            ShoppingCart copy = new ShoppingCart(this);
            return new Result<ShoppingCart>("", true, copy);
        }
    }
}
