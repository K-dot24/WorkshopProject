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
            this.User = new RegisteredUser((RegisteredUserDAL)shoppingBagDAL.User);    // TODO - check if only registered users shopping bag is saved
            this.Store = new Store(shoppingBagDAL.Store);
            this.Products = new LinkedList<Product>();
            foreach(ProductDAL product in shoppingBagDAL.Products)
            {
                Products.AddLast(Store.GetProduct(product).Data);
            }
        }

    }
}
