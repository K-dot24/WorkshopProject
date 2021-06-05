using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;
using Xunit;


namespace Terminal3_E2ETests
{
    public class E2ETests
    {
        String BadMongoURLPath = @"..\netcoreapp3.1\BadMongoUrlConfig.json";
        String BadExternalSystemUrlPath = @"..\netcoreapp3\BadExternalSystemConfig.json";
        String InvalidConfigPath = @"..\netcoreapp3\InvalidConfig.json";

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


    }
}
