using System;
using System.Collections.Generic;
using System.Threading;
using Terminal3.DALobjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ExternalSystems;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public abstract class User
    {
        public String Id { get;}
        public ShoppingCart ShoppingCart { get; set; }

        protected User()
        {
            Id = Service.GenerateId();
            ShoppingCart = new ShoppingCart();
        }

        public Result<bool> AddProductToCart(Product product, int productQuantity, Store store)
        {
            try
            {
                Monitor.TryEnter(product.Id);
                try
                {
                    ShoppingBag sb;
                    Result<Boolean> res;
                    Result<ShoppingBag> getSB = ShoppingCart.GetShoppingBag(store.Id);
                    if (getSB.ExecStatus)  // Check if shopping bag for store exists
                    {
                        sb = getSB.Data;
                        res = sb.AddProtuctToShoppingBag(product, productQuantity);
                        if (res.ExecStatus)
                        {
                            return new Result<bool>($"Product {product.Name} was added successfully to shopping cart.\n", true, true);
                        }
                        //else failed
                        return res;
                    }
                    //else create shopping bag for storeID
                    sb = new ShoppingBag(this, store);
                    res = sb.AddProtuctToShoppingBag(product, productQuantity);
                    if (res.ExecStatus)
                    {
                        ShoppingCart.AddShoppingBagToCart(sb);
                        return new Result<bool>($"Product {product.Name} was added successfully to cart.\n", true, true);
                    }
                    // else failed
                    return res;
                }
                finally
                {
                    Monitor.Exit(product.Id);
                }
            }
            catch (SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<bool>(SyncEx.Message, false, false);
            }
        }

        public Result<Boolean> UpdateShoppingCart(String storeID, Product product, int quantity)
        {
            Result<ShoppingBag> resBag = ShoppingCart.GetShoppingBag(storeID);
            if (resBag.ExecStatus)
            {
                ShoppingBag bag = resBag.Data;
                Result<bool> res = bag.UpdateShoppingBag(product, quantity);
                if (!bag.Products.ContainsKey(product))                   
                    ShoppingCart.ShoppingBags.TryRemove(storeID, out _);
                return res;
            }
            //else faild
            return new Result<bool>(resBag.Message, false, false);
        }

        public Result<ShoppingCart> GetUserShoppingCart()
        {
            return new Result<ShoppingCart>("User shopping cart\n", true, ShoppingCart);
        }

        public Result<ShoppingCart> Purchase(IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            if (ShoppingCart.ShoppingBags.IsEmpty)
            {
                return new Result<ShoppingCart>("The shopping cart is empty\n", false, null);
            }

            if (!isValidCartQuantity())
            {
                return new Result<ShoppingCart>("Notice - The store is out of stock\n", false, null);   // TODO - do we want to reduce the products from the bag (i think not) and do we want to inform which of the products are out of stock ?
            }

            Result<ShoppingCart> result = ShoppingCart.Purchase(paymentDetails, deliveryDetails);
            if(result.Data != null)
                ShoppingCart = new ShoppingCart();              // create new shopping cart for user

            return result;
        }

        private Boolean isValidCartQuantity()
        {
            ConcurrentDictionary<String, ShoppingBag> ShoppingBags = ShoppingCart.ShoppingBags;

            foreach (var bag in ShoppingBags)
            {
                ConcurrentDictionary<Product, int> Products = bag.Value.Products;

                foreach (var product in Products)
                {
                    if (product.Key.Quantity < product.Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Result<UserDAL> GetDAL()
        {
            ShoppingCartDAL shoppingCart = ShoppingCart.GetDAL().Data;
            return new Result<UserDAL>("User DAL object", true, new UserDAL(Id,shoppingCart));
        }



    }
}
