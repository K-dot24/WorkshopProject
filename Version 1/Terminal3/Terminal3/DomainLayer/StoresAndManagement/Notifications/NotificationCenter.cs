using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;

namespace Terminal3.DomainLayer.StoresAndManagement
{

    public sealed class NotificationCenter 
    {
        private static NotificationCenter Instance { get; set; } = null;
        public NotificationService NotificationService { get; }

        private NotificationCenter()
        {
            this.NotificationService = NotificationService.GetInstance();
        }

        public static NotificationCenter GetInstance()
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
