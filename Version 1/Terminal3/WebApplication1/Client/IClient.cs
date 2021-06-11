using signalRgateway.Model;
using SignalRgateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRgateway.Client
{
    public interface IClient {
        Task ReceiveMessage(String message);
        Task ReceiveMonitor(Record record);

    }
}
