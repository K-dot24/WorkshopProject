using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagment.Users
{
    public abstract class User
    {
        private History history;
        private ShoppingCart shoppingCart;

        public User()
        {
            history = new History();
            shoppingCart = new ShoppingCart();
        }
        
    }
}
