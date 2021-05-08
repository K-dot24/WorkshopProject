using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3WebAPI.Models;

namespace Terminal3WebAPI.Hubs.Client
{
    public interface IClient {
        Task ReceiveMessage(ChatMessage message);

    }
}
