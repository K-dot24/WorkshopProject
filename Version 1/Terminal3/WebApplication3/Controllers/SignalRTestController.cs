using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using signalRgateway.Model;
using SignalRgateway.Models;
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
        [Route("Broadcast")]
        [HttpPost]
        public async Task<string> Broadcast([FromBody] string msg)
        {
            var retMessage = string.Empty;
            try
            {
                await connection.InvokeAsync("SendBroadcast",msg);
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
        [Route("Monitor")]
        [HttpPost]
        public async Task<string> sendMonitor([FromBody] MonitorRecordModel record)
        {
            var retMessage = string.Empty;
            try
            {
                await connection.InvokeAsync("sendMonitor", new Record(record.Date,record.GuestUsers,record.RegisteredUsers,record.ManagersNotOwners,record.Owners,record.Admins));
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
    }
}

