using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class UserDAL
    {
        //Properties
        public ShoppingCartDAL ShoppingCart { get; }

        //Constructor
        public UserDAL(ShoppingCartDAL shoppingCart)
        {
            ShoppingCart = shoppingCart;
        }
    }
}
