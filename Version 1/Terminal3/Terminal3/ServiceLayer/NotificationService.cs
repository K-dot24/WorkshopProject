using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3.ServiceLayer
{
    public enum Event
    {
        StorePurchase,
        StoreClosed,
        StoreOpened,
        OwnerSubscriptionRemoved,
        ProductReview
    };

    public sealed class NotificationService
    {
        //Properties
        private static NotificationService Instance { get; set; } = null;
        private Dictionary<Event, List<Notification>> notificationToBeSend;
        //public IHubProxy hubProxy { get; set; }
        public HubConnection connection { get; set; }

        private NotificationService()
        {
            notificationToBeSend = new Dictionary<Event, List<Notification>>();
            notificationToBeSend.Add(Event.StorePurchase, new List<Notification>());
            notificationToBeSend.Add(Event.StoreClosed, new List<Notification>());
            notificationToBeSend.Add(Event.StoreOpened, new List<Notification>());
            notificationToBeSend.Add(Event.OwnerSubscriptionRemoved, new List<Notification>());
            notificationToBeSend.Add(Event.ProductReview, new List<Notification>());

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static NotificationService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NotificationService();
            }
            return Instance;
        }

        //TODO
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Result<bool> Update(Notification notification)
        {
            notification.isOpened = true;
            //hubProxy.Invoke("SendMessage", notification);
            connection.InvokeAsync("SendMessage", notification);
            //List<Notification> queue = notificationToBeSend[notification.EventName];
            //queue.Add(notification);            
            return new Result<bool>("Notification is displayed to user\n", true, true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Notification> GetNotificationByEvent(Event eventEnum)
        {
            List<Notification> toReturn = new List<Notification>(notificationToBeSend[eventEnum]);
            notificationToBeSend[eventEnum].Clear();
            return toReturn;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Notification> GetPendingMessagesByUserID(string userId)
        {
            List<Notification> pendingMessages = new List<Notification>();
            foreach (Event evetLibrman in notificationToBeSend.Keys)
            {
                List<Notification> queue = notificationToBeSend[evetLibrman];
                foreach (Notification notification in queue)
                {
                    if (notification.ClientId.Equals(userId)) {pendingMessages.Add(notification);}
                }
                queue.RemoveAll(x => x.ClientId == userId);
            }
            return pendingMessages;
        }

    }
}
