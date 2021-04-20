using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestTerminal3
{
    public class PurchaseTest: XUnitTerminal3TestCase
    {
        private string user_id;
        private string store_id;
        private string product_id;
        public PurchaseTest() : base()
        {
            sut.Register("test@gmail.com", "test123");
            this.user_id = sut.Login("test@gmail.com", "test123").Data;
            this.store_id = sut.OpenNewStore("test_store", user_id).Data;
            this.product_id = sut.AddProductToStore(user_id, store_id, "test_product", 10, 10, "test").Data;

        }

    }
}
