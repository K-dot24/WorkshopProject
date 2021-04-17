using System;
using Terminal3.DALobjects;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class ShoppingBagDAL
    {
        //Properties
        public UserDAL User { get; }
        public StoreDAL Store { get; }
        public LinkedList<ProductDAL> Products { get; }


        //Constructor 
        public ShoppingBagDAL(UserDAL user, StoreDAL store, LinkedList<ProductDAL> products)
        {
            User = user;
            Store = store;
            Products = products;
        }


        

    }
}
