using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Terminal3.DomainLayer;
using System.Text.Json;

namespace Terminal3.ExternalSystems {

    public interface ExternalSystemsAPIInterface
    {

        //static String sourceURL = "https://cs-bgu-wsep.herokuapp.com/";
        string sourceURL { get; set; }

        bool Handshake();
        String Pay(IDictionary<String, Object> paymentDetails);
        String CancelPay(IDictionary<String, Object> paymentDetails);
        String Supply(IDictionary<String, Object> paymentDetails);
        String CancelSupply(IDictionary<String, Object> paymentDetails);
        string HttpClientPost(IDictionary<String, Object> param);
        HttpWebResponse CreatePostHttpResponse(IDictionary<String, Object> parametersJson, Encoding charset);
    }
    public class ExternalSystemsAPI : ExternalSystemsAPIInterface
    {

        public static ExternalSystemsAPIInterface Instance = null;

        public string sourceURL { get ; set; }

        private ExternalSystemsAPI(string sourceURL)
        {
            this.sourceURL = sourceURL;
        }
        public static ExternalSystemsAPIInterface getInstance(String sourceURL="") {
            if(Instance is null)
            {
                if(sourceURL.Equals(""))
                    Instance = new MockExternalSystemApi(sourceURL);
                else
                    Instance = new ExternalSystemsAPI(sourceURL);
            }
            return Instance;
        }



        public bool Handshake()
        {
            var postContent = new Dictionary<String, Object> { { "action_type", "handshake" } };
            String result = HttpClientPost(postContent);
            if (result == "OK")
                return true;
            return false;
        }

        public String Pay(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "pay");
            return HttpClientPost(paymentDetails);
        }

        public String CancelPay(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "cancel_pay");
            return HttpClientPost(paymentDetails);
        }

        public String Supply(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "supply");
            return HttpClientPost(paymentDetails);
        }

        public String CancelSupply(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "cancel_supply");
            return HttpClientPost(paymentDetails);
        }

        public string HttpClientPost(IDictionary<String, Object> param)
        {
            string result;
            Encoding encoding = Encoding.GetEncoding("utf-8");
            HttpWebResponse response = CreatePostHttpResponse(param, encoding);
            if(!(response is null))
            {
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                return result;
            }
            Logger.LogError("Could not connect to external system API");
            return String.Empty;


        }

        public HttpWebResponse CreatePostHttpResponse(IDictionary<String, Object> parametersJson, Encoding charset)
        {
            try
            {
                Dictionary<String, String> parameters = DictionaryFromJson(parametersJson);
                HttpWebRequest request = null;
                request = WebRequest.Create(sourceURL) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = charset.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                return request.GetResponse() as HttpWebResponse;
            }
            catch(WebException e)
            {
                Logger.LogError(e.ToString());
                return null;
            }

        }

        private Dictionary<String, String> DictionaryFromJson(IDictionary<String, Object> parameters)
        {
            Dictionary<String, String> answer = new Dictionary<string, string>();
            foreach(KeyValuePair<String, Object> entry in parameters)
            {
                if (entry.Value is JsonElement)
                    answer.Add(entry.Key, ((JsonElement)entry.Value).ToString());
                else
                    answer.Add(entry.Key, (string)entry.Value);
            }
            return answer;
        }

    }
}
