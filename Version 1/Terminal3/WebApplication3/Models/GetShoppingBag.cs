using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3WebAPI.Models
{
    public class GetShoppingBag
    {
        //Properties
        public String Id { get; }
        public String UserId { get; }
        public String StoreId { get; }
        public LinkedList<ProductService> Products { get;}  //<productDAL , quantity>
        public Double TotalBagPrice { get; }


        //Constructor 
        public GetShoppingBag(String bagID, String userID, String storeID, LinkedList<ProductService> products, Double totalBagPrice)
        {
            Id = bagID;
            UserId = userID;
            StoreId = storeID;
            Products = new LinkedList<ProductService>();
            foreach (ProductService product in products)
            {
                Products.AddFirst(product);
            }
            TotalBagPrice = totalBagPrice;
        }
    }
}
