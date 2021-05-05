using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer
{
    public class NotificationService
    {
        private NotificationService Instance { get; set; }
        
        private NotificationService()
        {
            Instance = null;
        }

        public NotificationService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NotificationService();
            }
            return Instance;
        }

        //TODO
        public Result<bool> Update(Notification notification)
        {
            notification.isOpened = true;      

            throw new NotImplementedException();
            //return new Result<bool>("Notification is displayed to manager\n", true, true);
        }

    }
}
