using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class SendOfferResponseToUserModel
    {
        //Properties
        public String StoreID { get; }
        public String OwnerID { get; }
        public String UserID { get; }
        public String OfferID { get; }
        public bool Accepted { get; }
        public double CounterOffer{ get; }


        //Constructor
        public SendOfferResponseToUserModel(string storeID, string ownerID, string userID, string offerID, bool accepted, double counterOffer)
        {
            StoreID = storeID;
            OwnerID = ownerID;
            UserID = userID;
            OfferID = offerID;
            Accepted = accepted;
            CounterOffer = counterOffer;
        }
    }
}
