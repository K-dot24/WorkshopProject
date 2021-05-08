using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3WebAPI.Hubs;
using Terminal3WebAPI.Hubs.Client;

namespace Terminal3WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRTestController : ControllerBase
    {
        private IHubContext<NotificationHub, IClient> _signalrHub;

        public SignalRTestController(IHubContext<NotificationHub, IClient> signalrHub)
        {
            _signalrHub = signalrHub;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] Notification msg)
        {
            var retMessage = string.Empty;
            try
            {
                await _signalrHub.Clients.All.RecieveNotification(msg);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
    }
}

