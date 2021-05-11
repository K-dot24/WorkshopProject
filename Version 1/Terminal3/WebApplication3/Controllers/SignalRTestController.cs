using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using signalRgateway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.ServiceLayer;
using Terminal3WebAPI.Models;

namespace Terminal3WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRTestController : ControllerBase
    {
        //public IHubProxy hubProxy { get; set; }
        public HubConnection connection { get; set; }
        private readonly IECommerceSystem system;


        public SignalRTestController(IECommerceSystem system)
        {
            //Setting up SignalR connection
            this.system = system;

            //HubConnection SignalRClient = new HubConnection("http://localhost:8080/signalr");
            //hubProxy = SignalRClient.CreateHubProxy("NotificationHub");
            //SignalRClient.Start();
            //while (!(SignalRClient.State == ConnectionState.Connected)) { }

            string url = "https://localhost:4001/signalr/notification";
            connection = new HubConnectionBuilder()
               .WithUrl(url)
               .WithAutomaticReconnect()
               .Build();
            connection.StartAsync();
            while (connection.State != HubConnectionState.Connected) { }

        }

        [HttpPost]
        public async Task<string> Post([FromBody] Notification msg)
        {
            var retMessage = string.Empty;
            try
            {
                //hubProxy.Invoke("SendBroadcast",msg);
                await connection.InvokeAsync("SendBroadcast",msg);
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
    }
}

