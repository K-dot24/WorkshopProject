using System;
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
        public LinkedList<Tuple< ProductService , int>> Products { get; set; }  //<productDAL , quantity>  -- TODO - check why quantity here if there is a quantity field in productservice
        public Double TotalBagPrice { get; }


        //Constructor 
        public ShoppingBagService(String bagID , String userID, String storeID, IDictionary<ProductService, int> products ,Double totalBagPrice )
        {
            Id = bagID;
            UserId = userID;
            StoreId = storeID;
            Products = new LinkedList<Tuple<ProductService, int>>();
            foreach (ProductService product in products.Keys) {
                Products.AddFirst(new Tuple<ProductService, int>(product, products[product])); 
            }
            TotalBagPrice = totalBagPrice;
        }


        public ShoppingBagService(String bagID, String userID, String storeID, LinkedList<Tuple<ProductService, int>> products, Double totalBagPrice)
        {
            Id = bagID;
            UserId = userID;
            StoreId = storeID;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }

    }
}
