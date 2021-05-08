using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3WebAPI.Hubs.Client;
using Terminal3WebAPI.Models;

namespace Terminal3WebAPI.Hubs
{
    public class NotificationHub : Hub<IClient>
    {
        private static ConnectionMapping<string> _connections = new ConnectionMapping<string>(); //userid to connectionid

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            Console.WriteLine($"OnConnectedAsync name:{name}");

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
