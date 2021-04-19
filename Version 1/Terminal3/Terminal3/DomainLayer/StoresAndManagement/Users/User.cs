using System;
using Terminal3.DALobjects;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public abstract class User
    {
        public String Id { get; }
        public ShoppingCart ShoppingCart { get; }

        protected User()
        {
            Id = Service.GenerateId();
            ShoppingCart = new ShoppingCart();
        }

        public Result<bool> AddProductToCart(Product product, int productQuantity, Store store)
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

        public Result<Boolean> UpdateShoppingCart(String storeID, Product product, int quantity)
        {
            Result<ShoppingBag> resBag = ShoppingCart.GetShoppingBag(storeID);
            if (resBag.ExecStatus)
            {
                ShoppingBag bag = resBag.Data;
                return bag.UpdateShoppingBag(product, quantity);
            }
            //else faild
            return new Result<bool>(resBag.Message, false, false);
        }

        public Result<ShoppingCart> GetUserShoppingCart()
        {
            return new Result<ShoppingCart>("User shopping cart\n", true, ShoppingCart);
        }

        public Result<UserDAL> GetDAL()
        {
            ShoppingCartDAL shoppingCart = ShoppingCart.GetDAL().Data;
            return new Result<UserDAL>("User DAL object", true, new UserDAL(Id,shoppingCart));
        }

    }
}
