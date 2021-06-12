using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class AnswerCounterOfferModel
    {

        //Properties
        public String userID { get; }
        public String offerID{ get; }
        public bool accepted { get; }


        //Constructor
        public AnswerCounterOfferModel(string userID, string offerID, bool accepted)
        {
            this.userID = userID;
            this.offerID = offerID;
            this.accepted = accepted;
        }

    }
}
