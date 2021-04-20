using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTestTerminal3
{
    public class DeletingAndBuyingProductConcurrencyTest : XUnitTerminal3TestCase
    {

        private string owner_id;
        private string igor_id;
        private string kfir_id;
        private string store_id;
        private string product_id;

        private LinkedList<bool> results;

        public DeletingAndBuyingProductConcurrencyTest() : base()
        {
            sut.Register("owner@gmail.com", "test1");
            sut.Register("igor@gmail.com", "test1");
            sut.Register("kfir@gmail.com", "test1");
            this.igor_id = sut.Login("igor@gmail.com", "test1").Data;
            this.kfir_id = sut.Login("kfir@gmail.com", "test1").Data;
            this.store_id = sut.OpenNewStore("test_store", owner_id).Data;
            this.product_id = sut.AddProductToStore(owner_id, store_id, "test_product", 10, 1, "test").Data;
        }

        internal void ThreadOwner()
        {
            results.AddLast(sut.RemoveProductFromStore(owner_id, store_id, product_id).ExecStatus);
        }

        internal void ThreadBuyer(string user_id)
        {
            results.AddLast(sut.AddProductToCart(user_id, product_id, 1, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "concurrency")]
        public void DeleteAndBuy()
        {
            Thread threadOwner = new Thread(() => ThreadOwner());
            Thread threadBuyer1 = new Thread(() => ThreadBuyer(igor_id));
            Thread threadBuyer2 = new Thread(() => ThreadBuyer(kfir_id));

            threadOwner.Start();
            threadBuyer1.Start();
            threadBuyer2.Start();

            threadOwner.Join();
            threadBuyer1.Join();
            threadBuyer2.Join();

            int count = 0;
            foreach (bool result in results)
            {
                if (result)
                    count++;
            }
            Assert.True(count == 1);
        }

    }
}
