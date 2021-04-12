using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagment.Stores;

namespace Terminal3.DomainLayer.StoresAndManagment.Users
{
    // Assosiaction Class
    class ShoppingBag
    {
        private User user;
        private Store store;
        private LinkedList<Product> products;

        public ShoppingBag(User user , Store store)
        {
            this.user = user;
            this.store = store;
            products = new LinkedList<Product>();
        }

    }
}
