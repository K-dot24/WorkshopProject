using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
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
