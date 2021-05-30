using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTestTerminal3
{
    public class PurchaseConcurrencyTest : XUnitTerminal3TestCase
    {
        private string kfir_id;
        private string igor_id;
        private string store_id;
        private string product_id;
        private IDictionary<String, Object> paymentDetails;
        private IDictionary<String, Object> deliveryDetails;
        private BlockingCollection<bool> results;
        public PurchaseConcurrencyTest() : base()
        {
            sut.Register("kfir@gmail.com", "kfir");
            sut.Register("igor@gmail.com", "igor");
            this.kfir_id = sut.Login("kfir@gmail.com", "kfir").Data;
            this.igor_id = sut.Login("igor@gmail.com", "igor").Data;
            this.store_id = sut.OpenNewStore("test_store", kfir_id).Data;
            this.product_id = sut.AddProductToStore(kfir_id, store_id, "test_product", 10, 1, "test").Data;
            paymentDetails = new Dictionary<String, Object>
                    {
                     { "card_number", "2222333344445555" },
                     { "month", "4" },
                     { "year", "2021" },
                     { "holder", "Israel Israelovice" },
                     { "ccv", "262" },
                     { "id", "20444444" }
                    };
            deliveryDetails = new Dictionary<String, Object>
                    {
                     { "name", "Israel Israelovice" },
                     { "address", "Rager Blvd 12" },
                     { "city", "Beer Sheva" },
                     { "country", "Israel" },
                     { "zip", "8458527" }
                    };
            results = new BlockingCollection<bool>();
        }

        internal void ThreadWork(string user_id)
        {
            sut.AddProductToCart(user_id, product_id, 1, store_id);
            bool result = sut.Purchase(user_id, paymentDetails, deliveryDetails).Data == null;
            results.TryAdd(result);
        }

        [Fact]
        [Trait("Category", "concurrency")]
        public void Purchase()
        {

            Thread thread1 = new Thread(() => ThreadWork(kfir_id));
            Thread thread2 = new Thread(() => ThreadWork(igor_id));
            
            thread1.Start();
            thread2.Start();
            
            thread1.Join();
            thread2.Join();
            
            int count = 0;
            foreach (bool result in results)
            {
                if (result)
                    count++;
            }
            Assert.True(count == 1, count + " succeded out of " + results.Count);
        }
    }
}
