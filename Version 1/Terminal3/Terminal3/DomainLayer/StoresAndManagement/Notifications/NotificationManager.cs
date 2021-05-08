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
            notify(msg , true);
            return new Result<bool>("All staff members are notified with product purchase\n", true, true);
        }

        public Result<bool> notifyStoreClosed()
        {
            String msg = $"Event : Store Closed\nStore Id : {Store.Id}\n";
            notify(msg , true);
            return new Result<bool>($"All staff members are notified that store {Store.Id} is closed\n", true, true);

        }

        public Result<bool> notifyStoreOpened()
        {
            String msg = $"Event : Store Opened\nStore Id : {Store.Id}\n";
            notify(msg , true);
            return new Result<bool>($"All staff members are notified that store {Store.Id} is opened\n", true, true);
        }

        public Result<bool> notifyOwnerSubscriptionRemoved(string ownerID, StoreOwner removedOwner)
        {
            String msg = $"Event : Owner Subscription Removed\nStore Id : {Store.Id}\nOwner Id : {ownerID}";
            Notification notification = new Notification(ownerID, msg, true);
            removedOwner.Update(notification);      // send notification to the removed owner itself as well as to the store staff
            notify(msg , true);
            return new Result<bool>($"All staff members are notified that owner ({ownerID}) subscriptoin as store owner ({Store.Id}) has been removed\n", true, true);
        }



        // Private Functions
        private void notify(String msg , Boolean isStaff)
        {
            //Send a new (and different) notification for each staff member due to the addresse Id field in the notificaion itself
            ConcurrentDictionary<String, StoreOwner> Owners = Store.Owners;
            ConcurrentDictionary<String, StoreManager> Managers = Store.Managers;

            foreach (var owner in Owners)
            {
                Notification notification = new Notification(owner.Value.GetId() , msg, true);
                owner.Value.Update(notification);
            }

            foreach (var manager in Managers)
            {
                Notification notification = new Notification(manager.Value.GetId() ,msg, true);
                manager.Value.Update(notification);
            }
        }

    }
}
