using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;
using Xunit;


namespace Terminal3_E2ETests
{
    public class E2ETests
    {
        public E2ETests()
        { }

        [Fact()]
        public void BadMongoUrlConfig()
        {
            String config_path = @"..\Terminal3\BadMongoUrlConfig.json";
            ECommerceSystem system = new ECommerceSystem(config_path);

            Assert.True(system.Register("shaked@gmail.com", "123").ExecStatus);
        }

        [Fact()]
        public void BadExternalSystemUrlConfig()
        {
            String config_path = @"..\Terminal3\BadExternalSystemConfig.json";
            ECommerceSystem system = new ECommerceSystem(config_path);

            system.Register("shaked@gmail.com", "123");
            Result<RegisteredUserService> user = system.Login("shaked@gmail.com", "123");


            Result<StoreService> store = system.OpenNewStore("testStore", user.Data.Id);
            Result<ProductService> product = system.AddProductToStore(user.Data.Id, store.Data.Id, "testProduct", 10, 1, "Test");

            // Add product to user shopping bag
            system.AddProductToCart(user.Data.Id, product.Data.Id, 1, store.Data.Id);

            IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            Result<ShoppingCartService> res = system.Purchase(user.Data.Id, paymentDetails, deliveryDetails);
            Assert.True(res.ExecStatus);
        }

        [Fact()]
        public void InvalidConfig()
        {
            String config_path = @"..\Terminal3\InvalidConfig.json";
            new ECommerceSystem(config_path);
        }


    }
}
