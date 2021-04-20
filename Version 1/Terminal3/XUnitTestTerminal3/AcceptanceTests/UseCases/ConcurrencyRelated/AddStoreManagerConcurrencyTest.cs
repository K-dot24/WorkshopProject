using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTestTerminal3
{
    public class AddStoreManagerConcurrencyTest : XUnitTerminal3TestCase
    {
        private string kfir_id;
        private string igor_id;
        private string random_id;
        private string store_id;
        private LinkedList<bool> results;
        public AddStoreManagerConcurrencyTest() : base()
        {
            sut.Register("kfir@gmail.com", "test1");
            sut.Register("igor@gmail.com", "test12");
            sut.Register("random@gmail.com", "test123");
            this.kfir_id = sut.Login("kfir@gmail.com", "test1").Data;
            this.igor_id = sut.Login("igor@gmail.com", "test12").Data;
            this.random_id = sut.Login("random@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", kfir_id).Data;
            sut.AddStoreManager(igor_id, kfir_id, store_id);
            LinkedList<int> permission = new LinkedList<int>();
            permission.AddLast(4);
            sut.SetPermissions(store_id, igor_id, kfir_id, permission);
            results = new LinkedList<bool>();

        }

        internal void ThreadWork(string manager_id)
        {
            results.AddLast(sut.AddStoreManager(random_id, manager_id, store_id).ExecStatus);
        }

        [Fact]
        [Trait("Category", "concurrency")]
        public void AddStoreManager()
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
            Assert.True(count == 1, count+" succeded out of "+results.Count);
        }
    }

}
