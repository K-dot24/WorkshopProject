using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalrServer.Model
{
    public class Notification
    {
        public String UserID { get; set; }
        public String Message { get; set; }

        public Notification(string userID, string message)
        {
            UserID = userID;
            Message = message;
        }
    }
}
