﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;
using Xunit;


namespace Terminal3_E2ETests
{
    public class E2ETests
    {
        String BadMongoURLPath = @"..\netcoreapp3.1\BadMongoUrlConfig.json";
        String BadExternalSystemUrlPath = @"..\netcoreapp3.1\BadExternalSystemConfig.json";
        String InvalidConfigPath = @"..\netcoreapp3.1\InvalidConfig.json";
        String Config = @"..\netcoreapp3.1\Config.json";

        public E2ETests()
        { }

        [Fact()]
        [Trait("Category","InitConfigTests")]
        public void BadMongoUrlConfig()
        {
            ECommerceSystem system = new ECommerceSystem(configData: File.ReadAllText(BadMongoURLPath)); 
        }

        [Fact()]
        [Trait("Category", "InitConfigTests")]
        public void BadExternalSystemUrlConfig()
        {
            ECommerceSystem system = new ECommerceSystem(configData: File.ReadAllText(BadExternalSystemUrlPath));

            system.Register("shaked@gmail.com", "123");
            Result<RegisteredUserService> user = system.Login("shaked@gmail.com", "123");


            Result<StoreService> store = system.OpenNewStore("testStore", user.Data.Id);
            Result<ProductService> product = system.AddProductToStore(user.Data.Id, store.Data.Id, "testProduct", 10, 1, "Test");

            // Add product to user shopping bag
            system.AddProductToCart(user.Data.Id, product.Data.Id, 1, store.Data.Id);

            IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            Result<ShoppingCartService> res = system.Purchase(user.Data.Id, paymentDetails, deliveryDetails);
            Assert.False(res.ExecStatus);
        }

        [Fact()]
        [Trait("Category", "InitConfigTests")]
        public void InvalidConfig()
        {
 
            new ECommerceSystem(configData: File.ReadAllText(InvalidConfigPath));
        }



        #region recipt-test



        [Fact()]
        public void GetIncomeAmountGroupByDay_data()
        {
            ECommerceSystem system = new ECommerceSystem(configData: File.ReadAllText(Config));
            //ECommerceSystem system = new ECommerceSystem(config_path: @"..\..\..\..\..\Terminal3-E2ETests\Config.json");

            //system.ResetSystem("-777");
            //purchase 1
            String randomEmail = Service.GenerateId();
            system.Register(randomEmail, "123");
            Result<RegisteredUserService> user = system.Login(randomEmail, "123");


            Result<StoreService> store = system.OpenNewStore("testStore", user.Data.Id);
            Result<ProductService> product = system.AddProductToStore(user.Data.Id, store.Data.Id, "testProduct", 9.0, 1, "Test");

            // Add product to user shopping bag
            system.AddProductToCart(user.Data.Id, product.Data.Id, 1, store.Data.Id);

            //IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            //IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            IDictionary<String, Object>  paymentDetails = new Dictionary<String, Object>
                    {
                     { "card_number", "2222333344445555" },
                     { "month", "4" },
                     { "year", "2021" },
                     { "holder", "Israel Israelovice" },
                     { "ccv", "262" },
                     { "id", "20444444" }
                    };

            IDictionary<String, Object>  deliveryDetails = new Dictionary<String, Object>
                    {
                     { "name", "Israel Israelovice" },
                     { "address", "Rager Blvd 12" },
                     { "city", "Beer Sheva" },
                     { "country", "Israel" },
                     { "zip", "8458527" }
                    };

            Result<ShoppingCartService> res = system.Purchase(user.Data.Id, paymentDetails, deliveryDetails);

            Assert.True(res.ExecStatus);

            //purchase 2
            String randomEmail2 = Service.GenerateId();
            system.Register(randomEmail2, "456");
            Result<RegisteredUserService> user2 = system.Login(randomEmail2, "456");


            Result<StoreService> store2 = system.OpenNewStore("testStore", user2.Data.Id);
            Result<ProductService> product2 = system.AddProductToStore(user2.Data.Id, store2.Data.Id, "testProduct2", 4.5, 1, "Test");

            // Add product to user shopping bag
            system.AddProductToCart(user2.Data.Id, product2.Data.Id, 1, store2.Data.Id);

            //IDictionary<String, Object> paymentDetails2 = new Dictionary<String, Object>();
            //IDictionary<String, Object> deliveryDetails2 = new Dictionary<String, Object>();

            IDictionary<String, Object> paymentDetails2 = new Dictionary<String, Object>
                       {
                        { "card_number", "2222333344446666" },
                        { "month", "3" },
                        { "year", "2021" },
                        { "holder", "Israel Israelovice" },
                        { "ccv", "242" },
                        { "id", "20444445" }
                       };

            IDictionary<String, Object> deliveryDetails2 = new Dictionary<String, Object>
                       {
                        { "name", "Israel Israelovice" },
                        { "address", "Rager Blvd 13" },
                        { "city", "Beer Sheva" },
                        { "country", "Israel" },
                        { "zip", "8458526" }
                       };

            Result<ShoppingCartService> res2 = system.Purchase(user2.Data.Id, paymentDetails2, deliveryDetails2);

            Assert.True(res2.ExecStatus);

            Result<List<Tuple<DateTime, Double>>> recipts_owner = system.GetIncomeAmountGroupByDay("2021-06-16", "2021-06-16" , store.Data.Id, user.Data.Id);
            Result<List<Tuple<DateTime, Double>>> recipts_owner2 = system.GetIncomeAmountGroupByDay("2021-06-16", "2021-06-16", store2.Data.Id, user2.Data.Id);
            Result<List<Tuple<DateTime, Double>>> recipts_admin = system.GetIncomeAmountGroupByDay("2021-06-16", "2021-06-16", "-777");

            Assert.True(recipts_owner.Data[0].Item2 == 9.0);
            Assert.True(recipts_owner2.Data[0].Item2 == 4.5);
            Assert.True(recipts_admin.Data[0].Item2 == 13.5);
            
            system.ResetSystem("-777");
        }

        [Fact()]
        public void GetIncomeAmountGroupByDay_empty()
        {
            ECommerceSystem system = new ECommerceSystem(configData: File.ReadAllText(Config));
            //ECommerceSystem system = new ECommerceSystem(config_path: @"..\..\..\..\..\Terminal3\Config.json");
            system.Register("shaked@gmail.com", "123");
            Result<RegisteredUserService> user = system.Login("shaked@gmail.com", "123");


            Result<StoreService> store = system.OpenNewStore("testStore", user.Data.Id);
            //Result<ProductService> product = system.AddProductToStore(user.Data.Id, store.Data.Id, "testProduct", 10, 1, "Test");

            Result<List<Tuple<DateTime, Double>>> recipts_owner = system.GetIncomeAmountGroupByDay("2021-06-16", "2021-06-07", store.Data.Id, user.Data.Id);
            Result<List<Tuple<DateTime, Double>>> recipts_admin = system.GetIncomeAmountGroupByDay("2021-06-16", "2021-06-07", "-777");

            Assert.True(recipts_owner.Data[0].Item2 == 0);
            Assert.True(recipts_admin.Data[0].Item2 == 0);

            system.ResetSystem("-777");
        }

    }


    #endregion
}
