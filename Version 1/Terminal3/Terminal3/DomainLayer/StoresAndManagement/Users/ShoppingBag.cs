using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    // Association Class
    public class ShoppingBag
    {
        public User User { get; }
        public Store Store { get; }
        public LinkedList<Product> Products { get; }

        public ShoppingBag(User user , Store store)
        {
            this.User = user;
            this.Store = store;
            Products = new LinkedList<Product>();
        }

        public ShoppingBag(ShoppingBagDAL shoppingBagDAL)
        {
            this.User = Mapper.GetRegisteredUser((RegisteredUserDAL)shoppingBagDAL.User);    // TODO - check if only registered users shopping bag is saved
            this.Store = Mapper.GetStore(shoppingBagDAL.Store);
            this.Products = new LinkedList<Product>();
            foreach(ProductDAL product in shoppingBagDAL.Products)
            {
                Products.AddLast(Store.GetProduct(product).Data);
            }
        }

        public Result<ShoppingBagDAL> GetDAL()
        {
            RegisteredUserDAL user = (RegisteredUser)user.GetDAL().Data;
            StoreDAL store = store.GetDAL().Data;
            LinkedList<ProductDAL> products = new LinkedList<ProductDAL>();
            foreach(Product p in Products)
            {
                products.AddLast(p.GetDAL().Data);
            }

            return new Result<ShoppingBagDAL>("Shopping bag DAL object", true, new ShoppingBagDAL(user, store, products));
        }

    }
}
