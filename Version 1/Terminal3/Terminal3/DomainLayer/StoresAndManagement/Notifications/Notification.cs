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
        
        public Notification(String msg  ,Boolean staff)
        {
            this.Message = msg;
            this.Date = DateTime.Now;
            isOpened = false;
            isStoreStaff = staff;
        }

        public String ToString()
        {
            return $"{Date.ToString("MM/dd/yyyy HH:mm")}\nNotice:\n{Message}\n";
        }


    }
}
