using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalrServer.Models
{
    public class SignalRLoginModel
    {
        public string oldUserID { get; set; }
        public string newUserID { get; set; }

        public SignalRLoginModel(string oldUserID, string newUserID)
        {
            this.oldUserID = oldUserID;
            this.newUserID = newUserID;
        }
    }
}
