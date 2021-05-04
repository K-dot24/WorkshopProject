using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer;

namespace Terminal3.ServiceLayer
{
    public class Initializer
    {
        ECommerceSystem System;

        public Initializer()
        {
            System = new ECommerceSystem();
            System.DisplaySystem();
        }
        
        public void InsertMockData()
        {
            string DBUserName = "admin";
            string DBPassword = "terminal3";
            string databaseName = "Terminal3-development";
            MongoClient client = new MongoClient($"mongodb+srv://{DBUserName}:{DBPassword}@cluster0.cbdpv.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            var database = client.GetDatabase(databaseName);
        }

    }
}
