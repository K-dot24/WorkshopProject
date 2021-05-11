using Microsoft.AspNetCore.SignalR;
using signalRgateway.Client;
using signalRgateway.Model;
using signalRgateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace signalRgateway.Hubs
{
    public class NotificationHub : Hub<IClient>
    {
        private static ConnectionMapping<string> GuestConnections = new ConnectionMapping<string>(); //userid to connectionid
        private static ConnectionMapping<string> RegisteredConnections = new ConnectionMapping<string>(); //userid to connectionid

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("New connection");
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(Notification message)
        {
            String connectionID = GuestConnections.GetConnections(message.UserID);
            if (!connectionID.Equals(String.Empty))
            {
                await Clients.Client(connectionID).ReceiveMessage(message.Message);
            }
            else if (!RegisteredConnections.GetConnections(message.UserID).Equals(String.Empty))
            {
                connectionID = RegisteredConnections.GetConnections(message.UserID);
                await Clients.Client(connectionID).ReceiveMessage(message.Message);
            }
        }

        public async Task SendBroadcast(Notification message)
        {
            await Clients.All.ReceiveMessage(message.Message);
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
