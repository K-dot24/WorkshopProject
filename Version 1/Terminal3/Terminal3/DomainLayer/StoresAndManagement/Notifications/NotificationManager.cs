using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;

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

        // for loading from database only
        public NotificationManager()
        {
        }



        //TODO -         [MethodImpl(MethodImplOptions.Synchronized)]

        // Methods
        public Result<bool> notifyStorePurchase(Product product , int quantity)
        {
            String msg = $"Event : Product Purchased\nStore Id : {Store.Id}\nProduct Name : {product.Name}\nProduct Quantity : {quantity}\n";
            notify(Event.StorePurchase,msg , true);
            return new Result<bool>("All staff members are notified with product purchase\n", true, true);
        }

        public Result<bool> notifyStoreClosed()
        {
            String msg = $"Event : Store Closed\nStore Id : {Store.Id}\n";
            notify(Event.StoreClosed,msg , true);
            return new Result<bool>($"All staff members are notified that store {Store.Id} is closed\n", true, true);

        }

        public Result<bool> notifyStoreOpened()
        {
            String msg = $"Event : Store Opened\nStore Id : {Store.Id}\n";
            notify(Event.StoreOpened,msg , true);
            return new Result<bool>($"All staff members are notified that store {Store.Id} is opened\n", true, true);
        }

        public Result<bool> notifyOwnerSubscriptionRemoved(string ownerID, StoreOwner removedOwner)
        {
            String msg = $"Event : Owner Subscription Removed\nStore Id : {Store.Id}\nOwner Id : {ownerID}\n";
            Notification notification = new Notification(Event.OwnerSubscriptionRemoved,ownerID, msg, true);
            removedOwner.Update(notification);      // send notification to the removed owner itself as well as to the store staff
            notify(Event.OwnerSubscriptionRemoved,msg , true);
            return new Result<bool>($"All staff members are notified that owner ({ownerID}) subscriptoin as store owner ({Store.Id}) has been removed\n", true, true);
        }

        public Result<bool> notifyProductReview(Product product , String review)
        {
            String msg = $"Event : A product review was added\nStore Id : {Store.Id}\nProduct Id : {product.Id}\nReview : {review}\n";
            notifyOwners(Event.ProductReview,msg, true);
            return new Result<bool>($"All staff members are notified that a product review was added to store {Store.Id} successfuly\n", true, true);
        }



        // Private Functions
        private void notify(Event eventName,String msg , Boolean isStaff)
        {
            //Send a new (and different) notification for each staff member due to the addresse Id field in the notificaion itself
            ConcurrentDictionary<String, StoreOwner> Owners = Store.Owners;
            ConcurrentDictionary<String, StoreManager> Managers = Store.Managers;

            foreach (var owner in Owners)
            {
                Notification notification = new Notification(eventName, owner.Value.GetId() , msg, true);
                owner.Value.Update(notification);
            }

            foreach (var manager in Managers)
            {
                Notification notification = new Notification(eventName, manager.Value.GetId() ,msg, true);
                manager.Value.Update(notification);
            }
        }

        private void notifyOwners(Event eventName,String msg, Boolean isStaff)
        {
            //Send a new (and different) notification for each staff member due to the addresse Id field in the notificaion itself
            ConcurrentDictionary<String, StoreOwner> Owners = Store.Owners;

            foreach (var owner in Owners)
            {
                Notification notification = new Notification(eventName, owner.Value.GetId(), msg, true);
                owner.Value.Update(notification);
            }            
        }

    }
}
