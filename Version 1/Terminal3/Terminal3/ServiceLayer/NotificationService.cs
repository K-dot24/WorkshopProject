using Microsoft.AspNetCore.SignalR.Client;
using SignalRgateway.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
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
            try
            {
                notification.isOpened = true;
                //hubProxy.Invoke("SendMessage", notification);
                connection.InvokeAsync("SendMessage", notification.ClientId, notification.Message);
                //List<Notification> queue = notificationToBeSend[notification.EventName];
                //queue.Add(notification);            
                return new Result<bool>("Notification is displayed to user\n", true, true);
            }
            catch (Exception e){
                Logger.LogError(e.ToString());
                return new Result<bool>("There was a problem with the notification service", false, false);
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Notification> GetNotificationByEvent(Event eventEnum)
        {
            try
            {
                List<Notification> toReturn = new List<Notification>(notificationToBeSend[eventEnum]);
                notificationToBeSend[eventEnum].Clear();
                return toReturn;
            }
            catch(Exception e)
            {
                Logger.LogError(e.ToString());
                return new List<Notification>();
            }

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Broadcast(string message)
        {
            try
            {
                connection.InvokeAsync("SendBroadcast", message);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void sendMonitorStatus(DTO_Monitor monitor)
        {
            try
            {
                connection.InvokeAsync("sendMonitor", new Record(monitor.Date,monitor.GuestUsers,monitor.RegisteredUsers,monitor.ManagersNotOwners,monitor.Owners,monitor.Admins));
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());

            }
        }

    }
}
