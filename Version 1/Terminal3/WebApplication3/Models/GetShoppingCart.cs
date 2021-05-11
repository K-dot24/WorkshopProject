using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class GetShoppingCart
    {
        //Properties
        public String Id { get; }
        public LinkedList<GetShoppingBag> ShoppingBags { get; }
        public Double TotalCartPrice { get; }


        //Constructor
        public GetShoppingCart(string shoppingCartId, LinkedList<GetShoppingBag> shoppingBags, Double totalCartPrice)
        {
            Id = shoppingCartId;
            ShoppingBags = shoppingBags;
            TotalCartPrice = totalCartPrice;
        }
    }
}
