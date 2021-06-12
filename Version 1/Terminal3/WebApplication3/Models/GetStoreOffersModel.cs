using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class GetStoreOffersModel
    {
        //Properties
        public String StoreID { get; }       


        //Constructor
        public GetStoreOffersModel(string storeID)
        {
            StoreID = storeID;
        }
    }
}
