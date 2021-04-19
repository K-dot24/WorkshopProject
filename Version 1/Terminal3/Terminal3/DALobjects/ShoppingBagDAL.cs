using System;
using Terminal3.DALobjects;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Terminal3.DALobjects
{
    public class ShoppingBagDAL
    {
        //Properties
        public String Id { get; }
        public String UserId { get; }
        public String StoreId { get; }
        public ConcurrentDictionary<ProductDAL , int> Products { get; }  //<productDAL , quantity>
        public Double TotalBagPrice { get; }


        //Constructor 
        public ShoppingBagDAL(String bagID , String userID, String storeID, ConcurrentDictionary<ProductDAL, int> products ,Double totalBagPrice )
        {
            Id = bagID;
            UserId = userID;
            StoreId = storeID;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }


        

    }
}
