using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Xunit;

namespace XUnitTestTerminal3
{
    public class OfferPurchase : XUnitTerminal3TestCase
    {

        public string OwnerID{ get; }

        public string RegisteredUserID { get; }

        public string StoreID { get; }

        public string ProductID { get; }

        public string OfferID { get; }

        public OfferPurchase()
        {
            sut.Register("owner@offer", "pass1");
            OwnerID = sut.Login("owner@offer", "pass1").Data;
            StoreID = sut.OpenNewStore("store1", OwnerID).Data;
            ProductID = sut.AddProductToStore(OwnerID, StoreID, "product", 10, 100, "").Data;

            sut.Register("user@offer", "pass2");
            RegisteredUserID = sut.Login("user@offer", "pass2").Data;

        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void SingleOfferPrice()
        {
            sut.SendOfferToStore(StoreID, RegisteredUserID, ProductID, 2, 1);
            sut.SendOfferResponseToUser(StoreID, OwnerID, RegisteredUserID, OfferID, true, -1);
            sut.AddProductToCart(RegisteredUserID, ProductID, 2, StoreID);
            Assert.True(sut.GetTotalShoppingCartPrice(RegisteredUserID).Data == 1);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void TwoOffersPrice()
        {
            sut.SendOfferToStore(StoreID, RegisteredUserID, ProductID, 2, 1);
            sut.SendOfferToStore(StoreID, RegisteredUserID, ProductID, 1, 2);
            sut.SendOfferResponseToUser(StoreID, OwnerID, RegisteredUserID, OfferID, true, -1);
            sut.AddProductToCart(RegisteredUserID, ProductID, 4, StoreID);
            Assert.True(sut.GetTotalShoppingCartPrice(RegisteredUserID).Data == 3);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void OfferWithRegularPrice()
        {
            sut.SendOfferToStore(StoreID, RegisteredUserID, ProductID, 2, 1);
            sut.SendOfferResponseToUser(StoreID, OwnerID, RegisteredUserID, OfferID, true, -1);
            sut.AddProductToCart(RegisteredUserID, ProductID, 3, StoreID);
            Assert.True(sut.GetTotalShoppingCartPrice(RegisteredUserID).Data == 11);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void OfferButNotEnoughProductsPrice()
        {
            sut.SendOfferToStore(StoreID, RegisteredUserID, ProductID, 2, 1);
            sut.SendOfferResponseToUser(StoreID, OwnerID, RegisteredUserID, OfferID, true, -1);
            sut.AddProductToCart(RegisteredUserID, ProductID, 1, StoreID);
            Assert.True(sut.GetTotalShoppingCartPrice(RegisteredUserID).Data == 10);
        }

        [Fact]
        [Trait("Category", "acceptance")]
        public void PurchaseClearsAcceptedOffersPrice()
        {
            sut.SendOfferToStore(StoreID, RegisteredUserID, ProductID, 2, 1);
            sut.SendOfferResponseToUser(StoreID, OwnerID, RegisteredUserID, OfferID, true, -1);
            sut.AddProductToCart(RegisteredUserID, ProductID, 2, StoreID);
            IDictionary<String, Object> dictionaryPay = new Dictionary<String, Object>() { { "Name", "test_product" } };
            IDictionary<String, Object> dictionaryDeliver = new Dictionary<String, Object>() { { "Name", "test_product" } };
            Assert.True(sut.Purchase(RegisteredUserID, dictionaryPay, dictionaryDeliver).ExecStatus);

            sut.AddProductToCart(RegisteredUserID, ProductID, 2, StoreID);
            Assert.True(sut.GetTotalShoppingCartPrice(RegisteredUserID).Data == 20);
        }

    }
}
