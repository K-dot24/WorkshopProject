﻿//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using SignalrServer.Client;
//using SignalrServer.Hubs;
//using SignalrServer.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Terminal3WebAPI.Models;

//namespace Terminal3WebAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SignalRTestController : ControllerBase
//    {
//        private IHubContext<NotificationHub, IClient> _signalrHub;

//        public SignalRTestController()
//        {
//            IHubContext<NotificationHub, IClient> signalrHub
//            _signalrHub = signalrHub;
//        }

//        [HttpPost]
//        public async Task<string> Post([FromBody] Notification msg)
//        {
//            var retMessage = string.Empty;
//            try
//            {
//                await _signalrHub.Clients.All.ReceiveMessage(msg);
//                retMessage = "Success";
//            }
//            catch (Exception e)
//            {
//                retMessage = e.ToString();
//            }
//            return retMessage;
//        }
//    }
//}

