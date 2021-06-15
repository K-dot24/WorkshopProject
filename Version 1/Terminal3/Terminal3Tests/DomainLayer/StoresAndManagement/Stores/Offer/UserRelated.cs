using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Xunit;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class UserRelated
    {
        public RegisteredUser RegisteredUser { get; }

        public UserRelated()
        {
            RegisteredUser = new RegisteredUser("reg@user", "pass3");
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void AddOfferTest()
        {
            Assert.Empty(RegisteredUser.PendingOffers);
            Assert.Empty(RegisteredUser.AcceptedOffers);
            Result<Offer> result = RegisteredUser.SendOfferToStore("storeID", "productID", 10, 20);
            Assert.True(result.ExecStatus);
            Assert.NotNull(result.Data);
            Assert.Equal("storeID", result.Data.StoreID);
            Assert.Equal("productID", result.Data.ProductID);
            Assert.True(10 == result.Data.Amount);
            Assert.True(20 == result.Data.Price);
            Assert.Single(RegisteredUser.PendingOffers);
            Assert.Empty(RegisteredUser.AcceptedOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void RemoveOfferTest()
        {
            Offer Offer = RegisteredUser.SendOfferToStore("storeID", "productID", 10, 10).Data;
            RegisteredUser.RemovePendingOffer(Offer.Id);
            Assert.Empty(RegisteredUser.PendingOffers);
            Assert.Empty(RegisteredUser.AcceptedOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void AcceptOfferTest()
        {
            Offer Offer = RegisteredUser.SendOfferToStore("storeID", "productID", 10, 10).Data;
            RegisteredUser.AcceptOffer(Offer.Id);
            Assert.Empty(RegisteredUser.PendingOffers);
            Assert.Single(RegisteredUser.AcceptedOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void DeclineOfferTest()
        {
            Offer Offer = RegisteredUser.SendOfferToStore("storeID", "productID", 10, 10).Data;
            RegisteredUser.DeclineOffer(Offer.Id);
            Assert.Empty(RegisteredUser.PendingOffers);
            Assert.Empty(RegisteredUser.AcceptedOffers);
        }


        [Theory()]
        [Trait("Category", "Unit")]
        [InlineData(true)]
        [InlineData(false)]
        public void CounterOfferOfferTest(bool accept)
        {
            Offer Offer = RegisteredUser.SendOfferToStore("storeID", "productID", 10, 10).Data;
            Offer.CounterOffer = 20;
            RegisteredUser.CounterOffer(Offer.Id);
            Assert.Single(RegisteredUser.PendingOffers);
            Assert.Empty(RegisteredUser.AcceptedOffers);

            RegisteredUser.AnswerCounterOffer(Offer.Id, accept);
            Assert.Empty(RegisteredUser.PendingOffers);
            if (accept) 
                Assert.Single(RegisteredUser.AcceptedOffers);
            else
                Assert.Empty(RegisteredUser.AcceptedOffers);
        }


        [Fact()]
        [Trait("Category", "Unit")]
        public void OfferGetDataTest()
        {
            RegisteredUser.SendOfferToStore("storeID", "productID", 10, 10);
            Result<List<Dictionary<string, object>>> result = RegisteredUser.getUserPendingOffers();
            Assert.True(result.ExecStatus);
            Assert.Single(result.Data);
        }

    }
}
