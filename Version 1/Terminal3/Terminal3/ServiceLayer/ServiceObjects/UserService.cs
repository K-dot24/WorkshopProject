using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ServiceLayer.ServiceObjects
{
    public class UserService
    {
        //Properties
        public String Id { get; }
        public ShoppingCartService ShoppingCart { get; set; }

        //Constructor
        public UserService(String id,ShoppingCartService shoppingCart)
        {
            Id = id;
            ShoppingCart = shoppingCart;
        }
    }
}
