using System;
using Terminal3.DALobjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public abstract class User
    {
        public String UserId { get; }
        public ShoppingCart ShoppingCart { get; }

        protected User()
        {
            UserId = Service.GenerateId();
            ShoppingCart = new ShoppingCart();
        }

        public Result<bool> AddProductToCart(Product product, int productQuantity, Store store)
        {
            ShoppingBag sb;
            Result<ShoppingBag> getSB = ShoppingCart.GetShoppingBag(store.Id);
            if (getSB.ExecStatus)  // Check if shopping bag for store exists
            {
                sb = getSB.Data;
                sb.AddProtuctToShoppingBag(product, productQuantity);
                return new Result<bool>($"Product {product.Name} was added successfully to shopping cart.\n", true, true);
            }
            //else create shopping bag for storeID
            sb = new ShoppingBag(this, store);
            sb.AddProtuctToShoppingBag(product, productQuantity);
            ShoppingCart.AddShoppingBagToCart(sb);
            return new Result<bool>($"Product {product.Name} was added successfully to cart.\n", true, true);
        }

        protected Result<UserDAL> GetDAL()
        {
            ShoppingCartDAL shoppingCart = ShoppingCart.GetDAL().Data;
            return new Result<UserDAL>("User DAL object", true, new UserDAL(shoppingCart));
        }

    }
}
