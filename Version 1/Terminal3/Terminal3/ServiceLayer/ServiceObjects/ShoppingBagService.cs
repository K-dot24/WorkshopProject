﻿using System;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Terminal3.ServiceLayer.ServiceObjects
{
    public class ShoppingBagService
    {
        //Properties
        public String Id { get; }
        public String UserId { get; }
        public String StoreId { get; }
        public IDictionary<ProductService , int> Products { get; }  //<productDAL , quantity>
        public Double TotalBagPrice { get; }


        //Constructor 
        public ShoppingBagService(String bagID , String userID, String storeID, IDictionary<ProductService, int> products ,Double totalBagPrice )
        {
            Id = bagID;
            UserId = userID;
            StoreId = storeID;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }


        

    }
}
