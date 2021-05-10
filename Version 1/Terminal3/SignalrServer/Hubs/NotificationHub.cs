using Microsoft.AspNet.SignalR;
using SignalrServer.Client;
using SignalrServer.Model;
using SignalrServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SignalrServer.Hubs
{
    public class NotificationHub : Hub<IClient>
    {
        private static ConnectionMapping<string> GuestConnections = new ConnectionMapping<string>(); //userid to connectionid
        private static ConnectionMapping<string> RegisteredConnections = new ConnectionMapping<string>(); //userid to connectionid

        public override Task OnConnected()
        {
            Console.WriteLine("New connection");
            return base.OnConnected();
        }
        public async Task SendMessage(Notification message)
        {
            String connectionID = GuestConnections.GetConnections(message.UserID);
            if (!connectionID.Equals(String.Empty))
            {
                await Clients.Client(connectionID).ReceiveMessage(message);
            }
            else if (!RegisteredConnections.GetConnections(message.UserID).Equals(String.Empty))
            {
                connectionID = RegisteredConnections.GetConnections(message.UserID);
                await Clients.Client(connectionID).ReceiveMessage(message);
            }
        }
       
        public async Task Identify(Identifier message)
        {
            GuestConnections.Add(message.UserID, Context.ConnectionId);
            Console.WriteLine($"New Connection: UserID:{message.UserID}, ConnectionID:{Context.ConnectionId}");
        }

        public async Task Login(SignalRLoginModel message)
        {
            GuestConnections.Remove(message.oldUserID); //Clearing the old etry
            RegisteredConnections.Add(message.newUserID, Context.ConnectionId);
            Console.WriteLine($"New Connection: UserID:{message.newUserID}, ConnectionID:{Context.ConnectionId}");
        }
        public async Task Logout(SignalRLoginModel message)
        {
            RegisteredConnections.Remove(message.oldUserID); //Clearing the old etry
            GuestConnections.Add(message.newUserID, Context.ConnectionId);
   
        }
    }
}
