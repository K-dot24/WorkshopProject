using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTestTerminal3
{
    public class LoginConcurrencyTest : XUnitTerminal3TestCase
    {
        private BlockingCollection<bool> results;
        public LoginConcurrencyTest() : base()
        {
            sut.Register("kfir@gmail.com", "test1");
            results = new BlockingCollection<bool>();
        }

        internal void ThreadWork()
        {
            Terminal3.DomainLayer.Result<String> result = sut.Login("kfir@gmail.com", "test1");
            results.TryAdd(result.ExecStatus);
        }

        [Fact(Skip = "Not relevent")]
        [Trait("Category", "concurrency")]
        public void Login()
        {

            Thread thread1 = new Thread(() => ThreadWork());
            Thread thread2 = new Thread(() => ThreadWork());
            Thread thread3 = new Thread(() => ThreadWork());
            Thread thread4 = new Thread(() => ThreadWork());

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
