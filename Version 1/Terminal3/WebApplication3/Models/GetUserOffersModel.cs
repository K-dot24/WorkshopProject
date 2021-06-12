using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class GetUserOffersModel
    {
        //Properties
        public String UserID { get; }


        //Constructor
        public GetUserOffersModel(string userID)
        {
            UserID = userID;
        }
    }
}
