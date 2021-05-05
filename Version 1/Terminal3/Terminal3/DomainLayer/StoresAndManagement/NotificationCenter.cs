using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;

namespace Terminal3.DomainLayer.StoresAndManagement
{

    public class NotificationCenter 
    {
        private NotificationCenter Instance { get; set; }
        public NotificationService NotificationService { get; }

        private NotificationCenter()
        {
            Instance = null;
            this.NotificationService = NotificationService.GetInstance();
        }

        public NotificationCenter GetInstance()
        {
            if(Instance == null)
            {
                Instance = new NotificationCenter();
            }
            return Instance;
        } 

        
        public Result<Boolean> notifyNotificationServer(Notification notification)
        {
            return NotificationService.Update(notification);
        }
    }
}
