using System;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ExternalSystems;
using Terminal3.DataAccessLayer.DTOs;

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

        // For loading from database
        public ShoppingCart( String id , ConcurrentDictionary<String, ShoppingBag> shoppingBags , Double totalCartPrice)
        {
            Id = id;
            ShoppingBags =shoppingBags;
            TotalCartPrice = totalCartPrice;
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

            int paymentId = PaymentSystem.Pay(amount, paymentDetails);

            if (paymentId == -1)
            {
                return new Result<ShoppingCart>("Attempt to purchase the shopping cart failed due to error in payment details\n", true, null);

            }

            int deliverySuccess = DeliverySystem.Supply(deliveryDetails);
            if (deliverySuccess == -1)
            {
                IDictionary<String, Object> refundDetails = new Dictionary<String, Object>();
                refundDetails.Add("transaction_id", paymentId.ToString());
                int refundSuccess = PaymentSystem.CancelPay(refundDetails);
                if(refundSuccess == -1)
                    return new Result<ShoppingCart>("Attempt to purchase the shopping cart failed due to error in delivery details and refund failed\n", true, null);
                return new Result<ShoppingCart>("Attempt to purchase the shopping cart failed due to error in delivery details\n", true, null);
            }
            ShoppingCart copy = new ShoppingCart(this);
            return new Result<ShoppingCart>("", true, copy);
        }

        public DTO_ShoppingCart getDTO()
        {
            ConcurrentDictionary<string, DTO_ShoppingBag> bags = new ConcurrentDictionary<string, DTO_ShoppingBag>();
            foreach(var sb in this.ShoppingBags)
            {
                bags.TryAdd(sb.Key, sb.Value.getDTO()); 
            }
            return new DTO_ShoppingCart(this.Id, bags , this.TotalCartPrice);
        }
    }
}
