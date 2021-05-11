using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public class Notification
    {
        // Properties
        public Event EventName { get; }
        public String Message { get; }
        public DateTime Date { get; }
        public Boolean isOpened { get; set; }
        public Boolean isStoreStaff { get; set; }
        public String ClientId { get; set; }


        public Notification(Event eventName, String clientId , String msg  ,Boolean staff)
        {
            EventName = eventName;
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
