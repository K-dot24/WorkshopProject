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
            Console.WriteLine("OnConnected");
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string userID, string message)
        {
            String connectionID = GuestConnections.GetConnections(userID);
            if (!connectionID.Equals(String.Empty))
            {
                await Clients.Client(connectionID).ReceiveMessage(message);
            }
            else if (!RegisteredConnections.GetConnections(userID).Equals(String.Empty))
            {
                connectionID = RegisteredConnections.GetConnections(userID);
                await Clients.Client(connectionID).ReceiveMessage(message);
            }
        }

        public async Task SendBroadcast(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        public async Task Identify(Identifier message)
        {
            GuestConnections.Add(message.UserID, Context.ConnectionId);
            Console.WriteLine($"Identify: UserID:{message.UserID}, ConnectionID:{Context.ConnectionId}");
        }

        public async Task Login(SignalRLoginModel message)
        {
            GuestConnections.Remove(message.oldUserID); //Clearing the old etry
            RegisteredConnections.Add(message.newUserID, Context.ConnectionId);
            Console.WriteLine($"Login: UserID:{message.newUserID}, ConnectionID:{Context.ConnectionId}");
        }
        public async Task Logout(SignalRLoginModel message)
        {
            RegisteredConnections.Remove(message.oldUserID); //Clearing the old etry
            GuestConnections.Add(message.newUserID, Context.ConnectionId);
            Console.WriteLine($"Logout: UserID:{message.newUserID}, ConnectionID:{Context.ConnectionId}");


        }
    }
}
