﻿namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public interface IPurchasePolicy
    {
        //TODO
    }

    public enum policyType : int
    {
        Auction = 0 ,
        BuyNow = 1,
        Lottery = 2 ,
        Offer = 3
    }
}
