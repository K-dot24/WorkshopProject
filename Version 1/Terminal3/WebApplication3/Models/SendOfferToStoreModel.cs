using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class SendOfferToStoreModel
    {
        //Properties
        public String StoreID { get; }
        public String UserID { get; }
        public String ProductID { get; }
        public int Amount{ get; }
        public double Price { get; }


        //Constructor
        public SendOfferToStoreModel(string storeID, string userID, string productID, int amount, double price)
        {
            StoreID = storeID;
            UserID = userID;
            ProductID = productID;
            Amount = amount;
            Price = price;
        }
    }
}
