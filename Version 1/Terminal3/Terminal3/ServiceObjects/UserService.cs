using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class UserService
    {
        //Properties
        public String Id { get; }
        public ShoppingCartService ShoppingCart { get; }

        //Constructor
        public UserService(String id,ShoppingCartService shoppingCart)
        {
            Id = id;
            ShoppingCart = shoppingCart;
        }
    }
}
