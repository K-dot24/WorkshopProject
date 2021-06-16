using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTestTerminal3
{
    public class AddStoreOwnerConcurrencyTest : XUnitTerminal3TestCase
    {
        private string kfir_id;
        private string igor_id;
        private string hit_id;
        private string shlomot_id;
        private string random_id;
        private string store_id;
        private BlockingCollection<bool> results;
        public AddStoreOwnerConcurrencyTest() : base()
        {
            sut.Register("kfir@gmail.com", "test1");
            sut.Register("igor@gmail.com", "test12");
            sut.Register("hit@gmail.com", "navtut");
            sut.Register("shlomot@gmail.com", "test");
            sut.Register("random@gmail.com", "test123");
            this.kfir_id = sut.Login("kfir@gmail.com", "test1").Data;
            this.igor_id = sut.Login("igor@gmail.com", "test12").Data;
            this.hit_id = sut.Login("hit@gmail.com", "navtut").Data;
            this.shlomot_id = sut.Login("shlomot@gmail.com", "test").Data;
            this.random_id = sut.Login("random@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", kfir_id).Data;
            sut.AddStoreOwner("igor@gmail.com", kfir_id, store_id);
            sut.AddStoreOwner("hit@gmail.com", kfir_id, store_id);
            sut.AddStoreOwner("shlomot@gmail.com", kfir_id, store_id);
            results = new BlockingCollection<bool>();
        }

        internal void ThreadWork(string owner_id)
        {
            Terminal3.DomainLayer.Result<bool> result = sut.AddStoreOwner("random@gmail.com", owner_id, store_id);
            results.TryAdd(result.ExecStatus);
        }

        [Fact(Skip = "Not relevent")]
        [Trait("Category", "concurrency")]
        public void AddStoreOwner()
        {
            Thread thread1 = new Thread(() => ThreadWork(kfir_id));
            Thread thread2 = new Thread(() => ThreadWork(igor_id));
            Thread thread3 = new Thread(() => ThreadWork(hit_id));
            Thread thread4 = new Thread(() => ThreadWork(shlomot_id));

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

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
