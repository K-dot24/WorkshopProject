using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public class NotificationManager : PublisherInterface
    {
        // Properties
        public Store Store { get; set; }
        
        // Constructor
        public NotificationManager(Store store)
        {
            this.Store = store;
        }

        //TODO -         [MethodImpl(MethodImplOptions.Synchronized)]

        // Methods
        public Result<bool> notifyStorePurchase(Product product , int quantity)
        {
            String msg = $"Event : Product Purchased\nStore Id : {Store.Id}\nProduct Name : {product.Name}\nProduct Quantity : {quantity}\n";
            Notification notification = new Notification(msg, true);
            notify(notification);
            return new Result<bool>("All staff members are notified with product purchase\n", true, true);
        }

        public Result<bool> notifyStoreClosed()
        {
            String msg = $"Event : Store Closed\nStore Id : {Store.Id}\n";
            Notification notification = new Notification(msg, true);
            notify(notification);
            return new Result<bool>($"All staff members are notified that store {Store.Id} is closed\n", true, true);

        }

        public Result<bool> notifyStoreOpened()
        {
            String msg = $"Event : Store Opened\nStore Id : {Store.Id}\n";
            Notification notification = new Notification(msg, true);
            notify(notification);
            return new Result<bool>($"All staff members are notified that store {Store.Id} is opened\n", true, true);
        }

        public Result<bool> notifyOwnerSubscriptionRemoved(string ownerID)
        {
            String msg = $"Event : Owner Subscription Removed\nStore Id : {Store.Id}\nOwner Id : {ownerID}";
            Notification notification = new Notification(msg, true);
            notify(notification);
            return new Result<bool>($"All staff members are notified that owner ({ownerID}) subscriptoin as store owner ({Store.Id}) has been removed\n", true, true);
        }



        // Private Functions
        private void notify(Notification notification)
        {
            ConcurrentDictionary<String, StoreOwner> Owners = Store.Owners;
            ConcurrentDictionary<String, StoreManager> Managers = Store.Managers;

            foreach (var owner in Owners)
            {
                owner.Value.Update(notification);
            }

            foreach (var manager in Managers)
            {
                manager.Value.Update(notification);
            }
        }

    }
}
