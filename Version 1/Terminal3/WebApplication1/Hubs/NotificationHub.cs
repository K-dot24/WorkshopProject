using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using signalRgateway.Hubs.Client;
using SignalRgateway.Model;
using signalRgateway.Models;

namespace signalRgateway.Hubs
{
    public class NotificationHub : Hub<IClient>
    {
        private static ConnectionMapping<string> GuestConnections = new ConnectionMapping<string>(); //userid to connectionid
        private static ConnectionMapping<string> RegisteredConnections = new ConnectionMapping<string>(); //userid to connectionid

        public async Task SendMessage(Notification message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        public async Task Identify(Identifier message)
        {
            GuestConnections.Add(message.UserID, Context.ConnectionId);
            Console.WriteLine($"New Connection: UserID:{message.UserID}, ConnectionID:{Context.ConnectionId}");
            //m.ClientId = message.UserID;
            //m.Message = $"User Connection ID: {Context.ConnectionId}";
            //await Clients.Client(Context.ConnectionId).ReceiveMessage(m);
        }
        public async Task Login(SignalRLoginModel message)
        {
            GuestConnections.Remove(message.oldUserID); //Clearing the old etry
            RegisteredConnections.Add(message.newUserID, Context.ConnectionId);
            Console.WriteLine($"New Connection: UserID:{message.newUserID}, ConnectionID:{Context.ConnectionId}");
            //m.ClientId = message.newUserID;
            //m.Message = $"User Connection ID: {Context.ConnectionId}";
            //await Clients.Client(Context.ConnectionId).ReceiveMessage(m);
        }
        public async Task Logout(SignalRLoginModel message)
        {
            RegisteredConnections.Remove(message.oldUserID); //Clearing the old etry
            GuestConnections.Add(message.newUserID, Context.ConnectionId);
            Console.WriteLine($"New Connection: UserID:{message.newUserID}, ConnectionID:{Context.ConnectionId}");
            Notification m = new Notification("asd","asdasd");
            m.UserID = message.newUserID;
            m.Message = $"User Connection ID: {Context.ConnectionId}";
            await Clients.Client(Context.ConnectionId).ReceiveMessage(m);
        }
    }
}
