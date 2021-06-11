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

        public Result<bool> notifyOfferRecievedUser(String UserID, String StoreId, String ProductID, int Amount, double price, double CounterOffer, bool Accepted)
        {
            String msg = "";
            if (Accepted)
                msg = $"Event : An offer has been accepted\nProduct Id : {ProductID}\nStore Id : {StoreId}\nAmount : {Amount}\nPrice : {price}\n";
            else if (CounterOffer == -1)
                msg = $"Event : An offer has been declined\nProduct Id : {ProductID}\nStore Id : {StoreId}\nAmount : {Amount}\nPrice : {price}\n";
            else
                msg = $"Event : A counter offer was recieved\nProduct Id : {ProductID}\nStore Id : {StoreId}\nAmount : {Amount}\nNew price : {CounterOffer}\n";

            Notification notification = new Notification(Event.OfferRecievedUser, UserID, msg, false);
            return notifyNotificationServer(notification);
        }
    }
}
