using SignalrServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalrServer.Client
{
    public interface IClient {
        Task ReceiveMessage(String message);

    }
}
