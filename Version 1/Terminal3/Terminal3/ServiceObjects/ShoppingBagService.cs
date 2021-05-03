using System;
using Terminal3.DALobjects;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Terminal3.DALobjects
{
    public class ShoppingBagService
    {
        //Properties
        public String Id { get; }
        public String UserId { get; }
        public String StoreId { get; }
        public ConcurrentDictionary<ProductService , int> Products { get; }  //<productDAL , quantity>
        public Double TotalBagPrice { get; }


        //Constructor 
        public ShoppingBagService(String bagID , String userID, String storeID, ConcurrentDictionary<ProductService, int> products ,Double totalBagPrice )
        {
            Id = bagID;
            UserId = userID;
            StoreId = storeID;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }


        

    }
}
