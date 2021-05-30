using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Terminal3.DomainLayer;

namespace Terminal3.ExternalSystems
{
    public static class ExternalSystemsAPI
    {
        static String sourceURL = "https://cs-bgu-wsep.herokuapp.com/";

        public static bool Handshake()
        {
            var postContent = new Dictionary<String, Object> { { "action_type", "handshake" } };
            String result = HttpClientPost(postContent);
            if (result == "OK")
                return true;
            return false;
        }

        public static String Pay(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "pay");
            return HttpClientPost(paymentDetails);         
        }

        public static String CancelPay(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "cancel_pay");
            return HttpClientPost(paymentDetails);
        }

        public static String Supply(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "supply");
            return HttpClientPost(paymentDetails);
        }

        public static String CancelSupply(IDictionary<String, Object> paymentDetails)
        {
            paymentDetails.Add("action_type", "cancel_supply");
            return HttpClientPost(paymentDetails);
        }

        public static string HttpClientPost(IDictionary<String, Object> param)
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

        public static HttpWebResponse CreatePostHttpResponse(IDictionary<String, Object> parameters, Encoding charset)
        {
            try
            {
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

    }
}
