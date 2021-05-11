using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public class Notification
    {
        // Properties
        public String Message { get; }
        public DateTime Date { get; }
        public Boolean isOpened { get; set; }
        public Boolean isStoreStaff { get; set; }
        public String ClientId { get; set; }


        public Notification(String clientId , String msg  ,Boolean staff)
        {
            this.Message = msg;
            this.Date = DateTime.Now;
            isOpened = false;
            isStoreStaff = staff;
            this.ClientId = clientId;
        }

        public String ToString()
        {
            return $"{Date.ToString("MM/dd/yyyy HH:mm")}\nNotice:\n{Message}\n";
        }


    }
}
