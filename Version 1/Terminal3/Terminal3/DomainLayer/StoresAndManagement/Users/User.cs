using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ExternalSystems;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public abstract class User
    {
        public String Id { get;}
        public ShoppingCart ShoppingCart { get; set; }
        public List<Offer> PendingOffers { get; set; }
        public List<Offer> AcceptedOffers { get; set; }
        protected User()
        {
            Id = Service.GenerateId();
            ShoppingCart = new ShoppingCart();
            PendingOffers = new List<Offer>();
            AcceptedOffers = new List<Offer>();
        }
        protected User(string id)
        {
            if (id.Equals("-1"))
                Id = Service.GenerateId();
            else Id = id;
            ShoppingCart = new ShoppingCart();
            PendingOffers = new List<Offer>();
            AcceptedOffers = new List<Offer>();
        }
        protected User(String id , ShoppingCart shoppingCart)
        {
            Id = id;
            ShoppingCart = shoppingCart;
            PendingOffers = new List<Offer>();
            AcceptedOffers = new List<Offer>();
        }


        public Result<ShoppingCart> AddProductToCart(Product product, int productQuantity, Store store)
        {
            try
            {
                Monitor.Enter(product);
                try
                {
                    ShoppingBag sb;
                    Result<Boolean> res;
                    Result<ShoppingBag> getSB = ShoppingCart.GetShoppingBag(store.Id);
                    if (getSB.ExecStatus)  // Check if shopping bag for store exists
                    {
                        sb = getSB.Data;
                        res = sb.AddProtuctToShoppingBag(product, productQuantity, AcceptedOffers);
                        if (res.ExecStatus)
                        {
                            return new Result<ShoppingCart>($"Product {product.Name} was added successfully to shopping cart.\n", true, ShoppingCart);
                        }
                        //else failed
                        return new Result<ShoppingCart>(res.Message, res.ExecStatus, null);
                    }
                    //else create shopping bag for storeID
                    sb = new ShoppingBag(this, store);
                    res = sb.AddProtuctToShoppingBag(product, productQuantity, AcceptedOffers);
                    if (res.ExecStatus)
                    {
                        ShoppingCart.AddShoppingBagToCart(sb);
                        return new Result<ShoppingCart>($"Product {product.Name} was added successfully to cart.\n", true, ShoppingCart);
                    }
                    // else failed
                    return new Result<ShoppingCart>(res.Message, res.ExecStatus, null);
                }
                finally
                {
                    Monitor.Exit(product);
                }
            }
            catch (SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<ShoppingCart>(SyncEx.Message, false, null);
            }
        }

        public Result<ShoppingCart> UpdateShoppingCart(String storeID, Product product, int quantity)
        {
            Result<ShoppingBag> resBag = ShoppingCart.GetShoppingBag(storeID);
            if (resBag.ExecStatus)
            {
                ShoppingBag bag = resBag.Data;
                Result<ShoppingBag> res = bag.UpdateShoppingBag(product, quantity);
                if (!bag.Products.ContainsKey(product))                   
                    ShoppingCart.ShoppingBags.TryRemove(storeID, out _);
                return new Result<ShoppingCart>(res.Message, res.ExecStatus, ShoppingCart);
            }
            //else faild
            return new Result<ShoppingCart>(resBag.Message, false, null);
        }

        public Result<ShoppingCart> GetUserShoppingCart()
        {
            return new Result<ShoppingCart>("User shopping cart\n", true, ShoppingCart);
        }

        public Result<ShoppingCart> Purchase(IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails , MongoDB.Driver.IClientSessionHandle session = null)
        {            
            if (ShoppingCart.ShoppingBags.IsEmpty)
            {
                return new Result<ShoppingCart>("The shopping cart is empty\n", false, null);
            }

            if (!isValidCartQuantity())
            {
                return new Result<ShoppingCart>("Notice - The store is out of stock\n", false, null);   // TODO - do we want to reduce the products from the bag (i think not) and do we want to inform which of the products are out of stock ?
            }

            Result<ShoppingCart> result = ShoppingCart.Purchase(paymentDetails, deliveryDetails , AcceptedOffers, session);
            if(result.Data != null)
                ShoppingCart = new ShoppingCart();              // create new shopping cart for user

            Result<bool> removeAccatedOffersResult = removeAcceptedOffers();
            if (!removeAccatedOffersResult.ExecStatus)
                return new Result<ShoppingCart>("The purchase failed because the system failed to remove accepted offers", false, null);

            return result;
        }
     
        private Boolean isValidCartQuantity()
        {
            ConcurrentDictionary<String, ShoppingBag> ShoppingBags = ShoppingCart.ShoppingBags;

            foreach(var bag in ShoppingBags)
            {
                ConcurrentDictionary<Product, int> Products = bag.Value.Products;

                foreach(var product in Products)
                {
                    if(product.Key.Quantity < product.Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<Offer> getAcceptedOffers()
        {
            return AcceptedOffers;
        }

        public Result<bool> removeAcceptedOffers()
        {
            AcceptedOffers = new List<Offer>();
            //TODO mapper? Zoe

            return new Result<bool>("", true, true);
        }

        public Result<Offer> SendOfferToStore(string storeID, string productID, int amount, double price)
        {
            Offer offer = new Offer(this.Id, productID, amount, price, storeID);
            PendingOffers.Add(offer);
            //TODO mapper Zoe
            return new Result<Offer>("", true, offer);
        }

        protected Offer findPendingOffer(string id)
        {
            foreach (Offer offer in PendingOffers)
                if (offer.Id == id)
                    return offer;
            return null;
        }

        public Result<bool> AnswerCounterOffer(string offerID, bool accepted)
        {
            Offer offer = findPendingOffer(offerID);
            if (offer == null)
                return new Result<bool>("Failed to respond to a counter offer: Failed to locate the offer", false, false);
            if (offer.CounterOffer == -1)
                return new Result<bool>("Failed to respond to a counter offer: The offer is not a counter offer", false, false);
            if(accepted)
                return MovePendingOfferToAccepted(offerID);
            return RemovePendingOffer(offerID);
        }

        public Result<bool> RemovePendingOffer(string id)
        {
            Offer offer = findPendingOffer(id);
            if (offer == null)
                return new Result<bool>("Failed to remove offer from user: Failed to locate the offer", false, false);
            PendingOffers.Remove(offer);
            //TODO mapper Zoe
            return new Result<bool>("", true, true);
        }

        public Result<bool> MovePendingOfferToAccepted(string id)
        {
            Offer offer = findPendingOffer(id);
            if (offer == null)
                return new Result<bool>("Failed to move an offer from pending to accepted: Failed to locate the offer", false, false);
            Result<bool> removeResult = RemovePendingOffer(id);
            if (!removeResult.ExecStatus)
                return removeResult;
            AcceptedOffers.Add(offer);
            //TODO mapper? Zoe
            return new Result<bool>("", true, true);
        }

        public abstract Result<bool> AcceptOffer(string offerID);

        public abstract Result<bool> DeclineOffer(string offerID);

        public abstract Result<bool> CounterOffer(string offerID);

        public Result<List<Dictionary<string, object>>> getUserPendingOffers()
        {
            //TODO: Mapper load Zoe
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (Offer offer in PendingOffers)
                list.Add(offer.GetData());
            return new Result<List<Dictionary<string, object>>>("", true, list);
        }

        public Result<UserService> GetDAL()
        {
            ShoppingCartService shoppingCart = ShoppingCart.GetDAL().Data;
            return new Result<UserService>("User DAL object", true, new UserService(Id,shoppingCart));
        }



    }
}
