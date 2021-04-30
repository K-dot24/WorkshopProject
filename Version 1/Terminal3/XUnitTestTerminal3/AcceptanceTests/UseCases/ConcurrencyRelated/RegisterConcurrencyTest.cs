using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTestTerminal3
{
    public class RegisterConcurrencyTest : XUnitTerminal3TestCase
    {

        private BlockingCollection<bool> results;

        public RegisterConcurrencyTest() : base()
        {

            results = new BlockingCollection<bool>();
        }

        internal void ThreadRegister()
        {
            Terminal3.DomainLayer.Result<bool> result = sut.Register("igor@gmail.com", "password");
            results.TryAdd(result.ExecStatus);
        }

        [Fact]
        [Trait("Category", "concurrency")]
        public void Register()
        {

            Thread thread1 = new Thread(() => ThreadRegister());
            Thread thread2 = new Thread(() => ThreadRegister());

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
            Assert.True(count == 1, count + " succeded out of " + results.Count + " (need to be 1)");
        }
    }
}
