using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3WebAPI.Hubs.Client;

namespace Terminal3WebAPI.Hubs
{
    public class NotificationHub : Hub<IClient>
    {
        public async Task SendMessage(Notification message)
        {
            await Clients.All.RecieveNotification(message);
        }
    }
}
