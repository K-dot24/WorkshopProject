using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Xunit;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class StoreRelated
    {
        public RegisteredUser Owner1 { get; }
        public RegisteredUser Owner2 { get; }
        public RegisteredUser RegisteredUser { get; }

        public Store Store { get; }

        public Product Product { get; }

        public Offer Offer { get; }

        public StoreRelated()
        {
            Owner1 = new RegisteredUser("owner1@offer", "pass1");
            Owner2 = new RegisteredUser("owner2@offer", "pass2");
            Store = new Store("store1", Owner1, "-1", true);
            Store.AddStoreOwner(Owner2, Owner1.Id);
            Product = Store.AddNewProduct(Owner1.Id, "product", 100, 100, "").Data;
            RegisteredUser = new RegisteredUser("reg@user", "pass3");
            Offer = new Offer(RegisteredUser.Id, Product.Id, 10, 10, Store.Id);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void sendOfferToStoreTest()
        {
            Store.SendOfferToStore(Offer);
            Assert.Single(Store.OfferManager.PendingOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void sendOfferResponseDeclineTest()
        {
            Store.SendOfferToStore(Offer);
            Store.SendOfferResponseToUser(Owner1.Id, "wrong id", false, -1);
            Store.SendOfferResponseToUser("wrong id", Offer.Id, false, -1);
            Assert.Single(Store.OfferManager.PendingOffers);
            Store.SendOfferResponseToUser(Owner1.Id, Offer.Id, false, -1);
            Assert.Empty(Store.OfferManager.PendingOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void sendOfferResponseCounterOfferTest()
        {
            Store.SendOfferToStore(Offer);
            Store.SendOfferResponseToUser(Owner1.Id, "wrong id", false, 2);
            Store.SendOfferResponseToUser("wrong id", Offer.Id, false, 2);
            Assert.Single(Store.OfferManager.PendingOffers);
            Store.SendOfferResponseToUser(Owner1.Id, Offer.Id, false, 2);
            Assert.Empty(Store.OfferManager.PendingOffers);
            Assert.True(Offer.CounterOffer == 2);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void sendOfferResponseAcceptTest()
        {
            Store.SendOfferToStore(Offer);
            Store.SendOfferResponseToUser(Owner1.Id, Offer.Id, true, -1);
            Assert.Single(Store.OfferManager.PendingOffers);
            Store.SendOfferResponseToUser(Owner2.Id, Offer.Id, true, -1);
            Assert.Empty(Store.OfferManager.PendingOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void OfferGetDataTest()
        {
            Store.SendOfferToStore(Offer);
            Result<List<Dictionary<string, object>>> result = Store.getStoreOffers();
            Assert.True(result.ExecStatus);
            Assert.Single(result.Data);
        }

    }
}
