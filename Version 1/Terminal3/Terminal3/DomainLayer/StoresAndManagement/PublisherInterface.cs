using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface PublisherInterface
    {
        Result<Boolean> notifyStorePurchase(Product product , int quantity);
        Result<Boolean> notifyStoreClosed();
        Result<Boolean> notifyStoreOpened();
        Result<Boolean> notifyOwnerSubscriptionRemoved(String ownerID);                

    }
}
