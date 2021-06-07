using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Terminal3.ExternalSystems
{
    public class MockExternalSystemApi : ExternalSystemsAPIInterface
    {
        public string sourceURL { get; set; }

        private Random random { get; }

        public MockExternalSystemApi(string sourceURL)
        {
            this.sourceURL = sourceURL;
            this.random = new Random();
        }

        public string CancelPay(IDictionary<string, object> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("transaction_id"))
                return "" + -1;
            return "" + generateId();
        }

        public string CancelSupply(IDictionary<string, object> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("transaction_id"))
                return "" + -1;
            return "" + generateId();
        }

        public HttpWebResponse CreatePostHttpResponse(IDictionary<string, object> parametersJson, Encoding charset)
        {
            return null;
        }

        public bool Handshake()
        {
            return true;
        }

        public string HttpClientPost(IDictionary<string, object> param)
        {
            return "";
        }

        public string Pay(IDictionary<string, object> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("card_number"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("month"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("year"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("holder"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("ccv"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("id"))
                return "" + -1;
            return "" + generateId();
        }

        public string Supply(IDictionary<string, object> paymentDetails)
        {

            if (!paymentDetails.ContainsKey("name"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("address"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("city"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("country"))
                return "" + -1;
            if (!paymentDetails.ContainsKey("zip"))
                return "" + -1;
            return "" + generateId();
        }

        private int generateId()
        {
            int minId = 10000;
            int maxId = 100000;
            int id = random.Next(maxId - minId) + minId;
            return id;
        }
    }
}
