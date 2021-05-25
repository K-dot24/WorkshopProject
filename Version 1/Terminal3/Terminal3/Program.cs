using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terminal3.DataAccessLayer;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;

namespace Terminal3
{
    class Program
    {        
        static void Main(string[] args)
        {

















            Thread.Sleep(1000);
            ECommerceSystem a = new ECommerceSystem();
            //HubConnection SignalRClient = new HubConnection("http://localhost:8080/signalr");
            //SignalRClient.CreateHubProxy("NotificationHub");
            //SignalRClient.Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();

        }
    }
}
