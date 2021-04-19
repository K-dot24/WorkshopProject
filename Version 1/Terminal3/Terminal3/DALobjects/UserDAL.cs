using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class UserDAL
    {
        //Properties
        public String Id { get; }
        public ShoppingCartDAL ShoppingCart { get; }

        //Constructor
        public UserDAL(String id,ShoppingCartDAL shoppingCart)
        {
            Id = id;
            ShoppingCart = shoppingCart;
        }
    }
}
