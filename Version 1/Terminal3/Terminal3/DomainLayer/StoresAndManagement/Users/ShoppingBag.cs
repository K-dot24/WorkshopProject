using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

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

    }
}
